using Messenger.Models;

namespace Messenger.Contracts
{
    /// <summary>
    /// <code>ISubscribeService</code> represents the contract for the
    /// subscribe service implementation
    /// </summary>
    public interface ISubscribeService
    {
        /// <summary>
        /// Consumes a message from a RabbitMQ queue
        /// </summary>
        /// <param name="model">The <code>RabbitModel</code> countains RabbitMQ parameters (routingkey and hostname)</param>
        void RecieveMessage(RabbitModel rabbitModel);
    }
}
