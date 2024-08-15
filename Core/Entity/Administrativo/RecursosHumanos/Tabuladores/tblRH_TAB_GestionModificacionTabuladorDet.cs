using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Tabuladores
{
    public class tblRH_TAB_GestionModificacionTabuladorDet
    {
        #region SQL
        public int id { get; set; }
        public int FK_IncrementoAnual { get; set; }
        public int FK_Puesto { get; set; }
        public int porcentajeIncremento { get; set; }
        public int clave_empleado { get; set; }
        public int tabulador { get; set; }
        public int tabulador_anterior { get; set; }
        public int FK_Tabulador { get; set; }
        public int FK_TabuladorDet { get; set; }
        public int FK_Categoria { get; set; }
        public EstatusGestionAutorizacionEnum tabuladorDetAutorizado { get; set; }
        public int FK_LineaNegocio { get; set; }
        public int FK_EsquemaPago { get; set; }
        public DateTime? fecha_cambio { get; set; }
        public DateTime? fechaAplicaCambio { get; set; }
        public TimeSpan? hora { get; set; }
        public decimal salario_base { get; set; }
        public decimal complemento { get; set; }
        public decimal suma { get; set; }
        public decimal totalMensual { get; set; }
        public decimal bono_zona { get; set; }
        public int motivoCambio { get; set; }
        public EstatusGestionAutorizacionEnum registroAplicado { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion
    }
}
