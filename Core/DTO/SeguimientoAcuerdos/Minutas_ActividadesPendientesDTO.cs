using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.SeguimientoAcuerdos
{
    public class Minutas_ActividadesPendientesDTO
    {
        public int creadorMinutaID { get; set; }
        public string creadorMinuta { get; set; }
        public int minutaID { get; set; }
        public string minuta { get; set; }
        public int departamentoID { get; set; }
        public string departamento { get; set; }
        public int actividadesPendientes { get; set; }
        public int actividadesFinalizadas { get; set; }
        public int actividadesTotal { get; set; }

        public string btnListaAsistencia { get; set; }
        public string btnReporteMinuta { get; set; }
    }
}
