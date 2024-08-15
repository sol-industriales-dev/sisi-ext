using Core.DTO.Administracion.Seguridad.Capacitacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.CtrlPptalOficinasCentrales
{
    public class InfoDetalleNivelDosIngresoGastoDTO
    {
        public decimal cumplimientoAcumuladoEmpresa { get; set; }
        public string mesCierre { get; set; }
        public List<InfoDetalleNivelDosInfoPorCC> datosTablaCC { get; set; }
        public GraficaDTO graficaBarraMensual { get; set; }
        public decimal maximoGrafica { get; set; }
        public decimal maximoPorcentaje { get; set; }
        public decimal total { get; set; }
    }

    public class InfoDetalleNivelDosInfoPorCC
    {
        public int id { get; set; }
        public int idCC { get; set; }
        public string cc { get; set; }
        public string descripcion { get; set; }
        public decimal gastoMensual { get; set; }
        public decimal ingresoMensual { get; set; }
        public decimal presupuestoMensual { get; set; }
        public decimal cumplimientoMensual { get; set; }
        public decimal gastoAcumulado { get; set; }
        public decimal ingresoAcumulado { get; set; }
        public decimal presupuestoAcumulado { get; set; }
        public decimal cumplimientoAcumulado { get; set; }
        public bool esIngresoGasto { get; set; }
        public int tipo { get; set; }
        public decimal presupuestoEnero { get; set; }
        public decimal gastoEnero { get; set; }
        public decimal presupuestoFebrero { get; set; }
        public decimal gastoFebrero { get; set; }
        public decimal presupuestoMarzo { get; set; }
        public decimal gastoMarzo { get; set; }
        public decimal presupuestoAbril { get; set; }
        public decimal gastoAbril { get; set; }
        public decimal presupuestoMayo { get; set; }
        public decimal gastoMayo { get; set; }
        public decimal presupuestoJunio { get; set; }
        public decimal gastoJunio { get; set; }
        public decimal presupuestoJulio { get; set; }
        public decimal gastoJulio { get; set; }
        public decimal presupuestoAgosto { get; set; }
        public decimal gastoAgosto { get; set; }
        public decimal presupuestoSeptiembre { get; set; }
        public decimal gastoSeptiembre { get; set; }
        public decimal presupuestoOctubre { get; set; }
        public decimal gastoOctubre { get; set; }
        public decimal presupuestoNoviembre { get; set; }
        public decimal gastoNoviembre { get; set; }
        public decimal presupuestoDiciembre { get; set; }
        public decimal gastoDiciembre { get; set; }
        public decimal diferenciaMensual { get; set; }
        public decimal diferenciaAcumulado { get; set; }
        public decimal total { get; set; }
        public decimal gastoAnioPasado { get; set; }
        public decimal diferenciaAnioActualVsAnioAcumulado { get; set; }
        public int empresa { get; set; }
    }
}
