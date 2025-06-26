let currentInvoiceType = '';

function initializeInvoiceFunctionality() {
    console.log("Initializing invoice functionality");

    // Remove any existing event handlers
    $('.invoice-type-btn').off('click');

    // Add new event handlers
    $('.invoice-type-btn').on('click', function () {
        console.log("Invoice Type Btn Clicked");
        currentInvoiceType = $(this).data('invoice-type');
        const invoiceTitleElement = document.getElementById('invoice-title');

        $('.invoice-type-btn').removeClass('active');
        $(this).addClass('active');

        switch (currentInvoiceType) {
            case 'sell':
                invoiceTitleElement.innerText = 'فواتير البيع';
                break;
            case 'purchase':
                invoiceTitleElement.innerText = 'فواتير الشراء';
                break;
        }
        // Clear previous content before appending
        $('#invoice-details-section').find('.table-container, .pagination-container, .no-invoices-message').remove();
        $('#invoice-details-section').append(`
            <div class="no-invoices-message">
                <i class="fas fa-file-invoice fa-3x mb-3"></i>
                <p>الرجاء تحديد نطاق تاريخ لعرض الفواتير</p>
            </div>
        `);
    });
}

function showInvoices() {
    const startDate = document.getElementById('invoiceStartDate').value;
    const endDate = document.getElementById('invoiceEndDate').value;

    if (!startDate || !endDate) {
        Swal.fire({
            icon: 'warning',
            title: 'تنبيه',
            text: 'الرجاء تحديد تاريخي البدء والانتهاء',
            confirmButtonText: 'حسناً'
        });
        return;
    }

    // Validate that end date is greater than start date
    if (new Date(endDate) <= new Date(startDate)) {
        Swal.fire({
            icon: 'error',
            title: 'خطأ في التواريخ',
            text: 'تاريخ الانتهاء يجب أن يكون أكبر من تاريخ البدء',
            confirmButtonText: 'حسناً'
        });
        return;
    }

    loadInvoices(currentInvoiceType, 1, startDate, endDate);
}

function loadInvoices(invoiceType, page, startDate = null, endDate = null) {
    // Show loading indicator
    $('#invoice-details-section').html('<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>');

    // Construct URL
    let url = `/Admin/Admin/Invoices?invoiceType=${invoiceType}&page=${page}`;
    if (startDate && endDate) {
        url += `&startDate=${startDate}&endDate=${endDate}`;
    }

    // Load the partial view via AJAX
    $.ajax({
        url: url,
        type: 'GET',
        success: function (result) {
            $('#invoice-details-section').html($(result).find('#invoice-details-section').html());
            initializeInvoiceFunctionality();
        },
        error: function (error) {
            $('#invoice-details-section').html('<div class="alert alert-danger">خطأ في تحميل الفواتير</div>');
        }
    });
}

// Function to show add invoice modal
function showAddInvoiceModal() {
    Swal.fire({
        title: 'إضافة فاتورة شراء جديدة',
        html: `
            <div class="text-center">
                <i class="fas fa-file-invoice fa-3x text-primary mb-3"></i>
                <p class="text-muted">سيتم توجيهك إلى صفحة إضافة فاتورة الشراء</p>
            </div>
        `,
        icon: 'info',
        showCancelButton: true,
        confirmButtonText: 'متابعة',
        cancelButtonText: 'إلغاء',
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#6c757d',
        background: 'white',
        backdrop: `
            rgba(0,0,0,0.5)
            center
            no-repeat
        `,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            console.log('User confirmed, about to show SweetAlert...');

            //Swal.fire({
            //    icon: 'success',
            //    title: 'سيتم توجيهك قريباً',
            //    text: 'صفحة إضافة فاتورة الشراء قيد التطوير',
            //    timer: 3000,
            //    showConfirmButton: false,
            //    showProgressBar: true
            //});

            console.log('SweetAlert should be showing now...');

            // Try navigation after a delay
            setTimeout(() => {
                console.log('Timeout reached, attempting navigation...');
                window.location.href = '/Seller/Invoice/Create';
            }, 4000);
        }
    
    });
}

