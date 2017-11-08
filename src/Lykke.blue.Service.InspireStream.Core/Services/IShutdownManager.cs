using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
