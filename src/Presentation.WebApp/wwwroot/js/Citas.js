﻿$(document).ready(function () {

    consumirMetodoAccion("/Citas/ListarCitas", false, 'post', '{}', GnerarTablaCitas);

});

function listarCitas() {

    consumirMetodoAccion("/Citas/ListarCitas", false, 'post', '{}', GnerarTablaCitas);
}


function GnerarTablaCitas(response) {

    $('#citasTabla').DataTable().clear().destroy();


    $('#citasTabla').DataTable({
        "ordering": true,
        "searching": true,
        "lengthChange": true,

        "pageLength": 15,
        processing: true,


        deferRender: true,
        scrollY: 500,
        scrollCollapse: true,
        scroller: true,
        async: true,
        /**/

        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Citas/EjemploPagDataTables",
            "type": "POST",
            "datatype": "json"
        },

        /**/

        //data: response,
        columns: [
            { 'data': 'id' },
            { 'data': 'nombrePaciente' },
            { 'data': 'estatusCita' },
            { 'data': 'areaAtencion' },
            { 'data': 'fecha' },
            {
                mRender: function (data, type, row) {
                    return '<a href="#' + row.id + '" class="btn btn-danger m-1" onclick="eliminar(this)">Cancelar</a><a data-bs-toggle="modal" data-bs-target="#exampleModal2" data-bs-whatever="mdo"  href="#' + row.id + '" class="btn btn-info m-1" onclick="detalleCita(this)">Detalle</a><a class="btn btn-primary m-1" data-bs-toggle="modal" data-bs-target="#exampleModal" data-bs-whatever="mdo" onclick="editarCita(this)" href="#' + row.id + '">Editar</a >'
                }
            }
        ],
        dom: 'Bfrtip',
        buttons: [         
            {

                text: '<i class="fas fa-file-pdf"></i> Generar reporte',
                titleAttr: 'generarReporte',
                className: 'btn-import',
                action: function (e, dt, node, config) {
                    //consumirMetodoAccion("/Citas/GenerarReporte", false,"GET",null,null)
                    //window.location = "/Citas/GenerarReporte";
                    window.open('/Citas/GenerarReporte', '_blank');
                }

            }

        ],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.11.3/i18n/es_es.json",
            processing: '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only">Loading...</span> '

        }
    });




}

const eliminar = (idCitaElimiar) => {

    alertify.confirm('Cancelar cita', 'A continuación cancelará esta cita, desea continuar?',
        () => {

            let data = {
                id: idCitaElimiar.hash.replace('#', '')
            };
            consumirMetodoAccion("/Citas/CancelarCita", false, 'post', data, (result) => {
                if (result != null) {
                    alertify.success('La cita se canceló correctamente');
                    listarCitas();
                }
            });
        }, () => { alertify.error('No se canceló la cita') }

    ).set('labels', { ok: 'Sí', cancel: 'No' });
}

const detalleCita = (cita) => {

    /*$.ajax({
        url: location.origin + "/Citas/Details", // Url
        data: {
            id: cita.hash.replace('#', '')
            // ...
        },
        async: false,
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                document.querySelector("#tablaDetalleCita").innerHTML =
                    `<tbody >
                        <tr><td class="p-1">Identificador del paciente:</td><td class="p-2"><strong>${result.id}</strong></td></tr>
                        <tr><td class="p-1">Nombre del paciente:</td><td class="p-2"><strong>${result.nombrePaciente}</strong></td></tr>
                        <tr><td class="p-1">Fecha de la cita:</td><td class="p-2"><strong>${new Date(result.fecha).toLocaleString()}</strong></td></tr>
                        <tr><td class="p-1"> Estatus de cita:</td><td class="p-1"><strong>${result.estatusCita}</strong></td></tr>
                        <tr><td class="p-1">Área de atención:</td><td class="p-1"><strong>${result.areaAtencion}</strong></td></tr>
                    </tbody>`



            }
            else {
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });*/
    consumirMetodoAccion("/Citas/Details", false, "post", { id: cita.hash.replace('#', '') }, (result) => {

        if (result != null) {

            document.querySelector("#tablaDetalleCita").innerHTML =
                `<tbody >
                        <tr><td class="p-1">Identificador del paciente:</td><td class="p-2"><strong>${result.id}</strong></td></tr>
                        <tr><td class="p-1">Nombre del paciente:</td><td class="p-2"><strong>${result.nombrePaciente}</strong></td></tr>
                        <tr><td class="p-1">Fecha de la cita:</td><td class="p-2"><strong>${new Date(result.fecha).toLocaleString()}</strong></td></tr>
                        <tr><td class="p-1"> Estatus de cita:</td><td class="p-1"><strong>${result.estatusCita}</strong></td></tr>
                        <tr><td class="p-1">Área de atención:</td><td class="p-1"><strong>${result.areaAtencion}</strong></td></tr>
                    </tbody>`



        }

    });


}

const editarCita = (cita) => {

    let textValidacion = document.querySelector("#validacionFormularioEditarCita");
    textValidacion.innerHTML = '';
    $.ajax({
        type: "GET",
        url: location.origin + "/Citas/CatalogoEstatus",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: (response) => {
            $("#estatus").find('option').remove();

            $(response).each(function (i, v) {
                $("#estatus").append('<option value="' + v.idEstatus + '">' + v.nombre_Estatus + '</option>');
            })
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

    $.ajax({
        type: "GET",
        url: location.origin + "/Citas/CatalogoAreas",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: (response) => {
            $("#areatencion").find('option').remove();

            $(response).each(function (i, v) {
                $("#areatencion").append('<option value="' + v.id_Area + '">' + v.nombre_Area + '</option>');
            })
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });

    $.ajax({
        url: location.origin + "/Citas/Details", // Url
        data: {
            id: cita.hash.replace('#', '')
            // ...
        },
        async: false,
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                document.querySelector("#identificadorcita").value = result.id;
                document.querySelector("#nombrePaciente").value = result.nombrePaciente;
                document.querySelector("#observaciones").value = result.observaciones;
                document.querySelector("#fecha").value = result.fecha;

                $("#areatencion option:contains(" + result.areaAtencion + ")").attr('selected', true);
                $("#estatus option:contains(" + result.estatusCita + ")").attr('selected', true);

            }
            else {
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}

const enviarCitaEditada = function () {

    let datosCorrectos = true;
    let fecha = document.querySelector("#fecha");

    let observaciones = document.querySelector("#observaciones");
    let textValidacion = document.querySelector("#validacionFormularioEditarCita");
    textValidacion.innerHTML = '';
    if (observaciones.value == '') {
        textValidacion.innerHTML = "<li>El campo Observaciones es requerido</li>";
        datosCorrectos = false;
    }
    if (fecha.value == '') {
        textValidacion.innerHTML += "<li>El campo Fecha es requerido</li>";
        datosCorrectos = false;
    }

    let dataCita = {};

    dataCita.pacienteid = null;
    dataCita.idEstaus = document.querySelector("#estatus").value;
    dataCita.idArea = document.querySelector("#areatencion").value;
    dataCita.fecha = document.querySelector("#fecha").value;
    dataCita.observaciones = document.querySelector("#observaciones").value;

    if (datosCorrectos) {

        let data = { id: document.querySelector("#identificadorcita").value, dataCita };

        consumirMetodoAccion("/Citas/EditarCita", false, 'post', data, () => { $('#exampleModal').modal('hide') });

        $('#exampleModal').modal('hide');
        listarCitas();
    }

}