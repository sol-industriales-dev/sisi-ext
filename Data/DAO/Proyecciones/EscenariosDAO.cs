using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class EscenariosDAO : GenericDAO<tblPro_CatEscenarios>, IEscenariosDAO
    {

        public void Guardar(tblPro_CatEscenarios obj)
        {
            if (!ExiteEscenario(obj.descripcion))
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.PROCATESCENARIOS);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.PROCATESCENARIOS);

                if (obj.PadreID == 0)
                {
                    obj.nivel = obj.id;
                    Update(obj, obj.id, (int)BitacoraEnum.PROCATESCENARIOS);
                }
            }

        }

        private bool ExiteEscenario(string obj)
        {

            var Data = _context.tblPro_CatEscenarios.FirstOrDefault(x => x.descripcion == obj);

            if (Data == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public List<tblPro_CatEscenarios> GetListaEscenarios()
        {

            return _context.tblPro_CatEscenarios.Where(x => x.estatus != false).ToList();
        }

        public List<tblPro_CatEscenarios> GetListaEscenariosPrincipales()
        {
            return _context.tblPro_CatEscenarios.Where(x => x.estatus != false && x.PadreID == 0).ToList();
        }

        public List<tblPro_CatEscenarios> GetListEscenariosTable(int id, string descripcion)
        {
            return _context.tblPro_CatEscenarios.Where(x => (id != 0 ? x.id == id : x.id == x.id)
                && (string.IsNullOrEmpty(descripcion) ? x.id == x.id : descripcion == x.descripcion)).ToList();
        }

        public tblPro_CatEscenarios CatEscenarioByID(int id)
        {

            return _context.tblPro_CatEscenarios.FirstOrDefault(x => x.id == id);

        }

    }
}
