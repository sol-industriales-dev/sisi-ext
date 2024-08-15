using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Proyecciones
{
    public class ComentariosObraDAO : GenericDAO<tblPro_ComentariosObras>, IComentariosObraDAO
    {
        public List<tblPro_ComentariosObras> GetListaComentarios(int capaturaObrasID, int registroID)
        {
            return _context.tblPro_ComentariosObras.Where(x => x.capturadeObrasID == capaturaObrasID && x.registroID == registroID).ToList();
        }

        public void GuardarComentario(tblPro_ComentariosObras obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.COMENTARIOPROYECCIONES);
            else
                Update(obj, obj.id, (int)BitacoraEnum.COMENTARIOPROYECCIONES);
        }
        public void GuardarComentarioArchivo(tblPro_ComentariosObras obj, HttpPostedFileBase file)
        {

            IObjectSet<tblPro_ComentariosObras> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblPro_ComentariosObras>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }

            _objectSet.AddObject(obj);
            _context.SaveChanges();
            if (file != null)
            {
                obj.adjuntoNombre = Path.GetFileName(file.FileName);
                obj.adjuntoExt = Path.GetExtension(file.FileName);
                obj.adjunto = GlobalUtils.ConvertFileToByte(file.InputStream);
                _context.SaveChanges();
            }

            SaveBitacora((int)BitacoraEnum.COMENTARIOPROYECCIONES, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }

        public tblPro_ComentariosObras GetComentarioById(int id)
        {
            return _context.tblPro_ComentariosObras.FirstOrDefault(x => x.id == id);
        }

    }
}
