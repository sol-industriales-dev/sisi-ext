using Core.DAO.Facturacion.Prefacturacion;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Facturacion.Prefacturacion
{
    public class CapImporteDAO : GenericDAO<tblF_CapImporte>, ICapImporteDAO
    {
        public tblF_CapImporte saveRepPrefactura(tblF_CapImporte obj)
        {
            try
            {
                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.CAPIMPORTES);
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.CAPIMPORTES);
                }

            }
            catch (Exception e)
            {
                return new tblF_CapImporte();
            }
            return obj;
        }

        public List<tblF_CapImporte> getImportePorReporte(int id)
        {
            var lstResutl = _context.tblF_CapImporte
                .Where(x => x.idReporte == id);
            return lstResutl.ToList();
        }
    }
}
