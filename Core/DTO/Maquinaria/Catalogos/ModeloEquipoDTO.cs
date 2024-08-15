using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class ModeloEquipoDTO
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public int marcaEquipoID { get; set; }
        public string nomCorto { get; set; }
        public int noComponente { get; set; }
        public int? idGrupo { get; set; }
        public string Ruta { get; set; }
        public bool overhaul { get; set; }

        public string marcaDesc { get; set; }
        public string grupoDesc { get; set; }
    }
}
