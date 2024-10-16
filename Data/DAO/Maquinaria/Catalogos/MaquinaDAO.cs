using Core.DAO.Maquinaria.Catalogos;
using Core.DTO;
using Core.DTO.Captura;
using Core.DTO.COMPRAS;
using Core.DTO.Enkontrol.Tablas.CC;
using Core.DTO.Maquinaria.Captura;
using Core.DTO.Maquinaria.Catalogos;
using Core.DTO.Maquinaria.Reporte;
using Core.DTO.Principal.Generales;
using Core.DTO.Utils.Data;
using Core.Entity.Maquinaria.Captura;
using Core.Entity.Maquinaria.Catalogo;
using Core.Entity.Maquinaria.Inventario;
using Core.Entity.Principal.Alertas;
using Core.Entity.Principal.Multiempresa;
using Core.Enum.Maquinaria;
using Core.Enum.Maquinaria.CargoNomina;
using Core.Enum.Maquinaria.Catalogos;
using Core.Enum.Multiempresa;
using Core.Enum.Principal;
using Core.Enum.Principal.Bitacoras;
using Data.EntityFramework.Context;
using Data.EntityFramework.Generic;
using Data.Factory.Principal.Archivos;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using Core.Entity.Maquinaria;

namespace Data.DAO.Maquinaria.Catalogos
{
    public class MaquinaDAO : GenericDAO<tblM_CatMaquina>, IMaquinaDAO
    {
        private int[] usuariosAlertaAltaNoEco = new int[1]
            {
                3978 /*Jessica Galdean*/
            };
        private ArchivoFactoryServices archivofs = new ArchivoFactoryServices();
        public List<string> getListaCorreosInventario(int id)
        {
            var data = _context.tblM_CorreosEnvioInventario.Where(x => x.TipoEnvio == id && x.estatus == true).Select(x => x.correo);

            return data.ToList();
        }

        public List<tblM_CatMaquina> FillGridMaquina(MaquinaFiltrosDTO obj)
        {

            var result = (from m in _context.tblM_CatMaquina
                          join g in _context.tblM_CatGrupoMaquinaria on m.grupoMaquinariaID equals g.id
                          join t in _context.tblM_CatTipoMaquinaria on g.tipoEquipoID equals t.id
                          where (string.IsNullOrEmpty(obj.descripcion) == true ? m.descripcion == m.descripcion : m.descripcion.Contains(obj.descripcion))
                         && (obj.idTipo == 0 ? t.id == t.id : obj.idTipo == t.id) && (obj.idGrupo == 0 ? g.id == g.id : g.id == obj.idGrupo)
                         && (string.IsNullOrEmpty(obj.noEconomico) == true ? m.noEconomico == m.noEconomico : m.noEconomico.Contains(obj.noEconomico)) && obj.estatus == m.estatus
                         && m.aseguradora.estatus == true && g.estatus == true && t.estatus == true && m.marca.estatus == true && !string.IsNullOrEmpty(m.noEconomico)
                          select m).ToList();
            return result;
        }
        public List<tblM_CatMaquina> FillGridMaquina(DateTime inicio, DateTime fin, int tipo, int estatus)
        {
            var result = _context.tblM_CatMaquina.Where(x => x.estatus == estatus && (tipo == 0 ? true : x.TipoBajaID == tipo) && (x.fechaBaja >= inicio && x.fechaBaja <= fin)).ToList();
            return result;

        }
        public List<tblM_CatMaquina> FillInventarioMaquinaria(MaquinaFiltrosDTO obj)
        {

            var result = from m in _context.tblM_CatMaquina
                         where (obj.idTipo == 0 ? m.grupoMaquinariaID == m.grupoMaquinariaID : m.grupoMaquinaria.tipoEquipoID.Equals(obj.idTipo))
                         && (string.IsNullOrEmpty(obj.ccId) ? m.centro_costos == m.centro_costos : obj.ListCC.Contains(m.centro_costos)) && m.marcaID != 0 && m.modeloEquipoID != 0 && !string.IsNullOrEmpty(m.noEconomico) && !string.IsNullOrEmpty(m.centro_costos)
                         select m;

            return result.ToList();
        }

        public List<tblM_CatModeloEquipo> FillCboModeloEquipo(int idMarca)
        {
            return _context.tblM_CatModeloEquipo.Where(x => x.marcaEquipoID == idMarca && x.estatus == true && x.marcaEquipo.estatus == true).ToList();
        }

        public List<tblM_CatModeloEquipo> FillCboModeloEquipoGrupo(int idGrupo)
        {
            return _context.tblM_CatModeloEquipo.Where(x => x.idGrupo == idGrupo && x.estatus == true && x.marcaEquipo.estatus == true).ToList();
        }

        public List<tblM_CatTipoMaquinaria> FillCboTipoMaquinaria(bool estatus)
        {
            return _context.tblM_CatTipoMaquinaria.Where(x => x.estatus == estatus).ToList();
        }
        public List<tblM_CatGrupoMaquinaria> FillCboGrupoMaquinaria(int idTipo)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == true && x.tipoEquipoID == idTipo).OrderBy(x => x.descripcion).ToList();
        }
        public tblM_CatGrupoMaquinaria getGrupoMaquina(int idGrupo)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.id == idGrupo && x.estatus == true).FirstOrDefault();
        }

        public List<tblM_CatGrupoMaquinaria> FillCboFiltroGrupoMaquinaria(bool estatus)
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.estatus == estatus).ToList();
        }
        public List<tblM_CatAseguradora> FillCboAseguradora(bool estatus)
        {
            return _context.tblM_CatAseguradora.Where(x => x.estatus == estatus).ToList();
        }
        public List<tblM_CatMarcaEquipo> FillCboMarcasEquipo(int idGrupo)
        {
            var relacionMarca = _context.tblM_CatMarcaEquipotblM_CatGrupoMaquinaria.Where(x => x.tblM_CatGrupoMaquinaria_id == idGrupo).Select(x => x.tblM_CatMarcaEquipo_id).ToList();
            return _context.tblM_CatMarcaEquipo.Where(x => relacionMarca.Contains(x.id) && x.estatus == true).ToList();
        }

        public void Guardar(tblM_CatMaquina obj)
        {
            if (!Exists(obj))
            {

                if (obj.id == 0)
                {
                    SaveEntity(obj, (int)BitacoraEnum.MAQUINA);

                    if (!string.IsNullOrEmpty(obj.noEconomico))
                    {
                        var alertas = new List<tblP_Alerta>();
                        foreach (var usuario in usuariosAlertaAltaNoEco)
                        {
                            var alerta = new tblP_Alerta()
                            {
                                userEnviaID = vSesiones.sesionUsuarioDTO.id,
                                userRecibeID = usuario,
                                tipoAlerta = 2,
                                sistemaID = 11,
                                visto = false,
                                url = "/ActivoFijo/DepreciacionMaquinas?obj=" + obj.id,
                                objID = obj.id,
                                msj = "Alta #económico: " + obj.noEconomico,
                                moduloID = 0
                            };
                            alertas.Add(alerta);
                        }
                        _context.tblP_Alerta.AddRange(alertas);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    Update(obj, obj.id, (int)BitacoraEnum.MAQUINA);
                }
            }
            else
            {
                if (obj.id == 0)
                    throw new Exception("Ya existe una maquina con ese numero economico contacte el area de sistemas");
                else
                    Update(obj, obj.id, (int)BitacoraEnum.MAQUINA);
            }
        }
        public bool Exists(tblM_CatMaquina obj)
        {
            if (obj.noEconomico != null)
            {
                return _context.tblM_CatMaquina.Where(x => x.noEconomico == obj.noEconomico &&
                                        x.id != obj.id && !string.IsNullOrEmpty(x.noEconomico)).ToList().Count > 0 ? true : false;
            }
            return false;

        }

        public void NotificarAltaFichaTecnica(tblM_CatMaquina objMaquina, FichaTecnicaAltaDTO setImprimible)
        {
            try
            {

                #region NOTIFICANTES
                
                var ccs = _context.tblP_CC.Where(e => e.estatus && e.areaCuenta.Contains("-")).OrderBy(x=>x.area).ThenBy(x=>x.cuenta).ToList();
                var lstAutorizadores = _context.tblP_CC_Usuario.ToList();
                var lstUsuarios = _context.tblP_Usuario.ToList();
                var lstTiposEquipo = _context.tblM_CatTipoMaquinaria.Where(e => e.estatus).ToList();
                var lstCorreos = new List<string>();

                var usuariosAC = lstAutorizadores.Where(x => x.cc.Equals(objMaquina.centro_costos)).Select(x => x.id).ToList();
                var autorizadores = _context.tblP_Autoriza.Where(x => usuariosAC.Contains(x.cc_usuario_ID)).ToList();

                var objObra = ccs.FirstOrDefault(e => e.areaCuenta == objMaquina.centro_costos);
                string descObra = "";

                if (objObra != null)
	            {
                    descObra = "[" + objObra.areaCuenta + "] " + objObra.descripcion;
	            }

                var numTipoEquipo = Convert.ToInt32(setImprimible.TipoEquipo);
                var objTipoEquipo = lstTiposEquipo.FirstOrDefault(e => e.id == numTipoEquipo);
                string descTipoEquipo = "";

                if (objTipoEquipo != null)
                {
                    descTipoEquipo = objTipoEquipo.descripcion;
                }

                var adminMaq = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 5);
                var gerenteObra = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 1);
                var directorArea = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 4);
                var directoDivision = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 2);
                //var directorServicios = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 11); ESTOS ÑO
                //var altaDireccion = autorizadores.FirstOrDefault(x => x.perfilAutorizaID == 3); ESTOS ÑO

                if (adminMaq != null)
                {
                    var objUsuarioAdmin = lstUsuarios.FirstOrDefault(e => e.id == adminMaq.usuarioID);

                    if (objUsuarioAdmin != null)
                    {
                        lstCorreos.Add(objUsuarioAdmin.correo);
                    }
                }

                if (gerenteObra != null)
                {
                    var objUsuarioGerenteObra = lstUsuarios.FirstOrDefault(e => e.id == gerenteObra.usuarioID);

                    if (objUsuarioGerenteObra != null)
                    {
                        lstCorreos.Add(objUsuarioGerenteObra.correo);
                    }
                }

                if (directorArea != null)
                {
                    var objUsuarioDirectorArea = lstUsuarios.FirstOrDefault(e => e.id == directorArea.usuarioID);

                    if (objUsuarioDirectorArea != null)
                    {
                        lstCorreos.Add(objUsuarioDirectorArea.correo);
                    }
                }

                if (directoDivision != null)
                {
                    var objUsuarioDirectorDivision = lstUsuarios.FirstOrDefault(e => e.id == directoDivision.usuarioID);

                    if (objUsuarioDirectorDivision != null)
                    {
                        lstCorreos.Add(objUsuarioDirectorDivision.correo);
                    }
                }

                lstCorreos = lstCorreos.Distinct().ToList();
                #endregion

                #region CORREO
                string AsuntoCorreo = @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día, se le informa de la captura de una nueva ficha tecnica: <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p> 
                                                    <p class=MsoNormal>
                                                        <o:p><b>Obra</b>: " + (descObra) + @"</o:p><br>
                                                        <o:p><b>Grupo</b>: " + (objMaquina.grupoMaquinaria != null ? objMaquina.grupoMaquinaria.descripcion : "") + @"</o:p><br>
                                                        <o:p><b>Modelo</b>: " + (objMaquina.modeloEquipo != null ? objMaquina.modeloEquipo.descripcion : "") + @"</o:p><br>
                                                        <o:p><b>Proveedor</b>: " + (objMaquina.proveedor ?? "N/A") + @"</o:p><br>
                                                        <o:p><b>Tipo del equipo</b>: " + (descTipoEquipo) + @"</o:p><br>
                                                        <o:p><b>Descripcion</b>: " + (objMaquina.descripcion )+ @"</o:p><br>
                                                        <o:p><b>Marca</b>: " + (objMaquina.marca != null ? objMaquina.marca.descripcion : "") + @"</o:p><br>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de MAQUINARÍA, menu de Inventario y al sub-menu Alta Equipo<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";

