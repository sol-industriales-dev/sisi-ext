using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.SubContratistas
{
    public class tblX_RelacionSubContratistaDocumentacion
    {
        public int id { get; set; }
        public int subContratistaID { get; set; }
        public int documentacionID { get; set; }
        public string rutaArchivo { get; set; }
        public DateTime? fechaVencimiento { get; set; }
        public DateTime? fechaSolicitud { get; set; }
        public bool aplica { get; set; }
        public string justificacionOpcional { get; set; }
        public string justificacionValidacion { get; set; }
        public ArchivoValidacionEnum validacion { get; set; }
        public string rutaArchivoValidacion { get; set; }
        public DateTime? fechaCaptura { get; set; }
        public DateTime? fechaValidacion { get; set; }
        public bool estatus { get; set; }
    }
}
