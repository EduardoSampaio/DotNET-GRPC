using Greet;
using Grpc.Core;
using Server;

const int Port = 50051;

Grpc.Core.Server? server = null;

try
{
    server = new Grpc.Core.Server()
    {
        Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) },
        Services = {GreetingService.BindService(new GreetingServiceImpl()) }
    };

    server.Start();
    Console.WriteLine("Server listening on port " + Port);
    Console.ReadKey();
} 
catch (IOException e)
{
    Console.WriteLine("Server failed to start: " + e.Message);
    throw;
}
finally
{
    server?.ShutdownAsync().Wait();
}
