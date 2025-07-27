namespace Shared.Models.DTOs.Responses;

public record UserDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Role
);