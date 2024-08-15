using Core.DAO.Administracion.Seguridad.Evaluacion;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.Evaluacion;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.ChartJS;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contratistas;
using Core.Entity.Administrativo.Seguridad.Evaluacion;
using Core.Enum.Administracion.Seguridad.Evaluacion;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Administracion.Seguridad.Evaluacion
{
    public class EvaluacionDAO : GenericDAO<tblSED_Actividad>, IEvaluacionDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        UsuarioFactoryServices ufs = new UsuarioFactoryServices();

        private readonly bool productivo = true;
        private readonly int divisionActual = 0;
        private const string NombreBaseEvidencia = @"Evidencia";
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\SEGURIDAD_EVALUACION";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\SEGURIDAD_EVALUACION";
        private readonly string RutaEvidencia;

        #region Constructor
        public EvaluacionDAO()
        {
            resultado.Clear();

            // Local
            // Descomentar esta línea de código para hacer pruebas en local.
#if DEBUG
            RutaBase = RutaLocal;            
#endif

            divisionActual = vSesiones.sesionDivisionActual;
            RutaEvidencia = Path.Combine(RutaBase, "EVIDENCIA");
        }
        #endregion

        #region Catálogos
        public Dictionary<string, object> getActividades()
        {
            try
            {
                var data = _context.tblSED_Actividad.Where(x => x.estatus && x.division == divisionActual).Select(x => new ActividadDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    ponderacion = x.ponderacion,
                    estatus = x.estatus,
                    aplica = false,
                    periodicidad = 0
                }).ToList();
                var combo = data.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion,
                    Prefijo = x.ponderacion.ToString()
                }).ToList();

                resultado.Add("data", data);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaActividad(tblSED_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    actividad.division = divisionActual;

                    _context.tblSED_Actividad.Add(actividad);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarActividad(tblSED_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblSED_Actividad.FirstOrDefault(x => x.id == actividad.id && x.division == divisionActual);

                    actividadSIGOPLAN.descripcion = actividad.descripcion;
                    actividadSIGOPLAN.ponderacion = actividad.ponderacion;
                    actividadSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarActividad(tblSED_Actividad actividad)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var actividadSIGOPLAN = _context.tblSED_Actividad.FirstOrDefault(x => x.id == actividad.id && x.division == divisionActual);

                    actividadSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    #region Eliminar las relaciones puesto-actividad
                    var relaciones = _context.tblSED_RelPuestoActividad.Where(x => x.estatus && x.actividadID == actividadSIGOPLAN.id && x.division == divisionActual).ToList();

                    foreach (var rel in relaciones)
                    {
                        rel.estatus = false;
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getPuestos()
        {
            try
            {
                var data = _context.tblSED_Puesto.Where(x => x.estatus && x.division == divisionActual).Select(x => new PuestoDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    categoria = x.categoria,
                    estatus = x.estatus,
                    categoriaDesc = x.categoria.ToString()
                }).ToList();
                var combo = data.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.descripcion,
                    Prefijo = x.categoria.ToString()
                }).ToList();

                resultado.Add("data", data);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getActividadesPuesto(int puestoID)
        {
            try
            {
                var actividades = _context.tblSED_Actividad.Where(x => x.estatus && x.division == divisionActual).Select(x => new ActividadDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    ponderacion = x.ponderacion,
                    estatus = x.estatus,
                    aplica = false,
                    periodicidad = 0
                }).ToList();

                var actividadesPorPuesto = _context.tblSED_RelPuestoActividad.Where(x => x.estatus && x.puestoID == puestoID && x.division == divisionActual).ToList();

                foreach (var act in actividadesPorPuesto)
                {
                    var actExist = actividades.FirstOrDefault(x => x.id == act.actividadID);

                    if (actExist != null)
                    {
                        actExist.aplica = true;
                        actExist.periodicidad = act.periodicidad;
                    }
                }

                resultado.Add("data", actividades.OrderByDescending(x => x.aplica).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    puesto.division = divisionActual;

                    _context.tblSED_Puesto.Add(puesto);
                    _context.SaveChanges();

                    if (actividades != null)
                    {
                        foreach (var act in actividades)
                        {
                            _context.tblSED_RelPuestoActividad.Add(new tblSED_RelPuestoActividad
                            {
                                puestoID = puesto.id,
                                actividadID = act.id,
                                periodicidad = act.periodicidad,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarPuesto(tblSED_Puesto puesto, List<ActividadDTO> actividades)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var puestoSIGOPLAN = _context.tblSED_Puesto.FirstOrDefault(x => x.id == puesto.id && x.division == divisionActual);

                    puestoSIGOPLAN.descripcion = puesto.descripcion;
                    puestoSIGOPLAN.categoria = puesto.categoria;
                    puestoSIGOPLAN.division = divisionActual;
                    _context.SaveChanges();

                    var actividadesAnteriores = _context.tblSED_RelPuestoActividad.Where(x => x.puestoID == puesto.id && x.division == divisionActual).ToList();

                    foreach (var act in actividadesAnteriores)
                    {
                        act.estatus = false;
                        _context.SaveChanges();
                    }

                    if (actividades != null)
                    {
                        foreach (var act in actividades)
                        {
                            _context.tblSED_RelPuestoActividad.Add(new tblSED_RelPuestoActividad
                            {
                                puestoID = puesto.id,
                                actividadID = act.id,
                                periodicidad = act.periodicidad,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarPuesto(tblSED_Puesto puesto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var puestoSIGOPLAN = _context.tblSED_Puesto.FirstOrDefault(x => x.id == puesto.id && x.division == divisionActual);

                    puestoSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    #region Quitar el puesto de los empleados registrados
                    var empleados = _context.tblSED_Empleado.Where(x => x.estatus && x.puestoEvaluacionID == puestoSIGOPLAN.id && x.division == divisionActual).ToList();

                    foreach (var emp in empleados)
                    {
                        emp.puestoEvaluacionID = 0;
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getEmpleados()
        {
            try
            {
                var puestos = _context.tblSED_Puesto.Where(x => x.estatus && x.division == divisionActual).ToList();

                var data = _context.tblSED_Empleado.ToList().Where(x => x.estatus && x.division == divisionActual).Select(x => new EmpleadoDTO
                {
                    id = x.id,
                    claveEmpleado = x.claveEmpleado,
                    nombre = x.nombre,
                    apellidoPaterno = x.apellidoPaterno,
                    apellidoMaterno = x.apellidoMaterno,
                    puestoEvaluacionID = x.puestoEvaluacionID,
                    puestoDesc = puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID) != null ? puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID).descripcion : "",
                    evaluador = x.evaluador,
                    rol = x.rol,
                    fechaInicioRol = x.fechaInicioRol,
                    cc = x.cc,
                    estatus = x.estatus,
                    idEmpresa = x.idEmpresa,
                    idAgrupacion = (int)x.idAgrupacion
                }).ToList();
                var combo = data.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                }).OrderBy(x => x.Text).ToList();
                var comboEmpleados = data
                    //.Where(x => !x.evaluador)
                    .Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                }).OrderBy(x => x.Text).ToList();
                var comboEvaluadores = data.Where(x => x.evaluador).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", data);
                resultado.Add("dataCombo", combo);
                resultado.Add("dataComboEmpleados", comboEmpleados);
                resultado.Add("dataComboEvaluadores", comboEvaluadores);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getEvaluadores()
        {
            try
            {
                var puestos = _context.tblSED_Puesto.Where(x => x.estatus && x.division == divisionActual).ToList();

                var data = _context.tblSED_Empleado.ToList().Where(x => x.estatus && x.evaluador && x.division == divisionActual).Select(x => new EmpleadoDTO
                {
                    id = x.id,
                    claveEmpleado = x.claveEmpleado,
                    nombre = x.nombre,
                    apellidoPaterno = x.apellidoPaterno,
                    apellidoMaterno = x.apellidoMaterno,
                    puestoEvaluacionID = x.puestoEvaluacionID,
                    puestoDesc = puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID) != null ? puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID).descripcion : "",
                    evaluador = x.evaluador,
                    rol = x.rol,
                    fechaInicioRol = x.fechaInicioRol,
                    estatus = x.estatus,
                    idEmpresa = x.idEmpresa,
                    idAgrupacion = (int)x.idAgrupacion
                }).ToList();
                var combo = data.Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno),
                    Prefijo = x.puestoEvaluacionID.ToString()
                }).OrderBy(x => x.Text).ToList();

                resultado.Add("data", data);
                resultado.Add("dataCombo", combo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getEvaluadoresEmpleado(int empleadoID)
        {
            try
            {
                var puestos = _context.tblSED_Puesto.Where(x => x.estatus && x.division == divisionActual).ToList();

                var evaluadores = _context.tblSED_Empleado.ToList().Where(x => x.estatus && x.evaluador && x.division == divisionActual).Select(x => new EmpleadoDTO
                {
                    id = x.id,
                    claveEmpleado = x.claveEmpleado,
                    nombre = x.nombre,
                    apellidoPaterno = x.apellidoPaterno,
                    apellidoMaterno = x.apellidoMaterno,
                    puestoEvaluacionID = x.puestoEvaluacionID,
                    evaluador = x.evaluador,
                    rol = x.rol,
                    fechaInicioRol = x.fechaInicioRol,
                    estatus = x.estatus,
                    puestoDesc = puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID) != null ? puestos.FirstOrDefault(y => y.id == x.puestoEvaluacionID).descripcion : "",
                    aplica = false,
                    idEmpresa = x.idEmpresa,
                    idAgrupacion = (int)x.idAgrupacion
                }).ToList();

                var evaluadoresPorEmpleado = _context.tblSED_RelEmpleadoEvaluador.Where(x => x.estatus && x.empleadoID == empleadoID && x.division == divisionActual).ToList();

                foreach (var eva in evaluadoresPorEmpleado)
                {
                    var evaExist = evaluadores.FirstOrDefault(x => x.id == eva.evaluadorID);

                    if (evaExist != null)
                    {
                        evaExist.aplica = true;
                    }
                }

                resultado.Add("data", evaluadores.OrderByDescending(x => x.aplica).ToList());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (empleado.idAgrupacion == null || empleado.idAgrupacion == 0)
                    {
                        throw new Exception("Debe seleccionar un centro de costos.");
                    }

                    empleado.apellidoPaterno = empleado.apellidoPaterno ?? "";
                    empleado.apellidoMaterno = empleado.apellidoMaterno ?? "";
                    empleado.division = divisionActual;
                    empleado.cc = "";
                    _context.tblSED_Empleado.Add(empleado);
                    _context.SaveChanges();

                    if (evaluadores != null)
                    {
                        foreach (var eva in evaluadores)
                        {
                            _context.tblSED_RelEmpleadoEvaluador.Add(new tblSED_RelEmpleadoEvaluador
                            {
                                empleadoID = empleado.id,
                                evaluadorID = eva.id,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarEmpleado(tblSED_Empleado empleado, List<EmpleadoDTO> evaluadores)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (empleado.idAgrupacion == null || empleado.idAgrupacion == 0)
                    {
                        throw new Exception("Debe seleccionar un centro de costos.");
                    }

                    var empleadoSIGOPLAN = _context.tblSED_Empleado.FirstOrDefault(x => x.id == empleado.id && x.division == divisionActual);

                    empleadoSIGOPLAN.claveEmpleado = empleado.claveEmpleado;
                    empleadoSIGOPLAN.nombre = empleado.nombre ?? "";
                    empleadoSIGOPLAN.apellidoPaterno = empleado.apellidoPaterno ?? "";
                    empleadoSIGOPLAN.apellidoMaterno = empleado.apellidoMaterno ?? "";
                    empleadoSIGOPLAN.puestoEvaluacionID = empleado.puestoEvaluacionID;
                    empleadoSIGOPLAN.evaluador = empleado.evaluador;
                    empleadoSIGOPLAN.rol = empleado.rol;
                    empleadoSIGOPLAN.fechaInicioRol = empleado.fechaInicioRol;
                    empleadoSIGOPLAN.cc = "";
                    empleadoSIGOPLAN.division = divisionActual;
                    empleadoSIGOPLAN.estatus = empleado.estatus;
                    empleadoSIGOPLAN.idEmpresa = empleado.idEmpresa;
                    empleadoSIGOPLAN.idAgrupacion = empleado.idAgrupacion;
                    _context.SaveChanges();

                    var evaluadoresAnteriores = _context.tblSED_RelEmpleadoEvaluador.Where(x => x.empleadoID == empleado.id && x.division == divisionActual).ToList();

                    foreach (var eva in evaluadoresAnteriores)
                    {
                        eva.estatus = false;
                        _context.SaveChanges();
                    }

                    if (evaluadores != null)
                    {
                        foreach (var eva in evaluadores)
                        {
                            _context.tblSED_RelEmpleadoEvaluador.Add(new tblSED_RelEmpleadoEvaluador
                            {
                                empleadoID = empleado.id,
                                evaluadorID = eva.id,
                                division = divisionActual,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarEmpleado(tblSED_Empleado empleado)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var empleadoSIGOPLAN = _context.tblSED_Empleado.FirstOrDefault(x => x.id == empleado.id && x.division == divisionActual);

                    empleadoSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado, bool esContratista, int idEmpresaContratista)
        {
            try
            {
                if (!esContratista)
                {
                    #region EMPLEADO INTERNO
                    //var empleadoEK = ContextEnKontrolNomina.Where(string.Format(@"SELECT * FROM sn_empleados WHERE clave_empleado = {0}", claveEmpleado));
                    var empleado = _context.tblRH_EK_Empleados.Where(e => e.clave_empleado == claveEmpleado).FirstOrDefault();
                    if (empleado != null)
                    {
                        var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.cveEmpleado == claveEmpleado.ToString());

                        if (usuarioSIGOPLAN != null)
                        {
                            var data = new EmpleadoDTO
                            {
                                claveEmpleado = claveEmpleado,
                                nombre = (string)empleado.nombre,
                                apellidoPaterno = (string)empleado.ape_paterno,
                                apellidoMaterno = (string)empleado.ape_materno
                            };

                            resultado.Add("data", data);
                            resultado.Add(SUCCESS, true);
                        }
                        else
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "El empleado no tiene usuario de SIGOPLAN.");
                        }
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encuentra el empleado.");
                    }
                    #endregion
                }
                else
                {
                    #region SE OBTIENE INFORMACIÓN DE EMPLEADOS EN CASO DE SER CONTRATISTA EL QUE INICIO SESIÓN
                    List<tblS_IncidentesEmpleadoContratistas> lstEmpleadosContratistas = _context.tblS_IncidentesEmpleadoContratistas
                        .Where(x => x.id == claveEmpleado && (idEmpresaContratista > 0 ? x.idEmpresaContratista == idEmpresaContratista : true) && x.esActivo).ToList();
                    if (lstEmpleadosContratistas.Count() > 0)
                    {
                        EmpleadoDTO objEmpleadoContratista = new EmpleadoDTO();
                        objEmpleadoContratista.claveEmpleado = lstEmpleadosContratistas[0].id;
                        objEmpleadoContratista.nombre = lstEmpleadosContratistas[0].nombre;
                        objEmpleadoContratista.apellidoPaterno = lstEmpleadosContratistas[0].apePaterno;
                        objEmpleadoContratista.apellidoMaterno = lstEmpleadosContratistas[0].apeMaterno;
                        resultado.Add("data", objEmpleadoContratista);
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encuentra el empleado.");
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        #endregion

        public Dictionary<string, object> getActividadesCapturaCombo()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                var empleadoEvaluacion = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.division == divisionActual);

                if (empleadoEvaluacion != null)
                {
                    var actividades = _context.tblSED_Actividad.Where(x => x.estatus && x.division == divisionActual).ToList();
                    var actividadesPorPuesto = _context.tblSED_RelPuestoActividad.Where(x => x.estatus && x.puestoID == empleadoEvaluacion.puestoEvaluacionID && x.division == divisionActual).ToList();

                    var combo = actividadesPorPuesto.Select(x => new ComboDTO
                    {
                        Value = x.actividadID.ToString(),
                        Text = actividades.FirstOrDefault(y => y.id == x.actividadID).descripcion,
                        Prefijo = actividades.FirstOrDefault(y => y.id == x.actividadID).ponderacion.ToString()
                    }).ToList();

                    resultado.Add("dataCombo", combo);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No está dado de alta como empleado para evaluación o para evaluar.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarCaptura(EvaluacionDTO captura)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //#region Validación fecha límite para la fecha de la actividad.
                    //var fechaActual = DateTime.Now.Date;
                    //var fechaActividad = captura.fechaActividad.Date;
                    //var fechaActividadMaxima = fechaActual.AddDays(5).Date;

                    //if (fechaActual > fechaActividadMaxima)
                    //{
                    //    throw new Exception("La fecha límite para la captura de la actividad es \"" + fechaActividadMaxima.ToShortDateString() + "\".");
                    //}
                    //#endregion

                    var usuario = vSesiones.sesionUsuarioDTO;
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    var empleadoEvaluacion = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.division == divisionActual);
                    var actividad = _context.tblSED_Actividad.FirstOrDefault(x => x.id == captura.actividadID && x.division == divisionActual);
                    var relacionPuestoActividad =
                        _context.tblSED_RelPuestoActividad.FirstOrDefault(x => x.estatus && x.puestoID == empleadoEvaluacion.puestoEvaluacionID && x.actividadID == actividad.id && x.division == divisionActual);

                    string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidencia, captura.evidencia.FileName);
                    string rutaArchivoEvidencia = Path.Combine(RutaEvidencia, nombreArchivoEvidencia);
                    listaRutaArchivos.Add(Tuple.Create(captura.evidencia, rutaArchivoEvidencia));

                    if (empleadoEvaluacion != null)
                    {
                        tblSED_Evaluacion nuevaCaptura = new tblSED_Evaluacion();

                        nuevaCaptura.empleadoID = empleadoEvaluacion.id;
                        nuevaCaptura.actividadID = captura.actividadID;
                        nuevaCaptura.rutaEvidencia = rutaArchivoEvidencia;
                        nuevaCaptura.comentariosEmpleado = captura.comentariosEmpleado ?? "";
                        nuevaCaptura.fechaActividad = captura.fechaActividad;
                        nuevaCaptura.fechaCaptura = DateTime.Now;
                        nuevaCaptura.ponderacionActual = actividad.ponderacion;
                        nuevaCaptura.periodicidadActual = relacionPuestoActividad.periodicidad;
                        nuevaCaptura.aplica = true;
                        nuevaCaptura.evaluadorID = 0;
                        nuevaCaptura.comentariosEvaluador = "";
                        nuevaCaptura.aprobado = false;
                        nuevaCaptura.fechaEvaluacion = null;
                        nuevaCaptura.division = divisionActual;
                        nuevaCaptura.estatus = true;

                        _context.tblSED_Evaluacion.Add(nuevaCaptura);
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("El empleado no está dado de alta para la captura.");
                    }

                    foreach (var archivo in listaRutaArchivos)
                    {
                        if (GlobalUtils.SaveHTTPPostedFile(archivo.Item1, archivo.Item2) == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(fileName));
        }

        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        public Dictionary<string, object> getCapturasEmpleado(DateTime fechaInicio, DateTime fechaFin, int evaluadorID, int estatus)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var empleadoEvaluacion = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.division == divisionActual);

                if (empleadoEvaluacion != null)
                {
                    var actividades = _context.tblSED_Actividad.Where(x => x.division == divisionActual).ToList();

                    var capturas = _context.tblSED_Evaluacion.Where(x => x.estatus && x.empleadoID == empleadoEvaluacion.id && x.division == divisionActual).ToList();
                    var empleados = _context.tblSED_Empleado.Where(x => x.division == divisionActual).ToList().Select(x => new
                    {
                        id = x.id,
                        nombreCompleto = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                    }).ToList();

                    #region Filtros
                    capturas = capturas.Where(x =>
                        x.fechaCaptura.Date >= fechaInicio.Date && x.fechaCaptura.Date <= fechaFin.Date && (evaluadorID > 0 ? x.evaluadorID == evaluadorID : true)
                    ).ToList();

                    switch (estatus)
                    {
                        case 0: //Todas
                            break;
                        case 1: //Evaluadas
                            capturas = capturas.Where(x => x.evaluadorID > 0).ToList();
                            break;
                        case 2: //No Evaluadas
                            capturas = capturas.Where(x => x.evaluadorID == 0).ToList();
                            break;
                    }
                    #endregion

                    var data = capturas.Select(x => new EvaluacionDTO
                    {
                        id = x.id,
                        empleadoID = x.empleadoID,
                        actividadID = x.actividadID,
                        rutaEvidencia = x.rutaEvidencia,
                        comentariosEmpleado = x.comentariosEmpleado,
                        fechaActividad = x.fechaActividad,
                        fechaCaptura = x.fechaCaptura,
                        ponderacionActual = x.ponderacionActual,
                        periodicidadActual = x.periodicidadActual,
                        aplica = x.aplica,
                        evaluadorID = x.evaluadorID,
                        comentariosEvaluador = x.comentariosEvaluador,
                        aprobado = x.aprobado,
                        fechaEvaluacion = x.fechaEvaluacion,
                        actividadDesc = actividades.FirstOrDefault(y => y.id == x.actividadID).descripcion,
                        fechaActividadDesc = x.fechaActividad.ToShortDateString(),
                        fechaCapturaDesc = x.fechaCaptura.ToShortDateString(),
                        periodicidadActualDesc = x.periodicidadActual.ToString(),
                        empleadoDesc = empleados.FirstOrDefault(y => y.id == x.empleadoID).nombreCompleto,
                        evaluadorDesc = x.evaluadorID > 0 ? empleados.FirstOrDefault(y => y.id == x.evaluadorID).nombreCompleto : "",
                        fechaEvaluacionDesc = x.fechaEvaluacion != null ? ((DateTime)x.fechaEvaluacion).ToShortDateString() : ""
                    }).ToList();

                    resultado.Add("data", data);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("El usuario no está dado de alta para evaluación.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> cargarDatosArchivoEvidencia(int capturaID)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblSED_Evaluacion.FirstOrDefault(x => x.id == capturaID && x.division == divisionActual);

                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaEvidencia).ToUpper());
                resultado.Add(SUCCESS, true);
            }

            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Tuple<Stream, string> descargarArchivoEvidencia(int capturaID)
        {
            try
            {
                var captura = _context.tblSED_Evaluacion.FirstOrDefault(x => x.id == capturaID && x.division == divisionActual);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                string name = Path.GetFileName(captura.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> getCapturasEvaluador(int empleadoID, int estatus)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var empleadoEvaluador = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.evaluador && x.division == divisionActual);

                if (empleadoEvaluador != null)
                {
                    var actividades = _context.tblSED_Actividad.Where(x => x.division == divisionActual).ToList();
                    var empleadosRelacionEvaluador = _context.tblSED_RelEmpleadoEvaluador.Where(x => x.estatus && x.evaluadorID == empleadoEvaluador.id && x.division == divisionActual).Select(x => x.empleadoID).ToList();

                    var capturas = _context.tblSED_Evaluacion.ToList().Where(x =>
                        x.estatus &&
                        (usuario.id != 3807 ? empleadosRelacionEvaluador.Contains(x.empleadoID) : true) &&
                        x.division == divisionActual
                    ).ToList();
                    var empleados = _context.tblSED_Empleado.Where(x => x.division == divisionActual).ToList().Select(x => new
                    {
                        id = x.id,
                        nombreCompleto = string.Format(@"{0} {1} {2}", x.nombre, x.apellidoPaterno, x.apellidoMaterno)
                    }).ToList();

                    #region Filtros
                    //El periodo de evaluación de un mes abarca desde el día 28 del mismo mes hasta el día 5 del siguiente mes.
                    var diaActual = DateTime.Now.Day;
                    var mesActual = DateTime.Now.Month;
                    var mesAnterior = DateTime.Now.Month > 1 ? DateTime.Now.Month - 1 : 12;
                    var anioActual = DateTime.Now.Year;

                    //if (!empleadoEvaluador.superUsuario) //if (usuario.cveEmpleado != "17803")
                    //{
                    //    if (diaActual < 28)
                    //    {
                    //        if (diaActual > 5)
                    //        {
                    //            throw new Exception("Se encuentra fuera del periodo para evaluar las capturas del mes.");
                    //        }

                    //        //Obtener capturas del mes anterior.
                    //        capturas = capturas.Where(x => x.fechaCaptura.Year == anioActual && x.fechaCaptura.Month == mesAnterior).ToList();
                    //    }
                    //    else if (diaActual >= 28)
                    //    {
                    //        //Obtener capturas del mes actual.
                    //        capturas = capturas.Where(x => x.fechaCaptura.Year == anioActual && x.fechaCaptura.Month == mesActual).ToList();
                    //    }
                    //}

                    capturas = capturas.Where(x => (empleadoID > 0 ? x.empleadoID == empleadoID : true)).ToList();

                    switch (estatus)
                    {
                        case 0: //Todas
                            break;
                        case 1: //Evaluadas
                            capturas = capturas.Where(x => x.evaluadorID > 0).ToList();
                            break;
                        case 2: //No Evaluadas
                            capturas = capturas.Where(x => x.evaluadorID == 0).ToList();
                            break;
                    }
                    #endregion

                    var data = capturas.Select(x => new EvaluacionDTO
                    {
                        id = x.id,
                        empleadoID = x.empleadoID,
                        actividadID = x.actividadID,
                        rutaEvidencia = x.rutaEvidencia,
                        comentariosEmpleado = x.comentariosEmpleado,
                        fechaActividad = x.fechaActividad,
                        fechaCaptura = x.fechaCaptura,
                        ponderacionActual = x.ponderacionActual,
                        periodicidadActual = x.periodicidadActual,
                        aplica = x.aplica,
                        evaluadorID = x.evaluadorID,
                        comentariosEvaluador = x.comentariosEvaluador,
                        aprobado = x.aprobado,
                        fechaEvaluacion = x.fechaEvaluacion,
                        actividadDesc = actividades.FirstOrDefault(y => y.id == x.actividadID).descripcion,
                        fechaCapturaDesc = x.fechaCaptura.ToShortDateString(),
                        periodicidadActualDesc = x.periodicidadActual.ToString(),
                        empleadoDesc = empleados.FirstOrDefault(y => y.id == x.empleadoID).nombreCompleto,
                        evaluadorDesc = x.evaluadorID > 0 ? empleados.FirstOrDefault(y => y.id == x.evaluadorID).nombreCompleto : "",
                        fechaEvaluacionDesc = x.fechaEvaluacion != null ? ((DateTime)x.fechaEvaluacion).ToShortDateString() : ""
                    }).ToList();

                    resultado.Add("data", data);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("El usuario no está dado de alta para evaluar.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEvaluacion(EvaluacionDTO evaluacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;
                    var empleadoEvaluador = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.evaluador && x.division == divisionActual);

                    if (empleadoEvaluador != null)
                    {
                        //#region Validación periodo de evaluación
                        ////El periodo de evaluación de un mes abarca desde el día 28 del mismo mes hasta el día 5 del siguiente mes.
                        //var diaActual = DateTime.Now.Day;

                        //if (!empleadoEvaluador.superUsuario && (diaActual > 5 && diaActual < 28)) //if (usuario.cveEmpleado != "17803" && (diaActual > 5 && diaActual < 28))
                        //{
                        //    throw new Exception("Se encuentra fuera del periodo para evaluar las capturas del mes.");
                        //}
                        //#endregion

                        var captura = _context.tblSED_Evaluacion.FirstOrDefault(x => x.estatus && x.id == evaluacion.id && x.division == divisionActual);

                        if (captura != null)
                        {
                            captura.evaluadorID = empleadoEvaluador.id;
                            captura.comentariosEvaluador = evaluacion.comentariosEvaluador ?? "";
                            captura.aprobado = evaluacion.aprobado;
                            captura.fechaEvaluacion = DateTime.Now;

                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información de la captura.");
                        }
                    }
                    else
                    {
                        throw new Exception("El empleado no está dado de alta para evaluar.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public List<ComboDTO> FillComboCc()
        {
            try
            {
                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"SELECT 
                                    c.cc AS Value, 
                                    (c.cc + '-' + c.descripcion) AS Text 
                                FROM cc c 
                                WHERE c.st_ppto != 'T' 
                                ORDER BY Value",
                    parametros = new List<OdbcParameterDTO>() { }
                };
                var resultado = _contextEnkontrol.Select<ComboDTO>(productivo ? EnkontrolAmbienteEnum.Prod : EnkontrolAmbienteEnum.Prueba, odbc);

                return resultado;
            }
            catch (Exception)
            {
                return new List<ComboDTO>();
            }
        }

        public Dictionary<string, object> cargarDashboard(DateTime mes, int idEmpresa, int idAgrupador, List<int> categorias, int evaluadorID)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == usuario.id);
                var empleadoEvaluador = new tblSED_Empleado();
                try
                {
                    empleadoEvaluador = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Convert.ToInt32(usuario.cveEmpleado) && x.evaluador && x.division == divisionActual);
                }
                catch(Exception e2){}
                var limiteMes = mes.AddMonths(1);

                if (usuarioSIGOPLAN.usuarioAuditor || empleadoEvaluador != null || usuarioSIGOPLAN.perfilID == 1)
                {
                    var anioFiltro = mes.Year;
                    var mesFiltro = mes.Month;

                    var actividadesActuales = _context.tblSED_Actividad.Where(x => x.estatus && x.division == divisionActual).ToList();
                    var puestos = _context.tblSED_Puesto.Where(x => x.estatus && x.division == divisionActual).ToList();
                    var evaluados = _context.tblSED_Empleado.ToList().Where(x => x.estatus && x.division == divisionActual && x.fechaInicioCC < limiteMes).ToList();
                    var relacionEmpleadoEvaluador = _context.tblSED_RelEmpleadoEvaluador.ToList().Where(x => x.estatus && evaluados.Select(y => y.id).Contains(x.empleadoID) && x.division == divisionActual).ToList();
                    var usuariosSIGOPLAN = _context.tblP_Usuario.ToList().Where(x => x.estatus && evaluados.Select(y => y.claveEmpleado.ToString()).Contains(x.cveEmpleado)).ToList();

                    #region Filtro Centro de Costo
                    if (idAgrupador != null)
                    {
                        evaluados = evaluados.Where(x => x.idAgrupacion == idAgrupador && x.idEmpresa == idEmpresa).ToList();
                    }
                    #endregion

                    #region Filtro Categorías
                    if (categorias != null)
                    {
                        evaluados = (
                            from eva in evaluados
                            join puesto in puestos on eva.puestoEvaluacionID equals puesto.id
                            where categorias.Contains((int)puesto.categoria)
                            select eva
                        ).ToList();
                    }
                    #endregion

                    #region Filtro Evaluador
                    if (evaluadorID > 0)
                    {
                        evaluados = (
                            from eva in evaluados
                            join rel in relacionEmpleadoEvaluador on eva.id equals rel.empleadoID
                            where rel.evaluadorID == evaluadorID
                            select eva
                        ).ToList();
                    }
                    #endregion

                    var capturas = _context.tblSED_Evaluacion.ToList().Where(x =>
                        x.estatus && x.fechaActividad.Year == anioFiltro && x.fechaActividad.Month == mesFiltro && x.division == divisionActual
                    ).ToList();

                    var data = new List<EmpleadoDTO>();

                    foreach (var eva in evaluados)
                    {
                        var capturasEvaluado = capturas.Where(x => x.empleadoID == eva.id).ToList();

                        var evaluado = new EmpleadoDTO();

                        evaluado.id = eva.id;
                        evaluado.claveEmpleado = eva.claveEmpleado;
                        evaluado.nombre = eva.nombre;
                        evaluado.apellidoPaterno = eva.apellidoPaterno;
                        evaluado.apellidoMaterno = eva.apellidoMaterno;
                        evaluado.puestoEvaluacionID = eva.puestoEvaluacionID;
                        evaluado.evaluador = eva.evaluador;
                        evaluado.rol = eva.rol;
                        evaluado.fechaInicioRol = eva.fechaInicioRol;
                        evaluado.estatus = eva.estatus;
                        evaluado.empresa = usuariosSIGOPLAN.Where(x => x.cveEmpleado == eva.claveEmpleado.ToString()).Select(x => x.empresa ?? "").FirstOrDefault();
                        evaluado.categoria = puestos.FirstOrDefault(x => x.id == eva.puestoEvaluacionID).categoria;
                        evaluado.actividades = new List<ActividadDTO>();

                        IEnumerable<DateTime> diasTrabajados = new List<DateTime>();

                        switch ((int)evaluado.rol)
                        {
                            case 1: //Grupo 1 14x7
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 14, 7);
                                break;
                            case 2: //Grupo 2 6x1
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 6, 1);
                                break;
                            case 3: //Grupo 3 5x2
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 5, 2);
                                break;
                            case 4: //Grupo 4 11x3
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 11, 3);
                                break;
                            case 5: //Grupo 5 10x4
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 10, 4);
                                break;
                            case 6: //Grupo 6 20x10
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 20, 10);
                                break;
                            case 7: //Grupo 7 12x4
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 12, 4);
                                break;
                            case 8: //Grupo 8 10x5
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 10, 5);
                                break;
                            case 9: //Grupo 9 8x4
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 8, 4);
                                break;
                            case 10: //Grupo 10 21x11
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 21, 11);
                                break;
                            case 11: //Grupo 11 22x11
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 22, 11);
                                break;
                            case 12: //Grupo 12 21x7
                                diasTrabajados = getDiasTrabajados(evaluado.fechaInicioRol, DateTime.Now, 21, 7);
                                break;
                        }

                        diasTrabajados = diasTrabajados.Where(x => x.Year == anioFiltro && x.Month == mesFiltro).ToList();
                        var cantidadDiasTrabajados = diasTrabajados.Count();

                        var actividadesPorPuesto = _context.tblSED_RelPuestoActividad.Where(x => x.estatus && x.puestoID == eva.puestoEvaluacionID && x.division == divisionActual).Select(x => x.actividadID).ToList();
                        var actividadesFiltradas = actividadesActuales.Where(x => actividadesPorPuesto.Contains(x.id)).ToList();

                        foreach (var act in actividadesFiltradas) //foreach (var act in actividadesActuales)
                        {
                            var capturasEvaluadoActividad = capturasEvaluado.Where(x => x.actividadID == act.id).ToList();
                            var relPuestoActividad = _context.tblSED_RelPuestoActividad.FirstOrDefault(x => x.estatus && x.puestoID == evaluado.puestoEvaluacionID && x.actividadID == act.id && x.division == divisionActual);

                            if (relPuestoActividad == null)
                            {
                                throw new Exception("No se encuentra la información relación puesto actividad.");
                            }

                            var actividad = new ActividadDTO();

                            actividad.id = act.id;
                            actividad.descripcion = act.descripcion;
                            actividad.ponderacion = act.ponderacion;
                            actividad.estatus = act.estatus;
                            actividad.periodicidad = relPuestoActividad.periodicidad;
                            actividad.cantidadDiasTrabajados = cantidadDiasTrabajados;

                            switch ((int)relPuestoActividad.periodicidad)
                            {
                                case 1: //Diaria
                                    actividad.cantidadProgramada = cantidadDiasTrabajados;
                                    break;
                                case 2: //Semanal
                                    actividad.cantidadProgramada = 4;
                                    break;
                                case 3: //Quincenal
                                    actividad.cantidadProgramada = 2;
                                    break;
                                case 4: //Mensual
                                    actividad.cantidadProgramada = 1;
                                    break;
                            }

                            var cantidadRealizada = capturasEvaluadoActividad.Count();
                            var cantidadAprobada = capturasEvaluadoActividad.Where(x => x.aprobado).ToList().Count();

                            actividad.cantidadRealizada = cantidadRealizada;
                            actividad.cantidadAprobada = cantidadAprobada;
                            actividad.porcentajeCumplido = actividad.cantidadProgramada > 0 ? ((actividad.cantidadAprobada * 100) / actividad.cantidadProgramada) : 0;

                            if (actividad.porcentajeCumplido > 100)
                            {
                                actividad.porcentajeCumplido = 100;
                            }

                            evaluado.actividades.Add(actividad);
                        }

                        var actividadesAplicables = actividadesFiltradas.Count();
                        var sumatoriaPorcentajesAplicables = evaluado.actividades.Sum(x => x.porcentajeCumplido);
                        var porcentajeCumplimientoMensual = Math.Truncate(100 * (sumatoriaPorcentajesAplicables / (actividadesAplicables > 0 ? actividadesAplicables : 1))) / 100;

                        //evaluado.porcentajeCumplimientoMensual = evaluado.actividades.Sum(x => x.porcentajeCumplido * x.ponderacion) / 100;
                        evaluado.porcentajeCumplimientoMensual = porcentajeCumplimientoMensual;

                        data.Add(evaluado);
                    }

                    var chartPromedioGeneral = ObtenerChartPromedioGeneral(data);
                    var chartPromedioPorAreas = ObtenerChartPromedioPorAreas(data, categorias);
                    var chartPromedioPorActividades = ObtenerChartPromedioPorActividades(data);

                    resultado.Add("data", data);
                    resultado.Add("dataActividadesActuales", actividadesActuales);
                    resultado.Add("chartPromedioGeneral", chartPromedioGeneral);
                    resultado.Add("chartPromedioPorAreas", chartPromedioPorAreas);
                    resultado.Add("chartPromedioPorActividades", chartPromedioPorActividades);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("El usuario no está dado de alta como evaluador.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add("data", null);
                resultado.Add("dataActividadesActuales", new List<tblSED_Actividad>());
                resultado.Add(MESSAGE, e.Message);
            }

            resultado.Add("centroCostoDesc", _context.tblS_IncidentesAgrupacionCC.Where(x => x.id == idAgrupador).Select(x => x.nomAgrupacion).FirstOrDefault());

            return resultado;
        }

        public List<DateTime> getDiasTrabajados(DateTime fechaInicio, DateTime fechaFin, int rangoTrabajo, int rangoDescanso)
        {
            List<DateTime> dias = new List<DateTime>();
            List<Tuple<int, bool>> dicDiasTrabajoDescanso = new List<Tuple<int, bool>>();

            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(0, rangoTrabajo).Select(index => new Tuple<int, bool>(index, true)).ToList());
            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(rangoTrabajo, rangoDescanso).Select(index => new Tuple<int, bool>(index, false)).ToList());

            int contador = 0;

            for (var dia = fechaInicio.Date; dia.Date <= fechaFin.Date; dia = dia.AddDays(1))
            {
                if (contador == dicDiasTrabajoDescanso.Count())
                {
                    contador = 0;
                }

                var diaDiccionario = dicDiasTrabajoDescanso.FirstOrDefault(x => x.Item1 == contador);

                if (diaDiccionario.Item2)
                {
                    dias.Add(dia);
                }

                contador++;
            }

            return dias;
        }

        private ChartDataDTO ObtenerChartPromedioGeneral(List<EmpleadoDTO> data)
        {
            var chartPromedioGeneral = new ChartDataDTO { labels = new List<string> { "Cumplido", "No Cumplido" }, datasets = new List<DatasetDTO>() };
            var dataSet = new DatasetDTO
            {
                backgroundColor = new List<string>(),
                borderColor = new List<string>(),
                borderWidth = 1,
                fill = true,
                data = new List<decimal>()
            };

            var porcentajeCumplidoPromedioGeneral = data.Count > 0 ? data.Average(x => x.porcentajeCumplimientoMensual) : 0;
            var porcentajeNoCumplido = 100 - porcentajeCumplidoPromedioGeneral;

            #region Cumplido
            dataSet.backgroundColor.Add("rgb(0,176,80)");
            dataSet.borderColor.Add("rgb(0,176,80)");
            dataSet.data.Add(porcentajeCumplidoPromedioGeneral);
            #endregion

            #region No Cumplido
            dataSet.backgroundColor.Add("rgb(255,0,0)");
            dataSet.borderColor.Add("rgb(255,0,0)");
            dataSet.data.Add(porcentajeNoCumplido);
            #endregion

            chartPromedioGeneral.datasets.Add(dataSet);

            return chartPromedioGeneral;
        }

        private ChartDataDTO ObtenerChartPromedioPorAreas(List<EmpleadoDTO> data, List<int> listaFiltroCategorias)
        {
            var dataPorCategorias = data.GroupBy(x => x.categoria).ToList();
            var areas = GlobalUtils.ParseEnumToCombo<CategoriaPuestoEnum>();
            List<string> labelsAreas = areas.Where(x => listaFiltroCategorias != null ? listaFiltroCategorias.Contains((int)x.Value) : true).Select(x => x.Text).ToList();
            List<decimal> datosAreas = new List<decimal>();

            foreach (var labelArea in labelsAreas)
            {
                int area = (int)areas.FirstOrDefault(x => x.Text == labelArea).Value;
                var porcentaje = dataPorCategorias.Count() > 0 ? dataPorCategorias.Select(x =>
                    x.Count() > 0 ?
                        x.Where(y => (int)y.categoria == area).Count() > 0 ?
                            x.Where(y => (int)y.categoria == area).Average(z => z.porcentajeCumplimientoMensual)
                        : 0
                    : 0
                ).Average() : 0;

                datosAreas.Add(porcentaje);
            }

            return new ChartDataDTO
            {
                labels = labelsAreas,
                datasets = new List<DatasetDTO>{ new DatasetDTO
                    {
                        backgroundColor = dataPorCategorias.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderColor = dataPorCategorias.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderWidth = 2,
                        fill = true,
                        data = datosAreas
                    }
                }
            };
        }

        private ChartDataDTO ObtenerChartPromedioPorActividades(List<EmpleadoDTO> data)
        {
            var dataPorActividades = data.GroupBy(x => x.actividades.Select(y => y.id)).ToList();
            List<string> labelsActividades = dataPorActividades.Select(x => x.Select(y => y.actividades.Select(z => z.descripcion)).SelectMany(w => w)).SelectMany(x => x).Distinct().ToList();
            List<decimal> datosActividades = new List<decimal>();

            foreach (var labelAct in labelsActividades)
            {
                var porcentaje = dataPorActividades.Count() > 0 ? dataPorActividades.Select(x =>
                    x.Count() > 0 ?
                        x.Average(y =>
                            y.actividades.Where(q => q.descripcion == labelAct).Count() > 0 ?
                                y.actividades.Where(q => q.descripcion == labelAct).Average(z => z.porcentajeCumplido)
                            : 0
                        )
                    : 0
                ).Average() : 0;

                datosActividades.Add(porcentaje);
            }

            return new ChartDataDTO
            {
                labels = labelsActividades,
                datasets = new List<DatasetDTO>{ new DatasetDTO
                    {
                        backgroundColor = dataPorActividades.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderColor = dataPorActividades.Select(x => ObtenerColorGraficaAleatorio()).ToList(),
                        borderWidth = 2,
                        fill = true,
                        data = datosActividades
                    }
                }
            };
        }

        private string ObtenerColorGraficaAleatorio()
        {
            int r = RandomInteger(0, 255);
            int g = RandomInteger(0, 255);
            int b = RandomInteger(0, 255);
            float a = 0.6f; // Valor constante para colores definidos.

            return String.Format("rgba({0},{1},{2},{3})", r, g, b, a);
        }

        // Return a random integer between a min and max value.
        private int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                new RNGCryptoServiceProvider().GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }

        public Dictionary<string, object> getAgendaActividades(DateTime mes)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var empleadoEvaluacion = _context.tblSED_Empleado.ToList().FirstOrDefault(x => x.estatus && x.claveEmpleado == Int32.Parse(usuario.cveEmpleado) && x.division == divisionActual);

                if (empleadoEvaluacion != null)
                {
                    #region Validación fecha menor a la fecha de inicio del empleado
                    var mesFechaInicio = empleadoEvaluacion.fechaInicioRol.Month;
                    var anioFechaInicio = empleadoEvaluacion.fechaInicioRol.Year;
                    var fechaValidacionInicio = new DateTime(anioFechaInicio, mesFechaInicio, 1);

                    if (mes < fechaValidacionInicio)
                    {
                        throw new Exception("Error al consultar la información. La fecha de inicio del empleado es: " + empleadoEvaluacion.fechaInicioRol.ToShortDateString());
                    }
                    #endregion

                    #region Cálculo de días agenda por mes
                    List<Tuple<DateTime, bool>> diasAgenda = new List<Tuple<DateTime, bool>>();
                    DateTime ultimoDiaMesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                    switch ((int)empleadoEvaluacion.rol)
                    {
                        case 1: //Grupo 1 14x7
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 14, 7);
                            break;
                        case 2: //Grupo 2 6x1
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 6, 1);
                            break;
                        case 3: //Grupo 3 5x2
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 5, 2);
                            break;
                        case 4: //Grupo 4 11x3
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 11, 3);
                            break;
                        case 5: //Grupo 5 10x4
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 10, 4);
                            break;
                        case 6: //Grupo 6 20x10
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 20, 10);
                            break;
                        case 7: //Grupo 7 12x4
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 12, 4);
                            break;
                        case 8: //Grupo 8 10x5
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 10, 5);
                            break;
                        case 9: //Grupo 9 8x4
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 8, 4);
                            break;
                        case 10: //Grupo 10 21x11
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 21, 11);
                            break;
                        case 11: //Grupo 11 22x11
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 22, 11);
                            break;
                        case 12: //Grupo 12 21x7
                            diasAgenda = getDiasAgenda(empleadoEvaluacion.fechaInicioRol, ultimoDiaMesActual, 21, 7);
                            break;
                    }

                    var anioFiltro = mes.Year;
                    var mesFiltro = mes.Month;

                    diasAgenda = diasAgenda.Where(x => x.Item1.Year == anioFiltro && x.Item1.Month == mesFiltro).ToList();
                    #endregion

                    #region Diagrama Gantt por Actividades
                    List<GanttDTO> listaGantt = new List<GanttDTO>();

                    var diasTrabajados = diasAgenda.Where(x => x.Item2).ToList();
                    var cantidadDiasTrabajados = diasTrabajados.Count();
                    var actividadesActuales = _context.tblSED_Actividad.Where(x => x.estatus && x.division == divisionActual).ToList();

                    foreach (var act in actividadesActuales)
                    {
                        var relPuestoActividad =
                            _context.tblSED_RelPuestoActividad.FirstOrDefault(x => x.estatus && x.puestoID == empleadoEvaluacion.puestoEvaluacionID && x.actividadID == act.id && x.division == divisionActual);

                        if (relPuestoActividad != null)
                        {
                            var cantidadProgramada = 0;

                            switch ((int)relPuestoActividad.periodicidad)
                            {
                                case 1: //Diaria
                                    cantidadProgramada = cantidadDiasTrabajados;
                                    break;
                                case 2: //Semanal
                                    cantidadProgramada = 4;
                                    break;
                                case 3: //Quincenal
                                    cantidadProgramada = 2;
                                    break;
                                case 4: //Mensual
                                    cantidadProgramada = 1;
                                    break;
                            }

                            var fechaMaximaMes = new DateTime(anioFiltro, mesFiltro, DateTime.DaysInMonth(anioFiltro, mesFiltro));
                            var duracion = (fechaMaximaMes - diasTrabajados.Min(x => x.Item1)).Days;

                            listaGantt.Add(new GanttDTO
                            {
                                id = act.id,
                                text = act.descripcion + " (" + relPuestoActividad.periodicidad.ToString() + ")",
                                start_date = diasTrabajados.Min(x => x.Item1).ToShortDateString(),
                                duration = duracion,
                                parent = null,
                                progress = 0,
                                open = true,
                                users = new List<string>(),
                                priority = 1,
                                color = "#0890ff",
                                textColor = "black",
                                progressColor = "#12ff3d"
                            });
                        }
                    }
                    #endregion

                    #region Diagrama Gantt por Periodos
                    List<GanttDTO> listaGanttPeriodos = new List<GanttDTO>();

                    var gruposPeriodos = new List<List<Tuple<DateTime, bool>>>();
                    var grupo1 = new List<Tuple<DateTime, bool>>() { diasAgenda[0] };

                    gruposPeriodos.Add(grupo1);

                    Tuple<DateTime, bool> lastTuple = diasAgenda[0];
                    for (int i = 1; i < diasAgenda.Count; i++)
                    {
                        Tuple<DateTime, bool> currTuple = diasAgenda[i];
                        //TimeSpan timeDiff = currTuple.Item1 - lastTuple.Item1;

                        //Should we create a new group?
                        bool isNewGroup = lastTuple.Item2 != currTuple.Item2; //bool isNewGroup = timeDiff.Days > 1;
                        if (isNewGroup)
                        {
                            gruposPeriodos.Add(new List<Tuple<DateTime, bool>>());
                        }

                        gruposPeriodos.Last().Add(diasAgenda[i]);
                        lastTuple = diasAgenda[i];
                    }

                    int contador = 0;
                    int contadorPeriodo = 0;

                    foreach (var grupo in gruposPeriodos)
                    {
                        bool periodoTrabajo = grupo[0].Item2;
                        var duracion = (grupo.Max(x => x.Item1) - grupo.Min(x => x.Item1)).Days + 1; //Se le suma un día porque se brinca uno al hacer la resta.
                        var periodoID = ++contador;

                        listaGanttPeriodos.Add(new GanttDTO
                        {
                            id = periodoID,
                            text = "Periodo " + ++contadorPeriodo + (periodoTrabajo ? " Laboral" : " Descanso"),
                            start_date = grupo.Min(x => x.Item1).ToShortDateString(),
                            duration = duracion,
                            parent = null,
                            progress = 0,
                            open = true,
                            users = new List<string>(),
                            priority = 1,
                            color = periodoTrabajo ? "#0890ff" : "#12ff3d",
                            textColor = "black",
                            progressColor = periodoTrabajo ? "" : ""
                        });

                        if (periodoTrabajo)
                        {
                            foreach (var act in listaGantt)
                            {
                                listaGanttPeriodos.Add(new GanttDTO
                                {
                                    id = ++contador,
                                    text = act.text,
                                    start_date = grupo.Min(x => x.Item1).ToShortDateString(),
                                    duration = duracion,
                                    parent = periodoID,
                                    progress = 0,
                                    open = true,
                                    users = new List<string>(),
                                    priority = 1,
                                    color = "#b4ddff",
                                    textColor = "black",
                                    progressColor = ""
                                });
                            }
                        }
                    }
                    #endregion

                    resultado.Add("data", listaGantt);
                    resultado.Add("dataPeriodos", listaGanttPeriodos);
                    resultado.Add(SUCCESS, true);
                }
                else
                {
                    throw new Exception("El usuario no está dado de alta en el módulo de Evaluación de Desempeño.");
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public List<Tuple<DateTime, bool>> getDiasAgenda(DateTime fechaInicio, DateTime fechaFin, int rangoTrabajo, int rangoDescanso)
        {
            List<Tuple<DateTime, bool>> dias = new List<Tuple<DateTime, bool>>();
            List<Tuple<int, bool>> dicDiasTrabajoDescanso = new List<Tuple<int, bool>>();

            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(0, rangoTrabajo).Select(index => new Tuple<int, bool>(index, true)).ToList());
            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(rangoTrabajo, rangoDescanso).Select(index => new Tuple<int, bool>(index, false)).ToList());

            int contador = 0;

            for (var dia = fechaInicio.Date; dia.Date <= fechaFin.Date; dia = dia.AddDays(1))
            {
                if (contador == dicDiasTrabajoDescanso.Count())
                {
                    contador = 0;
                }

                var diaDiccionario = dicDiasTrabajoDescanso.FirstOrDefault(x => x.Item1 == contador);

                dias.Add(new Tuple<DateTime, bool>(dia, diaDiccionario.Item2));

                contador++;
            }

            return dias;
        }
    }
}
