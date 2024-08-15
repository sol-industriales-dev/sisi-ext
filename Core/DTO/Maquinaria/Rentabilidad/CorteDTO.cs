using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Rentabilidad
{
    public class CorteDTO
    {
        public int id { get; set; }
        public int corteID { get; set; }
        public string cuenta { get; set; }
        public decimal monto { get; set; }
        public DateTime fechapol { get; set; }
        public int tipoEquipo { get; set; }
        public string cc { get; set; }
        public string concepto { get; set; }
        public string poliza { get; set; }
        public string referencia { get; set; }
        public int semana { get; set; }
        public int empresa { get; set; }
        public string areaCuenta { get; set; }
        public int linea { get; set; }
        public int ccEstatus { get; set; }
        //0 -> Existe en semana anterior y semana actual; 1 -> Existe solo en semana actual; 2-> Eciste solo en semana anterior
        public int tipoMov { get; set; }
        public string division { get; set; }
    }
}