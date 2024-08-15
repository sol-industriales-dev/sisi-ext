using Core.DAO.RecursosHumanos.Bajas;
using Core.DTO.RecursosHumanos.BajasPersonal;
using Core.DTO.RecursosHumanos.Reclutamientos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Infrastructure.DTO;
using Core.DTO.Principal.Generales;
using Core.DAO.RecursosHumanos.Reclutamientos;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;

namespace Core.Service.RecursosHumanos.Reclutamientos
{
    public class BajasPersonalService : IBajasPersonalDAO
    {
        #region INIT
        public IBajasPersonalDAO r_bajasPersonal { get; set; }

        public BajasPersonalService(IBajasPersonalDAO BajasPersonal)
        {
            this.r_bajasPersonal = BajasPersonal;
        }
        #endregion

        #region CRUD BAJAS
        public Dictionary<string, object> GetBajasPersonal(List<string> listaCC, int contratable, int prioridad, DateTime? fechaInicio, DateTime? fechaFin, int? clave_empleado, string nombre_empleado
                                                            , DateTime? fechaContaInicio, DateTime? fechaContaFin, string anticipada)
        {
            return r_bajasPersonal.GetBajasPersonal(listaCC, contratable, prioridad, fechaInicio, fechaFin, clave_empleado, nombre_empleado, fechaContaInicio, fechaContaFin, anticipada);
        }

        public Dictionary<string, object> CrearEditarBajaPersonal(BajaPersonalDTO objDTO)
        {
            return r_bajasPersonal.CrearEditarBajaPersonal(objDTO);
        }

        public Dictionary<string, object> EliminarBajaPersonal(int idBaja)
        {
            return r_bajasPersonal.EliminarBajaPersonal(idBaja);
        }

        public Dictionary<string, object> GetDatosActualizarBajaPersonal(int idBaja)
        {
            return r_bajasPersonal.GetDatosActualizarBajaPersonal(idBaja);
        }

        public Dictionary<string, object> GetDatosPersona(int claveEmpleado, string nombre)
        {
            return r_bajasPersonal.GetDatosPersona(claveEmpleado, nombre);
        }

        public Dictionary<string, object> GetDatosPersonaReporte(int claveEmpleado, string nombre)
        {
            return r_bajasPersonal.GetDatosPersonaReporte(claveEmpleado, nombre);
        }

        public Dictionary<string, object> FillCboPreguntas(int idPregunta)
        {
            return r_bajasPersonal.FillCboPreguntas(idPregunta);
        }

        public Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado)
        {
            return r_bajasPersonal.GetEmpleadoCursosActos(claveEmpleado);
        }

        public Dictionary<string, object> GetHistorialCC(int cvEmpleado)
        {
            return r_bajasPersonal.GetHistorialCC(cvEmpleado);
        }

        public Dictionary<string, object> GetDetalleAut(int id, int tipo)
        {
            return r_bajasPersonal.GetDetalleAut(id,tipo);
        }

        public Dictionary<string, object> GetFacultamientosAutorizante(string cc)
        {
            return r_bajasPersonal.GetFacultamientosAutorizante(cc);
        }

        public tblRH_EK_Empleados GetFacultamientosResponsableCC(string cc)
        {
            return r_bajasPersonal.GetFacultamientosResponsableCC(cc);
        }

        #endregion

        #region FILL COMBOS
        public Dictionary<string, object> GetCCs()
        {
            return r_bajasPersonal.GetCCs();
        }

        public Dictionary<string, object> FillCboEstados()
        {
            return r_bajasPersonal.FillCboEstados();
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {
            return r_bajasPersonal.FillCboMunicipios(idEstado);
        }

        public Dictionary<string, object> FillCboEstadosEK()
        {
            return r_bajasPersonal.FillCboEstadosEK();
        }

        public Dictionary<string, object> FillCboMunicipiosEK(int idEstado)
        {
            return r_bajasPersonal.FillCboMunicipiosEK(idEstado);
        }

        public Dictionary<string, object> FillCboEstadosCiviles()
        {
            return r_bajasPersonal.FillCboEstadosCiviles();
        }

        public Dictionary<string, object> FillCboEscolaridades()
        {
            return r_bajasPersonal.FillCboEscolaridades();
        }

        public Dictionary<string, object> FillCboCCByBajas()
        {
            return r_bajasPersonal.FillCboCCByBajas();
        }

        #endregion

        #region FUNCIONES GENERALES

        public Dictionary<string, object> EnviarCorreo(string email, string link, int id)
        {
            return r_bajasPersonal.EnviarCorreo(email, link, id);
        }

        public Dictionary<string, object> GuardarArchivoFiniquito(int idBaja, HttpPostedFileBase archivo, int tipoFiniquito, decimal monto)
        {
            return r_bajasPersonal.GuardarArchivoFiniquito(idBaja, archivo, tipoFiniquito, monto);
        }

        public Dictionary<string, object> GetAutorizantes(string cc, int? clave_empleado, string nombre_empleado)
        {
            return r_bajasPersonal.GetAutorizantes(cc, clave_empleado, nombre_empleado);
        }

        public Dictionary<string, object> VisualizarDocumento(int idBaja, int tipoFiniquito)
        {
            return r_bajasPersonal.VisualizarDocumento(idBaja, tipoFiniquito);
        }

        public Dictionary<string , object> NotificarBajas(List<int> lstClavesEmps)
        {
            return r_bajasPersonal.NotificarBajas(lstClavesEmps);
        }
        #endregion

        #region DASHBOARD

        public Dictionary<string, object> getMesdeBaja(List<int> año, string idCC)
        {
            return r_bajasPersonal.getMesdeBaja(año, idCC);
        }

        public Dictionary<string, object> getMotivoSeparacion(List<int> año, bool filterData, string idCC)
        {
            return r_bajasPersonal.getMotivoSeparacion(año, filterData, idCC);
        }

        #endregion

        #region AUTORIZACIÓN
        public Dictionary<string, object> FillCboCCByBajasPermiso()
        {
            return r_bajasPersonal.FillCboCCByBajasPermiso();
        }
        public Dictionary<string, object> GetBajasPersonalAutorizacion(List<string> listaCC)
        {
            return r_bajasPersonal.GetBajasPersonalAutorizacion(listaCC);
        }

        public Dictionary<string, object> GuardarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            return r_bajasPersonal.GuardarAutorizacionBajas(objAutorizacion);
        }
        #endregion

