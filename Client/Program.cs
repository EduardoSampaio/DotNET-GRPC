using Blog;
using Dummy;
using Greet;
using Grpc.Core;
using System;

var clientCert = File.ReadAllText("ssl/client.crt");
var clientKey = File.ReadAllText("ssl/client.key");
var caCert = File.ReadAllText("ssl/ca.crt");

var channelCredentials = new SslCredentials(caCert, new KeyCertificatePair(clientCert, clientKey));

var channel = new Channel("localhost", 50051, channelCredentials);

await channel.ConnectAsync().ContinueWith((task) =>
{
    if (task.Status == TaskStatus.RanToCompletion)
    {
        Console.WriteLine("The client connected successfully");
    }
});

var client = new BlogService.BlogServiceClient(channel);
//create blog
//var response = client.CreateBlog(new CreateBlogRequest
//{
//    Blog = new Blog.Blog
//    {
//        AuthorId = "John",
//        Title = "My First Blog",
//        Content = "Content of the first blog"
//    }
//});

//Console.WriteLine("Blog created: " + response.Blog.Id);

//read blog
var response = client.ReadBlog(new ReadBlogRequest { Id = "679fa4921f026df1242f5267" });
Console.WriteLine("Blog created: " + response);

channel.ShutdownAsync().Wait();
Console.ReadKey();


//var response = client.Greet(new GreetingRequest
//{
//    Greeting = new Greeting
//    {
//        FirstName = "John",
//        LastName = "Doe"
//    }
//});

//var greeting = new Greeting
//{
//    FirstName = "John",
//    LastName = "Doe"
//};

//var request = new LongGreetRequest
//{
//    Greeting = greeting
//};

//var stream = client.GreetEveryone();

//await Task.Run(async () =>
//{
//    while (await stream.ResponseStream.MoveNext())
//    {
//        Console.WriteLine("Result: " + stream.ResponseStream.Current.Result);
//    }
//});

//foreach (int i in Enumerable.Range(1, 10))
//{
//    Console.WriteLine("Sending message " + i);
//    await stream.RequestStream.WriteAsync(new GreetEveryoneRequest { Greeting = greeting });
//    await Task.Delay(1000);
//}

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


