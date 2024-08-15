using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class MaquinaFiltrosDTO
    {
        public int idTipo { get; set; }
        public int idGrupo { get; set; }
        public string descripcion { get; set; }
        public int estatus { get; set; }
        public string noEconomico { get; set; }
        public string ccId { get; set; }
        public DateTime Fecha { get; set; }
        public List<string> ListCC { get; set; }
    }
}
