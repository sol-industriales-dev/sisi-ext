using Core.DAO.Contabilidad.Reportes;
using Core.Entity.Administrativo.Contabilidad;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad
{
    public class CCDivisionDAO : GenericDAO<tblC_CCDivision> ,ICCDivisionDAO
    {
        #region Guardar
        public bool Guardar(tblC_CCDivision obj)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var save = _context.tblC_CCDivision.ToList().FirstOrDefault(w => w.cc.Equals(obj.cc));
                if (save == null)
                {
                    save = new tblC_CCDivision()
                    {
                        cc = obj.cc,
                    };
                }
                save.division = obj.division;
                _context.tblC_CCDivision.AddOrUpdate(save);
                _context.SaveChanges();
                esGuardado = save.id > 0;
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        public bool Guardar(List<tblC_RelCuentaDivision> lst)
        {
            var lstEsGuardado = new List<bool>();
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var lstbd = _context.tblC_RelCuentaDivision.ToList().Where(w => w.cuenta.Equals(lst.FirstOrDefault().cuenta)).ToList();
                lst.ForEach(rel =>
                {
                    var save = lstbd.FirstOrDefault(w => w.division.Equals(rel.division));
                    if (save == null)
                    {
                        save = new tblC_RelCuentaDivision()
                        {
                            division = rel.division,
                            cuenta = rel.cuenta
                        };
                    }
                    save.esActivo = rel.esActivo;
                    save.fechaRegistro = ahora;
                    _context.tblC_RelCuentaDivision.AddOrUpdate(save);
                    _context.SaveChanges();
                    lstEsGuardado.Add(save.id > 0);
                });
                dbTransaction.Commit();
            }
            return lstEsGuardado.All(g => g); ;
        }
        #endregion
        public List<tblC_RelCuentaDivision> getLstRelCtaDiv()
        {
            return _context.tblC_RelCuentaDivision.ToList();
        }
    }
}
