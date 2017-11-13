using Common.Log;
using Lykke.blue.Service.InspireStream.Core;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using Lykke.blue.Service.InspireStream.Models.Tweets;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Lykke.blue.Service.InspireStream.Controllers
{
    [Route("api/tweetCash")]
    public class TweetCashController : Controller
    {
        private readonly ITweetsCashRepository _tweetCashRepository;
        private readonly ITwitterAppAccountRepository _twitterAppAccountRepository;
        private readonly TwitterSettings _twitterSettings;
        private readonly ILog _log;

        public TweetCashController(ITweetsCashRepository tweetCashRepository,
                                   ITwitterAppAccountRepository twitterAppAccountRepository,
                                   TwitterSettings twitterSettings,
                                   ILog log)
        {
            _tweetCashRepository = tweetCashRepository;
            _twitterAppAccountRepository = twitterAppAccountRepository;
            _twitterSettings = twitterSettings;
            _log = log;
        }
        DateTime lastUpdatetweets = DateTime.Now.AddMinutes(-15);

        [HttpGet()]
        [SwaggerOperation("GetTweets")]
        [ProducesResponseType(typeof(IEnumerable<TweetsResponseModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTweetsByAdditionalParameters([FromBody]TweetsSearchModel model)
        {
            var twitterAppAccount = await _twitterAppAccountRepository.GetAsync(model.AccountEmail);
            if (twitterAppAccount == null)
            {
                return NotFound("Account not found");
            }

            SearchTweetsParameters searchParameters;

            if (model.IsExtendedSearch)
            {
                searchParameters = new SearchTweetsParameters(model.SearchQuery)
                {
                    MaximumNumberOfResults = model.MaxResult,
                    Until = model.UntilDate,
                    Lang = LanguageFilter.English,
                };
            }
            else
            {
                searchParameters = new SearchTweetsParameters(model.SearchQuery)
                {
                    Lang = LanguageFilter.English,
                };
            }

            List<TweetsResponseModel> tweetsToShow = new List<TweetsResponseModel>();
            TimeSpan defaultMinutes = TimeSpan.FromMinutes(_twitterSettings.DefaultMinutesToCheck);

            if (DateTime.UtcNow - twitterAppAccount.LastSyncDate.ToUniversalTime() > defaultMinutes)
            {
                var tweets = TweetsManager.GetTweetsByQuery(searchParameters, twitterAppAccount);
                tweetsToShow.AddRange(tweets?.Select(t => TweetsResponseModel.Create(t)));

                if (tweetsToShow.Count > 0)
                {
                    twitterAppAccount.LastSyncDate = DateTime.UtcNow;

                    await _twitterAppAccountRepository.UpdateAsync(twitterAppAccount);
                    await TweetsManager.SaveTweetsCash(_tweetCashRepository, tweets);
                }
                else
                {
                    return NotFound("No Tweets Found");
                }
            }
            else
            {
                tweetsToShow.AddRange((await TweetsManager.GetTweetsCash(_tweetCashRepository, twitterAppAccount.Id))
                                                          .Select(t => TweetsResponseModel.Create(t)));
            }

            if (model.PageNumber > 0 && model.PageSize > 0)
                return Ok(tweetsToShow.OrderByDescending(t => t.Date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize));

            return Ok(tweetsToShow.OrderByDescending(t => t.Date));
        }
    }
}
