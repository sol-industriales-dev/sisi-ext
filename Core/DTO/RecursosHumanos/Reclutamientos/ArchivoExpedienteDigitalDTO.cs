using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class ArchivoExpedienteDigitalDTO
    {
        public int id { get; set; }
        public int expediente_id { get; set; }
        public int archivo_id { get; set; }
        public int archivoCargado_id { get; set; }
        public string rutaArchivo { get; set; }
        public bool aplica { get; set; } //OBSOLETO
        public bool registroActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string fechaCreacionString { get; set; }
        public bool esCargadoDesdeCapacitacionOperativa { get; set; }

        //BANDERA PARA ALIMENTAR TABLA DE PANEL DE ALTA EMPLEADO
        public bool estado { get; set; }
        public string archiveDesc { get; set; }
        public bool? esNoAplica { get; set; }
    }
}
