using System;

namespace Lykke.blue.Service.InspireStream.Core.Twitter
{
    public interface ITweetCash
    {
        string TweetId { get; set; }
        string Title { get; set; }
        string UserImage { get; set; }
        string TweetImage { get; set; }
        DateTime Date { get; set; }
        string Author { get; set; }
        string AccountId { get; set; }

        string TweetJSON { get; set; }
    }
}
