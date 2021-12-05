using Super.Paula.Application.Administration.Requests;
using Super.Paula.Application.Inventory.Requests;
using Super.Paula.Application.Inventory.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Super.Paula.Application.Inventory
{
    public interface IBusinessObjectEventHandler : 
        IEventHandler<InspectionEvent>
    {
    }
}