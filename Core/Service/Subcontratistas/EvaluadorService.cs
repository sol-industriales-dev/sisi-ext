using Core.DAO.Subcontratistas;
using Core.DTO.ControlObra;
using Core.DTO.ControlObra.EvaluacionSubcontratista;
using Core.DTO.Principal.Generales;
using Core.DTO.Subcontratistas.Evaluacion;
using Core.Entity.ControlObra;
using Core.Entity.Encuestas;
using Core.Entity.SubContratistas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Subcontratistas
{
    public class EvaluadorService : IEvaluadorDAO
    {
        private IEvaluadorDAO _evaluacionSubcontratistaDAO;

        public IEvaluadorDAO evaluacionSubcontratista
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
        public EvaluadorService(IEvaluadorDAO evaluacionSubcontratista)
        {
            this.evaluacionSubcontratista = evaluacionSubcontratista;
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
        public SubContratistasDTO getTblSubContratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.getTblSubContratista(parametros);
        }
        public SubContratistasDTO addEditSubContratista(List<HttpPostedFileBase> Archivo, List<SubContratistasDTO> parametros, tblPUsuarioDTO ObjUsuario, bool subcontratista)
        {
            return _evaluacionSubcontratistaDAO.addEditSubContratista(Archivo, parametros, ObjUsuario, subcontratista);
        }
        public List<SubContratistasDTO> CargarArchivosXSubcontratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosXSubcontratista(parametros);
        }
        public List<SubContratistasDTO> CargarArchivosSubcontratista(SubContratistasDTO parametros, tblPUsuarioDTO ObjUsuario)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosSubcontratista(parametros, ObjUsuario);
        }

        public List<SubContratistasDTO> CargarArchivosXSubcontratistaEvaluacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.CargarArchivosXSubcontratistaEvaluacion(parametros);
        }

        public List<SubContratistasDTO> ObtenerTblAutorizacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerTblAutorizacion(parametros);
        }
        public Dictionary<string, object> obtenerPlantillas()
        {
            return _evaluacionSubcontratistaDAO.obtenerPlantillas();
        }
        public DivicionesMenuDTO addEditPlantilla(DivicionesMenuDTO parametros)
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
        public List<DivicionesMenuDTO> obtenerDiviciones(int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerDiviciones(idPlantilla, idAsignacion);
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluador()
        {
            return _evaluacionSubcontratistaDAO.obtenerDivicionesEvaluador();
        }
        public List<DivicionesMenuDTO> obtenerDivicionesEvaluadorArchivos(int idPlantilla, int idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerDivicionesEvaluadorArchivos(idPlantilla, idAsignacion);
        }

        public DivicionesMenuDTO addEditDiviciones(DivicionesMenuDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.addEditDiviciones(parametros);
        }
        public DivicionesMenuDTO eliminarDiviciones(int id)
        {
            return _evaluacionSubcontratistaDAO.eliminarDiviciones(id);
        }
        public List<RequerimientosDTO> obtenerRequerimientos(int idDiv, int? idAsignacion)
        {
            return _evaluacionSubcontratistaDAO.obtenerRequerimientos(idDiv, idAsignacion);
        }

        public byte[] DescargarArchivos(long idDet)
        {
            return _evaluacionSubcontratistaDAO.DescargarArchivos(idDet);
        }
        public string getFileName(long idDet)
        {
            return _evaluacionSubcontratistaDAO.getFileName(idDet);
        }
        public SubContratistasDTO obtenerEvaluacionxReq(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerEvaluacionxReq(parametros);
        }
        public SubContratistasDTO GuardarEvaluacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.GuardarEvaluacion(parametros);
        }
        public SubContratistasDTO obtenerPromegioEvaluacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerPromegioEvaluacion(parametros);
        }

        public Dictionary<string, object> ObtenerGraficaDeBarras(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.ObtenerGraficaDeBarras(parametros);
        }

        public List<SubContratistasDTO> obtenerPromediosxSubcontratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.obtenerPromediosxSubcontratista(parametros);
        }
        public bool ObtenerEvaluacionPendiente(tblPUsuarioDTO ObjUsuario)
        {
            return _evaluacionSubcontratistaDAO.ObtenerEvaluacionPendiente(ObjUsuario);
        }
        public List<ContratoSubContratistaDTO> obtenerContratistasConContrato(string AreaCuenta, int subcontratista, int Estatus, tblPUsuarioDTO ObjUsuario)
        {
            return _evaluacionSubcontratistaDAO.obtenerContratistasConContrato(AreaCuenta, subcontratista, Estatus, ObjUsuario);
        }
        public SubContratistasDTO GuardarEvaluacionSubContratista(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.GuardarEvaluacionSubContratista(parametros);
        }
        public SubContratistasDTO GuardarAsignacion(SubContratistasDTO parametros)
        {
            return _evaluacionSubcontratistaDAO.GuardarAsignacion(parametros);
        }
        public List<SubContratistasDTO> getUsuariosAutorizantes(string term)
        {
            return _evaluacionSubcontratistaDAO.getUsuariosAutorizantes(term);
        }
        public SubContratistasDTO AutorizarEvaluacion(SubContratistasDTO parametros, int idUsuario)
        {
            return _evaluacionSubcontratistaDAO.AutorizarEvaluacion(parametros, idUsuario);
        }
        public SubContratistasDTO AutorizarAsignacion(SubContratistasDTO parametros, int idUsuario)
        {
            return _evaluacionSubcontratistaDAO.AutorizarAsignacion(parametros, idUsuario);
        }

        public List<ElementosDTO> obtenerTodosLosElementosConSuRequerimiento()
        {
            return _evaluacionSubcontratistaDAO.obtenerTodosLosElementosConSuRequerimiento();
        }
        public ElementosDTO guardarRelacion(List<ElementosDTO> lstRelacion)
        {
            return _evaluacionSubcontratistaDAO.guardarRelacion(lstRelacion);
        }
        public List<ContratoSubContratistaDTO> tblObtenerDashBoardSubContratista(tblPUsuarioDTO ObjUsuario)
        {
            return _evaluacionSubcontratistaDAO.tblObtenerDashBoardSubContratista(ObjUsuario);
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
        public List<evaluadorXccDTO> getEvaluadoresxCC()
        {
            return _evaluacionSubcontratistaDAO.getEvaluadoresxCC();
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
        public Dictionary<string, object> obtenerElementosEvaluar(int idUsuario, int idPlantilla)
        {
            return _evaluacionSubcontratistaDAO.obtenerElementosEvaluar(idUsuario, idPlantilla);
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

        #region Gestión
        //public Dictionary<string, object> ObtenerProyectosParaFiltro()
        //{
        //    return this._evaluacionSubcontratistaDAO.ObtenerProyectosParaFiltro();
        //}

        //public Dictionary<string, object> ObtenerSubcontratistasParaFiltro(string proyecto)
        //{
        //    return this._evaluacionSubcontratistaDAO.ObtenerSubcontratistasParaFiltro(proyecto);
        //}

        //public Dictionary<string, object> ObtenerEvaluacionesSubcontratistas(string proyecto, int? subcontratistaId)
        //{
        //    return this._evaluacionSubcontratistaDAO.ObtenerEvaluacionesSubcontratistas(proyecto, subcontratistaId);
        //}

        //public Dictionary<string, object> ObtenerEstatusFirmantes(int evaluacionId)
        //{
        //    return this._evaluacionSubcontratistaDAO.ObtenerEstatusFirmantes(evaluacionId);
        //}

        //public Dictionary<string, object> ObtenerFirmante(int evaluacionId)
        //{
        //    return this._evaluacionSubcontratistaDAO.ObtenerFirmante(evaluacionId);
        //}

        //public Dictionary<string, object> GuardarFirma(InformacionFirmaDigitalDTO firma)
        //{
        //    return this._evaluacionSubcontratistaDAO.GuardarFirma(firma);
        //}
        public Dictionary<string, object> obtenerPromedioGeneral(int id)
        {
            return this._evaluacionSubcontratistaDAO.obtenerPromedioGeneral(id);
        }
        public Dictionary<string, object> cambiarDeColor(int idPlantilla, int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.cambiarDeColor(idPlantilla, idAsignacion);
        }
        public List<DivReqDTO> obtenerLst(int idPlantilla, int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.obtenerLst(idPlantilla, idAsignacion);
        }
        public List<tblX_FirmaEvaluacion> obtenerFirmas(int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.obtenerFirmas(idAsignacion);
        }
        public List<excelDTO> obtenerListado2(int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.obtenerListado2(idAsignacion);
        }
        public tblCO_ADP_EvalSubConAsignacion traermeDatosPrincipales(int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.traermeDatosPrincipales(idAsignacion);
        }
        public List<tblCO_ADP_EvalSubConAsignacion> obtenerTodasLasASinaciones(int idAsignacion)
        {
            return this._evaluacionSubcontratistaDAO.obtenerTodasLasASinaciones(idAsignacion);
        }
        #endregion


    }
}
