using Core.DAO.Maquinaria.Overhaul;
using Core.DTO.Maquinaria.Overhaul;
using Core.Entity.Maquinaria.Overhaul;
using Core.Entity.Principal.Menus;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Archivos;
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
using Data.DAO.Principal.Usuarios;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Principal.Usuarios;
using Core.DTO;
using Core.Enum.Maquinaria.NewFolder1;
using System.Reflection;
using System.ComponentModel;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Data.EntityFramework.Context;
using Core.Enum.Multiempresa;
using Core.Entity.Maquinaria.Catalogo;




namespace Data.DAO.Maquinaria.Overhaul
{
    public class NotaCreditoDAO : GenericDAO<tblM_CapNotaCredito>, INotaCreditoDAO
    {

        #region variables y constructor
        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();

        private const string nombreControlador = "NotaCredito";

        /// <summary>
        /// Constructor
        /// </summary>
        public NotaCreditoDAO()
        {
            resultado.Clear();
        }
        #endregion
        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();

        #region Combos

        public dynamic getInsumo(string term, bool porDesc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"
                    SELECT TOP 12 insumo, descripcion FROM insumos
                    WHERE " + (porDesc ? @" descripcion " : @" insumo ") +
                    @" LIKE ? ORDER BY insumo";

                if(vSesiones.sesionEmpresaActual==3)
                {
                    odbc.consulta = @"
                    SELECT TOP 12 insumo, descripcion FROM DBA.insumos
                    WHERE " + (porDesc ? @" descripcion " : @" insumo ") +
                   @" LIKE ? ORDER BY insumo";
                }
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "label",
                    tipo = OdbcType.VarChar,
                    valor = "%" + (string)term.Trim() + "%"
                });

