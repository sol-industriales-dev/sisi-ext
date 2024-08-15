using Core.Entity.Maquinaria.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario
{
    public interface IAutorizaStandbyDAO
    {
        void Guardar(tblM_AutorizaStandby obj);
        int GetUsuarioValida(int id, string CC);
        tblM_AutorizaStandby getAutorizacionesbyStandbyID(int id);

    }
}
