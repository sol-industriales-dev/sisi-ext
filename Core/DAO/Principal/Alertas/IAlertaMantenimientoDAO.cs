using System.Collections.Generic;

namespace Core.DAO.Principal.Alertas
{
    public interface IAlertaMantenimientoDAO
    {
        /// <summary>
        /// Verfica si hay activa una alerta de mantenimiento de SIGOPLAN.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, object> VerificarAlertaMantenimiento();
    }
}
