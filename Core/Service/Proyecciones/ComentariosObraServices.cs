using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Proyecciones
{
    public class ComentariosObraServices : IComentariosObraDAO
    {
        #region Atributos
        private IComentariosObraDAO m_interfazDAO;
        #endregion

        #region Propiedades
        private IComentariosObraDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion

        #region Constructores
        public ComentariosObraServices(IComentariosObraDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion

        public List<tblPro_ComentariosObras> GetListaComentarios(int capaturaObrasID, int registroID)
        {
            return interfazDAO.GetListaComentarios(capaturaObrasID, registroID);
        }

        public void GuardarComentario(tblPro_ComentariosObras obj)
        {
            interfazDAO.GuardarComentario(obj);
        }

        public void GuardarComentarioArchivo(tblPro_ComentariosObras obj, HttpPostedFileBase file)
        {
            interfazDAO.GuardarComentarioArchivo(obj, file);
        }
        public tblPro_ComentariosObras GetComentarioById(int id)
        {
            return interfazDAO.GetComentarioById(id);
        }
    }
}
