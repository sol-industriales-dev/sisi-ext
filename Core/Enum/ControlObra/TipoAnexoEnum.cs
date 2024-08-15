using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.ControlObra
{
      public enum TipoAnexoEnum
    {
        [DescriptionAttribute("Contrato")]
        contrato = 13,
        [DescriptionAttribute("Orden de Compra")]
        ind_ordenCompra = 1,
        [DescriptionAttribute("Catálogo de Conceptos")]
        ind_catalogoConceptos = 2,
        [DescriptionAttribute("Planos o Layout")]
        ind_planosLayout = 3,
        [DescriptionAttribute("Cronograma de Obra")]
        ind_cronogramaObra = 4,
        [DescriptionAttribute("Entrega Recepción")]
        ind_entregaRecepcion = 5,
        [DescriptionAttribute("Copias Certificadas de Personalidad")]
        ind_personalidad = 6,
        [DescriptionAttribute("Garantías del Contrato")]
        ind_garantias = 7,
        [DescriptionAttribute("Especificaciones de la Obra")]
        cons_especificacionesObra = 8,
        [DescriptionAttribute("Programa Detallado")]
        cons_programaDetallado = 9,
        [DescriptionAttribute("Alcances de la Obra")]
        cons_alcancesObra = 10,
        [DescriptionAttribute("Circulares")]
        cons_circulares = 11,
        [DescriptionAttribute("Garantías Contractuales")]
        cons_garantias = 12
    }
}
