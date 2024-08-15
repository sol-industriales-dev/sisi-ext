using Core.Entity.Facturacion.Prefacturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Facturacion.Prefacturacion
{
    public interface IFilePrefacturacionDAO
    {
        void Guardar(tblF_FilePrefactura obj);
        List<tblF_FilePrefactura> getlistaByPrefactura(int obj);
        tblF_FilePrefactura getlistaByID(int obj);
        bool SaveArchivo(HttpPostedFileBase archivo, string ruta);
    }
}
