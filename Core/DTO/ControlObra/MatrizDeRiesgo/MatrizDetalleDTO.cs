using Core.Entity.ControlObra.MatrizDeRiesgo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.ControlObra.MatrizDeRiesgo
{
    public class MatrizDetalleDTO
    {
        public int id { get; set; }
        public DateTime fechaElaboracion { get; set; }
        public string cc { get; set; }
        public string nombreDelProyecto { get; set; }
        public string personalElaboro { get; set; }
        public string faseDelProyecto { get; set; }
        public bool estatus { get; set; }
        public List<MatrizDetDTO> lstMatrizDeRiesgo { get; set; }
        public List<tblCO_MR_ImpractosSobreObjetivosDelProyecto> lstImpacto { get; set; }

    }
}
