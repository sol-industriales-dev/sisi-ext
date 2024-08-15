using System;

namespace Core.Entity.Maquinaria.Captura
{
    public class tblM_CapNominaMensualCC
    {
        public int id { get; set; }
        public int areaCuentaID { get; set; }
        public int mes { get; set; }
        public int año { get; set; }
        public decimal horasHombreTotales { get; set; }
        public decimal nominaIMSS { get; set; }
        public decimal nominaInfonavit { get; set; }
        public decimal ISN { get; set; }
        public decimal ISR { get; set; }
        public int usuarioCreaID { get; set; }
        public DateTime fechaCreacion { get; set; }
        public int? usuarioEditaID { get; set; }
        public DateTime? fechaEdicion { get; set; }
        public bool completo { get; set; }
    }
}
