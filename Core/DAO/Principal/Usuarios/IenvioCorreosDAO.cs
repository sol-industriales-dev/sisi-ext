using Core.Entity.Principal.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Usuarios
{
    public interface IenvioCorreosDAO
    {
        List<tblP_EnvioCorreos> GetListaCorreos(int moduloID, string cc);

    }
}
