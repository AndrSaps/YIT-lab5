using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;

namespace Milkify.Services
{
    public interface ICustomerService : IBaseService<Customer, CustomerDto>, IDependency
    {
    }
    
    public class CustomerService : BaseService<Customer, CustomerDto>, ICustomerService
    {
        public CustomerService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }
    }
}