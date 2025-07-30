// Check if daily inventory namespace already exists
if (typeof window.DailyInventory === 'undefined') {
    window.DailyInventory = {};
}

// Global variables for cached table data - only declare if not already declared
if (typeof window.DailyInventory.sellersRows === 'undefined') {
    window.DailyInventory.sellersRows = [];
}
if (typeof window.DailyInventory.productsRows === 'undefined') {
    window.DailyInventory.productsRows = [];
}
if (typeof window.DailyInventory.originalSellersRows === 'undefined') {
    window.DailyInventory.originalSellersRows = [];
}
if (typeof window.DailyInventory.originalProductsRows === 'undefined') {
    window.DailyInventory.originalProductsRows = [];
}
if (typeof window.DailyInventory.sellersCurrentPage === 'undefined') {
    window.DailyInventory.sellersCurrentPage = 1;
}
if (typeof window.DailyInventory.productsCurrentPage === 'undefined') {
    window.DailyInventory.productsCurrentPage = 1;
}
if (typeof window.DailyInventory.isInitialized === 'undefined') {
    window.DailyInventory.isInitialized = false;
}

const pageSize = 5;

// Create local references for easier access
const sellersRows = window.DailyInventory.sellersRows;
const productsRows = window.DailyInventory.productsRows;
const originalSellersRows = window.DailyInventory.originalSellersRows;
const originalProductsRows = window.DailyInventory.originalProductsRows;
let sellersCurrentPage = window.DailyInventory.sellersCurrentPage;
let productsCurrentPage = window.DailyInventory.productsCurrentPage;
let isInitialized = window.DailyInventory.isInitialized;

$(document).ready(function () {
    // Initialize only once on page load
    if (!isInitialized) {
        initializeDailyInventory();
        isInitialized = true;
        window.DailyInventory.isInitialized = true;
    }
});

function initializeDailyInventory() {
    console.log('Initializing daily inventory...');
    
    // Show sellers inventory by default (no AJAX)
    $('#sellers-inventory-table-container').show();
    $('#products-remain-table-container').hide();

    // Cache original table data - store both the DOM elements and their clones
    originalSellersRows.length = 0; // Clear array
    originalProductsRows.length = 0; // Clear array
    
    const sellersTableRows = $('#sellers-inventory-table tbody tr').toArray();
    const productsTableRows = $('#products-remain-table tbody tr').toArray();
    
    originalSellersRows.push(...sellersTableRows);
    originalProductsRows.push(...productsTableRows);
    
    // Create clones for pagination
    sellersRows.length = 0; // Clear array
    productsRows.length = 0; // Clear array
    
    sellersRows.push(...originalSellersRows.map(row => row.cloneNode(true)));
    productsRows.push(...originalProductsRows.map(row => row.cloneNode(true)));

    // Clear existing event handlers to prevent duplicates
    $('#sellers-inventory-table-container').off('click', '.pagination .page-link');
    $('#products-remain-table-container').off('click', '.pagination .page-link');
    $('#btnShowSellersInventory').off('click');
    $('#btnShowProductsRemain').off('click');

    // Clear existing pagination containers
    $('#sellers-inventory-table-container .pagination').empty();
    $('#products-remain-table-container .pagination').empty();

    // Reset pagination state
    sellersCurrentPage = 1;
    productsCurrentPage = 1;
    window.DailyInventory.sellersCurrentPage = 1;
    window.DailyInventory.productsCurrentPage = 1;

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
        // Initialize mobile table enhancements after showing table
        initMobileTableEnhancements();
    }

    function showProductsRemain() {
        $('#sellers-inventory-table-container').hide();
        $('#products-remain-table-container').show();
        paginateProductsTable(1);
        // Initialize mobile table enhancements after showing table
        initMobileTableEnhancements();
    }

    // Initialize mobile table enhancements
    initMobileTableEnhancements();

    // The rest of the AJAX and render functions remain for future use
}

