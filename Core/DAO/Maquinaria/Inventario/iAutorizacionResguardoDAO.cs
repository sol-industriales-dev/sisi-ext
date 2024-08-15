using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface iAutorizacionResguardoDAO
    {
        void Guardar(tblM_AutorizacionResguardo obj);

        tblM_AutorizacionResguardo GetObjAutorizaciones(int ResguardoVehiculoID);

    }
}
