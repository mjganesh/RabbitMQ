// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ;
using System.Text;

Console.WriteLine("RabbitMQ!");



for (int i = 1; i <= 10; i++)
{
    Book book = new()
    {
        SiNo = i,
        BookName = "Rabbit MQ",
        Price = 100,
        Publisher = "MJ Publications",
        Year = 2022,
        Author = "Ganesh",
    };
    SendMessage(book);
    Console.WriteLine($"published=>{book.Id}-SiNo-{book.SiNo }");
}

ReceiveMessage();
Console.ReadKey();

static void SendMessage<T>(T message)
{
    //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
    var factory = new ConnectionFactory
    {
        HostName = "localhost"
    };

    //Create the RabbitMQ connection using connection factory details as i mentioned above
    var connection = factory.CreateConnection();

    //Here we create channel with session and model
    using var channel = connection.CreateModel();

    //declare the queue after mentioning name and a few property related to that
    channel.QueueDeclare("book", exclusive: false);

    //Serialize the message
    var json = JsonConvert.SerializeObject(message);
    var body = Encoding.UTF8.GetBytes(json);

    //put the data on to the  queue
    channel.BasicPublish(exchange: "book", routingKey: "book", body: body);
}




static void ReceiveMessage()
{
    //Here we specify the Rabbit MQ Server. 
    var factory = new ConnectionFactory
    {
        HostName = "localhost"
    };

    //Create the RabbitMQ connection using connection factory details as i mentioned above
    var connection = factory.CreateConnection();

    //Here we create channel with session and model
    using var channel = connection.CreateModel();

    //declare the queue after mentioning name and a few property related to that
    channel.QueueDeclare("book", exclusive: false);

    //Set Event object which listen message from chanel which is sent by producer
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, eventArgs) =>
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        Console.WriteLine($" message received: {message}");
    };

    //read the message
    channel.BasicConsume(queue: "book", autoAck: true, consumer: consumer);

    Console.ReadKey();
}