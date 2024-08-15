using Core.DAO.Maquinaria.Captura;
using Core.Entity.Maquinaria;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.RecursosHumanos.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Capturas
{
    public class CapturaOTServices : ICapturaOTDAO
    {
        #region Atributos
        private ICapturaOTDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICapturaOTDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CapturaOTServices(ICapturaOTDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void Guardar(tblM_CapOrdenTrabajo obj, int idBL)
        {
            interfazDAO.Guardar(obj, idBL);
        }
        public List<tblRH_CatEmpleados> getCatEmpleados(string term, List<string> CentroCostos)
        {
            return interfazDAO.getCatEmpleados(term, CentroCostos);
        }

        public List<tblM_CapOrdenTrabajo> getListaOT(string cc, List<string> listcc)
        {
            return interfazDAO.getListaOT(cc, listcc);
        }
        public List<tblM_DetOrdenTrabajo> getListaOTDet(string cc, List<string> listcc,DateTime fechaInicio,DateTime fechaFin)
        {
            return interfazDAO.getListaOTDet(cc, listcc, fechaInicio, fechaFin);
        }
        public tblM_CapOrdenTrabajo GetOTbyID(int id)
        {
            return interfazDAO.GetOTbyID(id);
        }

        public tblM_CapOrdenTrabajo GetOTByEconomico(int idEconomico)
        {
            return interfazDAO.GetOTByEconomico(idEconomico);
        }

    }
}
