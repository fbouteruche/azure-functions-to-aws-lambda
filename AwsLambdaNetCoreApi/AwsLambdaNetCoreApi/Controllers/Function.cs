using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using AwsLambdaNetCoreApi.Models;
using AwsLambdaNetCoreApi.Services;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsLambdaNetCoreApi.Controllers
{
    public class ClientController
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public ClientController()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse GetClientsList(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Request\n");

            string name = (!(request.QueryStringParameters is null) && request.QueryStringParameters.ContainsKey("name")) ? request.QueryStringParameters["name"] : string.Empty;

            ClientService clientService = new ClientService();

            IEnumerable<ClientDto> clientsList = clientService.GetClientsList(name);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = System.Text.Json.JsonSerializer.Serialize(clientsList),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return response;
        }
    }
}
