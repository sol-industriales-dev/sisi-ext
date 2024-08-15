using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
   public class TerminacionObraServices : ITerminacionObraDAO
    {
                #region Atributos
        private ITerminacionObraDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ITerminacionObraDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public TerminacionObraServices(ITerminacionObraDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void Guardar(tblPro_CierreObra obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblPro_CierreObra> GetObrasCerradasByID(int capaturaObrasID, int registroID)
        {
           return interfazDAO.GetObrasCerradasByID(capaturaObrasID,registroID);
        }
         public List<tblPro_CierreObra> GetObrasCerradas()
        {
           return interfazDAO.GetObrasCerradas();
        }
    }
}