// Function to reset daily inventory when switching sections
function resetDailyInventory() {
    console.log('Resetting daily inventory...');
    
    // Reset pagination state
    sellersCurrentPage = 1;
    productsCurrentPage = 1;
    window.DailyInventory.sellersCurrentPage = 1;
    window.DailyInventory.productsCurrentPage = 1;
    
    // Clear existing event handlers
    $('#sellers-inventory-table-container').off('click', '.pagination .page-link');
    $('#products-remain-table-container').off('click', '.pagination .page-link');
    $('#btnShowSellersInventory').off('click');
    $('#btnShowProductsRemain').off('click');
    
    // Clear pagination containers
    $('#sellers-inventory-table-container .pagination').empty();
    $('#products-remain-table-container .pagination').empty();
    
    // Re-initialize
    initializeDailyInventory();
}

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

// PDF Download function for Sellers Inventory
function downloadSellersInventoryAsPDF() {
    const reportTitle = 'تقرير جرد البائعين اليومي';
    const currentDate = new Date().toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    });

    // Get the visible sellers inventory table container
    const sellersTableContainer = document.getElementById('sellers-inventory-table-container');

    // Clone the container for PDF generation
    const pdfContainer = document.createElement('div');
    pdfContainer.style.cssText = `
        direction: rtl;
        text-align: right;
        font-family: 'Arial', sans-serif;
        padding: 20px;
        background: white;
        color: black;
        max-width: 100%;
    `;

    // Add report header
    let pdfContent = `
        <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
            <h1 style="color: #17a2b8; margin-bottom: 10px;">كافو</h1>
            <h2 style="color: #333; margin-bottom: 10px;">${reportTitle}</h2>
            <p style="color: #666; margin: 0;">تقرير شامل - ${currentDate}</p>
        </div>
    `;

    // Clone the table container and clean it up for PDF
    const tableClone = sellersTableContainer.cloneNode(true);

    // Remove action buttons and pagination from PDF
    const actionButtons = tableClone.querySelectorAll('.btn, .pagination-container, .d-flex.justify-content-end');
    actionButtons.forEach(btn => btn.remove());

    // Style the table for PDF
    const table = tableClone.querySelector('table');
    if (table) {
        table.style.cssText = 'width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 14px;';
        
        const headers = table.querySelectorAll('th');
        headers.forEach(th => {
            th.style.cssText = 'background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;';
        });
        
        const cells = table.querySelectorAll('td');
        cells.forEach(td => {
            td.style.cssText = 'padding: 10px; text-align: center; border: 1px solid #ddd;';
        });
        
        const rows = table.querySelectorAll('tr:nth-child(even)');
        rows.forEach(row => {
            row.style.backgroundColor = '#f8f9fa';
        });
    }

    // Add the cleaned content
    pdfContent += tableClone.innerHTML;

    // Add footer
    pdfContent += `
        <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd; text-align: center; color: #666; font-size: 12px;">
            <p>تم إنشاء هذا التقرير في: ${currentDate}</p>
        </div>
    `;

    pdfContainer.innerHTML = pdfContent;

    document.body.appendChild(pdfContainer);

    const opt = {
        margin: [0.5, 0.5, 0.5, 0.5],
        filename: `${reportTitle}-${new Date().toISOString().split('T')[0]}.pdf`,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { 
            scale: 2,
            useCORS: true,
            allowTaint: true,
            scrollY: 0,
            scrollX: 0
        },
        jsPDF: { 
            unit: 'in', 
            format: 'a4', 
            orientation: 'portrait',
            compress: true
        }
    };

    // Generate PDF
    html2pdf().set(opt).from(pdfContainer).save().then(() => {
        document.body.removeChild(pdfContainer);
    }).catch(error => {
        console.error('PDF generation error:', error);
        if (document.body.contains(pdfContainer)) {
            document.body.removeChild(pdfContainer);
        }
    });
}

