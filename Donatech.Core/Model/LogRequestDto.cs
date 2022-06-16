namespace Donatech.Core.Model
{
    public class LogRequestDto
    {
        public long Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime FchRequest { get; set; }
        public string? Username { get; set; }
    }
}
