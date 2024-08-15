using Core.DTO;
using Core.Entity.Maquinaria.Catalogo;
using Data.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class TallerDAO
    {
        public IList<tblM_CentroCostos> FillGridTaller(tblM_CentroCostos obj)
        {
            var result = (IList<tblM_CentroCostos>)_contextEnkontrol.Where("SELECT cc as cc,descripcion as des,corto as corto FROM CC where descripcion like '%" + obj.des + "%' AND cc like '%" + obj.cc + "%' AND st_ppto = 'N' ").ToObject<IList<tblM_CentroCostos>>();

            if(vSesiones.sesionEmpresaActual==3)
            {
                result = (IList<tblM_CentroCostos>)_contextEnkontrol.Where("SELECT cc as cc,descripcion as des,corto as corto FROM DBA.CC where descripcion like '%" + obj.des + "%' AND cc like '%" + obj.cc + "%' AND st_ppto = 'N' ").ToObject<IList<tblM_CentroCostos>>();
            }

            return result.Where(x => conversionDato(x.cc) > 20).ToList();
        }

        private int conversionDato(string c)
        {
            int dato=0;
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(c);
            if(match.Success)
            {
                dato = Convert.ToInt32(match.Value);
            }
            return dato;
        }
    }
}
