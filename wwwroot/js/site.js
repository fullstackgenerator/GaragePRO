// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function initializeAjaxSelect2(selector, placeholderText, ajaxUrl) {
    var $selectElement = $(selector);

    // Initialize Select2
    $selectElement.select2({
        placeholder: placeholderText,
        minimumInputLength: 1, // Minimum characters before searching
        ajax: {
            url: ajaxUrl,
            dataType: 'json',
            delay: 250, // Debounce delay for typing
            data: function (params) {
                // Parameters sent to the server (e.g., ?term=...)
                return { term: params.term };
            },
            processResults: function (data) {
                // Transforms the data returned by the server into the format Select2 expects ({ results: [...] })
                return { results: data };
            },
            cache: true // Cache AJAX results for performance
        },
        // As the server is doing the primary filtering, a simple matcher is sufficient.
        // It simply passes through the results received from the AJAX call.
        matcher: function (params, data) {
            return data;
        },
        allowClear: true // Adds a clear button to the selected item
    });

    // Handle pre-selection for Edit forms (if data-selected-id exists)
    var selectedId = $selectElement.data('selected-id'); // Read the data attribute

    // Check if selectedId is a valid number (e.g., 0 for no selection, or > 0)
    // We check for type 'number' for robustness in case it's string "0" from C#
    if (typeof selectedId === 'number' && selectedId > 0) {
        // Make an AJAX call to get the display text for the pre-selected ID
        $.ajax({
            type: 'GET',
            url: ajaxUrl, // Re-use the same AJAX URL
            data: { term: selectedId }, // Send the ID as the term
            dataType: 'json'
        }).then(function (data) {
            // Assuming the AJAX endpoint returns an array of { id, text }
            var item = data.results.find(res => res.id === selectedId); // Use strict equality (===)
            if (item) {
                // Create a new option element, append it, and trigger Select2 to select it
                var option = new Option(item.text, item.id, true, true);
                $selectElement.append(option).trigger('change');
            }
        });
    }
}

// Ensure the code runs after the DOM is fully loaded
$(document).ready(function () {
    // Initialize Mechanic Select2 dropdown
    initializeAjaxSelect2(
        '#mechanic-select',
        "Search mechanic by name or assigned brand",
        '/WorkOrder/SearchMechanics'
    );

    // Initialize Vehicle Select2 dropdown
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