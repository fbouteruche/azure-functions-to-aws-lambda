using System.Collections.Generic;

namespace AwsLambdaAspNetCoreApi.Models
{
    public class AmazonMQEvent
    {
        public string EventSource { get; set; }

        public string EventSourceArn { get; set; }

        public IEnumerable<AmazonMQMessage> Messages { get; set; }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this, typeof(AmazonMQEvent));
        }
    }

}
