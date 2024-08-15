using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.ControlInterno.Obra
{
    public enum tipoCatalogoEnum
    {
        [DescriptionAttribute("Area Cuenta")]
        AreaCuenta = 1,
        //[DescriptionAttribute("Maquinaria")]
        //Maquinaria = 2,
        [DescriptionAttribute("Departamento Administrativo")]
        DepartamentoAdministrativo = 3,
        [DescriptionAttribute("Departamento de Nomina")]
        DepartamentoDeNomina = 4,
    }
}
