let currentReportType = '';

function initializeReportFunctionality() {
    console.log("Initializing report functionality");
    
    // Remove any existing event handlers
    $('.report-type-btn').off('click');
    
    // Add new event handlers
    $('.report-type-btn').on('click', function () {
        console.log("Btn Report Clicked");
        currentReportType = $(this).data('report-type');
        const reportTitleElement = document.getElementById('report-title');
        const reportDetailsSection = document.getElementById('report-details-section');

        // Always show the date input group unless it's a future/present report
        if (currentReportType === 'stock_money' || currentReportType === 'expected_profits') {
            document.querySelector('.date-input-group').style.display = 'none';
            document.getElementById('report-results').innerHTML = '';
        } else {
            document.querySelector('.date-input-group').style.display = '';
            document.getElementById('report-results').innerHTML = '';
        }

        $('.report-type-btn').removeClass('active');
        $(this).addClass('active');

        switch (currentReportType) {
            case 'profit':
                reportTitleElement.innerText = 'تقرير إجمالي الأرباح';
                break;
            case 'sales':
                reportTitleElement.innerText = 'تقرير إجمالي المبيعات';
                break;
            case 'most_sold':
                reportTitleElement.innerText = 'تقرير المنتج الأكثر مبيعًا';
                break;
            case 'least_sold':
                reportTitleElement.innerText = 'تقرير المنتج الأقل مبيعًا';
                break;
            case 'most_profitable_seller':
                reportTitleElement.innerText = 'تقرير البائع الأكثر ربحاً';
                break;
            case 'most_profitable_product':
                reportTitleElement.innerText = 'تقرير المنتج الأكثر ربحاً';
                break;
            case 'total_payments':
                reportTitleElement.innerText = 'تقرير عدد فواتير البيع';
                break;
            case 'sold_products':
                reportTitleElement.innerText = 'تقرير إجمالي مبلغ فواتير الشراء';
                break;
            case 'stock_money':
                reportTitleElement.innerText = 'تقرير إجمالي الأموال في المخزون';
                break;
            case 'expected_profits':
                reportTitleElement.innerText = 'تقرير إجمالي الأرباح المتوقعة';
                break;
            default:
                document.querySelector('.date-input-group').style.display = '';
                break;
        }
        reportDetailsSection.style.display = 'block';

        // Automatically generate report for stock_money and expected_profits
        if (currentReportType === 'stock_money' || currentReportType === 'expected_profits') {
            generateReport();
        }
    });

    // Add a new button for least sold product after the most sold product button
    if ($('.report-type-btn[data-report-type="least_sold"]').length === 0) {
        var mostSoldBtn = $('.report-type-btn[data-report-type="most_sold"]');
        if (mostSoldBtn.length) {
            var leastSoldBtn = mostSoldBtn.clone();
            leastSoldBtn.removeClass('active');
            leastSoldBtn.attr('data-report-type', 'least_sold');
            leastSoldBtn.text('المنتج الأقل مبيعًا');
            // Ensure the class is present and remove any inline style
            leastSoldBtn.addClass('report-type-btn');
            leastSoldBtn.removeAttr('style');
            mostSoldBtn.after(leastSoldBtn);
        }
    }
}

