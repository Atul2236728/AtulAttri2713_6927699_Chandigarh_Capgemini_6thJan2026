// =============================================
// MediCare Hospital - site.js
// =============================================

$(document).ready(function () {

    // --- Sidebar Toggle ---
    $('#sidebarToggle').on('click', function () {
        $('#sidebar-wrapper').toggleClass('collapsed');
        $('#page-content-wrapper').toggleClass('expanded');
    });

    // --- Active Nav Link ---
    const path = window.location.pathname.toLowerCase();
    $('#sidebar-wrapper .sidebar-nav a').each(function () {
        const href = $(this).attr('href')?.toLowerCase() || '';
        if (href !== '/' && path.startsWith(href)) {
            $(this).addClass('active');
        }
    });

    // --- DataTables Init ---
    if ($('.data-table').length) {
        $('.data-table').DataTable({
            responsive: true,
            pageLength: 10,
            language: { search: '<i class="fas fa-search"></i>', searchPlaceholder: 'Search...' }
        });
    }

    // --- Auto-dismiss alerts ---
    setTimeout(function () {
        $('.alert.alert-success').fadeOut('slow');
    }, 4000);

    // --- Confirm delete/cancel ---
    $(document).on('click', '.btn-confirm', function (e) {
        const msg = $(this).data('confirm') || 'Are you sure?';
        if (!confirm(msg)) e.preventDefault();
    });

    // --- Medicine row adder for prescription form ---
    let medCount = 0;

    window.addMedicineRow = function (medicines) {
        medCount++;
        const idx = medCount;
        let options = '<option value="">-- Select Medicine --</option>';
        medicines.forEach(function (m) {
            options += `<option value="${m.medicineId}" data-price="${m.unitPrice}">${m.medicineName} (₹${m.unitPrice}/${m.unit})</option>`;
        });

        const row = `
        <div class="medicine-row border rounded p-3 mb-2" id="medrow-${idx}">
            <div class="row g-2 align-items-end">
                <div class="col-md-3">
                    <label class="form-label">Medicine</label>
                    <select name="SelectedMedicines[${idx}].MedicineId" class="form-select med-select" onchange="updateMedPrice(this)">${options}</select>
                </div>
                <div class="col-md-2">
                    <label class="form-label">Qty</label>
                    <input type="number" name="SelectedMedicines[${idx}].Quantity" class="form-control med-qty" value="1" min="1" onchange="calcMedTotal()" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Dosage</label>
                    <input type="text" name="SelectedMedicines[${idx}].Dosage" class="form-control" placeholder="e.g. 1-0-1" />
                </div>
                <div class="col-md-2">
                    <label class="form-label">Duration</label>
                    <input type="text" name="SelectedMedicines[${idx}].Duration" class="form-control" placeholder="e.g. 5 days" />
                </div>
                <div class="col-md-1">
                    <label class="form-label">Price</label>
                    <div class="form-control-plaintext fw-bold text-success med-price-display">₹0</div>
                </div>
                <div class="col-md-1">
                    <button type="button" class="btn btn-outline-danger btn-sm w-100" onclick="removeMedRow(${idx})"><i class="fas fa-trash"></i></button>
                </div>
            </div>
        </div>`;
        $('#medicine-rows').append(row);
    };

    window.removeMedRow = function (idx) {
        $(`#medrow-${idx}`).remove();
        calcMedTotal();
    };

    window.updateMedPrice = function (sel) {
        const price = parseFloat($(sel).find(':selected').data('price') || 0);
        const row = $(sel).closest('.medicine-row');
        const qty = parseInt(row.find('.med-qty').val() || 1);
        row.find('.med-price-display').text('₹' + (price * qty).toFixed(2));
        calcMedTotal();
    };

    window.calcMedTotal = function () {
        let total = 0;
        $('.medicine-row').each(function () {
            const price = parseFloat($(this).find('.med-select').find(':selected').data('price') || 0);
            const qty = parseInt($(this).find('.med-qty').val() || 1);
            $(this).find('.med-price-display').text('₹' + (price * qty).toFixed(2));
            total += price * qty;
        });
        $('#med-total').text('₹' + total.toFixed(2));
    };

    // --- Print bill ---
    window.printBill = function () {
        window.print();
    };
});
