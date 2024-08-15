using Core.DAO.Maquinaria.Catalogos;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class CentroCostosDAO : GenericDAO<tblP_CC>, ICentroCostosDAO
    {
        public IList<tblM_CentroCostos> FillGridCC(tblM_CentroCostos cc)
        {
            var query_cc = new OdbcConsultaDTO();

            query_cc.consulta =
                @"SELECT
                    cc as cc,
                    descripcion as des,
                    corto as corto
                FROM
                    cc
                WHERE
                    descripcion LIKE '%" + cc.des + "%' AND cc LIKE '%" + cc.cc + "%'";
            if(vSesiones.sesionEmpresaActual==3)
            {
                query_cc.consulta =
                                @"SELECT
                    cc as cc,
                    descripcion as des,
                    corto as corto
                FROM
                    DBA.cc
                WHERE
                    descripcion LIKE '%" + cc.des + "%' AND cc LIKE '%" + cc.cc + "%'";
            }
            else if(vSesiones.sesionEmpresaActual==6)
            {

                var listaCc = _context.tblP_CC.Select(X => new tblM_CentroCostos
                {
                    des = X.cc + "-" + X.descripcion,
                    cc = X.cc
                });

                return listaCc.ToList();
            }
           
            return _contextEnkontrol.Select<tblM_CentroCostos>(vSesiones.sesionAmbienteEnkontrolAdm, query_cc);
            //return (IList<tblM_CentroCostos>)_contextEnkontrol.Where("SELECT cc as cc,descripcion as des,corto as corto FROM CC where descripcion like '%" + cc.des + "%' AND cc like '%" + cc.cc + "%'").ToObject<IList<tblM_CentroCostos>>();
        }

        public IList<InventarioDTO> fillGridMaquinaria(int cc, int idGrupo)
        {
            string grupo = idGrupo != 0 ? " AND B.tipo_activo= '" + idGrupo + "' " : "";
            string centro_costos = "SELECT A.descripcion AS Economico,b.Descripcion AS grupo,C.descripcion AS TipoMaq FROM si_area_cuenta A" +
                                   " INNER JOIN saf_activos_fijos B on A.descripcion = num_economico" +
                                   " INNER JOIN saf_tipos_activo C on C.tipo_activo = B.tipo_activo" +
                                   " WHERE centro_costo='" + cc + "' " + grupo + " AND cc_activo=1 AND id_estatus = 1";
            return (IList<InventarioDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<InventarioDTO>>(); ;
        }

        public string getNombreCC(int cc)
        {
            try
            {
                string nombre_CC = "";

                string centroCotos = cc.ToString().PadLeft(3, '0');
                if (cc.ToString() != "0" && !string.IsNullOrEmpty(cc.ToString()))
                {
                    List<string> lista = new List<string>();

                    string centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + centroCotos + "';";

                    var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();

                    nombre_CC = resultado.Select(x => x.descripcion).FirstOrDefault();
                }

                return nombre_CC;
            }
            catch (Exception)
            {

                return "";
            }

        }

        public string getNombreCcFromSIGOPLAN(string centroCosto)
        {
            var cc = "";
            try
            {
                cc = _context.tblP_CC.Where(x => x.areaCuenta.Equals(centroCosto)).FirstOrDefault().descripcion;
            }
            catch (Exception ex)
            {

            }
            return cc;
        }

        public string getNombreCC(string cc)
        {
            try
            {
                string nombre_CC = "";
                string centro_costos = "SELECT descripcion FROM cc WHERE cc = '" + cc + "';";
                var resultado = (IList<economicoDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<economicoDTO>>();
                nombre_CC = resultado.Select(x => x.descripcion).FirstOrDefault();

                return nombre_CC;
            }
            catch (Exception)
            {

                return "";
            }

        }
        public string getNombreCCFix(string centroCostos)
        {
            try
            {
                switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
                {
                    case EmpresaEnum.Peru:
                        {
                            var areaCuenta = _context.tblP_CC.FirstOrDefault(x => x.cc == centroCostos);
                            centroCostos = areaCuenta != null ? areaCuenta.areaCuenta : centroCostos;
                        }
                        break;
                }

                var res = "";
                if (centroCostos == "1015")
                    return "PATIO DE MAQUINARIA";
                if (centroCostos == "1010")
                    return "TALLER DE MAQUINARIA";
                if (centroCostos == "1018")
                    return "TALLER DE VIRTUAL OVERHAUL";


                try
                {
                    var resultado = _context.tblP_CC.FirstOrDefault(x=>x.areaCuenta.Equals(centroCostos) || x.cc.Equals(centroCostos));

                    res = resultado.descripcion ?? "";
                    return res;
                }
                catch 
                {

                }
                try
                {
                    var resultado = _context.tblP_CCRH.FirstOrDefault(x => x.areaCuenta.Equals(centroCostos) || x.cc.Equals(centroCostos));

                    res = resultado.descripcion ?? "";
                    return res;
                }
                catch 
                {
      
                }
                return res;
                //List<economicoDTO> result = new List<economicoDTO>();
                //try
                //{
                //    var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';", 1).ToObject<IList<economicoDTO>>();

                //    return resultado.FirstOrDefault().descripcion;
                //}
                //catch (Exception)
                //{

                //    return "";
                //}

                //try
                //{
                //    var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';", 2).ToObject<IList<economicoDTO>>();
                //    return resultado.FirstOrDefault().descripcion;
                //}

                //catch (Exception)
                //{

                //    return "";
                //}


            }
            catch (Exception)
            {

                return "";
            }

        }

        public string getNombreAreaCuent(string areaCuenta)
        {
            try
            {
                return _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == areaCuenta).descripcion;
            }
            catch (Exception)
            {

                return "";
            }
        }
        public string getNombreCCArrendadoraRH(string centroCostos)
        {
            try
            {
                if (centroCostos == "1015")
                    return "PATIO DE MAQUINARIA";
                if (centroCostos == "1010")
                    return "TALLER DE MAQUINARIA";
                //switch (vSesiones.sesionEmpresaActual)
                //{
                //    case (int)Empresa.Construplan:

                //    case (int)Empresa.Arrendadora:
                //        var res = (List<dynamic>)ContextEnKontrolNominaArrendadora.Where(string.Format(@"SELECT top 1 descripcion FROM si_area_cuenta WHERE  area = {0} AND cuenta = {1}", centroCostos.Split('-')[0], centroCostos.Split('-')[1]), vSesiones.sesionEmpresaActual).ToObject<List<dynamic>>();
                //        return res.Count == 0 ? string.Empty : (string)res[0].descripcion;
                //    default: return string.Empty;
                //}
                List<economicoDTO> result = new List<economicoDTO>();
                try
                {
                    //var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';", 1).ToObject<IList<economicoDTO>>();
                    var query_cc = new OdbcConsultaDTO();

                    query_cc.consulta =
                        @"SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "'";

                    var resultado = (IList<economicoDTO>)_contextEnkontrol.Select<economicoDTO>(EnkontrolAmbienteEnum.ProdCPLAN, query_cc);
                    result.AddRange(resultado);
                }
                catch (Exception)
                {


                }

                try
                {
                    //var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "';", 2).ToObject<IList<economicoDTO>>();
                    var query_cc = new OdbcConsultaDTO();

                    query_cc.consulta =
                        @"SELECT descripcion FROM cc WHERE cc = '" + centroCostos + "";

                    var resultado = (IList<economicoDTO>)_contextEnkontrol.Select<economicoDTO>(EnkontrolAmbienteEnum.ProdARREND, query_cc);
                    result.AddRange(resultado);
                }
                catch (Exception)
                {


                }


                return result.LastOrDefault().descripcion;

            }
            catch (Exception)
            {

                return "";
            }

        }
        public IList<InventarioDTO> fillListaMaquinaria(string grupo, string tipo, string modelo)
        {
            string centro_costos =
                "SELECT ROW_NUMBER()OVER( ORDER BY A.descripcion) as id,D.cc AS CCOrigen,A.descripcion AS Economico,b.Descripcion AS grupo,C.descripcion AS TipoMaq," +
                "B.num_serie AS Serie, B.Modelo AS Modelo, B.Marca AS Marca,D.Descripcion AS CC " +
                "FROM si_area_cuenta A " +
                                  "INNER JOIN saf_activos_fijos B ON A.descripcion = num_economico " +
                                  "INNER JOIN saf_tipos_activo C ON C.tipo_activo = B.tipo_activo " +
                                  "INNER JOIN CC D ON D.cc = B.cc_actual " +
                "WHERE b.Descripcion like '%" + grupo + "%' AND C.descripcion like '%" + tipo + "%' OR B.modelo like '%" + modelo + "%' " +
                " AND cc_activo=1 AND id_estatus = 1 " +
                "group by A.descripcion,b.Descripcion,C.descripcion,B.num_serie, B.Modelo, B.Marca,D.Descripcion,D.cc order by a.descripcion;";




            return (IList<InventarioDTO>)_contextEnkontrol.Where(centro_costos).ToObject<IList<InventarioDTO>>();
        }

        public List<ComboDTO> getListaCC()
        {

            //if (vSesiones.sesionEmpresaActual == 1)
            //    return (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc", 1).ToObject<List<ComboDTO>>();
            //else
            //    return (List<ComboDTO>)ContextArrendadora.Where("SELECT DISTINCT (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))) AS Value, (CAST(area  AS varchar(4))+'-'+CAST(cuenta AS varchar(4))+'-'+descripcion) AS Text, area, cuenta FROM si_area_cuenta ORDER BY area, cuenta").ToObject<List<ComboDTO>>();
           if(vSesiones.sesionEmpresaActual == 1 )
           {
           
                return _context.tblP_CC.Where(x => x.estatus).OrderBy(x => x.cc).ToList().Select(X => new ComboDTO
                {
                    Text = X.cc + "-" + X.descripcion,
                    Value = X.cc
                }).ToList();
            }
           //else
           //{
           //    return _context.tblP_CC.Where(x => x.estatus).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList().Select(X => new ComboDTO
           //    {
           //        Text = X.areaCuenta + "-" + X.descripcion,
           //        Value = X.areaCuenta
           //    }).ToList();

           //}
           else if (vSesiones.sesionEmpresaActual == 3)
           {
               return _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).OrderBy(x => x.cc).ToList().Select(X => new ComboDTO
               {
                   Text = X.cc + "-" + X.ccDescripcion,
                   Value = X.cc,
               }).ToList();
           }
           else if(vSesiones.sesionEmpresaActual == 6)
           {
               return _context.tblC_Nom_CatalogoCC.Where(x => x.estatus).OrderBy(x => x.cc).ToList().Select(X => new ComboDTO
               {
                   Text = X.cc + "-" + X.ccDescripcion,
                   Value = X.area+"-"+X.cuenta,
                   Prefijo = X.area + "-" + X.cuenta
               }).ToList();
           }
           else
           {
               return _context.tblP_CC.Where(x => x.estatus).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList().Select(X => new ComboDTO
               {
                   Text = X.areaCuenta + "-" + X.descripcion,
                   Value = X.areaCuenta
               }).ToList();
           }



        }
        public List<ComboDTO> getListaCC_Rep_Costos()
        {

            var ccs = _context.tblAP_Rep_Costos_Accesos.Where(x => x.usuarioID == vSesiones.sesionUsuarioDTO.id).Select(x=>x.cc).ToList();
            if (vSesiones.sesionEmpresaActual == 1)
            {
                var todo = ccs.Any(x => x.Equals("*"));
                if (todo)
                {
                    return _context.tblP_CC.Where(x => x.estatus).OrderBy(x => x.cc).ToList().Select(X => new ComboDTO
                    {
                        Text = X.cc + "-" + X.descripcion,
                        Value = X.cc
                    }).ToList();
                }
                else {
                    return _context.tblP_CC.Where(x => x.estatus && ccs.Contains(x.cc)).OrderBy(x => x.cc).ToList().Select(X => new ComboDTO
                    {
                        Text = X.cc + "-" + X.descripcion,
                        Value = X.cc
                    }).ToList();
                }
            }
            else
            {
                var todo = ccs.Any(x => x.Equals("*"));
                if (todo)
                {
                    return _context.tblP_CC.Where(x => x.estatus).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList().Select(X => new ComboDTO
                    {
                        Text = X.areaCuenta + "-" + X.descripcion,
                        Value = X.areaCuenta
                    }).ToList();
                }
                else {
                    return _context.tblP_CC.Where(x => x.estatus && ccs.Contains(x.areaCuenta)).OrderBy(x => x.area).ThenBy(x => x.cuenta).ToList().Select(X => new ComboDTO
                    {
                        Text = X.areaCuenta + "-" + X.descripcion,
                        Value = X.areaCuenta
                    }).ToList();
                }
            }

        }

        public List<ComboDTO> getListaCCSIGOPLAN()
        {
            if (vSesiones.sesionEmpresaActual == 1)
            {
                return (List<ComboDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc", 1).ToObject<List<ComboDTO>>();
            }
            else
            {
                return _context.tblP_CC.Select(X => new ComboDTO
                {
                    Text = X.areaCuenta + "-" + X.descripcion,
                    Value = X.areaCuenta,
                    Prefijo = X.areaCuenta
                }).ToList();
            }

        }

        public List<ComboDTO> getLstCcArrendadoraProd()
        {
            //if(vSesiones.sesionEmpresaActual==)
            //var result = _context.Select<ComboDTO>(new DapperDTO
            //{
            //    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
            //    consulta = @"SELECT descripcion as Text , cc as Value FROM tblP_CC",
            //    //consulta = @"SELECT ccDescripcion as Text , cc as Value FROM tblC_Nom_CatalogoCC",
            //});
            //return result;
            return _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, "SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc");

            //return _contextEnkontrol.Select<ComboDTO>(EnkontrolEnum.ArrenProd, "SELECT cc as Value,  (cc+'-' +descripcion) as Text FROM cc");
        }
        public List<ComboDTO> getListaCCConstruplan()
        {
            return _context.tblP_CC.Select(X => new ComboDTO
            {
                Text = X.areaCuenta + "-" + X.descripcion,
                Value = X.areaCuenta
            }).ToList();
        }

        public List<ComboDTO> ListCC()
        {
            if (vSesiones.sesionEmpresaActual == 3 || vSesiones.sesionEmpresaActual == 6)
            {
                var result = _context.Select<ComboDTO>(new DapperDTO
                {
                    baseDatos = (MainContextEnum)vSesiones.sesionEmpresaActual,
                    //consulta = @"SELECT descripcion as Text , cc as Value FROM tblP_CC",
                    consulta = @"SELECT cc +' '+ ccDescripcion as Text , cc as Value FROM tblC_Nom_CatalogoCC",
                });
                return result;
            }
            else
            {
                return _contextEnkontrol.Select<ComboDTO>(EnkontrolAmbienteEnum.Prod, string.Format("SELECT descripcion as Text , cc as Value FROM {0}cc;", vSesiones.sesionEmpresaDBPregijo));
            }
        }

        public void setCCAreaCuenta()
        {
            var lst = (List<tblP_CC>)ContextArrendadora.Where("SELECT DISTINCT (CAST(area  AS varchar(4)) + '-' + CAST(cuenta AS varchar(4))) AS areaCuenta, descripcion, area, cuenta FROM si_area_cuenta ORDER BY area, cuenta").ToObject<List<tblP_CC>>();
            lst.ForEach(c =>
            {
                c.cc = string.Empty;
                SaveEntity(c, (int)BitacoraEnum.CC);
            });
        }

        public tblP_CC getEntityCCConstruplan(int ccID)
        {
            return _context.tblP_CC.Where(x => x.id == ccID).FirstOrDefault();
        }
    }
}
