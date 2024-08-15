using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Tipos_Nomina
    {
        [Key]
        public int tipo_nomina { get; set; }
        public string descripcion { get; set; }
        public decimal num_dias { get; set; }
        public string control { get; set; }
        public decimal? factor_faltas { get; set; }
        public bool st_bloq_inic { get; set; }
        public int? usuario_bloq_inic { get; set; }
        public string dias_mes { get; set; }
        public string tipo_calculo_imss { get; set; }
        public decimal factor_septimo { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool? esActivo { get; set; }
    }
}
