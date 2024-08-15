using Core.DAO.Administracion.Seguridad.SaludOcupacional;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.SaludOcupacional;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Seguridad.SaludOcupacional;
using Core.Entity.Principal.Usuarios;
using Core.Enum.Administracion.Seguridad.SaludOcupacional;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Administracion.Seguridad.SaludOcupacional
{
    public class SaludOcupacionalDAO : GenericDAO<tblP_Usuario>, ISaludOcupacionalDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private string NombreControlador = "SaludOcupacionalController";
        private readonly string RutaAtencionMedica;
        private readonly string RutaHistorialClinico;
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\SEGURIDAD_SALUD_OCUPACIONAL";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\SEGURIDAD_SALUD_OCUPACIONAL";

        public SaludOcupacionalDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaAtencionMedica = Path.Combine(RutaBase, "ATENCION_MEDICA");
            RutaHistorialClinico = Path.Combine(RutaBase, "HISTORIAL_CLINICO");
        }

        #region Atención Médica
        public Dictionary<string, object> CargarInformacionEmpleado(int claveEmpleado)
        {
            try
            {
                resultado.Add("data", getInformacionEmpleado(claveEmpleado));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "CargarInformacionEmpleado", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        private EmpleadoDTO getInformacionEmpleado(int claveEmpleado)
        {
            var empleado = new EmpleadoDTO();

//            var odbc = new OdbcConsultaDTO()
//            {
//                consulta = @"
//                    SELECT
//                        emp.clave_empleado AS claveEmpleado,
//                        (emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno) AS nombre,
//                        emp.curp,
//                        emp.fecha_alta AS fechaIngreso,
//                        '' AS fechaIngresoString,
//                        emp.puesto,
//                        pue.descripcion AS puestoDesc,
//                        emp.fecha_nac AS fechaNacimiento,
//                        0 AS edad,
//                        emp.jefe_inmediato AS supervisor,
//                        (sup.nombre + ' ' + sup.ape_paterno + ' ' + sup.ape_materno) AS supervisorDesc,
//                        emp.cc_contable AS cc,
//                        c.descripcion AS ccDesc,
//                        CONVERT(int, emp.clave_depto) AS area,
//                        dep.desc_depto AS areaDesc
//                    FROM sn_empleados emp
//                        LEFT JOIN si_puestos pue ON emp.puesto = pue.puesto
//                        LEFT JOIN sn_departamentos dep ON emp.clave_depto = dep.clave_depto
//                        LEFT JOIN sn_empleados sup ON emp.jefe_inmediato = sup.clave_empleado
//                        LEFT JOIN cc c ON emp.cc_contable = c.cc
//                    WHERE emp.clave_empleado = ?",
//                parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = claveEmpleado } }
//            };
//            var empleadoConstruplan = _contextEnkontrol.Select<EmpleadoDTO>(EnkontrolEnum.CplanRh, odbc);
//            var empleadoArrendadora = _contextEnkontrol.Select<EmpleadoDTO>(EnkontrolEnum.ArrenRh, odbc);

            var empleadoConstruplan = _context.Select<EmpleadoDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT
                                emp.clave_empleado AS claveEmpleado,
                                (emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno) AS nombre,
                                emp.curp,
                                emp.fecha_alta AS fechaIngreso,
                                '' AS fechaIngresoString,
                                emp.puesto,
                                pue.descripcion AS puestoDesc,
                                emp.fecha_nac AS fechaNacimiento,
                                0 AS edad,
                                emp.jefe_inmediato AS supervisor,
                                (sup.nombre + ' ' + sup.ape_paterno + ' ' + sup.ape_materno) AS supervisorDesc,
                                emp.cc_contable AS cc,
                                c.descripcion AS ccDesc,
                                CONVERT(int, emp.clave_depto) AS area,
                                dep.desc_depto AS areaDesc
                            FROM tblRH_EK_Empleados emp
                                LEFT JOIN tblRH_EK_Puestos pue ON emp.puesto = pue.puesto
                                LEFT JOIN tblRH_EK_Departamentos dep ON emp.clave_depto = dep.clave_depto
                                LEFT JOIN tblRH_EK_Empleados sup ON emp.jefe_inmediato = sup.clave_empleado
                                LEFT JOIN tblP_CC c ON emp.cc_contable = c.cc
                            WHERE emp.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            });

            var empleadoArrendadora = _context.Select<EmpleadoDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT
                                emp.clave_empleado AS claveEmpleado,
                                (emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno) AS nombre,
                                emp.curp,
                                emp.fecha_alta AS fechaIngreso,
                                '' AS fechaIngresoString,
                                emp.puesto,
                                pue.descripcion AS puestoDesc,
                                emp.fecha_nac AS fechaNacimiento,
                                0 AS edad,
                                emp.jefe_inmediato AS supervisor,
                                (sup.nombre + ' ' + sup.ape_paterno + ' ' + sup.ape_materno) AS supervisorDesc,
                                emp.cc_contable AS cc,
                                c.descripcion AS ccDesc,
                                CONVERT(int, emp.clave_depto) AS area,
                                dep.desc_depto AS areaDesc
                            FROM tblRH_EK_Empleados emp
                                LEFT JOIN tblRH_EK_Puestos pue ON emp.puesto = pue.puesto
                                LEFT JOIN tblRH_EK_Departamentos dep ON emp.clave_depto = dep.clave_depto
                                LEFT JOIN tblRH_EK_Empleados sup ON emp.jefe_inmediato = sup.clave_empleado
                                LEFT JOIN tblP_CC c ON emp.cc_contable = c.cc
                            WHERE emp.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado }
            });

            if (empleadoConstruplan.Count() > 0)
            {
                empleadoConstruplan[0].fechaIngresoString = empleadoConstruplan[0].fechaIngreso.ToShortDateString();
                empleadoConstruplan[0].edad = calcularEdad(empleadoConstruplan[0].fechaNacimiento);

                empleado = empleadoConstruplan[0];
            }
            else if (empleadoArrendadora.Count() > 0)
            {
                empleadoArrendadora[0].fechaIngresoString = empleadoArrendadora[0].fechaIngreso.ToShortDateString();
                empleadoArrendadora[0].edad = calcularEdad(empleadoArrendadora[0].fechaNacimiento);

                empleado = empleadoArrendadora[0];
            }
            else
            {
                throw new Exception("No se encuentra la información del empleado.");
            }

            return empleado;
        }

        int calcularEdad(DateTime fechaNacimiento)
        {
            // Calculate the age.
            var age = DateTime.Today.Year - fechaNacimiento.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public Dictionary<string, object> GuardarNuevaAtencionMedica(tblS_SO_AtencionMedica atencionMedica, tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var listaFormatosValidos = new List<string> { ".JPG", ".JPEG", ".PNG", ".PDF" };

                    if (archivoST7 != null)
                    {
                        var extensionST7 = Path.GetExtension(archivoST7.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST7))
                        {
                            throw new Exception("Archivo ST7 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }

                    if (archivoST2 != null)
                    {
                        var extensionST2 = Path.GetExtension(archivoST2.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST2))
                        {
                            throw new Exception("Archivo ST2 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }
                    #endregion

                    atencionMedica.fecha = DateTime.Now;
                    atencionMedica.rutaArchivoST7 = null;
                    atencionMedica.rutaArchivoST2 = null;
                    atencionMedica.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    atencionMedica.fechaCreacion = DateTime.Now;
                    atencionMedica.usuarioModificacion_id = 0;
                    atencionMedica.fechaModificacion = null;
                    atencionMedica.registroActivo = true;

                    if (archivoST7 != null || archivoST2 != null)
                    {
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        var rutaCarpetaEmpleado = Path.Combine(RutaAtencionMedica, atencionMedica.clave_empleado.ToString());

                        verificarExisteCarpeta(rutaCarpetaEmpleado, true);

                        if (archivoST7 != null)
                        {
                            var rutaArchivoST7 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST7_", archivoST7.FileName)));
                            atencionMedica.rutaArchivoST7 = rutaArchivoST7;
                            listaRutaArchivos.Add(Tuple.Create(archivoST7, rutaArchivoST7));
                        }

                        if (archivoST2 != null)
                        {
                            var rutaArchivoST2 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST2_", archivoST2.FileName)));
                            atencionMedica.rutaArchivoST2 = rutaArchivoST2;
                            listaRutaArchivos.Add(Tuple.Create(archivoST2, rutaArchivoST2));
                        }

                        foreach (var archivo in listaRutaArchivos)
                        {
                            var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(archivo.Item1, archivo.Item2);

                            if (guardarArchivo.Item1 == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                LogError(0, 0, NombreControlador, "GuardarNuevaAtencionMedica_GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { nombreArchivo = archivo.Item2 });
                                return resultado;
                            }
                        }
                    }

                    _context.tblS_SO_AtencionMedica.Add(atencionMedica);
                    _context.SaveChanges();

                    revision.consecutivo = 1;
                    revision.fecha = DateTime.Now;
                    revision.atencionMedica_id = atencionMedica.id;
                    revision.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    revision.fechaCreacion = DateTime.Now;
                    revision.usuarioModificacion_id = 0;
                    revision.fechaModificacion = null;
                    revision.registroActivo = true;

                    _context.tblS_SO_AtencionMedica_Revision.Add(revision);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, atencionMedica.id, JsonUtils.convertNetObjectToJson(new { atencionMedica = atencionMedica, revision = revision }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevaAtencionMedica", e, AccionEnum.AGREGAR, 0, new { atencionMedica = atencionMedica, revision = revision });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevaRevision(tblS_SO_AtencionMedica_Revision revision, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var listaFormatosValidos = new List<string> { ".JPG", ".JPEG", ".PNG", ".PDF" };

                    if (archivoST7 != null)
                    {
                        var extensionST7 = Path.GetExtension(archivoST7.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST7))
                        {
                            throw new Exception("Archivo ST7 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }

                    if (archivoST2 != null)
                    {
                        var extensionST2 = Path.GetExtension(archivoST2.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST2))
                        {
                            throw new Exception("Archivo ST2 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }
                    #endregion

                    var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.id == revision.atencionMedica_id);

                    if (atencionMedicaSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra la información de la atención médica.");
                    }

                    var listaRevisionesPasadas = _context.tblS_SO_AtencionMedica_Revision.Where(x => x.registroActivo && x.atencionMedica_id == atencionMedicaSIGOPLAN.id).ToList();

                    if (archivoST7 != null || archivoST2 != null)
                    {
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        var rutaCarpetaEmpleado = Path.Combine(RutaAtencionMedica, atencionMedicaSIGOPLAN.clave_empleado.ToString());

                        verificarExisteCarpeta(rutaCarpetaEmpleado, true);

                        if (archivoST7 != null)
                        {
                            var rutaArchivoST7 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST7_", archivoST7.FileName)));
                            atencionMedicaSIGOPLAN.rutaArchivoST7 = rutaArchivoST7;
                            listaRutaArchivos.Add(Tuple.Create(archivoST7, rutaArchivoST7));
                        }

                        if (archivoST2 != null)
                        {
                            var rutaArchivoST2 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST2_", archivoST2.FileName)));
                            atencionMedicaSIGOPLAN.rutaArchivoST2 = rutaArchivoST2;
                            listaRutaArchivos.Add(Tuple.Create(archivoST2, rutaArchivoST2));
                        }

                        foreach (var archivo in listaRutaArchivos)
                        {
                            var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(archivo.Item1, archivo.Item2);

                            if (guardarArchivo.Item1 == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                LogError(0, 0, NombreControlador, "GuardarNuevaAtencionMedica_GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { nombreArchivo = archivo.Item2 });
                                return resultado;
                            }
                        }
                    }

                    revision.consecutivo = listaRevisionesPasadas.Count() + 1;
                    revision.fecha = DateTime.Now;
                    revision.atencionMedica_id = atencionMedicaSIGOPLAN.id;
                    revision.usuarioCreacion_id = vSesiones.sesionUsuarioDTO.id;
                    revision.fechaCreacion = DateTime.Now;
                    revision.usuarioModificacion_id = 0;
                    revision.fechaModificacion = null;
                    revision.registroActivo = true;

                    _context.tblS_SO_AtencionMedica_Revision.Add(revision);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, revision.id, JsonUtils.convertNetObjectToJson(new { revision = revision }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarNuevaRevision", e, AccionEnum.AGREGAR, 0, new { revision = revision });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarArchivosST7ST2(int atencionMedica_id, HttpPostedFileBase archivoST7, HttpPostedFileBase archivoST2)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var listaFormatosValidos = new List<string> { ".JPG", ".JPEG", ".PNG", ".PDF" };

                    if (archivoST7 != null)
                    {
                        var extensionST7 = Path.GetExtension(archivoST7.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST7))
                        {
                            throw new Exception("Archivo ST7 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }

                    if (archivoST2 != null)
                    {
                        var extensionST2 = Path.GetExtension(archivoST2.FileName).ToUpper();

                        if (!listaFormatosValidos.Contains(extensionST2))
                        {
                            throw new Exception("Archivo ST2 con formato inválido. Sólo se permiten los siguientes formatos: \".jpg\", \".jpeg\", \".png\", \".pdf\".");
                        }
                    }
                    #endregion

                    var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.id == atencionMedica_id);

                    if (atencionMedicaSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra la información de la atención médica.");
                    }

                    if (archivoST7 != null || archivoST2 != null)
                    {
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        var rutaCarpetaEmpleado = Path.Combine(RutaAtencionMedica, atencionMedicaSIGOPLAN.clave_empleado.ToString());

                        verificarExisteCarpeta(rutaCarpetaEmpleado, true);

                        if (archivoST7 != null)
                        {
                            var rutaArchivoST7 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST7_", archivoST7.FileName)));
                            atencionMedicaSIGOPLAN.rutaArchivoST7 = rutaArchivoST7;
                            listaRutaArchivos.Add(Tuple.Create(archivoST7, rutaArchivoST7));
                        }

                        if (archivoST2 != null)
                        {
                            var rutaArchivoST2 = ObtenerRutaArchivo(Path.Combine(rutaCarpetaEmpleado, ObtenerFormatoNombreArchivo("ST2_", archivoST2.FileName)));
                            atencionMedicaSIGOPLAN.rutaArchivoST2 = rutaArchivoST2;
                            listaRutaArchivos.Add(Tuple.Create(archivoST2, rutaArchivoST2));
                        }

                        foreach (var archivo in listaRutaArchivos)
                        {
                            var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(archivo.Item1, archivo.Item2);

                            if (guardarArchivo.Item1 == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                LogError(0, 0, NombreControlador, "GuardarNuevaAtencionMedica_GuardarArchivo", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { nombreArchivo = archivo.Item2 });
                                return resultado;
                            }
                        }
                    }

                    dbContextTransaction.Commit();
                    SaveBitacora(3, (int)AccionEnum.AGREGAR, atencionMedicaSIGOPLAN.id, JsonUtils.convertNetObjectToJson(new { atencionMedica_id = atencionMedica_id }));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "GuardarArchivosST7ST2", e, AccionEnum.AGREGAR, 0, atencionMedica_id);
                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        private static bool verificarExisteCarpeta(string path, bool crear = false)
        {
            bool existe = false;
            try
            {
                existe = Directory.Exists(path);
                if (!existe && crear)
                {
                    Directory.CreateDirectory(path);
                    existe = true;
                }
            }
            catch (Exception e)
            {
                existe = false;
            }

            return existe;
        }

        private string ObtenerRutaArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                int count = 1;

                string fileNameOnly = Path.GetFileNameWithoutExtension(ruta);
                string extension = Path.GetExtension(ruta);
                string path = Path.GetDirectoryName(ruta);
                string newFullPath = ruta;

                while (File.Exists(newFullPath))
                {
                    string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                    newFullPath = Path.Combine(path, tempFileName + extension);
                }

                ruta = newFullPath;
            }

            return ruta;
        }

        public Dictionary<string, object> CargarAtencionesMedicas(int claveEmpleado)
        {
            try
            {
                var hoy = DateTime.Now;
                  //var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();
                var listaEmpleados = _context.tblRH_EK_Empleados.Where(x => x.esActivo).ToList();
                var listaAtencionesMedicas = _context.tblS_SO_AtencionMedica.Where(x => x.registroActivo && (claveEmpleado > 0 ? x.clave_empleado == claveEmpleado : true)).ToList().Select(x => new AtencionMedicaDTO
                {
                    id = x.id,
                    claveEmpleado = x.clave_empleado,
                    empleadoDesc = listaEmpleados.Where(y => y.clave_empleado == x.clave_empleado).Select(emp => PersonalUtilities.NombreCompletoMayusculas(emp.nombre, emp.ape_paterno, emp.ape_materno)).FirstOrDefault(),
                    fecha = x.fecha,
                    fechaString = x.fecha.ToShortDateString(),
                    tipo = x.tipo,
                    tipoDesc = x.tipo.GetDescription(),
                    rutaArchivoST7 = x.rutaArchivoST7,
                    rutaArchivoST2 = x.rutaArchivoST2
                }).ToList();

                foreach (var atencionMedica in listaAtencionesMedicas)
                {
                    var revisiones = _context.tblS_SO_AtencionMedica_Revision.Where(x => x.registroActivo && x.atencionMedica_id == atencionMedica.id).ToList();
                    var ultimaRevision = revisiones.OrderByDescending(x => x.consecutivo).Last();
                    var fechaSiguienteRevision = ultimaRevision.fecha.AddDays(ultimaRevision.diasSiguienteRevision);

                    atencionMedica.terminacion = revisiones.Any(x => x.terminacion);
                    atencionMedica.archivoPendiente = ((atencionMedica.rutaArchivoST7 == null || atencionMedica.rutaArchivoST2 == null) && revisiones.Any(x => x.incapacidad));
                    atencionMedica.diasRestantes = !atencionMedica.terminacion ? (int)(fechaSiguienteRevision - hoy).TotalDays : 0;
                    atencionMedica.estatus = atencionMedica.terminacion && !atencionMedica.archivoPendiente ? "COMPLETO" : atencionMedica.terminacion && atencionMedica.archivoPendiente ? "ARCHIVO PENDIENTE" : atencionMedica.diasRestantes <= 0 ? "VENCIDO" : "PENDIENTE";
                }

                resultado.Add("data", listaAtencionesMedicas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "CargarAtencionesMedicas", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarAtencionMedica(int idAtencion)
        {
            var result = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (idAtencion <= 0)
                        throw new Exception("Ocurrió un error al eliminar la atención medica");
                    #endregion

                    #region SE ELIMINA AL MEDICO SELECCIONADO
                    tblS_SO_AtencionMedica objEliminar = _context.tblS_SO_AtencionMedica.Where(e => e.id == idAtencion).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar la atención medica");
                    else
                    {
                        objEliminar.usuarioModificacion_id = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.registroActivo = false;
                        _context.SaveChanges();

                        result.Add(SUCCESS, true);
                        result.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, idAtencion, JsonUtils.convertNetObjectToJson(objEliminar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "SaludOcupacionalController", "EliminarAtencionMedica", e, AccionEnum.ELIMINAR, idAtencion, 0);
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, e.Message);
                }
            return result;
        }

        public Dictionary<string, object> CargarAtencionMedicaDetalle(int atencionMedica_id)
        {
            try
            {
                var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.registroActivo && x.id == atencionMedica_id);

                if (atencionMedicaSIGOPLAN == null)
                {
                    throw new Exception("No se encuentra la información de la atención médica.");
                }

                var listaRevisiones = _context.tblS_SO_AtencionMedica_Revision.Where(x => x.registroActivo && x.atencionMedica_id == atencionMedica_id).ToList().Select(x => new RevisionDTO
                {
                    id = x.id,
                    consecutivo = x.consecutivo,
                    diagnostico = x.diagnostico,
                    tratamiento = x.tratamiento,
                    comentarios = x.comentarios,
                    fecha = x.fecha,
                    fechaString = x.fecha.ToShortDateString(),
                    incapacidad = x.incapacidad,
                    terminacion = x.terminacion,
                    diasSiguienteRevision = x.diasSiguienteRevision,
                    atencionMedica_id = x.atencionMedica_id
                }).ToList();

                var informacionEmpleado = getInformacionEmpleado(atencionMedicaSIGOPLAN.clave_empleado);

                var data = new AtencionMedicaDTO
                {
                    id = atencionMedicaSIGOPLAN.id,
                    claveEmpleado = atencionMedicaSIGOPLAN.clave_empleado,
                    nombre = informacionEmpleado.nombre,
                    fechaIngreso = informacionEmpleado.fechaIngreso,
                    fechaIngresoString = informacionEmpleado.fechaIngresoString,
                    puesto = informacionEmpleado.puesto,
                    puestoDesc = informacionEmpleado.puestoDesc,
                    edad = informacionEmpleado.edad,
                    supervisor = informacionEmpleado.supervisor,
                    supervisorDesc = informacionEmpleado.supervisorDesc,
                    area = informacionEmpleado.area,
                    areaDesc = informacionEmpleado.areaDesc,
                    fecha = atencionMedicaSIGOPLAN.fecha,
                    fechaString = atencionMedicaSIGOPLAN.fecha.ToShortDateString(),
                    tipo = atencionMedicaSIGOPLAN.tipo,
                    tipoDesc = atencionMedicaSIGOPLAN.tipo.GetDescription(),
                    terminacion = listaRevisiones.Any(x => x.terminacion),
                    archivoPendiente = ((atencionMedicaSIGOPLAN.rutaArchivoST7 == null || atencionMedicaSIGOPLAN.rutaArchivoST2 == null) && listaRevisiones.Any(x => x.incapacidad)),
                    revisiones = listaRevisiones,
                    rutaArchivoST7 = atencionMedicaSIGOPLAN.rutaArchivoST7,
                    rutaArchivoST2 = atencionMedicaSIGOPLAN.rutaArchivoST2
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "CargarAtencionMedicaDetalle", e, AccionEnum.CONSULTA, 0, atencionMedica_id);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarArchivoST7(int atencionMedica_id)
        {
            try
            {
                var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.id == atencionMedica_id);

                var fileStream = GlobalUtils.GetFileAsStream(atencionMedicaSIGOPLAN.rutaArchivoST7);
                string name = Path.GetFileName(atencionMedicaSIGOPLAN.rutaArchivoST7);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "DescargarArchivoST7", e, AccionEnum.CONSULTA, 0, atencionMedica_id);
                return null;
            }
        }

        public Tuple<Stream, string> DescargarArchivoST2(int atencionMedica_id)
        {
            try
            {
                var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.id == atencionMedica_id);

                var fileStream = GlobalUtils.GetFileAsStream(atencionMedicaSIGOPLAN.rutaArchivoST2);
                string name = Path.GetFileName(atencionMedicaSIGOPLAN.rutaArchivoST2);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "DescargarArchivoST2", e, AccionEnum.CONSULTA, 0, atencionMedica_id);
                return null;
            }
        }

        public AtencionMedicaReporteDTO GetAtencionMedicaReporte(int atencionMedica_id)
        {
            var atencionMedicaReporte = new AtencionMedicaReporteDTO();

            try
            {
                var atencionMedicaSIGOPLAN = _context.tblS_SO_AtencionMedica.FirstOrDefault(x => x.registroActivo && x.id == atencionMedica_id);

                if (atencionMedicaSIGOPLAN == null)
                {
                    throw new Exception("No se encuentra la información de la atención médica.");
                }

                var informacionEmpleado = getInformacionEmpleado(atencionMedicaSIGOPLAN.clave_empleado);

                atencionMedicaReporte.nombreEmpleado = informacionEmpleado.nombre;
                atencionMedicaReporte.fechaIngreso = informacionEmpleado.fechaIngresoString;
                atencionMedicaReporte.puesto = informacionEmpleado.puestoDesc;
                atencionMedicaReporte.edad = informacionEmpleado.edad.ToString();
                atencionMedicaReporte.supervisor = informacionEmpleado.supervisorDesc;
                atencionMedicaReporte.fechaAtencionMedica = atencionMedicaSIGOPLAN.fecha.ToString("dd/MM/yyyy hh:mm tt");
                atencionMedicaReporte.area = informacionEmpleado.areaDesc;
                atencionMedicaReporte.tipo = atencionMedicaSIGOPLAN.tipo.GetDescription();

                var revisionesSIGOPLAN = _context.tblS_SO_AtencionMedica_Revision.Where(x => x.registroActivo && x.atencionMedica_id == atencionMedica_id).ToList();

                atencionMedicaReporte.revisiones = revisionesSIGOPLAN.Select(x => new RevisionReporteDTO
                {
                    consecutivo = x.consecutivo.ToString(),
                    diagnostico = x.diagnostico,
                    tratamiento = x.tratamiento,
                    comentarios = x.comentarios,
                    fecha = x.fecha.ToShortDateString()
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "GetAtencionMedicaReporte", e, AccionEnum.CONSULTA, 0, atencionMedica_id);
            }

            return atencionMedicaReporte;
        }

        public Dictionary<string, object> CargarHistorialEmpleado(int claveEmpleado)
        {
            try
            {
                var data = new List<HistorialEmpleadoDTO>();
                var informacionEmpleado = getInformacionEmpleado(claveEmpleado);
                var listaAtencionesMedicas = _context.tblS_SO_AtencionMedica.Where(x => x.registroActivo && x.clave_empleado == claveEmpleado).ToList();

                data.AddRange(listaAtencionesMedicas.Select(x => new HistorialEmpleadoDTO
                {
                    id = x.id,
                    claveEmpleado = x.clave_empleado,
                    empleadoDesc = informacionEmpleado.nombre,
                    curp = informacionEmpleado.curp,
                    cc = informacionEmpleado.cc,
                    ccDesc = informacionEmpleado.ccDesc,
                    area = informacionEmpleado.area,
                    areaDesc = informacionEmpleado.areaDesc,
                    tipo = TipoDocumentoEnum.atencionMedica,
                    tipoDesc = TipoDocumentoEnum.atencionMedica.GetDescription(),
                    fecha = x.fecha,
                    fechaString = x.fecha.ToShortDateString()
                }));

                var listaHistorialesClinicos = _context.tblS_SO_HistorialesClinicos.Where(x => (bool)x.registroActivo && x.dtsPer_CURP == informacionEmpleado.curp).ToList();

                data.AddRange(listaHistorialesClinicos.Select(x => new HistorialEmpleadoDTO
                {
                    id = (int)x.id,
                    claveEmpleado = 0,
                    empleadoDesc = informacionEmpleado.nombre,
                    curp = x.dtsPer_CURP,
                    cc = informacionEmpleado.cc,
                    ccDesc = informacionEmpleado.ccDesc,
                    area = informacionEmpleado.area,
                    areaDesc = informacionEmpleado.areaDesc,
                    tipo = TipoDocumentoEnum.historialClinico,
                    tipoDesc = TipoDocumentoEnum.historialClinico.GetDescription(),
                    fecha = (DateTime)x.fechaCreacion,
                    fechaString = ((DateTime)x.fechaCreacion).ToShortDateString()
                }));

                resultado.Add("data", data.OrderByDescending(x => x.fecha).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(16, 0, NombreControlador, "CargarHistorialEmpleado", e, AccionEnum.CONSULTA, 0, claveEmpleado);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        #endregion

        #region MEDICOS
        public Dictionary<string, object> GetMedicos(medicoDTO _objFiltroDTO)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    var lstMedicosDTO = _context.tblS_SO_Medicos.Where(e => e.registroActivo == true).ToList();
                    result.Add(ITEMS, lstMedicosDTO);
                    result.Add(SUCCESS, true);

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.CONSULTA, 0, JsonUtils.convertNetObjectToJson(lstMedicosDTO));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "SaludOcupacionalController", "GetMedico", e, AccionEnum.CONSULTA, 0, _objFiltroDTO);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            return result;
        }

        public Dictionary<string, object> CrearEditarMedicos(medicoDTO _objMedicoDTO)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {

                    #region VALIDACIÓN DE CAMPOS VACIOS
                    bool errorCrearEditar = false;
                    string strMensajeError = string.Empty;
                    if (string.IsNullOrEmpty(_objMedicoDTO.nombre) || string.IsNullOrEmpty(_objMedicoDTO.cedulaProfesional) || string.IsNullOrEmpty(_objMedicoDTO.puesto) ||
                        string.IsNullOrEmpty(_objMedicoDTO.empresa))
                    {
                        errorCrearEditar = true;
                        strMensajeError += string.IsNullOrEmpty(_objMedicoDTO.nombre) ? "Es necesario indicar el nombre." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objMedicoDTO.cedulaProfesional) ? "Es necesario indicar la cedula profesional." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objMedicoDTO.puesto) ? "Es necesario indicar el puesto." : string.Empty;
                        strMensajeError += string.IsNullOrEmpty(_objMedicoDTO.empresa) ? "Es necesario indicar la empresa" : string.Empty;

                        result.Add(SUCCESS, false);
                        result.Add(MESSAGE, strMensajeError);
                    }
                    #endregion


                    #region CREAR/EDITAR Medico
                    tblS_SO_Medicos objCEMedicoDTO = new tblS_SO_Medicos();

                    if (_objMedicoDTO.id > 0 && !errorCrearEditar)
                    {
                        #region EDITAR MEDICO

                        objCEMedicoDTO = _context.tblS_SO_Medicos.Where(e => e.id == _objMedicoDTO.id).FirstOrDefault();

                        objCEMedicoDTO.nombre = _objMedicoDTO.nombre;
                        objCEMedicoDTO.cedulaProfesional = _objMedicoDTO.cedulaProfesional;
                        objCEMedicoDTO.puesto = _objMedicoDTO.puesto;
                        objCEMedicoDTO.empresa = _objMedicoDTO.empresa;
                        objCEMedicoDTO.idUsuarioSIGOPLAN = _objMedicoDTO.idUsuarioSIGOPLAN;
                        objCEMedicoDTO.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEMedicoDTO.fechaModificacion = DateTime.Now;
                        _context.SaveChanges();

                        result.Add(MESSAGE, "Se ha actualizado con éxito al medico.");
                        result.Add(SUCCESS, true);

                        #endregion
                    }
                    else if (!errorCrearEditar)
                    {
                        #region CREAR MEDICO

                        objCEMedicoDTO.nombre = _objMedicoDTO.nombre;
                        objCEMedicoDTO.cedulaProfesional = _objMedicoDTO.cedulaProfesional;
                        objCEMedicoDTO.puesto = _objMedicoDTO.puesto;
                        objCEMedicoDTO.empresa = _objMedicoDTO.empresa;
                        objCEMedicoDTO.idUsuarioSIGOPLAN = _objMedicoDTO.idUsuarioSIGOPLAN;
                        objCEMedicoDTO.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCEMedicoDTO.fechaModificacion = DateTime.Now;
                        objCEMedicoDTO.fechaCreacion = DateTime.Now;
                        objCEMedicoDTO.fechaModificacion = new DateTime(2000, 01, 01);
                        objCEMedicoDTO.registroActivo = true;
                        _context.tblS_SO_Medicos.Add(objCEMedicoDTO);
                        _context.SaveChanges();

                        result.Add(MESSAGE, "Se ha registrado con éxito al medico.");
                        result.Add(SUCCESS, true);

                        #endregion
                    }

                    dbContextTransaction.Commit();

                    #endregion

                    #region SE CREA BITACORA
                    int idMedico = _objMedicoDTO.id > 0 ? _objMedicoDTO.id : 0;
                    if (idMedico == 0)
                    {
                        idMedico = _context.tblS_SO_Medicos.Where(e => e.registroActivo == true).OrderByDescending(e => e.id).FirstOrDefault().id;
                    }
                    SaveBitacora(16, _objMedicoDTO.id > 0 ? (int)AccionEnum.ACTUALIZAR : (int)AccionEnum.AGREGAR, idMedico, JsonUtils.convertNetObjectToJson(objCEMedicoDTO));
                    #endregion


                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "SaludOcupacionalController", "CrearEditarMedico", e, _objMedicoDTO.id > 0 ? AccionEnum.ACTUALIZAR : AccionEnum.AGREGAR, _objMedicoDTO.id > 0 ? _objMedicoDTO.id : 0, 0);

                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            return result;
        }

        public Dictionary<string, object> EliminarMedico(int _idMedico)
        {
            var result = new Dictionary<string, object>();
            using (var dbContextTransaction = _context.Database.BeginTransaction())
                try
                {
                    #region VALIDACIONES
                    if (_idMedico <= 0)
                        throw new Exception("Ocurrió un error al eliminar al transportista.");
                    #endregion

                    #region SE ELIMINA AL MEDICO SELECCIONADO
                    tblS_SO_Medicos objEliminar = _context.tblS_SO_Medicos.Where(e => e.id == _idMedico).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar al transportista.");
                    else
                    {
                        objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objEliminar.fechaModificacion = DateTime.Now;
                        objEliminar.registroActivo = false;
                        _context.SaveChanges();

                        result.Add(SUCCESS, true);
                        result.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    }

                    dbContextTransaction.Commit();
                    #endregion

                    #region SE CREA BITACORA
                    SaveBitacora(16, (int)AccionEnum.ELIMINAR, _idMedico, JsonUtils.convertNetObjectToJson(objEliminar));
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, "SaludOcupacionalController", "EliminarMedico", e, AccionEnum.ELIMINAR, _idMedico, 0);
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, e.Message);
                }
            return result;
        }

        #endregion

        #region HISTORIAL CLINICO
        public Dictionary<string, object> CEHistorialClinico(HistorialClinicoDTO objHistorialClinicoDTO, List<HttpPostedFileBase> lstArchivosDatosPersonales, List<HttpPostedFileBase> lstArchivosEspirometria, List<HttpPostedFileBase> lstArchivosAudiometria, List<HttpPostedFileBase> lstArchivosElectrocardiograma,
                                               List<HttpPostedFileBase> lstArchivosRadiografias, List<HttpPostedFileBase> lstArchivosLaboratorio, List<HttpPostedFileBase> lstArchivosDocumentosAdjuntos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    int idHC = 0;
                    tblS_SO_HistorialesClinicos objCE = new tblS_SO_HistorialesClinicos();
                    if (objHistorialClinicoDTO.id > 0)
                    {
                        #region SE ACTUALIZA LOS DATOS DEL HISTORIAL CLINICO

                        objCE = _context.tblS_SO_HistorialesClinicos.Where(w => w.id == objHistorialClinicoDTO.id).FirstOrDefault();
                        if (objCE == null)
                            throw new Exception("Ocurrió un error al actualizar la información del historial clinico.");

                        #region DATOS PERSONALES
                        objCE.dtsPer_EmpresaID = objHistorialClinicoDTO.dtsPer_EmpresaID;
                        objCE.dtsPer_CCID = objHistorialClinicoDTO.dtsPer_CCID;
                        objCE.dtsPer_Paciente = objHistorialClinicoDTO.dtsPer_Paciente;
                        objCE.dtsPer_FechaHora = objHistorialClinicoDTO.dtsPer_FechaHora;
                        objCE.dtsPer_FechaNac = objHistorialClinicoDTO.dtsPer_FechaNac;
                        objCE.dtsPer_Sexo = objHistorialClinicoDTO.dtsPer_Sexo;
                        objCE.dtsPer_EstadoCivilID = objHistorialClinicoDTO.dtsPer_EstadoCivilID;
                        objCE.dtsPer_TipoSangreID = objHistorialClinicoDTO.dtsPer_TipoSangreID;
                        objCE.dtsPer_CURP = objHistorialClinicoDTO.dtsPer_CURP;
                        objCE.dtsPer_Domicilio = objHistorialClinicoDTO.dtsPer_Domicilio;
                        objCE.dtsPer_Ciudad = objHistorialClinicoDTO.dtsPer_Ciudad;
                        objCE.dtsPer_EscolaridadID = objHistorialClinicoDTO.dtsPer_EscolaridadID;
                        objCE.dtsPer_LugarNac = objHistorialClinicoDTO.dtsPer_LugarNac;
                        objCE.dtsPer_Telefono = objHistorialClinicoDTO.dtsPer_Telefono;
                        #endregion

                        #region MOTIVO DE LA EVALUACIÓN
                        objCE.motEva_esIngreso = objHistorialClinicoDTO.motEva_esIngreso;
                        objCE.motEva_esRetiro = objHistorialClinicoDTO.motEva_esRetiro;
                        objCE.motEva_esEvaOpcional = objHistorialClinicoDTO.motEva_esEvaOpcional;
                        objCE.motEva_esPostIncapacidad = objHistorialClinicoDTO.motEva_esPostIncapacidad;
                        objCE.motEva_esReubicacion = objHistorialClinicoDTO.motEva_esReubicacion;
                        #endregion

                        #region ANTECEDENTES LABORALES
                        objCE.antLab_Puesto = objHistorialClinicoDTO.antLab_Puesto;
                        objCE.antLab_Empresa = objHistorialClinicoDTO.antLab_Empresa;
                        objCE.antLab_Desde = objHistorialClinicoDTO.antLab_Desde;
                        objCE.antLab_Hasta = objHistorialClinicoDTO.antLab_Hasta;
                        objCE.antLab_Turno = objHistorialClinicoDTO.antLab_Turno;
                        objCE.antLab_esDePie = objHistorialClinicoDTO.antLab_esDePie;
                        objCE.antLab_esInclinado = objHistorialClinicoDTO.antLab_esInclinado;
                        objCE.antLab_esSentado = objHistorialClinicoDTO.antLab_esSentado;
                        objCE.antLab_esArrodillado = objHistorialClinicoDTO.antLab_esArrodillado;
                        objCE.antLab_esCaminando = objHistorialClinicoDTO.antLab_esCaminando;
                        objCE.antLab_esOtra = objHistorialClinicoDTO.antLab_esOtra;
                        objCE.antLab_Cual = objHistorialClinicoDTO.antLab_Cual;
                        #endregion

                        #region ACCIDENTES Y ENFERMEDADES DE TRABAJO
                        objCE.accET_Empresa = objHistorialClinicoDTO.accET_Empresa;
                        objCE.accET_Anio = objHistorialClinicoDTO.accET_Anio;
                        objCE.accET_LesionAreaAnatomica = objHistorialClinicoDTO.accET_LesionAreaAnatomica;
                        objCE.accET_Secuelas = objHistorialClinicoDTO.accET_Secuelas;
                        objCE.accET_Cuales = objHistorialClinicoDTO.accET_Cuales;
                        objCE.accET_ExamNoAceptables = objHistorialClinicoDTO.accET_ExamNoAceptables;
                        objCE.accET_Causas = objHistorialClinicoDTO.accET_Causas;
                        objCE.accET_AbandonoTrabajo = objHistorialClinicoDTO.accET_AbandonoTrabajo;
                        objCE.accET_IncapacidadFrecuente = objHistorialClinicoDTO.accET_IncapacidadFrecuente;
                        objCE.accET_Prolongadas = objHistorialClinicoDTO.accET_Prolongadas;
                        #endregion

                        #region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
                        objCE.usoElePP_esActual = objHistorialClinicoDTO.usoElePP_esActual;
                        objCE.usoElePP_esCasco = objHistorialClinicoDTO.usoElePP_esCasco;
                        objCE.usoElePP_esTapaboca = objHistorialClinicoDTO.usoElePP_esTapaboca;
                        objCE.usoElePP_esGafas = objHistorialClinicoDTO.usoElePP_esGafas;
                        objCE.usoElePP_esRespirador = objHistorialClinicoDTO.usoElePP_esRespirador;
                        objCE.usoElePP_esBotas = objHistorialClinicoDTO.usoElePP_esBotas;
                        objCE.usoElePP_esAuditivos = objHistorialClinicoDTO.usoElePP_esAuditivos;
                        objCE.usoElePP_esOverol = objHistorialClinicoDTO.usoElePP_esOverol;
                        objCE.usoElePP_esGuantes = objHistorialClinicoDTO.usoElePP_esGuantes;
                        objCE.usoElePP_OtroCual = objHistorialClinicoDTO.usoElePP_OtroCual;
                        objCE.usoElePP_DeberiaRecibir = objHistorialClinicoDTO.usoElePP_DeberiaRecibir;
                        objCE.usoElePP_ConsideraAdecuado = objHistorialClinicoDTO.usoElePP_ConsideraAdecuado;
                        #endregion

                        #region ANTECEDENTES FAMILIARES
                        objCE.antFam_esTuberculosis = objHistorialClinicoDTO.antFam_esTuberculosis;
                        objCE.antFam_TuberculosisParentesco = objHistorialClinicoDTO.antFam_TuberculosisParentesco;
                        objCE.antFam_esHTA = objHistorialClinicoDTO.antFam_esHTA;
                        objCE.antFam_HTAParentesco = objHistorialClinicoDTO.antFam_HTAParentesco;
                        objCE.antFam_esDiabetes = objHistorialClinicoDTO.antFam_esDiabetes;
                        objCE.antFam_DiabetesParentesco = objHistorialClinicoDTO.antFam_DiabetesParentesco;
                        objCE.antFam_esACV = objHistorialClinicoDTO.antFam_esACV;
                        objCE.antFam_ACVParentesco = objHistorialClinicoDTO.antFam_ACVParentesco;
                        objCE.antFam_esInfarto = objHistorialClinicoDTO.antFam_esInfarto;
                        objCE.antFam_InfartoParentesco = objHistorialClinicoDTO.antFam_InfartoParentesco;
                        objCE.antFam_esAsma = objHistorialClinicoDTO.antFam_esAsma;
                        objCE.antFam_AsmaParentesco = objHistorialClinicoDTO.antFam_AsmaParentesco;
                        objCE.antFam_esAlergias = objHistorialClinicoDTO.antFam_esAlergias;
                        objCE.antFam_AlergiasParentesco = objHistorialClinicoDTO.antFam_AlergiasParentesco;
                        objCE.antFam_esMental = objHistorialClinicoDTO.antFam_esMental;
                        objCE.antFam_MentalParentesco = objHistorialClinicoDTO.antFam_MentalParentesco;
                        objCE.antFam_esCancer = objHistorialClinicoDTO.antFam_esCancer;
                        objCE.antFam_CancerParentesco = objHistorialClinicoDTO.antFam_CancerParentesco;
                        objCE.antFam_Observaciones = objHistorialClinicoDTO.antFam_Observaciones;
                        #endregion

                        #region ANTECEDENTES PERSONALES NO PATOLÓGICOS
                        objCE.antPerNoPat_Tabaquismo = objHistorialClinicoDTO.antPerNoPat_Tabaquismo;
                        objCE.antPerNoPat_CigarroDia = objHistorialClinicoDTO.antPerNoPat_CigarroDia;
                        objCE.antPerNoPat_CigarroAnios = objHistorialClinicoDTO.antPerNoPat_CigarroAnios;
                        objCE.antPerNoPat_Alcoholismo = objHistorialClinicoDTO.antPerNoPat_Alcoholismo;
                        objCE.antPerNoPat_AlcoholismoAnios = objHistorialClinicoDTO.antPerNoPat_AlcoholismoAnios;
                        objCE.antPerNoPat_esDrogadiccion = objHistorialClinicoDTO.antPerNoPat_esDrogadiccion;
                        objCE.antPerNoPat_esMarihuana = objHistorialClinicoDTO.antPerNoPat_esMarihuana;
                        objCE.antPerNoPat_esCocaina = objHistorialClinicoDTO.antPerNoPat_esCocaina;
                        objCE.antPerNoPat_esAnfetaminas = objHistorialClinicoDTO.antPerNoPat_esAnfetaminas;
                        objCE.antPerNoPat_Otros = objHistorialClinicoDTO.antPerNoPat_Otros;
                        objCE.antPerNoPat_Inmunizaciones = objHistorialClinicoDTO.antPerNoPat_Inmunizaciones;
                        objCE.antPerNoPat_Tetanicos = objHistorialClinicoDTO.antPerNoPat_Tetanicos;
                        objCE.antPerNoPat_FechaAntitetanica = objHistorialClinicoDTO.antPerNoPat_FechaAntitetanica;
                        objCE.antPerNoPat_Hepatitis = objHistorialClinicoDTO.antPerNoPat_Hepatitis;
                        objCE.antPerNoPat_Influenza = objHistorialClinicoDTO.antPerNoPat_Influenza;
                        objCE.antPerNoPat_FechaInfluenza = objHistorialClinicoDTO.antPerNoPat_FechaInfluenza;
                        objCE.antPerNoPat_Infancia = objHistorialClinicoDTO.antPerNoPat_Infancia;
                        objCE.antPerNoPat_DescInfancia = objHistorialClinicoDTO.antPerNoPat_DescInfancia;
                        objCE.antPerNoPat_Alimentacion = objHistorialClinicoDTO.antPerNoPat_Alimentacion;
                        objCE.antPerNoPat_Higiene = objHistorialClinicoDTO.antPerNoPat_Higiene;
                        objCE.antPerNoPat_MedicacionActual = objHistorialClinicoDTO.antPerNoPat_MedicacionActual;
                        #endregion

                        #region ANTECEDENTES PERSONALES PATOLÓGICOS
                        objCE.antPerPat_esNeoplasicos = objHistorialClinicoDTO.antPerPat_esNeoplasicos;
                        objCE.antPerPat_esNeumopatias = objHistorialClinicoDTO.antPerPat_esNeumopatias;
                        objCE.antPerPat_esAsma = objHistorialClinicoDTO.antPerPat_esAsma;
                        objCE.antPerPat_esFimico = objHistorialClinicoDTO.antPerPat_esFimico;
                        objCE.antPerPat_esNeumoconiosis = objHistorialClinicoDTO.antPerPat_esNeumoconiosis;
                        objCE.antPerPat_esCardiopatias = objHistorialClinicoDTO.antPerPat_esCardiopatias;
                        objCE.antPerPat_esReumaticos = objHistorialClinicoDTO.antPerPat_esReumaticos;
                        objCE.antPerPat_esAlergias = objHistorialClinicoDTO.antPerPat_esAlergias;
                        objCE.antPerPat_esHipertension = objHistorialClinicoDTO.antPerPat_esHipertension;
                        objCE.antPerPat_esHepatitis = objHistorialClinicoDTO.antPerPat_esHepatitis;
                        objCE.antPerPat_esTifoidea = objHistorialClinicoDTO.antPerPat_esTifoidea;
                        objCE.antPerPat_esHernias = objHistorialClinicoDTO.antPerPat_esHernias;
                        objCE.antPerPat_esLumbalgias = objHistorialClinicoDTO.antPerPat_esLumbalgias;
                        objCE.antPerPat_esDiabetes = objHistorialClinicoDTO.antPerPat_esDiabetes;
                        objCE.antPerPat_esEpilepsias = objHistorialClinicoDTO.antPerPat_esEpilepsias;
                        objCE.antPerPat_esVenereas = objHistorialClinicoDTO.antPerPat_esVenereas;
                        objCE.antPerPat_esCirugias = objHistorialClinicoDTO.antPerPat_esCirugias;
                        objCE.antPerPat_esFracturas = objHistorialClinicoDTO.antPerPat_esFracturas;
                        objCE.antPerPat_ObservacionesPat = objHistorialClinicoDTO.antPerPat_ObservacionesPat;
                        #endregion

                        #region INTERROGATORIO POR APARATOS Y SISTEMAS
                        objCE.intApaSis_esRespiratorio = objHistorialClinicoDTO.intApaSis_esRespiratorio;
                        objCE.intApaSis_esDigestivo = objHistorialClinicoDTO.intApaSis_esDigestivo;
                        objCE.intApaSis_esCardiovascular = objHistorialClinicoDTO.intApaSis_esCardiovascular;
                        objCE.intApaSis_esNervioso = objHistorialClinicoDTO.intApaSis_esNervioso;
                        objCE.intApaSis_esUrinario = objHistorialClinicoDTO.intApaSis_esUrinario;
                        objCE.intApaSis_esEndocrino = objHistorialClinicoDTO.intApaSis_esEndocrino;
                        objCE.intApaSis_esPsiquiatrico = objHistorialClinicoDTO.intApaSis_esPsiquiatrico;
                        objCE.intApaSis_esEsqueletico = objHistorialClinicoDTO.intApaSis_esEsqueletico;
                        objCE.intApaSis_esAudicion = objHistorialClinicoDTO.intApaSis_esAudicion;
                        objCE.intApaSis_esVision = objHistorialClinicoDTO.intApaSis_esVision;
                        objCE.intApaSis_esOlfato = objHistorialClinicoDTO.intApaSis_esOlfato;
                        objCE.intApaSis_esTacto = objHistorialClinicoDTO.intApaSis_esTacto;
                        objCE.intApaSis_ObservacionesPat = objHistorialClinicoDTO.intApaSis_ObservacionesPat;
                        #endregion

                        #region PADECIMIENTOS ACTUALES
                        objCE.padAct_PadActuales = objHistorialClinicoDTO.padAct_PadActuales;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-SIGNOS VITALES
                        objCE.expFSV_TArterial = objHistorialClinicoDTO.expFSV_TArterial;
                        objCE.expFSV_Pulso = objHistorialClinicoDTO.expFSV_Pulso;
                        objCE.expFSV_Temp = objHistorialClinicoDTO.expFSV_Temp;
                        objCE.expFSV_FCardiaca = objHistorialClinicoDTO.expFSV_FCardiaca;
                        objCE.expFSV_FResp = objHistorialClinicoDTO.expFSV_FResp;
                        objCE.expFSV_Peso = objHistorialClinicoDTO.expFSV_Peso;
                        objCE.expFSV_Talla = objHistorialClinicoDTO.expFSV_Talla;
                        objCE.expFSV_IMC = objHistorialClinicoDTO.expFSV_IMC;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-CABEZA
                        objCE.expFC_Craneo = objHistorialClinicoDTO.expFC_Craneo;
                        objCE.expFC_Parpados = objHistorialClinicoDTO.expFC_Parpados;
                        objCE.expFC_Conjutiva = objHistorialClinicoDTO.expFC_Conjutiva;
                        objCE.expFC_Reflejos = objHistorialClinicoDTO.expFC_Reflejos;
                        objCE.expFC_FosasNasales = objHistorialClinicoDTO.expFC_FosasNasales;
                        objCE.expFC_Boca = objHistorialClinicoDTO.expFC_Boca;
                        objCE.expFC_Amigdalas = objHistorialClinicoDTO.expFC_Amigdalas;
                        objCE.expFC_Dentadura = objHistorialClinicoDTO.expFC_Dentadura;
                        objCE.expFC_Encias = objHistorialClinicoDTO.expFC_Encias;
                        objCE.expFC_Cuello = objHistorialClinicoDTO.expFC_Cuello;
                        objCE.expFC_Tiroides = objHistorialClinicoDTO.expFC_Tiroides;
                        objCE.expFC_Ganglios = objHistorialClinicoDTO.expFC_Ganglios;
                        objCE.expFC_Oidos = objHistorialClinicoDTO.expFC_Oidos;
                        objCE.expFC_Otros = objHistorialClinicoDTO.expFC_Otros;
                        objCE.expFC_Observaciones = objHistorialClinicoDTO.expFC_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
                        objCE.expFAV_VisCerAmbosOjos = objHistorialClinicoDTO.expFAV_VisCerAmbosOjos;
                        objCE.expFAV_VisCerOjoIzq = objHistorialClinicoDTO.expFAV_VisCerOjoIzq;
                        objCE.expFAV_VisCerOjoDer = objHistorialClinicoDTO.expFAV_VisCerOjoDer;
                        objCE.expFAV_VisLejAmbosOjos = objHistorialClinicoDTO.expFAV_VisLejAmbosOjos;
                        objCE.expFAV_VisLejOjoIzq = objHistorialClinicoDTO.expFAV_VisLejOjoIzq;
                        objCE.expFAV_VisLejOjoDer = objHistorialClinicoDTO.expFAV_VisLejOjoDer;
                        objCE.expFAV_CorregidaAmbosOjos = objHistorialClinicoDTO.expFAV_CorregidaAmbosOjos;
                        objCE.expFAV_CorregidaOjoIzq = objHistorialClinicoDTO.expFAV_CorregidaOjoIzq;
                        objCE.expFAV_CorregidaOjoDer = objHistorialClinicoDTO.expFAV_CorregidaOjoDer;
                        objCE.expFAV_CampimetriaOI = objHistorialClinicoDTO.expFAV_CampimetriaOI;
                        objCE.expFAV_CampimetriaOD = objHistorialClinicoDTO.expFAV_CampimetriaOD;
                        objCE.expFAV_PterigionOI = objHistorialClinicoDTO.expFAV_PterigionOI;
                        objCE.expFAV_PterigionOD = objHistorialClinicoDTO.expFAV_PterigionOD;
                        objCE.expFAV_FondoOjo = objHistorialClinicoDTO.expFAV_FondoOjo;
                        objCE.expFAV_Daltonismo = objHistorialClinicoDTO.expFAV_Daltonismo;
                        objCE.expFAV_Observaciones = objHistorialClinicoDTO.expFAV_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
                        objCE.expFTATE_esCamposPulmonares = objHistorialClinicoDTO.expFTATE_esCamposPulmonares;
                        objCE.expFTATE_esPuntosDolorosos = objHistorialClinicoDTO.expFTATE_esPuntosDolorosos;
                        objCE.expFTATE_esGenitales = objHistorialClinicoDTO.expFTATE_esGenitales;
                        objCE.expFTATE_esRuidosCardiacos = objHistorialClinicoDTO.expFTATE_esRuidosCardiacos;
                        objCE.expFTATE_esHallusValgus = objHistorialClinicoDTO.expFTATE_esHallusValgus;
                        objCE.expFTATE_esHerniasUmbili = objHistorialClinicoDTO.expFTATE_esHerniasUmbili;
                        objCE.expFTATE_esAreaRenal = objHistorialClinicoDTO.expFTATE_esAreaRenal;
                        objCE.expFTATE_esVaricocele = objHistorialClinicoDTO.expFTATE_esVaricocele;
                        objCE.expFTATE_esGrandulasMamarias = objHistorialClinicoDTO.expFTATE_esGrandulasMamarias;
                        objCE.expFTATE_esColumnaVertebral = objHistorialClinicoDTO.expFTATE_esColumnaVertebral;
                        objCE.expFTATE_esPiePlano = objHistorialClinicoDTO.expFTATE_esPiePlano;
                        objCE.expFTATE_esVarices = objHistorialClinicoDTO.expFTATE_esVarices;
                        objCE.expFTATE_esMiembrosSup = objHistorialClinicoDTO.expFTATE_esMiembrosSup;
                        objCE.expFTATE_esParedAbdominal = objHistorialClinicoDTO.expFTATE_esParedAbdominal;
                        objCE.expFTATE_esAnillosInguinales = objHistorialClinicoDTO.expFTATE_esAnillosInguinales;
                        objCE.expFTATE_esMiembrosInf = objHistorialClinicoDTO.expFTATE_esMiembrosInf;
                        objCE.expFTATE_esTatuajes = objHistorialClinicoDTO.expFTATE_esTatuajes;
                        objCE.expFTATE_esVisceromegalias = objHistorialClinicoDTO.expFTATE_esVisceromegalias;
                        objCE.expFTATE_esMarcha = objHistorialClinicoDTO.expFTATE_esMarcha;
                        objCE.expFTATE_esHerniasInguinales = objHistorialClinicoDTO.expFTATE_esHerniasInguinales;
                        objCE.expFTATE_esHombrosDolorosos = objHistorialClinicoDTO.expFTATE_esHombrosDolorosos;
                        objCE.expFTATE_esQuistes = objHistorialClinicoDTO.expFTATE_esQuistes;
                        objCE.expFTATE_Observaciones = objHistorialClinicoDTO.expFTATE_Observaciones;
                        objCE.expFTATE_MS_HombroDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esFlexion;
                        objCE.expFTATE_MS_HombroIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esFlexion;
                        objCE.expFTATE_MS_CodoDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esFlexion;
                        objCE.expFTATE_MS_CodoIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esFlexion;
                        objCE.expFTATE_MS_MunecaDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esFlexion;
                        objCE.expFTATE_MS_MunecaIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esFlexion;
                        objCE.expFTATE_MS_DedosDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esFlexion;
                        objCE.expFTATE_MS_DedosIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esFlexion;
                        objCE.expFTATE_MS_HombroDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esExtension;
                        objCE.expFTATE_MS_HombroIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esExtension;
                        objCE.expFTATE_MS_CodoDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esExtension;
                        objCE.expFTATE_MS_CodoIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esExtension;
                        objCE.expFTATE_MS_MunecaDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esExtension;
                        objCE.expFTATE_MS_MunecaIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esExtension;
                        objCE.expFTATE_MS_DedosDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esExtension;
                        objCE.expFTATE_MS_DedosIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esExtension;
                        objCE.expFTATE_MS_HombroDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esAbduccion;
                        objCE.expFTATE_MS_HombroIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esAbduccion;
                        objCE.expFTATE_MS_CodoDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esAbduccion;
                        objCE.expFTATE_MS_CodoIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esAbduccion;
                        objCE.expFTATE_MS_MunecaDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esAbduccion;
                        objCE.expFTATE_MS_MunecaIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esAbduccion;
                        objCE.expFTATE_MS_DedosDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esAbduccion;
                        objCE.expFTATE_MS_DedosIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esAbduccion;
                        objCE.expFTATE_MS_HombroDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esAduccion;
                        objCE.expFTATE_MS_HombroIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esAduccion;
                        objCE.expFTATE_MS_MunecaDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esAduccion;
                        objCE.expFTATE_MS_MunecaIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esAduccion;
                        objCE.expFTATE_MS_DedosDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esAduccion;
                        objCE.expFTATE_MS_DedosIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esAduccion;
                        objCE.expFTATE_MS_HombroDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esRotInterna;
                        objCE.expFTATE_MS_HombroIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esRotInterna;
                        objCE.expFTATE_MS_MunecaDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esRotInterna;
                        objCE.expFTATE_MS_MunecaIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esRotInterna;
                        objCE.expFTATE_MS_DedosDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esRotInterna;
                        objCE.expFTATE_MS_DedosIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esRotInterna;
                        objCE.expFTATE_MS_HombroDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esRotExterna;
                        objCE.expFTATE_MS_HombroIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esRotExterna;
                        objCE.expFTATE_MS_MunecaDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esRotExterna;
                        objCE.expFTATE_MS_MunecaIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esRotExterna;
                        objCE.expFTATE_MS_DedosDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esRotExterna;
                        objCE.expFTATE_MS_DedosIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esRotExterna;
                        objCE.expFTATE_MS_CodoDer_esPronacion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esPronacion;
                        objCE.expFTATE_MS_CodoIzq_esPronacion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esPronacion;
                        objCE.expFTATE_MS_MunecaDer_esPronacion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esPronacion;
                        objCE.expFTATE_MS_MunecaIzq_esPronacion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esPronacion;
                        objCE.expFTATE_MS_CodoDer_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esSupinacion;
                        objCE.expFTATE_MS_CodoIzq_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esSupinacion;
                        objCE.expFTATE_MS_MunecaDer_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esSupinacion;
                        objCE.expFTATE_MS_MunecaIzq_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esSupinacion;
                        objCE.expFTATE_MS_MunecaDer_esDesvUlnar = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esDesvUlnar;
                        objCE.expFTATE_MS_MunecaIzq_esDesvUlnar = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esDesvUlnar;
                        objCE.expFTATE_MS_MunecaDer_esDesvRadial = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esDesvRadial;
                        objCE.expFTATE_MS_MunecaIzq_esDesvRadial = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esDesvRadial;
                        objCE.expFTATE_MS_MunecaDer_esOponencia = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esOponencia;
                        objCE.expFTATE_MS_MunecaIzq_esOponencia = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esOponencia;
                        objCE.expFTATE_MS_DedosDer_esOponencia = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esOponencia;
                        objCE.expFTATE_MS_DedosIzq_esOponencia = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esOponencia;
                        objCE.expFTATE_MI_CaderaDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esFlexion;
                        objCE.expFTATE_MI_CaderaIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esFlexion;
                        objCE.expFTATE_MI_RodillasDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esFlexion;
                        objCE.expFTATE_MI_RodillasIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esFlexion;
                        objCE.expFTATE_MI_CllPieDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esFlexion;
                        objCE.expFTATE_MI_CllPieIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esFlexion;
                        objCE.expFTATE_MI_DedosDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esFlexion;
                        objCE.expFTATE_MI_DedosIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esFlexion;
                        objCE.expFTATE_MI_CaderaDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esExtension;
                        objCE.expFTATE_MI_CaderaIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esExtension;
                        objCE.expFTATE_MI_RodillasDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esExtension;
                        objCE.expFTATE_MI_RodillasIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esExtension;
                        objCE.expFTATE_MI_CllPieDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esExtension;
                        objCE.expFTATE_MI_CllPieIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esExtension;
                        objCE.expFTATE_MI_DedosDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esExtension;
                        objCE.expFTATE_MI_DedosIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esExtension;
                        objCE.expFTATE_MI_CaderaDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esAbduccion;
                        objCE.expFTATE_MI_CaderaIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esAbduccion;
                        objCE.expFTATE_MI_CllPieDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esAbduccion;
                        objCE.expFTATE_MI_CllPieIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esAbduccion;
                        objCE.expFTATE_MI_DedosDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esAbduccion;
                        objCE.expFTATE_MI_DedosIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esAbduccion;
                        objCE.expFTATE_MI_CaderaDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esAduccion;
                        objCE.expFTATE_MI_CaderaIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esAduccion;
                        objCE.expFTATE_MI_CllPieDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esAduccion;
                        objCE.expFTATE_MI_CllPieIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esAduccion;
                        objCE.expFTATE_MI_DedosDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esAduccion;
                        objCE.expFTATE_MI_DedosIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esAduccion;
                        objCE.expFTATE_MI_CaderaDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esRotInterna;
                        objCE.expFTATE_MI_CaderaIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esRotInterna;
                        objCE.expFTATE_MI_RodillasDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esRotInterna;
                        objCE.expFTATE_MI_RodillasIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esRotInterna;
                        objCE.expFTATE_MI_CllPieDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esRotInterna;
                        objCE.expFTATE_MI_CllPieIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esRotInterna;
                        objCE.expFTATE_MI_CaderaDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esRotExterna;
                        objCE.expFTATE_MI_CaderaIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esRotExterna;
                        objCE.expFTATE_MI_RodillasDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esRotExterna;
                        objCE.expFTATE_MI_RodillasIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esRotExterna;
                        objCE.expFTATE_MI_CllPieDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esRotExterna;
                        objCE.expFTATE_MI_CllPieIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esRotExterna;
                        objCE.expFTATE_MI_CllPieDer_esInversion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esInversion;
                        objCE.expFTATE_MI_CllPieIzq_esInversion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esInversion;
                        objCE.expFTATE_MI_CllPieDer_esEversion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esEversion;
                        objCE.expFTATE_MI_CllPieIzq_esEversion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esEversion;
                        objCE.expFTATE_MS_MI_Observaciones = objHistorialClinicoDTO.expFTATE_MS_MI_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-DATOS GENERALES
                        objCE.expFDT_PielMucosas = objHistorialClinicoDTO.expFDT_PielMucosas;
                        objCE.expFDT_EstadoPsiquiatrico = objHistorialClinicoDTO.expFDT_EstadoPsiquiatrico;
                        objCE.expFDT_ExamenNeurologico = objHistorialClinicoDTO.expFDT_ExamenNeurologico;
                        objCE.expFDT_FobiasActuales = objHistorialClinicoDTO.expFDT_FobiasActuales;
                        objCE.expFDT_Higiene = objHistorialClinicoDTO.expFDT_Higiene;
                        objCE.expFDT_ConstitucionFisica = objHistorialClinicoDTO.expFDT_ConstitucionFisica;
                        objCE.expFDT_Otros = objHistorialClinicoDTO.expFDT_Otros;
                        objCE.expFDT_Observaciones = objHistorialClinicoDTO.expFDT_Observaciones;
                        #endregion

                        #region ESTUDIOS DE GABINETE
                        objCE.estGab_TipoSanguineoID = objHistorialClinicoDTO.estGab_TipoSanguineoID;
                        objCE.estGab_Antidoping = objHistorialClinicoDTO.estGab_Antidoping;
                        objCE.estGab_Laboratorios = objHistorialClinicoDTO.estGab_Laboratorios;
                        objCE.estGab_ObservacionesGrupoRH = objHistorialClinicoDTO.estGab_ObservacionesGrupoRH;
                        objCE.estGab_ExamGenOrina = objHistorialClinicoDTO.estGab_ExamGenOrina;
                        objCE.estGab_ExamGenOrinaObservaciones = objHistorialClinicoDTO.estGab_ExamGenOrinaObservaciones;
                        objCE.estGab_Radiografias = objHistorialClinicoDTO.estGab_Radiografias;
                        objCE.estGab_RadiografiasObservaciones = objHistorialClinicoDTO.estGab_RadiografiasObservaciones;
                        objCE.estGab_Audiometria = objHistorialClinicoDTO.estGab_Audiometria;
                        objCE.estGab_AudiometriaObservaciones = objHistorialClinicoDTO.estGab_AudiometriaObservaciones;
                        objCE.estGab_HBC = objHistorialClinicoDTO.estGab_HBC;
                        objCE.estGab_Espirometria = objHistorialClinicoDTO.estGab_Espirometria;
                        objCE.estGab_EspirometriaObservaciones = objHistorialClinicoDTO.estGab_EspirometriaObservaciones;
                        objCE.estGab_Electrocardiograma = objHistorialClinicoDTO.estGab_Electrocardiograma;
                        objCE.estGab_ElectrocardiogramaObservaciones = objHistorialClinicoDTO.estGab_ElectrocardiogramaObservaciones;
                        objCE.estGab_FechaPrimeraDosis = objHistorialClinicoDTO.estGab_FechaPrimeraDosis;
                        objCE.estGab_FechaSegundaDosis = objHistorialClinicoDTO.estGab_FechaSegundaDosis;
                        objCE.estGab_MarcaDosisID = objHistorialClinicoDTO.estGab_MarcaDosisID;
                        objCE.estGab_VacunacionObservaciones = objHistorialClinicoDTO.estGab_VacunacionObservaciones;
                        objCE.estGab_LstProblemas = objHistorialClinicoDTO.estGab_LstProblemas;
                        objCE.estGab_Recomendaciones = objHistorialClinicoDTO.estGab_Recomendaciones;
                        #endregion

                        #region ESPIROMETRÍA
                        objCE.esp_Espirometria = objHistorialClinicoDTO.esp_Espirometria;
                        objCE.esp_EspirometriaObservaciones = objHistorialClinicoDTO.esp_EspirometriaObservaciones;
                        #endregion

                        #region AUDIOMETRÍA
                        objCE.aud_HipoacusiaOD = objHistorialClinicoDTO.aud_HipoacusiaOD;
                        objCE.aud_HipoacusiaOI = objHistorialClinicoDTO.aud_HipoacusiaOI;
                        objCE.aud_HBC = objHistorialClinicoDTO.aud_HBC;
                        objCE.aud_Diagnostico = objHistorialClinicoDTO.aud_Diagnostico;
                        objCE.aud_KH1 = objHistorialClinicoDTO.aud_KH1;
                        objCE.aud_KH1_OI = objHistorialClinicoDTO.aud_KH1_OI;
                        objCE.aud_KH1_OD = objHistorialClinicoDTO.aud_KH1_OD;
                        objCE.aud_KH2 = objHistorialClinicoDTO.aud_KH2;
                        objCE.aud_KH2_OI = objHistorialClinicoDTO.aud_KH2_OI;
                        objCE.aud_KH2_OD = objHistorialClinicoDTO.aud_KH2_OD;
                        objCE.aud_KH3 = objHistorialClinicoDTO.aud_KH3;
                        objCE.aud_KH3_OI = objHistorialClinicoDTO.aud_KH3_OI;
                        objCE.aud_KH3_OD = objHistorialClinicoDTO.aud_KH3_OD;
                        objCE.aud_KH4 = objHistorialClinicoDTO.aud_KH4;
                        objCE.aud_KH4_OI = objHistorialClinicoDTO.aud_KH4_OI;
                        objCE.aud_KH4_OD = objHistorialClinicoDTO.aud_KH4_OD;
                        objCE.aud_KH5 = objHistorialClinicoDTO.aud_KH5;
                        objCE.aud_KH5_OI = objHistorialClinicoDTO.aud_KH5_OI;
                        objCE.aud_KH5_OD = objHistorialClinicoDTO.aud_KH5_OD;
                        objCE.aud_KH6 = objHistorialClinicoDTO.aud_KH6;
                        objCE.aud_KH6_OI = objHistorialClinicoDTO.aud_KH6_OI;
                        objCE.aud_KH6_OD = objHistorialClinicoDTO.aud_KH6_OD;
                        objCE.aud_KH7 = objHistorialClinicoDTO.aud_KH7;
                        objCE.aud_KH7_OI = objHistorialClinicoDTO.aud_KH7_OI;
                        objCE.aud_KH7_OD = objHistorialClinicoDTO.aud_KH7_OD;
                        objCE.aud_NotasAudiometria = objHistorialClinicoDTO.aud_NotasAudiometria;
                        #endregion

                        #region ELECTROCARDIOGRAMA 12 DERIVACIONES
                        objCE.eleDer_Interpretacion = objHistorialClinicoDTO.eleDer_Interpretacion;
                        #endregion

                        #region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
                        objCE.radTCLP_Conclusiones = objHistorialClinicoDTO.radTCLP_Conclusiones;
                        #endregion

                        #region CERTIFICADO MEDICO
                        objCE.certMed_CertificadoMedico = objHistorialClinicoDTO.certMed_CertificadoMedico;
                        objCE.certMed_AptitudID = objHistorialClinicoDTO.certMed_AptitudID;
                        objCE.certMed_Fecha = objHistorialClinicoDTO.certMed_Fecha;
                        objCE.certMed_NombrePaciente = objHistorialClinicoDTO.certMed_NombrePaciente;
                        #endregion

                        #region RECOMENDACIÓN
                        objCE.recom_Recomendaciones = objHistorialClinicoDTO.recom_Recomendaciones;
                        #endregion

                        #region SE REGISTRA EL USUARIO QUE ACTUALIZO EL HISTORIAL CLINICO
                        objCE.cp_esVoBo = 0;
                        objCE.medicoCreacionID = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaModificacion = DateTime.Now;
                        #endregion

                        _context.SaveChanges();

                        #region SE REGISTRA LOS ANEXOS
                        #region DATOS PERSONALES
                        if (lstArchivosDatosPersonales != null)
                        {
                            for (int i = 0; i < lstArchivosDatosPersonales.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.imagenPersona, lstArchivosDatosPersonales[i], "ImagenPersona-");
                            }
                        }
                        #endregion
                        #region ESPIROMETRIA
                        if (lstArchivosEspirometria != null)
                        {
                            for (int i = 0; i < lstArchivosEspirometria.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.espirometria, lstArchivosEspirometria[i], "Espirometria-");
                            }
                        }
                        #endregion
                        #region AUDIOMETRIA
                        if (lstArchivosAudiometria != null)
                        {
                            for (int i = 0; i < lstArchivosAudiometria.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.audiometria, lstArchivosAudiometria[i], "Audiometria-");
                            }
                        }
                        #endregion
                        #region ELECTROCARDIOGRAMA
                        if (lstArchivosElectrocardiograma != null)
                        {
                            for (int i = 0; i < lstArchivosElectrocardiograma.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.electrocardiograma, lstArchivosElectrocardiograma[i], "Electrocardiograma-");
                            }
                        }
                        #endregion
                        #region RADIOGRAFIAS
                        if (lstArchivosRadiografias != null)
                        {
                            for (int i = 0; i < lstArchivosRadiografias.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.radiografia, lstArchivosRadiografias[i], "Radiografias-");
                            }
                        }
                        #endregion
                        #region LABORATORIO
                        if (lstArchivosLaboratorio != null)
                        {
                            for (int i = 0; i < lstArchivosLaboratorio.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.laboratorio, lstArchivosLaboratorio[i], "Laboratorio-");
                            }
                        }
                        #endregion
                        #region DOCUMENTOS ADJUNTOS
                        if (lstArchivosDocumentosAdjuntos != null)
                        {
                            for (int i = 0; i < lstArchivosDocumentosAdjuntos.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.documentosAdjuntos, lstArchivosDocumentosAdjuntos[i], "DocumentosAdjuntos-");
                            }
                        }
                        #endregion
                        #endregion


                        resultado = new Dictionary<string, object>();
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        resultado.Add(SUCCESS, true);
                        #endregion
                    }
                    else if (objHistorialClinicoDTO.id == 0)
                    {
                        #region SE GUARDA LOS DATOS DEL HISTORIAL CLINICO
                        
                        #region DATOS PERSONALES
                        Dictionary<string, object> objFolio = new Dictionary<string, object>();
                        objFolio = GetUltimoFolioHistorialClinico();

                        objCE.folio = objFolio["folio"] as string;
                        objCE.dtsPer_EmpresaID = objHistorialClinicoDTO.dtsPer_EmpresaID;
                        objCE.dtsPer_CCID = objHistorialClinicoDTO.dtsPer_CCID;
                        objCE.dtsPer_Paciente = objHistorialClinicoDTO.dtsPer_Paciente;
                        objCE.dtsPer_FechaHora = objHistorialClinicoDTO.dtsPer_FechaHora;
                        objCE.dtsPer_FechaNac = objHistorialClinicoDTO.dtsPer_FechaNac;
                        objCE.dtsPer_Sexo = objHistorialClinicoDTO.dtsPer_Sexo;
                        objCE.dtsPer_EstadoCivilID = objHistorialClinicoDTO.dtsPer_EstadoCivilID;
                        objCE.dtsPer_TipoSangreID = objHistorialClinicoDTO.dtsPer_TipoSangreID;
                        objCE.dtsPer_CURP = objHistorialClinicoDTO.dtsPer_CURP;
                        objCE.dtsPer_Domicilio = objHistorialClinicoDTO.dtsPer_Domicilio;
                        objCE.dtsPer_Ciudad = objHistorialClinicoDTO.dtsPer_Ciudad;
                        objCE.dtsPer_EscolaridadID = objHistorialClinicoDTO.dtsPer_EscolaridadID;
                        objCE.dtsPer_LugarNac = objHistorialClinicoDTO.dtsPer_LugarNac;
                        objCE.dtsPer_Telefono = objHistorialClinicoDTO.dtsPer_Telefono;
                        #endregion

                        #region MOTIVO DE LA EVALUACIÓN
                        objCE.motEva_esIngreso = objHistorialClinicoDTO.motEva_esIngreso;
                        objCE.motEva_esRetiro = objHistorialClinicoDTO.motEva_esRetiro;
                        objCE.motEva_esEvaOpcional = objHistorialClinicoDTO.motEva_esEvaOpcional;
                        objCE.motEva_esPostIncapacidad = objHistorialClinicoDTO.motEva_esPostIncapacidad;
                        objCE.motEva_esReubicacion = objHistorialClinicoDTO.motEva_esReubicacion;
                        #endregion

                        #region ANTECEDENTES LABORALES
                        objCE.antLab_Puesto = objHistorialClinicoDTO.antLab_Puesto;
                        objCE.antLab_Empresa = objHistorialClinicoDTO.antLab_Empresa;
                        objCE.antLab_Desde = objHistorialClinicoDTO.antLab_Desde;
                        objCE.antLab_Hasta = objHistorialClinicoDTO.antLab_Hasta;
                        objCE.antLab_Turno = objHistorialClinicoDTO.antLab_Turno;
                        objCE.antLab_esDePie = objHistorialClinicoDTO.antLab_esDePie;
                        objCE.antLab_esInclinado = objHistorialClinicoDTO.antLab_esInclinado;
                        objCE.antLab_esSentado = objHistorialClinicoDTO.antLab_esSentado;
                        objCE.antLab_esArrodillado = objHistorialClinicoDTO.antLab_esArrodillado;
                        objCE.antLab_esCaminando = objHistorialClinicoDTO.antLab_esCaminando;
                        objCE.antLab_esOtra = objHistorialClinicoDTO.antLab_esOtra;
                        objCE.antLab_Cual = objHistorialClinicoDTO.antLab_Cual;
                        #endregion

                        #region ACCIDENTES Y ENFERMEDADES DE TRABAJO
                        objCE.accET_Empresa = objHistorialClinicoDTO.accET_Empresa;
                        objCE.accET_Anio = objHistorialClinicoDTO.accET_Anio;
                        objCE.accET_LesionAreaAnatomica = objHistorialClinicoDTO.accET_LesionAreaAnatomica;
                        objCE.accET_Secuelas = objHistorialClinicoDTO.accET_Secuelas;
                        objCE.accET_Cuales = objHistorialClinicoDTO.accET_Cuales;
                        objCE.accET_ExamNoAceptables = objHistorialClinicoDTO.accET_ExamNoAceptables;
                        objCE.accET_Causas = objHistorialClinicoDTO.accET_Causas;
                        objCE.accET_AbandonoTrabajo = objHistorialClinicoDTO.accET_AbandonoTrabajo;
                        objCE.accET_IncapacidadFrecuente = objHistorialClinicoDTO.accET_IncapacidadFrecuente;
                        objCE.accET_Prolongadas = objHistorialClinicoDTO.accET_Prolongadas;
                        #endregion

                        #region USO DE ELEMENTOS DE PROTECCIÓN PERSONAL
                        objCE.usoElePP_esActual = objHistorialClinicoDTO.usoElePP_esActual;
                        objCE.usoElePP_esCasco = objHistorialClinicoDTO.usoElePP_esCasco;
                        objCE.usoElePP_esTapaboca = objHistorialClinicoDTO.usoElePP_esTapaboca;
                        objCE.usoElePP_esGafas = objHistorialClinicoDTO.usoElePP_esGafas;
                        objCE.usoElePP_esRespirador = objHistorialClinicoDTO.usoElePP_esRespirador;
                        objCE.usoElePP_esBotas = objHistorialClinicoDTO.usoElePP_esBotas;
                        objCE.usoElePP_esAuditivos = objHistorialClinicoDTO.usoElePP_esAuditivos;
                        objCE.usoElePP_esOverol = objHistorialClinicoDTO.usoElePP_esOverol;
                        objCE.usoElePP_esGuantes = objHistorialClinicoDTO.usoElePP_esGuantes;
                        objCE.usoElePP_OtroCual = objHistorialClinicoDTO.usoElePP_OtroCual;
                        objCE.usoElePP_DeberiaRecibir = objHistorialClinicoDTO.usoElePP_DeberiaRecibir;
                        objCE.usoElePP_ConsideraAdecuado = objHistorialClinicoDTO.usoElePP_ConsideraAdecuado;
                        #endregion

                        #region ANTECEDENTES FAMILIARES
                        objCE.antFam_esTuberculosis = objHistorialClinicoDTO.antFam_esTuberculosis;
                        objCE.antFam_TuberculosisParentesco = objHistorialClinicoDTO.antFam_TuberculosisParentesco;
                        objCE.antFam_esHTA = objHistorialClinicoDTO.antFam_esHTA;
                        objCE.antFam_HTAParentesco = objHistorialClinicoDTO.antFam_HTAParentesco;
                        objCE.antFam_esDiabetes = objHistorialClinicoDTO.antFam_esDiabetes;
                        objCE.antFam_DiabetesParentesco = objHistorialClinicoDTO.antFam_DiabetesParentesco;
                        objCE.antFam_esACV = objHistorialClinicoDTO.antFam_esACV;
                        objCE.antFam_ACVParentesco = objHistorialClinicoDTO.antFam_ACVParentesco;
                        objCE.antFam_esInfarto = objHistorialClinicoDTO.antFam_esInfarto;
                        objCE.antFam_InfartoParentesco = objHistorialClinicoDTO.antFam_InfartoParentesco;
                        objCE.antFam_esAsma = objHistorialClinicoDTO.antFam_esAsma;
                        objCE.antFam_AsmaParentesco = objHistorialClinicoDTO.antFam_AsmaParentesco;
                        objCE.antFam_esAlergias = objHistorialClinicoDTO.antFam_esAlergias;
                        objCE.antFam_AlergiasParentesco = objHistorialClinicoDTO.antFam_AlergiasParentesco;
                        objCE.antFam_esMental = objHistorialClinicoDTO.antFam_esMental;
                        objCE.antFam_MentalParentesco = objHistorialClinicoDTO.antFam_MentalParentesco;
                        objCE.antFam_esCancer = objHistorialClinicoDTO.antFam_esCancer;
                        objCE.antFam_CancerParentesco = objHistorialClinicoDTO.antFam_CancerParentesco;
                        objCE.antFam_Observaciones = objHistorialClinicoDTO.antFam_Observaciones;
                        #endregion

                        #region ANTECEDENTES PERSONALES NO PATOLÓGICOS
                        objCE.antPerNoPat_Tabaquismo = objHistorialClinicoDTO.antPerNoPat_Tabaquismo;
                        objCE.antPerNoPat_CigarroDia = objHistorialClinicoDTO.antPerNoPat_CigarroDia;
                        objCE.antPerNoPat_CigarroAnios = objHistorialClinicoDTO.antPerNoPat_CigarroAnios;
                        objCE.antPerNoPat_Alcoholismo = objHistorialClinicoDTO.antPerNoPat_Alcoholismo;
                        objCE.antPerNoPat_AlcoholismoAnios = objHistorialClinicoDTO.antPerNoPat_AlcoholismoAnios;
                        objCE.antPerNoPat_esDrogadiccion = objHistorialClinicoDTO.antPerNoPat_esDrogadiccion;
                        objCE.antPerNoPat_esMarihuana = objHistorialClinicoDTO.antPerNoPat_esMarihuana;
                        objCE.antPerNoPat_esCocaina = objHistorialClinicoDTO.antPerNoPat_esCocaina;
                        objCE.antPerNoPat_esAnfetaminas = objHistorialClinicoDTO.antPerNoPat_esAnfetaminas;
                        objCE.antPerNoPat_Otros = objHistorialClinicoDTO.antPerNoPat_Otros;
                        objCE.antPerNoPat_Inmunizaciones = objHistorialClinicoDTO.antPerNoPat_Inmunizaciones;
                        objCE.antPerNoPat_Tetanicos = objHistorialClinicoDTO.antPerNoPat_Tetanicos;
                        objCE.antPerNoPat_FechaAntitetanica = objHistorialClinicoDTO.antPerNoPat_FechaAntitetanica;
                        objCE.antPerNoPat_Hepatitis = objHistorialClinicoDTO.antPerNoPat_Hepatitis;
                        objCE.antPerNoPat_Influenza = objHistorialClinicoDTO.antPerNoPat_Influenza;
                        objCE.antPerNoPat_FechaInfluenza = objHistorialClinicoDTO.antPerNoPat_FechaInfluenza;
                        objCE.antPerNoPat_Infancia = objHistorialClinicoDTO.antPerNoPat_Infancia;
                        objCE.antPerNoPat_DescInfancia = objHistorialClinicoDTO.antPerNoPat_DescInfancia;
                        objCE.antPerNoPat_Alimentacion = objHistorialClinicoDTO.antPerNoPat_Alimentacion;
                        objCE.antPerNoPat_Higiene = objHistorialClinicoDTO.antPerNoPat_Higiene;
                        objCE.antPerNoPat_MedicacionActual = objHistorialClinicoDTO.antPerNoPat_MedicacionActual;
                        #endregion

                        #region ANTECEDENTES PERSONALES PATOLÓGICOS
                        objCE.antPerPat_esNeoplasicos = objHistorialClinicoDTO.antPerPat_esNeoplasicos;
                        objCE.antPerPat_esNeumopatias = objHistorialClinicoDTO.antPerPat_esNeumopatias;
                        objCE.antPerPat_esAsma = objHistorialClinicoDTO.antPerPat_esAsma;
                        objCE.antPerPat_esFimico = objHistorialClinicoDTO.antPerPat_esFimico;
                        objCE.antPerPat_esNeumoconiosis = objHistorialClinicoDTO.antPerPat_esNeumoconiosis;
                        objCE.antPerPat_esCardiopatias = objHistorialClinicoDTO.antPerPat_esCardiopatias;
                        objCE.antPerPat_esReumaticos = objHistorialClinicoDTO.antPerPat_esReumaticos;
                        objCE.antPerPat_esAlergias = objHistorialClinicoDTO.antPerPat_esAlergias;
                        objCE.antPerPat_esHipertension = objHistorialClinicoDTO.antPerPat_esHipertension;
                        objCE.antPerPat_esHepatitis = objHistorialClinicoDTO.antPerPat_esHepatitis;
                        objCE.antPerPat_esTifoidea = objHistorialClinicoDTO.antPerPat_esTifoidea;
                        objCE.antPerPat_esHernias = objHistorialClinicoDTO.antPerPat_esHernias;
                        objCE.antPerPat_esLumbalgias = objHistorialClinicoDTO.antPerPat_esLumbalgias;
                        objCE.antPerPat_esDiabetes = objHistorialClinicoDTO.antPerPat_esDiabetes;
                        objCE.antPerPat_esEpilepsias = objHistorialClinicoDTO.antPerPat_esEpilepsias;
                        objCE.antPerPat_esVenereas = objHistorialClinicoDTO.antPerPat_esVenereas;
                        objCE.antPerPat_esCirugias = objHistorialClinicoDTO.antPerPat_esCirugias;
                        objCE.antPerPat_esFracturas = objHistorialClinicoDTO.antPerPat_esFracturas;
                        objCE.antPerPat_ObservacionesPat = objHistorialClinicoDTO.antPerPat_ObservacionesPat;
                        #endregion

                        #region INTERROGATORIO POR APARATOS Y SISTEMAS
                        objCE.intApaSis_esRespiratorio = objHistorialClinicoDTO.intApaSis_esRespiratorio;
                        objCE.intApaSis_esDigestivo = objHistorialClinicoDTO.intApaSis_esDigestivo;
                        objCE.intApaSis_esCardiovascular = objHistorialClinicoDTO.intApaSis_esCardiovascular;
                        objCE.intApaSis_esNervioso = objHistorialClinicoDTO.intApaSis_esNervioso;
                        objCE.intApaSis_esUrinario = objHistorialClinicoDTO.intApaSis_esUrinario;
                        objCE.intApaSis_esEndocrino = objHistorialClinicoDTO.intApaSis_esEndocrino;
                        objCE.intApaSis_esPsiquiatrico = objHistorialClinicoDTO.intApaSis_esPsiquiatrico;
                        objCE.intApaSis_esEsqueletico = objHistorialClinicoDTO.intApaSis_esEsqueletico;
                        objCE.intApaSis_esAudicion = objHistorialClinicoDTO.intApaSis_esAudicion;
                        objCE.intApaSis_esVision = objHistorialClinicoDTO.intApaSis_esVision;
                        objCE.intApaSis_esOlfato = objHistorialClinicoDTO.intApaSis_esOlfato;
                        objCE.intApaSis_esTacto = objHistorialClinicoDTO.intApaSis_esTacto;
                        objCE.intApaSis_ObservacionesPat = objHistorialClinicoDTO.intApaSis_ObservacionesPat;
                        #endregion

                        #region PADECIMIENTOS ACTUALES
                        objCE.padAct_PadActuales = objHistorialClinicoDTO.padAct_PadActuales;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-SIGNOS VITALES
                        objCE.expFSV_TArterial = objHistorialClinicoDTO.expFSV_TArterial;
                        objCE.expFSV_Pulso = objHistorialClinicoDTO.expFSV_Pulso;
                        objCE.expFSV_Temp = objHistorialClinicoDTO.expFSV_Temp;
                        objCE.expFSV_FCardiaca = objHistorialClinicoDTO.expFSV_FCardiaca;
                        objCE.expFSV_FResp = objHistorialClinicoDTO.expFSV_FResp;
                        objCE.expFSV_Peso = objHistorialClinicoDTO.expFSV_Peso;
                        objCE.expFSV_Talla = objHistorialClinicoDTO.expFSV_Talla;
                        objCE.expFSV_IMC = objHistorialClinicoDTO.expFSV_IMC;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-CABEZA
                        objCE.expFC_Craneo = objHistorialClinicoDTO.expFC_Craneo;
                        objCE.expFC_Parpados = objHistorialClinicoDTO.expFC_Parpados;
                        objCE.expFC_Conjutiva = objHistorialClinicoDTO.expFC_Conjutiva;
                        objCE.expFC_Reflejos = objHistorialClinicoDTO.expFC_Reflejos;
                        objCE.expFC_FosasNasales = objHistorialClinicoDTO.expFC_FosasNasales;
                        objCE.expFC_Boca = objHistorialClinicoDTO.expFC_Boca;
                        objCE.expFC_Amigdalas = objHistorialClinicoDTO.expFC_Amigdalas;
                        objCE.expFC_Dentadura = objHistorialClinicoDTO.expFC_Dentadura;
                        objCE.expFC_Encias = objHistorialClinicoDTO.expFC_Encias;
                        objCE.expFC_Cuello = objHistorialClinicoDTO.expFC_Cuello;
                        objCE.expFC_Tiroides = objHistorialClinicoDTO.expFC_Tiroides;
                        objCE.expFC_Ganglios = objHistorialClinicoDTO.expFC_Ganglios;
                        objCE.expFC_Oidos = objHistorialClinicoDTO.expFC_Oidos;
                        objCE.expFC_Otros = objHistorialClinicoDTO.expFC_Otros;
                        objCE.expFC_Observaciones = objHistorialClinicoDTO.expFC_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-AGUDEZA VISUAL
                        objCE.expFAV_VisCerAmbosOjos = objHistorialClinicoDTO.expFAV_VisCerAmbosOjos;
                        objCE.expFAV_VisCerOjoIzq = objHistorialClinicoDTO.expFAV_VisCerOjoIzq;
                        objCE.expFAV_VisCerOjoDer = objHistorialClinicoDTO.expFAV_VisCerOjoDer;
                        objCE.expFAV_VisLejAmbosOjos = objHistorialClinicoDTO.expFAV_VisLejAmbosOjos;
                        objCE.expFAV_VisLejOjoIzq = objHistorialClinicoDTO.expFAV_VisLejOjoIzq;
                        objCE.expFAV_VisLejOjoDer = objHistorialClinicoDTO.expFAV_VisLejOjoDer;
                        objCE.expFAV_CorregidaAmbosOjos = objHistorialClinicoDTO.expFAV_CorregidaAmbosOjos;
                        objCE.expFAV_CorregidaOjoIzq = objHistorialClinicoDTO.expFAV_CorregidaOjoIzq;
                        objCE.expFAV_CorregidaOjoDer = objHistorialClinicoDTO.expFAV_CorregidaOjoDer;
                        objCE.expFAV_CampimetriaOI = objHistorialClinicoDTO.expFAV_CampimetriaOI;
                        objCE.expFAV_CampimetriaOD = objHistorialClinicoDTO.expFAV_CampimetriaOD;
                        objCE.expFAV_PterigionOI = objHistorialClinicoDTO.expFAV_PterigionOI;
                        objCE.expFAV_PterigionOD = objHistorialClinicoDTO.expFAV_PterigionOD;
                        objCE.expFAV_FondoOjo = objHistorialClinicoDTO.expFAV_FondoOjo;
                        objCE.expFAV_Daltonismo = objHistorialClinicoDTO.expFAV_Daltonismo;
                        objCE.expFAV_Observaciones = objHistorialClinicoDTO.expFAV_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-TORAX, ABDOMEN, TRONCO Y EXTREMIDADES
                        objCE.expFTATE_esCamposPulmonares = objHistorialClinicoDTO.expFTATE_esCamposPulmonares;
                        objCE.expFTATE_esPuntosDolorosos = objHistorialClinicoDTO.expFTATE_esPuntosDolorosos;
                        objCE.expFTATE_esGenitales = objHistorialClinicoDTO.expFTATE_esGenitales;
                        objCE.expFTATE_esRuidosCardiacos = objHistorialClinicoDTO.expFTATE_esRuidosCardiacos;
                        objCE.expFTATE_esHallusValgus = objHistorialClinicoDTO.expFTATE_esHallusValgus;
                        objCE.expFTATE_esHerniasUmbili = objHistorialClinicoDTO.expFTATE_esHerniasUmbili;
                        objCE.expFTATE_esAreaRenal = objHistorialClinicoDTO.expFTATE_esAreaRenal;
                        objCE.expFTATE_esVaricocele = objHistorialClinicoDTO.expFTATE_esVaricocele;
                        objCE.expFTATE_esGrandulasMamarias = objHistorialClinicoDTO.expFTATE_esGrandulasMamarias;
                        objCE.expFTATE_esColumnaVertebral = objHistorialClinicoDTO.expFTATE_esColumnaVertebral;
                        objCE.expFTATE_esPiePlano = objHistorialClinicoDTO.expFTATE_esPiePlano;
                        objCE.expFTATE_esVarices = objHistorialClinicoDTO.expFTATE_esVarices;
                        objCE.expFTATE_esMiembrosSup = objHistorialClinicoDTO.expFTATE_esMiembrosSup;
                        objCE.expFTATE_esParedAbdominal = objHistorialClinicoDTO.expFTATE_esParedAbdominal;
                        objCE.expFTATE_esAnillosInguinales = objHistorialClinicoDTO.expFTATE_esAnillosInguinales;
                        objCE.expFTATE_esMiembrosInf = objHistorialClinicoDTO.expFTATE_esMiembrosInf;
                        objCE.expFTATE_esTatuajes = objHistorialClinicoDTO.expFTATE_esTatuajes;
                        objCE.expFTATE_esVisceromegalias = objHistorialClinicoDTO.expFTATE_esVisceromegalias;
                        objCE.expFTATE_esMarcha = objHistorialClinicoDTO.expFTATE_esMarcha;
                        objCE.expFTATE_esHerniasInguinales = objHistorialClinicoDTO.expFTATE_esHerniasInguinales;
                        objCE.expFTATE_esHombrosDolorosos = objHistorialClinicoDTO.expFTATE_esHombrosDolorosos;
                        objCE.expFTATE_esQuistes = objHistorialClinicoDTO.expFTATE_esQuistes;
                        objCE.expFTATE_Observaciones = objHistorialClinicoDTO.expFTATE_Observaciones;
                        objCE.expFTATE_MS_HombroDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esFlexion;
                        objCE.expFTATE_MS_HombroIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esFlexion;
                        objCE.expFTATE_MS_CodoDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esFlexion;
                        objCE.expFTATE_MS_CodoIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esFlexion;
                        objCE.expFTATE_MS_MunecaDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esFlexion;
                        objCE.expFTATE_MS_MunecaIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esFlexion;
                        objCE.expFTATE_MS_DedosDer_esFlexion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esFlexion;
                        objCE.expFTATE_MS_DedosIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esFlexion;
                        objCE.expFTATE_MS_HombroDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esExtension;
                        objCE.expFTATE_MS_HombroIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esExtension;
                        objCE.expFTATE_MS_CodoDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esExtension;
                        objCE.expFTATE_MS_CodoIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esExtension;
                        objCE.expFTATE_MS_MunecaDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esExtension;
                        objCE.expFTATE_MS_MunecaIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esExtension;
                        objCE.expFTATE_MS_DedosDer_esExtension = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esExtension;
                        objCE.expFTATE_MS_DedosIzq_esExtension = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esExtension;
                        objCE.expFTATE_MS_HombroDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esAbduccion;
                        objCE.expFTATE_MS_HombroIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esAbduccion;
                        objCE.expFTATE_MS_CodoDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esAbduccion;
                        objCE.expFTATE_MS_CodoIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esAbduccion;
                        objCE.expFTATE_MS_MunecaDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esAbduccion;
                        objCE.expFTATE_MS_MunecaIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esAbduccion;
                        objCE.expFTATE_MS_DedosDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esAbduccion;
                        objCE.expFTATE_MS_DedosIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esAbduccion;
                        objCE.expFTATE_MS_HombroDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esAduccion;
                        objCE.expFTATE_MS_HombroIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esAduccion;
                        objCE.expFTATE_MS_MunecaDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esAduccion;
                        objCE.expFTATE_MS_MunecaIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esAduccion;
                        objCE.expFTATE_MS_DedosDer_esAduccion = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esAduccion;
                        objCE.expFTATE_MS_DedosIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esAduccion;
                        objCE.expFTATE_MS_HombroDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esRotInterna;
                        objCE.expFTATE_MS_HombroIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esRotInterna;
                        objCE.expFTATE_MS_MunecaDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esRotInterna;
                        objCE.expFTATE_MS_MunecaIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esRotInterna;
                        objCE.expFTATE_MS_DedosDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esRotInterna;
                        objCE.expFTATE_MS_DedosIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esRotInterna;
                        objCE.expFTATE_MS_HombroDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_HombroDer_esRotExterna;
                        objCE.expFTATE_MS_HombroIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_HombroIzq_esRotExterna;
                        objCE.expFTATE_MS_MunecaDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esRotExterna;
                        objCE.expFTATE_MS_MunecaIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esRotExterna;
                        objCE.expFTATE_MS_DedosDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esRotExterna;
                        objCE.expFTATE_MS_DedosIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esRotExterna;
                        objCE.expFTATE_MS_CodoDer_esPronacion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esPronacion;
                        objCE.expFTATE_MS_CodoIzq_esPronacion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esPronacion;
                        objCE.expFTATE_MS_MunecaDer_esPronacion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esPronacion;
                        objCE.expFTATE_MS_MunecaIzq_esPronacion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esPronacion;
                        objCE.expFTATE_MS_CodoDer_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_CodoDer_esSupinacion;
                        objCE.expFTATE_MS_CodoIzq_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_CodoIzq_esSupinacion;
                        objCE.expFTATE_MS_MunecaDer_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esSupinacion;
                        objCE.expFTATE_MS_MunecaIzq_esSupinacion = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esSupinacion;
                        objCE.expFTATE_MS_MunecaDer_esDesvUlnar = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esDesvUlnar;
                        objCE.expFTATE_MS_MunecaIzq_esDesvUlnar = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esDesvUlnar;
                        objCE.expFTATE_MS_MunecaDer_esDesvRadial = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esDesvRadial;
                        objCE.expFTATE_MS_MunecaIzq_esDesvRadial = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esDesvRadial;
                        objCE.expFTATE_MS_MunecaDer_esOponencia = objHistorialClinicoDTO.expFTATE_MS_MunecaDer_esOponencia;
                        objCE.expFTATE_MS_MunecaIzq_esOponencia = objHistorialClinicoDTO.expFTATE_MS_MunecaIzq_esOponencia;
                        objCE.expFTATE_MS_DedosDer_esOponencia = objHistorialClinicoDTO.expFTATE_MS_DedosDer_esOponencia;
                        objCE.expFTATE_MS_DedosIzq_esOponencia = objHistorialClinicoDTO.expFTATE_MS_DedosIzq_esOponencia;
                        objCE.expFTATE_MI_CaderaDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esFlexion;
                        objCE.expFTATE_MI_CaderaIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esFlexion;
                        objCE.expFTATE_MI_RodillasDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esFlexion;
                        objCE.expFTATE_MI_RodillasIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esFlexion;
                        objCE.expFTATE_MI_CllPieDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esFlexion;
                        objCE.expFTATE_MI_CllPieIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esFlexion;
                        objCE.expFTATE_MI_DedosDer_esFlexion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esFlexion;
                        objCE.expFTATE_MI_DedosIzq_esFlexion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esFlexion;
                        objCE.expFTATE_MI_CaderaDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esExtension;
                        objCE.expFTATE_MI_CaderaIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esExtension;
                        objCE.expFTATE_MI_RodillasDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esExtension;
                        objCE.expFTATE_MI_RodillasIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esExtension;
                        objCE.expFTATE_MI_CllPieDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esExtension;
                        objCE.expFTATE_MI_CllPieIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esExtension;
                        objCE.expFTATE_MI_DedosDer_esExtension = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esExtension;
                        objCE.expFTATE_MI_DedosIzq_esExtension = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esExtension;
                        objCE.expFTATE_MI_CaderaDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esAbduccion;
                        objCE.expFTATE_MI_CaderaIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esAbduccion;
                        objCE.expFTATE_MI_CllPieDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esAbduccion;
                        objCE.expFTATE_MI_CllPieIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esAbduccion;
                        objCE.expFTATE_MI_DedosDer_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esAbduccion;
                        objCE.expFTATE_MI_DedosIzq_esAbduccion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esAbduccion;
                        objCE.expFTATE_MI_CaderaDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esAduccion;
                        objCE.expFTATE_MI_CaderaIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esAduccion;
                        objCE.expFTATE_MI_CllPieDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esAduccion;
                        objCE.expFTATE_MI_CllPieIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esAduccion;
                        objCE.expFTATE_MI_DedosDer_esAduccion = objHistorialClinicoDTO.expFTATE_MI_DedosDer_esAduccion;
                        objCE.expFTATE_MI_DedosIzq_esAduccion = objHistorialClinicoDTO.expFTATE_MI_DedosIzq_esAduccion;
                        objCE.expFTATE_MI_CaderaDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esRotInterna;
                        objCE.expFTATE_MI_CaderaIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esRotInterna;
                        objCE.expFTATE_MI_RodillasDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esRotInterna;
                        objCE.expFTATE_MI_RodillasIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esRotInterna;
                        objCE.expFTATE_MI_CllPieDer_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esRotInterna;
                        objCE.expFTATE_MI_CllPieIzq_esRotInterna = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esRotInterna;
                        objCE.expFTATE_MI_CaderaDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CaderaDer_esRotExterna;
                        objCE.expFTATE_MI_CaderaIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CaderaIzq_esRotExterna;
                        objCE.expFTATE_MI_RodillasDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_RodillasDer_esRotExterna;
                        objCE.expFTATE_MI_RodillasIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_RodillasIzq_esRotExterna;
                        objCE.expFTATE_MI_CllPieDer_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esRotExterna;
                        objCE.expFTATE_MI_CllPieIzq_esRotExterna = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esRotExterna;
                        objCE.expFTATE_MI_CllPieDer_esInversion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esInversion;
                        objCE.expFTATE_MI_CllPieIzq_esInversion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esInversion;
                        objCE.expFTATE_MI_CllPieDer_esEversion = objHistorialClinicoDTO.expFTATE_MI_CllPieDer_esEversion;
                        objCE.expFTATE_MI_CllPieIzq_esEversion = objHistorialClinicoDTO.expFTATE_MI_CllPieIzq_esEversion;
                        objCE.expFTATE_MS_MI_Observaciones = objHistorialClinicoDTO.expFTATE_MS_MI_Observaciones;
                        #endregion

                        #region EXPLORACIÓN FÍSICA-DATOS GENERALES
                        objCE.expFDT_PielMucosas = objHistorialClinicoDTO.expFDT_PielMucosas;
                        objCE.expFDT_EstadoPsiquiatrico = objHistorialClinicoDTO.expFDT_EstadoPsiquiatrico;
                        objCE.expFDT_ExamenNeurologico = objHistorialClinicoDTO.expFDT_ExamenNeurologico;
                        objCE.expFDT_FobiasActuales = objHistorialClinicoDTO.expFDT_FobiasActuales;
                        objCE.expFDT_Higiene = objHistorialClinicoDTO.expFDT_Higiene;
                        objCE.expFDT_ConstitucionFisica = objHistorialClinicoDTO.expFDT_ConstitucionFisica;
                        objCE.expFDT_Otros = objHistorialClinicoDTO.expFDT_Otros;
                        objCE.expFDT_Observaciones = objHistorialClinicoDTO.expFDT_Observaciones;
                        #endregion

                        #region ESTUDIOS DE GABINETE
                        objCE.estGab_TipoSanguineoID = objHistorialClinicoDTO.estGab_TipoSanguineoID;
                        objCE.estGab_Antidoping = objHistorialClinicoDTO.estGab_Antidoping;
                        objCE.estGab_Laboratorios = objHistorialClinicoDTO.estGab_Laboratorios;
                        objCE.estGab_ObservacionesGrupoRH = objHistorialClinicoDTO.estGab_ObservacionesGrupoRH;
                        objCE.estGab_ExamGenOrina = objHistorialClinicoDTO.estGab_ExamGenOrina;
                        objCE.estGab_ExamGenOrinaObservaciones = objHistorialClinicoDTO.estGab_ExamGenOrinaObservaciones;
                        objCE.estGab_Radiografias = objHistorialClinicoDTO.estGab_Radiografias;
                        objCE.estGab_RadiografiasObservaciones = objHistorialClinicoDTO.estGab_RadiografiasObservaciones;
                        objCE.estGab_Audiometria = objHistorialClinicoDTO.estGab_Audiometria;
                        objCE.estGab_AudiometriaObservaciones = objHistorialClinicoDTO.estGab_AudiometriaObservaciones;
                        objCE.estGab_HBC = objHistorialClinicoDTO.estGab_HBC;
                        objCE.estGab_Espirometria = objHistorialClinicoDTO.estGab_Espirometria;
                        objCE.estGab_EspirometriaObservaciones = objHistorialClinicoDTO.estGab_EspirometriaObservaciones;
                        objCE.estGab_Electrocardiograma = objHistorialClinicoDTO.estGab_Electrocardiograma;
                        objCE.estGab_ElectrocardiogramaObservaciones = objHistorialClinicoDTO.estGab_ElectrocardiogramaObservaciones;
                        objCE.estGab_FechaPrimeraDosis = objHistorialClinicoDTO.estGab_FechaPrimeraDosis;
                        objCE.estGab_FechaSegundaDosis = objHistorialClinicoDTO.estGab_FechaSegundaDosis;
                        objCE.estGab_MarcaDosisID = objHistorialClinicoDTO.estGab_MarcaDosisID;
                        objCE.estGab_VacunacionObservaciones = objHistorialClinicoDTO.estGab_VacunacionObservaciones;
                        objCE.estGab_LstProblemas = objHistorialClinicoDTO.estGab_LstProblemas;
                        objCE.estGab_Recomendaciones = objHistorialClinicoDTO.estGab_Recomendaciones;
                        #endregion

                        #region ESPIROMETRÍA
                        objCE.esp_Espirometria = objHistorialClinicoDTO.esp_Espirometria;
                        objCE.esp_EspirometriaObservaciones = objHistorialClinicoDTO.esp_EspirometriaObservaciones;
                        #endregion

                        #region AUDIOMETRÍA
                        objCE.aud_HipoacusiaOD = objHistorialClinicoDTO.aud_HipoacusiaOD;
                        objCE.aud_HipoacusiaOI = objHistorialClinicoDTO.aud_HipoacusiaOI;
                        objCE.aud_HBC = objHistorialClinicoDTO.aud_HBC;
                        objCE.aud_Diagnostico = objHistorialClinicoDTO.aud_Diagnostico;
                        objCE.aud_KH1 = objHistorialClinicoDTO.aud_KH1;
                        objCE.aud_KH1_OI = objHistorialClinicoDTO.aud_KH1_OI;
                        objCE.aud_KH1_OD = objHistorialClinicoDTO.aud_KH1_OD;
                        objCE.aud_KH2 = objHistorialClinicoDTO.aud_KH2;
                        objCE.aud_KH2_OI = objHistorialClinicoDTO.aud_KH2_OI;
                        objCE.aud_KH2_OD = objHistorialClinicoDTO.aud_KH2_OD;
                        objCE.aud_KH3 = objHistorialClinicoDTO.aud_KH3;
                        objCE.aud_KH3_OI = objHistorialClinicoDTO.aud_KH3_OI;
                        objCE.aud_KH3_OD = objHistorialClinicoDTO.aud_KH3_OD;
                        objCE.aud_KH4 = objHistorialClinicoDTO.aud_KH4;
                        objCE.aud_KH4_OI = objHistorialClinicoDTO.aud_KH4_OI;
                        objCE.aud_KH4_OD = objHistorialClinicoDTO.aud_KH4_OD;
                        objCE.aud_KH5 = objHistorialClinicoDTO.aud_KH5;
                        objCE.aud_KH5_OI = objHistorialClinicoDTO.aud_KH5_OI;
                        objCE.aud_KH5_OD = objHistorialClinicoDTO.aud_KH5_OD;
                        objCE.aud_KH6 = objHistorialClinicoDTO.aud_KH6;
                        objCE.aud_KH6_OI = objHistorialClinicoDTO.aud_KH6_OI;
                        objCE.aud_KH6_OD = objHistorialClinicoDTO.aud_KH6_OD;
                        objCE.aud_KH7 = objHistorialClinicoDTO.aud_KH7;
                        objCE.aud_KH7_OI = objHistorialClinicoDTO.aud_KH7_OI;
                        objCE.aud_KH7_OD = objHistorialClinicoDTO.aud_KH7_OD;
                        objCE.aud_NotasAudiometria = objHistorialClinicoDTO.aud_NotasAudiometria;
                        #endregion

                        #region ELECTROCARDIOGRAMA 12 DERIVACIONES
                        objCE.eleDer_Interpretacion = objHistorialClinicoDTO.eleDer_Interpretacion;
                        #endregion

                        #region RADIOGRAFIAS - TORAX Y COLUMNA LUMBAR DOS POSICIONES
                        objCE.radTCLP_Conclusiones = objHistorialClinicoDTO.radTCLP_Conclusiones;
                        #endregion

                        #region CERTIFICADO MEDICO
                        objCE.certMed_CertificadoMedico = objHistorialClinicoDTO.certMed_CertificadoMedico;
                        objCE.certMed_AptitudID = objHistorialClinicoDTO.certMed_AptitudID;
                        objCE.certMed_Fecha = objHistorialClinicoDTO.certMed_Fecha;
                        objCE.certMed_NombrePaciente = objHistorialClinicoDTO.certMed_NombrePaciente;
                        #endregion

                        #region RECOMENDACIÓN
                        objCE.recom_Recomendaciones = objHistorialClinicoDTO.recom_Recomendaciones;
                        #endregion

                        #region SE REGISTRA QUE USUARIO REALIZO EL HISTORIAL CLINICO
                        objCE.cp_esVoBo = 0;
                        objCE.medicoCreacionID = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objCE.fechaCreacion = DateTime.Now;
                        objCE.registroActivo = true;
                        #endregion

                        _context.tblS_SO_HistorialesClinicos.Add(objCE);
                        _context.SaveChanges();

                        #region SE OBTIENE ID DEL HISTORIAL CLINICO REGISTRADO PARCIALMENTE
                        idHC = _context.tblS_SO_HistorialesClinicos.Where(w => w.registroActivo).Select(s => s.id).OrderByDescending(o => o).FirstOrDefault();
                        objHistorialClinicoDTO.id = idHC;
                        #endregion

                        #region SE REGISTRA LOS ANEXOS
                        #region DATOS PERSONALES
                        if (lstArchivosDatosPersonales != null)
                        {
                            for (int i = 0; i < lstArchivosDatosPersonales.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.imagenPersona, lstArchivosDatosPersonales[i], "ImagenPersona-");
                            }
                        }
                        #endregion
                        #region ESPIROMETRIA
                        if (lstArchivosEspirometria != null)
                        {
                            for (int i = 0; i < lstArchivosEspirometria.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.espirometria, lstArchivosEspirometria[i], "Espirometria-");
                            }
                        }
                        #endregion
                        #region AUDIOMETRIA
                        if (lstArchivosAudiometria != null)
                        {
                            for (int i = 0; i < lstArchivosAudiometria.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.audiometria, lstArchivosAudiometria[i], "Audiometria-");
                            }
                        }
                        #endregion
                        #region ELECTROCARDIOGRAMA
                        if (lstArchivosElectrocardiograma != null)
                        {
                            for (int i = 0; i < lstArchivosElectrocardiograma.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.electrocardiograma, lstArchivosElectrocardiograma[i], "Electrocardiograma-");
                            }
                        }
                        #endregion
                        #region RADIOGRAFIAS
                        if (lstArchivosRadiografias != null)
                        {
                            for (int i = 0; i < lstArchivosRadiografias.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.radiografia, lstArchivosRadiografias[i], "Radiografias-");
                            }
                        }
                        #endregion
                        #region LABORATORIO
                        if (lstArchivosLaboratorio != null)
                        {
                            for (int i = 0; i < lstArchivosLaboratorio.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.laboratorio, lstArchivosLaboratorio[i], "Laboratorio-");
                            }
                        }
                        #endregion
                        #region DOCUMENTOS ADJUNTOS
                        if (lstArchivosDocumentosAdjuntos != null)
                        {
                            for (int i = 0; i < lstArchivosDocumentosAdjuntos.Count(); i++)
                            {
                                RegistrarArchivos(objHistorialClinicoDTO, TipoArchivoEnum.documentosAdjuntos, lstArchivosDocumentosAdjuntos[i], "DocumentosAdjuntos-");
                            }
                        }
                        #endregion
                        #endregion

                        resultado = new Dictionary<string, object>();
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        resultado.Add(SUCCESS, true);
                        resultado.Add("dataID", idHC);
                        #endregion
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboEscolaridades", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        private bool RegistrarArchivos(HistorialClinicoDTO objHC = null, TipoArchivoEnum tipoArchivo = 0, HttpPostedFileBase objArchivo = null, string nombreBaseArchivo = "")
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OPTIMIZA IMAGEN
                using (var imgAntes = Image.FromStream(objArchivo.InputStream))
                {
                    Bitmap myBitmap;
                    ImageCodecInfo myImageCodecInfo;
                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;

                    myBitmap = new Bitmap(imgAntes, imgAntes.Width, imgAntes.Height);
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    using (MemoryStream memStream = new MemoryStream())
                    {
                        myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                        Image newImage = Image.FromStream(memStream);
                        ImageAttributes imageAttributes = new ImageAttributes();
                        using (Graphics g = Graphics.FromImage(newImage))
                        {
                            g.InterpolationMode =
                              System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0,
                              newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                        }

                        #region SE GUARDAR EL ARCHIVO
                        List<Tuple<HttpPostedFileBase, string>> listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        var CarpetaNueva = Path.Combine(RutaHistorialClinico, objHC.dtsPer_CURP);
                        verificarExisteCarpeta(CarpetaNueva, true);

                        string nombreArchivo = ObtenerFormatoNombreArchivoA(nombreBaseArchivo, objArchivo.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(objArchivo, rutaArchivo));

                        tblS_SO_Archivos objC = new tblS_SO_Archivos();
                        objC.idHC = objHC.id;
                        objC.nombreArchivo = nombreArchivo;
                        objC.rutaArchivo = rutaArchivo;
                        objC.tipoArchivo = (int)tipoArchivo;
                        objC.fechaCreacion = DateTime.Now;
                        objC.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                        objC.registroActivo = true;
                        _context.tblS_SO_Archivos.Add(objC);
                        _context.SaveChanges();

                        newImage.Save(rutaArchivo);

                        //if (GlobalUtils.SaveHTTPPostedFile((HttpPostedFileBase)newImage, rutaArchivo) == false)
                        //    throw new Exception("Ocurrió un error al registrar el archivo.");
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, NombreControlador, "RegistrarArchivos", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return true;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private string ObtenerFormatoNombreArchivoA(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public Dictionary<string, object> GetUltimoFolioHistorialClinico()
        {
            resultado = new Dictionary<string, object>();
            try
            {
                #region SE OBTIENE EL ULTIMO FOLIO DE LOS HISTORIALES CLINICOS
                string folio = _context.Select<string>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT folio FROM tblS_SO_HistorialesClinicos WHERE registroActivo = @registroActivo ORDER BY id DESC",
                    parametros = new { registroActivo = true }
                }).FirstOrDefault();

                resultado.Add(SUCCESS, true);
                if (!string.IsNullOrEmpty(folio))
                {
                    int nuevoFolio = Convert.ToInt32(folio) + 1;
                    resultado.Add("folio", nuevoFolio.ToString());
                }
                else
                    resultado.Add("folio", 1.ToString());
                #endregion
            }
            catch (Exception e)
            {
                LogError(16, 16, NombreControlador, "GetUltimoFolioHistorialClinico", e, AccionEnum.CONSULTA, 0, 0);
                resultado.Add(MESSAGE, e.Message);
                resultado.Add(SUCCESS, false);
            }
            return resultado;
        }

        public Dictionary<string, object> GetDatosActualizarHistorialClinico(int idHistorialClinico)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LA INFORMACIÓN DEL HISTORIAL CLINICO SELECCIONADO

                    #region SE CONSTRUYE QUERY SELECT HISTORIAL CLINICO
                    string strQuery = @"SELECT id, folio, dtsPer_EmpresaID, dtsPer_CCID, dtsPer_Paciente, dtsPer_FechaHora, dtsPer_FechaNac, dtsPer_Sexo, dtsPer_EstadoCivilID, dtsPer_TipoSangreID, dtsPer_CURP, dtsPer_Domicilio, dtsPer_Ciudad,
                                                dtsPer_EscolaridadID, dtsPer_LugarNac, dtsPer_Telefono, motEva_esIngreso, motEva_esRetiro, motEva_esEvaOpcional, motEva_esPostIncapacidad, motEva_esReubicacion, antLab_Puesto, antLab_Empresa,
                                                antLab_Desde, antLab_Hasta, antLab_Turno, antLab_esDePie, antLab_esInclinado, antLab_esSentado, antLab_esArrodillado, antLab_esCaminando, antLab_esOtra, antLab_Cual, accET_Empresa,
                                                accET_Anio, accET_LesionAreaAnatomica, accET_Secuelas, accET_Cuales, accET_ExamNoAceptables, accET_Causas, accET_AbandonoTrabajo, accET_IncapacidadFrecuente, accET_Prolongadas, usoElePP_esActual,
                                                usoElePP_esCasco, usoElePP_esTapaboca, usoElePP_esGafas, usoElePP_esRespirador, usoElePP_esBotas, usoElePP_esAuditivos, usoElePP_esOverol, usoElePP_esGuantes, usoElePP_OtroCual, usoElePP_DeberiaRecibir,
                                                usoElePP_ConsideraAdecuado, antFam_esTuberculosis, antFam_TuberculosisParentesco, antFam_esHTA, antFam_HTAParentesco, antFam_esDiabetes, antFam_DiabetesParentesco, antFam_esACV, antFam_ACVParentesco,
                                                antFam_esInfarto, antFam_InfartoParentesco, antFam_esAsma, antFam_AsmaParentesco, antFam_esAlergias, antFam_AlergiasParentesco, antFam_esMental, antFam_MentalParentesco, antFam_esCancer,
                                                antFam_CancerParentesco, antFam_Observaciones, antPerNoPat_Tabaquismo, antPerNoPat_CigarroDia, antPerNoPat_CigarroAnios, antPerNoPat_Alcoholismo, antPerNoPat_AlcoholismoAnios,
                                                antPerNoPat_esDrogadiccion, antPerNoPat_esMarihuana, antPerNoPat_esCocaina, antPerNoPat_esAnfetaminas, antPerNoPat_Otros, antPerNoPat_Inmunizaciones, antPerNoPat_Tetanicos, antPerNoPat_FechaAntitetanica,
                                                antPerNoPat_Hepatitis, antPerNoPat_Influenza, antPerNoPat_FechaInfluenza, antPerNoPat_Infancia, antPerNoPat_DescInfancia, antPerNoPat_Alimentacion, antPerNoPat_Higiene, antPerNoPat_MedicacionActual,
                                                antPerPat_esNeoplasicos, antPerPat_esNeumopatias, antPerPat_esAsma, antPerPat_esFimico, antPerPat_esNeumoconiosis, antPerPat_esCardiopatias, antPerPat_esReumaticos, antPerPat_esAlergias,
                                                antPerPat_esHipertension, antPerPat_esHepatitis, antPerPat_esTifoidea, antPerPat_esHernias, antPerPat_esLumbalgias, antPerPat_esDiabetes, antPerPat_esEpilepsias, antPerPat_esVenereas,
                                                antPerPat_esCirugias, antPerPat_esFracturas, antPerPat_ObservacionesPat, intApaSis_esRespiratorio, intApaSis_esDigestivo, intApaSis_esCardiovascular, intApaSis_esNervioso, intApaSis_esUrinario,
                                                intApaSis_esEndocrino, intApaSis_esPsiquiatrico, intApaSis_esEsqueletico, intApaSis_esAudicion, intApaSis_esVision, intApaSis_esOlfato, intApaSis_esTacto, intApaSis_ObservacionesPat,
                                                padAct_PadActuales, expFSV_TArterial, expFSV_Pulso, expFSV_Temp, expFSV_FCardiaca, expFSV_FResp, expFSV_Peso, expFSV_Talla, expFSV_IMC, expFC_Craneo, expFC_Parpados, expFC_Conjutiva, expFC_Reflejos,
                                                expFC_FosasNasales, expFC_Boca, expFC_Amigdalas, expFC_Dentadura, expFC_Encias, expFC_Cuello, expFC_Tiroides, expFC_Ganglios, expFC_Oidos, expFC_Otros, expFC_Observaciones, expFAV_VisCerAmbosOjos,
                                                expFAV_VisCerOjoIzq, expFAV_VisCerOjoDer, expFAV_VisLejAmbosOjos, expFAV_VisLejOjoIzq, expFAV_VisLejOjoDer, expFAV_CorregidaAmbosOjos, expFAV_CorregidaOjoIzq, expFAV_CorregidaOjoDer,
                                                expFAV_CampimetriaOI, expFAV_CampimetriaOD, expFAV_PterigionOI, expFAV_PterigionOD, expFAV_FondoOjo, expFAV_Daltonismo, expFAV_Observaciones, expFTATE_esCamposPulmonares, expFTATE_esPuntosDolorosos,
                                                expFTATE_esGenitales, expFTATE_esRuidosCardiacos, expFTATE_esHallusValgus, expFTATE_esHerniasUmbili, expFTATE_esAreaRenal, expFTATE_esVaricocele, expFTATE_esGrandulasMamarias,
                                                expFTATE_esColumnaVertebral, expFTATE_esPiePlano, expFTATE_esVarices, expFTATE_esMiembrosSup, expFTATE_esParedAbdominal, expFTATE_esAnillosInguinales, expFTATE_esMiembrosInf, expFTATE_esTatuajes,
                                                expFTATE_esVisceromegalias, expFTATE_esMarcha, expFTATE_esHerniasInguinales, expFTATE_esHombrosDolorosos, expFTATE_esQuistes, expFTATE_Observaciones, expFTATE_MS_HombroDer_esFlexion,
                                                expFTATE_MS_HombroIzq_esFlexion, expFTATE_MS_CodoDer_esFlexion, expFTATE_MS_CodoIzq_esFlexion, expFTATE_MS_MunecaDer_esFlexion, expFTATE_MS_MunecaIzq_esFlexion, expFTATE_MS_DedosDer_esFlexion,
                                                expFTATE_MS_DedosIzq_esFlexion, expFTATE_MS_HombroDer_esExtension, expFTATE_MS_HombroIzq_esExtension, expFTATE_MS_CodoDer_esExtension, expFTATE_MS_CodoIzq_esExtension,
                                                expFTATE_MS_MunecaDer_esExtension, expFTATE_MS_MunecaIzq_esExtension, expFTATE_MS_DedosDer_esExtension, expFTATE_MS_DedosIzq_esExtension, expFTATE_MS_HombroDer_esAbduccion, expFTATE_MS_HombroIzq_esAbduccion,
                                                expFTATE_MS_CodoDer_esAbduccion, expFTATE_MS_CodoIzq_esAbduccion, expFTATE_MS_MunecaDer_esAbduccion, expFTATE_MS_MunecaIzq_esAbduccion, expFTATE_MS_DedosDer_esAbduccion, expFTATE_MS_DedosIzq_esAbduccion,
                                                expFTATE_MS_HombroDer_esAduccion, expFTATE_MS_HombroIzq_esAduccion, expFTATE_MS_MunecaDer_esAduccion, expFTATE_MS_MunecaIzq_esAduccion, expFTATE_MS_DedosDer_esAduccion, expFTATE_MS_DedosIzq_esAduccion,
                                                expFTATE_MS_HombroDer_esRotInterna, expFTATE_MS_HombroIzq_esRotInterna, expFTATE_MS_MunecaDer_esRotInterna, expFTATE_MS_MunecaIzq_esRotInterna, expFTATE_MS_DedosDer_esRotInterna,
                                                expFTATE_MS_DedosIzq_esRotInterna, expFTATE_MS_HombroDer_esRotExterna, expFTATE_MS_HombroIzq_esRotExterna, expFTATE_MS_MunecaDer_esRotExterna, expFTATE_MS_MunecaIzq_esRotExterna,
                                                expFTATE_MS_DedosDer_esRotExterna, expFTATE_MS_DedosIzq_esRotExterna, expFTATE_MS_CodoDer_esPronacion, expFTATE_MS_CodoIzq_esPronacion, expFTATE_MS_MunecaDer_esPronacion, expFTATE_MS_MunecaIzq_esPronacion,
                                                expFTATE_MS_CodoDer_esSupinacion, expFTATE_MS_CodoIzq_esSupinacion, expFTATE_MS_MunecaDer_esSupinacion, expFTATE_MS_MunecaIzq_esSupinacion, expFTATE_MS_MunecaDer_esDesvUlnar, 
                                                expFTATE_MS_MunecaIzq_esDesvUlnar, expFTATE_MS_MunecaDer_esDesvRadial, expFTATE_MS_MunecaIzq_esDesvRadial, expFTATE_MS_MunecaDer_esOponencia, expFTATE_MS_MunecaIzq_esOponencia, 
                                                expFTATE_MS_DedosDer_esOponencia, expFTATE_MS_DedosIzq_esOponencia, expFTATE_MI_CaderaDer_esFlexion, expFTATE_MI_CaderaIzq_esFlexion, expFTATE_MI_RodillasDer_esFlexion, expFTATE_MI_RodillasIzq_esFlexion,
                                                expFTATE_MI_CllPieDer_esFlexion, expFTATE_MI_CllPieIzq_esFlexion, expFTATE_MI_DedosDer_esFlexion, expFTATE_MI_DedosIzq_esFlexion, expFTATE_MI_CaderaDer_esExtension, expFTATE_MI_CaderaIzq_esExtension,
                                                expFTATE_MI_RodillasDer_esExtension, expFTATE_MI_RodillasIzq_esExtension, expFTATE_MI_CllPieDer_esExtension, expFTATE_MI_CllPieIzq_esExtension, expFTATE_MI_DedosDer_esExtension,
                                                expFTATE_MI_DedosIzq_esExtension, expFTATE_MI_CaderaDer_esAbduccion, expFTATE_MI_CaderaIzq_esAbduccion, expFTATE_MI_CllPieDer_esAbduccion, expFTATE_MI_CllPieIzq_esAbduccion, expFTATE_MI_DedosDer_esAbduccion,
                                                expFTATE_MI_DedosIzq_esAbduccion, expFTATE_MI_CaderaDer_esAduccion, expFTATE_MI_CaderaIzq_esAduccion, expFTATE_MI_CllPieDer_esAduccion, expFTATE_MI_CllPieIzq_esAduccion, expFTATE_MI_DedosDer_esAduccion,
                                                expFTATE_MI_DedosIzq_esAduccion, expFTATE_MI_CaderaDer_esRotInterna, expFTATE_MI_CaderaIzq_esRotInterna, expFTATE_MI_RodillasDer_esRotInterna, expFTATE_MI_RodillasIzq_esRotInterna,
                                                expFTATE_MI_CllPieDer_esRotInterna, expFTATE_MI_CllPieIzq_esRotInterna, expFTATE_MI_CaderaDer_esRotExterna, expFTATE_MI_CaderaIzq_esRotExterna, expFTATE_MI_RodillasDer_esRotExterna,
                                                expFTATE_MI_RodillasIzq_esRotExterna, expFTATE_MI_CllPieDer_esRotExterna, expFTATE_MI_CllPieIzq_esRotExterna, expFTATE_MI_CllPieDer_esInversion, expFTATE_MI_CllPieIzq_esInversion, 
                                                expFTATE_MI_CllPieDer_esEversion, expFTATE_MI_CllPieIzq_esEversion, expFTATE_MS_MI_Observaciones, expFDT_PielMucosas, expFDT_EstadoPsiquiatrico, expFDT_ExamenNeurologico, expFDT_FobiasActuales,
                                                expFDT_Higiene, expFDT_ConstitucionFisica, expFDT_Otros, expFDT_Observaciones, estGab_TipoSanguineoID, estGab_Antidoping, estGab_Laboratorios, estGab_ObservacionesGrupoRH, estGab_ExamGenOrina,
                                                estGab_ExamGenOrinaObservaciones, estGab_Radiografias, estGab_RadiografiasObservaciones, estGab_Audiometria, estGab_AudiometriaObservaciones, estGab_HBC, estGab_Espirometria, estGab_EspirometriaObservaciones,
                                                estGab_Electrocardiograma, estGab_ElectrocardiogramaObservaciones, estGab_FechaPrimeraDosis, estGab_FechaSegundaDosis, estGab_MarcaDosisID, estGab_VacunacionObservaciones, estGab_LstProblemas,
                                                estGab_Recomendaciones, esp_Espirometria, esp_EspirometriaObservaciones, aud_HipoacusiaOD, aud_HipoacusiaOI, aud_HBC, aud_Diagnostico, aud_KH1, aud_KH1_OI, aud_KH1_OD, aud_KH2, aud_KH2_OI, aud_KH2_OD,
                                                aud_KH3, aud_KH3_OI, aud_KH3_OD, aud_KH4, aud_KH4_OI, aud_KH4_OD, aud_KH5, aud_KH5_OI, aud_KH5_OD, aud_KH6, aud_KH6_OI, aud_KH6_OD, aud_KH7, aud_KH7_OI, aud_KH7_OD, aud_NotasAudiometria,
                                                eleDer_Interpretacion, radTCLP_Conclusiones, certMed_CertificadoMedico, certMed_AptitudID, certMed_Fecha, certMed_NombrePaciente, recom_Recomendaciones, medicoCreacionID 
                                                    FROM tblS_SO_HistorialesClinicos WHERE id = @id";
                    #endregion

                    HistorialClinicoDTO objHC = _context.Select<HistorialClinicoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery,
                        parametros = new { id = idHistorialClinico }
                    }).FirstOrDefault();
                    resultado.Add("objHC", objHC);
                    resultado.Add(SUCCESS, true);
                    #endregion

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "GetDatosActualizarHistorialClinico", e, AccionEnum.CONSULTA, idHistorialClinico, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetHistorialesClinicos()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LA INFORMACIÓN DEL HISTORIAL CLINICO SELECCIONADO
                    List<HistorialClinicoDTO> lstHC = _context.Select<HistorialClinicoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id, dtsPer_Paciente, dtsPer_CURP, cp_esVoBo FROM tblS_SO_HistorialesClinicos WHERE registroActivo = @registroActivo",
                        parametros = new { registroActivo = true }
                    }).ToList();
                    foreach (var item in lstHC)
                    {
                        switch (item.cp_esVoBo)
                        {
                            case 0:
                                item.cp_StrEsVoBo = "PENDIENTE";
                                break;
                            case 1:
                                item.cp_StrEsVoBo = "COMPLETO";
                                break;
                            default:
                                item.cp_StrEsVoBo = "PENDIENTE";
                                break;
                        }
                    }
                    resultado.Add("lstHC", lstHC);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "GetHistorialesClinicos", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EliminarHistorialClinico(int idHistorialClinico)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE ELIMINA HISTORIAL CLINICO SELECCIONADO
                    if (idHistorialClinico <= 0)
                        throw new Exception("Ocurrió un error al eliminar el historial clinico seleccionado.");

                    tblS_SO_HistorialesClinicos objEliminar = _context.tblS_SO_HistorialesClinicos.Where(w => w.id == idHistorialClinico).FirstOrDefault();
                    if (objEliminar == null)
                        throw new Exception("Ocurrió un error al eliminar el historial clinico seleccionado.");

                    objEliminar.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                    objEliminar.fechaModificacion = DateTime.Now;
                    objEliminar.registroActivo = false;
                    _context.SaveChanges();

                    resultado.Add(MESSAGE, "Se ha eliminado con éxito el registro.");
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "EliminarHistorialClinico", e, AccionEnum.ELIMINAR, idHistorialClinico, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        #region OBSERVACIÓN MEDICO CP
        public Dictionary<string, object> GetObservacionMedicoInternoCP(int idHC)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    if (idHC > 0)
                    {
                        #region SE OBTIENE LA OSBERVACIÓN DEL MEDICO INTERNO, EN CASO QUE EXISTA
                        HistorialClinicoDTO objHC = _context.Select<HistorialClinicoDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT cp_observacionMedicoCP, cp_aptitudIDCP FROM tblS_SO_HistorialesClinicos WHERE id = @id",
                            parametros = new { id = idHC }
                        }).FirstOrDefault();
                        resultado.Add("objHC", objHC);
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "GetObservacionMedicoInternoCP", e, AccionEnum.CONSULTA, idHC, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> EditarObservacionMedicoInternoCP(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    if (objDTO.id > 0)
                    {
                        #region SE ACTUALIZA LA OBSERVACIÓN DEL MEDICO INTERNO DE CP
                        tblS_SO_HistorialesClinicos objE = _context.tblS_SO_HistorialesClinicos.Where(w => w.id == objDTO.id).FirstOrDefault();
                        if (objE == null)
                            throw new Exception("Ocurrió un error ingresar la observación.");

                        objE.cp_aptitudIDCP = objDTO.cp_aptitudIDCP;
                        objE.cp_observacionMedicoCP = objDTO.cp_observacionMedicoCP;
                        objE.cp_esVoBo = 1;
                        _context.SaveChanges();

                        #region SE OBTIENE CURP DEL PACIENTE
                        objDTO.dtsPer_CURP = _context.tblS_SO_HistorialesClinicos.Where(w => w.id == objDTO.id).Select(s => s.dtsPer_CURP).FirstOrDefault();
                        #endregion

                        #region SE REGISTRA LOS ANEXOS
                        if (lstArchivos != null)
                        {
                            for (int i = 0; i < lstArchivos.Count(); i++)
                            {
                                RegistrarArchivos(objDTO, TipoArchivoEnum.voboMedicoInterno, lstArchivos[i], "VoBoMedicoInterno-");
                            }
                        }
                        #endregion

                        resultado.Clear();
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "EditarObservacionMedicoInternoCP", e, AccionEnum.ACTUALIZAR, objDTO.id, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion 

        #region CARGAR DOCUMENTO FIRMADO MEDICO EXTERNO
        public Dictionary<string, object> CargarDocumentoFirmadoMedicoExterno(List<HttpPostedFileBase> lstArchivos, HistorialClinicoDTO objDTO)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    if (objDTO.id > 0)
                    {
                        #region SE OBTIENE CURP DEL PACIENTE
                        objDTO.dtsPer_CURP = _context.tblS_SO_HistorialesClinicos.Where(w => w.id == objDTO.id).Select(s => s.dtsPer_CURP).FirstOrDefault();
                        #endregion

                        #region SE REGISTRA LOS ANEXOS
                        if (lstArchivos != null)
                        {
                            for (int i = 0; i < lstArchivos.Count(); i++)
                            {
                                RegistrarArchivos(objDTO, TipoArchivoEnum.hcFirmadoMedicoExterno, lstArchivos[i], "HCFirmadoMedicoExterno-");
                            }
                        }
                        #endregion

                        resultado.Clear();
                        resultado.Add(MESSAGE, "Se ha registrado con éxito.");
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "EditarObservacionMedicoInternoCP", e, AccionEnum.ACTUALIZAR, objDTO.id, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetRutaArchivo(int idHC, int tipoArchivo)
        {
            try
            {
                resultado = new Dictionary<string, object>();
                string rutaArchivo = _context.tblS_SO_Archivos.Where(w => w.idHC == idHC && w.tipoArchivo == (int)tipoArchivo).Select(s => s.rutaArchivo).FirstOrDefault();
                resultado.Add("rutaArchivo", rutaArchivo);
                resultado.Add(SUCCESS, true);
                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "GetRutaArchivo", e, AccionEnum.CONSULTA, idHC, 0);
                return null;
            }
        }

        public Tuple<Stream, string> DescargarArchivo(int idHC, int tipoArchivo)
        {
            try
            {
                string rutaArchivo = _context.tblS_SO_Archivos.Where(w => w.idHC == idHC && w.tipoArchivo == (int)tipoArchivo).Select(s => s.rutaArchivo).FirstOrDefault();

                var fileStream = GlobalUtils.GetFileAsStream(rutaArchivo);
                string name = Path.GetFileName(rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarArchivo", e, AccionEnum.DESCARGAR, idHC, 0);
                return null;
            }
        }

        public Dictionary<string, object> VerificarExisteDocumento(int idHC, int tipoArchivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    if (idHC > 0)
                    {
                        #region SE VERIFICA SI CUENTA CON DOCUMENTO CARGADO
                        int existeDocumento = _context.tblS_SO_Archivos.Where(w => w.idHC == idHC && w.tipoArchivo == (int)tipoArchivo && w.registroActivo).Count();
                        resultado.Add("cantDocumentos", existeDocumento);
                        resultado.Add(SUCCESS, true);
                        dbContextTransaction.Commit();
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "GetObservacionMedicoInternoCP", e, AccionEnum.CONSULTA, idHC, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion
        #endregion

        #region FUNCIONES GENERALES
        public Dictionary<string, object> FillCboEscolaridades()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE ESCOLARIDADES
                    List<ComboDTO> lstEscolaridades = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id AS Value, escolaridad AS Text FROM tblP_CatEscolaridades WHERE registroActivo = @registroActivo ORDER BY orden",
                        parametros = new { registroActivo = true }
                    });
                    resultado.Add(ITEMS, lstEscolaridades);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboEscolaridades", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEmpresas()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE EMPRESAS
                    if(vSesiones.sesionEmpresaActual==1 || vSesiones.sesionEmpresaActual==2)
                    {
                        List<ComboDTO> lstEmpresas = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Construplan,
                            consulta = @"SELECT id AS Value, nombre AS Text FROM tblP_Empresas WHERE estatus = @estatus AND id IN (@idEmpresaCP, @idEmpresaARR)",
                            parametros = new { estatus = true, idEmpresaCP = (int)EmpresaEnum.Construplan, idEmpresaARR = (int)EmpresaEnum.Arrendadora }
                        });
                        resultado.Add(ITEMS, lstEmpresas);
                    }
                    else if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
                    {
                        var empresa = (MainContextEnum)vSesiones.sesionEmpresaActual;
                        List<ComboDTO> lstEmpresas = _context.Select<ComboDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT id AS Value, nombre AS Text FROM tblP_Empresas WHERE estatus = @estatus AND id = @empresa",
                            parametros = new { estatus = true, empresa = empresa }
                        });
                        resultado.Add(ITEMS, lstEmpresas);
                    }


                   
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboEmpresas", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboCC()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE CC
                    //string strQuery = string.Empty;

                    //if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    //    strQuery = @"SELECT id AS Value, CONVERT(VARCHAR(20), cc) + ' - ' + CONVERT(VARCHAR(250), descripcion) AS Text FROM tblP_CC WHERE estatus = @estatus ORDER BY cc";
                    //else
                    //    strQuery = @"SELECT id AS Value, CONVERT(VARCHAR(20), areaCuenta) + ' ' + CONVERT(VARCHAR(250), descripcion) AS Text FROM tblP_CC WHERE estatus = @estatus ORDER BY cc";

                    List<ComboDTO> lstCC = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT id AS Value, CONVERT(VARCHAR(20), cc) + ' - ' + CONVERT(VARCHAR(250), descripcion) AS Text FROM tblP_CC WHERE estatus = @estatus ORDER BY cc",
                        parametros = new { estatus = true }
                    });
                    resultado.Add(ITEMS, lstCC);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboCC", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboEstadoCivil()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE ESTADO CIVIL
                    List<ComboDTO> lstEstadoCivil = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT id AS Value, estadoCivil AS Text FROM tblP_EstadoCivil WHERE registroActivo = @registroActivo ORDER BY estadoCivil",
                        parametros = new { registroActivo = true }
                    });
                    resultado.Add(ITEMS, lstEstadoCivil);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboEstadoCivil", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboTipoSanguineo()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE TIPO SANGRE
                    List<ComboDTO> lstTipoSangre = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT id AS Value, tipoSangre AS Text FROM tblP_CatTipoSangre WHERE esActivo = 1 ORDER BY tipoSangre",
                        parametros = new { esActivo = true }
                    });
                    resultado.Add(ITEMS, lstTipoSangre);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboTipoSanguineo", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> GetEdadPaciente(DateTime fechaNac)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE EDAD DEL PACIENTE EN BASE A SU FECHA NACIMIENTO
                    int edadPaciente = calcularEdad(fechaNac);
                    resultado.Add("edadPaciente", edadPaciente);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "GetEdadPaciente", e, AccionEnum.CONSULTA, 0, new { fechaNac });
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboMarcasCovid19()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE MARCAS DE VACUNAS COVID 19
                    List<ComboDTO> lstMarcasCovid = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id AS Value, marca AS Text FROM tblS_SO_MarcasVacunasCovid19 WHERE registroActivo = @registroActivo ORDER BY marca",
                        parametros = new { registroActivo = true }
                    });
                    resultado.Add(ITEMS, lstMarcasCovid);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboMarcasCovid19", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> FillCboUsuariosMedicos()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LISTADO DE USUARIOS TIPO MEDICO
                    List<ComboDTO> lstMedicos = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT id AS Value, CONVERT(VARCHAR(250), nombre) + ' ' + CONVERT(VARCHAR(250), apellidoPaterno) +  ' ' + CONVERT(VARCHAR(250), apellidoMaterno) as Text FROM tblP_Usuario WHERE estatus = 1 AND perfilID = 5",
                        parametros = new { estatus = 1, perfilID = 5 }
                    });
                    resultado.Add(ITEMS, lstMedicos);
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(16, 16, NombreControlador, "FillCboUsuariosMedicos", e, AccionEnum.CONSULTA, 0, 0);
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }
        #endregion

        #region CrystalReports

        public Dictionary<string, object> GetReportDataHistorialClinico(int idHistorialClinico)
        {

                resultado = new Dictionary<string, object>();
                try
                {
                    #region SE OBTIENE LA INFORMACIÓN DEL HISTORIAL CLINICO SELECCIONADO

                    #region SE CONSTRUYE QUERY SELECT HISTORIAL CLINICO
                    string strQuery = @"SELECT id, folio, 
	                                        --dtsPer_EmpresaID,
	                                        (
	                                        SELECT 
		                                        nombre
	                                        FROM 
		                                        tblP_Empresas
	                                        WHERE
		                                        id = dtsPer_EmpresaID
	                                        )dtsPer_EmpresaID,
	                                        --dtsPer_CCID,
	                                        (
	                                        SELECT 
		                                        descripcion
	                                        FROM
		                                        tblP_CC
	                                        WHERE 
		                                        id = dtsPer_CCID
	                                        )dtsPer_CCID,
	                                        dtsPer_Paciente, dtsPer_FechaHora, dtsPer_FechaNac, dtsPer_Sexo, 
	                                        --dtsPer_EstadoCivilID, 
	                                        (
	                                        SELECT 
		                                        estadoCivil
	                                        FROM
		                                        tblP_EstadoCivil
	                                        WHERE
		                                        id = dtsPer_EstadoCivilID
	                                        )dtsPer_EstadoCivilID,
	                                        --dtsPer_TipoSangreID,
	                                        (
	                                        SELECT
		                                        tipoSangre
	                                        FROM
		                                        tblP_CatTipoSangre
	                                        WHERE
		                                        id = dtsPer_TipoSangreID
	                                        )dtsPer_TipoSangreID,
	                                        dtsPer_CURP, dtsPer_Domicilio, dtsPer_Ciudad,
	                                        --dtsPer_EscolaridadID,
	                                        (
	                                        SELECT
		                                        escolaridad
	                                        FROM
		                                        tblP_CatEscolaridades
	                                        WHERE
		                                        id = dtsPer_EscolaridadID
	                                        )dtsPer_EscolaridadID,
	                                        dtsPer_LugarNac, dtsPer_Telefono, motEva_esIngreso, motEva_esRetiro, motEva_esEvaOpcional, motEva_esPostIncapacidad, motEva_esReubicacion, antLab_Puesto, antLab_Empresa,
	                                        antLab_Desde, antLab_Hasta, antLab_Turno, antLab_esDePie, antLab_esInclinado, antLab_esSentado, antLab_esArrodillado, antLab_esCaminando, antLab_esOtra, accET_Empresa,
	                                        accET_Anio, accET_LesionAreaAnatomica, accET_Secuelas, accET_Cuales, accET_ExamNoAceptables, accET_Causas, accET_AbandonoTrabajo, accET_IncapacidadFrecuente, accET_Prolongadas, usoElePP_esActual,
	                                        usoElePP_esCasco, usoElePP_esTapaboca, usoElePP_esGafas, usoElePP_esRespirador, usoElePP_esBotas, usoElePP_esAuditivos, usoElePP_esOverol, usoElePP_esGuantes, usoElePP_OtroCual, usoElePP_DeberiaRecibir,
	                                        usoElePP_ConsideraAdecuado, antFam_esTuberculosis, antFam_TuberculosisParentesco, antFam_esHTA, antFam_HTAParentesco, antFam_esDiabetes, antFam_DiabetesParentesco, antFam_esACV, antFam_ACVParentesco,
	                                        antFam_esInfarto, antFam_InfartoParentesco, antFam_esAsma, antFam_AsmaParentesco, antFam_esAlergias, antFam_AlergiasParentesco, antFam_esMental, antFam_MentalParentesco, antFam_esCancer,
	                                        antFam_CancerParentesco, antFam_Observaciones, antPerNoPat_Tabaquismo, antPerNoPat_CigarroDia, antPerNoPat_CigarroAnios, antPerNoPat_Alcoholismo, antPerNoPat_AlcoholismoAnios,
	                                        antPerNoPat_esDrogadiccion, antPerNoPat_esMarihuana, antPerNoPat_esCocaina, antPerNoPat_esAnfetaminas, antPerNoPat_Otros, antPerNoPat_Inmunizaciones, antPerNoPat_Tetanicos, antPerNoPat_FechaAntitetanica,
	                                        antPerNoPat_Hepatitis, antPerNoPat_Influenza, antPerNoPat_FechaInfluenza, antPerNoPat_Infancia, antPerNoPat_DescInfancia, antPerNoPat_Alimentacion, antPerNoPat_Higiene, antPerNoPat_MedicacionActual,
	                                        antPerPat_esNeoplasicos, antPerPat_esNeumopatias, antPerPat_esAsma, antPerPat_esFimico, antPerPat_esNeumoconiosis, antPerPat_esCardiopatias, antPerPat_esReumaticos, antPerPat_esAlergias,
	                                        antPerPat_esHipertension, antPerPat_esHepatitis, antPerPat_esTifoidea, antPerPat_esHernias, antPerPat_esLumbalgias, antPerPat_esDiabetes, antPerPat_esEpilepsias, antPerPat_esVenereas,
	                                        antPerPat_esCirugias, antPerPat_esFracturas, antPerPat_ObservacionesPat, intApaSis_esRespiratorio, intApaSis_esDigestivo, intApaSis_esCardiovascular, intApaSis_esNervioso, intApaSis_esUrinario,
	                                        intApaSis_esEndocrino, intApaSis_esPsiquiatrico, intApaSis_esEsqueletico, intApaSis_esAudicion, intApaSis_esVision, intApaSis_esOlfato, intApaSis_esTacto, intApaSis_ObservacionesPat,
	                                        padAct_PadActuales, expFSV_TArterial, expFSV_Pulso, expFSV_Temp, expFSV_FCardiaca, expFSV_FResp, expFSV_Peso, expFSV_Talla, expFSV_IMC, expFC_Craneo, expFC_Parpados, expFC_Conjutiva, expFC_Reflejos,
	                                        expFC_FosasNasales, expFC_Boca, expFC_Amigdalas, expFC_Dentadura, expFC_Encias, expFC_Cuello, expFC_Tiroides, expFC_Ganglios, expFC_Oidos, expFC_Otros, expFC_Observaciones, expFAV_VisCerAmbosOjos,
	                                        expFAV_VisCerOjoIzq, expFAV_VisCerOjoDer, expFAV_VisLejAmbosOjos, expFAV_VisLejOjoIzq, expFAV_VisLejOjoDer, expFAV_CorregidaAmbosOjos, expFAV_CorregidaOjoIzq, expFAV_CorregidaOjoDer,
	                                        expFAV_CampimetriaOI, expFAV_CampimetriaOD, expFAV_PterigionOI, expFAV_PterigionOD, expFAV_FondoOjo, expFAV_Daltonismo, expFAV_Observaciones, expFTATE_esCamposPulmonares, expFTATE_esPuntosDolorosos,
	                                        expFTATE_esGenitales, expFTATE_esRuidosCardiacos, expFTATE_esHallusValgus, expFTATE_esHerniasUmbili, expFTATE_esAreaRenal, expFTATE_esVaricocele, expFTATE_esGrandulasMamarias,
	                                        expFTATE_esColumnaVertebral, expFTATE_esPiePlano, expFTATE_esVarices, expFTATE_esMiembrosSup, expFTATE_esParedAbdominal, expFTATE_esAnillosInguinales, expFTATE_esMiembrosInf, expFTATE_esTatuajes,
	                                        expFTATE_esVisceromegalias, expFTATE_esMarcha, expFTATE_esHerniasInguinales, expFTATE_esHombrosDolorosos, expFTATE_esQuistes, expFTATE_Observaciones, expFTATE_MS_HombroDer_esFlexion,
	                                        expFTATE_MS_HombroIzq_esFlexion, expFTATE_MS_CodoDer_esFlexion, expFTATE_MS_CodoIzq_esFlexion, expFTATE_MS_MunecaDer_esFlexion, expFTATE_MS_MunecaIzq_esFlexion, expFTATE_MS_DedosDer_esFlexion,
	                                        expFTATE_MS_DedosIzq_esFlexion, expFTATE_MS_HombroDer_esExtension, expFTATE_MS_HombroIzq_esExtension, expFTATE_MS_CodoDer_esExtension, expFTATE_MS_CodoIzq_esExtension,
	                                        expFTATE_MS_MunecaDer_esExtension, expFTATE_MS_MunecaIzq_esExtension, expFTATE_MS_DedosDer_esExtension, expFTATE_MS_DedosIzq_esExtension, expFTATE_MS_HombroDer_esAbduccion, expFTATE_MS_HombroIzq_esAbduccion,
	                                        expFTATE_MS_CodoDer_esAbduccion, expFTATE_MS_CodoIzq_esAbduccion, expFTATE_MS_MunecaDer_esAbduccion, expFTATE_MS_MunecaIzq_esAbduccion, expFTATE_MS_DedosDer_esAbduccion, expFTATE_MS_DedosIzq_esAbduccion,
	                                        expFTATE_MS_HombroDer_esAduccion, expFTATE_MS_HombroIzq_esAduccion, expFTATE_MS_MunecaDer_esAduccion, expFTATE_MS_MunecaIzq_esAduccion, expFTATE_MS_DedosDer_esAduccion, expFTATE_MS_DedosIzq_esAduccion,
	                                        expFTATE_MS_HombroDer_esRotInterna, expFTATE_MS_HombroIzq_esRotInterna, expFTATE_MS_MunecaDer_esRotInterna, expFTATE_MS_MunecaIzq_esRotInterna, expFTATE_MS_DedosDer_esRotInterna,
	                                        expFTATE_MS_DedosIzq_esRotInterna, expFTATE_MS_HombroDer_esRotExterna, expFTATE_MS_HombroIzq_esRotExterna, expFTATE_MS_MunecaDer_esRotExterna, expFTATE_MS_MunecaIzq_esRotExterna,
	                                        expFTATE_MS_DedosDer_esRotExterna, expFTATE_MS_DedosIzq_esRotExterna, expFTATE_MS_CodoDer_esPronacion, expFTATE_MS_CodoIzq_esPronacion, expFTATE_MS_MunecaDer_esPronacion, expFTATE_MS_MunecaIzq_esPronacion,
	                                        expFTATE_MS_CodoDer_esSupinacion, expFTATE_MS_CodoIzq_esSupinacion, expFTATE_MS_MunecaDer_esSupinacion, expFTATE_MS_MunecaIzq_esSupinacion, expFTATE_MS_MunecaDer_esDesvUlnar, 
	                                        expFTATE_MS_MunecaIzq_esDesvUlnar, expFTATE_MS_MunecaDer_esDesvRadial, expFTATE_MS_MunecaIzq_esDesvRadial, expFTATE_MS_MunecaDer_esOponencia, expFTATE_MS_MunecaIzq_esOponencia, 
	                                        expFTATE_MS_DedosDer_esOponencia, expFTATE_MS_DedosIzq_esOponencia, expFTATE_MI_CaderaDer_esFlexion, expFTATE_MI_CaderaIzq_esFlexion, expFTATE_MI_RodillasDer_esFlexion, expFTATE_MI_RodillasIzq_esFlexion,
	                                        expFTATE_MI_CllPieDer_esFlexion, expFTATE_MI_CllPieIzq_esFlexion, expFTATE_MI_DedosDer_esFlexion, expFTATE_MI_DedosIzq_esFlexion, expFTATE_MI_CaderaDer_esExtension, expFTATE_MI_CaderaIzq_esExtension,
	                                        expFTATE_MI_RodillasDer_esExtension, expFTATE_MI_RodillasIzq_esExtension, expFTATE_MI_CllPieDer_esExtension, expFTATE_MI_CllPieIzq_esExtension, expFTATE_MI_DedosDer_esExtension,
	                                        expFTATE_MI_DedosIzq_esExtension, expFTATE_MI_CaderaDer_esAbduccion, expFTATE_MI_CaderaIzq_esAbduccion, expFTATE_MI_CllPieDer_esAbduccion, expFTATE_MI_CllPieIzq_esAbduccion, expFTATE_MI_DedosDer_esAbduccion,
	                                        expFTATE_MI_DedosIzq_esAbduccion, expFTATE_MI_CaderaDer_esAduccion, expFTATE_MI_CaderaIzq_esAduccion, expFTATE_MI_CllPieDer_esAduccion, expFTATE_MI_CllPieIzq_esAduccion, expFTATE_MI_DedosDer_esAduccion,
	                                        expFTATE_MI_DedosIzq_esAduccion, expFTATE_MI_CaderaDer_esRotInterna, expFTATE_MI_CaderaIzq_esRotInterna, expFTATE_MI_RodillasDer_esRotInterna, expFTATE_MI_RodillasIzq_esRotInterna,
	                                        expFTATE_MI_CllPieDer_esRotInterna, expFTATE_MI_CllPieIzq_esRotInterna, expFTATE_MI_CaderaDer_esRotExterna, expFTATE_MI_CaderaIzq_esRotExterna, expFTATE_MI_RodillasDer_esRotExterna,
	                                        expFTATE_MI_RodillasIzq_esRotExterna, expFTATE_MI_CllPieDer_esRotExterna, expFTATE_MI_CllPieIzq_esRotExterna, expFTATE_MI_CllPieDer_esInversion, expFTATE_MI_CllPieIzq_esInversion, 
	                                        expFTATE_MI_CllPieDer_esEversion, expFTATE_MI_CllPieIzq_esEversion, expFTATE_MS_MI_Observaciones, expFDT_PielMucosas, expFDT_EstadoPsiquiatrico, expFDT_ExamenNeurologico, expFDT_FobiasActuales,
	                                        expFDT_Higiene, expFDT_ConstitucionFisica, expFDT_Otros, expFDT_Observaciones, estGab_TipoSanguineoID, estGab_Antidoping, estGab_Laboratorios, estGab_ObservacionesGrupoRH, estGab_ExamGenOrina,
	                                        estGab_ExamGenOrinaObservaciones, estGab_Radiografias, estGab_RadiografiasObservaciones, estGab_Audiometria, estGab_AudiometriaObservaciones, estGab_HBC, estGab_Espirometria, estGab_EspirometriaObservaciones,
	                                        estGab_Electrocardiograma, estGab_ElectrocardiogramaObservaciones, estGab_FechaPrimeraDosis, estGab_FechaSegundaDosis, 
	                                        --estGab_MarcaDosisID,
	                                        (
	                                        SELECT 
		                                        marca
	                                        FROM
		                                        tblS_SO_MarcasVacunasCovid19
	                                        WHERE
		                                        id = 1 
	                                        )estGab_MarcaDosisID,
	                                        estGab_VacunacionObservaciones, estGab_LstProblemas,
	                                        estGab_Recomendaciones, esp_Espirometria, esp_EspirometriaObservaciones, aud_HipoacusiaOD, aud_HipoacusiaOI, aud_HBC, aud_Diagnostico, aud_KH1, aud_KH1_OI, aud_KH1_OD, aud_KH2, aud_KH2_OI, aud_KH2_OD,
	                                        aud_KH3, aud_KH3_OI, aud_KH3_OD, aud_KH4, aud_KH4_OI, aud_KH4_OD, aud_KH5, aud_KH5_OI, aud_KH5_OD, aud_KH6, aud_KH6_OI, aud_KH6_OD, aud_KH7, aud_KH7_OI, aud_KH7_OD, aud_NotasAudiometria,
	                                        eleDer_Interpretacion, radTCLP_Conclusiones, certMed_CertificadoMedico, certMed_AptitudID, certMed_Fecha, certMed_NombrePaciente, recom_Recomendaciones, 
	                                        --medicoCreacionID
	                                        (
	                                        SELECT
		                                        nombre
	                                        FROM
		                                        tblS_SO_Medicos
	                                        WHERE
		                                        idUsuarioSIGOPLAN = medicoCreacionID AND registroActivo = 1
	                                        ) medicoCreacionID,
	                                        (
	                                        SELECT
		                                        puesto
	                                        FROM
		                                        tblS_SO_Medicos
	                                        WHERE
		                                        idUsuarioSIGOPLAN = medicoCreacionID AND registroActivo = 1
	                                        ) medicoCreacionPuesto,
	                                        (
	                                        SELECT
		                                        cedulaProfesional
	                                        FROM
		                                        tblS_SO_Medicos
	                                        WHERE
		                                        idUsuarioSIGOPLAN = medicoCreacionID AND registroActivo = 1
	                                        ) medicoCreacionCedula
	
                                        FROM tblS_SO_HistorialesClinicos 

                                        WHERE id = @id";
                    #endregion

                    HistorialClinicoStrDTO objHC = _context.Select<HistorialClinicoStrDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = strQuery,
                        parametros = new { id = idHistorialClinico }
                    }).FirstOrDefault();

                    #region HCobj

                    HistorialClinicoStrDTO objStrHC = new HistorialClinicoStrDTO();
                    objStrHC.id = objHC.id != null && objHC.id != string.Empty ? objHC.id.ToString() : "";
                    objStrHC.folio = objHC.folio != null && objHC.folio != string.Empty ? objHC.folio.ToString() : "";
                    objStrHC.dtsPer_EmpresaID = objHC.dtsPer_EmpresaID != null && objHC.dtsPer_EmpresaID != string.Empty ? objHC.dtsPer_EmpresaID.ToString() : "";
                    objStrHC.dtsPer_CCID = objHC.dtsPer_CCID != null && objHC.dtsPer_CCID != string.Empty ? objHC.dtsPer_CCID.ToString() : "";
                    objStrHC.dtsPer_Paciente = objHC.dtsPer_Paciente != null && objHC.dtsPer_Paciente != string.Empty ? objHC.dtsPer_Paciente : "";
                    objStrHC.dtsPer_FechaHora = objHC.dtsPer_FechaHora != null && objHC.dtsPer_FechaHora != string.Empty ? Convert.ToDateTime(objHC.dtsPer_FechaHora).ToString("dd/MM/yyyy") : "";
                    objStrHC.dtsPer_FechaNac = objHC.dtsPer_FechaNac != null && objHC.dtsPer_FechaNac != string.Empty ? Convert.ToDateTime(objHC.dtsPer_FechaNac).ToString("dd/MM/yyyy") : "";
                    objStrHC.dtsPer_Edad = objHC.dtsPer_FechaNac != null && objHC.dtsPer_FechaNac != string.Empty ? GetEdadPaciente(Convert.ToDateTime(objHC.dtsPer_FechaNac))["edadPaciente"].ToString() : "";
                    objStrHC.dtsPer_Sexo = objHC.dtsPer_Sexo != null && objHC.dtsPer_Sexo != string.Empty ? objHC.dtsPer_Sexo.ToString() : "";
                    objStrHC.dtsPer_EstadoCivilID = objHC.dtsPer_EstadoCivilID != null && objHC.dtsPer_EstadoCivilID != string.Empty ? objHC.dtsPer_EstadoCivilID.ToString() : "";
                    objStrHC.dtsPer_TipoSangreID = objHC.dtsPer_TipoSangreID != null && objHC.dtsPer_TipoSangreID != string.Empty ? objHC.dtsPer_TipoSangreID.ToString() : "";
                    objStrHC.dtsPer_CURP = objHC.dtsPer_CURP != null && objHC.dtsPer_CURP != string.Empty ? objHC.dtsPer_CURP.ToString() : "";
                    objStrHC.dtsPer_Domicilio = objHC.dtsPer_Domicilio != null && objHC.dtsPer_Domicilio != string.Empty ? objHC.dtsPer_Domicilio.ToString() : "";
                    objStrHC.dtsPer_Ciudad = objHC.dtsPer_Ciudad != null && objHC.dtsPer_Ciudad != string.Empty ? objHC.dtsPer_Ciudad.ToString() : "";
                    objStrHC.dtsPer_EscolaridadID = objHC.dtsPer_EscolaridadID != null && objHC.dtsPer_EscolaridadID != string.Empty ? objHC.dtsPer_EscolaridadID.ToString() : "";
                    objStrHC.dtsPer_LugarNac = objHC.dtsPer_LugarNac != null && objHC.dtsPer_LugarNac != string.Empty ? objHC.dtsPer_LugarNac.ToString() : "";
                    objStrHC.dtsPer_Telefono = objHC.dtsPer_Telefono != null && objHC.dtsPer_Telefono != string.Empty ? objHC.dtsPer_Telefono.ToString() : "";
                    objStrHC.motEva_esIngreso = Convert.ToBoolean(objHC.motEva_esIngreso) ? "X" : "";
                    objStrHC.motEva_esRetiro = Convert.ToBoolean(objHC.motEva_esRetiro) ? "X" : "";
                    objStrHC.motEva_esEvaOpcional = Convert.ToBoolean(objHC.motEva_esEvaOpcional) ? "X" : "";
                    objStrHC.motEva_esPostIncapacidad = Convert.ToBoolean(objHC.motEva_esPostIncapacidad) ? "X" : "";
                    objStrHC.motEva_esReubicacion = Convert.ToBoolean(objHC.motEva_esReubicacion) ? "X" : "";
                    objStrHC.antLab_Puesto = objHC.antLab_Puesto != null && objHC.antLab_Puesto != string.Empty ? objHC.antLab_Puesto.ToString() : "";
                    objStrHC.antLab_Empresa = objHC.antLab_Empresa != null && objHC.antLab_Empresa != string.Empty ? objHC.antLab_Empresa.ToString() : "";
                    objStrHC.antLab_Desde = objHC.antLab_Desde != null && objHC.antLab_Desde != string.Empty ? objHC.antLab_Desde : "";
                    objStrHC.antLab_Hasta = objHC.antLab_Hasta != null && objHC.antLab_Hasta != string.Empty ? objHC.antLab_Hasta : "";
                    objStrHC.antLab_Turno = objHC.antLab_Turno != null && objHC.antLab_Turno != string.Empty ? objHC.antLab_Turno.ToString() : "";
                    objStrHC.antLab_esDePie = Convert.ToBoolean(objHC.antLab_esDePie) ? "X" : "";
                    objStrHC.antLab_esInclinado = Convert.ToBoolean(objHC.antLab_esInclinado) ? "X" : "";
                    objStrHC.antLab_esSentado = Convert.ToBoolean(objHC.antLab_esSentado) ? "X" : "";
                    objStrHC.antLab_esArrodillado = Convert.ToBoolean(objHC.antLab_esArrodillado) ? "X" : "";
                    objStrHC.antLab_esCaminando = Convert.ToBoolean(objHC.antLab_esCaminando) ? "X" : "";
                    objStrHC.antLab_esOtra = Convert.ToBoolean(objHC.antLab_esOtra) ? "X" : "";
                    objStrHC.antLab_esCual = objHC.antLab_esCual != null && objHC.antLab_esCual != string.Empty ? objHC.antLab_esCual.ToString() : "";
                    objStrHC.accET_Empresa = objHC.accET_Empresa != null && objHC.accET_Empresa != string.Empty ? objHC.accET_Empresa.ToString() : "";
                    objStrHC.accET_Anio = objHC.accET_Anio != null && objHC.accET_Anio != string.Empty ? objHC.accET_Anio.ToString() : "";
                    objStrHC.accET_LesionAreaAnatomica = objHC.accET_LesionAreaAnatomica != null && objHC.accET_LesionAreaAnatomica != string.Empty ? objHC.accET_LesionAreaAnatomica.ToString() : "";
                    objStrHC.accET_Secuelas = objHC.accET_Secuelas != null && objHC.accET_Secuelas != string.Empty ? objHC.accET_Secuelas.ToString() : "";
                    objStrHC.accET_Cuales = objHC.accET_Cuales != null && objHC.accET_Cuales != string.Empty ? objHC.accET_Cuales.ToString() : "";
                    objStrHC.accET_ExamNoAceptables = objHC.accET_ExamNoAceptables != null && objHC.accET_ExamNoAceptables != string.Empty ? objHC.accET_ExamNoAceptables.ToString() : "";
                    objStrHC.accET_Causas = objHC.accET_Causas != null && objHC.accET_Causas != string.Empty ? objHC.accET_Causas.ToString() : "";
                    objStrHC.accET_AbandonoTrabajo = objHC.accET_AbandonoTrabajo != null && objHC.accET_AbandonoTrabajo != string.Empty ? objHC.accET_AbandonoTrabajo.ToString() : "";
                    objStrHC.accET_IncapacidadFrecuente = objHC.accET_IncapacidadFrecuente != null && objHC.accET_IncapacidadFrecuente != string.Empty ? objHC.accET_IncapacidadFrecuente.ToString() : "";
                    objStrHC.accET_Prolongadas = objHC.accET_Prolongadas != null && objHC.accET_Prolongadas != string.Empty ? objHC.accET_Prolongadas.ToString() : "";
                    objStrHC.usoElePP_esActual = Convert.ToBoolean(objHC.usoElePP_esActual) ? "X" : "";
                    objStrHC.usoElePP_esCasco = Convert.ToBoolean(objHC.usoElePP_esCasco) ? "X" : "";
                    objStrHC.usoElePP_esTapaboca = Convert.ToBoolean(objHC.usoElePP_esTapaboca) ? "X" : "";
                    objStrHC.usoElePP_esGafas = Convert.ToBoolean(objHC.usoElePP_esGafas) ? "X" : "";
                    objStrHC.usoElePP_esRespirador = Convert.ToBoolean(objHC.usoElePP_esRespirador) ? "X" : "";
                    objStrHC.usoElePP_esBotas = Convert.ToBoolean(objHC.usoElePP_esBotas) ? "X" : "";
                    objStrHC.usoElePP_esAuditivos = Convert.ToBoolean(objHC.usoElePP_esAuditivos) ? "X" : "";
                    objStrHC.usoElePP_esOverol = Convert.ToBoolean(objHC.usoElePP_esOverol) ? "X" : "";
                    objStrHC.usoElePP_esGuantes = Convert.ToBoolean(objHC.usoElePP_esGuantes) ? "X" : "";
                    objStrHC.usoElePP_OtroCual = objHC.usoElePP_OtroCual != null && objHC.usoElePP_OtroCual != string.Empty ? objHC.usoElePP_OtroCual.ToString() : "";
                    objStrHC.usoElePP_DeberiaRecibir = objHC.usoElePP_DeberiaRecibir != null && objHC.usoElePP_DeberiaRecibir != string.Empty ? objHC.usoElePP_DeberiaRecibir.ToString() : "";
                    objStrHC.usoElePP_ConsideraAdecuado = objHC.usoElePP_ConsideraAdecuado != null && objHC.usoElePP_ConsideraAdecuado != string.Empty ? objHC.usoElePP_ConsideraAdecuado.ToString() : "";
                    objStrHC.antFam_esTuberculosis = !!string.IsNullOrEmpty(objHC.antFam_esTuberculosis) ? objHC.antFam_esTuberculosis.Trim().ToUpper() == "SI" ? objHC.antFam_esTuberculosis.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO" : "NO";
                    objStrHC.antFam_TuberculosisParentesco = objHC.antFam_TuberculosisParentesco != null && objHC.antFam_TuberculosisParentesco != string.Empty ? objHC.antFam_TuberculosisParentesco.ToString() : "";
                    objStrHC.antFam_esHTA = !string.IsNullOrEmpty(objHC.antFam_esHTA) ? objHC.antFam_esHTA.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_HTAParentesco = objHC.antFam_HTAParentesco != null && objHC.antFam_HTAParentesco != string.Empty ? objHC.antFam_HTAParentesco.ToString() : "";
                    objStrHC.antFam_esDiabetes = !string.IsNullOrEmpty(objHC.antFam_esDiabetes) ? objHC.antFam_esDiabetes.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_DiabetesParentesco = objHC.antFam_DiabetesParentesco != null && objHC.antFam_DiabetesParentesco != string.Empty ? objHC.antFam_DiabetesParentesco.ToString() : "";
                    objStrHC.antFam_esACV = !string.IsNullOrEmpty(objHC.antFam_esACV) ? objHC.antFam_esACV.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_ACVParentesco = objHC.antFam_ACVParentesco != null && objHC.antFam_ACVParentesco != string.Empty ? objHC.antFam_ACVParentesco.ToString() : "";
                    objStrHC.antFam_esInfarto = !string.IsNullOrEmpty(objHC.antFam_esInfarto) ? objHC.antFam_esInfarto.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_InfartoParentesco = objHC.antFam_InfartoParentesco != null && objHC.antFam_InfartoParentesco != string.Empty ? objHC.antFam_InfartoParentesco.ToString() : "";
                    objStrHC.antFam_esAsma = !string.IsNullOrEmpty(objHC.antFam_esAsma) ? objHC.antFam_esAsma.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_AsmaParentesco = objHC.antFam_AsmaParentesco != null && objHC.antFam_AsmaParentesco != string.Empty ? objHC.antFam_AsmaParentesco.ToString() : "";
                    objStrHC.antFam_esAlergias = !string.IsNullOrEmpty(objHC.antFam_esAlergias) ? objHC.antFam_esAlergias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_AlergiasParentesco = objHC.antFam_AlergiasParentesco != null && objHC.antFam_AlergiasParentesco != string.Empty ? objHC.antFam_AlergiasParentesco.ToString() : "";
                    objStrHC.antFam_esMental = !string.IsNullOrEmpty(objHC.antFam_esMental) ? objHC.antFam_esMental.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_MentalParentesco = objHC.antFam_MentalParentesco != null && objHC.antFam_MentalParentesco != string.Empty ? objHC.antFam_MentalParentesco.ToString() : "";
                    objStrHC.antFam_esCancer = !string.IsNullOrEmpty(objHC.antFam_esCancer) ? objHC.antFam_esCancer.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antFam_CancerParentesco = objHC.antFam_CancerParentesco != null && objHC.antFam_CancerParentesco != string.Empty ? objHC.antFam_CancerParentesco.ToString() : "";
                    objStrHC.antFam_Observaciones = objHC.antFam_Observaciones != null && objHC.antFam_Observaciones != string.Empty ? objHC.antFam_Observaciones.ToString() : "";
                    objStrHC.antPerNoPat_Tabaquismo = objHC.antPerNoPat_Tabaquismo != null && objHC.antPerNoPat_Tabaquismo != string.Empty ? objHC.antPerNoPat_Tabaquismo.ToString() : "";
                    objStrHC.antPerNoPat_CigarroDia = objHC.antPerNoPat_CigarroDia != null && objHC.antPerNoPat_CigarroDia != string.Empty ? objHC.antPerNoPat_CigarroDia.ToString() : "";
                    objStrHC.antPerNoPat_CigarroAnios = objHC.antPerNoPat_CigarroAnios != null && objHC.antPerNoPat_CigarroAnios != string.Empty ? objHC.antPerNoPat_CigarroAnios.ToString() : "";
                    objStrHC.antPerNoPat_Alcoholismo = objHC.antPerNoPat_Alcoholismo != null && objHC.antPerNoPat_Alcoholismo != string.Empty ? objHC.antPerNoPat_Alcoholismo.ToString() : "";
                    objStrHC.antPerNoPat_AlcoholismoAnios = objHC.antPerNoPat_AlcoholismoAnios != null && objHC.antPerNoPat_AlcoholismoAnios != string.Empty ? objHC.antPerNoPat_AlcoholismoAnios.ToString() : "";
                    objStrHC.antPerNoPat_esDrogadiccion = objHC.antPerNoPat_esDrogadiccion != null && objHC.antPerNoPat_esDrogadiccion != string.Empty ? "AFIRMADO" : "NEGADO";
                    objStrHC.antPerNoPat_esMarihuana = objHC.antPerNoPat_esMarihuana != null && objHC.antPerNoPat_esMarihuana != string.Empty ? "AFIRMADO" : "NEGADO";
                    objStrHC.antPerNoPat_esCocaina = objHC.antPerNoPat_esCocaina != null && objHC.antPerNoPat_esCocaina != string.Empty ? "AFIRMADO" : "NEGADO";
                    objStrHC.antPerNoPat_esAnfetaminas = objHC.antPerNoPat_esAnfetaminas != null && objHC.antPerNoPat_esAnfetaminas != string.Empty ? "AFIRMADO" : "NEGADO";
                    objStrHC.antPerNoPat_Otros = objHC.antPerNoPat_Otros != null && objHC.antPerNoPat_Otros != string.Empty ? objHC.antPerNoPat_Otros.ToString() : "";
                    objStrHC.antPerNoPat_Inmunizaciones = objHC.antPerNoPat_Inmunizaciones != null && objHC.antPerNoPat_Inmunizaciones != string.Empty ? objHC.antPerNoPat_Inmunizaciones.ToString() : "";
                    objStrHC.antPerNoPat_Tetanicos = objHC.antPerNoPat_Tetanicos != null && objHC.antPerNoPat_Tetanicos != string.Empty ? objHC.antPerNoPat_Tetanicos.ToString() : "";
                    objStrHC.antPerNoPat_FechaAntitetanica = !string.IsNullOrEmpty(objHC.antPerNoPat_FechaAntitetanica) ? Convert.ToDateTime(objHC.antPerNoPat_FechaAntitetanica).ToString("dd/MM/yyyy") : "";
                    objStrHC.antPerNoPat_Hepatitis = objHC.antPerNoPat_Hepatitis != null && objHC.antPerNoPat_Hepatitis != string.Empty ? objHC.antPerNoPat_Hepatitis.ToString() : "";
                    objStrHC.antPerNoPat_Influenza = objHC.antPerNoPat_Influenza != null && objHC.antPerNoPat_Influenza != string.Empty ? objHC.antPerNoPat_Influenza.ToString() : "";
                    objStrHC.antPerNoPat_FechaInfluenza = !string.IsNullOrEmpty(objHC.antPerNoPat_FechaInfluenza) ? Convert.ToDateTime(objHC.antPerNoPat_FechaInfluenza).ToString("dd/MM/yyyy") : "";
                    objStrHC.antPerNoPat_Infancia = objHC.antPerNoPat_Infancia != null && objHC.antPerNoPat_Infancia != string.Empty ? objHC.antPerNoPat_Infancia.ToString() : "";
                    objStrHC.antPerNoPat_DescInfancia = objHC.antPerNoPat_DescInfancia != null && objHC.antPerNoPat_DescInfancia != string.Empty ? objHC.antPerNoPat_DescInfancia.ToString() : "";
                    objStrHC.antPerNoPat_Alimentacion = objHC.antPerNoPat_Alimentacion != null && objHC.antPerNoPat_Alimentacion != string.Empty ? objHC.antPerNoPat_Alimentacion.ToString() : "";
                    objStrHC.antPerNoPat_Higiene = objHC.antPerNoPat_Higiene != null && objHC.antPerNoPat_Higiene != string.Empty ? objHC.antPerNoPat_Higiene.ToString() : "";
                    objStrHC.antPerNoPat_MedicacionActual = objHC.antPerNoPat_MedicacionActual != null && objHC.antPerNoPat_MedicacionActual != string.Empty ? objHC.antPerNoPat_MedicacionActual.ToString() : "";
                    objStrHC.antPerPat_esNeoplasicos = !string.IsNullOrEmpty(objHC.antPerPat_esNeoplasicos) ? objHC.antPerPat_esNeoplasicos.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esNeumopatias = !string.IsNullOrEmpty(objHC.antPerPat_esNeumopatias) ? objHC.antPerPat_esNeumopatias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esAsma = !string.IsNullOrEmpty(objHC.antPerPat_esAsma) ? objHC.antPerPat_esAsma.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esFimico = !string.IsNullOrEmpty(objHC.antPerPat_esFimico) ? objHC.antPerPat_esFimico.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esNeumoconiosis = !string.IsNullOrEmpty(objHC.antPerPat_esNeumoconiosis) ? objHC.antPerPat_esNeumoconiosis.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esCardiopatias = !string.IsNullOrEmpty(objHC.antPerPat_esCardiopatias) ? objHC.antPerPat_esCardiopatias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esReumaticos = !string.IsNullOrEmpty(objHC.antPerPat_esReumaticos) ? objHC.antPerPat_esReumaticos.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esAlergias = !string.IsNullOrEmpty(objHC.antPerPat_esAlergias) ? objHC.antPerPat_esAlergias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esHipertension = !string.IsNullOrEmpty(objHC.antPerPat_esHipertension) ? objHC.antPerPat_esHipertension.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esHepatitis = !string.IsNullOrEmpty(objHC.antPerPat_esHepatitis) ? objHC.antPerPat_esHepatitis.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esTifoidea = !string.IsNullOrEmpty(objHC.antPerPat_esTifoidea) ? objHC.antPerPat_esTifoidea.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esHernias = !string.IsNullOrEmpty(objHC.antPerPat_esHernias) ? objHC.antPerPat_esHernias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esLumbalgias = !string.IsNullOrEmpty(objHC.antPerPat_esLumbalgias) ? objHC.antPerPat_esLumbalgias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esDiabetes = !string.IsNullOrEmpty(objHC.antPerPat_esDiabetes) ? objHC.antPerPat_esDiabetes.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esEpilepsias = !string.IsNullOrEmpty(objHC.antPerPat_esEpilepsias) ? objHC.antPerPat_esEpilepsias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esVenereas = !string.IsNullOrEmpty(objHC.antPerPat_esVenereas) ? objHC.antPerPat_esVenereas.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esCirugias = !string.IsNullOrEmpty(objHC.antPerPat_esCirugias) ? objHC.antPerPat_esCirugias.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_esFracturas = !string.IsNullOrEmpty(objHC.antPerPat_esFracturas) ? objHC.antPerPat_esFracturas.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.antPerPat_ObservacionesPat = objHC.antPerPat_ObservacionesPat != null && objHC.antPerPat_ObservacionesPat != string.Empty ? objHC.antPerPat_ObservacionesPat.ToString() : "";
                    objStrHC.intApaSis_esRespiratorio = objHC.intApaSis_esRespiratorio != null && objHC.intApaSis_esRespiratorio != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esDigestivo = objHC.intApaSis_esDigestivo != null && objHC.intApaSis_esDigestivo != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esCardiovascular = objHC.intApaSis_esCardiovascular != null && objHC.intApaSis_esCardiovascular != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esNervioso = objHC.intApaSis_esNervioso != null && objHC.intApaSis_esNervioso != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esUrinario = objHC.intApaSis_esUrinario != null && objHC.intApaSis_esUrinario != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esEndocrino = objHC.intApaSis_esEndocrino != null && objHC.intApaSis_esEndocrino != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esPsiquiatrico = objHC.intApaSis_esPsiquiatrico != null && objHC.intApaSis_esPsiquiatrico != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esEsqueletico = objHC.intApaSis_esEsqueletico != null && objHC.intApaSis_esEsqueletico != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esAudicion = objHC.intApaSis_esAudicion != null && objHC.intApaSis_esAudicion != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esVision = objHC.intApaSis_esVision != null && objHC.intApaSis_esVision != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esOlfato = objHC.intApaSis_esOlfato != null && objHC.intApaSis_esOlfato != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_esTacto = objHC.intApaSis_esTacto != null && objHC.intApaSis_esTacto != string.Empty ? "AFIRMADOS" : "NEGADOS";
                    objStrHC.intApaSis_ObservacionesPat = objHC.intApaSis_ObservacionesPat != null && objHC.intApaSis_ObservacionesPat != string.Empty ? objHC.intApaSis_ObservacionesPat.ToString() : "";
                    objStrHC.padAct_PadActuales = objHC.padAct_PadActuales != null && objHC.padAct_PadActuales != string.Empty ? objHC.padAct_PadActuales.ToString() : "";
                    objStrHC.expFSV_TArterial = objHC.expFSV_TArterial != null && objHC.expFSV_TArterial != string.Empty ? objHC.expFSV_TArterial.ToString() : "";
                    objStrHC.expFSV_Pulso = objHC.expFSV_Pulso != null && objHC.expFSV_Pulso != string.Empty ? objHC.expFSV_Pulso.ToString() : "";
                    objStrHC.expFSV_Temp = objHC.expFSV_Temp != null && objHC.expFSV_Temp != string.Empty ? objHC.expFSV_Temp.ToString() : "";
                    objStrHC.expFSV_FCardiaca = objHC.expFSV_FCardiaca != null && objHC.expFSV_FCardiaca != string.Empty ? objHC.expFSV_FCardiaca.ToString() : "";
                    objStrHC.expFSV_FResp = objHC.expFSV_FResp != null && objHC.expFSV_FResp != string.Empty ? objHC.expFSV_FResp.ToString() : "";
                    objStrHC.expFSV_Peso = objHC.expFSV_Peso != null && objHC.expFSV_Peso != string.Empty ? objHC.expFSV_Peso.ToString() : "";
                    objStrHC.expFSV_Talla = objHC.expFSV_Talla != null && objHC.expFSV_Talla != string.Empty ? objHC.expFSV_Talla.ToString() : "";
                    objStrHC.expFSV_IMC = objHC.expFSV_IMC != null && objHC.expFSV_IMC != string.Empty ? objHC.expFSV_IMC.ToString() : "";
                    objStrHC.expFC_Craneo = objHC.expFC_Craneo != null && objHC.expFC_Craneo != string.Empty ? objHC.expFC_Craneo.ToString() : "";
                    objStrHC.expFC_Parpados = objHC.expFC_Parpados != null && objHC.expFC_Parpados != string.Empty ? objHC.expFC_Parpados.ToString() : "";
                    objStrHC.expFC_Conjutiva = objHC.expFC_Conjutiva != null && objHC.expFC_Conjutiva != string.Empty ? objHC.expFC_Conjutiva.ToString() : "";
                    objStrHC.expFC_Reflejos = objHC.expFC_Reflejos != null && objHC.expFC_Reflejos != string.Empty ? objHC.expFC_Reflejos.ToString() : "";
                    objStrHC.expFC_FosasNasales = objHC.expFC_FosasNasales != null && objHC.expFC_FosasNasales != string.Empty ? objHC.expFC_FosasNasales.ToString() : "";
                    objStrHC.expFC_Boca = objHC.expFC_Boca != null && objHC.expFC_Boca != string.Empty ? objHC.expFC_Boca.ToString() : "";
                    objStrHC.expFC_Amigdalas = objHC.expFC_Amigdalas != null && objHC.expFC_Amigdalas != string.Empty ? objHC.expFC_Amigdalas.ToString() : "";
                    objStrHC.expFC_Dentadura = objHC.expFC_Dentadura != null && objHC.expFC_Dentadura != string.Empty ? objHC.expFC_Dentadura.ToString() : "";
                    objStrHC.expFC_Encias = objHC.expFC_Encias != null && objHC.expFC_Encias != string.Empty ? objHC.expFC_Encias.ToString() : "";
                    objStrHC.expFC_Cuello = objHC.expFC_Cuello != null && objHC.expFC_Cuello != string.Empty ? objHC.expFC_Cuello.ToString() : "";
                    objStrHC.expFC_Tiroides = objHC.expFC_Tiroides != null && objHC.expFC_Tiroides != string.Empty ? objHC.expFC_Tiroides.ToString() : "";
                    objStrHC.expFC_Ganglios = objHC.expFC_Ganglios != null && objHC.expFC_Ganglios != string.Empty ? objHC.expFC_Ganglios.ToString() : "";
                    objStrHC.expFC_Oidos = objHC.expFC_Oidos != null && objHC.expFC_Oidos != string.Empty ? objHC.expFC_Oidos.ToString() : "";
                    objStrHC.expFC_Otros = objHC.expFC_Otros != null && objHC.expFC_Otros != string.Empty ? objHC.expFC_Otros.ToString() : "";
                    objStrHC.expFC_Observaciones = objHC.expFC_Observaciones != null && objHC.expFC_Observaciones != string.Empty ? objHC.expFC_Observaciones.ToString() : "";
                    objStrHC.expFAV_VisCerAmbosOjos = objHC.expFAV_VisCerAmbosOjos != null && objHC.expFAV_VisCerAmbosOjos != string.Empty ? objHC.expFAV_VisCerAmbosOjos.ToString() : "";
                    objStrHC.expFAV_VisCerOjoIzq = objHC.expFAV_VisCerOjoIzq != null && objHC.expFAV_VisCerOjoIzq != string.Empty ? objHC.expFAV_VisCerOjoIzq.ToString() : "";
                    objStrHC.expFAV_VisCerOjoDer = objHC.expFAV_VisCerOjoDer != null && objHC.expFAV_VisCerOjoDer != string.Empty ? objHC.expFAV_VisCerOjoDer.ToString() : "";
                    objStrHC.expFAV_VisLejAmbosOjos = objHC.expFAV_VisLejAmbosOjos != null && objHC.expFAV_VisLejAmbosOjos != string.Empty ? objHC.expFAV_VisLejAmbosOjos.ToString() : "";
                    objStrHC.expFAV_VisLejOjoIzq = objHC.expFAV_VisLejOjoIzq != null && objHC.expFAV_VisLejOjoIzq != string.Empty ? objHC.expFAV_VisLejOjoIzq.ToString() : "";
                    objStrHC.expFAV_VisLejOjoDer = objHC.expFAV_VisLejOjoDer != null && objHC.expFAV_VisLejOjoDer != string.Empty ? objHC.expFAV_VisLejOjoDer.ToString() : "";
                    objStrHC.expFAV_CorregidaAmbosOjos = objHC.expFAV_CorregidaAmbosOjos != null && objHC.expFAV_CorregidaAmbosOjos != string.Empty ? objHC.expFAV_CorregidaAmbosOjos.ToString() : "";
                    objStrHC.expFAV_CorregidaOjoIzq = objHC.expFAV_CorregidaOjoIzq != null && objHC.expFAV_CorregidaOjoIzq != string.Empty ? objHC.expFAV_CorregidaOjoIzq.ToString() : "";
                    objStrHC.expFAV_CorregidaOjoDer = objHC.expFAV_CorregidaOjoDer != null && objHC.expFAV_CorregidaOjoDer != string.Empty ? objHC.expFAV_CorregidaOjoDer.ToString() : "";
                    objStrHC.expFAV_CampimetriaOI = objHC.expFAV_CampimetriaOI != null && objHC.expFAV_CampimetriaOI != string.Empty ? objHC.expFAV_CampimetriaOI.ToString() : "";
                    objStrHC.expFAV_CampimetriaOD = objHC.expFAV_CampimetriaOD != null && objHC.expFAV_CampimetriaOD != string.Empty ? objHC.expFAV_CampimetriaOD.ToString() : "";
                    objStrHC.expFAV_PterigionOI = objHC.expFAV_PterigionOI != null && objHC.expFAV_PterigionOI != string.Empty ? objHC.expFAV_PterigionOI.ToString() : "";
                    objStrHC.expFAV_PterigionOD = objHC.expFAV_PterigionOD != null && objHC.expFAV_PterigionOD != string.Empty ? objHC.expFAV_PterigionOD.ToString() : "";
                    objStrHC.expFAV_FondoOjo = objHC.expFAV_FondoOjo != null && objHC.expFAV_FondoOjo != string.Empty ? objHC.expFAV_FondoOjo.ToString() : "";
                    objStrHC.expFAV_Daltonismo = objHC.expFAV_Daltonismo != null && objHC.expFAV_Daltonismo != string.Empty ? objHC.expFAV_Daltonismo.ToString() : "";
                    objStrHC.expFAV_Observaciones = objHC.expFAV_Observaciones != null && objHC.expFAV_Observaciones != string.Empty ? objHC.expFAV_Observaciones.ToString() : "";
                    objStrHC.expFTATE_esCamposPulmonares = Convert.ToBoolean(objHC.expFTATE_esCamposPulmonares) ? "OK" : "NO";
                    objStrHC.expFTATE_esPuntosDolorosos = Convert.ToBoolean(objHC.expFTATE_esPuntosDolorosos) ? "OK" : "NO";
                    objStrHC.expFTATE_esGenitales = Convert.ToBoolean(objHC.expFTATE_esGenitales) ? "OK" : "NO";
                    objStrHC.expFTATE_esRuidosCardiacos = Convert.ToBoolean(objHC.expFTATE_esRuidosCardiacos) ? "OK" : "NO";
                    objStrHC.expFTATE_esHallusValgus = Convert.ToBoolean(objHC.expFTATE_esHallusValgus) ? "OK" : "NO";
                    objStrHC.expFTATE_esHerniasUmbili = Convert.ToBoolean(objHC.expFTATE_esHerniasUmbili) ? "OK" : "NO";
                    objStrHC.expFTATE_esAreaRenal = Convert.ToBoolean(objHC.expFTATE_esAreaRenal) ? "OK" : "NO";
                    objStrHC.expFTATE_esVaricocele = Convert.ToBoolean(objHC.expFTATE_esVaricocele) ? "OK" : "NO";
                    objStrHC.expFTATE_esGrandulasMamarias = Convert.ToBoolean(objHC.expFTATE_esGrandulasMamarias) ? "OK" : "NO";
                    objStrHC.expFTATE_esColumnaVertebral = Convert.ToBoolean(objHC.expFTATE_esColumnaVertebral) ? "OK" : "NO";
                    objStrHC.expFTATE_esPiePlano = Convert.ToBoolean(objHC.expFTATE_esPiePlano) ? "OK" : "NO";
                    objStrHC.expFTATE_esVarices = Convert.ToBoolean(objHC.expFTATE_esVarices) ? "OK" : "NO";
                    objStrHC.expFTATE_esMiembrosSup = Convert.ToBoolean(objHC.expFTATE_esMiembrosSup) ? "OK" : "NO";
                    objStrHC.expFTATE_esParedAbdominal = Convert.ToBoolean(objHC.expFTATE_esParedAbdominal) ? "OK" : "NO";
                    objStrHC.expFTATE_esAnillosInguinales = Convert.ToBoolean(objHC.expFTATE_esAnillosInguinales) ? "OK" : "NO";
                    objStrHC.expFTATE_esMiembrosInf = Convert.ToBoolean(objHC.expFTATE_esMiembrosInf) ? "OK" : "NO";
                    objStrHC.expFTATE_esTatuajes = Convert.ToBoolean(objHC.expFTATE_esTatuajes) ? "OK" : "NO";
                    objStrHC.expFTATE_esVisceromegalias = Convert.ToBoolean(objHC.expFTATE_esVisceromegalias) ? "OK" : "NO";
                    objStrHC.expFTATE_esMarcha = Convert.ToBoolean(objHC.expFTATE_esMarcha) ? "OK" : "NO";
                    objStrHC.expFTATE_esHerniasInguinales = Convert.ToBoolean(objHC.expFTATE_esHerniasInguinales) ? "OK" : "NO";
                    objStrHC.expFTATE_esHombrosDolorosos = Convert.ToBoolean(objHC.expFTATE_esHombrosDolorosos) ? "OK" : "NO";
                    objStrHC.expFTATE_esQuistes = Convert.ToBoolean(objHC.expFTATE_esQuistes) ? "OK" : "NO";
                    objStrHC.expFTATE_Observaciones = objHC.expFTATE_Observaciones != null && objHC.expFTATE_Observaciones != string.Empty ? objHC.expFTATE_Observaciones.ToString() : "";
                    objStrHC.expFTATE_MS_HombroDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_CodoDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_CodoIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_CodoDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_CodoIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_CodoDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_CodoIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_HombroDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_HombroIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_HombroIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoDer_esPronacion = Convert.ToBoolean(objHC.expFTATE_MS_CodoDer_esPronacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoIzq_esPronacion = Convert.ToBoolean(objHC.expFTATE_MS_CodoIzq_esPronacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esPronacion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esPronacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esPronacion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esPronacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoDer_esSupinacion = Convert.ToBoolean(objHC.expFTATE_MS_CodoDer_esSupinacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_CodoIzq_esSupinacion = Convert.ToBoolean(objHC.expFTATE_MS_CodoIzq_esSupinacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esSupinacion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esSupinacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esSupinacion = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esSupinacion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esDesvUlnar = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esDesvUlnar) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esDesvUlnar = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esDesvUlnar) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esDesvRadial = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esDesvRadial) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esDesvRadial = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esDesvRadial) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaDer_esOponencia = Convert.ToBoolean(objHC.expFTATE_MS_MunecaDer_esOponencia) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MunecaIzq_esOponencia = Convert.ToBoolean(objHC.expFTATE_MS_MunecaIzq_esOponencia) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosDer_esOponencia = Convert.ToBoolean(objHC.expFTATE_MS_DedosDer_esOponencia) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_DedosIzq_esOponencia = Convert.ToBoolean(objHC.expFTATE_MS_DedosIzq_esOponencia) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_RodillasDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_RodillasIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosDer_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_DedosDer_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosIzq_esFlexion = Convert.ToBoolean(objHC.expFTATE_MI_DedosIzq_esFlexion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_RodillasDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_RodillasIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosDer_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_DedosDer_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosIzq_esExtension = Convert.ToBoolean(objHC.expFTATE_MI_DedosIzq_esExtension) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosDer_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_DedosDer_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosIzq_esAbduccion = Convert.ToBoolean(objHC.expFTATE_MI_DedosIzq_esAbduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosDer_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_DedosDer_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_DedosIzq_esAduccion = Convert.ToBoolean(objHC.expFTATE_MI_DedosIzq_esAduccion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_RodillasDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_RodillasIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esRotInterna = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esRotInterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_CaderaDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CaderaIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_CaderaIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_RodillasDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_RodillasIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_RodillasIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esRotExterna = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esRotExterna) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esInversion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esInversion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esInversion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esInversion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieDer_esEversion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieDer_esEversion) ? "OK" : "NO";
                    objStrHC.expFTATE_MI_CllPieIzq_esEversion = Convert.ToBoolean(objHC.expFTATE_MI_CllPieIzq_esEversion) ? "OK" : "NO";
                    objStrHC.expFTATE_MS_MI_Observaciones = objHC.expFTATE_MS_MI_Observaciones != null && objHC.expFTATE_MS_MI_Observaciones != string.Empty ? objHC.expFTATE_MS_MI_Observaciones.ToString() : "";
                    objStrHC.expFDT_PielMucosas = objHC.expFDT_PielMucosas != null && objHC.expFDT_PielMucosas != string.Empty ? objHC.expFDT_PielMucosas.ToString() : "";
                    objStrHC.expFDT_EstadoPsiquiatrico = objHC.expFDT_EstadoPsiquiatrico != null && objHC.expFDT_EstadoPsiquiatrico != string.Empty ? objHC.expFDT_EstadoPsiquiatrico.ToString() : "";
                    objStrHC.expFDT_ExamenNeurologico = objHC.expFDT_ExamenNeurologico != null && objHC.expFDT_ExamenNeurologico != string.Empty ? objHC.expFDT_ExamenNeurologico.ToString() : "";
                    objStrHC.expFDT_FobiasActuales = objHC.expFDT_FobiasActuales != null && objHC.expFDT_FobiasActuales != string.Empty ? objHC.expFDT_FobiasActuales.ToString() : "";
                    objStrHC.expFDT_Higiene = objHC.expFDT_Higiene != null && objHC.expFDT_Higiene != string.Empty ? objHC.expFDT_Higiene.ToString() : "";
                    objStrHC.expFDT_ConstitucionFisica = objHC.expFDT_ConstitucionFisica != null && objHC.expFDT_ConstitucionFisica != string.Empty ? objHC.expFDT_ConstitucionFisica.ToString() : "";
                    objStrHC.expFDT_Otros = objHC.expFDT_Otros != null && objHC.expFDT_Otros != string.Empty ? objHC.expFDT_Otros.ToString() : "";
                    objStrHC.expFDT_Observaciones = objHC.expFDT_Observaciones != null && objHC.expFDT_Observaciones != string.Empty ? objHC.expFDT_Observaciones.ToString() : "";
                    objStrHC.estGab_TipoSanguineoID = objHC.estGab_TipoSanguineoID != null && objHC.estGab_TipoSanguineoID != string.Empty ? objHC.estGab_TipoSanguineoID.ToString() : "";
                    objStrHC.estGab_Antidoping = objHC.estGab_Antidoping != null && objHC.estGab_Antidoping != string.Empty ? objHC.estGab_Antidoping.ToString() : "";
                    objStrHC.estGab_Laboratorios = !string.IsNullOrEmpty(objHC.estGab_Laboratorios) ? objHC.estGab_Laboratorios.Trim().ToUpper() == "SI" ? "SI" : "NO" : "NO";
                    objStrHC.estGab_ObservacionesGrupoRH = objHC.estGab_ObservacionesGrupoRH != null && objHC.estGab_ObservacionesGrupoRH != string.Empty ? objHC.estGab_ObservacionesGrupoRH.ToString() : "";
                    objStrHC.estGab_ExamGenOrina = objHC.estGab_ExamGenOrina != null && objHC.estGab_ExamGenOrina != string.Empty ? objHC.estGab_ExamGenOrina.ToString() : "";
                    objStrHC.estGab_ExamGenOrinaObservaciones = objHC.estGab_ExamGenOrinaObservaciones != null && objHC.estGab_ExamGenOrinaObservaciones != string.Empty ? objHC.estGab_ExamGenOrinaObservaciones.ToString() : "";
                    objStrHC.estGab_Radiografias = objHC.estGab_Radiografias != null && objHC.estGab_Radiografias != string.Empty ? objHC.estGab_Radiografias.ToString() : "";
                    objStrHC.estGab_RadiografiasObservaciones = objHC.estGab_RadiografiasObservaciones != null && objHC.estGab_RadiografiasObservaciones != string.Empty ? objHC.estGab_RadiografiasObservaciones.ToString() : "";
                    objStrHC.estGab_Audiometria = objHC.estGab_Audiometria != null && objHC.estGab_Audiometria != string.Empty ? objHC.estGab_Audiometria.ToString() : "";
                    objStrHC.estGab_HBC = objHC.estGab_HBC != null && objHC.estGab_HBC != string.Empty ? objHC.estGab_HBC.ToString() : "";
                    objStrHC.estGab_AudiometriaObservaciones = objHC.estGab_AudiometriaObservaciones != null && objHC.estGab_AudiometriaObservaciones != string.Empty ? objHC.estGab_AudiometriaObservaciones.ToString() : "";
                    objStrHC.estGab_Espirometria = objHC.estGab_Espirometria != null && objHC.estGab_Espirometria != string.Empty ? objHC.estGab_Espirometria.ToString() : "";
                    objStrHC.estGab_EspirometriaObservaciones = objHC.estGab_EspirometriaObservaciones != null && objHC.estGab_EspirometriaObservaciones != string.Empty ? objHC.estGab_EspirometriaObservaciones.ToString() : "";
                    objStrHC.estGab_Electrocardiograma = objHC.estGab_Electrocardiograma != null && objHC.estGab_Electrocardiograma != string.Empty ? objHC.estGab_Electrocardiograma.ToString() : "";
                    objStrHC.estGab_ElectrocardiogramaObservaciones = objHC.estGab_ElectrocardiogramaObservaciones != null && objHC.estGab_ElectrocardiogramaObservaciones != string.Empty ? objHC.estGab_ElectrocardiogramaObservaciones.ToString() : "";
                    objStrHC.estGab_FechaPrimeraDosis = !string.IsNullOrEmpty(objHC.estGab_FechaPrimeraDosis) ? Convert.ToDateTime(objHC.estGab_FechaPrimeraDosis).ToString("dd/MM/yyyy") : "";
                    objStrHC.estGab_FechaSegundaDosis = !string.IsNullOrEmpty(objHC.estGab_FechaSegundaDosis) ? Convert.ToDateTime(objHC.estGab_FechaSegundaDosis).ToString("dd/MM/yyyy") : "";
                    objStrHC.estGab_MarcaDosisID = objHC.estGab_MarcaDosisID != null && objHC.estGab_MarcaDosisID != string.Empty ? objHC.estGab_MarcaDosisID.ToString() : "";
                    objStrHC.estGab_VacunacionObservaciones = objHC.estGab_VacunacionObservaciones != null && objHC.estGab_VacunacionObservaciones != string.Empty ? objHC.estGab_VacunacionObservaciones.ToString() : "";
                    objStrHC.estGab_LstProblemas = objHC.estGab_LstProblemas != null && objHC.estGab_LstProblemas != string.Empty ? objHC.estGab_LstProblemas.ToString() : "";
                    objStrHC.estGab_Recomendaciones = objHC.estGab_Recomendaciones != null && objHC.estGab_Recomendaciones != string.Empty ? objHC.estGab_Recomendaciones.ToString() : "";
                    objStrHC.esp_Espirometria = objHC.esp_Espirometria != null && objHC.esp_Espirometria != string.Empty ? objHC.esp_Espirometria.ToString() : "";
                    objStrHC.esp_EspirometriaObservaciones = objHC.esp_EspirometriaObservaciones != null && objHC.esp_EspirometriaObservaciones != string.Empty ? objHC.esp_EspirometriaObservaciones.ToString() : "";
                    objStrHC.aud_HipoacusiaOD = objHC.aud_HipoacusiaOD != null && objHC.aud_HipoacusiaOD != string.Empty ? objHC.aud_HipoacusiaOD.ToString() : "";
                    objStrHC.aud_HipoacusiaOI = objHC.aud_HipoacusiaOI != null && objHC.aud_HipoacusiaOI != string.Empty ? objHC.aud_HipoacusiaOI.ToString() : "";
                    objStrHC.aud_HBC = objHC.aud_HBC != null && objHC.aud_HBC != string.Empty ? objHC.aud_HBC.ToString() : "";
                    objStrHC.aud_Diagnostico = objHC.aud_Diagnostico != null && objHC.aud_Diagnostico != string.Empty ? objHC.aud_Diagnostico.ToString() : "";
                    objStrHC.aud_KH1 = objHC.aud_KH1 != null && objHC.aud_KH1 != string.Empty ? objHC.aud_KH1.ToString() : "";
                    objStrHC.aud_KH1_OI = objHC.aud_KH1_OI != null && objHC.aud_KH1_OI != string.Empty ? objHC.aud_KH1_OI.ToString() : "";
                    objStrHC.aud_KH1_OD = objHC.aud_KH1_OD != null && objHC.aud_KH1_OD != string.Empty ? objHC.aud_KH1_OD.ToString() : "";
                    objStrHC.aud_KH2 = objHC.aud_KH2 != null && objHC.aud_KH2 != string.Empty ? objHC.aud_KH2.ToString() : "";
                    objStrHC.aud_KH2_OI = objHC.aud_KH2_OI != null && objHC.aud_KH2_OI != string.Empty ? objHC.aud_KH2_OI.ToString() : "";
                    objStrHC.aud_KH2_OD = objHC.aud_KH2_OD != null && objHC.aud_KH2_OD != string.Empty ? objHC.aud_KH2_OD.ToString() : "";
                    objStrHC.aud_KH3 = objHC.aud_KH3 != null && objHC.aud_KH3 != string.Empty ? objHC.aud_KH3.ToString() : "";
                    objStrHC.aud_KH3_OI = objHC.aud_KH3_OI != null && objHC.aud_KH3_OI != string.Empty ? objHC.aud_KH3_OI.ToString() : "";
                    objStrHC.aud_KH3_OD = objHC.aud_KH3_OD != null && objHC.aud_KH3_OD != string.Empty ? objHC.aud_KH3_OD.ToString() : "";
                    objStrHC.aud_KH4 = objHC.aud_KH4 != null && objHC.aud_KH4 != string.Empty ? objHC.aud_KH4.ToString() : "";
                    objStrHC.aud_KH4_OI = objHC.aud_KH4_OI != null && objHC.aud_KH4_OI != string.Empty ? objHC.aud_KH4_OI.ToString() : "";
                    objStrHC.aud_KH4_OD = objHC.aud_KH4_OD != null && objHC.aud_KH4_OD != string.Empty ? objHC.aud_KH4_OD.ToString() : "";
                    objStrHC.aud_KH5 = objHC.aud_KH5 != null && objHC.aud_KH5 != string.Empty ? objHC.aud_KH5.ToString() : "";
                    objStrHC.aud_KH5_OI = objHC.aud_KH5_OI != null && objHC.aud_KH5_OI != string.Empty ? objHC.aud_KH5_OI.ToString() : "";
                    objStrHC.aud_KH5_OD = objHC.aud_KH5_OD != null && objHC.aud_KH5_OD != string.Empty ? objHC.aud_KH5_OD.ToString() : "";
                    objStrHC.aud_KH6 = objHC.aud_KH6 != null && objHC.aud_KH6 != string.Empty ? objHC.aud_KH6.ToString() : "";
                    objStrHC.aud_KH6_OI = objHC.aud_KH6_OI != null && objHC.aud_KH6_OI != string.Empty ? objHC.aud_KH6_OI.ToString() : "";
                    objStrHC.aud_KH6_OD = objHC.aud_KH6_OD != null && objHC.aud_KH6_OD != string.Empty ? objHC.aud_KH6_OD.ToString() : "";
                    objStrHC.aud_KH7 = objHC.aud_KH7 != null && objHC.aud_KH7 != string.Empty ? objHC.aud_KH7.ToString() : "";
                    objStrHC.aud_KH7_OI = objHC.aud_KH7_OI != null && objHC.aud_KH7_OI != string.Empty ? objHC.aud_KH7_OI.ToString() : "";
                    objStrHC.aud_KH7_OD = objHC.aud_KH7_OD != null && objHC.aud_KH7_OD != string.Empty ? objHC.aud_KH7_OD.ToString() : "";
                    objStrHC.aud_NotasAudiometria = objHC.aud_NotasAudiometria != null && objHC.aud_NotasAudiometria != string.Empty ? objHC.aud_NotasAudiometria.ToString() : "";
                    objStrHC.eleDer_Interpretacion = objHC.eleDer_Interpretacion != null && objHC.eleDer_Interpretacion != string.Empty ? objHC.eleDer_Interpretacion.ToString() : "";
                    objStrHC.radTCLP_Conclusiones = objHC.radTCLP_Conclusiones != null && objHC.radTCLP_Conclusiones != string.Empty ? objHC.radTCLP_Conclusiones.ToString() : "";
                    objStrHC.certMed_CertificadoMedico = objHC.certMed_CertificadoMedico != null && objHC.certMed_CertificadoMedico != string.Empty ? objHC.certMed_CertificadoMedico.ToString() : "";
                    objStrHC.certMed_AptitudID = objHC.certMed_AptitudID != null && objHC.certMed_AptitudID != string.Empty ? objHC.certMed_AptitudID.ToString() : "";
                    objStrHC.certMed_Fecha = !string.IsNullOrEmpty(objHC.certMed_Fecha) ? Convert.ToDateTime(objHC.certMed_Fecha).ToString("dd/MM/yyyy") : "";
                    objStrHC.certMed_NombrePaciente = objHC.certMed_NombrePaciente != null && objHC.certMed_NombrePaciente != string.Empty ? objHC.certMed_NombrePaciente.ToString() : "";
                    objStrHC.recom_Recomendaciones = objHC.recom_Recomendaciones != null && objHC.recom_Recomendaciones != string.Empty ? objHC.recom_Recomendaciones.ToString() : "";
                    objStrHC.medicoCreacionID = objHC.medicoCreacionID != null && objHC.medicoCreacionID != string.Empty ? objHC.medicoCreacionID.ToString() : "";
                    objStrHC.medicoCreacionPuesto = objHC.medicoCreacionPuesto != null && objHC.medicoCreacionPuesto != string.Empty ? objHC.medicoCreacionPuesto.ToString() : "";
                    objStrHC.medicoCreacionCedula = objHC.medicoCreacionCedula != null && objHC.medicoCreacionCedula != string.Empty ? objHC.medicoCreacionCedula.ToString() : "";
                    #endregion
                    //lista de imagenes del Historial
                    List<ArchivoDTO> imagesHC = new List<ArchivoDTO>();
                    imagesHC = _context.tblS_SO_Archivos.Where(e => e.idHC == idHistorialClinico).Select(e => new ArchivoDTO
                    { 
                        id = e.id,
                        idHC = e.idHC,
                        rutaArchivo = e.rutaArchivo,
                        tipoArchivo = e.tipoArchivo
                    }).ToList();
                    
                    resultado.Add("imagesHC", imagesHC);
                    resultado.Add("objHC", objStrHC);
                    //resultado.Add(SUCCESS, true);
                    #endregion

                }
                catch (Exception e)
                {
                    LogError(16, 16, NombreControlador, "GetReportDataHistorialClinico", e, AccionEnum.CONSULTA, idHistorialClinico, 0);
                    resultado.Add(MESSAGE, e.Message);
                    //resultado.Add(SUCCESS, false);
                }
            
            return resultado;
        }

        public Dictionary<string, object> GetReportDataCertificado()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    var emp = _context.tblS_SO_Medicos.Where(e => e.idUsuarioSIGOPLAN == (int)vSesiones.sesionUsuarioDTO.id).FirstOrDefault();
                    
                    if(emp != null){
                        medicoDTO med = new medicoDTO()
                        {
                            id = emp.id,
                            nombre = emp.nombre,
                            puesto = emp.puesto,
                            cedulaProfesional = emp.cedulaProfesional,
                            empresa = emp.empresa,
                            idUsuarioSIGOPLAN = emp.idUsuarioSIGOPLAN,
                            idUsuarioCreacion = emp.idUsuarioCreacion,
                            idUsuarioModificacion = emp.idUsuarioModificacion,
                            fechaCreacion = emp.fechaCreacion,
                            fechaModificacion = emp.fechaModificacion,
                            registroActivo = emp.registroActivo
                        };
                        resultado.Add(SUCCESS, true);
                        resultado.Add("empleado", med);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("empleado", null);
                    }
                }catch(Exception e){
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }
            

            //resultado.Add("",);

            return resultado;
        }

        #endregion

    }
}
