interface ILogger {
    void Info(string message);
    void Error(string message);
}

public class Logger : ILogger
{
    public void Error(string message)
    {
        Console.Error.WriteLine(message);
    }

    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}