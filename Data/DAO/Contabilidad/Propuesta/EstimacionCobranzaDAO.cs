using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Acomulado;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class EstimacionCobranzaDAO : GenericDAO<tblC_EstimacionCobranza>, IEstimacionCobranzaDAO
    {
        #region Guardar
        public bool guardarEstimacionCobro(List<tblC_EstimacionCobranza> lst)
        {
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var ahora = DateTime.Now;
                var lstEstCob = getLstEstimacionCobranza(lst.Min(m => m.fecha));
                lstEstCob.ForEach(r =>
                {
                    r.esActivo = false;
                    _context.tblC_EstimacionCobranza.AddOrUpdate(r);
                });
                lst.ForEach(estCob =>
                {
                    var guardar = lstEstCob.FirstOrDefault(w => w.cc.Equals(estCob.cc));
                    var esNuevo = guardar == null;
                    if (!esNuevo)
                    {
                        estCob.id = guardar.id;
                    }
                    estCob.esActivo = true;
                    estCob.fechaRegistro = ahora;
                    _context.tblC_EstimacionCobranza.AddOrUpdate(estCob);
                    SaveChanges();
                    dbTransaction.Commit();
                });
                return lst.All(a => a.id > 0);
            }
        }
        #endregion
        public List<tblC_EstimacionCobranza> getLstEstimacionCobranza(DateTime fecha)
        {
            return _context.tblC_EstimacionCobranza.ToList()
                .Where(w => w.esActivo)
                .Where(w => w.fecha >= fecha)
                .Where(w => w.fecha <= fecha).ToList();
        }
    }
}
