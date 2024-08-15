using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.FacultamientosDpto
{

    public class tblFA_Grupos
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public virtual List<tblFA_PlantillatblFA_Grupos> plantillasFaDepartamento { get; set; }
        public virtual List<tblP_CC> listaCC { get; set; }
    }
}
