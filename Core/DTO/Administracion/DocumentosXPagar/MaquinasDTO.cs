using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class MaquinasDTO
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int MaquinaId { get; set; }
        public string NumeroEconomico { get; set; }
        public decimal Credito { get; set; }
    }
}