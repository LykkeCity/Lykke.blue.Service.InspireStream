using Lykke.blue.Service.InspireStream.Core.Twitter;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Lykke.blue.Service.InspireStream.AzureRepositories.Twitter
{
    public class TwitterAppAccount : TableEntity, ITwitterAppAccount
    {
        public string Id => RowKey;
        public string Email { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public DateTime LastSyncDate { get; set; }

        public static TwitterAppAccount Create(ITwitterAppAccount src)
        {
            return new TwitterAppAccount
            {
                PartitionKey = GeneratePartitionKey(src.Email),
                RowKey = GenerateRowKey(),
                ConsumerKey = src.ConsumerKey,
                ConsumerSecret = src.ConsumerSecret,
                AccessToken = src.AccessToken,
                AccessTokenSecret = src.AccessTokenSecret,
                LastSyncDate = src.LastSyncDate,
                Email = src.Email
            };
        }

        public static TwitterAppAccount Update(ITwitterAppAccount src)
        {
            return new TwitterAppAccount
            {
                PartitionKey = src.Email,
                RowKey = src.Id,
                ConsumerKey = src.ConsumerKey,
                ConsumerSecret = src.ConsumerSecret,
                AccessToken = src.AccessToken,
                AccessTokenSecret = src.AccessTokenSecret,
                LastSyncDate = src.LastSyncDate,
                Email = src.Email
            };
        }

        public static string GeneratePartitionKey(string email)
        {
            return email;
        }

        internal static string GenerateRowKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
