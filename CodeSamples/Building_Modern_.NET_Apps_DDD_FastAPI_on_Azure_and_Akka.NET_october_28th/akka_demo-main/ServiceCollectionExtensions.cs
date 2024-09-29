using Akka.Actor;
using Akka.Hosting;
using Demo.Api.Actors;

namespace Demo.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterApiActor(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var actorSystemName = "DemoActorSystem";

            services.AddAkka(actorSystemName, builder =>
            {
                builder.WithActors((system, registry, resolver) =>
                {
                    var strategy = new OneForOneStrategy(3, TimeSpan.FromSeconds(3), ex =>
                    {
                        if (ex is not ActorInitializationException)
                            return Directive.Resume;

                        system.Terminate().Wait(1000);

                        return Directive.Stop;
                    });

                    var props = resolver.Props<WeatherProcessorActor>().WithSupervisorStrategy(strategy);

                    var actorRef = system.ActorOf(props, nameof(WeatherProcessorActor));

                    registry.Register<WeatherProcessorActor>(actorRef);
                });
            });

            return services;
        }
    }
}
