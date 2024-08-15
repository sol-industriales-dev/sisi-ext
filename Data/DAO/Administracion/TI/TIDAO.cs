using Core.DAO.Administracion.TI;
using Core.Entity.Principal.Usuarios;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal;

namespace Data.DAO.Administracion.TI
{
    public class TIDAO : GenericDAO<tblP_Usuario>, ITIDAO
    {
        #region INIT
        private const string _NOMBRE_CONTROLADOR = "TIController";
        private const int _SISTEMA = (int)SistemasEnum.TI;
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        #endregion
    }
}
