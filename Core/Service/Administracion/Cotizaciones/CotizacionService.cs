using Core.DAO.Administracion.Cotizaciones;
using Core.DTO.Administracion.Cotizaciones;
using Core.Entity.Administrativo.cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Service.Administracion.Cotizaciones
{
    public class CotizacionService : ICotizacionDAO
    {
        #region Atributos
        private ICotizacionDAO m_interfazDAO;
        #endregion Atributos

        #region Propiedades
        private ICotizacionDAO interfazDAO
        {
            get { return m_interfazDAO; }
            set { m_interfazDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CotizacionService(ICotizacionDAO InterfazDAO)
        {
            this.interfazDAO = InterfazDAO;
        }
        #endregion Constructores
        public List<CotizacionDTO> obtenerCotizacion(CotizacionDTO obj, DateTime fechaInicio, DateTime fechaFin)
        {
            return interfazDAO.obtenerCotizacion(obj, fechaInicio, fechaFin);
        }
        public void guardarCotizacion(tblAD_Cotizaciones obj)
        {
            interfazDAO.guardarCotizacion(obj);
        }
        public void eliminarCotizacion(List<int> lista)
        {
            interfazDAO.eliminarCotizacion(lista);
        }
        public List<tblAD_CotizacionComentariosDTO> guardarComentario(tblAD_CotizacionComentarios objS, HttpPostedFileBase file)
        {
            return interfazDAO.guardarComentario(objS, file);
        }
        public List<tblAD_CotizacionComentariosDTO> obtenerComentarios(int id)
        {
            return interfazDAO.obtenerComentarios(id);
        }
        public tblAD_CotizacionComentarios getComentarioByID(int id)
        {
            return interfazDAO.getComentarioByID(id);
        }

        public string getFolioCotizaciones(string CC)
        {
            return interfazDAO.getFolioCotizaciones(CC);
        }
    }
}
