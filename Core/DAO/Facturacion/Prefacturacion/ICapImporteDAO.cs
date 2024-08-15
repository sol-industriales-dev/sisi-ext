using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Facturacion.Prefacturacion
{
    public interface ICapImporteDAO
    {
        tblF_CapImporte saveRepPrefactura(tblF_CapImporte obj);
        List<tblF_CapImporte> getImportePorReporte(int id);
    }
}