                List<dynamic> listaInsumos = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd, odbc);

                if(vSesiones.sesionEmpresaActual==3)
                {
                    listaInsumos = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, odbc);
                }

                if (porDesc)
                {
                    return listaInsumos.Select(x => new
                    {
                        id = ((decimal)x.insumo).ToString(),
                        value = (string)x.descripcion
                    }).ToList();
                }
                else
                {
                    return listaInsumos.Select(x => new
                    {
                        id = (string)x.descripcion,
                        value = ((decimal)x.insumo).ToString()
                    }).ToList();
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ComboDTO> getComboAlamcen()
        {
            string query = string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm 
                                    ORDER BY Text ASC");
            if(vSesiones.sesionEmpresaActual==3)
            {
                query = string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM DBA.si_almacen alm 
                                    ORDER BY Text ASC");
            }

            var comboDTO = (List<ComboDTO>)ContextArrendadora.Where(query).ToObject<List<ComboDTO>>();

            return comboDTO;
        }
        #endregion
        public void Guardar(tblM_CapNotaCredito obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.NOTACREDITO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.NOTACREDITO);
        }
        public void setFactura(int id, string oc, string factura)
        {
            var f = _context.tblM_CapNotaCredito.FirstOrDefault(x => x.id == id);
            f.Estado = 4;
            f.OC = oc;
            f.factura = factura;
            _context.SaveChanges();
        }
        public List<tblM_CapNotaCredito> getFillGridNotasCredito(FiltrosNotasCredito obj)
        {
            obj.FechaFin = obj.FechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            switch (obj.TipoFiltro)
            {
                case 1:
                    {
                        var resultado = (from nc in _context.tblM_CapNotaCredito
                                         where nc.Estado == obj.TipoFiltro && nc.TipoNC == (obj.FiltroTipoNC == 0 ? nc.TipoNC : obj.FiltroTipoNC) && (string.IsNullOrEmpty(obj.cc) ? true : nc.cc.Equals(obj.cc)) && (string.IsNullOrEmpty(obj.almacen) ? true : nc.noAlmacen.Equals(obj.almacen))
                                         select nc).ToList();

                        return resultado.Where(x => (x.Fecha.Date >= obj.FechaInicio && x.Fecha.Date <= obj.FechaFin)).ToList();
                    }
                case 2:
                    {
                        var res = _context.tblM_CapNotaCredito.Where(x => x.Estado == 2 && x.TipoNC == (obj.FiltroTipoNC == 0 ? x.TipoNC : obj.FiltroTipoNC) && (string.IsNullOrEmpty(obj.cc) ? true : x.cc.Equals(obj.cc)) && (string.IsNullOrEmpty(obj.almacen) ? true : x.noAlmacen.Equals(obj.almacen))).ToList();
                        return res.Where(x => (x.FechaCaptura.Date >= obj.FechaInicio && x.FechaCaptura.Date <= obj.FechaFin)).ToList();
                    }
                case 4:
                    {
                        var res = _context.tblM_CapNotaCredito.Where(x => x.Estado == 4 && x.TipoNC == (obj.FiltroTipoNC == 0 ? x.TipoNC : obj.FiltroTipoNC) && (string.IsNullOrEmpty(obj.cc) ? true : x.cc.Equals(obj.cc)) && (string.IsNullOrEmpty(obj.almacen) ? true : x.noAlmacen.Equals(obj.almacen))).ToList();
                        return res.Where(x => (x.FechaCaptura.Date >= obj.FechaInicio && x.FechaCaptura.Date <= obj.FechaFin)).ToList();
                    }
                case 3:
                    {
                        var res = _context.tblM_CapNotaCredito.Where(x => x.Estado == 3 && x.TipoNC == (obj.FiltroTipoNC == 0 ? x.TipoNC : obj.FiltroTipoNC) && (string.IsNullOrEmpty(obj.cc) ? true : x.cc.Equals(obj.cc)) && (string.IsNullOrEmpty(obj.almacen) ? true : x.noAlmacen.Equals(obj.almacen))).ToList();
                        return res.Where(x => (x.FechaCaptura.Date >= obj.FechaInicio && x.FechaCaptura.Date <= obj.FechaFin)).ToList();
                    }
                default:
                    return new List<tblM_CapNotaCredito>();
            }
        }

        public tblM_CapNotaCredito getNotaCreditoById(int obj)
        {
            var resultado = (from nc in _context.tblM_CapNotaCredito
                             where nc.id == obj
                             select nc).FirstOrDefault();
            return resultado;
        }
        public decimal TipoCambio(string fecha)
        {
            decimal valor = 0m;

            var listaTipoCambio = _contextEnkontrol.Select<decimal>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() {
                consulta = @"SELECT tipo_cambio FROM tipo_cambio WHERE fecha ='" + fecha + @"' ORDER BY fecha DESC"
            });
            if(vSesiones.sesionEmpresaActual==3)
            {
                listaTipoCambio = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ColombiaProductivo, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT tipo_cambio FROM DBA.tipo_cambio WHERE fecha ='" + fecha + @"' ORDER BY fecha DESC"
                });
            }
            if (listaTipoCambio.Count() > 0)
            {
                valor = listaTipoCambio[0];
            }
            else
            {
                var ultimoTipoCambio = _contextEnkontrol.Select<decimal>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT TOP 1 tipo_cambio FROM tipo_cambio ORDER BY fecha DESC"
                });
                if(vSesiones.sesionEmpresaActual==3)
                {
                    ultimoTipoCambio = _contextEnkontrol.Select<decimal>(EnkontrolEnum.ColombiaProductivo, new OdbcConsultaDTO()
                    {
                        consulta = @"SELECT TOP 1 tipo_cambio FROM DBA.tipo_cambio ORDER BY fecha DESC"
                    });
                }
                valor = ultimoTipoCambio[0];
            }

            return valor;
        }
        public void SaveOrUpdate(tblM_CapNotaCredito obj, HttpPostedFileBase file)
        {
            if (true)
            {
                if (obj.id == 0)
                {

                    if (file != null)
                    {

                        string extension = Path.GetExtension(file.FileName);

                        SaveEntity(obj, (int)BitacoraEnum.NOTACREDITO);

                        string FileName = obj.ClaveCredito + "." + extension;
                        string Ruta = archivofs.getArchivo().getUrlDelServidor(12) + FileName;
                        SaveArchivo(file, Ruta);

                        obj.RutaArchivo = Ruta;
                        Update(obj, obj.id, (int)BitacoraEnum.NOTACREDITO);
                    }
                    else
                    {
                        obj.RutaArchivo = "";
                        SaveEntity(obj, (int)BitacoraEnum.NOTACREDITO);
                    }
                }

                else
                    if (file != null)
                    {

                        string extension = Path.GetExtension(file.FileName);

                        Update(obj, obj.id, (int)BitacoraEnum.NOTACREDITO);

                        string FileName = obj.ClaveCredito + "." + extension;
                        string Ruta = archivofs.getArchivo().getUrlDelServidor(6) + FileName;
                        SaveArchivo(file, Ruta);

                        obj.RutaArchivo = Ruta;
                        Update(obj, obj.id, (int)BitacoraEnum.NOTACREDITO);
                    }
                    else
                    {
                        obj.RutaArchivo = "";
                        SaveEntity(obj, (int)BitacoraEnum.NOTACREDITO);
                    }

            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("Ya se capturo el registro.");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.NOTACREDITO);
            }
        }

        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
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
            return File.Exists(ruta);
        }
        public List<tblM_CapNotaCredito> GetNotasCreditoRpt(DateTime obj1, DateTime obj2, int TipoControl, int Estatus, string cc, string almacen)
        {
            obj2 = obj2.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<tblM_CapNotaCredito> resultado = new List<tblM_CapNotaCredito>();
            switch (Estatus)
            {
                case 1:
                    {
                        resultado = (from nc in _context.tblM_CapNotaCredito
                                     where nc.Estado == Estatus && nc.TipoNC == (TipoControl == 0 ? nc.TipoNC : TipoControl) && (string.IsNullOrEmpty(cc) ? true : nc.cc.Equals(cc)) && (string.IsNullOrEmpty(almacen) ? true : nc.noAlmacen.Equals(almacen))
                                     select nc).ToList();

                        return resultado.Where(x => (x.Fecha.Date >= obj1 && x.Fecha.Date <= obj2)).ToList();
                    }
                case 2:
                    {
                        resultado = _context.tblM_CapNotaCredito.Where(x => x.Estado == 2 && x.TipoNC == (TipoControl == 0 ? x.TipoNC : TipoControl) && (string.IsNullOrEmpty(cc) ? true : x.cc.Equals(cc)) && (string.IsNullOrEmpty(almacen) ? true : x.noAlmacen.Equals(almacen))).ToList();
                        return resultado.Where(x => (x.FechaCaptura.Date >= obj1 && x.FechaCaptura.Date <= obj2)).ToList();
                    }
                case 4:
                    {
                        resultado = _context.tblM_CapNotaCredito.Where(x => x.Estado == 4 && x.TipoNC == (TipoControl == 0 ? x.TipoNC : TipoControl) && (string.IsNullOrEmpty(cc) ? true : x.cc.Equals(cc)) && (string.IsNullOrEmpty(almacen) ? true : x.noAlmacen.Equals(almacen))).ToList();
                        return resultado.Where(x => (x.FechaCaptura.Date >= obj1 && x.FechaCaptura.Date <= obj2)).ToList();
                    }
                case 3:
                    {
                        resultado = _context.tblM_CapNotaCredito.Where(x => x.Estado == 3 && x.TipoNC == (TipoControl == 0 ? x.TipoNC : TipoControl) && (string.IsNullOrEmpty(cc) ? true : x.cc.Equals(cc)) && (string.IsNullOrEmpty(almacen) ? true : x.noAlmacen.Equals(almacen))).ToList();
                        return resultado.Where(x => (x.FechaCaptura.Date >= obj1 && x.FechaCaptura.Date <= obj2)).ToList();
                    }
                default:
                    return new List<tblM_CapNotaCredito>();
            }
        }
        public List<tblP_Usuario> obtenerListasuario()
        {
            return _context.tblP_Usuario.Where(r => r.estatus).ToList();
        }
        public List<tblM_Almacen> ObtenerAlmacenes()
        {
            List<tblM_Almacen> lstAlaamcenes = new List<tblM_Almacen>();


            var getCatEmpleado = @"SELECT almacen,descripcion  FROM si_almacen";
            if(vSesiones.sesionEmpresaActual==3)
            {
                getCatEmpleado = @"SELECT almacen,descripcion  FROM DBA.si_almacen";
            }
            try
            {
                var odbc = new OdbcConsultaDTO() { consulta = getCatEmpleado };
                lstAlaamcenes = _contextEnkontrol.Select<tblM_Almacen>(EnkontrolEnum.ArrenProd, odbc).ToList();
                if(vSesiones.sesionEmpresaActual==3)
                {
                    lstAlaamcenes = _contextEnkontrol.Select<tblM_Almacen>(EnkontrolEnum.ColombiaProductivo, odbc).ToList();
                }
            }
            catch(Exception ex)
            {
                return lstAlaamcenes;
            }

            return lstAlaamcenes;
        }


        public tblM_ComentariosNotaCreditoDTO guardarComentario(tblM_ComentariosNotaCredito obj)
        {
            IObjectSet<tblM_ComentariosNotaCredito> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_ComentariosNotaCredito>();
            if (obj == null) { throw new ArgumentNullException("Entity"); }

            _objectSet.AddObject(obj);
            _context.SaveChanges();
            SaveBitacora((int)BitacoraEnum.COMENTARIO, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
            var result = new tblM_ComentariosNotaCreditoDTO();
            result.id = obj.id;
            result.notaCreditoID = obj.notaCreditoID;
            result.comentario = obj.comentario;
            result.usuarioNombre = obj.usuarioNombre;
            result.usuarioID = obj.usuarioID;
            result.fecha = obj.fecha.ToShortDateString();
            result.factura = obj.factura;

            if (!string.IsNullOrEmpty(obj.factura) && !string.IsNullOrWhiteSpace(obj.factura))
            {
                var a = _context.tblM_CapNotaCredito.FirstOrDefault(x => x.id == obj.notaCreditoID);
                a.Estado = 4;
                a.factura = obj.factura;
                _context.SaveChanges();
            }

            return result;
        }

        public List<tblM_ComentariosNotaCreditoDTO> getComentarios(int id, int tipoComentarios)
        {
            var result = new List<tblM_ComentariosNotaCreditoDTO>();

            if (tipoComentarios == 0)
            {
                var comentario = _context.tblM_ComentariosNotaCredito.FirstOrDefault(f => f.notaCreditoID == id && f.tipoComentario == 0);
                var nota = _context.tblM_CapNotaCredito.FirstOrDefault(f => f.id == id && f.Estado == 3);

                if (nota != null)
                {
                    tblM_ComentariosNotaCreditoDTO obj = new tblM_ComentariosNotaCreditoDTO();

                    obj.comentario = comentario.comentario;
                    obj.factura = comentario.factura;
                    obj.fecha = comentario.fecha.ToShortDateString();
                    obj.id = comentario.id;
                    obj.notaCreditoID = comentario.notaCreditoID;
                    obj.usuarioID = comentario.usuarioID;
                    obj.usuarioNombre = comentario.usuarioNombre;
                    obj.tieneEvidencia = !string.IsNullOrEmpty(comentario.nombreEvidencia);
                    obj.nombreArchivo = comentario.nombreEvidencia;

                    result.Add(obj);
                }
            }
            else
            {
                var listaComentarios = _context.tblM_ComentariosNotaCredito.Where(x => x.notaCreditoID.Equals(id) && x.tipoComentario == tipoComentarios);
                foreach (var item in listaComentarios)
                {
                    tblM_ComentariosNotaCreditoDTO obj = new tblM_ComentariosNotaCreditoDTO();

                    obj.comentario = item.comentario;
                    obj.factura = item.factura;
                    obj.fecha = item.fecha.ToShortDateString();
                    obj.id = item.id;
                    obj.notaCreditoID = item.notaCreditoID;
                    obj.usuarioID = item.usuarioID;
                    obj.usuarioNombre = item.usuarioNombre;

                    result.Add(obj);

                }
            }

            return result;
        }

        public string GetComentario(int id)
        {
            var Comentario = _context.tblM_ComentariosNotaCredito.Where(x => x.notaCreditoID == id).FirstOrDefault();

            if (Comentario != null)
            {
                return Comentario.comentario;
            }
            else
            {
                return "";
            }
        }

        //Obtiene los tipos de nota de credito de TipoNCEnum
        public Dictionary<int, string> getTiposNotaCredito()
        {
            var dict = new Dictionary<int, string>();
            foreach (var name in Enum.GetNames(typeof(TipoNCEnum)))
            {
                //dict.Add((int)Enum.Parse(typeof(TipoNCEnum), name), GetDescription((Enum)Enum.Parse(typeof(TipoNCEnum),name)));
                dict.Add((int)Enum.Parse(typeof(TipoNCEnum), name), Infrastructure.Utils.EnumExtensions.GetDescription((Enum)Enum.Parse(typeof(TipoNCEnum), name)));
            }

            return dict;
        }

        //public static string GetDescription(System.Enum value)
        //{
        //    var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
        //    var descriptionAttribute =
        //        enumMember == null
        //            ? default(DescriptionAttribute)
        //            : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
        //    return
        //        descriptionAttribute == null
        //            ? value.ToString()
        //            : descriptionAttribute.Description;
        //}

    }
}
