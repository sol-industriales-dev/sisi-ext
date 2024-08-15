using Core.Enum.Administracion.Seguridad.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.Indicadores
{
    public class tblS_IncidentesMedidasControl
    {
        public int id { get; set; }
        public int responsable_id { get; set; }
        public string accionPreventiva { get; set; }
        public string responsableNombre { get; set; }
        public DateTime fechaCump { get; set; }
        public int estatus { get; set; }
        public PrioridadActividadEnum prioridad { get; set; }
        public int usuarioID { get; set; }

        public int incidente_id { get; set; }
        public virtual tblS_Incidentes Incidente { get; set; }

    }
}
