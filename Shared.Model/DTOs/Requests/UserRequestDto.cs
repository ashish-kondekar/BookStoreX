namespace Shared.Models.DTOs.Requests;

public record UserRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Role
);