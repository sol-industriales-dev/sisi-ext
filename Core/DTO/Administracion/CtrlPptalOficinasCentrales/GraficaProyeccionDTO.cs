using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class GraficaProyeccionDTO
    {
        public List<string> lstCategorias { get; set; }
        public List<decimal> lstPpto { get; set; }
        public List<decimal> lstPptoReal { get; set; }
        public List<decimal> lstCumplimiento { get; set; }
        public List<decimal> lstProyeccion { get; set; }
        public decimal max { get; set; }
        public List<decimal> lstPointKey { get; set; }
        public List<decimal> lstPptosNew { get; set; }
        public int mesActualID { get; set; }
        public List<decimal> lstCumplimientoPorc { get; set; }

        public GraficaProyeccionDTO()
        {
            lstCategorias = new List<string>();
            lstPpto = new List<decimal>();
            lstPptoReal = new List<decimal>();
            lstCumplimiento = new List<decimal>();
            lstProyeccion = new List<decimal>();
            max = new decimal();
            lstPointKey = new List<decimal>();
            lstPptosNew = new List<decimal>();
            mesActualID = new int();
            lstCumplimientoPorc = new List<decimal>();
        }
    }
}
