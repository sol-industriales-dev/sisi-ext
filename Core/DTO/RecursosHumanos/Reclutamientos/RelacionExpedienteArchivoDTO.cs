using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class RelacionExpedienteArchivoDTO
    {
        #region TABLA SQL
        public int id { get; set; }
        public int expediente_id { get; set; }
        public int archivo_id { get; set; }
        public string rutaArchivo { get; set; }
        public int usuarioCreacion_id { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int usuarioModificacion_id { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int clave_empleado { get; set; }
        public string imageToBase64 { get; set; }
        #endregion
    }
}
