using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Maquinaria.StandBy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class StandByNuevoServices : IStandByNuevoDAO
    {
        #region Atributos
        private IStandByNuevoDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IStandByNuevoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public StandByNuevoServices(IStandByNuevoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion
        public bool GuardarCaptura(List<tblM_STB_CapturaStandBy> lst)
        {
           return interfazDAO.GuardarCaptura(lst);
        }
        public bool GuardarValidacion(List<StandByNuevoDTO> lst)
        {
           return interfazDAO.GuardarValidacion(lst);
        }
        public bool GuardarLibracion(List<StandByNuevoDTO> lst)
        {
           return interfazDAO.GuardarLibracion(lst);
        }
        public List<tblM_CatMaquina> getListaDisponible(string cc)
        {
           return interfazDAO.getListaDisponible(cc);
        }
        public List<tblM_STB_CapturaStandBy> getListaByEstatus(int estatus, string noAC, string noEconomico)
        {
           return interfazDAO.getListaByEstatus(estatus,noAC,noEconomico);
        }
        public List<tblM_STB_CapturaStandBy> getListaByEstatusConDepreciacion(int estatus, string noAC, string noEconomico, DateTime fechaInicio, DateTime fechaFin, int tipo)
        {
            return interfazDAO.getListaByEstatusConDepreciacion(estatus, noAC, noEconomico, fechaInicio, fechaFin, tipo);
        }


        public List<DepreciacionLugarDTO> getDepreciacionPorStandBy(string ac, string economico, DateTime fechaInicio, DateTime fechaFin, bool corteSemanal)
        {
            return interfazDAO.getDepreciacionPorStandBy(ac, economico, fechaInicio, fechaFin, corteSemanal);
        }

        public List<DepreciacionLugarDTO> getDepreciacionPorNoasignado(string economico, DateTime fechaInicio, DateTime fechaFin, bool corteSemanal)
        {
            return interfazDAO.getDepreciacionPorNoasignado( economico, fechaInicio, fechaFin, corteSemanal);
        }

        public List<DateTime> getDiasMartes(DateTime inicio, DateTime fin)
        {
            return interfazDAO.getDiasMartes(inicio, fin);
        }

        public bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false)
        {
            return interfazDAO.ActivarEconomicoPorAccionRealizada(numeroEconomico, idEconomico, accion, objeto, buscarEnEnkontrol);
        }

        public Dictionary<string, object> GetUsuarioTipoAutorizacion()
        {
            return interfazDAO.GetUsuarioTipoAutorizacion();
        }

        public Dictionary<string, object> GuardarVoBo(List<StandByNuevoDTO> lstStandByDTO)
        {
            return interfazDAO.GuardarVoBo(lstStandByDTO);
        }
    }
}