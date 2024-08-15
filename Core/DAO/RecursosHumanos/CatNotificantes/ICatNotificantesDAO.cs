using Core.DTO.RecursosHumanos.CatNotificantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.CatNotificantes
{
    public interface ICatNotificantesDAO
    {
        #region CAT NOTIFICANTES
        Dictionary<string, object> GetNotificantes();
        Dictionary<string, object> GetNotificantesDet(string cc, int idConcepto);
        Dictionary<string, object> CrearEditarNotificantes(string cc, int idConcepto, List<int> lstUsuariosNuevos);
        Dictionary<string, object> RemoveNotificante(int idRelNoti);

        #endregion

        #region FILLCOMBO
        Dictionary<string,object> FillCboCC();
        Dictionary<string, object> FillCboUsuarios();
        Dictionary<string, object> FillCboConceptos();
        
        #endregion
    }
}
