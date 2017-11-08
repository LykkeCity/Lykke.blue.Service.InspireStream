using System;

namespace Lykke.blue.Service.InspireStream.Client.AutorestClient.Models.ResponseModels
{
    public abstract class BaseResponseModel<T>
    {
        public ErrorModel Error { get; set; }

        public abstract T GetPayload();

        public BaseResponseModel<T> Validate()
        {
            if (Error != null)
            {
                throw new Exception(Error.Message);
            }

            return this;
        }
    }
}
