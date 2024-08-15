using Core.DAO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Archivos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Maquinaria.Inventario
{
    public class SolicitudEquipoReemplazoDetDAO : GenericDAO<tblM_SolicitudReemplazoDet>, ISolicitudEquipoReemplazoDetDAO
    {
        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        public void Guardar(List<tblM_SolicitudReemplazoDet> obj)
        {
            if (true)
            {
                saveEntitys(obj, (int)BitacoraEnum.SolicitudReempazoDet);
            }
            else
            {
                throw new Exception("");
            }

        }

        public void GuardarSingle(tblM_SolicitudReemplazoDet obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.SolicitudReempazoDet);
            else
                Update(obj, obj.id, (int)BitacoraEnum.SolicitudReempazoDet);
        }



        public void GuardarDetalleArchivos(tblM_SolicitudReemplazoDet obj, HttpPostedFileBase file)
        {
            if (true)
            {
                if (obj.id == 0)
                {

                    if (file != null)
                    {

                        string extension = Path.GetExtension(file.FileName);
                        SaveEntity(obj, (int)BitacoraEnum.SolicitudReempazoDet);
                        DateTime fechanow = DateTime.Now;
                        string fecha = fechanow.ToString("ddMMyyyyHHmmss");
                        string FileName = file.FileName;
                        string nomnbreArchivoPath = obj.id + fecha + extension;

                        string Ruta = archivofs.getArchivo().getUrlDelServidor(11) + nomnbreArchivoPath;
                        SaveArchivo(file, Ruta);
                        obj.ruta = Ruta;
                        obj.nombreArchivo = FileName;
                        Update(obj, obj.id, (int)BitacoraEnum.SolicitudReempazoDet);
                    }
                }
                else
                    Update(obj, obj.id, (int)BitacoraEnum.SolicitudReempazoDet);
            }
        }

        public List<tblM_SolicitudReemplazoDet> GetSolicitudReemplazoDetByIdSolicitud(int id)
        {
            return _context.tblM_SolicitudReemplazoDet.Where(x => x.SolicitudEquipoReemplazoID==id).ToList();

        }
        public tblM_SolicitudReemplazoDet GetSolicitudReemplazoDetById(int id)
        {
            return _context.tblM_SolicitudReemplazoDet.FirstOrDefault(x => x.id == id);

        }

        
        public void SaveArchivo(HttpPostedFileBase archivo, string ruta)
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
        }

        public List<tblM_SolicitudReemplazoEquipo> GetSolicitudesPendientes(int tipo)
        {
            int estatus = 1;

            if (tipo == 1)
            {
                estatus = 0;
            }

            var c = (from srd in _context.tblM_SolicitudReemplazoEquipo
                     join sr in _context.tblM_SolicitudReemplazoDet
                     on srd.id equals sr.SolicitudEquipoReemplazoID
                     where sr.estatus == estatus
                     select srd).ToList();
            return c;
        }
    }
}
