using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsLambdaAspNetCoreApi.Models
{
    public class AmazonMQMessage
    {
        public string MessageID { get; set; }

        public string MessageType { get; set; }

        public long Timestamp { get; set; }

        public long DeliveryMode { get; set; }

        public string CorrelationID { get; set; }

        public string ReplyTo { get; set; }

        public MessageDestination Destination { get; set; }

        public bool Redelivered { get; set; }

        public string Type { get; set; }

        public long Expiration { get; set; }

        public long Priority { get; set; }

        public string Data { get; set; }

        public long BrokerInTime { get; set; }

        public long BrokerOutTime { get; set; }

        public class MessageDestination
        {
            public string PhysicalName { get; set; }
        }
    }
}
