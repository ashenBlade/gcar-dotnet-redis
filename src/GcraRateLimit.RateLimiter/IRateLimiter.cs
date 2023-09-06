namespace GcraRateLimit.RateLimiter;

public interface IRateLimiter
{
    /// <summary>
    /// Получить доступ у ограничителя запросов для указанного ключа
    /// </summary>
    /// <param name="key">Ключ для определения ресурса получения доступа</param>
    /// <param name="token">Токен отмены</param>
    /// <returns><c>true</c> - доступ получен, <c>false</c> - иначе</returns>
    /// <exception cref="OperationCanceledException">Токен отменен</exception>
    public Task<bool> TryGetAccess(string key, CancellationToken token = default);
}