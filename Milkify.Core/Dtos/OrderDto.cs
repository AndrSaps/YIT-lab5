using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;

namespace Milkify.Core.Dtos
{
    public class OrderDto: BaseDto
    {
        public OrderDto()
        {
            Items = new List<OrderLineItemDto>();
        }

        public long Id { get; set; }

        [TableColumn("Order Number")]
        public string OrderNumber { get; set; }

        [TableColumn("Create Date")]
        public DateTime OrderCreated { get; set; }

        [TableColumn("Total Amount")]
        public decimal TotalAmount { get; set; }
        
        [TableColumn("Total Weight")]
        public decimal TotalWeight { get; set; }

        [Required]
        [DisplayName("Customer")]
        public long? CustomerId { get; set; }

        [TableColumn("Customer", "Name")]
        public CustomerDto Customer { get; set; }

        [Required]
        [DisplayName("Seller")]
        public long? SellerId { get; set; }
        
        [TableColumn("Seller", "DisplayName")]
        public UserDto Seller { get; set; }

        [TableColumn("Status")]
        public OrderStatus OrderStatus { get; set; }
        
        public List<OrderLineItemDto> Items { get; set; }

        public List<PaymentDto> Payments { get; set; }

        public long ShipmentId { get; set; }

        public ShipmentDto Shipment { get; set; }

        public AudioRecordDto AudioRecord { get; set; }

        public bool IsEditable => OrderStatus == OrderStatus.Placed;

        public override IList<DatatableAction> GetDtoActions(ClaimsPrincipal currentUser)
        {
            var actions = new List<DatatableAction>();
            if (currentUser.IsInRole("Admin") || currentUser.IsInRole("Seller"))
            {
                actions.Add(new DatatableAction()
                {
                    Name = "Edit",
                    CssIconClass = "fa fa-edit"
                });
                if (currentUser.IsInRole("Admin"))
                {
                    actions.Add(new DatatableAction()
                    {
                        Name = "Delete",
                        CssIconClass = "fa fa-trash"
                    });
                }

            }
            return actions;
        }
    }
}
