using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Captura
{
    public class RptAceitesLubricantes
    {
        public string noEconomico { get; set; }
        public string horasTrabajadas { get; set; }
        public decimal Antifreeze { get; set; }
        public decimal motor { get; set; }
        public decimal motor2 { get; set; }
        public decimal trans { get; set; }
        public decimal hidraulico { get; set; }
        public decimal diferenciales { get; set; }
        public decimal mandoFinal { get; set; }
        public decimal direccion { get; set; }
        public decimal grasa { get; set; }
        public decimal otros1 { get; set; }
        public decimal otros2 { get; set; }
        public decimal otros3 { get; set; }
        public decimal otros4 { get; set; }

        public int motorDes { get; set; }
        public int motor2Des { get; set; }
        public int transDescr { get; set; }
        public int hidraulicoDesc { get; set; }
        public int difDesc { get; set; }
        public int mandoFinalDesc { get; set; }
        public int direccionDesc { get; set; }
        public int grasaDesc { get; set; }
        public int otro1Desc { get; set; }
        public int otro2Desc { get; set; }
        public int otro3Desc { get; set; }
        public int otro4Desc { get; set; }

    }
}
