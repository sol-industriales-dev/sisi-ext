using Core.DAO.Administracion.Seguridad;
using Core.DAO.Principal.Alertas;
using Core.DAO.Principal.Usuarios;
using Core.DAO.RecursosHumanos.Captura;
using Core.DTO;
using Core.DTO.RecursosHumanos;
using Core.DTO.RecursosHumanos.FormatoCambio;
using Core.DTO.Utils.Auth;
using Core.Entity.Principal.Alertas;
using Core.Entity.RecursosHumanos.Captura;
using Core.Enum.Maquinaria.Reportes;
using Core.Enum.Principal;
using Core.Enum.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Data.DAO.Principal.Usuarios;
using Data.Factory.Administracion.Seguridad.Capacitacion;
using Data.Factory.Principal.Alertas;
using Data.Factory.Principal.Usuarios;
using Data.Factory.RecursosHumanos.Captura;
using Infrastructure.Utils;
using SIGOPLAN.Areas.Administrativo.Models.ViewModels.RecursosHumanos;
using SIGOPLAN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Enum.Principal.Usuario;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using Core.DAO.Maquinaria.Reporte;
using Data.Factory.Maquinaria.Reporte;
using Core.Enum.RecursosHumanos;
using System.Reflection;
using Reportes.Reports.RecursosHumanos;


namespace SIGOPLAN.Areas.Administrativo.Controllers.RecursosHumanos.FormatoCambio
{
    public class FormatoCambioController : BaseController
    {
        IFormatoCambio capturaFormatoCambioFS;
        IAutorizacionFormatoCambio authFormatoCambioFS;
        IUsuarioDAO usuarioFS;
        IAlertasDAO alertasFS;
        IBonoDAO bonoFS;
        ICapacitacionDAO capacitacionService;

        string RutaServidor = "";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            usuarioFS = new UsuarioFactoryServices().getUsuarioService();
            capturaFormatoCambioFS = new FormatoCambioFactoryService().getFormatoCambioService();
            authFormatoCambioFS = new AutorizacionFormatoCambioFactoryService().getAutorizacionFormatoCambioService();
            alertasFS = new AlertaFactoryServices().getAlertaService();
            bonoFS = new BonoFactoryService().getBonoService();
            capacitacionService = new CapacitacionFactoryService().GetCapacitacionService();
            

            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index(int obj = 0)
        {
            CancelarFormatoCambioPorTiempo();


            return View();
        }
        public ActionResult CapturaFormato(int id = 0)
        {
            ViewBag.permisoVerSueldos = capturaFormatoCambioFS.GetPermisoSueldos();
            ViewBag.IdFormato = id;
            ViewBag.esEditarPuestos = capturaFormatoCambioFS.CheckEsEditarPuestos();
            ViewBag.Empresa = vSesiones.sesionEmpresaActual;
            return View();
        }

