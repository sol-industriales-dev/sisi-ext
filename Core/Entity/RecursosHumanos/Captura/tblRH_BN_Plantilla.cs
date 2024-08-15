using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_BN_Plantilla
    {
        public int id { get; set; }
        public string cc { get; set; }
        public string ccNombre { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int usuarioCapturoID { get; set; }
        public virtual tblP_Usuario usuarioCapturo { get; set; }
        public int estatus { get; set; }
        public int version { get; set; }
        public bool versionActiva { get; set; }
        public virtual ICollection<tblRH_BN_Plantilla_Det> listDetalle { get; set; }
        public virtual ICollection<tblRH_BN_Plantilla_Aut> listAutorizadores { get; set; }
    }
}
