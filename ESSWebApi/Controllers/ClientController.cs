using CZ.Api.Base;
using ESSWebApi.Dtos.Request.Client;
using ESSWebApi.Routes;
using ESSWebApi.Services.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ESSWebApi.Controllers
{
    [Authorize, ApiController]
    public class ClientController : BaseController
    {
        private readonly IClient _client;

        public ClientController(IClient client)
        {
            _client = client;
        }

        [HttpGet(APIRoutes.Client.GetAll)]
        public async Task<IActionResult> GetAllClients(int skip, int take = 10)
        {

            var result = await _client.GetAll(skip, take);

            if (!result.IsSuccess)
                return BadRequest(result.Message);


            return Ok(result);
        }

        [HttpGet(APIRoutes.Client.Search)]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Invalid query");

            var result = await _client.Search(query);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPost(APIRoutes.Client.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateRequest request)
        {
            if (request == null) return BadRequest("Client required");
            if (request.Name == null) return BadRequest("Client name cannot be empty");
            if (request.Latitude == null) return BadRequest("Latitude cannot be empty");
            if (request.Longitude == null) return BadRequest("Longitude cannot be empty");
            if (request.PhoneNumber == null) return BadRequest("Phone Number cannot be empty");
            if (request.Address == null) return BadRequest("Address cannot be empty");
            if (request.ContactPerson == null) return BadRequest("Contact Person cannot be empty");
            if (request.City == null) return BadRequest("City cannot be empty");
            //if (request.Email == null) return BadRequest("Email cannot be empty");

            var userId = GetUserId();
            var result = await _client.Create(request, userId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost(APIRoutes.Client.Delete)]
        public async Task<IActionResult> Delete(int clientId)
        {
            if (clientId == 0) return BadRequest("ID cannot be empty");

            var result = await _client.Delete(clientId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost(APIRoutes.Client.Update)]
        public async Task<IActionResult> Update([FromBody] CreateUpdateRequest request, int clientId)
        {

            if (request == null) return BadRequest("Request cannot be empty");

            var userId = GetUserId();
            var result = await _client.Update(request, clientId, userId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }


        /*        [HttpGet(APIRoutes.Client.GetById)]
                public async Task<IActionResult> GetById(int id)
                {
                    var result = await GetClientById(id);
                    if (result == null)
                        return BadRequest("Unable to find the client");

                    return View(result);
                }


                private async Task<GetClientResult> GetAll()
                {
                    var clients = await _context.Clients
                          .Select(x => new ClientDto { Id = x.Id, Name = x.Name })
                          .ToListAsync();

                    return new GetClientResult
                    {
                        IsSuccess = true,
                        Clients = clients
                    };
                }

                private async Task<ClientDto?> GetClientById(int id)
                {
                    return await _context.Clients
                        .Select(x => new ClientDto { Id = x.Id, Name = x.Name })
                        .FirstOrDefaultAsync(x => x.Id == id);
                }*/
    }
}
