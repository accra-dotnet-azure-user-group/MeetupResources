using Akka.Actor;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TroaselLottery.Notification.Consumer.ActorSystem;

namespace TroaselLottery.Notification.Consumer;

public class KafkaConsumerWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaConsumerWorker> _logger;
        private IConsumer<Ignore, string> _consumer;

        public KafkaConsumerWorker(IConfiguration configuration, ILogger<KafkaConsumerWorker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Worker started at: {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(KafkaConsumerWorker)} background service has started");

            var conf = new ConsumerConfig
            {
                GroupId = "TroaselLottery4",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
            _consumer.Subscribe("test-topic");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(stoppingToken);
                        _logger.LogInformation($"message received is {cr.Message.Value}");
                        // do some work and the work is heavy
                        
                        TopLevelActors.MainActor.Tell(Tuple.Create(cr.Message.Value));

                        // var request = JsonConvert.DeserializeObject<>(cr.Message.Value);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"Error: {e}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Unsubscribe();
            _logger.LogInformation("Consumer unsubscribed from topic");

            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation($"Consumer disposed at: {DateTime.Now}");

            base.Dispose();
        }
    }
