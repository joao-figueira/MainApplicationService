using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Interfaces;
using MainApplicationService.Providers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace MainApplicationService.Repositories
{
    public class CommentsRavenDbRepository : ICommentsRepository
    {
        private readonly IAsyncDocumentSession _asyncSession;
        private readonly CurrentUserProvider _currentUserProvider;

        public CommentsRavenDbRepository(IAsyncDocumentSession asyncSession, CurrentUserProvider currentUserProvider)
        {
            _asyncSession = asyncSession;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<Comment?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var comment = await _asyncSession.LoadAsync<Comment?>(id, cancellationToken);
            if (comment != null)
            {
                await _asyncSession.StoreAsync(new CommentReadLog()
                {
                    Id = string.Empty,
                    ParentEntityId = comment.ParentId,
                    CommentId = comment.Id,
                    UserId = _currentUserProvider.CurrentUser?.Id,
                    TimestampUtc = DateTime.UtcNow
                }, cancellationToken);
            }
            return comment;
        }

        //ToDo: when getting only new comments, this method is making 2 round trips. Change the queries to avoid it.
        public async Task<(IEnumerable<Comment> Results, int TotalCount)> GetListByParentAsync(string parentEntityId, int skip, int take, bool onlyNew = false, CancellationToken cancellationToken = default)
        {
            List<Comment> results;
            QueryStatistics stats;
            if (onlyNew && !string.IsNullOrEmpty(_currentUserProvider.CurrentUser?.Id))
            {
                var entityCommentsReadByUser = await _asyncSession
                    .Query<CommentReadLog>()
                    .Where(l => l.UserId == _currentUserProvider.CurrentUser.Id && l.ParentEntityId == parentEntityId)
                    .ToListAsync(cancellationToken);

                results = await _asyncSession
                    .Advanced
                    .AsyncDocumentQuery<Comment>()
                    .Statistics(out stats)
                    .WhereEquals("ParentId", parentEntityId)
                    .AndAlso()
                    .Not
                    .WhereIn("Id", entityCommentsReadByUser.Select(c => c.CommentId))
                    .OrderByDescending(x => x.CreatedOnUtc)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                results = await _asyncSession
                    .Query<Comment>()
                    .Statistics(out stats)
                    .Where(x => x.ParentId == parentEntityId)
                    .OrderByDescending(x => x.CreatedOnUtc)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }
            return (results, stats.TotalResults);
        }

        public async Task<Comment> CreateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            comment.Id = string.Empty; // RavenDB automatically generates a GUID
            await _asyncSession.StoreAsync(comment, cancellationToken);
            return comment;
        }

        /**
         * Left on purpose to implement the interface. In this case, using RavenDB doesn't make a lot of sense
         * but changing the implementation (using another DB), it would be useful and avoid a lot of changes around the app.
         */
        public async Task<Comment> UpdateAsync(Comment comment, CancellationToken cancellationToken = default)
        {
            return comment;
        }

        public string? Delete(Comment comment, CancellationToken cancellationToken = default)
        {
            _asyncSession.Delete(comment);
            return comment.Id;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _asyncSession.SaveChangesAsync(cancellationToken);
        }
    }
}
