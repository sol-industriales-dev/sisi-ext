using Core.DAO.Enkontrol.General.CC;
using Core.DTO;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Multiempresa;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Principal.CC
{
    public class CCDAO_SP : ICCDAO
    {
        public List<ccDTO> GetCCs()
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblP_CC.Where(e => e.cc != "0").Select(x => new ccDTO
                {
                    cc = x.cc,
                    descripcion = x.descripcion.Trim(),
                    corto = x.abreviacion
                }).ToList();
                
            }
        }

        public List<ccDTO> GetCCs(List<string> ccs)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblP_CC
                    .Where(x =>
                        ccs.Contains(x.cc)
                    ).Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.descripcion.Trim(),
                        corto = x.abreviacion
                    }).ToList();
            }
        }

        public ccDTO GetCC(string cc)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblP_CC.Where(x => x.cc == cc).Select(x => new ccDTO
                {
                    cc = x.cc,
                    descripcion = x.descripcion.Trim(),
                    corto = x.abreviacion
                }).FirstOrDefault();
            }
        }

        #region NOMINAS
        public ccDTO GetCCNomina(string cc)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblC_Nom_CatalogoCC
                    .Where(x =>
                        x.estatus &&
                        x.cc == cc)
                    .Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).FirstOrDefault();
            }
        }

        public List<ccDTO> GetCCsNomina(bool? activos)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblC_Nom_CatalogoCC
                    .Where(x =>
                        x.estatus &&
                        (
                            activos != null ?
                                activos.Value ?
                                    (x.semanal || x.quincenal) :
                                    (x.semanal == false && x.quincenal == false) :
                                true
                        )
                    )
                    .Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).OrderBy(x => x.cc).ToList();
            }
        }

        public List<ccDTO> GetCCsNominaInactivos()
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblC_Nom_CatalogoCC
                    .Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).OrderBy(x => x.cc).ToList();
            }
        }

        public List<ccDTO> GetCCsNominaInactivos(List<string> ccs)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblC_Nom_CatalogoCC
                    .Where(e => ccs.Contains(e.cc))
                    .Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).OrderBy(x => x.cc).ToList();
            }
        }

        public List<ccDTO> GetCCsNominaFiltrados(List<string> ccs)
        {
            using (var _ctx = new MainContext())
            {
                return _ctx.tblC_Nom_CatalogoCC
                    .Where(x =>
                        ccs.Contains(x.cc))
                    .Select(x => new ccDTO
                    {
                        cc = x.cc,
                        descripcion = x.ccDescripcion
                    }).OrderBy(x => x.cc).ToList();
            }
        }
        #endregion
    }
}
