using Core.Enum.Maquinaria.Barrenacion;
using System;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_PiezaBarrenadora
    {
        public int id { get; set; }
        public string noSerie { get; set; }
        public int insumo { get; set; }
        public TipoPiezaEnum tipoPieza { get; set; }
        public TipoBrocaEnum tipoBroca { get; set; }
        public bool barraSegunda { get; set; }
        public decimal horasTrabajadas { get; set; }
        public decimal horasAcumuladas { get; set; }
        public bool reparando { get; set; }
        public int cantidadReparaciones { get; set; }
        public decimal precio { get; set; }
        public bool activa { get; set; }
        public bool montada { get; set; }
        public int culataID { get; set; }
        public int cilindroID { get; set; }
        public int? barrenadoraID { get; set; }
        public string serialExcel { get; set; }
        public string areaCuenta { get; set; }
    }
}
