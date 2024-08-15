using Core.DAO.Kubrix;
using Core.DTO.Kubrix;
using Core.Entity.Kubrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Kubrix
{
    public class BaseDatosServices : IBaseDatosDAO
    {
        #region Atributos
        private IBaseDatosDAO k_bdDAO { get; set; }
        #endregion
        #region Propiedades
        public IBaseDatosDAO BdDAO
        {
            get { return k_bdDAO; }
            set { k_bdDAO = value; }
        }
        #endregion
        #region Constructores
        public BaseDatosServices(IBaseDatosDAO bdDAO)
        {
            this.BdDAO = bdDAO;
        }
        #endregion
        public List<VencimientoDTO> lstVencimiento()
        {
            return this.BdDAO.lstVencimiento();
        }
        public List<SalContCCDTO> lstSalContCC(int anio)
        {
            return this.BdDAO.lstSalContCC(anio);
        }
        public List<object> getInfoMaquinaria(DateTime fechaInicio, DateTime fechaFin)
        {
            return this.BdDAO.getInfoMaquinaria(fechaInicio, fechaFin);
        }
        public List<object> getInfoCapturaMaquinaria(DateTime fechaInicio, DateTime fechaFin, string cc)
        {
            return this.BdDAO.getInfoCapturaMaquinaria(fechaInicio, fechaFin, cc);
        }
        public List<tblK_CatAvance> lstArchivos()
        {
            return this.BdDAO.lstArchivos();
        }

        public void CapturarMaq(List<CapturaMaqDTO> arr)
        {
            this.BdDAO.CapturarMaq(arr);
        }
    }
}
