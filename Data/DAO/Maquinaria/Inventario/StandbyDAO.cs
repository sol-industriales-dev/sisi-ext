using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Inventario
{
    public class StandbyDAO : GenericDAO<tblM_CapStandBy>, IStandByDAO
    {

        public void GuardarStandBy(tblM_CapStandBy obj)
        {

            if (obj.id == 0)
                SaveEntity(obj, (int)BitacoraEnum.STANDBY);
            else
                Update(obj, obj.id, (int)BitacoraEnum.STANDBY);
        }

        public List<tblM_CapStandBy> getListaStandBy(List<string> listCC, DateTime fechainicio, DateTime fechaFin, int filtro)
        {
            if (listCC.Count > 0)
            {
                var ListaCapStandBy = _context.tblM_CapStandBy.Where(x => listCC.Contains(x.CC) && x.estatus == filtro).ToList();

                return ListaCapStandBy;
            }
            else
            {
                var ListaCapStandBy = _context.tblM_CapStandBy.Where(x => x.estatus == filtro).ToList();

                return ListaCapStandBy;
            }
        }

        public List<StandbyGridDTO> GetListMaquinaria(List<string> listCC, DateTime fechaInicio, DateTime fechaFin)
        {
            var EquiposObra = _context.tblM_CatMaquina.Where(x => listCC.Contains(x.centro_costos) && x.estatus == 1 && x.grupoMaquinaria.tipoEquipoID != 3).ToList();

            var listaExcepciones = grupoMaquinaria();

            var EquiposTransporteValidos = _context.tblM_CatMaquina.Where(x => listCC.Contains(x.centro_costos) && listaExcepciones.Contains(x.grupoMaquinariaID)).ToList();


            EquiposObra.AddRange(EquiposTransporteValidos);
            var result = new List<StandbyGridDTO>();

            foreach (var maquinaria in EquiposObra.Where(x => !string.IsNullOrEmpty(x.noEconomico)))
            {

                var Horometro = _context.tblM_CapHorometro.Where(x => x.Economico == maquinaria.noEconomico && x.Fecha <= fechaInicio).OrderByDescending(x => x.id).FirstOrDefault();
                var HorometroFinal = _context.tblM_CapHorometro.Where(x => x.Economico == maquinaria.noEconomico && x.Fecha <= fechaFin).OrderByDescending(x => x.id).FirstOrDefault();

                var numCapturas = _context.tblM_CapHorometro.Where(x => x.Economico == maquinaria.noEconomico && (x.Fecha >= fechaInicio && x.Fecha <= fechaFin)).ToList();

                StandbyGridDTO maquina = new StandbyGridDTO()
                {

                    noEconomicoID = maquinaria.id,
                    Economico = maquinaria.noEconomico,
                    Grupo = maquinaria.grupoMaquinaria.descripcion,
                    Modelo = maquinaria.modeloEquipo.descripcion,
                    centro_costos = maquinaria.centro_costos,
                    HorometroInicial = maquinaria.horometroActual,
                    HorometroFinal = maquinaria.horometroActual
                };


                maquina.HorometroInicial = Horometro != null ? Horometro.Horometro - Horometro.HorasTrabajo : 0;
                maquina.HorometroFinal = HorometroFinal != null ? HorometroFinal.Horometro : 0;


                if (numCapturas.Count > 3)
                {
                    result.Add(maquina);
                }
                else if (numCapturas.Sum(x => x.HorasTrabajo) <= 60)
                {
                    result.Add(maquina);
                }


            }

            return result;
        }

        public List<int> grupoMaquinaria()
        {
            List<int> lista = new List<int>();

            lista.Add(161);
            lista.Add(225);
            lista.Add(217);

            return lista;
        }

        public tblM_CapStandBy getStandByID(int id)
        {
            var rest = _context.tblM_CapStandBy.FirstOrDefault(x => x.id.Equals(id));
            return rest;
        }

    }
}
