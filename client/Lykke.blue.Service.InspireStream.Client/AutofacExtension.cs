using System;
using Autofac;
using Common.Log;

namespace Lykke.blue.Service.InspireStream.Client
{
    public static class AutofacExtension
    {
        public static void RegisterInspireStreamClient(this ContainerBuilder builder, string serviceUrl, ILog log, int timeOut)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (log == null) throw new ArgumentNullException(nameof(log));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterInstance(new InspireStreamClient(serviceUrl, log, timeOut)).As<IInspireStreamClient>().SingleInstance();
        }
    }
}
