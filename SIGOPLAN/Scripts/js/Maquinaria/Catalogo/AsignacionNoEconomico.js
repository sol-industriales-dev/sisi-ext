(function () {
    $.namespace('maquinaria.CatMaquina.AsignacionNoEconomico');
    AsignacionNoEconomico = function () {
        //#region VARIABLEs
        const botonNuevoCuadro = $('#botonNuevoCuadro');
        var dialog,
            idAsignacionp = 0;
        tblEconomicos = $("#tblEconomicos"),
            cboFiltroTipo = $("#cboFiltroTipo"),
            cboFiltroGrupo = $("#cboFiltroGrupo"),
            txtFiltroNoEconomico = $("#txtFiltroNoEconomico"),
            tblCompraRentaEquipo = $("#tblCompraRentaEquipo"),
            cboFiltroTipoEconomicos = $("#cboFiltroTipoEconomicos");
        cboTipoCaptura = $("#cboTipoCaptura");
        estatus = {
            ACTIVO: '1',
            INACTIVO: '0'
        };
        mensajes = {
            NOMBRE: 'Maquina',
            SELECCIONAR_REGISTRO: 'Favor de seleccionar un registro',
            ELIMINACION_REGISTRO: '¿Esta seguro que desea dar de baja este registro?',
            PROCESANDO: 'Procesando...'
        };
        _guardarNuevo = false
        imgdefault = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAYAAAD0eNT6AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAGFJJREFUeNrs3U+IXdd9B/AjW3aE7caThZwGgzxaSBujVMLUUIw7T2C6SFI6biG0dOFRFm03AYsuSuuCbCgUWszYZOVNZ7RI0xaKZOxSWmw00zQtpAmSI2dRaaF54zovkRo8cv4p9sK9Z95TW5NInrn3vnfuuefzgYuEwaOZ6+tzv+d73/0pBAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4CPs6fj3N1cdR/1n4jY2JgcAGQaAweRYqI75yQF1w8B6daxNDgA6Jt7wV6rjner4wOGYwvHO5Bob+N8NoBs3/nNuTo4ZH+cEAYA05qvjjBuRI/FxJnjEBDAzi0HV7+jWo4FF/1sCTNeyG46jo8ey/z0BpmPFTcbR8WPF/6ZAie6c8s1/ySmm4+KcifnqeNmpAASA5mK1+gdOLxmFgDh06p+cCqAU0xgEFD9cdcapJUNPVsdZpwEQAHZvvjrOT3ZTkJut6jgWjBYGCnBHy19v2c2fjM0FbwYAGoBdG4TxtDXI3fHg7xEANAA7dsrppCdcy4AAsIvd/8DppCdcz4AAsENPOZX0jGsa6LW2PgMQZ6u38uG/uX13haUjB8PCgf3V7+8Og+pXuJW1zWth68Z7Yb36dfXiler377f1peMbAZ9whgEB4NYGoaUP/y0dmQ/LTxzbDgGw6zt2dfM/+dr5KghstPUlfRgQ6K02JgEuhRael6587tHw7OMPh3177/RfhVritbN4+MEwP3dvePnS2218yaEAAPRVG58BWGhj5x8PaEOL19OCswkIALfWaKWNdX+s/aFNLT1KkkoBAWBai2T8wJ9n/rTt5odJBQCA6QWARhZ8yh/XFkB5ASC+6geuLYDCAoD3/HFtARQYAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAAAQAAEAAAAAEAABAAAAABAAAQAKB/Bk4BIABAeeacAkAAgPIsOAWAAACZmb//3qZfYkkLAAgAUF4AiDf/ZWcSEAAgI4OH9rfxZZYmB4AAADlYOPBAW19qZXJ4HAD0wl6ngF43AAf2h7l9d4WtG++31QQsVsdqdaxXx5YzDJ22MTkQACjR4qEHw+rF1taA2AA8PTmA/MJADO9rk6NoHgHQe099+qCTAMyH8WCvU9VxrjreCePHegMBAHoqPgYYtPdZAKAfYpu3NAkD50oMAgIARTj1+MNOAnDLfcIkBJyZNAUCAPSpBVg8/KATAdxO/JDv+cmvAgD0xfITR7ffCAC4jblJE9D7AWACAMWIUwFXPvuoEwHsRHzTZ0UAgJ6IjwGe/uXDTgSwE0t9DgECAMWJjwKWjsw7EcBOQ0AvHwcIABRp5XOPCgHATsXHAb37YKAAQNEhwOMAYKdLRujZK4ICAEWLjwPO/NZj3g4APkrv/mpwAYDixQ8Gnv/Cr5kTAHzkchF6NDFQAIAwfkUwNgHnfve4scHA7Zzqyw+yp4Wv8UGjf/mPP+9yonPWNq+F09+6Es5efrutv0oY6I/joQd/m6AAADsIA+ubV8Pa8FrYuP6j7QMo2mp1nBAABAAAOhret268VwX4a2H14pU227yt6viEACAAANBx8eZ/8rXzVRDYaOtLZv8YwIcAAei9+KpvnP0Rj5YMcj8nAgAAxYgTQFuaArogAABARpafONbG8K/53M+DAABAUeLNf+nIQQHApQBAaRYO7C/+HAgAABTYAtwtALgMACjNQAMgAACAAAAACAAAgAAAAAgAAIAAAAAIAACAAAAACAAAgAAAAAgAAIAAAAAIAACAAAAACAAAgAAAAAgAAIAAAAACAAAgAAAAAgAAIAAAABnb6xTc2taN98OFq1tOBEAN8/ffUx33OhECQHetbV4L65tXw9rwWti4/qPtA4A2w8C928fgof1h4cADYXBgv5MiAKS76Z/+1pVw9vLb2zt9AKbn5uZqrdpshfDtMLfvrrB46MHw1KcPCgMCwOxu/M999duTixCAFOLGa/XixvYxOPBAOPX4w4KAADC99HnytQvh7KW3/VcH6NTG7GpY+/LVsHj4wbD8xFGfG5iRIt4CiDf9Y3/1z27+ANZqSgkAcdf/5N9/zXN+gAzEtTqu2XHtRgCo7cSrXw8v/Mcl/5UBMhPX7riGIwDUuvnHD5cAkKe4hgsBAsCuxOrIzR+gHyHA4wABYEfih0fU/gD9Edd0HwwUAG4rvup34h/URQB9E9d2U1oFgFuKNZFP+wP0T1zbPQoQAH6uOOFPRQTQX3GNj2s9AsCHxPG+APSbtV4A+Jndv9n+AP23PTZYCyAA3BT/Vj8AymDNb0cv/jKg+Ff6tmXuF0JY+vUQFh4Z/37wiIsEoNZu/ZshbP0ghPXq19VXxr9va81fcXoFgFgFtfXJ/3jjX/7D8Y0fgGZubqAWByGc+r0QTj4/DgJNxTU/rv3++uBmsn8EsN7Ss/+VZ8eHmz9A++LaenOd7dLaLwDk3AAMm38YJO784wHAdLW13rax9gsAmWs6GSqm0lj7AzAbbTxqNRVQAGh8EcQkqvYHmJ2bH7YWAASApBZ8yh/A2isAlJlEAbD2CgCF8Z4/gLVXAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAAEAABAAAAABAAAQAAAAAQAAEAAAAABAAAQAAAAAQAAEAAAAAEAAMjJXqegu7Z+EMKFS84DAAJAL619M4T16lj7Rggbo+r4jnMCgADQ25v+6VdCOLs23ukDgADQ8xv/cy+NfwUAAaDnYq1/8vnxjh8ABIACxJv+iWdV/QAIAMWIu/4X/tp5AEAAKEbc9a++4jwA0D0GAbn5AyAA0IZY+7v5AyAAFCR+4M8zfwAEgILEV/1i9Q8AAkBBYvXvVT8ABICCxMl+hvwAIAAUJo73BQABoLDdv9n+AAgAhTntlT8AMmMSYAvafPY/d/89Yem3fyUsPHY4zH38njCofgXooz37f99JEADyFav/tj75H2/8y3/2+e0QAAACQIett/Tsf+VLS9sBAABmwWcAmjYA32hn5+/mD4AAkJGNUbN/P9b9sfYHAAEgpwDwnea7f8/8ARAACrPgU/4ACADlia/6AYAAUBjv+QMgAAAAAgAAIAAAAAIAACAAAAACAAAgAAAAAgAACAAAgAAAAAgAAIAAAAAIAACAAAAACAAAgAAAAAgAAIAAAAAIAACAAAAACAAAgAAAAOzGXqeALlr72iUnAbj9OrF5LfW3MLjFP79QHVsCANzq/5A33wov/+Mb1c3+P8PG5vfDxlvfd1KAHTv+5XOpv4WdfAMbk0DwRnWcnfxeAKA88Sb/4kuvh9W/+fewdf3HTgjQd/OTY7E6Tk2agdXqeHESDgQA+r/bP/mnf6faB0o3Vx1PT4616jiZqhUQAJiquMuPN/644wfgQwbVcX7SCJwMM/7cgLcAmJq42z/4yDNu/gC3t1QdV8KtP1QoAJCPeNM/vvi85/wAOxMfDZybhAEBgDyd+OLq9gHArq1MDgGA/G7+Kn+ARpZmEQIEAFoTb/xu/gCthYAlAYDOix/4U/sDtCq2AEcFADorftDPzR9gKs6E8QcEBQC657m/fNUYX4DpmA/jCYICAN0Sb/wvvPS6EwEwPU9PgoAAQId2/3/xipMAMH2ttwACAI12/z71DzATS223AAIAtZ128weYpcU2v5i/DIjaVr/yb619rbn7PhYWf/Vw+I3HD23//uihT27/CpCTrR/+NFy4/L3tX1/+6uVw9l8ubf++JU9VxwsCAEnF+r+tT/4vVjf9lWc+64YPZC+uY4NjB/53bTv1hcfCyRdfD2erMNCCOBNgPi7BbXwxjwCoJQ7+acPSZ46EM3/+m27+QC/N/+L922tcXOtaMmjrCwkA1PLGm281v4qrlLzyJ59xMoHei2vdzWagoV8SAEjqQgsBYOUZN3+goBDQzprX2mhgAYBaNjabPf+Pz8ZiNQZQirjmxbWv6ZcRAEgbABp+AHChnSoMICstrH0CAPne/KOjhx5wIoHidGntEwDYfQDYbB4A5j+l/gcQACiuAfD8Hyhy/fzudQGAfA0bBgC7f6DY9XP0buMMIQCQLsFu/rfdP0CaBkAAIOEFrAEAqLd+jgQAsm4AGgYADQCgAahrKACQbQPw0Kc+7iQCGoDEDYC/DZCZ3vw1ANAda+c3G/37TWfbN/3zU9vtz9/SGwACAIkCgBkA0BvHv/iVRv/+B//6R0n//NR2+/O3sPtvNQB4BIAGAGAW6+d3BQAyZgYAQM31s0MzAAQAdn/1mQEAkKoBEABIeAFrAADqrZ8jAYCsGwAzAAASNQDDNr8fAYCZNgBmAAAaAA0Ahd38NQCA3b8AQI4XsBkAAKl2/wIAGgAADYAAwAyZAQBQc/3s2AwAAYDdXX1mAACkagAEABJewBoAgHrr50gAIOsGwAwAgEQNwLDt70kAYGYNgBkAgAZAA0BhN38NAGD3360AsNd/GnZ05ZkBAL1z7ku/U/Sfn9HuXwBAAwC0Z3DsQNF/fukNgEcA7IgZAAA1188OzgAQANj51WcGAECqBkAAIOEFrAEAqLd+jgQAsm4AzAAASNQADKfxfQkAzKQBMAMA0ABoACjs5q8BAOz+BQByvIDNAABItfsXANAAAGgABABmyAwAgJrrZ0dnAAgA7OzqMwMAIFUDIACQ8ALWAADUWz9HAgBZNwBmAAAkagCG0/reBACm3gCYAQBoADQAFHbz1wAAdv8CADlewGYAAKTa/QsAaAAANAACADNkBgBAzfWzwzMABAA++uozAwAgVQMgAJDwAtYAANRbP0cCAFk3AGYAACRqAIbT/P4EAKbaAJgBAGgANAAUdvPXAAB2/wIAOV7AZgAApNr9CwBoAAA0AAIAM2QGAEDN9bPjMwAEAG5/9ZkBAJCqARAASHgBawAA6q2fIwGArBsAMwAAEjUAw2l/jwIAU2sAzAAANAAaAAq7+WsAALt/AYAcL2AzAABS7f4FADQAABoAAYAZMgMAoOb6mcEMAAGAW199ZgAApGoABAASXsAaAIB66+dIACDrBsAMAIBEDcBwFt+nAMBUGgAzAAANgAaAwm7+GgDA7l8AIMcL2AwAgFS7fwEADQCABkAAYIbMAACouX5mMgNAAODnX31mAACkagAEABJewBoAgHrr50gAIOsGwAwAgEQNwHBW36sAQOsNgBkAgAZAA0BhN38NAGD3LwCQ4wVsBgBAqt2/AIAGAEADIAAwQ2YAANRcPzOaASAA8LNXnxkAAKkaAAGAhBewBgCg3vo5EgDIugEwAwAgUQMwnOX3KwDQagNw/30fcxIBDYAGgJJu/tHRQw84kYDdvwBAVhdwCzMA5u7b50QCdv8CABoAAA2AAECnNZ0BMOf5P1Dq+pnZDAABgA9ffQ1nABw99EknEdAACABkdwFrAADqrZ8jAYCsG4BmAUADAGgAahvO+nsWAGitATADANAAaAAo7OY/bgC8AQDY/QsA5HUBmwEAkGr3LwCgAQDQAAgAzJAZAAA1188MZwAIAPzf1WcGAECqBkAAIOEFrAEAqLd+jgQAsm4AzAAASNQADFN83wIArTQAZgAAGgANAIXd/McNgDcAALt/AYC8LmAzAABS7f4FADQAABoAAYAZMgMAoOb6mekMAAGA8dVnBgBAqgZAACDhBawBAKi3fo4EALJuAMwAAEjUAAxTfe8CAGYAAGgAcPOv0wB4AwCw+xcAyOsCNgMAINXuXwBAAwCgARAAmCEzAABqrp8ZzwAQADADACBdAyAAkPAC1gAA1Fs/RwIAWTcAZgAAJGoAhim/fwFAA9Do3zcDANAAaAAo7OY/bgC8AQDY/QsA5HUBmwEAkGr3LwCgAQDQAAgAzJAZAAA118/MZwAIAKUnWDMAAFI1AAIACS9gDQBAvfVzJACQdQNgBgBAogZgmPpnEAA0ALWZAQBoADQAFHbzHzcA3gAA7P4FAPK6gM0AAEi1+xcA0AAAaAAEAGbIDACAmutnD2YACAAlJ1gzAABSNQACAAkvYA0AQL31cyQAkHUDYAYAQKIGYNiFn0MA0ADUYgYAoAHQAFDYzX/cAHgDALD7FwDI6wI2AwAg1e5fAEADAKABEACYITMAAGqunz2ZASAAlJpgzQAASNUACAAkvIA1AAD11s+RAEDWDYAZAACJGoBhV34WAUADsGtmAAAaAA0Ahd38xw2ANwAAu38BgLwuYDMAAFLt/gUANAAAGgABgBkyAwCg5vrZoxkAAkCJCdYMAIBUDYAAQMILWAMAUG/9HAkAZN0AmAEAkKgBGHbp5xEANAC7YgYAoAHQAFDYzX/cAHgDALD7FwDI6wI2AwAg1e5fAEADAKABEACYITMAAGqunz2bASAAlJZgzQAASNUACAAkvIA1AAD11s+RAEDWDYAZAACJGoBh134mAUADsGNmAAAaAA0Ahd38xw2ANwAAu38BgLwuYDMAAFLt/gUANAAAGgABgBkyAwCg5vrZwxkAAkBJCdYMAIBUDYAAQMILWAMAUG/9HAkAZN0AmAEAkKgBGHbx5xIANAA7YgYAoAHQAFDYzX/cAHgDALD7FwDI6wI2AwAg1e5fACCdC2++pQEA0AAIAKW5/u5PGu7+Pf8HytTXGQACQCkNwMVmDYA3AAANgABAhrbe/bEGAKDO3XskAJBzA/Dmf2kAANI0AMOu/mwCQAkNwPVmDYAZAIAGQANAdrt/bwAAJNr9CwCk3P3/pPHXMAMAsPsXANAAAGgABAC6zgwAgHr6PANAACihATADACBVAyAAkI4ZAAA1794jAYCcGwAzAABSNQDDLv98AkDfGwAzAAA0AAJAabt/bwAAJNr9CwCk3P2bAQCQaPcvAKABANAACADMkBkAAPX0fQaAAND3BsAMAIBUDYAAQDpmAADUvHuPBABybgDMAABI1QAMu/4zCgB9bgDMAADQAAgApe3+vQEAkGj3LwCQcvdvBgBAot2/AIAGAEADIAAwQ2YAANRTwgwAAaDPDYAZAACpGgABgHTMAACoefceCQDk3ACYAQCQqgEY5vBzCgB9bQDMAADQAAgApe3+vQEAkGj3LwCQcvdvBgBAot2/AIAGACC79fPyVQGAfJkBAFBz/fzhT4u4+QsAfU2wZgAA1GwAvicAkC8zAABqrp8aALJOsGYAAKRqAIa5/KwCQB8TrBkAABoAAaC03b83AADq7f7LeQNAAOjl7t8MAIB6u/8bAgAaAAANgABARswAAKi5fhb0/F8A6GOCNQMAoGYDUM4MAAGgh8wAAKi5fmoAyDrBmgEAkKoBGOb08woAfUuwZgAAaAAEgNJ2/94AAKi3+y/rDQABoHe7fzMAAOrt/suaASAAaAA0AAAaAHJnBgBAzfWzsOf/AkDfEqwZAAA1G4CyZgAIAD1jBgBAzfVTA0DWCdYMAIBUDcAwt59ZAOhTgjUDAEADIACUtvv3BgBAvd1/eW8ACAC92v2bAQBQb/df3gwAAUADoAEArJ8aAHJmBgBAzfWzwOf/AkCfEqwZAAA1G4DyZgAIAD1iBgBAzfVTA0DWCdYMAIBUDcAwx59bAOhLgjUDAEADIACUtvv3BgBAvd1/mW8AdCIArG1ecwU23v2bAQBQb/df5gyATgSArRvvuQI1AAAagNICwLoGoDEzAABqrp+FPv9vKwA0+uFXL14JWzfedxU2SbBmAADUbADKnAHQiQAQb/4nXzvvKmxyDs0AAKi3fmoAGllv+gVWL25sH9RMsGYAAKRqAIa5/uxtBIC1Nr6RE69+ffvwOKBGgjUDAEADsEt7WwoAW9Ux10YTcPby22HpyMGwcGB/mNt3dxhUv3K73b83AADq7f7LfQMg2tPS11mpjiWXEwCFORgKfw3wtGsAgAJl2wC0FQDWQkufBQAAN/98AkD0nGsBAAGgvAAQG4CzrgcABICyAkB0MozfCACAvhvm/M23HQBiGjrhmgBAA1BWAIjiY4AXXBcACABlBYAoPgpYdW0AIAB0054pf30DggDoqz05f/N3TPnrx88DeBwAQN+s5f4D3DGDPyM+DngyeDsAgP7IfgLuLOuL+epYro5F1w0AGbtQHcdy/yHunOGfFRuAv62O9UkYmHcNAZCZeC87HnrQat+Z4M/cCOPqZP3/NQP7XFMAZLDzPx4y//T/TV35BONgcixoBwDomLXJxnW1Tz9UDq8wDFx7ACTa8fsAOwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlOt/BBgAFdrfnQmBfccAAAAASUVORK5CYII=';

        tbGarantia = $("#tbGarantia"),
            txtComentarios = $("#txtComentarios"),

            cboLugarEntrega = $("#cboLugarEntrega");
        /*Nuevo Codigo*/
        tblEconomicoCC = $("#tblEconomicoCC"),
            cboProveedores = $("#cboProveedores"),
            cboModalEquipoRenta = $("#cboModalEquipoRenta")
        txtModalNoEconomico = $("#txtModalNoEconomico"),
            btnGuardarEconomico = $("#btnGuardarEconomico"),
            ireport = $("#report"),
            tbHorometroInicial = $("#tbHorometroInicial"),
            btnSetReporte = $("#btnSetReporte"),
            frmMaquinaFichaTecnica = $("#frmMaquinaFichaTecnica"),
            Paso1 = $("#Paso1"),
            txtModalProveedorFichaT = $("#txtModalProveedorFichaT"),
            txtFechaEntregaSitioFichaT = $("#txtFechaEntregaSitioFichaT"),
            txtLugarEntrega = $("#txtLugarEntrega"),
            tbOrdenCompra = $("#tbOrdenCompra"),
            tbCostoEquipo = $("#tbCostoEquipo"),
            cboTipoFichaT = $("#cboTipoFichaT"), // Tipo Equipo
            txtDescripcionFichaT = $("#txtDescripcionFichaT"),
            cboModalMarcaFichaT = $("#cboModalMarcaFichaT"),
            cboModalModeloFichaT = $("#cboModalModeloFichaT"),
            txtModalNoSerie = $("#txtModalNoSerie"),
            txtArreglo = $("#txtArreglo"),
            txtMarcaMotor = $("#txtMarcaMotor"),
            txtModeloMotor = $("#txtModeloMotor"),
            txtSerieMotor = $("#txtSerieMotor"),
            txtArrelgoMotor = $("#txtArrelgoMotor"),
            cboCondicionesUso = $("#cboCondicionesUso"),
            cboAdquisicionEquipo = $("#cboAdquisicionEquipo"),
            cboModalAniosFichaT = $("#cboModalAniosFichaT"),
            cboLugarFabricacion = $("#cboLugarFabricacion"),
            tbnumPedimento = $("#tbnumPedimento"),
            report = $("#report1"),
            cboCargoEmpresa = $("#cboCargoEmpresa"),
            cboEmpresa = $('#cboEmpresa'),
            chKTieneSeguro = $('#chKTieneSeguro'),
            tblEconomicoCCAlta = $("#tblEconomicoCCAlta"),
            btnCargarInfo = $("#btnCargarInfo"),
            btnGuardarFicha = $("#btnGuardarFicha");
        // btnNuevoAutorizar = $('#btnNuevoAutorizar');

        cboEditTipoMaquinaria = $("#cboEditTipoMaquinaria"),
            cboModalGrupoMaquinaria = $("#cboModalGrupoMaquinaria"),
            txtModalDescripcion = $("#txtModalDescripcion"),
            cboModalMarca = $("#cboModalMarca"),
            cboModalModelo = $("#cboModalModelo"),
            txtModalNoSerieEdit = $("#txtModalNoSerieEdit"),
            cboModalAseguradoras = $("#cboModalAseguradoras"),
            txtModalPoliza = $("#txtModalPoliza"),
            dateModalVigenciaPoliza = $("#dateModalVigenciaPoliza"),
            txtModalPlaca = $("#txtModalPlaca"),
            cboModalTipoEncierro = $("#cboModalTipoEncierro"),
            cboModalTipoCombustible = $("#cboModalTipoCombustible"),
            txtModalCapTanque = $("#txtModalCapTanque"),
            txtModalProveedor = $("#txtModalProveedor"),
            txtModalHorasAdquisicion = $("#txtModalHorasAdquisicion"),
            txtModalHorometroActual = $("#txtModalHorometroActual");
        cboModalAnios = $("#cboModalAnios");
        /*END*/
        dateModalFechaAdquiere = $("#dateModalFechaAdquiere");
        /**/
        tbCostoEquipoRenta = $("#tbCostoEquipoRenta"),
            tbUtilizacionHoras = $("#tbUtilizacionHoras"),
            cboTipoMoneda = $("#cboTipoMoneda");
        /**/
        divCostoRenta = $("#divCostoRenta"),
            divOrdenCompra = $("#divOrdenCompra"),
            divCostoEquipo = $("#divCostoEquipo"),
            divUtilizacionHoras = $("#divUtilizacionHoras");
        cboPropiedadEmpresa = $("#cboPropiedadEmpresa");
        const getTblCRPendientes = originURL('/CatMaquina/getDataSolicitudes');
        const getTblFichaTecnica = originURL('/CatMaquina/getListaMaquinas');

        const gpxInteresVsEfectiva = $('#gpxInteresVsEfectiva');
        const gpxPagoTotalBanco = $('#gpxPagoTotalBanco');

        let vistaCapturar = $('#vistaCapturar');
        let tblComparativo = $('#tblComparativoAdquisicionyRenta');
        let btnNuevo = $('#btnNuevo');
        let btnBuscar = $('#btnBuscar');

        let lstFormateando = [];
        let dtComaparativo;
        let idDet1;
        let idDet2;
        let idDet3;
        let idDet4;
        let idDet5;
        let idDet6;
        let idDet7;
        let addCaracteristicas = 22;
        let intCaracteristicas = 0;

        let idCaracteristica1;
        let idCaracteristica2;
        let idCaracteristica3;
        let idCaracteristica4;
        let idCaracteristica5;
        let idCaracteristica6;
        let idCaracteristica7;


        let idCaracteristica21;
        let idCaracteristica22;
        let idCaracteristica23;
        let idCaracteristica24;
        let idCaracteristica25;
        let idCaracteristica26;
        let idCaracteristica27;
        let idCaracteristica31;
        let idCaracteristica32;
        let idCaracteristica33;
        let idCaracteristica34;
        let idCaracteristica35;
        let idCaracteristica36;
        let idCaracteristica37;
        let idCaracteristica41;
        let idCaracteristica42;
        let idCaracteristica43;
        let idCaracteristica44;
        let idCaracteristica45;
        let idCaracteristica46;
        let idCaracteristica47;
        let idCaracteristica51;
        let idCaracteristica52;
        let idCaracteristica53;
        let idCaracteristica54;
        let idCaracteristica55;
        let idCaracteristica56;
        let idCaracteristica57;
        let idCaracteristica61;
        let idCaracteristica62;
        let idCaracteristica63;
        let idCaracteristica64;
        let idCaracteristica65;
        let idCaracteristica66;
        let idCaracteristica67;
        let idCaracteristica71;
        let idCaracteristica72;
        let idCaracteristica73;
        let idCaracteristica74;
        let idCaracteristica75;
        let idCaracteristica76;
        let idCaracteristica77;

        let estatusFinanciera = 0;
        let estatus2 = 0;
        const selAutSolicita = $("#selAutSolicita");
        const selAutSolicita1 = $("#selAutSolicita1");
        const selAutSolicita2 = $("#selAutSolicita2");
        const selAutSolicita3 = $("#selAutSolicita3");
        // const selAutSolicita4 = $("#selAutSolicita4");
        const selAutSolicita5 = $("#selAutSolicita5");

        const selAutSolicitaFinanciero = $('#selAutSolicitaFinanciero');
        const selAutSolicitaFinanciero1 = $('#selAutSolicitaFinanciero1');
        const selAutSolicitaFinanciero2 = $('#selAutSolicitaFinanciero2');
        const selAutSolicitaFinanciero3 = $('#selAutSolicitaFinanciero3');
        // const selAutSolicitaFinanciero4 = $('#selAutSolicitaFinanciero4');
        const selAutSolicitaFinanciero5 = $('#selAutSolicitaFinanciero5');

        let dtAutorizacion;
        let tblComparativoFinanciero = $('#tblComparativoFinanciero');

        const btnBuscarCompraORenta = $('#btnBuscarCompraORenta');
        const btnAgregarColumna = $('#btnAgregarColumna');
        const btnEliminarColumna = $('#btnEliminarColumna');
        const btnCerrarCuadroComparativo = $('#btnCerrarCuadroComparativo');
        const btnAgregarColumnaFinanciero = $('#btnAgregarColumnaFinanciero');
        const btnEliminarColumnaFinanciero = $('#btnEliminarColumnaFinanciero');
        const btnLimpiarFinanciero = $('#btnLimpiarFinanciero');
        const btnNuevoFinanciero = $('#btnNuevoFinanciero');
        const btnCerrarCuadroComparativoFinanciero = $('#btnCerrarCuadroComparativoFinanciero');

        const contenidoPRINTadquisicion = $('#contenidoPRINTadquisicion');
        const contenidoPRINTFinanciero = $('#contenidoPRINTFinanciero');
        const txtEquipo = $('#txtEquipo');


        const modalMensualidadesFinanciero = $("#modalMensualidadesFinanciero");
        const tblMensualidadesFinanciero = $("#tblMensualidadesFinanciero");
        let dtMensualidadesFinanciero;

        let NumeroMayor = 0;
        let columnas = 1;
        let columnasFinanciero = 1;
        let soyAdFin = 0;
        let folio = '';

        const chKSeguro = $("#chKSeguro");
        const chKManualesOperacion = $('#chKManualesOperacion');
        //#endregion
        function init() {
            cboProveedores.fillCombo('/CatMaquina/fillCboProveedores');
            cboFiltroTipo.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            cboTipoFichaT.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
            txtFechaEntregaSitioFichaT.datepicker().datepicker("setDate", new Date());
            dateModalVigenciaPoliza.datepicker().datepicker("setDate", new Date());
            dateModalFechaAdquiere.datepicker().datepicker("setDate", new Date());
            txtDescripcionFichaT.change(FillCboMarca);
            cboFiltroTipo.change(FillCboGrupo);
            cboModalMarcaFichaT.change(FillCboModelo);
            cboModalAniosFichaT.fillCombo('/CatMaquina/FillCbo_Anios');
            cboTipoFichaT.change(FillDescripcion);
            cboFiltroTipoEconomicos.change(bootGMaquinaria);
            btnSetReporte.click(SendData);
            btnCargarInfo.click(bootGMaquinaria);
            bootGMaquinaria();
            bootGRenta();
            iniciarModal();
            initdt_ComparativoFinanciero();
            initTblMensualidadesFinanciero();
            btnGuardarEconomico.click(GuardarData);
            cboCondicionesUso.change(fnCondicionesUso);
            botonNuevoCuadro.click(mostrarModalCuadro);
            fncBtnNuevo();
            fncBtnBuscar();
            fncbtnAgregarColumna();
            fncbtnCerrarCuadroComparativo();
            btnbtnBuscarCompraORenta();
            // fncbtnNuevoAutorizar();
            $('#tblComparativoAdquisicionyRenta_paginate').css('display', 'none');
            $('#tblComparativoAdquisicionyRenta_length').css('display', 'none');
            $('#tblComparativoAdquisicionyRenta_filter').css('display', 'none');
            $('.ui-icon-closethick').click(function () {
                $('#contenidoPRINTadquisicion').find('table').remove();
                $('#contenidoPRINTFinanciero').find('table').remove();
                $('#tblComparativoAdquisicionyRenta').find('tr').remove();
                $('#tblComparativoFinanciero').find('tr').remove();
                $('#contenidoheader').remove();
                $('#contenidoFecha').remove();
                $('#contenidoAdquisicionRenta').remove();

            });
        }
        const btnbtnBuscarCompraORenta = function () {
            btnBuscarCompraORenta.click(function () {
                bootGRenta();
            });
        }
        function AddRows(tbl, lst) {
            dt = tbl.DataTable();
            dt.clear().draw();
            dt.rows.add(lst).draw(false);
        }
        function mostrarModalCuadro() {
            idAsignacionp = $(this).attr('data-idAsignacion');
            addCaracteristicas = 22;
            columnas = 1;
            soyAdFin = 0;
            soyAdFin = 1;

            btnLimpiarFinanciero.css('visibility', 'visible');
            btnNuevoFinanciero.css('visibility', 'visible');
            btnAgregarColumnaFinanciero.css('visibility', 'visible');
            btnCerrarCuadroComparativoFinanciero.css('visibility', 'visible');
            dlgFormAdquisicion.dialog("open");

            $('#idAsignacion').val(idAsignacionp);
            $('#dlgFormAdquisicion').css('min-height', '850px');
            $('#dlgFormAdquisicion').css('height', '100%');

            // addAdquisisionP(0);
            _guardarNuevo = true;
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAdquisicion",
                data: { objFiltro: objFiltro },
                success: function (response) {
                    if (response.success) {
                        establecerAdquisicion(response.items);
                        acomadorInput(0, 0);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });

            $('#txtObra').val('');
            $('#txtNombreDelEquipo').val('');
            $('#checkCompra').prop('checked', false);
            $('#checkRenta').prop('checked', false);
            $('#checkRoc').prop('checked', false);

            selAutSolicita.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutSolicita1.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutSolicita2.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutSolicita3.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
            selAutSolicita5.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
        }
        function validateData() {
            let state = true;
            if (!validarCampo(selAutSolicita)) { state = false; }
            return state;
        }
        function fnSelRevisa(event, ui) {
            $(this).data("id", ui.item.id);
            $(this).data("nombre", ui.item.value);
        }
        function fnSelNull(event, ui) {
            if (ui.item === null && $(this).val() != '') {
                $(this).val("");
                $(this).data("id", "");
                $(this).data("nombre", "");
                AlertaGeneral("Alerta", "Solo puede seleccionar un usuario de la lista, si no aparece en la lista de autocompletado favor de solicitar al personal de TI");
            }
        }
        const fncbtnAgregarColumna = function () {
            btnAgregarColumna.click(function () {
                fncAgregarColumnas();
            });
            btnEliminarColumna.click(function () {
                fncEliminarColumna();
            });
        }
        const fncbtnCerrarCuadroComparativo = function () {
            btnCerrarCuadroComparativo.click(function () {
                CerrarCuadroComparativo(0);
            });
        }
        function SendData() {
            ValidaCampos();
            if (Validacion()) {
                setFichaTecnica();
            }
            else {
                AlertaGeneral("Alerta", "¡Hay campos obligatorios!");
            }
        }
        function ValidaCampos() {
            txtFechaEntregaSitioFichaT.addClass('required');
            txtLugarEntrega.addClass('required');
            if (cboCondicionesUso.val() == "1") {

                tbOrdenCompra.addClass('required');
                tbCostoEquipo.addClass('required');
                tbUtilizacionHoras.removeClass('required');
                tbCostoEquipoRenta.removeClass('required');
            }
            else {
                tbOrdenCompra.removeClass('required');
                tbCostoEquipo.removeClass('required');
                tbUtilizacionHoras.addClass('required');
                tbCostoEquipoRenta.addClass('required');
            }
            cboProveedores.addClass('required');
            cboTipoFichaT.addClass('required');
            txtDescripcionFichaT.addClass('required');
            cboModalMarcaFichaT.addClass('required');
            cboModalModeloFichaT.addClass('required');
            txtModalNoSerie.addClass('required');
            cboModalAniosFichaT.addClass('required');

        }
        function Validacion() {

            var state = true;

            if (cboCondicionesUso.val() == "1") {

                if (!validarCampo(tbOrdenCompra)) { state = false; }
                if (!validarCampo(tbCostoEquipo)) { state = false; }
            }
            else {
                if (!validarCampo(tbUtilizacionHoras)) { state = false; }
                if (!validarCampo(tbCostoEquipoRenta)) { state = false; }
            }

            if (!validarCampo(txtModalProveedorFichaT)) { state = false; }
            if (!validarCampo(txtFechaEntregaSitioFichaT)) { state = false; }
            if (!validarCampo(txtLugarEntrega)) { state = false; }

            if (!validarCampo(cboProveedores)) { state = false }
            if (!validarCampo(cboTipoFichaT)) { state = false; }
            //   if (!validarCampo(txtDescripcionFichaT)) { state = false; }
            if (!validarCampo(cboModalMarcaFichaT)) { state = false; }
            if (!validarCampo(cboModalModeloFichaT)) { state = false; }
            if (!validarCampo(txtModalNoSerie)) { state = false; }
            if (!validarCampo(cboModalAniosFichaT)) { state = false; }

            return state;
        }
        function iniciarModal() {

            // dialog1.dialog('destroy');
            $("#dialog-form1").removeClass('hide');
            dialog1 = $("#dialog-form1").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                height: "auto",
                width: "auto",
                autoOpen: false
            });



            $("#dlgFormAdquisicion").removeClass('hide');
            dlgFormAdquisicion = $("#dlgFormAdquisicion").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });
            $("#dlgAutorizacion").removeClass('hide');
            dlgAutorizacion = $("#dlgAutorizacion").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });
            $('#dlgCuadroFinanciero').removeClass('hide');
            dlgCuadroFinanciero = $("#dlgCuadroFinanciero").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });
            // $("#dialog-form").removeClass('hide');
            dialog = $("#dialog-form").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                height: "auto",
                width: "auto",
                autoOpen: false
            });
            $('#dlgFormAdquisicionPrint').removeClass('hide');
            dlgFormAdquisicionPrint = $("#dlgFormAdquisicionPrint").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });

            $('#dlgFormFinancieroPrint').removeClass('hide');
            dlgFormFinancieroPrint = $("#dlgFormFinancieroPrint").dialog({
                draggable: false,
                modal: true,
                resizable: true,
                width: "100%",
                height: "100%",
                autoOpen: false,
                position: 'absolute'
            });
        }
        function fnCondicionesUso() {
            var valorCondicion = cboCondicionesUso.val();

            tbCostoEquipo.val(0);
            tbOrdenCompra.val('');
            tbUtilizacionHoras.val(0);
            tbCostoEquipoRenta.val(0);
            switch (valorCondicion) {
                case "1":
                    {
                        divCostoRenta.addClass('hide');
                        divUtilizacionHoras.addClass('hide');
                        divOrdenCompra.removeClass('hide');
                        divCostoEquipo.removeClass('hide');
                        $('#divManualesOperacion').show();
                        break;
                    }
                case "0":
                    {
                        divCostoRenta.removeClass('hide');
                        divUtilizacionHoras.removeClass('hide');
                        divOrdenCompra.addClass('hide');
                        divCostoEquipo.addClass('hide');
                        chKManualesOperacion.prop('checked', false);
                        $('#divManualesOperacion').hide();
                        break;
                    }

                default:
                    break;
            }
        }
        function setFichaTecnica() {

            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/setDataEconomicos',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: setData(), idAsignacion: Number(idAsignacionp), setImprimible: getFichaTecnica(), lugarEntrega: Number(cboLugarEntrega.val()), LibreAbordo: $("#cboLugarEntrega option:selected").text() }),
                success: function (response) {
                    if (response.success) {
                        dialog1.dialog('close');
                        AlertaGeneral("Confirmacion", "¡El registro fue creado correctamente!");
                        bootGRenta();
                    }
                    else {
                        AlertaGeneral("Alerta", "Se produjo un error al intentar grabar la información. " + response.message);
                    }
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function getFichaTecnica() {
            return {
                Proveedor: cboProveedores.find("option:selected").text(),
                ProveedorID: cboProveedores.val(),
                EntregaSitio: txtFechaEntregaSitioFichaT.val(),
                LugarEntrega: txtLugarEntrega.val(),
                OrdenCompra: tbOrdenCompra.val(),
                CostoEquipo: tbCostoEquipo.val(),
                TipoEquipo: cboTipoFichaT.val(),

                Descripcion: txtDescripcionFichaT.find("option:selected").text(), // txtDescripcionFichaT.val(),
                Marca: cboModalMarcaFichaT.find("option:selected").text(),//.val(),
                Modelo: cboModalModeloFichaT.find("option:selected").text(),//.val(),
                NoSerie: txtModalNoSerie.val(),
                Arreglo: txtArreglo.val(),
                MarcaMotor: txtMarcaMotor.val(),
                ModeloMotor: txtModeloMotor.val(),
                SerieMotor: txtSerieMotor.val(),
                ArregloMotor: txtArrelgoMotor.val(),
                CodicionesUso: cboCondicionesUso.find("option:selected").text(),//.val(),
                Adquisicion: cboAdquisicionEquipo.val(),
                añoEquipo: cboModalAniosFichaT.val(),
                LugarFabricacion: cboLugarFabricacion.val(),
                Pedimento: tbnumPedimento.val(),
                horometro: tbHorometroInicial.val(),
                CostoRenta: tbCostoEquipoRenta.val(),
                UtilizacionHoras: tbUtilizacionHoras.val(),
                TipoCambio: cboTipoMoneda.val(),
                Garantia: tbGarantia.val(),
                Comentario: txtComentarios.val(),
                tieneSeguro: chKSeguro.is(":checked"),
                ManualesOperacion: chKManualesOperacion.is('checked')
            }
        }
        function setData() {
            return {
                Proveedor: cboProveedores.find("option:selected").text(),
                ProveedorID: cboProveedores.val(),
                EntregaSitio: txtFechaEntregaSitioFichaT.val(),
                LugarEntrega: txtLugarEntrega.val(),
                OrdenCompra: tbOrdenCompra.val(),
                CostoEquipo: tbCostoEquipo.val(),
                TipoEquipo: cboTipoFichaT.val(),

                Descripcion: txtDescripcionFichaT.val(), // txtDescripcionFichaT.val(),
                Marca: cboModalMarcaFichaT.val(),//.val(),
                Modelo: cboModalModeloFichaT.val(),//.val(),
                NoSerie: txtModalNoSerie.val(),
                Arreglo: txtArreglo.val(),
                MarcaMotor: txtMarcaMotor.val(),
                ModeloMotor: txtModeloMotor.val(),
                SerieMotor: txtSerieMotor.val(),
                ArregloMotor: txtArrelgoMotor.val(),
                CodicionesUso: cboCondicionesUso.val(),//.val(),
                Adquisicion: cboAdquisicionEquipo.val(),
                añoEquipo: cboModalAniosFichaT.val(),
                LugarFabricacion: cboLugarFabricacion.val(),
                Pedimento: tbnumPedimento.val(),
                horometro: tbHorometroInicial.val(),
                CostoRenta: tbCostoEquipoRenta.val(),
                UtilizacionHoras: tbUtilizacionHoras.val(),
                TipoCambio: cboTipoMoneda.val(),
                EconomicoCC: tblEconomicoCC.val(),
                Garantia: tbGarantia.val(),
                Comentario: txtComentarios.val(),
                empresa: cboPropiedadEmpresa.val(),
                // tieneSeguro: chKSeguro.is(":checked"),
                tieneSeguro: chKSeguro.prop('checked'),
                ManualesOperacion: chKManualesOperacion.prop('checked')
            }
        }
        function FillCboGrupo() {
            if (cboFiltroTipo.val() != "") {
                cboFiltroGrupo.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboFiltroTipo.val() });
                cboFiltroGrupo.attr('disabled', false);
            }
            else {
                cboFiltroGrupo.clearCombo();
                cboFiltroGrupo.attr('disabled', true);
            }
        }
        function FillDescripcion() {
            if (cboTipoFichaT.val() != "") {
                txtDescripcionFichaT.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboTipoFichaT.val() });
                txtDescripcionFichaT.attr('disabled', false);
            }
            else {
                txtDescripcionFichaT.clearCombo();
                txtDescripcionFichaT.attr('disabled', true);
            }
        }
        function FillCboMarca() {

            if (txtDescripcionFichaT.val() != null && txtDescripcionFichaT.val() != "") {
                //fillTXTNoEconomico();
                cboModalMarcaFichaT.fillCombo('/CatMaquina/FillCboMarca_Maquina', { idGrupo: txtDescripcionFichaT.val() });
                cboModalMarcaFichaT.attr('disabled', false);

            }
            else {
                cboModalMarcaFichaT.clearCombo();
                cboModalMarcaFichaT.attr('disabled', true);
            }
        }
        function FillCboModelo() {

            if (cboModalMarcaFichaT.val() != null && cboModalMarcaFichaT.val() != "") {
                cboModalModeloFichaT.fillCombo('/CatMaquina/FillCboModelo_Maquina', { idMarca: cboModalMarcaFichaT.val() });
                cboModalModeloFichaT.attr('disabled', false);
            }
            else {
                cboModalModeloFichaT.clearCombo();
                cboModalModeloFichaT.attr('disabled', true);
            }
        }
        function LoadReporte(idSolicitud, CC) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/SolicitudEquipo/GetReporte2',
                type: "POST",
                datatype: "json",
                data: { obj: idSolicitud },
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=12&pCC=" + CC;
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };

                },
                error: function () {
                    $.unblockUI();
                }
            });
        }
        async function bootGRenta() {
            try {
                dtTblCompraRentaEquipo.clear().draw();
                response = await ejectFetchJson(getTblCRPendientes, {});
                dtTblCompraRentaEquipo.rows.add(response.tblEquiporenta).draw();
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function bootGMaquinaria() {
            try {
                dtTblEconomicos.clear().draw();
                var obj = getDataFilter();
                response = await ejectFetchJson(getTblFichaTecnica, { obj });
                dtTblEconomicos.rows.add(response.Maquinas).draw();
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function getDataFilter() {
            return {
                idTipo: cboFiltroTipo.val(),
                idGrupo: cboFiltroGrupo.val(),
                descripcion: '',
                estatus: cboFiltroTipoEconomicos.val(),
                noEconomico: txtFiltroNoEconomico.val()
            }

        }
        function createBtnAuth(auth, claseId, descripcion) {
            let clssIco = "";
            switch (true) {
                case auth === null:
                case auth.estado === 3:
                    claseId += " btn-primary";
                    clssIco = "fab fa-wpforms";
                    break;
                case auth.estado === 1:
                    claseId += " btn-success";
                    clssIco = "fas fa-check";
                    break;
                case auth.estado === 2:
                    claseId += " btn-danger";
                    clssIco = "fas fa-ban";
                    break;
                default:
                    claseId += " btn-default";
                    clssIco = "fab fa-wpforms";
                    break;
            }
            let boton = $(`<button>`, {
                type: "button",
                text: descripcion,
                class: `btn ${claseId}`
            }),
                icon = $(`<i>`, {
                    class: `${clssIco}`
                });
            boton.append(icon);
            return boton;
        }
        function iniciarGrid() {
            dtTblCompraRentaEquipo = tblCompraRentaEquipo.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title: 'Solicitud', data: 'noSolicitud', width: '6%' },
                    { title: 'Tipo Solicitud', data: 'TipoSolicitud', width: '7%' },
                    { title: 'Descripción', data: 'GrupoEquipo' },
                    { title: 'Modelo', data: 'Modelo' },
                    { title: 'Fecha En Obra', data: 'FechaPromesa', width: '7%' },
                    { title: 'Comentario', data: 'Comentario' },
                    // {
                    //     title: 'Cuadros Comparativos', data: 'CCEquipo', width: '13%', createdCell: function (td, data, rowData, row, col) {
                    //         let btnEquipo = createBtnAuth(data, "btnEquipo", "Proveedor "),
                    //             btnFinanciero = createBtnAuth(rowData.CCFinanciero, "btnFinanciero", "Financiero ");
                    //         let div = $(`<div>`, {
                    //             class: 'btn-group'
                    //         });
                    //         div.append(btnEquipo);
                    //         div.append(btnFinanciero);
                    //         $(td).append(div);
                    //     }
                    // },
                    // {
                    //     title: 'Autorizante', data: 'Comentario', width: '8%', createdCell: (td, data, rowData, row, col) => {
                    //         $(td).html(`<button type='button' class='btn btn-primary Autorizante' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-user'></span>  </button>`);
                    //     }
                    // },
                    {

                        title: 'Cuadro comparativo Financiero', data: 'btnFinanciero', width: '8%', createdCell: (td, data, rowData, row, col) => {

                            if (rowData.TipoSolicitud == "COMPRA") {
                                if (data == false) {
                                    $(td).html(`<button type='button' class='btn btn-primary CuadroComparativoFinanciero' data-CCDescripcion='${rowData.CCDescripcion}' data-Modelo='${rowData.Modelo}' data-Descripcion='${rowData.GrupoEquipo}' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                                } else {
                                    $(td).html(`<button type='button' class='btn btn-warning rptCuadroComparativoFinanciero' data-CCDescripcion='${rowData.CCDescripcion}' data-Modelo='${rowData.Modelo}' data-Descripcion='${rowData.GrupoEquipo}' data-Compra='${rowData.TipoSolicitud}' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                                }
                            } else {
                                $(td).html('');

                            }
                        }
                    },
                    {

                        title: 'Cuadro comparativo', data: 'btnAdquisicion', width: '8%', createdCell: (td, data, rowData, row, col) => {
                            if (data == false) {
                                $(td).html(`<button type='button' class='btn btn-primary CuadroComparativo' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                            } else {
                                $(td).html(`<button type='button' class='btn btn-warning rptCuadroComparativo' data-Compra='${rowData.TipoSolicitud}' data-idAsignacion='${rowData.idAsignacion}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-check'></span>  </button>`);
                            }

                        }
                    },
                    {
                        title: 'Reporte Solicitud', data: 'Comentario', width: '8%', createdCell: (td, data, rowData, row, col) => {
                            $(td).html(`<button type='button' class='btn btn-primary reporteSolicitud' data-idSolicitud='${rowData.idSolicitud}' data-CentroCostos='${rowData.CentroCostos} '><span class='glyphicon glyphicon-print'></span>  </button>`);
                        }
                    },
                    {
                        title: 'Alta Datos', data: 'Activarbutton', width: '7%', createdCell: (td, data, rowData, row, col) => {
                            //if (data == true) {
                            if (true) {
                                $(td).html(`<button type='button' class='btn btn-success AltaDatos' data-idAsignacion='${rowData.idAsignacion}' ><span class='glyphicon glyphicon-plus'></span>  </button>`);
                            } else {
                                $(td).html('');
                            }
                        }
                    }
                ],
                initComplete: function () {
                    tblCompraRentaEquipo.on('click', '.btnEquipo', function (event) {
                        let data = dtTblCompraRentaEquipo.row($(this).closest("tr")).data();
                        setCuadroComparativoEquipo(data.idAsignacion);
                    });
                },
                drawCallback: function (settings) {
                    $(".reporteSolicitud").on("click", function (e) {
                        var idSolicitud = $(this).attr('data-idSolicitud');
                        var CC = $(this).attr('data-CentroCostos');
                        LoadReporte(idSolicitud, CC);
                    });

                    $(".AltaDatos").on("click", function (e) {
                        idAsignacionp = $(this).attr('data-idAsignacion');
                        LimpiarCampos();
                        getInfoSetData(idAsignacionp);
                    });
                    $(".CuadroComparativo").on("click", function (e) {
                        idAsignacionp = 0;
                        idAsignacionp = $(this).attr('data-idAsignacion');
                        addCaracteristicas = 22;
                        columnas = 1;
                        soyAdFin = 0;
                        soyAdFin = 1;
                        addAdquisisionP(idAsignacionp);
                        // LimpiarCampos();
                        // getInfoSetData(idAsignacionp);
                        dlgFormAdquisicion.dialog("open");
                        $('#idAsignacion').val(0);
                        $('#idAsignacion').val(idAsignacionp);
                        $('#dlgFormAdquisicion').css('min-height', '850px');
                        $('#dlgFormAdquisicion').css('height', '100%');
                        fncCargarTablaIncidentes();
                        CargarAutorizantes(idAsignacionp);
                        $('#txtObra').val(''),
                            $('#txtNombreDelEquipo').val(''),
                            $('#checkCompra').prop('checked', false),
                            $('#checkRenta').prop('checked', false),
                            $('#checkRoc').prop('checked', false),
                            selAutSolicita.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        selAutSolicita1.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        selAutSolicita2.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        selAutSolicita3.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        // selAutSolicita4.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        selAutSolicita5.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                    });
                    $('.CuadroComparativoFinanciero').on("click", function (e) {
                        idAsignacionp = 0;
                        idAsignacionp = $(this).attr('data-idAsignacion');
                        soyAdFin = 0;
                        soyAdFin = 2;
                        addAdquisisionP(idAsignacionp);
                        columnasFinanciero = 1;
                        dlgCuadroFinanciero.dialog("open");
                        $('#dlgCuadroFinanciero').css('min-height', '850px');
                        $('#dlgCuadroFinanciero').css('height', '100%');
                        getTablaComparativoFinanciero();
                        txtEquipo.val($(this).attr('data-Modelo') + ' ' + $(this).attr('data-Descripcion') + ' ' + $(this).attr('data-CCDescripcion'));
                        CargarAutorizantesFinanciero(idAsignacionp);
                        selAutSolicitaFinanciero.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        // selAutSolicitaFinanciero1.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        // selAutSolicitaFinanciero2.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        // selAutSolicitaFinanciero3.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        // // selAutSolicitaFinanciero4.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');
                        selAutSolicitaFinanciero5.getAutocompleteValid(fnSelRevisa, fnSelNull, null, '/Administrativo/FormatoCambio/getUsuarioSelectWithException');

                    });
                    // $(".Autorizante").on("click", function (e) {
                    //     idAsignacionp = $(this).attr('data-idAsignacion');

                    //     dlgAutorizacion.dialog("open");
                    //     $('#dlgAutorizacion').css('min-height', '850px');
                    //     $('#dlgAutorizacion').css('height', '100%');
                    //     $('#divAceptacion').find('input').css('width','')
                    $('.rptCuadroComparativo').on("click", function (e) {
                        idAsignacionp = 0;
                        idAsignacionp = $(this).attr('data-idAsignacion');
                        $('#idAsignacion').val(idAsignacionp);
                        console.log($(this).attr('data-Compra'));
                        let renta;
                        if ($(this).attr('data-Compra') == "COMPRA") {
                            renta = true
                        } else {
                            renta = false;
                        }

                        // dlgFormAdquisicionPrint.dialog("open");
                        // $('#dlgFormAdquisicionPrint').css('min-height', '850px');
                        // $('#dlgFormAdquisicionPrint').css('height', '100%');
                        fncCargarTablaIncidentesPRINT(renta);

                    });
                    $('.rptCuadroComparativoFinanciero').on("click", function (e) {
                        idAsignacionp = 0;
                        idAsignacionp = $(this).attr('data-idAsignacion');
                        console.log($(this).attr('data-Compra'));
                        $('#idAsignacion').val(idAsignacionp);
                        //$('#txtreporteEquipo').val($(this).attr('data-Modelo')+' ' +$(this).attr('data-Descripcion')+' '+$(this).attr('data-CCDescripcion'));
                        $('#txtreporteEquipo').text($(this).attr('data-Modelo') + ' ' + $(this).attr('data-Descripcion') + ' ' + $(this).attr('data-CCDescripcion'));
                        // dlgFormFinancieroPrint.dialog("open");
                        // $('#dlgFormFinancieroPrint').css('min-height', '850px');
                        // $('#dlgFormFinancieroPrint').css('height', '100%');
                        getTablaComparativoFinancieroPRINT();
                    });

                    // });

                    // $.unblockUI();
                },
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ]
            });

            //tblCompraRentaEquipo.bootgrid({
            //    headerCssClass: '.bg-table-header',
            //    align: 'center',
            //    templates: {
            //        header: ""
            //    },
            //    formatters: {
            //        "reporteSolicitud": function (column, row) {
            //            return "<button type='button' class='btn btn-primary reporteSolicitud' data-idSolicitud='" + row.idSolicitud + "' data-CentroCostos='" + row.CentroCostos + " '>" +
            //                "<span class='glyphicon glyphicon-print'></span> " +
            //                       " </button>"
            //            ;
            //        },
            //        "AltaDatos": function (column, row) {
            //            return "<button type='button' class='btn btn-success AltaDatos' data-idAsignacion='" + row.idAsignacion + "' >" +
            //                "<span class='glyphicon glyphicon-plus'></span> " +
            //                       " </button>"
            //            ;
            //        }
            //    }
            //}).on("loaded.rs.jquery.bootgrid", function () {
            //    /* Executes after data is loaded and rendered */
            //    tblCompraRentaEquipo.find(".reporteSolicitud").on("click", function (e) {
            //        var idSolicitud = $(this).attr('data-idSolicitud');
            //        var CC = $(this).attr('data-CentroCostos');
            //        LoadReporte(idSolicitud, CC)
            //    });

            //    tblCompraRentaEquipo.find(".AltaDatos").on("click", function (e) {
            //        idAsignacionp = $(this).attr('data-idAsignacion');
            //        LimpiarCampos();
            //        getInfoSetData(idAsignacionp);



            //    });

            //});
            dtTblEconomicos = tblEconomicos.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title: 'Descripción', data: 'Economico' },
                    { title: 'Modelo', data: 'Descripcion' },
                    {
                        title: 'Reporte', data: 'Descripcion', createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<button type='button' class='btn btn-primary Reporte' data-idMaquina='" + rowData.idMaquina + "'  >" +
                                "<span class='glyphicon glyphicon-print'></span> " +
                                " </button>");
                        }
                    },
                    {
                        title: 'Edición', data: 'Descripcion', createdCell: (td, data, rowData, row, col) => {
                            $(td).html("<button type='button' class='btn btn-warning EditarDato' data-idMaquina='" + rowData.idMaquina + "' >" +
                                "<span class='glyphicon glyphicon-edit'></span> " +
                                " </button>");
                        }
                    }
                ],
                drawCallback: function (settings) {
                    $(".Reporte").on("click", function (e) {
                        var idMaquina = $(this).attr('data-idMaquina');
                        VerReporte(idMaquina);

                    });
                    $(".EditarDato").on("click", function (e) {
                        var idMaquina = $(this).attr('data-idMaquina');
                        LoadMaquina(idMaquina);

                    });
                }
            });
            //tblEconomicos.bootgrid({
            //    headerCssClass: '.bg-table-header',
            //    align: 'center',
            //    templates: {
            //        header: ""
            //    },
            //    formatters: {
            //        "Reporte": function (column, row) {
            //            return "<button type='button' class='btn btn-primary Reporte' data-idMaquina='" + row.idMaquina + "'  >" +
            //                "<span class='glyphicon glyphicon-print'></span> " +
            //                       " </button>"
            //            ;
            //        },
            //        "EditarDato": function (column, row) {
            //            return "<button type='button' class='btn btn-warning EditarDato' data-idMaquina='" + row.idMaquina + "' >" +
            //                "<span class='glyphicon glyphicon-edit'></span> " +
            //                       " </button>"
            //            ;
            //        }
            //    }
            //}).on("loaded.rs.jquery.bootgrid", function () {
            //    /* Executes after data is loaded and rendered */
            //    tblEconomicos.find(".Reporte").on("click", function (e) {
            //        var idMaquina = $(this).attr('data-idMaquina');
            //        VerReporte(idMaquina);

            //    });
            //    tblEconomicos.find(".EditarDato").on("click", function (e) {
            //        var idMaquina = $(this).attr('data-idMaquina');
            //        LoadMaquina(idMaquina);

            //    });

            //});
        }
        function getInfoSetData(idAsignacionp) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/getInfoAsignacion',
                type: 'POST',
                dataType: 'json',
                data: { id: idAsignacionp },
                success: function (response) {
                    dialog1.dialog("open");
                    var tipoId = response.tipoId;
                    var grupoId = response.grupoId;

                    if (tipoId != "" || tipoId != undefined) {
                        cboTipoFichaT.prop('disabled', false);
                    }
                    if (grupoId != "" || grupoId != undefined) {
                        txtDescripcionFichaT.prop('disabled', false);
                    }
                    cboTipoFichaT.val(tipoId);
                    cboTipoFichaT.trigger('change');
                    txtDescripcionFichaT.val(grupoId);
                    txtDescripcionFichaT.trigger('change');



                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function VerReporte(id) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/getMaquinaFichaTecnica',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: id }),
                success: function (response) {
                    var path = "/Reportes/Vista.aspx?idReporte=25"
                    ireport.attr("src", path);
                    document.getElementById('report').onload = function () {
                        $.unblockUI();
                        openCRModal();
                    };
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function LoadMaquina(idMaquina) {
            btnGuardarEconomico.attr('data-idMaquinaria', idMaquina);
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/getInformacionMaquinaEDIT',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ id: idMaquina }),
                success: function (response) {
                    dialog.dialog("open");
                    var data = response.EditEquipo[0];
                    if (cboFiltroTipoEconomicos.val() == 0) {
                        txtModalNoEconomico.val(response.NumEconomico);
                    }
                    else {
                        txtModalNoEconomico.val(data.noEconomico);
                    }
                    cboModalAnios.fillCombo('/CatMaquina/FillCbo_Anios');
                    cboModalTipoEncierro.fillCombo('/CatMaquina/FillCbo_TipoEncierro');


                    cboEditTipoMaquinaria.fillCombo('/CatMaquina/FillCboTipo_Maquina', { estatus: true });
                    cboEditTipoMaquinaria.val(data.Tipo);
                    cboModalGrupoMaquinaria.fillCombo('/CatMaquina/FillCboGrupo_Maquina', { idTipo: cboEditTipoMaquinaria.val() });
                    cboModalGrupoMaquinaria.val(data.Grupo);
                    txtModalDescripcion.val(data.Descripcion);
                    cboModalMarca.fillCombo('/CatMaquina/FillCboMarca_Maquina', { idGrupo: cboModalGrupoMaquinaria.val() });
                    cboModalMarca.val(data.Marca);
                    cboModalAnios.val(data.Año);
                    cboModalModelo.fillCombo('/CatMaquina/FillCboModelo_Maquina', { idMarca: cboModalMarca.val() });
                    cboModalModelo.val(data.Modelo);
                    txtModalNoSerieEdit.val(data.noSerie);
                    cboModalAseguradoras.fillCombo('/CatMaquina/FillCboAseguradora_Maquina', { estatus: true });
                    cboModalAseguradoras.val(data.Aseguradora);
                    txtModalPoliza.val(data.Poliza);
                    dateModalVigenciaPoliza.val(data.FechaPoliza);
                    txtModalPlaca.val(data.Placas);
                    cboModalEquipoRenta.val(data.renta == true ? '1' : '0');
                    // cboModalTipoEncierro.val(data.TipoEncierro);
                    cboModalTipoCombustible.fillCombo('/CatMaquina/FillCbo_TipoCombustible');
                    txtModalCapTanque.val(data.CapacidadTanque);
                    txtModalProveedor.val(data.Proveedor);
                    txtModalHorasAdquisicion.val(data.horometroAdquisicion);
                    txtModalHorometroActual.val(data.horometroActual);
                    cboEmpresa.val(data.empresa);
                    chKTieneSeguro.prop('checked', data.tieneSeguro);
                    //txtFechaEntregaSitioFichaT.val();
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function GuardarData() {
            var json = {};
            json.id = btnGuardarEconomico.attr('data-idMaquinaria');
            json.noEconomico = txtModalNoEconomico.val();
            json.descripcion = txtModalDescripcion.val();
            json.grupoMaquinariaID = cboModalGrupoMaquinaria.val();
            json.modeloEquipoID = cboModalModelo.val();
            json.marcaID = cboModalMarca.val();
            json.anio = cboModalAnios.val();
            json.placas = '';
            json.noSerie = txtModalNoSerieEdit.val();
            json.aseguradoraID = cboModalAseguradoras.val();
            json.noPoliza = txtModalPoliza.val();
            json.TipoCombustibleID = cboModalTipoCombustible.val();
            json.capacidadTanque = txtModalCapTanque.val();
            json.unidadCarga = '';
            json.capacidadCarga = '';
            json.horometroAdquisicion = txtModalHorasAdquisicion.val();
            json.horometroActual = txtModalHorometroActual.val();
            json.estatus = true;
            json.renta = '' == 1 ? true : false;
            json.tipoEncierro = cboModalTipoEncierro.val();
            json.fechaPoliza = dateModalVigenciaPoliza.val();
            json.fechaAdquisicion = dateModalFechaAdquiere.val();
            json.fechaEntregaSitio = txtFechaEntregaSitioFichaT.val();
            json.proveedor = txtModalProveedor.val();
            json.TipoCaptura = cboTipoCaptura.val();
            json.EconomicoCC = tblEconomicoCCAlta.val();
            json.CargoEmpresa = cboCargoEmpresa.val();
            json.empresa = cboEmpresa.val();
            json.tieneSeguro = chKTieneSeguro.prop('checked');
            dialog.dialog("close");

            saveOrUpdate(json);

        }
        function saveOrUpdate(obj) {
            $.blockUI({ message: mensajes.PROCESANDO });
            $.ajax({
                url: '/CatMaquina/updateEconomico',
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify({ obj: obj }),
                success: function (response) {

                    ConfirmacionGeneral("Confirmación", "El Registro fue actualizado correctamente");

                    bootGMaquinaria();

                    txtDescripcionFichaT.prop('disabled', true);
                    cboTipoFichaT.prop('disabled', true);
                    $.unblockUI();
                },
                error: function (response) {
                    $.unblockUI();
                    AlertaGeneral("Alerta", response.message);
                }
            });
        }
        function addAdquisisionP(idAsignacionp, nuevoCuadro) {
            let objFiltro = fncGetFiltros2(idAsignacionp);
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/addAdquisisionP",
                data: objFiltro,
                success: function (response) {
                    $('#idComparativo').val(response.items.idComparativo);

                    if (nuevoCuadro) {
                        addEditTabla();
                    }
                    if (response.items.estatus == 2) {
                        btnNuevo.css('visibility', 'hidden');
                        btnAgregarColumna.css('visibility', 'hidden');
                        btnCerrarCuadroComparativo.css('visibility', 'hidden');
                        estatus2 = response.items.estatus;
                    } else {
                        btnNuevo.css('visibility', 'visible');
                        btnAgregarColumna.css('visibility', 'visible');
                        btnCerrarCuadroComparativo.css('visibility', 'visible');
                        estatus2 = response.items.estatus;
                    }
                    if (response.items.estatusFinanciera == 2) {
                        btnLimpiarFinanciero.css('visibility', 'hidden');
                        btnNuevoFinanciero.css('visibility', 'hidden');
                        btnAgregarColumnaFinanciero.css('visibility', 'hidden');
                        btnCerrarCuadroComparativoFinanciero.css('visibility', 'hidden');
                        estatusFinanciera = response.items.estatusFinanciera;
                    } else {
                        btnLimpiarFinanciero.css('visibility', 'visible');
                        btnNuevoFinanciero.css('visibility', 'visible');
                        btnAgregarColumnaFinanciero.css('visibility', 'visible');
                        btnCerrarCuadroComparativoFinanciero.css('visibility', 'visible');
                        estatusFinanciera = response.items.estatusFinanciera;
                    }
                    if (soyAdFin == 1) {
                        folio = '';
                        folio = response.items.folioAdquisicion;
                    } else {
                        folio = '';
                        folio = response.items.folioFinanciera;
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        function fncGetFiltros2(idAsignacionp) {
            let item = {};

            item = { idAsignacion: idAsignacionp };

            return item;
        }
        function LimpiarCampos() {
            txtModalProveedorFichaT.val('');
            txtFechaEntregaSitioFichaT.datepicker().datepicker("setDate", new Date());
            txtLugarEntrega.val('');
            tbOrdenCompra.val('');
            tbCostoEquipo.val('');
            tbCostoEquipoRenta.val('');
            tbUtilizacionHoras.val('');
            cboTipoMoneda.val(1);
            cboTipoFichaT.val('');
            cboTipoFichaT.trigger('change');
            txtDescripcionFichaT.trigger('change');
            cboModalMarcaFichaT.trigger('change');
            cboModalModeloFichaT.trigger('change');
            txtModalNoSerie.val('');
            txtArreglo.val('');
            txtMarcaMotor.val('');
            txtModeloMotor.val('');
            txtSerieMotor.val('');
            txtArrelgoMotor.val('');
            cboCondicionesUso.val(1);
            cboAdquisicionEquipo.val(1);
            cboModalAniosFichaT.val('');
            tbHorometroInicial.val(0);
            cboLugarFabricacion.val(1);
            tbnumPedimento.val('');
            tbGarantia.val('');
            txtComentarios.val('');
            chKSeguro.prop("checked", false);
            chKManualesOperacion.prop('checked', false);
        }
        let fncBtnBuscar = function () {
            btnBuscar.click(function (e) {
                fncCargarTablaIncidentes();
            });
        }
        let fncBtnNuevo = function () {
            btnNuevo.click(function (e) {
                if (_guardarNuevo) {
                    addAdquisisionP(0, true);
                } else {
                    // if (ChecarCamposVacios()==true) {

                    addEditTabla();
                    addCaracteristicas = 22;

                    // }else{
                    //     AlertaGeneral("Aviso","No puede guardar con los campos vacios en autorizante.");
                    // }
                }
                _guardarNuevo = false;
            });
        }
        const VerModalConfirmarCerrar = function () {
            $('#opinionGeneral').val('');
            dlgFormAdquisicion.dialog("close");
            dlgCuadroFinanciero.dialog("close");
            $('#tblComparativoAdquisicionyRenta').find('tr').remove();
            $('#tblComparativoFinanciero').find('tr').remove();
            AlertaGeneral("Folio", "Numero de folio:" + folio);
        }
        let establecerAdquisicion = function (lstAdquisicion) {

            const columnas = [
                {
                    data: 'header'
                    , render: (data, type, row) => {
                        if (data == 'Caracteristica1' || data == 'Caracteristica2' || data == 'Caracteristica3' || data == 'Caracteristica4' || data == 'Caracteristica5' || data == 'Caracteristica6' || data == 'Caracteristica7') {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                            return html;
                        } else if (data == 'Caracteristicas del equipo') {
                            let html = '';
                            html += '<div id=' + data + '>' + data + '</div>';
                            return html;

                        } else {
                            let html = '';
                            if (data != null) {
                                html += '<div id=' + data + '>' + data + '</div>';
                            }
                            return html;

                        }
                    }
                },
                {
                    data: 'txtIdnumero1', render: (data, type, row) => {
                        if (data == "idMarcaNum1Caracteristicas") {
                            let html = '';
                            html += '<button id="' + data + '" type="button" class="btn btn-primary" placeholder=""> Agregar Caracteristicas </button>';
                            return html;
                        } else if (data == 'inputAgregarImagen1') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg" id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {

                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control " placeholder="">';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero2', render: (data, type, row) => {

                        if (data == "idMarcaNum2Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen2') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + '  class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero3', render: (data, type, row) => {
                        if (data == "idMarcaNum3Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen3') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero4'
                    , render: (data, type, row) => {
                        if (data == "idMarcaNum4Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen4') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="" >';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero5'
                    , render: (data, type, row) => {
                        if (data == "idMarcaNum5Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen5') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero6'
                    , render: (data, type, row) => {
                        if (data == "idMarcaNum6Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen6') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                            return html;
                        }
                    }
                },
                {
                    data: 'txtIdnumero7'
                    , render: (data, type, row) => {
                        if (data == "idMarcaNum7Caracteristicas") {
                            let html = '';
                            html += '';
                            return html;
                        } else if (data == 'inputAgregarImagen7') {
                            let html = '';
                            html += '<div class="image-upload">' +
                                '<label for=' + data + ' class="' + data + '">' +
                                '<center><img src="https://icon-library.net/images/upload-photo-icon/upload-photo-icon-21.jpg"  id="img' + data + '" style="width:30%"/>' +
                                '</center></label>' +
                                '<input id=' + data + ' class="' + data + '" type="file" style="display:none;" />' +
                                ' </div>';
                            return html;

                        } else {
                            let html = '';
                            html += '<input id="' + data + '" type="text" class="form-control" placeholder="">';
                            return html;
                        }
                    }
                },
            ];

            //  lstAdquisicion.forEach(x => 
            //      columnas.push({ data: x.header , title: x.Item2 })


            //     );

            initDataTblM_AdquisicionyRenta(lstAdquisicion, columnas);
        }
        var initDataTblM_AdquisicionyRenta = function (data, columns) {
            if (dtComaparativo != null) {
                dtComaparativo.clear().destroy();
                tblComparativo.empty();
                dtComaparativo.draw();

            }

            dtComaparativo = tblComparativo.DataTable({
                destroy: true,
                language: dtDicEsp,
                data,
                paging: false,
                ordering: false,
                searching: false,
                order: [[3, "asc"], [1, "asc"]],
                // fixedHeader: true,
                columns,
                scrollX: true,
                scrollY: false,
                scrollCollapse: true,
                columnDefs: [
                    // { className: "dt-center", "targets": "_all" }
                    { className: "dt-head-left", "targets": "_all" },
                    { className: "dt-body-left", "targets": "_all" }
                    //dt[-head|-body]-left
                ]
                , drawCallback: function (settings) {


                    $('.inputAgregarImagen1').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen1');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen1').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen1').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen1').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen2').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen2');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen2').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen2').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen2').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen3').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen3');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen3').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen3').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen3').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen4').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen4');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen4').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen4').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen4').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen5').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen5');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen5').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen5').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen5').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen6').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen6');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen6').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen6').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen6').attr('src', imgdefault)
                        }
                    });
                    $('.inputAgregarImagen7').on("change", function (e) {
                        var fileInput = document.getElementById('inputAgregarImagen7');
                        var reader = new FileReader();
                        reader.readAsDataURL(fileInput.files[0]);
                        if (document.getElementById('inputAgregarImagen7').files[0].type == "image/jpeg") {
                            reader.onload = function () {
                                console.log(reader.result)
                                $('#imginputAgregarImagen7').attr('src', reader.result)
                            };
                        } else {
                            $('#imginputAgregarImagen7').attr('src', imgdefault)
                        }
                    });

                }
            });

            // Al agregar columnas estáticas, agregarlas de esta forma para evitar el error: "fixedColumns already initialised on this table"
            // new $.fn.dataTable.FixedColumns(dtComaparativo, {
            //     leftColumns: 1
            // });
        }
        var fncCargarTablaIncidentes = function () {
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAdquisicion",
                data: { objFiltro: objFiltro },
                success: function (response) {
                    if (response.success) {

                        getTablaComparativoAdquisicionDetalle();
                        establecerAdquisicion(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        var getTablaComparativoAdquisicionDetalle = function () {
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAdquisicionDetalle",
                data: objFiltro,
                success: function (response) {
                    if (response.success) {

                        if (response.items.length != 0) {
                            NumeroMayor = response.items[0].numeroMayor == undefined ? 0 : response.items[0].numeroMayor;
                            columnas = response.items.length + 1;
                        }
                        formatearLista(response.items);
                        acomadorInput(NumeroMayor, response.items.length);

                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        const formatearLista = function (lstDatos) {
            lstDatos.forEach(x => {



                if (x.idRow == 1) {
                    idDet1 = 0;
                    idDet1 = x.idDet;
                    $('#idMarcaNum1Marca').val(x.marcaModelo);
                    $('#idMarcaNum1proveedor').val(x.proveedor)
                    $('#idMarcaNum1precio').val(x.precioDeVenta)
                    $('#idMarcaNum1Trade').val(x.tradeIn)
                    $('#idMarcaNum1Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum1Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum1PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum1BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum1Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum1Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum1Horas').val(x.horas)
                    $('#idMarcaNum1Seguro').val(x.seguro)
                    $('#idMarcaNum1Garantia').val(x.garantia)
                    $('#idMarcaNum1Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum1Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum1Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum1Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum1Flete').val(x.flete)
                    $('#idMarcaNum1Condiciones').val(x.condicionesDePagoEntrega)

                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica1 = 0;
                            idCaracteristica1 = y.id;
                            $('#idMarcaNum1Caracteristicas11').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica2 = 0;
                            idCaracteristica2 = y.id;
                            $('#idMarcaNum1Caracteristicas12').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica3 = 0;
                            idCaracteristica3 = y.id;
                            $('#idMarcaNum1Caracteristicas13').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica4 = 0;
                            idCaracteristica4 = y.id;
                            $('#idMarcaNum1Caracteristicas14').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica5 = 0;
                            idCaracteristica5 = y.id;
                            $('#idMarcaNum1Caracteristicas15').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica6 = 0;
                            idCaracteristica6 = y.id;
                            $('#idMarcaNum1Caracteristicas16').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica7 = 0;
                            idCaracteristica7 = y.id;
                            $('#idMarcaNum1Caracteristicas17').val(y.Descripcion)
                        }
                    });
                }

                if (x.idRow == 2) {
                    idDet2 = 0;
                    idDet2 = x.idDet;
                    $('#idMarcaNum2Marca').val(x.marcaModelo)
                    $('#idMarcaNum2proveedor').val(x.proveedor)
                    $('#idMarcaNum2precio').val(x.precioDeVenta)
                    $('#idMarcaNum2Trade').val(x.tradeIn)
                    $('#idMarcaNum2Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum2Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum2PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum2BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum2Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum2Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum2Horas').val(x.horas)
                    $('#idMarcaNum2Seguro').val(x.seguro)
                    $('#idMarcaNum2Garantia').val(x.garantia)
                    $('#idMarcaNum2Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum2Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum2Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum2Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum2Flete').val(x.flete)
                    $('#idMarcaNum2Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica21 = 0;
                            idCaracteristica21 = y.id;
                            $('#idMarcaNum2Caracteristicas21').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica22 = 0;
                            idCaracteristica22 = y.id;
                            $('#idMarcaNum2Caracteristicas22').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica23 = 0;
                            idCaracteristica23 = y.id;
                            $('#idMarcaNum2Caracteristicas23').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica24 = 0;
                            idCaracteristica24 = y.id;
                            $('#idMarcaNum2Caracteristicas24').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica25 = 0;
                            idCaracteristica25 = y.id;
                            $('#idMarcaNum2Caracteristicas25').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica26 = 0;
                            idCaracteristica26 = y.id;
                            $('#idMarcaNum2Caracteristicas26').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica27 = 0;
                            idCaracteristica27 = y.id;
                            $('#idMarcaNum2Caracteristicas27').val(y.Descripcion)
                        }
                    });

                }
                if (x.idRow == 3) {

                    idDet3 = 0;
                    idDet3 = x.idDet;
                    $('#idMarcaNum3Marca').val(x.marcaModelo)
                    $('#idMarcaNum3proveedor').val(x.proveedor)
                    $('#idMarcaNum3precio').val(x.precioDeVenta)
                    $('#idMarcaNum3Trade').val(x.tradeIn)
                    $('#idMarcaNum3Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum3Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum3PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum3BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum3Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum3Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum3Horas').val(x.horas)
                    $('#idMarcaNum3Seguro').val(x.seguro)
                    $('#idMarcaNum3Garantia').val(x.garantia)
                    $('#idMarcaNum3Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum3Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum3Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum3Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum3Flete').val(x.flete)
                    $('#idMarcaNum3Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica31 = 0;
                            idCaracteristica31 = y.id;
                            $('#idMarcaNum3Caracteristicas31').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica32 = 0;
                            idCaracteristica32 = y.id;
                            $('#idMarcaNum3Caracteristicas32').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica33 = 0;
                            idCaracteristica33 = y.id;
                            $('#idMarcaNum3Caracteristicas33').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica34 = 0;
                            idCaracteristica34 = y.id;
                            $('#idMarcaNum3Caracteristicas34').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica35 = 0;
                            idCaracteristica35 = y.id;
                            $('#idMarcaNum3Caracteristicas35').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica36 = 0;
                            idCaracteristica36 = y.id;
                            $('#idMarcaNum3Caracteristicas36').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica37 = 0;
                            idCaracteristica37 = y.id;
                            $('#idMarcaNum3Caracteristicas37').val(y.Descripcion)
                        }
                    });
                }
                if (x.idRow == 4) {


                    idDet4 = 0;
                    idDet4 = x.idDet;
                    $('#idMarcaNum4Marca').val(x.marcaModelo)
                    $('#idMarcaNum4proveedor').val(x.proveedor)
                    $('#idMarcaNum4precio').val(x.precioDeVenta)
                    $('#idMarcaNum4Trade').val(x.tradeIn)
                    $('#idMarcaNum4Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum4Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum4PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum4BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum4Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum4Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum4Horas').val(x.horas)
                    $('#idMarcaNum4Seguro').val(x.seguro)
                    $('#idMarcaNum4Garantia').val(x.garantia)
                    $('#idMarcaNum4Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum4Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum4Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum4Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum4Flete').val(x.flete)
                    $('#idMarcaNum4Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica41 = 0;
                            idCaracteristica41 = y.id;
                            $('#idMarcaNum4Caracteristicas41').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica42 = 0;
                            idCaracteristica42 = y.id;
                            $('#idMarcaNum4Caracteristicas42').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica43 = 0;
                            idCaracteristica43 = y.id;
                            $('#idMarcaNum4Caracteristicas43').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica44 = 0;
                            idCaracteristica44 = y.id;
                            $('#idMarcaNum4Caracteristicas44').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica45 = 0;
                            idCaracteristica45 = y.id;
                            $('#idMarcaNum4Caracteristicas45').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica46 = 0;
                            idCaracteristica46 = y.id;
                            $('#idMarcaNum4Caracteristicas46').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica47 = 0;
                            idCaracteristica47 = y.id;
                            $('#idMarcaNum4Caracteristicas47').val(y.Descripcion)
                        }
                    });
                }
                if (x.idRow == 5) {


                    idDet5 = 0;
                    idDet5 = x.idDet;
                    $('#idMarcaNum5Marca').val(x.marcaModelo)
                    $('#idMarcaNum5proveedor').val(x.proveedor)
                    $('#idMarcaNum5precio').val(x.precioDeVenta)
                    $('#idMarcaNum5Trade').val(x.tradeIn)
                    $('#idMarcaNum5Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum5Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum5PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum5BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum5Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum5Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum5Horas').val(x.horas)
                    $('#idMarcaNum5Seguro').val(x.seguro)
                    $('#idMarcaNum5Garantia').val(x.garantia)
                    $('#idMarcaNum5Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum5Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum5Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum5Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum5Flete').val(x.flete)
                    $('#idMarcaNum5Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica51 = 0;
                            idCaracteristica51 = y.id;
                            $('#idMarcaNum5Caracteristicas51').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica52 = 0;
                            idCaracteristica52 = y.id;
                            $('#idMarcaNum5Caracteristicas52').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica53 = 0;
                            idCaracteristica53 = y.id;
                            $('#idMarcaNum5Caracteristicas53').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica54 = 0;
                            idCaracteristica54 = y.id;
                            $('#idMarcaNum5Caracteristicas54').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica55 = 0;
                            idCaracteristica55 = y.id;
                            $('#idMarcaNum5Caracteristicas55').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica56 = 0;
                            idCaracteristica56 = y.id;
                            $('#idMarcaNum5Caracteristicas56').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica57 = 0;
                            idCaracteristica57 = y.id;
                            $('#idMarcaNum5Caracteristicas57').val(y.Descripcion)
                        }
                    });

                }

                if (x.idRow == 6) {


                    idDet6 = 0;
                    idDet6 = x.idDet;
                    $('#idMarcaNum6Marca').val(x.marcaModelo)
                    $('#idMarcaNum6proveedor').val(x.proveedor)
                    $('#idMarcaNum6precio').val(x.precioDeVenta)
                    $('#idMarcaNum6Trade').val(x.tradeIn)
                    $('#idMarcaNum6Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum6Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum6PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum6BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum6Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum6Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum6Horas').val(x.horas)
                    $('#idMarcaNum6Seguro').val(x.seguro)
                    $('#idMarcaNum6Garantia').val(x.garantia)
                    $('#idMarcaNum6Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum6Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum6Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum6Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum6Flete').val(x.flete)
                    $('#idMarcaNum6Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica61 = 0;
                            idCaracteristica61 = y.id;
                            $('#idMarcaNum6Caracteristicas61').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica62 = 0;
                            idCaracteristica62 = y.id;
                            $('#idMarcaNum6Caracteristicas62').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica63 = 0;
                            idCaracteristica63 = y.id;
                            $('#idMarcaNum6Caracteristicas63').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica64 = 0;
                            idCaracteristica64 = y.id;
                            $('#idMarcaNum6Caracteristicas64').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica65 = 0;
                            idCaracteristica65 = y.id;
                            $('#idMarcaNum6Caracteristicas65').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica66 = 0;
                            idCaracteristica66 = y.id;
                            $('#idMarcaNum6Caracteristicas66').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica67 = 0;
                            idCaracteristica67 = y.id;
                            $('#idMarcaNum6Caracteristicas67').val(y.Descripcion)
                        }
                    });
                }
                if (x.idRow == 7) {

                    idDet7 = 0;
                    idDet7 = x.idDet;
                    $('#idMarcaNum7Marca').val(x.marcaModelo)
                    $('#idMarcaNum7proveedor').val(x.proveedor)
                    $('#idMarcaNum7precio').val(x.precioDeVenta)
                    $('#idMarcaNum7Trade').val(x.tradeIn)
                    $('#idMarcaNum7Valores').val(x.valoresDeRecompra)
                    $('#idMarcaNum7Precio').val(x.precioDeRentaPura)
                    $('#idMarcaNum7PrecioRoc').val(x.precioDeRentaEnRoc)
                    $('#idMarcaNum7BaseHoras').val(x.baseHoras)
                    $('#idMarcaNum7Tiempo').val(x.tiempoDeEntrega)
                    $('#idMarcaNum7Ubicacion').val(x.ubicacion)
                    $('#idMarcaNum7Horas').val(x.horas)
                    $('#idMarcaNum7Seguro').val(x.seguro)
                    $('#idMarcaNum7Garantia').val(x.garantia)
                    $('#idMarcaNum7Servicios').val(x.serviciosPreventivos)
                    $('#idMarcaNum7Capacitacion').val(x.capacitacion)
                    $('#idMarcaNum7Deposito').val(x.depositoEnGarantia)
                    $('#idMarcaNum7Lugar').val(x.lugarDeEntrega)
                    $('#idMarcaNum7Flete').val(x.flete)
                    $('#idMarcaNum7Condiciones').val(x.condicionesDePagoEntrega)
                    x.lstCaracteristicas.forEach(y => {

                        if (y.idRow == 1) {
                            idCaracteristica71 = 0;
                            idCaracteristica71 = y.id;
                            $('#idMarcaNum7Caracteristicas71').val(y.Descripcion)
                        }
                        if (y.idRow == 2) {
                            idCaracteristica72 = 0;
                            idCaracteristica72 = y.id;
                            $('#idMarcaNum7Caracteristicas72').val(y.Descripcion)
                        }
                        if (y.idRow == 3) {
                            idCaracteristica73 = 0;
                            idCaracteristica73 = y.id;
                            $('#idMarcaNum7Caracteristicas73').val(y.Descripcion)
                        }
                        if (y.idRow == 4) {
                            idCaracteristica74 = 0;
                            idCaracteristica74 = y.id;
                            $('#idMarcaNum7Caracteristicas74').val(y.Descripcion)
                        }
                        if (y.idRow == 5) {
                            idCaracteristica75 = 0;
                            idCaracteristica75 = y.id;
                            $('#idMarcaNum7Caracteristicas75').val(y.Descripcion)
                        }
                        if (y.idRow == 6) {
                            idCaracteristica76 = 0;
                            idCaracteristica76 = y.id;
                            $('#idMarcaNum7Caracteristicas76').val(y.Descripcion)
                        }
                        if (y.idRow == 7) {
                            idCaracteristica77 = 0;
                            idCaracteristica77 = y.id;
                            $('#idMarcaNum7Caracteristicas77').val(y.Descripcion)
                        }
                    });
                }



            });
        }
        const addEditTabla = function () {
            let data = CrearLstObjeto();
            console.log(data);
            $.ajax({
                datatype: "json",
                type: "POST",
                contentType: false,
                processData: false,
                url: "/CatMaquina/addeditTablaComparativoAdiquisicion",
                data: data,

                success: function (response) {
                    if (response.success) {
                        guardarAutorizacion(false);

                        // fncCargarTablaIncidentes();
                        // formatearLista(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                }
            });
        }
        const CrearLstObjeto = function () {
            let objComparativo = [];

            var formData = new FormData();



            if ($('#idMarcaNum1Marca').val() != "" &&
                $('#idMarcaNum1proveedor').val() != "" &&
                $('#idMarcaNum1precio').val() != "" &&
                $('#idMarcaNum1Trade').val() != "" &&
                $('#idMarcaNum1Valores').val() != "" &&
                $('#idMarcaNum1Precio').val() != "" &&
                $('#idMarcaNum1PrecioRoc').val() != "" &&
                $('#idMarcaNum1BaseHoras').val() != "" &&
                $('#idMarcaNum1Tiempo').val() != "" &&
                $('#idMarcaNum1Ubicacion').val() != "" &&
                $('#idMarcaNum1Horas').val() != "" &&
                $('#idMarcaNum1Seguro').val() != "" &&
                $('#idMarcaNum1Garantia').val() != "" &&
                $('#idMarcaNum1Servicios').val() != "" &&
                $('#idMarcaNum1Capacitacion').val() != "" &&
                $('#idMarcaNum1Deposito').val() != "" &&
                $('#idMarcaNum1Lugar').val() != "" &&
                $('#idMarcaNum1Flete').val() != "" &&
                $('#idMarcaNum1Condiciones').val() != "") {

                formData.append("objComparativo[0][idRow]", 1);
                formData.append("objComparativo[0][idDet]", idDet1);
                formData.append("objComparativo[0][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[0][marcaModelo]", $('#idMarcaNum1Marca').val());
                formData.append("objComparativo[0][proveedor]", $('#idMarcaNum1proveedor').val());
                formData.append("objComparativo[0][precioDeVenta]", $('#idMarcaNum1precio').val());
                formData.append("objComparativo[0][tradeIn]", $('#idMarcaNum1Trade').val());
                formData.append("objComparativo[0][valoresDeRecompra]", $('#idMarcaNum1Valores').val());
                formData.append("objComparativo[0][precioDeRentaPura]", $('#idMarcaNum1Precio').val());
                formData.append("objComparativo[0][precioDeRentaEnRoc]", $('#idMarcaNum1PrecioRoc').val());
                formData.append("objComparativo[0][baseHoras]", $('#idMarcaNum1BaseHoras').val());
                formData.append("objComparativo[0][tiempoDeEntrega]", $('#idMarcaNum1Tiempo').val());
                formData.append("objComparativo[0][ubicacion]", $('#idMarcaNum1Ubicacion').val());
                formData.append("objComparativo[0][horas]", $('#idMarcaNum1Horas').val());
                formData.append("objComparativo[0][seguro]", $('#idMarcaNum1Seguro').val());
                formData.append("objComparativo[0][garantia]", $('#idMarcaNum1Garantia').val());
                formData.append("objComparativo[0][serviciosPreventivos]", $('#idMarcaNum1Servicios').val());
                formData.append("objComparativo[0][capacitacion]", $('#idMarcaNum1Capacitacion').val());
                formData.append("objComparativo[0][depositoEnGarantia]", $('#idMarcaNum1Deposito').val());
                formData.append("objComparativo[0][lugarDeEntrega]", $('#idMarcaNum1Lugar').val());
                formData.append("objComparativo[0][flete]", $('#idMarcaNum1Flete').val());
                formData.append("objComparativo[0][condicionesDePagoEntrega]", $('#idMarcaNum1Condiciones').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[0][caracteristicasDelEquipo7]", $('#Caracteristica7').val());
                formData.append("objComparativo[0][tipoMoneda]", $('#printTipoMoneda').val())

                let lstCaracteristicas = crearlstCaracteristicas(1);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[0][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });
                if (document.getElementById("inputAgregarImagen1") != null) {
                    var file1 = document.getElementById("inputAgregarImagen1").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum2Marca').val() != "" &&
                $('#idMarcaNum2proveedor').val() != "" &&
                $('#idMarcaNum2precio').val() != "" &&
                $('#idMarcaNum2Trade').val() != "" &&
                $('#idMarcaNum2Valores').val() != "" &&
                $('#idMarcaNum2Precio').val() != "" &&
                $('#idMarcaNum2PrecioRoc').val() != "" &&
                $('#idMarcaNum2BaseHoras').val() != "" &&
                $('#idMarcaNum2Tiempo').val() != "" &&
                $('#idMarcaNum2Ubicacion').val() != "" &&
                $('#idMarcaNum2Horas').val() != "" &&
                $('#idMarcaNum2Seguro').val() != "" &&
                $('#idMarcaNum2Garantia').val() != "" &&
                $('#idMarcaNum2Servicios').val() != "" &&
                $('#idMarcaNum2Capacitacion').val() != "" &&
                $('#idMarcaNum2Deposito').val() != "" &&
                $('#idMarcaNum2Lugar').val() != "" &&
                $('#idMarcaNum2Flete').val() != "" &&
                $('#idMarcaNum2Condiciones').val() != "") {

                formData.append("objComparativo[1][idRow]", 2);
                formData.append("objComparativo[1][idDet]", idDet2);
                formData.append("objComparativo[1][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[1][marcaModelo]", $('#idMarcaNum2Marca').val());
                formData.append("objComparativo[1][proveedor]", $('#idMarcaNum2proveedor').val());
                formData.append("objComparativo[1][precioDeVenta]", $('#idMarcaNum2precio').val());
                formData.append("objComparativo[1][tradeIn]", $('#idMarcaNum2Trade').val());
                formData.append("objComparativo[1][valoresDeRecompra]", $('#idMarcaNum2Valores').val());
                formData.append("objComparativo[1][precioDeRentaPura]", $('#idMarcaNum2Precio').val());
                formData.append("objComparativo[1][precioDeRentaEnRoc]", $('#idMarcaNum2PrecioRoc').val());
                formData.append("objComparativo[1][baseHoras]", $('#idMarcaNum2BaseHoras').val());
                formData.append("objComparativo[1][tiempoDeEntrega]", $('#idMarcaNum2Tiempo').val());
                formData.append("objComparativo[1][ubicacion]", $('#idMarcaNum2Ubicacion').val());
                formData.append("objComparativo[1][horas]", $('#idMarcaNum2Horas').val());
                formData.append("objComparativo[1][seguro]", $('#idMarcaNum2Seguro').val());
                formData.append("objComparativo[1][garantia]", $('#idMarcaNum2Garantia').val());
                formData.append("objComparativo[1][serviciosPreventivos]", $('#idMarcaNum2Servicios').val());
                formData.append("objComparativo[1][capacitacion]", $('#idMarcaNum2Capacitacion').val());
                formData.append("objComparativo[1][depositoEnGarantia]", $('#idMarcaNum2Deposito').val());
                formData.append("objComparativo[1][lugarDeEntrega]", $('#idMarcaNum2Lugar').val());
                formData.append("objComparativo[1][flete]", $('#idMarcaNum2Flete').val());
                formData.append("objComparativo[1][condicionesDePagoEntrega]", $('#idMarcaNum2Condiciones').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica3').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica4').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica5').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo2]", $('#Caracteristica6').val());
                formData.append("objComparativo[1][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


                let lstCaracteristicas = crearlstCaracteristicas(2);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[1][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });


                if (document.getElementById("inputAgregarImagen2") != null) {
                    var file1 = document.getElementById("inputAgregarImagen2").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum3Marca').val() != "" &&
                $('#idMarcaNum3proveedor').val() != "" &&
                $('#idMarcaNum3precio').val() != "" &&
                $('#idMarcaNum3Trade').val() != "" &&
                $('#idMarcaNum3Valores').val() != "" &&
                $('#idMarcaNum3Precio').val() != "" &&
                $('#idMarcaNum3PrecioRoc').val() != "" &&
                $('#idMarcaNum3BaseHoras').val() != "" &&
                $('#idMarcaNum3Tiempo').val() != "" &&
                $('#idMarcaNum3Ubicacion').val() != "" &&
                $('#idMarcaNum3Horas').val() != "" &&
                $('#idMarcaNum3Seguro').val() != "" &&
                $('#idMarcaNum3Garantia').val() != "" &&
                $('#idMarcaNum3Servicios').val() != "" &&
                $('#idMarcaNum3Capacitacion').val() != "" &&
                $('#idMarcaNum3Deposito').val() != "" &&
                $('#idMarcaNum3Lugar').val() != "" &&
                $('#idMarcaNum3Flete').val() != "" &&
                $('#idMarcaNum3Condiciones').val() != "") {


                formData.append("objComparativo[2][idRow]", 3);
                formData.append("objComparativo[2][idDet]", idDet3);
                formData.append("objComparativo[2][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[2][marcaModelo]", $('#idMarcaNum3Marca').val());
                formData.append("objComparativo[2][proveedor]", $('#idMarcaNum3proveedor').val());
                formData.append("objComparativo[2][precioDeVenta]", $('#idMarcaNum3precio').val());
                formData.append("objComparativo[2][tradeIn]", $('#idMarcaNum3Trade').val());
                formData.append("objComparativo[2][valoresDeRecompra]", $('#idMarcaNum3Valores').val());
                formData.append("objComparativo[2][precioDeRentaPura]", $('#idMarcaNum3Precio').val());
                formData.append("objComparativo[2][precioDeRentaEnRoc]", $('#idMarcaNum3PrecioRoc').val());
                formData.append("objComparativo[2][baseHoras]", $('#idMarcaNum3BaseHoras').val());
                formData.append("objComparativo[2][tiempoDeEntrega]", $('#idMarcaNum3Tiempo').val());
                formData.append("objComparativo[2][ubicacion]", $('#idMarcaNum3Ubicacion').val());
                formData.append("objComparativo[2][horas]", $('#idMarcaNum3Horas').val());
                formData.append("objComparativo[2][seguro]", $('#idMarcaNum3Seguro').val());
                formData.append("objComparativo[2][garantia]", $('#idMarcaNum3Garantia').val());
                formData.append("objComparativo[2][serviciosPreventivos]", $('#idMarcaNum3Servicios').val());
                formData.append("objComparativo[2][capacitacion]", $('#idMarcaNum3Capacitacion').val());
                formData.append("objComparativo[2][depositoEnGarantia]", $('#idMarcaNum3Deposito').val());
                formData.append("objComparativo[2][lugarDeEntrega]", $('#idMarcaNum3Lugar').val());
                formData.append("objComparativo[2][flete]", $('#idMarcaNum3Flete').val());
                formData.append("objComparativo[2][condicionesDePagoEntrega]", $('#idMarcaNum3Condiciones').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[2][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


                let lstCaracteristicas = crearlstCaracteristicas(3);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[2][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });

                if (document.getElementById("inputAgregarImagen3") != null) {
                    var file1 = document.getElementById("inputAgregarImagen3").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum4Marca').val() != "" &&
                $('#idMarcaNum4proveedor').val() != "" &&
                $('#idMarcaNum4precio').val() != "" &&
                $('#idMarcaNum4Trade').val() != "" &&
                $('#idMarcaNum4Valores').val() != "" &&
                $('#idMarcaNum4Precio').val() != "" &&
                $('#idMarcaNum4PrecioRoc').val() != "" &&
                $('#idMarcaNum4BaseHoras').val() != "" &&
                $('#idMarcaNum4Tiempo').val() != "" &&
                $('#idMarcaNum4Ubicacion').val() != "" &&
                $('#idMarcaNum4Horas').val() != "" &&
                $('#idMarcaNum4Seguro').val() != "" &&
                $('#idMarcaNum4Garantia').val() != "" &&
                $('#idMarcaNum4Servicios').val() != "" &&
                $('#idMarcaNum4Capacitacion').val() != "" &&
                $('#idMarcaNum4Deposito').val() != "" &&
                $('#idMarcaNum4Lugar').val() != "" &&
                $('#idMarcaNum4Flete').val() != "" &&
                $('#idMarcaNum4Condiciones').val() != "") {
                formData.append("objComparativo[3][idRow]", 4);
                formData.append("objComparativo[3][idDet]", idDet4);
                formData.append("objComparativo[3][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[3][marcaModelo]", $('#idMarcaNum4Marca').val());
                formData.append("objComparativo[3][proveedor]", $('#idMarcaNum4proveedor').val());
                formData.append("objComparativo[3][precioDeVenta]", $('#idMarcaNum4precio').val());
                formData.append("objComparativo[3][tradeIn]", $('#idMarcaNum4Trade').val());
                formData.append("objComparativo[3][valoresDeRecompra]", $('#idMarcaNum4Valores').val());
                formData.append("objComparativo[3][precioDeRentaPura]", $('#idMarcaNum4Precio').val());
                formData.append("objComparativo[3][precioDeRentaEnRoc]", $('#idMarcaNum4PrecioRoc').val());
                formData.append("objComparativo[3][baseHoras]", $('#idMarcaNum4BaseHoras').val());
                formData.append("objComparativo[3][tiempoDeEntrega]", $('#idMarcaNum4Tiempo').val());
                formData.append("objComparativo[3][ubicacion]", $('#idMarcaNum4Ubicacion').val());
                formData.append("objComparativo[3][horas]", $('#idMarcaNum4Horas').val());
                formData.append("objComparativo[3][seguro]", $('#idMarcaNum4Seguro').val());
                formData.append("objComparativo[3][garantia]", $('#idMarcaNum4Garantia').val());
                formData.append("objComparativo[3][serviciosPreventivos]", $('#idMarcaNum4Servicios').val());
                formData.append("objComparativo[3][capacitacion]", $('#idMarcaNum4Capacitacion').val());
                formData.append("objComparativo[3][depositoEnGarantia]", $('#idMarcaNum4Deposito').val());
                formData.append("objComparativo[3][lugarDeEntrega]", $('#idMarcaNum4Lugar').val());
                formData.append("objComparativo[3][flete]", $('#idMarcaNum4Flete').val());
                formData.append("objComparativo[3][condicionesDePagoEntrega]", $('#idMarcaNum4Condiciones').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[3][caracteristicasDelEquipo7]", $('#Caracteristica7').val());

                let lstCaracteristicas = crearlstCaracteristicas(4);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[3][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });
                if (document.getElementById("inputAgregarImagen4") != null) {
                    var file1 = document.getElementById("inputAgregarImagen4").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum5Marca').val() != "" &&
                $('#idMarcaNum5proveedor').val() != "" &&
                $('#idMarcaNum5precio').val() != "" &&
                $('#idMarcaNum5Trade').val() != "" &&
                $('#idMarcaNum5Valores').val() != "" &&
                $('#idMarcaNum5Precio').val() != "" &&
                $('#idMarcaNum5PrecioRoc').val() != "" &&
                $('#idMarcaNum5BaseHoras').val() != "" &&
                $('#idMarcaNum5Tiempo').val() != "" &&
                $('#idMarcaNum5Ubicacion').val() != "" &&
                $('#idMarcaNum5Horas').val() != "" &&
                $('#idMarcaNum5Seguro').val() != "" &&
                $('#idMarcaNum5Garantia').val() != "" &&
                $('#idMarcaNum5Servicios').val() != "" &&
                $('#idMarcaNum5Capacitacion').val() != "" &&
                $('#idMarcaNum5Deposito').val() != "" &&
                $('#idMarcaNum5Lugar').val() != "" &&
                $('#idMarcaNum5Flete').val() != "" &&
                $('#idMarcaNum5Condiciones').val() != "") {
                formData.append("objComparativo[4][idRow]", 5);
                formData.append("objComparativo[4][idDet]", idDet5);
                formData.append("objComparativo[4][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[4][marcaModelo]", $('#idMarcaNum5Marca').val());
                formData.append("objComparativo[4][proveedor]", $('#idMarcaNum5proveedor').val());
                formData.append("objComparativo[4][precioDeVenta]", $('#idMarcaNum5precio').val());
                formData.append("objComparativo[4][tradeIn]", $('#idMarcaNum5Trade').val());
                formData.append("objComparativo[4][valoresDeRecompra]", $('#idMarcaNum5Valores').val());
                formData.append("objComparativo[4][precioDeRentaPura]", $('#idMarcaNum5Precio').val());
                formData.append("objComparativo[4][precioDeRentaEnRoc]", $('#idMarcaNum5PrecioRoc').val());
                formData.append("objComparativo[4][baseHoras]", $('#idMarcaNum5BaseHoras').val());
                formData.append("objComparativo[4][tiempoDeEntrega]", $('#idMarcaNum5Tiempo').val());
                formData.append("objComparativo[4][ubicacion]", $('#idMarcaNum5Ubicacion').val());
                formData.append("objComparativo[4][horas]", $('#idMarcaNum5Horas').val());
                formData.append("objComparativo[4][seguro]", $('#idMarcaNum5Seguro').val());
                formData.append("objComparativo[4][garantia]", $('#idMarcaNum5Garantia').val());
                formData.append("objComparativo[4][serviciosPreventivos]", $('#idMarcaNum5Servicios').val());
                formData.append("objComparativo[4][capacitacion]", $('#idMarcaNum5Capacitacion').val());
                formData.append("objComparativo[4][depositoEnGarantia]", $('#idMarcaNum5Deposito').val());
                formData.append("objComparativo[4][lugarDeEntrega]", $('#idMarcaNum5Lugar').val());
                formData.append("objComparativo[4][flete]", $('#idMarcaNum5Flete').val());
                formData.append("objComparativo[4][condicionesDePagoEntrega]", $('#idMarcaNum5Condiciones').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[4][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


                let lstCaracteristicas = crearlstCaracteristicas(5);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[4][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });
                if (document.getElementById("inputAgregarImagen5") != null) {
                    var file1 = document.getElementById("inputAgregarImagen5").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum6Marca').val() != "" &&
                $('#idMarcaNum6proveedor').val() != "" &&
                $('#idMarcaNum6precio').val() != "" &&
                $('#idMarcaNum6Trade').val() != "" &&
                $('#idMarcaNum6Valores').val() != "" &&
                $('#idMarcaNum6Precio').val() != "" &&
                $('#idMarcaNum6PrecioRoc').val() != "" &&
                $('#idMarcaNum6BaseHoras').val() != "" &&
                $('#idMarcaNum6Tiempo').val() != "" &&
                $('#idMarcaNum6Ubicacion').val() != "" &&
                $('#idMarcaNum6Horas').val() != "" &&
                $('#idMarcaNum6Seguro').val() != "" &&
                $('#idMarcaNum6Garantia').val() != "" &&
                $('#idMarcaNum6Servicios').val() != "" &&
                $('#idMarcaNum6Capacitacion').val() != "" &&
                $('#idMarcaNum6Deposito').val() != "" &&
                $('#idMarcaNum6Lugar').val() != "" &&
                $('#idMarcaNum6Flete').val() != "" &&
                $('#idMarcaNum6Condiciones').val() != "") {
                formData.append("objComparativo[5][idRow]", 6);
                formData.append("objComparativo[5][idDet]", idDet6);
                formData.append("objComparativo[5][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[5][marcaModelo]", $('#idMarcaNum6Marca').val());
                formData.append("objComparativo[5][proveedor]", $('#idMarcaNum6proveedor').val());
                formData.append("objComparativo[5][precioDeVenta]", $('#idMarcaNum6precio').val());
                formData.append("objComparativo[5][tradeIn]", $('#idMarcaNum6Trade').val());
                formData.append("objComparativo[5][valoresDeRecompra]", $('#idMarcaNum6Valores').val());
                formData.append("objComparativo[5][precioDeRentaPura]", $('#idMarcaNum6Precio').val());
                formData.append("objComparativo[5][precioDeRentaEnRoc]", $('#idMarcaNum6PrecioRoc').val());
                formData.append("objComparativo[5][baseHoras]", $('#idMarcaNum6BaseHoras').val());
                formData.append("objComparativo[5][tiempoDeEntrega]", $('#idMarcaNum6Tiempo').val());
                formData.append("objComparativo[5][ubicacion]", $('#idMarcaNum6Ubicacion').val());
                formData.append("objComparativo[5][horas]", $('#idMarcaNum6Horas').val());
                formData.append("objComparativo[5][seguro]", $('#idMarcaNum6Seguro').val());
                formData.append("objComparativo[5][garantia]", $('#idMarcaNum6Garantia').val());
                formData.append("objComparativo[5][serviciosPreventivos]", $('#idMarcaNum6Servicios').val());
                formData.append("objComparativo[5][capacitacion]", $('#idMarcaNum6Capacitacion').val());
                formData.append("objComparativo[5][depositoEnGarantia]", $('#idMarcaNum6Deposito').val());
                formData.append("objComparativo[5][lugarDeEntrega]", $('#idMarcaNum6Lugar').val());
                formData.append("objComparativo[5][flete]", $('#idMarcaNum6Flete').val());
                formData.append("objComparativo[5][condicionesDePagoEntrega]", $('#idMarcaNum6Condiciones').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[5][caracteristicasDelEquipo7]", $('#Caracteristica7').val());


                let lstCaracteristicas = crearlstCaracteristicas(6);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[5][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });
                if (document.getElementById("inputAgregarImagen7") != null) {
                    var file1 = document.getElementById("inputAgregarImagen6").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }

            if ($('#idMarcaNum7Marca').val() != "" &&
                $('#idMarcaNum7proveedor').val() != "" &&
                $('#idMarcaNum7precio').val() != "" &&
                $('#idMarcaNum7Trade').val() != "" &&
                $('#idMarcaNum7Valores').val() != "" &&
                $('#idMarcaNum7Precio').val() != "" &&
                $('#idMarcaNum7PrecioRoc').val() != "" &&
                $('#idMarcaNum7BaseHoras').val() != "" &&
                $('#idMarcaNum7Tiempo').val() != "" &&
                $('#idMarcaNum7Ubicacion').val() != "" &&
                $('#idMarcaNum7Horas').val() != "" &&
                $('#idMarcaNum7Seguro').val() != "" &&
                $('#idMarcaNum7Garantia').val() != "" &&
                $('#idMarcaNum7Servicios').val() != "" &&
                $('#idMarcaNum7Capacitacion').val() != "" &&
                $('#idMarcaNum7Deposito').val() != "" &&
                $('#idMarcaNum7Lugar').val() != "" &&
                $('#idMarcaNum7Flete').val() != "" &&
                $('#idMarcaNum7Condiciones').val() != "") {
                formData.append("objComparativo[6][idRow]", 7);
                formData.append("objComparativo[6][idDet]", idDet7);
                formData.append("objComparativo[6][idComparativo]", $('#idComparativo').val());
                formData.append("objComparativo[6][marcaModelo]", $('#idMarcaNum7Marca').val());
                formData.append("objComparativo[6][proveedor]", $('#idMarcaNum7proveedor').val());
                formData.append("objComparativo[6][precioDeVenta]", $('#idMarcaNum7precio').val());
                formData.append("objComparativo[6][tradeIn]", $('#idMarcaNum7Trade').val());
                formData.append("objComparativo[6][valoresDeRecompra]", $('#idMarcaNum7Valores').val());
                formData.append("objComparativo[6][precioDeRentaPura]", $('#idMarcaNum7Precio').val());
                formData.append("objComparativo[6][precioDeRentaEnRoc]", $('#idMarcaNum7PrecioRoc').val());
                formData.append("objComparativo[6][baseHoras]", $('#idMarcaNum7BaseHoras').val());
                formData.append("objComparativo[6][tiempoDeEntrega]", $('#idMarcaNum7Tiempo').val());
                formData.append("objComparativo[6][ubicacion]", $('#idMarcaNum7Ubicacion').val());
                formData.append("objComparativo[6][horas]", $('#idMarcaNum7Horas').val());
                formData.append("objComparativo[6][seguro]", $('#idMarcaNum7Seguro').val());
                formData.append("objComparativo[6][garantia]", $('#idMarcaNum7Garantia').val());
                formData.append("objComparativo[6][serviciosPreventivos]", $('#idMarcaNum7Servicios').val());
                formData.append("objComparativo[6][capacitacion]", $('#idMarcaNum7Capacitacion').val());
                formData.append("objComparativo[6][depositoEnGarantia]", $('#idMarcaNum7Deposito').val());
                formData.append("objComparativo[6][lugarDeEntrega]", $('#idMarcaNum7Lugar').val());
                formData.append("objComparativo[6][flete]", $('#idMarcaNum7Flete').val());
                formData.append("objComparativo[6][condicionesDePagoEntrega]", $('#idMarcaNum7Condiciones').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo1]", $('#Caracteristica1').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo2]", $('#Caracteristica2').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo3]", $('#Caracteristica3').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo4]", $('#Caracteristica4').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo5]", $('#Caracteristica5').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo6]", $('#Caracteristica6').val());
                formData.append("objComparativo[6][caracteristicasDelEquipo7]", $('#Caracteristica7').val());

                let lstCaracteristicas = crearlstCaracteristicas(7);

                let conlst = 0;
                lstCaracteristicas.forEach(x => {
                    formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][idRow]", x.idRow);
                    formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][idComparativoDetalle]", x.idComparativoDetalle);
                    formData.append("objComparativo[6][lstCaracteristicas][" + conlst + "][Descripcion]", x.Descripcion);
                    conlst++;
                });
                if (document.getElementById("inputAgregarImagen7") != null) {
                    var file1 = document.getElementById("inputAgregarImagen7").files[0];
                    if (file1 != undefined) {
                        formData.append("file", file1);
                    }
                }
            }


            return formData;
        }
        const fncGetFiltros = function () {
            let item = {};
            item = { idAsignacion: $('#idAsignacion').val() }
            return item;
        }
        const acomadorInput = function (data, numeroDeListas) {

            // $('#Marca').css('width', '649px')
            $('#idMarcaNum1Marca').css('width', '210px');
            $('#idMarcaNum2Marca').css('width', '210px');
            $('#idMarcaNum3Marca').css('width', '210px');
            $('#idMarcaNum4Marca').css('width', '210px');
            $('#idMarcaNum5Marca').css('width', '210px');
            $('#idMarcaNum6Marca').css('width', '210px');
            $('#idMarcaNum7Marca').css('width', '210px');
            // $('.form-control').css('width','100%');
            $('.dataTables_scrollHead').css('display', 'none');
            $('#tblComparativoAdquisicionyRenta').find('td').css('padding', 0)

            var tabla = $('#tblComparativoAdquisicionyRenta').find("tr").each(function (e) {
            });
            addCaracteristicas = (intCaracteristicas + addCaracteristicas + NumeroMayor);

            for (let i = 0; i < tabla.length; i++) {

                if (i >= addCaracteristicas) {
                    let item = tabla[i];
                    $(item).css('visibility', 'hidden');
                }
            }
            numeroDeListas = numeroDeListas + 1;
            for (let i = 0; i < 8; i++) {
                if (i <= numeroDeListas) {
                } else {
                    var table = $('#tblComparativoAdquisicionyRenta').DataTable();
                    table.columns([i]).visible(false);
                }
            }

            // $('#tblComparativoAdquisicionyRenta').css('position', 'relative');
            // $('#tblComparativoAdquisicionyRenta').css('right', '670px');
            // $("#tblComparativoAdquisicionyRenta tbody").css("align", "left"); //TODO
            // $("#tblComparativoAdquisicionyRenta").css("", "left"); //TODO

            let tr = $('#tblComparativoAdquisicionyRenta').find('tr')
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                $(td[0]).css('font-weight', 'bold')
            }

            $('#idMarcaNum1Caracteristicas').on("click", function (e) {
                var tabla = $('#tblComparativoAdquisicionyRenta').find("tr").each(function (e) {
                });

                if (addCaracteristicas <= tabla.length) {
                    for (let i = 0; i < tabla.length; i++) {
                        if (i == addCaracteristicas) {
                            let item = tabla[i];
                            $(item).css('visibility', 'visible');

                        }
                    }
                    addCaracteristicas++;

                } else {
                }
            });
            $('#tblComparativoAdquisicionyRenta').css('margin', '0')
            if (estatus2 == 2) {

                $('#tblComparativoAdquisicionyRenta').find('input').prop('disabled', true)
            } else {
                $('#tblComparativoAdquisicionyRenta').find('input').prop('disabled', false)

            }
        }
        const crearlstCaracteristicas = function (Tipo) {
            let lstReturn = [];

            switch (Tipo) {
                case 1:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 2:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 3:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 4:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 5:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 6:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                case 7:
                    for (let i = 0; i < 7; i++) {
                        let item = {};
                        if (item != {}) {
                            if (i == 0) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 1) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 2) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 3) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 4) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 5) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }
                            if (i == 6) {
                                item = objCaracteristicas(Tipo, i);
                                lstReturn.push(item);
                            }

                        }
                    }
                    break;
                default:
                    break;
            }
            return lstReturn;
        }
        const objCaracteristicas = function (Tipo, idRow) {
            let objReturn = {};

            switch (Tipo) {
                case 1:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica1,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas11').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica2,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas12').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica3,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas13').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica4,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas14').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica5,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas15').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica6,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas16').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica7,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum1Caracteristicas17').val()
                        };
                    }
                    break;
                case 2:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica21,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas21').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica22,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas22').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica23,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas23').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica24,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas24').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica25,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas25').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica26,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas26').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica27,
                            idComparativoDetalle: idDet2,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum2Caracteristicas27').val()
                        };
                    }
                    break;
                case 3:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica31,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas31').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica32,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas32').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica33,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas33').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica34,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas34').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica35,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas35').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica36,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas36').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica37,
                            idComparativoDetalle: idDet3,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum3Caracteristicas37').val()
                        };
                    }
                    break;
                case 4:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica41,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas41').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica42,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas42').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica43,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas43').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica44,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas44').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica45,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas45').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica46,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas46').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica47,
                            idComparativoDetalle: idDet4,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum4Caracteristicas47').val()
                        };
                    }
                    break;
                case 5:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica51,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas51').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica52,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas52').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica53,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas53').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica54,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas54').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica55,
                            idComparativoDetalle: idDet1,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas55').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica56,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas56').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica57,
                            idComparativoDetalle: idDet5,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum5Caracteristicas57').val()
                        };
                    }
                    break;
                case 6:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica61,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas61').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica62,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas62').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica63,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas63').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica64,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas64').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica65,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas65').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica66,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas66').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica7,
                            idComparativoDetalle: idDet6,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum6Caracteristicas67').val()
                        };
                    }
                    break;
                case 7:
                    if (idRow == 0) {
                        objReturn = {
                            id: idCaracteristica71,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas71').val()
                        };
                    }
                    if (idRow == 1) {
                        objReturn = {
                            id: idCaracteristica72,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas72').val()
                        };
                    }
                    if (idRow == 2) {
                        objReturn = {
                            id: idCaracteristica73,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas73').val()
                        };
                    }
                    if (idRow == 3) {
                        objReturn = {
                            id: idCaracteristica74,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas74').val()
                        };
                    }
                    if (idRow == 4) {
                        objReturn = {
                            id: idCaracteristica75,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas75').val()
                        };
                    }
                    if (idRow == 5) {
                        objReturn = {
                            id: idCaracteristica76,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas76').val()
                        };
                    }
                    if (idRow == 6) {
                        objReturn = {
                            id: idCaracteristica77,
                            idComparativoDetalle: idDet7,
                            idRow: idRow + 1,
                            Descripcion: $('#idMarcaNum7Caracteristicas77').val()
                        };
                    }
                    break;
                default:
                    break;
            }

            return objReturn;
        }
        const fncAgregarColumnas = function () {
            let tr = $('#tblComparativoAdquisicionyRenta').find('tr')
            let td = $(tr[1]).find('td')
            columnas = td.length - 1;
            if (columnas <= 7) {
                columnas++;
                console.log(columnas)
            }
            if (columnas > 7) {
                columnas = 7;
            } else {
                var table = $('#tblComparativoAdquisicionyRenta').DataTable();
                table.columns([columnas]).visible(true);
            }
        }
        const fncEliminarColumna = function () {
            let tr = $('#tblComparativoAdquisicionyRenta').find('tr')
            let td = $(tr[1]).find('td')
            columnas = td.length;
            if (td.length == 2) {
            } else {

                if (columnas >= 2) {
                    columnas--;
                }
                if (columnas < 1) {
                    columnas = 1;
                } else {
                    $('#idMarcaNum' + columnas + 'Marca').val('');
                    $('#idMarcaNum' + columnas + 'proveedor').val('');
                    $('#idMarcaNum' + columnas + 'precio').val('');
                    $('#idMarcaNum' + columnas + 'Trade').val('');
                    $('#idMarcaNum' + columnas + 'Valores').val('');
                    $('#idMarcaNum' + columnas + 'Precio').val('');
                    $('#idMarcaNum' + columnas + 'PrecioRoc').val('');
                    $('#idMarcaNum' + columnas + 'BaseHoras').val('');
                    $('#idMarcaNum' + columnas + 'Tiempo').val('');
                    $('#idMarcaNum' + columnas + 'Ubicacion').val('');
                    $('#idMarcaNum' + columnas + 'Horas').val('');
                    $('#idMarcaNum' + columnas + 'Seguro').val('');
                    $('#idMarcaNum' + columnas + 'Garantia').val('');
                    $('#idMarcaNum' + columnas + 'Servicios').val('');
                    $('#idMarcaNum' + columnas + 'Capacitacion').val('');
                    $('#idMarcaNum' + columnas + 'Deposito').val('');
                    $('#idMarcaNum' + columnas + 'Lugar').val('');
                    $('#idMarcaNum' + columnas + 'Flete').val('');
                    $('#idMarcaNum' + columnas + 'Condiciones').val('');

                    var table = $('#tblComparativoAdquisicionyRenta').DataTable();
                    table.columns([columnas]).visible(false);
                }
            }
        }
        const guardarAutorizacion = function (Financiero) {
            let lstComparativo = [];
            let html = '';
            let objFiltro = {};
            if (Financiero == false) {
                lstComparativo = Parametros();
                objFiltro = {
                    obra: $('#txtObra').val(),
                    nombreDelEquipo: $('#txtNombreDelEquipo').val(),
                    compra: $('#checkCompra').prop('checked'),
                    renta: $('#checkRenta').prop('checked'),
                    roc: $('#checkRoc').prop('checked'),
                }
            } else {
                lstComparativo = ParametrosFinanciero();
                objFiltro = {};
            }
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/guardarAutorizacion",
                data: { lstComparativo: lstComparativo, objFiltro: objFiltro, Financiero: Financiero },
                success: function (response) {
                    if (response.success) {
                        VerModalConfirmarCerrar();
                        // dlgAutorizacion.dialog("close");
                        // AlertaGeneral("Guardado", "Guardado con exito.");
                        // Alert2Error("cuadro comparativo guardado con exito");
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    bootGRenta();
                }
            });
        }
        const Parametros = function () {
            let objeto = [];
            let item = {};
            item = {
                id: selAutSolicita.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita.data("id"),
                autorizanteNombre: selAutSolicita.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 1
            }
            objeto.push(item);
            item = {
                id: selAutSolicita1.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita1.data("id"),
                autorizanteNombre: selAutSolicita1.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 2
            }
            objeto.push(item);
            item = {
                id: selAutSolicita2.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita2.data("id"),
                autorizanteNombre: selAutSolicita2.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 3
            }
            objeto.push(item);
            item = {
                id: selAutSolicita3.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita3.data("id"),
                autorizanteNombre: selAutSolicita3.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 4
            }
            objeto.push(item);
            // item ={
            //     id:selAutSolicita4.attr('data-id'),
            //     idAsignacion:idAsignacionp,
            //     autorizanteID: selAutSolicita4.data("id"),
            //     autorizanteNombre:  selAutSolicita4.val(),
            //     autorizanteStatus: false,
            //     autorizanteFinal: false,
            //     orden:5
            // }
            // objeto.push(item);
            item = {
                id: selAutSolicita5.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicita5.data("id"),
                autorizanteNombre: selAutSolicita5.val(),
                autorizanteStatus: false,
                autorizanteFinal: true,
                orden: 6
            }
            objeto.push(item);
            return objeto;
        }
        const ParametrosFinanciero = function () {
            let objeto = [];
            let item = {};
            item = {
                id: selAutSolicitaFinanciero.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicitaFinanciero.data("id"),
                autorizanteNombre: selAutSolicitaFinanciero.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 1
            }
            objeto.push(item);
            item = {
                id: selAutSolicitaFinanciero1.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicitaFinanciero1.data("id"),
                autorizanteNombre: selAutSolicitaFinanciero1.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 2
            }
            objeto.push(item);
            item = {
                id: selAutSolicitaFinanciero2.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicitaFinanciero2.data("id"),
                autorizanteNombre: selAutSolicitaFinanciero2.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 3
            }
            objeto.push(item);
            item = {
                id: selAutSolicitaFinanciero3.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicitaFinanciero3.data("id"),
                autorizanteNombre: selAutSolicitaFinanciero3.val(),
                autorizanteStatus: false,
                autorizanteFinal: false,
                orden: 4
            }
            objeto.push(item);
            // item ={
            //     id:selAutSolicitaFinanciero4.attr('data-id'),
            //     idAsignacion:idAsignacionp,
            //     autorizanteID: selAutSolicitaFinanciero4.data("id"),
            //     autorizanteNombre:  selAutSolicitaFinanciero4.val(),
            //     autorizanteStatus: false,
            //     autorizanteFinal: false,
            //     orden:5
            // }
            // objeto.push(item);
            item = {
                id: selAutSolicitaFinanciero5.attr('data-id'),
                idAsignacion: idAsignacionp,
                autorizanteID: selAutSolicitaFinanciero5.data("id"),
                autorizanteNombre: selAutSolicitaFinanciero5.val(),
                autorizanteStatus: false,
                autorizanteFinal: true,
                orden: 6
            }
            objeto.push(item);
            return objeto;
        }
        const CargarAutorizantes = function (id) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/CargarAutorizadores?idAsignacion=" + id + "",
                success: function (response) {
                    if (response.success) {
                        imprimirInputs(response.items);

                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        const CargarAutorizantesFinanciero = function (id) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/CargarAutorizadoresFinanciero?idAsignacion=" + id + "",
                success: function (response) {
                    if (response.success) {
                        imprimirInputsFinanciero(response.items);
                        initGpxInteresVsEfectiva('gpxInteresVsEfectiva', null, null, null);
                        initGpxPagoTotalBanco('gpxPagoTotalBanco', null, null, null);

                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        const imprimirInputs = function (lstDatos) {
            selAutSolicita.attr('data-id', '0')
            selAutSolicita.val("");
            selAutSolicita.data("id", "");
            selAutSolicita.data("nombre", "");
            selAutSolicita1.attr('data-id', '0')
            selAutSolicita1.val("");
            selAutSolicita1.data("id", "");
            selAutSolicita1.data("nombre", "");
            selAutSolicita2.attr('data-id', '0')
            selAutSolicita2.val("");
            selAutSolicita2.data("id", "");
            selAutSolicita2.data("nombre", "");
            selAutSolicita3.attr('data-id', '0')
            selAutSolicita3.val("");
            selAutSolicita3.data("id", "");
            selAutSolicita3.data("nombre", "");
            // selAutSolicita4.attr('data-id','0')
            // selAutSolicita4.val("");
            // selAutSolicita4.data("id","");
            // selAutSolicita4.data("nombre","");
            selAutSolicita5.attr('data-id', '0')
            selAutSolicita5.val("");
            selAutSolicita5.data("id", "");
            selAutSolicita5.data("nombre", "");
            lstDatos.forEach(y => {
                switch (y.orden) {
                    case 1:
                        selAutSolicita.attr('data-id', y.id)
                        selAutSolicita.val(y.autorizanteNombre);
                        selAutSolicita.data("id", y.autorizanteID);
                        selAutSolicita.data("nombre", y.autorizanteNombre);
                        break;
                    case 2:
                        selAutSolicita1.attr('data-id', y.id)
                        selAutSolicita1.val(y.autorizanteNombre);
                        selAutSolicita1.data("id", y.autorizanteID);
                        selAutSolicita1.data("nombre", y.autorizanteNombre);
                        break;
                    case 3:
                        selAutSolicita2.attr('data-id', y.id)
                        selAutSolicita2.val(y.autorizanteNombre);
                        selAutSolicita2.data("id", y.autorizanteID);
                        selAutSolicita2.data("nombre", y.autorizanteNombre);
                        break;
                    case 4:
                        selAutSolicita3.attr('data-id', y.id)
                        selAutSolicita3.val(y.autorizanteNombre);
                        selAutSolicita3.data("id", y.autorizanteID);
                        selAutSolicita3.data("nombre", y.autorizanteNombre);
                        break;
                    case 5:
                        //     selAutSolicita4.attr('data-id',y.id)
                        //     selAutSolicita4.val(y.autorizanteNombre);
                        //     selAutSolicita4.data("id", y.autorizanteID);
                        //     selAutSolicita4.data("nombre", y.autorizanteNombre);
                        //     break;
                        // case 6:
                        selAutSolicita5.attr('data-id', y.id)
                        selAutSolicita5.val(y.autorizanteNombre);
                        selAutSolicita5.data("id", y.autorizanteID);
                        selAutSolicita5.data("nombre", y.autorizanteNombre);
                        break;

                    default:
                        break;
                }


            });




        }
        const imprimirInputsFinanciero = function (lstDatos) {
            selAutSolicitaFinanciero.attr('data-id', '0')
            selAutSolicitaFinanciero.val("");
            selAutSolicitaFinanciero.data("id", "");
            selAutSolicitaFinanciero.data("nombre", "");
            selAutSolicitaFinanciero1.attr('data-id', '0')
            selAutSolicitaFinanciero1.val("");
            selAutSolicitaFinanciero1.data("id", "");
            selAutSolicitaFinanciero1.data("nombre", "");
            selAutSolicitaFinanciero2.attr('data-id', '0')
            selAutSolicitaFinanciero2.val("");
            selAutSolicitaFinanciero2.data("id", "");
            selAutSolicitaFinanciero2.data("nombre", "");
            selAutSolicitaFinanciero3.attr('data-id', '0')
            selAutSolicitaFinanciero3.val("");
            selAutSolicitaFinanciero3.data("id", "");
            selAutSolicitaFinanciero3.data("nombre", "");
            // selAutSolicitaFinanciero4.attr('data-id','0')
            // selAutSolicitaFinanciero4.val("");
            // selAutSolicitaFinanciero4.data("id","");
            // selAutSolicitaFinanciero4.data("nombre","");
            selAutSolicitaFinanciero5.attr('data-id', '0')
            selAutSolicitaFinanciero5.val("");
            selAutSolicitaFinanciero5.data("id", "");
            selAutSolicitaFinanciero5.data("nombre", "");
            lstDatos.forEach(y => {
                switch (y.orden) {
                    case 1:
                        selAutSolicitaFinanciero.attr('data-id', y.id)
                        selAutSolicitaFinanciero.val(y.autorizanteNombre);
                        selAutSolicitaFinanciero.data("id", y.autorizanteID);
                        selAutSolicitaFinanciero.data("nombre", y.autorizanteNombre);
                        break;
                    case 2:
                        selAutSolicitaFinanciero1.attr('data-id', y.id)
                        selAutSolicitaFinanciero1.val(y.autorizanteNombre);
                        selAutSolicitaFinanciero1.data("id", y.autorizanteID);
                        selAutSolicitaFinanciero1.data("nombre", y.autorizanteNombre);
                        break;
                    case 3:
                        selAutSolicitaFinanciero2.attr('data-id', y.id)
                        selAutSolicitaFinanciero2.val(y.autorizanteNombre);
                        selAutSolicitaFinanciero2.data("id", y.autorizanteID);
                        selAutSolicitaFinanciero2.data("nombre", y.autorizanteNombre);
                        break;
                    case 4:
                        selAutSolicitaFinanciero3.attr('data-id', y.id)
                        selAutSolicitaFinanciero3.val(y.autorizanteNombre);
                        selAutSolicitaFinanciero3.data("id", y.autorizanteID);
                        selAutSolicitaFinanciero3.data("nombre", y.autorizanteNombre);
                        break;
                    case 5:
                        //     selAutSolicitaFinanciero4.attr('data-id',y.id)
                        //     selAutSolicitaFinanciero4.val(y.autorizanteNombre);
                        //     selAutSolicitaFinanciero4.data("id", y.autorizanteID);
                        //     selAutSolicitaFinanciero4.data("nombre", y.autorizanteNombre);
                        //     break;
                        // case 6:
                        selAutSolicitaFinanciero5.attr('data-id', y.id)
                        selAutSolicitaFinanciero5.val(y.autorizanteNombre);
                        selAutSolicitaFinanciero5.data("id", y.autorizanteID);
                        selAutSolicitaFinanciero5.data("nombre", y.autorizanteNombre);
                        break;

                    default:
                        break;
                }


            });




        }
        const CerrarCuadroComparativo = function (dif) {

            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/editEstatusComparativo?Dif=" + dif + "&idComparativo=" + $('#idComparativo').val() + "",
                success: function (response) {
                    if (response.success) {
                        if (response.items.estatusExito == 2) {
                            AlertaGeneral("Alerta", response.items.msjExito);
                            btnNuevo.css('visibility', 'hidden');
                            btnAgregarColumna.css('visibility', 'hidden');
                            // btnCerrarCuadroComparativo.css('visibility', 'hidden');
                            btnAgregarColumnaFinanciero.css('visibility', 'hidden');
                            // btnCerrarCuadroComparativoFinanciero.css('visibility', 'hidden');
                            btnNuevoFinanciero.css('visibility', 'hidden');
                        } else {
                            AlertaGeneral("Alerta", response.items.msjExito);
                            btnNuevo.css('visibility', 'visible');
                            btnAgregarColumna.css('visibility', 'visible');
                            // btnCerrarCuadroComparativo.css('visibility', 'visible');
                            btnAgregarColumnaFinanciero.css('visibility', 'visible');
                            // btnCerrarCuadroComparativoFinanciero.css('visibility', 'visible');
                            btnNuevoFinanciero.css('visibility', 'visible');
                        }
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        const getTablaComparativoFinanciero = function () {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoFinanciero",
                success: function (response) {
                    if (response.success) {
                        dtAutorizacion.clear();
                        dtAutorizacion.rows.add(response.items);
                        dtAutorizacion.draw();
                        getTablaComparativoFinancieroDetalle();
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        var initdt_ComparativoFinanciero = function () {
            dtAutorizacion = tblComparativoFinanciero.DataTable({
                destroy: true,
                language: dtDicEsp,
                paging: false,
                ordering: false,
                searching: false,
                order: [[3, "asc"], [1, "asc"]],
                // fixedHeader: true,
                scrollX: true,
                scrollY: false,
                scrollCollapse: true,
                columnDefs: [
                    { className: "dt-center", "targets": "_all" }
                ],
                columns: [
                    {
                        data: 'header', title: 'header' //,width: '7%'
                        , render: (data, type, row) => {
                            let html = "";
                            if (data == "botones") {
                                html = '<div id="botones"></div>';
                            } else if (data == "Banco") {
                                html = '<div id="Banco">' + data + '</div>'
                            }
                            // else if(data=="checbox"){
                            //     html='Plazos por financiamiento';
                            // }
                            else {
                                html = data;
                            }
                            console.log(html);
                            return html;
                        }
                    },
                    {
                        data: 'txtIdnumero1', title: 'txtIdnumero1' //,width: '30%'
                        , render: (data, type, row) => {

                            let html = "";
                            if (data == 'btnAgregarInput') {
                                html += '<button type="button" id="btnEliminarPlazos" class="btn btn-warning btnEliminarPlazos" style="margin: 5px;"><i class="fas fa-minus"></i></button>';
                                html += '<button type="button" id="btnAgregarPlazos" class="btn btn-warning btnAgregarPlazos" style="margin: 5px;"><i class="fas fa-plus"></i></button>';
                            } else {
                                html = '<div id=' + data + '></div>';
                            }
                            return html;
                        }
                    },
                    {
                        data: 'txtIdnumero2', title: 'txtIdnumero2' //,width: '30%'
                        , render: (data, type, row) => {

                            let html = "";
                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    },
                    {
                        data: 'txtIdnumero3', title: 'txtIdnumero3'//,width: '30%'
                        , render: (data, type, row) => {

                            let html = "";

                            html = '<div id=' + data + '></div>';
                            return html;
                        }
                    }
                ],
                columnDefs: [
                    { className: "dt-center", "targets": "_all" },
                    //{ "width": "40%", "targets": [0, 2, 3] }
                ],
                drawCallback: function (settings) {

                    $('.btnEliminarPlazos').on("click", function (e) {

                        limpiarGpx(false, fncEliminarPlazos);
                        //fncEliminarPlazos();
                    });
                    $('.btnAgregarPlazos').on("click", function (e) {

                        fncAgregarPlazos();
                    });
                },
                createdCell: function (td, cellData, rowData, row, col) {
                    $(td).css('display', 'in-linea');
                    $(td).css('white-space', 'no-wrap');
                }

            });
            $('#tblComparativoFinanciero_length').css('visibility', 'hidden');
            $('#tblComparativoFinanciero_filter').css('visibility', 'hidden');
            $('#tblComparativoFinanciero_length').css('display', 'none');
            $('#tblComparativoFinanciero_filter').css('display', 'none');
            $('.dataTables_scrollHead').css('display', 'none');
            $('.dataTables_scrollHead').css('visibility', 'hidden');

            AcomodarFinanciero();
            BotoonesFinanciero();
        }
        function fncEliminarPlazos() {
            let lengthFinanciera1 = $('#banco1').find('select').length == undefined ? 0 : $('#banco1').find('select').length;
            lengthFinanciera1--;
            if (lengthFinanciera1 < 1) {
                lengthFinanciera1 = 1;
            } else {
                let tr = $('#tblComparativoFinanciero').find('tr')
                for (let index = 0; index < tr.length; index++) {
                    let td = $(tr[index]).find('td');
                    let input = $(td[1]).find('input');
                    let select = $(td[1]).find('select');
                    let button = $(td[1]).find('button');
                    $(input[lengthFinanciera1]).remove();
                    $(select[lengthFinanciera1]).remove();
                    $('.mensulialidad1').eq(lengthFinanciera1).remove();
                }

            }

            let lengthFinanciera2 = $('#banco2').find('select').length == undefined ? 0 : $('#banco2').find('select').length;
            lengthFinanciera2--;
            if (lengthFinanciera2 < 1) {
                lengthFinanciera2 = 1;
            } else {
                let tr = $('#tblComparativoFinanciero').find('tr')
                for (let index = 0; index < tr.length; index++) {
                    let td = $(tr[index]).find('td');
                    let input = $(td[2]).find('input');
                    let select = $(td[2]).find('select');
                    let button = $(td[2]).find('button');
                    $(input[lengthFinanciera1]).remove();
                    $(select[lengthFinanciera1]).remove();
                    $('.mensulialidad1').eq(lengthFinanciera1).remove();
                }

            }
            let lengthFinanciera3 = $('#banco3').find('select').length == undefined ? 0 : $('#banco3').find('select').length;
            lengthFinanciera3--;
            if (lengthFinanciera3 < 1) {
                lengthFinanciera3 = 1;
            } else {
                let tr = $('#tblComparativoFinanciero').find('tr')
                for (let index = 0; index < tr.length; index++) {
                    let td = $(tr[index]).find('td');
                    let input = $(td[3]).find('input');
                    let select = $(td[3]).find('select');
                    let button = $(td[3]).find('button');
                    $(input[lengthFinanciera1]).remove();
                    $(select[lengthFinanciera1]).remove();
                    $('.mensulialidad1').eq(lengthFinanciera1).remove();
                }

            }


        }
        const fncAgregarPlazos = function () {
            let lengthFinanciera1 = $('#banco1').find('select').length == undefined ? 0 : $('#banco1').find('select').length;
            let lengthFinanciera2 = $('#banco2').find('select').length == undefined ? 0 : $('#banco2').find('select').length;
            let lengthFinanciera3 = $('#banco3').find('select').length == undefined ? 0 : $('#banco3').find('select').length;

            $('#banco1').append('<select class="comboFinanciera" style="margin: 3px; width: 184px;"></select>');
            $('#plazo1').append('<select style="margin: 3px; width: 184px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
            $('#precio1').append('<input type="text" style="margin: 3px;"/>')
            $('#tiempoRestanteProyecto1').append('<input type="text" style="margin: 3px;" />')
            $('#iva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#total1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#montoFinanciar1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tipoOperacion1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#opcionCompra1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#valorResidual1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#depositoEfectivo1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#moneda1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#plazoMeses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tasaDeInteres1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#gastosFijos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#comision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#montoComision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#rentasEnGarantia1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#crecimientoPagos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoInicial1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoTotalIntereses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tasaEfectiva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#mensualidad1').append('<input type="text" style="margin: 3px; background: #f8f8f8;" />')
            $('#mensualidad1').append('<button type="button" class="btn btn-xs btn-warning mensulialidad1" style="margin: 5px;"><i class="glyphicon glyphicon-eye-open"></i></button>')
            $('#mensualidadSinIVA1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoTotal1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')

            if (lengthFinanciera2 < 4) {
                $('#banco2').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                $('#plazo2').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                $('#precio2').append('<input type="text" style="margin: 3px;"/>')
                $('#tiempoRestanteProyecto2').append('<input type="text" style="margin: 3px;"/>')
                $('#iva2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#total2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#montoFinanciar2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tipoOperacion2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#opcionCompra2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#valorResidual2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#depositoEfectivo2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#moneda2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#plazoMeses2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tasaDeInteres2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#gastosFijos2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#comision2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#montoComision2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#rentasEnGarantia2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#crecimientoPagos2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoInicial2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoTotalIntereses2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tasaEfectiva2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#mensualidad2').append('<input type="text" style="margin: 3px; background: #f8f8f8;" />')
                $('#mensualidad2').append('<button type="button" id="mensulialidad2" class="btn btn-xs btn-warning" style="margin: 5px;"><i class="glyphicon glyphicon-eye-open"></i></button>')

                $('#mensualidadSinIVA2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoTotal2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            }

            if (lengthFinanciera3 < 4) {
                $('#banco3').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                $('#plazo3').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                $('#precio3').append('<input type="text" style="margin: 3px;"/>')
                $('#tiempoRestanteProyecto3').append('<input type="text" style="margin: 3px;"/>')
                $('#iva3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#total3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#montoFinanciar3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tipoOperacion3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#opcionCompra3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#valorResidual3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#depositoEfectivo3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#moneda3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#plazoMeses3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tasaDeInteres3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#gastosFijos3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#comision3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#montoComision3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#rentasEnGarantia3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#crecimientoPagos3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoInicial3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoTotalIntereses3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#tasaEfectiva3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#mensualidad3').append('<input type="text" style="margin: 3px; background: #f8f8f8;"  />')
                $('#mensualidad3').append('<button type="button" id="mensulialidad3" class="btn btn-xs btn-warning" style="margin: 5px;"><i class="glyphicon glyphicon-eye-open"></i></button>')
                $('#mensualidadSinIVA3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                $('#pagoTotal3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            }


            $('.select').css('width', '184px');
            $('.comboFinanciera').last().fillCombo('/CatMaquina/FillCboFinancieros');
            let ifin = $('#banco1').find('select')
            let btnAgre = $('#plazo1').find('select')
            let idPre = $('#precio1').find('input')
            let idTem = $('#tiempoRestanteProyecto1').find('input')
            let btnMens = $('#mensualidad1').find('button')
            for (let b = 0; b < ifin.length; b++) {
                let posicion = b;
                $(ifin[b]).unbind('change');
                $(ifin[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = unmaskNumero($(idPre[b]).val())
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(btnAgre[b]).unbind('change');
                $(btnAgre[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = unmkasNumero($(idPre[b]).val())
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(idPre[b]).unbind('change');
                $(idPre[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = unmaskNumero($(idPre[b]).val())
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(idTem[b]).unbind('change');
                $(idTem[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = unmaskNumero($(idPre[b]).val())
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(btnMens[b]).unbind('click');
                $(btnMens[b]).click(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    if (finan != "" && plazo != "" && precio != "") {
                        CargarMensualidadesFinanciero(finan, plazo, precio, 1, posicion);
                    }
                });
                // $(idPre[b]).unbind('keyup');
                // $(idPre[b]).keyup(function (event) {

                //     // skip for arrow keys
                //     if (event.which >= 37 && event.which <= 40) {
                //         event.preventDefault();
                //     }

                //     $(this).val(function (index, value) {
                //         return value
                //             .replace(/\D/g, "")
                //             .replace(/([0-9])([0-9]{2})$/, '$1.$2')
                //             .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",")
                //             ;
                //     });
                // });
                $(idPre[b]).on('paste', function (e) {
                    permitePegarSoloNumero2D($(this), e);
                });
                $(idPre[b]).on('keypress', function (event) {
                    aceptaSoloNumero2D($(this), event);
                });

            }

            // $('.comboFinanciera').change(function(e){
            //     let financiero = $(this).val();
            //     let plazo = $(this).parent().parent().parent().next().find('.comboPlazo').val();
            //     let precio = $(this).parent().parent().parent().next().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().next().next().next().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {
            //         CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());
            //     }    

            // });
            // $('.comboPlazo').change(function(e){
            //     let financiero = $(this).parent().parent().parent().prev().find('.comboFinanciera').val();
            //     let plazo = $(this).val();
            //     let precio = $(this).parent().parent().parent().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().next().next().next().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {
            //     CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());            
            //     }
            // });
        }
        const AgregarColumnasFinanciero = function () {

            if ($('#banco1').find('select').length != 0) {
                // $('#btnAgregarPlazos').css('visibility','hidden');
                let tr = $('#tblComparativoFinanciero').find('tr')
                let td = $(tr[1]).find('td')
                columnasFinanciero = td.length - 1;
                if (columnasFinanciero <= 3) {
                    columnasFinanciero++;
                    console.log(columnasFinanciero)
                }

                if (columnasFinanciero > 3) {
                    columnasFinanciero = 3;
                } else {
                    var table = $('#tblComparativoFinanciero').DataTable();
                    table.columns([columnasFinanciero]).visible(true);

                    for (let i = 0; i < $('#banco1').find('select').length; i++) {
                        $('#banco' + columnasFinanciero + '').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                        $('#plazo' + columnasFinanciero + '').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                        $('#precio' + columnasFinanciero + '').append('<input type="text" style="margin: 3px;"/>')
                        $('#tiempoRestanteProyecto' + columnasFinanciero + '').append('<input type="text" style="margin: 3px;"/>')
                        $('#iva' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#total' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#montoFinanciar' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#tipoOperacion' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#opcionCompra' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#valorResidual' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#depositoEfectivo' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#moneda' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#plazoMeses' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#tasaDeInteres' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#gastosFijos' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#comision' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#montoComision' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#rentasEnGarantia' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#crecimientoPagos' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#pagoInicial' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#pagoTotalIntereses' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#tasaEfectiva' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#mensualidad' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#mensualidadSinIVA' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                        $('#pagoTotal' + columnasFinanciero + '').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')


                        for (let sd = 1; sd <= 3; sd++) {
                            let ifin = $('#banco' + sd).find('select')
                            let btnAgre = $('#plazo' + sd).find('select')
                            let idPre = $('#precio' + sd).find('input')
                            let idTem = $('#tiempoRestanteProyecto' + sd).find('input')
                            let btnMens = $('#mensualidad' + sd).find('button')
                            for (let b = 0; b < ifin.length; b++) {
                                let posicion = b;
                                $(ifin[b]).unbind('change');
                                $(ifin[b]).change(function (e) {
                                    let finan = $(ifin[b]).val()
                                    let plazo = $(btnAgre[b]).val()
                                    let precio = $(idPre[b]).val()
                                    let meses = $(idTem[b]).val()
                                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                                        CargarDatosFinanciero(finan, plazo, precio, meses, sd, posicion);
                                    }
                                });
                                $(btnAgre[b]).unbind('change');
                                $(btnAgre[b]).change(function (e) {
                                    let finan = $(ifin[b]).val()
                                    let plazo = $(btnAgre[b]).val()
                                    let precio = $(idPre[b]).val()
                                    let meses = $(idTem[b]).val()
                                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                                        CargarDatosFinanciero(finan, plazo, precio, meses, sd, posicion);
                                    }
                                });
                                $(idPre[b]).unbind('change');
                                $(idPre[b]).change(function (e) {
                                    let finan = $(ifin[b]).val()
                                    let plazo = $(btnAgre[b]).val()
                                    let precio = $(idPre[b]).val()
                                    let meses = $(idTem[b]).val()
                                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                                        CargarDatosFinanciero(finan, plazo, precio, meses, sd, posicion);
                                    }
                                });
                                $(idTem[b]).unbind('change');
                                $(idTem[b]).change(function (e) {
                                    let finan = $(ifin[b]).val()
                                    let plazo = $(btnAgre[b]).val()
                                    let precio = $(idPre[b]).val()
                                    let meses = $(idTem[b]).val()
                                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                                        CargarDatosFinanciero(finan, plazo, precio, meses, sd, posicion);
                                    }
                                });
                                $(btnMens[b]).unbind('click');
                                $(btnMens[b]).click(function (e) {
                                    let finan = $(ifin[b]).val()
                                    let plazo = $(btnAgre[b]).val()
                                    let precio = $(idPre[b]).val()
                                    if (finan != "" && plazo != "" && precio != "") {
                                        CargarMensualidadesFinanciero(finan, plazo, precio, sd, posicion);
                                    }
                                });
                                // $(idPre[b]).unbind('keyup');
                                // $(idPre[b]).keyup(function (event) {

                                //     // skip for arrow keys
                                //     if (event.which >= 37 && event.which <= 40) {
                                //         event.preventDefault();
                                //     }

                                //     $(this).val(function (index, value) {
                                //         return value
                                //             .replace(/\D/g, "")
                                //             .replace(/([0-9])([0-9]{2})$/, '$1.$2')
                                //             .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",")
                                //             ;
                                //     });
                                // });
                                $(idPre[b]).on('paste', function (e) {
                                    permitePegarSoloNumero2D($(this), e);
                                });
                                $(idPre[b]).on('keypress', function (event) {
                                    aceptaSoloNumero2D($(this), event);
                                });

                            }

                        }


                    }



                    // $('#btnAgregarInput3').parent().css('background-color', '#3556ae !important;');
                    // $('#btnEliminarPlazos').parent().css('background-color', '#3556ae !important;');
                    // $('#btnAgregarInput2').parent().css('background-color', '#3556ae !important;');
                    // $('#banco1').parent().css('width','40%')
                    // if (columnasFinanciero==3) {
                    // if ($('#banco1').find('select').length==3) {
                    //     $('#banco1').css('width','700px');
                    //     $('#banco2').css('width','700px');
                    //     $('#banco3').css('width','700px');
                    //   }   
                    // }
                    // if(columnasFinanciero == 3)
                    // {
                    //   $('#banco1').css('width','800px');
                    //   $('#banco3').css('width','800px');
                    //   $('#banco2').css('width','800px');
                    // }
                }
            }
            $('#tblComparativoFinanciero').find('td').css('padding', 0)
            $('.select').css('width', '203px');
            $('.comboFinanciera').fillCombo('/CatMaquina/FillCboFinancieros');



            // $('.comboFinanciera').change(function(e){
            //     let financiero = $(this).val();
            //     let plazo = $(this).parent().parent().parent().next().find('.comboPlazo').val();
            //     let precio = $(this).parent().parent().parent().next().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().parent().children().last().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {

            //     CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());
            //     }
            // });
            // $('.comboPlazo').change(function(e){
            //     let financiero = $(this).parent().parent().parent().prev().find('.comboFinanciera').val();
            //     let plazo = $(this).val();
            //     let precio = $(this).parent().parent().parent().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().parent().children().last().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {
            //         CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());            
            //     }
            //     });
        }

        const EliminarColumnasFinanciero = function () {
            let tr = $('#tblComparativoFinanciero').find('tr')
            let td = $(tr[1]).find('td')
            columnasFinanciero = td.length;
            if (td.length == 2) {
            } else {

                if (columnasFinanciero >= 2) {
                    columnasFinanciero--;
                }
                if (columnasFinanciero < 1) {
                    columnasFinanciero = 1;
                } else {

                    $('#banco1').css('width', '');
                    $('#banco2').css('width', '');

                    $('#banco' + columnasFinanciero + '').find('select').remove();
                    $('#plazo' + columnasFinanciero + '').find('select').remove();
                    $('#precio' + columnasFinanciero + '').find('input').remove();
                    $('#tiempoRestanteProyecto' + columnasFinanciero + '').find('input').remove();
                    $('#iva' + columnasFinanciero + '').find('input').remove();
                    $('#total' + columnasFinanciero + '').find('input').remove();
                    $('#montoFinanciar' + columnasFinanciero + '').find('input').remove();
                    $('#tipoOperacion' + columnasFinanciero + '').find('input').remove();
                    $('#opcionCompra' + columnasFinanciero + '').find('input').remove();
                    $('#valorResidual' + columnasFinanciero + '').find('input').remove();
                    $('#depositoEfectivo' + columnasFinanciero + '').find('input').remove();
                    $('#moneda' + columnasFinanciero + '').find('input').remove();
                    $('#plazoMeses' + columnasFinanciero + '').find('input').remove();
                    $('#tasaDeInteres' + columnasFinanciero + '').find('input').remove();
                    $('#gastosFijos' + columnasFinanciero + '').find('input').remove();
                    $('#comision' + columnasFinanciero + '').find('input').remove();
                    $('#montoComision' + columnasFinanciero + '').find('input').remove();
                    $('#rentasEnGarantia' + columnasFinanciero + '').find('input').remove();
                    $('#crecimientoPagos' + columnasFinanciero + '').find('input').remove();
                    $('#pagoInicial' + columnasFinanciero + '').find('input').remove();
                    $('#pagoTotalIntereses' + columnasFinanciero + '').find('input').remove();
                    $('#tasaEfectiva' + columnasFinanciero + '').find('input').remove();
                    $('#mensualidad' + columnasFinanciero + '').find('input').remove();
                    $('#mensualidad' + columnasFinanciero + '').find('button').remove();
                    $('#mensualidadSinIVA' + columnasFinanciero + '').find('input').remove();
                    $('#pagoTotal' + columnasFinanciero + '').find('input').remove();

                    var table = $('#tblComparativoFinanciero').DataTable();
                    table.columns([columnasFinanciero]).visible(false);

                }
            }
        }
        const AcomodarFinanciero = function (numeroDeListasFinanciero) {
            numeroDeListasFinanciero = numeroDeListasFinanciero + 1;
            for (let i = 0; i < 8; i++) {
                if (i <= numeroDeListasFinanciero) {
                } else {
                    var table = $('#tblComparativoFinanciero').DataTable();
                    table.columns([2, 3]).visible(false);
                }
            }
        }

        function limpiarFinanciero() {
            $('#opinionGeneral').val('');
            $('#banco1').find('select').remove();
            $('#banco2').find('select').remove();
            $('#banco3').find('select').remove();
            $('#btnAgregarPlazos').css('visibility', 'visible');
            $('#btnAgregarPlazo1').find('select').remove();
            $('#btnAgregarPlazo2').find('select').remove();
            $('#btnAgregarPlazo3').find('select').remove();


            $('#plazo1').find('select').remove();
            $('#precio1').find('input').remove();
            $('#tiempoRestanteProyecto1').find('input').remove();
            $('#iva1').find('input').remove();
            $('#total1').find('input').remove();
            $('#montoFinanciar1').find('input').remove();
            $('#tipoOperacion1').find('input').remove();
            $('#opcionCompra1').find('input').remove();
            $('#valorResidual1').find('input').remove();
            $('#depositoEfectivo1').find('input').remove();
            $('#moneda1').find('input').remove();
            $('#plazoMeses1').find('input').remove();
            $('#tasaDeInteres1').find('input').remove();
            $('#gastosFijos1').find('input').remove();
            $('#comision1').find('input').remove();
            $('#montoComision1').find('input').remove();
            $('#rentasEnGarantia1').find('input').remove();
            $('#crecimientoPagos1').find('input').remove();
            $('#pagoInicial1').find('input').remove();
            $('#pagoTotalIntereses1').find('input').remove();
            $('#tasaEfectiva1').find('input').remove();
            $('#mensualidad1').find('input').remove();
            $('#mensualidad1').find('button').remove();
            $('#mensualidadSinIVA1').find('input').remove();
            $('#pagoTotal1').find('input').remove();

            $('#plazo2').find('select').remove();
            $('#precio2').find('input').remove();
            $('#tiempoRestanteProyecto2').find('input').remove();
            $('#iva2').find('input').remove();
            $('#total2').find('input').remove();
            $('#montoFinanciar2').find('input').remove();
            $('#tipoOperacion2').find('input').remove();
            $('#opcionCompra2').find('input').remove();
            $('#valorResidual2').find('input').remove();
            $('#depositoEfectivo2').find('input').remove();
            $('#moneda2').find('input').remove();
            $('#plazoMeses2').find('input').remove();
            $('#tasaDeInteres2').find('input').remove();
            $('#gastosFijos2').find('input').remove();
            $('#comision2').find('input').remove();
            $('#montoComision2').find('input').remove();
            $('#rentasEnGarantia2').find('input').remove();
            $('#crecimientoPagos2').find('input').remove();
            $('#pagoInicial2').find('input').remove();
            $('#pagoTotalIntereses2').find('input').remove();
            $('#tasaEfectiva2').find('input').remove();
            $('#mensualidad2').find('input').remove();
            $('#mensualidad2').find('button').remove();
            $('#mensualidadSinIVA2').find('input').remove();
            $('#pagoTotal2').find('input').remove();

            $('#plazo3').find('select').remove();
            $('#precio3').find('input').remove();
            $('#tiempoRestanteProyecto3').find('input').remove();
            $('#iva3').find('input').remove();
            $('#total3').find('input').remove();
            $('#montoFinanciar3').find('input').remove();
            $('#tipoOperacion3').find('input').remove();
            $('#opcionCompra3').find('input').remove();
            $('#valorResidual3').find('input').remove();
            $('#depositoEfectivo3').find('input').remove();
            $('#moneda3').find('input').remove();
            $('#plazoMeses3').find('input').remove();
            $('#tasaDeInteres3').find('input').remove();
            $('#gastosFijos3').find('input').remove();
            $('#comision3').find('input').remove();
            $('#montoComision3').find('input').remove();
            $('#rentasEnGarantia3').find('input').remove();
            $('#crecimientoPagos3').find('input').remove();
            $('#pagoInicial3').find('input').remove();
            $('#pagoTotalIntereses3').find('input').remove();
            $('#tasaEfectiva3').find('input').remove();
            $('#mensualidad3').find('input').remove();
            $('#mensualidad3').find('button').remove();
            $('#mensualidadSinIVA3').find('input').remove();
            $('#pagoTotal3').find('input').remove();

            columnasFinanciero = 1;
            var table = $('#tblComparativoFinanciero').DataTable();
            table.columns([2, 3]).visible(false);



            cargarDefault();
        }

        const BotoonesFinanciero = function () {
            btnAgregarColumnaFinanciero.click(function () {
                AgregarColumnasFinanciero();
            });
            btnEliminarColumnaFinanciero.click(function () {
                EliminarColumnasFinanciero();
            });
            btnLimpiarFinanciero.click(function () {
                limpiarGpx(true, limpiarFinanciero);
            });
            btnNuevoFinanciero.click(function () {
                // if (ChecarCamposVaciosFinanciero()==true) {
                let obj = crearObjetoGuardadoFinanciero();
                GuardarFinanciero(obj);
                // dlgCuadroFinanciero.dialog("close");
                // bootGRenta();

                // }else{
                //     AlertaGeneral("Aviso","No puede guardar con los campos vacios en autorizante.");
                // }

            });
            btnCerrarCuadroComparativoFinanciero.click(function () {
                CerrarCuadroComparativo(1);
            });
        }
        const crearObjetoGuardadoFinanciero = function () {
            let lst = [];

            //#region IDROW1
            let banco1 = $('#banco1').find('select');
            let plazo1 = $('#plazo1').find('select');
            let precioDelEquipo1 = $('#precio1').find('input');
            let tiempoRestanteProyecto1 = $('#tiempoRestanteProyecto1').find('input');
            let iva1 = $('#iva1').find('input');
            let total1 = $('#total1').find('input');
            let montoFinanciar1 = $('#montoFinanciar1').find('input');
            let tipoOperacion1 = $('#tipoOperacion1').find('input');
            let opcionCompra1 = $('#opcionCompra1').find('input');
            let valorResidual1 = $('#valorResidual1').find('input');
            let depositoEfectivo1 = $('#depositoEfectivo1').find('input');
            let moneda1 = $('#moneda1').find('input');
            let plazoMeses1 = $('#plazoMeses1').find('input');
            let tasaDeInteres1 = $('#tasaDeInteres1').find('input');
            let gastosFijos1 = $('#gastosFijos1').find('input');
            let comision1 = $('#comision1').find('input');
            let montoComision1 = $('#montoComision1').find('input');
            let rentasEnGarantia1 = $('#rentasEnGarantia1').find('input');
            let crecimientoPagos1 = $('#crecimientoPagos1').find('input');
            let pagoInicial1 = $('#pagoInicial1').find('input');
            let pagoTotalIntereses1 = $('#pagoTotalIntereses1').find('input');
            let tasaEfectiva1 = $('#tasaEfectiva1').find('input');
            let mensualidad1 = $('#mensualidad1').find('input');
            let mensualidadSinIVA1 = $('#mensualidadSinIVA1').find('input');
            let pagoTotal1 = $('#pagoTotal1').find('input');
            for (let i = 0; i < banco1.length; i++) {
                let item = {
                    id: $(banco1[i]).attr('data-id'),
                    idFinanciero: idAsignacionp,
                    idRow: 1,
                    ComentarioGeneral: $('#opinionGeneral').val(),
                    banco: $(banco1[i]).val(),
                    plazo: $(plazo1[i]).val(),
                    precioDelEquipo: $(precioDelEquipo1[i]).val(),
                    tiempoRestanteProyecto: $(tiempoRestanteProyecto1[i]).val(),
                    iva: $(iva1[i]).val(),
                    total: $(total1[i]).val(),
                    montoFinanciar: $(montoFinanciar1[i]).val(),
                    tipoOperacion: $(tipoOperacion1[i]).val(),
                    opcionCompra: $(opcionCompra1[i]).val(),
                    valorResidual: $(valorResidual1[i]).val(),
                    depositoEfectivo: $(depositoEfectivo1[i]).val(),
                    moneda: $(moneda1[i]).val(),
                    plazoMeses: $(plazoMeses1[i]).val(),
                    tasaDeInteres: $(tasaDeInteres1[i]).val(),
                    gastosFijos: $(gastosFijos1[i]).val(),
                    comision: $(comision1[i]).val(),
                    montoComision: $(montoComision1[i]).val(),
                    rentasEnGarantia: $(rentasEnGarantia1[i]).val(),
                    crecimientoPagos: $(crecimientoPagos1[i]).val(),
                    pagoInicial: $(pagoInicial1[i]).val(),
                    pagoTotalIntereses: $(pagoTotalIntereses1[i]).val(),
                    tasaEfectiva: $(tasaEfectiva1[i]).val(),
                    mensualidad: $(mensualidad1[i]).val(),
                    mensualidadSinIVA: $(mensualidadSinIVA1[i]).val(),
                    pagoTotal: $(pagoTotal1[i]).val(),
                }
                lst.push(item);
            }


            //#region IDROW2
            let banco2 = $('#banco2').find('select');
            let plazo2 = $('#plazo2').find('select');
            let precioDelEquipo2 = $('#precio2').find('input');
            let tiempoRestanteProyecto2 = $('#tiempoRestanteProyecto2').find('input');
            let iva2 = $('#iva2').find('input');
            let total2 = $('#total2').find('input');
            let montoFinanciar2 = $('#montoFinanciar2').find('input');
            let tipoOperacion2 = $('#tipoOperacion2').find('input');
            let opcionCompra2 = $('#opcionCompra2').find('input');
            let valorResidual2 = $('#valorResidual2').find('input');
            let depositoEfectivo2 = $('#depositoEfectivo2').find('input');
            let moneda2 = $('#moneda2').find('input');
            let plazoMeses2 = $('#plazoMeses2').find('input');
            let tasaDeInteres2 = $('#tasaDeInteres2').find('input');
            let gastosFijos2 = $('#gastosFijos2').find('input');
            let comision2 = $('#comision2').find('input');
            let montoComision2 = $('#montoComision2').find('input');
            let rentasEnGarantia2 = $('#rentasEnGarantia2').find('input');
            let crecimientoPagos2 = $('#crecimientoPagos2').find('input');
            let pagoInicial2 = $('#pagoInicial2').find('input');
            let pagoTotalIntereses2 = $('#pagoTotalIntereses2').find('input');
            let tasaEfectiva2 = $('#tasaEfectiva2').find('input');
            let mensualidad2 = $('#mensualidad2').find('input');
            let mensualidadSinIVA2 = $('#mensualidadSinIVA2').find('input');
            let pagoTotal2 = $('#pagoTotal2').find('input');
            for (let i = 0; i < banco2.length; i++) {
                let item = {
                    id: $(banco2[i]).attr('data-id'),
                    idFinanciero: idAsignacionp,
                    idRow: 2,
                    ComentarioGeneral: $('#opinionGeneral').val(),
                    banco: $(banco2[i]).val(),
                    plazo: $(plazo2[i]).val(),
                    precioDelEquipo: $(precioDelEquipo2[i]).val(),
                    tiempoRestanteProyecto: $(tiempoRestanteProyecto2[i]).val(),
                    iva: $(iva2[i]).val(),
                    total: $(total2[i]).val(),
                    montoFinanciar: $(montoFinanciar2[i]).val(),
                    tipoOperacion: $(tipoOperacion2[i]).val(),
                    opcionCompra: $(opcionCompra2[i]).val(),
                    valorResidual: $(valorResidual2[i]).val(),
                    depositoEfectivo: $(depositoEfectivo2[i]).val(),
                    moneda: $(moneda2[i]).val(),
                    plazoMeses: $(plazoMeses2[i]).val(),
                    tasaDeInteres: $(tasaDeInteres2[i]).val(),
                    gastosFijos: $(gastosFijos2[i]).val(),
                    comision: $(comision2[i]).val(),
                    montoComision: $(montoComision2[i]).val(),
                    rentasEnGarantia: $(rentasEnGarantia2[i]).val(),
                    crecimientoPagos: $(crecimientoPagos2[i]).val(),
                    pagoInicial: $(pagoInicial2[i]).val(),
                    pagoTotalIntereses: $(pagoTotalIntereses2[i]).val(),
                    tasaEfectiva: $(tasaEfectiva2[i]).val(),
                    mensualidad: $(mensualidad2[i]).val(),
                    mensualidadSinIVA: $(mensualidadSinIVA2[i]).val(),
                    pagoTotal: $(pagoTotal2[i]).val(),

                }
                lst.push(item);
            }

            //#region IDROW3
            let banco3 = $('#banco3').find('select');
            let plazo3 = $('#plazo3').find('select');
            let precioDelEquipo3 = $('#precio3').find('input');
            let tiempoRestanteProyecto3 = $('#tiempoRestanteProyecto3').find('input');
            let iva3 = $('#iva3').find('input');
            let total3 = $('#total3').find('input');
            let montoFinanciar3 = $('#montoFinanciar3').find('input');
            let tipoOperacion3 = $('#tipoOperacion3').find('input');
            let opcionCompra3 = $('#opcionCompra3').find('input');
            let valorResidual3 = $('#valorResidual3').find('input');
            let depositoEfectivo3 = $('#depositoEfectivo3').find('input');
            let moneda3 = $('#moneda3').find('input');
            let plazoMeses3 = $('#plazoMeses3').find('input');
            let tasaDeInteres3 = $('#tasaDeInteres3').find('input');
            let gastosFijos3 = $('#gastosFijos3').find('input');
            let comision3 = $('#comision3').find('input');
            let montoComision3 = $('#montoComision3').find('input');
            let rentasEnGarantia3 = $('#rentasEnGarantia3').find('input');
            let crecimientoPagos3 = $('#crecimientoPagos3').find('input');
            let pagoInicial3 = $('#pagoInicial3').find('input');
            let pagoTotalIntereses3 = $('#pagoTotalIntereses3').find('input');
            let tasaEfectiva3 = $('#tasaEfectiva3').find('input');
            let mensualidad3 = $('#mensualidad3').find('input');
            let mensualidadSinIVA3 = $('#mensualidadSinIVA3').find('input');
            let pagoTotal3 = $('#pagoTotal3').find('input');
            for (let i = 0; i < banco3.length; i++) {
                let item = {
                    id: $(banco3[i]).attr('data-id'),
                    idFinanciero: idAsignacionp,
                    idRow: 3,
                    ComentarioGeneral: $('#opinionGeneral').val(),
                    banco: $(banco3[i]).val(),
                    plazo: $(plazo3[i]).val(),
                    precioDelEquipo: $(precioDelEquipo3[i]).val(),
                    tiempoRestanteProyecto: $(tiempoRestanteProyecto3[i]).val(),
                    iva: $(iva3[i]).val(),
                    total: $(total3[i]).val(),
                    montoFinanciar: $(montoFinanciar3[i]).val(),
                    tipoOperacion: $(tipoOperacion3[i]).val(),
                    opcionCompra: $(opcionCompra3[i]).val(),
                    valorResidual: $(valorResidual3[i]).val(),
                    depositoEfectivo: $(depositoEfectivo3[i]).val(),
                    moneda: $(moneda3[i]).val(),
                    plazoMeses: $(plazoMeses3[i]).val(),
                    tasaDeInteres: $(tasaDeInteres3[i]).val(),
                    gastosFijos: $(gastosFijos3[i]).val(),
                    comision: $(comision3[i]).val(),
                    montoComision: $(montoComision3[i]).val(),
                    rentasEnGarantia: $(rentasEnGarantia3[i]).val(),
                    crecimientoPagos: $(crecimientoPagos3[i]).val(),
                    pagoInicial: $(pagoInicial3[i]).val(),
                    pagoTotalIntereses: $(pagoTotalIntereses3[i]).val(),
                    tasaEfectiva: $(tasaEfectiva3[i]).val(),
                    mensualidad: $(mensualidad3[i]).val(),
                    mensualidadSinIVA: $(mensualidadSinIVA3[i]).val(),
                    pagoTotal: $(pagoTotal3[i]).val(),

                }
                lst.push(item);
            }

            return lst;
        }
        const GuardarFinanciero = function (obj) {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/addeditTablaComparativoFinanciero",
                data: { objComparativo: obj },
                success: function (response) {
                    if (response.success) {
                        guardarAutorizacion(true);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                }
            });

        }
        const getTablaComparativoFinancieroDetalle = function () {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoFinancieroDetalle?idFinanciero=" + idAsignacionp + "",
                success: function (response) {
                    if (response.success) {
                        cargarDefault();
                        dibujarInputs(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }

        const cargarDefault = function () {
            $('#banco1').append('<select class="comboFinanciera" style="margin: 3px; width: 184px;"></select>');
            $('#plazo1').append('<select style="margin: 3px; width: 184px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
            $('#precio1').append('<input type="text" style="margin: 3px;"/>')
            $('#tiempoRestanteProyecto1').append('<input type="text" style="margin: 3px;"/>')
            $('#iva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#total1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#montoFinanciar1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tipoOperacion1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#opcionCompra1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#valorResidual1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#depositoEfectivo1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#moneda1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#plazoMeses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tasaDeInteres1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#gastosFijos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#comision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#montoComision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#rentasEnGarantia1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#crecimientoPagos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoInicial1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoTotalIntereses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#tasaEfectiva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#mensualidad1').append('<input type="text" style="margin: 3px; background: #f8f8f8;" />')
            $('#mensualidad1').append('<button type="button" class="btn btn-xs btn-warning mensulialidad1" style="margin: 5px;"><i class="glyphicon glyphicon-eye-open"></i></button>')
            $('#mensualidadSinIVA1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('#pagoTotal1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
            $('.comboFinanciera').fillCombo('/CatMaquina/FillCboFinancieros');
            $('#btnAgregarInput1').parent().css('background-color', '#3556ae !important;');


            let ifin = $('#banco1').find('select')
            let btnAgre = $('#plazo1').find('select')
            let idPre = $('#precio1').find('input')
            let idTem = $('#tiempoRestanteProyecto1').find('input')
            let btnMens = $('#mensualidad1').find('button')
            for (let b = 0; b < ifin.length; b++) {
                let posicion = b;
                $(ifin[b]).unbind('change');
                $(ifin[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(btnAgre[b]).unbind('change');
                $(btnAgre[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(idPre[b]).unbind('change');
                $(idPre[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(idTem[b]).unbind('change');
                $(idTem[b]).change(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    let meses = $(idTem[b]).val()
                    if (finan != "" && plazo != "" && precio != "" && meses != "") {
                        CargarDatosFinanciero(finan, plazo, precio, meses, 1, posicion);
                    }
                });
                $(btnMens[b]).unbind('click');
                $(btnMens[b]).click(function (e) {
                    let finan = $(ifin[b]).val()
                    let plazo = $(btnAgre[b]).val()
                    let precio = $(idPre[b]).val()
                    if (finan != "" && plazo != "" && precio != "") {
                        CargarMensualidadesFinanciero(finan, plazo, precio, 1, posicion);
                    }
                });
                // $(idPre[b]).unbind('keyup');
                // $(idPre[b]).keyup(function (event) {

                //     // skip for arrow keys
                //     if (event.which >= 37 && event.which <= 40) {
                //         event.preventDefault();
                //     }

                //     $(this).val(function (index, value) {
                //         return value
                //     });
                // });
                $(idPre[b]).on('paste', function (e) {
                    permitePegarSoloNumero2D($(this), e);
                });
                $(idPre[b]).on('keypress', function (event) {
                    aceptaSoloNumero2D($(this), event);
                });

            }

        }

        const dibujarInputs = function (lstDatosFinanciero) {
            $('#Banco').parent().css('width', '20%');

            let FinancieroMayor = 0;
            let lstidRow1 = [];
            let lstidRow2 = [];
            let lstidRow3 = [];

            if (lstDatosFinanciero.length == 0) {
                var table = $('#tblComparativoFinanciero').DataTable();
                table.columns([2, 3]).visible(false);
            }
            for (let i = 0; i < lstDatosFinanciero.length; i++) {
                if (FinancieroMayor < lstDatosFinanciero[i].idRow) {
                    FinancieroMayor = lstDatosFinanciero[i].idRow;
                    columnasFinanciero = FinancieroMayor;
                    if (columnasFinanciero == 3) {
                        $('#btnAgregarPlazos').css('visibility', 'hidden');
                    }
                    var table = $('#tblComparativoFinanciero').DataTable();
                    table.columns([FinancieroMayor]).visible(true);
                }
                if (lstDatosFinanciero[i].idRow == 1) {
                    lstidRow1.push(lstDatosFinanciero[i]);
                }
                if (lstDatosFinanciero[i].idRow == 2) {
                    lstidRow2.push(lstDatosFinanciero[i]);
                }
                if (lstDatosFinanciero[i].idRow == 3) {
                    lstidRow3.push(lstDatosFinanciero[i]);
                }
            }


            if (lstidRow1.length != 0) {

                for (let i = 0; i < lstidRow1.length; i++) {
                    $('#banco1').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                    $('#plazo1').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                    $('#precioDelEquipo1').append('<input type="text" style="margin: 3px;"/>')
                    $('#tiempoRestanteProyecto1').append('<input type="text" style="margin: 3px;"/>')
                    $('#iva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#total1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoFinanciar1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tipoOperacion1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#opcionCompra1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#valorResidual1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#depositoEfectivo1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#moneda1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#plazoMeses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaDeInteres1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#gastosFijos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#comision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoComision1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#rentasEnGarantia1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#crecimientoPagos1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoInicial1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotalIntereses1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaEfectiva1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#mensualidad1').append('<input type="text" style="margin: 3px; background: #f8f8f8;" />')
                    $('#mensualidad1').append('<button type="button" class="btn btn-xs btn-warning mensulialidad1" style="margin: 5px;"><i class="fas fa-minus"></i></button>')
                    $('#mensualidadSinIVA1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotal1').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')


                }
                if ($('#banco1').find('select').length == 3) {
                    $('#banco1').css('width', '700px');
                    $('#banco1').css('width', '700px');
                }
                if ($('#banco1').find('select').length > 3) {
                    $('#banco1').css('width', '800px');
                    $('#banco1').css('width', '800px');
                }
                let banco1 = $('#banco1').find('select');
                let plazo1 = $('#plazo1').find('select');
                let precioDelEquipo1 = $('#precioDelEquipo1').find('input');
                let tiempoRestanteProyecto1 = $('#tiempoRestanteProyecto1').find('input');
                let iva1 = $('#iva1').find('input');
                let total1 = $('#total1').find('input');
                let montoFinanciar1 = $('#montoFinanciar1').find('input');
                let tipoOperacion1 = $('#tipoOperacion1').find('input');
                let opcionCompra1 = $('#opcionCompra1').find('input');
                let valorResidual1 = $('#valorResidual1').find('input');
                let depositoEfectivo1 = $('#depositoEfectivo1').find('input');
                let moneda1 = $('#moneda1').find('input');
                let plazoMeses1 = $('#plazoMeses1').find('input');
                let tasaDeInteres1 = $('#tasaDeInteres1').find('input');
                let gastosFijos1 = $('#gastosFijos1').find('input');
                let comision1 = $('#comision1').find('input');
                let montoComision1 = $('#montoComision1').find('input');
                let rentasEnGarantia1 = $('#rentasEnGarantia1').find('input');
                let crecimientoPagos1 = $('#crecimientoPagos1').find('input');
                let pagoInicial1 = $('#pagoInicial1').find('input');
                let pagoTotalIntereses1 = $('#pagoTotalIntereses1').find('input');
                let tasaEfectiva1 = $('#tasaEfectiva1').find('input');
                let mensualidad1 = $('#mensualidad1').find('input');
                let mensualidadSinIVA1 = $('#mensualidadSinIVA1').find('input');
                let pagoTotal1 = $('#pagoTotal1').find('input');


                for (let i = 0; i < banco1.length; i++) {

                    $(banco1[i]).attr("data-id", lstidRow1[i].id);
                    $(banco1[i]).val(lstidRow1[i].financiera);
                    $(idMoneda1[i]).val(lstidRow1[i].moneda);
                    $(idTipoDeArrendamiento1[i]).val(lstidRow1[i].tipoDeArrendamiento);
                    $(idPlazo1[i]).val(lstidRow1[i].plazo);
                    $(idTasa1[i]).val(lstidRow1[i].tasa);
                    $(idComision1[i]).val(lstidRow1[i].comision);
                    $(idDeposito1[i]).val(lstidRow1[i].depositoEnGarantia);
                    $(idEnganche1[i]).val(lstidRow1[i].enganche);
                    $(idOpcionDeCompra1[i]).val(lstidRow1[i].opcionDeCompra);
                    $(idMonto1[i]).val(lstidRow1[i].monto);
                    $(idRentaFija1[i]).val(lstidRow1[i].rentaFija);
                    $(idIntereses1[i]).val(lstidRow1[i].interes);
                    $(idComisionDinero1[i]).val(lstidRow1[i].comisionMonto);
                    $(idOpcionDeCompraDinero1[i]).val(lstidRow1[i].opcionDeCompraMonto);
                    $(idEngancheDinero1[i]).val(lstidRow1[i].EngancheMonto);
                    $(idDepositoEnGarantia1[i]).val(lstidRow1[i].depositoEnGarantiaMonto);
                }
            }
            if (lstidRow2.length != 0) {
                for (let i = 0; i < lstidRow2.length; i++) {
                    $('#banco2').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                    $('#plazo2').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                    $('#precioDelEquipo2').append('<input type="text" style="margin: 3px;"/>')
                    $('#tiempoRestanteProyecto2').append('<input type="text" style="margin: 3px;"/>')
                    $('#iva2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#total2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoFinanciar2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tipoOperacion2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#opcionCompra2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#valorResidual2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#depositoEfectivo2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#moneda2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#plazoMeses2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaDeInteres2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#gastosFijos2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#comision2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoComision2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#rentasEnGarantia2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#crecimientoPagos2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoInicial2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotalIntereses2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaEfectiva2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#mensualidad2').append('<input type="text" style="margin: 3px; background: #f8f8f8" />')
                    $('#mensualidad2').append('<button type="button" id="mensulialidad2" class="btn btn-xs btn-warning" style="margin: 5px;"><i class="fas fa-minus"></i></button>')
                    $('#mensualidadSinIVA2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotal2').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')



                }
                if ($('#banco2').find('select').length == 3) {
                    $('#banco2').css('width', '700px');
                    $('#banco2').css('width', '700px');
                }
                if ($('#banco2').find('select').length > 3) {
                    $('#banco2').css('width', '800px');
                    $('#banco2').css('width', '800px');
                }
                let banco2 = $('#banco2').find('select');
                let plazo2 = $('#plazo2').find('select');
                let precioDelEquipo2 = $('#precioDelEquipo2').find('input');
                let tiempoRestanteProyecto2 = $('#tiempoRestanteProyecto2').find('input');
                let iva2 = $('#iva2').find('input');
                let total2 = $('#total2').find('input');
                let montoFinanciar2 = $('#montoFinanciar2').find('input');
                let tipoOperacion2 = $('#tipoOperacion2').find('input');
                let opcionCompra2 = $('#opcionCompra2').find('input');
                let valorResidual2 = $('#valorResidual2').find('input');
                let depositoEfectivo2 = $('#depositoEfectivo2').find('input');
                let moneda2 = $('#moneda2').find('input');
                let plazoMeses2 = $('#plazoMeses2').find('input');
                let tasaDeInteres2 = $('#tasaDeInteres2').find('input');
                let gastosFijos2 = $('#gastosFijos2').find('input');
                let comision2 = $('#comision2').find('input');
                let montoComision2 = $('#montoComision2').find('input');
                let rentasEnGarantia2 = $('#rentasEnGarantia2').find('input');
                let crecimientoPagos2 = $('#crecimientoPagos2').find('input');
                let pagoInicial2 = $('#pagoInicial2').find('input');
                let pagoTotalIntereses2 = $('#pagoTotalIntereses2').find('input');
                let tasaEfectiva2 = $('#tasaEfectiva2').find('input');
                let mensualidad2 = $('#mensualidad2').find('input');
                let mensualidadSinIVA2 = $('#mensualidadSinIVA2').find('input');
                let pagoTotal2 = $('#pagoTotal2').find('input');
                for (let i = 0; i < banco2.length; i++) {

                    $(banco2[i]).attr("data-id", lstidRow2[i].id);
                    $(banco2[i]).val(lstidRow2[i].financiera);
                    $(idMoneda2[i]).val(lstidRow2[i].moneda);
                    $(idTipoDeArrendamiento2[i]).val(lstidRow2[i].tipoDeArrendamiento);
                    $(idPlazo2[i]).val(lstidRow2[i].plazo);
                    $(idTasa2[i]).val(lstidRow2[i].tasa);
                    $(idComision2[i]).val(lstidRow2[i].comision);
                    $(idDeposito2[i]).val(lstidRow2[i].depositoEnGarantia);
                    $(idEnganche2[i]).val(lstidRow2[i].enganche);
                    $(idOpcionDeCompra2[i]).val(lstidRow2[i].opcionDeCompra);
                    $(idMonto2[i]).val(lstidRow2[i].monto);
                    $(idRentaFija2[i]).val(lstidRow2[i].rentaFija);
                    $(idIntereses2[i]).val(lstidRow2[i].interes);
                    $(idComisionDinero2[i]).val(lstidRow2[i].comisionMonto);
                    $(idOpcionDeCompraDinero2[i]).val(lstidRow2[i].opcionDeCompraMonto);
                    $(idEngancheDinero2[i]).val(lstidRow2[i].EngancheMonto);
                    $(idDepositoEnGarantia2[i]).val(lstidRow2[i].depositoEnGarantiaMonto);
                }
            }
            if (lstidRow3.length != 0) {
                for (let i = 0; i < lstidRow3.length; i++) {
                    $('#banco3').append('<select class="comboFinanciera" style="margin: 3px; width: 203px;"></select>');
                    $('#plazo3').append('<select style="margin: 3px;" name="select" class="select comboPlazo"><option value="6" selected>6 Meses</option><option value="12">12 Meses</option><option value="24">24 Meses</option><option value="36">36 Meses</option><option value="48">48 Meses</option></select>');
                    $('#precioDelEquipo3').append('<input type="text" style="margin: 3px;"/>')
                    $('#tiempoRestanteProyecto3').append('<input type="text" style="margin: 3px;"/>')
                    $('#iva3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#total3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoFinanciar3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tipoOperacion3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#opcionCompra3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#valorResidual3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#depositoEfectivo3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#moneda3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#plazoMeses3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaDeInteres3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#gastosFijos3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#comision3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#montoComision3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#rentasEnGarantia3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#crecimientoPagos3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoInicial3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotalIntereses3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#tasaEfectiva3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#mensualidad3').append('<input type="text" style="margin: 3px; background: #f8f8f8" />')
                    $('#mensualidad3').append('<button type="button" id="mensulialidad3" class="btn btn-xs btn-warning" style="margin: 5px;"><i class="fas fa-minus"></i></button>')
                    $('#mensualidadSinIVA3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')
                    $('#pagoTotal3').append('<input type="text" style="margin: 3px; background: #f8f8f8" readonly="true" />')


                }
                if ($('#banco3').find('select').length == 3) {
                    $('#banco3').css('width', '700px');
                    $('#banco3').css('width', '700px');
                }
                if ($('#banco3').find('select').length > 3) {
                    $('#banco3').css('width', '800px');
                    $('#banco3').css('width', '800px');
                }
                let banco3 = $('#banco3').find('select');
                let plazo3 = $('#plazo3').find('select');
                let precioDelEquipo3 = $('#precioDelEquipo3').find('input');
                let tiempoRestanteProyecto3 = $('#tiempoRestanteProyecto3').find('input');
                let iva3 = $('#iva3').find('input');
                let total3 = $('#total3').find('input');
                let montoFinanciar3 = $('#montoFinanciar3').find('input');
                let tipoOperacion3 = $('#tipoOperacion3').find('input');
                let opcionCompra3 = $('#opcionCompra3').find('input');
                let valorResidual3 = $('#valorResidual3').find('input');
                let depositoEfectivo3 = $('#depositoEfectivo3').find('input');
                let moneda3 = $('#moneda3').find('input');
                let plazoMeses3 = $('#plazoMeses3').find('input');
                let tasaDeInteres3 = $('#tasaDeInteres3').find('input');
                let gastosFijos3 = $('#gastosFijos3').find('input');
                let comision3 = $('#comision3').find('input');
                let montoComision3 = $('#montoComision3').find('input');
                let rentasEnGarantia3 = $('#rentasEnGarantia3').find('input');
                let crecimientoPagos3 = $('#crecimientoPagos3').find('input');
                let pagoInicial3 = $('#pagoInicial3').find('input');
                let pagoTotalIntereses3 = $('#pagoTotalIntereses3').find('input');
                let tasaEfectiva3 = $('#tasaEfectiva3').find('input');
                let mensualidad3 = $('#mensualidad3').find('input');
                let mensualidadSinIVA3 = $('#mensualidadSinIVA3').find('input');
                let pagoTotal3 = $('#pagoTotal3').find('input');
                for (let i = 0; i < banco3.length; i++) {

                    $(banco3[i]).attr("data-id", lstidRow3[i].id);
                    $(banco3[i]).val(lstidRow3[i].financiera);
                    $(idMoneda3[i]).val(lstidRow3[i].moneda);
                    $(idTipoDeArrendamiento3[i]).val(lstidRow3[i].tipoDeArrendamiento);
                    $(idPlazo3[i]).val(lstidRow3[i].plazo);
                    $(idTasa3[i]).val(lstidRow3[i].tasa);
                    $(idComision3[i]).val(lstidRow3[i].comision);
                    $(idDeposito3[i]).val(lstidRow3[i].depositoEnGarantia);
                    $(idEnganche3[i]).val(lstidRow3[i].enganche);
                    $(idOpcionDeCompra3[i]).val(lstidRow3[i].opcionDeCompra);
                    $(idMonto3[i]).val(lstidRow3[i].monto);
                    $(idRentaFija3[i]).val(lstidRow3[i].rentaFija);
                    $(idIntereses3[i]).val(lstidRow3[i].interes);
                    $(idComisionDinero3[i]).val(lstidRow3[i].comisionMonto);
                    $(idOpcionDeCompraDinero3[i]).val(lstidRow3[i].opcionDeCompraMonto);
                    $(idEngancheDinero3[i]).val(lstidRow3[i].EngancheMonto);
                    $(idDepositoEnGarantia3[i]).val(lstidRow3[i].depositoEnGarantiaMonto);
                }
            }
            let tr = $('#tblComparativoFinanciero').find('tr')
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                $(td[0]).css('font-weight', 'bold')
            }
            if (estatusFinanciera == 2) {
                $('#tblComparativoFinanciero').find('input').prop('disabled', true);
            } else {
                $('#tblComparativoFinanciero').find('input').prop('disabled', false);
            }
            $('#tblComparativoFinanciero').find('td').css('padding', 0);
            $('.comboFinanciera').fillCombo('/CatMaquina/FillCboFinancieros');


            let ifin = $('#banco3').find('select')
            for (let b = 0; b < ifin.length; b++) {
                $(ifin[b]).change(function (e) {
                    console.log(3)
                });

            }
            $('#btnEliminarPlazos').parent().parent().css('background-color', '#3556ae !important;');
            $('#btnAgregarInput3').parent().css('background-color', '#3556ae !important;');
            $('#btnEliminarPlazos').parent().css('background-color', '#3556ae !important;');
            $('#btnAgregarInput2').parent().css('background-color', '#3556ae !important;');



            // $('.comboFinanciera').change(function(e){
            //     let financiero = $(this).val();
            //     let plazo = $(this).parent().parent().parent().next().find('.comboPlazo').val();
            //     let precio = $(this).parent().parent().parent().next().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().parent().children().last().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {
            //         CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());
            //     }
            //     });
            // $('.comboPlazo').change(function(e){
            //     let financiero = $(this).parent().parent().parent().prev().find('.comboFinanciera').val();
            //     let plazo = $(this).val();
            //     let precio = $(this).parent().parent().parent().next().find('input').val();
            //     let meses = $(this).parent().parent().parent().parent().children().last().find('input').val();
            //     if (financiero != "" && plazo !="" && precio !="" && meses!="") {
            //         CargarDatosFinanciero(financiero, plazo, precio, meses, $(this).parent().parent().parent().parent());            
            //     }
            //     });
        }

        const ValidarCamposVaciosFinanciero = function () {
            let lstinput = $('#tblComparativoFinanciero').find('input');
            let vacio = false;
            for (let index = 0; index < lstinput.length; index++) {
                if ($(lstinput[index]).val() == "") {
                    vacio = true;
                    break;
                }
            }
            return vacio;
        }
        const ValidarCamposVacios = function () {
            let vacio = false;
            if ($('#idMarcaNum1Marca').val() != "") {
                $('#idMarcaNum1Marca').val() == "" ? vacio = true : vacio = false;;
                $('#idMarcaNum1proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum1Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum2Marca').val() != "") {
                $('#idMarcaNum2Marca').val() == "" ? vacio = true : vacio = false;;
                $('#idMarcaNum2proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum2Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum3Marca').val() != "") {
                $('#idMarcaNum3Marca').val() == "" ? vacio = true : vacio = false;;
                $('#idMarcaNum3proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum3Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum4Marca').val() != "") {
                $('#idMarcaNum4Marca').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum4Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum5Marca').val() != "") {
                $('#idMarcaNum5Marca').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum5Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum6Marca').val() != "") {
                $('#idMarcaNum6Marca').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum6Condiciones').val() == "" ? vacio = true : vacio = false;
            }
            if ($('#idMarcaNum7Marca').val() != "") {
                $('#idMarcaNum7Marca').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7proveedor').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Trade').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Valores').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Precio').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7PrecioRoc').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7BaseHoras').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Tiempo').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Ubicacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Horas').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Seguro').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Garantia').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Servicios').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Capacitacion').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Deposito').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Lugar').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Flete').val() == "" ? vacio = true : vacio = false;
                $('#idMarcaNum7Condiciones').val() == "" ? vacio = true : vacio = false;
            }

            return vacio;
        }

        const initTblMensualidadesFinanciero = function () {
            dtMensualidadesFinanciero = tblMensualidadesFinanciero.DataTable({
                destroy: true,
                ordering: false,
                language: dtDicEsp,
                columns: [
                    { title: 'Periodo', data: 'periodo' },
                    { title: 'Capital', data: 'capital', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { title: 'Intereses', data: 'intereses', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { title: 'IVA Capital ', data: 'ivaCapital', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { title: 'IVA Intereses', data: 'ivaIntereses', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { title: 'Pago Total', data: 'pagoTotal', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                    { title: '', data: 'pagoFinal', render: (data, type, row) => { return '$' + parseFloat(data, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString(); } },
                ],
                drawCallback: function (settings) {
                    //
                }
            });
        }

        function CargarMensualidadesFinanciero(financiera, plazo, precio, meses, row, posicion) {
            axios.post('/CatMaquina/ObtenerMensualidades', { financieraID: financiera, plazoMeses: plazo, precio: precio })
                .then(response => {
                    let { success, mensualidades, importeEfectivo, tasaEfectiva } = response.data;
                    if (success) {
                        dtMensualidadesFinanciero.clear().draw();
                        dtMensualidadesFinanciero.rows.add(mensualidades).draw();
                        $("#txtiImporteEfectivoFinanciero").val("$ " + parseFloat(importeEfectivo, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $("#txtTasaEfectivaFinanciero").val(parseFloat(tasaEfectiva, 16).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString() + " %");
                        modalMensualidadesFinanciero.modal();
                    }
                });
        }


        const ImprimirReporte = function (Financiera) {
            if (Financiera == false) {
                var doc = new html2pdf();
                // var doc = new jsPDF('l','mm','A4');
                var divContenido = $('#contenidoPRINTadquisicion').get(0)
                var opt = {
                    margin: [0.5, 0.5, 0.42, 0.5],
                    filename: 'CuadroComparativoAdquisicion.pdf',
                    image: { type: 'jpeg', quality: 1 },
                    html2canvas: { scale: 1 },
                    jsPDF: { unit: 'in', format: 'letter', orientation: 'P' }
                };

                // New Promise-based usage:
                doc.set(opt).from(divContenido).save().then(r => {
                    console.log(r);
                    $('#contenidoPRINTadquisicion').find('table').remove();
                    $('#contenidoheader').remove();
                    $('#contenidoFecha').remove();
                    $('#contenidoAdquisicionRenta').remove();
                    $('#rptprintTipoMoneda').remove()

                });
            } else {
                var doc = new html2pdf();
                // var doc = new jsPDF('l','mm','A4');
                var divContenido = $('#contenidoImpresion').get(0)
                var opt = {
                    margin: [0.5, 0.5, 0.42, 0.5],
                    filename: 'CuadroComparativoFinanciero.pdf',
                    image: { type: 'jpeg', quality: 1 },
                    html2canvas: { scale: 1 },
                    jsPDF: { unit: 'in', format: 'letter', orientation: 'P' }
                };

                // New Promise-based usage:
                doc.set(opt).from(divContenido).save().then(r => {
                    console.log(r);
                    $('#contenidoPRINTFinanciero').find('table').remove();
                    $('#divOpinionGeneral').remove();

                });
            }
        }
        var fncCargarTablaIncidentesPRINT = function (renta) {
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAutorizar",
                data: objFiltro,
                success: function (response) {
                    if (response.success) {

                        getTablaComparativoAdquisicionDetallePRINT();
                        establecerAdquisicionPRINT(response.items, renta);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        var getTablaComparativoAdquisicionDetallePRINT = function () {
            let objFiltro = fncGetFiltros();
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoAdquisicionDetalle",
                data: objFiltro,
                success: function (response) {
                    if (response.success) {
                        formatearListaPRINT(response.items);


                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        let establecerAdquisicionPRINT = function (lstAdquisicion, renta) {
            console.log(lstAdquisicion);
            let html = '';
            // check-square


            html += "<div class='row' id='contenidoheader'>";
            html += "<div class='col-lg-3' >";
            html += "</div>";

            html += "<div class='col-lg-2' >";
            html += "compra <input id='checkComprarpt' type='checkbox'/>";
            html += "</div>";

            html += "<div class='col-lg-2' >";
            html += "Renta <input id='checkRentarpt' type='checkbox'/>";
            html += "</div>";

            html += "<div class='col-lg-2' >";
            html += "Roc <input id='checkRocrpt' type='checkbox'/>";
            html += "</div>";

            html += "<div class='col-lg-3' >";
            html += "</div><br>";
            html += "</div>";
            html += "<div class='row' id='contenidoFecha'>";
            html += "<div class='col-lg-8' >";
            html += "</div>";
            html += "<div class='col-lg-4' >";
            html += "<p id='fechacomparativo'></p>";
            html += "</div>";
            html += "</div>";

            html += '<table style="font-size:10px;" >'
            lstAdquisicion.forEach(x => {
                if (x.header != "check") {
                    if (x.header == 'Caracteristica1' || x.header == 'Caracteristica2' || x.header == 'Caracteristica3' || x.header == 'Caracteristica4' || x.header == 'Caracteristica5' || x.header == 'Caracteristica6' || x.header == 'Caracteristica7') {
                        console.log('HOLA SOY ' + x.header, x)
                        // html += '<tr> <th><div id=' + x.header + '></div></th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';
                    } else {
                        console.log('HOLA SOY ' + x.header, x)
                        html += '<tr> <th>' + x.header + '</th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';
                    }
                } else {

                    html += '<tr> <th><div id=' + x.header + '></div></th> <td><div id="' + x.txtIdnumero1 + '"></div></td><td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td> </tr>';

                }

            });
            html += '</table>'
            html += '</table>'
            html += '  <div class="col-lg-5">'
            html += '<label class="text-color" for="cboCC">Tipo Moneda : </label>'
            html += '<input id="rptprintTipoMoneda" class="form-group" />'
            html += '</div>'
            contenidoPRINTadquisicion.append(html);
        }
        const formatearListaPRINT = function (lstDatos) {
            console.log(lstDatos);
            let contador = 0;
            $('#fechacomparativo').text('Fecha elaboracion: ' + moment(lstDatos[0].fechaDeElaboracion).format('YYYY/MM/DD'));
            $('#txtObrarpt').text('Obra: ' + lstDatos[0].obra);
            $('#txtNombreDelEquiporpt').text('Nombre del Equipo: ' + lstDatos[0].nombreDelEquipo);
            $('#checkComprarpt').prop('checked', lstDatos[0].compra);
            $('#checkRentarpt').prop('checked', lstDatos[0].renta);
            $('#checkRocrpt').prop('checked', lstDatos[0].roc);
            $('#rptprintTipoMoneda').val(lstDatos[0].tipoMoneda);

            lstDatos.forEach(x => {
                contador++;
                $('#Caracteristica' + contador).text(x.caracteristicasDelEquipo);
                $('#idMarcaNum' + contador + 'Marca').text(x.marcaModelo);
                $('#idMarcaNum' + contador + 'proveedor').text(x.proveedor);
                $('#idMarcaNum' + contador + 'precio').text(x.precioDeVenta);
                $('#idMarcaNum' + contador + 'Trade').text(x.tradeIn);
                $('#idMarcaNum' + contador + 'Valores').text(x.valoresDeRecompra);
                $('#idMarcaNum' + contador + 'Precio').text(x.precioDeRentaPura);
                $('#idMarcaNum' + contador + 'PrecioRoc').text(x.precioDeRentaEnRoc);
                $('#idMarcaNum' + contador + 'BaseHoras').text(x.baseHoras);
                $('#idMarcaNum' + contador + 'Tiempo').text(x.tiempoDeEntrega);
                $('#idMarcaNum' + contador + 'Ubicacion').text(x.ubicacion);
                $('#idMarcaNum' + contador + 'Horas').text(x.horas);
                $('#idMarcaNum' + contador + 'Seguro').text(x.seguro);
                $('#idMarcaNum' + contador + 'Garantia').text(x.garantia);
                $('#idMarcaNum' + contador + 'Servicios').text(x.serviciosPreventivos);
                $('#idMarcaNum' + contador + 'Capacitacion').text(x.capacitacion);
                $('#idMarcaNum' + contador + 'Deposito').text(x.depositoEnGarantia);
                $('#idMarcaNum' + contador + 'Lugar').text(x.lugarDeEntrega);
                $('#idMarcaNum' + contador + 'Flete').text(x.flete);
                $('#idMarcaNum' + contador + 'Condiciones').text(x.condicionesDePagoEntrega);

                x.lstCaracteristicas.forEach(y => {
                    if (x.idRow == 1) {
                        if (y.idRow == 1) { $('#idMarcaNum1Caracteristicas11').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum1Caracteristicas12').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum1Caracteristicas13').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum1Caracteristicas14').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum1Caracteristicas15').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum1Caracteristicas16').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum1Caracteristicas17').text(y.Descripcion); }

                    }
                    if (x.idRow == 2) {
                        if (y.idRow == 1) { $('#idMarcaNum2Caracteristicas21').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum2Caracteristicas22').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum2Caracteristicas23').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum2Caracteristicas24').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum2Caracteristicas25').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum2Caracteristicas26').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum2Caracteristicas27').text(y.Descripcion); }

                    }
                    if (x.idRow == 3) {
                        if (y.idRow == 1) { $('#idMarcaNum3Caracteristicas31').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum3Caracteristicas32').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum3Caracteristicas33').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum3Caracteristicas34').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum3Caracteristicas35').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum3Caracteristicas36').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum3Caracteristicas37').text(y.Descripcion); }
                    }
                    if (x.idRow == 4) {
                        if (y.idRow == 1) { $('#idMarcaNum4Caracteristicas41').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum4Caracteristicas42').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum4Caracteristicas43').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum4Caracteristicas44').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum4Caracteristicas45').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum4Caracteristicas46').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum4Caracteristicas47').text(y.Descripcion); }
                    }
                    if (x.idRow == 5) {
                        if (y.idRow == 1) { $('#idMarcaNum5Caracteristicas51').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum5Caracteristicas52').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum5Caracteristicas53').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum5Caracteristicas54').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum5Caracteristicas55').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum5Caracteristicas56').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum5Caracteristicas57').text(y.Descripcion); }
                    }
                    if (x.idRow == 6) {
                        if (y.idRow == 1) { $('#idMarcaNum6Caracteristicas61').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum6Caracteristicas62').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum6Caracteristicas63').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum6Caracteristicas64').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum6Caracteristicas65').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum6Caracteristicas66').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum6Caracteristicas67').text(y.Descripcion); }
                    }
                    if (x.idRow == 7) {
                        if (y.idRow == 1) { $('#idMarcaNum7Caracteristicas71').text(y.Descripcion); }
                        if (y.idRow == 2) { $('#idMarcaNum7Caracteristicas72').text(y.Descripcion); }
                        if (y.idRow == 3) { $('#idMarcaNum7Caracteristicas73').text(y.Descripcion); }
                        if (y.idRow == 4) { $('#idMarcaNum7Caracteristicas74').text(y.Descripcion); }
                        if (y.idRow == 5) { $('#idMarcaNum7Caracteristicas75').text(y.Descripcion); }
                        if (y.idRow == 6) { $('#idMarcaNum7Caracteristicas76').text(y.Descripcion); }
                        if (y.idRow == 7) { $('#idMarcaNum7Caracteristicas77').text(y.Descripcion); }
                    }
                });

            });
            $('#contenidoPRINTadquisicion').find('th').css('border', '0.2px solid');
            $('#contenidoPRINTadquisicion').find('td').css('border', '0.2px solid');
            $('#contenidoPRINTadquisicion').find('td').css('text-align', 'center');
            $('#contenidoPRINTadquisicion').find('th').css('text-align', 'center');
            $('#contenidoPRINTadquisicion').find('th').css('width', '160px');
            if (lstDatos.length <= 7) {
                let px = 110;
                if (lstDatos.length < 4) {
                    px = 250;
                } else if (lstDatos.length == 2) {
                    px = 350;
                }
                $('#contenidoPRINTadquisicion').find('td').css('width', '' + px + 'px');
            }


            let tr = $('#contenidoPRINTadquisicion').find('table').find('tr');
            let cantidadRow = lstDatos.length;
            let cantidadColum = lstDatos[0].lstCaracteristicas.length + 23;
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                $(td[0]).css('font-weight', 'bold')
                for (let n = 0; n < td.length; n++) {
                    if (n >= cantidadRow) {
                        $(td[n]).css('display', 'none');
                    }
                }
                if (index >= cantidadColum) {
                    $(tr[index]).css('display', 'none');
                }
            }



            ImprimirReporte(false);
        }

        const getTablaComparativoFinancieroPRINT = function () {
            $.ajax({
                datatype: "json",
                type: "POST",
                // url: "/CatMaquina/getTablaComparativoFinanciero",
                url: "/CatMaquina/getTablaComparativoFinancieroAutorizar",
                success: function (response) {
                    if (response.success) {
                        DibujarTabla(response.items);
                        getTablaComparativoFinancieroDetallePRINT();
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }
        const getTablaComparativoFinancieroDetallePRINT = function () {
            $.ajax({
                datatype: "json",
                type: "POST",
                url: "/CatMaquina/getTablaComparativoFinancieroDetalle?idFinanciero=" + idAsignacionp + "",
                success: function (response) {
                    if (response.success) {
                        initGpxPagoTotalBanco('gpxPagoTotalBancoImpresion', response.gpxLinea, null, null);
                        initGpxInteresVsEfectiva('gpxInteresVsEfectivaImpresion', response.gpxBarra, dibujarInputsPRINT, response.items);
                        //dibujarInputsPRINT(response.items);
                    } else {
                        Alert2Error(response.message);
                    }
                },
                error: function () {
                    AlertaGeneral("Error", "Ocurrió un error al lanzar la petición.");
                },
                complete: function () {
                    $.unblockUI();
                }
            });
        }

        const DibujarTabla = function (lstFinanciero) {
            console.log(lstFinanciero);
            let html = '<br />';

            html += '<table class="tblImpresionCC" style="font-size:10px; margin-left:auto; margin-right:auto;" >'
            lstFinanciero.forEach(x => {
                if (x.header != "botones" && x.header != "Selector") {
                    html += '<tr>';
                    html += '<th style="border: 1px solid black; ' + (x.classColor != null ? ('background-color:' + x.classColor) : ("")) + '">' + x.header + '</th>';
                    html += '<td style="border: 1px solid black;"><div id="' + x.txtIdnumero1 + '"></div></td>';
                    html += '<td><div id="' + x.txtIdnumero2 + '"></div></td><td><div id="' + x.txtIdnumero3 + '"></div></td><td><div id="' + x.txtIdnumero4 + '"></div></td><td><div id="' + x.txtIdnumero5 + '"></div></td><td><div id="' + x.txtIdnumero6 + '"></div></td><td><div id="' + x.txtIdnumero7 + '"></div></td><td><div id="' + x.txtIdnumero8 + '"></div></td><td><div id="' + x.txtIdnumero9 + '"></div></td><td><div id="' + x.txtIdnumero10 + '"></div></td><td><div id="' + x.txtIdnumero11 + '"></div></td><td><div id="' + x.txtIdnumero12 + '"></div></td> </tr>';
                }
            });
            html += '</table>';
            // html += "<div class='row' id='divOpinionGeneral'>";
            // html += "<h3> COMENTARIO GENERAL:</h3>";
            // html += "<textarea id='opinionGeneral2' name='opinionGeneral' rows='3' cols='70' style='border: 1px solid; margin-left: auto; margin-right:auto;'>";
            // html += "</textarea>";
            // html += "</div>";
            contenidoPRINTFinanciero.append(html);
        }
        const dibujarInputsPRINT = function (lstDatos) {
            console.log(lstDatos)
            let cont = 0;
            let tnt = '';
            $('#imprimirFecha').text('Fecha elaboracion: ' + moment(lstDatos[0].fechaDeElaboracionFinanciero).format('YYYY/MM/DD'));
            lstDatos.forEach(x => {
                cont++;
                tnt = x.ComentarioGeneral;
                $('#banco' + cont).text(x.banco);
                $('#banco' + cont).closest('td').addClass('columnaMahogany');
                $('#plazo' + cont).text(x.plazo);
                $('#precioDelEquipo' + cont).text(x.precioDelEquipo);
                $('#precioDelEquipo' + cont).closest('td').addClass('columnaMahogany');
                $('#tiempoRestanteProyecto' + cont).text(x.tiempoRestanteProyecto);
                $('#iva' + cont).text(x.iva);
                $('#total' + cont).text(x.total);
                $('#montoFinanciar' + cont).text(x.montoFinanciar);
                $('#tipoOperacion' + cont).text(x.tipoOperacion);
                $('#tipoOperacion' + cont).closest('td').addClass('columnaGris');
                $('#opcionCompra' + cont).text(x.opcionCompra);
                $('#opcionCompra' + cont).closest('td').addClass('columnaGris');
                $('#valorResidual' + cont).text(x.valorResidual);
                $('#valorResidual' + cont).closest('td').addClass('columnaGris');
                $('#depositoEfectivo' + cont).text(x.depositoEfectivo);
                $('#depositoEfectivo' + cont).closest('td').addClass('columnaGris');
                $('#moneda' + cont).text(x.moneda);
                $('#moneda' + cont).closest('td').addClass('columnaGris');
                $('#plazoMeses' + cont).text(x.plazoMeses);
                $('#plazoMeses' + cont).closest('td').addClass('columnaGris');
                $('#tasaDeInteres' + cont).text(x.tasaDeInteres);
                $('#tasaDeInteres' + cont).closest('td').addClass('columnaGris');
                $('#gastosFijos' + cont).text(x.gastosFijos);
                $('#gastosFijos' + cont).closest('td').addClass('columnaGris');
                $('#comision' + cont).text(x.comision);
                $('#comision' + cont).closest('td').addClass('columnaGris');
                $('#montoComision' + cont).text(x.montoComision);
                $('#rentasEnGarantia' + cont).text(x.rentasEnGarantia);
                $('#rentasEnGarantia' + cont).closest('td').addClass('columnaGris');
                $('#crecimientoPagos' + cont).text(x.crecimientoPagos);
                $('#crecimientoPagos' + cont).closest('td').addClass('columnaGris');
                $('#pagoInicial' + cont).text(x.pagoInicial);
                $('#pagoTotalIntereses' + cont).text(x.pagoTotalIntereses);
                $('#tasaEfectiva' + cont).text(x.tasaEfectiva);
                $('#mensualidad' + cont).text(x.mensualidad);
                $('#mensualidadSinIVA' + cont).text(x.mensualidadSinIVA);
                $('#pagoTotal' + cont).text(x.pagoTotal);
            });
            $('#opinionGeneral2').val(tnt);
            contenidoPRINTFinanciero.find('td').css('text-align', 'center');
            contenidoPRINTFinanciero.find('th').css('text-align', 'center');
            contenidoPRINTFinanciero.find('th').css('width', '140px');
            contenidoPRINTFinanciero.find('td').css('width', '110px');
            let tr = contenidoPRINTFinanciero.find('table').find('tr');
            let cantidadRow = lstDatos.length;
            for (let index = 0; index < tr.length; index++) {
                let td = $(tr[index]).find('td')
                $(td[0]).css('font-weight', 'bold')
                for (let n = 0; n < td.length; n++) {
                    if (n >= cantidadRow) {
                        $(td[n]).css('display', 'none');
                    }
                }
            }

            ImprimirReporte(true);
        }

        function CargarDatosFinanciero(financiera, plazo, precio, meses, row, posicion) {
            axios.post('/CatMaquina/LlenarDatosFinanciero', { financieraID: financiera, plazoMeses: plazo, precio: precio, mesesRestantes: meses, posicion })
                .then(response => {
                    let { success, datos, gpxBarra, gpxLinea } = response.data;
                    if (success) {


                        let iva = $('#iva' + row).find('input')
                        let total = $('#total' + row).find('input')
                        let montoFinanciar = $('#montoFinanciar' + row).find('input')
                        let tipoOperacion = $('#tipoOperacion' + row).find('input')
                        let opcionCompra = $('#opcionCompra' + row).find('input')
                        let valorResidual = $('#valorResidual' + row).find('input')
                        let depositoEfectivo = $('#depositoEfectivo' + row).find('input')
                        let moneda = $('#moneda' + row).find('input')
                        let plazoMeses = $('#plazoMeses' + row).find('input')
                        let tasaDeInteres = $('#tasaDeInteres' + row).find('input')
                        let gastosFijos = $('#gastosFijos' + row).find('input')
                        let comision = $('#comision' + row).find('input')
                        let montoComision = $('#montoComision' + row).find('input')
                        let rentasEnGarantia = $('#rentasEnGarantia' + row).find('input')
                        let crecimientoPagos = $('#crecimientoPagos' + row).find('input')
                        let pagoInicial = $('#pagoInicial' + row).find('input')
                        let pagoTotalIntereses = $('#pagoTotalIntereses' + row).find('input')
                        let tasaEfectiva = $('#tasaEfectiva' + row).find('input')
                        let mensualidad = $('#mensualidad' + row).find('input')
                        let mensualidadSinIVA = $('#mensualidadSinIVA' + row).find('input')
                        let pagoTotal = $('#pagoTotal' + row).find('input')


                        $(iva[posicion]).val('$' + parseFloat(datos.iva, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(total[posicion]).val('$' + parseFloat(datos.total).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(montoFinanciar[posicion]).val('$' + parseFloat(datos.monto).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(tipoOperacion[posicion]).val('$' + parseFloat(datos.tipoDeArrendamiento).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(tipoOperacion[posicion]).val(datos.tipoDeArrendamiento == 1 ? 'ARRENDAMIENTO' : 'CREDITO SIMPLE');
                        //$(opcionCompra[posicion]).val('$' + parseFloat(datos.opcionDeCompra).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(opcionCompra[posicion]).val(maskNumeroNM(datos.opcionDeCompra) + '%');
                        //$(valorResidual[posicion]).val('$' + parseFloat(datos.residual).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(valorResidual[posicion]).val(maskNumeroNM(datos.residual) + '%');
                        $(depositoEfectivo[posicion]).val('$' + parseFloat(datos.deposito).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(moneda[posicion]).val('$' + parseFloat(datos.moneda).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(moneda[posicion]).val(datos.moneda == 1 ? 'PESOS' : 'DOLARES');
                        //$(plazoMeses[posicion]).val('$' + parseFloat(datos.plazoFinal).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(plazoMeses[posicion]).val(datos.plazoFinal);
                        //$(tasaDeInteres[posicion]).val('$' + parseFloat(datos.tasa).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(tasaDeInteres[posicion]).val(maskNumeroNM(datos.tasa) + '%');
                        $(gastosFijos[posicion]).val('$' + parseFloat(datos.gastos).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(comision[posicion]).val('$' + parseFloat(datos.comision).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(comision[posicion]).val(maskNumeroNM(datos.comision) + '%');
                        //$(montoComision[posicion]).val('$' + parseFloat(datos.comisionDinero).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(montoComision[posicion]).val('$' + maskNumeroNM(datos.comisionDinero));
                        //$(rentasEnGarantia[posicion]).val('$' + parseFloat(datos.rentaFija).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(rentasEnGarantia[posicion]).val(datos.rentaFija);
                        //$(crecimientoPagos[posicion]).val('$' + parseFloat(datos.crecimiento).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(crecimientoPagos[posicion]).val(maskNumeroNM(datos.crecimiento) + '%');
                        $(pagoInicial[posicion]).val('$' + parseFloat(datos.enganche).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(pagoTotalIntereses[posicion]).val('$' + parseFloat(datos.intereses).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        //$(tasaEfectiva[posicion]).val('$' + parseFloat(datos.tasaEfectiva).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(tasaEfectiva[posicion]).val(maskNumeroNM(datos.tasaEfectiva * 100) + '%');
                        $(mensualidad[posicion]).val('$' + parseFloat(datos.mensualidad).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(mensualidadSinIVA[posicion]).val('$' + parseFloat(datos.mensualidadNoIVA).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $(pagoTotal[posicion]).val('$' + parseFloat(datos.totalAPagar).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());

                        initGpxInteresVsEfectiva('gpxInteresVsEfectiva', gpxBarra);
                        initGpxPagoTotalBanco('gpxPagoTotalBanco', gpxLinea);
                    }
                });
        }

        function CargarMensualidadesFinanciero(financiera, plazo, precio, row, posicion) {
            console.log(financiera)
            console.log(plazo)
            console.log(precio)
            // financieraID, int plazoMeses, decimal precio, int mesesRestantes
            axios.post('/CatMaquina/ObtenerMensualidades', { financieraID: financiera, plazoMeses: plazo, precio: precio })
                .then(response => {
                    let { success, datos } = response.data;
                    if (success) {
                        modalMensualidadesFinanciero.modal()

                    }
                });
        }
        function initGpxInteresVsEfectiva(grafica, datos, callback, items) {
            if (datos != null && datos != undefined) {
                gpxPagoTotalBanco.show();
                gpxInteresVsEfectiva.show();
            }
            else {
                gpxPagoTotalBanco.hide();
                gpxInteresVsEfectiva.hide();
            }
            Highcharts.chart(grafica, {
                chart: {
                    type: 'column',
                    width: 600,
                    height: 230,
                    events: {
                        load: function () {
                            setTimeout(function () {
                                if (callback) {
                                    if (items == null || items == undefined) {
                                        callback();
                                    }
                                    else {
                                        callback(items);
                                    }
                                }
                            }, 2500);
                        },
                        animation: false
                    }
                },
                title: {
                    text: 'Pago Total de Intereses / Tasa Efectiva'
                },
                xAxis: {
                    categories: datos == null ? '' : datos.categories
                },
                yAxis: [{
                    min: 0,
                    title: {
                        text: ''
                    }
                }, {
                    title: {
                        text: ''
                    },
                    opposite: true,
                    max: 25,
                    min: 0,
                    endOnTick: false,
                    gridLineWidth: 0
                }],
                legend: {
                    shadow: false
                },
                tooltip: {
                    shared: true
                },
                plotOptions: {
                    column: {
                        grouping: false,
                        shadow: false,
                        borderWidth: 0
                    }
                },
                series: datos == null ? '' : datos.series,
                credits: {
                    enabled: false
                }
            });
        }

        function initGpxPagoTotalBanco(grafica, datos, callback, items) {
            Highcharts.chart(grafica, {
                chart: {
                    width: 600,
                    height: 230,
                    events: {
                        load: function () {
                            setTimeout(function () {
                                if (callback) {
                                    if (items == null || items == undefined) {
                                        callback();
                                    }
                                    else {
                                        callback(items);
                                    }
                                }
                            }, 2500);
                        }
                    },
                    animation: false
                },
                title: {
                    text: 'Pago total por Banco'
                },

                subtitle: {
                    text: ''
                },

                yAxis: {
                    title: {
                        text: ''
                    }
                },

                xAxis: {
                    // accessibility: {
                    //     rangeDescription: 'Range: 2010 to 2017'
                    // }
                },

                legend: {
                    // layout: 'vertical',
                    // align: 'right',
                    // verticalAlign: 'middle'
                },

                plotOptions: {
                    series: {
                        label: {
                            connectorAllowed: false
                        },
                        pointStart: 1,
                        marker: {
                            enabled: false
                        }
                    }
                },

                series: datos == null ? '' : datos,
                credits: {
                    enabled: false
                }

                // responsive: {
                //     rules: [{
                //         condition: {
                //             maxWidth: 600
                //         },
                //         chartOptions: {
                //             legend: {
                //                 layout: 'horizontal',
                //                 align: 'center',
                //                 verticalAlign: 'bottom'
                //             }
                //         }
                //     }]
                // }
            });
        }

        iniciarGrid();
        init();

        $(".bg-table-header").attr("style", "background: #3556ae !important");
        $("#tblComparativoFinanciero tbody tr").attr("style", "background: #3556ae !important");

        function limpiarGpx(todas, callback) {
            $.post('/CatMaquina/LimpiarGxp', { todas }).then(response => {
                if (response) {
                    initGpxInteresVsEfectiva('gpxInteresVsEfectiva', response.gpxBarra, callback, null);
                    initGpxPagoTotalBanco('gpxPagoTotalBanco', response.gpxLinea, null, null);
                } else {
                    AlertaGeneral(`Alerta`, 'Ocurrió un error');
                }
            }, error => {
                AlertaGeneral(`Operación fallida`, `Ocurrió un error al lanzar la petición al servidor: Error ${error.status} - ${error.statusText}.`);
            }
            );
        }
    };

    $(document).ready(function () {
        maquinaria.CatMaquina.AsignacionNoEconomico = new AsignacionNoEconomico();
    })
        .ajaxStart(() => { $.blockUI({ message: 'Procesando...', baseZ: 2000 }); })
        .ajaxStop(() => { $.unblockUI(); });

})();