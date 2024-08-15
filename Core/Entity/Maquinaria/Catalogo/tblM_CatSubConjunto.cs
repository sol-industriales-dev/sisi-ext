using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enum.Maquinaria;

namespace Core.Entity.Maquinaria.Catalogo
{
    public class tblM_CatSubConjunto
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string posicionID { get; set; }
        public bool hasPosicion { get; set; }
        public bool  estatus { get; set; }
        public int conjuntoID { get; set; }
        public string prefijo { get; set; }
        public virtual tblM_CatConjunto conjunto { get; set; }
    }
}
