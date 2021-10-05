using System;

namespace Milkify.Core.Exceptions
{
    public class CreateUpdateException: Exception
    {
        public string ErrorKey { get; set; }

        public CreateUpdateException(string key, string message) : base(message)
        {
            ErrorKey = key;
        }
    }
}