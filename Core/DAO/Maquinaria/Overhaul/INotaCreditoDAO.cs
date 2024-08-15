using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Enum.Maquinaria.NewFolder1;
using Core.DTO.Principal.Generales;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Maquinaria.Catalogo;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface INotaCreditoDAO
    {
        void setFactura(int id, string oc, string factura);
        void Guardar(tblM_CapNotaCredito obj);
        List<tblM_CapNotaCredito> getFillGridNotasCredito(FiltrosNotasCredito obj);
        decimal TipoCambio(string fecha);
        tblM_CapNotaCredito getNotaCreditoById(int id);

        void SaveOrUpdate(tblM_CapNotaCredito obj, HttpPostedFileBase file);

        List<tblM_CapNotaCredito> GetNotasCreditoRpt(DateTime obj1, DateTime obj2, int TipoControl, int Estatus, string cc, string almacen);
        List<tblP_Usuario> obtenerListasuario();
        List<tblM_Almacen> ObtenerAlmacenes();
        tblM_ComentariosNotaCreditoDTO guardarComentario(tblM_ComentariosNotaCredito obj);

        List<tblM_ComentariosNotaCreditoDTO> getComentarios(int obj, int tipoComentarios);

        bool SaveArchivo(HttpPostedFileBase archivo, string ruta);
        string GetComentario(int id);

        //Tipo de nota de credito
        Dictionary<int, string> getTiposNotaCredito();
        dynamic getInsumo(string term, bool porDesc);
        List<ComboDTO> getComboAlamcen();
    }
}