#if DEBUG
                lstCorreos = new List<string>() { "miguel.buzani@construplan.com.mx", "martin.zayas@construplan.com.mx" }; 
#endif
                lstCorreos.Add("e.encinas@construplan.com.mx");
                if (lstCorreos.Count > 0)
                {
                    var success = GlobalUtils.sendEmail((string.Format("{0}: ALTA FICHA TECNICA MAQUINA: {1}", PersonalUtilities.GetNombreEmpresa(), objMaquina.noEconomico)), AsuntoCorreo, lstCorreos);

                    if (!success)
                    {
                        LogError(1, 0, "CatMaquinaController", "NotificarAltaFichaTecnica", null, AccionEnum.CORREO, objMaquina.id, objMaquina);
                    }
                }

                #endregion

            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        public tblM_CatMaquina GetMaquina(int obj, List<tblM_CatMaquina> _lstCatMaquinasDapperDTO = null)
        {
            #region v1
            //return _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj));
            #endregion

            #region v2
            if (_lstCatMaquinasDapperDTO != null)
                return _lstCatMaquinasDapperDTO.FirstOrDefault(f => f.id.Equals(obj));
            else
                return _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj));
            #endregion
        }
        public List<tblM_CatMaquina> getCboMaquinaria(string obj)
        {
            return _context.tblM_CatMaquina.Where(x => obj != "0" ? x.centro_costos.Equals(obj.ToString()) : x.id.Equals(x.id) && !string.IsNullOrEmpty(x.noEconomico)).ToList();
        }
        public List<tblM_CatMaquina> GetAllMaquinas()
        {
            return _context.tblM_CatMaquina.Where(x => x.estatus != 0 && !string.IsNullOrEmpty(x.noEconomico)).ToList();
        }
        public List<tblM_CatPipas> GetAllPipas()
        {
            return _context.tblM_CatPipa.ToList();
        }
        public List<tblM_CatMaquina> getCboMaquinariaFiltro(int obj)
        {
            var result = _context.tblM_CatMaquina
                .Join(_context.tblM_MaquinariaRentada,
                        Maquina => Maquina.noEconomico,
                        Renta => Renta.NoEconomico,
                        (Maquina, Renta) => Maquina)
                        .Where(x => x.centro_costos.Equals(obj.ToString()))
                        .GroupBy(x => x.noEconomico)
                        .Select(group => group.FirstOrDefault());
            return result.ToList();
        }

        public List<tblM_CatMaquina> FillCboEconomicos(int grupo)
        {
            return _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID.Equals(grupo)).ToList();
        }
        public List<AutocompletadoDTO> EconomicoDesripcion(string term)
        {
            var items = (from economico in _context.tblM_CatMaquina.AsQueryable()
                         where economico.noEconomico.ToUpper().Contains(term.ToUpper())
                         select new AutocompletadoDTO
                         {
                             id = economico.id.ToString(),
                             value = economico.noEconomico
                         }).Take(15).ToList();
            return items;
        }
        public tblM_CatMaquina GetMaquinaByNoEconomico(string obj)
        {
            try
            {
                var result = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(obj) && !string.IsNullOrEmpty(x.noEconomico));
                if (result == null)
                {
                    return new tblM_CatMaquina();
                }
                else
                {
                    return result;
                }

            }
            catch
            {

                var result = new tblM_CatMaquina();
                return result;
            }
        }

        public tblM_CatMaquina getEconomicoIDNo(string noEconomico)
        {
            var result = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(noEconomico));

            return result;
        }


        public List<tblM_CatMaquina> getListaMaquinaria(MaquinaFiltrosDTO obj)
        {

            var result = (from m in _context.tblM_CatMaquina
                          join g in _context.tblM_CatGrupoMaquinaria on m.grupoMaquinariaID equals g.id
                          join t in _context.tblM_CatTipoMaquinaria on g.tipoEquipoID equals t.id
                          where (string.IsNullOrEmpty(obj.descripcion) == true ? m.id == m.id : m.descripcion.Contains(obj.descripcion))
                         && (obj.idTipo == 0 ? g.tipoEquipoID == t.id : obj.idTipo == t.id) && (obj.idGrupo == 0 ? m.grupoMaquinariaID == g.id : g.id == obj.idGrupo)
                         && (string.IsNullOrEmpty(obj.noEconomico) == true ? m.id == m.id : m.noEconomico.Contains(obj.noEconomico))
                         && (obj.estatus == 0 ? m.noEconomico == null : m.id == m.id)
                          select m).ToList();
            /// var result = _context.tblM_CatMaquina.Where(x => x.noEconomico == null).ToList();
            return result;
        }

        public List<tblM_CatGrupoMaquinaria> GetGrupoMaquinarias()
        {
            return _context.tblM_CatGrupoMaquinaria.Where(x => x.id == x.id).ToList();
        }


        public List<tblM_CatMaquina> GetMaquinaByID(int id)
        {
            return _context.tblM_CatMaquina.Where(x => x.id.Equals(id)).ToList();
        }

        public tblM_CatMaquina EconomicoNotNull(int obj)
        {
            var result = _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj) && x.noEconomico != null);
            return result;
        }

        public string GetParcialEconomico(int obj)
        {
            tblM_CatMaquina res = _context.tblM_CatMaquina.FirstOrDefault(x => x.id.Equals(obj));


            int grupo = res.grupoMaquinariaID;
            var temp = _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID.Equals(res.grupoMaquinariaID) && x.id != res.id && res.renta.Equals(x.renta) && !string.IsNullOrEmpty(x.noEconomico)).OrderByDescending(x => x.noEconomico).ToList();


            string eco = "";
            if (temp.Count() > 0)
            {

                if (temp.FirstOrDefault().renta == true)
                {

                    var economico = temp.FirstOrDefault().noEconomico;

                    if (economico.Contains("-R"))
                    {
                        string[] parts1 = economico.Split(new string[] { "-R" }, StringSplitOptions.None);
                        var data = temp.FirstOrDefault().noEconomico.Split('-');
                        var count = data.Length - 1;
                        int numero = Convert.ToInt32(data[count].TrimStart('R'));
                        string nombCorto = parts1[0];
                        string adCero = "";
                        if (numero < 10)
                        {
                            adCero = "0";
                        }
                        eco = nombCorto + "-" + "R" + adCero + (numero + 1);
                    }
                    else
                    {
                        string nombCorto = temp.FirstOrDefault().noEconomico.Split('-')[0];


                        eco = nombCorto + "-" + "R01";
                    }

                }
                else
                {
                    var economico = temp.FirstOrDefault().noEconomico;
                    if (economico.Contains("-R"))
                    {
                        string[] parts1 = economico.Split(new string[] { "-R" }, StringSplitOptions.None);
                        var data = temp.FirstOrDefault().noEconomico.Split('-');
                        var count = data.Length - 1;
                        int numero = Convert.ToInt32(data[count].TrimStart('R'));


                        string nombCorto = parts1[0];
                        string adCero = "";
                        if (numero < 10)
                        {
                            adCero = "0";
                        }
                        eco = nombCorto + "-" + adCero + (numero + 1);
                    }
                    else
                    {
                        var data = temp.FirstOrDefault().noEconomico.Split('-');
                        var count = data.Length - 1;
                        int numero = Convert.ToInt32(data[count]);
                        string nombCorto = temp.FirstOrDefault().noEconomico.Split('-')[0];
                        string adCero = "";
                        if (numero < 10)
                        {
                            adCero = "0";
                        }
                        eco = nombCorto + "-" + adCero + (numero + 1);
                    }
                }
            }
            return eco;
        }

        public string GetNumeroEconomico(int idGrupo, bool renta)
        {

            var temp = _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID.Equals(idGrupo) && x.renta.Equals(renta)).OrderByDescending(x => x.noEconomico).ToList();
            string eco = "";
            if (temp != null)
            {

                if (renta == true)
                {

                    int numero = Convert.ToInt32(temp.FirstOrDefault().noEconomico.Split('R')[1]);
                    string nombCorto = temp.FirstOrDefault().noEconomico.Split('R')[0];

                    eco = nombCorto + "-" + "R" + ((numero + 1) < 10 ? "0" + (numero + 1) : (numero + 1).ToString());
                }
                else
                {
                    if (temp.FirstOrDefault() != null)
                    {
                        var split = temp.FirstOrDefault().noEconomico.Split('-').ToList();

                        if (split.Count() == 2)
                        {
                            int numero = Convert.ToInt32(split[1]);
                            string nombCorto = temp.FirstOrDefault().noEconomico.Split('-')[0];
                            //   eco = nombCorto + "-" + (numero + 1);

                            eco = nombCorto + "-" + ((numero + 1) < 10 ? "0" + (numero + 1) : (numero + 1).ToString());
                        }
                        else
                        {
                            int num1 = split.Count() - 1;
                            int numero = Convert.ToInt32(split[num1]);
                            string nombCorto = "";
                            for (int i = 0; i < num1; i++)
                            {
                                if (i != num1)
                                {
                                    nombCorto += split[i] + "-";
                                }
                            }
                            nombCorto = nombCorto.TrimEnd('-');

                            eco = nombCorto + "-" + ((numero + 1) < 10 ? "0" + (numero + 1) : (numero + 1).ToString());
                        }
                    }
                }
            }
            return eco;
        }

        public List<tblM_CatMaquina> ListaEquiposSolicitud(int grupo)
        {

            var data = _context.tblM_CatMaquina.Where(x => x.grupoMaquinariaID.Equals(grupo) && x.noEconomico != null);
            return data.ToList();
        }
        public List<tblM_CatMaquina> ListaEquiposGrupo(List<int> grupo)
        {

            var data = _context.tblM_CatMaquina.Where(x => grupo.Contains(x.grupoMaquinariaID) && x.estatus != 0).ToList();

            return data;
        }

        public List<inventarioGeneralDTO> GetInventarioMaquinaria(MaquinaFiltrosDTO obj)
        {
            //
            var _ccsSigoplan = _context.tblP_CC.ToList();
            var _ccsEnkontrol = _context.tblP_CC.ToList();
           // var _ccsEnkontrol = (IList<ccDTO>)_contextEnkontrol.Where("SELECT cc, descripcion, corto FROM cc", 1).ToObject<IList<ccDTO>>();
            //

            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Colombia:
                    {
                        var listCC = new List<string>();
                        foreach (var item in obj.ListCC)
                        {
                            var _cc = _ccsSigoplan.FirstOrDefault(x => x.cc == item);
                            if (_cc != null)
                            {
                                listCC.Add(_cc.areaCuenta);
                            }
                        }
                        obj.ListCC = listCC;
                    }
                    break;
            }


            List<inventarioGeneralDTO> Result = new List<inventarioGeneralDTO>();
            try
            {
                
            var R = new List<tblM_CatMaquina>();
            if (obj.ListCC != null)
            {
                R = (from m in _context.tblM_CatMaquina
                     where obj.ListCC.Contains(m.centro_costos)
                         && (obj.idTipo == 0 ? m.id.Equals(m.id) : obj.idTipo == m.grupoMaquinaria.tipoEquipo.id)
                         && m.estatus != 0

                     select m).ToList();
            }
            else
            {
                R = _context.tblM_CatMaquina.Where(m => (obj.idTipo == 0 ? m.id.Equals(m.id) : obj.idTipo == m.grupoMaquinaria.tipoEquipo.id) &&
                                (string.IsNullOrEmpty(obj.noEconomico) ? m.id == m.id : m.noEconomico.StartsWith(obj.noEconomico)) && m.estatus != 0).ToList();
            }

            var rawListR = R.Select(x => x.noEconomico).ToList();
            //var dataRaw = _context.tblM_CapHorometro.Where(x => rawListR.Contains(x.Economico)).ToList();

            List<tblM_CapHorometro> dataRaw = _context.tblM_CapHorometro.GroupBy(x => x.Economico, (key, g) => g.OrderByDescending(e => e.Fecha).ThenByDescending(e => e.id).FirstOrDefault()).ToList();
            //
            var rawIdR = R.Select(x => x.id).ToList();
            var _redi = _context.tblM_AsignacionEquipos.Where(x => rawIdR.Contains(x.noEconomicoID) && x.estatus >= 2 && x.estatus < 5).OrderByDescending(x => x.id).ToList();
            //

            var fech = DateTime.Now.ToShortDateString();
            DateTime a = Convert.ToDateTime(fech);
            var lstCapturaDatosDiarios = _context.tblM_CapturaDatosDiariosMaquinaria.Where(r => r.fechaCapturaMaquinaria == a ).ToList().Select(y => new
            {
                idCatMaquina = y.idCatMaquina,
                idEstatus = y.idEstatus,
                noEconomico = _context.tblM_CatMaquina.Where(s => s.id == y.idCatMaquina).FirstOrDefault().noEconomico
            }).ToList();

            if (R.Count > 0)
            {
                foreach (var m in R)
                {
                    var data = dataRaw.Where(x => x.Economico.Contains(m.noEconomico)).OrderByDescending(x => x.id).FirstOrDefault();

                    string horasAcumuladas = "";
                    string horaActual = "";
                    bool ccExist = false;
                    string stb = "";
                    string CargoObra = "";
                    string _ac_ = null;
                    string _cc_ = null;

                    string centro_costosName = "";
                    if (data != null)
                    {
                        horasAcumuladas = data.HorometroAcumulado.ToString();
                        horaActual = data.Horometro.ToString();
                    }

                    switch (m.centro_costos)
                    {
                        case "1010":
                            {
                                ccExist = true;
                                centro_costosName = "TALLER DE MAQUINARIA CENTRAL";
                                CargoObra = "MAQUINARIA NO ASIGNADA A OBRA";
                                _ac_ = "14-1";
                                _cc_ = "997";
                                break;
                            }
                        case "1015":
                            {
                                ccExist = true;
                                centro_costosName = "PATIO DE MAQUINARIA";
                                CargoObra = "MAQUINARIA NO ASIGNADA A OBRA";
                                _ac_ = "14-1";
                                _cc_ = "997";
                                break;
                            }
                        case "1018":
                            {
                                ccExist = true;
                                centro_costosName = "TALLER DE OVEHAUL";
                                CargoObra = "MAQUINARIA NO ASIGNADA A OBRA";
                                _ac_ = "14-1";
                                _cc_ = "997";
                                break;
                            }
                        default:
                            break;
                    }


                    if (string.IsNullOrEmpty(m.centro_costos))
                    {
                        ccExist = true;
                    }

                    if (!ccExist)
                    {
                        //centro_costosName = getDescripcionLocalizacionEquipo(m.centro_costos);
                        string _centro_costo = null;
                        var _CCEmpresa = _ccsSigoplan.FirstOrDefault(x => x.areaCuenta == m.centro_costos);
                        if (_CCEmpresa == null)
                        {
                            _centro_costo = _ccsEnkontrol.FirstOrDefault(x => x.cc == m.centro_costos).descripcion;
                        }
                        else
                        {
                            _centro_costo = _CCEmpresa.descripcion;
                            _ac_ = _CCEmpresa.areaCuenta;
                            _cc_ = _CCEmpresa.cc;
                        }
                        centro_costosName = _centro_costo;
                    }
                    if (m.estatus != 1)
                    {
                        stb = "Stand BY";
                        CargoObra = "MAQUINARIA NO ASIGNADA A OBRA";
                        _ac_ = "14-1";
                        _cc_ = "997";
                    }
                    else
                    {
                        //var Reedireccion = _context.tblM_AsignacionEquipos.OrderByDescending(x => x.id).FirstOrDefault(x => x.noEconomicoID == m.id && x.estatus == 2 && x.estatus < 3);
                        //var Reedireccion = _redi.FirstOrDefault(x => x.noEconomicoID == m.id && x.estatus == 2 && x.estatus < 3);
                        var Reedireccion = _redi.FirstOrDefault(x => x.noEconomicoID == m.id && (x.estatus == 2 || x.estatus == 4));

                        if (!ccExist)
                        {
                            CargoObra = centro_costosName;// "MAQUINARIA NO ASIGNADA A OBRA";
                        }

                        string reedir = "";
                        if (Reedireccion != null)
                        {
                            //var acNombre = getDescripcionLocalizacionEquipo(Reedireccion.cc);
                            string _centro_costo = null;
                            var _CCEmpresa = _ccsSigoplan.FirstOrDefault(x => x.areaCuenta == Reedireccion.cc);
                            if (_CCEmpresa == null)
                            {
                                if (Reedireccion.cc == "1097") _centro_costo = "ENVÍO PROVEEDOR";
                                else _centro_costo = _ccsEnkontrol.FirstOrDefault(x => x.cc == Reedireccion.cc).descripcion;
                            }
                            else
                            {
                                _centro_costo = _CCEmpresa.descripcion;
                                _ac_ = _CCEmpresa.areaCuenta;
                                _cc_ = _CCEmpresa.cc;
                            }
                            var acNombre = _centro_costo;

                            centro_costosName = "EN TRANSITO A : " + acNombre;
                            stb = "TRANSITO";
                            //CargoObra = "MAQUINARIA NO ASIGNADA A OBRA";
                            CargoObra = acNombre;

                            try
                            {
                                if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    var fechaCorte = _context.tblM_CorteInventarioMaq.OrderByDescending(x => x.FechaCorte).FirstOrDefault().FechaCorte;
                                    if (fechaCorte.Date != DateTime.Now.Date)
                                    {
                                        //var eq = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico.Equals(m.noEconomico));
                                        var eq = m;
                                        eq.centro_costos = Reedireccion.cc;
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            catch (Exception e3) { }

                        }
                        else
                        {
                            //Reedireccion = _context.tblM_AsignacionEquipos.OrderByDescending(x => x.id).FirstOrDefault(x => x.noEconomicoID == m.id && x.estatus > 3 && x.estatus < 5);
                            Reedireccion = _redi.FirstOrDefault(x => x.noEconomicoID == m.id && x.estatus > 3 && x.estatus < 5);
                            if (Reedireccion != null)
                            {
                                try
                                {
                                    //reedir = getDescripcionLocalizacionEquipo(Reedireccion.cc);
                                    string _centro_costo = null;
                                    var _CCEmpresa = _ccsSigoplan.FirstOrDefault(x => x.areaCuenta == Reedireccion.cc);
                                    if (_CCEmpresa == null)
                                    {
                                        _centro_costo = _ccsEnkontrol.FirstOrDefault(x => x.cc == Reedireccion.cc).descripcion;
                                    }
                                    else
                                    {
                                        _centro_costo = _CCEmpresa.descripcion;
                                    }
                                    reedir = _centro_costo;
                                }
                                catch (Exception)
                                {
                                    reedir = "";

                                }
                            }
                        }
                    }


                    
      
                    //var redireccionamientoVenta = _context.tblM_CatMaquina.FirstOrDefault(x => x.id == m.id).redireccionamientoVenta;
                    var redireccionamientoVenta = m.redireccionamientoVenta;
                    var select = new inventarioGeneralDTO
                    {
                        Tipo = m.grupoMaquinaria.tipoEquipo.descripcion,
                        Economico = m.noEconomico,
                        Descripcion = m.descripcion,
                        Marca = m.marca != null ? m.marca.descripcion : "",
                        Modelo = m.modeloEquipo != null ? m.modeloEquipo.descripcion : "",
                        NumeroSerie = m.noSerie,
                        Anio = m.anio.ToString(),
                        Ubicacion = centro_costosName,
                        Redireccion = (redireccionamientoVenta ? "Equipo para venta" : stb),
                        cc = _cc_,
                        ccCargoObra = _ac_,
                        CargoObra = CargoObra,
                        idEconomico = m.id,
                        HorometroAcumulado = horasAcumuladas,
                        empresaID = m.empresa,
                        empresa = (m.empresa == 0 ? "" : ((EmpresaEnum)m.empresa).GetDescription()),
                        Estatus = lstCapturaDatosDiarios.Where(r => r.noEconomico == m.noEconomico).FirstOrDefault() == null ? "" : switchEstatus(lstCapturaDatosDiarios.Where(r => r.noEconomico == m.noEconomico).FirstOrDefault().idEstatus)
                    };

                    Result.Add(select);
                }
            }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return Result;
        }

        public string switchEstatus(int id)
        {
            string texto = "";
            switch (id)
            {
                case (int)DatosDiariosEnum.OVERHAUL:
                    texto = "Overhaul";
                    break;
                case (int)DatosDiariosEnum.REHABILITACION:
                    texto = "Equipo en espera de rehabilitación ";
                    break;
                case (int)DatosDiariosEnum.REHABILITACIONTMC:
                    texto = "Equipo en rehabilitación en TMC";
                    break;
                case (int)DatosDiariosEnum.DISPONIBLEOBRA:
                    texto = "Equipo disponible para obra";
                    break;
                case (int)DatosDiariosEnum.DISPONIBLEVENTA:
                    texto = "Equipo disponible para venta";
                    break;
            }
            return texto;
        }

        public bool CorteInventarioEnviado(int? tipo)
        {
            var fecha = DatetimeUtils.UltimoDiaSemanaCorte_Martes(DateTime.Now);
            var fechaSinHoras = new DateTime(fecha.Year, fecha.Month, fecha.Day);
            return _context.tblM_CorteInventarioMaq.Any(a => a.FechaCorte == fechaSinHoras && a.Estatus && a.IdTipoMaquina == null);
            //if (tipo == null)
            //{
            //    return _context.tblM_CorteInventarioMaq.Any(a => a.FechaCorte == fechaSinHoras && a.Estatus && a.IdTipoMaquina == null);
            //}
            //else
            //{
            //    return _context.tblM_CorteInventarioMaq.Any(a => a.FechaCorte == fechaSinHoras && a.Estatus && (a.IdTipoMaquina == null || a.IdTipoMaquina == tipo.Value));
            //}

        }

        public bool guardarCorteInventario(tblM_CorteInventarioMaq corte)
        {
            //var existeCorte = _context.tblM_CorteInventarioMaq.Any(x => x.FechaCorte == corte.FechaCorte && x.Estatus && x.IdTipoMaquina == corte.IdTipoMaquina);
            var existeCorte = _context.tblM_CorteInventarioMaq.Any(x => x.FechaCorte == corte.FechaCorte && x.Estatus && x.IdTipoMaquina == null);
            if (existeCorte)
            {
                return false;
            }
            _context.tblM_CorteInventarioMaq.Add(corte);
            _context.SaveChanges();
            return true;
        }

        private string getDescripcionLocalizacionEquipo(string centro_costosName)
        {
            if (vSesiones.sesionEmpresaActual != 1)
            {
                var _CCEmpresa = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == centro_costosName);
                if (_CCEmpresa != null)
                {
                    centro_costosName = _CCEmpresa.descripcion;
                }
            }
            else
            {
                var _CCEmpresa = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == centro_costosName);
                if (_CCEmpresa != null)
                {
                    centro_costosName = _CCEmpresa.descripcion;
                }
                else
                {
                    var resultado = (IList<economicoDTO>)ContextEnKontrolNominaArrendadora.Where("SELECT descripcion FROM cc WHERE cc = '" + centro_costosName + "';", 1).ToObject<IList<economicoDTO>>();
                    return resultado.FirstOrDefault().descripcion;
                }
            }

            return centro_costosName;

        }

        public List<ListaAnexosDTO> GetListaAnexos(List<string> CCs, int grupo, int Economico, int tipo)
        {
            if (vSesiones.sesionEmpresaActual == 3) 
            {
                var _CCs = _context.tblP_CC.Where(x => CCs.Contains(x.cc)).ToList();
                CCs = _CCs.Select(x => x.areaCuenta).ToList();
            }
            
            var data2 = _context.tblM_CatMaquina.ToList().Where(x => (CCs != null ? CCs.Contains(x.centro_costos) : x.id == x.id) && (grupo != 0 ? grupo == x.grupoMaquinariaID : x.id == x.id) &&
                            (Economico != 0 ? Economico == x.id : x.id == x.id) && (tipo != 0 ? tipo == x.grupoMaquinaria.tipoEquipoID : x.id == x.id) && !string.IsNullOrEmpty(x.noEconomico)
                            && x.estatus != 0
                            ).ToList();

          
            //List<int> EconomicosAnsul =  new List<int>(){18, 89, 90, 91, 97, 539, 665, 1676, 1677, 1683, 3780, 3781, 3810, 3836, 3853};
         
            

            List<ListaAnexosDTO> ltsResult = new List<ListaAnexosDTO>();

            foreach (var maquina in data2.OrderBy(x => x.noEconomico))
            {
                
                ListaAnexosDTO dato = new ListaAnexosDTO();
                dato.PuedeAnsul = false;
                var item = _context.tblM_DocumentosMaquinaria.Where(x => x.economicoID == maquina.id).ToList();

                dato.noEconomico = maquina.noEconomico;

                var equiposModeloPuedenAnsul = _context.tblM_CatMaquina.FirstOrDefault(x => x.noEconomico == maquina.noEconomico);
                var ModelosPuedeAnsul = _context.tblM_EconomicoPuedeAnsul.Where(x => x.registroActivo && x.modelEquipoId == equiposModeloPuedenAnsul.modeloEquipoID).FirstOrDefault();
                if (ModelosPuedeAnsul != null)
                {
                    dato.PuedeAnsul = true;
                }

                    var FacturaRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 1);
                    var PedimentoRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 2);
                    var PolizaRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 3);
                    var TarCirculacionRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 4);
                    var PerEspecialCargaRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 5);
                    var CertificacionRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 6);
                    var CuadroComparativoRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 7);
                    var ContratosRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 8);
                    var AnsulRaw = item.OrderByDescending(x => x.id).FirstOrDefault(x => x.tipoArchivo == 9);

                    dato.noEconomicoID = maquina.id;
                    dato.Factura = getInfoAnexoDTO(FacturaRaw);
                    dato.Pedimento = getInfoAnexoDTO(PedimentoRaw);
                    dato.Poliza = getInfoAnexoDTO(PolizaRaw);
                    dato.TarCirculacion = getInfoAnexoDTO(TarCirculacionRaw);
                    dato.Certificacion = getInfoAnexoDTO(CertificacionRaw);
                    dato.PerEspecialCarga = getInfoAnexoDTO(PerEspecialCargaRaw);
                    dato.CuadroComparativo = getInfoAnexoDTO(CuadroComparativoRaw);
                    dato.Contratos = getInfoAnexoDTO(ContratosRaw);
                    dato.Ansul = getInfoAnexoDTO(AnsulRaw);
                    var docs = _context.tblM_CatMaquina_DocumentosAplica.FirstOrDefault(x => x.grupoID == maquina.grupoMaquinariaID);
                    if (docs == null)
                    {
                        dato.vFactura = dato.Factura.id == 0 ? "NO" : "SI";
                        dato.vPedimento = dato.Pedimento.id == 0 ? "NO" : "SI";
                        dato.vPoliza = dato.Poliza.id == 0 ? "NO" : "SI";
                        dato.vTarCirculacion = dato.TarCirculacion.id == 0 ? "NO" : "SI";
                        dato.vCertificacion = dato.Certificacion.id == 0 ? "NO" : "SI";
                        dato.vPerEspecialCarga = dato.PerEspecialCarga.id == 0 ? "NO" : "SI";
                        dato.vCuadroComparativo = dato.CuadroComparativo.id == 0 ? "NO" : "SI";
                        dato.vContratos = dato.Contratos.id == 0 ? "NO" : "SI";
                        dato.vAnsul = dato.Ansul.id == 0 ? "NO" : "SI";
                    }
                    else
                    {
                        dato.vFactura = (!docs.factura ? "N/A" : dato.Factura.id == 0 ? "NO" : "SI");
                        dato.vPedimento = (!docs.pedimento ? "N/A" : dato.Pedimento.id == 0 ? "NO" : "SI");
                        dato.vPoliza = (!docs.polizaSeguro ? "N/A" : dato.Poliza.id == 0 ? "NO" : "SI");
                        dato.vTarCirculacion = (!docs.tarjetaCirculacion ? "N/A" : dato.TarCirculacion.id == 0 ? "NO" : "SI");
                        dato.vCertificacion = (!docs.certificacion ? "N/A" : dato.Certificacion.id == 0 ? "NO" : "SI");
                        dato.vPerEspecialCarga = (!docs.permisoCarga ? "N/A" : dato.PerEspecialCarga.id == 0 ? "NO" : "SI");
                        dato.vCuadroComparativo = (!docs.cuadroComparativo ? "N/A" : dato.CuadroComparativo.id == 0 ? "NO" : "SI");
                        var esContrato = (maquina.grupoMaquinaria.tipoEquipoID == 1 || maquina.grupoMaquinaria.tipoEquipoID == 3 || maquina.renta) ? true : false;
                        dato.vContratos = (!esContrato ? "N/A" : dato.Contratos.id == 0 ? "NO" : "SI");
                        dato.vAnsul = (!docs.ansul ? "N/A" : dato.Ansul.id == 0 ? "NO" : "SI");
                    }
                
                ltsResult.Add(dato);
            }
            return ltsResult;
        }

        public List<ComboDTO> fillCboNoEconomicosCC(List<string> cc)
        {
            switch ((EmpresaEnum)vSesiones.sesionEmpresaActual)
            {
                case EmpresaEnum.Peru:
                    {
                        var areasCuentas = _context.tblP_CC.Where(x => cc.Contains(x.areaCuenta)).ToList();
                        cc = areasCuentas.Select(x => x.areaCuenta).ToList();
                    }
                    break;
            }

            var listaEconomicos = _context.tblM_CatMaquina.Where(x => cc.Contains(x.centro_costos) && x.estatus != 0).Select(x => new ComboDTO { Text = x.noEconomico, Value = x.id.ToString() }).OrderBy(x => x.Text).ToList();

            return listaEconomicos;
        }

        private AnexoMaquinaDTO getInfoAnexoDTO(tblM_DocumentosMaquinaria obj)
        {
            AnexoMaquinaDTO objReturn = new AnexoMaquinaDTO();
            if (obj != null)
            {
                objReturn.id = obj.id;
                objReturn.nombre = obj.nombreArchivo;
                objReturn.rutaArchivo = obj.nombreRuta;
                objReturn.tipo = obj.tipoArchivo;
            }
            return objReturn;
        }


        #region Consultas Generales
        public List<ComboDTO> fillCboNoEconomicos(string cc)
        {
            return _context.tblM_CatMaquina.Where(x => x.centro_costos == cc && x.estatus != 0).Select(r => new ComboDTO { Value = r.id.ToString(), Text = r.noEconomico }).ToList();
        }


        #endregion

        public List<RepCargoNominaCCArreDTO> GetEconomicos(List<string> arrProyectos, string periodoInicial, string periodoFinal)
        {
            DateTime hoy = new DateTime();
            DateTime primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);

            var inicial = (periodoInicial != "" ? DateTime.Parse(periodoInicial) : primerDiaMes).Date;
            var final = (periodoFinal != "" ? DateTime.Parse(periodoFinal) : hoy).Date;

            vSesiones.sesionEmpresaActual = 2;
            var consulta = @"SELECT cc, descripcion as noEconomico FROM DBA.cc";
            var ccEnkontrol = (List<RepCargoNominaCCArreDTO>)ContextArrendadora.Where(consulta).ToObject<List<RepCargoNominaCCArreDTO>>();
            var lstMaquinaria = _context.tblM_CatMaquina.Where(x => arrProyectos.Contains(x.centro_costos)).Select(x => new { x.id, x.noEconomico, x.centro_costos, x.descripcion }).ToList();
            var lstOrdenesTrabajo = _context.tblM_CapOrdenTrabajo.Where(ot => ot.FechaEntrada >= inicial && ot.FechaEntrada <= final).Select(x => new { x.id, x.EconomicoID }).ToList();

            var obj = (from eco in lstMaquinaria
                       join ot in lstOrdenesTrabajo on eco.id equals ot.EconomicoID
                       join detOT in _context.tblM_DetOrdenTrabajo.ToList() on ot.id equals detOT.OrdenTrabajoID
                       select new RepCargoNominaCCArreDTO
                       {
                           economicoID = eco.id,
                           noEconomico = eco.noEconomico,
                           descripcion = eco.descripcion,
                           cc = ccEnkontrol.Count > 0 ? ccEnkontrol.FirstOrDefault(w => w.noEconomico == eco.noEconomico) != null ? ccEnkontrol.FirstOrDefault(w => w.noEconomico == eco.noEconomico).cc : "" : "",
                           hhPeriodo = Convert.ToDecimal((detOT.HoraFin - detOT.HoraInicio).TotalHours, CultureInfo.InvariantCulture)
                       }).ToList();

            var objGroup = obj.GroupBy(x => x.noEconomico).Select(y => new RepCargoNominaCCArreDTO
            {
                economicoID = y.FirstOrDefault().economicoID,
                noEconomico = y.FirstOrDefault().noEconomico,
                descripcion = y.FirstOrDefault().descripcion,
                cc = y.FirstOrDefault().cc,
                hhPeriodo = Math.Round((y.Select(w => w.hhPeriodo).Sum() / 100) * 100, 2)
            }).ToList();

            return objGroup;
        }

        public string GetProyectosString(List<string> arrProyectos)
        {
            var proyectos = _context.tblP_CC.Where(x => arrProyectos.Contains(x.areaCuenta)).ToList();
            var cadena = "";
            var contador = 1;
            var lst = new List<string>();
            foreach (var pro in proyectos)
            {
                if (contador > 1)
                {
                    cadena += ", " + pro.descripcion.Trim();
                    lst.Add(", " + pro.areaCuenta.Trim());
                }
                else
                {
                    cadena += pro.descripcion.Trim();
                    lst.Add(pro.areaCuenta.Trim());
                }
                contador++;
            }
            vSesiones.sesionArrProyectos = lst;
            return cadena;
        }
        public bool checkPeriodoCapturado(string ac, DateTime ini, DateTime fin)
        {
            return _context.tblM_CapNominaCC.ToList().FirstOrDefault(x => x.ac == ac && x.periodoInicial.Date == ini.Date && x.periodoFinal.Date == fin.Date) != null;
        }
        public List<NominaCCDTO> GetNominaCCGuardados(string fechaCaptura, string proyecto, int estatus)
        {
            bool estatusNominaCC = false;
            bool isVerificado = false;
            string getAreaCuenta = "";
            string nombreAC = "";
            if (!string.IsNullOrEmpty(proyecto))
            {
                var ccIDObj = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == proyecto);

                if (ccIDObj != null)
                {
                    int idCC = Convert.ToInt32(ccIDObj.id);
                    getAreaCuenta = _context.tblP_CC.FirstOrDefault(x => x.id == idCC).areaCuenta;
                    nombreAC = _context.tblP_CC.FirstOrDefault(x => x.id == idCC).descripcion;
                }


            }
            switch (estatus)
            {
                case 0:

                    isVerificado = false;
                    estatusNominaCC = true;
                    break;
                case 1:
                    isVerificado = true;
                    estatusNominaCC = true;
                    break;
                default:
                    break;
            }
            fechaCaptura = fechaCaptura == "--Seleccione--" ? "" : fechaCaptura;
            var fechaEntrada = !string.IsNullOrEmpty(fechaCaptura) ? Convert.ToDateTime(fechaCaptura.Split('-')[0]) : default(DateTime);
            var fechaSalida = !string.IsNullOrEmpty(fechaCaptura) ? Convert.ToDateTime(fechaCaptura.Split('-')[1]) : default(DateTime);
            var proyectosNominaCC = _context.tblM_CapNominaCC_Proyectos.Where(x => x.estatus).Select(y => new NominaCCProyectosDTO
            {
                id = y.id,
                areaCuenta = y.areaCuenta,
                capNominaCCID = y.capNominaCCID
            }).ToList();
            var lstNominaCC = _context.tblM_CapNominaCC.ToList();
            var lstNoInv = Enum.GetValues(typeof(CCNoInventarioHH)).Cast<CCNoInventarioHH>().ToList().Select(x => x.GetDescription()).ToList();
            var consulta = (from a in proyectosNominaCC
                            join b in lstNominaCC on a.capNominaCCID equals b.id
                            where b.isVerificado == isVerificado && b.estatus == estatusNominaCC &&
                                   (fechaCaptura != "" ? (b.periodoInicial == fechaEntrada && b.periodoFinal == fechaSalida) : true) &&
                                   (getAreaCuenta != "" ? a.areaCuenta == getAreaCuenta : true)
                            select new NominaCCDTO
                             {
                                 id = b.id,
                                 periodoInicial = b.periodoInicial,
                                 periodoFinal = b.periodoFinal,
                                 nominaSemanal = b.nominaSemanal,
                                 archivo = b.archivo,
                                 fechaCaptura = b.fechaCaptura,
                                 proyectosString = _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == b.ac) != null ? _context.tblP_CC.FirstOrDefault(x => x.areaCuenta == b.ac).descripcion : "",
                                 isVerificado = b.isVerificado,
                                 totalHH = proyectosNominaCC.Count > 0 ? getTotalHH(proyectosNominaCC.Where(z => z.capNominaCCID == b.id).Select(w => w.capNominaCCID).ToList()) : 0,
                                 lblVerificado = string.Format("{0}{1}", b.isVerificado ? string.Empty : "No ", proyectosNominaCC.Any(c => lstNoInv.Any(i => i.Equals(c.areaCuenta) && b.id.Equals(c.capNominaCCID))) ? "Virtualizado" : "Verificado")
                             }).ToList();
            return consulta;
        }

        private decimal getTotalHH(List<int> list)
        {
            decimal sumaTotal = 0;
            foreach (var item in list)
            {
                try
                {
                    var suma = _context.tblM_CapNominaCC_Detalles.Where(x => x.idNomina == item).Sum(y => y.hh);
                    sumaTotal = suma;
                }
                catch
                {
                    sumaTotal = 0;
                }
            }

            return sumaTotal;
        }
        public bool existPeriodoNomina(string ac, DateTime ini, DateTime fin)
        {

            try
            {
                return _context.tblM_CapNominaCC.Where(w => w.estatus && w.periodoInicial.Equals(ini) && w.periodoFinal.Equals(fin) && w.ac == ac).ToList().Count > 1;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public tblM_CapNominaCC getNominaCC(string ac, DateTime ini, DateTime fin)
        {
            return _context.tblM_CapNominaCC.FirstOrDefault(n => n.periodoInicial.Equals(ini) && n.periodoFinal.Equals(fin) && n.ac == ac);
        }
        public tblM_CapNominaCC getNominaCC(int id)
        {
            return _context.tblM_CapNominaCC.FirstOrDefault(n => n.id.Equals(id));
        }
        public string getNominaCCProyectos(int id)
        {
            var proyectosNominaCC = _context.tblM_CapNominaCC_Proyectos.Where(w => w.capNominaCCID.Equals(id)).ToList();
            return proyectosNominaCC.Count > 0 ? GetProyectosString(proyectosNominaCC.Select(w => w.areaCuenta).ToList()) : "";
        }
        public List<string> getNominaCCArrProyectos(int id)
        {
            var proyectosNominaCC = _context.tblM_CapNominaCC_Proyectos.Where(w => w.capNominaCCID.Equals(id)).Select(w => w.areaCuenta.Trim()).ToList();
            vSesiones.sesionArrProyectos = proyectosNominaCC;
            return proyectosNominaCC.Count > 0 ? proyectosNominaCC : new List<string>();
        }
        public List<tblM_CapNominaCC_Proyectos> getNominaCCLstProyectos(int id)
        {
            var proyectosNominaCC = _context.tblM_CapNominaCC_Proyectos.Where(w => w.capNominaCCID.Equals(id)).ToList();
            vSesiones.sesionArrProyectos = proyectosNominaCC.Select(w => w.areaCuenta.Trim()).ToList();
            return proyectosNominaCC.Count > 0 ? proyectosNominaCC : new List<tblM_CapNominaCC_Proyectos>();
        }
        public List<tblM_CapNominaCC_Detalles> getNominaCCDet(int id)
        {
            return _context.tblM_CapNominaCC_Detalles.Where(d => d.idNomina.Equals(id)).ToList();
        }
        public void GuardarCargoNominaCC(List<byte[]> downloadPDF)
        {
            var arrProyectos = vSesiones.sesionArrProyectos;
            var periodoInicial = vSesiones.sesionPeriodoInicial;
            var periodoFinal = vSesiones.sesionPeriodoFinal;
            var nominaSemanal = vSesiones.sesionNominaSemanal;
            var lstDet = vSesiones.sesionNominaCCDetalles;
            tblM_CapNominaCC lastInserted = new tblM_CapNominaCC();
            int lastInsertedID = 1;
            try
            {
                lastInserted = _context.tblM_CapNominaCC.OrderByDescending(x => x.id).FirstOrDefault();
            }
            catch (Exception)
            {

                lastInsertedID = 1;
            }


            if (lastInserted != null)
            {
                lastInsertedID = lastInserted.id;
            }
            var nombre = "Cargos Nómina " + periodoInicial.Replace("/", "_") + " " + periodoFinal.Replace("/", "_") + "_" + fillNo((lastInserted != null ? (lastInsertedID + 1).ToString() : lastInsertedID.ToString()), 8);

            string ruta = archivofs.getArchivo().getUrlDelServidor(7) + nombre + ".pdf";
            byte[] array = downloadPDF
                .SelectMany(a => a)
                .ToArray();
            File.WriteAllBytes(ruta, array);

            tblM_CapNominaCC capNomina = new tblM_CapNominaCC();

            capNomina.periodoInicial = DateTime.Parse(periodoInicial);
            capNomina.periodoFinal = DateTime.Parse(periodoFinal);
            capNomina.nominaSemanal = Convert.ToDecimal(nominaSemanal, CultureInfo.InvariantCulture);
            capNomina.archivo = nombre;
            capNomina.fechaCaptura = DateTime.Now;
            capNomina.estatus = true;
            capNomina.isVerificado = false;
            capNomina.ac = arrProyectos.FirstOrDefault();

            _context.tblM_CapNominaCC.Add(capNomina);
            _context.SaveChanges();

            foreach (var pro in arrProyectos)
            {
                var cc = _context.tblP_CC.Where(x => x.areaCuenta == pro).FirstOrDefault();

                tblM_CapNominaCC_Proyectos capNominaPro = new tblM_CapNominaCC_Proyectos();

                capNominaPro.areaCuenta = pro;
                capNominaPro.capNominaCCID = capNomina.id;
                capNominaPro.estatus = true;

                _context.tblM_CapNominaCC_Proyectos.Add(capNominaPro);

            }
            lstDet.ForEach(d =>
            {
                d.idNomina = capNomina.id;
                _context.tblM_CapNominaCC_Detalles.Add(d);
                _context.SaveChanges();
            });
            var lstUsuario = new List<int>() { 1096, 1097, 1123, 3314, 1126 }; //alerta a nóminas

            List<int> listUsuarios = (from a in _context.tblP_Autoriza
                                      join b in _context.tblP_CC_Usuario
                                      on a.cc_usuario_ID equals b.id
                                      where a.perfilAutorizaID == 8 && b.cc == capNomina.ac
                                      select a.usuarioID).ToList();

            List<int> listUsuarios2 = (from a in _context.tblP_Autoriza
                                       join b in _context.tblP_CC_Usuario
                                       on a.cc_usuario_ID equals b.id
                                       where a.perfilAutorizaID == 5 && b.cc == capNomina.ac
                                       select a.usuarioID).ToList();

            List<int> listUsuarios3 = (from a in _context.tblP_Autoriza
                                       join b in _context.tblP_CC_Usuario
                                       on a.cc_usuario_ID equals b.id
                                       where a.perfilAutorizaID == 1 && b.cc == capNomina.ac
                                       select a.usuarioID).ToList();


            lstUsuario.AddRange(listUsuarios);

            lstUsuario.AddRange(listUsuarios2);

            lstUsuario.AddRange(listUsuarios3);
            lstUsuario.Add(vSesiones.sesionUsuarioDTO.id);
            lstUsuario.Add(6); //Luis Fortino

            lstUsuario.Add(6561); // Berenice Duarte

            lstUsuario.Remove(1123);
            lstUsuario.Distinct().ToList().ForEach(u =>
            {
                setAlerta(u, capNomina.id, true);
                sendCorreo(u, vSesiones.sesionUsuarioDTO.id, downloadPDF, nombre);
            });
        }
        public void ActualizarCargoNominaCC(List<byte[]> downloadPDF, tblM_CapNominaCC nomina, List<tblM_CapNominaCC_Detalles> lstDet)
        {
            var upNom = _context.tblM_CapNominaCC.FirstOrDefault(n => n.id.Equals(nomina.id));
            upNom.isVerificado = nomina.isVerificado;
            upNom.nominaSemanal = nomina.nominaSemanal;
            _context.SaveChanges();
            string ruta = archivofs.getArchivo().getUrlDelServidor(7) + upNom.archivo + ".pdf";
            byte[] array = downloadPDF
                .SelectMany(a => a)
                .ToArray();
            File.WriteAllBytes(ruta, array);
            lstDet.ForEach(d =>
            {
                var upDet = _context.tblM_CapNominaCC_Detalles.FirstOrDefault(w => w.id.Equals(d.id));
                upDet.cargoP = d.cargoP;
                upDet.cargoD = d.cargoD;
                _context.SaveChanges();
            });
            var lstCC = _context.tblM_CapNominaCC_Proyectos.Where(w => w.capNominaCCID.Equals(nomina.id)).ToList();
            var lstNoInv = Enum.GetValues(typeof(CCNoInventarioHH)).Cast<CCNoInventarioHH>().ToList().Select(x => x.GetDescription()).ToList();
            var pcc = (from a in _context.tblP_Autoriza
                       join b in _context.tblP_CC_Usuario on a.cc_usuario_ID equals b.id
                       where ((a.perfilAutorizaID == 1

                           || a.perfilAutorizaID == 5) && b.cc.Equals(upNom.ac))
                       select a.usuarioID).ToList();

            var lstUsuario = new List<int>() { 3314/*, 1123*/ }; //alerta a todos
            lstUsuario.AddRange(pcc);
            if (!lstCC.Any(c => lstNoInv.Any(i => i.Equals(c.areaCuenta))))
                lstUsuario.AddRange(new List<int>() { 1055, 1054, 6247 });//almacen
            lstUsuario.Remove(1123);
            lstUsuario.Add(6561); // Berenice Duarte
            if (upNom.isVerificado)
                lstUsuario.ForEach(u =>
                {
                    //setAlerta(u, nomina.id, false);
                    sendCorreo(u, vSesiones.sesionUsuarioDTO.id, downloadPDF, string.Empty);
                });
        }
        void setAlerta(int idUsuario, int idCapNomia, bool isSave)
        {
            var ultimo = _context.tblP_Alerta.OrderByDescending(o => o.id).FirstOrDefault().id;
            var a = new tblP_Alerta()
            {
                objID = idCapNomia,
                userEnviaID = vSesiones.sesionUsuarioDTO.id,
                userRecibeID = idUsuario,
                sistemaID = 1,
                tipoAlerta = 2,
                url = isSave ? "/RepCargoNominaCCArrendadora/Gestion" : "\\usuario\removerAlerta/?idAlerta=" + ultimo.ToString(),
                msj = string.Format("Cargo Nomina")
            };
            _context.tblP_Alerta.Add(a);
            SaveChanges();
        }
        void sendCorreo(int iduserRecibe, int iduserEnvia, List<Byte[]> pdf, string pdfNombre)
        {
            try
            {
                var usuarioEnvia = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserEnvia));
                var usuarioRecibe = _context.tblP_Usuario.FirstOrDefault(w => w.id.Equals(iduserRecibe));
                List<string> CorreoEnviar = new List<string>();
                string AsuntoCorreo = @"<html>
                                            <head>
                                                <style>
                                                    table {
                                                        font-family: arial, sans-serif;
                                                        border-collapse: collapse;
                                                        width: 100%;
                                                    }

                                                    td, th {
                                                        border: 1px solid #dddddd;
                                                        text-align: left;
                                                        padding: 8px;
                                                    }

                                                    tr:nth-child(even) {
                                                        background-color: #dddddd;
                                                    }
                                                </style>
                                            </head>
                                            <body lang=ES-MX link='#0563C1' vlink='#954F72'>
                                                <div class=WordSection1>
                                                    <p class=MsoNormal>
                                                        Buen día <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>";
                AsuntoCorreo += @" <p class=MsoNormal>Se informa que se registro un nuevo cargo de nómina por el empleado " + usuarioEnvia.nombre + " " + usuarioEnvia.apellidoPaterno + " " + usuarioEnvia.apellidoMaterno + ".<o:p></o:p></p>";
                CorreoEnviar.Add(usuarioRecibe.correo);
                AsuntoCorreo += @"</tbody></table>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Favor de ingresar al sistema SIGOPLAN (<a href='http://sigoplan.construplan.com.mx/'>http://sigoplan.construplan.com.mx/</a>), en el apartado de MAQUINARÍA, menú Mesa de Analisis, Reportes en la opción Géstion a Cargo Nómina.<o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        PD. Se informa que esta es una notificación autogenerada por el sistema SIGOPLAN no es necesario dar una respuesta <o:p></o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        <o:p>&nbsp;</o:p>
                                                    </p>
                                                    <p class=MsoNormal>
                                                        Gracias.<o:p></o:p>
                                                    </p>
                                                </div>
                                            </body>
                                        </html>";
                var tipoFormato = pdfNombre + ".pdf";
                GlobalUtils.sendEmailAdjuntoInMemory2(string.Format("{0}: {1}", PersonalUtilities.GetNombreEmpresa(), "Alerta de Cargo de Nóminas"), AsuntoCorreo, CorreoEnviar.Distinct().ToList(), pdf, tipoFormato);
            }
            catch (Exception) { }
        }
        public string fillNo(string e, int no)
        {
            var newe = "";
            var el = e.Length;
            if (e.Length < no)
            {
                for (int i = el; i < no; i++)
                {
                    newe += "0";
                }
                return newe + e;
            }
            else
            {
                return e;
            }
        }

        public string GetCCNomina(List<string> arrProyectos)
        {
            var proyectos = _context.tblP_CC.Where(x => arrProyectos.Contains(x.areaCuenta)).Select(y => y.descripcionRH.Trim()).ToList();
            var cadena = "";
            var contador = 1;

            foreach (var pro in proyectos)
            {
                if (contador > 1)
                {
                    if (pro != "" && pro != null)
                    {
                        if (cadena != "" && cadena != null)
                        {
                            cadena += ", " + pro;
                        }
                        else
                        {
                            cadena += pro;
                        }
                    }
                    else
                    {
                        cadena += pro;
                    }
                }
                else
                {
                    cadena = pro;
                }

                contador++;
            }

            return cadena;
        }

        public Dictionary<string, object> ObtenerNominaMensualCC(string[] areaCuentaArray, int mes, int año, int estatus)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                var listaNominaMensualCC = new List<NominaMensualCCDTO>();
                listaNominaMensualCC = CargarNominasMensuales(areaCuentaArray, mes, año, estatus);
                resultado.Add("data", listaNominaMensualCC);
            }
            catch (Exception)
            {
                resultado.Add("data", new List<NominaMensualCCDTO>());
                return resultado;
            }
            finally
            {
                _context.Configuration.AutoDetectChangesEnabled = true;
            }

            return resultado;
        }

        private List<NominaMensualCCDTO> CargarNominasMensuales(string[] areaCuentaArray, int mes, int año, int estatus)
        {
            // Se descarta el tracking de cambios en context para mejorar el rendimiento.
            _context.Configuration.AutoDetectChangesEnabled = false;

            bool estadoNomina = (estatus != 2) ? Convert.ToBoolean(estatus) : false;

            var listaNominaMensualCC = new List<NominaMensualCCDTO>();

            foreach (var areaCuenta in areaCuentaArray)
            {
                tblP_CC areaCuentaObj = null;

                // Se valida area cuenta
                if (!string.IsNullOrEmpty(areaCuenta))
                {
                    areaCuentaObj = _context.tblP_CC.FirstOrDefault(cc => cc.areaCuenta == areaCuenta);

                    // Si no encuentra la ac, salta la iteración.
                    if (areaCuentaObj == null)
                    {
                        continue;
                    }
                }

                // Si ya hay registros con ese periodo de fechas, ya no se consultan las HH.
                var noHayRegistros = !_context.tblM_CapNominaMensualCC.Any(nomina => nomina.areaCuentaID == areaCuentaObj.id && nomina.mes == mes && nomina.año == año);

                // Se verifica si ya hay registros guardados con ese periodo.
                var nominaExistente = _context.tblM_CapNominaMensualCC
                    .FirstOrDefault(nomina =>
                        nomina.areaCuentaID == areaCuentaObj.id &&
                        nomina.mes == mes &&
                        nomina.año == año);

                if (nominaExistente != null)
                {
                    // Si no trae los filtros de estado correspondientes, salta la iteración.
                    if ((estatus != 2 ? nominaExistente.completo != Convert.ToBoolean(estatus) : false))
                    {
                        continue;
                    }
                    var h = GetHorasTrabajoMensual(areaCuenta, mes, año);
                    listaNominaMensualCC.Add(new NominaMensualCCDTO
                    {
                        id = areaCuentaObj.id,
                        nominaID = nominaExistente.id,
                        nominaIMSS = nominaExistente.nominaIMSS,
                        nominaInfonavit = nominaExistente.nominaInfonavit,
                        ISN = nominaExistente.ISN,
                        ISR = nominaExistente.ISR,
                        mes = ObtenerNombreMes(mes),
                        año = año.ToString(),
                        areaCuenta = areaCuenta,
                        proyecto = areaCuenta + "-" + areaCuentaObj.descripcion,
                        estatus = nominaExistente.completo ? "Completo" : "En espera",
                        horasHombreTotales = h
                    });
                }
                // Si no hay registro existente, se hace la consulta para calcular todas las HH en el periodo.
                else
                {
                    if (noHayRegistros)
                    {
                        // Si no tiene registro de nomina y se aplicó como filtro por capturados, salta la iteración.
                        if (estadoNomina)
                        {
                            continue;
                        }

                        decimal horasTrabajadas = GetHorasTrabajoMensual(areaCuenta, mes, año);
                        if (horasTrabajadas == 0)
                        {
                            continue;
                        }

                        listaNominaMensualCC.Add(new NominaMensualCCDTO
                        {
                            id = areaCuentaObj.id,
                            mes = ObtenerNombreMes(mes),
                            año = año.ToString(),
                            areaCuenta = areaCuenta,
                            proyecto = areaCuenta + "-" + areaCuentaObj.descripcion,
                            estatus = "En Espera",
                            horasHombreTotales = horasTrabajadas,
                        });
                    }
                }
            }

            _context.Configuration.AutoDetectChangesEnabled = true;

            return listaNominaMensualCC;
        }

        private decimal GetHorasTrabajoMensual(string areaCuenta, int mes, int año)
        {
            _context.Configuration.AutoDetectChangesEnabled = false;

            // Se obtiene el rango de fechas del mes
            var fechasMes = ObtenerFechasPorMesAño(año, mes);
            var primerDiaMes = fechasMes.FirstOrDefault();
            var ultimoDiaMes = fechasMes.LastOrDefault();

            vSesiones.sesionEmpresaActual = 2;

            var obj = (from eco in _context.tblM_CatMaquina.ToList()
                       join ot in _context.tblM_CapOrdenTrabajo.ToList() on eco.id equals ot.EconomicoID
                       join detOT in _context.tblM_DetOrdenTrabajo.ToList() on ot.id equals detOT.OrdenTrabajoID
                       where areaCuenta == ot.CC && (ot.FechaEntrada.Date >= primerDiaMes && ot.FechaEntrada.Date <= ultimoDiaMes)
                       select new RepCargoNominaCCArreDTO
                       {
                           noEconomico = eco.noEconomico,
                           hhPeriodo = Convert.ToDecimal((detOT.HoraFin - detOT.HoraInicio).TotalHours, CultureInfo.InvariantCulture)
                       });

            if (obj.Count() == 0)
            {
                return 0;
            }

            var objGroup = obj.GroupBy(x => x.noEconomico).Select(y => new RepCargoNominaCCArreDTO
            {
                hhPeriodo = Math.Round((y.Select(w => w.hhPeriodo).Sum() / 100) * 100, 2)
            });

            return objGroup.Count() > 0 ? objGroup.Select(x => x.hhPeriodo).Sum() : 0;
        }

        private static string ObtenerNombreMes(int numeroMes)
        {
            if (numeroMes < 0 || numeroMes > 12)
            {
                return "Número de mes inválido";
            }
            string[] meses = DateTimeFormatInfo.CurrentInfo.MonthNames;
            string mes = meses[--numeroMes];
            mes = mes.First().ToString().ToUpper() + mes.Substring(1);
            return mes;
        }

        private static List<DateTime> ObtenerFechasPorMesAño(int año, int mes)
        {
            var fechasMes = Enumerable.Range(1, DateTime.DaysInMonth(año, mes))  // Days: 1, 2 ... 31 etc.
                            .Select(day => new DateTime(año, mes, day)) // Map each day to a date
                            .ToList(); // Load dates into a list

            DateTime primerDia = fechasMes.FirstOrDefault();
            DateTime ultimoDia = fechasMes.LastOrDefault();

            List<DateTime> listaFechas = new List<DateTime>()
            {
                primerDia,ultimoDia
            };

            return listaFechas;
        }

        public Dictionary<string, object> GuardarNominaMensualCC(List<NominaMensualCCDTO> nominasProyectos, int mes, int año)
        {
            var resultado = new Dictionary<string, object>();
            try
            {
                foreach (var nomina in nominasProyectos)
                {
                    var listaValores = new List<decimal> { nomina.nominaIMSS, nomina.nominaInfonavit, nomina.ISN, nomina.ISR };
                    bool esNominaCompleta = listaValores.Any(valor => valor > 0);

                    // Se obtiene la nomina mensual si es que existe.
                    var nominaExistente = _context.tblM_CapNominaMensualCC
                        .Where(x =>
                                (x.areaCuentaID == nomina.id) &&
                                (x.mes == mes) &&
                                (x.año == año))
                        .FirstOrDefault();

                    // Si ya existe el registro, se actualiza
                    if (nominaExistente != null)
                    {
                        if ((nominaExistente.nominaIMSS != Convert.ToDecimal(nomina.nominaIMSS)) ||
                            (nominaExistente.nominaInfonavit != Convert.ToDecimal(nomina.nominaInfonavit)) ||
                            (nominaExistente.ISN != Convert.ToDecimal(nomina.ISN)) ||
                            (nominaExistente.ISR != Convert.ToDecimal(nomina.ISR)))
                        {
                            nominaExistente.nominaIMSS = Convert.ToDecimal(nomina.nominaIMSS);
                            nominaExistente.nominaInfonavit = Convert.ToDecimal(nomina.nominaInfonavit);
                            nominaExistente.ISN = Convert.ToDecimal(nomina.ISN);
                            nominaExistente.ISR = Convert.ToDecimal(nomina.ISR);
                            nominaExistente.usuarioEditaID = vSesiones.sesionUsuarioDTO.id;
                            nominaExistente.fechaEdicion = DateTime.Now;
                            nominaExistente.completo = esNominaCompleta;
                        }
                    }
                    // Si no existe el registro, se crea uno nuevo
                    else
                    {
                        _context.tblM_CapNominaMensualCC.Add(new tblM_CapNominaMensualCC
                        {
                            areaCuentaID = nomina.id,
                            mes = mes,
                            año = año,
                            horasHombreTotales = Convert.ToDecimal(nomina.horasHombreTotales),
                            nominaIMSS = Convert.ToDecimal(nomina.nominaIMSS),
                            nominaInfonavit = Convert.ToDecimal(nomina.nominaInfonavit),
                            ISN = Convert.ToDecimal(nomina.ISN),
                            ISR = Convert.ToDecimal(nomina.ISR),
                            usuarioCreaID = vSesiones.sesionUsuarioDTO.id,
                            fechaCreacion = DateTime.Now,
                            completo = esNominaCompleta
                        });
                    }
                }

                _context.SaveChanges();
                resultado.Add("success", true);
            }
            catch (Exception)
            {
                resultado.Add("success", false);
                return resultado;
            }

            return resultado;
        }

        public ReporteCargoMensualNominaCCDTO ObtenerNominaMensualCCReporte(int nominaMensualID)
        {
            try
            {
                // Se descarta el tracking de cambios en context para mejorar el rendimiento.
                _context.Configuration.AutoDetectChangesEnabled = false;

                var nominaMensual = _context.tblM_CapNominaMensualCC.FirstOrDefault(x => x.id == nominaMensualID);

                List<DateTime> fechasMes = ObtenerFechasPorMesAño(nominaMensual.año, nominaMensual.mes);
                DateTime primerDiaMes = fechasMes.FirstOrDefault();
                DateTime ultimoDiaMes = fechasMes.LastOrDefault();

                tblP_CC areaCuenta = _context.tblP_CC.FirstOrDefault(cc => cc.id == nominaMensual.areaCuentaID);

                List<RepCargoNominaCCArreDTO> listaMaquinasMes = GetEconomicosMensual(areaCuenta.areaCuenta, primerDiaMes, ultimoDiaMes);

                decimal costoSocialTotal = nominaMensual.nominaIMSS + nominaMensual.nominaInfonavit + nominaMensual.ISN + nominaMensual.ISR;
                string costoSocialTotalString = "$" + costoSocialTotal.ToString("#,##0.00");

                var listaMaquinas = listaMaquinasMes.Select(x => new ReporteMaquinaNominaMensualCCDTO
                {
                    economico = x.noEconomico,
                    descripcion = x.descripcion,
                    cc = x.cc,
                    hhPeriodo = x.hhPeriodo.ToString(),
                    porcentajeCargo = (nominaMensual.horasHombreTotales != 0 ? (x.hhPeriodo / nominaMensual.horasHombreTotales) * 100 : 0).ToString("0.00") + "%",
                    cargoMaquina = "$" + ((nominaMensual.horasHombreTotales != 0 ? (x.hhPeriodo / nominaMensual.horasHombreTotales) : 0) * costoSocialTotal).ToString("#,##0.00"),
                }).ToList();
                var h = GetHorasTrabajoMensual(areaCuenta.areaCuenta, nominaMensual.mes, nominaMensual.año);
                return new ReporteCargoMensualNominaCCDTO
                {
                    proyecto = areaCuenta.areaCuenta + "-" + areaCuenta.descripcion,
                    mes = ObtenerNombreMes(nominaMensual.mes),
                    año = nominaMensual.año.ToString(),
                    listaMaquinas = listaMaquinas,
                    costoSocialTotal = costoSocialTotalString,
                    horasHombreTotales = h.ToString("0.00")
                };
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                _context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        private List<RepCargoNominaCCArreDTO> GetEconomicosMensual(string areaCuenta, DateTime primerDiaMes, DateTime ultimoDiaMes)
        {

            vSesiones.sesionEmpresaActual = 2;
            var consulta = @"SELECT cc, descripcion as noEconomico FROM DBA.cc";
            var ccEnkontrol = (List<RepCargoNominaCCArreDTO>)ContextArrendadora.Where(consulta).ToObject<List<RepCargoNominaCCArreDTO>>();

            var obj = (from eco in _context.tblM_CatMaquina.ToList()
                       join ot in _context.tblM_CapOrdenTrabajo.ToList() on eco.id equals ot.EconomicoID
                       join detOT in _context.tblM_DetOrdenTrabajo.ToList() on ot.id equals detOT.OrdenTrabajoID
                       where areaCuenta == ot.CC && ot.FechaEntrada.Date >= primerDiaMes && ot.FechaEntrada.Date <= ultimoDiaMes
                       select new RepCargoNominaCCArreDTO
                       {
                           economicoID = eco.id,
                           noEconomico = eco.noEconomico,
                           descripcion = eco.descripcion,
                           cc = ccEnkontrol.Count > 0 ? ccEnkontrol.FirstOrDefault(w => w.noEconomico == eco.noEconomico) != null ? ccEnkontrol.FirstOrDefault(w => w.noEconomico == eco.noEconomico).cc : "" : "",
                           hhPeriodo = Convert.ToDecimal((detOT.HoraFin - detOT.HoraInicio).TotalHours, CultureInfo.InvariantCulture)
                       });

            var objGroup = obj.GroupBy(x => x.noEconomico).Select(y => new RepCargoNominaCCArreDTO
            {
                economicoID = y.FirstOrDefault().economicoID,
                noEconomico = y.FirstOrDefault().noEconomico,
                descripcion = y.FirstOrDefault().descripcion,
                cc = y.FirstOrDefault().cc,
                hhPeriodo = Math.Round((y.Select(w => w.hhPeriodo).Sum() / 100) * 100, 2)
            }).ToList();

            return objGroup;
        }

        public bool verificarPermisoEliminarDocumentoEconomico()
        {
            if (vSesiones.sesionUsuarioDTO.id != 3807)
            {
                var permisoEliminar = _context.tblP_AccionesVistatblP_Usuario.FirstOrDefault(x =>
                    x.tblP_Usuario_id == vSesiones.sesionUsuarioDTO.id &&
                    x.tblP_AccionesVista_id == 3026 &&
                    x.sistema == vSesiones.sesionSistemaActual
                );

                return permisoEliminar != null;
            }
            else
            {
                return true;
            }
        }

        public List<tblM_CatMaquina> _lstCatMaquinasDapperDTO(MainContextEnum idEmpresa)
        {
            try
            {
                List<tblM_CatMaquina> _lstCatMaquinasDapperDTO = _context.Select<tblM_CatMaquina>(new Core.DTO.Utils.Data.DapperDTO
                {
                    baseDatos = idEmpresa,
                    consulta = @"SELECT id, grupoMaquinariaID, noEconomico, modeloEquipoID, anio, placas, noSerie, aseguradoraID, noPoliza, tipoCombustibleID, capacidadTanque, unidadCarga, 
		                                        capacidadCarga, horometroAdquisicion, horometroActual, estatus, trackComponenteID, descripcion, renta, marcaID, tipoEncierro, fechaPoliza, 
		                                        fechaAdquisicion, proveedor, centro_costos, ComentarioStandBy, TipoCaptura, fechaEntregaSitio, lugarEntregaProveedor, ordenCompra, costoEquipo, 
		                                        numArreglo, marcaMotor, modeloMotor, numSerieMotor, arregloCPL, CondicionUso, tipoAdquisicion, fabricacion, numPedimento, CostoRenta, UtilizacionHoras, 
		                                        TipoCambio, ProveedorID, TipoBajaID, IdUsuarioBaja, LibreAbordo, fechaBaja, kmBaja, HorometroBaja, EconomicoCC, CargoEmpresa, Comentario, Garantia, 
		                                        DepreciacionCapturada, redireccionamientoVenta, empresa, tieneSeguro 
			                                        FROM tblM_CatMaquina"
                });
                return _lstCatMaquinasDapperDTO;
            }
            catch (Exception e)
            {
                LogError(0, 1, "_lstCatMaquinasDapperDTO", "Controller", e, AccionEnum.CONSULTA, 0, 0);
                return null;
            }
        }
    }
}

