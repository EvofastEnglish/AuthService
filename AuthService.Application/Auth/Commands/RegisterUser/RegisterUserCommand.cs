namespace AuthService.Application.Auth.Commands.RegisterUser;

public record RegisterUserCommand(RegisterUserRequest UserRequest) : ICommand<RegisterUserResult>;
public record RegisterUserRequest(string FirstName, string LastName, string Email, string Username, string Password);
public record RegisterUserResult(bool IsSuccess);