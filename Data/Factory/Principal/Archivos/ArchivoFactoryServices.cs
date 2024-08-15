using Core.DAO.Principal.Archivos;
using Core.Service.Principal.Archivos;
using Data.DAO.Principal.Archivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Factory.Principal.Archivos
{
    public class ArchivoFactoryServices
    {
        public IDirArchivosDAO getArchivo()
        {
            return new DirArchivosService(new DirArchivosDAO());
        }
    }
}
