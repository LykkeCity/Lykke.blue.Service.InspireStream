using AzureStorage;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using System.Collections.Generic;
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

        //public async Task CreateAsync(IEnumerable<TweetCash> tweetCash)
        //{
        //    await _tableStorage.InsertAsync(tweetCash);
        //}

        public async Task CreateOrUpdateAsync(ITweetCash tweetCash)
        {
            if (_tableStorage.GetDataAsync(tweetCash.AccountId, tweetCash.TweetId) != null)
            {
                var newItem = TweetCash.Create(tweetCash);
                await _tableStorage.InsertOrReplaceAsync(newItem);
            }
            else
            {

            }
        }

        public async Task<IEnumerable<ITweetCash>> GetAsync(string accountId)
        {
            var partitionkey = TweetCash.GeneratePartitionKey(accountId);
            return await _tableStorage.GetDataAsync(partitionkey);
        }
    }
}
