using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario
{
    public class tblM_CatPreguntaResguardoVehiculo
    {
        public int id { get; set; }
        public int GrupoID { get; set; }
        public string DescripcionGrupo { get; set; }
        public string Pregunta { get; set; }
        public int TipoPregunta { get; set; }
        public bool Estatus { get; set; }
    }
}
