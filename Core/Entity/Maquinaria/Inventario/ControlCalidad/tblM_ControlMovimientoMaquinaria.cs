using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario.ControlCalidad
{
    public class tblM_ControlMovimientoMaquinaria
    {
        public int id { get; set; }
        public int tipoControl { get; set; }
        public int noEconomico { get; set; }
        public string lugar { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public string nota { get; set; }
        public decimal horometros { get; set; }
        public int kilometraje { get; set; }
        public DateTime fechaRecepcionEmbarque { get; set; }
        public int diasTranslado { get; set; }
        public int tanque1 { get; set; }
        public int tanque2 { get; set; }
        public bool pedAduana { get; set; }
        public bool controlCalidad { get; set; }
        public bool bitacora { get; set; }
        public bool placas { get; set; }
        public bool ReporteFalla { get; set; }
        public bool copiaFactura { get; set; }
        public bool manualMant { get; set; }
        public bool manualOperacion { get; set; }
        public string companiaTransporte { get; set; }
        public string responsableTrasnporte { get; set; }
        public string Transporte { get; set; }
        public string nombreResponsable { get; set; }
        public string compañiaResponsable { get; set; }
        public string nombreResponsableEnvio { get; set; }
        public string compañiaResponsableEnvio { get; set; }
        public string nombreResponsableRecepcion { get; set; }
        public string compañiaResponsableRecepcion { get; set; }
        public string firma { get; set; }
        public int solicitudEquipoID { get; set; }
        public int asignacionEquipoId { get; set; }
        public string Nombre { get; set; }
        public string RutaArchivo { get; set; }
        public virtual tblM_SolicitudEquipo SolicitudEquipo { get; set; }
        public string lugarRecepcion { get; set; }
    }
}
