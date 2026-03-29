namespace AskAgainApi.Settings;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UsersCollectionName { get; set; } = null!;
    public string SessionsCollectionName { get; set; } = null!;
    public string TariffsCollectionName { get; set; } = null!;
    public string PollResultCollectionName { get; set; } = null!;
}

