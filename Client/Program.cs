using Dummy;
using Greet;
using Grpc.Core;

const string target = "127.0.0.1:50051";

var channel = new Channel(target, ChannelCredentials.Insecure);

channel.ConnectAsync().ContinueWith((task) =>
{
    if (task.Status == TaskStatus.RanToCompletion)
    {
        Console.WriteLine("The client connected successfully");
    }
}).Wait();
 
//var client = new DummyService.DummyServiceClient(channel);
var client = new GreetingService.GreetingServiceClient(channel);

//var response = client.Greet(new GreetingRequest
//{
//    Greeting = new Greeting
//    {
//        FirstName = "John",
//        LastName = "Doe"
//    }
//});

var greeting = new Greeting
{
    FirstName = "John",
    LastName = "Doe"
};

var request = new LongGreetRequest
{
    Greeting = greeting
};

var stream = client.GreetEveryone();

var responseTask = Task.Run(async () =>
{
    while (await stream.ResponseStream.MoveNext())
    {
        Console.WriteLine("Result: " + stream.ResponseStream.Current.Result);
    }
});

foreach (int i in Enumerable.Range(1, 10))
{
    Console.WriteLine("Sending message " + i);
    await stream.RequestStream.WriteAsync(new GreetEveryoneRequest { Greeting = greeting });
    await Task.Delay(1000);
}

//foreach (int i in Enumerable.Range(1, 10))
//{
//    Console.WriteLine("Sending message " + i);
//    await stream.RequestStream.WriteAsync(request);
//    await Task.Delay(1000);
//}

//await stream.RequestStream.CompleteAsync(); 
//var response = await stream.ResponseAsync;

//Console.WriteLine("Response: " + response.Result);


//while (response.ResponseStream.MoveNext().Result)
//{
//    Console.WriteLine("Result: " + response.ResponseStream.Current.Result);
//}



channel.ShutdownAsync().Wait();
Console.ReadKey();
