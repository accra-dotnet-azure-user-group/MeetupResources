using Akka.Actor;
using Akka.DI.AutoFac;

namespace TroaselLottery.Notification.Consumer.ActorSystem
{
    /// <summary>
    /// </summary>
    public class TopLevelActors
    {
        /// <summary>
        /// </summary>
        public static string ActorSystemName = string.Empty;

        /// <summary>
        /// </summary>
        public static Akka.Actor.ActorSystem ActorSystem;

        /// <summary>
        /// </summary>
        public static AutoFacDependencyResolver Resolver;

        /// <summary>
        /// </summary>
        public static IActorRef MainActor = ActorRefs.Nobody;

        /// <summary>
        /// </summary>
        public static IActorRef MtnActor = ActorRefs.Nobody;


        /// <summary>
        /// </summary>
        public static IActorRef KafkaProducerActor = ActorRefs.Nobody;

        /// <summary>
        /// </summary>
        public static SupervisorStrategy GetDefaultSupervisorStrategy => new OneForOneStrategy(3,
            TimeSpan.FromSeconds(3),
            ex =>
            {
                if (ex is ActorInitializationException)
                {
                    Stop();
                    return Directive.Stop;
                }

                return Directive.Resume;
            });

        /// <summary>
        /// </summary>
        public static void Stop()
        {
            ActorSystem.Terminate().Wait(1000);
        }
    }
}