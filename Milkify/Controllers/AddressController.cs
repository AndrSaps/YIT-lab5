using Microsoft.AspNetCore.Mvc;
using Milkify.Services;

namespace Milkify.Controllers
{
    public class AddressController : Controller
    {
        private readonly ILocationService _locationService;

        public AddressController(ILocationService locationService)
        {
            _locationService = locationService;
        }


        [HttpPost]
        public IActionResult GetLocationId([FromBody] dynamic googlePlaceInfo)
        {
            var result = _locationService.CreateRecordFromGooglePlaceModel(googlePlaceInfo);
            return Ok(result);
        }
    }
}