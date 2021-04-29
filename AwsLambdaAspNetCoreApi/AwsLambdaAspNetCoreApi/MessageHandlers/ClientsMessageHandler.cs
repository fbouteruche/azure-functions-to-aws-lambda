using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Apache.NMS;
using AwsLambdaAspNetCoreApi.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace AwsLambdaAspNetCoreApi.MessageHandlers
{
    public class ClientsMessageHandler
    {
        /// <summary>
        /// This methode handles events from an Amazon MQ ActiveMQ broker    
        /// </summary>
        /// <param name="request">the Amazon MQ event data</param>
        /// <param name="context">the Lambda execution context</param>
        /// <returns></returns>
        public string CreateClientMQMessageHandler(AmazonMQEvent request, ILambdaContext context)
        {
            context.Logger.LogLine(request.ToString());

            MessageHandlerResponse response = new MessageHandlerResponse() { Messages = new List<string>() };

            foreach (AmazonMQMessage message in request.Messages)
            {
                // the message is base64 encoded so we must decode it assuming it is a JMS text message here
                string messageData = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(message.Data));
                context.Logger.LogLine(messageData);
                response.Messages.Add(messageData);

            }

            return JsonSerializer.Serialize(response, typeof(MessageHandlerResponse));
        }

        public string CreateClientSQSMessageHandler(SQSEvent request, ILambdaContext context)
        {
            foreach (SQSEvent.SQSMessage message in request.Records)
            {
                context.Logger.LogLine(message.Body);
            }
            return "messages processed";
        }
    }
}
