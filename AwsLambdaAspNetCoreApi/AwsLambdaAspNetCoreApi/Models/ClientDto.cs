using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsLambdaAspNetCoreApi.Models
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
