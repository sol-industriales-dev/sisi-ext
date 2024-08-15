using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Estados
    {
        public int id { get; set; }
        public int clave_pais { get; set; }
        public int? clave_departamento { get; set; }
        public int clave_estado { get; set; }
        public string descripcion { get; set; }
        public string desc_edo_curp { get; set; }
        public int? clave_shcp { get; set; }
        public decimal? porc_isn { get; set; }
        public decimal? porc_educ { get; set; }
        public decimal? porc_turismo { get; set; }
        public decimal? porc_ecologia { get; set; }
        public int? id_zona_economica { get; set; }
        public int? ind_precargado { get; set; }
    }
}
