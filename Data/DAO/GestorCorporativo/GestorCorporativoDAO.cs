using Core.DAO.GestorCorporativo;
using Core.DTO;
using Core.DTO.GestorCorporativo;
using Core.Entity.GestorCorporativo;
using Core.Enum.GestorCorporativo;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Data.DAO.GestorCorporativo
{
    public class GestorCorporativoDAO : GenericDAO<tblGC_Archivo>, IGestorCorporativoDAO
    {
        #region Variables
        private const string nombreControlador = "GestorCorporativoController";

        private readonly Dictionary<string, object> resultado = new Dictionary<string, object>();


        private const string FORMATO_FECHA_HORA = "dd/MM/yyyy hh:mm tt";
        private const string FORMATO_FECHA_SESION = "dd MMMM yyyy hh-mm tt";
        private const string TIPO_DEFAULT_CARPETA = "folder";
        private const string PERMISO_ADMIN_GESTOR = "Administrar";

        private readonly string RUTA_BASE = @"\\REPOSITORIO\Proyecto\SIGOPLAN\GESTOR_CORPORATIVO";
        private readonly string RUTA_ELIMINADOS;
        private readonly string RUTA_TEMPORALES;

        // Variante para hacer pruebas en local.
        private readonly string RUTA_LOCAL = @"C:\Proyecto\SIGOPLAN\GESTOR_CORPORATIVO";

        /// <summary>
        /// Indica si el usuario actual no tiene privilegios de adminsitrador.
        /// </summary>
        private bool noEsAdmin = true;
        #endregion

        #region Constructor
        public GestorCorporativoDAO()
        {
            resultado.Clear();
            // Producción.
            RUTA_BASE += vSesiones.sesionEmpresaActual == 1 ? @"\SIGOPLAN" : @"\ARRENDADORA";

            // Local
            //RUTA_BASE = RUTA_LOCAL + (vSesiones.sesionEmpresaActual == 1 ? @"\SIGOPLAN" : @"\ARRENDADORA");

            RUTA_ELIMINADOS = RUTA_BASE + @"\Eliminados";
            RUTA_TEMPORALES = RUTA_BASE + @"\Temporales";

            noEsAdmin = !(new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, PERMISO_ADMIN_GESTOR));
        }
        #endregion

        /// <summary>
        /// El metodo InicializarPermisosUsuarios solo debe ejecutarse una vez si se agrega algun usuario nuevo a un permiso como los que esta en la region de abajo 
        /// llamada "ID usuarios permiso" con esto se creara la estructura completa de sus permisos, solo de debe agregar manualmente la carpeta raiz de nivel 0,1,2
        /// </summary>
        public Dictionary<string, object> VerificarAccesoGestor()
        {
            try
            {

                //InicializarPermisosUsuarios();
                if (!noEsAdmin)
                {
                    resultado.Add("acceso", true);
                }
                else
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;
                    var tienePermiso = _context.tblGC_Permiso.Any(x => (x.usuarioID == vSesiones.sesionUsuarioDTO.id));
                    resultado.Add("acceso", tienePermiso);
                }
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "VerificarAccesoGestor", e, AccionEnum.CONSULTA, vSesiones.sesionUsuarioDTO.id, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al intentar verificar si el usuario tenía acceso al gestor.");
            }
            return resultado;
        }

        public DirectorioDTO ObtenerEstructuraDirectorios()
        {
            var contenedor = new DirectorioDTO();
            try
            {
                contenedor.data = new List<DirectorioDTO>();

                _context.Configuration.AutoDetectChangesEnabled = false;

                int index = 1;

                var listaArchivos = _context.tblGC_Archivo.Where(archivo => archivo.activo)
                    .ToList();

                var listaCarpetasRaiz = listaArchivos.Where(archivo => archivo.nivel == 0 && archivo.activo)
                    .OrderBy(x => x.esCarpeta).ThenBy(x => x.orden).ThenBy(x => x.nombre)
                    .ToList();

                var listaPermisosUsuario = _context.tblGC_Permiso
                    .Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).ToList();

                foreach (var carpetaRaiz in listaCarpetasRaiz)
                {
                    // Si no es admin, verifica si tiene acceso a la carpeta raíz.
                    if (noEsAdmin)
                    {
                        var permisoCarpetaRaiz = listaPermisosUsuario
                            .FirstOrDefault(permiso => permiso.archivoID == carpetaRaiz.id);

                        // Si no tiene permiso a esa carpeta, salta la iteración.
                        if (permisoCarpetaRaiz == null)
                        {
                            continue;
                        }
                    }

                    var directorioTemp = new DirectorioDTO
                    {
                        index = index++,
                        value = carpetaRaiz.nombre,
                        type = TIPO_DEFAULT_CARPETA,
                        date = carpetaRaiz.fechaCreacion.ToString(FORMATO_FECHA_HORA),
                        id = carpetaRaiz.id,
                        pId = 0,
                        level = carpetaRaiz.nivel,
                        open = true,
                        data = new List<DirectorioDTO>(),
                        permisos = ObtenerPermisosArchivo(carpetaRaiz.id, ref listaPermisosUsuario),
                        grupoCarpeta = carpetaRaiz.grupoCarpeta,
                        subGrupoCarpeta = carpetaRaiz.subGrupoCarpeta,
                    };

                    directorioTemp.data = ObtenerEstructuraSubcarpetas(directorioTemp, ref index, ref listaArchivos, ref listaPermisosUsuario);

                    contenedor.data.Add(directorioTemp);
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "ObtenerEstructuraDirectorios", e, AccionEnum.CONSULTA, 0, null);
                return new DirectorioDTO();
            }

            return contenedor;
        }

        public Dictionary<string, object> SubirArchivo(HttpPostedFileBase archivo, long padreID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var carpetaPadre = _context.tblGC_Archivo.FirstOrDefault(archivoPadre => archivoPadre.id == padreID && archivoPadre.activo);

                    // Se valida que el archivo no venga vacío.
                    if (archivo.ContentLength == 0)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El archivo que intentó subir está vacío.");
                        return resultado;
                    }

                    // Verifica que el usuario tenga permiso de subir archivos en la carpeta padre.
                    if (noEsAdmin)
                    {
                        var permisoSubir = _context.tblGC_Permiso
                        .FirstOrDefault(x =>
                            (x.archivoID == padreID &&
                            x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                            x.puedeSubir));
                        if (permisoSubir == null)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                            return resultado;
                        }
                    }

                    // Se valida que no exista otro archivo con el mismo nombre en la misma carpeta (En caso de ser nombre libre)
                    bool mismoNombre = _context.tblGC_Archivo
                        .Any(x => x.padreID == padreID && x.activo && x.nombre == archivo.FileName.Trim());

                    if (mismoNombre)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ya existe un archivo con ese nombre.");
                        return resultado;
                    }

                    // Se crea el nuevo archivo
                    var nuevoArchivo = new tblGC_Archivo
                    {
                        nombre = archivo.FileName,
                        ruta = carpetaPadre.ruta == null ? Path.Combine(carpetaPadre.nombre) : Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre),
                        nivel = carpetaPadre.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        orden = ObtenerOrdenArchivo(padreID),
                        padreID = padreID,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPadre.grupoCarpeta,
                        subGrupoCarpeta = carpetaPadre.subGrupoCarpeta,
                    };
                    _context.tblGC_Archivo.Add(nuevoArchivo);
                    _context.SaveChanges();


                    // Crea el registro de permisos del archivo.
                    _context.tblGC_Permiso.Add(new tblGC_Permiso
                    {
                        usuarioID = vSesiones.sesionUsuarioDTO.id,
                        archivoID = nuevoArchivo.id,
                        puedeEliminar = true,
                        puedeDescargarArchivo = true
                    });
                    _context.SaveChanges();


                    // Se crea el registro del permiso y se actualiza el estatus de las vistas para los demás usuarios.
                    CrearNuevosPermisosUsuarios(nuevoArchivo);

                    var rutaFisica = Path.Combine(RUTA_BASE, nuevoArchivo.ruta, nuevoArchivo.nombre);

                    if (rutaFisica.Length >= 260)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se puede guardar el archivo en el servidor. La ruta física del archivo es demasiado larga.");
                    }

                    // Si falla al guardar el archivo
                    if (!GlobalUtils.SaveCompressedFile(archivo, rutaFisica))
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                    }

                    // Si creó el archivo.
                    else
                    {
                        var nuevoArchivoDTO = new DirectorioDTO
                        {
                            id = nuevoArchivo.id,
                            value = nuevoArchivo.nombre,
                            type = Path.GetExtension(nuevoArchivo.nombre).Substring(1),
                            date = nuevoArchivo.fechaCreacion.ToString(FORMATO_FECHA_HORA),
                            pId = padreID,
                            grupoCarpeta = nuevoArchivo.grupoCarpeta,
                            subGrupoCarpeta = nuevoArchivo.subGrupoCarpeta,
                            permisos = new PermisosDTO()
                            {
                                puedeEliminar = true,
                                puedeDescargarArchivo = true
                            }
                        };

                        resultado.Add(SUCCESS, true);
                        resultado.Add("archivo", nuevoArchivoDTO);
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "SubirArchivo", e, AccionEnum.AGREGAR, padreID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar el archivo en el servidor.");
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarArchivo(long archivoID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var archivo = _context.tblGC_Archivo.Where(x => x.id == archivoID && x.activo).FirstOrDefault();

                    if (archivo == null)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró al archivo que se intentó eliminar.");
                        return resultado;
                    }

                    // Verifica que el usuario tenga permiso de eliminar el archivo.
                    if (noEsAdmin)
                    {
                        var permisoEliminar = _context.tblGC_Permiso
                            .FirstOrDefault(x =>
                                (x.archivoID == archivoID &&
                                x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                                x.puedeEliminar));
                        if (permisoEliminar == null)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                            return resultado;
                        }
                    }

                    long padreID = archivo.padreID;
                    bool esArchivo = !archivo.esCarpeta;

                    // Se actualiza la fecha edición de la carpeta padre.
                    var carpetaPadre = _context.tblGC_Archivo.FirstOrDefault(x => x.id == padreID && x.activo);

                    archivo.padreID = 0;
                    archivo.activo = false;
                    _context.SaveChanges();

                    var listaCorreosUsuarios = new List<string>();

                    // Se eliminan los registros de ese permiso.
                    EliminarPermisosUsuarios(archivoID, padreID);

                    if (archivo.esCarpeta)
                    {
                        // Verifica si la carpeta no tiene archivos activos.
                        var archivosHijosActivos = _context.tblGC_Archivo.Where(x => x.padreID == archivo.id && x.activo).ToList();

                        if (archivosHijosActivos != null && archivosHijosActivos.Count > 0)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Esta carpeta aún contiene archivos activos y por lo tanto no se puede eliminar.");
                            return resultado;
                        }

                        var rutaFisicaCarpeta = Path.Combine(RUTA_BASE, archivo.ruta, archivo.nombre);
                        if (Directory.Exists(rutaFisicaCarpeta))
                        {
                            if (Directory.EnumerateFileSystemEntries(rutaFisicaCarpeta).Any())
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "La carpeta aún tiene archivos.");
                                return resultado;
                            }

                            resultado.Add(SUCCESS, true);
                            Directory.Delete(rutaFisicaCarpeta, true);
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "La carpeta no existe en el servidor.");
                            return resultado;
                        }
                    }
                    else
                    {
                        // Se obtiene la lista de archivos (diferentes versiones)
                        var archivoPorEliminar = _context.tblGC_Archivo.FirstOrDefault(x => x.id == archivoID);

                        string nombreArchivoServidor = archivoPorEliminar.nombre.Replace(Path.GetExtension(archivoPorEliminar.nombre), ".zip");
                        var rutaFisicaArchivo = Path.Combine(RUTA_BASE, archivoPorEliminar.ruta, nombreArchivoServidor);

                        if (!File.Exists(rutaFisicaArchivo))
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El archivo no existe en el servidor.");
                            return resultado;
                        }
                        else
                        {
                            resultado.Add(SUCCESS, true);
                            File.Move(rutaFisicaArchivo, Path.Combine(RUTA_ELIMINADOS, (DateTime.Now.ToString("[dd_MM_HH_mm_ss] ") + nombreArchivoServidor)));
                        }
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "EliminarArchivo", e, AccionEnum.ELIMINAR, archivoID, null);
                    resultado.Clear();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar eliminar el archivo en el servidor.");
                }

                return resultado;
            }
        }

        public Dictionary<string, object> CrearCarpeta(string nombreCarpeta, long padreID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Verifica que el usuario tenga permiso de crear carpetas.
                    if (noEsAdmin)
                    {
                        var permisoCrearCarpeta = _context.tblGC_Permiso
                            .FirstOrDefault(x =>
                                (x.archivoID == padreID &&
                                x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                                x.puedeCrear));
                        if (permisoCrearCarpeta == null)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                            return resultado;
                        }
                    }

                    // Se valida el nombre de la carpeta
                    if (nombreCarpeta == null || nombreCarpeta.Trim().Length < 2 || EsNombreCarpetaInvalido(nombreCarpeta))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El nombre de la carpeta es inválido.");
                        return resultado;
                    }

                    nombreCarpeta = nombreCarpeta.Trim();

                    // Se verifica que no exista alguna carpeta con ese nombre en la ubicación indicada.
                    var nombreExistente = _context.tblGC_Archivo.Where(x => x.padreID == padreID && x.activo).Any(x => x.nombre == nombreCarpeta);
                    if (nombreExistente)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ya existe una carpeta con ese nombre.");
                        return resultado;
                    }

                    tblGC_Archivo carpetaPadre = _context.tblGC_Archivo.FirstOrDefault(x => x.id == padreID && x.activo);

                    // Se crea el nuevo archivo
                    var nuevaCarpeta = new tblGC_Archivo
                    {
                        nombre = nombreCarpeta,
                        ruta = carpetaPadre.ruta == null ? Path.Combine(carpetaPadre.nombre) : Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre),
                        nivel = carpetaPadre.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = ObtenerOrdenArchivo(padreID),
                        padreID = padreID,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPadre.grupoCarpeta,
                        subGrupoCarpeta = carpetaPadre.subGrupoCarpeta,
                    };
                    _context.tblGC_Archivo.Add(nuevaCarpeta);
                    _context.SaveChanges();

                    if (noEsAdmin)
                    {
                        // Crea el registro de permisos de la carpeta.
                        var nuevoPermiso = new tblGC_Permiso
                        {
                            usuarioID = vSesiones.sesionUsuarioDTO.id,
                            archivoID = nuevaCarpeta.id,
                            puedeSubir = true,
                            puedeCrear = true,
                            puedeDescargarCarpeta = true,
                            puedeEliminar = true,
                        };

                        _context.tblGC_Permiso.Add(nuevoPermiso);
                        _context.SaveChanges();
                    }

                    // Se crea el registro del permiso y se actualiza el estatus de las vistas para los demás usuarios.
                    CrearNuevosPermisosUsuarios(nuevaCarpeta);

                    var rutaFisicaCarpeta = Path.Combine(RUTA_BASE, nuevaCarpeta.ruta, nuevaCarpeta.nombre);
                    Directory.CreateDirectory(rutaFisicaCarpeta);

                    if (Directory.Exists(rutaFisicaCarpeta))
                    {
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "CrearCarpeta", e, AccionEnum.AGREGAR, padreID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar crear la carpeta en el servidor.");
                }
            }
            return resultado;
        }

        public Dictionary<string, object> CrearCarpetaSesion(GrupoCarpetaEnum grupoCarpeta)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                string rutaFisicaCarpetaSesion = "";
                try
                {
                    var listaCarpetasCreadas = new List<tblGC_Archivo>();
                    var listaCarpetasPadre = new List<tblGC_Archivo>();

                    switch (grupoCarpeta)
                    {
                        case GrupoCarpetaEnum.Consultivo:
                            listaCarpetasCreadas = CrearEstructuraSesionConsultivo(out rutaFisicaCarpetaSesion, out listaCarpetasPadre);
                            break;
                        case GrupoCarpetaEnum.Auditoria:
                            listaCarpetasCreadas = CrearEstructuraSesionAuditoria(out rutaFisicaCarpetaSesion, out listaCarpetasPadre);
                            break;
                        case GrupoCarpetaEnum.Practicas:
                            listaCarpetasCreadas = CrearEstructuraSesionPracticas(out rutaFisicaCarpetaSesion, out listaCarpetasPadre);
                            break;
                        default:
                            break;
                    }

                    var listaPermisosUsuarioActual = new List<tblGC_Permiso>();

                    var listaUsuariosIdsExtra = new List<int>();

                    // Se crea el registro de permiso de cada archivo creado para cada usuario
                    foreach (var carpetaCreada in listaCarpetasCreadas)
                    {

                        listaUsuariosIdsExtra.AddRange(CrearNuevosPermisosUsuarios(carpetaCreada));

                        // También se crea el registro para el usuario actual (Carla Velasco)
                        listaPermisosUsuarioActual.Add(new tblGC_Permiso
                        {
                            archivoID = carpetaCreada.id,
                            usuarioID = vSesiones.sesionUsuarioDTO.id,
                            puedeCrear = true,
                            puedeDescargarCarpeta = true,
                            puedeEliminar = true,
                            puedeSubir = true
                        });
                    }

                    // Se agrega el permiso de las carpetas padre a los usuarios extra
                    foreach (var usuarioExtra in listaUsuariosIdsExtra)
                    {
                        foreach (var carpetaPadre in listaCarpetasPadre)
                        {
                            listaPermisosUsuarioActual.Add(new tblGC_Permiso
                            {
                                archivoID = carpetaPadre.id,
                                usuarioID = usuarioExtra
                            });
                        }
                    }


                    _context.tblGC_Permiso.AddRange(listaPermisosUsuarioActual);
                    _context.SaveChanges();

                    // Se crean las carpetas en el servidor
                    foreach (var carpetaCreada in listaCarpetasCreadas)
                    {
                        var rutaFisicaCarpeta = Path.Combine(RUTA_BASE, carpetaCreada.ruta, carpetaCreada.nombre);
                        Directory.CreateDirectory(rutaFisicaCarpeta);
                    }

                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    // Intenta eliminar la carpeta sesión en caso de algún error para que no queden archivos basura.
                    try
                    {
                        Directory.Delete(rutaFisicaCarpetaSesion, true);
                    }
                    catch (Exception) { }

                    dbContextTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "CrearCarpetaSesion", e, AccionEnum.AGREGAR, (int)grupoCarpeta, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar crear la sesión en el servidor.");
                }
                return resultado;
            }
        }

        /// <summary>
        /// Crea un conjunto de carpetas para una sesión de consejo de consultivo
        /// </summary>
        private List<tblGC_Archivo> CrearEstructuraSesionConsultivo(out string rutaFisicaCarpetaSesion, out List<tblGC_Archivo> listaCarpetasPadre)
        {
            var listaCarpetasCreadas = new List<tblGC_Archivo>();
            listaCarpetasPadre = new List<tblGC_Archivo>();

            var carpetaConsultivo = _context.tblGC_Archivo.FirstOrDefault(x => x.activo && x.nivel == 2 && x.grupoCarpeta == GrupoCarpetaEnum.Consultivo);


            // Se crea la carpeta de la sesión
            var carpetaSesion = new tblGC_Archivo
            {
                nombre = String.Format("Sesión {0}", DateTime.Now.ToString(FORMATO_FECHA_SESION)),
                ruta = Path.Combine(carpetaConsultivo.nombre),
                nivel = carpetaConsultivo.nivel + 1,
                fechaCreacion = DateTime.Now,
                esCarpeta = true,
                orden = ObtenerOrdenArchivo(carpetaConsultivo.id),
                padreID = carpetaConsultivo.id,
                usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                activo = true,
                grupoCarpeta = carpetaConsultivo.grupoCarpeta,
                subGrupoCarpeta = carpetaConsultivo.subGrupoCarpeta,
            };
            _context.tblGC_Archivo.Add(carpetaSesion);
            listaCarpetasCreadas.Add(carpetaSesion);
            listaCarpetasPadre.Add(carpetaSesion);
            rutaFisicaCarpetaSesion = Path.Combine(RUTA_BASE, carpetaSesion.ruta, carpetaSesion.nombre);
            _context.SaveChanges();

            // Se crean las carpetas hijas

            #region Orden del día
            {
                var carpetaOrdenDia = new tblGC_Archivo
                {
                    nombre = "Orden del día",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 0,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaOrdenDia);
                listaCarpetasCreadas.Add(carpetaOrdenDia);
                _context.SaveChanges();
            }
            #endregion

            #region Acta
            {
                var carpetaActa = new tblGC_Archivo
                {
                    nombre = "Acta",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 1,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaActa);
                listaCarpetasCreadas.Add(carpetaActa);
                _context.SaveChanges();
            }
            #endregion

            #region Presentaciones
            {
                var carpetaPresentaciones = new tblGC_Archivo
                {
                    nombre = "Presentaciones",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 2,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaPresentaciones);
                listaCarpetasCreadas.Add(carpetaPresentaciones);
                listaCarpetasPadre.Add(carpetaPresentaciones);
                _context.SaveChanges();

                #region Finanzas
                {
                    var carpetaFinanzas = new tblGC_Archivo
                    {
                        nombre = "Finanzas",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 0,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaFinanzas);
                    listaCarpetasCreadas.Add(carpetaFinanzas);
                    _context.SaveChanges();
                }
                #endregion

                #region Comercialización
                {
                    var carpetaComercializacion = new tblGC_Archivo
                    {
                        nombre = "Comercialización",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 1,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.COM
                    };
                    _context.tblGC_Archivo.Add(carpetaComercializacion);
                    listaCarpetasCreadas.Add(carpetaComercializacion);
                    _context.SaveChanges();
                }
                #endregion

                #region Auditoría Interna
                {
                    var carpetaAuditoriaInterna = new tblGC_Archivo
                    {
                        nombre = "Auditoría Interna",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 2,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaAuditoriaInterna);
                    listaCarpetasCreadas.Add(carpetaAuditoriaInterna);
                    _context.SaveChanges();
                }
                #endregion

                #region TI
                {
                    var carpetaTI = new tblGC_Archivo
                    {
                        nombre = "TI",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 3,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.TI
                    };
                    _context.tblGC_Archivo.Add(carpetaTI);
                    listaCarpetasCreadas.Add(carpetaTI);
                    _context.SaveChanges();
                }
                #endregion

                #region RH
                {
                    var carpetaRH = new tblGC_Archivo
                    {
                        nombre = "RH",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 4,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.RH
                    };
                    _context.tblGC_Archivo.Add(carpetaRH);
                    listaCarpetasCreadas.Add(carpetaRH);
                    _context.SaveChanges();
                }
                #endregion

                #region Estrategia
                {
                    var carpetaEstrategia = new tblGC_Archivo
                    {
                        nombre = "Estrategia",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 5,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaEstrategia);
                    listaCarpetasCreadas.Add(carpetaEstrategia);
                    _context.SaveChanges();
                }
                #endregion

                #region Otros
                {
                    var carpetaOtros = new tblGC_Archivo
                    {
                        nombre = "Otros",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 6,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaOtros);
                    listaCarpetasCreadas.Add(carpetaOtros);
                    _context.SaveChanges();
                }
                #endregion

            }
            #endregion

            return listaCarpetasCreadas;
        }

        /// <summary>
        /// Crea un conjunto de carpetas para una sesión de comité de auditoría
        /// </summary>
        /// <returns></returns>
        private List<tblGC_Archivo> CrearEstructuraSesionAuditoria(out string rutaFisicaCarpetaSesion, out List<tblGC_Archivo> listaCarpetasPadre)
        {
            var listaCarpetasCreadas = new List<tblGC_Archivo>();
            listaCarpetasPadre = new List<tblGC_Archivo>();

            var carpetaAuditoria = _context.tblGC_Archivo.FirstOrDefault(x => x.activo && x.nivel == 2 && x.grupoCarpeta == GrupoCarpetaEnum.Auditoria);

            // Se crea la carpeta de la sesión
            var carpetaSesion = new tblGC_Archivo
            {
                nombre = String.Format("Sesión {0}", DateTime.Now.ToString(FORMATO_FECHA_SESION)),
                ruta = Path.Combine(carpetaAuditoria.nombre),
                nivel = carpetaAuditoria.nivel + 1,
                fechaCreacion = DateTime.Now,
                esCarpeta = true,
                orden = ObtenerOrdenArchivo(carpetaAuditoria.id),
                padreID = carpetaAuditoria.id,
                usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                activo = true,
                grupoCarpeta = carpetaAuditoria.grupoCarpeta,
                subGrupoCarpeta = carpetaAuditoria.subGrupoCarpeta,
            };
            _context.tblGC_Archivo.Add(carpetaSesion);
            listaCarpetasCreadas.Add(carpetaSesion);
            listaCarpetasPadre.Add(carpetaSesion);
            rutaFisicaCarpetaSesion = Path.Combine(RUTA_BASE, carpetaSesion.ruta, carpetaSesion.nombre);
            _context.SaveChanges();

            // Se crean las carpetas hijas

            #region Orden del día
            {
                var carpetaOrdenDia = new tblGC_Archivo
                {
                    nombre = "Orden del día",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 0,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaOrdenDia);
                listaCarpetasCreadas.Add(carpetaOrdenDia);
                _context.SaveChanges();
            }
            #endregion

            #region Acta
            {
                var carpetaActa = new tblGC_Archivo
                {
                    nombre = "Acta",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 1,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaActa);
                listaCarpetasCreadas.Add(carpetaActa);
                _context.SaveChanges();
            }
            #endregion

            #region Presentaciones
            {
                var carpetaPresentaciones = new tblGC_Archivo
                {
                    nombre = "Presentaciones",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 2,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaPresentaciones);
                listaCarpetasCreadas.Add(carpetaPresentaciones);
                listaCarpetasPadre.Add(carpetaPresentaciones);
                _context.SaveChanges();

                #region Finanzas
                {
                    var carpetaFinanzas = new tblGC_Archivo
                    {
                        nombre = "Finanzas",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 0,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaFinanzas);
                    listaCarpetasCreadas.Add(carpetaFinanzas);
                    _context.SaveChanges();
                }
                #endregion

                #region Control Interno
                {
                    var carpetaControlInterno = new tblGC_Archivo
                    {
                        nombre = "Control Interno",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 1,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaControlInterno);
                    listaCarpetasCreadas.Add(carpetaControlInterno);
                    _context.SaveChanges();
                }
                #endregion

                #region TI
                {
                    var carpetaTI = new tblGC_Archivo
                    {
                        nombre = "TI",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 2,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.TI
                    };
                    _context.tblGC_Archivo.Add(carpetaTI);
                    listaCarpetasCreadas.Add(carpetaTI);
                    _context.SaveChanges();
                }
                #endregion

                #region RH
                {
                    var carpetaRH = new tblGC_Archivo
                    {
                        nombre = "RH",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 3,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.RH
                    };
                    _context.tblGC_Archivo.Add(carpetaRH);
                    listaCarpetasCreadas.Add(carpetaRH);
                    _context.SaveChanges();
                }
                #endregion

                #region Otros
                {
                    var carpetaOtros = new tblGC_Archivo
                    {
                        nombre = "Otros",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 4,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaOtros);
                    listaCarpetasCreadas.Add(carpetaOtros);
                    _context.SaveChanges();
                }
                #endregion

            }
            #endregion

            return listaCarpetasCreadas;
        }

        /// <summary>
        /// Crea un conjunto de carpetas para una sesión de comité de prácticas societarias
        /// </summary>
        /// <returns></returns>
        private List<tblGC_Archivo> CrearEstructuraSesionPracticas(out string rutaFisicaCarpetaSesion, out List<tblGC_Archivo> listaCarpetasPadre)
        {
            var listaCarpetasCreadas = new List<tblGC_Archivo>();
            listaCarpetasPadre = new List<tblGC_Archivo>();
            var carpetaPracticas = _context.tblGC_Archivo.FirstOrDefault(x => x.activo && x.nivel == 2 && x.grupoCarpeta == GrupoCarpetaEnum.Practicas);

            // Se crea la carpeta de la sesión
            var carpetaSesion = new tblGC_Archivo
            {
                nombre = String.Format("Sesión {0}", DateTime.Now.ToString(FORMATO_FECHA_SESION)),
                ruta = Path.Combine(carpetaPracticas.nombre),
                nivel = carpetaPracticas.nivel + 1,
                fechaCreacion = DateTime.Now,
                esCarpeta = true,
                orden = ObtenerOrdenArchivo(carpetaPracticas.id),
                padreID = carpetaPracticas.id,
                usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                activo = true,
                grupoCarpeta = carpetaPracticas.grupoCarpeta,
                subGrupoCarpeta = carpetaPracticas.subGrupoCarpeta,
            };
            _context.tblGC_Archivo.Add(carpetaSesion);
            listaCarpetasCreadas.Add(carpetaSesion);
            listaCarpetasPadre.Add(carpetaSesion);
            rutaFisicaCarpetaSesion = Path.Combine(RUTA_BASE, carpetaSesion.ruta, carpetaSesion.nombre);
            _context.SaveChanges();

            // Se crean las carpetas hijas

            #region Orden del día
            {
                var carpetaOrdenDia = new tblGC_Archivo
                {
                    nombre = "Orden del día",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 0,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaOrdenDia);
                listaCarpetasCreadas.Add(carpetaOrdenDia);
                _context.SaveChanges();
            }
            #endregion

            #region Acta
            {
                var carpetaActa = new tblGC_Archivo
                {
                    nombre = "Acta",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 1,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaActa);
                listaCarpetasCreadas.Add(carpetaActa);
                _context.SaveChanges();
            }
            #endregion

            #region Presentaciones
            {
                var carpetaPresentaciones = new tblGC_Archivo
                {
                    nombre = "Presentaciones",
                    ruta = Path.Combine(carpetaSesion.ruta, carpetaSesion.nombre),
                    nivel = carpetaSesion.nivel + 1,
                    fechaCreacion = DateTime.Now,
                    esCarpeta = true,
                    orden = 2,
                    padreID = carpetaSesion.id,
                    usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                    activo = true,
                    grupoCarpeta = carpetaSesion.grupoCarpeta,
                    subGrupoCarpeta = carpetaSesion.subGrupoCarpeta,
                };
                _context.tblGC_Archivo.Add(carpetaPresentaciones);
                listaCarpetasCreadas.Add(carpetaPresentaciones);
                listaCarpetasPadre.Add(carpetaPresentaciones);
                _context.SaveChanges();

                #region Comercialización
                {
                    var carpetaComercializacion = new tblGC_Archivo
                    {
                        nombre = "Comercialización",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 0,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.COM
                    };
                    _context.tblGC_Archivo.Add(carpetaComercializacion);
                    listaCarpetasCreadas.Add(carpetaComercializacion);
                    _context.SaveChanges();
                }
                #endregion

                #region RH
                {
                    var carpetaRH = new tblGC_Archivo
                    {
                        nombre = "RH",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 1,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.RH
                    };
                    _context.tblGC_Archivo.Add(carpetaRH);
                    listaCarpetasCreadas.Add(carpetaRH);
                    _context.SaveChanges();
                }
                #endregion

                #region Nuevos Negocios
                {
                    var carpetaNuevosNegocios = new tblGC_Archivo
                    {
                        nombre = "Nuevos Negocios",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 2,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = SubGrupoCarpetaEnum.NN
                    };
                    _context.tblGC_Archivo.Add(carpetaNuevosNegocios);
                    listaCarpetasCreadas.Add(carpetaNuevosNegocios);
                    _context.SaveChanges();
                }
                #endregion

                #region Otros
                {
                    var carpetaOtros = new tblGC_Archivo
                    {
                        nombre = "Otros",
                        ruta = Path.Combine(carpetaPresentaciones.ruta, carpetaPresentaciones.nombre),
                        nivel = carpetaPresentaciones.nivel + 1,
                        fechaCreacion = DateTime.Now,
                        esCarpeta = true,
                        orden = 3,
                        padreID = carpetaPresentaciones.id,
                        usuarioCreadorID = vSesiones.sesionUsuarioDTO.id,
                        activo = true,
                        grupoCarpeta = carpetaPresentaciones.grupoCarpeta,
                        subGrupoCarpeta = carpetaPresentaciones.subGrupoCarpeta
                    };
                    _context.tblGC_Archivo.Add(carpetaOtros);
                    listaCarpetasCreadas.Add(carpetaOtros);
                    _context.SaveChanges();
                }
                #endregion

            }
            #endregion

            return listaCarpetasCreadas;
        }

        public Dictionary<string, object> RenombrarArchivo(string nuevoNombre, long archivoID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Verifica que el usuario tenga permiso de editar carpetas.
                    if (noEsAdmin)
                    {
                        var permisoEditarCarpeta = _context.tblGC_Permiso
                            .FirstOrDefault(x =>
                                (x.archivoID == archivoID &&
                                x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                                x.puedeCrear));

                        if (permisoEditarCarpeta == null)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                            return resultado;
                        }
                    }

                    tblGC_Archivo archivo = _context.tblGC_Archivo.FirstOrDefault(x => x.id == archivoID && x.activo);

                    if (archivo.nombre == nuevoNombre.Trim() || nuevoNombre == String.Empty)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El nuevo nombre es igual al anterior.");
                    }
                    else if (!archivo.esCarpeta)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Solo se pueden renombrar carpetas.");
                    }
                    else if (EsNombreCarpetaInvalido(nuevoNombre))
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El nombre contiene caracteres inválidos.");
                    }
                    else
                    {
                        try
                        {
                            bool rutasActualizadas;
                            string nombreAnterior = archivo.nombre;
                            string rutaAntes = Path.Combine(archivo.ruta, nombreAnterior);
                            string rutaDespues = Path.Combine(archivo.ruta, nuevoNombre);

                            try
                            {
                                ActualizarRutasArchivosHijos(archivo.id, rutaAntes, rutaDespues);

                                rutasActualizadas = true;
                            }
                            catch (Exception)
                            {
                                rutasActualizadas = false;
                            }

                            if (rutasActualizadas)
                            {
                                archivo.nombre = nuevoNombre;
                                _context.SaveChanges();

                                resultado.Add(SUCCESS, true);
                                resultado.Add("archivoID", archivoID);

                                string rutaNombreAnterior = Path.Combine(RUTA_BASE, archivo.ruta, nombreAnterior);
                                string rutaNombreNuevo = Path.Combine(RUTA_BASE, archivo.ruta, nuevoNombre);
                                Directory.Move(rutaNombreAnterior, rutaNombreNuevo);
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "No se pudo renombrar a los archivos hijos de la carpeta.");
                            }
                        }
                        catch (Exception e)
                        {
                            LogError(0, 0, nombreControlador, "RenombrarArchivo", e, AccionEnum.ACTUALIZAR, archivoID, null);
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error interno al intentar crear la carpeta.");
                        }
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "RenombrarArchivo", e, AccionEnum.ACTUALIZAR, archivoID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar renombrar el archivo en el servidor.");
                }
            }

            return resultado;
        }

        public Dictionary<string, object> DescargarArchivo(long archivoID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                tblGC_Archivo archivo = _context.tblGC_Archivo.FirstOrDefault(x => x.id == archivoID && x.activo);

                // Verifica que el usuario tenga permiso de descargar el archivo.
                if (noEsAdmin)
                {
                    var permisoDescargarArchivo = _context.tblGC_Permiso
                        .FirstOrDefault(x =>
                            (x.archivoID == archivoID &&
                            x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                            x.puedeDescargarArchivo));

                    if (permisoDescargarArchivo == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                        return resultado;
                    }
                }

                string nombreServidor = archivo.nombre.Replace(Path.GetExtension(archivo.nombre), ".zip");
                string rutaFisicaZip = Path.Combine(RUTA_BASE, archivo.ruta, nombreServidor);
                Stream fileStream = GlobalUtils.GetFileFromZipAsStream(rutaFisicaZip, archivo.nombre);
                resultado.Add("archivo", fileStream);
                resultado.Add("nombreDescarga", archivo.nombre);
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                LogError(0, 0, nombreControlador, "DescargarArchivo", e, AccionEnum.DESCARGAR, archivoID, null);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> DescargarCarpeta(long carpetaID)
        {
            string rutaFolderTemporal = "";
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var archivoFolder = _context.tblGC_Archivo.FirstOrDefault(x => x.id == carpetaID);

                // Verifica que el usuario tenga permiso de descargar la carpeta.
                if (noEsAdmin)
                {
                    var permisoDescargarCarpeta = _context.tblGC_Permiso
                        .FirstOrDefault(x =>
                            (x.archivoID == carpetaID &&
                            x.usuarioID == vSesiones.sesionUsuarioDTO.id &&
                            x.puedeDescargarCarpeta));
                    if (permisoDescargarCarpeta == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El usuario no cuenta con los permisos suficientes para realizar esta acción.");
                        return resultado;
                    }
                }

                var listaArchivosHijos = _context.tblGC_Archivo
                    .Where(archivo => archivo.padreID == carpetaID && archivo.activo)
                    .ToList();

                if (!archivoFolder.esCarpeta || listaArchivosHijos.Count == 0)
                {
                    resultado.Add(SUCCESS, false);
                    return resultado;
                }

                string nombreFolderTemporal = String.Format("{0} [{1}]", "tmp", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss").Replace(":", "-"));
                rutaFolderTemporal = Path.Combine(RUTA_TEMPORALES, nombreFolderTemporal);
                Directory.CreateDirectory(rutaFolderTemporal);

                // Función rescursiva para crear archivos hijos.
                CrearArchivosHijos(rutaFolderTemporal, listaArchivosHijos);

                // Ya que esta la carpeta temporal creada, se crea el zip
                string rutaNuevoZip = Path.Combine(RUTA_TEMPORALES, nombreFolderTemporal + ".zip");
                GlobalUtils.ComprimirCarpeta(rutaFolderTemporal, rutaNuevoZip);

                // Una vez creado el zip, se elimina el folder temporal.
                Directory.Delete(rutaFolderTemporal, true);

                resultado.Add(SUCCESS, true);
                resultado.Add("rutaDescarga", rutaNuevoZip);
                resultado.Add("nombreDescarga", Path.GetFileNameWithoutExtension(archivoFolder.nombre) + ".zip");
            }
            catch (Exception e)
            {
                // Intenta eliminar la carpeta temporal
                try
                {
                    File.Delete((rutaFolderTemporal + ".zip"));
                }
                catch (Exception) { }

                LogError(0, 0, nombreControlador, "DescargarCarpeta", e, AccionEnum.DESCARGAR, carpetaID, null);
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        #region helper methods
        private List<DirectorioDTO> ObtenerEstructuraSubcarpetas(DirectorioDTO directorioPadre, ref int index, ref List<tblGC_Archivo> listaArchivos, ref List<tblGC_Permiso> listaPermisosUsuario)
        {

            List<tblGC_Archivo> archivosHijos = listaArchivos
                .Where(archivo => archivo.padreID == directorioPadre.id)
                .OrderBy(archivo => archivo.esCarpeta).ThenBy(archivo => archivo.orden).ThenBy(archivo => archivo.nombre)
                .ToList();

            int usuarioID = vSesiones.sesionUsuarioDTO.id;

            foreach (var archivo in archivosHijos)
            {
                // Si no es admin, verifica si tiene acceso a la carpeta.
                if (noEsAdmin)
                {
                    var permisoArchivo = listaPermisosUsuario.FirstOrDefault(x => x.archivoID == archivo.id);

                    // Si no tiene permiso a ese archivo, salta a la siguiente iteración.
                    if (permisoArchivo == null)
                    {
                        continue;
                    }
                }

                var directorioTemp = new DirectorioDTO
                {
                    index = index++,
                    value = archivo.nombre,
                    date = archivo.fechaCreacion.ToString(FORMATO_FECHA_HORA),
                    id = archivo.id,
                    pId = archivo.padreID,
                    data = new List<DirectorioDTO>(),
                    permisos = ObtenerPermisosArchivo(archivo.id, ref listaPermisosUsuario),
                    grupoCarpeta = archivo.grupoCarpeta,
                    subGrupoCarpeta = archivo.subGrupoCarpeta
                };

                if (archivo.esCarpeta)
                {
                    directorioTemp.type = TIPO_DEFAULT_CARPETA;
                    directorioTemp.open = archivo.nivel < 2;
                }
                else
                {
                    directorioTemp.type = Path.GetExtension(archivo.nombre).Substring(1);
                }

                if (archivo.esCarpeta && listaArchivos.Any(x => x.padreID == archivo.id))
                {
                    directorioTemp.data = ObtenerEstructuraSubcarpetas(directorioTemp, ref index, ref listaArchivos, ref listaPermisosUsuario);
                }

                directorioPadre.data.Add(directorioTemp);
            }

            return directorioPadre.data;
        }

        private PermisosDTO ObtenerPermisosArchivo(long archivoID, ref List<tblGC_Permiso> listaPermisosUsuario)
        {
            if (!noEsAdmin)
            {
                return new PermisosDTO()
                {
                    puedeSubir = true,
                    puedeEliminar = true,
                    puedeDescargarArchivo = true,
                    puedeDescargarCarpeta = true,
                    puedeCrear = true,
                };
            }

            PermisosDTO permisos = listaPermisosUsuario
                                    .Where(permiso => (permiso.archivoID == archivoID && permiso.usuarioID == vSesiones.sesionUsuarioDTO.id))
                                    .Select(x => new PermisosDTO
                                    {
                                        puedeSubir = x.puedeSubir,
                                        puedeEliminar = x.puedeEliminar,
                                        puedeDescargarArchivo = x.puedeDescargarArchivo,
                                        puedeDescargarCarpeta = x.puedeDescargarCarpeta,
                                        puedeCrear = x.puedeCrear
                                    })
                                    .FirstOrDefault();

            return permisos ?? new PermisosDTO();
        }

        private int ObtenerOrdenArchivo(long padreID)
        {
            var archivosHijos = _context.tblGC_Archivo.Where(archivo => archivo.padreID == padreID).ToList();
            return archivosHijos.Count > 0 ? (archivosHijos.Max(x => x.orden) + 1) : 0;
        }

        private List<int> CrearNuevosPermisosUsuarios(tblGC_Archivo archivo)
        {
            var listaPermisos = new List<tblGC_Permiso>();
            var listaUsuariosExtraId = new List<int>();
            List<int> listaUsuarios = new List<int>();

            // Se añaden especiales en caso de un subgrupo
            var usuariosExtraId = AñadirUsuariosSubGrupo(archivo.subGrupoCarpeta);

            listaUsuariosExtraId.AddRange(usuariosExtraId);
            listaUsuarios.AddRange(usuariosExtraId);

            // Se añaden los usuarios dependiendo del grupo de la carpeta.
            switch (archivo.grupoCarpeta)
            {
                case GrupoCarpetaEnum.Asamblea:
                    listaUsuarios.AddRange(ObtenerUsuariosIdsAsamblea());
                    break;
                case GrupoCarpetaEnum.Consultivo:
                    listaUsuarios.AddRange(ObtenerUsuariosIdsConsultivo());
                    break;
                case GrupoCarpetaEnum.Auditoria:
                    listaUsuarios.AddRange(ObtenerUsuariosIdsAuditoria());
                    break;
                case GrupoCarpetaEnum.Practicas:
                    listaUsuarios.AddRange(ObtenerUsuariosIdsPracticas());
                    break;
                case GrupoCarpetaEnum.DireccionGeneral:
                    listaUsuarios.AddRange(ObtenerUsuariosIdsDireccionGeneral());
                    break;
                default:
                    break;
            }

            foreach (var usuarioID in listaUsuarios)
            {
                listaPermisos.Add(new tblGC_Permiso
                {
                    archivoID = archivo.id,
                    usuarioID = usuarioID,
                    puedeDescargarCarpeta = archivo.esCarpeta,
                    puedeDescargarArchivo = !archivo.esCarpeta
                });
            }

            _context.tblGC_Permiso.AddRange(listaPermisos);

            _context.SaveChanges();

            return listaUsuariosExtraId;
        }

        private List<int> AñadirUsuariosSubGrupo(SubGrupoCarpetaEnum subGrupoCarpeta)
        {
            switch (subGrupoCarpeta)
            {
                case SubGrupoCarpetaEnum.COM:
                    return new List<int> { JosePedroGonzalesID, AnaVidalID };
                case SubGrupoCarpetaEnum.TI:
                    return new List<int> { MartinValleID };
                case SubGrupoCarpetaEnum.RH:
                    return new List<int> { CarlosAmezcuaID };
                case SubGrupoCarpetaEnum.NN:
                    return new List<int> { FranciscoReinaID };
                default:
                    return new List<int>();
            }
        }

        private bool EsNombreCarpetaInvalido(string nombreCarpeta)
        {
            string invalidFileNameRegex = @"[^a-zA-Z0-9áéíóúüñÑ_.\- ]+";
            return Regex.Match(nombreCarpeta, invalidFileNameRegex, RegexOptions.IgnoreCase).Success;
        }

        private void ActualizarRutasArchivosHijos(long archivoID, string rutaNombreViejo, string rutaNombreNuevo)
        {
            List<tblGC_Archivo> archivosHijos = _context.tblGC_Archivo.Where(x => x.padreID == archivoID && x.activo).ToList();
            foreach (var archivoHijo in archivosHijos)
            {
                archivoHijo.ruta = archivoHijo.ruta.Replace(rutaNombreViejo, rutaNombreNuevo);
                if (archivoHijo.esCarpeta)
                {
                    ActualizarRutasArchivosHijos(archivoHijo.id, rutaNombreViejo, rutaNombreNuevo);
                }
            }
        }

        private void EliminarPermisosUsuarios(long archivoID, long padreID)
        {
            _context.tblGC_Permiso.RemoveRange(_context.tblGC_Permiso.Where(x => x.archivoID == archivoID));
            _context.SaveChanges();
        }

        private void CrearArchivosHijos(string rutaPadre, List<tblGC_Archivo> archivosHijos)
        {
            foreach (var archivoHijo in archivosHijos)
            {
                string nuevaRutaArchivo;


                if (noEsAdmin)
                {
                    // Verifica si el usuario tiene acceso al archivo.
                    var tienePermiso = _context.tblGC_Permiso
                        .FirstOrDefault(x => x.archivoID == archivoHijo.id && x.usuarioID == vSesiones.sesionUsuarioDTO.id);

                    // Si está nulo, significa que no tiene acceso al archivo y salta a la siguiente iteración.
                    if (tienePermiso == null)
                    {
                        continue;
                    }
                }

                // Si el archivo es una carpeta, simplemente la crea en la nueva ruta.
                if (archivoHijo.esCarpeta)
                {
                    nuevaRutaArchivo = Path.Combine(rutaPadre, archivoHijo.nombre);
                    Directory.CreateDirectory(nuevaRutaArchivo);

                    // Verifica si la carpeta tiene hijos.
                    var listaArchivosHijos = _context.tblGC_Archivo
                        .Where(archivo => archivo.padreID == archivoHijo.id && archivo.activo)
                        .ToList();

                    // Si la carpeta tiene hijos, se llama a la función recursivamente.
                    if (listaArchivosHijos.Count > 0)
                    {
                        CrearArchivosHijos(nuevaRutaArchivo, listaArchivosHijos);
                    }
                }
                else
                {
                    string nombreArchivoServidor = Path.GetFileNameWithoutExtension(archivoHijo.nombre) + ".zip";

                    string rutaArchivoServidor = Path.Combine(RUTA_BASE, archivoHijo.ruta, nombreArchivoServidor);
                    nuevaRutaArchivo = Path.Combine(rutaPadre, archivoHijo.nombre);

                    // Crea el archivo en el folder temporal.
                    GlobalUtils.SaveFileFromZip(rutaArchivoServidor, archivoHijo.nombre, nuevaRutaArchivo);
                }
            }
        }
        #endregion

        #region ID usuarios permiso
        int AlfonsoReinaID = 1085;
        int GerardoReinaID = 1164;
        int JosePedroGonzalesID = 1043;
        //int ArnulfoIslasID = 1070;
        int ArturoSanchezID = 1073;
        int FranciscoReinaID = 7;
        int AdrianaReinaID = 1080;
        int AnaVidalID = 1095;
        int JesusVillegasID = 1182;
        int CarlosAmezcuaID = 3912;
        int MartinValleID = 9;
        int JuanPabloID = 1170;
        int EdmundoFraijoID = 1063;
        int FranciscoArtalejoID = 1176;
        int JosePedroID = 1043;
        int AlbertoAzpeID = 6489;

        // Usuarios especiales (sólo acceso a la vista)
        int RodrigoReinaID = 6416;
        int CarlosCarpyID = 6417;
        int FaustoGarciaID = 6418;
        int YolandaOrtegaID = 6419;

        private List<int> ObtenerUsuariosIdsAsamblea()
        {
            return new List<int>{
                AlfonsoReinaID,
                GerardoReinaID,
                ArturoSanchezID
            };
        }

        private List<int> ObtenerUsuariosIdsConsultivo()
        {
            return new List<int>{
                AlfonsoReinaID,
                GerardoReinaID,
                RodrigoReinaID,
                FaustoGarciaID,
                CarlosCarpyID,
                //ArnulfoIslasID,
                ArturoSanchezID,
                AdrianaReinaID,
                JesusVillegasID,
                YolandaOrtegaID
            };
        }

        private List<int> ObtenerUsuariosIdsAuditoria()
        {
            return new List<int>
            {
                AlfonsoReinaID,
                GerardoReinaID,
                FaustoGarciaID,
                CarlosCarpyID,
                //ArnulfoIslasID,
                ArturoSanchezID,
                AdrianaReinaID,
                JesusVillegasID,
                YolandaOrtegaID
            };
        }

        private List<int> ObtenerUsuariosIdsPracticas()
        {
            return new List<int>
            {
                AlfonsoReinaID,
                GerardoReinaID,
                JuanPabloID,
                EdmundoFraijoID,
                FranciscoArtalejoID,
                JosePedroID,
                AlbertoAzpeID,
                JesusVillegasID
            };
        }

        private List<int> ObtenerUsuariosIdsDireccionGeneral()
        {
            return new List<int>
            {
                AlfonsoReinaID,
                GerardoReinaID
            };
        }
        #endregion

        #region inicializar permisos archivos

        private void InicializarPermisosUsuarios()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var archivos = _context.tblGC_Archivo.Where(x => x.activo).ToArray();

                    foreach (var archivo in archivos)
                    {
                        var listaUsuariosId = new List<int>();
                        var listaUsuariosExtra = new List<int>();
                        switch (archivo.grupoCarpeta)
                        {
                            case GrupoCarpetaEnum.Asamblea:
                                listaUsuariosId = ObtenerUsuariosIdsAsamblea();
                                break;
                            case GrupoCarpetaEnum.Consultivo:
                                listaUsuariosId = ObtenerUsuariosIdsConsultivo();
                                listaUsuariosExtra.AddRange(new List<int>
                                {
                                    JosePedroGonzalesID,AnaVidalID,MartinValleID,CarlosAmezcuaID
                                });
                                break;
                            case GrupoCarpetaEnum.Auditoria:
                                listaUsuariosId = ObtenerUsuariosIdsAuditoria();
                                listaUsuariosExtra.AddRange(new List<int>
                                {
                                    MartinValleID,CarlosAmezcuaID
                                });
                                break;
                            case GrupoCarpetaEnum.Practicas:
                                listaUsuariosId = ObtenerUsuariosIdsPracticas();
                                //listaUsuariosExtra.AddRange(new List<int>
                                //{
                                //    JosePedroGonzalesID,AnaVidalID,CarlosAmezcuaID,FranciscoReinaID
                                //});
                                break;
                            default:
                                break;
                        }

                        foreach (var usuarioID in listaUsuariosId)
                        {
                            _context.tblGC_Permiso.Add(new tblGC_Permiso
                            {
                                usuarioID = usuarioID,
                                archivoID = archivo.id,
                                puedeDescargarCarpeta = true,
                                puedeDescargarArchivo = true
                            });
                        }

                        foreach (var usuarioID in listaUsuariosExtra)
                        {
                            _context.tblGC_Permiso.Add(new tblGC_Permiso
                            {
                                usuarioID = usuarioID,
                                archivoID = archivo.id
                            });
                        }

                        _context.SaveChanges();
                    }


                    _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        #endregion

    }
}
