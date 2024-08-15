using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class EscenariosServices : IEscenariosDAO
    {

        #region Atributos
        private IEscenariosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IEscenariosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public EscenariosServices(IEscenariosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<tblPro_CatEscenarios> GetListaEscenariosPrincipales()
        {
            return interfazDAO.GetListaEscenariosPrincipales();
        }
        public void Guardar(tblPro_CatEscenarios obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblPro_CatEscenarios> GetListaEscenarios()
        {
            return interfazDAO.GetListaEscenarios();
        }

        public List<tblPro_CatEscenarios> GetListEscenariosTable(int id, string descripcion)
        {
            return interfazDAO.GetListEscenariosTable(id, descripcion);
        }

        public tblPro_CatEscenarios CatEscenarioByID(int id)
        {
            return interfazDAO.CatEscenarioByID(id);
        }


    }
}
