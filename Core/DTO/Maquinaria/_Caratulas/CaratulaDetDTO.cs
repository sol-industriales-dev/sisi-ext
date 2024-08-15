using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria._Caratulas
{
    public class CaratulaDetDTO
    {
        public int id { get; set; }
        public int idAgrupacion { get; set; }
        public int idGrupo { get; set; }
        public int idModelo { get; set; }
        public bool esActivo { get; set; }
    }
}
