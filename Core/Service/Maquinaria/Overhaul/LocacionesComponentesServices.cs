using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;

namespace Core.Service.Maquinaria.Overhaul
{
    public class LocacionesComponentesServices : ILocacionesComponentesDAO
    {
        #region Atributos
        private ILocacionesComponentesDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ILocacionesComponentesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public LocacionesComponentesServices(ILocacionesComponentesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public List<tblM_CatLocacionesComponentes> getLocaciones(bool estatus, string descripcion)
        {
            return interfazDAO.getLocaciones(estatus, descripcion);
        }

        public void Guardar(tblM_CatLocacionesComponentes obj)
        {
            interfazDAO.Guardar(obj);
        }

        public void eliminarLocacion(int idLocacion)
        {
            interfazDAO.eliminarLocacion(idLocacion);
        }

        public List<tblP_CC> getCentrosCostos() 
        {
            return interfazDAO.getCentrosCostos();
        }

        public List<string> GetCorreosLocacionesOverhaul(List<int> idLocaciones) 
        {
            return interfazDAO.GetCorreosLocacionesOverhaul(idLocaciones);
        }
    }
}
