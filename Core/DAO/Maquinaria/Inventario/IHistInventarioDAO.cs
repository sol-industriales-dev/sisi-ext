using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IHistInventarioDAO
    {
        string GetInfoHistorial(DateTime fecha);

    }
}
