using EventSourcing.Shared.Events;

namespace EventSourcing.API.EventStores
{
    public abstract class AbstrackStream
    {
        protected readonly LinkedList<IEvent> Events = new LinkedList<IEvent>();

        protected string StreamName { get;}
    }
}
