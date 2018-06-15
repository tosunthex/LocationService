using System;
using Microsoft.AspNetCore.Mvc;
using StatlerWaldorfCorp.LocationService.Models;

namespace StatlerWaldorfCorp.LocationService.Controllers
{
    [Route("api/location/{memberId}")]
    public class LocationRecordController:Controller
    {
        private readonly ILocationRecordRepository _locationRecordRepository;
        public LocationRecordController(ILocationRecordRepository locationRecordRepository)
        {
            _locationRecordRepository = locationRecordRepository;
        }

        [HttpPost]
        public IActionResult AddLocation(Guid memberId, [FromBody] LocationRecord locationRecord)
        {
            _locationRecordRepository.Add(locationRecord);
            return this.Created($"api/locations/{memberId}/{locationRecord.ID}", locationRecord);
        }

        [HttpGet]
        public IActionResult GetLocationFromMember(Guid memberId)
        {
            return this.Ok(_locationRecordRepository.AllForMember(memberId));
        }
    }
}