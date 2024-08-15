using Core.DTO.Encuestas.Proveedores.Reportes;
using Core.DTO.Encuestas.SubContratista;
using Core.Entity.Encuestas;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Encuestas
{
    public interface IEncuestasSubContratistasDAO
    {
        //Guardado y Actualizacion de Encuestas Para crear
        int saveEncuesta(tblEN_EncuestaSubContratista encuestaSubContratistaObj, List<tblEN_PreguntasSubContratistas> listPreguntasSubContraistaObj);
        int saveEncuestaUpdate(tblEN_EncuestaSubContratista encuestaSubContratistaObj, List<tblEN_PreguntasSubContratistas> listPreguntasSubContraistaObj);

        //Fin de Acciones
        void saveEncuestaResult(List<tblEN_ResultadoSubContratistas> obj, tblEN_ResultadoSubContratistasDet objSingle, string comentario);

        tblEN_EncuestaSubContratista getEncuestaID(int encuestaID);
        List<tblEN_PreguntasSubContratistas> getListaPreguntasByIDEncuesta(int encuestaID);
        dataSubContratistaDTO getInfoSubContratista(string cc, int numPro, int convenio);

        List<tblEN_EncuestaSubContratista> fillCboEncuestas(int tipoEncuesta);
        List<dataSubContratistaDTO> getListaSubContratistas(DateTime fechaInicio, DateTime fechaFin, int encuestaID);
        List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle);
        tblEN_ResultadoSubContratistasDet GetDetalleEncuesta(int id);

        List<dataSubContratistaDTO> getListaSubContratistasbySubContratista(int contratistaID);

        List<subContratistasDTO> getNombreSubContratistas(string term);
        DataTable GetMatrizEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc);
        DataTable GetMatrizEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc);

        List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc);
        List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc);
        List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaEvaluacionSubContratistaCC(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista);
        List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaSubContratistaCCEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista);

        Dictionary<string, object> getExcel(List<dataSubContratistaDTO> lstSubContratistas);
    }

}
