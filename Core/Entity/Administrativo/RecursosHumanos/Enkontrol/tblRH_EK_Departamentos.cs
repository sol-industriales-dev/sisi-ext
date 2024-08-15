using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Departamentos
    {
        public int id { get; set; }
        public int clave_depto { get; set; }
        public string desc_depto { get; set; }
        public string direccion { get; set; }
        public string cc { get; set; }
        public int? reg_patronal { get; set; }
        public int? cta { get; set; }
        public int? scta { get; set; }
        public int? sscta { get; set; }
        public Int64? id_identificador { get; set; }
        public decimal? porc_isn { get; set; }
        public int? clave_estado { get; set; }
        public int? id_zona_economica { get; set; }
        public string registro_obra { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
    }
}
