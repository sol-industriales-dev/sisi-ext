using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class RepComparativaTiposServices : IRepComparativaTiposDAO
    {
        #region Atributos
        private IRepComparativaTiposDAO m_repComparativaTiposDAO;
        #endregion
        #region Propiedades
        public IRepComparativaTiposDAO RepComparativaTipossDAO
        {
            get { return m_repComparativaTiposDAO; }
            set { m_repComparativaTiposDAO = value; }
        }
        #endregion
        #region Constructores
        public RepComparativaTiposServices(IRepComparativaTiposDAO repComparativaTiposDAO)
        {
            this.RepComparativaTipossDAO = repComparativaTiposDAO;
        }
        #endregion

        public IList<RepComparativaTiposDTO> getAmountbyType(RepGastosFiltrosDTO obj)
        {
            return this.RepComparativaTipossDAO.getAmountbyType(obj);
        }
        public IList<RepComparativaTiposDTO> getAmountbyGroup(RepGastosFiltrosDTO obj, string cc)
        {
            return this.RepComparativaTipossDAO.getAmountbyGroup(obj, cc);
        }
        public IList<RepGastosMaquinariaGrid> getGrupoInsumos(RepGastosFiltrosDTO obj)
        {
            return this.RepComparativaTipossDAO.getGrupoInsumos(obj);
        }

        public IList<area_cuentaDTO> getEconomicosXCentroCostos(string centroCostos)
        {
            return this.RepComparativaTipossDAO.getEconomicosXCentroCostos(centroCostos);
        }

        public IList<RepGastosMaquinariaGrid> getInsumos(RepGastosFiltrosDTO obj)
        {
            return this.RepComparativaTipossDAO.getInsumos(obj);
        }
        public IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaul(RepGastosFiltrosDTO obj)
        {
            return this.RepComparativaTipossDAO.getAmountbyTypeNoOverhaul(obj);
        }
        public IList<RepComparativaTiposDTO> getAmountbyTypeNoOverhaulByTipo(RepGastosFiltrosDTO obj, string cc)
        {
            return this.RepComparativaTipossDAO.getAmountbyTypeNoOverhaulByTipo(obj, cc);
        }


        public IList<pruebaDto> getDataPrueba()
        {
            return this.RepComparativaTipossDAO.getDataPrueba();
        }
        public double getTotalImporte(string obj)
        {
            return this.RepComparativaTipossDAO.getTotalImporte(obj);
        }
    }
}
