$(document).ready(function () {
    // Show sellers inventory by default (no AJAX)
    $('#sellers-inventory-table-container').show();
    $('#products-remain-table-container').hide();

    // Pagination state
    let sellersCurrentPage = 1;
    let productsCurrentPage = 1;
    const pageSize = 5;

    // Cache original table data
    let sellersRows = $('#sellers-inventory-table tbody tr').toArray();
    let productsRows = $('#products-remain-table tbody tr').toArray();

    function renderTablePage(tableId, rows, page, pageSize) {
        const start = (page - 1) * pageSize;
        const end = start + pageSize;
        const pageRows = rows.slice(start, end);
        $(tableId + ' tbody').empty().append(pageRows);
    }

    function updatePaginationControls(containerSelector, currentPage, totalPages) {
        const pagination = $(containerSelector + ' .pagination');
        pagination.empty();
        // Previous
        pagination.append(`<li class="page-item${currentPage === 1 ? ' disabled' : ''}"><a class="page-link" href="#" data-page="prev">السابق</a></li>`);
        // Page numbers
        for (let i = 1; i <= totalPages; i++) {
            pagination.append(`<li class="page-item${i === currentPage ? ' active' : ''}"><a class="page-link" href="#" data-page="${i}">${i}</a></li>`);
        }
        // Next
        pagination.append(`<li class="page-item${currentPage === totalPages ? ' disabled' : ''}"><a class="page-link" href="#" data-page="next">التالي</a></li>`);
    }

    function paginateSellersTable(page) {
        const totalPages = Math.ceil(sellersRows.length / pageSize) || 1;
        sellersCurrentPage = Math.max(1, Math.min(page, totalPages));
        renderTablePage('#sellers-inventory-table', sellersRows, sellersCurrentPage, pageSize);
        updatePaginationControls('#sellers-inventory-table-container', sellersCurrentPage, totalPages);
    }

    function paginateProductsTable(page) {
        const totalPages = Math.ceil(productsRows.length / pageSize) || 1;
        productsCurrentPage = Math.max(1, Math.min(page, totalPages));
        renderTablePage('#products-remain-table', productsRows, productsCurrentPage, pageSize);
        updatePaginationControls('#products-remain-table-container', productsCurrentPage, totalPages);
    }

    // Initial pagination
    paginateSellersTable(1);
    paginateProductsTable(1);

    // Pagination click handlers
    $('#sellers-inventory-table-container').on('click', '.pagination .page-link', function (e) {
        e.preventDefault();
        const val = $(this).data('page');
        const totalPages = Math.ceil(sellersRows.length / pageSize) || 1;
        if (val === 'prev' && sellersCurrentPage > 1) {
            paginateSellersTable(sellersCurrentPage - 1);
        } else if (val === 'next' && sellersCurrentPage < totalPages) {
            paginateSellersTable(sellersCurrentPage + 1);
        } else if (typeof val === 'number' || !isNaN(parseInt(val))) {
            paginateSellersTable(parseInt(val));
        }
    });
    $('#products-remain-table-container').on('click', '.pagination .page-link', function (e) {
        e.preventDefault();
        const val = $(this).data('page');
        const totalPages = Math.ceil(productsRows.length / pageSize) || 1;
        if (val === 'prev' && productsCurrentPage > 1) {
            paginateProductsTable(productsCurrentPage - 1);
        } else if (val === 'next' && productsCurrentPage < totalPages) {
            paginateProductsTable(productsCurrentPage + 1);
        } else if (typeof val === 'number' || !isNaN(parseInt(val))) {
            paginateProductsTable(parseInt(val));
        }
    });

    $('#btnShowSellersInventory').on('click', function () {
        showSellersInventory();
    });
    $('#btnShowProductsRemain').on('click', function () {
        showProductsRemain();
    });

    function showSellersInventory() {
        $('#sellers-inventory-table-container').show();
        $('#products-remain-table-container').hide();
        paginateSellersTable(1);
    }

    function showProductsRemain() {
        $('#sellers-inventory-table-container').hide();
        $('#products-remain-table-container').show();
        paginateProductsTable(1);
    }

    // The rest of the AJAX and render functions remain for future use
}); 