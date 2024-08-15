using Core.DAO.Administracion.ReservacionVehiculo;
using Core.Entity.Administrativo.ReservacionVehiculo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Administracion.ReservacionVehiculo
{
    public class ReservacionVehiculoService : IReservacionVehiculoDAO
    {
       private IReservacionVehiculoDAO m_reservacionVehiculoDAO;
       public IReservacionVehiculoDAO ReservacionVehiculoDAO
        {
            get { return m_reservacionVehiculoDAO; }
            set { m_reservacionVehiculoDAO = value; }
        }
       public ReservacionVehiculoService(IReservacionVehiculoDAO reservacionVehiculo)
        {
            this.ReservacionVehiculoDAO = reservacionVehiculo;
        }

       public Dictionary<string, object> guardarSolicitud(tblRV_Solicitudes solicitud)
       {
           return ReservacionVehiculoDAO.guardarSolicitud(solicitud);
       }
       public Dictionary<string, object> autorizarSolicitud(int solicitud_id)
       {
           return ReservacionVehiculoDAO.autorizarSolicitud(solicitud_id);
       }
       public Dictionary<string, object> eliminarSolicitud(int solicitud_id)
       {
           return ReservacionVehiculoDAO.eliminarSolicitud(solicitud_id);
       }
       public Dictionary<string, object> getSolicitudes()
       {
           return ReservacionVehiculoDAO.getSolicitudes();
       }
       public Dictionary<string, object> getSolicitudFecha(DateTime fechaSalida, DateTime fechaEntrega)
       {
           return ReservacionVehiculoDAO.getSolicitudFecha(fechaSalida, fechaEntrega);
       }
       public Dictionary<string, object> enviarCorreoSolicitud(int id)
       {
           return ReservacionVehiculoDAO.enviarCorreoSolicitud(id);
       }
       public Dictionary<string, object> enviarCorreoReemplazo(int solicitudAnterior_id, int solicitudNueva_id)
       {
           return ReservacionVehiculoDAO.enviarCorreoReemplazo(solicitudAnterior_id, solicitudNueva_id);
       }
       public Dictionary<string, object> enviarCorreoAutorizacion(int solicitudAutorizada_id)
       {
           return ReservacionVehiculoDAO.enviarCorreoAutorizacion(solicitudAutorizada_id);
       }
       public Dictionary<string, object> enviarCorreoRechazo(int solicitudRechazada_id)
       {
           return ReservacionVehiculoDAO.enviarCorreoRechazo(solicitudRechazada_id);
       }
       public Dictionary<string, object> enviarCorreoCancelacion(int solicitudCancelada_id)
       {
           return ReservacionVehiculoDAO.enviarCorreoCancelacion(solicitudCancelada_id);
       }
    }
}
