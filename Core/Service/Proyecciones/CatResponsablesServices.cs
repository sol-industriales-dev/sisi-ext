using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CatResponsablesServices : ICatResponsablesDAO
    {
        #region Atributos
        private ICatResponsablesDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICatResponsablesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CatResponsablesServices(ICatResponsablesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public tblPro_CatResponsables GetDataById(int id)
        {
            return interfazDAO.GetDataById(id);
        }
        public void Guardar(tblPro_CatResponsables obj)
        {
            interfazDAO.Guardar(obj);
        }

        public  List<tblPro_CatResponsables> fillCboResponsables()
        {
            return interfazDAO.fillCboResponsables();
        }
    }
}
