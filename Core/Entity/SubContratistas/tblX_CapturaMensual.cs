using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Subcontratistas;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity.SubContratistas
{
    public class tblX_CapturaMensual
    {
        public int id { get; set; }
        public int archivoMensualID { get; set; }
        public DateTime fechaCaptura { get; set; }
        public bool actualizacion { get; set; }
        public int contratoPeriodoID { get; set; }
        public bool estatus { get; set; }

        [ForeignKey("contratoPeriodoID")]
        public virtual tblX_ContratoPeriodo periodo { get; set; }

        [ForeignKey("archivoMensualID")]
        public virtual tblX_ArchivoMensual archivoMensual { get; set; }
    }
}
