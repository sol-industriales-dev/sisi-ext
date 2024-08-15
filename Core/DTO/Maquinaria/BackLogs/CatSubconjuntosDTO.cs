using Core.Entity.Maquinaria.BackLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.BackLogs
{
    public class CatSubconjuntosDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int idConjunto { get; set; }
        public string conjunto { get; set; }
        public bool esActivo { get; set; }
        public string abreviacion { get; set; }
    }
}
