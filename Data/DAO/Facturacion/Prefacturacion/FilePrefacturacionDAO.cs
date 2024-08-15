using Core.DAO.Facturacion.Prefacturacion;
using Core.Entity.Facturacion.Prefacturacion;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Facturacion.Prefacturacion
{
    public class FilePrefacturacionDAO : GenericDAO<tblF_FilePrefactura>, IFilePrefacturacionDAO
    {
        public void Guardar(tblF_FilePrefactura obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.FilePrefactura);
            else
                Update(obj, obj.id, (int)BitacoraEnum.FilePrefactura);
        }

        public List<tblF_FilePrefactura> getlistaByPrefactura(int obj)
        {
            return _context.tblF_FilePrefactura.Where(x => x.idRepFactura.Equals(obj)).ToList();
        }

        public tblF_FilePrefactura getlistaByID(int obj)
        {
            return _context.tblF_FilePrefactura.FirstOrDefault(x => x.id.Equals(obj));
        }
        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            byte[] data;
            using (Stream inputStream = archivo.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            ruta = ruta.Replace("C:\\", "\\REPOSITORIO\\");
            File.WriteAllBytes(ruta, data);

            return File.Exists(ruta);
        }
    }
}
