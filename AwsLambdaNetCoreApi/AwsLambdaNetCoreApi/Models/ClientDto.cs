using System;
using System.Collections.Generic;
using System.Text;

namespace AwsLambdaNetCoreApi.Models
{
    public class ClientDto
    {
        public string Name { get; private set; }

        public ClientDto(string name)
        {
            this.Name = name;
        }
    }
}
