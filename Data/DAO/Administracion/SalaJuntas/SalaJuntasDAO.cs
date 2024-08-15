using Core.DAO.Administracion.SalaJuntas;
using Data.DAO.Administracion.SalaJuntas;
using Core.Entity.Administrativo.SalaJuntas;
using Core.Enum.Administracion;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Factory.Administracion.SalaJuntas;
using Core.DTO;
using Infrastructure.DTO;
using Core.Enum.Principal;

namespace Data.DAO.Administracion.SalaJuntas
{
    public class SalaJuntasDAO : GenericDAO<tblP_Salas>, ISalaJuntasDAO
    {
        #region VARIABLES GLOBALES
        private Dictionary<string, object> resultado = new Dictionary<string, object>();
        private const string _NOMBRE_CONTROLADOR = "TabuladoresController";
        private const int _SISTEMA = (int)SistemasEnum.OTROS_SERVICIOS;
        #endregion

        #region CALENDARIO
        
        #endregion
    }
}
