using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Nomina
{
    public class tblC_Nom_PreNomina_Aut
    {
        public int id { get; set; }
        public int prenominaID { get; set; }
        //public virtual tblC_Nom_Prenomina prenomina { get; set; }
	    public int aprobadorClave { get; set; }
        //public virtual tblP_Usuario aprobador { get; set; }
	    public string aprobadorNombre { get; set; }
	    public string aprobadorPuesto { get; set; }
	    public string tipo { get; set; }
	    public int estatus { get; set; }
	    public string firma { get; set; }
	    public bool autorizando { get; set; }
	    public int orden { get; set; }
	    public string comentario { get; set; }
        public DateTime? fecha { get; set; }
        public bool esObra { get; set; }
    }
}