// Function to show invoice details
function showInvoiceDetails(invoiceId, invoiceType) {
    // Show loading modal first
    Swal.fire({
        title: 'جاري تحميل تفاصيل الفاتورة...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Simulate loading invoice details (replace with actual API call)
    setTimeout(() => {
        // Mock invoice details - replace with actual data from your backend
        const mockInvoiceDetails = {
            id: invoiceId,
            type: invoiceType,
            date: new Date().toLocaleDateString('ar-SA'),
            time: new Date().toLocaleTimeString('ar-SA'),
            user: 'أحمد محمد',
            customer: invoiceType === 'sell' ? 'محمد علي' : 'شركة الموردين',
            total: (Math.random() * 1000 + 100).toFixed(2),
            items: [
                { name: 'منتج 1', quantity: 2, price: 50, total: 100 },
                { name: 'منتج 2', quantity: 1, price: 75, total: 75 },
                { name: 'منتج 3', quantity: 3, price: 25, total: 75 }
            ],
            notes: 'ملاحظات إضافية حول الفاتورة'
        };

        displayInvoiceDetailsModal(mockInvoiceDetails);
    }, 1000);
}

// Function to display invoice details in modal
function displayInvoiceDetailsModal(invoice) {
    const itemsHtml = invoice.items.map(item => `
        <tr>
            <td>${item.name}</td>
            <td class="text-center">${item.quantity}</td>
            <td class="text-center">${item.price.toFixed(2)} ر.س</td>
            <td class="text-center fw-bold">${item.total.toFixed(2)} ر.س</td>
        </tr>
    `).join('');

    Swal.fire({
        title: `تفاصيل الفاتورة رقم #${invoice.id}`,
        html: `
            <div class="invoice-details-modal">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-calendar-alt text-primary"></i>
                            <span class="detail-label">التاريخ:</span>
                            <span class="detail-value">${invoice.date}</span>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-clock text-primary"></i>
                            <span class="detail-label">الوقت:</span>
                            <span class="detail-value">${invoice.time}</span>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-user text-primary"></i>
                            <span class="detail-label">المستخدم:</span>
                            <span class="detail-value">${invoice.user}</span>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-${invoice.type === 'sell' ? 'user-tie' : 'building'} text-primary"></i>
                            <span class="detail-label">${invoice.type === 'sell' ? 'العميل:' : 'المورد:'}</span>
                            <span class="detail-value">${invoice.customer}</span>
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-12">
                        <div class="detail-item">
                            <i class="fas fa-tag text-primary"></i>
                            <span class="detail-label">نوع الفاتورة:</span>
                            <span class="detail-value badge bg-${invoice.type === 'sell' ? 'success' : 'info'}">${invoice.type === 'sell' ? 'بيع' : 'شراء'}</span>
                        </div>
                    </div>
                </div>
                <hr>
                <h6 class="mb-3"><i class="fas fa-list me-2"></i>تفاصيل المنتجات:</h6>
                <div class="table-responsive">
                    <table class="table table-sm table-bordered">
                        <thead class="table-light">
                            <tr>
                                <th>المنتج</th>
                                <th class="text-center">الكمية</th>
                                <th class="text-center">السعر</th>
                                <th class="text-center">الإجمالي</th>
                            </tr>
                        </thead>
                        <tbody>
                            ${itemsHtml}
                        </tbody>
                    </table>
                </div>
                <div class="row mt-3">
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-sticky-note text-primary"></i>
                            <span class="detail-label">ملاحظات:</span>
                            <span class="detail-value">${invoice.notes}</span>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="detail-item">
                            <i class="fas fa-calculator text-primary"></i>
                            <span class="detail-label">المجموع الكلي:</span>
                            <span class="detail-value fw-bold text-success">${invoice.total} ر.س</span>
                        </div>
                    </div>
                </div>
                <div class="invoice-action-buttons">
                    <div class="d-flex gap-2 justify-content-end">
                        <button class="btn invoice-action-btn primary" onclick="downloadInvoiceAsPDF(${invoice.id}, '${invoice.type}')">
                            <i class="fas fa-download me-2"></i>تحميل PDF
                        </button>
                        <button class="btn invoice-action-btn secondary" onclick="printInvoice(${invoice.id}, '${invoice.type}')">
                            <i class="fas fa-print me-2"></i>طباعة
                        </button>
                    </div>
                </div>
            </div>
        `,
        width: '800px',
        showConfirmButton: true,
        confirmButtonText: 'إغلاق',
        confirmButtonColor: '#6c757d',
        showCloseButton: true,
        background: 'white',
        backdrop: `
            rgba(0,0,0,0.5)
            center
            no-repeat
        `,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        },
        customClass: {
            container: 'invoice-details-modal-container'
        }
    });
}

// Function to download invoice as PDF
function downloadInvoiceAsPDF(invoiceId, invoiceType) {
    Swal.fire({
        title: 'جاري تحضير ملف PDF...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Simulate PDF generation (replace with actual API call)
    setTimeout(() => {
        // Create a temporary div with invoice content for PDF generation
        const invoiceContent = createInvoiceContentForPDF(invoiceId, invoiceType);

        const opt = {
            margin: 1,
            filename: `invoice_${invoiceId}_${invoiceType}.pdf`,
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 2 },
            jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
        };

        html2pdf().set(opt).from(invoiceContent).save().then(() => {
            Swal.fire({
                icon: 'success',
                title: 'تم التحميل بنجاح!',
                text: `تم تحميل فاتورة رقم #${invoiceId} كملف PDF`,
                timer: 2000,
                showConfirmButton: false
            });
        }).catch(error => {
            Swal.fire({
                icon: 'error',
                title: 'خطأ في التحميل',
                text: 'حدث خطأ أثناء تحميل ملف PDF',
                confirmButtonText: 'حسناً'
            });
        });
    }, 1500);
}

// Function to print invoice
function printInvoice(invoiceId, invoiceType) {
    Swal.fire({
        title: 'جاري تحضير الطباعة...',
        html: '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>',
        allowOutsideClick: false,
        showConfirmButton: false
    });

    // Simulate print preparation (replace with actual API call)
    setTimeout(() => {
        // Create a temporary div with invoice content for printing
        const invoiceContent = createInvoiceContentForPrint(invoiceId, invoiceType);

        // Create a new window for printing
        const printWindow = window.open('', '_blank');
        printWindow.document.write(`
            <!DOCTYPE html>
            <html dir="rtl" lang="ar">
            <head>
                <meta charset="UTF-8">
                <title>فاتورة رقم #${invoiceId}</title>
                <style>
                    body { font-family: 'Arial', sans-serif; margin: 20px; direction: rtl; }
                    .invoice-header { text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px; }
                    .invoice-details { margin-bottom: 30px; }
                    .invoice-table { width: 100%; border-collapse: collapse; margin-bottom: 30px; }
                    .invoice-table th, .invoice-table td { border: 1px solid #ddd; padding: 12px; text-align: center; }
                    .invoice-table th { background-color: #f8f9fa; font-weight: bold; }
                    .total-section { text-align: left; font-weight: bold; font-size: 18px; }
                    @@media print { body { margin: 0; } }
                </style>
            </head>
            <body>
                ${invoiceContent}
            </body>
            </html>
        `);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();

        Swal.fire({
            icon: 'success',
            title: 'تم إرسال للطباعة!',
            text: `تم إرسال فاتورة رقم #${invoiceId} للطباعة`,
            timer: 2000,
            showConfirmButton: false
        });
    }, 1000);
}

// Function to create invoice content for PDF
function createInvoiceContentForPDF(invoiceId, invoiceType) {
    // Mock data - replace with actual invoice data
    const invoice = {
        id: invoiceId,
        type: invoiceType,
        date: new Date().toLocaleDateString('ar-SA'),
        time: new Date().toLocaleTimeString('ar-SA'),
        user: 'أحمد محمد',
        customer: invoiceType === 'sell' ? 'محمد علي' : 'شركة الموردين',
        total: (Math.random() * 1000 + 100).toFixed(2),
        items: [
            { name: 'منتج 1', quantity: 2, price: 50, total: 100 },
            { name: 'منتج 2', quantity: 1, price: 75, total: 75 },
            { name: 'منتج 3', quantity: 3, price: 25, total: 75 }
        ],
        notes: 'ملاحظات إضافية حول الفاتورة'
    };

    const itemsHtml = invoice.items.map(item => `
        <tr>
            <td>${item.name}</td>
            <td>${item.quantity}</td>
            <td>${item.price.toFixed(2)} ر.س</td>
            <td>${item.total.toFixed(2)} ر.س</td>
        </tr>
    `).join('');

    return `
        <div style="direction: rtl; text-align: right; padding: 20px; font-family: 'Arial', sans-serif;">
            <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
                <h1>كافو</h1>
                <h2>فاتورة ${invoice.type === 'sell' ? 'بيع' : 'شراء'} رقم #${invoice.id}</h2>
                <p>التاريخ: ${invoice.date} | الوقت: ${invoice.time}</p>
            </div>
            
            <div style="margin-bottom: 30px;">
                <div style="margin-bottom: 10px;">
                    <strong>المستخدم:</strong> ${invoice.user}
                </div>
                <div style="margin-bottom: 10px;">
                    <strong>${invoice.type === 'sell' ? 'العميل:' : 'المورد:'}</strong> ${invoice.customer}
                </div>
                <div style="margin-bottom: 10px;">
                    <strong>نوع الفاتورة:</strong> ${invoice.type === 'sell' ? 'بيع' : 'شراء'}
                </div>
            </div>
            
            <table style="width: 100%; border-collapse: collapse; margin-bottom: 30px;">
                <thead>
                    <tr style="background-color: #f8f9fa;">
                        <th style="border: 1px solid #ddd; padding: 12px;">المنتج</th>
                        <th style="border: 1px solid #ddd; padding: 12px;">الكمية</th>
                        <th style="border: 1px solid #ddd; padding: 12px;">السعر</th>
                        <th style="border: 1px solid #ddd; padding: 12px;">الإجمالي</th>
                    </tr>
                </thead>
                <tbody>
                    ${itemsHtml}
                </tbody>
            </table>
            
            <div style="margin-bottom: 20px;">
                <strong>ملاحظات:</strong> ${invoice.notes}
            </div>
            
            <div style="text-align: left; font-weight: bold; font-size: 18px; border-top: 2px solid #333; padding-top: 20px;">
                <strong>المجموع الكلي: ${invoice.total} ر.س</strong>
            </div>
        </div>
    `;
}

// Function to create invoice content for printing
function createInvoiceContentForPrint(invoiceId, invoiceType) {
    // Use the same content as PDF for consistency
    return createInvoiceContentForPDF(invoiceId, invoiceType);
}

// Initialize when document is ready
$(document).ready(function () {
    initializeInvoiceFunctionality();
}); 