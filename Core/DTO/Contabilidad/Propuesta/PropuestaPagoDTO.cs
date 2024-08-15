using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.Propuesta
{
    public class PropuestaPagoDTO
    {
        /// <summary>
        /// tipoPropuestaEnum. Tipo del estilo de la celda
        /// </summary>
        public string clase { get; set; }
        /// <summary>
        /// GrupoPropuestaEnum. Clase de suma de la misma clase
        /// </summary>
        public string grupo { get; set; }
        /// <summary>
        /// GrupoPropuestaEnum. Listado de clase que concatena las sumas. Similar a referenciar celdas.
        /// </summary>
        public List<string> lstGrupoConca { get; set; }
        public string desc { get; set; }
        public int nivelSuma { get; set; }
        public bool esEscondido { get; set; }
        public List<PropuestaCCDTO> cc { get; set; }
    }
    public class PropuestaCCDTO
    {
        public string cc { get; set; }
        public decimal saldo { get; set; }
    }
}
