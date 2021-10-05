namespace Milkify.Core.Models
{
    public class AddProductToOrderRequestModel
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
    }
}