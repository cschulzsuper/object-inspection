using System.Linq;

namespace Super.Paula.Application.Inventory
{
    public class BusinessObjects : IBusinessObjects
    {
        private readonly IBusinessObjectManager _businessObjectManager;

        public BusinessObjects(IBusinessObjectManager businessObjectManager)
        {
            _businessObjectManager = businessObjectManager;
        }

        public string[] GetAllUniqueNames()
            => _businessObjectManager.GetQueryable()
                .Select(x => x.UniqueName)
                .ToArray();
    }
}
