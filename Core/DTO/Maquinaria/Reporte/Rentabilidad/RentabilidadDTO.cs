using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.Rentabilidad
{
    public class RentabilidadDTO
    {
        public int id { get; set; }
        public string noEco { get; set; }
        public int cta { get; set; }
        //public decimal hrsTrabajadas { get; set; }
        public string tipoInsumo { get; set; }
        public string tipoInsumo_Desc { get; set; }
        public string grupoInsumo { get; set; }
        public string grupoInsumo_Desc { get; set; }
        public int insumo { get; set; }
        public string insumo_Desc { get; set; }
        public string areaCuenta { get; set; }
        public int tipo_mov { get; set; }
        public decimal importe { get; set; }
        public DateTime fecha { get; set; }
        public int tipo { get; set; }
        public string poliza { get; set; }
        public string cc { get; set; }
        //public int area { get; set; }
        //public int cuenta { get; set; }
        public string referencia { get; set; }
        public int semana { get; set; }
        public int corteID { get; set; }
        public int empresa { get; set; }
        public string division { get; set; }
        public int linea { get; set; }
        public int tipoMov { get; set; }
    }
}
