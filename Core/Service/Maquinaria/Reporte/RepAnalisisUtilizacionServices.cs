using Core.DAO.Maquinaria.Reporte;
using Core.DTO.Maquinaria.Reporte.RepAnalisisUtilizacion;
using Core.DTO.Principal.Generales;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Maquinaria.Reporte
{
    public class RepAnalisisUtilizacionServices : IRepAnalisisUtilizacionDAO
    {
        #region Atributos
        public IRepAnalisisUtilizacionDAO m_uso;
        #endregion
        #region Propiedades
        public IRepAnalisisUtilizacionDAO Uso
        {
            get { return m_uso; }
            set { m_uso = value; }
        }
        #endregion
        #region Constructores
        public RepAnalisisUtilizacionServices(IRepAnalisisUtilizacionDAO uso)
        {
           this.Uso = uso;
        }
        #endregion
        public List<AnalisisDTO> getAnalisis(BusqAnalisiDTO busq)
        {
            return Uso.getAnalisis(busq);
        }
        public List<RepAnalisisDTO> getRepAnalisisUtilizacion(BusqAnalisiDTO busq)
        {
            return Uso.getRepAnalisisUtilizacion(busq);
        }
        public List<ComboDTO> cboAC()
        {
            return Uso.cboAC();
        }
    }
}
