using Core.DTO.Principal.Generales;
using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.Enum.Administracion.Cotizaciones;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enum.Principal.Bitacoras;

namespace Data.DAO.Contabilidad.Reportes
{
    public class CatGiroProvDAO : GenericDAO<tblC_CatGiro>, ICatGiroProvDAO
    {
        public bool saveGiro(tblC_CatGiro giro)
        {
            var isSave = false;
            using (var scope = _context.Database.BeginTransaction()) 
            {
                giro.fechaCaptura = DateTime.Now;
                giro.descripcion = giro.descripcion.ToUpper();
                _context.tblC_CatGiro.AddOrUpdate(giro);
                SaveChanges();
                isSave = giro.id > 0;
                if (isSave)
                {
                    SaveBitacora((int)BitacoraEnum.GiroProveedor, (int)AccionEnum.AGREGAR, giro.id, JsonUtils.convertNetObjectToJson(giro));
                }
                scope.Commit();
            }
            return isSave;
        }
        public List<tblC_CatGiro> getAllGiro()
        {
            return _context.tblC_CatGiro.ToList();
        }
        public List<tblC_CatGiro> getLstGiro()
        {
            return _context.tblC_CatGiro.ToList().Where(giro => giro.esActivo).ToList();
        }
        public List<ComboDTO> getCboGiro()
        {
            var lst = getAllGiro().Where(giro => giro.esActivo).ToList();
            var cbo = lst.Select(giro => new ComboDTO()
            {
                Text = giro.descripcion,
                Value = giro.id.ToString(),
            }).ToList();
            return cbo;
        }
    }
}
