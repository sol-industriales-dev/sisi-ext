using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class CapPptoDTO
    {
        public int id { get; set; }
        public string actividad { get; set; }
        public string lenActividad { get; set; }
        public string cc { get; set; }
        public int idCC { get; set; }
        public int idAgrupacion { get; set; }
        public string agrupacion { get; set; }
        public int idConcepto { get; set; }
        public string concepto { get; set; }
        public decimal importeEnero { get; set; }
        public decimal importeFebrero { get; set; }
        public decimal importeMarzo { get; set; }
        public decimal importeAbril { get; set; }
        public decimal importeMayo { get; set; }
        public decimal importeJunio { get; set; }
        public decimal importeJulio { get; set; }
        public decimal importeAgosto { get; set; }
        public decimal importeSeptiembre { get; set; }
        public decimal importeOctubre { get; set; }
        public decimal importeNoviembre { get; set; }
        public decimal importeDiciembre { get; set; }
        public decimal importeTotal { get; set; }
        public int anio { get; set; }
        public int idResponsable { get; set; }
        public string responsable { get; set; }
        public int idUsuarioCreacion { get; set; }
        public int idUsuarioModificacion { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaModificacion { get; set; }
        public bool registroActivo { get; set; }
        public int idMes { get; set; }
        public decimal importe { get; set; }
        public bool esAgrupacion { get; set; }
        public decimal importeTotalConcepto { get; set; }
        public string insumoDescripcion { get; set; }
        public string cuentaDescripcion { get; set; }
        public bool autorizado { get; set; }
        public string descripcionCC { get; set; }

        public decimal importeContEnero { get; set; }
        public decimal importeContFebrero { get; set; }
        public decimal importeContMarzo { get; set; }
        public decimal importeContAbril { get; set; }
        public decimal importeContMayo { get; set; }
        public decimal importeContJunio { get; set; }
        public decimal importeContJulio { get; set; }
        public decimal importeContAgosto { get; set; }
        public decimal importeContSeptiembre { get; set; }
        public decimal importeContOctubre { get; set; }
        public decimal importeContNoviembre { get; set; }
        public decimal importeContDiciembre { get; set; }
        public decimal importeContTotal { get; set; }
        public decimal importeContTotalConcepto { get; set; }

        public decimal importeEneroAditiva { get; set; }
        public decimal importeFebreroAditiva { get; set; }
        public decimal importeMarzoAditiva { get; set; }
        public decimal importeAbrilAditiva { get; set; }
        public decimal importeMayoAditiva { get; set; }
        public decimal importeJunioAditiva { get; set; }
        public decimal importeJulioAditiva { get; set; }
        public decimal importeAgostoAditiva { get; set; }
        public decimal importeSeptiembreAditiva { get; set; }
        public decimal importeOctubreAditiva { get; set; }
        public decimal importeNoviembreAditiva { get; set; }
        public decimal importeDiciembreAditiva { get; set; }

        public decimal cumplEnero { get; set; }
        public decimal cumplFebrero { get; set; }
        public decimal cumplMarzo { get; set; }
        public decimal cumplAbril { get; set; }
        public decimal cumplMayo { get; set; }
        public decimal cumplJunio { get; set; }
        public decimal cumplJulio { get; set; }
        public decimal cumplAgosto { get; set; }
        public decimal cumplSeptiembre { get; set; }
        public decimal cumplOctubre { get; set; }
        public decimal cumplNoviembre { get; set; }
        public decimal cumplDiciembre { get; set; }

        public decimal totalAgrupacionCaptura { get; set; }
        public decimal totalAgrupacionPlanMaestro { get; set; }
        public decimal totalRow { get; set; }
        public bool esTotal { get; set; }
        public List<int> lstCCID { get; set; }
    }
}
