using Core.DAO.Contabilidad.Cheque;
using Core.DTO;
using Core.DTO.Administracion.Cheque;
using Core.DTO.Contabilidad.Bancos;
using Core.DTO.Contabilidad.Cheque;
using Core.DTO.Enkontrol.OrdenCompra;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Administrativo.Contabilidad.Cheque;
using Core.Entity.Administrativo.Contabilidad.Cheques;
using Core.Enum.Multiempresa;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.Contabilidad.Cheque
{
    public class CapChequeDAO : GenericDAO<tblC_sb_cheques>, ICapChequeDAO
    {
        #region variables y constructor
        private readonly Dictionary<string, object> Resultado = new Dictionary<string, object>();
        private const string NombreControlador = "Cheque";
        private EnkontrolEnum conexionEK = getConexion();


        /// <summary> 
        /// Constructor
        /// </summary>
        private static EnkontrolEnum getConexion()
        {
            int tipoAmbiente = vSesiones.sesionEmpresaActual;
            // tipoAmbiente = 3;
            switch (tipoAmbiente)
            {
                case 1:
                    return EnkontrolEnum.CplanProd;
                case 2:
                    return EnkontrolEnum.ArrenProd;
                case 3:
                    return EnkontrolEnum.PruebaCplanProd;
                default:
                    return 0;
            }
        }
        #endregion

        #region Consultas en ENKONTROL
        public dynamic GetListaProveedores(string term, bool porDesc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @" SELECT TOP 12  numpro as id, nombre as label
                    FROM sp_proveedores
                    WHERE" + (porDesc ? @" label " : @" id ") + @"LIKE ? ORDER BY id";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "label",
                    tipo = OdbcType.VarChar,
                    valor = (string)"%" + term.Trim() + "%"
                });
                List<dynamic> listaProveedores = _contextEnkontrol.Select<dynamic>(conexionEK, odbc);

                if (porDesc)
                {
                    return listaProveedores.Select(x => new
                     {
                         id = (string)x.id,
                         value = (string)x.label
                     }).ToList();
                }
                else
                {
                    return listaProveedores.Select(x => new
                     {
                         id = (string)x.label,
                         value = (string)x.id
                     }).ToList();
                }

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;

        }
        public Dictionary<string, object> GetListaCuentasInit()
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT ROW_NUMBER() OVER(ORDER BY cta ASC) AS numRow, cta,scta,sscta,descripcion,digito, requiere_oc 
                                  FROM catcta ";

                List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(conexionEK, odbc);
                Resultado.Add("listaCta", listaCuentas.ToList());

                Resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> ComboTipoMovimientos() { return Resultado; }
        public Dictionary<string, object> ComboSubTipoMovimiento() { return Resultado; }
        public Dictionary<string, object> GetMovPol(int poliza, int m, int a) { return Resultado; }
        public Dictionary<string, object> GetBeneficiario(string term) { return Resultado; }
        public Dictionary<string, object> GetInfoBanco(int banco) { return Resultado; }
        #endregion
        public Dictionary<string, object> GuardarCheque(tblC_sb_cheques objCheque, int ocID, List<tblC_sc_movpol> listaMovpol, int tipoCheque)
        {
            objCheque.anticipo = true;
            var objOC = _context.tblCom_OrdenCompra.FirstOrDefault(r => r.id == ocID);
            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT tp as Value ,descripcion as Text
                                  FROM tipos_poliza
                                  WHERE Value = ?";
            odbc.parametros.Add(new OdbcParameterDTO()
            {
                nombre = "Value",
                tipo = OdbcType.VarChar,
                valor = objCheque.itp,
            });
            List<ComboDTO> objtipos_poliza = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);

            var odbc2 = new OdbcConsultaDTO();
            odbc2.consulta = @"SELECT  A.clave AS Value,A.descripcion AS Text
                                  FROM sb_tm A";
            List<ComboDTO> listaTM = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc2);

            string concepto = objtipos_poliza.Count > 0 ? objtipos_poliza.FirstOrDefault().Text : "";

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    if (objCheque.id == 0)
                    {
                        if (_context.tblC_sb_cheques.Any(x => x.numero == objCheque.numero))
                        {
                            Resultado.Add(SUCCESS, false);
                            Resultado.Add(MESSAGE, "Ya se encuentra un cheque con el mismo número.");
                            return Resultado;
                        }
                        objCheque.estatusCheque = 0;//
                        _context.tblC_sb_cheques.Add(objCheque);
                        _context.SaveChanges();

                        tblC_sc_polizas objPoliza = new tblC_sc_polizas();
                        objPoliza.year = objCheque.iyear;
                        objPoliza.abonos = objCheque.monto;
                        objPoliza.cargos = 0;
                        objPoliza.concepto = concepto;
                        objPoliza.error = null;
                        objPoliza.fec_hora_movto = DateTime.UtcNow;
                        objPoliza.fecha_hora_crea = DateTime.UtcNow;
                        objPoliza.fechapol = objCheque.fecha_mov;
                        objPoliza.generada = "B";
                        objPoliza.mes = objCheque.imes;
                        objPoliza.poliza = objCheque.ipoliza;
                        objPoliza.socio_inversionista = 0;
                        objPoliza.status = "C";
                        objPoliza.status_carga_pol = "S";
                        objPoliza.status_lock = "L";
                        objPoliza.tp = objCheque.itp;
                        objPoliza.usuario_crea = "1";
                        objPoliza.usuario_movto = 1;

                        _context.tblC_sc_polizas.Add(objPoliza);
                        _context.SaveChanges();
                        var cuenta = getInfoCuentas(objPoliza.tp);

                        foreach (var objMovPol in listaMovpol)
                        {

                            objMovPol.year = objPoliza.year;
                            objMovPol.mes = objPoliza.mes;
                            objMovPol.poliza = objPoliza.poliza;
                            objMovPol.tp = objCheque.itp;
                            objMovPol.forma_pago = "CHEQUE";
                            objMovPol.itm = (int)objCheque.tm;

                            _context.tblC_sc_movpol.Add(objMovPol);
                        }
                        _context.SaveChanges();

                        var polizas = _context.tblC_sc_polizas.Where(x => x.poliza == objCheque.ipoliza).ToList();
                        var Cheque = _context.tblC_sb_cheques.FirstOrDefault(c => c.ipoliza == objCheque.ipoliza);
                        var movPol = _context.tblC_sc_movpol.Where(r => r.poliza == Cheque.ipoliza && r.year == Cheque.iyear && r.mes == Cheque.imes);
                        var bancosCuentas = getInfoCuentas(Cheque.itp);
                        var tm = listaTM.FirstOrDefault(r => r.Value == objCheque.tm.ToString());

                        using (var con = checkConexionProductivo())
                        {
                            using (var trans = con.BeginTransaction())
                            {
                                try
                                {
                                    var count = 0;

                                    var insertCheques = @"INSERT INTO sb_cheques 
                                                        (cuenta
                                                        ,fecha_mov
                                                        ,tm
                                                        ,numero
                                                        ,tipocheque
                                                        ,descripcion
                                                        ,cc
                                                        ,monto
                                                        ,hecha_por
                                                        ,status_bco
                                                        ,status_lp
                                                        ,num_pro_emp
                                                        ,cpto1
                                                        ,cpto2
                                                        ,cpto3
                                                        ,iyear
                                                        ,imes
                                                        ,ipoliza
                                                        ,itp
                                                        ,ilinea
                                                        ,tp
                                                        ,fecha_reten
                                                        ,status_transf_cash
                                                        ,id_empleado_firma
                                                        ,id_empleado_firma2
                                                        ,fecha_reten_fin
                                                        ,firma1
                                                        ,fecha_firma1
                                                        ,firma2
                                                        ,fecha_firma2
                                                        ,firma3
                                                        ,fecha_firma3
                                                        ,clave_sub_tm
                                                        ,ruta_comprobantebco_pdf
                                                        )
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                    using (var cmd = new OdbcCommand(insertCheques))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();
                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = objCheque.cuenta;
                                        parameters.Add("@fecha_mov", OdbcType.Date).Value = objCheque.fecha_mov;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = objCheque.tm;
                                        parameters.Add("@numero", OdbcType.Numeric).Value = objCheque.numero;
                                        parameters.Add("@tipocheque", OdbcType.Char).Value = DBNull.Value;
                                        parameters.Add("@descripcion", OdbcType.Char).Value = objCheque.descripcion;
                                        parameters.Add("@cc", OdbcType.Char).Value = objCheque.cc;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = objCheque.monto;
                                        parameters.Add("@hecha_por", OdbcType.Char).Value = objCheque.hecha_por;
                                        parameters.Add("@status_bco", OdbcType.Char).Value = objCheque.status_bco ?? string.Empty;
                                        parameters.Add("@status_lp", OdbcType.Char).Value = objCheque.status_lp;
                                        parameters.Add("@num_pro_emp", OdbcType.Numeric).Value = 0;// objCheque.num_pro_emp;
                                        parameters.Add("@cpto1", OdbcType.Char).Value = objCheque.cpto1;
                                        parameters.Add("@cpto2", OdbcType.Char).Value = DBNull.Value;
                                        parameters.Add("@cpto3", OdbcType.Char).Value = DBNull.Value;
                                        parameters.Add("@iyear", OdbcType.Numeric).Value = objCheque.iyear;
                                        parameters.Add("@imes", OdbcType.Numeric).Value = objCheque.imes;
                                        parameters.Add("@ipoliza", OdbcType.Numeric).Value = objCheque.ipoliza;
                                        parameters.Add("@itp", OdbcType.Char).Value = objCheque.itp;
                                        parameters.Add("@ilinea", OdbcType.Numeric).Value = objCheque.ilinea;
                                        parameters.Add("@tp", OdbcType.Char).Value = objCheque.tp ?? objCheque.itp;
                                        parameters.Add("@fecha_reten", OdbcType.Date).Value = DBNull.Value;
                                        parameters.Add("@status_transf_cash", OdbcType.Char).Value = 'N';
                                        parameters.Add("@id_empleado_firma", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@id_empleado_firma2", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@fecha_reten_fin", OdbcType.Date).Value = DBNull.Value;
                                        parameters.Add("@firma1", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@fecha_firma1", OdbcType.Date).Value = DBNull.Value;
                                        parameters.Add("@firma2", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@fecha_firma2", OdbcType.Date).Value = DBNull.Value;
                                        parameters.Add("@firma3", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@fecha_firma3", OdbcType.Date).Value = DBNull.Value;
                                        parameters.Add("@clave_sub_tm", OdbcType.Numeric).Value = DBNull.Value;
                                        parameters.Add("@ruta_comprobantebco_pdf", OdbcType.VarChar).Value = DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }

                                    var insertPoliza = "INSERT INTO sc_polizas (year ,mes ,poliza ,tp ,fechapol ,cargos ,abonos ,generada ,status ,status_lock ,fec_hora_movto ,usuario_movto ,fecha_hora_crea ,usuario_crea ,concepto ,error) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                                    using (var cmd = new OdbcCommand(insertPoliza))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();

                                        decimal cargos = 0;
                                        decimal abonos = 0;

                                        foreach (var item in listaMovpol)
                                        {
                                            if (item.tm == 1)
                                            {
                                                cargos += item.monto;
                                            }
                                            if (item.tm == 2)
                                            {
                                                decimal montoT = 0;
                                                if (item.monto > 0)
                                                    montoT = item.monto * (-1);
                                                else
                                                    montoT = item.monto;
                                                abonos += montoT;
                                            }
                                        }

                                        parameters.Add("@year", OdbcType.Numeric).Value = objPoliza.year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = objPoliza.mes;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = objPoliza.poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = objPoliza.tp;
                                        parameters.Add("@fechapol", OdbcType.Date).Value = objPoliza.fechapol;
                                        parameters.Add("@cargos", OdbcType.Numeric).Value = cargos;
                                        parameters.Add("@abonos", OdbcType.Numeric).Value = abonos;
                                        parameters.Add("@generada", OdbcType.Char).Value = objPoliza.generada;
                                        parameters.Add("@status", OdbcType.Char).Value = objPoliza.status ?? string.Empty;
                                        parameters.Add("@status_lock", OdbcType.Char).Value = 'N';
                                        parameters.Add("@fec_hora_movto", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_movto", OdbcType.Char).Value = 'C';
                                        parameters.Add("@fecha_hora_crea", OdbcType.DateTime).Value = DateTime.Now;
                                        parameters.Add("@usuario_crea", OdbcType.Char).Value = '1';
                                        parameters.Add("@concepto", OdbcType.VarChar).Value = objPoliza.concepto;
                                        parameters.Add("@error", OdbcType.VarChar).Value = string.Empty;
                                        parameters.Add("@status_carga_pol", OdbcType.VarChar).Value = DBNull.Value;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }

                                    foreach (var objMovPol1 in listaMovpol)
                                    {
                                        var insertMovpol = @"INSERT INTO sc_movpol 
                                                        (year
                                                        ,mes
                                                        ,poliza
                                                        ,tp
                                                        ,linea
                                                        ,cta
                                                        ,scta
                                                        ,sscta
                                                        ,digito
                                                        ,tm
                                                        ,referencia
                                                        ,cc
                                                        ,concepto
                                                        ,monto
                                                        ,iclave
                                                        ,itm
                                                        ,st_par
                                                        ,orden_compra
                                                        ,numpro
                                                        ,area
                                                        ,cuenta_oc)
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                        using (var cmd = new OdbcCommand(insertMovpol))
                                        {
                                            OdbcParameterCollection parameters = cmd.Parameters;
                                            parameters.Clear();
                                            decimal monto = 0;
                                            if (objMovPol1.tm == 1)
                                                monto = objMovPol1.monto;
                                            else
                                            {

                                                if (objMovPol1.monto > 0)
                                                    monto = objMovPol1.monto * (-1);
                                                else
                                                    monto = objMovPol1.monto;
                                            }

                                            parameters.Add("@year", OdbcType.Numeric).Value = objMovPol1.year;
                                            parameters.Add("@mes", OdbcType.Numeric).Value = objMovPol1.mes;
                                            parameters.Add("@poliza", OdbcType.Numeric).Value = objPoliza.poliza;
                                            parameters.Add("@tp", OdbcType.Char).Value = objCheque.itp;
                                            parameters.Add("@linea", OdbcType.Numeric).Value = objMovPol1.linea;
                                            parameters.Add("@cta", OdbcType.Numeric).Value = objMovPol1.cta;
                                            parameters.Add("@scta", OdbcType.Numeric).Value = objMovPol1.scta;
                                            parameters.Add("@sscta", OdbcType.Numeric).Value = objMovPol1.sscta;
                                            parameters.Add("@digito", OdbcType.Numeric).Value = objMovPol1.digito;
                                            parameters.Add("@tm", OdbcType.Numeric).Value = objMovPol1.tm;
                                            parameters.Add("@referencia", OdbcType.Char).Value = objMovPol1.referencia;
                                            parameters.Add("@cc", OdbcType.Char).Value = objMovPol1.cc;
                                            parameters.Add("@concepto", OdbcType.Char).Value = objMovPol1.concepto;
                                            parameters.Add("@monto", OdbcType.Numeric).Value = monto;
                                            parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                            parameters.Add("@itm", OdbcType.Numeric).Value = objMovPol1.tm;
                                            parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                            parameters.Add("@orden_compra", OdbcType.Numeric).Value = objOC != null ? objOC.numero : 0;
                                            parameters.Add("@numpro", OdbcType.Numeric).Value = objMovPol1.numpro;
                                            parameters.Add("@area", OdbcType.Numeric).Value = objMovPol1.area;
                                            parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = objMovPol1.cuenta_oc;
                                            cmd.Connection = trans.Connection;
                                            cmd.Transaction = trans;
                                            count += cmd.ExecuteNonQuery();
                                        }
                                    }

                                    var insertQuery = "UPDATE sb_cuenta SET ultimo_cheque = ? WHERE cuenta = ? AND tp = ?";
                                    using (var cmd = new OdbcCommand(insertQuery))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();


                                        if (tipoCheque == 1)
                                            parameters.Add("@ultimo_cheque", OdbcType.Numeric).Value = objCheque.numero;
                                        else
                                            parameters.Add("@ult_cheq_electronico", OdbcType.Numeric).Value = objCheque.numero;

                                        parameters.Add("@cuenta", OdbcType.Numeric).Value = objCheque.cuenta;
                                        parameters.Add("@tp", OdbcType.Char).Value = objCheque.itp;

                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                    dbTransaction.Commit();
                                    trans.Commit();
                                }
                                catch (Exception e)
                                {
                                    trans.Rollback();
                                    dbTransaction.Rollback();
                                    Resultado.Add(SUCCESS, false);
                                    Resultado.Add(MESSAGE, "Ocurrió un error el cheque al insertar en enkontrol.");
                                    return Resultado;
                                }
                            }
                        }
                        Resultado.Add("bancosCuentas", bancosCuentas);
                        Resultado.Add("polizas", polizas);
                        Resultado.Add("listaPolizas", movPol);
                        Resultado.Add("cheque", Cheque);
                        Resultado.Add("fechaCheque", Cheque.fecha_mov.ToShortDateString());
                        Resultado.Add("tm", tm.Text);

                    }
                    else
                    {
                        var actualizacion = _context.tblC_sb_cheques.FirstOrDefault(x => x.id == objCheque.numero);
                        actualizacion.descripcion = objCheque.descripcion;
                        actualizacion.monto = objCheque.monto;
                        actualizacion.tm = objCheque.tm;
                        _context.SaveChanges();
                    }

                    _context.SaveChanges();
                    Resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();

                    LogError(0, 0, NombreControlador, "GuardarBanco", e, AccionEnum.ACTUALIZAR, 0, objCheque);
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "Ocurrió un error el cheque.");
                }
                return Resultado;
            }
        }
        public Dictionary<string, object> ObtenerDatosPoliza(tblC_sb_cheques objCheque, int ocID)
        {

            try
            {
                if (_context.tblC_sb_cheques.Any(x => x.numero == objCheque.numero))
                {
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "Ya se encuentra un cheque con el mismo número.");
                    return Resultado;
                }
                var objOC = _context.tblCom_OrdenCompra.FirstOrDefault(r => r.id == ocID);
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT tp as Value ,descripcion as Text
                                  FROM tipos_poliza
                                  WHERE Value = ?";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "Value",
                    tipo = OdbcType.VarChar,
                    valor = objCheque.itp,
                });
                List<ComboDTO> objtipos_poliza = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);

                var odbc2 = new OdbcConsultaDTO();
                odbc2.consulta = @"SELECT  A.clave AS Value,A.descripcion AS Text
                                  FROM sb_tm A";
                List<ComboDTO> listaTM = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc2);

                string concepto = objtipos_poliza.Count > 0 ? objtipos_poliza.FirstOrDefault().Text : "";

                tblC_sc_polizas objPoliza = new tblC_sc_polizas();
                objPoliza.year = objCheque.iyear;
                objPoliza.abonos = objCheque.monto;
                objPoliza.cargos = 0;
                objPoliza.concepto = concepto;
                objPoliza.error = null;
                objPoliza.fec_hora_movto = DateTime.UtcNow;
                objPoliza.fecha_hora_crea = DateTime.UtcNow;
                objPoliza.fechapol = objCheque.fecha_mov;
                objPoliza.generada = "B";
                objPoliza.mes = objCheque.imes;
                objPoliza.poliza = objCheque.ipoliza;
                objPoliza.socio_inversionista = 0;
                objPoliza.status = "C";
                objPoliza.status_carga_pol = "S";
                objPoliza.status_lock = "L";
                objPoliza.tp = objCheque.itp;
                objPoliza.usuario_crea = "1";
                objPoliza.usuario_movto = 1;
                List<tblC_sc_polizas> polizas = new List<tblC_sc_polizas>();
                polizas.Add(objPoliza);
                var cuenta = getInfoCuentas(objPoliza.tp);

                tblC_sc_movpol objMovPol = new tblC_sc_movpol();
                objMovPol.year = objPoliza.year;
                objMovPol.mes = objPoliza.mes;
                objMovPol.poliza = objPoliza.poliza;
                objMovPol.tp = objCheque.itp;
                objMovPol.linea = 1;
                objMovPol.cta = cuenta.cta;
                objMovPol.scta = cuenta.scta;
                objMovPol.sscta = cuenta.sscta;
                objMovPol.digito = cuenta.d;
                objMovPol.tm = 2;
                objMovPol.referencia = objPoliza.poliza.ToString();
                objMovPol.cc = objCheque.cc;
                objMovPol.concepto = objCheque.cpto1;
                objMovPol.monto = -objCheque.monto;
                objMovPol.iclave = 0;
                objMovPol.itm = (int)objCheque.tm;
                objMovPol.orden_compra = ocID; //objOC == null ? 0 : objOC.numero;
                objMovPol.forma_pago = "CHEQUE";


                List<tblC_sc_movpol> movPol = new List<tblC_sc_movpol>();
                movPol.Add(objMovPol);
                var bancosCuentas = getInfoCuentas(objCheque.itp);
                var tm = listaTM.FirstOrDefault(r => r.Value == objCheque.tm.ToString());
                Resultado.Add("bancosCuentas", bancosCuentas);
                Resultado.Add("polizas", polizas);
                Resultado.Add("listaPolizas", movPol);
                Resultado.Add("cheque", objCheque);
                Resultado.Add("fechaCheque", objCheque.fecha_mov.ToShortDateString());
                Resultado.Add("tm", tm.Text);
                Resultado.Add(SUCCESS, true);

            }
            catch (Exception)
            {
                Resultado.Add(MESSAGE, "No se pudo consultar la información requerida para continuar con el proceso de cargado de poliza");
                Resultado.Add(SUCCESS, false);

            }
            return Resultado;
        }
        private CapCheques_CuentaDTO getInfoCuentas(string tp)
        {

            var odbc = new OdbcConsultaDTO();
            odbc.consulta = @"SELECT  A.cuenta,A.descripcion, A.banco,A.moneda,a.cta,a.tp,b.sucursal,a.ultimo_cheque,B.descripcion as bancoDescripcion, a.sscta , c.digito AS d, a.scta
                                  FROM sb_cuenta A
                                  INNER JOIN sb_bancos B ON B.banco = A.banco 
                                  INNER JOIN catcta C ON C.cta = A.cta AND C.scta = A.scta AND C.sscta = A.sscta 
                                  WHERE A.tp = ?";
            odbc.parametros.Add(new OdbcParameterDTO()
            {
                nombre = "tp",
                tipo = OdbcType.VarChar,
                valor = tp
            });
            List<CapCheques_CuentaDTO> objCuenta = _contextEnkontrol.Select<CapCheques_CuentaDTO>(conexionEK, odbc);

            return objCuenta.FirstOrDefault();
        }
        public Dictionary<string, object> GetListaCheques(filtroBusquedaDTO objFiltros) { return Resultado; }
        public Dictionary<string, object> SetUltimoCheque(int cuenta)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @" SELECT TOP 1  * 
                    FROM sb_cuenta
                    WHERE cuenta = ?";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "cuenta",
                    tipo = OdbcType.Numeric,
                    valor = (int)cuenta
                });
                List<dynamic> objCuenta = _contextEnkontrol.Select<dynamic>(conexionEK, odbc);

                Resultado.Add("infocuenta", objCuenta[0]);
                Resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        private dynamic GetServer(string consulta)
        {
            var dataResult = _contextEnkontrolPrueba.Where(consulta);
            return dataResult;
        }
        public Dictionary<string, object> GetInfoCheque(int cuenta)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();

                odbc.consulta = @"SELECT  A.cuenta,A.descripcion, A.banco,A.moneda,a.cta,a.tp,b.sucursal,a.ultimo_cheque,B.descripcion as bancoDescripcion,ult_cheq_electronico
                                  FROM sb_cuenta A
                                  INNER JOIN sb_bancos B ON B.banco = A.banco 
                                  WHERE cuenta = ?";

                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "cuenta",
                    tipo = OdbcType.Numeric,
                    valor = (int)cuenta
                });
                List<CapCheques_CuentaDTO> objCuenta = _contextEnkontrol.Select<CapCheques_CuentaDTO>(conexionEK, odbc);
                if (objCuenta.Count > 0)
                {
                    Resultado.Add("infocuenta", objCuenta[0]);
                    Resultado.Add(SUCCESS, true);
                }
                else
                {
                    Resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> GetTipoMovimientos()
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT  A.clave AS Value,A.descripcion AS Text
                                  FROM sb_tm A";
                List<ComboDTO> listaTM = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);
                Resultado.Add(ITEMS, listaTM.Select(r => new { id = r.Value, text = r.Value + '-' + r.Text }));
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> GetCuentasBanco()
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT  A.cuenta AS Value,A.descripcion AS Text
                                  FROM sb_cuenta A";
                List<ComboDTO> listaTM = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);
                Resultado.Add("listaObj", listaTM.Select(r => new { r.Value, Text = (r.Value.ToString() + "-" + r.Text) }));
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> GetProveedores()
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT  A.numpro AS Value,A.nombre AS Text
                                  FROM sp_proveedores A";
                List<ComboDTO> listaProveedores = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);
                Resultado.Add("listaObj", listaProveedores);
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> GetPolizasCheque(int iPoliza, string fecha)
        {
            try
            {
                var arrayFecha = fecha.Split('/');
                int mes = Convert.ToInt32(arrayFecha[1]);
                int year = Convert.ToInt32(arrayFecha[2]);
                var infoCheque = _context.tblC_sb_cheques.FirstOrDefault(r => r.numero == iPoliza && r.iyear == year && r.imes == mes);
                var infoMovPol = _context.tblC_sc_movpol.Where(r => r.poliza == infoCheque.ipoliza && r.year == year && r.mes == mes);
                var infoPoliza = _context.tblC_sc_polizas.FirstOrDefault(r => r.poliza == infoCheque.ipoliza && r.year == year && r.mes == mes);

                Resultado.Add("cheque", infoCheque);
                Resultado.Add("listaPolizas", infoMovPol);
                Resultado.Add("poliza", infoPoliza);
                Resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al cargar la informacion de la poliza y cheque.");
            }
            return Resultado;
        }
        public PrintInfoChequeDTO GetInfoCheque(int iPoliza, int mes, int year)
        {

            PrintInfoChequeDTO result = new PrintInfoChequeDTO();

            var objcheque = _context.tblC_sb_cheques.Where(r => r.ipoliza == iPoliza && r.imes == mes && r.iyear == year).FirstOrDefault();
            var objPolizas = _context.tblC_sc_polizas.Where(r => r.poliza == iPoliza && r.mes == mes && r.year == year).ToList();
            var objMovPolizas = _context.tblC_sc_movpol.Where(r => r.poliza == iPoliza && r.mes == mes && r.year == year).ToList();
            result.cheques = objcheque;
            result.polizas = objPolizas;
            result.movPolizas = objMovPolizas;
            return result;
        }
        public Dictionary<string, object> GetPolizas(int iPoliza)
        {
            try
            {
                var listaPolizas = _context.tblC_sc_polizas.Where(x => x.poliza == iPoliza).ToList();
                Resultado.Add("listaPolizas", listaPolizas);
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public dynamic GetListaCuentas(string term, bool porDesc)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @" SELECT TOP 12  cuenta as id, descripcion as label
                    FROM sb_cuenta
                    WHERE" + (porDesc ? @" label " : @" id ") + @"LIKE ? ORDER BY id";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "label",
                    tipo = OdbcType.VarChar,
                    valor = (string)"%" + term.Trim() + "%"
                });
                List<dynamic> listaCuentas = _contextEnkontrol.Select<dynamic>(conexionEK, odbc);

                if (porDesc)
                {
                    return listaCuentas.Select(x => new
                    {
                        id = (string)x.id,
                        value = (string)x.label
                    }).ToList();
                }
                else
                {
                    return listaCuentas.Select(x => new
                    {
                        id = (string)x.label,
                        value = (string)x.id
                    }).ToList();
                }

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, Ocurrio un error al momento de hacer la carga de Cuentas desde enkoltrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> GetOrdenCompraAnticipo()
        {
            try
            {

                string ocs = "";
                string cc = "";
                string numpro = "";


                var listaCheques = _context.tblC_sc_movpol.Select(r => r.orden_compra).ToList();
                var listOC = _context.tblCom_OrdenCompra.Where(r => r.anticipo && !listaCheques.Contains(r.numero)).ToList();


                listOC.ForEach(r =>
              {
                  cc += "'" + r.cc + "',";
                  ocs += "'" + r.numero + "',";
                  numpro += "'" + r.proveedor + "',";
              });
                var odbcProv = new OdbcConsultaDTO();
                odbcProv.consulta = @"SELECT nombre AS Text, numpro  AS Value FROM sp_proveedores
                                    WHERE numpro IN(" + numpro.TrimEnd(',') + ")";
                List<ComboDTO> proveedores = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbcProv);
                if (!string.IsNullOrEmpty(cc))
                {

                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = @"SELECT  A.numpro AS Value,A.cc AS Text, referenciaoc AS Prefijo
                                  FROM sp_gastos_prov A WHERE referenciaoc IN (" + ocs.Trim(',') + ") AND  cc IN (" + cc.Trim(',') + ") AND numpro IN (" + numpro.Trim(',') + ")";

                    List<ComboDTO> listaGastosProv = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);

                    var listaResultadoProv = listOC.Where(oc => //!listaGastosProv.Select(prov => Convert.ToInt32(prov.Text)).Contains((int)oc.proveedor) &&
                                                                !listaGastosProv.Select(c => c.Text).Contains(oc.cc) &&
                                                                !listaGastosProv.Select(rf => Convert.ToInt32(rf.Prefijo)).Contains(oc.numero))
                                                  .Select(r => new CboOCDTO
                                                  {
                                                      id = r.id,
                                                      text = (r.cc.ToString() + "-" + r.numero),
                                                      totalAnticipo = r.totalAnticipo,
                                                      cc = r.cc,
                                                      numero = r.numero,
                                                      proveedor = proveedores.FirstOrDefault(p => Convert.ToInt32(p.Value) == r.proveedor).Text
                                                  }).ToList();

                    Resultado.Add(ITEMS, listaResultadoProv.OrderBy(r => r.id));
                }
                else
                {
                    var listaResultadoProv = listOC
                                                .Select(r => new CboOCDTO
                                                {
                                                    id = r.id,
                                                    text = (r.cc.ToString() + "-" + r.numero),
                                                    totalAnticipo = r.totalAnticipo,
                                                    cc = r.cc,
                                                    numero = r.numero,
                                                    proveedor = proveedores.FirstOrDefault(p => Convert.ToInt32(p.Value) == r.proveedor).Text
                                                }).ToList();

                    Resultado.Add(ITEMS, listaResultadoProv.OrderBy(r => r.id));
                }

                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar la información de proveedores en enkontrol.");
            }
            return Resultado;
        }
        public Dictionary<string, object> CboEconomico()
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT 
                                        cc AS Value, 
                                         descripcion AS Text 
                                    FROM cc 
                                    WHERE st_ppto != 'T' 
                                    ORDER BY cc";
                List<ComboDTO> listaCC = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);
                listaCC.Add(new ComboDTO
                {
                    Value = "*",
                    Text = "todos"
                });

                Resultado.Add(ITEMS, listaCC.OrderBy(o => o.Value).Select(r => new { r.Value, Text = (r.Value.ToString() + "-" + r.Text) }));
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {

            }
            return Resultado;
        }
        public Dictionary<string, object> GetCheques(filtroCheques filtros)
        {
            try
            {

                DateTime nFechaIncio = new DateTime();
                DateTime nFechaFin = new DateTime();
                nFechaIncio = Convert.ToDateTime(filtros.fechaInicio);
                nFechaFin = Convert.ToDateTime(filtros.fechaFin);
                string parms = "";

                var cheques = _context.tblC_sb_cheques.Where(r => r.cuenta == filtros.cuenta).ToList();
                cheques.Select(r => r.cuenta).Distinct().ToList().ForEach(r =>
                {
                    parms += r + ",";
                });
                if (cheques != null && !string.IsNullOrEmpty(parms))
                {
                    var odbc = new OdbcConsultaDTO();
                    odbc.consulta = @"SELECT DISTINCT
                                        A.banco AS Value, 
                                        A.descripcion AS Text,
                                        B.cuenta AS Prefijo
                                    FROM sb_bancos A INNER JOIN sb_cuenta B ON A.banco = B.banco
                                    WHERE B.cuenta IN(" + parms.TrimEnd(',') + ")";

                    List<ComboDTO> listaBancos = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);


                    cheques.Select(r => r.num_pro_emp).Distinct().ToList().ForEach(r =>
                    {
                        parms += r + ",";
                    });

                    odbc = new OdbcConsultaDTO();
                    odbc.consulta = @"SELECT nombre AS Text, numpro  AS Value FROM sp_proveedores
                                    WHERE numpro IN(" + parms.TrimEnd(',') + ")";
                    List<ComboDTO> proveedores = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);




                    Resultado.Add("listaProveedores", proveedores.OrderBy(o => o.Value));
                    Resultado.Add("listaBancos", listaBancos.OrderBy(o => o.Value));
                    Resultado.Add("cheques", cheques.ToList());
                    Resultado.Add("PermisoDelete", filtros.permiso);
                    Resultado.Add(SUCCESS, true);
                }
                else
                {
                    Resultado.Add(MESSAGE, "No se encontro información");
                    Resultado.Add(SUCCESS, false);
                }
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al buscar los cheques.");
            }
            return Resultado;
        }

        private OdbcConnection checkConexionProductivo()
        {
            int tipoAmbiente = vSesiones.sesionEmpresaActual;

            switch (tipoAmbiente)
            {
                case 1:
                    return new Conexion().ConnectConstruplan();
                case 2:
                    return new Conexion().ConnectArrendarora();
                case 3:
                    return new Conexion().ConnectArrendaroraPrueba();
                default:
                    return null;
            }

        }
        public Dictionary<string, object> ValidaPoliza(List<tblC_sc_movpol> listaMovpol)
        {
            try
            {
                bool estado = true;
                decimal monto = 0;
                string parmRerencia = "";
                string cc = "";
                string month = "";
                string year = "";
                foreach (var item in listaMovpol)
                {
                    if (item.tm == 1)
                    {
                        monto += item.monto;
                    }
                    else
                    {
                        monto += item.monto;
                    }
                    if (!string.IsNullOrEmpty(item.referencia))
                        parmRerencia += "'" + item.referencia + "',";
                    if (item.year > 0)
                        year += item.year + ",";
                    if (item.mes > 0)
                        month += item.mes + ",";
                    if (!string.IsNullOrEmpty(item.cc))
                        cc += "'" + item.cc + "',";
                }

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT 
                                       A.poliza as Text, A.referencia as Value
                                    FROM sc_movpol A 
                                    WHERE A.referencia IN(" + parmRerencia.TrimEnd(',') + ") AND year IN  (" + year.TrimEnd(',') + ") AND mes IN (" + month.TrimEnd(',') + ") AND cc IN (" + cc.TrimEnd(',') + ")";
                List<ComboDTO> listaMovpolRes = _contextEnkontrol.Select<ComboDTO>(conexionEK, odbc);

                Resultado.Add("listaMovpol", listaMovpolRes);
                Resultado.Add("aplica", estado);
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception)
            {
                Resultado.Add("aplica", false);
                Resultado.Add(SUCCESS, false);
            }

            return Resultado;
        }
        public Dictionary<string, object> SaveOrUpdatePoliza(List<tblC_sc_movpol> data)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {

                    foreach (var item in data)
                    {
                        tblC_sc_movpol objMovPol = new tblC_sc_movpol();

                        objMovPol.year = item.year;
                        objMovPol.mes = item.mes;
                        objMovPol.poliza = item.poliza;
                        objMovPol.tp = item.tp;
                        objMovPol.linea = item.linea;
                        objMovPol.cta = item.cta;
                        objMovPol.scta = item.scta;
                        objMovPol.digito = item.digito;
                        objMovPol.tm = item.tm;
                        objMovPol.referencia = item.referencia;
                        objMovPol.cc = item.cc;
                        objMovPol.concepto = item.concepto;
                        objMovPol.monto = item.monto;
                        objMovPol.iclave = item.iclave;
                        objMovPol.itm = item.tm;
                        objMovPol.orden_compra = item.orden_compra;
                        objMovPol.forma_pago = item.forma_pago;
                        _context.tblC_sc_movpol.Add(objMovPol);
                        _context.SaveChanges();
                    }
                    _context.SaveChanges();
                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {

                                var count = 0;


                                foreach (var objMovPol in data)
                                {
                                    var insertMovpol = @"INSERT INTO sc_movpol 
                                                        (year
                                                        ,mes
                                                        ,poliza
                                                        ,tp
                                                        ,linea
                                                        ,cta
                                                        ,scta
                                                        ,sscta
                                                        ,digito
                                                        ,tm
                                                        ,referencia
                                                        ,cc
                                                        ,concepto
                                                        ,monto
                                                        ,iclave
                                                        ,itm
                                                        ,st_par
                                                        ,orden_compra
                                                        ,numpro
                                                        ,area
                                                        ,cuenta_oc)
                                                VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

                                    using (var cmd = new OdbcCommand(insertMovpol))
                                    {
                                        OdbcParameterCollection parameters = cmd.Parameters;
                                        parameters.Clear();

                                        parameters.Add("@year", OdbcType.Numeric).Value = objMovPol.year;
                                        parameters.Add("@mes", OdbcType.Numeric).Value = objMovPol.mes;
                                        parameters.Add("@poliza", OdbcType.Numeric).Value = objMovPol.poliza;
                                        parameters.Add("@tp", OdbcType.Char).Value = objMovPol.tp;
                                        parameters.Add("@linea", OdbcType.Numeric).Value = objMovPol.linea;
                                        parameters.Add("@cta", OdbcType.Numeric).Value = objMovPol.cta;
                                        parameters.Add("@scta", OdbcType.Numeric).Value = objMovPol.scta;
                                        parameters.Add("@sscta", OdbcType.Numeric).Value = objMovPol.sscta;
                                        parameters.Add("@digito", OdbcType.Numeric).Value = objMovPol.digito;
                                        parameters.Add("@tm", OdbcType.Numeric).Value = objMovPol.tm;
                                        parameters.Add("@referencia", OdbcType.Char).Value = objMovPol.referencia;
                                        parameters.Add("@cc", OdbcType.Char).Value = objMovPol.cc;
                                        parameters.Add("@concepto", OdbcType.Char).Value = objMovPol.concepto;
                                        parameters.Add("@monto", OdbcType.Numeric).Value = objMovPol.monto;
                                        parameters.Add("@iclave", OdbcType.Numeric).Value = 0;
                                        parameters.Add("@itm", OdbcType.Numeric).Value = objMovPol.itm;
                                        parameters.Add("@st_par", OdbcType.Char).Value = string.Empty;
                                        parameters.Add("@orden_compra", OdbcType.Numeric).Value = objMovPol.orden_compra;
                                        parameters.Add("@numpro", OdbcType.Numeric).Value = objMovPol.numpro;
                                        parameters.Add("@area", OdbcType.Numeric).Value = objMovPol.area;
                                        parameters.Add("@cuenta_oc", OdbcType.Numeric).Value = objMovPol.cuenta_oc;
                                        cmd.Connection = trans.Connection;
                                        cmd.Transaction = trans;
                                        count += cmd.ExecuteNonQuery();
                                    }
                                }

                                dbTransaction.Commit();
                                trans.Commit();

                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbTransaction.Rollback();
                                Resultado.Add(SUCCESS, false);
                                Resultado.Add(MESSAGE, "Ocurrió un error el cheque al insertar en enkontrol.");
                                return Resultado;
                            }
                        }
                    }
                    Resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "GuardarBanco", e, AccionEnum.ACTUALIZAR, 0, null);
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "Ocurrió un error el cheque.");
                }
                return Resultado;
            }

        }

        public Dictionary<string, object> BuscarCuenta(int cuenta, int subCuenta, int ssubCuenta)
        {
            try
            {
                string addCuenta = "";
                if (cuenta != 0)
                {
                    addCuenta = "cta=" + cuenta;
                    if (subCuenta != 0)
                    {
                        addCuenta += " AND scta=" + subCuenta;

                        if (ssubCuenta != 0)
                        {
                            addCuenta += " AND sscta=" + subCuenta;
                        }
                    }
                    else
                    {
                        if (ssubCuenta != 0)
                        {
                            addCuenta += " AND sscta=" + subCuenta;
                        }
                    }

                }

                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @"SELECT  cta,scta,sscta,descripcion,digito, cta AS Value, descripcion as Text, 0 AS  tipoS, requiere_oc 
                                  FROM catcta WHERE " + addCuenta;

                List<ctaDTO> listaCuentas = _contextEnkontrol.Select<ctaDTO>(conexionEK, odbc);

                Resultado.Add(SUCCESS, true);
                Resultado.Add("ListaCuentas", listaCuentas);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, e.Message);
            }
            return Resultado;
        }

        public dynamic GetDescripcionesCta(string term)
        {
            try
            {
                var odbc = new OdbcConsultaDTO();
                odbc.consulta = @" SELECT TOP 12  cta as cta,scta, sscta, descripcion as label
                    FROM sb_cuenta
                    WHERE LIKE ? ORDER BY id";
                odbc.parametros.Add(new OdbcParameterDTO()
                {
                    nombre = "label",
                    tipo = OdbcType.VarChar,
                    valor = (string)"%" + term.Trim() + "%"
                });
                List<dynamic> listaCuentas = _contextEnkontrol.Select<dynamic>(conexionEK, odbc);

                return listaCuentas.Select(x => new
                {
                    id = (string)x.id,
                    value = (string)x.label
                }).ToList();

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, Ocurrio un error al momento de hacer la carga de Cuentas desde enkoltrol.");
            }
            return Resultado;
        }

        public Dictionary<string, object> BuscarOC(string cc, int numero)
        {
            try
            {


                if (!string.IsNullOrEmpty(cc))
                {

                    var odbcProv = new OdbcConsultaDTO();
                    odbcProv.consulta = @"SELECT 0 as id,A.numero AS numero , 1 as Anticipo, A.total  as totalAnticipo , A.total as Total, a.fecha , A.cc, c.numpro as numpro , c.nombre as proveedor
                                            FROM so_orden_compra A 
                                            LEFT JOIN sp_gastos_prov B on A.cc = B.cc AND A.numero =referenciaoc 
                                            INNER JOIN sp_proveedores c ON a.proveedor = c.numpro
                                            where a.fecha > '20200101' AND A.estatus IS NOT NULL AND A.CC ='" + cc + "' AND (a.total - a.total_pag)!=0";

                    List<dtBusquedaOC> listOC = _contextEnkontrol.Select<dtBusquedaOC>(conexionEK, odbcProv);
                    Resultado.Add(ITEMS, listOC.OrderBy(r => r.id));

                }
                else
                {

                    var odbcProv = new OdbcConsultaDTO();
                    odbcProv.consulta = @"SELECT 0 as id,A.numero AS numero , 1 as Anticipo, A.total  as totalAnticipo , A.total as Total, a.fecha , A.cc, c.numpro as numpro , c.nombre as proveedor
                                            FROM so_orden_compra A 
                                            LEFT JOIN sp_gastos_prov B on A.cc = B.cc AND A.numero =referenciaoc 
                                            INNER JOIN sp_proveedores c ON a.proveedor = c.numpro
                                            where a.fecha > '20200101' AND A.estatus IS NOT NULL AND A.CC ='" + cc + "' and (a.total - a.total_pag)!=0";

                    List<dtBusquedaOC> listOC = _contextEnkontrol.Select<dtBusquedaOC>(conexionEK, odbcProv);
                    Resultado.Add(ITEMS, listOC.OrderBy(r => r.id));
                }

                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, al momento de cargar las ordenes de compra");
            }
            return Resultado;
        }


        public Dictionary<string, object> GetOCSeleccionado(string cc, int numero)
        {
            try
            {

                var odbcProv = new OdbcConsultaDTO();
                odbcProv.consulta = @"SELECT 0 as id,A.numero AS numero , TOTAL_REC - total_pag as Anticipo, TOTAL_REC - total_pag  as totalAnticipo , A.total as Total, A.fecha , A.cc, C.numpro as numpro , C.nombre as proveedor
                                            FROM so_orden_compra A 
                                            INNER JOIN sp_proveedores C ON A.proveedor = C.numpro
                                            where  A.CC ='" + cc + "' AND A.NUMERO=" + numero;

                List<dtBusquedaOC> listOC = _contextEnkontrol.Select<dtBusquedaOC>(conexionEK, odbcProv);
                Resultado.Add(ITEMS, listOC.OrderBy(r => r.id));
                Resultado.Add(SUCCESS, true);

            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Ocurrió un error, No se enconetro informacion de la orden de compra seleccionada.");
            }
            return Resultado;
        }

        public Dictionary<string, object> DeleteCheque(int chequeID)
        {

            using (var dbTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var chequeDelete = _context.tblC_sb_cheques.First(r => r.id == chequeID);

                    var movpolobjDelete = _context.tblC_sc_movpol.Where(r => r.mes == chequeDelete.imes && r.year == chequeDelete.iyear && r.poliza == chequeDelete.ipoliza && r.tp == chequeDelete.itp);

                    var polizaDelete = _context.tblC_sc_polizas.Where(r => r.mes == chequeDelete.imes && r.year == chequeDelete.iyear && r.poliza == chequeDelete.ipoliza && r.tp == chequeDelete.itp);

                    using (var con = checkConexionProductivo())
                    {
                        using (var trans = con.BeginTransaction())
                        {
                            try
                            {
                                var count = 0;
                                var consultaEliminarCheque = @"DELETE FROM sb_cheques WHERE iyear = ? AND imes = ? AND ipoliza=? AND itp=?";

                                using (var cmd = new OdbcCommand(consultaEliminarCheque))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;

                                    parameters.Add("@iyear", OdbcType.Numeric).Value = chequeDelete.iyear;
                                    parameters.Add("@imes", OdbcType.Numeric).Value = chequeDelete.imes;
                                    parameters.Add("@ipoliza", OdbcType.Numeric).Value = chequeDelete.ipoliza;
                                    parameters.Add("@itp", OdbcType.Char).Value = chequeDelete.itp;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;

                                    count += cmd.ExecuteNonQuery();
                                }

                                var consultaElimiarMovPol = @"DELETE FROM sc_movpol WHERE year = ? AND mes = ? AND poliza=? AND tp=?";

                                using (var cmd = new OdbcCommand(consultaElimiarMovPol))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;

                                    parameters.Add("@year", OdbcType.Numeric).Value = chequeDelete.iyear;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = chequeDelete.imes;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = chequeDelete.ipoliza;
                                    parameters.Add("@tp", OdbcType.Char).Value = chequeDelete.itp;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;

                                    count += cmd.ExecuteNonQuery();
                                }


                                var consultaElimiarPoliza = @"DELETE FROM sc_polizas WHERE year = ? AND mes = ? AND poliza=? AND tp=?";

                                using (var cmd = new OdbcCommand(consultaElimiarPoliza))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;

                                    parameters.Add("@year", OdbcType.Numeric).Value = chequeDelete.iyear;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = chequeDelete.imes;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = chequeDelete.ipoliza;
                                    parameters.Add("@tp", OdbcType.Char).Value = chequeDelete.itp;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;

                                    count += cmd.ExecuteNonQuery();
                                }

                                var movpolObj = movpolobjDelete.FirstOrDefault();
                                var consultaElimiar = @"DELETE FROM sp_movprov WHERE tm = ? AND cc = ? AND referenciaoc=? AND tp=? AND mes = ? AND year=? AND poliza = ?";

                                using (var cmd = new OdbcCommand(consultaElimiar))
                                {
                                    OdbcParameterCollection parameters = cmd.Parameters;

                                    parameters.Add("@tm", OdbcType.Numeric).Value = 51;
                                    parameters.Add("@cc", OdbcType.Char).Value = movpolObj.cc;
                                    parameters.Add("@referenciaoc", OdbcType.Char).Value = movpolObj.orden_compra;
                                    parameters.Add("@tp", OdbcType.Char).Value = 7;
                                    parameters.Add("@mes", OdbcType.Numeric).Value = movpolObj.mes;
                                    parameters.Add("@year", OdbcType.Numeric).Value = movpolObj.year;
                                    parameters.Add("@poliza", OdbcType.Numeric).Value = movpolObj.poliza;

                                    cmd.Connection = trans.Connection;
                                    cmd.Transaction = trans;

                                    count += cmd.ExecuteNonQuery();
                                }

                                _context.tblC_sb_cheques.Remove(chequeDelete);
                                _context.SaveChanges();
                                _context.tblC_sc_movpol.RemoveRange(movpolobjDelete);
                                _context.SaveChanges();
                                _context.tblC_sc_polizas.RemoveRange(polizaDelete);
                                _context.SaveChanges();
                                dbTransaction.Commit();
                                trans.Commit();
                            }
                            catch (Exception e)
                            {
                                trans.Rollback();
                                dbTransaction.Rollback();
                                Resultado.Add(SUCCESS, false);
                                Resultado.Add(MESSAGE, "Ocurrió un error el cheque al insertar en enkontrol.");
                                return Resultado;
                            }
                        }
                    }
                    Resultado.Add(SUCCESS, true);
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();
                    LogError(0, 0, NombreControlador, "CancelarCheque", e, AccionEnum.ACTUALIZAR, 0, null);
                    Resultado.Add(SUCCESS, false);
                    Resultado.Add(MESSAGE, "SE presento un error al momento de cancelar el cheque..");
                }
                return Resultado;
            }
        }

        #region EditarPolizas
        public Dictionary<string, object> OpenEditCheques(int idCheque)
        {
            try
            {
                var infoCheque = _context.tblC_sb_cheques.First(r => r.id == idCheque);
                var movPolCheque = _context.tblC_sc_movpol.Where(r => r.poliza == infoCheque.ipoliza && r.year == infoCheque.iyear && r.mes == infoCheque.imes).ToList();
                var objMovpol = movPolCheque.FirstOrDefault();
                ///  var infoGastosProv = _context.tblC_sp_gastos_prov.Where(r => r.cc == objMovpol.cc && r.numpro == objMovpol.numpro && objMovpol.orden_compra == Convert.ToInt32(r.referenciaoc));
                var movPolProveedores = _context.tblC_sc_movpol.Where(r => r.year == infoCheque.iyear && r.mes == infoCheque.imes && r.tp == "07" && r.orden_compra == objMovpol.orden_compra).ToList();
                var movPolDiario = _context.tblC_sc_movpol.Where(r => r.year == infoCheque.iyear && r.mes == infoCheque.imes && r.tp == "03" && r.orden_compra == objMovpol.orden_compra).ToList();

                Resultado.Add("listaPolizasChequeCap", movPolCheque);
                Resultado.Add("listaPolizasProveedores", movPolProveedores);
                Resultado.Add("listaPolizasDiario", movPolDiario);
                Resultado.Add(SUCCESS, true);
            }
            catch (Exception e)
            {
                Resultado.Add(SUCCESS, false);
                Resultado.Add(MESSAGE, "Se presento un error al momento de cargar la información de las polizas consulte con TI");
            }

            return Resultado;
        }

        private gastos_provDTO GetGastosProv(int refernciaOC, string cc)
        {
            var odbc = new OdbcConsultaDTO();
            string queryGastosCplan = @"SELECT numpro,referenciaoc,cc,tm,cfd_folio as factura,monto,iva,tipocambio,total,cfd_folio, moneda
                                            FROM sp_gastos_prov WHERE cc IN ('" + cc + "') AND referenciaoc IN ('" + refernciaOC + "')";
            odbc.consulta = queryGastosCplan;
            List<gastos_provDTO> listaGastosProv = _contextEnkontrol.Select<gastos_provDTO>(conexionEK, odbc);
            return listaGastosProv.FirstOrDefault();
        }


        #endregion

    }
}
