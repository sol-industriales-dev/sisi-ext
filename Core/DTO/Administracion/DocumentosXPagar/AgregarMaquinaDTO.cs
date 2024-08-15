using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class AgregarMaquinaDTO
    {
        public int ContratoId { get; set; }
        public int MaquinaId { get; set; }
        public decimal Credito { get; set; }
        public decimal porcentaje { get; set; }

    }
}