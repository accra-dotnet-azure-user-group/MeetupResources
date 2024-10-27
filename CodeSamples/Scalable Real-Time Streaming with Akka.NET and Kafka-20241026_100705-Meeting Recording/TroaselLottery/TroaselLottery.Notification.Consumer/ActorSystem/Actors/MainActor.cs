using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;

namespace TroaselLottery.Notification.Consumer.ActorSystem.Actors
{
    public class MainActor : BaseActor
    {
        public MainActor()
        {
            HandleMessages();
        }

        private void HandleMessages()
        {
            Receive<Tuple<string>>(request => ProcessMessage(request));
        }
        
        private void ProcessMessage(Tuple<string> message)
        {
            var actorRef = Context.ActorOf(Context.DI()
                .Props<SmsActor>()
                .WithRouter(new SmallestMailboxPool(20))
                .WithSupervisorStrategy(TopLevelActors.GetDefaultSupervisorStrategy), $"sms-{Guid.NewGuid():N}-actor");
            actorRef.Forward(message);
            actorRef.Tell(PoisonPill.Instance);
        }
    }
}