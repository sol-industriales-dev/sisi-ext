using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Proyecciones
{
    public class CxCDAO : GenericDAO<tblPro_CxC>, ICxCDAO
    {
        ValidacionProyecciones val = new ValidacionProyecciones();
        public tblPro_CxC GetJsonData(int escenario, int meses, int anio)
        {

            /*      var res = _context.tblPro_CxC.Where(x => x.Mes.Equals(meses) && x.Anio.Equals(anio)).OrderByDescending(x => x.id);

                  if (res.Count() == 0)
                  {
                      res = _context.tblPro_CxC.OrderByDescending(x => x.id);
                  }
                  return res.FirstOrDefault();*/

            var res = _context.tblPro_CxC.OrderByDescending(x => x.id).ToList();
            var currentMes = res.FirstOrDefault().Mes;
            int resultado = val.GetData(res, meses, currentMes);

            switch (resultado)
            {
                case 1:
                    return res.FirstOrDefault(x => x.Anio.Equals(anio));
                case 2:
                    return res.FirstOrDefault(x => x.Anio.Equals(anio) && x.Mes.Equals(meses));
                default:
                    break;
            }
            return null;

        }
        public void Guardar(tblPro_CxC obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.SALDOSINICIALES);
            else
                Update(obj, obj.id, (int)BitacoraEnum.SALDOSINICIALES);
        }

        public int getUltimoMesCapturado()
        {
            var res = _context.tblPro_CxC.OrderByDescending(x => x.id).ToList();
            int mes = res.FirstOrDefault().Mes;
            return mes;
        }

    }
}
