using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface IMensajeServiceProvider
	{
		Task<ResultDto<List<MensajeDto>>> GetListaMensajesByFilter(FilterMensajeDto filter);
		Task<ResultDto<bool>> InsertMessage(MensajeDto mensaje);
	}
}

