namespace TroaselLottery.Notification.Consumer.ActorSystem.Actors
{
    public class SmsActor : BaseActor
    {
        private readonly ILogger<SmsActor> _logger;
        
        public SmsActor(ILogger<SmsActor> logger)
        {
            _logger = logger;
            HandleMessages();
        }

        private void HandleMessages()
        {
            ReceiveAsync<Tuple<string>>(async request => await ProcessMessage(request));
        }

        private async Task ProcessMessage(Tuple<string> x)
        {
           _logger.LogInformation($"Sending sms: {x.Item1}");
           
           await Task.CompletedTask;
        }
    }
}