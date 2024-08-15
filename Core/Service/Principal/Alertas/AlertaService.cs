using Core.DAO.Principal.Alertas;
using Core.Entity.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Principal.Alertas
{
    public class AlertaService : IAlertasDAO
    {
        #region Atributos
        private IAlertasDAO m_alertasDAO;
        #endregion Atributos

        #region Propiedades
        private IAlertasDAO AlertasDAO
        {
            get { return m_alertasDAO; }
            set { m_alertasDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public AlertaService(IAlertasDAO AlertasDAO)
        {
            this.AlertasDAO = AlertasDAO;
        }
        #endregion Constructores

        public void saveAlerta(tblP_Alerta obj)
        {
            AlertasDAO.saveAlerta(obj);
        }

        public void updateAlertaByModulo(int id, int moduloID)
        {
            AlertasDAO.updateAlertaByModulo(id, moduloID);
        }
        public tblP_Alerta getAlertaByID(int id)
        {
            return AlertasDAO.getAlertaByID(id);
        }
        public List<tblP_Alerta> getAlertasByUsuario(int id)
        {
            return AlertasDAO.getAlertasByUsuario(id);
        }
        public List<tblP_Alerta> getAlertasBySistema(int id)
        {
            return AlertasDAO.getAlertasBySistema(id);
        }
        public List<tblP_Alerta> getAlertasByUsuarioAndSistema(int usuarioID, int sistemaID)
        {
            return AlertasDAO.getAlertasByUsuarioAndSistema(usuarioID, sistemaID);
        }
        public tblP_Alerta getAlertaByEnviaAndObjec(int idRecibe, int idObject)
        {
            return AlertasDAO.getAlertaByEnviaAndObjec(idRecibe, idObject);
        }
        public void updateAlerta(tblP_Alerta alert)
        {
          
            AlertasDAO.updateAlerta(alert);
        }

        public Dictionary<string, object> ColocarVistoAlerta(int alerta_id)
        {
            return AlertasDAO.ColocarVistoAlerta(alerta_id);
        }
    }
}
