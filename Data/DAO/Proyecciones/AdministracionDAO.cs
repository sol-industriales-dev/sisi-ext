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
    public class AdministracionDAO : GenericDAO<tblPro_Administracion>, IAdministracionDAO
    {
        public tblPro_Administracion GetJsonData(FiltrosGeneralDTO filtro)
        {
            var mes = filtro.mes;

            var res = _context.tblPro_Administracion.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == mes);

            if (res == null)
            {
                int m = _context.tblPro_Administracion.Max(x=>x.Mes);
                res = _context.tblPro_Administracion.FirstOrDefault(x => x.Anio == filtro.anio && x.Mes == m);
                return res;
            }
            else
            {
                return res;
            }
        }
        public void GuardarActualizarAdministracion(FiltrosGeneralDTO objFiltro, AdministracionDTO obj)
        {
            var temp=_context.tblPro_Administracion.FirstOrDefault(x=>x.Mes==objFiltro.mes && x.Anio==objFiltro.anio && x.Estatus);
            if (temp != null) {
                temp.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.SaveChanges();
            }
            else
            {
                var o = new tblPro_Administracion();
                o.Mes = objFiltro.mes;
                o.Anio = objFiltro.anio;
                o.Estatus = true;
                o.CadenaJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                _context.tblPro_Administracion.Add(o);
                _context.SaveChanges();
            }
        }

    }
}
