using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAutorizaMovimientoInternoDAO
    {

        tblM_AutorizaMovimientoInterno GetAutorizadores(int id);
        void GuardarActualizar(tblM_AutorizaMovimientoInterno obj, bool esUsuarioRecibe = false);
        int GetAutorizadores(int tipo, string cc);
    }
}
