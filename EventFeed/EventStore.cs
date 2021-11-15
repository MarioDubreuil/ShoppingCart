using System;
using System.Collections.Generic;

namespace ShoppingCart.EventFeed
{
    public class EventStore : IEventStore
    {
        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            throw new System.NotImplementedException();
        }

        public void Raise(string eventName, object content)
        {
            // throw new System.NotImplementedException();
            Console.WriteLine($"An event has been raised: Event = {eventName} Object = {content}");
        }
    }
}