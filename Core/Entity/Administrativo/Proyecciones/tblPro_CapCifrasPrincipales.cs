using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_CapCifrasPrincipales
    {

        public int id { get; set; }
        public int MesInicio { get; set; }
        public int  ejercicioAnio { get; set; }
        public decimal VentaProyectadaAnioAnterior { get; set; }
        public decimal VentaProyectadaMesActual { get; set; }
        public decimal VentaProyectadaAlAnio { get; set; }
        public decimal VentaRealAnioAnterior { get; set; }
        public decimal VentaRealMesActual { get; set; }
        public decimal VentaRealProyectdaAlAnio { get; set; }
        public decimal UtilidadPlaneadaAnioAnterior { get; set; }
        public decimal UtilidadPlaneadaMesActual { get; set; }
        public decimal UtilidadPlaneadaAnioActual { get; set; }
        public decimal UtilidadRealAnioAnterior { get; set; }
        public decimal UtilidadRealMesActual { get; set; }
        public decimal UtilidadRealAnioActual { get; set; }
        public bool estatus { get; set; }
        public string escenarios { get; set; }

    }
}
