using Core.Entity.RecursosHumanos.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos
{
    public class IncidenciaEmpDTO
    {
        public int id_incidencia { get; set; }
        public int anio { get; set; }
        public int periodo { get; set; }
        public string cc { get; set; }
        public int tipo_nomina { get; set; }
        public string estatus { get; set; }
        public int empleado_modifica { get; set; }
        public DateTime fecha_modifica { get; set; }
        public int usuario_auto { get; set; }
        public DateTime fecha_auto { get; set; }
        public IncidenciaEmpDTO()
        {

        }
        public IncidenciaEmpDTO(tblRH_BN_Incidencias obj)
        {
            id_incidencia = obj.id_incidencia;
            anio = obj.anio;
            periodo = obj.periodo;
            cc = obj.cc;
            tipo_nomina = obj.tipo_nomina;
            estatus = obj.estatus;
            empleado_modifica = obj.empleado_modifica;
            fecha_modifica = obj.fecha_modifica;
            usuario_auto = obj.idAuth;
            fecha_auto = obj.fechaAuth;
        }
    }
}
