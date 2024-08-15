using Core.DAO.Maquinaria.Catalogos;
using Core.Entity.Maquinaria.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class ArchivosModelosSevices : IArchivosModelosDAO
    {
                  #region Atributos
        private IArchivosModelosDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private IArchivosModelosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public ArchivosModelosSevices(IArchivosModelosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores

        public void GuardarArchivos(tblM_ArchivosModelos obj)
        {
            interfazDAO.GuardarArchivos(obj);
        }
        public List<tblM_ArchivosModelos> getlistaByModelo(int obj)
        {
            return interfazDAO.getlistaByModelo(obj);
        }
        public tblM_ArchivosModelos getlistaByID(int obj)
        {
            return interfazDAO.getlistaByID(obj);
        }

    }
}
