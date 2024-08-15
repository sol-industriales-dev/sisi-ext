using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Inventario;
using Data.DAO.Maquinaria;
using Data.Factory.Maquinaria.Captura;
using Data.Factory.Maquinaria.Catalogos;
using Data.Factory.Maquinaria.Inventario;
using Data.Factory.Principal.Usuarios;
using Infrastructure.DTO;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGOPLAN.Controllers.Maquinaria.Inventario
{
    public class MovimientosInternosController : BaseController
    {

        ConciliacionFactoryServices conciliacionFactoryServices = new ConciliacionFactoryServices();
        CentroCostosFactoryServices centroCostosFactoryServices = new CentroCostosFactoryServices();
        UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        ControlInternoMovimientoFactoryServices controlInternoMovimientoFactoryServices = new ControlInternoMovimientoFactoryServices();
        MaquinaFactoryServices maquinaFactoryServices = new MaquinaFactoryServices();
        AutorizaMovimientosInternosFactoryServices autorizaMovimientosInternosFactoryServices = new AutorizaMovimientosInternosFactoryServices();

        // GET: MovimientosInternos 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Autorizaciones()
        {
            return View();
        }

        public ActionResult FillCboEconomicos(string cc)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var res = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().FillCboEconomicos(cc).Select(x => new ComboDTO
                {
                    Value = x.id,
                    Text = x.noEconomico

                });
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboCentroCostos()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().getCentrosCostos(getUsuario().id);

                foreach (var item in res)
                {
                    item.Text = item.Value + "-" + centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(item.Value);


                }
                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillCboEconomicosRecepcion(string obj)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().getCentrosCostosRecepcion(obj);

                result.Add(ITEMS, res);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DataEconomico(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GetDataEconomicoID(id);
                result.Add("Descripcion", res.descripcion);
                result.Add("Marca", res.marca.descripcion);
                result.Add("Modelo", res.modeloEquipo.descripcion);
                result.Add("Serie", res.noSerie);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadFolio()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var res = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().LoadFolio();
                result.Add("Folio", res);

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarActualizar(ControlMovimientosInternosDTO obj)
        {
            var result = new Dictionary<string, object>();
            try
            {

                tblM_ControMovimientoInterno objDatos = new tblM_ControMovimientoInterno();
                bool nuevo = false;

                if (obj.id == 0)
                {
                    nuevo = true;
                }

                objDatos.Bateria = obj.Bateria;
                objDatos.Combustible = obj.Combustible;
                objDatos.Comentario = obj.Comentario;
                objDatos.EconomicoID = obj.EconomicoID;
                objDatos.Destino = obj.Destino;
                objDatos.Envio = obj.Envio;
                objDatos.Estatus = obj.Estatus;
                objDatos.Folio = obj.Folio;
                objDatos.Horometro = obj.Horometro;
                objDatos.id = obj.id;
                objDatos.Marca2 = obj.Marca2;
                objDatos.Registro = obj.Registro;
                objDatos.Serie2 = obj.Serie2;
                objDatos.FechaCaptura = DateTime.Now;
                objDatos.Combustible = obj.Combustible;
                objDatos.Estatus = 0;
                objDatos.EstadoRegistro = false;
                controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GuardarActualizar(objDatos);

                if (nuevo)
                {
                    tblM_AutorizaMovimientoInterno objAutorizadores = new tblM_AutorizaMovimientoInterno();
                    int envia = 0;
                    int recibe = 0;
                    int valida = 0;

                    envia = autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GetAutorizadores(1, obj.Envio);
                    recibe = autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GetAutorizadores(2, obj.Destino);
                    valida = autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GetAutorizadores(3, "*");
                    DateTime fecha = new DateTime();
                    string Cadena = "";
                    string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                    Cadena = getUsuario().id.ToString() + f + ("A");


                    objAutorizadores.usuarioEnvio = envia;
                    objAutorizadores.usuarioValida = valida;
                    objAutorizadores.usuarioRecibe = recibe;
                    objAutorizadores.cadenafirmaEnterado = "";
                    objAutorizadores.cadenafirmaEnvia = envia == recibe ? Cadena : "";
                    objAutorizadores.cadenafirmaRecibe = "";
                    objAutorizadores.firmaEnterado = false;
                    objAutorizadores.firmaEnvio = true;
                    objAutorizadores.firmaRecibe = false;
                    objAutorizadores.ControMovimientoInternoID = objDatos.id;
                    objAutorizadores.FechaCaptura = DateTime.Now;
                    objAutorizadores.Autoriza1 = 0;
                    objAutorizadores.Autoriza2 = envia == recibe ? 1 : 0;
                    objAutorizadores.Autoriza3 = 0;
                    autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GuardarActualizar(objAutorizadores);

                    List<string> Correos = new List<string>();

                    //Correos.Add(GetCorreo(1123));
                    Correos.Add(GetCorreo(6143));

                    string Otros = GetUsuarioEnviaCorreoCC(obj.Envio);

                    
                    
                    /*AGREGAR METODO DE ENVIO DE CORREOS ESTA PENDIENTE 06112018*/


                    if (!string.IsNullOrEmpty(Otros))
                        Correos.Add(Otros);

                    var maquinaria = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(obj.EconomicoID).FirstOrDefault();

                    if (envia == recibe && envia == getUsuario().id)
                    {
                        AutorizacionFun(objDatos.id, "Envia", true, false);
                        AutorizacionFun(objDatos.id, "Recibe", true, false);

                        var correoGerenteMaquinari = usuarioFactoryServices.getUsuarioService().correoPerfil(8, obj.Destino).Distinct();

                        Correos.Add(GetCorreo(envia));

                        GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación Movimiento Interno ", AsuntoCorreoString(maquinaria.noEconomico, maquinaria.id, obj.Envio, obj.Destino, 1), Correos, null, "Archivo");
                    }
                    else
                    {
                        GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación Movimiento Interno ", AsuntoCorreoString(maquinaria.noEconomico, maquinaria.id, obj.Envio, obj.Destino, 1), Correos, null, "Archivo");
                    }
                }

                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetUsuarioEnviaCorreoCC(string p)
        {
            switch (p)
            {
                case "040":
                case "043":
                case "044":
                case "045":
                case "C68":
                case "C69":
                    return "e.flores@construplan.com.mx>";
                case "C60":
                case "C65":
                case "C70":
                case "C71":
                case "C72":
                    return "d.laborin@construplan.com.mx";
                default:
                    return "";
            }
        }

        public ActionResult PendientesAutorizacion(int filtro)
        {
            var result = new Dictionary<string, object>();
            try
            {

                var objListaAutorizaciones = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GetControlesRealizados(filtro);
                var data = objListaAutorizaciones.Select(x => new
                {
                    id = x.id,
                    Economico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(x.EconomicoID).FirstOrDefault().noEconomico,
                    Envio = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.Envio),
                    Destino = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(x.Destino),
                    Fecha = x.FechaCaptura.ToShortDateString(),

                    Folio = x.Folio
                }).ToList();
                result.Add("ListaAutorizaciones",data );
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAutorizadores(int ControMovimientoInternoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var ObjAutorizacion = autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GetAutorizadores(ControMovimientoInternoID);

                var usuarioActual = getUsuario().id;
                string GetTipoAutorizacion = "";

                if (getAction("ValidaFirma"))
                {
                    GetTipoAutorizacion = "Valida";
                }

                if (ObjAutorizacion.usuarioEnvio == usuarioActual)
                {
                    GetTipoAutorizacion = "Envia";
                }

                if (ObjAutorizacion.usuarioRecibe == usuarioActual)
                {
                    GetTipoAutorizacion = "Recibe";
                }

                if (ObjAutorizacion.usuarioValida == usuarioActual)
                {
                    GetTipoAutorizacion = "Valida";
                }


                AutorizadoresDTO usuarioRecibe = new AutorizadoresDTO();
                AutorizadoresDTO usuarioEnvio = new AutorizadoresDTO();
                AutorizadoresDTO usuarioValida = new AutorizadoresDTO();

                var usuarioRecibeObj = usuarioFactoryServices.getUsuarioService().ListUsersById(ObjAutorizacion.usuarioRecibe).FirstOrDefault();
                usuarioRecibe.firma = ObjAutorizacion.firmaRecibe;
                usuarioRecibe.firmaCadena = ObjAutorizacion.cadenafirmaRecibe;
                usuarioRecibe.nombreUsuario = usuarioRecibeObj.nombre + " " + usuarioRecibeObj.apellidoPaterno + " " + usuarioRecibeObj.apellidoMaterno;
                usuarioRecibeObj = usuarioFactoryServices.getUsuarioService().ListUsersById(ObjAutorizacion.usuarioEnvio).FirstOrDefault();
                usuarioEnvio.firma = ObjAutorizacion.firmaEnvio;
                usuarioEnvio.firmaCadena = ObjAutorizacion.cadenafirmaEnvia;
                usuarioEnvio.nombreUsuario = usuarioRecibeObj.nombre + " " + usuarioRecibeObj.apellidoPaterno + " " + usuarioRecibeObj.apellidoMaterno;

                usuarioRecibeObj = usuarioFactoryServices.getUsuarioService().ListUsersById(ObjAutorizacion.usuarioValida).FirstOrDefault();
                usuarioValida.firma = ObjAutorizacion.firmaEnterado;
                usuarioValida.firmaCadena = ObjAutorizacion.cadenafirmaEnterado;
                usuarioValida.nombreUsuario = usuarioRecibeObj.nombre + " " + usuarioRecibeObj.apellidoPaterno + " " + usuarioRecibeObj.apellidoMaterno;


                if (string.IsNullOrEmpty(GetTipoAutorizacion))
                {
                    GetTipoAutorizacion = "SoloLectura";
                }

                result.Add("Autoriza1", ObjAutorizacion.Autoriza1);
                result.Add("Autoriza2", ObjAutorizacion.Autoriza2);
                result.Add("Autoriza3", ObjAutorizacion.Autoriza3);

                result.Add("usuarioValida", usuarioValida);
                result.Add("usuarioEnvio", usuarioEnvio);
                result.Add("usuarioRecibe", usuarioRecibe);
                result.Add("GetTipoAutorizacion", GetTipoAutorizacion);
                result.Add("ControMovimientoInternoID", ControMovimientoInternoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Autorizacion(int obj, string Autoriza, bool tipo)
        {
            var result = new Dictionary<string, object>();
            try
            {
                AutorizacionFun(obj, Autoriza, tipo, true);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private void AutorizacionFun(int ControMovimientoInternoID, string tipoAutorizacion, bool tipo, bool EnviaCorreo)
        {
            var autorizacion = autorizaMovimientosInternosFactoryServices
                .getAutorizaMovimientosInternosFactoryServices()
                .GetAutorizadores(ControMovimientoInternoID);

            DateTime fecha = DateTime.Now;
            string Cadena = "";
            bool esUsuarioRecibe = false;
            switch (tipoAutorizacion)
            {
                case "Envia":
                    {
                        autorizacion.Autoriza2 = 1;
                        autorizacion.firmaEnvio = tipo;
                        string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                        Cadena = getUsuario().id.ToString() + f + (tipo ? "A" : "R");
                        autorizacion.cadenafirmaEnvia = Cadena;

                        break;
                    }

                case "Recibe":
                    {
                        autorizacion.Autoriza3 = 1;
                        autorizacion.firmaRecibe = tipo;
                        string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                        Cadena = getUsuario().id.ToString() + f + (tipo ? "A" : "R");
                        autorizacion.cadenafirmaRecibe = Cadena;
                        esUsuarioRecibe = true;

                        break;
                    }

                case "Valida":
                    {
                        autorizacion.Autoriza1 = 1;
                        autorizacion.firmaEnterado = tipo;
                        string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                        Cadena = getUsuario().id.ToString() + f + (tipo ? "A" : "R");
                        autorizacion.cadenafirmaEnterado = Cadena;

                        break;
                    }
                default:
                    break;
            }

            autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GuardarActualizar(autorizacion, esUsuarioRecibe);

            if (tipoAutorizacion.Equals("Envia"))
            {

                autorizacion.Autoriza1 = 1;
                autorizacion.firmaEnterado = tipo;
                string f = fecha.ToString("ddMMyyyy") + fecha.Hour + "" + fecha.Minute;
                Cadena = getUsuario().id.ToString() + f + (tipo ? "A" : "R");
                autorizacion.cadenafirmaEnterado = Cadena;

                autorizaMovimientosInternosFactoryServices.getAutorizaMovimientosInternosFactoryServices().GuardarActualizar(autorizacion);

                var Maquinaria = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GetDataEconomicoID(autorizacion.ControMovimientoInterno.EconomicoID);
                string CentroCostos = autorizacion.ControMovimientoInterno.Destino; ;
                Maquinaria.centro_costos = CentroCostos;
                maquinaFactoryServices.getMaquinaServices().Guardar(Maquinaria);


                var ControlInternoObj = autorizacion.ControMovimientoInterno;
                ControlInternoObj.EstadoRegistro = true;
                ControlInternoObj.Estatus = 0;
                controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GuardarActualizar(ControlInternoObj);

                List<string> Correos = new List<string>();
                Correos.Add(GetCorreo(1126));
                //Correos.Add(GetCorreo(1123));
                Correos.Add(GetCorreo(6143));

                Correos.Add(GetCorreo(autorizacion.usuarioEnvio));
                Correos.Add(GetCorreo(autorizacion.usuarioRecibe));
                GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación Movimiento Interno ", AsuntoCorreoString(Maquinaria.noEconomico, Maquinaria.id, autorizacion.ControMovimientoInterno.Envio, autorizacion.ControMovimientoInterno.Destino, 2), Correos, null, "Archivo");
            }
            else if (tipoAutorizacion.Equals("Recibe"))
            {

                var Maquinaria = controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GetDataEconomicoID(autorizacion.ControMovimientoInterno.EconomicoID);
                string CentroCostos = autorizacion.ControMovimientoInterno.Destino;
                Maquinaria.centro_costos = CentroCostos;
                maquinaFactoryServices.getMaquinaServices().Guardar(Maquinaria);

                var ControlInternoObj = autorizacion.ControMovimientoInterno;
                ControlInternoObj.EstadoRegistro = true;
                ControlInternoObj.Estatus = 1;
                controlInternoMovimientoFactoryServices.getControlInternoMovimientoFactoryServices().GuardarActualizar(ControlInternoObj);
                List<string> Correos = new List<string>();
                //Correos.Add(GetCorreo(1123));
                Correos.Add(GetCorreo(autorizacion.usuarioEnvio));
                Correos.Add(GetCorreo(autorizacion.usuarioRecibe));
                //if (EnviaCorreo)
                //{
                GlobalUtils.sendEmailAdjuntoInMemorySend("Notificación Movimiento Interno ", AsuntoCorreoString(Maquinaria.noEconomico, Maquinaria.id, autorizacion.ControMovimientoInterno.Envio, autorizacion.ControMovimientoInterno.Destino, 3), Correos, null, "Archivo");

                //}

            }
        }

        private string AsuntoCorreoString(string economico, int economicoid, string CCOrigen, string CcDestino, int tipo)
        {
            string textoAsuntoCorreo = "";

            switch (tipo)
            {
                case 1:
                    {
                        textoAsuntoCorreo = "Se Genero un control de movimiento interno, del Economico ";
                        textoAsuntoCorreo += economico + " con el origen " + CCOrigen + " con destino " + CcDestino;
                    }
                    break;
                case 2:
                    {
                        textoAsuntoCorreo = "Se genero el control de envio del economico ";
                        textoAsuntoCorreo += economico + " con el origen " + CCOrigen + " con destino " + CcDestino;
                    }
                    break;
                case 3:
                    {
                        textoAsuntoCorreo = "Se genero el control de recepcion del economico ";
                        textoAsuntoCorreo += economico + " con el origen " + CCOrigen + " con destino " + CcDestino;
                    }
                    break;
                case 4:
                    {

                        var datosEconomico = maquinaFactoryServices.getMaquinaServices().GetMaquinaByID(economicoid);
                        if (datosEconomico.FirstOrDefault() != null)
                        {
                            var econoOBj = datosEconomico.FirstOrDefault();
                            var DestinoObj = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(CcDestino);
                            var OrigenObj = centroCostosFactoryServices.getCentroCostosService().getNombreCCFix(CCOrigen);

                            textoAsuntoCorreo = "Buen Día<br> Se Agrego un equipo al area cuenta " + DestinoObj + " (" + CcDestino + ") <br>";
                            textoAsuntoCorreo += "Economico: " + econoOBj.noEconomico +
                                                 "Descripción: " + (econoOBj.grupoMaquinaria != null ? econoOBj.grupoMaquinaria.descripcion : "N/A") + " <br>" +
                                                 "Marca: " + (econoOBj.marca != null ? econoOBj.marca.descripcion : "N/A") + " <br>" +
                                                 "Modelo: " + (econoOBj.modeloEquipo != null ? econoOBj.modeloEquipo.descripcion : "N/A") + " <br>" +

                                                 "Origen : " + OrigenObj + "  AL " + DestinoObj;


                        }
                        else
                        {
                            return "";
                        }





                    }
                    break;
                default:
                    break;
            }


            var AsuntoCorreo = @"<html>    <head>
                                    <style>
                                    table {
                                        font-family: arial, sans-serif;
                                        border-collapse: collapse;
                                        width: 100%;
                                    }

                                    td, th {
                                        border: 1px solid #dddddd;
                                        text-align: left;
                                        padding: 8px;
                                    }

                                    tr:nth-child(even) {
                                        background-color: #dddddd;
                                    }
                                    </style>
                                </head>
                                <body>"
                                    + textoAsuntoCorreo +
                                "</body>" +
                               " </html>";


            return AsuntoCorreo;
        }

        private string GetCorreo(int id)
        {
            return usuarioFactoryServices.getUsuarioService().ListUsersById(id).FirstOrDefault().correo;
        }
    }
}