using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionsApi.Models
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
