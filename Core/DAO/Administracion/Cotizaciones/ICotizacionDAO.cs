using Core.DTO.Administracion.Cotizaciones;
using Core.Entity.Administrativo.cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.DAO.Administracion.Cotizaciones
{
    public interface ICotizacionDAO
    {
        List<CotizacionDTO> obtenerCotizacion(CotizacionDTO obj, DateTime fechaInicio, DateTime fechaFin);
        void guardarCotizacion(tblAD_Cotizaciones obj);
        void eliminarCotizacion(List<int> lista);
        List<tblAD_CotizacionComentariosDTO> guardarComentario(tblAD_CotizacionComentarios objS, HttpPostedFileBase file);
        List<tblAD_CotizacionComentariosDTO> obtenerComentarios(int id);
        tblAD_CotizacionComentarios getComentarioByID(int id);

        string getFolioCotizaciones(string CC);
    }
}
