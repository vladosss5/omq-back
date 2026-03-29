namespace AskAgainApi.Models.DTO.Session.Response
{
    public class SessionSettingsResponseDTO
    {
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public int TimeOffset { get; set; }
        public string HelloText { get; set; }
        public bool SyncPollResults { get; set; }
        public bool ShowPollOnEnd { get; set; }
        public string PullResultFormat { get; set; }
        public bool ShowQR { get; set; }

    }
}
