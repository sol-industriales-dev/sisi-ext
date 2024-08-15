using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte.CuadroComparativo
{
    public class CCEquipoServices : ICCEquipoDAO
    {
        #region Atributos
        private ICCEquipoDAO m_EquipoDAO;
        #endregion
        #region Propiedades
        public ICCEquipoDAO EquipoDAO
        {
            get { return m_EquipoDAO; }
            set { m_EquipoDAO = value; }
        }
        #endregion
        #region Constructor
        public CCEquipoServices(ICCEquipoDAO EquipoDAO)
        {
            this.EquipoDAO = EquipoDAO;
        }
        #endregion
        #region AsignacionNoEconomico
        public List<tblM_CCE_EncEquipo> GetCuadroEquipo(List<int> lstIdAsignacion)
        {
            return EquipoDAO.GetCuadroEquipo(lstIdAsignacion);
        }
        #endregion
        #region _formCCEquipo
        public List<tblM_CCE_CatConcepto> LstCatalogoActivo()
        {
            return EquipoDAO.LstCatalogoActivo();
        }
        public tblM_CCE_EncEquipo GetCuadroEquipo(int idAsignacion)
        {
            return EquipoDAO.GetCuadroEquipo(idAsignacion);
        }
        #endregion
    }
}
