﻿using Core.DAO.Enkontrol.Compras;
using Core.DTO;
using Core.DTO.Almacen;
using Core.DTO.Enkontrol.Alamcen;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Enkontrol.Requisicion;
using Core.DTO.Utils;
using Core.DTO.Utils.Data;
using Core.DTO.Utils.DataTable;
using Core.Entity.Enkontrol.Compras;
using Core.Entity.Enkontrol.Compras.OrdenCompra;
using Core.Entity.Enkontrol.Compras.Requisicion;
using Core.Entity.Maquinaria.BackLogs;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.StandBy;
using Core.Entity.Principal.Usuarios;
using Core.Entity.StarSoft.Almacen;
using Core.Entity.StarSoft.Requisiciones;
using Core.Enum.Enkontrol;
using Core.Enum.Enkontrol.Compras;
using Core.Enum.Enkontrol.Requisicion;
using Core.Enum.Maquinaria.BackLogs;
using Core.Enum.Maquinaria.StandBy;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal.Usuario;
using Dapper;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Maquinaria;
using Data.Factory.Principal.Menus;
using Data.Factory.Principal.Usuarios;
using Infrastructure.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Data.DAO.Enkontrol.Compras
{
    public class Requisicion2DTO
    {
        public string cc { get; set; }
        public int numero { get; set; }
        public DateTime fecha { get; set; }
        public string autorizo { get; set; }
        public string solicito { get; set; }
        public bool isAuth { get; set; }
        public bool flagCheckBox { get; set; }
        public decimal montoTotal { get; set; }
        public string monedaDesc { get; set; }
        public bool consigna { get; set; }
        public bool licitacion { get; set; }
        public bool crc { get; set; }
        public bool convenio { get; set; }
        public string ccNom { get; set; }
        public string solNom { get; set; }
        public decimal cantidadTotal { get; set; }
        public int contieneCancelado { get; set; }
        public string otFolio { get; set; }
        public string economico { get; set; }
        public int tipoPartida { get; set; }
        public int tipoPartidaDet { get; set; }
    }

    public class RequisicionDAO : GenericDAO<tblCom_Req>, IRequisicionDAO
    {
        private const int _SISTEMA = (int)SistemasEnum.COMPRAS;
        private const string _NOMBRE_CONTROLADOR = "RequisicionController";

        UsuarioFactoryServices ufs = new UsuarioFactoryServices();
        MenuFactoryServices mfs = new MenuFactoryServices();

        private bool productivo = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["enkontrolProductivo"]) == "1";

        private OdbcConnection checkConexionProductivo()
        {
            if (productivo)
            {
                return new Conexion().Connect();
            }
            else
            {
                return new Conexion().ConnectPrueba();
            }
        }

        private dynamic consultaCheckProductivo(string consulta)
        {
            if (productivo)
            {
                return _contextEnkontrol.WhereComprasOrigen(consulta);
            }
            else
            {
                return _contextEnkontrolPrueba.Where(consulta);
            }
        }

        private List<dynamic> consultaListCheckProductivo(List<string> listString)
        {
            if (productivo)
            {
                return _contextEnkontrol.Where(listString);
            }
            else
            {
                return _contextEnkontrolPrueba.Where(listString);
            }
        }

        #region Guardar
        public Dictionary<string, object> guardar(tblCom_Req req, List<tblCom_ReqDet> det, List<ReqDetalleComentarioDTO> comentarios)
        {
            var result = new Dictionary<string, object>();

            #region PERU
                using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var usuario = vSesiones.sesionUsuarioDTO;
                        var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                        req.solicito = relUser.empleado;

                        var esUpdate = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == req.cc && x.numero == req.numero);
                        // Obtener el último número de requisición y sumarle 1
                        var ultimoNumeroRequisicion = _context.tblCom_Req
                            .Where(x => x.estatusRegistro && x.cc == req.cc)
                            .OrderByDescending(x => x.numero)  // Ordenar de forma descendente por el número
                            .Select(x => x.numero)
                            .FirstOrDefault();

                        // Si no hay requisiciones previas, el número empieza en 1
                        var nuevoNumeroRequisicion = ultimoNumeroRequisicion + 1;

                        #region Guardar Requisición
                        req.stEstatus = req.stEstatus ?? string.Empty;

                        var num = esUpdate != null ? req.numero : nuevoNumeroRequisicion;
                        var save = _context.tblCom_Req.FirstOrDefault(r => r.estatusRegistro && r.cc.Equals(req.cc) && r.numero == num && r.PERU_tipoRequisicion == req.PERU_tipoRequisicion);
                        var isSigoplanSave = save == null;

                        if (isSigoplanSave)
                            save = new tblCom_Req();
                        else
                        {
                            var detallesGuardados = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == save.id).ToList();

                            foreach (var detGua in detallesGuardados)
                            {
                                detGua.estatus = "false";
                                detGua.estatusRegistro = false;

                                _context.SaveChanges();
                            }
                        }

                        //var registroRelacionUsuarioStarsoft = _context.tblP_Usuario_Starsoft.FirstOrDefault(x => x.sigoplan_usuario_id == vSesiones.sesionUsuarioDTO.id);

                        //if (registroRelacionUsuarioStarsoft == null)
                        //{
                        //    throw new Exception("No se encuentra la información del usuario de Starsoft.");
                        //}

                        save.numero = esUpdate != null ? req.numero : Convert.ToInt32(nuevoNumeroRequisicion);
                        save.cc = req.cc;
                        save.fecha = req.fecha;
                        save.idLibreAbordo = req.idLibreAbordo;
                        save.idTipoReqOc = req.idTipoReqOc;
                        //save.solicito = Int32.Parse(registroRelacionUsuarioStarsoft.starsoft_usuario_id);
                        save.solicito = req.solicito;
                        save.vobo = req.vobo;
                        save.autorizo = req.autorizo;
                        save.comentarios = req.comentarios;
                        save.stEstatus = req.stEstatus;
                        save.stImpresa = req.stImpresa;
                        save.stAutoriza = req.stAutoriza;
                        save.empAutoriza = req.empAutoriza;
                        save.empModifica = req.empModifica;
                        save.modifica = esUpdate != null ? req.modifica : new DateTime(2011, 1, 1);
                        save.autoriza = esUpdate != null ? req.modifica : new DateTime(2011, 1, 1);
                        save.isTmc = req.isTmc;
                        save.isActivos = req.isActivos;
                        save.folioAsignado = req.folioAsignado != null ? req.folioAsignado : "";
                        save.consigna = req.consigna;
                        save.licitacion = req.licitacion;
                        save.crc = req.crc;
                        save.convenio = req.convenio;
                        save.proveedor = req.proveedor;
                        save.validadoAlmacen = true;
                        save.validadoCompras = true;
                        save.validadoRequisitor = true; //Se salta el paso de validación del requisitor.
                        save.fechaValidacionAlmacen = null;
                        save.comprador = req.comprador ?? 0;
                        save.usuarioSolicita = req.usuarioSolicita;
                        save.usuarioSolicitaUso = req.usuarioSolicitaUso ?? "";
                        save.usuarioSolicitaEmpresa = req.usuarioSolicitaEmpresa;
                        save.estatusRegistro = true;
                        save.PERU_codigoAuditoria = req.PERU_codigoAuditoria;
                        save.PERU_tipoRequisicion = req.PERU_tipoRequisicion;
                        save.otID = req.otID;
                        save.otFolio = req.otFolio;
                        save.noEconomico = req.noEconomico;
                        _context.tblCom_Req.AddOrUpdate(save);
                        SaveChanges();
                        #endregion

                        req.id = save.id;

                        if (req.id > 0)
                        {
                            det.ForEach(d =>
                            {
                                d.idReq = req.id;
                                d.cc = req.cc;
                                d.numero = req.numero;
                                d.comentarioSurtidoQuitar = d.comentarioSurtidoQuitar ?? "";

                                #region Guardar Requisición Partida
                                //var tipoRequisicion = req.idTipoReqOc;
                                DateTime fechaRequerida = DateTime.Now;

                                //if (tipoRequisicion > 0)
                                //{
                                //    var diasEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_tipo_requisicion WHERE tipo_req_oc = {0}", tipoRequisicion));

                                //    if (diasEK != null)
                                //    {
                                //        var dias = (int)(((List<dynamic>)diasEK.ToObject<List<dynamic>>())[0].dias_requisicion);

                                fechaRequerida = DateTime.Now.AddDays(1);
                                //    }
                                //}

                                var registroDetalle = new tblCom_ReqDet();

                                registroDetalle.idReq = req.id;
                                registroDetalle.partida = d.partida;
                                registroDetalle.insumo = d.insumo;
                                registroDetalle.referencia = "";
                                registroDetalle.descripcion = d.descripcion;
                                registroDetalle.requerido = fechaRequerida;
                                registroDetalle.cantidad = d.cantidad;
                                registroDetalle.precio = d.precio;
                                registroDetalle.cantOrdenada = d.cantOrdenada;
                                registroDetalle.ordenada = d.ordenada;
                                registroDetalle.estatus = d.estatus;
                                registroDetalle.cantCancelada = d.cantCancelada;
                                registroDetalle.cantExcedida = 0;
                                registroDetalle.area = d.area;
                                registroDetalle.cuenta = d.cuenta;
                                registroDetalle.observaciones = d.observaciones;
                                registroDetalle.comentarioSurtidoQuitar = d.comentarioSurtidoQuitar;
                                registroDetalle.estatusRegistro = true;
                                registroDetalle.PERU_ordenFabricacion = d.PERU_ordenFabricacion;
                                registroDetalle.PERU_saldo = d.PERU_saldo;
                                registroDetalle.PERU_tipoRequisicion = d.PERU_tipoRequisicion;
                                registroDetalle.noEconomico = d.noEconomico;
                                registroDetalle.numero = save.numero;
                                registroDetalle.cc = save.cc;
                                registroDetalle.tipoPartida = d.tipoPartida;
                                registroDetalle.tipoPartidaDet = d.tipoPartidaDet;

                                _context.tblCom_ReqDet.AddOrUpdate(registroDetalle);
                                SaveChanges();
                                #endregion
                            });
                        }

                        // if (esUpdate == null)
                        // {
                        //     var numRequiEntity = _starsoft.NUM_DOCCOMPRAS.FirstOrDefault(x => x.CTNCODIGO == req.PERU_tipoRequisicion);
                        //     numRequiEntity.CTNNUMERO = nuevoNumeroRequisicion;
                        //     _starsoft.SaveChanges();
                        // }

                        #region SE ACTUALIZA EL ESTATUS DEL BL A ESTATUS DE REQUISICIÓN
                        if (req.idBL.HasValue && req.idBL.Value != 0)
                        {
                            tblBL_Requisiciones requiBL = null;
                            requiBL = _context.tblBL_Requisiciones.FirstOrDefault(f => f.idBackLog == req.idBL.Value && f.esActivo);

                            if (requiBL == null)
                            {
                                requiBL = new tblBL_Requisiciones();
                                requiBL.idBackLog = req.idBL.Value;
                                requiBL.numRequisicion = save.numero.ToString();
                                requiBL.FK_UsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                requiBL.fechaCreacionRequisicion = DateTime.Now;
                                requiBL.esActivo = true;

                                var backlog = _context.tblBL_CatBackLogs.FirstOrDefault(f => f.id == req.idBL && f.esActivo);

                                if (backlog == null)
                                    throw new Exception("No se encontró el BL");
                                else
                                {
                                    backlog.idEstatus = (int)EstatusBackLogEnum.ElaboracionRequisicion;
                                    _context.tblBL_Requisiciones.Add(requiBL);

                                    #region SE REGISTRA BITACORA DE CUANTOS DÍAS DURO EL ESTATUS A ACTUALIZAR
                                    tblBL_BitacoraEstatusBL objBitacoraBL = _context.tblBL_BitacoraEstatusBL.Where(w => w.idBL == req.idBL && w.esActivo).OrderByDescending(o => o.id).FirstOrDefault();
                                    if (objBitacoraBL != null)
                                    {
                                        int diasTranscurridos = (DateTime.Now - backlog.fechaCreacionBL).Days;
                                        tblBL_BitacoraEstatusBL objGuardarBitacoraEstatusBL = new tblBL_BitacoraEstatusBL();
                                        objGuardarBitacoraEstatusBL.idBL = backlog.id;
                                        objGuardarBitacoraEstatusBL.areaCuenta = backlog.areaCuenta;
                                        objGuardarBitacoraEstatusBL.diasTranscurridos = diasTranscurridos;
                                        objGuardarBitacoraEstatusBL.idEstatus = (int)EstatusBackLogEnum.ElaboracionRequisicion;
                                        objGuardarBitacoraEstatusBL.idUsuarioCreacion = (int)vSesiones.sesionUsuarioDTO.id;
                                        objGuardarBitacoraEstatusBL.idUsuarioModificacion = 0;
                                        objGuardarBitacoraEstatusBL.fechaCreacion = DateTime.Now;
                                        objGuardarBitacoraEstatusBL.fechaModificacion = new DateTime(2000, 01, 01);
                                        objGuardarBitacoraEstatusBL.esActivo = true;
                                        _context.tblBL_BitacoraEstatusBL.Add(objGuardarBitacoraEstatusBL);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                if (save.numero.ToString() != requiBL.numRequisicion)
                                {
                                    throw new Exception("No se guardó la requisición debido a que esta haciendo referencia a la requisición de un backlog con un número de requisición diferente");
                                }
                                requiBL.fechaModificacionRequisicion = DateTime.Now;
                            }
                            _context.SaveChanges();
                        }
                        #endregion

                        dbSigoplanTransaction.Commit();

                        var numeroRequisicionNueva = esUpdate != null ? req.numero : nuevoNumeroRequisicion;

                        result.Add(SUCCESS, numeroRequisicionNueva > 0 ? true : false);
                        result.Add("numeroRequisicionNueva", numeroRequisicionNueva);
                    }
                    catch (Exception e)
                    {
                        dbSigoplanTransaction.Rollback();

                        result.Add(MESSAGE, e.Message);
                        result.Add(SUCCESS, false);

                        LogError(0, 0, "RequisicionController", "guardarRequisicionPeru", e, AccionEnum.AGREGAR, 0, new { req = req, det = det, comentarios = comentarios });
                    }
                }
                #endregion

            return result;
        }

        private bool ActivarEconomicoPorAccionRealizada(string numeroEconomico, int? idEconomico, AccionActivacionEconomicoEnum accion, object objeto, bool buscarEnEnkontrol = false)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Arrendadora)
            {
                return false;
            }

            tblM_CatMaquina maquina = null;

            if (buscarEnEnkontrol)
            {
                if (!string.IsNullOrEmpty(numeroEconomico))
                {
                    var queryEk = new OdbcConsultaDTO();
                    queryEk.consulta = "SELECT * FROM cc WHERE cc = ?";
                    queryEk.parametros.Add(new OdbcParameterDTO
                    {
                        nombre = "cc",
                        tipo = OdbcType.NVarChar,
                        valor = numeroEconomico
                    });
                    var ccDescripcion = _contextEnkontrol.Select<dynamic>(vSesiones.sesionAmbienteEnkontrolAdm, queryEk).FirstOrDefault();

                    if (ccDescripcion != null)
                    {
                        numeroEconomico = (string)ccDescripcion.descripcion;
                    }
                }
                else
                {
                    throw new Exception("Se tiene que indicar un CC");
                }
            }

            if (!string.IsNullOrEmpty(numeroEconomico))
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == numeroEconomico && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }
            else if (idEconomico.HasValue)
            {
                maquina = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == idEconomico.Value && x.estatus == 2); //x.estatus == 2 significa que esta en StandBy
            }

            if (maquina != null)
            {
                if (_context.tblM_STB_EconomicoBloqueado.Any(x => x.noEconomico == maquina.noEconomico && x.registroActivo))
                {
                    throw new Exception("No es posible realizar la acción puesto que el equipo referenciado se encuentra bloqueado por estatus StandBy");
                }

                var standBy = _context.tblM_STB_CapturaStandBy
                    .FirstOrDefault(x =>
                        x.noEconomicoID == maquina.id &&
                        x.estatus == 2 //Autorizado
                    );

                if (standBy != null)
                {
                    string motivoLiberacion = "";
                    maquina.estatus = 1;
                    standBy.estatus = 4; //Liberado
                    standBy.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                    standBy.fechaLibera = DateTime.Now;
                    standBy.comentarioLiberacion = "Se liberó por sistema - ";
                    switch (accion)
                    {
                        case AccionActivacionEconomicoEnum.ELABORACION_REQUISICION:
                            standBy.comentarioLiberacion += "Se realizó una requisición";
                            motivoLiberacion = "elaboración de requisición";
                            break;
                        case AccionActivacionEconomicoEnum.ELABORACION_ORDEN_COMPRA:
                            standBy.comentarioLiberacion += "Se realizó una orden de compra";
                            motivoLiberacion = "elaboración de orden de compra";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_HOROMETROS:
                            standBy.comentarioLiberacion += "Se capturó horómetros";
                            motivoLiberacion = "captura de horómetros";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_COMBUSTIBLE:
                            standBy.comentarioLiberacion += "Se capturó combustible";
                            motivoLiberacion = "captura de combustible";
                            break;
                        case AccionActivacionEconomicoEnum.CAPTURA_ACEITE:
                            standBy.comentarioLiberacion += "Se capturó aceite";
                            motivoLiberacion = "captura de aceite";
                            break;
                        case AccionActivacionEconomicoEnum.RECEPCION_FACTURA:
                            standBy.comentarioLiberacion += "Por recepción de factura";
                            motivoLiberacion = "recepción de factura";
                            break;
                        case AccionActivacionEconomicoEnum.SALIDA_ALMACEN:
                            standBy.comentarioLiberacion += "Por salida de almacén";
                            motivoLiberacion = "salida de almacén";
                            break;
                    }

                    var bitacora = new tblM_STB_BitacoraActivacionEconomico();
                    bitacora.economicoId = maquina.id;
                    bitacora.fechaAccion = DateTime.Now;
                    bitacora.motivoActivacionId = (int)accion;
                    bitacora.usuarioAccionId = vSesiones.sesionUsuarioDTO.id;
                    bitacora.objeto = JsonUtils.convertNetObjectToJson(objeto);
                    _context.tblM_STB_BitacoraActivacionEconomico.Add(bitacora);
                    _context.SaveChanges();

                    var correos = new List<string>();
                    var correosCC = new List<string>();

                    var adminsGerentes = _context.Select<Core.DTO.Maquinaria.StandBy.AutorizanteDTO>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = @"SELECT
                                                u.id,
                                                u.nombre,
                                                u.apellidoPaterno,
                                                u.apellidoMaterno,
                                                u.correo,
                                                c.cc as ac,
                                                a.perfilAutorizaID
                                            FROM
                                                tblP_Autoriza AS a
                                            INNER JOIN
                                                tblP_Usuario AS u ON u.id = a.usuarioID
                                            INNER JOIN
                                                tblP_CC_Usuario AS c ON c.id = a.cc_usuario_ID
                                            WHERE
                                                u.estatus = 1 AND
                                                a.perfilAutorizaID in (5, 1) AND /*5 == Admin, 1 == Gerente*/
                                                c.cc = @paramCC",
                        parametros = new { paramCC = standBy.ccActual }
                    });

                    correosCC.AddRange(adminsGerentes.Select(x => x.correo).Distinct().ToList());

                    correos.Add("oscar.roman@construplan.com.mx");
                    correosCC.Add("g.reina@construplan.com.mx");
                    correosCC.Add("e.encinas@construplan.com.mx");
                    correosCC.Add("luis.fortino@construplan.com.mx");
                    correosCC.Add("martin.valle@construplan.com.mx");
                    correosCC.Add("alan.palomera@construplan.com.mx");
                    correosCC.Add("diego.gonzalez@construplan.com.mx");
#if DEBUG
                    correos = new List<string> { "martin.zayas@construplan.com.mx" };
                    correosCC = new List<string> { "martin.zayas@construplan.com.mx" };
#endif
                    var cc = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == maquina.centro_costos);
                    var ccDescripcion = cc != null ? cc.descripcion.Trim() : maquina.centro_costos;
                    string asunto = "El equipo " + maquina.noEconomico + " ha sido liberado de StandBy por " + motivoLiberacion;
                    string contenido = string.Format(@"
                        <p>Buen día.</p>
                        <p>El equipo <strong>{0}</strong> ha sido liberado de StandBy por {1} </p>
                        <p>El equipo se encuentra en <strong>{2}.</strong>", maquina.noEconomico, motivoLiberacion, ccDescripcion);

                    var envioCorrecto = EnviarCorreo(new Infrastructure.DTO.CorreoDTO
                    {
                        asunto = asunto,
                        cuerpo = contenido,
                        correos = correos,
                        correosCC = correosCC
                    });

                    if (!envioCorrecto)
                    {
                        throw new Exception("Error al enviar correo de liberación de StandBy");
                    }

                    return true;
                }
                else
                {
                    throw new Exception("El económico esta en StandBy pero no se encuentra su registro autorizado");
                }
            }

            return false;
        }

        private bool EnviarCorreo(Infrastructure.DTO.CorreoDTO correo)
        {
            if (correo.correos == null || correo.correos.Count == 0 || string.IsNullOrEmpty(correo.asunto) || string.IsNullOrEmpty(correo.cuerpo))
            {
                return false;
            }

            MailMessage mailMessage = new MailMessage();

            correo.correos.ForEach(c => mailMessage.To.Add(new MailAddress(c)));
            correo.correosCC.ForEach(c => mailMessage.CC.Add(new MailAddress(c)));
            correo.archivos.ForEach(archivo => mailMessage.Attachments.Add(archivo));

            mailMessage.IsBodyHtml = true;
            mailMessage.From = new MailAddress("alertas.sigoplan@construplan.com.mx");
            mailMessage.Subject = correo.asunto;
            mailMessage.Body = string.Format(@"
                {0} 
                <p><o:p>&nbsp;</o:p></p>
                <p><o:p>&nbsp;</o:p></p>
                <p>Se informa que esta es un correo autogenerado por el sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx</a>). No es necesario dar una respuesta. Gracias.</p>
            ", correo.cuerpo);

            SmtpClient smptConfig = new SmtpClient();
            smptConfig.Send(mailMessage);
            smptConfig.Dispose();

            return true;
        }

        public bool getFolio(string folio, int tipo)
        {
            switch (tipo)
            {
                case 1:
                    return _context.tblM_CapOrdenTrabajo.FirstOrDefault(x => x.folio == folio) != null;
                case 2:
                    return _context.tblM_CapStandBy.FirstOrDefault(x => x.folio == folio) != null;
                case 3:
                    return _context.tblM_CapHorometro.FirstOrDefault(x => x.folio == folio) != null;
                case 4:
                    return _context.tblM_CapNotaCredito.FirstOrDefault(x => x.folio == folio) != null;
                default:
                    return false;
            }
        }
        #region Autorizacion
        public Dictionary<string, object> setAuth(List<tblCom_Req> lst)
        {
            var result = new Dictionary<string, object>();
            try
            {
            #region PERÚ
            var usuario = vSesiones.sesionUsuarioDTO;
            var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

            #region Validación para requisiciones con compras.
            foreach (var req in lst)
            {
                //var compraExistenteEK = _context.tblCom_OrdenCompraDet.FirstOrDefault(e => e.estatusRegistro && e.cc == req.cc && e.num_requisicion == req.numero && e.);
                var compraExistenteEK = _context.Select<tblCom_OrdenCompraDet>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = "SELECT det.* FROM tblCom_OrdenCompraDet AS det INNER JOIN tblCom_OrdenCompra AS com ON com.id = det.idOrdenCompra WHERE det.estatusRegistro = 1 AND det.cc = @paramCC AND det.num_requisicion = @paramNumero",
                    parametros = new { paramCC = req.cc, paramNumero = req.numero, paramTipoCompra = req.PERU_tipoRequisicion }
                }).ToList().FirstOrDefault();

                if (compraExistenteEK != null)
                {

                    if (_context.tblCom_OrdenCompra.Any(e => e.estatusRegistro && e.id == compraExistenteEK.idOrdenCompra))
                    {
                        throw new Exception("No se puede autorizar/desautorizar la requisición \"" + req.cc + "-" + req.numero + "\". Ya tiene orden de compra.");
                    }
                }
            }
            #endregion

            #region Validación para responsables de centros de costos.
            if (relUser.empleado != 1)
            {
                var listaAutorizantesCentroCosto = _context.tblCom_AutorizanteCentroCosto.Where(x => x.registroActivo).ToList();

                foreach (var requisicion in lst)
                {
                    var registroCentroCosto = _context.tblP_CC.FirstOrDefault(x => x.cc == requisicion.cc);
                    var autorizantesCentroCosto = listaAutorizantesCentroCosto.Where(x => x.cc == registroCentroCosto.ccRH).ToList();
                    var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == requisicion.cc && x.numero == requisicion.numero);

                    if (!autorizantesCentroCosto.Any(x => x.empleado == relUser.empleado))
                    {
                        throw new Exception("No es responsable del centro de costo \"" + requisicion.cc + "\".");
                    }
                }
            }
            #endregion

            #region Autorizar Requisiciones
            foreach (var requisicion in lst)
            {
                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(r => r.estatusRegistro && r.cc == requisicion.cc && r.numero == requisicion.numero);

                if (requisicionSIGOPLAN != null)
                {
                    #region SIGOPLAN
                    requisicionSIGOPLAN.stAutoriza = requisicion.stAutoriza;
                    requisicionSIGOPLAN.empAutoriza = relUser.empleado;
                    requisicionSIGOPLAN.autoriza = DateTime.Now;
                    requisicionSIGOPLAN.empleadoUltimaAccion = relUser.empleado;
                    requisicionSIGOPLAN.fechaUltimaAccion = DateTime.Now;
                    requisicionSIGOPLAN.tipoUltimaAccion = requisicion.stAutoriza ? TipoUltimaAccionEnum.Autorizacion : TipoUltimaAccionEnum.Desautorizacion;

                    _context.SaveChanges();
                    #endregion
                }
                else
                {
                    throw new Exception("No se encuentra la requisición en SIGOPLAN.");
                }

            }
            #endregion
            #endregion

            result.Add(SUCCESS, true);
            return result;
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
                return result;
            }
        }
        #endregion
        #endregion
        public Dictionary<string, object> getNewReq(string cc, string tpRequi)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);
                // var ultimaRequisicion = consultaCheckProductivo(string.Format(@"SELECT TOP 1 numero FROM so_requisicion WHERE cc = '{0}' ORDER BY numero DESC", cc));
                //linq
                var ultimaRequisicion = _context.tblCom_Req.Where(x => x.estatusRegistro && x.cc == cc).OrderByDescending(x => x.numero).FirstOrDefault();
                var numeroUltimaRequisicion = ultimaRequisicion != null ?ultimaRequisicion.numero : 0;
                // var empleado = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM empleados where empleado = {0}", relUser.empleado)).ToObject<List<dynamic>>())[0];
                var empleado = _context.tblRH_EK_Empleados.Where(x => x.id == relUser.empleado).FirstOrDefault();
                result.Add("solicito", empleado.clave_empleado);
                result.Add("solicitoNom", empleado.nombre);
                result.Add("numero", numeroUltimaRequisicion + 1);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add("numero", 0);
                result.Add("solicitoNom", "Default");
                result.Add("solicito", 0);
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        public dynamic getUltimaRequisicionSIGOPLAN(string cc)
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                var requisicion = _context.tblCom_Req.Where(x => x.estatusRegistro && x.cc == cc && x.stAutoriza && (x.validadoAlmacen == false || x.validadoAlmacen == null)).OrderByDescending(x => x.numero).FirstOrDefault();

                //Consulta a EnKontrol para comprobar que exista y coincida la información de la requisición.
                return requisicion.numero;
            }
            catch (Exception) { return 0; }
        }
        public Dictionary<string, object> getRequisicion(string cc, int num, bool esServicio = false)
        {
            var result = new Dictionary<string, object>();
            try
            {

                #region TODAS LAS EMPRESAS MENOS PERU Y COLOMBIA
                var requisicionSIGOPLAN = getRequisicionSIGOPLAN(cc, num);
                // var requisicionEK = consultaCheckProductivo(
                //     string.Format(@"SELECT 
                //                 r.*, 
                //                 s.descripcion as solicitoNom, 
                //                 e.descripcion as empModificaNom, 
                //                 v.descripcion as voboNom, 
                //                 a.descripcion as empAutNom 
                //             FROM so_requisicion r 
                //                 LEFT JOIN empleados s ON s.empleado = r.solicito 
                //                 LEFT JOIN empleados v ON v.empleado = r.vobo 
                //                 LEFT JOIN empleados e ON e.empleado = r.empleado_modifica 
                //                 LEFT JOIN empleados a ON a.empleado = r.emp_autoriza 
                //             WHERE r.cc = '{0}' AND r.numero = {1}", cc, num));

                var requisicionEK = (
                    from r in _context.tblCom_Req

                    // LEFT JOIN tblCom_ReqTipo (si no existe relación en ReqTipo, r aún será seleccionado)
                    join tr in _context.tblCom_ReqTipo
                        on r.idTipoReqOc equals tr.tipo_req_oc into trGroup
                    from tr in trGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "solicito"
                    join s in _context.tblRH_EK_Empleados
                        on r.solicito equals s.clave_empleado into sGroup
                    from s in sGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "vobo"
                    join v in _context.tblRH_EK_Empleados
                        on r.vobo equals v.clave_empleado into vGroup
                    from v in vGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "empModifica"
                    join e in _context.tblRH_EK_Empleados
                        on r.empModifica equals e.clave_empleado into eGroup
                    from e in eGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "empAutoriza"
                    join a in _context.tblRH_EK_Empleados
                        on r.empAutoriza equals a.clave_empleado into aGroup
                    from a in aGroup.DefaultIfEmpty()

                    where r.cc == cc && r.numero == num

                    select new
                    {
                        r, // Todos los campos de tblCom_Req
                        tr, // Todos los campos de tblCom_ReqTipo, o null si no hay coincidencia
                        solicitoNom = s != null ? s.nombre : null, // si no existe coincidencia, asigna null
                        empModificaNom = e != null ? e.nombre : null,
                        voboNom = v != null ? v.nombre : null,
                        empAutorizaNom = a != null ? a.nombre : null
                    }
                ).ToList();


                if (requisicionEK.Count > 0)
                {
                    var requisicion = requisicionEK[0];

                    // var partidasRequisicionEK = consultaCheckProductivo(
                    // string.Format(@"SELECT 
                    //             d.*, 
                    //             i.descripcion as insumoDesc, 
                    //             i.unidad, 
                    //             i.cancelado, 
                    //             i.compras_req, 
                    //             d.descripcion as partidaDesc 
                    //         FROM so_requisicion_det d 
                    //             INNER JOIN insumos i ON d.insumo = i.insumo 
                    //         WHERE d.cc = '{0}' AND d.numero = {1} 
                    //         ORDER BY d.partida", cc, num));

                    var partidasRequisicionEK = (
                        from d in _context.tblCom_ReqDet
                        join c in _context.tblCom_Req
                            on d.idReq equals c.id
                        join i in _context.tblAlm_Insumo
                            on d.insumo equals i.insumo
                        where c.cc == cc && c.numero == num && d.estatusRegistro
                        orderby d.partida
                        select new
                        {
                            d, // Todos los campos de tblCom_ReqDet
                            c,
                            insumoDesc = i.descripcion,
                            i.unidad,
                            i.cancelado,
                            i.compras_req,
                            partidaDesc = d.descripcion // Puedes proyectar d.descripcion como partidaDesc si es necesario
                        }
                    ).ToList();


                    if (partidasRequisicionEK != null)
                    {
                        // var partidasRequisicion = (List<dynamic>)partidasRequisicionEK.ToObject<List<dynamic>>();
                        List<string> compras = new List<string>();
                        // var comprasEK = consultaCheckProductivo(
                        //     string.Format(@"SELECT 
                        //                 cc, numero, (cc + '-' + CONVERT(varchar(10), numero)) AS compra 
                        //             FROM so_orden_compra_det 
                        //             WHERE cc = '{0}' AND num_requisicion = {1} AND cantidad > 0 
                        //             GROUP BY cc, numero", (string)requisicion.r.cc, (int)requisicion.r.numero)
                        // );

                        var comprasEK = (
                            from det in _context.tblCom_OrdenCompraDet
                            join oc in _context.tblCom_OrdenCompra
                                on det.idOrdenCompra equals oc.id
                            where oc.cc == requisicion.r.cc 
                                && det.num_requisicion == requisicion.r.numero
                                && det.cantidad > 0
                            group det by new { det.cc, det.numero } into g
                            select new
                            {
                                cc = g.Key.cc,
                                numero = g.Key.numero,
                                compra = g.Key.cc + "-" + g.Key.numero.ToString() // Concatenación similar a SQL
                            }
                        ).ToList();


                        if (comprasEK != null)
                        {
                            compras = comprasEK.Select(x => (string)x.compra).ToList();
                        }

                        var requisicionInfo = new
                        {
                            id = 0,
                            cc = (string)requisicion.r.cc,
                            numero = (int?)requisicion.r.numero,
                            fecha = (DateTime?)requisicion.r.fecha,
                            fechaString = ((DateTime)requisicion.r.fecha).ToShortDateString(),
                            libre_abordo = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.libre_abordo : 0,
                            tipo_req_oc = (string)requisicion.tr.tipo_req_oc.ToString(),
                            solicito = (int?)requisicion.r.solicito,
                            vobo = (int?)requisicion.r.vobo,
                            autorizo = (int?)requisicion.r.autorizo,
                            comentarios = (string)requisicion.r.comentarios,
                            st_estatus = (string)requisicion.r.stEstatus,
                            st_autoriza = requisicion.r.stAutoriza ? "S": "N",
                            empleado_modifica = (int?)requisicion.r.empModifica,
                            fecha_modifica = (DateTime?)requisicion.r.fecha,
                            fecha_modificaString = ((DateTime?)requisicion.r.fecha).Value.Date.ToShortDateString(),
                            hora_modifica = (DateTime?)requisicion.r.fecha,
                            hora_modificaString = ((DateTime?)requisicion.r.fecha).Value.TimeOfDay.ToString(),
                            fecha_autoriza = (DateTime?)requisicion.r.autoriza,
                            tmc = 0,
                            autoriza_activos = requisicion.r.isActivos?1:0,
                            num_vobo = (int?)requisicion.r.numVobo,
                            solicitoNom = (string)requisicion.solicitoNom,
                            empModificaNom = (string)requisicion.empModificaNom,
                            voboNom = (string)requisicion.voboNom,
                            empAutNom = (string)requisicion.empAutorizaNom,
                            st_impresa = requisicion.r.stImpresa ? "S": "",
                            folioOrigen = getFolioOrigen(cc, num),
                            consigna = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.consigna : false,
                            licitacion = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.licitacion : false,
                            crc = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.crc : false,
                            convenio = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.convenio : false,
                            proveedor = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.proveedor : 0,
                            validadoAlmacen = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.validadoAlmacen != null ? requisicionSIGOPLAN.validadoAlmacen : false : false,
                            comprador = requisicionSIGOPLAN != null ? (requisicionSIGOPLAN.comprador ?? 0) : 0,
                            compras = compras,
                            comprasString = string.Join(", ", compras),
                            usuarioSolicita = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.usuarioSolicita : 0,
                            usuarioSolicitaDesc = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.usuarioSolicitaDesc : "",
                            usuarioSolicitaUso = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.usuarioSolicitaUso : "",
                            usuarioSolicitaEmpresa = requisicionSIGOPLAN != null ? requisicionSIGOPLAN.usuarioSolicitaEmpresa : 0,
                            otroCC = requisicionSIGOPLAN != null && requisicionSIGOPLAN.cc != cc,
                            otID = requisicion.r.otID,
                            otFolio = requisicion.r.otFolio,
                        };

                        var requisicionPartidas = new List<object>();

                        var surtidos = getSurtidoPorReq(cc, num);

                        foreach (var p in partidasRequisicionEK)
                        {
                            var observaciones = "";
                            var comentarioSurtidoQuitar = "";
                            var cantidadCapturada = default(decimal);
                            var precio = 0m;

                            if (requisicionSIGOPLAN != null)
                            {
                                var partidaSIGOPLAN = requisicionSIGOPLAN.partidas.FirstOrDefault(x => x.partida == (int)p.d.partida && x.insumo == (int)p.d.insumo);

                                if (partidaSIGOPLAN != null)
                                {
                                    precio = partidaSIGOPLAN.precio;
                                    observaciones = partidaSIGOPLAN.observaciones;
                                }
                            }

                            if (surtidos.Count > 0)
                            {
                                var surtidoPartida = surtidos.Where(x => x.partidaRequisicion == (int)p.d.partida && x.insumo == (int)p.d.insumo).ToList();

                                if (surtidoPartida.Count() > 0)
                                {
                                    cantidadCapturada = surtidoPartida.Select(y => y.cantidad).Sum();
                                }
                            }

                            requisicionPartidas.Add(new
                            {
                                id = 0,
                                idReq = 0,
                                cc = (string)p.c.cc,
                                numero = (int?)p.c.numero,
                                partida = (int?)p.d.partida,
                                insumo = (int?)p.d.insumo,
                                insumoDesc = (string)p.insumoDesc,
                                unidad = (string)p.unidad,
                                cancelado = (string)p.cancelado,
                                fecha_requerido = (DateTime?)p.c.fecha,
                                cantidad = (decimal?)p.d.cantidad - (decimal?)p.d.cantCancelada,
                                precio = precio,
                                cant_ordenada = (decimal?)p.d.cantOrdenada,
                                fecha_ordenada = (DateTime?)p.c.fecha,
                                estatus = (string)p.d.estatus,
                                cant_cancelada = (decimal?)p.d.cantCancelada,
                                referencia_1 = "",
                                cantidad_excedida_ppto = 0,
                                area = (int?)p.d.area,
                                cuenta = (int?)p.d.cuenta,
                                compras_req = 1,
                                partidaDesc = (string)p.d.descripcion + (comentarioSurtidoQuitar.Count() > 0 ? " ***PARTIDA CANCELADA EN SURTIDO: " + comentarioSurtidoQuitar + "***" : ""),
                                observaciones = observaciones,
                                comentarioSurtidoQuitar = comentarioSurtidoQuitar,
                                cantidadCapturada = cantidadCapturada,
                                tipoPartida = p.d.tipoPartida,
                                tipoPartidaDet = p.d.tipoPartidaDet,
                            });
                        }

                        result.Add("req", requisicionInfo);
                        result.Add("partidas", requisicionPartidas);
                        result.Add("requisicionNueva", false);
                    }
                    else
                    {
                        throw new Exception("No se encontraron partidas para esta requisición.");
                    }
                }
                else
                {
                    var ultimaRequisicion = getUltimaRequisicionNumero(cc);

                    result.Add("requisicionNueva", true);
                    result.Add("ultimaRequisicionNumero", ultimaRequisicion);
                }
                
                #endregion
            
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        public Dictionary<string, object> getReq(string cc, int num)
        {
            var result = new Dictionary<string, object>();

            try
            {
//                var requisicionEK = consultaCheckProductivo(
//                    string.Format(@"SELECT 
//                                    r.*, 
//                                    s.descripcion as solicitoNom, 
//                                    e.descripcion as empModificaNom, 
//                                    v.descripcion as voboNom, 
//                                    a.descripcion as empAutNom 
//                                FROM so_requisicion r 
//                                    LEFT JOIN empleados s ON s.empleado = r.solicito 
//                                    LEFT JOIN empleados v ON v.empleado = r.vobo 
//                                    LEFT JOIN empleados e ON e.empleado = r.empleado_modifica 
//                                    LEFT JOIN empleados a ON a.empleado = r.emp_autoriza 
//                                WHERE r.cc = '{0}' AND r.numero = {1}", cc, num)
//                );
                var requisicionEK = (
                    from r in _context.tblCom_Req

                    // LEFT JOIN tblCom_ReqTipo (si no existe relación en ReqTipo, r aún será seleccionado)
                    join tr in _context.tblCom_ReqTipo
                        on r.idTipoReqOc equals tr.tipo_req_oc into trGroup
                    from tr in trGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "solicito"
                    join s in _context.tblRH_EK_Empleados
                        on r.solicito equals s.clave_empleado into sGroup
                    from s in sGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "vobo"
                    join v in _context.tblRH_EK_Empleados
                        on r.vobo equals v.clave_empleado into vGroup
                    from v in vGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "empModifica"
                    join e in _context.tblRH_EK_Empleados
                        on r.empModifica equals e.clave_empleado into eGroup
                    from e in eGroup.DefaultIfEmpty()

                    // LEFT JOIN tblRH_EK_Empleados para "empAutoriza"
                    join a in _context.tblRH_EK_Empleados
                        on r.empAutoriza equals a.clave_empleado into aGroup
                    from a in aGroup.DefaultIfEmpty()

                    where r.cc == cc && r.numero == num

                    select new
                    {
                        r, // Todos los campos de tblCom_Req
                        tr, // Todos los campos de tblCom_ReqTipo, o null si no hay coincidencia
                        solicitoNom = s != null ? s.nombre : null, // si no existe coincidencia, asigna null
                        empModificaNom = e != null ? e.nombre : null,
                        voboNom = v != null ? v.nombre : null,
                        empAutorizaNom = a != null ? a.nombre : null
                    }
                ).ToList();

                if (requisicionEK.Count > 0)
                {
                    var requisicion = requisicionEK[0];
//                    var partidas = (List<dynamic>)consultaCheckProductivo(
//                        string.Format(@"SELECT 
//                                        d.*, 
//                                        i.descripcion as insumoDesc, 
//                                        i.unidad, 
//                                        i.cancelado, 
//                                        i.compras_req, 
//                                        (
//                                            SELECT 
//                                                l.descripcion 
//                                            FROM so_req_det_linea l 
//                                            WHERE l.cc = d.cc AND l.numero = d.numero AND l.partida = d.partida 
//                                        ) as partidaDesc 
//                                    FROM so_requisicion_det d 
//                                        INNER JOIN insumos i ON d.insumo = i.insumo 
//                                    WHERE d.cc = '{0}' AND d.numero = {1} 
//                                    ORDER BY d.partida", cc, num)
//                    ).ToObject<List<dynamic>>();
                    var partidas = (
                        from d in _context.tblCom_ReqDet
                        join c in _context.tblCom_Req
                            on d.idReq equals c.id
                        join i in _context.tblAlm_Insumo
                            on d.insumo equals i.insumo
                        where c.cc == cc && c.numero == num
                        orderby d.partida
                        select new
                        {
                            d, // Todos los campos de tblCom_ReqDet
                            c,
                            insumoDesc = i.descripcion,
                            i.unidad,
                            i.cancelado,
                            i.compras_req,
                            partidaDesc = d.descripcion // Puedes proyectar d.descripcion como partidaDesc si es necesario
                        }
                    ).ToList();

                    var requisicionInfo = new RequisicionDTO
                    {
                        id = 0,
                        cc = (string)requisicion.r.cc,
                        numero = requisicion.r.numero,
                        fecha = requisicion.r.fecha,
                        libre_abordo = 0,
                        tipo_req_oc = (string)requisicion.tr.tipo_req_oc.ToString(),
                        solicito = requisicion.r.solicito,
                        vobo = requisicion.r.vobo,
                        autorizo = requisicion.r.autorizo,
                        comentarios = (string)requisicion.r.comentarios,
                        st_estatus = (string)requisicion.r.stEstatus,
                        st_autoriza = requisicion.r.stAutoriza ? "S" : "N",
                        empleado_modifica = (int?)requisicion.r.empModifica,
                        fecha_modifica = (DateTime?)requisicion.r.fecha,
                        fecha_modificaString = ((DateTime?)requisicion.r.fecha).Value.Date.ToShortDateString(),
                        hora_modifica = (DateTime?)requisicion.r.fecha,
                        hora_modificaString = ((DateTime?)requisicion.r.fecha).Value.TimeOfDay.ToString(),
                        fecha_autoriza = (DateTime?)requisicion.r.autoriza,
                        tmc = 0,
                        autoriza_activos = requisicion.r.isActivos ? 1 : 0,
                        num_vobo = (int?)requisicion.r.numVobo,
                        solicitoNom = (string)requisicion.solicitoNom,
                        empModificaNom = (string)requisicion.empModificaNom,
                        voboNom = (string)requisicion.voboNom,
                        empAutNom = (string)requisicion.empAutorizaNom,
                        st_impresa = requisicion.r.stImpresa ? "S" : "",

                        folioOrigen = getFolioOrigen(cc, num),
                        consigna = false,
                        licitacion = false,
                        crc = false,
                        convenio = false,
                        validadoAlmacen = false,
                        comprador = 0
                    };

                    List<dynamic> listaPartidas = new List<dynamic>();

                    var surtidos = getSurtidoPorReq(cc, num);

                    var requisicionSIGOPLAN = getRequisicionSIGOPLAN(cc, num);

                    if (requisicionSIGOPLAN != null)
                    {
                        requisicionInfo.libre_abordo = requisicionSIGOPLAN.libre_abordo;
                        requisicionInfo.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                        requisicionInfo.licitacion = requisicionSIGOPLAN.licitacion;
                        requisicionInfo.crc = requisicionSIGOPLAN.crc;
                        requisicionInfo.convenio = requisicionSIGOPLAN.convenio;
                        requisicionInfo.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen != null ? (bool)requisicionSIGOPLAN.validadoAlmacen : false;
                        requisicionInfo.comprador = requisicionSIGOPLAN.comprador ?? 0;
                        requisicionInfo.fechaSurtidoCompromiso = requisicionSIGOPLAN.fechaSurtidoCompromiso;
                        requisicionInfo.fechaSurtidoCompromisoString =
                            requisicionSIGOPLAN.fechaSurtidoCompromiso != null ? ((DateTime)requisicionSIGOPLAN.fechaSurtidoCompromiso).ToShortDateString() : "";

                        foreach (var part in listaPartidas)
                        {
                            part.observaciones =
                                requisicionSIGOPLAN.partidas.FirstOrDefault(x => x.insumo == (int)part.insumo) != null ?
                                requisicionSIGOPLAN.partidas.FirstOrDefault(x => x.insumo == (int)part.insumo).observaciones : "";
                        }
                    }

                    foreach (var part in partidas)
                    {
                        var listaSurtidoPartida = surtidos.Where(x => x.partidaRequisicion == (int)part.d.partida).ToList();
                        List<SurtidoDetDTO> listaSurtido = new List<SurtidoDetDTO>();

                        foreach (var sur in listaSurtidoPartida)
                        {
                            listaSurtido.Add(new SurtidoDetDTO
                            {
                                almacenID = sur.almacenOrigenID,
                                area_alm = sur.area_alm,
                                lado_alm = sur.lado_alm,
                                estante_alm = sur.estante_alm,
                                nivel_alm = sur.nivel_alm,
                                aSurtir = sur.cantidad,
                            });
                        }

                        decimal existenciaTotal = default(decimal);
                        decimal existenciaLAB = default(decimal);

                        // var listaExistenciaEK = consultaCheckProductivo(
                        //     string.Format(@"SELECT 
                        //                     mov.almacen, 
                        //                     det.insumo, 
                        //                     det.area_alm, 
                        //                     det.lado_alm, 
                        //                     det.estante_alm, 
                        //                     det.nivel_alm, 
                        //                     SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                        //                     SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                        //                     SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                        //                 FROM si_movimientos mov 
                        //                     INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                        //                     INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                        //                 WHERE 
                        //                     det.insumo = {0} 
                        //                 GROUP BY mov.almacen, det.insumo, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", part.insumo)
                        // );

                        var listaExistenciaEK = (
                            from mov in _context.tblAlm_Movimientos
                            join det in _context.tblAlm_MovimientosDet
                                on new { mov.almacen, mov.tipo_mov, mov.numero } equals new { det.almacen, det.tipo_mov, det.numero }
                            join alm in _context.tblAlm_Almacen
                                on mov.almacen equals alm.almacen
                            where det.insumo == part.d.insumo // El filtro por insumo
                            group new { mov, det } by new 
                            {
                                mov.almacen,
                                det.insumo,
                                det.area_alm,
                                det.lado_alm,
                                det.estante_alm,
                                det.nivel_alm
                            } into g
                            select new
                            {
                                Almacen = g.Key.almacen,
                                Insumo = g.Key.insumo,
                                AreaAlmacen = g.Key.area_alm,
                                LadoAlmacen = g.Key.lado_alm,
                                EstanteAlmacen = g.Key.estante_alm,
                                NivelAlmacen = g.Key.nivel_alm,
                                Entradas = g.Sum(x => (new[] { 1, 2, 3, 4, 5 }.Contains(x.mov.tipo_mov) ? x.det.cantidad : 0)),
                                Salidas = g.Sum(x => (new[] { 51, 52, 53, 54, 55 }.Contains(x.mov.tipo_mov) ? x.det.cantidad : 0)),
                                Existencia = g.Sum(x => x.det.cantidad * (new[] { 1, 2, 3, 4, 5 }.Contains(x.mov.tipo_mov) ? 1 : -1))
                            }
                        ).ToList();

                        if (listaExistenciaEK.Count > 0)
                        {
                            var listaExistencia = listaExistenciaEK;

                            listaExistencia = listaExistencia.Where(x => (decimal)x.Existencia > 0).ToList();

                            existenciaTotal = listaExistencia.Where(x => (int)x.Almacen < 900).Sum(x => x.Existencia > 0 ? (decimal)x.Existencia : default(decimal));
                            existenciaLAB = listaExistencia.Where(x =>
                                (int)x.Almacen == requisicionSIGOPLAN.libre_abordo
                                ).Sum(x => x.Existencia > 0 ? (decimal)x.Existencia : default(decimal));
                        }

                        listaPartidas.Add(new RequisicionDetDTO
                        {
                            id = 0,
                            idReq = 0,
                            cc = (string)part.c.cc,
                            numero = (int)part.c.numero,
                            partida = (int)part.d.partida,
                            insumo = (int)part.d.insumo,
                            insumoDesc = (string)part.insumoDesc,
                            unidad = (string)part.unidad,
                            cancelado = (string)part.cancelado,
                            fecha_requerido = (DateTime)part.c.fechaSurtidoCompromiso,
                            cantidad = (decimal)part.d.cantidad - (decimal)part.d.cantCancelada,
                            cant_ordenada = (decimal)part.d.cantOrdenada,
                            fecha_ordenada = (DateTime?)part.c.fecha,
                            estatus = (string)part.c.stEstatus,
                            cant_cancelada = (decimal)part.d.cantCancelada,
                            referencia_1 = "",
                            cantidad_excedida_ppto = 0,
                            area = (int)part.d.area,
                            cuenta = (int)part.d.cuenta,
                            compras_req = (int?)part.compras_req,
                            partidaDesc = (string)part.partidaDesc,
                            observaciones = "",
                            cantidadCapturada =
                                (surtidos.Count > 0 && surtidos.Where(x => x.insumo == (int)part.d.insumo).ToList().Count > 0) ?
                                (surtidos.Where(x => x.insumo == (int)part.d.insumo).Select(y => y.cantidad).Sum()) : 0,
                            listaSurtido = listaSurtido,
                            totalASurtir = listaSurtido.Select(x => x.aSurtir).Sum(),
                            existenciaTotal = existenciaTotal,
                            existenciaLAB = existenciaLAB,
                            validadoAlmacen = requisicionInfo.validadoAlmacen ?? false
                        });
                    }

                    result.Add("req", requisicionInfo);
                    result.Add("partidas", listaPartidas);
                    result.Add("requisicionNueva", false);

                }
                else
                {
                    result.Add("requisicionNueva", true);
                    result.Add("ultimaRequisicionNumero", getUltimaRequisicionNumero(cc));
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        public dynamic getInsumos_old(string term, string cc, bool esServicio = false)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var listaInsumos = new List<InsumoRequisicionDTO>();

                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ACODIGO.Contains(term) && (esServicio ? x.ACODIGO.Substring(0, 2) == "02" : x.ACODIGO.Substring(0, 2) == "01")).Take(12).ToList().Select(x => new InsumoRequisicionDTO
                                {
                                    id = x.ADESCRI,
                                    value = x.ACODIGO,
                                    unidad = x.AUNIDAD,
                                    exceso = 0,
                                    isAreaCueta = false,
                                    cancelado = "A",
                                    costoPromedio = "0",
                                    color_resguardo = 0,
                                    compras_req = 1,
                                    ultimaCompra = 0
                                }).Where(x => x.value != "0").ToList();
                            }

                            return listaInsumos;
                        }
                    case EmpresaEnum.Colombia:
                        {
                            var res = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT TOP 12 
                                                i.insumo, 
                                                i.descripcion, 
                                                i.unidad, 
                                                ( 
                                                    SELECT TOP 1 
                                                        m.cant_requerida 
                                                    FROM so_explos_mat m 
                                                    WHERE 
                                                        m.insumo = i.insumo AND 
                                                        m.cc = '{1}' 
                                                    ORDER BY year_explos DESC 
                                                ) AS cant_requerida, 
                                                ( 
                                                    ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 ) 
                                                ) AS costo_promedio, 
                                                i.cancelado
                                            FROM insumos i 
                                            WHERE i.insumo LIKE '{0}%' 
                                            ORDER BY i.insumo", term, cc)).ToObject<List<dynamic>>();

                            //TODO POR INSUMO NUMERO
                            #region SE OBTIENE EL ULTIMO PRECIO DEL INSUMO COMPRADO
                            string strQueryInsumos = string.Empty;
                            List<string> lstInsumos = new List<string>();
                            for (int i = 0; i < res.Count(); i++)
                            {
                                int insumo = 0;
                                insumo = res[i].insumo;
                                if (i == 0)
                                    strQueryInsumos += "insumo = " + insumo;
                                else
                                    strQueryInsumos += " OR insumo = " + insumo;
                            }

                            List<Core.DTO.Enkontrol.Alamcen.InsumoDTO> resUltimaCompra = new List<Core.DTO.Enkontrol.Alamcen.InsumoDTO>();
                            string strQuery = @"SELECT TOP {2} insumo, precio FROM DBA.so_orden_compra_det WHERE cc = '{0}' AND ({1}) ORDER BY fecha_entrega DESC";
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery, cc, strQueryInsumos, res.Count());
                            resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(EnkontrolEnum.ArrenProd, odbc);
                            //resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(productivo ? EnkontrolEnum.ArrenProd : EnkontrolEnum.PruebaArrenADM, odbc);
                            #endregion

                            var lstResultado = res.Select(x => new
                            {
                                id = (string)x.descripcion,
                                value = (string)x.insumo,
                                unidad = (string)x.unidad,
                                exceso = (decimal?)x.cant_requerida,
                                cancelado = (string)x.cancelado,
                                costoPromedio = (string)x.costo_promedio,
                                color_resguardo = 0,
                                compras_req = 1,
                                ultimaCompra = resUltimaCompra.Count() > 0 ? resUltimaCompra.Where(w => w.insumo == (int)x.insumo).Select(s => s.precio).FirstOrDefault() : 0
                            }).ToList();

                            return lstResultado;
                        }
                    default:
                        {
                            var res = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT TOP 12 
                                                i.insumo, 
                                                i.descripcion, 
                                                i.unidad, 
                                                i.bit_area_cta, 
                                                ( 
                                                    SELECT TOP 1 
                                                        m.cant_requerida 
                                                    FROM so_explos_mat m 
                                                    WHERE 
                                                        m.insumo = i.insumo AND 
                                                        m.cc = '{1}' 
                                                    ORDER BY year_explos DESC 
                                                ) AS cant_requerida, 
                                                ( 
                                                    ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 ) 
                                                ) AS costo_promedio, 
                                                i.cancelado, 
                                                i.color_resguardo, 
                                                i.compras_req
                                            FROM insumos i 
                                            WHERE i.insumo LIKE '{0}%' 
                                            ORDER BY i.insumo", term, cc)).ToObject<List<dynamic>>();

                            //TODO POR INSUMO NUMERO
                            #region SE OBTIENE EL ULTIMO PRECIO DEL INSUMO COMPRADO
                            string strQueryInsumos = string.Empty;
                            List<string> lstInsumos = new List<string>();
                            for (int i = 0; i < res.Count(); i++)
                            {
                                int insumo = 0;
                                insumo = res[i].insumo;
                                if (i == 0)
                                    strQueryInsumos += "insumo = " + insumo;
                                else
                                    strQueryInsumos += " OR insumo = " + insumo;
                            }

                            List<Core.DTO.Enkontrol.Alamcen.InsumoDTO> resUltimaCompra = new List<Core.DTO.Enkontrol.Alamcen.InsumoDTO>();
                            string strQuery = @"SELECT TOP {2} insumo, precio FROM DBA.so_orden_compra_det WHERE cc = '{0}' AND ({1}) ORDER BY fecha_entrega DESC";
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery, cc, strQueryInsumos, res.Count());
                            resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(EnkontrolEnum.ArrenProd, odbc);
                            //resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(productivo ? EnkontrolEnum.ArrenProd : EnkontrolEnum.PruebaArrenADM, odbc);
                            #endregion

                            var lstResultado = res.Select(x => new
                            {
                                id = (string)x.descripcion,
                                value = (string)x.insumo,
                                unidad = (string)x.unidad,
                                exceso = (decimal?)x.cant_requerida,
                                isAreaCueta = (bool)x.bit_area_cta,
                                cancelado = (string)x.cancelado,
                                costoPromedio = (string)x.costo_promedio,
                                color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                                compras_req = (int?)x.compras_req,
                                ultimaCompra = resUltimaCompra.Count() > 0 ? resUltimaCompra.Where(w => w.insumo == (int)x.insumo).Select(s => s.precio).FirstOrDefault() : 0
                            }).ToList();

                            return lstResultado;
                        }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public dynamic getInsumos(string term, string cc, bool esServicio = false)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    default:
                        {
                            var listaInsumos = new List<InsumoRequisicionDTO>();

                            using (var ctx = new MainContext())
                            {
                                listaInsumos = ctx.tblAlm_Insumo
                                    .Where(x => x.insumo.ToString().Contains(term))
                                    .Take(12)
                                    .Select(x => new InsumoRequisicionDTO
                                    {
                                        id = x.insumo.ToString(),
                                        value = x.insumo.ToString(),
                                        descripcion = x.descripcion,
                                        unidad = x.unidad,
                                        exceso = 0,
                                        isAreaCueta = false,
                                        cancelado = x.cancelado,
                                        costoPromedio = "0",
                                        color_resguardo = x.color_resguardo,
                                        compras_req = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                                        ultimaCompra = 0
                                    })
                                    .Where(x => x.value != "0")  // Este filtro ahora se aplica sobre la lista en memoria
                                    .ToList();
                            }
                            foreach (var item in listaInsumos)
                            {
                                using (var ctx = new MainContext())
                                {

                                    var resUltimaCompra = ctx.tblCom_OrdenCompraDet
                                        .Where(x => x.cc == cc && x.insumo.ToString().Equals(item.value))
                                        .OrderByDescending(x => x.fecha_entrega)
                                        .Take(1)
                                        .Select(x => x.precio)
                                        .FirstOrDefault();

                                        item.ultimaCompra = resUltimaCompra;


                                }
                            }

                            return listaInsumos;
                        }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public dynamic getInsumosDesc(string term, string cc, bool esServicio = false)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    default:
                        {
                            var listaInsumos = new List<InsumoRequisicionDTO>();

                            using (var ctx = new MainContext())
                            {
                                listaInsumos = ctx.tblAlm_Insumo
                                    .Where(x => x.descripcion.Contains(term))
                                    .Take(12)
                                    .Select(x => new InsumoRequisicionDTO
                                    {
                                        id = x.insumo.ToString(),
                                        value = x.insumo.ToString() + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                        descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                        unidad = x.unidad,
                                        exceso = 0,
                                        isAreaCueta = false,
                                        cancelado = x.cancelado,
                                        costoPromedio = "0",
                                        color_resguardo = x.color_resguardo,
                                        compras_req = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                                        ultimaCompra = 0
                                    })
                                    .Where(x => x.value != "0")  // Este filtro ahora se aplica sobre la lista en memoria
                                    .ToList();
                            }
                            foreach (var item in listaInsumos)
                            {
                                using (var ctx = new MainContext())
                                {

                                    var resUltimaCompra = ctx.tblCom_OrdenCompraDet
                                        .Where(x => x.cc == cc && x.insumo.ToString().Equals(item.value))
                                        .OrderByDescending(x => x.fecha_entrega)
                                        .Take(1)
                                        .Select(x => x.precio)
                                        .FirstOrDefault();

                                        item.ultimaCompra = resUltimaCompra;


                                }
                            }

                            return listaInsumos;
                        }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public dynamic getInsumosByAlmacen(string term, string cc, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        --i.bit_area_cta, 
                                        ( 
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM DBA.so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida, 
                                        ( 
                                            ISNULL ( (SELECT ROUND(AVG(det.precio),1) FROM DBA.si_movimientos_det det WHERE det.almacen={2} AND det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado
                                        --i.color_resguardo, 
                                        --i.compras_req 
                                    FROM DBA.insumos i 
                                    WHERE i.insumo LIKE '{0}%' 
                                    ORDER BY i.insumo", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo,
                        unidad = (string)x.unidad,
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = false, //(bool)x.bit_area_cta,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                        compras_req = x.compras_req != null ? (int)x.compras_req : 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ACODIGO.Contains(term)).Take(12).Select(x => new InsumoRequisicionDTO
                        {
                            id = x.ADESCRI,
                            value = x.ACODIGO,
                            unidad = x.AUNIDAD,
                            exceso = 0,
                            isAreaCueta = false,
                            cancelado = "A",
                            costoPromedio = "0",
                            costoPromedioEntrada = 0,
                            costo = 0,
                            color_resguardo = 0,
                            compras_req = 1,
                            ultimaCompra = 0
                        }).ToList();

                        foreach (var insumo in listaInsumos)
                        {
                            if (almacen < 90)
                            {
                                var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == insumo.value);

                                if (registroStock != null)
                                {
                                    insumo.costoPromedio = registroStock.STKPREPRO.ToString();
                                }
                            }
                            else
                            {
                                int insumoInt = Int32.Parse(insumo.value);

                                insumo.costoPromedio = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == almacen && x.insumo == insumoInt).ToList().Average(x => x.precio).ToString();
                            }
                        }

                        return listaInsumos;
                    }
                    #endregion
                }
                else
                {
                    #region DEMAS EMPRESAS
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        ( 
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida, 
                                        ( 
                                            ISNULL ( (select ROUND(AVG(det.precio),1) from si_movimientos_det det where det.almacen={2} and det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.insumo LIKE '{0}%' 
                                    ORDER BY i.insumo", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo,
                        unidad = (string)x.unidad,
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = (bool)x.bit_area_cta,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                        compras_req = x.compras_req != null ? (int)x.compras_req : 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                        //ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public dynamic getInsumosDescByAlmacen(string term, string cc, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ADESCRI.Contains(term.ToUpper())).Take(12).Select(x => new InsumoRequisicionDTO
                        {
                            id = x.ACODIGO,
                            value = x.ACODIGO + " - " + x.ADESCRI,
                            descripcion = x.ADESCRI,
                            unidad = x.AUNIDAD,
                            exceso = 0,
                            isAreaCueta = false,
                            cancelado = "A",
                            costoPromedio = "0",
                            costoPromedioEntrada = 0,
                            costo = 0,
                            color_resguardo = 0,
                            compras_req = 1,
                            ultimaCompra = 0
                        }).ToList();

                        foreach (var insumo in listaInsumos)
                        {
                            var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == insumo.id);

                            if (registroStock != null)
                            {
                                insumo.costoPromedio = registroStock.STKPREPRO.ToString();
                            }
                        }

                        return listaInsumos;
                    }
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region MyRegion
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        (
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida,
                                        (
                                         ISNULL ( (select ROUND(AVG(det.precio),1) from si_movimientos_det det where det.almacen={2} and det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado
                                    FROM insumos i 
                                    WHERE i.descripcion LIKE '%{0}%'
                                    ORDER BY i.insumo ", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = false, // NO EXISTE EN COLOMBIA EK
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = 0, // NO EXISTE EN COLOMBIA EK
                        compras_req = 0, // NO EXISTE EN COLOMBIA EK
                        numValue = x.insumo
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, ins.numValue);
                        //ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion
                }
                else
                {
                    #region RESTO EMPRESAS
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        (
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida,
                                        (
                                         ISNULL ( (select ROUND(AVG(det.precio),1) from si_movimientos_det det where det.almacen={2} and det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.descripcion LIKE '%{0}%'
                                    ORDER BY i.insumo ", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = (bool)x.bit_area_cta,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                        compras_req = x.compras_req != null ? (int)x.compras_req : 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                        //ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion
                }
            }
            catch (Exception) { return 0; }
        }

        public dynamic getInsumosByAlmacenEntrada(string term, string cc, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ACODIGO.Contains(term)).Take(12).Select(x => new InsumoRequisicionDTO
                        {
                            id = x.ADESCRI,
                            value = x.ACODIGO,
                            unidad = x.AUNIDAD,
                            exceso = 0,
                            isAreaCueta = false,
                            cancelado = "A",
                            costoPromedio = "0",
                            costoPromedioEntrada = 0,
                            costo = 0,
                            color_resguardo = 0,
                            compras_req = 1,
                            ultimaCompra = 0
                        }).ToList();

                        foreach (var insumo in listaInsumos)
                        {
                            //Se cambió la lógica para traer el último precio de entrada.
                            //var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == insumo.value);

                            //if (registroStock != null)
                            //{
                            //    insumo.costoPromedio = registroStock.STKPREPRO.ToString();
                            //    insumo.costoPromedioEntrada = (decimal)registroStock.STKPREPRO;
                            //}

                            var listaEntradas = _starsoft.MOVALMCAB.Where(x => x.CACODMOV == "CL").ToList().Where(x => Int32.Parse(x.CAALMA) == almacen).Join(
                                _starsoft.MovAlmDet,
                                g => new { almacen = g.CAALMA, td = g.CATD, numdoc = g.CANUMDOC },
                                d => new { almacen = d.DEALMA, td = d.DETD, numdoc = d.DENUMDOC },
                                (g, d) => new { g, d }
                            ).ToList().Where(x => Int32.Parse(x.d.DECODIGO) == Int32.Parse(insumo.value) && x.d.DEPRECIO > 0).ToList();

                            if (listaEntradas.Count() > 0)
                            {
                                var ultimaEntrada = listaEntradas.OrderByDescending(x => x.g.CAFECDOC).FirstOrDefault();

                                insumo.costoPromedio = ultimaEntrada.d.DEPRECIO.ToString();
                                insumo.costoPromedioEntrada = (decimal)ultimaEntrada.d.DEPRECIO;
                            }
                            else
                            {
                                insumo.costoPromedio = "0";
                                insumo.costoPromedioEntrada = 0m;
                            }
                        }

                        return listaInsumos;
                    }
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA

                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        ( 
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida, 
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {2} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado
                                    FROM insumos i 
                                    WHERE i.insumo LIKE '{0}%' 
                                    ORDER BY i.insumo", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo,
                        unidad = (string)x.unidad,
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = false,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = 0,
                        compras_req = 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                        ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion

                }
                else
                {
                    #region RESTO EMPRESAS

                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        ( 
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida, 
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {2} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.insumo LIKE '{0}%' 
                                    ORDER BY i.insumo", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo,
                        unidad = (string)x.unidad,
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = (bool)x.bit_area_cta,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                        compras_req = x.compras_req != null ? (int)x.compras_req : 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                        ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public dynamic getInsumosDescByAlmacenEntrada(string term, string cc, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU

                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ADESCRI.Contains(term.ToUpper())).Take(12).Select(x => new InsumoRequisicionDTO
                        {
                            id = x.ADESCRI,
                            value = x.ACODIGO + " - " + x.ADESCRI,
                            descripcion = x.ADESCRI,
                            unidad = x.AUNIDAD,
                            exceso = 0,
                            isAreaCueta = false,
                            cancelado = "A",
                            costoPromedio = "0",
                            costoPromedioEntrada = 0,
                            costo = 0,
                            color_resguardo = 0,
                            compras_req = 1,
                            ultimaCompra = 0
                        }).ToList();

                        foreach (var insumo in listaInsumos)
                        {
                            var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == insumo.value.Split('-')[0].Trim());

                            if (registroStock != null)
                            {
                                insumo.costoPromedio = registroStock.STKPREPRO.ToString();
                                insumo.costoPromedioEntrada = (decimal)registroStock.STKPREPRO;
                            }
                        }

                        return listaInsumos;
                    }
                    #endregion

                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad,
                                        (
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida,
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {2} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado
                                    FROM insumos i 
                                    WHERE i.descripcion LIKE '%{0}%'
                                    ORDER BY i.insumo ", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = false, // NO EXITE COLOMBIA EK
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = 0, // NO EXITE COLOMBIA EK
                        compras_req = 0, // NO EXITE COLOMBIA EK
                        numValue = x.insumo
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, ins.numValue);
                        ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, ins.numValue);
                    }

                    return insumos;
                    #endregion

                }
                else
                {
                    #region RESTO EMPRESAS
                    var insumosEK = (List<dynamic>)consultaCheckProductivo(
                        string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        (
                                            SELECT TOP 1 
                                                m.cant_requerida 
                                            FROM so_explos_mat m 
                                            WHERE 
                                                m.insumo = i.insumo AND 
                                                m.cc = '{1}' 
                                            ORDER BY year_explos DESC 
                                        ) AS cant_requerida,
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {2} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.descripcion LIKE '%{0}%'
                                    ORDER BY i.insumo ", term, cc, almacen)).ToObject<List<dynamic>>();

                    var insumos = insumosEK.Select(x => new Core.DTO.Enkontrol.Alamcen.InsumoDTO
                    {
                        id = (string)x.descripcion,
                        value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                        exceso = x.cant_requerida != null ? (decimal)x.cant_requerida : 0,
                        isAreaCueta = (bool)x.bit_area_cta,
                        cancelado = (string)x.cancelado,
                        costoPromedio = x.costo_promedio != null ? Convert.ToDecimal(x.costo_promedio, CultureInfo.InvariantCulture) : 0,
                        costo = x.costo_promedio,
                        color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                        compras_req = x.compras_req != null ? (int)x.compras_req : 0
                    }).ToList();

                    foreach (var ins in insumos)
                    {
                        ins.costoPromedio = getCostoPromedioAcumulaAlmacen(almacen, Int32.Parse(ins.value));
                        ins.costoPromedioEntrada = getCostoPromedioEntrada(almacen, cc, Int32.Parse(ins.value));
                    }

                    return insumos;
                    #endregion

                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Dictionary<string, object> getExistenciaInsumo(int insumo, string cc, int almacen)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
//                var res = (List<dynamic>)consultaCheckProductivo(
//                    string.Format(@"SELECT 
//                                        SUM(det.cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS existencia 
//                                    FROM si_movimientos mov
//                                        INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
//                                        INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
//                                    WHERE det.insumo = {0} 
//                                        --AND alm.almacen < 900", insumo)
//                ).ToObject<List<dynamic>>();
                decimal existencia = 0;
                int[] tiposMovValidos = { 1, 2, 3, 4, 5 };
                using(var ctx = new MainContext())
                {
                    existencia = (
                        from mov in ctx.tblAlm_Movimientos
                        join det in ctx.tblAlm_MovimientosDet
                            on new { mov.almacen, mov.tipo_mov, mov.numero } equals new { det.almacen, det.tipo_mov, det.numero }
                        join alm in ctx.tblAlm_Almacen
                            on mov.almacen equals alm.almacen
                        where det.insumo == insumo // Filtra por el insumo que se pasa como parámetro
                        // Si aplicas el filtro por almacén, descoméntalo
                        // && alm.almacen < 900
                        select det.cantidad * (tiposMovValidos.Contains(mov.tipo_mov) ? 1 : -1)
                    ).Sum();
                }

                resultado.Add("success", true);
                resultado.Add("existencia", existencia);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getExistenciaInsumoDetalle(int insumo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                using (var ctx = new MainContext())
                {
                    var tiposEntradas = new int[] { 1, 2, 3, 4, 5 };
                    var tiposSalidas = new int[] { 51, 52, 53, 54, 55 };

                    // Consulta principal en LINQ, reemplazando la consulta SQL original
                    var resEK = (
                        from mov in ctx.tblAlm_Movimientos
                        join alm in ctx.tblAlm_Almacen
                            on mov.almacen equals alm.almacen
                        join det in ctx.tblAlm_MovimientosDet
                            on new { mov.almacen, mov.tipo_mov, mov.numero } equals new { det.almacen, det.tipo_mov, det.numero }
                        where det.insumo == insumo
                        // && alm.almacen < 900 // Descomentar si es necesario
                        group new { mov, det, alm } by new
                        {
                            mov.almacen,
                            alm.descripcion,
                            det.area_alm,
                            det.lado_alm,
                            det.estante_alm,
                            det.nivel_alm
                        } into g
                        select new
                        {
                            Almacen = g.Key.almacen,
                            Descripcion = g.Key.descripcion,
                            AreaAlmacen = g.Key.area_alm,
                            LadoAlmacen = g.Key.lado_alm,
                            EstanteAlmacen = g.Key.estante_alm,
                            NivelAlmacen = g.Key.nivel_alm,
                            Entradas = g.Sum(x => tiposEntradas.Contains(x.mov.tipo_mov) ? x.det.cantidad : 0),
                            Salidas = g.Sum(x => tiposSalidas.Contains(x.mov.tipo_mov) ? x.det.cantidad : 0),
                            Existencia = g.Sum(x => x.det.cantidad * (tiposEntradas.Contains(x.mov.tipo_mov) ? 1 : -1))
                        }
                    ).OrderByDescending(x => x.Existencia)
                    .ThenBy(x => x.Descripcion)
                    .ToList();

                    if (resEK.Any())
                    {
                        var reservados = getReservados(insumo);
                        var stockMinimo = ctx.tblAlm_StockMinimo.Where(x => x.estatus && x.insumo == insumo).ToList();

                        var list = resEK.Select(x => new
                        {
                            almacenID = x.Almacen,
                            almacen = x.Almacen + "-" + x.Descripcion,
                            entradas = x.Entradas,
                            salidas = x.Salidas,
                            existencia = x.Existencia,
                            minimo = stockMinimo.Where(y => y.almacenID == (int)x.Almacen).Select(z => z.stockMinimo).FirstOrDefault() ?? "",
                            ultimoConsumoString = getUltimoConsumo(insumo, x.Almacen),
                            ultimaCompraString = getUltimaCompra(insumo, x.Almacen),
                            reservados = reservados.Where(y =>
                                y.almacenOrigenID == x.Almacen &&
                                y.area_alm == x.AreaAlmacen &&
                                y.lado_alm == x.LadoAlmacen &&
                                y.estante_alm == x.EstanteAlmacen &&
                                y.nivel_alm == x.NivelAlmacen
                            ).Sum(w => w.cantidad),
                            area_alm = x.AreaAlmacen ?? "",
                            lado_alm = x.LadoAlmacen ?? "",
                            estante_alm = x.EstanteAlmacen ?? "",
                            nivel_alm = x.NivelAlmacen ?? ""
                        }).ToList();

                        resultado.Add("existencia", list.Where(x => x.existencia > 0).ToList());
                    }
                    else
                    {
                        resultado.Add("existencia", new List<dynamic>());
                    }

                    resultado.Add("success", true);
                }
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getExistenciaInsumoDetalle_old(int insumo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            alm.almacen, 
                                            alm.descripcion, 
                                            det.area_alm, 
                                            det.lado_alm, 
                                            det.estante_alm, 
                                            det.nivel_alm, 
                                            SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                                            SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                                            SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} 
                                            --AND alm.almacen < 900 
                                        GROUP BY mov.almacen, det.insumo, alm.almacen, alm.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm 
                                        ORDER BY Existencia DESC, descripcion ASC", insumo)
                );

                if (resEK != null)
                {
                    var res = (List<dynamic>)resEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);
                    var stockMinimo = _context.tblAlm_StockMinimo.Where(x => x.estatus && x.insumo == insumo).ToList();

                    var list = res.Select(x => new
                    {
                        almacenID = (int)x.almacen,
                        almacen = (int)x.almacen + "-" + (string)x.descripcion,
                        entradas = (decimal)x.Entradas,
                        salidas = (decimal)x.Salidas,
                        existencia = (decimal)x.Existencia,
                        minimo = stockMinimo.Where(y => y.almacenID == (int)x.almacen).Select(z => z.stockMinimo).FirstOrDefault() ?? "",
                        ultimoConsumoString = getUltimoConsumo(insumo, (int)x.almacen),
                        ultimaCompraString = getUltimaCompra(insumo, (int)x.almacen),
                        reservados = reservados.Where(y =>
                            y.almacenOrigenID == (int)x.almacen &&
                            y.area_alm == (string)x.area_alm &&
                            y.lado_alm == (string)x.lado_alm &&
                            y.estante_alm == (string)x.estante_alm &&
                            y.nivel_alm == (string)x.nivel_alm
                            ).Sum(w => w.cantidad),
                        area_alm = x.area_alm != null ? (string)x.area_alm : "",
                        lado_alm = x.lado_alm != null ? (string)x.lado_alm : "",
                        estante_alm = x.estante_alm != null ? (string)x.estante_alm : "",
                        nivel_alm = x.nivel_alm != null ? (string)x.nivel_alm : "",
                    }).ToList();

                    resultado.Add("existencia", list.Where(x => x.existencia > 0).ToList());
                }
                else
                {
                    resultado.Add("existencia", new List<dynamic>());
                }

                resultado.Add("success", true);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getExistenciaInsumoDetalleTotal(int insumo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            alm.almacen, 
                                            alm.descripcion, 
                                            det.area_alm, 
                                            det.lado_alm, 
                                            det.estante_alm, 
                                            det.nivel_alm, 
                                            SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                                            SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                                            SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} 
                                        GROUP BY mov.almacen, det.insumo, alm.almacen, alm.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm 
                                        ORDER BY Existencia DESC, descripcion ASC", insumo)
                );

                if (resEK != null)
                {
                    var res = (List<dynamic>)resEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);
                    var stockMinimo = _context.tblAlm_StockMinimo.Where(x => x.estatus && x.insumo == insumo).ToList();

                    var list = res.Select(x => new
                    {
                        almacenID = (int)x.almacen,
                        almacen = (int)x.almacen + "-" + (string)x.descripcion,
                        entradas = (decimal)x.Entradas,
                        salidas = (decimal)x.Salidas,
                        existencia = (decimal)x.Existencia,
                        minimo = stockMinimo.Where(y => y.almacenID == (int)x.almacen).Select(z => z.stockMinimo).FirstOrDefault() ?? "",
                        ultimoConsumoString = getUltimoConsumo(insumo, (int)x.almacen),
                        ultimaCompraString = getUltimaCompra(insumo, (int)x.almacen),
                        reservados = reservados.Where(y =>
                            y.almacenOrigenID == (int)x.almacen &&
                            y.area_alm == (string)x.area_alm &&
                            y.lado_alm == (string)x.lado_alm &&
                            y.estante_alm == (string)x.estante_alm &&
                            y.nivel_alm == (string)x.nivel_alm
                            ).Sum(w => w.cantidad),
                        area_alm = x.area_alm != null ? (string)x.area_alm : "",
                        lado_alm = x.lado_alm != null ? (string)x.lado_alm : "",
                        estante_alm = x.estante_alm != null ? (string)x.estante_alm : "",
                        nivel_alm = x.nivel_alm != null ? (string)x.nivel_alm : "",
                    }).ToList();

                    resultado.Add("existencia", list.Where(x => x.existencia > 0).ToList());
                }
                else
                {
                    resultado.Add("existencia", new List<dynamic>());
                }

                resultado.Add("success", true);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getExistenciaInsumoDetalleAlmacenFisico(int insumo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            alm.almacen, 
                                            alm.descripcion, 
                                            det.area_alm, 
                                            det.lado_alm, 
                                            det.estante_alm, 
                                            det.nivel_alm, 
                                            SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                                            SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                                            SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} 
                                            AND alm.almacen < 900 
                                        GROUP BY mov.almacen, det.insumo, alm.almacen, alm.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm 
                                        ORDER BY Existencia DESC, descripcion ASC", insumo)
                );

                if (resEK != null)
                {
                    var res = (List<dynamic>)resEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);
                    var stockMinimo = _context.tblAlm_StockMinimo.Where(x => x.estatus && x.insumo == insumo).ToList();

                    var list = res.Select(x => new
                    {
                        almacenID = (int)x.almacen,
                        almacen = (int)x.almacen + "-" + (string)x.descripcion,
                        entradas = (decimal)x.Entradas,
                        salidas = (decimal)x.Salidas,
                        existencia = (decimal)x.Existencia,
                        minimo = stockMinimo.Where(y => y.almacenID == (int)x.almacen).Select(z => z.stockMinimo).FirstOrDefault() ?? "",
                        ultimoConsumoString = getUltimoConsumo(insumo, (int)x.almacen),
                        ultimaCompraString = getUltimaCompra(insumo, (int)x.almacen),
                        reservados = reservados.Where(y =>
                            y.almacenOrigenID == (int)x.almacen &&
                            y.area_alm == (string)x.area_alm &&
                            y.lado_alm == (string)x.lado_alm &&
                            y.estante_alm == (string)x.estante_alm &&
                            y.nivel_alm == (string)x.nivel_alm
                            ).Sum(w => w.cantidad),
                        area_alm = x.area_alm != null ? (string)x.area_alm : "",
                        lado_alm = x.lado_alm != null ? (string)x.lado_alm : "",
                        estante_alm = x.estante_alm != null ? (string)x.estante_alm : "",
                        nivel_alm = x.nivel_alm != null ? (string)x.nivel_alm : "",
                    }).ToList();

                    resultado.Add("existencia", list.Where(x => x.existencia > 0).ToList());
                }
                else
                {
                    resultado.Add("existencia", new List<dynamic>());
                }

                resultado.Add("success", true);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        public Dictionary<string, object> getExistenciaInsumoDetalleTotalAlmacenFisico(int insumo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var resEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            alm.almacen, 
                                            alm.descripcion, 
                                            det.area_alm, 
                                            det.lado_alm, 
                                            det.estante_alm, 
                                            det.nivel_alm, 
                                            SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                                            SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                                            SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} AND alm.almacen < 900 
                                        GROUP BY mov.almacen, det.insumo, alm.almacen, alm.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm 
                                        ORDER BY Existencia DESC, descripcion ASC", insumo)
                );

                if (resEK != null)
                {
                    var res = (List<dynamic>)resEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);
                    var stockMinimo = _context.tblAlm_StockMinimo.Where(x => x.estatus && x.insumo == insumo).ToList();

                    var list = res.Select(x => new
                    {
                        almacenID = (int)x.almacen,
                        almacen = (int)x.almacen + "-" + (string)x.descripcion,
                        entradas = (decimal)x.Entradas,
                        salidas = (decimal)x.Salidas,
                        existencia = (decimal)x.Existencia,
                        minimo = stockMinimo.Where(y => y.almacenID == (int)x.almacen).Select(z => z.stockMinimo).FirstOrDefault() ?? "",
                        ultimoConsumoString = getUltimoConsumo(insumo, (int)x.almacen),
                        ultimaCompraString = getUltimaCompra(insumo, (int)x.almacen),
                        reservados = reservados.Where(y =>
                            y.almacenOrigenID == (int)x.almacen &&
                            y.area_alm == (string)x.area_alm &&
                            y.lado_alm == (string)x.lado_alm &&
                            y.estante_alm == (string)x.estante_alm &&
                            y.nivel_alm == (string)x.nivel_alm
                            ).Sum(w => w.cantidad),
                        area_alm = x.area_alm != null ? (string)x.area_alm : "",
                        lado_alm = x.lado_alm != null ? (string)x.lado_alm : "",
                        estante_alm = x.estante_alm != null ? (string)x.estante_alm : "",
                        nivel_alm = x.nivel_alm != null ? (string)x.nivel_alm : "",
                    }).ToList();

                    resultado.Add("existencia", list.Where(x => x.existencia > 0).ToList());
                }
                else
                {
                    resultado.Add("existencia", new List<dynamic>());
                }

                resultado.Add("success", true);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        string getUltimoConsumo(int insumo, int almacenID)
        {
            var res = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 mov.fecha 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} AND alm.almacen = {1} AND mov.tipo_mov = 51 
                                        ORDER BY mov.fecha DESC", insumo, almacenID));

            List<dynamic> ultimoConsumo = new List<dynamic>();

            if (res != null)
            {
                ultimoConsumo = (List<dynamic>)res.ToObject<List<dynamic>>();
            }

            //((DateTime?)res[0][0].fecha_modifica).Value.Date.ToShortDateString(),
            return res != null ? ((DateTime)ultimoConsumo[0].fecha).Date.ToShortDateString() : "";
        }

        string getUltimaCompra(int insumo, int almacenID)
        {
            var res = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 mov.fecha 
                                        FROM si_movimientos mov 
                                            INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                            INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                        WHERE 
                                            det.insumo = {0} AND alm.almacen = {1} AND mov.tipo_mov = 1 
                                        ORDER BY mov.fecha DESC", insumo, almacenID));

            List<dynamic> ultimaCompra = new List<dynamic>();

            if (res != null)
            {
                ultimaCompra = (List<dynamic>)res.ToObject<List<dynamic>>();
            }

            return res != null ? ((DateTime)ultimaCompra[0].fecha).Date.ToShortDateString() : "";
        }

        public dynamic getInsumosDesc_old(string term, string cc, bool esServicio = false)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var listaInsumos = new List<InsumoRequisicionDTO>();

                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                listaInsumos = _starsoft.MAEART.ToList().Where(x => x.ADESCRI.Contains(term.Trim().ToUpper()) && (esServicio ? x.ACODIGO.Substring(0, 2) == "02" : x.ACODIGO.Substring(0, 2) == "01")).Take(12).ToList().Select(x => new InsumoRequisicionDTO
                                {
                                    id = x.ACODIGO,
                                    value = x.ACODIGO + " - " + x.ADESCRI,
                                    descripcion = x.ADESCRI,
                                    unidad = x.AUNIDAD,
                                    exceso = 0,
                                    isAreaCueta = false,
                                    cancelado = "A",
                                    costoPromedio = "0",
                                    color_resguardo = 0,
                                    compras_req = 1,
                                    ultimaCompra = 0
                                }).ToList();
                            }

                            return listaInsumos;
                        }
                    case EmpresaEnum.Colombia:
                        {
                            var res = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT TOP 12 
                                                i.insumo, 
                                                i.descripcion, 
                                                i.unidad, 
                                                (
                                                    SELECT TOP 1 
                                                        m.cant_requerida 
                                                    FROM so_explos_mat m 
                                                    WHERE 
                                                        m.insumo = i.insumo AND 
                                                        m.cc = '{1}' 
                                                    ORDER BY year_explos DESC 
                                                ) AS cant_requerida,
                                                (
                                                 ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 )
                                                ) AS costo_promedio, 
                                                i.cancelado
                                            FROM insumos i 
                                            WHERE i.descripcion LIKE '%{0}%'
                                            ORDER BY i.insumo ", term, cc)).ToObject<List<dynamic>>();

                            //TODO POR INSUMO DESCRIPCION
                            #region SE OBTIENE EL ULTIMO PRECIO DEL INSUMO COMPRADO
                            string strQueryInsumos = string.Empty;
                            List<string> lstInsumos = new List<string>();
                            for (int i = 0; i < res.Count(); i++)
                            {
                                int insumo = 0;
                                insumo = res[i].insumo;
                                if (i == 0)
                                    strQueryInsumos += "insumo = " + insumo;
                                else
                                    strQueryInsumos += " OR insumo = " + insumo;
                            }

                            List<Core.DTO.Enkontrol.Alamcen.InsumoDTO> resUltimaCompra = new List<Core.DTO.Enkontrol.Alamcen.InsumoDTO>();
                            string strQuery = @"SELECT TOP {2} insumo, precio FROM DBA.so_orden_compra_det WHERE cc = '{0}' AND ({1}) ORDER BY fecha_entrega DESC";
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery, cc, strQueryInsumos, res.Count());
                            //resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(productivo ? EnkontrolEnum.ArrenProd : EnkontrolEnum.PruebaArrenADM, odbc);
                            resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(EnkontrolEnum.ArrenProd, odbc);
                            #endregion

                            var lstResultados = res.Select(x => new
                            {
                                id = (string)x.insumo,
                                value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                unidad = (string)x.unidad,
                                exceso = (decimal?)x.cant_requerida,
                                cancelado = (string)x.cancelado,
                                costoPromedio = (string)x.costo_promedio,
                                color_resguardo = 0,
                                compras_req = 1,
                                ultimaCompra = resUltimaCompra.Count() > 0 ? resUltimaCompra.Where(w => w.insumo == (int)x.insumo).Select(s => s.precio).FirstOrDefault() : 0
                            }).ToList();

                            return lstResultados.ToList();
                        }
                    default:
                        {
                            var res = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT TOP 12 
                                                i.insumo, 
                                                i.descripcion, 
                                                i.unidad, 
                                                i.bit_area_cta, 
                                                (
                                                    SELECT TOP 1 
                                                        m.cant_requerida 
                                                    FROM so_explos_mat m 
                                                    WHERE 
                                                        m.insumo = i.insumo AND 
                                                        m.cc = '{1}' 
                                                    ORDER BY year_explos DESC 
                                                ) AS cant_requerida,
                                                (
                                                 ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 )
                                                ) AS costo_promedio, 
                                                i.cancelado, 
                                                i.color_resguardo, 
                                                i.compras_req 
                                            FROM insumos i 
                                            WHERE i.descripcion LIKE '%{0}%'
                                            ORDER BY i.insumo ", term, cc)).ToObject<List<dynamic>>();

                            //TODO POR INSUMO DESCRIPCION
                            #region SE OBTIENE EL ULTIMO PRECIO DEL INSUMO COMPRADO
                            string strQueryInsumos = string.Empty;
                            List<string> lstInsumos = new List<string>();
                            for (int i = 0; i < res.Count(); i++)
                            {
                                int insumo = 0;
                                insumo = res[i].insumo;
                                if (i == 0)
                                    strQueryInsumos += "insumo = " + insumo;
                                else
                                    strQueryInsumos += " OR insumo = " + insumo;
                            }

                            List<Core.DTO.Enkontrol.Alamcen.InsumoDTO> resUltimaCompra = new List<Core.DTO.Enkontrol.Alamcen.InsumoDTO>();
                            string strQuery = @"SELECT TOP {2} insumo, precio FROM DBA.so_orden_compra_det WHERE cc = '{0}' AND ({1}) ORDER BY fecha_entrega DESC";
                            var odbc = new OdbcConsultaDTO() { consulta = strQuery };
                            odbc.consulta = String.Format(strQuery, cc, strQueryInsumos, res.Count());
                            //resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(productivo ? EnkontrolEnum.ArrenProd : EnkontrolEnum.PruebaArrenADM, odbc);
                            resUltimaCompra = _contextEnkontrol.Select<Core.DTO.Enkontrol.Alamcen.InsumoDTO>(EnkontrolEnum.ArrenProd, odbc);
                            #endregion

                            var lstResultados = res.Select(x => new
                            {
                                id = (string)x.insumo,
                                value = (string)x.insumo + " - " + (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                descripcion = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                unidad = (string)x.unidad,
                                exceso = (decimal?)x.cant_requerida,
                                isAreaCueta = (bool)x.bit_area_cta,
                                cancelado = (string)x.cancelado,
                                costoPromedio = (string)x.costo_promedio,
                                color_resguardo = x.color_resguardo != null ? (int)x.color_resguardo : 0,
                                compras_req = (int?)x.compras_req,
                                ultimaCompra = resUltimaCompra.Count() > 0 ? resUltimaCompra.Where(w => w.insumo == (int)x.insumo).Select(s => s.precio).FirstOrDefault() : 0
                            }).ToList();

                            return lstResultados.ToList();
                        }
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }



        public bool isEmpAdmin()
        {
            var usuario = vSesiones.sesionUsuarioDTO;
            var relUser = ufs.getUsuarioService().getUserEk(usuario.id);
            return relUser.empleado.Equals(1);
        }
        public dynamic getReq(bool isAuth, List<string> cc)
        {
            try
            {
                if (cc.Any(x => x == "Todos"))
                {
                    cc = new List<string>();
                }

                #region CONSTRUPLAN/ARRENDADORA
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);
//                var requisicionesEK = consultaCheckProductivo(string.Format(@"
//                                SELECT 
//                                    r.cc, 
//                                    r.numero, 
//                                    r.fecha, 
//                                    r.autorizo, 
//                                    r.solicito, 
//                                    (ccc.cc + '-' + ccc.descripcion) AS ccNom, 
//                                    (SELECT e.descripcion FROM empleados e WHERE r.solicito = e.empleado ) AS solNom, 
//                                    (SELECT SUM(det.cantidad) FROM so_requisicion_det det WHERE det.cc = r.cc AND det.numero = r.numero) as cantidadTotal, 
//                                    CASE 
//                                        WHEN EXISTS(
//                                            SELECT 
//                                                det2.insumo 
//                                            FROM insumos i 
//                                                INNER JOIN so_requisicion_det det2 ON det2.cc = r.cc AND det2.numero = r.numero AND i.insumo = det2.insumo 
//                                            WHERE i.cancelado = 'C'
//                                        ) THEN 1 ELSE 0 
//                                    END AS contieneCancelado 
//                                FROM so_requisicion r 
//                                    INNER JOIN cc AS ccc ON ccc.cc = r.cc
//                                WHERE ccc.st_ppto != 'T' AND r.fecha >= '2019-11-01' AND r.st_autoriza = '{0}'" + (cc.Count > 0 ? @" AND r.cc IN ({1})" : @""), isAuth ? "S" : "N", string.Join(", ", cc.Select(x => "'" + x + "'")))
//                );

                var requisicionesEK = (
                    from r in _context.tblCom_Req
                    join ccc in _context.tblP_CC
                        on r.cc equals ccc.cc
                    where ccc.estatus
                          && r.fecha >= new DateTime(2019, 11, 1)
                          && ccc.st_ppto != "T"
                          && r.stAutoriza == isAuth
                          && (cc.Count == 0 || cc.Contains(r.cc)) 
                    select new Requisicion2DTO
                    {
                        cc = r.cc,
                        numero = r.numero,
                        fecha = r.fecha,
                        autorizo = r.autorizo.ToString(),
                        solicito = r.solicito.ToString(),
                        isAuth = false,
                        flagCheckBox = false,
                        montoTotal = 0,
                        monedaDesc = "",
                        consigna = false,
                        licitacion = false,
                        crc = false,
                        convenio = false,
                        ccNom = r.cc + "-" + ccc.descripcion,
                        otFolio = r.otFolio,
                        economico = r.noEconomico,
                        solNom = _context.tblRH_EK_Empleados
                                          .Where(e => e.clave_empleado == r.solicito)
                                          .Select(e => e.nombre)
                                          .FirstOrDefault(), // Subconsulta para obtener solNom
                        cantidadTotal = _context.tblCom_ReqDet
                                                .Where(det => det.cc == r.cc && det.numero == r.numero)
                                                .Sum(det => (decimal?)det.cantidad) ?? 0, // Subconsulta para calcular la cantidad total
                        contieneCancelado = _context.tblCom_ReqDet
                                                  .Where(det2 => det2.cc == r.cc && det2.numero == r.numero)
                                                  .Join(_context.tblAlm_Insumo,
                                                        det2 => det2.insumo,
                                                        i => i.insumo,
                                                        (det2, i) => i)
                                                  .Any(i => i.cancelado == "C") ? 1 : 0 // Subconsulta para verificar si contiene insumos cancelados
                    }
                ).ToList();

                if (requisicionesEK != null)
                {
                    var requisiciones = requisicionesEK.Where(x => x.fecha.Year >= (DateTime.Now.Year - 1)).ToList();

                    var listaAutorizantesCentroCosto = _context.tblCom_AutorizanteCentroCosto.Where(x => x.registroActivo).ToList();

                    foreach (var requisicion in requisiciones)
                    {
                        var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == requisicion.cc && x.numero == requisicion.numero);

                        var autorizantesPorCentroCosto = listaAutorizantesCentroCosto.Where(x => x.cc == requisicion.cc).ToList();

                        requisicion.isAuth = autorizantesPorCentroCosto.Any(x => x.empleado == relUser.empleado);

                        requisicion.flagCheckBox = false;

                        if (requisicionSIGOPLAN != null)
                        {
                            requisicion.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                            requisicion.licitacion = requisicionSIGOPLAN.licitacion;
                            requisicion.crc = requisicionSIGOPLAN.crc;
                            requisicion.convenio = requisicionSIGOPLAN.convenio;

                            var partidasSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicionSIGOPLAN.id).ToList();

                            requisicion.montoTotal = partidasSIGOPLAN.Sum(x => x.cantidad * x.precio);

                            if ((requisicionSIGOPLAN.proveedor > 0 && requisicionSIGOPLAN.proveedor < 9000) || requisicionSIGOPLAN.proveedor == 9999)
                            {
                                requisicion.monedaDesc = "MN";
                            }
                            else if (requisicionSIGOPLAN.proveedor >= 9000 && requisicionSIGOPLAN.proveedor != 9999)
                            {
                                requisicion.monedaDesc = "USD";
                            }
                        }
                    }

                    //Regresar las requisiciones que el usuario sí pueda autorizar/desautorizar.
                    return requisiciones.Where(x => (relUser.empleado != 1 ? x.isAuth : true)).ToList();
                }
                else
                {
                    return 0;
                }
                #endregion
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public string getFolioOrigen(string cc, int num)
        {
            var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == num);

            return requisicion != null ? requisicion.folioAsignado != null ? requisicion.folioAsignado : "" : "";
        }
        #region Usuario
        public dynamic getThisEmpleadoEnkontrol()
        {
            return (List<dynamic>)ContextEnKontrolNomina.Where(string.Format("SELECT * FROM ek010ab where empleado = {0}", ufs.getUsuarioService().getUserEk(vSesiones.sesionUsuarioDTO.id).empleado)).ToObject<List<dynamic>>();
        }
        public void setUsuarioEk()
        {

            var lst = (List<tblP_Usuario_Enkontrol>)ContextEnKontrolNomina.Where("SELECT num as sn_empleado, empleado ,nom FROM ek010ab where empleado is not null").ToObject<List<tblP_Usuario_Enkontrol>>();
            lst.ForEach(u =>
            {
                _context.tblP_Usuario_Enkontrol.AddOrUpdate(u);
                SaveChanges();
            });
        }
        #endregion
        #region cbo
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboLab()
        {
            try
            {
                var lst = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol
                    .Where("SELECT numero as Value, descripcion as Text FROM so_libre_abordo").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                lst.AsParallel().Where(w => string.IsNullOrEmpty(w.Prefijo)).ForAll(x => x.Prefijo = string.Empty);
                return lst.ToList();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoReq()
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    default:
                        var data = _context.tblCom_ReqTipo.Where(x => x.registroActivo).ToList().Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.tipo_req_oc.ToString(),
                            Text = x.descripcion,
                            Prefijo = x.dias_requisicion.ToString()
                        }).ToList();
                        return data;
                        // return (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(@"
                        //     SELECT
                        //         tipo_req_oc as Value,
                        //         descripcion as Text,
                        //         dias_requisicion as Prefijo
                        //     FROM so_tipo_requisicion
                        // ").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboResponsablePorCc(string cc)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {

                    case EmpresaEnum.Peru:
                        {
                            using (var ctxPeru = new MainContext())
                            {
                                var ccSigoplan = ctxPeru.tblP_CC.FirstOrDefault(x => x.cc == cc);
                                if (ccSigoplan != null)
                                {
                                    cc = ccSigoplan.ccRH;
                                }

                                var responsablesEK = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        res.empleado AS Value, 
                                        emp.descripcion AS Text 
                                    FROM so_cc_responsable res 
                                        INNER JOIN empleados emp ON res.empleado = emp.empleado 
                                    WHERE res.cc = '{0}'", cc)
                                );

                                List<Core.DTO.Principal.Generales.ComboDTO> responsables = new List<Core.DTO.Principal.Generales.ComboDTO>();

                                if (responsablesEK != null)
                                {
                                    responsables.AddRange((List<Core.DTO.Principal.Generales.ComboDTO>)responsablesEK.ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>());
                                }

                                return responsables.Where(x => !x.Text.Equals("Administrador")).ToList();
                            }
                        }
                    case EmpresaEnum.Colombia:
                        var empleadosEK = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM DBA.empleados emp")).ToObject<List<dynamic>>();

                        return _context.tblCom_ResponsableCC.Where(x => x.registroActivo && x.cc == cc).ToList().Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.empleado.ToString(),
                            Text = empleadosEK.Where(y => (int)y.empleado == x.empleado).Select(z => (string)z.descripcion).FirstOrDefault()
                        }).Where(x => x.Text != null).OrderBy(x => x.Text).ToList();
                    default:
                        {
                            var responsablesEK = consultaCheckProductivo(
                            string.Format(@"SELECT 
                                        res.empleado AS Value, 
                                        emp.descripcion AS Text 
                                    FROM so_cc_responsable res 
                                        INNER JOIN empleados emp ON res.empleado = emp.empleado 
                                    WHERE res.cc = '{0}'", cc)
                            );

                            List<Core.DTO.Principal.Generales.ComboDTO> responsables = new List<Core.DTO.Principal.Generales.ComboDTO>();

                            if (responsablesEK != null)
                            {
                                responsables.AddRange((List<Core.DTO.Principal.Generales.ComboDTO>)responsablesEK.ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>());
                            }

                            return responsables.Where(x => !x.Text.Equals("Administrador")).ToList();
                        }
                }
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public dynamic FillComboAreaCuenta(string cc)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                    case EmpresaEnum.Colombia:
                        {
                            return _context.tblM_CatMaquina.Where(x => x.estatus == 1).ToList().Select(x => new
                            {
                                Value = x.noEconomico,
                                Text = x.noEconomico,
                                Prefijo = 0
                            }).OrderBy(x => x.Text).ToList();
                        }
                    case EmpresaEnum.Arrendadora:
                        {
                            var listaAreaCuenta = (List<dynamic>)consultaCheckProductivo(string.Format("SELECT * FROM si_area_cuenta WHERE cc_activo = 1 AND centro_costo = '{0}'", cc)).ToObject<List<dynamic>>();
                            var listaBloqueoAreaCuenta = _context.tblCom_BloqueoAreaCuenta.Where(x => x.registroActivo).ToList();

                            foreach (var areaCuenta in listaBloqueoAreaCuenta)
                            {
                                listaAreaCuenta.RemoveAll(x => (int)x.area == areaCuenta.area && (int)x.cuenta == areaCuenta.cuenta);
                            }

                            return listaAreaCuenta.Select(x => new
                            {
                                Value = (int)x.area,
                                Text = string.Format("{0:000}-{1:000} {2}", x.area, x.cuenta, x.descripcion),
                                Prefijo = (int)x.cuenta
                            }).ToList();
                        }
                    default:
                        {
                            var listaAreaCuenta = (List<dynamic>)consultaCheckProductivo(string.Format("SELECT * FROM si_area_cuenta WHERE cc_activo = 1 AND centro_costo = '{0}'", cc)).ToObject<List<dynamic>>();

                            return listaAreaCuenta.Select(x => new
                            {
                                Value = (int)x.area,
                                Text = string.Format("{0:000}-{1:000} {2}", x.area, x.cuenta, x.descripcion),
                                Prefijo = (int)x.cuenta
                            }).ToList();
                        }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public dynamic FillComboTipoPartida(string cc)
        {
            try
            {
                var lista = new List<dynamic>();
                lista.Add(new
                {
                    Value = 0,
                    Text = "N/A",
                    Prefijo = 0
                });

                lista.Add(new
                {
                    Value = 1,
                    Text = "CH",
                    Prefijo = 0
                });

                lista.Add(new
                {
                    Value = 2,
                    Text = "MAQUINARIA",
                    Prefijo = 0
                });

                return lista;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public dynamic FillComboTipoPartidaDet(int tipo)
        {
            try
            {
                var lista = new List<dynamic>();
                lista.Add(new
                {
                    Value = 0,
                    Text = "N/A",
                    Prefijo = 0
                });

                if(tipo == 1){
                    var empleados = _context.tblRH_EK_Empleados.ToList().Select(x => new
                    {
                        Value = x.clave_empleado,
                        Text = x.nombre + " " + x.ape_paterno + " " + x.ape_materno,
                        Prefijo = 0
                    }).OrderBy(x => x.Text).ToList();
                    lista.AddRange(empleados);
                }
                else if(tipo == 2){
                    var maquinas = _context.tblM_CatMaquina.ToList().Select(x => new
                                {
                                    Value = x.id,
                                    Text = x.noEconomico,
                                    Prefijo = 0
                                }).OrderBy(x => x.Text).ToList();
                    lista.AddRange(maquinas);
                }

                return lista;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcReq(bool isAuth)
        {
            try
            {
                using (var ctx = new MainContext())
                {
                    var listaBloqueoCC = ctx.tblCom_BloqueoCentroCosto.Where(x => x.registroActivo).Select(x => x.cc).ToList();

                    var ccsPermitidos = ctx.tblP_CC_Usuario.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                    var listaCentroCosto = ctx.tblP_CC
                        .Where(x =>
                            x.estatus &&
                            (((PerfilUsuarioEnum)vSesiones.sesionUsuarioDTO.idPerfil == PerfilUsuarioEnum.ADMINISTRADOR) ? true : ccsPermitidos.Contains(x.cc)) &&
                            !listaBloqueoCC.Contains(x.cc)
                        ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.cc,
                            Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        }).ToList();

                    return listaCentroCosto;
                }
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcAsigReq()
        {
            try
            {
                using (var ctx = new MainContext())
                {
                    var listaBloqueoCC = ctx.tblCom_BloqueoCentroCosto.Where(x => x.registroActivo).Select(x => x.cc).ToList();

                    var ccsPermitidos = ctx.tblP_CC_Usuario.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x => x.cc).ToList();
                    var listaCentroCosto = ctx.tblP_CC
                        .Where(x =>
                            x.estatus &&
                            (((PerfilUsuarioEnum)vSesiones.sesionUsuarioDTO.idPerfil == PerfilUsuarioEnum.ADMINISTRADOR) ? true : ccsPermitidos.Contains(x.cc)) &&
                            // x.cc.Length > 3 &&
                            !listaBloqueoCC.Contains(x.cc)
                        ).Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.cc,
                            Text = "[" + x.cc + "] " + x.descripcion.Trim()
                        }).ToList();

                    return listaCentroCosto;
                }
            }
            catch (Exception e)
            {
                var NOMBRE_FUNCION = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_FUNCION, e, AccionEnum.CONSULTA, 0, null);
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcTodos()
        {
            try
            {
                //                var centrosCostoEK = consultaCheckProductivo(
                //                    string.Format(@"SELECT 
                //                                        c.cc AS Value, 
                //                                        (c.cc + '-' + c.descripcion) AS Text 
                //                                    FROM cc c 
                //                                    WHERE c.st_ppto != 'T' 
                //                                    ORDER BY Value")
                //                );

                //if (centrosCostoEK != null)
                //{
                //    return (List<Core.DTO.Principal.Generales.ComboDTO>)centrosCostoEK.ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                //}
                //else
                //{
                //    return new List<Core.DTO.Principal.Generales.ComboDTO>();

                //}

                return _context.tblC_Nom_CatalogoCC.Where(e => e.estatus == true).Select(e => new Core.DTO.Principal.Generales.ComboDTO
                 {
                     Value = e.cc,
                     Text = e.cc + " - " + e.ccDescripcion,


                 }).ToList();


            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboCcPorResponsable()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);
                var res = (List<dynamic>)consultaCheckProductivo(string.Format("SELECT cc FROM so_asigna_req WHERE empleado = {0}", relUser.empleado)).ToObject<List<dynamic>>();
                var lstCC = res.Select(c => (string)c.cc).ToList();
                return (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        c.cc AS Value, 
                                        (c.cc + '-' + c.descripcion) AS Text 
                                    FROM cc c 
                                    {0} 
                                    WHERE c.st_ppto != 'T' 
                                    ORDER BY Value"
                    , lstCC.Any(c => c.Equals("*")) ? "" : "INNER JOIN so_asigna_req a ON a.cc = c.cc AND a.empleado = {0}" + relUser.empleado
                    )).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoFolio()
        {
            try
            {
                var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();

                var folios = Enum.GetValues(typeof(TipoFolio)).Cast<TipoFolio>().ToList();
                foreach (var f in folios)
                {
                    lst.Add(new Core.DTO.Principal.Generales.ComboDTO
                    {
                        Value = f.ParseInt().ToString(),
                        Text = f.GetDescription()
                    });
                }

                return lst;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTxtFolio(int tipo)
        {
            try
            {
                var lst = new List<Core.DTO.Principal.Generales.ComboDTO>();
                if (tipo == 0) { return lst; }
                int folio = 0;
                if (tipo == 1)
                    folio = 2526;
                else
                    folio = 1324;
                for (int i = 0; i < 3; i++)
                {
                    Core.DTO.Principal.Generales.ComboDTO aux = new Core.DTO.Principal.Generales.ComboDTO();
                    aux.Value = (folio + i).ToString();
                    aux.Text = (folio + i).ToString();
                    lst.Add(aux);
                }
                return lst;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtir()
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    default:
                        {
                            using (var ctx = new MainContext())
                            {
                                var almacenes = ctx.tblAlm_Almacen.ToList().Select(x => new Core.DTO.Principal.Generales.ComboDTO
                                {
                                    Value = Convert.ToInt32(x.almacen).ToString(),
                                    Text = "[" + x.almacen + "] " + x.descripcion
                                }).ToList();

                                return almacenes;
                            }
                        }
                }
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirAcceso()
        {
            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                var accesosAlmacenesEK = consultaCheckProductivo(string.Format(@"SELECT * FROM si_emp_almacen WHERE empleado = {0}", relUser.empleado));

                if (accesosAlmacenesEK != null)
                {
                    List<int> accesosAlmacenes = ((List<dynamic>)accesosAlmacenesEK.ToObject<List<dynamic>>()).Select(x => (int)x.almacen).ToList();

                    var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            alm.almacen AS Value, 
                                            (CONVERT(varchar(12), alm.almacen) + ' - ' + alm.descripcion) AS Text 
                                        FROM si_almacen alm 
                                        WHERE (alm.almacen_virtual != 1 or alm.almacen_virtual IS null) AND (alm.bit_mp = 'S' OR alm.almacen = 997 OR alm.almacen = 998 OR alm.almacen = 999) 
                                        ORDER BY Value")
                    ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                    var almacenesFiltradosAcceso = almacenes.Where(x => accesosAlmacenes.Contains(Int32.Parse(x.Value))).ToList();

                    return almacenesFiltradosAcceso;
                }
                else
                {
                    return new List<Core.DTO.Principal.Generales.ComboDTO>();
                }
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboAlmacenSurtirTodos()
        {
            try
            {
                if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
                {
                    return (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(string.Format(@"
                        SELECT 
                            alm.almacen AS Value, 
                            (CONVERT(varchar(12), alm.almacen) + ' - ' + alm.descripcion) AS Text 
                        FROM si_almacen alm 
                        WHERE alm.bit_mp = 'S' OR alm.almacen = 997 OR alm.almacen = 998 OR alm.almacen = 999 
                        ORDER BY Value
                    ")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                }
                else
                {
                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        return _starsoft.TABALM.ToList().Select(x => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = x.TAALMA,
                            Text = x.TAALMA + " - " + x.TADESCRI
                        }).ToList();
                    }
                }
            }
            catch (Exception) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }
        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboTipoInsumo()
        {
            try
            {
                var listaTipoInsumo = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        tipo_insumo AS Value, 
                                        (CONVERT(varchar(10), tipo_insumo) + '-' + descripcion) AS Text 
                                    FROM tipos_insumo")
                ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                return listaTipoInsumo;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedores()
        {
            try
            {
                return _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(getEnkontrolEnumADM(), string.Format(@"
                    SELECT
                        CAST(numpro AS varchar) AS Value, (CAST(numpro AS varchar) + ' - ' + nombre) AS Text
                    FROM sp_proveedores
                    ORDER BY numpro"));
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresConsignaLicitacionConvenio(TipoConsignaLicitacionConvenioEnum tipo)
        {
            try
            {
                var listaProveedoresEK = new List<Core.DTO.Principal.Generales.ComboDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERU
                            using (var ctxStarsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                listaProveedoresEK = ctxStarsoft.MAEPROV.Select(x => new Core.DTO.Principal.Generales.ComboDTO
                                {
                                    Value = x.PRVCCODIGO,
                                    Text = "[" + x.PRVCCODIGO + "] " + x.PRVCNOMBRE.Trim()
                                }).ToList();

                                switch (tipo)
                                {
                                    case TipoConsignaLicitacionConvenioEnum.CONSIGNA:
                                        var listaProveedoresConsigna = _context.tblCom_InsumosConsignaPeru.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                        listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConsigna.Contains(x.Value)).ToList();
                                        break;
                                    case TipoConsignaLicitacionConvenioEnum.LICITACION:
                                        var listaProveedoresLicitacion = _context.tblCom_InsumosLicitacionPeru.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                        listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresLicitacion.Contains(x.Value)).ToList();
                                        break;
                                    case TipoConsignaLicitacionConvenioEnum.CRC:
                                        listaProveedoresEK = listaProveedoresEK.Where(x => Int32.Parse(x.Value) == 9000).ToList(); //CRC tiene el proveedor 9000 nomás. Por lo pronto.
                                        break;
                                    case TipoConsignaLicitacionConvenioEnum.CONVENIO:
                                        var listaProveedoresConvenio = _context.tblCom_InsumosConvenioPeru.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                        listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConvenio.Contains(x.Value)).ToList();
                                        break;
                                    default:
                                        listaProveedoresEK = new List<Core.DTO.Principal.Generales.ComboDTO>();
                                        break;
                                }
                            }
                            #endregion
                        }
                        break;
                    case EmpresaEnum.Colombia:
                        {
                            listaProveedoresEK = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(getEnkontrolEnumADM(), string.Format(@"
                                SELECT
                                    CAST(numpro AS varchar) AS Value, (CAST(numpro AS varchar) + ' - ' + nombre) AS Text
                                FROM sp_proveedores
                                ORDER BY numpro"
                            ));

                            switch (tipo)
                            {
                                case TipoConsignaLicitacionConvenioEnum.CONSIGNA:
                                    var listaProveedoresConsigna = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConsigna.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.LICITACION:
                                    var listaProveedoresLicitacion = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresLicitacion.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.CRC:
                                    listaProveedoresEK = listaProveedoresEK.Where(x => Int32.Parse(x.Value) == 9000).ToList(); //CRC tiene el proveedor 9000 nomás. Por lo pronto.
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.CONVENIO:
                                    var listaProveedoresConvenio = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConvenio.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                default:
                                    listaProveedoresEK = new List<Core.DTO.Principal.Generales.ComboDTO>();
                                    break;
                            }
                        }
                        break;
                    default:
                        {
                            listaProveedoresEK = _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(getEnkontrolEnumADM(), string.Format(@"
                                SELECT
                                    CAST(numpro AS varchar) AS Value, (CAST(numpro AS varchar) + ' - ' + nombre) AS Text
                                FROM sp_proveedores
                                ORDER BY numpro"
                            ));

                            switch (tipo)
                            {
                                case TipoConsignaLicitacionConvenioEnum.CONSIGNA:
                                    var listaProveedoresConsigna = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConsigna.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.LICITACION:
                                    var listaProveedoresLicitacion = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresLicitacion.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.CRC:
                                    listaProveedoresEK = listaProveedoresEK.Where(x => Int32.Parse(x.Value) == 9000).ToList(); //CRC tiene el proveedor 9000 nomás. Por lo pronto.
                                    break;
                                case TipoConsignaLicitacionConvenioEnum.CONVENIO:
                                    var listaProveedoresConvenio = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).GroupBy(x => x.proveedor).Select(x => x.Key).ToList();

                                    listaProveedoresEK = listaProveedoresEK.Where(x => listaProveedoresConvenio.Contains(Int32.Parse(x.Value))).ToList();
                                    break;
                                default:
                                    listaProveedoresEK = new List<Core.DTO.Principal.Generales.ComboDTO>();
                                    break;
                            }
                        }
                        break;
                }

                return listaProveedoresEK;
            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        #endregion

        public RequisicionDTO getRequisicionSIGOPLAN(string cc, int num, string PERU_tipoRequisicion = "")
        {
            tblCom_Req req = null;

            if (PERU_tipoRequisicion == "")
            {
                req = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == num);
            }
            else
            {
                req = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == num && x.PERU_tipoRequisicion == PERU_tipoRequisicion);
            }

            if ((EmpresaEnum)vSesiones.sesionEmpresaActual == EmpresaEnum.Peru && req == null)
            {
                req = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.numero == num && x.PERU_tipoRequisicion == PERU_tipoRequisicion);
            }

            if (req != null)
            {
                List<RequisicionDetDTO> partidas = new List<RequisicionDetDTO>();

                var part = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == req.id && x.estatus != "false").ToList();

                if (part.Count > 0)
                {
                    partidas.AddRange(part.Select(x => new RequisicionDetDTO
                    {
                        partida = x.partida,
                        insumo = x.insumo,
                        fecha_requerido = x.requerido,
                        cantidad = x.cantidad,
                        precio = x.precio,
                        cant_ordenada = x.cantOrdenada,
                        fecha_ordenada = x.ordenada,
                        estatus = x.estatus,
                        cant_cancelada = x.cantCancelada,
                        referencia_1 = x.referencia,
                        cantidad_excedida_ppto = x.cantExcedida,
                        area = x.area,
                        cuenta = x.cuenta,
                        observaciones = x.observaciones,
                        noEconomico = x.noEconomico
                    }));
                }

                var empleadoEnkontrolCP = _context.Select<dynamic>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = @"SELECT 
                                    e.clave_empleado AS claveEmpleado, 
                                    (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado
                                FROM tblRH_EK_Empleados AS e 
                                WHERE e.estatus_empleado = 'A' AND e.clave_empleado = @usr",
                    parametros = new { usr = req.usuarioSolicita }
                });

                var requisicion = new RequisicionDTO
                {
                    cc = req.cc,
                    numero = req.numero,
                    fecha = req.fecha,
                    libre_abordo = req.idLibreAbordo,
                    tipo_req_oc = req.idTipoReqOc.ToString(),
                    solicito = req.solicito,
                    vobo = req.vobo,
                    autorizo = req.autorizo,
                    comentarios = req.comentarios,
                    st_estatus = req.stEstatus,
                    emp_autoriza = req.empAutoriza,
                    empleado_modifica = req.empModifica,
                    consigna = req.consigna,
                    licitacion = req.licitacion,
                    crc = req.crc,
                    convenio = req.convenio,
                    proveedor = req.proveedor,
                    partidas = partidas,
                    validadoAlmacen = req.validadoAlmacen,
                    comprador = req.comprador,
                    fechaSurtidoCompromiso = req.fechaSurtidoCompromiso,
                    usuarioSolicita = req.usuarioSolicita,
                    usuarioSolicitaDesc =
                        empleadoEnkontrolCP.Count() > 0 ? empleadoEnkontrolCP[0].nombreEmpleado : "",
                    usuarioSolicitaUso = req.usuarioSolicitaUso,
                    usuarioSolicitaEmpresa = req.usuarioSolicitaEmpresa
                };

                return requisicion;
            }
            else
            {
                return null;
            }
        }

        public void GuardarSurtido(RequisicionDTO info, List<SurtidoDTO> lstSurtido)
        {
            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var req = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == info.cc && x.numero == info.numero);

                            if (!req.stAutoriza)
                            {
                                throw new Exception("La requisición no está autorizada en SIGOPLAN.");
                            }

                            #region Validación para cambio de insumos en la requisición.
                            var flagCambioRequisicion = false;
                            var requisicionDetalle = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT * FROM so_requisicion_det WHERE cc = '{0}' AND numero = {1}", info.cc, info.numero)
                            ).ToObject<List<dynamic>>();
                            var requisicionDetalleSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.idReq == req.id).ToList();

                            if (requisicionDetalle.Count() != lstSurtido.Count())
                            {
                                flagCambioRequisicion = true;
                            }
                            else
                            {
                                foreach (var sur in lstSurtido)
                                {
                                    var partidaRequisicion = requisicionDetalle.FirstOrDefault(x => (int)x.partida == sur.partida);

                                    if ((int)partidaRequisicion.insumo != sur.insumo)
                                    {
                                        flagCambioRequisicion = true;
                                    }
                                }
                            }

                            if (flagCambioRequisicion)
                            {
                                #region Enkontrol
                                foreach (var sur in lstSurtido)
                                {
                                    var consultaUpdateRequisicionDetalle = @"
                                    UPDATE so_requisicion_det 
                                    SET 
                                        insumo = ? 
                                    WHERE cc = ? and numero = ? AND partida = ?";

                                    using (var cmd = new OdbcCommand(consultaUpdateRequisicionDetalle))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;

                                        parameters.Add("@insumo", OdbcType.Numeric).Value = sur.insumo;

                                        parameters.Add("@cc", OdbcType.Char).Value = req.cc;
                                        parameters.Add("@numero", OdbcType.Numeric).Value = req.numero;
                                        parameters.Add("@partida", OdbcType.Numeric).Value = sur.partida;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                #endregion

                                #region SIGOPLAN
                                foreach (var sur in lstSurtido)
                                {
                                    var partidaRequisicion = requisicionDetalleSIGOPLAN.FirstOrDefault(x => x.partida == sur.partida);

                                    partidaRequisicion.insumo = sur.insumo;
                                    _context.SaveChanges();
                                }
                                #endregion
                            }
                            #endregion

                            var fechaActual = DateTime.Now;

                            var lstSurtidoCapturados = lstSurtido.Where(x => x.quitar || (x.nuevaCaptura != null && x.nuevaCaptura[0] != null && x.nuevaCaptura.Sum(y => y.aSurtir) > 0)).ToList();

                            req.fechaSurtidoCompromiso = info.fechaSurtidoCompromiso;
                            _context.Entry(req).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();

                            #region Cancelación del surtido establecido anterior.
                            var surtidoAnterior = _context.tblCom_Surtido.Where(x => x.estatus && x.cc == info.cc && x.numero == info.numero).ToList();

                            foreach (var sur in surtidoAnterior)
                            {
                                sur.estatus = false;

                                _context.Entry(sur).State = System.Data.Entity.EntityState.Modified;
                                _context.SaveChanges();
                            }

                            var surtidoDetAnterior = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.cc == info.cc && x.numero == info.numero).ToList();

                            foreach (var surDet in surtidoDetAnterior)
                            {
                                surDet.estatus = false;

                                _context.Entry(surDet).State = System.Data.Entity.EntityState.Modified;
                                _context.SaveChanges();
                            }
                            #endregion

                            foreach (var sur in lstSurtidoCapturados)
                            {
                                if (!sur.quitar)
                                {
                                    var surtido = new tblCom_Surtido
                                    {
                                        fecha = fechaActual,
                                        cc = info.cc,
                                        numero = info.numero,
                                        insumo = sur.insumo,
                                        cantidadTotal = sur.nuevaCaptura.Sum(x => x.aSurtir),
                                        estatus = true,
                                        tipo = ""
                                    };

                                    _context.tblCom_Surtido.Add(surtido);
                                    _context.SaveChanges();

                                    foreach (var det in sur.nuevaCaptura.Where(x => x.aSurtir > 0))
                                    {
                                        var surtidoDet = new tblCom_SurtidoDet
                                        {
                                            surtidoID = surtido.id,
                                            cc = info.cc,
                                            numero = info.numero,
                                            fecha = fechaActual,
                                            partidaRequisicion = sur.partida, //Número de partida del renglón principal.
                                            insumo = sur.insumo,
                                            almacenOrigenID = det.almacenID,
                                            almacenDestinoID = req.idLibreAbordo,
                                            cantidad = det.aSurtir,
                                            area_alm = det.area_alm,
                                            lado_alm = det.lado_alm,
                                            estante_alm = det.estante_alm,
                                            nivel_alm = det.nivel_alm,
                                            estadoSurtido = "R",
                                            tipoSurtidoDetalle = det.almacenID == req.idLibreAbordo ? "AP" : "AE",
                                            estatus = true,
                                        };

                                        _context.tblCom_SurtidoDet.Add(surtidoDet);
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    if (req != null)
                                    {
                                        var partidaRequisicion = _context.tblCom_ReqDet.FirstOrDefault(x =>
                                            x.estatusRegistro &&
                                            x.idReq == req.id &&
                                            x.partida == sur.partida &&
                                            x.insumo == sur.insumo
                                        );

                                        if (partidaRequisicion != null)
                                        {
                                            partidaRequisicion.cantidad = 0;
                                            partidaRequisicion.comentarioSurtidoQuitar = sur.comentarioSurtidoQuitar;

                                            _context.SaveChanges();
                                        }

                                        var consultaUpdateRequisicionDetalle = @"
                                            UPDATE so_requisicion_det 
                                            SET 
                                                cantidad = ? 
                                            WHERE cc = ? AND numero = ? AND partida = ?";

                                        using (var cmd = new OdbcCommand(consultaUpdateRequisicionDetalle))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cantidad", OdbcType.Numeric).Value = 0;

                                            parameters.Add("@cc", OdbcType.Char).Value = req.cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = req.numero;
                                            parameters.Add("@partida", OdbcType.Numeric).Value = sur.partida;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            var count = cmd.ExecuteNonQuery();

                                            if (count > 1)
                                            {
                                                throw new Exception("Error al actualizar la partida de la requisición.");
                                            }
                                        }
                                    }
                                }
                            }

                            trans.Commit();
                            dbSigoplanTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            dbSigoplanTransaction.Rollback();

                            LogError(12, 0, "RequisicionController", "GuardarSurtido", e, AccionEnum.AGREGAR, 0, new { info = info, lstSurtido = lstSurtido });

                            throw new Exception(e.Message);
                        }
                    }
                }
            }
        }

        public List<tblCom_SurtidoDet> getSurtidoPorReq(string cc, int numero)
        {
            return _context.tblCom_SurtidoDet.Where(x => x.estatus && x.cc == cc && x.numero == numero).ToList();
        }

        public List<SurtidoDetDTO> getSalidas(int almacenOrigenID, int almacenDestinoID)
        {
            var insumos = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        insumo AS Value, 
                                        descripcion AS Text 
                                    FROM insumos 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var centrosCosto = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        cc AS Value, 
                                        descripcion AS Text 
                                    FROM cc 
                                    WHERE st_ppto != 'T'
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var lstSurtidos = _context.tblCom_SurtidoDet.Where(x =>
                    x.estadoSurtido == "R" &&
                    x.estatus &&
                    (almacenOrigenID != 0 ? x.almacenOrigenID == almacenOrigenID : true) &&
                    (almacenDestinoID != 0 ? x.almacenDestinoID == almacenDestinoID : true)
                ).ToList();

            var resultados = lstSurtidos.Select(y => new SurtidoDetDTO
            {
                cc = y.cc,
                ccDesc = centrosCosto.FirstOrDefault(z => z.Value == y.cc) != null ?
                         centrosCosto.FirstOrDefault(z => z.Value == y.cc).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == y.cc).Text : "",
                numero = y.numero,
                fecha = y.fecha,
                insumo = y.insumo,
                insumoDesc = insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo) != null ? insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo).Text : "",
                almacenOrigenID = y.almacenOrigenID,
                almacenOrigenDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigenID) != null ?
                                    almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigenID).Text : "",
                almacenDestinoID = y.almacenDestinoID,
                almacenDestinoDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestinoID) != null ?
                                     almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestinoID).Text : "",
                cantidad = y.cantidad,
                traspasoSinOrigen = false,
                traspasoID = 0
            }).ToList();

            //if (lstTraspasos.Count > 0)
            //{
            //    resultados.AddRange(lstTraspasos.Select(y => new SurtidoDetDTO
            //    {
            //        cc = y.ccDestino,
            //        ccDesc = centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino) != null ?
            //                 centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino).Text : "",
            //        numero = 0,
            //        fecha = y.fecha,
            //        insumo = y.insumo,
            //        insumoDesc = insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo) != null ? insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo).Text : "",
            //        almacenOrigenID = y.almacenOrigen,
            //        almacenOrigenDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigen) != null ?
            //                            almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigen).Text : "",
            //        almacenDestinoID = y.almacenDestino,
            //        almacenDestinoDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestino) != null ?
            //                             almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestino).Text : "",
            //        cantidad = y.cantidadTraspasar - y.cantidadCancelada,
            //        traspasoSinOrigen = true,
            //        traspasoID = y.id
            //    }));
            //}

            return resultados;
        }
        public List<salidasAlmacenDTO> GuardarSalidas(List<SurtidoDetDTO> salidas)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            List<salidasAlmacenDTO> movSalidas = new List<salidasAlmacenDTO>();

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            foreach (var sal in salidas)
            {
                if (sal.checkBoxRechazado)
                {
                    var salida = _context.tblCom_SurtidoDet.FirstOrDefault(x => x.estadoSurtido == "R" && x.estatus && x.cc == sal.cc && x.numero == sal.numero && x.insumo == sal.insumo);

                    if (salida != null)
                    {
                        salida.estatus = false;

                        _context.Entry(salida).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var cantidadCancelada = sal.cantidad - sal.cantidadAutorizar;

                    if (cantidadCancelada > 0)
                    {
                        var salida = _context.tblCom_SurtidoDet.FirstOrDefault(x => x.estadoSurtido == "R" && x.estatus && x.cc == sal.cc && x.numero == sal.numero && x.insumo == sal.insumo);

                        if (salida != null)
                        {
                            salida.cantidad = sal.cantidadAutorizar;

                            _context.Entry(salida).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }

                    var ultimoMovimientoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 numero, folio_traspaso 
                                        FROM si_movimientos 
                                        WHERE almacen = {0} AND tipo_mov = {1} 
                                        ORDER BY numero DESC", sal.almacenOrigenID, 52)
                    );

                    var nuevoNumero = 0;

                    if (ultimoMovimientoEK != null)
                    {
                        var ultimoMovimiento = ((List<SurtidoDetDTO>)ultimoMovimientoEK.ToObject<List<SurtidoDetDTO>>())[0];

                        nuevoNumero = ultimoMovimiento.numero;
                    }

                    tblAlm_Movimientos nuevaSalida = new tblAlm_Movimientos();
                    List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

                    var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == sal.cc && x.numero == sal.numero);

                    if (requisicion != null)
                    {
                        requisicionDet = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicion.id).ToList();
                    }

                    sal.ccDestino = requisicion != null ? requisicion.cc : "";
                    sal.precio = 1;
                    sal.importe = sal.cantidadAutorizar * sal.precio;

                    nuevaSalida = new tblAlm_Movimientos
                    {
                        almacen = sal.almacenOrigenID,
                        tipo_mov = 52,
                        numero = nuevoNumero + 1,
                        cc = sal.cc.ToUpper(),
                        compania = 1,
                        periodo = DateTime.Now.Month,
                        ano = DateTime.Now.Year,
                        orden_ct = sal.ordenTraspaso,
                        frente = 0,
                        fecha = DateTime.Now.Date,
                        proveedor = 0,
                        total = sal.importe,
                        estatus = "A",
                        transferida = "N",
                        alm_destino = sal.almacenDestinoID,
                        cc_destino = sal.ccDestino.ToUpper(),
                        comentarios = sal.comentarios,
                        tipo_trasp = "0",
                        tipo_cambio = 1,
                        requisicion = requisicion,
                        estatusHabilitado = true
                    };

                    _context.tblAlm_Movimientos.Add(nuevaSalida);
                    _context.SaveChanges();

                    var insumoReq = requisicionDet.FirstOrDefault(x => x.insumo == sal.insumo);

                    List<tblAlm_MovimientosDet> listSalidaDet = new List<tblAlm_MovimientosDet>();

                    var partidaContador = 1;

                    foreach (var ubi in sal.listUbicacionMovimiento)
                    {
                        var partidaMovimiento = partidaContador++;
                        decimal costoPromedio = getCostoPromedioKardex(nuevaSalida.almacen, ubi.insumo);

                        var nuevaSalidaDet = new tblAlm_MovimientosDet
                        {
                            almacen = sal.almacenOrigenID,
                            tipo_mov = 52,
                            numero = nuevoNumero + 1,
                            partida = partidaMovimiento,
                            insumo = sal.insumo,
                            comentarios = sal.comentarios,
                            area = insumoReq != null ? insumoReq.area : 0,
                            cuenta = insumoReq != null ? insumoReq.cuenta : 0,
                            cantidad = ubi.cantidadMovimiento,
                            precio = costoPromedio,
                            importe = ubi.cantidadMovimiento * costoPromedio,
                            id_resguardo = 0,
                            area_alm = ubi.area_alm ?? "",
                            lado_alm = ubi.lado_alm ?? "",
                            estante_alm = ubi.estante_alm ?? "",
                            nivel_alm = ubi.nivel_alm ?? "",
                            transporte = sal.transporte ?? "",
                            estatusHabilitado = true
                        };

                        _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                        _context.SaveChanges();
                        listSalidaDet.Add(nuevaSalidaDet);
                    }

                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            if (nuevaSalida.total <= 0)
                            {
                                throw new Exception("El total no puede ser igual o menor a cero.");
                            }

                            var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                            var commandMovimientos = new OdbcCommand(consultaMovimientos);

                            OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                            parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaSalida.almacen;
                            parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaSalida.tipo_mov;
                            parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaSalida.numero;
                            parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaSalida.cc;
                            parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaSalida.compania;
                            parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaSalida.periodo;
                            parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaSalida.ano;
                            parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                            parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaSalida.frente;
                            parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaSalida.fecha.Date;
                            parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaSalida.proveedor;
                            parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaSalida.total;
                            parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaSalida.estatus;
                            parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaSalida.transferida;
                            parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                            parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                            parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaSalida.alm_destino;
                            parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaSalida.cc_destino;
                            parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaSalida.comentarios != null ? nuevaSalida.comentarios : "";
                            parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaSalida.tipo_trasp;
                            parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = sal.numeroDestino;
                            parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                            parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                            parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaSalida.tipo_cambio;
                            parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                            parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                            parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                            parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                            parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                            parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientos.Add("@folio_traspaso", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                            parametersMovimientos.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                            commandMovimientos.Connection = trans.Connection;
                            commandMovimientos.Transaction = trans;

                            var success = commandMovimientos.ExecuteNonQuery();
                            var successDet = 0;

                            foreach (var salDet in listSalidaDet)
                            {
                                decimal costoPromedio = getCostoPromedioKardex(nuevaSalida.almacen, salDet.insumo);
                                var importe = salDet.cantidad * costoPromedio;

                                if (costoPromedio <= 0 || importe <= 0)
                                {
                                    throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                                }

                                var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                                OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                                parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = salDet.almacen;
                                parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = salDet.tipo_mov;
                                parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = nuevoNumero + 1;
                                parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = salDet.partida;
                                parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = salDet.insumo;
                                parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = salDet.comentarios != null ? salDet.comentarios : "";
                                parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = salDet.area;
                                parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = salDet.cuenta;
                                parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = salDet.cantidad;
                                parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = costoPromedio;
                                parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = importe;
                                parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = costoPromedio;
                                parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                                parametersMovimientosDetalles.Add("@area_alm", OdbcType.Char).Value = salDet.area_alm;
                                parametersMovimientosDetalles.Add("@lado_alm", OdbcType.Char).Value = salDet.lado_alm;
                                parametersMovimientosDetalles.Add("@estante_alm", OdbcType.Char).Value = salDet.estante_alm;
                                parametersMovimientosDetalles.Add("@nivel_alm", OdbcType.Char).Value = salDet.nivel_alm;
                                parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                                commandMovimientosDetalles.Connection = trans.Connection;
                                commandMovimientosDetalles.Transaction = trans;

                                successDet = commandMovimientosDetalles.ExecuteNonQuery();

                                var ccDestinoDesc = "";

                                if (sal.ccDestino != "")
                                {
                                    var ccDestinoDescEK = (List<dynamic>)consultaCheckProductivo(
                                            string.Format(@"SELECT descripcion FROM cc WHERE cc = '{0}' AND st_ppto != 'T'", sal.ccDestino)
                                        ).ToObject<List<dynamic>>();

                                    ccDestinoDesc = (string)ccDestinoDescEK[0].descripcion;
                                }

                                movSalidas.Add(new salidasAlmacenDTO
                                {
                                    almacen = sal.almacenOrigenDesc,
                                    centroCosto = sal.ccDesc,
                                    fechaSalida = nuevaSalida.fecha,
                                    folioSalida = (nuevoNumero + 1).ToString(),
                                    partida = salDet.partida,
                                    insumo = sal.insumo + "-" + sal.insumoDesc ?? "",
                                    areaCuenta = salDet.area + "-" + salDet.cuenta,
                                    cantidad = salDet.cantidad,
                                    costoPromedio = 0,
                                    importe = salDet.importe,
                                    almacenDestino = sal.almacenDestinoDesc,
                                    centroCostoDestino = sal.ccDestino + "-" + ccDestinoDesc
                                });

                                #region Actualizar Tablas Acumula
                                var objAcumula = new MovimientoDetalleEnkontrolDTO
                                {
                                    insumo = salDet.insumo,
                                    cantidad = salDet.cantidad,
                                    precio = costoPromedio,
                                    tipo_mov = salDet.tipo_mov,
                                    costo_prom = costoPromedio
                                };

                                actualizarAcumula(nuevaSalida.almacen, nuevaSalida.cc, objAcumula, null, trans);
                                #endregion
                            }

                            if (success > 0 && successDet > 0)
                            {
                                var salida = _context.tblCom_SurtidoDet.FirstOrDefault(x => x.estadoSurtido == "R" && x.estatus && x.cc == sal.cc && x.numero == sal.numero && x.insumo == sal.insumo);

                                if (salida != null)
                                {
                                    if (sal.cantidadAutorizar == salida.cantidad)
                                    {
                                        salida.estadoSurtido = "P";

                                        _context.Entry(salida).State = System.Data.Entity.EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                }

                                _context.SaveChanges();
                                trans.Commit();
                            }
                        }
                    }
                }
            }

            return movSalidas;
        }
        public List<SurtidoDetDTO> getEntradas(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino)
        {
            var insumos = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        insumo AS Value, 
                                        descripcion AS Text 
                                    FROM insumos 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var centrosCosto = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        cc AS Value, 
                                        descripcion AS Text 
                                    FROM cc 
                                    WHERE st_ppto != 'T' 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

            var lstSurtidos = _context.tblCom_SurtidoDet.Where(x =>
                (x.estadoSurtido == "T" || x.estadoSurtido == "P") &&
                x.estatus &&
                (almacenOrigen > 0 ? x.almacenOrigenID == almacenOrigen : true) &&
                (centroCostoOrigen != "" ? x.cc == centroCostoOrigen : true) &&
                (almacenDestino > 0 ? x.almacenDestinoID == almacenDestino : true) &&
                (centroCostoDestino != "" ? x.cc == centroCostoDestino : true)
            ).ToList();

            var lstTraspasos = _context.tblAlm_Traspaso.Where(x =>
                x.estado == "T" && x.autorizado &&
                (almacenOrigen > 0 ? x.almacenOrigen == almacenOrigen : true) &&
                (centroCostoOrigen != "" ? x.ccOrigen == centroCostoOrigen : true) &&
                (almacenDestino > 0 ? x.almacenDestino == almacenDestino : true) &&
                (centroCostoDestino != "" ? x.ccDestino == centroCostoDestino : true)
            ).ToList();

            var resultados = lstSurtidos.Select(y => new SurtidoDetDTO
            {
                cc = y.cc,
                ccDesc = centrosCosto.FirstOrDefault(z => z.Value == y.cc) != null ?
                         centrosCosto.FirstOrDefault(z => z.Value == y.cc).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == y.cc).Text : "",
                numero = y.numero,
                fecha = y.fecha,
                insumo = y.insumo,
                insumoDesc = insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo) != null ? insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo).Text : "",
                almacenOrigenID = y.almacenOrigenID,
                almacenOrigenDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigenID) != null ?
                                    almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigenID).Text : "",
                almacenDestinoID = y.almacenDestinoID,
                almacenDestinoDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestinoID) != null ?
                                     almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestinoID).Text : "",
                cantidad = y.cantidad,
                traspasoSinOrigen = false,
                traspasoID = 0
            }).ToList();

            if (lstTraspasos.Count > 0)
            {
                resultados.AddRange(lstTraspasos.Select(y => new SurtidoDetDTO
                {
                    cc = y.ccDestino,
                    ccDesc = centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino) != null ?
                             centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == y.ccDestino).Text : "",
                    ccDestino = y.ccOrigen,
                    ccDestinoDesc = centrosCosto.FirstOrDefault(z => z.Value == y.ccOrigen) != null ?
                                    centrosCosto.FirstOrDefault(z => z.Value == y.ccOrigen).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == y.ccOrigen).Text : "",
                    numero = 0,
                    fecha = y.fecha,
                    insumo = y.insumo,
                    insumoDesc = insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo) != null ? insumos.FirstOrDefault(z => Int32.Parse(z.Value) == y.insumo).Text : "",
                    almacenOrigenID = y.almacenOrigen,
                    almacenOrigenDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigen) != null ?
                                        almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenOrigen).Text : "",
                    almacenDestinoID = y.almacenDestino,
                    almacenDestinoDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestino) != null ?
                                         almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == y.almacenDestino).Text : "",
                    cantidad = y.cantidadTraspasar - y.cantidadCancelada,
                    traspasoSinOrigen = true,
                    traspasoID = y.id
                }));
            }

            return resultados;
        }
        public List<SurtidoDetDTO> getSalidaTraspaso(int almacenOrigen, string centroCostoOrigen, int almacenDestino, string centroCostoDestino, int folioTraspaso)
        {
            switch ((MainContextEnum)vSesiones.sesionEmpresaActual)
            {
                case MainContextEnum.PERU:
                    {
                        #region PERU
                        var salidaSIGOPLAN = _context.tblAlm_Movimientos.FirstOrDefault(x =>
                            x.estatusHabilitado &&
                            x.tipo_mov == 52 &&
                            x.almacen == almacenOrigen &&
                            x.cc == centroCostoOrigen &&
                            x.alm_destino == almacenDestino &&
                            x.cc_destino == centroCostoDestino &&
                            x.orden_ct == folioTraspaso
                        );

                        var centroCostoOrigenDesc = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == salidaSIGOPLAN.cc).ccDescripcion;
                        var almacenOrigenDesc = "";
                        var almacenDestinoDesc = "";
                        var listaInsumosStarsoft = new List<MAEART>();

                        using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                        {
                            almacenOrigenDesc = _starsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == almacenOrigen).TADESCRI;
                            almacenDestinoDesc = _starsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == almacenDestino).TADESCRI;
                            listaInsumosStarsoft = _starsoft.MAEART.ToList();
                        }

                        var salidaDetalleSIGOPLAN = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == salidaSIGOPLAN.almacen && x.tipo_mov == 52 && x.numero == salidaSIGOPLAN.numero).ToList();

                        var resultados = salidaDetalleSIGOPLAN.Select(x => new SurtidoDetDTO
                        {
                            partida = x.partida,
                            cc = salidaSIGOPLAN.cc,
                            ccDesc = centroCostoOrigenDesc,
                            numero = x.numero,
                            fecha = salidaSIGOPLAN.fecha,
                            insumo = x.insumo,
                            insumoDesc = listaInsumosStarsoft.Where(y => Int32.Parse(y.ACODIGO) == x.insumo).Select(z => z.ADESCRI).FirstOrDefault(),
                            almacenOrigenID = salidaSIGOPLAN.almacen,
                            almacenOrigenDesc = almacenOrigenDesc,
                            almacenDestinoID = salidaSIGOPLAN.alm_destino,
                            almacenDestinoDesc = almacenDestinoDesc,
                            cantidad = x.cantidad,
                            traspasoSinOrigen = true,
                            traspasoID = 0,
                            folio_traspaso = salidaSIGOPLAN.orden_ct,
                            precio = x.precio
                        }).ToList();

                        return resultados;
                        #endregion
                    }
                default:
                    {
                        #region DEMÁS EMPRESAS
                        var movimientoEK = consultaCheckProductivo(
                            string.Format(@"SELECT * FROM si_movimientos WHERE almacen = {0} AND tipo_mov = 52 AND cc = '{1}' AND alm_destino = {2} AND cc_destino = '{3}' AND folio_traspaso = {4}",
                                            almacenOrigen, centroCostoOrigen, almacenDestino, centroCostoDestino, folioTraspaso)
                        );

                        if (movimientoEK != null)
                        {
                            var movimiento = ((List<dynamic>)movimientoEK.ToObject<List<dynamic>>())[0];

                            var detalle = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT * FROM si_movimientos_det WHERE almacen = {0} AND tipo_mov = 52 AND numero = {1}", (int)movimiento.almacen, (int)movimiento.numero)
                            ).ToObject<List<dynamic>>();


                            var entradasEK = consultaCheckProductivo(
                                string.Format(@"SELECT * FROM si_movimientos WHERE almacen = {0} AND tipo_mov = 2 AND cc = '{1}' AND alm_destino = {2} AND cc_destino = '{3}' AND folio_traspaso = {4}",
                                                almacenDestino, centroCostoDestino, almacenOrigen, centroCostoOrigen, folioTraspaso));

                            if (entradasEK != null)
                            {
                                var entradas = (List<dynamic>)entradasEK.ToObject<List<dynamic>>();
                                decimal totalSalida = (decimal)movimiento.total;
                                decimal totalEntradas = entradas.Sum(x => (decimal)x.total);

                                if (totalSalida <= totalEntradas)
                                {
                                    throw new Exception("La salida por traspaso ya tiene entrada por traspaso completa.");
                                }
                            }

                            var insumos = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        insumo AS Value, 
                                        descripcion AS Text 
                                    FROM insumos 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                            var centrosCosto = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                                    string.Format(@"SELECT 
                                            cc AS Value, 
                                            descripcion AS Text 
                                        FROM cc 
                                        WHERE st_ppto != 'T' 
                                        ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                            var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                                    string.Format(@"SELECT 
                                            alm.almacen AS Value, 
                                            alm.descripcion AS Text 
                                        FROM si_almacen alm 
                                        ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                            var resultados = detalle.Select(x => new SurtidoDetDTO
                            {
                                partida = (int)x.partida,
                                cc = (string)movimiento.cc,
                                ccDesc = centrosCosto.FirstOrDefault(z => z.Value == (string)movimiento.cc) != null ?
                                         centrosCosto.FirstOrDefault(z => z.Value == (string)movimiento.cc).Value + "-" + centrosCosto.FirstOrDefault(z => z.Value == (string)movimiento.cc).Text : "",
                                numero = (int)x.numero,
                                fecha = (DateTime)movimiento.fecha,
                                insumo = (int)x.insumo,
                                insumoDesc = insumos.FirstOrDefault(z => Int32.Parse(z.Value) == (int)x.insumo) != null ? insumos.FirstOrDefault(z => Int32.Parse(z.Value) == (int)x.insumo).Text : "",
                                almacenOrigenID = (int)movimiento.almacen,
                                almacenOrigenDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == (int)movimiento.almacen) != null ?
                                                    almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == (int)movimiento.almacen).Text : "",
                                almacenDestinoID = (int)movimiento.alm_destino,
                                almacenDestinoDesc = almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == (int)movimiento.alm_destino) != null ?
                                                     almacenes.FirstOrDefault(z => Int32.Parse(z.Value) == (int)movimiento.alm_destino).Text : "",
                                cantidad = Convert.ToDecimal(x.cantidad, CultureInfo.InvariantCulture),
                                traspasoSinOrigen = true,
                                traspasoID = 0,
                                folio_traspaso = movimiento.folio_traspaso != null ? (int)movimiento.folio_traspaso : 0,
                                precio = x.precio
                            }).ToList();

                            return resultados;
                        }
                        else
                        {
                            return new List<SurtidoDetDTO>();
                        }
                        #endregion
                    }
            }
        }
        public List<salidasAlmacenDTO> GuardarEntradas(List<SurtidoDetDTO> entradas, int folio_traspaso, int almacenDestinoOriginal)
        {
            var empleado = 0;
            var usuarioEnkontrolSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            List<salidasAlmacenDTO> movEntradas = new List<salidasAlmacenDTO>();

            if (usuarioEnkontrolSigoplan != null)
            {
                empleado = usuarioEnkontrolSigoplan.empleado;
            }

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        #region PERÚ
                        #region Validación Sobrepaso Salida
                        #region Salida
                        var almacenOrigenID = entradas[0].almacenOrigenID;
                        var almacenDestinoID = entradas[0].almacenDestinoID;
                        var entradaCC = entradas[0].cc;
                        var entradaCCDestino = entradas[0].ccDestino;
                        var salidaSIGOPLAN = _context.tblAlm_Movimientos.FirstOrDefault(x =>
                            x.estatusHabilitado &&
                            x.almacen == almacenOrigenID &&
                            x.tipo_mov == 52 &&
                            x.alm_destino == almacenDestinoID &&
                            x.cc == entradaCC &&
                            x.cc_destino == entradaCCDestino &&
                            x.orden_ct == folio_traspaso
                        );

                        if (salidaSIGOPLAN == null)
                        {
                            throw new Exception("No se encuentra la información de la salida.");
                        }

                        var salidaDetSIGOPLAN = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == salidaSIGOPLAN.almacen && x.tipo_mov == 52 && x.numero == salidaSIGOPLAN.numero).ToList();

                        if (salidaDetSIGOPLAN.Count() == 0)
                        {
                            throw new Exception("No se encuentra la información de las partidas de la salida.");
                        }
                        #endregion

                        #region Entradas Capturadas
                        var listaEntradasCapturadas = _context.tblAlm_Movimientos.Where(x =>
                            x.estatusHabilitado &&
                            x.almacen == almacenDestinoID &&
                            x.tipo_mov == 2 &&
                            x.alm_destino == almacenOrigenID &&
                            x.cc == entradaCCDestino &&
                            x.cc_destino == entradaCC &&
                            x.orden_ct == folio_traspaso
                        ).ToList();

                        var listaEntradasCapturadasDet = new List<tblAlm_MovimientosDet>();

                        if (listaEntradasCapturadas.Count() > 0)
                        {
                            var entradasCapturadas_numero = listaEntradasCapturadas.Select(x => x.numero).ToList();

                            listaEntradasCapturadasDet = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == almacenDestinoID && x.tipo_mov == 2 && entradasCapturadas_numero.Contains(x.numero)).ToList();
                        }
                        #endregion

                        foreach (var sal in salidaDetSIGOPLAN)
                        {
                            var cantidadEntradasCapturadas = listaEntradasCapturadasDet.Where(x => x.partida == sal.partida && x.insumo == sal.insumo).Sum(x => x.cantidad);
                            var entradasPendientes = entradas.Where(x => x.partida == sal.partida && x.insumo == sal.insumo && x.listUbicacionMovimiento != null && x.listUbicacionMovimiento.Count() > 0).ToList();
                            var cantidadEntradas = entradasPendientes.Sum(x => x.listUbicacionMovimiento.Sum(y => y.cantidad));
                            var cantidadEntradasTotal = cantidadEntradasCapturadas + cantidadEntradas;

                            if (cantidadEntradasTotal > sal.cantidad)
                            {
                                throw new Exception("La cantidad de entrada (" + cantidadEntradas + (cantidadEntradasCapturadas > 0 ? " + " + cantidadEntradasCapturadas : "") + ") sobrepasa la cantidad de salida (" + sal.cantidad + ") en la partida #" + sal.partida + ". </p><br><p style='color: red;'>ATENCIÓN: Se toma en cuenta las entradas por traspaso ya capturadas para la salida en caso de que existan.</p><p>");
                            }
                        }
                        #endregion

                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            using (var dbStarsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                using (var dbStarsoftTransaction = dbStarsoft.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        var groupAutorizados = entradas.GroupBy(x => new
                                        {
                                            x.cc,
                                            x.almacenOrigenID,
                                            x.ccDestino,
                                            x.almacenDestinoID,
                                            x.traspasoSinOrigen
                                        })
                                        .Select(grupo => new
                                        {
                                            ccOrigen = grupo.Key.cc,
                                            almacenOrigen = grupo.Key.almacenOrigenID,
                                            ccDestino = grupo.Key.ccDestino,
                                            almacenDestino = grupo.Key.almacenDestinoID,
                                            traspasoSinOrigen = grupo.Key.traspasoSinOrigen,
                                            renglones = grupo.ToList()
                                        });

                                        #region Validación Folio de Traspaso mayor a cero
                                        if (folio_traspaso <= 0)
                                        {
                                            throw new Exception("Debe capturar un folio de traspaso válido.");
                                        }
                                        #endregion

                                        #region USUARIO INVENTARIOS STARSOFT
                                        var objUsrStarsoftInventarios = _context.tblAlm_Almacenistas.FirstOrDefault(e => e.id_usuario_sigoplan == vSesiones.sesionUsuarioDTO.id);

                                        string idAlmacenistaStarsoft = "0";

                                        if (objUsrStarsoftInventarios == null)
                                        {
                                            if (vSesiones.sesionUsuarioDTO.idPerfil == 1)
                                            {
                                                idAlmacenistaStarsoft = "1";
                                            }
                                            else
                                            {
                                                throw new Exception("Esta usuario no es un almacenista en el sistema de PERU");
                                            }
                                        }
                                        else
                                        {
                                            idAlmacenistaStarsoft = objUsrStarsoftInventarios.id_usuario_inventarios;
                                        }
                                        #endregion

                                        #region Validación Inventariables STARSOFT
                                        var lstInsumos = new List<MAEART>();
                                        var listaCatalogoInsumos = dbStarsoft.MAEART.ToList();

                                        foreach (var det in entradas)
                                        {
                                            var objInsumo = listaCatalogoInsumos.FirstOrDefault(e => Int64.Parse(e.ACODIGO) == det.insumo);

                                            if (objInsumo != null)
                                            {
                                                lstInsumos.Add(objInsumo);

                                                if (objInsumo.ACODIGO.Substring(0, 2) == "02")
                                                {
                                                    throw new Exception(string.Format(@"El insumo ""0{0}"" no es inventariable.", det.insumo));
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception(string.Format(@"No se encuentra la información para el insumo ""0{0}"".", det.insumo));
                                            }
                                        }
                                        #endregion

                                        foreach (var grp in groupAutorizados)
                                        {
                                            #region Validación Almacén Activo
                                            string almacenStarsoftOrigen = grp.almacenOrigen.ToString("D2");
                                            var registroAlmacenOrigen = dbStarsoft.TABALM.FirstOrDefault(e => e.TAALMA == almacenStarsoftOrigen);

                                            if (registroAlmacenOrigen == null)
                                            {
                                                throw new Exception("No se encuentra la información del almacén origen.");
                                            }

                                            string almacenStarsoftDestino = grp.almacenDestino.ToString("D2");
                                            var registroAlmacenDestino = dbStarsoft.TABALM.FirstOrDefault(e => e.TAALMA == almacenStarsoftDestino);

                                            if (registroAlmacenDestino == null)
                                            {
                                                throw new Exception("No se encuentra la información del almacén destino.");
                                            }
                                            #endregion

                                            #region Validación para checar que se haya capturado la entrada completa
                                            foreach (var ren in grp.renglones)
                                            {
                                                if (ren.listUbicacionMovimiento.Count() <= 0 || ren.listUbicacionMovimiento == null)
                                                {
                                                    throw new Exception("No se capturó las ubicaciones para la partida #" + ren.partida);
                                                }
                                            }
                                            #endregion

                                            int nuevoNumero = dbStarsoft.MOVALMCAB.ToList().Where(x => Int32.Parse(x.CAALMA) == grp.almacenDestino && x.CATD == "NI").Select(x => Int32.Parse(x.CANUMDOC)).OrderByDescending(x => x).FirstOrDefault() + 1;

                                            #region Tipo de Cambio
                                            decimal tipoCambioPeru = 0M;

                                            using (var dbStarsoftConta = new MainContextPeruStarSoft003BDCONTABILIDAD())
                                            {
                                                var tipoCambioActual = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.Date);
                                                if (tipoCambioActual != null)
                                                {
                                                    tipoCambioPeru = tipoCambioActual.TIPOCAMB_COMPRA;
                                                }
                                                else
                                                {
                                                    tipoCambioPeru = dbStarsoftConta.TIPO_CAMBIO.ToList().FirstOrDefault(e => e.TIPOCAMB_FECHA.Date == DateTime.Now.AddDays(-1).Date).TIPOCAMB_COMPRA;
                                                }

                                            }
                                            #endregion

                                            #region Obtener Salida Traspaso
                                            var salidaTraspasoSIGOPLAN = _context.tblAlm_Movimientos.FirstOrDefault(x =>
                                                x.estatusHabilitado &&
                                                x.almacen == grp.almacenOrigen &&
                                                x.tipo_mov == 52 &&
                                                x.cc == grp.ccOrigen &&
                                                x.alm_destino == almacenDestinoOriginal &&
                                                x.cc_destino == grp.ccDestino &&
                                                x.orden_ct == folio_traspaso
                                            );

                                            if (salidaTraspasoSIGOPLAN == null)
                                            {
                                                throw new Exception("No se encuentra la información de la salida en SIGOPLAN.");
                                            }

                                            List<tblAlm_MovimientosDet> detalleSalidaTraspaso = _context.tblAlm_MovimientosDet.Where(x =>
                                                x.estatusHabilitado && x.almacen == salidaTraspasoSIGOPLAN.almacen && x.tipo_mov == 52 && x.numero == salidaTraspasoSIGOPLAN.numero
                                            ).ToList();
                                            #endregion

                                            #region Calcular Total
                                            decimal total = 0;

                                            foreach (var ren in grp.renglones)
                                            {
                                                foreach (var ubi in ren.listUbicacionMovimiento)
                                                {
                                                    decimal precio = Convert.ToDecimal(detalleSalidaTraspaso.FirstOrDefault(x =>
                                                        (int)x.partida == ren.partida && (int)x.insumo == ubi.insumo
                                                    ).precio, CultureInfo.InvariantCulture);
                                                    total += ubi.cantidadMovimiento * precio;
                                                }
                                            }
                                            #endregion

                                            #region Movimiento SIGOPLAN
                                            tblAlm_Movimientos nuevaEntrada = new tblAlm_Movimientos
                                            {
                                                almacen = grp.almacenDestino,
                                                tipo_mov = 2,
                                                numero = nuevoNumero,
                                                cc = grp.ccDestino.ToUpper(),
                                                compania = 1,
                                                periodo = DateTime.Now.Month,
                                                ano = DateTime.Now.Year,
                                                orden_ct = folio_traspaso,
                                                frente = 0,
                                                fecha = DateTime.Now.Date,
                                                proveedor = 0,
                                                total = total,
                                                estatus = "A",
                                                transferida = "N",
                                                alm_destino = grp.almacenOrigen,
                                                cc_destino = grp.ccOrigen.ToUpper(),
                                                comentarios = grp.renglones[0].comentarios, //Se escoge el primer renglón ya que se juntaron para que nomás generen un movimiento.
                                                tipo_trasp = "0",
                                                tipo_cambio = 1,
                                                numeroReq = salidaTraspasoSIGOPLAN != null ? salidaTraspasoSIGOPLAN.numeroReq : 0,
                                                estatusHabilitado = true
                                            };

                                            _context.tblAlm_Movimientos.Add(nuevaEntrada);
                                            _context.SaveChanges();
                                            #endregion

                                            #region Movimiento Starsoft
                                            var salidaTraspasoStarsoft = dbStarsoft.MOVALMCAB.ToList().FirstOrDefault(x =>
                                                Int32.Parse(x.CAALMA) == salidaTraspasoSIGOPLAN.almacen && x.CATD == "NS" && Int32.Parse(x.CANUMDOC) == salidaTraspasoSIGOPLAN.numero
                                            );

                                            if (salidaTraspasoStarsoft == null)
                                            {
                                                throw new Exception("No se encuentra la información de la salida en Starsoft.");
                                            }

                                            salidaTraspasoStarsoft.CARFNDOC = nuevoNumero.ToString("D10");
                                            dbStarsoft.SaveChanges();

                                            MOVALMCAB objCrearMovCab = new MOVALMCAB();

                                            objCrearMovCab.CAALMA = almacenStarsoftDestino;
                                            objCrearMovCab.CATD = "NI";
                                            objCrearMovCab.CANUMDOC = nuevoNumero.ToString("D10");
                                            objCrearMovCab.CAFECDOC = DateTime.Now.Date;
                                            objCrearMovCab.CATIPMOV = "I";
                                            objCrearMovCab.CACODMOV = "TD";
                                            objCrearMovCab.CASITUA = null;
                                            objCrearMovCab.CARFTDOC = "NS";
                                            objCrearMovCab.CARFNDOC = salidaTraspasoStarsoft.CANUMDOC;
                                            objCrearMovCab.CASOLI = null;
                                            objCrearMovCab.CAFECDEV = null;
                                            objCrearMovCab.CACODPRO = "";
                                            objCrearMovCab.CACENCOS = "";
                                            objCrearMovCab.CARFALMA = almacenStarsoftOrigen;
                                            objCrearMovCab.CAGLOSA = null;
                                            objCrearMovCab.CAFECACT = DateTime.Now.Date;
                                            objCrearMovCab.CAHORA = DateTime.Now.ToString("HH:mm:ss");
                                            objCrearMovCab.CAUSUARI = objUsrStarsoftInventarios != null ? objUsrStarsoftInventarios.id_usuario_inventarios : "01";
                                            objCrearMovCab.CACODCLI = "";
                                            objCrearMovCab.CARUC = null;
                                            objCrearMovCab.CANOMCLI = "";
                                            objCrearMovCab.CAFORVEN = null;
                                            objCrearMovCab.CACODMON = "MN"; //PENDIENTE TIPO DE MONED;
                                            objCrearMovCab.CAVENDE = null;
                                            objCrearMovCab.CATIPCAM = tipoCambioPeru;
                                            objCrearMovCab.CATIPGUI = null;
                                            objCrearMovCab.CASITGUI = "V";
                                            objCrearMovCab.CAGUIFAC = null;
                                            objCrearMovCab.CADIRENV = null;
                                            objCrearMovCab.CACODTRAN = null;
                                            objCrearMovCab.CANUMORD = null;
                                            objCrearMovCab.CAGUIDEV = null;
                                            objCrearMovCab.CANOMPRO = "";
                                            objCrearMovCab.CANROPED = null;
                                            objCrearMovCab.CACOTIZA = null;
                                            objCrearMovCab.CAPORDESCL = 0M;
                                            objCrearMovCab.CAPORDESES = 0M;
                                            objCrearMovCab.CAIMPORTE = 0M;
                                            objCrearMovCab.CANOMTRA = null;
                                            objCrearMovCab.CADIRTRA = null;
                                            objCrearMovCab.CARUCTRA = null;
                                            objCrearMovCab.CAPLATRA = null;
                                            objCrearMovCab.CANROIMP = null;
                                            objCrearMovCab.CACODLIQ = null;
                                            objCrearMovCab.CAESTIMP = "V";
                                            objCrearMovCab.CACIERRE = false;
                                            objCrearMovCab.CATIPDEP = null;
                                            objCrearMovCab.CAZONAF = null;
                                            objCrearMovCab.FLAGGS = false;
                                            objCrearMovCab.ASIENTO = false;
                                            objCrearMovCab.CAFLETE = 0M;
                                            objCrearMovCab.CAORDFAB = null;
                                            objCrearMovCab.CAPEDREFE = null;
                                            objCrearMovCab.CAIMPORTACION = false;
                                            objCrearMovCab.CANROCAJAS = 0;
                                            objCrearMovCab.CAPESOTOTAL = 0M;
                                            objCrearMovCab.CADESPACHO = false;
                                            objCrearMovCab.LINVCODIGO = null;
                                            objCrearMovCab.COD_DIRECCION = null;
                                            objCrearMovCab.COSTOMIN = 0M;
                                            objCrearMovCab.CAINTERFACE = 0;
                                            objCrearMovCab.CACTACONT = null;
                                            objCrearMovCab.CACONTROLSTOCK = "S";
                                            objCrearMovCab.CANOMRECEP = null;
                                            objCrearMovCab.CADNIRECEP = null;
                                            objCrearMovCab.CFDIREREFE = null;
                                            objCrearMovCab.REG_COMPRA = false;
                                            objCrearMovCab.OC_NI_GUIA = false;
                                            objCrearMovCab.COD_AUDITORIA = "0";
                                            objCrearMovCab.COD_MODULO = "03";
                                            objCrearMovCab.NO_GIRO_NEGOCIO = false;
                                            objCrearMovCab.MOTIVO_ANULACION_DOC_ELECTRONICO = null;
                                            objCrearMovCab.DOCUMENTO_ELECTRONICO = null;
                                            objCrearMovCab.GS_BAJA = null;
                                            objCrearMovCab.CADocumentoImportado = null;
                                            objCrearMovCab.SOLICITANTE = null;
                                            objCrearMovCab.DOCUMENTO_CONTINGENCIA = null;
                                            objCrearMovCab.GE_BAJA = null;

                                            dbStarsoft.MOVALMCAB.Add(objCrearMovCab);
                                            dbStarsoft.SaveChanges();
                                            #endregion

                                            int partidaContador = 1;
                                            string almacenOrigenDesc = dbStarsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == grp.almacenOrigen).TADESCRI;
                                            string almacenDestinoDesc = dbStarsoft.TABALM.ToList().FirstOrDefault(x => Int32.Parse(x.TAALMA) == grp.almacenDestino).TADESCRI;
                                            string centroCostoOrigen = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == grp.ccOrigen).cc;
                                            string centroCostoDestino = _context.tblC_Nom_CatalogoCC.FirstOrDefault(x => x.cc == grp.ccDestino).cc;
                                            string recibio = _context.tblP_Usuario.Where(x => x.id == vSesiones.sesionUsuarioDTO.id).Select(x => x.nombre + " " + x.apellidoPaterno + " " + x.apellidoMaterno).FirstOrDefault();

                                            foreach (var renglon in grp.renglones)
                                            {
                                                foreach (var ubi in renglon.listUbicacionMovimiento)
                                                {
                                                    int partida = partidaContador++;
                                                    var costoPromedio = detalleSalidaTraspaso.FirstOrDefault(x => x.insumo == ubi.insumo).precio;

                                                    if (costoPromedio == 0)
                                                    {
                                                        throw new Exception("Costo Promedio en cero para el insumo " + ("0" + ubi.insumo));
                                                    }

                                                    var objInsumo = lstInsumos.FirstOrDefault(e => Int64.Parse(e.ACODIGO) == ubi.insumo);

                                                    #region Movimiento Detalle SIGOPLAN
                                                    tblAlm_MovimientosDet nuevaEntradaDet = new tblAlm_MovimientosDet
                                                    {
                                                        almacen = grp.almacenDestino,
                                                        tipo_mov = 2,
                                                        numero = nuevoNumero,
                                                        partida = partida,
                                                        insumo = ubi.insumo,
                                                        comentarios = renglon.comentarios,
                                                        area = 0,
                                                        cuenta = 0,
                                                        cantidad = ubi.cantidadMovimiento,
                                                        precio = costoPromedio,
                                                        importe = ubi.cantidadMovimiento * costoPromedio,
                                                        id_resguardo = 0,
                                                        area_alm = ubi.area_alm ?? "",
                                                        lado_alm = ubi.lado_alm ?? "",
                                                        estante_alm = ubi.estante_alm ?? "",
                                                        nivel_alm = ubi.nivel_alm ?? "",
                                                        transporte = "",
                                                        estatusHabilitado = true
                                                    };

                                                    _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                                                    _context.SaveChanges();
                                                    #endregion

                                                    #region Movimiento Detalle Starsoft
                                                    var importe = nuevaEntradaDet.cantidad * costoPromedio;

                                                    if (costoPromedio <= 0 || importe <= 0)
                                                    {
                                                        throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                                                    }

                                                    #region INSERT MOVALMDET
                                                    dbStarsoft.MovAlmDet.Add(new MovAlmDet()
                                                    {
                                                        DEALMA = almacenStarsoftDestino,
                                                        DETD = "NI",
                                                        DENUMDOC = nuevoNumero.ToString("D10"),
                                                        DEITEM = partidaContador,
                                                        DECODIGO = ubi.insumo.ToString("D11"),
                                                        DECODREF = null,
                                                        DECANTID = ubi.cantidadMovimiento,
                                                        DECANTENT = 0M,
                                                        DECANREF = 0M,
                                                        DECANFAC = 0M,
                                                        DEORDEN = null,
                                                        DEPREUNI = 0M,
                                                        DEPRECIO = costoPromedio,
                                                        DEPRECI1 = 0M,
                                                        DEDESCTO = 0M,
                                                        DESTOCK = null,
                                                        DEIGV = 0M,
                                                        DEIMPMN = costoPromedio,
                                                        DEIMPUS = costoPromedio * tipoCambioPeru,
                                                        DESERIE = null,
                                                        DESITUA = null,
                                                        DEFECDOC = null,
                                                        DECENCOS = "",
                                                        DERFALMA = null,
                                                        DETR = null,
                                                        DEESTADO = "V",
                                                        DECODMOV = "TD",
                                                        DEVALTOT = 0M,
                                                        DECOMPRO = null,
                                                        DECODMON = "MN",
                                                        DETIPO = null,
                                                        DETIPCAM = tipoCambioPeru,
                                                        DEPREVTA = null,
                                                        DEMONVTA = null,
                                                        DEFECVEN = null,
                                                        DEDEVOL = 0M,
                                                        DESOLI = null,
                                                        DEDESCRI = objInsumo.ADESCRI,
                                                        DEPORDES = 0M,
                                                        DEIGVPOR = 0M,
                                                        DEDESCLI = 0M,
                                                        DEDESESP = 0M,
                                                        DENUMFAC = null,
                                                        DELOTE = null,
                                                        DEUNIDAD = objInsumo.AUNIDAD,
                                                        DECANTBRUTA = 0M,
                                                        DEDSCTCANTBRUTA = 0M,
                                                        DEORDFAB = "",
                                                        DEQUIPO = null,
                                                        DEFLETE = 0M,
                                                        DEITEMI = null, //????????
                                                        DEGLOSA = "",
                                                        DEVALORIZADO = true,
                                                        DESECUENORI = null,
                                                        DEREFERENCIA = null,
                                                        UMREFERENCIA = null,
                                                        CANTREFERENCIA = 0M,
                                                        DECUENTA = null,
                                                        DETEXTO = null,
                                                        CTA_CONSUMO = null,
                                                        CODPARTE = "",
                                                        CODPLANO = "",
                                                        DETPRODUCCION = 0,
                                                        MPMA = "",
                                                        PorcentajeCosto = 0M,
                                                        SALDO_NC = null,
                                                        DEPRECIOREF = 0M,
                                                    });
                                                    dbStarsoft.SaveChanges();
                                                    #endregion

                                                    #region INSERT STKART
                                                    var obkStkart = dbStarsoft.STKART.Where(e => e.STALMA == almacenStarsoftDestino).ToList().FirstOrDefault(e => Int64.Parse(e.STCODIGO) == ubi.insumo);

                                                    if (obkStkart != null)
                                                    {
                                                        obkStkart.STSKDIS += ubi.cantidadMovimiento;

                                                        dbStarsoft.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        var nuevoRegistroStock = new STKART();

                                                        nuevoRegistroStock.STALMA = almacenStarsoftDestino;
                                                        nuevoRegistroStock.STCODIGO = ubi.insumo.ToString("D11");
                                                        nuevoRegistroStock.STSKDIS = ubi.cantidadMovimiento;
                                                        nuevoRegistroStock.STSKREF = 0M;
                                                        nuevoRegistroStock.STSKMIN = 0M;
                                                        nuevoRegistroStock.STSKMAX = 0M;
                                                        nuevoRegistroStock.STPUNREP = 0M;
                                                        nuevoRegistroStock.STSEMREP = 0M;
                                                        nuevoRegistroStock.STTIPREP = null;
                                                        nuevoRegistroStock.STUBIALM = null;
                                                        nuevoRegistroStock.STLOTCOM = 0M;
                                                        nuevoRegistroStock.STTIPCOM = null;
                                                        nuevoRegistroStock.STSKCOM = 0M;
                                                        nuevoRegistroStock.STKPREPRO = costoPromedio;
                                                        nuevoRegistroStock.STKPREULT = 0M;
                                                        nuevoRegistroStock.STKFECULT = null;
                                                        nuevoRegistroStock.STKPREPROUS = costoPromedio / (tipoCambioPeru > 0 ? tipoCambioPeru : 1);
                                                        nuevoRegistroStock.CANTREFERENCIA = 0;

                                                        dbStarsoft.STKART.Add(nuevoRegistroStock);
                                                        dbStarsoft.SaveChanges();
                                                    }
                                                    #endregion

                                                    #region MORESMES
                                                    var objMoResMes = dbStarsoft.MoResMes.ToList().FirstOrDefault(e => e.SMALMA == almacenStarsoftOrigen && e.SMMESPRO == (DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM")) && Int64.Parse(e.SMCODIGO) == ubi.insumo);

                                                    if (objMoResMes != null)
                                                    {
                                                        objMoResMes.SMCANENT += ubi.cantidadMovimiento;

                                                        dbStarsoft.SaveChanges();
                                                    }
                                                    #endregion
                                                    #endregion

                                                    #region Información para el reporte
                                                    var insumoFormatoStarsoft = nuevaEntradaDet.insumo.ToString("D11");
                                                    var insumoDesc = dbStarsoft.MAEART.Where(x => x.ACODIGO == insumoFormatoStarsoft).Select(x => x.ADESCRI).FirstOrDefault();
                                                    var area = nuevaEntradaDet.area;
                                                    var cuenta = nuevaEntradaDet.cuenta;
                                                    var cantidad = nuevaEntradaDet.cantidad;
                                                    var precio = nuevaEntradaDet.precio;
                                                    var area_alm = nuevaEntradaDet.area_alm ?? "";
                                                    var lado_alm = nuevaEntradaDet.lado_alm ?? "";
                                                    var estante_alm = nuevaEntradaDet.estante_alm ?? "";
                                                    var nivel_alm = nuevaEntradaDet.nivel_alm ?? "";

                                                    movEntradas.Add(new salidasAlmacenDTO
                                                    {
                                                        centroCostoOrigen = centroCostoOrigen,
                                                        almacenOrigen = almacenOrigenDesc,
                                                        centroCostoDestino = centroCostoDestino,
                                                        almacenDestino = almacenDestinoDesc,
                                                        ordenTraspaso = nuevaEntrada.orden_ct.ToString(),
                                                        numero = nuevaEntrada.numero.ToString(),
                                                        recibio = recibio,

                                                        fechaEntrada = nuevaEntrada.fecha,
                                                        partida = nuevaEntradaDet.partida,
                                                        insumo = nuevaEntradaDet.insumo + "-" + insumoDesc ?? "",
                                                        areaCuenta = area + "-" + cuenta,
                                                        cantidad = cantidad,
                                                        precio = precio,
                                                        costoPromedio = precio,
                                                        importe = cantidad * precio,
                                                        comentarios = nuevaEntrada.comentarios ?? "",
                                                        area_alm = area_alm,
                                                        lado_alm = lado_alm,
                                                        estante_alm = estante_alm,
                                                        nivel_alm = nivel_alm
                                                    });
                                                    #endregion
                                                }
                                            }

                                            #region Actualizar Registro SIGOPLAN
                                            if (!grp.traspasoSinOrigen)
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    var entrada = _context.tblCom_SurtidoDet.FirstOrDefault(x =>
                                                        (x.estadoSurtido == "T" || x.estadoSurtido == "P") &&
                                                        x.estatus &&
                                                        x.cc == grp.ccOrigen &&
                                                        x.numero == ren.numero &&
                                                        x.insumo == ren.insumo
                                                    );

                                                    if (entrada != null)
                                                    {
                                                        if (ren.cantidad == entrada.cantidad)
                                                        {
                                                            entrada.estadoSurtido = "S";

                                                            _context.Entry(entrada).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    var traspaso = _context.tblAlm_Traspaso.FirstOrDefault(x => x.estatusRegistro && x.id == ren.traspasoID);

                                                    if (traspaso != null)
                                                    {
                                                        traspaso.estado = "S"; //Se pone como surtido aunque sea parcial.

                                                        _context.Entry(traspaso).State = System.Data.Entity.EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        dbStarsoftTransaction.Commit();
                                        dbSigoplanTransaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        dbStarsoftTransaction.Rollback();
                                        dbSigoplanTransaction.Rollback();

                                        LogError(12, 0, "RequisicionController", "GuardarEntradas", e, AccionEnum.AGREGAR, 0, new { entradas = entradas, folio_traspaso = folio_traspaso, almacenDestinoOriginal = almacenDestinoOriginal });

                                        throw new Exception(e.Message);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    break;
                default:
                    {
                        #region DEMÁS EMPRESAS
                        #region Validación Sobrepaso Salida
                        #region Salida
                        var salidaEK = consultaCheckProductivo(string.Format(@"
                SELECT 
                    * 
                FROM si_movimientos 
                WHERE almacen = {0} AND tipo_mov = 52 AND alm_destino = {1} AND cc = '{2}' AND cc_destino = '{3}' AND folio_traspaso = {4}", entradas[0].almacenOrigenID, entradas[0].almacenDestinoID, entradas[0].cc, entradas[0].ccDestino, entradas[0].folio_traspaso)
                        );

                        if (salidaEK == null)
                        {
                            throw new Exception("No se encuentra la información de la salida.");
                        }

                        var salida = ((List<dynamic>)salidaEK.ToObject<List<dynamic>>())[0];
                        var salidaDetEK = consultaCheckProductivo(string.Format(@"
                SELECT 
                    * 
                FROM si_movimientos_det 
                WHERE almacen = {0} AND tipo_mov = 52 AND numero = {1}", salida.almacen, salida.numero)
                        );

                        if (salidaDetEK == null)
                        {
                            throw new Exception("No se encuentra la información de las partidas de la salida.");
                        }

                        var salidaDet = (List<dynamic>)salidaDetEK.ToObject<List<dynamic>>();
                        #endregion

                        #region Entradas Capturadas
                        var entradasCapturadasEK = consultaCheckProductivo(string.Format(@"
                SELECT 
                    * 
                FROM si_movimientos 
                WHERE almacen = {0} AND tipo_mov = 2 AND alm_destino = {1} AND cc = '{2}' AND cc_destino = '{3}' AND folio_traspaso = {4}", entradas[0].almacenDestinoID, entradas[0].almacenOrigenID, entradas[0].ccDestino, entradas[0].cc, entradas[0].folio_traspaso)
                        );

                        var entradasCapturadas = new List<dynamic>();
                        var entradasCapturadasDet = new List<dynamic>();

                        if (entradasCapturadasEK != null)
                        {
                            entradasCapturadas = (List<dynamic>)entradasCapturadasEK.ToObject<List<dynamic>>();

                            var entradasCapturadasDetEK = consultaCheckProductivo(string.Format(@"
                    SELECT 
                        * 
                    FROM si_movimientos_det 
                    WHERE almacen = {0} AND tipo_mov = 2 AND numero IN ({1})", entradas[0].almacenDestinoID, string.Join(", ", entradasCapturadas.Select(x => x.numero).ToList()))
                            );

                            if (entradasCapturadasDetEK != null)
                            {
                                entradasCapturadasDet = (List<dynamic>)entradasCapturadasDetEK.ToObject<List<dynamic>>();
                            }
                        }
                        #endregion

                        foreach (var sal in salidaDet)
                        {
                            var cantidadSalida = Convert.ToDecimal(sal.cantidad, CultureInfo.InvariantCulture);
                            var cantidadEntradasCapturadas = entradasCapturadasDet.Where(x => x.partida == sal.partida && x.insumo == sal.insumo).Sum(x => x.cantidad);
                            var entradasPendientes = entradas.Where(x => x.partida == (int)sal.partida && x.insumo == (int)sal.insumo && x.listUbicacionMovimiento != null && x.listUbicacionMovimiento.Count() > 0).ToList();
                            var cantidadEntradas = entradasPendientes.Sum(x => x.listUbicacionMovimiento.Sum(y => y.cantidad));
                            var cantidadEntradasTotal = cantidadEntradasCapturadas + cantidadEntradas;

                            if (cantidadEntradasTotal > cantidadSalida)
                            {
                                throw new Exception("La cantidad de entrada (" + cantidadEntradas + (cantidadEntradasCapturadas > 0 ? " + " + cantidadEntradasCapturadas : "") + ") sobrepasa la cantidad de salida (" + cantidadSalida + ") en la partida #" + sal.partida + ". </p><br><p style='color: red;'>ATENCIÓN: Se toma en cuenta las entradas por traspaso ya capturadas para la salida en caso de que existan.</p><p>");
                            }
                        }
                        #endregion

                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            using (var con = checkConexionProductivo())
                            {
                                using (var trans = con.BeginTransaction())
                                {
                                    try
                                    {
                                        var groupAutorizados = entradas.GroupBy(x => new
                                        {
                                            x.cc,
                                            x.almacenOrigenID,
                                            x.ccDestino,
                                            x.almacenDestinoID,
                                            x.traspasoSinOrigen
                                        })
                                        .Select(grupo => new
                                        {
                                            ccOrigen = grupo.Key.cc,
                                            almacenOrigen = grupo.Key.almacenOrigenID,
                                            ccDestino = grupo.Key.ccDestino,
                                            almacenDestino = grupo.Key.almacenDestinoID,
                                            traspasoSinOrigen = grupo.Key.traspasoSinOrigen,
                                            renglones = grupo.ToList()
                                        });

                                        #region Validación Folio de Traspaso mayor a cero
                                        if (folio_traspaso <= 0)
                                        {
                                            throw new Exception("Debe capturar un folio de traspaso válido.");
                                        }
                                        #endregion

                                        #region Validación Inventariables
                                        var registrosGrupoInsumo = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM grupos_insumo")).ToObject<List<dynamic>>();

                                        foreach (var grp in groupAutorizados)
                                        {
                                            foreach (var ren in grp.renglones)
                                            {
                                                foreach (var ubi in ren.listUbicacionMovimiento)
                                                {
                                                    var tipo_insumo = Int32.Parse(ubi.insumo.ToString().Substring(0, 1));
                                                    var grupo_insumo = Int32.Parse(ubi.insumo.ToString().Substring(1, 2));
                                                    var registroGrupoInsumo = registrosGrupoInsumo.FirstOrDefault(x => (int)x.tipo_insumo == tipo_insumo && (int)x.grupo_insumo == grupo_insumo);

                                                    if (registroGrupoInsumo != null)
                                                    {
                                                        if ((string)registroGrupoInsumo.inventariado != "I")
                                                        {
                                                            throw new Exception(string.Format(@"El insumo ""{0}"" no es inventariable.", ubi.insumo));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        throw new Exception(string.Format(@"No se encuentra la información del grupo de insumo para el insumo ""{0}"".", ubi.insumo));
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        var count = 0;

                                        foreach (var grp in groupAutorizados)
                                        {
                                            #region Validación Almacén Activo
                                            var almacenOrigenEnkontrol = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(),
                                                new OdbcConsultaDTO()
                                                {
                                                    consulta = @"SELECT * FROM si_almacen WHERE almacen = ?",
                                                    parametros = new List<OdbcParameterDTO>() {
                                        new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = grp.almacenOrigen }
                                    }
                                                }
                                            );

                                            if (almacenOrigenEnkontrol.Count() == 0)
                                            {
                                                throw new Exception("No se encuentra la información del almacén origen.");
                                            }

                                            if ((string)almacenOrigenEnkontrol[0].bit_mp != "S")
                                            {
                                                throw new Exception("El almacén origen no está activo.");
                                            }

                                            var almacenDestinoEnkontrol = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(),
                                                new OdbcConsultaDTO()
                                                {
                                                    consulta = @"SELECT * FROM si_almacen WHERE almacen = ?",
                                                    parametros = new List<OdbcParameterDTO>() {
                                        new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = grp.almacenDestino }
                                    }
                                                }
                                            );

                                            if (almacenDestinoEnkontrol.Count() == 0)
                                            {
                                                throw new Exception("No se encuentra la información del almacén destino.");
                                            }

                                            if ((string)almacenDestinoEnkontrol[0].bit_mp != "S")
                                            {
                                                throw new Exception("El almacén destino no está activo.");
                                            }
                                            #endregion

                                            #region Validación para checar que se haya capturado la entrada completa
                                            foreach (var ren in grp.renglones)
                                            {
                                                if (ren.listUbicacionMovimiento.Count() <= 0 || ren.listUbicacionMovimiento == null)
                                                {
                                                    throw new Exception("No se capturó las ubicaciones para la partida #" + ren.partida);
                                                }
                                            }
                                            #endregion

                                            var ultimoMovimientoEK = consultaCheckProductivo(
                                                string.Format(@"SELECT 
                                                    TOP 1 numero, folio_traspaso 
                                                FROM si_movimientos 
                                                WHERE almacen = {0} AND tipo_mov = {1} 
                                                ORDER BY numero DESC", grp.almacenDestino, 2)
                                                );

                                            var nuevoFolio = 0;

                                            if (ultimoMovimientoEK != null)
                                            {
                                                var ultimoMovimiento = ((List<SurtidoDetDTO>)ultimoMovimientoEK.ToObject<List<SurtidoDetDTO>>())[0];

                                                nuevoFolio = ultimoMovimiento.numero;
                                            }

                                            List<dynamic> detalleSalidaTraspaso = new List<dynamic>();

                                            if (folio_traspaso > 0)
                                            {
                                                var salidaTraspasoEK = consultaCheckProductivo(
                                                    string.Format(@"SELECT 
                                                            * 
                                                        FROM si_movimientos 
                                                        WHERE almacen = {0} AND tipo_mov = 52 AND cc = '{1}' AND alm_destino = {2} AND cc_destino = '{3}' AND folio_traspaso = {4}",
                                                                    grp.almacenOrigen, grp.ccOrigen,
                                                                    almacenDestinoOriginal, //Se toma el almacén destino original para consultar la información de la salida. //grp.almacenDestino,
                                                                    grp.ccDestino, folio_traspaso));

                                                if (salidaTraspasoEK != null)
                                                {
                                                    var salidaTraspaso = ((List<dynamic>)salidaTraspasoEK.ToObject<List<dynamic>>())[0];
                                                    var detalleSalidaTraspasoEK = consultaCheckProductivo(
                                                        string.Format(@"SELECT * FROM si_movimientos_det WHERE almacen = {0} AND tipo_mov = 52 AND numero = {1}",
                                                                        (int)salidaTraspaso.almacen, (int)salidaTraspaso.numero));

                                                    if (detalleSalidaTraspasoEK != null)
                                                    {
                                                        detalleSalidaTraspaso = (List<dynamic>)detalleSalidaTraspasoEK.ToObject<List<dynamic>>();
                                                    }
                                                }
                                            }

                                            decimal total = 0;

                                            if (folio_traspaso > 0)
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    foreach (var ubi in ren.listUbicacionMovimiento)
                                                    {
                                                        decimal precio = Convert.ToDecimal(detalleSalidaTraspaso.FirstOrDefault(x =>
                                                            (int)x.partida == ren.partida && (int)x.insumo == ubi.insumo
                                                        ).precio, CultureInfo.InvariantCulture);
                                                        total += ubi.cantidadMovimiento * precio;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    foreach (var ubi in ren.listUbicacionMovimiento)
                                                    {
                                                        decimal costoPromedio = getCostoPromedioNuevo(grp.almacenOrigen, ubi.insumo);
                                                        total += ubi.cantidadMovimiento * costoPromedio;
                                                    }
                                                }
                                            }

                                            var salidaTraspasoSIGOPLAN = _context.tblAlm_Movimientos.Where(x =>
                                                x.cc == grp.ccOrigen && x.almacen == grp.almacenOrigen && x.tipo_mov == 52 && x.orden_ct == folio_traspaso
                                            ).FirstOrDefault();

                                            #region Movimiento SIGOPLAN
                                            tblAlm_Movimientos nuevaEntrada = new tblAlm_Movimientos
                                            {
                                                almacen = grp.almacenDestino,
                                                tipo_mov = 2,
                                                numero = nuevoFolio + 1,
                                                cc = grp.ccDestino.ToUpper(),
                                                compania = 1,
                                                periodo = DateTime.Now.Month,
                                                ano = DateTime.Now.Year,
                                                orden_ct = folio_traspaso,
                                                frente = 0,
                                                fecha = DateTime.Now.Date,
                                                proveedor = 0,
                                                total = total,
                                                estatus = "A",
                                                transferida = "N",
                                                alm_destino = grp.almacenOrigen,
                                                cc_destino = grp.ccOrigen.ToUpper(),
                                                comentarios = grp.renglones[0].comentarios, //Se escoge el primer renglón ya que se juntaron para que nomás generen un movimiento.
                                                tipo_trasp = "0",
                                                tipo_cambio = 1,
                                                numeroReq = salidaTraspasoSIGOPLAN != null ? salidaTraspasoSIGOPLAN.numeroReq : 0,
                                                estatusHabilitado = true
                                            };

                                            _context.tblAlm_Movimientos.Add(nuevaEntrada);
                                            _context.SaveChanges();
                                            #endregion

                                            #region Movimiento Enkontrol
                                            if (nuevaEntrada.total <= 0)
                                            {
                                                throw new Exception("El total no puede ser igual o menor a cero.");
                                            }

                                            var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                            using (var cmd = new OdbcCommand(consultaMovimientos))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@almacen", OdbcType.Numeric).Value = nuevaEntrada.almacen;
                                                parameters.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaEntrada.tipo_mov;
                                                parameters.Add("@numero", OdbcType.Numeric).Value = nuevaEntrada.numero;
                                                parameters.Add("@cc", OdbcType.Char).Value = nuevaEntrada.cc;
                                                parameters.Add("@compania", OdbcType.Numeric).Value = nuevaEntrada.compania;
                                                parameters.Add("@periodo", OdbcType.Numeric).Value = nuevaEntrada.periodo;
                                                parameters.Add("@ano", OdbcType.Numeric).Value = nuevaEntrada.ano;
                                                parameters.Add("@orden_ct", OdbcType.Numeric).Value = nuevaEntrada.orden_ct;
                                                parameters.Add("@frente", OdbcType.Numeric).Value = nuevaEntrada.frente;
                                                parameters.Add("@fecha", OdbcType.Date).Value = nuevaEntrada.fecha.Date;
                                                parameters.Add("@proveedor", OdbcType.Numeric).Value = nuevaEntrada.proveedor;
                                                parameters.Add("@total", OdbcType.Numeric).Value = nuevaEntrada.total;
                                                parameters.Add("@estatus", OdbcType.Char).Value = nuevaEntrada.estatus;
                                                parameters.Add("@transferida", OdbcType.Char).Value = nuevaEntrada.transferida;
                                                parameters.Add("@poliza", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@empleado", OdbcType.Numeric).Value = empleado;
                                                parameters.Add("@alm_destino", OdbcType.Numeric).Value = nuevaEntrada.alm_destino;
                                                parameters.Add("@cc_destino", OdbcType.Char).Value = nuevaEntrada.cc_destino;
                                                parameters.Add("@comentarios", OdbcType.Char).Value = nuevaEntrada.comentarios != null ? nuevaEntrada.comentarios : "";
                                                parameters.Add("@tipo_trasp", OdbcType.Char).Value = nuevaEntrada.tipo_trasp;
                                                parameters.Add("@numero_destino", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                                                parameters.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                                                parameters.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                                                parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaEntrada.tipo_cambio;
                                                parameters.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                                                parameters.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                                                parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                                                parameters.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                                                parameters.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                                                parameters.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                                                parameters.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                                                parameters.Add("@folio_traspaso", OdbcType.Numeric).Value = folio_traspaso;
                                                parameters.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;

                                                count += cmd.ExecuteNonQuery();
                                            }
                                            #endregion

                                            var centroCostoOrigenEK = consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", nuevaEntrada.cc));
                                            var centroCostoOrigen = nuevaEntrada.cc + " - " + ((string)(((List<dynamic>)centroCostoOrigenEK.ToObject<List<dynamic>>())[0].descripcion));
                                            var almacenOrigenEK = consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", nuevaEntrada.almacen));
                                            var almacenOrigen = nuevaEntrada.almacen + " - " + ((string)(((List<dynamic>)almacenOrigenEK.ToObject<List<dynamic>>())[0].descripcion));

                                            var centroCostoDestinoEK = consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", nuevaEntrada.cc_destino));
                                            var centroCostoDestino = nuevaEntrada.cc_destino + " - " + ((string)(((List<dynamic>)centroCostoDestinoEK.ToObject<List<dynamic>>())[0].descripcion));
                                            var almacenDestinoEK = consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", nuevaEntrada.alm_destino));
                                            var almacenDestino = nuevaEntrada.alm_destino + " - " + ((string)(((List<dynamic>)almacenDestinoEK.ToObject<List<dynamic>>())[0].descripcion));

                                            var recibio = (string)(((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM empleados WHERE empleado = {0}", empleado)).ToObject<List<dynamic>>())[0].descripcion);

                                            int partidaContador = 1;

                                            foreach (var renglon in grp.renglones)
                                            {
                                                #region Actualizar registros de surtido.
                                                if (salidaTraspasoSIGOPLAN != null)
                                                {
                                                    var listaSurtido = _context.tblCom_SurtidoDet.Where(x =>
                                                        x.estatus &&
                                                        x.cc == grp.ccOrigen &&
                                                        x.numero == salidaTraspasoSIGOPLAN.numeroReq &&
                                                        x.insumo == renglon.insumo &&
                                                        x.almacenOrigenID == grp.almacenOrigen &&
                                                        x.almacenDestinoID == grp.almacenDestino &&
                                                        x.estadoSurtido == "T" &&
                                                        x.tipoSurtidoDetalle == "AE"
                                                    ).ToList();

                                                    foreach (var sur in listaSurtido)
                                                    {
                                                        sur.estadoSurtido = "S";
                                                        _context.SaveChanges();
                                                    }
                                                }
                                                #endregion

                                                foreach (var ubi in renglon.listUbicacionMovimiento)
                                                {
                                                    int partida = partidaContador++;

                                                    decimal costoPromedio = 0;

                                                    if (folio_traspaso > 0)
                                                    {
                                                        costoPromedio = Convert.ToDecimal(detalleSalidaTraspaso.FirstOrDefault(x =>
                                                            (int)x.partida == renglon.partida && (int)x.insumo == ubi.insumo
                                                        ).precio, CultureInfo.InvariantCulture);
                                                    }
                                                    else
                                                    {
                                                        costoPromedio = getCostoPromedioNuevo(nuevaEntrada.almacen, ubi.insumo);
                                                    }

                                                    #region Movimiento Detalle SIGOPLAN
                                                    tblAlm_MovimientosDet nuevaEntradaDet = new tblAlm_MovimientosDet
                                                    {
                                                        almacen = grp.almacenDestino,
                                                        tipo_mov = 2,
                                                        numero = nuevoFolio + 1,
                                                        partida = partida,
                                                        insumo = ubi.insumo,
                                                        comentarios = renglon.comentarios,
                                                        area = 0,
                                                        cuenta = 0,
                                                        cantidad = ubi.cantidadMovimiento,
                                                        precio = costoPromedio,
                                                        importe = ubi.cantidadMovimiento * costoPromedio,
                                                        id_resguardo = 0,
                                                        area_alm = ubi.area_alm ?? "",
                                                        lado_alm = ubi.lado_alm ?? "",
                                                        estante_alm = ubi.estante_alm ?? "",
                                                        nivel_alm = ubi.nivel_alm ?? "",
                                                        transporte = "",
                                                        estatusHabilitado = true
                                                    };

                                                    _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                                                    _context.SaveChanges();
                                                    #endregion

                                                    #region Movimiento Detalle Enkontrol
                                                    var importe = nuevaEntradaDet.cantidad * costoPromedio;

                                                    if (costoPromedio <= 0 || importe <= 0)
                                                    {
                                                        throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                                                    }

                                                    var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                                        (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                                        partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                                        remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                                    using (var cmd = new OdbcCommand(consultaMovimientosDetalle))
                                                    {
                                                        OdbcParameterCollection parameters = cmd.Parameters;

                                                        parameters.Add("@almacen", OdbcType.Numeric).Value = nuevaEntradaDet.almacen;
                                                        parameters.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaEntradaDet.tipo_mov;
                                                        parameters.Add("@numero", OdbcType.Numeric).Value = nuevoFolio + 1;
                                                        parameters.Add("@partida", OdbcType.Numeric).Value = nuevaEntradaDet.partida;
                                                        parameters.Add("@insumo", OdbcType.Numeric).Value = nuevaEntradaDet.insumo;
                                                        parameters.Add("@comentarios", OdbcType.Char).Value = nuevaEntradaDet.comentarios != null ? nuevaEntradaDet.comentarios : "";
                                                        parameters.Add("@area", OdbcType.Numeric).Value = nuevaEntradaDet.area;
                                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = nuevaEntradaDet.cuenta;
                                                        parameters.Add("@cantidad", OdbcType.Numeric).Value = nuevaEntradaDet.cantidad;
                                                        parameters.Add("@precio", OdbcType.Numeric).Value = costoPromedio;
                                                        parameters.Add("@importe", OdbcType.Numeric).Value = importe;
                                                        parameters.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@costo_prom", OdbcType.Numeric).Value = costoPromedio;
                                                        parameters.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                                                        parameters.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                                                        parameters.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                                                        parameters.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                                                        parameters.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                                                        parameters.Add("@area_alm", OdbcType.Char).Value = nuevaEntradaDet.area_alm;
                                                        parameters.Add("@lado_alm", OdbcType.Char).Value = nuevaEntradaDet.lado_alm;
                                                        parameters.Add("@estante_alm", OdbcType.Char).Value = nuevaEntradaDet.estante_alm;
                                                        parameters.Add("@nivel_alm", OdbcType.Char).Value = nuevaEntradaDet.nivel_alm;
                                                        parameters.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                                                        cmd.Connection = trans.Connection;
                                                        cmd.Transaction = trans;

                                                        count += cmd.ExecuteNonQuery();
                                                    }
                                                    #endregion

                                                    #region Información para el reporte
                                                    //movEntradas.Add(new entradasAlmacenDTO
                                                    //{
                                                    //    centroCosto = nuevaEntrada.cc,
                                                    //    folioEntrada = (nuevoFolio + 1).ToString(),
                                                    //    almacen = renglon.almacenDestinoDesc,
                                                    //    fechaEntrada = nuevaEntrada.fecha,
                                                    //    partida = nuevaEntradaDet.partida,
                                                    //    insumo = renglon.insumo + "-" + renglon.insumoDesc ?? "",
                                                    //    areaCuenta = nuevaEntradaDet.area + "-" + nuevaEntradaDet.cuenta,
                                                    //    referencia = "",
                                                    //    remision = "",
                                                    //    cantidad = nuevaEntradaDet.cantidad,
                                                    //    precio = nuevaEntradaDet.precio,
                                                    //    importe = nuevaEntradaDet.importe,
                                                    //    comentarios = nuevaEntradaDet.comentarios != null ? nuevaEntradaDet.comentarios : "",
                                                    //    ordenCompra = nuevaEntrada.orden_ct.ToString(),
                                                    //    proveedor = "",
                                                    //    direccion = "",
                                                    //    ciudad = "",
                                                    //    telefonos = "",

                                                    //    area_alm = nuevaEntradaDet.area_alm ?? "",
                                                    //    lado_alm = nuevaEntradaDet.lado_alm ?? "",
                                                    //    estante_alm = nuevaEntradaDet.estante_alm ?? "",
                                                    //    nivel_alm = nuevaEntradaDet.nivel_alm ?? ""
                                                    //});

                                                    var insumoDescEK = consultaCheckProductivo(string.Format(@"SELECT * FROM insumos WHERE insumo = {0}", nuevaEntradaDet.insumo));
                                                    var insumoDesc = (string)(((List<dynamic>)insumoDescEK.ToObject<List<dynamic>>())[0].descripcion);
                                                    var area = nuevaEntradaDet.area;
                                                    var cuenta = nuevaEntradaDet.cuenta;
                                                    var cantidad = nuevaEntradaDet.cantidad;
                                                    var precio = nuevaEntradaDet.precio;
                                                    var area_alm = nuevaEntradaDet.area_alm ?? "";
                                                    var lado_alm = nuevaEntradaDet.lado_alm ?? "";
                                                    var estante_alm = nuevaEntradaDet.estante_alm ?? "";
                                                    var nivel_alm = nuevaEntradaDet.nivel_alm ?? "";

                                                    movEntradas.Add(new salidasAlmacenDTO
                                                    {
                                                        centroCostoOrigen = centroCostoOrigen,
                                                        almacenOrigen = almacenOrigen,
                                                        centroCostoDestino = centroCostoDestino,
                                                        almacenDestino = almacenDestino,
                                                        ordenTraspaso = nuevaEntrada.orden_ct.ToString(),
                                                        numero = nuevaEntrada.numero.ToString(),
                                                        recibio = recibio,

                                                        fechaEntrada = nuevaEntrada.fecha,
                                                        partida = nuevaEntradaDet.partida,
                                                        insumo = nuevaEntradaDet.insumo + "-" + insumoDesc ?? "",
                                                        areaCuenta = area + "-" + cuenta,
                                                        cantidad = cantidad,
                                                        precio = precio,
                                                        costoPromedio = precio,
                                                        importe = cantidad * precio,
                                                        comentarios = nuevaEntrada.comentarios ?? "",
                                                        area_alm = area_alm,
                                                        lado_alm = lado_alm,
                                                        estante_alm = estante_alm,
                                                        nivel_alm = nivel_alm
                                                    });
                                                    #endregion

                                                    #region Actualizar Tablas Acumula
                                                    var objAcumula = new MovimientoDetalleEnkontrolDTO
                                                    {
                                                        insumo = nuevaEntradaDet.insumo,
                                                        cantidad = nuevaEntradaDet.cantidad,
                                                        precio = costoPromedio,
                                                        tipo_mov = nuevaEntradaDet.tipo_mov,
                                                        costo_prom = costoPromedio
                                                    };

                                                    actualizarAcumula(nuevaEntradaDet.almacen, nuevaEntrada.cc, objAcumula, dbSigoplanTransaction, trans);
                                                    #endregion
                                                }
                                            }

                                            #region Actualizar Registro SIGOPLAN
                                            if (!grp.traspasoSinOrigen)
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    var entrada = _context.tblCom_SurtidoDet.FirstOrDefault(x =>
                                                        (x.estadoSurtido == "T" || x.estadoSurtido == "P") &&
                                                        x.estatus &&
                                                        x.cc == grp.ccOrigen &&
                                                        x.numero == ren.numero &&
                                                        x.insumo == ren.insumo
                                                    );

                                                    if (entrada != null)
                                                    {
                                                        if (ren.cantidad == entrada.cantidad)
                                                        {
                                                            entrada.estadoSurtido = "S";

                                                            _context.Entry(entrada).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                foreach (var ren in grp.renglones)
                                                {
                                                    var traspaso = _context.tblAlm_Traspaso.FirstOrDefault(x => x.estatusRegistro && x.id == ren.traspasoID);

                                                    if (traspaso != null)
                                                    {
                                                        traspaso.estado = "S"; //Se pone como surtido aunque sea parcial.

                                                        _context.Entry(traspaso).State = System.Data.Entity.EntityState.Modified;
                                                        _context.SaveChanges();
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Mandar correo al requisitor si el surtido de la requisición está completo.
                                            if (salidaTraspasoSIGOPLAN != null)
                                            {
                                                if (salidaTraspasoSIGOPLAN.numeroReq != null)
                                                {
                                                    var surtidoRequisicionCompleto = verificarRequisicionCompletamenteSurtida(grp.ccOrigen, (int)salidaTraspasoSIGOPLAN.numeroReq);

                                                    if (surtidoRequisicionCompleto)
                                                    {
                                                        var titulo = "Requisición \"" + grp.ccOrigen + "-" + (int)salidaTraspasoSIGOPLAN.numeroReq + "\" completamente surtida.";
                                                        var mensaje = "La requisición \"" + grp.ccOrigen + "-" + (int)salidaTraspasoSIGOPLAN.numeroReq + "\" se ha surtido por completo. \n" + "Fecha Surtido Completo: " + DateTime.Now.Date.ToShortDateString();
                                                        var correo = new List<string>();

                                                        if (empleado != 1)
                                                        {
                                                            var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.id == usuarioEnkontrolSigoplan.idUsuario);

                                                            if (usuarioSIGOPLAN.correo != null)
                                                            {
                                                                correo.Add(usuarioSIGOPLAN.correo);
                                                            }
                                                        }
                                                        //else
                                                        //{
                                                        //    correo.Add("oscar.valencia@construplan.com.mx");
                                                        //}

                                                        if (correo.Count == 1)
                                                        {
                                                            Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), titulo), mensaje, correo);
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        trans.Commit();
                                        dbSigoplanTransaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        trans.Rollback();
                                        dbSigoplanTransaction.Rollback();

                                        LogError(12, 0, "RequisicionController", "GuardarEntradas", e, AccionEnum.AGREGAR, 0, new { entradas = entradas, folio_traspaso = folio_traspaso, almacenDestinoOriginal = almacenDestinoOriginal });

                                        throw new Exception(e.Message);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    break;
            }

            return movEntradas;
        }
        public List<tblCom_SurtidoDet> getReservados(int insumo)
        {
            return _context.tblCom_SurtidoDet.Where(x => x.estatus && x.insumo == insumo && (x.estadoSurtido == "R" || x.estadoSurtido == "T" || x.tipoSurtidoDetalle == "AP")).ToList();
        }
        public int getUltimaRequisicionNumero(string cc)
        {
            // var ultimaRequisicion = (int)consultaCheckProductivo(string.Format(@"SELECT TOP 1 numero FROM so_requisicion WHERE cc = '{0}' ORDER BY numero DESC", cc)).ToObject<int>();

            // return ultimaRequisicion;
            //linq
            var ultimaRequisicion = _context.tblCom_Req.Where(x => x.cc == cc).OrderByDescending(x => x.numero).FirstOrDefault();
            if (ultimaRequisicion != null)
            {
                return ultimaRequisicion.numero;
            }
            else
            {
                return 1;
            }
        }
        public dynamic getMovSalidaAlmacen(int almacen_id, string cc, int folioSalida)
        {
            return (List<dynamic>)consultaListCheckProductivo(new List<string>()
                {
                    string.Format("SELECT" 
                                  + "m.partida, (i.insumo + '-' + i.descripcion) as insumo, (m.area + '-' + m.cuenta) as areaCuenta, m.costo_prom, m.importe"
                                  + "FROM si_movimientos_det m"
                                  + "INNER JOIN insumos i on i.insumo == m.insumo"
                                  + "WHERE m.almacen = '{0}' AND m.cc = '{1}' m.numero = '{2}' ",almacen_id, cc, folioSalida)

                }).ToList<dynamic>();
        }

        #region SALIDA POR CONSUMO
        public List<RequisicionDTO> getReqSalidasConsumo(string cc, int tipo)
        {
            var req = new List<RequisicionDTO>();

            try
            {
                var centrosCosto = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        cc AS Value, 
                                        descripcion AS Text 
                                    FROM cc 
                                    WHERE cc = '{0}' AND st_ppto != 'T' 
                                    ORDER BY Text ASC", cc)
                ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                 string.Format(@"SELECT 
                                    alm.almacen AS Value, 
                                    alm.descripcion AS Text 
                                FROM si_almacen alm                
                                ORDER BY Text ASC")
                ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                var requis = _context.tblCom_Req.Where(x =>
                    x.estatusRegistro &&
                    x.cc == cc &&
                    x.stAutoriza == true &&
                    x.stEstatus != "C"
                ).ToList();
                var requisicionesDetalle = _context.tblCom_ReqDet.ToList().Where(x => x.estatusRegistro && requis.Select(y => y.id).Contains(x.idReq)).ToList();

                switch (tipo)
                {
                    case 1: //Parciales
                        requis = requis.Where(x => x.stEstatus != "T" && x.stEstatus != "C").ToList();
                        break;
                    case 2: //Completas
                        requis = requis.Where(x => x.stEstatus == "T").ToList();
                        break;
                    default: //Pendientes
                        break;
                }

                var listaTipoGrupoEK = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM grupos_insumo")).ToObject<List<dynamic>>();

                foreach (var re in requis)
                {
                    #region Validación para quitar las requisiciones no inventariables.
                    var flagNoInventariable = false;
                    var requisicionDetalle = requisicionesDetalle.Where(x => x.idReq == re.id).ToList();

                    foreach (var reqDet in requisicionDetalle)
                    {
                        var tipoInsumo = Int32.Parse(reqDet.insumo.ToString().Substring(0, 1));
                        var grupoInsumo = Int32.Parse(reqDet.insumo.ToString().Substring(1, 2));

                        var tipoGrupoEK = listaTipoGrupoEK.FirstOrDefault(x => (int)x.tipo_insumo == tipoInsumo && (int)x.grupo_insumo == grupoInsumo);

                        if (tipoGrupoEK != null)
                        {
                            if ((string)tipoGrupoEK.inventariado.Value == "N")
                            {
                                flagNoInventariable = true;
                            }
                        }
                    }
                    #endregion

                    var listaCompras = _context.tblCom_OrdenCompraDet.Where(x => x.cc == re.cc && x.num_requisicion == re.numero).Select(x => x.numero).Distinct().ToList();
                    var numeroOCString = string.Join(", ", listaCompras);

                    if (!flagNoInventariable)
                    {
                        req.Add(new RequisicionDTO
                        {
                            ccDescripcion = centrosCosto.Where(y => y.Value == re.cc).Select(y => y.Value + "-" + y.Text).FirstOrDefault(),
                            numero = re.numero,
                            fecha = re.fecha,
                            almacenLAB = almacenes.Where(y => y.Value == re.idLibreAbordo.ToString()).Select(y => y.Value + "-" + y.Text).FirstOrDefault(),
                            cc = re.cc,
                            libre_abordo = re.idLibreAbordo,
                            numeroOCString = numeroOCString
                        });
                    }
                }
            }
            catch (Exception)
            {
                req = new List<RequisicionDTO>();
            }

            return req;
        }
        public List<salidaConsumoDTO> getReqDetSalidasConsumo(string cc, int req, int almacen)
        {
            List<salidaConsumoDTO> detalle = new List<salidaConsumoDTO>();

            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    #region EmpresaPeru
                    case EmpresaEnum.Peru:
                        {
                            using (var ctxPeru = new MainContext())
                            {

                                //Insumos Starsoft
                                var consultaInsumosStarsoft = @"SELECT ACODIGO,ADESCRI,AUNIDAD FROM [003BDCOMUN].[dbo].[MAEART]";
                                List<infoInsumosStarsoftDTO> listaInsumos = new List<infoInsumosStarsoftDTO>();
                                DynamicParameters lstParametrosInsumo = new DynamicParameters();
                                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                                {
                                    conexion.Open();
                                    listaInsumos = conexion.Query<infoInsumosStarsoftDTO>(consultaInsumosStarsoft, lstParametrosInsumo, null, true, 300, commandType: CommandType.Text).ToList();
                                    conexion.Close();
                                };


                                //                                var requisicionDetalle = (List<RequisicionDetDTO>)consultaCheckProductivo(
                                //                      string.Format(@"SELECT 
                                //                                        det.*, 
                                //                                        ins.insumo, 
                                //                                        ins.descripcion AS insumoDescripcion 
                                //                                    FROM so_requisicion_det det 
                                //                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                //                                    WHERE det.cc = '{0}' AND det.numero = {1}", cc, req)
                                //                  ).ToObject<List<RequisicionDetDTO>>();
                                var requisicionDetalle = (
                                from insumo in listaInsumos
                                join det in ctxPeru.tblCom_ReqDet.ToList()
                                    on insumo.ACODIGO equals "0" + det.insumo.ToString()
                                select new RequisicionDetDTO
                                {
                                    insumo = det.insumo,
                                    cantidad = det.cantidad
                                }
                            ).ToList();

                                //Almacen Starsoft
                                var ConsultAlmacenesStarsoft = @"SELECT STALMA AS Almacen,TADESCRI AS Descripción,CONVERT(numeric(11,2),STSKDIS) AS stock FROM [003BDCOMUN].[dbo].[STKART] s INNER JOIN [003BDCOMUN].[dbo].[TABALM]t ON s.STALMA=t.TAALMA";
                                List<infoAlmacenStarsoftDTO> listaAlmacen = new List<infoAlmacenStarsoftDTO>();
                                DynamicParameters lstParametrosAlmacen = new DynamicParameters();
                                using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                                {
                                    conexion.Open();
                                    listaAlmacen = conexion.Query<infoAlmacenStarsoftDTO>(ConsultAlmacenesStarsoft, lstParametrosAlmacen, null, true, 300, commandType: CommandType.Text).ToList();
                                    conexion.Close();
                                };



                                #region Almacén Propio
                                var listaAP = ctxPeru.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AP" && x.cc == cc && x.surtido.numero == req).ToList();
                                #endregion

                                #region Almacén Externo
                                var listaAE = ctxPeru.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AE" && x.cc == cc && x.surtido.numero == req).ToList();
                                var entradasTraspasosAE = ctxPeru.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.cc == cc && x.numeroReq == req).ToList();
                                var entradasTraspasosDetalleAE = (
                                    from mov in entradasTraspasosAE
                                    join det in ctxPeru.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 2).ToList()
                                        on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                    select new
                                    {
                                        insumo = det.insumo,
                                        cantidad = det.cantidad
                                    }
                                ).ToList();
                                #endregion

                                #region Orden Compra
                                //var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", cc, req));


                                var comprasDetallePeru = ctxPeru.tblCom_OrdenCompraDet.Where(x => x.cc == cc && x.num_requisicion == req).ToList();
                                List<tblCom_OrdenCompraDet> comprasDetalle = new List<tblCom_OrdenCompraDet>();

                                if (comprasDetallePeru != null)
                                {
                                    comprasDetalle = comprasDetallePeru;
                                }
                                #endregion

                                #region Salida por Consumo
                                var salidasConsumo = ctxPeru.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.cc == cc && x.numeroReq == req && x.tipo_mov == 51).ToList();
                                var salidasConsumoDetalle = (
                                    from mov in salidasConsumo
                                    join det in ctxPeru.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 51).ToList()
                                        on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                    select new
                                    {
                                        insumo = det.insumo,
                                        cantidad = det.cantidad
                                    }
                                ).ToList();
                                #endregion

                                foreach (var det in requisicionDetalle)
                                {
                                    var cantidadPartida = det.cantidad - det.cant_cancelada;
                                    var almacenDescripcion = listaAlmacen.Where(x => x.ACODIGO == almacen.ToString()).Select(x => x.TADESCRI).FirstOrDefault();

                                    var solicitadoAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                    var cantidadAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo
                                        //&& (x.estadoSurtido == "S" || x.estadoSurtido == "P")
                                    ).Select(x => x.cantidad).Sum();

                                    var solicitadoAE = listaAE.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                    var cantidadAE = entradasTraspasosDetalleAE.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();

                                    var solicitadoOC = cantidadPartida - solicitadoAP - solicitadoAE;
                                    var cantidadOC = default(decimal);

                                    if (comprasDetalle.Count() > 0)
                                    {
                                        foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == det.partida).ToList())
                                        {
                                            cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                                        }
                                    }

                                    var salidasConsumoInsumo = salidasConsumoDetalle.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                    var existencia = cantidadAP + cantidadAE + cantidadOC;
                                    var consumidoCompleto = salidasConsumoInsumo >= cantidadPartida;

                                    detalle.Add(new salidaConsumoDTO
                                    {
                                        partidaRequisicion = det.partida,
                                        insumo = det.insumo,
                                        insumoDescripcion = det.insumo + "-" + det.insumoDescripcion,
                                        almacen = almacen,
                                        almacenDescripcion = almacenDescripcion,
                                        solicitado = cantidadPartida,
                                        existencia = existencia,
                                        solicitadoAP = solicitadoAP,
                                        cantidadAP = cantidadAP,
                                        solicitadoAE = solicitadoAE,
                                        cantidadAE = cantidadAE,
                                        solicitadoOC = solicitadoOC,
                                        cantidadOC = cantidadOC,
                                        cc = det.cc,
                                        numeroReq = req,
                                        cantidadConsumida = salidasConsumoInsumo,
                                        consumidoCompleto = consumidoCompleto
                                    });
                                }

                            }
                        }
                        break;
                    #endregion

                    case EmpresaEnum.Colombia:
                        {
                            var requisicionDetalle = (List<RequisicionDetDTO>)consultaCheckProductivo(
                              string.Format(@"SELECT 
                                        det.*, 
                                        ins.insumo, 
                                        ins.descripcion AS insumoDescripcion 
                                    FROM so_requisicion_det det 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE det.cc = '{0}' AND det.numero = {1}", cc, req)
                          ).ToObject<List<RequisicionDetDTO>>();

                            var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm 
                                    WHERE alm.almacen = '{0}'               
                                    ORDER BY Text ASC", almacen)
                            ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                            #region Almacén Propio
                            var listaAP = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AP" && x.cc == cc && x.surtido.numero == req).ToList();
                            #endregion

                            #region Almacén Externo
                            var listaAE = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AE" && x.cc == cc && x.surtido.numero == req).ToList();
                            var entradasTraspasosAE = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.cc == cc && x.numeroReq == req).ToList();
                            var entradasTraspasosDetalleAE = (
                                from mov in entradasTraspasosAE
                                join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 2).ToList()
                                    on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                select new
                                {
                                    insumo = det.insumo,
                                    cantidad = det.cantidad
                                }
                            ).ToList();
                            #endregion

                            #region Orden Compra
                            var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", cc, req));
                            List<dynamic> comprasDetalle = new List<dynamic>();

                            if (comprasDetalleEK != null)
                            {
                                comprasDetalle = (List<dynamic>)comprasDetalleEK.ToObject<List<dynamic>>();
                            }
                            #endregion

                            #region Salida por Consumo
                            var salidasConsumo = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.cc == cc && x.numeroReq == req && x.tipo_mov == 51).ToList();
                            var salidasConsumoDetalle = (
                                from mov in salidasConsumo
                                join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 51).ToList()
                                    on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                select new
                                {
                                    insumo = det.insumo,
                                    cantidad = det.cantidad
                                }
                            ).ToList();
                            #endregion

                            foreach (var det in requisicionDetalle)
                            {
                                var cantidadPartida = det.cantidad - det.cant_cancelada;
                                var almacenDescripcion = almacenes.Where(x => x.Value == almacen.ToString()).Select(x => x.Text).FirstOrDefault();

                                var solicitadoAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var cantidadAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo
                                    //&& (x.estadoSurtido == "S" || x.estadoSurtido == "P")
                                ).Select(x => x.cantidad).Sum();

                                var solicitadoAE = listaAE.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var cantidadAE = entradasTraspasosDetalleAE.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();

                                var solicitadoOC = cantidadPartida - solicitadoAP - solicitadoAE;
                                var cantidadOC = default(decimal);

                                if (comprasDetalle.Count() > 0)
                                {
                                    foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == det.partida).ToList())
                                    {
                                        cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                                    }
                                }

                                var salidasConsumoInsumo = salidasConsumoDetalle.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var existencia = cantidadAP + cantidadAE + cantidadOC;
                                var consumidoCompleto = salidasConsumoInsumo >= cantidadPartida;

                                detalle.Add(new salidaConsumoDTO
                                {
                                    partidaRequisicion = det.partida,
                                    insumo = det.insumo,
                                    insumoDescripcion = det.insumo + "-" + det.insumoDescripcion,
                                    almacen = almacen,
                                    almacenDescripcion = almacenDescripcion,
                                    solicitado = cantidadPartida,
                                    existencia = existencia,
                                    solicitadoAP = solicitadoAP,
                                    cantidadAP = cantidadAP,
                                    solicitadoAE = solicitadoAE,
                                    cantidadAE = cantidadAE,
                                    solicitadoOC = solicitadoOC,
                                    cantidadOC = cantidadOC,
                                    cc = det.cc,
                                    numeroReq = req,
                                    cantidadConsumida = salidasConsumoInsumo,
                                    consumidoCompleto = consumidoCompleto
                                });
                            }
                        }
                        break;
                    default:
                        {


                            var requisicionDetalle = (List<RequisicionDetDTO>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        det.*, 
                                        ins.insumo, 
                                        ins.descripcion AS insumoDescripcion 
                                    FROM so_requisicion_det det 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE det.cc = '{0}' AND det.numero = {1}", cc, req)
                            ).ToObject<List<RequisicionDetDTO>>();

                            var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm 
                                    WHERE alm.almacen = '{0}'               
                                    ORDER BY Text ASC", almacen)
                            ).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                            #region Almacén Propio
                            var listaAP = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AP" && x.cc == cc && x.surtido.numero == req).ToList();
                            #endregion

                            #region Almacén Externo
                            var listaAE = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AE" && x.cc == cc && x.surtido.numero == req).ToList();
                            var entradasTraspasosAE = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.cc == cc && x.numeroReq == req).ToList();
                            var entradasTraspasosDetalleAE = (
                                from mov in entradasTraspasosAE
                                join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 2).ToList()
                                    on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                select new
                                {
                                    insumo = det.insumo,
                                    cantidad = det.cantidad
                                }
                            ).ToList();
                            #endregion

                            #region Orden Compra
                            var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", cc, req));
                            List<dynamic> comprasDetalle = new List<dynamic>();

                            if (comprasDetalleEK != null)
                            {
                                comprasDetalle = (List<dynamic>)comprasDetalleEK.ToObject<List<dynamic>>();
                            }
                            #endregion

                            #region Salida por Consumo
                            var salidasConsumo = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.cc == cc && x.numeroReq == req && x.tipo_mov == 51).ToList();
                            var salidasConsumoDetalle = (
                                from mov in salidasConsumo
                                join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 51).ToList()
                                    on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                                select new
                                {
                                    insumo = det.insumo,
                                    cantidad = det.cantidad
                                }
                            ).ToList();
                            #endregion

                            foreach (var det in requisicionDetalle)
                            {
                                var cantidadPartida = det.cantidad - det.cant_cancelada;
                                var almacenDescripcion = almacenes.Where(x => x.Value == almacen.ToString()).Select(x => x.Text).FirstOrDefault();

                                var solicitadoAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var cantidadAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo
                                    //&& (x.estadoSurtido == "S" || x.estadoSurtido == "P")
                                ).Select(x => x.cantidad).Sum();

                                var solicitadoAE = listaAE.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var cantidadAE = entradasTraspasosDetalleAE.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();

                                var solicitadoOC = cantidadPartida - solicitadoAP - solicitadoAE;
                                var cantidadOC = default(decimal);

                                if (comprasDetalle.Count() > 0)
                                {
                                    foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == det.partida).ToList())
                                    {
                                        cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                                    }
                                }

                                var salidasConsumoInsumo = salidasConsumoDetalle.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                                var existencia = cantidadAP + cantidadAE + cantidadOC;
                                var consumidoCompleto = salidasConsumoInsumo >= cantidadPartida;

                                detalle.Add(new salidaConsumoDTO
                                {
                                    partidaRequisicion = det.partida,
                                    insumo = det.insumo,
                                    insumoDescripcion = det.insumo + "-" + det.insumoDescripcion,
                                    almacen = almacen,
                                    almacenDescripcion = almacenDescripcion,
                                    solicitado = cantidadPartida,
                                    existencia = existencia,
                                    solicitadoAP = solicitadoAP,
                                    cantidadAP = cantidadAP,
                                    solicitadoAE = solicitadoAE,
                                    cantidadAE = cantidadAE,
                                    solicitadoOC = solicitadoOC,
                                    cantidadOC = cantidadOC,
                                    cc = det.cc,
                                    numeroReq = req,
                                    cantidadConsumida = salidasConsumoInsumo,
                                    consumidoCompleto = consumidoCompleto
                                });
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                detalle = new List<salidaConsumoDTO>();
            }

            return detalle;
        }
        public Dictionary<string, object> getUbicacionDetalleSurtido(string cc, int numero_requisicion, int almacenID, int partida, int insumo)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var ubicacionDetalle = new List<UbicacionDetalleDTO>();

                #region Existencias
                var entradasEK = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        det.insumo, 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE mov.almacen = {0} AND det.insumo = {1} AND det.tipo_mov < 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", almacenID, insumo));

                var salidasEK = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        det.insumo, 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE mov.almacen = {0} AND det.insumo = {1} AND det.tipo_mov > 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", almacenID, insumo));

                if (entradasEK != null)
                {
                    var entradas = (List<UbicacionDetalleDTO>)entradasEK.ToObject<List<UbicacionDetalleDTO>>();

                    if (salidasEK != null)
                    {
                        var salidas = (List<UbicacionDetalleDTO>)salidasEK.ToObject<List<UbicacionDetalleDTO>>();

                        foreach (var ent in entradas)
                        {
                            var salida = salidas.FirstOrDefault(x =>
                                    x.insumoDesc == ent.insumoDesc &&
                                    x.area_alm == ent.area_alm &&
                                    x.lado_alm == ent.lado_alm &&
                                    x.estante_alm == ent.estante_alm &&
                                    x.nivel_alm == ent.nivel_alm
                                );

                            if (salida != null)
                            {
                                ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                            }
                        }

                        ubicacionDetalle.AddRange(entradas);
                    }
                    else
                    {
                        ubicacionDetalle.AddRange(entradas);
                    }
                }
                #endregion

                #region Cruce con la información de lo surtido y reservado a la requisición
                #region Almacén Propio
                var listaAP = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AP" && x.cc == cc && x.surtido.numero == numero_requisicion).ToList();
                #endregion

                #region Almacén Externo
                var listaAE = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AE" && x.cc == cc && x.surtido.numero == numero_requisicion).ToList();
                var entradasTraspasosAE = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.cc == cc && x.numeroReq == numero_requisicion).ToList();
                var entradasTraspasosDetalleAE = (
                    from mov in entradasTraspasosAE
                    join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 2).ToList()
                        on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                    select new
                    {
                        insumo = det.insumo,
                        cantidad = det.cantidad,
                        area_alm = det.area_alm,
                        lado_alm = det.lado_alm,
                        estante_alm = det.estante_alm,
                        nivel_alm = det.nivel_alm
                    }
                ).ToList();
                #endregion

                #region Orden Compra
                var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", cc, numero_requisicion));
                List<dynamic> comprasDetalle = new List<dynamic>();
                List<dynamic> entradasCompra = new List<dynamic>();

                if (comprasDetalleEK != null)
                {
                    comprasDetalle = (List<dynamic>)comprasDetalleEK.ToObject<List<dynamic>>();
                }

                if (comprasDetalle.Count() > 0)
                {
                    var listaNumerosOC = comprasDetalle.Select(x => x.numero).Distinct().ToList();
                    var entradasCompraEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
	                                    mov.*, 
	                                    det.* 
                                    FROM si_movimientos mov 
	                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                    WHERE mov.cc = '{0}' AND mov.tipo_mov = 1 AND mov.orden_ct IN ({1})", cc, string.Join(", ", listaNumerosOC)));

                    if (entradasCompraEK != null)
                    {
                        entradasCompra = (List<dynamic>)entradasCompraEK.ToObject<List<dynamic>>();
                    }
                }
                #endregion

                foreach (var exis in ubicacionDetalle)
                {
                    var cantidadAP = listaAP.Where(x =>
                        x.partidaRequisicion == partida &&
                        x.insumo == exis.insumo &&
                        x.area_alm == exis.area_alm &&
                        x.lado_alm == exis.lado_alm &&
                        x.estante_alm == exis.estante_alm &&
                        x.nivel_alm == exis.nivel_alm
                    ).Select(x => x.cantidad).Sum();
                    var cantidadAE = entradasTraspasosDetalleAE.Where(x =>
                        x.insumo == exis.insumo &&
                        x.area_alm == exis.area_alm &&
                        x.lado_alm == exis.lado_alm &&
                        x.estante_alm == exis.estante_alm &&
                        x.nivel_alm == exis.nivel_alm
                    ).Select(x => x.cantidad).Sum();
                    var cantidadOC = default(decimal);

                    if (entradasCompra.Count() > 0)
                    {
                        var entradasInsumo = entradasCompra.Where(x =>
                            (int)x.insumo == exis.insumo &&
                            (string)x.area_alm == exis.area_alm &&
                            (string)x.lado_alm == exis.lado_alm &&
                            (string)x.estante_alm == exis.estante_alm &&
                            (string)x.nivel_alm == exis.nivel_alm
                        ).ToList();

                        if (entradasInsumo.Count() > 0)
                        {
                            cantidadOC = entradasInsumo.Sum(x => (decimal)x.cantidad);
                        }
                    }

                    //if (comprasDetalle.Count() > 0)
                    //{
                    //    foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == partida).ToList())
                    //    {
                    //        cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                    //    }
                    //}

                    var existenciaSurtido = cantidadAP + cantidadAE + cantidadOC;

                    exis.cantidad = existenciaSurtido > exis.cantidad ? exis.cantidad : existenciaSurtido;
                }
                #endregion

                result.Add("data", ubicacionDetalle);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }
        public decimal getExistenciaValidacionObsoleto(int insumo)
        {
            if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
            {
                #region COLOMBIA
                decimal existencia = 0;
                var resultadoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            SUM(det.cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia, 
                                            AVG(det.precio) AS precio_promedio 
                                                FROM DBA.si_movimientos mov 
                                                INNER JOIN DBA.si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                                    WHERE mov.almacen = 3 AND det.insumo = {0}", insumo));
                if (resultadoEK != null)
                {
                    var resultado = (List<dynamic>)resultadoEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);

                    existencia = resultado.Select(x => new { Existencia = x.Existencia == null ? 0 : (decimal)x.Existencia - reservados.Sum(y => y.cantidad) }).FirstOrDefault().Existencia;

                    var precioPromedio = resultado[0].precio_promedio != null ? Convert.ToDecimal(resultado[0].precio_promedio, CultureInfo.InvariantCulture) : 0;
                    if (existencia > 0 && precioPromedio < 600)
                        existencia = 0; //Se regresa a cero la existencia para que entre correctamente la validación: cuando el precio promedio es menor a 600 no se toma en cuenta el bloqueo y se permite hacer la requisición independientemente de la existencia.
                }
                return existencia;
                #endregion
            }
            else
            {
                #region DEMAS EMPRESAS
                decimal existencia = 0;
                var resultadoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        SUM(det.cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia, 
                                        AVG(det.precio) AS precio_promedio 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                    WHERE 
                                        mov.almacen = 3 AND 
                                        det.insumo = {0}", insumo));

                if (resultadoEK != null)
                {
                    var resultado = (List<dynamic>)resultadoEK.ToObject<List<dynamic>>();
                    var reservados = getReservados(insumo);

                    existencia = resultado.Select(x => new { Existencia = x.Existencia == null ? 0 : (decimal)x.Existencia - reservados.Sum(y => y.cantidad) }).FirstOrDefault().Existencia;

                    var precioPromedio = resultado[0].precio_promedio != null ? Convert.ToDecimal(resultado[0].precio_promedio, CultureInfo.InvariantCulture) : 0;

                    if (existencia > 0 && precioPromedio < 600)
                    {
                        existencia = 0; //Se regresa a cero la existencia para que entre correctamente la validación: cuando el precio promedio es menor a 600 no se toma en cuenta el bloqueo y se permite hacer la requisición independientemente de la existencia.
                    }
                }

                return existencia;
                #endregion
            }
        }
        public List<salidasAlmacenDTO> guardarSalidasConsumo(List<SurtidoDetDTO> salidas, bool salidaNormal)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            List<salidasAlmacenDTO> movSalidas = new List<salidasAlmacenDTO>();

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            if (vSesiones.sesionEmpresaActual == 2)
            {
                if (salidas.Any(x => x.area == 0 || x.cuenta == 0))
                {
                    throw new Exception("Debe capturar Área-Cuenta");
                }
            }

            #region Validación familias 101 y 102
            if (vSesiones.sesionEmpresaActual == 2)
            {
                if (salidas.Any(x => x.almacenOrigenID < 600 || x.almacenOrigenID > 620))
                {
                    var listaAlmacenesExcepciones = _context.tblAlm_Validacion_101_102_AlmacenesExcepciones.Where(x => x.estatus).Select(x => x.almacen).ToList();
                    var flagAlmacenExcepcion = false;

                    if (listaAlmacenesExcepciones.Count() > 0)
                    {
                        foreach (var sal in salidas)
                        {
                            if (listaAlmacenesExcepciones.Contains(sal.almacenOrigenID))
                            {
                                flagAlmacenExcepcion = true;
                            }
                        }
                    }

                    if (!flagAlmacenExcepcion)
                    {
                        if (salidas.Select(x => x.insumo.ToString().Substring(0, 3)).Any(x => x == "101") || salidas.Select(x => x.insumo.ToString().Substring(0, 3)).Any(x => x == "102"))
                        {
                            List<int> insumosExcepciones = _context.tblAlm_Validacion_101_102_InsumosExcepciones.Where(x => x.estatus).Select(x => x.insumo).ToList();
                            List<int> insumosMovimiento = salidas.Where(x =>
                                x.insumo.ToString().Substring(0, 3) == "101" || x.insumo.ToString().Substring(0, 3) == "102"
                            ).Select(x => x.insumo).ToList();

                            if (insumosExcepciones.Count() > 0)
                            {
                                foreach (int insMov in insumosMovimiento)
                                {
                                    if (!insumosExcepciones.Contains(insMov))
                                    {
                                        throw new Exception("No se puede dar salida a las familias de insumo 101 y 102. Insumo: \"" + insMov + "\".");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("No se puede dar salida a las familias de insumo 101 y 102.");
                            }
                        }
                    }
                }
            }
            #endregion

            #region Validación Área-Cuenta 14-1 y 14-2 para Gerardo Hernández y Kasia Miranda
            if (salidas.Any(x => x.area == 14 && x.cuenta == 1) || salidas.Any(x => x.area == 14 && x.cuenta == 2))
            {
                if (usuarioSigoplan.sn_empleado != 30)
                {
                    if (salidas.Any(x => x.area == 14 && x.cuenta == 2))
                    {
                        if (usuarioSigoplan.sn_empleado != 18800)
                        {
                            throw new Exception("No se guardó la información. Su usuario no puede dar salida por consumo para las áreas cuenta 14-1 y 14-2.");
                        }
                    }
                    else
                    {
                        throw new Exception("No se guardó la información. Su usuario no puede dar salida por consumo para las áreas cuenta 14-1 y 14-2.");
                    }
                }
            }
            #endregion

            foreach (var sal in salidas)
            {
                #region Validación Permisos Familias
                if (vSesiones.sesionEmpresaActual == 2)
                {
                    var permisosFamilias = getPermisosFamilia(sal.cc);

                    if (permisosFamilias.Count > 0)
                    {
                        if (!permisosFamilias.Any(x => x.familia == "*"))
                        {
                            var familias = sal.listUbicacionMovimiento.Select(x => x.insumo.ToString().Substring(0, 3)).ToList();

                            foreach (var fam in familias)
                            {
                                if (!permisosFamilias.Select(x => x.familia).Contains(fam))
                                {
                                    throw new Exception(
                                        string.Format(@"Bloqueo de familia de insumo '{0}' para el centro de costo '{1}'. No se puede proceder con la salida.", fam, sal.cc)
                                    );
                                }
                            }
                        }
                    }
                }
                #endregion

                var ultimoMovimiento = (List<SurtidoDetDTO>)consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 numero 
                                        FROM si_movimientos 
                                        WHERE almacen = {0} AND tipo_mov = {1} 
                                        ORDER BY numero DESC", salidaNormal ? sal.almacenOrigenID : sal.almacenDestinoID, 51)
                    ).ToObject<List<SurtidoDetDTO>>();

                var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == sal.cc && x.numero == sal.numeroReq);

                List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

                if (requisicion != null)
                {
                    requisicionDet = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicion.id).ToList();
                }

                sal.precio = 1;
                sal.importe = sal.cantidad * sal.precio;

                var nuevaSalida = new tblAlm_Movimientos
                {
                    almacen = salidaNormal ? sal.almacenOrigenID : sal.almacenDestinoID,
                    tipo_mov = 51,
                    numero = ultimoMovimiento[0].numero + 1,
                    cc = salidaNormal ? sal.cc.ToUpper() : sal.ccDestino.ToUpper(),
                    compania = 1,
                    periodo = DateTime.Now.Month,
                    ano = DateTime.Now.Year,
                    orden_ct = sal.ordenTraspaso,
                    frente = 0,
                    fecha = DateTime.Now.Date,
                    proveedor = 0,
                    total = sal.importe,
                    estatus = "A",
                    transferida = "N",
                    alm_destino = salidaNormal ? sal.almacenOrigenID : sal.almacenDestinoID,
                    cc_destino = salidaNormal ? sal.cc.ToUpper() : sal.ccDestino.ToUpper(),
                    comentarios = sal.comentarios,
                    tipo_trasp = "0",
                    tipo_cambio = 1,
                    estatusHabilitado = true,
                    numeroReq = sal.numeroReq,
                    ccReq = sal.cc
                };

                _context.tblAlm_Movimientos.Add(nuevaSalida);
                _context.SaveChanges();

                var insumoReq = requisicionDet.FirstOrDefault(x => x.insumo == sal.insumo);

                List<tblAlm_MovimientosDet> listSalidaDet = new List<tblAlm_MovimientosDet>();

                var partidaContador = 1;

                foreach (var ubi in sal.listUbicacionMovimiento)
                {
                    var partidaMovimiento = partidaContador++;

                    var nuevaSalidaDet = new tblAlm_MovimientosDet
                    {
                        almacen = salidaNormal ? sal.almacenOrigenID : sal.almacenDestinoID,
                        tipo_mov = 51,
                        numero = ultimoMovimiento[0].numero + 1,
                        partida = partidaMovimiento,
                        insumo = sal.insumo,
                        comentarios = sal.comentarios,
                        area = sal.area, //area = insumoReq != null ? insumoReq.area : 0,
                        cuenta = sal.cuenta, //cuenta = insumoReq != null ? insumoReq.cuenta : 0,
                        cantidad = ubi.cantidadMovimiento,
                        precio = sal.precio,
                        importe = sal.importe,
                        id_resguardo = 0,
                        area_alm = ubi.area_alm ?? "",
                        lado_alm = ubi.lado_alm ?? "",
                        estante_alm = ubi.estante_alm ?? "",
                        nivel_alm = ubi.nivel_alm ?? "",
                        transporte = sal.transporte ?? "",
                        estatusHabilitado = true
                    };

                    _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                    _context.SaveChanges();
                    listSalidaDet.Add(nuevaSalidaDet);
                }

                if (nuevaSalida.total <= 0)
                {
                    throw new Exception("El total no puede ser igual o menor a cero.");
                }

                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        var commandMovimientos = new OdbcCommand(consultaMovimientos);

                        OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                        parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaSalida.almacen;
                        parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaSalida.tipo_mov;
                        parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaSalida.numero;
                        parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaSalida.cc;
                        parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaSalida.compania;
                        parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaSalida.periodo;
                        parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaSalida.ano;
                        parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                        parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaSalida.frente;
                        parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaSalida.fecha.Date;
                        parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaSalida.proveedor;
                        parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaSalida.total;
                        parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaSalida.estatus;
                        parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaSalida.transferida;
                        parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                        parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                        parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaSalida.alm_destino;
                        parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaSalida.cc_destino;
                        parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaSalida.comentarios != null ? nuevaSalida.comentarios : "";
                        parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaSalida.tipo_trasp;
                        parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = sal.numeroDestino;
                        parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                        parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                        parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                        parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaSalida.tipo_cambio;
                        parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                        parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                        parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                        parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                        parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                        parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                        parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@folio_traspaso", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                        parametersMovimientos.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                        commandMovimientos.Connection = trans.Connection;
                        commandMovimientos.Transaction = trans;

                        var success = commandMovimientos.ExecuteNonQuery();
                        var successDet = 0;

                        foreach (var salDet in listSalidaDet)
                        {
                            decimal costoPromedio = getCostoPromedioKardex(nuevaSalida.almacen, salDet.insumo);
                            var importe = salDet.cantidad * costoPromedio;

                            if (costoPromedio <= 0 || importe <= 0)
                            {
                                throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                            }

                            var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                            var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                            OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                            parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = salDet.almacen;
                            parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = salDet.tipo_mov;
                            parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = ultimoMovimiento[0].numero + 1;
                            parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = salDet.partida;
                            parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = salDet.insumo;
                            parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = salDet.comentarios != null ? salDet.comentarios : "";
                            parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = salDet.area;
                            parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = salDet.cuenta;
                            parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = salDet.cantidad;
                            parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = costoPromedio;
                            parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = importe;
                            parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = costoPromedio;
                            parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@area_alm", OdbcType.Char).Value = salDet.area_alm;
                            parametersMovimientosDetalles.Add("@lado_alm", OdbcType.Char).Value = salDet.lado_alm;
                            parametersMovimientosDetalles.Add("@estante_alm", OdbcType.Char).Value = salDet.estante_alm;
                            parametersMovimientosDetalles.Add("@nivel_alm", OdbcType.Char).Value = salDet.nivel_alm;
                            parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                            commandMovimientosDetalles.Connection = trans.Connection;
                            commandMovimientosDetalles.Transaction = trans;

                            successDet += commandMovimientosDetalles.ExecuteNonQuery();

                            #region Actualizar Tablas Acumula
                            var objAcumula = new MovimientoDetalleEnkontrolDTO
                            {
                                insumo = salDet.insumo,
                                cantidad = salDet.cantidad,
                                precio = costoPromedio,
                                tipo_mov = salDet.tipo_mov,
                                costo_prom = costoPromedio
                            };

                            actualizarAcumula(nuevaSalida.almacen, nuevaSalida.cc, objAcumula, null, trans);
                            #endregion
                        }

                        if (success > 0 && successDet > 0)
                        {
                            //Si la cantidad de salida de consumo es igual a solicitado, se cambia estatos Req a Completado
                            if (sal.solicitado == sal.cantidad)
                            {
                                requisicion.stEstatus = "T";

                                _context.Entry(requisicion).State = System.Data.Entity.EntityState.Modified;
                                _context.SaveChanges();
                            }

                            movSalidas.Add(new salidasAlmacenDTO
                            {
                                almacen = sal.almacenOrigenDesc,
                                centroCosto = sal.ccDesc,
                                fechaSalida = nuevaSalida.fecha,
                                folioSalida = (ultimoMovimiento[0].numero + 1).ToString(),
                                partida = insumoReq != null ? insumoReq.partida : 0,
                                insumo = sal.insumo + "-" + sal.insumoDesc ?? "",
                                areaCuenta = insumoReq != null ? insumoReq.area.ToString() + "-" + insumoReq.cuenta.ToString() : "00-00",
                                cantidad = sal.cantidad,
                                costoPromedio = 0,
                                importe = sal.importe,
                            });
                            _context.SaveChanges();
                            trans.Commit();
                        }
                    }
                }
            }

            return movSalidas;
        }
        public List<entradasAlmacenDTO> GuardarEntradasConsumo(List<SurtidoDetDTO> entradas)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            List<entradasAlmacenDTO> movEntradas = new List<entradasAlmacenDTO>();

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            foreach (var ent in entradas)
            {
                var ultimoMovimiento = (List<SurtidoDetDTO>)consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 numero, folio_traspaso 
                                        FROM si_movimientos 
                                        WHERE almacen = {0} AND tipo_mov = {1} 
                                        ORDER BY numero DESC", ent.almacenDestinoID, 2)
                    ).ToObject<List<SurtidoDetDTO>>();

                var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == ent.cc && x.numero == ent.numeroReq);

                List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

                if (requisicion != null)
                {
                    requisicionDet = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicion.id).ToList();
                }

                ent.precio = 1;
                ent.importe = ent.cantidad * ent.precio;

                var nuevaEntrada = new tblAlm_Movimientos
                {
                    almacen = ent.almacenDestinoID,
                    tipo_mov = 2,
                    numero = ultimoMovimiento[0].numero + 1,
                    cc = ent.ccDestino.ToUpper(),
                    compania = 1,
                    periodo = DateTime.Now.Month,
                    ano = DateTime.Now.Year,
                    orden_ct = ent.ordenTraspaso,
                    frente = 0,
                    fecha = DateTime.Now.Date,
                    proveedor = 0,
                    total = ent.importe,
                    estatus = "A",
                    transferida = "N",
                    alm_destino = requisicion.idLibreAbordo,
                    cc_destino = ent.ccDestino.ToUpper(),
                    comentarios = ent.comentarios,
                    tipo_trasp = "0",
                    tipo_cambio = 1,
                    estatusHabilitado = true
                };

                _context.tblAlm_Movimientos.Add(nuevaEntrada);
                _context.SaveChanges();

                var insumoReq = requisicionDet.FirstOrDefault(x => x.insumo == ent.insumo);

                List<tblAlm_MovimientosDet> listEntradaDet = new List<tblAlm_MovimientosDet>();

                var partidaContador = 1;

                foreach (var ubi in ent.listUbicacionMovimiento)
                {
                    var partidaMovimiento = partidaContador++;

                    var nuevaEntradaDet = new tblAlm_MovimientosDet
                    {
                        almacen = nuevaEntrada.almacen,
                        tipo_mov = 2,
                        numero = ultimoMovimiento[0].numero + 1,
                        partida = partidaMovimiento,
                        insumo = ent.insumo,
                        comentarios = ent.comentarios,
                        area = insumoReq != null ? insumoReq.area : 0,
                        cuenta = insumoReq != null ? insumoReq.cuenta : 0,
                        //cantidad = ent.cantidad,
                        cantidad = ubi.cantidadMovimiento,
                        precio = ent.precio,
                        importe = ent.importe,
                        id_resguardo = 0,
                        area_alm = ubi.area_alm ?? "",
                        lado_alm = ubi.lado_alm ?? "",
                        estante_alm = ubi.estante_alm ?? "",
                        nivel_alm = ubi.nivel_alm ?? "",
                        transporte = ent.transporte ?? "",
                        estatusHabilitado = true
                    };

                    _context.tblAlm_MovimientosDet.Add(nuevaEntradaDet);
                    _context.SaveChanges();
                    listEntradaDet.Add(nuevaEntradaDet);
                }

                if (nuevaEntrada.total <= 0)
                {
                    throw new Exception("El total no puede ser igual o menor a cero.");
                }

                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        var consulta = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        using (var cmd = new OdbcCommand(consulta))
                        {
                            OdbcParameterCollection parameters = cmd.Parameters;

                            parameters.Add("@almacen", OdbcType.Numeric).Value = nuevaEntrada.almacen;
                            parameters.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaEntrada.tipo_mov;
                            parameters.Add("@numero", OdbcType.Numeric).Value = nuevaEntrada.numero;
                            parameters.Add("@cc", OdbcType.Char).Value = nuevaEntrada.cc;
                            parameters.Add("@compania", OdbcType.Numeric).Value = nuevaEntrada.compania;
                            parameters.Add("@periodo", OdbcType.Numeric).Value = nuevaEntrada.periodo;
                            parameters.Add("@ano", OdbcType.Numeric).Value = nuevaEntrada.ano;
                            parameters.Add("@orden_ct", OdbcType.Numeric).Value = nuevaEntrada.orden_ct;
                            parameters.Add("@frente", OdbcType.Numeric).Value = nuevaEntrada.frente;
                            parameters.Add("@fecha", OdbcType.Date).Value = nuevaEntrada.fecha.Date;
                            parameters.Add("@proveedor", OdbcType.Numeric).Value = nuevaEntrada.proveedor;
                            parameters.Add("@total", OdbcType.Numeric).Value = nuevaEntrada.total;
                            parameters.Add("@estatus", OdbcType.Char).Value = nuevaEntrada.estatus;
                            parameters.Add("@transferida", OdbcType.Char).Value = nuevaEntrada.transferida;
                            parameters.Add("@poliza", OdbcType.Numeric).Value = 0;
                            parameters.Add("@empleado", OdbcType.Numeric).Value = empleado;
                            parameters.Add("@alm_destino", OdbcType.Numeric).Value = nuevaEntrada.alm_destino;
                            parameters.Add("@cc_destino", OdbcType.Char).Value = nuevaEntrada.cc_destino;
                            parameters.Add("@comentarios", OdbcType.Char).Value = nuevaEntrada.comentarios != null ? nuevaEntrada.comentarios : "";
                            parameters.Add("@tipo_trasp", OdbcType.Char).Value = nuevaEntrada.tipo_trasp;
                            parameters.Add("@numero_destino", OdbcType.Numeric).Value = ent.numeroDestino;
                            parameters.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                            parameters.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                            parameters.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                            parameters.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaEntrada.tipo_cambio;
                            parameters.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                            parameters.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                            parameters.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                            parameters.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                            parameters.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                            parameters.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                            parameters.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                            parameters.Add("@folio_traspaso", OdbcType.Numeric).Value =
                                ultimoMovimiento.Count > 0 && ultimoMovimiento[0].folio_traspaso != null ? ultimoMovimiento[0].folio_traspaso + 1 : 0;
                            parameters.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                            cmd.Connection = trans.Connection;
                            cmd.Transaction = trans;

                            var count = cmd.ExecuteNonQuery();

                            if (count > 0)
                            {
                                trans.Commit();
                                movEntradas.Add(new entradasAlmacenDTO
                                {
                                    centroCosto = ent.ccDesc,
                                    folioEntrada = nuevaEntrada.numero.ToString(),
                                    almacen = ent.almacenDestinoDesc,
                                    fechaEntrada = nuevaEntrada.fecha,
                                    partida = insumoReq != null ? insumoReq.partida : 0,
                                    insumo = ent.insumo + "-" + ent.insumoDesc ?? "",
                                    areaCuenta = insumoReq != null ? insumoReq.area.ToString() + "-" + insumoReq.cuenta.ToString() : "00-00",
                                    referencia = "",
                                    remision = "",
                                    cantidad = ent.cantidad,
                                    precio = ent.precio,
                                    importe = ent.importe,
                                    comentarios = nuevaEntrada.comentarios,
                                    ordenCompra = nuevaEntrada.orden_ct.ToString(),
                                    proveedor = "",
                                    direccion = "",
                                    ciudad = "",
                                    telefonos = "",
                                });
                            }
                        }

                        foreach (var entDet in listEntradaDet)
                        {
                            decimal costoPromedio = getCostoPromedioNuevo(nuevaEntrada.almacen, entDet.insumo);
                            var importe = entDet.cantidad * costoPromedio;

                            if (costoPromedio <= 0 || importe <= 0)
                            {
                                throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                            }

                            var consultaDet = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                            using (var cmd = new OdbcCommand(consultaDet))
                            {
                                OdbcParameterCollection parameters = cmd.Parameters;

                                parameters.Add("@almacen", OdbcType.Numeric).Value = entDet.almacen;
                                parameters.Add("@tipo_mov", OdbcType.Numeric).Value = entDet.tipo_mov;
                                parameters.Add("@numero", OdbcType.Numeric).Value = nuevaEntrada.numero;
                                parameters.Add("@partida", OdbcType.Numeric).Value = entDet.partida;
                                parameters.Add("@insumo", OdbcType.Numeric).Value = entDet.insumo;
                                parameters.Add("@comentarios", OdbcType.Char).Value = entDet.comentarios != null ? entDet.comentarios : "";
                                parameters.Add("@area", OdbcType.Numeric).Value = entDet.area;
                                parameters.Add("@cuenta", OdbcType.Numeric).Value = entDet.cuenta;
                                parameters.Add("@cantidad", OdbcType.Numeric).Value = entDet.cantidad;
                                parameters.Add("@precio", OdbcType.Numeric).Value = costoPromedio;
                                parameters.Add("@importe", OdbcType.Numeric).Value = importe;
                                parameters.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@costo_prom", OdbcType.Numeric).Value = costoPromedio;
                                parameters.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                                parameters.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                                parameters.Add("@area_alm", OdbcType.Char).Value = entDet.area_alm;
                                parameters.Add("@lado_alm", OdbcType.Char).Value = entDet.lado_alm;
                                parameters.Add("@estante_alm", OdbcType.Char).Value = entDet.estante_alm;
                                parameters.Add("@nivel_alm", OdbcType.Char).Value = entDet.nivel_alm;
                                parameters.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                                cmd.Connection = trans.Connection;
                                cmd.Transaction = trans;

                                var count = cmd.ExecuteNonQuery();

                                #region Actualizar Tablas Acumula
                                var objAcumula = new MovimientoDetalleEnkontrolDTO
                                {
                                    insumo = entDet.insumo,
                                    cantidad = entDet.cantidad,
                                    precio = costoPromedio,
                                    tipo_mov = entDet.tipo_mov,
                                    costo_prom = costoPromedio
                                };

                                actualizarAcumula(nuevaEntrada.almacen, nuevaEntrada.cc, objAcumula, null, trans);
                                #endregion

                                if (count > 0)
                                {
                                    trans.Commit();
                                }
                            }
                        }
                    }
                }
            }

            return movEntradas;
        }
        public List<salidasAlmacenDTO> GuardarSalidasC(List<SurtidoDetDTO> salidas)
        {
            var empleado = 0;
            var usuarioSigoplan = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            List<salidasAlmacenDTO> movSalidas = new List<salidasAlmacenDTO>();

            if (usuarioSigoplan != null)
            {
                empleado = usuarioSigoplan.empleado;
            }

            #region Validación familias 101 y 102
            if (vSesiones.sesionEmpresaActual == 2)
            {
                if (salidas.Any(x => x.almacenOrigenID < 600 || x.almacenOrigenID > 620))
                {
                    var listaAlmacenesExcepciones = _context.tblAlm_Validacion_101_102_AlmacenesExcepciones.Where(x => x.estatus).Select(x => x.almacen).ToList();
                    var flagAlmacenExcepcion = false;

                    if (listaAlmacenesExcepciones.Count() > 0)
                    {
                        foreach (var sal in salidas)
                        {
                            if (listaAlmacenesExcepciones.Contains(sal.almacenOrigenID))
                            {
                                flagAlmacenExcepcion = true;
                            }
                        }
                    }

                    if (!flagAlmacenExcepcion)
                    {
                        if (salidas.Select(x => x.insumo.ToString().Substring(0, 3)).Any(x => x == "101") || salidas.Select(x => x.insumo.ToString().Substring(0, 3)).Any(x => x == "102"))
                        {
                            List<int> insumosExcepciones = _context.tblAlm_Validacion_101_102_InsumosExcepciones.Where(x => x.estatus).Select(x => x.insumo).ToList();
                            List<int> insumosMovimiento = salidas.Where(x =>
                                x.insumo.ToString().Substring(0, 3) == "101" || x.insumo.ToString().Substring(0, 3) == "102"
                            ).Select(x => x.insumo).ToList();

                            if (insumosExcepciones.Count() > 0)
                            {
                                foreach (int insMov in insumosMovimiento)
                                {
                                    if (!insumosExcepciones.Contains(insMov))
                                    {
                                        throw new Exception("No se puede dar salida a las familias de insumo 101 y 102. Insumo: \"" + insMov + "\".");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("No se puede dar salida a las familias de insumo 101 y 102.");
                            }
                        }
                    }
                }
            }
            #endregion

            #region Validación Área-Cuenta 14-1 y 14-2 para Gerardo Hernández y Kasia Miranda
            if (salidas.Any(x => x.area == 14 && x.cuenta == 1) || salidas.Any(x => x.area == 14 && x.cuenta == 2))
            {
                if (usuarioSigoplan.sn_empleado != 30)
                {
                    if (salidas.Any(x => x.area == 14 && x.cuenta == 2))
                    {
                        if (usuarioSigoplan.sn_empleado != 18800)
                        {
                            throw new Exception("No se guardó la información. Su usuario no puede dar salida por consumo para las áreas cuenta 14-1 y 14-2.");
                        }
                    }
                    else
                    {
                        throw new Exception("No se guardó la información. Su usuario no puede dar salida por consumo para las áreas cuenta 14-1 y 14-2.");
                    }
                }
            }
            #endregion

            foreach (var sal in salidas)
            {
                var ultimoMovimiento = (List<SurtidoDetDTO>)consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            TOP 1 numero, folio_traspaso 
                                        FROM si_movimientos 
                                        WHERE almacen = {0} AND tipo_mov = {1} 
                                        ORDER BY numero DESC", sal.almacenOrigenID, 52)
                    ).ToObject<List<SurtidoDetDTO>>();

                var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == sal.cc && x.numero == sal.numeroReq);

                List<tblCom_ReqDet> requisicionDet = new List<tblCom_ReqDet>();

                if (requisicion != null)
                {
                    requisicionDet = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicion.id).ToList();
                }

                sal.precio = 1;
                sal.importe = sal.cantidad * sal.precio;

                var nuevaSalida = new tblAlm_Movimientos
                {
                    almacen = sal.almacenOrigenID,
                    tipo_mov = 52,
                    numero = ultimoMovimiento[0].numero + 1,
                    cc = sal.cc.ToUpper(),
                    compania = 1,
                    periodo = DateTime.Now.Month,
                    ano = DateTime.Now.Year,
                    orden_ct = sal.ordenTraspaso,
                    frente = 0,
                    fecha = DateTime.Now.Date,
                    proveedor = 0,
                    total = sal.importe,
                    estatus = "A",
                    transferida = "N",
                    alm_destino = sal.almacenOrigenID,
                    cc_destino = sal.ccDestino.ToUpper(),
                    comentarios = sal.comentarios,
                    tipo_trasp = "0",
                    tipo_cambio = 1,
                    estatusHabilitado = true
                };

                _context.tblAlm_Movimientos.Add(nuevaSalida);
                _context.SaveChanges();

                var insumoReq = requisicionDet.FirstOrDefault(x => x.insumo == sal.insumo);

                List<tblAlm_MovimientosDet> listSalidaDet = new List<tblAlm_MovimientosDet>();

                var partidaContador = 1;

                foreach (var ubi in sal.listUbicacionMovimiento)
                {
                    var partidaMovimiento = partidaContador++;

                    var nuevaSalidaDet = new tblAlm_MovimientosDet
                    {
                        almacen = sal.almacenOrigenID,
                        tipo_mov = 52,
                        numero = ultimoMovimiento[0].numero + 1,
                        partida = partidaMovimiento,
                        insumo = sal.insumo,
                        comentarios = sal.comentarios,
                        area = insumoReq != null ? insumoReq.area : 0,
                        cuenta = insumoReq != null ? insumoReq.cuenta : 0,
                        //cantidad = sal.cantidad,
                        cantidad = ubi.cantidadMovimiento,
                        precio = sal.precio,
                        importe = sal.importe,
                        id_resguardo = 0,
                        area_alm = ubi.area_alm ?? "",
                        lado_alm = ubi.lado_alm ?? "",
                        estante_alm = ubi.estante_alm ?? "",
                        nivel_alm = ubi.nivel_alm ?? "",
                        transporte = sal.transporte ?? "",
                        estatusHabilitado = true
                    };

                    _context.tblAlm_MovimientosDet.Add(nuevaSalidaDet);
                    listSalidaDet.Add(nuevaSalidaDet);
                }

                if (nuevaSalida.total <= 0)
                {
                    throw new Exception("El total no puede ser igual o menor a cero.");
                }

                using (var con = checkConexionProductivo())
                {
                    using (var trans = con.BeginTransaction())
                    {
                        var consultaMovimientos = @"INSERT INTO si_movimientos 
                                         (almacen, tipo_mov, numero, cc, compania, periodo, ano, orden_ct, frente, fecha, proveedor, total, estatus, 
                                         transferida, poliza, empleado, alm_destino, cc_destino, comentarios, tipo_trasp, numero_destino, tp, year_poliza, 
                                         mes_poliza, tipo_cambio, hora, fecha_modifica, empleado_modifica, destajista, obra, id_residente, factura, sector_id, 
                                         tc_cc, paquete, tipo_cargo, cargo_destajista, cargo_id_residente, embarque, orden_prod, folio_traspaso, bit_sin_ubicacion) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                        var commandMovimientos = new OdbcCommand(consultaMovimientos);

                        OdbcParameterCollection parametersMovimientos = commandMovimientos.Parameters;

                        parametersMovimientos.Add("@almacen", OdbcType.Numeric).Value = nuevaSalida.almacen;
                        parametersMovimientos.Add("@tipo_mov", OdbcType.Numeric).Value = nuevaSalida.tipo_mov;
                        parametersMovimientos.Add("@numero", OdbcType.Numeric).Value = nuevaSalida.numero;
                        parametersMovimientos.Add("@cc", OdbcType.Char).Value = nuevaSalida.cc;
                        parametersMovimientos.Add("@compania", OdbcType.Numeric).Value = nuevaSalida.compania;
                        parametersMovimientos.Add("@periodo", OdbcType.Numeric).Value = nuevaSalida.periodo;
                        parametersMovimientos.Add("@ano", OdbcType.Numeric).Value = nuevaSalida.ano;
                        parametersMovimientos.Add("@orden_ct", OdbcType.Numeric).Value = nuevaSalida.orden_ct;
                        parametersMovimientos.Add("@frente", OdbcType.Numeric).Value = nuevaSalida.frente;
                        parametersMovimientos.Add("@fecha", OdbcType.Date).Value = nuevaSalida.fecha.Date;
                        parametersMovimientos.Add("@proveedor", OdbcType.Numeric).Value = nuevaSalida.proveedor;
                        parametersMovimientos.Add("@total", OdbcType.Numeric).Value = nuevaSalida.total;
                        parametersMovimientos.Add("@estatus", OdbcType.Char).Value = nuevaSalida.estatus;
                        parametersMovimientos.Add("@transferida", OdbcType.Char).Value = nuevaSalida.transferida;
                        parametersMovimientos.Add("@poliza", OdbcType.Numeric).Value = 0;
                        parametersMovimientos.Add("@empleado", OdbcType.Numeric).Value = empleado;
                        parametersMovimientos.Add("@alm_destino", OdbcType.Numeric).Value = nuevaSalida.alm_destino;
                        parametersMovimientos.Add("@cc_destino", OdbcType.Char).Value = nuevaSalida.cc_destino;
                        parametersMovimientos.Add("@comentarios", OdbcType.Char).Value = nuevaSalida.comentarios != null ? nuevaSalida.comentarios : "";
                        parametersMovimientos.Add("@tipo_trasp", OdbcType.Char).Value = nuevaSalida.tipo_trasp;
                        parametersMovimientos.Add("@numero_destino", OdbcType.Numeric).Value = sal.numeroDestino;
                        parametersMovimientos.Add("@tp", OdbcType.Char).Value = DBNull.Value;
                        parametersMovimientos.Add("@year_poliza", OdbcType.Numeric).Value = DateTime.Now.Year;
                        parametersMovimientos.Add("@mes_poliza", OdbcType.Numeric).Value = DateTime.Now.Month;
                        parametersMovimientos.Add("@tipo_cambio", OdbcType.Numeric).Value = nuevaSalida.tipo_cambio;
                        parametersMovimientos.Add("@hora", OdbcType.Time).Value = DateTime.Now.TimeOfDay;
                        parametersMovimientos.Add("@fecha_modifica", OdbcType.Date).Value = DateTime.Now.Date;
                        parametersMovimientos.Add("@empleado_modifica", OdbcType.Numeric).Value = empleado;
                        parametersMovimientos.Add("@destajista", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                        parametersMovimientos.Add("@id_residente", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@factura", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@tc_cc", OdbcType.Numeric).Value = 1;
                        parametersMovimientos.Add("@paquete", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@tipo_cargo", OdbcType.Numeric).Value = 0;
                        parametersMovimientos.Add("@cargo_Destajista", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@cargo_id_residente", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@embarque", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@orden_prod", OdbcType.Numeric).Value = DBNull.Value;
                        parametersMovimientos.Add("@folio_traspaso", OdbcType.Numeric).Value =
                            ultimoMovimiento.Count > 0 && ultimoMovimiento[0].folio_traspaso != null ? ultimoMovimiento[0].folio_traspaso + 1 : 0;
                        parametersMovimientos.Add("@bit_sin_ubicacion", OdbcType.Numeric).Value = 0;

                        commandMovimientos.Connection = trans.Connection;
                        commandMovimientos.Transaction = trans;

                        var success = commandMovimientos.ExecuteNonQuery();
                        var successDet = 0;

                        foreach (var salDet in listSalidaDet)
                        {
                            decimal costoPromedio = getCostoPromedioKardex(nuevaSalida.almacen, salDet.insumo);
                            var importe = salDet.cantidad * costoPromedio;

                            if (costoPromedio <= 0 || importe <= 0)
                            {
                                throw new Exception("El precio, el importe y el costo promedio no pueden ser igual o menor a cero.");
                            }

                            var consultaMovimientosDetalle = @"INSERT INTO si_movimientos_det 
                                         (almacen, tipo_mov, numero, partida, insumo, comentarios, area, cuenta, cantidad, precio, importe, 
                                         partida_oc, costo_prom, obra, manzana_ini, lote_ini, interior_ini, manzana_fin, lote_fin, interior_fin, 
                                         remision, sector_id, id_resguardo, area_alm, lado_alm, estante_alm, nivel_alm, fecha_fisico) 
                                         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                            var commandMovimientosDetalles = new OdbcCommand(consultaMovimientosDetalle);

                            OdbcParameterCollection parametersMovimientosDetalles = commandMovimientosDetalles.Parameters;

                            parametersMovimientosDetalles.Add("@almacen", OdbcType.Numeric).Value = salDet.almacen;
                            parametersMovimientosDetalles.Add("@tipo_mov", OdbcType.Numeric).Value = salDet.tipo_mov;
                            parametersMovimientosDetalles.Add("@numero", OdbcType.Numeric).Value = ultimoMovimiento[0].numero + 1;
                            parametersMovimientosDetalles.Add("@partida", OdbcType.Numeric).Value = salDet.partida;
                            parametersMovimientosDetalles.Add("@insumo", OdbcType.Numeric).Value = salDet.insumo;
                            parametersMovimientosDetalles.Add("@comentarios", OdbcType.Char).Value = salDet.comentarios != null ? salDet.comentarios : "";
                            parametersMovimientosDetalles.Add("@area", OdbcType.Numeric).Value = salDet.area;
                            parametersMovimientosDetalles.Add("@cuenta", OdbcType.Numeric).Value = salDet.cuenta;
                            parametersMovimientosDetalles.Add("@cantidad", OdbcType.Numeric).Value = salDet.cantidad;
                            parametersMovimientosDetalles.Add("@precio", OdbcType.Numeric).Value = costoPromedio;
                            parametersMovimientosDetalles.Add("@importe", OdbcType.Numeric).Value = importe;
                            parametersMovimientosDetalles.Add("@partida_oc", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@costo_prom", OdbcType.Numeric).Value = costoPromedio;
                            parametersMovimientosDetalles.Add("@obra", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@manzana_ini", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@lote_ini", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@interior_ini", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@manzana_fin", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@lote_fin", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@interior_fin", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@remision", OdbcType.Char).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@sector_id", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@id_resguardo", OdbcType.Numeric).Value = DBNull.Value;
                            parametersMovimientosDetalles.Add("@area_alm", OdbcType.Char).Value = salDet.area_alm;
                            parametersMovimientosDetalles.Add("@lado_alm", OdbcType.Char).Value = salDet.lado_alm;
                            parametersMovimientosDetalles.Add("@estante_alm", OdbcType.Char).Value = salDet.estante_alm;
                            parametersMovimientosDetalles.Add("@nivel_alm", OdbcType.Char).Value = salDet.nivel_alm;
                            parametersMovimientosDetalles.Add("@fecha_fisico", OdbcType.Date).Value = DBNull.Value;

                            commandMovimientosDetalles.Connection = trans.Connection;
                            commandMovimientosDetalles.Transaction = trans;

                            successDet = commandMovimientosDetalles.ExecuteNonQuery();

                            #region Actualizar Tablas Acumula
                            var objAcumula = new MovimientoDetalleEnkontrolDTO
                            {
                                insumo = salDet.insumo,
                                cantidad = salDet.cantidad,
                                precio = costoPromedio,
                                tipo_mov = salDet.tipo_mov,
                                costo_prom = costoPromedio
                            };

                            actualizarAcumula(nuevaSalida.almacen, nuevaSalida.cc, objAcumula, null, trans);
                            #endregion
                        }

                        if (success > 0 && successDet > 0)
                        {
                            movSalidas.Add(new salidasAlmacenDTO
                            {
                                almacen = sal.almacenOrigenDesc,
                                centroCosto = sal.ccDesc,
                                fechaSalida = nuevaSalida.fecha,
                                folioSalida = (ultimoMovimiento[0].numero + 1).ToString(),
                                partida = insumoReq != null ? insumoReq.partida : 0,
                                insumo = sal.insumo + "-" + sal.insumoDesc ?? "",
                                areaCuenta = insumoReq != null ? insumoReq.area.ToString() + "-" + insumoReq.cuenta.ToString() : "00-00",
                                cantidad = sal.cantidad,
                                costoPromedio = 0,
                                importe = sal.importe,
                            });
                            _context.SaveChanges();
                            trans.Commit();
                        }
                    }
                }


            }
            return movSalidas;
        }
        #endregion

        #region Pendiente por Surtir
        public List<RequisicionDTO> ObtenerRequisicionesPendientes(List<string> listaCC, List<int> listaAlmacenes, int estatus, int validadoAlmacen, DateTime fechaInicio, DateTime fechaFin)
        {
            var listaRequisiciones = new List<RequisicionDTO>();

            try
            {
                var centrosCosto = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        cc AS Value, 
                                        descripcion AS Text 
                                    FROM cc 
                                    WHERE st_ppto != 'T' 
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                if (listaCC != null && listaCC.Count() > 0)
                {
                    centrosCosto = centrosCosto.Where(x => listaCC.Contains(x.Value)).ToList();
                }

                var almacenes = (List<Core.DTO.Principal.Generales.ComboDTO>)consultaCheckProductivo(
                 string.Format(@"SELECT 
                                        alm.almacen AS Value, 
                                        alm.descripcion AS Text 
                                    FROM si_almacen alm                
                                    ORDER BY Text ASC")).ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();

                var requisiciones = _context.tblCom_Req.Where(x =>
                    x.estatusRegistro && x.stAutoriza == true && x.stEstatus != "T" && DbFunctions.TruncateTime(x.fecha) >= DbFunctions.TruncateTime(fechaInicio) && DbFunctions.TruncateTime(x.fecha) <= DbFunctions.TruncateTime(fechaFin)
                ).ToList();

                #region Filtros
                if (listaCC != null && listaCC.Count() > 0)
                {
                    requisiciones = requisiciones.Where(x => listaCC.Contains(x.cc)).ToList();
                }

                if (listaAlmacenes != null && listaAlmacenes.Count() > 0)
                {
                    requisiciones = requisiciones.Where(x => listaAlmacenes.Contains(x.idLibreAbordo)).ToList();
                }

                switch (validadoAlmacen)
                {
                    case 1: //Validado
                        requisiciones = requisiciones.Where(x => x.validadoAlmacen == true).ToList();
                        break;
                    case 2: //No Validado
                        requisiciones = requisiciones.Where(x => x.validadoAlmacen == false).ToList();
                        break;
                    default: //Ambas
                        break;
                }
                #endregion

                var requisiciones_id = requisiciones.Select(y => y.id).ToList();
                var requisicionesDetalle = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && requisiciones_id.Contains(x.idReq)).ToList();

                //var listaReqPendientes = ObtenerRequisicionesPendientesPorEstatus(listaCC, fechaInicio, fechaFin, almacenes);

                //var listaReqParciales = listaReqPendientes
                //    .Where(x => ((x.cantidadAE + x.cantidadAP + x.cantidadOC) > 1 && (x.cantidadAE + x.cantidadAP + x.cantidadOC) < x.solicitado) && (x.solicitado > 0)).ToList();

                //var listaReqSinSurtir = listaReqPendientes
                //    .Where(x => (x.cantidadAE == 0 && x.cantidadAP == 0 && x.cantidadOC == 0)).ToList();

                var listaSurtidoRequisicion = _context.tblCom_SurtidoDet.Where(x => x.estatus).ToList();
                var listaTipoGrupoEK = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM grupos_insumo")).ToObject<List<dynamic>>();

                foreach (var req in requisiciones)
                {
                    //if (estatus == 0)
                    //{
                    //    existe = listaReqPendientes.Any(x => x.numeroReq == re.numero);
                    //}
                    //else if (estatus == 1)
                    //{
                    //    existe = listaReqParciales.Any(x => x.numeroReq == re.numero);
                    //}
                    //else if (estatus == 2)
                    //{
                    //    existe = listaReqSinSurtir.Any(x => x.numeroReq == re.numero);
                    //}

                    var surtidoRequisicion = listaSurtidoRequisicion.Where(x => x.cc == req.cc && x.numero == req.numero).ToList();
                    var estatusSurtido = "";
                    var ocs = "";
                    if (surtidoRequisicion.Count() > 0)
                    {
                        var compras = _context.tblCom_OrdenCompraDet.ToList().Where(x =>
                            surtidoRequisicion.Any(y => y.cc == x.cc && y.numero == x.num_requisicion)
                        ).Select(x => x.numero).Distinct().ToList();
                        ocs = string.Join(",", compras);
                        if (surtidoRequisicion.All(x => x.estadoSurtido == "S"))
                        {
                            estatusSurtido = "Surtido";
                        }
                        else if (surtidoRequisicion.Any(x => x.estadoSurtido == "P"))
                        {
                            estatusSurtido = "Parcial";
                        }
                        else
                        {

                            estatusSurtido = "Sin Surtir";
                        }
                    }
                    else
                    {
                        var compras = _context.tblCom_OrdenCompraDet.Where(x => x.cc == req.cc && x.num_requisicion == req.numero).Distinct().Select(x => x.numero).Distinct().ToList();
                        ocs = string.Join(",", compras);
                        estatusSurtido = "Sin Surtir";
                    }

                    var agregar = false;

                    switch (estatus)
                    {
                        case 0:
                            agregar = true;
                            break;
                        case 1:
                            if (estatusSurtido == "Parcial")
                            {
                                agregar = true;
                            }
                            break;
                        case 2:
                            if (estatusSurtido == "Sin Surtir")
                            {
                                agregar = true;
                            }
                            break;
                    }

                    #region Validación para quitar las requisiciones no inventariables.
                    var flagNoInventariable = false;
                    var requisicionDetalle = requisicionesDetalle.Where(x => x.idReq == req.id).ToList();

                    foreach (var reqDet in requisicionDetalle)
                    {
                        var tipo = Int32.Parse(reqDet.insumo.ToString().Substring(0, 1));
                        var grupo = Int32.Parse(reqDet.insumo.ToString().Substring(1, 2));

                        var tipoGrupoEK = listaTipoGrupoEK.FirstOrDefault(x => (int)x.tipo_insumo == tipo && (int)x.grupo_insumo == grupo);

                        if (tipoGrupoEK != null)
                        {
                            if ((string)tipoGrupoEK.inventariado.Value == "N")
                            {
                                flagNoInventariable = true;
                            }
                        }
                    }
                    #endregion

                    if (agregar && !flagNoInventariable)
                    {
                        listaRequisiciones.Add(new RequisicionDTO
                        {
                            ccDescripcion = centrosCosto.Where(y => y.Value == req.cc).Select(y => req.cc + "-" + y.Text).FirstOrDefault(),
                            numero = req.numero,
                            fecha = req.fecha,
                            almacenLAB = almacenes.Where(y => y.Value == req.idLibreAbordo.ToString()).Select(y => req.idLibreAbordo + "-" + y.Text).FirstOrDefault(),
                            cc = req.cc,
                            libre_abordo = req.idLibreAbordo,
                            estatusSurtido = estatusSurtido,
                            validadoAlmacen = req.validadoAlmacen,
                            validadoRequisitor = req.validadoRequisitor,
                            consigna = req.consigna,
                            licitacion = req.licitacion,
                            crc = req.crc,
                            convenio = req.convenio,
                            numeroOCString = ocs
                        });
                    }
                }
            }
            catch (Exception)
            {
                listaRequisiciones = new List<RequisicionDTO>();
            }

            return listaRequisiciones;
        }

        public List<salidaConsumoDTO> ObtenerRequisicionesPendientesPorEstatus(List<string> listaCC, DateTime fechaInicio, DateTime fechaFin, List<Core.DTO.Principal.Generales.ComboDTO> almacenes)
        {
            List<salidaConsumoDTO> detalle = new List<salidaConsumoDTO>();

            try
            {
                var listaRequisiciones = _context.tblCom_Req.ToList().Where(x =>
                    x.estatusRegistro &&
                    ((listaCC != null && listaCC.Count() > 0) ? listaCC.Contains(x.cc) : true) &&
                    x.stEstatus != "T" &&
                    x.stAutoriza &&
                    x.fecha.Date >= fechaInicio.Date && x.fecha.Date <= fechaFin.Date
                ).ToList();

                var listaSurtidosAE = _context.tblCom_SurtidoDet.Where(x => x.tipoSurtidoDetalle == "AE" && x.estadoSurtido != "S").ToList();
                var listaSurtidosAP = _context.tblCom_SurtidoDet.Where(x => x.tipoSurtidoDetalle == "AP" && x.estadoSurtido != "S").ToList();

                foreach (var requi in listaRequisiciones)
                {
                    var insumosReqEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                            i.insumo, i.descripcion AS insumoDescripcion, rd.partida, rd.cantidad, rd.cc 
                                        FROM insumos i 
                                            INNER JOIN so_requisicion_det rd on rd.insumo = i.insumo 
                                        WHERE rd.cc = '{0}' AND rd.numero = '{1}' 
                                        ORDER BY i.insumo ASC", requi.cc, requi.numero));

                    if (insumosReqEK != null)
                    {
                        var insumosReq = (List<RequisicionDetDTO>)insumosReqEK.ToObject<List<RequisicionDetDTO>>();

                        var surtidosAE = listaSurtidosAE.Where(x => x.cc == requi.cc && x.surtido.numero == requi.numero).ToList();
                        var surtidoAP = listaSurtidosAP.Where(x => x.cc == requi.cc && x.surtido.numero == requi.numero).ToList();

                        #region Orden de Compra
                        var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", requi.cc, requi.numero));
                        List<dynamic> comprasDetalle = new List<dynamic>();

                        if (comprasDetalleEK != null)
                        {
                            comprasDetalle = (List<dynamic>)comprasDetalleEK.ToObject<List<dynamic>>();
                        }
                        #endregion

                        foreach (var ins in insumosReq)
                        {
                            var cantidadAP = surtidoAP.Where(x => x.insumo == ins.insumo).Select(x => x.cantidad).Sum();
                            var cantidadAE = surtidosAE.Where(x => x.insumo == ins.insumo).Select(x => x.cantidad).Sum();
                            var cantidadOC = default(decimal);

                            if (comprasDetalle.Count() > 0)
                            {
                                foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == ins.partida).ToList())
                                {
                                    cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                                }
                            }

                            detalle.Add(new salidaConsumoDTO
                            {
                                insumo = ins.insumo,
                                insumoDescripcion = ins.insumo + "-" + ins.insumoDescripcion,
                                solicitado = ins.cantidad,
                                existencia = cantidadAP + cantidadAE + cantidadOC,
                                cantidadAP = cantidadAP,
                                cantidadAE = cantidadAE,
                                cantidadOC = cantidadOC,
                                cc = ins.cc,
                                numeroReq = requi.numero
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                detalle = new List<salidaConsumoDTO>();
            }

            return detalle;
        }
        #endregion

        public rptRequisicionInfoDTO getRequisicionRpt(string cc, int numero, string PERU_tipoRequisicion)
        {

            // try
            // {
                var rptRequisicionInfo = new rptRequisicionInfoDTO();
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Construplan:
                        {
                            #region EmpresaPeru
                            using (var ctx = new MainContext())
                            {
                                var listaCentrosCosto = ctx.tblP_CC.ToList();

                                //var consultaUsuariosStarsoft = @"SELECT cast(TCLAVE as int) as TCLAVE,TDESCRI FROM [003BDCOMUN].[dbo].[TABAYU] WHERE TCOD = '12' ";
                                //List<InfoUsuariosStarsoftDTO> listaEmpleados = new List<InfoUsuariosStarsoftDTO>();
                                //DynamicParameters lstParametros = new DynamicParameters();
                                //using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                                //{
                                //    conexion.Open();
                                //    listaEmpleados = conexion.Query<InfoUsuariosStarsoftDTO>(consultaUsuariosStarsoft, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();
                                //    conexion.Close();
                                //}

                                var requisicionPeru = ctx.tblCom_Req.Where(x => x.cc == cc && x.numero == numero && x.estatusRegistro).ToList();

                                if (requisicionPeru != null)
                                {
                                    foreach (var req in requisicionPeru)
                                    {
                                        var partidaReqPeru = ctx.tblCom_ReqDet.Where(x => x.idReq == req.id && x.estatusRegistro).ToList();
                                        string descLAB = "";
                                        //using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                                        //{
                                        //    var objAlm = _starsoft.TABALM.ToList().FirstOrDefault(e => Convert.ToInt32(e.TAALMA) == req.idLibreAbordo);

                                        //    if (objAlm != null)
                                        //    {
                                        //        descLAB = objAlm.TADESCRI;
                                        //    }
                                        //}

                                        var empleadoSolicito = (from emp in ctx.tblP_Usuario_Enkontrol
                                                                join usu in ctx.tblP_Usuario
                                                                on emp.idUsuario equals usu.id
                                                                where emp.empleado == req.solicito
                                                                select usu.nombreUsuario).FirstOrDefault();

                                        var solicitoDesc = "";

                                        if (empleadoSolicito != null)
                                        {
                                            solicitoDesc = (string)empleadoSolicito;
                                        }

                                        var centroCosto = listaCentrosCosto.FirstOrDefault(x => (string)x.cc == req.cc);
                                        var ccDesc = "";

                                        if (centroCosto != null)
                                        {
                                            ccDesc = (string)centroCosto.descripcion;
                                        }

                                        string estatus = "";

                                        switch ((string)req.stEstatus)
                                        {
                                            case "P":
                                                estatus = "Parcialmente Comprado";
                                                break;
                                            case "T":
                                                estatus = "Total";
                                                break;
                                            case "C":
                                                estatus = "Totalmente Cancelado";
                                                break;
                                            default:
                                                break;
                                        };

                                        //ESTATUS REQ OC

                                        string estatusReqOC = "";
                                        switch (req.idTipoReqOc)
                                        {
                                            case 1:
                                                estatusReqOC = "Normal";
                                                break;
                                            case 2:
                                                estatusReqOC = "Urgente";
                                                break;
                                            case 3:
                                                estatusReqOC = "Programa mensual";
                                                break;
                                            case 4:
                                                estatusReqOC = "Por reembolso";
                                                break;
                                            default:
                                                break;
                                        }

                                        var estatusVencido = "";
                                        var tipoRequisicionDesc = "";
                                        foreach (var req2 in requisicionPeru)
                                        {

                                            var fechaHoy = DateTime.Now;
                                            if (req.fechaValidacionAlmacen != null)
                                            {
                                                var diferenciaDias = (fechaHoy.Date - (DateTime)req2.fechaValidacionAlmacen).TotalDays;

                                                switch (req2.idTipoReqOc)
                                                {
                                                    case 1: //Normal (3 días)
                                                        tipoRequisicionDesc = "NORMAL";
                                                        estatusVencido = diferenciaDias > 3 ? "VENCIDO" : "VIGENTE";
                                                        break;
                                                    case 2: //Urgente (3 días)
                                                        tipoRequisicionDesc = "URGENTE";
                                                        estatusVencido = diferenciaDias > 3 ? "VENCIDO" : "VIGENTE";
                                                        break;
                                                    case 3: //Programa Mensual (1 día)
                                                        tipoRequisicionDesc = "PROGRAMA";
                                                        estatusVencido = diferenciaDias > 1 ? "VENCIDO" : "VIGENTE";
                                                        break;
                                                    case 4: //Por reembolso (1 día)
                                                        tipoRequisicionDesc = "REEMBOLSO";
                                                        estatusVencido = diferenciaDias > 1 ? "VENCIDO" : "VIGENTE";
                                                        break;
                                                }
                                            }
                                        }

                                        List<rptRequisicionPartidasDTO> rptRequisicionPartidas = new List<rptRequisicionPartidasDTO>();
                                        foreach (var p in partidaReqPeru)
                                        {
                                            var partidaOrdenCompraPeru = ctx.Select<tblCom_OrdenCompraDet>(new DapperDTO
                                            {
                                                baseDatos = MainContextEnum.Construplan,
                                                consulta = @"
                                                    SELECT
                                                        TOP 1 det.*
                                                    FROM tblCom_OrdenCompraDet AS det
                                                        INNER JOIN tblCom_OrdenCompra AS com ON com.id = det.idOrdenCompra
                                                        INNER JOIN tblCom_Req AS r ON r.numero = @paramNumRequi AND r.numero = det.num_requisicion AND r.estatusRegistro = 1 
                                                    WHERE det.estatusRegistro = 1 AND com.estatusRegistro = 1 AND com.cc = @paramCC AND det.insumo = @paramInsumo",
                                                parametros = new { paramTipoRequi = PERU_tipoRequisicion, paramCC = req.cc, paramInsumo = p.insumo, paramNumRequi = req.numero }
                                            }).FirstOrDefault();

                                            string numeroOrdenCompra = "";
                                            string proveedorOrdenCompra = "";
                                            string partidaDescripcion = "";

                                            if (partidaOrdenCompraPeru != null)
                                            {
                                                numeroOrdenCompra = fillNo(((int)partidaOrdenCompraPeru.numero).ToString(), 6);
                                                partidaDescripcion = (string)partidaOrdenCompraPeru.partidaDescripcion != null ? ((string)partidaOrdenCompraPeru.partidaDescripcion).ToString() : "";
                                                var ordenCompra = ctx.tblCom_OrdenCompra.FirstOrDefault(x => x.id == partidaOrdenCompraPeru.idOrdenCompra && x.estatusRegistro);
                                                var proveedorNum = ordenCompra.proveedor;
                                                var proveedor = ctx.tblCom_sp_proveedores.FirstOrDefault(x => x.numpro == proveedorNum && x.registroActivo);
                                                if(proveedor != null)
                                                {
                                                    proveedorOrdenCompra = proveedor.nomcorto;
                                                }
                                            }

                                            var insumoObj = ctx.tblAlm_Insumo.FirstOrDefault(x => x.insumo == p.insumo);
                                            rptRequisicionPartidas.Add(new rptRequisicionPartidasDTO
                                            {
                                                partida = ((int)p.partida).ToString(),
                                                insumo = (int)p.insumo + " " + (string)insumoObj.descripcion + " " + p.descripcion,
                                                areaCuenta = p.noEconomico,
                                                fechaRequerido = ((DateTime)p.requerido).ToShortDateString(),
                                                cantidadRequerida = (p.cantidad).ToString() + " " + (string)insumoObj.unidad,
                                                estatus = (string)p.estatus,
                                                ordenCompra = numeroOrdenCompra != null ? (numeroOrdenCompra).ToString() : "",
                                                fechaOrdenada = p.ordenada != null ? ((DateTime)p.ordenada).ToShortDateString() : "",
                                                cantidadOrdenada = p.cantOrdenada.ToString(),
                                                proveedor = ""
                                            });
                                        }

                                        string nombreUsrSoli = "";
                                        string nombreUsrAuth = "";
                                        string nombreUsrVobo = "";

                                        var objUsuarioSoliEK = ctx.tblP_Usuario.FirstOrDefault(e => e.id == req.solicito);
                                        var objUsuarioAuthEK = ctx.tblP_Usuario.FirstOrDefault(e => e.id == req.empAutoriza);
                                        var objUsuarioVoboEK = ctx.tblP_Usuario.FirstOrDefault(e => e.id == req.vobo);

                                        if (objUsuarioSoliEK != null)
                                        {
                                            nombreUsrSoli = objUsuarioSoliEK.nombre + " " + objUsuarioSoliEK.apellidoPaterno + " " + objUsuarioSoliEK.apellidoMaterno;
                                        }

                                        if (objUsuarioAuthEK != null)
                                        {
                                            nombreUsrAuth = objUsuarioAuthEK.nombre + " " + objUsuarioAuthEK.apellidoPaterno + " " + objUsuarioAuthEK.apellidoMaterno;

                                        }

                                        if (objUsuarioVoboEK != null)
                                        {
                                            nombreUsrVobo = objUsuarioVoboEK.nombre + " " + objUsuarioVoboEK.apellidoPaterno + " " + objUsuarioVoboEK.apellidoMaterno;

                                        }

                                        var usuarioSolicitaDesc = "";
                                        if (req.usuarioSolicita > 0)
                                        {
                                            var cveEmpleado = req.usuarioSolicita.ToString();
                                            var usuarioSpSolicita = ctx.tblP_Usuario.FirstOrDefault(x => x.cveEmpleado == cveEmpleado && x.cveEmpleado != null && x.cveEmpleado != "0" && x.cveEmpleado != "");
                                            if (usuarioSpSolicita != null)
                                            {
                                                usuarioSolicitaDesc = PersonalUtilities.NombreCompletoMayusculas(usuarioSpSolicita.nombre, usuarioSpSolicita.apellidoPaterno, usuarioSpSolicita.apellidoMaterno);
                                            }
                                        }


                                        rptRequisicionInfo = new rptRequisicionInfoDTO
                                       {
                                           folioReq = (string)req.cc + "-" + fillNo(((int)req.numero).ToString(), 6),
                                           cc = (string)req.cc + " " + (string)ccDesc,
                                           lab = req.idLibreAbordo + " - " + descLAB,
                                           tipoReq = estatusReqOC,
                                           fechaHoy = DateTime.Now.Date.ToShortDateString(),
                                           fechaReq = ((DateTime)req.fecha).ToShortDateString(),
                                           estatus = estatus,
                                           comentarios = req.comentarios != null ? (string)req.comentarios : "",
                                           solicito = nombreUsrSoli,
                                           vobo = nombreUsrVobo,
                                           autorizo = nombreUsrAuth,
                                           usuarioSolicitaDesc = usuarioSolicitaDesc,
                                           usuarioSolicitaUso = "",
                                           partidas = rptRequisicionPartidas
                                       };

                                    }

                                }

                            }
                            #endregion
                        }
                        break;
                    case EmpresaEnum.Colombia:
                        {
                            #region EMPRESA COLOMBIA
                            var requisicionEKColombia = consultaCheckProductivo(
                            string.Format(@"SELECT 
                                        r.*, 
                                        s.descripcion AS solicitoNom, 
                                        e.descripcion AS empModificaNom, 
                                        v.descripcion AS voboNom, 
                                        a.descripcion AS empAutNom, 
                                        cc.descripcion AS ccDescripcion,
                                        tipo_req_oc AS tipoReqDescripcion
                                    FROM DBA.so_requisicion r 
                                        LEFT JOIN DBA.empleados s ON s.empleado = r.solicito 
                                        LEFT JOIN DBA.empleados v ON v.empleado = r.vobo 
                                        LEFT JOIN DBA.empleados e ON e.empleado = r.empleado_modifica 
                                        LEFT JOIN DBA.empleados a ON a.empleado = r.emp_autoriza 
                                        INNER JOIN DBA.cc cc ON r.cc = cc.cc 
                                    WHERE r.cc = '{0}' AND r.numero = {1}", cc, numero));

                            if (requisicionEKColombia != null)
                            {
                                var partidasEKColombia = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        d.*, 
                                        i.descripcion AS insumoDesc, 
                                        i.unidad, 
                                        i.cancelado, 
                                        (
                                            SELECT 
                                                l.descripcion 
                                            FROM DBA.so_req_det_linea l 
                                            WHERE l.cc = d.cc AND l.numero = d.numero AND l.partida = d.partida 
                                        ) AS partidaDesc 
                                    FROM DBA.so_requisicion_det d 
                                        INNER JOIN DBA.insumos i ON d.insumo = i.insumo 
                                    WHERE d.cc = '{0}' AND d.numero = {1} 
                                    ORDER BY d.partida", cc, numero));

                                var req = (List<dynamic>)requisicionEKColombia.ToObject<List<dynamic>>();
                                var part = (List<dynamic>)partidasEKColombia.ToObject<List<dynamic>>();

                                string estatus = "";

                                switch ((string)req[0].st_estatus.Value)
                                {
                                    case "P":
                                        estatus = "Parcialmente Comprado";
                                        break;
                                    case "T":
                                        estatus = "Total";
                                        break;
                                    case "C":
                                        estatus = "Totalmente Cancelado";
                                        break;
                                    default:
                                        break;
                                };

                                List<rptRequisicionPartidasDTO> rptRequisicionPartidas = new List<rptRequisicionPartidasDTO>();

                                var req_cc = (string)req[0].cc;
                                var req_numero = (int)req[0].numero;
                                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == req_cc && x.numero == req_numero);
                                var listaPartidasRequisicionSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicionSIGOPLAN.id);

                                foreach (var p in part)
                                {
                                    var partidaOrdenCompraEK = consultaCheckProductivo(
                                    string.Format(@"SELECT 
                                            * 
                                        FROM DBA.so_orden_compra_det 
                                        WHERE 
                                            cc = '{0}' AND 
                                            num_requisicion = {1} AND 
                                            part_requisicion = {2} AND 
                                            insumo = {3} AND 
                                            (cantidad - cant_canc) > 0", cc, numero, (int)p.partida.Value, (int)p.insumo.Value));

                                    string numeroOrdenCompra = "";
                                    string proveedorOrdenCompra = "";

                                    if (partidaOrdenCompraEK != null)
                                    {
                                        var partidaOrdenCompra = (List<dynamic>)partidaOrdenCompraEK.ToObject<List<dynamic>>();

                                        numeroOrdenCompra = fillNo(((int)partidaOrdenCompra[0].numero.Value).ToString(), 6);

                                        var ordenCompra = (List<dynamic>)consultaCheckProductivo(
                                        string.Format(@"SELECT 
                                                * 
                                            FROM DBA.so_orden_compra 
                                            WHERE cc = '{0}' AND numero = {1}", (string)partidaOrdenCompra[0].cc.Value, (int)partidaOrdenCompra[0].numero.Value)).ToObject<List<dynamic>>();

                                        var proveedorNum = (int)ordenCompra[0].proveedor.Value;

                                        var proveedorEK = consultaCheckProductivo(
                                        string.Format(@"SELECT 
                                                * 
                                            FROM DBA.sp_proveedores 
                                            WHERE numpro = {0}", proveedorNum));

                                        if (proveedorEK != null)
                                        {
                                            var proveedor = (List<dynamic>)proveedorEK.ToObject<List<dynamic>>();

                                            proveedorOrdenCompra = (string)proveedor[0].nombre.Value;
                                        }
                                    }

                                    var partida = (int)p.partida.Value;
                                    var partidaRequisicionSIGOPLAN = listaPartidasRequisicionSIGOPLAN.FirstOrDefault(x => x.partida == partida);

                                    rptRequisicionPartidas.Add(new rptRequisicionPartidasDTO
                                    {
                                        partida = ((int)p.partida.Value).ToString(),
                                        insumo = (int)p.insumo.Value + " " + (string)p.insumoDesc.Value + " " + (string)p.partidaDesc.Value,
                                        areaCuenta = partidaRequisicionSIGOPLAN.noEconomico,
                                        fechaRequerido = ((DateTime)p.fecha_requerido.Value).ToShortDateString(),
                                        cantidadRequerida = (p.cantidad.Value).ToString() + " " + (string)p.unidad.Value,
                                        estatus = (string)p.estatus.Value,
                                        ordenCompra = numeroOrdenCompra,
                                        fechaOrdenada = p.fecha_ordenada.Value != null ? ((DateTime)p.fecha_ordenada.Value).ToShortDateString() : "",
                                        cantidadOrdenada = p.cant_ordenada.Value != null ? (p.cant_ordenada.Value).ToString() : "",
                                        proveedor = proveedorOrdenCompra
                                    });
                                }

                                var idTipoReq = _context.tblCom_ReqTipo.ToList().FirstOrDefault(x => x.tipo_req_oc == (int)req[0].tipoReqDescripcion);
                                var TipoReq = "";
                                if (idTipoReq == null)
                                {
                                    TipoReq = string.Empty;
                                }
                                else
                                {
                                    TipoReq = idTipoReq.descripcion;
                                }

                                rptRequisicionInfo = new rptRequisicionInfoDTO
                                {
                                    folioReq = (string)req[0].cc.Value + "-" + fillNo(((int)req[0].numero.Value).ToString(), 6),
                                    cc = (string)req[0].cc.Value + " " + (string)req[0].ccDescripcion.Value,
                                    lab = "",
                                    tipoReq = TipoReq,
                                    fechaHoy = DateTime.Now.Date.ToShortDateString(),
                                    fechaReq = ((DateTime)req[0].fecha.Value).ToShortDateString(),
                                    estatus = estatus,
                                    comentarios = req[0].comentarios.Value != null ? (string)req[0].comentarios.Value : "",
                                    solicito = req[0].solicitoNom.Value != null ? (string)req[0].solicitoNom.Value : "",
                                    vobo = req[0].voboNom.Value != null ? (string)req[0].voboNom.Value : "",
                                    autorizo = req[0].empAutNom.Value != null ? (string)req[0].empAutNom.Value : "",
                                    usuarioSolicitaDesc = "",
                                    usuarioSolicitaUso = "",
                                    partidas = rptRequisicionPartidas
                                };
                            }
                            #endregion
                        }
                        break;
                    default:
                        {
                            #region Empresa Construplan
                            var requisicionEK = consultaCheckProductivo(
                              string.Format(@"SELECT 
                                        r.*, 
                                        s.descripcion AS solicitoNom, 
                                        e.descripcion AS empModificaNom, 
                                        v.descripcion AS voboNom, 
                                        a.descripcion AS empAutNom, 
                                        cc.descripcion AS ccDescripcion, 
                                        tr.descripcion AS tipoReqDescripcion 
                                    FROM so_requisicion r 
                                        LEFT JOIN empleados s ON s.empleado = r.solicito 
                                        LEFT JOIN empleados v ON v.empleado = r.vobo 
                                        LEFT JOIN empleados e ON e.empleado = r.empleado_modifica 
                                        LEFT JOIN empleados a ON a.empleado = r.emp_autoriza 
                                        INNER JOIN cc cc ON r.cc = cc.cc 
                                        INNER JOIN so_tipo_requisicion tr ON r.tipo_req_oc = tr.tipo_req_oc 
                                    WHERE r.cc = '{0}' AND r.numero = {1}", cc, numero));

                            if (requisicionEK != null)
                            {
                                var partidasEK = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        d.*, 
                                        i.descripcion AS insumoDesc, 
                                        i.unidad, 
                                        i.cancelado, 
                                        (
                                            SELECT 
                                                l.descripcion 
                                            FROM so_req_det_linea l 
                                            WHERE l.cc = d.cc AND l.numero = d.numero AND l.partida = d.partida 
                                        ) AS partidaDesc 
                                    FROM so_requisicion_det d 
                                        INNER JOIN insumos i ON d.insumo = i.insumo 
                                    WHERE d.cc = '{0}' AND d.numero = {1} 
                                    ORDER BY d.partida", cc, numero));

                                var req = (List<dynamic>)requisicionEK.ToObject<List<dynamic>>();
                                var part = (List<dynamic>)partidasEK.ToObject<List<dynamic>>();

                                string estatus = "";

                                switch ((string)req[0].st_estatus.Value)
                                {
                                    case "P":
                                        estatus = "Parcialmente Comprado";
                                        break;
                                    case "T":
                                        estatus = "Total";
                                        break;
                                    case "C":
                                        estatus = "Totalmente Cancelado";
                                        break;
                                    default:
                                        break;
                                };

                                var almacenes = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        * 
                                    FROM si_almacen")).ToObject<List<dynamic>>();

                                var libreAbordo = (List<dynamic>)consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        * 
                                    FROM so_libre_abordo")).ToObject<List<dynamic>>();

                                string libre_abordo = "";
                                int libreAbordoID = (int)req[0].libre_abordo.Value;

                                if (almacenes.FirstOrDefault(x => (int)x.almacen.Value == libreAbordoID) != null)
                                {
                                    libre_abordo = (string)almacenes.FirstOrDefault(x => (int)x.almacen.Value == libreAbordoID).descripcion.Value;
                                }
                                else if (libreAbordo.FirstOrDefault(x => (int)x.numero.Value == libreAbordoID) != null)
                                {
                                    libre_abordo = (string)libreAbordo.FirstOrDefault(x => (int)x.numero.Value == libreAbordoID).descripcion.Value;
                                }

                                List<rptRequisicionPartidasDTO> rptRequisicionPartidas = new List<rptRequisicionPartidasDTO>();

                                foreach (var p in part)
                                {
                                    var partidaOrdenCompraEK = consultaCheckProductivo(
                                    string.Format(@"SELECT 
                                            * 
                                        FROM so_orden_compra_det 
                                        WHERE 
                                            cc = '{0}' AND 
                                            num_requisicion = {1} AND 
                                            part_requisicion = {2} AND 
                                            insumo = {3} AND 
                                            (cantidad - cant_canc) > 0", cc, numero, (int)p.partida.Value, (int)p.insumo.Value));

                                    string numeroOrdenCompra = "";
                                    string proveedorOrdenCompra = "";

                                    if (partidaOrdenCompraEK != null)
                                    {
                                        var partidaOrdenCompra = (List<dynamic>)partidaOrdenCompraEK.ToObject<List<dynamic>>();

                                        numeroOrdenCompra = fillNo(((int)partidaOrdenCompra[0].numero.Value).ToString(), 6);

                                        var ordenCompra = (List<dynamic>)consultaCheckProductivo(
                                        string.Format(@"SELECT 
                                                * 
                                            FROM so_orden_compra 
                                            WHERE cc = '{0}' AND numero = {1}", (string)partidaOrdenCompra[0].cc.Value, (int)partidaOrdenCompra[0].numero.Value)).ToObject<List<dynamic>>();

                                        var proveedorNum = (int)ordenCompra[0].proveedor.Value;

                                        var proveedorEK = consultaCheckProductivo(
                                        string.Format(@"SELECT 
                                                * 
                                            FROM sp_proveedores 
                                            WHERE numpro = {0}", proveedorNum));

                                        if (proveedorEK != null)
                                        {
                                            var proveedor = (List<dynamic>)proveedorEK.ToObject<List<dynamic>>();

                                            proveedorOrdenCompra = (string)proveedor[0].nombre.Value;
                                        }
                                    }

                                    rptRequisicionPartidas.Add(new rptRequisicionPartidasDTO
                                    {
                                        partida = ((int)p.partida.Value).ToString(),
                                        insumo = (int)p.insumo.Value + " " + (string)p.insumoDesc.Value + " " + (string)p.partidaDesc.Value,
                                        areaCuenta = (int)p.area.Value + "-" + (int)p.cuenta.Value,
                                        fechaRequerido = ((DateTime)p.fecha_requerido.Value).ToShortDateString(),
                                        cantidadRequerida = (p.cantidad.Value).ToString() + " " + (string)p.unidad.Value,
                                        estatus = (string)p.estatus.Value,
                                        ordenCompra = numeroOrdenCompra,
                                        fechaOrdenada = p.fecha_ordenada.Value != null ? ((DateTime)p.fecha_ordenada.Value).ToShortDateString() : "",
                                        cantidadOrdenada = p.cant_ordenada.Value != null ? (p.cant_ordenada.Value).ToString() : "",
                                        proveedor = proveedorOrdenCompra
                                    });
                                }

                                var requisicionSIGOPLAN = _context.tblCom_Req.ToList().FirstOrDefault(x => x.estatusRegistro && x.cc == (string)req[0].cc && x.numero == (int)req[0].numero);

                                if (requisicionSIGOPLAN != null)
                                {
                                    var almacen = almacenes.FirstOrDefault(x => (int)x.almacen == requisicionSIGOPLAN.idLibreAbordo);

                                    libre_abordo = (int)almacen.almacen + " - " + (string)almacen.descripcion;
                                }

                                //                    List<dynamic> empleadoEnkontrol = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanRh : EnkontrolEnum.ArrenRh, new OdbcConsultaDTO()
                                //                    {
                                //                        consulta = @"
                                //                            SELECT 
                                //                                e.clave_empleado AS claveEmpleado, 
                                //                                (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado
                                //                            FROM DBA.sn_empleados AS e 
                                //                            WHERE e.estatus_empleado = 'A' AND e.claveEmpleado = ?",
                                //                        parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO() { nombre = "claveEmpleado", tipo = OdbcType.Numeric, valor = requisicionSIGOPLAN.usuarioSolicita } }
                                //                    });

                                var empleadoEnkontrol = _context.Select<dynamic>(new DapperDTO
                                {
                                    baseDatos = (int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan ? MainContextEnum.Construplan : MainContextEnum.Arrendadora,
                                    consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado
                                    FROM tblRH_EK_Empleados AS e 
                                    WHERE e.estatus_empleado = 'A' AND e.claveEmpleado = @usr",
                                    parametros = new { usr = requisicionSIGOPLAN.usuarioSolicita }
                                });

                                if (empleadoEnkontrol.Count() == 0)
                                {
                                    empleadoEnkontrol = _context.Select<dynamic>(new DapperDTO
                                    {
                                        baseDatos = MainContextEnum.GCPLAN,
                                        consulta = @"SELECT 
                                        e.clave_empleado AS claveEmpleado, 
                                        (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado
                                    FROM tblRH_EK_Empleados AS e 
                                    WHERE e.estatus_empleado = 'A' AND e.claveEmpleado = @usr",
                                        parametros = new { usr = requisicionSIGOPLAN.usuarioSolicita }
                                    });
                                }

                                rptRequisicionInfo = new rptRequisicionInfoDTO
                                {
                                    folioReq = (string)req[0].cc.Value + "-" + fillNo(((int)req[0].numero.Value).ToString(), 6),
                                    cc = (string)req[0].cc.Value + " " + (string)req[0].ccDescripcion.Value,
                                    lab = libre_abordo,
                                    tipoReq = (string)req[0].tipoReqDescripcion.Value,
                                    fechaHoy = DateTime.Now.Date.ToShortDateString(),
                                    fechaReq = ((DateTime)req[0].fecha.Value).ToShortDateString(),
                                    estatus = estatus,
                                    comentarios = req[0].comentarios.Value != null ? (string)req[0].comentarios.Value : "",
                                    solicito = req[0].solicitoNom.Value != null ? (string)req[0].solicitoNom.Value : "",
                                    vobo = req[0].voboNom.Value != null ? (string)req[0].voboNom.Value : "",
                                    autorizo = req[0].empAutNom.Value != null ? (string)req[0].empAutNom.Value : "",
                                    usuarioSolicitaDesc = empleadoEnkontrol.Count() > 0 ? empleadoEnkontrol[0].nombreEmpleado : "",
                                    usuarioSolicitaUso = requisicionSIGOPLAN.usuarioSolicitaUso,
                                    partidas = rptRequisicionPartidas
                                };


                            }

                            #endregion
                        }
                        break;
                }

                return rptRequisicionInfo;
            // }
            // catch (Exception ex)
            // {
            //     return new rptRequisicionInfoDTO();
            // }
        }


        public Dictionary<string, object> validarSurtido(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == numero);

                if (requisicion != null)
                {
                    if (!requisicion.stAutoriza)
                    {
                        throw new Exception("La requisición no está autorizada en SIGOPLAN.");
                    }

                    requisicion.validadoAlmacen = true;
                    requisicion.validadoCompras = true;
                    requisicion.validadoRequisitor = true;
                    requisicion.fechaValidacionAlmacen = DateTime.Now;

                    _context.Entry(requisicion).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    var solicitoUsuarioEnkontrol = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == requisicion.solicito);

                    if (solicitoUsuarioEnkontrol != null)
                    {
                        var usuarioSIGOPLAN = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.id == solicitoUsuarioEnkontrol.idUsuario);

                        if (usuarioSIGOPLAN != null)
                        {
                            if (usuarioSIGOPLAN.correo != null)
                            {
                                #region Mandar correo al requisitor por requisición validada.
                                var titulo = "Requisición \"" + cc + "-" + numero + "\" ha sido validada por almacén.";
                                var mensaje = "Se ha validado la requisición \"" + cc + "-" + numero + "\" por almacén. \n" + "Fecha Validación: " + DateTime.Now.Date.ToShortDateString();
                                var correo = new List<string>();

                                if (requisicion.solicito != 1)
                                {
                                    correo.Add(usuarioSIGOPLAN.correo);
                                }
                                //else
                                //{
                                //    correo.Add("oscar.valencia@construplan.com.mx");
                                //}

                                if (correo.Count == 1)
                                {
#if DEBUG
                                    correo = new List<string> { "omar.nunez@construplan.com.mx" };
#endif
                                    Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), titulo), mensaje, correo);
                                }
                                #endregion

                                //Se le manda otro correo por si todo el surtido se va realizar desde almacén propio.

                                #region Mandar correo al requisitor si el surtido de la requisición está completo.
                                var surtidoRequisicionCompleto = verificarRequisicionCompletamenteSurtida(cc, numero);

                                if (surtidoRequisicionCompleto)
                                {
                                    var titulo2 = "Requisición \"" + cc + "-" + numero + "\" completamente surtida.";
                                    var mensaje2 = "La requisición \"" + cc + "-" + numero + "\" se ha surtido por completo. \n" + "Fecha Surtido Completo: " + DateTime.Now.Date.ToShortDateString();
                                    var correo2 = new List<string>();

                                    if (requisicion.solicito != 1)
                                    {
                                        correo2.Add(usuarioSIGOPLAN.correo);
                                    }

                                    if (correo2.Count == 1)
                                    {
#if DEBUG
                                        correo2 = new List<string> { "omar.nunez@construplan.com.mx" };
#endif
                                        Infrastructure.Utils.GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), titulo2), mensaje2, correo2);
                                    }
                                }
                                #endregion

                                #region Mandar correo al comprador.
                                try
                                {
                                    List<string> listaCorreos = new List<string>();

                                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                                    {
                                        var comprador = _context.tblAlm_RelacionCorreoAlmacenCC.FirstOrDefault(x => x.estatus && x.almacen == requisicion.idLibreAbordo && x.cc == requisicion.cc);

                                        if (comprador != null)
                                        {
                                            var usuarioComprador = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.correo.Contains("@construplan.com.mx") && x.id == comprador.compradorID);

                                            if (usuarioComprador != null)
                                            {
                                                listaCorreos.Add(usuarioComprador.correo);
                                            }
                                        }
                                    }
                                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                                    {
                                        var ccEnkontrol = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO
                                        {
                                            consulta = @"SELECT * FROM cc WHERE cc = ?",
                                            parametros = new List<OdbcParameterDTO> { new OdbcParameterDTO { nombre = "cc", tipo = OdbcType.VarChar, valor = requisicion.cc } }
                                        });
                                        var equipo = _context.tblM_CatMaquina.Where(x => x.estatus == 1).ToList().FirstOrDefault(x =>
                                            x.noEconomico.ToUpper() == ((string)ccEnkontrol[0].corto).ToUpper()
                                        );

                                        if (equipo != null)
                                        {
                                            var comprador = _context.tblAlm_RelacionCorreoAlmacenCC.FirstOrDefault(x =>
                                                x.estatus && x.almacen == requisicion.idLibreAbordo && x.areaCuenta == equipo.centro_costos
                                            );

                                            if (comprador != null)
                                            {
                                                var usuarioComprador = _context.tblP_Usuario.FirstOrDefault(x => x.estatus && x.correo.Contains("@construplan.com.mx") && x.id == comprador.compradorID);

                                                if (usuarioComprador != null)
                                                {
                                                    listaCorreos.Add(usuarioComprador.correo);
                                                }
                                            }
                                        }
                                    }

#if DEBUG
                                    listaCorreos = new List<string> { "omar.nunez@construplan.com.mx" };
#endif

                                    var titulo3 = string.Format(@"Requisición Procesada: {0}", requisicion.cc + "-" + requisicion.numero);
                                    var mensaje3 = string.Format(
                                        @"Se ha validado el surtido la requisición {0}.<br/>Fecha y hora: {1}", requisicion.cc + "-" + requisicion.numero, DateTime.Now
                                    );

                                    var correoEnviado = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), titulo3), mensaje3, listaCorreos.Distinct().ToList());
                                }
                                catch (Exception e)
                                {
                                    LogError(0, 0, "RequisicionController", "enviarCorreoValidarSurtido", e, AccionEnum.CORREO, 0, new { cc = cc, numero = numero });
                                }
                                #endregion
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("No se encuentra la información de la requisición en SIGOPLAN.");
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> validarSurtidoCompras(string cc, int numero)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == numero && x.stAutoriza);

                if (requisicion != null)
                {
                    requisicion.validadoCompras = true;

                    _context.Entry(requisicion).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(12, 0, "RequisicionController", "validarSurtidoCompras", e, AccionEnum.AGREGAR, 0, new { cc = cc, numero = numero });

                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public string fillNo(string e, int no)
        {
            var newe = "";
            var el = e.Length;
            if (e.Length < no)
            {
                for (int i = el; i < no; i++)
                {
                    newe += "0";
                }
                return newe + e;
            }
            else
            {
                return e;
            }
        }

        public Dictionary<string, object> getRequisicionesPorUsuarioProcesadas(List<string> listCC)
        {
            var result = new Dictionary<string, object>();
            var usuario = vSesiones.sesionUsuarioDTO;
            var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

            try
            {
                var requisiciones = (
                                        from req in _context.tblCom_Req
                                        join reqDet in _context.tblCom_ReqDet on req.id equals reqDet.idReq
                                        where
                                            req.estatusRegistro &&
                                            reqDet.estatusRegistro &&
                                            req.stAutoriza &&
                                            listCC.Contains(req.cc) &&
                                            req.solicito == relUser.empleado &&
                                            req.validadoAlmacen == true
                                        //(req.validadoRequisitor == false || req.validadoRequisitor == null)
                                        select new
                                        {
                                            cc = req.cc,
                                            numero = req.numero,
                                            solicito = req.solicito,
                                            fecha = req.fecha
                                        }
                                    ).Distinct().ToList();

                List<dynamic> resultado = new List<dynamic>();

                foreach (var req in requisiciones)
                {
                    var ccEK = consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}' AND st_ppto != 'T'", req.cc));
                    var ccDesc = "";

                    if (ccEK != null)
                    {
                        ccDesc = ((List<dynamic>)ccEK.ToObject<List<dynamic>>())[0].descripcion.Value as string;
                    }

                    var solicitoDescEK = getUsuarioEnKontrol(req.solicito);
                    var solicitoDesc = "";

                    if (solicitoDescEK != null)
                    {
                        solicitoDesc = ((List<dynamic>)solicitoDescEK.ToObject<List<dynamic>>())[0].descripcion.Value as string;
                    }

                    var data = new
                    {
                        cc = req.cc,
                        ccDesc = req.cc + "-" + ccDesc,
                        numero = req.numero,
                        solicito = req.solicito,
                        solicitoDesc = solicitoDesc,
                        fecha = req.fecha
                    };

                    resultado.Add(data);
                }

                result.Add("data", resultado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public dynamic getUsuarioEnKontrol(int numEmpleado)
        {
            try
            {
                var empleado = consultaCheckProductivo(
                    string.Format(@"SELECT * FROM empleados WHERE empleado = {0}", numEmpleado)
                );

                return empleado;
            }
            catch (Exception) { return null; }
        }

        public Dictionary<string, object> validacionesRequisitor(string cc, List<int> numeros)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var usuario = vSesiones.sesionUsuarioDTO;
                var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                foreach (var num in numeros)
                {
                    var requisicion = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == num && x.stAutoriza);

                    if (requisicion != null)
                    {
                        requisicion.validadoRequisitor = true;

                        _context.Entry(requisicion).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public Dictionary<string, object> getUbicacionDetalle(string cc, int almacenID, int insumo)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                List<UbicacionDetalleDTO> ubicacionDetalle = new List<UbicacionDetalleDTO>();

                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERÚ
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                var listaInsumosStarsoft = _starsoft.MAEART.ToList();

                                var entradas = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov < 50 && x.almacen == almacenID && x.insumo == insumo).ToList().GroupBy(x => new
                                {
                                    x.insumo,
                                    x.area_alm,
                                    x.lado_alm,
                                    x.estante_alm,
                                    x.nivel_alm
                                }).Select(x => new UbicacionDetalleDTO
                                {
                                    insumo = x.Key.insumo,
                                    insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.Key.insumo)).Select(z => z.ADESCRI).FirstOrDefault(),
                                    cantidad = x.Sum(y => y.cantidad),
                                    area_alm = x.Key.area_alm,
                                    lado_alm = x.Key.lado_alm,
                                    estante_alm = x.Key.estante_alm,
                                    nivel_alm = x.Key.nivel_alm
                                }).ToList();

                                if (entradas.Count() > 0)
                                {
                                    var salidas = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov > 50 && x.almacen == almacenID && x.insumo == insumo).ToList().GroupBy(x => new
                                    {
                                        x.insumo,
                                        x.area_alm,
                                        x.lado_alm,
                                        x.estante_alm,
                                        x.nivel_alm
                                    }).Select(x => new UbicacionDetalleDTO
                                    {
                                        insumo = x.Key.insumo,
                                        insumoDesc = listaInsumosStarsoft.Where(y => y.ACODIGO == ("0" + x.Key.insumo)).Select(z => z.ADESCRI).FirstOrDefault(),
                                        cantidad = x.Sum(y => y.cantidad),
                                        area_alm = x.Key.area_alm,
                                        lado_alm = x.Key.lado_alm,
                                        estante_alm = x.Key.estante_alm,
                                        nivel_alm = x.Key.nivel_alm
                                    }).ToList();

                                    if (salidas.Count() > 0)
                                    {
                                        foreach (var ent in entradas)
                                        {
                                            var salida = salidas.FirstOrDefault(x =>
                                                x.insumoDesc == ent.insumoDesc &&
                                                x.area_alm == ent.area_alm &&
                                                x.lado_alm == ent.lado_alm &&
                                                x.estante_alm == ent.estante_alm &&
                                                x.nivel_alm == ent.nivel_alm
                                            );

                                            if (salida != null)
                                            {
                                                ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                                            }
                                        }

                                        ubicacionDetalle.AddRange(entradas);
                                    }
                                    else
                                    {
                                        ubicacionDetalle.AddRange(entradas);
                                    }
                                }
                            }

                            break;
                            #endregion
                        }
                    case EmpresaEnum.Colombia:
                        {
                            #region COLOMBIA
                            var listaInsumos = _contextEnkontrol.Select<InsumoCatalogoDTO>(getEnkontrolEnumADM(), new OdbcConsultaDTO
                            {
                                consulta = @"SELECT ins.insumo, ins.descripcion AS insumoDesc FROM insumos ins"
                            });

                            var entradas = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov < 50 && x.almacen == almacenID && x.insumo == insumo).ToList().GroupBy(x => new
                            {
                                x.insumo,
                                x.area_alm,
                                x.lado_alm,
                                x.estante_alm,
                                x.nivel_alm
                            }).Select(x => new UbicacionDetalleDTO
                            {
                                insumo = x.Key.insumo,
                                insumoDesc = listaInsumos.Where(y => y.insumo == x.Key.insumo).Select(z => z.insumoDesc).FirstOrDefault(),
                                cantidad = x.Sum(y => y.cantidad),
                                area_alm = x.Key.area_alm,
                                lado_alm = x.Key.lado_alm,
                                estante_alm = x.Key.estante_alm,
                                nivel_alm = x.Key.nivel_alm
                            }).ToList();

                            if (entradas.Count() > 0)
                            {
                                var salidas = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov > 50 && x.almacen == almacenID && x.insumo == insumo).ToList().GroupBy(x => new
                                {
                                    x.insumo,
                                    x.area_alm,
                                    x.lado_alm,
                                    x.estante_alm,
                                    x.nivel_alm
                                }).Select(x => new UbicacionDetalleDTO
                                {
                                    insumo = x.Key.insumo,
                                    insumoDesc = listaInsumos.Where(y => y.insumo == x.Key.insumo).Select(z => z.insumoDesc).FirstOrDefault(),
                                    cantidad = x.Sum(y => y.cantidad),
                                    area_alm = x.Key.area_alm,
                                    lado_alm = x.Key.lado_alm,
                                    estante_alm = x.Key.estante_alm,
                                    nivel_alm = x.Key.nivel_alm
                                }).ToList();

                                if (salidas.Count() > 0)
                                {
                                    foreach (var ent in entradas)
                                    {
                                        var salida = salidas.FirstOrDefault(x =>
                                            x.insumoDesc == ent.insumoDesc &&
                                            x.area_alm == ent.area_alm &&
                                            x.lado_alm == ent.lado_alm &&
                                            x.estante_alm == ent.estante_alm &&
                                            x.nivel_alm == ent.nivel_alm
                                        );

                                        if (salida != null)
                                        {
                                            ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                                        }
                                    }

                                    ubicacionDetalle.AddRange(entradas);
                                }
                                else
                                {
                                    ubicacionDetalle.AddRange(entradas);
                                }
                            }

                            break;
                            #endregion
                        }
                    default:
                        {
                            #region OTRAS EMPRESAS
                            var entradasEK = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                                det.insumo, 
                                                (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                                SUM(det.cantidad) AS cantidad, 
                                                det.area_alm, 
                                                det.lado_alm, 
                                                det.estante_alm, 
                                                det.nivel_alm 
                                            FROM si_movimientos mov 
                                                INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                                INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                            WHERE mov.almacen = {0} AND det.insumo = {1} AND det.tipo_mov < 50 
                                            GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", almacenID, insumo));

                            var salidasEK = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                        det.insumo, 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE mov.almacen = {0} AND det.insumo = {1} AND det.tipo_mov > 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", almacenID, insumo));

                            if (entradasEK != null)
                            {
                                var entradas = (List<UbicacionDetalleDTO>)entradasEK.ToObject<List<UbicacionDetalleDTO>>();

                                if (salidasEK != null)
                                {
                                    var salidas = (List<UbicacionDetalleDTO>)salidasEK.ToObject<List<UbicacionDetalleDTO>>();

                                    foreach (var ent in entradas)
                                    {
                                        var salida = salidas.FirstOrDefault(x =>
                                                x.insumoDesc == ent.insumoDesc &&
                                                x.area_alm == ent.area_alm &&
                                                x.lado_alm == ent.lado_alm &&
                                                x.estante_alm == ent.estante_alm &&
                                                x.nivel_alm == ent.nivel_alm
                                            );

                                        if (salida != null)
                                        {
                                            ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad, CultureInfo.InvariantCulture);
                                        }
                                    }

                                    ubicacionDetalle.AddRange(entradas);
                                }
                                else
                                {
                                    ubicacionDetalle.AddRange(entradas);
                                }
                            }

                            break;
                            #endregion
                        }
                }

                result.Add("data", ubicacionDetalle);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public List<UbicacionDetalleDTO> getUbicacionPorRequisicion(RequisicionDTO requisicion)
        {
            var ubicacionDetalle = new List<UbicacionDetalleDTO>();

            var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == requisicion.cc && x.numero == requisicion.numero);

            var requisicionDetalle = (List<RequisicionDetDTO>)consultaCheckProductivo(
               string.Format(@"SELECT 
                                        i.insumo, i.descripcion AS insumoDescripcion, rd.partida, rd.cantidad, rd.cc 
                                    FROM insumos i 
                                        INNER JOIN so_requisicion_det rd on rd.insumo = i.insumo 
                                    WHERE rd.cc = '{0}' AND rd.numero = '{1}' 
                                    ORDER BY i.insumo ASC", requisicion.cc, requisicion.numero)).ToObject<List<RequisicionDetDTO>>();

            var insumosString = string.Join(", ", requisicionDetalle.Select(x => x.insumo.ToString()).ToList());

            var entradasEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE cc = '{0}' AND mov.almacen = {1} AND det.insumo IN ({2}) AND det.tipo_mov < 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", requisicion.cc, requisicion.libre_abordo, insumosString));

            var salidasEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                        (CAST(det.insumo AS Varchar) + '-' + ins.descripcion) AS insumoDesc, 
                                        SUM(det.cantidad) AS cantidad, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                    WHERE cc = '{0}' AND mov.almacen = {1} AND det.insumo IN ({2}) AND det.tipo_mov > 50 
                                    GROUP BY det.insumo, ins.descripcion, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", requisicion.cc, requisicion.libre_abordo, insumosString));

            if (entradasEK != null)
            {
                var entradas = (List<UbicacionDetalleDTO>)entradasEK.ToObject<List<UbicacionDetalleDTO>>();

                if (salidasEK != null)
                {
                    var salidas = (List<dynamic>)salidasEK.ToObject<List<dynamic>>();

                    foreach (var ent in entradas)
                    {
                        var salida = salidas.FirstOrDefault(x =>
                                x.insumoDesc.Value as string == ent.insumoDesc &&
                                x.area_alm.Value as string == ent.area_alm &&
                                x.lado_alm.Value as string == ent.lado_alm &&
                                x.estante_alm.Value as string == ent.estante_alm &&
                                x.nivel_alm.Value as string == ent.nivel_alm
                            );

                        if (salida != null)
                        {
                            ent.cantidad = ent.cantidad - Convert.ToDecimal(salida.cantidad.Value, CultureInfo.InvariantCulture);
                        }
                    }

                    ubicacionDetalle.AddRange(entradas.Select(x => new UbicacionDetalleDTO
                    {
                        ccDescripcion = requisicion.ccDescripcion,
                        numero = requisicion.numero,
                        fecha = requisicionSIGOPLAN == null ? requisicion.fecha : requisicionSIGOPLAN.fecha,
                        almacenLAB = requisicion.almacenLAB,
                        insumoDesc = x.insumoDesc,
                        cantidad = x.cantidad,
                        area_alm = x.area_alm,
                        lado_alm = x.lado_alm,
                        estante_alm = x.estante_alm,
                        nivel_alm = x.nivel_alm
                    }));
                }
                else
                {
                    ubicacionDetalle.AddRange(entradas.Select(x => new UbicacionDetalleDTO
                    {
                        ccDescripcion = requisicion.ccDescripcion,
                        numero = requisicion.numero,
                        fecha = requisicionSIGOPLAN == null ? requisicion.fecha : requisicionSIGOPLAN.fecha,
                        almacenLAB = requisicion.almacenLAB,
                        insumoDesc = x.insumoDesc,
                        cantidad = x.cantidad,
                        area_alm = x.area_alm,
                        lado_alm = x.lado_alm,
                        estante_alm = x.estante_alm,
                        nivel_alm = x.nivel_alm
                    }));
                }
            }

            return ubicacionDetalle;
        }

        public Dictionary<string, object> confirmarRequisicion(RequisicionDTO requisicion)
        {
            var result = new Dictionary<string, object>();

            try
            {
                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == requisicion.cc && x.numero == requisicion.numero && x.stAutoriza);

                if (requisicionSIGOPLAN != null)
                {
                    var partidasCantidadConfirmadaMenor = requisicion.partidas.Where(x => x.cantidadConfirmada < x.cantidad).ToList();

                    if (partidasCantidadConfirmadaMenor.Count > 0)
                    {
                        var requisicionDetalleSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.idReq == requisicionSIGOPLAN.id).ToList();

                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            using (var con = checkConexionProductivo())
                            {
                                using (var trans = con.BeginTransaction())
                                {
                                    try
                                    {
                                        foreach (var part in partidasCantidadConfirmadaMenor)
                                        {
                                            var partidaSIGOPLAN = requisicionDetalleSIGOPLAN.FirstOrDefault(x => x.partida == part.partida && x.insumo == part.insumo);
                                            var cantidadCancelada = part.cantidad - part.cantidadConfirmada;

                                            //Agregar la cantidad cancelada a la tabla detalle de SIGOPLAN.
                                            #region Update tblCom_ReqDet
                                            if (partidaSIGOPLAN != null)
                                            {
                                                partidaSIGOPLAN.cantCancelada += cantidadCancelada;

                                                _context.Entry(partidaSIGOPLAN).State = System.Data.Entity.EntityState.Modified;
                                                _context.SaveChanges();
                                            }
                                            #endregion

                                            //Agregar la cantidad cancelada a la tabla detalle de EnKontrol.
                                            #region Update EnKontrol so_requisicion_det
                                            var consultaUpdateDetalle = @"
                                                            UPDATE so_requisicion_det 
                                                                SET cant_cancelada = (cant_cancelada + ?) 
                                                            WHERE cc = '?' AND numero = ? AND partida = ? AND insumo = ?";

                                            using (var cmd = new OdbcCommand(consultaUpdateDetalle))
                                            {
                                                OdbcParameterCollection parameters = cmd.Parameters;

                                                parameters.Add("@cant_cancelada", OdbcType.Numeric).Value = cantidadCancelada;

                                                parameters.Add("@cc", OdbcType.Char).Value = requisicionSIGOPLAN.cc;
                                                parameters.Add("@numero", OdbcType.Numeric).Value = requisicionSIGOPLAN.numero;
                                                parameters.Add("@partida", OdbcType.Numeric).Value = part.partida;
                                                parameters.Add("@insumo", OdbcType.Numeric).Value = part.insumo;

                                                cmd.Connection = trans.Connection;
                                                cmd.Transaction = trans;
                                                var count = cmd.ExecuteNonQuery();
                                            }
                                            #endregion

                                            //Modificar la tabla de surtido en SIGOPLAN.
                                            #region Update tblCom_Surtido
                                            var listSurtidoSIGOPLAN =
                                                _context.tblCom_Surtido.Where(x =>
                                                    x.estatus &&
                                                    x.cc == requisicionSIGOPLAN.cc &&
                                                    x.numero == requisicionSIGOPLAN.numero &&
                                                    x.insumo == part.insumo
                                                ).ToList();

                                            var cantidadSurtido = listSurtidoSIGOPLAN.Select(x => x.cantidadTotal).Sum();

                                            if (cantidadSurtido > part.cantidadConfirmada)
                                            {
                                                var faltante = cantidadSurtido - part.cantidadConfirmada;

                                                foreach (var surt in listSurtidoSIGOPLAN)
                                                {
                                                    if (faltante > 0)
                                                    {
                                                        var resultado = surt.cantidadTotal - faltante;

                                                        if (resultado <= 0)
                                                        {
                                                            surt.cantidadTotal = 0;
                                                            surt.estatus = false;

                                                            _context.Entry(surt).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            surt.cantidadTotal = resultado;

                                                            _context.Entry(surt).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }

                                                        faltante -= surt.cantidadTotal;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            //Modificar la tabla de surtido detalle en SIGOPLAN.
                                            #region Update tblCom_SurtidoDet
                                            var listSurtidoDetalleSIGOPLAN =
                                                _context.tblCom_SurtidoDet.Where(x =>
                                                    x.estatus &&
                                                    x.cc == requisicionSIGOPLAN.cc &&
                                                    x.numero == requisicionSIGOPLAN.numero &&
                                                    x.insumo == part.insumo
                                                ).ToList();

                                            var cantidadSurtidoDetalle = listSurtidoDetalleSIGOPLAN.Select(x => x.cantidad).Sum();

                                            if (cantidadSurtidoDetalle > part.cantidadConfirmada)
                                            {
                                                var faltante = cantidadSurtidoDetalle - part.cantidadConfirmada;

                                                foreach (var surtDet in listSurtidoDetalleSIGOPLAN)
                                                {
                                                    if (faltante > 0)
                                                    {
                                                        var resultado = surtDet.cantidad - faltante;

                                                        if (resultado <= 0)
                                                        {
                                                            surtDet.cantidad = 0;
                                                            surtDet.estatus = false;

                                                            _context.Entry(surtDet).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            surtDet.cantidad = resultado;

                                                            _context.Entry(surtDet).State = System.Data.Entity.EntityState.Modified;
                                                            _context.SaveChanges();
                                                        }

                                                        faltante -= surtDet.cantidad;
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        requisicionSIGOPLAN.validadoRequisitor = true;

                                        _context.Entry(requisicionSIGOPLAN).State = System.Data.Entity.EntityState.Modified;
                                        _context.SaveChanges();

                                        trans.Commit();
                                        dbSigoplanTransaction.Commit();
                                    }
                                    catch (Exception)
                                    {
                                        trans.Rollback();
                                        dbSigoplanTransaction.Rollback();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        requisicionSIGOPLAN.validadoRequisitor = true;

                        _context.Entry(requisicionSIGOPLAN).State = System.Data.Entity.EntityState.Modified;
                        _context.SaveChanges();
                    }

                    result.Add(SUCCESS, true);
                }
                else
                {
                    result.Add(MESSAGE, "No se encuentra la requisición.");
                    result.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return result;
        }

        public ValuacionDTO getInformacionInsumo(FiltrosExistenciaInsumoDTO filtros)
        {
            ValuacionDTO resultado = new ValuacionDTO();

            try
            {
                var existenciasEK = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        det.insumo, 
                                        i.tipo, 
                                        i.grupo, 
                                        mov.almacen, 
                                        area_alm, 
                                        lado_alm, 
                                        estante_alm, 
                                        nivel_alm, 
                                        i.descripcion AS descInsumo, 
                                        ( 
                                           SELECT 
                                              TOP 1 g.descripcion 
                                           FROM 
                                              grupos_insumo g 
                                           WHERE g.grupo_insumo = i.grupo AND g.tipo_insumo = i.tipo 
                                        ) 
                                        AS descripcion, 
                                        ( 
                                           SELECT 
                                              TOP 1 Replace(Replace(n.descripcion, 'ALMACEN ', ''), 'DE ', '') 
                                           FROM si_almacen n 
                                           WHERE n.almacen = mov.almacen 
                                        ) 
                                        AS nomAlmacen, 
                                        SUM( 
                                            CASE 
                                                WHEN mov.tipo_mov < 50 THEN det.cantidad 
                                                ELSE det.cantidad * -1 
                                            END 
                                        ) AS cantidad 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                        INNER JOIN insumos i ON det.insumo = i.insumo 
                                    WHERE mov.almacen = {0} AND det.insumo = {1} {2} {3} {4} {5} 
                                    GROUP BY det.insumo, i.tipo, i.grupo, descripcion, mov.almacen, descInsumo, area_alm, lado_alm, estante_alm, nivel_alm",
                                    filtros.almacen,
                    //filtros.cc,
                                    filtros.insumo,
                                    (filtros.area_alm != "" && filtros.area_alm != null) ? "AND area_alm = '" + filtros.area_alm + "'" : "",
                                    (filtros.lado_alm != "" && filtros.lado_alm != null) ? "AND lado_alm = '" + filtros.lado_alm + "'" : "",
                                    (filtros.estante_alm != "" && filtros.estante_alm != null) ? "AND estante_alm = '" + filtros.estante_alm + "'" : "",
                                    (filtros.nivel_alm != "" && filtros.nivel_alm != null) ? "AND nivel_alm = '" + filtros.nivel_alm + "'" : ""));

                if (existenciasEK != null)
                {
                    var existencias = (List<ValuacionDTO>)existenciasEK.ToObject<List<ValuacionDTO>>();
                    var stockMinimo = _context.tblAlm_StockMinimo.FirstOrDefault(x => x.estatus && x.almacenID == filtros.almacen && x.insumo == filtros.insumo);
                    var listSolicitadoPendiente = _context.tblAlm_Traspaso.Where(x =>
                            x.estatusRegistro &&
                            x.ccOrigen == filtros.cc &&
                            x.almacenOrigen == filtros.almacen &&
                            !x.autorizado &&
                            x.insumo == filtros.insumo).ToList();
                    var solicitadoPendiente = listSolicitadoPendiente.Count > 0 ? listSolicitadoPendiente.Select(z => z.cantidadTraspasar != null ? z.cantidadTraspasar : 0).Sum() : 0;

                    resultado = new ValuacionDTO
                    {
                        insumo = existencias[0].insumo,
                        descInsumo = existencias[0].descInsumo,
                        cantidad = existencias.Sum(x => x.cantidad),
                        minimo = stockMinimo != null ? stockMinimo.stockMinimo : "",
                        solicitadoPendiente = solicitadoPendiente
                    };
                }
            }
            catch (Exception)
            {
            }

            return resultado;
        }

        public dynamic getInsumosAutoComplete(string term)
        {
            try
            {
                var res = (List<dynamic>)consultaCheckProductivo(
                    string.Format(@"SELECT TOP 12 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad 
                                    FROM insumos i 
                                    WHERE i.insumo LIKE '{0}%' 
                                    ORDER BY i.insumo", term)).ToObject<List<dynamic>>();
                return res.Select(x => new
                {
                    id = (string)x.descripcion,
                    value = (string)x.insumo,
                    unidad = (string)x.unidad
                }).ToList();
            }
            catch (Exception) { return 0; }
        }

        public dynamic getInsumosDescAutoComplete(string term)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERÚ
                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                return _starsoft.MAEART.ToList().Where(x => !string.IsNullOrEmpty(x.ADESCRI)).ToList().Where(x => x.ADESCRI.Contains(term.Trim().ToUpper())).ToList().Take(12).Select(x => new
                                {
                                    id = Int32.Parse(x.ACODIGO),
                                    value = x.ADESCRI,
                                    unidad = x.AUNIDAD,
                                }).ToList();
                            }
                            #endregion
                        }
                    default:
                        {
                            #region DEMÁS EMPRESAS
                            var res = (List<dynamic>)consultaCheckProductivo(string.Format(@"
                                SELECT TOP 12 
                                    i.insumo, 
                                    i.descripcion, 
                                    i.unidad, 
                                    i.cancelado 
                                FROM {1}insumos i 
                                WHERE i.descripcion LIKE '%{0}%'
                                ORDER BY i.insumo", term, (EmpresaEnum)vSesiones.sesionEmpresaActual != EmpresaEnum.Colombia ? "" : "DBA."
                            )).ToObject<List<dynamic>>();

                            return res.Select(x => new
                            {
                                id = (string)x.insumo,
                                value = (string)x.descripcion + ((string)x.cancelado == "C" ? " (INSUMO CANCELADO)" : ""),
                                unidad = (string)x.unidad
                            }).ToList();
                            #endregion
                        }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool checarUbicacionesValidas(List<SurtidoDetDTO> entradas)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    break;
                default:
                    foreach (var ent in entradas)
                    {
                        foreach (var ubi in ent.listUbicacionMovimiento)
                        {
                            var checkUbicacionValida = consultaCheckProductivo(
                                string.Format(@"SELECT 
                                            * 
                                        FROM si_matriz_ubicaciones 
                                        WHERE 
                                            almacen = {0} AND 
                                            area_alm = '{1}' AND 
                                            lado_alm = '{2}' AND 
                                            estante_alm = '{3}' AND 
                                            nivel_alm = '{4}'", ent.almacenDestinoID, ubi.area_alm, ubi.lado_alm, ubi.estante_alm, ubi.nivel_alm)
                            );

                            if (checkUbicacionValida == null)
                            {
                                throw new Exception("Debe capturar ubicaciones válidas para el almacén.");
                            }
                        }
                    }
                    break;
            }

            return true;
        }
        public dynamic getInsumoInformacion(int insumo, bool esServicio = false)
        {
            try
            {
                using (var ctx = new MainContext())
                {
                    var insumoEK = (
                        from i in ctx.tblAlm_Insumo
                        where i.insumo == insumo // insumo es el parámetro que pasas
                        select new
                        {
                            i.insumo,
                            i.descripcion,
                            i.unidad,
                            i.bit_area_cta,
                            costoPromedio = ctx.tblCom_OrdenCompraDet
                                                .Where(det => det.insumo == i.insumo)
                                                .Average(det => (decimal?)det.precio) ?? 0, // Manejo de valores nulos
                            i.cancelado,
                            i.color_resguardo,
                            i.compras_req
                        }
                    ).FirstOrDefault(); // Obtener el primer resultado o nulo si no existe

                    if (insumoEK != null)
                    {
                        return new
                        {
                            id = insumoEK.descripcion,
                            value = insumoEK.insumo,
                            unidad = insumoEK.unidad,
                            // exceso = null, // No se encuentra equivalente directo para 'cant_requerida' en el código original
                            isAreaCueta = insumoEK.bit_area_cta, // Manejo de posibles nulos
                            cancelado = insumoEK.cancelado,
                            costoPromedio = insumoEK.costoPromedio.ToString("F2"), // Convertir a string con formato si es necesario
                            color_resguardo = insumoEK.color_resguardo != null ? (int)insumoEK.color_resguardo : 0,
                            compras_req = (int?)insumoEK.compras_req
                        };
                    }

                    return null; // En caso de que no se encuentre el insumo
                }

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public dynamic getInsumoInformacion_old(int insumo, bool esServicio = false)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            #region PERU
                            var data = new InsumoRequisicionDTO();

                            using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                            {
                                var registroInsumo = _starsoft.MAEART.ToList().FirstOrDefault(x => x.ACODIGO == insumo.ToString("D11") && (esServicio ? x.ACODIGO.Substring(0, 2) == "02" : x.ACODIGO.Substring(0, 2) == "01"));

                                if (registroInsumo != null)
                                {
                                    data.id = registroInsumo.ADESCRI;
                                    data.value = registroInsumo.ACODIGO;
                                    data.unidad = registroInsumo.AUNIDAD;
                                    data.exceso = 0;
                                    data.isAreaCueta = false;
                                    data.cancelado = "A";
                                    data.costoPromedio = "0";
                                    data.color_resguardo = 0;
                                    data.compras_req = 1;
                                    data.ultimaCompra = 0;
                                }
                            }

                            return data;
                            #endregion
                        }
                    case EmpresaEnum.Colombia:
                        {
                            #region COLOMBIA
                            var insumoColombiaENKONTROL = ((List<dynamic>)consultaCheckProductivo(string.Format(@"
                            SELECT 
                                i.insumo, 
                                i.descripcion, 
                                i.unidad, 
                                ( 
                                    ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 ) 
                                ) AS costo_promedio
                            FROM DBA.insumos i 
                            WHERE i.insumo = {0} ", insumo)
                            ).ToObject<List<dynamic>>())[0];

                            return new
                            {
                                id = (string)insumoColombiaENKONTROL.descripcion,
                                value = (string)insumoColombiaENKONTROL.insumo,
                                unidad = (string)insumoColombiaENKONTROL.unidad,
                                isAreaCueta = true,
                                cancelado = "A",
                                costoPromedio = (string)insumoColombiaENKONTROL.costo_promedio,
                                color_resguardo = 0,
                                compras_req = 1
                            };
                            #endregion
                        }
                    default:
                        {
                            #region RESTO DE EMPRESAS
                            var insumoEK = consultaCheckProductivo(
                            string.Format(@"SELECT 
                                            i.insumo, 
                                            i.descripcion, 
                                            i.unidad, 
                                            i.bit_area_cta, 
                                            ( 
                                                ISNULL ( (SELECT AVG(precio) FROM so_orden_compra_det as det WHERE det.insumo = i.insumo) , 0 ) 
                                            ) AS costo_promedio, 
                                            i.cancelado, 
                                            i.color_resguardo, 
                                            i.compras_req 
                                        FROM insumos i 
                                        WHERE i.insumo = {0} ", insumo)
                            );

                            var insumoENKONTROL = ((List<dynamic>)insumoEK.ToObject<List<dynamic>>())[0];

                            return new
                            {
                                id = (string)insumoENKONTROL.descripcion,
                                value = (string)insumoENKONTROL.insumo,
                                unidad = (string)insumoENKONTROL.unidad,
                                //exceso = (decimal?)insumoENKONTROL.cant_requerida,
                                isAreaCueta = (bool)insumoENKONTROL.bit_area_cta,
                                cancelado = (string)insumoENKONTROL.cancelado,
                                costoPromedio = (string)insumoENKONTROL.costo_promedio,
                                color_resguardo = insumoENKONTROL.color_resguardo != null ? (int)insumoENKONTROL.color_resguardo : 0,
                                compras_req = (int?)insumoENKONTROL.compras_req
                            };
                            #endregion
                        }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public dynamic getInsumoInformacionByAlmacen(int insumo, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
                {
                    #region COLOMBIA
                    var insumoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        --i.bit_area_cta, 
                                        ( 
                                            ISNULL ( (select ROUND(AVG(det.precio),1) FROM DBA.si_movimientos_det det WHERE det.almacen={0} AND det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado
                                        --i.color_resguardo, 
                                        --i.compras_req 
                                    FROM DBA.insumos i 
                                    WHERE i.insumo = {1}", almacen, insumo)
                    );

                    var insumoENKONTROL = ((IEnumerable<dynamic>)insumoEK.ToObject<IEnumerable<dynamic>>()).FirstOrDefault();
                    var costoPromedioKardex = getCostoPromedioKardex(almacen, insumo);

                    return new
                    {
                        id = (string)insumoENKONTROL.descripcion,
                        value = (string)insumoENKONTROL.insumo,
                        unidad = (string)insumoENKONTROL.unidad,
                        isAreaCueta = false,
                        cancelado = (string)insumoENKONTROL.cancelado,
                        costoPromedio = costoPromedioKardex, //costoPromedio = (string)insumoENKONTROL.costo_promedio,
                        color_resguardo = insumoENKONTROL.color_resguardo != null ? (int)insumoENKONTROL.color_resguardo : 0,
                        compras_req = (int?)insumoENKONTROL.compras_req
                    };
                    #endregion
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    #region PERU
                    var data = new InsumoRequisicionDTO();

                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var registroInsumo = _starsoft.MAEART.ToList().FirstOrDefault(x => x.ACODIGO == insumo.ToString("D11"));

                        if (registroInsumo != null)
                        {
                            data.id = registroInsumo.ADESCRI;
                            data.value = registroInsumo.ACODIGO;
                            data.unidad = registroInsumo.AUNIDAD;
                            data.exceso = 0;
                            data.isAreaCueta = false;
                            data.cancelado = "A";
                            data.costoPromedio = "0";
                            data.color_resguardo = 0;
                            data.compras_req = 1;
                            data.ultimaCompra = 0;

                            if (almacen < 90)
                            {
                                var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == data.value);

                                if (registroStock != null)
                                {
                                    data.costoPromedio = registroStock.STKPREPRO.ToString();
                                }
                            }
                            else
                            {
                                var existencias = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == almacen && x.insumo == insumo).ToList().GroupBy(x => new { x.almacen, x.insumo }).Select(x => new
                                {
                                    almacen = x.Key.almacen,
                                    insumo = x.Key.insumo,
                                    cantidad = x.Sum(y => y.tipo_mov < 50 ? y.cantidad : (y.cantidad * -1))
                                }).FirstOrDefault();

                                if (existencias != null)
                                {
                                    if (existencias.cantidad > 0)
                                    {
                                        data.costoPromedio = _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.almacen == almacen && x.insumo == insumo).ToList().Average(x => x.precio).ToString();
                                    }
                                    else
                                    {
                                        data.costoPromedio = "0";
                                    }
                                }
                                else
                                {
                                    data.costoPromedio = "0";
                                }
                            }
                        }
                    }

                    return data;
                    #endregion
                }
                else
                {
                    #region DEMAS EMPRESAS
                    var insumoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        ( 
                                            ISNULL ( (select ROUND(AVG(det.precio),1) from si_movimientos_det det where det.almacen={0} and det.insumo=i.insumo) , 0 )
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.insumo = {1}", almacen, insumo)
                    );

                    var insumoENKONTROL = ((IEnumerable<dynamic>)insumoEK.ToObject<IEnumerable<dynamic>>()).FirstOrDefault();

                    //                #region Checar existencia para el reseteo del precio y costo promedio
                    //                decimal precioEntrada = 0;
                    //                decimal costoPromedioEntrada = 0;
                    //                decimal existencias = 0;

                    //                var movimientosEK = consultaCheckProductivo(
                    //                    string.Format(@"SELECT 
                    //                                        mov.almacen, mov.tipo_mov, mov.numero, mov.fecha, det.partida, det.insumo, det.cantidad, det.precio, det.importe, det.costo_prom 
                    //                                    FROM si_movimientos mov 
                    //                                        INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                    //                                    WHERE mov.almacen = {0} AND det.insumo = {1} 
                    //                                    ORDER BY mov.fecha DESC", almacen, insumo)
                    //                );

                    //                if (movimientosEK != null)
                    //                {
                    //                    var movimientos = (List<dynamic>)movimientosEK.ToObject<List<dynamic>>();
                    //                    var entradas = movimientos.Where(x => (int)x.tipo_mov < 50).ToList();
                    //                    var ultimaEntrada = entradas.OrderByDescending(x => (DateTime)x.fecha).First();
                    //                    entradas.RemoveAt(0); //Quitar la ultima entrada para calcular las existencias antes de que se realizara ese movimiento.
                    //                    decimal cantidadEntradas = 0;
                    //                    foreach (var ent in entradas)
                    //                    {
                    //                        cantidadEntradas += Convert.ToDecimal(ent.cantidad, CultureInfo.InvariantCulture);
                    //                    }
                    //                    var salidas = movimientos.Where(x => (int)x.tipo_mov > 50).ToList();
                    //                    decimal cantidadSalidas = 0;
                    //                    foreach (var sal in salidas)
                    //                    {
                    //                        cantidadSalidas += Convert.ToDecimal(sal.cantidad, CultureInfo.InvariantCulture);
                    //                    }
                    //                    existencias = cantidadEntradas - cantidadSalidas;

                    //                    if (existencias <= 0)
                    //                    {
                    //                        precioEntrada = Convert.ToDecimal(ultimaEntrada.precio, CultureInfo.InvariantCulture);
                    //                        costoPromedioEntrada = Convert.ToDecimal(ultimaEntrada.precio, CultureInfo.InvariantCulture);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    throw new Exception("No existen movimientos para el insumo \"" + insumo + "\" en el almacén \"" + almacen + "\".");
                    //                }
                    //                #endregion

                    //var costoPromedioNormal = getCostoPromedioNuevo(almacen, insumo);
                    //var costoPromedio = existencias > 0 ? costoPromedioNormal : costoPromedioEntrada;
                    var costoPromedioKardex = getCostoPromedioKardex(almacen, insumo);

                    return new
                    {
                        id = (string)insumoENKONTROL.descripcion,
                        value = (string)insumoENKONTROL.insumo,
                        unidad = (string)insumoENKONTROL.unidad,
                        //exceso = (decimal?)insumoENKONTROL.cant_requerida,
                        isAreaCueta = (bool)insumoENKONTROL.bit_area_cta,
                        cancelado = (string)insumoENKONTROL.cancelado,
                        costoPromedio = costoPromedioKardex, //costoPromedio = (string)insumoENKONTROL.costo_promedio,
                        color_resguardo = insumoENKONTROL.color_resguardo != null ? (int)insumoENKONTROL.color_resguardo : 0,
                        compras_req = (int?)insumoENKONTROL.compras_req
                    };
                    #endregion
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public dynamic getInsumoInformacionByAlmacenEntrada(int insumo, int almacen)
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
                {
                    var data = new InsumoRequisicionDTO();

                    using (var _starsoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var registroInsumo = _starsoft.MAEART.ToList().FirstOrDefault(x => x.ACODIGO == insumo.ToString("D11"));

                        if (registroInsumo != null)
                        {
                            data.id = registroInsumo.ADESCRI;
                            data.value = registroInsumo.ACODIGO;
                            data.unidad = registroInsumo.AUNIDAD;
                            data.exceso = 0;
                            data.isAreaCueta = false;
                            data.cancelado = "A";
                            data.costoPromedio = "0";
                            data.color_resguardo = 0;
                            data.compras_req = 1;
                            data.ultimaCompra = 0;

                            //Se cambió la lógica para traer el último precio de entrada.
                            //var registroStock = _starsoft.STKART.ToList().FirstOrDefault(x => Int32.Parse(x.STALMA) == almacen && x.STCODIGO == data.value);

                            //if (registroStock != null)
                            //{
                            //    data.costoPromedio = registroStock.STKPREPRO.ToString();
                            //}

                            var listaEntradas = _starsoft.MOVALMCAB.Where(x => x.CACODMOV == "CL").ToList().Where(x => Int32.Parse(x.CAALMA) == almacen).Join(
                                _starsoft.MovAlmDet,
                                g => new { almacen = g.CAALMA, td = g.CATD, numdoc = g.CANUMDOC },
                                d => new { almacen = d.DEALMA, td = d.DETD, numdoc = d.DENUMDOC },
                                (g, d) => new { g, d }
                            ).ToList().Where(x => Int32.Parse(x.d.DECODIGO) == insumo && x.d.DEPRECIO > 0).ToList();

                            if (listaEntradas.Count() > 0)
                            {
                                var ultimaEntrada = listaEntradas.OrderByDescending(x => x.g.CAFECDOC).FirstOrDefault();

                                data.costoPromedio = ultimaEntrada.d.DEPRECIO.ToString();
                            }
                            else
                            {
                                data.costoPromedio = "0";
                            }
                        }
                    }

                    return data;
                }
                else if ((MainContextEnum)vSesiones.sesionEmpresaActual == MainContextEnum.Colombia)
                {
                    #region COLOMBIA
                    var insumoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {0} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado
                                    FROM insumos i 
                                    WHERE i.insumo = {1}", almacen, insumo)
                    );

                    var insumoENKONTROL = ((IEnumerable<dynamic>)insumoEK.ToObject<IEnumerable<dynamic>>()).FirstOrDefault();

                    var costoPromedioKardex = getCostoPromedioKardex(almacen, insumo);

                    return new
                    {
                        id = (string)insumoENKONTROL.descripcion,
                        value = (string)insumoENKONTROL.insumo,
                        unidad = (string)insumoENKONTROL.unidad,
                        isAreaCueta = false,
                        cancelado = (string)insumoENKONTROL.cancelado,
                        costoPromedio = (string)insumoENKONTROL.costo_promedio, //Precio de la última entrada
                        color_resguardo = 0,
                        compras_req = 0
                    };
                    #endregion
                }
                else
                {
                    #region RESTO EMPRESAS
                    var insumoEK = consultaCheckProductivo(
                        string.Format(@"SELECT 
                                        i.insumo, 
                                        i.descripcion, 
                                        i.unidad, 
                                        i.bit_area_cta, 
                                        ( 
                                            ISNULL (
                                                (
                                                    SELECT TOP 1
	                                                    det.precio 
                                                    FROM si_movimientos mov 
	                                                    INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero
                                                    WHERE mov.almacen = {0} AND det.insumo = i.insumo AND mov.tipo_mov < 50
                                                    ORDER BY mov.fecha DESC
                                                )
                                            , 0)
                                        ) AS costo_promedio, 
                                        i.cancelado, 
                                        i.color_resguardo, 
                                        i.compras_req 
                                    FROM insumos i 
                                    WHERE i.insumo = {1}", almacen, insumo)
                    );

                    var insumoENKONTROL = ((IEnumerable<dynamic>)insumoEK.ToObject<IEnumerable<dynamic>>()).FirstOrDefault();

                    var costoPromedioKardex = getCostoPromedioKardex(almacen, insumo);

                    return new
                    {
                        id = (string)insumoENKONTROL.descripcion,
                        value = (string)insumoENKONTROL.insumo,
                        unidad = (string)insumoENKONTROL.unidad,
                        isAreaCueta = (bool)insumoENKONTROL.bit_area_cta,
                        cancelado = (string)insumoENKONTROL.cancelado,
                        costoPromedio = (string)insumoENKONTROL.costo_promedio, //Precio de la última entrada
                        color_resguardo = insumoENKONTROL.color_resguardo != null ? (int)insumoENKONTROL.color_resguardo : 0,
                        compras_req = (int?)insumoENKONTROL.compras_req
                    };
                    #endregion
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<SurtidoDTO> getSurtidoDetalle(string cc, int numero)
        {
            List<SurtidoDTO> resultado = new List<SurtidoDTO>();

            //var surtido = _context.tblCom_Surtido.FirstOrDefault(x => x.estatus && x.cc == cc && x.numero == numero);

            //if (surtido != null)
            //{
            var surtidoDetalle = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.cc == cc && x.numero == numero).ToList();

            foreach (var surtDet in surtidoDetalle)
            {
                var insumoEK = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM insumos WHERE insumo = {0}", surtDet.insumo)).ToObject<List<dynamic>>())[0];
                var almacenOrigenEK = ((List<dynamic>)consultaCheckProductivo(
                    string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", surtDet.almacenOrigenID)
                ).ToObject<List<dynamic>>())[0];
                var almacenDestinoEK = ((List<dynamic>)consultaCheckProductivo(
                    string.Format(@"SELECT * FROM si_almacen WHERE almacen = {0}", surtDet.almacenDestinoID)
                ).ToObject<List<dynamic>>())[0];
                var tipoSurtido = "";

                switch (surtDet.tipoSurtidoDetalle)
                {
                    case "AP":
                        tipoSurtido = "Almacén Propio";
                        break;
                    case "AE":
                        tipoSurtido = "Almacén Externo";
                        break;
                    default:
                        break;
                }

                resultado.Add(new SurtidoDTO
                {
                    insumo = surtDet.insumo,
                    insumoDesc = surtDet.insumo + " - " + (string)insumoEK.descripcion,
                    cantidad = surtDet.cantidad,
                    almacenOrigen = surtDet.almacenOrigenID,
                    almacenOrigenDesc = surtDet.almacenOrigenID + " - " + (string)almacenOrigenEK.descripcion,
                    almacenDestino = surtDet.almacenDestinoID,
                    almacenDestinoDesc = surtDet.almacenDestinoID + " - " + (string)almacenDestinoEK.descripcion,
                    tipoSurtido = tipoSurtido
                });
            }

            return resultado;
            //}
            //else
            //{
            //    return new List<SurtidoDTO>();
            //}
        }

        public void BorrarRequisicionesMasivo()
        {
            var listaRequisiciones = new List<Tuple<string, int, string>>
            {
                new Tuple<string, int, string>("", 0, "")
            };

            foreach (var req in listaRequisiciones)
            {
                try
                {
                    borrarRequisicion(req.Item1, req.Item2, req.Item3);
                }
                catch (Exception e)
                {
                    LogError(0, 0, "RequisicionController", "BorrarRequisicionesMasivo", e, AccionEnum.ELIMINAR, 0, null);
                }
            }
        }

        public void borrarRequisicion(string cc, int numero, string tpRequi)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {

                case EmpresaEnum.Peru:
                    {
                        #region PERU
                        using (var ctxStarsoft = new MainContextPeruStarSoft003BDCOMUN())
                        {
                            using (var ctxPeru = new MainContext())
                            {
                                using (var transaccionPeru = ctxPeru.Database.BeginTransaction())
                                {
                                    using (var transaccionStarsoft = ctxStarsoft.Database.BeginTransaction())
                                    {
                                        try
                                        {
                                            #region Validación Compra Existente
                                            var registroCompraExistente = ctxPeru.tblCom_OrdenCompra.Where(x => x.estatusRegistro && x.cc == cc && x.PERU_tipoCompra == tpRequi && x.estatus != "C").ToList().Join(
                                                ctxPeru.tblCom_OrdenCompraDet.Where(x => x.estatusRegistro && x.cc == cc && x.num_requisicion == numero).ToList(),
                                                g => g.id,
                                                d => d.idOrdenCompra,
                                                (g, d) => new { g, d }
                                            ).ToList();

                                            if (registroCompraExistente.Count() > 0)
                                            {
                                                throw new Exception("No se puede eliminar una requisición con compras existentes.");
                                            }
                                            #endregion

                                            var empleado = ctxPeru.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                                            if (empleado != null)
                                            {
                                                var numRequi = numero.ToString();
                                                numRequi = numRequi.PadLeft(10, '0');
                                                var requiStarsoft = ctxStarsoft.REQUISC.FirstOrDefault(x => x.NROREQUI == numRequi && x.TIPOREQUI == tpRequi);
                                                if (requiStarsoft != null)
                                                {
                                                    if (requiStarsoft.ESTREQUI == "P")
                                                    {
                                                        var requiStarsoftDet = ctxStarsoft.REQUISD.Where(x => x.NROREQUI == numRequi && x.TIPOREQUI == tpRequi).ToList();
                                                        ctxStarsoft.REQUISD.RemoveRange(requiStarsoftDet);
                                                        ctxStarsoft.SaveChanges();

                                                        ctxStarsoft.REQUISC.Remove(requiStarsoft);
                                                        ctxStarsoft.SaveChanges();
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("No se puede eliminar una requisición utilizada en una orden de compra");
                                                    }
                                                }

                                                var requiSigoplan = ctxPeru.tblCom_Req.FirstOrDefault(x => x.numero == numero && x.cc == cc && x.estatusRegistro && x.PERU_tipoRequisicion == tpRequi);
                                                if (requiSigoplan != null)
                                                {
                                                    var requiSigoplanDet = ctxPeru.tblCom_ReqDet.Where(x => x.idReq == requiSigoplan.id && x.estatusRegistro).ToList();
                                                    foreach (var item in requiSigoplanDet)
                                                    {
                                                        item.estatusRegistro = false;
                                                    }
                                                    ctxPeru.SaveChanges();

                                                    requiSigoplan.estatusRegistro = false;
                                                    requiSigoplan.empleadoUltimaAccion = empleado.empleado;
                                                    requiSigoplan.fechaUltimaAccion = DateTime.Now;
                                                    requiSigoplan.tipoUltimaAccion = TipoUltimaAccionEnum.Eliminacion;
                                                    ctxPeru.SaveChanges();
                                                }

                                                transaccionStarsoft.Commit();
                                                transaccionPeru.Commit();
                                            }
                                            else
                                            {
                                                throw new Exception("No se encontró información del empleado que solicita eliminar la requisición");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            transaccionPeru.Rollback();
                                            transaccionStarsoft.Rollback();
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    break;
                case EmpresaEnum.Colombia:
                    {
                        #region COLOMBIA
                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            using (var con = checkConexionProductivo())
                            {
                                using (var trans = con.BeginTransaction())
                                {
                                    try
                                    {
                                        #region Validación compras existentes no canceladas
                                        var listaComprasDetEK = consultaCheckProductivo(
                                            string.Format(@"SELECT 
                                                    det.cc, det.numero, oc.estatus 
                                                FROM DBA.so_orden_compra_det det
                                                    INNER JOIN DBA.so_orden_compra oc ON det.cc = oc.cc AND det.numero = oc.numero 
                                                WHERE det.cc = '{0}' AND det.num_requisicion = {1} AND (det.cantidad > det.cant_canc) GROUP BY det.cc, det.numero, oc.estatus", cc, numero)
                                        );

                                        if (listaComprasDetEK != null)
                                        {
                                            var listaComprasDet = (List<dynamic>)listaComprasDetEK.ToObject<List<dynamic>>();
                                            List<dynamic> listaComprasDetFiltrada = new List<dynamic>();

                                            foreach (var compraDet in listaComprasDet)
                                            {
                                                var estatusCompra = compraDet.estatus != null ? (string)compraDet.estatus : "";

                                                if (estatusCompra != "C")
                                                {
                                                    listaComprasDetFiltrada.Add(compraDet);
                                                }
                                            }

                                            if (listaComprasDetFiltrada.Count() > 0)
                                            {
                                                var stringCompras = string.Join(", ", listaComprasDetFiltrada.Select(x => (string)x.cc + "-" + (int)x.numero).ToList());

                                                throw new Exception("No se puede borrar la requisición ya que cuenta con compras existentes no canceladas. Compra(s): " + stringCompras);
                                            }
                                        }
                                        #endregion

                                        var usuario = vSesiones.sesionUsuarioDTO;
                                        var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                                        //if (relUser.empleado != 1)
                                        //{
                                        //    #region Validación Periodo Contable
                                        //    var periodoContable = getPeriodoContableRequisicion(cc, numero);

                                        //    if (periodoContable != null)
                                        //    {
                                        //        if ((string)periodoContable.periodoContable.soc != "N")
                                        //        {
                                        //            throw new Exception("Periodo Contable Cerrado.");
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        throw new Exception("Periodo Contable Cerrado.");
                                        //    }
                                        //    #endregion
                                        //}

                                        #region SIGOPLAN
                                        var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.estatusRegistro && x.cc == cc && x.numero == numero);

                                        if (requisicionSIGOPLAN != null)
                                        {
                                            var detalleRequisicionSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.estatusRegistro && x.idReq == requisicionSIGOPLAN.id).ToList();

                                            foreach (var det in detalleRequisicionSIGOPLAN)
                                            {
                                                det.estatusRegistro = false;

                                                _context.Entry(det).State = System.Data.Entity.EntityState.Modified;
                                                _context.SaveChanges();
                                            }

                                            requisicionSIGOPLAN.estatusRegistro = false;
                                            requisicionSIGOPLAN.empleadoUltimaAccion = relUser.empleado;
                                            requisicionSIGOPLAN.fechaUltimaAccion = DateTime.Now;
                                            requisicionSIGOPLAN.tipoUltimaAccion = TipoUltimaAccionEnum.Eliminacion;

                                            _context.Entry(requisicionSIGOPLAN).State = System.Data.Entity.EntityState.Modified;
                                            _context.SaveChanges();
                                        }
                                        #endregion

                                        #region Cancelar links pendientes de proveedores.
                                        var listaLinks = _context.tblCom_ProveedoresLinks.Where(x => x.registroActivo && x.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && x.cc == cc && x.numRequisicion == numero).ToList();

                                        foreach (var link in listaLinks)
                                        {
                                            link.idEstatusRegistro = EstatusRegistroProveedorLinkEnum.CANCELADO;
                                            _context.SaveChanges();
                                        }
                                        #endregion

                                        #region Enkontrol
                                        var count = 0;

                                        #region Borrar Detalle Linea
                                        var consultaEliminarRequisicionDetalleLinea = @"DELETE FROM DBA.so_req_det_linea WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicionDetalleLinea))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region Actualizar Explosión
                                        var detalleRequisicionEK = consultaCheckProductivo(string.Format(@"SELECT * FROM DBA.so_requisicion_det WHERE cc = '{0}' AND numero = {1}", cc, numero));

                                        if (detalleRequisicionEK != null)
                                        {
                                            var detalleRequisicion = (List<dynamic>)detalleRequisicionEK.ToObject<List<dynamic>>();

                                            foreach (var det in detalleRequisicion)
                                            {
                                                var registroExplosionEK = consultaCheckProductivo(
                                                    string.Format(@"SELECT * FROM DBA.so_explos_mat WHERE cc = '{0}' AND insumo = {1} AND year_explos = {2}", cc, (int)det.insumo, DateTime.Now.Year)
                                                );

                                                if (registroExplosionEK != null)
                                                {
                                                    var registroExplosion = ((List<dynamic>)registroExplosionEK.ToObject<List<dynamic>>())[0];

                                                    var nuevaCantidadRequerida =
                                                        Convert.ToDecimal(registroExplosion.cant_requerida, CultureInfo.InvariantCulture) - Convert.ToDecimal(det.cantidad, CultureInfo.InvariantCulture);

                                                    var consultaExplosionUpdate = @"UPDATE DBA.so_explos_mat SET cant_requerida = ? WHERE cc = ? AND insumo = ? AND year_explos = ?";

                                                    using (var cmd = new OdbcCommand(consultaExplosionUpdate))
                                                    {
                                                        OdbcParameterCollection parametersExplosion = cmd.Parameters;

                                                        parametersExplosion.Add("@cant_requerida", OdbcType.Numeric).Value = nuevaCantidadRequerida;

                                                        parametersExplosion.Add("@cc", OdbcType.Char).Value = cc;
                                                        parametersExplosion.Add("@insumo", OdbcType.Numeric).Value = det.insumo;
                                                        parametersExplosion.Add("@year_explos", OdbcType.Numeric).Value = DateTime.Now.Year;

                                                        cmd.Connection = trans.Connection;
                                                        cmd.Transaction = trans;

                                                        count += cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Borrar Cuadros Comparativos
                                        var cuadrosEK = consultaCheckProductivo(string.Format(@"SELECT * FROM DBA.so_cuadro_comparativo WHERE cc = '{0}' AND numero = {1}", cc, numero));

                                        if (cuadrosEK != null)
                                        {
                                            var cuadros = (List<dynamic>)cuadrosEK.ToObject<List<dynamic>>();

                                            foreach (var cuadro in cuadros)
                                            {
                                                #region Borrar Tabla Detalle
                                                var consultaEliminarCuadroDetalle = @"DELETE FROM DBA.so_cuadro_comparativo_det WHERE cc = ? AND numero = ? AND folio = ?";

                                                using (var cmd = new OdbcCommand(consultaEliminarCuadroDetalle))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                                                    parameters.Add("@numero", OdbcType.Numeric).Value = numero;
                                                    parameters.Add("@folio", OdbcType.Numeric).Value = (int)cuadro.folio;

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;

                                                    count += cmd.ExecuteNonQuery();
                                                }
                                                #endregion

                                                #region Borrar Tabla General
                                                var consultaEliminarCuadro = @"DELETE FROM DBA.so_cuadro_comparativo WHERE cc = ? AND numero = ? AND folio = ?";

                                                using (var cmd = new OdbcCommand(consultaEliminarCuadro))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                                                    parameters.Add("@numero", OdbcType.Numeric).Value = numero;
                                                    parameters.Add("@folio", OdbcType.Numeric).Value = (int)cuadro.folio;

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;

                                                    count += cmd.ExecuteNonQuery();
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        #region Borrar Requisición Detalle
                                        var consultaEliminarRequisicionDetalle = @"DELETE FROM DBA.so_requisicion_det WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicionDetalle))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region Borrar Requisición
                                        var consultaEliminarRequisicion = @"DELETE FROM DBA.so_requisicion WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicion))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion
                                        #endregion

                                        if (count > 0)
                                        {
                                            trans.Commit();
                                            dbSigoplanTransaction.Commit();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        trans.Rollback();
                                        dbSigoplanTransaction.Rollback();

                                        throw new Exception(e.Message);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    break;
                default:
                    {
                        #region DEMAS EMPRESAS
                        using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
                        {
                            using (var con = checkConexionProductivo())
                            {
                                using (var trans = con.BeginTransaction())
                                {
                                    try
                                    {
                                        #region Validación compras existentes no canceladas
                                        var listaComprasDetEK = consultaCheckProductivo(
                                            string.Format(@"SELECT 
                                                    det.cc, det.numero, oc.estatus 
                                                FROM so_orden_compra_det det
                                                    INNER JOIN so_orden_compra oc ON det.cc = oc.cc AND det.numero = oc.numero 
                                                WHERE det.cc = '{0}' AND det.num_requisicion = {1} AND (det.cantidad > det.cant_canc) GROUP BY det.cc, det.numero, oc.estatus", cc, numero)
                                        );

                                        if (listaComprasDetEK != null)
                                        {
                                            var listaComprasDet = (List<dynamic>)listaComprasDetEK.ToObject<List<dynamic>>();
                                            List<dynamic> listaComprasDetFiltrada = new List<dynamic>();

                                            foreach (var compraDet in listaComprasDet)
                                            {
                                                var estatusCompra = compraDet.estatus != null ? (string)compraDet.estatus : "";

                                                if (estatusCompra != "C")
                                                {
                                                    listaComprasDetFiltrada.Add(compraDet);
                                                }
                                            }

                                            if (listaComprasDetFiltrada.Count() > 0)
                                            {
                                                var stringCompras = string.Join(", ", listaComprasDetFiltrada.Select(x => (string)x.cc + "-" + (int)x.numero).ToList());

                                                throw new Exception("No se puede borrar la requisición ya que cuenta con compras existentes no canceladas. Compra(s): " + stringCompras);
                                            }
                                        }
                                        #endregion

                                        var usuario = vSesiones.sesionUsuarioDTO;
                                        var relUser = ufs.getUsuarioService().getUserEk(usuario.id);

                                        //if (relUser.empleado != 1)
                                        //{
                                        //    #region Validación Periodo Contable
                                        //    var periodoContable = getPeriodoContableRequisicion(cc, numero);

                                        //    if (periodoContable != null)
                                        //    {
                                        //        if ((string)periodoContable.periodoContable.soc != "N")
                                        //        {
                                        //            throw new Exception("Periodo Contable Cerrado.");
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        throw new Exception("Periodo Contable Cerrado.");
                                        //    }
                                        //    #endregion
                                        //}

                                        #region SIGOPLAN
                                        var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.estatusRegistro && x.cc == cc && x.numero == numero);

                                        if (requisicionSIGOPLAN != null)
                                        {
                                            var detalleRequisicionSIGOPLAN = _context.tblCom_ReqDet.Where(x => x.estatusRegistro && x.estatusRegistro && x.idReq == requisicionSIGOPLAN.id).ToList();

                                            foreach (var det in detalleRequisicionSIGOPLAN)
                                            {
                                                det.estatusRegistro = false;

                                                _context.Entry(det).State = System.Data.Entity.EntityState.Modified;
                                                _context.SaveChanges();
                                            }

                                            requisicionSIGOPLAN.estatusRegistro = false;
                                            requisicionSIGOPLAN.empleadoUltimaAccion = relUser.empleado;
                                            requisicionSIGOPLAN.fechaUltimaAccion = DateTime.Now;
                                            requisicionSIGOPLAN.tipoUltimaAccion = TipoUltimaAccionEnum.Eliminacion;

                                            _context.Entry(requisicionSIGOPLAN).State = System.Data.Entity.EntityState.Modified;
                                            _context.SaveChanges();
                                        }
                                        #endregion

                                        #region Cancelar links pendientes de proveedores.
                                        var listaLinks = _context.tblCom_ProveedoresLinks.Where(x => x.registroActivo && x.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && x.cc == cc && x.numRequisicion == numero).ToList();

                                        foreach (var link in listaLinks)
                                        {
                                            link.idEstatusRegistro = EstatusRegistroProveedorLinkEnum.CANCELADO;
                                            _context.SaveChanges();
                                        }
                                        #endregion

                                        #region Enkontrol
                                        var count = 0;

                                        #region Borrar Detalle Linea
                                        var consultaEliminarRequisicionDetalleLinea = @"DELETE FROM so_req_det_linea WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicionDetalleLinea))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region Actualizar Explosión
                                        var detalleRequisicionEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_requisicion_det WHERE cc = '{0}' AND numero = {1}", cc, numero));

                                        if (detalleRequisicionEK != null)
                                        {
                                            var detalleRequisicion = (List<dynamic>)detalleRequisicionEK.ToObject<List<dynamic>>();

                                            foreach (var det in detalleRequisicion)
                                            {
                                                var registroExplosionEK = consultaCheckProductivo(
                                                    string.Format(@"SELECT * FROM so_explos_mat WHERE cc = '{0}' AND insumo = {1} AND year_explos = {2}", cc, (int)det.insumo, DateTime.Now.Year)
                                                );

                                                if (registroExplosionEK != null)
                                                {
                                                    var registroExplosion = ((List<dynamic>)registroExplosionEK.ToObject<List<dynamic>>())[0];

                                                    var nuevaCantidadRequerida =
                                                        Convert.ToDecimal(registroExplosion.cant_requerida, CultureInfo.InvariantCulture) - Convert.ToDecimal(det.cantidad, CultureInfo.InvariantCulture);

                                                    var consultaExplosionUpdate = @"
                                            UPDATE so_explos_mat 
                                            SET cant_requerida = ? 
                                            WHERE cc = ? AND insumo = ? AND year_explos = ?";

                                                    using (var cmd = new OdbcCommand(consultaExplosionUpdate))
                                                    {
                                                        OdbcParameterCollection parametersExplosion = cmd.Parameters;

                                                        parametersExplosion.Add("@cant_requerida", OdbcType.Numeric).Value = nuevaCantidadRequerida;

                                                        parametersExplosion.Add("@cc", OdbcType.Char).Value = cc;
                                                        parametersExplosion.Add("@insumo", OdbcType.Numeric).Value = det.insumo;
                                                        parametersExplosion.Add("@year_explos", OdbcType.Numeric).Value = DateTime.Now.Year;

                                                        cmd.Connection = trans.Connection;
                                                        cmd.Transaction = trans;

                                                        count += cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Borrar Cuadros Comparativos
                                        var cuadrosEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_cuadro_comparativo WHERE cc = '{0}' AND numero = {1}", cc, numero));

                                        if (cuadrosEK != null)
                                        {
                                            var cuadros = (List<dynamic>)cuadrosEK.ToObject<List<dynamic>>();

                                            foreach (var cuadro in cuadros)
                                            {
                                                #region Borrar Tabla Detalle
                                                var consultaEliminarCuadroDetalle = @"DELETE FROM so_cuadro_comparativo_det WHERE cc = ? AND numero = ? AND folio = ?";

                                                using (var cmd = new OdbcCommand(consultaEliminarCuadroDetalle))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                                                    parameters.Add("@numero", OdbcType.Numeric).Value = numero;
                                                    parameters.Add("@folio", OdbcType.Numeric).Value = (int)cuadro.folio;

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;

                                                    count += cmd.ExecuteNonQuery();
                                                }
                                                #endregion

                                                #region Borrar Tabla General
                                                var consultaEliminarCuadro = @"DELETE FROM so_cuadro_comparativo WHERE cc = ? AND numero = ? AND folio = ?";

                                                using (var cmd = new OdbcCommand(consultaEliminarCuadro))
                                                {
                                                    OdbcParameterCollection parameters = cmd.Parameters;

                                                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                                                    parameters.Add("@numero", OdbcType.Numeric).Value = numero;
                                                    parameters.Add("@folio", OdbcType.Numeric).Value = (int)cuadro.folio;

                                                    cmd.Connection = trans.Connection;
                                                    cmd.Transaction = trans;

                                                    count += cmd.ExecuteNonQuery();
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        #region Borrar Requisición Detalle
                                        var consultaEliminarRequisicionDetalle = @"DELETE FROM so_requisicion_det WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicionDetalle))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion

                                        #region Borrar Requisición
                                        var consultaEliminarRequisicion = @"DELETE FROM so_requisicion WHERE cc = ? AND numero = ?";

                                        using (var cmd = new OdbcCommand(consultaEliminarRequisicion))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;

                                            parameters.Add("@cc", OdbcType.Char).Value = cc;
                                            parameters.Add("@numero", OdbcType.Numeric).Value = numero;

                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;

                                            count += cmd.ExecuteNonQuery();
                                        }
                                        #endregion
                                        #endregion

                                        if (count > 0)
                                        {
                                            trans.Commit();
                                            dbSigoplanTransaction.Commit();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        trans.Rollback();
                                        dbSigoplanTransaction.Rollback();

                                        throw new Exception(e.Message);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    break;
            }
        }

        public dynamic getPeriodoContableRequisicion(string cc, int numero)
        {
            var requisicionEK = ((List<dynamic>)consultaCheckProductivo(
                string.Format(@"SELECT * FROM so_requisicion WHERE cc = '{0}' AND numero = {1}", cc, numero)
            ).ToObject<List<dynamic>>())[0];

            var mesRequisicion = ((DateTime)requisicionEK.fecha).Month;
            var anioRequisicion = ((DateTime)requisicionEK.fecha).Year;

            var periodoContableEK = consultaCheckProductivo(string.Format(@"SELECT * FROM sc_mesproc WHERE year = {0} AND mes = {1}", anioRequisicion, mesRequisicion));

            if (periodoContableEK != null)
            {
                var periodoContable = ((List<dynamic>)periodoContableEK.ToObject<List<dynamic>>())[0];

                if ((string)periodoContable.soc == "N")
                {
                    return new
                    {
                        periodoContable = periodoContable,
                        flagPeriodoAbierto = true
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool actualizarAcumula(int almacen, string cc, MovimientoDetalleEnkontrolDTO det, DbContextTransaction dbSigoplanTransaction, OdbcTransaction trans)
        {
            var anio = DateTime.Now.Year;
            var insumo = det.insumo;
            var cantidad = det.cantidad;
            var precio = det.precio;
            var importe = cantidad * precio;
            var tipoMovimiento = det.tipo_mov;

            var mes = DateTime.Now.Month;
            var columnaExistencia = "existencia_" + (tipoMovimiento < 50 ? "ent_" : "sal_");
            var columnaImporte = "importe_" + (tipoMovimiento < 50 ? "ent_" : "sal_");

            #region Localizar Columnas Afectadas
            switch (mes)
            {
                case 1:
                    columnaExistencia = string.Concat(columnaExistencia, "ene");
                    columnaImporte = string.Concat(columnaImporte, "ene");
                    break;
                case 2:
                    columnaExistencia = string.Concat(columnaExistencia, "feb");
                    columnaImporte = string.Concat(columnaImporte, "feb");
                    break;
                case 3:
                    columnaExistencia = string.Concat(columnaExistencia, "mar");
                    columnaImporte = string.Concat(columnaImporte, "mar");
                    break;
                case 4:
                    columnaExistencia = string.Concat(columnaExistencia, "abr");
                    columnaImporte = string.Concat(columnaImporte, "abr");
                    break;
                case 5:
                    columnaExistencia = string.Concat(columnaExistencia, "may");
                    columnaImporte = string.Concat(columnaImporte, "may");
                    break;
                case 6:
                    columnaExistencia = string.Concat(columnaExistencia, "jun");
                    columnaImporte = string.Concat(columnaImporte, "jun");
                    break;
                case 7:
                    columnaExistencia = string.Concat(columnaExistencia, "jul");
                    columnaImporte = string.Concat(columnaImporte, "jul");
                    break;
                case 8:
                    columnaExistencia = string.Concat(columnaExistencia, "ago");
                    columnaImporte = string.Concat(columnaImporte, "ago");
                    break;
                case 9:
                    columnaExistencia = string.Concat(columnaExistencia, "sep");
                    columnaImporte = string.Concat(columnaImporte, "sep");
                    break;
                case 10:
                    columnaExistencia = string.Concat(columnaExistencia, "oct");
                    columnaImporte = string.Concat(columnaImporte, "oct");
                    break;
                case 11:
                    columnaExistencia = string.Concat(columnaExistencia, "nov");
                    columnaImporte = string.Concat(columnaImporte, "nov");
                    break;
                case 12:
                    columnaExistencia = string.Concat(columnaExistencia, "dic");
                    columnaImporte = string.Concat(columnaImporte, "dic");
                    break;
            }
            #endregion

            var count = 0;

            #region Update Registro Acumula Almacén
            var registroAcumulaAlmacenEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_almacen WHERE almacen = {0} AND cc = '{1}' AND ano = {2} AND insumo = {3}", almacen, cc, anio, insumo)
            );

            if (registroAcumulaAlmacenEK != null)
            {
                var registroAcumulaAlmacen = ((List<AcumulaEnkontrolDTO>)registroAcumulaAlmacenEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);
                var importeAnteriorAlmacen = Convert.ToDecimal(registroAcumulaAlmacen.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaAlmacen), CultureInfo.InvariantCulture);

                var consultaUpdateAlmacen =
                    string.Format(@"UPDATE si_acumula_almacen 
                                SET {0} = ?, {1} = ? 
                                WHERE almacen = ? AND cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorAlmacen + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorAlmacen + importe;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_almacen"
                var consultaInsertAcumulaAlmacen = @"INSERT INTO si_acumula_almacen 
                                                        (almacen, cc, ano, 
                                                        existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                        existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                        existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                        existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                        existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                        existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                        existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                        existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                        existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                        existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                        existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                        existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                        existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                        insumo) 
                                                        VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaAlmacen))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@almacen", OdbcType.Numeric).Value = almacen;
                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            #region Update Registro Acumula CC
            var registroAcumulaCCEK = consultaCheckProductivo(
                string.Format(@"SELECT * FROM si_acumula_cc WHERE cc = '{0}' AND ano = {1} AND insumo = {2}", cc, anio, insumo)
            );

            if (registroAcumulaCCEK != null)
            {
                var registroAcumulaCC = ((List<AcumulaEnkontrolDTO>)registroAcumulaCCEK.ToObject<List<AcumulaEnkontrolDTO>>())[0];

                var existenciaAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaExistencia).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);
                var importeAnteriorCC = Convert.ToDecimal(registroAcumulaCC.GetType().GetProperty(columnaImporte).GetValue(registroAcumulaCC), CultureInfo.InvariantCulture);

                var consultaUpdateCC =
                    string.Format(@"UPDATE si_acumula_cc 
                                SET {0} = ?, {1} = ?, ultimo_cp = ?, fecha_cp = ? 
                                WHERE cc = ? AND ano = ? AND insumo = ?", columnaExistencia, columnaImporte);

                using (var cmd = new OdbcCommand(consultaUpdateCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add(string.Concat("@", columnaExistencia), OdbcType.Numeric).Value = existenciaAnteriorCC + cantidad;
                    parameters.Add(string.Concat("@", columnaImporte), OdbcType.Numeric).Value = importeAnteriorCC + importe;

                    if (det.costo_prom > 0)
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = det.costo_prom ?? 0;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;
                    }
                    else
                    {
                        parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = registroAcumulaCC.ultimo_cp ?? (object)DBNull.Value;
                        parameters.Add("@fecha_cp", OdbcType.Date).Value = registroAcumulaCC.fecha_cp ?? (object)DBNull.Value;
                    }

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = anio;
                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
            }
            else
            {
                #region Insert Registro en la tabla "si_acumula_cc"
                var consultaInsertAcumulaCC = @"INSERT INTO si_acumula_cc 
                                                    (cc, ano, 
                                                    existencia_ent_ini, importe_ent_ini, existencia_sal_ini, importe_sal_ini, 
                                                    existencia_ent_ene, importe_ent_ene, existencia_sal_ene, importe_sal_ene, 
                                                    existencia_ent_feb, importe_ent_feb, existencia_sal_feb, importe_sal_feb, 
                                                    existencia_ent_mar, importe_ent_mar, existencia_sal_mar, importe_sal_mar, 
                                                    existencia_ent_abr, importe_ent_abr, existencia_sal_abr, importe_sal_abr, 
                                                    existencia_ent_may, importe_ent_may, existencia_sal_may, importe_sal_may, 
                                                    existencia_ent_jun, importe_ent_jun, existencia_sal_jun, importe_sal_jun, 
                                                    existencia_ent_jul, importe_ent_jul, existencia_sal_jul, importe_sal_jul, 
                                                    existencia_ent_ago, importe_ent_ago, existencia_sal_ago, importe_sal_ago, 
                                                    existencia_ent_sep, importe_ent_sep, existencia_sal_sep, importe_sal_sep, 
                                                    existencia_ent_oct, importe_ent_oct, existencia_sal_oct, importe_sal_oct, 
                                                    existencia_ent_nov, importe_ent_nov, existencia_sal_nov, importe_sal_nov, 
                                                    existencia_ent_dic, importe_ent_dic, existencia_sal_dic, importe_sal_dic, 
                                                    insumo, ultimo_cp, fecha_cp) 
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                using (var cmd = new OdbcCommand(consultaInsertAcumulaCC))
                {
                    OdbcParameterCollection parameters = cmd.Parameters;

                    parameters.Add("@cc", OdbcType.Char).Value = cc;
                    parameters.Add("@ano", OdbcType.Numeric).Value = DateTime.Now.Year;

                    parameters.Add("@existencia_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_ent_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@existencia_sal_ini", OdbcType.Numeric).Value = 0;
                    parameters.Add("@importe_sal_ini", OdbcType.Numeric).Value = 0;

                    parameters.Add("@existencia_ent_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ene" ? cantidad : 0;
                    parameters.Add("@importe_ent_ene", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ene" ? importe : 0;
                    parameters.Add("@existencia_sal_ene", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ene" ? cantidad : 0;
                    parameters.Add("@importe_sal_ene", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ene" ? importe : 0;

                    parameters.Add("@existencia_ent_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_feb" ? cantidad : 0;
                    parameters.Add("@importe_ent_feb", OdbcType.Numeric).Value = columnaImporte == "importe_ent_feb" ? importe : 0;
                    parameters.Add("@existencia_sal_feb", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_feb" ? cantidad : 0;
                    parameters.Add("@importe_sal_feb", OdbcType.Numeric).Value = columnaImporte == "importe_sal_feb" ? importe : 0;

                    parameters.Add("@existencia_ent_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_mar" ? cantidad : 0;
                    parameters.Add("@importe_ent_mar", OdbcType.Numeric).Value = columnaImporte == "importe_ent_mar" ? importe : 0;
                    parameters.Add("@existencia_sal_mar", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_mar" ? cantidad : 0;
                    parameters.Add("@importe_sal_mar", OdbcType.Numeric).Value = columnaImporte == "importe_sal_mar" ? importe : 0;

                    parameters.Add("@existencia_ent_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_abr" ? cantidad : 0;
                    parameters.Add("@importe_ent_abr", OdbcType.Numeric).Value = columnaImporte == "importe_ent_abr" ? importe : 0;
                    parameters.Add("@existencia_sal_abr", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_abr" ? cantidad : 0;
                    parameters.Add("@importe_sal_abr", OdbcType.Numeric).Value = columnaImporte == "importe_sal_abr" ? importe : 0;

                    parameters.Add("@existencia_ent_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_may" ? cantidad : 0;
                    parameters.Add("@importe_ent_may", OdbcType.Numeric).Value = columnaImporte == "importe_ent_may" ? importe : 0;
                    parameters.Add("@existencia_sal_may", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_may" ? cantidad : 0;
                    parameters.Add("@importe_sal_may", OdbcType.Numeric).Value = columnaImporte == "importe_sal_may" ? importe : 0;

                    parameters.Add("@existencia_ent_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jun" ? cantidad : 0;
                    parameters.Add("@importe_ent_jun", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jun" ? importe : 0;
                    parameters.Add("@existencia_sal_jun", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jun" ? cantidad : 0;
                    parameters.Add("@importe_sal_jun", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jun" ? importe : 0;

                    parameters.Add("@existencia_ent_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_jul" ? cantidad : 0;
                    parameters.Add("@importe_ent_jul", OdbcType.Numeric).Value = columnaImporte == "importe_ent_jul" ? importe : 0;
                    parameters.Add("@existencia_sal_jul", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_jul" ? cantidad : 0;
                    parameters.Add("@importe_sal_jul", OdbcType.Numeric).Value = columnaImporte == "importe_sal_jul" ? importe : 0;

                    parameters.Add("@existencia_ent_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_ago" ? cantidad : 0;
                    parameters.Add("@importe_ent_ago", OdbcType.Numeric).Value = columnaImporte == "importe_ent_ago" ? importe : 0;
                    parameters.Add("@existencia_sal_ago", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_ago" ? cantidad : 0;
                    parameters.Add("@importe_sal_ago", OdbcType.Numeric).Value = columnaImporte == "importe_sal_ago" ? importe : 0;

                    parameters.Add("@existencia_ent_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_sep" ? cantidad : 0;
                    parameters.Add("@importe_ent_sep", OdbcType.Numeric).Value = columnaImporte == "importe_ent_sep" ? importe : 0;
                    parameters.Add("@existencia_sal_sep", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_sep" ? cantidad : 0;
                    parameters.Add("@importe_sal_sep", OdbcType.Numeric).Value = columnaImporte == "importe_sal_sep" ? importe : 0;

                    parameters.Add("@existencia_ent_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_oct" ? cantidad : 0;
                    parameters.Add("@importe_ent_oct", OdbcType.Numeric).Value = columnaImporte == "importe_ent_oct" ? importe : 0;
                    parameters.Add("@existencia_sal_oct", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_oct" ? cantidad : 0;
                    parameters.Add("@importe_sal_oct", OdbcType.Numeric).Value = columnaImporte == "importe_sal_oct" ? importe : 0;

                    parameters.Add("@existencia_ent_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_nov" ? cantidad : 0;
                    parameters.Add("@importe_ent_nov", OdbcType.Numeric).Value = columnaImporte == "importe_ent_nov" ? importe : 0;
                    parameters.Add("@existencia_sal_nov", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_nov" ? cantidad : 0;
                    parameters.Add("@importe_sal_nov", OdbcType.Numeric).Value = columnaImporte == "importe_sal_nov" ? importe : 0;

                    parameters.Add("@existencia_ent_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_ent_dic" ? cantidad : 0;
                    parameters.Add("@importe_ent_dic", OdbcType.Numeric).Value = columnaImporte == "importe_ent_dic" ? importe : 0;
                    parameters.Add("@existencia_sal_dic", OdbcType.Numeric).Value = columnaExistencia == "existencia_sal_dic" ? cantidad : 0;
                    parameters.Add("@importe_sal_dic", OdbcType.Numeric).Value = columnaImporte == "importe_sal_dic" ? importe : 0;

                    parameters.Add("@insumo", OdbcType.Numeric).Value = insumo;
                    parameters.Add("@ultimo_cp", OdbcType.Numeric).Value = importe;
                    parameters.Add("@fecha_cp", OdbcType.Date).Value = DateTime.Now.Date;

                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;

                    count += cmd.ExecuteNonQuery();
                }
                #endregion
            }
            #endregion

            return true;
        }

        public List<tblAlm_PermisoFamilias> getPermisosFamilia(string cc)
        {
            return _context.tblAlm_PermisoFamilias.Where(x => x.estatus && x.cc == cc).ToList();
        }

        private decimal getCostoPromedioNuevo(int almacen, int insumo)
        {
            decimal costoPromedio = 0;

            var costoPromedioEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                    ROUND(AVG(precio), 1) AS costo_prom 
                                FROM si_movimientos_det 
                                WHERE almacen = {0} AND insumo = {1}", almacen, insumo)
            );

            if (costoPromedioEK != null)
            {
                var costoPromedioENKONTROL = ((List<dynamic>)costoPromedioEK.ToObject<List<dynamic>>())[0];

                if (costoPromedioENKONTROL.costo_prom != null)
                {
                    costoPromedio = Convert.ToDecimal(costoPromedioENKONTROL.costo_prom, CultureInfo.InvariantCulture);
                }
            }

            return costoPromedio;
        }

        private decimal getCostoPromedioKardex(int almacen, int insumo)
        {
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
            {
                #region COLOMBIA
                var promedioEnkontrol = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, new OdbcConsultaDTO()
                {
                    consulta = @"
                    SELECT
                        SUM((CASE WHEN existencia_ent_ini IS NOT NULL THEN existencia_ent_ini ELSE 0 END) + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic) AS entradas,
                        SUM((CASE WHEN existencia_sal_ini IS NOT NULL THEN existencia_sal_ini ELSE 0 END) + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic) AS salidas,
                        SUM((CASE WHEN importe_ent_ini IS NOT NULL THEN importe_ent_ini ELSE 0 END) + importe_ent_ene + importe_ent_feb + importe_ent_mar + importe_ent_abr + importe_ent_may + importe_ent_jun + importe_ent_jul + importe_ent_ago + importe_ent_sep + importe_ent_oct + importe_ent_nov + importe_ent_dic) AS montoEntradas,
                        SUM((CASE WHEN importe_sal_ini IS NOT NULL THEN importe_sal_ini ELSE 0 END) + importe_sal_ene + importe_sal_feb + importe_sal_mar + importe_sal_abr + importe_sal_may + importe_sal_jun + importe_sal_jul + importe_sal_ago + importe_sal_sep + importe_sal_oct + importe_sal_nov + importe_sal_dic) AS montoSalidas,
                        entradas - salidas AS existencias,
                        montoEntradas - montoSalidas AS montoResultado,
                        CASE WHEN existencias > 0 THEN (montoResultado / existencias) ELSE 0 END AS costoPromedio
                            FROM DBA.si_acumula_almacen
                            WHERE almacen = ? AND insumo = ? AND ano >= ?",
                    parametros = new List<OdbcParameterDTO>() {
                        new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = almacen },
                        new OdbcParameterDTO() { nombre = "insumo", tipo = OdbcType.Numeric, valor = insumo },
                        new OdbcParameterDTO() { nombre = "ano", tipo = OdbcType.Numeric, valor = DateTime.Now.Year }
                    }
                });

                return Convert.ToDecimal(promedioEnkontrol[0].costoPromedio);
                #endregion
            }
            else
            {
                #region DEMAS EMPRESAS
                var promedioEnkontrol = _contextEnkontrol.Select<dynamic>(vSesiones.sesionEmpresaActual == 1 ? EnkontrolEnum.CplanProd : EnkontrolEnum.ArrenProd,
                    new OdbcConsultaDTO()
                    {
                        consulta = @"
                            SELECT
                                SUM((CASE WHEN existencia_ent_ini IS NOT NULL THEN existencia_ent_ini ELSE 0 END) + existencia_ent_ene + existencia_ent_feb + existencia_ent_mar + existencia_ent_abr + existencia_ent_may + existencia_ent_jun + existencia_ent_jul + existencia_ent_ago + existencia_ent_sep + existencia_ent_oct + existencia_ent_nov + existencia_ent_dic) AS entradas,
                                SUM((CASE WHEN existencia_sal_ini IS NOT NULL THEN existencia_sal_ini ELSE 0 END) + existencia_sal_ene + existencia_sal_feb + existencia_sal_mar + existencia_sal_abr + existencia_sal_may + existencia_sal_jun + existencia_sal_jul + existencia_sal_ago + existencia_sal_sep + existencia_sal_oct + existencia_sal_nov + existencia_sal_dic) AS salidas,
                                SUM((CASE WHEN importe_ent_ini IS NOT NULL THEN importe_ent_ini ELSE 0 END) + importe_ent_ene + importe_ent_feb + importe_ent_mar + importe_ent_abr + importe_ent_may + importe_ent_jun + importe_ent_jul + importe_ent_ago + importe_ent_sep + importe_ent_oct + importe_ent_nov + importe_ent_dic) AS montoEntradas,
                                SUM((CASE WHEN importe_sal_ini IS NOT NULL THEN importe_sal_ini ELSE 0 END) + importe_sal_ene + importe_sal_feb + importe_sal_mar + importe_sal_abr + importe_sal_may + importe_sal_jun + importe_sal_jul + importe_sal_ago + importe_sal_sep + importe_sal_oct + importe_sal_nov + importe_sal_dic) AS montoSalidas,
                                entradas - salidas AS existencias,
                                montoEntradas - montoSalidas AS montoResultado,
                                CASE WHEN existencias > 0 THEN (montoResultado / existencias) ELSE 0 END AS costoPromedio
                            FROM si_acumula_almacen
                            WHERE almacen = ? AND insumo = ? AND ano >= ?",
                        parametros = new List<OdbcParameterDTO>() {
                            new OdbcParameterDTO() { nombre = "almacen", tipo = OdbcType.Numeric, valor = almacen },
                            new OdbcParameterDTO() { nombre = "insumo", tipo = OdbcType.Numeric, valor = insumo },
                            new OdbcParameterDTO() { nombre = "ano", tipo = OdbcType.Numeric, valor = DateTime.Now.Year }
                        }
                    }
                );

                return Convert.ToDecimal(promedioEnkontrol[0].costoPromedio);
                #endregion
            }
        }

        public decimal getCostoPromedioAcumulaAlmacen(int almacen, int insumo)
        {
            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Colombia)
            {
                #region COLOMBIA
                decimal costoPromedio = 0;

                var listaAcumulaEK = consultaCheckProductivo(
                    string.Format(@"SELECT * FROM DBA.si_acumula_almacen WHERE almacen = {0} AND ano = {1} AND insumo = {2}", almacen, DateTime.Now.Year, insumo)
                );

                if (listaAcumulaEK != null)
                {
                    var listaAcumula = (List<AcumulaAlmacenDTO>)listaAcumulaEK.ToObject<List<AcumulaAlmacenDTO>>();
                    decimal existenciaResultado = 0;
                    decimal importeResultado = 0;

                    foreach (var acu in listaAcumula)
                    {
                        existenciaResultado += acu.existencia_ent_ini ?? 0;
                        importeResultado += acu.importe_ent_ini ?? 0;
                        existenciaResultado -= acu.existencia_sal_ini ?? 0;
                        importeResultado -= acu.importe_sal_ini ?? 0;

                        existenciaResultado += acu.existencia_ent_ene;
                        importeResultado += acu.importe_ent_ene;
                        existenciaResultado -= acu.existencia_sal_ene;
                        importeResultado -= acu.importe_sal_ene;

                        existenciaResultado += acu.existencia_ent_feb;
                        importeResultado += acu.importe_ent_feb;
                        existenciaResultado -= acu.existencia_sal_feb;
                        importeResultado -= acu.importe_sal_feb;

                        existenciaResultado += acu.existencia_ent_mar;
                        importeResultado += acu.importe_ent_mar;
                        existenciaResultado -= acu.existencia_sal_mar;
                        importeResultado -= acu.importe_sal_mar;

                        existenciaResultado += acu.existencia_ent_abr;
                        importeResultado += acu.importe_ent_abr;
                        existenciaResultado -= acu.existencia_sal_abr;
                        importeResultado -= acu.importe_sal_abr;

                        existenciaResultado += acu.existencia_ent_may;
                        importeResultado += acu.importe_ent_may;
                        existenciaResultado -= acu.existencia_sal_may;
                        importeResultado -= acu.importe_sal_may;

                        existenciaResultado += acu.existencia_ent_jun;
                        importeResultado += acu.importe_ent_jun;
                        existenciaResultado -= acu.existencia_sal_jun;
                        importeResultado -= acu.importe_sal_jun;

                        existenciaResultado += acu.existencia_ent_jul;
                        importeResultado += acu.importe_ent_jul;
                        existenciaResultado -= acu.existencia_sal_jul;
                        importeResultado -= acu.importe_sal_jul;

                        existenciaResultado += acu.existencia_ent_ago;
                        importeResultado += acu.importe_ent_ago;
                        existenciaResultado -= acu.existencia_sal_ago;
                        importeResultado -= acu.importe_sal_ago;

                        existenciaResultado += acu.existencia_ent_sep;
                        importeResultado += acu.importe_ent_sep;
                        existenciaResultado -= acu.existencia_sal_sep;
                        importeResultado -= acu.importe_sal_sep;

                        existenciaResultado += acu.existencia_ent_oct;
                        importeResultado += acu.importe_ent_oct;
                        existenciaResultado -= acu.existencia_sal_oct;
                        importeResultado -= acu.importe_sal_oct;

                        existenciaResultado += acu.existencia_ent_nov;
                        importeResultado += acu.importe_ent_nov;
                        existenciaResultado -= acu.existencia_sal_nov;
                        importeResultado -= acu.importe_sal_nov;

                        existenciaResultado += acu.existencia_ent_dic;
                        importeResultado += acu.importe_ent_dic;
                        existenciaResultado -= acu.existencia_sal_dic;
                        importeResultado -= acu.importe_sal_dic;
                    }

                    costoPromedio = existenciaResultado != 0 ? (importeResultado / existenciaResultado) : 0;
                }
                return costoPromedio;
                #endregion
            }
            else
            {
                #region DEMAS EMPRESAS
                decimal costoPromedio = 0;

                var listaAcumulaEK = consultaCheckProductivo(
                    string.Format(@"SELECT * FROM si_acumula_almacen WHERE almacen = {0} AND ano = {1} AND insumo = {2}", almacen, DateTime.Now.Year, insumo)
                );

                if (listaAcumulaEK != null)
                {
                    var listaAcumula = (List<AcumulaAlmacenDTO>)listaAcumulaEK.ToObject<List<AcumulaAlmacenDTO>>();
                    decimal existenciaResultado = 0;
                    decimal importeResultado = 0;

                    foreach (var acu in listaAcumula)
                    {
                        existenciaResultado += acu.existencia_ent_ini ?? 0;
                        importeResultado += acu.importe_ent_ini ?? 0;
                        existenciaResultado -= acu.existencia_sal_ini ?? 0;
                        importeResultado -= acu.importe_sal_ini ?? 0;

                        existenciaResultado += acu.existencia_ent_ene;
                        importeResultado += acu.importe_ent_ene;
                        existenciaResultado -= acu.existencia_sal_ene;
                        importeResultado -= acu.importe_sal_ene;

                        existenciaResultado += acu.existencia_ent_feb;
                        importeResultado += acu.importe_ent_feb;
                        existenciaResultado -= acu.existencia_sal_feb;
                        importeResultado -= acu.importe_sal_feb;

                        existenciaResultado += acu.existencia_ent_mar;
                        importeResultado += acu.importe_ent_mar;
                        existenciaResultado -= acu.existencia_sal_mar;
                        importeResultado -= acu.importe_sal_mar;

                        existenciaResultado += acu.existencia_ent_abr;
                        importeResultado += acu.importe_ent_abr;
                        existenciaResultado -= acu.existencia_sal_abr;
                        importeResultado -= acu.importe_sal_abr;

                        existenciaResultado += acu.existencia_ent_may;
                        importeResultado += acu.importe_ent_may;
                        existenciaResultado -= acu.existencia_sal_may;
                        importeResultado -= acu.importe_sal_may;

                        existenciaResultado += acu.existencia_ent_jun;
                        importeResultado += acu.importe_ent_jun;
                        existenciaResultado -= acu.existencia_sal_jun;
                        importeResultado -= acu.importe_sal_jun;

                        existenciaResultado += acu.existencia_ent_jul;
                        importeResultado += acu.importe_ent_jul;
                        existenciaResultado -= acu.existencia_sal_jul;
                        importeResultado -= acu.importe_sal_jul;

                        existenciaResultado += acu.existencia_ent_ago;
                        importeResultado += acu.importe_ent_ago;
                        existenciaResultado -= acu.existencia_sal_ago;
                        importeResultado -= acu.importe_sal_ago;

                        existenciaResultado += acu.existencia_ent_sep;
                        importeResultado += acu.importe_ent_sep;
                        existenciaResultado -= acu.existencia_sal_sep;
                        importeResultado -= acu.importe_sal_sep;

                        existenciaResultado += acu.existencia_ent_oct;
                        importeResultado += acu.importe_ent_oct;
                        existenciaResultado -= acu.existencia_sal_oct;
                        importeResultado -= acu.importe_sal_oct;

                        existenciaResultado += acu.existencia_ent_nov;
                        importeResultado += acu.importe_ent_nov;
                        existenciaResultado -= acu.existencia_sal_nov;
                        importeResultado -= acu.importe_sal_nov;

                        existenciaResultado += acu.existencia_ent_dic;
                        importeResultado += acu.importe_ent_dic;
                        existenciaResultado -= acu.existencia_sal_dic;
                        importeResultado -= acu.importe_sal_dic;
                    }

                    costoPromedio = existenciaResultado != 0 ? (importeResultado / existenciaResultado) : 0;
                }
                return costoPromedio;
                #endregion
            }
        }

        public decimal getCostoPromedioEntrada(int almacen, string cc, int insumo)
        {
            decimal costoPromedio = 0;

            #region Checar Existencia
            bool hayExistencia = false;

            var movimientosEK = consultaCheckProductivo(
                string.Format(@"SELECT 
                                    det.almacen, det.tipo_mov, det.numero, det.insumo, det.cantidad 
                                FROM si_movimientos mov 
                                	INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                WHERE det.almacen = {0} AND det.insumo = {1}", almacen, insumo)
            );

            if (movimientosEK != null)
            {
                var movimientos = (List<MovimientoDetalleEnkontrolDTO>)movimientosEK.ToObject<List<MovimientoDetalleEnkontrolDTO>>();

                decimal cantidadEntradas = movimientos.Where(x => x.tipo_mov < 50).Sum(x => x.cantidad);
                decimal cantidadSalidas = movimientos.Where(x => x.tipo_mov > 50).Sum(x => x.cantidad);

                if (cantidadEntradas > cantidadSalidas)
                {
                    hayExistencia = true;
                }
            }
            #endregion

            if (hayExistencia)
            {
                var listaAcumulaCCEK = consultaCheckProductivo(
                    string.Format(@"SELECT TOP 1 * FROM si_acumula_cc WHERE cc = '{0}' AND ano = {1} AND insumo = {2}", cc, DateTime.Now.Year, insumo)
                );

                if (listaAcumulaCCEK != null)
                {
                    var listaAcumula = ((List<dynamic>)listaAcumulaCCEK.ToObject<List<dynamic>>())[0];

                    costoPromedio = listaAcumula.ultimo_cp != null ? Convert.ToDecimal(listaAcumula.ultimo_cp, CultureInfo.InvariantCulture) : 0;
                }
            }
            else
            {
                var ultimoPrecioEntradaEK = consultaCheckProductivo(
                    string.Format(@"SELECT TOP 1 
                                        det.precio 
                                    FROM si_movimientos mov 
                                    	INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                    WHERE det.almacen = {0} AND det.insumo = {1} AND mov.tipo_mov < 50 
                                    ORDER BY mov.fecha DESC, mov.hora DESC", almacen, insumo)
                );

                if (ultimoPrecioEntradaEK != null)
                {
                    var ultimoPrecioEntrada = ((List<dynamic>)ultimoPrecioEntradaEK.ToObject<List<dynamic>>())[0];

                    costoPromedio = ultimoPrecioEntrada.precio != null ? Convert.ToDecimal(ultimoPrecioEntrada.precio, CultureInfo.InvariantCulture) : 0;
                }
            }

            return costoPromedio;
        }

        public List<RequisicionSeguimientoDTO> getRequisicionesSeguimiento(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, int compradorSugeridoEnReq)
        {
            List<RequisicionSeguimientoDTO> seguimiento = new List<RequisicionSeguimientoDTO>();
            List<RequisicionSeguimientoDTO> seguimientoFiltrado = new List<RequisicionSeguimientoDTO>();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                #region EmpresaPeru
                case EmpresaEnum.Peru:
                    {
                        using (var ctxPeru = new MainContext())
                        {

                            var listaCentrosCosto = ctxPeru.tblP_CC.ToList();

                            //usuarios Starsoft
                            var consultaUsuariosStarsoft = @"SELECT cast(TCLAVE as int) as TCLAVE,TDESCRI FROM [003BDCOMUN].[dbo].[TABAYU] WHERE TCOD = '12' ";

                            List<InfoUsuariosStarsoftDTO> listaEmpleados = new List<InfoUsuariosStarsoftDTO>();
                            DynamicParameters lstParametros = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaEmpleados = conexion.Query<InfoUsuariosStarsoftDTO>(consultaUsuariosStarsoft, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            }

                            //Proveedores Starsoft

                            var consultaProveedoresStarsoft = @"SELECT prvccodigo,prvcnombre,prvcruc FROM [003BDCOMUN].[dbo].[MAEPROV] WHERE ISNULL(PRVCESTADO,'') = 'V' ";
                            List<infoProveedoresStarsoftDTO> listaProveedores = new List<infoProveedoresStarsoftDTO>();
                            DynamicParameters lstParametrosProv = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaProveedores = conexion.Query<infoProveedoresStarsoftDTO>(consultaProveedoresStarsoft, lstParametrosProv, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };

                            //Insumos Starsoft
                            var consultaInsumosStarsoft = @"SELECT ACODIGO,ADESCRI,AUNIDAD FROM [003BDCOMUN].[dbo].[MAEART]";
                            List<infoInsumosStarsoftDTO> listaInsumos = new List<infoInsumosStarsoftDTO>();
                            DynamicParameters lstParametrosInsumo = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaInsumos = conexion.Query<infoInsumosStarsoftDTO>(consultaInsumosStarsoft, lstParametrosInsumo, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            //Almacen Starsoft
                            var consultaAlmacenStarsoft = @"SELECT STALMA AS Almacen,TADESCRI AS Descripción,CONVERT(numeric(11,2),STSKDIS) AS stock FROM [003BDCOMUN].[dbo].[STKART] s INNER JOIN [003BDCOMUN].[dbo].[TABALM]t ON s.STALMA=t.TAALMA";
                            List<infoAlmacenStarsoftDTO> listaAlmacen = new List<infoAlmacenStarsoftDTO>();
                            DynamicParameters lstParametrosAlmacen = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaAlmacen = conexion.Query<infoAlmacenStarsoftDTO>(consultaAlmacenStarsoft, lstParametrosAlmacen, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            var consultaAuditoriaStarsoft = @"SELECT COD_AUDITORIA,OBSERVACION FROM [BDWENCO].[dbo].[AUDITORIA_SISTEMAS]";
                            List<infoAuditoriaStarsoftDTO> listaAuditoria = new List<infoAuditoriaStarsoftDTO>();
                            DynamicParameters lstParametrosAuditoria = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaAuditoria = conexion.Query<infoAuditoriaStarsoftDTO>(consultaAuditoriaStarsoft, lstParametrosAuditoria, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            if (listaCC == null)
                            {
                                listaCC = new List<string>();
                            }
                            /*
                            var seguimientoReqPeru = ctxPeru.tblCom_Req.Where(re => listaCC.Contains(re.cc)).ToList().Where(requi =>
                                (requi.fecha >= fechaInicial && requi.fecha <= fechaFinal) &&
                                 (requisitor > 0 ? requi.comprador == requisitor : true)).ToList();
                             * */
                            var seguimientoReqPeru = ctxPeru.Select<SeguimientoRequisicionDTO>(new DapperDTO
                                {
                                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                    consulta = string.Format(@"
                                        SELECT
                                            REQ.id AS reqId,
                                            REQ.cc AS reqCc,
                                            REQ.numero AS reqNumero,
                                            REQ.cc + '-' + CAST(REQ.numero AS VARCHAR) AS requisicion,
                                            REQ.PERU_tipoRequisicion AS reqPeruTipoRequisicion,
                                            REQ.solicito AS reqSolicito,
                                            REQ.comprador AS reqComprador,
                                            REQ.fecha AS reqFecha,
                                            REQ.autoriza AS reqAutoriza,
                                            REQ.idLibreAbordo AS reqLibreAbordo,
                                            REQDET.insumo AS reqInsumo,
                                            REQDET.cantOrdenada AS reqCantidadOrdenada,
                                            OC.id AS ocId,
                                            OC.cc AS ocCc,
                                            OC.numero AS ocNumero,
                                            OC.PERU_proveedor AS ocPeruProveedor,
                                            OC.fecha AS ocFecha,
                                            OC.fecha_autoriza AS ocFechaAutoriza,
                                            OC.bienes_servicios AS ocBienesServicios,
                                            OCDET.fecha_entrega AS ocDetFechaEntrega,
	                                        U.nombre + ' ' + U.apellidoPaterno + ' ' + CASE WHEN U.apellidoMaterno IS NOT NULL THEN U.apellidoMaterno ELSE '' END AS solicitoNombreCompleto,
	                                        PROV.PRVCNOMBRE AS provedorNombre,
	                                        ART.ADESCRI AS insumoNombre,
	                                        UCOMPRADOR.nombreUsuario AS compradorUsuario,
	                                        USUGERIDO.nombre + ' ' + USUGERIDO.apellidoPaterno + ' ' + CASE WHEN USUGERIDO.apellidoMaterno IS NOT NULL THEN USUGERIDO.apellidoMaterno ELSE '' END AS compradorSugerido,
											REQ.validadoAlmacen AS reqValidadoAlmacen,
											REQ.validadoCompras AS reqValidadoCompras,
											REQ.fechaValidacionAlmacen AS reqFechaValidacionAlmacen,
											REQ.consigna AS reqConsigna,
											REQ.licitacion AS reqLicitacion,
											REQ.crc AS reqCrc,
											REQ.convenio AS reqConvenio,
											OC.tiempoEntregaDias AS ocTiempoEntrega,
											OC.tiempoEntregaComentarios AS ocTiempoEntregaComentarios,
											OC.colocada AS ocColocada,
											OC.colocadaFecha AS ocColocadaFecha,
                                            CASE
		                                        WHEN MOV.fecha IS NOT NULL THEN MOV.fecha
		                                        ELSE NULL
	                                        END AS fechaEntrada,
	                                        MOV.fecha AS fechaAutoriza
                                        FROM
                                            tblCom_Req AS REQ
                                        INNER JOIN
                                            tblCom_ReqDet AS REQDET
                                            ON
                                                REQDET.idReq = REQ.id
                                        LEFT JOIN
                                            tblCom_OrdenCompraDet AS OCDET
                                            ON
                                                OCDET.cc = REQ.cc AND
                                                OCDET.num_requisicion = REQ.numero AND
                                                OCDET.part_requisicion = REQDET.partida AND
                                                OCDET.estatusRegistro = 1
                                        LEFT JOIN
                                            tblCom_OrdenCompra AS OC
                                            ON
                                                OC.id = OCDET.idOrdenCompra AND
                                                OC.PERU_tipoCompra = REQ.PERU_tipoRequisicion AND
                                                OC.estatusRegistro = 1
                                        LEFT JOIN
	                                        tblP_Usuario_Enkontrol AS UEK
	                                        ON
		                                        UEK.empleado = REQ.solicito
                                        LEFT JOIN
	                                        tblP_Usuario AS U
	                                        ON
		                                        U.id = UEK.idUsuario
                                        LEFT JOIN
	                                        tblCom_MAEPROV AS PROV
	                                        ON
		                                        PROV.PRVCCODIGO = OC.PERU_proveedor AND
                                                PROV.PRVCESTADO = 'V'
                                        LEFT JOIN
	                                        [10.1.0.136].[003BDCOMUN].dbo.MAEART AS ART
	                                        ON
		                                        ART.ACODIGO = '0' + CAST(REQDET.insumo AS varchar)
                                        LEFT JOIN
	                                        tblP_Usuario_Enkontrol AS UEKCOMPRADOR
	                                        ON
		                                        UEKCOMPRADOR.empleado = REQ.comprador
                                        LEFT JOIN
	                                        tblP_Usuario AS UCOMPRADOR
	                                        ON
		                                        UCOMPRADOR.id = UEKCOMPRADOR.idUsuario
                                        LEFT JOIN
	                                        tblP_Usuario_Enkontrol AS UEKSUGERIDO
	                                        ON
		                                        UEKSUGERIDO.empleado = REQ.comprador
                                        LEFT JOIN
	                                        tblP_Usuario AS USUGERIDO
	                                        ON
		                                        USUGERIDO.id = UEKSUGERIDO.idUsuario
                                        LEFT JOIN
	                                        (
		                                        SELECT
			                                        MOV.cc, MOV.orden_ct, MOV.fecha, MOVDET.partida_oc
		                                        FROM
			                                        tblAlm_Movimientos AS MOV
		                                        INNER JOIN
			                                        tblAlm_MovimientosDet AS MOVDET
			                                        ON
				                                        MOVDET.almacen = MOV.almacen AND
				                                        MOVDET.tipo_mov = MOV.tipo_mov AND
				                                        MOVDET.numero = MOV.numero AND
				                                        MOVDET.estatusHabilitado = 1
		                                        WHERE
			                                        MOV.estatusHabilitado = 1
	                                        ) AS MOV ON MOV.cc = OC.cc AND MOV.orden_ct = OC.numero AND MOV.partida_oc = OCDET.partida
                                        WHERE
                                            REQ.estatusRegistro = 1 AND
                                            REQDET.estatusRegistro = 1 AND
                                            REQ.cc IN @paramCCs AND
                                            REQ.fecha BETWEEN @paramFechaInicial AND @paramFechaFinal
                                            {0}
                                        ORDER BY
                                            REQ.PERU_tipoRequisicion,
                                            REQ.cc,
                                            REQ.numero,
                                            REQDET.partida
                                            ", requisitor == 0 ? "" : " AND REQ.comprador = @paramRequisitor"),
                                    parametros = new
                                    {
                                        paramCCs = listaCC,
                                        paramFechaInicial = fechaInicial,
                                        paramFechaFinal = fechaFinal,
                                        paramRequisitor = requisitor
                                    }
                                });

                            //var estatusVencido = "";
                            //var tipoRequisicionDesc = "";
                            if (seguimientoReqPeru != null)
                            {
                                foreach (var segui in seguimientoReqPeru)
                                {
                                    /*
                                    var requisicionDet = ctxPeru.tblCom_ReqDet.Where(rd => rd.idReq == segui.id && rd.estatusRegistro).FirstOrDefault() ?? new tblCom_ReqDet();
                                    var ordenCompra = ctxPeru.Select<tblCom_OrdenCompra>(new DapperDTO
                                    {
                                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                        consulta = "SELECT com.* FROM tblCom_OrdenCompra AS com INNER JOIN tblCom_OrdenCompraDet AS det ON com.id = det.idOrdenCompra INNER JOIN tblCom_Req AS r ON r.numero = det.num_requisicion AND r.PERU_tipoRequisicion = @paramTipoCompra AND r.estatusRegistro = 1 WHERE det.estatusRegistro = 1 AND com.estatusRegistro = 1 AND det.cc = @paramCC AND det.num_requisicion = @paramNumRequi AND com.PERU_tipoCompra = @paramTipoCompra",
                                        parametros = new { paramCC = segui.cc, paramNumRequi = segui.numero, paramTipoCompra = segui.PERU_tipoRequisicion }
                                    }).FirstOrDefault();
                                    var idCompra = ordenCompra != null ? ordenCompra.id : 0;
                                    var ordenCompraDet = ctxPeru.tblCom_OrdenCompraDet.Where(ocDet => ocDet.idOrdenCompra == idCompra).FirstOrDefault() ?? new tblCom_OrdenCompraDet();
                                    //var ordenCompra = ctxPeru.tblCom_OrdenCompra.Where(oc => oc.id == ordenCompraDet.idOrdenCompra).FirstOrDefault() ?? new tblCom_OrdenCompra();
                                    */



                                    //var solicito = listaEmpleados.FirstOrDefault(x => x.TCLAVE == segui.solicito);
                                    /*
                                    var solicito = "";
                                    var solicitoEK = ctxPeru.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == segui.solicito);
                                    if (solicitoEK != null)
                                    {
                                        var solicitoSP = ctxPeru.tblP_Usuario.FirstOrDefault(x => x.id == solicitoEK.idUsuario);
                                        if (solicitoSP != null)
                                        {
                                            solicito = PersonalUtilities.NombreCompletoMayusculas(solicitoSP.nombre, solicitoSP.apellidoPaterno, solicitoSP.apellidoMaterno);
                                        }
                                    }
                                    var solicitoDescr = "";
                                    var provDescr = "";
                                    var insumoDescr = "";
                                    if (solicito != null)
                                    {
                                        //solicitoDescr = (string)solicito.TDESCRI;
                                        solicitoDescr = solicito;
                                    }

                                    var proNombre = "";
                                    if (ordenCompra != null)
                                    {
                                        var descProvvedor = listaProveedores.FirstOrDefault(x => x.prvccodigo == ordenCompra.PERU_proveedor);
                                        if (descProvvedor != null)
                                        {
                                            proNombre = descProvvedor.prvcnombre;
                                        }
                                    }

                                    provDescr = proNombre;

                                    var descInsumo = listaInsumos.FirstOrDefault(x => x.ACODIGO == "0" + requisicionDet.insumo.ToString());
                                    if (descInsumo != null)
                                    {
                                        insumoDescr = (string)descInsumo.ADESCRI;
                                    }

                                    var compradorDesc = (from emp in ctxPeru.tblP_Usuario_Enkontrol
                                                         join usu in ctxPeru.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == segui.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();

                                    var comprador = "";
                                    var compradorEk = ctxPeru.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == segui.comprador);
                                    if (compradorEk != null)
                                    {
                                        var compradorSP = ctxPeru.tblP_Usuario.FirstOrDefault(x => x.id == compradorEk.idUsuario);
                                        if (compradorSP != null)
                                        {
                                            comprador = PersonalUtilities.NombreCompletoMayusculas(compradorSP.nombre, compradorSP.apellidoPaterno, compradorSP.apellidoMaterno);
                                        }
                                    }*/

                                    var seguimientoPeru = new RequisicionSeguimientoDTO();
                                    seguimientoPeru.cc = segui.reqCc;
                                    seguimientoPeru.requisitor = segui.reqSolicito;
                                    seguimientoPeru.numeroRequisicion = segui.reqNumero;
                                    seguimientoPeru.requisitorDesc = segui.solicitoNombreCompleto;
                                    seguimientoPeru.fechaEntregaCompras = segui.ocDetFechaEntrega;
                                    seguimientoPeru.fechaElaboracion = segui.reqFecha;
                                    seguimientoPeru.tipoRequisicion = segui.reqPeruTipoRequisicion;
                                    seguimientoPeru.economico = "";
                                    seguimientoPeru.descripcion = segui.insumoNombre;
                                    seguimientoPeru.comprador = segui.reqComprador;
                                    seguimientoPeru.compradorDesc = segui.compradorUsuario;
                                    seguimientoPeru.numeroOrdenCompra = segui.ocNumero;
                                    seguimientoPeru.fechaCompra = segui.ocFecha;
                                    seguimientoPeru.fechaAutorizacionCompra = !segui.ocFechaAutoriza.HasValue || segui.ocFechaAutoriza.Value < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.ocFechaAutoriza.Value;
                                    seguimientoPeru.fechaAutorizacionCompraDesc = segui.ocFechaAutoriza.HasValue ? segui.ocFechaAutoriza.Value.ToString("dd/MM/yyyy") : "";
                                    seguimientoPeru.ordenCompraAutorizada = !segui.ocFecha.HasValue || segui.ocFecha.Value < new DateTime(2022, 1, 1) ? "NO" : "SI";
                                    seguimientoPeru.proveedorPeru = segui.ocPeruProveedor ?? "";
                                    seguimientoPeru.bienes_servicios = segui.ocBienesServicios ?? "";
                                    seguimientoPeru.proveedorDesc = segui.provedorNombre ?? "";
                                    seguimientoPeru.fechaAutorizaRe = segui.reqAutoriza < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.reqAutoriza;
                                    seguimientoPeru.almacen = segui.reqLibreAbordo;
                                    seguimientoPeru.compraCantidadRecibida = segui.reqDetCantidadOrdenada;
                                    seguimientoPeru.sugerido = segui.compradorSugerido;
                                    seguimientoPeru.requisicion = segui.reqCc + "-" + segui.reqNumero;
                                    seguimientoPeru.fechaElaboracionDesc = seguimientoPeru.fechaElaboracion.Value.ToShortDateString();
                                    seguimientoPeru.fechaEntregaComprasDesc = "";
                                    seguimientoPeru.sugeridoNum = segui.reqComprador;
                                    seguimientoPeru.validadoAlmacen = segui.reqValidadoAlmacen ?? false;
                                    seguimientoPeru.validadoCompras = segui.reqValidadoCompras;
                                    seguimientoPeru.almacen = segui.reqLibreAbordo;
                                    seguimientoPeru.fechaEntregaCompras = segui.reqFechaValidacionAlmacen;
                                    seguimientoPeru.fechaEntregaComprasDesc = segui.reqFechaValidacionAlmacen.HasValue ? segui.reqFechaValidacionAlmacen.Value.ToShortDateString() : "";
                                    seguimientoPeru.consigna = segui.reqConsigna.HasValue ? segui.reqConsigna.Value : false;
                                    seguimientoPeru.licitacion = segui.reqLicitacion;
                                    seguimientoPeru.crc = segui.reqCrc;
                                    seguimientoPeru.convenio = segui.reqConvenio;
                                    seguimientoPeru.fechaEntrada = segui.fechaEntrada;

                                    if (seguimientoPeru.numeroOrdenCompra != null && seguimientoPeru.numeroOrdenCompra > 0)
                                    {
                                        seguimientoPeru.tiempoEntregaDias = segui.ocTiempoEntrega;
                                        seguimientoPeru.tiempoEntregaComentarios = segui.ocTiempoEntregaComentarios;
                                        seguimientoPeru.colocada = segui.ocColocada;
                                        seguimientoPeru.colocadaFecha = segui.ocColocada ? segui.ocColocadaFecha : null;
                                    }
                                    else
                                    {
                                        seguimientoPeru.ordenCompraAutorizada = "";
                                    }

                                    seguimiento.Add(seguimientoPeru);

                                    /*
                                    var seguimientoPeru = new RequisicionSeguimientoDTO
                                    {
                                        cc = segui.cc,
                                        requisitor = segui.solicito,
                                        numeroRequisicion = segui.numero,
                                        requisitorDesc = solicitoDescr,
                                        fechaEntregaCompras = ordenCompraDet.fecha_entrega,
                                        fechaElaboracion = segui.fecha,
                                        tipoRequisicion = segui.PERU_tipoRequisicion,
                                        economico = "",
                                        descripcion = insumoDescr,
                                        comprador = segui.comprador,
                                        compradorDesc = compradorDesc,
                                        numeroOrdenCompra = ordenCompraDet.numero,
                                        fechaCompra = ordenCompra == null || ordenCompra.fecha < new DateTime(2022, 1, 1) ? (DateTime?)null : ordenCompra.fecha,
                                        fechaAutorizacionCompra = ordenCompra != null ? ordenCompra.fecha_autoriza : (DateTime?)null,
                                        fechaAutorizacionCompraDesc = ordenCompra != null && ordenCompra.fecha_autoriza.HasValue ? ordenCompra.fecha_autoriza.Value.ToString("dd/MM/yyyy") : "",
                                        ordenCompraAutorizada = ordenCompra == null || ordenCompra.fecha < new DateTime(2022, 1, 1) ? "NO" : "SI",
                                        proveedorPeru = ordenCompra != null ? ordenCompra.PERU_proveedor : "",
                                        bienes_servicios = ordenCompra != null ? ordenCompra.bienes_servicios : "",
                                        proveedorDesc = provDescr,
                                        fechaAutorizaRe = segui.autoriza < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.autoriza,
                                        almacen = segui.idLibreAbordo,
                                        compraCantidadRecibida = requisicionDet.cantOrdenada,
                                        sugerido = comprador
                                    };

                                    seguimiento.Add(seguimientoPeru);
                                    */
                                }
                            }



                            if (seguimiento != null)
                            {
                                switch (estatus)
                                {
                                    case 0:
                                        seguimientoFiltrado = seguimiento;
                                        break;
                                    case 1:
                                        seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                        break;
                                    case 2:
                                        seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                        break;
                                }


                                foreach (var seg in seguimientoFiltrado)
                                {
                                    /*
                                    seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                    seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                    seg.fechaEntregaComprasDesc = "";

                                    string compradorSugerido = "";
                                    var requisicionSIGOPLAN = ctxPeru.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);

                                    if (requisicionSIGOPLAN != null)
                                    {
                                        compradorSugerido = (from emp in ctxPeru.tblP_Usuario_Enkontrol
                                                             join usu in ctxPeru.tblP_Usuario
                                                             on emp.idUsuario equals usu.id
                                                             where emp.empleado == requisicionSIGOPLAN.comprador
                                                             select usu.nombreUsuario).FirstOrDefault();
                                        seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;

                                        seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                        seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                        seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                        //La fecha de entrega compras es la fecha de validación de almacén.
                                        seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                        seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                        seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                        seg.licitacion = requisicionSIGOPLAN.licitacion;
                                        seg.crc = requisicionSIGOPLAN.crc;
                                        seg.convenio = requisicionSIGOPLAN.convenio;
                                    }
                                    */

                                    //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                    //{
                                    //    seg.fechaAutorizaRe = null;
                                    //}

                                    if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4)
                                    {
                                        seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                    }

                                    if (seg.numeroOrdenCompra > 0)
                                    {
                                        seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                        /*
                                        var compraSIGOPLAN = ctxPeru.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                        if (compraSIGOPLAN != null)
                                        {
                                            seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                            seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                            seg.colocada = compraSIGOPLAN.colocada;
                                            if (seg.colocada)
                                            {
                                                seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                            }
                                        }
                                         * */
                                    }
                                    else
                                    {
                                        seg.ordenCompraAutorizada = "";
                                    }

                                    seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                    seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                    seg.entregaVencida = false;
                                    if (seg.fechaEntrada == null)
                                    {
                                        if (seg.colocada == true)
                                        {
                                            seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                            var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                            var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                            if (diasPasados > 0)
                                            {
                                                seg.entregaVencida = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (seg.colocada == true)
                                        {
                                            seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                            var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                            var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                            if (diasPasados > 0)
                                            {
                                                seg.entregaVencida = true;
                                            }
                                        }
                                    }

                                    #region Columnas Niveles de Servicios
                                    seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                    seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                    seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                    seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                    #endregion
                                }
                            }
                        }
                    }

                    break;
                #endregion

                #region Empresa Colombia
                case EmpresaEnum.Colombia:
                    {
                        var stringListaCC = listaCC != null && listaCC.Count > 0 ? string.Join(", ", listaCC.Select(x => "'" + x + "'")) : "";
                        var stringListaTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Join(", ", listaTipoInsumo.Select(x => "'" + x + "'")) : "";

                        var filtroCC = listaCC != null && listaCC.Count > 0 ? string.Format(@"req.cc IN ({0}) AND ", stringListaCC) : "";
                        var filtroTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Format(@"SUBSTRING(CONVERT(varchar(10), reqDet.insumo), 1, 1) IN ({0}) AND ", stringListaTipoInsumo) : "";

                        var seguimientoEK = consultaCheckProductivo(
                            string.Format(@"SELECT 
                                    req.cc AS cc, 
                                    req.solicito AS requisitor, 
                                    empReq.descripcion AS requisitorDesc, 
                                    req.numero AS numeroRequisicion, 
                                    req.fecha AS fechaElaboracion, 
                                    --'' AS fechaEntregaCompras, 
                                    Cast(req.tipo_req_oc as int) AS idTipoRequisicion, 
                                    eco.descripcion AS economico, 
                                    ins.descripcion AS descripcion, 
                                    oc.comprador AS comprador, 
                                    empComp.descripcion AS compradorDesc, 
                                    oc.numero AS numeroOrdenCompra, 
                                    oc.fecha AS fechaCompra, 
                                    oc.fecha_autoriza AS fechaAutorizacionCompra, 
                                    ISNULL(CONVERT(varchar, oc.fecha_autoriza, 103), '') AS fechaAutorizacionCompraDesc, 
                                    (CASE WHEN (oc.vobo_aut = 'S' OR oc.aut_aut = 'S') THEN 'SI' ELSE 'NO' END) AS ordenCompraAutorizada, 
                                    oc.proveedor AS proveedor, 
                                    oc.bienes_servicios AS bienes_servicios, 
                                    prov.nombre AS proveedorDesc, 
                                    (CASE WHEN mov.fecha IS NOT NULL THEN mov.fecha WHEN movNoInv.fecha IS NOT NULL THEN movNoInv.fecha ELSE NULL END) AS fechaEntrada,
                                    mov.fecha as fechaAutoriza,
                                    req.fecha_Autoriza as fechaAutorizaRe, 
                                    req.libre_abordo AS almacen, 
                                    oc.cant_recibida AS compraCantidadRecibida 
                                FROM DBA.so_requisicion req 
                                    INNER JOIN DBA.empleados empReq ON req.solicito = empReq.empleado 
                                    INNER JOIN DBA.so_requisicion_det reqDet ON req.cc = reqDet.cc AND req.numero = reqDet.numero AND reqDet.cantidad > 0 
                                    INNER JOIN DBA.insumos ins ON reqDet.insumo = ins.insumo 
                                    INNER JOIN DBA.cc eco ON req.cc = eco.cc 
                                    LEFT JOIN ( 
                                        SELECT 
                                            compra.cc, compra.numero, compra.comprador, compra.fecha, compra.fecha_autoriza, compra.vobo_aut, compra.aut_aut, compra.proveedor, compra.bienes_servicios, detalle.partida, detalle.num_requisicion, detalle.part_requisicion, detalle.cant_recibida 
                                        FROM DBA.so_orden_compra compra 
                                            INNER JOIN DBA.so_orden_compra_det detalle ON compra.cc = detalle.cc AND compra.numero = detalle.numero 
                                        WHERE compra.estatus != 'C' 
                                    ) AS oc ON req.cc = oc.cc AND req.numero = oc.num_requisicion AND reqDet.partida = oc.part_requisicion 
                                    LEFT JOIN DBA.empleados empComp ON oc.comprador = empComp.empleado 
                                    LEFT JOIN DBA.sp_proveedores prov ON oc.proveedor = prov.numpro 
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha, det.partida_oc 
                                        FROM DBA.si_movimientos mov 
                                        	INNER JOIN DBA.si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                                    ) AS mov ON oc.cc = mov.cc AND oc.numero = mov.orden_ct AND oc.partida = mov.partida_oc 
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha, det.partida_oc 
                                        FROM DBA.so_movimientos_noinv mov 
                                        	INNER JOIN DBA.so_movimientos_noinv_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.remision = det.remision 
                                    ) AS movNoInv ON oc.cc = movNoInv.cc AND oc.numero = movNoInv.orden_ct AND oc.partida = movNoInv.partida_oc  
                                WHERE {0} {1} req.fecha >= '{2}' AND req.fecha <= '{3}' " +
                                           (requisitor == 0 ? "" : " AND req.solicito = " + requisitor) + @" GROUP BY 
                                    req.cc, req.solicito, empReq.descripcion, req.numero, req.fecha, eco.descripcion, ins.descripcion, oc.comprador, empComp.descripcion, 
                                    oc.numero, oc.fecha, oc.fecha_autoriza, ordenCompraAutorizada,req.tipo_req_oc, oc.bienes_servicios, oc.proveedor, prov.nombre, mov.fecha, movNoInv.fecha , req.fecha_Autoriza, req.libre_abordo, oc.cant_recibida 
                                ORDER BY req.numero, oc.numero", filtroCC, filtroTipoInsumo, fechaInicial.ToString("yyyyMMdd"), fechaFinal.ToString("yyyyMMdd"))
                        );


                        if (seguimientoEK != null)
                        {
                            seguimiento = ((List<RequisicionSeguimientoDTO>)seguimientoEK.ToObject<List<RequisicionSeguimientoDTO>>())
                                //.Where(x =>
                                //    x.fechaEntrada == null || DateTime.Now.Subtract((DateTime)x.fechaEntrada).Days <= 5
                                //)
                            .ToList();

                            switch (estatus)
                            {
                                case 0:
                                    seguimientoFiltrado = seguimiento;
                                    break;
                                case 1:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                    break;
                                case 2:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                    break;
                            }

                            foreach (var seg in seguimientoFiltrado)
                            {
                                seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                seg.fechaEntregaComprasDesc = "";

                                string compradorSugerido = "";
                                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);
                                var tipoReqDesc = _context.tblCom_ReqTipo.FirstOrDefault(x => x.tipo_req_oc == seg.idTipoRequisicion);
                                if (requisicionSIGOPLAN != null)
                                {
                                    compradorSugerido = (from emp in _context.tblP_Usuario_Enkontrol
                                                         join usu in _context.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == requisicionSIGOPLAN.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();
                                    seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;
                                    seg.tipoRequisicion = tipoReqDesc.descripcion ?? "";
                                    seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                    seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                    seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                    //La fecha de entrega compras es la fecha de validación de almacén.
                                    seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                    seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                    seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                    seg.licitacion = requisicionSIGOPLAN.licitacion;
                                    seg.crc = requisicionSIGOPLAN.crc;
                                    seg.convenio = requisicionSIGOPLAN.convenio;
                                }

                                seg.sugerido = compradorSugerido;

                                //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                //{
                                //    seg.fechaAutorizaRe = null;
                                //}

                                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4)
                                {
                                    seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                }

                                if (seg.numeroOrdenCompra > 0)
                                {
                                    seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                    var compraSIGOPLAN = _context.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                    if (compraSIGOPLAN != null)
                                    {
                                        seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                        seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                        seg.colocada = compraSIGOPLAN.colocada;
                                        if (seg.colocada)
                                        {
                                            seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                        }
                                    }
                                }
                                else
                                {
                                    seg.ordenCompraAutorizada = "";
                                }

                                seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                seg.entregaVencida = false;
                                if (seg.fechaEntrada == null)
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }

                                #region Columnas Niveles de Servicios
                                seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                #endregion
                            }
                        }
                    }

                    break;
                #endregion

                #region EmpresaConstruplan
                default:
                    {
                        if (listaCC == null)
                        {
                            listaCC = new List<string>();
                        }

                        var stringListaCC = listaCC != null && listaCC.Count > 0 ? string.Join(", ", listaCC.Select(x => "'" + x + "'")) : "";
                        var stringListaTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Join(", ", listaTipoInsumo.Select(x => "'" + x + "'")) : "";

                        var filtroCC = listaCC != null && listaCC.Count > 0 ? string.Format(@"req.cc IN ({0}) AND ", stringListaCC) : "";
                        var filtroTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Format(@"SUBSTRING(CONVERT(varchar(10), reqDet.insumo), 1, 1) IN ({0}) AND ", stringListaTipoInsumo) : "";

                        var whereRequisicionesPorComprador = "";

                        if (compradorSugeridoEnReq != 0)
                        {
                            var requisicionesPorCompradorSugerido = _context.tblCom_Req
                            .Where(x =>
                                x.estatusRegistro &&
                                (listaCC.Count > 0 ? listaCC.Contains(x.cc) : true) &&
                                (x.fecha >= fechaInicial && x.fecha <= fechaFinal) &&
                                (x.comprador == compradorSugeridoEnReq))
                            .Select(x => new
                            {
                                cc = x.cc,
                                numero = x.numero,
                                requisicion = x.cc + "-" + x.numero
                            })
                            .GroupBy(x => x.cc).ToList();

                            foreach (var item in requisicionesPorCompradorSugerido)
                            {
                                whereRequisicionesPorComprador += "(req.cc='" + item.Key + "' AND req.numero IN (";
                                foreach (var numero in item)
                                {
                                    if (numero == item.Last())
                                    {
                                        whereRequisicionesPorComprador += numero.numero + ")";
                                    }
                                    else
                                    {
                                        whereRequisicionesPorComprador += numero.numero + ",";
                                    }
                                }

                                if (item == requisicionesPorCompradorSugerido.Last())
                                {
                                    whereRequisicionesPorComprador += ")";
                                }
                                else
                                {
                                    whereRequisicionesPorComprador += ") OR ";
                                }
                            }
                        }

                        // var seguimientoEK = consultaCheckProductivo(
                        //     string.Format(@"SELECT 
                        //             req.cc AS cc, 
                        //             req.solicito AS requisitor, 
                        //             empReq.descripcion AS requisitorDesc, 
                        //             req.numero AS numeroRequisicion, 
                        //             req.fecha AS fechaElaboracion, 
                        //             --'' AS fechaEntregaCompras, 
                        //             tipoReq.descripcion AS tipoRequisicion, 
                        //             eco.descripcion AS economico, 
                        //             ins.descripcion AS descripcion, 
                        //             oc.comprador AS comprador, 
                        //             empComp.descripcion AS compradorDesc, 
                        //             oc.numero AS numeroOrdenCompra, 
                        //             oc.fecha AS fechaCompra, 
                        //             oc.fecha_autoriza AS fechaAutorizacionCompra, 
                        //             ISNULL(CONVERT(varchar, oc.fecha_autoriza, 103), '') AS fechaAutorizacionCompraDesc, 
                        //             (CASE WHEN oc.ST_OC = 'A' THEN 'SI' ELSE 'NO' END) AS ordenCompraAutorizada, 
                        //             oc.proveedor AS proveedor, 
                        //             oc.bienes_servicios AS bienes_servicios, 
                        //             prov.nombre AS proveedorDesc, 
                        //             (CASE WHEN mov.fecha IS NOT NULL THEN mov.fecha WHEN movNoInv.fecha IS NOT NULL THEN movNoInv.fecha ELSE NULL END) AS fechaEntrada,
                        //             mov.fecha as fechaAutoriza,
                        //             req.fecha_Autoriza as fechaAutorizaRe, 
                        //             req.libre_abordo AS almacen, 
                        //             oc.cant_recibida AS compraCantidadRecibida 
                        //         FROM so_requisicion req 
                        //             INNER JOIN empleados empReq ON req.solicito = empReq.empleado 
                        //             INNER JOIN so_requisicion_det reqDet ON req.cc = reqDet.cc AND req.numero = reqDet.numero AND reqDet.cantidad > 0 
                        //             INNER JOIN insumos ins ON reqDet.insumo = ins.insumo 
                        //             INNER JOIN cc eco ON req.cc = eco.cc 
                        //             LEFT JOIN ( 
                        //                 SELECT 
                        //                     compra.cc, compra.numero, compra.comprador, compra.fecha, compra.fecha_autoriza, compra.proveedor, compra.bienes_servicios, compra.ST_OC, detalle.partida, detalle.num_requisicion, detalle.part_requisicion, detalle.cant_recibida 
                        //                 FROM so_orden_compra compra 
                        //                     INNER JOIN so_orden_compra_det detalle ON compra.cc = detalle.cc AND compra.numero = detalle.numero 
                        //                 WHERE compra.estatus != 'C' 
                        //             ) AS oc ON req.cc = oc.cc AND req.numero = oc.num_requisicion AND reqDet.partida = oc.part_requisicion 
                        //             LEFT JOIN empleados empComp ON oc.comprador = empComp.empleado 
                        //             LEFT JOIN so_tipo_requisicion tipoReq ON req.tipo_req_oc = tipoReq.tipo_req_oc 
                        //             LEFT JOIN sp_proveedores prov ON oc.proveedor = prov.numpro 
                        //             LEFT JOIN ( 
                        //                 SELECT 
                        //                 	mov.cc, mov.orden_ct, mov.fecha, det.partida_oc 
                        //                 FROM si_movimientos mov 
                        //                 	INNER JOIN si_movimientos_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.numero = det.numero 
                        //             ) AS mov ON oc.cc = mov.cc AND oc.numero = mov.orden_ct AND oc.partida = mov.partida_oc 
                        //             LEFT JOIN ( 
                        //                 SELECT 
                        //                 	mov.cc, mov.orden_ct, mov.fecha, det.partida_oc 
                        //                 FROM so_movimientos_noinv mov 
                        //                 	INNER JOIN so_movimientos_noinv_det det ON mov.almacen = det.almacen AND mov.tipo_mov = det.tipo_mov AND mov.remision = det.remision 
                        //             ) AS movNoInv ON oc.cc = movNoInv.cc AND oc.numero = movNoInv.orden_ct AND oc.partida = movNoInv.partida_oc 
                        //         WHERE {0} {1} req.fecha >= '{2}' AND req.fecha <= '{3}' " +
                        //                    (requisitor == 0 ? "" : " AND req.solicito = " + requisitor) + "{4}" + @" GROUP BY 
                        //             req.cc, req.solicito, empReq.descripcion, req.numero, req.fecha, eco.descripcion, ins.descripcion, oc.comprador, empComp.descripcion, 
                        //             oc.numero, oc.fecha, oc.fecha_autoriza, ordenCompraAutorizada, oc.bienes_servicios, oc.proveedor, prov.nombre, mov.fecha, movNoInv.fecha , req.fecha_Autoriza, req.libre_abordo, oc.cant_recibida, tipoReq.descripcion
                        //         ORDER BY req.numero, oc.numero", filtroCC, filtroTipoInsumo, fechaInicial.ToString("yyyyMMdd"), fechaFinal.ToString("yyyyMMdd"),
                        //                                        string.IsNullOrEmpty(whereRequisicionesPorComprador) ? "" : " AND (" + whereRequisicionesPorComprador + ")")
                        // );
                        var seguimientoEK = _context.Select<SeguimientoRequisicionDTO>(new DapperDTO
                        {
                            baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                            consulta = string.Format(@"
                                SELECT
                                    REQ.id AS reqId,
                                    REQ.cc AS reqCc,
                                    REQ.numero AS reqNumero,
                                    REQ.cc + '-' + CAST(REQ.numero AS VARCHAR) AS requisicion,
                                    REQ.PERU_tipoRequisicion AS reqPeruTipoRequisicion,
                                    REQ.solicito AS reqSolicito,
                                    REQ.comprador AS reqComprador,
                                    REQ.fecha AS reqFecha,
                                    REQ.autoriza AS reqAutoriza,
                                    REQ.idLibreAbordo AS reqLibreAbordo,
                                    REQDET.insumo AS reqInsumo,
                                    REQDET.cantOrdenada AS reqCantidadOrdenada,
                                    OC.id AS ocId,
                                    OC.cc AS ocCc,
                                    OC.numero AS ocNumero,
                                    OC.PERU_proveedor AS ocPeruProveedor,
                                    OC.fecha AS ocFecha,
                                    OC.fecha_autoriza AS ocFechaAutoriza,
                                    OC.bienes_servicios AS ocBienesServicios,
                                    OCDET.fecha_entrega AS ocDetFechaEntrega,
                                    U.nombre + ' ' + U.apellidoPaterno + ' ' + CASE WHEN U.apellidoMaterno IS NOT NULL THEN U.apellidoMaterno ELSE '' END AS solicitoNombreCompleto,
                                    PROV.nomcorto AS provedorNombre,
                                    ART.descripcion AS insumoNombre,
                                    UCOMPRADOR.nombreUsuario AS compradorUsuario,
                                    USUGERIDO.nombre + ' ' + USUGERIDO.apellidoPaterno + ' ' + CASE WHEN USUGERIDO.apellidoMaterno IS NOT NULL THEN USUGERIDO.apellidoMaterno ELSE '' END AS compradorSugerido,
                                    REQ.validadoAlmacen AS reqValidadoAlmacen,
                                    REQ.validadoCompras AS reqValidadoCompras,
                                    REQ.fechaValidacionAlmacen AS reqFechaValidacionAlmacen,
                                    REQ.consigna AS reqConsigna,
                                    REQ.licitacion AS reqLicitacion,
                                    REQ.crc AS reqCrc,
                                    REQ.convenio AS reqConvenio,
                                    OC.tiempoEntregaDias AS ocTiempoEntrega,
                                    OC.tiempoEntregaComentarios AS ocTiempoEntregaComentarios,
                                    OC.colocada AS ocColocada,
                                    OC.colocadaFecha AS ocColocadaFecha,
                                    CASE
                                        WHEN MOV.fecha IS NOT NULL THEN MOV.fecha
                                        ELSE NULL
                                    END AS fechaEntrada,
                                    MOV.fecha AS fechaAutoriza
                                FROM
                                    tblCom_Req AS REQ
                                INNER JOIN
                                    tblCom_ReqDet AS REQDET
                                    ON
                                        REQDET.idReq = REQ.id
                                LEFT JOIN
                                    tblCom_OrdenCompraDet AS OCDET
                                    ON
                                        OCDET.cc = REQ.cc AND
                                        OCDET.num_requisicion = REQ.numero AND
                                        OCDET.part_requisicion = REQDET.partida AND
                                        OCDET.estatusRegistro = 1
                                LEFT JOIN
                                    tblCom_OrdenCompra AS OC
                                    ON
                                        OC.id = OCDET.idOrdenCompra AND
                                        OC.estatusRegistro = 1
                                LEFT JOIN
                                    tblP_Usuario_Enkontrol AS UEK
                                    ON
                                        UEK.empleado = REQ.solicito
                                LEFT JOIN
                                    tblP_Usuario AS U
                                    ON
                                        U.id = UEK.idUsuario
                                LEFT JOIN
                                    tblCom_sp_proveedores AS PROV
                                    ON
                                        PROV.numpro = OC.proveedor
                                LEFT JOIN
                                    tblAlm_Insumo AS ART
                                    ON
                                        ART.insumo = REQDET.insumo
                                LEFT JOIN
                                    tblP_Usuario_Enkontrol AS UEKCOMPRADOR
                                    ON
                                        UEKCOMPRADOR.empleado = REQ.comprador
                                LEFT JOIN
                                    tblP_Usuario AS UCOMPRADOR
                                    ON
                                        UCOMPRADOR.id = UEKCOMPRADOR.idUsuario
                                LEFT JOIN
                                    tblP_Usuario_Enkontrol AS UEKSUGERIDO
                                    ON
                                        UEKSUGERIDO.empleado = REQ.comprador
                                LEFT JOIN
                                    tblP_Usuario AS USUGERIDO
                                    ON
                                        USUGERIDO.id = UEKSUGERIDO.idUsuario
                                LEFT JOIN
                                    (
                                        SELECT
                                            MOV.cc, MOV.orden_ct, MOV.fecha, MOVDET.partida_oc
                                        FROM
                                            tblAlm_Movimientos AS MOV
                                        INNER JOIN
                                            tblAlm_MovimientosDet AS MOVDET
                                            ON
                                                MOVDET.almacen = MOV.almacen AND
                                                MOVDET.tipo_mov = MOV.tipo_mov AND
                                                MOVDET.numero = MOV.numero AND
                                                MOVDET.estatusHabilitado = 1
                                        WHERE
                                            MOV.estatusHabilitado = 1
                                    ) AS MOV ON MOV.cc = OC.cc AND MOV.orden_ct = OC.numero AND MOV.partida_oc = OCDET.partida
                                WHERE
                                    REQ.estatusRegistro = 1 AND
                                    REQDET.estatusRegistro = 1 AND
                                    REQ.cc IN @paramCCs AND
                                    REQ.fecha BETWEEN @paramFechaInicial AND @paramFechaFinal
                                    {0}
                                ORDER BY
                                    REQ.PERU_tipoRequisicion,
                                    REQ.cc,
                                    REQ.numero,
                                    REQDET.partida
                                    ", requisitor == 0 ? "" : " AND REQ.comprador = @paramRequisitor"),
                            parametros = new
                            {
                                paramCCs = listaCC,
                                paramFechaInicial = fechaInicial.ToString("yyyy-MM-dd"),
                                paramFechaFinal = fechaFinal.ToString("yyyy-MM-dd"),
                                paramRequisitor = requisitor
                            }
                        });

                        if (seguimientoEK != null)
                        {
                            if (seguimientoEK != null)
                            {
                                foreach (var segui in seguimientoEK)
                                {

                                    var seguimientoPeru = new RequisicionSeguimientoDTO();
                                    seguimientoPeru.cc = segui.reqCc;
                                    seguimientoPeru.requisitor = segui.reqSolicito;
                                    seguimientoPeru.numeroRequisicion = segui.reqNumero;
                                    seguimientoPeru.requisitorDesc = segui.solicitoNombreCompleto;
                                    seguimientoPeru.fechaEntregaCompras = segui.ocDetFechaEntrega;
                                    seguimientoPeru.fechaElaboracion = segui.reqFecha;
                                    seguimientoPeru.tipoRequisicion = segui.reqPeruTipoRequisicion;
                                    seguimientoPeru.economico = "";
                                    seguimientoPeru.descripcion = segui.insumoNombre;
                                    seguimientoPeru.comprador = segui.reqComprador;
                                    seguimientoPeru.compradorDesc = segui.compradorUsuario;
                                    seguimientoPeru.numeroOrdenCompra = segui.ocNumero;
                                    seguimientoPeru.fechaCompra = segui.ocFecha;
                                    seguimientoPeru.fechaAutorizacionCompra = !segui.ocFechaAutoriza.HasValue || segui.ocFechaAutoriza.Value < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.ocFechaAutoriza.Value;
                                    seguimientoPeru.fechaAutorizacionCompraDesc = segui.ocFechaAutoriza.HasValue ? segui.ocFechaAutoriza.Value.ToString("dd/MM/yyyy") : "";
                                    seguimientoPeru.ordenCompraAutorizada = !segui.ocFecha.HasValue || segui.ocFecha.Value < new DateTime(2022, 1, 1) ? "NO" : "SI";
                                    seguimientoPeru.proveedorPeru = segui.ocPeruProveedor ?? "";
                                    seguimientoPeru.bienes_servicios = segui.ocBienesServicios ?? "";
                                    seguimientoPeru.proveedorDesc = segui.provedorNombre ?? "";
                                    seguimientoPeru.fechaAutorizaRe = segui.reqAutoriza < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.reqAutoriza;
                                    seguimientoPeru.almacen = segui.reqLibreAbordo;
                                    seguimientoPeru.compraCantidadRecibida = segui.reqDetCantidadOrdenada;
                                    seguimientoPeru.sugerido = segui.compradorSugerido;
                                    seguimientoPeru.requisicion = segui.reqCc + "-" + segui.reqNumero;
                                    seguimientoPeru.fechaElaboracionDesc = seguimientoPeru.fechaElaboracion.Value.ToShortDateString();
                                    seguimientoPeru.fechaEntregaComprasDesc = "";
                                    seguimientoPeru.sugeridoNum = segui.reqComprador;
                                    seguimientoPeru.validadoAlmacen = segui.reqValidadoAlmacen ?? false;
                                    seguimientoPeru.validadoCompras = segui.reqValidadoCompras;
                                    seguimientoPeru.almacen = segui.reqLibreAbordo;
                                    seguimientoPeru.fechaEntregaCompras = segui.reqFechaValidacionAlmacen;
                                    seguimientoPeru.fechaEntregaComprasDesc = segui.reqFechaValidacionAlmacen.HasValue ? segui.reqFechaValidacionAlmacen.Value.ToShortDateString() : "";
                                    seguimientoPeru.consigna = segui.reqConsigna.HasValue ? segui.reqConsigna.Value : false;
                                    seguimientoPeru.licitacion = segui.reqLicitacion;
                                    seguimientoPeru.crc = segui.reqCrc;
                                    seguimientoPeru.convenio = segui.reqConvenio;
                                    seguimientoPeru.fechaEntrada = segui.fechaEntrada;

                                    if (seguimientoPeru.numeroOrdenCompra != null && seguimientoPeru.numeroOrdenCompra > 0)
                                    {
                                        seguimientoPeru.tiempoEntregaDias = segui.ocTiempoEntrega;
                                        seguimientoPeru.tiempoEntregaComentarios = segui.ocTiempoEntregaComentarios;
                                        seguimientoPeru.colocada = segui.ocColocada;
                                        seguimientoPeru.colocadaFecha = segui.ocColocada ? segui.ocColocadaFecha : null;
                                    }
                                    else
                                    {
                                        seguimientoPeru.ordenCompraAutorizada = "";
                                    }

                                    seguimiento.Add(seguimientoPeru);

                                }
                            }

                            switch (estatus)
                            {
                                case 0:
                                    seguimientoFiltrado = seguimiento;
                                    break;
                                case 1:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                    break;
                                case 2:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                    break;
                            }

                            foreach (var seg in seguimientoFiltrado)
                            {
                                seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                seg.fechaEntregaComprasDesc = "";

                                string compradorSugerido = "";
                                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);

                                if (requisicionSIGOPLAN != null)
                                {
                                    compradorSugerido = (from emp in _context.tblP_Usuario_Enkontrol
                                                         join usu in _context.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == requisicionSIGOPLAN.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();
                                    seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;

                                    seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                    seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                    seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                    //La fecha de entrega compras es la fecha de validación de almacén.
                                    seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                    seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                    seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                    seg.licitacion = requisicionSIGOPLAN.licitacion;
                                    seg.crc = requisicionSIGOPLAN.crc;
                                    seg.convenio = requisicionSIGOPLAN.convenio;
                                }

                                seg.sugerido = compradorSugerido;

                                //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                //{
                                //    seg.fechaAutorizaRe = null;
                                //}

                                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4)
                                {
                                    seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                }

                                if (seg.numeroOrdenCompra > 0)
                                {
                                    seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                    var compraSIGOPLAN = _context.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                    if (compraSIGOPLAN != null)
                                    {
                                        seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                        seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                        seg.colocada = compraSIGOPLAN.colocada;
                                        if (seg.colocada)
                                        {
                                            seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                        }
                                    }
                                }
                                else
                                {
                                    seg.ordenCompraAutorizada = "";
                                }

                                seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                seg.entregaVencida = false;
                                if (seg.fechaEntrada == null)
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }

                                #region Columnas Niveles de Servicios
                                seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                #endregion
                            }
                        }
                    }
                    break;
                #endregion
            }
            return seguimientoFiltrado;
        }

        #region REPORTE TIEMPO DE PROCESO DE OC
        public List<RequisicionSeguimientoDTO> GetTiempoProcesoOC(List<string> listaCC, List<int> listaTipoInsumo, DateTime fechaInicial, DateTime fechaFinal, int estatus, int requisitor, List<string> claveProveedor)
        {
            List<RequisicionSeguimientoDTO> seguimiento = new List<RequisicionSeguimientoDTO>();
            List<RequisicionSeguimientoDTO> seguimientoFiltrado = new List<RequisicionSeguimientoDTO>();

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                #region EmpresaPeru
                case EmpresaEnum.Peru:
                    {
                        using (var ctxPeru = new MainContext())
                        {

                            var listaCentrosCosto = ctxPeru.tblP_CC.ToList();

                            //usuarios Starsoft
                            var consultaUsuariosStarsoft = @"SELECT cast(TCLAVE as int) as TCLAVE,TDESCRI FROM [003BDCOMUN].[dbo].[TABAYU] WHERE TCOD = '12' ";

                            List<InfoUsuariosStarsoftDTO> listaEmpleados = new List<InfoUsuariosStarsoftDTO>();
                            DynamicParameters lstParametros = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaEmpleados = conexion.Query<InfoUsuariosStarsoftDTO>(consultaUsuariosStarsoft, lstParametros, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            }

                            //Proveedores Starsoft

                            var consultaProveedoresStarsoft = @"SELECT prvccodigo,prvcnombre,prvcruc FROM [003BDCOMUN].[dbo].[MAEPROV] WHERE ISNULL(PRVCESTADO,'') = 'V' ";
                            List<infoProveedoresStarsoftDTO> listaProveedores = new List<infoProveedoresStarsoftDTO>();
                            DynamicParameters lstParametrosProv = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaProveedores = conexion.Query<infoProveedoresStarsoftDTO>(consultaProveedoresStarsoft, lstParametrosProv, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };

                            //Insumos Starsoft
                            var consultaInsumosStarsoft = @"SELECT ACODIGO,ADESCRI,AUNIDAD FROM [003BDCOMUN].[dbo].[MAEART]";
                            List<infoInsumosStarsoftDTO> listaInsumos = new List<infoInsumosStarsoftDTO>();
                            DynamicParameters lstParametrosInsumo = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaInsumos = conexion.Query<infoInsumosStarsoftDTO>(consultaInsumosStarsoft, lstParametrosInsumo, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            //Almacen Starsoft
                            var consultaAlmacenStarsoft = @"SELECT STALMA AS Almacen,TADESCRI AS Descripción,CONVERT(numeric(11,2),STSKDIS) AS stock FROM [003BDCOMUN].[dbo].[STKART] s INNER JOIN [003BDCOMUN].[dbo].[TABALM]t ON s.STALMA=t.TAALMA";
                            List<infoAlmacenStarsoftDTO> listaAlmacen = new List<infoAlmacenStarsoftDTO>();
                            DynamicParameters lstParametrosAlmacen = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaAlmacen = conexion.Query<infoAlmacenStarsoftDTO>(consultaAlmacenStarsoft, lstParametrosAlmacen, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            var consultaAuditoriaStarsoft = @"SELECT COD_AUDITORIA,OBSERVACION FROM [BDWENCO].[dbo].[AUDITORIA_SISTEMAS]";
                            List<infoAuditoriaStarsoftDTO> listaAuditoria = new List<infoAuditoriaStarsoftDTO>();
                            DynamicParameters lstParametrosAuditoria = new DynamicParameters();
                            using (var conexion = new SqlConnection(ConextSigoDapper.conexionStarsoftBancos()))
                            {
                                conexion.Open();
                                listaAuditoria = conexion.Query<infoAuditoriaStarsoftDTO>(consultaAuditoriaStarsoft, lstParametrosAuditoria, null, true, 300, commandType: CommandType.Text).ToList();
                                conexion.Close();
                            };


                            if (listaCC == null)
                            {
                                listaCC = new List<string>();
                            }
                            var seguimientoReqPeru = ctxPeru.tblCom_Req.Where(re => listaCC.Contains(re.cc)).ToList().Where(requi =>
                                (requi.fecha >= fechaInicial && requi.fecha <= fechaFinal) &&
                                 (requisitor > 0 ? requi.comprador == requisitor : true)).ToList();



                            //var estatusVencido = "";
                            //var tipoRequisicionDesc = "";
                            if (seguimientoReqPeru != null)
                            {
                                foreach (var segui in seguimientoReqPeru)
                                {
                                    var requisicionDet = ctxPeru.tblCom_ReqDet.Where(rd => rd.idReq == segui.id && rd.estatusRegistro).FirstOrDefault() ?? new tblCom_ReqDet();
                                    var ordenCompra = ctxPeru.Select<tblCom_OrdenCompra>(new DapperDTO
                                    {
                                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                                        consulta = "SELECT com.* FROM tblCom_OrdenCompra AS com INNER JOIN tblCom_OrdenCompraDet AS det ON com.id = det.idOrdenCompra INNER JOIN tblCom_Req AS r ON r.numero = det.num_requisicion AND r.PERU_tipoRequisicion = @paramTipoCompra AND r.estatusRegistro = 1 WHERE det.estatusRegistro = 1 AND com.estatusRegistro = 1 AND det.cc = @paramCC AND det.num_requisicion = @paramNumRequi AND com.PERU_tipoCompra = @paramTipoCompra",
                                        parametros = new { paramCC = segui.cc, paramNumRequi = segui.numero, paramTipoCompra = segui.PERU_tipoRequisicion }
                                    }).FirstOrDefault();
                                    var idCompra = ordenCompra != null ? ordenCompra.id : 0;
                                    var ordenCompraDet = ctxPeru.tblCom_OrdenCompraDet.Where(ocDet => ocDet.idOrdenCompra == idCompra).FirstOrDefault() ?? new tblCom_OrdenCompraDet();
                                    //var ordenCompra = ctxPeru.tblCom_OrdenCompra.Where(oc => oc.id == ordenCompraDet.idOrdenCompra).FirstOrDefault() ?? new tblCom_OrdenCompra();

                                    //var solicito = listaEmpleados.FirstOrDefault(x => x.TCLAVE == segui.solicito);
                                    var solicito = "";
                                    var solicitoEK = ctxPeru.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == segui.solicito);
                                    if (solicitoEK != null)
                                    {
                                        var solicitoSP = ctxPeru.tblP_Usuario.FirstOrDefault(x => x.id == solicitoEK.idUsuario);
                                        if (solicitoSP != null)
                                        {
                                            solicito = PersonalUtilities.NombreCompletoMayusculas(solicitoSP.nombre, solicitoSP.apellidoPaterno, solicitoSP.apellidoMaterno);
                                        }
                                    }
                                    var solicitoDescr = "";
                                    var provDescr = "";
                                    var insumoDescr = "";
                                    if (solicito != null)
                                    {
                                        //solicitoDescr = (string)solicito.TDESCRI;
                                        solicitoDescr = solicito;
                                    }

                                    var proNombre = "";
                                    if (ordenCompra != null)
                                    {
                                        var descProvvedor = listaProveedores.FirstOrDefault(x => x.prvccodigo == ordenCompra.PERU_proveedor);
                                        if (descProvvedor != null)
                                        {
                                            proNombre = descProvvedor.prvcnombre;
                                        }
                                    }

                                    provDescr = proNombre;

                                    var descInsumo = listaInsumos.FirstOrDefault(x => x.ACODIGO == "0" + requisicionDet.insumo.ToString());
                                    if (descInsumo != null)
                                    {
                                        insumoDescr = (string)descInsumo.ADESCRI;
                                    }

                                    var compradorDesc = (from emp in ctxPeru.tblP_Usuario_Enkontrol
                                                         join usu in ctxPeru.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == segui.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();

                                    var comprador = "";
                                    var compradorEk = ctxPeru.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.empleado == segui.comprador);
                                    if (compradorEk != null)
                                    {
                                        var compradorSP = ctxPeru.tblP_Usuario.FirstOrDefault(x => x.id == compradorEk.idUsuario);
                                        if (compradorSP != null)
                                        {
                                            comprador = PersonalUtilities.NombreCompletoMayusculas(compradorSP.nombre, compradorSP.apellidoPaterno, compradorSP.apellidoMaterno);
                                        }
                                    }


                                    var seguimientoPeru = new RequisicionSeguimientoDTO
                                    {

                                        cc = segui.cc,
                                        requisitor = segui.solicito,
                                        numeroRequisicion = segui.numero,
                                        requisitorDesc = solicitoDescr,
                                        fechaEntregaCompras = ordenCompraDet.fecha_entrega,
                                        fechaElaboracion = segui.fecha,
                                        tipoRequisicion = segui.PERU_tipoRequisicion,
                                        economico = "",
                                        descripcion = insumoDescr,
                                        comprador = segui.comprador,
                                        compradorDesc = compradorDesc,
                                        numeroOrdenCompra = ordenCompraDet.numero,
                                        fechaCompra = ordenCompra == null || ordenCompra.fecha < new DateTime(2022, 1, 1) ? (DateTime?)null : ordenCompra.fecha,
                                        fechaAutorizacionCompra = ordenCompra != null ? ordenCompra.fecha_autoriza : (DateTime?)null,
                                        fechaAutorizacionCompraDesc = ordenCompra != null && ordenCompra.fecha_autoriza.HasValue ? ordenCompra.fecha_autoriza.Value.ToString("dd/MM/yyyy") : "",
                                        ordenCompraAutorizada = ordenCompra == null || ordenCompra.fecha < new DateTime(2022, 1, 1) ? "NO" : "SI",
                                        proveedorPeru = ordenCompra != null ? ordenCompra.PERU_proveedor : "",
                                        bienes_servicios = ordenCompra != null ? ordenCompra.bienes_servicios : "",
                                        proveedorDesc = provDescr,
                                        fechaAutorizaRe = segui.autoriza < new DateTime(2022, 1, 1) ? (DateTime?)null : segui.autoriza,
                                        almacen = segui.idLibreAbordo,
                                        compraCantidadRecibida = requisicionDet.cantOrdenada,
                                        sugerido = comprador
                                    };

                                    if (claveProveedor != null && claveProveedor.Count() > 0)
                                    {
                                        if (ordenCompra != null && claveProveedor.Contains(ordenCompra.PERU_proveedor))
                                        {
                                            seguimiento.Add(seguimientoPeru);

                                        }

                                    }
                                    else
                                    {
                                        seguimiento.Add(seguimientoPeru);

                                    }
                                }
                            }

                            if (seguimiento != null)
                            {
                                switch (estatus)
                                {
                                    case 0:
                                        seguimientoFiltrado = seguimiento;
                                        break;
                                    case 1:
                                        seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                        break;
                                    case 2:
                                        seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                        break;
                                }


                                foreach (var seg in seguimientoFiltrado)
                                {
                                    seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                    seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                    seg.fechaEntregaComprasDesc = "";

                                    string compradorSugerido = "";
                                    var requisicionSIGOPLAN = ctxPeru.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);

                                    if (requisicionSIGOPLAN != null)
                                    {
                                        compradorSugerido = (from emp in ctxPeru.tblP_Usuario_Enkontrol
                                                             join usu in ctxPeru.tblP_Usuario
                                                             on emp.idUsuario equals usu.id
                                                             where emp.empleado == requisicionSIGOPLAN.comprador
                                                             select usu.nombreUsuario).FirstOrDefault();
                                        seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;

                                        seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                        seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                        seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                        //La fecha de entrega compras es la fecha de validación de almacén.
                                        seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                        seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                        seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                        seg.licitacion = requisicionSIGOPLAN.licitacion;
                                        seg.crc = requisicionSIGOPLAN.crc;
                                        seg.convenio = requisicionSIGOPLAN.convenio;
                                    }

                                    seg.sugerido = compradorSugerido;

                                    //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                    //{
                                    //    seg.fechaAutorizaRe = null;
                                    //}

                                    if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4)
                                    {
                                        seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                    }

                                    if (seg.numeroOrdenCompra > 0)
                                    {
                                        seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                        var compraSIGOPLAN = ctxPeru.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                        if (compraSIGOPLAN != null)
                                        {
                                            seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                            seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                            seg.colocada = compraSIGOPLAN.colocada;
                                            if (seg.colocada)
                                            {
                                                seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        seg.ordenCompraAutorizada = "";
                                    }

                                    seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                    seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                    seg.entregaVencida = false;
                                    if (seg.fechaEntrada == null)
                                    {
                                        if (seg.colocada == true)
                                        {
                                            seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                            var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                            var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                            if (diasPasados > 0)
                                            {
                                                seg.entregaVencida = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (seg.colocada == true)
                                        {
                                            seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                            var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                            var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                            if (diasPasados > 0)
                                            {
                                                seg.entregaVencida = true;
                                            }
                                        }
                                    }

                                    #region Columnas Niveles de Servicios
                                    seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                    seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                    seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                    seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                    #endregion
                                }
                            }
                        }
                    }

                    break;
                #endregion

                #region Empresa Colombia
                case EmpresaEnum.Colombia:
                    {
                        var stringListaCC = listaCC != null && listaCC.Count > 0 ? string.Join(", ", listaCC.Select(x => "'" + x + "'")) : "";
                        var stringListaTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Join(", ", listaTipoInsumo.Select(x => "'" + x + "'")) : "";

                        var filtroCC = listaCC != null && listaCC.Count > 0 ? string.Format(@"req.cc IN ({0}) AND ", stringListaCC) : "";
                        var filtroTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Format(@"SUBSTRING(CONVERT(varchar(10), reqDet.insumo), 1, 1) IN ({0}) AND ", stringListaTipoInsumo) : "";

                        string filtroProveedor = claveProveedor != null && claveProveedor.Count() > 0 ? ("oc.proveedor IN (" + string.Join(", ", claveProveedor.Select(x => "'" + x + "'")) + ") AND ") : "";

                        var seguimientoEK = consultaCheckProductivo(
                            string.Format(@"
                               SELECT 
                                    req.cc AS cc, 
                                    req.solicito AS requisitor, 
                                    empReq.descripcion AS requisitorDesc, 
                                    req.numero AS numeroRequisicion, 
                                    req.fecha AS fechaElaboracion, 
                                    --'' AS fechaEntregaCompras, 
                                    Cast(req.tipo_req_oc as int) AS idTipoRequisicion, 
                                    eco.descripcion AS economico, 
                                    oc.comprador AS comprador, 
                                    empComp.descripcion AS compradorDesc, 
                                    oc.numero AS numeroOrdenCompra, 
                                    oc.fecha AS fechaCompra, 
                                    oc.fecha_autoriza AS fechaAutorizacionCompra, 
                                    ISNULL(CONVERT(varchar, oc.fecha_autoriza, 103), '') AS fechaAutorizacionCompraDesc, 
                                    (CASE WHEN (oc.vobo_aut = 'S' OR oc.aut_aut = 'S') THEN 'SI' ELSE 'NO' END) AS ordenCompraAutorizada, 
                                    (CASE WHEN mov.fecha IS NOT NULL THEN mov.fecha WHEN movNoInv.fecha IS NOT NULL THEN movNoInv.fecha ELSE NULL END) AS fechaEntrada,
                                    mov.fecha as fechaAutoriza,
                                    oc.proveedor AS proveedor, 
                                    oc.bienes_servicios AS bienes_servicios, 
                                    prov.nombre AS proveedorDesc, 
                                    req.fecha_Autoriza as fechaAutorizaRe, 
                                    req.libre_abordo AS almacen
                                FROM DBA.so_requisicion req 
                                    INNER JOIN DBA.empleados empReq ON req.solicito = empReq.empleado 
                                    INNER JOIN DBA.cc eco ON req.cc = eco.cc 
                                    INNER JOIN DBA.so_requisicion_det reqDet ON req.cc = reqDet.cc AND req.numero = reqDet.numero AND reqDet.cantidad > 0 
                                    LEFT JOIN ( 
                                        SELECT 
                                            compra.cc, compra.numero, compra.comprador, compra.fecha, compra.fecha_autoriza, compra.vobo_aut, compra.aut_aut, compra.proveedor, compra.bienes_servicios, detalle.num_requisicion, detalle.part_requisicion
                                        FROM DBA.so_orden_compra compra 
                                            INNER JOIN DBA.so_orden_compra_det detalle ON compra.cc = detalle.cc AND compra.numero = detalle.numero 
                                        WHERE compra.estatus != 'C' 
                                    ) AS oc ON req.cc = oc.cc AND req.numero = oc.num_requisicion AND reqDet.partida = oc.part_requisicion 
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha
                                        FROM DBA.si_movimientos mov 
                                    ) AS mov ON oc.cc = mov.cc AND oc.numero = mov.orden_ct
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha
                                        FROM DBA.so_movimientos_noinv mov 
                                    ) AS movNoInv ON oc.cc = movNoInv.cc AND oc.numero = movNoInv.orden_ct
                                    LEFT JOIN DBA.empleados empComp ON oc.comprador = empComp.empleado 
                                    LEFT JOIN DBA.sp_proveedores prov ON oc.proveedor = prov.numpro    
                                WHERE {0} {1} {4} req.fecha >= '{2}' AND req.fecha <= '{3}' " +
                                            (requisitor == 0 ? "" : " AND req.solicito = " + requisitor) + @" 
                                GROUP BY 
                                    req.cc, req.solicito, empReq.descripcion, req.numero, req.fecha, eco.descripcion, oc.comprador, empComp.descripcion, 
                                    oc.numero, oc.fecha, oc.fecha_autoriza, ordenCompraAutorizada, oc.bienes_servicios, oc.proveedor, prov.nombre, req.fecha_Autoriza, mov.fecha, movNoInv.fecha, req.libre_abordo, req.tipo_req_oc
                                ORDER BY req.numero, oc.numero", filtroCC, filtroTipoInsumo, fechaInicial.ToString("yyyyMMdd"), fechaFinal.ToString("yyyyMMdd"), filtroProveedor)
                        );


                        if (seguimientoEK != null)
                        {
                            seguimiento = ((List<RequisicionSeguimientoDTO>)seguimientoEK.ToObject<List<RequisicionSeguimientoDTO>>())
                                //.Where(x =>
                                //    x.fechaEntrada == null || DateTime.Now.Subtract((DateTime)x.fechaEntrada).Days <= 5
                                //)
                            .ToList();

                            switch (estatus)
                            {
                                case 0:
                                    seguimientoFiltrado = seguimiento;
                                    break;
                                case 1:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                    break;
                                case 2:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                    break;
                            }

                            foreach (var seg in seguimientoFiltrado)
                            {
                                seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                seg.fechaEntregaComprasDesc = "";

                                string compradorSugerido = "";
                                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);
                                var tipoReqDesc = _context.tblCom_ReqTipo.FirstOrDefault(x => x.tipo_req_oc == seg.idTipoRequisicion);
                                if (requisicionSIGOPLAN != null)
                                {
                                    compradorSugerido = (from emp in _context.tblP_Usuario_Enkontrol
                                                         join usu in _context.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == requisicionSIGOPLAN.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();
                                    seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;
                                    seg.tipoRequisicion = tipoReqDesc.descripcion ?? "";
                                    seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                    seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                    seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                    //La fecha de entrega compras es la fecha de validación de almacén.
                                    seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                    seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                    seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                    seg.licitacion = requisicionSIGOPLAN.licitacion;
                                    seg.crc = requisicionSIGOPLAN.crc;
                                    seg.convenio = requisicionSIGOPLAN.convenio;
                                }

                                seg.sugerido = compradorSugerido;

                                //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                //{
                                //    seg.fechaAutorizaRe = null;
                                //}

                                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4)
                                {
                                    seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                }

                                if (seg.numeroOrdenCompra > 0)
                                {
                                    seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                    var compraSIGOPLAN = _context.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                    if (compraSIGOPLAN != null)
                                    {
                                        seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                        seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                        seg.colocada = compraSIGOPLAN.colocada;
                                        if (seg.colocada)
                                        {
                                            seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                        }
                                    }
                                }
                                else
                                {
                                    seg.ordenCompraAutorizada = "";
                                }

                                seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                seg.entregaVencida = false;
                                if (seg.fechaEntrada == null)
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }

                                #region Columnas Niveles de Servicios
                                seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                #endregion
                            }
                        }
                    }

                    break;
                #endregion

                #region EmpresaConstruplan
                default:
                    {
                        var stringListaCC = listaCC != null && listaCC.Count > 0 ? string.Join(", ", listaCC.Select(x => "'" + x + "'")) : "";
                        var stringListaTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Join(", ", listaTipoInsumo.Select(x => "'" + x + "'")) : "";

                        var filtroCC = listaCC != null && listaCC.Count > 0 ? string.Format(@"req.cc IN ({0}) AND ", stringListaCC) : "";
                        var filtroTipoInsumo = listaTipoInsumo != null && listaTipoInsumo.Count > 0 ? string.Format(@"SUBSTRING(CONVERT(varchar(10), reqDet.insumo), 1, 1) IN ({0}) AND ", stringListaTipoInsumo) : "";

                        string filtroProveedor = claveProveedor != null && claveProveedor.Count() > 0 ? ("oc.proveedor IN (" + string.Join(", ", claveProveedor.Select(x => "'" + x + "'")) + ") AND ") : "";

                        var seguimientoEK = consultaCheckProductivo(
                            string.Format(@"
                                SELECT 
                                    req.cc AS cc, 
                                    req.solicito AS requisitor, 
                                    empReq.descripcion AS requisitorDesc, 
                                    req.numero AS numeroRequisicion, 
                                    req.fecha AS fechaElaboracion, 
                                    --'' AS fechaEntregaCompras, 
                                    tipoReq.descripcion AS tipoRequisicion, 
                                    eco.descripcion AS economico, 
                                    oc.comprador AS comprador, 
                                    empComp.descripcion AS compradorDesc, 
                                    oc.numero AS numeroOrdenCompra, 
                                    oc.fecha AS fechaCompra, 
                                    oc.fecha_autoriza AS fechaAutorizacionCompra, 
                                    ISNULL(CONVERT(varchar, oc.fecha_autoriza, 103), '') AS fechaAutorizacionCompraDesc, 
                                    (CASE WHEN oc.ST_OC = 'A' THEN 'SI' ELSE 'NO' END) AS ordenCompraAutorizada, 
                                    (CASE WHEN mov.fecha IS NOT NULL THEN mov.fecha WHEN movNoInv.fecha IS NOT NULL THEN movNoInv.fecha ELSE NULL END) AS fechaEntrada,
                                    mov.fecha as fechaAutoriza,
                                    oc.proveedor AS proveedor, 
                                    oc.bienes_servicios AS bienes_servicios, 
                                    prov.nombre AS proveedorDesc, 
                                    req.fecha_Autoriza as fechaAutorizaRe, 
                                    req.libre_abordo AS almacen
                                FROM so_requisicion req 
                                    INNER JOIN empleados empReq ON req.solicito = empReq.empleado 
                                    INNER JOIN cc eco ON req.cc = eco.cc 
                                    INNER JOIN so_requisicion_det reqDet ON req.cc = reqDet.cc AND req.numero = reqDet.numero AND reqDet.cantidad > 0 
                                    LEFT JOIN ( 
                                        SELECT 
                                            compra.cc, compra.numero, compra.comprador, compra.fecha, compra.fecha_autoriza, compra.proveedor, compra.bienes_servicios, compra.ST_OC, detalle.num_requisicion, detalle.part_requisicion
                                        FROM so_orden_compra compra 
                                            INNER JOIN so_orden_compra_det detalle ON compra.cc = detalle.cc AND compra.numero = detalle.numero 
                                        WHERE compra.estatus != 'C' 
                                    ) AS oc ON req.cc = oc.cc AND req.numero = oc.num_requisicion AND reqDet.partida = oc.part_requisicion 
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha
                                        FROM si_movimientos mov 
                                    ) AS mov ON oc.cc = mov.cc AND oc.numero = mov.orden_ct
                                    LEFT JOIN ( 
                                        SELECT 
                                        	mov.cc, mov.orden_ct, mov.fecha
                                        FROM so_movimientos_noinv mov 
                                    ) AS movNoInv ON oc.cc = movNoInv.cc AND oc.numero = movNoInv.orden_ct
                                    LEFT JOIN empleados empComp ON oc.comprador = empComp.empleado 
                                    LEFT JOIN so_tipo_requisicion tipoReq ON req.tipo_req_oc = tipoReq.tipo_req_oc 
                                    LEFT JOIN sp_proveedores prov ON oc.proveedor = prov.numpro    
                                WHERE {0} {1} {4} req.fecha >= '{2}' AND req.fecha <= '{3}' " +
                                            (requisitor == 0 ? "" : " AND req.solicito = " + requisitor) + @" 
                                GROUP BY 
                                    req.cc, req.solicito, empReq.descripcion, req.numero, req.fecha, eco.descripcion, oc.comprador, empComp.descripcion, 
                                    oc.numero, oc.fecha, oc.fecha_autoriza, ordenCompraAutorizada, oc.bienes_servicios, oc.proveedor, prov.nombre, req.fecha_Autoriza, mov.fecha, movNoInv.fecha, req.libre_abordo, tipoReq.descripcion 
                                ORDER BY req.numero, oc.numero", filtroCC, filtroTipoInsumo, fechaInicial.ToString("yyyyMMdd"), fechaFinal.ToString("yyyyMMdd"), filtroProveedor)
                        );

                        if (seguimientoEK != null)
                        {
                            seguimiento = ((List<RequisicionSeguimientoDTO>)seguimientoEK.ToObject<List<RequisicionSeguimientoDTO>>())
                                //.Where(x =>
                                //    x.fechaEntrada == null || DateTime.Now.Subtract((DateTime)x.fechaEntrada).Days <= 5
                                //)
                            .ToList();

                            switch (estatus)
                            {
                                case 0:
                                    seguimientoFiltrado = seguimiento;
                                    break;
                                case 1:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra != null).ToList();
                                    break;
                                case 2:
                                    seguimientoFiltrado = seguimiento.Where(x => x.numeroOrdenCompra == null).ToList();
                                    break;
                            }

                            foreach (var seg in seguimientoFiltrado)
                            {
                                seg.requisicion = seg.cc + "-" + seg.numeroRequisicion;
                                seg.fechaElaboracionDesc = ((DateTime)seg.fechaElaboracion).ToShortDateString();
                                seg.fechaEntregaComprasDesc = "";

                                string compradorSugerido = "";
                                var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroRequisicion);

                                if (requisicionSIGOPLAN != null)
                                {
                                    compradorSugerido = (from emp in _context.tblP_Usuario_Enkontrol
                                                         join usu in _context.tblP_Usuario
                                                         on emp.idUsuario equals usu.id
                                                         where emp.empleado == requisicionSIGOPLAN.comprador
                                                         select usu.nombreUsuario).FirstOrDefault();
                                    seg.sugeridoNum = requisicionSIGOPLAN.comprador ?? 0;

                                    seg.validadoAlmacen = requisicionSIGOPLAN.validadoAlmacen ?? false;
                                    seg.validadoCompras = requisicionSIGOPLAN.validadoCompras;
                                    seg.almacen = requisicionSIGOPLAN.idLibreAbordo;

                                    //La fecha de entrega compras es la fecha de validación de almacén.
                                    seg.fechaEntregaCompras = requisicionSIGOPLAN.fechaValidacionAlmacen;
                                    seg.fechaEntregaComprasDesc = requisicionSIGOPLAN.fechaValidacionAlmacen != null ? ((DateTime)requisicionSIGOPLAN.fechaValidacionAlmacen).ToShortDateString() : "";
                                    seg.consigna = requisicionSIGOPLAN.consigna != null ? (bool)requisicionSIGOPLAN.consigna : false;
                                    seg.licitacion = requisicionSIGOPLAN.licitacion;
                                    seg.crc = requisicionSIGOPLAN.crc;
                                    seg.convenio = requisicionSIGOPLAN.convenio;
                                }

                                seg.sugerido = compradorSugerido;

                                //if (seg.fechaAutorizaRe != null && ((DateTime)seg.fechaAutorizaRe).Year <= 1900)
                                //{
                                //    seg.fechaAutorizaRe = null;
                                //}

                                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 4 || vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                                {
                                    seg.economico = ""; //Se quita el texto del económico en Construplan porque no es la máquina.
                                }

                                if (seg.numeroOrdenCompra > 0)
                                {
                                    seg.ordenCompra = seg.cc + "-" + seg.numeroOrdenCompra;

                                    var compraSIGOPLAN = _context.tblCom_OrdenCompra.FirstOrDefault(x => x.estatusRegistro && x.cc == seg.cc && x.numero == seg.numeroOrdenCompra);

                                    if (compraSIGOPLAN != null)
                                    {
                                        seg.tiempoEntregaDias = compraSIGOPLAN.tiempoEntregaDias;
                                        seg.tiempoEntregaComentarios = compraSIGOPLAN.tiempoEntregaComentarios;
                                        seg.colocada = compraSIGOPLAN.colocada;
                                        if (seg.colocada)
                                        {
                                            seg.colocadaFecha = compraSIGOPLAN.colocadaFecha;
                                        }
                                    }
                                }
                                else
                                {
                                    seg.ordenCompraAutorizada = "";
                                }

                                seg.tiempoEntregaDiasDesc = seg.tiempoEntregaDias > 0 ? seg.tiempoEntregaDias + " días." : "";
                                seg.fechaEntradaDesc = seg.fechaEntrada != null ? ((DateTime)seg.fechaEntrada).ToShortDateString() : "";
                                seg.entregaVencida = false;
                                if (seg.fechaEntrada == null)
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (DateTime.Now - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (seg.colocada == true)
                                    {
                                        seg.colocadaFechaStr = ((DateTime)seg.colocadaFecha).ToShortDateString();
                                        var fechaEstimada = ((DateTime)seg.colocadaFecha).AddDays(seg.tiempoEntregaDias);
                                        var diasPasados = (((DateTime)seg.fechaEntrada) - fechaEstimada).TotalDays;
                                        if (diasPasados > 0)
                                        {
                                            seg.entregaVencida = true;
                                        }
                                    }
                                }

                                #region Columnas Niveles de Servicios
                                seg.nivelReqAutReq = seg.fechaAutorizaRe != null ? (((DateTime)seg.fechaAutorizaRe).Date - ((DateTime)seg.fechaElaboracion).Date).TotalDays : 0;
                                seg.nivelAutReqOC = (seg.fechaCompra != null && seg.fechaAutorizaRe != null) ? (((DateTime)seg.fechaCompra).Date - ((DateTime)seg.fechaAutorizaRe).Date).TotalDays : 0;
                                seg.nivelOCAutOC = (seg.fechaCompra != null && seg.fechaAutorizacionCompra != null) ? (((DateTime)seg.fechaAutorizacionCompra).Date - ((DateTime)seg.fechaCompra).Date).TotalDays : 0;
                                seg.nivelAutOCEnt = (seg.fechaAutorizacionCompra != null && seg.fechaEntrada != null) ? (((DateTime)seg.fechaEntrada).Date - ((DateTime)seg.fechaAutorizacionCompra).Date).TotalDays : 0;
                                #endregion
                            }
                        }
                    }
                    break;
                #endregion
            }
            return seguimientoFiltrado;
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> FillComboProveedoresReporteProcesoOC()
        {
            try
            {
                if (vSesiones.sesionEmpresaActual == 6)
                {
                    using (var dbStartSoft = new MainContextPeruStarSoft003BDCOMUN())
                    {
                        var lstProveedoresPeru = dbStartSoft.MAEPROV.Where(e => e.PRVCESTADO == "V").Select(e => new Core.DTO.Principal.Generales.ComboDTO
                        {
                            Value = e.PRVCCODIGO,
                            Text = e.PRVCNOMBRE
                        }).ToList();

                        return lstProveedoresPeru;

                    }
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    return _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(getEnkontrolEnumADM(), string.Format(@"
                    SELECT
                        CAST(numpro AS varchar) AS Value, (CAST(numpro AS varchar) + ' - ' + nombre) AS Text
                    FROM DBA.sp_proveedores
                    ORDER BY numpro"));
                }
                else
                {
                    return _contextEnkontrol.Select<Core.DTO.Principal.Generales.ComboDTO>(getEnkontrolEnumADM(), string.Format(@"
                    SELECT
                        CAST(numpro AS varchar) AS Value, (CAST(numpro AS varchar) + ' - ' + nombre) AS Text
                    FROM sp_proveedores
                    ORDER BY numpro"));
                }

            }
            catch (Exception)
            {
                return new List<Core.DTO.Principal.Generales.ComboDTO>();
            }
        }
        #endregion

        public Dictionary<string, object> cancelarValidado(string cc, int numero)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == numero);
                    var checkComprasExistentes = _context.tblCom_OrdenCompraDet.Where(x => x.estatusRegistro && x.cc == cc && x.num_requisicion == numero).ToList();
                    var checkMovimientosExistentes = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.cc == cc && x.numeroReq == numero).ToList();

                    if (requisicionSIGOPLAN == null)
                    {
                        throw new Exception("No se encuentra la información de la requisición.");
                    }

                    if (checkComprasExistentes.Count > 0)
                    {
                        throw new Exception("No se puede cancelar la validación. Ya existen compras relacionadas a la requisición.");
                    }

                    if (checkMovimientosExistentes.Count > 0)
                    {
                        throw new Exception("No se puede cancelar la validación. Ya existen movimientos relacionados a la requisición.");
                    }

                    requisicionSIGOPLAN.validadoAlmacen = false;
                    requisicionSIGOPLAN.validadoRequisitor = true; //Se salta el paso de validación del requisitor.
                    requisicionSIGOPLAN.fechaValidacionAlmacen = null;

                    _context.SaveChanges();

                    var listaSurtido = _context.tblCom_Surtido.Where(x => x.estatus && x.cc == cc && x.numero == numero).ToList();
                    var listaSurtidoDet = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.cc == cc && x.numero == numero).ToList();

                    foreach (var sur in listaSurtido)
                    {
                        sur.estatus = false;
                        _context.SaveChanges();
                    }

                    foreach (var surDet in listaSurtidoDet)
                    {
                        surDet.estatus = false;
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

        public List<SurtidoRequisicionDTO> getReporteSurtidoRequisicion(string cc, int numero)
        {
            var almacenes = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM si_almacen")).ToObject<List<dynamic>>();
            var centrosCosto = (List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM cc")).ToObject<List<dynamic>>();
            var surtidoDet = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.cc == cc && x.numero == numero).ToList();
            var stringInsumos = string.Join(", ", surtidoDet.Select(x => x.insumo).ToList());

            List<dynamic> insumos = new List<dynamic>();

            if (surtidoDet.Count > 0)
            {
                var insumosEK = consultaCheckProductivo(string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", stringInsumos));
                insumos = (List<dynamic>)insumosEK.ToObject<List<dynamic>>();
            }

            var data = surtidoDet.Select(x => new SurtidoRequisicionDTO
            {
                centroCosto = centrosCosto.Where(y => (string)y.cc == x.cc).Select(z => (string)z.cc + "-" + (string)z.descripcion).FirstOrDefault(),
                numero = x.numero.ToString(),
                insumo = insumos.Where(y => (int)y.insumo == x.insumo).Select(z => (int)z.insumo + "-" + (string)z.descripcion).FirstOrDefault(),
                cantidad = x.cantidad.ToString(),
                almacen = almacenes.Where(y => (int)y.almacen == x.almacenOrigenID).Select(z => (int)z.almacen + "-" + (string)z.descripcion).FirstOrDefault(),
                area_alm = x.area_alm,
                lado_alm = x.lado_alm,
                estante_alm = x.estante_alm,
                nivel_alm = x.nivel_alm
            }).ToList();

            return data;
        }

        public bool checkMaquinaStandBy(string cc)
        {
            var flagMaquinaStandBy = false;
            var ccEK = ((List<dynamic>)consultaCheckProductivo(string.Format(@"SELECT * FROM cc WHERE cc = '{0}'", cc)).ToObject<List<dynamic>>())[0];
            var catMaquina = _context.tblM_CatMaquina.ToList().FirstOrDefault(x => x.noEconomico == (string)ccEK.descripcion);

            if (catMaquina != null)
            {
                if (catMaquina.estatus == 2) //Estatus en Stand-By.
                {
                    var listaMaquinasBloqueoStandby = _context.tblM_STB_EconomicoBloqueado.Where(x => x.registroActivo).Select(x => x.noEconomico).ToList();
                    if (listaMaquinasBloqueoStandby.Contains(catMaquina.noEconomico)) throw new Exception("No es posible realizar la acción puesto que el equipo referenciado se encuentra en estatus StandBy");

                    flagMaquinaStandBy = true;

                    catMaquina.estatus = 1; //Se quita el estado Stand-By de la máquina.
                    _context.SaveChanges();

                    #region Modificar la tabla "tblM_STB_CapturaStandBy"
                    try
                    {
                        string economico = (string)ccEK.descripcion;
                        var registroCapturaStandBy = _context.tblM_STB_CapturaStandBy.FirstOrDefault(x => x.Economico == economico && x.estatus == 2);

                        if (registroCapturaStandBy != null)
                        {
                            registroCapturaStandBy.estatus = 4;
                            registroCapturaStandBy.usuarioLiberaID = vSesiones.sesionUsuarioDTO.id;
                            registroCapturaStandBy.fechaLibera = DateTime.Now;
                            registroCapturaStandBy.comentarioLiberacion = "Se quitó el Stand-By por movimiento de almacén.";

                            _context.SaveChanges();
                        }
                    }
                    catch (Exception ep) { }
                    #endregion

                }
            }

            return flagMaquinaStandBy;
        }

        public List<Core.DTO.Principal.Generales.ComboDTO> getRequisitores()
        {
            try
            {
                var listaEmpleadosEK = new List<Core.DTO.Principal.Generales.ComboDTO>();
                var listaUsuariosSIGOPLAN = _context.tblP_Usuario.Where(x => x.estatus == true && x.cliente == false).Select(x => x.id).ToList();
                var listaUsuariosSIGOPLAN_ENKONTROL = _context.tblP_Usuario_Enkontrol.Where(x => listaUsuariosSIGOPLAN.Contains(x.idUsuario)).ToList();
                if (vSesiones.sesionEmpresaActual == 1 || vSesiones.sesionEmpresaActual == 2)
                {
                    listaEmpleadosEK = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT empleado AS Value, descripcion AS Text FROM empleados").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    listaEmpleadosEK = (List<Core.DTO.Principal.Generales.ComboDTO>)_contextEnkontrol.Where("SELECT empleado AS Value, descripcion AS Text FROM DBA.empleados").ToObject<List<Core.DTO.Principal.Generales.ComboDTO>>();
                }


                listaEmpleadosEK = listaEmpleadosEK.Where(x => listaUsuariosSIGOPLAN_ENKONTROL.Select(y => y.empleado).Contains(Int32.Parse(x.Value))).ToList();

                return listaEmpleadosEK;
            }
            catch (Exception o_O) { return new List<Core.DTO.Principal.Generales.ComboDTO>(); }
        }

        public Dictionary<string, object> getInsumoInformacionSurtido(int insumo, string cc, int numero_requisicion)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var requisicionEK = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        r.*, 
                                        s.descripcion as solicitoNom, 
                                        e.descripcion as empModificaNom, 
                                        v.descripcion as voboNom, 
                                        a.descripcion as empAutNom 
                                    FROM so_requisicion r 
                                        LEFT JOIN empleados s ON s.empleado = r.solicito 
                                        LEFT JOIN empleados v ON v.empleado = r.vobo 
                                        LEFT JOIN empleados e ON e.empleado = r.empleado_modifica 
                                        LEFT JOIN empleados a ON a.empleado = r.emp_autoriza 
                                    WHERE r.cc = '{0}' AND r.numero = {1}", cc, numero_requisicion)
                );
                var requisicion = ((List<dynamic>)requisicionEK.ToObject<List<dynamic>>())[0];
                var requisicionSIGOPLAN = getRequisicionSIGOPLAN(cc, numero_requisicion);

                decimal existenciaTotal = default(decimal);
                decimal existenciaLAB = default(decimal);

                var listaExistenciaEK = consultaCheckProductivo(
                    string.Format(@"SELECT 
                                        mov.almacen, 
                                        det.insumo, 
                                        det.area_alm, 
                                        det.lado_alm, 
                                        det.estante_alm, 
                                        det.nivel_alm, 
                                        SUM(IF mov.tipo_mov IN (1,2,3,4,5) THEN det.Cantidad ELSE 0 ENDIF) AS Entradas, 
                                        SUM(IF mov.tipo_mov IN (51,52,53,54,55) THEN det.Cantidad ELSE 0 ENDIF) AS Salidas, 
                                        SUM(det.Cantidad * IF mov.tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF) AS Existencia 
                                    FROM si_movimientos mov 
                                        INNER JOIN si_almacen alm ON alm.almacen = mov.almacen 
                                        INNER JOIN si_movimientos_det det ON det.almacen = mov.almacen AND det.tipo_mov = mov.tipo_mov AND det.numero = mov.numero 
                                    WHERE 
                                        det.insumo = {0} AND alm.almacen < 900 
                                    GROUP BY mov.almacen, det.insumo, det.area_alm, det.lado_alm, det.estante_alm, det.nivel_alm", insumo)
                );

                if (listaExistenciaEK != null)
                {
                    var listaExistencia = (List<dynamic>)listaExistenciaEK.ToObject<List<dynamic>>();

                    listaExistencia = listaExistencia.Where(x => (decimal)x.Existencia > 0).ToList();

                    existenciaTotal = listaExistencia.Sum(x => x.Existencia.Value != null ? (decimal)x.Existencia : default(decimal));
                    existenciaLAB = listaExistencia.Where(x =>
                        (int)x.almacen == (requisicionSIGOPLAN != null ? requisicionSIGOPLAN.libre_abordo : (int)requisicion.libre_abordo)
                        ).Sum(x => x.Existencia.Value != null ? (decimal)x.Existencia : default(decimal));
                }

                var insumoEK = ((List<dynamic>)consultaCheckProductivo(
                    string.Format(@"SELECT i.insumo, i.descripcion, i.unidad, i.bit_area_cta, i.cancelado, i.color_resguardo, i.compras_req FROM insumos i WHERE i.insumo = {0}", insumo)
                ).ToObject<List<dynamic>>())[0];

                var insumoInformacion = new RequisicionDetDTO
                {
                    id = 0,
                    idReq = 0,
                    cc = cc,
                    numero = numero_requisicion,
                    partida = 0,
                    insumo = insumo,
                    insumoDesc = (string)insumoEK.descripcion,
                    unidad = (string)insumoEK.unidad,
                    cancelado = (string)insumoEK.cancelado,
                    compras_req = (int?)insumoEK.compras_req,
                    observaciones = "",
                    cantidadCapturada = 0,
                    listaSurtido = null,
                    totalASurtir = 0,
                    existenciaTotal = existenciaTotal,
                    existenciaLAB = existenciaLAB,
                    validadoAlmacen = false
                };

                resultado.Add(SUCCESS, true);
                resultado.Add("insumoInformacion", insumoInformacion);
            }
            catch (Exception e)
            {
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public bool verificarRequisicionCompletamenteSurtida(string cc, int numero_requisicion)
        {
            try
            {
                var requisicionDetalle = (List<RequisicionDetDTO>)consultaCheckProductivo(
                    string.Format(@"SELECT 
                                            det.*, 
                                            ins.insumo, 
                                            ins.descripcion AS insumoDescripcion 
                                        FROM so_requisicion_det det 
                                            INNER JOIN insumos ins ON det.insumo = ins.insumo 
                                        WHERE det.cc = '{0}' AND det.numero = {1}", cc, numero_requisicion)
                ).ToObject<List<RequisicionDetDTO>>();

                #region Almacén Propio
                var listaAP = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AP" && x.cc == cc && x.surtido.numero == numero_requisicion).ToList();
                #endregion

                #region Almacén Externo
                var listaAE = _context.tblCom_SurtidoDet.Where(x => x.estatus && x.tipoSurtidoDetalle == "AE" && x.cc == cc && x.surtido.numero == numero_requisicion).ToList();
                var entradasTraspasosAE = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.tipo_mov == 2 && x.cc == cc && x.numeroReq == numero_requisicion).ToList();
                var entradasTraspasosDetalleAE = (
                    from mov in entradasTraspasosAE
                    join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 2).ToList()
                        on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                    select new
                    {
                        insumo = det.insumo,
                        cantidad = det.cantidad
                    }
                ).ToList();
                #endregion

                #region Orden Compra
                var comprasDetalleEK = consultaCheckProductivo(string.Format(@"SELECT * FROM so_orden_compra_det WHERE cc = '{0}' AND num_requisicion = {1}", cc, numero_requisicion));
                List<dynamic> comprasDetalle = new List<dynamic>();

                if (comprasDetalleEK != null)
                {
                    comprasDetalle = (List<dynamic>)comprasDetalleEK.ToObject<List<dynamic>>();
                }
                #endregion

                #region Salida por Consumo
                var salidasConsumo = _context.tblAlm_Movimientos.Where(x => x.estatusHabilitado && x.cc == cc && x.numeroReq == numero_requisicion && x.tipo_mov == 51).ToList();
                var salidasConsumoDetalle = (
                    from mov in salidasConsumo
                    join det in _context.tblAlm_MovimientosDet.Where(x => x.estatusHabilitado && x.tipo_mov == 51).ToList()
                        on new { mov.almacen, mov.numero } equals new { det.almacen, det.numero }
                    select new
                    {
                        insumo = det.insumo,
                        cantidad = det.cantidad
                    }
                ).ToList();
                #endregion

                List<bool> listaPartidasSurtidas = new List<bool>();

                foreach (var det in requisicionDetalle)
                {
                    var cantidadPartida = det.cantidad - det.cant_cancelada;

                    var solicitadoAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                    var cantidadAP = listaAP.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();

                    var solicitadoAE = listaAE.Where(x => x.partidaRequisicion == det.partida && x.insumo == det.insumo).Select(x => x.cantidad).Sum();
                    var cantidadAE = entradasTraspasosDetalleAE.Where(x => x.insumo == det.insumo).Select(x => x.cantidad).Sum();

                    var solicitadoOC = cantidadPartida - solicitadoAP - solicitadoAE;
                    var cantidadOC = default(decimal);

                    if (comprasDetalle.Count() > 0)
                    {
                        foreach (var ocDet in comprasDetalle.Where(x => x.part_requisicion == det.partida).ToList())
                        {
                            cantidadOC += Convert.ToDecimal(ocDet.cant_recibida, CultureInfo.InvariantCulture);
                        }
                    }

                    var partidaCompletamenteSurtida = solicitadoAP == cantidadAP && solicitadoAE == cantidadAE && solicitadoOC == cantidadOC;

                    listaPartidasSurtidas.Add(partidaCompletamenteSurtida);
                }

                return listaPartidasSurtidas.All(x => x);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "verificarRequisicionCompletamenteSurtida", e, AccionEnum.CORREO, 0, new { cc = cc, numero_requisicion = numero_requisicion });
                return false;
            }
        }

        public object getEmpleadoEnKontrolAutocomplete(string term)
        {
            try
            {
                List<EmpleadoAutocompleteDTO> listaEmpleados = new List<EmpleadoAutocompleteDTO>();

                var listaEmpleadosCP = _context.Select<EmpleadoAutocompleteDTO>(new DapperDTO
                {
                    baseDatos = MainContextEnum.Construplan,
                    consulta = @"SELECT 
                                    e.clave_empleado AS claveEmpleado, 
                                    (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) AS nombreEmpleado,
                                    p.descripcion AS puestoEmpleado,
                                    e.cc_contable AS ccID,
                                    c.descripcion as cc
                                FROM tblRH_EK_Empleados AS e
                                    INNER JOIN tblRH_EK_Puestos as p on e.puesto=p.puesto
                                    INNER JOIN tblP_CC as c on e.cc_contable=c.cc
                                WHERE e.estatus_empleado ='A' AND (TRIM(e.nombre) + ' ' + TRIM(e.ape_paterno) + ' ' + TRIM(e.ape_materno)) LIKE CONCAT('%', @term, '%')
                                ORDER BY e.ape_paterno DESC",
                    parametros = new { term = term }
                });

                foreach (var cp in listaEmpleadosCP)
                {
                    cp.empresa = (int)EmpresaEnum.Construplan;
                }

                listaEmpleados.AddRange(listaEmpleadosCP);

                return listaEmpleados.Select(x => new
                {
                    id = x.claveEmpleado,
                    value = x.nombreEmpleado,
                    nombreEmpleado = (string)x.nombreEmpleado,
                    puestoEmpleado = (string)x.puestoEmpleado,
                    ccID = (string)x.ccID,
                    cc = (string)x.cc,
                    empresa = x.empresa
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Dictionary<string, object> CalcularExistenciasRequisicion(int almacen, List<int> listaInsumos)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<decimal> listaExistencias = new List<decimal>();

                foreach (var insumo in listaInsumos)
                {
                    var existenciasEK = _contextEnkontrol.Select<decimal>(getEnkontrolEnumADM(), string.Format(@"
                        SELECT 
                            ISNULL(SUM(cantidad * IF tipo_mov IN (1,2,3,4,5) THEN 1 ELSE -1 ENDIF), 0) AS existencia 
                        FROM si_movimientos_det
                        WHERE almacen = {0} AND insumo = {1}
                    ", almacen, insumo)).FirstOrDefault();

                    listaExistencias.Add(existenciasEK);
                }

                resultado.Add(SUCCESS, true);
                resultado.Add("data", listaExistencias);
            }
            catch (Exception e)
            {
                resultado.Add("success", false);
                resultado.Add("EMPTY", true);
            }

            return resultado;
        }

        private EnkontrolEnum getEnkontrolEnumADM()
        {
            var baseDatos = new EnkontrolEnum();

            if (productivo)
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.CplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.ArrenProd;
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                }
                else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.GCPLAN)
                {
                    baseDatos = EnkontrolEnum.GCPLAN;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan, Arrendadora y Colombia");
                }
            }
            else
            {
                if (vSesiones.sesionEmpresaActual == 1)
                {
                    baseDatos = EnkontrolEnum.PruebaCplanProd;
                }
                else if (vSesiones.sesionEmpresaActual == 2)
                {
                    baseDatos = EnkontrolEnum.PruebaArrenADM;
                }
                else if (vSesiones.sesionEmpresaActual == 3)
                {
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                }
                else
                {
                    throw new Exception("Empresa distinta a Construplan, Arrendadora y Colombia");
                }
            }

            return baseDatos;
        }

        #region CRUD Insumos Consignación - Licitación - Convenio
        #region Consigna
        public Dictionary<string, object> GetInsumosConsigna(DataTablesParam param)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if (param.sSearch == "" || param.sSearch == null)
                {
                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo).ToList();
                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }
                else
                {
                    var listaProveedoresFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE nombre LIKE '%{0}%' OR CAST(numpro AS varchar) LIKE '%{0}%'", param.sSearch));
                    var listaInsumosFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE descripcion LIKE '%{0}%'", param.sSearch));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo).ToList().Where(x =>
                        x.insumo.ToString().Contains(param.sSearch) ||
                        listaInsumosFiltro.Select(y => (int)y.insumo).Contains(x.insumo) ||
                        listaProveedoresFiltro.Select(y => (int)y.numpro).Contains(x.proveedor) ||
                        x.precio.ToString().Contains(param.sSearch)
                    ).Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumosConsigna", null, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosConsigna.FirstOrDefault(x => x.registroActivo && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor);

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un artículo con esa información.");
                    }
                    #endregion

                    insumo.registroActivo = true;

                    _context.tblCom_InsumosConsigna.Add(insumo);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "GuardarNuevoInsumoConsigna", null, AccionEnum.AGREGAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosConsigna.FirstOrDefault(x =>
                        x.id != insumo.id && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor
                    );

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un registro capturado para esa información.");
                    }
                    #endregion

                    var insumoSIGOPLAN = _context.tblCom_InsumosConsigna.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.insumo = insumo.insumo;
                    insumoSIGOPLAN.proveedor = insumo.proveedor;
                    insumoSIGOPLAN.precio = insumo.precio;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EditarInsumoConsigna", null, AccionEnum.ACTUALIZAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarInsumoConsigna(tblCom_InsumosConsigna insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var insumoSIGOPLAN = _context.tblCom_InsumosConsigna.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EliminarInsumoConsigna", null, AccionEnum.ELIMINAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CargarExcelInsumosConsigna(HttpFileCollectionBase archivos)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (archivos.Count == 0)
                    {
                        throw new Exception("No se cargaron archivos.");
                    }

                    for (int i = 0; i < archivos.Count; i++)
                    {
                        HttpPostedFileBase archivo = archivos[i];

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
                                for (int x = worksheet.Dimension.Start.Row; x <= worksheet.Dimension.End.Row; x++)
                                {
                                    List<string> fila = new List<string>();

                                    //loop all columns in a row
                                    for (int y = worksheet.Dimension.Start.Column; y <= worksheet.Dimension.End.Column; y++)
                                    {
                                        //add the cell data to the List
                                        if (worksheet.Cells[x, y].Value != null)
                                        {
                                            fila.Add(worksheet.Cells[x, y].Value.ToString());
                                        }
                                        else
                                        {
                                            fila.Add("");
                                        }
                                    }

                                    if (x > 1 && fila[0] != "")
                                    {
                                        tabla.Add(fila);
                                    }
                                }
                            }
                        }
                        #endregion

                        foreach (var fila in tabla)
                        {
                            var numeroActivo = Int32.Parse(fila[3]);

                            if (numeroActivo != 1 && numeroActivo != 0)
                            {
                                throw new Exception("Debe indicar la columna de \"Activo\" para todos los insumos.");
                            }

                            var insumo = Int32.Parse(fila[0]);
                            var proveedor = Int32.Parse(fila[1]);
                            var precio = Convert.ToDecimal(fila[2].Replace("$", "").Replace(",", ""), CultureInfo.InvariantCulture);

                            var registroExistente = _context.tblCom_InsumosConsigna.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor);

                            if (numeroActivo == 1)
                            {
                                if (registroExistente == null) //Nuevo Registro
                                {
                                    var nuevoRegistro = new tblCom_InsumosConsigna();

                                    nuevoRegistro.insumo = insumo;
                                    nuevoRegistro.proveedor = proveedor;
                                    nuevoRegistro.precio = precio;
                                    nuevoRegistro.registroActivo = true;

                                    _context.tblCom_InsumosConsigna.Add(nuevoRegistro);
                                    _context.SaveChanges();
                                }
                                else //Registro Existente
                                {
                                    registroExistente.precio = precio;
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                if (registroExistente != null) //Si existe el registro se desactiva, si no se ignora el renglón del Excel.
                                {
                                    registroExistente.registroActivo = false;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }

                    dbSigoplanTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "CargarExcelInsumosConsigna", null, AccionEnum.AGREGAR, 0, null);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }

        public MemoryStream DescargarExcelInsumosConsigna()
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Insumos");

                    #region HEADER
                    string TituloRango = "A1:F1";
                    hoja1.Cells[TituloRango].LoadFromArrays(new List<string[]>() { new string[] { "Insumo", "Descripción", "Proveedor", "Precio", "Activo", "Tipo" } });
                    hoja1.Cells[TituloRango].Style.Font.Bold = true;
                    hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    #endregion

                    #region DATOS
                    var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores"));
                    var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos"));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var cellData = new List<object[]>();

                    foreach (var ins in listaInsumosSIGOPLAN)
                    {
                        cellData.Add(new object[] { ins.insumo, ins.insumoDesc, ins.proveedor, ins.precio, "1", "Consigna" });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    #endregion

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

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

        #region Licitación
        public Dictionary<string, object> GetInsumosLicitacion(DataTablesParam param)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if (param.sSearch == "" || param.sSearch == null)
                {
                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).ToList();
                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        articulo = x.articulo,
                        unidad = x.unidad,
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }
                else
                {
                    var listaProveedoresFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE nombre LIKE '%{0}%' OR CAST(numpro AS varchar) LIKE '%{0}%'", param.sSearch));
                    var listaInsumosFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE descripcion LIKE '%{0}%'", param.sSearch));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).ToList().Where(x =>
                        x.insumo.ToString().Contains(param.sSearch) ||
                        listaInsumosFiltro.Select(y => (int)y.insumo).Contains(x.insumo) ||
                        listaProveedoresFiltro.Select(y => (int)y.numpro).Contains(x.proveedor) ||
                        x.articulo.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        x.unidad.ToUpper().Contains(param.sSearch.ToUpper()) ||
                        x.precio.ToString().Contains(param.sSearch)
                    ).Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        articulo = x.articulo,
                        unidad = x.unidad,
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }

                resultado.Add(SUCCESS, true);

                //var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores"));
                //var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos"));

                //var insumos = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).ToList().Select(x => new
                //{
                //    id = x.id,
                //    insumo = x.insumo,
                //    insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                //    proveedor = x.proveedor,
                //    proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                //    articulo = x.articulo,
                //    unidad = x.unidad,
                //    precio = x.precio
                //}).ToList();

                //resultado.Add("data", insumos);
                //resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumosLicitacion", null, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosLicitacion.FirstOrDefault(x => x.registroActivo && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor && x.articulo == insumo.articulo);

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un artículo con esa información.");
                    }
                    #endregion

                    insumo.registroActivo = true;

                    _context.tblCom_InsumosLicitacion.Add(insumo);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "GuardarNuevoInsumoLicitacion", null, AccionEnum.AGREGAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosLicitacion.FirstOrDefault(x =>
                        x.id != insumo.id && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor && x.articulo == insumo.articulo
                    );

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un registro capturado para esa información.");
                    }
                    #endregion

                    var insumoSIGOPLAN = _context.tblCom_InsumosLicitacion.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.insumo = insumo.insumo;
                    insumoSIGOPLAN.proveedor = insumo.proveedor;
                    insumoSIGOPLAN.articulo = insumo.articulo;
                    insumoSIGOPLAN.unidad = insumo.unidad;
                    insumoSIGOPLAN.precio = insumo.precio;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EditarInsumoLicitacion", null, AccionEnum.ACTUALIZAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarInsumoLicitacion(tblCom_InsumosLicitacion insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var insumoSIGOPLAN = _context.tblCom_InsumosLicitacion.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EliminarInsumoLicitacion", null, AccionEnum.ELIMINAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CargarExcelInsumosLicitacion(HttpFileCollectionBase archivos)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (archivos.Count == 0)
                    {
                        throw new Exception("No se cargaron archivos.");
                    }

                    for (int i = 0; i < archivos.Count; i++)
                    {
                        HttpPostedFileBase archivo = archivos[i];

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
                                for (int x = worksheet.Dimension.Start.Row; x <= worksheet.Dimension.End.Row; x++)
                                {
                                    List<string> fila = new List<string>();

                                    //loop all columns in a row
                                    for (int y = worksheet.Dimension.Start.Column; y <= worksheet.Dimension.End.Column; y++)
                                    {
                                        //add the cell data to the List
                                        if (worksheet.Cells[x, y].Value != null)
                                        {
                                            fila.Add(worksheet.Cells[x, y].Value.ToString());
                                        }
                                        else
                                        {
                                            fila.Add("");
                                        }
                                    }

                                    if (x > 1 && fila[0] != "")
                                    {
                                        tabla.Add(fila);
                                    }
                                }
                            }
                        }
                        #endregion

                        foreach (var fila in tabla)
                        {
                            var numeroActivo = Int32.Parse(fila[5]);

                            if (numeroActivo != 1 && numeroActivo != 0)
                            {
                                throw new Exception("Debe indicar la columna de \"Activo\" para todos los insumos.");
                            }

                            var insumo = Int32.Parse(fila[0]);
                            var proveedor = Int32.Parse(fila[1]);
                            var articulo = fila[2];
                            var unidad = fila[3];
                            var precio = Convert.ToDecimal(fila[4].Replace("$", "").Replace(",", ""), CultureInfo.InvariantCulture);

                            var registroExistente = _context.tblCom_InsumosLicitacion.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor && x.articulo == articulo);

                            if (numeroActivo == 1)
                            {
                                if (registroExistente == null) //Nuevo Registro
                                {
                                    var nuevoRegistro = new tblCom_InsumosLicitacion();

                                    nuevoRegistro.insumo = insumo;
                                    nuevoRegistro.proveedor = proveedor;
                                    nuevoRegistro.articulo = articulo;
                                    nuevoRegistro.unidad = unidad;
                                    nuevoRegistro.precio = precio;
                                    nuevoRegistro.registroActivo = true;

                                    _context.tblCom_InsumosLicitacion.Add(nuevoRegistro);
                                    _context.SaveChanges();
                                }
                                else //Registro Existente
                                {
                                    registroExistente.unidad = unidad;
                                    registroExistente.precio = precio;
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                if (registroExistente != null) //Si existe el registro se desactiva, si no se ignora el renglón del Excel.
                                {
                                    registroExistente.registroActivo = false;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }

                    dbSigoplanTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "CargarExcelInsumosLicitacion", null, AccionEnum.AGREGAR, 0, null);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }

        public MemoryStream DescargarExcelInsumosLicitacion()
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Insumos");

                    #region HEADER
                    string TituloRango = "A1:H1";
                    hoja1.Cells[TituloRango].LoadFromArrays(new List<string[]>() { new string[] { "Insumo", "Descripción", "Proveedor", "Artículo", "Unidad", "Precio", "Activo", "Tipo" } });
                    hoja1.Cells[TituloRango].Style.Font.Bold = true;
                    hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    #endregion

                    #region DATOS
                    var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores"));
                    var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos"));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        articulo = x.articulo,
                        unidad = x.unidad,
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var cellData = new List<object[]>();

                    foreach (var ins in listaInsumosSIGOPLAN)
                    {
                        cellData.Add(new object[] { ins.insumo, ins.insumoDesc, ins.proveedor, ins.articulo, ins.unidad, ins.precio, "1", "Licitacion" });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    #endregion

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

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

        #region Convenio
        public Dictionary<string, object> GetInsumosConvenio(DataTablesParam param)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                if (param.sSearch == "" || param.sSearch == null)
                {
                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).ToList();
                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }
                else
                {
                    var listaProveedoresFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE nombre LIKE '%{0}%' OR CAST(numpro AS varchar) LIKE '%{0}%'", param.sSearch));
                    var listaInsumosFiltro = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE descripcion LIKE '%{0}%'", param.sSearch));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).ToList().Where(x =>
                        x.insumo.ToString().Contains(param.sSearch) ||
                        listaInsumosFiltro.Select(y => (int)y.insumo).Contains(x.insumo) ||
                        listaProveedoresFiltro.Select(y => (int)y.numpro).Contains(x.proveedor) ||
                        x.precio.ToString().Contains(param.sSearch)
                    ).Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        //insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        //proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var rangoRenglonInicial = param.iDisplayStart + 1;
                    var rangoRenglonFinal = param.iDisplayStart + param.iDisplayLength;

                    var datos = listaInsumosSIGOPLAN.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                    if (datos.Count() > 0)
                    {
                        #region Setear insumo descripción y proveedor nombre
                        var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos WHERE insumo IN ({0})", string.Join(", ", datos.Select(x => x.insumo).ToList())));
                        var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores WHERE numpro IN ({0})", string.Join(", ", datos.Select(x => x.proveedor).ToList())));

                        foreach (var ins in datos)
                        {
                            ins.insumoDesc = listaInsumos.Where(x => (int)x.insumo == ins.insumo).Select(x => (string)x.descripcion).FirstOrDefault();
                            ins.proveedorDesc = listaProveedores.Where(x => (int)x.numpro == ins.proveedor).Select(x => (int)x.numpro + " - " + (string)x.nombre).FirstOrDefault();
                        }
                        #endregion
                    }

                    resultado.Add("datos", new
                    {
                        aaData = datos,
                        sEcho = param.sEcho,
                        iTotalDisplayRecords = listaInsumosSIGOPLAN.Count(),
                        iTotalRecords = listaInsumosSIGOPLAN.Count()
                    });
                }

                resultado.Add(SUCCESS, true);

                //var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores"));
                //var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos"));

                //var insumos = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).ToList().Select(x => new
                //{
                //    id = x.id,
                //    insumo = x.insumo,
                //    insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                //    proveedor = x.proveedor,
                //    proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                //    precio = x.precio
                //}).ToList();

                //resultado.Add("data", insumos);
                //resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumosConvenio", null, AccionEnum.CONSULTA, 0, null);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GuardarNuevoInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosConvenio.FirstOrDefault(x => x.registroActivo && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor);

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un artículo con esa información.");
                    }
                    #endregion

                    insumo.registroActivo = true;

                    _context.tblCom_InsumosConvenio.Add(insumo);
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.AGREGAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "GuardarNuevoInsumoConvenio", null, AccionEnum.AGREGAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EditarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    #region Validaciones
                    var registroExistente = _context.tblCom_InsumosConvenio.FirstOrDefault(x =>
                        x.id != insumo.id && x.insumo == insumo.insumo && x.proveedor == insumo.proveedor
                    );

                    if (registroExistente != null)
                    {
                        throw new Exception("Ya existe un registro capturado para esa información.");
                    }
                    #endregion

                    var insumoSIGOPLAN = _context.tblCom_InsumosConvenio.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.insumo = insumo.insumo;
                    insumoSIGOPLAN.proveedor = insumo.proveedor;
                    insumoSIGOPLAN.precio = insumo.precio;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ACTUALIZAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EditarInsumoConvenio", null, AccionEnum.ACTUALIZAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> EliminarInsumoConvenio(tblCom_InsumosConvenio insumo)
        {
            var resultado = new Dictionary<string, object>();

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var insumoSIGOPLAN = _context.tblCom_InsumosConvenio.FirstOrDefault(x => x.id == insumo.id);

                    insumoSIGOPLAN.registroActivo = false;
                    _context.SaveChanges();

                    dbContextTransaction.Commit();
                    SaveBitacora(0, (int)AccionEnum.ELIMINAR, 0, JsonUtils.convertNetObjectToJson(insumo));
                    resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EliminarInsumoConvenio", null, AccionEnum.ELIMINAR, 0, insumo);
                    resultado.Add(SUCCESS, false);
                    resultado.Add(MESSAGE, e.Message);
                }
            }

            return resultado;
        }

        public Dictionary<string, object> CargarExcelInsumosConvenio(HttpFileCollectionBase archivos)
        {
            var result = new Dictionary<string, object>();

            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (archivos.Count == 0)
                    {
                        throw new Exception("No se cargaron archivos.");
                    }

                    for (int i = 0; i < archivos.Count; i++)
                    {
                        HttpPostedFileBase archivo = archivos[i];

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
                                for (int x = worksheet.Dimension.Start.Row; x <= worksheet.Dimension.End.Row; x++)
                                {
                                    List<string> fila = new List<string>();

                                    //loop all columns in a row
                                    for (int y = worksheet.Dimension.Start.Column; y <= worksheet.Dimension.End.Column; y++)
                                    {
                                        //add the cell data to the List
                                        if (worksheet.Cells[x, y].Value != null)
                                        {
                                            fila.Add(worksheet.Cells[x, y].Value.ToString());
                                        }
                                        else
                                        {
                                            fila.Add("");
                                        }
                                    }

                                    if (x > 1 && fila[0] != "")
                                    {
                                        tabla.Add(fila);
                                    }
                                }
                            }
                        }
                        #endregion

                        foreach (var fila in tabla)
                        {
                            var numeroActivo = Int32.Parse(fila[3]);

                            if (numeroActivo != 1 && numeroActivo != 0)
                            {
                                throw new Exception("Debe indicar la columna de \"Activo\" para todos los insumos.");
                            }

                            var insumo = Int32.Parse(fila[0]);
                            var proveedor = Int32.Parse(fila[1]);
                            var precio = Convert.ToDecimal(fila[2].Replace("$", "").Replace(",", ""), CultureInfo.InvariantCulture);

                            var registroExistente = _context.tblCom_InsumosConvenio.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor);

                            if (numeroActivo == 1)
                            {
                                if (registroExistente == null) //Nuevo Registro
                                {
                                    var nuevoRegistro = new tblCom_InsumosConvenio();

                                    nuevoRegistro.insumo = insumo;
                                    nuevoRegistro.proveedor = proveedor;
                                    nuevoRegistro.precio = precio;
                                    nuevoRegistro.registroActivo = true;

                                    _context.tblCom_InsumosConvenio.Add(nuevoRegistro);
                                    _context.SaveChanges();
                                }
                                else //Registro Existente
                                {
                                    registroExistente.precio = precio;
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                if (registroExistente != null) //Si existe el registro se desactiva, si no se ignora el renglón del Excel.
                                {
                                    registroExistente.registroActivo = false;
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }

                    dbSigoplanTransaction.Commit();
                    result.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "CargarExcelInsumosConvenio", null, AccionEnum.AGREGAR, 0, null);
                    result.Add(MESSAGE, e.Message);
                    result.Add(SUCCESS, false);
                }
            }

            return result;
        }

        public MemoryStream DescargarExcelInsumosConvenio()
        {
            try
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var hoja1 = excel.Workbook.Worksheets.Add("Insumos");

                    #region HEADER
                    string TituloRango = "A1:F1";
                    hoja1.Cells[TituloRango].LoadFromArrays(new List<string[]>() { new string[] { "Insumo", "Descripción", "Proveedor", "Precio", "Activo", "Tipo" } });
                    hoja1.Cells[TituloRango].Style.Font.Bold = true;
                    hoja1.Cells[TituloRango].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    hoja1.Cells[TituloRango].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    #endregion

                    #region DATOS
                    var listaProveedores = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM sp_proveedores"));
                    var listaInsumos = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT * FROM insumos"));

                    var listaInsumosSIGOPLAN = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo).ToList().Select(x => new InsumoCLCDTO
                    {
                        id = x.id,
                        insumo = x.insumo,
                        insumoDesc = listaInsumos.Where(y => (int)y.insumo == x.insumo).Select(z => z.descripcion).FirstOrDefault(),
                        proveedor = x.proveedor,
                        proveedorDesc = x.proveedor + " - " + listaProveedores.Where(y => (int)y.numpro == x.proveedor).Select(z => (string)z.nombre).FirstOrDefault(),
                        precio = x.precio
                    }).OrderBy(x => x.insumo).ThenBy(x => x.proveedor).ToList();

                    var cellData = new List<object[]>();

                    foreach (var ins in listaInsumosSIGOPLAN)
                    {
                        cellData.Add(new object[] { ins.insumo, ins.insumoDesc, ins.proveedor, ins.precio, "1", "Convenio" });
                    }

                    hoja1.Cells[2, 1].LoadFromArrays(cellData);
                    #endregion

                    excel.Compression = OfficeOpenXml.CompressionLevel.BestSpeed;

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
        #endregion

        public bool EnviarCorreoRequisicion(string cc, int numero, List<string> listaCorreos, List<Byte[]> downloadPDF, string link)
        {
            bool correoEnviado = false;


            using (var dbSigoplanTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    switch ((MainContextEnum)vSesiones.sesionEmpresaActual)
                    {
                        case MainContextEnum.Colombia:
                            {
                                #region COLOMBIA
                                var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ColombiaProductivo, new OdbcConsultaDTO() { consulta = @"SELECT * FROM DBA.cc" });

                                string asunto = "";
                                string mensaje = "";

                                if (string.IsNullOrEmpty(link)) //Envío normal de correo sin link.
                                {
                                    asunto = "Requisición [" + cc + "-" + numero + "]";
                                    mensaje = "Se envía la información sobre la requisición [" + cc + "-" + numero + "].";
                                }
                                else //Envío de correo con link.
                                {
                                    asunto = string.Format("COTIZACIÓN PENDIENTE POR REALIZAR [{0}-{1}]", cc, numero);
                                    mensaje = string.Format("Buen día, favor de ingresar al siguiente link para realizar la cotización:<br>{0}<br><br>Se informa que este es un correo autogenerado por el sistema. No es necesario dar una respuesta, gracias.", link);
                                }

#if DEBUG
                                listaCorreos = new List<string>();
                                listaCorreos.Add("oscar.valencia@construplan.com.mx");
                                listaCorreos.Add("miguel.buzani@construplan.com.mx");
#endif

                                if (downloadPDF.Count() > 1)
                                {
                                    LogError(0, 0, "RequisicionController", "EnviarCorreoRequisicion_DobleArchivoRequisicion", null, AccionEnum.CORREO, 0, new { cc = cc, numero = numero, listaCorreos = listaCorreos, link = link });
                                    List<adjuntoCorreoDTO> listaArchivos = new List<adjuntoCorreoDTO>();
                                    int contador = 1;

                                    foreach (var archivo in downloadPDF)
                                    {
                                        listaArchivos.Add(new adjuntoCorreoDTO
                                        {
                                            archivo = archivo,
                                            extArchivo = ".pdf",
                                            nombreArchivo = "Archivo No_" + contador++
                                        });
                                    }

                                    GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "ERROR Correo Requisición - Archivos Múltiples : " + asunto), mensaje, new List<string> { "oscar.valencia@construplan.com.mx" }, listaArchivos);
                                }

                                correoEnviado = GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos, new List<adjuntoCorreoDTO> { new adjuntoCorreoDTO { archivo = downloadPDF.LastOrDefault(), extArchivo = ".pdf", nombreArchivo = "Requisición_" + cc + "_" + numero } });

                                if (correoEnviado)
                                {
                                    #region LINK

                                    //if (string.IsNullOrEmpty(link))
                                    //{
                                    //    var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == numero);

                                    //    if (requisicionSIGOPLAN != null)
                                    //    {
                                    //        requisicionSIGOPLAN.fechaEnvioCorreoProveedor = DateTime.Now;
                                    //        _context.SaveChanges();
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    var registroLink = _context.tblCom_ProveedoresLinks.FirstOrDefault(x =>
                                    //        x.cc == cc && x.numRequisicion == numero && x.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && x.registroActivo
                                    //    );

                                    //    if (registroLink != null)
                                    //    {
                                    //        registroLink.esEnvioCorreoSIGOPLAN = true;
                                    //        registroLink.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                    //        registroLink.fechaModificacion = DateTime.Now;
                                    //        registroLink.fechaEnvioCorreo = DateTime.Now;
                                    //        _context.SaveChanges();
                                    //    }
                                    //}
                                    #endregion
                                }
                                else
                                {
                                    throw new Exception("No se pudo enviar el correo.");
                                }
                                #endregion
                            }
                            break;
                        default:
                            {
                                #region RESTO EMPRESAS
                                var ccConstruplan = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.CplanProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });
                                var ccArrendadora = _contextEnkontrol.Select<dynamic>(EnkontrolEnum.ArrenProd, new OdbcConsultaDTO() { consulta = @"SELECT * FROM cc" });

                                string asunto = "";
                                string mensaje = "";

                                if (string.IsNullOrEmpty(link)) //Envío normal de correo sin link.
                                {
                                    asunto = "Requisición [" + cc + "-" + numero + "]";
                                    mensaje = "Se envía la información sobre la requisición [" + cc + "-" + numero + "].";
                                }
                                else //Envío de correo con link.
                                {
                                    asunto = string.Format("COTIZACIÓN PENDIENTE POR REALIZAR [{0}-{1}]", cc, numero);
                                    mensaje = string.Format("Buen día, favor de ingresar al siguiente link para realizar la cotización:<br>{0}<br><br>Se informa que este es un correo autogenerado por el sistema. No es necesario dar una respuesta, gracias.", link);
                                }

#if DEBUG
                                listaCorreos = new List<string>();
                                listaCorreos.Add("oscar.valencia@construplan.com.mx");
#endif

                                if (downloadPDF.Count() > 1)
                                {
                                    LogError(0, 0, "RequisicionController", "EnviarCorreoRequisicion_DobleArchivoRequisicion", null, AccionEnum.CORREO, 0, new { cc = cc, numero = numero, listaCorreos = listaCorreos, link = link });
                                    List<adjuntoCorreoDTO> listaArchivos = new List<adjuntoCorreoDTO>();
                                    int contador = 1;

                                    foreach (var archivo in downloadPDF)
                                    {
                                        listaArchivos.Add(new adjuntoCorreoDTO
                                        {
                                            archivo = archivo,
                                            extArchivo = ".pdf",
                                            nombreArchivo = "Archivo No_" + contador++
                                        });
                                    }

                                    GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "ERROR Correo Requisición - Archivos Múltiples : " + asunto), mensaje, new List<string> { "oscar.valencia@construplan.com.mx" }, listaArchivos);
                                }

                                correoEnviado = GlobalUtils.sendMailWithFiles(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), asunto), mensaje, listaCorreos, new List<adjuntoCorreoDTO> { new adjuntoCorreoDTO { archivo = downloadPDF.LastOrDefault(), extArchivo = ".pdf", nombreArchivo = "Requisición_" + cc + "_" + numero } });

                                if (correoEnviado)
                                {
                                    if (string.IsNullOrEmpty(link))
                                    {
                                        var requisicionSIGOPLAN = _context.tblCom_Req.FirstOrDefault(x => x.estatusRegistro && x.cc == cc && x.numero == numero);

                                        if (requisicionSIGOPLAN != null)
                                        {
                                            requisicionSIGOPLAN.fechaEnvioCorreoProveedor = DateTime.Now;
                                            _context.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        var registroLink = _context.tblCom_ProveedoresLinks.FirstOrDefault(x =>
                                            x.cc == cc && x.numRequisicion == numero && x.idEstatusRegistro == EstatusRegistroProveedorLinkEnum.PENDIENTE && x.registroActivo
                                        );

                                        if (registroLink != null)
                                        {
                                            registroLink.esEnvioCorreoSIGOPLAN = true;
                                            registroLink.idUsuarioModificacion = (int)vSesiones.sesionUsuarioDTO.id;
                                            registroLink.fechaModificacion = DateTime.Now;
                                            registroLink.fechaEnvioCorreo = DateTime.Now;
                                            _context.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("No se pudo enviar el correo.");
                                }
                                #endregion
                            }
                            break;
                    }

                    dbSigoplanTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbSigoplanTransaction.Rollback();
                    LogError(0, 0, "RequisicionController", "EnviarCorreoRequisicion", e, AccionEnum.CORREO, 0, new { cc = cc, numero = numero, listaCorreos = listaCorreos, link = link });
                }
            }

            return correoEnviado;
        }

        public Dictionary<string, object> GetArticulosConsignaLicitacionConvenioPorProveedor(int proveedor, TipoConsignaLicitacionConvenioEnum tipo)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var listaInsumosEK = _contextEnkontrol.Select<dynamic>(getEnkontrolEnumADM(), string.Format(@"SELECT insumo, descripcion FROM insumos"));

                switch (tipo)
                {
                    case TipoConsignaLicitacionConvenioEnum.CONSIGNA:
                        var listaConsigna = _context.tblCom_InsumosConsigna.Where(x => x.registroActivo && x.proveedor == proveedor).ToList().Select(x => new
                        {
                            id = x.id,
                            insumo = x.insumo,
                            insumoDesc = listaInsumosEK.Where(y => y.insumo == (int)x.insumo).Select(z => (string)z.descripcion).FirstOrDefault(),
                            proveedor = x.proveedor,
                            articulo = "",
                            unidad = "",
                            precio = x.precio
                        });

                        resultado.Add("data", listaConsigna);
                        break;
                    case TipoConsignaLicitacionConvenioEnum.LICITACION:
                        var listaLicitacion = _context.tblCom_InsumosLicitacion.Where(x => x.registroActivo && x.proveedor == proveedor).ToList().Select(x => new
                        {
                            id = x.id,
                            insumo = x.insumo,
                            insumoDesc = listaInsumosEK.Where(y => y.insumo == (int)x.insumo).Select(z => (string)z.descripcion).FirstOrDefault(),
                            proveedor = x.proveedor,
                            articulo = x.articulo,
                            unidad = x.unidad,
                            precio = x.precio
                        });

                        resultado.Add("data", listaLicitacion);
                        break;
                    case TipoConsignaLicitacionConvenioEnum.CONVENIO:
                        var listaConvenio = _context.tblCom_InsumosConvenio.Where(x => x.registroActivo && x.proveedor == proveedor).ToList().Select(x => new
                        {
                            id = x.id,
                            insumo = x.insumo,
                            insumoDesc = listaInsumosEK.Where(y => y.insumo == (int)x.insumo).Select(z => (string)z.descripcion).FirstOrDefault(),
                            proveedor = x.proveedor,
                            articulo = "",
                            unidad = "",
                            precio = x.precio
                        });

                        resultado.Add("data", listaConvenio);
                        break;
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetArticulosConsignaLicitacionConvenioPorProveedor", null, AccionEnum.CONSULTA, 0, new { proveedor = proveedor, tipo = tipo });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetInsumoProveedorConsigna(int insumo, int proveedor)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                resultado.Add("data", _context.tblCom_InsumosConsigna.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumoProveedorConsigna", null, AccionEnum.CONSULTA, 0, new { insumo = insumo, proveedor = proveedor });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetInsumoProveedorConvenio(int insumo, int proveedor)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                resultado.Add("data", _context.tblCom_InsumosConvenio.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumoProveedorConvenio", null, AccionEnum.CONSULTA, 0, new { insumo = insumo, proveedor = proveedor });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }

        public Dictionary<string, object> GetInsumoProveedorLicitacion(int insumo, int proveedor)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                resultado.Add("data", _context.tblCom_InsumosLicitacion.FirstOrDefault(x => x.registroActivo && x.insumo == insumo && x.proveedor == proveedor));
                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "RequisicionController", "GetInsumoProveedorLicitacion", null, AccionEnum.CONSULTA, 0, new { insumo = insumo, proveedor = proveedor });
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}
