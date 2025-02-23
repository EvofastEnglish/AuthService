namespace BuildingBlocks.Messaging.Events;

public record AuthRegisterEvent : IntegrationEvent
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}