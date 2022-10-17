// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;

Console.WriteLine("Hello, I'm a Mailer Console Application!");

var connectionString = "Endpoint=sb://irfestivalservicebussm.servicebus.windows.net/;SharedAccessKeyName=listener;SharedAccessKey=EJ1Zz3Iui5GeJokhVRlP2RWppoq2bezOXOlqBdqVcwU=;EntityPath=mails";
var queueName = "mails";

await using (var client = new ServiceBusClient(connectionString))
{
    var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();

    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("Stopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}

static async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Mail to send: {body}");

    await args.CompleteMessageAsync(args.Message);
}

static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
