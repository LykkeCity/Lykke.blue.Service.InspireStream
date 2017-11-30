using Lykke.blue.Service.InspireStream.Core.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Parameters;

namespace Lykke.blue.Service.InspireStream.Abstractions
{
    public interface ITweetsManager
    {
        Task SaveTweetsCash(ITweetsCashRepository tweetCashRepository, IEnumerable<ITweetCash> tweets);
        Task<List<ITweetCash>> GetTweetsCash(ITweetsCashRepository tweetCashRepository, string accountId);
        IEnumerable<ITweetCash> GetTweetsByQuery(SearchTweetsParameters searchParameters, ITwitterAppAccount account);
    }
}
