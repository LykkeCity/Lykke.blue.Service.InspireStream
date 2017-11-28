using Lykke.blue.Service.InspireStream.Core.Twitter;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Globalization;

namespace Lykke.blue.Service.InspireStream.AzureRepositories.Twitter
{
    public class TweetCash : TableEntity, ITweetCash
    {
        public string TweetId { get; set; }
        public string Title { get; set; }
        public string UserImage { get; set; }
        public string TweetImage { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string AccountId { get; set; }

        public string TweetJSON { get; set; }

        public static TweetCash Create(ITweetCash src)
        {
            return new TweetCash
            {
                PartitionKey = GeneratePartitionKey(src.AccountId),
                RowKey = GenerateRowKey(src.Date, src.TweetId),
                TweetId = src.TweetId,
                Title = src.Title,
                UserImage = src.UserImage,
                TweetImage = src.TweetImage,
                Date = src.Date.ToUniversalTime(),
                Author = src.Author,
                AccountId = src.AccountId,
                TweetJSON = src.TweetJSON
            };
        }

        public static string GeneratePartitionKey(string accountId)
        {
            return accountId;
        }

        public static string GenerateRowKey(DateTime tweetDate, string tweetId)
        {
            var inverseTimeKey = DateTime
                            .MaxValue
                            .Subtract(tweetDate.ToUniversalTime())
                            .TotalMilliseconds
                            .ToString(CultureInfo.InvariantCulture);
            return string.Format("{0}-{1}",
                                     inverseTimeKey,
                                     tweetId);
        }
    }
}
