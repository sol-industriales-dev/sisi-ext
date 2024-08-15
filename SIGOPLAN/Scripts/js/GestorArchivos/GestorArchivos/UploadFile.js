//Extensiones válidas para archivos a subir
const extensionesAceptadas =
    [
        "doc", "docx", "docm", "dotx", "dotm",
        "xls", "xlsx", "xlsm", "xltx", "xltm", "xlsb", "xlam",
        "ppt", "pptx", "pptm", "potx", "potm", "ppam", "ppsx", "ppsm", "sldx", "sldm",
        "txt", "pdf", "rar", "zip",
        "bmp", "gif", "jpg", "jpeg", "tiff", "png", "ico"
    ];

const extensionesNoAceptadas = ["exe", "app", "vb", "scr", "vbe", "vbs"];

const tamañoMaximoArchivos = 26214400; //Tamaño en bytes máximo permitido para subir archivos (actual 25MB)
let tamañoActual = 0;

function extensionValida(nombreArchivo) {
    const extension = nombreArchivo.split('.').pop();
    const extensionEsValida = extensionesAceptadas.filter(x => x === extension);
    return extensionEsValida.length > 0 ? true : false;
}

function extensionValida2(nombreArchivo) {
    const extension = nombreArchivo.split('.').pop();
    const extensionNoEsValida = extensionesNoAceptadas.filter(x => x === extension);
    return extensionNoEsValida.length > 0 ? false : true;
}

function excedeTamañoMaximo(archivos) {

    // for (let index = 0; index < archivos.length; index++) {
    //     tamañoActual += archivos[index].size;
    // }
    // return (tamañoActual > tamañoMaximoArchivos) ? true : false;
    return false;
}

function subirArchivo(folderID) {
    const archivosPorSubir = document.getElementById('subirArchivo');
    const datosPorSubir = new FormData();

    if (!archivosPorSubir.files.length > 0) {
        return;
    }

    if (excedeTamañoMaximo(archivosPorSubir.files)) {
        mostrarModal("Error", `El tamaño de los archivos excede lo permitido. Tamaño actual: ${((tamañoActual / 1024) / 1024).toFixed(2)}MB. Máximo: ${(tamañoMaximoArchivos / 1024) / 1024}MB.`);
        tamañoActual = 0;
        return;
    }
    tamañoActual = 0;

    for (let i = 0; i < archivosPorSubir.files.length; i++) {
        if (extensionValida2(archivosPorSubir.files[i].name)) {
            datosPorSubir.append("archivos", archivosPorSubir.files[i]);
        } else {
            mostrarModal("Aviso", "Error al cargar los archivos. El tipo de archivo no está permitido.");
            return;
        }
    }

    $.blockUI({ message: 'Cargando archivos...' });

    let exito = true;
    for (let index = 0; index < archivosPorSubir.files.length; index++) {

        let datosPorSubir = new FormData();
        datosPorSubir.append("listaArchivosFolder", archivosPorSubir.files[index]);

        const request = new XMLHttpRequest();
        request.open("POST", "/GestorArchivos/GestorArchivos/SubirArchivosFolder", false);
        request.send(datosPorSubir);
        request.onload = () => {
            if (request.status == 200) {
                let resultado = JSON.parse(request.response);
                if (!resultado.exito) {
                    exito = false;
                }
            } else {
                exito = false;
            }
        };

    }

    if (!exito) {
        mostrarModal("Error", "No se pudo completar la carga de archivos. ");
        return;
    }

    $.ajax({
        url: '/GestorArchivos/GestorArchivos/subirArchivo',
        datatype: "json",
        async: false,
        type: "POST",
        data: { folderID: folderID },
        success: function (response) {
            $.unblockUI();
            if (response === "[]") {
                mostrarModal("Aviso", "No se puede subir un archivo con el mismo nombre.");
                return;
            }
            if (response != "") {
                fManager.hx = null;
                response.forEach(element => {
                    fManager.add({
                        id: element.id,
                        value: element.value,
                        type: element.type,
                        date: element.date
                    }, -1, element.pId);
                });
                fManager.refreshCursor();
                mostrarModal("Aviso", "Archivos subidos correctamente.");
            } else {
                mostrarModal("Error", "Error al intentar subir los archivos.");
            }
        },
        error: function (error) {
            $.unblockUI();
            mostrarModal("Error", "No se pudo completar la carga de archivos. ");
        }
    });
    archivosPorSubir.value = "";
};

