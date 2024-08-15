using Core.Enum.Maquinaria.Barrenacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Core.Entity.Maquinaria.Barrenacion
{
    public class tblB_CapturaDiaria
    {
        public int id { get; set; }
        public DateTime fechaCaptura { get; set; }
        public DateTime fechaCreacion { get; set; }
        public decimal horometroFinal { get; set; }
        public decimal horasTrabajadas { get; set; }
        public TipoCapturaEnum tipoCaptura { get; set; }
        public int turno { get; set; }
        public int claveOperador { get; set; }
        public decimal precioOperador { get; set; }
        public decimal fsrOperador { get; set; }
        public decimal totalOperador { get; set; }
        public int claveAyudante { get; set; }
        public decimal precioAyudante { get; set; }
        public decimal fsrAyudante { get; set; }
        public decimal totalAyudante { get; set; }
        public int brocaID { get; set; }
        public int martilloID { get; set; }
        public int barraID { get; set; }
        public int barraSegundaID { get; set; }
        public int culataID { get; set; }
        public int portabitID { get; set; }
        public int cilindroID { get; set; }
        public int zancoID { get; set; }
        public int barrenadoraID { get; set; }

        public string brocaSerie { get; set; }
        public string martilloSerie { get; set; }
        public string barraSerie { get; set; }
        public string barraSegundaSerie { get; set; }
        public string culataSerie { get; set; }
        public string portabitSerie { get; set; }
        public string cilindroSerie { get; set; }
        public string zancoSerie { get; set; }

        public decimal bordo { get; set; }
        public decimal espaciamiento { get; set; }
        public int barrenos { get; set; }
        public decimal profundidad { get; set; }
        public string banco { get; set; }
        public decimal densidadMaterial { get; set; }
        public int tipoBarreno { get; set; }
        public decimal subBarreno { get; set; }
        [JsonIgnore]
        public virtual tblB_Barrenadora barrenadora { get; set; }
        public int usuarioCreadorID { get; set; }
        [JsonIgnore]
        public virtual List<tblB_DetalleCaptura> detalles { get; set; }
        public string areaCuenta { get; set; }
        public int rehabilitacion { get; set; }
    }
}
