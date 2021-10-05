using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Milkify.Core;
using Milkify.Core.Datatable.DatatableRequests.RequestModels;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.Entities.Membership;
using Milkify.Core.Enums;
using Milkify.Core.Exceptions;
using Milkify.Core.IoC;
using Milkify.Core.Models;
using Milkify.Core.Repository;
using Milkify.PdfGenerator;

namespace Milkify.Services
{
    public interface IOrderService : IBaseService<Order, OrderDto>, IDependency
    {
        void AddProductToOrder(long productId, long orderId);
        void RemoveItemFromOrder(long orderId, long productOrderId);
        Stream GenerateInvoicePdf(long orderId);
        void MarkOrderAsPaid(long orderId, bool isShipmentNeeded);
    }

    public class OrderService : BaseService<Order, OrderDto>, IOrderService
    {
        private readonly IBaseRepository _repository;
        private readonly IProductService _productService;
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IOrderProductService _orderProductService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mappingService;

        public OrderService(IMapper mappingService,
            IBaseRepository repository,
            IProductService productService,
            IPdfGenerator pdfGenerator,
            IOrderProductService orderProductService,
            UserManager<User> userManager,
            IUserService userService, IPaymentService paymentService) : base(mappingService, repository)
        {
            _mappingService = mappingService;
            _repository = repository;
            _productService = productService;
            _pdfGenerator = pdfGenerator;
            _orderProductService = orderProductService;
            _userManager = userManager;
            _userService = userService;
            _paymentService = paymentService;
        }

        public override Order PrepareForInserting(Order entity, OrderDto model)
        {
            var order = base.PrepareForInserting(entity, model);
            order.Items = model.Items.Select(x => new OrderLineItem
            {
                OrderId = x.OrderId,
                ProductId = x.ProductId,
                ProductQuantity = x.ProductQuantity,
                Product = _repository.Get<Product>(y => y.Id == x.ProductId)
            }).ToList();
            order.OrderStatus = OrderStatus.Placed;
            order.OrderCreated = DateTime.Now;
            order.TotalAmount = order.Items.Sum(x => x.Product.Price * x.ProductQuantity);
            order.TotalWeight = order.Items.Sum(x => x.Product.Weight);
            order.OrderNumber = DateTimeOffset.Now.ToString("yyyyMMdd_HHmmssff");

            return order;
        }

        public override Order PrepareForUpdating(Order entity, OrderDto model)
        {
            var order = base.PrepareForUpdating(entity, model);

            foreach (var item in model.Items)
            {
                var itemEntity = _orderProductService.GetEntity(item.Id);
                var product = _productService.GetEntity(item.ProductId);
                var productQtyLeft = product.Inventories.Sum(x => x.SupplyQuantity - x.ReservedSupplyQuantity);
                if (itemEntity != null)
                {
                    if (item.ProductQuantity - itemEntity.ProductQuantity > productQtyLeft)
                    {
                        throw new CreateUpdateException("Items", $"Product {product.ProductName} is out of quantity. Only {productQtyLeft} items left");
                    }

                    itemEntity.ProductQuantity = item.ProductQuantity;
                    entity.Items.Add(itemEntity);
                }
                else
                {
                    if (item.ProductQuantity > productQtyLeft)
                    {
                        throw new CreateUpdateException("Items", $"Product {product.ProductName} is out of quantity");
                    }

                    itemEntity = MappingService.Map<OrderLineItem>(item);
                    itemEntity.Product = product;
                }
                order.Items.Add(itemEntity);
            }

            order.Items = order.Items.Where(x => model.Items.Any(y => y.Id == x.Id)).ToList();
            order.TotalAmount = order.Items.Sum(x => x.Product.Price * x.ProductQuantity);
            order.TotalWeight = order.Items.Sum(x => x.Product.Weight);
            
            return order;
        }

        public override IEnumerable<OrderDto> FilterForDataTables<TDatatableRequestModel>(IEnumerable<OrderDto> dtos,
            TDatatableRequestModel requestModel, ClaimsPrincipal currentUser)
        {
            var userId = long.Parse(_userManager.GetUserId(currentUser));
            var roles = _userManager
                .GetRolesAsync(_userService.GetEntity(userId)).Result;
            if (roles.Contains("Admin")) { }
            else if (roles.Contains("Seller"))
            {
                dtos = dtos.Where(x => x.SellerId == userId);
            }
            else
            {
                return new List<OrderDto>();
            }
            var request = requestModel as OrderRequestModel;

            if (request != null)
            {
                if (request.CustomerId.HasValue)
                {
                    dtos = dtos.Where(x => x.CustomerId == request.CustomerId);
                }

                if (request.SellerId.HasValue)
                {
                    dtos = dtos.Where(x => x.SellerId == request.SellerId.Value);
                }

                if (request.StatusId.HasValue)
                {
                    dtos = dtos.Where(x => x.SellerId == request.StatusId);
                }

                if (request.DateFrom.HasValue)
                {
                    dtos = dtos.Where(x => x.OrderCreated >= request.DateFrom);
                }

                if (request.DateTo.HasValue)
                {
                    DateTime createdToDate = request.DateTo.Value.Date.AddDays(1).AddTicks(-1);
                    dtos = dtos.Where(x => x.OrderCreated <= createdToDate);
                }
            }

            return base.FilterForDataTables(dtos, requestModel, currentUser);
        }

