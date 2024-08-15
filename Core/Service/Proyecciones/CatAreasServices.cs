using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Proyecciones
{
    public class CatAreasServices : IcatAreasDAO
    {
                        #region Atributos
        private IcatAreasDAO m_catAreasDAO;
        #endregion Atributos

        #region Propiedades
        private IcatAreasDAO CatAreasDAO
        {
            get { return m_catAreasDAO; }
            set { m_catAreasDAO = value; }
        }
        #endregion Propiedades

        #region Constructores
        public CatAreasServices(IcatAreasDAO catAreasDAO)
        {
            this.CatAreasDAO = catAreasDAO;
        }
        #endregion Constructores

        public void Guardar(tblPro_CatAreas obj)
        {
            CatAreasDAO.Guardar(obj);
        }

        public List<tblPro_CatAreas> FillCboArea()
        {
            return CatAreasDAO.FillCboArea();
        }

        public string getAreaByID(int obj)
        {
            return CatAreasDAO.getAreaByID(obj);
        }
    }
}
