using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctionsApi.Services;
using AzureFunctionsApi.Models;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using System.Linq;

namespace AzureFunctionsApi.Controllers
{
    public static class ClientController
    {
        [FunctionName("GetClientsList")]
        [QueryStringParameter("name", "Search string pattern to search clients by name", DataType = typeof(string), Required = false)]
        [ProducesResponseType(typeof(IEnumerable<ClientDto>), StatusCodes.Status200OK)]
        public static async Task<IActionResult> GetClientsList(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "1.0/clients/list")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();

            string name = queryParams?.FirstOrDefault(x => x.Key.ToLower() == "name").Value ?? "";
                        
            ClientService clientService = new ClientService();

            IEnumerable<ClientDto> clientsList = clientService.GetClientsList(name);

            return new OkObjectResult(clientsList);
        }
    }
}

