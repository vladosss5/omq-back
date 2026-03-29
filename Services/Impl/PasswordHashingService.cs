namespace AskAgainApi.Services.Impl;

/// <inheritdoc cref="IPasswordHashingService"/>
public class PasswordHashingService : IPasswordHashingService
{
    /// <inheritdoc/>
    public string GenerateHash(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    /// <inheritdoc/>
    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}