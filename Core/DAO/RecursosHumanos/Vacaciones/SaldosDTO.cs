using Core.DTO.RecursosHumanos.Vacaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.RecursosHumanos.Vacaciones
{
    public class SaldosDTO
    {
        public int clave_empleado { get; set; }
        public int num_dias { get; set; }

        //adicionales 
        public string nombre_completo { get; set; }
        //SALDOS ACTUALES
        public int saldo_inicial { get; set; }
        public int saldo_adicional { get; set; }
        public int dias_gozados { get; set; }
        public int saldo_actual { get; set; }
        public DateTime fechaAntiguedad { get; set; }
        public List<SaldosAnualesDTO> lstSaldosAnuales { get; set; }

    }
}
