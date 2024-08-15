using Core.Entity.Maquinaria.Captura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Captura
{
    public interface IProyeccionesKPIDAO
    {
        void Guardar(tblM_CapProyeccionesKPI obj);
        List<tblM_CapProyeccionesKPI> ListaKPIMES(int mes);


    }
}
