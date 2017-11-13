using AzureStorage;
using Lykke.AzureStorage.Tables.Paging;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.AzureRepositories.Twitter
{
    public class TweetsCashRepository : ITweetsCashRepository
    {
        private readonly INoSQLTableStorage<TweetCash> _tableStorage;

        public TweetsCashRepository(INoSQLTableStorage<TweetCash> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<string> RegisterAsync(ITweetCash tweetCash)
        {
            var newItem = TweetCash.Create(tweetCash);
            await _tableStorage.InsertAsync(newItem);
            return newItem.TweetId;
        }

        public async Task CreateAsync(ITweetCash tweetCash)
        {
            var newItem = TweetCash.Create(tweetCash);
            await _tableStorage.InsertAsync(newItem);
        }

        public async Task CreateOrUpdateAsync(IEnumerable<ITweetCash> tweetsCash)
        {
            await _tableStorage.InsertOrReplaceAsync(tweetsCash.Select(t => new TweetCash()
            {
                PartitionKey = t.AccountId,
                RowKey = TweetCash.GenerateRowKey(t.Date, t.TweetId),
                Title = t.Title,
                TweetId = t.TweetId,
                UserImage = t.UserImage,
                Date = t.Date.ToUniversalTime(),
                Author = t.Author,
                TweetImage = t.TweetImage,
                AccountId = t.AccountId
            }));
        }

        public async Task<IEnumerable<ITweetCash>> GetAsync(string accountId)
        {
            var partitionKeyCond = TableQuery.GenerateFilterCondition(nameof(TweetCash.PartitionKey),
                            QueryComparisons.Equal, TweetCash.GeneratePartitionKey(accountId));

            return await _tableStorage.ExecuteQueryWithPaginationAsync(
                new TableQuery<TweetCash>()
                {
                    FilterString = partitionKeyCond
                },
               new PagingInfo()
               {
                   ElementCount = 1000
               });
        }
    }
}