// PDF Download function for Products Inventory
function downloadProductsInventoryAsPDF() {
    const reportTitle = 'تقرير جرد المنتجات المتبقية';
    const currentDate = new Date().toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    });

    // Get the visible products inventory table container
    const productsTableContainer = document.getElementById('products-remain-table-container');

    // Clone the container for PDF generation
    const pdfContainer = document.createElement('div');
    pdfContainer.style.cssText = `
        direction: rtl;
        text-align: right;
        font-family: 'Arial', sans-serif;
        padding: 20px;
        background: white;
        color: black;
        max-width: 100%;
    `;

    // Add report header
    let pdfContent = `
        <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
            <h1 style="color: #17a2b8; margin-bottom: 10px;">كافو</h1>
            <h2 style="color: #333; margin-bottom: 10px;">${reportTitle}</h2>
            <p style="color: #666; margin: 0;">تقرير شامل - ${currentDate}</p>
        </div>
    `;

    // Clone the table container and clean it up for PDF
    const tableClone = productsTableContainer.cloneNode(true);

    // Remove product image column (header and cells)
    const table = tableClone.querySelector('table');
    if (table) {
        // Remove the first column (image) from header
        const headerRow = table.querySelector('thead tr');
        if (headerRow) {
            const ths = headerRow.querySelectorAll('th');
            if (ths.length > 0) ths[0].remove();
        }
        // Remove the first cell from each body row
        const bodyRows = table.querySelectorAll('tbody tr');
        bodyRows.forEach(row => {
            const tds = row.querySelectorAll('td');
            if (tds.length > 0) tds[0].remove();
        });
    }

    // Remove action buttons and pagination from PDF
    const actionButtons = tableClone.querySelectorAll('.btn, .pagination-container, .d-flex.justify-content-end');
    actionButtons.forEach(btn => btn.remove());

    // Style the table for PDF
    if (table) {
        table.style.cssText = 'width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 14px;';
        
        const headers = table.querySelectorAll('th');
        headers.forEach(th => {
            th.style.cssText = 'background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;';
        });
        
        const cells = table.querySelectorAll('td');
        cells.forEach(td => {
            td.style.cssText = 'padding: 10px; text-align: center; border: 1px solid #ddd;';
        });
        
        const rows = table.querySelectorAll('tr:nth-child(even)');
        rows.forEach(row => {
            row.style.backgroundColor = '#f8f9fa';
        });
    }

    // Add the cleaned content
    pdfContent += tableClone.innerHTML;

    // Add footer
    pdfContent += `
        <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd; text-align: center; color: #666; font-size: 12px;">
            <p>تم إنشاء هذا التقرير في: ${currentDate}</p>
        </div>
    `;

    pdfContainer.innerHTML = pdfContent;

    document.body.appendChild(pdfContainer);

    const opt = {
        margin: [0.5, 0.5, 0.5, 0.5],
        filename: `${reportTitle}-${new Date().toISOString().split('T')[0]}.pdf`,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { 
            scale: 2,
            useCORS: true,
            allowTaint: true,
            scrollY: 0,
            scrollX: 0
        },
        jsPDF: { 
            unit: 'in', 
            format: 'a4', 
            orientation: 'portrait',
            compress: true
        }
    };

    // Generate PDF
    html2pdf().set(opt).from(pdfContainer).save().then(() => {
        document.body.removeChild(pdfContainer);
    }).catch(error => {
        console.error('PDF generation error:', error);
        if (document.body.contains(pdfContainer)) {
            document.body.removeChild(pdfContainer);
        }
    });
}

