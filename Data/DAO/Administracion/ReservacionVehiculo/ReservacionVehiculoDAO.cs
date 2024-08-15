using Core.DTO.Administracion.ReservacionVehiculo;
using Core.DAO.Administracion.ReservacionVehiculo;
using Core.Entity.Administrativo.ReservacionVehiculo;
using Infrastructure.DTO;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;
using Data.DAO.Principal.Usuarios;
using Core.DTO;
using System.Data.Entity.Infrastructure;
using Infrastructure.Utils;


namespace Data.DAO.Administracion.ReservacionVehiculo
{
    class ReservacionVehiculoDAO : GenericDAO<tblRV_Solicitudes>, IReservacionVehiculoDAO
    {
        #region variables
        // Variables a utilizar en los diccionarios de resultados.
        public readonly string SUCCESS = "success";
        public readonly string ERROR = "error";

        private readonly string NOMBRE_CONTROLADOR = "ReservacionVehiculoController";
        #endregion

        #region SOLICITUD
        public Dictionary<string, object> guardarSolicitud(tblRV_Solicitudes solicitud)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                solicitud.usuarioRegistroID = vSesiones.sesionUsuarioDTO.id;
                _context.tblRV_Solicitudes.Add(solicitud);
                _context.SaveChanges();

                if (solicitud.id > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("id", solicitud.id);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> autorizarSolicitud(int solicitud_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var solicitud = _context.tblRV_Solicitudes.Where(x => x.estatus == true && x.id == solicitud_id).FirstOrDefault();

                if (solicitud != null)
                {
                    solicitud.autorizada = true;

                    _context.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> eliminarSolicitud(int solicitud_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var solicitud = _context.tblRV_Solicitudes.Where(x => x.estatus == true && x.id == solicitud_id).FirstOrDefault();

                if (solicitud != null)
                {
                    solicitud.estatus = false;
                    solicitud.autorizada = false;

                    _context.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();

                    resultado.Add(SUCCESS, true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> getSolicitudFecha(DateTime fechaSalida, DateTime fechaEntrega)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var solicitudes = _context.tblRV_Solicitudes.ToList().ToList().Where(x => (x.estatus == true) && (fechaSalida >= x.fechaSalida && fechaSalida <= x.fechaEntrega) || (fechaEntrega >= x.fechaSalida && fechaEntrega <= x.fechaEntrega) || (x.fechaSalida >= fechaSalida && x.fechaSalida <= fechaEntrega))
                    .Select(x => new ReservacionVehiculoDTO
                {
                    id = x.id,
                    motivo = x.motivo,
                    justificacion = x.descripcion,
                    fechaSalida = x.fechaSalida.ToShortDateString() + " " + x.fechaSalida.ToShortTimeString(),
                    fechaEntrega = x.fechaEntrega.ToShortDateString() + " " + x.fechaEntrega.ToShortTimeString(),
                    solicitante = x.solicitante
                }).ToList();

                var ud = new UsuarioDAO();
                var puedeReemplezar = ud.getViewAction(vSesiones.sesionCurrentView, "ReemplazarSolicitud");


                if (solicitudes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("permiso", puedeReemplezar);
                    resultado.Add("items", solicitudes);
                }
                else
                {
                    resultado.Add(SUCCESS, false);
                    resultado.Add("permiso", puedeReemplezar);
                    resultado.Add("EMPTY", true);
                }
            }
            catch (Exception)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> getSolicitudes()
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var ud = new UsuarioDAO();
                var puedeReemplezar = ud.getViewAction(vSesiones.sesionCurrentView, "ReemplazarSolicitud");

                var solicitudes = _context.tblRV_Solicitudes.ToList().Where(x => x.estatus == true).Select(x => new ReservacionVehiculoDTO
                {
                    id = x.id,
                    motivo = x.motivo,
                    justificacion = x.descripcion,
                    fechaSalida = x.fechaSalida.ToShortDateString() + " " + x.fechaSalida.ToShortTimeString(),
                    fechaEntrega = x.fechaEntrega.ToShortDateString() + " " + x.fechaEntrega.ToShortTimeString(),
                    solicitante = x.solicitante,
                    estatus = x.estatus,
                    autorizada = x.autorizada,
                    tienePermiso = puedeReemplezar
                }).ToList();

                if (solicitudes.Count > 0)
                {
                    resultado.Add(SUCCESS, true);
                    resultado.Add("items", solicitudes);
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
                resultado.Add(ERROR, "Ocurrió un error interno al intentar obtener las solicitudes");
            }



            return resultado;
        }
        public Dictionary<string, object> enviarCorreoSolicitud(int id)
        {
            var resultado = new Dictionary<string, object>();

            if (vSesiones.sesionUsuarioDTO.id != 6185)// Carla Velasco (ENCARGADA)
            {
                try
                {
                    List<string> correo = new List<string>();

                    if (vSesiones.sesionUsuarioDTO.id != 6185) // Carla Velasco (ENCARGADA)
                    {
                        var solicitudCorreo = _context.tblRV_Solicitudes.Where(x => x.estatus == true && x.id == id).FirstOrDefault();
                        correo.Add("carla.velasco@construplan.com.mx");
                        correo.Add(solicitudCorreo.usuarioRegistro.correo);
                       // correo.Add("jesus.murua@construplan.com.mx");

                        var subject = "Nueva solicitud de reservación de vehiculo";

                        var body = @"Usuario:   " + solicitudCorreo.solicitante + @"<br/>
                                 Fecha salida:     " + solicitudCorreo.fechaSalida.ToShortDateString() + " " + solicitudCorreo.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitudCorreo.fechaEntrega.ToShortDateString() + " " + solicitudCorreo.fechaEntrega.ToShortTimeString() + "<br/>";
                        body += @"   Motivo: " + solicitudCorreo.motivo + @"<br/>
                                 Justificación:       " + solicitudCorreo.descripcion + @"<br/>";


                        var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                        resultado.Add(SUCCESS, true);
                    }        
                }
                catch (Exception ex)
                {
                    resultado.Add(SUCCESS, false);
                }
            }
            else
            {
                resultado.Add(SUCCESS, true);
            }

            return resultado;
        }
        public Dictionary<string, object> enviarCorreoReemplazo(int solicitudAnterior_id, int solicitudNueva_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<string> correo = new List<string>();

                var solicitudAnterior = _context.tblRV_Solicitudes.Where(x => x.estatus == false && x.id == solicitudAnterior_id).FirstOrDefault();
                var solicitudNueva = _context.tblRV_Solicitudes.Where(x => x.estatus == true && x.id == solicitudNueva_id).FirstOrDefault();
                var subject = "";
                var body = "";

                if (solicitudAnterior != null)
                {
                    correo.Add(solicitudAnterior.usuarioRegistro.correo);
                    correo.Add(solicitudNueva.usuarioRegistro.correo);
                    correo.Add("carla.velasco@construplan.com.mx"); // Carla Velasco (ENCARGADA)

                    subject = "Se ha reemplazado solicitud de reservación de vehiculo";

                    body = @"<b> SOLICITUD ANTERIOR </b> <br/>
                                 Usuario:   " + solicitudAnterior.solicitante + @"<br/>
                                 Fecha salida:     " + solicitudAnterior.fechaSalida.ToShortDateString() + " " + solicitudAnterior.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitudAnterior.fechaEntrega.ToShortDateString() + " " + solicitudAnterior.fechaEntrega.ToShortTimeString() + "<br/>";
                    body += @"Motivo: " + solicitudAnterior.motivo + @"<br/>
                                 Justificación:       " + solicitudAnterior.descripcion + @"<br/>-----------------<br/>";

                    body += @"<b> SOLICITUD ACTUAL </b> <br/>
                                 Usuario:   " + solicitudNueva.solicitante + @"<br/>
                                 Fecha salida:     " + solicitudNueva.fechaSalida.ToShortDateString() + " " + solicitudNueva.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitudNueva.fechaEntrega.ToShortDateString() + " " + solicitudNueva.fechaEntrega.ToShortTimeString() + "<br/>";
                    body += @"Motivo: " + solicitudNueva.motivo + @"<br/>
                                 Justificación:       " + solicitudNueva.descripcion + @"<br/>";
                }

                var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> enviarCorreoAutorizacion(int solicitudAutorizada_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<string> correo = new List<string>();

                var solicitud = _context.tblRV_Solicitudes.Where(x => x.estatus == true && x.autorizada == true && x.id == solicitudAutorizada_id).FirstOrDefault();

                if (solicitud.usuarioRegistroID != 6185)// Carla Velasco (ENCARGADA)
                {
                    var body = "";
                    correo.Add(solicitud.usuarioRegistro.correo);

                    var subject = "Se ha autorizado solicitud de reservación de vehiculo";

                    if (solicitud != null)
                    {
                        body = @"Usuario:   " + solicitud.solicitante + @"<br/>
                                 Fecha salida:     " + solicitud.fechaSalida.ToShortDateString() + " " + solicitud.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitud.fechaEntrega.ToShortDateString() + " " + solicitud.fechaEntrega.ToShortTimeString() + "<br/>";
                        body += @"Motivo: " + solicitud.motivo + @"<br/>
                                 Justificación:       " + solicitud.descripcion + @"<br/>";
                    }

                    var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                }
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> enviarCorreoRechazo(int solicitudRechazada_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<string> correo = new List<string>();

                var solicitud = _context.tblRV_Solicitudes.Where(x => x.estatus == false && x.id == solicitudRechazada_id).FirstOrDefault();

                if (solicitud.usuarioRegistroID != 6185) // Carla Velasco (ENCARGADA)
                {
                    var body = "";
                    var subject = "Se ha rechazado solicitud de reservación de vehiculo";

                    correo.Add(solicitud.usuarioRegistro.correo);

                    if (solicitud != null)
                    {

                        body = @"Usuario:   " + solicitud.solicitante + @"<br/>
                                 Fecha salida:     " + solicitud.fechaSalida.ToShortDateString() + " " + solicitud.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitud.fechaEntrega.ToShortDateString() + " " + solicitud.fechaEntrega.ToShortTimeString() + "<br/>";
                        body += @"Motivo: " + solicitud.motivo + @"<br/>
                                 Justificación:       " + solicitud.descripcion + @"<br/>";
                    }
                    var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        public Dictionary<string, object> enviarCorreoCancelacion(int solicitudCancelada_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                List<string> correo = new List<string>();
            
                var solicitud = _context.tblRV_Solicitudes.Where(x => x.estatus == false && x.id == solicitudCancelada_id).FirstOrDefault();

                if (solicitud != null)
                {
                    if (solicitud.usuarioRegistroID != 6185) // Carla Velasco (ENCARGADA)
                    {
                        var body = "";
                        var subject = "Se ha cancelado la solicitud de reservación de vehiculo";

                        correo.Add(solicitud.usuarioRegistro.correo);

                        body = @"Usuario:   " + solicitud.solicitante + @"<br/>
                                 Fecha salida:     " + solicitud.fechaSalida.ToShortDateString() + " " + solicitud.fechaSalida.ToShortTimeString() + @"<br/>
                                 Fecha entrega: " + solicitud.fechaEntrega.ToShortDateString() + " " + solicitud.fechaEntrega.ToShortTimeString() + "<br/>";
                        body += @"Motivo: " + solicitud.motivo + @"<br/>
                                 Justificación:       " + solicitud.descripcion + @"<br/>";

                        var r = GlobalUtils.sendEmail(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), subject), body, correo);
                    }
                }
     
                resultado.Add(SUCCESS, true);
            }
            catch (Exception ex)
            {
                resultado.Add(SUCCESS, false);
            }

            return resultado;
        }
        #endregion
    }
}
