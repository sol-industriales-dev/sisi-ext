using Core.DAO.Principal.Bitacoras;
using Core.Enum.Principal.Bitacoras;
using System;

namespace Core.Service.Principal.Bitacora
{
    public class LogErrorService : ILogErrorDAO
    {
        #region Atributos
        private ILogErrorDAO m_logErrorDAO;
        #endregion Atributos

        #region Propiedades
        private ILogErrorDAO LogErrorDAO
        {
            get { return m_logErrorDAO; }
            set { m_logErrorDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public LogErrorService(ILogErrorDAO LogErrorDAO)
        {
            this.LogErrorDAO = LogErrorDAO;
        }
        #endregion Constructores

        public void LogError(int sistema, int modulo, string controlador, string accion, Exception excepcion, AccionEnum tipo, long registroID, object objeto)
        {
            LogErrorDAO.LogError(sistema, modulo, controlador, accion, excepcion, tipo, registroID, objeto);
        }
    }
}
