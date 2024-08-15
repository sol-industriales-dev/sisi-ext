using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Administracion.DocumentosXPagar
{
    public class ContratosDTO
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string Descripcion { get; set; }
        public string Institucion { get; set; }
        public int Plazo { get; set; }
        public int ParcialidadActual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool DiaInhabil { get; set; }
        public decimal Credito { get; set; }
        public decimal AmortizacionCapital { get; set; }
        public decimal Intereses { get; set; }
        public decimal InteresMoratorio { get; set; }
        public decimal TipoCambio { get; set; }
        public bool Domiciliado { get; set; }
        public int PagosVencidos { get; set; }
        public bool ArchivoContrato { get; set; }
        public bool ArchivoPagare { get; set; }
        public int InstitucionId { get; set; }
        public int FechaVencimientoTipoId { get; set; }
        public decimal TasaInteres { get; set; }
        public string rfc { get; set; }
        public decimal montoOpcioncompra { get; set; }
        public int monedaContrato { get; set; }
        public string penaConvencional { get; set; }
        public string fechaFirma { get; set; }
        public string fechaVencimiento { get; set; }

        public decimal? pagoInterino { get; set; }
        public decimal? pagoInterino2 { get; set; }
        public decimal? depGarantia { get; set; }

        public int cta { get; set; }
        public int scta { get; set; }
        public int sscta { get; set; }
        public int digito { get; set; }
        public string ctaConcat { get; set; }

        public string nombreCorto { get; set; }
        public int empresa { get; set; }

        public bool aplicaInteres { get; set; }
        public bool? aplicaContratoIntereses { get; set; }
        public bool tasaFija { get; set; }
        public int arrendamientoPuro { get; set; }
        public string fileContrato { get; set; }

        //esAdmin var para alternar visibilidad de los botones de la tabla
        public bool? esAdmin { get; set; }
    }
}