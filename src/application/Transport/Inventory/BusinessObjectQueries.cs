using System.Linq;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjectQueries : IBusinessObjectQueries
    {
        private readonly IBusinessObjectManager _businessObjectManager;

        public BusinessObjectQueries(IBusinessObjectManager businessObjectManager)
        {
            _businessObjectManager = businessObjectManager;
        }

        public string[] GetAllUniqueNames()
            => _businessObjectManager.GetQueryable()
                .Select(x => x.UniqueName)
                .ToArray();
    }
}
