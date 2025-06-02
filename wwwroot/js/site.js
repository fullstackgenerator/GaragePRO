// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeAjaxSelect2(selector, placeholderText, ajaxUrl) {
    var $selectElement = $(selector);

    //initialize Select2
    $selectElement.select2({
        placeholder: placeholderText,
        minimumInputLength: 1,
        ajax: {
            url: ajaxUrl,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                //parameters sent to the server (e.g., ?term=...)
                return { term: params.term };
            },
            processResults: function (data) {
                return { results: data };
            },
            cache: true
        },
        matcher: function (params, data) {
            return data;
        },
        allowClear: true
    });

    //handle pre-selection for Edit forms (if data-selected-id exists)
    var selectedId = $selectElement.data('selected-id'); // Read the data attribute
    
    if (typeof selectedId === 'number' && selectedId > 0) {
        //aake an AJAX call to get the display text for the pre-selected ID
        $.ajax({
            type: 'GET',
            url: ajaxUrl,
            data: { term: selectedId },
            dataType: 'json'
        }).then(function (data) {
       
            var item = data.results.find(res => res.id === selectedId);
            if (item) {
        
                var option = new Option(item.text, item.id, true, true);
                $selectElement.append(option).trigger('change');
            }
        });
    }
}


$(document).ready(function () {
    //initialize Mechanic Select2 dropdown
    initializeAjaxSelect2(
        '#mechanic-select',
        "Search mechanic by name or assigned brand",
        '/WorkOrder/SearchMechanics'
    );

    //initialize Vehicle Select2 dropdown
    initializeAjaxSelect2(
        '#vehicle-select',
        "Search vehicle by VIN, make or model",
        '/WorkOrder/SearchVehicles'
    );
    
    //autoload invoice form
    $(document).ready(function () {
        $('#WorkOrderId').change(function () {
            const workOrderId = $(this).val();
            if (!workOrderId) {
                //clear fields if no work order is selected
                $('#SubTotal').val('');
                $('#TaxAmount').val('');
                $('#Total').val('');
                $('#AmountDue').val('');
                return;
            }
            
            $.get(`/Invoice/GetWorkOrderDetails?id=${workOrderId}`, function (data) {
                console.log("Raw data from server:", data);
                $('#SubTotal').val(data.subTotal.toFixed(2));
                $('#TaxAmount').val(data.tax.toFixed(2));
                $('#Total').val(data.total.toFixed(2));
                $('#AmountDue').val(data.amountDue.toFixed(2));
            }).fail(function(jqXHR, textStatus, errorThrown) {
                //error handling for the AJAX call
                console.error("Error fetching work order details:", textStatus, errorThrown);
                //clear fields or show an error message
                $('#SubTotal').val('');
                $('#TaxAmount').val('');
                $('#Total').val('');
                $('#AmountDue').val('');
                alert("Could not load work order details. Please try again.");
            });
        });
    });
    });

flatpickr(".datepicker", {
    dateFormat: "d. m. y",
    allowInput: true
});