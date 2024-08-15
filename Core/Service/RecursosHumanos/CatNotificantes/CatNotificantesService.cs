using Core.DAO.RecursosHumanos.CatNotificantes;
using Core.DTO.RecursosHumanos.CatNotificantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.RecursosHumanos.CatNotificantes
{
    public class CatNotificantesService : ICatNotificantesDAO
    {
        public ICatNotificantesDAO catNotificantesInterfaz { get; set; }

        public CatNotificantesService(ICatNotificantesDAO catNotificantesTemp)
        {
            this.catNotificantesInterfaz = catNotificantesTemp;
        }

        #region CAT NOTIFICANTES
        public Dictionary<string,object> GetNotificantes()
        {
            return catNotificantesInterfaz.GetNotificantes();
        }
        public Dictionary<string,object> GetNotificantesDet(string cc, int idConcepto)
        {
            return catNotificantesInterfaz.GetNotificantesDet(cc, idConcepto);
        }

        public Dictionary<string, object> CrearEditarNotificantes(string cc, int idConcepto, List<int> lstUsuariosNuevos)
        {
            return catNotificantesInterfaz.CrearEditarNotificantes(cc, idConcepto, lstUsuariosNuevos);
        }

        public Dictionary<string,object> RemoveNotificante(int idRelNoti)
        {
            return catNotificantesInterfaz.RemoveNotificante(idRelNoti);
        }
        #endregion

        #region FILLCOMBO
        public Dictionary<string,object> FillCboCC()
        {
            return catNotificantesInterfaz.FillCboCC();
        }

        public Dictionary<string, object> FillCboUsuarios()
        {
            return catNotificantesInterfaz.FillCboUsuarios();
        }

        public Dictionary<string, object> FillCboConceptos()
        {
            return catNotificantesInterfaz.FillCboConceptos();
        }
        #endregion
    }
}
