﻿namespace Gateway.Api.Services;

public interface ITokenService
{
    string GenerateToken(string username, string role, string tenantId);
}
