using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IParos
    {
        List<tblM_Paros> getParosMaquina(int obj);
    }
}
