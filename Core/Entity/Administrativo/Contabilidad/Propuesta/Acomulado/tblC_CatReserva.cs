using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado
{
    public class tblC_CatReserva
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        /// <summary>
        /// Seleciona los registros desde el contrado a prorratear
        /// </summary>
        public bool esSeleccionado { get; set; }
        /// <summary>
        /// Toma los registros a prorratear
        /// </summary>
        public bool esAutomatico { get; set; }
        public int tipoReservaSaldoGlobal { get; set; }
        public bool esPrincipal { get; set; }
        /// <summary>
        /// tblC_CatCalculoCatReserva
        /// </summary>
        public string hexColor { get; set; }
        public bool esActivo { get; set; }
        public DateTime fechaRegistro { get; set; }
    }
}
