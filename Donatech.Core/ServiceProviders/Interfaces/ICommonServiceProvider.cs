using System;
using Donatech.Core.Model;

namespace Donatech.Core.ServiceProviders.Interfaces
{
	public interface ICommonServiceProvider
	{
		Task<ResultDto<List<ComunaDto>>> GetListaComunas();
	}
}

