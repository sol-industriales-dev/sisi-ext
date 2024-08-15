using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class EstimadoRealMensualDTO
    {
        #region SQL
        public int id { get; set; }
        public int anio { get; set; }
        public decimal estimadoReal_Enero { get; set; }
        public decimal estimadoReal_Febrero { get; set; }
        public decimal estimadoReal_Marzo { get; set; }
        public decimal estimadoReal_Abril { get; set; }
        public decimal estimadoReal_Mayo { get; set; }
        public decimal estimadoReal_Junio { get; set; }
        public decimal estimadoReal_Julio { get; set; }
        public decimal estimadoReal_Agosto { get; set; }
        public decimal estimadoReal_Septiembre { get; set; }
        public decimal estimadoReal_Octubre { get; set; }
        public decimal estimadoReal_Noviembre { get; set; }
        public decimal estimadoReal_Diciembre { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}