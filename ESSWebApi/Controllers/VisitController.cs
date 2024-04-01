using CZ.Api.Base;
using CZ.Api.Base.Extensions;
using ESSWebApi.Dtos.Request.Visit;
using ESSWebApi.Routes;
using ESSWebApi.Services.Visit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;


namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class VisitController : BaseController
    {
        private readonly IVisit _visit;


        public VisitController(IVisit visit)
        {
            _visit = visit;

        }


        [HttpPost(APIRoutes.Visit.Create)]
        public async Task<IActionResult> Create([FromForm] CreateVisitRequest request, IFormFile? file)
        {
            if (file == null) return BadRequest("Image is required");

            if (request.Latitude == null || request.Longitude == null) return BadRequest("Latitude & Longitude is required");

            if (request.ClientId == null) return BadRequest("ClientId is required");

            var result = await _visit.Create(GetUserId(), request, file);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok("Added");
        }

        [HttpGet(APIRoutes.Visit.GetHistory)]
        public async Task<IActionResult> GetHistory(string? date)
        {
            DateTime dateTime = DateTime.Now;

            if (date != null)
            {
                dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var result = await _visit.GetHistory(HttpContext.GetUserId(), dateTime);

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Data);
        }


        [HttpGet(APIRoutes.Visit.GetVisitsCount)]
        public async Task<IActionResult> GetVisitsCount()
        {
            var result = await _visit.GetVisitsCount(HttpContext.GetUserId());

            if (!result.IsSuccess) return BadRequest(result.Message);

            return Ok(result.Data);
        }
    }
}
