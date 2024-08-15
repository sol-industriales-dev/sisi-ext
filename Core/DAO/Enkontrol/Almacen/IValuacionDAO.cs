using Core.DTO.Enkontrol.Alamcen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.Almacen
{
    public interface IValuacionDAO
    {
        #region Entrada
        /// <summary>
        /// Consulta los alamcenes de entrada del año
        /// </summary>
        /// <param name="lstComp">Compañías</param>
        /// <returns>Almacenes</returns>
        List<chkAlmacenDTO> getAlamences(List<int> lstComp);
        /// <summary>
        /// Grupos de insumo con entradas
        /// </summary>
        /// <param name="lstAlmacenes">Alamcenes</param>
        /// <returns>Grupos de insumos</returns>
        List<chkAlmacenDTO> getGruposInsumos(List<chkAlmacenDTO> lstAlmacenes);
        /// <summary>
        /// Consulta de valuaciones de entrada
        /// </summary>
        /// <param name="lstGrupo">Grupos y alamcenes</param>
        /// <returns>importes de insumos</returns>
        List<ValuacionDTO> getValuacion(List<chkAlmacenDTO> lstGrupo);
        #endregion
        #region Salida
        /// <summary>
        /// Insumos con salida de inventario
        /// </summary>
        /// <returns>Importes por insumos</returns>
        List<ValuacionDTO> getValuacionSalida(DateTime fecha);
        /// <summary>
        /// Consulta de periodos por salida
        /// </summary>
        /// <returns>Periodos</returns>
        List<chkAlmacenSalidaDTO> getPeriodos(DateTime fecha);
        /// <summary>
        /// Consulta de alamcenes con salidas de inventarios
        /// </summary>
        /// <returns>Almacens</returns>
        List<chkAlmacenSalidaDTO> getAlmacenesSalida(DateTime fecha);
        #endregion
    }
}
