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
                || string.IsNullOrEmpty(configuration["destination_type"])
                || configuration["username"] == "<your-username>" 
                || configuration["password"] == "<your-password>"
                || configuration["broker_uri"] == "<your-borker-uri>")
            {
                Console.WriteLine("you must provide valid values for username, password, broker_uri and destination_type properties in the appsettings.json file");
                return;
            }
            
            Uri connecturi = new Uri(configuration["broker_uri"]);
            IConnectionFactory factory = new ConnectionFactory(connecturi);
            using (IConnection connection = factory.CreateConnection(configuration["username"], configuration["password"])) 
            using (ISession session = connection.CreateSession())
            {
                IDestination destination = null;

                if(configuration["destination_type"] == "topics")
                {
                    destination = session.GetTopic("VirtualTopic.<your-topic-name>");
                }
                else
                {
                    destination = session.GetQueue("<your-queue-name>");
                }
    
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
