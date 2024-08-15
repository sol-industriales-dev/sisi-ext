using Core.DAO.Facturacion.Prefacturacion;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Facturacion.Prefacturacion
{
    public class CapImporteService : ICapImporteDAO
    {
        private ICapImporteDAO f_Importe;

        public ICapImporteDAO Importe
        {
            get { return f_Importe; }
            set { f_Importe = value; }
        }

        public CapImporteService(ICapImporteDAO importe)
        {
            this.f_Importe = importe;
        }

        public tblF_CapImporte saveRepPrefactura(tblF_CapImporte obj)
        {
            return f_Importe.saveRepPrefactura(obj);
        }

        public List<tblF_CapImporte> getImportePorReporte(int id)
        {
            return f_Importe.getImportePorReporte(id);
        }
    }
}
