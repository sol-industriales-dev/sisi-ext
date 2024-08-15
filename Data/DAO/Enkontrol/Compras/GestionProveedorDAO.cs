using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Utils;
using Core.Entity.Enkontrol.Compras;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Usuarios;
using Core.Entity.StarSoft;
using Core.Entity.StarSoft.OrdenCompra;
using Core.Entity.StarSoft.Requisiciones;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Dapper;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.EntityFramework.Mapping.StarSoft;
using Data.Factory.Enkontrol.Compras;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Data.DAO.Enkontrol.Compras
{
    public class GestionProveedorDAO : GenericDAO<tblCom_MAEPROV>, IGestionProveedorDAO
    {
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        GestionProveedorFactoryService ufs = new GestionProveedorFactoryService();
        private readonly string RutaBase = @"\\REPOSITORIO\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORES\PERU";
        private const string RutaLocal = @"C:\Proyecto\SIGOPLAN\COMPRAS\PROVEEDORES\PERU";
        private const int _SISTEMA = (int)SistemasEnum.COMPRAS;
        private const string _NOMBRE_CONTROLADOR = "GestionProveedorController";
        private const int _OMAR_NUNEZ_ID = 7939;
        private const string _OMAR_NUNEZ_CORREO = "omar.nunez@construplan.com.mx"; 

        public Dictionary<string, object> getProveedores()
        {
            try
            {
                List<tblCom_MAEPROVDTO> DatosMAEPROVDTO = new List<tblCom_MAEPROVDTO>();


                var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).ToList();
                var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).ToList();
                List<tblCom_MAEPROV> listaProveedores = _context.tblCom_MAEPROV.Where(x => x.registroActivo).ToList();

                foreach (var item in listaProveedores)
                {
                    var listaProveedoresDTO = new tblCom_MAEPROVDTO();
                    listaProveedoresDTO.id = item.id;
                    listaProveedoresDTO.PRVCCODIGO = item.PRVCCODIGO.Trim();
                    listaProveedoresDTO.PRVCNOMBRE = item.PRVCNOMBRE;
                    listaProveedoresDTO.PRVCESTADO = item.PRVCESTADO;
                    listaProveedoresDTO.PRVPAGO = item.PRVPAGO;
                    listaProveedoresDTO.PRVCDIRECC = item.PRVCDIRECC;
                    listaProveedoresDTO.PRVCLOCALI = item.PRVCLOCALI;
                    listaProveedoresDTO.PRVCTELEF1 = item.PRVCTELEF1;
                    listaProveedoresDTO.PRVCRUC = item.PRVCRUC;
                    listaProveedoresDTO.PRVCDOCIDEN = item.PRVCDOCIDEN;

                    listaProveedoresDTO.PRVDFECCRE = item.PRVDFECCRE;
                    listaProveedoresDTO.PRVCTIPO_DOCUMENTO = item.PRVCTIPO_DOCUMENTO;
                    listaProveedoresDTO.PRVCAPELLIDO_PATERNO = item.PRVCAPELLIDO_PATERNO;
                    listaProveedoresDTO.PRVCAPELLIDO_MATERNO = item.PRVCAPELLIDO_MATERNO;
                    listaProveedoresDTO.PRVCPRIMER_NOMBRE = item.PRVCPRIMER_NOMBRE;
                    listaProveedoresDTO.PRVCSEGUNDO_NOMBRE = item.PRVCSEGUNDO_NOMBRE;
                    listaProveedoresDTO.FEC_INACTIVO_BLOQUEADO = item.FEC_INACTIVO_BLOQUEADO;
                    listaProveedoresDTO.COD_AUDITORIA = item.COD_AUDITORIA;
                    listaProveedoresDTO.UBIGEO = item.UBIGEO;
                    listaProveedoresDTO.FLGPORTAL_PROVEEDOR = item.FLGPORTAL_PROVEEDOR;
                    listaProveedoresDTO.PRVCCODIGO = item.PRVCCODIGO;
                    listaProveedoresDTO.statusAutorizacion = item.statusAutorizacion;

                    if (objAutorizador.Count() > 0 && item.Vobo)
                    {
                        listaProveedoresDTO.PuedeAutorizar = true;
                    }
                    else
                    {
                        listaProveedoresDTO.PuedeAutorizar = false;
                    }

                    if (objVobo.Count() > 0 && !item.Vobo)
                    {
                        listaProveedoresDTO.PuedeVobo = true;
                    }
                    else
                    {
                        listaProveedoresDTO.PuedeVobo = false;
                    }

                    if (item.Autorizado == true)
                    {
                        listaProveedoresDTO.PuedeAutorizar = false;
                    }

                    if (item.Vobo == true)
                    {
                        listaProveedoresDTO.PuedeVobo = false;
                    }
                    DatosMAEPROVDTO.Add(listaProveedoresDTO);
                }

                resultado.Add(ITEMS, DatosMAEPROVDTO);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getDatosProveedores(int id)
        {
            try
            {
                var listaProveedores = _context.tblCom_MAEPROV.Where(x => x.registroActivo && x.id == id).FirstOrDefault();
                var lstArchivosEK = _context.tblCom_ArchivosAdjuntosProveedores.Where(x => x.registroActivo && x.FK_idProv == listaProveedores.id).ToList();

                resultado.Add(ITEMS, listaProveedores);
                resultado.Add("lstArchivos", lstArchivosEK);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarEditarProveedor(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    using (var dbStarsoftTransaction = _starsoft.Database.BeginTransaction())
                    {
                        using (var _starsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                        {
                            using (var dbStarsoftTransactionCont = _starsoftConta.Database.BeginTransaction())
                            {
                                List<string> lstArchivos = new List<string>();
                                try
                                {
                                    tblCom_MAEPROV objProveedor = _context.tblCom_MAEPROV.Where(w => w.registroActivo && w.id == proveedor.id).FirstOrDefault();
                                    tblP_Usuario objUsuarioNombreUsuario = _context.tblP_Usuario.Where(w => w.id == vSesiones.sesionUsuarioDTO.id).FirstOrDefault();
                                    List<tblCom_AutorizarProveedor> objAutorizadorNot = _context.tblCom_AutorizarProveedor.Where(w => w.registroActivo && w.PuedeAutorizar == true).ToList();
                                    List<tblCom_AutorizarProveedor> objVoboNot = _context.tblCom_AutorizarProveedor.Where(w => w.registroActivo && w.PuedeVobo == true).ToList();
                                    tblP_Usuario_Starsoft objUsuarioStarSoft = _context.tblP_Usuario_Starsoft.Where(w => w.sigoplan_usuario_id == vSesiones.sesionUsuarioDTO.id).FirstOrDefault();
                                    string usuarioID = string.Empty;
                                    if (objUsuarioStarSoft == null)
                                        usuarioID = string.Empty;
                                    else
                                        usuarioID = objUsuarioStarSoft.starsoft_usuario_id;

                                    if (objProveedor != null)
                                    {
                                        objProveedor.PRVCCODIGO = proveedor.PRVCCODIGO;
                                        objProveedor.PRVCNOMBRE = proveedor.PRVCNOMBRE;
                                        objProveedor.PRVCESTADO = proveedor.PRVCESTADO;
                                        objProveedor.PRVPAGO = proveedor.PRVPAGO;
                                        objProveedor.PRVCDIRECC = proveedor.PRVCDIRECC;
                                        objProveedor.PRVCLOCALI = proveedor.PRVCLOCALI;
                                        objProveedor.PRVCTELEF1 = proveedor.PRVCTELEF1;
                                        objProveedor.PRVCRUC = proveedor.PRVCRUC;
                                        objProveedor.PRVCDOCIDEN = proveedor.PRVCDOCIDEN;
                                        objProveedor.PRVCUSER = usuarioID;
                                        objProveedor.PRVDFECCRE = proveedor.PRVDFECCRE;
                                        objProveedor.PRVCTIPO_DOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                                        objProveedor.PRVCAPELLIDO_PATERNO = proveedor.PRVCAPELLIDO_PATERNO;
                                        objProveedor.PRVCAPELLIDO_MATERNO = proveedor.PRVCAPELLIDO_MATERNO;
                                        objProveedor.PRVCPRIMER_NOMBRE = proveedor.PRVCPRIMER_NOMBRE;
                                        objProveedor.PRVCSEGUNDO_NOMBRE = proveedor.PRVCSEGUNDO_NOMBRE;
                                        objProveedor.COD_AUDITORIA = proveedor.COD_AUDITORIA;
                                        objProveedor.UBIGEO = proveedor.UBIGEO;
                                        objProveedor.FLGPORTAL_PROVEEDOR = proveedor.FLGPORTAL_PROVEEDOR;
                                        objProveedor.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        objProveedor.fechaModificacion = DateTime.Now;

                                        #region revisa si hay registros proveedores en starsoft
                                        MAEPROV objProveedorStarsoft = _starsoft.MAEPROV.Where(e => e.PRVCCODIGO == proveedor.PRVCCODIGO).FirstOrDefault();
                                        if (objProveedorStarsoft != null)
                                        {
                                            objProveedorStarsoft.PRVCCODIGO = proveedor.PRVCCODIGO;
                                            objProveedorStarsoft.PRVCNOMBRE = proveedor.PRVCNOMBRE;
                                            objProveedorStarsoft.PRVCESTADO = proveedor.PRVCESTADO;
                                            objProveedorStarsoft.PRVPAGO = proveedor.PRVPAGO;
                                            objProveedorStarsoft.PRVCDIRECC = proveedor.PRVCDIRECC;
                                            objProveedorStarsoft.PRVCLOCALI = proveedor.PRVCLOCALI;
                                            objProveedorStarsoft.PRVCTELEF1 = proveedor.PRVCTELEF1;
                                            objProveedorStarsoft.PRVCRUC = proveedor.PRVCRUC;
                                            objProveedorStarsoft.PRVCDOCIDEN = proveedor.PRVCDOCIDEN;
                                            objProveedorStarsoft.PRVCUSER = usuarioID;
                                            objProveedorStarsoft.PRVDFECCRE = proveedor.PRVDFECCRE;
                                            objProveedorStarsoft.PRVCTIPO_DOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                                            objProveedorStarsoft.PRVCAPELLIDO_PATERNO = proveedor.PRVCAPELLIDO_PATERNO;
                                            objProveedorStarsoft.PRVCAPELLIDO_MATERNO = proveedor.PRVCAPELLIDO_MATERNO;
                                            objProveedorStarsoft.PRVCPRIMER_NOMBRE = proveedor.PRVCPRIMER_NOMBRE;
                                            objProveedorStarsoft.PRVCSEGUNDO_NOMBRE = proveedor.PRVCSEGUNDO_NOMBRE;
                                            objProveedorStarsoft.COD_AUDITORIA = proveedor.COD_AUDITORIA;
                                            objProveedorStarsoft.UBIGEO = proveedor.UBIGEO;
                                            objProveedorStarsoft.FLGPORTAL_PROVEEDOR = proveedor.FLGPORTAL_PROVEEDOR;
                                            _starsoft.SaveChanges();
                                        }
                                        #endregion

                                        #region revisa si hay registro en la tabla anexo de starsoft
                                        ANEXO objAnexoStarsoft = _starsoftConta.ANEXO.Where(w => w.ANEX_CODIGO == proveedor.PRVCCODIGO).FirstOrDefault();
                                        if (objAnexoStarsoft != null)
                                        {
                                            objAnexoStarsoft.ANEX_CODIGO = proveedor.PRVCCODIGO;
                                            objAnexoStarsoft.TIPOANEX_CODIGO = "03";
                                            objAnexoStarsoft.ANEX_RUC = proveedor.PRVCCODIGO;
                                            objAnexoStarsoft.ANEX_DESCRIPCION = proveedor.PRVCNOMBRE;
                                            objAnexoStarsoft.ANEX_REFERENCIA = string.Empty;
                                            objAnexoStarsoft.ANEX_DIRECCION = proveedor.PRVCDIRECC;
                                            objAnexoStarsoft.ANEX_TELEFONO = proveedor.PRVCTELEF1;
                                            objAnexoStarsoft.ANEX_REPRESENTANTE = proveedor.PRVREPRESENTANTE;
                                            objAnexoStarsoft.ANEX_GIRO = string.Empty;
                                            objAnexoStarsoft.NRETENCION = false;
                                            objAnexoStarsoft.ANEX_NOMBRE = string.Format("{0} {1}", proveedor.PRVCPRIMER_NOMBRE, proveedor.PRVCSEGUNDO_NOMBRE);
                                            objAnexoStarsoft.ANEX_APE_PAT = proveedor.PRVCAPELLIDO_PATERNO;
                                            objAnexoStarsoft.ANEX_APE_MAT = proveedor.PRVCAPELLIDO_MATERNO;
                                            objAnexoStarsoft.TIPOPERSONA = string.Empty;
                                            objAnexoStarsoft.TIPODOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                                            objAnexoStarsoft.DOCUMENTOIDENTIDAD = proveedor.PRVCDOCIDEN;
                                            objAnexoStarsoft.ANE_DETRACCION = false;
                                            objAnexoStarsoft.ANE_TASA_DETRACC = 0;
                                            objAnexoStarsoft.COD_DETRAC = string.Empty;
                                            objAnexoStarsoft.ANEX_NOMBRE_2 = string.Empty;
                                            objAnexoStarsoft.ANEX_NACIONALIDAD = string.Empty;
                                            objAnexoStarsoft.ANEX_SEXO = false;
                                            objAnexoStarsoft.TCL_CODIGO = string.Empty;
                                            objAnexoStarsoft.ANEX_GLOSA = string.Empty;
                                            objAnexoStarsoft.ANEX_EST_CODIGO = string.Empty;
                                            objAnexoStarsoft.ANEX_COND_CODIGO = string.Empty;
                                            objAnexoStarsoft.COD_AFP_ONP = string.Empty;
                                            objAnexoStarsoft.ANEX_TIPO_COMISION = string.Empty;
                                            objAnexoStarsoft.COD_DOBLE_TRIB = string.Empty;
                                            objAnexoStarsoft.ANEX_DISTRITO = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_CTA = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_METODOENRUTA = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_CODIGOENRUTA = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_NOMBRE = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_DIRECCION1 = string.Empty;
                                            objAnexoStarsoft.ANEX_BENE_DIRECCION2 = string.Empty;
                                            objAnexoStarsoft.ANEX_INTERME_METODOENRUTA = string.Empty;
                                            objAnexoStarsoft.ANEX_INTERME_CODIGOENRUTA = string.Empty;
                                            objAnexoStarsoft.ANEX_INTERME_NOMBRE = string.Empty;
                                            objAnexoStarsoft.ANEX_INTERME_DIRECCION1 = string.Empty;
                                            objAnexoStarsoft.ANEX_INTERME_DIRECCION2 = string.Empty;
                                            objAnexoStarsoft.ANEX_CORREO = string.Empty;
                                            objAnexoStarsoft.ANEX_UBIGEO = proveedor.UBIGEO;
                                            objAnexoStarsoft.TIPO_PROVEEDOR = string.Empty;
                                            objAnexoStarsoft.ANEX_PORTAL_PROVEEDOR = false;
                                            _starsoftConta.SaveChanges();
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        proveedor.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        proveedor.fechaCreacion = DateTime.Now;
                                        proveedor.registroActivo = true;
                                        proveedor.UBIGEO = proveedor.UBIGEO != null ? proveedor.UBIGEO : string.Empty;
                                        proveedor.PRVEMAILREP = proveedor.PRVEMAILREP != null ? proveedor.PRVEMAILREP : string.Empty;
                                        _context.tblCom_MAEPROV.Add(proveedor);
                                        _context.SaveChanges();

                                        #region ARCHIVOS
                                        if (objFile != null)
                                        {
                                            #region SE REGISTRA EL ARCHIVO ADJUNTO
                                            var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                            var CarpetaNueva = Path.Combine(RutaLocal, proveedor.PRVCCODIGO);
#else
                                            var CarpetaNueva = Path.Combine(RutaBase, proveedor.PRVCCODIGO);
#endif
                                            // Verifica si existe la carpeta y si no, la crea.
                                            if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                                                throw new Exception("No se pudo crear la carpeta en el servidor.");

                                            string nombreArchivo = SetNombreArchivo("EvidenciaProveedor", objFile.FileName);
                                            string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                            listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                                            // GUARDAR TABLA ARCHIVOS
                                            tblCom_ArchivosAdjuntosProveedores objEvidencia = new tblCom_ArchivosAdjuntosProveedores()
                                            {
                                                FK_idProv = proveedor.id,
                                                FK_numpro = 0,
                                                nombreArchivo = nombreArchivo,
                                                rutaArchivo = rutaArchivo,
                                                FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                                fechaCreacion = DateTime.Now,
                                                registroActivo = true
                                            };
                                            _context.tblCom_ArchivosAdjuntosProveedores.Add(objEvidencia);
                                            _context.SaveChanges();

                                            lstArchivos.Add(objEvidencia.rutaArchivo);

                                            if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                                                throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                                            #endregion
                                        }
                                        else
                                            throw new Exception("Favor de cargar al menos un archivo");
                                        #endregion

                                        List<tblCom_AutorizarProveedor> objVobo = _context.tblCom_AutorizarProveedor.Where(w => w.registroActivo && w.PuedeVobo == true).ToList();
                                        foreach (var item in objVobo)
                                        {
                                            #region Alerta SIGOPLAN
                                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                            objNuevaAlerta.userRecibeID = item.idUsuario;
#if DEBUG
                                            objNuevaAlerta.userRecibeID = _OMAR_NUNEZ_ID;
#endif
                                            objNuevaAlerta.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                                            objNuevaAlerta.sistemaID = _SISTEMA;
                                            objNuevaAlerta.visto = false;
                                            objNuevaAlerta.url = string.Format("/Enkontrol/GestionProveedor/GestionProveedores?id={0}", proveedor.PRVCCODIGO);
                                            objNuevaAlerta.objID = proveedor.id != null ? proveedor.id : 0;
                                            objNuevaAlerta.obj = "AltaProveedor";
                                            objNuevaAlerta.msj = string.Format("Alta Proveedor - {0}", proveedor.PRVCCODIGO);
                                            objNuevaAlerta.documentoID = 0;
                                            objNuevaAlerta.moduloID = 0;
                                            _context.tblP_Alerta.Add(objNuevaAlerta);
                                            _context.SaveChanges();
                                            #endregion
                                        }

                                        int idPro = proveedor.id;
                                        foreach (var item in objVoboNot)
                                        {
                                            tblP_Usuario objUsuarioAlta = _context.tblP_Usuario.Where(w => w.id == item.idUsuario).FirstOrDefault();
                                            if (objUsuarioAlta == null)
                                                throw new Exception("No se encuentra el correo del creador del alta.");

                                            if (string.IsNullOrEmpty(objUsuarioAlta.correo))
                                                throw new Exception("No se encuentra el correo al autorizante.");

                                            List<string> lstCorreos = new List<string>();
                                            lstCorreos.Add(objUsuarioAlta.correo);
#if DEBUG
                                            lstCorreos = new List<string> { _OMAR_NUNEZ_CORREO };
#endif
                                            string subject = "Registro Alta de proveedor";
                                            string body = string.Format(@"Buen dia,<br>Se registro el proveedor {0} {1}.<br><br> Por el usuario {2} {3} {4}<br><br>
                                                      {5}<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                                                 proveedor.PRVCCODIGO,
                                                                                 proveedor.PRVCNOMBRE,
                                                                                 objUsuarioNombreUsuario.nombre,
                                                                                 objUsuarioNombreUsuario.apellidoPaterno,
                                                                                 objUsuarioNombreUsuario.apellidoMaterno,
                                                                                 htmlCorreo(idPro));

                                            GlobalUtils.sendEmailAdjunto(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos, lstArchivos);
                                        }
                                    }
                                    _context.SaveChanges();

                                    dbStarsoftTransaction.Commit();
                                    dbContextTransaction.Commit();
                                    dbStarsoftTransactionCont.Commit();

                                    resultado.Add(SUCCESS, true);
                                    resultado.Add(MESSAGE, "Se ha registrado con éxito.");

                                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(proveedor));
                                }
                                catch (Exception e)
                                {
                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, e.Message);

                                    dbStarsoftTransaction.Rollback();
                                    dbContextTransaction.Rollback();
                                    dbStarsoftTransactionCont.Rollback();

                                    var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.AGREGAR, 0, new { proveedor = proveedor, objFile = objFile });
                                }
                            }
                        }
                    }
                }
            }
            return resultado;
        }
        private string SetNombreArchivo(string nombreBase, string fileName)
        {
            return String.Format("{0}{1}{2}", nombreBase, fileName.Split('.')[0], Path.GetExtension(fileName));
        }
        public string htmlCorreo(int id)
        {
            List<InfoNotificacionAutorizadorProvDTO> lstInfoAutorizadores = new List<InfoNotificacionAutorizadorProvDTO>();

            string html = "";
            contextSigoplan db = new contextSigoplan();
            //html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            //html += "border: 1px solid}table.dataTable thead {font-size: 18px;background-color:  white;color: black;}";
            //html += "</style>";
            var lstProveedores = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == id).ToList();



            html += "<style>";
            html += "table, th, td {";
            html += "border: 1px solid grey;";
            html += "border-collapse: collapse;";
            html += "}";
            html += "th {";
            html += "text-align: center;";
            html += "}";
            html += "td {";
            html += "text-align: center;";
            html += "}";
            html += "</style>";


            html += "<br><table style='width:100%'>";
            html += "<thead>";
            html += "<tr>";

            html += "<th>Nombre Autorizador</th>";
            html += "<th>Tipo</th>";
            html += "<th>Autorizó</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            var objAutorizadorPuede = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeAutorizar).ToList();
            var objVoboPuede = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.PuedeVobo).ToList();
            foreach (var item in lstProveedores)
            {

                var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioAutorizo).FirstOrDefault();
                var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == item.id_usuarioVobo).FirstOrDefault();
                //if (objAutorizador != null)
                if (item.Autorizado)
                {
                    InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                    InfoAutorizadores.idUsuario = objAutorizador.idUsuario;
                    InfoAutorizadores.autorizo = "AUTORIZADO";
                    InfoAutorizadores.tipo = "AUTORIZANTE";
                    InfoAutorizadores.color = "#82e0aa";

                    lstInfoAutorizadores.Add(InfoAutorizadores);
                }
                else
                {
                    foreach (var item3 in objAutorizadorPuede)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();
                        InfoAutorizadores.idUsuario = item3.idUsuario;
                        InfoAutorizadores.autorizo = "PENDIENTE";
                        InfoAutorizadores.tipo = "AUTORIZADOR";
                        InfoAutorizadores.color = "#f08024";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }


                if (item.Vobo)
                {
                    InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                    InfoAutorizadores.idUsuario = objVobo.idUsuario;
                    InfoAutorizadores.autorizo = "AUTORIZADO";
                    InfoAutorizadores.tipo = "VOBO";
                    InfoAutorizadores.color = "#82e0aa";

                    lstInfoAutorizadores.Add(InfoAutorizadores);
                }
                else
                {
                    foreach (var item4 in objVoboPuede)
                    {
                        InfoNotificacionAutorizadorProvDTO InfoAutorizadores = new InfoNotificacionAutorizadorProvDTO();

                        InfoAutorizadores.idUsuario = item4.idUsuario;
                        InfoAutorizadores.autorizo = "PENDIENTE";
                        InfoAutorizadores.tipo = "VOBO";
                        InfoAutorizadores.color = "#f08024";
                        lstInfoAutorizadores.Add(InfoAutorizadores);
                    }
                }
            }



            foreach (var item in lstInfoAutorizadores)
            {

                int autorizante = item.idUsuario;

                html += "<tr>";
                html += "<td>" + _context.tblP_Usuario.Where(r => r.id == autorizante).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                html += "<td>" + item.tipo + "</td>";
                html += "<td style='background-color:" + item.color + ";'>" + item.autorizo + "</td>";
                html += "</tr>";

            }

            html += "</tbody>";
            html += "</table>";
            //html += "</div>";

            return html;
        }

        public string htmlCorreoCajaChica(int id)
        {
            List<InfoNotificacionAutorizadorProvDTO> lstInfoAutorizadores = new List<InfoNotificacionAutorizadorProvDTO>();

            string html = "";
            contextSigoplan db = new contextSigoplan();
            //html += "<style>h3 {text-align: center;}table.dataTable tbody tr td, table thead tr th, table.dataTable, .dataTables_scrollBody {";
            //html += "border: 1px solid}table.dataTable thead {font-size: 18px;background-color:  white;color: black;}";
            //html += "</style>";
            //var lstProveedores = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == id).ToList();



            html += "<style>";
            html += "table, th, td {";
            html += "border: 1px solid grey;";
            html += "border-collapse: collapse;";
            html += "}";
            html += "th {";
            html += "text-align: center;";
            html += "}";
            html += "td {";
            html += "text-align: center;";
            html += "}";
            html += "</style>";


            html += "<br><table style='width:100%'>";
            html += "<thead>";
            html += "<tr>";

            html += "<th>Nombre Autorizador</th>";
            html += "<th>Tipo</th>";
            html += "<th>Autorizó</th>";

            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";


            InfoNotificacionAutorizadorProvDTO InfoAutorizadoresAuto = new InfoNotificacionAutorizadorProvDTO();
            InfoAutorizadoresAuto.idUsuario = vSesiones.sesionUsuarioDTO.id;
            InfoAutorizadoresAuto.autorizo = "AUTORIZADO";
            InfoAutorizadoresAuto.tipo = "AUTORIZADOR";
            InfoAutorizadoresAuto.color = "#82e0aa";
            lstInfoAutorizadores.Add(InfoAutorizadoresAuto);

            InfoNotificacionAutorizadorProvDTO InfoAutorizadoresVobo = new InfoNotificacionAutorizadorProvDTO();
            InfoAutorizadoresVobo.idUsuario = vSesiones.sesionUsuarioDTO.id;
            InfoAutorizadoresVobo.autorizo = "AUTORIZADO";
            InfoAutorizadoresVobo.tipo = "VOBO";
            InfoAutorizadoresVobo.color = "#82e0aa";
            lstInfoAutorizadores.Add(InfoAutorizadoresVobo);

            foreach (var item in lstInfoAutorizadores)
            {

                int autorizante = item.idUsuario;

                html += "<tr>";
                html += "<td>" + _context.tblP_Usuario.Where(r => r.id == autorizante).Select(y => y.nombre + " " + y.apellidoPaterno + " " + y.apellidoMaterno).FirstOrDefault() + "</td>";
                html += "<td>" + item.tipo + "</td>";
                html += "<td style='background-color:" + item.color + ";'>" + item.autorizo + "</td>";
                html += "</tr>";

            }

            html += "</tbody>";
            html += "</table>";
            //html += "</div>";

            return html;
        }
        public Dictionary<string, object> GuardadoCajaChica(tblCom_MAEPROV proveedor, HttpPostedFileBase objFile)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    using (var dbStarsoftTransaction = _starsoft.Database.BeginTransaction())
                    {
                        using (var _starsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                        {
                            using (var dbStarsoftTransactionCont = _starsoftConta.Database.BeginTransaction())
                            {
                                List<string> lstArchivos = new List<string>();

                                try
                                {
                                    var objProveedor = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == proveedor.id).FirstOrDefault();

                                    var usuarioStarsof = _context.tblP_Usuario_Starsoft.Where(x => x.sigoplan_usuario_id == vSesiones.sesionUsuarioDTO.id).FirstOrDefault();
                                    var usuarioId = string.Empty;
                                    if (usuarioStarsof == null)
                                    {
                                        usuarioId = string.Empty;
                                    }
                                    else
                                    {
                                        usuarioId = usuarioStarsof.starsoft_usuario_id;
                                    }
                                    if (objProveedor != null)
                                    {
                                        proveedor.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                        proveedor.fechaModificacion = DateTime.Now;
                                    }
                                    else
                                    {
                                        proveedor.PRVCUSER = usuarioId;

                                        proveedor.Autorizado = true;
                                        proveedor.Vobo = true;
                                        proveedor.statusAutorizacion = true;
                                        proveedor.id_usuarioAutorizo = vSesiones.sesionUsuarioDTO.id;
                                        proveedor.fechaAutorizo = DateTime.Now;
                                        proveedor.id_usuarioVobo = vSesiones.sesionUsuarioDTO.id;
                                        proveedor.fechaVobo = DateTime.Now;
                                        proveedor.id_usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                        proveedor.fechaCreacion = DateTime.Now;
                                        _context.tblCom_MAEPROV.Add(proveedor);
                                        proveedor.registroActivo = true;
                                    }
                                    _context.SaveChanges();

                                    #region ARCHIVOS
                                    if (objFile != null)
                                    {
                                        #region SE REGISTRA EL ARCHIVO ADJUNTO
                                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
#if DEBUG
                                        var CarpetaNueva = Path.Combine(RutaLocal, proveedor.PRVCCODIGO);
#else
                                        var CarpetaNueva = Path.Combine(RutaBase, proveedor.PRVCCODIGO);
#endif
                                        // Verifica si existe la carpeta y si no, la crea.
                                        if (GlobalUtils.VerificarExisteCarpeta(CarpetaNueva, true) == false)
                                            throw new Exception("No se pudo crear la carpeta en el servidor.");

                                        string nombreArchivo = SetNombreArchivo("EvidenciaProveedor", objFile.FileName);
                                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                                        listaRutaArchivos.Add(Tuple.Create(objFile, rutaArchivo));

                                        // GUARDAR TABLA ARCHIVOS
                                        tblCom_ArchivosAdjuntosProveedores objEvidencia = new tblCom_ArchivosAdjuntosProveedores()
                                        {
                                            FK_idProv = proveedor.id,
                                            FK_numpro = 0,
                                            nombreArchivo = nombreArchivo,
                                            rutaArchivo = rutaArchivo,
                                            FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id,
                                            fechaCreacion = DateTime.Now,
                                            registroActivo = true
                                        };
                                        _context.tblCom_ArchivosAdjuntosProveedores.Add(objEvidencia);
                                        _context.SaveChanges();

                                        lstArchivos.Add(objEvidencia.rutaArchivo);

                                        if (GlobalUtils.SaveHTTPPostedFile(objFile, rutaArchivo) == false)
                                            throw new Exception("Ocurrió un error al guardar los archivos en el servidor.");
                                        #endregion
                                    }
                                    else
                                        throw new Exception("Favor de cargar al menos un archivo");
                                    #endregion

                                    MAEPROV infoStarsoftProv = new MAEPROV();

                                    infoStarsoftProv.PRVCCODIGO = proveedor.PRVCCODIGO;
                                    infoStarsoftProv.PRVCNOMBRE = proveedor.PRVCNOMBRE;
                                    infoStarsoftProv.PRVCESTADO = proveedor.PRVCESTADO;
                                    infoStarsoftProv.PRVPAGO = proveedor.PRVPAGO;
                                    infoStarsoftProv.PRVCDIRECC = proveedor.PRVCDIRECC;
                                    infoStarsoftProv.PRVCLOCALI = proveedor.PRVCLOCALI;
                                    infoStarsoftProv.PRVCTELEF1 = proveedor.PRVCTELEF1;
                                    infoStarsoftProv.PRVCRUC = proveedor.PRVCRUC;
                                    infoStarsoftProv.PRVCDOCIDEN = proveedor.PRVCDOCIDEN;
                                    infoStarsoftProv.PRVCUSER = usuarioId;
                                    infoStarsoftProv.PRVDFECCRE = proveedor.PRVDFECCRE;
                                    infoStarsoftProv.PRVCTIPO_DOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                                    infoStarsoftProv.PRVCAPELLIDO_PATERNO = proveedor.PRVCAPELLIDO_PATERNO;
                                    infoStarsoftProv.PRVCAPELLIDO_MATERNO = proveedor.PRVCAPELLIDO_MATERNO;
                                    infoStarsoftProv.PRVCPRIMER_NOMBRE = proveedor.PRVCPRIMER_NOMBRE;
                                    infoStarsoftProv.PRVCSEGUNDO_NOMBRE = proveedor.PRVCSEGUNDO_NOMBRE;
                                    //infoStarsoftProv.FEC_INACTIVO_BLOQUEADO = objProveedor.FEC_INACTIVO_BLOQUEADO;
                                    infoStarsoftProv.COD_AUDITORIA = proveedor.COD_AUDITORIA;
                                    infoStarsoftProv.UBIGEO = proveedor.UBIGEO;
                                    infoStarsoftProv.FLGPORTAL_PROVEEDOR = proveedor.FLGPORTAL_PROVEEDOR;


                                    _starsoft.MAEPROV.Add(infoStarsoftProv);
                                    _starsoft.SaveChanges();

                                    ANEXO infoStarsoftAnexo = new ANEXO();

                                    infoStarsoftAnexo.ANEX_CODIGO = proveedor.PRVCCODIGO;
                                    infoStarsoftAnexo.TIPOANEX_CODIGO = "03";
                                    infoStarsoftAnexo.ANEX_RUC = proveedor.PRVCCODIGO;
                                    infoStarsoftAnexo.ANEX_DESCRIPCION = proveedor.PRVCNOMBRE;
                                    infoStarsoftAnexo.ANEX_REFERENCIA = "";
                                    infoStarsoftAnexo.ANEX_DIRECCION = proveedor.PRVCDIRECC;
                                    infoStarsoftAnexo.ANEX_TELEFONO = proveedor.PRVCTELEF1;
                                    infoStarsoftAnexo.ANEX_REPRESENTANTE = proveedor.PRVREPRESENTANTE;
                                    infoStarsoftAnexo.ANEX_GIRO = "";
                                    infoStarsoftAnexo.NRETENCION = false;
                                    infoStarsoftAnexo.ANEX_NOMBRE = proveedor.PRVCPRIMER_NOMBRE + "" + proveedor.PRVCSEGUNDO_NOMBRE;
                                    infoStarsoftAnexo.ANEX_APE_PAT = proveedor.PRVCAPELLIDO_PATERNO;
                                    infoStarsoftAnexo.ANEX_APE_MAT = proveedor.PRVCAPELLIDO_MATERNO;
                                    infoStarsoftAnexo.TIPOPERSONA = "";
                                    infoStarsoftAnexo.TIPODOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                                    infoStarsoftAnexo.DOCUMENTOIDENTIDAD = proveedor.PRVCDOCIDEN;
                                    infoStarsoftAnexo.ANE_DETRACCION = false;
                                    infoStarsoftAnexo.ANE_TASA_DETRACC = 0;
                                    infoStarsoftAnexo.COD_DETRAC = "";
                                    infoStarsoftAnexo.ANEX_NOMBRE_2 = "";
                                    infoStarsoftAnexo.ANEX_NACIONALIDAD = "";
                                    infoStarsoftAnexo.ANEX_SEXO = false;
                                    infoStarsoftAnexo.TCL_CODIGO = "";
                                    infoStarsoftAnexo.ANEX_GLOSA = "";
                                    infoStarsoftAnexo.ANEX_EST_CODIGO = "";
                                    infoStarsoftAnexo.ANEX_COND_CODIGO = "";
                                    infoStarsoftAnexo.COD_AFP_ONP = "";
                                    infoStarsoftAnexo.ANEX_TIPO_COMISION = "";
                                    infoStarsoftAnexo.COD_DOBLE_TRIB = "";
                                    infoStarsoftAnexo.ANEX_DISTRITO = "";
                                    infoStarsoftAnexo.ANEX_BENE_CTA = "";
                                    infoStarsoftAnexo.ANEX_BENE_METODOENRUTA = "";
                                    infoStarsoftAnexo.ANEX_BENE_CODIGOENRUTA = "";
                                    infoStarsoftAnexo.ANEX_BENE_NOMBRE = "";
                                    infoStarsoftAnexo.ANEX_BENE_DIRECCION1 = "";
                                    infoStarsoftAnexo.ANEX_BENE_DIRECCION2 = "";
                                    infoStarsoftAnexo.ANEX_INTERME_METODOENRUTA = "";
                                    infoStarsoftAnexo.ANEX_INTERME_CODIGOENRUTA = "";
                                    infoStarsoftAnexo.ANEX_INTERME_NOMBRE = "";
                                    infoStarsoftAnexo.ANEX_INTERME_DIRECCION1 = "";
                                    infoStarsoftAnexo.ANEX_INTERME_DIRECCION2 = "";
                                    infoStarsoftAnexo.ANEX_CORREO = "";
                                    infoStarsoftAnexo.ANEX_UBIGEO = proveedor.UBIGEO;
                                    infoStarsoftAnexo.TIPO_PROVEEDOR = "";
                                    infoStarsoftAnexo.ANEX_PORTAL_PROVEEDOR = false;

                                    _starsoftConta.ANEXO.Add(infoStarsoftAnexo);
                                    _starsoftConta.SaveChanges();


                                    var objCuentaBancoProveedor = _context.tblCom_CuentasBancosProveedor.Where(e => e.registroActivo && e.ANEXO == "03" + proveedor.PRVCCODIGO).FirstOrDefault();

                                    if (objCuentaBancoProveedor != null)
                                    {
                                        CUENTA_TELE_ANEXO infoCuentaBancoProv = new CUENTA_TELE_ANEXO();

                                        infoCuentaBancoProv.ANEXO = objCuentaBancoProveedor.ANEXO;
                                        infoCuentaBancoProv.BAN_CODIGO = objCuentaBancoProveedor.BAN_CODIGO;
                                        infoCuentaBancoProv.CTABAN_CODIGO = objCuentaBancoProveedor.CTABAN_CODIGO;
                                        infoCuentaBancoProv.MON_CODIGO = objCuentaBancoProveedor.MON_CODIGO;


                                        _starsoftConta.CUENTA_TELE_ANEXO.Add(infoCuentaBancoProv);
                                        _starsoftConta.SaveChanges();

                                    }
                                    var ultimoidProv = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == proveedor.id).OrderByDescending(x => x.id).FirstOrDefault();
                                    //                                    #region Alerta SIGOPLAN
                                    //                                    tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                                    //                                    objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                                    //                                    objNuevaAlerta.userRecibeID = 6068;
                                    //#if DEBUG
                                    //                                    //objNuevaAlerta.userRecibeID = 79626; //USUARIO ID:Aaron.
                                    //                                    objNuevaAlerta.userRecibeID = 13; //USUARIO ID:Administrador.
                                    //#endif
                                    //                                    objNuevaAlerta.tipoAlerta = 2;
                                    //                                    objNuevaAlerta.sistemaID = 4;
                                    //                                    objNuevaAlerta.visto = false;
                                    //                                    objNuevaAlerta.url = "/Enkontrol/GestionProveedor/GestionProveedores";
                                    //                                    objNuevaAlerta.objID = ultimoidProv.id != null ? ultimoidProv.id : 0;
                                    //                                    objNuevaAlerta.obj = "AltaProveedor";
                                    //                                    objNuevaAlerta.msj = "Auto. Prov CajaChica";
                                    //                                    objNuevaAlerta.documentoID = 0;
                                    //                                    objNuevaAlerta.moduloID = 0;
                                    //                                    _context.tblP_Alerta.Add(objNuevaAlerta);
                                    //                                    _context.SaveChanges();
                                    //                                    #endregion

                                    //                                    var UltimoID = _context.tblCom_MAEPROV.Where(e => e.registroActivo).OrderByDescending(x => x.id).FirstOrDefault();
                                    //                                    tblP_Usuario objUsuarioAutorizoAlta = _context.tblP_Usuario.Where(w => w.id == proveedor.id_usuarioAutorizo).FirstOrDefault();
                                    //                                    if (objUsuarioAutorizoAlta == null)
                                    //                                        throw new Exception("No se encuentra el correo del creador del alta.");

                                    //                                    if (string.IsNullOrEmpty(objUsuarioAutorizoAlta.correo))
                                    //                                        throw new Exception("No se encuentra el correo al autorizante.");

                                    //                                    #region SE ENVIA CORREO AL AUTORIZANTE
                                    //                                    List<string> lstCorreos = new List<string>();
                                    //                                    var objUsuarioNombreUsuario = _context.tblP_Usuario.Where(x => x.id == vSesiones.sesionUsuarioDTO.id).FirstOrDefault();

                                    //                                    lstCorreos.Add(objUsuarioAutorizoAlta.correo);
                                    //                                    lstCorreos.Add("alan.palomera@construplan.com.mx");

                                    //#if DEBUG
                                    //                                    lstCorreos = new List<string>();
                                    //                                    lstCorreos.Add("aaron.gracia@construplan.com.mx");
                                    //#endif

                                    //                                                        //        string body = string.Format("Buen día,<br> Se registro el proveedor " + proveedor.PRVCCODIGO + " " + proveedor.PRVCNOMBRE + "." +
                                    //                                                        //"<br><br>Por el usuario " + objUsuarioNombreUsuario.nombre + " " + objUsuarioNombreUsuario.apellidoPaterno + " " + objUsuarioNombreUsuario .apellidoMaterno + ".<br>" +

                                    //                                    string subject = "Registro Alta de proveedor";

                                    //                                    string body = string.Format(@"Buen dia ,<br>Se realizo el alta de proveedor {0} {1} como Caja Chica por el usuario {2} {3} {4}<br><br>
                                    //                                                      {5}<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>
                                    //                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                    //                                                       No es necesario dar una respuesta. Gracias.",
                                    //                                                           proveedor.PRVCCODIGO,
                                    //                                                           proveedor.PRVCNOMBRE,
                                    //                                                           objUsuarioNombreUsuario.nombre,
                                    //                                                           objUsuarioNombreUsuario.apellidoPaterno,
                                    //                                                           objUsuarioNombreUsuario.apellidoMaterno,
                                    //                                                           htmlCorreoCajaChica(UltimoID.id));

                                    //                                    GlobalUtils.sendEmail(subject, body, lstCorreos);
                                    //#endregion

                                    _starsoftConta.SaveChanges();
                                    dbStarsoftTransaction.Commit();
                                    dbContextTransaction.Commit();
                                    dbStarsoftTransactionCont.Commit();



                                    resultado.Add(SUCCESS, true);
                                }
                                catch (Exception e)
                                {
                                    dbContextTransaction.Rollback();
                                    dbStarsoftTransaction.Rollback();
                                    dbStarsoftTransactionCont.Rollback();
                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, e.Message);
                                }
                            }
                        }
                    }
                }
            }

            return resultado;
        }

        public Dictionary<string, object> AutorizarProveedor(int id)
        {
            bool Autorizado = false;
            var objAutorizador = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeAutorizar == true).ToList();
            var objVobo = _context.tblCom_AutorizarProveedor.Where(e => e.registroActivo && e.idUsuario == vSesiones.sesionUsuarioDTO.id && e.PuedeVobo == true).ToList();
            var objProveedor = _context.tblCom_MAEPROV.Where(e => e.registroActivo && e.id == id).FirstOrDefault();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                {
                    using (var dbStarsoftTransaction = _starsoft.Database.BeginTransaction())
                    {
                        using (var _starsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                        {
                            using (var dbStarsoftTransactionCont = _starsoftConta.Database.BeginTransaction())
                            {
                                try
                                {
                                    var validarProveedor = _starsoft.MAEPROV.Where(x => x.PRVCCODIGO == objProveedor.PRVCCODIGO).FirstOrDefault();

                                    if (validarProveedor != null)
                                    {
                                        throw new ArgumentException("El Codigo del proveedor ya existe", objProveedor.PRVCCODIGO);
                                    }
                                    else
                                    {
                                        var quitarAlertas = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && !x.visto && x.objID == id && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.userRecibeID == vSesiones.sesionUsuarioDTO.id).ToList();
                                        foreach (var item in quitarAlertas)
                                        {
                                            item.visto = true;
                                        }
                                        _context.SaveChanges();

                                        if (objProveedor.Vobo == true)
                                        {
                                            if (objAutorizador.Count() > 0)
                                            {
                                                objProveedor.Autorizado = true;
                                                objProveedor.id_usuarioAutorizo = vSesiones.sesionUsuarioDTO.id;
                                                objProveedor.fechaAutorizo = DateTime.Now;
                                                objProveedor.statusAutorizacion = true;
                                                Autorizado = true;
                                            }
                                        }

                                        _context.SaveChanges();
                                        //var AutorizarProveedor = _context.tblCom_MAEPROV.Where(e => e.id == id && e.Vobo && e.Autorizado).FirstOrDefault();
                                        //if (AutorizarProveedor != null)
                                        //{
                                        //    Autorizado = true;
                                        //    _context.SaveChanges();
                                        //}

                                        if (Autorizado == true)
                                        {
                                            MAEPROV infoStarsoftProv = new MAEPROV();

                                            infoStarsoftProv.PRVCCODIGO = objProveedor.PRVCCODIGO;
                                            infoStarsoftProv.PRVCNOMBRE = objProveedor.PRVCNOMBRE;
                                            infoStarsoftProv.PRVCESTADO = objProveedor.PRVCESTADO;
                                            infoStarsoftProv.PRVPAGO = objProveedor.PRVPAGO;
                                            infoStarsoftProv.PRVCDIRECC = objProveedor.PRVCDIRECC;
                                            infoStarsoftProv.PRVCLOCALI = objProveedor.PRVCLOCALI;
                                            infoStarsoftProv.PRVCTELEF1 = objProveedor.PRVCTELEF1;
                                            infoStarsoftProv.PRVCRUC = objProveedor.PRVCRUC;
                                            infoStarsoftProv.PRVCDOCIDEN = objProveedor.PRVCDOCIDEN;

                                            infoStarsoftProv.PRVDFECCRE = objProveedor.PRVDFECCRE;
                                            infoStarsoftProv.PRVCTIPO_DOCUMENTO = objProveedor.PRVCTIPO_DOCUMENTO;
                                            infoStarsoftProv.PRVCAPELLIDO_PATERNO = objProveedor.PRVCAPELLIDO_PATERNO;
                                            infoStarsoftProv.PRVCAPELLIDO_MATERNO = objProveedor.PRVCAPELLIDO_MATERNO;
                                            infoStarsoftProv.PRVCPRIMER_NOMBRE = objProveedor.PRVCPRIMER_NOMBRE;
                                            infoStarsoftProv.PRVCSEGUNDO_NOMBRE = objProveedor.PRVCSEGUNDO_NOMBRE;
                                            //infoStarsoftProv.FEC_INACTIVO_BLOQUEADO = objProveedor.FEC_INACTIVO_BLOQUEADO;
                                            infoStarsoftProv.COD_AUDITORIA = objProveedor.COD_AUDITORIA;
                                            infoStarsoftProv.UBIGEO = objProveedor.UBIGEO;
                                            infoStarsoftProv.FLGPORTAL_PROVEEDOR = objProveedor.FLGPORTAL_PROVEEDOR;

                                            _starsoft.MAEPROV.Add(infoStarsoftProv);
                                            _starsoft.SaveChanges();

                                            //infoStarsoftProv.PRVDFECCRE = objProveedor.PRVDFECCRE;
                                            ANEXO infoStarsoftAnexo = new ANEXO();

                                            infoStarsoftAnexo.ANEX_CODIGO = objProveedor.PRVCCODIGO;
                                            infoStarsoftAnexo.TIPOANEX_CODIGO = "03";
                                            infoStarsoftAnexo.ANEX_RUC = objProveedor.PRVCCODIGO;
                                            infoStarsoftAnexo.ANEX_DESCRIPCION = objProveedor.PRVCNOMBRE;
                                            infoStarsoftAnexo.ANEX_REFERENCIA = "";
                                            infoStarsoftAnexo.ANEX_DIRECCION = objProveedor.PRVCDIRECC;
                                            infoStarsoftAnexo.ANEX_TELEFONO = objProveedor.PRVCTELEF1;
                                            infoStarsoftAnexo.ANEX_REPRESENTANTE = objProveedor.PRVREPRESENTANTE;
                                            infoStarsoftAnexo.ANEX_GIRO = "";
                                            infoStarsoftAnexo.NRETENCION = false;
                                            infoStarsoftAnexo.ANEX_NOMBRE = objProveedor.PRVCPRIMER_NOMBRE + "" + objProveedor.PRVCSEGUNDO_NOMBRE;
                                            infoStarsoftAnexo.ANEX_APE_PAT = objProveedor.PRVCAPELLIDO_PATERNO;
                                            infoStarsoftAnexo.ANEX_APE_MAT = objProveedor.PRVCAPELLIDO_MATERNO;
                                            infoStarsoftAnexo.TIPOPERSONA = "";
                                            infoStarsoftAnexo.TIPODOCUMENTO = objProveedor.PRVCTIPO_DOCUMENTO;
                                            infoStarsoftAnexo.DOCUMENTOIDENTIDAD = objProveedor.PRVCDOCIDEN;
                                            infoStarsoftAnexo.ANE_DETRACCION = false;
                                            infoStarsoftAnexo.ANE_TASA_DETRACC = 0;
                                            infoStarsoftAnexo.COD_DETRAC = "";
                                            infoStarsoftAnexo.ANEX_NOMBRE_2 = "";
                                            infoStarsoftAnexo.ANEX_NACIONALIDAD = "";
                                            infoStarsoftAnexo.ANEX_SEXO = false;
                                            infoStarsoftAnexo.TCL_CODIGO = "";
                                            infoStarsoftAnexo.ANEX_GLOSA = "";
                                            infoStarsoftAnexo.ANEX_EST_CODIGO = "";
                                            infoStarsoftAnexo.ANEX_COND_CODIGO = "";
                                            infoStarsoftAnexo.COD_AFP_ONP = "";
                                            infoStarsoftAnexo.ANEX_TIPO_COMISION = "";
                                            infoStarsoftAnexo.COD_DOBLE_TRIB = "";
                                            infoStarsoftAnexo.ANEX_DISTRITO = "";
                                            infoStarsoftAnexo.ANEX_BENE_CTA = "";
                                            infoStarsoftAnexo.ANEX_BENE_METODOENRUTA = "";
                                            infoStarsoftAnexo.ANEX_BENE_CODIGOENRUTA = "";
                                            infoStarsoftAnexo.ANEX_BENE_NOMBRE = "";
                                            infoStarsoftAnexo.ANEX_BENE_DIRECCION1 = "";
                                            infoStarsoftAnexo.ANEX_BENE_DIRECCION2 = "";
                                            infoStarsoftAnexo.ANEX_INTERME_METODOENRUTA = "";
                                            infoStarsoftAnexo.ANEX_INTERME_CODIGOENRUTA = "";
                                            infoStarsoftAnexo.ANEX_INTERME_NOMBRE = "";
                                            infoStarsoftAnexo.ANEX_INTERME_DIRECCION1 = "";
                                            infoStarsoftAnexo.ANEX_INTERME_DIRECCION2 = "";
                                            infoStarsoftAnexo.ANEX_CORREO = "";
                                            infoStarsoftAnexo.ANEX_UBIGEO = objProveedor.UBIGEO;
                                            infoStarsoftAnexo.TIPO_PROVEEDOR = "";
                                            infoStarsoftAnexo.ANEX_PORTAL_PROVEEDOR = false;

                                            _starsoftConta.ANEXO.Add(infoStarsoftAnexo);
                                            _starsoftConta.SaveChanges();

                                            var objCuentaBancoProveedor = _context.tblCom_CuentasBancosProveedor.Where(e => e.registroActivo && e.ANEXO == "03" + objProveedor.PRVCCODIGO).ToList();

                                            foreach (var item in objCuentaBancoProveedor)
                                            {
                                                CUENTA_TELE_ANEXO infoCuentaBancoProv = new CUENTA_TELE_ANEXO();

                                                infoCuentaBancoProv.ANEXO = item.ANEXO;
                                                infoCuentaBancoProv.BAN_CODIGO = item.BAN_CODIGO;
                                                infoCuentaBancoProv.CTABAN_CODIGO = item.CTABAN_CODIGO;
                                                infoCuentaBancoProv.MON_CODIGO = item.MON_CODIGO;


                                                _starsoftConta.CUENTA_TELE_ANEXO.Add(infoCuentaBancoProv);
                                                _starsoftConta.SaveChanges();
                                            }


                                            #region SE ENVIA CORREO AL AUTORIZANTE
                                            List<string> lstCorreos = new List<string>();
                                            var usuarioQueAplicoVobo = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioVobo);
                                            var usuarioQueAplicoAutorizacion = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioAutorizo);
                                            var usuarioCreoProveedor = _context.tblP_Usuario.FirstOrDefault(x => x.id == objProveedor.id_usuarioCreacion);

                                            if (usuarioQueAplicoVobo != null && !string.IsNullOrEmpty(usuarioQueAplicoVobo.correo))
                                            {
                                                lstCorreos.Add(usuarioQueAplicoVobo.correo);
                                            }
                                            if (usuarioQueAplicoAutorizacion != null && !string.IsNullOrEmpty(usuarioQueAplicoAutorizacion.correo))
                                            {
                                                lstCorreos.Add(usuarioQueAplicoAutorizacion.correo);
                                            }
                                            if (usuarioCreoProveedor != null && !string.IsNullOrEmpty(usuarioCreoProveedor.correo))
                                            {
                                                lstCorreos.Add(usuarioCreoProveedor.correo);
                                            }
#if DEBUG
                                            lstCorreos = new List<string> { _OMAR_NUNEZ_CORREO };
#endif
                                            string subject = "Alta de proveedor";
                                            string body = string.Format(@"Buen dia ,<br>Se informa que el usuario {0} {1} {2} Autorizó el alta del siguiente proveedor {3} {4} <br>
                                                      {5}<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                                   usuarioQueAplicoAutorizacion.nombre,
                                                                   usuarioQueAplicoAutorizacion.apellidoPaterno,
                                                                   usuarioQueAplicoAutorizacion.apellidoMaterno,
                                                                   objProveedor.PRVCCODIGO,
                                                                   objProveedor.PRVCNOMBRE,
                                                                   htmlCorreo(objProveedor.id));

                                            // SE OBTIENE LA RUTA DE LOS ARCHIVOS LIGADOS AL REGISTRO
                                            List<tblCom_ArchivosAdjuntosProveedores> lstArchivosAdjuntosProveedores = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == id && w.registroActivo).ToList();
                                            List<string> lstArchivos = new List<string>();
                                            foreach (var objArchivo in lstArchivosAdjuntosProveedores)
                                            {
                                                if (!string.IsNullOrEmpty(objArchivo.rutaArchivo))
                                                    lstArchivos.Add(objArchivo.rutaArchivo);
                                            }

                                            GlobalUtils.sendEmailAdjunto(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos, lstArchivos);
                                            #endregion
                                        }

                                        dbStarsoftTransaction.Commit();
                                        dbStarsoftTransactionCont.Commit();
                                        dbContextTransaction.Commit();
                                        resultado.Add(SUCCESS, true);
                                    }
                                }
                                catch (Exception e)
                                {
                                    dbContextTransaction.Rollback();
                                    dbStarsoftTransaction.Rollback();
                                    dbStarsoftTransactionCont.Rollback();
                                    resultado.Add(SUCCESS, false);
                                    resultado.Add(MESSAGE, e.Message);
                                }
                            }
                        }
                    }
                }
            }

            return resultado;
        }
        public Dictionary<string, object> NotificarAltaProveedor(int id)
        {
            using (var dbContextTransactionNotificacion = _context.Database.BeginTransaction())
            {
                resultado = new Dictionary<string, object>();
                try
                {
                    #region VALIDACIONES
                    if (id <= 0) { throw new Exception("Ocurrió un error al notificar el alta de proveedor."); }
                    #endregion

                    #region SE INDICA QUE EL ALTA DE PROVEEDOR SE ENCUENTRA EN ESTATUS NOTIFICADO
                    tblCom_MAEPROV objProveedor = _context.tblCom_MAEPROV.Where(w => w.registroActivo && w.id == id && !w.Vobo).FirstOrDefault();
                    if (objProveedor == null)
                        throw new Exception("Ocurrió un error al notificar.");
                    #endregion

                    #region SE OBTIENE AL AUTORIZADOR
                    List<tblCom_AutorizarProveedor> lstAutorizador = _context.tblCom_AutorizarProveedor.Where(w => w.registroActivo && w.PuedeAutorizar == true).ToList();
                    tblCom_AutorizarProveedor objVobo = _context.tblCom_AutorizarProveedor.Where(w => w.registroActivo && w.idUsuario == vSesiones.sesionUsuarioDTO.id && w.PuedeVobo == true).FirstOrDefault();
                    
                    // SE OBTIENE EL CORREO DEL CREADOR DEL CREADOR DEL ALTA
                    if (objVobo != null)
                    {
                        List<tblP_Alerta> alertasRelacionadasAlVoBo = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && x.objID == id && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && 
                                                                                                        !x.visto && x.userRecibeID == vSesiones.sesionUsuarioDTO.id).ToList();
                        foreach (var item in alertasRelacionadasAlVoBo)
                        {
                            item.visto = true;
                        }
                        _context.SaveChanges();

                        objProveedor.Vobo = true;
                        objProveedor.id_usuarioVobo = vSesiones.sesionUsuarioDTO.id;
                        objProveedor.fechaVobo = DateTime.Now;

                        _context.SaveChanges();

                        foreach (var item in lstAutorizador)
                        {
                            tblP_Usuario objUsuarioAutorizoAlta = _context.tblP_Usuario.Where(w => w.id == item.idUsuario).FirstOrDefault();
                            tblP_Usuario objUsuarioVobo = _context.tblP_Usuario.Where(w => w.id == objVobo.idUsuario).FirstOrDefault();
                            if (objUsuarioAutorizoAlta == null)
                                throw new Exception("No se encuentra el correo del creador del alta.");

                            if (string.IsNullOrEmpty(objUsuarioAutorizoAlta.correo))
                                throw new Exception("No se encuentra el correo al autorizante.");

                            #region SE ENVIA CORREO AL AUTORIZANTE
                            List<string> lstCorreos = new List<string>();
                            lstCorreos.Add(objUsuarioAutorizoAlta.correo);
#if DEBUG
                            lstCorreos = new List<string> { _OMAR_NUNEZ_CORREO };
#endif
                            string subject = "Alta de proveedor";
                            string body = string.Format(@"Buen dia,<br>Se informa que el usuario {0} {1} {2} confirmó el alta del siguiente proveedor {3} {4} y se encuentra listo para ser autorizado <br>
                                                      {5}<br><br>Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>Construplan Peru > Compras > Proveedores.<br>
                                                       Se informa que este es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>).
                                                       No es necesario dar una respuesta. Gracias.",
                                                   objUsuarioVobo.nombre,
                                                   objUsuarioVobo.apellidoPaterno,
                                                   objUsuarioVobo.apellidoMaterno,
                                                   objProveedor.PRVCCODIGO,
                                                   objProveedor.PRVCNOMBRE,
                                                   htmlCorreo(objProveedor.id));

                            // SE OBTIENE LA RUTA DE LOS ARCHIVOS LIGADOS AL REGISTRO
                            List<tblCom_ArchivosAdjuntosProveedores> lstArchivosAdjuntosProveedores = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.FK_idProv == id && w.registroActivo).ToList();
                            List<string> lstArchivos = new List<string>();
                            foreach (var objArchivo in lstArchivosAdjuntosProveedores)
                            {
                                if (!string.IsNullOrEmpty(objArchivo.rutaArchivo))
                                    lstArchivos.Add(objArchivo.rutaArchivo);
                            }

                            GlobalUtils.sendEmailAdjunto(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lstCorreos, lstArchivos); // TODO
                            #endregion

                            #region Alerta SIGOPLAN
                            tblP_Alerta objNuevaAlerta = new tblP_Alerta();
                            objNuevaAlerta.userEnviaID = (int)vSesiones.sesionUsuarioDTO.id;
                            objNuevaAlerta.userRecibeID = item.idUsuario;
#if DEBUG
                            //objNuevaAlerta.userRecibeID = 79626; //USUARIO ID:Aaron.
                            objNuevaAlerta.userRecibeID = 13; //USUARIO ID:Administrador.
#endif
                            objNuevaAlerta.tipoAlerta = 2;
                            objNuevaAlerta.sistemaID = 4;
                            objNuevaAlerta.visto = false;
                            objNuevaAlerta.url = "/Enkontrol/GestionProveedor/GestionProveedores?id=" + objProveedor.PRVCCODIGO;
                            objNuevaAlerta.objID = objProveedor.id;
                            objNuevaAlerta.obj = "AutorizacionProveedor";
                            objNuevaAlerta.msj = "Autorización Proveedor - " + objProveedor.PRVCCODIGO;
                            objNuevaAlerta.documentoID = 0;
                            objNuevaAlerta.moduloID = 0;
                            _context.tblP_Alerta.Add(objNuevaAlerta);
                            _context.SaveChanges();
                            #endregion //ALERTA SIGPLAN

                    #endregion
                            objProveedor.statusNotificacion = true;
                        }
                    }
                    else
                    {
                        throw new Exception("No tiene permiso para aplicar VoBo");
                    }

                    _context.SaveChanges();
                    dbContextTransactionNotificacion.Commit();
                    resultado.Add(SUCCESS, true);
                    resultado.Add(MESSAGE, "Se ha notificado con éxito");
                }
                catch (Exception e)
                {
                    dbContextTransactionNotificacion.Rollback();
                    resultado.Add(MESSAGE, e.Message);
                    resultado.Add(SUCCESS, false);
                }
            }
            return resultado;
        }

        public Dictionary<string, object> editarProveedor(tblCom_MAEPROV proveedor)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objProveedor = _context.tblCom_MAEPROV.FirstOrDefault(x => x.id == proveedor.id);

                    objProveedor.PRVCCODIGO = proveedor.PRVCCODIGO;
                    objProveedor.PRVCNOMBRE = proveedor.PRVCNOMBRE;
                    objProveedor.PRVCDIRECC = proveedor.PRVCDIRECC;
                    objProveedor.PRVCLOCALI = proveedor.PRVCLOCALI;
                    objProveedor.PRVCPAISAC = proveedor.PRVCPAISAC;
                    objProveedor.PRVCTELEF1 = proveedor.PRVCDIRECC;
                    objProveedor.PRVCFAXACR = proveedor.PRVCFAXACR;
                    objProveedor.PRVCTIPOAC = proveedor.PRVCTIPOAC;
                    objProveedor.PRVCGIROAC = proveedor.PRVCGIROAC;
                    objProveedor.PRVCREPRES = proveedor.PRVCREPRES;
                    objProveedor.PRVCCARREP = proveedor.PRVCCARREP;
                    objProveedor.PRVCTELREP = proveedor.PRVCTELREP;
                    objProveedor.PRVDFECCRE = proveedor.PRVDFECCRE;
                    objProveedor.PRVCUSER = proveedor.PRVCUSER;
                    objProveedor.PRPVCRUC = proveedor.PRPVCRUC;
                    objProveedor.PRVCRUC = proveedor.PRVCRUC;
                    objProveedor.PRVCABREVI = proveedor.PRVCABREVI;
                    objProveedor.PRVCESTADO = proveedor.PRVCESTADO;
                    objProveedor.PRVDFECMOD = proveedor.PRVDFECMOD;
                    objProveedor.PRVEMAIL = proveedor.PRVEMAIL;
                    objProveedor.PRVCODFAB = proveedor.PRVCODFAB;
                    objProveedor.PRVPAGO = proveedor.PRVPAGO;
                    objProveedor.PRVFACTOR = proveedor.PRVFACTOR;
                    objProveedor.PRVGLOSA = proveedor.PRVGLOSA;
                    objProveedor.PRVREPRESENTANTE = proveedor.PRVREPRESENTANTE;
                    objProveedor.PRVCONTACTO = proveedor.PRVCONTACTO;
                    objProveedor.PRVTELREP = proveedor.PRVTELREP;
                    objProveedor.PRVFAXREP = proveedor.PRVFAXREP;
                    objProveedor.PRVEMAILREP = proveedor.PRVEMAILREP;
                    objProveedor.PRVCTIPO_DOCUMENTO = proveedor.PRVCTIPO_DOCUMENTO;
                    objProveedor.PRVCAPELLIDO_PATERNO = proveedor.PRVCAPELLIDO_PATERNO;
                    objProveedor.PRVCAPELLIDO_MATERNO = proveedor.PRVCAPELLIDO_MATERNO;
                    objProveedor.PRVCPRIMER_NOMBRE = proveedor.PRVCPRIMER_NOMBRE;
                    objProveedor.PRVCSEGUNDO_NOMBRE = proveedor.PRVCSEGUNDO_NOMBRE;
                    objProveedor.PRVCDOCIDEN = proveedor.PRVCDOCIDEN;
                    objProveedor.FEC_INACTIVO_BLOQUEADO = proveedor.FEC_INACTIVO_BLOQUEADO;
                    objProveedor.COD_AUDITORIA = proveedor.COD_AUDITORIA;
                    objProveedor.UBIGEO = proveedor.UBIGEO;
                    objProveedor.FLGPORTAL_PROVEEDOR = proveedor.FLGPORTAL_PROVEEDOR;

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

        public Dictionary<string, object> eliminarProveedor(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objProveedor = _context.tblCom_MAEPROV.FirstOrDefault(x => x.id == id);

                    var quitarAlertas = _context.tblP_Alerta.Where(x => x.tipoAlerta == 2 && x.sistemaID == 4 && !x.visto && x.objID == (int)objProveedor.id && (x.obj == "AutorizacionProveedor" || x.obj == "AltaProveedor") && x.userRecibeID == vSesiones.sesionUsuarioDTO.id).ToList();
                    foreach (var item in quitarAlertas)
                    {
                        item.visto = true;
                    }
                    _context.SaveChanges();

                    objProveedor.registroActivo = false;
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

        public Dictionary<string, object> getCuentasBancosProveedores(string anexo)
        {

            try
            {
                var listaCuentas = _context.tblCom_CuentasBancosProveedor.Where(x => x.registroActivo && x.ANEXO == anexo).ToList();
                resultado.Add(ITEMS, listaCuentas);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> getDatosCuentasBancoProveedor(int id)
        {
            try
            {
                var listaCuentaBancoProveedores = _context.tblCom_CuentasBancosProveedor.Where(x => x.registroActivo && x.id == id).FirstOrDefault();
                listaCuentaBancoProveedores.ANEXO.Trim();
                listaCuentaBancoProveedores.CTABAN_CODIGO.Trim();
                resultado.Add(ITEMS, listaCuentaBancoProveedores);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> guardarEditarCuentaBancoProveedor(tblCom_CuentasBancosProveedor CuentasBancosProveedor)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                using (var _starsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                {
                    using (var dbStarsoftTransactionCont = _starsoftConta.Database.BeginTransaction())
                    {
                        try
                        {
                            var objCuentaBancoProveedor = _context.tblCom_CuentasBancosProveedor.Where(e => e.registroActivo && e.id == CuentasBancosProveedor.id).FirstOrDefault();

                            if (objCuentaBancoProveedor != null)
                            {
                                objCuentaBancoProveedor.BAN_CODIGO = CuentasBancosProveedor.BAN_CODIGO;
                                objCuentaBancoProveedor.CTABAN_CODIGO = CuentasBancosProveedor.CTABAN_CODIGO;
                                objCuentaBancoProveedor.MON_CODIGO = CuentasBancosProveedor.MON_CODIGO;
                                objCuentaBancoProveedor.id_usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                                objCuentaBancoProveedor.fechaModificacion = DateTime.Now;

                                //var anexo13Digitos = CuentasBancosProveedor.ANEXO.Substring(2);
                                //var objCuentaBancoProveedorStarsoft = _starsoftConta.CUENTA_TELE_ANEXO.Where(e => e.ANEXO ==anexo13Digitos).FirstOrDefault();

                                //if (objCuentaBancoProveedorStarsoft != null)
                                //{
                                //    objCuentaBancoProveedorStarsoft.BAN_CODIGO = CuentasBancosProveedor.BAN_CODIGO;
                                //    objCuentaBancoProveedorStarsoft.CTABAN_CODIGO = CuentasBancosProveedor.CTABAN_CODIGO;
                                //    objCuentaBancoProveedorStarsoft.MON_CODIGO = CuentasBancosProveedor.MON_CODIGO;

                                //    _starsoftConta.SaveChanges();

                                //}
                            }
                            else
                            {
                                CuentasBancosProveedor.idUsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                                CuentasBancosProveedor.fechaCreacion = DateTime.Now;
                                _context.tblCom_CuentasBancosProveedor.Add(CuentasBancosProveedor);
                                CuentasBancosProveedor.registroActivo = true;
                            }

                            _context.SaveChanges();
                            dbContextTransaction.Commit();
                            dbStarsoftTransactionCont.Commit();
                            resultado.Add(SUCCESS, true);
                        }
                        catch (Exception e)
                        {
                            dbContextTransaction.Rollback();
                            resultado.Add(SUCCESS, false);
                            resultado.Add(MESSAGE, e.Message);
                        }
                    }
                }
            }
            return resultado;
        }

        public Dictionary<string, object> eliminarCuentaBancoProveedor(int id)
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var objCuentaBancoProveedor = _context.tblCom_CuentasBancosProveedor.FirstOrDefault(x => x.id == id);

                    objCuentaBancoProveedor.registroActivo = false;
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

        #region ARCHIVOS
        public Dictionary<string, object> RemoveArchivos(int idArchivo)
        {
            resultado.Clear();


            using (var ctx = new MainContext())
            {
                using (var dbContext = ctx.Database.BeginTransaction())
                {
                    var objArchivoToRemove = ctx.tblCom_ArchivosAdjuntosProveedores.FirstOrDefault(e => e.registroActivo && e.id == idArchivo);

                    try
                    {
                        objArchivoToRemove.registroActivo = false;
                        objArchivoToRemove.fechaModificacion = DateTime.Now;
                        objArchivoToRemove.FK_UsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        SaveBitacora(0, (int)AccionEnum.ELIMINAR, objArchivoToRemove.id, JsonUtils.convertNetObjectToJson(objArchivoToRemove));

                        ctx.SaveChanges();
                        dbContext.Commit();

                        resultado.Add(SUCCESS, true);
                    }
                    catch (Exception e)
                    {
                        dbContext.Rollback();

                        LogError(0, 0, "GestionProveedorDAO", "RemoveArchivos", e, AccionEnum.ELIMINAR, objArchivoToRemove.id, objArchivoToRemove);

                        resultado.Add(MESSAGE, e.Message);
                        resultado.Add(SUCCESS, false);
                    }
                }

            }

            return resultado;
        }

        public Tuple<Stream, string> descargarArchivo(int idArchivo)
        {
            try
            {
                var objArchivoAdjunto = _context.tblCom_ArchivosAdjuntosProveedores.Where(w => w.id == idArchivo && w.registroActivo).FirstOrDefault();

                var fileStream = GlobalUtils.GetFileAsStream(objArchivoAdjunto.rutaArchivo);
                string name = Path.GetFileName(objArchivoAdjunto.rutaArchivo);

                return Tuple.Create(fileStream, name);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        #endregion
    }
}

