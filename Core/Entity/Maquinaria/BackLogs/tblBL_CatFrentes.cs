using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entity.Maquinaria.BackLogs
{
    public class tblBL_CatFrentes
    {
        public int id { get; set; }
        public string nombreFrente { get; set; }
        public int idUsuarioAsignado { get; set; }
        public virtual tblP_Usuario UsuarioCreacion { get; set; }
        //public string cc { get; set; }
        //public string folioPpto { get; set; }
        //public decimal avance { get; set; }
        //public DateTime? fechaAsignacion { get; set; }
        //public DateTime? fechaPromesa { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fecheModificacion { get; set; }
        public bool esActivo { get; set; }
       
    }
}
