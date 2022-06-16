using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
    public interface IReporteServiceProvider
    {
        Task<ResultDto<List<LogRequestDto>>> ReporteVisitas(FiltroLogRequestDto filtroReporte);
        Task<ResultDto<List<ProductoDto>>> ReporteDonaciones(FiltroLogRequestDto filtroReporte);
    }
}
