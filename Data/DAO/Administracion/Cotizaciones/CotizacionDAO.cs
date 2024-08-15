using Core.DAO.Administracion.Cotizaciones;
using Core.DTO;
using Core.DTO.Administracion.Cotizaciones;
using Core.Entity.Administrativo.cotizaciones;
using Core.Enum.Administracion.Cotizaciones;
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

namespace Data.DAO.Administracion.Cotizaciones
{
    public class CotizacionDAO : GenericDAO<tblAD_Cotizaciones>, ICotizacionDAO
    {
        public List<CotizacionDTO> obtenerCotizacion(CotizacionDTO obj, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new List<CotizacionDTO>();

            var data = _context.tblAD_Cotizaciones
                      .Where(x =>
                          x.activo == true &&
                          x.cc.Contains(obj.cc) &&
                          x.folio.Contains(obj.folio) &&
                          x.cliente.Contains(obj.cliente) &&
                          x.proyecto.Contains(obj.proyecto) &&
                          (obj.vEstatus == 0 ? true : x.estatus == obj.vEstatus)

                          ).ToList().Where(y => (y.fechaStatus.Date >= fechaInicio.Date && y.fechaStatus.Date <= fechaFin.Date));


            foreach (var i in data)
            {
                var o = new CotizacionDTO();
                o.id = i.id;
                o.folio = i.folio;
                o.cc = i.cc;
                o.cliente = i.cliente;
                o.proyecto = i.proyecto;
                o.monto = i.monto.ToString("C2");
                o.vMonto = i.monto;
                o.estatus = EnumExtensions.GetDescription((CotizacionEnum)i.estatus);
                o.vEstatus = i.estatus;
                o.fechaCaptura = i.fechaCaptura.ToShortDateString();
                o.noRevision = i.revision;
                o.tipoMoneda = i.tipoMoneda;
                o.Margen = i.Margen.ToString();
                o.fechaStatus = i.fechaStatus.ToShortDateString();
                o.fechaProbableF = i.fechaProbableF.ToShortDateString();
                o.contacto = i.contacto;
                o.comentario = getConcatComentario(i.id);
                o.nombreMoneda = i.tipoMoneda == 1 ? "PESOS" : "USD";
                result.Add(o);
            }

            var usuarioID = vSesiones.sesionUsuarioDTO.id;
            var ListaCC = _context.tblP_CC_Usuario.Where(x => x.usuarioID == usuarioID).Select(c => c.cc).ToList();

            vSesiones.ReporteCotizacionDTO = result.Where(c => ListaCC.Contains(c.cc)).ToList();
            return result;
        }

        private string getConcatComentario(int p)
        {

            string caderaResult = "";
            var data = obtenerComentarios(p);

            foreach (var item in data)
            {
                caderaResult += item.comentario + " || ";
            }

            return caderaResult;

        }

        public void guardarCotizacion(tblAD_Cotizaciones obj)
        {
            if (obj.id == 0)
            {
                obj.fechaCaptura = DateTime.Now;
                obj.year = DateTime.Now.Year;
                obj.fechaStatus = obj.fechaStatus;
                _context.tblAD_Cotizaciones.Add(obj);
                _context.SaveChanges();
            }
            else
            {
                var temp = _context.tblAD_Cotizaciones.FirstOrDefault(x => x.id == obj.id);
                temp.folio = obj.folio;
                temp.cc = obj.cc;
                temp.cliente = obj.cliente;
                temp.proyecto = obj.proyecto;
                temp.monto = obj.monto;
                temp.estatus = obj.estatus;
                temp.year = temp.year;
                temp.fechaCaptura = DateTime.Now;
                temp.fechaStatus = obj.fechaStatus;
                temp.fechaCaptura = temp.fechaCaptura;
                temp.revision = obj.revision;
                temp.Margen = obj.Margen;
                temp.tipoMoneda = obj.tipoMoneda;
                temp.contacto = obj.contacto;
                temp.fechaProbableF = obj.fechaProbableF;
                _context.SaveChanges();
            }
        }
        public void eliminarCotizacion(List<int> lista)
        {
            var data = _context.tblAD_Cotizaciones.Where(x => lista.Contains(x.id)).ToList();
            data.ForEach(x => x.activo = false);
            _context.SaveChanges();
        }

        public List<tblAD_CotizacionComentariosDTO> guardarComentario(tblAD_CotizacionComentarios obj, HttpPostedFileBase file)
        {
            IObjectSet<tblAD_CotizacionComentarios> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblAD_CotizacionComentarios>();
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

            var temp = _context.tblAD_Cotizaciones.Find(obj.cotizacionID);
            temp.comentariosCount = temp.comentarios.Count;
            _context.SaveChanges();
            var result = obtenerComentarios(obj.cotizacionID);
            return result;
        }

        public List<tblAD_CotizacionComentariosDTO> obtenerComentarios(int id)
        {
            var data = _context.tblAD_Cotizaciones.FirstOrDefault(x => x.id == id).comentarios;
            var result = new List<tblAD_CotizacionComentariosDTO>();
            foreach (var j in data.OrderBy(x => x.fecha))
            {
                var cObj = new tblAD_CotizacionComentariosDTO();
                cObj.id = j.id;
                cObj.cotizacionID = j.cotizacionID;
                cObj.comentario = j.comentario;
                cObj.usuarioNombre = j.usuarioNombre;
                cObj.usuarioID = j.usuarioID;
                cObj.fecha = j.fecha.ToShortDateString();
                cObj.tipo = j.tipo;
                cObj.adjuntoNombre = j.adjuntoNombre;
                result.Add(cObj);

            }

            return result;
        }
        public tblAD_CotizacionComentarios getComentarioByID(int id)
        {
            return _context.tblAD_CotizacionComentarios.FirstOrDefault(x => x.id == id);
        }
        public string getFolioCotizaciones(string CC)
        {
            var Folio = "";
            var currentYear = DateTime.Now.Year;
            var Data = _context.tblAD_Cotizaciones.Where(x => x.cc == CC && x.year == currentYear).ToList();
            int noFolio = 0;
            if (Data != null)
            {
                noFolio = Data.Count();
            }
            DateTime currentTime = DateTime.Now;
            Folio = CC + "-" + (noFolio + 1) + "-" + currentTime.ToString("yy");

            return Folio;
        }

    }
}
