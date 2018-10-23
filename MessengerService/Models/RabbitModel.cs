namespace Messenger.Models
{
    /// <summary>
    /// The <code>RabbitModel</code> contains properties that is required by RabbitMQ
    /// </summary>
    public class RabbitModel
    {
        /// <summary>
        /// the host name of the domain where the RabbitMQ service is running
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The serialized message that will be published
        /// (Any model can be converted to a serialized JSON string and assigned to this property
        /// </summary>
        public string SerializedMessage { get; set; }

        /// <summary>
        /// The routing key that informs RabbitMQ where to route the message
        /// </summary>
        public string RoutingKey { get; set; }
    }
}
