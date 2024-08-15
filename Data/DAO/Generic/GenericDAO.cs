
using Core.DTO;
using Core.Entity.Principal.Bitacoras;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;

namespace Data.EntityFramework.Generic
{
    public abstract class GenericDAO<T> : IGenericDAO<T> where T : class
    {
        public MainContext _context = new MainContext();
        public IObjectSet<T> _objectSet;

        public readonly string SUCCESS = "success";
        public readonly string MESSAGE = "message";
        public const string PAGE = "page";
        public const string TOTAL_PAGE = "total";
        public const string ROWS = "rows";
        public const string ITEMS = "items";

        public GenericDAO()
        {
            setObjectContext();
        }

        private void setObjectContext()
        {
            _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<T>();
        }

        public void AddRows(List<T> list)
        {
            list.ForEach(x => _objectSet.AddObject(x));
            SaveChanges();
        }

        public void saveEntitys(List<T> list, int modulo)
        {
            foreach (var item in list)
            {
                _objectSet.AddObject(item);

                //SaveBitacora(modulo, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(item));
            }
            SaveChanges();
        }
        //Guardar nueva entidad
        public void SaveEntity(dynamic entity, int modulo)
        {
            if (entity == null) { throw new ArgumentNullException("Entity"); }
            _objectSet.AddObject(entity);
            SaveChanges();
            SaveBitacora(modulo, (int)AccionEnum.AGREGAR, (int)entity.id, JsonUtils.convertNetObjectToJson(entity));
        }
        public void SaveEntity(dynamic entity)
        {
            if (entity == null) { throw new ArgumentNullException("Entity"); }
            _objectSet.AddObject(entity);
            SaveChanges();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void AddRow(T entity)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges(SaveOptions options)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        //eliminar entidad
        public void Delete(dynamic entity, int modulo)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            SaveBitacora(modulo, (int)AccionEnum.ELIMINAR, (int)entity.id, JsonUtils.convertNetObjectToJson(entity));
        }

        public void DeleteById(long PK)
        {

        }

        public T Single(Func<T, bool> predicate)
        {
            return _objectSet.Single<T>(predicate);
        }
        //Actualizar entidad existente
        public T Update(dynamic entity, long PK, int modulo)
        {
            T existing = _context.Set<T>().Find(PK);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();

                try
                {


                    //  SaveBitacora(modulo, (int)AccionEnum.ACTUALIZAR, (int)entity.id, JsonUtils.convertNetObjectToJson(entity));
                }
                catch (Exception)
                {

                }

            }
            return existing;
        }
        public T Update(dynamic entity, long PK)
        {
            T existing = _context.Set<T>().Find(PK);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            return existing;
        }
        public T UpdateBit(dynamic entity, long PK, long oid, dynamic o, int modulo)
        {
            T existing = _context.Set<T>().Find(PK);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                _context.SaveChanges();
                SaveBitacora(modulo, (int)AccionEnum.ACTUALIZAR, (int)o.id, JsonUtils.convertNetObjectToJson(o));
            }
            return existing;
        }
        public void SaveChanges(dynamic entity, int modulo)
        {
            SaveChanges();
        }
        public void SaveBitacora(int modulo, int accion, int registroID, string objeto)
        {
            var bitacora = new tblP_Bitacora();

            bitacora.modulo = modulo;
            bitacora.accion = accion;
            bitacora.usuarioID = vSesiones.sesionUsuarioDTO.id;
            bitacora.fecha = DateTime.Now;
            bitacora.registroID = registroID;
            bitacora.objeto = objeto;
            bitacora.publicIP = "";
            bitacora.localIP = "";

            try
            {
                string publicIP = new WebClient().DownloadString("http://icanhazip.com");

                bitacora.publicIP = publicIP;
            }
            catch (Exception e)
            {
                LogError(0, 0, "Generic", "SaveBitacora (publicIP)", e, AccionEnum.CONSULTA, 0, new { bitacora = bitacora });
            }

            try
            {
                string localIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(localIP))
                {
                    localIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                bitacora.localIP = localIP;
            }
            catch (Exception e)
            {
                LogError(0, 0, "Generic", "SaveBitacora (localIP)", e, AccionEnum.CONSULTA, 0, new { bitacora = bitacora });
            }

            _context.tblP_Bitacora.Add(bitacora);
            _context.SaveChanges();
        }
        public void LogError(int sistema, int modulo, string controlador, string accion, Exception excepcion, AccionEnum tipo, long registroID, object objeto)
        {
            try
            {
                try
                {
                    string objetoJSON;
                    if (objeto != null)
                    {
                        try
                        {
                            objetoJSON = JsonUtils.convertNetObjectToJson(objeto);
                        }
                        catch (Exception)
                        {
                            objetoJSON = "No se pudo serializar el objeto.";
                        }
                    }
                    else
                    {
                        objetoJSON = "";
                    }

                    string mensajeError;
                    try
                    {
                        mensajeError = obtenerMensajeErrorRecursivo(excepcion);
                    }
                    catch (Exception)
                    {
                        mensajeError = excepcion.Message;
                    }

                    var error = new tblP_LogError
                    {
                        empresa = vSesiones.sesionEmpresaActual,
                        sistema = sistema,
                        modulo = modulo,
                        controlador = controlador,
                        accion = accion,
                        mensaje = mensajeError + " StackTrace: " + excepcion.StackTrace, //mensaje = excepcion.Message,
                        tipo = (int)tipo,
                        usuarioID = vSesiones.sesionUsuarioDTO.id,
                        fecha = DateTime.Now,
                        registroID = registroID,
                        objeto = objetoJSON
                    };
                    try
                    {
                        string publicIP = new WebClient().DownloadString("http://icanhazip.com");
                        error.publicIP = publicIP;
                    }
                    catch (Exception)
                    {
                        error.publicIP = "Error al tratar de obtener la IP pública";
                    }
                    try
                    {
                        string localIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(localIP))
                        {
                            localIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        }
                        error.localIP = localIP;
                    }
                    catch (Exception)
                    {
                        error.localIP = "Error al tratar de obtener la IP local";
                    }
                    _context.tblP_LogError.Add(error);
                    _context.SaveChanges();
                }
                // Si falla al intentar guardar el error, lo intenta guardar de nuevo con menos datos.
                catch (Exception)
                {
                    var error = new tblP_LogError
                    {
                        empresa = vSesiones.sesionEmpresaActual,
                        controlador = controlador,
                        accion = accion,
                        mensaje = excepcion.Message,
                        usuarioID = vSesiones.sesionUsuarioDTO.id,
                        fecha = DateTime.Now,
                    };
                    _context.tblP_LogError.Add(error);
                    _context.SaveChanges();
                }
            }
            catch (Exception) { }
        }

        private string obtenerMensajeErrorRecursivo(Exception e)
        {
            var mensajeError = e.Message;

            if (mensajeError.Contains("See the inner exception for details"))
            {
                mensajeError = obtenerMensajeErrorRecursivo(e.InnerException);
            }

            return mensajeError;
        }
    }
}
