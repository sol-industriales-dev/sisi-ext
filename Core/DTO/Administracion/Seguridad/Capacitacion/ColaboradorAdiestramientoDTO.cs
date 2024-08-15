using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.Seguridad.Capacitacion;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.DTO.Administracion.Seguridad.Capacitacion
{
    public class ColaboradorAdiestramientoDTO
    {
        public int orden { get; set; }
        public int id { get; set; }
        public int colaborador { get; set; }
        public string colaboradorDesc { get; set; }
        public string estatus_empleado { get; set; }
        public string cc { get; set; }
        public int equipo { get; set; }
        public string equipoDesc { get; set; }
        public int equipoAdiestramiento_id { get; set; }
        public string equipoAdiestramientoDesc { get; set; }
        public int actividad_id { get; set; }
        public string actividadDesc { get; set; }
        public DateTime fechaInicio { get; set; }
        public string fechaInicioString { get; set; }
        public DateTime fechaTermino { get; set; }
        public string fechaTerminoString { get; set; }
        public int adiestrador { get; set; }
        public string adiestradorDesc { get; set; }
        public bool liberado { get; set; }
        public string rutaSoporteAdiestramiento { get; set; }
        public bool puedeEvaluar { get; set; }
        public bool tieneActividad { get; set; }
        public TipoAdiestramientoEnum tipo { get; set; }
        public int actividades { get; set; }
        public decimal horas { get; set; }

        public int instructor { get; set; }
        public string nombreInstructor { get; set; }

        public int seguridad { get; set; }
        public string nombreSeguridad { get; set; }

        public int recursosHumanos { get; set; }
        public string nombreRecursosHumanos { get; set; }

        public int sobrestante { get; set; }
        public string nombreSobrestante { get; set; }

        public int gerenteObra { get; set; }
        public string nombreGerenteObra { get; set; }
    }
}
