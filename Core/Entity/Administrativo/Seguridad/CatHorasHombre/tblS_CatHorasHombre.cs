using Core.Entity.Principal.Multiempresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Seguridad.CatHorasHombre
{
    public class tblS_CatHorasHombre
    {
        public int id { get; set; }
        public string cc { get; set; }
        //public virtual tblP_CC catCC { get; set; }
        public int clave_depto { get; set; }

        public DateTime fechaInicio { get; set; }
        public int horas { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool esActivo { get; set; }
        public int idEmpresa { get; set; }
        public int idGrupo { get; set; }
    }
}
