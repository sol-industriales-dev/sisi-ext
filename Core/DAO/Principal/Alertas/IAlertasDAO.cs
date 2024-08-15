using Core.Entity.Principal.Alertas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Principal.Alertas
{
    public interface IAlertasDAO
    {
        void saveAlerta(tblP_Alerta obj);
        tblP_Alerta getAlertaByID(int id);
        List<tblP_Alerta> getAlertasByUsuario(int id);
        List<tblP_Alerta> getAlertasBySistema(int id);
        List<tblP_Alerta> getAlertasByUsuarioAndSistema(int usuarioID,int sistemaID);
        tblP_Alerta getAlertaByEnviaAndObjec(int idRecibe, int idObject);
        void updateAlerta(tblP_Alerta obj);
        void updateAlertaByModulo(int id, int moduloID);
        Dictionary<string, object> ColocarVistoAlerta(int alerta_id);
    }
}
