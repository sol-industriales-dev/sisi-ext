using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Inventario
{
    public class AutorizacionStandByServices : IAutorizacionStandByDAO
    {

        #region Atributos
        private IAutorizacionStandByDAO m_AutorizacionStandByDAO;
        #endregion
        #region Propiedades
        public IAutorizacionStandByDAO AutorizacionStandByDAO
        {
            get { return m_AutorizacionStandByDAO; }
            set { m_AutorizacionStandByDAO = value; }
        }
        #endregion
        #region Constructores
        public AutorizacionStandByServices(IAutorizacionStandByDAO asignacionEquiposDAO)
        {
            this.AutorizacionStandByDAO = asignacionEquiposDAO;
        }
        #endregion

        public void Guardar(tblM_AutorizacionStandBy obj)
        {
            AutorizacionStandByDAO.Guardar(obj);
        }
        public tblM_AutorizacionStandBy GetAutorizacion(int idAsignacion, int idEconomico)
        {
            return AutorizacionStandByDAO.GetAutorizacion(idAsignacion, idEconomico); ;
        }
        public tblM_AutorizacionStandBy GetAutorizacionByID(int idAutorizacion)
        {
            return AutorizacionStandByDAO.GetAutorizacionByID(idAutorizacion); ;

        }
        public List<rptConciliacionDTO> GetReporte(string cc, DateTime inicio, DateTime fin)
        {
            return AutorizacionStandByDAO.GetReporte(cc, inicio, fin); 

        }

        public List<int> GetAutorizadoresStandby(string cc, DateTime inicio, DateTime fin)
        {
            return AutorizacionStandByDAO.GetAutorizadoresStandby(cc, inicio, fin);

        }
        public tblM_AutorizaStandby GetAutorizacionByIDStanby(int idStandby)
        {
            return AutorizacionStandByDAO.GetAutorizacionByIDStanby(idStandby);
        }
    }
}
