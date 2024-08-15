using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario.ControlCalidad
{
    class PreguntasCalidadDAO : GenericDAO<tblM_CatPreguntasCalidad>, IPreguntasCalidadDAO
    {
        public List<tblM_CatPreguntasCalidad> getListPreguntasCalidad()
        {
            return _context.tblM_CatPreguntasCalidad.OrderBy(x => x.IdGrupo).ThenBy(x=> x.Id).ToList();
        }
    }
}
