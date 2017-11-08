using AzureStorage;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.blue.Service.InspireStream.AzureRepositories.Twitter
{
    public class TwitterAppAccountRepository : ITwitterAppAccountRepository
    {
        private readonly INoSQLTableStorage<TwitterAppAccount> _tableStorage;

        public TwitterAppAccountRepository(INoSQLTableStorage<TwitterAppAccount> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<string> RegisterAsync(ITwitterAppAccount twitterAppAccount)
        {
            var newItem = TwitterAppAccount.Create(twitterAppAccount);
            await _tableStorage.InsertAsync(newItem);
            return newItem.Id;
        }

        public async Task UpdateAsync(ITwitterAppAccount twitterAppAccount)
        {
            var itemToupdate = TwitterAppAccount.Update(twitterAppAccount);
            await _tableStorage.InsertOrReplaceAsync(itemToupdate);
        }

        public async Task<ITwitterAppAccount> GetAsync(string email)
        {
            var partitionKey = TwitterAppAccount.GeneratePartitionKey(email);
            return (await _tableStorage.GetDataAsync(partitionKey)).FirstOrDefault();
        }
    }
}