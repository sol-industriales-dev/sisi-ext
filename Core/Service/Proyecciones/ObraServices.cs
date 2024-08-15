using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class ObraServices : IObraDAO
    {
        #region Atributos
        private IObraDAO m_ObraDAO;
        #endregion Atributos

        #region Propiedades
        private IObraDAO ObraDAO
        {
            get { return m_ObraDAO; }
            set { m_ObraDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public ObraServices(IObraDAO obraDAO)
        {
            this.ObraDAO = obraDAO;
        }
        #endregion Constructores

        public List<tblPro_Obras> getObras(int tipo)
        {
            return ObraDAO.getObras(tipo);
        }
        public void GuardarRegistros(tblPro_Obras obj)
        {

            ObraDAO.GuardarRegistros(obj);
        }

        public void GuardarActualizarRegistroMensual(List<tblPro_Obras> obj)
        {
            ObraDAO.GuardarActualizarRegistroMensual(obj);
        }
    }
}
