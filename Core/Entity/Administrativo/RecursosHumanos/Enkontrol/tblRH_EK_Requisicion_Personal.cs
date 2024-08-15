using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Requisicion_Personal
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int puesto { get; set; }
        public int cantidad_solicitada { get; set; }
        public int solicitante { get; set; }
        public DateTime fecha_solicitante { get; set; }
        public int visto_bueno { get; set; }
        public DateTime fecha_visto_bueno { get; set; }
        public string estatus{ get; set; }
        public int jefe_inmediato { get; set; }
        public int razon_solicitud { get; set; }
        public int sustituye { get; set; }
        public DateTime fecha_baja { get; set; }
        public DateTime fecha_de_contratacion { get; set; }
        public string justificacion { get; set; }
        public int tipo_contrato { get; set; }
        public int edad { get; set; }
        public string sexo { get; set; }
        public int estado_civil { get; set; }
        public int presentacion { get; set; }
        public string escolaridad { get; set; }
        public string especialidad { get; set; }
        public int experiencia { get; set; }
        public string horario { get; set; }
        public string habilidades { get; set; }
        public string actividades { get; set; }
        public string conocimientos { get; set; }
        public int altas { get; set; }
        public int bajas { get; set; }
        public int id_plantilla { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
