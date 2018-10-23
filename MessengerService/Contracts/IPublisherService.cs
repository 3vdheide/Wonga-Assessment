using Messenger.Models;

namespace Messenger.Contracts
{
    /// <summary>
    /// <code>IPublisherService</code> represents the contract for the
    /// publish service implementation
    /// </summary>
    public interface IPublisherService
    {
        /// <summary>
        /// Publishes a message to a RabbitMQ exchange
        /// </summary>
        /// <param name="model">The <code>RabbitModel</code> countains RabbitMQ parameters and the messsage to be published</param>
        void PublishMessage(RabbitModel model);
    }
}
