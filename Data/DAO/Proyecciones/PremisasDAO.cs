using Core.DAO.Proyecciones;
using Core.DTO.Proyecciones;
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
    public class PremisasDAO : GenericDAO<tblPro_Premisas>, IPremisasDAO
    {
        public tblPro_Premisas GetJsonData(FiltrosGeneralDTO objFiltro)
        {
            var res = (from af in _context.tblPro_Premisas
                       where af.Mes <= objFiltro.mes &&
                             af.Anio <= objFiltro.anio
                       select af).OrderByDescending(x => x.id).ToList();
            return res.FirstOrDefault();
        }
        public void GuardarActualizarPremisas(tblPro_Premisas objFiltro)
        {
            var temp = _context.tblPro_Premisas.FirstOrDefault(x => x.Mes == objFiltro.Mes && x.Anio == objFiltro.Anio && x.Estatus);
            if (temp != null)
            {
                temp.CadenaJson = objFiltro.CadenaJson;
                _context.SaveChanges();
            }
            else
            {
                var o = new tblPro_Premisas();
                o.Anio = objFiltro.Anio;
                o.Mes = objFiltro.Mes;
                o.Estatus = true;
                o.CadenaJson = objFiltro.CadenaJson;
                _context.tblPro_Premisas.Add(o);
                _context.SaveChanges();
            }
        }
    }
}
