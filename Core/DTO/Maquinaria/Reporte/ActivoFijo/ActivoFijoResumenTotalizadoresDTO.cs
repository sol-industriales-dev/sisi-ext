using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Reporte.ActivoFijo
{
    public class ActivoFijoResumenTotalizadoresDTO
    {
        public decimal TotalDepAcumuladaAnterior { get; set; }
        public decimal TotalDepAñoActual { get; set; }
        public decimal TotalBajaDep { get; set; }
        public decimal TotalDepContableAcumulada { get; set; }
        public decimal TotalDepValLibros { get; set; }

        public List<ActivoFijoDetallesTotalizadoresDTO> Detalles { get; set; }

        public decimal TotalDepAcumuladaAnteriorSalCont { get; set; }
        public decimal TotalDepAñoActualSalCont { get; set; }
        public decimal TotalBajaDepSalCont { get; set; }
        public decimal TotalDepContableAcumuladaSalCont { get; set; }
        public decimal TotalDepValLibrosSalCont { get; set; }

        public ActivoFijoResumenTotalizadoresDTO(List<ActivoFijoDetallesTotalizadoresDTO> detalles)
        {
            Detalles = detalles;

            foreach (var item in detalles)
            {
                TotalDepAcumuladaAnterior += item.DepAcumuladaAnterior;
                TotalDepAñoActual += item.DepAñoActual;
                TotalBajaDep += item.BajaDep;
                TotalDepContableAcumulada += item.DepContableAcumulada;
                TotalDepValLibros += item.DepValLibros;

                TotalDepAcumuladaAnteriorSalCont += item.DepAcumuladaAnteriorSalCont;
                TotalDepAñoActualSalCont += item.DepAñoActualSalCont;
                TotalBajaDepSalCont += item.BajaDepSalCont;
                TotalDepContableAcumuladaSalCont += item.DepContableAcumuladaSalCont;
                TotalDepValLibrosSalCont += item.DepValLibrosSalCont;
            }
        }
    }

    public class ActivoFijoDetallesTotalizadoresDTO
    {
        public int Cuenta { get; set; }
        public string Concepto { get; set; }
        public decimal DepAcumuladaAnterior { get; set; }
        public decimal DepAñoActual { get; set; }
        public decimal BajaDep { get; set; }
        public decimal DepContableAcumulada { get; set; }
        public decimal DepValLibros { get; set; }

        public decimal DepAcumuladaAnteriorSalCont { get; set; }
        public decimal DepAñoActualSalCont { get; set; }
        public decimal BajaDepSalCont { get; set; }
        public decimal DepContableAcumuladaSalCont { get; set; }
        public decimal DepValLibrosSalCont { get; set; }
    }
}