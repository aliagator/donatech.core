namespace Donatech.Core.Model
{
    public class MailRequestDto
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<IFormFile> Attachments { get; set; } = null!;
    }
}
