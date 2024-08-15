using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IRitmoHorometroDAO
    {
        void GuardarRitmo(tblM_CapRitmoHorometro obj);
        tblM_CapRitmoHorometro CapRitmoHorometro(string obj);
        
       
    }
}
