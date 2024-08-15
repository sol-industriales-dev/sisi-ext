using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.EntityFramework.Mapping.Administrativo
{
    public class CatCCBaseDAO : GenericDAO<tblC_CatCCBase>, ICatCCBaseDAO
    {
        void Guardar(tblC_CatCCBase obj)
        {
            SaveEntity(obj, (int)BitacoraEnum.CatCCBase);
        }
        public void MigrarBaseHastaCP2017()
        {
            try
            {
                string consulta = @"SELECT A.cc AS centro_costos
                                   ,(select descripcion from CC C where C.cc = A.cc) nombCC
                                   , SUM (A.monto * A.tipocambio ) as total
                                   , 'Historico hasta fin de año 2016' as descripcion
                               FROM sp_movprov A 
                                   WHERE es_factura ='S'  
                                       and year(a.fechavenc) < 2017
                                   group by centro_costos";
                var lstEk = (List<tblC_CatCCBase>)_contextEnkontrol.Where(consulta).ToObject<List<tblC_CatCCBase>>();
                lstEk.ForEach(x => { Guardar(x); });
            }
            catch (Exception)
            {
            }
        }
        public List<tblC_CatCCBase> getHistorico()
        {
            return _context.tblC_CatCCBase.ToList();
        }
    }
}
