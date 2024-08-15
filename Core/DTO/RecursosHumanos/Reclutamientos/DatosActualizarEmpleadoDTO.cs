using Core.Enum.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.RecursosHumanos.Reclutamientos
{
    public class DatosActualizarEmpleadoDTO
    {
        public string estatus_empleado { get; set; }
        public string clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public DateTime fecha_nac { get; set; }
        public string fecha_nacString { get; set; }
        public string clave_pais_nac { get; set; }
        public string clave_departamento_nac_PERU { get; set; }
        public string clave_estado_nac { get; set; }
        public string str_estado_nac { get; set; }
        public string clave_ciudad_nac { get; set; }
        public string localidad_nacimiento { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime fecha_antiguedad { get; set; }
        public string sexo { get; set; }
        public string rfc { get; set; }
        public string CPCIF { get; set; }
        public string curp { get; set; }
        public string cuspp { get; set; }
        public string requisicion { get; set; }
        public int id_regpat { get; set; }
        public string cc_contable { get; set; }
        public string puesto { get; set; }
        public string duracion_contrato { get; set; }
        public DateTime? fecha_fin { get; set; }
        public int jefe_inmediato { get; set; }
        public int? autoriza { get; set; }
        public int usuario_compras { get; set; }
        public string sindicato { get; set; }
        public string clave_depto { get; set; }
        public string nss { get; set; }
        public string unidad_medica { get; set; }
        public string strUnidad_medica { get; set; }
        public string tipo_formula_imss { get; set; }
        public DateTime fecha_contrato { get; set; }
        public string descripcion_puesto { get; set; }
        public string descripcion { get; set; }
        public string antiguedad { get; set; }
        public string nombreJefeInmediato { get; set; }
        public string puestoJefeInmediato { get; set; }
        public string nombreAutoriza { get; set; }
        public string puestoAutoriza { get; set; }
        public string nombreRegPat { get; set; }
        public string descripcionRegPat { get; set; }
        public string nombreCC { get; set; }
        public string nombreTipoContrato { get; set; }
        public string desc_depto { get; set; }
        public string nombreUsuarioReg { get; set; }
        public string actividades { get; set; }
        public int? banco { get; set; }
        public string strBanco { get; set; }
        public string num_cta_pago { get; set; }
        public string num_cta_fondo_aho { get; set; }
        public int? tipo_nomina { get; set; }
        public int? tabulador { get; set; }
        public string solicita_tarjeta { get; set; }
        public string num_dni { get; set; }
        public string cedula_cuidadania { get; set; }


        ////////////////////////////////

        public string estado_civil { get; set; }
        public string fecha_planta { get; set; }
        public string ocupacion { get; set; }
        public string ocupacion_abrev { get; set; }
        public string num_cred_elector { get; set; }
        public string domicilio { get; set; }
        public string numero_exterior { get; set; }
        public string numero_interior { get; set; }
        public string colonia { get; set; }
        public int? pais_dom { get; set; }
        public string estado_dom { get; set; }
        public string str_estado_dom { get; set; }
        public string ciudad_dom { get; set; }
        public string str_ciudad_dom { get; set; }
        public string codigo_postal { get; set; }
        public string tel_casa { get; set; }
        public string tel_cel { get; set; }
        public string email { get; set; }
        public string tipo_casa { get; set; }
        public string strTipo_casa { get; set; }
        public string tipo_sangre { get; set; }
        public string strTipo_sangre { get; set; }
        public string alergias { get; set; }
        public string parentesco_ben { get; set; }
        public string strParentesco_ben { get; set; } // str para reporte
        public DateTime? fecha_nac_ben { get; set; }
        public string codigo_postal_ben { get; set; }
        public string paterno_ben { get; set; }
        public string materno_ben { get; set; }
        public string nombre_ben { get; set; }
        public int? pais_ben { get; set; }
        public string estado_ben { get; set; }
        public string ciudad_ben { get; set; }
        public string colonia_ben { get; set; }
        public string domicilio_ben { get; set; }
        public string num_ext_ben { get; set; }
        public string num_int_ben { get; set; }
        public string ben_num_dni { get; set; }
        public string en_accidente_nombre { get; set; }
        public string en_accidente_telefono { get; set; }
        public string en_accidente_direccion { get; set; }
        public int idUsuarioEK { get; set; }
        public int? PERU_departamento_ben { get; set; }
        public int? PERU_departamento_dom { get; set; }
        public string usuarioModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public EstatusEnvioCorreoEnum estatusEnvioCorreo { get; set; }
        public int edad { get; set; }
        public int? idCategoria { get; set; }
        public string descCategoria { get; set; }
        public List<string> lstCCOcultar { get; set; } // PERMISO PARA OCULTAR LA PESTAÑA DE TABULADORES A LOS CC QUE ESTEN EN LA TABLA tblRH_REC_OcultarCC
        public string PERU_descDepartamento { get; set; }
        public string PERU_descDepartamento_dom { get; set; }
    }
}