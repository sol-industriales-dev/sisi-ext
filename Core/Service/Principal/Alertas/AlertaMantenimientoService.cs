using Core.DAO.Principal.Alertas;
using System.Collections.Generic;

namespace Core.Service.Principal.Alertas
{
    public class AlertaMantenimientoService : IAlertaMantenimientoDAO
    {
        #region Propiedades
        public IAlertaMantenimientoDAO AlertaMantenimientoDAO { get; set; }
        #endregion Propiedades

        #region Constructores
        public AlertaMantenimientoService(IAlertaMantenimientoDAO alertaMantenimientoDAO)
        {
            this.AlertaMantenimientoDAO = alertaMantenimientoDAO;
        }
        #endregion Constructores

        public Dictionary<string, object> VerificarAlertaMantenimiento()
        {
            return this.AlertaMantenimientoDAO.VerificarAlertaMantenimiento();
        }
    }
}
