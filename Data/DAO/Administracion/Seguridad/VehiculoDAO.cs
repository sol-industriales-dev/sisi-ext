using Core.DAO.Administracion.Seguridad;
using Core.Entity.Administrativo.Seguridad;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Administracion.Seguridad
{
    public class VehiculoDAO : GenericDAO<tblS_Vehiculo>, IVehiculoDAO
    {
        #region Consulta
        public tblS_Vehiculo getVehiculo(string cc, string eco, DateTime fecha)
        {
            var v = _context.tblS_Vehiculo.FirstOrDefault(w => w.cc.Equals(cc) && w.economico.Equals(eco) && w.fecha.Year.Equals(fecha.Year) && fecha.Month.Equals(fecha.Month));
            if (v == null)
                v = new tblS_Vehiculo() { cc = cc, economico = eco, fecha = fecha };
            return v;
        }
        public List<tblS_Observaciones> getLstObs(int idVehiculo)
        {
            return _context.tblS_Observaciones.Where(w => w.idVehiculo.Equals(idVehiculo)).ToList();
        }
        public List<tblS_CatPartes> getLstPartes()
        {
            return _context.tblS_CatPartes.ToList();
        }
        #endregion
        #region combobox
        public List<object> fillCboEconomico(string cc)
        {
            var lst = _context.tblM_CatMaquina.Where(w => string.IsNullOrEmpty(cc) ? w.estatus != 0 : w.centro_costos.Equals(cc) && w.estatus != 0).ToList();
            return lst.Select(x => new
            {
                Text = x.noEconomico,
                Value = x.noEconomico
            }).Cast<object>().ToList();
        }
        #endregion
    }
}
