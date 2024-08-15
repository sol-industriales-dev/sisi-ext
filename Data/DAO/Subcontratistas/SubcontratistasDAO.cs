using Core.DAO.Subcontratistas;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.SubContratistas;
using Core.DTO;
using Core.Enum.Principal.Bitacoras;
using Core.DTO.Subcontratistas;
using System.Web;
using Infrastructure.Utils;
using System.IO;
using Data.EntityFramework.Context;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.Enum;

namespace Data.DAO.Subcontratistas
{
    public class SubcontratistasDAO : GenericDAO<tblX_SubContratista>, ISubcontratistasDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();

        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SUBCONTRATISTAS";
        private const string RutaLocal = @"C:\Proyecto\SUBCONTRATISTAS";
        private const string NombreBaseArchivoSubcontratistas = @"ArchivoSubcontratista";
        private readonly string RutaArchivos;
        private readonly string RutaArchivosJustificacion;

        #region Constructor
        public SubcontratistasDAO()
        {
            resultado.Clear();

#if DEBUG
            RutaBase = RutaLocal;
#endif

            RutaArchivos = Path.Combine(RutaBase, @"ARCHIVOS_FIJOS");
            RutaArchivosJustificacion = Path.Combine(RutaBase, @"ARCHIVOS_JUSTIFICACION");
        }
        #endregion

