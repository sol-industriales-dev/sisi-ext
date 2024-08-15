using Core.Enum.Administracion.Seguridad.MedioAmbiente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.Seguridad.MedioAmbiente
{
    public class ArchivoDTO
    {
        public int id { get; set; }
        public int idCaptura { get; set; }
        public string nombreArchivo { get; set; }
        public int tipoArchivo { get; set; }
        public string tipoArchivoDesc { get; set; }
    }
}