        public void AddProductToOrder(long orderId, long productId)
        {
            var order = GetDto(orderId);
            if (order.Items.Any(x => x.Product.Id == productId))
            {
                throw new CreateUpdateException("Items", "Items with this product is already exit");
            }
            order.Items.Add(new OrderLineItemDto
            {
                OrderId = orderId,
                ProductId = productId,
                ProductQuantity = 1
            });

            Update(order, orderId);
        }

        public void RemoveItemFromOrder(long orderId, long productOrderId)
        {
            var order = GetDto(orderId);
            order.Items = order.Items.Where(x => x.Id != productOrderId).ToList();

            Update(order, order.Id);
        }

        public Stream GenerateInvoicePdf(long orderId)
        {
            var order = GetDto(orderId);
            var orderItems = order.Items;
            PdfPTable headerTable = new PdfPTable(2)
            {
                SpacingBefore = 15f,
                TotalWidth = 550f,
                LockedWidth = true
            };
            headerTable.AddCell(new PdfPCell(new Paragraph(5f, "Customer Name: " + order.Customer.Name))
            {
                Border = 0,
                VerticalAlignment = Element.ALIGN_MIDDLE,
            });
            headerTable.AddCell(new PdfPCell(new Paragraph(5f, "Seller Name: " + order.Seller.DisplayName))
            {
                Border = 0,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            headerTable.AddCell(new PdfPCell(new Paragraph(5f, "Customer Address: " + order.Customer.Location.Address))
            {
                Border = 0,
                VerticalAlignment = Element.ALIGN_MIDDLE,


            });
            headerTable.AddCell(new PdfPCell()
            {
                Border = 0
            });

            headerTable.AddCell(new PdfPCell(new Paragraph(5f, "Customer Phone: " + order.Customer.PhoneNumber))
            {
                Border = 0,
                VerticalAlignment = Element.ALIGN_MIDDLE,

            });
            headerTable.AddCell(new PdfPCell()
            {
                Border = 0
            });
            headerTable.SetWidths(new float[] { 275f, 275f });
            IList<PdfPCell> tableHeader = new List<PdfPCell>();
            string[] centeredHeaderValues = { "Product Name", "Unit Price", "QTY", "Amount Price" };
            foreach (var value in centeredHeaderValues)
            {
                var pCell =
                    new PdfPCell(
                        new Phrase(new Chunk(
                            value.ToUpper(CultureInfo.CurrentCulture), FontFactory.GetFont(FontFactory.HELVETICA_BOLD)
                        ))
                    )
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                tableHeader.Add(pCell);
            }

            PdfPTable table = new PdfPTable(tableHeader.Count)
            {
                SpacingBefore = 15f,
                TotalWidth = 550f,
                LockedWidth = true
            };
            float[] widths = { 300f, 75f, 35f, 100f };
            table.SetWidths(widths);

            foreach (var item in tableHeader)
            {
                table.AddCell(item);
            }

            foreach (var orderItem in orderItems)
            {
                var productPrice = orderItem.ProductQuantity * orderItem.Product.Price;
                table.AddCell(new PdfPCell(new Phrase(orderItem.Product.ProductName))
                {
                    Padding = 5,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
                string[] centeredStrings =
                {
                        orderItem.Product.Price?.ToString("C") ?? "$0",
                        orderItem.ProductQuantity.ToString(),
                        productPrice?.ToString("C") ?? "$0"
                    };
                foreach (var centeredString in centeredStrings)
                {
                    var pCell = new PdfPCell(new Phrase(centeredString))
                    {
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(pCell);
                }
            }
            var emptyCell = new PdfPCell(new Phrase(string.Empty))
            {
                Border = 0,
                Padding = 5,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(emptyCell);
            table.AddCell(new PdfPCell(new Phrase("ORDER PRICE"))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Colspan = 2,
                Padding = 5,
            });
            table.AddCell(emptyCell);
            table.AddCell(new PdfPCell(new Phrase("SHIPMENT PRICE"))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Colspan = 2,
                Padding = 5,
            });

            table.AddCell(emptyCell);
            table.AddCell(new PdfPCell(new Phrase(new Chunk("TOTAL PRICE", FontFactory.GetFont(FontFactory.HELVETICA_BOLD))))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Colspan = 2,
                Padding = 5,
            });
            string header = $"Invoice #{order.OrderNumber}";
            return _pdfGenerator.GenerateTablePdf(header, headerTable, table);
        }

        public void MarkOrderAsPaid(long orderId, bool isShipmentNeeded)
        {
            var order = GetDto(orderId);
            order.OrderStatus = OrderStatus.Paid;
            _paymentService.Create(new PaymentDto
            {
                AmountPaid = order.TotalAmount,
                OrderId = orderId,
                PaymentDate = DateTime.Now
            });
            Update(order, orderId);
        }
        

        public override Order Update(OrderDto model, long id)
        {
            var order = base.Update(model, id);
            foreach (var item in order.Items)
            {
                _repository.Context.Entry(item.Product).Reload();
            }
            return order;
        }
    }
}

