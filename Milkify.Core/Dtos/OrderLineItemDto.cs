namespace Milkify.Core.Dtos
{
    public class OrderLineItemDto
    {
        public long Id { get; set; }

        public int ProductQuantity { get; set; }

        public long OrderId { get; set; }

        public virtual OrderDto Order { get; set; }

        public long ProductId { get; set; }

        public virtual ProductDto Product { get; set; }

        public decimal Total => ProductQuantity * (Product?.Price ?? 0);
    }
}