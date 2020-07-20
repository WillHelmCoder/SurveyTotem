/*=========================================================================================
    File Name: datatables-basic.js
    Description: Basic Datatable
    ----------------------------------------------------------------------------------------
    Item Name: Modern Admin - Clean Bootstrap 4 Dashboard HTML Template
    Version: 1.0
    Author: PIXINVENT
    Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

$(document).ready(function() {

/****************************************
*       js of zero configuration        *
****************************************/

$('.zero-configuration').DataTable();

/**************************************
*       js of default ordering        *
**************************************/

$('.default-ordering').DataTable( {
    "order": [[ 3, "desc" ]]
} );

/************************************
*       js of multi ordering        *
************************************/

$('.multi-ordering').DataTable( {
    columnDefs: [ {
        targets: [ 0 ],
        orderData: [ 0, 1 ]
    }, {
        targets: [ 1 ],
        orderData: [ 1, 0 ]
    }, {
        targets: [ 4 ],
        orderData: [ 4, 0 ]
    } ]
} );

/*************************************
*       js of complex headers        *
*************************************/

$('.complex-headers').DataTable();

/*************************************
*       js of dom positioning        *
*************************************/

$('.dom-positioning').DataTable( {
    "dom": '<"top"i>rt<"bottom"flp><"clear">'
} );

/************************************
*       js of alt pagination        *
************************************/

$('.alt-pagination').DataTable( {
    "pagingType": "full_numbers"
} );

/*************************************
*       js of scroll vertical        *
*************************************/

$('.scroll-vertical').DataTable( {
    "scrollY":        "200px",
    "scrollCollapse": true,
    "paging":         false
} );

/************************************
*       js of dynamic height        *
************************************/

$('.dynamic-height').DataTable( {
    scrollY:        '50vh',
    scrollCollapse: true,
    paging:         false
} );

/***************************************
*       js of scroll horizontal        *
***************************************/

$('.scroll-horizontal').DataTable( {
    "scrollX": true
} );

/**************************************************
*       js of scroll horizontal & vertical        *
**************************************************/

$('.scroll-horizontal-vertical').DataTable( {
    "scrollY": 200,
    "scrollX": true
} );

/**********************************************
*       Language - Comma decimal place        *
**********************************************/

$('.comma-decimal-place').DataTable( {
    "language": {
        "decimal": ",",
        "thousands": "."
    }
} );

    /****************************************
*       js of Report Datatable        *
****************************************/

    $('.ReportDatatable').DataTable(
        {
            paging: false,
            info: true,
            searching: true,
            colReorder: true,
            ordering: true,
            retrieve: true,
            responsive: true,
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'collection',
                    text: 'Export',
                    background: true,
                    buttons: [
                        { extend: 'copy', text: 'Copy', className: 'copyButton' },
                        { extend: 'excel', text: 'Excel', filename: 'Excel Report', title: $(".ReportDatatable").attr("data-title") },
                        { extend: 'csv', text: 'CSV', filename: 'CSV Report', title: $(".ReportDatatable").attr("data-title") },
                        { extend: 'print', text: 'Print', filename: 'Printed Report', title: $(".ReportDatatable").attr("data-title") },
                        { extend: 'pdf', text: 'PDF', filename: 'PDF Report', title: $(".ReportDatatable").attr("data-title") },
                    ]
                }
            ]
    });


    $('.ReportDatatable-NoOrder').DataTable(
        {
            paging: false,
            info: true,
            searching: true,
            colReorder: true,
            ordering: false,
            retrieve: true,
            responsive: true,
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'collection',
                    text: 'Export',
                    background: true,
                    buttons: [
                        { extend: 'copy', text: 'Copy', className: 'copyButton' },
                        { extend: 'excel', text: 'Excel', filename: 'Excel Report', title: $(".ReportDatatable-NoOrder").attr("data-title") },
                        { extend: 'csv', text: 'CSV', filename: 'CSV Report', title: $(".ReportDatatable-NoOrder").attr("data-title") },
                        { extend: 'print', text: 'Print', filename: 'Printed Report', title: $(".ReportDatatable-NoOrder").attr("data-title")},
                        { extend: 'pdf', text: 'PDF', filename: 'PDF Report', title: $(".ReportDatatable-NoOrder").attr("data-title") },
                    ]
                }
            ]
        });


    $('.exportDatatable').DataTable(
        {
            paging: true,
            info: true,
            searching: true,
            colReorder: true,
            ordering: true,
            retrieve: true,
            responsive: true,
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'collection',
                    
                    text: 'Export',
                    background: true,
                    buttons: [
                        { extend: 'copy', text: 'Copy', exportOptions: { columns: ':visible th:not(.noExport)' },  className: 'copyButton' },
                        { extend: 'excel', text: 'Excel', filename: 'Excel Report', exportOptions: { columns: ':visible th:not(.noExport)'  }, title: 'Excel Report' },
                        { extend: 'csv', text: 'CSV', filename: 'CSV Report', exportOptions: { columns: ':visible th:not(.noExport)' }, title: 'CSV Report' },
                        { extend: 'print', text: 'Print', filename: 'Printed Report', exportOptions: { columns: ':visible th:not(.noExport)' }, title: 'Printed Report' },
                        { extend: 'pdf', text: 'PDF', filename: 'PDF Report', exportOptions: { columns: ':visible th:not(.noExport)' }, title: 'PDF Report' },
                    ]
                }
            ]
        });



});