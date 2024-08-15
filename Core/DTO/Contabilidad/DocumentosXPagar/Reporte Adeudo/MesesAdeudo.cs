using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Contabilidad.DocumentosXPagar.Reporte_Adeudo
{
    public class MesesAdeudo
    {
        public int idInstitucion { get; set; }
        public string descripcionInstitucion { get; set; }
        public decimal anioAnterior { get; set; }
        public decimal enero { get; set; }
        public decimal febrero { get; set; }
        public decimal marzo { get; set; }
        public decimal abril { get; set; }
        public decimal mayo { get; set; }
        public decimal junio { get; set; }
        public decimal julio { get; set; }
        public decimal agosto { get; set; }
        public decimal septiembre { get; set; }
        public decimal octubre { get; set; }
        public decimal noviembre { get; set; }
        public decimal diciembre { get; set; }
        public decimal anioActual { get; set; }
        public int anio { get; set; }


        public decimal anioAnteriorDlls { get; set; }
        public decimal eneroDlls { get; set; }
        public decimal febreroDlls { get; set; }
        public decimal marzoDlls { get; set; }
        public decimal abrilDlls { get; set; }
        public decimal mayoDlls { get; set; }
        public decimal junioDlls { get; set; }
        public decimal julioDlls { get; set; }
        public decimal agostoDlls { get; set; }
        public decimal septiembreDlls { get; set; }
        public decimal octubreDlls { get; set; }
        public decimal noviembreDlls { get; set; }
        public decimal diciembreDlls { get; set; }
        public decimal anioActualDlls { get; set; }

        public int moneda { get; set; }
        public string tipoCambio { get; set; }
    }
}
