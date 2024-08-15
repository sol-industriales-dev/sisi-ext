using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class CostoEstimadoDAO : GenericDAO<tblC_CostoEstimado>, ICostoEstimadoDAO
    {
        #region Guardar
        public bool guardarLstCostoEstimado(List<tblC_CostoEstimado> lst)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var lstCosEst = getLstCostoEstimado(lst.Min(m => m.fecha));
                lst.ForEach(bd =>
                {
                    bd.esActivo = false;
                    _context.tblC_CostoEstimado.AddOrUpdate(bd);
                    SaveChanges();
                });
                lst.ForEach(estCob =>
                {
                    var guardar = lstCosEst.FirstOrDefault(w => w.cc.Equals(estCob.cc));
                    var esNuevo = guardar == null;
                    if (!esNuevo)
                    {
                        estCob.id = guardar.id;
                    }
                    estCob.esActivo = true;
                    estCob.fechaRegistro = ahora;
                    _context.tblC_CostoEstimado.AddOrUpdate(estCob);
                    SaveChanges();
                    dbTransaction.Commit();
                });
            }
            return lst.All(a => a.id > 0);
        }
        #endregion
        public List<tblC_CostoEstimado> getLstCostoEstimado(DateTime fecha)
        {
            return _context.tblC_CostoEstimado.ToList()
                .Where(w => w.esActivo)
                .Where(w => w.fecha >= fecha && w.fecha >= fecha).ToList();
        }
    }
}
