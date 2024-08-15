using Core.DTO;
using Core.DTO.Utils.Data;
using Core.Enum.Multiempresa;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Data.EntityFramework.Context
{
    public class _contextEnkontrol
    {
        public static dynamic Where(string consulta)
        {
            //var lst = Select<dynamic>(EnkontrolAmbienteEnum.Prod, consulta);
            //var res = new FakeListUtilities<dynamic>() { lst = lst };
            //return res;
            var c = new Conexion();
            string jsonResult = "";
            var con = c.Connect();
            DataSet dsEntity = new DataSet();
            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                    reader.Close();
                    command.Dispose();
                    c.Close(con);
                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);

            jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        }

        public static dynamic Where(string consulta, int bd)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectEKMerge(bd);

            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();
                DataSet dsEntity = new DataSet();
                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                    reader.Close();
                    command.Dispose();
                    c.Close(con);
                    return JsonConvert.DeserializeObject<dynamic>(jsonResult, settings);
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);
            return null;
        }
        public static List<dynamic> Where(List<string> lst)
        {
            var lstRes = new List<dynamic>();
            lst.ForEach(consulta =>
            {
                lstRes.Add(Select<dynamic>(EnkontrolAmbienteEnum.Prod, consulta));
            });
            return lstRes;
            var c = new Conexion();
            var dsEntity = new DataSet();
            var con = c.Connect();
            var jsonResult = "";
            var i = 0;
            if (con != null)
            {
                var command = con.CreateCommand();
                command.CommandTimeout = 300;
                lst.ForEach(consulta =>
                {
                    command.CommandText = consulta;
                    var reader = command.ExecuteReader();
                    dsEntity.Tables.Add("");
                    dsEntity.Tables[i].Load(reader);
                    if (dsEntity.Tables[i].Rows.Count > 0)
                    {
                        reader.Close();
                        jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[i]);
                        lstRes.Add(JsonConvert.DeserializeObject<dynamic>(jsonResult));
                    }
                    i++;
                });
                command.Dispose();
            }
            c.Close(con);
            return lstRes;
        }
        #region Auxiliares
        public static EnkontrolEnum BaseDatosProductivoDesdeEmpresa(EmpresaEnum empresa)
        {
            EnkontrolEnum baseDatos = 0;
            if (empresa == 0)
            {
                empresa = (EmpresaEnum)vSesiones.sesionEmpresaActual;
            }
            switch (empresa)
            {
                case EmpresaEnum.Construplan:
                    baseDatos = EnkontrolEnum.CplanProd;
                    break;
                case EmpresaEnum.Arrendadora:
                    baseDatos = EnkontrolEnum.ArrenProd;
                    break;
                case EmpresaEnum.Colombia:
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                    break;
                case EmpresaEnum.EICI:
                    baseDatos = EnkontrolEnum.CplanEici;
                    break;
            }
            return baseDatos;
        }
        public static EnkontrolEnum BaseDatosRecursosHumanosDesdeEmpresa(EmpresaEnum empresa)
        {
            EnkontrolEnum baseDatos = 0;
            if (empresa == 0)
            {
                empresa = (EmpresaEnum)vSesiones.sesionEmpresaActual;
            }
            switch (empresa)
            {
                case EmpresaEnum.Construplan:
                    baseDatos = EnkontrolEnum.CplanRh;
                    break;
                case EmpresaEnum.Arrendadora:
                    baseDatos = EnkontrolEnum.ArrenRh;
                    break;
                case EmpresaEnum.Colombia:
                    baseDatos = EnkontrolEnum.ColombiaProductivo;
                    break;
                case EmpresaEnum.EICI:
                    baseDatos = EnkontrolEnum.CplanEici;
                    break;
            }
            return baseDatos;
        }
        #endregion
        #region ContextSupport
        //DbType DbTypeDesdeOdbcType(OdbcType odbcTipo)
        //{
        //    switch(odbcTipo)
        //    {
        //        case OdbcType.BigInt:
        //            break;
        //        case OdbcType.Binary:
        //            break;
        //        case OdbcType.Bit:
        //            break;
        //        case OdbcType.Char:
        //            break;
        //        case OdbcType.Date:
        //            break;
        //        case OdbcType.DateTime:
        //            break;
        //        case OdbcType.Decimal:
        //            break;
        //        case OdbcType.Double:
        //            break;
        //        case OdbcType.Image:
        //            break;
        //        case OdbcType.Int:
        //            break;
        //        case OdbcType.NChar:
        //            break;
        //        case OdbcType.NText:
        //            break;
        //        case OdbcType.NVarChar:
        //            break;
        //        case OdbcType.Numeric:
        //            break;
        //        case OdbcType.Real:
        //            break;
        //        case OdbcType.SmallDateTime:
        //            break;
        //        case OdbcType.SmallInt:
        //            break;
        //        case OdbcType.Text:
        //            break;
        //        case OdbcType.Time:
        //            break;
        //        case OdbcType.Timestamp:
        //            break;
        //        case OdbcType.TinyInt:
        //            break;
        //        case OdbcType.UniqueIdentifier:
        //            break;
        //        case OdbcType.VarBinary:
        //            break;
        //        case OdbcType.VarChar:
        //            break;
        //        //default:
        //        //    break;
        //    }
        //}
        #endregion
        /// <summary>
        /// Inicializa conector
        /// </summary>
        /// <param name="ek">Base de datos</param>
        /// <returns>Conector iniciado</returns>
        protected static OdbcConnection conectar(EnkontrolEnum ek)
        {
            var conn = new OdbcConnection();
            switch (ek)
            {
                case EnkontrolEnum.CplanProd:
                    conn = new Conexion().ConnectCplanProductivo();
                    break;
                case EnkontrolEnum.ArrenProd:
                    conn = new Conexion().ConnectArrendarora();
                    break;
                case EnkontrolEnum.CplanRh:
                    conn = new Conexion().ConnectRHConstruplan();
                    break;
                case EnkontrolEnum.ArrenRh:
                    conn = new Conexion().ConnectRHArrendadora();
                    break;
                case EnkontrolEnum.PruebaCplanProd:
                    conn = new Conexion().ConnectPruebaCplanProd();
                    break;
                case EnkontrolEnum.PruebaRh:
                    conn = new Conexion().ConnectPruebaRH();
                    break;
                case EnkontrolEnum.CplanEici:
                    conn = new Conexion().ConnectCplanEici();
                    break;
                case EnkontrolEnum.ColombiaProductivo:
                    conn = new Conexion().ConnectColombiaProductivo();
                    break;
                case EnkontrolEnum.PruebaEici:
                    conn = new Conexion().ConnectPruebaEici();
                    break;
                case EnkontrolEnum.PruebaArrenADM:
                    conn = new Conexion().ConnectArrendaroraPrueba();
                    break;
                case EnkontrolEnum.CplanVirtual:
                    conn = new Conexion().ConnectCplanVirtual();
                    break;
                case EnkontrolEnum.CplanIntegradora:
                    conn = new Conexion().ConnectCplanIntegradora();
                    break;
                case EnkontrolEnum.ArrenVirtual:
                    conn = new Conexion().ConnectArrendadoraVirtual();
                    break;
                case EnkontrolEnum.GCPLAN:
                    conn = new Conexion().Connect();
                    break;
                default:
                    conn = new Conexion().ConnectConstruplan();
                    break;
            }
            return conn;
        }
        /// <summary>
        /// Inicializa conector
        /// </summary>
        /// <param name="ek">Ambiente de base de datos</param>
        /// <returns>Conector iniciado</returns>
        protected static OdbcConnection conectar(EnkontrolAmbienteEnum ek)
        {
            var conn = new OdbcConnection();
            switch (ek)
            {
                case EnkontrolAmbienteEnum.Prod:
                    conn = new Conexion().Connect();
                    break;
                case EnkontrolAmbienteEnum.Rh:
                    conn = new Conexion().ConnectRHMERGE(vSesiones.sesionEmpresaActual);
                    break;
                case EnkontrolAmbienteEnum.Prueba:
                    conn = new Conexion().ConnectPrueba();
                    break;
                case EnkontrolAmbienteEnum.PruebaRh:
                    conn = new Conexion().ConnectPruebaRH();
                    break;
                case EnkontrolAmbienteEnum.RhArre:
                    conn = new Conexion().ConnectRHMERGE(2);
                    break;
                case EnkontrolAmbienteEnum.Colombia:
                    conn = new Conexion().ConnectColombiaProductivo();
                    break;
                case EnkontrolAmbienteEnum.CpRhPruebas:
                    conn = new Conexion().ConnectCPPruebaRH();
                    break;
                case EnkontrolAmbienteEnum.RhCplan:
                    conn = new Conexion().ConnectRHMERGE(1);
                    break;
                case EnkontrolAmbienteEnum.ProdCPLAN:
                    conn = new Conexion().Connect(1);
                    break;
                case EnkontrolAmbienteEnum.ProdARREND:
                    conn = new Conexion().Connect(2);
                    break;
                case EnkontrolAmbienteEnum.PruebaCPLAN:
                    conn = new Conexion().ConnectPrueba();
                    break;
                case EnkontrolAmbienteEnum.PruebaARREND:
                    conn = new Conexion().ConnectPrueba();
                    break;
                case EnkontrolAmbienteEnum.ProdGCPLAN:
                    conn = new Conexion().Connect(8);
                    break;
                default:
                    conn = new Conexion().Connect();
                    break;
            }
            return conn;
        }
        /// <summary>
        /// Executa la consula de Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de base de datos</param>
        /// <param name="consulta">Query de la consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolEnum ek, string consulta) where T : new()
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(consulta, con))
            {
                return _contextEnkontrol.getTypedReader<T>(cmd);
            }
        }
        /// <summary>
        /// Executa la consula a Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de base de datos</param>
        /// <param name="odbc">DTO de consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolEnum ek, OdbcConsultaDTO odbc) where T : new()
        {
            if (vSesiones.sesionEmpresaActual == 3)
            {
                odbc.consulta = odbc.consulta.Replace("FROM ", "FROM DBA.");
                odbc.consulta = odbc.consulta.Replace("FROM DBA.DBA.", "FROM DBA.");
                odbc.consulta = odbc.consulta.Replace("JOIN ", "JOIN DBA.");
                odbc.consulta = odbc.consulta.Replace("JOIN DBA.DBA.", "JOIN DBA.");
                odbc.consulta = odbc.consulta.Replace("FROM DBA.(", "FROM (");
                odbc.consulta = odbc.consulta.Replace("DBA.(", "(");
            }

            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return _contextEnkontrol.getTypedReader<T>(cmd);
            }
        }
        public static IEnumerable<T> SelectNoList<T>(EnkontrolEnum ek, OdbcConsultaDTO odbc) where T : new()
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return _contextEnkontrol.getTypedReaderIEnumerable<T>(cmd);
            }
        }
        /// <summary>
        /// Executa la consula a Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de base de datos</param>
        /// <param name="lstOdbc">listado de consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolEnum ek, List<OdbcConsultaDTO> lstOdbc) where T : new()
        {
            var lstT = new List<T>();
            using (var con = conectar(ek))
            {
                for (int i = 0; i < lstOdbc.Count; i++)
                {
                    var odbc = lstOdbc[i];
                    using (var cmd = new OdbcCommand(odbc.consulta, con))
                    {
                        var parameters = cmd.Parameters;
                        odbc.parametros.ForEach(p =>
                        {
                            parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                        });
                        lstT.AddRange(_contextEnkontrol.getTypedReader<T>(cmd));
                    }
                }
            }
            return lstT;
        }
        /// <summary>
        /// Executa la consula de Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de ambiente</param>
        /// <param name="consulta">Query de la consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolAmbienteEnum ek, string consulta) where T : new()
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(consulta, con))
            {
                return _contextEnkontrol.getTypedReader<T>(cmd);
            }
        }
        public static List<T> Select<T>(EnkontrolAmbienteEnum ek, int empresa, string consulta) where T : new()
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(consulta, con))
            {
                return _contextEnkontrol.getTypedReader<T>(cmd);
            }
        }
        /// <summary>
        /// Executa la consula a Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de ambiente</param>
        /// <param name="odbc">DTO de consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolAmbienteEnum ek, OdbcConsultaDTO odbc) where T : new()
        {
            using (var con = conectar(ek))
            using (var cmd = new OdbcCommand(odbc.consulta, con))
            {
                var parameters = cmd.Parameters;
                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });
                return _contextEnkontrol.getTypedReader<T>(cmd);
            }
        }
        /// <summary>
        /// Executa la consula a Enkontrol Operativo
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="ek">Enum de ambiente</param>
        /// <param name="lstOdbc">listado de consulta</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> Select<T>(EnkontrolAmbienteEnum ek, List<OdbcConsultaDTO> lstOdbc) where T : new()
        {
            var lstT = new List<T>();
            using (var con = conectar(ek))
            {
                for (int i = 0; i < lstOdbc.Count; i++)
                {
                    var odbc = lstOdbc[i];
                    using (var cmd = new OdbcCommand(odbc.consulta, con))
                    {
                        var parameters = cmd.Parameters;
                        odbc.parametros.ForEach(p =>
                        {
                            parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                        });
                        lstT.AddRange(_contextEnkontrol.getTypedReader<T>(cmd));
                    }
                }
            }
            return lstT;
        }
        /// <summary>
        /// Lee los resultados de OdbcCommand
        /// </summary>
        /// <typeparam name="T">Tipo de consula</typeparam>
        /// <param name="cmd">Comando a leer</param>
        /// <returns>Resultado tipado</returns>
        public static List<T> getTypedReader<T>(OdbcCommand cmd) where T : new()
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                return dt.Parse<T>().ToList();
            }
        }
        public static IEnumerable<T> getTypedReaderIEnumerable<T>(OdbcCommand cmd) where T : new()
        {
            cmd.CommandTimeout = 6000;
            using (var dt = cmd.ExecuteReader())
            {
                return dt.Parse<T>();
            }
        }
        /// <summary>
        /// Ejecuta acciones CUD de ODBC
        /// </summary>
        /// <param name="ek">Enum de ambiente de base de datos</param>
        /// <param name="odbc">DTO de consulta</param>
        /// <returns>respuesta de ExecuteNonQuery</returns>
        public static int Save(EnkontrolAmbienteEnum ek, OdbcConsultaDTO odbc)
        {
            using (var trans = conectar(ek).BeginTransaction())
            using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
                try
                {
                    var parameters = cmd.Parameters;
                    cmd.Transaction = trans;
                    cmd.CommandTimeout = 400;
                    odbc.parametros.ForEach(p =>
                    {
                        parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                    });
                    var i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        cmd.Transaction.Commit();
                    }
                    else
                    {
                        cmd.Transaction.Rollback();
                    }
                    return i;
                }
                catch (Exception)
                {
                    cmd.Transaction.Rollback();
                    return 0;
                }
        }
        /// <summary>
        /// Ejecuta acciones CUD de ODBC
        /// </summary>
        /// <param name="ek">Enum de base de datos</param>
        /// <param name="odbc">DTO de consulta</param>
        /// <returns>respuesta de ExecuteNonQuery</returns>
        public static int Save(EnkontrolEnum ek, OdbcConsultaDTO odbc)
        {
            using (var trans = conectar(ek).BeginTransaction())
            using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
                try
                {
                    var parameters = cmd.Parameters;
                    odbc.parametros.ForEach(p =>
                    {
                        parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                    });
                    cmd.Connection = trans.Connection;
                    cmd.Transaction = trans;
                    var i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        cmd.Transaction.Commit();
                    }
                    else
                    {
                        cmd.Transaction.Rollback();
                    }
                    return i;
                }
                catch (Exception)
                {
                    cmd.Transaction.Rollback();
                    return 0;
                }
        }
        public static int SaveT(OdbcTransaction trans, OdbcConsultaDTO odbc)
        {
            using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
            {
                var parameters = cmd.Parameters;

                odbc.parametros.ForEach(p =>
                {
                    parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                });

                cmd.Connection = trans.Connection;
                cmd.Transaction = trans;

                var i = cmd.ExecuteNonQuery();

                return i;
            }
        }
        /// <summary>
        /// Ejecuta acciones CUD de ODBC
        /// </summary>
        /// <param name="ek">Enum de ambiente de base de datos</param>
        /// <param name="lst">List de consulta</param></param>
        /// <returns>respuesta de ExecuteNonQuery</returns>
        public static List<int> Save(EnkontrolAmbienteEnum ek, List<OdbcConsultaDTO> lst)
        {
            var res = new List<int>();
            using (var trans = conectar(ek).BeginTransaction())
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    var odbc = lst[i];
                    using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
                        try
                        {
                            var parameters = cmd.Parameters;
                            odbc.parametros.ForEach(p =>
                            {
                                parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                            });
                            cmd.Connection = trans.Connection;
                            cmd.Transaction = trans;
                            var j = cmd.ExecuteNonQuery();
                            res.Add(j);
                            if (j == 1)
                            {

                            }
                            else
                            {
                                cmd.Transaction.Rollback();
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            cmd.Transaction.Rollback();
                            res.Add(0);
                        }
                }
                if (lst.Count == res.Where(a => a == 1).Count())
                {
                    trans.Commit();
                }
            }
            return res;
        }
        public static List<int> Update(EnkontrolAmbienteEnum ek, List<OdbcConsultaDTO> lst)
        {
            var res = new List<int>();
            using (var trans = conectar(ek).BeginTransaction())
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    var odbc = lst[i];
                    using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
                        try
                        {
                            var parameters = cmd.Parameters;
                            odbc.parametros.ForEach(p =>
                            {
                                parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                            });
                            cmd.Connection = trans.Connection;
                            cmd.Transaction = trans;
                            var j = cmd.ExecuteNonQuery();
                            res.Add(j);

                        }
                        catch (Exception)
                        {
                            cmd.Transaction.Rollback();
                            res.Add(0);
                        }
                }
                trans.Commit();
            }
            return res;
        }
        /// <summary>
        /// Ejecuta acciones CUD de ODBC
        /// </summary>
        /// <param name="ek">Enum de base de datos</param>
        /// <param name="lst">List de consulta</param></param>
        /// <returns>respuesta de ExecuteNonQuery</returns>
        public static List<int> Save(EnkontrolEnum ek, List<OdbcConsultaDTO> lst)
        {
            var res = new List<int>();
            using (var trans = conectar(ek).BeginTransaction())
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    var odbc = lst[i];
                    using (var cmd = new OdbcCommand(odbc.consulta, trans.Connection))
                        try
                        {
                            var parameters = cmd.Parameters;
                            odbc.parametros.ForEach(p =>
                            {
                                parameters.Add("@" + p.nombre, p.tipo).Value = p.valor;
                            });
                            cmd.Connection = trans.Connection;
                            cmd.Transaction = trans;
                            var j = cmd.ExecuteNonQuery();
                            res.Add(j);
                            if (j == 1)
                            {

                            }
                            else
                            {
                                cmd.Transaction.Rollback();
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            cmd.Transaction.Rollback();
                            res.Add(0);
                            break;
                        }
                }
                if (lst.Count == res.Where(a => a == 1).Count())
                {
                    trans.Commit();
                }
            }
            return res;
        }
        //Función Where para el módulo de "EnKontrol Compras por Origen".
        public static dynamic WhereComprasOrigen(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.Connect();

            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();

                command.CommandTimeout = 300;

                if (vSesiones.sesionEmpresaActual == 3)
                {
                    consulta = consulta.Replace("FROM ", "FROM DBA.");
                    consulta = consulta.Replace("FROM DBA.DBA.", "FROM DBA.");
                    consulta = consulta.Replace("JOIN ", "JOIN DBA.");
                    consulta = consulta.Replace("JOIN DBA.DBA.", "JOIN DBA.");
                    consulta = consulta.Replace("FROM DBA.(", "FROM (");
                    consulta = consulta.Replace("DBA.(", "(");
                }

                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();
                DataSet dsEntity = new DataSet();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);

                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);

                    reader.Close();
                    command.Dispose();
                    c.Close(con);

                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }

            c.Close(con);

            return null;
        }

        public static dynamic WhereComprasOrigenConstruplan(string consulta)
        {
            var c = new Conexion();
            string jsonResult = "";
            var con = c.Connect((int)EmpresaEnum.Construplan);

            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();

                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();
                DataSet dsEntity = new DataSet();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);

                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);

                    reader.Close();
                    command.Dispose();
                    c.Close(con);

                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }

            c.Close(con);

            return null;
        }

        public static dynamic WherePruebaArrendadora(string consulta)
        {
            return Select<dynamic>(EnkontrolAmbienteEnum.Prod, consulta);
            var c = new Conexion();
            string jsonResult = "";
            var con = c.ConnectArrendaroraPrueba();
            DataSet dsEntity = new DataSet();
            if (con != null)
            {
                OdbcCommand command = con.CreateCommand();
                command.CommandTimeout = 300;
                command.CommandText = consulta;

                OdbcDataReader reader = command.ExecuteReader();

                dsEntity.Tables.Add("");
                dsEntity.Tables[0].Load(reader);
                if (dsEntity.Tables[0].Rows.Count > 0)
                {
                    jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
                    reader.Close();
                    command.Dispose();
                    c.Close(con);
                    return JsonConvert.DeserializeObject<dynamic>(jsonResult);
                }

                reader.Close();
                command.Dispose();
            }
            c.Close(con);

            jsonResult = JsonConvert.SerializeObject(dsEntity.Tables[0]);
            return JsonConvert.DeserializeObject<dynamic>(jsonResult);
        }
    }
}
