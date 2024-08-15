using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;

namespace Core.Service.Maquinaria.Overhaul
{
    public class MarcasComponentesServices : IMarcasComponentesDAO
    {
        #region Atributos
        private IMarcasComponentesDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IMarcasComponentesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public MarcasComponentesServices(IMarcasComponentesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void Guardar(tblM_CatMarcasComponentes obj) 
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblM_CatMarcasComponentes> getLocaciones(bool estatus, string descripcion) 
        {
            return interfazDAO.getLocaciones(estatus, descripcion);
        }
        public void eliminar(int idMarca) 
        {
            interfazDAO.eliminar(idMarca);
        }
    }
}



