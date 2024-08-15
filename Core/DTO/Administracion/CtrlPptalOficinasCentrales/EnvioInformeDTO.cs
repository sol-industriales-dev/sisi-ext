using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Administrativo.CtrlPptalOficinasCentrales;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class EnvioInformeDTO
    {
        #region SQL
        public int id { get; set; }
        public int anio { get; set; }
        public int idMes { get; set; }
        public int idEmpresa { get; set; }
        public int idCC { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public decimal cumplimientoMensual { get; set; }
        public int area { get; set; }
        public int cuenta { get; set; }
        public bool aplicaAditiva { get; set; }
        public decimal totalAditivas { get; set; }
        public int numAditivasAutorizadas { get; set; }
        public int numDeductivasAutorizadas { get; set; }
        public int numAditivasPendientes { get; set; }
        public bool aplicaInforme { get; set; }
        public List<tblAF_CtrlPptalOfCe_Mensaje> mensajes { get; set; }
        #endregion

        #region ADICIONAL
        public string mes { get; set; }
        public string cc { get; set; }
        public string descripcionCC { get; set; }
        public int envioInforme { get; set; }
        public string empresa { get; set; }
        #endregion
    }
}
