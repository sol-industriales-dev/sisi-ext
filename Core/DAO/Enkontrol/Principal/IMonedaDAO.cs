using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Enkontrol.Principal
{
    public interface IMonedaDAO
    {
        /// <summary>
        /// Guarda Tipo de cambio de hoy
        /// </summary>
        /// <param name="moneda">ID</param>
        /// <param name="tc">Tipo de cambio</param>
        void guardarTC(int moneda, decimal tc);
        /// <summary>
        /// Valida si el usuario tiene permiso de acceso a la ventana de tipo de cambio del sistema de enkontrol Orden de compra
        /// </summary>
        /// <returns>permiso de la vista</returns>
        bool isUsuarioCambiarTC();
        /// <summary>
        /// Consulta el tipo de cambio de hoy
        /// </summary>
        /// <param name="moneda">ID</param>
        /// <returns>Tipo de cambio</returns>
        decimal getTcHoy(int moneda);
        /// <summary>
        /// Lista de monedas
        /// </summary>
        /// <returns>Combobox de monedas</returns>
        List<Core.DTO.Principal.Generales.ComboDTO> FillComboMonedaHoy();
        
    }
}
