using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_ResguardoVehiculosServicio
    {
        public int id { get; set; }
        public int noEmpleado { get; set; }
        public string nombEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Obra { get; set; }
        public int MaquinariaID { get; set; }
        public decimal Kilometraje { get; set; }
        public int TipoEncierro { get; set; }
        public DateTime Fecha { get; set; }
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
        public DateTime fechaVencimiento { get; set; }
        public DateTime fechaVencimientoPoliza { get; set; }
        public int estado { get; set; }
        public string Comentario { get; set; }
        public string Placas { get; set; }
        public string DocumentoFirmadoRuta { get; set; }
        public string DocumentoFirmado { get; set; }

        public string DocumentoAnexo { get; set; }
        public string DocumentoAnexoRuta { get; set; }

        public DateTime? fechaVigenciaCurso { get; set; }
    }
}
