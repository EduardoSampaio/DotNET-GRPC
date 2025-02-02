using Greet;
using Grpc.Core;
using static Greet.GreetingService;

namespace Server;
public class GreetingServiceImpl : GreetingServiceBase
{
    public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
    {
        string result = "Hello " + request.Greeting.FirstName + " " + request.Greeting.LastName;
        return Task.FromResult(new GreetingResponse() { Result = result });
    }

    public override async Task GreetManyTimes(GreetingManyTimesRequest request, IServerStreamWriter<GreetingManyTimesResponse> responseStream, ServerCallContext context)
    {
        string result = "Hello " + request.Greeting.FirstName + " " + request.Greeting.LastName;
        foreach (int i in Enumerable.Range(1, 10))
        {
            context.Status = new Status(StatusCode.OK, "This is a status message");
            await Task.Delay(1000);
            await responseStream.WriteAsync(new GreetingManyTimesResponse() { Result = result });
        }
    }

    public override async Task<LongGreetResponse> LongGreet(IAsyncStreamReader<LongGreetRequest> requestStream, ServerCallContext context)
    {
        string result = "";
        while (await requestStream.MoveNext())
        {
            result += "Hello " + requestStream.Current.Greeting.FirstName + " " + requestStream.Current.Greeting.LastName + "! ";
        }
        return new LongGreetResponse() { Result = result };
    }

    public override async Task GreetEveryone(IAsyncStreamReader<GreetEveryoneRequest> requestStream, IServerStreamWriter<GreetEveryoneResponse> responseStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext())
        {
            var result = "Hello " + requestStream.Current.Greeting.FirstName + " " + requestStream.Current.Greeting.LastName;
            Console.WriteLine("Sending: " + result);
            await Task.Delay(1000);
            await responseStream.WriteAsync(new GreetEveryoneResponse() { Result = result });
        }
    }
}
