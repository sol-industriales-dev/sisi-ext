using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Enkontrol
{
    public class RelRegPatronalesDTO
    {
        public int id { get; set; }
        public int clave_reg_pat { get; set; }
        public string desc_reg_pat { get; set; }
        public string direccion { get; set; }
        public string colonia { get; set; }
        public string localidad { get; set; }
        public int clave_cuidad { get; set; }
        public int clave_estado { get; set; }
        public int codigo_postal { get; set; }
        public int zona_economica { get; set; }
        public string giro { get; set; }
        public int cve_shcp { get; set; }
        public int num_shcp { get; set; }
        public string rfc_cia { get; set; }
        public string expediente_infonavit { get; set; }
        public string registro_patronal { get; set; }
        public string registro_patronal_eventual { get; set; }
        public int cuenta_estatal { get; set; }
        public string nombre_representante { get; set; }
        public string rfc_representante { get; set; }
        public int guia { get; set; }
        public int id_identificador { get; set; }
        public string nombre_corto { get; set; }
        #region PROPS REPORT
        public string estadoDesc { get; set; }
        public string cuidadDesc { get; set; }
        #endregion
    }
}
