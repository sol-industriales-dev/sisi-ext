using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO.Principal.Generales;
using System.IO;
using Core.Entity.Administrativo.ReservacionVehiculo;

namespace Core.DAO.Administracion.ReservacionVehiculo
{
    public interface IReservacionVehiculoDAO
    {
        Dictionary<string, object> guardarSolicitud(tblRV_Solicitudes solicitud);
        Dictionary<string, object> autorizarSolicitud(int solicitud_id);
        Dictionary<string, object> eliminarSolicitud(int solicitud_id);
        Dictionary<string, object> getSolicitudes();
        Dictionary<string, object> getSolicitudFecha(DateTime fechaSalida, DateTime fechaEntrega);
        Dictionary<string, object> enviarCorreoSolicitud(int id);
        Dictionary<string, object> enviarCorreoReemplazo(int solicitudAnterior_id, int solicitudNueva_id);
        Dictionary<string, object> enviarCorreoAutorizacion(int solicitudAutorizada_id);
        Dictionary<string, object> enviarCorreoRechazo(int solicitudRechazada_id);
        Dictionary<string, object> enviarCorreoCancelacion(int solicitudCancelada_id);
    }
}
