using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Captura
{
    public class tblRH_FormatoCambio
    {
        #region Informacion General
        public int id { get; set; }
        public int Clave_Empleado { get; set; }
        public string Nombre { get; set; }
        public string Ape_Paterno { get; set; }
        public string Ape_Materno { get; set; }
        public DateTime Fecha_Alta { get; set; }
        public int PuestoID { get; set; }
        public string Puesto { get; set; }
        public int TipoNominaID { get; set; }
        public string TipoNomina { get; set; }
        public string CcID { get; set; }
        public string CC { get; set; }
        public int RegistroPatronalID { get; set; }
        public string RegistroPatronal { get; set; }
        public int Clave_Jefe_Inmediato { get; set; }
        public string Nombre_Jefe_Inmediato { get; set; }
        public decimal Salario_Base { get; set; }
        public decimal Complemento { get; set; }
        #endregion  
        public string folio { get; set; }
        public int InicioNomina { get; set; }
        public DateTime FechaInicioCambio { get; set; }
        public string Justificacion { get; set; }
        public string CamposCambiados { get; set; }
        public bool Aprobado { get; set; }
        public bool Rechazado { get; set; }
        public int usuarioCap { get; set; }
        public string nomUsuarioCap { get; set; }
        public bool editable { get; set; }
        public decimal Bono { get; set; }
        public DateTime fechaCaptura { get; set; }
        public int? idLineaNegocios { get; set; }
        public string descLineaNegocios { get; set; }
        public int? idCategoria { get; set; }
        public string descCategoria { get; set; }
        //public string Genero { get; set; }
        //public DateTime FechaNac { get; set; }
        //public Nullable<decimal> ClaveCdNac { get; set; }
        //public Nullable<decimal> ClaveEstadoNac { get; set; }
        //public Nullable<decimal> ClavePaisNac { get; set; }
        //public string Rfc { get; set; }
        //public string Curp { get; set; }
        //public string EstatusEmpleado { get; set; }
        //public string ClaveDepto { get; set; }
        //public Nullable<decimal> ClaveRegistro { get; set; }
        //public string Sindicato { get; set; }
        //public Nullable<decimal> ClaveSalario { get; set; }
        //public Nullable<decimal> ClaveJornada { get; set; }
        //public string RecibePtu { get; set; }
        //public string DeclaraIsr { get; set; }
        //public Nullable<decimal> EstadoDeclara { get; set; }
        //public string HonorarioAsimilable { get; set; }
        //public Nullable<decimal> ClaveTurno { get; set; }
        //public Nullable<decimal> Banco { get; set; }
        //public Nullable<decimal> NumCtaPago { get; set; }
        //public Nullable<decimal> NumCtaFondoAho { get; set; }
        //public string Nss { get; set; }
        //public Nullable<decimal> UnidadMedica { get; set; }
        //public string TipoFormulaImss { get; set; }
        //public int DiasPeriodo { get; set; }
        //public string GrupoImss { get; set; }
        //public Nullable<decimal> SdiInfonavit { get; set; }
        //public Nullable<decimal> BaseVarImss { get; set; }
        //public Nullable<decimal> BaseVarInf { get; set; }
        //public string Codigo { get; set; }
        //public string IdseAltas { get; set; }
        //public string IdseBajas { get; set; }
        //public string IdseCambios { get; set; }
        //public Nullable<decimal> Numpro { get; set; }
        //public Nullable<decimal> SueldoNeto { get; set; }
        //public string RecibeDespensa { get; set; }
        //public string Contratable { get; set; }
        //public string JefeCuadrilla { get; set; }
        //public string AsistDiaria { get; set; }
        //public string SubsidioEmpleo { get; set; }
        //public Nullable<decimal> TipoCuentaPago { get; set; }
        //public Nullable<decimal> Tabulador { get; set; }
        //public Nullable<decimal> DuracionContrato { get; set; }
        //public Nullable<decimal> Solicitud { get; set; }
        //public string TipoFirma { get; set; }
        //public decimal Requisicion { get; set; }
        //public int ArchivoEnviado { get; set; }
        //public Nullable<int> IdContratoEmpleado { get; set; }
        //public string LocalidadNacimiento { get; set; }
        //public int FormatoContrato { get; set; }
        //public string DescPuesto { get; set; }
        //public Nullable<decimal> Autoriza { get; set; }
        //public Nullable<decimal> VistoBueno { get; set; }
        //public Nullable<decimal> UsuarioCompras { get; set; }
        //public string ClaveInterbancaria { get; set; }
        //public Nullable<DateTime> FechaContrato { get; set; }
        //public Nullable<int> IdExpediente { get; set; }
        public decimal SalarioAnt { get; set; }
        public decimal ComplementoAnt { get; set; }
        public decimal BonoAnt { get; set; }
        public string CCAntID { get; set; }
        public string CCAnt { get; set; }
        public string PuestoAnt { get; set; }
        public string RegistroPatronalAnt { get; set; }
        public string Nombre_Jefe_InmediatoAnt { get; set; }
        public string TipoNominaAnt { get; set; }
        public string Departamento { get; set; }
        public int? ClaveDepartamento { get; set; }
        public string DepartamentoAnterior { get; set; }
        public int? ClaveDepartamentoAnterior { get; set; }
        public int? idLineaNegociosAnterior { get; set; }
        public string descLineaNegociosAnterior { get; set; }
        public int? idCategoriaAnterior { get; set; }
        public string descCategoriaAnterior { get; set; }
        [NotMapped]
        public decimal lowerBase { get; set; }
        [NotMapped]
        public decimal lowerComplemento { get; set; }
        [NotMapped]
        public int? idTabulador { get; set; }
        [NotMapped]
        public int? idTabuladorDet { get; set; }
        [NotMapped]
        public bool esRango { get; set; }
    }
}
