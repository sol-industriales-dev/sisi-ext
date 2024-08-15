using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ControlEnvioRecepcionDTO
    {

        public string Lugar { get; set; }



        public string TipoControl { get; set; }

        public string Economico { get; set; }
        public string Fecha { get; set; }
        public string Clase { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Notas { get; set; }
        public string HorometroEnvio { get; set; }
        public string HorometroRecepcion { get; set; }
        public string KmEnvio { get; set; }
        public string KmRecepcion { get; set; }
        public string Tanque1 { get; set; }
        public string Tanque2 { get; set; }
        public string ReporteFalla { get; set; }
        public string CopiaFactura { get; set; }
        public string ControlCalidad { get; set; }

        public string CatPartes { get; set; }
        public string ManualServicios { get; set; }

        public string ManualOperacion { get; set; }
        public string ManualMantto { get; set; }
        public string Bitacora { get; set; }
        public string Placas { get; set; }
        public string CompaniaTransporte { get; set; }
        public string ResponsableTransporte { get; set; }
        public string Transporte { get; set; }
        public string NombreEnvio { get; set; }
        public string CompaniaEnvio { get; set; }
        public string NombreRecepcion { get; set; }
        public string CompaniaRecepcion { get; set; }
        public string FechaEmbarque { get; set; }
        public string FechaRecepcion { get; set; }
        public string DiasTransalado { get; set; }
        public string Aduana { get; set; }
    }
}
