using Core.DAO.Proyecciones;
using Core.Entity.Administrativo.Proyecciones;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Utils;


namespace Data.DAO.Proyecciones
{
    public class SaldosInicialesDAO : GenericDAO<tblPro_SaldosIniciales>, ISaldosInicialesDAO
    {

        ValidacionProyecciones val = new ValidacionProyecciones();
        public tblPro_SaldosIniciales GetJsonData(int mes, int anio, int estatus)
        {

            var res = _context.tblPro_SaldosIniciales.OrderByDescending(x => x.id).ToList();
            var currentMes = res.FirstOrDefault().Mes;
            int resultado = val.GetData(res, mes, currentMes);

            switch (resultado)
            {
                case 1:
                    return res.FirstOrDefault(x => x.Anio.Equals(anio));
                case 2:
                    return res.FirstOrDefault(x => x.Anio.Equals(anio) && x.Mes.Equals(mes));
                default:
                    break;
            }
            return null;


        }
        public void Guardar(tblPro_SaldosIniciales obj)
        {
            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.SALDOSINICIALES);
            else
                Update(obj, obj.id, (int)BitacoraEnum.SALDOSINICIALES);
        }
        public int getUltimoMesCapturado()
        {
            var res = _context.tblPro_SaldosIniciales.OrderByDescending(x => x.id).ToList();
            int mes = res.FirstOrDefault().Mes;
            return mes;
        }
    }
}
