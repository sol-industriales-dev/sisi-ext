using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
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
    public class ActivoFijoDAO : GenericDAO<tblPro_ActivoFijo>, IActivoFijoDAO
    {
        ValidacionProyecciones val = new ValidacionProyecciones();
        public tblPro_ActivoFijo GetJsonData(FiltrosGeneralDTO objFiltro)
        {
            //var res = (from af in _context.tblPro_ActivoFijo
            //           where af.Mes <= objFiltro.mes &&
            //                 af.Anio <= objFiltro.anio
            //           select af).OrderByDescending(x => x.id).ToList();
            //return res.FirstOrDefault();

            var res = _context.tblPro_ActivoFijo.OrderByDescending(x => x.id).ToList();
            var currentMes = res.FirstOrDefault().Mes;
            int resultado = val.GetData(res, objFiltro.mes, currentMes);

            switch (resultado)
            {
                case 1:
                    return res.FirstOrDefault(x => x.Anio.Equals(objFiltro.anio));
                case 2:
                    return res.FirstOrDefault(x => x.Anio.Equals(objFiltro.anio) && x.Mes.Equals(objFiltro.mes));
                default:
                    break;
            }
            return null;
        }
        public void GuardarActualizarActivoFijo(tblPro_ActivoFijo obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.USUARIO);
            else
                Update(obj, obj.id, (int)BitacoraEnum.CADENAPRODUCTIVA);
        }
        public int getUltimoMesCapturado(int Mes)
        {
            var res = _context.tblPro_ActivoFijo.OrderByDescending(x => x.id).ToList();
            int mes = !res.Count.Equals(0) ? res[0].Mes : Mes;
            return mes;
        }

    }
}
