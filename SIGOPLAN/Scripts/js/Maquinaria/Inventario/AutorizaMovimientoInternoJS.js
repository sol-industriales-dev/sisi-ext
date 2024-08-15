(function () {
  $.namespace("maquinaria.MovimientosInternos.AutorizaMovimientoInterno");

  AutorizaMovimientoInterno = function () {
    estatus = {
      ACTIVO: "1",
      INACTIVO: "0"
    };
    mensajes = {
      NOMBRE: "Autorizacion de Solicitudes Reemplazo",
      SELECCIONAR_REGISTRO: "Favor de seleccionar un registro",
      ELIMINACION_REGISTRO: "¿Esta seguro que desea dar de baja este registro?",
      PROCESANDO: "Procesando..."
    };

    (tbFolio = $("#tbFolio")),
      (tbEstatus = $("#tbEstatus")),
      (divppal = $("#divppal")),
      (tblAutorizacionInterno = $("#tblAutorizacionInterno")),
      (divVista = $("#divVista")),
      (ireport = $("#report")),
      (BntRegresar = $("#BntRegresar")),
      (btnAtras = $("#btnAtras")),
      (btnNext = $("#btnNext")),
      (lblEnvia = $("#lblEnvia")),
      (btnEnvia = $("#btnEnvia")),
      (lblGuardian = $("#lblGuardian")),
      (btnGuardian = $("#btnGuardian")),
      (lblValida = $("#lblValida")),
      (btnValida = $("#btnValida")),
      (lblRecepcion = $("#lblRecepcion")),
      (btnRecepcion = $("#btnRecepcion"));
    var ControMovimientoInternoID = 0;

    function init() {
      LoadTablaInicial();
      BntRegresar.click(regresar);
      tbEstatus.change(LoadTablaInicial);
    }

    function regresar() {
      divVista.addClass("hide");
      divppal.removeClass("hide");
    }

    function LoadTablaInicial() {
      $.blockUI({ message: mensajes.PROCESANDO });
      $.ajax({
        datatype: "json",
        type: "POST",
        url: "/MovimientosInternos/PendientesAutorizacion",
        data: { filtro: Number(tbEstatus.val()) },
        success: function (response) {
          $.unblockUI();
          var data = response.ListaAutorizaciones;
          tblAutorizacionInterno.bootgrid("clear");
          tblAutorizacionInterno.bootgrid("append", data);
        },
        error: function () {
          $.unblockUI();
        }
      });
    }

    function iniciarGrid() {
      tblAutorizacionInterno
        .bootgrid({
          headerCssClass: ".bg-table-header",
          align: "center",
          formatters: {
            VerSolicitud: function (column, row) {
              return (
                "<button type='button' class='btn btn-primary VerSolicitud' data-id='" +
                row.id +
                "' >" +
                "<span class='glyphicon glyphicon-eye-open'></span> " +
                " </button>"
              );
            }
          }
        })
        .on("loaded.rs.jquery.bootgrid", function () {
          /* Executes after data is loaded and rendered */
          tblAutorizacionInterno.find(".VerSolicitud").on("click", function (e) {
            ControMovimientoInternoID = $(this).attr("data-id");
            LoadInfoTabla(ControMovimientoInternoID);
          });
        });
    }

    $(document).on("click", "#btnAutorizacion", function () {
      var id = $("#btnAutorizacion").attr("data-idAutorizacion");
      var puesto = $("#btnAutorizacion").attr("data-PuestoAutorizador");
      saveOrUpdate(id, puesto, true);
    });

    function saveOrUpdate(obj, Autoriza, tipo) {
      $.blockUI({ message: mensajes.PROCESANDO });
      $.ajax({
        url: "/MovimientosInternos/Autorizacion",
        type: "POST",
        dataType: "json",
        data: { obj: obj, Autoriza: Autoriza, tipo: tipo },
        success: function (response) {
          LoadInfoTabla(ControMovimientoInternoID);
          ConfirmacionGeneral(
            "Confirmación",
            "Se Autorizo Correctamente",
            "bg-green"
          );

          $.unblockUI();
        },
        error: function (response) {
          $.unblockUI();
          AlertaGeneral("Alerta", response.message);
        }
      });
    }

    function LoadInfoTabla(ControMovimientoInternoID) {
      $.blockUI({ message: mensajes.PROCESANDO });
      $.ajax({
        datatype: "json",
        type: "POST",
        url: "/MovimientosInternos/GetAutorizadores",
        data: { ControMovimientoInternoID: ControMovimientoInternoID },
        success: function (response) {
          $.unblockUI();
          var usuarioRecibe = response.usuarioRecibe;
          var usuarioEnvio = response.usuarioEnvio;
          var usuarioValida = response.usuarioValida;
          var GetTipoAutorizacion = response.GetTipoAutorizacion;
          var ControMovimientoInternoID = response.ControMovimientoInternoID;

          lblEnvia.text(usuarioEnvio.nombreUsuario);
          lblGuardian.text("Guardia en Turno");
          lblValida.text(usuarioValida.nombreUsuario);
          lblRecepcion.text(usuarioRecibe.nombreUsuario);

          if (GetTipoAutorizacion != "SoloLectura") {
            if (GetTipoAutorizacion == "Valida") {
              if (response.Autoriza3 != "0" && response.Autoriza2 != "0") {
                setFirmas(
                  btnValida,
                  "",
                  ControMovimientoInternoID,
                  GetTipoAutorizacion
                );
              }
            } else if (GetTipoAutorizacion == "Envia") {
              if (response.Autoriza2 == "0") {
                setFirmas(
                  btnEnvia,
                  "",
                  ControMovimientoInternoID,
                  GetTipoAutorizacion
                );
              }
            } else if (GetTipoAutorizacion == "Recibe") {
              if (response.Autoriza2 != "0") {
                setFirmas(
                  btnRecepcion,
                  "",
                  ControMovimientoInternoID,
                  GetTipoAutorizacion
                );
              }
            }

            if (response.Autoriza1 != "0") {
              btnValida.children().remove();
              if (usuarioValida.firma) {
                btnValida.next().removeClass("panel-footer-Rechazo");
                btnValida.next().removeClass("panel-footer-Pendiente");
                btnValida
                  .next()
                  .addClass("panel-footer-Autoriza")
                  .html("Autorizado");
                btnValida.removeClass("btn btn-block");
                btnValida.attr("data-Autorizado", true);
                btnValida.removeClass("bg-primary");
                btnValida.removeClass("noPadding");
              } else if (usuarioValida.firmaCadena.length > 0) {
                btnValida.next().removeClass("panel-footer-Autoriza");
                btnValida.next().removeClass("panel-footer-Pendiente");
                btnValida
                  .next()
                  .addClass("panel-footer-Rechazo")
                  .html("Rechazado");
                btnValida.removeClass("noPadding");
                btnValida.attr("data-Autorizado", false);
              } else {
                btnValida.next().removeClass("panel-footer-Autoriza");
                btnValida.next().removeClass("panel-footer-Rechazo");
                btnValida
                  .next()
                  .addClass("panel-footer-Pendiente")
                  .html("Pendiente");
                btnValida.removeClass("noPadding");
                btnValida.attr("data-Autorizado", false);
              }
            }
            if (response.Autoriza2 != "0") {
              btnEnvia.children().remove();
              if (usuarioEnvio.firma) {
                btnEnvia.next().removeClass("panel-footer-Rechazo");
                btnEnvia.next().removeClass("panel-footer-Pendiente");
                btnEnvia
                  .next()
                  .addClass("panel-footer-Autoriza")
                  .html("Autorizado");
                btnEnvia.removeClass("btn btn-block");
                btnEnvia.attr("data-Autorizado", true);
                btnEnvia.removeClass("bg-primary");
                btnEnvia.removeClass("noPadding");
              } else if (usuarioEnvio.firmaCadena.length > 0) {
                btnEnvia.next().removeClass("panel-footer-Autoriza");
                btnEnvia.next().removeClass("panel-footer-Pendiente");
                btnEnvia
                  .next()
                  .addClass("panel-footer-Rechazo")
                  .html("Rechazado");
                btnEnvia.removeClass("noPadding");
                btnEnvia.attr("data-Autorizado", false);
              } else {
                btnEnvia.next().removeClass("panel-footer-Autoriza");
                btnEnvia.next().removeClass("panel-footer-Rechazo");
                btnEnvia
                  .next()
                  .addClass("panel-footer-Pendiente")
                  .html("Pendiente");
                btnEnvia.removeClass("noPadding");
                btnEnvia.attr("data-Autorizado", false);
              }
            }
            if (response.Autoriza3 != "0") {
              btnRecepcion.children().remove();
              if (usuarioRecibe.firma) {
                btnRecepcion.next().removeClass("panel-footer-Rechazo");
                btnRecepcion.next().removeClass("panel-footer-Pendiente");
                btnRecepcion
                  .next()
                  .addClass("panel-footer-Autoriza")
                  .html("Autorizado");
                btnRecepcion.removeClass("btn btn-block");
                btnRecepcion.attr("data-Autorizado", true);
                btnRecepcion.removeClass("bg-primary");
                btnRecepcion.removeClass("noPadding");
              } else if (usuarioRecibe.firmaCadena.length > 0) {
                btnRecepcion.next().removeClass("panel-footer-Autoriza");
                btnRecepcion.next().removeClass("panel-footer-Pendiente");
                btnRecepcion
                  .next()
                  .addClass("panel-footer-Rechazo")
                  .html("Rechazado");
                btnRecepcion.removeClass("noPadding");
                btnRecepcion.attr("data-Autorizado", false);
              } else {
                btnRecepcion.next().removeClass("panel-footer-Autoriza");
                btnRecepcion.next().removeClass("panel-footer-Rechazo");
                btnRecepcion
                  .next()
                  .addClass("panel-footer-Pendiente")
                  .html("Pendiente");
                btnRecepcion.removeClass("noPadding");
                btnRecepcion.attr("data-Autorizado", false);
              }
            }
          }
          // Si es de solo lectura
          else {
            btnValida.children().remove();
            btnEnvia.children().remove();
            btnRecepcion.children().remove();

            if (usuarioValida.firma) {
              btnValida.next().removeClass("panel-footer-Rechazo");
              btnValida.next().removeClass("panel-footer-Pendiente");
              btnValida
                .next()
                .addClass("panel-footer-Autoriza")
                .html("Autorizado");
              btnValida.removeClass("btn btn-block");
              btnValida.attr("data-Autorizado", true);
              btnValida.removeClass("bg-primary");
              btnValida.removeClass("noPadding");
            } else if (usuarioValida.firmaCadena.length > 0) {
              btnValida.next().removeClass("panel-footer-Autoriza");
              btnValida.next().removeClass("panel-footer-Pendiente");
              btnValida
                .next()
                .addClass("panel-footer-Rechazo")
                .html("Rechazado");
              btnValida.removeClass("noPadding");
              btnValida.attr("data-Autorizado", false);
            } else {
              btnValida.next().removeClass("panel-footer-Rechazo");
              btnValida.next().removeClass("panel-footer-Autoriza");
              btnValida
                .next()
                .addClass("panel-footer-Pendiente")
                .html("Pendiente");
              btnValida.removeClass("noPadding");
              btnValida.attr("data-Autorizado", false);
            }

            if (usuarioEnvio.firma) {
              btnEnvia.next().removeClass("panel-footer-Rechazo");
              btnEnvia.next().removeClass("panel-footer-Pendiente");
              btnEnvia
                .next()
                .addClass("panel-footer-Autoriza")
                .html("Autorizado");
              btnEnvia.removeClass("btn btn-block");
              btnEnvia.attr("data-Autorizado", true);
              btnEnvia.removeClass("bg-primary");
              btnEnvia.removeClass("noPadding");
            } else if (usuarioEnvio.firmaCadena.length > 0) {
              btnEnvia.next().removeClass("panel-footer-Autoriza");
              btnEnvia.next().removeClass("panel-footer-Pendiente");
              btnEnvia
                .next()
                .addClass("panel-footer-Rechazo")
                .html("Rechazado");
              btnEnvia.removeClass("noPadding");
              btnEnvia.attr("data-Autorizado", false);
            } else {
              btnEnvia.next().removeClass("panel-footer-Autoriza");
              btnEnvia.next().removeClass("panel-footer-Rechazo");
              btnEnvia
                .next()
                .addClass("panel-footer-Pendiente")
                .html("Pendiente");
              btnEnvia.removeClass("noPadding");
              btnEnvia.attr("data-Autorizado", false);
            }

            if (usuarioRecibe.firma) {
              btnRecepcion.next().removeClass("panel-footer-Rechazo");
              btnRecepcion.next().removeClass("panel-footer-Pendiente");
              btnRecepcion
                .next()
                .addClass("panel-footer-Autoriza")
                .html("Autorizado");
              btnRecepcion.removeClass("btn btn-block");
              btnRecepcion.attr("data-Autorizado", true);
              btnRecepcion.removeClass("bg-primary");
              btnRecepcion.removeClass("noPadding");
            } else if (usuarioRecibe.firmaCadena.length > 0) {
              btnRecepcion.next().removeClass("panel-footer-Autoriza");
              btnRecepcion.next().removeClass("panel-footer-Pendiente");
              btnRecepcion
                .next()
                .addClass("panel-footer-Rechazo")
                .html("Rechazado");
              btnRecepcion.removeClass("noPadding");
              btnRecepcion.attr("data-Autorizado", false);
            } else {
              btnRecepcion.next().removeClass("panel-footer-Autoriza");
              btnRecepcion.next().removeClass("panel-footer-Rechazo");
              btnRecepcion
                .next()
                .addClass("panel-footer-Pendiente")
                .html("Pendiente");
              btnRecepcion.removeClass("noPadding");
              btnRecepcion.attr("data-Autorizado", false);
            }
          }

          divVista.removeClass("hide");
          divppal.addClass("hide");

          var path =
            "/Reportes/Vista.aspx?idReporte=51&idControlInterno=" +
            ControMovimientoInternoID;
          ireport.attr("src", path);
          document.getElementById("report").onload = function () {
            $.unblockUI();
          };
        },
        error: function () {
          $.unblockUI();
        }
      });
    }
    function setFirmas(
      elemento,
      texto,
      ControMovimientoInternoID,
      tipoAutorizacion
    ) {
      elemento.children().remove();
      var btnsControl =
        "<div class='row'> <div class='col-lg-12 col-xs-12' id='divAccionesAutorizacion'> <div class='col-xs-offset-2 col-xs-8'><button class='form-control btn btn-block colorAutoriza' id='btnAutorizacion'>Autorizar</button></div>";
      // "<div class='col-xs-6'><button class='form-control btn btn-block colorRechaza rechazo' id='btnRechazo'>Rechazar</button></div></div></div>";
      elemento.append(btnsControl);

      $("#btnAutorizacion").attr(
        "data-idAutorizacion",
        ControMovimientoInternoID
      );
      $("#btnAutorizacion").attr("data-PuestoAutorizador", tipoAutorizacion);
      $("#btnRechazo").attr("data-idAutorizacion", ControMovimientoInternoID);
      $("#btnRechazo").attr("data-PuestoAutorizador", tipoAutorizacion);
    }

    iniciarGrid();
    init();
  };

  $(document).ready(function () {
    maquinaria.MovimientosInternos.AutorizaMovimientoInterno = new AutorizaMovimientoInterno();
  });
})();
