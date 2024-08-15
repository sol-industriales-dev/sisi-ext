using Core.Enum.Contabilidad.Propuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta.Validacion
{
    public class ArchivoRutaDTO
    {
        public TipoArchivoEnum tipoArchivo { get; set; }
        public string ruta { get; set; }
    }
}
