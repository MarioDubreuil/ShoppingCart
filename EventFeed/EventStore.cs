using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.EventFeed
{
    public class EventStore : IEventStore
    {
        private static readonly List<Event> events = new();
        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            return events
                    .Where(e => e.SequenceNumber >= firstEventSequenceNumber && e.SequenceNumber <= lastEventSequenceNumber)
                    .OrderBy(e => e.SequenceNumber);
        }

        public void Raise(string eventName, object content)
        {
            Console.WriteLine("An event has been raised");
            events.Add(new Event(events.Count + 1, DateTimeOffset.UtcNow, eventName, content));
            Console.WriteLine(events.Last());
        }
    }
}