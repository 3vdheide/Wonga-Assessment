namespace Messenger.Models
{
    /// <summary>
    /// The <code>MessageModel</code> that will be published
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// The message to be publised
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The name of the sender
        /// </summary>
        public string Name { get; set; }
    }
}
