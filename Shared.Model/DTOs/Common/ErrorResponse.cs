using System.Net;

namespace Shared.Models.DTOs.Common;

public record ErrorResponse(string Message, HttpStatusCode Code);
