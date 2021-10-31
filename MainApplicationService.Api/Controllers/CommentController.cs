using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Api.Dtos;
using MainApplicationService.Api.Mappers;
using MainApplicationService.Api.StaticMappers;
using MainApplicationService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Exceptions;

namespace MainApplicationService.Api.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentsMapper _commentsMapper;
        private readonly ICommentsService _commentsService;
        private readonly ICommentsRepository _commentsRepository;
        private readonly IEntityBaseRepository _entityBaseRepository;

        public CommentController(ICommentsMapper commentsMapper, ICommentsRepository commentsRepository,
            ICommentsService commentsService, IEntityBaseRepository entityBaseRepository)
        {
            _commentsMapper = commentsMapper;
            _commentsRepository = commentsRepository;
            _commentsService = commentsService;
            _entityBaseRepository = entityBaseRepository;
        }

        /// <summary>
        /// Get a comment by the unique id
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("comments/{commentId}", Name = "GetCommentById")]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetByIdAsync(string commentId, CancellationToken cancellationToken = default)
        {
            var permissionResult = _commentsService.CheckGetPermissions(commentId);
            if (!permissionResult.IsSuccess)
                return StatusCode((int) HttpStatusCode.Forbidden, permissionResult.ValidationErrors);

            var comment = await _commentsRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null)
                return NotFound($"Comment with id {commentId} not found.");
            await _commentsRepository.SaveChangesAsync(cancellationToken);
            return Ok(comment.ToCommentDto());
        }

        /// <summary>
        /// Get the list of comments for a specific entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="onlyNew"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("entity/{entityId}/comments")]
        [ProducesResponseType(typeof(CommentsListDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetListAsync(string entityId, int skip = 0, int take = 50, bool onlyNew = false, CancellationToken cancellationToken = default)
        {
            var parentEntity = await _entityBaseRepository.GetByIdAsync(entityId, cancellationToken);
            if (parentEntity == null)
                return NotFound($"Parent entity with id {entityId} not found.");

            var permissionResult = _commentsService.CheckGetListPermissions(entityId);
            if (!permissionResult.IsSuccess)
                return StatusCode((int)HttpStatusCode.Forbidden, permissionResult.ValidationErrors);

            var comments = await _commentsRepository.GetListByParentAsync(entityId, skip, take, onlyNew, cancellationToken);
            return Ok(new CommentsListDto()
            {
                Results = comments.Select(c => c.ToCommentDto())
            });
        }

        /// <summary>
        /// Create a new comment for a specific entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="postModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("entity/{entityId}/comments")]
        [ProducesResponseType(typeof(CommentDto), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAsync(string entityId, CommentPostModel postModel, CancellationToken cancellationToken = default)
        {
            var parentEntity = await _entityBaseRepository.GetByIdAsync(entityId, cancellationToken);
            if (parentEntity == null)
                return NotFound($"Parent entity with id {entityId} not found.");

            var permissionResult = _commentsService.CheckCreatePermissions();
            if (!permissionResult.IsSuccess)
                return StatusCode((int)HttpStatusCode.Forbidden, permissionResult.ValidationErrors);

            var comment = _commentsMapper.Create(entityId, postModel);
            var serviceResult = await _commentsService.CreateAsync(comment, cancellationToken);
            if (!serviceResult.IsSuccess)
            {
                return BadRequest(serviceResult.ValidationErrors);
            }
            await _commentsRepository.SaveChangesAsync(cancellationToken);
            return Created(Url.Link("GetCommentById", new { commentId = comment.Id }), serviceResult.ResultObject?.ToCommentDto());
        }

        /// <summary>
        /// Update a comment by unique id
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="putModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("comments/{commentId}")]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateAsync(string commentId, CommentPutModel putModel, CancellationToken cancellationToken = default)
        {
            var permissionResult = _commentsService.CheckUpdatePermissions();
            if (!permissionResult.IsSuccess)
                return StatusCode((int)HttpStatusCode.Forbidden, permissionResult.ValidationErrors);

            var comment = await _commentsRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null)
                return NotFound($"Comment with id {commentId} not found.");
            
            comment = _commentsMapper.Update(comment, putModel);
            var serviceResult = await _commentsService.UpdateAsync(comment, cancellationToken);
            if (!serviceResult.IsSuccess)
                return BadRequest(serviceResult.ValidationErrors);
            
            try
            {
                await _commentsRepository.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyException)
            {
                return Conflict("The update could not be completed due to a conflict.");
            }
            return Ok(serviceResult.ResultObject?.ToCommentDto());
        }

        /// <summary>
        /// Delete a comment by unique id
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("comments/{commentId}")]
        [ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IDictionary<string, List<string>>), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteAsync(string commentId, CancellationToken cancellationToken = default)
        {
            var permissionResult = _commentsService.CheckDeletePermissions(commentId);
            if (!permissionResult.IsSuccess)
                return StatusCode((int)HttpStatusCode.Forbidden, permissionResult.ValidationErrors);

            var comment = await _commentsRepository.GetByIdAsync(commentId, cancellationToken);
            if (comment == null)
                return NotFound($"Comment with id {commentId} not found.");
            
            var serviceResult = _commentsService.Delete(comment);
            if (!serviceResult.IsSuccess)
                return BadRequest(serviceResult.ValidationErrors);
           
            try
            {
                await _commentsRepository.SaveChangesAsync(cancellationToken);
            }
            catch (ConcurrencyException)
            {
                return Conflict("The update could not be completed due to a conflict.");
            }
            return Ok(commentId);
        }
    }
}
