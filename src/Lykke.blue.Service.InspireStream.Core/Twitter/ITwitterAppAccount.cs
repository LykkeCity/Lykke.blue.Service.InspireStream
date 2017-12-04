using System;

namespace Lykke.blue.Service.InspireStream.Core.Twitter
{
    public interface ITwitterAppAccount
    {
        string Id { get; }
        string Email { get; set; }
        string ConsumerKey { get; set; }
        string ConsumerSecret { get; set; }
        string AccessToken { get; set; }
        string AccessTokenSecret { get; set; }
        DateTime LastSyncDate { get; set; }
    }
}
