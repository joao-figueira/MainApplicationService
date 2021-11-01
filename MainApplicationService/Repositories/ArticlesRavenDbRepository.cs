using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Interfaces;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;

namespace MainApplicationService.Repositories
{
    public class ArticlesRavenDbRepository : IArticlesRepository
    {
        private readonly IAsyncDocumentSession _asyncSession;

        public ArticlesRavenDbRepository(IAsyncDocumentSession asyncSession)
        {
            _asyncSession = asyncSession;
        }

        public async Task<(IEnumerable<Article> Results, int TotalCount)> GetListAsync(int skip, int take, CancellationToken cancellationToken = default)
        {
            var results = await _asyncSession
                .Query<Article>()
                .Statistics(out var stats)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);

            var totalCount = stats.TotalResults;

            return (results, totalCount);
        }

        public async Task<Article?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _asyncSession
                .Query<Article>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Article> CreateAsync(Article article, CancellationToken cancellationToken = default)
        {
            await _asyncSession.StoreAsync(article, cancellationToken);
            return article;
        }

        public Task<Article> UpdateAsync(Article genericObject, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public string? Delete(Article genericObject, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _asyncSession.SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (ConcurrencyException)
            {
                return false;
            }
        }
    }
}
