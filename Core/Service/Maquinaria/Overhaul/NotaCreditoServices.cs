using Core.DAO.Maquinaria.Overhaul;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Maquinaria.NewFolder1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Maquinaria.Overhaul
{
    public class NotaCreditoServices : INotaCreditoDAO
    {
        #region Atributos
        private INotaCreditoDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private INotaCreditoDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public NotaCreditoServices(INotaCreditoDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public void setFactura(int id, string oc, string factura)
        {
            interfazDAO.setFactura(id, oc, factura);
        }
        public void Guardar(tblM_CapNotaCredito obj)
        {
            interfazDAO.Guardar(obj);
        }
        public List<tblM_CapNotaCredito> getFillGridNotasCredito(FiltrosNotasCredito obj)
        {
            return interfazDAO.getFillGridNotasCredito(obj);
        }
        public decimal TipoCambio(string fecha)
        {
            return interfazDAO.TipoCambio(fecha);
        }
        public tblM_CapNotaCredito getNotaCreditoById(int obj)
        {
            return interfazDAO.getNotaCreditoById(obj);
        }

        public void SaveOrUpdate(tblM_CapNotaCredito obj, HttpPostedFileBase file)
        {
            interfazDAO.SaveOrUpdate(obj, file);
        }

        public List<tblM_CapNotaCredito> GetNotasCreditoRpt(DateTime obj1, DateTime obj2, int TipoControl, int Estatus, string cc,string almacen)
        {
            return interfazDAO.GetNotasCreditoRpt(obj1, obj2, TipoControl, Estatus, cc,almacen);
        }
        public List<tblP_Usuario> obtenerListasuario()
        {
            return interfazDAO.obtenerListasuario();
        }

        public tblM_ComentariosNotaCreditoDTO guardarComentario(tblM_ComentariosNotaCredito obj)
        {
            return interfazDAO.guardarComentario(obj);
        }

        public List<tblM_ComentariosNotaCreditoDTO> getComentarios(int obj, int tipoComentario)
        {
            return interfazDAO.getComentarios(obj, tipoComentario);
        }

        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            return interfazDAO.SaveArchivo(archivo, ruta);
        }

        public string GetComentario(int id)
        {
            return interfazDAO.GetComentario(id);
        }

        public Dictionary<int, string> getTiposNotaCredito()
        {
            return interfazDAO.getTiposNotaCredito();
        }
        public dynamic getInsumo(string term, bool porDesc)
        {
            return interfazDAO.getInsumo(term, porDesc);
        }

        public List<ComboDTO> getComboAlamcen()
        {
            return interfazDAO.getComboAlamcen();
        }
        public List<tblM_Almacen> ObtenerAlmacenes()
        {
            return interfazDAO.ObtenerAlmacenes();
        }
    }
}
