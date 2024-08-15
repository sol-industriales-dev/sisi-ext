using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class PlanMaestroDTO
    {
        public int id { get; set; }
        public int anio { get; set; }
        public int idCC { get; set; }
        public string cc { get; set; }
        public string misionArea { get; set; }
        public string objEspecificoMedible { get; set; }
        public string meta { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public List<MedicionIndicadorDTO> lstMedicionesIndicadores { get; set; }
        public List<RNAgrupacionDTO> lstAgrupacionesRN { get; set; }
        public List<RNConceptoDTO> lstConceptosRN { get; set; }
        public bool autorizado { get; set; }
        public bool notificado { get; set; }
    }
}
