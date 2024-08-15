using Core.DAO.Administracion.Seguridad;
using Core.DAO.Enkontrol.General.CC;
using Core.DTO;
using Core.DTO.Administracion.Seguridad.CapacitacionSeguridad;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Principal.Generales;
using Core.DTO.RecursosHumanos.Capacitacion;
using Core.DTO.Utils;
using Core.DTO.Utils.ChartJS;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Seguridad.ActoCondicion;
using Core.Entity.Administrativo.Seguridad.CapacitacionSeguridad;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Menus;
using Core.Entity.Principal.Multiempresa;
using Core.Entity.Principal.Usuarios;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Administracion.Seguridad.CapacitacionSeguridad;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Enkontrol.General.CC;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using MoreLinq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;

namespace Data.DAO.Administracion.Seguridad.CapacitacionSeguridad
{
    public class CapacitacionSeguridadDAO : GenericDAO<tblS_CapacitacionSeguridadCursos>, ICapacitacionSeguridadDAO
    {
        public readonly string Error = "error";
        public static string _msgError = "";
        private readonly bool productivo = true;
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\CAPACITACION_DEPARTAMENTO_SEGURIDAD";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\CAPACITACION_DEPARTAMENTO_SEGURIDAD";

        private const string NombreBaseControlAsistencia = @"ControlAsistencia";
        private const string NombreBaseFormatoAutorizacion = @"FormatoAutorizacion";
        private const string NombreBaseEvidencia = @"EvidenciaExtracurricular";
        private const string NombreBaseEvidenciaDeteccionNecesidades = @"EvidenciaSeguimiento";
        private const string NombreBaseArchivoLiberado = @"ArchivoLiberado";

        private readonly string RutaCursos;
        private readonly string RutaControlAsistencias;
        private readonly string RutaExpedientes;
        private readonly string RutaTemp;
        private readonly string RutaDeteccionNecesidadesAcciones;
        private readonly string RutaDeteccionNecesidadesPropuestas;
        private readonly string RutaDeteccionNecesidadesRecorridos;
        private readonly string RutaCompetenciasOperativasGestion;
        private readonly string RutaColaboradorLiberado;

        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string NombreControlador = "CapacitacionSeguridadController";
        private const int MenuCapacitacionPadreID = 7361;
        private const int VistaAutorizacionID = 7364;
        private const int SistemaMenuID = (int)SistemasEnum.RH; // Capital Humano
        private readonly int divisionActual = 0;

        #region Constructor
        public CapacitacionSeguridadDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaCursos = Path.Combine(RutaBase, "CURSOS");
            RutaControlAsistencias = Path.Combine(RutaBase, "CONTROL_ASISTENCIAS");
            RutaExpedientes = Path.Combine(RutaBase, "EXPEDIENTES");
            RutaTemp = Path.Combine(RutaBase, "TEMP");
            RutaDeteccionNecesidadesAcciones = Path.Combine(RutaBase, @"DETECCION_NECESIDADES\ACCIONES");
            RutaDeteccionNecesidadesPropuestas = Path.Combine(RutaBase, @"DETECCION_NECESIDADES\PROPUESTAS");
            RutaDeteccionNecesidadesRecorridos = Path.Combine(RutaBase, @"DETECCION_NECESIDADES\RECORRIDOS");
            RutaCompetenciasOperativasGestion = Path.Combine(RutaBase, @"COMPETENCIAS_OPERATIVAS");
            RutaColaboradorLiberado = Path.Combine(RutaBase, @"COLABORADOR_LIBERADO");

            divisionActual = vSesiones.sesionDivisionActual;
        }
        #endregion

        #region Cursos
        public Dictionary<string, object> getCursos(List<int> clasificaciones, List<int> puestos, int estatus)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var cursos = _context.tblS_CapacitacionSeguridadCursos
                    .Where(x => x.isActivo && x.division == divisionActual)
                    .ToList()
                    .Where(x =>
                        (estatus == 0 ? true : (int)x.estatus == estatus) &&
                        (clasificaciones != null ? clasificaciones.Contains((int)x.clasificacion) : true) &&
                        (
                            (puestos != null ? x.Puestos.Select(y => y.puesto_id).Intersect(puestos).Count() > 0 : true) ||
                            (puestos != null ? x.PuestosAutorizacion.Select(y => y.puesto_id).Intersect(puestos).Count() > 0 : true)
                        )
                    )
                    .Select(x => new capacitacionCursosDTO
                    {
                        id = x.id,
                        claveCurso = x.claveCurso,
                        nombre = x.nombre,
                        duracion = x.duracion,
                        clasificacionDesc = ((ClasificacionCursoEnum)x.clasificacion).GetDescription(),
                        clasificacion = (ClasificacionCursoEnum)x.clasificacion,
                        estatus = ((EstatusCursoEnum)x.estatus).GetDescription()
                    }).ToList();

                if (cursos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", cursos);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo de cursos");
            }

            return resultado;
        }
        public Dictionary<string, object> getCursoById(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var curso = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.id == id && x.division == divisionActual).ToList().Select(x => new capacitacionCursosDTO
                {
                    id = x.id,
                    claveCurso = x.claveCurso,
                    clasificacion = (ClasificacionCursoEnum)x.clasificacion,
                    nombre = x.nombre,
                    duracion = x.duracion,
                    esGeneral = x.esGeneral == true ? 1 : 0,
                    aplicaTodosPuestos = x.aplicaTodosPuestos,
                    capacitacionUnica = x.capacitacionUnica,
                    objetivo = x.objetivo,
                    temasPrincipales = x.temasPrincipales,
                    referenciasNormativas = x.referenciasNormativas,
                    nota = x.nota,
                    nombreExamen = x.Examenes.ToList().Select(y => y.nombreExamen).ToList(),
                    mandos = x.Mandos.Where(y => y.estatus).ToList().Select(y => new mandosCursoDTO
                    {
                        id = y.id,
                        mando = (int)y.mando
                    }).ToList(),
                    puestos = x.Puestos.Where(y => y.estatus == true).ToList().Select(y => new puestosCursoDTO
                    {
                        id = y.id,
                        puesto_id = y.puesto_id
                    }).ToList(),
                    puestosAutorizacion = x.PuestosAutorizacion.Where(y => y.estatus == true).ToList().Select(y => new puestosCursoDTO
                    {
                        id = y.id,
                        puesto_id = y.puesto_id
                    }).ToList(),
                    centrosCosto = x.CentrosCosto.Where(y => y.estatus).ToList().Select(y => new centrosCostoCursoDTO
                    {
                        id = y.id,
                        cc = y.cc
                    }).ToList()
                }).FirstOrDefault();

                if (curso != null)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informacion", curso);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el curso");
            }

            return resultado;
        }
        public Dictionary<string, object> getExamenesCursoById(int id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var examenes = ObtenerListaExamenesPorCursoID(id).Select(x => new examenesCursoDTO
                {
                    curso_id = x.curso_id,
                    id = x.id,
                    nombreExamen = x.nombreExamen,
                    tipoExamen = x.tipoExamen,
                    pathExamen = x.pathExamen
                }).ToList();


                if (examenes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("informacion", examenes);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener los examenes");
            }

            return resultado;
        }
        public byte[] descargarArchivo(long examen_id)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.id == examen_id && x.division == divisionActual).FirstOrDefault().pathExamen;
                string rutaFisica = Path.Combine(RutaCursos, pathExamen);
                fileStream = GlobalUtils.GetFileAsStream(rutaFisica);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileName(long examen_id)
        {
            string fileName = "";
            try
            {
                string pathExamen = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.id == examen_id && x.division == divisionActual).FirstOrDefault().pathExamen;
                fileName = pathExamen.Split('\\')[1];
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }
        public Dictionary<string, object> getTipoExamen(int curso_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var examenes = ObtenerListaExamenesPorCursoID(curso_id);
                int tipoExamen = examenes.Count > 0 ? examenes.Max(x => x.tipoExamen) + 1 : 1;


                resultado.Add("tipoExamen", tipoExamen);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el tipod examen ");
            }

            return resultado;
        }

        private List<tblS_CapacitacionSeguridadCursosExamen> ObtenerListaExamenesPorCursoID(int cursoID)
        {
            return _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.curso_id == cursoID && x.isActivo && x.division == divisionActual).ToList();
        }

        public Dictionary<string, object> getClasificacionCursos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var clasificacionComboDTO = GlobalUtils.ParseEnumToCombo<ClasificacionCursoEnum>();

                if (vSesiones.sesionCapacitacionOperativa)
                {
                    List<int> clasificacionesCapacitacionOperativa = new List<int> {
                        (int)ClasificacionCursoEnum.ProtocoloFatalidad,
                        (int)ClasificacionCursoEnum.Normativo,
                        (int)ClasificacionCursoEnum.InstructivoOperativo,
                        (int)ClasificacionCursoEnum.TecnicoOperativo
                    };
                    clasificacionComboDTO = clasificacionComboDTO.Where(x => clasificacionesCapacitacionOperativa.Contains(Convert.ToInt32(x.Value))).ToList();
                }
                else
                {
                    List<int> clasificacionesCapitalHumano = new List<int> {
                        (int)ClasificacionCursoEnum.General,
                        (int)ClasificacionCursoEnum.Formativo,
                        (int)ClasificacionCursoEnum.HabilidadesTécnicas,
                        (int)ClasificacionCursoEnum.HabilidadesBlandas,
                        (int)ClasificacionCursoEnum.Inductivo,
                        (int)ClasificacionCursoEnum.Difusion
                    };
                    clasificacionComboDTO = clasificacionComboDTO.Where(x => clasificacionesCapitalHumano.Contains(Convert.ToInt32(x.Value))).ToList();
                }

                if (clasificacionComboDTO.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", clasificacionComboDTO);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo de clasificaciones");
            }

            return resultado;
        }
        public Dictionary<string, object> GetEstatusCursos()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var comboDTO = GlobalUtils.ParseEnumToCombo<EstatusCursoEnum>();

                if (comboDTO.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", comboDTO);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo de clasificaciones");
            }

            return resultado;
        }
        public Dictionary<string, object> getPuestosEnkontrol()
        {

            var resultado = new Dictionary<string, object>();

            try
            {
//                string query = @"
//                    SELECT puesto as Value, descripcion as Text 
//                    FROM DBA.si_puestos 
//                    WHERE (descripcion NOT LIKE '%NO USAR%') AND
//                        (descripcion NOT LIKE '%NOUSAR%') AND
//                        (descripcion NOT LIKE '%NO US%') AND
//                        (descripcion NOT LIKE '%NO USA%') AND 
//                        (descripcion NOT LIKE '%NOUSAR%')
//                    ORDER BY Text
//                    ";

//                var puestos = (List<Core.DTO.Principal.Generales.ComboDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                var puestos = _context.Select<ComboDTO>(new DapperDTO 
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = @"SELECT puesto as Value, descripcion as Text FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%NO US%' AND descripcion NOT LIKE '%(NO%' ORDER BY TEXT",
                });

                if(vSesiones.sesionEmpresaActual==6)
                {
                    puestos = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = @"SELECT puesto as Value, descripcion as Text FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%NO US%' AND descripcion NOT LIKE '%(NO%' ORDER BY TEXT",
                    });
                }

                if (puestos.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", puestos);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo tipos de empleados");
            }

            return resultado;
        }
        public Dictionary<string, object> guardarCurso(tblS_CapacitacionSeguridadCursos curso)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                curso.claveCurso = curso.claveCurso.ToUpper();
                curso.division = divisionActual;

                foreach (var man in curso.Mandos)
                {
                    man.division = divisionActual;
                }

                foreach (var pues in curso.Puestos)
                {
                    pues.division = divisionActual;
                }

                if (curso.PuestosAutorizacion != null)
                {
                    foreach (var pues in curso.PuestosAutorizacion)
                    {
                        pues.division = divisionActual;
                    }
                }

                foreach (var cc in curso.CentrosCosto)
                {
                    cc.division = divisionActual;
                }

                if (curso.claveCurso.Length <= 1 || curso.claveCurso.Length > 11)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(Error, "La longitud de la clave del curso es inválida.");
                    return resultado;
                }
                else if (EsCursoRepetido(curso))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(Error, "Ya existe un curso con esa clave o con ese nombre.");
                    return resultado;
                }
                else if (EsNombreCarpetaInvalido(curso.claveCurso))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(Error, "La clave del curso contiene caracteres inválidos.");
                    return resultado;
                }
                else if (EsNombreCarpetaInvalido(curso.nombre))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(Error, "El nombre del curso contiene caracteres inválidos.");
                    return resultado;
                }

                curso.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                curso.fechaCreacion = DateTime.Now;
                curso.estatus = (
                    (ClasificacionCursoEnum)curso.clasificacion == ClasificacionCursoEnum.General ||
                    (ClasificacionCursoEnum)curso.clasificacion == ClasificacionCursoEnum.Formativo ||
                    (ClasificacionCursoEnum)curso.clasificacion == ClasificacionCursoEnum.Difusion
                ) ?
                    (int)EstatusCursoEnum.Completo : (int)EstatusCursoEnum.PendienteExamenes;
                _context.tblS_CapacitacionSeguridadCursos.Add(curso);

                _context.SaveChanges();

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error al guardar el curso");
            }

            return resultado;
        }

        private bool EsCursoRepetido(tblS_CapacitacionSeguridadCursos curso)
        {
            return _context.tblS_CapacitacionSeguridadCursos.Any(x => (x.claveCurso.ToUpper() == curso.claveCurso || x.nombre.ToUpper() == curso.nombre.ToUpper()) && x.division == divisionActual);
        }

        private bool EsCursoRepetidoUpdate(tblS_CapacitacionSeguridadCursos curso)
        {
            return _context.tblS_CapacitacionSeguridadCursos.Any(x => x.isActivo && x.id != curso.id && (x.claveCurso.ToUpper() == curso.claveCurso || x.nombre.ToUpper() == curso.nombre.ToUpper()) && x.division == divisionActual);
        }

        public Dictionary<string, object> guardarExamenes(List<tblS_CapacitacionSeguridadCursosExamen> examenes, List<HttpPostedFileBase> archivos)
        {
            string rutaCarpetaCurso = "";
            string nombreCurso = "";
            List<object> examenPath = new List<object>();

            var listaExamenes = new List<Tuple<HttpPostedFileBase, string>>();

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //CREAR CARPETA CURSO
                    int curso_id = examenes.ToList().FirstOrDefault().curso_id;
                    var curso = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.id == curso_id && x.division == divisionActual);
                    rutaCarpetaCurso = Path.Combine(RutaCursos, curso.nombre);

                    //SI YA EXISTE CARPETA SOLO METO LOS ARCHIVOS A LA CARPETA
                    verificarExisteCarpeta(rutaCarpetaCurso, true);

                    // AGREGAR TIPO EXAMEN
                    int index = 0;
                    foreach (var examen in examenes)
                    {
                        string nuevoNombreExamen = ObtenerFormatoNombreExamenCurso(archivos[index].FileName);
                        examen.nombreExamen = nuevoNombreExamen;
                        examen.pathExamen = Path.Combine(curso.nombre, nuevoNombreExamen);
                        examen.tipoExamen = getTipoExamenByCurso(examen.curso_id);
                        examen.division = divisionActual;

                        _context.tblS_CapacitacionSeguridadCursosExamen.Add(examen);
                        _context.SaveChanges();

                        var rutaExamen = Path.Combine(rutaCarpetaCurso, nuevoNombreExamen);
                        listaExamenes.Add(Tuple.Create(archivos[index], rutaExamen));

                        index++;
                    }
                    _context.SaveChanges();

                    if (curso.esGeneral == false)
                    {
                        // Verifica la cantidad de exámenes cargados para actualizar el estatus del curso.
                        var examenesCurso = ObtenerListaExamenesPorCursoID(curso.id);
                        curso.estatus = examenesCurso.Count > 2 ? (int)EstatusCursoEnum.Completo : (int)EstatusCursoEnum.PendienteExamenes;
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

                    // Se intenta hacer el guardado de los archivos.
                    foreach (var examen in listaExamenes)
                    {
                        if (SaveArchivo(examen.Item1, examen.Item2) == false)
                        {
                            dbTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo guardar el examen en el servidor.");
                            return resultado;
                        }
                    }

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "guardarExamenes", e, AccionEnum.AGREGAR, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar los exámenes en el servidor.");
                }
            }


            return resultado;
        }
        public Dictionary<string, object> actualizarCurso(tblS_CapacitacionSeguridadCursos curso, List<tblS_CapacitacionSeguridadCursosMando> mandos, List<tblS_CapacitacionSeguridadCursosPuestos> puestosNuevos, List<tblS_CapacitacionSeguridadCursosPuestosAutorizacion> puestosAutorizacionNuevos, List<tblS_CapacitacionSeguridadCursosCC> centrosCosto)
        {
            var resultado = new Dictionary<string, object>();

            using (DbContextTransaction dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Validaciones para el curso actualizado.
                    if (curso.claveCurso.Length <= 1 || curso.claveCurso.Length > 11)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(Error, "La longitud de la clave del curso es inválida.");
                        return resultado;
                    }
                    else if (EsCursoRepetidoUpdate(curso))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(Error, "Ya existe otro curso con esa clave o con ese nombre.");
                        return resultado;
                    }
                    else if (EsNombreCarpetaInvalido(curso.claveCurso))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(Error, "La clave del curso contiene caracteres inválidos.");
                        return resultado;
                    }
                    else if (EsNombreCarpetaInvalido(curso.nombre))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(Error, "El nombre del curso contiene caracteres inválidos.");
                        return resultado;
                    }

                    #region Mandos
                    var mandosAnteriores = _context.tblS_CapacitacionSeguridadCursosMando.Where(x => x.estatus && x.curso_id == curso.id && x.division == divisionActual).ToList();

                    foreach (var mand in mandosAnteriores)
                    {
                        mand.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var man in mandos)
                    {
                        man.division = divisionActual;
                    }

                    _context.tblS_CapacitacionSeguridadCursosMando.AddRange(mandos);
                    #endregion

                    #region Cursos
                    // Se agregan los puestos nuevos en caso de existir.
                    //var puestos = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.curso_id == curso.id && x.estatus && x.division == divisionActual).ToList();
                    //if (puestos != null && puestos.Count > 0)
                    //{
                    //if (puestosNuevos == null || puestosNuevos.Count < puestos.Count)
                    //{
                    //    dbContextTransaction.Rollback();
                    //    resultado.Add(SUCCESS, false);
                    //    resultado.Add(Error, "No se pueden eliminar puestos de un curso ya creado, sólo agregar.");
                    //    return resultado;
                    //}

                    //var puestosIguales = puestos.Select(x => x.puesto_id).Intersect(puestosNuevos.Select(x => x.puesto_id));

                    //if (puestosIguales.Count() < puestos.Count)
                    //{
                    //    dbContextTransaction.Rollback();
                    //    resultado.Add(SUCCESS, false);
                    //    resultado.Add(Error, "No se pueden eliminar o cambiar puestos de un curso ya creado, sólo agregar.");
                    //    return resultado;
                    //}

                    // Puestos nuevos
                    //var nuevosPuestos = puestosNuevos.Where(x => puestosIguales.Contains(x.puesto_id) == false).ToList();

                    var puestosAnteriores = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.curso_id == curso.id && x.estatus && x.division == divisionActual).ToList();

                    foreach (var pues in puestosAnteriores)
                    {
                        pues.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var pues in puestosNuevos)
                    {
                        pues.division = divisionActual;
                    }

                    _context.tblS_CapacitacionSeguridadCursosPuestos.AddRange(puestosNuevos);
                    _context.SaveChanges();
                    //}
                    #endregion

                    #region Cursos Autorización
                    // Se agregan los puestos nuevos en caso de existir.
                    //var puestosAutorizacion = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Where(x => x.curso_id == curso.id && x.estatus && x.division == divisionActual).ToList();
                    //if (puestosAutorizacion != null && puestosAutorizacion.Count > 0)
                    //{
                    //if (puestosAutorizacionNuevos == null || puestosAutorizacionNuevos.Count < puestosAutorizacion.Count)
                    //{
                    //    dbContextTransaction.Rollback();
                    //    resultado.Add(SUCCESS, false);
                    //    resultado.Add(Error, "No se pueden eliminar puestos de un curso ya creado, sólo agregar.");
                    //    return resultado;
                    //}

                    //var puestosAutorizacionIguales = puestosAutorizacion.Select(x => x.puesto_id).Intersect(puestosAutorizacionNuevos.Select(x => x.puesto_id));

                    //if (puestosAutorizacionIguales.Count() < puestosAutorizacion.Count)
                    //{
                    //    dbContextTransaction.Rollback();
                    //    resultado.Add(SUCCESS, false);
                    //    resultado.Add(Error, "No se pueden eliminar o cambiar puestos de un curso ya creado, sólo agregar.");
                    //    return resultado;
                    //}

                    // Puestos nuevos
                    //var nuevosPuestosAutorizacion = puestosAutorizacionNuevos.Where(x => puestosAutorizacionIguales.Contains(x.puesto_id) == false).ToList();

                    var puestosAutorizacionAnteriores = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Where(x => x.curso_id == curso.id && x.estatus && x.division == divisionActual).ToList(); ;

                    foreach (var pues in puestosAutorizacionAnteriores)
                    {
                        pues.estatus = false;
                        _context.SaveChanges();
                    }

                    if (puestosAutorizacionNuevos != null)
                    {
                        foreach (var pues in puestosAutorizacionNuevos)
                        {
                            pues.division = divisionActual;
                        }

                        _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.AddRange(puestosAutorizacionNuevos);
                        _context.SaveChanges();
                    }
                    //}
                    #endregion

                    #region Centros de Costo
                    var centrosCostoAnteriores = _context.tblS_CapacitacionSeguridadCursosCC.Where(x => x.estatus && x.curso_id == curso.id && x.division == divisionActual).ToList();

                    foreach (var cc in centrosCostoAnteriores)
                    {
                        cc.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var cc in centrosCosto)
                    {
                        cc.division = divisionActual;
                    }

                    _context.tblS_CapacitacionSeguridadCursosCC.AddRange(centrosCosto);
                    #endregion

                    var cursoActual = _context.tblS_CapacitacionSeguridadCursos.First(w => w.id == curso.id && w.division == divisionActual);

                    cursoActual.claveCurso = curso.claveCurso;
                    cursoActual.nombre = curso.nombre;
                    cursoActual.duracion = curso.duracion;
                    cursoActual.objetivo = curso.objetivo;
                    cursoActual.temasPrincipales = curso.temasPrincipales;
                    cursoActual.referenciasNormativas = curso.referenciasNormativas;
                    cursoActual.nota = curso.nota;
                    cursoActual.capacitacionUnica = curso.capacitacionUnica;
                    cursoActual.division = divisionActual;

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(Error, "Ocurrió un error al actualizar el curso.");
                }
            }

            return resultado;
        }
        public Dictionary<string, object> eliminarExamen(int examen_id)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var examen = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.id == examen_id && x.division == divisionActual).FirstOrDefault();
                    if (examen != null)
                    {
                        examen.isActivo = false;
                        _context.SaveChanges();

                        // Evalua cuántos exámenes quedan activos para el curso y modifica su estatus de ser necesario
                        var examenesActivos = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.curso_id == examen.curso_id && x.isActivo && x.division == divisionActual).ToList();
                        var curso = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.id == examen.curso_id && x.division == divisionActual);
                        curso.estatus = examenesActivos.Count >= 3 ? (int)EstatusCursoEnum.Completo : (int)EstatusCursoEnum.PendienteExamenes;

                        _context.SaveChanges();
                        resultado.Add(SUCCESS, true);
                        dbTransaction.Commit();
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                    }
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "eliminarExamen", e, AccionEnum.ELIMINAR, examen_id, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar eliminar el examen.");
                }
            }

            return resultado;
        }
        public bool guardarExamenesInfo(List<tblS_CapacitacionSeguridadCursosExamen> examenes)
        {
            bool seGuardo = false;
            try
            {
                foreach (var exa in examenes)
                {
                    exa.division = divisionActual;
                }

                _context.tblS_CapacitacionSeguridadCursosExamen.AddRange(examenes);
                _context.SaveChanges();
                seGuardo = true;

            }
            catch (Exception e)
            {
                seGuardo = false;
                _msgError = "Ocurrío un error al guardar el examen";
            }

            return seGuardo;

        }
        public bool SaveArchivo(HttpPostedFileBase archivo, string ruta)
        {
            try
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
                File.WriteAllBytes(ruta, data);
            }
            catch (Exception e)
            {
                _msgError = "Ocurrio un error al guardar el archivo.";
                LogError(0, 0, NombreControlador, "GuardarArchivo", e, AccionEnum.AGREGAR, 0, new { archivo = archivo.FileName, ruta = ruta, ContentType = archivo.ContentType });
            }

            return File.Exists(ruta);
        }
        private List<puestosEnkontrolDTO> listPuestosEnkontrol()
        {
            string inf_puesto = "SELECT";
            inf_puesto += " puesto, descripcion ";
            inf_puesto += " FROM  DBA.si_puestos ";

            try
            {
                var resultado = (List<puestosEnkontrolDTO>)ContextEnKontrolNomina.Where(inf_puesto).ToObject<List<puestosEnkontrolDTO>>();
                return resultado;
            }
            catch (Exception)
            {


            }

            return null;
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Verifica si existe una carpeta física en el servidor.
        /// </summary>
        /// <param name="path">Ruta de la carpeta física.</param>
        /// <param name="crear">Bandera que al activarla, si no existe la carpeta, la creará.</param>
        /// <returns></returns>
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
        public int getTipoExamenByCurso(int curso_id)
        {
            int tipoExamen = 0;
            try
            {
                var examenes = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.curso_id == curso_id && x.division == divisionActual).ToList();
                tipoExamen = examenes.Count > 0 ? examenes.Max(x => x.tipoExamen) + 1 : 1;
            }
            catch (Exception e)
            {
                tipoExamen = 0;
            }

            return tipoExamen;
        }

        public Dictionary<string, object> EliminarCurso(int cursoID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var curso = _context.tblS_CapacitacionSeguridadCursos.First(x => x.id == cursoID && x.division == divisionActual);
                    var rutaCarpetaCurso = Path.Combine(RutaCursos, curso.nombre);

                    // Eliminamos puestos.
                    var puestos = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.curso_id == cursoID && x.division == divisionActual).ToList();
                    if (puestos.Count > 0)
                    {
                        _context.tblS_CapacitacionSeguridadCursosPuestos.RemoveRange(puestos);
                    }

                    // Eliminamos puestos autorizacion.
                    var puestosAutorizacion = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Where(x => x.curso_id == cursoID && x.division == divisionActual).ToList();
                    if (puestosAutorizacion.Count > 0)
                    {
                        _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.RemoveRange(puestosAutorizacion);
                    }

                    // Eliminamos exámenes.
                    var examenes = _context.tblS_CapacitacionSeguridadCursosExamen.Where(x => x.curso_id == cursoID && x.division == divisionActual).ToList();
                    if (examenes.Count > 0)
                    {
                        _context.tblS_CapacitacionSeguridadCursosExamen.RemoveRange(examenes);
                    }

                    // Controles de asistencia por eliminar.
                    var controlesAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x => x.cursoID == cursoID && x.division == divisionActual).ToList();
                    if (controlesAsistencia.Count > 0)
                    {
                        var controlesAsistenciaIDs = controlesAsistencia.Select(x => x.id);

                        // Eliminamos notificaciones de autorización.
                        var alertasAutorizacion = _context.tblP_Alerta
                            .Where(x => x.url.Contains("/Administrativo/CapacitacionSeguridad/AutorizacionCapacitacion"))
                            .Where(x => controlesAsistenciaIDs.Contains(x.objID))
                            .ToList();

                        if (alertasAutorizacion.Count > 0)
                        {
                            _context.tblP_Alerta.RemoveRange(alertasAutorizacion);
                        }

                        // Eliminamos detalles de controles de asistencia.
                        var controlAsistenciaDetalles = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(x => x.controlAsistencia.cursoID == cursoID && x.division == divisionActual).ToList();
                        if (controlAsistenciaDetalles.Count > 0)
                        {
                            _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.RemoveRange(controlAsistenciaDetalles);
                        }

                        // Eliminamos autorizaciones.
                        var autorizaciones = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => controlesAsistenciaIDs.Contains(x.controlAsistenciaID) && x.division == divisionActual).ToList();
                        if (autorizaciones.Count > 0)
                        {
                            _context.tblS_CapacitacionSeguridadAutorizacion.RemoveRange(autorizaciones);
                        }

                        // Eliminamos los controles de asistencia.
                        _context.tblS_CapacitacionSeguridadControlAsistencia.RemoveRange(controlesAsistencia);
                    }

                    // Eliminamos el curso.
                    _context.tblS_CapacitacionSeguridadCursos.Remove(curso);
                    _context.SaveChanges();

                    // Tratamos de eliminar la carpeta del curso.
                    if (Directory.Exists(rutaCarpetaCurso))
                    {
                        Directory.Delete(rutaCarpetaCurso, true);
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EliminarCurso", e, AccionEnum.ELIMINAR, cursoID, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar eliminar el curso.");
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getPuestosEnkontrolMandos(List<int> mandos)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
//                string query = @"
//                    SELECT puesto as Value, descripcion as Text 
//                    FROM DBA.si_puestos 
//                    WHERE (descripcion NOT LIKE '%NO USAR%') AND
//                        (descripcion NOT LIKE '%NOUSAR%') AND
//                        (descripcion NOT LIKE '%NO US%') AND
//                        (descripcion NOT LIKE '%NO USA%') AND 
//                        (descripcion NOT LIKE '%NOUSAR%')
//                    ORDER BY Text
//                    ";

                if (mandos != null)
                {
                    //var puestos = (List<Core.DTO.Principal.Generales.ComboDTO>)ContextEnKontrolNomina.Where(query).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                    var puestos = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                        consulta = @"SELECT puesto as Value, descripcion as Text FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%NO US%' AND descripcion NOT LIKE '%(NO%' ORDER BY TEXT",
                    });

                    if (puestos.Count > 0)
                    {
                        var puestosMandos = _context.tblS_CapacitacionSeguridadPuestoMando.Where(x => x.estatus && mandos.Contains((int)x.mando)).Select(x => x.puesto).ToList();

                        puestos = puestos.Where(x => puestosMandos.Contains(Int32.Parse(x.Value))).ToList();

                        resultado.Add(SUCCESS, true);
                        resultado.Add("items", puestos);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("EMPTY", true);
                    }
                }
                else
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", new List<Core.DTO.Principal.Generales.ComboDTO>());
                }
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo tipos de empleados");
            }

            return resultado;
        }
        #endregion

        #region Control de Asistencia
        public Dictionary<string, object> ObtenerComboCC()
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                var privilegio = getPrivilegioActual();
                var lstCc = _context.tblP_CC_Usuario.ToList().Where(w => w.usuarioID == privilegio.idUsuario).ToList();
                var listaCC = _context.tblP_CC.ToList()
                    .Where(x => x.estatus)
                    .Where(x => (PrivilegioEnum)privilegio.idPrivilegio == PrivilegioEnum.Administrador ? true : lstCc.Any(a => a.cc == x.cc))
                    .OrderBy(x => x.cc)
                    .Select(centroCosto => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = centroCosto.cc + " - " + centroCosto.descripcion,
                        Value = centroCosto.cc
                    });

                resultado.Add(ITEMS, listaCC);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerComboCC", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerListaControlesAsistencia(string cc, int estado, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var odbc = new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT
                        cc as Value,
                        (cc + ' - ' + descripcion) as Text
                    FROM cc
                    ORDER BY cc"
                };
                var listaConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanProd, odbc);
                var listaArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, odbc);

                var listaControlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                    x.activo &&
                    (cc == "Todos" ? true : x.cc == cc) &&
                    (estado == 0 ? true : x.estatus == estado) &&
                    x.fechaCapacitacion >= fechaInicio &&
                    x.fechaCapacitacion <= fechaFin && x.division == divisionActual).ToList().Select(x => new
                    {
                        x.id,
                        fecha = x.fechaCapacitacion.ToString("dd/MM/yyyy"),
                        curso = x.curso.nombre,
                        esExterno = x.esExterno,
                        instructor = x.esExterno ? x.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(x.instructor),
                        lugar = x.lugar ?? "Indefinido",
                        clasificacion = x.curso.clasificacion,
                        estatusDesc = ((EstatusControlAsistenciaEnum)x.estatus).GetDescription(),
                        x.estatus,
                        cc = (x.empresa == 1 || x.empresa == 0) ?
                        listaConstruplan.FirstOrDefault(y => y.Value == x.cc) != null ? listaConstruplan.FirstOrDefault(y => y.Value == x.cc).Text : ""
                        :
                        listaArrendadora.FirstOrDefault(y => y.Value == x.cc) != null ? listaArrendadora.FirstOrDefault(y => y.Value == x.cc).Text : ""
                    })
                    .ToList();

                if(vSesiones.sesionEmpresaActual==6)
                {
                    var listaCCPeru = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.ccDescripcion
                    }).ToList();

                    listaControlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                        x.activo &&
                        (cc == "Todos" ? true : x.cc == cc) &&
                        (estado == 0 ? true : x.estatus == estado) &&
                        x.fechaCapacitacion >= fechaInicio &&
                        x.fechaCapacitacion <= fechaFin && x.division == divisionActual).ToList().Select(x => new
                        {
                            x.id,
                            fecha = x.fechaCapacitacion.ToString("dd/MM/yyyy"),
                            curso = x.curso.nombre,
                            esExterno = x.esExterno,
                            instructor = x.esExterno ? x.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(x.instructor),
                            lugar = x.lugar ?? "Indefinido",
                            clasificacion = x.curso.clasificacion,
                            estatusDesc = ((EstatusControlAsistenciaEnum)x.estatus).GetDescription(),
                            x.estatus,
                            cc = listaCCPeru.FirstOrDefault(y => y.Value == x.cc).Value                            
                          
                        }).ToList();
                }


                resultado.Add(ITEMS, listaControlAsistencia);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerListaControlesAsistencia", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public object GetCursosAutocomplete(string term, bool porClave)
        {
            var cursos = _context.tblS_CapacitacionSeguridadCursos
                .Where(x => x.isActivo).ToList().Where(x =>
                    x.estatus == (int)EstatusCursoEnum.Completo &&
                    (porClave ? x.claveCurso.Contains(term) : x.nombre.Contains(term)) && x.division == divisionActual
                ).OrderBy(x => x.id).Take(12).ToList();

            return porClave ?
                cursos.Select(x => new
                {
                    id = x.id,
                    value = x.claveCurso,
                    x.claveCurso,
                    x.nombre,
                    x.duracion,
                    x.objetivo,
                    x.temasPrincipales,
                    x.referenciasNormativas,
                    x.nota,
                    clasificacion = ((ClasificacionCursoEnum)x.clasificacion).GetDescription(),
                    claseSpan = ObtenerClaseSpanClasificacion((ClasificacionCursoEnum)x.clasificacion)
                }).ToList()
                :
                cursos.Select(x => new
                {
                    id = x.id,
                    value = x.nombre,
                    x.claveCurso,
                    x.nombre,
                    duracion = x.duracion,
                    x.objetivo,
                    x.temasPrincipales,
                    x.referenciasNormativas,
                    x.nota,
                    clasificacion = ((ClasificacionCursoEnum)x.clasificacion).GetDescription(),
                    claseSpan = ObtenerClaseSpanClasificacion((ClasificacionCursoEnum)x.clasificacion)
                }).ToList();
        }

        public object GetUsuariosAutocomplete(string term, bool porClave)
        {
            var usuarios = _context.tblP_Usuario
                .Where(x =>
                    x.estatus &&
                    (porClave ? x.cveEmpleado.Contains(term) : (x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).Contains(term))
                ).OrderBy(x => x.id).Take(12).ToList();

            return porClave ?
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = x.cveEmpleado,
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                })
                :
                usuarios.Select(x => new
                {
                    id = x.id,
                    value = GlobalUtils.ObtenerNombreCompletoUsuario(x),
                    claveEmpleado = x.cveEmpleado,
                    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x)
                });
        }

        public object GetLugarCursoAutocomplete(string term)
        {
            return _context.tblS_CapacitacionSeguridadControlAsistencia
                .Where(x => x.activo && x.lugar.Contains(term) && x.division == divisionActual)
                .GroupBy(x => x.lugar)
                .Take(12)
                .Select(x => new
                {
                    id = x.FirstOrDefault().id,
                    value = x.Key
                }).ToList();
        }

        public object GetEmpleadoEnKontrolAutocomplete(string term, bool porClave)
        {
            try
            {
                #region MIGRADO
                
                //                string query =
//                @"
//                SELECT 
//                    e.clave_empleado AS claveEmpleado, 
//                    (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
//                    p.descripcion AS puestoEmpleado,
//                    e.cc_contable AS ccID,
//                    c.descripcion as cc
//                FROM DBA.sn_empleados AS e
//                    INNER JOIN DBA.si_puestos as p on e.puesto=p.puesto
//                    INNER JOIN DBA.cc as c on e.cc_contable=c.cc
//                WHERE e.estatus_empleado ='A' AND " + (porClave ? @" claveEmpleado " : @" nombreEmpleado ") + @" LIKE ?  
//                ORDER BY e.ape_paterno DESC
//                ";

                //var odbc = new OdbcConsultaDTO()
                //{
                //    consulta = query,
                //    parametros = new List<OdbcParameterDTO>
                //    {
                //        new OdbcParameterDTO() { nombre = "term", tipo = OdbcType.VarChar, valor = String.Format("%{0}%", term) }
                //    }
                //};

                //List<dynamic> listaEmpleadosCP = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                //List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);
                #endregion

                var listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO 
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT 
                                e.clave_empleado AS claveEmpleado, 
                                (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                                p.descripcion AS puestoEmpleado,
                                e.cc_contable AS ccID,
                                c.descripcion as cc
                            FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto
                                INNER JOIN tblP_CC as c on e.cc_contable=c.cc
                            WHERE e.estatus_empleado = 'A' AND " + (porClave ? @" e.clave_empleado " : @" (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) ") + @" LIKE CONCAT('%', @term, '%') " +
                            "ORDER BY e.ape_paterno DESC",
                    parametros = new {term}

                });
                listaEmpleadosCP.AddRange( _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT 
                                e.clave_empleado AS claveEmpleado, 
                                (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                                p.descripcion AS puestoEmpleado,
                                e.cc_contable AS ccID,
                                c.descripcion as cc
                            FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto
                                INNER JOIN tblP_CC as c on e.cc_contable=c.cc
                            WHERE e.estatus_empleado = 'A' AND " + (porClave ? @" e.clave_empleado " : @" (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) ") + @" LIKE CONCAT('%', @term, '%') " +
                            "ORDER BY e.ape_paterno DESC",
                    parametros = new { term }

                }));

                var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT 
                                e.clave_empleado AS claveEmpleado, 
                                (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                                p.descripcion AS puestoEmpleado,
                                e.cc_contable AS ccID,
                                c.descripcion as cc
                            FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto
                                INNER JOIN tblP_CC as c on e.cc_contable=c.cc
                            WHERE e.estatus_empleado = 'A' AND " + (porClave ? @" e.clave_empleado " : @" (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) ") + @" LIKE CONCAT('%', @term, '%') " +
                            "ORDER BY e.ape_paterno DESC",
                    parametros = new { term }

                });
                // Juntamos los resultados de ambas consultas.
                listaEmpleadosCP.AddRange(listaEmpleadosARR);

                if (porClave)
                {
                    return listaEmpleadosCP.Select(x => new
                    {
                        id = ((decimal)x.claveEmpleado).ToString(),
                        value = ((decimal)x.claveEmpleado).ToString(),
                        nombre = (string)x.nombreEmpleado,
                        nombreEmpleado = (string)x.nombreEmpleado,
                        puestoEmpleado = (string)x.puestoEmpleado,
                        ccID = (string)x.ccID,
                        cc = (string)x.cc
                    }).ToList();
                }
                else
                {
                    return listaEmpleadosCP.Select(x => new
                    {
                        id = x.claveEmpleado,
                        value = x.nombreEmpleado,
                        nombreEmpleado = (string)x.nombreEmpleado,
                        puestoEmpleado = (string)x.puestoEmpleado,
                        ccID = (string)x.ccID,
                        cc = (string)x.cc
                    }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<string, object> CrearControlAsistencia(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    controlAsistencia.fechaCreacion = DateTime.Now;
                    controlAsistencia.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                    controlAsistencia.division = divisionActual;

                    foreach (var asis in controlAsistencia.asistentes)
                    {
                        asis.division = divisionActual;

                        #region Asignación de cc a los asistentes.
                        //var odbc = new OdbcConsultaDTO()
                        //{
                        //    consulta = @"SELECT * FROM sn_empleados WHERE estatus_empleado = 'A' AND clave_empleado = ?",
                        //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = asis.claveEmpleado } }
                        //};

                        //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                        //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                        var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { asis.claveEmpleado }
                        });

                        empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { asis.claveEmpleado }
                        }));

                        var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { asis.claveEmpleado }
                        });

                        if (empleadoConstruplan.Count() > 0)
                        {
                            asis.cc = (string)empleadoConstruplan[0].cc_contable;
                        }

                        if (empleadoArrendadora.Count() > 0)
                        {
                            asis.cc = (string)empleadoArrendadora[0].cc_contable;
                        }
                        #endregion
                    }

                    #region Modificar el valor "estatusAutorizacion" en base a los puestos del curso.

                    
//                    var odbcPuestos = new OdbcConsultaDTO()
//                    {
//                        consulta = @"
//                            SELECT puesto AS Value, descripcion AS Text
//                            FROM si_puestos
//                            WHERE (descripcion NOT LIKE '%NO USAR%') AND
//                                (descripcion NOT LIKE '%NOUSAR%') AND
//                                (descripcion NOT LIKE '%NO US%') AND
//                                (descripcion NOT LIKE '%NO USA%') AND 
//                                (descripcion NOT LIKE '%NOUSAR%')
//                            ORDER BY Text"
//                    };

                    //List<Core.DTO.Principal.Generales.ComboDTO> puestosEnkontrolConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbcPuestos);
                    //List<Core.DTO.Principal.Generales.ComboDTO> puestosEnkontrolArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, odbcPuestos);

                    var puestosEnkontrolConstruplan = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT puesto as Value, descripcion as Text FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%NO US%' AND descripcion NOT LIKE '%(NO%' ORDER BY TEXT",
                    });

                    var puestosEnkontrolArrendadora = _context.Select<ComboDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT puesto as Value, descripcion as Text FROM tblRH_EK_Puestos WHERE descripcion NOT LIKE '%NO US%' AND descripcion NOT LIKE '%(NO%' ORDER BY TEXT",
                    });

                    var cursoPuestos = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.estatus && x.curso_id == controlAsistencia.cursoID && x.division == divisionActual).ToList();
                    var cursoPuestosAutorizacion = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Where(x => x.estatus && x.curso_id == controlAsistencia.cursoID && x.division == divisionActual).ToList();

                    foreach (var asist in controlAsistencia.asistentes)
                    {
                        ComboDTO puesto = null;
                        puesto = puestosEnkontrolConstruplan.FirstOrDefault(x => x.Text == asist.puesto);

                        if (puesto == null)
                        {
                            puesto = puestosEnkontrolArrendadora.FirstOrDefault(x => x.Text == asist.puesto);

                            if (puesto == null)
                            {
                                throw new Exception("No se encuentra la información del puesto \"" + asist.puesto + "\".");
                            }
                        }

                        if (vSesiones.sesionCapacitacionOperativa)
                        {
                            asist.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado;
                        }
                        else
                        {
                            asist.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica; //Se coloca "NoAplica" por default para que no se guarde con cero.

                            if (cursoPuestos.Count() > 0)
                            {
                                var cursoPuesto = cursoPuestos.FirstOrDefault(x => x.puesto_id == Int32.Parse(puesto.Value));

                                if (cursoPuesto != null)
                                {
                                    asist.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica;
                                }
                            }

                            if (cursoPuestosAutorizacion.Count() > 0)
                            {
                                var cursoPuestoAutorizacion = cursoPuestosAutorizacion.FirstOrDefault(x => x.puesto_id == Int32.Parse(puesto.Value));

                                if (cursoPuestoAutorizacion != null)
                                {
                                    asist.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.PendienteAutorizacion;
                                }
                            }
                        }
                    }
                    #endregion

                    var curso = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.id == controlAsistencia.cursoID && x.division == divisionActual);

                    List<tblS_CapacitacionSeguridadCursosExamen> examenesCurso = null;

                    switch (curso.clasificacion)
                    {
                        case (int)ClasificacionCursoEnum.ProtocoloFatalidad:
                            examenesCurso = ObtenerListaExamenesPorCursoID(controlAsistencia.cursoID);

                            if (examenesCurso.Count() == 0)
                            {
                                throw new Exception("El curso no tiene los formatos de exámenes cargados.");
                            }

                            controlAsistencia.asistentes.ForEach(x =>
                            {
                                x.examenID = ObtenerExamenIdAleatorio(examenesCurso);
                            });
                            break;

                        case (int)ClasificacionCursoEnum.Normativo:
                        case (int)ClasificacionCursoEnum.MandosMedios:
                            examenesCurso = ObtenerListaExamenesPorCursoID(controlAsistencia.cursoID);

                            if (examenesCurso.Count() == 0)
                            {
                                throw new Exception("El curso no tiene los formatos de exámenes cargados.");
                            }

                            controlAsistencia.asistentes.ForEach(x =>
                            {
                                x.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica;
                                x.examenID = ObtenerExamenIdAleatorio(examenesCurso);
                            });
                            break;

                        case (int)ClasificacionCursoEnum.Formativo:
                        case (int)ClasificacionCursoEnum.General:
                        case (int)ClasificacionCursoEnum.Difusion:
                            controlAsistencia.asistentes.ForEach(x => x.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica);
                            break;

                        case (int)ClasificacionCursoEnum.InstructivoOperativo:
                        case (int)ClasificacionCursoEnum.TecnicoOperativo:
                            examenesCurso = ObtenerListaExamenesPorCursoID(controlAsistencia.cursoID);

                            if (examenesCurso.Count() == 0)
                            {
                                throw new Exception("El curso no tiene los formatos de exámenes cargados.");
                            }

                            controlAsistencia.asistentes.ForEach(x =>
                            {
                                x.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica;
                                x.examenID = ObtenerExamenIdAleatorio(examenesCurso);
                            });
                            break;

                        //examenesCurso = ObtenerListaExamenesPorCursoID(controlAsistencia.cursoID);
                        //controlAsistencia.asistentes.ForEach(x =>
                        //{
                        //    x.examenID = ObtenerExamenIdAleatorio(examenesCurso);
                        //});
                        //break;

                        default:
                            break;
                    }

                    _context.tblS_CapacitacionSeguridadControlAsistencia.Add(controlAsistencia);
                    _context.SaveChanges();

                    controlAsistencia.nombreCarpeta = NombreBaseControlAsistencia + controlAsistencia.id;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "CrearControlAsistencia", e, AccionEnum.AGREGAR, 0, controlAsistencia);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
                return resultado;
            }
        }

        public Dictionary<string, object> SubirArchivoControlAsistencia(HttpPostedFileBase archivo, int controlAsistenciaID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                    if (controlAsistencia == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                        return resultado;
                    }

                    var rutaCarpetaControlAsistencia = Path.Combine(RutaControlAsistencias, controlAsistencia.nombreCarpeta);

                    // Verifica si existe la carpeta y si no, la crea.
                    if (verificarExisteCarpeta(rutaCarpetaControlAsistencia, true) == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                        return resultado;
                    }

                    var nombreArchivo = String.Format("{0} {1}{2}",
                        NombreBaseControlAsistencia,
                        ObtenerFormatoCarpetaFechaActual(),
                        Path.GetExtension(archivo.FileName));

                    var rutaArchivo = Path.Combine(rutaCarpetaControlAsistencia, nombreArchivo);

                    // Se actualizan campos de la entidad control de asistencia
                    controlAsistencia.rutaListaAsistencia = rutaArchivo;

                    // Si el curso es general, se aprueba automáticamente a los asistentes
                    if (controlAsistencia.curso.esGeneral || controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.Difusion)
                    {
                        controlAsistencia.asistentes.ForEach(a => a.estatus = (int)EstatusEmpledoControlAsistenciaEnum.Aprobado);
                    }

                    controlAsistencia.estatus = (controlAsistencia.curso.esGeneral || controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.Difusion) ? (int)EstatusControlAsistenciaEnum.Completa : (int)EstatusControlAsistenciaEnum.PendienteGestionarEvaluacion;

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

                    // Si creó el archivo.
                    if (SaveArchivo(archivo, rutaArchivo))
                    {
                        dbContextTransaction.Commit();
                    }
                    else
                    {
                        // Si falla al guardar el archivo
                        dbContextTransaction.Rollback();
                        resultado.Clear();
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "SubirArchivo", e, AccionEnum.AGREGAR, controlAsistenciaID, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar el archivo en el servidor.");
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarListaControlAsistencia(int controlAsistenciaID)
        {
            try
            {
                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                if (controlAsistencia == null)
                {
                    return null;
                }

                var fileStream = GlobalUtils.GetFileAsStream(controlAsistencia.rutaListaAsistencia);
                string name = Path.GetFileName(controlAsistencia.rutaListaAsistencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarListaControlAsistencia", e, AccionEnum.DESCARGAR, controlAsistenciaID, 0);
                return null;
            }
        }

        public Tuple<Stream, string> DescargarFormatoAutorizacion(int controlAsistenciaID)
        {
            try
            {
                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                if (controlAsistencia == null)
                {
                    return null;
                }

                var fileStream = GlobalUtils.GetFileAsStream(controlAsistencia.rutaListaAutorizacion);
                string name = Path.GetFileName(controlAsistencia.rutaListaAutorizacion);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarFormatoAutorizacion", e, AccionEnum.DESCARGAR, controlAsistenciaID, 0);
                return null;
            }
        }

        public Dictionary<string, object> CargarDatosControlAsistencia(int controlAsistenciaID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                if (controlAsistencia == null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al cargar los datos del control de asistencia.");
                    return resultado;
                }

                var controlAsistenciaContainer = new
                {
                    ccDesc = ObtenerDescripcionCC(controlAsistencia.cc),
                    controlAsistencia.id,
                    fechaCapacitacion = controlAsistencia.fechaCapacitacion.ToString("dd/MM/yyyy"),
                    controlAsistencia.curso.duracion,
                    controlAsistencia.curso.claveCurso,
                    nombreCurso = controlAsistencia.curso.nombre,
                    claveInstructor = controlAsistencia.esExterno ?
                        0 : controlAsistencia.instructorID,
                    nombreInstructor = controlAsistencia.esExterno ?
                        controlAsistencia.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(controlAsistencia.instructor),
                    empresaExterna = controlAsistencia.esExterno ?
                        controlAsistencia.empresaExterna : "N/A.",
                    controlAsistencia.lugar,
                    controlAsistencia.horario,
                    objetivos = controlAsistencia.curso.objetivo,
                    controlAsistencia.curso.temasPrincipales,
                    controlAsistencia.curso.referenciasNormativas,
                    notas = controlAsistencia.curso.nota,
                    clasificacion = ((ClasificacionCursoEnum)controlAsistencia.curso.clasificacion).GetDescription(),
                    claseSpan = ObtenerClaseSpanClasificacion((ClasificacionCursoEnum)controlAsistencia.curso.clasificacion),
                    asistentes = controlAsistencia.asistentes.Select(x => new
                    {
                        x.id,
                        x.claveEmpleado,
                        nombreEmpleado = ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                        x.puesto,
                        ccDesc = ObtenerDescripcionCC(x.cc),
                        aplicaAutorizacion = x.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica
                    }).ToList()
                };

                resultado.Add("controlAsistencia", controlAsistenciaContainer);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al cargar los datos del control de asistencia desde el servidor.");
                LogError(0, 0, NombreControlador, "CargarDatosControlAsistencia", e, AccionEnum.CONSULTA, controlAsistenciaID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> CargarAsistentesCapacitacion(int controlAsistenciaID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                if (controlAsistencia == null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al cargar los datos del control de asistencia.");
                    return resultado;
                }

                #region Autorizantes
                var listaRelacionesCCAutorizantes = _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.Where(x => x.estatus && x.cc == controlAsistencia.cc && x.division == divisionActual).ToList();
                var listaAutorizantes = (from rel in listaRelacionesCCAutorizantes
                                         join usu in _context.tblP_Usuario.Where(x => x.estatus).ToList() on rel.usuarioID equals usu.id
                                         select new
                                         {
                                             usuarioID = rel.usuarioID,
                                             nombre = string.Format(@"{0} {1} {2}", usu.nombre, usu.apellidoPaterno, usu.apellidoMaterno),
                                             tipoPuesto = rel.tipoPuesto,
                                             orden = rel.orden
                                         }).ToList();

                resultado.Add("listaAutorizantes", listaAutorizantes);
                #endregion

                var asistentes = controlAsistencia.asistentes.Select(x => new
                {
                    x.id,
                    x.claveEmpleado,
                    nombreEmpleado = ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                    x.puesto,
                    ccDesc = ObtenerDescripcionCC(x.cc),
                    x.estatus,
                    estatusDesc = ((EstatusEmpledoControlAsistenciaEnum)x.estatus).GetDescription(),
                    x.estatusAutorizacion,
                    estatusAutorizacionDesc = ((EstatusAutorizacionEmpleadoControlAsistenciaEnum)x.estatusAutorizacion).GetDescription(),
                    controlAsistencia.curso.clasificacion,
                    tipoExamen = ObtenerTipoExamen(x.examenID),
                    rutaDC3 = x.rutaDC3,
                    calificacion = x.calificacion
                }).ToList();
                ;

                resultado.Add(ITEMS, asistentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "CargarAsistentesCapacitacion", e, AccionEnum.CONSULTA, controlAsistenciaID, null);
                resultado.Add(MESSAGE, "Ocurrió un error al cargar los asistentes desde el servidor.");
            }
            return resultado;
        }

        public Dictionary<string, object> GuardarExamenesAsistentes(List<ExamenesAsistenteDTO> listaExamenesAsistentes, int jefeID, int coordinadorID, int secretarioID, int gerenteID, string rfc, string razonSocial)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                bool moduloCapacitacionOperativa = vSesiones.sesionSistemaActual == 18;
                var listaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                tblS_CapacitacionSeguridadControlAsistencia controlAsistencia = null;

                var asistentes = new List<tblS_CapacitacionSeguridadControlAsistenciaDetalle>();

                try
                {
                    if (listaExamenesAsistentes == null || listaExamenesAsistentes.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de exámenes está vacía.");
                        return resultado;
                    }

                    var aplicaAutorizacionPorPuesto = false;

                    // Se itera sobre cada empleado y sus examenes
                    foreach (var examenesAsistente in listaExamenesAsistentes)
                    {
                        var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(x => x.id == examenesAsistente.id && x.division == divisionActual);

                        if (controlAsistencia == null)
                        {
                            controlAsistencia = controlAsistenciaDetalle.controlAsistencia;
                        }

                        var puestosAutorizacion = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Where(x =>
                            x.estatus && x.division == divisionActual && x.curso_id == controlAsistencia.cursoID
                        ).ToList();

                        var nombreEmpleado = ObtenerNombreEmpleadoPorClave(examenesAsistente.claveEmpleado);

                        string nombreCarpetaExpedienteEmpleado = ObtenerFormatoCarpetaExpediente(examenesAsistente.claveEmpleado, nombreEmpleado);

                        var rutaCarpetaExpedienteEmpleado = Path.Combine(RutaExpedientes, nombreCarpetaExpedienteEmpleado);

                        // Verifica si existe la carpeta y si no, la crea.
                        if (verificarExisteCarpeta(rutaCarpetaExpedienteEmpleado, true) == false)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                            return resultado;
                        }

                        controlAsistenciaDetalle.rutaExamenInicial = "";

                        //Si se encuentra en el módulo de capacitación operativa, no lleva examen de diagnóstico. Si se encuentra en capital humano, sí lleva.
                        if (!moduloCapacitacionOperativa)
                        {
                            var nombreExamenDiagnostico = ObtenerFormatoNombreExamen(examenesAsistente.examenDiagnostico.FileName, TipoExamenEnum.Diagnostico);
                            var rutaArchivoInicial = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreExamenDiagnostico);

                            controlAsistenciaDetalle.rutaExamenInicial = rutaArchivoInicial;

                            listaArchivos.Add(Tuple.Create(examenesAsistente.examenDiagnostico, rutaArchivoInicial));
                        }


                        var nombreExamenFinal = ObtenerFormatoNombreExamen(examenesAsistente.examenFinal.FileName, TipoExamenEnum.Final);
                        var rutaArchivoFinal = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreExamenFinal);

                        // Se actualizan campos de la entidad control de asistencia detalle
                        controlAsistenciaDetalle.rutaExamenFinal = rutaArchivoFinal;
                        controlAsistenciaDetalle.estatus = examenesAsistente.aprobado ? (int)EstatusEmpledoControlAsistenciaEnum.Aprobado : (int)EstatusEmpledoControlAsistenciaEnum.Reprobado;
                        controlAsistenciaDetalle.calificacion = examenesAsistente.calificacion;

                        #region Verificar si el puesto lleva autorización.
                        var empleadoRequiereAutorizacion = false;

                        //var odbc = new OdbcConsultaDTO()
                        //{
                        //    consulta = @"SELECT * FROM sn_empleados WHERE estatus_empleado = 'A' AND clave_empleado = ?",
                        //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = controlAsistenciaDetalle.claveEmpleado } }
                        //};

                        //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                        //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                        var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { controlAsistenciaDetalle.claveEmpleado }
                        });

                        empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { controlAsistenciaDetalle.claveEmpleado }
                        }));

                        var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                            parametros = new { controlAsistenciaDetalle.claveEmpleado }
                        });

                        if (empleadoConstruplan.Count() > 0)
                        {
                            var puestoEK = (int)empleadoConstruplan[0].puesto;

                            if (puestosAutorizacion.Count > 0)
                            {
                                if (puestosAutorizacion.Select(x => x.puesto_id).Contains(puestoEK))
                                {
                                    empleadoRequiereAutorizacion = true;
                                    aplicaAutorizacionPorPuesto = true;
                                }
                            }
                        }

                        if (empleadoArrendadora.Count() > 0)
                        {
                            var puestoEK = (int)empleadoArrendadora[0].puesto;

                            if (puestosAutorizacion.Count > 0)
                            {
                                if (puestosAutorizacion.Select(x => x.puesto_id).Contains(puestoEK))
                                {
                                    empleadoRequiereAutorizacion = true;
                                    aplicaAutorizacionPorPuesto = true;
                                }
                            }
                        }

                        controlAsistenciaDetalle.estatusAutorizacion = empleadoRequiereAutorizacion ?
                            (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.PendienteAutorizacion :
                            (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado;
                        #endregion

                        // Se agregan los archivos a la lista de archivos por crear.
                        listaArchivos.Add(Tuple.Create(examenesAsistente.examenFinal, rutaArchivoFinal));

                        asistentes.Add(controlAsistenciaDetalle);
                    }

                    // Se verifica si algun empleado aplicaba para autorización y si aprobó, para asignar el estatus del control de asistencia.
                    var aplicaAutorizacion = asistentes.Any(x => x.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.PendienteAutorizacion && x.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado);

                    if (vSesiones.sesionCapacitacionOperativa)
                    {
                        controlAsistencia.estatus = (int)EstatusControlAsistenciaEnum.Completa;

                        //Se colocan como false para que no se envíe el correo de autorización.
                        aplicaAutorizacion = false;
                        aplicaAutorizacionPorPuesto = false;
                    }
                    else
                    {
                        controlAsistencia.estatus = aplicaAutorizacion ? (int)EstatusControlAsistenciaEnum.PendienteAutorizacion : (int)EstatusControlAsistenciaEnum.Completa;

                        // Si aplica para autorización, se crea un registro por cada tipo de autorizante.
                        if (aplicaAutorizacion || aplicaAutorizacionPorPuesto)
                        {
                            //if (String.IsNullOrEmpty(rfc))
                            //{
                            //    dbContextTransaction.Rollback();
                            //    resultado.Clear();
                            //    resultado.Add(SUCCESS, false);
                            //    resultado.Add(MESSAGE, "Algunos asistentes aplican para autorización y sin embargo no se especificó el RFC.");
                            //    return resultado;
                            //}
                            //else if (String.IsNullOrEmpty(razonSocial))
                            //{
                            //    dbContextTransaction.Rollback();
                            //    resultado.Clear();
                            //    resultado.Add(SUCCESS, false);
                            //    resultado.Add(MESSAGE, "Algunos asistentes aplican para autorización  y sin embargo no se especificó la razón social.");
                            //    return resultado;
                            //}
                            if (jefeID == 0 || coordinadorID == 0 || secretarioID == 0 || gerenteID == 0)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Algunos asistentes aplican para autorización y sin embargo no se indicaron las personas autorizantes.");
                                return resultado;
                            }
                            CrearRegistrosAutorizantes(controlAsistencia, jefeID, coordinadorID, secretarioID, gerenteID);

                            controlAsistencia.rfc = ""; //controlAsistencia.rfc = rfc.Trim();
                            controlAsistencia.razonSocial = ""; //controlAsistencia.razonSocial = razonSocial.Trim();

                            // Se crea la alerta para el primer usuario.
                            var mensaje = String.Format("Autorizar Capacitación: {0}", controlAsistencia.curso.claveCurso);
                            CrearAlertaAutorizacion(mensaje, new List<int> { jefeID }, controlAsistencia.id);
                        }
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("aplicaAutorizacion", aplicaAutorizacion);
                    resultado.Add("aplicaAutorizacionPorPuesto", aplicaAutorizacionPorPuesto);
                    resultado.Add("controlAsistenciaID", controlAsistencia.id);

                    // Se intenta hacer el guardado de los archivos.
                    foreach (var examenes in listaArchivos)
                    {
                        //try
                        //{
                        byte[] data;
                        using (Stream inputStream = examenes.Item1.InputStream)
                        {
                            MemoryStream memoryStream = inputStream as MemoryStream;
                            if (memoryStream == null)
                            {
                                memoryStream = new MemoryStream();
                                inputStream.CopyTo(memoryStream);
                            }
                            data = memoryStream.ToArray();
                        }
                        File.WriteAllBytes(examenes.Item2, data);
                        //}
                        //catch (Exception e)
                        //{
                        //    _msgError = "Ocurrio un error al guardar el archivo.";
                        //    LogError(0, 0, NombreControlador, "GuardarArchivo", e, AccionEnum.AGREGAR, 0, new { archivo = examenes.Item1.FileName, ruta = examenes.Item2, ContentType = examenes.Item1.ContentType });
                        //}

                        var existeArchivo = File.Exists(examenes.Item2);

                        if (!existeArchivo)
                        {
                            throw new Exception("No se pudo crear el archivo.");
                        }

                        //if (SaveArchivo(examenes.Item1, examenes.Item2) == false)
                        //{
                        //    dbContextTransaction.Rollback();
                        //    resultado.Clear();
                        //    resultado.Add(SUCCESS, false);
                        //    resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                        //    return resultado;
                        //}
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarExamenesAsistentes", e, AccionEnum.AGREGAR, 0, listaExamenesAsistentes);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEvaluacionAsistentes(List<AsistenteCursoDTO> listaAsistentes)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {

                tblS_CapacitacionSeguridadControlAsistencia controlAsistencia = null;

                try
                {
                    if (listaAsistentes == null || listaAsistentes.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de asistentes está vacía.");
                        return resultado;
                    }

                    // Se itera sobre cada empleado y sus examenes
                    foreach (var evaluacionAsistente in listaAsistentes)
                    {
                        var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(x => x.id == evaluacionAsistente.id && x.division == divisionActual);

                        if (controlAsistencia == null)
                        {
                            controlAsistencia = controlAsistenciaDetalle.controlAsistencia;
                        }

                        // Se actualizan campos de la entidad control de asistencia detalle
                        controlAsistenciaDetalle.estatus = evaluacionAsistente.aprobado ? (int)EstatusEmpledoControlAsistenciaEnum.Aprobado : (int)EstatusEmpledoControlAsistenciaEnum.Reprobado;
                        controlAsistenciaDetalle.calificacion = evaluacionAsistente.calificacion;
                    }


                    controlAsistencia.estatus = (int)EstatusControlAsistenciaEnum.Completa;

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarEvaluacionAsistentes", e, AccionEnum.ACTUALIZAR, 0, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar las evaluaciones de los asistentes en el servidor.");
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarExamenAsistente(int controlAsistenciaDetalleID, int tipoExamen)
        {
            try
            {
                var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(x => x.id == controlAsistenciaDetalleID && x.division == divisionActual);

                if (controlAsistenciaDetalle == null)
                {
                    return null;
                }

                string rutaExamen = "";

                switch (tipoExamen)
                {
                    case 1:
                        rutaExamen = controlAsistenciaDetalle.rutaExamenInicial;
                        break;
                    case 2:
                        rutaExamen = controlAsistenciaDetalle.rutaExamenFinal;
                        break;
                    case 3:
                        var examen = _context.tblS_CapacitacionSeguridadCursosExamen.FirstOrDefault(x => x.id == controlAsistenciaDetalle.examenID && x.division == divisionActual);

                        if (examen == null)
                        {
                            throw new Exception("No se encontró el examen base del asistente");
                        }

                        rutaExamen = Path.Combine(RutaCursos, examen.pathExamen);
                        break;
                    default:
                        throw new Exception("Tipo de examen no definido");
                }


                var fileStream = GlobalUtils.GetFileAsStream(rutaExamen);
                string name = Path.GetFileName(rutaExamen);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarExamenAsistente", e, AccionEnum.DESCARGAR, controlAsistenciaDetalleID, 0);
                return null;
            }
        }

        public ControlAsistenciaDTO ObtenerDatosControlAsistenciaReporte(int controlAsistenciaID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);

                if (controlAsistencia == null)
                {
                    return null;
                }

                int numeroAsistente = 0;

                var controlAsistenciaDTO = new ControlAsistenciaDTO
                {
                    ccCurso = ObtenerDescripcionCC(controlAsistencia.cc),
                    id = controlAsistencia.id,
                    fechaCapacitacion = controlAsistencia.fechaCapacitacion.ToString("dd/MM/yyyy"),
                    duracion = String.Format("{0:0.0}", controlAsistencia.curso.duracion),
                    nombreCurso = controlAsistencia.curso.nombre,
                    esExterno = controlAsistencia.esExterno,
                    nombreInstructor = controlAsistencia.esExterno ?
                        controlAsistencia.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(controlAsistencia.instructor),
                    empresaExterna = controlAsistencia.esExterno ? controlAsistencia.empresaExterna : "",
                    lugar = controlAsistencia.lugar,
                    horario = controlAsistencia.horario,
                    objetivos = controlAsistencia.curso.objetivo,
                    temasPrincipales = controlAsistencia.curso.temasPrincipales,
                    asistentes = controlAsistencia.asistentes.Select(x => new AsistenteCapacitacionDTO
                    {
                        numeroAsistente = ++numeroAsistente,
                        claveEmpleado = x.claveEmpleado.ToString(),
                        nombreEmpleado = ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                        puesto = x.puesto,
                        cc = x.cc,
                    }).ToList()
                };

                foreach (var asis in controlAsistenciaDTO.asistentes)
                {
                    #region Asignación de cc a los asistentes.
                    //var odbc = new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT * FROM sn_empleados WHERE estatus_empleado = 'A' AND clave_empleado = ?",
                    //    parametros = new List<OdbcParameterDTO>() { new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = asis.claveEmpleado } }
                    //};

                    //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                    //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                    var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                        parametros = new { asis.claveEmpleado }
                    });

                    empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                        parametros = new { asis.claveEmpleado }
                    }));

                    var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT * FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A' AND clave_empleado = @claveEmpleado",
                        parametros = new { asis.claveEmpleado }
                    });

                    if (empleadoConstruplan.Count() > 0)
                    {
                        asis.cc = (string)empleadoConstruplan[0].cc_contable;
                    }

                    if (empleadoArrendadora.Count() > 0)
                    {
                        asis.cc = (string)empleadoArrendadora[0].cc_contable;
                    }
                    #endregion
                }

                return controlAsistenciaDTO;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerDatosControlAsistenciaReporte", e, AccionEnum.CONSULTA, controlAsistenciaID, null);
                return null;
            }
        }

        public Dictionary<string, object> EliminarControlAsistencia(int controlAsistenciaID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.First(x => x.id == controlAsistenciaID && x.division == divisionActual);
                    var rutaCarpetaControlAsistencia = Path.Combine(RutaControlAsistencias, controlAsistencia.nombreCarpeta);

                    // Eliminamos notificaciones de autorización.
                    var alertasAutorizacion = _context.tblP_Alerta
                        .Where(x => x.url.Contains("/Administrativo/CapacitacionSeguridad/AutorizacionCapacitacion"))
                        .Where(x => x.objID == controlAsistenciaID)
                        .ToList();

                    if (alertasAutorizacion.Count > 0)
                    {
                        _context.tblP_Alerta.RemoveRange(alertasAutorizacion);
                    }

                    // Eliminamos autorizaciones.
                    var autorizaciones = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();
                    if (autorizaciones.Count > 0)
                    {
                        _context.tblS_CapacitacionSeguridadAutorizacion.RemoveRange(autorizaciones);
                    }

                    // Eliminamos detalles de controles de asistencia.
                    var controlAsistenciaDetalles = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();
                    if (controlAsistenciaDetalles.Count > 0)
                    {
                        _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.RemoveRange(controlAsistenciaDetalles);
                    }

                    // Eliminamos el control de asistencia.
                    _context.tblS_CapacitacionSeguridadControlAsistencia.Remove(controlAsistencia);
                    _context.SaveChanges();

                    // Tratamos de eliminar la carpeta del curso.
                    if (Directory.Exists(rutaCarpetaControlAsistencia))
                    {
                        Directory.Delete(rutaCarpetaControlAsistencia, true);
                    }

                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "EliminarControlAsistencia", e, AccionEnum.ELIMINAR, controlAsistenciaID, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar eliminar el control de asistencia.");
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarArchivosDC3(HttpPostedFileBase archivoDC3, int controlAsistenciaDetalleID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(x => x.id == controlAsistenciaDetalleID && x.division == divisionActual);
                    var nombreEmpleado = ObtenerNombreEmpleadoPorClave(controlAsistenciaDetalle.claveEmpleado);
                    var nombreCarpetaExpedienteEmpleado = ObtenerFormatoCarpetaExpediente(controlAsistenciaDetalle.claveEmpleado, nombreEmpleado);
                    var rutaCarpetaExpedienteEmpleado = Path.Combine(RutaExpedientes, nombreCarpetaExpedienteEmpleado);

                    // Verifica si existe la carpeta y si no, la crea.
                    if (verificarExisteCarpeta(rutaCarpetaExpedienteEmpleado, true) == false)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                        return resultado;
                    }

                    var nombreArchivoDC3 = ObtenerFormatoNombreArchivoDC3(archivoDC3.FileName);
                    var rutaArchivoDC3 = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreArchivoDC3);

                    // Se actualizan campos de la entidad control de asistencia detalle
                    controlAsistenciaDetalle.rutaDC3 = rutaArchivoDC3;
                    _context.SaveChanges();

                    byte[] data;
                    using (Stream inputStream = archivoDC3.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }
                    File.WriteAllBytes(rutaArchivoDC3, data);

                    var existeArchivo = File.Exists(rutaArchivoDC3);

                    if (!existeArchivo)
                    {
                        throw new Exception("No se pudo crear el archivo.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarArchivoDC3", e, AccionEnum.AGREGAR, 0, controlAsistenciaDetalleID);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarDC3(int controlAsistenciaDetalleID)
        {
            try
            {
                var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(x => x.id == controlAsistenciaDetalleID && x.division == divisionActual);

                if (controlAsistenciaDetalle == null)
                {
                    return null;
                }

                var fileStream = GlobalUtils.GetFileAsStream(controlAsistenciaDetalle.rutaDC3);
                string name = Path.GetFileName(controlAsistenciaDetalle.rutaDC3);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarDC3", e, AccionEnum.DESCARGAR, controlAsistenciaDetalleID, 0);
                return null;
            }
        }
        #endregion

        #region Autorización Capacitación
        public Dictionary<string, object> ObtenerComboEstatusAutorizacionCapacitacion()
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var comboEstatus = GlobalUtils.ParseEnumToCombo<EstatusAutorizacionCapacitacion>();

                resultado.Add(ITEMS, comboEstatus);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerComboEstatusAutorizacionCapacitacion", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        public Dictionary<string, object> ObtenerAutorizaciones(string cc, int curso, int estatus)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var listaControlesAsistencia = _context.tblS_CapacitacionSeguridadAutorizacion
                    .Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.division == divisionActual)
                    .Select(x => x.controlAsistenciaID)
                    .ToList();

                var listaControlesAsistenciaFiltrados = (from a in listaControlesAsistencia
                                                         join b in _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x => x.activo).ToList() on a equals b.id
                                                         select a).ToList();

                var privilegio = getPrivilegioActual().idPrivilegio;

                var esAdmin = (PrivilegioEnum)privilegio == PrivilegioEnum.Administrador || (PrivilegioEnum)privilegio == PrivilegioEnum.ControlDocumentos;

                var puedeVer = esAdmin || listaControlesAsistenciaFiltrados.Count > 0;

                if (puedeVer == false)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "El usuario no ha sido designado como autorizante para ninguna capacitación aplicable.");
                    resultado.Add("reporteAutorizacionGeneral", null);
                    return resultado;
                }

                var autorizacionesPendientes = _context.tblS_CapacitacionSeguridadAutorizacion
                    .Where(x =>
                        (cc == "Todos" ? true : x.controlAsistencia.cc == cc) &&
                        (curso > 0 ? x.controlAsistencia.cursoID == curso : true) &&
                        (esAdmin || listaControlesAsistenciaFiltrados.Contains(x.controlAsistenciaID)) && x.division == divisionActual)
                    .ToList()
                    .GroupBy(x => x.controlAsistenciaID)
                    .Where(x =>
                    {
                        switch (estatus)
                        {
                            case 0:
                                return true;
                            case 1:

                                var estatusGeneral = ObtenerEstatusAutorizacion(x);

                                if (esAdmin)
                                {
                                    return EstatusAutorizacionCapacitacion.Pendiente == estatusGeneral;
                                }
                                else if (estatusGeneral != EstatusAutorizacionCapacitacion.Pendiente)
                                {
                                    return false;
                                }

                                var autorizanteEnTurno = x
                                     .OrderBy(y => y.orden)
                                     .FirstOrDefault(y => (EstatusAutorizacionCapacitacion)y.estatus == EstatusAutorizacionCapacitacion.Pendiente);

                                return autorizanteEnTurno.usuarioID == vSesiones.sesionUsuarioDTO.id;

                            case 2:
                                return EstatusAutorizacionCapacitacion.Autorizado == ObtenerEstatusAutorizacion(x);
                            case 3:
                                return EstatusAutorizacionCapacitacion.Rechazado == ObtenerEstatusAutorizacion(x);
                            default:
                                return false;
                        }
                    })
                    .Select(x => new
                    {
                        id = x.Key,
                        nombreCurso = String.Format("{0} - {1}",
                            x.FirstOrDefault().controlAsistencia.curso.claveCurso,
                            x.FirstOrDefault().controlAsistencia.curso.nombre),
                        fechaCapacitacion = x.FirstOrDefault().controlAsistencia.fechaCapacitacion.ToShortDateString(),
                        instructor = x.FirstOrDefault().controlAsistencia.esExterno ?
                         x.FirstOrDefault().controlAsistencia.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(x.FirstOrDefault().controlAsistencia.instructor),
                        ccDesc = ObtenerDescripcionCC(x.FirstOrDefault().controlAsistencia.cc),
                        comentario = x.FirstOrDefault().controlAsistencia.comentario ?? "",
                        estatusDesc = ObtenerEstatusAutorizacionDesc(x),
                        estatus = ObtenerEstatusAutorizacion(x)
                    }).ToList();

                //Filtrar la información por los controles de asistencia activos.
                autorizacionesPendientes = (from a in autorizacionesPendientes
                                            join b in _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x => x.activo).ToList() on a.id equals b.id
                                            select a).ToList();

                #region Autorizantes Reporte General
                var autorizacionPendiente = autorizacionesPendientes.FirstOrDefault();

                if (autorizacionPendiente != null)
                {
                    //foreach (var aut in autorizacionesPendientes)
                    //{
                    var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == autorizacionPendiente.id && x.division == divisionActual);
                    var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == autorizacionPendiente.id && x.division == divisionActual).ToList();

                    var jefe = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.CoordinadorCMCAP);
                    var gerente = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.GerenteProyecto);
                    var coordinador = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.CoordinadorCSH);
                    var secretario = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.SecretarioCSH);

                    var listaRelacionCCDepartamentoRazonSocial = _context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Where(x => x.estatus && x.division == divisionActual).ToList();
                    var listaRazonSocial = _context.tblS_CapacitacionSeguridadRazonSocial.Where(x => x.estatus).ToList();

                    var listaAsistentes = new List<AsistenteCapacitacionDTO>();

                    foreach (var aut in autorizacionesPendientes)
                    {
                        var autControlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == aut.id && x.division == divisionActual);

                        foreach (var asis in autControlAsistencia.asistentes)
                        {
                            if (asis.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica && asis.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado)
                            {
                                var tuplaDepCC = ObtenerDepartamentoCCPorClaveEmpleado(asis.claveEmpleado);

                                listaAsistentes.Add(new AsistenteCapacitacionDTO
                                {
                                    claveEmpleado = asis.claveEmpleado.ToString(),
                                    nombreEmpleado = ObtenerNombreEmpleadoPorClave(asis.claveEmpleado),
                                    puesto = asis.puesto,
                                    departamento = tuplaDepCC != null ? tuplaDepCC.Item1 : "",
                                    cc = tuplaDepCC != null ? tuplaDepCC.Item2 : ""
                                });
                            }
                        }
                    }

                    foreach (var asis in listaAsistentes)
                    {
                        var relacionCCDepartamentoRazonSocial = listaRelacionCCDepartamentoRazonSocial.FirstOrDefault(x => x.estatus && x.cc == asis.cc && x.departamento == asis.departamento);

                        if (relacionCCDepartamentoRazonSocial != null)
                        {
                            asis.razonSocial = relacionCCDepartamentoRazonSocial.razonSocial.razonSocial;
                            asis.rfc = relacionCCDepartamentoRazonSocial.razonSocial.rfc;
                        }
                        else
                        {
                            asis.razonSocial = "";
                            asis.rfc = "";
                        }
                    }

                    var controlAsistenciaDTO = new FormatoAutorizacionDTO
                    {
                        nombreCurso = controlAsistencia.curso.nombre,
                        claveCurso = controlAsistencia.curso.claveCurso,
                        revision = "01",
                        fechaExpedicion = controlAsistencia.fechaCapacitacion.ToShortDateString(),
                        fechaVencimiento = controlAsistencia.fechaCapacitacion.AddYears(1).ToShortDateString(),
                        razonSocial = controlAsistencia.razonSocial,
                        rfc = controlAsistencia.rfc,
                        nota = controlAsistencia.curso.nota,

                        nombreJefe = GlobalUtils.ObtenerNombreCompletoUsuario(jefe.usuario),
                        firmaJefe = jefe.firma,

                        nombreGerente = GlobalUtils.ObtenerNombreCompletoUsuario(gerente.usuario),
                        firmaGerente = gerente.firma,

                        nombreCoordinador = GlobalUtils.ObtenerNombreCompletoUsuario(coordinador.usuario),
                        firmaCoordinador = coordinador.firma,

                        nombreSecretario = GlobalUtils.ObtenerNombreCompletoUsuario(secretario.usuario),
                        firmaSecretario = secretario.firma,

                        referenciaNormativa = controlAsistencia.curso.referenciasNormativas,

                        asistentes = listaAsistentes
                    };
                    //}

                    resultado.Add("reporteAutorizacionGeneral", controlAsistenciaDTO);
                }
                else
                {
                    resultado.Add("reporteAutorizacionGeneral", null);
                }
                #endregion

                resultado.Add(ITEMS, autorizacionesPendientes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerAutorizacionesPendientes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(MESSAGE, "Ocurrió un error al tratar de obtener la lista de autorizaciones.");
            }
            return resultado;
        }

        private string ObtenerEstatusAutorizacionDesc(IGrouping<int, tblS_CapacitacionSeguridadAutorizacion> autorizantes)
        {
            return autorizantes.All(y => y.estatus == (int)EstatusAutorizacionCapacitacion.Autorizado) ?
                    EstatusAutorizacionCapacitacion.Autorizado.GetDescription() :
                        autorizantes.Any(y => y.estatus == (int)EstatusAutorizacionCapacitacion.Rechazado) ?
                        EstatusAutorizacionCapacitacion.Rechazado.GetDescription() :
                    EstatusAutorizacionCapacitacion.Pendiente.GetDescription();
        }

        private EstatusAutorizacionCapacitacion ObtenerEstatusAutorizacion(IGrouping<int, tblS_CapacitacionSeguridadAutorizacion> autorizantes)
        {
            return autorizantes.All(y => y.estatus == (int)EstatusAutorizacionCapacitacion.Autorizado) ?
                    EstatusAutorizacionCapacitacion.Autorizado :
                        autorizantes.Any(y => y.estatus == (int)EstatusAutorizacionCapacitacion.Rechazado) ?
                        EstatusAutorizacionCapacitacion.Rechazado :
                    EstatusAutorizacionCapacitacion.Pendiente;
        }

        public Dictionary<string, object> ObtenerAutorizantes(int capacitacionID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                bool esAdmin = false;

                var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion
                    .Where(x => x.controlAsistenciaID == capacitacionID && x.division == divisionActual)
                    .OrderBy(x => x.orden)
                    .ToList()
                    .Select(x => new UsuarioAutorizanteDTO
                    {
                        usuarioID = x.usuarioID,
                        firma = x.firma,
                        estatus = ((EstatusAutorizacionCapacitacion)x.estatus),
                        nombre = GlobalUtils.ObtenerNombreCompletoUsuario(x.usuario),
                        orden = x.orden,
                        puestoDesc = ((PuestoAutorizanteEnum)x.tipoPuesto).GetDescription()
                    }).ToList();

                var autorizanteEnTurno = autorizantes.FirstOrDefault(x => x.estatus == EstatusAutorizacionCapacitacion.Pendiente);
                if (autorizanteEnTurno != null)
                {
                    autorizanteEnTurno.puedeAutorizar = autorizanteEnTurno.usuarioID == vSesiones.sesionUsuarioDTO.id || esAdmin;
                }

                resultado.Add(ITEMS, autorizantes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerAutorizantes", e, AccionEnum.CONSULTA, capacitacionID, null);
            }
            return resultado;
        }

        public FormatoAutorizacionDTO ObtenerDatosFormatoAutorizacion(int controlAsistenciaID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var controlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaID && x.division == divisionActual);
                var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();

                var jefe = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.CoordinadorCMCAP);
                var gerente = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.GerenteProyecto);
                var coordinador = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.CoordinadorCSH);
                var secretario = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)PuestoAutorizanteEnum.SecretarioCSH);

                if (controlAsistencia == null)
                {
                    return null;
                }

                var controlAsistenciaDTO = new FormatoAutorizacionDTO
                {
                    nombreCurso = controlAsistencia.curso.nombre,
                    claveCurso = controlAsistencia.curso.claveCurso,
                    fechaExpedicion = controlAsistencia.fechaCapacitacion.ToShortDateString(),
                    fechaVencimiento = controlAsistencia.fechaCapacitacion.AddYears(1).ToShortDateString(),
                    razonSocial = controlAsistencia.razonSocial,
                    rfc = controlAsistencia.rfc,
                    nota = controlAsistencia.curso.nota,

                    nombreJefe = GlobalUtils.ObtenerNombreCompletoUsuario(jefe.usuario),
                    firmaJefe = jefe.firma,

                    nombreGerente = GlobalUtils.ObtenerNombreCompletoUsuario(gerente.usuario),
                    firmaGerente = gerente.firma,

                    nombreCoordinador = GlobalUtils.ObtenerNombreCompletoUsuario(coordinador.usuario),
                    firmaCoordinador = coordinador.firma,

                    nombreSecretario = GlobalUtils.ObtenerNombreCompletoUsuario(secretario.usuario),
                    firmaSecretario = secretario.firma,

                    referenciaNormativa = controlAsistencia.curso.referenciasNormativas,

                    asistentes = controlAsistencia.asistentes.Where(x =>
                        x.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica &&
                        x.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado).Select(x => new AsistenteCapacitacionDTO
                        {
                            claveEmpleado = x.claveEmpleado.ToString(),
                            nombreEmpleado = ObtenerNombreEmpleadoPorClave(x.claveEmpleado),
                            puesto = x.puesto,
                            departamento = ObtenerDepartamentoCCPorClaveEmpleado(x.claveEmpleado) != null ? ObtenerDepartamentoCCPorClaveEmpleado(x.claveEmpleado).Item1 : ""
                        }).ToList()
                };

                var listaRelacionCCDepartamentoRazonSocial = _context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Where(x => x.estatus && x.division == divisionActual).ToList();
                var listaRazonSocial = _context.tblS_CapacitacionSeguridadRazonSocial.Where(x => x.estatus).ToList();

                foreach (var asis in controlAsistenciaDTO.asistentes)
                {
                    var relacionCCDepartamentoRazonSocial = listaRelacionCCDepartamentoRazonSocial.FirstOrDefault(x => x.estatus && x.cc == asis.cc && x.departamento == asis.departamento);

                    if (relacionCCDepartamentoRazonSocial != null)
                    {
                        asis.razonSocial = relacionCCDepartamentoRazonSocial.razonSocial.razonSocial;
                        asis.rfc = relacionCCDepartamentoRazonSocial.razonSocial.rfc;
                    }
                    else
                    {
                        asis.razonSocial = "";
                        asis.rfc = "";
                    }
                }

                return controlAsistenciaDTO;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerDatosFormatoAutorizacion", e, AccionEnum.CONSULTA, controlAsistenciaID, null);
                return null;
            }
        }

        public Dictionary<string, object> AutorizarControlAsistencia(int controlAsistenciaID)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion
                        .Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual)
                        .OrderBy(x => x.orden)
                        .ToList();

                    bool esAdmin = false;

                    if (autorizantes.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró el control de asistencia.");
                        return resultado;
                    }

                    var autorizanteEnTurno = autorizantes.FirstOrDefault(x => x.estatus == (int)EstatusAutorizacionCapacitacion.Pendiente);

                    if (autorizanteEnTurno == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No hay usuarios pendientes por autorizar.");
                        return resultado;
                    }
                    else if (autorizanteEnTurno.usuarioID != vSesiones.sesionUsuarioDTO.id || esAdmin)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El usuario no está en turno de autorizar.");
                        return resultado;
                    }
                    else
                    {
                        // Mientras el usuario sea el que sigue de autorizar, va recorriendo la lista de autorizantes.

                        int orden = autorizanteEnTurno.orden - 1;

                        while (orden < autorizantes.Count)
                        {
                            if (autorizantes[orden].usuarioID == vSesiones.sesionUsuarioDTO.id)
                            {
                                autorizantes[orden].estatus = (int)EstatusAutorizacionCapacitacion.Autorizado;

                                autorizantes[orden].firma =
                                    GlobalUtils.CrearFirmaDigital(controlAsistenciaID, DocumentosEnum.FormatoAutorizacionCapacitacion, autorizanteEnTurno.usuarioID);

                                autorizantes[orden].fecha = DateTime.Now;
                            }
                            else
                            {
                                break;
                            }

                            orden++;
                        }
                    }
                    _context.SaveChanges();

                    // Se desactiva la alerta para el usuario actual.
                    DesactivarAlertaAutorizacion(autorizanteEnTurno.controlAsistenciaID, new List<int> { autorizanteEnTurno.usuarioID });

                    // Actualiza el estatus del control de asistencia en caso de que ya todos hayan autorizado
                    if (autorizantes.All(x => x.estatus == (int)EstatusAutorizacionCapacitacion.Autorizado))
                    {
                        autorizanteEnTurno.controlAsistencia.estatus = (int)EstatusControlAsistenciaEnum.Completa;

                        // Se actualiza el estatus de autorización de los asistentes que aplicaban para autorización.
                        autorizanteEnTurno.controlAsistencia.asistentes
                            .Where(x => x.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica)
                            .ToList()
                            .ForEach(x => x.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado);

                        resultado.Add("autCompleta", true);
                    }
                    else
                    {
                        if (!vSesiones.sesionCapacitacionOperativa)
                        {
                            // Si aún faltan usuarios por autorizar, se crea una alerta para el siguiente usuario.
                            var mensaje = String.Format("Autorizar Capacitación: {0}", autorizanteEnTurno.controlAsistencia.curso.claveCurso);
                            var usuarioRecibe = autorizantes.FirstOrDefault(x => x.estatus == (int)EstatusAutorizacionCapacitacion.Pendiente);
                            CrearAlertaAutorizacion(mensaje, new List<int> { usuarioRecibe.usuarioID }, autorizanteEnTurno.controlAsistenciaID);

                            resultado.Add("autCompleta", false);
                        }
                    }
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "AutorizarControlAsistencia", e, AccionEnum.ACTUALIZAR, 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar autorizar el control de asistencia.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> RechazarControlAsistencia(int controlAsistenciaID, string comentario)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion
                        .Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual)
                        .OrderBy(x => x.orden)
                        .ToList();

                    bool esAdmin = false; // Pendiente

                    if (autorizantes.Count == 0)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No se encontró el control de asistencia.");
                        return resultado;
                    }
                    else if (String.IsNullOrEmpty(comentario))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El comentario de rechazo viene vacío.");
                        return resultado;
                    }
                    comentario = comentario.Trim();

                    var autorizanteEnTurno = autorizantes.FirstOrDefault(x => x.estatus == (int)EstatusAutorizacionCapacitacion.Pendiente);

                    if (autorizanteEnTurno == null)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "No hay usuarios pendientes por autorizar.");
                        return resultado;
                    }
                    else if (autorizanteEnTurno.usuarioID != vSesiones.sesionUsuarioDTO.id || esAdmin)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "El usuario no está en turno de autorizar.");
                        return resultado;
                    }
                    else
                    {
                        autorizanteEnTurno.firma =
                                    GlobalUtils.CrearFirmaDigital(controlAsistenciaID, DocumentosEnum.FormatoAutorizacionCapacitacion, autorizanteEnTurno.usuarioID, TipoFirmaEnum.Rechazo);

                        autorizanteEnTurno.fecha = DateTime.Now;
                    }

                    autorizantes.ForEach(x => x.estatus = (int)EstatusAutorizacionCapacitacion.Rechazado);
                    _context.SaveChanges();

                    // Actualiza el estatus del control de asistencia a completa.
                    var controlAsistencia = autorizanteEnTurno.controlAsistencia;
                    controlAsistencia.estatus = (int)EstatusControlAsistenciaEnum.Completa;
                    controlAsistencia.comentario = comentario;

                    // Se actualiza el estatus de autorización de los asistentes que aplicaban para autorización.
                    controlAsistencia.asistentes
                        .Where(x => x.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica)
                        .ToList()
                        .ForEach(x => x.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Rechazado);

                    // Se desactiva la alerta del usuario rechazando
                    DesactivarAlertaAutorizacion(autorizanteEnTurno.controlAsistenciaID, new List<int> { autorizanteEnTurno.usuarioID });

                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "RechazarControlAsistencia", e, AccionEnum.ACTUALIZAR, 0, 0);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al intentar rechazar el control de asistencia.");
                }
                return resultado;
            }
        }

        public Dictionary<string, object> EnviarCorreoRechazo(int controlAsistenciaID, List<Byte[]> pdf)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();

                var controlAsistencia = autorizantes.FirstOrDefault().controlAsistencia;

                string lugarCurso = String.Format(@"{0} - {1}", ObtenerDescripcionCC(controlAsistencia.cc).Trim(), controlAsistencia.lugar);

                string nombreCurso = String.Format(@"[{0}] {1}", controlAsistencia.curso.claveCurso, controlAsistencia.curso.nombre);

                var usuarioRechaza = autorizantes.OrderByDescending(x => x.fecha).FirstOrDefault().usuario;
                string nombreUsuarioRechaza = GlobalUtils.ObtenerNombreCompletoUsuario(usuarioRechaza);

                var listaCorreos = autorizantes.Select(x => x.usuario.correo).ToList();

                string cuerpoCorreo = @"<html>
                                        <head>
                                            <style>
                                                p {
                                                    font-family: arial, sans-serif;
                                                }
                                            </style>
                                        </head>
                                        <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <div class=WordSection1>
                                                <p class=MsoNormal>
                                                    Se <strong>rechazó</strong> la capacitación del curso " + nombreCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Lugar del curso: " + lugarCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Usuario que rechazó: " + nombreUsuarioRechaza + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Razón: " + controlAsistencia.comentario + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";

                var correoEnviado = GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Capacitación Rechazada"), cuerpoCorreo, listaCorreos, pdf, "Formato Autorización");
                resultado.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "EnviarCorreoRechazo", e, AccionEnum.CORREO, controlAsistenciaID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoAutorizacion(int controlAsistenciaID, List<Byte[]> pdf)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();

                var controlAsistencia = autorizantes.FirstOrDefault().controlAsistencia;

                string lugarCurso = String.Format(@"{0} - {1}", ObtenerDescripcionCC(controlAsistencia.cc).Trim(), controlAsistencia.lugar);

                string nombreCurso = String.Format(@"[{0}] {1}", controlAsistencia.curso.claveCurso, controlAsistencia.curso.nombre);

                var instructor = controlAsistencia.instructor;
                string nombreInstructor = controlAsistencia.esExterno ? controlAsistencia.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(instructor);

                var correoUsuario = autorizantes
                    .Where(x => x.estatus == (int)EstatusAutorizacionCapacitacion.Pendiente)
                    .OrderBy(x => x.orden)
                    .Select(x => x.usuario.correo)
                    .FirstOrDefault();

                string cuerpoCorreo = @"<html>
                                        <head>
                                            <style>
                                                p {
                                                    font-family: arial, sans-serif;
                                                }
                                            </style>
                                        </head>
                                        <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <div class=WordSection1>
                                                <p class=MsoNormal>
                                                    Se ha iniciado un proceso de autorización eletrónica para la capacitación " + nombreCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Lugar del curso: " + lugarCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Instructor: " + nombreInstructor + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Favor de entrar a SIGOPLAN y verificar la notificación correspondiente para continuar con el proceso.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Favor de consultar el formato adjunto para más información.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";

                var correoEnviado =
                    GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Autorizar Capacitación"), cuerpoCorreo, new List<string> { correoUsuario }, pdf, "Formato Autorización");
                resultado.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "EnviarCorreoAutorizacion", e, AccionEnum.CORREO, controlAsistenciaID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> EnviarCorreoAutorizacionCompleta(int controlAsistenciaID, List<Byte[]> pdf)
        {
            try
            {
                var autorizantes = _context.tblS_CapacitacionSeguridadAutorizacion.Where(x => x.controlAsistenciaID == controlAsistenciaID && x.division == divisionActual).ToList();

                var controlAsistencia = autorizantes.FirstOrDefault().controlAsistencia;

                var rutaArchivo = ObtenerRutaFormatoAutorizacion(controlAsistencia);

                // Se actualizan campos de la entidad control de asistencia
                controlAsistencia.rutaListaAutorizacion = rutaArchivo;

                // Guarda el formato de autorización completo en el servidor
                if (GlobalUtils.SaveArchivoByteArray(pdf.FirstOrDefault(), rutaArchivo) == false)
                {
                    resultado.Add(SUCCESS, false);
                    return resultado;
                }

                _context.SaveChanges();

                string lugarCurso = String.Format(@"{0} - {1}", ObtenerDescripcionCC(controlAsistencia.cc).Trim(), controlAsistencia.lugar);

                string nombreCurso = String.Format(@"[{0}] {1}", controlAsistencia.curso.claveCurso, controlAsistencia.curso.nombre);

                var instructor = controlAsistencia.instructor;
                string nombreInstructor = controlAsistencia.esExterno ? controlAsistencia.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(instructor);

                var listaCorreos = autorizantes.Select(x => x.usuario.correo).ToList();

                string cuerpoCorreo = @"<html>
                                        <head>
                                            <style>
                                                p {
                                                    font-family: arial, sans-serif;
                                                }
                                            </style>
                                        </head>
                                        <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                            <div class=WordSection1>
                                                <p class=MsoNormal>
                                                    Se <strong>completó</strong> el proceso de autorización eletrónica de la capacitación del curso " + nombreCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Lugar del curso: " + lugarCurso + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Instructor: " + nombreInstructor + @".<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Favor de consultar el formato adjunto para más información.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    PD. Se informa que esta notificación es autogenerada por el sistema SIGOPLAN y no es necesario dar una respuesta.<o:p></o:p>
                                                </p>
                                                <p class=MsoNormal>
                                                    Gracias.<o:p></o:p>
                                                </p>
                                            </div>
                                        </body>
                                    </html>";

                var correoEnviado = GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Capacitación Completada"), cuerpoCorreo, listaCorreos, pdf, "Formato Autorización");
                resultado.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "EnviarCorreoAutorizacionCompleta", e, AccionEnum.CORREO, controlAsistenciaID, null);
            }
            return resultado;
        }

        public Dictionary<string, object> guardarCargaMasiva(HttpPostedFileBase archivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<List<string>> tabla = new List<List<string>>();

                    #region Convertir Archivo a Arreglo de bytes.
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
                    #endregion

                    #region Leer Arreglo de bytes.
                    using (MemoryStream stream = new MemoryStream(data))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        //loop all worksheets
                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            //loop all rows
                            for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                            {
                                List<string> fila = new List<string>();

                                //loop all columns in a row
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                {
                                    //add the cell data to the List
                                    if (worksheet.Cells[i, j].Value != null)
                                    {
                                        fila.Add(worksheet.Cells[i, j].Value.ToString());
                                    }
                                    else
                                    {
                                        fila.Add("");
                                    }
                                }

                                if (i > 1 && fila[0] != "")
                                {
                                    tabla.Add(fila);
                                }
                            }
                        }
                    }
                    #endregion

                    foreach (var fila in tabla)
                    {
                        //var requerimiento = fila[0];
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
        #endregion

        #region Matriz de empleados
        public Dictionary<string, object> ObtenerComboCCEnKontrol(EmpresaEnum empresa)
        {
            try
            {
                var listaCCDivision = _context.tblS_CapacitacionSeguridadCentroCostoDivision.Where(x => x.estatus && x.division == divisionActual).ToList();
                var ccs = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2 || vSesiones.sesionEmpresaActual == 3)
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;
                    string query = @"
                    SELECT cc as Value, (cc + ' - ' + descripcion) as Text 
                    FROM DBA.cc 
                    ORDER BY cc
                    ";

                    var odbc = new OdbcConsultaDTO() { consulta = query };

                    //var ccs = new List<Core.DTO.Principal.Generales.ComboDTO>();

                    switch (empresa)
                    {
                        case EmpresaEnum.Construplan:
                            ccs = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanProd, odbc);
                            break;
                        case EmpresaEnum.Arrendadora:
                            ccs = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, odbc);
                            break;
                        case EmpresaEnum.Colombia:
                            ccs = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ColombiaProductivo, odbc);
                            break;

                        //throw new NotImplementedException("No hay base de datos de EnKontrol configurada para la empresa Colombia.");
                        default:
                            throw new NotImplementedException("Empresa no definida.");
                    }
                    if (ccs.Count > 0)
                    {
                        if (vSesiones.sesionCapacitacionOperativa)
                        {
                            //Filtrar por división
                            ccs = ccs.Where(x => listaCCDivision.Where(z => z.empresa == 0 || z.empresa == (int)empresa).Select(y => y.cc).Contains(x.Value)).ToList();
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add("items", ccs);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("EMPTY", true);
                    }
                }else if (vSesiones.sesionEmpresaActual == 6)
                {
                    var listaCCPeru = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.ccDescripcion
                    }).ToList();
                    if (listaCCPeru.Count > 0)
                    {
                        if (vSesiones.sesionCapacitacionOperativa)
                        {
                            //Filtrar por división
                            listaCCPeru = ccs.Where(x => listaCCDivision.Where(z => z.empresa == 6).Select(y => y.cc).Contains(x.Value)).ToList();
                        }

                        resultado.Add(SUCCESS, true);
                        resultado.Add("items", listaCCPeru);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("EMPTY", true);
                    }
                }
                       
                      

            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(Error, "Ocurrió un error interno al intentar obtener el catálogo de áreas");
            }

            return resultado;
        }

        public List<EmpleadoPuestoDTO> ObtenerEmpleados(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                bool existeCCsCplan = ccsCplan != null && ccsCplan.Count > 0;
                bool existeCCsArr = ccsArr != null && ccsArr.Count > 0;

                if (existeCCsCplan == false && existeCCsArr == false)
                {
                    return null;
                }

                var listaEmpleados = new List<EmpleadoPuestoDTO>();
                var listaEmpleadosCP = new List<EmpleadoPuestoDTO>();
                var listaEmpleadosARR = new List<EmpleadoPuestoDTO>();

//                string queryBase =
//                @"
//                SELECT 
//                    e.clave_empleado AS claveEmpleado, 
//                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
//                    e.fecha_alta AS fechaAlta, 
//                    p.descripcion AS puestoEmpleado,
//                    e.puesto as puestoID,
//                    e.cc_contable AS ccID,
//                    (c.cc + ' - ' + c.descripcion) AS cc
//                FROM DBA.sn_empleados AS e
//                    INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                    INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
//                WHERE e.estatus_empleado ='A' AND e.cc_contable IN {0} " + (puestos != null ? @" AND e.puesto IN {1} " : @"") +
//                @" ORDER BY c.cc, nombreEmpleado
//                ";

                string queryBase = @"
                                    SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                                        e.fecha_alta AS fechaAlta, 
                                        p.descripcion AS puestoEmpleado,
                                        e.puesto as puestoID,
                                        e.cc_contable AS ccID,
                                        (c.cc + ' - ' + c.descripcion) AS cc
                                    FROM tblRH_EK_Empleados AS e
                                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                                        INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                                    WHERE e.estatus_empleado ='A' AND e.cc_contable IN ('{0}') " + (puestos != null ? @" AND e.puesto IN ('{1}') " : @"") +
                                    @" ORDER BY c.cc, nombreEmpleado";
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {


                    if (existeCCsCplan)
                    {
                        string queryCplan = "";
                        if (puestos != null)
                        {
                            //queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue(), puestos.ToParamInValue());
                            queryCplan = String.Format(queryBase, String.Join("','", ccsCplan), String.Join("','", puestos));
                        }
                        else
                        {
                            //queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue());
                            queryCplan = String.Format(queryBase, String.Join("','", ccsCplan));
                        }

                        //var odbcCplan = new OdbcConsultaDTO() { consulta = queryCplan };

                        //odbcCplan.parametros.AddRange(
                        //    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //if (puestos != null)
                        //{
                        //    odbcCplan.parametros.AddRange(
                        //        puestos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                        //    );
                        //}

                        //listaEmpleadosCP = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolEnum.CplanRh, odbcCplan);

                        listaEmpleadosCP = _context.Select<EmpleadoPuestoDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = queryCplan,
                        });

                        listaEmpleadosCP.AddRange(_context.Select<EmpleadoPuestoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = queryCplan,
                        }));

                        listaEmpleadosCP.ForEach(x => x.empresa = 1);
                        listaEmpleados.AddRange(listaEmpleadosCP);
                    }

                    if (existeCCsArr)
                    {
                        string queryArr = "";
                        if (puestos != null)
                        {
                            //queryArr = String.Format(queryBase, ccsArr.ToParamInValue(), puestos.ToParamInValue());
                            queryArr = String.Format(queryBase, String.Join("','", ccsArr), String.Join("','", puestos));
                        }
                        else
                        {
                            //queryArr = String.Format(queryBase, ccsArr.ToParamInValue());
                            queryArr = String.Format(queryBase, String.Join("','", ccsArr));
                        }

                        //var odbcArr = new OdbcConsultaDTO() { consulta = queryArr };

                        //odbcArr.parametros.AddRange(
                        //    ccsArr.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //if (puestos != null)
                        //{
                        //    odbcArr.parametros.AddRange(
                        //        puestos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                        //    );
                        //}

                        //listaEmpleadosARR = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolEnum.ArrenRh, odbcArr);

                        listaEmpleadosARR = _context.Select<EmpleadoPuestoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = queryArr,
                        });

                        listaEmpleadosARR.ForEach(x => x.empresa = 2);
                        listaEmpleados.AddRange(listaEmpleadosARR);

                    }
                }else if (vSesiones.sesionEmpresaActual == 6)
                {
                     queryBase = @"
                                    SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                                        e.fecha_alta AS fechaAlta, 
                                        p.descripcion AS puestoEmpleado,
                                        e.puesto as puestoID,
                                        e.cc_contable AS ccID,
                                        (c.cc + ' - ' + c.descripcion) AS cc
                                    FROM tblRH_EK_Empleados AS e
                                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                                        INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                                    WHERE e.estatus_empleado ='A' AND e.cc_contable IN ('{0}') " + (puestos != null ? @" AND e.puesto IN ('{1}') " : @"") +
                                   @" ORDER BY c.cc, nombreEmpleado";

                    string queryPeru = "";
                    queryPeru = String.Format(queryBase, String.Join("','", ccsCplan));

                    listaEmpleadosCP = _context.Select<EmpleadoPuestoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.PERU,
                        consulta = queryPeru,
                    });
                    listaEmpleados.AddRange(listaEmpleadosCP);

                }
             
                return listaEmpleados;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "ObtenerEmpleados", e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }

        public Dictionary<string, object> ObtenerCursosEmpleado(int claveEmpleado, int puestoID)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var cursos = ObtenerCursosVigentesEmpleado(claveEmpleado);

                string porcentajeGeneral = "";
                var listaPorcentajes = ObtenerListaPorcentajeCapacitacion(claveEmpleado, puestoID, out porcentajeGeneral);

                bool tieneExpediente = EmpleadoTieneExpediente(claveEmpleado);

                resultado.Add(ITEMS, cursos);
                resultado.Add("listaPorcentajes", listaPorcentajes);
                resultado.Add("porcentajeGeneral", porcentajeGeneral);
                resultado.Add("tieneExpediente", tieneExpediente);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerCursosEmpleado", e, AccionEnum.CONSULTA, claveEmpleado, null);
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarExpedienteEmpleado(int claveEmpleado, byte[] licencia)
        {
            string rutaFolderTemp = "";
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var capacitaciones = _context.tblS_CapacitacionSeguridadControlAsistencia
                    .Where(x => x.asistentes.Any(y => y.claveEmpleado == claveEmpleado)).ToList().Where(x =>
                        x.estatus != (int)EstatusControlAsistenciaEnum.PendienteCargaAsistencia && x.division == divisionActual)
                    .ToList();

                var extracurriculares = _context.tblS_CapacitacionSeguridadExtracurricular
                    .Where(x =>
                        x.claveEmpleado == claveEmpleado &&
                        x.esActivo &&
                        x.rutaEvidencia != null && x.division == divisionActual)
                    .ToList();

                var listaCarpetas = CrearCarpetasExpediente(claveEmpleado, capacitaciones);

                var nombreFolderTemp = String.Format("{0} {1}", "tmp", ObtenerFormatoCarpetaFechaActual());
                rutaFolderTemp = Path.Combine(RutaTemp, nombreFolderTemp);

                Directory.CreateDirectory(rutaFolderTemp);

                // Se copian los archivos al folder temporal
                CopiarArchivosCarpetasExpediente(rutaFolderTemp, listaCarpetas);

                // Si el empleado tiene archivos extracurriculares crea el folder de extracurriculares en caso de aplicar de la licencia
                if (extracurriculares.Count > 0)
                {
                    var rutaCarpetaExtracurricular = Path.Combine(rutaFolderTemp, "Extracurriculares");
                    Directory.CreateDirectory(rutaCarpetaExtracurricular);
                    CopiarArchivosExtracurricularesCarpetaExpediente(rutaCarpetaExtracurricular, extracurriculares);
                }


                // Si el empleado tiene actos seguros o inseguros, se crea el folder de actos.
                var actosEmpleado = _context.tblSAC_Acto.Where(x => x.claveEmpleado == claveEmpleado && x.activo && x.rutaEvidencia != null).ToList();

                if (actosEmpleado.Count > 0)
                {
                    var rutaCarpetaActos = Path.Combine(rutaFolderTemp, "Actos");
                    Directory.CreateDirectory(rutaCarpetaActos);
                    CopiarArchivosActosCarpetaExpediente(rutaCarpetaActos, actosEmpleado);
                }

                // Se crea el archivo de la licencia
                var rutaLicencia = Path.Combine(rutaFolderTemp, "licencia.pdf");
                File.WriteAllBytes(rutaLicencia, licencia);

                // Ya que esta la carpeta temporal creada, se crea el zip
                string rutaNuevoZip = Path.Combine(RutaTemp, nombreFolderTemp + ".zip");
                GlobalUtils.ComprimirCarpeta(rutaFolderTemp, rutaNuevoZip);

                // Una vez creado el zip, se elimina el folder temporal 
                // y se obtiene el stream de bytes del zip
                Directory.Delete(rutaFolderTemp, true);
                var zipStream = GlobalUtils.GetFileAsStream(rutaNuevoZip);

                // Una vez cargado el stream, se elimina el zip
                File.Delete(rutaNuevoZip);

                string nombreEmpleado = ObtenerNombreEmpleadoPorClave(claveEmpleado);
                string nombreZip = String.Format("Expediente - {0} {1}.zip", nombreEmpleado, claveEmpleado);

                return Tuple.Create(zipStream, nombreZip);
            }
            catch (Exception e)
            {
                try { Directory.Delete(rutaFolderTemp); }
                catch (Exception) { }

                LogError(0, 0, NombreControlador, "DescargarExpedienteEmpleado", e, AccionEnum.DESCARGAR, claveEmpleado, 0);
                return null;
            }
        }

        public LicenciaHabilidadesDTO ObtenerLicenciaEmpleado(int claveEmpleado, int empresa)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;
                LicenciaHabilidadesDTO licencia = ObtenerDatosEmpleadoEnKontrol(claveEmpleado, empresa);
                licencia.cursos = ObtenerCursosLicenciaEmpleado(claveEmpleado);
                return licencia;
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerLicenciaEmpleado", e, AccionEnum.CONSULTA, claveEmpleado, null);
                return null;
            }
        }

        public Dictionary<string, object> ObtenerExtracurricularesEmpleado(int claveEmpleado)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var extracurriculares = _context.tblS_CapacitacionSeguridadExtracurricular
                    .Where(x => x.claveEmpleado == claveEmpleado && x.esActivo && x.division == divisionActual)
                    .ToList()
                    .Select(x => new
                    {
                        x.id,
                        x.nombre,
                        x.duracion,
                        fecha = x.fecha.ToShortDateString(),
                        fechaExpiracion = x.fechaExpiracion.HasValue ? x.fechaExpiracion.Value.ToShortDateString() : "N/A",
                        tieneEvidencia = x.rutaEvidencia != null
                    });

                resultado.Add(ITEMS, extracurriculares);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerExtracurricularesEmpleado", e, AccionEnum.CONSULTA, claveEmpleado, null);
            }
            return resultado;
        }

        public Dictionary<string, object> SubirEvidenciaExtracurricular(int claveEmpleado, string nombre, decimal duracion, string fecha, string fechaFin, HttpPostedFileBase evidencia)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime fechaInicio;
                    var exitoFechaInicio = DateTime.TryParse(fecha, out fechaInicio);

                    string rutaEvidencia = "";

                    // Se validan los datos
                    if (claveEmpleado <= 0 || String.IsNullOrEmpty(nombre) || duracion <= 0 || !exitoFechaInicio)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Información por guardar inválida.");
                        return resultado;
                    }

                    // Se valida que no exista otra información extracurricular con el mismo nombre.
                    var yaExiste = _context.tblS_CapacitacionSeguridadExtracurricular.Any(x => x.claveEmpleado == claveEmpleado && x.nombre.ToUpper() == nombre.Trim().ToUpper() && x.esActivo && x.division == divisionActual);

                    if (yaExiste)
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "Ya existe un logro extracurricular con ese nombre.");
                        return resultado;
                    }

                    var nuevoExtracurricular = new tblS_CapacitacionSeguridadExtracurricular
                    {
                        claveEmpleado = claveEmpleado,
                        nombre = nombre.Trim(),
                        duracion = duracion,
                        fecha = fechaInicio,
                        esActivo = true,
                        division = divisionActual
                    };

                    if (fechaFin != "")
                    {
                        nuevoExtracurricular.fechaExpiracion = DateTime.Parse(fechaFin);
                    }

                    if (evidencia != null)
                    {

                        var nombreEmpleado = ObtenerNombreEmpleadoPorClave(claveEmpleado);

                        string nombreCarpetaExpedienteEmpleado = ObtenerFormatoCarpetaExpediente(claveEmpleado, nombreEmpleado);

                        var rutaCarpetaExpedienteEmpleado = Path.Combine(RutaExpedientes, nombreCarpetaExpedienteEmpleado);

                        // Verifica si existe la carpeta y si no, la crea.
                        if (verificarExisteCarpeta(rutaCarpetaExpedienteEmpleado, true) == false)
                        {
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo crear la carpeta en el servidor.");
                            return resultado;
                        }

                        var nombreArchivo = String.Format("{0} {1}{2}",
                            NombreBaseEvidencia,
                            ObtenerFormatoCarpetaFechaActual(),
                            Path.GetExtension(evidencia.FileName));

                        var rutaArchivo = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreArchivo);

                        // Se agrega la ruta física de la evidencia.
                        nuevoExtracurricular.rutaEvidencia = rutaArchivo;
                        rutaEvidencia = rutaArchivo;
                    }

                    _context.tblS_CapacitacionSeguridadExtracurricular.Add(nuevoExtracurricular);
                    _context.SaveChanges();
                    resultado.Add(SUCCESS, true);

                    // Si creó el archivo.
                    if (evidencia != null)
                    {
                        if (SaveArchivo(evidencia, rutaEvidencia))
                        {
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            // Si falla al guardar el archivo
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "No se pudo guardar el archivo en el servidor.");
                        }
                    }
                    else
                    {
                        dbContextTransaction.Commit();
                    }

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "SubirEvidenciaExtracurricular", e, AccionEnum.AGREGAR, claveEmpleado, null);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error interno al intentar guardar la información extracurricular en el servidor.");
                }
            }

            return resultado;
        }

        public Tuple<Stream, string> DescargarEvidenciaExtracurricular(int extracurricularID)
        {
            try
            {
                var extracurricular = _context.tblS_CapacitacionSeguridadExtracurricular.FirstOrDefault(x => x.id == extracurricularID && x.division == divisionActual);

                if (extracurricular == null)
                {
                    return null;
                }

                var fileStream = GlobalUtils.GetFileAsStream(extracurricular.rutaEvidencia);
                string name = Path.GetFileName(extracurricular.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarEvidenciaExtracurricular", e, AccionEnum.DESCARGAR, extracurricularID, 0);
                return null;
            }
        }

        public Dictionary<string, object> EliminarEvidenciaExtracurricular(int extracurricularID)
        {
            try
            {
                var extracurricular = _context.tblS_CapacitacionSeguridadExtracurricular.FirstOrDefault(x => x.id == extracurricularID && x.division == divisionActual);

                if (extracurricular == null)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontró el registro por eliminar.");
                    return resultado;
                }

                extracurricular.esActivo = false;
                resultado.Add(SUCCESS, true);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al interntar eliminar el registro en el servidor.");
                return resultado;
            }
            return resultado;
        }
        #endregion

        #region Dashboard

        public Dictionary<string, object> CargarDatosGeneralesDashboard(List<string> ccsCplan, List<string> ccsArr, DateTime fechaInicio, DateTime fechaFin, List<string> clasificaciones)
        {
            var listaCCs = new List<string>();
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                if ((ccsCplan == null || ccsCplan.Count == 0) && (ccsArr == null || ccsArr.Count == 0))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La lista de centros de costos viene vacía.");
                    return resultado;
                }

                if (ccsCplan != null)
                {
                    listaCCs.AddRange(ccsCplan);
                }
                if (ccsArr != null)
                {
                    listaCCs.AddRange(ccsArr);
                }

                List<ClasificacionCursoEnum> listaClasificacionesEnum = new List<ClasificacionCursoEnum>();

                if (clasificaciones != null)
                {
                    foreach (var cla in clasificaciones)
                    {
                        listaClasificacionesEnum.Add(Enum.GetValues(typeof(ClasificacionCursoEnum)).Cast<ClasificacionCursoEnum>().FirstOrDefault(v => v.GetDescription() == cla));
                    }
                }

                var capacitacionesPorCC = _context.tblS_CapacitacionSeguridadControlAsistencia.ToList().Where(c =>
                    c.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                    listaCCs.Contains(c.cc) &&
                    (c.fechaCapacitacion >= fechaInicio && c.fechaCapacitacion <= fechaFin) &&
                    (clasificaciones != null ? listaClasificacionesEnum.Contains((ClasificacionCursoEnum)c.curso.clasificacion) : true) && c.division == divisionActual
                ).ToList();

                // HHC = Horas Hombre Capacitación
                var totalHHC = ObtenerTotalHorasHombreCapacitacion(capacitacionesPorCC);

                var empleados = ObtenerEmpleadosCC(ccsCplan, ccsArr, null);

                var detalleCurso = cargarTblEmpleadosExpirar(capacitacionesPorCC, empleados);

                var expirados = detalleCurso.Where(x => x.fechaExpiracion <= DateTime.Today.AddMonths(1));

                var porcentajeCursosVigentes = ObtenerPorcentajeCursosVigentes(detalleCurso, listaCCs, empleados);

                var cursosImpartidos = ObtenerCursosImpartidos(capacitacionesPorCC, fechaInicio, fechaFin)
                    .Where(x => clasificaciones != null ? clasificaciones.Contains(x.Item2) : true)
                    .ToList();

                var HHClasificacion = ObtenerHH(capacitacionesPorCC, fechaInicio, fechaFin);

                HHClasificacion = HHClasificacion.Where(x => clasificaciones != null ? clasificaciones.Contains(x.Item2) : true).ToList();

                var totalCursos = capacitacionesPorCC.Count;

                var totalPersonalCapacitado = ObtenerTotalPersonalCapacitado(capacitacionesPorCC);

                var datos = new List<Tuple<string, decimal>>();

                datos.Add(Tuple.Create("totalHHC", totalHHC));
                datos.Add(Tuple.Create("totalPersonalCapacitado", (decimal)totalPersonalCapacitado));
                datos.Add(Tuple.Create("totalCursos", (decimal)totalCursos));

                resultado.Add("datos", datos);

                resultado.Add("expProtocoloFatalidad", expirados.Where(x => x.clasificacion == ClasificacionCursoEnum.ProtocoloFatalidad));
                resultado.Add("expProcedimientosOperativos", expirados.Where(x => x.clasificacion == ClasificacionCursoEnum.TecnicoOperativo));
                resultado.Add("expInstructivoOperativos", expirados.Where(x => x.clasificacion == ClasificacionCursoEnum.InstructivoOperativo));
                resultado.Add("porcentajeVigentes", porcentajeCursosVigentes);
                resultado.Add("cursosImpartidos", cursosImpartidos);
                resultado.Add("HHClasificacion", HHClasificacion);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "CargarDatosGeneralesDashboard", e, AccionEnum.CONSULTA, 0, null);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información para el dashboard.");
            }

            return resultado;
        }

        private static decimal ObtenerTotalHorasHombreCapacitacion(List<tblS_CapacitacionSeguridadControlAsistencia> listaCapacitaciones)
        {
            if (listaCapacitaciones == null || listaCapacitaciones.Count == 0)
            {
                return 0;
            }
            return listaCapacitaciones.Sum(x => x.asistentes.Count * x.curso.duracion);
        }

        private static double ObtenerTotalPersonalCapacitado(List<tblS_CapacitacionSeguridadControlAsistencia> listaCapacitaciones)
        {
            if (listaCapacitaciones == null || listaCapacitaciones.Count == 0)
            {
                return 0;
            }

            return listaCapacitaciones.Sum(x =>
            {
                return x.asistentes.Where(y =>
                    y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado ||
                    (y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica && y.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado))
                .Count();
            });
        }

        private List<EmpleadoExpirarDTO> cargarTblEmpleadosExpirar(List<tblS_CapacitacionSeguridadControlAsistencia> listaCapacitaciones, List<EmpleadoCapacitacionDTO> empleados)
        {
            var data = new List<EmpleadoExpirarDTO>();

            var auxDetalle = new List<tblS_CapacitacionSeguridadControlAsistenciaDetalle>();

            if (listaCapacitaciones == null || listaCapacitaciones.Count == 0)
            {
                return data;
            }

            var puestosID = empleados.Select(x => x.puestoID).Distinct();

            var cursosAplicablesPorPuesto = _context.tblS_CapacitacionSeguridadCursosPuestos
                .Where(x => x.estatus && puestosID.Contains(x.puesto_id) && x.division == divisionActual)
                .Select(x => x.curso_id).Distinct().ToList();

            var cursosAplicablesEmpleado = _context.tblS_CapacitacionSeguridadCursos.ToList()
                .Where(x =>
                    x.estatus == (int)EstatusCursoEnum.Completo &&
                    (x.esGeneral || x.aplicaTodosPuestos || cursosAplicablesPorPuesto.Contains(x.id)) && x.division == divisionActual
                    ).ToList();

            var cursosGenerales = _context.tblS_CapacitacionSeguridadCursos.ToList()
                .Where(x => x.estatus == (int)EstatusCursoEnum.Completo && x.esGeneral && x.division == divisionActual);

            var empleadosAprobados = listaCapacitaciones.SelectMany(x =>
                 x.asistentes.Where(y =>
                    y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado ||
                    (y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica && y.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado))
            ).ToList();

            var asistentesUltimaCap = empleadosAprobados.
                GroupBy(x =>
                    new { x.claveEmpleado, x.controlAsistencia.cursoID },
                    (key, g) => g.OrderByDescending(e => e.controlAsistencia.fechaCapacitacion).ThenByDescending(e => e.id).FirstOrDefault())
                .ToList();

            foreach (var empleado in empleados)
            {
                var cursosPorEmp = cursosAplicablesEmpleado.Where(x =>
                    x.aplicaTodosPuestos ||
                    x.esGeneral ||
                    x.Puestos.Any(y => y.puesto_id == empleado.claveEmpleado));

                foreach (var curso in cursosPorEmp)
                {
                    var asistencia = asistentesUltimaCap.FirstOrDefault(x => x.claveEmpleado == empleado.claveEmpleado && x.controlAsistencia.cursoID == curso.id);

                    var agregar = new EmpleadoExpirarDTO
                    {
                        claveEmpleado = empleado.claveEmpleado,
                        nombre = empleado.nombreEmpleado,
                        puesto = empleado.puestoEmpleado,
                        puestoID = empleado.puestoID,
                        ccID = empleado.ccID,
                        cc = empleado.cc,
                        clasificacion = (ClasificacionCursoEnum)curso.clasificacion
                    };

                    if (asistencia == null)
                    {
                        agregar.curso = curso.nombre;
                        agregar.cursoID = curso.id;
                        agregar.fechaExpiracion = default(DateTime);
                        agregar.fechaExpiracionStr = "-";
                    }
                    else
                    {
                        agregar.curso = asistencia.controlAsistencia.curso.nombre;
                        agregar.cursoID = asistencia.controlAsistencia.cursoID;
                        agregar.fechaExpiracion = asistencia.controlAsistencia.fechaCapacitacion.AddYears(1);
                        agregar.fechaExpiracionStr = asistencia.controlAsistencia.fechaCapacitacion.AddYears(1).ToString("dd/MM/yyyy");
                    }

                    data.Add(agregar);
                }

                foreach (var curso in cursosGenerales)
                {
                    var asistencia = asistentesUltimaCap.FirstOrDefault(x => x.claveEmpleado == empleado.claveEmpleado && x.controlAsistencia.cursoID == curso.id);
                    var agregar = new EmpleadoExpirarDTO();
                    if (asistencia == null)
                    {
                        agregar.claveEmpleado = empleado.claveEmpleado;
                        agregar.curso = curso.nombre;
                        agregar.cursoID = curso.id;
                        agregar.fechaExpiracion = default(DateTime);
                        agregar.fechaExpiracionStr = "-";
                        agregar.nombre = empleado.nombreEmpleado;
                        agregar.puesto = empleado.puestoEmpleado;
                        agregar.puestoID = empleado.puestoID;
                        agregar.ccID = empleado.ccID;
                        agregar.cc = empleado.cc;
                        agregar.clasificacion = (ClasificacionCursoEnum)curso.clasificacion;
                    }
                    else
                    {
                        agregar.claveEmpleado = empleado.claveEmpleado;
                        agregar.curso = asistencia.controlAsistencia.curso.nombre;
                        agregar.cursoID = asistencia.controlAsistencia.cursoID;
                        agregar.fechaExpiracion = asistencia.controlAsistencia.fechaCapacitacion.AddYears(1);
                        agregar.fechaExpiracionStr = asistencia.controlAsistencia.fechaCapacitacion.AddYears(1).ToString("dd/MM/yyyy");
                        agregar.nombre = empleado.nombreEmpleado;
                        agregar.puesto = empleado.puestoEmpleado;
                        agregar.puestoID = empleado.puestoID;
                        agregar.ccID = empleado.ccID;
                        agregar.cc = empleado.cc;
                        agregar.clasificacion = (ClasificacionCursoEnum)curso.clasificacion;
                    }
                    data.Add(agregar);
                }
            }
            return data;
        }

        private List<EmpleadoCapacitacionDTO> ObtenerEmpleadosCC(List<string> ccsCplan, List<string> ccsArr, List<string> puestos)
        {
            bool existeCCsCplan = ccsCplan != null && ccsCplan.Count > 0;
            bool existeCCsArr = ccsArr != null && ccsArr.Count > 0;

            if (existeCCsCplan == false && existeCCsArr == false)
            {
                return null;
            }

            var listaEmpleados = new List<dynamic>();
            var listaEmpleadosCP = new List<dynamic>();
            var listaEmpleadosARR = new List<dynamic>();

//            string queryBase =
//            @"
//                SELECT 
//                    e.clave_empleado AS claveEmpleado, 
//                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
//                    e.fecha_alta AS fechaAlta, 
//                    p.descripcion AS puestoEmpleado,
//                    e.puesto as puestoID,
//                    e.cc_contable AS ccID,
//                    (c.cc + ' - ' + c.descripcion) AS cc
//                FROM DBA.sn_empleados AS e
//                    INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                    INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
//                WHERE e.estatus_empleado ='A' AND e.cc_contable IN {0} " + (puestos != null ? @" AND e.puesto IN {1} " : @"") +
//            @" ORDER BY c.cc, nombreEmpleado
//                ";
            string queryBase = @"SELECT 
                    e.clave_empleado AS claveEmpleado, 
                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                    e.fecha_alta AS fechaAlta, 
                    p.descripcion AS puestoEmpleado,
                    e.puesto as puestoID,
                    e.cc_contable AS ccID,
                    (c.cc + ' - ' + c.descripcion) AS cc
                FROM tblRH_EK_Empleados AS e
                    INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                    INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                WHERE e.estatus_empleado ='A' AND e.cc_contable IN {0} " + (puestos != null ? @" AND e.puesto IN {1} " : @"") +
            @" ORDER BY c.cc, nombreEmpleado";

            if (existeCCsCplan)
            {
                string queryCplan = "";
                if (puestos != null)
                {
                    queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue(), puestos.ToParamInValue());
                }
                else
                {
                    queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue());
                }

                var odbcCplan = new OdbcConsultaDTO() { consulta = queryCplan };

                odbcCplan.parametros.AddRange(
                    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                );

                if (puestos != null)
                {
                    odbcCplan.parametros.AddRange(
                        puestos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                    );
                }

                //listaEmpleadosCP = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbcCplan);

                listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = queryBase,
                });

                if (vSesiones.sesionEmpresaActual == 1)
                {
                    listaEmpleadosCP.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = queryBase,
                    }));
                }

                listaEmpleados.AddRange(listaEmpleadosCP);
            }

            if (existeCCsArr)
            {
                string queryArr = "";
                if (puestos != null)
                {
                    queryArr = String.Format(queryBase, ccsArr.ToParamInValue(), puestos.ToParamInValue());
                }
                else
                {
                    queryArr = String.Format(queryBase, ccsArr.ToParamInValue());
                }

                var odbcArr = new OdbcConsultaDTO() { consulta = queryArr };

                odbcArr.parametros.AddRange(
                    ccsArr.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                );

                if (puestos != null)
                {
                    odbcArr.parametros.AddRange(
                        puestos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                    );
                }

                //listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbcArr);

                listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = queryBase,
                });

                listaEmpleados.AddRange(listaEmpleadosARR);
            }

            var data = listaEmpleados.Select(x => new EmpleadoCapacitacionDTO
            {
                claveEmpleado = Convert.ToInt32((decimal)x.claveEmpleado),
                nombreEmpleado = x.nombreEmpleado,
                puestoEmpleado = x.puestoEmpleado,
                puestoID = Convert.ToInt32((decimal)x.puestoID),
                fechaAlta = x.fechaAlta,
                ccID = x.ccID,
                cc = x.cc
            }).ToList();
            return data;
        }

        private List<EmpleadoCapacitacionDTO> ObtenerEmpleadosCCPorDepartamento(List<string> ccsCplan, List<string> ccsArr, List<string> departamentos)
        {
            bool existeCCsCplan = ccsCplan != null && ccsCplan.Count > 0;
            bool existeCCsArr = ccsArr != null && ccsArr.Count > 0;

            if (existeCCsCplan == false && existeCCsArr == false)
            {
                return null;
            }

            var listaEmpleados = new List<dynamic>();
            var listaEmpleadosCP = new List<dynamic>();
            var listaEmpleadosARR = new List<dynamic>();

//            string queryBase =
//            @"
//                SELECT 
//                    e.clave_empleado AS claveEmpleado, 
//                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
//                    e.fecha_alta AS fechaAlta, 
//                    p.descripcion AS puestoEmpleado,
//                    e.puesto as puestoID,
//                    e.clave_depto as departamentoID,
//                    d.desc_depto as departamentoEmpleado,
//                    e.cc_contable AS ccID,
//                    (c.cc + ' - ' + c.descripcion) AS cc,
//                    e.curp
//                FROM DBA.sn_empleados AS e
//                INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                INNER JOIN DBA.sn_departamentos AS d ON e.clave_depto = d.clave_depto
//                INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
//                WHERE e.estatus_empleado ='A' AND e.cc_contable IN {0} " + (departamentos != null ? @" AND e.clave_depto IN {1} " : @"") +
//            @" ORDER BY c.cc, nombreEmpleado";


            if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
            {


                if (existeCCsCplan)
                {
                    string queryBaseCP = @"SELECT 
                                    e.clave_empleado AS claveEmpleado, 
                                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                                    e.fecha_alta AS fechaAlta, 
                                    p.descripcion AS puestoEmpleado,
                                    e.puesto as puestoID,
                                    e.clave_depto as departamentoID,
                                    d.desc_depto as departamentoEmpleado,
                                    e.cc_contable AS ccID,
                                    (c.cc + ' - ' + c.descripcion) AS cc,
                                    e.curp
                                FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                                WHERE e.estatus_empleado ='A' AND e.cc_contable IN (" + getStringInlineArray(ccsCplan) + ") " + (departamentos != null ? @" 
                                AND e.clave_depto IN (" + getStringInlineArray(departamentos) + ") " : @"") +
                                    @" ORDER BY c.cc, nombreEmpleado";

                    //string queryCplan = "";
                    //if (departamentos != null)
                    //{
                    //    queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue(), departamentos.ToParamInValue());
                    //}
                    //else
                    //{
                    //    queryCplan = String.Format(queryBase, ccsCplan.ToParamInValue());
                    //}

                    //var odbcCplan = new OdbcConsultaDTO() { consulta = queryCplan };

                    //odbcCplan.parametros.AddRange(
                    //    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                    //);

                    //if (departamentos != null)
                    //{
                    //    odbcCplan.parametros.AddRange(
                    //        departamentos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                    //    );
                    //}

                    //listaEmpleadosCP = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbcCplan);

                    listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = queryBaseCP,
                    });
                    if (vSesiones.sesionEmpresaActual == 1)
                    {
                        listaEmpleadosCP.AddRange(_context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = queryBaseCP,
                        }));
                    }

                    listaEmpleados.AddRange(listaEmpleadosCP);
                }

                if (existeCCsArr)
                {
                    string queryBaseARR = @"SELECT 
                                    e.clave_empleado AS claveEmpleado, 
                                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                                    e.fecha_alta AS fechaAlta, 
                                    p.descripcion AS puestoEmpleado,
                                    e.puesto as puestoID,
                                    e.clave_depto as departamentoID,
                                    d.desc_depto as departamentoEmpleado,
                                    e.cc_contable AS ccID,
                                    (c.cc + ' - ' + c.descripcion) AS cc,
                                    e.curp
                                FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                                WHERE e.estatus_empleado ='A' AND e.cc_contable IN (" + getStringInlineArray(ccsArr) + ") " + (departamentos != null ? @" 
                                AND e.clave_depto IN (" + getStringInlineArray(departamentos) + ") " : @"") +
                                    @" ORDER BY c.cc, nombreEmpleado";

                    //string queryArr = "";
                    //if (departamentos != null)
                    //{
                    //    queryArr = String.Format(queryBase, ccsArr.ToParamInValue(), departamentos.ToParamInValue());
                    //}
                    //else
                    //{
                    //    queryArr = String.Format(queryBase, ccsArr.ToParamInValue());
                    //}

                    //var odbcArr = new OdbcConsultaDTO() { consulta = queryArr };

                    //odbcArr.parametros.AddRange(
                    //    ccsArr.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                    //);

                    //if (departamentos != null)
                    //{
                    //    odbcArr.parametros.AddRange(
                    //        departamentos.Select(x => new OdbcParameterDTO() { nombre = "puestos", tipo = OdbcType.Int, valor = x })
                    //    );
                    //}

                    //listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbcArr);

                    listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = queryBaseARR,
                    });

                    listaEmpleados.AddRange(listaEmpleadosARR);
                }

            }else if (vSesiones.sesionEmpresaActual == 6)
            {
               var queryBase = @"SELECT 
                                    e.clave_empleado AS claveEmpleado, 
                                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                                    e.fecha_alta AS fechaAlta, 
                                    p.descripcion AS puestoEmpleado,
                                    e.puesto as puestoID,
                                    e.clave_depto as departamentoID,
                                    d.desc_depto as departamentoEmpleado,
                                    e.cc_contable AS ccID,
                                    (c.cc + ' - ' + c.descripcion) AS cc,
                                    empGral.num_dni AS curp
                                FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                                INNER JOIN tblRH_EK_Empl_Grales AS empGral ON empGral.clave_empleado = e.clave_empleado
                                WHERE e.estatus_empleado ='A' AND e.cc_contable IN (" + getStringInlineArray(ccsCplan) + ") " + (departamentos != null ? @" 
                                AND e.clave_depto IN (" + getStringInlineArray(departamentos) + ") " : @"") +
                                    @" ORDER BY c.cc, nombreEmpleado";

                string queryPeru = "";
                queryPeru = String.Format(queryBase, String.Join("','", ccsCplan));

                listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.PERU,
                    consulta = queryPeru,
                });
                listaEmpleados.AddRange(listaEmpleadosCP);

            }
            var data = listaEmpleados.Select(x => new EmpleadoCapacitacionDTO
            {
                claveEmpleado = Convert.ToInt32(x.claveEmpleado),
                curp = (string)x.curp,
                nombreEmpleado = x.nombreEmpleado,
                puestoEmpleado = x.puestoEmpleado,
                puestoID = Convert.ToInt32(x.puestoID),
                departamentoEmpleado = x.departamentoEmpleado,
                departamentoID = Convert.ToInt32(x.departamentoID),
                fechaAlta = x.fechaAlta,
                ccID = x.ccID,
                cc = x.cc
            }).ToList();
            return data;
        }

        private List<PorcentajesExpiradosDTO> ObtenerPorcentajeCursosVigentes(List<EmpleadoExpirarDTO> detalleCurso, List<string> listaCCs, List<EmpleadoCapacitacionDTO> empleados)
        {
            var puestosID = empleados.Select(x => x.puestoID).Distinct().ToList();

            var cursosIDPorPuesto = _context.tblS_CapacitacionSeguridadCursosPuestos
                .Where(x => x.estatus && puestosID.Contains(x.puesto_id) && x.division == divisionActual)
                .Select(x => x.curso_id).Distinct();

            var cursosAplicables = _context.tblS_CapacitacionSeguridadCursos.ToList().Where(x =>
                x.estatus == (int)EstatusCursoEnum.Completo &&
                (cursosIDPorPuesto.Contains(x.id) || x.aplicaTodosPuestos || x.esGeneral) && x.division == divisionActual);

            var data = new List<PorcentajesExpiradosDTO>();

            foreach (var item in cursosAplicables)
            {
                PorcentajesExpiradosDTO aux = new PorcentajesExpiradosDTO();
                aux.expirados = detalleCurso.Where(x => x.cursoID == item.id && x.fechaExpiracion < DateTime.Today.AddDays(1)).Count();
                aux.totales = detalleCurso.Where(x => x.cursoID == item.id).Count();
                aux.cursoID = item.id;
                aux.curso = item.nombre;

                if (aux.totales == 0)
                {
                    aux.porcentaje = 0;
                }
                else
                {
                    aux.porcentaje = Math.Round((((aux.totales - aux.expirados) * 100) / aux.totales), 2);
                }

                data.Add(aux);
            }

            return data.OrderByDescending(x => x.porcentaje).ToList();
        }

        private List<Tuple<int, string>> ObtenerCursosImpartidos(List<tblS_CapacitacionSeguridadControlAsistencia> listaCapacitaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            List<Tuple<int, string>> data = new List<Tuple<int, string>>();
            var clasificaciones = Enum.GetValues(typeof(ClasificacionCursoEnum)).Cast<ClasificacionCursoEnum>(); ;
            foreach (var item in clasificaciones)
            {
                var contador = listaCapacitaciones.Where(x => (ClasificacionCursoEnum)x.curso.clasificacion == item && x.fechaCapacitacion >= fechaInicio && x.fechaCapacitacion <= fechaFin).Count();
                Tuple<int, string> clasificacion = new Tuple<int, string>(contador, item.GetDescription());
                data.Add(clasificacion);
            }
            return data;
        }

        private List<Tuple<decimal, string>> ObtenerHH(List<tblS_CapacitacionSeguridadControlAsistencia> listaCapacitaciones, DateTime fechaInicio, DateTime fechaFin)
        {
            var data = new List<Tuple<decimal, string>>();

            foreach (var clasificacionEnum in EnumExtensions.ToArray<ClasificacionCursoEnum>())
            {
                var capacitacionesPorClas = listaCapacitaciones
                    .Where(x => (ClasificacionCursoEnum)x.curso.clasificacion == clasificacionEnum && x.fechaCapacitacion >= fechaInicio && x.fechaCapacitacion <= fechaFin);

                var horas = capacitacionesPorClas.Sum(x => x.curso.duracion * x.asistentes.Count());

                var clasificacion = Tuple.Create(horas, clasificacionEnum.GetDescription());

                data.Add(clasificacion);
            }

            return data;
        }


        #endregion

        #region Matriz de Necesidades
        //ObtenerAreasPorCC
        public Dictionary<string, object> ObtenerAreasPorCC(List<string> ccsCplan, List<string> ccsArr)
        {
            try
            {
                var areasPorCC = new List<ComboGroupDTO>();
              
                    if ((ccsCplan == null || ccsCplan.Count == 0) && (ccsArr == null || ccsArr.Count == 0))
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add(MESSAGE, "La lista de centro de costos está vacía.");
                        return resultado;
                    }
                    string strCCSCP = ccsCplan != null ? getStringInlineArray(ccsCplan) : "''";
                    string strCCSARR = ccsArr != null ? getStringInlineArray(ccsArr) : "''";

                    string queryBaseCP = @"
                    SELECT clave_depto as id, desc_depto AS departamento, cc
                                    FROM tblRH_EK_Departamentos 
                                    WHERE 
                                        desc_depto NOT LIKE '%(NO USAR)%' AND 
                                        desc_depto NOT LIKE '%LA YAQUI%' AND 
                                        cc IN (" + strCCSCP + ") " +
                                        "ORDER BY departamento";

                    string queryBaseARR = @"
                    SELECT clave_depto as id, desc_depto AS departamento, cc
                                    FROM tblRH_EK_Departamentos 
                                    WHERE 
                                        desc_depto NOT LIKE '%(NO USAR)%' AND 
                                        desc_depto NOT LIKE '%LA YAQUI%' AND 
                                        cc IN (" + strCCSARR + ") " +
                                        "ORDER BY departamento";


                    //string strCCSARR = getStringInlineArray(ccsArr);
                    //var odbc = new OdbcConsultaDTO();

                    if (ccsCplan != null && ccsCplan.Count > 0)
                    {
                        //odbc.consulta = String.Format(queryBase, ccsCplan.ToParamInValue());

                        //odbc.parametros.AddRange(
                        //    ccsCplan.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //var departamentosCplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, odbc);

                        var departamentosCplan = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = queryBaseCP
                        });

                        if (departamentosCplan.Count > 0)
                        {
                            var areasCplan = departamentosCplan
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = (MainContextEnum)vSesiones.sesionEmpresaActual +" "+ x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasCplan);
                        }
                    }

                    if (ccsArr != null && ccsArr.Count > 0)
                    {
                        //odbc.consulta = String.Format(queryBase, ccsArr.ToParamInValue());

                        //odbc.parametros.Clear();

                        //odbc.parametros.AddRange(
                        //    ccsArr.Select(x => new OdbcParameterDTO() { nombre = "ccs", tipo = OdbcType.VarChar, valor = x })
                        //);

                        //var departamentosArr = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, odbc);

                        var departamentosArr = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = queryBaseARR,
                            parametros = new { ccs = getStringInlineArray(ccsArr) }
                        });

                        if (departamentosArr.Count > 0)
                        {
                            var areasArr = departamentosArr
                                .GroupBy(x => x.cc)
                                .OrderBy(x => x.Key)
                                .Select(x => new ComboGroupDTO
                                {
                                    label = "ARRENDADORA " + x.Key,
                                    options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "2", Id = x.Key }).ToList()
                                }).ToList();

                            areasPorCC.AddRange(areasArr);
                        }
                    }                  
            
                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, areasPorCC);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
            }

            return resultado;
        }

        public string getStringInlineArray(List<string> lista)
        {
            string result = @"";
            foreach (var i in lista)
            {
                result += "'" + i + "',";
            }
            result = result.TrimEnd(',');
            return result;
        }

        public Dictionary<string, object> CargarDatosMatrizNecesidades(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones)
        {
            try
            {
                #region Filtros
                if (clasificaciones == null)
                {
                    clasificaciones = EnumExtensions.ToArray<ClasificacionCursoEnum>().OrderBy(x => x).ToList();
                }

                clasificaciones = clasificaciones.OrderBy(x => x).ToList();

                if ((ccsCplan == null || ccsCplan.Count == 0) && (ccsArr == null || ccsArr.Count == 0))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La lista de centros de costos viene vacía.");
                    return resultado;
                }

                var listaCCs = new List<string>();

                if (ccsCplan != null)
                {
                    listaCCs.AddRange(ccsCplan);
                }
                if (ccsArr != null)
                {
                    listaCCs.AddRange(ccsArr);
                }
                #endregion

                #region Información inicial
                var empleados = ObtenerEmpleadosCCPorDepartamento(ccsCplan, ccsArr, departamentosIDs);
                var cursosAplicables = CargarCursosAplicables(empleados, clasificaciones, ccsCplan, ccsArr);
                var indicadores = new List<IndicadorClasificacionDTO>();
                var listaEmpleadosRelacionCursos = DefinirRelacionCursosEmpleados(cursosAplicables, empleados, listaCCs, ref indicadores);
                var columnasCursos = cursosAplicables.Select(x => Tuple.Create(x.id.ToString(), x.nombre)).OrderBy(x => x.Item2).ToList();

                resultado.Add("totalEmpleados", empleados.Count);

                // Se agregan las variables a sesión en caso de que se quiera exportar a Excel.
                HttpContext.Current.Session["columnasCursos"] = columnasCursos;
                HttpContext.Current.Session["listaEmpleadosRelacionCursos"] = listaEmpleadosRelacionCursos;

                resultado.Add("columnasCursos", columnasCursos);
                resultado.Add("listaEmpleadosRelacionCursos", listaEmpleadosRelacionCursos);
                #endregion

                AgregarIndicadoresGlobales(ref resultado, indicadores, departamentosIDs);
                AgregarEstadisticas(ref resultado, indicadores, clasificaciones);
                //AgregarDatosGraficasDetalles(ref resultado, indicadores);

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "CargarDatosMatrizNecesidades", e, AccionEnum.CONSULTA, 0, new { ccsCplan = ccsCplan, ccsArr = ccsArr, departamentosIDs = departamentosIDs, clasificaciones = clasificaciones });
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información para el dashboard.");
            }

            return resultado;
        }

        public Dictionary<string, object> CargarDatosSeccionMatriz(List<string> ccsCplan, List<string> ccsArr, List<string> departamentosIDs, List<ClasificacionCursoEnum> clasificaciones, SeccionMatrizEnum seccion)
        {
            try
            {
                #region Filtros
                if (clasificaciones == null)
                {
                    clasificaciones = EnumExtensions.ToArray<ClasificacionCursoEnum>().OrderBy(x => x).ToList();
                }

                clasificaciones = clasificaciones.OrderBy(x => x).ToList();

                if ((ccsCplan == null || ccsCplan.Count == 0) && (ccsArr == null || ccsArr.Count == 0))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La lista de centros de costos viene vacía.");
                    return resultado;
                }

                var listaCCs = new List<string>();

                if (ccsCplan != null)
                {
                    listaCCs.AddRange(ccsCplan);
                }
                if (ccsArr != null)
                {
                    listaCCs.AddRange(ccsArr);
                }
                #endregion

                #region Información inicial
                var empleados = ObtenerEmpleadosCCPorDepartamento(ccsCplan, ccsArr, departamentosIDs);
                var cursosAplicables = CargarCursosAplicables(empleados, clasificaciones, ccsCplan, ccsArr);
                var indicadores = new List<IndicadorClasificacionDTO>();
                var listaEmpleadosRelacionCursos = DefinirRelacionCursosEmpleados(cursosAplicables, empleados, listaCCs, ref indicadores);
                var columnasCursos = cursosAplicables.Select(x => Tuple.Create(x.id.ToString(), x.nombre)).OrderBy(x => x.Item2).ToList();

                resultado.Add("totalEmpleados", empleados.Count);

                // Se agregan las variables a sesión en caso de que se quiera exportar a Excel.
                HttpContext.Current.Session["columnasCursos"] = columnasCursos;
                HttpContext.Current.Session["listaEmpleadosRelacionCursos"] = listaEmpleadosRelacionCursos;

                resultado.Add("columnasCursos", columnasCursos);
                resultado.Add("listaEmpleadosRelacionCursos", listaEmpleadosRelacionCursos);
                #endregion

                switch (seccion)
                {
                    case SeccionMatrizEnum.PERSONAL_ACTIVO:
                        break;
                    case SeccionMatrizEnum.ESTADISTICAS_INDIVIDUALES:
                        AgregarEstadisticasSeccion(ref resultado, indicadores, clasificaciones);
                        break;
                    case SeccionMatrizEnum.MANDO_ADMINISTRATIVO_PROTOCOLO_FATALIDAD:
                        AgregarGraficasMandoAdministrativoProtocoloFatalidad(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_MEDIO_PROTOCOLO_FATALIDAD:
                        AgregarGraficasMandoMedioProtocoloFatalidad(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_OPERATIVO_PROTOCOLO_FATALIDAD:
                        AgregarGraficasMandoOperativoProtocoloFatalidad(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_ADMINISTRATIVO_NORMATIVO:
                        AgregarGraficasMandoAdministrativoNormativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_MEDIO_NORMATIVO:
                        AgregarGraficasMandoMedioNormativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_OPERATIVO_NORMATIVO:
                        AgregarGraficasMandoOperativoNormativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_ADMINISTRATIVO_TECNICO_OPERATIVO:
                        AgregarGraficasMandoAdministrativoTecnicoOperativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_MEDIO_TECNICO_OPERATIVO:
                        AgregarGraficasMandoMedioTecnicoOperativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_OPERATIVO_TECNICO_OPERATIVO:
                        AgregarGraficasMandoOperativoTecnicoOperativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_ADMINISTRATIVO_INSTRUCTIVO_OPERATIVO:
                        AgregarGraficasMandoAdministrativoInstructivoOperativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_MEDIO_INSTRUCTIVO_OPERATIVO:
                        AgregarGraficasMandoMedioInstructivoOperativo(ref resultado, indicadores);
                        break;
                    case SeccionMatrizEnum.MANDO_OPERATIVO_INSTRUCTIVO_OPERATIVO:
                        AgregarGraficasMandoOperativoInstructivoOperativo(ref resultado, indicadores);
                        break;
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "CargarDatosSeccionMatriz", e, AccionEnum.CONSULTA, 0, new { ccsCplan = ccsCplan, ccsArr = ccsArr, departamentosIDs = departamentosIDs, clasificaciones = clasificaciones, seccion = seccion });
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información para el dashboard.");
            }

            return resultado;
        }

        private void AgregarEstadisticasSeccion(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores, List<ClasificacionCursoEnum> clasificaciones)
        {
            var estadisticas = new List<EstadisticaPorcentaje2DTO>();
            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();

            indicadores.GroupBy(x => x.cursoDesc).OrderBy(x => x.Key).ForEach(x =>
            {
                int totalCapacitados = x.Where(y => y.capacitado).Count();
                int totalFaltante = x.Where(y => y.capacitado == false).Count();
                int totalPersonalAplica = x.Count();
                decimal porcentaje = totalPersonalAplica > 0 ? Math.Round((totalCapacitados * 100) / (decimal)totalPersonalAplica, 2) : 0;
                var reglaPersonalAutorizado = listaCursosPersonalAutorizado.FirstOrDefault(y => y.claveCurso == x.First().cursoClave) != null;

                if (reglaPersonalAutorizado)
                {
                    porcentaje = (porcentaje * 2 > 100) ? 100 : porcentaje * 2;
                }

                var estadistica = new EstadisticaPorcentaje2DTO
                {
                    claveCurso = x.First().cursoClave,
                    cursoDesc = x.First().cursoDesc,
                    clasificacionEnum = x.First().clasificacion,
                    clasificacion = x.First().clasificacion.GetDescription(),
                    totalVigentes = totalCapacitados,
                    totalFaltante = totalFaltante,
                    personalAplica = totalPersonalAplica,
                    porcentajeNumero = porcentaje,
                    porcentaje = porcentaje + " %",
                    reglaPersonalAutorizado = reglaPersonalAutorizado
                };

                estadisticas.Add(estadistica);
            });

            resultado.Add("estadisticas", estadisticas);
        }

        #region Gráficas Matriz de Necesidades por Secciones
        private void AgregarGraficasMandoAdministrativoProtocoloFatalidad(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresProtocolos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.ProtocoloFatalidad);
            var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeProtocolo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeProtocolo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

            var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
            resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarGraficasMandoMedioProtocoloFatalidad(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresProtocolos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.ProtocoloFatalidad);
            var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Medio, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeProtocolo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeProtocolo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoMedioProtocoloFatalidad", porcentajeProtocolo);

            var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Medio));
            resultado.Add("chartCantidadMandoMedioProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarGraficasMandoOperativoProtocoloFatalidad(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresProtocolos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.ProtocoloFatalidad);
            var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Operativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeProtocolo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeProtocolo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoOperativoProtocoloFatalidad", porcentajeProtocolo);

            var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Operativo));
            resultado.Add("chartCantidadMandoOperativoProtocoloFatalidad", cantidadProtocolo);
        }

        private void AgregarGraficasMandoAdministrativoNormativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresNormativos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.Normativo);
            var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Administrativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeNormativo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeNormativo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoAdministrativoNormativo", porcentajeNormativo);

            var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Administrativo));
            resultado.Add("chartCantidadMandoAdministrativoNormativo", cantidadNormativo);
        }

        private void AgregarGraficasMandoMedioNormativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresNormativos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.Normativo);
            var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Medio, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeNormativo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeNormativo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoMedioNormativo", porcentajeNormativo);

            var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Medio));
            resultado.Add("chartCantidadMandoMedioNormativo", cantidadNormativo);
        }

        private void AgregarGraficasMandoOperativoNormativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresNormativos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.Normativo);
            var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Operativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeNormativo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeNormativo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoOperativoNormativo", porcentajeNormativo);

            var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Operativo));
            resultado.Add("chartCantidadMandoOperativoNormativo", cantidadNormativo);
        }

        private void AgregarGraficasMandoAdministrativoTecnicoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresTecnico = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.TecnicoOperativo);
            var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Administrativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeTecnico.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeTecnico.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoAdministrativoTecnicoOperativo", porcentajeTecnico);

            var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Administrativo));
            resultado.Add("chartCantidadMandoAdministrativoTecnicoOperativo", cantidadTecnico);
        }

        private void AgregarGraficasMandoMedioTecnicoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresTecnico = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.TecnicoOperativo);
            var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Medio, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeTecnico.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeTecnico.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoMedioTecnicoOperativo", porcentajeTecnico);

            var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Medio));
            resultado.Add("chartCantidadMandoMedioTecnicoOperativo", cantidadTecnico);
        }

        private void AgregarGraficasMandoOperativoTecnicoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresTecnico = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.TecnicoOperativo);
            var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Operativo, true);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeTecnico.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeTecnico.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoOperativoTecnicoOperativo", porcentajeTecnico);

            var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Operativo));
            resultado.Add("chartCantidadMandoOperativoTecnicoOperativo", cantidadTecnico);
        }

        private void AgregarGraficasMandoAdministrativoInstructivoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresInstructivo = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.InstructivoOperativo);

            var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Administrativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeInstructivo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeInstructivo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoAdministrativoInstructivoOperativo", porcentajeInstructivo);

            var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Administrativo));
            resultado.Add("chartCantidadMandoAdministrativoInstructivoOperativo", cantidadInstructivo);
        }

        private void AgregarGraficasMandoMedioInstructivoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresInstructivo = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.InstructivoOperativo);
            var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Medio, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeInstructivo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeInstructivo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoMedioInstructivoOperativo", porcentajeInstructivo);

            var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Medio));
            resultado.Add("chartCantidadMandoMedioInstructivoOperativo", cantidadInstructivo);
        }

        private void AgregarGraficasMandoOperativoInstructivoOperativo(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresInstructivo = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.InstructivoOperativo);
            var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Operativo, false);
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            {
                listaLinea.Add(90m);
            }

            porcentajeInstructivo.datasets.Add(new DatasetDTO
            {
                data = listaLinea,
                label = "Porcentaje Mínimo Requerido",
                type = "line"
            });
            porcentajeInstructivo.datasets.Add(datosBarras);
            resultado.Add("porcentajeMandoOperativoInstructivoOperativo", porcentajeInstructivo);

            var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Operativo));
            resultado.Add("chartCantidadMandoOperativoInstructivoOperativo", cantidadInstructivo);
        }
        #endregion

        public Tuple<MemoryStream, string> DescargarExcelPersonalActivo()
        {
            try
            {
                var listaEmpleadosRelacionCursos = HttpContext.Current.Session["listaEmpleadosRelacionCursos"] as List<Dictionary<string, object>>;
                var columnasCursosSesion = HttpContext.Current.Session["columnasCursos"] as List<Tuple<string, string>>;

                if (listaEmpleadosRelacionCursos == null || columnasCursosSesion == null)
                {
                    return null;
                }

                // Se agregan las columnas.
                var columnasCursos = new List<Tuple<string, string>>();
                columnasCursos.AddRange(new List<Tuple<string, string>>
                {
                    Tuple.Create("claveEmpleado","Clave"),
                    Tuple.Create("curp", "CURP"),
                    Tuple.Create("nombre","Nombre"),
                    Tuple.Create("puesto","Puesto"),
                    Tuple.Create("departamentoEmpleado","Área"),
                });
                columnasCursos.AddRange(columnasCursosSesion);

                var headersExcel = columnasCursos.Select(x => x.Item2).ToArray();

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja = excel.Workbook.Worksheets.Add("Personal Activo");

                    List<string[]> headerRow = new List<string[]>() { headersExcel };

                    string headerRange = "A1:" + ExcelUtilities.GetExcelColumnName(headersExcel.Count()) + "1";

                    hoja.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var empleadoCursos in listaEmpleadosRelacionCursos)
                    {
                        var cellDataEmployee = new object[empleadoCursos.Count];

                        int counter = 0;

                        foreach (var dict in empleadoCursos)
                        {
                            cellDataEmployee[counter++] = ObtenerStringEstatusCurso(dict.Value.ToString());
                        }

                        cellData.Add(cellDataEmployee);
                    }

                    hoja.Cells[2, 1].LoadFromArrays(cellData);

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return Tuple.Create(bytes, "Personal Activo.xlsx");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "DescargarExcelPersonalActivo", e, AccionEnum.DESCARGAR, 0, 0);
                return null;
            }
        }

        private static string ObtenerStringEstatusCurso(string valorCelda)
        {
            if (valorCelda.Contains("<i") && valorCelda.Contains("cProgramado"))
            {
                return "Programado";
            }
            else if (valorCelda.Contains("<i") && valorCelda.Contains("cProximoVencer"))
            {
                return "Próximo a vencer";
            }
            else if (valorCelda.Contains("<i") && valorCelda.Contains("cCumplido"))
            {
                return "Cumplido";
            }
            else if (valorCelda.Contains("<i") && valorCelda.Contains("cExpirado"))
            {
                return "Expirado";
            }
            else if (valorCelda.Contains("<i") && valorCelda.Contains("cNoAplica"))
            {
                return "No Aplica";
            }
            else if (valorCelda.Contains("<i") && valorCelda.Contains("cValidacion"))
            {
                return "Validación";
            }
            else
            {
                return valorCelda;
            }
        }

        private void AgregarEstadisticas(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores, List<ClasificacionCursoEnum> clasificaciones)
        {
            var estadisticas = new List<EstadisticaPorcentaje2DTO>();
            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();
            var listaPorcentajes = new List<decimal>();

            indicadores.GroupBy(x => x.cursoDesc).OrderBy(x => x.Key).ForEach(x =>
            {
                int totalCapacitados = x.Where(y => y.capacitado).Count();
                int totalFaltante = x.Where(y => y.capacitado == false).Count();
                int totalPersonalAplica = x.Count();
                decimal porcentaje = totalPersonalAplica > 0 ? Math.Round((totalCapacitados * 100) / (decimal)totalPersonalAplica, 2) : 0;
                var reglaPersonalAutorizado = listaCursosPersonalAutorizado.FirstOrDefault(y => y.claveCurso == x.First().cursoClave) != null;

                if (reglaPersonalAutorizado)
                {
                    porcentaje = (porcentaje * 2 > 100) ? 100 : porcentaje * 2;
                }

                listaPorcentajes.Add(porcentaje);

                var estadistica = new EstadisticaPorcentaje2DTO
                {
                    claveCurso = x.First().cursoClave,
                    cursoDesc = x.First().cursoDesc,
                    clasificacionEnum = x.First().clasificacion,
                    clasificacion = x.First().clasificacion.GetDescription(),
                    totalVigentes = totalCapacitados,
                    totalFaltante = totalFaltante,
                    personalAplica = totalPersonalAplica,
                    porcentajeNumero = porcentaje,
                    porcentaje = porcentaje + " %",
                    reglaPersonalAutorizado = reglaPersonalAutorizado
                };

                estadisticas.Add(estadistica);
            });

            decimal porcentajeGlobal = listaPorcentajes.Count() > 0 ? listaPorcentajes.Average() : 0m;

            resultado.Add("porcentajeGlobal", String.Format("{0:0.00} %", porcentajeGlobal));

            #region Gráfica Porcentaje Total Por Clasificación
            List<decimal> listaLinea = new List<decimal>();

            foreach (var x in clasificaciones.Select(x => x.GetDescription()).ToList())
            {
                listaLinea.Add(90m);
            }

            //var porcentajeTotalClasificacion = new
            //{
            //    labels = clasificaciones.Select(x => x.GetDescription()).ToList(),
            //    datasets = new List<DatasetDTO>()
            //};

            ////var dataBarras = ObtenerPorcentajeTotalClasificacion(indicadores, clasificaciones);

            //porcentajeTotalClasificacion.datasets.Add(new DatasetDTO { data = listaLinea, label = "Porcentaje Mínimo Requerido", type = "line" });

            //var datasetBarras = new DatasetDTO
            //{
            //    fill = true,
            //    borderWidth = 2,
            //    backgroundColor = new List<string>(),
            //    borderColor = new List<string>(),
            //    data = new List<decimal>()
            //};

            //foreach (var clasificacion in clasificaciones)
            //{
            //    var estadisticasClasificacion = estadisticas.Where(x => x.clasificacionEnum == clasificacion).ToList();

            //    if (estadisticasClasificacion.Count() > 0)
            //    {
            //        decimal porcentaje = estadisticasClasificacion.Average(x => x.porcentajeNumero);

            //        datasetBarras.backgroundColor.Add("#EB8825");
            //        datasetBarras.borderColor.Add("#EB8825");
            //        datasetBarras.data.Add((Math.Truncate(100 * porcentaje) / 100));
            //    }
            //    else
            //    {
            //        datasetBarras.backgroundColor.Add("#EB8825");
            //        datasetBarras.borderColor.Add("#EB8825");
            //        datasetBarras.data.Add(0m);
            //    }
            //}

            //porcentajeTotalClasificacion.datasets.Add(datasetBarras); //porcentajeTotalClasificacion.datasets.Add(dataBarras);

            //resultado.Add("porcentajeTotalClasificacion", porcentajeTotalClasificacion);

            var graficaPorcentajeTotalClasificacion = new GraficaDTO();

            foreach (var clasificacion in clasificaciones)
            {
                var estadisticasClasificacion = estadisticas.Where(x => x.clasificacionEnum == clasificacion).ToList();

                if (estadisticasClasificacion.Count() > 0)
                {
                    decimal porcentaje = estadisticasClasificacion.Average(x => x.porcentajeNumero);

                    graficaPorcentajeTotalClasificacion.serie1.Add((Math.Truncate(100 * porcentaje) / 100));
                }
                else
                {
                    graficaPorcentajeTotalClasificacion.serie1.Add(0m);
                }

                graficaPorcentajeTotalClasificacion.categorias.Add(clasificacion.GetDescription());
                graficaPorcentajeTotalClasificacion.serie1Descripcion = "";
            }

            graficaPorcentajeTotalClasificacion.serie2 = listaLinea;

            resultado.Add("porcentajeTotalClasificacion", graficaPorcentajeTotalClasificacion);
            #endregion

            resultado.Add("estadisticas", estadisticas);
        }

        private void AgregarDatosGraficasDetalles(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores)
        {
            var indicadoresProtocolos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.ProtocoloFatalidad);
            var indicadoresNormativos = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.Normativo);
            var indicadoresInstructivo = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.InstructivoOperativo);
            var indicadoresTecnico = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.TecnicoOperativo);

            //// Protocolos
            //{
            //    var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    var datosBarras = ObtenerPorcentajeClasificacion(indicadoresProtocolos, false);
            //    List<decimal> listaLinea = new List<decimal>();

            //    foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //    {
            //        listaLinea.Add(90m);
            //    }

            //    porcentajeProtocolo.datasets.Add(new DatasetDTO
            //    {
            //        data = listaLinea,
            //        label = "Porcentaje Mínimo Requerido",
            //        type = "line"
            //    });

            //    porcentajeProtocolo.datasets.Add(datosBarras);
            //    resultado.Add("porcentajeProtocolo", porcentajeProtocolo);

            //    var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresProtocolos));
            //    resultado.Add("cantidadProtocolo", cantidadProtocolo);
            //}

            //// Normativo 
            //{
            //    var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    var datosBarras = ObtenerPorcentajeClasificacion(indicadoresNormativos, false);
            //    List<decimal> listaLinea = new List<decimal>();

            //    foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //    {
            //        listaLinea.Add(90m);
            //    }

            //    porcentajeNormativo.datasets.Add(new DatasetDTO
            //    {
            //        data = listaLinea,
            //        label = "Porcentaje Mínimo Requerido",
            //        type = "line"
            //    });

            //    porcentajeNormativo.datasets.Add(datosBarras);
            //    resultado.Add("porcentajeNormativo", porcentajeNormativo);

            //    var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresNormativos));
            //    resultado.Add("cantidadNormativo", cantidadNormativo);
            //}

            //// General 
            //{
            //    var indicadoresGeneral = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.General);

            //    var porcentajeGeneral = new { labels = indicadoresGeneral.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    porcentajeGeneral.datasets.Add(ObtenerPorcentajeClasificacion(indicadoresGeneral, false));
            //    resultado.Add("porcentajeGeneral", porcentajeGeneral);

            //    var cantidadGeneral = new { labels = indicadoresGeneral.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadGeneral.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresGeneral));
            //    resultado.Add("cantidadGeneral", cantidadGeneral);
            //}

            //// Formativo 
            //{
            //    var indicadoresFormativo = ObtenerIndicadoresPorClasificacion(indicadores, ClasificacionCursoEnum.Formativo);

            //    var porcentajeFormativo = new { labels = indicadoresFormativo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    porcentajeFormativo.datasets.Add(ObtenerPorcentajeClasificacion(indicadoresFormativo, false));
            //    resultado.Add("porcentajeFormativo", porcentajeFormativo);

            //    var cantidadFormativo = new { labels = indicadoresFormativo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadFormativo.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresFormativo));
            //    resultado.Add("cantidadFormativo", cantidadFormativo);
            //}

            //// Instructivo 
            //{
            //    var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    var datosBarras = ObtenerPorcentajeClasificacion(indicadoresInstructivo, false);
            //    List<decimal> listaLinea = new List<decimal>();

            //    foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //    {
            //        listaLinea.Add(90m);
            //    }

            //    porcentajeInstructivo.datasets.Add(new DatasetDTO
            //    {
            //        data = listaLinea,
            //        label = "Porcentaje Mínimo Requerido",
            //        type = "line"
            //    });
            //    porcentajeInstructivo.datasets.Add(datosBarras);
            //    resultado.Add("porcentajeInstructivo", porcentajeInstructivo);

            //    var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresInstructivo));
            //    resultado.Add("cantidadInstructivo", cantidadInstructivo);
            //}

            //// Técnico 
            //{
            //    var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    var datosBarras = ObtenerPorcentajeClasificacion(indicadoresTecnico, true);
            //    List<decimal> listaLinea = new List<decimal>();

            //    foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
            //    {
            //        listaLinea.Add(90m);
            //    }

            //    porcentajeTecnico.datasets.Add(new DatasetDTO
            //    {
            //        data = listaLinea,
            //        label = "Porcentaje Mínimo Requerido",
            //        type = "line"
            //    });
            //    porcentajeTecnico.datasets.Add(datosBarras);
            //    resultado.Add("porcentajeTecnico", porcentajeTecnico);

            //    var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
            //    cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacion(indicadoresTecnico));
            //    resultado.Add("cantidadTecnico", cantidadTecnico);
            //}

            #region Gráficas de Capacitación Operativa
            #region Protocolo de Fatalidad
            {
                var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeProtocolo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeProtocolo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoAdministrativoProtocoloFatalidad", porcentajeProtocolo);

                var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Administrativo));
                resultado.Add("chartCantidadMandoAdministrativoProtocoloFatalidad", cantidadProtocolo);
            }

            {
                var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Medio, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeProtocolo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeProtocolo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoMedioProtocoloFatalidad", porcentajeProtocolo);

                var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Medio));
                resultado.Add("chartCantidadMandoMedioProtocoloFatalidad", cantidadProtocolo);
            }

            {
                var porcentajeProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresProtocolos, MandoEnum.Operativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeProtocolo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeProtocolo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoOperativoProtocoloFatalidad", porcentajeProtocolo);

                var cantidadProtocolo = new { labels = indicadoresProtocolos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadProtocolo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresProtocolos, MandoEnum.Operativo));
                resultado.Add("chartCantidadMandoOperativoProtocoloFatalidad", cantidadProtocolo);
            }
            #endregion

            #region Normativo
            {
                var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Administrativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeNormativo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeNormativo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoAdministrativoNormativo", porcentajeNormativo);

                var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Administrativo));
                resultado.Add("chartCantidadMandoAdministrativoNormativo", cantidadNormativo);
            }

            {
                var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Medio, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeNormativo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeNormativo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoMedioNormativo", porcentajeNormativo);

                var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Medio));
                resultado.Add("chartCantidadMandoMedioNormativo", cantidadNormativo);
            }

            {
                var porcentajeNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresNormativos, MandoEnum.Operativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeNormativo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeNormativo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoOperativoNormativo", porcentajeNormativo);

                var cantidadNormativo = new { labels = indicadoresNormativos.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadNormativo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresNormativos, MandoEnum.Operativo));
                resultado.Add("chartCantidadMandoOperativoNormativo", cantidadNormativo);
            }
            #endregion

            #region Técnico Operativo
            {
                var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Administrativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeTecnico.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeTecnico.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoAdministrativoTecnicoOperativo", porcentajeTecnico);

                var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Administrativo));
                resultado.Add("chartCantidadMandoAdministrativoTecnicoOperativo", cantidadTecnico);
            }

            {
                var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Medio, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeTecnico.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeTecnico.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoMedioTecnicoOperativo", porcentajeTecnico);

                var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Medio));
                resultado.Add("chartCantidadMandoMedioTecnicoOperativo", cantidadTecnico);
            }

            {
                var porcentajeTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresTecnico, MandoEnum.Operativo, true);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeTecnico.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeTecnico.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoOperativoTecnicoOperativo", porcentajeTecnico);

                var cantidadTecnico = new { labels = indicadoresTecnico.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadTecnico.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresTecnico, MandoEnum.Operativo));
                resultado.Add("chartCantidadMandoOperativoTecnicoOperativo", cantidadTecnico);
            }
            #endregion

            #region Instructivo Operativo
            {
                var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Administrativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeInstructivo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeInstructivo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoAdministrativoInstructivoOperativo", porcentajeInstructivo);

                var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Administrativo));
                resultado.Add("chartCantidadMandoAdministrativoInstructivoOperativo", cantidadInstructivo);
            }

            {
                var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Medio, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeInstructivo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeInstructivo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoMedioInstructivoOperativo", porcentajeInstructivo);

                var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Medio));
                resultado.Add("chartCantidadMandoMedioInstructivoOperativo", cantidadInstructivo);
            }

            {
                var porcentajeInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                var datosBarras = ObtenerPorcentajeClasificacionPorMando(indicadoresInstructivo, MandoEnum.Operativo, false);
                List<decimal> listaLinea = new List<decimal>();

                foreach (var x in indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList())
                {
                    listaLinea.Add(90m);
                }

                porcentajeInstructivo.datasets.Add(new DatasetDTO
                {
                    data = listaLinea,
                    label = "Porcentaje Mínimo Requerido",
                    type = "line"
                });
                porcentajeInstructivo.datasets.Add(datosBarras);
                resultado.Add("porcentajeMandoOperativoInstructivoOperativo", porcentajeInstructivo);

                var cantidadInstructivo = new { labels = indicadoresInstructivo.GroupBy(x => x.cursoClave).Select(x => x.Key).ToList(), datasets = new List<DatasetDTO>() };
                cantidadInstructivo.datasets.AddRange(ObtenerCantidadClasificacionPorMando(indicadoresInstructivo, MandoEnum.Operativo));
                resultado.Add("chartCantidadMandoOperativoInstructivoOperativo", cantidadInstructivo);
            }
            #endregion
            #endregion
        }

        private List<DatasetDTO> ObtenerCantidadClasificacion(List<IndicadorClasificacionDTO> indicadoresPorClasificacion)
        {
            var indicadoresPorCurso = indicadoresPorClasificacion
                .GroupBy(x => x.cursoClave)
                .OrderBy(x => x.Key)
                .ToList();

            string randomColorRGBA = ObtenerColorGraficaAleatorio();

            var datasetReal = new DatasetDTO
            {
                label = "Real",
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                data = new List<decimal>()
            };

            foreach (var x in indicadoresPorCurso)
            {
                int capacitados = x.Where(y => y.capacitado).Count();
                datasetReal.data.Add(capacitados);
            }

            randomColorRGBA = ObtenerColorGraficaAleatorio();

            var datasetFaltante = new DatasetDTO
            {
                label = "Faltante",
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#b5b5b5").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#b5b5b5").ToList(),
                data = new List<decimal>()
            };

            foreach (var x in indicadoresPorCurso)
            {
                int capacitados = x.Where(y => y.capacitado == false).Count();
                datasetFaltante.data.Add(capacitados);
            }

            return new List<DatasetDTO> { datasetReal, datasetFaltante };
        }

        private List<DatasetDTO> ObtenerCantidadClasificacionPorMando(List<IndicadorClasificacionDTO> indicadoresPorClasificacion, MandoEnum mando)
        {
            var indicadoresPorCurso = indicadoresPorClasificacion.Where(x => x.listaMandos.Contains((int)mando))
                .GroupBy(x => x.cursoClave)
                .OrderBy(x => x.Key)
                .ToList();

            string randomColorRGBA = ObtenerColorGraficaAleatorio();

            var datasetReal = new DatasetDTO
            {
                label = "Real",
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                data = new List<decimal>()
            };

            foreach (var x in indicadoresPorCurso)
            {
                int capacitados = x.Where(y => y.capacitado).Count();
                datasetReal.data.Add(capacitados);
            }

            randomColorRGBA = ObtenerColorGraficaAleatorio();

            var datasetFaltante = new DatasetDTO
            {
                label = "Faltante",
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#b5b5b5").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#b5b5b5").ToList(),
                data = new List<decimal>()
            };

            foreach (var x in indicadoresPorCurso)
            {
                int capacitados = x.Where(y => y.capacitado == false).Count();
                datasetFaltante.data.Add(capacitados);
            }

            return new List<DatasetDTO> { datasetReal, datasetFaltante };
        }

        private List<IndicadorClasificacionDTO> ObtenerIndicadoresPorClasificacion(List<IndicadorClasificacionDTO> indicadores, ClasificacionCursoEnum clasificacion)
        {
            return indicadores.Where(x => x.clasificacion == clasificacion).OrderBy(x => x.cursoClave).ToList();
        }

        private DatasetDTO ObtenerPorcentajeClasificacion(List<IndicadorClasificacionDTO> indicadoresPorClasificacion, bool reglaPersonalAutorizado)
        {
            var indicadoresPorCurso = indicadoresPorClasificacion
                .GroupBy(x => x.cursoClave)
                .OrderBy(x => x.Key)
                .ToList();

            string randomColorRGBA = ObtenerColorGraficaAleatorio();

            var dataset = new DatasetDTO
            {
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                data = new List<decimal>()
            };

            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();

            foreach (var ind in indicadoresPorCurso)
            {
                int requeridos = ind.Count();
                int capacitados = ind.Where(y => y.capacitado).Count();

                if (reglaPersonalAutorizado)
                {
                    var cantidadCursosCapacitadosPersonalAutorizado = ind.Where(y =>
                        y.capacitado &&
                        listaCursosPersonalAutorizado.Select(z => z.claveCurso).Contains(y.cursoClave)
                    ).Count();

                    ////Se duplica la cantidad de cursos capacitados que tengan la regla de personal autorizado para indicar que nomás se necesita el 50% de capacitación para llegar al 100% del cumplimiento.
                    //capacitados = (capacitados + cantidadCursosCapacitadosPersonalAutorizado > requeridos) ? requeridos : capacitados + cantidadCursosCapacitadosPersonalAutorizado;
                }

                decimal porcentaje = requeridos > 0 ? Math.Round((capacitados * 100) / (decimal)requeridos, 2) : 0;
                dataset.data.Add(porcentaje);
            }

            return dataset;
        }

        private DatasetDTO ObtenerPorcentajeClasificacionPorMando(List<IndicadorClasificacionDTO> indicadoresPorClasificacion, MandoEnum mando, bool reglaPersonalAutorizado)
        {
            var indicadoresPorCurso = indicadoresPorClasificacion.Where(x => x.listaMandos.Contains((int)mando))
                .GroupBy(x => x.cursoClave)
                .OrderBy(x => x.Key)
                .ToList();

            string randomColorRGBA = ObtenerColorGraficaAleatorio();

            var dataset = new DatasetDTO
            {
                fill = true,
                borderWidth = 2,
                backgroundColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                borderColor = indicadoresPorCurso.Select(x => "#EB8825").ToList(),
                data = new List<decimal>()
            };

            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();

            foreach (var ind in indicadoresPorCurso)
            {
                int requeridos = ind.Count();
                int capacitados = ind.Where(y => y.capacitado).Count();

                if (reglaPersonalAutorizado)
                {
                    var cantidadCursosCapacitadosPersonalAutorizado = ind.Where(y =>
                        y.capacitado &&
                        listaCursosPersonalAutorizado.Select(z => z.claveCurso).Contains(y.cursoClave)
                    ).Count();

                    ////Se duplica la cantidad de cursos capacitados que tengan la regla de personal autorizado para indicar que nomás se necesita el 50% de capacitación para llegar al 100% del cumplimiento.
                    //capacitados = (capacitados + cantidadCursosCapacitadosPersonalAutorizado > requeridos) ? requeridos : capacitados + cantidadCursosCapacitadosPersonalAutorizado;
                }

                decimal porcentaje = requeridos > 0 ? Math.Round((capacitados * 100) / (decimal)requeridos, 2) : 0;
                dataset.data.Add(porcentaje);
            }

            return dataset;
        }

        private DatasetDTO ObtenerPorcentajeTotalClasificacion(List<IndicadorClasificacionDTO> indicadores, List<ClasificacionCursoEnum> clasificaciones)
        {
            var indicadoresPorClasificacion = indicadores.GroupBy(x => x.clasificacion).OrderBy(x => x.Key).ToList();
            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();
            var dataset = new DatasetDTO
            {
                fill = true,
                borderWidth = 2,
                backgroundColor = new List<string>(),
                borderColor = new List<string>(),
                data = new List<decimal>()
            };

            foreach (var clasificacion in clasificaciones)
            {
                var indicadorClasificacion = indicadoresPorClasificacion.FirstOrDefault(x => x.Key == clasificacion);

                if (indicadorClasificacion != null)
                {
                    int requeridos = indicadorClasificacion.Count();
                    int capacitados = indicadorClasificacion.Where(y => y.capacitado).Count();

                    var cantidadCursosCapacitadosPersonalAutorizado = indicadorClasificacion.Where(y =>
                        y.capacitado &&
                        listaCursosPersonalAutorizado.Select(z => z.claveCurso).Contains(y.cursoClave)
                    ).Count();

                    ////Se duplica la cantidad de cursos capacitados que tengan la regla de personal autorizado para indicar que nomás se necesita el 50% de capacitación para llegar al 100% del cumplimiento.
                    //capacitados = (capacitados + cantidadCursosCapacitadosPersonalAutorizado > requeridos) ? requeridos : capacitados + cantidadCursosCapacitadosPersonalAutorizado;

                    decimal porcentaje = requeridos > 0 ? Math.Round((capacitados * 100) / (decimal)requeridos, 2) : 0;

                    dataset.backgroundColor.Add("#EB8825");
                    dataset.borderColor.Add("#EB8825");
                    dataset.data.Add(porcentaje);
                }
                else
                {
                    dataset.backgroundColor.Add("#EB8825");
                    dataset.borderColor.Add("#EB8825");
                    dataset.data.Add(0m);
                }
            }

            return dataset;
        }

        private void AgregarIndicadoresGlobales(ref Dictionary<string, object> resultado, List<IndicadorClasificacionDTO> indicadores, List<string> departamentosIDs)
        {
            var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();

            #region Tabla Indicadores Globales
            var indicadoresAgrupados = indicadores.GroupBy(x => x.departamentoDesc).Select(x => new { departamentoDesc = x.Key, grp = x.ToList() }).ToList();
            var listaIndicadoresGlobales = new List<dynamic>();

            foreach (var ind in indicadoresAgrupados)
            {
                var gruposPorCursos = ind.grp.GroupBy(x => x.cursoDesc).ToList();
                var listaPorcentajesGrupo = new List<decimal>();

                foreach (var c in gruposPorCursos)
                {
                    int totalCapacitadosCurso = c.Where(y => y.capacitado).Count();
                    int totalFaltanteCurso = c.Where(y => y.capacitado == false).Count();
                    int totalPersonalAplicaCurso = c.Count();
                    decimal porcentajeCursoEnGrupo = totalPersonalAplicaCurso > 0 ? Math.Round((totalCapacitadosCurso * 100) / (decimal)totalPersonalAplicaCurso, 2) : 0;
                    var reglaPersonalAutorizado = listaCursosPersonalAutorizado.FirstOrDefault(y => y.claveCurso == c.First().cursoClave) != null;

                    if (reglaPersonalAutorizado)
                    {
                        porcentajeCursoEnGrupo = (porcentajeCursoEnGrupo * 2 > 100) ? 100 : porcentajeCursoEnGrupo * 2;
                    }

                    listaPorcentajesGrupo.Add(porcentajeCursoEnGrupo);
                }

                listaIndicadoresGlobales.Add(new
                {
                    areaOperativa = ind.departamentoDesc,
                    porcentajeCapacitacion = (listaPorcentajesGrupo.Count() > 0 ? listaPorcentajesGrupo.Average().ToString("0.00") : "0.00") + "%",
                    porcentajeCapacitacionNumero = listaPorcentajesGrupo.Count() > 0 ? listaPorcentajesGrupo.Average() : 0m
                });
            }

            resultado.Add("tablaIndicadoresGlobales", listaIndicadoresGlobales);
            #endregion

            // Se agregan datos globales
            int totalRequeridos = indicadores.Count;
            int totalCapacitados = indicadores.Where(x => x.capacitado).Count();
            int totalRestantes = totalRequeridos - totalCapacitados;

            var cantidadCursosCapacitadosPersonalAutorizado = indicadores.Where(y =>
                y.capacitado &&
                listaCursosPersonalAutorizado.Select(z => z.claveCurso).Contains(y.cursoClave)
            ).Count();

            ////Se duplica la cantidad de cursos capacitados que tengan la regla de personal autorizado para indicar que nomás se necesita el 50% de capacitación para llegar al 100% del cumplimiento.
            //totalCapacitados =
            //    (totalCapacitados + cantidadCursosCapacitadosPersonalAutorizado > totalRequeridos) ?
            //        totalRequeridos :
            //        totalCapacitados + cantidadCursosCapacitadosPersonalAutorizado;

            //double porcentajeGlobal = totalRequeridos > 0 ? (totalCapacitados * 100) / (double)totalRequeridos : 0;

            resultado.Add("totalRestantes", totalRestantes);
            resultado.Add("totalCapacitados", totalCapacitados);
            resultado.Add("totalRequeridos", totalRequeridos);
            //resultado.Add("porcentajeGlobal", String.Format("{0:0.00} %", porcentajeGlobal));

            // Se agregan indicadores por departamento
            var departamentos = indicadores
                .GroupBy(x => x.departamentoDesc)
                .Select(x => new
                {
                    departamento = x.Key,
                    requeridos = x.Count(),
                    capacitados = x.Where(y => y.capacitado).Count(),
                    restantes = x.Where(y => y.capacitado == false).Count()
                })
                .OrderByDescending(x => x.requeridos)
                .ToList();

            resultado.Add("departamentos", departamentos);
        }

        private List<Dictionary<string, object>> DefinirRelacionCursosEmpleados(List<tblS_CapacitacionSeguridadCursos> cursosAplicables, List<EmpleadoCapacitacionDTO> empleados, List<string> listaCCs, ref List<IndicadorClasificacionDTO> indicadores)
        {
            var fechaMinima = DateTime.Today.AddHours(23).AddMinutes(59).AddYears(-1); // Un año atrás al día de hoy.
            var fechaFiltroCapacitaciones = DateTime.Today.AddYears(-2); //Dos años atrás para filtrar las capacitaciones consultadas.
            var listaEmpleadosRelacionCursos = new List<Dictionary<string, object>>();
            var cursosAplicables_id = cursosAplicables.Select(y => y.id).ToList();
            var capacitacionesPorCC = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                x.activo &&
                x.division == divisionActual &&
                DbFunctions.TruncateTime(x.fechaCapacitacion) >= DbFunctions.TruncateTime(fechaFiltroCapacitaciones) &&
                cursosAplicables_id.Contains(x.cursoID)
            ).ToList().Where(x => ((EstatusControlAsistenciaEnum)x.estatus) == EstatusControlAsistenciaEnum.Completa).Select(x => new CapacitacionDTO
            {
                id = x.id,
                cursoID = x.cursoID,
                fechaCapacitacion = x.fechaCapacitacion,
                validacion = x.validacion
            }).ToList();
            var capacitacionesPorCC_id = capacitacionesPorCC.Select(x => x.id).ToList();

            cursosAplicables = cursosAplicables.OrderBy(x => x.nombre).ToList(); //Se ordena por nombre los cursos para que coincida la información con sus columnas al momento de exportar el reporte.

            var lstCapacitacionesAprobadas = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(e => (EstatusEmpledoControlAsistenciaEnum)e.estatus == EstatusEmpledoControlAsistenciaEnum.Aprobado).ToList();

            foreach (var empleado in empleados)
            {
                var empleadoRelacionCursoDTO = new Dictionary<string, object>();

                empleadoRelacionCursoDTO.Add("claveEmpleado", empleado.claveEmpleado);
                empleadoRelacionCursoDTO.Add("curp", empleado.curp);
                empleadoRelacionCursoDTO.Add("nombre", empleado.nombreEmpleado);
                empleadoRelacionCursoDTO.Add("puesto", empleado.puestoEmpleado);
                //empleadoRelacionCursoDTO.Add("ccDesc", empleado.cc);
                //empleadoRelacionCursoDTO.Add("departamentoID", empleado.departamentoID);
                empleadoRelacionCursoDTO.Add("departamentoEmpleado", empleado.departamentoEmpleado);

                var capacitacionesAprobadasEmpleado = lstCapacitacionesAprobadas.Where(x => x.claveEmpleado == empleado.claveEmpleado &&
                    capacitacionesPorCC_id.Contains(x.controlAsistenciaID)
                ).ToList().Select(x => new CapacitacionDTO
                {
                    cursoID = capacitacionesPorCC.FirstOrDefault(y => y.id == x.controlAsistenciaID).cursoID,
                    fechaCapacitacion = capacitacionesPorCC.FirstOrDefault(y => y.id == x.controlAsistenciaID).fechaCapacitacion,
                    validacion = capacitacionesPorCC.FirstOrDefault(y => y.id == x.controlAsistenciaID).validacion,
                }).ToList();
                //var cursosEmpleado = capacitacionesPorCC.Where(x => x.asistentes.Exists(y => y.claveEmpleado == empleado.claveEmpleado)).ToList();

                // Se itera entre los cursos y se agregan las propiedades al diccionario.
                foreach (var cursoAplicable in cursosAplicables)
                {
                    var capacitacionesAprobadasEmpleadoPorCursos = capacitacionesAprobadasEmpleado.Where(x => x.cursoID == cursoAplicable.id).ToList();
                    //var tupla = Tuple.Create(cursoAplicable.id.ToString(), ObtenerEstatusCursoEmpleado(cursoAplicable, cursosEmpleado, empleado, fechaMinima, ref indicadores));
                    var tupla = Tuple.Create(cursoAplicable.id.ToString(), ObtenerEstatusCursoEmpleado2(cursoAplicable, capacitacionesAprobadasEmpleadoPorCursos, empleado, fechaMinima, ref indicadores));
                    empleadoRelacionCursoDTO.Add(tupla.Item1, tupla.Item2);
                }

                listaEmpleadosRelacionCursos.Add(empleadoRelacionCursoDTO);
            }

            return listaEmpleadosRelacionCursos;
        }

        private string ObtenerEstatusCursoEmpleado(tblS_CapacitacionSeguridadCursos cursoAplicable, List<tblS_CapacitacionSeguridadControlAsistencia> cursosEmpleado, EmpleadoCapacitacionDTO empleado, DateTime fechaMinima, ref List<IndicadorClasificacionDTO> indicadores)
        {
            string clase = "";
            string title = "";

            bool aplicaSuPuesto = false;
            bool tienePuestos = cursoAplicable.Puestos != null && cursoAplicable.Puestos.Where(x => x.estatus && x.division == divisionActual).ToList().Count > 0;

            if (tienePuestos)
            {
                aplicaSuPuesto = cursoAplicable.Puestos.Where(x => x.estatus && x.division == divisionActual).Select(x => x.puesto_id).Contains(empleado.puestoID);
            }

            // Si es general y le aplica
            if (
                //cursoAplicable.esGeneral ||
                cursoAplicable.aplicaTodosPuestos || (tienePuestos && aplicaSuPuesto))
            {
                var listaMandosCurso = _context.tblS_CapacitacionSeguridadCursosMando.Where(x =>
                    x.estatus && x.division == divisionActual && x.curso_id == cursoAplicable.id
                ).Select(x => (int)x.mando).ToList();

                // Si es aplicable si inicializa indicador.
                var indicador = new IndicadorClasificacionDTO
                {
                    clasificacion = (ClasificacionCursoEnum)cursoAplicable.clasificacion,
                    cursoDesc = cursoAplicable.nombre,
                    cursoClave = cursoAplicable.claveCurso,
                    cursoID = cursoAplicable.id.ToString(),
                    listaMandos = listaMandosCurso,
                    departamentoDesc = String.Format("{0} - CC {1}", empleado.departamentoEmpleado, empleado.ccID),
                    departamentoID = empleado.departamentoID.ToString(),
                    tematica = (TematicaCursoEnum)cursoAplicable.tematica
                };

                // Cursos que haya hecho y aprobado.
                var cursosAprobados = cursosEmpleado.Where(x =>
                    x.cursoID == cursoAplicable.id &&
                    x.asistentes.Any(y => y.claveEmpleado == empleado.claveEmpleado && (EstatusEmpledoControlAsistenciaEnum)y.estatus == EstatusEmpledoControlAsistenciaEnum.Aprobado)
                ).ToList();

                // Si le aplica el curso y nunca lo ha hecho o aprobado
                if (cursosAprobados.Count == 0)
                {
                    clase = "cProgramado";
                    title = "Falta por recibir capacitación";
                }
                // Si tiene cursos hechos, se verifica el más reciente.
                else
                {
                    var cursoMasReciente = cursosAprobados.OrderByDescending(x => x.fechaCapacitacion).First();

                    //Se verifica si el curso es de capacitación única. Si no lo es se verifica la vigencia.
                    if (!cursoAplicable.capacitacionUnica)
                    {
                        // Si el curso más reciente aún está vigente, se determina si está próximo a expirar.
                        if (cursoMasReciente.fechaCapacitacion >= fechaMinima)
                        {
                            // Si el curso está vigente y próximo a expirar
                            if (cursoMasReciente.fechaCapacitacion >= fechaMinima && cursoMasReciente.fechaCapacitacion <= fechaMinima.AddMonths(1))
                            {
                                clase = "cProximoVencer";
                                indicador.capacitado = true;
                                title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                            }
                            else
                            {
                                //Se verifica si la capacitación fue de validación.
                                if (cursoMasReciente.validacion)
                                {
                                    clase = "cValidacion";
                                    indicador.capacitado = true;
                                    title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                                }
                                else
                                {
                                    clase = "cCumplido";
                                    indicador.capacitado = true;
                                    title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                                }
                            }
                        }
                        else
                        {
                            clase = "cExpirado";
                            title = "Expiró el " + cursoMasReciente.fechaCapacitacion.AddYears(1).ToShortDateString();
                        }
                    }
                    else
                    {
                        clase = "cCumplido";
                        indicador.capacitado = true;
                        title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                    }
                }

                indicadores.Add(indicador);
            }
            else
            {
                clase = "cNoAplica";
                title = "No aplica";
            }

            return String.Format(@"<i class='fas fa-circle infoHover {0}' title='{1}'></i>", clase, title);
        }

        private string ObtenerEstatusCursoEmpleado2(tblS_CapacitacionSeguridadCursos cursoAplicable, List<CapacitacionDTO> capacitacionesAprobadas, EmpleadoCapacitacionDTO empleado, DateTime fechaMinima, ref List<IndicadorClasificacionDTO> indicadores)
        {
            string clase = "";
            string title = "";

            bool aplicaSuPuesto = false;
            bool tienePuestos = cursoAplicable.Puestos != null && cursoAplicable.Puestos.Where(x => x.estatus && x.division == divisionActual).ToList().Count > 0;

            if (tienePuestos)
            {
                aplicaSuPuesto = cursoAplicable.Puestos.Where(x => x.estatus && x.division == divisionActual).Select(x => x.puesto_id).Contains(empleado.puestoID);
            }

            // Si es general y le aplica
            if (
                //cursoAplicable.esGeneral ||
                cursoAplicable.aplicaTodosPuestos || (tienePuestos && aplicaSuPuesto))
            {
                var listaMandosCurso = _context.tblS_CapacitacionSeguridadCursosMando.Where(x =>
                    x.estatus && x.division == divisionActual && x.curso_id == cursoAplicable.id
                ).Select(x => (int)x.mando).ToList();

                // Si es aplicable si inicializa indicador.
                var indicador = new IndicadorClasificacionDTO
                {
                    clasificacion = (ClasificacionCursoEnum)cursoAplicable.clasificacion,
                    cursoDesc = cursoAplicable.nombre,
                    cursoClave = cursoAplicable.claveCurso,
                    cursoID = cursoAplicable.id.ToString(),
                    listaMandos = listaMandosCurso,
                    departamentoDesc = String.Format("{0} - CC {1}", empleado.departamentoEmpleado, empleado.ccID),
                    departamentoID = empleado.departamentoID.ToString(),
                    tematica = (TematicaCursoEnum)cursoAplicable.tematica
                };

                // Si le aplica el curso y nunca lo ha hecho o aprobado
                if (capacitacionesAprobadas.Count == 0)
                {
                    clase = "cProgramado";
                    title = "Falta por recibir capacitación";
                }
                // Si tiene cursos hechos, se verifica el más reciente.
                else
                {
                    var cursoMasReciente = capacitacionesAprobadas.OrderByDescending(x => x.fechaCapacitacion).First();

                    //Se verifica si el curso es de capacitación única. Si no lo es se verifica la vigencia.
                    if (!cursoAplicable.capacitacionUnica)
                    {
                        // Si el curso más reciente aún está vigente, se determina si está próximo a expirar.
                        if (cursoMasReciente.fechaCapacitacion >= fechaMinima)
                        {
                            // Si el curso está vigente y próximo a expirar
                            if (cursoMasReciente.fechaCapacitacion >= fechaMinima && cursoMasReciente.fechaCapacitacion <= fechaMinima.AddMonths(1))
                            {
                                clase = "cProximoVencer";
                                indicador.capacitado = true;
                                title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                            }
                            else
                            {
                                //Se verifica si la capacitación fue de validación.
                                if (cursoMasReciente.validacion)
                                {
                                    clase = "cValidacion";
                                    indicador.capacitado = true;
                                    title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                                }
                                else
                                {
                                    clase = "cCumplido";
                                    indicador.capacitado = true;
                                    title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                                }
                            }
                        }
                        else
                        {
                            clase = "cExpirado";
                            title = "Expiró el " + cursoMasReciente.fechaCapacitacion.AddYears(1).ToShortDateString();
                        }
                    }
                    else
                    {
                        clase = "cCumplido";
                        indicador.capacitado = true;
                        title = "Capacitado el " + cursoMasReciente.fechaCapacitacion.ToShortDateString();
                    }
                }

                indicadores.Add(indicador);
            }
            else
            {
                clase = "cNoAplica";
                title = "No aplica";
            }

            return String.Format(@"<i class='fas fa-circle infoHover {0}' title='{1}'></i>", clase, title);
        }

        /// <summary>
        /// Determina entre una lista de asistentes si el empleado indicado aprobó el curso.
        /// </summary>
        /// <param name="asistentes"></param>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        private bool TieneCursosAprobados(List<tblS_CapacitacionSeguridadControlAsistenciaDetalle> asistentes, int claveEmpleado)
        {
            return asistentes.Any(y => y.claveEmpleado == claveEmpleado &&
                                        ((EstatusAutorizacionEmpleadoControlAsistenciaEnum)y.estatusAutorizacion == EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado ||
                                        ((EstatusAutorizacionEmpleadoControlAsistenciaEnum)y.estatusAutorizacion == EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica && (EstatusEmpledoControlAsistenciaEnum)y.estatus == EstatusEmpledoControlAsistenciaEnum.Aprobado)));
        }

        private List<tblS_CapacitacionSeguridadCursos> CargarCursosAplicables(List<EmpleadoCapacitacionDTO> empleados, List<ClasificacionCursoEnum> clasificaciones, List<string> ccsCplan, List<string> ccsArr)
        {
            List<tblS_CapacitacionSeguridadCursos> cursosAplicables = new List<tblS_CapacitacionSeguridadCursos>();
            List<tblS_CapacitacionSeguridadCursosPuestos> cursosPorPuesto = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.estatus && x.division == divisionActual).ToList();
            List<tblS_CapacitacionSeguridadCursosCC> cursosPorCC = _context.tblS_CapacitacionSeguridadCursosCC.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                ((ccsCplan != null ? ccsCplan.Contains(x.cc) : true) || (ccsArr != null ? ccsArr.Contains(x.cc) : true))
            ).ToList();
            List<tblS_CapacitacionSeguridadCursos> listaCursos = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList().Where(x =>
                (clasificaciones == null ? true : clasificaciones.Contains((ClasificacionCursoEnum)x.clasificacion))
            ).ToList();

            foreach (var cur in listaCursos)
            {
                var puestosAplicables = cursosPorPuesto.Where(x => x.curso_id == cur.id).Select(x => x.puesto_id).ToList();
                var ccAplicables = cursosPorCC.Where(x => x.curso_id == cur.id).Select(x => x.cc).ToList();
                var flagAgregado = false;

                foreach (var emp in empleados)
                {
                    if (puestosAplicables.Contains(emp.puestoID) && ccAplicables.Contains(emp.ccID) && !flagAgregado)
                    {
                        cursosAplicables.Add(cur);
                        flagAgregado = true;
                    }

                    if (vSesiones.sesionSistemaActual == 16) //Módulo Capital Humano
                    {
                        if (!flagAgregado && (cur.esGeneral || cur.aplicaTodosPuestos))
                        {
                            cursosAplicables.Add(cur);
                            flagAgregado = true;
                        }
                    }
                }
            }

            //var cursosPorCC = _context.tblS_CapacitacionSeguridadCursosCC.Where(x =>
            //    x.estatus && x.division == divisionActual).ToList().Where(x => ((ccsCplan != null ? ccsCplan.Contains(x.cc) : true) || (ccsArr != null ? ccsArr.Contains(x.cc) : true))
            //).Where(x => x.Curso != null).Select(x => x.Curso).Distinct().ToList();

            //foreach (var emp in empleados)
            //{
            //    var listaCursosPorPuesto = cursosPorPuesto.Where(x => x.puesto_id == emp.puestoID).ToList();

            //    foreach (var cur in listaCursosPorPuesto)
            //    {
            //        var cursoPorCC = cursosPorCC.FirstOrDefault(x => x.cc == emp.ccID);

            //        if (cursoPorCC != null)
            //        {
            //            var curso = listaCursos.FirstOrDefault(x => x.id == cursoPorCC.curso_id);

            //            if (curso != null)
            //            {
            //                cursosAplicables.Add(curso);
            //            }
            //        }
            //    }
            //}

            //// Cursos que aplican en base al puesto del empleado.
            //var cursosAplicablesPorPuesto = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x =>
            //    x.estatus && empleados.Select(y => y.puestoID).Contains(x.puesto_id) && x.division == divisionActual
            //).Select(x => x.curso).Distinct().ToList();

            //#region Filtro con los centros de costo de los cursos
            //var centrosCostoCursos = _context.tblS_CapacitacionSeguridadCursosCC.Where(x =>
            //    x.estatus && x.division == divisionActual
            //).ToList();

            ////Filtrar los cursos aplicables por los centros de costos aplicables.
            //cursosAplicablesPorPuesto = cursosAplicablesPorPuesto.Where(x => centrosCostoCursos.Select(y => y.curso_id).Contains(x.id)).ToList();

            ////Filtrar los cursos aplicables por los centros de costos seleccionados por el usuario en los filtros.
            //var cursosPorCC = centrosCostoCursos.Where(x =>
            //    ((ccsCplan != null ? ccsCplan.Contains(x.cc) : true) || (ccsArr != null ? ccsArr.Contains(x.cc) : true))
            //).Select(x => x.curso_id).ToList();

            //cursosAplicables = cursosAplicables.Where(x => cursosPorCC.Contains(x.id)).ToList();
            //#endregion

            // Cursos que aplican en base al puesto, son generales o aplican a todos.
            //var totalCursosAplicables = _context.tblS_CapacitacionSeguridadCursos
            //    .Where(x => x.estatus == EstatusCursoEnum.Completo && x.division == divisionActual)
            //    .ToList()
            //    .Where(x => x.esGeneral || x.aplicaTodosPuestos || cursosAplicables.Contains(x))
            //    .Where(x => clasificaciones == null ? true : clasificaciones.Contains(x.clasificacion))
            //    .ToList();

            //return totalCursosAplicables;

            return cursosAplicables;
        }

        private string ObtenerColorGraficaAleatorio()
        {
            int r = RandomInteger(0, 255);
            int g = RandomInteger(0, 255);
            int b = RandomInteger(0, 255);
            float a = 0.6f; // Valor constante para colores definidos.

            return String.Format("rgba({0},{1},{2},{3})", r, g, b, a);
        }

        #endregion

        #region Privilegios
        private List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> getLstEmpleadoPrivilegios(BusqPrivilegiosDTO busq)
        {
            List<int> priv = busq.privilegios.Select(x => (int)x).ToList();
            return _context.tblS_CapacitacionSeguridadEmpleadoPrivilegio
                    .Where(w => w.esActivo && w.division == divisionActual)
                //.Where(w => busq.privilegios.Any(p => p == w.idPrivilegio))
                    .Where(w => priv.Contains((int)w.idPrivilegio))
                    .ToList();
        }
        private List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> getLstEmpleadoPrivilegios(List<int> lstIdUsuario)
        {
            return _context.tblS_CapacitacionSeguridadEmpleadoPrivilegio
                    .Where(w => lstIdUsuario.Any(cve => cve == w.idUsuario) && w.division == divisionActual)
                    .ToList();
        }
        public tblS_CapacitacionSeguridadEmpleadoPrivilegio getPrivilegioActual()
        {
            var idUsuario = vSesiones.sesionUsuarioDTO.id;
            try
            {
                var privilegio = _context.tblS_CapacitacionSeguridadEmpleadoPrivilegio.FirstOrDefault(w => w.idUsuario == idUsuario && w.division == divisionActual);
                var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == idUsuario);

                if (privilegio != null)
                {
                    if (usuarioSIGOPLAN.usuarioAuditor)
                    {
                        privilegio.idPrivilegio = (int)PrivilegioEnum.Visor;
                    }

                    return privilegio;
                }
                else
                {
                    if (usuarioSIGOPLAN.usuarioAuditor)
                    {
                        return new tblS_CapacitacionSeguridadEmpleadoPrivilegio() { idUsuario = idUsuario, idPrivilegio = (int)PrivilegioEnum.Visor };
                    }
                    else
                    {
                        return new tblS_CapacitacionSeguridadEmpleadoPrivilegio() { idUsuario = idUsuario };
                    }

                }
            }
            catch (Exception)
            {
                return new tblS_CapacitacionSeguridadEmpleadoPrivilegio() { idUsuario = idUsuario };
            }
        }
        public Dictionary<string, object> ObtenerEmpleadosPrivilegios(BusqPrivilegiosDTO busq)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                var lstCC = _context.tblP_CC_Usuario
                    .Where(w => busq.lstCc.Contains(w.cc))
                    .ToList();

                var lstUsuarios = _context.tblP_Usuario
                    .Where(w => w.estatus && w.id != 13)
                    .ToList()
                    .Where(w => lstCC.Any(a => a.usuarioID == w.id));

                if (lstUsuarios.Count() == 0)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "No se encontraron usuarios con los filtros indicados.");
                    return resultado;
                }

                //var lstPrivilegios = getLstEmpleadoPrivilegios(busq);

                //var lst = lstUsuarios.Select(emp => new
                //{
                //    emp,
                //    priv = lstPrivilegios.Any(priv => priv.idUsuario == emp.id) ?
                //    lstPrivilegios.First(priv => priv.idUsuario == emp.id) :
                //    new tblS_CapacitacionSeguridadEmpleadoPrivilegio()
                //})
                //.Where(w => busq.privilegios.Any(a => a == w.priv.idPrivilegio))
                //.Select(emp => new
                //{
                //    id = emp.priv.id,
                //    nombre = GlobalUtils.ObtenerNombreCompletoUsuario(emp.emp),
                //    idUsuario = emp.emp.id,
                //    cc = lstCC.First(x => x.usuarioID == emp.emp.id).cc,
                //    ccDesc = ObtenerDescripcionCC(lstCC.First(x => x.usuarioID == emp.emp.id).cc),
                //    idPrivilegio = emp.priv.idPrivilegio
                //}).OrderBy(x => x.nombre);
                List<int> priv = busq.privilegios.Select(x => (int)x).ToList();
                var lst2 = new List<dynamic>();
                foreach (var i in lstUsuarios)
                {
                    var p = _context.tblS_CapacitacionSeguridadEmpleadoPrivilegio.FirstOrDefault(x => x.idUsuario == i.id && x.division == divisionActual);
                    if ((p != null ? priv.Contains((int)p.idPrivilegio) : priv.Contains(0)))
                    {
                        lst2.Add(new
                        {
                            id = p != null ? p.id : 0,
                            nombre = GlobalUtils.ObtenerNombreCompletoUsuario(i),
                            idUsuario = i.id,
                            cc = lstCC.First(x => x.usuarioID == i.id).cc,
                            ccDesc = ObtenerDescripcionCC(lstCC.First(x => x.usuarioID == i.id).cc),
                            idPrivilegio = p != null ? p.idPrivilegio : 0,
                        });
                    }
                }
                resultado.Add("lst", lst2.OrderBy(x => x.nombre).ToList());

                resultado.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al buscar los privilegios de los usuarios.");
                LogError(0, 0, NombreControlador, "ObtenerEmpleadosPrivilegios", o_O, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }
        public Dictionary<string, object> guardarEmpleadosPrivilegios(List<tblS_CapacitacionSeguridadEmpleadoPrivilegio> lst)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
                try
                {
                    var registrosUsuarios = getLstEmpleadoPrivilegios(lst.Select(s => s.idUsuario).ToList());

                    foreach (var usuario in lst)
                    {
                        var registroUsuario = registrosUsuarios.FirstOrDefault(db => db.idUsuario == usuario.idUsuario);

                        if (registroUsuario == null)
                        {
                            if (usuario.idPrivilegio == (int)PrivilegioEnum.NoAsignado)
                            {
                                continue;
                            }

                            registroUsuario = new tblS_CapacitacionSeguridadEmpleadoPrivilegio
                            {
                                esActivo = true,
                                fechaRegistro = DateTime.Now,
                                idPrivilegio = usuario.idPrivilegio,
                                nombre = usuario.nombre,
                                idUsuario = usuario.idUsuario,
                                division = divisionActual
                            };
                            _context.tblS_CapacitacionSeguridadEmpleadoPrivilegio.Add(registroUsuario);
                        }
                        else
                        {
                            registroUsuario.idPrivilegio = usuario.idPrivilegio;
                        }
                    }

                    resultado.Add(SUCCESS, true);
                    _context.SaveChanges();
                    dbTransaction.Commit();
                }
                catch (Exception o_O)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "guardarEmpleadosPrivilegios", o_O, AccionEnum.ACTUALIZAR, 0, lst);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los privilegios de los empleados.");
                }

            return resultado;
        }

        #endregion

        #region Catálogos
        public Dictionary<string, object> getRelacionesCCAutorizantes()
        {
            try
            {
                List<dynamic> listaCCEnkontrol = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT * FROM cc"
                });
                var usuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                var relaciones = _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.Where(x => x.estatus && x.division == divisionActual).ToList().Select(x => new RelacionCCAutorizanteDTO
                {
                    id = x.id,
                    cc = x.cc,
                    ccDesc = x.cc + "-" + (string)listaCCEnkontrol.FirstOrDefault(y => (string)y.cc == x.cc).descripcion,
                    usuarioID = x.usuarioID,
                    clave_empleado = usuariosSIGOPLAN.Where(y => y.id == x.usuarioID).Select(z => Int32.Parse(z.cveEmpleado)).FirstOrDefault(),
                    nombre = usuariosSIGOPLAN.Where(y => y.id == x.usuarioID).Select(z => z.nombre).FirstOrDefault(),
                    apellidoPaterno = usuariosSIGOPLAN.Where(y => y.id == x.usuarioID).Select(z => z.apellidoPaterno).FirstOrDefault(),
                    apellidoMaterno = usuariosSIGOPLAN.Where(y => y.id == x.usuarioID).Select(z => z.apellidoMaterno).FirstOrDefault(),
                    nombreCompleto = usuariosSIGOPLAN.Where(y => y.id == x.usuarioID).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    tipoPuesto = (PuestoAutorizanteEnum)x.tipoPuesto,
                    tipoPuestoDesc = ((PuestoAutorizanteEnum)x.tipoPuesto).GetDescription(),
                    orden = x.orden
                }).ToList();

                resultado.Add("data", relaciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuarioSIGOPLAN = _context.tblP_Usuario.ToList().FirstOrDefault(x => x.estatus && x.cveEmpleado == relacion.clave_empleado.ToString());

                    if (usuarioSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra el usuario de SIGOPLAN para el empleado \"" + relacion.clave_empleado + "\".");
                    }

                    relacion.usuarioID = usuarioSIGOPLAN.id;

                    switch (relacion.tipoPuesto)
                    {
                        case PuestoAutorizanteEnum.CoordinadorCMCAP:
                            relacion.orden = 1;
                            break;
                        case PuestoAutorizanteEnum.CoordinadorCSH:
                            relacion.orden = 2;
                            break;
                        case PuestoAutorizanteEnum.SecretarioCSH:
                            relacion.orden = 3;
                            break;
                        case PuestoAutorizanteEnum.GerenteProyecto:
                            relacion.orden = 4;
                            break;
                    }

                    _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.Add(new tblS_CapacitacionSeguridadRelacionCCAutorizante
                    {
                        cc = relacion.cc,
                        usuarioID = relacion.usuarioID,
                        tipoPuesto = (int)relacion.tipoPuesto,
                        orden = relacion.orden,
                        estatus = relacion.estatus,
                        division = divisionActual
                    });
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

        public Dictionary<string, object> editarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuarioSIGOPLAN = _context.tblP_Usuario.ToList().FirstOrDefault(x => x.estatus && x.cveEmpleado == relacion.clave_empleado.ToString());

                    if (usuarioSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra el usuario de SIGOPLAN para el empleado \"" + relacion.clave_empleado + "\".");
                    }

                    switch (relacion.tipoPuesto)
                    {
                        case PuestoAutorizanteEnum.CoordinadorCMCAP:
                            relacion.orden = 1;
                            break;
                        case PuestoAutorizanteEnum.CoordinadorCSH:
                            relacion.orden = 2;
                            break;
                        case PuestoAutorizanteEnum.SecretarioCSH:
                            relacion.orden = 3;
                            break;
                        case PuestoAutorizanteEnum.GerenteProyecto:
                            relacion.orden = 4;
                            break;
                    }

                    var relacionSIGOPLAN = _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                    relacionSIGOPLAN.cc = relacion.cc;
                    relacionSIGOPLAN.usuarioID = usuarioSIGOPLAN.id;
                    relacionSIGOPLAN.tipoPuesto = (int)relacion.tipoPuesto;
                    relacionSIGOPLAN.orden = relacion.orden;
                    relacionSIGOPLAN.division = divisionActual;

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

        public Dictionary<string, object> eliminarRelacionCCAutorizante(RelacionCCAutorizanteDTO relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var relacionSIGOPLAN = _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                    relacionSIGOPLAN.estatus = false;
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

        public Dictionary<string, object> getUsuarioPorClave(int claveEmpleado)
        {
            try
            {
                var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == claveEmpleado.ToString());

                if (usuarioSIGOPLAN != null)
                {
                    var data = new
                    {
                        claveEmpleado = claveEmpleado,
                        nombre = usuarioSIGOPLAN.nombre,
                        apellidoPaterno = usuarioSIGOPLAN.apellidoPaterno,
                        apellidoMaterno = usuarioSIGOPLAN.apellidoMaterno
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
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getRelacionesCCDepartamentoRazonSocial()
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    List<dynamic> centrosCostoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    List<dynamic> centrosCostoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var departamentosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    {
                        consulta = @"SELECT * FROM sn_departamentos"
                    });
                    var departamentosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    {
                        consulta = @"SELECT * FROM sn_departamentos"
                    });

                    List<RelacionCCDepartamentoRazonSocialDTO> relaciones = new List<RelacionCCDepartamentoRazonSocialDTO>();

                    #region Relaciones Construplan
                    relaciones.AddRange(_context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Where(x => x.estatus && x.empresa == 1 && x.division == divisionActual).ToList().Select(x => new RelacionCCDepartamentoRazonSocialDTO
                    {
                        id = x.id,
                        cc = x.cc,
                        ccDesc = x.cc + "-" + (string)centrosCostoConstruplan.FirstOrDefault(y => (string)y.cc == x.cc).descripcion,
                        departamento = x.departamento,
                        departamentoDesc = departamentosConstruplan.FirstOrDefault(y => (string)y.clave_depto == x.departamento) != null ?
                            departamentosConstruplan.FirstOrDefault(y => (string)y.clave_depto == x.departamento).desc_depto : "",
                        razonSocialID = x.razonSocialID,
                        razonSocialDesc = x.razonSocial.razonSocial,
                        rfc = x.razonSocial.rfc,
                        empresa = x.empresa,
                        empresaDesc = "CONSTRUPLAN"
                    }).ToList());
                    #endregion

                    #region Relaciones Arrendadora
                    relaciones.AddRange(_context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Where(x => x.estatus && x.empresa == 2 && x.division == divisionActual).ToList().Select(x => new RelacionCCDepartamentoRazonSocialDTO
                    {
                        id = x.id,
                        cc = x.cc,
                        ccDesc = x.cc + "-" + (string)centrosCostoArrendadora.FirstOrDefault(y => (string)y.cc == x.cc).descripcion,
                        departamento = x.departamento,
                        departamentoDesc = departamentosArrendadora.FirstOrDefault(y => (string)y.clave_depto == x.departamento) != null ?
                            departamentosArrendadora.FirstOrDefault(y => (string)y.clave_depto == x.departamento).desc_depto : "",
                        razonSocialID = x.razonSocialID,
                        razonSocialDesc = x.razonSocial.razonSocial,
                        rfc = x.razonSocial.rfc,
                        empresa = x.empresa,
                        empresaDesc = "ARRENDADORA"
                    }).ToList());
                    #endregion
                    resultado.Add("data", relaciones);
                }
                else
                {
                    var lstCCEmpresas = _context.tblP_CC.Where(x => x.estatus).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();

                    var lstCboDepartamentosEmpresas = _context.tblS_CatDepartamentos.Where(x => x.esActivo).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = x.cc,
                        Text = x.cc + " - " + x.descripcion
                    }).ToList();

                    List<RelacionCCDepartamentoRazonSocialDTO> relaciones = new List<RelacionCCDepartamentoRazonSocialDTO>();
                    #region Relaciones Empresas
                    relaciones.AddRange(_context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Where(x => x.estatus && x.empresa == (int)vSesiones.sesionEmpresaActual && x.division == divisionActual).ToList().Select(x => new RelacionCCDepartamentoRazonSocialDTO
                    {
                        id = x.id,
                        cc = x.cc,
                        ccDesc = x.cc + "-" + (string)lstCCEmpresas.FirstOrDefault(y => (string)y.Value == x.cc).Text,
                        departamento = x.departamento,
                        departamentoDesc = lstCboDepartamentosEmpresas.FirstOrDefault(y => (string)y.Value == x.departamento) != null ?
                            lstCboDepartamentosEmpresas.FirstOrDefault(y => (string)y.Value == x.cc).Text : "",
                        razonSocialID = x.razonSocialID,
                        razonSocialDesc = x.razonSocial.razonSocial,
                        rfc = x.razonSocial.rfc,
                        empresa = x.empresa,
                        empresaDesc = " ",
                    }).ToList());
                    #endregion
                    resultado.Add("data", relaciones);
                }
           

                //resultado.Add("data", relaciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevaRelacionCCDepartamentoRazonSocial(List<RelacionCCDepartamentoRazonSocialDTO> relaciones)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var rel in relaciones)
                    {
                        if (rel.empresa != 1 && rel.empresa != 2)
                        {
                            throw new Exception("No se encuentra la empresa del departamento.");
                        }

                        List<dynamic> departamentoConstruplan = _context.Select<dynamic>( new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = string.Format(@"SELECT * FROM tblRH_EK_Departamentos WHERE clave_depto = '{0}'", rel.departamento)
                        });
                        List<dynamic> departamentoArrendadora =_context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,

                            consulta = string.Format(@"SELECT * FROM tblRH_EK_Departamentos WHERE clave_depto = '{0}'", rel.departamento)
                        });

                        if (departamentoConstruplan.Count == 0 && departamentoArrendadora.Count == 0)
                        {
                            throw new Exception("No se encuentra la información del departamento \"" + rel.departamento + "\".");
                        }

                        _context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.Add(new tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial
                        {
                            cc = rel.empresa == 1 ? (string)departamentoConstruplan[0].cc : (string)departamentoArrendadora[0].cc,
                            departamento = rel.departamento,
                            razonSocialID = rel.razonSocialID,
                            empresa = rel.empresa,
                            estatus = rel.estatus,
                            division = divisionActual
                        });
                        _context.SaveChanges();
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

        public Dictionary<string, object> editarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (relacion.empresa != 1 && relacion.empresa != 2)
                    {
                        throw new Exception("No se encuentra la empresa del departamento.");
                    }

                    List<dynamic> departamentoConstruplan = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = string.Format(@"SELECT * FROM tblRH_EK_Departamentos WHERE clave_depto = '{0}'", relacion.departamento)
                    });
                    List<dynamic> departamentoArrendadora = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,

                        consulta = string.Format(@"SELECT * FROM tblRH_EK_Departamentos WHERE clave_depto = '{0}'", relacion.departamento)
                    });

                    if (departamentoConstruplan.Count == 0 && departamentoArrendadora.Count == 0)
                    {
                        throw new Exception("No se encuentra la información del departamento \"" + relacion.departamento + "\".");
                    }

                    var relacionSIGOPLAN = _context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                    relacionSIGOPLAN.cc = relacion.empresa == 1 ? (string)departamentoConstruplan[0].cc : (string)departamentoArrendadora[0].cc;
                    relacionSIGOPLAN.departamento = relacion.departamento;
                    relacionSIGOPLAN.razonSocialID = relacion.razonSocialID;
                    relacionSIGOPLAN.empresa = relacion.empresa;
                    relacionSIGOPLAN.division = divisionActual;

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

        public Dictionary<string, object> eliminarRelacionCCDepartamentoRazonSocial(RelacionCCDepartamentoRazonSocialDTO relacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var relacionSIGOPLAN = _context.tblS_CapacitacionSeguridadRelacionCCDepartamentoRazonSocial.FirstOrDefault(x => x.id == relacion.id && x.division == divisionActual);

                    relacionSIGOPLAN.estatus = false;
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

        public Dictionary<string, object> getDepartamentosCombo()
        {
            try
            {
                var areasPorCC = new List<ComboGroupDTO>();

                var odbc = new OdbcConsultaDTO();

                List<dynamic> centrosCostoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                List<dynamic> centrosCostoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                var departamentosConstruplan = _context.Select<DepartamentoDTO>( new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,

                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"
                });
                var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,

                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"
                });

                if (departamentosConstruplan.Count > 0)
                {
                    areasPorCC.AddRange(departamentosConstruplan.GroupBy(x => x.cc).OrderBy(x => x.Key).Select(x => new ComboGroupDTO
                    {
                        label = "CONSTRUPLAN - " + x.Key,
                        options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "CONSTRUPLAN", Id = x.Key }).ToList(),
                        Prefijo = "CONSTRUPLAN"
                    }));
                }

                if (departamentosArrendadora.Count > 0)
                {
                    areasPorCC.AddRange(departamentosArrendadora.GroupBy(x => x.cc).OrderBy(x => x.Key).Select(x => new ComboGroupDTO
                    {
                        label = "ARRENDADORA - " + x.Key,
                        options = x.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "ARRENDADORA", Id = x.Key }).ToList(),
                        Prefijo = "ARRENDADORA"
                    }));
                }

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, areasPorCC);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error interno al intentar obtener el catálogo tipos de emleados");
            }

            return resultado;
        }
        #endregion

        public List<ComboDTO> getRazonSocialCombo()
        {
            try
            {
                var listaRazonSocial = _context.tblS_CapacitacionSeguridadRazonSocial.ToList().Where(x => x.estatus).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.razonSocial,
                    Prefijo = x.rfc
                }).OrderBy(x => x.Text).ToList();

                return listaRazonSocial;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public MemoryStream crearExcelRelacionCursosPuestos()
        {
            try
            {
                var relaciones = (from relacion in _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x => x.division == divisionActual).ToList()
                                  join curso in _context.tblS_CapacitacionSeguridadCursos.Where(x => x.division == divisionActual).ToList() on relacion.curso_id equals curso.id
                                  where relacion.estatus && curso.isActivo
                                  select new RelacionCursoPuestoDTO
                                  {
                                      curso_id = relacion.curso_id,
                                      puesto_id = relacion.puesto_id,
                                      claveCurso = curso.claveCurso,
                                      nombreCurso = curso.nombre
                                  }).ToList();

                //var puestosEnkontrol = (List<dynamic>)ContextEnKontrolNomina.Where(string.Format(@"SELECT * FROM si_puestos")).ToObject<List<dynamic>>();

                var puestosEnkontrol = _context.Select<dynamic>(new DapperDTO 
                {
                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                    consulta = "SELECT * FROM tblRH_EK_Puestos",
                });

                foreach (var rel in relaciones)
                {
                    var puestoEnkontrol = puestosEnkontrol.FirstOrDefault(x => (int)x.puesto == rel.puesto_id);

                    if (puestoEnkontrol != null)
                    {
                        rel.puesto = (string)puestoEnkontrol.descripcion;
                    }
                }

                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Insumos");

                    List<string[]> headerRow = new List<string[]>() { new string[] { "curso_id", "puesto_id", "Clave Curso", "Nombre Curso", "Puesto Descripción", "Aplica Autorización" } };
                    string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    hoja1.Cells[headerRange].LoadFromArrays(headerRow);

                    var cellData = new List<object[]>();

                    foreach (var rel in relaciones)
                    {
                        cellData.Add(new object[] { rel.curso_id, rel.puesto_id, rel.claveCurso, rel.nombreCurso, rel.puesto, "" });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);

                    //excel.Compression = CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ComboDTO> obtenerComboCursos()
        {
            try
            {
                var listaCursos = _context.tblS_CapacitacionSeguridadCursos.ToList().Where(x => x.isActivo && x.division == divisionActual).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.claveCurso + " " + x.nombre
                }).OrderBy(x => x.Text).ToList();

                return listaCursos;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public Dictionary<string, object> guardarCargaMasivaControlAsistencia(HttpPostedFileBase controlAsistencia)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var usuario = vSesiones.sesionUsuarioDTO;

                    using (ZipArchive archive = new ZipArchive(controlAsistencia.InputStream))
                    {
                        var archivos = archive.Entries.Where(x => !string.IsNullOrEmpty(Path.GetExtension(x.FullName))).ToList(); //Filtro para quitar carpetas.

                        foreach (ZipArchiveEntry archivo in archivos)
                        {
                            var listaRuta = archivo.FullName.Split('/');
                            var centroCosto = listaRuta[0];
                            var codigoCurso = listaRuta[1];
                            var claveInstructor = listaRuta[2];
                            var lugar = listaRuta[3];
                            var fecha = DateTime.Parse(listaRuta[4]);
                            var hora = listaRuta[5];
                            //var rfc = listaRuta[6];
                            var nombreArchivo = Path.GetFileNameWithoutExtension(archivo.Name);

                            var cursoSIGOPLAN = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.isActivo && x.claveCurso == codigoCurso && x.division == divisionActual);

                            if (cursoSIGOPLAN == null)
                            {
                                throw new Exception("No se encuentra la información del curso \"" + codigoCurso + "\".");
                            }

                            var usuarioInstructor = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == claveInstructor);

                            if (usuarioInstructor == null)
                            {
                                throw new Exception("No se encuentra la información de usuario para el instructor con la clave \"" + claveInstructor + "\".");
                            }

                            var controlAsistenciaSIGOPLAN = _context.tblS_CapacitacionSeguridadControlAsistencia.ToList().FirstOrDefault(x =>
                                x.activo && x.cursoID == cursoSIGOPLAN.id && x.instructorID == usuarioInstructor.id && x.fechaCapacitacion.Date == fecha.Date && x.cc == centroCosto && x.division == divisionActual
                            );

                            if (controlAsistenciaSIGOPLAN == null)
                            {
                                #region Crear el registro del control de asistencia
                                tblS_CapacitacionSeguridadControlAsistencia nuevoControlAsistencia = new tblS_CapacitacionSeguridadControlAsistencia();

                                nuevoControlAsistencia.cursoID = cursoSIGOPLAN.id;
                                nuevoControlAsistencia.instructorID = usuarioInstructor.id;
                                nuevoControlAsistencia.fechaCapacitacion = fecha;
                                nuevoControlAsistencia.cc = centroCosto;
                                nuevoControlAsistencia.lugar = lugar;
                                nuevoControlAsistencia.horario = hora;

                                if (vSesiones.sesionCapacitacionOperativa)
                                {
                                    nuevoControlAsistencia.estatus = (int)EstatusControlAsistenciaEnum.Completa;
                                }
                                else
                                {
                                    nuevoControlAsistencia.estatus = cursoSIGOPLAN.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad ?
                                    (int)EstatusControlAsistenciaEnum.PendienteAutorizacion : (int)EstatusControlAsistenciaEnum.Completa;
                                }

                                nuevoControlAsistencia.rutaListaAutorizacion = null;
                                nuevoControlAsistencia.comentario = null;
                                nuevoControlAsistencia.rfc = "";
                                nuevoControlAsistencia.razonSocial = "";
                                nuevoControlAsistencia.esExterno = false;
                                nuevoControlAsistencia.instructorExterno = null;
                                nuevoControlAsistencia.empresaExterna = null;
                                nuevoControlAsistencia.activo = true;
                                nuevoControlAsistencia.fechaCreacion = DateTime.Now;
                                nuevoControlAsistencia.usuarioCreadorID = vSesiones.sesionUsuarioDTO.id;
                                nuevoControlAsistencia.division = divisionActual;

                                _context.tblS_CapacitacionSeguridadControlAsistencia.Add(nuevoControlAsistencia);
                                _context.SaveChanges();

                                nuevoControlAsistencia.nombreCarpeta = NombreBaseControlAsistencia + nuevoControlAsistencia.id;
                                _context.SaveChanges();
                                #endregion

                                #region Crear carpeta y archivo del control de asistencia
                                var rutaCarpetaControlAsistencia = Path.Combine(RutaControlAsistencias, nuevoControlAsistencia.nombreCarpeta);

                                // Verifica si existe la carpeta y si no, la crea.
                                if (verificarExisteCarpeta(rutaCarpetaControlAsistencia, true) == false)
                                {
                                    throw new Exception("No se pudo crear la carpeta del control de asistencia en el servidor.");
                                }
                                #endregion

                                controlAsistenciaSIGOPLAN = nuevoControlAsistencia;

                                // Se agrega la información referente a los autorizantes.
                                if (cursoSIGOPLAN.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad)
                                {
                                    if (!vSesiones.sesionCapacitacionOperativa)
                                    {
                                        var autorizantes = _context.tblS_CapacitacionSeguridadRelacionCCAutorizante.Where(x => x.estatus && x.cc == centroCosto && x.division == divisionActual).ToList();
                                        var puestosAutorizantes = Infrastructure.Utils.EnumExtensions.ToCombo<PuestoAutorizanteEnum>();
                                        var jefeID = 0;
                                        var coordinadorID = 0;
                                        var secretarioID = 0;
                                        var gerenteID = 0;

                                        foreach (var puestoAutorizante in puestosAutorizantes)
                                        {
                                            var autorizante = autorizantes.FirstOrDefault(x => x.tipoPuesto == (int)puestoAutorizante.Value);

                                            if (autorizante == null)
                                            {
                                                throw new Exception("No existe un autorizante asignado para el puesto \"" + puestoAutorizante.Text + "\" en el centro de costo \"" + centroCosto + "\".");
                                            }

                                            switch ((PuestoAutorizanteEnum)puestoAutorizante.Value)
                                            {
                                                case PuestoAutorizanteEnum.CoordinadorCMCAP:
                                                    jefeID = autorizante.usuarioID;
                                                    break;
                                                case PuestoAutorizanteEnum.CoordinadorCSH:
                                                    coordinadorID = autorizante.usuarioID;
                                                    break;
                                                case PuestoAutorizanteEnum.SecretarioCSH:
                                                    secretarioID = autorizante.usuarioID;
                                                    break;
                                                case PuestoAutorizanteEnum.GerenteProyecto:
                                                    gerenteID = autorizante.usuarioID;
                                                    break;
                                                default:
                                                    throw new NotImplementedException("Tipo de autorizante no definido.");
                                            }
                                        }

                                        CrearRegistrosAutorizantes(controlAsistenciaSIGOPLAN, jefeID, coordinadorID, secretarioID, gerenteID);

                                        // Se crea la alerta para el primer usuario.
                                        var mensaje = String.Format("Autorizar Capacitación: {0}", cursoSIGOPLAN.claveCurso);
                                        CrearAlertaAutorizacion(mensaje, new List<int> { jefeID }, controlAsistenciaSIGOPLAN.id);
                                    }
                                }
                            }

                            // Verifica si el archivo del ciclo es el control de asistencia.
                            if (nombreArchivo.ToUpper() == "CONTROL DE ASISTENCIA" || nombreArchivo.ToUpper() == "CONTROL ASISTENCIA")
                            {
                                var rutaCarpetaControlAsistencia = Path.Combine(RutaControlAsistencias, controlAsistenciaSIGOPLAN.nombreCarpeta);
                                var nombreArchivoControlAsistencia = String.Format("{0} {1}{2}",
                                NombreBaseControlAsistencia,
                                ObtenerFormatoCarpetaFechaActual(),
                                Path.GetExtension(archivo.Name));

                                var rutaArchivo = Path.Combine(rutaCarpetaControlAsistencia, nombreArchivoControlAsistencia);

                                // Se actualizan campos de la entidad control de asistencia
                                var registroControlAsistenciaSIGOPLAN = _context.tblS_CapacitacionSeguridadControlAsistencia.FirstOrDefault(x => x.id == controlAsistenciaSIGOPLAN.id && x.division == divisionActual);
                                registroControlAsistenciaSIGOPLAN.rutaListaAsistencia = rutaArchivo;
                                _context.SaveChanges();

                                #region Guardar el archivo de control de asistencia
                                if (File.Exists(rutaArchivo))
                                {
                                    int count = 1;

                                    string fileNameOnly = Path.GetFileNameWithoutExtension(rutaArchivo);
                                    string extension = Path.GetExtension(rutaArchivo);
                                    string path = Path.GetDirectoryName(rutaArchivo);
                                    string newFullPath = rutaArchivo;

                                    while (File.Exists(newFullPath))
                                    {
                                        string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                                        newFullPath = Path.Combine(path, tempFileName + extension);
                                    }

                                    rutaArchivo = newFullPath;
                                }

                                archivo.ExtractToFile(Path.Combine(rutaArchivo));

                                if (File.Exists(rutaArchivo) == false)
                                {
                                    dbContextTransaction.Rollback();
                                    resultado.Clear();
                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                    return resultado;
                                }
                                #endregion
                            }
                            else
                            {
                                var listaArchivoExamen = nombreArchivo.Split('-');
                                var claveEmpleado = Int32.Parse(listaArchivoExamen[0]);
                                var examenConsecutivo = listaArchivoExamen[1];
                                var estadoEvaluacion = listaArchivoExamen[2];

                                if (listaArchivoExamen.ElementAtOrDefault(3) == null)
                                {
                                    throw new Exception("No se especificó la calificación para el archivo \"" + nombreArchivo + "\".");
                                }

                                var calificacion = Convert.ToDecimal(listaArchivoExamen[3]);

                                //var empleadoEnkontrolConsulta = ContextEnKontrolNomina.Where(
                                //    string.Format(@"SELECT * FROM sn_empleados WHERE clave_empleado = {0}", claveEmpleado)
                                //);

                                //if (empleadoEnkontrolConsulta == null)
                                //{
                                //    throw new Exception("No se encuentra la información del empleado con la clave: " + claveEmpleado + ".");
                                //}

                                //var empleadoEnkontrol = ((List<dynamic>)empleadoEnkontrolConsulta.ToObject<List<dynamic>>())[0];

                                var empleadoEnkontrol = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == claveEmpleado);
                                Core.Entity.Administrativo.RecursosHumanos.Enkontrol.tblRH_EK_Puestos puestoEnkontrol = new Core.Entity.Administrativo.RecursosHumanos.Enkontrol.tblRH_EK_Puestos();
                                if (empleadoEnkontrol == null)
                                {
                                    using (var _gcplan = new MainContext(EmpresaEnum.GCPLAN))
                                    {
                                        empleadoEnkontrol = _gcplan.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == claveEmpleado);
                                        puestoEnkontrol = _gcplan.tblRH_EK_Puestos.FirstOrDefault(e => e.puesto == empleadoEnkontrol.puesto);
                                    }
                                }
                                else 
                                {
                                    puestoEnkontrol = _context.tblRH_EK_Puestos.FirstOrDefault(e => e.puesto == empleadoEnkontrol.puesto);
                                }

                                //var puestoEnkontrol = ((List<dynamic>)ContextEnKontrolNomina.Where(
                                //    string.Format(@"SELECT * FROM si_puestos WHERE puesto = {0}", (int)empleadoEnkontrol.puesto)
                                //).ToObject<List<dynamic>>())[0];

                                var nombreEmpleado = string.Format(@"{0} {1} {2}", (string)empleadoEnkontrol.nombre, (string)empleadoEnkontrol.ape_paterno, (string)empleadoEnkontrol.ape_materno);

                                string nombreCarpetaExpedienteEmpleado = ObtenerFormatoCarpetaExpediente(claveEmpleado, nombreEmpleado);
                                var rutaCarpetaExpedienteEmpleado = Path.Combine(RutaExpedientes, nombreCarpetaExpedienteEmpleado);

                                // Verifica si existe la carpeta y si no, la crea.
                                if (verificarExisteCarpeta(rutaCarpetaExpedienteEmpleado, true) == false)
                                {
                                    throw new Exception("No se pudo crear la carpeta en el servidor.");
                                }

                                var rutaExamenInicial = "";
                                var rutaExamenFinal = "";
                                var examenID = 0;

                                var examenSIGOPLAN = _context.tblS_CapacitacionSeguridadCursosExamen.FirstOrDefault(x =>
                                    x.isActivo && x.tipoExamen == 1 //Se colocan todos como examen base 1 a decisión de Diego Cárdenas.
                                    && x.curso_id == cursoSIGOPLAN.id && x.division == divisionActual
                                );

                                if (examenSIGOPLAN == null)
                                {
                                    throw new Exception("No se encuentra la información del examen.");
                                }

                                examenID = examenSIGOPLAN.id;

                                if (examenConsecutivo == "Ex1")
                                {
                                    var nombreExamenDiagnostico = ObtenerFormatoNombreExamen(archivo.Name, TipoExamenEnum.Diagnostico);
                                    rutaExamenInicial = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreExamenDiagnostico);

                                    // Checar existencia archivo para examen inicial
                                    if (File.Exists(rutaExamenInicial))
                                    {
                                        rutaExamenInicial = nuevoNombreArchivoConsecutivo(rutaExamenInicial);
                                    }

                                    archivo.ExtractToFile(Path.Combine(rutaExamenInicial));

                                    if (File.Exists(rutaExamenInicial) == false)
                                    {
                                        throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                                    }
                                }
                                else if (examenConsecutivo == "Ex2")
                                {
                                    var nombreExamenFinal = ObtenerFormatoNombreExamen(archivo.Name, TipoExamenEnum.Final);
                                    rutaExamenFinal = Path.Combine(rutaCarpetaExpedienteEmpleado, nombreExamenFinal);

                                    // Checar existencia archivo para examen final
                                    if (File.Exists(rutaExamenFinal))
                                    {
                                        rutaExamenFinal = nuevoNombreArchivoConsecutivo(rutaExamenFinal);
                                    }

                                    archivo.ExtractToFile(Path.Combine(rutaExamenFinal));

                                    if (File.Exists(rutaExamenFinal) == false)
                                    {
                                        throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                                    }
                                }

                                #region Verifica si existe el registro del asistente
                                var controlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.FirstOrDefault(
                                    x => x.claveEmpleado == claveEmpleado && x.controlAsistenciaID == controlAsistenciaSIGOPLAN.id
                                );

                                if (controlAsistenciaDetalle == null)
                                {
                                    #region Crear registro del detalle de control de asistencia
                                    tblS_CapacitacionSeguridadControlAsistenciaDetalle nuevoControlAsistenciaDetalle = new tblS_CapacitacionSeguridadControlAsistenciaDetalle();

                                    nuevoControlAsistenciaDetalle.claveEmpleado = claveEmpleado;
                                    nuevoControlAsistenciaDetalle.puesto = (string)puestoEnkontrol.descripcion;
                                    nuevoControlAsistenciaDetalle.cc = (string)empleadoEnkontrol.cc_contable;
                                    nuevoControlAsistenciaDetalle.examenID = examenID;
                                    nuevoControlAsistenciaDetalle.estatus = estadoEvaluacion.ToUpper() == "APROBADO" ?
                                        (int)EstatusEmpledoControlAsistenciaEnum.Aprobado : (int)EstatusEmpledoControlAsistenciaEnum.Reprobado;
                                    nuevoControlAsistenciaDetalle.calificacion = calificacion;

                                    if (vSesiones.sesionCapacitacionOperativa)
                                    {
                                        nuevoControlAsistenciaDetalle.estatusAutorizacion = (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado;
                                    }
                                    else
                                    {
                                        nuevoControlAsistenciaDetalle.estatusAutorizacion = controlAsistenciaSIGOPLAN.curso.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad ?
                                        (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.PendienteAutorizacion : (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica;
                                    }

                                    nuevoControlAsistenciaDetalle.rutaExamenInicial = examenConsecutivo == "Ex1" ? rutaExamenInicial : null;
                                    nuevoControlAsistenciaDetalle.rutaExamenFinal = examenConsecutivo == "Ex2" ? rutaExamenFinal : null;
                                    nuevoControlAsistenciaDetalle.controlAsistenciaID = controlAsistenciaSIGOPLAN.id;
                                    nuevoControlAsistenciaDetalle.division = divisionActual;

                                    _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Add(nuevoControlAsistenciaDetalle);
                                    _context.SaveChanges();
                                    #endregion
                                }
                                else
                                {
                                    #region Editar registro del detalle de control de asistencia
                                    controlAsistenciaDetalle.rutaExamenInicial = examenConsecutivo == "Ex1" ? rutaExamenInicial : controlAsistenciaDetalle.rutaExamenInicial;
                                    controlAsistenciaDetalle.rutaExamenFinal = examenConsecutivo == "Ex2" ? rutaExamenFinal : controlAsistenciaDetalle.rutaExamenFinal;

                                    _context.SaveChanges();
                                    #endregion
                                }
                                #endregion
                            }
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

        public Dictionary<string, object> guardarCargaMasivaRelacionCursosPuestosAutorizacion(HttpPostedFileBase archivo)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    List<List<string>> tabla = new List<List<string>>();

                    #region Convertir Archivo a Arreglo de bytes.
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
                    #endregion

                    #region Leer Arreglo de bytes.
                    using (MemoryStream stream = new MemoryStream(data))
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        //loop all worksheets
                        foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                        {
                            //loop all rows
                            for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
                            {
                                List<string> fila = new List<string>();

                                //loop all columns in a row
                                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                                {
                                    //add the cell data to the List
                                    if (worksheet.Cells[i, j].Value != null)
                                    {
                                        fila.Add(worksheet.Cells[i, j].Value.ToString());
                                    }
                                    else
                                    {
                                        fila.Add("");
                                    }
                                }

                                if (i > 1 && fila[0] != "")
                                {
                                    tabla.Add(fila);
                                }
                            }
                        }
                    }
                    #endregion

                    foreach (var fila in tabla)
                    {
                        var cursoID = Int32.Parse(fila[0]);
                        var puestoID = Int32.Parse(fila[1]);
                        var claveCurso = fila[2];
                        var aplicaAutorizacion = fila[5].Length > 0 ? true : false;

                        if (aplicaAutorizacion)
                        {
                            var cursoSIGOPLAN = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.isActivo && x.id == cursoID && x.division == divisionActual);

                            if (cursoSIGOPLAN == null)
                            {
                                throw new Exception("No se encuentra la información del curso con la clave \"" + claveCurso + "\".");
                            }

                            var registroSIGOPLAN = _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.FirstOrDefault(x => x.estatus && x.curso_id == cursoID && x.puesto_id == puestoID && x.division == divisionActual);

                            if (registroSIGOPLAN == null)
                            {
                                _context.tblS_CapacitacionSeguridadCursosPuestosAutorizacion.Add(new tblS_CapacitacionSeguridadCursosPuestosAutorizacion
                                {
                                    curso_id = cursoID,
                                    puesto_id = puestoID,
                                    estatus = true,
                                    division = divisionActual
                                });
                                _context.SaveChanges();
                            }
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

        #region Helper methods
        private string ObtenerDescripcionCC(string cc)
        {
            var centroCostos = _context.tblP_CC.FirstOrDefault(x => x.estatus && x.cc == cc);
            return centroCostos != null ? String.Format("{0} - {1}", centroCostos.cc, centroCostos.descripcion) : "";
        }

        private string ObtenerDescripcionCCEmpresa(string cc, int empresa)
        {
            if (empresa == 1 || empresa == 0)
            {
                var centroCostos = _context.tblP_CC.FirstOrDefault(x => x.estatus && x.cc == cc);
                return centroCostos != null ? String.Format("{0} - {1}", centroCostos.cc, centroCostos.descripcion) : "";
            }
            else
            {
                using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var centroCostos = _contextArrendadora.tblP_CC.FirstOrDefault(x => x.estatus && x.cc == cc);
                    return centroCostos != null ? String.Format("{0} - {1}", centroCostos.cc, centroCostos.descripcion) : "";
                }
            }
        }

        /// <summary>
        /// Retorna la fecha actual en un formato válido para nombre de carpeta en Windows.
        /// </summary>
        /// <returns></returns>
        private string ObtenerFormatoCarpetaFechaActual()
        {
            return DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-");
        }

        private string ObtenerFormatoControlAsistencia(string claveCurso, string nombreCurso, DateTime fechaCapacitacion)
        {
            return String.Format("[{0}] {1} {2}", claveCurso, nombreCurso, fechaCapacitacion.ToString("dd-MM-yyyy"));
        }

        /// <summary>
        /// Obtiene el formato de nombre para un examen diagnóstico o final de un asistente a un control de asistencia.
        /// </summary>
        /// <param name="nombreArchivo">Nombre original del archivo (para obtener su extensión)</param>
        /// <param name="tipoExamen">Tipo de examen. Diagnóstico = 1, Final = 2</param>
        /// <returns></returns>
        private string ObtenerFormatoNombreExamen(string nombreArchivo, TipoExamenEnum tipoExamen)
        {
            var nombreBase = tipoExamen == TipoExamenEnum.Diagnostico ? "examen_diagnóstico" : "examen_final";
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(nombreArchivo));
        }

        private string ObtenerFormatoNombreArchivoDC3(string nombreArchivo)
        {
            var nombreBase = "DC_3";
            return String.Format("{0} {1}{2}", nombreBase, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(nombreArchivo));
        }

        /// <summary>
        /// Obtiene el formato de nombre para un examen de un curso.
        /// </summary>
        /// <param name="tipoExamen">Tipo de examen. Diagnóstico = 1, Final = 2</param>
        /// <param name="nombreOriginal">Nombre original del archivo (para obtener su extensión)</param>
        /// <returns></returns>
        private string ObtenerFormatoNombreExamenCurso(string nombreOriginal)
        {
            string uniqueID = Guid.NewGuid().ToString().Substring(0, 7);
            return String.Format("examen{0} {1}{2}", uniqueID, ObtenerFormatoCarpetaFechaActual(), Path.GetExtension(nombreOriginal));
        }

        private string ObtenerFormatoCarpetaExpediente(int claveEmpleado, string nombreEmpleado)
        {
            return String.Format("[{0}] - {1}", claveEmpleado, nombreEmpleado);
        }

        /// <summary>
        /// Obtiene el nombre completo de un empleado en base a su clave de empleado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        private string ObtenerNombreEmpleadoPorClave(int claveEmpleado)
        {
//            var odbc = new OdbcConsultaDTO();
//            odbc.consulta = @"
//                    SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM sn_empleados 
//                    WHERE clave_empleado = ?";
//            odbc.parametros.Add(new OdbcParameterDTO()
//            {
//                nombre = "claveEmpleado",
//                tipo = OdbcType.Decimal,
//                valor = Convert.ToDecimal(claveEmpleado)
//            });
//            List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(EnkontrolAmbienteEnum.Rh, odbc);
            //var empleado = listaEmpleados.FirstOrDefault();

            var empleado = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                    WHERE clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado},
            }).FirstOrDefault();

            if (empleado == null && vSesiones.sesionEmpresaActual == 1) 
            {
                empleado = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT (nombre + ' ' + ape_paterno + ' ' + ape_materno) as label FROM tblRH_EK_Empleados 
                    WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado },
                }).FirstOrDefault();
            }

            return empleado != null ? empleado.label : "INDEFINIDO";
        }

        private LicenciaHabilidadesDTO ObtenerDatosEmpleadoEnKontrol(int claveEmpleado, int empresa)
        {
//            var odbc = new OdbcConsultaDTO();

//            odbc.consulta = @"
//                    SELECT (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as nombreCompleto,
//                    e.nss AS NSS, 
//                    p.descripcion AS puesto,
//                    d.direccion as area
//                    FROM DBA.sn_empleados AS e
//                    INNER JOIN DBA.sn_departamentos AS d ON e.clave_depto = d.clave_depto 
//                    INNER JOIN DBA.si_puestos as p on e.puesto = p.puesto
//                    WHERE e.estatus_empleado = 'A' AND e.clave_empleado = ?";

//            odbc.parametros.Add(new OdbcParameterDTO()
//            {
//                nombre = "claveEmpleado",
//                tipo = OdbcType.Decimal,
//                valor = Convert.ToDecimal(claveEmpleado)
//            });
//            List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(empresa == 1 ? EnkontrolAmbienteEnum.Rh : EnkontrolAmbienteEnum.RhArre, odbc);

            var listaEmpleados = _context.Select<dynamic>(new DapperDTO
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as nombreCompleto,
                                    e.nss AS NSS, 
                                    p.descripcion AS puesto,
                                    d.direccion as area
                                FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto = p.puesto
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @claveEmpleado",
                parametros = new { claveEmpleado}
            });

            if (vSesiones.sesionEmpresaActual == 1) 
            {
                listaEmpleados.AddRange( _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) as nombreCompleto,
                                    e.nss AS NSS, 
                                    p.descripcion AS puesto,
                                    d.direccion as area
                                FROM tblRH_EK_Empleados AS e
                                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto 
                                INNER JOIN tblRH_EK_Puestos as p on e.puesto = p.puesto
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }));
            }

            return listaEmpleados.Select(x => new LicenciaHabilidadesDTO
            {
                nombre = (string)x.nombreCompleto,
                NSS = (string)x.NSS,
                puesto = (string)x.puesto,
                area = (string)x.area
            }).FirstOrDefault();
        }

        /// <summary>
        /// Retorna la clase indicada que debe tener el label de clasificación del curso según su clasificación
        /// </summary>
        /// <param name="clasificacionID"></param>
        /// <returns></returns>
        private string ObtenerClaseSpanClasificacion(ClasificacionCursoEnum clasificacion)
        {
            switch (clasificacion)
            {
                case ClasificacionCursoEnum.ProtocoloFatalidad:
                    return "label-danger";
                case ClasificacionCursoEnum.Normativo:
                case ClasificacionCursoEnum.MandosMedios:
                case ClasificacionCursoEnum.InstructivoOperativo:
                case ClasificacionCursoEnum.TecnicoOperativo:
                    return "label-warning";
                case ClasificacionCursoEnum.Formativo:
                case ClasificacionCursoEnum.General:
                    return "label-default";
                default:
                    return "label-default";
            }
        }

        private int ObtenerExamenIdAleatorio(List<tblS_CapacitacionSeguridadCursosExamen> examenesCurso)
        {
            int index = RandomInteger(0, examenesCurso.Count);
            return examenesCurso[index].id;
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

        private int ObtenerTipoExamen(int examenID)
        {
            var examen = _context.tblS_CapacitacionSeguridadCursosExamen.FirstOrDefault(x => x.id == examenID && x.division == divisionActual);

            if (examen == null)
            {
                return -1;
            }
            return examen.tipoExamen;
        }

        /// <summary>
        /// Crea registros de autorización electrónica para un control de asistencia indicado.
        /// </summary>
        /// <param name="controlAsistencia"></param>
        private void CrearRegistrosAutorizantes(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia, int jefeID, int coordinadorID, int secretarioID, int gerenteID)
        {
            var puestos = Infrastructure.Utils.EnumExtensions.ToCombo<PuestoAutorizanteEnum>();

            var estatusPendiente = EstatusAutorizacionCapacitacion.Pendiente;

            foreach (var puestoAutorizante in puestos)
            {
                var registroAutorizacion = new tblS_CapacitacionSeguridadAutorizacion
                {
                    controlAsistenciaID = controlAsistencia.id,
                    estatus = (int)estatusPendiente,
                    orden = puestoAutorizante.Value.ParseInt(),
                    tipoPuesto = (int)puestoAutorizante.Value,
                    division = divisionActual
                };

                switch ((PuestoAutorizanteEnum)puestoAutorizante.Value)
                {
                    case PuestoAutorizanteEnum.CoordinadorCMCAP:
                        registroAutorizacion.usuarioID = jefeID;
                        break;
                    case PuestoAutorizanteEnum.CoordinadorCSH:
                        registroAutorizacion.usuarioID = coordinadorID;
                        break;
                    case PuestoAutorizanteEnum.SecretarioCSH:
                        registroAutorizacion.usuarioID = secretarioID;
                        break;
                    case PuestoAutorizanteEnum.GerenteProyecto:
                        registroAutorizacion.usuarioID = gerenteID;
                        break;
                    default:
                        throw new NotImplementedException("Tipo de autorizante no definido.");
                }

                _context.tblS_CapacitacionSeguridadAutorizacion.Add(registroAutorizacion);
            }

            AgregarAccesoVistaAutorizacion(new List<int> { jefeID, coordinadorID, secretarioID, gerenteID });
        }

        /// <summary>
        /// En caso de que un usuario señalado como autorizante no tenga acceso a la vista de autorización, se le agrega dicho acceso.
        /// </summary>
        /// <param name="listaUsuariosIDs"></param>
        private void AgregarAccesoVistaAutorizacion(List<int> listaUsuariosIDs)
        {
            foreach (var usuarioID in listaUsuariosIDs)
            {
                var tieneAccesoVista = _context.tblP_MenutblP_Usuario.Any(x => x.tblP_Usuario_id == usuarioID && x.tblP_Menu_id == VistaAutorizacionID);

                if (tieneAccesoVista == false)
                {
                    var accesoVistaPadre = new tblP_MenutblP_Usuario { tblP_Menu_id = MenuCapacitacionPadreID, sistema = SistemaMenuID, tblP_Usuario_id = usuarioID };
                    var accesoVistaAutorizacion = new tblP_MenutblP_Usuario { tblP_Menu_id = VistaAutorizacionID, sistema = SistemaMenuID, tblP_Usuario_id = usuarioID };
                    _context.tblP_MenutblP_Usuario.Add(accesoVistaPadre);
                    _context.tblP_MenutblP_Usuario.Add(accesoVistaAutorizacion);
                }
            }

            _context.SaveChanges();
        }

        private Tuple<string, string> ObtenerDepartamentoCCPorClaveEmpleado(int clave)
        {
            try
            {
//                string query =
//                @"
//                SELECT 
//                    d.direccion as departamento,
//                    d.cc
//                FROM DBA.sn_empleados AS e
//                    INNER JOIN DBA.sn_departamentos AS d ON e.clave_depto = d.clave_depto
//                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = ?";

//                var odbc = new OdbcConsultaDTO()
//                {
//                    consulta = query,
//                    parametros = new List<OdbcParameterDTO>
//                    {
//                        new OdbcParameterDTO() { nombre = "clave", tipo = OdbcType.Int, valor = clave }
//                    }
//                };
                var empresaActual = vSesiones.sesionEmpresaActual;
                List<dynamic> listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)empresaActual,
                    consulta = @"SELECT 
                                    d.direccion as departamento,
                                    d.cc
                                FROM tblRH_EK_Empleados AS e
                                    INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @clave",
                    parametros = new { clave}
                });

                if (vSesiones.sesionEmpresaActual == 1) 
                {
                    listaEmpleadosCP.AddRange( _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT 
                                    d.direccion as departamento,
                                    d.cc
                                FROM tblRH_EK_Empleados AS e
                                    INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @clave",
                        parametros = new { clave }
                    }));
                }



                List<dynamic> listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT 
                                    d.direccion as departamento,
                                    d.cc
                                FROM tblRH_EK_Empleados AS e
                                    INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @clave",
                    parametros = new { clave }
                });

                // Juntamos los resultados de ambas consultas.
                listaEmpleadosCP.AddRange(listaEmpleadosARR);

                var resultado = listaEmpleadosCP.Select(x => new Tuple<string, string>((string)x.departamento, (string)x.cc)).FirstOrDefault();

                return resultado;
            }
            catch (Exception)
            {
                return new Tuple<string, string>("", "");
            }
        }
        // AQUI
        private void CrearAlertaAutorizacion(string mensaje, List<int> listaUsuariosRecibe, int controlAsistenciaID)
        {
            foreach (var usuarioRecibeID in listaUsuariosRecibe)
            {
                var alertaFacultamiento = new tblP_Alerta
                {
                    msj = mensaje,
                    sistemaID = SistemaMenuID,
                    tipoAlerta = (int)AlertasEnum.REDIRECCION,
                    url = "/Administrativo/CapacitacionSeguridad/AutorizacionCapacitacion",
                    userEnviaID = vSesiones.sesionUsuarioDTO.id,
                    userRecibeID = usuarioRecibeID,
                    objID = controlAsistenciaID
                };
                _context.tblP_Alerta.Add(alertaFacultamiento);
            }
            _context.SaveChanges();
        }

        private void DesactivarAlertaAutorizacion(int controlAsistenciaID, List<int> listaUsuariosRecibe)
        {
            foreach (var usuarioRecibeID in listaUsuariosRecibe)
            {
                var alerta = _context.tblP_Alerta.FirstOrDefault(x =>
                    x.url.Contains("/Administrativo/CapacitacionSeguridad/AutorizacionCapacitacion") &&
                    x.objID == controlAsistenciaID &&
                    x.visto == false &&
                    x.userRecibeID == usuarioRecibeID);

                if (alerta != null)
                {
                    alerta.visto = true;
                }
            }
            _context.SaveChanges();
        }

        private string ObtenerRutaFormatoAutorizacion(tblS_CapacitacionSeguridadControlAsistencia controlAsistencia)
        {
            var rutaCarpetaControlAsistencia = Path.Combine(RutaControlAsistencias, controlAsistencia.nombreCarpeta);

            var nombreArchivo = String.Format("{0} {1}{2}",
                NombreBaseFormatoAutorizacion,
                ObtenerFormatoCarpetaFechaActual(),
                ".pdf");

            var rutaArchivo = Path.Combine(rutaCarpetaControlAsistencia, nombreArchivo);

            return rutaArchivo;
        }

        /// <summary>
        /// Se obtienen todos los cursos en donde el asistente haya aprobado, y en caso de aplicar autorización, lo hayan autorizado.
        /// </summary>
        /// <param name="claveEmpleado"></param>
        /// <returns></returns>
        private object ObtenerCursosVigentesEmpleado(int claveEmpleado)
        {
            //
            var cap = _context.tblS_CapacitacionSeguridadControlAsistencia
                .Where(x => x.division == divisionActual).ToList().Where(x =>
                    x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                    x.asistentes.
                        Any(y =>
                            y.claveEmpleado == claveEmpleado &&
                            y.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado &&
                            (y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado || y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica)))
                .OrderByDescending(x => x.fechaCapacitacion)
                .Select(x => new CurosVigentesDTO
                {
                    id = x.id,
                    claveCurso = x.curso.claveCurso,
                    nombre = x.curso.nombre,
                    lugar = x.lugar,
                    clasificacionEnum = (ClasificacionCursoEnum)x.curso.clasificacion,
                    esExterno = x.esExterno,
                    usuarioInstructor = x.instructor,
                    fechaCapacitacionDate = x.fechaCapacitacion,
                    fechaVigenciaDate = x.fechaCapacitacion,
                    externo = x.instructorExterno
                }).ToList();

            foreach (var item in cap)
            {
                item.clasificacion = item.clasificacionEnum.GetDescription();
                item.instructor = item.esExterno ? item.externo : GlobalUtils.ObtenerNombreCompletoUsuario(item.usuarioInstructor);
                item.fechaCapacitacion = item.fechaCapacitacionDate.ToShortDateString();
                item.fechaVigencia = item.fechaVigenciaDate.AddYears(1).ToShortDateString();
                item.usuarioInstructor = null;
            }

            return cap;
            //

            return _context.tblS_CapacitacionSeguridadControlAsistencia
                     .Where(x => x.division == divisionActual)
                     .ToList()
                     .Where(x => x.estatus == (int)EstatusControlAsistenciaEnum.Completa && CursoAprobadoEmpleado(x.asistentes, claveEmpleado))
                     .OrderByDescending(x => x.fechaCapacitacion)
                     .Select(x => new
                     {
                         x.id,
                         claveCurso = x.curso.claveCurso,
                         x.curso.nombre,
                         clasificacion = ((ClasificacionCursoEnum)x.curso.clasificacion).GetDescription(),
                         x.lugar,
                         instructor = x.esExterno ? x.instructorExterno : GlobalUtils.ObtenerNombreCompletoUsuario(x.instructor),
                         fechaCapacitacion = x.fechaCapacitacion.ToShortDateString(),
                         fechaVigencia = x.fechaCapacitacion.AddYears(1).ToShortDateString()
                     })
                     .ToList();
        }

        private Tuple<string, string>[] ObtenerCursosLicenciaEmpleado(int claveEmpleado)
        {
            int totalCursosEnLicencia = 9;

            return _context.tblS_CapacitacionSeguridadControlAsistencia
                .ToList()
                .Where(x => x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                         (x.curso.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad || x.curso.clasificacion == (int)ClasificacionCursoEnum.TecnicoOperativo) && x.division == divisionActual && CursoAprobadoEmpleado(x.asistentes, claveEmpleado))
                .Where(x => x.fechaCapacitacion.AddYears(1) >= DateTime.Today)
                .Select(x =>
                    new Tuple<string, string>(x.curso.nombre, x.fechaCapacitacion.AddYears(1).ToShortDateString()))
                .DistinctBy(x => x.Item1)
                .OrderByDescending(x => x.Item2)
                .Take(totalCursosEnLicencia)
                .ToArray<Tuple<string, string>>();
        }

        private List<PorcentajeCapacitacionDTO> ObtenerListaPorcentajeCapacitacion(int claveEmpleado, int puestoID, out string porcentajeGeneral)
        {
            var listaPorcentaje = new List<PorcentajeCapacitacionDTO>();
            var clasificaciones = EnumExtensions.ToArray<ClasificacionCursoEnum>();
            var hashSet = new HashSet<string>();
            var ccEnkontrol = ObtenerDepartamentoCCPorClaveEmpleado(claveEmpleado).Item2;

            // Se itera sobre cada tipo de clasificación para sacar el porcentaje de capacitación de cada una
            foreach (var clasificacion in clasificaciones)
            {
                var porcentajeDTO = new PorcentajeCapacitacionDTO
                {
                    clasificacionDesc = clasificacion.GetDescription(),
                    clasificacion = clasificacion
                };

                hashSet.Clear();

                // Se obtienen a todos los cursos activos de esa clasificacion
                var cursosClasificacion = _context.tblS_CapacitacionSeguridadCursos
                    .Where(x => x.isActivo && x.division == divisionActual)
                    .ToList().Where(x => x.estatus == (int)EstatusCursoEnum.Completo && x.clasificacion == (int)clasificacion).ToList();

                // Se filtran los cursos aprobados de esa clasificación.
                //
                var cursosAprobadosClasificacion = _context.tblS_CapacitacionSeguridadControlAsistencia.ToList()
                    .Where(x =>
                        x.curso.clasificacion == (int)clasificacion &&
                        x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                        x.division == divisionActual &&
                        x.asistentes
                        .Any(y =>
                            y.claveEmpleado == claveEmpleado &&
                            y.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado &&
                            (y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado || y.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica)))
                        .ToList();
                //
                //var cursosAprobadosClasificacion = _context.tblS_CapacitacionSeguridadControlAsistencia
                //    .Where(x => x.curso.clasificacion == clasificacion && x.estatus == EstatusControlAsistenciaEnum.Completa && x.division == divisionActual)
                //    .ToList()
                //    .Where(x => CursoAprobadoEmpleado(x.asistentes, claveEmpleado)).ToList();

                var cursosAprobadosMasRecientes = new List<int>();

                // En caso de tener cursos repetidos, se filtra por el aprobado más reciente
                cursosAprobadosClasificacion
                    .OrderByDescending(x => x.fechaCapacitacion)
                    .ForEach(x =>
                    {
                        if (hashSet.Contains(x.curso.claveCurso) == false)
                        {
                            cursosAprobadosMasRecientes.Add(x.id);
                            hashSet.Add(x.curso.claveCurso);
                        }
                    });

                cursosAprobadosClasificacion = cursosAprobadosClasificacion.Where(x => cursosAprobadosMasRecientes.Contains(x.id)).ToList();

                //if (clasificacion == ClasificacionCursoEnum.General || clasificacion == ClasificacionCursoEnum.Formativo)
                //{
                //    // Los cursos generales aplican a todos los puestos.
                //    porcentajeDTO.cursosAplican = cursosClasificacion.Count;
                //    porcentajeDTO.cursosVigentes = cursosAprobadosClasificacion.Count();
                //}
                //else
                //{
                // Todos los cursos activos de esa clasificación que apliquen a su puesto
                var listaCursosAplican = cursosClasificacion.Where(x =>
                    x.aplicaTodosPuestos ||
                    (
                        x.Puestos.Any(y => y.estatus && y.division == divisionActual && y.puesto_id == puestoID) &&
                        x.CentrosCosto.Any(y => y.estatus && y.division == divisionActual && y.cc == ccEnkontrol)
                    )
                ).ToList();
                porcentajeDTO.cursosAplican = listaCursosAplican.Count();

                // Todos los cursos activos de esa clasificación que apliquen a su puesto,
                // que los haya aprobado y que estén vigentes.
                var listaCursosAplicables = cursosAprobadosClasificacion.Where(x =>
                    x.curso.aplicaTodosPuestos ||
                    (
                        x.curso.Puestos.Any(y => y.estatus && y.division == divisionActual && y.puesto_id == puestoID) &&
                        x.curso.CentrosCosto.Any(y => y.estatus && y.division == divisionActual && y.cc == ccEnkontrol) //x.cc == ccEnkontrol
                    )
                ).ToList();
                var listaCursosVigentes = listaCursosAplicables.Where(x => DateTime.Now <= x.fechaCapacitacion.AddYears(1)).ToList();

                porcentajeDTO.cursosVigentes = listaCursosVigentes.Count(); // Que estén vigentes (1 año)
                //}

                if (porcentajeDTO.cursosAplican <= 0)
                {
                    continue;
                }

                porcentajeDTO.porcentajeCapacitacion = ObtenerCadenaPorcentaje(porcentajeDTO.cursosAplican, porcentajeDTO.cursosVigentes);

                listaPorcentaje.Add(porcentajeDTO);
            }

            var totalCursosAplican = listaPorcentaje.Sum(x => x.cursosAplican);
            var totalCursosVigentes = listaPorcentaje.Sum(x => x.cursosVigentes);
            porcentajeGeneral = ObtenerCadenaPorcentaje(totalCursosAplican, totalCursosVigentes);

            return listaPorcentaje;
        }

        private string ObtenerCadenaPorcentaje(int divisor, int dividendo)
        {
            if (divisor <= 0)
            {
                return "0.00 %";
            }

            float porcentaje = ((float)dividendo / (float)divisor) * 100;

            return String.Format(@"{0:0.00} %", porcentaje);
        }

        private bool CursoAprobadoEmpleado(List<tblS_CapacitacionSeguridadControlAsistenciaDetalle> asistentes, int claveEmpleado)
        {
            return asistentes.Any(asistente =>
            {
                if (asistente.claveEmpleado == claveEmpleado)
                {
                    if (asistente.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado)
                    {
                        if (asistente.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.Autorizado ||
                            asistente.estatusAutorizacion == (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            });
        }

        private static void CopiarArchivosCarpetasExpediente(string rutaFolderTemp, List<CarpetaExpedienteDTO> listaCarpetas)
        {
            listaCarpetas.GroupBy(x => new { x.year, x.clasificacion })
                .ForEach(agrupado =>
                {
                    // Carpeta año
                    var year = agrupado.Key.year;
                    var rutaCarpetaAnio = Path.Combine(rutaFolderTemp, year.ToString());
                    verificarExisteCarpeta(rutaCarpetaAnio, true);

                    // Carpeta clasificación
                    var clasificacion = agrupado.Key.clasificacion.GetDescription();
                    var rutaCarpetaClasificacion = Path.Combine(rutaCarpetaAnio, clasificacion);
                    verificarExisteCarpeta(rutaCarpetaClasificacion, true);

                    foreach (var carpetaDTO in agrupado)
                    {
                        var rutaCarpetaControlAsistencia = Path.Combine(rutaCarpetaClasificacion, carpetaDTO.nombreControlAsistencia);
                        Directory.CreateDirectory(rutaCarpetaControlAsistencia);

                        // Lista de asistencia
                        var listaAsistencia =
                            String.Format("{0}{1}", NombreBaseControlAsistencia, Path.GetExtension(carpetaDTO.rutaListaAsistencia));
                        var rutaListaAsistencia = Path.Combine(rutaCarpetaControlAsistencia, listaAsistencia);

                        #region Checar existencia archivo para lista asistencia
                        if (File.Exists(rutaListaAsistencia))
                        {
                            rutaListaAsistencia = nuevoNombreArchivoConsecutivo(rutaListaAsistencia);
                        }
                        #endregion

                        File.Copy(carpetaDTO.rutaListaAsistencia, rutaListaAsistencia);

                        // Lista de autorización
                        if (String.IsNullOrEmpty(carpetaDTO.rutaListaAutorizacion) == false)
                        {
                            var listaAutorizacion =
                                String.Format("{0}{1}", NombreBaseFormatoAutorizacion, Path.GetExtension(carpetaDTO.rutaListaAutorizacion));
                            var rutaListaAutorizacion = Path.Combine(rutaCarpetaControlAsistencia, listaAutorizacion);

                            #region Checar existencia archivo para lista autorización
                            if (File.Exists(rutaListaAutorizacion))
                            {
                                rutaListaAutorizacion = nuevoNombreArchivoConsecutivo(rutaListaAutorizacion);
                            }
                            #endregion

                            File.Copy(carpetaDTO.rutaListaAutorizacion, rutaListaAutorizacion);
                        }

                        // Examen inicial
                        if (String.IsNullOrEmpty(carpetaDTO.rutaExamenInicial) == false)
                        {
                            var examenInicial =
                                String.Format("ExamenInicial{0}", Path.GetExtension(carpetaDTO.rutaExamenInicial));
                            var rutaExamenInicial = Path.Combine(rutaCarpetaControlAsistencia, examenInicial);

                            #region Checar existencia archivo para examen inicial
                            if (File.Exists(rutaExamenInicial))
                            {
                                rutaExamenInicial = nuevoNombreArchivoConsecutivo(rutaExamenInicial);
                            }
                            #endregion

                            File.Copy(carpetaDTO.rutaExamenInicial, rutaExamenInicial);
                        }

                        // Examen final
                        if (String.IsNullOrEmpty(carpetaDTO.rutaExamenFinal) == false)
                        {
                            var examenFinal =
                                String.Format("ExamenFinal{0}", Path.GetExtension(carpetaDTO.rutaExamenFinal));
                            var rutaExamenFinal = Path.Combine(rutaCarpetaControlAsistencia, examenFinal);

                            #region Checar existencia archivo para examen final
                            if (File.Exists(rutaExamenFinal))
                            {
                                rutaExamenFinal = nuevoNombreArchivoConsecutivo(rutaExamenFinal);
                            }
                            #endregion

                            File.Copy(carpetaDTO.rutaExamenFinal, rutaExamenFinal);
                        }
                    }
                });
        }

        private static void CopiarArchivosExtracurricularesCarpetaExpediente(string rutaCarpetaExtracurricular, List<tblS_CapacitacionSeguridadExtracurricular> listaExtracurriculares)
        {
            foreach (var extracurricular in listaExtracurriculares)
            {
                var nombreArchivo = Path.GetFileName(extracurricular.nombre);

                var extension = Path.GetExtension(extracurricular.rutaEvidencia);

                var nuevoNombreArchivo = String.Format("{0}{1}", nombreArchivo, extension);

                var nuevaRutaArchivo = Path.Combine(rutaCarpetaExtracurricular, nuevoNombreArchivo);

                #region Checar existencia archivo
                if (File.Exists(nuevaRutaArchivo))
                {
                    nuevaRutaArchivo = nuevoNombreArchivoConsecutivo(nuevaRutaArchivo);
                }
                #endregion

                File.Copy(extracurricular.rutaEvidencia, nuevaRutaArchivo);
            }
        }

        private static void CopiarArchivosActosCarpetaExpediente(string rutaCarpetaActos, List<tblSAC_Acto> listaActos)
        {
            foreach (var acto in listaActos)
            {
                string tipoActo = acto.tipoActo.GetDescription();

                var nombreArchivo = Path.GetFileName(acto.rutaEvidencia);

                var nuevoNombreArchivo = String.Format("Acto {0} - {1}", tipoActo, nombreArchivo);

                var nuevaRutaArchivo = Path.Combine(rutaCarpetaActos, nuevoNombreArchivo);

                #region Checar existencia archivo
                if (File.Exists(nuevaRutaArchivo))
                {
                    nuevaRutaArchivo = nuevoNombreArchivoConsecutivo(nuevaRutaArchivo);
                }
                #endregion

                File.Copy(acto.rutaEvidencia, nuevaRutaArchivo);
            }
        }

        private List<CarpetaExpedienteDTO> CrearCarpetasExpediente(int claveEmpleado, List<tblS_CapacitacionSeguridadControlAsistencia> capacitaciones)
        {
            var listaCarpetas = new List<CarpetaExpedienteDTO>();

            foreach (var capacitacion in capacitaciones)
            {
                var curso = capacitacion.curso;

                var nombreCarpetaControlAsistencia =
                    ObtenerFormatoControlAsistencia(curso.claveCurso, curso.nombre, capacitacion.fechaCapacitacion);

                var carpetaDTO = new CarpetaExpedienteDTO
                {
                    nombreControlAsistencia = nombreCarpetaControlAsistencia,
                    year = capacitacion.fechaCapacitacion.Year,
                    clasificacion = (ClasificacionCursoEnum)curso.clasificacion
                };

                if (capacitacion.rutaListaAsistencia != null)
                {
                    carpetaDTO.rutaListaAsistencia = capacitacion.rutaListaAsistencia;
                }

                if (capacitacion.curso.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad ||
                    capacitacion.curso.clasificacion == (int)ClasificacionCursoEnum.Normativo ||
                    capacitacion.curso.clasificacion == (int)ClasificacionCursoEnum.MandosMedios ||
                    capacitacion.curso.clasificacion == (int)ClasificacionCursoEnum.InstructivoOperativo ||
                    capacitacion.curso.clasificacion == (int)ClasificacionCursoEnum.TecnicoOperativo)
                {
                    var asistente = capacitacion.asistentes.FirstOrDefault(x => x.claveEmpleado == claveEmpleado);
                    carpetaDTO.rutaExamenInicial = asistente.rutaExamenInicial;
                    carpetaDTO.rutaExamenFinal = asistente.rutaExamenFinal;
                }

                if (capacitacion.rutaListaAutorizacion != null && EmpleadoAplicaAutorizacion(capacitacion.asistentes, claveEmpleado))
                {
                    carpetaDTO.rutaListaAutorizacion = capacitacion.rutaListaAutorizacion;
                }

                listaCarpetas.Add(carpetaDTO);
            }

            return listaCarpetas;
        }

        private bool EmpleadoAplicaAutorizacion(List<tblS_CapacitacionSeguridadControlAsistenciaDetalle> asistentes, int claveEmpleado)
        {
            return asistentes.Any(x =>
                x.claveEmpleado == claveEmpleado &&
                x.estatusAutorizacion != (int)EstatusAutorizacionEmpleadoControlAsistenciaEnum.NoAplica && x.estatus == (int)EstatusEmpledoControlAsistenciaEnum.Aprobado);
        }

        private bool EmpleadoTieneExpediente(int claveEmpleado)
        {
            var capacitaciones = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle
                .Where(x => x.claveEmpleado == claveEmpleado && x.controlAsistencia.estatus != (int)EstatusControlAsistenciaEnum.PendienteCargaAsistencia && x.division == divisionActual)
                .ToList();

            return capacitaciones.Count > 0;
        }

        /// <summary>
        /// Verifica si el nombre contiene caracteres inválidos para carpetas.
        /// </summary>
        /// <param name="nombreCarpeta">Nombre de la carpeta a verificar.</param>
        /// <returns>Verdadero si la carpeta contiene caracteres inválidos.</returns>
        private bool EsNombreCarpetaInvalido(string nombreCarpeta)
        {

            string invalidFileNameRegex = @"[^a-zA-Z0-9áéíóúüñÑ_.\- ]+";
            return Regex.Match(nombreCarpeta, invalidFileNameRegex, RegexOptions.IgnoreCase).Success;
        }

        private static string nuevoNombreArchivoConsecutivo(string rutaArchivo)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(rutaArchivo);
            string extension = Path.GetExtension(rutaArchivo);
            string path = Path.GetDirectoryName(rutaArchivo);
            string newFullPath = rutaArchivo;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0} ({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }

        #endregion

        public Dictionary<string, object> ObtenerComboCCAmbasEmpresas()
        {
            try
            {
                var listaCCDivision = _context.tblS_CapacitacionSeguridadCentroCostoDivision.Where(x => x.estatus && x.division == divisionActual).ToList();

                string query = @"
                    SELECT
                        cc as Value,
                        (cc + ' - ' + descripcion) as Text
                    FROM cc
                    ORDER BY cc";

                var odbc = new OdbcConsultaDTO() { consulta = query };
                //var listaConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, odbc);
                var listaArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenProd, odbc);

                var listaCC = new List<ComboGroupDTO>();
                

                //if (listaConstruplan.Count() > 0)
                //{
                //    listaCC.Add(new ComboGroupDTO { label = "CONSTRUPLAN", options = listaConstruplan });
                //}

                #region Construplan
                //Se agarra la información de la tabla en SIGOPLAN para que coincida la información en los cursos ya capturados.
                var privilegio = getPrivilegioActual();
                var lstCc = _context.tblP_CC_Usuario.ToList().Where(w => w.usuarioID == privilegio.idUsuario).ToList();
                var listaConstruplan = _context.tblP_CC.ToList().Where(x => x.estatus).Where(x => privilegio.idPrivilegio == (int)PrivilegioEnum.Administrador ? true : lstCc.Any(a => a.cc == x.cc)).OrderBy(x => x.cc)
                    .Select(centroCosto => new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Text = centroCosto.cc + " - " + centroCosto.descripcion,
                        Value = centroCosto.cc,
                        Prefijo = "1"
                    }).ToList();
                if(vSesiones.sesionEmpresaActual==6)
                {
                    listaConstruplan = _context.tblC_Nom_CatalogoCC.ToList().Where(x => x.estatus).Where(x => privilegio.idPrivilegio == (int)PrivilegioEnum.Administrador ? true : lstCc.Any(a => a.cc == x.cc)).OrderBy(x => x.cc)
                                        .Select(centroCosto => new Core.DTO.Principal.Generales.ComboDTO
                                        {
                                            Text = centroCosto.cc + " - " + centroCosto.ccDescripcion,
                                            Value = centroCosto.cc,
                                            Prefijo = "1"
                                        }).ToList();
                }

             
                    if (listaConstruplan.Count() > 0)
                    {
                        string empresaDesc = "";

                        switch (vSesiones.sesionEmpresaActual)
                        {
                            case 1:
                                empresaDesc = "CONSTRUPLAN";
                                break;
                            case 3:
                                empresaDesc = "COLOMBIA";
                                break;
                            case 6:
                                empresaDesc = "PERÚ";
                                break;
                            default:
                                break;
                        }

                        if (vSesiones.sesionCapacitacionOperativa)
                        {
                            //Filtrar por división
                            listaConstruplan = listaConstruplan.Where(x => listaCCDivision.Where(z => z.empresa == 0 || z.empresa == 1 || z.empresa == 3 || z.empresa == 6).Select(y => y.cc).Contains(x.Value)).ToList();
                        }

                        listaCC.Add(new ComboGroupDTO { label = empresaDesc, options = listaConstruplan });
                    }
                #endregion

                    if (listaArrendadora.Count() > 0)
                    {
                        foreach (var ar in listaArrendadora)
                        {
                            ar.Prefijo = "2";
                        }

                        if (vSesiones.sesionCapacitacionOperativa)
                        {
                            //Filtrar por división
                            listaArrendadora = listaArrendadora.Where(x => listaCCDivision.Where(z => z.empresa == 0 || z.empresa == 2 || z.empresa == 6).Select(y => y.cc).Contains(x.Value)).ToList();
                        }

                        listaCC.Add(new ComboGroupDTO { label = "ARRENDADORA", options = listaArrendadora });
                    }

                    if (listaCC.Count > 0)
                    {
                        resultado.Add(ITEMS, listaCC);
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        resultado.Add(SUCCESS, false);
                        resultado.Add("EMPTY", true);
                    }
                
              
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "ObtenerComboCCAmbasEmpresas", e, AccionEnum.CONSULTA, 0, null);
            }
            return resultado;
        }

        #region Personal Autorizado
        public Dictionary<string, object> getListasAutorizacion(List<int> listaCursos, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCC)
        {
            try
            {
                var listasAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.Where(x => x.estatus && x.division == divisionActual).ToList();

                if (listaCursos != null && listaCursos.Count() > 0)
                {
                    listasAutorizacion = listasAutorizacion.Where(x => listaCursos.Contains(x.cursoID)).ToList();
                }

                //if (listaCC != null && listaCC.Count() > 0)
                //{
                //    var listaCCResultado = new List<tblS_CapacitacionSeguridadListaAutorizacionCC>();
                //    var listasCentrosCosto = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x => x.estatus).ToList();

                //    foreach (var cc in listaCC)
                //    {
                //        var listasCentrosCostoFiltrado = listasCentrosCosto.Where(x => x.cc == cc.cc && x.departamento == cc.departamento && x.empresa == cc.empresa).ToList();

                //        listaCCResultado.AddRange(listasCentrosCostoFiltrado);
                //    }

                //    listasAutorizacion = listasAutorizacion.Where(x => listaCCResultado.Select(y => y.listaAutorizacionID).Distinct().Contains(x.id)).ToList();
                //}

                var cursosSIGOPLAN = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList();

                #region Centros de Costo
                //var listaConstruplan = _context.tblP_CC.ToList().Where(x => x.estatus).Select(centroCosto => new Core.DTO.Principal.Generales.ComboDTO
                //{
                //    Text = centroCosto.cc + " - " + centroCosto.descripcion,
                //    Value = centroCosto.cc,
                //    Prefijo = "1"
                //}).ToList();
                var listaConstruplan = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT
                        cc as Value,
                        (cc + ' - ' + descripcion) as Text
                    FROM cc
                    ORDER BY cc"
                });
                var listaArrendadora = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT
                        cc as Value,
                        (cc + ' - ' + descripcion) as Text
                    FROM cc
                    ORDER BY cc"
                });

                var centrosCostoAmbasEmpresas = new List<Core.DTO.Principal.Generales.ComboDTO>();
                centrosCostoAmbasEmpresas.AddRange(listaConstruplan);
                centrosCostoAmbasEmpresas.AddRange(listaArrendadora);
                #endregion

                #region Departamentos
                var departamentosAmbasEmpresas = new List<DepartamentoDTO>();

                //var odbc = new OdbcConsultaDTO();
                //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc, 1 AS empresa FROM DBA.sn_departamentos"
                //});
                //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc, 2 AS empresa FROM DBA.sn_departamentos"
                //});

                var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO 
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc, 1 AS empresa FROM tblRH_EK_Departamentos"
                    

                });
                var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO 
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc, 1 AS empresa FROM tblRH_EK_Departamentos"

                });

                if (departamentosConstruplan.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new DepartamentoDTO
                    {
                        id = y.id,
                        departamento = y.departamento,
                        cc = y.cc,
                        empresa = y.empresa
                    }).ToList());
                }

                if (departamentosArrendadora.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new DepartamentoDTO
                    {
                        id = y.id,
                        departamento = y.departamento,
                        cc = y.cc,
                        empresa = y.empresa
                    }).ToList());
                }
                #endregion

                List<ListaAutorizacionDTO> datos = new List<ListaAutorizacionDTO>();

                foreach (var aut in listasAutorizacion)
                {
                    var listaCCAut = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x => x.estatus && x.listaAutorizacionID == aut.id).ToList();
                    var listaCCAutResultado = new List<tblS_CapacitacionSeguridadListaAutorizacionCC>();

                    if (listaCC != null && listaCC.Count() > 0)
                    {
                        foreach (var cc in listaCC)
                        {
                            listaCCAutResultado.AddRange(listaCCAut.Where(x => x.cc == cc.cc && x.departamento == cc.departamento && x.empresa == cc.empresa));
                        }
                    }
                    else
                    {
                        listaCCAutResultado.AddRange(listaCCAut);
                    }

                    foreach (var cc in listaCCAutResultado)
                    {
                        datos.Add(new ListaAutorizacionDTO
                        {
                            id = aut.id,
                            claveLista = aut.claveLista,
                            cursoID = aut.cursoID,
                            claveCurso = cursosSIGOPLAN.Where(y => y.id == aut.cursoID).Select(z => z.claveCurso).FirstOrDefault(),
                            cursoNombre = cursosSIGOPLAN.Where(y => y.id == aut.cursoID).Select(z => z.nombre).FirstOrDefault(),
                            revision = aut.revision,
                            cc = cc.cc,
                            ccDesc = centrosCostoAmbasEmpresas.Where(y => y.Value == cc.cc).Select(z => z.Text).FirstOrDefault(),
                            departamento = cc.departamento,
                            departamentoDesc =
                                departamentosAmbasEmpresas.Where(y => y.id == cc.departamento.ToString() && y.cc == cc.cc && y.empresa == cc.empresa).Select(z => z.departamento).FirstOrDefault(),
                            jefeDepartamento = aut.jefeDepartamento,
                            jefeDepartamentoDesc = "",
                            gerenteProyecto = aut.gerenteProyecto,
                            gerenteProyectoDesc = "",
                            coordinadorCSH = aut.coordinadorCSH,
                            coordinadorCSHDesc = "",
                            secretarioCSH = aut.secretarioCSH,
                            secretarioCSHDesc = "",
                            seguridad = aut.seguridad,
                            seguridadDesc = "",
                            fechaCreacion = aut.fechaCreacion,
                            fechaCreacionString = aut.fechaCreacion.ToShortDateString()
                        });
                    }
                }

                resultado.Add("datos", datos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getListasAutorizacion", e, AccionEnum.CONSULTA, 0, new { listaCursos = listaCursos, listaCC = listaCC });
            }

            return resultado;
        }

        public Dictionary<string, object> getListasAutorizacionCombo()
        {
            try
            {
                var listasAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.Where(x => x.estatus && x.division == divisionActual).Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.claveLista
                }).ToList();

                resultado.Add(ITEMS, listasAutorizacion);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getListasAutorizacionCombo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    listaAutorizacion.fechaCreacion = DateTime.Now.Date;
                    listaAutorizacion.division = divisionActual;

                    _context.tblS_CapacitacionSeguridadListaAutorizacion.Add(listaAutorizacion);
                    _context.SaveChanges();

                    foreach (var cc in listaCentrosCosto)
                    {
                        cc.listaAutorizacionID = listaAutorizacion.id;

                        _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Add(cc);
                        _context.SaveChanges();
                    }

                    foreach (var rfc in listaRFC)
                    {
                        rfc.listaAutorizacionID = listaAutorizacion.id;

                        _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.Add(rfc);
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarListaAutorizacion", e, AccionEnum.AGREGAR, 0, listaAutorizacion);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarListaAutorizacion(tblS_CapacitacionSeguridadListaAutorizacion listaAutorizacion, List<tblS_CapacitacionSeguridadListaAutorizacionRFC> listaRFC, List<tblS_CapacitacionSeguridadListaAutorizacionCC> listaCentrosCosto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var listaAutorizacionSIGOPLAN = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacion.id);

                    if (listaAutorizacionSIGOPLAN != null)
                    {
                        listaAutorizacionSIGOPLAN.claveLista = listaAutorizacion.claveLista;
                        listaAutorizacionSIGOPLAN.revision = listaAutorizacion.revision;
                        listaAutorizacionSIGOPLAN.cursoID = listaAutorizacion.cursoID;

                        _context.SaveChanges();

                        #region Centros de Costo - Departamentos
                        var listaCCAnterior = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                        foreach (var cc in listaCCAnterior)
                        {
                            cc.estatus = false;

                            _context.SaveChanges();
                        }

                        foreach (var cc in listaCentrosCosto)
                        {
                            cc.listaAutorizacionID = listaAutorizacion.id;

                            _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Add(cc);
                            _context.SaveChanges();
                        }
                        #endregion

                        #region RFC - Razón Social
                        var listaRFCAnterior = _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                        foreach (var rfc in listaRFCAnterior)
                        {
                            rfc.estatus = false;

                            _context.SaveChanges();
                        }

                        foreach (var rfc in listaRFC)
                        {
                            rfc.listaAutorizacionID = listaAutorizacion.id;

                            _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.Add(rfc);
                            _context.SaveChanges();
                        }
                        #endregion

                        dbContextTransaction.Commit();
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información de la lista de autorización.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "editarListaAutorizacion", e, AccionEnum.ACTUALIZAR, 0, listaAutorizacion);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarListaAutorizacion(int listaAutorizacionID)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var listaAutorizacionSIGOPLAN = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                    if (listaAutorizacionSIGOPLAN != null)
                    {
                        listaAutorizacionSIGOPLAN.estatus = false;

                        _context.SaveChanges();

                        var listaCC = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacionID).ToList();

                        foreach (var cc in listaCC)
                        {
                            cc.estatus = false;

                            _context.SaveChanges();
                        }

                        var listaRFC = _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacionID).ToList();

                        foreach (var rfc in listaRFC)
                        {
                            rfc.estatus = false;

                            _context.SaveChanges();
                        }

                        var listaAsistentes = _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacionID).ToList();

                        foreach (var asis in listaAsistentes)
                        {
                            asis.estatus = false;

                            _context.SaveChanges();
                        }

                        var listaInteresados = _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacionID).ToList();

                        foreach (var inte in listaInteresados)
                        {
                            inte.estatus = false;

                            _context.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                        resultado.Add(SUCCESS, true);
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información de la lista de autorización.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "eliminarListaAutorizacion", e, AccionEnum.ELIMINAR, 0, listaAutorizacionID);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getListaAutorizacionByID(int listaAutorizacionID)
        {
            try
            {
                var listaAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                if (listaAutorizacion != null)
                {
                    #region Departamentos Enkontrol
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"


                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "CONSTRUPLAN" }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "ARRENDADORA" }).ToList());
                    }
                    #endregion

                    var listaCC = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList().Select(x => new ListaAutorizacionCCDTO
                    {
                        id = x.id,
                        cc = x.cc,
                        departamento = x.departamento,
                        departamentoDesc = departamentosAmbasEmpresas.Where(y => y.Value == x.departamento.ToString()).Select(z => z.Text).FirstOrDefault(),
                        empresa = x.empresa,
                        listaAutorizacionID = x.listaAutorizacionID
                    }).ToList();
                    var listaRFC = _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                    #region Lista Empleados
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = @"
                        SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM DBA.sn_empleados AS e
                            INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
                            INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    };

                    //List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                    //List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                    var listaEmpleados = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    });

                    listaEmpleados.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    }));

                    var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"

                    });

                    listaEmpleados.AddRange(listaEmpleadosARR);
                    #endregion

                    #region Lista Asistentes
                    var listaAsistentes = _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.Where(x =>
                        x.estatus && x.listaAutorizacionID == listaAutorizacion.id).Select(x => new ListaAsistentesDTO
                        {
                            id = x.id,
                            claveEmpleado = x.claveEmpleado,
                            listaAutorizacionID = x.listaAutorizacionID
                        }).ToList();

                    foreach (var asis in listaAsistentes)
                    {
                        var empleado = listaEmpleados.FirstOrDefault(x => x.claveEmpleado == asis.claveEmpleado);

                        if (empleado != null)
                        {
                            asis.nombreEmpleado = empleado.nombreEmpleado;
                            asis.puesto = Convert.ToInt32(empleado.puesto);
                            asis.puestoDesc = empleado.puestoDesc;
                            asis.cc = empleado.cc;
                            asis.ccDesc = empleado.ccDesc;
                        }
                    }
                    #endregion

                    #region Lista Interesados
                    var listaInteresados = _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Where(x =>
                        x.estatus && x.listaAutorizacionID == listaAutorizacion.id).Select(x => new ListaInteresadosDTO
                        {
                            id = x.id,
                            claveEmpleado = x.claveEmpleado,
                            listaAutorizacionID = x.listaAutorizacionID
                        }).ToList();

                    foreach (var inte in listaInteresados)
                    {
                        var empleado = listaEmpleados.FirstOrDefault(x => x.claveEmpleado == inte.claveEmpleado);

                        if (empleado != null)
                        {
                            inte.nombreEmpleado = empleado.nombreEmpleado;
                            inte.puesto = Convert.ToInt32(empleado.puesto);
                            inte.puestoDesc = empleado.puestoDesc;
                            inte.cc = empleado.cc;
                            inte.ccDesc = empleado.ccDesc;
                        }
                    }
                    #endregion

                    var datos = new ListaAutorizacionDTO
                    {
                        claveLista = listaAutorizacion.claveLista,
                        revision = listaAutorizacion.revision,
                        cursoID = listaAutorizacion.cursoID,
                        jefeDepartamento = listaAutorizacion.jefeDepartamento,
                        jefeDepartamentoDesc = listaEmpleados.Where(x => x.claveEmpleado == listaAutorizacion.jefeDepartamento).Select(x => x.nombreEmpleado).FirstOrDefault(),
                        gerenteProyecto = listaAutorizacion.gerenteProyecto,
                        gerenteProyectoDesc = listaEmpleados.Where(x => x.claveEmpleado == listaAutorizacion.gerenteProyecto).Select(x => x.nombreEmpleado).FirstOrDefault(),
                        coordinadorCSH = listaAutorizacion.coordinadorCSH,
                        coordinadorCSHDesc = listaEmpleados.Where(x => x.claveEmpleado == listaAutorizacion.coordinadorCSH).Select(x => x.nombreEmpleado).FirstOrDefault(),
                        secretarioCSH = listaAutorizacion.secretarioCSH,
                        secretarioCSHDesc = listaEmpleados.Where(x => x.claveEmpleado == listaAutorizacion.secretarioCSH).Select(x => x.nombreEmpleado).FirstOrDefault(),
                        seguridad = listaAutorizacion.seguridad,
                        seguridadDesc = listaEmpleados.Where(x => x.claveEmpleado == listaAutorizacion.seguridad).Select(x => x.nombreEmpleado).FirstOrDefault(),
                        listaCC = listaCC,
                        listaRFC = listaRFC,
                        listaAsistentes = listaAsistentes,
                        listaInteresados = listaInteresados
                    };

                    resultado.Add("datos", datos);
                }
                else
                {
                    throw new Exception("No se encuentra la información de la lista de autorización.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public object getAutorizanteEnkontrolAutocomplete(string term)
        {
            try
            {
//                string query = @"
//                    SELECT 
//                        e.clave_empleado AS claveEmpleado, 
//                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
//                        p.descripcion AS puestoEmpleado,
//                        e.cc_contable AS ccID,
//                        c.descripcion AS cc
//                    FROM DBA.sn_empleados AS e
//                        INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                        INNER JOIN DBA.cc AS c on e.cc_contable = c.cc
//                    WHERE e.estatus_empleado = 'A' AND nombreEmpleado LIKE ?
//                    ORDER BY e.ape_paterno DESC";

//                var odbc = new OdbcConsultaDTO()
//                {
//                    consulta = query,
//                    parametros = new List<OdbcParameterDTO>
//                    {
//                        new OdbcParameterDTO() { nombre = "term", tipo = OdbcType.VarChar, valor = String.Format("%{0}%", term) }
//                    }
//                };

//                List<dynamic> listaEmpleadosCP = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
//                List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                var listaEmpleadosCP = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"  SELECT 
                        e.clave_empleado AS claveEmpleado, 
                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                        p.descripcion AS puestoEmpleado,
                        e.cc_contable AS ccID,
                        c.descripcion AS cc
                    FROM tblRH_EK_Empleados AS e
                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                        INNER JOIN tblP_CC AS c on e.cc_contable = c.cc
                    WHERE e.estatus_empleado = 'A' AND (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) LIKE @nombre
                    ORDER BY e.ape_paterno DESC",
                    parametros = new { nombre = String.Format("%{0}%", term) }
                });

                listaEmpleadosCP.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                }));

                var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"  SELECT 
                        e.clave_empleado AS claveEmpleado, 
                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                        p.descripcion AS puestoEmpleado,
                        e.cc_contable AS ccID,
                        c.descripcion AS cc
                    FROM tblRH_EK_Empleados AS e
                        INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                        INNER JOIN tblP_CC AS c on e.cc_contable = c.cc
                    WHERE e.estatus_empleado = 'A' AND (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) LIKE @nombre
                    ORDER BY e.ape_paterno DESC",
                    parametros = new { nombre = String.Format("%{0}%", term) }

                });

                listaEmpleadosCP.AddRange(listaEmpleadosARR);

                return listaEmpleadosCP.Select(x => new
                {
                    id = x.claveEmpleado,
                    value = x.nombreEmpleado,
                    nombreEmpleado = (string)x.nombreEmpleado,
                    puestoEmpleado = (string)x.puestoEmpleado,
                    ccID = (string)x.ccID,
                    cc = (string)x.cc
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<string, object> guardarInformacionAutorizados(ListaAutorizacionDTO listaAutorizacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var listaAutorizacionSIGOPLAN = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacion.id);

                    if (listaAutorizacionSIGOPLAN != null)
                    {
                        #region Asistentes
                        #region Se quitan los asistentes anteriores
                        var listaAsistentesAnterior = _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                        foreach (var asis in listaAsistentesAnterior)
                        {
                            asis.estatus = false;

                            _context.SaveChanges();
                        }
                        #endregion

                        foreach (var asis in listaAutorizacion.listaAsistentes)
                        {
                            _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.Add(new tblS_CapacitacionSeguridadListaAutorizacionAsistentes
                            {
                                claveEmpleado = asis.claveEmpleado,
                                fechaVencimiento = null,
                                listaAutorizacionID = listaAutorizacionSIGOPLAN.id,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                        #endregion

                        #region Autorizantes
                        listaAutorizacionSIGOPLAN.jefeDepartamento = listaAutorizacion.jefeDepartamento;
                        listaAutorizacionSIGOPLAN.gerenteProyecto = listaAutorizacion.gerenteProyecto;
                        listaAutorizacionSIGOPLAN.coordinadorCSH = listaAutorizacion.coordinadorCSH;
                        listaAutorizacionSIGOPLAN.secretarioCSH = listaAutorizacion.secretarioCSH;
                        listaAutorizacionSIGOPLAN.seguridad = listaAutorizacion.seguridad;

                        _context.SaveChanges();
                        #endregion

                        #region Interesados
                        #region Se quitan los interesados anteriores.
                        var listaInteresadosAnterior = _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                        foreach (var inte in listaInteresadosAnterior)
                        {
                            inte.estatus = false;

                            _context.SaveChanges();
                        }
                        #endregion

                        foreach (var inte in listaAutorizacion.listaInteresados)
                        {
                            _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Add(new tblS_CapacitacionSeguridadListaAutorizacionInteresados
                            {
                                claveEmpleado = inte.claveEmpleado,
                                listaAutorizacionID = listaAutorizacionSIGOPLAN.id,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                        #endregion
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información de la lista de autorización.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarInformacionAutorizados", e, AccionEnum.AGREGAR, 0, listaAutorizacion);
                }
            }

            return resultado;
        }

        public ListaAutorizacionReporteDTO getListaAutorizacionReporte(int listaAutorizacionID, int razonSocialID, int departamento, string cc, int empresa)
        {
            ListaAutorizacionReporteDTO datos = new ListaAutorizacionReporteDTO();

            try
            {
                var listaAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                if (listaAutorizacion != null)
                {
                    var curso = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.isActivo && x.id == listaAutorizacion.cursoID);

                    if (curso == null)
                    {
                        throw new Exception("No se encuentra la información del curso.");
                    }

                    var departamentoDesc = "";
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    var ccDepartamento = _context.tblS_CapacitacionSeguridadListaAutorizacionCC.Where(x =>
                        x.estatus && x.listaAutorizacionID == listaAutorizacion.id && x.departamento == departamento && x.cc == cc && x.empresa == empresa
                    ).FirstOrDefault();

                    if (empresa == 1)
                    {
                        departamentoDesc = departamentosConstruplan.Where(x => x.id == ccDepartamento.departamento.ToString()).Select(x => x.departamento).FirstOrDefault();
                    }
                    else
                    {
                        departamentoDesc = departamentosArrendadora.Where(x => x.id == ccDepartamento.departamento.ToString()).Select(x => x.departamento).FirstOrDefault();
                    }

                    var rfcSIGOPLAN = _context.tblS_CapacitacionSeguridadListaAutorizacionRFC.FirstOrDefault(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id && x.id == razonSocialID);

                    #region Lista Empleados
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = @"
                        SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM DBA.sn_empleados AS e
                            INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
                            INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    };

                    //List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                    //List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                    var listaEmpleados = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    });

                    listaEmpleados.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                    }));

                    var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"

                    });

                    listaEmpleados.AddRange(listaEmpleadosARR);
                    #endregion

                    #region Lista Asistentes
                    var listaAsistentes = _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.Where(x =>
                        x.estatus && x.listaAutorizacionID == listaAutorizacion.id).Select(x => new ListaAsistentesDTO
                        {
                            id = x.id,
                            claveEmpleado = x.claveEmpleado,
                            listaAutorizacionID = x.listaAutorizacionID
                        }).ToList();

                    foreach (var asis in listaAsistentes)
                    {
                        var empleado = listaEmpleados.FirstOrDefault(x => x.claveEmpleado == asis.claveEmpleado);

                        if (empleado != null)
                        {
                            asis.nombreEmpleado = empleado.nombreEmpleado;
                            asis.puesto = Convert.ToInt32(empleado.puesto);
                            asis.puestoDesc = empleado.puestoDesc;
                            asis.cc = empleado.cc;
                            asis.ccDesc = empleado.ccDesc;
                        }
                    }
                    #endregion

                    datos = new ListaAutorizacionReporteDTO
                    {
                        claveListaAutorizacion = listaAutorizacion.claveLista,
                        revision = listaAutorizacion.revision,
                        codigoCurso = curso.claveCurso,
                        fechaEmision = DateTime.Now.Date.ToShortDateString(),
                        razonSocial = rfcSIGOPLAN != null ? rfcSIGOPLAN.razonSocial : "",
                        rfc = rfcSIGOPLAN != null ? rfcSIGOPLAN.rfc : "",
                        departamento = departamentoDesc,
                        objetivoContenidoCapacitacion =
                            "<p><b>Objetivo:</b> " + curso.objetivo + "</p>" +
                            "<p><b>Contenido de la Capacitación:</b> " + curso.nombre + "</p>",
                        nota = curso.nota,
                        nombreJefe = listaEmpleados.Where(x => Convert.ToInt32(x.claveEmpleado) == listaAutorizacion.jefeDepartamento).Select(x => x.nombreEmpleado).FirstOrDefault() ?? "",
                        firmaJefe = "",
                        nombreGerente = listaEmpleados.Where(x => Convert.ToInt32(x.claveEmpleado) == listaAutorizacion.gerenteProyecto).Select(x => x.nombreEmpleado).FirstOrDefault() ?? "",
                        firmaGerente = "",
                        nombreCoordinador = listaEmpleados.Where(x => Convert.ToInt32(x.claveEmpleado) == listaAutorizacion.coordinadorCSH).Select(x => x.nombreEmpleado).FirstOrDefault() ?? "",
                        firmaCoordinador = "",
                        nombreSecretario = listaEmpleados.Where(x => Convert.ToInt32(x.claveEmpleado) == listaAutorizacion.secretarioCSH).Select(x => x.nombreEmpleado).FirstOrDefault() ?? "",
                        firmaSecretario = "",
                        nombreSeguridad = listaEmpleados.Where(x => Convert.ToInt32(x.claveEmpleado) == listaAutorizacion.seguridad).Select(x => x.nombreEmpleado).FirstOrDefault() ?? "",
                        firmaSeguridad = "",
                        referenciaNormativa = curso.referenciasNormativas,
                        listaAsistentes = listaAsistentes.Select(x => new ListaAsistentesReporteDTO
                        {
                            claveEmpleado = x.claveEmpleado.ToString(),
                            nombreEmpleado = x.nombreEmpleado,
                            puestoDesc = x.puestoDesc,
                            fechaVencimientoString = x.fechaVencimiento != null && x.fechaVencimiento.Year > 1 ? x.fechaVencimiento.ToShortDateString() : ""
                        }).ToList()
                    };
                }
                else
                {
                    throw new Exception("No se encuentra la información de la lista de autorización.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "getListaAutorizacionReporte", e, AccionEnum.CONSULTA, 0, listaAutorizacionID);
            }

            return datos;
        }

        public Dictionary<string, object> cargarDashboardPersonalAutorizado(FiltrosDashboardPersonalAutorizadoDTO filtros)
        {
            try
            {
                if ((filtros.listaCCConstruplan == null || filtros.listaCCConstruplan.Count == 0) && (filtros.listaCCArrendadora == null || filtros.listaCCArrendadora.Count == 0))
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, "La lista de centros de costos viene vacía.");
                    return resultado;
                }

                var data = new List<Tuple<int, string>>();

                #region Autorizados
                var listaAutorizaciones = (
                    from aut in _context.tblS_CapacitacionSeguridadListaAutorizacion.Where(x => x.estatus && x.division == divisionActual).ToList()
                    join cc in _context.tblS_CapacitacionSeguridadListaAutorizacionCC.ToList() on aut.id equals cc.listaAutorizacionID
                    where filtros.listaCursosID != null ? filtros.listaCursosID.Contains(aut.cursoID) : true
                    select new
                    {
                        id = aut.id,
                        cc = cc.cc,
                        departamento = cc.departamento,
                        empresa = cc.empresa
                    }
                ).ToList();

                if (filtros.listaAreas != null && filtros.listaAreas.Count() > 0)
                {
                    listaAutorizaciones = (
                        from aut in listaAutorizaciones
                        join fil in filtros.listaAreas on
                            new { cc = aut.cc, departamento = aut.departamento, empresa = aut.empresa } equals
                            new { cc = fil.cc, departamento = fil.departamento, empresa = fil.empresa }
                        select new
                        {
                            id = aut.id,
                            cc = aut.cc,
                            departamento = aut.departamento,
                            empresa = aut.empresa
                        }
                    ).ToList();
                }

                var listaAsistentes = _context.tblS_CapacitacionSeguridadListaAutorizacionAsistentes.ToList().Where(x =>
                    x.estatus && listaAutorizaciones.Select(y => y.id).Contains(x.listaAutorizacionID)
                ).ToList();

                data.Add(Tuple.Create(listaAsistentes.Count(), "Autorizados"));
                #endregion

                #region Personal en Puesto
                var empleadosPorCCDepartamento = ObtenerEmpleadosCCPorDepartamento(
                    filtros.listaCCConstruplan,
                    filtros.listaCCArrendadora,
                    filtros.listaAreas != null ? filtros.listaAreas.Select(x => x.departamento.ToString()).ToList() : null
                );
                var puestosAplicables = _context.tblS_CapacitacionSeguridadCursosPuestos.Where(x =>
                    x.estatus && x.division == divisionActual).ToList().Where(x => filtros.listaCursosID != null ? filtros.listaCursosID.Contains(x.curso_id) : true
                ).Select(x => x.puesto_id).ToList();

                var empleadosFiltrados = empleadosPorCCDepartamento.Where(x => puestosAplicables.Contains(x.puestoID)).ToList();

                data.Add(Tuple.Create(empleadosFiltrados.Count(), "Personal en Puesto"));
                #endregion

                var graficaPersonalAutorizado = new
                {
                    labels = data.Select(x => x.Item2).ToList(),
                    datasets = new List<DatasetDTO> { new DatasetDTO {
                        data = data.Select(x => Convert.ToDecimal(x.Item1)).ToList(),
                        backgroundColor = new List<string>{"#FF802C", "#FF802C"}
                    }}
                };

                #region DC3
                var data2 = new List<Tuple<int, string>>();
                var fechaMinima = DateTime.Today.AddHours(23).AddMinutes(59).AddYears(-1); // Fecha un año atrás al día de hoy para las capacitaciones vigentes.
                var listaControlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                    x.activo && x.division == divisionActual && DbFunctions.TruncateTime(x.fechaCapacitacion) >= DbFunctions.TruncateTime(fechaMinima)
                ).ToList().Where(x =>
                    (filtros.listaCursosID != null ? filtros.listaCursosID.Contains(x.cursoID) : true) &&
                    (x.asistentes.Select(y => y.claveEmpleado).Intersect(empleadosFiltrados.Select(z => z.claveEmpleado)).Any())
                ).ToList();

                data2.Add(Tuple.Create(listaControlAsistencia.Where(x => x.asistentes.Select(y => y.calificacion).Average() >= 85).Count(), "CURSOS APROBADOS"));

                var contadorDC3 = 0;
                foreach (var controlAsistencia in listaControlAsistencia)
                {
                    var personalConDC3 = controlAsistencia.asistentes.Where(x => x.rutaDC3 != null).ToList();

                    contadorDC3 += personalConDC3.Count();
                }

                data2.Add(Tuple.Create(contadorDC3, "DC-3 REALIZADAS"));

                var graficaDC3 = new
                {
                    labels = data2.Select(x => x.Item2).ToList(),
                    datasets = new List<DatasetDTO> { new DatasetDTO {
                        data = data2.Select(x => Convert.ToDecimal(x.Item1)).ToList(),
                        backgroundColor = new List<string>{"#FF802C", "#404040"}
                    }}
                };
                #endregion

                #region Lista Empleados
//                var odbc = new OdbcConsultaDTO()
//                {
//                    consulta = @"
//                        SELECT
//                            e.clave_empleado AS claveEmpleado,
//                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
//                            e.puesto,
//                            p.descripcion AS puestoDesc,
//                            e.cc_contable AS cc,
//                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
//                        FROM DBA.sn_empleados AS e
//                            INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                            INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
//                        WHERE e.estatus_empleado ='A'"
//                };

//                List<dynamic> listaEmpleados = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
//                List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                var listaEmpleados = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                });

                listaEmpleados.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"
                }));

                var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @" SELECT
                            e.clave_empleado AS claveEmpleado,
                            (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                            e.puesto,
                            p.descripcion AS puestoDesc,
                            e.cc_contable AS cc,
                            (e.cc_contable + '-' + c.descripcion) AS ccDesc
                        FROM tblRH_EK_Empleados AS e
                            INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                            INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                        WHERE e.estatus_empleado ='A'"

                });

                listaEmpleados.AddRange(listaEmpleadosARR);

                listaEmpleados = listaEmpleados.Where(x =>
                    (filtros.listaCCConstruplan != null ? filtros.listaCCConstruplan.Contains((string)x.cc) : true) &&
                    (filtros.listaCCArrendadora != null ? filtros.listaCCArrendadora.Contains((string)x.cc) : true)
                ).ToList();
                #endregion

                #region Tabla Asistentes
                var tablaListaAsistentes = new List<dynamic>();

                foreach (var asis in listaAsistentes)
                {
                    var empleado = listaEmpleados.FirstOrDefault(x => x.claveEmpleado == asis.claveEmpleado);

                    if (empleado != null)
                    {
                        tablaListaAsistentes.Add(new
                        {
                            claveEmpleado = asis.claveEmpleado,
                            nombreEmpleado = empleado.nombreEmpleado,
                            puestoDesc = empleado.puestoDesc,
                            ccDesc = empleado.ccDesc
                        });
                    }
                }
                #endregion

                resultado.Add("graficaPersonalAutorizado", graficaPersonalAutorizado);
                resultado.Add("graficaDC3", graficaDC3);
                resultado.Add("tablaListaAsistentes", tablaListaAsistentes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "cargarDashboardPersonalAutorizado", e, AccionEnum.CONSULTA, 0, filtros);
                resultado.Clear();
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, "Ocurrió un error al consultar la información para el dashboard.");
            }

            return resultado;
        }

        public Dictionary<string, object> getCorreosListaAutorizacion(int listaAutorizacionID)
        {
            try
            {
                var listaAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                if (listaAutorizacion != null)
                {
                    var listaCorreos = new List<string>();

                    if (listaAutorizacion.jefeDepartamento > 0)
                    {
                        var usuarioJefe = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.jefeDepartamento.ToString());

                        if (usuarioJefe != null)
                        {
                            if (usuarioJefe.correo != null)
                            {
                                listaCorreos.Add(usuarioJefe.correo);
                            }
                        }
                    }

                    if (listaAutorizacion.gerenteProyecto > 0)
                    {
                        var usuarioGerente = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.gerenteProyecto.ToString());

                        if (usuarioGerente != null)
                        {
                            if (usuarioGerente.correo != null)
                            {
                                listaCorreos.Add(usuarioGerente.correo);
                            }
                        }
                    }

                    if (listaAutorizacion.coordinadorCSH > 0)
                    {
                        var usuarioCoordinador = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.coordinadorCSH.ToString());

                        if (usuarioCoordinador != null)
                        {
                            if (usuarioCoordinador.correo != null)
                            {
                                listaCorreos.Add(usuarioCoordinador.correo);
                            }
                        }
                    }

                    if (listaAutorizacion.secretarioCSH > 0)
                    {
                        var usuarioSecretario = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.secretarioCSH.ToString());

                        if (usuarioSecretario != null)
                        {
                            if (usuarioSecretario.correo != null)
                            {
                                listaCorreos.Add(usuarioSecretario.correo);
                            }
                        }
                    }

                    if (listaAutorizacion.seguridad > 0)
                    {
                        var usuarioSeguridad = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.seguridad.ToString());

                        if (usuarioSeguridad != null)
                        {
                            if (usuarioSeguridad.correo != null)
                            {
                                listaCorreos.Add(usuarioSeguridad.correo);
                            }
                        }
                    }

                    var listaInteresados = _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                    foreach (var inte in listaInteresados)
                    {
                        var usuarioInteresado = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == inte.claveEmpleado.ToString());

                        if (usuarioInteresado != null)
                        {
                            if (usuarioInteresado.correo != null)
                            {
                                listaCorreos.Add(usuarioInteresado.correo);
                            }
                        }
                    }

                    resultado.Add("correos", string.Join("; ", listaCorreos.Distinct()));
                }
                else
                {
                    throw new Exception("No se encuentra la información de la lista de autorización.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getCorreosListaAutorizacion", e, AccionEnum.CONSULTA, 0, listaAutorizacionID);
            }

            return resultado;
        }

        public bool enviarCorreoListaAutorizacion(int listaAutorizacionID, List<Byte[]> archivoListaAutorizacion, List<string> listaCorreos)
        {
            bool correoEnviado = false;

            try
            {
                var listaAutorizacion = _context.tblS_CapacitacionSeguridadListaAutorizacion.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                if (listaAutorizacion != null)
                {
                    //#region Correos
                    //var listaCorreos = new List<string>();

                    //if (listaAutorizacion.jefeDepartamento > 0)
                    //{
                    //    var usuarioJefe = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.jefeDepartamento.ToString());

                    //    if (usuarioJefe != null)
                    //    {
                    //        if (usuarioJefe.correo != null)
                    //        {
                    //            listaCorreos.Add(usuarioJefe.correo);
                    //        }
                    //    }
                    //}

                    //if (listaAutorizacion.gerenteProyecto > 0)
                    //{
                    //    var usuarioGerente = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.gerenteProyecto.ToString());

                    //    if (usuarioGerente != null)
                    //    {
                    //        if (usuarioGerente.correo != null)
                    //        {
                    //            listaCorreos.Add(usuarioGerente.correo);
                    //        }
                    //    }
                    //}

                    //if (listaAutorizacion.coordinadorCSH > 0)
                    //{
                    //    var usuarioCoordinador = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.coordinadorCSH.ToString());

                    //    if (usuarioCoordinador != null)
                    //    {
                    //        if (usuarioCoordinador.correo != null)
                    //        {
                    //            listaCorreos.Add(usuarioCoordinador.correo);
                    //        }
                    //    }
                    //}

                    //if (listaAutorizacion.secretarioCSH > 0)
                    //{
                    //    var usuarioSecretario = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == listaAutorizacion.secretarioCSH.ToString());

                    //    if (usuarioSecretario != null)
                    //    {
                    //        if (usuarioSecretario.correo != null)
                    //        {
                    //            listaCorreos.Add(usuarioSecretario.correo);
                    //        }
                    //    }
                    //}

                    //var listaInteresados = _context.tblS_CapacitacionSeguridadListaAutorizacionInteresados.Where(x => x.estatus && x.listaAutorizacionID == listaAutorizacion.id).ToList();

                    //foreach (var inte in listaInteresados)
                    //{
                    //    var usuarioInteresado = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.cveEmpleado == inte.claveEmpleado.ToString());

                    //    if (usuarioInteresado != null)
                    //    {
                    //        if (usuarioInteresado.correo != null)
                    //        {
                    //            listaCorreos.Add(usuarioInteresado.correo);
                    //        }
                    //    }
                    //}
                    //#endregion

                    var cursoSIGOPLAN = _context.tblS_CapacitacionSeguridadCursos.FirstOrDefault(x => x.isActivo && x.division == divisionActual && x.id == listaAutorizacion.cursoID);

                    if (cursoSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra la información del curso.");
                    }

#if DEBUG
                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif

                    var asunto = string.Format(@"Lista de Autorización - {0} - {1}", listaAutorizacion.claveLista, cursoSIGOPLAN.claveCurso);
                    var mensaje = string.Format(
                        @"Se ha creado una nueva lista de autorización para el curso {0}.<br/>Fecha y hora: {1}", cursoSIGOPLAN.claveCurso, DateTime.Now
                    );

                    var archivosAdjuntos = new List<adjuntoCorreoDTO> { new adjuntoCorreoDTO
                        {
                            archivo = archivoListaAutorizacion[0],
                            extArchivo = ".pdf",
                            nombreArchivo = "Lista de Autorización"
                        }
                    };

                    correoEnviado = GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos, archivosAdjuntos);
                }
                else
                {
                    throw new Exception("No se encuentra la información de la lista de autorización.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "CapacitacionSeguridadController", "enviarCorreoListaAutorizacion", e, AccionEnum.CORREO, 0, listaAutorizacionID);
            }

            return correoEnviado;
        }
        #endregion

        #region Detección de Necesidades
        #region Ciclos de Trabajo
        public List<ComboDTO> getCiclosTrabajoCombo()
        {
            try
            {
                var listaCiclosTrabajo = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.estatus && x.division == divisionActual).ToList().Select(x => new ComboDTO
                {
                    Value = x.id.ToString(),
                    Text = x.titulo
                }).OrderBy(x => x.Text).ToList();

                return listaCiclosTrabajo;
            }
            catch (Exception)
            {
                return new List<ComboDTO>();
            }
        }

        public Dictionary<string, object> guardarNuevoCiclo(tblS_CapacitacionSeguridadDNCicloTrabajo ciclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> listaAreas, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> listaCriterios)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ciclo.fechaCiclo = DateTime.Now;
                    ciclo.fechaCreacion = DateTime.Now;
                    ciclo.division = divisionActual;

                    _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Add(ciclo);
                    _context.SaveChanges();

                    foreach (var area in listaAreas)
                    {
                        area.cicloTrabajoID = ciclo.id;

                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoAreas.Add(area);
                        _context.SaveChanges();
                    }

                    foreach (var criterio in listaCriterios)
                    {
                        criterio.cicloTrabajoID = ciclo.id;

                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Add(criterio);
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarNuevoCiclo", e, AccionEnum.AGREGAR, 0, new { ciclo = ciclo, listaAreas = listaAreas, listaCriterios = listaCriterios });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getCicloByID(int cicloID)
        {
            try
            {
                var cicloSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == cicloID);

                if (cicloSIGOPLAN != null)
                {
                    var listaCriterios = _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x =>
                        x.estatus && x.cicloTrabajoID == cicloSIGOPLAN.id).ToList().Select(x => new CicloTrabajoCriterioDTO
                    {
                        id = x.id,
                        descripcion = x.descripcion,
                        ponderacion = (PonderacionCriterioEnum)x.ponderacion,
                        ponderacionDesc = ((PonderacionCriterioEnum)x.ponderacion).GetDescription(),
                        cicloTrabajoID = x.cicloTrabajoID
                    }).ToList();

                    var ciclo = new CicloTrabajoDTO
                    {
                        id = cicloSIGOPLAN.id,
                        titulo = cicloSIGOPLAN.titulo,
                        descripcion = cicloSIGOPLAN.descripcion,
                        tipoCiclo = (TipoCicloEnum)cicloSIGOPLAN.tipoCiclo,
                        tipoCicloDesc = ((TipoCicloEnum)cicloSIGOPLAN.tipoCiclo).GetDescription(),
                        listaCriterios = listaCriterios
                    };

                    resultado.Add("datos", ciclo);
                }
                else
                {
                    throw new Exception("No se encuentra la información del ciclo.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getCicloByID", e, AccionEnum.CONSULTA, 0, cicloID);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarRegistroCiclo(tblS_CapacitacionSeguridadDNCicloTrabajoRegistro registroCiclo, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones> listaRevisiones, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> listaPropuestas, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas> listaAreas)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (registroCiclo.fecha == null)
                    {
                        throw new Exception("Debe capturar una fecha válida.");
                    }

                    registroCiclo.accionRequerida = registroCiclo.accionRequerida ?? "";
                    //registroCiclo.fecha = DateTime.Now;
                    registroCiclo.comentariosEvaluador = "";
                    registroCiclo.observacionesRevisor = registroCiclo.observacionesRevisor ?? "";
                    registroCiclo.accionesTomadas = registroCiclo.accionesTomadas ?? "";
                    registroCiclo.observacionesLider = registroCiclo.observacionesLider ?? "";
                    registroCiclo.division = divisionActual;
                    registroCiclo.estatus = true;

                    _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Add(registroCiclo);
                    _context.SaveChanges();

                    foreach (var revision in listaRevisiones)
                    {
                        revision.cicloRegistroID = registroCiclo.id;
                        revision.estatus = true;

                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones.Add(revision);
                        _context.SaveChanges();
                    }

                    if (listaPropuestas != null)
                    {
                        foreach (var propuesta in listaPropuestas)
                        {
                            propuesta.cicloRegistroID = registroCiclo.id;
                            propuesta.estatus = true;

                            _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.Add(propuesta);
                            _context.SaveChanges();
                        }
                    }

                    foreach (var area in listaAreas)
                    {
                        area.cicloRegistroID = registroCiclo.id;
                        area.estatus = true;

                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas.Add(area);
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarRegistroCiclo", e, AccionEnum.AGREGAR, 0, new { registroCiclo = registroCiclo, listaRevisiones = listaRevisiones, listaPropuestas = listaPropuestas });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getRegistrosCiclos(FiltrosRegistrosCiclo filtros)
        {
            try
            {
                var listaCiclosRegistro = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    (filtros.listaCC != null ? filtros.listaCC.Contains(x.cc) : true) &&
                    (filtros.listaCiclos != null ? filtros.listaCiclos.Contains(x.cicloID) : true) &&
                    (x.fecha != null ? ((DateTime)x.fecha).Date >= filtros.fechaInicio.Date && ((DateTime)x.fecha).Date <= filtros.fechaFin.Date : false)
                ).ToList();
                var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas.Where(x => x.estatus).ToList();
                var listaFiltrada = new List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro>();

                foreach (var cicloRegistro in listaCiclosRegistro)
                {
                    if (filtros.listaAreas != null)
                    {
                        var listaAreasPorCiclo = listaAreasSIGOPLAN.Where(x => x.cicloRegistroID == cicloRegistro.id).ToList();

                        if (filtros.listaAreas.Any(x => listaAreasPorCiclo.Any(y => y.area == x.departamento && y.cc == x.cc && y.empresa == x.empresa)))
                        {
                            listaFiltrada.Add(cicloRegistro);
                        }

                        //foreach (var area in filtros.listaAreas)
                        //{
                        //    if (cicloRegistro.cc == area.cc && cicloRegistro.area == area.departamento && cicloRegistro.empresa == area.empresa)
                        //    {
                        //        listaFiltrada.Add(cicloRegistro);
                        //        break;
                        //    }
                        //}
                    }
                    else
                    {
                        listaFiltrada.Add(cicloRegistro);
                    }
                }

                #region Departamentos
                var departamentosAmbasEmpresas = new List<ComboDTO>();

                //var odbc = new OdbcConsultaDTO();
                //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});
                //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});

                var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });
                var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });

                if (departamentosConstruplan.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "CONSTRUPLAN" }).ToList());
                }

                if (departamentosArrendadora.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "ARRENDADORA" }).ToList());
                }
                #endregion

                var listaCiclos = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.estatus && x.division == divisionActual).ToList();

                var datos = listaFiltrada.Select(x => new
                {
                    id = x.id,
                    cicloID = x.cicloID,
                    cicloDesc = listaCiclos.Where(y => y.id == x.cicloID).Select(z => z.titulo).FirstOrDefault(),
                    //area = x.area,
                    //areaDesc = departamentosAmbasEmpresas.Where(y => y.Value == x.area.ToString()).Select(z => z.Text + " (" + x.cc + ")").FirstOrDefault(),
                    areaDesc = string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                         listaAreasSIGOPLAN.Where(q => q.cicloRegistroID == x.id).Select(w => w.area.ToString()).Contains(y.Value)
                    ).Select(z => z.Text + " (" + x.cc + ")")),
                    fecha = x.fecha,
                    fechaString = ((DateTime)x.fecha).ToShortDateString()
                });

                resultado.Add("datos", datos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getRegistrosCiclos", e, AccionEnum.CONSULTA, 0, filtros);
            }

            return resultado;
        }

        public Dictionary<string, object> getListaSeguimiento(List<string> listaCC, TipoSeguimientoEnum tipoSeguimiento, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var listaUsuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                #region Empleados
                //var listaEmpleadosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa FROM sn_empleados"
                //});
                //var listaEmpleadosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 2 AS empresa FROM sn_empleados"
                //});

                var listaEmpleadosConstruplan = _context.Select<dynamic>(new DapperDTO 
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa 
                                    FROM tblRH_EK_Empleados",
                });

                listaEmpleadosConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa 
                                    FROM tblRH_EK_Empleados",
                }));

                var listaEmpleadosArrendadora = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 2 AS empresa 
                                    FROM tblRH_EK_Empleados",
                });

                var listaEmpleados = new List<dynamic>();

                listaEmpleados.AddRange(listaEmpleadosConstruplan);
                listaEmpleados.AddRange(listaEmpleadosArrendadora);
                #endregion

                if (tipoSeguimiento == TipoSeguimientoEnum.acciones)
                {
                    var listaSeguimiento = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                        x.metodo != (int)MetodoAccionRequeridaEnum.noEspecificado &&
                        (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                        (x.fecha != null ? ((DateTime)x.fecha).Date >= fechaInicio.Date && ((DateTime)x.fecha).Date <= fechaFin.Date : false)
                    ).Select(x => new
                    {
                        id = x.id,
                        accion = x.accionRequerida,
                        metodo = (int)x.metodo,
                        metodoDesc = ((MetodoEnum)x.metodo).GetDescription(),
                        rutaEvidencia = x.rutaEvidencia,
                        evaluador = x.evaluador,
                        evaluadorDesc = listaUsuariosSIGOPLAN.Where(y => y.id == x.evaluador).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                        aprobo = x.aprobo,
                        comentariosEvaluador = x.comentariosEvaluador,
                        colaborador = x.colaborador,
                        colaboradorDesc = listaEmpleados.Where(y => (int)y.clave_empleado == x.colaborador).Select(z => (string)z.nombre).FirstOrDefault(),
                    }).ToList();

                    var totalAcciones = listaSeguimiento.Count();
                    var solventadas = listaSeguimiento.Where(x => x.aprobo).ToList().Count();
                    var proceso = listaSeguimiento.Where(x => !x.aprobo).ToList().Count();
                    var porcentaje = (solventadas * 100) / (totalAcciones > 0 ? totalAcciones : 1);

                    resultado.Add("datos", new
                    {
                        listaSeguimiento = listaSeguimiento,
                        totalAcciones = totalAcciones,
                        solventadas = solventadas,
                        proceso = proceso,
                        porcentaje = porcentaje
                    });
                }
                else if (tipoSeguimiento == TipoSeguimientoEnum.propuestas)
                {
                    var listaRegistrosCiclo = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                        (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                        (x.fecha != null ? ((DateTime)x.fecha).Date >= fechaInicio.Date && ((DateTime)x.fecha).Date <= fechaFin.Date : false)
                    ).ToList();
                    var listaSeguimiento = new List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas>();

                    foreach (var reg in listaRegistrosCiclo)
                    {
                        var listaPropuestas = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.Where(x => x.estatus && x.cicloRegistroID == reg.id).ToList();

                        if (listaPropuestas.Count() > 0)
                        {
                            listaSeguimiento.AddRange(listaPropuestas);
                        }
                    }

                    var totalPropuestas = listaSeguimiento.Count();
                    var solventadas = listaSeguimiento.Where(x => x.solventada).ToList().Count();
                    var proceso = listaSeguimiento.Where(x => !x.solventada).ToList().Count();
                    var porcentaje = (solventadas * 100) / (totalPropuestas > 0 ? totalPropuestas : 1);

                    resultado.Add("datos", new
                    {
                        listaSeguimiento = listaSeguimiento,
                        totalPropuestas = totalPropuestas,
                        solventadas = solventadas,
                        proceso = proceso,
                        porcentaje = porcentaje
                    });
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getListaSeguimiento", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, tipoSeguimiento = (int)tipoSeguimiento, fechaInicio = fechaInicio, fechaFin = fechaFin });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarSeguimientoAcciones(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro> capturaEvaluaciones)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Guardar Evidencias
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var cap in capturaEvidencias)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidenciaDeteccionNecesidades, evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaDeteccionNecesidadesAcciones, nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(evidencias[index], rutaArchivoEvidencia));

                        var registroCicloSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == cap.id);

                        if (registroCicloSIGOPLAN != null)
                        {
                            registroCicloSIGOPLAN.rutaEvidencia = rutaArchivoEvidencia;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del registro del ciclo.");
                        }

                        index++;
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
                    #endregion

                    #region Guardar Evaluaciones
                    foreach (var eva in capturaEvaluaciones)
                    {
                        var registroCicloSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == eva.id);

                        if (registroCicloSIGOPLAN != null)
                        {
                            registroCicloSIGOPLAN.evaluador = vSesiones.sesionUsuarioDTO.id;
                            registroCicloSIGOPLAN.aprobo = eva.aprobo;
                            registroCicloSIGOPLAN.comentariosEvaluador = eva.comentariosEvaluador;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del registro del ciclo.");
                        }
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
                    LogError(0, 0, NombreControlador, "guardarSeguimientoAcciones", e, AccionEnum.AGREGAR, 0, new { capturaEvidencias = capturaEvidencias, capturaEvaluaciones = capturaEvaluaciones });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarSeguimientoPropuestas(List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvidencias, List<HttpPostedFileBase> evidencias, List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas> capturaEvaluaciones)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Guardar Evidencias
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var cap in capturaEvidencias)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseEvidenciaDeteccionNecesidades, evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaDeteccionNecesidadesPropuestas, nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(evidencias[index], rutaArchivoEvidencia));

                        var registroPropuestaSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.FirstOrDefault(x => x.estatus && x.id == cap.id);

                        if (registroPropuestaSIGOPLAN != null)
                        {
                            registroPropuestaSIGOPLAN.rutaEvidencia = rutaArchivoEvidencia;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del registro de la propuesta.");
                        }

                        index++;
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
                    #endregion

                    #region Guardar Evaluaciones
                    foreach (var eva in capturaEvaluaciones)
                    {
                        var registroPropuestaSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.FirstOrDefault(x => x.estatus && x.id == eva.id);

                        if (registroPropuestaSIGOPLAN != null)
                        {
                            registroPropuestaSIGOPLAN.evaluador = vSesiones.sesionUsuarioDTO.id;
                            registroPropuestaSIGOPLAN.solventada = eva.solventada;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del registro del ciclo.");
                        }
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
                    LogError(0, 0, NombreControlador, "guardarSeguimientoPropuestas", e, AccionEnum.AGREGAR, 0, new { capturaEvidencias = capturaEvidencias, capturaEvaluaciones = capturaEvaluaciones });
                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoAcciones(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.FirstOrDefault(x => x.id == id);

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

        public Tuple<Stream, string> descargarArchivoEvidenciaAccion(int id)
        {
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.FirstOrDefault(x => x.id == id);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                string name = Path.GetFileName(captura.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<string, object> cargarDatosArchivoEvidenciaSeguimientoPropuestas(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.FirstOrDefault(x => x.id == id);

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

        public Tuple<Stream, string> descargarArchivoEvidenciaPropuesta(int id)
        {
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.FirstOrDefault(x => x.id == id);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaEvidencia);
                string name = Path.GetFileName(captura.rutaEvidencia);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<Tuple<string, int>> months = new List<Tuple<string, int>> {
            new Tuple<string, int>("Enero", 1),
            new Tuple<string, int>("Febrero", 2),
            new Tuple<string, int>("Marzo", 3),
            new Tuple<string, int>("Abril", 4),
            new Tuple<string, int>("Mayo", 5),
            new Tuple<string, int>("Junio", 6),
            new Tuple<string, int>("Julio", 7),
            new Tuple<string, int>("Agosto", 8),
            new Tuple<string, int>("Septiembre", 9),
            new Tuple<string, int>("Octubre", 10),
            new Tuple<string, int>("Noviembre", 11),
            new Tuple<string, int>("Diciembre", 12)
        };

        public Dictionary<string, object> cargarDashboardCiclos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                #region Filtros
                var listaRegistrosCiclo = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    (x.fecha != null ? (((DateTime)x.fecha).Date >= fechaInicio.Date && ((DateTime)x.fecha).Date <= fechaFin.Date) : false)
                ).OrderBy(x => x.id).ToList();

                if (listaAreas != null && listaAreas.Count() > 0)
                {
                    var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas.Where(x => x.estatus).ToList();
                    var listaFiltrada = new List<tblS_CapacitacionSeguridadDNCicloTrabajoRegistro>();

                    foreach (var cicloRegistro in listaRegistrosCiclo)
                    {
                        var listaAreasPorCiclo = listaAreasSIGOPLAN.Where(x => x.cicloRegistroID == cicloRegistro.id).ToList();

                        if (listaAreas.Any(x => listaAreasPorCiclo.Any(y => y.area == x.area && y.cc == x.cc && y.empresa == x.empresa)))
                        {
                            listaFiltrada.Add(cicloRegistro);
                        }
                    }

                    listaRegistrosCiclo = listaFiltrada;

                    //listaRegistrosCiclo = (
                    //    from reg in listaRegistrosCiclo
                    //    join area in listaAreas on new { empresa = reg.empresa, cc = reg.cc, area = reg.area } equals new { empresa = area.empresa, cc = area.cc, area = area.area }
                    //    select reg
                    //).ToList();
                }
                #endregion

                var listaRevisiones = (
                    from reg in listaRegistrosCiclo
                    join cic in _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.estatus && x.division == divisionActual).ToList() on reg.cicloID equals cic.id
                    join rev in _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones.Where(x => x.estatus).ToList() on reg.id equals rev.cicloRegistroID
                    join crit in _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x => x.estatus).ToList() on rev.criterioID equals crit.id
                    select new
                    {
                        tituloCiclo = cic.titulo,
                        registroCicloID = reg.id,
                        cc = reg.cc,
                        fecha = reg.fecha,
                        //area = reg.area,
                        empresa = reg.empresa,
                        cicloID = reg.cicloID,
                        registroCicloAcredito = reg.acredito,
                        retroalimentacion = reg.retroalimentacion,
                        colaborador = reg.colaborador,
                        revisionID = rev.id,
                        criterioID = rev.criterioID,
                        criterioDesc = crit.descripcion,
                        revisionAcredito = rev.acredito,
                        ponderacion = crit.ponderacion
                    }
                ).ToList();
                //var listaRevisionesCumplidas = listaRevisiones.Where(x => x.revisionAcredito).ToList();

                #region Empleados
                //var listaEmpleadosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                //});
                //var listaEmpleadosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 2 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                //});

                var listaEmpleadosConstruplan = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa 
                                    FROM tblRH_EK_Empleados",
                });

                listaEmpleadosConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 1 AS empresa 
                                    FROM tblRH_EK_Empleados",
                }));

                var listaEmpleadosArrendadora = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, (nombre + ' ' + ape_paterno + ' ' + ape_materno) AS nombre, clave_depto, cc_contable, 2 AS empresa 
                                    FROM tblRH_EK_Empleados",
                });

                var listaEmpleados = new List<dynamic>();

                listaEmpleados.AddRange(listaEmpleadosConstruplan);
                listaEmpleados.AddRange(listaEmpleadosArrendadora);
                #endregion

                #region Gráfica Indicadores y Tabla de Inspecciones Cumplidas
                List<InspeccionesCumplidasDTO> listaInspeccionesCumplidas = new List<InspeccionesCumplidasDTO>();
                var graficaIndicadores = new GraficaDTO();

                foreach (var mes in months)
                {
                    var fechaInicioMes = new DateTime(DateTime.Now.Year, mes.Item2, 1);
                    var fechaFinMes = new DateTime(DateTime.Now.Year, mes.Item2, DateTime.DaysInMonth(DateTime.Now.Year, mes.Item2));
                    var revisionesPorMes = listaRevisiones.Where(x =>
                        (x.fecha != null ? ((DateTime)x.fecha).Date >= fechaInicioMes.Date && ((DateTime)x.fecha).Date <= fechaFinMes.Date : false)
                    ).ToList();
                    var revisionesCumplidas = revisionesPorMes.Where(x => x.revisionAcredito).ToList();
                    var revisionesNOCumplidas = revisionesPorMes.Where(x => !x.revisionAcredito).ToList();

                    graficaIndicadores.categorias.Add(mes.Item1);
                    graficaIndicadores.serie1Descripcion = "Puntos revisión cumplidos";
                    graficaIndicadores.serie1.Add(revisionesCumplidas.Count());
                    graficaIndicadores.serie2Descripcion = "Puntos revisión NO cumplidos";
                    graficaIndicadores.serie2.Add(revisionesNOCumplidas.Count());

                    var totalCiclos = revisionesPorMes.GroupBy(x => x.registroCicloID).Count(); //var totalCiclos = revisionesCumplidas.Count() + revisionesNOCumplidas.Count();
                    var totalRevisiones = revisionesCumplidas.Count() + revisionesNOCumplidas.Count();
                    var criticos =
                        (revisionesCumplidas.Where(x => x.ponderacion == (int)PonderacionCriterioEnum.critico && x.revisionAcredito).ToList().Count() * 100) / (totalRevisiones > 0 ? totalRevisiones : 1);
                    var medios =
                        (revisionesCumplidas.Where(x => x.ponderacion == (int)PonderacionCriterioEnum.medio && x.revisionAcredito).ToList().Count() * 100) / (totalRevisiones > 0 ? totalRevisiones : 1);
                    var bajos =
                        (revisionesCumplidas.Where(x => x.ponderacion == (int)PonderacionCriterioEnum.bajo && x.revisionAcredito).ToList().Count() * 100) / (totalRevisiones > 0 ? totalRevisiones : 1);

                    listaInspeccionesCumplidas.Add(new InspeccionesCumplidasDTO
                    {
                        totalCiclos = totalCiclos,
                        mes = mes.Item2,
                        mesDesc = mes.Item1,
                        criticos = criticos,
                        criticosDesc = criticos.ToString("0.##") + "%",
                        medios = medios,
                        mediosDesc = medios.ToString("0.##") + "%",
                        bajos = bajos,
                        bajosDesc = bajos.ToString("0.##") + "%"
                    });
                }
                #endregion

                #region Tabla Inspecciones a Realizar Nuevamente
                List<InspeccionesRealizarNuevamenteDTO> listaInspeccionesRealizarNuevamente = new List<InspeccionesRealizarNuevamenteDTO>();

                var listaRevisionesNoCumplidas = listaRevisiones.Where(x => !x.revisionAcredito).ToList();
                var listaRevisionesUnicas = listaRevisionesNoCumplidas.GroupBy(x => new { x.revisionID, x.criterioDesc }).ToList();

                foreach (var rev in listaRevisionesUnicas)
                {
                    var listaMeses = new List<string> { "", "", "", "", "", "", "", "", "", "", "", "" };

                    foreach (var mes in months)
                    {
                        var listaRevisionesNoCumplidasPorMes = listaRevisionesNoCumplidas.Where(x =>
                            x.revisionID == rev.Key.revisionID && (x.fecha != null ? ((DateTime)x.fecha).Month == mes.Item2 : false)
                        ).ToList();

                        if (listaRevisionesNoCumplidasPorMes.Count() > 0)
                        {
                            listaMeses[mes.Item2 - 1] = string.Join(", ", listaRevisionesNoCumplidasPorMes.Select(x => x.tituloCiclo).Distinct().ToList());
                        }
                    }

                    listaInspeccionesRealizarNuevamente.Add(new InspeccionesRealizarNuevamenteDTO
                    {
                        inspeccion = rev.Key.criterioDesc,
                        enero = listaMeses[0],
                        febrero = listaMeses[1],
                        marzo = listaMeses[2],
                        abril = listaMeses[3],
                        mayo = listaMeses[4],
                        junio = listaMeses[5],
                        julio = listaMeses[6],
                        agosto = listaMeses[7],
                        septiembre = listaMeses[8],
                        octubre = listaMeses[9],
                        noviembre = listaMeses[10],
                        diciembre = listaMeses[11]
                    });
                }
                #endregion

                #region Tabla de Detalles Ciclos
                var listaCiclos = listaRevisiones.GroupBy(x => new { x.registroCicloID, x.registroCicloAcredito, x.retroalimentacion }).ToList();
                var listaDetallesCiclos = new List<dynamic> { new
                    {
                        ciclosAcreditados = listaCiclos.Where(x => x.Key.registroCicloAcredito).ToList().Count(),
                        ciclosNoAcreditados = listaCiclos.Where(x => !x.Key.registroCicloAcredito).ToList().Count(),
                        retroalimentaciones = listaCiclos.Where(x => x.Key.retroalimentacion).ToList().Count()
                    }
                };
                #endregion

                #region Gráfica Detalles
                var graficaDetalles = new GraficaDTO();

                foreach (var mes in months)
                {
                    var listaRevisionesPorMes = listaRevisiones.Where(x => (x.fecha != null ? ((DateTime)x.fecha).Month == mes.Item2 : false)).ToList();
                    var ciclosRevisionesPorMes = listaRevisionesPorMes.GroupBy(x => x.registroCicloID).ToList();
                    var ciclosRevisionesPorMes_id = ciclosRevisionesPorMes.Select(x => x.Key).ToList();
                    var ciclosSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x => ciclosRevisionesPorMes_id.Contains(x.id)).ToList();

                    graficaDetalles.categorias.Add(mes.Item1);
                    graficaDetalles.serie1Descripcion = "Ciclos Acreditados";
                    graficaDetalles.serie1.Add(ciclosSIGOPLAN.Where(x => x.acredito).ToList().Count());
                    graficaDetalles.serie2Descripcion = "Ciclos NO Acreditados";
                    graficaDetalles.serie2.Add(ciclosSIGOPLAN.Where(x => !x.acredito).ToList().Count());
                }
                #endregion

                var ciclos = listaRevisiones.GroupBy(x => new
                {
                    x.cicloID,
                    x.tituloCiclo,
                    x.colaborador
                }).Select(x => new { Key = x.Key, grp = x }).ToList();

                #region Tabla Detalles Anual
                var listaDetallesAnual = new List<DetalleAnualDTO>();
                var listaCatalogoCiclos = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.estatus && x.division == divisionActual).ToList();

                foreach (var ciclo in listaCatalogoCiclos)
                {
                    listaDetallesAnual.Add(new DetalleAnualDTO
                    {
                        cicloDesc = ciclo.titulo,
                        enero = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 1 : false)).ToList().Count(),
                        febrero = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 2 : false)).ToList().Count(),
                        marzo = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 3 : false)).ToList().Count(),
                        abril = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 4 : false)).ToList().Count(),
                        mayo = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 5 : false)).ToList().Count(),
                        junio = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 6 : false)).ToList().Count(),
                        julio = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 7 : false)).ToList().Count(),
                        agosto = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 8 : false)).ToList().Count(),
                        septiembre = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 9 : false)).ToList().Count(),
                        octubre = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 10 : false)).ToList().Count(),
                        noviembre = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 11 : false)).ToList().Count(),
                        diciembre = listaRegistrosCiclo.Where(x => x.cicloID == ciclo.id && (x.fecha != null ? ((DateTime)x.fecha).Month == 12 : false)).ToList().Count(),
                    });
                }

                listaDetallesAnual.Add(new DetalleAnualDTO
                {
                    cicloDesc = "Total Ciclos",
                    enero = listaDetallesAnual.Select(x => x.enero).Sum(),
                    febrero = listaDetallesAnual.Select(x => x.febrero).Sum(),
                    marzo = listaDetallesAnual.Select(x => x.marzo).Sum(),
                    abril = listaDetallesAnual.Select(x => x.abril).Sum(),
                    mayo = listaDetallesAnual.Select(x => x.mayo).Sum(),
                    junio = listaDetallesAnual.Select(x => x.junio).Sum(),
                    julio = listaDetallesAnual.Select(x => x.julio).Sum(),
                    agosto = listaDetallesAnual.Select(x => x.agosto).Sum(),
                    septiembre = listaDetallesAnual.Select(x => x.septiembre).Sum(),
                    octubre = listaDetallesAnual.Select(x => x.octubre).Sum(),
                    noviembre = listaDetallesAnual.Select(x => x.noviembre).Sum(),
                    diciembre = listaDetallesAnual.Select(x => x.diciembre).Sum(),
                });
                #endregion

                #region Tabla Colaboradores
                var ciclosPorColaborador = listaRegistrosCiclo.GroupBy(x => new
                {
                    x.colaborador
                }).Select(x => new { Key = x.Key, grp = x }).ToList();
                var listaDetallesColaboradores = new List<dynamic>();

                foreach (var ciclo in ciclosPorColaborador)
                {
                    var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == ciclo.Key.colaborador);
                    var colaboradorDesc = listaEmpleados.Where(x => (int)x.clave_empleado == ciclo.Key.colaborador).Select(x => (string)x.nombre).FirstOrDefault();

                    listaDetallesColaboradores.Add(new
                    {
                        colaborador = colaboradorDesc,
                        ciclos = ciclo.grp.Count()
                    });
                }
                #endregion

                #region Tabla Promedios
                var listaCiclos_id = listaCiclos.Select(x => x.Key.registroCicloID).ToList();
                var listaCiclosSIGOPLAN = (
                    from reg in _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro
                    join cic in _context.tblS_CapacitacionSeguridadDNCicloTrabajo on reg.cicloID equals cic.id
                    where listaCiclos_id.Contains(reg.id)
                    select new
                    {
                        cicloTrabajoRegistroID = reg.id,
                        cicloID = cic.id,
                        tipoCiclo = cic.tipoCiclo,
                        calificacion = reg.calificacion
                    }
                ).ToList();
                var ciclosPeriodicos = listaCiclosSIGOPLAN.Where(x => x.tipoCiclo == (int)TipoCicloEnum.periodico).ToList();
                var promedioCiclosPeriodicos = Math.Truncate(100 * (ciclosPeriodicos.Sum(x => x.calificacion) / (ciclosPeriodicos.Count() > 0 ? ciclosPeriodicos.Count() : 1))) / 100;
                var ciclosLiberacion = listaCiclosSIGOPLAN.Where(x => x.tipoCiclo == (int)TipoCicloEnum.liberacion).ToList();
                var promedioCiclosLiberacion = Math.Truncate(100 * (ciclosLiberacion.Sum(x => x.calificacion) / (ciclosLiberacion.Count() > 0 ? ciclosLiberacion.Count() : 1))) / 100;
                var promedioGeneral = Math.Truncate(100 * (listaCiclosSIGOPLAN.Sum(x => x.calificacion) / (listaCiclosSIGOPLAN.Count() > 0 ? listaCiclosSIGOPLAN.Count() : 1))) / 100;
                #endregion

                resultado.Add("graficaIndicadores", graficaIndicadores);
                resultado.Add("ciclosRealizados", listaRegistrosCiclo.Count());
                resultado.Add("listaInspeccionesCumplidas", listaInspeccionesCumplidas);
                resultado.Add("listaInspeccionesRealizarNuevamente", listaInspeccionesRealizarNuevamente);
                resultado.Add("listaDetallesCiclos", listaDetallesCiclos);
                resultado.Add("listaDetallesAnual", listaDetallesAnual);
                resultado.Add("graficaDetalles", graficaDetalles);
                resultado.Add("listaDetallesColaboradores", listaDetallesColaboradores);
                resultado.Add("promedioCiclosPeriodicos", promedioCiclosPeriodicos);
                resultado.Add("promedioCiclosLiberacion", promedioCiclosLiberacion);
                resultado.Add("promedioGeneral", promedioGeneral);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDashboardCiclos", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, listaAreas = listaAreas, fechaInicio = fechaInicio, fechaFin = fechaFin });
            }

            return resultado;
        }

        public Dictionary<string, object> getRegistroCicloTrabajoByID(int id)
        {
            try
            {
                var registroCicloTrabajoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == id);

                if (registroCicloTrabajoSIGOPLAN != null)
                {
                    var listaCriterios = _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x => x.estatus && x.cicloTrabajoID == registroCicloTrabajoSIGOPLAN.cicloID).ToList();
                    var listaRevisiones = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroRevisiones.Where(x => x.estatus && x.cicloRegistroID == registroCicloTrabajoSIGOPLAN.id).ToList().Select(x => new
                    {
                        id = x.id,
                        criterioID = x.criterioID,
                        acredito = x.acredito,
                        descripcion = listaCriterios.Where(y => y.id == x.criterioID).Select(z => z.descripcion).FirstOrDefault(),
                        ponderacion = listaCriterios.Where(y => y.id == x.criterioID).Select(z => z.ponderacion).FirstOrDefault(),
                        ponderacionDesc = listaCriterios.Where(y => y.id == x.criterioID).Select(z => ((PonderacionCriterioEnum)z.ponderacion).GetDescription()).FirstOrDefault()
                    }).ToList();
                    var listaPropuestas = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroPropuestas.Where(x => x.estatus && x.cicloRegistroID == registroCicloTrabajoSIGOPLAN.id).Select(x => new
                    {
                        id = x.id,
                        descripcion = x.propuesta,
                        evaluador = x.evaluador,
                        solventada = x.solventada
                    }).ToList();
                    var listaAreas = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistroAreas.Where(x => x.estatus && x.cicloRegistroID == registroCicloTrabajoSIGOPLAN.id).ToList();

                    #region Empleados Enkontrol
                    var odbc = new OdbcConsultaDTO()
                    {
                        consulta = @"
                            SELECT 
                                e.clave_empleado AS claveEmpleado, 
                                (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado 
                            FROM DBA.sn_empleados AS e 
                            WHERE e.estatus_empleado ='A'"
                    };

                    //List<dynamic> listaEmpleadosEnkontrol = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, odbc);
                    //List<dynamic> listaEmpleadosARR = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, odbc);

                    var listaEmpleadosEnkontrol = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado 
                                    FROM tblRH_EK_Empleados AS e 
                                    WHERE e.estatus_empleado ='A'",
                    });

                    listaEmpleadosEnkontrol.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado 
                                    FROM tblRH_EK_Empleados AS e 
                                    WHERE e.estatus_empleado ='A'",
                    }));

                    var listaEmpleadosARR = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado 
                                    FROM tblRH_EK_Empleados AS e 
                                    WHERE e.estatus_empleado ='A'",
                    });
                    listaEmpleadosEnkontrol.AddRange(listaEmpleadosARR);
                    #endregion

                    var registroCicloTrabajo = new
                    {
                        id = registroCicloTrabajoSIGOPLAN.id,
                        cc = registroCicloTrabajoSIGOPLAN.cc,
                        fecha = registroCicloTrabajoSIGOPLAN.fecha,
                        fechaString = ((DateTime)registroCicloTrabajoSIGOPLAN.fecha).ToShortDateString(),
                        cicloID = registroCicloTrabajoSIGOPLAN.cicloID,
                        revisor = registroCicloTrabajoSIGOPLAN.revisor,
                        revisorDesc = _context.tblP_Usuario.Where(x => x.id == registroCicloTrabajoSIGOPLAN.revisor).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault(),
                        colaborador = registroCicloTrabajoSIGOPLAN.colaborador,
                        colaboradorDesc = listaEmpleadosEnkontrol.Where(x => Convert.ToInt32(x.claveEmpleado) == registroCicloTrabajoSIGOPLAN.colaborador).Select(y => (string)y.nombreEmpleado).FirstOrDefault(),
                        tipoCiclo = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.id == registroCicloTrabajoSIGOPLAN.cicloID).Select(y => y.tipoCiclo).FirstOrDefault(),
                        calificacion = registroCicloTrabajoSIGOPLAN.calificacion,
                        economico = registroCicloTrabajoSIGOPLAN.economico,
                        acredito = registroCicloTrabajoSIGOPLAN.acredito,
                        retroalimentacion = registroCicloTrabajoSIGOPLAN.retroalimentacion,
                        observacionesRevisor = registroCicloTrabajoSIGOPLAN.observacionesRevisor,
                        accionesTomadas = registroCicloTrabajoSIGOPLAN.accionesTomadas,
                        observacionesLider = registroCicloTrabajoSIGOPLAN.observacionesLider,

                        accionRequerida = registroCicloTrabajoSIGOPLAN.accionRequerida,
                        metodo = registroCicloTrabajoSIGOPLAN.metodo,
                        cursoID = registroCicloTrabajoSIGOPLAN.cursoID,

                        listaRevisiones = listaRevisiones,
                        listaPropuestas = listaPropuestas,
                        listaAreas = listaAreas
                    };

                    resultado.Add("data", registroCicloTrabajo);
                }
                else
                {
                    throw new Exception("No se encuentra la información del registro del ciclo de trabajo.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getRegistroCicloTrabajoByID", e, AccionEnum.CONSULTA, 0, id);
            }

            return resultado;
        }

        #region Funcionalidad de Ciclos de trabajo
        public List<CicloTrabajoDTO> GetTablaCicloTrabajo()
        {
            List<CicloTrabajoDTO> listaCiclosTrabajo = new List<CicloTrabajoDTO>();

            try
            {
                List<tblS_CapacitacionSeguridadDNCicloTrabajo> listaCiclosTrabajoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.estatus && x.division == divisionActual).ToList();
                List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> listaCriterios = _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x => x.estatus).ToList();
                List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> listaAreas = _context.tblS_CapacitacionSeguridadDNCicloTrabajoAreas.Where(x => x.estatus).ToList();

                listaCiclosTrabajo = listaCiclosTrabajoSIGOPLAN.Select(x => new CicloTrabajoDTO
                {
                    id = x.id,
                    titulo = x.titulo,
                    tipoCiclo = (TipoCicloEnum)x.tipoCiclo,
                    tipoCicloDesc = (int)x.tipoCiclo == 1 ? TipoCicloEnum.periodico.ToString() : TipoCicloEnum.liberacion.ToString(),
                    descripcion = x.descripcion,
                    criterio = listaCriterios.Where(y => y.cicloTrabajoID == x.id).Select(w => w.descripcion).FirstOrDefault(),
                    ponderacion = listaCriterios.Where(y => y.cicloTrabajoID == x.id).Select(w => ((PonderacionCriterioEnum)w.ponderacion).GetDescription()).FirstOrDefault(),
                    area = listaAreas.Where(y => y.cicloTrabajoID == x.id).Select(w => w.area).FirstOrDefault(),
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapacitacionSeguridadController", "GetTablaCicloTrabajo", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }

            return listaCiclosTrabajo;
        }

        public List<CicloTrabajoCriterioDTO> GetTablaCriterioTrabajo(int id)
        {
            List<CicloTrabajoCriterioDTO> lst = new List<CicloTrabajoCriterioDTO>();
            try
            {

                List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> lstCicloTrabajoCriterio = _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x => x.estatus && x.cicloTrabajoID == id).ToList();
                lst = lstCicloTrabajoCriterio.Select(x => new CicloTrabajoCriterioDTO
                {
                    id = x.id,
                    descripcion = x.descripcion,
                    ponderacion = (PonderacionCriterioEnum)x.ponderacion,
                }).ToList();
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapacitacionSeguridadController", "GetTablaCicloTrabajo", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
            return lst;
        }

        public bool EliminarCicloTrabajo(int id)
        {
            try
            {
                tblS_CapacitacionSeguridadDNCicloTrabajo lstCiclo = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.id == id).FirstOrDefault();
                lstCiclo.estatus = false;
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogError(2, 0, "CapacitacionSeguridadController", "EliminarCicloTrabajo", e, AccionEnum.CONSULTA, 0, 0);
                return false;
            }
        }

        public bool EditarCicloTrabajo(CicloTrabajoDTO parametros, List<tblS_CapacitacionSeguridadDNCicloTrabajoCriterio> criterio, List<tblS_CapacitacionSeguridadDNCicloTrabajoAreas> lstAreass)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    tblS_CapacitacionSeguridadDNCicloTrabajo lst = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.Where(x => x.id == parametros.id).FirstOrDefault();
                    lst.descripcion = parametros.descripcion;
                    lst.tipoCiclo = (int)parametros.tipoCiclo;
                    _context.SaveChanges();

                    #region Criterios
                    var listaCriteriosAnteriores = _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Where(x => x.estatus && x.cicloTrabajoID == parametros.id).ToList();

                    foreach (var cri in listaCriteriosAnteriores)
                    {
                        cri.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var item in criterio)
                    {
                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoCriterio.Add(new tblS_CapacitacionSeguridadDNCicloTrabajoCriterio
                        {
                            descripcion = item.descripcion,
                            ponderacion = item.ponderacion,
                            cicloTrabajoID = parametros.id,
                            estatus = true
                        });
                        _context.SaveChanges();
                    }
                    #endregion

                    #region Áreas
                    var listaAreasAnteriores = _context.tblS_CapacitacionSeguridadDNCicloTrabajoAreas.Where(x => x.estatus && x.cicloTrabajoID == parametros.id).ToList();

                    foreach (var area in listaAreasAnteriores)
                    {
                        area.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var area in lstAreass)
                    {
                        _context.tblS_CapacitacionSeguridadDNCicloTrabajoAreas.Add(new tblS_CapacitacionSeguridadDNCicloTrabajoAreas
                        {
                            cc = area.cc,
                            area = area.area,
                            empresa = area.empresa,
                            cicloTrabajoID = parametros.id,
                            estatus = true
                        });
                        _context.SaveChanges();
                    }
                    #endregion

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(2, 0, "CapacitacionSeguridadController", "EditarCicloTrabajo", e, AccionEnum.CONSULTA, 0, 0);

                    return false;
                }
            }
        }

        public Dictionary<string, object> getListaDepartamientos(int listaAutorizacionID)
        {
            try
            {
                var listaAutorizacion = _context.tblS_CapacitacionSeguridadDNCicloTrabajo.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == listaAutorizacionID);

                if (listaAutorizacion != null)
                {
                    #region Departamentos Enkontrol
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "CONSTRUPLAN" }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "ARRENDADORA" }).ToList());
                    }
                    #endregion

                    var listaCC = _context.tblS_CapacitacionSeguridadDNCicloTrabajoAreas.Where(x => x.estatus && x.cicloTrabajoID == listaAutorizacion.id).ToList().Select(x => new CicloTrabajoDTO
                    {
                        id = x.id,
                        cc = x.cc.ToString(),
                        area = x.area,
                        departamento = departamentosAmbasEmpresas.Where(y => y.Value == x.area.ToString()).Select(z => z.Text).FirstOrDefault(),
                        empresa = x.empresa,
                        cicloID = x.cicloTrabajoID

                    }).ToList();

                    var datos = new listaDepartamentosDTO
                    {
                        lista = listaCC
                    };

                    resultado.Add("datos", datos);
                }
                else
                {
                    throw new Exception("No se encuentra la información de la lista de autorización.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public string obtenerPuestoDesc(int idpuesto)
        {
            string puestroDesc = "";

            var obj = _context.tblP_Puesto.Where(r => r.id == idpuesto).FirstOrDefault();
            if (obj != null)
            {
                puestroDesc = _context.tblP_Puesto.Where(r => r.id == idpuesto).Select(y => y.descripcion).FirstOrDefault(); ;
            }
            else
            {
                puestroDesc = "";
            }

            return puestroDesc;
        }

        public List<tblP_Usuario> llenarCorreos(int _IdUsuario)
        {

            try
            {
                List<int> lista = new List<int>();
                var listaHallazgos = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x => x.estatus && x.recorridoID == _IdUsuario).ToList();
                var listaLideres = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Where(x => x.estatus).ToList().Where(x => listaHallazgos.Select(y => y.id).Contains(x.hallazgoID)).Select(x => x.lider).Distinct().ToList();
                lista.AddRange(listaLideres);

                List<tblP_Usuario> data = new List<tblP_Usuario>();
                List<int> listaUnica = new List<int>();
                listaUnica = lista.Distinct().ToList();
                data = _context.tblP_Usuario.Where(x => listaUnica.Contains(x.id)).ToList();
                return data;
            }
            catch (Exception r)
            {

                throw;
            }

        }



        #endregion
        #endregion

        #region Detecciones Primarias
        public Dictionary<string, object> getRegistrosDeteccionesPrimarias(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            try
            {
                var listaDetecciones = _context.tblS_CapacitacionSeguridadDNDeteccionPrimaria.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                    (x.fecha.Month == fecha.Month && x.fecha.Year == fecha.Year)
                ).ToList();
                var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas.Where(x => x.estatus).ToList();
                var listaFiltrada = new List<tblS_CapacitacionSeguridadDNDeteccionPrimaria>();

                foreach (var deteccion in listaDetecciones)
                {
                    if (listaAreas != null)
                    {
                        var listaAreasPorDeteccionPrimaria = listaAreasSIGOPLAN.Where(x => x.deteccionPrimariaID == deteccion.id).ToList();

                        if (listaAreas.Any(x => listaAreasPorDeteccionPrimaria.Any(y => y.area == x.area && y.cc == x.cc && y.empresa == x.empresa)))
                        {
                            listaFiltrada.Add(deteccion);
                        }
                    }
                    else
                    {
                        listaFiltrada.Add(deteccion);
                    }
                }

                #region Departamentos
                var departamentosAmbasEmpresas = new List<ComboDTO>();

                //var odbc = new OdbcConsultaDTO();
                //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});
                //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});

                var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });
                var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });

                if (departamentosConstruplan.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1" }).ToList());
                }

                if (departamentosArrendadora.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1" }).ToList());
                }
                #endregion

                var datos = new List<dynamic>();

                foreach (var filt in listaFiltrada)
                {
                    var areas = "";
                    var areasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas.Where(x => x.estatus && x.deteccionPrimariaID == filt.id).ToList();

                    if (areasSIGOPLAN.Count() > 0)
                    {
                        var listaString = new List<string>();

                        foreach (var area in areasSIGOPLAN)
                        {
                            var areaEnkontrol = departamentosAmbasEmpresas.Where(y =>
                                y.Value == area.area.ToString() && y.Prefijo == area.empresa.ToString()
                            ).Select(z => z.Text + " (" + area.cc + ")").FirstOrDefault();

                            listaString.Add(areaEnkontrol);
                        }

                        areas = string.Join(", ", listaString);
                    }

                    datos.Add(new
                    {
                        id = filt.id,
                        area = 0,
                        areaDesc = areas,
                        fecha = filt.fecha,
                        fechaString = ((DateTime)filt.fecha).ToShortDateString()
                    });
                }

                resultado.Add("datos", datos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getRegistrosDeteccionesPrimarias", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, listaAreas = listaAreas, fecha = fecha });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarDeteccionPrimaria(tblS_CapacitacionSeguridadDNDeteccionPrimaria deteccionPrimaria, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad> listaNecesidades, List<tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas> listaAreas)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    deteccionPrimaria.division = divisionActual;
                    deteccionPrimaria.estatus = true;

                    _context.tblS_CapacitacionSeguridadDNDeteccionPrimaria.Add(deteccionPrimaria);
                    _context.SaveChanges();

                    foreach (var nec in listaNecesidades)
                    {
                        nec.deteccionPrimariaID = deteccionPrimaria.id;
                        nec.estatus = true;

                        _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad.Add(nec);
                        _context.SaveChanges();
                    }

                    foreach (var area in listaAreas)
                    {
                        area.deteccionPrimariaID = deteccionPrimaria.id;
                        area.estatus = true;

                        _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas.Add(area);
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarDeteccionPrimaria", e, AccionEnum.AGREGAR, 0, new { deteccionPrimaria = deteccionPrimaria, listaNecesidades = listaNecesidades });
                }
            }

            return resultado;
        }

        public NecesidadPrimariaReporteDTO getDeteccionPrimariaReporte(int deteccionPrimariaID)
        {
            try
            {
                var deteccionPrimariaSIGOPLAN = _context.tblS_CapacitacionSeguridadDNDeteccionPrimaria.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == deteccionPrimariaID);

                if (deteccionPrimariaSIGOPLAN != null)
                {
                    var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });

                    #region Departamentos
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var odbc = new OdbcConsultaDTO();
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1" }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "2" }).ToList());
                    }
                    #endregion

                    var proyectoObra = "";

                    if (deteccionPrimariaSIGOPLAN.empresa == 1)
                    {
                        proyectoObra = ccConstruplan.Where(x => (string)x.cc == deteccionPrimariaSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }
                    else if (deteccionPrimariaSIGOPLAN.empresa == 2)
                    {
                        proyectoObra = ccArrendadora.Where(x => (string)x.cc == deteccionPrimariaSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }

                    List<NecesidadDetectadaDTO> listaNecesidadesDetectadas = new List<NecesidadDetectadaDTO>();

                    var necesidadesDetectadasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad.Where(x => x.estatus && x.deteccionPrimariaID == deteccionPrimariaID).ToList();

                    if (necesidadesDetectadasSIGOPLAN.Count() > 0)
                    {
                        var listaCursos = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList();

                        listaNecesidadesDetectadas.AddRange(necesidadesDetectadasSIGOPLAN.Select(x => new NecesidadDetectadaDTO
                        {
                            metodo = ((MetodoEnum)x.metodo).GetDescription(),
                            detecciones = x.detecciones,
                            accionesCursoID = x.accionesCursoID,
                            acciones = listaCursos.Where(y => y.id == x.accionesCursoID).Select(z => z.claveCurso).FirstOrDefault(),
                            observaciones = x.observaciones
                        }).ToList());
                    }

                    var areas = "";
                    var areasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaAreas.Where(x => x.estatus && x.deteccionPrimariaID == deteccionPrimariaID).ToList();

                    if (areasSIGOPLAN.Count() > 0)
                    {
                        var listaString = new List<string>();

                        foreach (var area in areasSIGOPLAN)
                        {
                            var areaEnkontrol = departamentosAmbasEmpresas.Where(y =>
                                y.Value == area.area.ToString() && y.Prefijo == area.empresa.ToString()
                            ).Select(z => z.Text + " (" + area.cc + ")").FirstOrDefault();

                            listaString.Add(areaEnkontrol);
                        }

                        areas = string.Join(", ", listaString);
                    }

                    var deteccionPrimaria = new NecesidadPrimariaReporteDTO
                    {
                        proyectoObra = proyectoObra,
                        area = areas,
                        fecha = deteccionPrimariaSIGOPLAN.fecha.ToShortDateString(),
                        listaNecesidadesDetectadas = listaNecesidadesDetectadas
                    };

                    return deteccionPrimaria;
                }
                else
                {
                    throw new Exception("No se encuentra la información de la detección primaria.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "getDeteccionPrimariaReporte", e, AccionEnum.CONSULTA, deteccionPrimariaID, deteccionPrimariaID);
                return null;
            }
        }
        #endregion

        #region Recorridos
        public Dictionary<string, object> getRegistrosRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            try
            {
                var listaRecorridos = _context.tblS_CapacitacionSeguridadDNRecorrido.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                    (x.fecha.Month == fecha.Month && x.fecha.Year == fecha.Year)
                ).ToList();
                var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus).ToList();
                var listaFiltrada = new List<tblS_CapacitacionSeguridadDNRecorrido>();

                foreach (var recorrido in listaRecorridos)
                {
                    if (listaAreas != null)
                    {
                        var listaAreasPorCiclo = listaAreasSIGOPLAN.Where(x => x.recorridoID == recorrido.id).ToList();

                        if (listaAreas.Any(x => listaAreasPorCiclo.Any(y => y.area == x.area && y.cc == x.cc && y.empresa == x.empresa)))
                        {
                            listaFiltrada.Add(recorrido);
                        }
                    }
                    else
                    {
                        listaFiltrada.Add(recorrido);
                    }
                }

                if (realizador > 0)
                {
                    listaFiltrada = listaFiltrada.Where(x => x.realizador == realizador).ToList();
                }

                #region Departamentos
                var departamentosAmbasEmpresas = new List<ComboDTO>();

                //var odbc = new OdbcConsultaDTO();
                //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});
                //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                //{
                //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                //});

                var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });
                var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                });

                if (departamentosConstruplan.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = y.cc, Orden = 1 }).ToList());
                }

                if (departamentosArrendadora.Count > 0)
                {
                    departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = y.cc, Orden = 2 }).ToList());
                }
                #endregion

                var listaUsuarios = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                var datos = listaFiltrada.Select(x => new
                {
                    id = x.id,
                    idusu = listaUsuarios.Where(y => y.cveEmpleado == x.realizador.ToString()).Select(w => w.cveEmpleado).FirstOrDefault(),
                    //area = x.area,
                    //areaDesc = departamentosAmbasEmpresas.Where(y => y.Value == x.area.ToString() && y.Prefijo == x.cc && y.Orden == x.empresa).Select(z => z.Text + " (" + x.cc + ")").FirstOrDefault(),
                    areaDesc = string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                         listaAreasSIGOPLAN.Where(q => q.recorridoID == x.id).Select(w => w.area.ToString()).Contains(y.Value)
                    ).Select(z => z.Text + " (" + x.cc + ")")),
                    fecha = x.fecha,
                    fechaString = ((DateTime)x.fecha).ToShortDateString(),
                    realizador = x.realizador,
                    realizadorDesc = listaUsuarios.Where(y => y.id == x.realizador).Select(z => z.nombre + " " + z.apellidoPaterno + " " + z.apellidoMaterno).FirstOrDefault(),
                    cerrado = x.cerrado
                });

                resultado.Add("datos", datos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getRegistrosRecorridos", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, listaAreas = listaAreas, fecha = fecha, realizador = realizador });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (recorrido.realizador == 0)
                    {
                        throw new Exception("Debe capturar un realizador válido.");
                    }

                    if (listaAreas == null || listaAreas.Count() == 0)
                    {
                        throw new Exception("Debe seleccionar por lo menos una área.");
                    }

                    if (listaHallazgos.Count() != evidencias.Count())
                    {
                        throw new Exception("La cantidad de hallazgos no coincide con la cantidad de evidencias.");
                    }

                    var listaFormatosImagenes = new List<string> { ".JPG", ".JPEG", ".PNG" };

                    foreach (var archivo in evidencias)
                    {
                        var extension = Path.GetExtension(archivo.FileName).ToUpper();

                        if (!listaFormatosImagenes.Contains(extension))
                        {
                            throw new Exception("Solo se permiten imágenes (archivos con formato \".jpg\", \".jpeg\", \".png\").");
                        }
                    }

                    recorrido.division = divisionActual;
                    recorrido.estatus = true;

                    _context.tblS_CapacitacionSeguridadDNRecorrido.Add(recorrido);
                    _context.SaveChanges();

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var hal in listaHallazgos)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo("Evidencia_Hallazgo_" + (index + 1), evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaDeteccionNecesidadesRecorridos, nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(evidencias[index], rutaArchivoEvidencia));

                        var hallazgo = new tblS_CapacitacionSeguridadDNRecorridoHallazgo
                        {
                            deteccion = hal.deteccion,
                            recomendacion = hal.recomendacion,
                            clasificacion = (int)hal.clasificacion,
                            rutaEvidencia = rutaArchivoEvidencia,
                            evaluador = 0,
                            solventada = false,
                            recorridoID = recorrido.id,
                            estatus = true
                        };

                        _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Add(hallazgo);
                        _context.SaveChanges();

                        index++;

                        foreach (var lid in hal.listaLideres)
                        {
                            _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Add(new tblS_CapacitacionSeguridadDNRecorridoHallazgoLider
                            {
                                lider = lid,
                                hallazgoID = hallazgo.id,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    foreach (var area in listaAreas)
                    {
                        _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Add(new tblS_CapacitacionSeguridadDNRecorridoAreas
                        {
                            cc = area.cc,
                            area = area.area,
                            empresa = area.empresa,
                            recorridoID = recorrido.id,
                            estatus = true
                        });
                        _context.SaveChanges();
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
                    LogError(0, 0, NombreControlador, "guardarNuevoRecorrido", e, AccionEnum.AGREGAR, 0, new { recorrido = recorrido, listaHallazgos = listaHallazgos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarRecorrido(tblS_CapacitacionSeguridadDNRecorrido recorrido, List<RecorridoHallazgoDTO> listaHallazgos, List<tblS_CapacitacionSeguridadDNRecorridoAreas> listaAreas, List<HttpPostedFileBase> evidencias)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (listaAreas == null || listaAreas.Count() == 0)
                    {
                        throw new Exception("Debe seleccionar por lo menos una área.");
                    }

                    if (listaHallazgos.Count() != evidencias.Count())
                    {
                        throw new Exception("La cantidad de hallazgos no coincide con la cantidad de evidencias.");
                    }

                    var listaFormatosImagenes = new List<string> { ".JPG", ".JPEG", ".PNG" };

                    foreach (var archivo in evidencias)
                    {
                        var extension = Path.GetExtension(archivo.FileName).ToUpper();

                        if (!listaFormatosImagenes.Contains(extension))
                        {
                            throw new Exception("Solo se permiten imágenes (archivos con formato \".jpg\", \".jpeg\", \".png\").");
                        }
                    }

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var hal in listaHallazgos)
                    {
                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo("Evidencia_Hallazgo_" + (index + 1), evidencias[index].FileName);
                        string rutaArchivoEvidencia = Path.Combine(RutaDeteccionNecesidadesRecorridos, nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(evidencias[index], rutaArchivoEvidencia));

                        var hallazgo = new tblS_CapacitacionSeguridadDNRecorridoHallazgo
                        {
                            deteccion = hal.deteccion,
                            recomendacion = hal.recomendacion,
                            clasificacion = (int)hal.clasificacion,
                            rutaEvidencia = rutaArchivoEvidencia,
                            evaluador = 0,
                            solventada = false,
                            recorridoID = recorrido.id,
                            estatus = true
                        };

                        _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Add(hallazgo);
                        _context.SaveChanges();

                        index++;

                        foreach (var lid in hal.listaLideres)
                        {
                            _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Add(new tblS_CapacitacionSeguridadDNRecorridoHallazgoLider
                            {
                                lider = lid,
                                hallazgoID = hallazgo.id,
                                estatus = true
                            });
                            _context.SaveChanges();
                        }
                    }

                    var listaAreasAnterior = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus && x.recorridoID == recorrido.id).ToList();

                    foreach (var area in listaAreasAnterior)
                    {
                        area.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var area in listaAreas)
                    {
                        _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Add(new tblS_CapacitacionSeguridadDNRecorridoAreas
                        {
                            cc = area.cc,
                            area = area.area,
                            empresa = area.empresa,
                            recorridoID = recorrido.id,
                            estatus = true
                        });
                        _context.SaveChanges();
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
                    LogError(0, 0, NombreControlador, "editarRecorrido", e, AccionEnum.ACTUALIZAR, 0, new { recorrido = recorrido, listaHallazgos = listaHallazgos });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getRecorridoByID(int recorridoID)
        {
            try
            {
                var recorridoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorrido.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == recorridoID);

                if (recorridoSIGOPLAN != null)
                {
                    var privilegio = getPrivilegioActual();
                    var lideresSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Where(x => x.estatus).ToList();
                    var listaHallazgos = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x =>
                        x.estatus && x.recorridoID == recorridoSIGOPLAN.id).ToList().Select(x => new RecorridoHallazgoDTO
                        {
                            id = x.id,
                            deteccion = x.deteccion,
                            recomendacion = x.recomendacion,
                            clasificacion = (ClasificacionHallazgoEnum)x.clasificacion,
                            clasificacionDesc = ((ClasificacionHallazgoEnum)x.clasificacion).GetDescription(),
                            rutaEvidencia = x.rutaEvidencia,
                            evaluador = x.evaluador,
                            solventada = x.solventada,
                            recorridoID = x.recorridoID,
                            listaLideres =
                                lideresSIGOPLAN.Where(y => y.hallazgoID == x.id).ToList().Count() > 0 ?
                                    lideresSIGOPLAN.Where(y => y.hallazgoID == x.id).Select(z => z.lider).ToList() :
                                    new List<int>(),
                            lideresString = getLideresString(lideresSIGOPLAN.Where(y => y.hallazgoID == x.id).ToList()),
                            puedeEvaluar = privilegio.idPrivilegio == (int)PrivilegioEnum.Instructor || privilegio.idPrivilegio == (int)PrivilegioEnum.Administrador
                        }).ToList();
                    var listaAreas = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus && x.recorridoID == recorridoSIGOPLAN.id).ToList();

                    var realizadorDesc = _context.tblP_Usuario.Where(x => x.estatus && x.id == recorridoSIGOPLAN.realizador).Select(x =>
                        x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno
                    ).FirstOrDefault();

                    var recorrido = new RecorridoDTO
                    {
                        id = recorridoSIGOPLAN.id,
                        cc = recorridoSIGOPLAN.cc,
                        area = recorridoSIGOPLAN.area,
                        empresa = recorridoSIGOPLAN.empresa,
                        fecha = recorridoSIGOPLAN.fecha,
                        fechaString = recorridoSIGOPLAN.fecha.ToShortDateString(),
                        realizador = recorridoSIGOPLAN.realizador,
                        realizadorDesc = realizadorDesc,
                        listaHallazgos = listaHallazgos,
                        listaAreas = listaAreas
                    };

                    resultado.Add("datos", recorrido);
                }
                else
                {
                    throw new Exception("No se encuentra la información del recorrido.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getRecorridoByID", e, AccionEnum.CONSULTA, 0, recorridoID);
            }

            return resultado;
        }

        private string getLideresString(List<tblS_CapacitacionSeguridadDNRecorridoHallazgoLider> listaLideres)
        {
            if (listaLideres.Count() > 0)
            {
                var lideresString = "";
                var listaUsuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList().Where(x => listaLideres.Select(y => y.lider).Contains(x.id)).ToList();

                if (listaUsuariosSIGOPLAN.Count() > 0)
                {
                    lideresString = string.Join(", ", listaUsuariosSIGOPLAN.Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno));
                }

                return lideresString;
            }
            else
            {
                return "";
            }
        }

        public Dictionary<string, object> guardarSeguimientoRecorrido(List<tblS_CapacitacionSeguridadDNRecorridoHallazgo> listaSeguimiento)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var seg in listaSeguimiento)
                    {
                        var hallazgoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.FirstOrDefault(x => x.estatus && x.id == seg.id);

                        if (hallazgoSIGOPLAN != null)
                        {
                            hallazgoSIGOPLAN.evaluador = seg.evaluador;
                            hallazgoSIGOPLAN.solventada = seg.solventada;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del hallazgo.");
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
                    LogError(0, 0, NombreControlador, "guardarSeguimientoRecorrido", e, AccionEnum.AGREGAR, 0, listaSeguimiento);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> cargarDashboardRecorridos(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha, int realizador)
        {
            try
            {
                #region Filtrar la información de los recorridos
                var listaRecorridos = _context.tblS_CapacitacionSeguridadDNRecorrido.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                    (x.fecha.Month == fecha.Month && x.fecha.Year == fecha.Year)
                ).ToList();
                var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus).ToList();
                var listaRecorridosFiltrada = new List<tblS_CapacitacionSeguridadDNRecorrido>();

                foreach (var recorrido in listaRecorridos)
                {
                    if (listaAreas != null)
                    {
                        var listaAreasPorCiclo = listaAreasSIGOPLAN.Where(x => x.recorridoID == recorrido.id).ToList();

                        if (listaAreas.Any(x => listaAreasPorCiclo.Any(y => y.area == x.area && y.cc == x.cc && y.empresa == x.empresa)))
                        {
                            listaRecorridosFiltrada.Add(recorrido);
                        }
                    }
                    else
                    {
                        listaRecorridosFiltrada.Add(recorrido);
                    }
                }

                if (realizador > 0)
                {
                    listaRecorridosFiltrada = listaRecorridosFiltrada.Where(x => x.realizador == realizador).ToList();
                }
                #endregion

                var listaHallazgos = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x => x.estatus).ToList().Where(x =>
                    listaRecorridosFiltrada.Select(y => y.id).Contains(x.recorridoID)
                ).ToList();

                #region Tablas actos, condiciones y acciones
                var actosDetectados = listaHallazgos.Where(x => x.clasificacion == (int)ClasificacionHallazgoEnum.actoInseguro).ToList().Count();
                var actosSolventados = listaHallazgos.Where(x => x.clasificacion == (int)ClasificacionHallazgoEnum.actoInseguro && x.solventada).ToList().Count();
                var condicionesDetectadas = listaHallazgos.Where(x => x.clasificacion == (int)ClasificacionHallazgoEnum.condicionInsegura).ToList().Count();
                var condicionesSolventadas = listaHallazgos.Where(x => x.clasificacion == (int)ClasificacionHallazgoEnum.condicionInsegura && x.solventada).ToList().Count();
                var acciones = listaHallazgos.Where(x => x.clasificacion == (int)ClasificacionHallazgoEnum.accionEficienteSegura).ToList().Count();

                var listaActos = new List<dynamic> { new {
                        detectados = actosDetectados,
                        solventados = actosSolventados,
                        porcentajeCumplimiento = Math.Truncate(100 * (decimal)((actosSolventados / (actosDetectados > 0m ? actosDetectados : 1m)) * 100)) / 100
                    }
                };

                var listaCondiciones = new List<dynamic> { new {
                        detectados = condicionesDetectadas,
                        solventados = condicionesSolventadas,
                        porcentajeCumplimiento = Math.Truncate(100 * (decimal)((condicionesSolventadas / (condicionesDetectadas > 0m ? condicionesDetectadas : 1m)) * 100)) / 100
                    }
                };

                var listaAcciones = new List<dynamic> { new {
                        acciones = acciones
                    }
                };
                #endregion

                #region Gráfica Actos
                var graficaActos = new GraficaDTO();

                graficaActos.categorias.Add("# ACTOS DETECTADOS");
                graficaActos.serie1Descripcion = "";
                graficaActos.serie1.Add(actosDetectados);

                graficaActos.categorias.Add("# ACTOS SOLVENTADOS");
                graficaActos.serie1Descripcion = "";
                graficaActos.serie1.Add(actosSolventados);
                #endregion

                #region Gráfica Condiciones
                var graficaCondiciones = new GraficaDTO();

                graficaCondiciones.categorias.Add("# CONDICIONES DETECTADAS");
                graficaCondiciones.serie1Descripcion = "";
                graficaCondiciones.serie1.Add(condicionesDetectadas);

                graficaCondiciones.categorias.Add("# CONDICIONES SOLVENTADAS");
                graficaCondiciones.serie1Descripcion = "";
                graficaCondiciones.serie1.Add(condicionesSolventadas);
                #endregion

                resultado.Add("listaActos", listaActos);
                resultado.Add("listaCondiciones", listaCondiciones);
                resultado.Add("listaAcciones", listaAcciones);
                resultado.Add("graficaActos", graficaActos);
                resultado.Add("graficaCondiciones", graficaCondiciones);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarDashboardRecorridos", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, listaAreas = listaAreas, fecha = fecha, realizador = realizador });
            }

            return resultado;
        }

        public RecorridoReporteDTO getRecorridoReporte(int recorridoID)
        {
            try
            {
                var recorridoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorrido.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == recorridoID);

                if (recorridoSIGOPLAN != null)
                {
                    var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });

                    #region Departamentos
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var odbc = new OdbcConsultaDTO();
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1", TextoOpcional = y.cc }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "2", TextoOpcional = y.cc }).ToList());
                    }
                    #endregion

                    var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus).ToList();

                    var proyectoObra = "";
                    var primerArea = listaAreasSIGOPLAN.FirstOrDefault(x => x.recorridoID == recorridoSIGOPLAN.id);

                    if (primerArea.empresa == 1)
                    {
                        proyectoObra = ccConstruplan.Where(x => (string)x.cc == primerArea.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }
                    else if (primerArea.empresa == 2)
                    {
                        proyectoObra = ccArrendadora.Where(x => (string)x.cc == primerArea.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }

                    List<HallazgoReporteDTO> listaHallazgos = new List<HallazgoReporteDTO>();

                    var hallazgosSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x => x.estatus && x.recorridoID == recorridoID).ToList();

                    if (hallazgosSIGOPLAN.Count() > 0)
                    {
                        foreach (var hallazgo in hallazgosSIGOPLAN)
                        {
                            var stringListaLideres = "";
                            var listaLideresSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Where(x => x.estatus && x.hallazgoID == hallazgo.id).ToList();

                            if (listaLideresSIGOPLAN.Count() > 0)
                            {
                                var usuariosLideresSIGOPLAN = _context.tblP_Usuario.ToList().Where(x => listaLideresSIGOPLAN.Select(y => y.lider).Contains(x.id)).ToList();

                                stringListaLideres = string.Join(", ", usuariosLideresSIGOPLAN.Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno));
                            }

#if DEBUG
                            hallazgo.rutaEvidencia = hallazgo.rutaEvidencia.Replace("\\\\REPOSITORIO", "C:");
#endif

                            Image imagen = (Bitmap)((new ImageConverter()).ConvertFrom(File.ReadAllBytes(hallazgo.rutaEvidencia)));
                            Image imagenResize = ResizeImage(imagen);
                            byte[] datosImagenResize = ImageToByteArray(imagenResize);

                            listaHallazgos.Add(new HallazgoReporteDTO
                            {
                                deteccion = hallazgo.deteccion,
                                recomendacion = hallazgo.recomendacion,
                                clasificacion = ((ClasificacionHallazgoEnum)hallazgo.clasificacion).GetDescription(),
                                lideres = stringListaLideres,
                                evidencia = datosImagenResize //File.ReadAllBytes(hallazgo.rutaEvidencia)
                            });
                        }
                    }

                    var recorrido = new RecorridoReporteDTO
                    {
                        proyectoObra = proyectoObra,
                        //area =
                        //    departamentosAmbasEmpresas.Where(y =>
                        //        y.Value == recorridoSIGOPLAN.area.ToString() && y.Prefijo == recorridoSIGOPLAN.empresa.ToString()
                        //    ).Select(z => z.Text + " (" + recorridoSIGOPLAN.cc + ")").FirstOrDefault(),
                        area = string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                                listaAreasSIGOPLAN.Where(q => q.recorridoID == recorridoSIGOPLAN.id).Select(w => w.area.ToString()).Contains(y.Value)
                        ).Select(z => z.Text + " (" + z.TextoOpcional + ")")),
                        fecha = recorridoSIGOPLAN.fecha.ToShortDateString(),
                        realizador = _context.tblP_Usuario.Where(x => x.id == recorridoSIGOPLAN.realizador).Select(x =>
                            x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).FirstOrDefault(),
                        listaHallazgos = listaHallazgos
                    };

                    return recorrido;
                }
                else
                {
                    throw new Exception("No se encuentra la información del recorrido.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "getRecorridoReporte", e, AccionEnum.CONSULTA, recorridoID, recorridoID);
                return null;
            }
        }

        public Image ResizeImage(Image imagen)
        {
            Image imagenNueva = null;

            using (var imgAntes = imagen)
            {
                Bitmap myBitmap;
                ImageCodecInfo myImageCodecInfo;
                Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myBitmap = new Bitmap(imgAntes, 320, 240);
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                myEncoder = Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, 45L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                using (MemoryStream memStream = new MemoryStream())
                {
                    myBitmap.Save(memStream, myImageCodecInfo, myEncoderParameters);
                    Image newImage = Image.FromStream(memStream);
                    ImageAttributes imageAttributes = new ImageAttributes();
                    using (Graphics g = Graphics.FromImage(newImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(newImage, new Rectangle(Point.Empty, newImage.Size), 0, 0, newImage.Width, newImage.Height, GraphicsUnit.Pixel, imageAttributes);
                    }

                    imagenNueva = newImage;
                }
            }

            return imagenNueva;
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

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public bool enviarCorreoi(int recorridoID, List<Byte[]> archivo)
        {
            bool correoEnviado = false;

            try
            {
                var recorridoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorrido.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == recorridoID);

                if (recorridoSIGOPLAN != null)
                {
                    #region Correos
                    List<string> listaCorreos = new List<string>();

                    var listaHallazgos = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x => x.estatus && x.recorridoID == recorridoSIGOPLAN.id).ToList();
                    var listaLideres = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Where(x => x.estatus).ToList().Where(x =>
                        listaHallazgos.Select(y => y.id).Contains(x.hallazgoID)).Select(x => x.lider).Distinct().ToList();
                    var usuariosLideresSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus && x.correo != null).ToList().Where(x => listaLideres.Contains(x.id)).ToList();

                    listaCorreos.AddRange(usuariosLideresSIGOPLAN.Select(x => x.correo));

#if DEBUG
                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif
                    #endregion

                    #region Departamentos
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var odbc = new OdbcConsultaDTO();
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, TextoOpcional = y.cc, Prefijo = "1" }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, TextoOpcional = y.cc, Prefijo = "2" }).ToList());
                    }
                    #endregion

                    #region Centro de Costo
                    var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });

                    var proyectoObra = "";

                    if (recorridoSIGOPLAN.empresa == 1)
                    {
                        proyectoObra = ccConstruplan.Where(x => (string)x.cc == recorridoSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }
                    else if (recorridoSIGOPLAN.empresa == 2)
                    {
                        proyectoObra = ccArrendadora.Where(x => (string)x.cc == recorridoSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                    }
                    #endregion

                    var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus).ToList();

                    var asunto = string.Format(@"Reporte Recorrido Semanal - {0} - {1} - {2}",
                        recorridoSIGOPLAN.cc,
                        //departamentosAmbasEmpresas.Where(y => y.Value == recorridoSIGOPLAN.area.ToString() && y.Prefijo == recorridoSIGOPLAN.empresa.ToString()).Select(z => z.Text).FirstOrDefault(),
                        string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                            listaAreasSIGOPLAN.Where(q => q.recorridoID == recorridoSIGOPLAN.id).Select(w => w.area.ToString()).Contains(y.Value)
                        ).Select(z => z.Text + " (" + z.TextoOpcional + ")")),
                        recorridoSIGOPLAN.fecha.ToShortDateString()
                    );
                    var mensaje = string.Format(@"Proyecto: {0} <br/>Área: {1} <br/>Fecha: {2} <br/>Realizador: {3}",
                        proyectoObra,
                        //departamentosAmbasEmpresas.Where(y => y.Value == recorridoSIGOPLAN.area.ToString() && y.Prefijo == recorridoSIGOPLAN.empresa.ToString()).Select(z => z.Text).FirstOrDefault(),
                        string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                            listaAreasSIGOPLAN.Where(q => q.recorridoID == recorridoSIGOPLAN.id).Select(w => w.area.ToString()).Contains(y.Value)
                        ).Select(z => z.Text + " (" + z.TextoOpcional + ")")),
                        recorridoSIGOPLAN.fecha.ToShortDateString(),
                        _context.tblP_Usuario.Where(x => x.id == recorridoSIGOPLAN.realizador).Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).FirstOrDefault()
                    );

                    var archivosAdjuntos = new List<adjuntoCorreoDTO> { new adjuntoCorreoDTO
                        {
                            archivo = archivo[0],
                            extArchivo = ".pdf",
                            nombreArchivo = "Recorrido"
                        }
                    };

                    correoEnviado = GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos, archivosAdjuntos);
                }
                else
                {
                    throw new Exception("No se encuentra la información del recorrido.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, "CapacitacionSeguridadController", "enviarCorreoRecorrido", e, AccionEnum.CORREO, 0, recorridoID);
            }

            return correoEnviado;
        }

        public List<string> enviarCorreoRecorrido(int recorridoID, List<int> usuarios, List<Byte[]> downloadPDF)
        {
            var result = new List<string>();

            bool correoEnviado = false;

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var recorridoSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorrido.FirstOrDefault(x => x.estatus && x.division == divisionActual && x.id == recorridoID);
                    var contactos = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id)).ToList();
                    List<string> correo = new List<string>();

                    foreach (var c in contactos)
                    {
                        correo.Add(c.correo);
                    }
                    if (recorridoSIGOPLAN != null)
                    {
                        #region Correos


                        var listaHallazgos = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgo.Where(x => x.estatus && x.recorridoID == recorridoSIGOPLAN.id).ToList();
                        var listaLideres = _context.tblS_CapacitacionSeguridadDNRecorridoHallazgoLider.Where(x => x.estatus).ToList().Where(x =>
                            listaHallazgos.Select(y => y.id).Contains(x.hallazgoID)).Select(x => x.lider).Distinct().ToList();
                        var usuariosLideresSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus && x.correo != null).ToList().Where(x => listaLideres.Contains(x.id)).ToList();

                        correo.AddRange(usuariosLideresSIGOPLAN.Select(x => x.correo));

#if DEBUG
                        correo = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif
                        #endregion

                        #region Departamentos
                        var departamentosAmbasEmpresas = new List<ComboDTO>();

                        //var odbc = new OdbcConsultaDTO();
                        //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                        //{
                        //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                        //});
                        //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                        //{
                        //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                        //});

                        var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                        });
                        var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos"

                        });

                        if (departamentosConstruplan.Count > 0)
                        {
                            departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, TextoOpcional = y.cc, Prefijo = "1" }).ToList());
                        }

                        if (departamentosArrendadora.Count > 0)
                        {
                            departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, TextoOpcional = y.cc, Prefijo = "2" }).ToList());
                        }
                        #endregion

                        #region Centro de Costo
                        var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                        var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });

                        var proyectoObra = "";

                        if (recorridoSIGOPLAN.empresa == 1)
                        {
                            proyectoObra = ccConstruplan.Where(x => (string)x.cc == recorridoSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                        }
                        else if (recorridoSIGOPLAN.empresa == 2)
                        {
                            proyectoObra = ccArrendadora.Where(x => (string)x.cc == recorridoSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                        }
                        #endregion

                        var listaAreasSIGOPLAN = _context.tblS_CapacitacionSeguridadDNRecorridoAreas.Where(x => x.estatus).ToList();

                        var asunto = string.Format(@"Reporte Recorrido Semanal - {0} - {1} - {2}",
                            recorridoSIGOPLAN.cc,
                            //departamentosAmbasEmpresas.Where(y => y.Value == recorridoSIGOPLAN.area.ToString() && y.Prefijo == recorridoSIGOPLAN.empresa.ToString()).Select(z => z.Text).FirstOrDefault(),
                            string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                                listaAreasSIGOPLAN.Where(q => q.recorridoID == recorridoSIGOPLAN.id).Select(w => w.area.ToString()).Contains(y.Value)
                            ).Select(z => z.Text + " (" + z.TextoOpcional + ")")),
                            recorridoSIGOPLAN.fecha.ToShortDateString()
                        );
                        var mensaje = string.Format(@"Proyecto: {0} <br/>Área: {1} <br/>Fecha: {2} <br/>Realizador: {3}",
                            proyectoObra,
                            //departamentosAmbasEmpresas.Where(y => y.Value == recorridoSIGOPLAN.area.ToString() && y.Prefijo == recorridoSIGOPLAN.empresa.ToString()).Select(z => z.Text).FirstOrDefault(),
                            string.Join(", ", departamentosAmbasEmpresas.Where(y =>
                                listaAreasSIGOPLAN.Where(q => q.recorridoID == recorridoSIGOPLAN.id).Select(w => w.area.ToString()).Contains(y.Value)
                            ).Select(z => z.Text + " (" + z.TextoOpcional + ")")),
                            recorridoSIGOPLAN.fecha.ToShortDateString(),
                            _context.tblP_Usuario.Where(x => x.id == recorridoSIGOPLAN.realizador).Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).FirstOrDefault()
                        );



                        var archivosAdjuntos = new List<adjuntoCorreoDTO> { new adjuntoCorreoDTO
                            {                                
                                archivo = downloadPDF[0],
                                extArchivo = ".pdf",
                                nombreArchivo = "Recorrido"
                            }
                        };

                        correoEnviado = GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, correo, archivosAdjuntos);

                        if (correoEnviado)
                        {
                            recorridoSIGOPLAN.cerrado = true;
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del recorrido.");
                    }



                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "enviarCorreoRecorrido", e, AccionEnum.CORREO, 0, recorridoID);
                }
            }

            return result;
        }
        #endregion
        #endregion

        #region Competencias Operativas
        public Dictionary<string, object> getPromedioEvaluaciones(List<string> listaCC, List<AreaDTO> listaAreas, DateTime fecha)
        {
            try
            {
                var listaControlAsistencia = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x => x.activo && x.division == divisionActual).ToList().Where(x =>
                    x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                    (listaCC != null ? listaCC.Contains(x.cc) : true) &&
                    (x.fechaCapacitacion.Month == fecha.Month && x.fechaCapacitacion.Year == fecha.Year)
                ).ToList();
                var listaControlAsistenciaDetalle = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(x => x.division == divisionActual).ToList().Where(x =>
                    listaControlAsistencia.Select(y => y.id).Contains(x.controlAsistenciaID)
                ).ToList();

                #region Empleados
                //var listaEmpleadosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                //});
                //var listaEmpleadosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 2 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                //});

                var listaEmpleadosConstruplan = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                });
                listaEmpleadosConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                }));
                var listaEmpleadosArrendadora = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 2 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                });

                var listaEmpleados = new List<dynamic>();

                listaEmpleados.AddRange(listaEmpleadosConstruplan);
                listaEmpleados.AddRange(listaEmpleadosArrendadora);
                #endregion

                var listaControlAsistenciaDetalleFiltrada = new List<tblS_CapacitacionSeguridadControlAsistenciaDetalle>();

                foreach (var controlAsistenciaDet in listaControlAsistenciaDetalle)
                {
                    if (listaAreas != null)
                    {
                        foreach (var area in listaAreas)
                        {
                            var empleadoEnkontrol = listaEmpleados.FirstOrDefault(x => (int)x.clave_empleado == controlAsistenciaDet.claveEmpleado);

                            if (empleadoEnkontrol != null)
                            {
                                if (
                                    //(string)empleadoEnkontrol.cc == area.cc && 
                                    Convert.ToInt32(empleadoEnkontrol.clave_depto) == area.area && (int)empleadoEnkontrol.empresa == area.empresa)
                                {
                                    listaControlAsistenciaDetalleFiltrada.Add(controlAsistenciaDet);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        listaControlAsistenciaDetalleFiltrada.Add(controlAsistenciaDet);
                    }
                }

                decimal totalEvaluaciones = listaControlAsistenciaDetalleFiltrada.Count();


                var totalPersonal = listaControlAsistenciaDetalleFiltrada.DistinctBy(x => x.claveEmpleado).Count();

                var listaEvaluacionesInstructivosOperativos =
                    listaControlAsistenciaDetalleFiltrada.Where(x => x.controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.InstructivoOperativo).ToList();
                decimal porcentajeInstructivosOperativos = Math.Truncate(100m *
                    (listaEvaluacionesInstructivosOperativos.Sum(x => x.calificacion) /
                    ((decimal)listaEvaluacionesInstructivosOperativos.Count() > 0 ? (decimal)listaEvaluacionesInstructivosOperativos.Count() : 1))
                ) / 100m;

                var listaEvaluacionesTecnicosOperativos =
                    listaControlAsistenciaDetalleFiltrada.Where(x => x.controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.TecnicoOperativo).ToList();
                decimal porcentajeTecnicosOperativos = Math.Truncate(100m *
                    (listaEvaluacionesTecnicosOperativos.Sum(x => x.calificacion) /
                    ((decimal)listaEvaluacionesTecnicosOperativos.Count() > 0 ? (decimal)listaEvaluacionesTecnicosOperativos.Count() : 1))
                ) / 100m;

                var listaEvaluacionesProtocolosFatalidad =
                    listaControlAsistenciaDetalleFiltrada.Where(x => x.controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.ProtocoloFatalidad).ToList();
                decimal porcentajeProtocolosFatalidad = Math.Truncate(100m *
                    (listaEvaluacionesProtocolosFatalidad.Sum(x => x.calificacion) /
                    ((decimal)listaEvaluacionesProtocolosFatalidad.Count() > 0 ? (decimal)listaEvaluacionesProtocolosFatalidad.Count() : 1))
                ) / 100m;

                var listaEvaluacionesNormativo =
                    listaControlAsistenciaDetalleFiltrada.Where(x => x.controlAsistencia.curso.clasificacion == (int)ClasificacionCursoEnum.Normativo).ToList();
                decimal porcentajeNormativos = Math.Truncate(100m *
                    (listaEvaluacionesNormativo.Sum(x => x.calificacion) /
                    ((decimal)listaEvaluacionesNormativo.Count() > 0 ? (decimal)listaEvaluacionesNormativo.Count() : 1))
                ) / 100m;

                int divisor = 0;

                if (porcentajeInstructivosOperativos > 0)
                {
                    divisor++;
                }

                if (porcentajeTecnicosOperativos > 0)
                {
                    divisor++;
                }

                if (porcentajeProtocolosFatalidad > 0)
                {
                    divisor++;
                }

                if (porcentajeNormativos > 0)
                {
                    divisor++;
                }

                decimal porcentajePromedioGeneral = (porcentajeInstructivosOperativos + porcentajeTecnicosOperativos + porcentajeProtocolosFatalidad + porcentajeNormativos) / (divisor > 0 ? divisor : 1);

                resultado.Add("porcentajePromedioGeneral", porcentajePromedioGeneral);
                resultado.Add("totalPersonal", totalPersonal);
                resultado.Add("porcentajeInstructivosOperativos", porcentajeInstructivosOperativos);
                resultado.Add("porcentajeTecnicosOperativos", porcentajeTecnicosOperativos);
                resultado.Add("porcentajeProtocolosFatalidad", porcentajeProtocolosFatalidad);
                resultado.Add("porcentajeNormativos", porcentajeNormativos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getPromedioEvaluaciones", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, listaAreas = listaAreas, fecha = fecha });
            }

            return resultado;
        }

        //public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    HashSet<TKey> seenKeys = new HashSet<TKey>();
        //    foreach (TSource element in source)
        //    {
        //        if (seenKeys.Add(keySelector(element)))
        //        {
        //            yield return element;
        //        }
        //    }
        //}
        #endregion

        #region Indicador Hrs-Hombre
        public Dictionary<string, object> getEquiposCombo()
        {
            try
            {
                using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    var listaEquipos = _contextArrendadora.tblM_CatMaquina.Where(x => x.estatus == 1).Select(y => new ComboDTO
                    {
                        Value = y.id.ToString(),
                        Text = y.noEconomico
                    }).OrderBy(x => x.Text).ToList();

                    resultado.Add(ITEMS, listaEquipos);
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getEquiposCombo", e, AccionEnum.CONSULTA, 0, null);
            }

            return resultado;
        }

        public Dictionary<string, object> getEmpleadoPorClave(int claveEmpleado)
        {
            try
            {
                //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, 1 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //});
                //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, 2 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = claveEmpleado } }
                //});

                var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado}
                });
                empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                }));
                var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, nombre, ape_paterno, ape_materno, 2 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @claveEmpleado",
                    parametros = new { claveEmpleado }
                });

                var listaEmpleados = new List<dynamic>();

                listaEmpleados.AddRange(empleadoConstruplan);
                listaEmpleados.AddRange(empleadoArrendadora);

                if (listaEmpleados.Count() > 0)
                {
                    var empleado = listaEmpleados[0];
                    //var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.cveEmpleado == claveEmpleado.ToString());

                    //if (usuarioSIGOPLAN != null)
                    //{
                    var data = new
                    {
                        claveEmpleado = claveEmpleado,
                        nombre = (string)empleado.nombre + " " + (string)empleado.ape_paterno + " " + (string)empleado.ape_materno
                    };

                    resultado.Add("data", data);
                    //}
                    //else
                    //{
                    //    throw new Exception("El empleado no tiene usuario de SIGOPLAN.");
                    //}
                }
                else
                {
                    throw new Exception("No se encuentra el empleado.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> cargarHorasAdiestramiento(List<string> listaCC, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                //Se saca el último día del mes.
                fechaFinal = new DateTime(fechaFinal.Year, fechaFinal.Month, DateTime.DaysInMonth(fechaFinal.Year, fechaFinal.Month));

                #region Empleados
                //var listaEmpleadosConstruplan = _contextEnkontrol.Select<EmpleadoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, clave_depto, cc_contable, 1 AS empresa, estatus_empleado FROM sn_empleados"
                //});
                //var listaEmpleadosArrendadora = _contextEnkontrol.Select<EmpleadoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                //{
                //    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, clave_depto, cc_contable, 2 AS empresa, estatus_empleado FROM sn_empleados"
                //});

                var listaEmpleadosConstruplan = _context.Select<EmpleadoDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, clave_depto, cc_contable, 1 AS empresa, estatus_empleado FROM tblRH_EK_Empleados",
                });
                listaEmpleadosConstruplan.AddRange(_context.Select<EmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, clave_depto, cc_contable, 1 AS empresa, estatus_empleado FROM tblRH_EK_Empleados",
                }));
                var listaEmpleadosArrendadora = _context.Select<EmpleadoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, clave_depto, cc_contable, 2 AS empresa, estatus_empleado FROM tblRH_EK_Empleados",
                });

                var listaEmpleados = new List<EmpleadoDTO>();

                listaEmpleados.AddRange(listaEmpleadosConstruplan);
                listaEmpleados.AddRange(listaEmpleadosArrendadora);
                #endregion

                var listaEquipos = new List<tblM_CatMaquina>();
                using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    listaEquipos = _contextArrendadora.tblM_CatMaquina.Where(x => x.estatus == 1).ToList();
                }

                var privilegio = getPrivilegioActual();

                var listaColaboradorCapacitacion = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.Where(x => x.estatus && x.division == divisionActual).ToList().Where(x =>
                    //(listaCC != null ? listaCC.Contains(x.cc) : true) &&
                    (x.fechaCaptura.Date >= fechaInicial.Date && x.fechaCaptura.Date <= fechaFinal.Date)
                ).Select(x => new
                {
                    id = x.id,
                    colaborador = x.colaborador,
                    colaboradorDesc = listaEmpleados.Where(y => y.clave_empleado == x.colaborador).Select(z => z.nombre).FirstOrDefault(),
                    estatus_empleado = listaEmpleados.Where(y => y.clave_empleado == x.colaborador).Select(z => z.estatus_empleado).FirstOrDefault(),
                    cc = listaEmpleados.Where(y => y.clave_empleado == x.colaborador).Select(z => z.cc_contable).FirstOrDefault(),
                    equipo = x.equipo,
                    equipoDesc = listaEquipos.Where(y => y.id == x.equipo).Select(z => z.noEconomico).FirstOrDefault(),
                    fechaInicio = x.fechaInicio,
                    fechaInicioString = x.fechaInicio.ToShortDateString(),
                    fechaTermino = x.fechaTermino,
                    fechaTerminoString = x.fechaTermino.ToShortDateString(),
                    adiestrador = x.adiestrador,
                    adiestradorDesc = listaEmpleados.Where(y => y.clave_empleado == x.adiestrador).Select(z => (string)z.nombre).FirstOrDefault(),
                    liberado = x.liberado,
                    rutaSoporteAdiestramiento = x.rutaSoporteAdiestramiento,
                    puedeEvaluar = privilegio.idPrivilegio == (int)PrivilegioEnum.Instructor || privilegio.idPrivilegio == (int)PrivilegioEnum.Administrador
                }).ToList();

                if (listaCC != null)
                {
                    listaColaboradorCapacitacion = listaColaboradorCapacitacion.Where(x => listaCC.Contains(x.cc)).ToList();
                }

                var listaControlHoras = _context.tblS_CapacitacionSeguridadIHHControlHoras.Where(x => x.estatus).ToList();
                var listaPendientes = new List<dynamic>();
                var listaCompletos = new List<dynamic>();

                foreach (var col in listaColaboradorCapacitacion)
                {
                    var controlHoras = listaControlHoras.Where(x => x.colaboradorCapacitacionID == col.id).ToList();

                    if (controlHoras.Count() > 0)
                    {
                        if (controlHoras.Sum(x => x.horas) < 250)
                        {
                            listaPendientes.Add(col);
                        }
                        else if (controlHoras.Sum(x => x.horas) >= 250)
                        {
                            listaCompletos.Add(col);
                        }
                    }
                    else
                    {
                        listaPendientes.Add(col);
                    }
                }

                #region Gráfica Dashboard
                //var listaColaboradorCapacitacionFiltrados = listaColaboradorCapacitacion.Where(x => x.estatus_empleado == "A").ToList();

                var graficaDashboard = new List<GraficaPastelDTO> {
                    new GraficaPastelDTO() {
                        name = "EN ADIESTRAMIENTO",
                        y = listaPendientes.Where(x => x.estatus_empleado == "A").Count() //y = listaColaboradorCapacitacionFiltrados.Where(x => !x.liberado).Count()
                    },
                    new GraficaPastelDTO() {
                        name = "LIBERADOS",
                        y = listaColaboradorCapacitacion.Where(x => x.liberado).Count() //y = listaColaboradorCapacitacionFiltrados.Where(x => x.liberado).Count()
                    }
                };
                #endregion

                resultado.Add("pendientes", listaPendientes);
                resultado.Add("liberados", listaCompletos); //Los "liberados" son los completos que tienen 250 horas acumuladas.
                resultado.Add("dashboard", graficaDashboard);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarHorasAdiestramiento", e, AccionEnum.CONSULTA, 0, new { listaCC = listaCC, fechaInicial = fechaInicial, fechaFinal = fechaFinal });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoColaboradorCapacitacion(tblS_CapacitacionSeguridadIHHColaboradorCapacitacion colaboradorCapacitacion)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_empleado, clave_depto, 1 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                    //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = colaboradorCapacitacion.colaborador } }
                    //});
                    //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_empleado, clave_depto, 2 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                    //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = colaboradorCapacitacion.colaborador } }
                    //});

                    var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_empleado, clave_depto, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                        parametros = new { colaborador = colaboradorCapacitacion.colaborador }
                    });
                    empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT clave_empleado, clave_depto, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                        parametros = new { colaborador = colaboradorCapacitacion.colaborador }
                    }));
                    var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado, clave_depto, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                        parametros = new { colaborador = colaboradorCapacitacion.colaborador }
                    });

                    var listaEmpleados = new List<dynamic>();

                    listaEmpleados.AddRange(empleadoConstruplan);
                    listaEmpleados.AddRange(empleadoArrendadora);

                    colaboradorCapacitacion.area = Convert.ToInt32(listaEmpleados[0].clave_depto);
                    colaboradorCapacitacion.fechaCaptura = DateTime.Now;
                    colaboradorCapacitacion.division = divisionActual;
                    colaboradorCapacitacion.estatus = true;

                    _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.Add(colaboradorCapacitacion);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add("colaboradorCapacitacionID", colaboradorCapacitacion.id);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarNuevoColaboradorCapacitacion", e, AccionEnum.AGREGAR, 0, new { colaboradorCapacitacion = colaboradorCapacitacion });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getInfoColaboradorCapacitacion(int colaboradorCapacitacionID)
        {
            try
            {
                var colaboradorCapacitacionSIGOPLAN = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x =>
                    x.estatus && x.division == divisionActual && x.id == colaboradorCapacitacionID
                );

                if (colaboradorCapacitacionSIGOPLAN != null)
                {
                    var listaControlHoras = _context.tblS_CapacitacionSeguridadIHHControlHoras.Where(x => x.estatus && x.colaboradorCapacitacionID == colaboradorCapacitacionID).ToList();
                    var horasAcumuladas = listaControlHoras.Sum(x => x.horas);
                    var horasPendientes = 250m - horasAcumuladas;

                    resultado.Add("horasAcumuladas", horasAcumuladas);
                    resultado.Add("horasPendientes", horasPendientes < 0 ? 0 : horasPendientes);
                }
                else
                {
                    throw new Exception("No se encuentra la información del colaborador.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "getInfoColaboradorCapacitacion", e, AccionEnum.CONSULTA, 0, new { colaboradorCapacitacionID = colaboradorCapacitacionID });
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoControlHoras(List<tblS_CapacitacionSeguridadIHHControlHoras> listaControlHoras)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (listaControlHoras.Any(x => x.colaboradorCapacitacionID == 0))
                    {
                        throw new Exception("No se escogió el colaborador.");
                    }

                    foreach (var control in listaControlHoras)
                    {
                        control.estatus = true;

                        _context.tblS_CapacitacionSeguridadIHHControlHoras.Add(control);
                        _context.SaveChanges();
                    }

                    dbContextTransaction.Commit();

                    #region Enviar correo a los cinco "autorizantes".
                    var colaboradorCapacitacionID = listaControlHoras[0].colaboradorCapacitacionID;

                    var colaboradorCapacitacionSIGOPLAN = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x =>
                        x.estatus && x.division == divisionActual && x.id == colaboradorCapacitacionID
                    );

                    if (colaboradorCapacitacionSIGOPLAN != null)
                    {
                        #region Correos
                        var usuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus).ToList();

                        var instructorSIGOPLAN = usuariosSIGOPLAN.FirstOrDefault(x => x.cveEmpleado == colaboradorCapacitacionSIGOPLAN.instructor.ToString());
                        var seguridadSIGOPLAN = usuariosSIGOPLAN.FirstOrDefault(x => x.cveEmpleado == colaboradorCapacitacionSIGOPLAN.seguridad.ToString());
                        var recursosHumanosSIGOPLAN = usuariosSIGOPLAN.FirstOrDefault(x => x.cveEmpleado == colaboradorCapacitacionSIGOPLAN.recursosHumanos.ToString());
                        var sobrestanteSIGOPLAN = usuariosSIGOPLAN.FirstOrDefault(x => x.cveEmpleado == colaboradorCapacitacionSIGOPLAN.sobrestante.ToString());
                        var gerenteObraSIGOPLAN = usuariosSIGOPLAN.FirstOrDefault(x => x.cveEmpleado == colaboradorCapacitacionSIGOPLAN.gerenteObra.ToString());

                        List<string> listaCorreos = new List<string>();

                        if (instructorSIGOPLAN != null)
                        {
                            if (instructorSIGOPLAN.correo.Contains("@"))
                            {
                                listaCorreos.Add(instructorSIGOPLAN.correo);
                            }
                        }

                        if (seguridadSIGOPLAN != null)
                        {
                            if (seguridadSIGOPLAN.correo.Contains("@"))
                            {
                                listaCorreos.Add(seguridadSIGOPLAN.correo);
                            }
                        }

                        if (recursosHumanosSIGOPLAN != null)
                        {
                            if (recursosHumanosSIGOPLAN.correo.Contains("@"))
                            {
                                listaCorreos.Add(recursosHumanosSIGOPLAN.correo);
                            }
                        }

                        if (sobrestanteSIGOPLAN != null)
                        {
                            if (sobrestanteSIGOPLAN.correo.Contains("@"))
                            {
                                listaCorreos.Add(sobrestanteSIGOPLAN.correo);
                            }
                        }

                        if (gerenteObraSIGOPLAN != null)
                        {
                            if (gerenteObraSIGOPLAN.correo.Contains("@"))
                            {
                                listaCorreos.Add(gerenteObraSIGOPLAN.correo);
                            }
                        }

#if DEBUG
                        listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
#endif
                        #endregion

                        #region Empleado Enkontrol
                        //var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                        //{
                        //    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, 1 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                        //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = colaboradorCapacitacionSIGOPLAN.colaborador } }
                        //});
                        //var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                        //{
                        //    consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, 2 AS empresa FROM sn_empleados WHERE clave_empleado = ?",
                        //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Numeric, valor = colaboradorCapacitacionSIGOPLAN.colaborador } }
                        //});

                        var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                            parametros = new { colaborador = colaboradorCapacitacionSIGOPLAN.colaborador }
                        });
                        empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.GCPLAN,
                            consulta = @"SELECT clave_empleado, clave_depto, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                            parametros = new { colaborador = colaboradorCapacitacionSIGOPLAN.colaborador }
                        }));
                        var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                        {
                            baseDatos = MainContextEnum.Arrendadora,
                            consulta = @"SELECT clave_empleado, nombre + ' ' + ape_paterno + ' ' + ape_materno AS nombre, 1 AS empresa FROM tblRH_EK_Empleados WHERE clave_empleado = @colaborador",
                            parametros = new { colaborador = colaboradorCapacitacionSIGOPLAN.colaborador }
                        });

                        var listaEmpleados = new List<dynamic>();

                        listaEmpleados.AddRange(empleadoConstruplan);
                        listaEmpleados.AddRange(empleadoArrendadora);

                        var colaboradorDesc = (string)listaEmpleados[0].nombre;
                        #endregion

                        #region Centro de costo y área
                        List<dynamic> centrosCostoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                        List<dynamic> centrosCostoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                        var departamentosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                        {
                            consulta = @"SELECT * FROM sn_departamentos"
                        });
                        var departamentosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                        {
                            consulta = @"SELECT * FROM sn_departamentos"
                        });

                        var ccDesc = "";
                        var areaDesc = "";

                        if (colaboradorCapacitacionSIGOPLAN.empresa == (int)EmpresaEnum.Construplan)
                        {
                            ccDesc = centrosCostoConstruplan.Where(x => (string)x.cc == colaboradorCapacitacionSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                            areaDesc = departamentosConstruplan.Where(x => (string)x.clave_depto == colaboradorCapacitacionSIGOPLAN.area.ToString()).Select(x => (string)x.desc_depto).FirstOrDefault();
                        }
                        else if (colaboradorCapacitacionSIGOPLAN.empresa == (int)EmpresaEnum.Arrendadora)
                        {
                            ccDesc = centrosCostoArrendadora.Where(x => (string)x.cc == colaboradorCapacitacionSIGOPLAN.cc).Select(x => (string)x.cc + "-" + (string)x.descripcion).FirstOrDefault();
                            areaDesc = departamentosArrendadora.Where(x => (string)x.clave_depto == colaboradorCapacitacionSIGOPLAN.area.ToString()).Select(x => (string)x.desc_depto).FirstOrDefault();
                        }
                        #endregion

                        var totalHorasAdiestramiento = _context.tblS_CapacitacionSeguridadIHHControlHoras.Where(x =>
                            x.estatus && x.colaboradorCapacitacionID == colaboradorCapacitacionID
                        ).Sum(x => x.horas);

                        var asunto = string.Format(@"Indicador Hrs-Hombre - {0} - {1} - {2}",
                            colaboradorDesc,
                            colaboradorCapacitacionSIGOPLAN.fechaInicio.ToShortDateString(),
                            colaboradorCapacitacionSIGOPLAN.fechaTermino.ToShortDateString());
                        var mensaje = string.Format(@"
                            Proyecto: {0}<br/>
                            Área: {1}<br/>
                            Fecha: {2}<br/>
                            Colaborador: {3}<br/>
                            Total de horas de adiestramiento: {4}", ccDesc, areaDesc, DateTime.Now.ToShortDateString(), colaboradorDesc, totalHorasAdiestramiento
                        );

                        var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos.Distinct().ToList());
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del colaborador.");
                    }
                    #endregion

                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarNuevoControlHoras", e, AccionEnum.AGREGAR, 0, new { listaControlHoras = listaControlHoras });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarLiberados(List<tblS_CapacitacionSeguridadIHHColaboradorCapacitacion> captura, List<HttpPostedFileBase> archivos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    foreach (var cap in captura)
                    {
                        string nombreArchivo = ObtenerFormatoNombreArchivo(NombreBaseArchivoLiberado, archivos[index].FileName);
                        string rutaArchivo = Path.Combine(RutaColaboradorLiberado, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(archivos[index], rutaArchivo));

                        var colaboradorCapacitacionSIGOPLAN = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x => x.estatus && x.id == cap.id);

                        if (colaboradorCapacitacionSIGOPLAN != null)
                        {
                            colaboradorCapacitacionSIGOPLAN.liberado = cap.liberado;
                            colaboradorCapacitacionSIGOPLAN.rutaSoporteAdiestramiento = rutaArchivo;
                            _context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del colaborador.");
                        }

                        index++;
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
                    LogError(0, 0, NombreControlador, "guardarLiberados", e, AccionEnum.AGREGAR, 0, new { captura = captura });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> cargarDatosArchivoSoporteAdiestramiento(int id)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x => x.id == id);

                Stream fileStream = GlobalUtils.GetFileAsStream(captura.rutaSoporteAdiestramiento);
                var byteArray = GlobalUtils.ConvertFileToByte(fileStream);

                resultado.Add("archivo", byteArray);
                resultado.Add("extension", Path.GetExtension(captura.rutaSoporteAdiestramiento).ToUpper());
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }
            return resultado;
        }

        public Tuple<Stream, string> descargarArchivoSoporteAdiestramiento(int id)
        {
            try
            {
                var captura = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x => x.id == id);

                var fileStream = GlobalUtils.GetFileAsStream(captura.rutaSoporteAdiestramiento);
                string name = Path.GetFileName(captura.rutaSoporteAdiestramiento);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ColaboradorCapacitacionReporteDTO getColaboradorCapacitacionReporte(int colaboradorCapacitacionID)
        {
            try
            {
                var colaboradorCapacitacionSIGOPLAN = _context.tblS_CapacitacionSeguridadIHHColaboradorCapacitacion.FirstOrDefault(x =>
                    x.estatus && x.division == divisionActual && x.id == colaboradorCapacitacionID
                );

                if (colaboradorCapacitacionSIGOPLAN != null)
                {
                    #region Departamentos
                    var departamentosAmbasEmpresas = new List<ComboDTO>();

                    //var odbc = new OdbcConsultaDTO();
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM DBA.sn_departamentos"
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos",
                    });
                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos",
                    });

                    if (departamentosConstruplan.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosConstruplan.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "1" }).ToList());
                    }

                    if (departamentosArrendadora.Count > 0)
                    {
                        departamentosAmbasEmpresas.AddRange(departamentosArrendadora.Select(y => new ComboDTO { Value = y.id, Text = y.departamento, Prefijo = "2" }).ToList());
                    }
                    #endregion

                    #region Proyecto
                    var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                    var proyectoObra = "";

                    if (colaboradorCapacitacionSIGOPLAN.empresa == 1)
                    {
                        proyectoObra = ccConstruplan.Where(x => (string)x.cc == colaboradorCapacitacionSIGOPLAN.cc).Select(x => (string)x.descripcion).FirstOrDefault();
                    }
                    else if (colaboradorCapacitacionSIGOPLAN.empresa == 2)
                    {
                        proyectoObra = ccArrendadora.Where(x => (string)x.cc == colaboradorCapacitacionSIGOPLAN.cc).Select(x => (string)x.descripcion).FirstOrDefault();
                    }
                    #endregion

                    #region Equipo
                    var listaEquipos = new List<tblM_CatMaquina>();
                    using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                    {
                        listaEquipos = _contextArrendadora.tblM_CatMaquina.Where(x => x.estatus == 1).ToList();
                    }
                    #endregion

                    #region Empleados
//                    var empleadoConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
//                        {
//                            consulta = @"
//                                SELECT
//                                    emp.clave_empleado,
//                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno AS nombre,
//                                    emp.puesto,
//                                    pue.descripcion AS puestoDesc,
//                                    1 AS empresa 
//                                FROM sn_empleados emp
//                                    LEFT JOIN si_puestos pue ON emp.puesto = pue.puesto"
//                        });
//                    var empleadoArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
//                    {
//                        consulta = @"
//                                SELECT
//                                    emp.clave_empleado,
//                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno AS nombre,
//                                    emp.puesto,
//                                    pue.descripcion AS puestoDesc,
//                                    2 AS empresa 
//                                FROM sn_empleados emp
//                                    LEFT JOIN si_puestos pue ON emp.puesto = pue.puesto"
//                    });

                    var empleadoConstruplan = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                    emp.clave_empleado,
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno AS nombre,
                                    emp.puesto,
                                    pue.descripcion AS puestoDesc,
                                    1 AS empresa 
                                FROM tblRH_EK_Empleados emp
                                    LEFT JOIN tblRH_EK_Puestos pue ON emp.puesto = pue.puesto",
                    });
                    empleadoConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT
                                    emp.clave_empleado,
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno AS nombre,
                                    emp.puesto,
                                    pue.descripcion AS puestoDesc,
                                    1 AS empresa 
                                FROM tblRH_EK_Empleados emp
                                    LEFT JOIN tblRH_EK_Puestos pue ON emp.puesto = pue.puesto",
                    }));
                    var empleadoArrendadora = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT
                                    emp.clave_empleado,
                                    emp.nombre + ' ' + emp.ape_paterno + ' ' + emp.ape_materno AS nombre,
                                    emp.puesto,
                                    pue.descripcion AS puestoDesc,
                                    1 AS empresa 
                                FROM tblRH_EK_Empleados emp
                                    LEFT JOIN tblRH_EK_Puestos pue ON emp.puesto = pue.puesto",
                    });

                    var listaEmpleados = new List<dynamic>();

                    listaEmpleados.AddRange(empleadoConstruplan);
                    listaEmpleados.AddRange(empleadoArrendadora);
                    #endregion

                    var colaboradorCapacitacion = new ColaboradorCapacitacionReporteDTO
                    {
                        departamento =
                            departamentosAmbasEmpresas.Where(y =>
                                y.Value == colaboradorCapacitacionSIGOPLAN.area.ToString() && y.Prefijo == colaboradorCapacitacionSIGOPLAN.empresa.ToString()
                            ).Select(z => z.Text).FirstOrDefault(),
                        proyecto = proyectoObra,
                        clave = colaboradorCapacitacionSIGOPLAN.colaborador.ToString(),
                        nombre = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.colaborador).Select(z => (string)z.nombre).FirstOrDefault(),
                        puesto = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.colaborador).Select(z => (string)z.puestoDesc).FirstOrDefault(),
                        equipo = listaEquipos.Where(y => y.id == colaboradorCapacitacionSIGOPLAN.equipo).Select(z => z.noEconomico).FirstOrDefault(),
                        inicio = colaboradorCapacitacionSIGOPLAN.fechaInicio.ToShortDateString(),
                        termino = colaboradorCapacitacionSIGOPLAN.fechaTermino.ToShortDateString(),
                        adiestrador = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.adiestrador).Select(z => (string)z.nombre).FirstOrDefault(),
                        instructor = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.instructor).Select(z => (string)z.nombre).FirstOrDefault(),
                        seguridad = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.seguridad).Select(z => (string)z.nombre).FirstOrDefault(),
                        capitalHumano = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.recursosHumanos).Select(z => (string)z.nombre).FirstOrDefault(),
                        sobrestante = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.sobrestante).Select(z => (string)z.nombre).FirstOrDefault(),
                        gerenteObra = listaEmpleados.Where(y => Convert.ToInt32(y.clave_empleado) == colaboradorCapacitacionSIGOPLAN.gerenteObra).Select(z => (string)z.nombre).FirstOrDefault()
                    };

                    return colaboradorCapacitacion;
                }
                else
                {
                    throw new Exception("No se encuentra la información del colaborador.");
                }
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "getColaboradorCapacitacionReporte", e, AccionEnum.CONSULTA, colaboradorCapacitacionID, colaboradorCapacitacionID);
                return null;
            }
        }

        public List<HorasHombreDTO> postObtenerTablaHorasHombre(HorasHombreDTO parametros, bool ActivarHeader)
        {
            try
            {
                #region DECLARACION DE OBJETOS Y LISTAS
                List<HorasHombreDTO> lstRetornar = new List<HorasHombreDTO>();
                HorasHombreDTO obj = new HorasHombreDTO();
                HorasHombreAnualDTO objAnual = new HorasHombreAnualDTO();
                List<HorasHombreDIF> lstHorasHombre = new List<HorasHombreDIF>();
                HorasHombreDIF objHorasHombre;
                string CentroDeCosto = "";
                int totalPersonalOperativo = 0;
                int totalGlobalPersonalOperativo = 0;
                #endregion

                #region Agregar TABLA Header
                if (ActivarHeader == true)
                {
                    obj.centrocosto = "CC";
                    obj.año = "AÑO";
                    obj.descripcion = "DESCRIPCION";
                    obj.totalPersonal = "TOTAL PERSONAL OPERATIVO";

                    obj.lstGlobal = new HorasHombreAnualDTO();
                    obj.lstGlobal.descripcion = "GLOBAL";
                    obj.lstGlobal.HrsCap = "HRS CAP";
                    obj.lstGlobal.HrsTrab = "HRS TRAB";

                    obj.lstEnero = new HorasHombreAnualDTO();
                    obj.lstEnero.descripcion = "ENERO";
                    obj.lstEnero.HrsCap = "HRS CAP";
                    obj.lstEnero.HrsTrab = "HRS TRAB";

                    obj.lstFebrero = new HorasHombreAnualDTO();
                    obj.lstFebrero.descripcion = "FEBRERO";
                    obj.lstFebrero.HrsCap = "HRS CAP";
                    obj.lstFebrero.HrsTrab = "HRS TRAB";

                    obj.lstMarzo = new HorasHombreAnualDTO();
                    obj.lstMarzo.descripcion = "MARZO";
                    obj.lstMarzo.HrsCap = "HRS CAP";
                    obj.lstMarzo.HrsTrab = "HRS TRAB";

                    obj.lstAbril = new HorasHombreAnualDTO();
                    obj.lstAbril.descripcion = "ABRIL";
                    obj.lstAbril.HrsCap = "HRS CAP";
                    obj.lstAbril.HrsTrab = "HRS TRAB";

                    obj.lstMayo = new HorasHombreAnualDTO();
                    obj.lstMayo.descripcion = "MAYO";
                    obj.lstMayo.HrsCap = "HRS CAP";
                    obj.lstMayo.HrsTrab = "HRS TRAB";

                    obj.lstJunio = new HorasHombreAnualDTO();
                    obj.lstJunio.descripcion = "JUNIO";
                    obj.lstJunio.HrsCap = "HRS CAP";
                    obj.lstJunio.HrsTrab = "HRS TRAB";

                    obj.lstJulio = new HorasHombreAnualDTO();
                    obj.lstJulio.descripcion = "JULIO";
                    obj.lstJulio.HrsCap = "HRS CAP";
                    obj.lstJulio.HrsTrab = "HRS TRAB";

                    obj.lstAgosto = new HorasHombreAnualDTO();
                    obj.lstAgosto.descripcion = "AGOSTO";
                    obj.lstAgosto.HrsCap = "HRS CAP";
                    obj.lstAgosto.HrsTrab = "HRS TRAB";

                    obj.lstSeptiembre = new HorasHombreAnualDTO();
                    obj.lstSeptiembre.descripcion = "SEPTIEMBRE";
                    obj.lstSeptiembre.HrsCap = "HRS CAP";
                    obj.lstSeptiembre.HrsTrab = "HRS TRAB";

                    obj.lstOctubre = new HorasHombreAnualDTO();
                    obj.lstOctubre.descripcion = "OCTUBRE";
                    obj.lstOctubre.HrsCap = "HRS CAP";
                    obj.lstOctubre.HrsTrab = "HRS TRAB";

                    obj.lstNoviembre = new HorasHombreAnualDTO();
                    obj.lstNoviembre.descripcion = "NOVIEMBRE";
                    obj.lstNoviembre.HrsCap = "HRS CAP";
                    obj.lstNoviembre.HrsTrab = "HRS TRAB";

                    obj.lstDiciembre = new HorasHombreAnualDTO();
                    obj.lstDiciembre.descripcion = "DICIEMBRE";
                    obj.lstDiciembre.HrsCap = "HRS CAP";
                    obj.lstDiciembre.HrsTrab = "HRS TRAB";
                    lstRetornar.Add(obj);
                }
                #endregion

                #region obtenerMesesDelAño

                for (int i = 1; i <= 12; i++)
                {
                    int DiaFinal = System.DateTime.DaysInMonth(Convert.ToInt32(parametros.año), i);
                    objHorasHombre = new HorasHombreDIF();
                    objHorasHombre.FechaInicio = Convert.ToDateTime(parametros.año + "-" + i + "-01");
                    objHorasHombre.FechaFin = Convert.ToDateTime(parametros.año + "-" + i + "-" + DiaFinal + "");
                    objHorasHombre.NombreMes = objHorasHombre.FechaInicio.ToString("MMMM");

                    lstHorasHombre.Add(objHorasHombre);
                }


                #endregion

                #region SWITCH TABLA BODY
                obj.año = parametros.año;

                foreach (var s in parametros.comboCC)
                {
                    CentroDeCosto = s.cc;
                    totalPersonalOperativo = 0;
                    obj = new HorasHombreDTO();

                    foreach (var item in lstHorasHombre)
                    {
                        var lstIdControlAsistencias = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(r => r.cc == CentroDeCosto && r.fechaCapacitacion >= item.FechaInicio && r.fechaCapacitacion <= item.FechaFin).ToList().Select(y => y.id).ToList();
                        var lstIdCursos = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(r => r.cc == CentroDeCosto && r.fechaCapacitacion >= item.FechaInicio && r.fechaCapacitacion <= item.FechaFin).ToList().Select(y => y.cursoID).ToList();


                        var sumaCapacitados = _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(r => r.cc == CentroDeCosto && lstIdControlAsistencias.Contains(r.controlAsistenciaID)).ToList().Count();
                        var sumaDuracion = _context.tblS_CapacitacionSeguridadCursos.Where(r => lstIdCursos.Contains(r.id)).ToList().Sum(y => y.duracion);
                        var ObtenerHorasCapacitadas = (sumaCapacitados * sumaDuracion);

                        var lstAgrupaciones = _context.tblS_IncidentesAgrupacionCCDet.Where(r => r.cc == CentroDeCosto).ToList().Select(y => y.idAgrupacionCC).ToList();

                        var ObtenerHorasTrabajadas = _context.tblS_IncidentesInformacionColaboradoresClasificacion.Where(r => lstAgrupaciones.Contains((int)r.idAgrupacion) && r.estatus && r.fecha >= item.FechaInicio && r.fecha <= item.FechaFin).ToList()
                            .Sum(h => h.horasAdministrativo + h.horasMantenimiento + h.horasOperativo);

                        switch (item.NombreMes)
                        {
                            case "enero":
                                obj.lstEnero = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstEnero = objAnual;
                                totalPersonalOperativo = ObtenerTotal(s.cc, s.empresa);
                                totalGlobalPersonalOperativo += totalPersonalOperativo;
                                break;
                            case "febrero":
                                obj.lstFebrero = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstFebrero = objAnual;
                                break;
                            case "marzo":
                                obj.lstMarzo = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstMarzo = objAnual;
                                break;
                            case "abril":
                                obj.lstAbril = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstAbril = objAnual;
                                break;
                            case "mayo":
                                obj.lstMayo = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstMayo = objAnual;
                                break;
                            case "junio":
                                obj.lstJunio = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstJunio = objAnual;
                                break;
                            case "julio":
                                obj.lstJulio = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstJulio = objAnual;
                                break;
                            case "agosto":
                                obj.lstAgosto = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstAgosto = objAnual;
                                break;
                            case "septiembre":
                                obj.lstSeptiembre = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstSeptiembre = objAnual;
                                break;
                            case "octubre":
                                obj.lstOctubre = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstOctubre = objAnual;
                                break;
                            case "noviembre":
                                obj.lstNoviembre = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstNoviembre = objAnual;
                                break;
                            case "diciembre":
                                obj.lstDiciembre = new HorasHombreAnualDTO();
                                objAnual = new HorasHombreAnualDTO();
                                objAnual.descripcion = "";
                                objAnual.HrsCap = ObtenerHorasCapacitadas.ToString();
                                objAnual.HrsTrab = ObtenerHorasTrabajadas.ToString();
                                obj.lstDiciembre = objAnual;
                                break;
                        }

                    }
                    obj.lstGlobal = new HorasHombreAnualDTO();
                    obj.lstGlobal.descripcion = "";
                    obj.lstGlobal.HrsCap = (Convert.ToDecimal(obj.lstEnero.HrsCap) + Convert.ToDecimal(obj.lstFebrero.HrsCap) + Convert.ToDecimal(obj.lstMarzo.HrsCap) + Convert.ToDecimal(obj.lstAbril.HrsCap) + Convert.ToDecimal(obj.lstMayo.HrsCap) + Convert.ToDecimal(obj.lstJunio.HrsCap) + Convert.ToDecimal(obj.lstJulio.HrsCap) + Convert.ToDecimal(obj.lstAgosto.HrsCap) + Convert.ToDecimal(obj.lstSeptiembre.HrsCap) + Convert.ToDecimal(obj.lstOctubre.HrsCap) + Convert.ToDecimal(obj.lstNoviembre.HrsCap) + Convert.ToDecimal(obj.lstDiciembre.HrsCap)).ToString();
                    obj.lstGlobal.HrsTrab = (Convert.ToDecimal(obj.lstEnero.HrsTrab) + Convert.ToDecimal(obj.lstFebrero.HrsTrab) + Convert.ToDecimal(obj.lstMarzo.HrsTrab) + Convert.ToDecimal(obj.lstAbril.HrsTrab) + Convert.ToDecimal(obj.lstMayo.HrsTrab) + Convert.ToDecimal(obj.lstJunio.HrsTrab) + Convert.ToDecimal(obj.lstJulio.HrsTrab) + Convert.ToDecimal(obj.lstAgosto.HrsTrab) + Convert.ToDecimal(obj.lstSeptiembre.HrsTrab) + Convert.ToDecimal(obj.lstOctubre.HrsTrab) + Convert.ToDecimal(obj.lstNoviembre.HrsTrab) + Convert.ToDecimal(obj.lstDiciembre.HrsTrab)).ToString();

                    obj.descripcion = _context.tblP_CC.Where(r => r.cc == CentroDeCosto).Select(y => y.descripcion).FirstOrDefault();
                    obj.centrocosto = CentroDeCosto;
                    obj.totalPersonal = totalPersonalOperativo.ToString();
                    lstRetornar.Add(obj);
                }
                #endregion

                #region Agregar TABLA Footer

                obj = obtenerFooter(lstRetornar, totalGlobalPersonalOperativo.ToString());

                lstRetornar.Add(obj);
                #endregion


                return lstRetornar;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "postObtenerTablaHorasHombre", e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }


        public int ObtenerTotal(string cc, int cPlanOcArren)
        {
            List<string> ccsCplan = new List<string>();
            int numerototal = 0;
//            string queryBase =
//            @"
//                SELECT 
//                    e.clave_empleado AS claveEmpleado, 
//                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
//                    e.fecha_alta AS fechaAlta, 
//                    p.descripcion AS puestoEmpleado,
//                    e.puesto as puestoID,
//                    e.clave_depto as departamentoID,
//                    d.desc_depto as departamentoEmpleado,
//                    e.cc_contable AS ccID,
//                    (c.cc + ' - ' + c.descripcion) AS cc,
//                    e.curp
//                FROM DBA.sn_empleados AS e
//                INNER JOIN DBA.si_puestos AS p ON e.puesto = p.puesto
//                INNER JOIN DBA.sn_departamentos AS d ON e.clave_depto = d.clave_depto
//                INNER JOIN DBA.cc AS c ON e.cc_contable = c.cc
//                WHERE e.estatus_empleado ='A' AND e.cc_contable = '" + cc + "' ORDER BY c.cc, nombreEmpleado";

//            var odbc = new OdbcConsultaDTO();

//            if (cPlanOcArren == 1)
//            {
//                //CPLAN
//                odbc.consulta = String.Format(queryBase);
//                var departamentosCplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, odbc);
//                numerototal = departamentosCplan.Count();
//            }
//            else
//            {
//                odbc.consulta = String.Format(queryBase);
//                var departamentosCplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, odbc);
//                numerototal = departamentosCplan.Count();
//            }

            var departamentosCplan = _context.Select<DepartamentoDTO>(new DapperDTO 
            {
                baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                consulta = @"SELECT 
                    e.clave_empleado AS claveEmpleado, 
                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                    e.fecha_alta AS fechaAlta, 
                    p.descripcion AS puestoEmpleado,
                    e.puesto as puestoID,
                    e.clave_depto as departamentoID,
                    d.desc_depto as departamentoEmpleado,
                    e.cc_contable AS ccID,
                    (c.cc + ' - ' + c.descripcion) AS cc,
                    e.curp
                FROM tblRH_EK_Empleados AS e
                INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                WHERE e.estatus_empleado ='A' AND e.cc_contable = '" + cc + "' ORDER BY c.cc, nombreEmpleado",             
            });
            if (vSesiones.sesionEmpresaActual == 1) 
            {
                departamentosCplan.AddRange(_context.Select<DepartamentoDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT 
                    e.clave_empleado AS claveEmpleado, 
                    (e.nombre + ' ' + e.ape_paterno + ' ' + e.ape_materno) AS nombreEmpleado,
                    e.fecha_alta AS fechaAlta, 
                    p.descripcion AS puestoEmpleado,
                    e.puesto as puestoID,
                    e.clave_depto as departamentoID,
                    d.desc_depto as departamentoEmpleado,
                    e.cc_contable AS ccID,
                    (c.cc + ' - ' + c.descripcion) AS cc,
                    e.curp
                FROM tblRH_EK_Empleados AS e
                INNER JOIN tblRH_EK_Puestos AS p ON e.puesto = p.puesto
                INNER JOIN tblRH_EK_Departamentos AS d ON e.clave_depto = d.clave_depto
                INNER JOIN tblP_CC AS c ON e.cc_contable = c.cc
                WHERE e.estatus_empleado ='A' AND e.cc_contable = '" + cc + "' ORDER BY c.cc, nombreEmpleado",
                }));
                departamentosCplan = departamentosCplan.Distinct().ToList();
            }
            numerototal = departamentosCplan.Count();

            return numerototal;
        }

        public HorasHombreDTO obtenerFooter(List<HorasHombreDTO> lst, string totalGlobal)
        {
            HorasHombreDTO obj = new HorasHombreDTO();
            obj.centrocosto = "GLOBAL PERSONAL";
            obj.totalPersonal = totalGlobal;
            #region VARIABLES
            decimal totalGlobalaHrsCap = 0;
            decimal totalEneroHrsCap = 0;
            decimal totalFebreroHrsCap = 0;
            decimal totalMarzoHrsCap = 0;
            decimal totalAbrilHrsCap = 0;
            decimal totalMayoHrsCap = 0;
            decimal totalJunioHrsCap = 0;
            decimal totalJulioHrsCap = 0;
            decimal totalAgostoHrsCap = 0;
            decimal totalSeptiembreHrsCap = 0;
            decimal totalOctubreHrsCap = 0;
            decimal totalNoviembreHrsCap = 0;
            decimal totalDiciembreHrsCap = 0;
            decimal totalGlobalaHrsTrab = 0;
            decimal totalEneroHrsTrab = 0;
            decimal totalFebreroHrsTrab = 0;
            decimal totalMarzoHrsTrab = 0;
            decimal totalAbrilHrsTrab = 0;
            decimal totalMayoHrsTrab = 0;
            decimal totalJunioHrsTrab = 0;
            decimal totalJulioHrsTrab = 0;
            decimal totalAgostoHrsTrab = 0;
            decimal totalSeptiembreHrsTrab = 0;
            decimal totalOctubreHrsTrab = 0;
            decimal totalNoviembreHrsTrab = 0;
            decimal totalDiciembreHrsTrab = 0;
            #endregion

            int cont = 0;

            #region OBTENER TOTAL GLOBAL
            foreach (var item in lst)
            {
                cont++;
                if (cont > 1)
                {
                    totalGlobalaHrsCap += Convert.ToDecimal(item.lstGlobal.HrsCap);
                    totalGlobalaHrsTrab += Convert.ToDecimal(item.lstGlobal.HrsTrab);
                    totalEneroHrsCap += Convert.ToDecimal(item.lstEnero.HrsCap);
                    totalEneroHrsTrab += Convert.ToDecimal(item.lstEnero.HrsTrab);
                    totalFebreroHrsCap += Convert.ToDecimal(item.lstFebrero.HrsCap);
                    totalFebreroHrsTrab += Convert.ToDecimal(item.lstFebrero.HrsTrab);
                    totalMarzoHrsCap += Convert.ToDecimal(item.lstMarzo.HrsCap);
                    totalMarzoHrsTrab += Convert.ToDecimal(item.lstMarzo.HrsTrab);
                    totalAbrilHrsCap += Convert.ToDecimal(item.lstAbril.HrsCap);
                    totalAbrilHrsTrab += Convert.ToDecimal(item.lstAbril.HrsTrab);
                    totalMayoHrsCap += Convert.ToDecimal(item.lstMayo.HrsCap);
                    totalMayoHrsTrab += Convert.ToDecimal(item.lstMayo.HrsTrab);
                    totalJunioHrsCap += Convert.ToDecimal(item.lstJunio.HrsCap);
                    totalJunioHrsTrab += Convert.ToDecimal(item.lstJunio.HrsTrab);
                    totalJulioHrsCap += Convert.ToDecimal(item.lstJulio.HrsCap);
                    totalJulioHrsTrab += Convert.ToDecimal(item.lstJulio.HrsTrab);
                    totalAgostoHrsCap += Convert.ToDecimal(item.lstAgosto.HrsCap);
                    totalAgostoHrsTrab += Convert.ToDecimal(item.lstAgosto.HrsTrab);
                    totalSeptiembreHrsCap += Convert.ToDecimal(item.lstSeptiembre.HrsCap);
                    totalSeptiembreHrsTrab += Convert.ToDecimal(item.lstSeptiembre.HrsTrab);
                    totalOctubreHrsCap += Convert.ToDecimal(item.lstOctubre.HrsCap);
                    totalOctubreHrsTrab += Convert.ToDecimal(item.lstOctubre.HrsTrab);
                    totalNoviembreHrsCap += Convert.ToDecimal(item.lstNoviembre.HrsCap);
                    totalNoviembreHrsTrab += Convert.ToDecimal(item.lstNoviembre.HrsTrab);
                    totalDiciembreHrsCap += Convert.ToDecimal(item.lstDiciembre.HrsCap);
                    totalDiciembreHrsTrab += Convert.ToDecimal(item.lstDiciembre.HrsTrab);
                }
            }
            #endregion

            #region LLENAR OBJETO
            obj.lstGlobal = new HorasHombreAnualDTO();
            obj.lstGlobal.descripcion = "";
            obj.lstGlobal.HrsCap = totalGlobalaHrsCap.ToString();
            obj.lstGlobal.HrsTrab = totalGlobalaHrsTrab.ToString();

            obj.lstEnero = new HorasHombreAnualDTO();
            obj.lstEnero.descripcion = "";
            obj.lstEnero.HrsCap = totalEneroHrsCap.ToString();
            obj.lstEnero.HrsTrab = totalEneroHrsTrab.ToString();

            obj.lstFebrero = new HorasHombreAnualDTO();
            obj.lstFebrero.descripcion = "";
            obj.lstFebrero.HrsCap = totalFebreroHrsCap.ToString();
            obj.lstFebrero.HrsTrab = totalFebreroHrsTrab.ToString();

            obj.lstMarzo = new HorasHombreAnualDTO();
            obj.lstMarzo.descripcion = "";
            obj.lstMarzo.HrsCap = totalMarzoHrsCap.ToString();
            obj.lstMarzo.HrsTrab = totalMarzoHrsTrab.ToString();

            obj.lstAbril = new HorasHombreAnualDTO();
            obj.lstAbril.descripcion = "";
            obj.lstAbril.HrsCap = totalAbrilHrsCap.ToString();
            obj.lstAbril.HrsTrab = totalAbrilHrsTrab.ToString();

            obj.lstMayo = new HorasHombreAnualDTO();
            obj.lstMayo.descripcion = "";
            obj.lstMayo.HrsCap = totalMayoHrsCap.ToString();
            obj.lstMayo.HrsTrab = totalMayoHrsTrab.ToString();

            obj.lstJunio = new HorasHombreAnualDTO();
            obj.lstJunio.descripcion = "";
            obj.lstJunio.HrsCap = totalJunioHrsCap.ToString();
            obj.lstJunio.HrsTrab = totalJunioHrsTrab.ToString();

            obj.lstJulio = new HorasHombreAnualDTO();
            obj.lstJulio.descripcion = "";
            obj.lstJulio.HrsCap = totalJulioHrsCap.ToString();
            obj.lstJulio.HrsTrab = totalJulioHrsTrab.ToString();

            obj.lstAgosto = new HorasHombreAnualDTO();
            obj.lstAgosto.descripcion = "";
            obj.lstAgosto.HrsCap = totalAgostoHrsCap.ToString();
            obj.lstAgosto.HrsTrab = totalAgostoHrsTrab.ToString();

            obj.lstSeptiembre = new HorasHombreAnualDTO();
            obj.lstSeptiembre.descripcion = "";
            obj.lstSeptiembre.HrsCap = totalSeptiembreHrsCap.ToString();
            obj.lstSeptiembre.HrsTrab = totalSeptiembreHrsTrab.ToString();

            obj.lstOctubre = new HorasHombreAnualDTO();
            obj.lstOctubre.descripcion = "";
            obj.lstOctubre.HrsCap = totalOctubreHrsCap.ToString();
            obj.lstOctubre.HrsTrab = totalOctubreHrsTrab.ToString();

            obj.lstNoviembre = new HorasHombreAnualDTO();
            obj.lstNoviembre.descripcion = "";
            obj.lstNoviembre.HrsCap = totalNoviembreHrsCap.ToString();
            obj.lstNoviembre.HrsTrab = totalNoviembreHrsTrab.ToString();

            obj.lstDiciembre = new HorasHombreAnualDTO();
            obj.lstDiciembre.descripcion = "";
            obj.lstDiciembre.HrsCap = totalDiciembreHrsCap.ToString();
            obj.lstDiciembre.HrsTrab = totalDiciembreHrsTrab.ToString();
            #endregion

            return obj;
        }

        public HorasHombreDTO obtenerInputPromedios(HorasHombreDTO parametros)
        {
            List<HorasHombreDTO> lstRetornar = new List<HorasHombreDTO>();
            HorasHombreDTO objReturn = new HorasHombreDTO();
            try
            {
                lstRetornar = postObtenerTablaHorasHombre(parametros, true);

                foreach (var item in lstRetornar)
                {
                    if (item.centrocosto == "GLOBAL PERSONAL")
                    {
                        objReturn.promedioHRSCapacitaciones = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", (Convert.ToDecimal(item.lstGlobal.HrsCap) / 12));
                        objReturn.promedioHRSTrabajadas = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", (Convert.ToDecimal(item.lstGlobal.HrsTrab) / 12));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objReturn;
        }

        public MemoryStream crearExcelHorasHombreCapacitacion(HorasHombreDTO parametros)
        {
            try
            {
                HorasHombreDTO objReturn = new HorasHombreDTO();
                var HorasHombre = postObtenerTablaHorasHombre(parametros, true);
                foreach (var item in HorasHombre)
                {
                    if (item.centrocosto == "GLOBAL PERSONAL")
                    {
                        objReturn.promedioHRSCapacitaciones = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", (Convert.ToDecimal(item.lstGlobal.HrsCap) / 12));
                        objReturn.promedioHRSTrabajadas = String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", (Convert.ToDecimal(item.lstGlobal.HrsTrab) / 12));
                    }
                }
                Color colorDeCelda = ColorTranslator.FromHtml("#fff");

                using (ExcelPackage excel = new ExcelPackage())
                {
                    #region CONTENIDO

                    var hoja1 = excel.Workbook.Worksheets.Add("HorasHombreCapacitacion");

                    #region HEADER DE EL EXCEL

                    List<string[]> headerRow = new List<string[]>() { new string[] { 
                        "CAPACITACION CONSTRUPLAN " + DateTime.Now.Year,
                    } };
                    //string headerRange = "A1:" + Char.ConvertFromUtf32(headerRow[0].Length + 64) + "1";

                    string TituloRango = "B2:" + "F3";
                    hoja1.Cells[TituloRango].Merge = true;
                    hoja1.Cells[TituloRango].LoadFromArrays(headerRow);
                    hoja1.Cells[TituloRango].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[TituloRango].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[TituloRango].Style.Font.Bold = true;
                    hoja1.Cells[TituloRango].Style.Fill.BackgroundColor.SetColor(1, 236, 124, 48);
                    hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    string CeldasPromedioHRSCAP = "E5:G6";
                    string CeldasPromedioHRSCAPTotal = "H5:I6";
                    List<string[]> headerRowPromedioCAP = new List<string[]>() { new string[] { 
                        "Promedio Hrs Capacitacion mensual",
                    } };
                    hoja1.Cells[CeldasPromedioHRSCAP].Merge = true;
                    hoja1.Cells[CeldasPromedioHRSCAP].LoadFromArrays(headerRowPromedioCAP);
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.Font.Bold = true;
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.WrapText = true;
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSCAP].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> headerRowPromedioCAPTotal = new List<string[]>() { new string[] { 
                        objReturn.promedioHRSCapacitaciones,
                    } };
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Merge = true;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].LoadFromArrays(headerRowPromedioCAPTotal);
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.Font.Bold = true;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.WrapText = true;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSCAPTotal].Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);

                    string CeldasPromedioHRSTRAB = "K5:M6";
                    string CeldasPromedioHRSTRABTotal = "N5:O6";
                    List<string[]> headerRowPromedioTRAB = new List<string[]>() { new string[] { 
                        "Promedio Hrs Capacitacion mensual / Hrs Trabajadas",
                    } };
                    hoja1.Cells[CeldasPromedioHRSTRAB].Merge = true;
                    hoja1.Cells[CeldasPromedioHRSTRAB].LoadFromArrays(headerRowPromedioTRAB);
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.Font.Bold = true;
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.WrapText = true;
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSTRAB].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    List<string[]> headerRowPromedioTRABTotal = new List<string[]>() { new string[] { 
                       objReturn.promedioHRSTrabajadas,
                    } };
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Merge = true;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].LoadFromArrays(headerRowPromedioTRABTotal);
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.Font.Bold = true;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.WrapText = true;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    hoja1.Cells[CeldasPromedioHRSTRABTotal].Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);

                    #endregion
                    #region MESES
                    List<string[]> TituloMesGlobal = new List<string[]>() { new string[] { 
                        "",
                        "",
                        "",
                        "GLOBAL",
                        } };
                    string tituloMesRangeGlobalPrin = "B9:F9";
                    string tituloMesRangeGlobal = "E9:F9";
                    hoja1.Cells[tituloMesRangeGlobalPrin].LoadFromArrays(TituloMesGlobal);
                    hoja1.Cells[tituloMesRangeGlobalPrin].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeGlobalPrin].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeGlobal].Merge = true;
                    hoja1.Cells[tituloMesRangeGlobal].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeGlobal].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeGlobal].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeGlobal].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeGlobal].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeGlobal].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesEnero = new List<string[]>() { new string[] { 
                        "ENERO",
                        } };
                    string tituloMesRangeEnero = "G9:H9";
                    hoja1.Cells[tituloMesRangeEnero].Merge = true;
                    hoja1.Cells[tituloMesRangeEnero].LoadFromArrays(TituloMesEnero);
                    hoja1.Cells[tituloMesRangeEnero].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeEnero].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeEnero].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeEnero].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeEnero].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeEnero].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeEnero].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesFebrero = new List<string[]>() { new string[] { 
                        "FEBRERO",
                        } };
                    string tituloMesRangeFebrero = "I9:J9";
                    hoja1.Cells[tituloMesRangeFebrero].Merge = true;
                    hoja1.Cells[tituloMesRangeFebrero].LoadFromArrays(TituloMesFebrero);
                    hoja1.Cells[tituloMesRangeFebrero].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeFebrero].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeFebrero].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeFebrero].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeFebrero].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeFebrero].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeFebrero].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesMarzo = new List<string[]>() { new string[] { 
                        "MARZO",
                        } };
                    string tituloMesRangeMarzo = "K9:L9";
                    hoja1.Cells[tituloMesRangeMarzo].Merge = true;
                    hoja1.Cells[tituloMesRangeMarzo].LoadFromArrays(TituloMesMarzo);
                    hoja1.Cells[tituloMesRangeMarzo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeMarzo].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeMarzo].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeMarzo].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeMarzo].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeMarzo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeMarzo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesAbril = new List<string[]>() { new string[] { 
                        "ABRIL",
                        } };
                    string tituloMesRangeAbril = "M9:N9";
                    hoja1.Cells[tituloMesRangeAbril].Merge = true;
                    hoja1.Cells[tituloMesRangeAbril].LoadFromArrays(TituloMesAbril);
                    hoja1.Cells[tituloMesRangeAbril].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeAbril].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeAbril].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeAbril].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeAbril].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeAbril].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeAbril].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesMayo = new List<string[]>() { new string[] { 
                        "MAYO",
                        } };
                    string tituloMesRangeMayo = "O9:P9";
                    hoja1.Cells[tituloMesRangeMayo].Merge = true;
                    hoja1.Cells[tituloMesRangeMayo].LoadFromArrays(TituloMesMayo);
                    hoja1.Cells[tituloMesRangeMayo].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeMayo].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeMayo].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeMayo].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeMayo].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeMayo].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeMayo].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesJunio = new List<string[]>() { new string[] { 
                        "JUNIO",
                        } };
                    string tituloMesRangeJunio = "Q9:R9";
                    hoja1.Cells[tituloMesRangeJunio].Merge = true;
                    hoja1.Cells[tituloMesRangeJunio].LoadFromArrays(TituloMesJunio);
                    hoja1.Cells[tituloMesRangeJunio].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeJunio].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeJunio].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeJunio].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeJunio].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeJunio].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeJunio].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesJulio = new List<string[]>() { new string[] { 
                        "JULIO",
                        } };
                    string tituloMesRangeJulio = "S9:T9";
                    hoja1.Cells[tituloMesRangeJulio].Merge = true;
                    hoja1.Cells[tituloMesRangeJulio].LoadFromArrays(TituloMesJulio);
                    hoja1.Cells[tituloMesRangeJulio].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeJulio].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeJulio].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeJulio].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeJulio].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeJulio].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeJulio].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesAgosto = new List<string[]>() { new string[] { 
                        "AGOSTO",
                        } };
                    string tituloMesRangeAgosto = "U9:V9";
                    hoja1.Cells[tituloMesRangeAgosto].Merge = true;
                    hoja1.Cells[tituloMesRangeAgosto].LoadFromArrays(TituloMesAgosto);
                    hoja1.Cells[tituloMesRangeAgosto].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeAgosto].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeAgosto].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeAgosto].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeAgosto].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeAgosto].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeAgosto].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesSeptiembre = new List<string[]>() { new string[] { 
                        "SEPTIEMBRE",
                        } };
                    string tituloMesRangeSeptiembre = "W9:X9";
                    hoja1.Cells[tituloMesRangeSeptiembre].Merge = true;
                    hoja1.Cells[tituloMesRangeSeptiembre].LoadFromArrays(TituloMesSeptiembre);
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeSeptiembre].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesOctubre = new List<string[]>() { new string[] { 
                        "OCTUBRE",
                        } };
                    string tituloMesRangeOctubre = "Y9:Z9";
                    hoja1.Cells[tituloMesRangeOctubre].Merge = true;
                    hoja1.Cells[tituloMesRangeOctubre].LoadFromArrays(TituloMesOctubre);
                    hoja1.Cells[tituloMesRangeOctubre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeOctubre].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeOctubre].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeOctubre].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeOctubre].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeOctubre].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeOctubre].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesNoviembre = new List<string[]>() { new string[] { 
                        "NOVIEMBRE",
                        } };
                    string tituloMesRangeNoviembre = "AA9:AB9";
                    hoja1.Cells[tituloMesRangeNoviembre].Merge = true;
                    hoja1.Cells[tituloMesRangeNoviembre].LoadFromArrays(TituloMesNoviembre);
                    hoja1.Cells[tituloMesRangeNoviembre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeNoviembre].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeNoviembre].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeNoviembre].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeNoviembre].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeNoviembre].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeNoviembre].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    List<string[]> TituloMesDiciembre = new List<string[]>() { new string[] { 
                        "DICIEMBRE",
                        } };
                    string tituloMesRangeDiciembre = "AC9:AD9";
                    hoja1.Cells[tituloMesRangeDiciembre].Merge = true;
                    hoja1.Cells[tituloMesRangeDiciembre].LoadFromArrays(TituloMesDiciembre);
                    hoja1.Cells[tituloMesRangeDiciembre].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[tituloMesRangeDiciembre].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[tituloMesRangeDiciembre].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[tituloMesRangeDiciembre].Style.Font.Bold = true;
                    hoja1.Cells[tituloMesRangeDiciembre].Style.WrapText = true;
                    hoja1.Cells[tituloMesRangeDiciembre].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[tituloMesRangeDiciembre].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    #endregion

                    string headerRange = "B10:" + "AD10";
                    hoja1.Cells[headerRange].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    hoja1.Cells[headerRange].Style.Font.Color.SetColor(colorDeCelda);
                    hoja1.Cells[headerRange].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[headerRange].Style.Fill.BackgroundColor.SetColor(1, 64, 64, 64);
                    hoja1.Cells[headerRange].Style.Font.Bold = true;
                    hoja1.Cells[headerRange].Style.WrapText = true;
                    hoja1.Column(3).Width = 40;
                    hoja1.Column(4).Width = 40;
                    for (int i = 5; i <= 30; i++)
                    {
                        hoja1.Column(i).Width = 15;
                    }
                    //hoja1.Cells[headerRange].Merge = true;

                    var cellData = new List<object[]>();

                    int contadorLista = 0;
                    foreach (var ins in HorasHombre)
                    {

                        cellData.Add(new object[]{
                            ins.centrocosto.Trim(),
                            ins.descripcion == null ? "":ins.descripcion.Trim(),
                            ins.totalPersonal,
                            contadorLista == 0 ? ins.lstGlobal.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstGlobal.HrsCap)),
                            contadorLista == 0 ? ins.lstGlobal.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstGlobal.HrsTrab)),
                            contadorLista == 0 ? ins.lstEnero.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstEnero.HrsCap)),
                            contadorLista == 0 ? ins.lstEnero.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstEnero.HrsTrab)),
                            contadorLista == 0 ? ins.lstFebrero.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstFebrero.HrsCap)),
                            contadorLista == 0 ? ins.lstFebrero.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstFebrero.HrsTrab)),
                            contadorLista == 0 ? ins.lstMarzo.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstMarzo.HrsCap)),
                            contadorLista == 0 ? ins.lstMarzo.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstMarzo.HrsTrab)),
                            contadorLista == 0 ? ins.lstAbril.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstAbril.HrsCap)),
                            contadorLista == 0 ? ins.lstAbril.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstAbril.HrsTrab)),
                            contadorLista == 0 ? ins.lstMayo.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstMayo.HrsCap)),
                            contadorLista == 0 ? ins.lstMayo.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstMayo.HrsTrab)),
                            contadorLista == 0 ? ins.lstJunio.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstJunio.HrsCap)),
                            contadorLista == 0 ? ins.lstJunio.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstJunio.HrsTrab)),
                            contadorLista == 0 ? ins.lstJulio.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstJulio.HrsCap)),
                            contadorLista == 0 ? ins.lstJulio.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstJulio.HrsTrab)),
                            contadorLista == 0 ? ins.lstAgosto.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstAgosto.HrsCap)),
                            contadorLista == 0 ? ins.lstAgosto.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstAgosto.HrsTrab)),
                            contadorLista == 0 ? ins.lstSeptiembre.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstSeptiembre.HrsCap)),
                            contadorLista == 0 ? ins.lstSeptiembre.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstSeptiembre.HrsTrab)),
                            contadorLista == 0 ? ins.lstOctubre.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstOctubre.HrsCap)),
                            contadorLista == 0 ? ins.lstOctubre.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstOctubre.HrsTrab)),
                            contadorLista == 0 ? ins.lstNoviembre.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstNoviembre.HrsCap)),
                            contadorLista == 0 ? ins.lstNoviembre.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstNoviembre.HrsTrab)),
                            contadorLista == 0 ? ins.lstDiciembre.HrsCap : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstDiciembre.HrsCap)),
                            contadorLista == 0 ? ins.lstDiciembre.HrsTrab : String.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", Convert.ToDecimal(ins.lstDiciembre.HrsTrab)),
                            });
                        contadorLista++;
                    }

                    hoja1.Cells[10, 2].LoadFromArrays(cellData);
                    int cont = 10;
                    foreach (var item in HorasHombre)
                    {
                        cont++;
                        string headAling = "D" + cont + ":" + "AD" + cont;
                        //hoja1.Cells[headAling].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        hoja1.Cells[headAling].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        hoja1.Cells[headAling].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //hoja1.Cells[headAling].Style.Fill.BackgroundColor.SetColor(1, 255, 255, 255);
                    }

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

                    List<byte[]> lista = new List<byte[]>();
                    #endregion

                    var bytes = new MemoryStream();

                    using (var exportData = new MemoryStream())
                    {
                        excel.SaveAs(exportData);
                        bytes = exportData;
                    }

                    return bytes;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        #endregion

        #region CAMBIOS DE CATEGORIA
        public List<tblRH_AutorizacionFormatoCambio> getAutorizacion(int idFormato)
        {
            try
            {
                var resultado = new List<tblRH_AutorizacionFormatoCambio>();

                using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                {
                    resultado.AddRange(_contextConstruplan.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == idFormato).OrderBy(x => x.Orden).ToList());
                }

                using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                {
                    resultado.AddRange(_contextArrendadora.tblRH_AutorizacionFormatoCambio.Where(x => x.Id_FormatoCambio == idFormato).OrderBy(x => x.Orden).ToList());
                }

                return resultado;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "getAutorizacion", e, AccionEnum.CONSULTA, 0, idFormato);

                return new List<tblRH_AutorizacionFormatoCambio>();
            }
        }

        public List<resultCapacitacionDTO> TablaFormatosPendientes(FiltrosCapacitacionDTO parametros)
        {
            try
            {
                var result = new List<resultCapacitacionDTO>();
                var ud = new UsuarioDAO();
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var exclusion = _context.tblRH_FormatoCambioExclusion.Select(x => x.empleadoCVE).ToList();
                var usuarioFactoryServices = new UsuarioFactoryServices();

                if (parametros.id != 0 && parametros.CC.Equals("") && parametros.claveEmpleado == 0 && parametros.estado == 1 && parametros.tipo.Equals("") && parametros.numero == 0)
                {
                    result = _context.tblRH_FormatoCambio.Where(x => x.id == parametros.id).ToList().Select(y => new resultCapacitacionDTO
                    {
                        id = y.id,
                        usuarioCap = y.Clave_Empleado,
                        folio = y.folio,
                        InicioNomina = y.InicioNomina,
                        FechaInicioCambio = y.FechaInicioCambio,
                        Justificacion = y.Justificacion,
                        CamposCambiados = y.CamposCambiados,
                        Aprobado = y.Aprobado,
                        Rechazado = y.Rechazado,
                        nomUsuarioCap = y.Nombre + " " + y.Ape_Paterno + " " + y.Ape_Materno,
                        editable = y.editable,
                        Bono = y.Bono,
                        fechaCaptura = y.fechaCaptura,
                        SalarioAnt = y.SalarioAnt,
                        ComplementoAnt = y.ComplementoAnt,
                        BonoAnt = y.BonoAnt,
                        CCAntID = y.CCAntID,
                        CCAnt = y.CCAnt,
                        PuestoAnt = y.PuestoAnt,
                        RegistroPatronalAnt = y.RegistroPatronalAnt,
                        Nombre_Jefe_InmediatoAnt = y.Nombre_Jefe_InmediatoAnt,
                        TipoNominaAnt = y.TipoNominaAnt,
                        btnSubirArchivo = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(r => r.idFormatoCambio == y.id).ToList() != null ? false : true
                    }).ToList();
                }
                else if (rh)
                {
                    result = _context.tblRH_FormatoCambio.Where(x =>
                        (parametros.id == 0 ? true : x.id == parametros.id) &&
                        (x.CcID.Equals("") ? true : parametros.CC.Contains(x.CcID)) &&
                        (parametros.claveEmpleado != 0 ? x.Clave_Empleado == parametros.claveEmpleado : true) &&
                        (parametros.estado == 2 ? (!x.Aprobado && !x.Rechazado) : (parametros.estado == 3 ? x.Aprobado : (parametros.estado == 4 ? x.Rechazado : (true)))) &&
                        (parametros.tipo.Equals("") ? true : (x.TipoNomina.Equals(parametros.tipo) && x.InicioNomina == parametros.numero))).ToList().Select(y => new resultCapacitacionDTO
                    {
                        id = y.id,
                        usuarioCap = y.Clave_Empleado,
                        folio = y.folio,
                        InicioNomina = y.InicioNomina,
                        FechaInicioCambio = y.FechaInicioCambio,
                        Justificacion = y.Justificacion,
                        CamposCambiados = y.CamposCambiados,
                        Aprobado = y.Aprobado,
                        Rechazado = y.Rechazado,
                        nomUsuarioCap = y.Nombre + " " + y.Ape_Paterno + " " + y.Ape_Materno,
                        editable = y.editable,
                        Bono = y.Bono,
                        fechaCaptura = y.fechaCaptura,
                        SalarioAnt = y.SalarioAnt,
                        ComplementoAnt = y.ComplementoAnt,
                        BonoAnt = y.BonoAnt,
                        CCAntID = y.CCAntID,
                        CCAnt = y.CCAnt,
                        PuestoAnt = y.PuestoAnt,
                        RegistroPatronalAnt = y.RegistroPatronalAnt,
                        Nombre_Jefe_InmediatoAnt = y.Nombre_Jefe_InmediatoAnt,
                        TipoNominaAnt = y.TipoNominaAnt,
                        btnSubirArchivo = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(r => r.idFormatoCambio == y.id).ToList() != null ? false : true
                    }).ToList();
                }
                else
                {
                    using (var _contextConstruplan = new MainContext(EmpresaEnum.Construplan))
                    {
                        var autorizante = _contextConstruplan.tblRH_AutorizacionFormatoCambio.Where(x => x.Clave_Aprobador == 3367 && x.Autorizando).Select(x => x.Id_FormatoCambio).ToList();
                        var formatosCambio = _contextConstruplan.tblRH_FormatoCambio.Where(x =>
                            (parametros.id == 0 ? true : x.id == parametros.id) &&
                            (parametros.CC.Equals("") ? true : parametros.CC.Contains(x.CcID)) &&
                            (parametros.claveEmpleado != 0 ? x.Clave_Empleado == parametros.claveEmpleado : true) &&
                            (parametros.estado == 2 ? (!x.Aprobado && !x.Rechazado) : (parametros.estado == 3 ? x.Aprobado : (parametros.estado == 4 ? x.Rechazado : (true)))) &&
                            (parametros.tipo.Equals("") ? true : (x.TipoNomina.Equals(parametros.tipo) && x.InicioNomina == parametros.numero)) &&
                                //(x.Clave_Empleado == vSesiones.sesionUsuarioDTO.id) &&
                            (autorizante.Contains(x.id))
                        ).ToList();

                        result.AddRange(formatosCambio.Select(y => new resultCapacitacionDTO
                        {
                            id = y.id,
                            usuarioCap = y.Clave_Empleado,
                            folio = y.folio,
                            InicioNomina = y.InicioNomina,
                            FechaInicioCambio = y.FechaInicioCambio,
                            Justificacion = y.Justificacion,
                            CamposCambiados = y.CamposCambiados,
                            Aprobado = y.Aprobado,
                            Rechazado = y.Rechazado,
                            nomUsuarioCap = y.Nombre + " " + y.Ape_Paterno + " " + y.Ape_Materno,
                            editable = y.editable,
                            Bono = y.Bono,
                            fechaCaptura = y.fechaCaptura,
                            SalarioAnt = y.SalarioAnt,
                            ComplementoAnt = y.ComplementoAnt,
                            BonoAnt = y.BonoAnt,
                            CCAntID = y.CCAntID,
                            CCAnt = y.CCAnt,
                            PuestoAnt = y.PuestoAnt,
                            RegistroPatronalAnt = y.RegistroPatronalAnt,
                            Nombre_Jefe_InmediatoAnt = y.Nombre_Jefe_InmediatoAnt,
                            TipoNominaAnt = y.TipoNominaAnt,
                            btnSubirArchivo = RetornarBoton(y.id),
                            empresa = EmpresaEnum.Construplan
                        }).ToList());
                    }

                    using (var _contextArrendadora = new MainContext(EmpresaEnum.Arrendadora))
                    {
                        var autorizante = _contextArrendadora.tblRH_AutorizacionFormatoCambio.Where(x => x.Clave_Aprobador == 3367 && x.Autorizando).Select(x => x.Id_FormatoCambio).ToList();
                        var formatosCambio = _contextArrendadora.tblRH_FormatoCambio.Where(x =>
                            (parametros.id == 0 ? true : x.id == parametros.id) &&
                            (parametros.CC.Equals("") ? true : parametros.CC.Contains(x.CcID)) &&
                            (parametros.claveEmpleado != 0 ? x.Clave_Empleado == parametros.claveEmpleado : true) &&
                            (parametros.estado == 2 ? (!x.Aprobado && !x.Rechazado) : (parametros.estado == 3 ? x.Aprobado : (parametros.estado == 4 ? x.Rechazado : (true)))) &&
                            (parametros.tipo.Equals("") ? true : (x.TipoNomina.Equals(parametros.tipo) && x.InicioNomina == parametros.numero)) &&
                                //(x.Clave_Empleado == vSesiones.sesionUsuarioDTO.id) &&
                            (autorizante.Contains(x.id))
                        ).ToList();

                        result.AddRange(formatosCambio.Select(y => new resultCapacitacionDTO
                        {
                            id = y.id,
                            usuarioCap = y.Clave_Empleado,
                            folio = y.folio,
                            InicioNomina = y.InicioNomina,
                            FechaInicioCambio = y.FechaInicioCambio,
                            Justificacion = y.Justificacion,
                            CamposCambiados = y.CamposCambiados,
                            Aprobado = y.Aprobado,
                            Rechazado = y.Rechazado,
                            nomUsuarioCap = y.Nombre + " " + y.Ape_Paterno + " " + y.Ape_Materno,
                            editable = y.editable,
                            Bono = y.Bono,
                            fechaCaptura = y.fechaCaptura,
                            SalarioAnt = y.SalarioAnt,
                            ComplementoAnt = y.ComplementoAnt,
                            BonoAnt = y.BonoAnt,
                            CCAntID = y.CCAntID,
                            CCAnt = y.CCAnt,
                            PuestoAnt = y.PuestoAnt,
                            RegistroPatronalAnt = y.RegistroPatronalAnt,
                            Nombre_Jefe_InmediatoAnt = y.Nombre_Jefe_InmediatoAnt,
                            TipoNominaAnt = y.TipoNominaAnt,
                            btnSubirArchivo = RetornarBoton(y.id),
                            empresa = EmpresaEnum.Arrendadora
                        }).ToList());
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                LogError(0, 0, NombreControlador, "TablaFormatosPendientes", e, AccionEnum.CONSULTA, 0, parametros);

                return null;
            }

        }

        public bool RetornarBoton(int id)
        {
            bool variableRetorno = false;
            var lst = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(r => r.idFormatoCambio == id).ToList();
            if (lst.Count != 0)
            {
                variableRetorno = true;
            }

            return variableRetorno;
        }

        public Dictionary<string, object> postSubirArchivos(int id, EmpresaEnum empresa, List<HttpPostedFileBase> archivo)
        {
            MainContext _contextEmpresa = null;

            switch (empresa)
            {
                case EmpresaEnum.Construplan:
                    _contextEmpresa = new MainContext(EmpresaEnum.Construplan);
                    break;
                case EmpresaEnum.Arrendadora:
                    _contextEmpresa = new MainContext(EmpresaEnum.Arrendadora);
                    break;
            }

            using (var dbContextTransaction = _contextEmpresa.Database.BeginTransaction())
            {
                try
                {
                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    var obtenerClaveEmpleado = _contextEmpresa.tblRH_FormatoCambio.Where(r => r.id == id).FirstOrDefault();
                    var CarpetaNueva = Path.Combine(RutaCompetenciasOperativasGestion, obtenerClaveEmpleado.Clave_Empleado.ToString());

                    verificarExisteCarpeta(CarpetaNueva, true);

                    int index = 0;

                    foreach (var arch in archivo)
                    {
                        string nombreArchivo = ObtenerFormatoNombreArchivoA("CO_", arch.FileName);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        listaRutaArchivos.Add(Tuple.Create(arch, rutaArchivo));

                        // GUARDAR TABLA ARCHIVOS
                        var obj = new tblS_CapacitacionSeguridadCO_GCArchivos()
                        {
                            idFormatoCambio = id,
                            rutaArchivo = rutaArchivo
                        };

                        _contextEmpresa.tblS_CapacitacionSeguridadCO_GCArchivos.Add(obj);
                        _contextEmpresa.SaveChanges();

                        index++;
                    }

                    foreach (var arch in listaRutaArchivos)
                    {
                        var guardarArchivo = GlobalUtils.SaveHTTPPostedFileValidacion(arch.Item1, arch.Item2);

                        if (guardarArchivo.Item1 == false)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Clear();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                            LogError(0, 0, NombreControlador, "guardarNuevoRecorrido", guardarArchivo.Item2, AccionEnum.AGREGAR, 0, new { id = id, empresa = empresa });
                            return resultado;
                        }
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, NombreControlador, "guardarNuevoRecorrido", e, AccionEnum.AGREGAR, 0, new { id = id, empresa = empresa });
                }
            }

            return resultado;
        }
        private string ObtenerFormatoNombreArchivoA(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }

        public byte[] descargarArchivoCO(int id)
        {
            var resultado = new Dictionary<string, object>();
            Stream fileStream;
            try
            {
                string pathExamen = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(x => x.id == id).FirstOrDefault().rutaArchivo;
                fileStream = GlobalUtils.GetFileAsStream(pathExamen);

            }
            catch (Exception e)
            {
                fileStream = null;
            }

            //resultado.Add("nombreDescarga", version.nombre);
            //resultado.Add(SUCCESS, true);
            return ReadFully(fileStream);
        }
        public string getFileNameCO(int id)
        {
            string fileName = "";
            try
            {
                string pathExamen = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(x => x.id == id).FirstOrDefault().rutaArchivo;
                fileName = pathExamen.Split('\\')[8];
            }
            catch (Exception e)
            {
                fileName = "";
            }

            return fileName;
        }

        public Dictionary<string, object> obtenerArchivoCODescargas(int idFormatoCambio)
        {
            try
            {
                var result = _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(r => r.idFormatoCambio == idFormatoCambio).ToList();

                resultado.Add("data", result);
            }
            catch (Exception o_O)
            {
                resultado.Add("data", "ocurrio algun error");
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }

        public List<tblS_CapacitacionSeguridadCO_GCArchivos> getArchivosFormatoCambio(int formatoCambioID)
        {
            return _context.tblS_CapacitacionSeguridadCO_GCArchivos.Where(x => x.idFormatoCambio == formatoCambioID).ToList();
        }
        #endregion

        #region ADMINISTRACIONINSTRUCTORES
        public Dictionary<string, object> GetInstructores()
        {
            var listaRoles = _context.tblS_CapacitacionSeguridadRolesGrupoTrabajo.Where(x => x.esActivo).ToList();

            var result = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(r => r.esActivo == true).ToList().Select(y => new AdministracionInstructoresDTO
            {
                id = y.id,
                grupo = y.grupo,
                cveEmpleado = y.cveEmpleado,
                nombreCompleto = devolverstring(y.cveEmpleado).nombreCompleto,
                ApeidoP = devolverstring(y.cveEmpleado).ApeidoP,
                ApeidoM = devolverstring(y.cveEmpleado).ApeidoM,
                nombreGrupo = listaRoles.Where(r => r.id == y.grupo).Select(n => n.descripcion).FirstOrDefault(),
                fechaInicio = y.fechaInicio,
                tematica = (TematicaCursoEnum)y.tematica,
                instructor = y.instructor,

            }).ToList();

            resultado.Add("data", result);
            return resultado;
        }

        public AdministracionInstructoresDTO devolverstring(string cveEmpleado)
        {
            AdministracionInstructoresDTO retorna = new AdministracionInstructoresDTO();
//            string query = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion
//                                FROM sn_empleados  
//                                INNER JOIN cc  on cc_contable=cc 
//                                WHERE clave_empleado=" + cveEmpleado;

//            var odbc = new OdbcConsultaDTO() { consulta = query };
//            var listaConstruplan = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.CplanRh, odbc).ToList().Select(y => new AdministracionInstructoresDTO
//            {
//                cc = y.cc,
//                descripcion = y.descripcion,
//                clave_empleado = y.clave_empleado,
//                nombreCompleto = y.nombreCompleto,
//                ApeidoM = y.ApeidoM,
//                ApeidoP = y.ApeidoP,
//            }).FirstOrDefault();
//            var listaArrendadora = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.ArrenRh, odbc).ToList().Select(y => new AdministracionInstructoresDTO
//            {
//                cc = y.cc,
//                descripcion = y.descripcion,
//                clave_empleado = y.clave_empleado,
//                nombreCompleto = y.nombreCompleto,
//                ApeidoM = y.ApeidoM,
//                ApeidoP = y.ApeidoP,
//            }).FirstOrDefault();

            var listaConstruplan = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc
                                FROM tblRH_EK_Empleados  
                                WHERE clave_empleado=" + cveEmpleado
            }).ToList().Select(y => new AdministracionInstructoresDTO
            {
                cc = y.cc,
                descripcion = "",
                clave_empleado = y.clave_empleado,
                nombreCompleto = y.nombreCompleto,
                ApeidoM = y.ApeidoM,
                ApeidoP = y.ApeidoP,
            }).FirstOrDefault();

            if (vSesiones.sesionEmpresaActual == 1 && listaConstruplan == null) 
            {
                listaConstruplan = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.GCPLAN,
                    consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc
                                FROM tblRH_EK_Empleados  
                                WHERE clave_empleado=" + cveEmpleado
                }).ToList().Select(y => new AdministracionInstructoresDTO
                {
                    cc = y.cc,
                    descripcion = "",
                    clave_empleado = y.clave_empleado,
                    nombreCompleto = y.nombreCompleto,
                    ApeidoM = y.ApeidoM,
                    ApeidoP = y.ApeidoP,
                }).FirstOrDefault();
            }

            if (listaConstruplan != null)
            {
                var query_cc = new OdbcConsultaDTO();
                query_cc.consulta =
                    @"SELECT TOP 1
                        *
                    FROM
                        cc
                    WHERE
                        cc = ?";
                query_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cc",
                    tipo = OdbcType.Char,
                    valor = listaConstruplan.cc
                });
                var ccEmpleado = _contextEnkontrol.Select<ccDTO>(EnkontrolAmbienteEnum.ProdCPLAN, query_cc).FirstOrDefault();

                if (ccEmpleado != null)
                {
                    listaConstruplan.descripcion = ccEmpleado.descripcion;
                }
            }

            var listaArrendadora = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc
                                FROM sn_empleados
                                WHERE clave_empleado=" + cveEmpleado,
            }).ToList().Select(y => new AdministracionInstructoresDTO
            {
                cc = y.cc,
                descripcion = "",
                clave_empleado = y.clave_empleado,
                nombreCompleto = y.nombreCompleto,
                ApeidoM = y.ApeidoM,
                ApeidoP = y.ApeidoP,
            }).FirstOrDefault();

            if (listaArrendadora != null)
            {
                var query_cc = new OdbcConsultaDTO();
                query_cc.consulta =
                    @"SELECT TOP 1
                        *
                    FROM
                        cc
                    WHERE
                        cc = ?";
                query_cc.parametros.Add(new OdbcParameterDTO
                {
                    nombre = "cc",
                    tipo = OdbcType.Char,
                    valor = listaArrendadora.cc
                });
                var ccEmpleado = _contextEnkontrol.Select<ccDTO>(EnkontrolAmbienteEnum.ProdARREND, query_cc).FirstOrDefault();
                if (ccEmpleado != null)
                {
                    listaArrendadora.descripcion = ccEmpleado.descripcion;
                }
            }

            if (listaConstruplan != null)
            {
                retorna = listaConstruplan;
            }
            else
            {
                retorna = listaArrendadora;
            }

            return retorna;
        }

        public List<ComboDTO> GetInstructoresCombo(string cc)
        {

//            string query = @"SELECT clave_empleado as cveEmpleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion ,estatus_empleado
//                                FROM sn_empleados  
//                                INNER JOIN cc  on cc_contable=cc 
//                                WHERE cc='" + cc + "' and estatus_empleado='A'";

//            var odbc = new OdbcConsultaDTO() { consulta = query };
//            var listaConstruplan = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.CplanRh, odbc).ToList().Select(y => new ComboDTO
//            {
//                Value = y.cveEmpleado,
//                Text = y.nombreCompleto + " " + y.ApeidoP + " " + y.ApeidoM
//            }).ToList();
//            var listaArrendadora = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.ArrenRh, odbc).ToList().Select(y => new ComboDTO
//            {
//                Value = y.cveEmpleado,
//                Text = y.nombreCompleto + " " + y.ApeidoP + " " + y.ApeidoM
//            }).ToList();

            var listaConstruplan = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT clave_empleado as cveEmpleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion ,estatus_empleado
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE cc='" + cc + "' and estatus_empleado='A'",
            }).ToList().Select(y => new ComboDTO
            {
                Value = y.cveEmpleado,
                Text = y.nombreCompleto + " " + y.ApeidoP + " " + y.ApeidoM
            }).ToList();

            listaConstruplan.AddRange(_context.Select<AdministracionInstructoresDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.GCPLAN,
                consulta = @"SELECT clave_empleado as cveEmpleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion ,estatus_empleado
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE cc='" + cc + "' and estatus_empleado='A'",
            }).ToList().Select(y => new ComboDTO
            {
                Value = y.cveEmpleado,
                Text = y.nombreCompleto + " " + y.ApeidoP + " " + y.ApeidoM
            }).ToList());

            var listaArrendadora = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
            {
                baseDatos = MainContextEnum.Arrendadora,
                consulta = @"SELECT clave_empleado as cveEmpleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion ,estatus_empleado
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE cc='" + cc + "' and estatus_empleado='A'",
            }).ToList().Select(y => new ComboDTO
            {
                Value = y.cveEmpleado,
                Text = y.nombreCompleto + " " + y.ApeidoP + " " + y.ApeidoM
            }).ToList();

            List<ComboDTO> result = new List<ComboDTO>();
            foreach (var item in listaConstruplan)
            {
                result.Add(item);
            }
            foreach (var item in listaArrendadora)
            {
                result.Add(item);
            }
            var listaInstructores = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(x => x.esActivo).ToList().Select(y => y.cveEmpleado).ToList();

            var resultar = result.Where(x => listaInstructores.Contains(x.Value)).ToList();

            return resultar;
        }

        public List<ComboDTO> GetCC()
        {
            try
            {
                var result = _context.tblS_CapacitacionSeguridadRolesGrupoTrabajo.ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<ComboDTO> GetRolesCombo()
        {
            try
            {
                var result = _context.tblS_CapacitacionSeguridadRolesGrupoTrabajo.ToList().Select(y => new ComboDTO
                {
                    Value = y.id.ToString(),
                    Text = y.descripcion
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Dictionary<string, object> PostGuardarInstructor(tblS_CapacitacionSeguridad_PCAdministracionInstructores parametros, bool AddEdit)
        {
            AdministracionInstructoresDTO obj = new AdministracionInstructoresDTO();
            try
            {
                if (AddEdit == false)
                {
                    #region Agregar
                    var objAdd = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(r => r.cveEmpleado == parametros.cveEmpleado && r.esActivo == true).FirstOrDefault();
                    if (objAdd == null)
                    {
                        parametros.esActivo = true;
                        _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Add(parametros);
                        _context.SaveChanges();
                        obj.messaje = "Guardado con exito";
                        obj.status = 1;
                        resultado.Add("data", obj);
                    }
                    else
                    {
                        obj.status = 2;
                        obj.messaje = "Ya existe un registros con esa clave de empleado";
                        resultado.Add("data", obj);
                    }
                    #endregion

                }
                else
                {
                    var objAddEdit = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(r => r.id == parametros.id).FirstOrDefault();
                    objAddEdit.grupo = parametros.grupo;
                    objAddEdit.fechaInicio = parametros.fechaInicio;
                    objAddEdit.instructor = parametros.instructor;
                    objAddEdit.tematica = parametros.tematica;

                    _context.SaveChanges();
                    obj.messaje = "Editado con exito";
                    obj.status = 1;
                    resultado.Add("data", obj);
                }


            }
            catch (Exception)
            {
                obj.status = 0;
                obj.messaje = "Fallo al guardar";
                resultado.Add("data", obj);
                throw;
            }
            return resultado;
        }
        public AdministracionInstructoresDTO getFechaInicio(string cveEmpleado)
        {

            var instructor = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.FirstOrDefault(r => r.cveEmpleado == cveEmpleado && r.esActivo == true);

            if (instructor == null)
            {
                throw new Exception("No se encuentra la información del instructor.");
            }

            var grupo = _context.tblS_CapacitacionSeguridadRolesGrupoTrabajo.FirstOrDefault(x => x.esActivo && x.id == instructor.grupo);

            if (grupo == null)
            {
                throw new Exception("No se encuentra la información del grupo.");
            }

            return new AdministracionInstructoresDTO
            {
                fechaInicio = instructor.fechaInicio,
                grupo = instructor.grupo,
                descripcion = grupo.descripcion,
                cantDiasTrabajados = grupo.cantDiasLaborales,
                cantDiasDescansados = grupo.CantDiasDescando,
                cantDiasTrabajados2 = grupo.cantDiasLaborales2,
                cantDiasDescansados2 = grupo.CantDiasDescando2,
                mixto = grupo.mixto
            };
        }
        public bool EliminarInstructor(int id)
        {
            bool Exitoso = false;

            try
            {
                var obj = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(r => r.id == id).FirstOrDefault();
                obj.esActivo = false;
                _context.SaveChanges();
                Exitoso = true;
            }
            catch (Exception ex)
            {
                Exitoso = false;
                throw;
            }
            return Exitoso;
        }

        public AdministracionInstructoresDTO ObtenerCCUnico(string cveEmpleado)
        {
            AdministracionInstructoresDTO obj = new AdministracionInstructoresDTO();
            try
            {
//                string query = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion
//                                FROM sn_empleados  
//                                INNER JOIN cc  on cc_contable=cc 
//                                WHERE clave_empleado=" + cveEmpleado + " and estatus_empleado='A'";

//                var odbc = new OdbcConsultaDTO() { consulta = query };
//                var listaConstruplan = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.CplanRh, odbc).ToList().Select(y => new AdministracionInstructoresDTO
//                {
//                    cc = y.cc,
//                    descripcion = y.descripcion,
//                    clave_empleado = y.clave_empleado,
//                    nombreCompleto = y.nombreCompleto,
//                    ApeidoM = y.ApeidoM,
//                    ApeidoP = y.ApeidoP,
//                }).FirstOrDefault();
//                var listaArrendadora = _contextEnkontrol.Select<AdministracionInstructoresDTO>(EnkontrolEnum.ArrenRh, odbc).ToList().Select(y => new AdministracionInstructoresDTO
//                {
//                    cc = y.cc,
//                    descripcion = y.descripcion,
//                    clave_empleado = y.clave_empleado,
//                    nombreCompleto = y.nombreCompleto,
//                    ApeidoM = y.ApeidoM,
//                    ApeidoP = y.ApeidoP,
//                }).FirstOrDefault();

                var listaConstruplan = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE clave_empleado=" + cveEmpleado + " and estatus_empleado='A'",
                }).ToList().Select(y => new AdministracionInstructoresDTO
                {
                    cc = y.cc,
                    descripcion = y.descripcion,
                    clave_empleado = y.clave_empleado,
                    nombreCompleto = y.nombreCompleto,
                    ApeidoM = y.ApeidoM,
                    ApeidoP = y.ApeidoP,
                }).FirstOrDefault();

                if (listaConstruplan == null) 
                {
                    listaConstruplan = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE clave_empleado=" + cveEmpleado + " and estatus_empleado='A'",
                    }).ToList().Select(y => new AdministracionInstructoresDTO
                    {
                        cc = y.cc,
                        descripcion = y.descripcion,
                        clave_empleado = y.clave_empleado,
                        nombreCompleto = y.nombreCompleto,
                        ApeidoM = y.ApeidoM,
                        ApeidoP = y.ApeidoP,
                    }).FirstOrDefault();
                }

                var listaArrendadora = _context.Select<AdministracionInstructoresDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Arrendadora,
                    consulta = @"SELECT clave_empleado,nombre as nombreCompleto,ape_paterno as ApeidoP,ape_materno as ApeidoM ,cc ,descripcion
                                FROM tblRH_EK_Empleados  
                                INNER JOIN tblP_CC  on cc_contable=cc 
                                WHERE clave_empleado=" + cveEmpleado + " and estatus_empleado='A'",
                }).ToList().Select(y => new AdministracionInstructoresDTO
                {
                    cc = y.cc,
                    descripcion = y.descripcion,
                    clave_empleado = y.clave_empleado,
                    nombreCompleto = y.nombreCompleto,
                    ApeidoM = y.ApeidoM,
                    ApeidoP = y.ApeidoP,
                }).FirstOrDefault();


                if (listaConstruplan == null && listaArrendadora == null)
                {
                    obj = new AdministracionInstructoresDTO();
                    obj.status = 2;
                    obj.messaje = "Este usuario no se encuentra en sigoplan ni en arrendadora comuniquese con el departamento de ti";
                    throw new Exception("Este usuario no se encuentra en sigoplan ni en arrendadora comuniquese con el departamento de ti");
                }
                if (listaConstruplan != null)
                {
                    obj = new AdministracionInstructoresDTO();
                    obj.cc = listaConstruplan.cc;
                    obj.descripcion = listaConstruplan.descripcion;
                    obj.clave_empleado = listaConstruplan.clave_empleado;
                    obj.nombreCompleto = listaConstruplan.nombreCompleto;
                    obj.ApeidoP = listaConstruplan.ApeidoP;
                    obj.ApeidoM = listaConstruplan.ApeidoM;
                    obj.empresa = Convert.ToString(1);
                }

                if (listaArrendadora != null)
                {
                    obj = new AdministracionInstructoresDTO();
                    obj.cc = listaArrendadora.cc;
                    obj.descripcion = listaArrendadora.descripcion;
                    obj.clave_empleado = listaArrendadora.clave_empleado;
                    obj.nombreCompleto = listaArrendadora.nombreCompleto;
                    obj.ApeidoP = listaArrendadora.ApeidoP;
                    obj.ApeidoM = listaArrendadora.ApeidoM;
                    obj.empresa = Convert.ToString(2);
                }


            }
            catch (Exception e)
            {
                resultado.Add("data", e.Message);
                LogError(0, 0, NombreControlador, "ObtenerCCUnico", e, AccionEnum.CONSULTA, 0, null);
            }
            return obj;
        }
        #endregion

        #region Plan de Capacitación
        public Dictionary<string, object> cargarCalendarioPlanCapacitacion(string cc, List<TematicaCursoEnum> listaTematicas, int empresa, DateTime mesCalendario)
        {
            try
            {
                #region Información Base
                var listaCursosAplicables = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList().Where(x =>
                    x.CentrosCosto.Select(y => y.cc).Contains(cc) && (listaTematicas != null && listaTematicas.Count() > 0 ? listaTematicas.Contains((TematicaCursoEnum)x.tematica) : true)
                ).ToList();
                var listaInstructores = _context.tblS_CapacitacionSeguridad_PCAdministracionInstructores.Where(x =>
                    x.esActivo && x.division == divisionActual && x.instructor && x.empresa == empresa
                ).ToList();
                var listaInstructoresPorCC = new List<tblS_CapacitacionSeguridad_PCAdministracionInstructores>();

                #region Filtrar instructores por centro de costo
                if (empresa == (int)EmpresaEnum.Construplan)
                {
                    //var listaEmpleadosConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                    //});

                    var listaEmpleadosConstruplan = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                    });

                    listaEmpleadosConstruplan.AddRange(_context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.GCPLAN,
                        consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 1 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                    }));

                    foreach (var instructor in listaInstructores)
                    {
                        var empleado = listaEmpleadosConstruplan.FirstOrDefault(x => Convert.ToInt32(x.clave_empleado) == Int32.Parse(instructor.cveEmpleado));

                        if (empleado != null)
                        {
                            if ((string)empleado.cc_contable == cc)
                            {
                                listaInstructoresPorCC.Add(instructor);
                            }
                        }
                    }
                }
                else if (empresa == (int)EmpresaEnum.Arrendadora)
                {
                    //var listaEmpleadosArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                    //{
                    //    consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 2 AS empresa FROM sn_empleados WHERE estatus_empleado = 'A'"
                    //});

                    var listaEmpleadosArrendadora = _context.Select<dynamic>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_empleado, clave_depto, cc_contable, 2 AS empresa FROM tblRH_EK_Empleados WHERE estatus_empleado = 'A'"

                    });

                    foreach (var instructor in listaInstructores)
                    {
                        var empleado = listaEmpleadosArrendadora.FirstOrDefault(x => Convert.ToInt32(x.clave_empleado) == Int32.Parse(instructor.cveEmpleado));

                        if (empleado != null)
                        {
                            if ((string)empleado.cc_contable == cc)
                            {
                                listaInstructoresPorCC.Add(instructor);
                            }
                        }
                    }
                }
                #endregion

                #region Lista Empleados
                List<EmpleadoCapacitacionDTO> empleados = new List<EmpleadoCapacitacionDTO>();

                if (empresa == (int)EmpresaEnum.Construplan)
                {
                    //var departamentosConstruplan = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.CplanRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM sn_departamentos WHERE cc = ?",
                    //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc } }
                    //});

                    var departamentosConstruplan = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos WHERE cc = @cc",
                        parametros = new { cc}
                    });

                    empleados = ObtenerEmpleadosCCPorDepartamento(
                        new List<string> { cc },
                        null,
                        departamentosConstruplan.Count() > 0 ? departamentosConstruplan.Select(x => x.id).ToList() : null
                    );
                }
                else if (empresa == (int)EmpresaEnum.Arrendadora)
                {
                    //var departamentosArrendadora = _contextEnkontrol.Select<DepartamentoDTO>(EnkontrolEnum.ArrenRh, new OdbcConsultaDTO
                    //{
                    //    consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM sn_departamentos WHERE cc = ?",
                    //    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = cc } }
                    //});

                    var departamentosArrendadora = _context.Select<DepartamentoDTO>(new DapperDTO
                    {
                        baseDatos = MainContextEnum.Arrendadora,
                        consulta = @"SELECT clave_depto as id, desc_depto as departamento, cc FROM tblRH_EK_Departamentos WHERE cc = @cc",
                        parametros = new { cc }
                    });

                    empleados = ObtenerEmpleadosCCPorDepartamento(
                        null,
                        new List<string> { cc },
                        departamentosArrendadora.Count() > 0 ? departamentosArrendadora.Select(x => x.id).ToList() : null
                    );
                }
                #endregion

                var listaColores = new List<Tuple<ClasificacionCursoEnum, string>> {
                    new Tuple<ClasificacionCursoEnum, string>(ClasificacionCursoEnum.TecnicoOperativo, "rgb(255, 102, 0)"),
                    new Tuple<ClasificacionCursoEnum, string>(ClasificacionCursoEnum.ProtocoloFatalidad, "rgb(192, 0, 0)"),
                    new Tuple<ClasificacionCursoEnum, string>(ClasificacionCursoEnum.InstructivoOperativo, "rgb(0, 176, 80)"),
                    new Tuple<ClasificacionCursoEnum, string>(ClasificacionCursoEnum.Normativo, "rgb(46, 117, 182)"),
                    new Tuple<ClasificacionCursoEnum, string>(ClasificacionCursoEnum.Formativo, "rgb(0, 176, 240)")
                };

                var listaOrdenPrioridadClasificacion = new List<Tuple<ClasificacionCursoEnum, int>> {
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.TecnicoOperativo, 1),
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.ProtocoloFatalidad, 2),
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.HabilidadesTécnicas, 3), //Inducción?
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.InstructivoOperativo, 4),
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.Normativo, 5),
                    new Tuple<ClasificacionCursoEnum, int>(ClasificacionCursoEnum.Formativo, 6)
                };

                var listaGrupos = _context.tblS_CapacitacionSeguridadRolesGrupoTrabajo.Where(x => x.esActivo).ToList();
                var listaCursosPersonalAutorizado = _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.reglaPersonalAutorizado).ToList();
                var fechaMinima = DateTime.Today.AddHours(23).AddMinutes(59).AddYears(-1); // Fecha un año atrás al día de hoy para las capacitaciones vigentes.
                #endregion

                var eventos = new List<EventoCalendarioDTO>();
                var consecutivoEventos = 0;

                foreach (TematicaCursoEnum tematica in listaTematicas ?? Enum.GetValues(typeof(TematicaCursoEnum)).Cast<TematicaCursoEnum>().ToList())
                {
                    if (tematica != TematicaCursoEnum.noAsignado)
                    {
                        var listaInstructoresPorTematica = listaInstructoresPorCC.Where(x => x.tematica == (int)tematica).ToList();

                        if (listaInstructoresPorTematica.Count() > 0)
                        {
                            #region Lista Agendas
                            List<DiaAgendaDTO> listaAgendas = new List<DiaAgendaDTO>();

                            foreach (var instructor in listaInstructoresPorTematica)
                            {
                                var grupoSIGOPLAN = listaGrupos.FirstOrDefault(x => x.id == instructor.grupo);

                                if (grupoSIGOPLAN != null)
                                {
                                    #region Cálculo de días agenda por mes
                                    List<DiaAgendaDTO> diasAgenda = new List<DiaAgendaDTO>();
                                    DateTime ultimoDiaMesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                                    diasAgenda = getDiasAgenda((DateTime)instructor.fechaInicio, ultimoDiaMesActual, grupoSIGOPLAN);

                                    //var anioFiltro = mesCalendario.Year;
                                    //var mesFiltro = mesCalendario.Month;

                                    //diasAgenda = diasAgenda.Where(x => x.dia.Year == anioFiltro && x.dia.Month == mesFiltro).ToList();
                                    #endregion

                                    listaAgendas.AddRange(diasAgenda);
                                }
                                else
                                {
                                    throw new Exception("No se encuentra la información del grupo para el instructor con la clave \"" + instructor.cveEmpleado + "\".");
                                }
                            }

                            listaAgendas = listaAgendas.Where(x => x.laboral).DistinctBy(x => x.dia).OrderBy(x => x.dia).ToList();
                            #endregion

                            var listaCursosPorTematica = listaCursosAplicables.Where(x => x.tematica == (int)tematica).ToList();
                            var listaMeses = MonthsBetween(listaAgendas.First().dia, DateTime.Now);

                            foreach (var mes in listaMeses)
                            {
                                var flagMesCompleto = false;
                                DateTime primerDiaMes = new DateTime(mes.Item2, mes.Item1, 1);
                                DateTime ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1);
                                DateTime primerDiaMesAnterior = primerDiaMes.AddMonths(-1);
                                DateTime ultimoDiaMesAnterior = primerDiaMesAnterior.AddMonths(1).AddDays(-1);

                                List<string> listaCursosAsignadosMes = new List<string>();

                                var listaAgendasPorMes = listaAgendas.Where(x => x.dia.Month == mes.Item1 && x.dia.Year == mes.Item2).ToList();
                                var diaAsignado = listaAgendasPorMes.Min(x => x.dia);

                                int index = 1; //Empieza en 1 para agarrar el segundo día después del primer objeto inicial de "eventos".
                                int contadorCursosAlDia = 0;
                                int flagCursoTecnicoOperativo = 0;

                                #region Primer Regla: Porcentajes de Capacitación
                                #region Obtener Indicadores - Lista Estadísticas
                                var indicadores = new List<IndicadorClasificacionDTO>();
                                var capacitacionesPorCC = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                                    x.activo  && x.division == divisionActual && x.cc == cc
                                ).ToList().Where(x =>
                                    x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                                    listaCursosPorTematica.Select(y => y.id).Contains(x.cursoID) &&
                                    (x.fechaCapacitacion.Date >= primerDiaMes.Date && x.fechaCapacitacion.Date <= ultimoDiaMes.Date)
                                ).ToList();

                                foreach (var empleado in empleados)
                                {
                                    var cursosEmpleado = capacitacionesPorCC.Where(x => x.asistentes.Exists(y => y.claveEmpleado == empleado.claveEmpleado)).ToList();

                                    foreach (var cursoAplicable in listaCursosPorTematica)
                                    {
                                        ObtenerEstatusCursoEmpleado(cursoAplicable, cursosEmpleado, empleado, fechaMinima, ref indicadores);
                                    }
                                }

                                var listaEstadisticasPorcentaje = new List<EstadisticaPorcentajeDTO>();

                                indicadores.GroupBy(x => x.cursoDesc).OrderBy(x => x.Key).ForEach(x =>
                                {
                                    int totalCapacitados = x.Where(y => y.capacitado).Count();
                                    int totalFaltante = x.Where(y => y.capacitado == false).Count();
                                    int totalPersonalAplica = x.Count();
                                    decimal porcentaje = totalPersonalAplica > 0 ? Math.Round((totalCapacitados * 100) / (decimal)totalPersonalAplica, 2) : 0;
                                    var reglaPersonalAutorizado = listaCursosPersonalAutorizado.FirstOrDefault(y => y.claveCurso == x.First().cursoClave) != null;

                                    if (reglaPersonalAutorizado)
                                    {
                                        porcentaje = (porcentaje * 2 > 100) ? 100 : porcentaje * 2;
                                    }

                                    var estadistica = new EstadisticaPorcentajeDTO
                                    {
                                        cursoID = x.First().cursoID,
                                        claveCurso = x.First().cursoClave,
                                        cursoDesc = x.First().cursoDesc,
                                        clasificacion = x.First().clasificacion,
                                        totalVigentes = totalCapacitados,
                                        totalFaltante = totalFaltante,
                                        personalAplica = totalPersonalAplica,
                                        porcentaje = porcentaje,
                                        porcentajeString = porcentaje + " %",
                                        reglaPersonalAutorizado = reglaPersonalAutorizado,
                                        tematica = x.First().tematica
                                    };

                                    listaEstadisticasPorcentaje.Add(estadistica);
                                });

                                var listaEstadisticasOrdenada = (
                                    from est in listaEstadisticasPorcentaje
                                    join ord in listaOrdenPrioridadClasificacion on est.clasificacion equals ord.Item1
                                    select new
                                    {
                                        claveCurso = est.claveCurso,
                                        clasificacion = est.clasificacion,
                                        porcentaje = est.porcentaje,
                                        ordenClasificacion = ord.Item2
                                    }
                                ).OrderBy(x => x.porcentaje).ThenBy(x => x.ordenClasificacion).ToList();
                                #endregion

                                foreach (var curso in listaEstadisticasOrdenada)
                                {
                                    if (flagMesCompleto)
                                    {
                                        continue;
                                    }

                                    if (diaAsignado <= ultimoDiaMes)
                                    {
                                        if (curso.clasificacion != ClasificacionCursoEnum.TecnicoOperativo)
                                        {
                                            if (diaAsignado.DayOfWeek == DayOfWeek.Wednesday)
                                            {
                                                if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                {
                                                    diaAsignado = listaAgendasPorMes[index].dia;
                                                }
                                                else
                                                {
                                                    var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                    if (cursosAsignadosDia.Count() > 0)
                                                    {
                                                        if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                        {
                                                            flagMesCompleto = true;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }

                                            eventos.Add(new EventoCalendarioDTO
                                            {
                                                consecutivoEvento = consecutivoEventos++,
                                                title = curso.claveCurso,
                                                start = diaAsignado.ToString("yyyy-MM-dd"),
                                                end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                classNames = "",
                                                backgroundColor = listaColores.Where(x => x.Item1 == curso.clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                fechaStart = diaAsignado,
                                                fechaEnd = diaAsignado.AddDays(1),
                                                clasificacion = curso.clasificacion
                                            });

                                            if (flagCursoTecnicoOperativo == 2)
                                            {
                                                contadorCursosAlDia = 0;
                                            }

                                            contadorCursosAlDia++;
                                            listaCursosAsignadosMes.Add(curso.claveCurso);

                                            if (contadorCursosAlDia >= 5)
                                            {
                                                if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                {
                                                    diaAsignado = listaAgendasPorMes[index].dia;

                                                    index++;
                                                    contadorCursosAlDia = 0;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }

                                            flagCursoTecnicoOperativo = 1;
                                        }
                                        else
                                        {
                                            if (flagCursoTecnicoOperativo == 1)
                                            {
                                                if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                {
                                                    diaAsignado = listaAgendasPorMes[index].dia;
                                                    index++;
                                                }
                                                else
                                                {
                                                    var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                    if (cursosAsignadosDia.Count() > 0)
                                                    {
                                                        flagMesCompleto = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            for (int i = 0; i < 3; i++) //Los cursos de clasificación "Técnico Operativo" tienen duración de tres días.
                                            {
                                                eventos.Add(new EventoCalendarioDTO
                                                {
                                                    consecutivoEvento = consecutivoEventos++,
                                                    title = curso.claveCurso,
                                                    start = diaAsignado.ToString("yyyy-MM-dd"),
                                                    end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                    classNames = "",
                                                    backgroundColor = listaColores.Where(x => x.Item1 == curso.clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                    fechaStart = diaAsignado,
                                                    fechaEnd = diaAsignado.AddDays(1),
                                                    clasificacion = curso.clasificacion
                                                });

                                                flagCursoTecnicoOperativo = 2;

                                                if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                {
                                                    diaAsignado = listaAgendasPorMes[index].dia;
                                                    index++;
                                                }
                                                else
                                                {
                                                    var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                    if (cursosAsignadosDia.Count() > 0)
                                                    {
                                                        flagMesCompleto = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            contadorCursosAlDia = 5;

                                            listaCursosAsignadosMes.Add(curso.claveCurso);
                                        }
                                    }
                                }
                                #endregion

                                #region Segunda Regla: Detecciones de Necesidades de tipo "Capacitación" o "Adiestramiento"
                                var listaDeteccionesPrimarias = _context.tblS_CapacitacionSeguridadDNDeteccionPrimaria.Where(x =>
                                    x.estatus && x.division == divisionActual && x.cc == cc && x.empresa == empresa
                                ).ToList().Where(x => x.fecha.Date >= primerDiaMesAnterior.Date && x.fecha.Date <= ultimoDiaMesAnterior.Date).ToList();
                                var listaNecesidades = (
                                    from prim in listaDeteccionesPrimarias
                                    join nec in _context.tblS_CapacitacionSeguridadDNDeteccionPrimariaNecesidad.Where(x => x.estatus).ToList() on prim.id equals nec.deteccionPrimariaID
                                    join cur in _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList() on nec.accionesCursoID equals cur.id
                                    where (nec.metodo == (int)MetodoEnum.capacitacion || nec.metodo == (int)MetodoEnum.adiestramiento) && (listaCursosPorTematica.Select(x => x.id).Contains(cur.id))
                                    select new
                                    {
                                        metodo = nec.metodo,
                                        accionesCursoID = nec.accionesCursoID,
                                        deteccionPrimariaID = nec.deteccionPrimariaID,
                                        claveCurso = cur.claveCurso,
                                        clasificacion = cur.clasificacion
                                    }
                                ).ToList();

                                if (listaNecesidades.Count() > 0)
                                {
                                    var listaNecesidadesAgrupada = listaNecesidades.GroupBy(x => x.claveCurso).Select(x => new { claveCurso = x.Key, grp = x }).Where(x =>
                                        x.grp.Count() >= 3 //Se escogen los cursos con tres o más capacitaciones o adiestramientos.
                                    ).ToList();

                                    flagCursoTecnicoOperativo = 0;
                                    foreach (var curso in listaNecesidadesAgrupada)
                                    {
                                        if (flagMesCompleto)
                                        {
                                            continue;
                                        }

                                        if (diaAsignado <= ultimoDiaMesAnterior)
                                        {
                                            if (!listaCursosAsignadosMes.Contains(curso.claveCurso))
                                            {
                                                if (curso.grp.First().clasificacion != (int)ClasificacionCursoEnum.TecnicoOperativo)
                                                {
                                                    if (diaAsignado.DayOfWeek == DayOfWeek.Wednesday)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                                {
                                                                    flagMesCompleto = true;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    eventos.Add(new EventoCalendarioDTO
                                                    {
                                                        consecutivoEvento = consecutivoEventos++,
                                                        title = curso.claveCurso,
                                                        start = diaAsignado.ToString("yyyy-MM-dd"),
                                                        end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                        classNames = "",
                                                        backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                        fechaStart = diaAsignado,
                                                        fechaEnd = diaAsignado.AddDays(1),
                                                        clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                    });

                                                    if (flagCursoTecnicoOperativo == 2)
                                                    {
                                                        contadorCursosAlDia = 0;
                                                    }

                                                    contadorCursosAlDia++;
                                                    listaCursosAsignadosMes.Add(curso.claveCurso);

                                                    if (contadorCursosAlDia >= 5)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;

                                                            index++;
                                                            contadorCursosAlDia = 0;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                                {
                                                                    flagMesCompleto = true;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    flagCursoTecnicoOperativo = 1;
                                                }
                                                else
                                                {
                                                    if (flagCursoTecnicoOperativo == 1)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                            index++;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    for (int i = 0; i < 3; i++) //Los cursos de clasificación "Técnico Operativo" tienen duración de tres días.
                                                    {
                                                        eventos.Add(new EventoCalendarioDTO
                                                        {
                                                            consecutivoEvento = consecutivoEventos++,
                                                            title = curso.claveCurso,
                                                            start = diaAsignado.ToString("yyyy-MM-dd"),
                                                            end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                            classNames = "",
                                                            backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                            fechaStart = diaAsignado,
                                                            fechaEnd = diaAsignado.AddDays(1),
                                                            clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                        });

                                                        flagCursoTecnicoOperativo = 2;

                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                            index++;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    contadorCursosAlDia = 5;

                                                    listaCursosAsignadosMes.Add(curso.claveCurso);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Tercer Regla: Ciclos de trabajo con acciones de tipo "Capacitación" o "Adiestramiento"
                                var listaCiclosTrabajoRegistro = _context.tblS_CapacitacionSeguridadDNCicloTrabajoRegistro.Where(x =>
                                    x.estatus && x.division == divisionActual && x.cc == cc && x.empresa == empresa
                                ).ToList().Where(x => ((DateTime)x.fecha).Date >= primerDiaMesAnterior.Date && ((DateTime)x.fecha).Date <= ultimoDiaMesAnterior.Date).ToList();

                                if (listaCiclosTrabajoRegistro.Count() > 0)
                                {
                                    var listaCiclosTrabajoRegistroFiltrada = (
                                        from cic in listaCiclosTrabajoRegistro
                                        join cur in _context.tblS_CapacitacionSeguridadCursos.Where(x => x.isActivo && x.division == divisionActual).ToList() on cic.cursoID equals cur.id
                                        where
                                            (cic.metodo == (int)MetodoAccionRequeridaEnum.capacitacion || cic.metodo == (int)MetodoAccionRequeridaEnum.adiestramiento) &&
                                            (listaCursosPorTematica.Select(x => x.id).Contains(cur.id))
                                        select new
                                        {
                                            metodo = cic.metodo,
                                            cursoID = cic.cursoID,
                                            cicloID = cic.cicloID,
                                            claveCurso = cur.claveCurso,
                                            clasificacion = cur.clasificacion
                                        }
                                    ).ToList();
                                    var listaCiclosTrabajoRegistroAgrupada = listaCiclosTrabajoRegistroFiltrada.GroupBy(x => x.claveCurso).Select(x => new { claveCurso = x.Key, grp = x }).Where(x =>
                                        x.grp.Count() >= 3 //Se escogen los cursos con tres o más capacitaciones o adiestramientos.
                                    ).ToList();

                                    flagCursoTecnicoOperativo = 0;
                                    foreach (var curso in listaCiclosTrabajoRegistroAgrupada)
                                    {
                                        if (flagMesCompleto)
                                        {
                                            continue;
                                        }

                                        if (diaAsignado <= ultimoDiaMesAnterior)
                                        {
                                            if (!listaCursosAsignadosMes.Contains(curso.claveCurso))
                                            {
                                                if (curso.grp.First().clasificacion != (int)ClasificacionCursoEnum.TecnicoOperativo)
                                                {
                                                    if (diaAsignado.DayOfWeek == DayOfWeek.Wednesday)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                                {
                                                                    flagMesCompleto = true;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    eventos.Add(new EventoCalendarioDTO
                                                    {
                                                        consecutivoEvento = consecutivoEventos++,
                                                        title = curso.claveCurso,
                                                        start = diaAsignado.ToString("yyyy-MM-dd"),
                                                        end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                        classNames = "",
                                                        backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                        fechaStart = diaAsignado,
                                                        fechaEnd = diaAsignado.AddDays(1),
                                                        clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                    });

                                                    if (flagCursoTecnicoOperativo == 2)
                                                    {
                                                        contadorCursosAlDia = 0;
                                                    }

                                                    contadorCursosAlDia++;
                                                    listaCursosAsignadosMes.Add(curso.claveCurso);

                                                    if (contadorCursosAlDia >= 5)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;

                                                            index++;
                                                            contadorCursosAlDia = 0;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                                {
                                                                    flagMesCompleto = true;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    flagCursoTecnicoOperativo = 1;
                                                }
                                                else
                                                {
                                                    if (flagCursoTecnicoOperativo == 1)
                                                    {
                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                            index++;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    for (int i = 0; i < 3; i++) //Los cursos de clasificación "Técnico Operativo" tienen duración de tres días.
                                                    {
                                                        eventos.Add(new EventoCalendarioDTO
                                                        {
                                                            consecutivoEvento = consecutivoEventos++,
                                                            title = curso.claveCurso,
                                                            start = diaAsignado.ToString("yyyy-MM-dd"),
                                                            end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                            classNames = "",
                                                            backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                            fechaStart = diaAsignado,
                                                            fechaEnd = diaAsignado.AddDays(1),
                                                            clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                        });

                                                        flagCursoTecnicoOperativo = 2;

                                                        if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                        {
                                                            diaAsignado = listaAgendasPorMes[index].dia;
                                                            index++;
                                                        }
                                                        else
                                                        {
                                                            var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                            if (cursosAsignadosDia.Count() > 0)
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    contadorCursosAlDia = 5;

                                                    listaCursosAsignadosMes.Add(curso.claveCurso);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region Cuarta Regla: Promedio de calificaciones
                                var controlesAsistenciaPorCC = _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x =>
                                    x.activo  && x.division == divisionActual && x.cc == cc
                                ).ToList().Where(x =>
                                    x.estatus == (int)EstatusControlAsistenciaEnum.Completa &&
                                    listaCursosPorTematica.Select(y => y.id).Contains(x.cursoID) &&
                                    (x.fechaCapacitacion.Date >= primerDiaMesAnterior.Date && x.fechaCapacitacion.Date <= ultimoDiaMesAnterior.Date)
                                ).ToList();
                                var listaControlesAsistenciaFiltrada = (
                                    from cont in controlesAsistenciaPorCC
                                    join asis in _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(x => x.division == divisionActual).ToList() on cont.id equals asis.controlAsistenciaID
                                    where asis.calificacion < 85
                                    select new
                                    {
                                        controlAsistenciaID = cont.id,
                                        claveCurso = cont.curso.claveCurso,
                                        clasificacion = cont.curso.clasificacion,
                                        detalleID = asis.id,
                                        claveEmpleado = asis.claveEmpleado,
                                        calificacion = asis.calificacion
                                    }
                                ).ToList();
                                var listaControlesAsistenciaAgrupada = listaControlesAsistenciaFiltrada.GroupBy(x => x.claveCurso).Select(x => new
                                {
                                    claveCurso = x.Key,
                                    grp = x
                                }).ToList();
                                var listaGruposFiltrados = listaControlesAsistenciaAgrupada.Where(x => x.grp.Count() >= 20).ToList();

                                flagCursoTecnicoOperativo = 0;
                                foreach (var curso in listaGruposFiltrados)
                                {
                                    if (flagMesCompleto)
                                    {
                                        continue;
                                    }

                                    if (diaAsignado <= ultimoDiaMes)
                                    {
                                        if (!listaCursosAsignadosMes.Contains(curso.claveCurso))
                                        {
                                            if (curso.grp.First().clasificacion != (int)ClasificacionCursoEnum.TecnicoOperativo)
                                            {
                                                if (diaAsignado.DayOfWeek == DayOfWeek.Wednesday)
                                                {
                                                    if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                    {
                                                        diaAsignado = listaAgendasPorMes[index].dia;
                                                    }
                                                    else
                                                    {
                                                        var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                        if (cursosAsignadosDia.Count() > 0)
                                                        {
                                                            if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }

                                                eventos.Add(new EventoCalendarioDTO
                                                {
                                                    consecutivoEvento = consecutivoEventos++,
                                                    title = curso.claveCurso,
                                                    start = diaAsignado.ToString("yyyy-MM-dd"),
                                                    end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                    classNames = "",
                                                    backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                    fechaStart = diaAsignado,
                                                    fechaEnd = diaAsignado.AddDays(1),
                                                    clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                });

                                                if (flagCursoTecnicoOperativo == 2)
                                                {
                                                    contadorCursosAlDia = 0;
                                                }

                                                contadorCursosAlDia++;
                                                listaCursosAsignadosMes.Add(curso.claveCurso);

                                                if (contadorCursosAlDia >= 5)
                                                {
                                                    if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                    {
                                                        diaAsignado = listaAgendasPorMes[index].dia;

                                                        index++;
                                                        contadorCursosAlDia = 0;
                                                    }
                                                    else
                                                    {
                                                        var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                        if (cursosAsignadosDia.Count() > 0)
                                                        {
                                                            if (cursosAsignadosDia.Count() > 5 || cursosAsignadosDia.Select(x => x.clasificacion).Contains(ClasificacionCursoEnum.TecnicoOperativo))
                                                            {
                                                                flagMesCompleto = true;
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }

                                                flagCursoTecnicoOperativo = 1;
                                            }
                                            else
                                            {
                                                if (flagCursoTecnicoOperativo == 1)
                                                {
                                                    if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                    {
                                                        diaAsignado = listaAgendasPorMes[index].dia;
                                                        index++;
                                                    }
                                                    else
                                                    {
                                                        var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                        if (cursosAsignadosDia.Count() > 0)
                                                        {
                                                            flagMesCompleto = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                for (int i = 0; i < 3; i++) //Los cursos de clasificación "Técnico Operativo" tienen duración de tres días.
                                                {
                                                    eventos.Add(new EventoCalendarioDTO
                                                    {
                                                        consecutivoEvento = consecutivoEventos++,
                                                        title = curso.claveCurso,
                                                        start = diaAsignado.ToString("yyyy-MM-dd"),
                                                        end = diaAsignado.AddDays(1).ToString("yyyy-MM-dd"),
                                                        classNames = "",
                                                        backgroundColor = listaColores.Where(x => (int)x.Item1 == curso.grp.First().clasificacion).Select(y => y.Item2).FirstOrDefault(),
                                                        fechaStart = diaAsignado,
                                                        fechaEnd = diaAsignado.AddDays(1),
                                                        clasificacion = (ClasificacionCursoEnum)curso.grp.First().clasificacion
                                                    });

                                                    flagCursoTecnicoOperativo = 2;

                                                    if (listaAgendasPorMes.ElementAtOrDefault(index) != null)
                                                    {
                                                        diaAsignado = listaAgendasPorMes[index].dia;
                                                        index++;
                                                    }
                                                    else
                                                    {
                                                        var cursosAsignadosDia = eventos.Where(x => x.fechaStart == diaAsignado).ToList();

                                                        if (cursosAsignadosDia.Count() > 0)
                                                        {
                                                            flagMesCompleto = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                contadorCursosAlDia = 5;

                                                listaCursosAsignadosMes.Add(curso.claveCurso);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }

                var eventosOrdenados = eventos.OrderBy(x => x.fechaStart).ToList();
                var listaDiaCantidadCursos = new List<Tuple<DateTime, int>>();

                for (var dia = eventosOrdenados.First().fechaStart.Date; dia.Date <= eventosOrdenados.Last().fechaEnd.Date; dia = dia.AddDays(1))
                {
                    var eventosDia = eventosOrdenados.Where(x => dia.Date == x.fechaStart.Date).OrderBy(x => x.consecutivoEvento).ToList();
                    var primerEventoTecnicoOperativo = eventosDia.FirstOrDefault(x => x.clasificacion == ClasificacionCursoEnum.TecnicoOperativo);

                    if (primerEventoTecnicoOperativo == null)
                    {
                        if (eventosDia.Count() > 5)
                        {
                            var primeros5Eventos = eventosDia.Take(5);

                            foreach (var evento in eventosDia)
                            {
                                if (!primeros5Eventos.Select(x => x.title).Contains(evento.title))
                                {
                                    eventosOrdenados.Remove(evento);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var evento in eventosDia)
                        {
                            if (evento.title != primerEventoTecnicoOperativo.title)
                            {
                                eventosOrdenados.Remove(evento);
                            }
                        }
                    }
                }

                resultado.Add("data", eventosOrdenados);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
                LogError(0, 0, NombreControlador, "cargarCalendarioPlanCapacitacion", e, AccionEnum.CONSULTA, 0, new { cc = cc });
            }

            return resultado;
        }

        public List<DiaAgendaDTO> getDiasAgenda(DateTime fechaInicio, DateTime fechaFin, tblS_CapacitacionSeguridadRolesGrupoTrabajo grupoTrabajo)
        {
            List<DiaAgendaDTO> dias = new List<DiaAgendaDTO>();
            List<Tuple<int, bool>> dicDiasTrabajoDescanso = new List<Tuple<int, bool>>();

            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(0, grupoTrabajo.cantDiasLaborales).Select(index => new Tuple<int, bool>(index, true)).ToList());
            dicDiasTrabajoDescanso.AddRange(Enumerable.Range(grupoTrabajo.cantDiasLaborales, grupoTrabajo.CantDiasDescando).Select(index => new Tuple<int, bool>(index, false)).ToList());

            //Para los grupos mixtos se agrega otro set de días para el "segundo ciclo" del grupo.
            if (grupoTrabajo.mixto)
            {
                var contadorPrimerSet = dicDiasTrabajoDescanso.Count();

                dicDiasTrabajoDescanso.AddRange(Enumerable.Range(contadorPrimerSet, grupoTrabajo.cantDiasLaborales2).Select(index => new Tuple<int, bool>(index, true)).ToList());
                dicDiasTrabajoDescanso.AddRange(Enumerable.Range(grupoTrabajo.cantDiasLaborales2 + contadorPrimerSet, grupoTrabajo.CantDiasDescando2).Select(index => new Tuple<int, bool>(index, false)).ToList());
            }

            int contador = 0;

            for (var dia = fechaInicio.Date; dia.Date <= fechaFin.Date; dia = dia.AddDays(1))
            {
                if (contador == dicDiasTrabajoDescanso.Count())
                {
                    contador = 0;
                }

                var diaDiccionario = dicDiasTrabajoDescanso.FirstOrDefault(x => x.Item1 == contador);

                dias.Add(new DiaAgendaDTO { dia = dia, laboral = diaDiccionario.Item2 });

                contador++;
            }

            return dias;
        }

        private DateTime getDiaAsignado(DateTime primerDiaMes, DateTime ultimoDiaMes, List<DiaAgendaDTO> listaAgendasInstructores)
        {
            var diaAsignado = primerDiaMes;

            while (!listaAgendasInstructores.Select(x => x.dia).Contains(diaAsignado) && diaAsignado <= ultimoDiaMes)
            {
                diaAsignado = diaAsignado.AddDays(1);
            }

            return diaAsignado;
        }

        public static IEnumerable<Tuple<int, int>> MonthsBetween(DateTime startDate, DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return new Tuple<int, int>(iterator.Month, iterator.Year);

                iterator = iterator.AddMonths(1);
            }
        }
        #endregion

        public Dictionary<string, object> GetEmpleadoCursosActos(int claveEmpleado)
        {
            try
            {
                var listaActos = _context.tblSAC_Acto.Where(x => x.activo && x.tipoActo == Core.Enum.Administracion.Seguridad.ActoCondicion.TipoActo.Inseguro && x.claveEmpleado == claveEmpleado).ToList();
                var listaCapacitaciones = (
                    from cap in _context.tblS_CapacitacionSeguridadControlAsistencia.Where(x => x.activo && x.asistentes.Select(y => y.claveEmpleado).Contains(claveEmpleado)).ToList()
                    join det in _context.tblS_CapacitacionSeguridadControlAsistenciaDetalle.Where(x => x.claveEmpleado == claveEmpleado).ToList() on cap.id equals det.controlAsistenciaID
                    select new
                    {
                        id = det.id,
                        cursoID = cap.cursoID,
                        curso = cap.curso.nombre,
                        cc = cap.cc,
                        examenID = det.examenID,
                        calificacion = det.calificacion,
                        divison = det.division,
                        fecha = cap.fechaCapacitacion.ToShortDateString()
                    }
                ).ToList();

                var data = new
                {
                    actos = listaActos,
                    cantidadActos = listaActos.Count,
                    capacitaciones = listaCapacitaciones,
                    cantidadCursos = listaCapacitaciones.GroupBy(x => x.cursoID).Select(x => x.Key).Count()
                };

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                LogError(0, 0, NombreControlador, "GetEmpleadoCursosActos", e, AccionEnum.CONSULTA, 0, claveEmpleado);
            }
            return resultado;
        }
    }
}
