using System.Threading;
using System.Threading.Tasks;
namespace CarpoolPlanner.NotificationService
{
    public interface ISmsClient
    {
        Task<bool> SendMessage(string to, string message, CancellationToken token);
    }
}
