// Global variables
let allInvoices = [];
let currentPage = 1;
const itemsPerPage = 5;
let currentInvoiceType = 'sell';

// Global functions that need to be accessible from HTML
window.createNewInvoice = function() {
    Swal.fire({
        title: 'إنشاء فاتورة شراء جديدة',
        text: 'سيتم توجيهك إلى صفحة إنشاء فاتورة الشراء',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'متابعة',
        cancelButtonText: 'إلغاء',
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#6c757d',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            // Show loading
            Swal.fire({
                title: 'جاري التوجيه...',
                html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
                allowOutsideClick: false,
                showConfirmButton: false,
                timer: 1500
            });
            
            // Navigate to the invoice creation page
            setTimeout(() => {
                window.location.href = '/seller/Invoice/Create';
            }, 1500);
        }
    });
};

window.setInvoiceType = function(type) {
    currentInvoiceType = type;
    currentPage = 1;
    
    // Update button states - check if elements exist first
    const invoiceTypeButtons = document.querySelectorAll('.invoice-type-btn');
    if (invoiceTypeButtons.length > 0) {
        invoiceTypeButtons.forEach(btn => {
            btn.classList.remove('active');
        });
        
        const activeButton = document.querySelector(`[data-invoice-type="${type}"]`);
        if (activeButton) {
            activeButton.classList.add('active');
        }
    }
    
    // Update title - check if element exists
    const titleElement = document.getElementById('invoice-title');
    if (titleElement) {
        let title = '';
        if (type === 'sell') title = 'فواتير البيع';
        else if (type === 'purchase') title = 'فواتير الشراء';
        else if (type === 'return') title = 'مرتجعات';
        else title = 'الفواتير';
        titleElement.textContent = title;
    }
    
    // Clear table - check if elements exist
    const tableBody = document.getElementById('invoices-table-body');
    if (tableBody) {
        tableBody.innerHTML = '';
    }
    
    const pagination = document.getElementById('invoices-pagination');
    if (pagination) {
        pagination.innerHTML = '';
    }
};

