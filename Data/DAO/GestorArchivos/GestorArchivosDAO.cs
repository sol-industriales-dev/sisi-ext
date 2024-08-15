using Core.DAO.GestorArchivos;
using Core.Entity.GestorArchivos;
using Data.EntityFramework.Generic;
using Core.DTO.GestorArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entity.Principal.Usuarios;
using System.IO;
using Core.Enum.Principal.Bitacoras;
using System.Data.Entity.Infrastructure;
using System.Web;
using Infrastructure.Utils;
using Core.DTO.Principal.Generales;


using System;
using System.IO;
using System.IO.Compression;
using Data.EntityFramework.Context;
using Data.DAO.Principal.Usuarios;
using Core.DTO;
using Data.Factory.Principal.Archivos;

namespace Data.DAO.GestorArchivos
{
    public class GestorArchivosDAO : GenericDAO<tblGA_Directorio>, IGestorArchivosDAO
    {

        //Variables constantes
        ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        private readonly string NOMBRE_DEFAULT_CARPETA = "nueva carpeta";
        private readonly string FORMATO_FECHA_HORA = "yyyy-MM-dd HH:mm:ss";
        private const string TIPO_DEFAULT_CARPETA = "folder";

        private readonly string RUTA_BASE_CONSTRUPLAN = @"\\REPOSITORIO\Proyecto\SIGOPLAN\GESTOR_ARCHIVOS\CONSTRUPLAN";
        private readonly string RUTA_BASE_ARRENDADORA = @"\\REPOSITORIO\Proyecto\SIGOPLAN\GESTOR_ARCHIVOS\ARRENDADORA";

        private readonly string NOMBRE_CARPETA_ELIMINADOS = "[[[Eliminados]]]";
        private readonly string NOMBRE_CARPETA_ACTUALIZADOS = "[[[Actualizados]]]";
        private readonly string NOMBRE_CARPETA_COMPRIMIDOS = "[[[Comprimidos]]]";

        public bool VerificarAccesoPrincipal(int usuarioID)
        {

            ///////////////////////////////////////////////////////////////////////////////////////////
            //cargarCarpetaRaizManualmente(@"C:\Proyecto\SIGOPLAN\GESTOR_ARCHIVOS\CONSTRUPLAN\Licitaciones", 10, 1);
            ///////////////////////////////////////////////////////////////////////////////////////////

            //Verifica si el usuario tiene acceso a las carpetas raíz de los departamentos en los cuales tiene registro de acceso
            try
            {

                var usuario = new UsuarioDAO();
                bool esAdmin = usuario.getViewAction(vSesiones.sesionCurrentView, "Administrar");
                if (esAdmin)
                {
                    return true;
                }


                List<tblGA_AccesoDepartamento> listaAccesoDepartamentos = _context.tblGA_AccesoDepartamento.Where(x => x.usuarioID.Equals(usuarioID)).ToList();
                if (listaAccesoDepartamentos.Count.Equals(0))
                {
                    return false;
                }

                var numeroVistasActivas = listaAccesoDepartamentos.Count;

                foreach (var accesoDepartamento in listaAccesoDepartamentos)
                {
                    int departamentoID = accesoDepartamento.departamentoID;

                    tblGA_Version carpetaRaizDepartamento = _context.tblGA_Version
                        .FirstOrDefault(x => x.directorio.departamentoID == departamentoID && x.esActivo && x.directorio.nivel.Equals(0));

                    tblGA_Vistas vistaCarpetaDepartamento = _context.tblGA_Vistas
                        .FirstOrDefault(x => x.directorioID.Equals(carpetaRaizDepartamento.directorioID) && x.usuarioID.Equals(usuarioID) && x.estatusVista != 0);
                    if (vistaCarpetaDepartamento == null)
                    {
                        numeroVistasActivas--;
                    }
                }

                if (numeroVistasActivas.Equals(0))
                {
                    return false;
                }

            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public DirectorioDTO ObtenerEstructuraDirectorios(int usuarioID)
        {

            var contenedor = new DirectorioDTO();
            contenedor.data = new List<DirectorioDTO>();

            try
            {
                // Si el usuario es administrador, carga la carpeta de su departamento.
                List<int> departamentosID;
                var usuario = new UsuarioDAO();
                bool esAdmin = usuario.getViewAction(vSesiones.sesionCurrentView, "Administrar");
                if (esAdmin)
                {
                    departamentosID = _context.tblP_Usuario.Where(x => x.id.Equals(usuarioID)).Select(x => x.puesto.departamentoID).ToList();
                }
                else
                {
                    departamentosID = _context.tblGA_AccesoDepartamento.Where(x => x.usuarioID.Equals(usuarioID)).Select(x => x.departamentoID).ToList();
                }


                int index = 1;

                foreach (var accesoDepartamento in departamentosID)
                {

                    var directorioTemp = new DirectorioDTO(); //Contender donde se guardara cada carpeta raiz

                    int departamentoID = accesoDepartamento;

                    //Obtener carpeta raiz del departamento
                    tblGA_Version carpetaDepartamento = _context.tblGA_Version
                        .FirstOrDefault(x => x.directorio.departamentoID == departamentoID && x.esActivo && x.directorio.nivel.Equals(0));


                    if (!esAdmin)
                    {
                        //Verifica si tiene acceso a la vista de la carpeta raiz
                        tblGA_Vistas vistaCarpetaDepartamento = _context.tblGA_Vistas
                            .FirstOrDefault(x => x.directorioID.Equals(carpetaDepartamento.directorioID) && x.usuarioID.Equals(usuarioID) && x.estatusVista != 0);
                        if (vistaCarpetaDepartamento == null)
                        {
                            continue;
                        }
                    }

                    //Si el usuario es admin, le otorga todos los permisos
                    if (esAdmin)
                    {
                        directorioTemp.permisos = new PermisosDTO
                        {
                            usuarioID = usuarioID,
                            directorioID = carpetaDepartamento.directorioID,
                            puedeSubir = true,
                            puedeEliminar = true,
                            puedeActualizar = true,
                            puedeCrear = true,
                            puedeDescargarArchivo = true,
                            puedeDescargarCarpeta = true
                        };
                    }
                    else
                    {
                        directorioTemp.permisos = ObtenerPermisosCarpeta(carpetaDepartamento.directorioID, usuarioID);
                    }

                    directorioTemp.index = index++;
                    directorioTemp.value = carpetaDepartamento.nombre;
                    directorioTemp.type = TIPO_DEFAULT_CARPETA;
                    directorioTemp.date = carpetaDepartamento.fecha.ToString(FORMATO_FECHA_HORA);
                    directorioTemp.id = carpetaDepartamento.directorioID;
                    directorioTemp.pId = carpetaDepartamento.directorio.padreID;
                    directorioTemp.level = carpetaDepartamento.directorio.nivel;
                    directorioTemp.open = true;
                    directorioTemp.data = new List<DirectorioDTO>();
                    directorioTemp.data = ObtenerEstructuraSubcarpetas(directorioTemp.data, directorioTemp.id, ref index, usuarioID, esAdmin);

                    contenedor.data.Add(directorioTemp);

                }

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new DirectorioDTO();
            }

            return contenedor;
        }

        private List<DirectorioDTO> ObtenerEstructuraSubcarpetas(List<DirectorioDTO> subarchivos, int carpetaPadreID, ref int index, int usuarioID, bool esAdmin)
        {

            try
            {
                List<tblGA_Version> archivosHijos = _context.tblGA_Version.Where(x => x.directorio.padreID.Equals(carpetaPadreID) && x.esActivo).OrderBy(x => x.nombre).ToList();

                foreach (var archivo in archivosHijos)
                {

                    var directorioTemp = new DirectorioDTO();

                    if (!esAdmin)
                    {
                        //Verifica si tiene acceso a la carpeta
                        if (archivo.directorio.esCarpeta)
                        {
                            tblGA_Vistas vista = _context.tblGA_Vistas
                                .FirstOrDefault(x => x.directorioID.Equals(archivo.directorioID) && x.estatusVista != 0 && x.usuarioID.Equals(usuarioID));
                            if (vista == null)
                            {
                                continue;
                            }
                        }
                    }

                    //Si el usuario es admin, le otorga todos los permisos
                    if (esAdmin)
                    {
                        directorioTemp.permisos = new PermisosDTO
                        {
                            usuarioID = usuarioID,
                            directorioID = archivo.directorioID,
                            puedeSubir = true,
                            puedeEliminar = true,
                            puedeActualizar = true,
                            puedeCrear = true,
                            puedeDescargarArchivo = true,
                            puedeDescargarCarpeta = true
                        };
                    }
                    else
                    {
                        directorioTemp.permisos = ObtenerPermisosCarpeta(archivo.directorioID, usuarioID);
                    }

                    directorioTemp.index = index++;
                    directorioTemp.value = archivo.nombre;
                    directorioTemp.date = archivo.fecha.ToString(FORMATO_FECHA_HORA);
                    directorioTemp.id = archivo.directorioID;
                    directorioTemp.userID = archivo.usuarioID;
                    directorioTemp.pId = archivo.directorio.padreID;
                    directorioTemp.data = new List<DirectorioDTO>();

                    if (archivo.directorio.esCarpeta)
                    {
                        directorioTemp.type = TIPO_DEFAULT_CARPETA;
                        directorioTemp.open = true;
                    }
                    else
                    {
                        directorioTemp.type = this.ObtenerExtensionArchivo(archivo.nombre);
                    }

                    directorioTemp.data = ObtenerEstructuraSubcarpetas(directorioTemp.data, directorioTemp.id, ref index, usuarioID, esAdmin);

                    subarchivos.Add(directorioTemp);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<DirectorioDTO>();
            }

            return subarchivos;
        }

        public Dictionary<string, object> AgregarCarpeta(int padreID, int usuarioID, int empresa)
        {

            var nuevoDirectorio = new tblGA_Directorio();
            var nuevaVersionDirectorio = new tblGA_Version();
            tblGA_Version carpetaPadre;
            var resultado = new Dictionary<string, object>();

            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        carpetaPadre = context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(padreID) && x.esActivo);
                        int nivel = carpetaPadre.directorio.nivel;

                        nuevoDirectorio.padreID = padreID;
                        nuevoDirectorio.nivel = ++nivel;
                        nuevoDirectorio.esCarpeta = true;
                        nuevoDirectorio.departamentoID = carpetaPadre.directorio.departamentoID;
                        context.tblGA_Directorio.Add(nuevoDirectorio);
                        context.SaveChanges();

                        nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
                        nuevaVersionDirectorio.esActivo = true;
                        nuevaVersionDirectorio.version = 1;
                        nuevaVersionDirectorio.usuarioID = usuarioID;

                        if (carpetaPadre.ruta == null)
                        {
                            nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.nombre);
                        }
                        else
                        {

                            nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre);

                            var usuario = new UsuarioDAO();
                            bool esAdmin = usuario.getViewAction(vSesiones.sesionCurrentView, "Administrar");
                            if (!esAdmin)
                            {
                                //otorgar los mismos permisos de la carpeta padre a la carpeta hija al usuario que creó la carpeta.
                                tblGA_Permisos permisosCarpetaHija = new tblGA_Permisos();
                                tblGA_Permisos permisosCarpetaPadre = context.tblGA_Permisos.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpetaPadre.directorioID));
                                if (permisosCarpetaPadre != null)
                                {
                                    permisosCarpetaHija.usuarioID = usuarioID;
                                    permisosCarpetaHija.directorioID = nuevaVersionDirectorio.directorioID;
                                    permisosCarpetaHija.puedeSubir = permisosCarpetaPadre.puedeSubir;
                                    permisosCarpetaHija.puedeEliminar = permisosCarpetaPadre.puedeEliminar;
                                    permisosCarpetaHija.puedeDescargarArchivo = permisosCarpetaPadre.puedeDescargarArchivo;
                                    permisosCarpetaHija.puedeDescargarCarpeta = permisosCarpetaPadre.puedeDescargarCarpeta;
                                    permisosCarpetaHija.puedeActualizar = permisosCarpetaPadre.puedeActualizar;
                                    permisosCarpetaHija.puedeCrear = permisosCarpetaPadre.puedeCrear;
                                    context.tblGA_Permisos.Add(permisosCarpetaHija);
                                }
                            }
                        }

