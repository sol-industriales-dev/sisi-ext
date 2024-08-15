using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.ControlObra
{
    public class tblCO_Actividades
    {
        public int id { get; set; }
        public string actividad { get; set; }
        public decimal cantidad { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string observaciones { get; set; }
        public bool estatus { get; set; }
        public bool actividadPadreRequerida { get; set; }
        public bool actividadTerminada { get; set; }
        public decimal? costo { get; set; }
        public decimal? precioUnitario { get; set; }
        public decimal? importeContratado { get; set; }
        public int? tipoPeriodoAvance { get; set; }

        public int? subcapituloN1_id { get; set; }
        public virtual tblCO_Subcapitulos_Nivel1 subcapitulos_N1 { get; set; }

        public int? subcapituloN2_id { get; set; }
        public virtual tblCO_Subcapitulos_Nivel2 subcapitulos_N2 { get; set; }

        public int? subcapituloN3_id { get; set; }
        public virtual tblCO_Subcapitulos_Nivel3 subcapitulos_N3 { get; set; }

        public int? actividadPadre_id { get; set; }
        public virtual tblCO_Actividades actividadPadre { get; set; }

        public virtual List<tblCO_Actividades_Avance_Detalle> actividadAvances_detalle { get; set; }
        public virtual List<tblCO_Actividades> actividades { get; set; }
        public virtual List<tblCO_Unidades_Actividad> unidadesCostos { get; set; }
        public virtual List<tblCO_Actividades_Facturado_Detalle> actividadFacturado_detalle { get; set; }

    }
}
