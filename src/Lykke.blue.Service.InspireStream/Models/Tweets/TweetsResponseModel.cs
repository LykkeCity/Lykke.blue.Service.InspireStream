using Lykke.blue.Service.InspireStream.AzureRepositories.Twitter;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using System;

namespace Lykke.blue.Service.InspireStream.Models.Tweets
{
    public class TweetsResponseModel
    {
        public string TweetId { get; set; }
        public string Title { get; set; }
        public string UserImage { get; set; }
        public string TweetImage { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string AccountId { get; set; }

        public static TweetsResponseModel Create(ITweetCash src)
        {
            return new TweetsResponseModel
            {
                TweetId = src.TweetId,
                Title = src.Title,
                UserImage = src.UserImage,
                TweetImage = src.TweetImage,
                Date = src.Date,
                Author = src.Author,
                AccountId = src.AccountId
            };
        }
    }
}
