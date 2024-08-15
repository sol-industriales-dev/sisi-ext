using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Administracion.Seguridad.Capacitacion;

namespace Core.Entity.Administrativo.Seguridad.Capacitacion
{
    public class tblS_CapacitacionIHHColaboradorCapacitacion
    {
        public int id { get; set; }
        public string cc { get; set; }
        public int area { get; set; }
        public int empresa { get; set; }
        public int equipo { get; set; }
        public int equipoAdiestramiento_id { get; set; }
        public DateTime fechaCaptura { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaTermino { get; set; }
        public int colaborador { get; set; }
        public int adiestrador { get; set; }
        public int instructor { get; set; }
        public int seguridad { get; set; }
        public int recursosHumanos { get; set; }
        public int sobrestante { get; set; }
        public int gerenteObra { get; set; }
        public TipoAdiestramientoEnum tipo { get; set; }
        public int actividad_id { get; set; }
        public bool liberado { get; set; }
        public string rutaSoporteAdiestramiento { get; set; }
        public int division { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("colaboradorCapacitacionId")]
        public virtual List<tblS_CapacitacionIHHControlActividad> actividades { get; set; }
    }
}
