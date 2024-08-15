using Core.DAO.Principal.Archivos;
using Core.DTO;
using Core.Entity.Sistemas;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.Archivos
{
    public class DirArchivosDAO : GenericDAO<tblP_DirArchivos>, IDirArchivosDAO
    {
        public string getUrlDelServidor(int id)
        {
            return _context.tblP_DirArchivos.FirstOrDefault(d => d.id.Equals(id)).dirVirtual;
        }

        public tblP_DirArchivos getRegistro(int id)
        {
            return _context.tblP_DirArchivos.FirstOrDefault(x => x.id == id);
        }
    }
}
