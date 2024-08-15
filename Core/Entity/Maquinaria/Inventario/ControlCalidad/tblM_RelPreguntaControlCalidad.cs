using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Maquinaria.Inventario.ControlCalidad
{
    public class tblM_RelPreguntaControlCalidad
    {
        public int id {get;set;}
        public int IdControl {get;set;}
        public int IdPregunta {get;set;}
        public int Respuesta {get;set;}
        public int Cantidad {get;set;}
        public string Marca {get;set;}
        public string Serie {get;set;}
        public string Medida {get;set;}
        public string VidaUtil { get; set; }

    }
}
