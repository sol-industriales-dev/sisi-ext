using Core.DTO.Administracion.ControlInterno.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Administracion.ControlInterno.Reporte
{
    public interface IRepTrapasoDAO
    {
        List<RepTraspasoDTO> getLstMovCerrados(string cc, string folio, string almacen,DateTime fechaIni,DateTime fechaFin);
        List<RepTraspasoDTO> getLstMovAbiertos(string cc, string folio, string almacen, DateTime fechaIni, DateTime fechaFin);
    }
}
