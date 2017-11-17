using Lykke.blue.Service.InspireStream.AzureRepositories.Twitter;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Lykke.blue.Service.InspireStream.Models.Tweets
{
    public static class TweetsManager
    {
        public static IEnumerable<ITweetCash> GetTweetsByQuery(SearchTweetsParameters searchParameters,
                                                               ITwitterAppAccount account)
        {
            Auth.SetUserCredentials(account.ConsumerKey, account.ConsumerSecret,
                                   account.AccessToken, account.AccessTokenSecret);

            var user = User.GetAuthenticatedUser();

            List<ITweet> tweets = Search.SearchTweets(searchParameters).ToList();

            List<ITweetCash> tweetsToShow = new List<ITweetCash>();

            tweets?.ForEach(t => tweetsToShow.Add(new TweetCash()
            {
                PartitionKey = TweetCash.GeneratePartitionKey(account.Id),
                RowKey = TweetCash.GenerateRowKey(t.CreatedAt, t.IdStr),
                TweetId = t.IdStr,
                Title = t.Text,
                Author = t.CreatedBy.Name,
                Date = t.CreatedAt,
                UserImage = t.CreatedBy.ProfileImageUrl,
                TweetImage = t.Media?.FirstOrDefault()?.MediaURL,
                AccountId = account.Id
            })
            );

            return tweetsToShow;
        }

        public static async Task<List<ITweetCash>> GetTweetsCash(ITweetsCashRepository tweetCashRepository,
                                                                 string accountId)
        {
            return (await tweetCashRepository.GetAsync(accountId)).ToList();
        }

        public static async Task SaveTweetsCash(ITweetsCashRepository tweetCashRepository, IEnumerable<ITweetCash> tweets)
        {
            await tweetCashRepository.CreateOrUpdateAsync(tweets);
        }
    }
}
