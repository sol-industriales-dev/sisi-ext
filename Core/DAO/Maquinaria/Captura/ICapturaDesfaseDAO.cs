using Core.DTO.Captura;
using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface ICapturaDesfaseDAO
    {
        IList<economicoDTO> getEconomicos(int cc);
        void Guardar(tblM_CapDesfase obj);

        tblM_CapDesfase getDesfase(string economico);
    }
}
