using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Desempeno
{
    public class tblRH_ED_DetMetas
    {
        public int id { get; set; }
        public int idProceso { get; set; }
        public int idUsuario { get; set; }
        public int idJefe { get; set; }
        public string nombre { get; set; }
        public int tipo { get; set; }
        public string descripcion { get; set; }
        public decimal peso { get; set; }
        public bool esVobo { get; set; }
        public bool notificado { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
        [ForeignKey("idProceso")]
        public virtual tblRH_ED_CatProceso proceso { get; set; }
        [ForeignKey("tipo")]
        public virtual tblRH_ED_CatEstrategia estrategia { get; set; }
        [ForeignKey("idUsuario")]
        public virtual tblP_Usuario usuario { get; set; }
        [ForeignKey("idJefe")]
        public virtual tblP_Usuario jefe { get; set; }
        [ForeignKey("idMeta")]
        public virtual List<tblRH_ED_DetObservacion> observaciones { get; set; }
    }
}
