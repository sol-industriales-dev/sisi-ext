using Core.DAO.ControlObra.AdministradorProyectos;
using Core.DTO;
using Core.Entity.AdministradorProyectos.CGP;
using Data.DAO.Principal.Archivos;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.ControlObra.AdministradorProyectos
{
    public class CGPDAO : GenericDAO<tblAP_CGP_MenuArchivos>, ICGPDAO
    {
        public string RutaArchivoDeLaVistaId(int vistaID)
        {
            try
            {
#if DEBUG
                return "C:\\Proyectos\\Presentación1.pdf";
#else
                var rutaBase = new DirArchivosDAO().getUrlDelServidor(1021);
                var dirArchivo = (from ruta in _context.tblAP_CGP_MenuArchivos
                                   where ruta.esActivo && ruta.IdMenu == vistaID
                                   select ruta).FirstOrDefault().DirArchivos;
                return rutaBase + dirArchivo;
#endif
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
    }
}
