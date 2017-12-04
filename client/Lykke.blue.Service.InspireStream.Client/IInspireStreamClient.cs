
using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.Client
{
    public interface IInspireStreamClient
    {
        /// <summary>
        /// Get new tweets or get tweets from the cash
        /// </summary>
        /// <param name='model'>
        /// Tweets Search Model by which we search for tweets.
        /// </param>
        Task<IEnumerable<TweetsResponseModel>> GetAsync(TweetsSearchModel model);

        /// <summary>
        /// Create new twitter application account
        /// </summary>
        /// <param name="model">
        /// Twitter app account rquest model by by which w e create the twitter application account
        /// </param>
        Task CreateTweeterAccountAsync(TwitterAppAccountRquestModel model);
    }
}