window.showInvoices = function() {
    const startDate = document.getElementById('invoiceStartDate').value;
    const endDate = document.getElementById('invoiceEndDate').value;

    if (!startDate || !endDate) {
        Swal.fire({
            icon: 'warning',
            title: 'تنبيه',
            text: 'يرجى تحديد تاريخ البدء وتاريخ الانتهاء',
            confirmButtonText: 'حسناً',
            confirmButtonColor: '#6f42c1'
        });
        return;
    }

    // Show loading with SweetAlert
    Swal.fire({
        title: 'جاري تحميل الفواتير...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Show loading in table
    document.getElementById('invoices-table-body').innerHTML = '<tr><td colspan="8" class="text-center">جاري التحميل...</td></tr>';
    
    fetch(`/Admin/Admin/GetInvoices?invoiceType=${currentInvoiceType}&startDate=${startDate}&endDate=${endDate}`)
        .then(response => response.json())
        .then(data => {
            allInvoices = data;
            currentPage = 1;
            renderTable();
            renderPagination();
            
            // Close loading
            Swal.close();
            
            // Show success message
            if (data.length > 0) {
                Swal.fire({
                    icon: 'success',
                    title: 'تم التحميل بنجاح!',
                    text: `تم العثور على ${data.length} فاتورة`,
                    timer: 2000,
                    showConfirmButton: false
                });
            } else {
                Swal.fire({
                    icon: 'info',
                    title: 'لا توجد فواتير',
                    text: 'لم يتم العثور على فواتير في النطاق المحدد',
                    confirmButtonText: 'حسناً',
                    confirmButtonColor: '#6f42c1'
                });
            }
        })
        .catch(error => {
            console.error('Error:', error);
            document.getElementById('invoices-table-body').innerHTML = '<tr><td colspan="8" class="text-center text-danger">حدث خطأ في تحميل البيانات</td></tr>';
            
            // Close loading and show error
            Swal.close();
            Swal.fire({
                icon: 'error',
                title: 'خطأ في التحميل',
                text: 'حدث خطأ أثناء تحميل الفواتير',
                confirmButtonText: 'حسناً',
                confirmButtonColor: '#dc3545'
            });
        });
};

window.changePage = function(page) {
    const totalPages = Math.ceil(allInvoices.length / itemsPerPage);
    if (page >= 1 && page <= totalPages) {
        currentPage = page;
        renderTable();
        renderPagination();
    }
};

// Modal functions
window.showInvoiceDetails = function(invoiceId) {
    // Find the invoice in the stored data
    const invoice = allInvoices.find(inv => inv.id === invoiceId);
    
    if (!invoice) {
        Swal.fire({
            icon: 'error',
            title: 'خطأ',
            text: 'لم يتم العثور على الفاتورة',
            confirmButtonText: 'حسناً',
            confirmButtonColor: '#dc3545'
        });
        return;
    }

    // Show loading
    Swal.fire({
        title: 'جاري تحميل التفاصيل...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Simulate loading (in real app, you might want to fetch detailed invoice data)
    setTimeout(() => {
        displayInvoiceModal(invoice);
        Swal.close();
    }, 500);
};

window.closeInvoiceModal = function() {
    document.getElementById('invoiceModal').style.display = 'none';
};

window.downloadInvoicePDF = function(invoiceId) {
    const invoice = allInvoices.find(inv => inv.id === invoiceId);
    if (!invoice) return;

    Swal.fire({
        title: 'جاري تحضير ملف PDF...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Create a temporary container for PDF generation
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

    // Add invoice header
    let pdfContent = `
        <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
            <h1 style="color: #17a2b8; margin-bottom: 10px;">كافو</h1>
            <h2 style="color: #333; margin-bottom: 10px;">فاتورة ${getInvoiceTypeText(invoice.type)} رقم #${invoice.id}</h2>
            <p style="color: #666; margin: 0;">التاريخ: <strong>${formatDate(invoice.createdAt)}</strong></p>
        </div>
    `;

    // Invoice and customer info
    pdfContent += `
        <div style="margin-bottom: 24px; display: flex; gap: 32px;">
            <div style="flex:1; background: #f8f9fa; padding: 15px; border-radius: 8px;">
                <h4 style="margin: 0 0 10px 0; color: #6f42c1;">معلومات الفاتورة</h4>
                <p><strong>رقم الفاتورة:</strong> #${invoice.id}</p>
                <p><strong>التاريخ:</strong> ${formatDate(invoice.createdAt)}</p>
                <p><strong>نوع الفاتورة:</strong> ${getInvoiceTypeText(invoice.type)}</p>
            </div>
            <div style="flex:1; background: #f8f9fa; padding: 15px; border-radius: 8px;">
                <h4 style="margin: 0 0 10px 0; color: #6f42c1;">معلومات العميل</h4>
                <p><strong>المستخدم:</strong> ${invoice.userName || 'غير محدد'}</p>
                <p><strong>العميل:</strong> ${invoice.customerName || 'غير محدد'}</p>
                <p><strong>عدد العناصر:</strong> ${invoice.itemsCount || 0}</p>
            </div>
        </div>
    `;

    // Table
    pdfContent += `
        <table style="width: 100%; border-collapse: collapse; margin: 20px 0; font-size: 14px;">
            <thead>
                <tr>
                    <th style="background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;">المنتج</th>
                    <th style="background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;">الكمية</th>
                    <th style="background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;">السعر</th>
                    <th style="background: #343a40; color: white; padding: 12px; text-align: center; border: 1px solid #ddd; font-weight: bold;">الإجمالي</th>
                </tr>
            </thead>
            <tbody>
                ${(invoice.items || []).map((item, idx) => `
                    <tr style="${idx % 2 === 1 ? 'background-color: #f8f9fa;' : ''}">
                        <td style="padding: 10px; text-align: center; border: 1px solid #ddd;">${item.productName || 'غير محدد'}</td>
                        <td style="padding: 10px; text-align: center; border: 1px solid #ddd;">${item.quantity}</td>
                        <td style="padding: 10px; text-align: center; border: 1px solid #ddd;">${formatCurrency(item.unitPrice)}</td>
                        <td style="padding: 10px; text-align: center; border: 1px solid #ddd;">${formatCurrency(item.totalPrice)}</td>
                    </tr>
                `).join('')}
            </tbody>
        </table>
    `;

    // Total
    pdfContent += `
        <div style="margin-top: 30px; padding: 20px; background: #f8f9fa; border-radius: 8px; font-size: 20px; font-weight: bold; color: #6f42c1; text-align: left;">
            المجموع الكلي للفاتورة: ${formatCurrency(invoice.total)}
        </div>
    `;

    // Footer
    pdfContent += `
        <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd; text-align: center; color: #666; font-size: 12px;">
            <p>تم إنشاء هذه الفاتورة في: ${new Date().toLocaleDateString('ar-EG')}</p>
        </div>
    `;

    pdfContainer.innerHTML = pdfContent;

    const opt = {
        margin: [0.5, 0.5, 0.5, 0.5],
        filename: `invoice_${invoice.id}_${invoice.type}.pdf`,
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

    document.body.appendChild(pdfContainer);
    html2pdf().set(opt).from(pdfContainer).save().then(() => {
        document.body.removeChild(pdfContainer);
        Swal.fire({
            icon: 'success',
            title: 'تم التحميل بنجاح!',
            text: `تم تحميل فاتورة رقم #${invoice.id} كملف PDF`,
            timer: 2000,
            showConfirmButton: false
        });
    }).catch(error => {
        document.body.removeChild(pdfContainer);
        Swal.fire({
            icon: 'error',
            title: 'خطأ في التحميل',
            text: 'حدث خطأ أثناء تحميل ملف PDF',
            confirmButtonText: 'حسناً',
            confirmButtonColor: '#dc3545'
        });
    });
};

window.printInvoice = function(invoiceId) {
    const invoice = allInvoices.find(inv => inv.id === invoiceId);
    if (!invoice) return;

    Swal.fire({
        title: 'جاري تحضير الطباعة...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    setTimeout(() => {
        const printContent = createInvoicePrintContent(invoice);
        const printWindow = window.open('', '_blank');
        printWindow.document.write(printContent);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();

        Swal.fire({
            icon: 'success',
            title: 'تم إرسال للطباعة!',
            text: `تم إرسال فاتورة رقم #${invoice.id} للطباعة`,
            timer: 2000,
            showConfirmButton: false
        });
    }, 1000);
};

// Helper functions
function setDefaultDates() {
    const startDateElement = document.getElementById('invoiceStartDate');
    const endDateElement = document.getElementById('invoiceEndDate');
    
    if (startDateElement && endDateElement) {
        const today = new Date();
        const thirtyDaysAgo = new Date(today.getTime() - (30 * 24 * 60 * 60 * 1000));
        
        startDateElement.value = thirtyDaysAgo.toISOString().split('T')[0];
        endDateElement.value = today.toISOString().split('T')[0];
    }
}

function renderTable() {
    const tbody = document.getElementById('invoices-table-body');
    if (!tbody) return; // Exit if table body doesn't exist
    
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const pageInvoices = allInvoices.slice(startIndex, endIndex);

    if (pageInvoices.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" class="text-center">لا توجد فواتير</td></tr>';
        return;
    }

    tbody.innerHTML = pageInvoices.map(invoice => `
        <tr>
            <td><span class="invoice-id">${invoice.id}</span></td>
            <td class="invoice-date">${formatDate(invoice.createdAt)}</td>
            <td class="invoice-user">${invoice.userName || 'غير محدد'}</td>
            <td><span class="invoice-total">${formatCurrency(invoice.total)}</span></td>
            <td><span class="invoice-type">${getInvoiceTypeText(invoice.type)}</span></td>
            <td class="invoice-customer">${invoice.customerName || 'غير محدد'}</td>
            <td><span class="invoice-items">${invoice.itemsCount || 0}</span></td>
            <td>
                <div class="action-buttons">
                    <button class="action-btn view" 
                            onclick="showInvoiceDetails(${invoice.id})" 
                            title="عرض التفاصيل">
                        <i class="fas fa-eye"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

function renderPagination() {
    const pagination = document.getElementById('invoices-pagination');
    if (!pagination) return; // Exit if pagination element doesn't exist
    
    const totalPages = Math.ceil(allInvoices.length / itemsPerPage);
    
    if (totalPages <= 1) {
        pagination.innerHTML = '';
        return;
    }

    let paginationHTML = '';
    
    // Previous button
    paginationHTML += `
        <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="changePage(${currentPage - 1})">السابق</a>
        </li>
    `;

    // Page numbers
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
        paginationHTML += `
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
            </li>
        `;
    }

    // Next button
    paginationHTML += `
        <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
            <a class="page-link" href="#" onclick="changePage(${currentPage + 1})">التالي</a>
        </li>
    `;

    pagination.innerHTML = paginationHTML;
}

function displayInvoiceModal(invoice) {
    // Use real items data from InvoiceManager if available, otherwise fallback to mock data
    let items = [];
    
    if (invoice.items && Array.isArray(invoice.items) && invoice.items.length > 0) {
        // Use real product data from InvoiceManager
        items = invoice.items.map(item => ({
            name: item.productName || 'غير محدد',
            quantity: item.quantity || 0,
            price: item.unitPrice || 0,
            total: item.totalPrice || 0
        }));
    } else {
        // Fallback to mock data if no real items
        const itemsCount = invoice.itemsCount || 3;
        for (let i = 1; i <= itemsCount; i++) {
            const price = Math.floor(Math.random() * 100) + 10;
            const quantity = Math.floor(Math.random() * 5) + 1;
            items.push({
                name: `منتج ${i}`,
                quantity: quantity,
                price: price,
                total: price * quantity
            });
        }
    }

    const itemsHtml = items.map(item => `
        <tr>
            <td>${item.name}</td>
            <td>${item.quantity}</td>
            <td>${formatCurrency(item.price)}</td>
            <td>${formatCurrency(item.total)}</td>
        </tr>
    `).join('');

    // --- Invoice Image Section ---
    let imageSection = '';
    if (invoice.imageUrl && invoice.imageUrl !== "") {
        imageSection = `
            <div class="card shadow-sm p-4 text-center mb-4" style="border-radius: 1rem; background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);">
                <h5 class="mb-3" style="color: #6f42c1; font-weight: 600;"><i class="fas fa-paperclip me-2"></i>مرفق صورة الفاتورة الورقية</h5>
                <a href="${invoice.imageUrl}" target="_blank" class="btn btn-lg btn-primary" style="background: linear-gradient(135deg, #007bff 0%, #0056b3 100%); border: none; border-radius: 0.5rem; font-weight: 600; box-shadow: 0 2px 8px rgba(0,0,0,0.08);">
                    <i class="fas fa-image me-2"></i>عرض الصورة في تبويب جديد
                </a>
            </div>
        `;
    } else {
        imageSection = `
            <div class="alert alert-warning d-flex align-items-center justify-content-center mb-4" style="border-radius: 0.5rem; font-size: 1.1rem;">
                <i class="fas fa-exclamation-circle me-2"></i>لا يوجد ملف مرفق لهذه الفاتورة
            </div>
        `;
    }

    const modalBody = document.getElementById('invoiceModalBody');
    modalBody.innerHTML = `
        ${imageSection}
        <div class="invoice-info-grid">
            <div class="invoice-info-card">
                <div class="invoice-info-label">رقم الفاتورة</div>
                <div class="invoice-info-value">#${invoice.id}</div>
            </div>
            <div class="invoice-info-card">
                <div class="invoice-info-label">التاريخ</div>
                <div class="invoice-info-value">${formatDate(invoice.createdAt)}</div>
            </div>
            <div class="invoice-info-card">
                <div class="invoice-info-label">المستخدم</div>
                <div class="invoice-info-value">${invoice.userName || 'غير محدد'}</div>
            </div>
            <div class="invoice-info-card">
                <div class="invoice-info-label">العميل</div>
                <div class="invoice-info-value">${invoice.customerName || 'غير محدد'}</div>
            </div>
            <div class="invoice-info-card">
                <div class="invoice-info-label">نوع الفاتورة</div>
                <div class="invoice-info-value">${getInvoiceTypeText(invoice.type)}</div>
            </div>
            <div class="invoice-info-card">
                <div class="invoice-info-label">عدد العناصر</div>
                <div class="invoice-info-value">${invoice.itemsCount || 0}</div>
            </div>
        </div>

        <div class="invoice-items-table">
            <table class="table">
                <thead>
                    <tr>
                        <th>المنتج</th>
                        <th>الكمية</th>
                        <th>السعر</th>
                        <th>الإجمالي</th>
                    </tr>
                </thead>
                <tbody>
                    ${itemsHtml}
                </tbody>
            </table>
        </div>

        <div class="invoice-total-section">
            <div class="invoice-total-label">المجموع الكلي</div>
            <div class="invoice-total-amount">${formatCurrency(invoice.total)}</div>
        </div>

        <div class="invoice-actions">
            <button class="invoice-action-btn download" onclick="downloadInvoicePDF(${invoice.id})">
                <i class="fas fa-download"></i>
                تحميل PDF
            </button>
            <button class="invoice-action-btn print" onclick="printInvoice(${invoice.id})">
                <i class="fas fa-print"></i>
                طباعة
            </button>
            <button class="invoice-action-btn close" onclick="closeInvoiceModal()">
                <i class="fas fa-times"></i>
                إغلاق
            </button>
        </div>
    `;

    // Show modal
    document.getElementById('invoiceModal').style.display = 'block';
}

function createInvoicePDFContent(invoice) {
    // Use real items data from InvoiceManager if available, otherwise fallback to mock data
    let items = [];
    
    if (invoice.items && Array.isArray(invoice.items) && invoice.items.length > 0) {
        // Use real product data from InvoiceManager
        items = invoice.items.map(item => ({
            name: item.productName || 'غير محدد',
            quantity: item.quantity || 0,
            price: item.unitPrice || 0,
            total: item.totalPrice || 0
        }));
    } else {
        // Fallback to mock data if no real items
        const itemsCount = invoice.itemsCount || 3;
        for (let i = 1; i <= itemsCount; i++) {
            const price = Math.floor(Math.random() * 100) + 10;
            const quantity = Math.floor(Math.random() * 5) + 1;
            items.push({
                name: `منتج ${i}`,
                quantity: quantity,
                price: price,
                total: price * quantity
            });
        }
    }

    const itemsHtml = items.map(item => `
        <tr>
            <td style="border: 1px solid #ddd; padding: 12px; text-align: center;">${item.name}</td>
            <td style="border: 1px solid #ddd; padding: 12px; text-align: center;">${item.quantity}</td>
            <td style="border: 1px solid #ddd; padding: 12px; text-align: center;">${formatCurrency(item.price)}</td>
            <td style="border: 1px solid #ddd; padding: 12px; text-align: center;">${formatCurrency(item.total)}</td>
        </tr>
    `).join('');

    return `
        <div style="direction: rtl; text-align: right; padding: 20px; font-family: 'Arial', sans-serif; background: white;">
            <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
                <h1 style="color: #6f42c1; margin: 0; font-size: 28px;">كافو</h1>
                <h2 style="margin: 10px 0; font-size: 20px;">فاتورة ${getInvoiceTypeText(invoice.type)} رقم #${invoice.id}</h2>
                <p style="margin: 5px 0; color: #666;">التاريخ: ${formatDate(invoice.createdAt)}</p>
            </div>
            
            <div style="margin-bottom: 30px; display: grid; grid-template-columns: 1fr 1fr; gap: 20px;">
                <div style="background: #f8f9fa; padding: 15px; border-radius: 8px;">
                    <h4 style="margin: 0 0 10px 0; color: #6f42c1;">معلومات الفاتورة</h4>
                    <p style="margin: 5px 0;"><strong>رقم الفاتورة:</strong> #${invoice.id}</p>
                    <p style="margin: 5px 0;"><strong>التاريخ:</strong> ${formatDate(invoice.createdAt)}</p>
                    <p style="margin: 5px 0;"><strong>نوع الفاتورة:</strong> ${getInvoiceTypeText(invoice.type)}</p>
                </div>
                <div style="background: #f8f9fa; padding: 15px; border-radius: 8px;">
                    <h4 style="margin: 0 0 10px 0; color: #6f42c1;">معلومات العميل</h4>
                    <p style="margin: 5px 0;"><strong>المستخدم:</strong> ${invoice.userName || 'غير محدد'}</p>
                    <p style="margin: 5px 0;"><strong>العميل:</strong> ${invoice.customerName || 'غير محدد'}</p>
                    <p style="margin: 5px 0;"><strong>عدد العناصر:</strong> ${invoice.itemsCount || 0}</p>
                </div>
            </div>
            
            <table style="width: 100%; border-collapse: collapse; margin-bottom: 30px;">
                <thead>
                    <tr style="background-color: #6f42c1; color: white;">
                        <th style="border: 1px solid #ddd; padding: 12px; text-align: center; font-weight: bold;">المنتج</th>
                        <th style="border: 1px solid #ddd; padding: 12px; text-align: center; font-weight: bold;">الكمية</th>
                        <th style="border: 1px solid #ddd; padding: 12px; text-align: center; font-weight: bold;">السعر</th>
                        <th style="border: 1px solid #ddd; padding: 12px; text-align: center; font-weight: bold;">الإجمالي</th>
                    </tr>
                </thead>
                <tbody>
                    ${itemsHtml}
                </tbody>
            </table>
            
            <div style="text-align: left; margin-top: 20px; padding: 20px; background: #f8f9fa; border-radius: 8px;">
                <div style="display: flex; justify-content: space-between; font-size: 18px; font-weight: bold;">
                    <span>المجموع الكلي:</span>
                    <span style="color: #6f42c1;">${formatCurrency(invoice.total)}</span>
            </div>
            </div>
        </div>
    `;
}

function createInvoicePrintContent(invoice) {
    return `
        <!DOCTYPE html>
        <html dir="rtl" lang="ar">
        <head>
            <meta charset="UTF-8">
            <title>فاتورة رقم #${invoice.id}</title>
            <style>
                body { 
                    font-family: 'Arial', sans-serif; 
                    margin: 20px; 
                    direction: rtl; 
                    background: white;
                }
                .invoice-header { 
                    text-align: center; 
                    margin-bottom: 30px; 
                    border-bottom: 2px solid #333; 
                    padding-bottom: 20px; 
                }
                .invoice-details { 
                    margin-bottom: 30px; 
                }
                .invoice-table { 
                    width: 100%; 
                    border-collapse: collapse; 
                    margin-bottom: 30px; 
                }
                .invoice-table th, .invoice-table td { 
                    border: 1px solid #ddd; 
                    padding: 12px; 
                    text-align: center; 
                }
                .invoice-table th { 
                    background-color: #6f42c1; 
                    color: white;
                    font-weight: bold; 
                }
                .total-section { 
                    text-align: left; 
                    font-weight: bold; 
                    font-size: 18px; 
                }
                @media print { 
                    body { margin: 0; } 
                }
            </style>
        </head>
        <body>
            ${createInvoicePDFContent(invoice)}
        </body>
        </html>
    `;
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('ar-EG', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('ar-EG', {
        style: 'currency',
        currency: 'EGP'
    }).format(amount);
}

function getInvoiceTypeText(type) {
    const typeMap = {
        'Cash': 'نقدي',
        'Credit': 'آجل',
        'Purchase': 'شراء',
        'CashReturn': 'مرتجع نقدي',
        'CreditReturn': 'مرتجع آجل',
        'PurchasingReturn': 'مرتجع شراء',
        'Return': 'مرتجع'
    };
    return typeMap[type] || type;
}

// Initialize when document is ready
$(document).ready(function() {
    // Only initialize invoice functionality if we're on the invoices page
    const invoiceContainer = document.getElementById('invoices-table-body');
    if (invoiceContainer) {
        // Initialize
        setInvoiceType('sell');
        setDefaultDates();
    }
    
    // Close modal when clicking outside
    window.onclick = function(event) {
        const modal = document.getElementById('invoiceModal');
        if (modal) { // Check if modal exists
            if (event.target === modal) {
                closeInvoiceModal();
            }
        }
    }
}); 