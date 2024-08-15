using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Maquinaria.Inventario.ControlCalidad
{
    public interface IPreguntasCalidadDAO
    {
        List<tblM_CatPreguntasCalidad> getListPreguntasCalidad();
    }
}
