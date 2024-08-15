using Core.DAO.Maquinaria.Inventario.ControlCalidad;
using Core.Entity.Maquinaria.Inventario.ControlCalidad;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario.ControlCalidad
{
    public class RespuestasCalidadDAO : GenericDAO<tblM_RelPreguntaControlCalidad>, IRespuestasCalidadDAO
    {
        public void saveRespuestasCalidad(List<tblM_RelPreguntaControlCalidad> lstObj)
        {
            IObjectSet<tblM_RelPreguntaControlCalidad> _objectSet = ((IObjectContextAdapter)_context).ObjectContext.CreateObjectSet<tblM_RelPreguntaControlCalidad>();
            try
            {
                var Bandera = lstObj.Where(x => x.id != 0);
                if (Bandera.Count() > 0)
                {
                    foreach (var item in lstObj)
                    {
                        tblM_RelPreguntaControlCalidad existe = _context.tblM_RelPreguntaControlCalidad.Find(item.id);

                        if (existe != null)
                        {
                            tblM_RelPreguntaControlCalidad update = _context.tblM_RelPreguntaControlCalidad.Find(item.id);
                            update.Respuesta = item.Respuesta;
                            update.Cantidad = item.Cantidad;
                            _context.SaveChanges();

                        }

                    }

                }
                else
                {

                    _context.tblM_RelPreguntaControlCalidad.AddRange(lstObj);
                    _context.SaveChanges();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<tblM_RelPreguntaControlCalidad> getListRespuestasCalidad(int idCalidad)
        {
            return _context.tblM_RelPreguntaControlCalidad.Where(x => x.IdControl == idCalidad).OrderBy(x => x.id).ToList();
        }
    }
}
