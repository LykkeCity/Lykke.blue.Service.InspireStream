using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.blue.Service.InspireStream.Abstractions;
using Lykke.blue.Service.InspireStream.AzureRepositories.Twitter;
using Lykke.blue.Service.InspireStream.Core;
using Lykke.blue.Service.InspireStream.Core.Services;
using Lykke.blue.Service.InspireStream.Core.Twitter;
using Lykke.blue.Service.InspireStream.Models.Tweets;
using Lykke.blue.Service.InspireStream.Services;
using Lykke.Logs;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;
namespace Lykke.blue.Service.InspireStream.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<InspireStreamSettings> _settings;
        private readonly ILog _log;
        private readonly IServiceCollection _services;
        //private readonly TwitterSettings _twitterSettings;

        public ServiceModule(IReloadingManager<InspireStreamSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
            _services = new ServiceCollection();
            //_twitterSettings = twitterSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
               .As<ILog>()
               .SingleInstance();

            builder.RegisterType<TweetsManager>()
                .As<ITweetsManager>();

            builder.RegisterInstance<ITweetsCashRepository>(
               new TweetsCashRepository(
                    AzureTableStorage<TweetCash>.Create(_settings.ConnectionString(x => x.Db.RepoConnectionString), "TweetsCash",
                       _log)));

            builder.RegisterInstance<ITwitterAppAccountRepository>(
              new TwitterAppAccountRepository(
                   AzureTableStorage<TwitterAppAccount>.Create(_settings.ConnectionString(x => x.Db.RepoConnectionString), "TwitterAppAccount",
                      _log)));

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterInstance(_settings.CurrentValue.TwitterSettings);

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.Populate(_services);
        }
    }
}
