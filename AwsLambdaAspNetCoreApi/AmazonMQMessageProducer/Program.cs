using System;
using Microsoft.Extensions.Configuration;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Threading.Tasks;

namespace AmazonMQMessageProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true).Build();

            if (string.IsNullOrEmpty(configuration["username"]) 
                || string.IsNullOrEmpty(configuration["password"]) 
                || string.IsNullOrEmpty(configuration["broker_uri"])
                || configuration["username"] == "<your-username>" 
                || configuration["password"] == "<your-password>"
                || configuration["broker_uri"] == "<your-borker-uri>")
            {
                Console.WriteLine("you must provide valid values for username, password and broker_uri properties in the appsettings.json file");
                return;
            }
            
            Uri connecturi = new Uri(configuration["broker_uri"]);
            IConnectionFactory factory = new ConnectionFactory(connecturi);
            using (IConnection connection = factory.CreateConnection(configuration["username"], configuration["password"])) 
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = session.GetQueue("dotnet-queue");
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    connection.Start();

                    for (int i = 0; i < 300; i++)
                    {
                        Console.WriteLine(String.Format("My AWSome message {0}", i));
                        IMessage message = session.CreateTextMessage(String.Format("My AWSome message {0}.", i));
                        
                        producer.Send(message);
                        System.Threading.Thread.Sleep(1000);
                    }

                    connection.Close();
                }
                
            }
        }
    }
}
