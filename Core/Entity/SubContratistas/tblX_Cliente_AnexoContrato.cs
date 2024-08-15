using Core.Enum.ControlObra;
using Core.Enum.Subcontratistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_Cliente_AnexoContrato
    {
        public int id { get; set; }
        public int contratoID { get; set; }
        public TipoAnexoEnum tipoAnexo { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime? fechaCaptura { get; set; }
        public bool estatus { get; set; }
    }
}
