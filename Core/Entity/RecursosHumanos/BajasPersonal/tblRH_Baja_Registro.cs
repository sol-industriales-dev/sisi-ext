using Core.Enum.RecursosHumanos.BajasPersonal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.RecursosHumanos.Bajas
{
    public class tblRH_Baja_Registro
    {
        public int id { get; set; }
        public int numeroEmpleado { get; set; }
        public string nombre { get; set; }
        public string cc { get; set; }
        public string descripcionCC { get; set; }
        public int? idPuesto { get; set; }
        public string nombrePuesto { get; set; }
        public DateTime? fechaIngreso { get; set; }
        public string habilidadesConEquipo { get; set; }
        public string telPersonal { get; set; }
        public bool? tieneWha { get; set; }
        public string telCasa { get; set; }
        public string contactoFamilia { get; set; }
        public int? idDepartamento { get; set; } // PERU
        public int? idEstado { get; set; }
        public int? idCiudad { get; set; }
        public int? idMunicipio { get; set; }
        public string direccion { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string correo { get; set; }
        public DateTime? fechaBaja { get; set; }
        public string motivoBajaDeSistema { get; set; }
        public string motivoSeparacionDeEmpresa { get; set; }
        public bool regresariaALaEmpresa { get; set; }
        public string porqueRegresariaALaEmpresa { get; set; }
        public string dispuestoCambioDeProyecto { get; set; }
        public string experienciaEnCP { get; set; }
        public bool esContratable { get; set; }
        public int prioridad { get; set; }
        public int? clave_autoriza { get; set; }
        public string nombre_autoriza { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public string curp { get; set; }
        public string rfc { get; set; }
        public string nss { get; set; }
        public string dni { get; set; } //PERU
        public string cedula_ciudadania { get; set; } // COLOMBIA
        public string est_baja { get; set; }
        public int? est_baja_usuario { get; set; }
        public DateTime? est_baja_fecha { get; set; }
        public string est_baja_comentario { get; set; }
        public string est_baja_firma { get; set; }

        public string est_inventario { get; set; }
        public int? est_inventario_usuario { get; set; }
        public DateTime? est_inventario_fecha { get; set; }
        public string est_inventario_comentario { get; set; }
        public string est_inventario_firma { get; set; }

        public string est_contabilidad { get; set; }
        public int? est_contabilidad_usuario { get; set; }
        public DateTime? est_contabilidad_fecha { get; set; }
        public string est_contabilidad_comentario { get; set; }
        public string est_contabilidad_firma { get; set; }

        public string est_compras { get; set; }
        public int? est_compras_usuario { get; set; }
        public DateTime? est_compras_fecha { get; set; }
        public string est_compras_comentario { get; set; }
        public string est_compras_firma { get; set; }

        //NOMINAS
        public string est_nominas { get; set; }
        public int? est_nominas_usuario { get; set; }
        public DateTime? est_nominas_fecha { get; set; }
        public string est_nominas_comentario { get; set; }
        public string est_nominas_firma { get; set; }

        public string rutaFiniquito { get; set; }
        public AutorizacionEnum autorizada { get; set; }
        public string comentarios { get; set; }
        public string comentariosAutorizacion { get; set; }
        public string comentariosRecontratacion { get; set; }
        public string comentariosCancelacion { get; set; }
        public bool esAnticipada { get; set; }
        public bool? esPendienteDarBaja { get; set; } // ESTATUS PARA LAS BAJAS ANTICIPADAS PARA DAR DE BAJA MEDIENTE SERVICIO
        public DateTime? fechaBajaServicio { get; set; }
        public bool? esPendienteNoti { get; set; }
        public int? usuarioNoti { get; set; }
        public DateTime? fechaNoti { get; set; }
    }
}
