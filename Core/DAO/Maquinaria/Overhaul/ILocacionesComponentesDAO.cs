using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Principal.Multiempresa;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface ILocacionesComponentesDAO
    {
        List<tblM_CatLocacionesComponentes> getLocaciones(bool estatus, string descripcion);
        void Guardar(tblM_CatLocacionesComponentes obj);
        void eliminarLocacion(int idLocacion);
        List<tblP_CC> getCentrosCostos();
        List<string> GetCorreosLocacionesOverhaul(List<int> idLocaciones);
    }
}