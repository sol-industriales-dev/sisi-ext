using Core.DAO.RecursosHumanos.Empleado;
using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.General.RH.Empleado
{
    public class EmpleadoService : IEmpleadoDAO
    {
        private IEmpleadoDAO _empleadoDAO;

        private IEmpleadoDAO EmpleadoDAO
        {
            get { return _empleadoDAO; }
            set { _empleadoDAO = value; }
        }

        public EmpleadoService(IEmpleadoDAO empleado)
        {
            this.EmpleadoDAO = empleado;
        }

        public List<sn_empleadosDTO> ObtenerEmpleados()
        {
            return this.EmpleadoDAO.ObtenerEmpleados();
        }

        public tblRH_EK_Empleados ObtenerEmpleadoPorClave(int clave_empleado)
        {
            return this.EmpleadoDAO.ObtenerEmpleadoPorClave(clave_empleado);
        }
    }
}
