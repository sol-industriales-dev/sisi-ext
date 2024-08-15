using Core.DAO.Encuestas;
using Core.DTO.Encuestas.Proveedores;
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

namespace Core.Service.Encuestas
{
    public class EncuestaSubContratistasServices : IEncuestasSubContratistasDAO
    {
        #region Atributos
        private IEncuestasSubContratistasDAO m_encuestasDAO;
        #endregion

        #region Propiedades
        public IEncuestasSubContratistasDAO EncuestasDAO
        {
            get { return m_encuestasDAO; }
            set { m_encuestasDAO = value; }
        }
        #endregion

        #region Constructores
        public EncuestaSubContratistasServices(IEncuestasSubContratistasDAO encuestasDAO)
        {
            this.EncuestasDAO = encuestasDAO;
        }
        #endregion

        public int saveEncuesta(tblEN_EncuestaSubContratista obj, List<tblEN_PreguntasSubContratistas> listObj)
        {
            return EncuestasDAO.saveEncuesta(obj, listObj);
        }

        public int saveEncuestaUpdate(tblEN_EncuestaSubContratista obj, List<tblEN_PreguntasSubContratistas> listObj)
        {
            return EncuestasDAO.saveEncuestaUpdate(obj, listObj);
        }



        public tblEN_EncuestaSubContratista getEncuestaID(int encuestaID)
        {
            return EncuestasDAO.getEncuestaID(encuestaID);
        }

        public List<tblEN_PreguntasSubContratistas> getListaPreguntasByIDEncuesta(int encuestaID)
        {
            return EncuestasDAO.getListaPreguntasByIDEncuesta(encuestaID);
        }

        public dataSubContratistaDTO getInfoSubContratista(string cc, int numPro, int convenio)
        {
            return EncuestasDAO.getInfoSubContratista(cc, numPro, convenio);
        }

        public void saveEncuestaResult(List<tblEN_ResultadoSubContratistas> obj, tblEN_ResultadoSubContratistasDet objSingle, string comentario)
        {
            EncuestasDAO.saveEncuestaResult(obj, objSingle, comentario);
        }
        public List<tblEN_EncuestaSubContratista> fillCboEncuestas(int tipoEncuesta)
        {
            return EncuestasDAO.fillCboEncuestas(tipoEncuesta);
        }


        public List<dataSubContratistaDTO> getListaSubContratistas(DateTime fechaInicio, DateTime fechaFin, int encuestaID)
        {
            return EncuestasDAO.getListaSubContratistas(fechaInicio, fechaFin, encuestaID);
        }

        public List<RespuestasEncuestasDTO> GetEncuestaContestada(int idEncuestaDEtalle)
        {
            return EncuestasDAO.GetEncuestaContestada(idEncuestaDEtalle);
        }
        public tblEN_ResultadoSubContratistasDet GetDetalleEncuesta(int id)
        {
            return EncuestasDAO.GetDetalleEncuesta(id);
        }

        public List<dataSubContratistaDTO> getListaSubContratistasbySubContratista(int contratistaID)
        {
            return EncuestasDAO.getListaSubContratistasbySubContratista(contratistaID);
        }

        public List<subContratistasDTO> getNombreSubContratistas(string term)
        {
            return EncuestasDAO.getNombreSubContratistas(term);
        }

        public DataTable GetMatrizEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            return EncuestasDAO.GetMatrizEvaluacionSubContratista(fechaInicio, fechaFin, cc);
        }

        public DataTable GetMatrizEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            return EncuestasDAO.GetMatrizEvaluacionSubContratistaEstrellas(fechaInicio, fechaFin, cc);
        }

        public List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratista(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            return EncuestasDAO.GetGraficaEvaluacionSubContratista(fechaInicio, fechaFin, cc);
        }

        public List<GraficaEvaluacionSubContratistaDTO> GetGraficaEvaluacionSubContratistaEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc)
        {
            return EncuestasDAO.GetGraficaEvaluacionSubContratistaEstrellas(fechaInicio, fechaFin, cc);
        }

        public List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaEvaluacionSubContratistaCC(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {
            return EncuestasDAO.GetGraficaEvaluacionSubContratistaCC(fechaInicio, fechaFin, cc, subContratista);
        }
        public List<GraficaEvaluacionSubContratistaCCDTO> GetGraficaSubContratistaCCEstrellas(DateTime fechaInicio, DateTime fechaFin, List<string> cc, int subContratista)
        {
            return EncuestasDAO.GetGraficaSubContratistaCCEstrellas(fechaInicio, fechaFin, cc, subContratista);
        }

        public Dictionary<string, object> getExcel(List<dataSubContratistaDTO> lstSubContratistas)
        {
            return EncuestasDAO.getExcel(lstSubContratistas);
        }
    }
}
