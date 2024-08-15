using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Enkontrol
{
    public class CatTabuladoresDTO
    {
        public int? id { get; set; }
        public int? tabulador { get; set; }
        public int? puesto { get; set; }
        public string puesto_desc { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal bono_de_zona { get; set; }
        public int? year { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
    }
}
