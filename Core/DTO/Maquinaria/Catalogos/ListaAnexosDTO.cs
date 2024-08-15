using Core.Enum.Maquinaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Maquinaria.Catalogos
{
    public class ListaAnexosDTO
    {

        public string noEconomico { get; set; }
        public int noEconomicoID { get; set; }

        public AnexoMaquinaDTO Factura { get; set; }
        public AnexoMaquinaDTO Pedimento { get; set; }
        public AnexoMaquinaDTO Poliza { get; set; }
        public AnexoMaquinaDTO TarCirculacion { get; set; }
        public AnexoMaquinaDTO PerEspecialCarga { get; set; }
        public AnexoMaquinaDTO Certificacion { get; set; }
        public AnexoMaquinaDTO Contratos { get; set; }
        public AnexoMaquinaDTO CuadroComparativo { get; set; }
        public AnexoMaquinaDTO Ansul { get; set; }

        public string vFactura { get; set; }
        public string vPedimento { get; set; }
        public string vPoliza { get; set; }
        public string vTarCirculacion { get; set; }
        public string vPerEspecialCarga { get; set; }
        public string vCertificacion { get; set; }
        public string vContratos { get; set; }
        public string vCuadroComparativo { get; set; }
        public string vAnsul { get; set; }
        public bool PuedeAnsul { get; set; }

    }
}
