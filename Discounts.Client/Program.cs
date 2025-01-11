// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
Console.WriteLine("Press 'q' to quit.");

//using var channel = GrpcChannel.ForAddress("https://localhost:7184");

//var client = new Discounts.Client.Discounts.DiscountsClient(channel);

//var response = client.GenerateCode(new Discounts.Client.GenerateCodeRequest { Count = 50, Length = 7 });

//foreach (var code in response.Codes)
//{
//    Console.WriteLine(code);
//}

while (true)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Q)
        {
            break;
        }
    }

    await Task.Delay(1);
}