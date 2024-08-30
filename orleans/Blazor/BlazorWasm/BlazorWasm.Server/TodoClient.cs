namespace BlazorWasm.Server;

public interface ITodoClient
{
}

public class TodoClient : ITodoClient
{
    public Guid Instance = Guid.NewGuid();

    public TodoClient()
    {

    }
}


public interface IActionClient
{
}

public class WebClient : IActionClient
{
    public Guid Instance = Guid.NewGuid();

    public WebClient()
    {

    }
}

public class OrleansClient : IActionClient
{
    public Guid Instance = Guid.NewGuid();

    public OrleansClient()
    {

    }
}
