using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Overhaul
{
    public class TallerOverhaulDTO
    {
        public string CCName { get; set; }
        public string economico  { get; set; }
        public int maquinaID { get; set; }
        public int modeloID  { get; set; }
        public int tipoOverhaul  { get; set; }
        public int estatusOverhaul  { get; set; }
        public string fecha  { get; set; }
        public string fechaInicio  { get; set; }
        public string fechaInicioFix  { get; set; }
        public string fechaFinP  { get; set; }
        public string fechaFin  { get; set; }
        public string fechaFinFix  { get; set; }
        public int mes  { get; set; }
        public int id  { get; set; }
        public decimal diasProgramados  { get; set; }
        public string subconjunto  { get; set; }
        public string componente  { get; set; }
        public decimal target  { get; set; }
        public decimal horasComponente { get; set; }
        public bool falla { get; set; }
    }
}
