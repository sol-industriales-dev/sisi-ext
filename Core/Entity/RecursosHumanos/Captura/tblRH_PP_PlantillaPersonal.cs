using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_PP_PlantillaPersonal
    {
        public int id { get; set; }
        public string ccID { get; set; }
        public string cc { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int usuarioID { get; set; }
        public virtual tblP_Usuario usuario { get; set; }
        public DateTime fechaMod { get; set; }
        public int estatus { get; set; }
        public virtual ICollection<tblRH_PP_PlantillaPersonal_Det> listDetalle { get; set; }
        public virtual ICollection<tblRH_PP_PlantillaPersonal_Aut> listAutorizadores { get; set; }
        public int? plantillaEKID { get; set; }
        public int? tabuladorEKID { get; set; }
    }
}
