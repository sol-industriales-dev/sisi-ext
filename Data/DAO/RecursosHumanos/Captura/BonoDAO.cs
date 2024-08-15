using Core.DAO.RecursosHumanos.Captura;
using Core.Entity.RecursosHumanos.Captura;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Core.DTO.Principal.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.RecursosHumanos.Catalogo;
using Core.DTO.RecursosHumanos;
using Core.DTO;
using Core.Entity.Principal.Usuarios;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Alertas;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using System.Data.Entity.Migrations;
using System.Globalization;
using Core.Enum.Principal.Bitacoras;
using Core.Enum.Principal;
using Core.Enum.Multiempresa;
using Core.DTO.Utils.Data;
using System.Data.Odbc;
using Core.DTO.Contabilidad.Propuesta.Nomina;
using Core.Enum.RecursosHumanos;
using System.Reflection;
using Core.DTO.Utils;
using Core.DTO.RecursosHumanos.ListaNegraBlanca;
using Core.Enum;
using Data.Factory.Principal.Usuarios;
using System.Web;
using Core.Entity.Administrativo.Contabilidad.Nomina;
using Newtonsoft.Json;
using System.Globalization;
using Core.Entity.Principal.Multiempresa;
using System.Text.RegularExpressions;
using Core.Entity.RecursosHumanos.Bajas;
using Core.Entity.RecursosHumanos.Vacaciones;
using Core.DTO.RecursosHumanos.Vacaciones;
using Core.Entity.Administrativo.RecursosHumanos.Enkontrol;
using System.IO;

namespace Data.DAO.RecursosHumanos.Captura
{
    public class BonoDAO : GenericDAO<tblRH_BN_Plantilla>, IBonoDAO
    {
        private readonly string _NOMBRE_CONTROLADOR = "BonoController";
        private readonly int _SISTEMA = (int)SistemasEnum.RH;
        private readonly string _RUTA_BASE_EVIDENCIAS = @"\\10.1.0.112\Proyecto\SIGOPLANPERU\CAPITAL_HUMANO\INCIDENCIAS\";
        private readonly string _RUTA_LOCAL_EVIDENCIAS = @"C:\Proyecto\SIGOPLANPERU\CAPITAL_HUMANO\INCIDENCIAS\";
        private readonly string _RUTA_EVIDENCIAS = string.Empty;

        public BonoDAO()
        {
#if DEBUG
            _RUTA_EVIDENCIAS += string.Format(@"{0}\{1}\", _RUTA_LOCAL_EVIDENCIAS, DateTime.Now.Year);
#else
            _RUTA_EVIDENCIAS += string.Format(@"{0}\{1}\", _RUTA_BASE_EVIDENCIAS, DateTime.Now.Year);
#endif
        }

        enum tipoListaEnum
        {
            blanca = 1,
            negra = 2
        }

        #region guardar

        public bool CrearPlantilla(tblRH_BN_Plantilla plan, bool isGestion)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!isGestion)
                    {
                        plan.version = ObtenerNumeroVersionPlantilla(plan.cc);
                        _context.tblRH_BN_Plantilla.Add(plan);
                        _context.SaveChanges();
                    }
                    else {
                        var data = _context.tblRH_BN_Plantilla.Where(x => x.cc.Equals(plan.cc) && x.estatus == 0 && x.versionActiva == false).OrderByDescending(x => x.version).FirstOrDefault();
                        data.fechaInicio = plan.fechaInicio;
                        data.fechaFin = plan.fechaFin;

                        var dataDet = _context.tblRH_BN_Plantilla_Det.Where(x => x.plantillaID == data.id);
                        _context.tblRH_BN_Plantilla_Det.RemoveRange(dataDet);
                        var dataAuth = _context.tblRH_BN_Plantilla_Aut.Where(x => x.plantillaID == data.id);
                        _context.tblRH_BN_Plantilla_Aut.RemoveRange(dataAuth);
                        _context.SaveChanges();
                        foreach (var i in plan.listDetalle)
                        {
                            i.plantillaID = data.id;
                        }
                        foreach (var i in plan.listAutorizadores)
                        {
                            i.plantillaID = data.id;
                        }
                        _context.tblRH_BN_Plantilla_Det.AddRange(plan.listDetalle);
                        _context.tblRH_BN_Plantilla_Aut.AddRange(plan.listAutorizadores);
                        _context.SaveChanges();

                    }

                    //Enviarcorreo

                    dbTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, nombreFuncion, e, AccionEnum.AGREGAR, 0, plan);
                    return false;
                }
            }
        }
        public bool CrearPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan, bool isGestion)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!isGestion)
                    {
                        plan.version = ObtenerNumeroVersionPlantilla(plan.cc);
                        plan.versionActiva = true;
                        _context.tblRH_BN_Plantilla_Cuadrado.Add(plan);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var data = _context.tblRH_BN_Plantilla_Cuadrado.Where(x => x.cc.Equals(plan.cc) && x.estatus == 0 && x.versionActiva == false).OrderByDescending(x => x.version).FirstOrDefault();
                        data.fechaInicio = plan.fechaInicio;
                        data.fechaFin = plan.fechaFin;

                        var dataDet = _context.tblRH_BN_Plantilla_Cuadrado_Det.Where(x => x.plantillaID == data.id);
                        _context.tblRH_BN_Plantilla_Cuadrado_Det.RemoveRange(dataDet);
                        _context.SaveChanges();
                        foreach (var i in plan.listDetalle)
                        {
                            i.plantillaID = data.id;
                        }
                        _context.tblRH_BN_Plantilla_Cuadrado_Det.AddRange(plan.listDetalle);
                        _context.SaveChanges();

                    }

                    //Enviarcorreo

                    dbTransaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, nombreFuncion, e, AccionEnum.AGREGAR, 0, plan);
                    return false;
                }
            }
        }

        private bool EnviarCorreo(tblRH_BN_Plantilla plantilla, List<tblRH_BN_Plantilla_Aut> autorizadores)
        {
            var correo = new Infrastructure.DTO.CorreoDTO();
            var folio = plantilla.id.ToString().PadLeft(6, '0');

            switch (plantilla.estatus)
            {
                case (int)authEstadoEnum.EnEspera:
                    correo.asunto = @"Se informa que se registró una nueva plantilla de persona - bono desempeño mensual
                                      con Folio: &#8220;" + folio + "&#8221; para el CC " + plantilla.cc + 
                                     "por el usuario (" + plantilla.usuarioCapturo.nombre + " " + plantilla.usuarioCapturo.apellidoPaterno + " " + plantilla.usuarioCapturo.apellidoMaterno;
                    correo.cuerpo = @"<html>
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
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Se informa que se registró una nueva plantilla de persona - bono desempeño mensual con Folio: &#8220;" +
                                                        folio + @"&#8221 para el CC " + plantilla.cc + " por el usuario (" +
                                                        plantilla.usuarioCapturo.nombre + " " + plantilla.usuarioCapturo.apellidoPaterno + " " + plantilla.usuarioCapturo.apellidoMaterno + @").<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
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
                    foreach (var autorizador in autorizadores)
                    {
                        correo.asunto += @"<tr>" +
                                            "<td>" +
                                                autorizador.aprobadorNombre +
                                            "</td>" +
                                            "<td>" +
                                                autorizador.aprobadorPuesto +
                                            "</td>" +
                                            getEstatus(autorizador.estatus, autorizador.autorizando) +
                                          "</tr>";
                    }
                    break;
            }

            correo.cuerpo += @"</tbody>" +
                             @"</table>
                                        <p class=MsoNormal>
                                            <o:p>&nbsp;</o:p>
                                        </p>
                                        <p class=MsoNormal>
                                            Favor de ingresar al sistema SISI, en el apartado de CH  en la opción Plantilla de Personal<o:p></o:p>
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


            return false;
        }

        private string getEstatus(int est, bool aut)
        {
            if ((int)EstatusRegEnum.PENDIENTE == (est) && aut)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if ((int)EstatusRegEnum.AUTORIZADO == (est))
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else
                if ((int)EstatusRegEnum.RECHAZADO == (est))
                    return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
                else
                    return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }

        public bool ActualizarPlantilla(tblRH_BN_Plantilla plan)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // El proceso de autorización terminó, y se convierte en la nueva versión activa.
                    if (plan.estatus == (int)authEstadoEnum.Autorizado)
                    {
                        var plantillaAnterior = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc == plan.cc && x.versionActiva);

                        if (plantillaAnterior != null)
                        {
                            plantillaAnterior.versionActiva = false;
                        }

                        plan.versionActiva = true;
                    }

                    // La plantilla es rechazada, y se descarta como nueva versión.
                    else if (plan.estatus == (int)authEstadoEnum.Rechazado)
                    {
                        plan.version = 0;
                    }

                    _context.SaveChanges();
                    dbTransaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, nombreFuncion, e, AccionEnum.ACTUALIZAR, 0, plan);
                    return false;
                }
            }
        }
        public bool ActualizarPlantilla_Cuadrado(tblRH_BN_Plantilla_Cuadrado plan)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // El proceso de autorización terminó, y se convierte en la nueva versión activa.
                    //if (plan.estatus == (int)authEstadoEnum.Autorizado)
                    //{
                    //    var plantillaAnterior = _context.tblRH_BN_Plantilla_Cuadrado.FirstOrDefault(x => x.cc == plan.cc && x.versionActiva);

                    //    if (plantillaAnterior != null)
                    //    {
                    //        plantillaAnterior.versionActiva = false;
                    //    }

                    //    plan.versionActiva = true;
                    //}

                    // La plantilla es rechazada, y se descarta como nueva versión.
                    //else if (plan.estatus == (int)authEstadoEnum.Rechazado)
                    //{
                    //    plan.version = 0;
                    //}
                    var plantillaAnterior = _context.tblRH_BN_Plantilla_Cuadrado.FirstOrDefault(x => x.cc == plan.cc && x.versionActiva);

                    if (plantillaAnterior != null)
                    {
                        plantillaAnterior.versionActiva = false;
                    }

                    plan.versionActiva = true;
                    _context.SaveChanges();
                    dbTransaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, nombreFuncion, e, AccionEnum.ACTUALIZAR, 0, plan);
                    return false;
                }
            }
        }
        public bool ActualizarEvaluacion(tblRH_BN_Evaluacion plan)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // El proceso de autorización terminó, y se convierte en la nueva versión activa.
                    if (plan.estatus == (int)authEstadoEnum.Autorizado)
                    {
                        var plantillaAnterior = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.id == plan.id);


                    }

                    // La plantilla es rechazada, y se descarta como nueva versión.
                    else if (plan.estatus == (int)authEstadoEnum.Rechazado)
                    {
                        
                    }

                    _context.SaveChanges();
                    dbTransaction.Commit();
 
                    return true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    var nombreFuncion = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, nombreFuncion, e, AccionEnum.ACTUALIZAR, 0, plan);
                    return false;
                }
            }
        }
        public bool EnviarCorreoEvaluacion(tblRH_BN_Evaluacion data,bool actualizacion)
        {
            UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
            bool enviado = false;
            try
            {
                var dataBase = data;
                var aut = data.listAutorizadores;
                var usuarioEnvia = vSesiones.sesionUsuarioDTO;
                var downloadPDF = vSesiones.downloadPDF;
                var usuariosAutorizadores = data.listAutorizadores;
                var sig = data.listAutorizadores.FirstOrDefault(x => x.autorizando);
                var _primerAutorizante = dataBase.listAutorizadores.FirstOrDefault(x => x.orden == 1).autorizando;

                var _tipoDoc = "Evaluacion de bonos de desempeño mensual";
                var _nomina = dataBase.listDetalle.First().tipo_Nom;
                var _cc = "(" + dataBase.plantilla.ccNombre + ")";
                var _periodo = dataBase.periodo + " del " + dataBase.fechaInicio.ToString("dd-MM-yyyy") + " al " + dataBase.fechaInicio.ToString("dd-MM-yyyy") + " " + _nomina;
                var _msj = _tipoDoc+@" del cc "+_cc+@" periodo "+_periodo;
                var _motivoRechazo = dataBase.listAutorizadores.FirstOrDefault(x => x.estatus == (int)authEstadoEnum.Rechazado);

                if (!actualizacion)
                {
                    if (sig != null)
                    {
                        var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.BONO_ADMIN && x.objID == dataBase.id).ToList();

                        if (a.Count > 0)
                        {
                            foreach (var i in a)
                            {
                                i.visto = true;
                            }
                            _context.SaveChanges();
                            var b = new tblP_Alerta();
                            b.userEnviaID = 13;
                            b.userRecibeID = sig.aprobadorClave;
                            b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                            b.visto = false;
                            b.url = "/Administrativo/Bono/GestionEvaluaciones/?autID=" + dataBase.id;
                            b.objID = dataBase.id;
                            b.obj = "";
                            b.msj = "Bono Desempeño Mensual (" + data.plantilla.ccNombre + ")";
                            b.documentoID = 0;
                            b.sistemaID = (int)SistemasEnum.RH;
                            b.moduloID = (int)BitacoraEnum.BONO_ADMIN;
                            _context.tblP_Alerta.Add(b);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var b = new tblP_Alerta();
                            b.userEnviaID = 13;
                            b.userRecibeID = sig.aprobadorClave;
                            b.tipoAlerta = (int)AlertasEnum.REDIRECCION;
                            b.visto = false;
                            b.url = "/Administrativo/Bono/GestionEvaluaciones/?autID=" + dataBase.id;
                            b.objID = dataBase.id;
                            b.obj = "";
                            b.msj = "Bono Desempeño Mensual (" + data.plantilla.ccNombre + ")";
                            b.documentoID = 0;
                            b.sistemaID = (int)SistemasEnum.RH;
                            b.moduloID = (int)BitacoraEnum.BONO_ADMIN;
                            _context.tblP_Alerta.Add(b);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        var a = _context.tblP_Alerta.Where(x => x.sistemaID == (int)SistemasEnum.RH && x.moduloID == (int)BitacoraEnum.BONO_ADMIN && x.objID == dataBase.id).ToList();
                        a.ForEach(x => x.visto = true);
                        _context.SaveChanges();
                    }
                }
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
                var folioID = dataBase.id;
                var folio = dataBase.id.ToString().PadLeft(6, '0');

                if (actualizacion)
                {
                    AsuntoCorreo += @"  <p class=MsoNormal>
                                            Se informa realizo una actualización en la " + _tipoDoc + @" del cc " + _cc + @" periodo " + _periodo + @".<o:p></o:p>
                                        </p>";
                }
                else if (dataBase.estatus == (int)authEstadoEnum2.Autorizado)
                {
                    AsuntoCorreo += @"  <p class=MsoNormal>
                                            Se informa que se autorizo correctamente la "+_tipoDoc+@" del cc "+_cc+@" periodo "+_periodo+@".<o:p></o:p>
                                        </p>";
                }
                else if (dataBase.estatus == (int)authEstadoEnum2.Rechazado)
                {
                    AsuntoCorreo += @"  <p class=MsoNormal>
                                            Se informa que se rechazo correctamente la " + _tipoDoc + @" del cc " + _cc + @" periodo " + _periodo + @".<o:p></o:p>
                                        </p>";
                    AsuntoCorreo += @"  <p class=MsoNormal>
                                            <strong>La razón del rechazo fue: </strong> " + HttpUtility.HtmlEncode((_motivoRechazo.comentario ?? "")) + @"<o:p></o:p>
                                        </p>";
                }
                else {
                    if (_primerAutorizante)
                    {
                        AsuntoCorreo += @"  <p class=MsoNormal>
                                                Se informa que se registro una nueva " + _tipoDoc + @" del cc " + _cc + @" periodo " + _periodo + @".<o:p></o:p>
                                            </p>";
                    }
                    else
                    {
                        AsuntoCorreo += @"  <p class=MsoNormal>
                                                Se informa que fue realizada una autorización en la " + _tipoDoc + @" del cc " + _cc + @" periodo " + _periodo + @".<o:p></o:p>
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

                foreach (var i in usuariosAutorizadores)
                {

                    AsuntoCorreo += @"<tr>
                                <td>" + i.aprobadorNombre + "</td>" +
                                    "<td>" + i.aprobadorPuesto + "</td>" +
                                        getEstatusAutorizadores(i.estatus, i.autorizando) +
                                    "</tr>";


                    var usuarioCorreo = usuarioFactoryServices.getUsuarioService().ListUsersById(i.aprobadorClave).FirstOrDefault();

                    CorreoEnviar.Add(usuarioCorreo.correo);
                }

                AsuntoCorreo += @"</tbody>" +
                            @"</table>
                            <p class=MsoNormal>
                                <o:p>&nbsp;</o:p>
                            </p>
                            <p class=MsoNormal>
                                Favor de ingresar al sistema SISI , en el apartado de Capital Humano, menú CH en la opción Incidencias / Bono Desempeño Mensual / Gestion Evaluación<o:p></o:p>
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

#if DEBUG
                CorreoEnviar = new List<string> { "martin.valle@construplan.com.mx", "martin.valle@construplan.com.mx" };
                var tipoFormato = _msj + ".pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Autorizaciones de " + _msj), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                vSesiones.downloadPDF = null;
                enviado = true;
#else
                CorreoEnviar.Add(usuarioEnvia.correo);
                var tipoFormato = _msj + ".pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Autorizaciones de " + _msj), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), downloadPDF, tipoFormato);
                vSesiones.downloadPDF = null;
                enviado = true;
