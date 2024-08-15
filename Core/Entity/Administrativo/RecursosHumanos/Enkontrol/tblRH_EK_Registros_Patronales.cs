using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Registros_Patronales
    {
        [Key]
        public int clave_reg_pat { get; set; }
        public string desc_reg_pat { get; set; }
        public string direccion { get; set; }
        public string colonia { get; set; }
        public string localidad { get; set; }
        public int clave_cuidad { get; set; }
        public int clave_estado { get; set; }
        public int clave_pais { get; set; }
        public int codigo_postal { get; set; }
        public int zona_economica { get; set; }
        public string giro { get; set; }
        public int? cve_shcp { get; set; }
        public int? num_shcp { get; set; }
        public string rfc_cia { get; set; }
        public string expediente_infonavit { get; set; }
        public string registro_patronal { get; set; }
        public string registro_patronal_eventual { get; set; }
        public Int64? cuenta_estatal { get; set; }
        public string nombre_representante { get; set; }
        public string rfc_representante { get; set; }
        public int? guia { get; set; }
        public Int64? id_identificador { get; set; }
        public string nombre_corto { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
