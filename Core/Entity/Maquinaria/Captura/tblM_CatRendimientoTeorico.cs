using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CatRendimientoTeorico
    {
        public int id { get; set; }
        public string bajo { get; set; }
        public string medio { get; set; }
        public bool estatus { get; set; }
        public DateTime fecha { get; set; }
        public string alto { get; set; }
        public int modeloEquipoID { get; set; }
        public virtual tblM_CatModeloEquipo modeloEquipo { get; set; }

    }
}
