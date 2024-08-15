using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Proyecciones
{
    public class PagosDivDTO
    {
        public MesDTO PagExt { get; set; }
        public MesDTO PorcSaldoAmortAbono { get; set; }
        public MesDTO CtoAplicarCXC { get; set; }
        public MesDTO ImporteAmortAbono { get; set; }
        public MesDTO BaseImporteAmortAbono { get; set; }
        public MesDTO PorcSaldoAmortCompania { get; set; }
        public MesDTO BaseImporteAmortCompania { get; set; }
        public MesDTO ImporteAmortCompania { get; set; }
        public decimal Saldo { get; set; }
        public decimal PuntosAdic { get; set; }
        public MesDTO BaseTasaAnual { get; set; }
        public MesDTO TasaAnual { get; set; }
        public decimal PagoCapital { get; set; }
        public MesDTO SaldoAmortCompania { get; set; }
        public MesDTO AmortCapitalCompania { get; set; }
        public MesDTO AmortVencidasCapitalCompania { get; set; }
        public MesDTO InteresesGenerados { get; set; }
        public MesDTO PorcSaldoAmortAcreedores { get; set; }
        public MesDTO BaseImporteAmortAcreedores { get; set; }
        public MesDTO ImporteAmortAcreedores { get; set; }
        public MesDTO GastosDiferidos { get; set; }
        public MesDTO SaldoFlujo { get; set; }

        public List<MesDTO> DesgloseVariosPagos { get; set; }

        public MesDTO Reserva_CBA { get; set; }
        public MesDTO Reserva_BA { get; set; }
        public MesDTO Reserva_A { get; set; }

        public MesDTO TotalConceptos { get; set; }
    }
}
