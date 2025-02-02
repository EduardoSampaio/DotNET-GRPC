using Blog;
using Greet;
using Grpc.Core;
using server;
using Server;

Grpc.Core.Server? server = null;

try
{
    var servercert = File.ReadAllText("ssl/server.crt");
    var serverkey = File.ReadAllText("ssl/server.key");
    var caCert = File.ReadAllText("ssl/ca.crt");

    var keypair = new KeyCertificatePair(servercert, serverkey);
    var sslCredentials = new SslServerCredentials(new List<KeyCertificatePair> { keypair }, caCert, false);

    server = new Grpc.Core.Server()
    {
        Ports = { new ServerPort("localhost", 50051, sslCredentials) },
        Services = {
            GreetingService.BindService(new GreetingServiceImpl()), 
            BlogService.BindService(new BlogServiceImpl())
        }
    };

    server.Start();
    Console.WriteLine("Server listening on port " + 50051);
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
