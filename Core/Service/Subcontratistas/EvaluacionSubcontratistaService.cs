using Core.DAO.Subcontratistas;
using Core.DTO;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Entity.Encuestas;
using Core.Entity.SubContratistas;
using Core.Enum.ControlObra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Subcontratistas
{
    public class EvaluacionSubcontratistaService : IEvaluacionSubcontratistaDAO
    {
        #region SUB
        private IEvaluacionSubcontratistaDAO _evaluacionSubcontratistaDAO;

        public IEvaluacionSubcontratistaDAO evaluacionSubcontratista
        {
            get
            {
                return _evaluacionSubcontratistaDAO;
            }

            set
            {
                _evaluacionSubcontratistaDAO = value;
            }
        }

        public EvaluacionSubcontratistaService(IEvaluacionSubcontratistaDAO evaluacionSubcontratista)
        {
            this.evaluacionSubcontratista = evaluacionSubcontratista;
        }

        #region Captura
        #endregion

        #region Gestión
        public Dictionary<string, object> ObtenerProyectosParaFiltro()
        {
            return this.evaluacionSubcontratista.ObtenerProyectosParaFiltro();
        }

        public Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto)
        {
            return this.evaluacionSubcontratista.ObtenerSubcontratistasParaFiltro(proyecto);
        }

        public Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
        {
            return this.evaluacionSubcontratista.ObtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId);
        }

        public Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId)
        {
            return this.evaluacionSubcontratista.ObtenerEstatusFirmantes(evaluacionId);
        }

        public Dictionary<string, object> ObtenerFirmante(int evaluacionId)
        {
            return this.evaluacionSubcontratista.ObtenerFirmante(evaluacionId);
        }

        public Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma)
        {
            return this.evaluacionSubcontratista.GuardarFirma(firma);
        }
        #endregion

        #region Dashboard
        #endregion

        public List<excelDTO> obtenerListado(int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerListado(idAsignacion);
        }
        public tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.traermeDatosPrincipales(idAsignacion);
        }
        public List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerTodasLasASinaciones(idAsignacion);
        }
        public Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.cambiarDeColor(idPlantilla, idAsignacion);
        }
        #region SUB CONTRATISTA

        public List<ComboDTO> getProyecto(int idUsuario)
        {
            return _evaluacionSubcontratistaDAO.getProyecto(idUsuario);
        }
        public List<ComboDTO> getSubContratistas(string AreaCuenta)
        {
            return _evaluacionSubcontratistaDAO.getSubContratistas(AreaCuenta);
        }
        public Dictionary<string, object> FillCboProyectosFacultamientos()
        {
            return _evaluacionSubcontratistaDAO.FillCboProyectosFacultamientos();
        }
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.getTblSubContratista(parametros);
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros)
        {
            return _evaluacionSubcontratistaDAO.addEditSubContratista(Archivo, parametros);
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosXSubcontratista(parametros);
        }
        public List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosSubcontratista(parametros);
        }

        public List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosXSubcontratistaEvaluacion(parametros);
        }

        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerTblAutorizacion(parametros);
        }
        public Dictionary<string, object> GetPlantillasCreadas(int plantilla_id, int contrato_id)
        {
            return _evaluacionSubcontratistaDAO.GetPlantillasCreadas(plantilla_id, contrato_id);
        }
        public Dictionary<string, object> addEditPlantilla(DivicionesMenuDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.addEditPlantilla(parametros);
        }
        public List<ComboDTO> cboObtenerContratos()
        {
            return _evaluacionSubcontratistaDAO.cboObtenerContratos();
        }
        public List<ComboDTO> cboObtenerContratosInclu(int idPlantilla)
        {
            return _evaluacionSubcontratistaDAO.cboObtenerContratosInclu(idPlantilla);
        }
        public Dictionary<string, object> eliminarPlantilla(int id)
        {
            return _evaluacionSubcontratistaDAO.eliminarPlantilla(id);
        }
        public List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla)
        {
            return _evaluacionSubcontratistaDAO.obtenerDiviciones(idPlantilla);
        }
        public Dictionary<string, object> obtenerDivicionesEvaluador()
        {
            return _evaluacionSubcontratistaDAO.obtenerDivicionesEvaluador();
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerDivicionesEvaluadorArchivos(idPlantilla, idAsignacion);
        }
        public List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerLst(idPlantilla, idAsignacion);
        }
        public List<tblCOES_Firma> obtenerFirmas(int evaluacion_id)
        {
            return _evaluacionSubcontratistaDAO.obtenerFirmas(evaluacion_id);
        }
        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.addEditDiviciones(parametros);
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            return _evaluacionSubcontratistaDAO.eliminarDiviciones(id);
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv)
        {
            return _evaluacionSubcontratistaDAO.obtenerRequerimientos(idDiv);
        }

        public byte[] DescargarArchivos(long idDet, int idEvaluacion)
        {
            return _evaluacionSubcontratistaDAO.DescargarArchivos(idDet, idEvaluacion);
        }
        public string getFileName(long idDet, int idEvaluacion)
        {
            return _evaluacionSubcontratistaDAO.getFileName(idDet, idEvaluacion);
        }
        public SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerEvaluacionxReq(parametros);
        }
        public SubContratistasDTO GuardarEvaluacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.GuardarEvaluacion(parametros);
        }
        public SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO objDTO)
        {
            return _evaluacionSubcontratistaDAO.obtenerPromegioEvaluacion(objDTO);
        }

        public Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerGraficaDeBarras(parametros);
        }

        public List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerPromediosxSubcontratista(parametros);
        }
        public bool ObtenerEvaluacionPendiente(string idUsuario)
        {
            return _evaluacionSubcontratistaDAO.ObtenerEvaluacionPendiente(idUsuario);
        }
        public List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, int tipoUsuario, string cc)
        {
            return _evaluacionSubcontratistaDAO.obtenerContratistasConContrato(AreaCuenta, subcontratista, Estatus, tipoUsuario, cc);
        }
        public SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.GuardarAsignacion(parametros);
        }
        public List<SubContratistasDTO> getUsuariosAutorizantes(string term)
        {
            return _evaluacionSubcontratistaDAO.getUsuariosAutorizantes(term);
        }
        //public SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario)
        //{
        //    return _evaluacionSubcontratistaDAO.AutorizarEvaluacion(parametros, idUsuario);
        //}
        //public SubContratistasDTO AutorizarAsignacion(SubContratistasDTO parametros, int idUsuario)
        public Dictionary<string, object> AutorizarAsignacion(SubContratistasDTO objDTO, int idUsuario)
        {
            return _evaluacionSubcontratistaDAO.AutorizarAsignacion(objDTO, idUsuario);
        }

        public List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento()
        {
            return _evaluacionSubcontratistaDAO.obtenerTodosLosElementosConSuRequerimiento();
        }
        public ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion)
        {
            return _evaluacionSubcontratistaDAO.guardarRelacion(lstRelacion);
        }
        public List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(string RFC)
        {
            return _evaluacionSubcontratistaDAO.tblObtenerDashBoardSubContratista(RFC);
        }
        public MemoryStream realizarExcel(int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.realizarExcel(idAsignacion);

        }
        public ContratoSubContratistaDTO EvaluarDetalle(int id, List<SubContratistasDTO> parametros, int userEnvia)
        {
            return _evaluacionSubcontratistaDAO.EvaluarDetalle(id, parametros, userEnvia);
        }
        public List<SubContratistasDTO> obtenerPromedioxElemento(List<SubContratistasDTO> parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerPromedioxElemento(parametros);
        }
        #endregion
        #region
        public List<evaluadorXccDTO> getEvaluadoresxCC(string cc, string elemento, int evaluadores)
        {
            return _evaluacionSubcontratistaDAO.getEvaluadoresxCC(cc, elemento, evaluadores);
        }
        public evaluadorXccDTO AgregarEditarEvaluadores(evaluadorXccDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.AgregarEditarEvaluadores(parametros);
        }
        public evaluadorXccDTO ActivarDesactivarEvaluadores(evaluadorXccDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ActivarDesactivarEvaluadores(parametros);
        }
        public List<ComboDTO> getSubContratistasRestantes()
        {
            return _evaluacionSubcontratistaDAO.getSubContratistasRestantes();
        }
        public List<ComboDTO> getProyectoRestantes(bool Agregar)
        {
            return _evaluacionSubcontratistaDAO.getProyectoRestantes(Agregar);
        }
        public List<tblEN_Estrellas> getEstrellas()
        {
            return _evaluacionSubcontratistaDAO.getEstrellas();
        }
        public List<ComboDTO> obtenerTodolosElementos()
        {
            return _evaluacionSubcontratistaDAO.obtenerTodolosElementos();
        }
        public Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerElementosEvaluar(idUsuario, idPlantilla, idAsignacion);
        }
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorCentroDeCosto(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerGraficaDeEvaluacionPorCentroDeCosto(parametros);
        }
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorDivisionElemento(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerGraficaDeEvaluacionPorDivisionElemento(parametros);
        }
        public Dictionary<string, object> ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerGraficaDeEvaluacionPorTendenciaMedianteEvaluaciones(parametros);
        }
        #endregion
        #region COMBOS

        public Dictionary<string, object> fillComboPrestadoresDeServicio()
        {
            return _evaluacionSubcontratistaDAO.fillComboPrestadoresDeServicio();
        }
        public Dictionary<string, object> AccionesUsuariosExpediente(tblPUsuarioDTO objUsuario, int Accion)
        {
            return _evaluacionSubcontratistaDAO.AccionesUsuariosExpediente(objUsuario, Accion);
        }

        public Dictionary<string, object> cboProyecto()
        {
            return _evaluacionSubcontratistaDAO.cboProyecto();
        }
        public Dictionary<string, object> cboProyecto3(int idSubcontratista)
        {
            return _evaluacionSubcontratistaDAO.cboProyecto3(idSubcontratista);
        }
        public Dictionary<string, object> cboContratosBuscar3(int idSubcontratista)
        {
            return _evaluacionSubcontratistaDAO.cboContratosBuscar3(idSubcontratista);
        }
        public Dictionary<string, object> cboEvaluador()
        {
            return _evaluacionSubcontratistaDAO.cboEvaluador();
        }
        public Dictionary<string, object> cboSubcontratistas()
        {
            return _evaluacionSubcontratistaDAO.cboSubcontratistas();
        }
        public Dictionary<string, object> cboElementos()
        {
            return _evaluacionSubcontratistaDAO.cboElementos();
        }
        public Dictionary<string, object> FillComboPlantillas()
        {
            return _evaluacionSubcontratistaDAO.FillComboPlantillas();
        }
        public Dictionary<string, object> cboContratosBuscar()
        {
            return _evaluacionSubcontratistaDAO.cboContratosBuscar();
        }
        #endregion
        #region FACULTAMIENTOS

        public Dictionary<string, object> AccionesFacultamientos(facultamientosCODTO objUsuario, int Accion)
        {
            return _evaluacionSubcontratistaDAO.AccionesFacultamientos(objUsuario, Accion);
        }

        #endregion
        public Dictionary<string, object> cboUsuarios()
        {
            return _evaluacionSubcontratistaDAO.cboUsuarios();
        }
        public Dictionary<string, object> obtenerPromedioGeneral(int id)
        {
            return this._evaluacionSubcontratistaDAO.obtenerPromedioGeneral(id);
        }

        public Respuesta PermisoVista(int vistaID)
        {
            return this._evaluacionSubcontratistaDAO.PermisoVista(vistaID);
        }

        public Dictionary<string, object> VerificarTipoUsuario()
        {
            return this._evaluacionSubcontratistaDAO.VerificarTipoUsuario();
        }

        public Dictionary<string, object> FillCboEstados()
        {
            return this._evaluacionSubcontratistaDAO.FillCboEstados();
        }

        public Dictionary<string, object> FillCboMunicipios(int idEstado)
        {
            return this._evaluacionSubcontratistaDAO.FillCboMunicipios(idEstado);
        }
        #endregion

        #region ADMINISTRACIÓN DE EVALUACIONES
        public Dictionary<string, object> VerificarElementoTerminado(EvaluacionDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.VerificarElementoTerminado(objDTO);
        }
        #endregion

        #region CATÁLOGO DE EVALUADORES
        public Dictionary<string, object> FillCboFiltroEvaluadores()
        {
            return this._evaluacionSubcontratistaDAO.FillCboFiltroEvaluadores();
        }

        public Dictionary<string, object> FillCboCEEvaluadores()
        {
            return this._evaluacionSubcontratistaDAO.FillCboCEEvaluadores();
        }
        #endregion

        #region CATALOGOS DE FACULTAMIENTOS
        public Dictionary<string, object> GetListadoUsuarioRelCC(int id)
        {
            return this._evaluacionSubcontratistaDAO.GetListadoUsuarioRelCC(id);
        }

        public Dictionary<string, object> GetCCActualizarFacultamiento(int id)
        {
            return this._evaluacionSubcontratistaDAO.GetCCActualizarFacultamiento(id);
        }

        public Dictionary<string, object> FillTipoUsuarioFacultamientos()
        {
            return this._evaluacionSubcontratistaDAO.FillTipoUsuarioFacultamientos();
        }
        #endregion

        #region CATALOGO FIRMASTES ENCARGADOS DE CIERTOS CONTRATOS
        public Dictionary<string, object> GetUsuariosRelSubcontratistas(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.GetUsuariosRelSubcontratistas(objDTO);
        }

        public Dictionary<string, object> CEUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.CEUsuarioRelSubcontratista(objDTO);
        }

        public Dictionary<string, object> EliminarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.EliminarUsuarioRelSubcontratista(objDTO);
        }

        public Dictionary<string, object> MandarUsuarioComoHistorial(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.MandarUsuarioComoHistorial(objDTO);
        }

        public Dictionary<string, object> FillComboSubcontratistas()
        {
            return this._evaluacionSubcontratistaDAO.FillComboSubcontratistas();
        }

        public Dictionary<string, object> GetDatosActualizarUsuarioRelSubcontratista(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.GetDatosActualizarUsuarioRelSubcontratista(objDTO);
        }

        public Dictionary<string, object> FillCboContratosRelSubcontratistas(int idSubcontratista)
        {
            return this._evaluacionSubcontratistaDAO.FillCboContratosRelSubcontratistas(idSubcontratista);
        }
        #endregion

        #region CALENDARIO
        public Dictionary<string, object> FillCboEvaluacionesActivas(SubContratistasDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.FillCboEvaluacionesActivas(objDTO);
        }

        public Dictionary<string, object> GetFechasEvaluaciones(UsuarioRelSubcontratistaDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.GetFechasEvaluaciones(objDTO);
        }

        public Dictionary<string, object> ActualizarFechasActualizacion(CalendarioDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.ActualizarFechasActualizacion(objDTO);
        }

        public Dictionary<string, object> GetFechasActualizar(CalendarioDTO objDTO)
        {
            return this._evaluacionSubcontratistaDAO.GetFechasActualizar(objDTO);
        }

        public Dictionary<string, object> GetTipoUsuario()
        {
            return this._evaluacionSubcontratistaDAO.GetTipoUsuario();
        }
        #endregion

        #region CATÁLOGO DE PLANTILLAS
        public Dictionary<string, object> FillComboContratos()
        {
            return this._evaluacionSubcontratistaDAO.FillComboContratos();
        }
        #endregion

        #region REPORTE CRYSTAL REPORT
        public List<tblCO_ADP_EvalSubConAsignacion> GetEvaluaciones(int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.GetEvaluaciones(idAsignacion);
        }
        #endregion

        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            return this._evaluacionSubcontratistaDAO.GetUsuariosAutocomplete(term, porClave);
        }

        public Dictionary<string, object> FillComboElementos()
        {
            return this._evaluacionSubcontratistaDAO.FillComboElementos();
        }

        public Dictionary<string, object> FillComboRequerimientos(int elemento_id)
        {
            return this._evaluacionSubcontratistaDAO.FillComboRequerimientos(elemento_id);
        }

        public Dictionary<string, object> GuardarNuevoElemento(tblCOES_Elemento elemento)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevoElemento(elemento);
        }

        public Dictionary<string, object> GuardarNuevoRequerimiento(tblCOES_Requerimiento requerimiento)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevoRequerimiento(requerimiento);
        }

        public Dictionary<string, object> GetRequerimientosElemento(int elemento_id)
        {
            return this._evaluacionSubcontratistaDAO.GetRequerimientosElemento(elemento_id);
        }

        public Dictionary<string, object> GuardarNuevaPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevaPlantilla(plantilla, contratos, requerimientos);
        }

        public Dictionary<string, object> EditarPlantilla(tblCOES_Plantilla plantilla, List<int> contratos, List<RequerimientoDTO> requerimientos)
        {
            return this._evaluacionSubcontratistaDAO.EditarPlantilla(plantilla, contratos, requerimientos);
        }

        public Dictionary<string, object> EliminarRequerimientoElemento(int requerimiento_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarRequerimientoElemento(requerimiento_id);
        }

        public Dictionary<string, object> GetPlantilla(int plantilla_id)
        {
            return this._evaluacionSubcontratistaDAO.GetPlantilla(plantilla_id);
        }

        public Dictionary<string, object> CopiarPlantillaBase(int plantilla_id)
        {
            return this._evaluacionSubcontratistaDAO.CopiarPlantillaBase(plantilla_id);
        }

        public Dictionary<string, object> EliminarPlantilla(int plantilla_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarPlantilla(plantilla_id);
        }

        public Dictionary<string, object> FillComboProyectos()
        {
            return this._evaluacionSubcontratistaDAO.FillComboProyectos();
        }

        public Dictionary<string, object> GetFacultamientoUsuario()
        {
            return this._evaluacionSubcontratistaDAO.GetFacultamientoUsuario();
        }

        #region Evaluadores
        public Dictionary<string, object> GetEvaluadores(string cc, int elemento)
        {
            return this._evaluacionSubcontratistaDAO.GetEvaluadores(cc, elemento);
        }

        public Dictionary<string, object> GuardarNuevoEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevoEvaluador(evaluador, proyectos, elementos);
        }

        public Dictionary<string, object> EditarEvaluador(tblCOES_Evaluador evaluador, List<tblCOES_Evaluador_Proyecto> proyectos, List<tblCOES_Evaluador_Elemento> elementos)
        {
            return this._evaluacionSubcontratistaDAO.EditarEvaluador(evaluador, proyectos, elementos);
        }

        public Dictionary<string, object> EliminarEvaluador(int evaluador_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarEvaluador(evaluador_id);
        }

        public Dictionary<string, object> GetEvaluador(int evaluador_id)
        {
            return this._evaluacionSubcontratistaDAO.GetEvaluador(evaluador_id);
        }
        #endregion

        #region Facultamientos
        public Dictionary<string, object> GetFacultamientos(string cc, TipoFacultamientoEnum tipo)
        {
            return this._evaluacionSubcontratistaDAO.GetFacultamientos(cc, tipo);
        }

        public Dictionary<string, object> GuardarNuevoFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevoFacultamiento(facultamiento, proyectos);
        }

        public Dictionary<string, object> EditarFacultamiento(tblCOES_Facultamiento facultamiento, List<tblCOES_Facultamiento_CentroCosto> proyectos)
        {
            return this._evaluacionSubcontratistaDAO.EditarFacultamiento(facultamiento, proyectos);
        }

        public Dictionary<string, object> EliminarFacultamiento(int facultamiento_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarFacultamiento(facultamiento_id);
        }

        public Dictionary<string, object> GetFacultamiento(int facultamiento_id)
        {
            return this._evaluacionSubcontratistaDAO.GetFacultamiento(facultamiento_id);
        }

        public Dictionary<string, object> GetListadoCCRelUsuarioFacultamientos(int facultamiento_id)
        {
            return this._evaluacionSubcontratistaDAO.GetListadoCCRelUsuarioFacultamientos(facultamiento_id);
        }
        #endregion

        #region Firmas Subcontratistas
        public Dictionary<string, object> GetFirmaSubcontratistas(int subcontratista_id)
        {
            return this._evaluacionSubcontratistaDAO.GetFirmaSubcontratistas(subcontratista_id);
        }

        public Dictionary<string, object> GuardarNuevaFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevaFirmaSubcontratista(firma, contratos);
        }

        public Dictionary<string, object> EditarFirmaSubcontratista(tblCOES_FirmaSubcontratista firma, List<tblCOES_FirmaSubcontratistatblX_Contrato> contratos)
        {
            return this._evaluacionSubcontratistaDAO.EditarFirmaSubcontratista(firma, contratos);
        }

        public Dictionary<string, object> EliminarFirmaSubcontratista(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarFirmaSubcontratista(firma_id);
        }

        public Dictionary<string, object> GetFirmaSubcontratista(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.GetFirmaSubcontratista(firma_id);
        }

        public Dictionary<string, object> EnviarCorreoNotificacionFirma(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.EnviarCorreoNotificacionFirma(firma_id);
        }
        #endregion

        #region Firmas Gerentes
        public Dictionary<string, object> GetFirmaGerentes(string cc)
        {
            return this._evaluacionSubcontratistaDAO.GetFirmaGerentes(cc);
        }

        public Dictionary<string, object> GuardarNuevaFirmaGerente(tblCOES_FirmaGerente firma)
        {
            return this._evaluacionSubcontratistaDAO.GuardarNuevaFirmaGerente(firma);
        }

        public Dictionary<string, object> EditarFirmaGerente(tblCOES_FirmaGerente firma)
        {
            return this._evaluacionSubcontratistaDAO.EditarFirmaGerente(firma);
        }

        public Dictionary<string, object> EliminarFirmaGerente(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.EliminarFirmaGerente(firma_id);
        }

        public Dictionary<string, object> GetFirmaGerente(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.GetFirmaGerente(firma_id);
        }
        #endregion

        #region Especialidades
        public Dictionary<string, object> FillComboEspecialidades()
        {
            return this._evaluacionSubcontratistaDAO.FillComboEspecialidades();
        }

        public Dictionary<string, object> GetSubcontratistasEspecialidad(string cc)
        {
            return this._evaluacionSubcontratistaDAO.GetSubcontratistasEspecialidad(cc);
        }

        public Dictionary<string, object> GuardarEspecialidadesSubcontratista(int subcontratista_id, List<tblCOES_EspecialidadtblX_SubContratista> especialidades)
        {
            return this._evaluacionSubcontratistaDAO.GuardarEspecialidadesSubcontratista(subcontratista_id, especialidades);
        }
        #endregion

        #region Administración Evaluaciones
        public Dictionary<string, object> CargarEvaluacionesSubcontratistas(string cc, int subcontratista_id, EstatusEvaluacionEnum estatus)
        {
            return this._evaluacionSubcontratistaDAO.CargarEvaluacionesSubcontratistas(cc, subcontratista_id, estatus);
        }

        public Dictionary<string, object> GuardarAsignacionEvaluacion(tblCOES_Asignacion asignacion, List<tblCOES_Asignacion_Evaluacion> evaluaciones)
        {
            return this._evaluacionSubcontratistaDAO.GuardarAsignacionEvaluacion(asignacion, evaluaciones);
        }

        public Dictionary<string, object> GetContratoInformacion(int contrato_id)
        {
            return this._evaluacionSubcontratistaDAO.GetContratoInformacion(contrato_id);
        }

        public Dictionary<string, object> GetElementosEvaluacion(int contrato_id, int evaluacion_id)
        {
            return this._evaluacionSubcontratistaDAO.GetElementosEvaluacion(contrato_id, evaluacion_id);
        }

        public Dictionary<string, object> GuardarRetroalimentacionEvaluador(tblCOES_Evidencia evidencia)
        {
            return this._evaluacionSubcontratistaDAO.GuardarRetroalimentacionEvaluador(evidencia);
        }

        public Dictionary<string, object> GuardarEvaluacionSubcontratista()
        {
            return this._evaluacionSubcontratistaDAO.GuardarEvaluacionSubcontratista();
        }

        public tblCOES_Evidencia GetArchivoEvidencia(int evidencia_id)
        {
            return this._evaluacionSubcontratistaDAO.GetArchivoEvidencia(evidencia_id);
        }

        public Dictionary<string, object> EnviarGestionFirmas(int evaluacion_id, int contrato_id, int subcontratista_id)
        {
            return this._evaluacionSubcontratistaDAO.EnviarGestionFirmas(evaluacion_id, contrato_id, subcontratista_id);
        }

        public Dictionary<string, object> GetSeguimientoFirmas(int evaluacion_id, int contrato_id)
        {
            return this._evaluacionSubcontratistaDAO.GetSeguimientoFirmas(evaluacion_id, contrato_id);
        }

        public Dictionary<string, object> AutorizarEvaluacion(int firma_id, HttpPostedFileBase archivoFirma)
        {
            return this._evaluacionSubcontratistaDAO.AutorizarEvaluacion(firma_id, archivoFirma);
        }

        public Dictionary<string, object> RechazarEvaluacion(int firma_id)
        {
            return this._evaluacionSubcontratistaDAO.RechazarEvaluacion(firma_id);
        }

        public Dictionary<string, object> GetAsignacionContrato(int asignacion_id)
        {
            return this._evaluacionSubcontratistaDAO.GetAsignacionContrato(asignacion_id);
        }

        public Dictionary<string, object> GuardarCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            return this._evaluacionSubcontratistaDAO.GuardarCambioEvaluacion(cambio);
        }

        public Dictionary<string, object> GetCambioEvaluacion(int cambioEvaluacion_id)
        {
            return this._evaluacionSubcontratistaDAO.GetCambioEvaluacion(cambioEvaluacion_id);
        }

        public Dictionary<string, object> GuardarAutorizacionCambioEvaluacion(tblCOES_CambioEvaluacion cambio)
        {
            return this._evaluacionSubcontratistaDAO.GuardarAutorizacionCambioEvaluacion(cambio);
        }

        public Dictionary<string, object> CargarGraficasSubcontratista(string cc, int subcontratista_id, int contrato_id, EstatusEvaluacionEnum estatus)
        {
            return this._evaluacionSubcontratistaDAO.CargarGraficasSubcontratista(cc, subcontratista_id, contrato_id, estatus);
        }

        public ReporteEvaluacionSubcontratistaDTO GetReporteEvaluacionSubcontratista(int evaluacion_id)
        {
            return this._evaluacionSubcontratistaDAO.GetReporteEvaluacionSubcontratista(evaluacion_id);
        }
        #endregion

        #region Calendario Evaluaciones

        public Dictionary<string, object> llenarCalendarioEvaluaciones(List<string> lstFiltroCC, List<int?> lstFiltroSubC)
        {
            return this._evaluacionSubcontratistaDAO.llenarCalendarioEvaluaciones(lstFiltroCC, lstFiltroSubC);
        }

        public Dictionary<string, object> buscarEvaluaciones(List<string> cc, List<string> subContratistas)
        {
            return this._evaluacionSubcontratistaDAO.buscarEvaluaciones(cc,subContratistas);
        }
        #endregion

