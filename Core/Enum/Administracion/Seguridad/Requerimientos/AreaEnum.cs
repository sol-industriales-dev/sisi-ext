using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.Seguridad.Requerimientos
{
    public enum AreaEnum
    {
        [DescriptionAttribute("Capital Humano")]
        CapitalHumano = 1,
        [DescriptionAttribute("Control de Procesos")]
        ControlProcesos = 2,
        [DescriptionAttribute("Seguridad")]
        Seguridad = 3,
        [DescriptionAttribute("Mantenimiento")]
        Mantenimiento = 4,
        [DescriptionAttribute("Gerencia de Proyecto")]
        GerenciaProyecto = 5,
        [DescriptionAttribute("Alta Gerencia Op")]
        AltaGerenciaOP = 6,
        [DescriptionAttribute("Alta Gerencia Mtto")]
        AltaGerenciaMtto = 7,
        [DescriptionAttribute("Alta Gerencia SST")]
        AltaGerenciaSST = 8,
        [DescriptionAttribute("Almacén")]
        Almacen = 9,
        [DescriptionAttribute("Compras")]
        Compras = 10,
        [DescriptionAttribute("Residente")]
        Residente = 11,
        [DescriptionAttribute("Dirección General")]
        DireccionGeneral = 12,
        [DescriptionAttribute("Contabilidad")]
        Contabilidad = 13,
        [DescriptionAttribute("Licitaciones")]
        Licitaciones = 14,
        [DescriptionAttribute("Nóminas")]
        Nominas = 15,
        [DescriptionAttribute("Comercialización")]
        Comercializacion = 16,
        [DescriptionAttribute("Tecnologías de la información")]
        TecnologiasInformacion = 17,
        [DescriptionAttribute("Contraloría")]
        Contraloria = 18,
        [DescriptionAttribute("P.M.O")]
        PMO = 19,
        [DescriptionAttribute("Adquisición y Renta de Equipos")]
        AdquisicionRentaEquipos = 20,
        [DescriptionAttribute("Construcción")]
        Construccion = 21
    }
}
