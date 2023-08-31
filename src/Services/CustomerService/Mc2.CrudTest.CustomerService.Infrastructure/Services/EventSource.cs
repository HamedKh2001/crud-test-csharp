using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace SharedKernel.Contracts.Infrastructure
{
    public class EventSource : IEventSource
    {
        private readonly IEventStoreConnection _connection;

        public EventSource(IEventStoreConnection connection)
        {
            _connection = connection;
        }
        public void Save<TEvent>(string aggregateName, string streamId, IEnumerable<TEvent> events) where TEvent : IEvent
        {
            if (!events.Any()) return;

            var changes = events
                .Select(@event =>
                    new EventData(
                        eventId: Guid.NewGuid(),
                        type: @event.GetType().Name,
                        isJson: true,
                        data: Serialize(@event),
                        metadata: Serialize(new Events.EventMetadata
                        { ClrType = @event.GetType().AssemblyQualifiedName })
                    ))
                .ToArray();

            if (!changes.Any()) return;
            var streamName = $"{aggregateName} - {streamId}";

            _connection.AppendToStreamAsync(
               streamName,
               ExpectedVersion.Any,
               changes).Wait();
        }
        private static byte[] Serialize(object data) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}
