using BallastLane.API.Service;
using BallastLane.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BallastLane.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientModel>>> Get()
        {
            var (result, clients) = await _clientService.GetAllClients();

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _clientService.GetErrorMessage());

            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientModel>> Get(string id)
        {
            var (result, client) = await _clientService.GetClientById(id);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _clientService.GetErrorMessage());

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ClientModel client)
        {
            bool result = await _clientService.CreateClient(client);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _clientService.GetErrorMessage());

            return Ok();
        }

        [HttpPut()]
        public async Task<ActionResult> Put([FromBody] ClientModel client)
        {
            bool result = await _clientService.UpdateClient(client);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _clientService.GetErrorMessage());

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            bool result = await _clientService.DeleteClient(id);

            if (!result)
                return StatusCode((int)HttpStatusCode.InternalServerError, _clientService.GetErrorMessage());

            return Ok();
        }
    }
}
