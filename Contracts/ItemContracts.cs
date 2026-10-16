namespace DemoApi.Contracts;

public record ItemRequest(string Name);
public record ItemResponse(int Id, string Name, DateTime Created);