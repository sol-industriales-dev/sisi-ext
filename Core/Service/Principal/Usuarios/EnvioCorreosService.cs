using Core.DAO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Usuarios
{
    public class EnvioCorreosService : IenvioCorreosDAO
    {
        #region Atributos
        private IenvioCorreosDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IenvioCorreosDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public EnvioCorreosService(IenvioCorreosDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<tblP_EnvioCorreos> GetListaCorreos(int moduloID, string cc)
        {
            return interfazDAO.GetListaCorreos(moduloID, cc);
        }
    }
}
