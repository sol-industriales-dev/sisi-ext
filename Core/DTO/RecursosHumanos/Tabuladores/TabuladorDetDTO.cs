using Core.Enum.RecursosHumanos.Tabuladores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Tabuladores
{
    public class TabuladorDetDTO
    {
        #region SQL
        public int id { get; set; }
        public int FK_Tabulador { get; set; }
        public int FK_LineaNegocio { get; set; }
        public int FK_Categoria { get; set; }
        public EstatusGestionAutorizacionEnum tabuladorDetAutorizado { get; set; }
        public decimal sueldoBase { get; set; }
        public decimal complemento { get; set; }
        public decimal totalNominal { get; set; }
        public decimal sueldoMensual { get; set; }
        public int FK_EsquemaPago { get; set; }
        public int FK_UsuarioCreacion { get; set; }
        public int FK_UsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        #endregion

        #region ADICIONAL
        public int idPuesto { get; set; }
        public string puestoDesc { get; set; }
        public int cantTabuladores { get; set; }
        public string lineaNegocioDesc { get; set; }
        public string categoriaDesc { get; set; }
        public string esquemaPagoDesc { get; set; }
        public string tipoNominaDesc { get; set; }
        public string areaDepartamentoDesc { get; set; }
        public string sueldoBaseString { get; set; }
        public string complementoString { get; set; }
        public string totalNominalString { get; set; }
        public string sueldoMensualString { get; set; }
        public string esquemaPagoDescString { get; set; }
        public string aumentoPorc { get; set; }
        public string sueldoBaseStringActual { get; set; }
        public string complementoStringActual { get; set; }
        public string totalNominalStringActual { get; set; }
        public string sueldoMensualStringActual { get; set; }
        public string sueldoBaseStringModificacion { get; set; }
        public string complementoStringModificacion { get; set; }
        public string totalNominalStringModificacion { get; set; }
        public string sueldoMensualStringModificacion { get; set; }
        public string categoriaDescModificacion { get; set; }
        public int contadorIndex { get; set; }
        public string nombreEmpleado { get; set; }
        public int FK_AreaDepartamento { get; set; }
        public string descAreaDepartamento { get; set; }
        public List<string> lstCC { get; set; }
        public string cc { get; set; }
        public int FK_Puesto { get; set; }
        public string tabuladorDetAutorizadoDesc { get; set; }
        public string personalNecesario { get; set; }
        public List<string> lstLineasNegocios { get; set; }
        public List<string> lstCategorias { get; set; }
        public List<string> lstSueldosBases { get; set; }
        public List<string> lstComplementos { get; set; }
        public List<string> lstTotalNominal { get; set; }
        public List<string> lstSueldoMensual { get; set; }
        public string descSindicato { get; set; }
        public string soloDescPuesto { get; set; }
        public string esquemaPagoDescModificacion { get; set; }
        #endregion
    }
}