using System.ComponentModel.DataAnnotations;

namespace AskAgainApi.Models.DTO.Session.Request
{
    public class SessionSettingsUpdateDTO
    {
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        [Required]
        public int TimeOffset { get; set; }

        public string? HelloText { get; set; }

        [Required]
        public bool SyncPollResults { get; set; }

        [Required]
        public bool ShowPollOnEnd { get; set; }

        public string? PullResultFormat { get; set; }

        [Required]
        public bool ShowQR { get; set; }

    }
}
