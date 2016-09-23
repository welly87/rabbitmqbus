using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Samples.StepByStep.Server";
            var endpointConfiguration = new EndpointConfiguration("Samples.StepByStep.Server");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseTransport<RabbitMQTransport>();
            endpointConfiguration.UseTransport<RabbitMQTransport>().ConnectionString("host=localhost");
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MutateIncomingMessages>(DependencyLifecycle.InstancePerCall);
                });
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            try
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpointInstance.Stop()
                    .ConfigureAwait(false);
            }
        }
    }
}
