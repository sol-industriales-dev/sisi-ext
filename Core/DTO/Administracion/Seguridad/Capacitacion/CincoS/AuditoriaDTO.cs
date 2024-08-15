using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DTO.Administracion.Seguridad.Capacitacion.CincoS
{
    public class AuditoriaDTO
    {
        #region SQL
        public int id { get; set; }
        public int checkListId { get; set; }
        public string cc { get; set; }
        public DateTime fecha { get; set; }
        public int usuario5sId { get; set; }
        public bool auditoriaCompleta { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int usuarioCreacionId { get; set; }
        public int usuarioModificacionId { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public List<AuditoriaDetDTO> lstAuditoriaDet { get; set; }
        public string nombreAuditoria { get; set; }
        public int usuarioAuditorId { get; set; }
        public string auditor { get; set; }
        public int areaId { get; set; }
        public string area { get; set; }
        public decimal porcCumplimiento { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFinal { get; set; }
        public string fechaStr { get; set; }
        #endregion
    }
}
