using Core.Enum.Principal.Bitacoras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Bitacoras
{
    public interface ILogErrorDAO
    {
        void LogError(
            int sistema,
            int modulo,
            string controlador,
            string accion,
            Exception excepcion,
            AccionEnum tipo,
            long registroID,
            object objeto
            );
    }
}
