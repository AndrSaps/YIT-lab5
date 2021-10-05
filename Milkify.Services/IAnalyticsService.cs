using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Milkify.Core.IoC;

namespace Milkify.Services
{
    public class AnalyticsResult
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
    }

    public class AnalyticsFilter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public long? CategoryId { get; set; }
        public long? ProductId { get; set; }
    }

    public class AnalyticsRequestModel
    {
        public AnalyticsFilter Filter { get; set; }
        public AnalyticsChartData Data { get; set; }
    }

    public class AnalyticsChartData
    {
        public IEnumerable<AnalyticsResult> PieChartData { get; set; }
        public IEnumerable<AnalyticsResult> LineChartData { get; set; }
    }

    public interface IAnalyticsService : IDependency
    {
        AnalyticsChartData ProductsSoldGroupBySeller(DateTime? dateFrom, DateTime? dateTo, long? categoryId, long? productId);
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IOrderService _orderService;

        public AnalyticsService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public AnalyticsChartData ProductsSoldGroupBySeller(DateTime? dateFrom, DateTime? dateTo, long? categoryId,
            long? productId)
        {
            var orderData = _orderService.GetAll();

            if (dateFrom.HasValue)
            {
                orderData = orderData.Where(x => x.OrderCreated >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                DateTime createdToDate = dateTo.Value.Date.AddDays(1).AddTicks(-1);
                orderData = orderData.Where(x => x.OrderCreated < createdToDate);
            }

            var productData = orderData.SelectMany(x => x.Items);

            if (categoryId.HasValue)
            {
                productData = productData.Where(x => x.Product.CategoryId == categoryId);
            }

            if (productId.HasValue)
            {
                productData = productData.Where(x => x.ProductId == productId);
            }

            var pieChartData = productData.GroupBy(x => x.Order.Seller).Select(g => new AnalyticsResult
            {
                Name = g.Key.FirstName + " " + g.Key.LastName,
                Value = g.Sum(s => s.ProductQuantity)
            });

            return new AnalyticsChartData
            {
                PieChartData = pieChartData
            };
        }
    }
}