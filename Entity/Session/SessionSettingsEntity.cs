namespace AskAgainApi.Entity.Session
{
    public class SessionSettingsEntity
    {
        public string Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; } = null;
        public int TimeOffset { get; set; } = 0;
        public string? HelloText { get; set; } = string.Empty;
        public bool SyncPollResults { get; set; } = false;
        public bool ShowPollOnEnd { get; set; } = false;
        public string PullResultFormat { get; set; } = string.Empty;
        public bool ShowQR { get; set; } = false;

    }
}
