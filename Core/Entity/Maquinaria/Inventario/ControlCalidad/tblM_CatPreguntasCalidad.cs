using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario.ControlCalidad
{
    public class tblM_CatPreguntasCalidad
    {
        public int Id {get;set;}
        public int IdGrupo {get;set;}
        public string Pregunta {get;set;}
        public int TipoPregunta {get;set;}
        public bool Estatus {get;set;}

    }
}
