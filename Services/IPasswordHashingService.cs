namespace AskAgainApi.Services;

/// <summary>
/// Сервис хеширования паролей.
/// </summary>
public interface IPasswordHashingService
{
    /// <summary>
    /// Сгенерировать хеш. 
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <returns>Хеш пароля.</returns>
    public string GenerateHash(string password);

    /// <summary>
    /// Проверка пароля с хешем.
    /// </summary>
    /// <param name="password">Пароль.</param>
    /// <param name="passwordHash">Хеш.</param>
    /// <returns></returns>
    public bool Verify(string password, string passwordHash);
}