using Core.DAO.RecursosHumanos.Empleado;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.RH.Empleado;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Enkontrol.RH.Empleado
{
    public class EmpleadoDAO : GenericDAO<tblP_Usuario>, IEmpleadoDAO
    {
        public List<sn_empleadosDTO> ObtenerEmpleados()
        {
//            var query_sn_empleados = new OdbcConsultaDTO();

//            query_sn_empleados.consulta =
//                @"SELECT
//                    clave_empleado,
//                    nombre,
//                    ape_paterno,
//                    ape_materno,
//                    sexo,
//                    tipo_nomina
//                FROM
//                    sn_empleados";

//            return _contextEnkontrol.Select<sn_empleadosDTO>(EnkontrolAmbienteEnum.Rh, query_sn_empleados);

            return _context.Select<sn_empleadosDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Construplan,
                consulta = @"SELECT
                                clave_empleado,
                                nombre,
                                ape_paterno,
                                ape_materno,
                                sexo,
                                tipo_nomina
                            FROM
                                tblRH_EK_Empleados",
            });
        }

        public tblRH_EK_Empleados ObtenerEmpleadoPorClave(int clave_empleado)
        {
            return _context.tblRH_EK_Empleados.FirstOrDefault(x => x.esActivo && x.clave_empleado == clave_empleado);
        }
    }
}
