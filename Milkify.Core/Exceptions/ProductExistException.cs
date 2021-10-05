using System;

namespace Milkify.Core.Exceptions
{
    public class ProductExistException:Exception
    {
        public ProductExistException(string message): base(message) { }
    }
}