using Core.DAO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Overhaul
{
    public class ArchivosNotasCreditoSevices : IArchivosNotasCreditoDAO
    {
          #region Atributos
        private IArchivosNotasCreditoDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IArchivosNotasCreditoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public ArchivosNotasCreditoSevices(IArchivosNotasCreditoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void Guardar(tblM_ArchivosNotasCredito obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblM_ArchivosNotasCredito> getlistaByNota(int obj)
        {
            return interfazDAO.getlistaByNota(obj);
        }
        public tblM_ArchivosNotasCredito getlistaByID(int obj)
        {
            return interfazDAO.getlistaByID(obj);
        }

        public List<string> getlistaArchivosAdjuntos(int obj)
        {
            return interfazDAO.getlistaArchivosAdjuntos(obj);
        }

    }
}
