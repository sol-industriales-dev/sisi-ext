using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity.Administrativo.Proyecciones
{
    public class tblPro_Obras
    {
        public string Concepto { get; set; }
        public string Escenario { get; set; }
        public int id { get; set; }
        public int Area { get; set; }
        public string AreaNombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Probabilidad { get; set; }
        public int Tipo { get; set; }
        public int Margen { get; set; }
        public decimal Monto { get; set; }
        public decimal Fecha1 { get; set; }
        public decimal Fecha2 { get; set; }
        public decimal Fecha3 { get; set; }
        public decimal Fecha4 { get; set; }
        public decimal Fecha5 { get; set; }
        public decimal Fecha6 { get; set; }
        public decimal Fecha7 { get; set; }
        public decimal Fecha8 { get; set; }
        public decimal Fecha9 { get; set; }
        public decimal Fecha10 { get; set; }
        public decimal Fecha11 { get; set; }
        public decimal Fecha12 { get; set; }
        public decimal Total { get; set; }
        public int IdRenglon { get; set; }
        public bool banderaFinanciamiento { get; set; }
        public int porcentaje { get; set; }
        public string Comentario { get; set; }
        public string CentroCostos { get; set; }
        public int estatus { get; set; }
        public int ? Prioridad { get; set; }
    }
}
