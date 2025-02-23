using AuthService.Domain.Models;
using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Application.Auth.Commands.RegisterUser;

public class RegisterUserHandler
    (UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint)
    : ICommandHandler<RegisterUserCommand, RegisterUserResult>
{
    public async Task<RegisterUserResult> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = userManager.FindByNameAsync(command.UserRequest.Username).Result;
        if (user == null)
        {
            user = new ApplicationUser()
            {
                FirstName = command.UserRequest.FirstName,
                LastName = command.UserRequest.LastName,
                UserName = command.UserRequest.Username,
                Email = command.UserRequest.Email,
                EmailConfirmed = true
            };
            var result = userManager.CreateAsync(user, command.UserRequest.Password).Result;
            await userManager.AddToRoleAsync(user, "User");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            var eventMessage = user.Adapt<AuthRegisterEvent>();
            eventMessage.UserId = user.Id;
            eventMessage.Username = user.UserName;
            await publishEndpoint.Publish(eventMessage, cancellationToken);        
        }

        return new RegisterUserResult(true);    }
}