using Super.Paula.Application.Administration.Continuation;
using Super.Paula.Application.Orchestration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Super.Paula.Application.Administration
{
    public class InspectorContinuationService : IInspectorContinuationService
    {
        private readonly IContinuationStorage _continuationStorage;
        private readonly ClaimsPrincipal _user;

        public InspectorContinuationService(
            IContinuationStorage continuationStorage, 
            ClaimsPrincipal user)
        {
            _continuationStorage = continuationStorage;
            _user = user;
        }

        public async ValueTask AddCreateIdentityInspectorContinuationAsync(Inspector entity)
        {
            var continuation = new CreateIdentityInspectorContinuation(
                entity.Organization,
                entity.Identity,
                entity.UniqueName,
                entity.Activated && entity.OrganizationActivated);
            
            await _continuationStorage.AddAsync(continuation, _user);
        }

        public ValueTask AddDeleteIdentityInspectorContinuationAsync(Inspector entity)
            => AddDeleteIdentityInspectorContinuationAsync(entity.Identity, entity.Organization, entity.UniqueName);

        public async ValueTask AddDeleteIdentityInspectorContinuationAsync(string uniqueName, string organization, string inspector)
        {
            var continuation = new DeleteIdentityInspectorContinuation(
                organization,
                uniqueName,
                inspector);

            await _continuationStorage.AddAsync(continuation, _user);
        }

        public async ValueTask AddActivateIdentityInspectorContinuationAsync(Inspector entity)
        {
            var continuation = new ActivateIdentityInspectorContinuation(
                entity.Organization,
                entity.Identity,
                entity.UniqueName);

            await _continuationStorage.AddAsync(continuation, _user);
        }

        public async ValueTask AddDeactivateIdentityInspectorContinuationAsync(Inspector entity)
        {
            var continuation = new DeactivateIdentityInspectorContinuation(
                entity.Organization,
                entity.Identity,
                entity.UniqueName);

            await _continuationStorage.AddAsync(continuation, _user);
        }
    }
}