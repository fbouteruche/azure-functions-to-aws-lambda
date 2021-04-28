using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsLambdaAspNetCoreApi.Models
{
    public class MessageHandlerResponse
    {
        public List<string> Messages { get; set; }
    }
}
