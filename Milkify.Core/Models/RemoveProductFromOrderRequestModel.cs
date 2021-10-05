namespace Milkify.Core.Models
{
    public class RemoveProductFromOrderRequestModel
    {
        public long OrderId { get; set; }
        public long ProductOrderId { get; set; }
    }
}