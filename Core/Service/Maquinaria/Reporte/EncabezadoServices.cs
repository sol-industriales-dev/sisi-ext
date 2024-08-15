using Core.DAO.Maquinaria.Reporte;
using Core.Entity.Maquinaria.Reporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class EncabezadoServices : IEncabezadoDAO
    {
        #region Atributos
        private IEncabezadoDAO m_encabezadoDAO;
        #endregion
        #region Propiedades
        public IEncabezadoDAO EncabezadoDAO
        {
            get { return m_encabezadoDAO; }
            set { m_encabezadoDAO = value; }
        }
        #endregion
        #region Constructores
        public EncabezadoServices(IEncabezadoDAO EncabezadoDAO)
        {
            this.EncabezadoDAO = EncabezadoDAO;
        }
        #endregion

        public tblP_Encabezado getEncabezadoDatos()
        {
            return this.EncabezadoDAO.getEncabezadoDatos();
        }

        public tblP_Encabezado getEncabezadoDatosCplan()
        {
            return this.EncabezadoDAO.getEncabezadoDatosCplan();
        }
    }
}
