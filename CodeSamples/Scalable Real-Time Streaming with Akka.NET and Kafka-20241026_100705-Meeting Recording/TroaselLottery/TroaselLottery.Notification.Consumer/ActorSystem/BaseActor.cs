using Akka.Actor;
using Akka.Event;

namespace TroaselLottery.Notification.Consumer.ActorSystem
{
    /// <summary>
    /// </summary>
    public class BaseActor : ReceiveActor
    {
        /// <summary>
        /// </summary>
        protected readonly ILoggingAdapter Logger = Context.GetLogger();

        /// <summary>
        /// </summary>
        /// <param name="event"></param>
        protected void Publish(object @event)
        {
            Context.Dispatcher.EventStream.Publish(@event);
        }
    }
}