        #region CRUD BAJAS (VER BAJA DE RECLUTAMIENTOS EN EL MODULOD DE BAJAS)

        public Dictionary<string, object> GetDatosActualizarEmpleado(int claveEmpleado, bool esReingresoEmpleado)
        {
            return r_bajasPersonal.GetDatosActualizarEmpleado(claveEmpleado, esReingresoEmpleado);
        }

        public List<FamiliaresDTO> GetFamiliares(int clave_empleado)
        {
            return r_bajasPersonal.GetFamiliares(clave_empleado);
        }

        public UniformesDTO GetUniformes(int claveEmpleado)
        {
            return r_bajasPersonal.GetUniformes(claveEmpleado);
        }

        public List<ContratoDTO> GetContratos(int clave_empleado)
        {
            return r_bajasPersonal.GetContratos(clave_empleado);
        }

        public List<ArchivosDTO> GetArchivoExamenMedico(int claveEmpleado)
        {
            return r_bajasPersonal.GetArchivoExamenMedico(claveEmpleado);
        }

        public List<TabuladoresDTO> GetTabuladores(TabuladoresDTO objTabDTO)
        {
            return r_bajasPersonal.GetTabuladores(objTabDTO);
        }

        public List<ComboDTO> FillComboRegistroPatronal(string cc)
        {
            return r_bajasPersonal.FillComboRegistroPatronal(cc);
        }

        public List<ComboDTO> FillComboDuracionContrato()
        {
            return r_bajasPersonal.FillComboDuracionContrato();
        }

        public Dictionary<string, object> FillDepartamentos(string cc)
        {
            return r_bajasPersonal.FillDepartamentos(cc);
        }

        public Dictionary<string, object> GetDocs(int? clave_empleado, int? id_candidato)
        {
            return r_bajasPersonal.GetDocs(clave_empleado, id_candidato);
        }

        #endregion

        #region COMENTARIOS/CANCELACION DE BAJA
        public Dictionary<string, object> CancelarAutorizacionBajas(AutorizacionBajaDTO objAutorizacion)
        {
            return r_bajasPersonal.CancelarAutorizacionBajas(objAutorizacion);
        }

        #endregion

        #region PERMISOS
        public bool UsuarioDeConsulta()
        {
            return r_bajasPersonal.UsuarioDeConsulta();
        }

        public bool UsuarioNotificar()
        {
            return r_bajasPersonal.UsuarioNotificar();
        }

        public bool UsuarioLiberarNominas()
        {
            return r_bajasPersonal.UsuarioLiberarNominas();

        }
        #endregion

        #region DIAS PARA BAJAS
        public bool GetPuedeEditarDiasDisponibles()
        {
            return r_bajasPersonal.GetPuedeEditarDiasDisponibles();
        }
        public Dictionary<string, object> GetDiasDisponibles()
        {
            return r_bajasPersonal.GetDiasDisponibles();
        }
        public Dictionary<string, object> GetDiasDisponiblesParaLimitarFecha()
        {
            return r_bajasPersonal.GetDiasDisponiblesParaLimitarFecha();
        }
        public Dictionary<string, object> SetDiasDisponibles(int anteriores, int posteriores)
        {
            return r_bajasPersonal.SetDiasDisponibles(anteriores, posteriores);
        }
        #endregion

        #region LIBERAR NOMINA
        public Dictionary<string,object> SetNominaLiberada(int idBaja, string comentario)
        {
            return r_bajasPersonal.SetNominaLiberada(idBaja, comentario);
        }

        #endregion
    }
}
