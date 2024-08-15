using Core.DAO.Encuestas;
using Core.DTO;
using Core.DTO.Encuestas;
using Core.Entity.Encuestas;
using Core.Entity.Principal.Alertas;
using Core.Enum.Encuesta;
using Core.Enum.Principal.Alertas;
using Data.DAO.Principal.Usuarios;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Alertas;
using Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Usuarios;
using Core.DTO.Principal.Usuarios;
using Data.Factory.Principal.Usuarios;
using System.Globalization;
using OfficeOpenXml;
using System.IO;
using Core.DTO.Encuestas.Cliente;
using Core.Entity.Principal.Menus;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;

namespace Data.DAO.Encuestas
{
    public class EncuestasDAO : GenericDAO<tblEN_Encuesta>, IEncuestasDAO
    {
        string nombreControlador = "Encuestas";
        static UsuarioDAO usuarioDao = new UsuarioDAO();
        private UsuarioFactoryServices usuarioFactoryServices = new UsuarioFactoryServices();
        public int saveEncuesta(tblEN_Encuesta obj)
        {

            if (!Exists(obj))
            {
                IObjectSet<tblEN_Encuesta> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_Encuesta>();
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                //SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                //SaveEntity(obj, (int)BitacoraEnum.MINUTA);

                try
                {

                    var subject = "Encuesta: " + obj.titulo;
                    var body = @"El usuario: " + usuarioDao.getNombreUsuario(obj.creadorID) +
                        (obj.descripcion != null ? " ha creado una encuesta nueva: " + obj.descripcion : " ha creado una encuesta nueva") + ".<br/> Queda pendiente de autorización.<br/> Hora " + DateTime.Now;
                    List<string> contactos = new List<string>();

                    //Correo "hard-codeado" para René Manuel Escalante Sánchez. Coordinador del Sistema de Gestión de Calidad.
                    var c = _context.tblP_Usuario.FirstOrDefault(x => x.id == 79649);
                    contactos.Add(c.correo);
                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, contactos);
                }
                catch (Exception ex)
                {
                    var c = ex.Message;
                }
            }
            else
            {
                IObjectSet<tblEN_Encuesta_Update> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_Encuesta_Update>();
                var o = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                var data = new tblEN_Encuesta_Update();
                data.encuestaID = obj.id;
                data.objData = o;

                data.usuarioID = vSesiones.sesionUsuarioDTO.id;
                data.fecha = DateTime.Now;
                data.estatus = (int)EncuestaEnum.PENDIENTE;
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(data);
                _context.SaveChanges();

                try
                {


                    var subject = "Encuesta Actualizada: " + obj.titulo;
                    var body = @"El usuario: " + usuarioDao.getNombreUsuario(vSesiones.sesionUsuarioDTO.id) +
                        (obj.descripcion != null ? " ha modificado la encuesta: " + obj.descripcion : " ha modificado la encuesta: " + obj.titulo) + ".<br/> Hora " + DateTime.Now;
                    List<string> contactos = new List<string>();

                    //Correo "hard-codeado" para René Manuel Escalante Sánchez. Coordinador del Sistema de Gestión de Calidad.
                    var c = _context.tblP_Usuario.FirstOrDefault(x => x.id == 79649);
                    contactos.Add(c.correo);
                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, contactos);
                }
                catch (Exception ex)
                {
                    var c = ex.Message;
                }
            }
            return obj.id;
        }
        public bool GuardarEncAsigUsuario(List<tblEN_EncuestaAsignaUsuario> lst)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var encuestaID = lst.FirstOrDefault().encuestaID;
                    var lstBD = _context.tblEN_EncuestaAsignaUsuario.Where(w => w.encuestaID.Equals(encuestaID)).ToList();
                    lstBD.ForEach(rel => _context.Set<tblEN_EncuestaAsignaUsuario>().Remove(rel));
                    _context.Set<tblEN_EncuestaAsignaUsuario>().AddRange(lst);
                    _context.SaveChanges();
                    esGuardado = lst.Count > 0 && lst.All(a => a.id > 0);
                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "GuardarEncAsigUsuario", e, AccionEnum.ACTUALIZAR, 0, lst);
                    esGuardado = false;
                }
                return esGuardado;
            }
        }
        public bool eliminaEncAsigUsuario(tblEN_EncuestaAsignaUsuario obj)
        {
            var esEliminado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var del = _context.tblEN_EncuestaAsignaUsuario.FirstOrDefault(rel => rel.encuestaID.Equals(obj.encuestaID) && rel.usuarioID.Equals(obj.usuarioID));
                    if (del == null)
                    {

                    }
                    else
                    {
                        _context.Set<tblEN_EncuestaAsignaUsuario>().Remove(del);
                        _context.SaveChanges();
                        dbTransaction.Commit();
                    }
                    esEliminado = true;
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, nombreControlador, "eliminaEncAsigUsuario", e, AccionEnum.ELIMINAR, obj.id, obj);
                    esEliminado = false;
                }
            }
            return esEliminado;
        }
        public EncuestaDTO getEncuesta(int id)
        {
            var result = new EncuestaDTO();

            using (var ctx = new MainContext(vSesiones.sesionEmpresaActual))
            {
                var uEncuesta = ctx.tblEN_Encuesta_Usuario.FirstOrDefault(x => x.id == id);
                var temp = ctx.tblEN_Encuesta.FirstOrDefault(x => x.id == uEncuesta.encuestaID);
                result.id = temp.id;
                result.encuestaUsuarioID = uEncuesta.id;
                result.titulo = temp.titulo;
                result.descripcion = temp.descripcion;
                result.creadorID = temp.creadorID;
                result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
                result.contestada = uEncuesta.estatus ? false : true;
                result.asunto = uEncuesta.asunto ?? "";
                var listaPreguntas = new List<PreguntaDTO>();
                foreach (var i in temp.preguntas.Where(x => x.visible == true))
                {
                    var p = new PreguntaDTO();
                    p.id = i.id;
                    p.encuestaID = i.encuestaID;
                    p.encuestaUsuarioID = uEncuesta.id;
                    p.pregunta = i.pregunta;
                    p.calificacion = 0;
                    listaPreguntas.Add(p);
                }
                result.preguntas = listaPreguntas;
            }

            return result;
        }
        public EncuestaDTO getEncuestaByID(int id)
        {
            var result = new EncuestaDTO();
            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id);
            result.id = temp.id;
            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            result.creadorID = temp.creadorID;
            result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
            result.tipo = temp.tipo;
            result.departamentoID = temp.departamentoID;

            var listaPreguntas = new List<PreguntaDTO>();
            foreach (var i in temp.preguntas.Where(x => x.visible == true))
            {
                var p = new PreguntaDTO();
                p.id = i.id;
                p.encuestaID = i.encuestaID;
                p.pregunta = i.pregunta;
                p.calificacion = 0;
                p.tipo = i.tipo;
                listaPreguntas.Add(p);
            }
            result.preguntas = listaPreguntas;
            return result;
        }
        public void saveEncuestaResult(List<tblEN_Resultado> obj, int encuestaID, string comentario)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var estrellas = _context.tblEN_Estrellas.ToList();

                    obj.ForEach(e =>
                    {
                        e.porcentaje = estrellas.FirstOrDefault(x => x.estrellas == e.calificacion) != null ? estrellas.FirstOrDefault(x => x.estrellas == e.calificacion).maximo : 0;
                    });

                    _context.tblEN_Resultado.AddRange(obj);
                    _context.SaveChanges();

                    var asig = _context.tblEN_Encuesta_Usuario.FirstOrDefault(x => x.id == encuestaID);

                    asig.calificacion = (decimal)Math.Truncate(100 * (double)(obj.Where(y => y.porcentaje != null).Select(x => x.porcentaje).Sum() / obj.Count).Value) / 100;
                    asig.comentario = comentario;
                    asig.estatus = false;

                    var encuesta = _context.tblEN_Encuesta.Where(x => x.id == asig.encuestaID).FirstOrDefault();
                    var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuesta.id).Select(y => y.pregunta).ToList();

                    var alert = _context.tblP_Alerta.FirstOrDefault(x => x.sistemaID == 8 && x.objID == encuestaID && x.userRecibeID == vSesiones.sesionUsuarioDTO.id);

                    if (alert != null)
                    {
                        alert.visto = true;
                    }

                    _context.SaveChanges();

                    //if (encuesta != null && encuesta.notificacion != null && encuesta.notificacion.Value)
                    //{
                    //    var usuarios = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus && (x.recibeNotificacion != null && x.recibeNotificacion.Value) && x.encuestaID == asig.encuestaID).Select(m => m.usuarioID).ToList();
                    //    var usuarioCorreo = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id)).Select(m => m.correo).ToList();
                    //    //var usuarioCorreo = _context.tblP_Usuario.Where(x => x.id == alert.userEnviaID).Select(y => y.correo).ToList();
                    //    var subject = "Respuesta de Encuesta: " + encuesta.titulo;
                    //    var body = @"El usuario: " + usuarioDao.getNombreUsuario(asig.usuarioResponderID) + // _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)).nombreUsuario +
                    //        " a contestado la encuesta: " + encuesta.descripcion + ".<br/> Hora " + DateTime.Now + ".<br/>";

                    //    GlobalUtils.sendEmail(subject, body, usuarioCorreo);
                    //}

                    var flagInconformidad = false;

                    foreach (var respuesta in obj)
                    {
                        if (respuesta.calificacion <= 3)
                        {
                            flagInconformidad = true;
                        }
                    }

                    var usuarioResponde = _context.tblP_Usuario.FirstOrDefault(x => x.id == asig.usuarioResponderID);

                    var usuariosNoti = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus == true && x.encuestaID == encuesta.id && x.recibeNotificacion == true).Select(y => y.usuarioID).ToList();
                    var usuariosCorreos = new List<string>();
                    if (flagInconformidad || usuarioResponde.cliente)
                    {
                        usuariosCorreos.AddRange(_context.tblP_Usuario.Where(x => usuariosNoti.Contains(x.id)).Select(y => y.correo).ToList());
                    }
                    else {
                        try
                        {
                            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
                            var envioA = en.FirstOrDefault(x => x.objID == asig.encuestaID);
                            var uen = _context.tblP_Usuario.FirstOrDefault(x => x.id == envioA.userEnviaID);
                            usuariosCorreos.Add(uen.correo);
                        }
                        catch(Exception e2){}
                    }

                    if (encuesta.notificacion == true)
                    {
                        if (usuarioResponde.cliente)
                        {
                            if (flagInconformidad == true)
                            {
                                correoResultadoEncuesta("Calificación baja en la Encuesta: " + encuesta.titulo, usuariosCorreos, asig, encuesta, obj, flagInconformidad);
                            }
                            else
                            {
                                correoResultadoEncuesta("Calificación en la Encuesta: " + encuesta.titulo, usuariosCorreos, asig, encuesta, obj, flagInconformidad);
                            }
                        }
                        else
                        {
                            if (flagInconformidad == true)
                            {
                                correoResultadoEncuesta("Calificación baja en la Encuesta: " + encuesta.titulo, usuariosCorreos, asig, encuesta, obj, flagInconformidad);
                            }
                            else {
                                correoResultadoEncuesta("Calificación en la Encuesta: " + encuesta.titulo, usuariosCorreos, asig, encuesta, obj, flagInconformidad);
                                //var usuarios = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus && (x.recibeNotificacion != null && x.recibeNotificacion.Value) && x.encuestaID == asig.encuestaID).Select(m => m.usuarioID).ToList();
                                //var usuarioCorreo = _context.tblP_Usuario.Where(x => usuarios.Contains(x.id)).Select(m => m.correo).ToList();
                                ////var usuarioCorreo = _context.tblP_Usuario.Where(x => x.id == alert.userEnviaID).Select(y => y.correo).ToList();
                                //var subject = "Respuesta de Encuesta: " + encuesta.titulo;
                                //var body = @"El usuario: " + usuarioDao.getNombreUsuario(asig.usuarioResponderID) + // _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)).nombreUsuario +
                                //    " a contestado la encuesta: " + encuesta.descripcion + ".<br/> Hora " + DateTime.Now + ".<br/>";

                                //GlobalUtils.sendEmail(subject, body, usuarioCorreo);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ArgumentNullException("Entity");
                }
            }
        }

        public void correoResultadoEncuesta(string subject, List<string> usuariosCorreos, tblEN_Encuesta_Usuario asig, tblEN_Encuesta encuesta, List<tblEN_Resultado> obj, bool flagInconformidad)
        {

            //  var usuarioNombre = _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)) != null ? _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)).nombre + " " + _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)).apellidoPaterno + " " + _context.tblP_Usuario.FirstOrDefault(x => x.id.Equals(asig.usuarioResponderID)).apellidoMaterno : "";


            var empresa = getAreaEmpresa(asig.usuarioResponderID);
            var calificacionText = flagInconformidad ? "con una calificación baja" : "de manera satisfactoria";

            var body = "El usuario: " + usuarioDao.getNombreUsuario(asig.usuarioResponderID) + " de " + empresa + " ha contestado " + calificacionText + " la encuesta: \"" + encuesta.titulo + "\"; sobre los servicios de \"" + asig.asunto + "\".<br/> Hora " + DateTime.Now + ".<br/>";

            var html = "<br/>Respuestas:<br/>";

            html += "<style>";
            html += "   table { border: 1px solid black; }";
            html += "   th, .pregunta, .calificacion, .explicacion { border: 1px solid black; }";
            html += "   .calificacion { text-align: center; }";
            html += "   .baja { background-color: red; color: black; }";
            html += "   .filaEncabezado { background-color: #81bd72; color: white; }";
            html += "</style>";

            html += "<table id='tblData' class='table table-condensed table-hover table-striped text-center' style='width:99% !important'>";
            html += "   <thead>";
            html += "       <tr class='filaEncabezado'>";
            html += "           <th>Pregunta</th>";
            html += "           <th>Calificación</th>";
            html += "           <th>Explicación</th>";
            html += "       </tr>";
            html += "   </thead>";
            html += "   <tbody>";

            foreach (var resp in obj)
            {
                if (resp.calificacion <= 3)
                {
                    var pregunta = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuesta.id && x.id == resp.preguntaID).Select(y => y.pregunta).FirstOrDefault();

                    html += "   <tr class='baja'>";
                    html += "       <td class='pregunta'>" + pregunta + "</td>";
                    html += "       <td class='calificacion'>" + resp.calificacion + "</td>";
                    html += "       <td class='explicacion'>" + ((resp.respuesta != null) ? resp.respuesta : "") + "</td>";
                    html += "   </tr>";
                }
                else
                {
                    var pregunta = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuesta.id && x.id == resp.preguntaID).Select(y => y.pregunta).FirstOrDefault();

                    html += "   <tr>";
                    html += "       <td class='pregunta'>" + pregunta + "</td>";
                    html += "       <td class='calificacion'>" + resp.calificacion + "</td>";
                    html += "       <td class='explicacion'></td>";
                    html += "   </tr>";
                }
            }

            html += "   </tbody>";
            html += "</table>";

            html += "<br/>";
            html += "Comentario:<br/>";
            html += asig.comentario;
            html += "<br/>";

            body += html;

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, usuariosCorreos.Distinct().ToList());
        }

        private string getAreaEmpresa(int p)
        {
            var usuario = _context.tblP_Usuario.FirstOrDefault(x => x.id == p);


            if (usuario != null)
            {
                if (usuario.cliente)
                {
                    return usuario.empresa;
                }
                else
                {
                    var cc = _context.tblP_CC.Where(x => x.cc == usuario.cc).ToList();

                    if (cc.Count > 0)
                    {
                        return cc.FirstOrDefault().descripcion;
                    }
                    else
                    {
                        return "";
                    }

                }
            }

            return "";

        }

        public void correoEncuesta(SendEncuestaDTO obj)
        {
            try
            {
                //Envio de correo de resultados
                var eu = _context.tblEN_Encuesta_Usuario.FirstOrDefault(x => x.id == obj.encuestaID);
                var m = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == eu.encuestaID);
                var subject = "Encuesta: " + m.titulo;
                var body = @"El usuario: " + usuarioDao.getNombreUsuario(obj.encuestadoID) +
                    " a contestado la encuesta:" + m.descripcion + ".<br/> Hora " + DateTime.Now;
                List<string> contactos = new List<string>();
                var ua = _context.tblP_Alerta.FirstOrDefault(x => x.sistemaID == 8 && x.objID == obj.encuestaID);
                var c = _context.tblP_Usuario.FirstOrDefault(x => x.id == ua.userEnviaID);
                contactos.Add(c.correo);
                GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, contactos);
            }
            catch (Exception ex)
            {
                var c = ex.Message;
            }
        }
        public EncuestaDTO getEncuestaResult(int id)
        {
            var result = new EncuestaDTO();
            var ue = _context.tblEN_Encuesta_Usuario.FirstOrDefault(x => x.id == id);
            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == ue.encuestaID);
            result.id = temp.id;
            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            result.creadorID = temp.creadorID;
            result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
            var listaPreguntas = new List<PreguntaDTO>();
            foreach (var i in temp.preguntas)
            {
                var p = new PreguntaDTO();
                p.id = i.id;
                p.encuestaID = i.encuestaID;
                p.pregunta = i.pregunta;
                var val = _context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == id && x.preguntaID == i.id);
                if (val != null)
                {
                    p.respuesta = _context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == id && x.preguntaID == i.id).respuesta;
                    p.calificacion = Convert.ToInt32(_context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == id && x.preguntaID == i.id).calificacion);
                    listaPreguntas.Add(p);
                }
            }
            result.preguntas = listaPreguntas;
            result.comentario = _context.tblEN_Encuesta_Usuario.FirstOrDefault(x => x.id == id).comentario;
            var envioA = _context.tblP_Alerta.FirstOrDefault(x => x.objID == ue.id && x.sistemaID == 8);
            if (envioA != null)
            {
                var uen = _context.tblP_Usuario.FirstOrDefault(x => x.id == envioA.userEnviaID);
                result.envio = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;

            }
            else
            {
                result.envio = "";
            }
            result.fechaEnvio = ue.fecha.ToShortDateString();
            var uer = _context.tblP_Usuario.FirstOrDefault(x => x.id == ue.usuarioResponderID);
            result.respondio = uer.nombre + " " + uer.apellidoPaterno + " " + uer.apellidoMaterno;
            var fur = _context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == id);
            result.fechaRespondio = fur != null ? fur.fecha.ToShortDateString() : "";
            result.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id == temp.departamentoID).descripcion;
            var preguntasCant = result.preguntas.Count;
            var preguntasSum = result.preguntas.Sum(x => x.calificacion);
            var cal100 = preguntasCant * 5;
            var cal = (preguntasSum * 100);
            result.calificacionEncuesta = (cal100 > 0 ? (cal / cal100) : 0) + "%";
            return result;
        }
        public EncuestaDTO getEncuestaValidar(int id)
        {
            var result = new EncuestaDTO();
            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id);
            result.id = temp.id;
            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            result.creadorID = temp.creadorID;
            result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
            var listaPreguntas = new List<PreguntaDTO>();
            foreach (var i in temp.preguntas)
            {
                var p = new PreguntaDTO();
                p.id = i.id;
                p.encuestaID = i.encuestaID;
                p.pregunta = i.pregunta;
                p.respuesta = "";
                p.calificacion = 5;
                listaPreguntas.Add(p);
            }
            result.preguntas = listaPreguntas;
            result.comentario = "";
            return result;
        }
        public EncuestaDTO getEncuestaValidarUpdate(int id)
        {
            var result = new EncuestaDTO();
            var up = _context.tblEN_Encuesta_Update.FirstOrDefault(x => x.id == id);
            var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<tblEN_Encuesta>(up.objData);
            result.id = temp.id;
            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            result.creadorID = temp.creadorID;
            //result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
            var listaPreguntas = new List<PreguntaDTO>();
            foreach (var i in temp.preguntas)
            {
                var p = new PreguntaDTO();
                p.id = i.id;
                p.encuestaID = i.encuestaID;
                p.pregunta = i.pregunta;
                p.respuesta = "";
                p.calificacion = 5;
                p.estatus = i.estatus;
                listaPreguntas.Add(p);
            }
            result.preguntas = listaPreguntas;
            result.comentario = "";
            return result;
        }
        public List<tblEN_Encuesta> getEncuestasByOwner(int id)
        {

            var result = _context.tblEN_Encuesta.Where(x => x.creadorID == id && x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO).ToList();

            return result;
        }
        public List<tblEN_Encuesta> getEncuestasByDepto(int id)
        {
            var ud = new UsuarioDAO();

            var isPermisoCompras = ud.getViewAction(vSesiones.sesionCurrentView, "ComprasYRenta");
            var isPermisoSGC = ud.getViewAction(vSesiones.sesionCurrentView, "Administrar");
            var isPermisoComercializacion = ud.getViewAction(vSesiones.sesionCurrentView, "EncuestaComercialización");
            var isPermisoCDI = ud.getViewAction(vSesiones.sesionCurrentView, "EsCDI");

            var result = _context.tblEN_Encuesta.Where(x =>
                (
                    (
                        isPermisoCompras ? (x.departamentoID == id || x.id == 22 || x.id == 17) :
                        isPermisoComercializacion ? (x.departamentoID == id || (x.departamentoID == 5 && x.estatus == true)) :
                        (
                            isPermisoSGC ? x.departamentoID == id :
                            isPermisoCDI ? (x.id == 35 || x.id == 55 || x.id == 56 || x.id == 64 || x.departamentoID == id) : (x.departamentoID == id && x.id != 8)
                        )
                    )
                ) && (x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO)).ToList();

            foreach (var i in result)
            {
                i.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id.Equals(i.departamentoID));
            }

            return result;
        }
        public List<EncuestaDTO> getEncuestasTodasByDepto(int id)
        {
            var result = _context.tblEN_Encuesta.Where(x => x.departamentoID == id && x.estatus == true).Select(y => new EncuestaDTO
            {
                id = y.id,
                titulo = y.titulo,
                descripcion = y.descripcion,
                tipo = y.tipo,
                creadorID = y.creadorID,
                departamentoID = y.departamentoID,
                departamento = y.departamento.descripcion
            }).ToList();
            return result;
        }
        public List<EncuestaDTO> getEncuestasTodasByDeptoConEncuestaID(int id)
        {
            var departamentoID = _context.tblEN_Encuesta.Where(x => x.id == id).Select(y => y.departamentoID).FirstOrDefault();
            var result = _context.tblEN_Encuesta.Where(x => x.departamentoID == departamentoID && x.estatus == true).Select(y => new EncuestaDTO
            {
                id = y.id,
                titulo = y.titulo,
                descripcion = y.descripcion,
                tipo = y.tipo,
                creadorID = y.creadorID,
                departamentoID = y.departamentoID,
                departamento = y.departamento.descripcion
            }).ToList();
            return result;
        }

        public List<tblEN_Encuesta> getEncuestasTodosDepto()
        {
            var result = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO).ToList();
            foreach (var i in result)
            {
                i.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id.Equals(i.departamentoID));
            }
            return result;
        }
        //public List<tblEN_Encuesta> getEncuestasPorPermisosCheck(int idDepartamento)
        //{
        //    var usuarioID = vSesiones.sesionUsuarioDTO.id;
        //    var permisos = _context.tblEN_Encuesta_Check_Usuario.Where(x =>
        //        x.usuarioID == usuarioID && x.estatus &&
        //        (x.ver == true || x.editar == true || x.enviar == true || x.contestaTelefonica == true || x.recibeNotificacion == true)
        //        ).ToList();
        //    var encuestasPermisos = permisos.Select(x => x.encuestaID).ToList();

        //    var encuestasPorPermisosCheck = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO && encuestasPermisos.Contains(x.id)).ToList();

        //    foreach (var i in encuestasPorPermisosCheck)
        //    {
        //        i.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id.Equals(i.departamentoID));
        //    }

        //    return encuestasPorPermisosCheck;
        //}

        public List<tblEN_Encuesta> getEncuestasPorPermisosCheck(int usuarioId)
        {
            var encuestas = _context.tblEN_Encuesta_Check_Usuario.Where
                (w =>
                    w.estatus &&
                    (usuarioId == 0 ? true : w.usuarioID == usuarioId) &&
                    w.Encuesta.estatusAutoriza == (int)estatusEnum.AUTORIZADO &&
                    w.Encuesta.estatus
                ).Select(m => m.Encuesta).Distinct().ToList();

            return encuestas;
        }

        public List<ClienteEmpresaDTO> getClienteEmpresas(DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new List<ClienteEmpresaDTO>();
            var usuarios = _context.tblP_Usuario.Where(x => x.cliente == true && x.estatus == true).Select(y => y.id).ToList();
            var usuariosEncuestasEnviadas = _context.tblEN_Encuesta_Usuario.Where(x => usuarios.Contains(x.usuarioResponderID) && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).Select(y => y.usuarioResponderID).ToList();
            var usuariosFinales = _context.tblP_Usuario.Where(x => usuariosEncuestasEnviadas.Contains(x.id)).ToList();
            foreach (var u in usuariosFinales)
            {
                var obj = new ClienteEmpresaDTO();

                obj.empresa = (u.empresa != null && u.empresa != "") ? u.empresa : (u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno);
                obj.clienteID = u.id;
                obj.cliente = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno;

                result.Add(obj);
            }
            return result;
        }

        public List<tblP_Departamento> getDepartamentos()
        {
            var result = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO).ToList();
            foreach (var i in result)
            {
                i.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id.Equals(i.departamentoID));
            }
            var depID = result.Select(x => x.departamentoID).ToList();
            //var resultado = _context.tblP_Departamento.Where(x => depID.Contains(x.id)).ToList();
            var resultado = _context.tblP_Departamento.ToList();
            return resultado;
        }

        public int savePregunta(tblEN_Preguntas obj)
        {
            IObjectSet<tblEN_Preguntas> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_Preguntas>();
            if (!ExistsP(obj))
            {
                if (obj == null) { throw new ArgumentNullException("Entity"); }
                _objectSet.AddObject(obj);
                _context.SaveChanges();
                //SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.AGREGAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                //SaveEntity(obj, (int)BitacoraEnum.MINUTA);
            }
            else
            {
                tblEN_Preguntas existing = _context.tblEN_Preguntas.Find(obj.id);
                if (existing != null)
                {
                    existing.pregunta = obj.pregunta;
                    _context.SaveChanges();
                    //SaveBitacora((int)BitacoraEnum.MINUTA, (int)AccionEnum.ACTUALIZAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
                }
                //SaveEntityWithDelet(obj, (int)BitacoraEnum.MINUTA);
            }
            return obj.id;
        }
        public void delPregunta(int id)
        {
            IObjectSet<tblEN_Preguntas> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblEN_Preguntas>();
            var p = _context.tblEN_Preguntas.FirstOrDefault(x => x.id == id);
            _objectSet.DeleteObject(p);
            _context.SaveChanges();
            //SaveBitacora((int)BitacoraEnum.PARTICIPANTE, (int)AccionEnum.ELIMINAR, obj.id, JsonUtils.convertNetObjectToJson(obj));
        }

        public List<EncuestaResultsDTO> getEncuestaResults(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new List<EncuestaResultsDTO>();
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var rep = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();
            var unicos = rep.Select(x => x.encuestaUsuarioID).ToList().Distinct().ToList();
            var temp = _context.tblEN_Encuesta_Usuario.Where(x => unicos.Contains(x.id)).ToList();
            var u = _context.tblP_Usuario.ToList();
            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
            string encuestaNombre = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id).titulo;

            foreach (var i in temp.OrderBy(x => x.fecha))
            {
                var o = new EncuestaResultsDTO();

                o.id = i.id;
                o.asunto = i.asunto ?? "";
                o.encuestaID = i.encuestaID;
                o.encuestaNombre = encuestaNombre;

                var envioA = en.FirstOrDefault(x => x.objID == i.id);

                if (envioA != null)
                {
                    var uen = u.FirstOrDefault(x => x.id == envioA.userEnviaID);

                    o.usuarioEnvioID = uen.id;
                    o.usuarioEnvioNombre = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;
                }
                else
                {
                    o.usuarioEnvioID = 0;
                    o.usuarioEnvioNombre = "";
                }

                o.usuarioResponderID = i.usuarioResponderID;

                var uRE = u.FirstOrDefault(x => x.id == i.usuarioResponderID);

                if (uRE != null)
                {
                    var objRawEnk = usuarioFactoryServices.getUsuarioService().getPersonalByClaveEmpleado(uRE.cveEmpleado);
                    if (objRawEnk.FirstOrDefault() != null)
                        o.Proyecto = objRawEnk.FirstOrDefault().cc + (uRE.cliente ? " / " + uRE.empresa : "");
                    else
                        o.Proyecto = "" + (uRE.cliente ? uRE.empresa : "");
                }

                var ut = u.FirstOrDefault(x => x.id == i.usuarioResponderID);

                o.usuarioResponderNombre = ut.nombre + " " + ut.apellidoPaterno + " " + ut.apellidoMaterno;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaRespndio = rep.FirstOrDefault(x => x.encuestaUsuarioID == i.id).fecha.ToShortDateString();
                o.fechaRespndioValue = rep.FirstOrDefault(x => x.encuestaUsuarioID == i.id).fecha;
                o.comentario = i.comentario;
                o.ver = "<button onclick='fnVerEncuesta(" + i.id + ")'><i class='fa fa-eye'></i>" + (rep.Where(x => x.encuestaUsuarioID == i.id && x.calificacion <= 3).Count() > 0 ? "<i class='glyphicon glyphicon-warning-sign' style='color:red;'></i>" : "") + "</button>";

                var preguntas = rep.Where(x => x.encuestaUsuarioID == i.id).ToList();
                var preguntasCant = preguntas.Count;
                var preguntasSum = preguntas.Sum(x => x.calificacion);
                var cal100 = preguntasCant * 5;
                var cal = cal100 == 0 ? 0 : Math.Round(((preguntasSum * 100) / cal100), 2);

                o.calificacion = cal + "%";
                o.calificacionPorcentajePromedio = i.calificacion != null ? i.calificacion + "%" : "";
                o.descarga = (!string.IsNullOrEmpty(i.rutaArchivo) ? "<button onclick='fnDownloadFileEncuesta("+o.id+")'><i class='glyphicon glyphicon-download-alt'></i></button>" : "");

                o.tipoRespuesta = i.tipoRespuesta;
                o.tipoRespuestaDesc = ((TipoRespuestaEncuestaEnum)i.tipoRespuesta).ToString();

                result.Add(o);
            }

            return result.OrderByDescending(x => x.fechaRespndioValue).ToList();
        }
        void enviaCorreoAsignado(tblEN_Encuesta enc)
        {
            var lstAsing = _context.tblEN_EncuestaAsignaUsuario.ToList().Where(w => w.encuestaID.Equals(enc.id)).ToList();
            if (lstAsing.Count > 0)
	        {
                var lstUsuarios = _context.tblP_Usuario.ToList();
                var lstClientes = _context.tblEN_Encuesta_Usuario.Where(eu => eu.encuestaID.Equals(enc.id) && lstUsuarios.Any(u => u.cliente && u.id.Equals(eu.usuarioResponderID))).ToList();
                if (lstClientes.Count > 0)
                {
                    var lstClienteNom = lstClientes.Select(s => string.Format("{0} {1} {2}", s.usuarioResponder.nombre, s.usuarioResponder.apellidoPaterno, s.usuarioResponder.apellidoMaterno)).ToList();
                    var creador = lstUsuarios.Where(x => x.id == enc.creadorID).FirstOrDefault();
                    var creadorNombre = creador.nombre + " " + creador.apellidoPaterno + " " + creador.apellidoMaterno;
                    var subject = "Encuesta Autorizada de clientes externos";
                    var body = string.Format(@"La Encuesta '{0}' ha sido autorizada. <br/> Hora: {1}</br> Para dar el seguimiento adecuado de clientes externos.</br> {2}."
                        , enc.titulo
                        , DateTime.Now.ToShortTimeString()
                        , lstClienteNom.ToLine(","));
                    var lsCorreo = lstUsuarios.Where(u => lstAsing.Any(a => a.usuarioID.Equals(u.id))).Select(s => s.correo).Distinct().ToList();
                    GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, lsCorreo);   
                }
	        }
        }
        public void setAceptarEncuesta(int id)
        {
            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id);
            temp.estatusAutoriza = (int)estatusEnum.AUTORIZADO;
            _context.SaveChanges();

            var creador = _context.tblP_Usuario.Where(x => x.id == temp.creadorID).FirstOrDefault();
            var creadorNombre = creador.nombre + " " + creador.apellidoPaterno + " " + creador.apellidoMaterno;

            var subject = "Encuesta Autorizada";
            var body = @"La Encuesta '" + temp.titulo + "' ha sido autorizada." + "<br/> Hora: " + DateTime.Now;
            List<string> creadorCorreo = new List<string>() { creador.correo };

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, creadorCorreo);

            //al autorizar encuesta, se le dan los permisos al usuario que la autorizó.
            var checkUsuario = new tblEN_Encuesta_Check_Usuario();
            checkUsuario.encuestaID = temp.id;
            checkUsuario.usuarioID = vSesiones.sesionUsuarioDTO.id;
            checkUsuario.ver = true;
            checkUsuario.editar = true;
            checkUsuario.enviar = true;
            checkUsuario.contestaTelefonica = true;
            checkUsuario.recibeNotificacion = false;
            checkUsuario.contestaPapel = true;
            checkUsuario.estatus = true;
            checkUsuario.crear = false;

            _context.tblEN_Encuesta_Check_Usuario.Add(checkUsuario);
            _context.SaveChanges();

            //enviaCorreoAsignado(temp);
        }
        public void setRechazarEncuesta(int id)
        {
            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id);
            temp.estatusAutoriza = (int)estatusEnum.RECHAZADO;
            _context.SaveChanges();

            var creador = _context.tblP_Usuario.Where(x => x.id == temp.creadorID).FirstOrDefault();
            var creadorNombre = creador.nombre + " " + creador.apellidoPaterno + " " + creador.apellidoMaterno;

            var subject = "Encuesta Rechazada";
            var body = @"La Encuesta '" + temp.titulo + "' ha sido rechazada." + "<br/> Hora: " + DateTime.Now;
            List<string> creadorCorreo = new List<string>() { creador.correo };

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, creadorCorreo);
        }
        public void setAceptarEncuestaUpdate(int id)
        {
            var tempUP = _context.tblEN_Encuesta_Update.FirstOrDefault(x => x.id == id);
            tempUP.estatus = (int)estatusEnum.AUTORIZADO;
            _context.SaveChanges();

            var result = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == tempUP.encuestaID);
            var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<tblEN_Encuesta>(tempUP.objData);

            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            _context.SaveChanges();

            foreach (var i in temp.preguntas)
            {
                if (i.estatus.Equals("Agregar"))
                {
                    var o = new tblEN_Preguntas();
                    o.encuestaID = result.id;
                    o.visible = true;
                    o.pregunta = i.pregunta;
                    _context.tblEN_Preguntas.Add(o);
                    _context.SaveChanges();
                }
                else if (i.estatus.Equals("Actualizar"))
                {
                    var o = _context.tblEN_Preguntas.FirstOrDefault(x => x.id == i.id);
                    o.encuestaID = result.id;
                    o.visible = true;
                    o.pregunta = i.pregunta;
                    _context.SaveChanges();
                }
                else if (i.estatus.Equals("Eliminar"))
                {
                    var o = _context.tblEN_Preguntas.FirstOrDefault(x => x.id == i.id);
                    o.visible = false;
                    _context.SaveChanges();
                }
            }

            var creador = _context.tblP_Usuario.Where(x => x.id == temp.creadorID).FirstOrDefault();
            var creadorNombre = creador.nombre + " " + creador.apellidoPaterno + " " + creador.apellidoMaterno;

            var subject = "Encuesta Modificada";
            var body = @"La Encuesta '" + temp.titulo + "' ha sido autorizada y modificada." + "<br/> Hora: " + DateTime.Now;
            List<string> creadorCorreo = new List<string>() { creador.correo };

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, creadorCorreo);
        }
        public void setRechazarEncuestaUpdate(int id)
        {
            var temp = _context.tblEN_Encuesta_Update.FirstOrDefault(x => x.id == id);
            temp.estatus = (int)estatusEnum.RECHAZADO;
            _context.SaveChanges();

            var encuesta = _context.tblEN_Encuesta.Where(x => x.id == temp.encuestaID).FirstOrDefault();
            var creador = _context.tblP_Usuario.Where(x => x.id == encuesta.creadorID).FirstOrDefault();
            var creadorNombre = creador.nombre + " " + creador.apellidoPaterno + " " + creador.apellidoMaterno;

            var subject = "Encuesta Rechazada";
            var body = @"La Encuesta '" + encuesta.titulo + "' ha sido rechazada." + "<br/> Hora: " + DateTime.Now;
            List<string> creadorCorreo = new List<string>() { creador.correo };

            GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, creadorCorreo);
        }

        public List<EncuestaAsignaUsuarioDTO> getEncuestaAsignaUsuario()
        {
            var relUser = _context.tblEN_EncuestaAsignaUsuario.ToList();
            var lstUsuario = _context.tblP_Usuario.ToList();
            var deps = _context.tblP_Departamento.ToList();
            var lstEnc = _context.tblEN_Encuesta.Where(w => w.estatus && w.estatusAutoriza == (int)estatusEnum.AUTORIZADO).ToList();
            var result = lstEnc.Select(enc => new EncuestaAsignaUsuarioDTO()
            {
                id = enc.id,
                titulo = enc.titulo,
                departamento = deps.FirstOrDefault(x => x.id == enc.departamentoID).descripcion,
                descripcion = enc.descripcion,
                fecha = enc.fecha.ToShortDateString(),
                fechaValue = enc.fecha,
                usuarios = lstUsuario.Where(u => relUser.Any(ru => ru.encuestaID.Equals(enc.id) && ru.usuarioID.Equals(u.id))).Select(s => new EncuestaCheckUsuarioDTO()
                {
                    id = s.id,
                    nombre = string.Format("{0} {1} {2}", s.nombre, s.apellidoPaterno, s.apellidoMaterno),
                    nombreUsuario = s.nombreUsuario,
                    puestoDescripcion = s.puesto.descripcion,
                    correo = s.correo
                }).ToList()
            }).ToList();
            return result.ToList();
        }

        public List<EncuestaEstatusDTO> getEncuestaPendiente()
        {
            var result = new List<EncuestaEstatusDTO>();
            var todas = _context.tblEN_Encuesta.ToList();
            var data = _context.tblEN_Encuesta.Where(x => x.estatusAutoriza == (int)estatusEnum.PENDIENTE).ToList();
            var deps = _context.tblP_Departamento.ToList();
            var u = _context.tblP_Usuario.ToList();
            var dataUpdate = _context.tblEN_Encuesta_Update.Where(x => x.estatus == (int)EncuestaEnum.PENDIENTE);

            foreach (var i in data.OrderBy(x => x.fecha))
            {

                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Crear";
                o.titulo = i.titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == i.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            foreach (var i in dataUpdate)
            {
                var d = JsonConvert.DeserializeObject<tblEN_Encuesta>(i.objData);
                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Actualizar";
                o.titulo = todas.FirstOrDefault(x => x.id == d.id).titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == d.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            return result.OrderBy(x => x.fechaValue).ToList();
        }
        public List<EncuestaEstatusDTO> getEncuestaAceptada()
        {
            var result = new List<EncuestaEstatusDTO>();
            var todas = _context.tblEN_Encuesta.ToList();
            var data = _context.tblEN_Encuesta.Where(x => x.estatusAutoriza == (int)estatusEnum.AUTORIZADO).ToList();
            var deps = _context.tblP_Departamento.ToList();
            var u = _context.tblP_Usuario.ToList();
            var dataUpdate = _context.tblEN_Encuesta_Update.Where(x => x.estatus == (int)EncuestaEnum.AUTORIZADA);

            foreach (var i in data.OrderBy(x => x.fecha))
            {

                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Crear";
                o.titulo = i.titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == i.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            foreach (var i in dataUpdate)
            {
                var d = JsonConvert.DeserializeObject<tblEN_Encuesta>(i.objData);
                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Actualizar";
                o.titulo = todas.FirstOrDefault(x => x.id == d.id).titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == d.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            return result.OrderBy(x => x.fechaValue).ToList();
        }
        public List<EncuestaEstatusDTO> getEncuestaRechazada()
        {
            var result = new List<EncuestaEstatusDTO>();
            var todas = _context.tblEN_Encuesta.ToList();
            var data = _context.tblEN_Encuesta.Where(x => x.estatusAutoriza == (int)estatusEnum.RECHAZADO).ToList();
            var deps = _context.tblP_Departamento.ToList();
            var u = _context.tblP_Usuario.ToList();
            var dataUpdate = _context.tblEN_Encuesta_Update.Where(x => x.estatus == (int)EncuestaEnum.RECHAZADA);

            foreach (var i in data.OrderBy(x => x.fecha))
            {

                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Crear";
                o.titulo = i.titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == i.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuesta(" + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            foreach (var i in dataUpdate)
            {
                var d = JsonConvert.DeserializeObject<tblEN_Encuesta>(i.objData);
                var o = new EncuestaEstatusDTO();
                o.id = i.id;
                o.tipo = "Actualizar";
                o.titulo = todas.FirstOrDefault(x => x.id == d.id).titulo;
                o.departamento = deps.FirstOrDefault(x => x.id == d.departamentoID).descripcion;
                o.fecha = i.fecha.ToShortDateString();
                o.fechaValue = i.fecha;
                o.btnVer = "<button class='btn btn-info' onclick='fnVerEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-eye-open'></i></button>";
                o.btnAceptar = "<button class='btn btn-primary' onclick='fnAceptarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-ok'></i></button>";
                o.btnRechazar = "<button class='btn btn-danger' onclick='fnRechazarEncuestaUpdate(" + d.id + "," + i.id + ")'><i class='glyphicon glyphicon-remove'></i></button>";
                result.Add(o);
            }
            return result.OrderBy(x => x.fechaValue).ToList();
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumen(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();
            var data = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();
            //var unicos = data.Select(x=>x.encuestaUsuarioID);
            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == id
                //&& x.visible == true
                ).Distinct().ToList();

            var preguntasIDPorResultados = data.Select(x => x.preguntaID).Distinct();
            var preguntasPorResultados = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && preguntasIDPorResultados.Contains(x.id)).OrderBy(w => w.pregunta).ToList();

            var preguntasIDActuales = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && x.visible == true).Select(x => x.id).ToList();

            var preguntasPorResultadosAnotacion = preguntasPorResultados.Where(x => !preguntasIDActuales.Contains(x.id)).Select(c =>
            {
                c.pregunta = "(Versión Anterior) " + c.pregunta; return c;
            }).ToList();

            var listaPreguntasFinal = preguntasPorResultados.Where(x => preguntasIDActuales.Contains(x.id)).ToList();
            listaPreguntasFinal.AddRange(preguntasPorResultadosAnotacion);

            //foreach (var i in temp.OrderBy(x => x.pregunta))
            foreach (var i in listaPreguntasFinal)
            {

                var o = new EncuestaResults2DTO();

                o.preguntaID = i.id;
                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMesConPorcentajes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMesConPorcentajes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMesConPorcentajes(data, 3, i.id), 2);
                o.abril = Math.Round(getPromedioPorMesConPorcentajes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMesConPorcentajes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMesConPorcentajes(data, 6, i.id), 2);
                o.julio = Math.Round(getPromedioPorMesConPorcentajes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMesConPorcentajes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMesConPorcentajes(data, 9, i.id), 2);
                o.octubre = Math.Round(getPromedioPorMesConPorcentajes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMesConPorcentajes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMesConPorcentajes(data, 12, i.id), 2);
                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                result.Add(o);

            }

            var t = new EncuestaResults2DTO();

            t.pregunta = "TOTAL:";

            //t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            //t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            //t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);
            //t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            //t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            //t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);
            //t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            //t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            //t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);
            //t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            //t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            //t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);
            //t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);

            t.enero = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / listaPreguntasFinal.Count(), 2);
            t.febrero = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / listaPreguntasFinal.Count(), 2);
            t.marzo = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / listaPreguntasFinal.Count(), 2);
            t.abril = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / listaPreguntasFinal.Count(), 2);
            t.mayo = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / listaPreguntasFinal.Count(), 2);
            t.junio = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / listaPreguntasFinal.Count(), 2);
            t.julio = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / listaPreguntasFinal.Count(), 2);
            t.agosto = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / listaPreguntasFinal.Count(), 2);
            t.septiembre = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / listaPreguntasFinal.Count(), 2);
            t.octubre = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / listaPreguntasFinal.Count(), 2);
            t.noviembre = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / listaPreguntasFinal.Count(), 2);
            t.diciembre = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / listaPreguntasFinal.Count(), 2);
            t.total = listaPreguntasFinal.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / listaPreguntasFinal.Count(), 2);

            result.Add(t);

            return result;
        }
        public List<EncuestaResults2DTO> getEncuestaResultsResumenNumero(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

            var result = new List<EncuestaResults2DTO>();

            List<string> temp = new List<string>();

            temp.Add("CONTESTADAS");
            temp.Add("NO CONTESTADAS");

            var unicos = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).Select(x => x.encuestaUsuarioID).ToList();

            //var unicos = resp.Select(x => x.encuestaUsuarioID).ToList();

            var unicosTemp = _context.tblEN_Encuesta_Usuario.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).Select(x => x.id).ToList();

            //var unicosTemp = dataTemp.Select(x => x.id).ToList();

            unicos.AddRange(unicosTemp);

            var unicosFinal = unicos.Distinct();

            var data = _context.tblEN_Encuesta_Usuario.Where(x => unicosFinal.Contains(x.id)).ToList();

            foreach (var i in temp)
            {
                var o = new EncuestaResults2DTO();

                o = getTotalesPorMesTodos(i, data, i.Equals("CONTESTADAS") ? false : true);

                //o.pregunta = i;

                //o.enero = getTotalesPorMes(data, 1, i.Equals("CONTESTADAS") ? false : true);
                //o.febrero = getTotalesPorMes(data, 2, i.Equals("CONTESTADAS") ? false : true);
                //o.marzo = getTotalesPorMes(data, 3, i.Equals("CONTESTADAS") ? false : true);
                //o.abril = getTotalesPorMes(data, 4, i.Equals("CONTESTADAS") ? false : true);
                //o.mayo = getTotalesPorMes(data, 5, i.Equals("CONTESTADAS") ? false : true);
                //o.junio = getTotalesPorMes(data, 6, i.Equals("CONTESTADAS") ? false : true);
                //o.julio = getTotalesPorMes(data, 7, i.Equals("CONTESTADAS") ? false : true);
                //o.agosto = getTotalesPorMes(data, 8, i.Equals("CONTESTADAS") ? false : true);
                //o.septiembre = getTotalesPorMes(data, 9, i.Equals("CONTESTADAS") ? false : true);
                //o.octubre = getTotalesPorMes(data, 10, i.Equals("CONTESTADAS") ? false : true);
                //o.noviembre = getTotalesPorMes(data, 11, i.Equals("CONTESTADAS") ? false : true);
                //o.diciembre = getTotalesPorMes(data, 12, i.Equals("CONTESTADAS") ? false : true);

                //o.total = (o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre);

                result.Add(o);
            }

            var t = new EncuestaResults2DTO();

            t.pregunta = "TOTAL ENCUESTAS ENVIADAS";

            t.enero = result.Sum(x => x.enero);
            t.febrero = result.Sum(x => x.febrero);
            t.marzo = result.Sum(x => x.marzo);
            t.abril = result.Sum(x => x.abril);
            t.mayo = result.Sum(x => x.mayo);
            t.junio = result.Sum(x => x.junio);
            t.julio = result.Sum(x => x.julio);
            t.agosto = result.Sum(x => x.agosto);
            t.septiembre = result.Sum(x => x.septiembre);
            t.octubre = result.Sum(x => x.octubre);
            t.noviembre = result.Sum(x => x.noviembre);
            t.diciembre = result.Sum(x => x.diciembre);

            t.total = result.Sum(x => x.total);

            result.Add(t);

            return result;
        }
        public decimal getPromedioPorMes(List<tblEN_Resultado> data, int mes, int pregunta)
        {
            var lista = data.Where(x => x.fecha.Month == mes && x.preguntaID.Equals(pregunta)).ToList();
            var promedio = lista.Count() > 0 ? ((lista.Where(x => x.preguntaID == pregunta).Sum(x => x.calificacion) / lista.Where(x => x.preguntaID == pregunta).Count()) * 100) / 5 : 0;
            return decimal.Round(promedio, 2);
        }
        public decimal getPromedioPorMesConPorcentajes(List<tblEN_Resultado> data, int mes, int pregunta)
        {
            var lista = data.Where(x => x.fecha.Month == mes && x.preguntaID.Equals(pregunta)).ToList();
            var promedio = lista.Count() > 0 ? (lista.Sum(x => (x.porcentaje != null ? x.porcentaje.Value : 0)) / lista.Count()) : 0;

            return decimal.Round(promedio, 2);
        }
        public decimal getTotalesPorMes(List<tblEN_Encuesta_Usuario> data, int mes, bool estatus)
        {
            decimal total = 0;
            foreach (var i in data)
            {
                if (i.id == 194)
                {

                }
                var o = _context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == i.id);
                if (o == null)
                {
                    i.estatus = true;
                }
                else
                {
                    if (i.fecha.Month == o.fecha.Month)
                    {
                        i.estatus = false;
                        i.fecha = o.fecha;
                    }
                    else
                    {
                        i.estatus = true;
                        i.fecha = o.fecha;
                    }
                }
            }
            total = data.Where(x => x.fecha.Month == mes && x.estatus == estatus).Count();
            return total;
        }
        public EncuestaResults2DTO getTotalesPorMesTodos(string pregunta, List<tblEN_Encuesta_Usuario> data, bool estatus)
        {
            var res = new EncuestaResults2DTO();

            res.pregunta = pregunta;

            foreach (var i in data)
            {
                var o = _context.tblEN_Resultado.FirstOrDefault(x => x.encuestaUsuarioID == i.id);

                if (o == null)
                {
                    i.estatus = true;
                }
                else
                {
                    if (i.fecha.Month == o.fecha.Month)
                    {
                        i.estatus = false;
                        i.fecha = o.fecha;
                    }
                    else
                    {
                        i.estatus = true;
                        i.fecha = o.fecha;
                    }
                }
            }

            res.enero = data.Where(x => x.fecha.Month == 1 && x.estatus == estatus).Count();
            res.febrero = data.Where(x => x.fecha.Month == 2 && x.estatus == estatus).Count();
            res.marzo = data.Where(x => x.fecha.Month == 3 && x.estatus == estatus).Count();
            res.abril = data.Where(x => x.fecha.Month == 4 && x.estatus == estatus).Count();
            res.mayo = data.Where(x => x.fecha.Month == 5 && x.estatus == estatus).Count();
            res.junio = data.Where(x => x.fecha.Month == 6 && x.estatus == estatus).Count();
            res.julio = data.Where(x => x.fecha.Month == 7 && x.estatus == estatus).Count();
            res.agosto = data.Where(x => x.fecha.Month == 8 && x.estatus == estatus).Count();
            res.septiembre = data.Where(x => x.fecha.Month == 9 && x.estatus == estatus).Count();
            res.octubre = data.Where(x => x.fecha.Month == 10 && x.estatus == estatus).Count();
            res.noviembre = data.Where(x => x.fecha.Month == 11 && x.estatus == estatus).Count();
            res.diciembre = data.Where(x => x.fecha.Month == 12 && x.estatus == estatus).Count();

            res.total = (res.enero + res.febrero + res.marzo + res.abril + res.mayo + res.junio + res.julio + res.agosto + res.septiembre + res.octubre + res.noviembre + res.diciembre);

            return res;
        }

        public decimal getPromediototal(EncuestaResults2DTO o)
        {
            decimal r = 0;
            if (o.enero > 0)
            {
                r++;
            }
            if (o.febrero > 0)
            {
                r++;
            }
            if (o.marzo > 0)
            {
                r++;
            }
            if (o.abril > 0)
            {
                r++;
            }
            if (o.mayo > 0)
            {
                r++;
            }
            if (o.junio > 0)
            {
                r++;
            }
            if (o.julio > 0)
            {
                r++;
            }
            if (o.agosto > 0)
            {
                r++;
            }
            if (o.septiembre > 0)
            {
                r++;
            }
            if (o.octubre > 0)
            {
                r++;
            }
            if (o.noviembre > 0)
            {
                r++;
            }
            if (o.diciembre > 0)
            {
                r++;
            }

            return r.Equals(0) ? 1 : r;
        }
        public List<EncuestaResultsDTO> getEncuestaResultsNoContestadas(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            //var rep = _context.tblEN_Resultado.Where(x => x.encuestaID == id).ToList();
            var result = new List<EncuestaResultsDTO>();
            var temp = _context.tblEN_Encuesta_Usuario.Where(x => x.encuestaID == id && x.estatus == true
                && (x.fecha >= fechaInicio && x.fecha <= fechaFin)
                );
            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
            if (temp.Count() > 0)
            {
                var current = temp.FirstOrDefault().usuarioResponderID;
                var u = _context.tblP_Usuario.ToList();
                string encuestaNombre = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id).titulo;
                foreach (var i in temp.OrderBy(x => x.fecha))
                {

                    var o = new EncuestaResultsDTO();
                    o.id = i.id;
                    o.asunto = i.asunto ?? "";
                    o.encuestaID = i.encuestaID;
                    var envioA = en.FirstOrDefault(x => x.objID == i.id);
                    if (envioA != null)
                    {
                        var uen = u.FirstOrDefault(x => x.id == envioA.userEnviaID);
                        o.usuarioEnvioID = uen.id;
                        o.usuarioEnvioNombre = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;
                    }
                    else
                    {
                        o.usuarioEnvioID = 0;
                        o.usuarioEnvioNombre = "";
                    }
                    o.encuestaNombre = encuestaNombre;
                    o.usuarioResponderID = i.usuarioResponderID;
                    var ut = u.FirstOrDefault(x => x.id == i.usuarioResponderID);
                    o.usuarioResponderNombre = ut.nombre + " " + ut.apellidoPaterno + " " + ut.apellidoMaterno;
                    o.fecha = i.fecha.ToShortDateString();

                    var objRawEnk = usuarioFactoryServices.getUsuarioService().getPersonalByClaveEmpleado(ut.cveEmpleado);
                    if (objRawEnk.FirstOrDefault() != null)
                        o.Proyecto = objRawEnk.FirstOrDefault().cc + " / " + ut.empresa;
                    else
                        o.Proyecto = "" + ut.empresa; ;

                    //o.fechaRespndio = rep.FirstOrDefault(x => x.encuestaUsuarioID == i.id).fecha.ToShortDateString();
                    //o.fechaRespndioValue = rep.FirstOrDefault(x => x.encuestaUsuarioID == i.id).fecha;
                    //o.comentario = i.comentario;
                    o.ver = "<button onclick='fnVerEncuesta(" + i.encuestaID + "," + i.usuarioResponderID + ")'><i class='fa fa-eye'></i></button>";
                    result.Add(o);
                }
            }
            return result;
        }
        public List<GraficaEncuestaDTO> getGraficaByEncuesta(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            var result = new List<GraficaEncuestaDTO>();

            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);

            var listaResultadosEncuestaFiltros = _context.tblEN_Resultado.ToList().Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            var preguntasManejadas = listaResultadosEncuestaFiltros.Select(x => x.preguntaID).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && preguntasManejadas.Contains(x.id)).Select(x => new GraficaEncuestaDTO
            {
                preguntaID = x.id,
                preguntaDescripcion = x.pregunta,
                calificacion = 0
            }).ToList();

            var resultados = getEncuestaResultsResumen(id, fechaInicio, fechaFin);

            foreach (var i in preguntas)
            {
                var reg = listaResultadosEncuestaFiltros.Where(x => x.preguntaID == i.preguntaID).ToList();

                //i.calificacion = reg.Count > 0 ? ((reg.Sum(x => x.calificacion) / reg.Count) * 100) / 5 : 0;
                //i.calificacion = reg.Count > 0 ? (reg.Sum(x => (x.porcentaje != null ? x.porcentaje.Value : 0)) / reg.Count()) : 0;

                i.calificacion = resultados.FirstOrDefault(x => x.preguntaID == i.preguntaID) != null ? resultados.FirstOrDefault(x => x.preguntaID == i.preguntaID).total : 0;

                result.Add(i);
            }

            return result;
        }
        public void sendEncuesta(string asunto, SendEncuestaDTO obj)
        {
            var m = new UsuarioDAO();
            var cadena = Encriptacion.encriptar(m.getPassByID(obj.encuestadoID).nombreUsuario) + "@" + m.getPassByID(obj.encuestadoID).contrasena;
            var temp = new tblEN_Encuesta_Usuario();
            temp.encuestaID = obj.encuestaID;
            temp.usuarioResponderID = obj.encuestadoID;
            temp.fecha = DateTime.Now;
            temp.estatus = true;
            temp.asunto = asunto;
            temp.tipoRespuesta = (int)TipoRespuestaEncuestaEnum.SISTEMA;
            _context.tblEN_Encuesta_Usuario.Add(temp);
            _context.SaveChanges();
            var enc = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == obj.encuestaID);
            var depto = _context.tblP_Departamento.FirstOrDefault(x => x.id == enc.departamentoID);

            var a = new tblP_Alerta();
            a.msj = "Encuesta: " + obj.encuestaNombre;
            a.sistemaID = 8;
            a.userEnviaID = vSesiones.sesionUsuarioDTO.id;
            a.userRecibeID = obj.encuestadoID;
            a.tipoAlerta = (int)AlertasEnum.REDIRECCION;
            a.url = "/Encuestas/Encuesta/Responder/?encuesta=" + temp.id;
            a.objID = temp.id;
            AlertaFactoryServices alertaFactoryServices = new AlertaFactoryServices();
            alertaFactoryServices.getAlertaService().saveAlerta(a);
            try
            {
                var c = _context.tblP_Usuario.FirstOrDefault(x => x.id == obj.encuestadoID);
                var subject = "Encuesta: " + _context.tblEN_Encuesta.FirstOrDefault(x => x.id == obj.encuestaID).titulo;
                var body = @"Estimado(a): " + c.nombre.ToUpper() + " " + c.apellidoPaterno.ToUpper() + " " + c.apellidoMaterno.ToUpper() + "<br/>Nos interesa mucho contar con tu opinión con respecto a nuestros servicios, por ello te hemos enviado una breve encuesta que agradecemos nos ayudes a responder.<br/>Link: http://sigoplan.construplan.com.mx" + ((vSesiones.sesionEmpresaActual == 1) ? "" : ":8084") + "/Encuestas/Encuesta/Responder/?blob=" + cadena + "&encuesta=" + temp.id + "&empresa=" + vSesiones.sesionEmpresaActual + "<br/><br/>";
                body += @"Atentamente,<br/>" + depto.descripcion.ToUpper() + "<br/>Grupo Construcciones Planificadas S.A. de C.V.<br/>Por favor no responda a esta dirección de correo, si necesita contactarnos puede llamar al (52) 662-108-0500.";
                body += @"<br/><br/>P.D: El motivo de la encuesta es el siguiente:<br/>" + asunto;
                List<string> contactos = new List<string>();

                contactos.Add(c.correo);
                GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, contactos);

            }
            catch (Exception ex)
            {
            }
        }
        public void enviarCorreoUsuariosAsignados(List<SendEncuestaDTO> listado)
        {
            var encuesta = _context.tblEN_Encuesta.ToList().FirstOrDefault(x => x.id == listado[0].encuestaID);
            List<int> encuestadosID = listado.Select(x => x.encuestadoID).ToList();
            List<string> usuariosEncuestados = _context.tblP_Usuario.Where(x => encuestadosID.Contains(x.id)).Select(z => 
                z.nombre.ToUpper() + " " + z.apellidoPaterno.ToUpper() + " " + z.apellidoMaterno.ToUpper()
            ).ToList();
            var stringUsuariosEncuestados = string.Join(", ", usuariosEncuestados);

            //Checar si la encuesta es para clientes externos.
            if (encuesta.tipo == 2)
            {
                List<int> usuariosAsignados = _context.tblEN_EncuestaAsignaUsuario.Where(x => x.encuestaID == encuesta.id).Select(x => x.usuarioID).ToList();

                if (usuariosAsignados.Count > 0)
                {
                    List<string> correos = _context.tblP_Usuario.Where(x => usuariosAsignados.Contains(x.id) && x.correo != null).Select(y => y.correo).ToList();

                    var titulo = "Encuesta enviada a clientes externos.";
                    var mensaje =
                        string.Format(@"La encuesta '{0}' se ha enviado para responder a los siguientes clientes externos: {1}.", encuesta.titulo, stringUsuariosEncuestados);

                    if (usuariosAsignados.Count >= correos.Count)
                    {
                        GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), titulo), mensaje, correos);
                    }
                }
            }
        }
        public bool validarCantidadEncuestas(int id)
        {
            var temp = _context.tblEN_Encuesta.Where(x => x.departamentoID == id && x.estatus.Equals(true)).ToList();
            var value = id == 4 ? 0 : temp.Count();
            return value > 0 ? false : true;
        }
        public bool Exists(tblEN_Encuesta obj)
        {
            return _context.tblEN_Encuesta.Where(x => x.id == obj.id).ToList().Count > 0 ? true : false;
        }
        public bool ExistsP(tblEN_Preguntas obj)
        {
            return _context.tblEN_Preguntas.Where(x => x.id == obj.id && x.visible == true).ToList().Count > 0 ? true : false;
        }

        public List<Preguntas3EstrellasDTO> getPreguntas3Estrellas(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Preguntas3EstrellasDTO> result = new List<Preguntas3EstrellasDTO>();
            //var preguntas = _context.tblEN_Resultado.Where(x => x.encuestaID == id && x.calificacion <= 3).ToList();
            //var preguntasFiltroFecha = preguntas.Where(x => (x.fecha.Date >= fechaInicio.Date && x.fecha.Date <= fechaFin.Date));
            var preguntasFiltroFecha = _context.tblEN_Resultado.Where(x => x.encuestaID == id && x.calificacion <= 3 && x.fecha >= fechaInicio && x.fecha <= fechaFin).ToList();
            var idsPreguntasFiltrosFecha = preguntasFiltroFecha.Select(x => x.encuestaUsuarioID).ToList();
            var u = _context.tblP_Usuario.ToList();
            var p = _context.tblEN_Preguntas.Where(x => x.visible == true).ToList();
            var pd = _context.tblEN_Preguntas.Where(x => x.visible == false).ToList();
            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
            string encuestaNombre = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id).titulo;
            var encuestaAsunto = _context.tblEN_Encuesta_Usuario.Where(x => x.encuestaID == id && idsPreguntasFiltrosFecha.Contains(x.id)).ToList();
            foreach (var i in preguntasFiltroFecha)
            {
                var o = new Preguntas3EstrellasDTO();
                o.Encuesta = encuestaNombre;
                var envioA = en.FirstOrDefault(x => x.objID == i.encuestaUsuarioID);
                if (envioA != null)
                {
                    var uen = u.FirstOrDefault(x => x.id == envioA.userEnviaID);
                    o.usuarioEnvioID = uen.id;
                    o.usuarioEnvioNombre = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;

                }
                else
                {
                    o.usuarioEnvioID = 0;
                    o.usuarioEnvioNombre = "";
                }
                o.asunto = encuestaAsunto.FirstOrDefault(x => x.id == i.encuestaUsuarioID).asunto ?? "";
                var tp = p.FirstOrDefault(x => x.id == i.preguntaID);
                if (tp != null)
                {
                    o.Pregunta = tp.pregunta;
                }
                else
                {
                    o.Pregunta = pd.FirstOrDefault(x => x.id == i.preguntaID).pregunta;
                }
                o.Respuesta = i.respuesta;
                o.Fecha = i.fecha.ToShortDateString();
                o.Calificación = (int)i.calificacion;
                o.Respondio = u.FirstOrDefault(x => x.id == i.usuarioRespondioID).nombre + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoPaterno + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoMaterno;

                var ut = u.FirstOrDefault(x => x.id == i.usuarioRespondioID);
                var objRawEnk = usuarioFactoryServices.getUsuarioService().getPersonalByClaveEmpleado(ut.cveEmpleado);

                if (objRawEnk.FirstOrDefault() != null)
                    o.Proyecto = objRawEnk.FirstOrDefault().cc + " / " + ut.empresa;
                else
                    o.Proyecto = "" + ut.empresa; ;

                result.Add(o);
            }

            return result;
        }

        public List<Preguntas3EstrellasDTO> getPreguntas3EstrellasPorTipo(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Preguntas3EstrellasDTO> result = new List<Preguntas3EstrellasDTO>();

            var filtroTipo = _context.tblEN_Preguntas.Where(x => x.tipo == 1 || x.tipo == 2 || x.tipo == 3).Select(x => x.id).ToList();
            //var preguntasTotal = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && filtroTipo.Contains(x.preguntaID)).ToList();
            var preguntas = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && x.calificacion <= 3 && filtroTipo.Contains(x.preguntaID)).ToList();

            var u = _context.tblP_Usuario.ToList();
            //var p = _context.tblEN_Preguntas.Where(x => x.visible == true).ToList();
            var p = _context.tblEN_Preguntas.Where(x => x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();
            var pd = _context.tblEN_Preguntas.Where(x => x.visible == false && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();
            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
            string encuestaNombre = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id).titulo;
            var encuestaAsunto = _context.tblEN_Encuesta_Usuario.Where(x => x.encuestaID == id).ToList();
            foreach (var i in preguntas)
            {
                var o = new Preguntas3EstrellasDTO();
                o.Encuesta = encuestaNombre;
                var envioA = en.FirstOrDefault(x => x.objID == i.encuestaUsuarioID);
                if (envioA != null)
                {
                    var uen = u.FirstOrDefault(x => x.id == envioA.userEnviaID);
                    o.usuarioEnvioID = uen.id;
                    o.usuarioEnvioNombre = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;
                }
                else
                {
                    o.usuarioEnvioID = 0;
                    o.usuarioEnvioNombre = "";
                }
                o.asunto = encuestaAsunto.FirstOrDefault(x => x.id == i.encuestaUsuarioID).asunto ?? "";
                var tp = p.FirstOrDefault(x => x.id == i.preguntaID);
                if (tp != null)
                {
                    o.Pregunta = tp.pregunta;
                }
                else
                {
                    o.Pregunta = pd.FirstOrDefault(x => x.id == i.preguntaID).pregunta;
                }
                o.Respuesta = i.respuesta;
                o.Fecha = i.fecha.ToShortDateString();
                o.Calificación = (int)i.calificacion;
                o.Respondio = u.FirstOrDefault(x => x.id == i.usuarioRespondioID).nombre + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoPaterno + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoMaterno;
                result.Add(o);
            }

            return result;
        }

        public List<Preguntas3EstrellasDTO> getPreguntas3EstrellasPorTipoYusuario(int id, List<int> usuarios, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<Preguntas3EstrellasDTO> result = new List<Preguntas3EstrellasDTO>();

            var filtroTipo = _context.tblEN_Preguntas.Where(x => x.tipo == 1 || x.tipo == 2 || x.tipo == 3).Select(x => x.id).ToList();
            //var preguntasTotal = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && filtroTipo.Contains(x.preguntaID)).ToList();
            var preguntas = _context.tblEN_Resultado.Where(x => x.encuestaID == id && usuarios.Contains(x.usuarioRespondioID) && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && x.calificacion <= 3 && filtroTipo.Contains(x.preguntaID)).ToList();

            var u = _context.tblP_Usuario.ToList();
            //var p = _context.tblEN_Preguntas.Where(x => x.visible == true).ToList();
            var p = _context.tblEN_Preguntas.Where(x => x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();
            var pd = _context.tblEN_Preguntas.Where(x => x.visible == false && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();
            var en = _context.tblP_Alerta.Where(x => x.sistemaID == 8).ToList();
            string encuestaNombre = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id).titulo;
            var encuestaAsunto = _context.tblEN_Encuesta_Usuario.Where(x => x.encuestaID == id).ToList();
            foreach (var i in preguntas)
            {
                var o = new Preguntas3EstrellasDTO();
                o.Encuesta = encuestaNombre;
                var envioA = en.FirstOrDefault(x => x.objID == i.encuestaUsuarioID);
                if (envioA != null)
                {
                    var uen = u.FirstOrDefault(x => x.id == envioA.userEnviaID);
                    o.usuarioEnvioID = uen.id;
                    o.usuarioEnvioNombre = uen.nombre + " " + uen.apellidoPaterno + " " + uen.apellidoMaterno;
                }
                else
                {
                    o.usuarioEnvioID = 0;
                    o.usuarioEnvioNombre = "";
                }
                o.asunto = encuestaAsunto.FirstOrDefault(x => x.id == i.encuestaUsuarioID).asunto ?? "";
                var tp = p.FirstOrDefault(x => x.id == i.preguntaID);
                if (tp != null)
                {
                    o.Pregunta = tp.pregunta;
                }
                else
                {
                    o.Pregunta = pd.FirstOrDefault(x => x.id == i.preguntaID).pregunta;
                }
                o.Respuesta = i.respuesta;
                o.Fecha = i.fecha.ToShortDateString();
                o.Calificación = (int)i.calificacion;
                o.Respondio = u.FirstOrDefault(x => x.id == i.usuarioRespondioID).nombre + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoPaterno + " " + u.FirstOrDefault(x => x.id == i.usuarioRespondioID).apellidoMaterno;
                result.Add(o);
            }

            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorPregunta(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();
            var data = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();
            var threestars = getPreguntas3EstrellasPorTipo(id, fechaInicio, fechaFin).ToList();

            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);
                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);
                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);
                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);
                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);
            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);
            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);
            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);
            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = threestars.Count;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            result.Add(t);
            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorPreguntaYear(int id, DateTime fechaInicio, DateTime fechaFin)
        {
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();
            var data = _context.tblEN_Resultado.Where(x => x.encuestaID == id && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();
            var usuariosID = data.Select(x => x.usuarioRespondioID).ToList();
            var usuarios = _context.tblP_Usuario.Where(x => usuariosID.Contains(x.id)).ToList();

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();
            var threestars = getPreguntas3EstrellasPorTipo(id, fechaInicio, fechaFin).ToList();

            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == id && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);

                List<decimal> meses1 = new List<decimal>(new decimal[] { o.enero, o.febrero, o.marzo });
                if (meses1.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim1 = Math.Round((meses1.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim1 = 0;
                }

                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);

                List<decimal> meses2 = new List<decimal>(new decimal[] { o.abril, o.mayo, o.junio });
                if (meses2.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim2 = Math.Round((meses2.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim2 = 0;
                }

                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);

                List<decimal> meses3 = new List<decimal>(new decimal[] { o.julio, o.agosto, o.septiembre });
                if (meses3.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim3 = Math.Round((meses3.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim3 = 0;
                }

                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);

                List<decimal> meses4 = new List<decimal>(new decimal[] { o.octubre, o.noviembre, o.diciembre });
                if (meses4.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim4 = Math.Round((meses4.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim4 = 0;
                }

                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);

            List<decimal> meses5 = new List<decimal>(new decimal[] { t.enero, t.febrero, t.marzo });
            if (meses5.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim1 = Math.Round((meses5.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim1 = 0;
            }

            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);

            List<decimal> meses6 = new List<decimal>(new decimal[] { t.abril, t.mayo, t.junio });
            if (meses6.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim2 = Math.Round((meses6.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim2 = 0;
            }

            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);

            List<decimal> meses7 = new List<decimal>(new decimal[] { t.julio, t.agosto, t.septiembre });
            if (meses7.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim3 = Math.Round((meses7.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim3 = 0;
            }

            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);

            List<decimal> meses8 = new List<decimal>(new decimal[] { t.octubre, t.noviembre, t.diciembre });
            if (meses8.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim4 = Math.Round((meses8.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim4 = 0;
            }

            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = threestars.Count;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            result.Add(t);
            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamento(int tipoResumen, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin)
        {
            var encuestaID = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.id).FirstOrDefault();
            var encuesta = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.titulo).FirstOrDefault();
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();

            var puestos = _context.tblP_Puesto.Where(x => x.departamentoID == area2ID).Select(y => y.id).ToList();
            var usuarios = _context.tblP_Usuario.Where(x => puestos.Contains(x.puestoID)).Select(y => y.id).ToList();
            var resultadosFiltrados = _context.tblEN_Resultado.Where(x => usuarios.Contains(x.usuarioRespondioID)).ToList();
            var resultadosFiltrados2 = resultadosFiltrados.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            //var dataPrueba = _context.tblEN_Resultado.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();
            var data = resultadosFiltrados2;

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();
            //var unicos2 = _context.tblEN_Encuesta_Usuario.Where(x => usuarios.Contains(x.usuarioResponderID) && x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && x.estatus == false).ToList();

            var threestars = (encuestaID != 0) ? getPreguntas3EstrellasPorTipoYusuario(encuestaID, usuarios, fechaInicio, fechaFin).ToList() : null;

            //var unicosPrueba = _context.tblEN_Resultado.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).Select(y => y.encuestaUsuarioID).ToList();
            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0 && threestars != null)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);
                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);
                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);
                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);
                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);
            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);
            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);
            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);
            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = (threestars != null) ? threestars.Count : 0;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            t.encuesta = encuesta;
            result.Add(t);
            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorDepartamentoYear(int tipoResumen, int areaID, int area2ID, DateTime fechaInicio, DateTime fechaFin)
        {
            var encuestaID = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.id).FirstOrDefault();
            var encuesta = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.titulo).FirstOrDefault();
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();

            var puestos = _context.tblP_Puesto.Where(x => x.departamentoID == area2ID).Select(y => y.id).ToList();
            var usuarios = _context.tblP_Usuario.Where(x => puestos.Contains(x.puestoID)).Select(y => y.id).ToList();
            var resultadosFiltrados = _context.tblEN_Resultado.Where(x => usuarios.Contains(x.usuarioRespondioID)).ToList();
            var resultadosFiltrados2 = resultadosFiltrados.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            var data = resultadosFiltrados2;

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();

            var threestars = (encuestaID != 0) ? getPreguntas3EstrellasPorTipoYusuario(encuestaID, usuarios, fechaInicio, fechaFin).ToList() : null;

            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0 && threestars != null)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);

                List<decimal> meses1 = new List<decimal>(new decimal[] { o.enero, o.febrero, o.marzo });
                if (meses1.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim1 = Math.Round((meses1.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim1 = 0;
                }

                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);

                List<decimal> meses2 = new List<decimal>(new decimal[] { o.abril, o.mayo, o.junio });
                if (meses2.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim2 = Math.Round((meses2.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim2 = 0;
                }

                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);

                List<decimal> meses3 = new List<decimal>(new decimal[] { o.julio, o.agosto, o.septiembre });
                if (meses3.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim3 = Math.Round((meses3.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim3 = 0;
                }

                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);

                List<decimal> meses4 = new List<decimal>(new decimal[] { o.octubre, o.noviembre, o.diciembre });
                if (meses4.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim4 = Math.Round((meses4.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim4 = 0;
                }

                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);

            List<decimal> meses5 = new List<decimal>(new decimal[] { t.enero, t.febrero, t.marzo });
            if (meses5.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim1 = Math.Round((meses5.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim1 = 0;
            }

            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);

            List<decimal> meses6 = new List<decimal>(new decimal[] { t.abril, t.mayo, t.junio });
            if (meses6.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim2 = Math.Round((meses6.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim2 = 0;
            }

            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);

            List<decimal> meses7 = new List<decimal>(new decimal[] { t.julio, t.agosto, t.septiembre });
            if (meses7.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim3 = Math.Round((meses7.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim3 = 0;
            }

            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);

            List<decimal> meses8 = new List<decimal>(new decimal[] { t.octubre, t.noviembre, t.diciembre });
            if (meses8.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim4 = Math.Round((meses8.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim4 = 0;
            }

            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = (threestars != null) ? threestars.Count : 0;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            t.encuesta = encuesta;
            result.Add(t);
            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresa(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa)
        {
            var encuestaID = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.id).FirstOrDefault();
            var encuesta = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.titulo).FirstOrDefault();
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();

            var usuarios = _context.tblP_Usuario.Where(x => x.empresa == empresa).Select(y => y.id).ToList();
            var resultadosFiltrados = _context.tblEN_Resultado.Where(x => usuarios.Contains(x.usuarioRespondioID)).ToList();
            var resultadosFiltrados2 = resultadosFiltrados.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            var data = resultadosFiltrados2;

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();
            var unicos2 = _context.tblEN_Encuesta_Usuario.Where(x => usuarios.Contains(x.usuarioResponderID) && x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin) && x.estatus == false).ToList();

            var threestars = (encuestaID != 0) ? getPreguntas3EstrellasPorTipoYusuario(encuestaID, usuarios, fechaInicio, fechaFin).ToList() : null;

            var unicosPrueba = _context.tblEN_Resultado.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).Select(y => y.encuestaUsuarioID).ToList();
            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0 && threestars != null)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);
                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);
                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);
                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);
                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);
            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);
            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);
            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);
            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = (threestars != null) ? threestars.Count : 0;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            t.encuesta = encuesta;
            result.Add(t);
            return result;
        }

        public List<EncuestaResults2DTO> getEncuestaResultsResumenPorEmpresaYear(int tipoResumen, int areaID, DateTime fechaInicio, DateTime fechaFin, string empresa)
        {
            var encuestaID = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.id).FirstOrDefault();
            var encuesta = _context.tblEN_Encuesta.Where(x => x.departamentoID == areaID && x.tipo == tipoResumen).Select(y => y.titulo).FirstOrDefault();
            fechaFin = fechaFin.AddHours(23).AddMinutes(59).AddSeconds(59);
            var result = new List<EncuestaResults2DTO>();

            var usuarios = _context.tblP_Usuario.Where(x => x.empresa == empresa).Select(y => y.id).ToList();
            var resultadosFiltrados = _context.tblEN_Resultado.Where(x => usuarios.Contains(x.usuarioRespondioID)).ToList();
            var resultadosFiltrados2 = resultadosFiltrados.Where(x => x.encuestaID == encuestaID && (x.fecha >= fechaInicio && x.fecha <= fechaFin)).ToList();

            var data = resultadosFiltrados2;

            var unicos = data.Select(x => x.encuestaUsuarioID).Distinct().ToList();

            var threestars = (encuestaID != 0) ? getPreguntas3EstrellasPorTipoYusuario(encuestaID, usuarios, fechaInicio, fechaFin).ToList() : null;

            var temp = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true).Distinct();

            var preguntas = _context.tblEN_Preguntas.Where(x => x.encuestaID == encuestaID && x.visible == true && (x.tipo == 1 || x.tipo == 2 || x.tipo == 3)).ToList();

            double threestarsPercentage = 0;
            if (unicos.Count != 0 && preguntas.Count != 0 && threestars != null)
            {
                threestarsPercentage = (threestars.Count * 100.00) / (unicos.Count * preguntas.Count);
            }

            foreach (var i in temp.OrderBy(x => x.pregunta))
            {

                var o = new EncuestaResults2DTO();

                o.pregunta = i.pregunta;
                o.enero = Math.Round(getPromedioPorMes(data, 1, i.id), 2);
                o.febrero = Math.Round(getPromedioPorMes(data, 2, i.id), 2);
                o.marzo = Math.Round(getPromedioPorMes(data, 3, i.id), 2);

                List<decimal> meses1 = new List<decimal>(new decimal[] { o.enero, o.febrero, o.marzo });
                if (meses1.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim1 = Math.Round((meses1.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim1 = 0;
                }

                o.abril = Math.Round(getPromedioPorMes(data, 4, i.id), 2);
                o.mayo = Math.Round(getPromedioPorMes(data, 5, i.id), 2);
                o.junio = Math.Round(getPromedioPorMes(data, 6, i.id), 2);

                List<decimal> meses2 = new List<decimal>(new decimal[] { o.abril, o.mayo, o.junio });
                if (meses2.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim2 = Math.Round((meses2.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim2 = 0;
                }

                o.julio = Math.Round(getPromedioPorMes(data, 7, i.id), 2);
                o.agosto = Math.Round(getPromedioPorMes(data, 8, i.id), 2);
                o.septiembre = Math.Round(getPromedioPorMes(data, 9, i.id), 2);

                List<decimal> meses3 = new List<decimal>(new decimal[] { o.julio, o.agosto, o.septiembre });
                if (meses3.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim3 = Math.Round((meses3.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim3 = 0;
                }

                o.octubre = Math.Round(getPromedioPorMes(data, 10, i.id), 2);
                o.noviembre = Math.Round(getPromedioPorMes(data, 11, i.id), 2);
                o.diciembre = Math.Round(getPromedioPorMes(data, 12, i.id), 2);

                List<decimal> meses4 = new List<decimal>(new decimal[] { o.octubre, o.noviembre, o.diciembre });
                if (meses4.Where(x => x != 0).ToList().Count > 0)
                {
                    o.trim4 = Math.Round((meses4.Where(x => x != 0).Average()), 2);
                }
                else
                {
                    o.trim4 = 0;
                }

                o.total = Math.Round((o.enero + o.febrero + o.marzo + o.abril + o.mayo + o.junio + o.julio + o.agosto + o.septiembre + o.octubre + o.noviembre + o.diciembre) / getPromediototal(o), 2);
                o.tipo = i.tipo;
                result.Add(o);
            }

            var t = new EncuestaResults2DTO();
            t.pregunta = "TOTAL:";
            t.enero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.enero) / temp.Count(), 2);
            t.febrero = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.febrero) / temp.Count(), 2);
            t.marzo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.marzo) / temp.Count(), 2);

            List<decimal> meses5 = new List<decimal>(new decimal[] { t.enero, t.febrero, t.marzo });
            if (meses5.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim1 = Math.Round((meses5.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim1 = 0;
            }

            t.abril = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.abril) / temp.Count(), 2);
            t.mayo = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.mayo) / temp.Count(), 2);
            t.junio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.junio) / temp.Count(), 2);

            List<decimal> meses6 = new List<decimal>(new decimal[] { t.abril, t.mayo, t.junio });
            if (meses6.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim2 = Math.Round((meses6.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim2 = 0;
            }

            t.julio = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.julio) / temp.Count(), 2);
            t.agosto = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.agosto) / temp.Count(), 2);
            t.septiembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.septiembre) / temp.Count(), 2);

            List<decimal> meses7 = new List<decimal>(new decimal[] { t.julio, t.agosto, t.septiembre });
            if (meses7.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim3 = Math.Round((meses7.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim3 = 0;
            }

            t.octubre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.octubre) / temp.Count(), 2);
            t.noviembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.noviembre) / temp.Count(), 2);
            t.diciembre = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.diciembre) / temp.Count(), 2);

            List<decimal> meses8 = new List<decimal>(new decimal[] { t.octubre, t.noviembre, t.diciembre });
            if (meses8.Where(x => x != 0).ToList().Count > 0)
            {
                t.trim4 = Math.Round((meses8.Where(x => x != 0).Average()), 2);
            }
            else
            {
                t.trim4 = 0;
            }

            t.total = temp.Count() == 0 ? 0 : Math.Round(result.Sum(x => x.total) / temp.Count(), 2);
            t.muestra = unicos.Count;
            t.tresestrellasCont = (threestars != null) ? threestars.Count : 0;
            t.tresestrellasPerc = threestarsPercentage.ToString("0.##");
            t.encuesta = encuesta;
            result.Add(t);
            return result;
        }

        public List<EncuestaDTO> getEncuestas()
        {
            List<EncuestaDTO> resultado = new List<EncuestaDTO>();
            try
            {
                resultado = _context.tblEN_Encuesta.ToList().Where(x => x.estatus == true && x.estatusAutoriza==2).Select(y => new EncuestaDTO
                {
                    id = y.id,
                    titulo = y.titulo,
                    descripcion = y.descripcion != null ? y.descripcion : "",
                    creadorID = y.creadorID,
                    creadorNombre = (y.creador.nombre != null ? y.creador.nombre : "") + " " + (y.creador.apellidoPaterno != null ? y.creador.apellidoPaterno : "") + " " + (y.creador.apellidoMaterno != null ? y.creador.apellidoMaterno : ""),
                    departamentoID = y.departamentoID,
                    departamento = _context.tblP_Departamento.ToList().Where(z => z.id == y.departamentoID).Select(w => w.descripcion).FirstOrDefault(),
                    fecha = y.fecha.ToShortDateString(),
                    tipo = y.tipo,
                    telefonica = y.telefonica != null ? y.telefonica : false,
                    notificacion = y.notificacion != null ? y.notificacion : false,
                    soloLectura = y.soloLectura,
                    papel = y.papel
                }).ToList();
            }
            catch (Exception)
            {

            }

            return resultado;
        }

        public void UpdateTelefonica(int encuestaID, bool telefonica)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var rowTemp = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.id == encuestaID).FirstOrDefault();

                    if (rowTemp != null)
                    {
                        rowTemp.telefonica = telefonica;
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public void UpdateNotificacion(int encuestaID, bool notificacion)
        {
            var rowTemp = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.id == encuestaID).FirstOrDefault();

            if (rowTemp != null)
            {
                rowTemp.notificacion = notificacion;

                _context.SaveChanges();
            }
        }

        public void UpdatePapel(int encuestaID, bool papel)
        {
            var rowTemp = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.id == encuestaID).FirstOrDefault();

            if (rowTemp != null)
            {
                rowTemp.papel = papel;

                _context.SaveChanges();
            }
        }

        public List<string> getEmpresas()
        {
            var data = _context.tblP_Usuario.Where(x => x.estatus == true && x.cliente == true && x.empresa != "" && x.empresa != null).Select(z => z.empresa).Distinct().ToList();

            return data;
        }

        public List<tblP_Usuario> getClientes(string empresa)
        {
            var data = _context.tblP_Usuario.Where(x => empresa.Equals("CONSTRUPLAN") ? ( x.estatus == true && x.cliente == false ) : (x.estatus == true && x.cliente == true && x.empresa == empresa)).ToList();

            return data;
        }

        public EncuestaDTO getEncuestaTelefonica(int id)
        {
            var result = new EncuestaDTO();

            var temp = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == id);
            result.id = temp.id;
            result.titulo = temp.titulo;
            result.descripcion = temp.descripcion;
            result.creadorID = temp.creadorID;
            result.creadorNombre = temp.creador.nombre + " " + temp.creador.apellidoPaterno + " " + temp.creador.apellidoMaterno;
            var listaPreguntas = new List<PreguntaDTO>();
            foreach (var i in temp.preguntas.Where(x => x.visible == true))
            {
                var p = new PreguntaDTO();
                p.id = i.id;
                p.encuestaID = i.encuestaID;
                p.pregunta = i.pregunta;
                p.calificacion = 0;
                listaPreguntas.Add(p);
            }
            result.preguntas = listaPreguntas;
            return result;
        }

        public tblP_Usuario nuevoCliente(UsuarioDTO nuevo)
        {
            string correo = nuevo.correo.Split('@')[0];

            tblP_Usuario newCli = new tblP_Usuario();

            newCli.nombre = nuevo.nombre;
            newCli.apellidoPaterno = nuevo.apellidoPaterno;
            newCli.apellidoMaterno = nuevo.apellidoMaterno;
            newCli.correo = nuevo.correo;
            newCli.empresa = nuevo.empresa;
            newCli.nombreUsuario = correo;
            newCli.contrasena = "Proyecto" + DateTime.Now.Year;
            newCli.estatus = false;
            newCli.perfilID = 2;
            newCli.puestoID = 2;
            newCli.enviar = false;
            newCli.cliente = true;
            newCli.tipoSGC = false;
            newCli.usuarioSGC = "visitante";
            newCli.usuarioAuditor = false;

            var us = usuarioFactoryServices.getUsuarioService().SaveUsuario(newCli);

            return us;
        }

        public MemoryStream exportData(List<int> listaEncuestas, DateTime fechaInicio, DateTime fechaFinal)
        {

            var listaEncuestasRaw = _context.tblEN_Encuesta_Usuario.Where(x => listaEncuestas.Contains(x.encuestaID)).ToList().Where(e => e.fecha.Date >= fechaInicio.Date && e.fecha.Date <= fechaFinal.Date).ToList();

            var listaEncuesta = listaEncuestasRaw.GroupBy(x => new { x.encuestaID }).Select(x => x.Key).Distinct().ToList();
            var objMesesEncuestas = listaEncuestasRaw.Select(x => new { anio = x.fecha.Year, mes = x.fecha.Month }).Distinct().ToList();


            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Layaout");

                int indiceX = 1;
                int indiceY = 2;
                int indiceY2 = 1;

                worksheet.Cells[indiceX, 1].Value = "Encuesta: ";

                foreach (var mesAnio in objMesesEncuestas.OrderBy(a => a.anio).ThenBy(m => m.mes))
                {
                    string nombreColumna = MonthName(mesAnio.mes) + " " + mesAnio.anio;
                    worksheet.Cells[indiceX, indiceY].Value = nombreColumna;
                    indiceY++;

                }

                indiceX++;

                foreach (var encuesta in listaEncuesta)
                {
                    int indiceY3 = 2;
                    var encuestaRaw = _context.tblEN_Encuesta.FirstOrDefault(x => x.id == encuesta.encuestaID);

                    if (string.IsNullOrEmpty(encuestaRaw.descripcion))
                    {
                        worksheet.Cells[indiceX, indiceY2].Value = encuestaRaw.titulo;
                    }
                    else
                    {
                        worksheet.Cells[indiceX, indiceY2].Value = encuestaRaw.descripcion;
                    }


                    foreach (var mesAnio in objMesesEncuestas.OrderBy(a => a.anio).ThenBy(m => m.mes))
                    {

                        var resultData = listaEncuestasRaw.Where(x => x.encuestaID == encuesta.encuestaID && x.fecha.Month == mesAnio.mes && x.fecha.Year == mesAnio.anio);//.Select(x => new { anio = x.fecha.Year, mes = x.fecha.Month }).Distinct().ToList();

                        if (resultData.Count() > 0)
                        {
                            int cantidadUsuarios = resultData.Select(x => x.usuarioResponderID).ToList().Distinct().Count();
                            int cantidadEncuestas = resultData.Where(x => x.estatus == false).Count();

                            worksheet.Cells[indiceX, indiceY3].Value = (cantidadEncuestas / cantidadUsuarios) * 100 + " %";

                        }
                        else
                        {
                            worksheet.Cells[indiceX, indiceY3].Value = "0%";
                        }

                        indiceY3++;
                    }

                    indiceX++;

                }
                worksheet.View.FreezePanes(1, 1);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.Compression = CompressionLevel.BestSpeed;

                using (var exportData = new MemoryStream())
                {
                    package.SaveAs(exportData);
                    //lista.Add(exportData.ToArray());

                    return exportData;
                }

            }
        }

        public tblEN_Encuesta_Usuario nuevoEncuestaUsuario(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID)
        {
            var newEncUsu = new tblEN_Encuesta_Usuario();

            newEncUsu.encuestaID = encuestaID;
            newEncUsu.usuarioResponderID = usuarioResponderID;
            newEncUsu.fecha = DateTime.Now;
            newEncUsu.estatus = true;
            newEncUsu.asunto = asunto;
            newEncUsu.telefonica = true;
            newEncUsu.usuarioTelefonoID = usuarioTelefonoID;
            newEncUsu.rutaArchivo = "";
            newEncUsu.tipoRespuesta = (int)TipoRespuestaEncuestaEnum.TELEFONO;



            _context.tblEN_Encuesta_Usuario.Add(newEncUsu);
            _context.SaveChanges();

            return newEncUsu;
        }

        public tblEN_Encuesta_Usuario nuevoEncuestaUsuarioPapel(int encuestaID, int usuarioResponderID, string asunto, int usuarioTelefonoID, string rutaArchivo)
        {
            var newEncUsu = new tblEN_Encuesta_Usuario();

            newEncUsu.encuestaID = encuestaID;
            newEncUsu.usuarioResponderID = usuarioResponderID;
            newEncUsu.fecha = DateTime.Now;
            newEncUsu.estatus = true;
            newEncUsu.asunto = asunto;
            newEncUsu.telefonica = false;
            newEncUsu.usuarioTelefonoID = usuarioTelefonoID;
            newEncUsu.rutaArchivo = "";
            newEncUsu.tipoRespuesta = (int)TipoRespuestaEncuestaEnum.PAPEL;
            newEncUsu.rutaArchivo = rutaArchivo;

            _context.tblEN_Encuesta_Usuario.Add(newEncUsu);
            _context.SaveChanges();

            return newEncUsu;
        }

        public List<EncuestaCheckUsuarioDTO> getUsuariosCheck(int encuestaID)
        {
            var usuariosID = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus == true && x.encuestaID == encuestaID).Select(y => y.usuarioID).ToList();

            var data = _context.tblP_Usuario.Where(x => usuariosID.Contains(x.id)).Select(y => new EncuestaCheckUsuarioDTO
            {
                id = y.id,
                nombre = y.nombre,
                apellidoPaterno = y.apellidoPaterno ?? "",
                apellidoMaterno = y.apellidoMaterno ?? "",
                correo = y.correo,
                puestoID = y.puestoID,
                puestoDescripcion = y.puesto.descripcion,
                contestaTelefonica = _context.tblEN_Encuesta_Check_Usuario.Where(z => z.estatus == true && z.encuestaID == encuestaID && z.usuarioID == y.id).Select(w => w.contestaTelefonica).FirstOrDefault(),
                recibeNotificacion = _context.tblEN_Encuesta_Check_Usuario.Where(z => z.estatus == true && z.encuestaID == encuestaID && z.usuarioID == y.id).Select(w => w.recibeNotificacion).FirstOrDefault()
            }).ToList();

            return data;
        }

        public UsuarioDTO checkUsuario(string nombre)
        {
            var nombreFiltrado = nombre.Replace(" ", "");
            var data = _context.tblP_Usuario.Where(x => x.estatus == true &&
                (x.nombre.Replace(" ", "") + x.apellidoPaterno.Replace(" ", "") + x.apellidoMaterno.Replace(" ", "")) == nombreFiltrado).Select(y => new UsuarioDTO
                {
                    id = y.id,
                    nombre = y.nombre,
                    apellidoPaterno = y.apellidoPaterno,
                    apellidoMaterno = y.apellidoMaterno,
                    correo = y.correo,
                    puestoID = y.puestoID,
                    puestoDescripcion = y.puesto.descripcion
                }).FirstOrDefault();

            return data;
        }

        public void GuardarUsuariosCheck(int encuestaID, List<EncuestaCheckUsuarioDTO> usuarios)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var infoActualUsuarios = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.encuestaID == encuestaID);

                    var usuariosNuevos = new List<tblEN_Encuesta_Check_Usuario>();

                    foreach (var infoActual in infoActualUsuarios.Where(w => w.estatus))
                    {
                        var infoNueva = usuarios.FirstOrDefault(f => f.id == infoActual.usuarioID);

                        if (infoNueva != null)
                        {
                            infoActual.contestaTelefonica = infoNueva.contestaTelefonica;
                            infoActual.recibeNotificacion = infoNueva.recibeNotificacion;

                            usuarios.Remove(infoNueva);
                        }
                        else
                        {
                            infoActual.estatus = false;
                        }
                    }

                    foreach (var usuario in usuarios)
                    {
                        var infoActual = infoActualUsuarios.FirstOrDefault(f => f.usuarioID == usuario.id && !f.estatus);

                        if (infoActual != null)
                        {
                            infoActual.estatus = true;
                            infoActual.ver = null;
                            infoActual.editar = null;
                            infoActual.enviar = null;
                            infoActual.contestaPapel = null;
                            infoActual.contestaTelefonica = usuario.contestaTelefonica;
                            infoActual.recibeNotificacion = usuario.recibeNotificacion;
                        }
                        else
                        {
                            var usuarioNuevo = new tblEN_Encuesta_Check_Usuario();
                            usuarioNuevo.encuestaID = encuestaID;
                            usuarioNuevo.usuarioID = usuario.id;
                            usuarioNuevo.contestaTelefonica = usuario.contestaTelefonica;
                            usuarioNuevo.recibeNotificacion = usuario.recibeNotificacion;
                            usuarioNuevo.estatus = true;

                            usuariosNuevos.Add(usuarioNuevo);
                        }
                    }

                    _context.tblEN_Encuesta_Check_Usuario.AddRange(usuariosNuevos);
                    SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public bool checkEncuestaTelefonica(int encuestaID)
        {
            var data = _context.tblEN_Encuesta.Where(x => x.id == encuestaID).Select(y => y.telefonica).FirstOrDefault();
            bool respuesta;
            if (data != null)
            {
                respuesta = (bool)data;
            }
            else
            {
                respuesta = false;
            }
            return respuesta;
        }

        public EncuestaDTO getEncuestaCheck(int encuestaID)
        {
            var data = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.estatusAutoriza == 2 && x.id == encuestaID).Select(y => new EncuestaDTO
            {
                id = y.id,
                titulo = y.titulo,
                descripcion = y.descripcion,
                telefonica = y.telefonica != null ? y.telefonica : false,
                notificacion = y.notificacion != null ? y.notificacion : false
            }).FirstOrDefault();

            return data;
        }

        public bool checkTelefonica(int encuestaID)
        {
            var encuesta = _context.tblEN_Encuesta.Where(x => x.estatus == true && x.estatusAutoriza == 2 && x.id == encuestaID).FirstOrDefault();
            var checkUsuario = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus == true && x.encuestaID == encuestaID && x.usuarioID == vSesiones.sesionUsuarioDTO.id && x.contestaTelefonica == true).FirstOrDefault();

            var flag = false;

            if (encuesta.telefonica == true && checkUsuario != null)
            {
                flag = true;
            }

            return flag;
        }

        public List<tblP_Usuario> getUsuarios(string term)
        {
            var data = _context.tblP_Usuario.Where(x => x.estatus == true && ((x.nombre + x.apellidoPaterno + x.apellidoMaterno).Replace(" ", "").Contains(term.Replace(" ", "")))).ToList();

            return data;
        }

        public List<tblEN_Estrellas> getEstrellas()
        {
            return _context.tblEN_Estrellas.ToList();
        }
        public void setCrearEncuesta(int empID, int crearID, bool crear)
        {
            var data = _context.tblP_MenutblP_Usuario.FirstOrDefault(x=>x.tblP_Usuario_id==empID && x.tblP_Menu_id==crearID);
            if (crear)
            {
                if (data == null)
                {
                    var o = new tblP_MenutblP_Usuario();
                    o.tblP_Menu_id = crearID;
                    o.tblP_Usuario_id = empID;
                    o.sistema = (int)SistemasEnum.ENCUESTAS;
                    _context.tblP_MenutblP_Usuario.Add(o);
                    _context.SaveChanges();
                }
            }
            else { 
                if (data != null)
                {
                    _context.tblP_MenutblP_Usuario.Remove(data);
                    _context.SaveChanges();
                }
            }
        }
        public List<muestroEncuestaDTO> getEncuestaCerteza(List<int> lstEncuesta, DateTime fechaInicio, DateTime fechaFinal)
        {

            var listaEncuestasRaw = _context.tblEN_Encuesta_Usuario.Where(x => lstEncuesta.Contains(x.encuestaID)).ToList().Where(e => e.fecha.Date >= fechaInicio.Date && e.fecha.Date <= fechaFinal.Date).ToList();

            var listaEncuestas = listaEncuestasRaw.GroupBy(x => new { x.encuestaID }).Select(x => x.Key).Distinct().ToList();

            List<muestroEncuestaDTO> LsobjReturn = new List<muestroEncuestaDTO>();


            foreach (var item in listaEncuestas)
            {
                var encuesta = _context.tblEN_Encuesta.FirstOrDefault(e => e.id == item.encuestaID);

                muestroEncuestaDTO objReturn = new muestroEncuestaDTO();

                var objEncuestas = listaEncuestasRaw.Where(x => x.encuestaID == item.encuestaID && x.estatus == false).ToList();

                var objMesesEncuestas = objEncuestas.Select(x => new { anio = x.fecha.Year, mes = x.fecha.Month }).Distinct();
                var CantidadEncuestas = objEncuestas.Count();
                var objEncuestasUsuarios = listaEncuestasRaw.Where(x => x.encuestaID == item.encuestaID).GroupBy(x => x.usuarioResponderID).Select(x => x.Key).Distinct().ToList();

                var CantidadUsuario = objEncuestasUsuarios.Count();

                objReturn.encuesta = encuesta.descripcion;
                objReturn.mesColumna = new List<string>();
                objReturn.porcetajeData = new List<string>();

                foreach (var mesAnio in objMesesEncuestas.OrderBy(a => a.anio).ThenBy(m => m.mes))
                {

                    decimal porcentaje = 0;
                    var objRawXMes = objEncuestas.Where(x => x.fecha.Month == mesAnio.mes && x.fecha.Year == mesAnio.anio);
                    var rawCantidadUsuarios = objRawXMes.GroupBy(x => x.usuarioResponderID).Select(x => x.Key).Distinct().ToList();
                    decimal cantidadUsuarioEnc = rawCantidadUsuarios.Count();

                    if (objRawXMes.Count() > 0)
                    {
                        if (cantidadUsuarioEnc > 0)
                        {
                            decimal cantidadEncuestas = objEncuestas.Count();

                            porcentaje = Math.Round((cantidadUsuarioEnc / cantidadEncuestas) * 100, 2);
                        }
                    }

                    string nombreColumna = MonthName(mesAnio.mes) + " " + mesAnio.anio;

                    objReturn.mesColumna.Add(nombreColumna);
                    objReturn.porcetajeData.Add(porcentaje.ToString());
                }



                LsobjReturn.Add(objReturn);
            }

            return LsobjReturn;
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }
        public List<EncuestaCheckUsuarioDTO> getUsuarioPermisosCheck(int usuarioID)
        {
            //var accionesPorUsuario = 
            //    _context.tblP_AccionesVistatblP_Usuario.Where(x => 
            //        x.tblP_AccionesVista_id == 3 || 
            //        x.tblP_AccionesVista_id == 4 ||
            //        x.tblP_AccionesVista_id == 1036 ||
            //        x.tblP_AccionesVista_id == 5 ||
            //        x.tblP_AccionesVista_id == 7 ||
            //        x.tblP_AccionesVista_id == 1037 ||
            //        x.tblP_AccionesVista_id == 1046).ToList();
            //var usuarios = _context.tblP_Usuario.Where(x => x.estatus && !x.cliente && x.id != 13).ToList();
            //var encuestasPermisosCheck = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus).ToList();

            //foreach (var usu in usuarios)
            //{
            //    var encuestasPorDepartamento = getEncuestasByDeptoTodosLosUsuarios(usu.puesto.departamentoID, usu.id);

            //    foreach (var enc in encuestasPorDepartamento)
            //    {
            //        var existenteEncuestaPermisoCheck = encuestasPermisosCheck.FirstOrDefault(x => x.encuestaID == enc.id && x.usuarioID == usu.id && x.estatus);

            //        if (existenteEncuestaPermisoCheck == null)
            //        {
            //            var nuevoPermisoCheck = new tblEN_Encuesta_Check_Usuario();

            //            nuevoPermisoCheck.encuestaID = enc.id;
            //            nuevoPermisoCheck.usuarioID = usu.id;
            //            nuevoPermisoCheck.ver = true;
            //            nuevoPermisoCheck.editar = accionesPorUsuario.FirstOrDefault(x => x.tblP_Usuario_id == usu.id && x.tblP_AccionesVista_id == 3) != null ? true : false;
            //            nuevoPermisoCheck.enviar = accionesPorUsuario.FirstOrDefault(x => x.tblP_Usuario_id == usu.id && x.tblP_AccionesVista_id == 4) != null ? true : false;
            //            nuevoPermisoCheck.contestaTelefonica = accionesPorUsuario.FirstOrDefault(x => x.tblP_Usuario_id == usu.id && x.tblP_AccionesVista_id == 1036) != null ? true : false;
            //            nuevoPermisoCheck.recibeNotificacion = false;
            //            nuevoPermisoCheck.contestaPapel = false;
            //            nuevoPermisoCheck.estatus = true;

            //            _context.tblEN_Encuesta_Check_Usuario.Add(nuevoPermisoCheck);
            //            _context.SaveChanges();
            //        }
            //        else
            //        {
            //            existenteEncuestaPermisoCheck.ver = true;
            //            existenteEncuestaPermisoCheck.editar = accionesPorUsuario.FirstOrDefault(x => x.tblP_Usuario_id == usu.id && x.tblP_AccionesVista_id == 3) != null ? true : false;
            //            existenteEncuestaPermisoCheck.enviar = accionesPorUsuario.FirstOrDefault(x => x.tblP_Usuario_id == usu.id && x.tblP_AccionesVista_id == 4) != null ? true : false;

            //            _context.Entry(existenteEncuestaPermisoCheck).State = System.Data.Entity.EntityState.Modified;
            //            _context.SaveChanges();
            //        }
            //    }
            //}

            var permisos = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus == true && x.usuarioID == usuarioID).Select(y => new EncuestaCheckUsuarioDTO
            {
                id = y.usuarioID,
                encuestaID = y.encuestaID,
                consultar = y.ver,
                editar = y.editar,
                enviar = y.enviar,
                contestaTelefonica = y.contestaTelefonica,
                recibeNotificacion = y.recibeNotificacion,
                contestaPapel = y.contestaPapel,
                crear = y.crear
            }).ToList();

            return permisos;
        }
        public List<tblEN_Encuesta> getEncuestasByDeptoTodosLosUsuarios(int id, int usuarioID)
        {
            var isPermisoCompras = getViewActionTodosLosUsuarios("ComprasYRenta", usuarioID);
            var isPermisoSGC = getViewActionTodosLosUsuarios("Administrar", usuarioID);
            var isPermisoComercializacion = getViewActionTodosLosUsuarios("EncuestaComercialización", usuarioID);
            var isPermisoCDI = getViewActionTodosLosUsuarios("EsCDI", usuarioID);

            var result = _context.tblEN_Encuesta.Where(x =>
                (
                    (
                        isPermisoCompras ? (x.departamentoID == id || x.id == 22 || x.id == 17) :
                        isPermisoComercializacion ? (x.departamentoID == id || (x.departamentoID == 5 && x.estatus == true)) :
                        (
                            isPermisoSGC ? x.departamentoID == id :
                            isPermisoCDI ? (x.id == 35 || x.id == 55 || x.id == 56 || x.id == 64 || x.departamentoID == id) : (x.departamentoID == id && x.id != 8)
                        )
                    )
                ) && (x.estatus == true && x.estatusAutoriza == (int)estatusEnum.AUTORIZADO)).ToList();

            foreach (var i in result)
            {
                i.departamento = _context.tblP_Departamento.FirstOrDefault(x => x.id.Equals(i.departamentoID));
            }

            return result;
        }
        public bool getViewActionTodosLosUsuarios(string accion, int usuarioID)
        {
            var result = (
                    from a in _context.tblP_AccionesVista
                    join b in _context.tblP_AccionesVistatblP_Usuario on a.id equals b.tblP_AccionesVista_id
                    where (a.vistaID == 2079 && a.Accion.Equals(accion) && b.tblP_Usuario_id == usuarioID)
                    select a
                ).ToList();

            if (result.Count > 0)
                return true;
            else
            {
                return false;
            }
        }

        public void guardarPermisosCheck(List<EncuestaCheckUsuarioDTO> lstPermisos)
        {
            var usuarioID = lstPermisos.FirstOrDefault().usuarioID;
            var permisosExistentes = _context.tblEN_Encuesta_Check_Usuario.Where(x => x.estatus && x.usuarioID == usuarioID).ToList();

            foreach (var per in lstPermisos)
            {
                var permisoExistentePorEncuesta = permisosExistentes.FirstOrDefault(x => x.encuestaID == per.encuestaID);

                if (permisoExistentePorEncuesta == null)
                {
                    if (per.editar == true || per.enviar == true || per.contestaTelefonica == true || per.recibeNotificacion == true || per.crear == true || per.consultar == true)
                    {
                        var nuevoPermiso = new tblEN_Encuesta_Check_Usuario();

                        nuevoPermiso.encuestaID = per.encuestaID;
                        nuevoPermiso.usuarioID = per.usuarioID;
                        nuevoPermiso.ver = per.consultar;
                        nuevoPermiso.editar = per.editar;
                        nuevoPermiso.enviar = per.enviar;

                        nuevoPermiso.contestaTelefonica = per.contestaTelefonica;
                        nuevoPermiso.recibeNotificacion = per.recibeNotificacion;
                        nuevoPermiso.contestaPapel = per.contestaPapel;
                        nuevoPermiso.crear = per.crear;
                        nuevoPermiso.estatus = true;

                        _context.tblEN_Encuesta_Check_Usuario.Add(nuevoPermiso);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    permisoExistentePorEncuesta.ver = per.consultar;
                    permisoExistentePorEncuesta.editar = per.editar;
                    permisoExistentePorEncuesta.enviar = per.enviar;
                    permisoExistentePorEncuesta.contestaTelefonica = per.contestaTelefonica;
                    permisoExistentePorEncuesta.recibeNotificacion = per.recibeNotificacion;
                    permisoExistentePorEncuesta.contestaPapel = per.contestaPapel;
                    permisoExistentePorEncuesta.crear = per.crear;

                    _context.Entry(permisoExistentePorEncuesta).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }
        public EncuestaCheckUsuarioDTO getUsuarioPermisosCheckPorEncuesta(int usuarioID, int encuestaID)
        {
            var permisos = _context.tblEN_Encuesta_Check_Usuario.FirstOrDefault(x => x.estatus == true && x.encuestaID == encuestaID && x.usuarioID == usuarioID && x.Encuesta.estatus);

            if (permisos != null)
            {
                return new EncuestaCheckUsuarioDTO
                {
                    id = permisos.usuarioID,
                    encuestaID = permisos.encuestaID,
                    consultar = permisos.ver,
                    editar = permisos.editar,
                    enviar = permisos.enviar,
                    contestaTelefonica = permisos.Encuesta.telefonica != null && permisos.Encuesta.telefonica.Value ? permisos.contestaTelefonica : false,
                    recibeNotificacion = permisos.Encuesta.notificacion != null && permisos.Encuesta.notificacion.Value ? permisos.recibeNotificacion : false,
                    contestaPapel = permisos.Encuesta.papel != null && permisos.Encuesta.papel.Value ? permisos.contestaPapel : false,
                    crear = permisos.crear
                };
            }
            else
            {
                return null;
            }
        }

        public List<EncuestaCheckUsuarioDTO> getRptPermisosEncuestas(List<int> listaEncuestas, List<int> usuarios, List<int> Departamentos)
        {
            List<int> newLista = new List<int>();
            var encuestasTemp = _context.tblEN_Encuesta.Where(x => x.estatus == true).Select(x=>x.id).ToList();
            var usuariossTemp = _context.tblP_Usuario.Where(x => x.estatus == true).Select(x => x.id).ToList();
            listaEncuestas = (listaEncuestas == null ? newLista : listaEncuestas);
            usuarios = (usuarios == null ? newLista : usuarios);


            var Result = from e in _context.tblEN_Encuesta
                         join euu in _context.tblEN_Encuesta_Check_Usuario
                         on e.id equals euu.encuestaID
                         join u in _context.tblP_Usuario
                         on euu.usuarioID equals u.id
                         where (listaEncuestas.Count > 0 ? listaEncuestas.Contains(e.id) : encuestasTemp.Contains(e.id)) &&
                               (usuarios.Count > 0 ? usuarios.Contains(u.id) : usuariossTemp.Contains(u.id)) &&
                               (Departamentos.Count > 0 ? Departamentos.Contains(e.departamentoID) : true)
                               && (e.estatus==true && euu.estatus==true && u.estatus==true)
                         select new EncuestaCheckUsuarioDTO
                         {

                             nombreUsuario = u.nombre + " " + u.apellidoPaterno + " " + u.apellidoMaterno,
                             encuestaID = e.id,
                             usuarioID = u.id,
                             titulo = e.titulo ?? "",
                             descripcion = e.descripcion ?? "",
                             departamentoID = e.departamentoID,
                             departamentoDesc = e.departamento==null?"":e.departamento.descripcion,
                             soloLectura = e.soloLectura,
                             encuestaEditar = true,
                             encuestaEnviar = true,
                             encuestaTelefonica = e.telefonica,
                             encuestaNotificacion = e.notificacion,
                             encuestaPapel = e.papel,

                             consultar = euu.ver,
                             editar = euu.editar,
                             enviar = euu.enviar,
                             contestaTelefonica = euu.contestaTelefonica,
                             recibeNotificacion = euu.recibeNotificacion,
                             contestaPapel = euu.contestaPapel,
                             crear = euu.crear
                         };

            return Result.ToList();
        }

        public tblEN_Encuesta_Usuario getEncuestaClienteByID(int id)
        {
            var data = _context.tblEN_Encuesta_Usuario.FirstOrDefault(x=>x.id == id);

            return data;
        }
    }
}
