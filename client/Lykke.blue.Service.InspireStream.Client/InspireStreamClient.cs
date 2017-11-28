using System;
using Common.Log;
using Lykke.blue.Service.InspireStream.Client.AutorestClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models;
using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models.ResponseModels;

namespace Lykke.blue.Service.InspireStream.Client
{
    public class InspireStreamClient : IInspireStreamClient, IDisposable
    {
        private readonly ILog _log;
        private InspireStreamAPI _apiClient;

        public InspireStreamClient(string serviceUrl, ILog log, int timeout)
        {
            _log = log;
            _apiClient =
               new InspireStreamAPI(new Uri(serviceUrl))
               {
                   HttpClient = { Timeout = TimeSpan.FromSeconds(timeout) }
               };
        }

        public void Dispose()
        {
            if (_apiClient == null)
                return;
            _apiClient.Dispose();
            _apiClient = null;
        }

        public async Task<IEnumerable<TweetsResponseModel>> GetAsync(TweetsSearchModel model)
        {
            try
            {
                var response = await _apiClient.GetTweetsWithHttpMessagesAsync(model);

                return TweetsRsponse
                    .Prepare(response)
                    .Validate()
                    .GetPayload();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task CreateTweeterAccount(TwitterAppAccountRquestModel model)
        {
            await _apiClient.CreateTweetAccountAsync(model);
        }
    }
}
