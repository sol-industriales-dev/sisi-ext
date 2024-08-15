using Core.DAO.Principal.Alertas;
using Core.Entity.Principal.Alertas;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.Alertas
{
    public class AlertasDAO : GenericDAO<tblP_Alerta>, IAlertasDAO
    {
        public void saveAlerta(tblP_Alerta obj)
        {
            SaveEntity(obj);
        }

        public void updateAlertaByModulo(int id, int moduloID)
        {

            var listaAlertas = _context.tblP_Alerta.Where(x => x.objID == id && x.moduloID == moduloID && x.visto == false).ToList();
            foreach (var alerta in listaAlertas)
            {

                alerta.visto = true;
                _context.SaveChanges();
            }

        }

        public tblP_Alerta getAlertaByID(int id)
        {
            return _context.tblP_Alerta.FirstOrDefault(x => x.id == id && !x.visto);
        }
        public List<tblP_Alerta> getAlertasByUsuario(int id)
        {
            return _context.tblP_Alerta.Where(x => x.userRecibeID == id && !x.visto).ToList();
        }
        public List<tblP_Alerta> getAlertasBySistema(int id)
        {
            return _context.tblP_Alerta.Where(x => x.sistemaID == id && !x.visto).ToList();
        }
        public List<tblP_Alerta> getAlertasByUsuarioAndSistema(int usuarioID, int sistemaID)
        {
            return _context.tblP_Alerta.Where(x => x.userRecibeID == usuarioID && x.sistemaID == sistemaID && !x.visto).ToList();
        }
        public tblP_Alerta getAlertaByEnviaAndObjec(int idRecibe, int idObject)
        {
            return _context.tblP_Alerta.FirstOrDefault(w => w.userRecibeID == idRecibe && w.objID == idObject);
        }
        public void updateAlerta(tblP_Alerta obj)
        {
            var alert = _context.tblP_Alerta.FirstOrDefault(x => x.id == obj.id);
            alert.visto = true;
            _context.SaveChanges();
        }

        public Dictionary<string, object> ColocarVistoAlerta(int alerta_id)
        {
            var resultado = new Dictionary<string, object>();

            try
            {
                var alertaSIGOPLAN = _context.tblP_Alerta.FirstOrDefault(x => x.id == alerta_id);

                if (alertaSIGOPLAN != null)
                {
                    alertaSIGOPLAN.visto = true;
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("No se encuentra el registro de la alerta en la base de datos.");
                }

                resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                LogError(0, 0, "AlertasController", "ColocarVistoAlerta", e, AccionEnum.ACTUALIZAR, alerta_id, alerta_id);
                resultado.Add(SUCCESS, false);
                resultado.Add(MESSAGE, e.Message);
            }

            return resultado;
        }
    }
}
