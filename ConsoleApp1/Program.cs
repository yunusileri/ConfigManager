using ConfigManager.Api;

var redisConnectionString = "localhost:6379";
var reader = new ConfigurationReader("Service1", redisConnectionString, "amqp://guest:guest@localhost:5672/");

while (true)
{
    try
    {
        var value = reader.GetValue<string>("key1");
        Console.WriteLine(value);
        
        var value2 = reader.GetValue<int>("key2");
        Console.WriteLine(value2);
        
        var value3 = reader.GetValue<bool>("key3");
        Console.WriteLine(value3);
        
        var value4 = reader.GetValue<bool>("key4");
        Console.WriteLine(value4);
    }
    catch (Exception e)
    {
        Console.WriteLine("Veri yok.");
    }
    finally
    {
        Thread.Sleep(2000);
    }
}


Console.ReadKey();