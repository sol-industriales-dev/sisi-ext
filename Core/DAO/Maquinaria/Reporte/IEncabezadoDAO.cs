using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Reporte
{
    public interface IEncabezadoDAO
    {
       tblP_Encabezado getEncabezadoDatos();
       tblP_Encabezado getEncabezadoDatosCplan();
    }
}
