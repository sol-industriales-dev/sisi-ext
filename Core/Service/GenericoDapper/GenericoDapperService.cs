using Core.DAO.GenericoDapper;
using Core.DAO.GestorArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.GenericoDapper
{
    public class GenericoDapperService : IGenericoDapper
    {
        #region Atributos
        private IGenericoDapper m_Dapper { get; set; }
        #endregion
        #region Propiedades
        public IGenericoDapper Dapper
        {
            get { return m_Dapper; }
            set { m_Dapper = value; }
        }
        #endregion
        #region Contructores
        public GenericoDapperService(IGenericoDapper _Dapper)
        {
            Dapper = _Dapper;
        }
        #endregion

        public Dictionary<string, object> ActivarDesactivar(string tabla, bool ActDesc, int id)
        {
            return Dapper.ActivarDesactivar(tabla, ActDesc, id);
        }
        public Dictionary<string, object> obtenerConsultaEnkontrol()
        {
            return Dapper.obtenerConsultaEnkontrol();
        }
    }
}
