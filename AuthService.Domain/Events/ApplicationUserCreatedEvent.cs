using AuthService.Domain.Abstractions;
using AuthService.Domain.Models;

namespace AuthService.Domain.Events;

public class ApplicationUserCreatedEvent(ApplicationUser WordSet) : IDomainEvent;