using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Proyecciones
{
    public interface IComentariosObraDAO
    {
        List<tblPro_ComentariosObras> GetListaComentarios(int capaturaObrasID, int registroID);
        void GuardarComentarioArchivo(tblPro_ComentariosObras obj, HttpPostedFileBase file);
        void GuardarComentario(tblPro_ComentariosObras obj);

        tblPro_ComentariosObras GetComentarioById(int id);
    }
}
