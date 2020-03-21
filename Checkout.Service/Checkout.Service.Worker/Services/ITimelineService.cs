using System.Threading.Tasks;
using Checkout.Service.Worker.Models;

namespace Checkout.Service.Worker.Services
{
    public interface ITimelineService
    {
        Task PublishTimelineOrderEvent(TimelineOrderEvent timelineEvent);
    }
}