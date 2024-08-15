using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.Reclutamientos;
using Core.Entity.RecursosHumanos.Bajas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Infrastructure.DTO;
using Core.DTO.Principal.Generales;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;

namespace Core.DAO.RecursosHumanos.Bajas
{
    public interface IBajasPersonalDAO
    {
        #region CRUD BAJAS
        Dictionary<string, object> GetBajasPersonal(List<string> listaCC, int contratable, int prioridad, DateTime? fechaInicio, DateTime? fechaFin,
            int? clave_empleado, string nombre_empleado, DateTime? fechaContaInicio, DateTime? fechaContaFin, string anticipada);

        Dictionary<string, object> CrearEditarBajaPersonal(BajaPersonalDTO objDTO);

        Dictionary<string, object> EliminarBajaPersonal(int idBaja);

        Dictionary<string, object> GetDatosActualizarBajaPersonal(int idBaja);

        Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre);

        Dictionary<string, object> GetDatosPersonaReporte(int claveEmpleado, string nombre);

        Dictionary<string, object> FillCboPreguntas(int idPregunta);

        Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado);

        Dictionary<string, object> GetHistorialCC(int cvEmpleado);

        Dictionary<string, object> GetDetalleAut(int id, int tipo);

        Dictionary<string, object> GetFacultamientosAutorizante(string cc);

        tblRH_EK_Empleados GetFacultamientosResponsableCC(string cc);

        #endregion

        #region FILL COMBOS
        Dictionary<string, object> GetCCs();

        Dictionary<string, object> FillCboEstados();

        Dictionary<string, object> FillCboMunicipios(int idEstado);

        Dictionary<string, object> FillCboEstadosEK();

        Dictionary<string, object> FillCboMunicipiosEK(int idEstado);

        Dictionary<string, object> FillCboEstadosCiviles();

        Dictionary<string, object> FillCboEscolaridades();

        Dictionary<string, object> FillCboCCByBajas();

        #endregion

        #region FUNCIONES GENERALES
        Dictionary<string, object> EnviarCorreo(string email, string link, int id);

        Dictionary<string, object> GuardarArchivoFiniquito(int idBaja, HttpPostedFileBase archivo, int tipoFiniquito, decimal monto);

        Dictionary<string, object> GetAutorizantes(string cc, int? clave_empleado, string nombre_empleado);

        Dictionary<string, object> VisualizarDocumento(int idBaja, int tipoFiniquito);

        Dictionary<string, object> NotificarBajas(List<int> lstClavesEmps);

        #endregion

        #region DASHBOARD

        Dictionary<string, object> getMesdeBaja(List<int> año, string idCC);
        Dictionary<string, object> getMotivoSeparacion(List<int> año, bool filterData, string idCC);

        #endregion

        #region AUTORIZACIÓN
        Dictionary<string, object> FillCboCCByBajasPermiso();
        Dictionary<string, object> GetBajasPersonalAutorizacion(List<string> listaCC);
        Dictionary<string, object> GuardarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion);
        #endregion

        #region CRUD BAJAS (VER BAJA DE RECLUTAMIENTOS EN EL MODULOD DE BAJAS)
        Dictionary<string, object> GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado);
        List<FamiliaresDTO> GetFamiliares(int clave_empleado);
        UniformesDTO GetUniformes(int claveEmpleado);
        List<ContratoDTO> GetContratos(int clave_empleado);
        List<ArchivosDTO> GetArchivoExamenMedico(int claveEmpleado);
        List<TabuladoresDTO> GetTabuladores(TabuladoresDTO objTabDTO);
        List<ComboDTO> FillComboRegistroPatronal(string cc);
        List<ComboDTO> FillComboDuracionContrato();
        Dictionary<string, object> FillDepartamentos(string cc);
        #endregion
        Dictionary<string, object> GetDocs(int? clave_empleado, int? id_candidato);

        #region COMENTARIOS/CANCELACION DE BAJA
        Dictionary<string, object> CancelarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion);

        #endregion

        #region PERMISOS
        bool UsuarioDeConsulta();
        bool UsuarioNotificar();
        bool UsuarioLiberarNominas();

        #endregion

        #region DIAS PARA BAJAS
        bool GetPuedeEditarDiasDisponibles();
        Dictionary<string, object> GetDiasDisponibles();
        Dictionary<string, object> GetDiasDisponiblesParaLimitarFecha();
        Dictionary<string, object> SetDiasDisponibles(int anteriores, int posteriores);
        #endregion

        #region LIBERAR NOMINA

        Dictionary<string, object> SetNominaLiberada(int idBaja, string comentario);

        #endregion
    }
}
