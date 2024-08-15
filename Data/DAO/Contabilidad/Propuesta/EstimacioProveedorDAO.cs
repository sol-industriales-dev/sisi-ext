using Core.DAO.Contabilidad.Propuesta;
using Core.Entity.Administrativo.Contabilidad.Propuesta.Proveedores;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Propuesta
{
    public class EstimacioProveedorDAO : GenericDAO<tblC_EstimacionProveedor>, IEstimacioProveedorDAO
    {
        #region Guardar
        public bool guardarEstProv(tblC_EstimacionProveedor estProv)
        {
            var esGuardado = false;
            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                var db = _context.tblC_EstimacionProveedor.ToList().Where(edb => edb.fecha.Year.Equals(estProv.fecha.Year) && edb.fecha.noSemana().Equals(estProv.fecha.noSemana())).FirstOrDefault(edb => edb.esActivo && edb.cc.Equals(estProv.cc) && edb.numpro.Equals(estProv.numpro));
                if (db == null)
                {
                    estProv.fechaRegistro = DateTime.Now;
                    estProv.esActivo = true;
                }
                else
                {
                    estProv.id = db.id;
                    estProv.fechaRegistro = db.fechaRegistro;
                    estProv.esActivo = db.esActivo;
                }    
                _context.tblC_EstimacionProveedor.AddOrUpdate(estProv);
                _context.SaveChanges();
                esGuardado = estProv.id > 0;
                dbTransaction.Commit();
            }
            return esGuardado;
        }
        #endregion
        public List<tblC_EstimacionProveedor> getLstEstProv(DateTime min, DateTime max)
        {
            return _context.tblC_EstimacionProveedor.ToList()
                .Where(e => e.esActivo)
                .Where(e => e.fecha >= min)
                .Where(e => e.fecha <= max).ToList();
        }
        
    }
}