function generateReport() {
    const startDate = document.getElementById('reportStartDate').value;
    const endDate = document.getElementById('reportEndDate').value;
    const reportResultsDiv = document.getElementById('report-results');

    // Only require dates for reports that need them
    if (
        currentReportType !== 'stock_money' &&
        currentReportType !== 'expected_profits' &&
        (!startDate || !endDate)
    ) {
        Swal.fire({
            icon: 'warning',
            title: 'تنبيه',
            text: 'الرجاء تحديد تاريخي البدء والانتهاء',
            confirmButtonText: 'حسناً'
        });
        return;
    }

    if (
        currentReportType !== 'stock_money' &&
        currentReportType !== 'expected_profits' &&
        new Date(endDate) <= new Date(startDate)
    ) {
        Swal.fire({
            icon: 'error',
            title: 'خطأ في التواريخ',
            text: 'تاريخ الانتهاء يجب أن يكون أكبر من تاريخ البدء',
            confirmButtonText: 'حسناً'
        });
        return;
    }

    if (
        (currentReportType === 'stock_money' || currentReportType === 'expected_profits') ||
        (startDate && endDate)
    ) {
        let reportDataHtml = `
            <div class="report-content">
                <div class="report-header mb-4">
                    <h4 class="text-center mb-3">${document.getElementById('report-title').innerText}</h4>`;
        // Only show date range for date-based reports
        if (currentReportType !== 'stock_money' && currentReportType !== 'expected_profits') {
            reportDataHtml += `<p class="text-center text-muted">التقرير من <strong>${startDate}</strong> إلى <strong>${endDate}</strong></p>`;
        }
        reportDataHtml += `</div>`;

        reportResultsDiv.innerHTML = '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>';

        let url = '';
        switch (currentReportType) {
            case 'profit':
                url = `/Admin/Report/GetProfitReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'sales':
                url = `/Admin/Report/GetSalesReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'most_sold':
                url = `/Admin/Report/GetMostSoldProductsReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'least_sold':
                url = `/Admin/Report/GetLeastSoldProductsReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'most_profitable_seller':
                url = `/Admin/Report/GetMostProfitableSellerReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'most_profitable_product':
                url = `/Admin/Report/GetMostProfitableProductReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'total_payments':
                url = `/Admin/Report/GetTotalPaymentsReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'sold_products':
                url = `/Admin/Report/GetTotalPurchaseInvoicesReport?startDate=${startDate}&endDate=${endDate}`;
                break;
            case 'stock_money':
                url = `/Admin/Report/GetStockMoneyReport`;
                break;
            case 'expected_profits':
                url = `/Admin/Report/GetExpectedProfitsReport`;
                break;
        }

        $.ajax({
            url: url,
            type: 'GET',
            success: function(data) {
                // Check if data is empty or has no items
                let hasData = false;

                if (
                    currentReportType === 'profit' ||
                    currentReportType === 'sales' ||
                    currentReportType === 'total_payments' ||
                    currentReportType === 'sold_products' ||
                    currentReportType === 'stock_money' ||
                    currentReportType === 'expected_profits'
                ) {
                    const value =
                        currentReportType === 'profit' ? data.totalProfit :
                        currentReportType === 'sales' ? data.totalSales :
                        currentReportType === 'total_payments' ? data.totalSoldInvoices :
                        currentReportType === 'sold_products' ? data.totalPurchaseAmount :
                        currentReportType === 'stock_money' ? data.totalStockMoney :
                        data.totalExpectedProfits;

                    hasData = value > 0;
                } else {
                    // For list reports, check if array has items
                    hasData = Array.isArray(data) ? data.length > 0 : (data && data.length > 0);
                }
                
                if (!hasData) {
                    reportResultsDiv.innerHTML = `
                        <div class="alert alert-info text-center">
                            <i class="fas fa-info-circle fa-2x mb-3"></i>
                            <h5>لا توجد بيانات</h5>
                            <p>لا توجد بيانات متاحة للفترة المحددة.</p>
                        </div>`;
                    return;
                }
                
                switch (currentReportType) {
                    case 'profit':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-success">
                                    <h5 class="mb-0">إجمالي الأرباح للفترة: <strong> ${data.totalProfit} جنيه</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'sales':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-info">
                                    <h5 class="mb-0">إجمالي المبيعات للفترة: <strong>${data.totalSales} جنيه</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'most_sold':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped mt-3">
                                        <thead class="table-dark">
                                            <tr>
                                                <th>المنتج</th>
                                                <th>الكمية المباعة</th>
                                            </tr>
                                        </thead>
                                        <tbody>`;
                        data.forEach(p => {
                            reportDataHtml += `
                                <tr>
                                    <td>${p.productName}</td>
                                    <td>${p.totalQuantity}</td>
                                </tr>`;
                        });
                        reportDataHtml += `
                                        </tbody>
                                    </table>
                                </div>
                            </div>`;
                        break;
                    case 'least_sold':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped mt-3">
                                        <thead class="table-dark">
                                            <tr>
                                                <th>المنتج</th>
                                                <th>الكمية المباعة</th>
                                            </tr>
                                        </thead>
                                        <tbody>`;
                        data.forEach(p => {
                            reportDataHtml += `
                                <tr>
                                    <td>${p.productName}</td>
                                    <td>${p.totalQuantity}</td>
                                </tr>`;
                        });
                        reportDataHtml += `
                                        </tbody>
                                    </table>
                                </div>
                            </div>`;
                        break;
                    case 'most_profitable_seller':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped mt-3">
                                        <thead class="table-dark">
                                            <tr>
                                                <th>البائع</th>
                                                <th>إجمالي الربح</th>
                                            </tr>
                                        </thead>
                                        <tbody>`;
                        data.forEach(s => {
                            reportDataHtml += `
                                <tr>
                                    <td>${s.sellerName}</td>
                                    <td>${s.totalProfit} جنيه</td>
                                </tr>`;
                        });
                        reportDataHtml += `
                                        </tbody>
                                    </table>
                                </div>
                            </div>`;
                        break;
                    case 'most_profitable_product':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped mt-3">
                                        <thead class="table-dark">
                                            <tr>
                                                <th>المنتج</th>
                                                <th>إجمالي الربح</th>
                                            </tr>
                                        </thead>
                                        <tbody>`;
                        data.forEach(p => {
                            reportDataHtml += `
                                <tr>
                                    <td>${p.productName}</td>
                                    <td>${p.totalProfit} جنيه</td>
                                </tr>`;
                        });
                        reportDataHtml += `
                                        </tbody>
                                    </table>
                                </div>
                            </div>`;
                        break;
                    case 'total_payments':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-primary">
                                    <h5 class="mb-0">عدد فواتير البيع للفترة: <strong>${data.totalSoldInvoices} فاتورة</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'sold_products':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-success">
                                    <h5 class="mb-0">إجمالي مبلغ فواتير الشراء للفترة: <strong>${data.totalPurchaseAmount} جنيه</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'stock_money':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-success">
                                    <h5 class="mb-0">إجمالي الأموال في المخزون: <strong>${data.totalStockMoney} جنيه</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'expected_profits':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="alert alert-info">
                                    <h5 class="mb-0">إجمالي الأرباح المتوقعة: <strong>${data.totalExpectedProfits} جنيه</strong></h5>
                                </div>
                            </div>`;
                        break;
                }

                reportDataHtml += `</div>`;

                // Add report actions only if there is data
                const reportActionsTemplate = document.getElementById('report-actions-template');
                if (reportActionsTemplate) {
                    const reportActions = reportActionsTemplate.content.cloneNode(true);
                    reportResultsDiv.innerHTML = reportDataHtml;
                    reportResultsDiv.appendChild(reportActions);
                } else {
                    reportResultsDiv.innerHTML = reportDataHtml;
                    reportResultsDiv.innerHTML += `
                        <div class="report-actions mt-3 d-flex gap-2 justify-content-end">
                            <button class="btn btn-primary" onclick="downloadReportAsPDF()">
                                <i class="fas fa-download"></i> تحميل PDF
                            </button>
                            <button class="btn btn-secondary" onclick="printReport()">
                                <i class="fas fa-print"></i> طباعة
                            </button>
                        </div>`;
                }
            },
            error: function(xhr) {
                let message = "حدث خطأ أثناء تحميل التقرير. الرجاء المحاولة مرة أخرى.";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    message = xhr.responseJSON.message;
                }
                reportResultsDiv.innerHTML = `
                    <div class="alert alert-danger">
                        <i class="fas fa-exclamation-circle"></i>
                        ${message}
                    </div>`;
                Swal.fire({
                    icon: 'error',
                    title: 'خطأ',
                    text: message,
                    confirmButtonText: 'حسناً'
                });
            }
        });
    }
}