#region Dashboard

        public Dictionary<string, object> FillComboEspecialidad()
        {
            return this._evaluacionSubcontratistaDAO.FillComboEspecialidad();
        }
        //public Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> cc, List<string> subContratistas, List<string> estados, List<string> municipios, List<string> especialidades)
        //{
        //    return this._evaluacionSubcontratistaDAO.CargarDatosGeneralesDashboard(cc, subContratistas, estados, municipios, especialidades);
        //}
	#endregion
        

        #region DASHBOARD

        #region GENERAL
        public Dictionary<string, object> GetGraficaCumplimientoPorSubContratista(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.GetGraficaCumplimientoPorSubContratista(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);
        }

        public Dictionary<string, object> GetGraficaCumplimientoPorElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.GetGraficaCumplimientoPorElementos(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);
        }

        public Dictionary<string, object> GetGraficaCumplimientoPorEvaluacion(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.GetGraficaCumplimientoPorEvaluacion(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);
        }

        public MemoryStream crearReporte(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.crearReporte(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);
        }

        public Dictionary<string, object> GetCumplimientosElementos(List<string> lstFiltroCC, List<int> lstFiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int idElemento, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.GetCumplimientosElementos(lstFiltroCC, lstFiltroSubC, fechaFiltroInicio, fechaFiltroFin,idElemento, estado_id, municipio_id, listaEspecialidades);
        }
        #endregion

        public Dictionary<string, object> GetReporteEjecutivo(List<string> lstfiltroCC, List<int> lstfiltroSubC, DateTime fechaFiltroInicio, DateTime fechaFiltroFin, int estado_id, int municipio_id, List<int> listaEspecialidades)
        {
            return this._evaluacionSubcontratistaDAO.GetReporteEjecutivo(lstfiltroCC, lstfiltroSubC, fechaFiltroInicio, fechaFiltroFin, estado_id, municipio_id, listaEspecialidades);
        }
        public Tuple<MemoryStream, string> DescargarExcelPersonalActivo()
        {
            return _evaluacionSubcontratistaDAO.DescargarExcelPersonalActivo();
        }

        #endregion

        public Dictionary<string, object> FillComboEstados()
        {
            return this._evaluacionSubcontratistaDAO.FillComboEstados();
        }

        public Dictionary<string, object> FillComboMunicipios(int estado_id)
        {
            return this._evaluacionSubcontratistaDAO.FillComboMunicipios(estado_id);
        }
    }
}