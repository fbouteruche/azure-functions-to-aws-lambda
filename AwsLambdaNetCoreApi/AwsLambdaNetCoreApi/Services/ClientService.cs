using AwsLambdaNetCoreApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwsLambdaNetCoreApi.Services
{
    internal class ClientService
    {
        internal ClientService()
        {

        }

        internal IEnumerable<ClientDto> GetClientsList(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return new List<ClientDto>() { new ClientDto(string.Format("{0}-1", name)), new ClientDto(string.Format("{0}-2", name)), new ClientDto(string.Format("{0}-3", name)) };
            }
            else
            {
                return new List<ClientDto>();
            }
        }
    }
}