function downloadReportAsPDF() {
    // Get the entire report results area instead of just .report-content
    const reportResultsDiv = document.getElementById('report-results');
    const reportTitle = document.getElementById('report-title').innerText;
    const startDate = document.getElementById('reportStartDate').value;
    const endDate = document.getElementById('reportEndDate').value;

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

    // Add report header
    let pdfContent = `
        <div style="text-align: center; margin-bottom: 30px; border-bottom: 2px solid #333; padding-bottom: 20px;">
            <h1 style="color: #17a2b8; margin-bottom: 10px;">كافو</h1>
            <h2 style="color: #333; margin-bottom: 10px;">${reportTitle}</h2>
    `;

    // Add date range if available
    if (startDate && endDate) {
        pdfContent += `<p style="color: #666; margin: 0;">التقرير من <strong>${startDate}</strong> إلى <strong>${endDate}</strong></p>`;
    } else {
        pdfContent += `<p style="color: #666; margin: 0;">تقرير شامل</p>`;
    }

    pdfContent += `</div>`;

    // Clone the report content and clean it up for PDF
    const reportClone = reportResultsDiv.cloneNode(true);
    
    // Remove action buttons from PDF
    const actionButtons = reportClone.querySelectorAll('.report-actions, .btn');
    actionButtons.forEach(btn => btn.remove());

    // Convert Bootstrap classes to inline styles for PDF
    const alerts = reportClone.querySelectorAll('.alert');
    alerts.forEach(alert => {
        if (alert.classList.contains('alert-success')) {
            alert.style.cssText = 'background: #d4edda; color: #155724; padding: 15px; border-radius: 5px; margin: 10px 0; border-right: 4px solid #28a745;';
        } else if (alert.classList.contains('alert-info')) {
            alert.style.cssText = 'background: #d1ecf1; color: #0c5460; padding: 15px; border-radius: 5px; margin: 10px 0; border-right: 4px solid #17a2b8;';
        } else if (alert.classList.contains('alert-primary')) {
            alert.style.cssText = 'background: #cce5ff; color: #004085; padding: 15px; border-radius: 5px; margin: 10px 0; border-right: 4px solid #007bff;';
        } else if (alert.classList.contains('alert-danger')) {
            alert.style.cssText = 'background: #f8d7da; color: #721c24; padding: 15px; border-radius: 5px; margin: 10px 0; border-right: 4px solid #dc3545;';
        }
    });

    // Style tables for PDF
    const tables = reportClone.querySelectorAll('table');
    tables.forEach(table => {
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
    });

    // Add the cleaned content
    pdfContent += reportClone.innerHTML;

    // Add footer
    pdfContent += `
        <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #ddd; text-align: center; color: #666; font-size: 12px;">
            <p>تم إنشاء هذا التقرير في: ${new Date().toLocaleDateString('en-US')}</p>
        </div>
    `;

    pdfContainer.innerHTML = pdfContent;

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
        // Clean up
        document.body.removeChild(pdfContainer);
    }).catch(error => {
        console.error('PDF generation error:', error);
        // Clean up on error
        if (document.body.contains(pdfContainer)) {
            document.body.removeChild(pdfContainer);
        }
    });
}

function printReport() {
    window.print();
} 