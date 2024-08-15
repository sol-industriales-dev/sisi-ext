using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Subcontratistas
{
    public class DocumentoSubcontratistaDTO
    {
        public int id { get; set; }
        public string clave { get; set; }
        public string nombre { get; set; }
        public bool aplicaFechaVencimiento { get; set; }
        public int mesesNotificacion { get; set; }
        //1 - Fisica; 2 - Moral;
        public int tipo { get; set; }
        public bool opcional { get; set; }
        //0 - Pendiente; 1 - Autorizada; 2 - Rechazada/Sin informacion
        public int validacion { get; set; }
        public bool aplica { get; set; }
        public string comentarioNoAplica { get; set; }
        public int archivoCargadoID { get; set; }
    }
}
