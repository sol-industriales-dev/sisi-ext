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

namespace Core.DAO.Encuestas
{
    public interface IEncuestasProveedorDAO
    {
        Respuesta setGraficaMensualProv(int numProv, int tipoEncuesta, int encuestaID, List<int> listaUsuario);
        Respuesta setGraficaPreguntasProv(int numProv, int tipoEncuesta, int encuestaID, DateTime fechaIni, DateTime fechaFin, List<int> listaUsuario);
        Respuesta ResponderEncuestaTop20(int tipoencuesta, int encuestaID, int numeroProveedor);
        Respuesta ResponderEncuestaTop20Peru(int tipoencuesta, int encuestaID, string numeroProveedor);
        Respuesta GetProveedoresTop20(int idUsuario, int tipoEncuestaId, bool desdeCompras = false);
        int getTipoEncuesta(int encuestaId);
        List<tblP_Usuario> getUsuariosRealizadoEncuestas();
        List<ResEvaluacionProveedoresDTO> ListaProveedores(List<int> detProveedorDet, int tipoEncuesta);
        List<ResEvaluacionProveedoresDTO> ListaProveedoresEstrellas(List<int> detProveedorDet, int tipoEncuesta);
        List<ResultProveedoresDTO> loadEncuestas(int estatus, DateTime fechaInicio, DateTime fechaFin);
        int saveEncuestasProveedores(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj);
        void updateEncuesta(tblEN_EncuestaProveedores obj, List<tblEN_PreguntasProveedores> listObj);
        List<PreguntasDTO> getPreguntasProveedores(int encuestaID);
        EncuentaDTO getEncuestaResult(int id);
        List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin);
        List<ResultProveedoresDTO> evaluacionesRespondidas(int estatus, DateTime fechaInicio, DateTime fechaFin, List<int> compradores);
        List<tblEN_EncuestaProveedores> getEncuestasByTipo(int tipoEncuesta);
        List<ResultProveedoresDTO> loadEncuestasByOC(int numeroOC, string centrocostos);
        tblEN_EncuestaProveedores getEncuesta(int id);
        ResultProveedoresDTO datosProveedor(int encuestaID, int numeroOC, string centrocostos);
        Respuesta saveEncuestaResult(List<tblEN_ResultadoProveedores> obj, tblEN_ResultadoProveedoresDet objSingle, int tipoEncuesta);
        Respuesta saveEncuestaResultReq(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, int tipoEncuesta);
        List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle, int tipo);
        tblEN_ResultadoProveedoresDet GetEncabezadoEncuesta(int proveedorDetID, int ptipo);
        tblEN_ResultadoProveedorRequisicionDet GetEncabezadoEncuestaRequisicion(int proveedorDetID, int ptipo);

        List<tblRequisicionesDTO> loadRequisiciones(int estatus, DateTime fechainicio, DateTime fechafin);
        List<tblRequisicionesDTO> loadRequisicionesByFiltros(int estatus, DateTime fechainicio, DateTime fechafin, List<int> compradores);
        tblRequisicionesDTO loadEncuestasByRequisicion(int requisicion, string centrocostos);
        void saveEncuestaResultRequisiciones(List<tblEN_ResultadoProveedorRequisiciones> obj, tblEN_ResultadoProveedorRequisicionDet objSingle, string comentario);
        ObjGraficasDTO getGraficaEvaluacionProveedores(DateTime fechainicio, DateTime fechafin, int encuesta, int tipoEncuesta, List<int> listaUsuarios);

        List<ResEvaluacionProveedoresDTO> getProveedoresCalificaciones(List<int> rawListProveedores, int tipoEncuesta);

        List<ProveedoresDTO> getNombreProveedores(string term);

        List<ResEvaluacionProveedoresDTO> getProveedoresCalificacionesEstrellas(List<int> rawListProveedores, int tipoEncuesta);

        string getEncuestaByFolioIDOC(int id);

        List<ComboDTO> ComboDtoProveedores(List<int> listaFoliosID, int tipoEncuesta);
        List<ComboDTO> comboTipoEncuesta();
//        void getAllProveedores(List<int> listaFoliosID);
    }
}
