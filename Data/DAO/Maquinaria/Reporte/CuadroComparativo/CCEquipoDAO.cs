using Core.DAO.Maquinaria.Reporte.CuadroComparativo;
using Core.Entity.Maquinaria.Reporte.CuadroComparativo.Equipo;
using Core.Enum.Maquinaria.Reportes.CuadroComparativo.Equipo;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Reporte.CuadroComparativo
{
    public class CCEquipoDAO : GenericDAO<tblM_CCE_EncEquipo>, ICCEquipoDAO
    {
        #region AsignacionNoEconomico
        public List<tblM_CCE_EncEquipo> GetCuadroEquipo(List<int> lstIdAsignacion)
        {
            lstIdAsignacion = lstIdAsignacion == null ? new List<int>() : lstIdAsignacion;
            return (from equipo in _context.tblM_CCE_EncEquipo
                    where equipo.esActivo && lstIdAsignacion.Contains(equipo.IdAsignacion)
                    select equipo).ToList();
        }
        #endregion
        #region _formCCEquipo
        public List<tblM_CCE_CatConcepto> LstCatalogoActivo()
        {
            return (from catalogo in _context.tblM_CCE_CatConcepto
                    where catalogo.esActivo
                    select catalogo).ToList();
        }
        public tblM_CCE_EncEquipo GetCuadroEquipo(int idAsignacion)
        {
            var asignacion = _context.tblM_AsignacionEquipos.FirstOrDefault(w => w.id == idAsignacion);
            var cuadro = _context.tblM_CCE_EncEquipo.FirstOrDefault(w => w.IdAsignacion == idAsignacion) ?? new tblM_CCE_EncEquipo();
            if(cuadro.Id == 0)
            {
                cuadro.IdAsignacion = idAsignacion;
                switch(asignacion.Economico.ToUpper())
                {
                    case "COMPRA":
                        cuadro.IdAdquisicion = AdquisicionEnum.Compra;
                        break;
                    case "RENTA":
                        cuadro.IdAdquisicion = AdquisicionEnum.Renta;
                        break;
                    default:
                        cuadro.IdAdquisicion = AdquisicionEnum.Roc;
                        break;
                }
#if DEBUG
                var equipo = new tblM_CCE_DetEquipo();
                equipo.IdProveedor = 25;
                equipo.IdMarca = asignacion.SolicitudDetalle.ModeloEquipo.marcaEquipoID;
                equipo.IdModelo =  asignacion.SolicitudDetalle.modeloEquipoID;
                equipo.Valores = new List<tblM_CCE_DetConcepto> {
                    new tblM_CCE_DetConcepto{
                        IdConcepto = 4,
                        Valor = "45.98"
                    }
                };
                equipo.Caracteristicas = new List<tblM_CCE_DetCaracteristicas> {
                    new tblM_CCE_DetCaracteristicas
                    {
                        Orden = 1,
                        Descripcion = "Titulo",
                        Valor = "Valor caracteristica"
                    }
                };
                cuadro.Equipos = new List<tblM_CCE_DetEquipo> {
                    equipo
                };
#endif
            }
            return cuadro;
        }
        #endregion
    }
}
