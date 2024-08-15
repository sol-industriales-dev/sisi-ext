using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class EmpleadosDTO
    {
        public int id { get; set; }
        public int idEstatus { get; set; }
        public int clave_empleado { get; set; }
        public string contratable { get; set; }
        public string nombreCompleto { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string sindicato { get; set; }
        public string fecha_nac_string { get; set; }
        public DateTime fecha_nac { get; set; }
        public int clave_pais_nac { get; set; }
        public string paisNac { get; set; }
        public int? clave_departamento_nac_PERU { get; set; }
        public string departamentoNacPERU { get; set; }
        public int clave_estado_nac { get; set; }
        public string estadoNac { get; set; }
        public int clave_ciudad_nac { get; set; }
        public string lugarNac { get; set; }
        public string localidad_nacimiento { get; set; }
        public DateTime fecha_alta { get; set; }
        public string str_fecha_alta { get; set; }
        public string fecha_altaStr { get; set; }
        public DateTime fecha_antiguedad { get; set; }
        public string fecha_antiguedadStr { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string sexo { get; set; }
        public string rfc { get; set; }
        public string CPCIF { get; set; }
        public string curp { get; set; }
        public string cuspp { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public string strPuesto { get; set; }
        public int? idCandidato { get; set; }
        public bool? esCandiReingreso { get; set; } // ඞ
        public bool esPendiente { get; set; }
        public int idEmpresa { get; set; }
        public string dbRegistrado { get; set; }
        public bool esActualizar { get; set; }
        public char estatus_empleado { get; set; }
        public bool esReingresoEmpleado { get; set; }
        public bool esAutorizante { get; set; }
        public int? autoriza { get; set; }
        public bool esComentado { get; set; }
        public string desc_puesto { get; set; }
        public bool estatusEnvioCorreo { get; set; }

        public string id_regpat { get; set; }
        public string desc_reg_pat { get; set; }

        #region CAMPOS EK
        public int puesto { get; set; }
        public string descripcion { get; set; } // PUESTO EN STRING
        public string cc_contable { get; set; }
        public string cc_contableDesc { get; set; }
        public DateTime? fecha_ultimo_reingreso { get; set; }
        public DateTime? fecha_primer_contrato { get; set; }
        #endregion

        #region INFORMACION BANCARIA
        public int? bancoNomina { get; set; }
        public string num_cta_pago { get; set; }
        public string num_cta_fondo_aho { get; set; }
        #endregion

        #region TABULADOR
        public int? tipo_nomina { get; set; }
        public int? tabulador { get; set; }
        #endregion

        #region HISTORIAL CC
        public int cantidadCCHistorial { get; set; }
        #endregion

        #region Bajaspersonal
        public DateTime? fechaBaja { get; set; }
        public string motivoBaja { get; set; }
        public string est_contabilidad { get; set; }
        #endregion

        #region EXPEDIENTE DIGITAL
        public int idExpediente { get; set; }
        #endregion

        #region FOTO EMPLEADO
        public string fotoEmpleado { get; set; }
        #endregion

        #region ES DIANA ⚆_⚆┗|｀O ´|┛
        public bool esDiana { get; set; }
        #endregion

        public string categoriaDescripcion { get; set; }
        public bool? esEvaluacion { get; set; }
    }
}
