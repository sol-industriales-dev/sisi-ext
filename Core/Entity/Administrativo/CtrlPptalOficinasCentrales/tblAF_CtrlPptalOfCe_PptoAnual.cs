using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.CtrlPptalOficinasCentrales
{
    public class tblAF_CtrlPptalOfCe_PptoAnual
    {
        public int id { get; set; }
        public int pptoInicialID { get; set; }
        public int idCC { get; set; }
        public string cc { get; set; }
        public int anio { get; set; }
        public string nombrePresupuesto { get; set; }
        public bool notificado { get; set; }
        public bool terminado { get; set; }
        public bool autorizado { get; set; }
        public int autorizante1 { get; set; }
        public int autorizante2 { get; set; }
        public int autorizante3 { get; set; }
        public string comentarioRechazo { get; set; }
        public DateTime? fechaAutorizacion1 { get; set; }
        public DateTime? fechaAutorizacion2 { get; set; }
        public DateTime? fechaAutorizacion3 { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }

        [ForeignKey("presupuestoId")]
        public virtual List<tblAF_CtrlAutorizacionPresupuesto> autorizaciones { get; set; }
    }
}
