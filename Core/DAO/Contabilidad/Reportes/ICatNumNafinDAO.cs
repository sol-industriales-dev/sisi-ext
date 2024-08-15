using Core.Entity.Administrativo.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.Contabilidad.Reportes
{
    public interface ICatNumNafinDAO
    {
        void Guardar(tblC_CatNumNafin obj);
        bool GuardarLstProvNafin(List<tblC_CatNumNafin> lst);
        List<tblC_CatNumNafin> GetLstHanilitadosNumNafin();
        List<object> GetLstNafin();
        List<tblC_CatNumNafin> GetLstNafin(int moneda);
        bool eliminarNafinProv(tblC_CatNumNafin obj);
    }
}
