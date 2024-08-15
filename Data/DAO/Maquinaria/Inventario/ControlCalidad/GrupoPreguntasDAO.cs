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
    public class GrupoPreguntasDAO : GenericDAO<tblM_CatGrupoPreguntasCalidad>, IGruposPreguntasCalidadDAO
    {
        public List<tblM_CatGrupoPreguntasCalidad> getListGrupoPreguntas()
        {
            return _context.tblM_CatGrupoPreguntasCalidad.OrderBy(x => x.Id).ToList();
        }
    }
}
