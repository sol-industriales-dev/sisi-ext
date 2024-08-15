using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enum.Administracion.DocumentosXPagar
{
    public enum PQLineaMovimientoEnum
    {
        ProvisionInteres = 1,
        CambioCC_MontoCCAnterior = 2,
        CambioCC_MontoCCNuevo = 3,
        Renovar_MontoNuevo = 4,
        PoderQuitarLinea = 5,
        Abono = 6
    }
}
