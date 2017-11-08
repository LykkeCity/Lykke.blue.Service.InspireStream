
using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.Client
{
    public interface IInspireStreamClient
    {
        Task<IEnumerable<TweetsResponseModel>> GetAsync(TweetsSearchModel model);
    }
}