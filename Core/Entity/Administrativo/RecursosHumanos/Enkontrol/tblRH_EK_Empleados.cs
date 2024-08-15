using Core.Entity.RecursosHumanos.Reclutamientos;
using Core.Enum.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.RecursosHumanos.Enkontrol
{
    public class tblRH_EK_Empleados
    {
        public int id { get; set; }
        public int clave_empleado { get; set; }
        public string nombre { get; set; }
        public string ape_paterno { get; set; }
        public string ape_materno { get; set; }
        public string sexo { get; set; }
        public DateTime? fecha_nac { get; set; }
        public int? clave_ciudad_nac { get; set; }
        public int? clave_estado_nac { get; set; }
        public int? clave_departamento_nac_PERU { get; set; }
        public int? clave_pais_nac { get; set; }
        public string rfc { get; set; }
        public string curp { get; set; }
        public string CPCIF { get; set; } //constancia de identificación fiscal
        public string cuspp { get; set; }
        public string estatus_empleado { get; set; }
        public string clave_depto { get; set; }
        public int? tipo_nomina { get; set; }
        public int? clave_registro { get; set; }
        public string sindicato { get; set; }
        public int? clave_salario { get; set; }
        public string tipo_salario { get; set; }
        public int? clave_jornada { get; set; }
        public string recibe_ptu { get; set; }
        public string declara_isr { get; set; }
        public int? estado_declara { get; set; }
        public string honorario_asimilable { get; set; }
        public decimal? prima_vac { get; set; }
        public decimal? vac_pagadas { get; set; }
        public int? clave_turno { get; set; }
        public int? id_zona_economica { get; set; }
        //public string pagodestajo { get; set; }
        public int? clave_destajista { get; set; }
        public string forma_pago { get; set; }
        public int? banco { get; set; }
        public string num_cta_pago { get; set; }
        public string num_cta_fondo_aho { get; set; }
        public decimal? cuota_aho { get; set; }
        //public int clave_aho { get; set; }
        public int? clave_afore { get; set; }
        public int? area { get; set; }
        public int? clave_nivel { get; set; }
        public int? puesto { get; set; }
        public int? idTabuladorDet { get; set; }
        public int? clave_categoria { get; set; }
        public string nss { get; set; }
        public int? unidad_medica { get; set; }
        public int? id_regpat { get; set; }
        public string tipo_formula_imss { get; set; }
        public DateTime? fecha_antiguedad { get; set; }
        public DateTime? fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
        public int? razon_baja { get; set; }
        public decimal? dias_periodo { get; set; }
        public decimal? salario_periodo { get; set; }
        public DateTime? fecha_cambio_sal { get; set; }
        public decimal? salario_diario { get; set; }
        public decimal? salario_hora { get; set; }
        public decimal? sdi_imss { get; set; }
        public string grupo_imss { get; set; }
        public decimal? sdi_infonavit { get; set; }
        public DateTime? fecha_cambio_sdi { get; set; }
        public decimal? base_var_imss { get; set; }
        public decimal? base_var_inf { get; set; }
        public string codigo { get; set; }
        public int? clave_credito_infonavit { get; set; }
        public DateTime? fecha_alta_infonavit { get; set; }
        public DateTime? fecha_baja_infonavit { get; set; }
        public int? clave_registro_fonacot { get; set; }
        public int? idse_altas { get; set; }
        public int? idse_bajas { get; set; }
        public DateTime? fecha_proc { get; set; }
        public DateTime? year_proc { get; set; }
        public int? bim_proc { get; set; }
        public int? mes_proc { get; set; }
        public int? tipo_nomina_proc { get; set; }
        public int? tipo_periodo_proc { get; set; }
        public int? periodo_proc { get; set; }
        //public string statuc_proc { get; set; }
        public decimal? porc_incremento { get; set; }
        public int? id_contable { get; set; }
        public int? numpro { get; set; }
        public decimal? sueldo_prof { get; set; }
        public decimal? sueldo_neto { get; set; }
        public decimal? sobresueldo { get; set; }
        public string recibe_despensa { get; set; }
        public string contratable { get; set; }
        public int? suc_cta_pago { get; set; }
        public DateTime? fecha_hora_log { get; set; }
        public int? usuario_log { get; set; }
        public DateTime? alta_hora_log { get; set; }
        public string cc_contable { get; set; }
        public string foto_dir { get; set; }
        public string jefe_cuadrilla { get; set; }
        public string asist_diaria { get; set; }
        //public string subsidio_empleado { get; set; }
        public int? tipo_cuenta_pago { get; set; }
        //public string orignen_log { get; set; }
        public int? tabulador { get; set; }
        public string baja_comentarios { get; set; }
        public int? duracion_contrato { get; set; }
        public DateTime? fecha_fin { get; set; }
        public int? solicitud { get; set; }
        public int? jefe_inmediato { get; set; }
        public string tipo_firma { get; set; }
        public int? requisicion { get; set; }
        public bool? archivo_enviado { get; set; }
        public int? id_contrato_empleado { get; set; }
        public string localidad_nacimiento { get; set; }
        public int? formato_contrato { get; set; }
        public string desc_puesto { get; set; }
        public int? autoriza { get; set; }
        public int? visto_bueno { get; set; }
        public int? usuario_compras { get; set; }
        public decimal? salario_base { get; set; }
        public decimal? complemento { get; set; }
        public decimal? bono_zona { get; set; }
        public string clabe_interbancaria { get; set; }
        public DateTime? fecha_contrato { get; set; }
        public int? id_expediente { get; set; }
        public string solicita_tarjeta { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public bool esActivo { get; set; }
        public int estatusEnvioCorreo { get; set; }

        [ForeignKey("requisicion")]
        public virtual tblRH_REC_Requisicion requisicionEntity { get; set; }
    }
}

