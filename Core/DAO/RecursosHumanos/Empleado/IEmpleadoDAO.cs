using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Empleado
{
    public interface IEmpleadoDAO
    {
        List<sn_empleadosDTO> ObtenerEmpleados();
        tblRH_EK_Empleados ObtenerEmpleadoPorClave(int clave_empleado);
    }
}
