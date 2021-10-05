using System;

namespace Milkify.Core.Exceptions
{
    public class QuantityException: Exception
    {
        public QuantityException(string message) : base(message) { }
    }
}