using Microsoft.Rest;
using System;
using System.Collections.Generic;

namespace Lykke.blue.Service.InspireStream.Client.AutorestClient.Models.ResponseModels
{
    public class TweetsRsponse : BaseResponseModel<IEnumerable<TweetsResponseModel>>
    {
        public IEnumerable<TweetsResponseModel> TweetsResponseModel { get; set; }

        public static TweetsRsponse Prepare(HttpOperationResponse<IList<TweetsResponseModel>> apiResponse)
        {
            var error = apiResponse.Body as ErrorResponse;
            var result = apiResponse.Body as IEnumerable<TweetsResponseModel>;

            if (error != null)
            {
                return new TweetsRsponse
                {
                    Error = new ErrorModel
                    {
                        Message = error.ErrorMessage
                    }
                };
            }

            if (result != null)
            {
                return new TweetsRsponse
                {
                    TweetsResponseModel = result
                };
            }

            throw new ArgumentException("Unknown response object");
        }

        public override IEnumerable<TweetsResponseModel> GetPayload()
        {
            return TweetsResponseModel;
        }
    }
}