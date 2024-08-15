using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.GestorArchivos
{
    public class EstructuraVistasDTO
    {

        public string title { get; set; }
        public bool selected { get; set; }
        public bool partsel { get; set; }
        public bool expanded { get; set; }
        public int id { get; set; }
        public int key { get; set; }
        public bool folder { get; set; }
        public List<EstructuraVistasDTO> children { get; set; }
        public bool checkbox { get; set; }
        public bool unselectable { get; set; }
        public PermisosDTO permisos { get; set; }

    }
}
