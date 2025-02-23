namespace BuildingBlocks.Messaging.Events;

public record AuthRegisterEvent : IntegrationEvent
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}