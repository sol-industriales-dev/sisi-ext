using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Archivos
{
    public interface IDirArchivosDAO
    {
        string getUrlDelServidor(int id);
        tblP_DirArchivos getRegistro(int id);
    }
}