                        nuevaVersionDirectorio.nombre = NOMBRE_DEFAULT_CARPETA + " (" + nuevoDirectorio.id + ")";

                        nuevaVersionDirectorio.fecha = DateTime.Now;

                        string ruta = Path.Combine(ObtenerRutaBase(empresa), nuevaVersionDirectorio.ruta, nuevaVersionDirectorio.nombre);
                        Directory.CreateDirectory(ruta);
                        Console.WriteLine("Archivo creado en: " + ruta);
                        if (Directory.Exists(ruta))
                        {
                            context.tblGA_Version.Add(nuevaVersionDirectorio);

                            //Seleccionar a todos los usuarios que tengan acceso a la vista padre
                            List<tblGA_Vistas> listaUsuarios = context.tblGA_Vistas.Where(x => x.directorioID.Equals(carpetaPadre.directorioID) && x.estatusVista != 0).ToList();

                            foreach (var usuario in listaUsuarios)
                            {
                                tblGA_Vistas vista = new tblGA_Vistas();
                                vista.usuarioID = usuario.usuarioID;
                                vista.directorioID = nuevaVersionDirectorio.directorioID;
                                vista.estatusVista = 2;
                                context.tblGA_Vistas.Add(vista);
                            }

                        }
                        else
                        {
                            Console.WriteLine("No se pudo crear la carpeta en la ubicación: " + ruta);
                            dbContextTransaction.Rollback();
                            resultado.Add("error", "Ya existe una carpeta con ese nombre en la ruta indicada");
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        resultado.Add("error", e.Message);
                    }
                }
            }
            resultado.Add("id", nuevoDirectorio.id);
            resultado.Add("nombre", nuevaVersionDirectorio.nombre);
            return resultado;
        }

        public List<DirectorioDTO> SubirArchivo(List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa)
        {

            var _contextVersion = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Version>();
            tblGA_Version carpetaPadre;
            var listaDTOs = new List<DirectorioDTO>();
            tblGA_Directorio nuevoDirectorio;
            tblGA_Version nuevaVersionDirectorio;

            try
            {
                carpetaPadre = _context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(padreID) && x.esActivo);

                foreach (var archivo in listaArchivos)
                {
                    string rutaArchivo;

                    if (carpetaPadre.ruta == null)
                    {
                        rutaArchivo = Path.Combine(ObtenerRutaBase(empresa), carpetaPadre.nombre, archivo.FileName);
                    }
                    else
                    {
                        rutaArchivo = Path.Combine(ObtenerRutaBase(empresa), carpetaPadre.ruta, carpetaPadre.nombre, archivo.FileName);
                    }


                    if (File.Exists(rutaArchivo))
                    {
                        return listaDTOs;
                    }
                }

                foreach (var archivo in listaArchivos)
                {
                    nuevoDirectorio = new tblGA_Directorio();
                    nuevaVersionDirectorio = new tblGA_Version();
                    int nivel = carpetaPadre.directorio.nivel;
                    nuevoDirectorio.padreID = padreID;
                    nuevoDirectorio.nivel = ++nivel;
                    nuevoDirectorio.esCarpeta = false;
                    nuevoDirectorio.departamentoID = carpetaPadre.directorio.departamentoID;
                    SaveEntity(nuevoDirectorio, (int)BitacoraEnum.Directorio);

                    nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
                    nuevaVersionDirectorio.esActivo = true;
                    nuevaVersionDirectorio.version = 1;
                    nuevaVersionDirectorio.usuarioID = usuarioID;
                    nuevaVersionDirectorio.nombre = NormalizarNombreArchivo(archivo.FileName);

                    if (carpetaPadre.ruta == null)
                    {
                        nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.nombre);
                    }
                    else
                    {
                        nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre);
                    }

                    nuevaVersionDirectorio.fecha = DateTime.Now;
                    var ruta = Path.Combine(ObtenerRutaBase(empresa), nuevaVersionDirectorio.ruta, nuevaVersionDirectorio.nombre);

                    if (GlobalUtils.SaveArchivo(archivo, ruta))
                    {
                        _contextVersion.AddObject(nuevaVersionDirectorio);

                        var dto = new DirectorioDTO();
                        dto.id = nuevaVersionDirectorio.directorioID;
                        dto.value = nuevaVersionDirectorio.nombre;
                        dto.type = ObtenerExtensionArchivo(dto.value);
                        dto.date = nuevaVersionDirectorio.fecha.ToString(FORMATO_FECHA_HORA);
                        dto.pId = padreID;
                        listaDTOs.Add(dto);
                    }
                    else
                    {
                        return null;
                    }
                }
                SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            return listaDTOs;
        }

        public Dictionary<string, object> SubirFolder(DirectorioDTO directorio, List<HttpPostedFileBase> listaArchivos, int padreID, int usuarioID, int empresa)
        {
            var resultado = new Dictionary<string, object>();
            bool esAdmin = new UsuarioDAO().getViewAction(vSesiones.sesionCurrentView, "Administrar");
            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var carpetaPadre = context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(padreID) && x.esActivo);

                        // Verifica si la carpeta tiene archivos.
                        if (directorio.data.Count.Equals(0))
                        {
                            resultado.Add("exito", false);
                            resultado.Add("error", "La carpeta no contiene archivos");
                            return resultado;
                        }

                        // Verifica si la carpeta a subir ya existe en la ruta actual
                        string rutaArchivo;
                        if (carpetaPadre.ruta == null)
                        {
                            rutaArchivo = Path.Combine(ObtenerRutaBase(empresa), carpetaPadre.nombre, directorio.value.Trim());
                        }
                        else
                        {
                            rutaArchivo = Path.Combine(ObtenerRutaBase(empresa), carpetaPadre.ruta, carpetaPadre.nombre, directorio.value.Trim());
                        }
                        if (Directory.Exists(rutaArchivo))
                        {
                            resultado.Add("exito", false);
                            resultado.Add("error", "Ya existe una carpeta con ese nombre en esa ruta.");
                            return resultado;
                        }

                        // Creacion del directorio.
                        var nuevoDirectorio = new tblGA_Directorio();
                        nuevoDirectorio.departamentoID = carpetaPadre.directorio.departamentoID;
                        nuevoDirectorio.esCarpeta = true;
                        nuevoDirectorio.padreID = carpetaPadre.directorioID;
                        int nivel = carpetaPadre.directorio.nivel;
                        nuevoDirectorio.nivel = ++nivel;
                        context.tblGA_Directorio.Add(nuevoDirectorio);
                        context.SaveChanges();

                        // Creacion de la nueva version.
                        var nuevaVersionDirectorio = new tblGA_Version();
                        nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
                        nuevaVersionDirectorio.usuarioID = usuarioID;
                        nuevaVersionDirectorio.version = 1;
                        if (carpetaPadre.ruta == null)
                        {
                            nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.nombre);
                        }
                        else
                        {
                            nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre);

                            if (!esAdmin)
                            {
                                // Otorga los mismos permisos de la carpeta padre a la carpeta hija al usuario que creó la carpeta.
                                var permisosCarpetaHija = new tblGA_Permisos();
                                tblGA_Permisos permisosCarpetaPadre = context.tblGA_Permisos.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpetaPadre.directorioID));
                                if (permisosCarpetaPadre != null)
                                {
                                    permisosCarpetaHija.usuarioID = usuarioID;
                                    permisosCarpetaHija.directorioID = nuevaVersionDirectorio.directorioID;
                                    permisosCarpetaHija.puedeSubir = permisosCarpetaPadre.puedeSubir;
                                    permisosCarpetaHija.puedeEliminar = permisosCarpetaPadre.puedeEliminar;
                                    permisosCarpetaHija.puedeDescargarArchivo = permisosCarpetaPadre.puedeDescargarArchivo;
                                    permisosCarpetaHija.puedeDescargarCarpeta = permisosCarpetaPadre.puedeDescargarCarpeta;
                                    permisosCarpetaHija.puedeActualizar = permisosCarpetaPadre.puedeActualizar;
                                    permisosCarpetaHija.puedeCrear = permisosCarpetaPadre.puedeCrear;
                                    context.tblGA_Permisos.Add(permisosCarpetaHija);
                                }

                                // Se le agrega el permiso de vista de la carpeta al usuario que la creó.
                                tblGA_Vistas nuevaVista = new tblGA_Vistas();
                                nuevaVista.usuarioID = usuarioID;
                                nuevaVista.directorioID = nuevaVersionDirectorio.directorioID;
                                nuevaVista.estatusVista = 2;
                                context.tblGA_Vistas.Add(nuevaVista);
                            }
                        }
                        nuevaVersionDirectorio.nombre = NormalizarNombreCarpeta(directorio.value.Trim());
                        nuevaVersionDirectorio.fecha = DateTime.Now;
                        nuevaVersionDirectorio.esActivo = true;
                        context.tblGA_Version.Add(nuevaVersionDirectorio);
                        context.SaveChanges();

                        var ruta = Path.Combine(ObtenerRutaBase(empresa), nuevaVersionDirectorio.ruta, nuevaVersionDirectorio.nombre);
                        Directory.CreateDirectory(ruta);
                        if (Directory.Exists(ruta))
                        {
                            SubirSubarchivosFolder(directorio.data, listaArchivos, nuevaVersionDirectorio, usuarioID, empresa, context, esAdmin);
                        }
                        else
                        {
                            resultado.Add("exito", false);
                            resultado.Add("error", "Error, no se pudo crear la carpeta inicial");
                            dbContextTransaction.Rollback();
                            return resultado;
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        resultado.Add("exito", false);
                        resultado.Add("error", "Error: " + e.Message);
                        dbContextTransaction.Rollback();
                        return resultado;
                    }
                }
            }

            resultado.Add("exito", true);
            return resultado;
        }

        private void SubirSubarchivosFolder(List<DirectorioDTO> archivosHijos, List<HttpPostedFileBase> listaArchivos, tblGA_Version carpetaPadre, int usuarioID, int empresa, MainContext context, bool esAdmin)
        {
            foreach (var archivo in archivosHijos)
            {
                // Creacion del directorio.
                var nuevoDirectorio = new tblGA_Directorio();
                nuevoDirectorio.padreID = carpetaPadre.directorioID;
                int nivel = carpetaPadre.directorio.nivel;
                nuevoDirectorio.nivel = ++nivel;
                nuevoDirectorio.esCarpeta = (null != archivo.data);
                nuevoDirectorio.departamentoID = carpetaPadre.directorio.departamentoID;
                context.tblGA_Directorio.Add(nuevoDirectorio);
                context.SaveChanges();

                // Creacion de la version.
                var nuevaVersionDirectorio = new tblGA_Version();
                nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
                nuevaVersionDirectorio.usuarioID = usuarioID;
                nuevaVersionDirectorio.version = 1;
                nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre);
                nuevaVersionDirectorio.nombre = NormalizarNombreCarpeta(archivo.value.Trim());
                nuevaVersionDirectorio.fecha = DateTime.Now;
                nuevaVersionDirectorio.esActivo = true;
                context.tblGA_Version.Add(nuevaVersionDirectorio);
                context.SaveChanges();

                // Crea la carpeta o el archivo en disco.
                if (nuevoDirectorio.esCarpeta)
                {
                    if (!esAdmin)
                    {
                        // Si el archivo es una carpeta, otorga los mismos permisos de la carpeta padre a la carpeta hija al usuario que creó la carpeta.
                        var permisosCarpetaHija = new tblGA_Permisos();
                        tblGA_Permisos permisosCarpetaPadre = context.tblGA_Permisos.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpetaPadre.directorioID));
                        if (permisosCarpetaPadre != null)
                        {
                            permisosCarpetaHija.usuarioID = usuarioID;
                            permisosCarpetaHija.directorioID = nuevaVersionDirectorio.directorioID;
                            permisosCarpetaHija.puedeSubir = permisosCarpetaPadre.puedeSubir;
                            permisosCarpetaHija.puedeEliminar = permisosCarpetaPadre.puedeEliminar;
                            permisosCarpetaHija.puedeDescargarArchivo = permisosCarpetaPadre.puedeDescargarArchivo;
                            permisosCarpetaHija.puedeDescargarCarpeta = permisosCarpetaPadre.puedeDescargarCarpeta;
                            permisosCarpetaHija.puedeActualizar = permisosCarpetaPadre.puedeActualizar;
                            permisosCarpetaHija.puedeCrear = permisosCarpetaPadre.puedeCrear;
                            context.tblGA_Permisos.Add(permisosCarpetaHija);
                        }

                        // Se le agrega el permiso de vista de la carpeta al usuario que la creó.
                        tblGA_Vistas nuevaVista = new tblGA_Vistas();
                        nuevaVista.usuarioID = usuarioID;
                        nuevaVista.directorioID = nuevaVersionDirectorio.directorioID;
                        nuevaVista.estatusVista = 2;
                        context.tblGA_Vistas.Add(nuevaVista);

                        context.SaveChanges();
                    }
                    // Creación de la carpeta.
                    var ruta = Path.Combine(ObtenerRutaBase(empresa), nuevaVersionDirectorio.ruta, nuevaVersionDirectorio.nombre);
                    Directory.CreateDirectory(ruta);
                    SubirSubarchivosFolder(archivo.data, listaArchivos, nuevaVersionDirectorio, usuarioID, empresa, context, esAdmin);
                }
                else
                {
                    // Creación del archivo.
                    var ruta = Path.Combine(ObtenerRutaBase(empresa), nuevaVersionDirectorio.ruta, nuevaVersionDirectorio.nombre);
                    HttpPostedFileBase archivoPorCrear =
                        listaArchivos.FirstOrDefault(
                        x => x.FileName.Split('/').Last().Equals(archivo.value.Trim())
                        //&&
                        //x.FileName.Split('/')[x.FileName.Split('/').Length - 2].Equals(archivo.parent.Trim())
                        );
                    if (archivoPorCrear != null)
                    {
                        if (GlobalUtils.SaveArchivo(archivoPorCrear, ruta))
                        {
                            if (listaArchivos.Count > 0)
                            {
                                listaArchivos.Remove(archivoPorCrear);
                            }
                        }
                    }
                }
            }
        }

        public string ObtenerRutaArchivo(int id, bool esVersion, int empresa)
        {

            string ruta;
            tblGA_Version archivo;

            try
            {
                if (!esVersion)
                {
                    archivo = _context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(id) && x.esActivo);
                }
                else
                {
                    archivo = _context.tblGA_Version.FirstOrDefault(x => x.id.Equals(id));
                }

                ruta = Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, archivo.nombre);
            }

            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return "";
            }

            return ruta;
        }

        public string ObtenerRutaCarpetaComprimida(int folderID, int empresa)
        {
            string rutaCarpetaComprimida;
            tblGA_Version carpetaRaiz;
            try
            {
                int departamentoID = _context.tblGA_Version.Where(x => x.directorioID.Equals(folderID) && x.esActivo).Select(x => x.directorio.departamentoID).FirstOrDefault();

                if (departamentoID != 0)
                {
                    carpetaRaiz = _context.tblGA_Version.FirstOrDefault(x => x.directorio.departamentoID.Equals(departamentoID) && x.directorio.nivel.Equals(0));
                }
                else
                {
                    return "";
                }

                tblGA_Version carpeta = _context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(folderID) && x.esActivo);
                string rutaFolder;
                if (carpeta.ruta == null)
                {
                    rutaFolder = Path.Combine(ObtenerRutaBase(empresa), carpeta.nombre.Trim());
                }
                else
                {
                    rutaFolder = Path.Combine(ObtenerRutaBase(empresa), carpeta.ruta, carpeta.nombre.Trim());
                }

                var rutaCarpetaComprimidos = Path.Combine(ObtenerRutaBase(empresa), carpetaRaiz.nombre, NOMBRE_CARPETA_COMPRIMIDOS);
                var nombreZip = FormatearNombreCarpetaComprimida(carpeta.nombre);
                var rutaZip = Path.Combine(rutaCarpetaComprimidos, nombreZip);


                var archivoCompreso = GlobalUtils.ComprimirCarpeta(rutaFolder, rutaZip);
                rutaCarpetaComprimida = "";
                if (archivoCompreso)
                    rutaCarpetaComprimida = rutaZip;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
            if (!rutaCarpetaComprimida.Equals(""))
            {
                return rutaCarpetaComprimida;
            }
            else
            {
                return "";
            }
        }

        public DirectorioDTO ActualizarArchivo(HttpPostedFileBase archivo, int archivoID, int usuarioID, int empresa)
        {

            tblGA_Version versionNueva;
            tblGA_Version versionVieja;
            tblGA_Version carpetaRaiz;
            var dto = new DirectorioDTO();
            try
            {
                var _contextVersion = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Version>();


                int departamentoID = _context.tblGA_Version.Where(x => x.directorioID.Equals(archivoID) && x.esActivo).Select(x => x.directorio.departamentoID).FirstOrDefault();

                if (departamentoID != 0)
                {
                    carpetaRaiz = _context.tblGA_Version.FirstOrDefault(x => x.directorio.departamentoID.Equals(departamentoID) && x.directorio.nivel.Equals(0));
                }
                else
                {
                    return null;
                }

                versionVieja = _contextVersion.FirstOrDefault(x => x.directorioID.Equals(archivoID) && x.esActivo);
                versionVieja.esActivo = false;
                string nombreTemp = versionVieja.nombre;
                versionVieja.nombre = FormatearNombreArchivoActualizado(versionVieja.nombre);


                var rutaCarpetaActualizados = Path.Combine(ObtenerRutaBase(empresa), carpetaRaiz.nombre, NOMBRE_CARPETA_ACTUALIZADOS);

                File.Move(Path.Combine(ObtenerRutaBase(empresa), versionVieja.ruta, nombreTemp), Path.Combine(rutaCarpetaActualizados, versionVieja.nombre));

                int version = versionVieja.version;

                versionNueva = new tblGA_Version();
                versionNueva.directorioID = versionVieja.directorioID;
                versionNueva.ruta = versionVieja.ruta;
                versionNueva.version = ++version;
                versionNueva.usuarioID = usuarioID;
                versionNueva.esActivo = true;
                nombreTemp = CambiarExtensionArchivo(nombreTemp, archivo.FileName);
                versionNueva.nombre = nombreTemp.Trim();
                versionNueva.fecha = DateTime.Now;

                versionVieja.ruta = Path.Combine(carpetaRaiz.nombre, NOMBRE_CARPETA_ACTUALIZADOS);

                var ruta = Path.Combine(ObtenerRutaBase(empresa), versionNueva.ruta, versionNueva.nombre);

                if (GlobalUtils.SaveArchivo(archivo, ruta))
                {
                    _contextVersion.AddObject(versionNueva);
                    SaveChanges();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            dto.id = versionNueva.directorioID;
            dto.value = versionNueva.nombre;
            dto.type = ObtenerExtensionArchivo(dto.value);
            dto.date = versionNueva.fecha.ToString(FORMATO_FECHA_HORA);

            return dto;
        }

        public List<DirectorioDTO> ObtenerHistorialVersiones(int archivoID)
        {

            List<DirectorioDTO> listaVersiones =
                _context.tblGA_Version.Where(x => x.directorioID.Equals(archivoID)).OrderByDescending(x => x.version)
                .Select(x => new DirectorioDTO
                {
                    id = x.id,
                    value = x.nombre,
                    version = x.version,
                    usuario = x.usuario.nombre.Trim() + " " + x.usuario.apellidoPaterno.Trim() + " " + x.usuario.apellidoMaterno.Trim(),
                    date = x.fecha.ToString()
                }).ToList();

            foreach (var archivo in listaVersiones)
            {
                archivo.date = String.Format("{0}", archivo.date);
            }

            return listaVersiones;
        }

        public string RenombrarArchivo(string nuevoNombre, int archivoID, int empresa)
        {

            tblGA_Version archivo;

            try
            {
                archivo = _context.tblGA_Version.FirstOrDefault(x => x.directorioID.Equals(archivoID) && x.esActivo);

                if (archivo.nombre.Equals(nuevoNombre.Trim()) || nuevoNombre.Equals(String.Empty))
                {
                    return nuevoNombre;
                }

                if (archivo.directorio.esCarpeta)
                {
                    nuevoNombre = NormalizarNombreCarpeta(nuevoNombre).Trim();
                    Directory.Move(Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, archivo.nombre), Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, nuevoNombre));
                }
                else
                {
                    nuevoNombre = NormalizarNombreArchivo(nuevoNombre).Trim();
                    File.Move(Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, archivo.nombre), Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, nuevoNombre));
                }


                ActualizarRutasArchivosHijos(archivo, archivo.nombre, nuevoNombre);

                archivo.nombre = nuevoNombre.Trim();
                SaveChanges();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "e_r_r_o_r";
            }

            return archivo.nombre;
        }

        public bool EliminarArchivo(int id, int empresa)
        {

            var _contextVersion = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Version>();
            tblGA_Version archivo;
            tblGA_Version carpetaRaiz;
            try
            {

                int departamentoID = _context.tblGA_Version.Where(x => x.directorioID.Equals(id) && x.esActivo).Select(x => x.directorio.departamentoID).FirstOrDefault();

                if (departamentoID != 0)
                {
                    carpetaRaiz = _context.tblGA_Version.FirstOrDefault(x => x.directorio.departamentoID.Equals(departamentoID) && x.directorio.nivel.Equals(0));
                }
                else
                {
                    return false;
                }

                archivo = _contextVersion.Where(x => x.directorioID.Equals(id) && x.esActivo).FirstOrDefault();
                if (null != archivo)
                {
                    archivo.esActivo = false;
                    var nombreTemp = archivo.nombre;

                    var rutaCarpetaEliminados = Path.Combine(ObtenerRutaBase(empresa), carpetaRaiz.nombre, NOMBRE_CARPETA_ELIMINADOS);

                    if (archivo.directorio.esCarpeta)
                    {
                        archivo.nombre = FormatearNombreBorradoCarpeta(archivo.nombre).Trim();
                        Directory.Move(Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, nombreTemp), Path.Combine(rutaCarpetaEliminados, archivo.nombre));
                    }
                    else
                    {
                        archivo.nombre = FormatearNombreBorradoArchivo(archivo.nombre).Trim();
                        File.Move(Path.Combine(ObtenerRutaBase(empresa), archivo.ruta, nombreTemp), Path.Combine(rutaCarpetaEliminados, archivo.nombre));
                    }

                    archivo.ruta = Path.Combine(carpetaRaiz.nombre, NOMBRE_CARPETA_ELIMINADOS);

                    SaveChanges();
                }
                else
                {
                    return false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }


        //////////////[[[ Helpers ]]]//////////////

        private string FormatearNombreBorradoCarpeta(string nombre)
        {
            return nombre + " [Eliminado " + ObtenerEtiquetaTiempo() + "]";
        }

        private string FormatearNombreBorradoArchivo(string nombre)
        {
            return Path.GetFileNameWithoutExtension(nombre) + " [Eliminado " + ObtenerEtiquetaTiempo() + "]" + Path.GetExtension(nombre);
        }

        private string FormatearNombreArchivoActualizado(string nombre)
        {
            return Path.GetFileNameWithoutExtension(nombre) + " [Actualizado " + ObtenerEtiquetaTiempo() + "]" + Path.GetExtension(nombre);
        }

        private string FormatearNombreCarpetaComprimida(string nombre)
        {
            return nombre + " [Comprimida " + ObtenerEtiquetaTiempo() + "].zip";
        }

        private string ObtenerEtiquetaTiempo()
        {
            return DateTime.Now.ToString(FORMATO_FECHA_HORA).Replace(":", "-");
        }

        private string CambiarExtensionArchivo(string nombreArchivoExtensionVieja, string nombreArchivoExtensionNueva)
        {
            return Path.GetFileNameWithoutExtension(nombreArchivoExtensionVieja) + Path.GetExtension(nombreArchivoExtensionNueva);
        }

        private string ObtenerExtensionArchivo(string nombreArchivo)
        {
            return nombreArchivo.Split('.').LastOrDefault();
        }

        //Metodo para eliminar caracteres inválidos (En Windows) en archivos
        private string NormalizarNombreArchivo(string nombreArchivo)
        {
            string caracteresInvalidos = System.Text.RegularExpressions.Regex.Escape(
                 new string(System.IO.Path.GetInvalidFileNameChars())
            );
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", caracteresInvalidos);

            return System.Text.RegularExpressions.Regex.Replace(nombreArchivo, invalidRegStr, "_");
        }

        //Metodo para eliminar caracteres inválidos (En Windows) en carpetas
        private string NormalizarNombreCarpeta(string nombreCarpeta)
        {
            string caracteresInvalidos = System.Text.RegularExpressions.Regex.Escape(
                 new string(System.IO.Path.GetInvalidPathChars())
            );
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", caracteresInvalidos);

            return System.Text.RegularExpressions.Regex.Replace(nombreCarpeta, invalidRegStr, "_");
        }

        private string ObtenerRutaBase(int empresa)
        {
            string rutaBase;
            if (empresa.Equals(1))
            {
                rutaBase = RUTA_BASE_CONSTRUPLAN;
            }
            else
            {
                rutaBase = RUTA_BASE_ARRENDADORA;
            }
            return rutaBase;
        }

        private bool ActualizarRutasArchivosHijos(tblGA_Version archivo, string nombreViejo, string nombreNuevo)
        {
            List<tblGA_Version> archivosHijos = _context.tblGA_Version.Where(x => x.directorio.padreID.Equals(archivo.directorioID) && x.esActivo).ToList();

            foreach (var archivoHijo in archivosHijos)
            {
                archivoHijo.ruta = archivoHijo.ruta.Replace(nombreViejo, nombreNuevo);
                if (archivoHijo.directorio.esCarpeta)
                {
                    ActualizarRutasArchivosHijos(archivoHijo, nombreViejo, nombreNuevo);
                }
            }
            SaveChanges();

            return true;
        }

        private PermisosDTO ObtenerPermisosCarpeta(int carpetaID, int usuarioID)
        {
            PermisosDTO permisos = _context.tblGA_Permisos
            .Where(x => x.directorioID.Equals(carpetaID) && x.usuarioID.Equals(usuarioID))
            .Select(x => new PermisosDTO
            {
                usuarioID = x.usuarioID,
                directorioID = x.directorioID,
                puedeSubir = x.puedeSubir,
                puedeEliminar = x.puedeEliminar,
                puedeDescargarArchivo = x.puedeDescargarArchivo,
                puedeDescargarCarpeta = x.puedeDescargarCarpeta,
                puedeActualizar = x.puedeActualizar,
                puedeCrear = x.puedeCrear
            }).FirstOrDefault();
            if (permisos != null)
            {
                return permisos;
            }
            else
            {
                return new PermisosDTO();
            }
        }


        //////////////[[[ Permisos ]]]//////////////

        public List<ComboDTO> ObtenerUsuariosPorDepartamento(int usuarioID, int departamentoID)
        {

            var listaUsuarios = new List<ComboDTO>();

            try
            {

                if (departamentoID.Equals(0))
                {
                    tblP_Usuario usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(usuarioID));
                    listaUsuarios = _context.tblP_Usuario.Where(x => x.puesto.departamentoID.Equals(usuario.puesto.departamentoID))
                    .Select(x => new ComboDTO
                    {
                        Value = x.id.ToString(),
                        Text = x.nombre.Trim() + " " + x.apellidoPaterno.Trim() + " " + x.apellidoMaterno.Trim(),
                        Prefijo = ""
                    }).OrderBy(x => x.Text).ToList();
                }
                else
                {
                    listaUsuarios = _context.tblP_Usuario.Where(x => x.puesto.departamentoID.Equals(departamentoID))
                     .Select(x => new ComboDTO
                     {
                         Value = x.id.ToString(),
                         Text = x.nombre.Trim() + " " + x.apellidoPaterno.Trim() + " " + x.apellidoMaterno.Trim(),
                         Prefijo = ""
                     }).OrderBy(x => x.Text).ToList();
                }
            }

            catch (Exception e)
            {
                return new List<ComboDTO>();
            }

            return listaUsuarios;
        }

        public List<ComboDTO> ObtenerDepartamentos(int adminID)
        {

            var listaDepartamentos = new List<ComboDTO>();

            try
            {
                tblP_Usuario usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(adminID));

                listaDepartamentos = _context.tblP_Departamento.Where(x => x.id != usuario.puesto.departamentoID)
                .Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion.Trim(),
                    Prefijo = ""
                }).OrderBy(x => x.Text).ToList();
            }

            catch (Exception e)
            {
                return new List<ComboDTO>();
            }

            return listaDepartamentos;
        }

        public EstructuraVistasDTO ObtenerEstructuraPermisos(int usuarioID, int adminID)
        {

            var directorioTemp = new EstructuraVistasDTO();
            int departamentoID;
            try
            {
                departamentoID = _context.tblP_Usuario.Where(x => x.id.Equals(adminID)).Select(x => x.puesto.departamentoID).FirstOrDefault();
                tblGA_Version carpetaRaiz = _context.tblGA_Version.FirstOrDefault(x => x.directorio.departamentoID == departamentoID && x.esActivo);

                directorioTemp.permisos = ObtenerPermisosCarpeta(carpetaRaiz.directorioID, usuarioID);

                directorioTemp.title = carpetaRaiz.nombre;
                directorioTemp.folder = true;
                directorioTemp.id = carpetaRaiz.directorioID;
                directorioTemp.key = carpetaRaiz.directorioID;
                directorioTemp.expanded = true;

                int estatus = ObtenerEstatusVista(carpetaRaiz.directorioID, usuarioID);

                if (0.Equals(estatus)) //estatus 0 (No seleccionada ni parcialmente seleccionada)
                {
                    directorioTemp.selected = false;
                    directorioTemp.partsel = false;
                }
                else if (1.Equals(estatus)) //estatus 1 (Parcialmente seleccionada)
                {
                    directorioTemp.selected = false;
                    directorioTemp.partsel = true;
                }
                else //estatus 2 (Seleccioanda y parcialmente seleccionada)
                {
                    directorioTemp.selected = true;
                    directorioTemp.partsel = true;
                }

                directorioTemp.children = new List<EstructuraVistasDTO>();
                directorioTemp.children = ObtenerEstructuraSubpermisos(directorioTemp.children, directorioTemp.id, usuarioID);

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new EstructuraVistasDTO();
            }

            return directorioTemp;
        }

        private List<EstructuraVistasDTO> ObtenerEstructuraSubpermisos(List<EstructuraVistasDTO> subarchivos, int carpetaPadreID, int usuarioID)
        {

            try
            {
                List<tblGA_Version> carpetasHijas = _context.tblGA_Version.Where(x => x.directorio.padreID.Equals(carpetaPadreID) && x.esActivo).ToList();
                foreach (var carpetas in carpetasHijas)
                {
                    var directorioTemp = new EstructuraVistasDTO();
                    if (carpetas.directorio.esCarpeta)
                    {

                        directorioTemp.permisos = ObtenerPermisosCarpeta(carpetas.directorioID, usuarioID);

                        directorioTemp.folder = true;
                        directorioTemp.title = carpetas.nombre;
                        directorioTemp.id = carpetas.directorioID;
                        directorioTemp.key = carpetas.directorioID;
                        directorioTemp.expanded = true;

                        int estatus = ObtenerEstatusVista(carpetas.directorioID, usuarioID);

                        if (0.Equals(estatus)) //estatus 0 (No seleccionada ni parcialmente seleccionada)
                        {
                            directorioTemp.selected = false;
                            directorioTemp.partsel = false;

                        }
                        else if (1.Equals(estatus)) //estatus 1 (Parcialmente seleccionada)
                        {
                            directorioTemp.selected = false;
                            directorioTemp.partsel = true;
                        }
                        else //estatus 2 (Seleccioanda y parcialmente seleccionada)
                        {
                            directorioTemp.selected = true;
                            directorioTemp.partsel = true;
                        }

                        directorioTemp.children = new List<EstructuraVistasDTO>();
                        directorioTemp.children = ObtenerEstructuraSubpermisos(directorioTemp.children, directorioTemp.id, usuarioID);
                        subarchivos.Add(directorioTemp);
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<EstructuraVistasDTO>();
            }

            return subarchivos;
        }

        private int ObtenerEstatusVista(int folderID, int usuarioID)
        {

            tblGA_Vistas vista;
            try
            {
                vista = _context.tblGA_Vistas.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(folderID));
                if (null != vista)
                {
                    return vista.estatusVista;
                }
                else
                {
                    return 0;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public bool GuardarVistasAccionesUsuario(List<EstructuraVistasDTO> carpetas, int usuarioID, int usuarioAdminID)
        {

            var _contextVistas = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Vistas>();
            var _contextPermisos = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Permisos>();
            var _contextAccesoDepartamento = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_AccesoDepartamento>();

            var carpetasDescartadas = new List<EstructuraVistasDTO>();

            try
            {
                foreach (var carpeta in carpetas)
                {

                    //Verificar si ya existe registro de la vista para ese usuario, y si si hay, la actualiza.
                    tblGA_Vistas vista = _contextVistas.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpeta.key));
                    if (vista != null)
                    {

                        if (!carpeta.partsel && !carpeta.selected)
                        {
                            vista.estatusVista = 0;
                        }
                        else if (carpeta.partsel && !carpeta.selected)
                        {
                            vista.estatusVista = 1;
                        }
                        else if (carpeta.partsel && carpeta.selected)
                        {
                            vista.estatusVista = 2;
                        }

                        if (vista.estatusVista != 0)
                        {
                            tblGA_Permisos permisos = _contextPermisos.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpeta.key));
                            if (permisos != null)
                            {
                                permisos.puedeSubir = carpeta.permisos.puedeSubir;
                                permisos.puedeEliminar = carpeta.permisos.puedeEliminar;
                                permisos.puedeDescargarArchivo = carpeta.permisos.puedeDescargarArchivo;
                                permisos.puedeDescargarCarpeta = carpeta.permisos.puedeDescargarCarpeta;
                                permisos.puedeActualizar = carpeta.permisos.puedeActualizar;
                                permisos.puedeCrear = carpeta.permisos.puedeCrear;
                            }
                            else
                            {
                                tblGA_Permisos permiso = new tblGA_Permisos();
                                permiso.usuarioID = usuarioID;
                                permiso.directorioID = carpeta.key;
                                permiso.puedeSubir = carpeta.permisos.puedeSubir;
                                permiso.puedeEliminar = carpeta.permisos.puedeEliminar;
                                permiso.puedeDescargarArchivo = carpeta.permisos.puedeDescargarArchivo;
                                permiso.puedeDescargarCarpeta = carpeta.permisos.puedeDescargarCarpeta;
                                permiso.puedeActualizar = carpeta.permisos.puedeActualizar;
                                permiso.puedeCrear = carpeta.permisos.puedeCrear;
                                _contextPermisos.AddObject(permiso);
                            }
                        }
                        carpetasDescartadas.Add(carpeta);
                    }

                }

                if (carpetasDescartadas.Count > 0)
                {
                    carpetas = carpetas.Except(carpetasDescartadas).ToList();
                }

                //Guarda el registro de la vista
                foreach (var carpeta in carpetas)
                {
                    var vista = new tblGA_Vistas();
                    vista.usuarioID = usuarioID;
                    vista.directorioID = carpeta.key;

                    if (!carpeta.partsel && !carpeta.selected)
                    {
                        vista.estatusVista = 0;
                    }
                    else if (carpeta.partsel && !carpeta.selected)
                    {
                        vista.estatusVista = 1;
                    }
                    else if (carpeta.partsel && carpeta.selected)
                    {
                        vista.estatusVista = 2;
                    }

                    if (vista.estatusVista != 0)
                    {
                        var permiso = new tblGA_Permisos();
                        permiso.usuarioID = usuarioID;
                        permiso.directorioID = carpeta.key;
                        permiso.puedeSubir = carpeta.permisos.puedeSubir;
                        permiso.puedeEliminar = carpeta.permisos.puedeEliminar;
                        permiso.puedeDescargarArchivo = carpeta.permisos.puedeDescargarArchivo;
                        permiso.puedeDescargarCarpeta = carpeta.permisos.puedeDescargarCarpeta;
                        permiso.puedeActualizar = carpeta.permisos.puedeActualizar;
                        permiso.puedeCrear = carpeta.permisos.puedeCrear;
                        _contextPermisos.AddObject(permiso);
                    }

                    _contextVistas.AddObject(vista);
                }

                int departamentoID = _context.tblP_Usuario.Where(x => x.id.Equals(usuarioAdminID)).Select(x => x.puesto.departamentoID).FirstOrDefault();
                tblGA_AccesoDepartamento acceso = _contextAccesoDepartamento.FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.departamentoID.Equals(departamentoID));

                if (acceso == null)
                {
                    var accesoDepartamento = new tblGA_AccesoDepartamento();
                    accesoDepartamento.usuarioID = usuarioID;
                    accesoDepartamento.departamentoID = departamentoID;
                    _contextAccesoDepartamento.AddObject(accesoDepartamento);
                }

                SaveChanges();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }


        #region Cargado de carpetas manuales
        ////////////////////////////////////Cargo de carpetas manuales///////////////////////////////////////////////////////
        private bool cargarCarpetaRaizManualmente(string PathCarpetaRaiz, int departamentoID, int usuarioID)
        {

            using (var context = new MainContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        DirectoryInfo directorioRaiz = new DirectoryInfo(PathCarpetaRaiz);
                        tblGA_Directorio nuevoDirectorio = new tblGA_Directorio();
                        tblGA_Version nuevaVersionDirectorio = new tblGA_Version();

                        nuevoDirectorio.padreID = 0;
                        nuevoDirectorio.nivel = 0;
                        nuevoDirectorio.esCarpeta = true;
                        nuevoDirectorio.departamentoID = departamentoID;
                        context.tblGA_Directorio.Add(nuevoDirectorio);
                        context.SaveChanges();

                        nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
                        nuevaVersionDirectorio.usuarioID = usuarioID;
                        nuevaVersionDirectorio.version = 1;
                        nuevaVersionDirectorio.ruta = null;
                        nuevaVersionDirectorio.nombre = directorioRaiz.Name;
                        nuevaVersionDirectorio.fecha = directorioRaiz.CreationTime;
                        nuevaVersionDirectorio.esActivo = true;
                        context.tblGA_Version.Add(nuevaVersionDirectorio);
                        context.SaveChanges();

                        string[] listaPathsHijos = Directory.GetFileSystemEntries(PathCarpetaRaiz);
                        for (int i = 0; i < listaPathsHijos.Length; i++)
                        {
                            cagarCarpetasHijas(new DirectoryInfo(listaPathsHijos[i]), nuevaVersionDirectorio, context);
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return true;
        }
        ///////////////////////////////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////////////////////////////
        private void cagarCarpetasHijas(DirectoryInfo archivo, tblGA_Version carpetaPadre, dynamic context)
        {

            Console.WriteLine("{0} fue creado el {1:D}. Tipo: {3}.Carpeta Padre: ---- {2}", archivo.Name, archivo.CreationTime, carpetaPadre, archivo.Attributes.ToString());

            tblGA_Directorio nuevoDirectorio = new tblGA_Directorio();
            tblGA_Version nuevaVersionDirectorio = new tblGA_Version();
            nuevoDirectorio.padreID = carpetaPadre.directorioID;
            int nivel = carpetaPadre.directorio.nivel;
            nuevoDirectorio.nivel = ++nivel;

            if ((archivo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                nuevoDirectorio.esCarpeta = true;
            }
            else
            {
                nuevoDirectorio.esCarpeta = false;
            }

            nuevoDirectorio.departamentoID = carpetaPadre.directorio.departamentoID;
            context.tblGA_Directorio.Add(nuevoDirectorio);
            context.SaveChanges();

            nuevaVersionDirectorio.directorioID = nuevoDirectorio.id;
            nuevaVersionDirectorio.usuarioID = carpetaPadre.usuarioID;
            nuevaVersionDirectorio.version = 1;

            if (carpetaPadre.ruta == null)
            {
                nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.nombre);
            }
            else
            {
                nuevaVersionDirectorio.ruta = Path.Combine(carpetaPadre.ruta, carpetaPadre.nombre);
            }
            nuevaVersionDirectorio.nombre = archivo.Name;
            nuevaVersionDirectorio.fecha = archivo.CreationTime;
            nuevaVersionDirectorio.esActivo = true;
            context.tblGA_Version.Add(nuevaVersionDirectorio);
            context.SaveChanges();

            //Si es carpeta
            if ((archivo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                FileSystemInfo[] archivosHijos = archivo.GetFileSystemInfos();
                if (archivosHijos.Length > 0)
                {
                    for (int i = 0; i < archivosHijos.Length; i++)
                    {
                        cagarCarpetasHijas(new DirectoryInfo(archivosHijos[i].FullName), nuevaVersionDirectorio, context);
                    }
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}

// #region Código cambiado / pendiente
//private List<DirectorioDTO> obtenerEstructuraSubcarpetasConPermiso(List<DirectorioDTO> subarchivos, int carpetaPadreID, ref int index, int usuarioID, PermisosDTO permisos)
//{

//    try
//    {
//        List<tblGA_Version> archivosHijos = _context.tblGA_Version.Where(x => x.directorio.padreID.Equals(carpetaPadreID) && x.esActivo).ToList();

//        foreach (var archivo in archivosHijos)
//        {

//            DirectorioDTO directorioTemp = new DirectorioDTO();

//            //Verifica si tiene acceso a la carpeta
//            if (archivo.directorio.esCarpeta)
//            {
//                tblGA_Vistas vista = _context.tblGA_Vistas
//                    .FirstOrDefault(x => x.directorioID.Equals(archivo.directorioID) && x.estatusVista != 0 && x.usuarioID.Equals(usuarioID));
//                if (vista == null)
//                {
//                    continue;
//                }
//            }

//            directorioTemp.index = index++;
//            directorioTemp.value = archivo.nombre;
//            directorioTemp.date = archivo.fecha.ToString(FORMATO_FECHA_HORA);
//            directorioTemp.id = archivo.directorioID;
//            directorioTemp.userID = archivo.usuarioID;
//            directorioTemp.pId = archivo.directorio.padreID;
//            directorioTemp.data = new List<DirectorioDTO>();

//            if (archivo.directorio.esCarpeta)
//            {
//                directorioTemp.type = TIPO_DEFAULT_CARPETA;
//                directorioTemp.permisos = permisos;
//                directorioTemp.open = true;
//                directorioTemp.data = obtenerEstructuraSubcarpetasConPermiso(directorioTemp.data, directorioTemp.id, ref index, usuarioID, permisos);
//            }
//            else
//            {
//                directorioTemp.type = this.obtenerExtensionArchivo(archivo.nombre);
//            }

//            subarchivos.Add(directorioTemp);
//        }
//    }

//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//        return new List<DirectorioDTO>();
//    }

//    return subarchivos;
//}

//Verifica si es carpeta base, y carga sus permisos.
//if (archivo.directorio.esCarpeta && archivo.directorio.nivel.Equals(1))
//{
//    PermisosDTO permiso = _context.tblGA_Permisos
//        .Where(x => x.directorioID.Equals(archivo.directorioID) && x.usuarioID.Equals(usuarioID))
//        .Select(x => new PermisosDTO
//        {
//            puedeSubir = x.puedeSubir,
//            puedeEliminar = x.puedeEliminar,
//            puedeDescargar = x.puedeDescargar,
//            puedeActualizar = x.puedeActualizar,
//            puedeCrear = x.puedeCrear
//        }).FirstOrDefault();

//    if (permiso != null)
//    {
//        tienePermisoCarpetaBase = true;
//        directorioTemp.permisos = permiso;
//    }
//    else
//    {
//        tienePermisoCarpetaBase = false;
//        directorioTemp.permisos = new PermisosDTO();
//    }
//}


//tblGA_Vistas accesoCarpetaDepartamento = _context.tblGA_Vistas
//    .FirstOrDefault(x => x.directorioID.Equals(carpetaRaizDepartamento.directorioID) && x.usuarioID.Equals(usuarioID) && x.estatusVista != 0);

//DirectorioDTO carpetaRaiz = new DirectorioDTO();
//carpetaRaiz.data = new List<DirectorioDTO>();


//public PermisosDTO obtenerPermisosUsuarioPorCarpeta(int carpetaID, int usuarioID)
//{
//    var _contextPermisos = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Permisos>();
//    PermisosDTO acciones;
//    try
//    {
//        acciones = _contextPermisos.Select(x => new PermisosDTO
//        {
//            usuarioID = x.usuarioID,
//            directorioID = x.directorioID,
//            puedeActualizar = x.puedeActualizar,
//            puedeCrear = x.puedeCrear,
//            puedeDescargar = x.puedeDescargar,
//            puedeEliminar = x.puedeEliminar,
//            puedeSubir = x.puedeSubir,
//        }).FirstOrDefault(x => x.usuarioID.Equals(usuarioID) && x.directorioID.Equals(carpetaID));
//        if (acciones == null)
//        {
//            return new PermisosDTO();
//        }
//    }
//    catch (System.Exception)
//    {
//        return new PermisosDTO();
//    }

//    return acciones;
//}

//public bool guardarAccionesUsuario(PermisosDTO acciones, int usuarioID)
//{

//    var _contextAcciones = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblGA_Permisos>();

//    try
//    {
//        //Verificar si ya existe registro de acciones para ese usuario, y si si hay, lo actualiza.
//        tblGA_Permisos accion = _contextAcciones.FirstOrDefault(x => x.usuarioID.Equals(usuarioID));
//        if (accion != null)
//        {
//            accion.puedeSubir = acciones.puedeSubir;
//            accion.puedeEliminar = acciones.puedeEliminar;
//            accion.puedeDescargar = acciones.puedeDescargar;
//            accion.puedeActualizar = acciones.puedeActualizar;
//            accion.puedeCrear = acciones.puedeCrear;
//            SaveChanges();
//        }
//        else
//        {
//            //Guarda las acciones
//            tblGA_Permisos nuevaAccion = new tblGA_Permisos();
//            nuevaAccion.usuarioID = usuarioID;
//            nuevaAccion.puedeSubir = acciones.puedeSubir;
//            nuevaAccion.puedeEliminar = acciones.puedeEliminar;
//            nuevaAccion.puedeDescargar = acciones.puedeDescargar;
//            nuevaAccion.puedeActualizar = acciones.puedeActualizar;
//            nuevaAccion.puedeCrear = acciones.puedeCrear;
//            _contextAcciones.AddObject(nuevaAccion);
//            SaveChanges();
//        }

//    }

//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//        return false;
//    }

//    return true;
//}
// #endregion