using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class GestionModificacionTabuladorDetDTO
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
        public int FK_EsquemaPago { get; set; }
        public DateTime fecha_cambio { get; set; }
        public DateTime fechaAplicaCambio { get; set; }
        public TimeSpan hora { get; set; }
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
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public TipoModificacionEnum tipoModificacion { get; set; }
        public int FK_LineaNegocio { get; set; }
        public string puestoDesc { get; set; }
        public string tipoNominaDesc { get; set; }
        public string lineaNegocioDesc { get; set; }
        public string categoriaDesc { get; set; }

        public string sueldoBase { get; set; }
        public string totalNominal { get; set; }

        public string sueldoBase_Modificacion { get; set; }
        public string complemento_Modificacion { get; set; }
        public string totalNominal_Modificacion { get; set; }
        public string totalMensual_Modificacion { get; set; }

        public string sueldoBase_Anterior { get; set; }
        public string complemento_Anterior { get; set; }
        public string totalNominal_Anterior { get; set; }
        public string totalMensual_Anterior { get; set; }

        public string tipoIncremento { get; set; }
        public string puestoSoloTexto { get; set; }
        public string nombreEmpleado { get; set; }
        public string areaDepartamento { get; set; }
        public string areaDepartamentoDesc { get; set; }
        public EstatusGestionAutorizacionEnum modificacionAutorizada { get; set; }
        public string esquemaPagoDesc { get; set; }
        #endregion
    }
}
