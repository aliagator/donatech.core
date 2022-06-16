using Donatech.Core.Model;
using Donatech.Core.Model.DbModels;
using Donatech.Core.ServiceProviders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Donatech.Core.ServiceProviders
{
    public class ReporteServiceProvider : IReporteServiceProvider
    {
        private readonly DonatechDBContext _dbContext;

        public ReporteServiceProvider(DonatechDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultDto<List<ProductoDto>>> ReporteDonaciones(FiltroLogRequestDto filtroReporte)
        {
            try
            {
                var result = await _dbContext.Productos.Include("IdTipoNavigation").Where(p =>
                (p.FchPublicacion >= filtroReporte.FchInicio || filtroReporte.FchInicio == null) &&
                (p.FchPublicacion <= filtroReporte.FchTermino || filtroReporte.FchTermino == null)
                ).Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Descripcion = p.Descripcion,
                    Enabled = p.Enabled,
                    Estado = p.Estado,
                    FchFinalizacion = p.FchFinalizacion,
                    FchPublicacion = p.FchPublicacion,
                    IdDemandante = p.IdDemandante,
                    IdOferente = p.IdOferente,
                    IdTipo = p.IdTipo,
                    Titulo = p.Titulo,
                    TipoProducto = new TipoProductoDto
                    {
                        Id = p.IdTipoNavigation.Id,
                        Descripcion = p.IdTipoNavigation.Descripcion
                    }
                }).ToListAsync();

                return new ResultDto<List<ProductoDto>>(result);
            }
            catch(Exception ex)
            {
                return new ResultDto<List<ProductoDto>>(error: new ResultError($"Error inesperado al obtener el reporte", ex));
            }
        }

        public async Task<ResultDto<List<LogRequestDto>>> ReporteVisitas(FiltroLogRequestDto filtroReporte)
        {
            try
            {
                var result = await _dbContext.LogRequests.Where(p =>
                (p.FchRequest >= filtroReporte.FchInicio || filtroReporte.FchInicio == null) &&
                (p.FchRequest <= filtroReporte.FchTermino || filtroReporte.FchTermino == null)
                ).Select(p => new LogRequestDto
                {
                    Id = p.Id,
                    FchRequest = p.FchRequest,
                    Url = p.Url,
                    Username = p.Username
                }).ToListAsync();

                return new ResultDto<List<LogRequestDto>>(result);
            }
            catch (Exception ex)
            {
                return new ResultDto<List<LogRequestDto>>(error: new ResultError($"Error inesperado al obtener el reporte", ex));
            }
        }
    }
}
