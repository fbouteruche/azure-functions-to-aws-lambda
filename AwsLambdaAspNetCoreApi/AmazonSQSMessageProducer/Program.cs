using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AmazonSQSMessageProducer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true).Build();

            if (string.IsNullOrEmpty(configuration["sqs_queue_uri"])
                || configuration["sqs_queue_uri"] == "<your-sqs-queue-uri>")
            {
                Console.WriteLine("you must provide a valid value for sqs_queue_uri in the appsettings.json file");
                return;
            }

            // locally AWS SDK for .NET will use your default profile AWS Credentials while on Lambda it will the permission associated to your Lambda function
            // or AWS Credentials that you configure through Lambda environment variable
            using (AmazonSQSClient sqsClient = new AmazonSQSClient())
            {
                for (int i = 0; i < 300; i++)
                {
                    string message = $"My AWSome message {i}";
                    Console.WriteLine(message);
                    SendMessageResponse response = await sqsClient.SendMessageAsync(configuration["sqs_queue_uri"], message);
                    Console.WriteLine($"HttpStatusCode: {response.HttpStatusCode}");
                    System.Threading.Thread.Sleep(1000);
                }

            }
        }
    }
}
