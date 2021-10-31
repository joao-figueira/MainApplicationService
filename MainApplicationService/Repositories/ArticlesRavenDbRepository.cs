using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Interfaces;
using Raven.Client.Documents.Session;

namespace MainApplicationService.Repositories
{
    public class ArticlesRavenDbRepository : IArticlesRepository
    {
        private readonly IAsyncDocumentSession _asyncSession;

        public ArticlesRavenDbRepository(IAsyncDocumentSession asyncSession)
        {
            _asyncSession = asyncSession;
        }

        public async Task<Article?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _asyncSession.LoadAsync<Article?>(id, cancellationToken);
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

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _asyncSession.SaveChangesAsync(cancellationToken);
        }
    }
}
