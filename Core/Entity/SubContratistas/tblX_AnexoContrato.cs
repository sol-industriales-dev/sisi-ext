using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Subcontratistas;
using Core.Enum.ControlObra;

namespace Core.Entity.SubContratistas
{
    public class tblX_AnexoContrato
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public TipoAnexoEnum tipoAnexo { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime? fechaCaptura { get; set; }
        public bool estatus { get; set; }
    }
}
