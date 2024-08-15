using Core.DAO.Facturacion.Prefacturacion;
using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Facturacion.Prefacturacion
{
    public class FilePrefacturacionService : IFilePrefacturacionDAO
    {
        private IFilePrefacturacionDAO f_IFilePrefacturacionDAO;

        public IFilePrefacturacionDAO FilePrefacturacion
        {
            get { return f_IFilePrefacturacionDAO; }
            set { f_IFilePrefacturacionDAO = value; }
        }

        public FilePrefacturacionService(IFilePrefacturacionDAO filePrefacturacion)
        {
            this.f_IFilePrefacturacionDAO = filePrefacturacion;
        }

        public void Guardar(tblF_FilePrefactura obj)
        {
            FilePrefacturacion.Guardar(obj);
        }
        public List<tblF_FilePrefactura> getlistaByPrefactura(int obj)
        {
            return FilePrefacturacion.getlistaByPrefactura(obj);
        }

        public tblF_FilePrefactura getlistaByID(int obj)
        {
            return FilePrefacturacion.getlistaByID(obj);
        }
        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            return FilePrefacturacion.SaveArchivo(archivo, ruta);
        }
    }
}
