using Common.Log;
using Lykke.blue.Service.InspireStream.Abstractions;
using Lykke.blue.Service.InspireStream.AzureRepositories.Twitter;
using Lykke.blue.Service.InspireStream.Core;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using Lykke.blue.Service.InspireStream.Models.Tweets;
using Lykke.blue.Service.InspireStream.Models.TwitterAppAccount;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tweetinvi;
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
        private readonly ITweetsManager _tweetsManager;

        public TweetCashController(ITweetsCashRepository tweetCashRepository,
                                   ITwitterAppAccountRepository twitterAppAccountRepository,
                                   TwitterSettings twitterSettings,
                                   ILog log, ITweetsManager tweetsManager)
        {
            _tweetCashRepository = tweetCashRepository;
            _twitterAppAccountRepository = twitterAppAccountRepository;
            _twitterSettings = twitterSettings;
            _log = log;
            _tweetsManager = tweetsManager;
        }
        DateTime lastUpdatetweets = DateTime.Now.AddMinutes(-15);

        [HttpPost]
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

            List<TweetsResponseModel> tweetsToShow = new List<TweetsResponseModel>();
            TimeSpan defaultMinutes = TimeSpan.FromMinutes(_twitterSettings.DefaultMinutesToCheck);

            if (DateTime.UtcNow - twitterAppAccount.LastSyncDate.ToUniversalTime() > defaultMinutes)
            {
                TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;

                SearchTweetsParameters searchParameters;

                if (model.IsExtendedSearch)
                {
                    searchParameters = new SearchTweetsParameters(model.SearchQuery)
                    {
                        MaximumNumberOfResults = model.MaxResult,
                        Until = model.UntilDate,
                        Lang = LanguageFilter.English
                    };
                }
                else
                {
                    searchParameters = new SearchTweetsParameters(model.SearchQuery)
                    {
                        Lang = LanguageFilter.English
                    };
                }

                var tweets = _tweetsManager.GetTweetsByQuery(searchParameters, twitterAppAccount);

                tweetsToShow.AddRange(tweets?.Select(t => TweetsResponseModel.Create(t)));

                if (tweetsToShow.Count > 0)
                {
                    twitterAppAccount.LastSyncDate = DateTime.UtcNow;
                    await _twitterAppAccountRepository.UpdateAsync(twitterAppAccount);
                    await _tweetsManager.SaveTweetsCash(_tweetCashRepository, tweets);
                }
                else
                {
                    return NotFound("No Tweets Found");
                }
            }
            else
            {
                tweetsToShow.AddRange((await _tweetsManager.GetTweetsCash(_tweetCashRepository, twitterAppAccount.Id))
                                                          .Select(t => TweetsResponseModel.Create(t)));
            }

            if (model.PageNumber > 0 && model.PageSize > 0)
                return Ok(tweetsToShow.OrderByDescending(t => t.Date).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize));

            return Ok(tweetsToShow.OrderByDescending(t => t.Date));
        }


        [HttpPut()]
        [SwaggerOperation("CreateTweetAccount")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task CreatAccount([FromBody]TwitterAppAccountRquestModel model)
        {
            await _twitterAppAccountRepository.RegisterAsync(new TwitterAppAccount()
            {
                Email = model.Email,
                ConsumerKey = model.ConsumerKey,
                AccessToken = model.AccessToken,
                AccessTokenSecret = model.AccessTokenSecret,
                ConsumerSecret = model.ConsumerSecret,
                LastSyncDate = DateTime.Now,
            });
        }
    }
}
