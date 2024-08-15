using Core.Entity.Maquinaria.Overhaul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity.Maquinaria.Catalogo;
using Core.DTO.Maquinaria.Overhaul;
using Core.DTO.Principal.Generales;

namespace Core.DAO.Maquinaria.Overhaul
{
    public interface IMarcasComponentesDAO
    {
        void Guardar(tblM_CatMarcasComponentes obj);
        List<tblM_CatMarcasComponentes> getLocaciones(bool estatus, string descripcion);
        void eliminar(int idMarca);
    }
}