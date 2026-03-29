namespace AskAgainApi.Settings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = null!;
        public string Port { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