        [HttpPost]
        public ActionResult SolicitarAprobacion(tblRH_FormatoCambio objEmpleadoCambio, List<tblRH_AutorizacionFormatoCambio> lstAutorizacion)
        {
            //objEmpleadoCambio.Aprobado = false;//analisis pone como pendiente ya aprobada al realizar la modificacion
            var result = new Dictionary<string, object>();
            var objFormatoCambioDTO = new CatFormatoCambioDTO();
            var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
            var modAutorizantes = false;
            try
            {
                objEmpleadoCambio.usuarioCap = base.getUsuario().id;
                objEmpleadoCambio.nomUsuarioCap = base.getUsuario().nombre;
                if (objEmpleadoCambio.id == 0)
                {
                    objEmpleadoCambio.Aprobado = false;
                    modAutorizantes = true;
                    var objEnk = capturaFormatoCambioFS.getEmpleadoForId(objEmpleadoCambio.Clave_Empleado, true);
                    objEmpleadoCambio.SalarioAnt = objEnk.Salario_Base;
                    objEmpleadoCambio.ComplementoAnt = objEnk.Complemento;
                    objEmpleadoCambio.CCAntID = objEnk.CcID;
                    objEmpleadoCambio.CCAnt = objEnk.CC;
                    objEmpleadoCambio.BonoAnt = objEnk.Bono;

                    objEmpleadoCambio.PuestoAnt = objEnk.Puesto;
                    objEmpleadoCambio.Nombre_Jefe_InmediatoAnt = objEnk.Nombre_Jefe_Inmediato;
                    objEmpleadoCambio.TipoNominaAnt = objEnk.TipoNomina;
                    objEmpleadoCambio.RegistroPatronalAnt = objEnk.RegistroPatronal;
                    objEmpleadoCambio.fechaCaptura = DateTime.Now;
                    objEmpleadoCambio.Fecha_Alta = objEnk.Fecha_Alta;
                    objFormatoCambioDTO.objFormatoCambio = capturaFormatoCambioFS.SaveChangesEmpleado(objEmpleadoCambio);
                    objFormatoCambioDTO.objFormatoCambio.folio = objFormatoCambioDTO.objFormatoCambio.CcID + "-" + objFormatoCambioDTO.objFormatoCambio.id;
                    objFormatoCambioDTO.objFormatoCambio = capturaFormatoCambioFS.SaveChangesEmpleado(objFormatoCambioDTO.objFormatoCambio);
                }
                else
                {
                    var objTemp = capturaFormatoCambioFS.getFormatoCambioByID(objEmpleadoCambio.id);
                    objEmpleadoCambio.Aprobado = objTemp.Aprobado;
                    objEmpleadoCambio.SalarioAnt = objTemp.SalarioAnt;
                    objEmpleadoCambio.ComplementoAnt = objTemp.ComplementoAnt;
                    objEmpleadoCambio.CCAntID = objTemp.CcID;
                    objEmpleadoCambio.CCAnt = objTemp.CC;
                    objEmpleadoCambio.BonoAnt = objTemp.BonoAnt;

                    objEmpleadoCambio.PuestoAnt = objTemp.Puesto;
                    objEmpleadoCambio.RegistroPatronalAnt = objTemp.RegistroPatronal;
                    objEmpleadoCambio.TipoNominaAnt = objTemp.TipoNomina;
                    objEmpleadoCambio.Nombre_Jefe_InmediatoAnt = objTemp.Nombre_Jefe_Inmediato;
                    objEmpleadoCambio.folio = objTemp.folio;
                    objEmpleadoCambio.Fecha_Alta = objTemp.Fecha_Alta;
                    objFormatoCambioDTO.objFormatoCambio = capturaFormatoCambioFS.SaveChangesEmpleado(objEmpleadoCambio);
                    objFormatoCambioDTO.objFormatoCambio.folio = objTemp.folio;
                    var usuariosFormatoCambios = authFormatoCambioFS.getAutorizacion(objEmpleadoCambio.id);
                    int cont = 0;
                    if (objEmpleadoCambio.id != 0)
                    {
                        foreach (var objRemplazo in lstAutorizacion.OrderBy(x => x.Orden))
                        {
                            foreach (var objAutorizadores in usuariosFormatoCambios.OrderBy(x => x.Orden))
                            {

                                //compara si el aprobador tiene diferente clave y el orden es el mismo
                                if (objAutorizadores.Clave_Aprobador != objRemplazo.Clave_Aprobador && objAutorizadores.Orden == objRemplazo.Orden && cont == 0)
                                {
                                    cont = cont + 1;
                                    enviarCorreos(objAutorizadores.Clave_Aprobador, objEmpleadoCambio.id, "remplazo", objRemplazo.Orden);//Remplazo de autorizador
                                    var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == objAutorizadores.Clave_Aprobador && x.objID.Equals(objAutorizadores.Id_FormatoCambio) && x.visto == false);
                                    if (AletaUpdate != null)
                                    {
                                        AletaUpdate.visto = true;//se cambia estatus a visto para que no le aparezca mas en la bandeja
                                        alertasFS.updateAlerta(AletaUpdate);
                                    }
                                }
                            }
                        }



                        //if ( lstAutorizacion.FirstOrDefault().Clave_Aprobador!=usuariosFormatoCambios.FirstOrDefault().Clave_Aprobador)
                        //{
                        //    objAutorizadorRemplazado = usuariosFormatoCambios.FirstOrDefault();
                        //    enviarCorreos(objAutorizadorRemplazado.Clave_Aprobador, objEmpleadoCambio.id, "remplazo");//Remplazo de autorizador
                        //    //modificar alerta
                        //    var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == objAutorizadorRemplazado.Clave_Aprobador && x.objID.Equals(objAutorizadorRemplazado.Id_FormatoCambio) && x.visto == false);
                        //      if (AletaUpdate != null)
                        //      {
                        //          AletaUpdate.visto = true;//se cambia estatus a visto para que no le aparezca mas en la bandeja
                        //          alertasFactoryServices.updateAlerta(AletaUpdate);
                        //      }
                        //}
                    }


                    foreach (var i in usuariosFormatoCambios)
                    {
                        if (lstAutorizacion.FirstOrDefault(x => x.Clave_Aprobador == i.Clave_Aprobador && x.Orden == i.Orden) == null)
                        {
                            modAutorizantes = true;
                        }
                    }
                    if (usuariosFormatoCambios.Count != lstAutorizacion.Count)
                    {
                        modAutorizantes = true;
                        //foreach (var autorizador in usuariosFormatoCambios)
                        //{
                        //    var UsuarioEliminado = lstAutorizacion.Where(x => x.Clave_Aprobador.Equals(autorizador.Clave_Aprobador) && x.PuestoAprobador.Equals(autorizador.PuestoAprobador)).ToList();
                        //    if (UsuarioEliminado.Count <= 0)
                        //    {
                        //        var useObj = UsuarioEliminado.FirstOrDefault();
                        //        capturaAutorizacionFormatoCambioService.EliminarAutorizador(autorizador);
                        //        var objAlertaDis = new tblP_Alerta();
                        //        var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == autorizador.Clave_Aprobador && x.objID.Equals(autorizador.Id_FormatoCambio));
                        //        if (AletaUpdate != null)
                        //            alertasFactoryServices.updateAlerta(AletaUpdate);
                        //    }
                        //}
                    }
                }
                objFormatoCambioDTO.objAutorizacion = new List<tblRH_AutorizacionFormatoCambio>();

                if (modAutorizantes)
                {

                    authFormatoCambioFS.EliminarAutorizadores(objEmpleadoCambio.id);
                    foreach (var objAutorizacion in lstAutorizacion)
                    {
                        objAutorizacion.Id_FormatoCambio = objEmpleadoCambio.id;
                        objAutorizacion.id = 0;
                        if (objAutorizacion.id == 0)
                        {
                            objAutorizacion.Firma = "S/F";
                        }

                        if (objAutorizacion.Orden == 1)
                        {
                            objAutorizacion.Autorizando = true;
                        }
                        var aprobacion = authFormatoCambioFS.SaveChangesAutorizacionCambios(objAutorizacion);
                        objFormatoCambioDTO.objAutorizacion.Add(aprobacion);
                    }
                    var siguiente = lstAutorizacion.FirstOrDefault(w => lstAutorizacion.Where(a => a.Autorizando).Max(m => m.Orden).Equals(w.Orden));
                    if (siguiente.Orden == 1)
                    {
                        var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == siguiente.Clave_Aprobador && x.objID.Equals(siguiente.id) && x.moduloID == 22 && x.visto == false);
                        var esNuevo = AletaUpdate == null;
                        if (esNuevo)
                        {
                            AletaUpdate = new tblP_Alerta();
                        }
                        AletaUpdate.msj = "Firma-Formato de Cambio " + objEmpleadoCambio.folio;
                        AletaUpdate.sistemaID = (int)SistemasEnum.RH;
                        AletaUpdate.documentoID = (int)ReportesEnum.Formato_Cambio;
                        AletaUpdate.moduloID = (int)BitacoraEnum.FORMATOCAMBIORH;
                        AletaUpdate.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                        AletaUpdate.url = "/Administrativo/FormatoCambio/Index?obj=" + siguiente.id + "";
                        AletaUpdate.userEnviaID = base.getUsuario().id;
                        AletaUpdate.userRecibeID = siguiente.Clave_Aprobador;
                        AletaUpdate.objID = siguiente.id;
                        if (esNuevo)
                        {
                            alertasFS.saveAlerta(AletaUpdate);
                        }
                        else
                        {
                            alertasFS.updateAlerta(AletaUpdate);
                        }
                    }
                }
                result.Add("idFormatoCambios", objFormatoCambioDTO.objFormatoCambio.id);
                result.Add("usuarioEnvia", 0);
                result.Add("objFormatoCambioDTO", objFormatoCambioDTO);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Clear();
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult enviarCorreos(int usuariorecibe, int formatoID, string tipo, int orden)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioEnvia = usuarioFS.ListUsersById(base.getUsuario().id).FirstOrDefault();
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];
                var usuariosFormatoCambios = authFormatoCambioFS.getAutorizacion(formatoID);
                List<string> CorreoEnviar = new List<string>();
                string AsuntoCorreo = @"<html>
                                            <head>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                var folioID = usuariosFormatoCambios.FirstOrDefault().Id_FormatoCambio;
                var folio = capturaFormatoCambioFS.getFormatoCambioByID(folioID).folio;
                if (tipo.Equals("nuevo"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que se registro un nuevo Formato de Cambios con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (tipo.Equals("cambio"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que fue realizada una modificación en el Formato de Cambios con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (tipo.Equals("remplazo"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal style='font-weight:bold;'>
                                                        Se informa que su colaboracion como autorizador ha sido relevada en el Formato de Cambios con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (tipo.Equals("autoriza"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                                        Se informa que fue realizada una autorización en el Formato de Cambios con Folio: &#8220;" + folio + @"&#8221 por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                                    </p>";
                }
                else if (tipo.Equals("rechazar"))
                {
                    AsuntoCorreo += @" <p class=MsoNormal>
                                            Se informa que el Formato de Cambios con Folio: &#8220;" + folio + @"&#8221 fue rechazado por el usuario (" + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + @").<o:p></o:p>
                                        </p>";
                    var autorizacionFormato = authFormatoCambioFS

                        .getAutorizacion(formatoID)
                        .Where(x => x.comentario != null)
                        .FirstOrDefault();
                    if (autorizacionFormato != null && autorizacionFormato.comentario != null)
                    {
                        AsuntoCorreo += @" <p class=MsoNormal>
                                                <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode(autorizacionFormato.comentario) + @"<o:p></o:p>
                                            </p>";
                    }
                }
                AsuntoCorreo += @" <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p><br/><br/>
                                                        <table>
                                                            <thead>
                                                              <tr>
                                                                <th>Nombre Autorizador </th>
                                                                <th>Tipo</th>
                                                                <th>Autorizó</th>
                                                              </tr></thead>
                                                            <tbody>";

                var excepcionesCorreo = usuarioFS.getPermisosAutorizaCorreo(1);
                List<int> excepcionesCorreoIDs = new List<int>();
                if (excepcionesCorreo.Count > 0)
                {
                    excepcionesCorreoIDs.AddRange(excepcionesCorreo.Select(x => x.usuarioID));
                }
                int cont = 0;
                int lonusuariosFormatoCambios = usuariosFormatoCambios.Count();
                foreach (var i in usuariosFormatoCambios)
                {
                    if (tipo.Equals("remplazo"))
                    {
                        cont = cont + 1;
                        if (cont == orden)
                        {
                            AsuntoCorreo += @"<tr>
                                               <td>" + i.Nombre_Aprobador + "</td>" +
                                        "<td>" + i.PuestoAprobador + "</td>" +
                                             "<td style='background-color: red;'>Remplazo</td>" +
                                        "</tr>";
                        }
                        else if (lonusuariosFormatoCambios == cont)
                        {
                            AsuntoCorreo += @"<tr>
                                               <td>" + i.Nombre_Aprobador + "</td>" +
                                        "<td>" + i.PuestoAprobador + "</td>" +
                                             "<td style='background-color: red;'>Remplazo</td>" +
                                        "</tr>";
                        }
                        else
                        {
                            AsuntoCorreo += @"<tr>
                                                                <td>" + i.Nombre_Aprobador + "</td>" +
                                    "<td>" + i.PuestoAprobador + "</td>" +
                                         "<td>Pendiente</td>" +
                                    "</tr>";
                        }
                    }
                    else
                    {
                        AsuntoCorreo += @"<tr>
                                                                <td>" + i.Nombre_Aprobador + "</td>" +
                                    "<td>" + i.PuestoAprobador + "</td>" +
                                        getEstatus(i) +
                                    "</tr>";
                    }
                    var usuarioCorreo = usuarioFS.ListUsersById(i.Clave_Aprobador).FirstOrDefault();
                    if (i.Autorizando)
                    {
                        CorreoEnviar.Add(usuarioCorreo.correo);
                    }
                    else
                    {
                        if (!excepcionesCorreoIDs.Contains(i.Clave_Aprobador))
                        {
                            CorreoEnviar.Add(usuarioCorreo.correo);
                        }
                    }
                }
                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Favor de ingresar al sistema SISI, en el apartado de CH en la opción Formato de cambios<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SISI no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";

              /*  var diana = usuarioFS.ListUsersById(1019).FirstOrDefault();
                
                CorreoEnviar.Add(usuarioEnvia.correo);
                CorreoEnviar.Add(diana.correo);
                var aranza = usuarioFS.ListUsersById(79552).FirstOrDefault();
                CorreoEnviar.Add(aranza.correo);*/
                try
                {
                    var formato = capturaFormatoCambioFS.getFormatoCambioByID(formatoID);
                    var cap = usuarioFS.ListUsersById(formato.usuarioCap).FirstOrDefault();
                    CorreoEnviar.Add(cap.correo);
                }
                catch (Exception e) { }

                var tipoFormato = "FormatoCambios.pdf";
                #region Remover_Gerardo Reina de seguimiento una ves autorizado
          /*      try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = authFormatoCambioFS.getAutorizacion(formatoID);
                        var greina = autorizadores.FirstOrDefault(x => x.Clave_Aprobador == 1164);
                        if (greina != null)
                        {
                            if (greina.Estatus || greina.Rechazado)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch { }*/
                #endregion

#if DEBUG
         

#endif
               // CorreoEnviar.Remove("keyla.vasquez@construplan.com.mx");
                var correoEnviado = GlobalUtils.sendEmailAdjuntoInMemory2("Alerta de Autorizaciones 'Cambios'", AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                Session["downloadPDF"] = null;
                result.Add(SUCCESS, correoEnviado);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult enviarCorreofin(List<string> CorreoEnviar, string folio, int formatoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var usuarioEnvia = usuarioFS.ListUsersById(base.getUsuario().id).FirstOrDefault();
                var downloadPDF = (List<Byte[]>)Session["downloadPDF"];



                var usuariosFormatoCambios = authFormatoCambioFS.getAutorizacion(formatoID);
                List<string> CorreoEnviarF = new List<string>();
                string AsuntoCorreo = @"<html xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:word' xmlns:m='http://schemas.microsoft.com/office/2004/12/omml' xmlns='http://www.w3.org/TR/REC-html40'>
                                            <head>
                                                <meta http-equiv=Content-Type content='text/html; charset=iso-8859-1'>
                                                <meta name=Generator content='Microsoft Word 15 (filtered medium)'>
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
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>Buen día <o:p></o:p>
                                                        </span>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'><o:p>&nbsp;</o:p>
                                                        </span>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>
                                                            Fue autorizada completamente la solicitud con el folio " + folio + @" del módulo &#8220;Formato de cambios&#8221;<o:p></o:p></span></p><p class=MsoNormal style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'><span style='font-size:12.0pt;font-family:'Arial',sans-serif'>PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                       </span>
                                                    </p><br/><br/>
                                                    <table>
                                                        <thead>
                                                          <tr>
                                                            <th>Nombre Autorizador </th>
                                                            <th>Tipo</th>
                                                            <th>Autorizó</th>
                                                          </tr></thead>
                                                        <tbody>";
                foreach (var i in usuariosFormatoCambios)
                {
                    AsuntoCorreo += @"<tr>
                                                            <td>" + i.Nombre_Aprobador + "</td>" +
                                    "<td>" + i.PuestoAprobador + "</td>" +
                                     getEstatus(i) +
                                  "</tr>";
                    var usuarioCorreo = usuarioFS.ListUsersById(i.Clave_Aprobador).FirstOrDefault();

                    var excepcionesCorreo = usuarioFS.getPermisosAutorizaCorreo(1);

                    if (excepcionesCorreo.Count > 0)
                    {
                        var existUsuario = excepcionesCorreo.FirstOrDefault(x => x.usuarioID == usuarioCorreo.id);

                        if (existUsuario == null)
                        {
                            CorreoEnviarF.Add(usuarioCorreo.correo);
                        }
                    }
                    else
                    {
                        CorreoEnviarF.Add(usuarioCorreo.correo);
                    }

                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                                                    <p class=MsoNormal style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'>
                                                        <span style='font-size:12.0pt;font-family:'Arial',sans-serif'>
                                                            Gracias.<o:p></o:p>
                                                        </span>
                                                    </p>
                                                </div>
                                           </body>
                                        </html>";
                CorreoEnviarF.Add(usuarioEnvia.correo);
              /*  var diana = usuarioFS.ListUsersById(1019).FirstOrDefault();
                CorreoEnviarF.Add(diana.correo);
                var aranza = usuarioFS.ListUsersById(79552).FirstOrDefault();
                CorreoEnviarF.Add(aranza.correo);*/
                try
                {
                    var formato = capturaFormatoCambioFS.getFormatoCambioByID(formatoID);
                    var cap = usuarioFS.ListUsersById(formato.usuarioCap).FirstOrDefault();
                    CorreoEnviar.Add(cap.correo);
                }
                catch (Exception e) { }
                var tipoFormato = "FormatoCambios.pdf";
                CorreoEnviar = CorreoEnviar.Distinct().ToList();
                #region Remover_Gerardo Reina de seguimiento una ves autorizado
               /* try
                {
                    if (CorreoEnviar.Contains("g.reina@construplan.com.mx"))
                    {
                        var autorizadores = authFormatoCambioFS.getAutorizacion(formatoID);
                        var greina = autorizadores.FirstOrDefault(x => x.Clave_Aprobador == 1164);
                        if (greina != null)
                        {
                            if (greina.Estatus || greina.Rechazado)
                            {
                                CorreoEnviar.Remove("g.reina@construplan.com.mx");
                            }
                        }
                        else
                        {

                            CorreoEnviar.Remove("g.reina@construplan.com.mx");
                        }
                    }
                }
                catch { }*/
                #endregion

#if DEBUG
                
#endif

                GlobalUtils.sendEmailAdjuntoInMemory2("Alerta de Autorizaciones", AsuntoCorreo, CorreoEnviarF.Distinct().ToList(), downloadPDF, tipoFormato);
                Session["downloadPDF"] = null;
                enviarCorreoCambioCC(formatoID);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public void enviarCorreoCambioCC(int formatoID)
        {
            try
            {
                var obj = capturaFormatoCambioFS.getFormatoCambioByID(formatoID);
                if (!obj.CCAntID.Equals(obj.CcID))
                {
                    var correos = new List<string>();
                    var titulo = "Alerta cambio de CC a empleado (" + obj.Clave_Empleado + " - " + (obj.Nombre + " " + obj.Ape_Paterno + " " + obj.Ape_Paterno) + ")";
                    var msj = "Se notifica cambio de cc de empleado <b>" + (obj.Nombre + " " + obj.Ape_Paterno + " " + obj.Ape_Paterno) + "</b> con los siguientes datos:<br/>";
                    msj += "<b>Clave Empleado:</b> " + obj.Clave_Empleado + "<br/>";
                    msj += "<b>Nombre Empleado:</b> " + (obj.Nombre + " " + obj.Ape_Paterno + " " + obj.Ape_Paterno) + "<br/>";
                    msj += "<b>CC Origen:</b> " + obj.CCAntID + " - " + obj.CCAnt + "<br/>";
                    msj += "<b>CC Destino:</b> " + obj.CcID + " - " + obj.CC + "<br/>";
                    msj += "<b>Fecha a la que aplica el cambio:</b> " + obj.FechaInicioCambio.ToShortDateString() + "<br/>";
                    msj += "<b>Folio de Formato de Cambio:</b> " + obj.folio + "<br/>";
                    msj += "<br/>Recuerde notificar al empleado sobre el restablecimiento de su Bono.";
                  

#if DEBUG
                   
#endif

                    GlobalUtils.sendEmail(titulo, msj, correos);
                }
            }
            catch (Exception e)
            {

            }
        }
        private string getEstatus(tblRH_AutorizacionFormatoCambio o)
        {
            if (o.Autorizando)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if (o.Estatus)
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else
                if (o.Rechazado == true)
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                else
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }
        public ActionResult getPersimoUsuarioGestionFormato()
        {
            var result = new Dictionary<string, object>();
            try
            {
                var esTodoFormatoCambio = vSesiones.sesionUsuarioDTO.permisos2.Any(p => p.accion.Any(a => a.Accion.Equals("TodoFormatoCambio")));
                var esEliminar = vSesiones.sesionUsuarioDTO.permisos2.Any(p => p.accion.Any(a => a.Accion.Equals("EditarTodo")));
                var ud = new UsuarioDAO();
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var vt = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var et = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodo");
                result.Add("esTodoFormatoCambio", (rh || vt));
                result.Add("esEliminar", et);
                result.Add(SUCCESS, true);
            }
            catch (Exception o_O)
            {
                result.Add("message", o_O.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private delegate bool EsMismoUsuarioCC(int usuarioCapturoID);

        public ActionResult TablaFormatosPendientes(string CC, int claveEmpleado, int id, string estado, string tipo, int numero)
        {
            var result = new Dictionary<string, object>();
            var lstObjFormatosPendientes = new List<CatFormatoCambioDTO>();
            var lstObjEmpleadoCambio = new List<tblRH_FormatoCambio>();
            try
            {
                int intEstado = 1;
                switch (estado)
                {
                    case "Todos":
                        intEstado = 1;
                        break;
                    case "Pendiente":
                        intEstado = 2;
                        break;
                    case "Aprobado":
                        intEstado = 3;
                        break;
                    case "Rechazado":
                        intEstado = 4;
                        break;
                    case "Cancelado":
                        intEstado = 5;
                        break;
                }
                lstObjEmpleadoCambio = capturaFormatoCambioFS.getListFormatosCambioPendientes(id, CC, claveEmpleado, intEstado, tipo, numero);
                int usuarioLog = base.getUsuario().id;
                var listaMostrar = new List<FormatoCambioDTO>();
                var ud = new UsuarioDAO();
                var isVerTodo = ud.getViewAction(vSesiones.sesionCurrentView, "TodoFormatoCambio");
                var rh = ud.getViewAction(vSesiones.sesionCurrentView, "VerTodoFormato");
                var rhEditar = ud.getViewAction(vSesiones.sesionCurrentView, "Editar");

                //CHECK TIPO USUARIO AUDITOR Y SI EL USUARIO ESTA DENTRO DE LA LISTA DE AUTORIZANTES
                bool esAuditor = false;

                //1080 Adriana reina
                if (vSesiones.sesionUsuarioDTO.esAuditor && vSesiones.sesionUsuarioDTO.id != 1080)
                {
                    esAuditor = true;
                }

                foreach (var objFormatoCambio in lstObjEmpleadoCambio)
                {

                    #region DTO
                    var formatoCambio = new FormatoCambioDTO
                    {
                        id = objFormatoCambio.id,
                        Clave_Empleado = objFormatoCambio.Clave_Empleado,
                        Nombre = objFormatoCambio.Nombre,
                        Ape_Paterno = objFormatoCambio.Ape_Paterno,
                        Ape_Materno = objFormatoCambio.Ape_Materno,
                        Fecha_Alta = objFormatoCambio.Fecha_Alta,
                        PuestoID = objFormatoCambio.PuestoID,
                        Puesto = objFormatoCambio.Puesto,
                        TipoNominaID = objFormatoCambio.TipoNominaID,
                        TipoNomina = objFormatoCambio.TipoNomina,
                        CcID = objFormatoCambio.CcID,
                        CC = objFormatoCambio.CC,
                        RegistroPatronalID = objFormatoCambio.RegistroPatronalID,
                        RegistroPatronal = objFormatoCambio.RegistroPatronal,
                        Clave_Jefe_Inmediato = objFormatoCambio.Clave_Jefe_Inmediato,
                        Nombre_Jefe_Inmediato = objFormatoCambio.Nombre_Jefe_Inmediato,
                        Salario_Base = objFormatoCambio.Salario_Base,
                        Complemento = objFormatoCambio.Complemento,
                        folio = objFormatoCambio.folio,
                        InicioNomina = objFormatoCambio.InicioNomina,
                        FechaInicioCambio = objFormatoCambio.FechaInicioCambio,
                        Justificacion = objFormatoCambio.Justificacion,
                        CamposCambiados = objFormatoCambio.CamposCambiados,
                        Aprobado = objFormatoCambio.Aprobado,
                        Rechazado = objFormatoCambio.Rechazado,
                        usuarioCap = objFormatoCambio.usuarioCap,
                        nomUsuarioCap = objFormatoCambio.nomUsuarioCap,
                        editable = objFormatoCambio.editable,
                        Bono = objFormatoCambio.Bono,
                        fechaCaptura = objFormatoCambio.fechaCaptura,
                        SalarioAnt = objFormatoCambio.SalarioAnt,
                        ComplementoAnt = objFormatoCambio.ComplementoAnt,
                        BonoAnt = objFormatoCambio.BonoAnt,
                        CCAntID = objFormatoCambio.CCAntID,
                        CCAnt = objFormatoCambio.CCAnt,
                        PuestoAnt = objFormatoCambio.PuestoAnt,
                        RegistroPatronalAnt = objFormatoCambio.RegistroPatronalAnt,
                        Nombre_Jefe_InmediatoAnt = objFormatoCambio.Nombre_Jefe_InmediatoAnt,
                        TipoNominaAnt = objFormatoCambio.TipoNominaAnt,
                        Departamento = objFormatoCambio.Departamento,
                        ClaveDepartamento = objFormatoCambio.ClaveDepartamento,
                        DepartamentoAnterior = objFormatoCambio.DepartamentoAnterior,
                        ClaveDepartamentoAnterior = objFormatoCambio.ClaveDepartamentoAnterior,
                        esAuditor = esAuditor,
                        descCategoria = objFormatoCambio.descCategoria ?? "S/N"
                    };
	                #endregion

                    var listAp = authFormatoCambioFS.getAutorizacion(formatoCambio.id);

                    var listApIds = listAp.Select(e => e.Clave_Aprobador).ToList();

                    if (listApIds.Contains(vSesiones.sesionUsuarioDTO.id))
                    {
                        formatoCambio.esAuditor = false;
                    }

                    EsMismoUsuarioCC esMismoUsuarioCC = authFormatoCambioFS.EsUsuarioMismoCC;

                    if ((formatoCambio.usuarioCap == usuarioLog) || (isVerTodo) || rh || esMismoUsuarioCC(formatoCambio.usuarioCap))
                    {
                        formatoCambio.editable = true;
                        var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodo");
                        if (isEditarTodo)
                        {
                            formatoCambio.editable = true;
                        }
                        else
                        {
                            foreach (var objAp in listAp)
                            {
                                if (objAp.Estatus || formatoCambio.Rechazado)
                                {
                                    formatoCambio.editable = false;
                                    break;
                                }
                                else if (rhEditar)
                                {
                                    formatoCambio.editable = true;
                                    break;
                                }
                            }
                        }

                        if (formatoCambio.esAuditor && vSesiones.sesionUsuarioDTO.id != 1080)
                        {
                            if (listApIds.Contains(vSesiones.sesionUsuarioDTO.id))
                            {
                                formatoCambio.editable = true;

                            }
                            else
                            {
                                formatoCambio.editable = false;

                            }
                        }

                        listaMostrar.Add(formatoCambio);
                    }
                    else
                    {
                        var isEditarTodo = ud.getViewAction(vSesiones.sesionCurrentView, "EditarTodo");
                        if (isEditarTodo)
                        {
                            formatoCambio.editable = true;
                            foreach (var objAp in listAp)
                            {
                                if (objAp.Clave_Aprobador == usuarioLog /*&& objAp.Estatus*/)
                                {
                                    listaMostrar.Add(formatoCambio);
                                }
                            }
                        }
                        else
                        {

                            //switch (estado)
                            //{
                            //    case "Todos":
                            //        {
                            //            listaMostrar.Add(formatoCambio);
                            //        }
                            //        break;
                            //    case "Pendiente":
                            //        {
                            //            var objAp = listAp.Where(x => x.Autorizando && x.Clave_Aprobador.Equals(usuarioLog)).FirstOrDefault();
                            //            if (objAp != null)
                            //            {
                            //                var claseBandera = string.Empty;
                            //                setEstado(objAp, out claseBandera);
                            //                if (claseBandera.Equals(EnumExtensions.GetDescription(authEstadoEnum.EnTurno)))
                            //                {
                            //                    listaMostrar.Add(formatoCambio);
                            //                }
                            //            }
                            //        }
                            //        break;
                            //    case "Aprobado":
                            //        {
                            //            var objAp = listAp.Where(x => x.Clave_Aprobador.Equals(usuarioLog)).OrderByDescending(w => w.Orden).FirstOrDefault();

                            //            var claseBandera = string.Empty;
                            //            setEstado(objAp, out claseBandera);
                            //            if (claseBandera.Equals(EnumExtensions.GetDescription(authEstadoEnum.Autorizado)))
                            //            {
                            //                listaMostrar.Add(formatoCambio);
                            //            }
                            //        }
                            //        break;
                            //    case "Rechazado":
                            //        {
                            //            var objAp = listAp.Where(x => x.Clave_Aprobador.Equals(usuarioLog)).OrderByDescending(w => w.Orden).FirstOrDefault();

                            //            var claseBandera = string.Empty;
                            //            setEstado(objAp, out claseBandera);
                            //            if (claseBandera.Equals(EnumExtensions.GetDescription(authEstadoEnum.Rechazado)))
                            //            {
                            //                listaMostrar.Add(formatoCambio);
                            //            }
                            //        }
                            //        break;
                            //}
                        }
                    }

                }
                result.Add("lst", listaMostrar);
                result.Add(SUCCESS, listaMostrar.Count > 0);
            }
            catch (Exception o_O)
            {
                result.Add("message", o_O.Message);
                result.Add(SUCCESS, false);
            }
            var json = Json(result, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public ActionResult getLstAuth(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var clase = string.Empty;
                var lstObjEmpleadoCambio = capturaFormatoCambioFS.getListFormatosCambioPendientes(id, "", 0, 1, "", 0);
                var listAp = authFormatoCambioFS.getAutorizacion(id).Select(auth => new authDTO()
                {
                    idRegistro = auth.id,
                    idAuth = auth.Clave_Aprobador,
                    idPadre = auth.Id_FormatoCambio,
                    orden = auth.Orden,
                    nombre = auth.Nombre_Aprobador,
                    firma = auth.Firma ?? string.Empty,
                    descripcion = string.Format("{0}. {1}", auth.Responsable, auth.PuestoAprobador) ?? string.Empty,
                    comentario = auth.comentario ?? string.Empty,
                    authEstado = setEstado(auth, out clase),
                    clase = clase,
                }).ToList();
                var userCapName = string.Format("Capturó: {0}", lstObjEmpleadoCambio.FirstOrDefault().nomUsuarioCap).ToUpper();
                result.Add(AUTORIZANTES, listAp);
                result.Add(MESSAGE, userCapName);
                result.Add(SUCCESS, listAp.Count > 0);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAutorizacion(int id)
        {
            var result = new Dictionary<string, object>();
            var lstObjEmpleadoCambio = capturaFormatoCambioFS.getListFormatosCambioPendientes(id, "", 0, 1, "", 0);
            var userCapName = lstObjEmpleadoCambio.FirstOrDefault().nomUsuarioCap;
            var listAp = authFormatoCambioFS.getAutorizacion(id);
            int usuarioLog = base.getUsuario().id;

            if (lstObjEmpleadoCambio.FirstOrDefault() != null)
            {
                if (lstObjEmpleadoCambio.FirstOrDefault().Aprobado)
                {
                    try
                    {
                        var objAlertaDis = new tblP_Alerta();
                        var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                        var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == base.getUsuario().id && x.objID.Equals(id));
                        alertasFS.updateAlerta(AletaUpdate);
                    }
                    catch (Exception o_O) { }
                }
                else
                {
                    if (lstObjEmpleadoCambio.FirstOrDefault().Rechazado)
                    {
                        try
                        {
                            var objAlertaDis = new tblP_Alerta();
                            var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                            var AletaUpdate = AletaRaw.FirstOrDefault(x => x.userRecibeID == base.getUsuario().id && x.objID.Equals(id));
                            alertasFS.updateAlerta(AletaUpdate);
                        }
                        catch (Exception o_O) { }
                    }

                }
            }

            foreach (var Ap in listAp)
            {

                if (Ap.Clave_Aprobador != usuarioLog && !Ap.Estatus && Ap.Autorizando)
                    Ap.Autorizando = false;
            }
            try
            {
                result.Add("CAPTURA", userCapName);
                result.Add(ITEMS, listAp);
                result.Add(SUCCESS, true);

                if (listAp.Any(x => x.comentario != null))
                {
                    string comentario = listAp.FirstOrDefault(x => x.comentario != null).comentario;
                    comentario = HttpUtility.HtmlEncode(comentario).Replace("\n", " ").Trim();
                    result.Add("comentario", comentario);
                }

            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        authEstadoEnum setEstado(tblRH_AutorizacionFormatoCambio auth, out string clase)
        {
            var estado = authEstadoEnum.EnEspera;
            if (auth.Estatus)
            {
                estado = authEstadoEnum.Autorizado;
            }
            else if (auth.Rechazado)
            {
                estado = authEstadoEnum.Rechazado;
            }
            else if (auth.Autorizando && auth.Clave_Aprobador.Equals(vSesiones.sesionUsuarioDTO.id))
            {
                estado = authEstadoEnum.EnTurno;
            }
            clase = ((authEstadoEnum)estado).GetDescription();
            return estado;
        }

        public ActionResult Aprobar(authDTO auth)
        {
            var result = new Dictionary<string, object>();

            if (vSesiones.sesionUsuarioDTO.id == 3367)
            {
                var listaArchivosCapacitacion = capacitacionService.getArchivosFormatoCambio(auth.idPadre);

                //Se verifica que se hayan capturado los cuatro archivos en el módulo de capacitación.
                if (listaArchivosCapacitacion.Count() < 2)
                {
                    result.Add(SUCCESS, false);
                    result.Add(MESSAGE, "No se han cargado todos los archivos de soporte en el módulo de Capacitación Operativa.");
                    result.Add("data", new { message = "No se han cargado todos los archivos de soporte en el módulo de Capacitación Operativa." });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            var listAp = authFormatoCambioFS.getAutorizacion(auth.idPadre);
            var objAutorizacion = listAp.FirstOrDefault(a => a.id.Equals(auth.idRegistro));
            int idAutorizado = objAutorizacion.id;
            objAutorizacion.Estatus = true;
            objAutorizacion.Firma = "--" + objAutorizacion.Id_FormatoCambio.ToString() + "|" + DateTime.Now.ToString("ddMMyyyy|HHmm") + "|" + (int)DocumentosEnum.Formato_Cambio + "|" + objAutorizacion.Clave_Aprobador + "--";
            objAutorizacion.Autorizando = false;
            authFormatoCambioFS.SaveChangesAutorizacionCambios(objAutorizacion);

            if (objAutorizacion.tipoAutoriza)
            {
                var autorizaTipo = listAp.Where(x => x.Orden.Equals(objAutorizacion.Orden));
                foreach (var item in autorizaTipo)
                {
                    if (item.Firma == "S/F")
                        item.Firma = objAutorizacion.Firma;
                    item.Autorizando = false;
                    item.Estatus = true;
                    authFormatoCambioFS.SaveChangesAutorizacionCambios(item);
                }
            }

            bool todasAp = true;
            var lstEnEspera = listAp.Where(w => !w.Estatus).ToList();
            var sigAuth = lstEnEspera.Count > 0 ? listAp.FirstOrDefault(a => a.Orden.Equals(lstEnEspera.Min(m => m.Orden))) : null;
            if (sigAuth != null)
            {
                todasAp = false;
                sigAuth.Autorizando = true;
                authFormatoCambioFS.SaveChangesAutorizacionCambios(sigAuth);
            }

            var lstObjEmpleadoCambio = capturaFormatoCambioFS.getListFormatosCambioPendientes(objAutorizacion.Id_FormatoCambio, "", 0, 1, "", 0);
            string folio = "";
            try
            {
                folio = lstObjEmpleadoCambio.Select(x => x.folio).First();
            }
            catch (Exception) { }
            if (todasAp)
            {
                objAutorizacion.Autorizando = true;
                if (lstObjEmpleadoCambio.Count > 0)
                {
                    foreach (var objEmp in lstObjEmpleadoCambio)
                    {
                        objEmp.Aprobado = true;
                        capturaFormatoCambioFS.SaveChangesEmpleado(objEmp);
                    }
                    result.Add(ITEMS, lstObjEmpleadoCambio.FirstOrDefault().id);
                }
                else
                {
                    var formatoCambio = capturaFormatoCambioFS.getFormatoByID(objAutorizacion.Id_FormatoCambio);
                    formatoCambio.Aprobado = true;
                    capturaFormatoCambioFS.SaveChangesEmpleado(formatoCambio);
                    result.Add(ITEMS, formatoCambio.id);
                }

                if (objAutorizacion != null)
                {
                    try
                    {
                        var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                        var AletaUpdate = AletaRaw.Where(x => x.userRecibeID == base.getUsuario().id && x.objID.Equals(idAutorizado)).ToList();
                        AletaUpdate.ForEach(alerta => alertasFS.updateAlerta(alerta));
                    }
                    catch (Exception) { }
                }
               // var diana = usuarioFS.ListUsersById(1019).FirstOrDefault();
              //  var aranza = usuarioFS.ListUsersById(79552).FirstOrDefault();
    
                result.Add(SUCCESS, true);
                result.Add("AprobadoTotal", true);
                result.Add("Folio", folio);
                result.Add("CorreoEnviar", new List<string>() { 
                  /*  diana.correo,
                    aranza.correo*/
                });
            }
            else
            {
                if (sigAuth != null)
                {
                    #region Ajusta alertas
                    try
                    {
                        var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                        var AletaUpdate = AletaRaw.Where(x => x.userRecibeID == base.getUsuario().id && x.objID.Equals(idAutorizado)).ToList();
                        AletaUpdate.ForEach(alerta => alertasFS.updateAlerta(alerta));
                        var objAlerta = new tblP_Alerta()
                        {
                            msj = "Firma-Formato de Cambio " + folio,
                            sistemaID = (int)SistemasEnum.RH,
                            documentoID = (int)ReportesEnum.Formato_Cambio,
                            moduloID = (int)BitacoraEnum.FORMATOCAMBIORH,
                            tipoAlerta = (int)AlertasEnum.REDIRECCION,
                            url = "/Administrativo/FormatoCambio/Index?obj=" + sigAuth.id + "",
                            objID = sigAuth.id,
                            userEnviaID = base.getUsuario().id,
                            userRecibeID = sigAuth.Clave_Aprobador,
                            visto = false
                        };
                        alertasFS.saveAlerta(objAlerta);
                    }
                    catch (Exception o_O) { }
                    #endregion
                }
                result.Add("idFormatoCambios", sigAuth.Id_FormatoCambio);
                result.Add("usuarioEnvia", sigAuth.Clave_Aprobador);
                result.Add("AprobadoTotal", false);
                result.Add(SUCCESS, true);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Rechazar(authDTO auth)
        {
            var result = new Dictionary<string, object>();
            try
            {
                if (auth.comentario == null || auth.comentario.Trim().Length < 10)
                {
                    result.Add(MESSAGE, "No se rechazó la solicitud. El comentario viene vacío.");
                    result.Add(SUCCESS, false);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var objAutorizacion = authFormatoCambioFS.getAutorizacion(auth.idPadre).FirstOrDefault(a => a.id.Equals(auth.idRegistro));
                int idAutorizado = objAutorizacion.id;
                objAutorizacion.Estatus = false;
                objAutorizacion.Autorizando = false;
                objAutorizacion.Rechazado = true;
                objAutorizacion.comentario = auth.comentario.Trim();
                authFormatoCambioFS.SaveChangesAutorizacionCambios(objAutorizacion);
                try
                {
                    var lstObjEmpleadoCambio = capturaFormatoCambioFS.getListFormatosCambioPendientes(objAutorizacion.Id_FormatoCambio, "", 0, 1, "", 0);
                    foreach (var objCancelado in lstObjEmpleadoCambio)
                    {
                        objCancelado.Rechazado = true;
                        var cancelado = capturaFormatoCambioFS.SaveChangesEmpleado(objCancelado);
                    }
                    var AletaRaw = alertasFS.getAlertasBySistema((int)SistemasEnum.RH);
                    var AletaUpdate = AletaRaw.Where(x => x.userRecibeID == base.getUsuario().id && x.objID.Equals(idAutorizado)).ToList();
                    AletaUpdate.ForEach(alerta => alertasFS.updateAlerta(alerta));
                }
                catch (Exception) { }
                alertasFS.updateAlertaByModulo(auth.idAuth, (int)BitacoraEnum.FORMATOCAMBIORH);
                result.Add(SUCCESS, true);
                result.Add("usuarioEnvia", objAutorizacion.Clave_Aprobador);
                result.Add("idFormatoCambios", objAutorizacion.Id_FormatoCambio);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                result.Add(SUCCESS, false);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getEmpleadoEditar(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, capturaFormatoCambioFS.getListFormatosCambioPendientes(id, "", 0, 1, "", 0));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult eliminarFormato(int formatoID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                capturaFormatoCambioFS.eliminarFormato(formatoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #region Actions para llenado de catalogos
        public ActionResult getEmpleado(int id, bool editar)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var objEmpleado = capturaFormatoCambioFS.getEmpleadoForId(id, true);
                objEmpleado.Fecha_Alta = DateTime.Parse(objEmpleado.Fecha_Alta.ToString("dd/MM/yyyy"));
                var pendientes = capturaFormatoCambioFS.getListFormatosCambioPendientes(0, "", id, 2, "", 0).ToList().Count;
                result.Add("Pendientes", pendientes > (editar ? 1 : 0) ? true : false);
                result.Add(ITEMS, objEmpleado);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPuestos(string term)
        {
            var items = capturaFormatoCambioFS.getCatPuestos(term);
            var filteredItems = items.Select(x => new { id = x.puesto, label = x.descripcion });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getPuestosToCombo()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, capturaFormatoCambioFS.getCatPuestos(null).Select(x => new { Value = x.puesto, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTipoNomina()
        {
            var result = new Dictionary<string, object>();
            try
            {
                result.Add(ITEMS, capturaFormatoCambioFS.getCatTipoNomina().Select(x => new { Value = x.tipo_nomina, Text = x.descripcion }));
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getEmpleadoExclusion(int empleadoCVE)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var permiso = capturaFormatoCambioFS.getEmpleadoExclusion(empleadoCVE);
                result.Add(SUCCESS, permiso);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult validReporteExclusion(int id)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var permiso = capturaFormatoCambioFS.validReporteExclusion(id);
                result.Add(SUCCESS, permiso);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCC(string term)
        {

            var items = capturaFormatoCambioFS.getCC(term);
            var filteredItems = items.Select(x => new { id = x.cc, label = x.descripcion });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCboCC()
        {

            var result = new Dictionary<string, object>();
            try
            {
                var items = capturaFormatoCambioFS.getCCList();
                var cbo = items.Select(x => new { Value = x.cc, Text = string.Format("{0} - {1} ", x.cc, x.descripcion) }).OrderBy(o => o.Value).ToList();
                result.Add(SUCCESS, cbo.Count > 0);
                result.Add(ITEMS, cbo);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getDepartamentosCC(string cc)
        {
            return Json(capturaFormatoCambioFS.getDepartamentosCC(cc));
        }

        public ActionResult getEmpleados(string term)
        {
            var items = capturaFormatoCambioFS.getCatEmpleados(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getEmpleadosGeneral(string term)
        {
            var items = capturaFormatoCambioFS.getCatEmpleados(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCatEmpleadosReclutamientos(string term)
        {
            var items = capturaFormatoCambioFS.getCatEmpleadosReclutamientos(term);
            var filteredItems = items.Select(x => new { id = x.clave_empleado, label = x.Nombre });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getUsuarioSelect(string term)
        {
            var items = usuarioFS.ListUsersByName(term);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + ' ' + x.apellidoPaterno + ' ' + x.apellidoMaterno });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuarioSelectConCorreo(string term)
        {
            var items = usuarioFS.ListUsersByName(term);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + ' ' + x.apellidoPaterno + ' ' + x.apellidoMaterno + " (" + x.correo + ")" });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getUsuarioSelectWithException(string term)
        {
            var items = usuarioFS.ListUsersByNameWithException(term);
            var filteredItems = items.Select(x => new { id = x.id, label = x.nombre + ' ' + x.apellidoPaterno + ' ' + x.apellidoMaterno, puesto = x.puesto.descripcion });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRegPatronal(string term)
        {
            var items = capturaFormatoCambioFS.getCatRegistroPatronales(term);
            var filteredItems = items.Select(x => new { id = x.clave_reg_pat, label = x.desc_reg_pat });
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult getFormatoIDbyNotification(int autorizaID)
        {
            var result = new Dictionary<string, object>();
            try
            {
                int formatoID = authFormatoCambioFS.getFormatoIDByAutorizacion(autorizaID);
                result.Add("formatoID", formatoID);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPeriodoActual(int tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var obj = bonoFS.getPeriodoActual(tipoNomina);
                var periodo = new
                {
                    periodo = obj.FirstOrDefault().periodo,
                    inicio = obj.FirstOrDefault().fecha_inicial.ToShortDateString(),
                    fin = obj.FirstOrDefault().fecha_final.ToShortDateString()
                };
                result.Add("periodo", periodo);
                result.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPeriodo(int anio, int periodo, int tipoNomina)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var lst = periodo == 0 ? bonoFS.getPeriodoActual() : bonoFS.getPeriodo(anio, periodo).Where(x => x.tipo_nomina == tipoNomina).ToList();
                var esSuccess = lst.Count > 0;
                if (esSuccess)
                {
                    var data = new
                    {
                        periodo = lst.FirstOrDefault().periodo,
                        inicio = lst.FirstOrDefault().fecha_inicial.ToShortDateString(),
                        fin = lst.FirstOrDefault().fecha_final.ToShortDateString()
                    };
                    result.Add("periodo", data);
                    //result.Add("periodo", lst);
                }
                result.Add(SUCCESS, esSuccess);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetResponsableCC(string cc)
        {
            return Json(capturaFormatoCambioFS.GetResponsableCC(cc), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetRegistroPatCC(string cc)
        {
            return Json(capturaFormatoCambioFS.GetRegistroPatCC(cc), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabuladorByEmpleado(int puesto, int lineaNegocios, int categoria)
        {
            return Json(capturaFormatoCambioFS.GetTabuladorByEmpleado(puesto, lineaNegocios, categoria), JsonRequestBehavior.AllowGet);
        }

        public void CancelarFormatoCambioPorTiempo()
        {

#if DEBUG
            RutaServidor = @"C:\dev\sisi_ext\DOCUMENTOS\CAPITALHUMANO\REPORTESCR";
#else
            RutaServidor = @"C:\dev\sisi_ext\DOCUMENTOS\CAPITALHUMANO\REPORTESCR";
#endif

            Dictionary<string, object> resultado = new Dictionary<string, object>();
            try
            {
                var dictFormatosCancelar = authFormatoCambioFS.CancelarFormatoCambioPorTiempo();
                var formatosCambioPorCancelar = dictFormatosCancelar["items"] as List<CambiosPorCancelarDTO>;

                if (formatosCambioPorCancelar != null)
                {
                    
                    foreach (var item in formatosCambioPorCancelar)
                    {
                        //var lstFirmantes = _context.tblRH_AutorizacionFormatoCambio.Where(e => e.Id_FormatoCambio == item.id).ToList();
                    

                        //foreach (var auth in lstFirmantes)
                        //{
                        //    var objUsuario = _context.tblP_Usuario.FirstOrDefault(e => e.id == auth.Clave_Aprobador);

                        //    if (objUsuario != null)
                        //    {
                        //        lstCorreosNotificar.Add(objUsuario.correo);
                        //    }
                        //}

#if DEBUG
                  
#endif

                        DataSet datos = new DataSet();

                        //LoadDatos(ref datos);

                        ReportDocument rptCV = new rptFormatoCambio();

                        //rptCV.Load(Server.MapPath("~/Reports/rptFormatoCambio.rpt"));
                        //string path = Path.Combine(RutaServidor, "rptFormatoCambio.rpt");
                        //rptCV.Load(path);

                        #region DATASOURCE
                        
                        var dto = new List<tblRH_AutorizacionFormatoCambio>();
                        var objEmploados = capturaFormatoCambioFS.getListFormatosCambioPendientes(item.id, "", 0, 1, "", 0);
                        var objEmploadosEnkontrol = new tblRH_FormatoCambio()
                        {
                            Ape_Materno = objEmploados.FirstOrDefault().Ape_Materno,
                            Ape_Paterno = objEmploados.FirstOrDefault().Ape_Paterno,
                            Aprobado = objEmploados.FirstOrDefault().Aprobado,
                            Bono = objEmploados.FirstOrDefault().Bono,
                            BonoAnt = objEmploados.FirstOrDefault().BonoAnt,
                            CCAnt = objEmploados.FirstOrDefault().CCAnt,
                            CCAntID = objEmploados.FirstOrDefault().CCAntID,
                            ComplementoAnt = objEmploados.FirstOrDefault().ComplementoAnt,
                            Fecha_Alta = objEmploados.FirstOrDefault().Fecha_Alta,
                            PuestoAnt = objEmploados.FirstOrDefault().PuestoAnt,
                            SalarioAnt = objEmploados.FirstOrDefault().SalarioAnt,
                            CamposCambiados = objEmploados.FirstOrDefault().CamposCambiados,
                            CcID = objEmploados.FirstOrDefault().CcID,
                            Clave_Empleado = objEmploados.FirstOrDefault().Clave_Empleado,
                            Clave_Jefe_Inmediato = objEmploados.FirstOrDefault().Clave_Jefe_Inmediato,
                            Complemento = objEmploados.FirstOrDefault().Complemento,
                            editable = objEmploados.FirstOrDefault().editable,
                            fechaCaptura = objEmploados.FirstOrDefault().fechaCaptura,
                            FechaInicioCambio = objEmploados.FirstOrDefault().FechaInicioCambio,
                            folio = objEmploados.FirstOrDefault().folio,
                            id = objEmploados.FirstOrDefault().id,
                            InicioNomina = objEmploados.FirstOrDefault().InicioNomina,
                            Justificacion = objEmploados.FirstOrDefault().Justificacion,
                            Nombre = objEmploados.FirstOrDefault().Nombre,
                            Nombre_Jefe_Inmediato = objEmploados.FirstOrDefault().Nombre_Jefe_InmediatoAnt,
                            nomUsuarioCap = objEmploados.FirstOrDefault().nomUsuarioCap,
                            PuestoID = objEmploados.FirstOrDefault().PuestoID,
                            Rechazado = objEmploados.FirstOrDefault().Rechazado,
                            RegistroPatronal = objEmploados.FirstOrDefault().RegistroPatronalAnt,
                            RegistroPatronalID = objEmploados.FirstOrDefault().RegistroPatronalID,
                            Salario_Base = objEmploados.FirstOrDefault().Salario_Base,
                            TipoNomina = objEmploados.FirstOrDefault().TipoNominaAnt,
                            TipoNominaID = objEmploados.FirstOrDefault().TipoNominaID,
                            usuarioCap = objEmploados.FirstOrDefault().usuarioCap,
                            Puesto = objEmploados.FirstOrDefault().PuestoAnt,
                            CC = objEmploados.FirstOrDefault().CCAntID + "-" + objEmploados.FirstOrDefault().CCAnt,
                            TipoNominaAnt = objEmploados.FirstOrDefault().TipoNominaAnt,
                            Departamento = objEmploados.FirstOrDefault().Departamento,
                            DepartamentoAnterior = objEmploados.FirstOrDefault().DepartamentoAnterior,
                            descCategoria = objEmploados.FirstOrDefault().descCategoria,
                            descLineaNegocios = objEmploados.FirstOrDefault().descLineaNegocios,
                            descCategoriaAnterior = objEmploados.FirstOrDefault().descCategoriaAnterior,
                            descLineaNegociosAnterior = objEmploados.FirstOrDefault().descLineaNegociosAnterior,
                        };
                        string TipoNomina = "";
                        string SalarioMesEmpleadoCambio = "";
                        string ComplementoMesEmpleadoCambio = "";
                        string BonoMesEmpleadoCambio = "";

                        foreach (var objEmp in objEmploados)
                        {
                            objEmp.CC = objEmp.CcID + " - " + objEmp.CC;
                            if (objEmp.TipoNominaID == (int)Tipo_NominaEnum.SEMANAL)
                            {
                                TipoNomina = "Semana";

                                SalarioMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Salario_Base) / 7) * 30.4), 2).ToString();
                                ComplementoMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Complemento) / 7) * 30.4), 2).ToString();
                                BonoMesEmpleadoCambio = Math.Round(((Convert.ToDouble(objEmp.Bono) / 7) * 30.4), 2).ToString();
                            }
                            else
                            {
                                TipoNomina = "Quincena";

                                SalarioMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Salario_Base) * 2), 2).ToString();
                                ComplementoMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Complemento) * 2), 2).ToString();
                                BonoMesEmpleadoCambio = Math.Round((Convert.ToDouble(objEmp.Bono) * 2), 2).ToString();
                            }

                            //var objAutorizacion = capturaAutorizacionFormatoCambioService.getAutorizacionFormatoCambioService().getAutorizacion(objEmp.id);
                            var objAutorizacion = authFormatoCambioFS.getAutorizacion(objEmp.id);
                            int formatoCambio = 0;
                            if (objAutorizacion.Count > 0)
                                formatoCambio = objAutorizacion.FirstOrDefault().Id_FormatoCambio;
                            var dto2 = new List<tblRH_AutorizacionFormatoCambio>();
                            for (int i = 1; i <= 10; i++)
                            {
                                var d = new tblRH_AutorizacionFormatoCambio();
                                d.Orden = i;
                                switch (i)
                                {
                                    case 1:
                                        d.Responsable = "Gerente Actual";
                                        d.PuestoAprobador = "Responsable del Centro de Costos Actual";
                                        d.Orden = 1;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;

                                        break;
                                    case 2:
                                        d.Responsable = "Gerente Recibe";
                                        d.PuestoAprobador = "Responsable del Centro de Costos Recibe";
                                        d.Orden = 2;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;

                                        break;
                                    case 3:
                                        d.Responsable = "VoBo";
                                        d.PuestoAprobador = "Capital Humano 1";
                                        d.Orden = 3;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 4:
                                        d.Responsable = "VoBo";
                                        d.PuestoAprobador = "Capital Humano 2";
                                        d.Orden = 4;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 5:
                                        d.Responsable = "Autorización 1";
                                        d.PuestoAprobador = "Gerente/SubDirector/Director de Área 1";
                                        d.Orden = 5;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 6:
                                        d.Responsable = "Autorización 1";
                                        d.PuestoAprobador = "Gerente/SubDirector/Director de Área 2";
                                        d.Orden = 6;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 7:
                                        d.Responsable = "Autorización 2";
                                        d.PuestoAprobador = "Director de Línea de Negocios 1";
                                        d.Orden = 7;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 8:
                                        d.Responsable = "Autorización 2";
                                        d.PuestoAprobador = "Director de Línea de Negocios 2";
                                        d.Orden = 8;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 9:
                                        d.Responsable = "Autorización 3";
                                        d.PuestoAprobador = "Alta Dirección 1";
                                        d.Orden = 9;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    case 10:
                                        d.Responsable = "Autorización 3";
                                        d.PuestoAprobador = "Alta Dirección 2";
                                        d.Orden = 10;
                                        d.Nombre_Aprobador = "";
                                        d.Firma = "SF";
                                        d.Id_FormatoCambio = formatoCambio;
                                        break;
                                    default:
                                        break;
                                }
                                dto2.Add(d);
                            }
                            bool bandera = false;

                            for (int i = 0; i < dto2.ToArray().Length; i++)
                            {
                                bandera = false;
                                foreach (var auth in objAutorizacion)
                                {
                                    if (dto2[i].Responsable == auth.Responsable && dto2[i].PuestoAprobador == auth.PuestoAprobador)
                                    {
                                        auth.PuestoAprobador = auth.PuestoAprobador.TrimEnd('1');
                                        auth.PuestoAprobador = auth.PuestoAprobador.TrimEnd('2');

                                        dto2[i] = auth;
                                        dto.Add(dto2[i]);

                                        bandera = true;
                                    }
                                }

                                if (!bandera)
                                {
                                    dto2[i].PuestoAprobador = dto2[i].PuestoAprobador.TrimEnd('1');
                                    dto2[i].PuestoAprobador = dto2[i].PuestoAprobador.TrimEnd('2');
                                    dto.Add(dto2[i]);
                                }

                            }

                        }
                        string SalarioMesEmpleadoOriginal = "";
                        string ComplementoMesEmpleadoOriginal = "";
                        string BonoMesEmpleadoOriginal = "";
                        if ((objEmploadosEnkontrol.TipoNominaAnt == "QUINCENAL" ? 4 : 1) == (int)Tipo_NominaEnum.SEMANAL)
                        {

                            SalarioMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.SalarioAnt) / 7) * 30.4), 2).ToString();
                            ComplementoMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) / 7) * 30.4), 2).ToString();
                            BonoMesEmpleadoOriginal = Math.Round(((Convert.ToDouble(objEmploadosEnkontrol.BonoAnt) / 7) * 30.4), 2).ToString();

                        }
                        else
                        {
                            BonoMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.BonoAnt) * 2), 2).ToString();
                            SalarioMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.SalarioAnt) * 2), 2).ToString();
                            ComplementoMesEmpleadoOriginal = Math.Round((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) * 2), 2).ToString();
                        }
                            // setParametro("SalarioMesEmpleadoOriginal", );
                            // setParametro("ComplementoMesEmpleadoOriginal", Math.Round((Convert.ToDouble(objEmploadosEnkontrol.ComplementoAnt) * 2), 2));

                        int empresaActual = vSesiones.sesionEmpresaActual;

                        var lstEnkontrol = new List<tblRH_FormatoCambio>() { objEmploadosEnkontrol };
                        rptCV.Database.Tables[0].SetDataSource(authFormatoCambioFS.getInfoEnca("FORMATO DE CAMBIO", "CAPITAL HUMANO"));
                        rptCV.Database.Tables[1].SetDataSource(dto.ToList());
                        rptCV.Database.Tables[2].SetDataSource(ToDataTable(objEmploados));
                        rptCV.Database.Tables[3].SetDataSource(ToDataTable(lstEnkontrol));

                        rptCV.SetParameterValue("TipoNomina", TipoNomina);
                        rptCV.SetParameterValue("SalarioMesEmpleadoCambio", SalarioMesEmpleadoCambio);
                        rptCV.SetParameterValue("ComplementoMesEmpleadoCambio", ComplementoMesEmpleadoCambio);
                        rptCV.SetParameterValue("BonoMesEmpleadoCambio", BonoMesEmpleadoCambio);
                        rptCV.SetParameterValue("SalarioMesEmpleadoOriginal", SalarioMesEmpleadoOriginal);
                        rptCV.SetParameterValue("ComplementoMesEmpleadoOriginal", ComplementoMesEmpleadoOriginal);
                        rptCV.SetParameterValue("BonoMesEmpleadoOriginal", BonoMesEmpleadoOriginal);
                        rptCV.SetParameterValue("NombreCompleto", objEmploadosEnkontrol.Nombre + " " + objEmploadosEnkontrol.Ape_Paterno + " " + objEmploadosEnkontrol.Ape_Materno);
                        rptCV.SetParameterValue("hCC", setTitleCC() + ":");
                        rptCV.SetParameterValue("DepartamentoAnterior", objEmploadosEnkontrol.DepartamentoAnterior ?? " ");
                        rptCV.SetParameterValue("Departamento", objEmploadosEnkontrol.Departamento ?? " ");
                        rptCV.SetParameterValue("descCategoria", objEmploadosEnkontrol.descCategoria ?? " ");
                        rptCV.SetParameterValue("descLineaNegocio", objEmploadosEnkontrol.descLineaNegocios ?? " ");
                        rptCV.SetParameterValue("descCategoriaAnterior", objEmploadosEnkontrol.descCategoriaAnterior ?? " ");
                        rptCV.SetParameterValue("descLineaNegocioAnterior", objEmploadosEnkontrol.descLineaNegociosAnterior ?? " ");
                        rptCV.SetParameterValue("empresaActual", empresaActual);
                        #endregion

                        //Stream stream = rptCV.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                        Stream stream = rptCV.ExportToStream(ExportFormatType.PortableDocFormat);
                        //stream.Seek(0, SeekOrigin.Begin);

                        List<byte[]> downloadPDFs = new List<byte[]>();
                        using (var streamReader = new MemoryStream())
                        {
                            stream.CopyTo(streamReader);
                            downloadPDFs.Add(streamReader.ToArray());

                           /* GlobalUtils.sendEmailAdjuntoInMemory2("FORMATO DE CAMBIO RECHAZADO AUTOMATICAMENTE POR SISTEMA", "Buen día,<br><br>Se ha rechazado un formato de cambio por sistema, el formato de cambio del cc: [" + item.CcID + "] " + item.CC + " folio: " + item.id + " Fecha de captura: " + item.fechaCaptura.Value.ToString("dd/MM/yyyy") + " excedio los dias en proceso de autorizacion (30 dias).<br><br>" +
                          
                            "Se informa que este es un correo autogenerado por el sistema SISI.<br>") ;*/
                         

                        }

                        //GlobalUtils.sendEmail("FORMATO DE CAMBIO RECHAZADO AUTOMATICAMENTE POR SISTEMA", "Buen día,<br><br>Se ha rechazado un formato de cambio por sistema, el formato de cambio del cc: [" + item.CcID + "] " + item.CC + " folio: " + item.id + " Fecha de captura: " + item.fechaCaptura.Value.ToString("dd/MM/yyyy") + " excedio los dias en proceso de autorizacion (30 dias).<br><br>" +
                        //        "Favor de ingresar a <b>sigoplan.construplan.com.mx</b> al menú:<br>" +
                        //        "Construplan > Capital Humano > Formato Cambio > Gestión.<br><br>" +
                        //        "Se informa que este es un correo autogenerado por el sistema SIGOPLAN.<br>" +
                        //        "(http://sigoplan.construplan.com.mx). No es necesario dar una respuesta. Gracias.", lstCorreosNotificar);

                        
                    }
                }
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        protected string setTitleCC()
        {
            return vSesiones.sesionEmpresaActual == 1 ? "Centro Costos" : "Area Cuenta";
        }

        public ActionResult cboCentroCostos()
        {

            var result = new Dictionary<string, object>();
            try
            {
                var cbo = capturaFormatoCambioFS.cboCentroCostos();
                result.Add(SUCCESS, cbo.Count > 0);
                result.Add(ITEMS, cbo);
            }
            catch (Exception e)
            {
                result.Add(MESSAGE, e.Message);
                result.Add(SUCCESS, false);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}