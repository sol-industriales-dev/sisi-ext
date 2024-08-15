using Core.DAO.Maquinaria.Inventario;
using Core.DTO.Maquinaria.Inventario;
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
    public class AutorizacionStandByDAO : GenericDAO<tblM_AutorizacionStandBy>, IAutorizacionStandByDAO
    {
        public void Guardar(tblM_AutorizacionStandBy obj)
        {
            if (true)
            {
                if (obj.id == 0)
                    SaveEntity(obj, (int)BitacoraEnum.SOLICITUDSTANDBY);
                else
                    Update(obj, obj.id, (int)BitacoraEnum.SOLICITUDSTANDBY);
            }
            else
            {
                throw new Exception("Se presento un inconveniente al hacer la ");
            }
        }

        public tblM_AutorizacionStandBy GetAutorizacion(int idAsignacion, int idEconomico)
        {
            return _context.tblM_AutorizacionStandBy.Where(x => x.idAsignacion.Equals(idAsignacion) && x.idEconomico.Equals(idEconomico)).OrderByDescending(x => x.id).FirstOrDefault();
        }
        public tblM_AutorizacionStandBy GetAutorizacionByID(int idAutorizacion)
        {
            return _context.tblM_AutorizacionStandBy.FirstOrDefault(x => x.id.Equals(idAutorizacion));
        }

        public tblM_AutorizaStandby GetAutorizacionByIDStanby(int idStandby)
        {
            return _context.tblM_AutorizaStandby.FirstOrDefault(x => x.standByID.Equals(idStandby));
        }

        private string Economico;

        public List<int> GetAutorizadoresStandby(string cc, DateTime inicio, DateTime fin)
        {
            List<int> datos = new List<int>();

            var res = _context.tblM_CapStandBy.FirstOrDefault(x => x.CC == cc && x.FechaInicio >= inicio && x.FechaFin <= fin);
            if (res != null)
            {
                datos.Add(res.UsuarioElabora);
                datos.Add(res.UsuarioGerente);
            }


            return datos;

        }

        public List<rptConciliacionDTO> GetReporte(string cc, DateTime inicio, DateTime fin)
        {
            var res = _context.tblM_CapStandBy.FirstOrDefault(x => x.CC == cc && x.FechaInicio >= inicio && x.FechaFin <= fin);
            if (res != null)
            {
                var Conciliacion = _context.tblM_DetStandby.Where(x => x.StandByID == res.id).ToList();


                List<rptConciliacionDTO> lista = Conciliacion.Select(x => new rptConciliacionDTO
                {
                    Economico = GetStringEconomico(x.noEconomicoID),
                    Descripcion = _context.tblM_CatMaquina.FirstOrDefault(h => h.id.Equals(x.noEconomicoID)).descripcion,
                    HorometroInicio = x.HorometroInicial.ToString(),
                    HorometroFinal = x.HorometroFinal.ToString(),
                    DiaParo = x.DiaParo.ToShortDateString(),
                    TipoConsideracion = GetTipo(x.TipoConsideracion)
                }).ToList();
                return lista;
            }
            else
            {
                return null;
            }


        }


        private string GetStringEconomico(int id)
        {
            Economico = _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(id)).noEconomico;
            return Economico;
        }

        private string GetHorometro(string Economico, int tipo, DateTime fecha)
        {
            var res = _context.tblM_CapHorometro.FirstOrDefault(x => x.Fecha <= fecha);

            if (res != null)
            {
                return res.Horometro.ToString();
            }
            else
            {
                return "";
            }


        }

        private string GetTipo(int c)
        {
            switch (c)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                default:
                    return "";
            }
        }
    }
}
