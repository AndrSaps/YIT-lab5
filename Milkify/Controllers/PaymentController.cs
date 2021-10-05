using Microsoft.AspNetCore.Mvc;
using Milkify.Core.Attributes;
using Milkify.Core.Datatable.DatatableRequests;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Services;

namespace Milkify.Controllers
{
    [LayoutLink(order: 3, roles:"Admin,Seller")]
    public class PaymentController : BaseCrudController<Payment, PaymentDto, IPaymentService, DefaultDataTablesRequest>
    {

        public PaymentController(IPaymentService crudService) : base(crudService)
        {
        }
    }
}