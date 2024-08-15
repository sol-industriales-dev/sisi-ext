using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_AutorizacionSolicitudReemplazo
    {
        public int id { get; set; }
        public int solicitudReemplazoEquipoID { get; set; }
        public int idAutorizaElabora { get; set; }
        public int idAutorizaGerente { get; set; }
        public int idAutorizaAsigna { get; set; }

        public bool AutorizaElabora { get; set; }
        public bool AutorizaGerente { get; set; }
        public bool AutorizaAsigna { get; set; }

        public string CadenaElabora { get; set; }
        public string CadenaGerente { get; set; }
        public string CadenaAsigna { get; set; }

        public DateTime FechaAutorizacion { get; set; }
        public DateTime FechaElaboracion { get; set; }
        public string Comentarios { get; set; }

        public virtual tblM_SolicitudReemplazoEquipo SolicitudReemplazoEquipo { get; set; }

    }
}
