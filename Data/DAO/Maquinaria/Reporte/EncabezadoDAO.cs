using Core.DAO.Maquinaria.Reporte;
using Core.DTO;
using Core.Entity.Maquinaria.Reporte;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte
{
    public class EncabezadoDAO : GenericDAO<tblP_Encabezado>, IEncabezadoDAO
    {
        public  tblP_Encabezado getEncabezadoDatos()
        {
            return _context.tblP_Encabezado.FirstOrDefault(w => w.id.Equals((int)vSesiones.sesionEmpresaActual));            
        }

        public tblP_Encabezado getEncabezadoDatosCplan()
        {
            return _context.tblP_Encabezado.FirstOrDefault(w => w.id == 1);
        }
    }
}
