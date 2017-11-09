using System.Collections.Generic;
using System.Threading.Tasks;
namespace Lykke.blue.Service.InspireStream.Core.Twitter
{
    public interface ITweetsCashRepository
    {
        Task<string> RegisterAsync(ITweetCash tweetCash);
        Task CreateAsync(ITweetCash tweetCash);
        Task<IEnumerable<ITweetCash>> GetAsync(string accountId);
        Task CreateOrUpdateAsync(IEnumerable<ITweetCash> tweetCash);
    }
}
