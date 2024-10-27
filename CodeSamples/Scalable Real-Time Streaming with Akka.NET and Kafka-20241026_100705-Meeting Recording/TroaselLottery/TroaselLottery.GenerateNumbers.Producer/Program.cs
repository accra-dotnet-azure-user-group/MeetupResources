using Akka.Actor;
using Akka.Streams;
using Akka.Streams.Dsl;
using Akka.Streams.Kafka.Settings;
using Confluent.Kafka;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TroaselLottery.GenerateNumbers.Producer
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var system = ActorSystem.Create("KafkaProducer");
            var materializer = system.Materializer();

            var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            var source = Source.From(Enumerable.Range(1, 100))
                .SelectAsync(1, async num =>
                {
                    var message = new Message<Null, string> { Value = $"message-{num}" };
                    await producer.ProduceAsync("test-topic", message);
                    Console.WriteLine($"Produced: message-{num}");
                    return num;
                });

            await source.RunWith(Sink.Ignore<int>(), materializer);

            await system.Terminate();
            Console.WriteLine("Producer has finished sending messages.");
        }
    }
}