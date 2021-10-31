using System.Threading;
using System.Threading.Tasks;
using MainApplicationService.Entities;
using MainApplicationService.Interfaces;
using Raven.Client.Documents.Session;

namespace MainApplicationService.Repositories
{
    public class EntityBaseRavenDbRepository : IEntityBaseRepository
    {
        private readonly IAsyncDocumentSession _asyncSession;

        public EntityBaseRavenDbRepository(IAsyncDocumentSession asyncSession)
        {
            _asyncSession = asyncSession;
        }

        public async Task<EntityBase?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _asyncSession.LoadAsync<EntityBase?>(id, cancellationToken);
        }
    }
}
