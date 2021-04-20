using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwsLambdaAspNetCoreApi.Models;
using AwsLambdaAspNetCoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwsLambdaAspNetCoreApi.Controllers
{
    [Route("1.0/[controller]")]
    public class ClientsController : ControllerBase
    {
        // GET api/values
        [HttpGet("list")]
        public IEnumerable<ClientDto> GetClientsList([FromQuery] string name)
        {
            ClientService clientService = new ClientService();

            IEnumerable<ClientDto> clientsList = clientService.GetClientsList(name);

            return clientsList;
        }
    }
}
