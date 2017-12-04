namespace Lykke.blue.Service.InspireStream.Core
{
    public class AppSettings
    {
        public InspireStreamSettings InspireStreamService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }

    public class InspireStreamSettings
    {
        public DbSettings Db { get; set; }
        public TwitterSettings TwitterSettings { get; set; }
    }

    public class DbSettings
    {
        public string RepoConnectionString { get; set; }
        public string LogsConnString { get; set; }
    }

    public class TwitterSettings
    {
        public double DefaultMinutesToCheck { get; set; }
    }

    public class SlackNotificationsSettings
    {
        public AzureQueueSettings AzureQueue { get; set; }
    }

    public class AzureQueueSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
