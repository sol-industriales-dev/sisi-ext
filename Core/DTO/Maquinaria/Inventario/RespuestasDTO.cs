using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Inventario
{
    public class RespuestasDTO
    {

        public int GrupoID { get; set; }
        public string Pregunta { get; set; }
        public string DescripcionGrupo { get; set; }
        public string Bueno { get; set; }
        public string Regular { get; set; }
        public string Malo { get; set; }
        public string NoAplica { get; set; }
        public string Observaciones { get; set; }

    }
}
