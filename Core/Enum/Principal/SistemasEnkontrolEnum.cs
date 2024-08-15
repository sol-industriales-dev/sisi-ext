using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Principal
{
    public enum SistemasEnkontrolEnum
    {
        [DescriptionAttribute("st_validacion")]
        General = 0,
        [DescriptionAttribute("sco")]
        Contabilidad = 1,
        [DescriptionAttribute("sbo")]
        Bancos = 2,
        [DescriptionAttribute("scp")]
        Proveedores = 3,
        [DescriptionAttribute("scx")]
        Clientes = 4,
        [DescriptionAttribute("snd")]
        snd = 5,
        [DescriptionAttribute("sin")]
        Inventario = 6,
        [DescriptionAttribute("soc")]
        Compras = 7,
        [DescriptionAttribute("sfa")]
        Facturacion = 8,
        [DescriptionAttribute("scv")]
        Vivienda = 9,
        [DescriptionAttribute("sac")]
        sac = 10,
        [DescriptionAttribute("st_codigo_agrupador")]
        st_codigo_agrupador = 11,
        [DescriptionAttribute("st_estatus_poliza")]
        st_estatus_poliza = 12,
        [DescriptionAttribute("st_saldo_mayor")]
        st_saldo_mayor = 13,
        [DescriptionAttribute("st_poliza_iva")]
        st_poliza_iva = 14,
        [DescriptionAttribute("st_sbo")]
        st_sbo = 15,
        [DescriptionAttribute("st_scx")]
        st_scx = 16,
        [DescriptionAttribute("st_scp")]
        st_scp = 17,
        [DescriptionAttribute("st_saldo_ini")]
        st_saldo_ini = 18,
        [DescriptionAttribute("st_tipo_contable")]
        st_tipo_contable = 19,
        [DescriptionAttribute("st_cta_cfdi")]
        st_cta_cfdi = 20
    }
}
