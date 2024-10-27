using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using TroaselLottery.Notification.Consumer;
using TroaselLottery.Notification.Consumer.ActorSystem;
using TroaselLottery.Notification.Consumer.ActorSystem.Actors;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<KafkaConsumerWorker>();
var abuilder = new ContainerBuilder();
abuilder.RegisterType<MainActor>();
abuilder.RegisterType<SmsActor>();
abuilder.Populate(builder.Services);
var container = abuilder.Build();

TopLevelActors.ActorSystemName = "LotteryActorSystem";
TopLevelActors.ActorSystem = Akka.Actor.ActorSystem.Create(TopLevelActors.ActorSystemName);
builder.Services.AddSingleton(typeof(Akka.Actor.ActorSystem),
    serviceProvider => TopLevelActors.ActorSystem);
TopLevelActors.Resolver = new AutoFacDependencyResolver(container, TopLevelActors.ActorSystem);
TopLevelActors.MainActor = TopLevelActors.ActorSystem.ActorOf(TopLevelActors.ActorSystem.DI()
        .Props<MainActor>()
        .WithSupervisorStrategy(TopLevelActors.GetDefaultSupervisorStrategy),
    nameof(MainActor));
var host = builder.Build();
host.Run();