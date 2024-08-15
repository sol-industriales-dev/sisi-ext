using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class CedulaMensualDetalleInstitucionesDTO
    {
        public int cuenta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public string descripcion { get; set; }
        public decimal saldoInicial { get; set; }
        public decimal cargos { get; set; }
        public decimal abonos { get; set; }
        public decimal saldoActual { get; set; }
        public decimal saldoActualSigoplan { get; set; }
        public decimal diferencia { get; set; }

        public decimal interesesPagados { get; set; }
        public decimal interesesCP { get; set; }
        public decimal interesesLP { get; set; }
    }
}
