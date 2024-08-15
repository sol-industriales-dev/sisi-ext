using Core.DAO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class EficienciaService : IEficienciaDAO
    {
        #region Atributos
        private IEficienciaDAO m_eficienciaDAO;
        #endregion
        #region Propiedades
        public IEficienciaDAO EficienciaDAO
        {
            get { return m_eficienciaDAO; }
            set { m_eficienciaDAO = value; }
        }
        #endregion
        #region Constructores
        public EficienciaService(IEficienciaDAO eficienciaDAO)
        {
            this.EficienciaDAO = eficienciaDAO;
        }
        #endregion
        public tblM_Eficiencia GuardaEficiencia(tblM_Eficiencia obj)
        {
            return EficienciaDAO.GuardaEficiencia(obj);
        }
        public List<tblM_Eficiencia> getTablaEficiencia(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc)
        {
            return EficienciaDAO.getTablaEficiencia(FechaInicioFiltro, FechaUltimoFiltro, cc);
        }
        public List<tblM_Eficiencia> getEficienciaObraInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro, string cc)
        {
            return EficienciaDAO.getEficienciaObraInfo(FechaInicioFiltro, FechaUltimoFiltro, cc);
        }
        public List<RepEficienciaGeneralDTO> getEficienciaGeneralInfo(DateTime FechaInicioFiltro, DateTime FechaUltimoFiltro)
        {
            return EficienciaDAO.getEficienciaGeneralInfo(FechaInicioFiltro, FechaUltimoFiltro);
        }
    }
}
