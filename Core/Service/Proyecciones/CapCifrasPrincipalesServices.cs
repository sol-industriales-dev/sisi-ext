using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CapCifrasPrincipalesServices : ICapCifrasPrincipalesDAO
    {
        #region Atributos
        private ICapCifrasPrincipalesDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private ICapCifrasPrincipalesDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public CapCifrasPrincipalesServices(ICapCifrasPrincipalesDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public void Guardar(tblPro_CapCifrasPrincipales obj)
        {
            interfazDAO.Guardar(obj);
        }

        public void BorrarEscenarios(int mes, int anio, string escenarios) 
        {
            interfazDAO.BorrarEscenarios(mes, anio, escenarios);
        }

        public tblPro_CapCifrasPrincipales getOBJCifrasPrincipales(int mes, int anio, string escenario, int tipo)
        {
            return interfazDAO.getOBJCifrasPrincipales(mes, anio, escenario, tipo);
        }

        public List<string> getEscenariosConfiguraciones(int mes, int anio)
        {
            return interfazDAO.getEscenariosConfiguraciones(mes, anio);
        }

    }
}