function subirFolder(folderID) {

    const archivosInput = document.getElementById('subirFolder').files;
    let listaArchivosFolder = [];

    for (let index = 0; index < archivosInput.length; index++) {
        if (archivosInput[index].size > 0) {
            listaArchivosFolder.push(archivosInput[index]);
        }
    }

    if (!listaArchivosFolder.length > 0) {
        return;
    }

    listaArchivosFolder.forEach(archivo => {
        if (!extensionValida2(archivo.name)) {
            mostrarModal("Aviso", "Error al cargar la caperta. La carpeta contiene archivos no permitidos.");
            return;
        }
    });

    function convertToHierarchy(listaArchivos /* array of array of strings */) {
        // Build the node structure
        const rootNode = { value: "root", data: [], parent: "" }
        for (let archivo of listaArchivos) {
            buildNodeRecursive(rootNode, archivo.webkitRelativePath.split('/'), 0);
        }
        return rootNode.data;
    }

    function buildNodeRecursive(node, path, idx) {
        if (idx < path.length) {
            let item = path[idx]
            let dir = node.data.find(child => child.value == item)
            if (!dir) {
                node.data.push(dir = { value: item, data: [], parent: node.value })
            }
            buildNodeRecursive(dir, path, idx + 1);
        }
    }

    const carpeta = /*JSON.stringify(*/convertToHierarchy(listaArchivosFolder)/*)*/;
    $.blockUI({ message: 'Cargando carpeta...' });
    $.ajax({
        url: '/GestorArchivos/GestorArchivos/subirEstructuraFolder',
        datatype: "json",
        async: false,
        type: "POST",
        data: carpeta[0],
        success: function (response) {
        },
        error: function (error) {
            console.log("Error");
        }
    });

    let exito = true;

    for (let index = 0; index < listaArchivosFolder.length; index++) {

        let datosPorSubir = new FormData();
        datosPorSubir.append("listaArchivosFolder", listaArchivosFolder[index]);

        const request = new XMLHttpRequest();
        request.open("POST", "/GestorArchivos/GestorArchivos/SubirArchivosFolder", false);
        request.send(datosPorSubir);
        request.onload = () => {
            if (request.status == 200) {
                let resultado = JSON.parse(request.response);
                if (!resultado.exito) {
                    exito = false;
                }
            } else {
                exito = false;
            }
        };

    }

    if (!exito) {
        mostrarModal("Error", "No se pudo completar la carga de carpeta. ");
        return;
    }

    $.ajax({
        url: '/GestorArchivos/GestorArchivos/subirFolder',
        datatype: "json",
        async: false,
        type: "POST",
        data: { folderID: folderID },
        success: function (response) {
            $.unblockUI();
            if (response.exito) {
                mostrarModal("Aviso", "Carpeta cargada exitosamente.");
                $$("files").load("/GestorArchivos/GestorArchivos/cargarDirectorios").then(() => {
                    fManager.refreshCursor();
                    fManager.refresh();
                });
            } else {
                mostrarModal("Error", "No se pudo completar la carga de carpeta. " + response.error);
            }
        },
        error: function (error) {
            $.unblockUI();
            mostrarModal("Error", "No se pudo completar la carga de carpeta. ");
        }
    });
    document.getElementById('subirFolder').value = "";
};

function actualizarArchivo(archivoID) {

    const archivoPorActualizar = document.getElementById('actualizarArchivo');
    const datosPorSubir = new FormData();
    const request = new XMLHttpRequest();
    const urlControlador = "/GestorArchivos/GestorArchivos/actualizarArchivo";

    if (!archivoPorActualizar.files.length > 0) {
        return;
    }

    if (excedeTamañoMaximo(archivoPorActualizar.files)) {
        mostrarModal("Error", `El tamaño del archivo excede lo permitido. Tamaño actual: ${((tamañoActual / 1024) / 1024).toFixed(2)}MB. Máximo: ${(tamañoMaximoArchivos / 1024) / 1024}MB.`);
        tamañoActual = 0;
        return;
    }
    tamañoActual = 0;


    if (extensionValida2(archivoPorActualizar.files[0].name)) {
        datosPorSubir.append("archivo", archivoPorActualizar.files[0]);
    } else {
        mostrarModal("Aviso", "Error al actualizar el archivo. El tipo de archivo no está permitido.");
        return;
    }

    datosPorSubir.append("archivoID", archivoID);
    request.open("POST", urlControlador);
    $.blockUI({ message: 'Actualizando archivo...' });
    request.send(datosPorSubir);
    request.onload = () => {
        $.unblockUI();
        archivoPorActualizar.value = "";
        if (request.status == 200) {
            const resultado = JSON.parse(request.response);
            if (request.response != "") {
                fManager.hx = null;
                const archivo = fManager.getItem(resultado.id);
                archivo.value = resultado.value;
                archivo.type = resultado.type;
                archivo.date = resultado.date;
                fManager.refreshCursor();
                mostrarModal("Aviso", "Archivo actualizado correctamente.");
            } else {
                mostrarModal("Error", "Error al intentar actualizar el archivo.");
            }
        };
    }
};