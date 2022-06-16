namespace Donatech.Core.ViewModels
{
    public class ReporteDonacionViewModel
    {
        public DateTime? FiltroFchInicio { get; set; }
        public DateTime? FiltroFchTermino { get; set; }
        public List<(string, int)>? ReportData { get; set; }
    }
}