#endif
            }
            catch (Exception e)
            {
                enviado = false;
            }
            return enviado;
        }
        private string getEstatusAutorizadores(int est, bool aut)
        {
            if (aut)
                return "<td style='background-color: yellow;'>AUTORIZANDO</td>";
            else if ((int)authEstadoEnum.Autorizado == (est))
                return "<td style='background-color: #82E0AA;'>AUTORIZADO</td>";
            else if ((int)authEstadoEnum.Rechazado == (est))
                return "<td style='background-color: #EC7063;'>RECHAZADO</td>";
            else
                return "<td style='background-color: #FAE5D3;'>PENDIENTE</td>";
        }
        private int ObtenerNumeroVersionPlantilla(string cc)
        {
            var ultimaVersion = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc == cc && x.versionActiva);

            return ultimaVersion == null ? 1 : ultimaVersion.version + 1;
        }

        public Tuple<bool, string> GuardarIncidencia(IncidenciasPaqueteDTO paquete, List<HttpPostedFileBase> archivos, List<int> lstClaveEmpleados)
        {
            var a = GuardarIncidenciaSIGOPLAN_ENKONTROL(paquete.busq, paquete.incidencia, paquete.incidencia_det, paquete.incidencia_det_Peru, archivos, lstClaveEmpleados);
            return a;

        }
        public bool GuardarIncidenciaSincronizar(IncidenciasPaqueteDTO paquete)
        {
            bool result = true;
            //try{
            var a = GuardarIncidenciaSIGOPLAN_ENKONTROL_SINCRONIZAR(paquete.incidencia, paquete.incidencia_det);
            //}
            //catch(Exception e)
            //{
            //    result =  false;
            //}
            return result;

        }
        
        bool GuardarIncidenciaEk(List<tblRH_BN_Incidencias> lst)
        {
            var lstOdbc = new List<OdbcConsultaDTO>();
            var ObjInc = new IncidenciaEmpDTO(lst.FirstOrDefault());
            var ObjIncEk = objIncidenciaEmp(ObjInc);
            lstOdbc.Add(ObjIncEk.id_incidencia > 0 ? updateIncidenciaEmp(ObjInc) : saveIncidenciaEmp(ObjInc));
            lst.ForEach(empl =>
            {
                var emplDet = new IncidenciasEmpDetDTO(empl);
                var emplDetEK = objIncidenciaEmpDet(emplDet);
                lstOdbc.Add(emplDetEK.id_incidencia > 0 ? updateIncidenciaEmpDet(emplDet) : saveIncidenciaEmpDet(emplDet));
            });
            var res = _contextEnkontrol.Save(EnkontrolAmbienteEnum.Prod, lstOdbc);
            return res.Count > 0 && res.All(a => a > 0);
        }
        IncidenciaEmpDTO objIncidenciaEmp(IncidenciaEmpDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT * FROM sn_incidencias_empl WHERE id_incidencia = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia }
            };
            var res = _contextEnkontrol.Select<IncidenciaEmpDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            return res.FirstOrDefault();
        }
        OdbcConsultaDTO saveIncidenciaEmp(IncidenciaEmpDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"INSERT INTO sn_incidencias_empl
                        (id_incidencia
                        ,anio
                        ,periodo
                        ,cc
                        ,tipo_nomina
                        ,estatus
                        ,empleado_modifica
                        ,fecha_modifica
                        ,usuario_auto
                        ,fecha_auto)
                        VALUES (?,?,?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = obj.anio },
                new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = obj.periodo },
                new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = obj.tipo_nomina },
                new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.VarChar, valor = obj.estatus },
                new OdbcParameterDTO() { nombre = "empleado_modifica", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_modifica", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                new OdbcParameterDTO() { nombre = "usuario_auto", tipo = OdbcType.Int, valor = obj.usuario_auto },
                new OdbcParameterDTO() { nombre = "fecha_auto", tipo = OdbcType.Date, valor = obj.fecha_auto },
            };
            return odbc;
        }
        OdbcConsultaDTO updateIncidenciaEmp(IncidenciaEmpDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl
                        ,anio = ?
                        ,periodo = ?
                        ,cc = ?
                        ,tipo_nomina= ?
                        ,estatus = ?
                        ,empleado_modifica = ?
                        ,fecha_modifica = ?
                        ,usuario_auto = ?
                        ,fecha_auto = ?
                    WHERE id_incidencia = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = obj.anio },
                new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = obj.periodo },
                new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = obj.tipo_nomina },
                new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.VarChar, valor = obj.estatus },
                new OdbcParameterDTO() { nombre = "empleado_modifica", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_modifica", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                new OdbcParameterDTO() { nombre = "usuario_auto", tipo = OdbcType.Int, valor = obj.usuario_auto },
                new OdbcParameterDTO() { nombre = "fecha_auto", tipo = OdbcType.Date, valor = obj.fecha_auto },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
            };
            return odbc;
        }
        IncidenciasEmpDetDTO objIncidenciaEmpDet(IncidenciasEmpDetDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT * FROM sn_incidencias_empl_det WHERE id_incidencia = ? AND clave_empleado = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado }
            };
            var res = _contextEnkontrol.Select<IncidenciasEmpDetDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            return res.FirstOrDefault();
        }
        OdbcConsultaDTO saveIncidenciaEmpDet(IncidenciasEmpDetDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"INSERT INTO sn_incidencias_empl_det
                            (id_incidencia ,clave_empleado
                            ,dia1 ,dia2 ,dia3 ,dia4 ,dia5 ,dia6 ,dia7 ,dia8 ,dia9 ,dia10 ,dia11 ,dia12 ,dia13 ,dia14 ,dia15 ,dia16
                            ,he_dia1 ,he_dia2 ,he_dia3 ,he_dia4 ,he_dia5 ,he_dia6 ,he_dia7 ,he_dia8 ,he_dia9 ,he_dia10 ,he_dia11 ,he_dia12 ,he_dia13 ,he_dia14 ,he_dia15 ,he_dia16
                            ,bono ,observaciones ,archivo_enviado ,dias_extras ,prima_dominical)
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado },
                new OdbcParameterDTO() { nombre = "dia1", tipo = OdbcType.Int, valor = obj.dia1 },
                new OdbcParameterDTO() { nombre = "dia2", tipo = OdbcType.Int, valor = obj.dia2 },
                new OdbcParameterDTO() { nombre = "dia3", tipo = OdbcType.Int, valor = obj.dia3 },
                new OdbcParameterDTO() { nombre = "dia4", tipo = OdbcType.Int, valor = obj.dia4 },
                new OdbcParameterDTO() { nombre = "dia5", tipo = OdbcType.Int, valor = obj.dia5 },
                new OdbcParameterDTO() { nombre = "dia6", tipo = OdbcType.Int, valor = obj.dia6 },
                new OdbcParameterDTO() { nombre = "dia7", tipo = OdbcType.Int, valor = obj.dia7 },
                new OdbcParameterDTO() { nombre = "dia8", tipo = OdbcType.Int, valor = obj.dia8 },
                new OdbcParameterDTO() { nombre = "dia9", tipo = OdbcType.Int, valor = obj.dia9 },
                new OdbcParameterDTO() { nombre = "dia10", tipo = OdbcType.Int, valor = obj.dia10 },
                new OdbcParameterDTO() { nombre = "dia11", tipo = OdbcType.Int, valor = obj.dia11 },
                new OdbcParameterDTO() { nombre = "dia12", tipo = OdbcType.Int, valor = obj.dia12 },
                new OdbcParameterDTO() { nombre = "dia13", tipo = OdbcType.Int, valor = obj.dia13 },
                new OdbcParameterDTO() { nombre = "dia14", tipo = OdbcType.Int, valor = obj.dia14 },
                new OdbcParameterDTO() { nombre = "dia15", tipo = OdbcType.Int, valor = obj.dia15 },
                new OdbcParameterDTO() { nombre = "dia16", tipo = OdbcType.Int, valor = obj.dia16 },
                new OdbcParameterDTO() { nombre = "he_dia1", tipo = OdbcType.Int, valor = obj.he_dia1 },
                new OdbcParameterDTO() { nombre = "he_dia2", tipo = OdbcType.Int, valor = obj.he_dia2 },
                new OdbcParameterDTO() { nombre = "he_dia3", tipo = OdbcType.Int, valor = obj.he_dia3 },
                new OdbcParameterDTO() { nombre = "he_dia4", tipo = OdbcType.Int, valor = obj.he_dia4 },
                new OdbcParameterDTO() { nombre = "he_dia5", tipo = OdbcType.Int, valor = obj.he_dia5 },
                new OdbcParameterDTO() { nombre = "he_dia6", tipo = OdbcType.Int, valor = obj.he_dia6 },
                new OdbcParameterDTO() { nombre = "he_dia7", tipo = OdbcType.Int, valor = obj.he_dia7 },
                new OdbcParameterDTO() { nombre = "he_dia8", tipo = OdbcType.Int, valor = obj.he_dia8 },
                new OdbcParameterDTO() { nombre = "he_dia9", tipo = OdbcType.Int, valor = obj.he_dia9 },
                new OdbcParameterDTO() { nombre = "he_dia10", tipo = OdbcType.Int, valor = obj.he_dia10 },
                new OdbcParameterDTO() { nombre = "he_dia11", tipo = OdbcType.Int, valor = obj.he_dia11 },
                new OdbcParameterDTO() { nombre = "he_dia12", tipo = OdbcType.Int, valor = obj.he_dia12 },
                new OdbcParameterDTO() { nombre = "he_dia13", tipo = OdbcType.Int, valor = obj.he_dia13 },
                new OdbcParameterDTO() { nombre = "he_dia14", tipo = OdbcType.Int, valor = obj.he_dia14 },
                new OdbcParameterDTO() { nombre = "he_dia15", tipo = OdbcType.Int, valor = obj.he_dia15 },
                new OdbcParameterDTO() { nombre = "he_dia16", tipo = OdbcType.Int, valor = obj.he_dia16 },
                new OdbcParameterDTO() { nombre = "bono", tipo = OdbcType.Int, valor = obj.bono },
                new OdbcParameterDTO() { nombre = "observaciones", tipo = OdbcType.Int, valor = obj.observaciones },
                new OdbcParameterDTO() { nombre = "archivo_enviado", tipo = OdbcType.Int, valor = obj.archivo_enviado },
                new OdbcParameterDTO() { nombre = "dias_extras", tipo = OdbcType.Int, valor = obj.dias_extras },
                new OdbcParameterDTO() { nombre = "prima_dominical", tipo = OdbcType.Int, valor = obj.prima_dominical },
            };
            return odbc;
        }
        OdbcConsultaDTO updateIncidenciaEmpDet(IncidenciasEmpDetDTO obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl_det
                            dia1 = ? ,dia2 = ? ,dia3 = ? ,dia4 = ? ,dia5 = ? ,dia6 = ? ,dia7 = ? ,dia8 = ? ,dia9 = ? ,dia10 = ? ,dia11 = ? ,dia12 = ? ,dia13 = ? ,dia14 = ? ,dia15 = ? ,dia16 = ?
                            ,he_dia1 = ? ,he_dia2 = ? ,he_dia3 = ? ,he_dia4 = ? ,he_dia5 = ? ,he_dia6 = ? ,he_dia7 = ? ,he_dia8 = ? ,he_dia9 = ? ,he_dia10 = ? ,he_dia11 = ? ,he_dia12 = ? ,he_dia13 = ? ,he_dia14 = ? ,he_dia15 = ? ,he_dia16 = ?
                            ,bono = ? ,observaciones = ? ,archivo_enviado = ? ,dias_extras = ? ,prima_dominical = ?
                            WHERE id_incidencia = ? AND clave_empleado = ?";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO { nombre = "dia1", tipo = OdbcType.Int, valor = obj.dia1 },
                new OdbcParameterDTO { nombre = "dia2", tipo = OdbcType.Int, valor = obj.dia2 },
                new OdbcParameterDTO { nombre = "dia3", tipo = OdbcType.Int, valor = obj.dia3 },
                new OdbcParameterDTO { nombre = "dia4", tipo = OdbcType.Int, valor = obj.dia4 },
                new OdbcParameterDTO { nombre = "dia5", tipo = OdbcType.Int, valor = obj.dia5 },
                new OdbcParameterDTO { nombre = "dia6", tipo = OdbcType.Int, valor = obj.dia6 },
                new OdbcParameterDTO { nombre = "dia7", tipo = OdbcType.Int, valor = obj.dia7 },
                new OdbcParameterDTO { nombre = "dia8", tipo = OdbcType.Int, valor = obj.dia8 },
                new OdbcParameterDTO { nombre = "dia9", tipo = OdbcType.Int, valor = obj.dia9 },
                new OdbcParameterDTO { nombre = "dia10", tipo = OdbcType.Int, valor = obj.dia10 },
                new OdbcParameterDTO { nombre = "dia11", tipo = OdbcType.Int, valor = obj.dia11 },
                new OdbcParameterDTO { nombre = "dia12", tipo = OdbcType.Int, valor = obj.dia12 },
                new OdbcParameterDTO { nombre = "dia13", tipo = OdbcType.Int, valor = obj.dia13 },
                new OdbcParameterDTO { nombre = "dia14", tipo = OdbcType.Int, valor = obj.dia14 },
                new OdbcParameterDTO { nombre = "dia15", tipo = OdbcType.Int, valor = obj.dia15 },
                new OdbcParameterDTO { nombre = "dia16", tipo = OdbcType.Int, valor = obj.dia16 },
                new OdbcParameterDTO { nombre = "he_dia1", tipo = OdbcType.Int, valor = obj.he_dia1 },
                new OdbcParameterDTO { nombre = "he_dia2", tipo = OdbcType.Int, valor = obj.he_dia2 },
                new OdbcParameterDTO { nombre = "he_dia3", tipo = OdbcType.Int, valor = obj.he_dia3 },
                new OdbcParameterDTO { nombre = "he_dia4", tipo = OdbcType.Int, valor = obj.he_dia4 },
                new OdbcParameterDTO { nombre = "he_dia5", tipo = OdbcType.Int, valor = obj.he_dia5 },
                new OdbcParameterDTO { nombre = "he_dia6", tipo = OdbcType.Int, valor = obj.he_dia6 },
                new OdbcParameterDTO { nombre = "he_dia7", tipo = OdbcType.Int, valor = obj.he_dia7 },
                new OdbcParameterDTO { nombre = "he_dia8", tipo = OdbcType.Int, valor = obj.he_dia8 },
                new OdbcParameterDTO { nombre = "he_dia9", tipo = OdbcType.Int, valor = obj.he_dia9 },
                new OdbcParameterDTO { nombre = "he_dia10", tipo = OdbcType.Int, valor = obj.he_dia10 },
                new OdbcParameterDTO { nombre = "he_dia11", tipo = OdbcType.Int, valor = obj.he_dia11 },
                new OdbcParameterDTO { nombre = "he_dia12", tipo = OdbcType.Int, valor = obj.he_dia12 },
                new OdbcParameterDTO { nombre = "he_dia13", tipo = OdbcType.Int, valor = obj.he_dia13 },
                new OdbcParameterDTO { nombre = "he_dia14", tipo = OdbcType.Int, valor = obj.he_dia14 },
                new OdbcParameterDTO { nombre = "he_dia15", tipo = OdbcType.Int, valor = obj.he_dia15 },
                new OdbcParameterDTO { nombre = "he_dia16", tipo = OdbcType.Int, valor = obj.he_dia16 },
                new OdbcParameterDTO { nombre = "bono", tipo = OdbcType.Int, valor = obj.bono },
                new OdbcParameterDTO { nombre = "observaciones", tipo = OdbcType.Int, valor = obj.observaciones },
                new OdbcParameterDTO { nombre = "archivo_enviado", tipo = OdbcType.Int, valor = obj.archivo_enviado },
                new OdbcParameterDTO { nombre = "dias_extras", tipo = OdbcType.Int, valor = obj.dias_extras },
                new OdbcParameterDTO { nombre = "prima_dominical", tipo = OdbcType.Int, valor = obj.prima_dominical },
                new OdbcParameterDTO { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado },
            };
            return odbc;
        }
        #endregion
        #region GestionPlantilla
        List<tblRH_CatPuestos> getCatPuestos()
        {
            var lst = _contextEnkontrol.Select<tblRH_CatPuestos>(EnkontrolAmbienteEnum.Rh, "SELECT a.puesto, b.descripcion, b.tipo_nomina from sn_plantilla_puesto as a inner join si_puestos as b on  a.puesto= b.puesto where b.tipo_nomina is not null");
            return lst;
        }
        public List<tblRH_BN_Plantilla> getLstGestionBono(BusqBono busq)
        {
            var lstPlantillas = _context.tblRH_BN_Plantilla.ToList().Where(mon => busq.cc == "Todos" ? true : mon.cc.Equals(busq.cc))
                .Where(mon => busq.st == "Todos" ? true : busq.st.ParseInt() == mon.estatus).ToList();
            return lstPlantillas;
        }
        public List<tblRH_BN_Evaluacion> getLstGestionEvaluacion(BusqBono busq)
        {
            var lstEvaluaciones = _context.tblRH_BN_Evaluacion.ToList().Where(mon => busq.cc == "Todos" ? true : mon.cc.Equals(busq.cc))
                .Where(mon => busq.st == "Todos" ? true : busq.st.ParseInt() == mon.estatus).ToList();
            return lstEvaluaciones;
        }

        public string getCCNotificacion(int? autID)
        {
            if (autID.HasValue)
            {
                var evaluacion = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.id == autID.Value);
                if (evaluacion != null)
                {
                    return evaluacion.cc;
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

        public Tuple<List<ComparacionPlantillaBonoDTO>, List<ComparacionPlantillaBonoDTO>> ObtenerComparativaVersionesPlantilla(tblRH_BN_Plantilla plantillaNueva)
        {
            try
            {
                _context.Configuration.AutoDetectChangesEnabled = false;

                if (plantillaNueva.version == 1 || plantillaNueva.version == 0)
                {
                    return null;
                }

                var plantillaAnterior = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc == plantillaNueva.cc && x.version == (plantillaNueva.version - 1));

                var puestosPlantillaAnterior = plantillaAnterior.listDetalle.Select(x => x.puesto);
                var puestosPlantillaNueva = plantillaNueva.listDetalle.Select(x => x.puesto);

                var puestosEliminados = puestosPlantillaAnterior.Except(puestosPlantillaNueva);

                var puestosNuevos = puestosPlantillaNueva.Except(puestosPlantillaAnterior);

                var puestosComun = puestosPlantillaAnterior.Intersect(puestosPlantillaNueva);

                var listaComparacionAnterior = new List<ComparacionPlantillaBonoDTO>();
                var listaComparacionNueva = new List<ComparacionPlantillaBonoDTO>();

                // Se evalúa la lista de puestos de la plantilla activa (anterior).
                foreach (var puesto in plantillaAnterior.listDetalle)
                {
                    var comparacion = new ComparacionPlantillaBonoDTO
                    {
                        puestoID = puesto.puesto,
                        puesto = puesto.puestoNombre,
                        periocidad = puesto.periodicidad,
                        periocidadDesc = ((Tipo_Nomina2Enum)puesto.periodicidad).GetDescription(),
                        monto = puesto.monto
                    };

                    if (puestosEliminados.Contains(puesto.puesto))
                    {
                        comparacion.clase = "eliminado";
                    }
                    else
                    {
                        comparacion.comun = true;

                        var puestoNuevo = plantillaNueva.listDetalle.First(x => x.puesto == puesto.puesto);

                        if ((puesto.periodicidad != puestoNuevo.periodicidad) || (puesto.monto != puestoNuevo.monto))
                        {
                            comparacion.clase = "modificado";
                        }
                        else
                        {
                            comparacion.clase = "normal";
                        }
                    }
                    listaComparacionAnterior.Add(comparacion);
                }

                // Se evalúa la lista de puestos de la plantilla activa (anterior).
                foreach (var puesto in plantillaNueva.listDetalle)
                {
                    var comparacion = new ComparacionPlantillaBonoDTO
                    {
                        puestoID = puesto.puesto,
                        puesto = puesto.puestoNombre,
                        periocidad = puesto.periodicidad,
                        periocidadDesc = ((Tipo_Nomina2Enum)puesto.periodicidad).GetDescription(),
                        monto = puesto.monto
                    };

                    if (puestosNuevos.Contains(puesto.puesto))
                    {
                        comparacion.clase = "nuevo";
                    }
                    else
                    {
                        var puestoAnterior = plantillaAnterior.listDetalle.First(x => x.puesto == puesto.puesto);

                        comparacion.comun = true;

                        if ((puesto.periodicidad != puestoAnterior.periodicidad) || (puesto.monto != puestoAnterior.monto))
                        {
                            comparacion.clase = "modificado";
                        }
                        else
                        {
                            comparacion.clase = "normal";
                        }
                    }
                    listaComparacionNueva.Add(comparacion);
                }

                listaComparacionAnterior = listaComparacionAnterior.OrderBy(x => x.comun).ThenBy(x => x.puestoID).ToList();
                listaComparacionNueva = listaComparacionNueva.OrderBy(x => x.comun).ThenBy(x => x.puestoID).ToList();

                return Tuple.Create(listaComparacionAnterior, listaComparacionNueva);
            }
            catch (Exception e)
            {
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, MethodBase.GetCurrentMethod().Name, e, AccionEnum.CONSULTA, 0, null);
                return null;
            }
        }
        #endregion
        #region CapturaPlantilla
        public List<BonoPuestoDTO> ActualizarPuestos(List<tblRH_CatPuestos> puestos, string cc, bool isGestion)
        {
            var data = new List<BonoPuestoDTO>();
            var plantillaTemp = _context.tblRH_BN_Plantilla.Where(x=>x.cc.Equals(cc) && x.estatus==0 && x.versionActiva==false).OrderByDescending(x=>x.version).FirstOrDefault();
            var plantillaTemp2 = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc.Equals(cc) && x.versionActiva);
            var plantilla = new tblRH_BN_Plantilla();
            if (isGestion)
            {
                plantilla = plantillaTemp;
            }
            else
            {
                plantilla = plantillaTemp2;
            }

            if (plantilla == null)
            {
                plantilla = new tblRH_BN_Plantilla { listDetalle = new List<tblRH_BN_Plantilla_Det>() };
            }

            puestos.ForEach(puesto =>
            {
                var aux = new BonoPuestoDTO()
                {
                    puestoID = puesto.puesto,
                    puesto = puesto.descripcion,
                    tipoNominaCve = puesto.tipo_nomina,
                    tipoNomina = puesto.tipo_nomina==1?"SEMANAL":puesto.tipo_nomina==4?"QUINCENAL":"MENSUAL"
                };
                var puestoGuardadoAux = plantilla.listDetalle.FirstOrDefault(x => x.puesto == puesto.puesto);
                if (puestoGuardadoAux != null)
                {
                    aux.id = puestoGuardadoAux.id;
                    aux.monto = puestoGuardadoAux.monto;
                    aux.periodicidad = puestoGuardadoAux.periodicidad;
                }
                data.Add(aux);
            });
            return data;
        }
        public tblRH_BN_Plantilla getPlantilla(int id)
        {
            return _context.tblRH_BN_Plantilla.FirstOrDefault(w => w.id == id);
        }
        public tblRH_BN_Plantilla getPlantilla(string cc, bool isGestion)
        {
            var plantillaTemp = _context.tblRH_BN_Plantilla.Where(x => x.cc.Equals(cc) && x.estatus == 0 && x.versionActiva == false).OrderByDescending(x => x.version).FirstOrDefault();
            var plantillaTemp2 = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc.Equals(cc) && x.versionActiva);
            var fecha = new tblRH_BN_Plantilla();
            if (isGestion)
            {
                fecha = plantillaTemp;
            }
            else
            {
                fecha = plantillaTemp2;
            }

            if (fecha == null)
            {
                fecha = new tblRH_BN_Plantilla()
                {
                    cc = cc,
                    fechaCaptura = DateTime.Now,
                    fechaInicio = DateTime.Now,
                    fechaFin = DateTime.Now,
                    listAutorizadores = new List<tblRH_BN_Plantilla_Aut>(),
                    listDetalle = new List<tblRH_BN_Plantilla_Det>()
                };
            }
            return fecha;
        }
        public tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(int id)
        {
            return _context.tblRH_BN_Plantilla_Cuadrado.FirstOrDefault(w => w.id == id);
        }
        public tblRH_BN_Plantilla_Cuadrado getPlantilla_Cuadrado(string cc, bool isGestion)
        {
            var plantillaTemp = _context.tblRH_BN_Plantilla_Cuadrado.Where(x => x.cc.Equals(cc) && x.estatus == 0 && x.versionActiva == false).OrderByDescending(x => x.version).FirstOrDefault();
            var plantillaTemp2 = _context.tblRH_BN_Plantilla_Cuadrado.FirstOrDefault(x => x.cc.Equals(cc) && x.versionActiva);
            var fecha = new tblRH_BN_Plantilla_Cuadrado();
            if (isGestion)
            {
                fecha = plantillaTemp;
            }
            else
            {
                fecha = plantillaTemp2;
            }

            if (fecha == null)
            {
                fecha = new tblRH_BN_Plantilla_Cuadrado()
                {
                    cc = cc,
                    fechaCaptura = DateTime.Now,
                    fechaInicio = DateTime.Now,
                    fechaFin = DateTime.Now,
                    listDetalle = new List<tblRH_BN_Plantilla_Cuadrado_Det>()
                };
            }
            return fecha;
        }
        public List<tblRH_BN_Plantilla_Aut> getLstAuth(int plantillaID)
        {
            return _context.tblRH_BN_Plantilla_Aut.ToList().Where(w => w.plantillaID == plantillaID).ToList();
        }
        public List<BonoPuestoDTO> ActualizarPuestos_Cuadrado(List<tblRH_CatPuestos> puestos, string cc, bool isGestion)
        {
            var data = new List<BonoPuestoDTO>();
            var plantillaTemp = _context.tblRH_BN_Plantilla_Cuadrado.Where(x => x.cc.Equals(cc) && x.estatus == 0 && x.versionActiva == false).OrderByDescending(x => x.version).FirstOrDefault();
            var plantillaTemp2 = _context.tblRH_BN_Plantilla_Cuadrado.FirstOrDefault(x => x.cc.Equals(cc) && x.versionActiva);
            var plantilla = new tblRH_BN_Plantilla_Cuadrado();
            if (isGestion)
            {
                plantilla = plantillaTemp;
            }
            else
            {
                plantilla = plantillaTemp2;
            }

            if (plantilla == null)
            {
                plantilla = new tblRH_BN_Plantilla_Cuadrado { listDetalle = new List<tblRH_BN_Plantilla_Cuadrado_Det>() };
            }

            puestos.ForEach(puesto =>
            {
                var aux = new BonoPuestoDTO()
                {
                    puestoID = puesto.puesto,
                    puesto = puesto.descripcion,
                    tipoNominaCve = puesto.tipo_nomina,
                    tipoNomina = puesto.tipo_nomina == 1 ? "SEMANAL" : puesto.tipo_nomina == 4 ? "QUINCENAL" : "MENSUAL"
                };
                var puestoGuardadoAux = plantilla.listDetalle.FirstOrDefault(x => x.puesto == puesto.puesto);
                if (puestoGuardadoAux != null)
                {
                    aux.id = puestoGuardadoAux.id;
                    aux.monto = puestoGuardadoAux.monto;
                    aux.periodicidad = puestoGuardadoAux.periodicidad;
                }
                data.Add(aux);
            });
            return data;
        }
        #endregion
        #region Evaluacion
        public List<EmpleadoPuestoDTO> getLstUsuariosFormPuestos(BusqBonoEvaluacion busq)
        {
            var odbc = new OdbcConsultaDTO()
            {
                consulta = queryUsuariosFormPuestos(busq),
                parametros = paramUsuariosFormPuestos(busq)
            };
            var lst = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            return lst.OrderBy(x=>x.nombre+x.ape_paterno).ToList();
        }
        string queryUsuariosFormPuestos(BusqBonoEvaluacion busq)
        {
            return string.Format(@"SELECT emp.clave_empleado ,emp.nombre ,emp.ape_paterno ,emp.ape_materno ,emp.puesto ,pue.descripcion
                                FROM sn_empleados emp
                                INNER JOIN si_puestos pue ON pue.puesto = emp.puesto
                                INNER JOIN sn_departamentos dep ON emp.cc_contable = dep.cc
                                WHERE emp.estatus_empleado <> 'B' AND emp.cc_contable = ? AND emp.puesto IN {0}"
                , busq.lstPuestos.ToParamInValue());
        }
        List<OdbcParameterDTO> paramUsuariosFormPuestos(BusqBonoEvaluacion busq)
        {
            var lst = new List<OdbcParameterDTO>();
            lst.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.VarChar, valor = busq.cc });
            lst.AddRange(busq.lstPuestos.Select(p => new OdbcParameterDTO() { nombre = "puesto", tipo = OdbcType.Numeric, valor = p }));
            return lst;
        }
        public tblRH_BN_Plantilla getPlantillaAutorizada(BusqBonoEvaluacion busq)
        {
            var plan = _context.tblRH_BN_Plantilla.ToList()
                .Where(w => w.cc == busq.cc)
                .Where(w => w.estatus == (int)authEstadoEnum.Autorizado)
                .Where(w => w.fechaInicio.Year <= busq.fecha.Year && w.fechaInicio.Month <= busq.fecha.Month)
                .Where(w => w.fechaFin.Year >= busq.fecha.Year && w.fechaFin.Month >= busq.fecha.Month)
                .OrderBy(o => o.id).FirstOrDefault();
            return plan;
        }
        public tblRH_BN_Evaluacion getEvaluacion(int idPlantilla)
        {
            var eval = _context.tblRH_BN_Evaluacion.ToList().FirstOrDefault(w => w.plantillaID == idPlantilla);
            return eval;
        }
        public tblRH_BN_Evaluacion getEvaluacionByID(int id)
        {
            return _context.tblRH_BN_Evaluacion.FirstOrDefault(w => w.id == id);
        }
        public int guardarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Aut> aut)
        {
            obj.fecha = DateTime.Now;
            obj.anio = DateTime.Now.Year;
            obj.usuarioEvaluoID = vSesiones.sesionUsuarioDTO.id;
            obj.aplicado = false;
            obj.fechaAplicacion = DateTime.Now;

            _context.tblRH_BN_Evaluacion.Add(obj);
            _context.SaveChanges();

            //foreach (var x in det)
            //{
            //    x.evaluacionID = obj.id;
            //    x.fechaAlta = DateTime.Now;
            //    x.fechaAplicacion = DateTime.Now;
 
            //}
            //_context.tblRH_BN_Evaluacion_Det.AddRange(det);
            //_context.SaveChanges();

            foreach (var x in aut)
	        {
		        x.evaluacionID = obj.id;
                x.fecha = DateTime.Now;
	        }
            _context.tblRH_BN_Evaluacion_Aut.AddRange(aut);
            _context.SaveChanges();


            return obj.id;
        }
        public bool guardarEvaluacionDet(List<tblRH_BN_Evaluacion_Det> det)
        {

            foreach (var x in det)
            {
                x.fechaAlta = DateTime.Now;
                x.fechaAplicacion = DateTime.Now;

            }
            _context.tblRH_BN_Evaluacion_Det.AddRange(det);
            _context.SaveChanges();

            return true;
        }

        public int actualizarEvaluacion(tblRH_BN_Evaluacion obj, List<tblRH_BN_Evaluacion_Det> det)
        {
            var newObj = _context.tblRH_BN_Evaluacion.FirstOrDefault(x=>x.id == obj.id);
            newObj.periodo = obj.periodo;
            newObj.fechaInicio = obj.fechaInicio;
            newObj.fechaFin = obj.fechaFin;


            foreach (var i in newObj.listDetalle)
            {
                var porcentaje_Asig = det.FirstOrDefault(x => x.id == i.id);
                var monto_Asig = det.FirstOrDefault(x => x.id == i.id);
                var con_Bono = det.FirstOrDefault(x => x.id == i.id);

                i.porcentaje_Asig = porcentaje_Asig.porcentaje_Asig;
                i.monto_Asig = monto_Asig.monto_Asig;
                i.con_Bono = con_Bono.con_Bono;
            }
            _context.SaveChanges();
            return obj.id;
        }
        #endregion
        #region Incidencias
        public List<PeriodosNominaDTO> getPeriodoActual()
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = string.Format("SELECT * FROM sn_periodos WHERE tipo_nomina IN {0} AND fecha_inicial <= ? AND fecha_final >= ?", EnumExtensions.ToParamInValue<Tipo_Nomina2Enum>()),
            //};
            //odbc.parametros.AddRange(EnumExtensions.ToCombo<Tipo_Nomina2Enum>().Select(nom => new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = nom.Value }));
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha_inicial", tipo = OdbcType.Date, valor = DateTime.Now });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha_final", tipo = OdbcType.Date, valor = DateTime.Now });
            //var per = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolAmbienteEnum.Rh, odbc);

            var auxFecha = DateTime.Today;

            List<PeriodosNominaDTO> per = _context.tblRH_EK_Periodos.Where(x => x.fecha_inicial <= auxFecha && x.fecha_final >= auxFecha)
                .Select(x => new PeriodosNominaDTO {
                    tipo_nomina = x.tipo_nomina,
                    periodo  = x.periodo,
                    tipo_periodo  = x.tipo_periodo,
                    fecha_inicial  = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = (x.fecha_pago ?? DateTime.Today),
                    year = x.year
                }).ToList();
            foreach (var x in per)
            {
                x.fecha_inicial = x.fecha_inicial.AddHours(8);
                x.fecha_final = x.fecha_final.AddHours(8);
                x.fecha_pago = x.fecha_pago;
                x.fecha_inicialStr = x.fecha_inicial.ToShortDateString();
                x.fecha_finalStr = x.fecha_final.ToShortDateString();
                x.fecha_pagoStr = (x.fecha_pago ?? DateTime.Now).ToShortDateString();
            }
            return per;
        }
        public List<PeriodosNominaDTO> getPeriodoActual(int tipoNomina)
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = string.Format("SELECT * FROM sn_periodos WHERE tipo_nomina = ? AND fecha_inicial <= ? AND fecha_final >= ?", EnumExtensions.ToParamInValue<Tipo_Nomina2Enum>()),
            //};
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = tipoNomina });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha_inicial", tipo = OdbcType.Date, valor = DateTime.Now });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "fecha_final", tipo = OdbcType.Date, valor = DateTime.Now });
            //var per = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            var auxFecha = DateTime.Today;

            List<PeriodosNominaDTO> per = _context.tblRH_EK_Periodos.Where(x => x.tipo_nomina == tipoNomina && x.fecha_inicial <= auxFecha && x.fecha_final >= auxFecha)
                .Select(x => new PeriodosNominaDTO {
                    tipo_nomina = x.tipo_nomina,
                    periodo  = x.periodo,
                    tipo_periodo  = x.tipo_periodo,
                    fecha_inicial = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = (x.fecha_pago ?? DateTime.Today),
                    year = x.year
                }).ToList();
            foreach (var x in per)
            {
                x.fecha_inicial = x.fecha_inicial.AddHours(8);
                x.fecha_final = x.fecha_final.AddHours(8);
                x.fecha_inicialStr = x.fecha_inicial.ToShortDateString();
                x.fecha_finalStr = x.fecha_final.ToShortDateString();
                x.fecha_pagoStr = (x.fecha_pago ?? DateTime.Now).ToShortDateString();
            }
            return per;
        }
        public List<PeriodosNominaDTO> getPeriodo(int anio, int periodo)
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = string.Format("SELECT * FROM sn_periodos WHERE tipo_nomina IN {0} AND year = ? AND periodo = ?", EnumExtensions.ToParamInValue<Tipo_Nomina2Enum>()),
            //};
            //odbc.parametros.AddRange(EnumExtensions.ToCombo<Tipo_Nomina2Enum>().Select(nom => new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = nom.Value }));
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = anio });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = periodo });
            //var per = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolAmbienteEnum.Rh, odbc);

            List<PeriodosNominaDTO> per = _context.tblRH_EK_Periodos.Where(x => x.periodo == periodo && x.year == anio)
                .Select(x => new PeriodosNominaDTO {
                    tipo_nomina = x.tipo_nomina,
                    periodo  = x.periodo,
                    tipo_periodo  = x.tipo_periodo,
                    fecha_inicial = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = (x.fecha_pago ?? DateTime.Today),
                    year = x.year
                }).ToList();
            foreach (var x in per)
            {
                x.fecha_inicial = x.fecha_inicial.AddHours(8);
                x.fecha_final = x.fecha_final.AddHours(8);
                x.fecha_inicialStr = x.fecha_inicial.ToShortDateString();
                x.fecha_finalStr = x.fecha_final.ToShortDateString();
                x.fecha_pagoStr = (x.fecha_pago ?? DateTime.Now).ToShortDateString();
            }
            return per;
        }
        public List<PeriodosNominaDTO> getPeriodos(int anio,int tipoNomina)
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = string.Format("SELECT * FROM sn_periodos WHERE tipo_nomina = ? AND year = ? ", EnumExtensions.ToParamInValue<Tipo_Nomina2Enum>()),
            //};
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = tipoNomina });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = anio });
            //var per = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolAmbienteEnum.Rh, odbc);

            List<PeriodosNominaDTO> per = _context.tblRH_EK_Periodos.Where(x => (x.year == anio) && x.tipo_nomina == tipoNomina)
                .Select(x => new PeriodosNominaDTO {
                    tipo_nomina = x.tipo_nomina,
                    periodo  = x.periodo,
                    tipo_periodo  = x.tipo_periodo,
                    fecha_inicial = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = (x.fecha_pago ?? DateTime.Today),
                    year = x.year
                }).ToList();
            foreach (var x in per)
            {
                x.fecha_inicial = x.fecha_inicial.AddHours(8);
                x.fecha_final = x.fecha_final.AddHours(8);
                x.fecha_inicialStr = x.fecha_inicial.ToShortDateString();
                x.fecha_finalStr = x.fecha_final.ToShortDateString();
                x.fecha_pagoStr = (x.fecha_pago ?? DateTime.Now).ToShortDateString();
            }
            return per;
        }
        public List<PeriodosNominaDTO> getPeriodosRestantes(int anio, int tipoNomina)
        {
            //var odbc = new OdbcConsultaDTO()
            //{
            //    consulta = string.Format("SELECT * FROM sn_periodos WHERE tipo_nomina = ? AND year = ? AND mes_cc >= ? ", EnumExtensions.ToParamInValue<Tipo_Nomina2Enum>()),
            //};
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = tipoNomina });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "year", tipo = OdbcType.Int, valor = anio });
            //odbc.parametros.Add(new OdbcParameterDTO() { nombre = "mes_cc", tipo = OdbcType.Int, valor = DateTime.Now.Month });
            //var per = _contextEnkontrol.Select<PeriodosNominaDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            var auxFecha = DateTime.Now.Month;

            List<PeriodosNominaDTO> per = _context.tblRH_EK_Periodos.Where(x => x.year == anio && x.tipo_nomina == tipoNomina && x.mes_cc >= auxFecha)
                .Select(x => new PeriodosNominaDTO {
                    tipo_nomina = x.tipo_nomina,
                    periodo  = x.periodo,
                    tipo_periodo  = x.tipo_periodo,
                    fecha_inicial = x.fecha_inicial,
                    fecha_final = x.fecha_final,
                    fecha_pago = (x.fecha_pago ?? DateTime.Today),
                    year = x.year
                }).ToList();
            return per;
            foreach (var x in per)
            {
                x.fecha_inicial = x.fecha_inicial.AddHours(8);
                x.fecha_final = x.fecha_final.AddHours(8);
                x.fecha_inicialStr = x.fecha_inicial.ToShortDateString();
                x.fecha_finalStr = x.fecha_final.ToShortDateString();
                x.fecha_pagoStr = (x.fecha_pago ?? DateTime.Now).ToShortDateString();
            }
        }
        public IncidenciasPaqueteDTO getLstIncidenciasEnk(BusqIncidenciaDTO busq)
        {
         
            var result = new IncidenciasPaqueteDTO();
            var cplan = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && x.periodo == busq.periodo && busq.anio == x.anio);
            var incapacidades = _context.tblRH_Vacaciones_Incapacidades.Where(x => x.cc == busq.cc && x.esActivo && x.estatus == 1).ToList();
            var vacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(x => x.registroActivo && x.estado == 1 && x.esPagadas.Value != true).ToList();
            List<tblRH_BN_Incidencia_det_Peru> datosPeru = new List<tblRH_BN_Incidencia_det_Peru>();
            if (cplan != null)
            {
                cplan.nombreAutoriza = "";
                if (cplan.estatus.Equals("P"))
                {
                    //getIncidencia_det_new(cplan.id, busq);
                }
                else {
                    if (cplan.usuario_autoriza_sigoplan != null)
                    {
                        var nom =_context.tblP_Usuario.FirstOrDefault(x => x.id == cplan.usuario_autoriza_sigoplan);

                        cplan.nombreAutoriza = nom.nombre;
                    }
                }

                #region CPLAN
                var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == cplan.id && (busq.depto > 0 ? x.clave_depto == busq.depto : true)).ToList();
                var detPeru = _context.tblRH_BN_Incidencia_det_Peru.Where(x => x.incidenciaID == cplan.id).ToList();
                if (det.Count() > 0)
                {                    
                    det.ForEach(x => x.estatus = true);
                    var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidencias_det_EnkEmptyNew(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                    var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = odbc_det_empty.consulta,
                        parametros = new
                        {
                            anio = busq.anio,
                            periodo = busq.periodo,
                            fechaBajaInicio = busq.fechaInicio,
                            fechaBajaFin = busq.fechaFin,
                            cc_contable = busq.cc,
                            tipo_nomina = busq.tipoNomina,
                            clave_depto = busq.depto
                        }
                    }).ToList();

                    // Si no existe actualmente en el cc eliminarlo
                    //foreach (var item in det)
                    //{
                    //    var auxDetalle = det_empty.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                    //    if (auxDetalle == null)
                    //    {
                    //        det.Remove(item);
                    //    }
                    //}

                    foreach (var item in det)
                    {
                        tblRH_BN_Incidencia_det_Peru datoPeru = detPeru.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if(datoPeru == null)
                        {
                            datoPeru = new tblRH_BN_Incidencia_det_Peru();
                            datoPeru.dia1 = 8;
                            datoPeru.dia2 = 8;
                            datoPeru.dia3 = 8;
                            datoPeru.dia4 = 8;
                            datoPeru.dia5 = 8;
                            datoPeru.dia6 = 8;
                            datoPeru.dia7 = 8;
                            datoPeru.dia8 = 8;
                            datoPeru.dia9 = 8;
                            datoPeru.dia10 = 8;
                            datoPeru.dia11 = 8;
                            datoPeru.dia12 = 8;
                            datoPeru.dia13 = 8;
                            datoPeru.dia14 = 8;
                            datoPeru.dia15 = 8;
                            datoPeru.dia16 = 8;
                            datoPeru.registroActivo = true;
                            datoPeru.incidencia_detID = item.id;
                            datoPeru.incidenciaID = item.incidenciaID;  
                            datoPeru.clave_empleado = item.clave_empleado;
                        }
                        datosPeru.Add(datoPeru);


                        if (cplan.estatus != "A")
                        {
                            var bonos = _context.tblRH_BN_Evaluacion.OrderBy(e => e.periodo)
                                    .Where(x => x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado).ToList();

                            var idsIncidenciaAplicado = bonos.Where(e => e.idIncidencia.HasValue).Select(e => e.idIncidencia).ToList();

                            #region BONO GUARDADO
                            if (idsIncidenciaAplicado.Contains(cplan.id))
                            {
                                foreach (var itemBono in bonos)
                                {
                                    var idsDetallesBonos = itemBono.listDetalle.Select(e => e.id).ToList();

                                    if (idsDetallesBonos.Contains(item.evaluacion_detID))
                                    {
                                        var objBonoDet = itemBono.listDetalle.FirstOrDefault(e => e.id == item.evaluacion_detID);
                                        item.bonoDM = objBonoDet.monto_Asig;
                                    }
                                    else
                                    {
                                        var idsEmpDetallesBonos = itemBono.listDetalle.Select(e => e.cve_Emp).ToList();

                                        if (idsEmpDetallesBonos.Contains(item.clave_empleado))
                                        {
                                            var objBonoDet = itemBono.listDetalle.FirstOrDefault(e => e.cve_Emp == item.clave_empleado);
                                            item.bonoDM = objBonoDet.monto_Asig;
                                            item.evaluacion_detID = objBonoDet.id;
                                        }
                                    }
                                }
                            }
                            var bonosCuadrado = _context.tblRH_BN_Plantilla_Cuadrado_Det.OrderByDescending(e => e.plantilla.fechaInicio).Where(x => x.plantilla.cc == busq.cc && x.tipoNominaCve == busq.tipoNomina).ToList();
                            if (bonosCuadrado.Count > 0)
                            {
                                //empty.evaluacionID = bonos.id;
                                foreach (var i in det)
                                {
                                    var hasBono = bonosCuadrado.FirstOrDefault(x => x.empleado == i.clave_empleado);
                                    if (hasBono != null)
                                    {
                                        i.bonoCuadrado = hasBono.monto;
                                    }
                                }
                            }
                            #endregion

                            var clave_empleadoString = item.clave_empleado.ToString();
                            var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == item.clave_empleado);
                            var objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == item.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                            var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                            var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                            var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                                && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                                && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();


                            //OBTENER CUAL ES EL PERIODO DONDE SE MOSTRARAN LOS DIAS EXTRATEMPORALES (EN ESTATUS PENDIENTE)
                            var lstIncideciasCC = _context.tblRH_BN_Incidencia.Where(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && busq.anio == x.anio).OrderByDescending(e => e.periodo).ToList();
                            var objUltimaIncidenciaPendientes = lstIncideciasCC.FirstOrDefault(e => e.estatus == "P");
                            var objUltimaIncidenciaAutorizadas = lstIncideciasCC.FirstOrDefault(e => e.estatus == "A");
                            int añoActual = DateTime.Now.Year;
                            var objPeriodoActual = _context.tblRH_EK_Periodos.Where(e => e.tipo_nomina == cplan.tipo_nomina && e.year == añoActual).ToList()
                                .FirstOrDefault(e => e.fecha_inicial.Date <= DateTime.Now.Date && e.fecha_final.Date >= DateTime.Now.Date);
                            int periodoAplicanExtratemporales = 0;

                            // SI NO TIENE INCIDENCIAS PENDIENTES SE ASIGNA AL SIGUIENTE
                            if (objUltimaIncidenciaPendientes == null)
                            {
                                var ultimaIncidencia = lstIncideciasCC.FirstOrDefault();

                                if (ultimaIncidencia != null)
                                {
                                    if (ultimaIncidencia.periodo < objPeriodoActual.periodo)
                                    {
                                        periodoAplicanExtratemporales = objPeriodoActual.periodo;

                                    }
                                    else
                                    {
                                        periodoAplicanExtratemporales = ultimaIncidencia.periodo + 1;

                                    }
                                }
                                else
                                {
                                    periodoAplicanExtratemporales = objPeriodoActual.periodo;
                                }

                            }
                            else
                            {

                                // SI LA ULTIMA INCIDENCIA PENDIENTE NO ES LA UNICA SE LE ASIGNA A LA DEL PERIODO ACTUAL
                                if (objUltimaIncidenciaAutorizadas != null)
                                {
                                    periodoAplicanExtratemporales = objUltimaIncidenciaAutorizadas.periodo + 1;

                                }
                                else
                                {
                                    periodoAplicanExtratemporales = objPeriodoActual.periodo;

                                }
                            }

                            int numDiasExtratemporales = 0;
                            int numDiasExtratemporalesARestar = 0;
                            var lstFechasExtra = new List<VacFechasDTO>();
                            List<int> diasVacaciones = new List<int>();
                            List<int> diasVacacionesTipo = new List<int>();

                            foreach (var itemVacaciones in vacacionesEmpleado)
                            {
                                #region DESC TIPO VACACIONES
                                string descMotivo = "";

                                switch (itemVacaciones.tipoVacaciones)
                                {
                                    case 0:
                                        descMotivo = "Permiso paternidad";
                                        break;
                                    case 1:
                                        descMotivo = "Permiso de matrimonio";
                                        break;
                                    case 2:
                                        descMotivo = "Permiso sindical";
                                        break;
                                    case 3:
                                        descMotivo = "Permiso por fallecimiento";
                                        break;
                                    case 5:
                                        descMotivo = "Permiso médico";
                                        break;
                                    case 7:
                                        descMotivo = "Vacaciones";
                                        break;
                                    case 8:
                                        descMotivo = "Permiso SIN goce de sueldo";
                                        break;
                                    case 9:
                                        descMotivo = "Permiso de comision de trabajo";
                                        break;
                                    case 10:
                                        descMotivo = "Home office";
                                        break;
                                    case 11:
                                        descMotivo = "Tiempo x tiempo";
                                        break;
                                    case 12:
                                        descMotivo = "Incapacidades";
                                        break;
                                    case 13:
                                        descMotivo = "Suspención (SUSP)";
                                        break;
                                    default:
                                        descMotivo = "S/N";
                                        break;
                                }
                                #endregion

                                var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                                foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado)
                                {
                                    if (cplan.estatus == "A")
                                    {
                                        if (itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fechaAplicadas.Value.Date == cplan.fecha_auto.Date && itemVacacionesDetalle.fecha.Value.Date < busq.fechaInicio.Date)
                                        {
                                            numDiasExtratemporales++;

                                            //PERMISO SIN GOSE SUELDO
                                            if (itemVacacionesDetalle.tipoInsidencia == 3)
                                            {
                                                numDiasExtratemporalesARestar++;
                                            }

                                            var objVacFecha = new VacFechasDTO();
                                            objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                            objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                            objVacFecha.descTipoVacaciones = descMotivo;

                                            lstFechasExtra.Add(objVacFecha);
                                        }
                                    }
                                    else
                                    {
                                        if (!itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fecha.Value.Date < busq.fechaInicio.Date && periodoAplicanExtratemporales == cplan.periodo)
                                        {
                                            numDiasExtratemporales++;

                                            //PERMISO SIN GOSE SUELDO
                                            if (itemVacacionesDetalle.tipoInsidencia == 3)
                                            {
                                                numDiasExtratemporalesARestar++;
                                            }

                                            var objVacFecha = new VacFechasDTO();
                                            objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                            objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                            objVacFecha.descTipoVacaciones = descMotivo;

                                            lstFechasExtra.Add(objVacFecha);
                                        }
                                    }

                                    if (itemVacacionesDetalle.fecha.Value.Date >= busq.fechaInicio.Date && itemVacacionesDetalle.fecha.Value.Date <= busq.fechaFin.Date)
                                    {
                                        TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                        int diaVacaciones = difFechasVacaciones.Days + 1;
                                        diasVacaciones.Add(diaVacaciones);
                                        diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                                    }
                                }
                            }

                            var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == item.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                                && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                                ).ToList();

                            List<int> diasIncapacidades = new List<int>();
                            foreach (var itemIncapacidad in incapacidadesEmpleado)
                            {

                                TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                                TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                                int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                                int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                                //int rango = (diasIncapacidadFin - diasIncapacidadInicio ) + 1;
                                for (int i = diasIncapacidadInicio; i <= diasIncapacidadFin; i++) diasIncapacidades.Add(i);

                                //CHECAR SI ALGUNA DE LAS INCAPACIDADES NO FUERON APLICADAS EN LAS INCIDENCIAS
                                DateTime tempFechaInicial = itemIncapacidad.fechaInicio.Date;
                                while (tempFechaInicial <= itemIncapacidad.fechaTerminacion.Date)
                                {
                                    if (!itemIncapacidad.esAplicadaIncidencias && tempFechaInicial < busq.fechaInicio && periodoAplicanExtratemporales == cplan.periodo)
                                    {
                                        numDiasExtratemporales++;
                                        numDiasExtratemporalesARestar++; //INCAPACIDADES

                                        var objVacFecha = new VacFechasDTO();
                                        objVacFecha.fecha = tempFechaInicial;
                                        objVacFecha.tipoVacaciones = 12;
                                        objVacFecha.descTipoVacaciones = "Incapacidades";
                                        lstFechasExtra.Add(objVacFecha);
                                    }

                                    tempFechaInicial = tempFechaInicial.AddDays(1);
                                }
                            }

                            item.numDiasExtratemporales = numDiasExtratemporales;
                            item.numDiasExtratemporalesARestar = numDiasExtratemporalesARestar;
                            item.lstFechasExtratemporaneas = lstFechasExtra;

                            //item.dia1 = 0;
                            if (diasVacaciones.Contains(1)) item.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) item.dia1 = 10;

                            //item.dia2 = 0;
                            if (diasVacaciones.Contains(2)) item.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) item.dia2 = 10;

                            //item.dia3 = 0;
                            if (diasVacaciones.Contains(3)) item.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) item.dia3 = 10;

                            //item.dia4 = 0;
                            if (diasVacaciones.Contains(4)) item.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) item.dia4 = 10;

                            //item.dia5 = 0;
                            if (diasVacaciones.Contains(5)) item.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) item.dia5 = 10;

                            //item.dia6 = 0;
                            if (diasVacaciones.Contains(6)) item.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) item.dia6 = 10;

                            //item.dia7 = 0;
                            if (diasVacaciones.Contains(7)) item.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) item.dia7 = 10;

                            //item.dia8 = 0;
                            if (diasVacaciones.Contains(8)) item.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) item.dia8 = 10;

                            //item.dia9 = 0;
                            if (diasVacaciones.Contains(9)) item.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) item.dia9 = 10;

                            //item.dia10 = 0;
                            if (diasVacaciones.Contains(10)) item.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) item.dia10 = 10;

                            //item.dia11 = 0;
                            if (diasVacaciones.Contains(11)) item.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) item.dia11 = 10;

                            //item.dia12 = 0;
                            if (diasVacaciones.Contains(12)) item.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) item.dia12 = 10;

                            //item.dia13 = 0;
                            if (diasVacaciones.Contains(13)) item.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) item.dia13 = 10;

                            //item.dia14 = 0;
                            if (diasVacaciones.Contains(14)) item.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) item.dia14 = 10;

                            //item.dia15 = 0;
                            if (diasVacaciones.Contains(15)) item.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) item.dia15 = 10;

                            //item.dia16 = 0;
                            if (diasVacaciones.Contains(16)) item.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) item.dia16 = 10;
                        }

                    }

                    // Si no existe actualmente en el guardado, dejarlo como pendiente de guardado
                    foreach (var item in det_empty) 
                    {
                        var auxDetalle = det.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if (auxDetalle == null)
                        {
                            #region SI LA FECHA DE ALTA CAE ENTRE EL PERIODO DE LA BUSQUEDA ASIGNAR LAS ASISTENCIAS CORREPONDIENTES
                            List<int> lstDiasAlta = new List<int>();

                            if (item.fechaAlta.Date >= busq.fechaInicio.Date && item.fechaAlta <= busq.fechaFin.Date)
                            {
                                TimeSpan difFechasRangoInicio = item.fechaAlta.Date - busq.fechaInicio;
                                TimeSpan difFechasRangoFin =  busq.fechaFin - busq.fechaInicio;
                                int diasRangoInicio = difFechasRangoInicio.Days + 1;
                                int diasRangoFin = difFechasRangoFin.Days + 1;
                                //int rango = (diasIncapacidadFin - diasIncapacidadInicio ) + 1;
                                for (int i = diasRangoInicio; i <= diasRangoFin; i++) lstDiasAlta.Add(i);
                            }

                            if (lstDiasAlta.Contains(1)) { item.dia1 = 0; } else { item.dia1 = 19; }
                            if (lstDiasAlta.Contains(2)) { item.dia2 = 0; } else { item.dia2 = 19; }
                            if (lstDiasAlta.Contains(3)) { item.dia3 = 0; } else { item.dia3 = 19; }
                            if (lstDiasAlta.Contains(4)) { item.dia4 = 0; } else { item.dia4 = 19; }
                            if (lstDiasAlta.Contains(5)) { item.dia5 = 0; } else { item.dia5 = 19; }
                            if (lstDiasAlta.Contains(6)) { item.dia6 = 0; } else { item.dia6 = 19; }
                            if (lstDiasAlta.Contains(7)) { item.dia7 = 0; } else { item.dia7 = 19; }
                            if (lstDiasAlta.Contains(8)) { item.dia8 = 0; } else { item.dia8 = 19; }
                            if (lstDiasAlta.Contains(9)) { item.dia9 = 0; } else { item.dia9 = 19; }
                            if (lstDiasAlta.Contains(10)) { item.dia10 = 0; } else { item.dia10 = 19; }
                            if (lstDiasAlta.Contains(11)) { item.dia11 = 0; } else { item.dia11 = 19; }
                            if (lstDiasAlta.Contains(12)) { item.dia12 = 0; } else { item.dia12 = 19; }
                            if (lstDiasAlta.Contains(13)) { item.dia13 = 0; } else { item.dia13 = 19; }
                            if (lstDiasAlta.Contains(14)) { item.dia14 = 0; } else { item.dia14 = 19; }
                            if (lstDiasAlta.Contains(15)) { item.dia15 = 0; } else { item.dia15 = 19; }
                            if (lstDiasAlta.Contains(16)) { item.dia16 = 0; } else { item.dia16 = 19; } 

                            #endregion,

                            #region SI LA FECHA DE ALTA ES MAYOR AL PERIODO
                            if (item.fechaAlta.Date > busq.fechaFin.Date)
                            {
                                item.dia1 = 19;
                                item.dia2 = 19;
                                item.dia3 = 19;
                                item.dia4 = 19;
                                item.dia5 = 19;
                                item.dia6 = 19;
                                item.dia7 = 19;
                                item.dia8 = 19;
                                item.dia9 = 19;
                                item.dia10 = 19;
                                item.dia11 = 19;
                                item.dia12 = 19;
                                item.dia13 = 19;
                                item.dia14 = 19;
                                item.dia15 = 19;
                                item.dia16 = 19;
                            }
                            else
                            {
                                if (item.fechaAlta.Date < busq.fechaInicio.Date)
                                {
                                    item.dia1 = 0;
                                    item.dia2 = 0;
                                    item.dia3 = 0;
                                    item.dia4 = 0;
                                    item.dia5 = 0;
                                    item.dia6 = 0;
                                    item.dia7 = 0;
                                    item.dia8 = 0;
                                    item.dia9 = 0;
                                    item.dia10 = 0;
                                    item.dia11 = 0;
                                    item.dia12 = 0;
                                    item.dia13 = 0;
                                    item.dia14 = 0;
                                    item.dia15 = 0;
                                    item.dia16 = 0;
                                }
                            }
                            #endregion

                            item.estatus = false;

                            det.Add(item);
                        }

                    }
                    if (cplan.estatus != "A")
                    {
                        var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                        if (bonos != null)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in det)
                            {
                                var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                if (hasBono != null && i.evaluacion_detID == 0)
                                {
                                    i.bonoDM = hasBono.monto_Asig;
                                    i.evaluacion_detID = hasBono.id;
                                }
                            }
                        }
                        var bonosCuadrado = _context.tblRH_BN_Plantilla_Cuadrado_Det.OrderByDescending(e => e.plantilla.fechaInicio).Where(x => x.plantilla.cc == busq.cc && x.tipoNominaCve == busq.tipoNomina).ToList();
                        if (bonosCuadrado.Count > 0)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in det)
                            {
                                var hasBono = bonosCuadrado.FirstOrDefault(x => x.empleado == i.clave_empleado);
                                if (hasBono != null)
                                {
                                    i.bonoCuadrado = hasBono.monto;
                                }
                            }
                        }
                    }
                }
                else {
                    tblRH_BN_Incidencia_det_Peru datoPeru = new tblRH_BN_Incidencia_det_Peru();
                    datoPeru.dia1 = 8;
                    datoPeru.dia2 = 8;
                    datoPeru.dia3 = 8;
                    datoPeru.dia4 = 8;
                    datoPeru.dia5 = 8;
                    datoPeru.dia6 = 8;
                    datoPeru.dia7 = 8;
                    datoPeru.dia8 = 8;
                    datoPeru.dia9 = 8;
                    datoPeru.dia10 = 8;
                    datoPeru.dia11 = 8;
                    datoPeru.dia12 = 8;
                    datoPeru.dia13 = 8;
                    datoPeru.dia14 = 8;
                    datoPeru.dia15 = 8;
                    datoPeru.dia16 = 8;
                    datoPeru.registroActivo = true;
                    datoPeru.incidencia_detID = 0;
                    datoPeru.incidenciaID = 0;                    
                    var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidencias_det_EnkEmptyNew(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                    //var det_empty = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det_empty);

                    var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = odbc_det_empty.consulta,
                        parametros = new
                        {
                            anio = busq.anio,
                            periodo = busq.periodo,
                            fechaBajaInicio = busq.fechaInicio,
                            fechaBajaFin = busq.fechaFin,
                            cc_contable = busq.cc,
                            tipo_nomina = busq.tipoNomina,
                            clave_depto = busq.depto
                        }
                    }).ToList();

                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                    var bonosCuadrado = _context.tblRH_BN_Plantilla_Cuadrado_Det.OrderByDescending(e => e.plantilla.fechaInicio).Where(x => x.plantilla.cc == busq.cc && x.tipoNominaCve == busq.tipoNomina).ToList();

                    var data = det_empty.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                    foreach (var x in data)
                    {
                        datoPeru.clave_empleado = x.clave_empleado;
                        datosPeru.Add(datoPeru);
                        var clave_empleadoString = x.clave_empleado.ToString();
                        var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == x.clave_empleado);
                        var objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == x.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                        var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                        var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                        var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                            && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();
                        List<int> diasVacaciones = new List<int>();
                        List<int> diasVacacionesTipo = new List<int>();

                        foreach (var itemVacaciones in vacacionesEmpleado)
                        {
                            var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                            foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado) 
                            {
                                if (itemVacacionesDetalle.fecha.Value.Date >= busq.fechaInicio.Date && itemVacacionesDetalle.fecha.Value.Date <= busq.fechaFin.Date) 
                                {
                                    TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                    int diaVacaciones = difFechasVacaciones.Days + 1;
                                    diasVacaciones.Add(diaVacaciones);
                                    diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                                }
                            }
                        }
                        
                        //var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado).ToList();
                        var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                        && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                        ).ToList();

                        List<int> diasIncapacidades = new List<int>();
                        foreach (var itemIncapacidad in incapacidadesEmpleado) 
                        {
                            TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                            TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                            int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                            int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                            for(int i = diasIncapacidadInicio; i <= (diasIncapacidadFin); i++) diasIncapacidades.Add(i);
                        }

                        if (x.fechaAlta > busq.fechaInicio && !x.isBaja)
                        {
                            TimeSpan difFechas = x.fechaAlta - busq.fechaInicio;
                            int dias = difFechas.Days;

                            if (dias >= 1)
                            {
                                x.dia1 = 13;
                                if(diasIncapacidades.Contains(1)) x.dia1 = 10;
                            }
                            if (dias >= 2)
                            {
                                x.dia2 = 13;
                                if(diasIncapacidades.Contains(2)) x.dia2 = 10;
                            }
                            if (dias >= 3)
                            {
                                x.dia3 = 13;
                                if(diasIncapacidades.Contains(3)) x.dia3 = 10;
                            }
                            if (dias >= 4)
                            {
                                x.dia4 = 13;
                                if(diasIncapacidades.Contains(4)) x.dia4 = 10;
                            }
                            if (dias >= 5)
                            {
                                x.dia5 = 13;
                                if(diasIncapacidades.Contains(5)) x.dia5 = 10;
                            }
                            if (dias >= 6)
                            {
                                x.dia6 = 13;
                                if(diasIncapacidades.Contains(6)) x.dia6 = 10;
                            }
                            if (dias >= 7)
                            {
                                x.dia7 = 13;
                                if(diasIncapacidades.Contains(7)) x.dia7 = 10;
                            }
                            if (dias >= 8)
                            {
                                x.dia8 = 13;
                                if(diasIncapacidades.Contains(8)) x.dia8 = 10;
                            }
                            if (dias >= 9)
                            {
                                x.dia9 = 13;
                                if(diasIncapacidades.Contains(9)) x.dia9 = 10;
                            }
                            if (dias >= 10)
                            {
                                x.dia10 = 13;
                                if(diasIncapacidades.Contains(10)) x.dia10 = 10;
                            }
                            if (dias >= 11)
                            {
                                x.dia11 = 13;
                                if(diasIncapacidades.Contains(11)) x.dia11 = 10;
                            }
                            if (dias >= 12)
                            {
                                x.dia12 = 13;
                                if(diasIncapacidades.Contains(12)) x.dia12 = 10;
                            }
                            if (dias >= 13)
                            {
                                x.dia13 = 13;
                                if(diasIncapacidades.Contains(13)) x.dia13 = 10;
                            }
                            if (dias >= 14)
                            {
                                x.dia14 = 13;
                                if(diasIncapacidades.Contains(14)) x.dia14 = 10;
                            }
                            if (dias >= 15)
                            {
                                x.dia15 = 13;
                                if(diasIncapacidades.Contains(15)) x.dia15 = 10;
                            }
                            if (dias >= 16)
                            {
                                x.dia16 = 13;
                                if(diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                        }
                        else if (x.isBaja && x.fechaAlta <= busq.fechaInicio)
                        {
                            TimeSpan difFechas = x.fechaBaja - busq.fechaInicio;
                            int diasBaja = difFechas.Days;
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if(diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if(diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if(diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if(diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if(diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if(diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if(diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if(diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if(diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if(diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if(diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if(diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if(diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if(diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if(diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if(diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            else
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                        }
                        else if ((x.fechaAlta > busq.fechaInicio && x.isBaja))
                        {
                            TimeSpan difFechasAlta = x.fechaAlta - busq.fechaInicio;
                            TimeSpan difFechasActivo = x.fechaBaja - x.fechaAlta;
                            int diasAlta = difFechasAlta.Days;
                            int diasActivo = difFechasActivo.Days;
                            int diasBaja = diasAlta + diasActivo;
                            #region Dias No aplica
                            if (diasAlta >= 1)
                            {
                                x.dia1 = 13;
                            }
                            if (diasAlta >= 2)
                            {
                                x.dia2 = 13;
                            }
                            if (diasAlta >= 3)
                            {
                                x.dia3 = 13;
                            }
                            if (diasAlta >= 4)
                            {
                                x.dia4 = 13;
                            }
                            if (diasAlta >= 5)
                            {
                                x.dia5 = 13;
                            }
                            if (diasAlta >= 6)
                            {
                                x.dia6 = 13;
                            }
                            if (diasAlta >= 7)
                            {
                                x.dia7 = 13;
                            }
                            if (diasAlta >= 8)
                            {
                                x.dia8 = 13;
                            }
                            if (diasAlta >= 9)
                            {
                                x.dia9 = 13;
                            }
                            if (diasAlta >= 10)
                            {
                                x.dia10 = 13;
                            }
                            if (diasAlta >= 11)
                            {
                                x.dia11 = 13;
                            }
                            if (diasAlta >= 12)
                            {
                                x.dia12 = 13;
                            }
                            if (diasAlta >= 13)
                            {
                                x.dia13 = 13;
                            }
                            if (diasAlta >= 14)
                            {
                                x.dia14 = 13;
                            }
                            if (diasAlta >= 15)
                            {
                                x.dia15 = 13;
                            }
                            if (diasAlta >= 16)
                            {
                                x.dia16 = 13;
                            }
                            #endregion
                            #region Dias Baja
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            else
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        x.primaDominical = false;
                        x.dias_extra_concepto = 0;

                        #region BONO GUARDADO

                        var bonosGuardados = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo)
                                .Where(e => e.aplicado && e.anio == busq.anio && e.cc == busq.cc && e.tipoNomina == busq.tipoNomina && e.estatus == (int)authEstadoEnum.Autorizado).ToList();
                        var idsIncidenciaAplicado = bonosGuardados.Where(e => e.idIncidencia.HasValue).Select(e => e.idIncidencia).ToList();

                        if (idsIncidenciaAplicado.Contains(cplan.id))
                        {
                            foreach (var itemBono in bonosGuardados)
                            {
                                var idsDetallesBonos = itemBono.listDetalle.Select(e => e.cve_Emp).ToList();

                                if (idsDetallesBonos.Contains(x.clave_empleado))
                                {
                                    var objBonoDet = itemBono.listDetalle.FirstOrDefault(e => e.cve_Emp == x.clave_empleado);
                                    x.bonoDM = objBonoDet.monto_Asig;
                                    x.evaluacion_detID = objBonoDet.id;
                                }
                            }
                        }

                        if (bonosCuadrado.Count > 0)
                        {
                            var hasBono = bonosCuadrado.FirstOrDefault(y => y.empleado == x.clave_empleado);
                            if (hasBono != null)
                            {
                                x.bonoCuadrado = hasBono.monto;
                            }
                        }
                        #endregion
                    }

                    if (cplan.estatus != "A") 
                    {
                        if (bonos != null)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in data)
                            {
                                var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                if (hasBono != null && i.evaluacion_detID == 0)
                                {
                                    i.bonoDM = hasBono.monto_Asig;
                                    i.evaluacion_detID = hasBono.id;
                                }
                            }
                        }
                        if (bonosCuadrado.Count > 0)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in data)
                            {
                                var hasBono = bonosCuadrado.FirstOrDefault(x => x.empleado == i.clave_empleado);
                                if (hasBono != null)
                                {
                                    i.bonoCuadrado = hasBono.monto;
                                }
                            }
                        }
                    }
                    det = data;
                    det.ForEach(x => x.estatus = false);
                    result.incidencia_det = det;
                }
                result.incidencia = cplan;
                result.incidencia_det = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                result.isAuth = permiso;
                var isDesauth = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.cc.Equals("*") && x.autoriza);
                result.isDesauth = isDesauth;
                result.permiso_bono_sinlimite = permiso_bono_sinlimite;                
                #endregion
            }
            else
            {
                #region Enkontrol
                //var odbc_main = new OdbcConsultaDTO() { consulta = queryLstIncidenciasEnk(), parametros = paramLstIncidenciasEnk(busq) };
                //var main = _contextEnkontrol.Select<tblRH_BN_Incidencia>(EnkontrolAmbienteEnum.Rh, odbc_main);
                //if (main.Count() > 0)
                //{
                //    var odbc_det = new OdbcConsultaDTO()
                //    {
                //        consulta = queryLstIncidencias_det_Enk(),
                //        parametros = paramLstIncidencias_det_Enk(busq)
                //    };
                //    var det = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det);
                //    var claves = det.Select(x => x.clave_empleado).ToList();
                //    det.ForEach(x =>
                //        {
                //            x.horas_extras = (x.he_dia1 + x.he_dia2 + x.he_dia3 + x.he_dia4 + x.he_dia5 + x.he_dia6 + x.he_dia7 + x.he_dia8 + x.he_dia9 + x.he_dia10 + x.he_dia11 + x.he_dia12 + x.he_dia13 + x.he_dia14 + x.he_dia15 + x.he_dia16);
                //            x.primaDominical = false;
                //        });
                //    var data = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();

                //    result.incidencia = main.FirstOrDefault();
                //    result.incidencia_det = data;
                //    var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                //    var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                //    result.isAuth = permiso;
                //    result.permiso_bono_sinlimite = permiso_bono_sinlimite;
                //    return result;
                //}
                //else
                //{
                    var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                    var empty = new tblRH_BN_Incidencia(); 
                    empty.id_incidencia = 0;
                    empty.usuarioID = vSesiones.sesionUsuarioDTO.id;
                    empty.anio = busq.anio;
                    empty.periodo = busq.periodo;
                    empty.cc = busq.cc;
                    empty.tipo_nomina = busq.tipoNomina;
                    empty.estatus = "P";
                    empty.estatusDesc = "PENDIENTE";
                    empty.fecha_auto = DateTime.Now;
                    empty.fecha_modifica = DateTime.Now;
                    if (emp != null)
                    {
                        //empty.empleado_modifica = emp.empleado;
                        //empty.nombreEmpMod = getNombreUsuarioEnk(emp.empleado);
                        empty.empleado_modifica = 1;
                        empty.nombreEmpMod = "ADMINISTRADOR";
                    }
                    else
                    {
                        empty.empleado_modifica = 1;
                        empty.nombreEmpMod = "ADMINISTRADOR";
                    }


                    var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidencias_det_EnkEmptyNew(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                    //var det_empty = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det_empty);

                    var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = odbc_det_empty.consulta,
                        parametros = new {
                            anio = busq.anio,
                            periodo = busq.periodo,
                            fechaBajaInicio = busq.fechaInicio,
                            fechaBajaFin = busq.fechaFin,
                            cc_contable = busq.cc,
                            tipo_nomina = busq.tipoNomina,
                            clave_depto = busq.depto
                        }
                    }).ToList();

                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                    var bonosCuadrado = _context.tblRH_BN_Plantilla_Cuadrado_Det.OrderByDescending(e => e.plantilla.fechaInicio).Where(x => x.plantilla.cc == busq.cc && x.tipoNominaCve == busq.tipoNomina).ToList();
                    var data = det_empty.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                    foreach (var x in data)
                    {
                        tblRH_BN_Incidencia_det_Peru datoPeru = new tblRH_BN_Incidencia_det_Peru();
                        datoPeru.dia1 = 8;
                        datoPeru.dia2 = 8;
                        datoPeru.dia3 = 8;
                        datoPeru.dia4 = 8;
                        datoPeru.dia5 = 8;
                        datoPeru.dia6 = 8;
                        datoPeru.dia7 = 8;
                        datoPeru.dia8 = 8;
                        datoPeru.dia9 = 8;
                        datoPeru.dia10 = 8;
                        datoPeru.dia11 = 8;
                        datoPeru.dia12 = 8;
                        datoPeru.dia13 = 8;
                        datoPeru.dia14 = 8;
                        datoPeru.dia15 = 8;
                        datoPeru.dia16 = 8;
                        datoPeru.registroActivo = true;
                        datoPeru.incidencia_detID = 0;
                        datoPeru.incidenciaID = 0;
                        datoPeru.clave_empleado = x.clave_empleado;
                        datosPeru.Add(datoPeru);

                        var clave_empleadoString = x.clave_empleado.ToString();
                        var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == x.clave_empleado);
                        var _objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == x.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").ToList();
                        tblRH_Baja_Registro objBajaEmpleado = null;
                        if (_objBajaEmpleado.Count() > 0 ) objBajaEmpleado = _objBajaEmpleado.OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                        var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                        var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                        var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                            && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();
                        List<int> diasVacaciones = new List<int>();
                        List<int> diasVacacionesTipo = new List<int>();

                        //OBTENER CUAL ES EL PERIODO DONDE SE MOSTRARAN LOS DIAS EXTRATEMPORALES (EN ESTATUS PENDIENTE)
                        var lstIncideciasCC = _context.tblRH_BN_Incidencia.Where(e => e.cc == busq.cc && e.tipo_nomina == busq.tipoNomina && busq.anio == e.anio).OrderByDescending(e => e.periodo).ToList();
                        var objUltimaIncidenciaPendientes = lstIncideciasCC.FirstOrDefault(e => e.estatus == "P");
                        var objUltimaIncidenciaAutorizadas = lstIncideciasCC.FirstOrDefault(e => e.estatus == "A");
                        int añoActual = DateTime.Now.Year;
                        var objPeriodoActual = _context.tblRH_EK_Periodos.Where(e => e.tipo_nomina == busq.tipoNomina && e.year == añoActual).ToList()
                            .FirstOrDefault(e => e.fecha_inicial.Date <= DateTime.Now.Date && e.fecha_final.Date >= DateTime.Now.Date);
                        int periodoAplicanExtratemporales = 0;

                        // SI NO TIENE INCIDENCIAS PENDIENTES SE ASIGNA AL SIGUIENTE
                        if (objUltimaIncidenciaPendientes == null)
                        {
                            var ultimaIncidencia = lstIncideciasCC.FirstOrDefault();

                            if (ultimaIncidencia != null)
                            {
                                if (ultimaIncidencia.periodo < objPeriodoActual.periodo)
                                {
                                    periodoAplicanExtratemporales = objPeriodoActual.periodo;

                                }
                                else
                                {
                                    periodoAplicanExtratemporales = ultimaIncidencia.periodo + 1;

                                }
                            }
                            else
                            {
                                periodoAplicanExtratemporales = objPeriodoActual.periodo;
                            }

                        }
                        else
                        {

                            // SI LA ULTIMA INCIDENCIA PENDIENTE NO ES LA UNICA SE LE ASIGNA A LA DEL PERIODO ACTUAL
                            if (objUltimaIncidenciaAutorizadas != null)
                            {
                                periodoAplicanExtratemporales = objUltimaIncidenciaAutorizadas.periodo + 1;

                            }
                            else
                            {
                                periodoAplicanExtratemporales = objPeriodoActual.periodo;

                            }
                        }

                        int numDiasExtratemporales = 0;
                        int numDiasExtratemporalesARestar = 0;
                        var lstFechasExtra = new List<VacFechasDTO>();
                        
                        foreach (var itemVacaciones in vacacionesEmpleado)
                        {
                            #region DESC TIPO VACACIONES
                            string descMotivo = "";

                            switch (itemVacaciones.tipoVacaciones)
                            {
                                case 0:
                                    descMotivo = "Permiso paternidad";
                                    break;
                                case 1:
                                    descMotivo = "Permiso de matrimonio";
                                    break;
                                case 2:
                                    descMotivo = "Permiso sindical";
                                    break;
                                case 3:
                                    descMotivo = "Permiso por fallecimiento";
                                    break;
                                case 5:
                                    descMotivo = "Permiso médico";
                                    break;
                                case 7:
                                    descMotivo = "Vacaciones";
                                    break;
                                case 8:
                                    descMotivo = "Permiso SIN goce de sueldo";
                                    break;
                                case 9:
                                    descMotivo = "Permiso de comision de trabajo";
                                    break;
                                case 10:
                                    descMotivo = ">Home office";
                                    break;
                                case 11:
                                    descMotivo = ">Tiempo x tiempo";
                                    break;
                                case 12:
                                    descMotivo = "Incapacidades";
                                    break;
                                case 13:
                                    descMotivo = "Suspención (SUSP)";
                                    break;
                                default:
                                    descMotivo = "S/N";
                                    break;
                            }
                            #endregion

                            var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                            foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado)
                            {

                                if (!itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fecha < busq.fechaInicio && periodoAplicanExtratemporales == busq.periodo)
                                {
                                    numDiasExtratemporales++;

                                    //PERMISO SIN GOSE 
                                    if (itemVacacionesDetalle.tipoInsidencia == 3)
                                    {
                                        numDiasExtratemporalesARestar++;
                                    }

                                    var objVacFecha = new VacFechasDTO();
                                    objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                    objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                    objVacFecha.descTipoVacaciones = descMotivo;
                                    lstFechasExtra.Add(objVacFecha);
                                }

                                if (itemVacacionesDetalle.fecha >= busq.fechaInicio && itemVacacionesDetalle.fecha <= busq.fechaFin)
                                {
                                    TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                    int diaVacaciones = difFechasVacaciones.Days + 1;
                                    diasVacaciones.Add(diaVacaciones);
                                    diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                                }
                            }
                        }


                        //var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado).ToList();
                        var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                            ).ToList();
                        List<int> diasIncapacidades = new List<int>();
                        foreach (var itemIncapacidad in incapacidadesEmpleado) 
                        {
                            TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                            TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                            int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                            int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                            for (int i = diasIncapacidadInicio; i <= (diasIncapacidadFin); i++) diasIncapacidades.Add(i);

                            //CHECAR SI ALGUNA DE LAS INCAPACIDADES NO FUERON APLICADAS EN LAS INCIDENCIAS
                            DateTime tempFechaInicial = itemIncapacidad.fechaInicio.Date;
                            while (tempFechaInicial <= itemIncapacidad.fechaTerminacion.Date)
                            {
                                if (!itemIncapacidad.esAplicadaIncidencias && tempFechaInicial < busq.fechaInicio && periodoAplicanExtratemporales == busq.periodo)
                                {
                                    numDiasExtratemporales++;
                                    numDiasExtratemporalesARestar++; //INCAPACIDADES

                                    var objVacFecha = new VacFechasDTO();
                                    objVacFecha.fecha = tempFechaInicial;
                                    objVacFecha.tipoVacaciones = 12;
                                    objVacFecha.descTipoVacaciones = "Incapacidades";
                                    lstFechasExtra.Add(objVacFecha);
                                }

                                tempFechaInicial = tempFechaInicial.AddDays(1);
                            }
                        }

                        //DIAS EXTRA TEMPORALES
                        x.numDiasExtratemporales = numDiasExtratemporales;
                        x.numDiasExtratemporalesARestar = numDiasExtratemporalesARestar;
                        x.lstFechasExtratemporaneas = lstFechasExtra;
                        
                        if (x.fechaAlta > busq.fechaInicio && !x.isBaja)
                        {
                            TimeSpan difFechas = x.fechaAlta - busq.fechaInicio;
                            int dias = difFechas.Days;

                            if (dias >= 1)
                            {
                                x.dia1 = 13;
                            }
                            if (dias >= 2)
                            {
                                x.dia2 = 13;
                            }
                            if (dias >= 3)
                            {
                                x.dia3 = 13;
                            }
                            if (dias >= 4)
                            {
                                x.dia4 = 13;
                            }
                            if (dias >= 5)
                            {
                                x.dia5 = 13;
                            }
                            if (dias >= 6)
                            {
                                x.dia6 = 13;
                            }
                            if (dias >= 7)
                            {
                                x.dia7 = 13;
                            }
                            if (dias >= 8)
                            {
                                x.dia8 = 13;
                            }
                            if (dias >= 9)
                            {
                                x.dia9 = 13;
                            }
                            if (dias >= 10)
                            {
                                x.dia10 = 13;
                            }
                            if (dias >= 11)
                            {
                                x.dia11 = 13;
                            }
                            if (dias >= 12)
                            {
                                x.dia12 = 13;
                            }
                            if (dias >= 13)
                            {
                                x.dia13 = 13;
                            }
                            if (dias >= 14)
                            {
                                x.dia14 = 13;
                            }
                            if (dias >= 15)
                            {
                                x.dia15 = 13;
                            }
                            if (dias >= 16)
                            {
                                x.dia16 = 13;
                            }
                        }
                        else if (x.isBaja && x.fechaAlta <= busq.fechaInicio)
                        {                            
                            TimeSpan difFechas = x.fechaBaja - busq.fechaInicio;
                            int diasBaja = difFechas.Days;
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            else
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                        }
                        else if ((x.fechaAlta > busq.fechaInicio && x.isBaja))
                        {
                            TimeSpan difFechasAlta = x.fechaAlta - busq.fechaInicio;
                            TimeSpan difFechasActivo = x.fechaBaja - x.fechaAlta;
                            int diasAlta = difFechasAlta.Days;
                            int diasActivo = difFechasActivo.Days;
                            int diasBaja = diasAlta + diasActivo;
                            #region Dias No aplica
                            if (diasAlta >= 1)
                            {
                                x.dia1 = 13;
                            }
                            if (diasAlta >= 2)
                            {
                                x.dia2 = 13;
                            }
                            if (diasAlta >= 3)
                            {
                                x.dia3 = 13;
                            }
                            if (diasAlta >= 4)
                            {
                                x.dia4 = 13;
                            }
                            if (diasAlta >= 5)
                            {
                                x.dia5 = 13;
                            }
                            if (diasAlta >= 6)
                            {
                                x.dia6 = 13;
                            }
                            if (diasAlta >= 7)
                            {
                                x.dia7 = 13;
                            }
                            if (diasAlta >= 8)
                            {
                                x.dia8 = 13;
                            }
                            if (diasAlta >= 9)
                            {
                                x.dia9 = 13;
                            }
                            if (diasAlta >= 10)
                            {
                                x.dia10 = 13;
                            }
                            if (diasAlta >= 11)
                            {
                                x.dia11 = 13;
                            }
                            if (diasAlta >= 12)
                            {
                                x.dia12 = 13;
                            }
                            if (diasAlta >= 13)
                            {
                                x.dia13 = 13;
                            }
                            if (diasAlta >= 14)
                            {
                                x.dia14 = 13;
                            }
                            if (diasAlta >= 15)
                            {
                                x.dia15 = 13;
                            }
                            if (diasAlta >= 16)
                            {
                                x.dia16 = 13;
                            }
                            #endregion
                            #region Dias Baja
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 20;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 20;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 20;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 20;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 20;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 20;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 20;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 20;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 20;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 20;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 20;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 20;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 20;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 20;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 20;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 20;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        x.primaDominical = false;
                        x.dias_extra_concepto = 0;
                    }

                    if (bonos != null)
                    {
                        empty.evaluacionID = bonos.id;
                        foreach (var i in data)
                        {
                            var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                            if (hasBono != null)
                            {
                                i.bonoDM = hasBono.monto_Asig;
                                i.evaluacion_detID = hasBono.id;
                            }
                        }
                    }
                    if (bonosCuadrado.Count > 0)
                    {
                        //empty.evaluacionID = bonos.id;
                        foreach (var i in data)
                        {
                            var hasBono = bonosCuadrado.FirstOrDefault(x => x.empleado == i.clave_empleado);
                            if (hasBono != null)
                            {
                                i.bonoCuadrado = hasBono.monto;
                            }
                        }
                    }
                    
                    result.incidencia = empty;
                    result.incidencia_det = data;

                    //var id_incidencia = GuardarIncidenciaSIGOPLAN_ENKONTROL(busq, result.incidencia, result.incidencia_det);

                    //result.incidencia.id_incidencia = id_incidencia;
                    //result.incidencia_det.ForEach(x=>x.id_incidencia = id_incidencia);

                    //var cplanNew = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && x.periodo == busq.periodo);
                    //var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == cplanNew.id && x.clave_depto == busq.depto).ToList();

                    //result.incidencia = cplanNew;
                    //result.incidencia_det = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                    var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                    var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                    result.isAuth = permiso;
                    result.permiso_bono_sinlimite = permiso_bono_sinlimite;
                    result.incidencia_det_Peru = datosPeru;
                    return result;
                //}
                
                #endregion
            }
            result.incidencia_det_Peru = datosPeru;
            return result;
        }
        public void getIncidencia_det_new(int incidenciaID, BusqIncidenciaDTO busq)
        {
            //var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidencias_det_EnkEmptyNew(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
            //var det_empty = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det_empty);

            var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidencias_det_EnkEmptyNew(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
            var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = odbc_det_empty.consulta,
                parametros = new
                {
                    anio = busq.anio,
                    periodo = busq.periodo,
                    fechaBajaInicio = busq.fechaInicio,
                    fechaBajaFin = busq.fechaFin,
                    cc_contable = busq.cc,
                    tipo_nomina = busq.tipoNomina,
                    clave_depto = busq.depto
                }
            }).ToList();     
            
            var data = det_empty.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
            //var a = data.FirstOrDefault(x=>x.clave_empleado==23001);
            foreach (var x in data)
            {
                if (x.fechaAlta > busq.fechaInicio && !x.isBaja)
                {
                    TimeSpan difFechas = x.fechaAlta - busq.fechaInicio;
                    int dias = difFechas.Days;

                    if (dias >= 1)
                    {
                        x.dia1 = 13;
                    }
                    if (dias >= 2)
                    {
                        x.dia2 = 13;
                    }
                    if (dias >= 3)
                    {
                        x.dia3 = 13;
                    }
                    if (dias >= 4)
                    {
                        x.dia4 = 13;
                    }
                    if (dias >= 5)
                    {
                        x.dia5 = 13;
                    }
                    if (dias >= 6)
                    {
                        x.dia6 = 13;
                    }
                    if (dias >= 7)
                    {
                        x.dia7 = 13;
                    }
                    if (dias >= 8)
                    {
                        x.dia8 = 13;
                    }
                    if (dias >= 9)
                    {
                        x.dia9 = 13;
                    }
                    if (dias >= 10)
                    {
                        x.dia10 = 13;
                    }
                    if (dias >= 11)
                    {
                        x.dia11 = 13;
                    }
                    if (dias >= 12)
                    {
                        x.dia12 = 13;
                    }
                    if (dias >= 13)
                    {
                        x.dia13 = 13;
                    }
                    if (dias >= 14)
                    {
                        x.dia14 = 13;
                    }
                    if (dias >= 15)
                    {
                        x.dia15 = 13;
                    }
                    if (dias >= 16)
                    {
                        x.dia16 = 13;
                    }
                }
                else if (x.isBaja && x.fechaAlta <= busq.fechaInicio)
                {
                    TimeSpan difFechas = x.fechaBaja - busq.fechaInicio;
                    int dias = difFechas.Days;
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        x.dia1 = 18;
                        x.dia2 = 18;
                        x.dia3 = 18;
                        x.dia4 = 18;
                        x.dia5 = 18;
                        x.dia6 = 18;
                        x.dia7 = 18;
                        x.dia8 = 18;
                        x.dia9 = 18;
                        x.dia10 = 18;
                        x.dia11 = 18;
                        x.dia12 = 18;
                        x.dia13 = 18;
                        x.dia14 = 18;
                        x.dia15 = 18;
                        x.dia16 = 18;
                    }
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        x.dia1 = 16;
                        x.dia2 = 16;
                        x.dia3 = 16;
                        x.dia4 = 16;
                        x.dia5 = 16;
                        x.dia6 = 16;
                        x.dia7 = 16;
                        x.dia8 = 16;
                        x.dia9 = 16;
                        x.dia10 = 16;
                        x.dia11 = 16;
                        x.dia12 = 16;
                        x.dia13 = 16;
                        x.dia14 = 16;
                        x.dia15 = 16;
                        x.dia16 = 16;
                    }
                    else
                    {
                        x.dia1 = 18;
                        x.dia2 = 18;
                        x.dia3 = 18;
                        x.dia4 = 18;
                        x.dia5 = 18;
                        x.dia6 = 18;
                        x.dia7 = 18;
                        x.dia8 = 18;
                        x.dia9 = 18;
                        x.dia10 = 18;
                        x.dia11 = 18;
                        x.dia12 = 18;
                        x.dia13 = 18;
                        x.dia14 = 18;
                        x.dia15 = 18;
                        x.dia16 = 18;
                    }

                    if (dias >= 1)
                    {
                        x.dia1 = 0;
                    }
                    if (dias >= 2)
                    {
                        x.dia2 = 0;
                    }
                    if (dias >= 3)
                    {
                        x.dia3 = 0;
                    }
                    if (dias >= 4)
                    {
                        x.dia4 = 0;
                    }
                    if (dias >= 5)
                    {
                        x.dia5 = 0;
                    }
                    if (dias >= 6)
                    {
                        x.dia6 = 0;
                    }
                    if (dias >= 7)
                    {
                        x.dia7 = 0;
                    }
                    if (dias >= 8)
                    {
                        x.dia8 = 0;
                    }
                    if (dias >= 9)
                    {
                        x.dia9 = 0;
                    }
                    if (dias >= 10)
                    {
                        x.dia10 = 0;
                    }
                    if (dias >= 11)
                    {
                        x.dia11 = 0;
                    }
                    if (dias >= 12)
                    {
                        x.dia12 = 0;
                    }
                    if (dias >= 13)
                    {
                        x.dia13 = 0;
                    }
                    if (dias >= 14)
                    {
                        x.dia14 = 0;
                    }
                    if (dias >= 15)
                    {
                        x.dia15 = 0;
                    }
                    if (dias >= 16)
                    {
                        x.dia16 = 0;
                    }
                }
                else if ((x.fechaAlta > busq.fechaInicio && x.isBaja))
                {
                    TimeSpan difFechasAlta = x.fechaAlta - busq.fechaInicio;
                    TimeSpan difFechasActivo = x.fechaBaja - x.fechaAlta;
                    int diasAlta = difFechasAlta.Days;
                    int diasActivo = difFechasActivo.Days;
                    int diasBaja = diasAlta + diasActivo;
                    #region Dias No aplica
                    if (diasAlta >= 1)
                    {
                        x.dia1 = 13;
                    }
                    if (diasAlta >= 2)
                    {
                        x.dia2 = 13;
                    }
                    if (diasAlta >= 3)
                    {
                        x.dia3 = 13;
                    }
                    if (diasAlta >= 4)
                    {
                        x.dia4 = 13;
                    }
                    if (diasAlta >= 5)
                    {
                        x.dia5 = 13;
                    }
                    if (diasAlta >= 6)
                    {
                        x.dia6 = 13;
                    }
                    if (diasAlta >= 7)
                    {
                        x.dia7 = 13;
                    }
                    if (diasAlta >= 8)
                    {
                        x.dia8 = 13;
                    }
                    if (diasAlta >= 9)
                    {
                        x.dia9 = 13;
                    }
                    if (diasAlta >= 10)
                    {
                        x.dia10 = 13;
                    }
                    if (diasAlta >= 11)
                    {
                        x.dia11 = 13;
                    }
                    if (diasAlta >= 12)
                    {
                        x.dia12 = 13;
                    }
                    if (diasAlta >= 13)
                    {
                        x.dia13 = 13;
                    }
                    if (diasAlta >= 14)
                    {
                        x.dia14 = 13;
                    }
                    if (diasAlta >= 15)
                    {
                        x.dia15 = 13;
                    }
                    if (diasAlta >= 16)
                    {
                        x.dia16 = 13;
                    }
                    #endregion
                    #region Dias Baja
                    if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        if (diasBaja < 1)
                        {
                            x.dia1 = 18;
                        }
                        if (diasBaja < 2)
                        {
                            x.dia2 = 18;
                        }
                        if (diasBaja < 3)
                        {
                            x.dia3 = 18;
                        }
                        if (diasBaja < 4)
                        {
                            x.dia4 = 18;
                        }
                        if (diasBaja < 5)
                        {
                            x.dia5 = 18;
                        }
                        if (diasBaja < 6)
                        {
                            x.dia6 = 18;
                        }
                        if (diasBaja < 7)
                        {
                            x.dia7 = 18;
                        }
                        if (diasBaja < 8)
                        {
                            x.dia8 = 18;
                        }
                        if (diasBaja < 9)
                        {
                            x.dia9 = 18;
                        }
                        if (diasBaja < 10)
                        {
                            x.dia10 = 18;
                        }
                        if (diasBaja < 11)
                        {
                            x.dia11 = 18;
                        }
                        if (diasBaja < 12)
                        {
                            x.dia12 = 18;
                        }
                        if (diasBaja < 13)
                        {
                            x.dia13 = 18;
                        }
                        if (diasBaja < 14)
                        {
                            x.dia14 = 18;
                        }
                        if (diasBaja < 15)
                        {
                            x.dia15 = 18;
                        }
                        if (diasBaja < 16)
                        {
                            x.dia16 = 18;
                        }
                    }
                    else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                    {
                        if (diasBaja < 1)
                        {
                            x.dia1 = 16;
                        }
                        if (diasBaja < 2)
                        {
                            x.dia2 = 16;
                        }
                        if (diasBaja < 3)
                        {
                            x.dia3 = 16;
                        }
                        if (diasBaja < 4)
                        {
                            x.dia4 = 16;
                        }
                        if (diasBaja < 5)
                        {
                            x.dia5 = 16;
                        }
                        if (diasBaja < 6)
                        {
                            x.dia6 = 16;
                        }
                        if (diasBaja < 7)
                        {
                            x.dia7 = 16;
                        }
                        if (diasBaja < 8)
                        {
                            x.dia8 = 16;
                        }
                        if (diasBaja < 9)
                        {
                            x.dia9 = 16;
                        }
                        if (diasBaja < 10)
                        {
                            x.dia10 = 16;
                        }
                        if (diasBaja < 11)
                        {
                            x.dia11 = 16;
                        }
                        if (diasBaja < 12)
                        {
                            x.dia12 = 16;
                        }
                        if (diasBaja < 13)
                        {
                            x.dia13 = 16;
                        }
                        if (diasBaja < 14)
                        {
                            x.dia14 = 16;
                        }
                        if (diasBaja < 15)
                        {
                            x.dia15 = 16;
                        }
                        if (diasBaja < 16)
                        {
                            x.dia16 = 16;
                        }
                    }
                    else
                    {
                        if (diasBaja < 1)
                        {
                            x.dia1 = 18;
                        }
                        if (diasBaja < 2)
                        {
                            x.dia2 = 18;
                        }
                        if (diasBaja < 3)
                        {
                            x.dia3 = 18;
                        }
                        if (diasBaja < 4)
                        {
                            x.dia4 = 18;
                        }
                        if (diasBaja < 5)
                        {
                            x.dia5 = 18;
                        }
                        if (diasBaja < 6)
                        {
                            x.dia6 = 18;
                        }
                        if (diasBaja < 7)
                        {
                            x.dia7 = 18;
                        }
                        if (diasBaja < 8)
                        {
                            x.dia8 = 18;
                        }
                        if (diasBaja < 9)
                        {
                            x.dia9 = 18;
                        }
                        if (diasBaja < 10)
                        {
                            x.dia10 = 18;
                        }
                        if (diasBaja < 11)
                        {
                            x.dia11 = 18;
                        }
                        if (diasBaja < 12)
                        {
                            x.dia12 = 18;
                        }
                        if (diasBaja < 13)
                        {
                            x.dia13 = 18;
                        }
                        if (diasBaja < 14)
                        {
                            x.dia14 = 18;
                        }
                        if (diasBaja < 15)
                        {
                            x.dia15 = 18;
                        }
                        if (diasBaja < 16)
                        {
                            x.dia16 = 18;
                        }
                    }
                    #endregion
                }
                else
                {
                    x.dia1 = 13;
                    x.dia2 = 13;
                    x.dia3 = 13;
                    x.dia4 = 13;
                    x.dia5 = 13;
                    x.dia6 = 13;
                    x.dia7 = 13;
                    x.dia8 = 13;
                    x.dia9 = 13;
                    x.dia10 = 13;
                    x.dia11 = 13;
                    x.dia12 = 13;
                    x.dia13 = 13;
                    x.dia14 = 13;
                    x.dia15 = 13;
                    x.dia16 = 13;
                }
            }

            var inc = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == incidenciaID);
            var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == incidenciaID ).ToList();
            var empleados = det.Select(x => x.clave_empleado).ToList();
            var empleadosNew = data.Select(x => x.clave_empleado).ToList();
            var detNew = data.Where(x => !empleados.Contains(x.clave_empleado)).ToList();
            var detDel = det.Where(x => !empleadosNew.Contains(x.clave_empleado)).ToList();
            //if (detNew.Count() > 0)
            //{
            //    try
            //    {
            //        var odbcDet = new List<OdbcConsultaDTO>();
            //        detNew.ForEach(empl =>
            //        {
            //            empl.incidenciaID = incidenciaID;
            //            empl.id_incidencia = inc.id_incidencia;
            //            empl.fecha = DateTime.Now;
            //            empl.usuarioID = vSesiones.sesionUsuarioDTO.id;
            //            empl.estatus = false;
            //            empl.primaDominical = false;
            //            empl.dias_extra_concepto = 0;
            //            odbcDet.Add(saveIncidenciaEmpDet(empl));
            //        });
            //        _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);

            //        _context.tblRH_BN_Incidencia_det.AddRange(detNew);
            //        _context.SaveChanges();
            //    }
            //    catch (Exception e) {
            //        var al = e;
            //    }
            //}
            //if (detDel.Count() > 0)
            //{
            //    try
            //    {
            //        var odbcDet = new List<OdbcConsultaDTO>();
            //        detDel.ForEach(empl =>
            //        {
            //            odbcDet.Add(delIncidenciaEmpDet(empl));
            //        });
            //        _contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);

            //        _context.tblRH_BN_Incidencia_det.RemoveRange(detDel);
            //        _context.SaveChanges();
            //    }
            //    catch (Exception e) { }
            //}
            //try
            //{
            //    foreach (var y in det)
            //    {
            //        var depto = data.FirstOrDefault(x => x.clave_empleado == y.clave_empleado);
            //        y.clave_depto = depto.clave_depto;
            //    }
            //    _context.SaveChanges();
            //}
            //catch (Exception e) { }
            #region update bono
            try
            {
                var bonos = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == busq.anio && x.cc == busq.cc && x.periodo == busq.periodo && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado && !x.aplicado);

                if (bonos != null)
                {
                    var det_bono = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == incidenciaID).ToList();
                    var odbcDet_Bono = new List<OdbcConsultaDTO>();
                    try
                    {
                        inc.evaluacionID = bonos.id;
                        //_context.SaveChanges();
                    }
                    catch (Exception e) { }
                    foreach (var i in det_bono)
                    {
                        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                        if (hasBono != null)
                        {
                            i.bonoDM = hasBono.monto_Asig;
                            i.evaluacion_detID = hasBono.id;
                        }
                        var depto = data.FirstOrDefault(x => x.clave_empleado == i.clave_empleado);
                        if (depto != null)
                        {
                            i.clave_depto = depto.clave_depto;
                        }
                        odbcDet_Bono.Add(updateIncidenciaEmpDet_Bono(i));
                    }
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet_Bono);
                    //_context.SaveChanges();
                }
                
            }
            catch (Exception e) { }
            #endregion

        }
        public IncidenciasPaqueteDTO getIncidenciaAuth(int incidenciaID, int anio = 0, int periodo = 0, int tipo_nomina = 0, string cc = "")
        {
            var result = new IncidenciasPaqueteDTO();
            var obj = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == incidenciaID);
            var empleadosCapturados = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == incidenciaID).ToList();
            var empleadosCapturadosID = empleadosCapturados.Select(x => x.clave_empleado).ToList();
            var empleadosPendienteCaptura = new List<tblRH_EK_Empleados>();
            var puestoEmpleados = new List<tblRH_EK_Puestos>();
            var departamentosEmpleados = new List<tblRH_EK_Departamentos>();
            List<tblRH_BN_Incidencia_det> detalles = new List<tblRH_BN_Incidencia_det>();

            bool ev = false;

            if (obj == null) 
            {
                var ccID = cc.Split('-');
                var _cc = ccID[0];
                _cc = _cc.Replace("-", "");
                empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => x.tipo_nomina == tipo_nomina && x.cc_contable == _cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var empleadosPendientesPuestos = empleadosPendienteCaptura.Select(x => x.puesto).ToList();
                puestoEmpleados = puestoEmpleados = _context.tblRH_EK_Puestos.Where(x => empleadosPendientesPuestos.Contains(x.puesto)).ToList();
                var empleadosPendientesDepto = empleadosPendienteCaptura.Select(x => x.clave_depto.ParseInt()).ToList();
                departamentosEmpleados = _context.tblRH_EK_Departamentos.Where(x => empleadosPendientesDepto.Contains(x.clave_depto)).ToList();
                ev = _context.tblRH_BN_Evaluacion.Any(x => x.cc == _cc && x.tipoNomina == tipo_nomina && x.periodo == periodo && x.estatus == (int)authEstadoEnum.EnEspera);
            }
            else
            {
                empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => x.tipo_nomina == obj.tipo_nomina && x.cc_contable == obj.cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var empleadosPendientesPuestos = empleadosPendienteCaptura.Select(x => x.puesto).ToList();
                puestoEmpleados = puestoEmpleados = _context.tblRH_EK_Puestos.Where(x => empleadosPendientesPuestos.Contains(x.puesto)).ToList();
                var empleadosPendientesDepto = empleadosPendienteCaptura.Select(x => x.clave_depto.ParseInt()).ToList();
                departamentosEmpleados = _context.tblRH_EK_Departamentos.Where(x => empleadosPendientesDepto.Contains(x.clave_depto)).ToList();
                ev = _context.tblRH_BN_Evaluacion.Any(x => x.cc == obj.cc && x.tipoNomina == obj.tipo_nomina && x.periodo == obj.periodo && x.estatus == (int)authEstadoEnum.EnEspera);
            }

            detalles =
                empleadosPendienteCaptura.Select(x =>
                {
                    var _puesto = puestoEmpleados.FirstOrDefault(y => y.puesto == x.puesto);
                    var _depto = departamentosEmpleados.FirstOrDefault(y => y.clave_depto == x.clave_depto.ParseInt());
                    return new tblRH_BN_Incidencia_det()
                    {
                        ape_paterno = x.ape_paterno,
                        ape_materno = x.ape_materno,
                        nombre = x.nombre,
                        clave_empleado = x.clave_empleado,
                        puesto = x.puesto ?? 0,
                        puestoDesc = _puesto == null ? "--" : _puesto.descripcion,
                        clave_depto = Int32.Parse(x.clave_depto),
                        deptoDesc = _depto == null ? "--" : _depto.desc_depto,
                        total_Dias = 0,
                        estatus = false
                    };
                }).ToList();
            detalles.AddRange(empleadosCapturados);
            result.incidencia_det = detalles.OrderBy(x => x.estatus).ThenBy(x => x.ape_paterno).ThenBy(x => x.ape_materno).ToList();
            result.evaluacion_pendiente = false;            
            result.evaluacion_pendiente = ev;

            return result;
        }

        Tuple<bool, string> comprobarEmpleados(BusqIncidenciaDTO busq, tblRH_BN_Incidencia obj, List<tblRH_BN_Incidencia_det> det) 
        {
            bool estado = true;
            string mensaje = "";

            var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == obj.anio && x.tipo_nomina == obj.tipo_nomina && x.periodo == obj.periodo && x.cc != obj.cc && x.estatus == "A").ToList();
            var incidenciasID = incidencias.Select(x => x.id).ToList();

            if (incidencias.Count() > 0) 
            {
                var detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                foreach (var item in det) 
                {
                    var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                    //if (auxDetalle != null) 
                    //{
                    //    var auxIncidencia = incidencias.FirstOrDefault(x => x.id == item.incidenciaID);
                    //    Tuple<bool, string> resultError = new Tuple<bool, string>(false, "EL EMPLEADO " + item.nombre + " " + item.ape_paterno + " " + item.ape_materno + " CON CLAVE EMPLEADO " + item.clave_empleado + " YA SE ENCUENTRA EN UNA NOMINA AUTORIZADA PARA EL CC " + auxIncidencia.cc + " Y NO ES POSIBLE REALIZAR EL GUARDADO");
                    //    return resultError;
                    //}
                }
                
            }

            Tuple<bool, string> result = new Tuple<bool, string>(estado, mensaje);
            return result;
        }

        bool eliminarOtrosGuardados(BusqIncidenciaDTO busq, tblRH_BN_Incidencia obj, List<tblRH_BN_Incidencia_det> det)
        {
            var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == obj.anio && x.tipo_nomina == obj.tipo_nomina && x.periodo == obj.periodo && x.cc != obj.cc && x.estatus == "P").ToList();
            var incidenciasID = incidencias.Select(x => x.id).ToList();

            if (incidencias.Count() > 0)
            {
                var detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                foreach (var item in det)
                {
                    var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                    if (auxDetalle != null)
                    {
                        _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);                        
                    }
                }

            }
            _context.SaveChanges();
            return true;
        }

        Tuple<bool, string> GuardarIncidenciaSIGOPLAN_ENKONTROL(BusqIncidenciaDTO busq, tblRH_BN_Incidencia obj, List<tblRH_BN_Incidencia_det> det, List<tblRH_BN_Incidencia_det_Peru> detPeru, List<HttpPostedFileBase> archivos, List<int> lstClaveEmpleados)
        {
            if (vSesiones.sesionEmpresaActual != (int)EmpresaEnum.Peru)
            {
                #region DEMAS EMPRESAS
                //var odbcMain = new OdbcConsultaDTO();
                //var odbcDet = new List<OdbcConsultaDTO>();
                var comprobacion = comprobarEmpleados(busq, obj, det);
                if (!comprobacion.Item1) { return comprobacion; }

                var create = obj.id == 0;
                if (create)
                {
                    //odbcMain = saveIncidenciaEmp(obj);
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);
                    //var inc = getIncidenciaEnkObj(busq);

                    var usuario = vSesiones.sesionUsuarioDTO;

                    obj.anio = busq.anio;
                    obj.cc = busq.cc;
                    obj.empleado_modifica = usuario.id;
                    obj.estatus = "P";
                    obj.estatusDesc = "PENDIENTE";
                    obj.fecha_auto = DateTime.Now;
                    obj.fecha_modifica = DateTime.Now;
                    obj.nombreEmpMod = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                    obj.periodo = busq.periodo;
                    obj.tipo_nomina = busq.tipoNomina;
                    obj.usuarioID = usuario.id;
                    obj.id_incidencia = -1;

                    det.ForEach(empl =>
                    {
                        empl.id_incidencia = 0;
                        empl.fecha = DateTime.Now;
                        empl.usuarioID = vSesiones.sesionUsuarioDTO.id;
                        empl.estatus = true;
                        //odbcDet.Add(saveIncidenciaEmpDet(empl));
                    });

                    if (detPeru != null)
                    {
                        detPeru.ForEach(empl =>
                        {
                            empl.fechaCreacion = DateTime.Now;
                            empl.fechaModificacion = DateTime.Now;
                            empl.usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            empl.usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            empl.registroActivo = true;
                            //odbcDet.Add(saveIncidenciaEmpDet(empl));
                        });
                    }

                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);
                    //obj.id_incidencia = 0;
                    _context.tblRH_BN_Incidencia.Add(obj);
                    _context.SaveChanges();
                    foreach (var x in det)
                    {
                        x.incidenciaID = obj.id;
                        x.id_incidencia = 0;
                        x.horas_extras = 0;
                    }

                    if (detPeru != null)
                    {
                        foreach (var x in detPeru) 
                        { 
                            x.incidenciaID = obj.id;
                            x.usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        }
                        _context.tblRH_BN_Incidencia_det_Peru.AddRange(detPeru);
                        _context.SaveChanges();
                    }

                    _context.tblRH_BN_Incidencia_det.AddRange(det);
                    _context.SaveChanges();

                    //--> Eliminar registros de emplado en otras incidencias
                    var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == obj.anio && x.tipo_nomina == obj.tipo_nomina && x.periodo == obj.periodo && x.cc != obj.cc && x.estatus == "P").ToList();
                    var incidenciasID = incidencias.Select(x => x.id).ToList();
                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).
                        FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);

                    if (bonos != null)
                    {
                        bonos.idIncidencia = obj.id;
                        bonos.aplicado = true;
                        _context.SaveChanges();
                    }

                    if (incidencias.Count() > 0)
                    {
                        var detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                        foreach (var item in det)
                        {
                            var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                            if (auxDetalle != null)
                            {
                                _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                            }
                            else
                            {
                                if (obj.estatus != "A")
                                {
                                    if (bonos != null)
                                    {
                                        //empty.evaluacionID = bonos.id;
                                        foreach (var i in det)
                                        {
                                            var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                            if (hasBono != null)
                                            {
                                                i.bonoDM = hasBono.monto_Asig;
                                                i.evaluacion_detID = hasBono.id;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    _context.SaveChanges();

                    //return 0;
                    Tuple<bool, string> result = new Tuple<bool, string>(true, "");
                    return result;
                }
                else
                {
                    var objN = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == obj.id);
                    var usuario = vSesiones.sesionUsuarioDTO;
                    //var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                    objN.fecha_modifica = DateTime.Now;
                    //if (emp != null)
                    //{
                    //    objN.empleado_modifica = emp.empleado;
                    //    objN.nombreEmpMod = getNombreUsuarioEnk(emp.empleado);
                    //}
                    //else
                    //{
                    //    objN.empleado_modifica = 1;
                    //    objN.nombreEmpMod = "ADMINISTRADOR";
                    //}
                    if (usuario != null)
                    {
                        objN.empleado_modifica = usuario.id;
                        objN.nombreEmpMod = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                    }
                    else
                    {
                        objN.empleado_modifica = 1;
                        objN.nombreEmpMod = "ADMINISTRADOR";
                    }

                    //odbcMain = updateIncidenciaEmp(objN);
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);

                    var detN = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id);
                    det.ForEach(empl =>
                    {
                        empl.fecha = DateTime.Now;
                        empl.usuarioID = vSesiones.sesionUsuarioDTO.id;
                        empl.estatus = true;
                        empl.incidenciaID = obj.id;
                        //odbcDet.Add(updateIncidenciaEmpDet(empl));
                    });

                    if (detPeru != null)
                    {
                        var borrarPeru = _context.tblRH_BN_Incidencia_det_Peru.Where(x => x.incidenciaID == obj.id).ToList();
                        _context.tblRH_BN_Incidencia_det_Peru.RemoveRange(borrarPeru);
                        _context.SaveChanges();
                        foreach (var item in detPeru)
                        {
                            item.incidenciaID = obj.id;
                            item.usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaCreacion = DateTime.Now;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.tblRH_BN_Incidencia_det_Peru.AddRange(detPeru);
                        _context.SaveChanges();
                    }
                    //foreach (var c in detN)
                    //{
                    //    var o = det.FirstOrDefault(x => x.clave_empleado == c.clave_empleado);
                    //    if (o != null)
                    //    {
                    //        c.evaluacion_detID = o.evaluacion_detID;
                    //        c.dia1 = o.dia1;
                    //        c.dia2 = o.dia2;
                    //        c.dia3 = o.dia3;
                    //        c.dia4 = o.dia4;
                    //        c.dia5 = o.dia5;
                    //        c.dia6 = o.dia6;
                    //        c.dia7 = o.dia7;
                    //        c.dia8 = o.dia8;
                    //        c.dia9 = o.dia9;
                    //        c.dia10 = o.dia10;
                    //        c.dia11 = o.dia11;
                    //        c.dia12 = o.dia12;
                    //        c.dia13 = o.dia13;
                    //        c.dia14 = o.dia14;
                    //        c.dia15 = o.dia15;
                    //        c.dia16 = o.dia16;
                    //        c.he_dia1 = o.he_dia1;
                    //        c.total_Dias = o.total_Dias;
                    //        c.totalo_Horas = o.totalo_Horas;
                    //        c.estatus = true;
                    //        c.dias_extras = o.dias_extras;
                    //        c.dias_extra_concepto = o.dias_extra_concepto;
                    //        c.prima_dominical = o.prima_dominical;
                    //        c.observaciones = o.observaciones;
                    //        c.bono_Obs = o.bono_Obs;
                    //        c.fecha = DateTime.Now;
                    //        c.usuarioID = vSesiones.sesionUsuarioDTO.id;
                    //        c.bono = o.bono;
                    //        c.archivo_enviado = o.archivo_enviado;
                    //        c.bonoDM = o.bonoDM;
                    //        c.bonoDM_Obs = o.bonoDM_Obs;
                    //        c.bonoU = o.bonoU;
                    //        c.primaDominical = o.primaDominical;
                    //    }
                    //}
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);

                    var empleadosActualizar = det.Select(x => x.clave_empleado).ToList();
                    var actualizadas = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id && empleadosActualizar.Contains(x.clave_empleado)).ToList();

                    var detalles = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id).ToList();
                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).
                        FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);

                    if (bonos != null && !bonos.aplicado)
                    {
                        bonos.idIncidencia = obj.id;
                        bonos.aplicado = true;
                        _context.SaveChanges();
                    }

                    foreach (var item in det)
                    {
                        var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if (auxDetalle != null)
                        {
                            _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                        }
                        else
                        {
                            if (obj.estatus != "A")
                            {
                                if (bonos != null)
                                {
                                    //empty.evaluacionID = bonos.id;
                                    foreach (var i in det)
                                    {
                                        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                        if (hasBono != null)
                                        {
                                            i.bonoDM = hasBono.monto_Asig;
                                            i.evaluacion_detID = hasBono.id;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _context.tblRH_BN_Incidencia_det.RemoveRange(actualizadas);
                    _context.SaveChanges();
                    _context.tblRH_BN_Incidencia_det.AddRange(det);
                    _context.SaveChanges();

                    //--> Eliminar registros de emplado en otras incidencias
                    var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == objN.anio && x.tipo_nomina == objN.tipo_nomina && x.periodo == objN.periodo && x.cc != objN.cc && x.estatus == "P").ToList();
                    var incidenciasID = incidencias.Select(x => x.id).ToList();

                    if (incidencias.Count() > 0)
                    {
                        detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                        foreach (var item in det)
                        {
                            var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                            if (auxDetalle != null)
                            {
                                _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                            }
                            else
                            {
                                if (obj.estatus != "A")
                                {
                                    if (bonos != null)
                                    {
                                        //empty.evaluacionID = bonos.id;
                                        foreach (var i in det)
                                        {
                                            var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                            if (hasBono != null)
                                            {
                                                i.bonoDM = hasBono.monto_Asig;
                                                i.evaluacion_detID = hasBono.id;
                                                _context.SaveChanges();
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    //return obj.id_incidencia;
                    Tuple<bool, string> result = new Tuple<bool, string>(true, "");
                    return result;
                }
                #endregion
            }
            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Peru)
            {
                #region PERU
                //var odbcMain = new OdbcConsultaDTO();
                //var odbcDet = new List<OdbcConsultaDTO>();
                var comprobacion = comprobarEmpleados(busq, obj, det);
                if (!comprobacion.Item1) { return comprobacion; }

                var create = obj.id == 0;
                if (create)
                {
                    //odbcMain = saveIncidenciaEmp(obj);
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);
                    //var inc = getIncidenciaEnkObj(busq);

                    var usuario = vSesiones.sesionUsuarioDTO;

                    obj.anio = busq.anio;
                    obj.cc = busq.cc;
                    obj.empleado_modifica = usuario.id;
                    obj.estatus = "P";
                    obj.estatusDesc = "PENDIENTE";
                    obj.fecha_auto = DateTime.Now;
                    obj.fecha_modifica = DateTime.Now;
                    obj.nombreEmpMod = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                    obj.periodo = busq.periodo;
                    obj.tipo_nomina = busq.tipoNomina;
                    obj.usuarioID = usuario.id;
                    obj.id_incidencia = -1;

                    det.ForEach(empl =>
                    {
                        empl.id_incidencia = 0;
                        empl.fecha = DateTime.Now;
                        empl.usuarioID = vSesiones.sesionUsuarioDTO.id;
                        empl.estatus = true;
                        //odbcDet.Add(saveIncidenciaEmpDet(empl));
                    });

                    if (detPeru != null)
                    {
                        detPeru.ForEach(empl =>
                        {
                            empl.fechaCreacion = DateTime.Now;
                            empl.fechaModificacion = DateTime.Now;
                            empl.usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            empl.usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            empl.registroActivo = true;
                            //odbcDet.Add(saveIncidenciaEmpDet(empl));
                        });
                    }

                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);
                    //obj.id_incidencia = 0;
                    _context.tblRH_BN_Incidencia.Add(obj);
                    _context.SaveChanges();
                    foreach (var x in det)
                    {
                        x.incidenciaID = obj.id;
                        x.id_incidencia = 0;
                        x.horas_extras = 0;
                    }

                    if (detPeru != null)
                    {
                        foreach (var x in detPeru) { x.incidenciaID = obj.id; }
                        _context.tblRH_BN_Incidencia_det_Peru.AddRange(detPeru);
                        _context.SaveChanges();
                    }
                    _context.tblRH_BN_Incidencia_det.AddRange(det);
                    _context.SaveChanges();

                    #region SE REGISTRA LAS EVIDENCIAS CARGADAS POR CADA EMPLEADO
                    List<tblRH_BN_Incidencia_Evidencias> lstEvidencias = new List<tblRH_BN_Incidencia_Evidencias>();
                    tblRH_BN_Incidencia_Evidencias objEvidencia = new tblRH_BN_Incidencia_Evidencias();
                    for (int i = 0; i < lstClaveEmpleados.Count(); i++)
                    {
                        var CarpetaNueva = Path.Combine(_RUTA_EVIDENCIAS, lstClaveEmpleados[i].ToString());
                        VerificarCarpeta(CarpetaNueva, true);

                        tblRH_BN_Incidencia_det_Peru objDetPeru = detPeru.Where(w => w.clave_empleado == lstClaveEmpleados[i]).FirstOrDefault();
                        objEvidencia = new tblRH_BN_Incidencia_Evidencias();
                        objEvidencia.incidenciaID = objDetPeru.incidenciaID;
                        objEvidencia.incidencia_detID = objDetPeru.id;
                        objEvidencia.clave_empleado = objDetPeru.clave_empleado;

                        string nombreArchivo = SetNombreArchivo(objEvidencia, archivos[i].FileName, i);
                        string rutaArchivo = Path.Combine(CarpetaNueva, nombreArchivo);
                        var listaRutaArchivos = new List<Tuple<HttpPostedFileBase, string>>();
                        listaRutaArchivos.Add(Tuple.Create(archivos[i], rutaArchivo));

                        foreach (var item in listaRutaArchivos)
                        {
                            if (GlobalUtils.SaveHTTPPostedFile(item.Item1, item.Item2) == false) { }
                                //dbContextTransaction.Rollback();
                        }
                        objEvidencia.ruta = rutaArchivo;
                        objEvidencia.nombreArchivo = nombreArchivo;

                        objEvidencia.FK_UsuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                        objEvidencia.fechaCreacion = DateTime.Now;
                        objEvidencia.registroActivo = true;
                        lstEvidencias.Add(objEvidencia);
                    }
                    _context.tblRH_BN_Incidencia_Evidencias.AddRange(lstEvidencias);
                    _context.SaveChanges();
                    #endregion

                    //--> Eliminar registros de emplado en otras incidencias
                    var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == obj.anio && x.tipo_nomina == obj.tipo_nomina && x.periodo == obj.periodo && x.cc != obj.cc && x.estatus == "P").ToList();
                    var incidenciasID = incidencias.Select(x => x.id).ToList();
                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).
                        FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);

                    if (bonos != null)
                    {
                        bonos.aplicado = true;
                        _context.SaveChanges();
                    }

                    if (incidencias.Count() > 0)
                    {
                        var detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                        foreach (var item in det)
                        {
                            var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                            if (auxDetalle != null)
                            {
                                _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                            }
                            else
                            {
                                if (obj.estatus != "A")
                                {
                                    if (bonos != null)
                                    {
                                        //empty.evaluacionID = bonos.id;
                                        foreach (var i in det)
                                        {
                                            var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                            if (hasBono != null)
                                            {
                                                i.bonoDM = hasBono.monto_Asig;
                                                i.evaluacion_detID = hasBono.id;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    _context.SaveChanges();

                    //return 0;
                    Tuple<bool, string> result = new Tuple<bool, string>(true, "");
                    return result;
                }
                else
                {
                    List<tblRH_BN_Incidencia_det_Peru> lstIncidenciasDetPeru = _context.tblRH_BN_Incidencia_det_Peru.Where(w => w.incidenciaID == obj.id).ToList();
                    List<int> lstCveEmpleados = lstIncidenciasDetPeru.Select(s => s.clave_empleado).ToList();
                    lstIncidenciasDetPeru = lstIncidenciasDetPeru.Where(w => !detPeru.Select(s => s.clave_empleado).Contains(w.clave_empleado)).ToList();
                    detPeru.AddRange(lstIncidenciasDetPeru);

                    var objN = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == obj.id);
                    var usuario = vSesiones.sesionUsuarioDTO;
                    //var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                    objN.fecha_modifica = DateTime.Now;
                    //if (emp != null)
                    //{
                    //    objN.empleado_modifica = emp.empleado;
                    //    objN.nombreEmpMod = getNombreUsuarioEnk(emp.empleado);
                    //}
                    //else
                    //{
                    //    objN.empleado_modifica = 1;
                    //    objN.nombreEmpMod = "ADMINISTRADOR";
                    //}
                    if (usuario != null)
                    {
                        objN.empleado_modifica = usuario.id;
                        objN.nombreEmpMod = usuario.nombre + " " + usuario.apellidoPaterno + " " + usuario.apellidoMaterno;
                    }
                    else
                    {
                        objN.empleado_modifica = 1;
                        objN.nombreEmpMod = "ADMINISTRADOR";
                    }

                    //odbcMain = updateIncidenciaEmp(objN);
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);

                    var detN = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id);
                    det.ForEach(empl =>
                    {
                        empl.fecha = DateTime.Now;
                        empl.usuarioID = vSesiones.sesionUsuarioDTO.id;
                        empl.estatus = true;
                        empl.incidenciaID = obj.id;
                        //odbcDet.Add(updateIncidenciaEmpDet(empl));
                    });

                    if (detPeru != null)
                    {
                        var borrarPeru = _context.tblRH_BN_Incidencia_det_Peru.Where(x => x.incidenciaID == obj.id).ToList();
                        _context.tblRH_BN_Incidencia_det_Peru.RemoveRange(borrarPeru);
                        _context.SaveChanges();

                        foreach (var item in detPeru)
                        {
                            item.incidenciaID = obj.id;
                            item.usuarioCreacion = vSesiones.sesionUsuarioDTO.id;
                            item.usuarioModificacion = vSesiones.sesionUsuarioDTO.id;
                            item.fechaCreacion = DateTime.Now;
                            item.fechaModificacion = DateTime.Now;
                        }
                        _context.tblRH_BN_Incidencia_det_Peru.AddRange(detPeru);
                        _context.SaveChanges();
                    }
                    //foreach (var c in detN)
                    //{
                    //    var o = det.FirstOrDefault(x => x.clave_empleado == c.clave_empleado);
                    //    if (o != null)
                    //    {
                    //        c.evaluacion_detID = o.evaluacion_detID;
                    //        c.dia1 = o.dia1;
                    //        c.dia2 = o.dia2;
                    //        c.dia3 = o.dia3;
                    //        c.dia4 = o.dia4;
                    //        c.dia5 = o.dia5;
                    //        c.dia6 = o.dia6;
                    //        c.dia7 = o.dia7;
                    //        c.dia8 = o.dia8;
                    //        c.dia9 = o.dia9;
                    //        c.dia10 = o.dia10;
                    //        c.dia11 = o.dia11;
                    //        c.dia12 = o.dia12;
                    //        c.dia13 = o.dia13;
                    //        c.dia14 = o.dia14;
                    //        c.dia15 = o.dia15;
                    //        c.dia16 = o.dia16;
                    //        c.he_dia1 = o.he_dia1;
                    //        c.total_Dias = o.total_Dias;
                    //        c.totalo_Horas = o.totalo_Horas;
                    //        c.estatus = true;
                    //        c.dias_extras = o.dias_extras;
                    //        c.dias_extra_concepto = o.dias_extra_concepto;
                    //        c.prima_dominical = o.prima_dominical;
                    //        c.observaciones = o.observaciones;
                    //        c.bono_Obs = o.bono_Obs;
                    //        c.fecha = DateTime.Now;
                    //        c.usuarioID = vSesiones.sesionUsuarioDTO.id;
                    //        c.bono = o.bono;
                    //        c.archivo_enviado = o.archivo_enviado;
                    //        c.bonoDM = o.bonoDM;
                    //        c.bonoDM_Obs = o.bonoDM_Obs;
                    //        c.bonoU = o.bonoU;
                    //        c.primaDominical = o.primaDominical;
                    //    }
                    //}
                    //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcDet);

                    var empleadosActualizar = det.Select(x => x.clave_empleado).ToList();
                    var actualizadas = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id && empleadosActualizar.Contains(x.clave_empleado)).ToList();

                    var detalles = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id).ToList();
                    var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).
                        FirstOrDefault(x => !x.aplicado && x.anio == busq.anio && x.cc == busq.cc && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);

                    if (bonos != null)
                    {
                        bonos.aplicado = true;
                        _context.SaveChanges();
                    }

                    foreach (var item in det)
                    {
                        var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if (auxDetalle != null)
                        {
                            _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                        }
                        else
                        {
                            if (obj.estatus != "A")
                            {
                                if (bonos != null)
                                {
                                    //empty.evaluacionID = bonos.id;
                                    foreach (var i in det)
                                    {
                                        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                        if (hasBono != null)
                                        {
                                            i.bonoDM = hasBono.monto_Asig;
                                            i.evaluacion_detID = hasBono.id;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _context.tblRH_BN_Incidencia_det.RemoveRange(actualizadas);
                    _context.SaveChanges();
                    _context.tblRH_BN_Incidencia_det.AddRange(det);
                    _context.SaveChanges();

                    //--> Eliminar registros de emplado en otras incidencias
                    var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.anio == objN.anio && x.tipo_nomina == objN.tipo_nomina && x.periodo == objN.periodo && x.cc != objN.cc && x.estatus == "P").ToList();
                    var incidenciasID = incidencias.Select(x => x.id).ToList();

                    if (incidencias.Count() > 0)
                    {
                        detalles = _context.tblRH_BN_Incidencia_det.Where(x => incidenciasID.Contains(x.incidenciaID)).ToList();
                        foreach (var item in det)
                        {
                            var auxDetalle = detalles.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                            if (auxDetalle != null)
                            {
                                _context.tblRH_BN_Incidencia_det.Remove(auxDetalle);
                            }
                            else
                            {
                                if (obj.estatus != "A")
                                {
                                    if (bonos != null)
                                    {
                                        //empty.evaluacionID = bonos.id;
                                        foreach (var i in det)
                                        {
                                            var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                            if (hasBono != null)
                                            {
                                                i.bonoDM = hasBono.monto_Asig;
                                                i.evaluacion_detID = hasBono.id;
                                                _context.SaveChanges();
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    //return obj.id_incidencia;
                    Tuple<bool, string> result = new Tuple<bool, string>(true, "");
                    return result;
                }
                #endregion
            }
            else
                return null;
        }

        private static bool VerificarCarpeta(string path, bool crear = false)
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

        private string SetNombreArchivo(tblRH_BN_Incidencia_Evidencias objEvidencia, string fileName, int i)
        {
            string nombreArchivo = string.Empty;
            try
            {
                nombreArchivo = string.Format("{0}-{1}-{2}-{3}{4}", objEvidencia.incidenciaID, objEvidencia.incidencia_detID, objEvidencia.clave_empleado, i, Path.GetExtension(fileName));
            }
            catch (Exception e)
            {
                var NOMBRE_METODO = System.Reflection.MethodBase.GetCurrentMethod().Name;
                LogError(_SISTEMA, 0, _NOMBRE_CONTROLADOR, NOMBRE_METODO, e, AccionEnum.CONSULTA, objEvidencia.id, objEvidencia);
            }
            return nombreArchivo;
        }

        int GuardarIncidenciaSIGOPLAN_ENKONTROL_SINCRONIZAR(tblRH_BN_Incidencia obj, List<tblRH_BN_Incidencia_det> det)
        {
            obj.usuarioID = vSesiones.sesionUsuarioDTO.id;
            obj.fecha_auto = DateTime.Now;
            obj.usuario_auto = 0;
            _context.tblRH_BN_Incidencia.Add(obj);


            det.ForEach(x => {
                x.incidenciaID = obj.id;
                x.usuarioID = vSesiones.sesionUsuarioDTO.id;
                x.fecha = DateTime.Now;
                x.estatus = false;
            });
            _context.tblRH_BN_Incidencia_det.AddRange(det);
            _context.SaveChanges();
            return obj.id;
        }
        public Dictionary<string, object> authIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj)
        {
            var resultado = new Dictionary<string, object>();

            var odbcMain = new OdbcConsultaDTO();
            var objN = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == obj.id);
            var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == objN.id).ToList();
            var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            var objPeriodo = _context.tblRH_EK_Periodos.FirstOrDefault(e => e.periodo == objN.periodo && e.tipo_nomina == objN.tipo_nomina && e.year == DateTime.Now.Year);

            #region GUARDAR ANTES DE AUTORIZAR
            try
            {
                var objFiltro = new BusqIncidenciaDTO()
                {
                    cc = objN.cc,
                    tipoNomina = objN.tipo_nomina,
                    anio = objN.anio,
                    periodo = objN.periodo,
                    depto = 0,
                    fechaInicio = objPeriodo.fecha_inicial,
                    fechaFin = objPeriodo.fecha_final,
                };
                var lstIncidencias = getLstIncidenciasEnk(objFiltro);
                var lstConceptos = _context.tblRH_EK_Incidencias_Conceptos.ToList();

                #region CLAVE DE DESCANSO PAGADO POR EMPRESA Y DESCANSO LABORADO
                int claveDescansoPagado = 16;
                int claveDescansoLaborado = 17;
                switch ((MainContextEnum)vSesiones.sesionEmpresaActual)
	            {   
		            case MainContextEnum.Construplan:

                     break;
                    case MainContextEnum.Arrendadora:
                        claveDescansoPagado = 14;
                        claveDescansoLaborado = 15;
                     break;
                    case MainContextEnum.Colombia:
                     break;
                    case MainContextEnum.PERU:
                     break;
                    default:
                     break;
	            }
	            #endregion

                foreach (var item in lstIncidencias.incidencia_det)
                {
                    int totalDias = 0;
                    List<int> lstDias = new List<int>();
                    var fechaPeriodoInicialTemp = objPeriodo.fecha_inicial;

                    #region ASISTENCIAS

                    for (int i = 1; i < 17; i++)
                    {
                        if (objPeriodo.fecha_final >= fechaPeriodoInicialTemp)
                        {
                            #region ADD ASISTENCIAS DENTRO DEL PERIODO
                            switch (i)
                            {
                                case 1:
                                    {
                                        if (item.dia1 != 0)
                                        {
                                            var objConcepto1 = lstConceptos.FirstOrDefault(e => e.id == item.dia1);
                                            totalDias += objConcepto1 != null ? objConcepto1.asistencia : 0;

                                            lstDias.Add(item.dia1);
                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia1 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia1 = 1;
                                                totalDias += 1;
                                            }
                                        }

                                        break;
                                    }
                                case 2:
                                    {
                                        if (item.dia2 != 0)
                                        {
                                            var objConcepto2 = lstConceptos.FirstOrDefault(e => e.id == item.dia2);
                                            totalDias += objConcepto2 != null ? objConcepto2.asistencia : 0;
                                            lstDias.Add(item.dia2);
                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia2 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia2 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;

                                    }

                                case 3:
                                    {
                                        if (item.dia3 != 0)
                                        {
                                            var objConcepto3 = lstConceptos.FirstOrDefault(e => e.id == item.dia3);
                                            totalDias += objConcepto3 != null ? objConcepto3.asistencia : 0;
                                            lstDias.Add(item.dia3);
                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia3 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia3 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;

                                    }

                                case 4:
                                    {
                                        if (item.dia4 != 0)
                                        {
                                            var objConcepto4 = lstConceptos.FirstOrDefault(e => e.id == item.dia4);
                                            totalDias += objConcepto4 != null ? objConcepto4.asistencia : 0;
                                            lstDias.Add(item.dia4);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia4 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia4 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 5:
                                    {
                                        if (item.dia5 != 0)
                                        {
                                            var objConcepto5 = lstConceptos.FirstOrDefault(e => e.id == item.dia5);
                                            totalDias += objConcepto5 != null ? objConcepto5.asistencia : 0;
                                            lstDias.Add(item.dia5);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia5 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia5 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 6:
                                    {
                                        if (item.dia6 != 0)
                                        {
                                            var objConcepto6 = lstConceptos.FirstOrDefault(e => e.id == item.dia6);
                                            totalDias += objConcepto6 != null ? objConcepto6.asistencia : 0;
                                            lstDias.Add(item.dia6);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia6 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia6 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 7:
                                    {
                                        if (item.dia7 != 0)
                                        {
                                            var objConcepto7 = lstConceptos.FirstOrDefault(e => e.id == item.dia7);
                                            totalDias += objConcepto7 != null ? objConcepto7.asistencia : 0;
                                            lstDias.Add(item.dia7);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia7 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia7 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 8:
                                    {
                                        if (item.dia8 != 0)
                                        {
                                            var objConcepto8 = lstConceptos.FirstOrDefault(e => e.id == item.dia8);
                                            totalDias += objConcepto8 != null ? objConcepto8.asistencia : 0;
                                            lstDias.Add(item.dia8);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia8 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia8 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 9:
                                    {
                                        if (item.dia9 != 0)
                                        {
                                            var objConcepto9 = lstConceptos.FirstOrDefault(e => e.id == item.dia9);
                                            totalDias += objConcepto9 != null ? objConcepto9.asistencia : 0;
                                            lstDias.Add(item.dia9);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia9 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia9 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 10:
                                    {
                                        if (item.dia10 != 0)
                                        {
                                            var objConcepto10 = lstConceptos.FirstOrDefault(e => e.id == item.dia10);
                                            totalDias += objConcepto10 != null ? objConcepto10.asistencia : 0;
                                            lstDias.Add(item.dia10);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia10 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia10 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 11:
                                    {
                                        if (item.dia11 != 0)
                                        {
                                            var objConcepto11 = lstConceptos.FirstOrDefault(e => e.id == item.dia11);
                                            totalDias += objConcepto11 != null ? objConcepto11.asistencia : 0;
                                            lstDias.Add(item.dia11);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia11 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia11 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 12:
                                    {
                                        if (item.dia12 != 0)
                                        {
                                            var objConcepto12 = lstConceptos.FirstOrDefault(e => e.id == item.dia12);
                                            totalDias += objConcepto12 != null ? objConcepto12.asistencia : 0;
                                            lstDias.Add(item.dia12);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia12 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia12 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 13:
                                    {
                                        if (item.dia13 != 0)
                                        {
                                            var objConcepto13 = lstConceptos.FirstOrDefault(e => e.id == item.dia13);
                                            totalDias += objConcepto13 != null ? objConcepto13.asistencia : 0;
                                            lstDias.Add(item.dia13);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia13 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia13 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 14:
                                    {
                                        if (item.dia14 != 0)
                                        {
                                            var objConcepto14 = lstConceptos.FirstOrDefault(e => e.id == item.dia14);
                                            totalDias += objConcepto14 != null ? objConcepto14.asistencia : 0;
                                            lstDias.Add(item.dia14);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia14 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia14 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 15:
                                    {
                                        if (item.dia15 != 0)
                                        {
                                            var objConcepto15 = lstConceptos.FirstOrDefault(e => e.id == item.dia15);
                                            totalDias += objConcepto15 != null ? objConcepto15.asistencia : 0;
                                            lstDias.Add(item.dia15);

                                        }
                                        else
                                        {
                                            if (fechaPeriodoInicialTemp.DayOfWeek == 0)
                                            {
                                                item.dia15 = claveDescansoPagado;
                                                totalDias += 1;
                                            }
                                            else
                                            {
                                                item.dia15 = 1;
                                                totalDias += 1;
                                            }
                                        }
                                        break;
                                    }

                                case 16:
                                    {
                                        lstDias.Add(item.dia16);
                                        break;
                                    }

                                default:
                                    break;
                            }
                            #endregion
                        }

                        fechaPeriodoInicialTemp = fechaPeriodoInicialTemp.AddDays(1);
                    }
                    #endregion

                    //SUMAR DIAS EXTRAS Y RESTAR INCAPACIDADES Y PERMISOS SIN GOCE
                    totalDias += item.dias_extras;
                    totalDias -= item.numDiasExtratemporalesARestar;

                    //TOTAL DIAS POR CONCEPTO
                    int totalDiasConcepto = 0;

                    //DIAS FESTIVOS x + n*2
                    int numDiasFestivos = lstDias.Where(e => e == 9).Count();
                    totalDiasConcepto += (numDiasFestivos * 2);

                    //DESCANSOS LABORADOS  x + n
                    int numDescansosLaborados = lstDias.Where(e => e == claveDescansoLaborado).Count();
                    totalDiasConcepto += numDescansosLaborados;

                    //AGREGAR ASISTENCIA DIA FESTIVOS (DESCANSO LABORADOS NO YA SE AGREGA DOBLE) 
                    totalDias += numDiasFestivos;

                    if (totalDias < 0)
                    {
                        
                    }

                    item.total_Dias = totalDias;
                    item.dias_extra_concepto = totalDiasConcepto;
                }

                //GUARDAR INCIDENCIAS 
                GuardarIncidenciaSIGOPLAN_ENKONTROL(objFiltro, objN, lstIncidencias.incidencia_det, new List<tblRH_BN_Incidencia_det_Peru>(), null, null); // TODO
            }
            catch (Exception e)
            {
                
                throw e;
            }
            
            #endregion

            objN.fecha_auto = DateTime.Now;
            objN.estatus = "A";
            objN.estatusDesc = "ACEPTADA";
            if (emp != null)
            {
                //objN.usuario_auto = emp.empleado;
                objN.usuario_auto = 1;
                objN.usuario_autoriza_sigoplan = vSesiones.sesionUsuarioDTO.id;
            }
            else
            {
                objN.usuario_auto = 1;
            }
            //odbcMain = authIncidenciaEmp(objN);
            //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);
            var detalles = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == obj.id).ToList();
            var bonos = _context.tblRH_BN_Evaluacion.OrderByDescending(e => e.periodo).FirstOrDefault(x => !x.aplicado && x.anio == objN.anio && x.cc == objN.cc && x.tipoNomina == objN.tipo_nomina && x.estatus == (int)authEstadoEnum.Autorizado);
            foreach (var item in det)
            {

                #region APLICAR VACACIONES
                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.estado == 1 && e.claveEmpleado == item.clave_empleado.ToString() && e.cc == objN.cc && e.esPagadas.Value != true).ToList();
                var lstIdsVacaciones = lstVacaciones.Select(e => e.id).ToList();

                foreach (var itemVac in lstVacaciones)
                {
                    var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacaciones.Contains(e.vacacionID) && !e.esAplicadaIncidencias).ToList().Where(e => e.fecha.Value.Date <= objPeriodo.fecha_final.Date).ToList();

                    foreach (var itemFechas in lstFechas)
                    {
                        itemFechas.esAplicadaIncidencias = true;
                        itemFechas.idIncidencia = objN.id;
                        itemFechas.fechaAplicadas = DateTime.Now;
                        itemFechas.fechaModificacion = DateTime.Now;
                        itemFechas.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        _context.SaveChanges();
                    }
                }
                #endregion

                #region APLICAR INCAPACIDADES
                var lstIncapacidades = _context.tblRH_Vacaciones_Incapacidades.Where(e => e.esActivo && !e.esAplicadaIncidencias && e.clave_empleado == item.clave_empleado).ToList().Where(e => e.fechaInicio.Date <= objPeriodo.fecha_final.Date).ToList();
                var diasIncapacidades = new List<int>();

                foreach (var itemIncaps in lstIncapacidades)
                {
                    itemIncaps.esAplicadaIncidencias = true;
                    itemIncaps.idIncidencia = objN.id;
                    itemIncaps.fechaAplicadas = DateTime.Now;
                    itemIncaps.fechaModificacion = DateTime.Now;
                    itemIncaps.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();
                }
                #endregion

                if (bonos != null)
                {
                    //empty.evaluacionID = bonos.id;
                    foreach (var i in det)
                    {
                        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                        if (hasBono != null)
                        {
                            i.bonoDM = hasBono.monto_Asig;
                            i.evaluacion_detID = hasBono.id;
                        }
                    }
                    bonos.aplicado = true;
                }
            }

            
            _context.SaveChanges();

            try {

                //if (bonos != null)
                //{
                //    //empty.evaluacionID = bonos.id;
                //    foreach (var i in det)
                //    {
                //        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                //        if (hasBono != null)
                //        {
                //            i.bonoDM = hasBono.monto_Asig;
                //            i.evaluacion_detID = hasBono.id;
                //        }
                //    }
                //}

                //if (objN.evaluacionID != 0)
                //{
                //    var ev = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.id == objN.evaluacionID);
                //    ev.aplicado = true;
                //    ev.fechaAplicacion = DateTime.Now;
                //    _context.SaveChanges();
                //}
                //else {
                //    var ev = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.cc == objN.cc && x.tipoNomina == objN.tipo_nomina && x.periodo == objN.periodo && x.estatus == (int)authEstadoEnum.EnEspera);
                //    if (ev != null)
                //    {
                //        var periodos = getPeriodos(ev.anio,ev.tipoNomina);
                //        var cp = periodos.FirstOrDefault(x => x.periodo == (ev.periodo + 1));
                //        ev.periodo = cp.periodo;
                //        ev.fechaInicio = cp.fecha_inicial;
                //        ev.fechaFin = cp.fecha_final;
                //        _context.SaveChanges();
                //    }
                //}

                resultado.Add(SUCCESS, true);
                resultado.Add(ITEMS, obj.id_incidencia);
                resultado.Add("cc", objN.cc);
                resultado.Add("tipoNomina", objN.tipo_nomina);
                resultado.Add("periodo", objN.periodo);
                resultado.Add("anio", objN.anio);
            }
            catch(Exception e){
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
        public int desAuthIncidenciaSIGOPLAN_ENKONTROL(tblRH_BN_Incidencia obj)
        {
            //var odbcMain = new OdbcConsultaDTO();
            var objN = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == obj.id);
            var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
            var objPeriodo = _context.tblRH_EK_Periodos.FirstOrDefault(e => e.periodo == objN.periodo && e.tipo_nomina == objN.tipo_nomina && e.year == DateTime.Now.Year);

            DateTime fechaOrig = objN.fecha_auto;

            objN.fecha_auto = DateTime.Now;
            objN.estatus = "P";
            objN.estatusDesc = "PENDIENTE";

            var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == objN.id).ToList();

            foreach (var item in det)
            {
                #region APLCIAR VACACIONES

                var lstVacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(e => e.registroActivo && e.estado == 1 && e.claveEmpleado == item.clave_empleado.ToString() && e.cc == objN.cc && e.esPagadas.Value != true).ToList();
                var lstIdsVacaciones = lstVacaciones.Select(e => e.id).ToList();

                foreach (var itemVac in lstVacaciones)
                {
                    var lstFechas = _context.tblRH_Vacaciones_Fechas.Where(e => e.registroActivo && lstIdsVacaciones.Contains(e.vacacionID) && e.esAplicadaIncidencias).ToList().Where(e => e.fechaAplicadas.Value.Date == fechaOrig.Date).ToList();

                    foreach (var itemFechas in lstFechas)
                    {
                        itemFechas.esAplicadaIncidencias = false;
                        itemFechas.fechaAplicadas = DateTime.Now;
                        itemFechas.fechaModificacion = DateTime.Now;
                        itemFechas.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        _context.SaveChanges();
                    }
                }

                #endregion


                #region APLICAR INCAPACIDADES
                //var lstIncapacidades = _context.tblRH_Vacaciones_Incapacidades.Where(e => e.esActivo && e.esAplicadaIncidencias).ToList().Where(e => e.fechaAplicadas.HasValue && e.fechaAplicadas.Value.Date == fechaOrig.Date).ToList();
                var lstIncapacidades = _context.tblRH_Vacaciones_Incapacidades.Where(e => e.esActivo && e.esAplicadaIncidencias && e.clave_empleado == item.clave_empleado ).ToList().Where(e => e.idIncidencia == objN.id).ToList();

                foreach (var itemIncaps in lstIncapacidades)
                {
                    if (itemIncaps.idIncidencia == objN.id)
	                {
		                itemIncaps.esAplicadaIncidencias = false;
                        itemIncaps.fechaAplicadas = DateTime.Now;
                        itemIncaps.fechaModificacion = DateTime.Now;
                        itemIncaps.idUsuarioModificacion = vSesiones.sesionUsuarioDTO.id;

                        _context.SaveChanges();
	                }
                    
                }
                #endregion
            }

            if (emp != null)
            {
                objN.usuario_auto = emp.empleado;
                objN.usuario_auto = 1;
                objN.usuario_autoriza_sigoplan = vSesiones.sesionUsuarioDTO.id;
            }
            else
            {
                objN.usuario_auto = 1;
            }
            //odbcMain = desAuthIncidenciaEmp(objN);
            //_contextEnkontrol.Save(EnkontrolAmbienteEnum.Rh, odbcMain);
            _context.SaveChanges();

            return obj.id_incidencia;
        }

        public int RevisarFechaCierre(List<tblRH_BN_Incidencia> obj)
        {
            //var odbcMain = new OdbcConsultaDTO();
            foreach (var item in obj)
            {
                var objN = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == item.id);
                var periodo = _context.tblRH_BN_EstatusPeriodos.FirstOrDefault(x => x.anio == objN.anio && x.periodo == objN.periodo && x.tipo_nomina == objN.tipo_nomina);
                var fechaActual = DateTime.Now;
                if (periodo != null && objN != null)
                {
                    var fechaCierre = new DateTime(periodo.fecha_limite.Year, periodo.fecha_limite.Month, periodo.fecha_limite.Day, 13, 8, 0);
                    if (fechaActual > fechaCierre)
                    {
                        return 0;
                    }

                    //where e.cc_contable = ? AND e.estatus_empleado = 'A' and tn.tipo_nomina = ?"
                    var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == item.id).ToList();
                    var detID = det.Select(x => x.clave_empleado).ToList();
                    var empleados = _context.tblRH_EK_Empleados.Where(x => x.cc_contable == item.cc && x.estatus_empleado == "A" && x.tipo_nomina == item.tipo_nomina).ToList();
                    var faltantes = empleados.Where(x => !detID.Contains(x.clave_empleado)).ToList();
                    if (faltantes.Count() > 0) return 3; 
                }
                else return 2;
            }
            return 1;

        }

        IncidenciaEmpDTO getIncidenciaEnkObj(BusqIncidenciaDTO busq)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT id_incidencia FROM sn_incidencias_empl WHERE anio = ? and periodo = ? and cc = ? and tipo_nomina = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = busq.anio },
                new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = busq.periodo },
                new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = busq.cc },
                new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina }
            };
            var res = _contextEnkontrol.Select<IncidenciaEmpDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            return res.FirstOrDefault();
        }
        int getNextIncidenciaID()
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT max(id_incidencia) as id_incidencia FROM sn_incidencias_empl";
            var res = _contextEnkontrol.Select<IncidenciaEmpDTO>(EnkontrolAmbienteEnum.Rh, odbc);
            return res.FirstOrDefault().id_incidencia + 1;
        }
        OdbcConsultaDTO saveIncidenciaEmp(tblRH_BN_Incidencia obj)
        {
            var id_incidencia = getNextIncidenciaID();
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"INSERT INTO sn_incidencias_empl
                        (id_incidencia
                        ,anio
                        ,periodo
                        ,cc
                        ,tipo_nomina
                        ,estatus
                        ,empleado_modifica
                        ,fecha_modifica)
                        VALUES (?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = id_incidencia },
                new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = obj.anio },
                new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = obj.periodo },
                new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.VarChar, valor = obj.cc },
                new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = obj.tipo_nomina },
                new OdbcParameterDTO() { nombre = "estatus", tipo = OdbcType.VarChar, valor = obj.estatus },
                new OdbcParameterDTO() { nombre = "empleado_modifica", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_modifica", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                //new OdbcParameterDTO() { nombre = "usuario_auto", tipo = OdbcType.Int, valor =  },
                //new OdbcParameterDTO() { nombre = "fecha_auto", tipo = OdbcType.Date, valor = null },
            };
            return odbc;
        }
        OdbcConsultaDTO updateIncidenciaEmp(tblRH_BN_Incidencia obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl set
                         empleado_modifica = ?
                        ,fecha_modifica = ?
                    WHERE id_incidencia = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "empleado_modifica", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_modifica", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia }
            };
            return odbc;
        }
        OdbcConsultaDTO authIncidenciaEmp(tblRH_BN_Incidencia obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl set
                         usuario_auto = ?
                        ,fecha_auto = ?
                        ,estatus = 'A'
                    WHERE id_incidencia = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "usuario_auto", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_auto", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia }
            };
            return odbc;
        }
        OdbcConsultaDTO desAuthIncidenciaEmp(tblRH_BN_Incidencia obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl set
                         usuario_auto = ?
                        ,fecha_auto = ?
                        ,estatus = 'P'
                    WHERE id_incidencia = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "usuario_auto", tipo = OdbcType.Int, valor = obj.empleado_modifica },
                new OdbcParameterDTO() { nombre = "fecha_auto", tipo = OdbcType.Date, valor = obj.fecha_modifica },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia }
            };
            return odbc;
        }
        OdbcConsultaDTO saveIncidenciaEmpDet(tblRH_BN_Incidencia_det obj)
        {
            obj.observaciones = "";
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"INSERT INTO sn_incidencias_empl_det
                            (id_incidencia ,clave_empleado
                            ,dia1 ,dia2 ,dia3 ,dia4 ,dia5 ,dia6 ,dia7 ,dia8 ,dia9 ,dia10 ,dia11 ,dia12 ,dia13 ,dia14 ,dia15 ,dia16
                            ,he_dia1 ,he_dia2 ,he_dia3 ,he_dia4 ,he_dia5 ,he_dia6 ,he_dia7 ,he_dia8 ,he_dia9 ,he_dia10 ,he_dia11 ,he_dia12 ,he_dia13 ,he_dia14 ,he_dia15 ,he_dia16
                            ,bono ,observaciones ,archivo_enviado ,dias_extras ,prima_dominical)
                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado },
                new OdbcParameterDTO() { nombre = "dia1", tipo = OdbcType.Int, valor = obj.dia1 },
                new OdbcParameterDTO() { nombre = "dia2", tipo = OdbcType.Int, valor = obj.dia2 },
                new OdbcParameterDTO() { nombre = "dia3", tipo = OdbcType.Int, valor = obj.dia3 },
                new OdbcParameterDTO() { nombre = "dia4", tipo = OdbcType.Int, valor = obj.dia4 },
                new OdbcParameterDTO() { nombre = "dia5", tipo = OdbcType.Int, valor = obj.dia5 },
                new OdbcParameterDTO() { nombre = "dia6", tipo = OdbcType.Int, valor = obj.dia6 },
                new OdbcParameterDTO() { nombre = "dia7", tipo = OdbcType.Int, valor = obj.dia7 },
                new OdbcParameterDTO() { nombre = "dia8", tipo = OdbcType.Int, valor = obj.dia8 },
                new OdbcParameterDTO() { nombre = "dia9", tipo = OdbcType.Int, valor = obj.dia9 },
                new OdbcParameterDTO() { nombre = "dia10", tipo = OdbcType.Int, valor = obj.dia10 },
                new OdbcParameterDTO() { nombre = "dia11", tipo = OdbcType.Int, valor = obj.dia11 },
                new OdbcParameterDTO() { nombre = "dia12", tipo = OdbcType.Int, valor = obj.dia12 },
                new OdbcParameterDTO() { nombre = "dia13", tipo = OdbcType.Int, valor = obj.dia13 },
                new OdbcParameterDTO() { nombre = "dia14", tipo = OdbcType.Int, valor = obj.dia14 },
                new OdbcParameterDTO() { nombre = "dia15", tipo = OdbcType.Int, valor = obj.dia15 },
                new OdbcParameterDTO() { nombre = "dia16", tipo = OdbcType.Int, valor = obj.dia16 },
                new OdbcParameterDTO() { nombre = "he_dia1", tipo = OdbcType.Decimal, valor = obj.he_dia1 },
                new OdbcParameterDTO() { nombre = "he_dia2", tipo = OdbcType.Decimal, valor = obj.he_dia2 },
                new OdbcParameterDTO() { nombre = "he_dia3", tipo = OdbcType.Decimal, valor = obj.he_dia3 },
                new OdbcParameterDTO() { nombre = "he_dia4", tipo = OdbcType.Decimal, valor = obj.he_dia4 },
                new OdbcParameterDTO() { nombre = "he_dia5", tipo = OdbcType.Decimal, valor = obj.he_dia5 },
                new OdbcParameterDTO() { nombre = "he_dia6", tipo = OdbcType.Decimal, valor = obj.he_dia6 },
                new OdbcParameterDTO() { nombre = "he_dia7", tipo = OdbcType.Decimal, valor = obj.he_dia7 },
                new OdbcParameterDTO() { nombre = "he_dia8", tipo = OdbcType.Decimal, valor = obj.he_dia8 },
                new OdbcParameterDTO() { nombre = "he_dia9", tipo = OdbcType.Decimal, valor = obj.he_dia9 },
                new OdbcParameterDTO() { nombre = "he_dia10", tipo = OdbcType.Decimal, valor = obj.he_dia10 },
                new OdbcParameterDTO() { nombre = "he_dia11", tipo = OdbcType.Decimal, valor = obj.he_dia11 },
                new OdbcParameterDTO() { nombre = "he_dia12", tipo = OdbcType.Decimal, valor = obj.he_dia12 },
                new OdbcParameterDTO() { nombre = "he_dia13", tipo = OdbcType.Decimal, valor = obj.he_dia13 },
                new OdbcParameterDTO() { nombre = "he_dia14", tipo = OdbcType.Decimal, valor = obj.he_dia14 },
                new OdbcParameterDTO() { nombre = "he_dia15", tipo = OdbcType.Decimal, valor = obj.he_dia15 },
                new OdbcParameterDTO() { nombre = "he_dia16", tipo = OdbcType.Decimal, valor = obj.he_dia16 },
                new OdbcParameterDTO() { nombre = "bono", tipo = OdbcType.Decimal, valor = (obj.bono + obj.bonoDM + obj.bonoU) },
                new OdbcParameterDTO() { nombre = "observaciones", tipo = OdbcType.VarChar, valor = obj.observaciones },
                new OdbcParameterDTO() { nombre = "archivo_enviado", tipo = OdbcType.Int, valor = obj.archivo_enviado },
                new OdbcParameterDTO() { nombre = "dias_extras", tipo = OdbcType.Int, valor = obj.dias_extras },
                new OdbcParameterDTO() { nombre = "prima_dominical", tipo = OdbcType.Decimal, valor = obj.prima_dominical },
            };
            return odbc;
        }
        OdbcConsultaDTO updateIncidenciaEmpDet(tblRH_BN_Incidencia_det obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl_det set
                                dia1 = ? ,
                                dia2 = ? ,
                                dia3 = ? ,
                                dia4 = ? ,
                                dia5 = ? ,
                                dia6 = ? ,
                                dia7 = ? ,
                                dia8 = ? ,
                                dia9 = ? ,
                                dia10 = ? ,
                                dia11 = ? ,
                                dia12 = ? ,
                                dia13 = ? ,
                                dia14 = ? ,
                                dia15 = ? ,
                                dia16 = ? ,
                                he_dia1 = ? ,
                                he_dia2 = ? ,
                                he_dia3 = ? ,
                                he_dia4 = ? ,
                                he_dia5 = ? ,
                                he_dia6 = ? ,
                                he_dia7 = ? ,
                                he_dia8 = ? ,
                                he_dia9 = ? ,
                                he_dia10 = ? , 
                                he_dia11 = ? ,
                                he_dia12 = ? ,
                                he_dia13 = ? ,
                                he_dia14 = ? ,
                                he_dia15 = ? ,
                                he_dia16 = ? ,
                                bono = ? ,
                                observaciones = ? ,
                                archivo_enviado = ? ,
                                dias_extras = ? ,
                                prima_dominical = ?
                            WHERE id_incidencia = ? AND clave_empleado = ?";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                 new OdbcParameterDTO() { nombre = "dia1", tipo = OdbcType.Int, valor = obj.dia1 },
                new OdbcParameterDTO() { nombre = "dia2", tipo = OdbcType.Int, valor = obj.dia2 },
                new OdbcParameterDTO() { nombre = "dia3", tipo = OdbcType.Int, valor = obj.dia3 },
                new OdbcParameterDTO() { nombre = "dia4", tipo = OdbcType.Int, valor = obj.dia4 },
                new OdbcParameterDTO() { nombre = "dia5", tipo = OdbcType.Int, valor = obj.dia5 },
                new OdbcParameterDTO() { nombre = "dia6", tipo = OdbcType.Int, valor = obj.dia6 },
                new OdbcParameterDTO() { nombre = "dia7", tipo = OdbcType.Int, valor = obj.dia7 },
                new OdbcParameterDTO() { nombre = "dia8", tipo = OdbcType.Int, valor = obj.dia8 },
                new OdbcParameterDTO() { nombre = "dia9", tipo = OdbcType.Int, valor = obj.dia9 },
                new OdbcParameterDTO() { nombre = "dia10", tipo = OdbcType.Int, valor = obj.dia10 },
                new OdbcParameterDTO() { nombre = "dia11", tipo = OdbcType.Int, valor = obj.dia11 },
                new OdbcParameterDTO() { nombre = "dia12", tipo = OdbcType.Int, valor = obj.dia12 },
                new OdbcParameterDTO() { nombre = "dia13", tipo = OdbcType.Int, valor = obj.dia13 },
                new OdbcParameterDTO() { nombre = "dia14", tipo = OdbcType.Int, valor = obj.dia14 },
                new OdbcParameterDTO() { nombre = "dia15", tipo = OdbcType.Int, valor = obj.dia15 },
                new OdbcParameterDTO() { nombre = "dia16", tipo = OdbcType.Int, valor = obj.dia16 },
                new OdbcParameterDTO() { nombre = "he_dia1", tipo = OdbcType.Decimal, valor = obj.he_dia1 },
                new OdbcParameterDTO() { nombre = "he_dia2", tipo = OdbcType.Decimal, valor = obj.he_dia2 },
                new OdbcParameterDTO() { nombre = "he_dia3", tipo = OdbcType.Decimal, valor = obj.he_dia3 },
                new OdbcParameterDTO() { nombre = "he_dia4", tipo = OdbcType.Decimal, valor = obj.he_dia4 },
                new OdbcParameterDTO() { nombre = "he_dia5", tipo = OdbcType.Decimal, valor = obj.he_dia5 },
                new OdbcParameterDTO() { nombre = "he_dia6", tipo = OdbcType.Decimal, valor = obj.he_dia6 },
                new OdbcParameterDTO() { nombre = "he_dia7", tipo = OdbcType.Decimal, valor = obj.he_dia7 },
                new OdbcParameterDTO() { nombre = "he_dia8", tipo = OdbcType.Decimal, valor = obj.he_dia8 },
                new OdbcParameterDTO() { nombre = "he_dia9", tipo = OdbcType.Decimal, valor = obj.he_dia9 },
                new OdbcParameterDTO() { nombre = "he_dia10", tipo = OdbcType.Decimal, valor = obj.he_dia10 },
                new OdbcParameterDTO() { nombre = "he_dia11", tipo = OdbcType.Decimal, valor = obj.he_dia11 },
                new OdbcParameterDTO() { nombre = "he_dia12", tipo = OdbcType.Decimal, valor = obj.he_dia12 },
                new OdbcParameterDTO() { nombre = "he_dia13", tipo = OdbcType.Decimal, valor = obj.he_dia13 },
                new OdbcParameterDTO() { nombre = "he_dia14", tipo = OdbcType.Decimal, valor = obj.he_dia14 },
                new OdbcParameterDTO() { nombre = "he_dia15", tipo = OdbcType.Decimal, valor = obj.he_dia15 },
                new OdbcParameterDTO() { nombre = "he_dia16", tipo = OdbcType.Decimal, valor = obj.he_dia16 },
                new OdbcParameterDTO() { nombre = "bono", tipo = OdbcType.Decimal, valor = (obj.bono + obj.bonoDM + obj.bonoU) },
                new OdbcParameterDTO() { nombre = "observaciones", tipo = OdbcType.VarChar, valor = obj.observaciones ?? ""},
                new OdbcParameterDTO() { nombre = "archivo_enviado", tipo = OdbcType.Int, valor = obj.archivo_enviado },
                new OdbcParameterDTO() { nombre = "dias_extras", tipo = OdbcType.Int, valor = (obj.dias_extras + obj.dias_extra_concepto) },
                new OdbcParameterDTO() { nombre = "prima_dominical", tipo = OdbcType.Decimal, valor = obj.prima_dominical },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado },
            };
            return odbc;
        }
        OdbcConsultaDTO updateIncidenciaEmpDet_Bono(tblRH_BN_Incidencia_det obj)
        {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"UPDATE sn_incidencias_empl_det set
                                bono = ? 
                            WHERE id_incidencia = ? AND clave_empleado = ?";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "bono", tipo = OdbcType.Decimal, valor = (obj.bono + obj.bonoDM + obj.bonoU) },
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado },
            };
            return odbc;
        }
        OdbcConsultaDTO delIncidenciaEmpDet(tblRH_BN_Incidencia_det obj)
        {
            obj.observaciones = "";
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"delete from sn_incidencias_empl_det WHERE id_incidencia = ? AND clave_empleado = ?";
            odbc.parametros = new List<OdbcParameterDTO>() 
            {
                new OdbcParameterDTO() { nombre = "id_incidencia", tipo = OdbcType.Int, valor = obj.id_incidencia },
                new OdbcParameterDTO() { nombre = "clave_empleado", tipo = OdbcType.Int, valor = obj.clave_empleado }
            };
            return odbc;
        }
        string getNombreUsuarioEnk(int num) {
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT nom as nombre FROM ek010ab WHERE num = ?";
            odbc.parametros = new List<OdbcParameterDTO>() {
                new OdbcParameterDTO() { nombre = "num", tipo = OdbcType.Int, valor = num }
            };
            var res = _contextEnkontrol.Select<EmpleadoPuestoDTO>(EnkontrolAmbienteEnum.Rh, odbc);

            var a = res.Count() > 0 ? res.FirstOrDefault().nombre : "";
            return a;
        }
        string queryLstIncidenciasEnk()
        {
            return @"SELECT a.*,mod.nom as nombreEmpMod, c.descripcion AS estatusDesc FROM sn_incidencias_empl a INNER JOIN ek010ab mod ON mod.num = a.empleado_modifica inner join sn_estatus c on c.id=a.estatus where a.anio = ? and a.cc = ? and a.periodo = ? and a.tipo_nomina = ?";
        }
        List<OdbcParameterDTO> paramLstIncidenciasEnk(BusqIncidenciaDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = busq.anio });
            param.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = busq.periodo });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            return param;
        }
        string queryLstIncidencias_det_Enk()
        {
            return @"SELECT emp.nombre ,emp.ape_paterno ,emp.ape_materno ,emp.tipo_nomina ,dep.clave_depto, dep.desc_depto as deptoDesc ,emp.puesto ,pue.descripcion as puestoDesc,inc.cc ,inc.anio ,inc.periodo ,est.descripcion AS estatusDesc  ,inc.empleado_modifica ,nom AS nombreEmpMod ,det.*  
                        FROM sn_incidencias_empl inc
                        INNER JOIN sn_incidencias_empl_det det ON inc.id_incidencia = det.id_incidencia
                        INNER JOIN sn_empleados emp ON emp.clave_empleado = det.clave_empleado
                        INNER JOIN ek010ab mod ON mod.num = inc.empleado_modifica
                        INNER JOIN si_puestos pue ON pue.puesto = emp.puesto AND emp.tipo_nomina = pue.tipo_nomina
                        INNER JOIN sn_estatus est ON est.id = inc.estatus
                        INNER JOIN sn_departamentos dep ON (dep.cc = emp.cc_contable and dep.clave_depto=emp.clave_depto)
                        INNER JOIN sn_periodos per ON per.estatus = 'N' AND per.tipo_nomina = inc.tipo_nomina AND per.year = inc.anio AND per.periodo = inc.periodo
                        WHERE inc.cc = ? AND inc.anio = ?  AND inc.periodo = ? AND emp.tipo_nomina = ? AND dep.clave_depto = ?
                            AND det.clave_empleado NOT IN (SELECT baj.clave_empleado FROM sn_empl_baja baj WHERE baj.estatus = 'A' AND baj.clave_empleado = det.clave_empleado AND baj.cc_contable = inc.cc AND baj.fecha_baja < per.fecha_inicial)";
        }
        List<OdbcParameterDTO> paramLstIncidencias_det_Enk(BusqIncidenciaDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "cc", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = busq.anio });
            param.Add(new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = busq.periodo });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            param.Add(new OdbcParameterDTO() { nombre = "clave_depto", tipo = OdbcType.Int, valor = busq.depto });
            return param;
        }

        string queryLstIncidencias_det_EnkEmpty()
        {
            return @"SELECT 
                        e.clave_empleado ,
                        e.nombre ,
                        e.ape_paterno ,
                        e.ape_materno ,
                        e.tipo_nomina ,
                        dep.clave_depto ,
                        dep.desc_depto as deptoDesc,
                        e.puesto ,
                        pu.descripcion as puestoDesc,
                        e.cc_contable ,
                        ? as anio ,
                        ? as periodo ,
                        'PENDIENTE' AS estatusDesc,
                        " + (vSesiones.sesionUsuarioDTO.cveEmpleado ?? "1") + @" as empleado_modifica ,
                        '" + vSesiones.sesionUsuarioDTO.nombre + @"' as nombreEmpMod ,
                        0 as dia1,
                        0 as dia2,
                        0 as dia3,
                        0 as dia4,
                        0 as dia5,
                        0 as dia6,
                        0 as dia7,
                        0 as dia8,
                        0 as dia9,
                        0 as dia10,
                        0 as dia11,
                        0 as dia12,
                        0 as dia13,
                        0 as dia14,
                        0 as dia15,
                        0 as dia16,
                        0 as horas_extras,
                        (select count(*) from (
        SELECT top 2 b.anio,b.periodo,b.cc,a.* FROM sn_incidencias_empl_det a inner join sn_incidencias_empl b on a.id_incidencia=b.id_incidencia
        where a.clave_empleado=e.clave_empleado and b.estatus='A'
        order by b.anio,b.periodo desc
        )x where x.bono>0) as countBonosPersonales,
                    0 as bonoMensual,
                    0 as bonoUnico,
                    e.fecha_antiguedad as fechaAlta,
                    0 as isBaja,
                    e.fecha_antiguedad as fechaBaja
                    FROM sn_empleados as e 
                    inner join sn_empleados as ne on e.jefe_inmediato=ne.clave_empleado 
                    inner join DBA.cc as c ON c.cc = e.cc_contable
                    inner join si_puestos as pu on e.puesto = pu.puesto
                    inner join sn_tipos_nomina as tn on e.tipo_nomina = tn.tipo_nomina
                    INNER JOIN sn_departamentos dep ON (dep.cc = e.cc_contable and dep.clave_depto=e.clave_depto)
                    where e.cc_contable = ? AND e.estatus_empleado = 'A' and tn.tipo_nomina = ?";
        }
        string queryLstIncidencias_det_EnkEmptyNew()
        {
            return @"SELECT 
                        e.clave_empleado,
                        e.nombre,
                        e.ape_paterno,
                        e.ape_materno,
                        e.tipo_nomina,
                        dep.clave_depto,
                        dep.desc_depto AS deptoDesc,
                        e.puesto,
                        pu.descripcion AS puestoDesc,
                        e.cc_contable,
                        @anio AS anio,
                        @periodo AS periodo,
                        'PENDIENTE' AS estatusDesc,
                        " + (vSesiones.sesionUsuarioDTO.cveEmpleado ?? "1") + @" AS empleado_modifica,
                        '" + vSesiones.sesionUsuarioDTO.nombre + @"' AS nombreEmpMod,
                        0 AS dia1,
                        0 AS dia2,
                        0 AS dia3,
                        0 AS dia4,
                        0 AS dia5,
                        0 AS dia6,
                        0 AS dia7,
                        0 AS dia8,
                        0 AS dia9,
                        0 AS dia10,
                        0 AS dia11,
                        0 AS dia12,
                        0 AS dia13,
                        0 AS dia14,
                        0 AS dia15,
                        0 AS dia16,
                        0 AS horas_extras,
                        (
                            SELECT 
                                COUNT(*) 
                            FROM (
                                SELECT 
                                    TOP 2 b.anio, 
                                    b.periodo, 
                                    b.cc, 
                                    a.* 
                                FROM 
                                    tblRH_BN_Incidencia_det a INNER JOIN tblRH_BN_Incidencia b ON a.id_incidencia=b.id_incidencia WHERE a.clave_empleado = e.clave_empleado AND b.estatus = 'A' ORDER BY b.anio, b.periodo DESC) x 
                            WHERE x.bono > 0
                        ) AS countBonosPersonales,
                        0 AS bonoMensual,
                        0 AS bonoUnico,
                        e.fecha_antiguedad AS fechaAlta,
                        (CASE WHEN EXISTS (SELECT b.numeroEmpleado FROM tblRH_Baja_Registro b WHERE b.registroActivo = 1 AND b.cc = e.cc_contable AND b.numeroEmpleado = e.clave_empleado AND (b.fechaBaja >= @fechaBajaInicio and b.fechaBaja <= @fechaBajaFin)) THEN 1 ELSE 0 END) AS isBaja,
                        (SELECT TOP 1 b.fechaBaja FROM tblRH_Baja_Registro b WHERE  b.registroActivo = 1 AND b.cc = e.cc_contable AND b.numeroEmpleado = e.clave_empleado AND (b.fechaBaja >= @fechaBajaInicio AND b.fechaBaja <= @fechaBajaFin)) AS fechaBaja 
                    FROM 
                        tblRH_EK_Empleados AS e 
                        --INNER JOIN DBA.cc AS c ON c.cc = e.cc_contable
                        INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto
                        INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina
                        INNER JOIN tblRH_EK_Departamentos dep ON (dep.cc = e.cc_contable AND dep.clave_depto = e.clave_depto)
                    WHERE 
                        dep.clave_depto = @clave_depto AND
                        (
                            (e.cc_contable = @cc_contable AND e.estatus_empleado = 'A' AND tn.tipo_nomina = @tipo_nomina) OR 
                            (
                                (e.cc_contable = @cc_contable AND e.estatus_empleado = 'B' AND tn.tipo_nomina = @tipo_nomina ) AND 
                                e.clave_empleado in
                                (
                                    SELECT 
                                        b.numeroEmpleado 
                                    FROM 
                                        tblRH_Baja_Registro b 
                                        INNER JOIN tblRH_EK_Empleados empl ON b.numeroEmpleado = empl.clave_empleado 
                                    WHERE 
                                        b.est_baja='A' AND 
                                        b.cc = @cc_contable AND 
                                        (b.fechaBaja >= @fechaBajaInicio AND b.fechaBaja <= @fechaBajaFin) AND 
                                        empl.tipo_nomina = @tipo_nomina
                                )
                            )
                        )
                    ";
        }
        List<OdbcParameterDTO> paramLstIncidencias_det_EnkEmptyNew(BusqIncidenciaDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = busq.anio });
            param.Add(new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = busq.periodo });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaFin });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaFin });
            param.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            //param.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.NVarChar, valor = busq.cc });
            //param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            //param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaFin });
            //param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            param.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            param.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaInicio });
            param.Add(new OdbcParameterDTO() { nombre = "fecha_baja", tipo = OdbcType.DateTime, valor = busq.fechaFin });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            param.Add(new OdbcParameterDTO() { nombre = "clave_depto", tipo = OdbcType.Int, valor = busq.depto });
            return param;
        }
        List<OdbcParameterDTO> paramLstIncidencias_det_EnkEmpty(BusqIncidenciaDTO busq)
        {
            var param = new List<OdbcParameterDTO>();
            param.Add(new OdbcParameterDTO() { nombre = "anio", tipo = OdbcType.Int, valor = busq.anio });
            param.Add(new OdbcParameterDTO() { nombre = "periodo", tipo = OdbcType.Int, valor = busq.periodo });
            param.Add(new OdbcParameterDTO() { nombre = "cc_contable", tipo = OdbcType.NVarChar, valor = busq.cc });
            param.Add(new OdbcParameterDTO() { nombre = "tipo_nomina", tipo = OdbcType.Int, valor = busq.tipoNomina });
            param.Add(new OdbcParameterDTO() { nombre = "clave_depto", tipo = OdbcType.Int, valor = busq.depto });
            return param;
        }
        #endregion
        #region combobox
        public List<ComboDTO> getTblP_CC()
        {
            using (var ctx = new MainContext((int)vSesiones.sesionEmpresaActual))
            {
                //var esCplan = vSesiones.sesionEmpresaActual.Equals((int)Empresa.Construplan);
                //var lst = ctx.tblP_CC.Where(c => c.estatus).ToList();
                //var lstAuth = _context.tblRH_BonoAdminMonto.Where(m => m.esActivo).ToList();
                //var cbo = lst.Select(cc => new ComboDTO()
                //{
                //    Text = string.Format("{0} - {1}", esCplan ? cc.cc : cc.areaCuenta, cc.descripcion).ToUpper(),
                //    Value = esCplan ? cc.cc : cc.areaCuenta,
                //    Prefijo = lstAuth.Any(a => a.cc.Equals(esCplan ? cc.cc : cc.areaCuenta)) ? lstAuth.FirstOrDefault(a => a.cc.Equals(esCplan ? cc.cc : cc.areaCuenta)).idAuth.ToString() : string.Empty
                //}).OrderBy(o => o.Value).ToList();
                //var ccs = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
                var ccs = ctx.tblC_Nom_CatalogoCC.Where(x => x.estatus).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.ccDescripcion.Trim()
                }).ToList();
                return ccs;
            }
        }
        public List<ComboDTO> getTblP_CCconPlantilla()
        {
            using (var ctx = new MainContext((int)vSesiones.sesionEmpresaActual))
            {
                List<ComboDTO> ccs = new List<ComboDTO>();
                ccs = ctx.tblC_Nom_CatalogoCC.Where(x => x.estatus && (x.quincenal || x.semanal)).Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.ccDescripcion.Trim()
                }).ToList();
                
                //var ccs = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
                var ccPlantilla = _context.tblRH_BN_Plantilla.Where(x => x.estatus == 1).Select(x => x.cc).ToList();
                //ccs.Where(x => ccPlantilla.Contains(x.Value)).ToList();
                return ccs.Where(x => ccPlantilla.Contains(x.Value)).ToList();
            }
        }
        public List<ComboDTO> getcboCcMonto()
        {
            using (var ctx = new MainContext((int)vSesiones.sesionEmpresaActual))
            {
                var ccs = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
                //var esCplan = vSesiones.sesionEmpresaActual.Equals((int)Empresa.Construplan);
                //var lst = ctx.tblP_CC.Where(c => c.estatus).ToList();
                //var lstAuth = _context.tblRH_BonoAdminMonto.Where(m => m.esActivo).ToList();
                //var lstMonto = _context.tblRH_BonoAdminMonto.ToList().Where(mon => mon.esActivo).ToList();
                
                //var cbo = lst.Where(cc => lstMonto.Any(mon => mon.cc.Equals(cc.cc)))
                //    .Select(cc => new ComboDTO()
                //{
                //    Text = string.Format("{0} - {1}", esCplan ? cc.cc : cc.areaCuenta, cc.descripcion).ToUpper(),
                //    Value = esCplan ? cc.cc : cc.areaCuenta,
                //    Prefijo = lstAuth.Any(a => a.cc.Equals(esCplan ? cc.cc : cc.areaCuenta)) ? lstAuth.FirstOrDefault(a => a.cc.Equals(esCplan ? cc.cc : cc.areaCuenta)).idAuth.ToString() : string.Empty
                //}).OrderBy(o => o.Value).ToList();
                return ccs;
            }
        }
        public List<ComboDTO> getcboCcDepto()
        {
            using (var ctx = new MainContext((int)vSesiones.sesionEmpresaActual))
            {
                //var lst = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();
                List<ComboDTO> lst = ctx.tblC_Nom_CatalogoCC.Where(x => x.estatus && x.cc != "180-A" && x.cc != "187-A").Select(x => new ComboDTO
                {
                    Value = x.cc,
                    Text = "[" + x.cc + "] " + x.ccDescripcion
                }).ToList();
                var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.cc.Equals("*"));
                var ccs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x=>x.cc).ToList();
                //var cboDepto = _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Rh, "SELECT cc AS Prefijo,clave_depto AS Value,(clave_depto +'-'+desc_depto) AS Text FROM sn_departamentos");
                var cboDepto = _context.tblRH_EK_Departamentos.Select(x => new ComboDTO { 
                    Prefijo = x.cc,
                    Value = x.clave_depto.ToString(),
                    Text = x.clave_depto + " " + x.desc_depto
                }).ToList();
                if(permiso)
                {
                    var cbo = lst.Select(cc => new ComboDTO()
                    {
                        Text = cc.Text,
                        Value = cc.Value,
                        Prefijo = JsonUtils.Json(new
                        {
                            cboDepto = cboDepto.Where(w => w.Prefijo.Equals(cc.Value)).ToList()
                        })
                    }).OrderBy(o => o.Value).ToList();


                    if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        cbo = cbo.Where(e => Regex.IsMatch(e.Value.ToString(), @"^\d+$")).ToList();
                    }

                    cbo.RemoveAll(x => x.Value.ToString() == "0");

                    return cbo;
                }
                else
                {
                    var cbo = lst.Where(x => ccs.Contains(x.Value))
                    .Select(cc => new ComboDTO()
                    {
                        Text = cc.Text,
                        Value = cc.Value,
                        Prefijo = JsonUtils.Json(new
                        {
                            cboDepto = cboDepto.Where(w => w.Prefijo.Equals(cc.Value)).ToList()
                        })
                    }).OrderBy(o => o.Value).ToList();

                    if ((int)vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                    {
                        cbo = cbo.Where(e => Regex.IsMatch(e.Value.ToString(), @"^\d+$")).ToList();
                    }

                    cbo.RemoveAll(x => x.Value.ToString() == "0");

                    return cbo;
                }

                
            }
        }
        public List<ComboDTO> getCboAutorizante(string cc)
        {
            using (var ctx = new MainContext((int)vSesiones.sesionEmpresaActual))
            {
                var lstPerfil = new List<int>() { 1, 2, 3, 4 };
                var esCplan = vSesiones.sesionEmpresaActual.Equals((int)EmpresaEnum.Construplan);
                var objCc = ctx.tblP_CC.FirstOrDefault(c => c.estatus && esCplan ? c.cc.Equals(cc) : c.areaCuenta.Equals(cc));
                var relCc = ctx.tblP_CC_Usuario.Where(r => r.cc.Equals(objCc.areaCuenta)).Select(s => s.usuarioID).ToList();
                var lstAuth = ctx.tblP_Autoriza.Where(w => lstPerfil.Any(a => a.Equals(w.perfilAutorizaID)) && relCc.Any(a => a.Equals(w.usuarioID))).ToList();
                var cbo = lstAuth
                    .GroupBy(g => g.usuario)
                    .Select(auth => new ComboDTO()
                {
                    Text = string.Format("{0} {1} {2}", auth.Key.nombre, auth.Key.apellidoPaterno, auth.Key.apellidoMaterno),
                    Value = auth.Key.id.ToString(),
                }).ToList();
                return cbo;
            }
        }
        public List<ComboDTO> getCboIncidecnciaConcepto()
        {
            //var lst = _contextEnkontrol.Select<IncidenciasConceptoDTO>(EnkontrolAmbienteEnum.Rh, "SELECT * FROM sn_incidencias_conceptos");
            var lst = _context.tblRH_EK_Incidencias_Conceptos.ToList();
            var cbo = lst.Select(con => new ComboDTO()
            {
                Value = con.id.ToString(),
                Text = con.abreviatura,
                Prefijo = JsonUtils.Json(con)
            }).ToList();
            return cbo;
        }
        #endregion 
        #region listaNegra
        private bool ExisteEnLista(int claveEmpleado, tipoListaEnum tipoLista)
        {
            bool existe = false;

            switch (tipoLista)
            {
                case tipoListaEnum.blanca:
                    existe = _context.tblRH_BN_ListaBlanca.Any(a => a.cve_Emp == claveEmpleado && a.estatus);
                    break;
                case tipoListaEnum.negra:
                    existe = _context.tblRH_BN_ListaNegra.Any(a => a.cve_Emp == claveEmpleado && a.estatus);
                    break;
            }

            return existe;
        }

        public Respuesta GuardarEnListaNegra(tblRH_BN_ListaNegra empleado)
        {
            var r = new Respuesta();

            try
            {
                var existeEnListaNegra = _context.tblRH_BN_ListaNegra.Any(f => f.cve_Emp == empleado.cve_Emp && f.estatus);

                if (!existeEnListaNegra && !ExisteEnLista(empleado.cve_Emp, tipoListaEnum.blanca))
                {
                    empleado.estatus = true;
                    empleado.fecha = DateTime.Now;
                    empleado.usuarioID = vSesiones.sesionUsuarioDTO.id;

                    _context.tblRH_BN_ListaNegra.Add(empleado);
                    _context.SaveChanges();

                    r.Success = true;
                    r.Message = "Ok";
                }
                else
                {
                    r.Message = "El empleado ya se encuentra en una lista (negra o blanca)";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta EmpleadosListaNegra()
        {
            var r = new Respuesta();

            try
            {
                var empleados = _context.tblRH_BN_ListaNegra.Where(w => w.estatus).Select(m => new ListaNegraBlancaDTO
                {
                    cve = m.cve_Emp,
                    nombre = m.nombre_Emp,
                    cc = m.cc
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = empleados;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta EliminarDeListaNegra(int claveEmpleado)
        {
            var r = new Respuesta();

            try
            {
                var empleado = _context.tblRH_BN_ListaNegra.FirstOrDefault(f => f.cve_Emp == claveEmpleado && f.estatus);

                if (empleado != null)
                {
                    empleado.estatus = false;
                    empleado.fecha = DateTime.Now;
                    empleado.usuarioID = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();
                }

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        #endregion
        #region listaBlanca
        public Respuesta GuardarEnListaBlanca(tblRH_BN_ListaBlanca empleado)
        {
            var r = new Respuesta();

            try
            {
                var existeEnListaBlanca = _context.tblRH_BN_ListaBlanca.Any(f => f.cve_Emp == empleado.cve_Emp && f.estatus);

                if (!existeEnListaBlanca && !ExisteEnLista(empleado.cve_Emp, tipoListaEnum.negra))
                {
                    empleado.estatus = true;
                    empleado.fecha = DateTime.Now;
                    empleado.usuarioID = vSesiones.sesionUsuarioDTO.id;

                    _context.tblRH_BN_ListaBlanca.Add(empleado);
                    _context.SaveChanges();

                    r.Success = true;
                    r.Message = "Ok";
                }
                else
                {
                    r.Message = "El empleado ya se encuentra en una lista (negra o blanca)";
                }
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta EmpleadosListaBlanca()
        {
            var r = new Respuesta();

            try
            {
                var empleados = _context.tblRH_BN_ListaBlanca.Where(w => w.estatus).Select(m => new ListaNegraBlancaDTO
                {
                    cve = m.cve_Emp,
                    nombre = m.nombre_Emp,
                    cc = m.cc
                }).ToList();

                r.Success = true;
                r.Message = "Ok";
                r.Value = empleados;
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }

        public Respuesta EliminarDeListaBlanca(int claveEmpleado)
        {
            var r = new Respuesta();

            try
            {
                var empleado = _context.tblRH_BN_ListaBlanca.FirstOrDefault(f => f.cve_Emp == claveEmpleado && f.estatus);

                if (empleado != null)
                {
                    empleado.estatus = false;
                    empleado.fecha = DateTime.Now;
                    empleado.usuarioID = vSesiones.sesionUsuarioDTO.id;

                    _context.SaveChanges();
                }

                r.Success = true;
                r.Message = "Ok";
            }
            catch (Exception ex)
            {
                r.Message += ex.Message;
            }

            return r;
        }
        #endregion
        public empleadosEvaluacionDTO getEmpleadosEvaluar(string cc, int periodo, int tipoNomina, DateTime fechaPeriodo)
        {
            var r = new empleadosEvaluacionDTO();
            var data = new List<tblRH_BN_Evaluacion_Det>();
            var ev = _context.tblRH_BN_Evaluacion.FirstOrDefault(x=> x.cc == cc && x.periodo == periodo && x.tipoNomina == tipoNomina && x.anio == fechaPeriodo.Year);

            bool permisoGerencia = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4048).Count()) > 0;
            bool permisoSueldos = (_context.tblP_AccionesVistatblP_Usuario.Where(x => x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id && x.tblP_AccionesVista_id == 4049).Count()) > 0;

            if (ev != null)
            {
                switch (ev.estatus)
                {
                    case (int)authEstadoEnum2.EnEspera:
                        {
                            r.estatus = (int)authEstadoEnum.EnTurno;
                        }
                        break;
                    case (int)authEstadoEnum.Autorizado:
                    case (int)authEstadoEnum.Rechazado:
                        {
                            r.estatus = ev.estatus;
                        }
                        break;
                    default:
                        break;
                }
                ev.listDetalle.ToList().ForEach(x=>x.periodicidad = (x.periodicidadCve == 1 ? "Sem" : x.periodicidadCve == 4 ? ("Quin") : "Mes"));
                //r.validos = ev.listDetalle.ToList();

                var lstFiltradaDetalle = new List<tblRH_BN_Evaluacion_Det>();

                foreach (var item in ev.listDetalle)
	            {
                    if (!permisoGerencia)
	                {
                       //(permisoGerencia ? "" : " AND (pu.descripcion NOT LIKE '%GERENCIA%' AND pu.descripcion NOT LIKE '%GERENTE%' AND pu.descripcion NOT LIKE '%DIRECCI%' AND pu.descripcion NOT LIKE '%DIRECTOR%')");
                       if (item.puesto_Emp.Contains("GERENCIA") || item.puesto_Emp.Contains("GERENTE") || item.puesto_Emp.Contains("DIRECCI") || item.puesto_Emp.Contains("DIRECTOR"))
                       {
                           lstFiltradaDetalle.Add(item);
                       }
	                }

                    if (!permisoSueldos)
	                {
                        
                        item.bono_Emp = 0;
                        item.base_Emp = 0;
                        item.complemento_Emp = 0;
                        item.bono_FC = 0;
                        item.total_Nom = 0;
                        item.total_Mensual = 0;
                        item.con_Bono = 0;
                        item.monto_Asig = 0;
                    }
	            }

                r.validos = ev.listDetalle.Except(lstFiltradaDetalle).ToList();

                r.novalidos = new List<tblRH_BN_Evaluacion_Det>();
                r.evaluacionID = ev.id;
            }
            else
            {
                //Primero obtenemos el día actual
                DateTime date = fechaPeriodo;

                //Asi obtenemos el primer dia del mes actual
                DateTime oPrimerDiaDelMes = new DateTime(date.Year, date.Month, 1);

                //Y de la siguiente forma obtenemos el ultimo dia del mes
                //agregamos 1 mes al objeto anterior y restamos 1 día.
                DateTime oUltimoDiaDelMesAnterior = oPrimerDiaDelMes.AddDays(-1);


//                string query = @"SELECT 
//            e.clave_empleado as cve_Emp,
//            (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as nombre_Emp,
//            pu.puesto as puestoCve_Emp,
//            pu.descripcion as puesto_Emp,
//            (select top 1 salario_base from sn_tabulador_historial where clave_empleado=e.clave_empleado order by id desc) as base_Emp,
//            (select top 1 complemento from sn_tabulador_historial where clave_empleado=e.clave_empleado order by id desc) as complemento_Emp,
//            (select top 1 bono_zona from sn_tabulador_historial where clave_empleado=e.clave_empleado order by id desc) as bono_FC,
//            0 as porcentaje_Asig,
//            0 as monto_Asig,
//            0 as total_Nom,
//            tn.descripcion as tipo_Nom,
//            tn.tipo_nomina as tipoCve_Nom,
//            0 as total_Mensual,
//            CONVERT( CHAR( 20 ),(SELECT TOP 1 ser.fecha_reingreso FROM sn_empl_recontratacion as ser where ser.clave_empleado = e.clave_empleado
//            AND ser.cc = e.cc_contable AND ser.fecha_reingreso > e.fecha_alta ORDER BY ser.fecha_reingreso DESC), 103 ) as fechaRe,
//            CONVERT( CHAR( 20 ), e.fecha_antiguedad, 103 ) as fechaAltaStr
//            FROM sn_empleados as e 
//            inner join sn_empleados as ne on e.jefe_inmediato=ne.clave_empleado 
//            inner join DBA.cc as c ON c.cc = e.cc_contable
//            inner join si_puestos as pu on e.puesto = pu.puesto
//            inner join sn_tipos_nomina as tn on e.tipo_nomina = tn.tipo_nomina
//            where e.cc_contable='" + cc + "' AND e.estatus_empleado = 'A' and tn.tipo_nomina=" + tipoNomina;



                //var result = (List<tblRH_BN_Evaluacion_Det>)ContextEnKontrolNomina.Where(query).ToObject<List<tblRH_BN_Evaluacion_Det>>();

                var query =
                    @"SELECT 
	                    e.clave_empleado AS cve_Emp,
                        (e.ape_paterno + ' ' + e.ape_materno + ' ' + e.nombre) AS nombre_Emp,
                        e.puesto_anterior AS puestoCve_Emp,
                        pu.descripcion AS puesto_Emp,
                        (SELECT TOP 1 salario_base FROM tblRH_EK_Tabulador_Historial WHERE clave_empleado=e.clave_empleado ORDER BY id DESC) AS base_Emp,
                        (SELECT TOP 1 complemento FROM tblRH_EK_Tabulador_Historial WHERE clave_empleado=e.clave_empleado ORDER BY id desc) AS complemento_Emp,
                        (SELECT TOP 1 bono_zona FROM tblRH_EK_Tabulador_Historial WHERE clave_empleado=e.clave_empleado ORDER BY id DESC) AS bono_FC,
                        0 AS porcentaje_Asig,
                        0 AS monto_Asig,
                        0 AS total_Nom,
                        tn.descripcion AS tipo_Nom,
                        tn.tipo_nomina AS tipoCve_Nom,
                        0 AS total_Mensual,
                        CONVERT(CHAR(20),
			                    (	
				                    SELECT TOP 1 ser.fecha_reingreso FROM tblRH_EK_Empl_Recontratacion AS ser
				                    WHERE
					                    ser.clave_empleado = e.clave_empleado AND
					                    ser.cc = e.cc_contable AND
					                    ser.fecha_reingreso > e.fecha_alta
				                    ORDER BY
					                    ser.fecha_reingreso DESC
			                    ), 103) AS fechaRe,
                        CONVERT(CHAR(20), e.fecha_antiguedad, 103) AS fechaAltaStr
                    FROM
	                    tblRH_EK_Empleados AS e 
                    INNER JOIN
	                    tblRH_EK_Empleados AS ne ON e.jefe_inmediato=ne.clave_empleado 
                    LEFT JOIN
	                    tblP_CC AS c ON c.cc = e.cc_contable
                    INNER JOIN
	                    tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto
                    INNER JOIN
	                    tblRH_EK_Tipos_Nomina AS tn ON e.tipo_nomina = tn.tipo_nomina
                    WHERE
                        e.cc_contable = @paramCC AND
                        e.estatus_empleado = 'A' AND
                        tn.tipo_nomina = @paramTipoNomina" +
                        (permisoGerencia ? "" : " AND (pu.descripcion NOT LIKE '%GERENCIA%' AND pu.descripcion NOT LIKE '%GERENTE%' AND pu.descripcion NOT LIKE '%DIRECCI%' AND pu.descripcion NOT LIKE '%DIRECTOR%')");

                var result = _context.Select<tblRH_BN_Evaluacion_Det>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = query,
                    parametros = new { paramTipoNomina = tipoNomina, paramCC = cc }
                });

                var lstNegra = _context.tblRH_BN_ListaNegra.Where(w => w.estatus).Select(m => m.cve_Emp).ToList();

                var plantilla = _context.tblRH_BN_Plantilla.FirstOrDefault(x => x.cc.Equals(cc) && x.versionActiva);
                if (plantilla != null)
                {
                    var puestos = plantilla.listDetalle.Select(x => x.puesto).ToList();

                    data.AddRange(result);
                    foreach (var i in data)
                    {
                        var o = plantilla.listDetalle.FirstOrDefault(x => x.puesto == i.puestoCve_Emp);
                        if (o != null)
                        {
                            if (permisoSueldos)
	                        {
                                i.bono_Emp = o.monto;
		 
	                        }else{
                                i.bono_Emp = 0;
                                i.base_Emp = 0;
                                i.complemento_Emp = 0;
                                i.bono_FC = 0;
                                i.monto_Asig = 0;

                            }

                            i.periodicidadCve = o.periodicidad;
                            i.periodicidad = (o.periodicidad == 1 ? "Sem" : o.periodicidad == 4 ? ("Quin") : "Mes");
                            i.plantillaID = plantilla.id;
                            i.plantillaDetID = plantilla.listDetalle.FirstOrDefault(x => x.puesto == i.puestoCve_Emp).id;
                        }
                        else {
                            i.bono_Emp = 0;
                            i.periodicidadCve = 0;
                            i.periodicidad = "NOPLANTILLA";
                            i.plantillaID = 0;
                            i.plantillaDetID = 0;
                        }
                    }
                }
                var test = new List<int>();
                var ccExc = _context.tblRH_BN_CCExepcion.FirstOrDefault(x => x.cc == cc);
                int meses = ccExc != null ? ccExc.meses : 4;
                int dias = meses * 30;
                foreach (var i in data)
                {
                    var fecha = !string.IsNullOrEmpty(i.fechaRe) ? Convert.ToDateTime(i.fechaRe) > Convert.ToDateTime(i.fechaAltaStr) ? Convert.ToDateTime(i.fechaRe) : Convert.ToDateTime(i.fechaAltaStr) : Convert.ToDateTime(i.fechaAltaStr);
                    i.fechaAltaRe = string.IsNullOrEmpty(i.fechaRe) ? i.fechaAltaStr : i.fechaRe;
                    i.fechaAlta = Convert.ToDateTime(i.fechaAltaStr);
                    i.antiguedad = anosMesesDias(i.fechaAlta, oUltimoDiaDelMesAnterior);
                    i.antiguedad.diasOBraParaBono = dias;

                }
                var lstBlanca = _context.tblRH_BN_ListaBlanca.Where(w => w.estatus).Select(m => m.cve_Emp);

                var datosValidos = data.Where(x => ((x.bono_FC <= 0 && (x.antiguedad.dias >= dias)) || (x.bono_FC > 0 && lstBlanca.Contains(x.cve_Emp)) && (lstBlanca.Contains(x.cve_Emp)) || (lstBlanca.Contains(x.cve_Emp))) && (!lstNegra.Contains(x.cve_Emp)) && (!x.periodicidad.Equals("NOPLANTILLA"))).OrderBy(x => x.nombre_Emp).ToList();
                var datosNoValidos = data.Where(x => !datosValidos.Any(y => y.cve_Emp == x.cve_Emp)).ToList();
                foreach (var x in datosNoValidos)
                {
                    var ln = lstNegra.Where(y => y.Equals(x.cve_Emp)).Count();
                    var tn = ln > 0 ? x.tipo_Nom = "lstnegra" : x.tipo_Nom = x.tipo_Nom;
                    x.tipo_Nom = tn;
                }
                r.validos = datosValidos;
                r.novalidos = datosNoValidos;
                r.estatus = (int)authEstadoEnum.EnEspera;
                r.evaluacionID = 0;
            }
            
            return r;
        }
        public PeriodoDTO anosMesesDias(System.DateTime fechaInicio, System.DateTime fechaFin)
        {
            PeriodoDTO obj = new PeriodoDTO();
            int anos = 0;
            int meses = 0;
            int dias = 0;
            int m = 0;

            anos = fechaFin.Year - fechaInicio.Year;
            if (fechaInicio.Month > fechaFin.Month)
            {
                anos = anos - 1;
            }
            if (fechaFin.Month < fechaInicio.Month)
            {
                meses = 12 - fechaInicio.Month + fechaFin.Month;
            }
            else
            {
                meses = fechaFin.Month - fechaInicio.Month;
            }
            if (fechaFin.Day < fechaInicio.Day)
            {
                meses = meses - 1;
                if (fechaFin.Month == fechaInicio.Month)
                {
                    anos = anos - 1;
                    meses = 11;
                }
            }
            dias = fechaFin.Day - fechaInicio.Day;
            if (dias < 0)
            {
                m = Convert.ToInt32(fechaFin.Month) - 1;
                if (m == 0)
                {
                    m = 12;
                }
                switch (m)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        dias = 31 + dias;
                        break;
                    case 4:
                    case 6:
                    case 9:
                    case 11:
                        dias = 30 + dias;
                        break;
                    case 2:
                        if ((fechaFin.Year % 4 == 0 & fechaFin.Year % 100 != 0) | fechaFin.Year % 400 == 0)
                        {
                            dias = 29 + dias;
                        }
                        else
                        {
                            dias = 28 + dias;
                        }
                        break;
                }
            }
            obj.anio = anos;
            obj.mes = meses;
            obj.dia = dias;
            TimeSpan difFechas = (fechaFin - fechaInicio);
            obj.dias = difFechas.Days;
            return obj;
        }
        public List<tblRH_BN_Evaluacion_Det> getEmpleadosUnicos(string cc, int tipoNomina)
        {
//            var data = new List<tblRH_BN_Evaluacion_Det>();
//            string query = @"SELECT 
//            e.clave_empleado as cve_Emp,
//            (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as nombre_Emp,
//            pu.puesto as puestoCve_Emp,
//            pu.descripcion as puesto_Emp,
//            (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as base_Emp,
//            (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as complemento_Emp,
//            (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as bono_Emp,
//            0 as porcentaje_Asig,
//            0 as monto_Asig,
//            0 as total_Nom,
//            tn.descripcion as tipo_Nom,
//            tn.tipo_nomina as tipoCve_Nom,
//            0 as total_Mensual 
//            FROM tblRH_EK_Empleados as e 
//            inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
//            inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
//            inner join tblrh_ek_puestos as pu on e.puesto = pu.puesto
//            inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
//            where e.cc_contable='" + cc + "' AND e.estatus_empleado = 'A' " + " and  tn.tipo_nomina="+tipoNomina;



            //var result = (List<tblRH_BN_Evaluacion_Det>)ContextEnKontrolNomina.Where(query).ToObject<List<tblRH_BN_Evaluacion_Det>>();

            var result = _context.Select<tblRH_BN_Evaluacion_Det>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT 
                    e.clave_empleado as cve_Emp,
                    (e.ape_paterno+' '+e.ape_materno+' '+e.nombre) as nombre_Emp,
                    pu.puesto as puestoCve_Emp,
                    pu.descripcion as puesto_Emp,
                    (select top 1 salario_base from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as base_Emp,
                    (select top 1 complemento from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as complemento_Emp,
                    (select top 1 bono_zona from tblRH_EK_Tabulador_Historial where clave_empleado=e.clave_empleado order by id desc) as bono_Emp,
                    0 as porcentaje_Asig,
                    0 as monto_Asig,
                    0 as total_Nom,
                    tn.descripcion as tipo_Nom,
                    tn.tipo_nomina as tipoCve_Nom,
                    0 as total_Mensual 
                    FROM tblRH_EK_Empleados as e 
                    inner join tblRH_EK_Empleados as ne on e.jefe_inmediato=ne.clave_empleado 
                    inner join tblC_Nom_CatalogoCC as c ON c.cc = e.cc_contable
                    inner join tblrh_ek_puestos as pu on e.puesto = pu.puesto
                    inner join tblRH_EK_Tipos_Nomina as tn on e.tipo_nomina = tn.tipo_nomina
                    where e.cc_contable = @paramCC and e.estatus_empleado = 'A' and tn.tipo_nomina = @paramTipoNomina",
                parametros = new { paramCC = cc, paramTipoNomina = tipoNomina }
            }).OrderBy(x => x.nombre_Emp).ToList();


            //return result.OrderBy(x=>x.nombre_Emp).ToList();
            return result;
        }
        public List<incidenciasPendientesDTO> getIncidenciasPendiente()
        {
            List<incidenciasPendientesDTO> result = new List<incidenciasPendientesDTO>();
            var _user = vSesiones.sesionUsuarioDTO.id;
            //var lst = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();

            List<ComboDTO> lst = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT cc AS VALUE, (cc + '-' + ccDescripcion) AS TEXT FROM tblC_Nom_CatalogoCC WHERE cc <> '180-A' and cc <> '187-A'",
            }).ToList();

            var permisoTodo = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.cc.Equals("*") && x.autoriza);
            var ccs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && !x.cc.Equals("*") && x.autoriza).Select(x => x.cc).ToList();

            var ccAutoriza = lst.Where(x => permisoTodo ? true : ccs.Contains(x.Value)).Select(x=>x.Value).OrderBy(o => o).ToList();

            var anio = DateTime.Now.Year;
            var diaActual = DateTime.Today;
            var diaLimiteSemana = diaActual.AddDays(6);
            var diaLimiteQuincena = diaActual.AddDays(14);
           
            DateTime firstDayInWeek = GetFirstDayOfWeek(DateTime.Now);
            DateTime lasttDayInWeek = GetFirstDayOfWeek(DateTime.Now).AddDays(40);
            var periodos = _context.tblRH_BN_EstatusPeriodos.OrderBy(x => x.fecha_inicial).ToList();
            var semanaActual = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_limite <= diaLimiteSemana && x.anio == anio && x.tipo_nomina == 1).OrderByDescending(x => x.periodo).FirstOrDefault();
            var quincenaActual = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_limite <= diaLimiteQuincena && x.anio == anio && x.tipo_nomina == 4).OrderByDescending(x => x.periodo).FirstOrDefault();
            //if (quincena.Count == 0)
            //{
            //    quincena.Add(new tblRH_BN_EstatusPeriodos() { tipo_nomina = 0, periodo = 0 });
            //}

            var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.estatus.Equals("P") && (ccAutoriza.Contains(x.cc)) && /*(((x.periodo == semana.periodo && x.tipo_nomina == 1) || (x.periodo == quincena.periodo && x.tipo_nomina == 4)))) &&*/ _context.tblRH_BN_Incidencia_det.Any(y => y.incidenciaID == x.id)).ToList();

            var incidenciasTotales = _context.tblRH_BN_Incidencia.Where(x => x.estatus.Equals("A") && ((x.anio >= anio && ccAutoriza.Contains(x.cc)) || (x.anio == anio && ccAutoriza.Contains(x.cc) && ((x.periodo == semanaActual.periodo && x.tipo_nomina == 1) || (x.periodo == quincenaActual.periodo && x.tipo_nomina == 4)))) && _context.tblRH_BN_Incidencia_det.Any(y => y.incidenciaID == x.id)).ToList();

            incidenciasTotales.AddRange(incidencias);

            var ccSinCaptura = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus && ccAutoriza.Contains(x.cc)).ToList();
            
            
            var ccConCapturaSemanal = incidenciasTotales.Where(x => x.tipo_nomina == 1 && x.periodo == semanaActual.periodo && x.anio == anio).Select(x => x.cc).ToList();
            var ccConCapturaQuincenal = incidenciasTotales.Where(x => x.tipo_nomina == 4 && x.periodo == quincenaActual.periodo && x.anio == anio).Select(x => x.cc).ToList();
            var ccSinCapturaSemanal = ccSinCaptura.Where(x => x.semanal && !ccConCapturaSemanal.Contains(x.cc)).Select(x => x.cc).ToList();
            var ccSinCapturaQuincenal = ccSinCaptura.Where(x => x.quincenal && !ccConCapturaQuincenal.Contains(x.cc)).Select(x => x.cc).ToList();

            

            foreach (var i in incidencias)
            {
                var periodo = periodos.Where(x => x.periodo == i.periodo && x.anio == i.anio && x.tipo_nomina == i.tipo_nomina).FirstOrDefault();
                var empleadosCapturadosID = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == i.id).Select(x => x.clave_empleado).ToList(); ;
                var empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => x.tipo_nomina == i.tipo_nomina && x.cc_contable == i.cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == i.anio && x.tipoNomina == i.tipo_nomina && x.periodo == i.periodo && x.cc == i.cc);
                var o = new incidenciasPendientesDTO();
                var cc = lst.FirstOrDefault(x => x.Value.Equals(i.cc));
                o.id = i.id;
                o.id_incidencia = i.id_incidencia;
                o.cc = cc == null ? i.cc : cc.Text;
                o.anio = i.anio;
                o.tipo_nomina = i.tipo_nomina;
                o.periodo = i.periodo;
                o.fechas = periodo.fecha_inicial.ToShortDateString() + " - " + periodo.fecha_final.ToShortDateString();
                o.cambio_pendiente = empleadosPendienteCaptura.Count() > 0;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = periodo.fecha_inicial;
                result.Add(o);
            }

            foreach (var item in ccSinCapturaSemanal) 
            {                
                var o = new incidenciasPendientesDTO();
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == anio && x.tipoNomina == 1 && x.periodo == semanaActual.periodo && x.cc == item);
                var cc = lst.FirstOrDefault(x => x.Value.Equals(item));
                o.id = 0;
                o.id_incidencia = 0;
                o.cc = cc == null ? item : cc.Text;
                o.anio = anio;
                o.tipo_nomina = 1;
                o.periodo = semanaActual.periodo;
                o.fechas = semanaActual.fecha_inicial.ToShortDateString() + " - " + semanaActual.fecha_final.ToShortDateString();
                o.cambio_pendiente = true;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = semanaActual.fecha_inicial;
                result.Add(o);
            }

            foreach (var item in ccSinCapturaQuincenal)
            {
                var o = new incidenciasPendientesDTO();
                var cc = lst.FirstOrDefault(x => x.Value.Equals(item));
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == anio && x.tipoNomina == 4 && x.periodo == semanaActual.periodo && x.cc == item);
                o.id = 0;
                o.id_incidencia = 0;
                o.cc = cc == null ? item : cc.Text;
                o.anio = anio;
                o.tipo_nomina = 4;
                o.periodo = quincenaActual.periodo;
                o.fechas = quincenaActual.fecha_inicial.ToShortDateString() + " - " + quincenaActual.fecha_final.ToShortDateString();
                o.cambio_pendiente = true;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = quincenaActual.fecha_inicial;
                result.Add(o);
            }

            return result.OrderBy(x=>x.anio).ThenBy(x=>x.fecha_inicio).ThenBy(x=>x.cc).ToList();
        }        

        public DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

        #region PERU
        public IncidenciasPaqueteDTO getLstIncidenciasPeru(BusqIncidenciaDTO busq)
        {

            var result = new IncidenciasPaqueteDTO();
            var cplan = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && x.periodo == busq.periodo && busq.anio == x.anio);
            var incapacidades = _context.tblRH_Vacaciones_Incapacidades.Where(x => x.cc == busq.cc && x.esActivo && x.estatus == 1).ToList();
            var vacaciones = _context.tblRH_Vacaciones_Vacaciones.Where(x => x.registroActivo && x.estado == 1 && x.esPagadas.Value != true).ToList();
            List<tblRH_BN_Incidencia_det_Peru> datosPeru = new List<tblRH_BN_Incidencia_det_Peru>();
            if (cplan != null)
            {
                cplan.nombreAutoriza = "";
                if (cplan.estatus.Equals("P"))
                {
                    //getIncidencia_det_new(cplan.id, busq);
                }
                else
                {
                    if (cplan.usuario_autoriza_sigoplan != null)
                    {
                        var nom = _context.tblP_Usuario.FirstOrDefault(x => x.id == cplan.usuario_autoriza_sigoplan);

                        cplan.nombreAutoriza = nom.nombre;
                    }
                }
                #region CPLAN
                var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == cplan.id && x.clave_depto == busq.depto).ToList();
                var detPeru = _context.tblRH_BN_Incidencia_det_Peru.Where(x => x.incidenciaID == cplan.id).ToList();
                if (det.Count() > 0)
                {
                    det.ForEach(x => x.estatus = true);
                    var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidenciasPeru(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                    var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = odbc_det_empty.consulta,
                        parametros = new
                        {
                            anio = busq.anio,
                            periodo = busq.periodo,
                            fechaBajaInicio = busq.fechaInicio,
                            fechaBajaFin = busq.fechaFin,
                            cc_contable = busq.cc,
                            tipo_nomina = busq.tipoNomina,
                            clave_depto = busq.depto
                        }
                    }).ToList();

                    foreach (var item in det)
                    {
                        tblRH_BN_Incidencia_det_Peru datoPeru = detPeru.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if (datoPeru == null)
                        {
                            datoPeru = new tblRH_BN_Incidencia_det_Peru();
                            datoPeru.dia1 = 8;
                            datoPeru.dia2 = 8;
                            datoPeru.dia3 = 8;
                            datoPeru.dia4 = 8;
                            datoPeru.dia5 = 8;
                            datoPeru.dia6 = 8;
                            datoPeru.dia7 = 8;
                            datoPeru.dia8 = 8;
                            datoPeru.dia9 = 8;
                            datoPeru.dia10 = 8;
                            datoPeru.dia11 = 8;
                            datoPeru.dia12 = 8;
                            datoPeru.dia13 = 8;
                            datoPeru.dia14 = 8;
                            datoPeru.dia15 = 8;
                            datoPeru.dia16 = 8;
                            datoPeru.registroActivo = true;
                            datoPeru.incidencia_detID = item.id;
                            datoPeru.incidenciaID = item.incidenciaID;
                            datoPeru.clave_empleado = item.clave_empleado;
                        }
                        datosPeru.Add(datoPeru);
                        var clave_empleadoString = item.clave_empleado.ToString();
                        var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == item.clave_empleado);
                        item.nombre = string.Format("{0} {1} {2}", objEmpleado.ape_paterno, objEmpleado.ape_materno, objEmpleado.nombre);
                        var objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == item.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                        var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                        var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                        var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                            && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();
                        List<int> diasVacaciones = new List<int>();
                        List<int> diasVacacionesTipo = new List<int>();

                        //OBTENER CUAL ES EL PERIODO DONDE SE MOSTRARAN LOS DIAS EXTRATEMPORALES (EN ESTATUS PENDIENTE)
                        var lstIncideciasCC = _context.tblRH_BN_Incidencia.Where(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && busq.anio == x.anio).OrderByDescending(e => e.periodo).ToList();
                        var objUltimaIncidenciaPendientes = lstIncideciasCC.FirstOrDefault(e => e.estatus == "P");
                        var objUltimaIncidenciaAutorizadas = lstIncideciasCC.FirstOrDefault(e => e.estatus == "A");
                        int añoActual = DateTime.Now.Year;
                        var objPeriodoActual = _context.tblRH_EK_Periodos.Where(e => e.tipo_nomina == cplan.tipo_nomina && e.year == añoActual).ToList()
                            .FirstOrDefault(e => e.fecha_inicial.Date <= DateTime.Now.Date && e.fecha_final.Date >= DateTime.Now.Date);
                        int periodoAplicanExtratemporales = 0;

                        // SI NO TIENE INCIDENCIAS PENDIENTES SE ASIGNA AL SIGUIENTE
                        if (objUltimaIncidenciaPendientes == null)
                        {
                            var ultimaIncidencia = lstIncideciasCC.FirstOrDefault();
                            if (ultimaIncidencia != null)
                            {
                                if (ultimaIncidencia.periodo < objPeriodoActual.periodo)
                                    periodoAplicanExtratemporales = objPeriodoActual.periodo;
                                else
                                    periodoAplicanExtratemporales = ultimaIncidencia.periodo + 1;
                            }
                            else
                                periodoAplicanExtratemporales = objPeriodoActual.periodo;
                        }
                        else
                        {
                            // SI LA ULTIMA INCIDENCIA PENDIENTE NO ES LA UNICA SE LE ASIGNA A LA DEL PERIODO ACTUAL
                            if (objUltimaIncidenciaAutorizadas != null)
                                periodoAplicanExtratemporales = objUltimaIncidenciaAutorizadas.periodo + 1;
                            else
                                periodoAplicanExtratemporales = objPeriodoActual.periodo;
                        }

                        int numDiasExtratemporales = 0;
                        int numDiasExtratemporalesARestar = 0;
                        var lstFechasExtra = new List<VacFechasDTO>();

                        foreach (var itemVacaciones in vacacionesEmpleado)
                        {
                            #region DESC TIPO VACACIONES
                            string descMotivo = "";

                            switch (itemVacaciones.tipoVacaciones)
                            {
                                case 0:
                                    descMotivo = "Permiso paternidad";
                                    break;
                                case 1:
                                    descMotivo = "Permiso de matrimonio";
                                    break;
                                case 2:
                                    descMotivo = "Permiso sindical";
                                    break;
                                case 3:
                                    descMotivo = "Permiso por fallecimiento";
                                    break;
                                case 5:
                                    descMotivo = "Permiso médico";
                                    break;
                                case 7:
                                    descMotivo = "Vacaciones";
                                    break;
                                case 8:
                                    descMotivo = "Permiso SIN goce de sueldo";
                                    break;
                                case 9:
                                    descMotivo = "Permiso de comision de trabajo";
                                    break;
                                case 10:
                                    descMotivo = "Home office";
                                    break;
                                case 11:
                                    descMotivo = "Tiempo x tiempo";
                                    break;
                                case 12:
                                    descMotivo = "Incapacidades";
                                    break;
                                case 13:
                                    descMotivo = "Suspención (SUSP)";
                                    break;
                                default:
                                    descMotivo = "S/N";
                                    break;
                            }
                            #endregion

                            var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                            foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado)
                            {
                                if (cplan.estatus == "A")
                                {
                                    if (itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fechaAplicadas.Value.Date == cplan.fecha_auto.Date && itemVacacionesDetalle.fecha.Value.Date < busq.fechaInicio.Date)
                                    {
                                        numDiasExtratemporales++;

                                        //PERMISO SIN GOSE SUELDO
                                        if (itemVacacionesDetalle.tipoInsidencia == 3)
                                            numDiasExtratemporalesARestar++;

                                        var objVacFecha = new VacFechasDTO();
                                        objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                        objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                        objVacFecha.descTipoVacaciones = descMotivo;
                                        lstFechasExtra.Add(objVacFecha);
                                    }
                                }
                                else
                                {
                                    if (!itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fecha.Value.Date < busq.fechaInicio.Date && periodoAplicanExtratemporales == cplan.periodo)
                                    {
                                        numDiasExtratemporales++;

                                        //PERMISO SIN GOSE SUELDO
                                        if (itemVacacionesDetalle.tipoInsidencia == 3)
                                            numDiasExtratemporalesARestar++;

                                        var objVacFecha = new VacFechasDTO();
                                        objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                        objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                        objVacFecha.descTipoVacaciones = descMotivo;
                                        lstFechasExtra.Add(objVacFecha);
                                    }
                                }

                                if (itemVacacionesDetalle.fecha.Value.Date >= busq.fechaInicio.Date && itemVacacionesDetalle.fecha.Value.Date <= busq.fechaFin.Date)
                                {
                                    TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                    int diaVacaciones = difFechasVacaciones.Days + 1;
                                    diasVacaciones.Add(diaVacaciones);
                                    diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                                }
                            }
                        }

                        var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == item.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                            ).ToList();

                        List<int> diasIncapacidades = new List<int>();
                        foreach (var itemIncapacidad in incapacidadesEmpleado)
                        {
                            TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                            TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                            int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                            int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                            //int rango = (diasIncapacidadFin - diasIncapacidadInicio ) + 1;
                            for (int i = diasIncapacidadInicio; i <= diasIncapacidadFin; i++) diasIncapacidades.Add(i);

                            //CHECAR SI ALGUNA DE LAS INCAPACIDADES NO FUERON APLICADAS EN LAS INCIDENCIAS
                            DateTime tempFechaInicial = itemIncapacidad.fechaInicio.Date;
                            while (tempFechaInicial <= itemIncapacidad.fechaTerminacion.Date)
                            {
                                if (!itemIncapacidad.esAplicadaIncidencias && tempFechaInicial < busq.fechaInicio && periodoAplicanExtratemporales == cplan.periodo)
                                {
                                    numDiasExtratemporales++;
                                    numDiasExtratemporalesARestar++; //INCAPACIDADES

                                    var objVacFecha = new VacFechasDTO();
                                    objVacFecha.fecha = tempFechaInicial;
                                    objVacFecha.tipoVacaciones = 12;
                                    objVacFecha.descTipoVacaciones = "Incapacidades";
                                    lstFechasExtra.Add(objVacFecha);
                                }

                                tempFechaInicial = tempFechaInicial.AddDays(1);
                            }
                        }

                        item.numDiasExtratemporales = numDiasExtratemporales;
                        item.numDiasExtratemporalesARestar = numDiasExtratemporalesARestar;
                        item.lstFechasExtratemporaneas = lstFechasExtra;

                        //item.dia1 = 0;
                        if (diasVacaciones.Contains(1)) item.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                        if (diasIncapacidades.Contains(1)) item.dia1 = 10;

                        //item.dia2 = 0;
                        if (diasVacaciones.Contains(2)) item.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                        if (diasIncapacidades.Contains(2)) item.dia2 = 10;

                        //item.dia3 = 0;
                        if (diasVacaciones.Contains(3)) item.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                        if (diasIncapacidades.Contains(3)) item.dia3 = 10;

                        //item.dia4 = 0;
                        if (diasVacaciones.Contains(4)) item.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                        if (diasIncapacidades.Contains(4)) item.dia4 = 10;

                        //item.dia5 = 0;
                        if (diasVacaciones.Contains(5)) item.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                        if (diasIncapacidades.Contains(5)) item.dia5 = 10;

                        //item.dia6 = 0;
                        if (diasVacaciones.Contains(6)) item.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                        if (diasIncapacidades.Contains(6)) item.dia6 = 10;

                        //item.dia7 = 0;
                        if (diasVacaciones.Contains(7)) item.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                        if (diasIncapacidades.Contains(7)) item.dia7 = 10;

                        //item.dia8 = 0;
                        if (diasVacaciones.Contains(8)) item.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                        if (diasIncapacidades.Contains(8)) item.dia8 = 10;

                        //item.dia9 = 0;
                        if (diasVacaciones.Contains(9)) item.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                        if (diasIncapacidades.Contains(9)) item.dia9 = 10;

                        //item.dia10 = 0;
                        if (diasVacaciones.Contains(10)) item.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                        if (diasIncapacidades.Contains(10)) item.dia10 = 10;

                        //item.dia11 = 0;
                        if (diasVacaciones.Contains(11)) item.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                        if (diasIncapacidades.Contains(11)) item.dia11 = 10;

                        //item.dia12 = 0;
                        if (diasVacaciones.Contains(12)) item.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                        if (diasIncapacidades.Contains(12)) item.dia12 = 10;

                        //item.dia13 = 0;
                        if (diasVacaciones.Contains(13)) item.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                        if (diasIncapacidades.Contains(13)) item.dia13 = 10;

                        //item.dia14 = 0;
                        if (diasVacaciones.Contains(14)) item.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                        if (diasIncapacidades.Contains(14)) item.dia14 = 10;

                        //item.dia15 = 0;
                        if (diasVacaciones.Contains(15)) item.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                        if (diasIncapacidades.Contains(15)) item.dia15 = 10;

                        //item.dia16 = 0;
                        if (diasVacaciones.Contains(16)) item.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                        if (diasIncapacidades.Contains(16)) item.dia16 = 10;
                    }

                    // Si no existe actualmente en el guardado, dejarlo como pendiente de guardado
                    foreach (var item in det_empty)
                    {
                        var auxDetalle = det.FirstOrDefault(x => x.clave_empleado == item.clave_empleado);
                        if (auxDetalle == null)
                        {
                            item.estatus = false;
                            det.Add(item);
                        }
                    }

                    if (cplan.estatus != "A")
                    {
                        var bonos = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == busq.anio && x.cc == busq.cc && x.periodo == busq.periodo && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                        if (bonos != null)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in det)
                            {
                                var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                if (hasBono != null)
                                {
                                    i.bonoDM = hasBono.monto_Asig;
                                    i.evaluacion_detID = hasBono.id;
                                }
                            }
                        }
                    }
                }
                else
                {

                    var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidenciasPeru(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                    //var det_empty = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det_empty);

                    var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                    {
                        baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                        consulta = odbc_det_empty.consulta,
                        parametros = new
                        {
                            anio = busq.anio,
                            periodo = busq.periodo,
                            fechaBajaInicio = busq.fechaInicio,
                            fechaBajaFin = busq.fechaFin,
                            cc_contable = busq.cc,
                            tipo_nomina = busq.tipoNomina,
                            clave_depto = busq.depto
                        }
                    }).ToList();

                    var bonos = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == busq.anio && x.cc == busq.cc && x.periodo == busq.periodo && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                    var data = det_empty.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                    foreach (var x in data)
                    {
                        tblRH_BN_Incidencia_det_Peru datoPeru = new tblRH_BN_Incidencia_det_Peru();
                        datoPeru.dia1 = 8;
                        datoPeru.dia2 = 8;
                        datoPeru.dia3 = 8;
                        datoPeru.dia4 = 8;
                        datoPeru.dia5 = 8;
                        datoPeru.dia6 = 8;
                        datoPeru.dia7 = 8;
                        datoPeru.dia8 = 8;
                        datoPeru.dia9 = 8;
                        datoPeru.dia10 = 8;
                        datoPeru.dia11 = 8;
                        datoPeru.dia12 = 8;
                        datoPeru.dia13 = 8;
                        datoPeru.dia14 = 8;
                        datoPeru.dia15 = 8;
                        datoPeru.dia16 = 8;
                        datoPeru.registroActivo = true;
                        datoPeru.incidencia_detID = 0;
                        datoPeru.incidenciaID = 0;
                        datoPeru.clave_empleado = x.clave_empleado;
                        datosPeru.Add(datoPeru);
                        var clave_empleadoString = x.clave_empleado.ToString();
                        var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == x.clave_empleado);
                        var objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == x.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                        var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                        var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                        var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                            && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                            && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();
                        List<int> diasVacaciones = new List<int>();
                        List<int> diasVacacionesTipo = new List<int>();

                        foreach (var itemVacaciones in vacacionesEmpleado)
                        {
                            var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                            foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado)
                            {
                                if (itemVacacionesDetalle.fecha.Value.Date >= busq.fechaInicio.Date && itemVacacionesDetalle.fecha.Value.Date <= busq.fechaFin.Date)
                                {
                                    TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                    int diaVacaciones = difFechasVacaciones.Days + 1;
                                    diasVacaciones.Add(diaVacaciones);
                                    diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                                }
                            }
                        }

                        //var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado).ToList();
                        var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                        && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                        ).ToList();

                        List<int> diasIncapacidades = new List<int>();
                        foreach (var itemIncapacidad in incapacidadesEmpleado)
                        {
                            TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                            TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                            int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                            int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                            for (int i = diasIncapacidadInicio; i <= (diasIncapacidadFin); i++) diasIncapacidades.Add(i);
                        }

                        if (x.fechaAlta > busq.fechaInicio && !x.isBaja)
                        {
                            TimeSpan difFechas = x.fechaAlta - busq.fechaInicio;
                            int dias = difFechas.Days;

                            if (dias >= 1)
                            {
                                x.dia1 = 13;
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;
                            }
                            if (dias >= 2)
                            {
                                x.dia2 = 13;
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;
                            }
                            if (dias >= 3)
                            {
                                x.dia3 = 13;
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;
                            }
                            if (dias >= 4)
                            {
                                x.dia4 = 13;
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;
                            }
                            if (dias >= 5)
                            {
                                x.dia5 = 13;
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;
                            }
                            if (dias >= 6)
                            {
                                x.dia6 = 13;
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;
                            }
                            if (dias >= 7)
                            {
                                x.dia7 = 13;
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;
                            }
                            if (dias >= 8)
                            {
                                x.dia8 = 13;
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;
                            }
                            if (dias >= 9)
                            {
                                x.dia9 = 13;
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;
                            }
                            if (dias >= 10)
                            {
                                x.dia10 = 13;
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;
                            }
                            if (dias >= 11)
                            {
                                x.dia11 = 13;
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;
                            }
                            if (dias >= 12)
                            {
                                x.dia12 = 13;
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;
                            }
                            if (dias >= 13)
                            {
                                x.dia13 = 13;
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;
                            }
                            if (dias >= 14)
                            {
                                x.dia14 = 13;
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;
                            }
                            if (dias >= 15)
                            {
                                x.dia15 = 13;
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;
                            }
                            if (dias >= 16)
                            {
                                x.dia16 = 13;
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                        }
                        else if (x.isBaja && x.fechaAlta <= busq.fechaInicio)
                        {
                            TimeSpan difFechas = x.fechaBaja - busq.fechaInicio;
                            int diasBaja = difFechas.Days;
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            else
                            {
                                x.dia1 = 0;
                                if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                                if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                                x.dia2 = 0;
                                if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                                if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                                x.dia3 = 0;
                                if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                                if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                                x.dia4 = 0;
                                if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                                if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                                x.dia5 = 0;
                                if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                                if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                                x.dia6 = 0;
                                if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                                if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                                x.dia7 = 0;
                                if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                                if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                                x.dia8 = 0;
                                if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                                if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                                x.dia9 = 0;
                                if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                                if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                                x.dia10 = 0;
                                if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                                if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                                x.dia11 = 0;
                                if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                                if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                                x.dia12 = 0;
                                if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                                if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                                x.dia13 = 0;
                                if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                                if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                                x.dia14 = 0;
                                if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                                if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                                x.dia15 = 0;
                                if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                                if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                                x.dia16 = 0;
                                if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                                if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                            }
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 18;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 18;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 18;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 18;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 18;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 18;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 18;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 18;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 18;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 18;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 18;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 18;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 18;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 18;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 18;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 18;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            else
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 18;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 18;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 18;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 18;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 18;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 18;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 18;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 18;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 18;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 18;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 18;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 18;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 18;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 18;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 18;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 18;
                                }
                            }
                        }
                        else if ((x.fechaAlta > busq.fechaInicio && x.isBaja))
                        {
                            TimeSpan difFechasAlta = x.fechaAlta - busq.fechaInicio;
                            TimeSpan difFechasActivo = x.fechaBaja - x.fechaAlta;
                            int diasAlta = difFechasAlta.Days;
                            int diasActivo = difFechasActivo.Days;
                            int diasBaja = diasAlta + diasActivo;
                            #region Dias No aplica
                            if (diasAlta >= 1)
                            {
                                x.dia1 = 13;
                            }
                            if (diasAlta >= 2)
                            {
                                x.dia2 = 13;
                            }
                            if (diasAlta >= 3)
                            {
                                x.dia3 = 13;
                            }
                            if (diasAlta >= 4)
                            {
                                x.dia4 = 13;
                            }
                            if (diasAlta >= 5)
                            {
                                x.dia5 = 13;
                            }
                            if (diasAlta >= 6)
                            {
                                x.dia6 = 13;
                            }
                            if (diasAlta >= 7)
                            {
                                x.dia7 = 13;
                            }
                            if (diasAlta >= 8)
                            {
                                x.dia8 = 13;
                            }
                            if (diasAlta >= 9)
                            {
                                x.dia9 = 13;
                            }
                            if (diasAlta >= 10)
                            {
                                x.dia10 = 13;
                            }
                            if (diasAlta >= 11)
                            {
                                x.dia11 = 13;
                            }
                            if (diasAlta >= 12)
                            {
                                x.dia12 = 13;
                            }
                            if (diasAlta >= 13)
                            {
                                x.dia13 = 13;
                            }
                            if (diasAlta >= 14)
                            {
                                x.dia14 = 13;
                            }
                            if (diasAlta >= 15)
                            {
                                x.dia15 = 13;
                            }
                            if (diasAlta >= 16)
                            {
                                x.dia16 = 13;
                            }
                            #endregion
                            #region Dias Baja
                            if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 18;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 18;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 18;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 18;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 18;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 18;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 18;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 18;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 18;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 18;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 18;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 18;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 18;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 18;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 18;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 18;
                                }
                            }
                            else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 16;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 16;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 16;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 16;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 16;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 16;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 16;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 16;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 16;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 16;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 16;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 16;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 16;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 16;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 16;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 16;
                                }
                            }
                            else
                            {
                                if (diasBaja < 1)
                                {
                                    x.dia1 = 18;
                                }
                                if (diasBaja < 2)
                                {
                                    x.dia2 = 18;
                                }
                                if (diasBaja < 3)
                                {
                                    x.dia3 = 18;
                                }
                                if (diasBaja < 4)
                                {
                                    x.dia4 = 18;
                                }
                                if (diasBaja < 5)
                                {
                                    x.dia5 = 18;
                                }
                                if (diasBaja < 6)
                                {
                                    x.dia6 = 18;
                                }
                                if (diasBaja < 7)
                                {
                                    x.dia7 = 18;
                                }
                                if (diasBaja < 8)
                                {
                                    x.dia8 = 18;
                                }
                                if (diasBaja < 9)
                                {
                                    x.dia9 = 18;
                                }
                                if (diasBaja < 10)
                                {
                                    x.dia10 = 18;
                                }
                                if (diasBaja < 11)
                                {
                                    x.dia11 = 18;
                                }
                                if (diasBaja < 12)
                                {
                                    x.dia12 = 18;
                                }
                                if (diasBaja < 13)
                                {
                                    x.dia13 = 18;
                                }
                                if (diasBaja < 14)
                                {
                                    x.dia14 = 18;
                                }
                                if (diasBaja < 15)
                                {
                                    x.dia15 = 18;
                                }
                                if (diasBaja < 16)
                                {
                                    x.dia16 = 18;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        x.primaDominical = false;
                        x.dias_extra_concepto = 0;
                    }

                    if (cplan.estatus != "A")
                    {
                        if (bonos != null)
                        {
                            //empty.evaluacionID = bonos.id;
                            foreach (var i in data)
                            {
                                var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                                if (hasBono != null)
                                {
                                    i.bonoDM = hasBono.monto_Asig;
                                    i.evaluacion_detID = hasBono.id;
                                }
                            }
                        }
                    }
                    det = data;
                    det.ForEach(x => x.estatus = false);
                    result.incidencia_det = det;
                }
                result.incidencia = cplan;
                result.incidencia_det = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                result.isAuth = permiso;
                var isDesauth = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.cc.Equals("*") && x.autoriza);
                result.isDesauth = isDesauth;
                result.permiso_bono_sinlimite = permiso_bono_sinlimite;
                #endregion
            }
            else
            {
                #region Enkontrol
                //var odbc_main = new OdbcConsultaDTO() { consulta = queryLstIncidenciasEnk(), parametros = paramLstIncidenciasEnk(busq) };
                //var main = _contextEnkontrol.Select<tblRH_BN_Incidencia>(EnkontrolAmbienteEnum.Rh, odbc_main);
                //if (main.Count() > 0)
                //{
                //    var odbc_det = new OdbcConsultaDTO()
                //    {
                //        consulta = queryLstIncidencias_det_Enk(),
                //        parametros = paramLstIncidencias_det_Enk(busq)
                //    };
                //    var det = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det);
                //    var claves = det.Select(x => x.clave_empleado).ToList();
                //    det.ForEach(x =>
                //        {
                //            x.horas_extras = (x.he_dia1 + x.he_dia2 + x.he_dia3 + x.he_dia4 + x.he_dia5 + x.he_dia6 + x.he_dia7 + x.he_dia8 + x.he_dia9 + x.he_dia10 + x.he_dia11 + x.he_dia12 + x.he_dia13 + x.he_dia14 + x.he_dia15 + x.he_dia16);
                //            x.primaDominical = false;
                //        });
                //    var data = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();

                //    result.incidencia = main.FirstOrDefault();
                //    result.incidencia_det = data;
                //    var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                //    var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                //    result.isAuth = permiso;
                //    result.permiso_bono_sinlimite = permiso_bono_sinlimite;
                //    return result;
                //}
                //else
                //{
                var emp = _context.tblP_Usuario_Enkontrol.FirstOrDefault(x => x.idUsuario == vSesiones.sesionUsuarioDTO.id);
                var empty = new tblRH_BN_Incidencia();
                empty.id_incidencia = 0;
                empty.usuarioID = vSesiones.sesionUsuarioDTO.id;
                empty.anio = busq.anio;
                empty.periodo = busq.periodo;
                empty.cc = busq.cc;
                empty.tipo_nomina = busq.tipoNomina;
                empty.estatus = "P";
                empty.estatusDesc = "PENDIENTE";
                empty.fecha_auto = DateTime.Now;
                empty.fecha_modifica = DateTime.Now;
                if (emp != null)
                {
                    //empty.empleado_modifica = emp.empleado;
                    //empty.nombreEmpMod = getNombreUsuarioEnk(emp.empleado);
                    empty.empleado_modifica = 1;
                    empty.nombreEmpMod = "ADMINISTRADOR";
                }
                else
                {
                    empty.empleado_modifica = 1;
                    empty.nombreEmpMod = "ADMINISTRADOR";
                }


                var odbc_det_empty = new OdbcConsultaDTO() { consulta = queryLstIncidenciasPeru(), parametros = paramLstIncidencias_det_EnkEmptyNew(busq) };
                //var det_empty = _contextEnkontrol.Select<tblRH_BN_Incidencia_det>(EnkontrolAmbienteEnum.Rh, odbc_det_empty);

                var det_empty = _context.Select<tblRH_BN_Incidencia_det>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    consulta = odbc_det_empty.consulta,
                    parametros = new
                    {
                        anio = busq.anio,
                        periodo = busq.periodo,
                        fechaBajaInicio = busq.fechaInicio,
                        fechaBajaFin = busq.fechaFin,
                        cc_contable = busq.cc,
                        tipo_nomina = busq.tipoNomina,
                        clave_depto = busq.depto
                    }
                }).ToList();

                var bonos = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == busq.anio && x.cc == busq.cc && x.periodo == busq.periodo && x.tipoNomina == busq.tipoNomina && x.estatus == (int)authEstadoEnum.Autorizado);
                var data = det_empty.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                foreach (var x in data)
                {
                    tblRH_BN_Incidencia_det_Peru datoPeru = new tblRH_BN_Incidencia_det_Peru();
                    datoPeru.dia1 = 8;
                    datoPeru.dia2 = 8;
                    datoPeru.dia3 = 8;
                    datoPeru.dia4 = 8;
                    datoPeru.dia5 = 8;
                    datoPeru.dia6 = 8;
                    datoPeru.dia7 = 8;
                    datoPeru.dia8 = 8;
                    datoPeru.dia9 = 8;
                    datoPeru.dia10 = 8;
                    datoPeru.dia11 = 8;
                    datoPeru.dia12 = 8;
                    datoPeru.dia13 = 8;
                    datoPeru.dia14 = 8;
                    datoPeru.dia15 = 8;
                    datoPeru.dia16 = 8;
                    datoPeru.registroActivo = true;
                    datoPeru.incidencia_detID = 0;
                    datoPeru.incidenciaID = 0;
                    datoPeru.clave_empleado = x.clave_empleado;
                    datosPeru.Add(datoPeru);

                    var clave_empleadoString = x.clave_empleado.ToString();
                    var objEmpleado = _context.tblRH_EK_Empleados.FirstOrDefault(e => e.clave_empleado == x.clave_empleado);
                    var _objBajaEmpleado = _context.tblRH_Baja_Registro.Where(e => e.registroActivo && e.numeroEmpleado == x.clave_empleado && e.est_baja == "A" && e.est_contabilidad == "A").ToList();
                    tblRH_Baja_Registro objBajaEmpleado = null;
                    if (_objBajaEmpleado.Count() > 0) objBajaEmpleado = _objBajaEmpleado.OrderByDescending(e => e.fechaBaja).FirstOrDefault();
                    var vacacionesEmpleado = vacaciones.Where(y => y.claveEmpleado == clave_empleadoString).ToList();
                    var vacacionesEmpleadoIDs = vacaciones.Select(y => y.id).ToList();
                    var vacacionesDetalleEmpleado = _context.tblRH_Vacaciones_Fechas.Where(y => vacacionesEmpleadoIDs.Contains(y.vacacionID)).ToList().Where(y => y.registroActivo
                        && objEmpleado.fecha_antiguedad.Value.Date <= y.fecha.Value.Date
                        && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fecha.Value.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)).ToList();
                    List<int> diasVacaciones = new List<int>();
                    List<int> diasVacacionesTipo = new List<int>();

                    //OBTENER CUAL ES EL PERIODO DONDE SE MOSTRARAN LOS DIAS EXTRATEMPORALES (EN ESTATUS PENDIENTE)
                    var lstIncideciasCC = _context.tblRH_BN_Incidencia.Where(e => e.cc == busq.cc && e.tipo_nomina == busq.tipoNomina && busq.anio == e.anio).OrderByDescending(e => e.periodo).ToList();
                    var objUltimaIncidenciaPendientes = lstIncideciasCC.FirstOrDefault(e => e.estatus == "P");
                    var objUltimaIncidenciaAutorizadas = lstIncideciasCC.FirstOrDefault(e => e.estatus == "A");
                    int añoActual = DateTime.Now.Year;
                    var objPeriodoActual = _context.tblRH_EK_Periodos.Where(e => e.tipo_nomina == busq.tipoNomina && e.year == añoActual).ToList()
                        .FirstOrDefault(e => e.fecha_inicial.Date <= DateTime.Now.Date && e.fecha_final.Date >= DateTime.Now.Date);
                    int periodoAplicanExtratemporales = 0;

                    // SI NO TIENE INCIDENCIAS PENDIENTES SE ASIGNA AL SIGUIENTE
                    if (objUltimaIncidenciaPendientes == null)
                    {
                        var ultimaIncidencia = lstIncideciasCC.FirstOrDefault();

                        if (ultimaIncidencia != null)
                        {
                            if (ultimaIncidencia.periodo < objPeriodoActual.periodo)
                            {
                                periodoAplicanExtratemporales = objPeriodoActual.periodo;

                            }
                            else
                            {
                                periodoAplicanExtratemporales = ultimaIncidencia.periodo + 1;

                            }
                        }
                        else
                        {
                            periodoAplicanExtratemporales = objPeriodoActual.periodo;
                        }

                    }
                    else
                    {

                        // SI LA ULTIMA INCIDENCIA PENDIENTE NO ES LA UNICA SE LE ASIGNA A LA DEL PERIODO ACTUAL
                        if (objUltimaIncidenciaAutorizadas != null)
                        {
                            periodoAplicanExtratemporales = objUltimaIncidenciaAutorizadas.periodo + 1;

                        }
                        else
                        {
                            periodoAplicanExtratemporales = objPeriodoActual.periodo;

                        }
                    }

                    int numDiasExtratemporales = 0;
                    int numDiasExtratemporalesARestar = 0;
                    var lstFechasExtra = new List<VacFechasDTO>();

                    foreach (var itemVacaciones in vacacionesEmpleado)
                    {
                        #region DESC TIPO VACACIONES
                        string descMotivo = "";

                        switch (itemVacaciones.tipoVacaciones)
                        {
                            case 0:
                                descMotivo = "Permiso paternidad";
                                break;
                            case 1:
                                descMotivo = "Permiso de matrimonio";
                                break;
                            case 2:
                                descMotivo = "Permiso sindical";
                                break;
                            case 3:
                                descMotivo = "Permiso por fallecimiento";
                                break;
                            case 5:
                                descMotivo = "Permiso médico";
                                break;
                            case 7:
                                descMotivo = "Vacaciones";
                                break;
                            case 8:
                                descMotivo = "Permiso SIN goce de sueldo";
                                break;
                            case 9:
                                descMotivo = "Permiso de comision de trabajo";
                                break;
                            case 10:
                                descMotivo = ">Home office";
                                break;
                            case 11:
                                descMotivo = ">Tiempo x tiempo";
                                break;
                            case 12:
                                descMotivo = "Incapacidades";
                                break;
                            case 13:
                                descMotivo = "Suspención (SUSP)";
                                break;
                            default:
                                descMotivo = "S/N";
                                break;
                        }
                        #endregion

                        var auxVacacionesDetalleEmpleado = vacacionesDetalleEmpleado.Where(y => y.vacacionID == itemVacaciones.id).ToList();
                        foreach (var itemVacacionesDetalle in auxVacacionesDetalleEmpleado)
                        {

                            if (!itemVacacionesDetalle.esAplicadaIncidencias && itemVacacionesDetalle.fecha < busq.fechaInicio && periodoAplicanExtratemporales == busq.periodo)
                            {
                                numDiasExtratemporales++;

                                //PERMISO SIN GOSE 
                                if (itemVacacionesDetalle.tipoInsidencia == 3)
                                {
                                    numDiasExtratemporalesARestar++;
                                }

                                var objVacFecha = new VacFechasDTO();
                                objVacFecha.fecha = itemVacacionesDetalle.fecha;
                                objVacFecha.tipoVacaciones = itemVacaciones.tipoVacaciones;
                                objVacFecha.descTipoVacaciones = descMotivo;
                                lstFechasExtra.Add(objVacFecha);
                            }

                            if (itemVacacionesDetalle.fecha.Value.Date >= busq.fechaInicio.Date && itemVacacionesDetalle.fecha.Value.Date <= busq.fechaFin.Date)
                            {
                                TimeSpan difFechasVacaciones = (itemVacacionesDetalle.fecha ?? DateTime.Today) - busq.fechaInicio;
                                int diaVacaciones = difFechasVacaciones.Days + 1;
                                diasVacaciones.Add(diaVacaciones);
                                diasVacacionesTipo.Add(itemVacacionesDetalle.tipoInsidencia);
                            }
                        }
                    }


                    //var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado).ToList();
                    var incapacidadesEmpleado = incapacidades.Where(y => y.clave_empleado == x.clave_empleado && objEmpleado.fecha_antiguedad.Value.Date <= y.fechaInicio.Date
                        && ((objBajaEmpleado != null && objEmpleado.estatus_empleado == "B") ? y.fechaTerminacion.Date <= objBajaEmpleado.fechaBaja.Value.Date : true)
                        ).ToList();
                    List<int> diasIncapacidades = new List<int>();
                    foreach (var itemIncapacidad in incapacidadesEmpleado)
                    {
                        TimeSpan difFechasIncapacidadInicio = itemIncapacidad.fechaInicio >= busq.fechaInicio ? itemIncapacidad.fechaInicio - busq.fechaInicio : busq.fechaInicio - busq.fechaInicio;
                        TimeSpan difFechasIncapacidadFin = itemIncapacidad.fechaTerminacion <= busq.fechaFin ? itemIncapacidad.fechaTerminacion - busq.fechaInicio : busq.fechaFin - busq.fechaInicio;
                        int diasIncapacidadInicio = difFechasIncapacidadInicio.Days + 1;
                        int diasIncapacidadFin = difFechasIncapacidadFin.Days + 1;
                        for (int i = diasIncapacidadInicio; i <= (diasIncapacidadFin); i++) diasIncapacidades.Add(i);

                        //CHECAR SI ALGUNA DE LAS INCAPACIDADES NO FUERON APLICADAS EN LAS INCIDENCIAS
                        DateTime tempFechaInicial = itemIncapacidad.fechaInicio.Date;
                        while (tempFechaInicial <= itemIncapacidad.fechaTerminacion.Date)
                        {
                            if (!itemIncapacidad.esAplicadaIncidencias && tempFechaInicial < busq.fechaInicio && periodoAplicanExtratemporales == busq.periodo)
                            {
                                numDiasExtratemporales++;
                                numDiasExtratemporalesARestar++; //INCAPACIDADES

                                var objVacFecha = new VacFechasDTO();
                                objVacFecha.fecha = tempFechaInicial;
                                objVacFecha.tipoVacaciones = 12;
                                objVacFecha.descTipoVacaciones = "Incapacidades";
                                lstFechasExtra.Add(objVacFecha);
                            }

                            tempFechaInicial = tempFechaInicial.AddDays(1);
                        }
                    }

                    //DIAS EXTRA TEMPORALES
                    x.numDiasExtratemporales = numDiasExtratemporales;
                    x.numDiasExtratemporalesARestar = numDiasExtratemporalesARestar;
                    x.lstFechasExtratemporaneas = lstFechasExtra;

                    if (x.fechaAlta > busq.fechaInicio && !x.isBaja)
                    {
                        TimeSpan difFechas = x.fechaAlta - busq.fechaInicio;
                        int dias = difFechas.Days;

                        if (dias >= 1)
                        {
                            x.dia1 = 13;
                        }
                        if (dias >= 2)
                        {
                            x.dia2 = 13;
                        }
                        if (dias >= 3)
                        {
                            x.dia3 = 13;
                        }
                        if (dias >= 4)
                        {
                            x.dia4 = 13;
                        }
                        if (dias >= 5)
                        {
                            x.dia5 = 13;
                        }
                        if (dias >= 6)
                        {
                            x.dia6 = 13;
                        }
                        if (dias >= 7)
                        {
                            x.dia7 = 13;
                        }
                        if (dias >= 8)
                        {
                            x.dia8 = 13;
                        }
                        if (dias >= 9)
                        {
                            x.dia9 = 13;
                        }
                        if (dias >= 10)
                        {
                            x.dia10 = 13;
                        }
                        if (dias >= 11)
                        {
                            x.dia11 = 13;
                        }
                        if (dias >= 12)
                        {
                            x.dia12 = 13;
                        }
                        if (dias >= 13)
                        {
                            x.dia13 = 13;
                        }
                        if (dias >= 14)
                        {
                            x.dia14 = 13;
                        }
                        if (dias >= 15)
                        {
                            x.dia15 = 13;
                        }
                        if (dias >= 16)
                        {
                            x.dia16 = 13;
                        }
                    }
                    else if (x.isBaja && x.fechaAlta <= busq.fechaInicio)
                    {
                        TimeSpan difFechas = x.fechaBaja - busq.fechaInicio;
                        int diasBaja = difFechas.Days;
                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        else
                        {
                            x.dia1 = 0;
                            if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                            if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                            x.dia2 = 0;
                            if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                            if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                            x.dia3 = 0;
                            if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                            if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                            x.dia4 = 0;
                            if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                            if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                            x.dia5 = 0;
                            if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                            if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                            x.dia6 = 0;
                            if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                            if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                            x.dia7 = 0;
                            if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                            if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                            x.dia8 = 0;
                            if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                            if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                            x.dia9 = 0;
                            if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                            if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                            x.dia10 = 0;
                            if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                            if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                            x.dia11 = 0;
                            if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                            if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                            x.dia12 = 0;
                            if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                            if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                            x.dia13 = 0;
                            if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                            if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                            x.dia14 = 0;
                            if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                            if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                            x.dia15 = 0;
                            if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                            if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                            x.dia16 = 0;
                            if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                            if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                        }
                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            if (diasBaja < 1)
                            {
                                x.dia1 = 18;
                            }
                            if (diasBaja < 2)
                            {
                                x.dia2 = 18;
                            }
                            if (diasBaja < 3)
                            {
                                x.dia3 = 18;
                            }
                            if (diasBaja < 4)
                            {
                                x.dia4 = 18;
                            }
                            if (diasBaja < 5)
                            {
                                x.dia5 = 18;
                            }
                            if (diasBaja < 6)
                            {
                                x.dia6 = 18;
                            }
                            if (diasBaja < 7)
                            {
                                x.dia7 = 18;
                            }
                            if (diasBaja < 8)
                            {
                                x.dia8 = 18;
                            }
                            if (diasBaja < 9)
                            {
                                x.dia9 = 18;
                            }
                            if (diasBaja < 10)
                            {
                                x.dia10 = 18;
                            }
                            if (diasBaja < 11)
                            {
                                x.dia11 = 18;
                            }
                            if (diasBaja < 12)
                            {
                                x.dia12 = 18;
                            }
                            if (diasBaja < 13)
                            {
                                x.dia13 = 18;
                            }
                            if (diasBaja < 14)
                            {
                                x.dia14 = 18;
                            }
                            if (diasBaja < 15)
                            {
                                x.dia15 = 18;
                            }
                            if (diasBaja < 16)
                            {
                                x.dia16 = 18;
                            }
                        }
                        else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        {
                            if (diasBaja < 1)
                            {
                                x.dia1 = 16;
                            }
                            if (diasBaja < 2)
                            {
                                x.dia2 = 16;
                            }
                            if (diasBaja < 3)
                            {
                                x.dia3 = 16;
                            }
                            if (diasBaja < 4)
                            {
                                x.dia4 = 16;
                            }
                            if (diasBaja < 5)
                            {
                                x.dia5 = 16;
                            }
                            if (diasBaja < 6)
                            {
                                x.dia6 = 16;
                            }
                            if (diasBaja < 7)
                            {
                                x.dia7 = 16;
                            }
                            if (diasBaja < 8)
                            {
                                x.dia8 = 16;
                            }
                            if (diasBaja < 9)
                            {
                                x.dia9 = 16;
                            }
                            if (diasBaja < 10)
                            {
                                x.dia10 = 16;
                            }
                            if (diasBaja < 11)
                            {
                                x.dia11 = 16;
                            }
                            if (diasBaja < 12)
                            {
                                x.dia12 = 16;
                            }
                            if (diasBaja < 13)
                            {
                                x.dia13 = 16;
                            }
                            if (diasBaja < 14)
                            {
                                x.dia14 = 16;
                            }
                            if (diasBaja < 15)
                            {
                                x.dia15 = 16;
                            }
                            if (diasBaja < 16)
                            {
                                x.dia16 = 16;
                            }
                        }
                        else
                        {
                            if (diasBaja < 1)
                            {
                                x.dia1 = 18;
                            }
                            if (diasBaja < 2)
                            {
                                x.dia2 = 18;
                            }
                            if (diasBaja < 3)
                            {
                                x.dia3 = 18;
                            }
                            if (diasBaja < 4)
                            {
                                x.dia4 = 18;
                            }
                            if (diasBaja < 5)
                            {
                                x.dia5 = 18;
                            }
                            if (diasBaja < 6)
                            {
                                x.dia6 = 18;
                            }
                            if (diasBaja < 7)
                            {
                                x.dia7 = 18;
                            }
                            if (diasBaja < 8)
                            {
                                x.dia8 = 18;
                            }
                            if (diasBaja < 9)
                            {
                                x.dia9 = 18;
                            }
                            if (diasBaja < 10)
                            {
                                x.dia10 = 18;
                            }
                            if (diasBaja < 11)
                            {
                                x.dia11 = 18;
                            }
                            if (diasBaja < 12)
                            {
                                x.dia12 = 18;
                            }
                            if (diasBaja < 13)
                            {
                                x.dia13 = 18;
                            }
                            if (diasBaja < 14)
                            {
                                x.dia14 = 18;
                            }
                            if (diasBaja < 15)
                            {
                                x.dia15 = 18;
                            }
                            if (diasBaja < 16)
                            {
                                x.dia16 = 18;
                            }
                        }
                    }
                    else if ((x.fechaAlta > busq.fechaInicio && x.isBaja))
                    {
                        TimeSpan difFechasAlta = x.fechaAlta - busq.fechaInicio;
                        TimeSpan difFechasActivo = x.fechaBaja - x.fechaAlta;
                        int diasAlta = difFechasAlta.Days;
                        int diasActivo = difFechasActivo.Days;
                        int diasBaja = diasAlta + diasActivo;
                        #region Dias No aplica
                        if (diasAlta >= 1)
                        {
                            x.dia1 = 13;
                        }
                        if (diasAlta >= 2)
                        {
                            x.dia2 = 13;
                        }
                        if (diasAlta >= 3)
                        {
                            x.dia3 = 13;
                        }
                        if (diasAlta >= 4)
                        {
                            x.dia4 = 13;
                        }
                        if (diasAlta >= 5)
                        {
                            x.dia5 = 13;
                        }
                        if (diasAlta >= 6)
                        {
                            x.dia6 = 13;
                        }
                        if (diasAlta >= 7)
                        {
                            x.dia7 = 13;
                        }
                        if (diasAlta >= 8)
                        {
                            x.dia8 = 13;
                        }
                        if (diasAlta >= 9)
                        {
                            x.dia9 = 13;
                        }
                        if (diasAlta >= 10)
                        {
                            x.dia10 = 13;
                        }
                        if (diasAlta >= 11)
                        {
                            x.dia11 = 13;
                        }
                        if (diasAlta >= 12)
                        {
                            x.dia12 = 13;
                        }
                        if (diasAlta >= 13)
                        {
                            x.dia13 = 13;
                        }
                        if (diasAlta >= 14)
                        {
                            x.dia14 = 13;
                        }
                        if (diasAlta >= 15)
                        {
                            x.dia15 = 13;
                        }
                        if (diasAlta >= 16)
                        {
                            x.dia16 = 13;
                        }
                        #endregion
                        #region Dias Baja
                        if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Construplan)
                        {
                            if (diasBaja < 1)
                            {
                                x.dia1 = 18;
                            }
                            if (diasBaja < 2)
                            {
                                x.dia2 = 18;
                            }
                            if (diasBaja < 3)
                            {
                                x.dia3 = 18;
                            }
                            if (diasBaja < 4)
                            {
                                x.dia4 = 18;
                            }
                            if (diasBaja < 5)
                            {
                                x.dia5 = 18;
                            }
                            if (diasBaja < 6)
                            {
                                x.dia6 = 18;
                            }
                            if (diasBaja < 7)
                            {
                                x.dia7 = 18;
                            }
                            if (diasBaja < 8)
                            {
                                x.dia8 = 18;
                            }
                            if (diasBaja < 9)
                            {
                                x.dia9 = 18;
                            }
                            if (diasBaja < 10)
                            {
                                x.dia10 = 18;
                            }
                            if (diasBaja < 11)
                            {
                                x.dia11 = 18;
                            }
                            if (diasBaja < 12)
                            {
                                x.dia12 = 18;
                            }
                            if (diasBaja < 13)
                            {
                                x.dia13 = 18;
                            }
                            if (diasBaja < 14)
                            {
                                x.dia14 = 18;
                            }
                            if (diasBaja < 15)
                            {
                                x.dia15 = 18;
                            }
                            if (diasBaja < 16)
                            {
                                x.dia16 = 18;
                            }
                        }
                        else if (vSesiones.sesionEmpresaActual == (int)EmpresaEnum.Arrendadora)
                        {
                            if (diasBaja < 1)
                            {
                                x.dia1 = 16;
                            }
                            if (diasBaja < 2)
                            {
                                x.dia2 = 16;
                            }
                            if (diasBaja < 3)
                            {
                                x.dia3 = 16;
                            }
                            if (diasBaja < 4)
                            {
                                x.dia4 = 16;
                            }
                            if (diasBaja < 5)
                            {
                                x.dia5 = 16;
                            }
                            if (diasBaja < 6)
                            {
                                x.dia6 = 16;
                            }
                            if (diasBaja < 7)
                            {
                                x.dia7 = 16;
                            }
                            if (diasBaja < 8)
                            {
                                x.dia8 = 16;
                            }
                            if (diasBaja < 9)
                            {
                                x.dia9 = 16;
                            }
                            if (diasBaja < 10)
                            {
                                x.dia10 = 16;
                            }
                            if (diasBaja < 11)
                            {
                                x.dia11 = 16;
                            }
                            if (diasBaja < 12)
                            {
                                x.dia12 = 16;
                            }
                            if (diasBaja < 13)
                            {
                                x.dia13 = 16;
                            }
                            if (diasBaja < 14)
                            {
                                x.dia14 = 16;
                            }
                            if (diasBaja < 15)
                            {
                                x.dia15 = 16;
                            }
                            if (diasBaja < 16)
                            {
                                x.dia16 = 16;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        x.dia1 = 0;
                        if (diasVacaciones.Contains(1)) x.dia1 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 1)];
                        if (diasIncapacidades.Contains(1)) x.dia1 = 10;

                        x.dia2 = 0;
                        if (diasVacaciones.Contains(2)) x.dia2 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 2)];
                        if (diasIncapacidades.Contains(2)) x.dia2 = 10;

                        x.dia3 = 0;
                        if (diasVacaciones.Contains(3)) x.dia3 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 3)];
                        if (diasIncapacidades.Contains(3)) x.dia3 = 10;

                        x.dia4 = 0;
                        if (diasVacaciones.Contains(4)) x.dia4 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 4)];
                        if (diasIncapacidades.Contains(4)) x.dia4 = 10;

                        x.dia5 = 0;
                        if (diasVacaciones.Contains(5)) x.dia5 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 5)];
                        if (diasIncapacidades.Contains(5)) x.dia5 = 10;

                        x.dia6 = 0;
                        if (diasVacaciones.Contains(6)) x.dia6 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 6)];
                        if (diasIncapacidades.Contains(6)) x.dia6 = 10;

                        x.dia7 = 0;
                        if (diasVacaciones.Contains(7)) x.dia7 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 7)];
                        if (diasIncapacidades.Contains(7)) x.dia7 = 10;

                        x.dia8 = 0;
                        if (diasVacaciones.Contains(8)) x.dia8 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 8)];
                        if (diasIncapacidades.Contains(8)) x.dia8 = 10;

                        x.dia9 = 0;
                        if (diasVacaciones.Contains(9)) x.dia9 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 9)];
                        if (diasIncapacidades.Contains(9)) x.dia9 = 10;

                        x.dia10 = 0;
                        if (diasVacaciones.Contains(10)) x.dia10 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 10)];
                        if (diasIncapacidades.Contains(10)) x.dia10 = 10;

                        x.dia11 = 0;
                        if (diasVacaciones.Contains(11)) x.dia11 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 11)];
                        if (diasIncapacidades.Contains(11)) x.dia11 = 10;

                        x.dia12 = 0;
                        if (diasVacaciones.Contains(12)) x.dia12 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 12)];
                        if (diasIncapacidades.Contains(12)) x.dia12 = 10;

                        x.dia13 = 0;
                        if (diasVacaciones.Contains(13)) x.dia13 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 13)];
                        if (diasIncapacidades.Contains(13)) x.dia13 = 10;

                        x.dia14 = 0;
                        if (diasVacaciones.Contains(14)) x.dia14 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 14)];
                        if (diasIncapacidades.Contains(14)) x.dia14 = 10;

                        x.dia15 = 0;
                        if (diasVacaciones.Contains(15)) x.dia15 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 15)];
                        if (diasIncapacidades.Contains(15)) x.dia15 = 10;

                        x.dia16 = 0;
                        if (diasVacaciones.Contains(16)) x.dia16 = diasVacacionesTipo[diasVacaciones.FindIndex(y => y == 16)];
                        if (diasIncapacidades.Contains(16)) x.dia16 = 10;
                    }
                    x.primaDominical = false;
                    x.dias_extra_concepto = 0;
                }
                if (bonos != null)
                {
                    empty.evaluacionID = bonos.id;
                    foreach (var i in data)
                    {
                        var hasBono = bonos.listDetalle.FirstOrDefault(x => x.cve_Emp == i.clave_empleado);
                        if (hasBono != null)
                        {
                            i.bonoDM = hasBono.monto_Asig;
                            i.evaluacion_detID = hasBono.id;
                        }
                    }
                }
                result.incidencia = empty;
                result.incidencia_det = data;

                //var id_incidencia = GuardarIncidenciaSIGOPLAN_ENKONTROL(busq, result.incidencia, result.incidencia_det);

                //result.incidencia.id_incidencia = id_incidencia;
                //result.incidencia_det.ForEach(x=>x.id_incidencia = id_incidencia);

                //var cplanNew = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.cc == busq.cc && x.tipo_nomina == busq.tipoNomina && x.periodo == busq.periodo);
                //var det = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == cplanNew.id && x.clave_depto == busq.depto).ToList();

                //result.incidencia = cplanNew;
                //result.incidencia_det = det.OrderBy(x => x.ape_paterno + x.ape_materno + x.nombre).ToList();
                var permiso = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.autoriza);
                var permiso_bono_sinlimite = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && (x.cc.Equals("*") || x.cc.Equals(busq.cc)) && x.permiso_bono_sinlimite);
                result.isAuth = permiso;
                result.permiso_bono_sinlimite = permiso_bono_sinlimite;
                result.incidencia_det_Peru = datosPeru;
                return result;
                //}

                #endregion
            }
            result.incidencia_det_Peru = datosPeru;
            return result;
        }

        string queryLstIncidenciasPeru()
        {
            return @"SELECT 
                        e.clave_empleado,
                        e.nombre,
                        e.ape_paterno,
                        e.ape_materno,
                        e.tipo_nomina,
                        dep.clave_depto,
                        dep.desc_depto AS deptoDesc,
                        e.puesto,
                        pu.descripcion AS puestoDesc,
                        e.cc_contable,
                        @anio AS anio,
                        @periodo AS periodo,
                        'PENDIENTE' AS estatusDesc,
                        " + (vSesiones.sesionUsuarioDTO.cveEmpleado ?? "1") + @" AS empleado_modifica,
                        '" + vSesiones.sesionUsuarioDTO.nombre + @"' AS nombreEmpMod,
                        0 AS dia1,
                        0 AS dia2,
                        0 AS dia3,
                        0 AS dia4,
                        0 AS dia5,
                        0 AS dia6,
                        0 AS dia7,
                        0 AS dia8,
                        0 AS dia9,
                        0 AS dia10,
                        0 AS dia11,
                        0 AS dia12,
                        0 AS dia13,
                        0 AS dia14,
                        0 AS dia15,
                        0 AS dia16,
                        0 AS horas_extras,
                        (
                            SELECT 
                                COUNT(*) 
                            FROM (
                                SELECT 
                                    TOP 2 b.anio, 
                                    b.periodo, 
                                    b.cc, 
                                    a.* 
                                FROM 
                                    tblRH_BN_Incidencia_det a INNER JOIN tblRH_BN_Incidencia b ON a.id_incidencia=b.id_incidencia WHERE a.clave_empleado = e.clave_empleado AND b.estatus = 'A' ORDER BY b.anio, b.periodo DESC) x 
                            WHERE x.bono > 0
                        ) AS countBonosPersonales,
                        0 AS bonoMensual,
                        0 AS bonoUnico,
                        e.fecha_antiguedad AS fechaAlta,
                        (CASE WHEN EXISTS (SELECT b.numeroEmpleado FROM tblRH_Baja_Registro b WHERE b.cc = e.cc_contable AND b.numeroEmpleado = e.clave_empleado AND (b.fechaBaja >= @fechaBajaInicio and b.fechaBaja <= @fechaBajaFin)) THEN 1 ELSE 0 END) AS isBaja,
                        (SELECT TOP 1 b.fechaBaja FROM tblRH_Baja_Registro b WHERE b.cc = e.cc_contable AND b.numeroEmpleado = e.clave_empleado AND (b.fechaBaja >= @fechaBajaInicio AND b.fechaBaja <= @fechaBajaFin)) AS fechaBaja 
                    FROM 
                        tblRH_EK_Empleados AS e 
                        INNER JOIN tblRH_REC_InfoEmpleadoPeru peru ON peru.claveEmpleado = e.clave_empleado
                        --INNER JOIN DBA.cc AS c ON c.cc = e.cc_contable
                        INNER JOIN tblRH_EK_Puestos AS pu ON e.puesto = pu.puesto
                        INNER JOIN tblRH_EK_Tipos_Nomina AS tn ON peru.tipotrab = tn.tipo_nomina
                        INNER JOIN tblRH_EK_Departamentos dep ON (dep.cc = e.cc_contable AND dep.clave_depto = e.clave_depto)
                        
                    WHERE 
                        dep.clave_depto = @clave_depto AND
                        (
                            (e.cc_contable = @cc_contable AND e.estatus_empleado = 'A' AND tn.tipo_nomina = @tipo_nomina) OR 
                            (
                                (e.cc_contable = @cc_contable AND e.estatus_empleado = 'B' AND tn.tipo_nomina = @tipo_nomina ) AND 
                                e.clave_empleado in
                                (
                                    SELECT 
                                        b.numeroEmpleado 
                                    FROM 
                                        tblRH_Baja_Registro b 
                                        INNER JOIN tblRH_REC_InfoEmpleadoPeru empl ON b.numeroEmpleado = empl.claveEmpleado 
                                    WHERE 
                                        b.est_baja='A' AND 
                                        b.cc = @cc_contable AND 
                                        (b.fechaBaja >= @fechaBajaInicio AND b.fechaBaja <= @fechaBajaFin) AND 
                                        empl.tipotrab = @tipo_nomina
                                )
                            )
                        )
                    ";
        }

        public List<incidenciasPendientesDTO> getIncidenciasPendientePeru()
        {
            List<incidenciasPendientesDTO> result = new List<incidenciasPendientesDTO>();
            var _user = vSesiones.sesionUsuarioDTO.id;
            //var lst = (List<ComboDTO>)ContextEnKontrolNomina.Where("SELECT cc as Value, (cc+'-' +descripcion) as Text FROM cc where st_ppto!='T'").ToObject<List<ComboDTO>>();

            List<ComboDTO> lst = _context.Select<ComboDTO>(new DapperDTO
            {
                baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                consulta = @"SELECT cc AS VALUE, (cc + '-' + ccDescripcion) AS TEXT FROM tblC_Nom_CatalogoCC WHERE cc <> '180-A' and cc <> '187-A'",
            }).ToList();

            var permisoTodo = _context.tblRH_BN_Usuario_CC.Any(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.cc.Equals("*") && x.autoriza);
            var ccs = _context.tblRH_BN_Usuario_CC.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id && !x.cc.Equals("*") && x.autoriza).Select(x => x.cc).ToList();

            var ccAutoriza = lst.Where(x => permisoTodo ? true : ccs.Contains(x.Value)).Select(x => x.Value).OrderBy(o => o).ToList();

            var anio = DateTime.Now.Year;
            var diaActual = DateTime.Today;
            var diaLimiteSemana = diaActual.AddDays(6);
            var diaLimiteQuincena = diaActual.AddDays(14);

            DateTime firstDayInWeek = GetFirstDayOfWeek(DateTime.Now);
            DateTime lasttDayInWeek = GetFirstDayOfWeek(DateTime.Now).AddDays(40);
            var periodos = _context.tblRH_BN_EstatusPeriodos.OrderBy(x => x.fecha_inicial).ToList();

            var periodoObraroActual = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_limite <= diaLimiteSemana && x.anio == anio && x.tipo_nomina == 20).OrderByDescending(x => x.periodo).FirstOrDefault();
            var periodoEmpleadoActual = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_limite <= diaLimiteQuincena && x.anio == anio && x.tipo_nomina == 21).OrderByDescending(x => x.periodo).FirstOrDefault();
            var periodoConstruccionCivilActual = _context.tblRH_BN_EstatusPeriodos.Where(x => x.fecha_limite <= diaLimiteSemana && x.anio == anio && x.tipo_nomina == 27).OrderByDescending(x => x.periodo).FirstOrDefault();

            var incidencias = _context.tblRH_BN_Incidencia.Where(x => x.estatus.Equals("P") && (ccAutoriza.Contains(x.cc)) && /*(((x.periodo == semana.periodo && x.tipo_nomina == 1) || (x.periodo == quincena.periodo && x.tipo_nomina == 4)))) &&*/ _context.tblRH_BN_Incidencia_det.Any(y => y.incidenciaID == x.id)).ToList();

            var incidenciasTotales = _context.tblRH_BN_Incidencia.Where(x => x.estatus.Equals("A") && ((x.anio >= anio && ccAutoriza.Contains(x.cc)) || (x.anio == anio && ccAutoriza.Contains(x.cc) && ((x.periodo == periodoObraroActual.periodo && x.tipo_nomina == 20) || (x.periodo == periodoEmpleadoActual.periodo && x.tipo_nomina == 21) || (x.periodo == periodoConstruccionCivilActual.periodo && x.tipo_nomina == 27)))) && _context.tblRH_BN_Incidencia_det.Any(y => y.incidenciaID == x.id)).ToList();

            incidenciasTotales.AddRange(incidencias);

            var ccSinCaptura = _context.tblC_Nom_CatalogoCC.Where(x => x.estatus && ccAutoriza.Contains(x.cc)).ToList();

            var ccConCapturaObrero = incidenciasTotales.Where(x => x.tipo_nomina == 20 && x.periodo == periodoObraroActual.periodo && x.anio == anio).Select(x => x.cc).ToList();
            var ccConCapturaEmpleado = incidenciasTotales.Where(x => x.tipo_nomina == 21 && x.periodo == periodoEmpleadoActual.periodo && x.anio == anio).Select(x => x.cc).ToList();
            var ccConCapturaConstruccionCivil = incidenciasTotales.Where(x => x.tipo_nomina == 27 && x.periodo == periodoConstruccionCivilActual.periodo && x.anio == anio).Select(x => x.cc).ToList();

            var ccSinCapturaObrero = ccSinCaptura.Where(x => !ccConCapturaObrero.Contains(x.cc)).Select(x => x.cc).ToList();
            var ccSinCapturaEmpleado = ccSinCaptura.Where(x => !ccConCapturaEmpleado.Contains(x.cc)).Select(x => x.cc).ToList();
            var ccSinCapturaConstruccionCivil = ccSinCaptura.Where(x => !ccConCapturaConstruccionCivil.Contains(x.cc)).Select(x => x.cc).ToList();



            foreach (var i in incidencias)
            {
                var periodo = periodos.Where(x => x.periodo == i.periodo && x.anio == i.anio && x.tipo_nomina == i.tipo_nomina).FirstOrDefault();
                var empleadosCapturadosID = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == i.id).Select(x => x.clave_empleado).ToList(); ;
                var empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => x.tipo_nomina == i.tipo_nomina && x.cc_contable == i.cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == i.anio && x.tipoNomina == i.tipo_nomina && x.periodo == i.periodo && x.cc == i.cc);
                var o = new incidenciasPendientesDTO();
                var cc = lst.FirstOrDefault(x => x.Value.Equals(i.cc));
                o.id = i.id;
                o.id_incidencia = i.id_incidencia;
                o.cc = cc == null ? i.cc : cc.Text;
                o.anio = i.anio;
                o.tipo_nomina = i.tipo_nomina;
                o.periodo = i.periodo;
                o.fechas = periodo.fecha_inicial.ToShortDateString() + " - " + periodo.fecha_final.ToShortDateString();
                o.cambio_pendiente = empleadosPendienteCaptura.Count() > 0;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = periodo.fecha_inicial;
                result.Add(o);
            }

            foreach (var item in ccSinCapturaObrero)
            {
                var o = new incidenciasPendientesDTO();
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == anio && x.tipoNomina == 20 && x.periodo == periodoObraroActual.periodo && x.cc == item);
                var cc = lst.FirstOrDefault(x => x.Value.Equals(item));
                o.id = 0;
                o.id_incidencia = 0;
                o.cc = cc == null ? item : cc.Text;
                o.anio = anio;
                o.tipo_nomina = 20;
                o.periodo = periodoObraroActual.periodo;
                o.fechas = periodoObraroActual.fecha_inicial.ToShortDateString() + " - " + periodoObraroActual.fecha_final.ToShortDateString();
                o.cambio_pendiente = true;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = periodoObraroActual.fecha_inicial;
                result.Add(o);
            }

            foreach (var item in ccSinCapturaEmpleado)
            {
                var o = new incidenciasPendientesDTO();
                var cc = lst.FirstOrDefault(x => x.Value.Equals(item));
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == anio && x.tipoNomina == 21 && x.periodo == periodoEmpleadoActual.periodo && x.cc == item);
                o.id = 0;
                o.id_incidencia = 0;
                o.cc = cc == null ? item : cc.Text;
                o.anio = anio;
                o.tipo_nomina = 21;
                o.periodo = periodoEmpleadoActual.periodo;
                o.fechas = periodoEmpleadoActual.fecha_inicial.ToShortDateString() + " - " + periodoEmpleadoActual.fecha_final.ToShortDateString();
                o.cambio_pendiente = true;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = periodoEmpleadoActual.fecha_inicial;
                result.Add(o);
            }

            foreach (var item in ccSinCapturaConstruccionCivil)
            {
                var o = new incidenciasPendientesDTO();
                var cc = lst.FirstOrDefault(x => x.Value.Equals(item));
                var evaluacionPendiente = _context.tblRH_BN_Evaluacion.FirstOrDefault(x => x.anio == anio && x.tipoNomina == 27 && x.periodo == periodoConstruccionCivilActual.periodo && x.cc == item);
                o.id = 0;
                o.id_incidencia = 0;
                o.cc = cc == null ? item : cc.Text;
                o.anio = anio;
                o.tipo_nomina = 27;
                o.periodo = periodoConstruccionCivilActual.periodo;
                o.fechas = periodoConstruccionCivilActual.fecha_inicial.ToShortDateString() + " - " + periodoConstruccionCivilActual.fecha_final.ToShortDateString();
                o.cambio_pendiente = true;
                o.evaluacion_pendiente = evaluacionPendiente == null ? 0 : (evaluacionPendiente.estatus == 0 ? 1 : 2);
                o.fecha_inicio = periodoConstruccionCivilActual.fecha_inicial;
                result.Add(o);
            }

            return result.OrderBy(x => x.anio).ThenBy(x => x.fecha_inicio).ThenBy(x => x.cc).ToList();
        }

        public IncidenciasPaqueteDTO getIncidenciaAuthPeru(int incidenciaID, int anio = 0, int periodo = 0, int tipo_nomina = 0, string cc = "")
        {
            var result = new IncidenciasPaqueteDTO();
            var obj = _context.tblRH_BN_Incidencia.FirstOrDefault(x => x.id == incidenciaID);
            var empleadosCapturados = _context.tblRH_BN_Incidencia_det.Where(x => x.incidenciaID == incidenciaID).ToList();
            var empleadosCapturadosID = empleadosCapturados.Select(x => x.clave_empleado).ToList();
            var empleadosPendienteCaptura = new List<tblRH_EK_Empleados>();
            var puestoEmpleados = new List<tblRH_EK_Puestos>();
            var departamentosEmpleados = new List<tblRH_EK_Departamentos>();
            List<int> empleadosPendientesPeru = new List<int>();
            List<tblRH_BN_Incidencia_det> detalles = new List<tblRH_BN_Incidencia_det>();

            bool ev = false;

            if (obj == null)
            {
                var ccID = cc.Split('-');
                var _cc = ccID[0];
                _cc = _cc.Replace("-", "");
                empleadosPendientesPeru = _context.tblRH_REC_InfoEmpleadoPeru.Where(x => x.tipotrab == tipo_nomina).Select(x => x.claveEmpleado).ToList();
                empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => empleadosPendientesPeru.Contains(x.clave_empleado) && x.cc_contable == _cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var empleadosPendientesPuestos = empleadosPendienteCaptura.Select(x => x.puesto).ToList();
                puestoEmpleados = puestoEmpleados = _context.tblRH_EK_Puestos.Where(x => empleadosPendientesPuestos.Contains(x.puesto)).ToList();
                var empleadosPendientesDepto = empleadosPendienteCaptura.Select(x => x.clave_depto.ParseInt()).ToList();
                departamentosEmpleados = _context.tblRH_EK_Departamentos.Where(x => empleadosPendientesDepto.Contains(x.clave_depto)).ToList();
                ev = _context.tblRH_BN_Evaluacion.Any(x => x.cc == _cc && x.tipoNomina == tipo_nomina && x.periodo == periodo && x.estatus == (int)authEstadoEnum.EnEspera);
            }
            else
            {
                empleadosPendientesPeru = _context.tblRH_REC_InfoEmpleadoPeru.Where(x => x.tipotrab == tipo_nomina).Select(x => x.claveEmpleado).ToList();
                empleadosPendienteCaptura = _context.tblRH_EK_Empleados.Where(x => empleadosPendientesPeru.Contains(x.clave_empleado) && x.cc_contable == obj.cc && x.estatus_empleado == "A" && !empleadosCapturadosID.Contains(x.clave_empleado)).ToList();
                var empleadosPendientesPuestos = empleadosPendienteCaptura.Select(x => x.puesto).ToList();
                puestoEmpleados = puestoEmpleados = _context.tblRH_EK_Puestos.Where(x => empleadosPendientesPuestos.Contains(x.puesto)).ToList();
                var empleadosPendientesDepto = empleadosPendienteCaptura.Select(x => x.clave_depto.ParseInt()).ToList();
                departamentosEmpleados = _context.tblRH_EK_Departamentos.Where(x => empleadosPendientesDepto.Contains(x.clave_depto)).ToList();
                ev = _context.tblRH_BN_Evaluacion.Any(x => x.cc == obj.cc && x.tipoNomina == obj.tipo_nomina && x.periodo == obj.periodo && x.estatus == (int)authEstadoEnum.EnEspera);
            }

            detalles =
                empleadosPendienteCaptura.Select(x =>
                {
                    var _puesto = puestoEmpleados.FirstOrDefault(y => y.puesto == x.puesto);
                    var _depto = departamentosEmpleados.FirstOrDefault(y => y.clave_depto == x.clave_depto.ParseInt());
                    return new tblRH_BN_Incidencia_det()
                    {
                        ape_paterno = x.ape_paterno,
                        ape_materno = x.ape_materno,
                        nombre = x.nombre,
                        clave_empleado = x.clave_empleado,
                        puesto = x.puesto ?? 0,
                        puestoDesc = _puesto == null ? "--" : _puesto.descripcion,
                        clave_depto = Int32.Parse(x.clave_depto),
                        deptoDesc = _depto == null ? "--" : _depto.desc_depto,
                        total_Dias = 0,
                        estatus = false
                    };
                }).ToList();
            detalles.AddRange(empleadosCapturados);
            result.incidencia_det = detalles.OrderBy(x => x.estatus).ThenBy(x => x.ape_paterno).ThenBy(x => x.ape_materno).ToList();
            result.evaluacion_pendiente = false;
            result.evaluacion_pendiente = ev;

            return result;
        }


        #endregion

    }
}
