using Core.DAO.Enkontrol.Almacen;
using Core.DTO.Enkontrol.Alamcen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Enkontrol.Almacen
{
    public class ValuacionService : IValuacionDAO
    {
        #region Atributo
        public IValuacionDAO e_vDAO;
        #endregion
        #region Propiedades
        public IValuacionDAO VDAO 
        {
            get { return e_vDAO; }
            set { e_vDAO = value; } 
        }
        #endregion
        #region Constructor
        public ValuacionService(IValuacionDAO vDAO)
        {
            this.VDAO = vDAO;
        }
        #endregion
        #region Entrada
        public List<chkAlmacenDTO> getAlamences(List<int> lstComp)
        {
            return VDAO.getAlamences(lstComp);
        }
        public List<chkAlmacenDTO> getGruposInsumos(List<chkAlmacenDTO> lstAlmacenes)
        {
            return VDAO.getGruposInsumos(lstAlmacenes);
        }
        public List<ValuacionDTO> getValuacion(List<chkAlmacenDTO> lstGrupo)
        {
            return VDAO.getValuacion(lstGrupo);
        }
        #endregion
        #region salida
        public List<ValuacionDTO> getValuacionSalida(DateTime fecha)
        {
            return VDAO.getValuacionSalida(fecha);
        }
        public List<chkAlmacenSalidaDTO> getPeriodos(DateTime fecha)
        {
            return VDAO.getPeriodos(fecha);
        }
        public List<chkAlmacenSalidaDTO> getAlmacenesSalida(DateTime fecha)
        {
            return VDAO.getAlmacenesSalida(fecha);
        }
        #endregion
    }
}
