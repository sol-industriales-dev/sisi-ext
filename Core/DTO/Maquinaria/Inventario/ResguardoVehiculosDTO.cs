using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class ResguardoVehiculosDTO
    {
        public int id { get; set; }
        public int noEmpleado { get; set; }
        public string nombEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Obra { get; set; }
        public int MaquinariaID { get; set; }
        public int Kilometraje { get; set; }
        public int TipoEncierro { get; set; }
        public string Fecha { get; set; }
        public string TarjetaCirculacionArchivo { get; set; }
        public string TarjetaCirculacionRuta { get; set; }
        public string CheckLiberacionArchivo { get; set; }
        public string CheckLiberacionRuta { get; set; }
        public string ManualMMTOPArchivo { get; set; }
        public string ManualMMTOPRuta { get; set; }
        public string PermisoCargaArchivo { get; set; }
        public string PermisoCargaRuta { get; set; }
        public string FormatoResguardoArchivo { get; set; }
        public string FormatoResguardoRuta { get; set; }
        public bool FormatoResguardo { get; set; }
        public string PolizaSegurosArchivo { get; set; }
        public string PolizaSegurosRuta { get; set; }
        public string LicenciaArchivo { get; set; }
        public string LicenciaRuta { get; set; }
        public string fechaVencimiento { get; set; }
        public int estado { get; set; }

        public string Comentario { get; set; }
    }
}
