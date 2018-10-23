using System;

namespace Messenger.Extensions
{
    /// <summary>
    /// Methods that extend a <see cref="String"/> for validation
    /// </summary>
    public static class StringValidationExtension
    {
        /// <summary>
        /// Validate a name
        /// </summary>
        /// <param name="name">The name to be validated</param>
        /// <returns>True is the name is valid, otherwise false</returns>
        public static bool ValidateName(this string name)
        {
            return !String.IsNullOrEmpty(name);
        }
    }
}