// Print function for Sellers Inventory
function printSellersInventory() {
    const reportTitle = 'تقرير جرد البائعين اليومي';
    const tableContent = generateSellersTableForPrint();
    const currentDate = new Date().toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    });

    // Create print window with enhanced design
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <!DOCTYPE html>
        <html dir="rtl">
        <head>
            <title>${reportTitle}</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; direction: rtl; background: #f8f9fa; }
                .header { text-align: center; margin-bottom: 30px; border-bottom: 3px solid #17a2b8; padding-bottom: 20px; background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%); padding: 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
                .header h1 { color: #17a2b8; margin-bottom: 10px; font-size: 28px; text-shadow: 1px 1px 2px rgba(0,0,0,0.1); }
                .header h2 { color: #333; margin-bottom: 10px; font-size: 20px; }
                .header p { color: #666; margin: 0; font-size: 14px; }
                .content { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
                table { width: 100%; border-collapse: collapse; margin: 20px 0; }
                th, td { border: 1px solid #ddd; padding: 12px; text-align: center; }
                th { background: linear-gradient(135deg, #343a40 0%, #495057 100%); color: white; font-weight: bold; }
                tr:nth-child(even) { background-color: #f8f9fa; }
                tr:hover { background-color: #e9ecef; }
                .footer { margin-top: 30px; padding-top: 20px; border-top: 2px solid #dee2e6; text-align: center; color: #666; font-size: 12px; background: #f8f9fa; padding: 15px; border-radius: 5px; }
                @media print {
                    body { margin: 0; background: white; }
                    .no-print { display: none; }
                    .header { box-shadow: none; }
                    .content { box-shadow: none; }
                }
            </style>
        </head>
        <body>
            <div class="header">
                <h1>كافو</h1>
                <h2>${reportTitle}</h2>
                <p>تقرير شامل - ${currentDate}</p>
            </div>
            <div class="content">
                ${tableContent}
            </div>
            <div class="footer">
                <p>تم إنشاء هذا التقرير في: ${currentDate}</p>
            </div>
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.print();
}

// Print function for Products Inventory
function printProductsInventory() {
    const reportTitle = 'تقرير جرد المنتجات المتبقية';
    const tableContent = generateProductsTableForPrint();
    const currentDate = new Date().toLocaleDateString('en-US', { 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric' 
    });

    // Create print window with enhanced design
    const printWindow = window.open('', '_blank');
    printWindow.document.write(`
        <!DOCTYPE html>
        <html dir="rtl">
        <head>
            <title>${reportTitle}</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; direction: rtl; background: #f8f9fa; }
                .header { text-align: center; margin-bottom: 30px; border-bottom: 3px solid #17a2b8; padding-bottom: 20px; background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%); padding: 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
                .header h1 { color: #17a2b8; margin-bottom: 10px; font-size: 28px; text-shadow: 1px 1px 2px rgba(0,0,0,0.1); }
                .header h2 { color: #333; margin-bottom: 10px; font-size: 20px; }
                .header p { color: #666; margin: 0; font-size: 14px; }
                .content { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
                table { width: 100%; border-collapse: collapse; margin: 20px 0; }
                th, td { border: 1px solid #ddd; padding: 12px; text-align: center; }
                th { background: linear-gradient(135deg, #343a40 0%, #495057 100%); color: white; font-weight: bold; }
                tr:nth-child(even) { background-color: #f8f9fa; }
                tr:hover { background-color: #e9ecef; }
                .footer { margin-top: 30px; padding-top: 20px; border-top: 2px solid #dee2e6; text-align: center; color: #666; font-size: 12px; background: #f8f9fa; padding: 15px; border-radius: 5px; }
                @media print {
                    body { margin: 0; background: white; }
                    .no-print { display: none; }
                    .header { box-shadow: none; }
                    .content { box-shadow: none; }
                }
            </style>
        </head>
        <body>
            <div class="header">
                <h1>كافو</h1>
                <h2>${reportTitle}</h2>
                <p>تقرير شامل - ${currentDate}</p>
            </div>
            <div class="content">
                ${tableContent}
            </div>
            <div class="footer">
                <p>تم إنشاء هذا التقرير في: ${currentDate}</p>
            </div>
        </body>
        </html>
    `);
    printWindow.document.close();
    printWindow.print();
}

// Helper function to generate sellers table for PDF (all data)
function generateSellersTableForPDF() {
    console.log('Generating sellers table for PDF...');
    
    let tableHtml = `
        <table style="width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 12px;">
            <thead>
                <tr style="background: #343a40; color: white;">
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">اسم البائع</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">رقم الهاتف</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">إجمالي الدفع النقدي</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">إجمالي الدفع الآجل</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">إجمالي المرتجعات الآجلة</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">إجمالي التوريد</th>
                </tr>
            </thead>
            <tbody>
    `;
    
    // Use original rows that contain all data
    const rowsToUse = originalSellersRows.length > 0 ? originalSellersRows : sellersRows;
    console.log('Using sellers rows:', rowsToUse.length);
    
    if (rowsToUse.length > 0) {
        rowsToUse.forEach((row, index) => {
            const cells = row.querySelectorAll('td');
            console.log(`Row ${index} has ${cells.length} cells`);
            
            if (cells.length > 0) {
                tableHtml += '<tr>';
                cells.forEach((cell, cellIndex) => {
                    const cellText = cell.textContent || cell.innerText || '';
                    console.log(`Cell ${cellIndex}: "${cellText.trim()}"`);
                    tableHtml += `<td style="padding: 8px; border: 1px solid #ddd; text-align: center;">${cellText.trim()}</td>`;
                });
                tableHtml += '</tr>';
            }
        });
    } else {
        console.log('No rows found in sellers table');
        tableHtml += '<tr><td colspan="6" style="padding: 8px; border: 1px solid #ddd; text-align: center;">لا توجد بيانات</td></tr>';
    }
    
    tableHtml += '</tbody></table>';
    console.log('Generated sellers table HTML length:', tableHtml.length);
    return tableHtml;
}

// Helper function to generate products table for PDF (all data)
function generateProductsTableForPDF() {
    console.log('Generating products table for PDF...');
    
    let tableHtml = `
        <table style="width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 12px;">
            <thead>
                <tr style="background: #343a40; color: white;">
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">اسم المنتج</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">عدد القطع في الصندوق</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">المتبقي بالصناديق</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">المتبقي بالقطع</th>
                    <th style="padding: 10px; border: 1px solid #ddd; text-align: center;">إجمالي المتبقي بالقطع</th>
                </tr>
            </thead>
            <tbody>
    `;
    
    // Use original rows that contain all data
    const rowsToUse = originalProductsRows.length > 0 ? originalProductsRows : productsRows;
    console.log('Using products rows:', rowsToUse.length);
    
    if (rowsToUse.length > 0) {
        rowsToUse.forEach((row, index) => {
            const cells = row.querySelectorAll('td');
            console.log(`Row ${index} has ${cells.length} cells`);
            
            if (cells.length > 1) { // Skip image column for PDF
                tableHtml += '<tr>';
                // Start from index 1 to skip the image column
                for (let i = 1; i < cells.length; i++) {
                    const cellText = cells[i].textContent || cells[i].innerText || '';
                    console.log(`Cell ${i}: "${cellText.trim()}"`);
                    tableHtml += `<td style="padding: 8px; border: 1px solid #ddd; text-align: center;">${cellText.trim()}</td>`;
                }
                tableHtml += '</tr>';
            }
        });
    } else {
        console.log('No rows found in products table');
        tableHtml += '<tr><td colspan="5" style="padding: 8px; border: 1px solid #ddd; text-align: center;">لا توجد بيانات</td></tr>';
    }
    
    tableHtml += '</tbody></table>';
    console.log('Generated products table HTML length:', tableHtml.length);
    return tableHtml;
}

// Helper function to generate sellers table for print (all data)
function generateSellersTableForPrint() {
    let tableHtml = `
        <table>
            <thead>
                <tr>
                    <th>اسم البائع</th>
                    <th>رقم الهاتف</th>
                    <th>إجمالي الدفع النقدي</th>
                    <th>إجمالي الدفع الآجل</th>
                    <th>إجمالي المرتجعات الآجلة</th>
                    <th>إجمالي التوريد</th>
                </tr>
            </thead>
            <tbody>
    `;
    
    // Use original rows that contain all data
    const rowsToUse = originalSellersRows.length > 0 ? originalSellersRows : sellersRows;
    
    if (rowsToUse.length > 0) {
        rowsToUse.forEach((row, index) => {
            const cells = row.querySelectorAll('td');
            if (cells.length > 0) {
                tableHtml += '<tr>';
                cells.forEach(cell => {
                    const cellText = cell.textContent || cell.innerText || '';
                    tableHtml += `<td>${cellText.trim()}</td>`;
                });
                tableHtml += '</tr>';
            }
        });
    } else {
        tableHtml += '<tr><td colspan="6">لا توجد بيانات</td></tr>';
    }
    
    tableHtml += '</tbody></table>';
    return tableHtml;
}

// Mobile table enhancement functions for daily inventory
function initMobileTableEnhancements() {
    if ($(window).width() <= 768) {
        enhanceTableScrolling();
        addTableScrollIndicators();
        improveTableTouchHandling();
    }
}

function enhanceTableScrolling() {
    $('.table-responsive').each(function() {
        const $container = $(this);
        const $table = $container.find('.table');
        
        // Ensure proper scrolling behavior
        $container.css({
            'overflow-x': 'auto',
            '-webkit-overflow-scrolling': 'touch',
            'scroll-behavior': 'smooth'
        });
        
        // Add scroll event listeners for better UX
        $container.on('scroll', function() {
            const scrollLeft = $(this).scrollLeft();
            const maxScroll = $(this)[0].scrollWidth - $(this).width();
            
            // Update scroll indicators
            updateScrollIndicators($(this), scrollLeft, maxScroll);
        });
    });
}

function addTableScrollIndicators() {
    $('.table-container').each(function() {
        const $container = $(this);
        const $tableResponsive = $container.find('.table-responsive');
        
        // Remove existing indicators to prevent duplicates
        $container.find('.scroll-indicator').remove();
        
        // Add scroll indicators
        $container.append(`
            <div class="scroll-indicator left" style="
                position: absolute;
                left: 10px;
                top: 50%;
                transform: translateY(-50%);
                background: rgba(0,0,0,0.7);
                color: white;
                padding: 8px 12px;
                border-radius: 20px;
                font-size: 12px;
                opacity: 0;
                transition: opacity 0.3s ease;
                pointer-events: none;
                z-index: 10;
            ">←</div>
            <div class="scroll-indicator right" style="
                position: absolute;
                right: 10px;
                top: 50%;
                transform: translateY(-50%);
                background: rgba(0,0,0,0.7);
                color: white;
                padding: 8px 12px;
                border-radius: 20px;
                font-size: 12px;
                opacity: 0;
                transition: opacity 0.3s ease;
                pointer-events: none;
                z-index: 10;
            ">→</div>
        `);
    });
}

function updateScrollIndicators($container, scrollLeft, maxScroll) {
    const $leftIndicator = $container.closest('.table-container').find('.scroll-indicator.left');
    const $rightIndicator = $container.closest('.table-container').find('.scroll-indicator.right');
    
    // Show/hide left indicator
    if (scrollLeft > 0) {
        $leftIndicator.css('opacity', '0.8');
    } else {
        $leftIndicator.css('opacity', '0');
    }
    
    // Show/hide right indicator
    if (scrollLeft < maxScroll - 1) {
        $rightIndicator.css('opacity', '0.8');
    } else {
        $rightIndicator.css('opacity', '0');
    }
}

function improveTableTouchHandling() {
    $('.table-responsive').each(function() {
        const $container = $(this);
        let startX = 0;
        let scrollLeft = 0;
        
        // Touch start
        $container.on('touchstart', function(e) {
            startX = e.touches[0].pageX - $container.offset().left;
            scrollLeft = $container.scrollLeft();
        });
        
        // Touch move
        $container.on('touchmove', function(e) {
            if (!startX) return;
            
            e.preventDefault();
            const x = e.touches[0].pageX - $container.offset().left;
            const walk = (startX - x) * 2; // Scroll speed multiplier
            $container.scrollLeft(scrollLeft + walk);
        });
        
        // Touch end
        $container.on('touchend', function() {
            startX = 0;
        });
    });
}

// Initialize mobile table enhancements when document is ready
$(document).ready(function() {
    // Initialize mobile table enhancements for daily inventory
    initMobileTableEnhancements();
    
    // Re-initialize on window resize
    $(window).on('resize', function() {
        initMobileTableEnhancements();
    });
});

// Helper function to generate products table for print (all data)
function generateProductsTableForPrint() {
    let tableHtml = `
        <table>
            <thead>
                <tr>
                    <th>اسم المنتج</th>
                    <th>عدد القطع في الصندوق</th>
                    <th>المتبقي بالصناديق</th>
                    <th>المتبقي بالقطع</th>
                    <th>إجمالي المتبقي بالقطع</th>
                </tr>
            </thead>
            <tbody>
    `;
    
    // Use original rows that contain all data
    const rowsToUse = originalProductsRows.length > 0 ? originalProductsRows : productsRows;
    
    if (rowsToUse.length > 0) {
        rowsToUse.forEach((row, index) => {
            const cells = row.querySelectorAll('td');
            if (cells.length > 1) { // Skip image column for print
                tableHtml += '<tr>';
                // Start from index 1 to skip the image column
                for (let i = 1; i < cells.length; i++) {
                    const cellText = cells[i].textContent || cells[i].innerText || '';
                    tableHtml += `<td>${cellText.trim()}</td>`;
                }
                tableHtml += '</tr>';
            }
        });
    } else {
        tableHtml += '<tr><td colspan="5">لا توجد بيانات</td></tr>';
    }
    
    tableHtml += '</tbody></table>';
    return tableHtml;
} 