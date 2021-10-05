using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Milkify.Core;
using Milkify.Core.Dtos;
using Milkify.Core.Entities;
using Milkify.Core.IoC;
using Milkify.Core.Repository;
using Newtonsoft.Json.Linq;

namespace Milkify.Services
{
    public interface ILocationService : IBaseService<Location, LocationDto>, IDependency
    {
        long CreateRecordFromGooglePlaceModel(dynamic model);
    }

    public class LocationService : BaseService<Location, LocationDto>, ILocationService
    {
        public LocationService(IMapper mappingService, IBaseRepository repository) : base(mappingService, repository)
        {
        }

        public long CreateRecordFromGooglePlaceModel(dynamic model)
        {
            Location location = new Location
            {
                Address = model.address,
                Latitude = model.latitude,
                Longitude = model.longitude
            };

            Repository.Insert(location);
            Repository.SaveChanges();
            return location.Id;
        }
    }
}