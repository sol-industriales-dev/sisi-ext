using Core.DAO.Principal.Archivos;
using Core.Entity.Sistemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Archivos
{
    public class DirArchivosService : IDirArchivosDAO
    {
        #region Atributos
        private IDirArchivosDAO p_archivoDAO;
        #endregion
        #region Propiedades
        private IDirArchivosDAO ArchivoDAO
        {
            get { return p_archivoDAO; }
            set { p_archivoDAO = value; }
        }
        #endregion
        #region Constructor
        public DirArchivosService(IDirArchivosDAO ArchivoDAO)
        {
            this.ArchivoDAO = ArchivoDAO;
        }
        #endregion
        public string getUrlDelServidor(int id)
        {
            return ArchivoDAO.getUrlDelServidor(id);
        }

        public tblP_DirArchivos getRegistro(int id)
        {
            return ArchivoDAO.getRegistro(id);
        }
    }
}
