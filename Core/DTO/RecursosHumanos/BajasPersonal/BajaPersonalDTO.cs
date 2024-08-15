using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.RecursosHumanos.BajasPersonal;

namespace Core.DTO.RecursosHumanos.BajasPersonal
{
    public class BajaPersonalDTO
    {
        #region BAJA DE PERSONAL
        public int id { get; set; }
        public int numeroEmpleado { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public int idPuesto { get; set; }
        public string nombrePuesto { get; set; }
        public string habilidadesConEquipo { get; set; }
        public string telPersonal { get; set; }
        public bool tieneWha { get; set; }
        public int strTieneWha { get; set; }
        public string telCasa { get; set; }
        public string contactoFamilia { get; set; }
        public int? idDepartamento { get; set; } // PERU
        public int idEstado { get; set; }
        public int idCiudad { get; set; }
        public int idMunicipio { get; set; }
        public string direccion { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string correo { get; set; }
        public DateTime fechaBaja { get; set; }
        public string motivoBajaDeSistema { get; set; }
        public string motivoSeparacionDeEmpresa { get; set; }
        public bool regresariaALaEmpresa { get; set; }
        public int strRegresariaALaEmpresa { get; set; }
        public string porqueRegresariaALaEmpresa { get; set; }
        public string dispuestoCambioDeProyecto { get; set; }
        public string experienciaEnCP { get; set; }
        public bool esContratable { get; set; }
        public string altaContratable { get; set; }
        public int strContratable { get; set; }
        public int prioridad { get; set; }
        public string strPrioridad { get; set; }
        public int idUsuarioCreacion { get; set; }
        public string usuarioCreacion_Nombre { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public string nombreCompleto { get; set; }
        public DateTime fechaIngreso { get; set; }
        public string numCC { get; set; }
        public string descripcionCC { get; set; }
        public string curp { get; set; }
        public string rfc { get; set; }
        public string nss { get; set; }
        public string dni { get; set; }
        public string cedula_ciudadania { get; set; }
        public string ciudad { get; set; }
        public string comentarios { get; set; }
        public string comentariosAutorizacion { get; set; }
        public string comentariosRecontratacion { get; set; }
        public string comentariosCancelacion { get; set; }
        public int? clave_autoriza { get; set; }
        public string nombre_autoriza { get; set; }
        public bool esAnticipada { get; set; }
        #endregion

        #region ENTREVISTA
        public int entrevista_id { get; set; }
        public int gerente_clave { get; set; }
        public string nombreGerente { get; set; }
        public DateTime? fecha_nacimiento { get; set; }
        public int estado_civil_clave { get; set; }
        public int escolaridad_clave { get; set; }
        public int p1_clave { get; set; }
        public string p1_concepto { get; set; }
        public int p2_clave { get; set; }
        public string p2_concepto { get; set; }
        public int p3_1_clave { get; set; }
        public string p3_1_concepto { get; set; }
        public int p3_2_clave { get; set; }
        public string p3_2_concepto { get; set; }
        public int p3_3_clave { get; set; }
        public string p3_3_concepto { get; set; }
        public int p3_4_clave { get; set; }
        public string p3_4_concepto { get; set; }
        public int p3_5_clave { get; set; }
        public string p3_5_concepto { get; set; }
        public int p3_6_clave { get; set; }
        public string p3_6_concepto { get; set; }
        public int p3_7_clave { get; set; }
        public string p3_7_concepto { get; set; }
        public int p3_8_clave { get; set; }
        public string p3_8_concepto { get; set; }
        public int p3_9_clave { get; set; }
        public string p3_9_concepto { get; set; }
        public int p3_10_clave { get; set; }
        public string p3_10_concepto { get; set; }
        public int p4_clave { get; set; }
        public string p4_concepto { get; set; }
        public int p5_clave { get; set; }
        public string p5_concepto { get; set; }
        public string p6_concepto { get; set; }
        public string p7_concepto { get; set; }
        public int p8_clave { get; set; }
        public string p8_concepto { get; set; }
        public string p8_porque { get; set; }
        public int p9_clave { get; set; }
        public string p9_concepto { get; set; }
        public string p9_porque { get; set; }
        public int p10_clave { get; set; }
        public string p10_concepto { get; set; }
        public string p10_porque { get; set; }
        public int p11_1_clave { get; set; }
        public string p11_1_concepto { get; set; }
        public int p11_2_clave { get; set; }
        public string p11_2_concepto { get; set; }
        public int p12_clave { get; set; }
        public string p12_concepto { get; set; }
        public string p12_porque { get; set; }
        public int p13_clave { get; set; }
        public string p13_concepto { get; set; }
        public int p14_clave { get; set; }
        public DateTime? p14_fecha { get; set; }
        public string p14_concepto { get; set; }
        public string p14_porque { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public string estado_civil_nombre { get; set; }
        public string escolaridad_nombre { get; set; }
        #endregion

        #region CAPACITACIONES Y ACTOS
        public int cantidadActos { get; set; }
        public int cantidadCursos { get; set; }
        public int cantidadHistorico { get; set; }
        #endregion

        #region autorizaciones

        public string est_baja { get; set; }
        public int? est_baja_usuario { get; set; }
        public DateTime est_baja_fecha { get; set; }
        public string est_baja_comentario { get; set; }
        public string est_baja_nombre { get; set; }
        public string est_baja_firma { get; set; }

        public string est_inventario { get; set; }
        public int? est_inventario_usuario { get; set; }
        public DateTime est_inventario_fecha { get; set; }
        public string est_inventario_comentario { get; set; }
        public string est_inventario_nombre { get; set; }
        public string est_inventario_firma { get; set; }

        public string est_contabilidad { get; set; }
        public int? est_contabilidad_usuario { get; set; }
        public DateTime? est_contabilidad_fecha { get; set; }
        public string est_contabilidad_comentario { get; set; }
        public string est_contabilidad_nombre { get; set; }
        public string est_contabilidad_firma { get; set; }

        public string est_compras { get; set; }
        public int? est_compras_usuario { get; set; }
        public DateTime est_compras_fecha { get; set; }
        public string est_compras_comentario { get; set; }
        public string est_compras_nombre { get; set; }
        public string est_compras_firma { get; set; }

        public string est_nominas { get; set; }
        public int? est_nominas_usuario { get; set; }
        public DateTime? est_nominas_fecha { get; set; }
        public string est_nominas_comentario { get; set; }
        public string est_nominas_firma { get; set; }

        public AutorizacionEnum autorizada { get; set; }
        #endregion

        #region FINIQUITOS
        public string rutaFiniquito { get; set; }  //RUTA INICIAL
        public decimal montoInicial { get; set; }
        public decimal montoRecalc { get; set; }
        public decimal montoCierre { get; set; }
        public string rutaRecalc { get; set; }
        public string rutaCierre { get; set; }
        public decimal montoIMS { get; set; }
        public string rutaIMS { get; set; }
        public string archivoFiniquito { get; set; }
        public string archivoRecalc { get; set; }
        public string archivoCierre { get; set; }
        public string archivoIMS { get; set; }
        public int numDiasFiniquito { get; set; }
        public bool esVencidoIMS { get; set; }
        public DateTime? fechaCapturaIMS { get; set; }
        #endregion

        #region CANCELAR
        public bool esCancelar { get; set; }
        #endregion
        public DateTime? fechaAlta { get; set; }
        public DateTime? fechaAntiguedad { get; set; }

        #region VAR REPORTE
        public string strMotivoBaja { get; set; }
        #endregion

        public string nombre_corto { get; set; }

        #region ESDIANA
        public bool esDiana { get; set; }
        #endregion

        public int empresa { get; set; }
        public bool? esPendienteNoti { get; set; }
    }
}
