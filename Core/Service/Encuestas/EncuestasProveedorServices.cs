using Core.DAO.Encuestas;
using Core.DTO;
using Core.DTO.COMPRAS;
using Core.DTO.Encuestas;
using Core.DTO.Encuestas.Proveedores;
using Core.DTO.Encuestas.Proveedores.Reportes;
using Core.DTO.Principal.Generales;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Encuestas
{
    public class EncuestasProveedorServices : IEncuestasProveedorDAO
    {
        #region Atributos
        private IEncuestasProveedorDAO m_encuestasDAO;
        #endregion

        #region Propiedades
        public IEncuestasProveedorDAO EncuestasDAO
        {
            get { return m_encuestasDAO; }
            set { m_encuestasDAO = value; }
        }
        #endregion

        #region Constructores
        public EncuestasProveedorServices(IEncuestasProveedorDAO encuestasDAO)
        {
            this.EncuestasDAO = encuestasDAO;
        }
        #endregion

        public Respuesta ResponderEncuestaTop20(int tipoEncuesta, int encuestaID, int numeroProveedor)
        {
            return EncuestasDAO.ResponderEncuestaTop20(tipoEncuesta, encuestaID, numeroProveedor);
        }

        public Respuesta ResponderEncuestaTop20Peru(int tipoEncuesta, int encuestaID, string numeroProveedor)
        {
            return EncuestasDAO.ResponderEncuestaTop20Peru(tipoEncuesta, encuestaID, numeroProveedor);
        }

        public Respuesta GetProveedoresTop20(int idUsuario, int tipoEncuestaId, bool desdeCompras = false)
        {
            return EncuestasDAO.GetProveedoresTop20(idUsuario, tipoEncuestaId, desdeCompras);
        }

        public int saveEncuestasProveedores(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj)
        {
            return EncuestasDAO.saveEncuestasProveedores(obj, listObj);
        }

        public int getTipoEncuesta(int encuestaId)
        {
            return EncuestasDAO.getTipoEncuesta(encuestaId);
        }

        public Respuesta setGraficaPreguntasProv(int numProv, int tipoEncuesta, int encuestaID, DateTime fechaIni, DateTime fechaFin, List<int> listaUsuario)
        {
            return EncuestasDAO.setGraficaPreguntasProv(numProv, tipoEncuesta, encuestaID, fechaIni, fechaFin, listaUsuario);
        }

        public Respuesta setGraficaMensualProv(int numProv, int tipoEncuesta, int encuestaID, List<int> listaUsuario)
        {
            return EncuestasDAO.setGraficaMensualProv(numProv, tipoEncuesta, encuestaID, listaUsuario);
        }

        public List<tblP_Usuario> getUsuariosRealizadoEncuestas()
        {
            return EncuestasDAO.getUsuariosRealizadoEncuestas();
        }

        public List<PreguntasDTO> getPreguntasProveedores(int encuestaID)
        {
            return EncuestasDAO.getPreguntasProveedores(encuestaID);
        }

        public EncuentaDTO getEncuestaResult(int id)
        {
            return EncuestasDAO.getEncuestaResult(id);
        }

        public List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.getEncuestaResults(id, fechaInicio, fechaFin);
        }

        public void updateEncuesta(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj)
        {
            EncuestasDAO.updateEncuesta(obj, listObj);
        }

        public List<ResultProveedoresDTO> evaluacionesRespondidas(int estatus, DateTime fechaInicio, DateTime fechaFin, List<int> compradores)
        {
            return EncuestasDAO.evaluacionesRespondidas(estatus, fechaInicio, fechaFin, compradores);
        }

        public List<ResultProveedoresDTO> loadEncuestas(int estatus, DateTime fechaInicio, DateTime fechaFin)
        {
            return EncuestasDAO.loadEncuestas(estatus, fechaInicio, fechaFin);
        }
        public List<tblEN_EncuestaProveedores> getEncuestasByTipo(int tipoEncuesta)
        {
            return EncuestasDAO.getEncuestasByTipo(tipoEncuesta);
        }

        public List<ResultProveedoresDTO> loadEncuestasByOC(int numeroOC, string centrocostos)
        {
            return EncuestasDAO.loadEncuestasByOC(numeroOC, centrocostos);
        }


        public tblEN_EncuestaProveedores getEncuesta(int id)
        {
            return EncuestasDAO.getEncuesta(id);

        }

        public ResultProveedoresDTO datosProveedor(int encuestaID, int numeroOC, string centrocostos)
        {
            return EncuestasDAO.datosProveedor(encuestaID, numeroOC, centrocostos);
        }

        public Respuesta saveEncuestaResult(List<tblEN_ResultadoProveedores> obj, tblEN_ResultadoProveedoresDet objSingle, int tipoEncuesta)
        {
            return EncuestasDAO.saveEncuestaResult(obj, objSingle, tipoEncuesta);
        }

        public Respuesta saveEncuestaResultReq(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, int tipoEncuesta)
        {
            return EncuestasDAO.saveEncuestaResultReq(obj, objSingle, tipoEncuesta);
        }

        public List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle, int tipo)
        {
            return EncuestasDAO.GetEncuestaContestada(idEncuestaDEtalle, tipo);
        }
        public tblEN_ResultadoProveedoresDet GetEncabezadoEncuesta(int proveedorDetID, int ptipo)
        {
            return EncuestasDAO.GetEncabezadoEncuesta(proveedorDetID, ptipo);
        }

        public tblEN_ResultadoProveedorRequisicionDet GetEncabezadoEncuestaRequisicion(int proveedorDetID, int ptipo)
        {
            return EncuestasDAO.GetEncabezadoEncuestaRequisicion(proveedorDetID, ptipo);
        }

        public List<tblRequisicionesDTO> loadRequisiciones(int estatus, DateTime fechainicio, DateTime fechafin)
        {
            return EncuestasDAO.loadRequisiciones(estatus, fechainicio, fechafin);
        }

        public tblRequisicionesDTO loadEncuestasByRequisicion(int requisicion, string centrocostos)
        {
            return EncuestasDAO.loadEncuestasByRequisicion(requisicion, centrocostos);
        }

        public void saveEncuestaResultRequisiciones(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, string comentario)
        {
            EncuestasDAO.saveEncuestaResultRequisiciones(obj, objSingle, comentario);
        }

        public List<tblRequisicionesDTO> loadRequisicionesByFiltros(int estatus, DateTime fechainicio, DateTime fechafin, List<int> compradores)
        {
            return EncuestasDAO.loadRequisicionesByFiltros(estatus, fechainicio, fechafin, compradores);
        }

        public ObjGraficasDTO getGraficaEvaluacionProveedores(DateTime fechainicio, DateTime fechafin, int encuesta, int tipoEncuesta, List<int> listaUsuarios)
        {
            return EncuestasDAO.getGraficaEvaluacionProveedores(fechainicio, fechafin, encuesta, tipoEncuesta, listaUsuarios);
        }

        public List<ResEvaluacionProveedoresDTO> getProveedoresCalificaciones(List<int> rawListProveedores, int tipoEncuesta)
        {
            return EncuestasDAO.getProveedoresCalificaciones(rawListProveedores, tipoEncuesta);
        }

        public List<ResEvaluacionProveedoresDTO> getProveedoresCalificacionesEstrellas(List<int> rawListProveedores, int tipoEncuesta)
        {
            return EncuestasDAO.getProveedoresCalificacionesEstrellas(rawListProveedores, tipoEncuesta);
        }

        public List<ProveedoresDTO> getNombreProveedores(string term)
        {
            return EncuestasDAO.getNombreProveedores(term);
        }

        public string getEncuestaByFolioIDOC(int id)
        {
            return EncuestasDAO.getEncuestaByFolioIDOC(id);
        }

        public List<ComboDTO> ComboDtoProveedores(List<int> listaFoliosID, int tipoEncuesta)
        {
            return EncuestasDAO.ComboDtoProveedores(listaFoliosID, tipoEncuesta);
        }
        public List<ComboDTO> comboTipoEncuesta()
        {
            return EncuestasDAO.comboTipoEncuesta();
        }
        public List<ResEvaluacionProveedoresDTO> ListaProveedores(List<int> detProveedorDet, int tipoEncuesta)
        {
            return EncuestasDAO.ListaProveedores(detProveedorDet, tipoEncuesta);
        }

        public List<ResEvaluacionProveedoresDTO> ListaProveedoresEstrellas(List<int> detProveedorDet, int tipoEncuesta)
        {
            return EncuestasDAO.ListaProveedoresEstrellas(detProveedorDet, tipoEncuesta);
        }

    }
}