        #region Catálogos
        #region Subcontratistas
        public Dictionary<string, object> getSubcontratistas()
        {
            try
            {
                var listaSubcontratistas = _context.tblX_SubContratista.Where(x => x.estatus).ToList();

                resultado.Add("data", listaSubcontratistas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getSubcontratistas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoSubcontratista(tblX_SubContratista subcontratista)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    if (subcontratista.numeroProveedor == 0)
                    {
                        throw new Exception("Debe capturar el número del proveedor.");
                    }

                    if (subcontratista.rfc == "" || subcontratista.rfc == null || subcontratista.correo == "" || subcontratista.correo == null)
                    {
                        throw new Exception("Debe capturar el RFC y el correo.");
                    }

                    var subcontratistaExistente = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.rfc == subcontratista.rfc);

                    if (subcontratistaExistente != null)
                    {
                        throw new Exception("Ya existe un subcontratista con el RFC \"" + subcontratista.rfc + "\".");
                    }
                    #endregion

                    _context.tblX_SubContratista.Add(subcontratista);
                    _context.SaveChanges();

                    #region Crear Usuario
                    //var usuarioSubcontratistaNuevo = new tblX_Usuarios();

                    //usuarioSubcontratistaNuevo._user = subcontratista.rfc;
                    //usuarioSubcontratistaNuevo._pass = "Expediente2021";
                    //usuarioSubcontratistaNuevo.nombre_completo = subcontratista.nombre;
                    //usuarioSubcontratistaNuevo.tipo = 3;
                    //usuarioSubcontratistaNuevo.estatus = true;

                    //_context.tblX_Usuarios.Add(usuarioSubcontratistaNuevo);
                    //_context.SaveChanges();
                    #endregion

                    //                    #region Enviar correo de alta
                    //                    var listaCorreos = new List<string>();

                    //                    listaCorreos.Add(subcontratista.correo);

                    //#if DEBUG
                    //                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
                    //#endif

                    //                    var asunto = "Usuario nuevo en el sistema SIGOPLAN";
                    //                    var mensaje = string.Format(@"
                    //                        Se ha creado su usuario para el sistema SIGOPLAN en el módulo de subcontratistas.<br/><br/>
                    //                        Usuario: {0}<br/>
                    //                        Contraseña: {1}<br/><br/>
                    //                        Fecha y hora: {2}", subcontratista.rfc, "Expediente2021", DateTime.Now
                    //                    );

                    //                    var correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, listaCorreos);
                    //                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "guardarNuevoSubcontratista", e, AccionEnum.AGREGAR, 0, new { subcontratista = subcontratista });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarSubcontratista(tblX_SubContratista subcontratista)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (subcontratista.rfc == "" || subcontratista.rfc == null || subcontratista.correo == "" || subcontratista.correo == null)
                    {
                        throw new Exception("Debe capturar el RFC y el correo.");
                    }

                    var subcontratistaSIGOPLAN = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == subcontratista.id);

                    subcontratistaSIGOPLAN.nombre = subcontratista.nombre;
                    subcontratistaSIGOPLAN.direccion = subcontratista.direccion;
                    subcontratistaSIGOPLAN.nombreCorto = subcontratista.nombreCorto;
                    subcontratistaSIGOPLAN.codigoPostal = subcontratista.codigoPostal;
                    subcontratistaSIGOPLAN.rfc = subcontratista.rfc;
                    subcontratistaSIGOPLAN.correo = subcontratista.correo;
                    subcontratistaSIGOPLAN.fisica = subcontratista.fisica;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "editarSubcontratista", e, AccionEnum.ACTUALIZAR, 0, new { subcontratista = subcontratista });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarSubcontratista(tblX_SubContratista subcontratista)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var subcontratistaSIGOPLAN = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == subcontratista.id);

                    subcontratistaSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "eliminarSubcontratista", e, AccionEnum.ELIMINAR, 0, new { subcontratista = subcontratista });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region Contratos
        public Dictionary<string, object> getContratos()
        {
            try
            {
                var listaContratos = _context.tblX_Contrato.Where(x => x.estatus).ToList();

                resultado.Add("data", listaContratos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getContratos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoContrato(tblX_Contrato contrato)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.tblX_Contrato.Add(contrato);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "guardarNuevoContrato", e, AccionEnum.AGREGAR, 0, new { contrato = contrato });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarContrato(tblX_Contrato contrato)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contratoSIGOPLAN = _context.tblX_Contrato.FirstOrDefault(x => x.estatus && x.id == contrato.id);

                    //contratoSIGOPLAN.nombre = contrato.nombre;
                    contratoSIGOPLAN.subcontratistaID = 0;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "editarContrato", e, AccionEnum.ACTUALIZAR, 0, new { contrato = contrato });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarContrato(tblX_Contrato contrato)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var contratoSIGOPLAN = _context.tblX_Contrato.FirstOrDefault(x => x.estatus && x.id == contrato.id);

                    contratoSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "eliminarContrato", e, AccionEnum.ELIMINAR, 0, new { contrato = contrato });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region Proyectos
        public Dictionary<string, object> getProyectos()
        {
            try
            {
                var listaProyectos = _context.tblX_Proyecto.Where(x => x.estatus).ToList();

                resultado.Add("data", listaProyectos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getProyectos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoProyecto(tblX_Proyecto proyecto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.tblX_Proyecto.Add(proyecto);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "guardarNuevoProyecto", e, AccionEnum.AGREGAR, 0, new { proyecto = proyecto });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarProyecto(tblX_Proyecto proyecto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var proyectoSIGOPLAN = _context.tblX_Proyecto.FirstOrDefault(x => x.estatus && x.id == proyecto.id);

                    proyectoSIGOPLAN.nombre = proyecto.nombre;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "editarProyecto", e, AccionEnum.ACTUALIZAR, 0, new { proyecto = proyecto });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarProyecto(tblX_Proyecto proyecto)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var proyectoSIGOPLAN = _context.tblX_Proyecto.FirstOrDefault(x => x.estatus && x.id == proyecto.id);

                    proyectoSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "eliminarProyecto", e, AccionEnum.ELIMINAR, 0, new { proyecto = proyecto });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion

        #region Clientes
        public Dictionary<string, object> getClientes()
        {
            try
            {
                var listaClientes = _context.tblX_Cliente.Where(x => x.estatus).ToList();

                resultado.Add("data", listaClientes);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getClientes", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarNuevoCliente(tblX_Cliente cliente)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.tblX_Cliente.Add(cliente);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "guardarNuevoCliente", e, AccionEnum.AGREGAR, 0, new { cliente = cliente });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> editarCliente(tblX_Cliente cliente)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var clienteSIGOPLAN = _context.tblX_Cliente.FirstOrDefault(x => x.estatus && x.id == cliente.id);

                    clienteSIGOPLAN.nombre = cliente.nombre;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "editarCliente", e, AccionEnum.ACTUALIZAR, 0, new { cliente = cliente });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> eliminarCliente(tblX_Cliente cliente)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var clienteSIGOPLAN = _context.tblX_Cliente.FirstOrDefault(x => x.estatus && x.id == cliente.id);

                    clienteSIGOPLAN.estatus = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "SubcontratistasController", "eliminarCliente", e, AccionEnum.ELIMINAR, 0, new { cliente = cliente });
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }
        #endregion
        #endregion

        public Dictionary<string, object> getSubcontratistasArchivos(int filtroCarga)
        {
            try
            {
                var listaSubcontratistas = _context.tblX_SubContratista.Where(x => x.estatus).ToList();
                var listaArchivosFijos = _context.tblX_DocumentacionFija.Where(x => x.estatus).ToList();
                var data = new List<SubcontratistaDTO>();

                foreach (var subcontratista in listaSubcontratistas)
                {
                    var listaArchivos = _context.tblX_RelacionSubContratistaDocumentacion.Where(x => x.estatus && x.subContratistaID == subcontratista.id).ToList();

                    data.Add(new SubcontratistaDTO
                    {
                        id = subcontratista.id,
                        numeroProveedor = subcontratista.numeroProveedor,
                        nombre = subcontratista.nombre,
                        direccion = subcontratista.direccion,
                        nombreCorto = subcontratista.nombreCorto,
                        codigoPostal = subcontratista.codigoPostal,
                        rfc = subcontratista.rfc,
                        correo = subcontratista.correo,
                        fisica = subcontratista.fisica,
                        pendienteValidacion = subcontratista.pendienteValidacion,
                        tieneHistorial = listaArchivos.Where(x => x.validacion == ArchivoValidacionEnum.rechazado).Count() > 0,
                        listaArchivos = listaArchivos,

                        actaConstitutivaID = listaArchivos.Where(x => x.documentacionID == 1).Select(y => y.id).LastOrDefault(),
                        poderRLID = listaArchivos.Where(x => x.documentacionID == 2).Select(y => y.id).LastOrDefault(),
                        INEID = listaArchivos.Where(x => x.documentacionID == 3).Select(y => y.id).LastOrDefault(),
                        cedulaFiscalID = listaArchivos.Where(x => x.documentacionID == 4).Select(y => y.id).LastOrDefault(),
                        registroPatronalID = listaArchivos.Where(x => x.documentacionID == 5).Select(y => y.id).LastOrDefault(),
                        objetoSocialVigenteID = listaArchivos.Where(x => x.documentacionID == 6).Select(y => y.id).LastOrDefault(),
                        registroEspecializacionID = listaArchivos.Where(x => x.documentacionID == 7).Select(y => y.id).LastOrDefault(),
                        comprobanteDomicilioID = listaArchivos.Where(x => x.documentacionID == 8).Select(y => y.id).LastOrDefault(),

                        rutaActaConstitutiva = listaArchivos.Where(x => x.documentacionID == 1).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaPoderRL = listaArchivos.Where(x => x.documentacionID == 2).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaINE = listaArchivos.Where(x => x.documentacionID == 3).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaCedulaFiscal = listaArchivos.Where(x => x.documentacionID == 4).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaRegistroPatronal = listaArchivos.Where(x => x.documentacionID == 5).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaObjetoSocialVigente = listaArchivos.Where(x => x.documentacionID == 6).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaRegistroEspecializacion = listaArchivos.Where(x => x.documentacionID == 7).Select(y => y.rutaArchivo).LastOrDefault(),
                        rutaComprobanteDomicilio = listaArchivos.Where(x => x.documentacionID == 8).Select(y => y.rutaArchivo).LastOrDefault(),

                        validadoActaConstitutiva = listaArchivos.LastOrDefault(x => x.documentacionID == 1) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 1).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 1 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoPoderRL = listaArchivos.LastOrDefault(x => x.documentacionID == 2) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 2).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 2 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoINE = listaArchivos.LastOrDefault(x => x.documentacionID == 3) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 3).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 3 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoCedulaFiscal = listaArchivos.LastOrDefault(x => x.documentacionID == 4) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 4).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 4 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoRegistroPatronal = listaArchivos.LastOrDefault(x => x.documentacionID == 5) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 5).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 5 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoObjetoSocialVigente = listaArchivos.LastOrDefault(x => x.documentacionID == 6) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 6).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 6 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoRegistroEspecializacion = listaArchivos.LastOrDefault(x => x.documentacionID == 7) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 7).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 7 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,
                        validadoComprobanteDomicilio = listaArchivos.LastOrDefault(x => x.documentacionID == 8) != null ? listaArchivos.LastOrDefault(x => x.documentacionID == 8).validacion == ArchivoValidacionEnum.validado : false, //listaArchivos.Where(x => x.documentacionID == 8 && x.validacion == ArchivoValidacionEnum.validado).Count() > 0,

                        aplicaActaConstitutiva = listaArchivos.Where(x => x.documentacionID == 1 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaPoderRL = listaArchivos.Where(x => x.documentacionID == 2 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaINE = listaArchivos.Where(x => x.documentacionID == 3 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaCedulaFiscal = listaArchivos.Where(x => x.documentacionID == 4 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaRegistroPatronal = listaArchivos.Where(x => x.documentacionID == 5 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaObjetoSocialVigente = listaArchivos.Where(x => x.documentacionID == 6 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaRegistroEspecializacion = listaArchivos.Where(x => x.documentacionID == 7 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault(),
                        aplicaComprobanteDomicilio = listaArchivos.Where(x => x.documentacionID == 8 && x.validacion == ArchivoValidacionEnum.validado).Select(y => y.aplica).LastOrDefault()
                    });
                }

                switch (filtroCarga)
                {
                    case 1: //Carga Parcial
                        data = data.Where(x => x.listaArchivos.Count() > 0 && x.listaArchivos.Count() < 8).ToList();
                        break;
                    case 2: //Carga Completa
                        data = data.Where(x => x.listaArchivos.Count() == 8).ToList();
                        break;
                    case 3: //Completamente validados
                        data = data.Where(x => x.listaArchivos.Where(y => y.validacion == ArchivoValidacionEnum.validado).Count() == 8).ToList();
                        break;
                    default: //Todos
                        break;
                }

                resultado.Add("data", data);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getSubcontratistasArchivos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getSubcontratistaByID(int id)
        {
            try
            {
                var subcontratista = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == id);

                if (subcontratista != null)
                {
                    resultado.Add("data", subcontratista);
                }
                else
                {
                    throw new Exception("No se encuentra la información del subcontratista.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getSubcontratistas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarArchivoSubcontratista(HttpPostedFileBase archivo, int documentacionID, string justificacion, DateTime? fechaVencimiento)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                tblX_SubContratista subcontratista = new tblX_SubContratista();
                try
                {
                    #region Guardar Evidencias
                    //List<tblX_DocumentacionFija> documentos = new List<tblX_DocumentacionFija>();
                    int id = vSesiones.sesionUsuarioDTO.id;
                    //Default id para pruebas
                    id = 2;

                    subcontratista = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == id);
                    var pendienteValidacion = subcontratista.pendienteValidacion;

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                    string rutaArchivoEvidencia = "";

                    var documentacionFija = _context.tblX_DocumentacionFija.FirstOrDefault(x => x.id == documentacionID);
                    if (archivo != null || !documentacionFija.opcional)
                    {

                        string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseArchivoSubcontratistas + "_" + subcontratista.id + "_" + documentacionID, archivo.FileName);
                        rutaArchivoEvidencia = Path.Combine(RutaArchivos, nombreArchivoEvidencia);
                        listaRutaArchivos.Add(Tuple.Create(archivo, rutaArchivoEvidencia));
                    }
                    var listaArchivosAnteriores = _context.tblX_RelacionSubContratistaDocumentacion.Where(x =>
                        x.estatus && x.subContratistaID == subcontratista.id && x.documentacionID == documentacionID
                    ).ToList();

                    foreach (var ant in listaArchivosAnteriores)
                    {
                        ant.estatus = false;
                        _context.SaveChanges();
                    }

                    var registroSIGOPLAN = new tblX_RelacionSubContratistaDocumentacion
                    {
                        subContratistaID = subcontratista.id,
                        documentacionID = documentacionID,
                        rutaArchivo = rutaArchivoEvidencia,
                        fechaVencimiento = documentacionFija.aplicaFechaVencimiento ? fechaVencimiento : null,
                        estatus = true,
                        validacion = ArchivoValidacionEnum.pendiente,
                        justificacionOpcional = justificacion,
                        justificacionValidacion = "",
                        fechaCaptura = DateTime.Now,
                        fechaValidacion = null,
                        aplica = (archivo == null && documentacionFija.opcional) ? false : true,

                    };

                    subcontratista.pendienteValidacion = true;

                    _context.tblX_RelacionSubContratistaDocumentacion.Add(registroSIGOPLAN);
                    _context.SaveChanges();

                    if (archivo != null || !documentacionFija.opcional)
                    {
                        foreach (var arc in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(arc.Item1, arc.Item2) == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                        }
                    }
                    #endregion

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);
                    if (!pendienteValidacion)
                    {
//                        #region Enviar correo de guardado
//                        var listaCorreos = new List<string>();
//                        listaCorreos.Add("andrea.felix@construplan.com.mx");
//#if DEBUG
//                        listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
//#endif
//                        var asunto = "Archivo Cargado SIGOPLAN";
//                        var mensaje = string.Format(@"Se ha cargado un archivo del subcontratista {0}.", subcontratista.nombreCorto);
//                        var correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, listaCorreos);
//                        #endregion
                    }

                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "SubcontratistasController", "guardarArchivoSubcontratista", e, AccionEnum.AGREGAR, 0, new { subcontratista = subcontratista });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarArchivoEditadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var registroArchivo = _context.tblX_RelacionSubContratistaDocumentacion.FirstOrDefault(x => x.estatus && x.id == archivoCargadoID);

                    if (registroArchivo != null)
                    {
                        #region Guardar Evidencias
                        int id = vSesiones.sesionUsuarioDTO.id;
                        id = 2;

                        var subcontratista = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == id);

                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        string rutaArchivoEvidencia = "";

                        if (archivo != null)
                        {
                            string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseArchivoSubcontratistas + "_" + subcontratista.id + "_" + registroArchivo.documentacionID, archivo.FileName);
                            rutaArchivoEvidencia = Path.Combine(RutaArchivos, nombreArchivoEvidencia);
                            listaRutaArchivos.Add(Tuple.Create(archivo, rutaArchivoEvidencia));

                            registroArchivo.rutaArchivo = rutaArchivoEvidencia;
                            _context.SaveChanges();
                        }

                        foreach (var arc in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(arc.Item1, arc.Item2) == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                        }
                        #endregion

                        dbContextTransaction.Commit();
                        resultado.Add(SUCCESS, true);

                        if (!subcontratista.pendienteValidacion)
                        {
                            //                    #region Enviar correo de guardado
                            //                        var listaCorreos = new List<string>();
                            //                        listaCorreos.Add("andrea.felix@construplan.com.mx");
                            //#if DEBUG
                            //                        listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
                            //#endif
                            //                        var asunto = "Archivo Cargado SIGOPLAN";
                            //                        var mensaje = string.Format(@"Se ha cargado un archivo del subcontratista {0}.", subcontratista.nombreCorto);
                            //                        var correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, listaCorreos);
                            //                    #endregion
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del archivo cargado.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "SubcontratistasController", "guardarArchivoEditadoSubcontratista", e, AccionEnum.AGREGAR, 0, new { archivoCargadoID = archivoCargadoID });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> guardarArchivoRenovadoSubcontratista(HttpPostedFileBase archivo, int archivoCargadoID, DateTime? fechaVencimiento)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                tblX_SubContratista subcontratista = new tblX_SubContratista();
                try
                {
                    var registroArchivo = _context.tblX_RelacionSubContratistaDocumentacion.FirstOrDefault(x => x.estatus && x.id == archivoCargadoID);

                    if (registroArchivo != null)
                    {
                        #region Guardar Evidencias
                        int id = vSesiones.sesionUsuarioDTO.id;
                        id = 2;

                        subcontratista = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == id);
                        var pendienteValidacion = subcontratista.pendienteValidacion;

                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        string rutaArchivoEvidencia = "";

                        if (archivo != null)
                        {
                            string nombreArchivoEvidencia = ObtenerFormatoNombreArchivo(NombreBaseArchivoSubcontratistas + "_" + subcontratista.id + "_" + registroArchivo.documentacionID, archivo.FileName);
                            rutaArchivoEvidencia = Path.Combine(RutaArchivos, nombreArchivoEvidencia);
                            listaRutaArchivos.Add(Tuple.Create(archivo, rutaArchivoEvidencia));
                        }

                        var registroSIGOPLAN = new tblX_RelacionSubContratistaDocumentacion
                        {
                            subContratistaID = subcontratista.id,
                            documentacionID = registroArchivo.documentacionID,
                            rutaArchivo = rutaArchivoEvidencia,
                            fechaVencimiento = fechaVencimiento,
                            estatus = true,
                            validacion = ArchivoValidacionEnum.pendiente,
                            justificacionOpcional = "",
                            justificacionValidacion = "",
                            fechaCaptura = DateTime.Now,
                            fechaValidacion = null,
                            aplica = true,

                        };

                        subcontratista.pendienteValidacion = true;

                        _context.tblX_RelacionSubContratistaDocumentacion.Add(registroSIGOPLAN);
                        _context.SaveChanges();

                        foreach (var arc in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(arc.Item1, arc.Item2) == false)
                            {
                                dbContextTransaction.Rollback();
                                resultado.Clear();
                                resultado.Add(SUCCESS, false);
                                resultado.Add(MESSAGE, "Ocurrió un error al guardar los archivos en el servidor.");
                                return resultado;
                            }
                        }
                        #endregion

                        dbContextTransaction.Commit();
                        resultado.Add(SUCCESS, true);

                        if (!pendienteValidacion)
                        {
                            //                    #region Enviar correo de guardado
                            //                        var listaCorreos = new List<string>();
                            //                        listaCorreos.Add("andrea.felix@construplan.com.mx");
                            //#if DEBUG
                            //                        listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
                            //#endif
                            //                        var asunto = "Archivo Cargado SIGOPLAN";
                            //                        var mensaje = string.Format(@"Se ha cargado un archivo del subcontratista {0}.", subcontratista.nombreCorto);
                            //                        var correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, listaCorreos);
                            //                    #endregion
                        }
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del archivo cargado.");
                    }
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "SubcontratistasController", "guardarArchivoRenovadoSubcontratista", e, AccionEnum.AGREGAR, 0, new { archivoCargadoID = archivoCargadoID });
                }
            }

            return resultado;
        }

        private string ObtenerFormatoNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0} {1}{2}", nombreBase, DateTime.Now.ToString("dd-MM-y HH:mm:ss").Replace(":", "-"), Path.GetExtension(fileName));
        }

        public tblX_RelacionSubContratistaDocumentacion getArchivoSubcontratista(int id)
        {
            return _context.tblX_RelacionSubContratistaDocumentacion.FirstOrDefault(x => x.id == id);
        }

        public Dictionary<string, object> getProveedor(int numeroProveedor)
        {
            try
            {
                var proveedorEnkontrol = _contextEnkontrol.Select<SubcontratistaDTO>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO()
                {
                    consulta = @"SELECT numpro AS numeroProveedor, nombre, direccion, nomcorto AS nombreCorto, cp AS codigoPostal, rfc, email AS correo FROM sp_proveedores WHERE numpro = ?",
                    parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "numpro", tipo = OdbcType.Numeric, valor = numeroProveedor } }
                });

                if (proveedorEnkontrol.Count() > 0)
                {
                    resultado.Add("data", proveedorEnkontrol);
                }
                else
                {
                    throw new Exception("No se encuentra la información del subcontratista.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getSubcontratistas", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getDocumentacionPendiente(int subcontratistaID)
        {
            try
            {
                var listaArchivos = (
                    from rel in _context.tblX_RelacionSubContratistaDocumentacion.Where(x => x.estatus)
                    join doc in _context.tblX_DocumentacionFija.Where(x => x.estatus) on rel.documentacionID equals doc.id
                    where rel.subContratistaID == subcontratistaID && rel.validacion == ArchivoValidacionEnum.pendiente
                    select new
                    {
                        id = rel.id,
                        subcontratistaID = rel.subContratistaID,
                        documentacionID = rel.documentacionID,
                        rutaArchivo = rel.rutaArchivo,
                        justificacionValidacion = rel.justificacionValidacion,
                        validacion = rel.validacion,
                        rutaArchivoValidacion = rel.rutaArchivoValidacion,
                        aplica = rel.aplica,
                        nombreDocumentacionFija = doc.nombre
                    }
                ).ToList();

                resultado.Add("data", listaArchivos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getDocumentacionPendiente", e, AccionEnum.CONSULTA, 0, new { subcontratistaID = subcontratistaID });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getHistorialRechazado(int subcontratistaID)
        {
            try
            {
                var listaArchivos = (
                    from rel in _context.tblX_RelacionSubContratistaDocumentacion.Where(x => x.estatus)
                    join doc in _context.tblX_DocumentacionFija.Where(x => x.estatus) on rel.documentacionID equals doc.id
                    where rel.subContratistaID == subcontratistaID && rel.validacion == ArchivoValidacionEnum.rechazado
                    select new
                    {
                        id = rel.id,
                        subcontratistaID = rel.subContratistaID,
                        documentacionID = rel.documentacionID,
                        rutaArchivo = rel.rutaArchivo,
                        justificacionValidacion = rel.justificacionValidacion,
                        validacion = rel.validacion,
                        rutaArchivoValidacion = rel.rutaArchivoValidacion,
                        aplica = rel.aplica,
                        nombreDocumentacionFija = doc.nombre
                    }
                ).ToList();

                resultado.Add("data", listaArchivos);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getDocumentacionPendiente", e, AccionEnum.CONSULTA, 0, new { subcontratistaID = subcontratistaID });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarValidacion(List<tblX_RelacionSubContratistaDocumentacion> listaValidacion, List<HttpPostedFileBase> archivos)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (listaValidacion.Any(x => x.validacion == ArchivoValidacionEnum.pendiente))
                    {
                        throw new Exception("Debe indicar la validación en todos los documentos.");
                    }

                    var archivosContador = archivos != null ? archivos.Count() : 0;

                    if (listaValidacion.Where(x => x.validacion == ArchivoValidacionEnum.rechazado).Count() != archivosContador)
                    {
                        throw new Exception("La cantidad de archivos rechazados no coincide con la cantidad de archivos de justificación subidos.");
                    }

                    var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();

                    int index = 0;
                    int indexRechazado = 0;
                    foreach (var val in listaValidacion)
                    {
                        var registroArchivo = _context.tblX_RelacionSubContratistaDocumentacion.FirstOrDefault(x => x.estatus && x.id == val.id);

                        if (registroArchivo != null)
                        {
                            if (val.validacion == ArchivoValidacionEnum.rechazado)
                            {
                                string nombreArchivoJustificacion = ObtenerFormatoNombreArchivo("Archivo_Justificacion_" + "_" + val.subContratistaID + "_" + val.id, archivos[indexRechazado].FileName);
                                string rutaArchivoEvidencia = Path.Combine(RutaArchivosJustificacion, nombreArchivoJustificacion);
                                listaRutaArchivos.Add(Tuple.Create(archivos[indexRechazado], rutaArchivoEvidencia));

                                registroArchivo.justificacionValidacion = val.justificacionValidacion;
                                registroArchivo.validacion = ArchivoValidacionEnum.rechazado;
                                registroArchivo.rutaArchivoValidacion = rutaArchivoEvidencia;
                                registroArchivo.fechaValidacion = DateTime.Now;

                                indexRechazado++;
                            }
                            else
                            {
                                registroArchivo.validacion = ArchivoValidacionEnum.validado;
                                registroArchivo.fechaValidacion = DateTime.Now;
                            }
                        }
                        else
                        {
                            throw new Exception("No se encuentra la información del registro.");
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

                    var subcontratistaID = listaValidacion[0].subContratistaID;
                    var subcontratistaSIGOPLAN = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == subcontratistaID);

                    if (subcontratistaSIGOPLAN != null)
                    {
                        subcontratistaSIGOPLAN.pendienteValidacion = false;
                        _context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("No se encuentra la información del subcontratista.");
                    }

                    dbContextTransaction.Commit();
                    resultado.Add(SUCCESS, true);

                    //                    #region Enviar correo de validación
                    //                    var listaCorreos = new List<string>();

                    //                    listaCorreos.Add(subcontratistaSIGOPLAN.correo);

                    //#if DEBUG
                    //                    listaCorreos = new List<string> { "oscar.valencia@construplan.com.mx" };
                    //#endif

                    //                    var asunto = "Archivos Validados SIGOPLAN";
                    //                    var mensaje = string.Format(@"
                    //                        Se han validado los archivos cargados en el sistema de SIGOPLAN.<br/>
                    //                        Fecha y hora: {0}", DateTime.Now
                    //                    );

                    //                    var correoEnviado = GlobalUtils.sendEmail(asunto, mensaje, listaCorreos);
                    //                    #endregion
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                    LogError(0, 0, "SubcontratistasController", "guardarValidacion", e, AccionEnum.AGREGAR, 0, new { listaValidacion = listaValidacion });
                }
            }

            return resultado;
        }

        public Dictionary<string, object> getJustificacionOpcional(int archivoID)
        {
            try
            {
                var registroArchivo = _context.tblX_RelacionSubContratistaDocumentacion.FirstOrDefault(x => x.estatus && x.id == archivoID);

                if (registroArchivo == null)
                {
                    throw new Exception("No se encuentra la información del archivo.");
                }

                resultado.Add("data", registroArchivo.justificacionOpcional);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "getJustificacionOpcional", e, AccionEnum.CONSULTA, 0, new { archivoID = archivoID });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> CargarArchivosFijos()
        {
            try
            {
                //List<tblX_DocumentacionFija> documentos = new List<tblX_DocumentacionFija>();
                int id = vSesiones.sesionUsuarioDTO.id;
                //Default id para pruebas
                id = 2;

                var subcontratista = _context.tblX_SubContratista.FirstOrDefault(x => x.estatus && x.id == id);

                if (subcontratista != null)
                {
                    List<tblX_DocumentacionFija> auxDocumentos = _context.tblX_DocumentacionFija.Where(x => (subcontratista.fisica ? x.fisica : true) && x.estatus).ToList();
                    List<int> auxDocumentosID = auxDocumentos.Select(x => x.id).ToList();
                    List<tblX_RelacionSubContratistaDocumentacion> relacionDocumentos = _context.tblX_RelacionSubContratistaDocumentacion.Where(x =>
                        x.subContratistaID == subcontratista.id && auxDocumentosID.Contains(x.documentacionID) && x.estatus
                    ).OrderBy(x => x.id).ToList();
                    List<DocumentoSubcontratistaDTO> documentos = auxDocumentos.Select(x =>
                    {
                        var relacion = relacionDocumentos.LastOrDefault(y => y.documentacionID == x.id);
                        var tipoDocumento = x.fisica ? 1 : 2;
                        return new DocumentoSubcontratistaDTO
                        {
                            id = x.id,
                            clave = x.clave,
                            nombre = x.nombre,
                            aplicaFechaVencimiento = x.aplicaFechaVencimiento,
                            mesesNotificacion = x.mesesNotificacion,
                            tipo = tipoDocumento,
                            validacion = relacion == null ? 2 : (int)relacion.validacion,
                            opcional = x.opcional,
                            archivoCargadoID = relacion != null ? relacion.id : 0
                            //aplica = relacion == null ? true : (relacion.)
                        };
                    }).ToList();
                    resultado.Add("documentos", documentos);
                }
                else
                {
                    throw new Exception("No se encuentra la información del subcontratista.");
                }
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "SubcontratistasController", "CargarArchivosFijos", e, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}
