using Core.Enum.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Utils.Auth
{
    /// <summary>
    /// DTO totalmente funcional con la vista parcial _authPanel
    /// </summary>
    public class authDTO
    {
        public int idRegistro { get; set; }
        public int idAuth { get; set; }
        public int idPadre { get; set; }
        public int orden { get; set; }
        public string nombre { get; set; }
        public string firma { get; set; }
        public string descripcion { get; set; }
        public authEstadoEnum authEstado { get; set; }
        public string clase { get; set; }
        public string comentario { get; set; }
        public int siguiente { get; set; }
        public string esAutorizada { get; set; }
        public int estatus { get; set; }
        public string imagen64GraficaDiario { get; set; }
        public string imagen64GraficaSemanal { get; set; }
        public string imagen64GraficaMensual { get; set; }
        public string areaCuenta { get; set; }
        public string descAreaCuenta { get; set; }
        public DateTime fechaInicio { get; set; }
        public string descPeriodoDia { get; set; }
        public string descPeriodoSemana { get; set; }
        public string descPeriodoMes { get; set; }

    }
}
