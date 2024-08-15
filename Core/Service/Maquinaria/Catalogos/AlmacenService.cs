using Core.DAO.Maquinaria.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Catalogos
{
    public class AlmacenService
    {
         #region Atributos
        private IAlmacenDAO m_almacenDAO;
        #endregion

        #region Propiedades
        public IAlmacenDAO AlmacenDAO
        {
            get { return m_almacenDAO; }
            set { m_almacenDAO = value; }
        }
        #endregion

        #region Constructores
        public AlmacenService(IAlmacenDAO AlmacenDAO)
        {
            this.AlmacenDAO = AlmacenDAO;
        }
        #endregion
    }
}
