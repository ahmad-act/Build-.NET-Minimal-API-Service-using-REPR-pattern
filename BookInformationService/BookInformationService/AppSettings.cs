namespace BookInformationService;

public class AppSettings
{
    public string EmailSubject { get; set; }
    public string FromEmail { get; set; }
    public string ToEmail { get; set; }
    public string MailServer { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string EmailPassword { get; set; }
}

