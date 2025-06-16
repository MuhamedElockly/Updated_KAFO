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
            case 'most_profitable_seller':
                reportTitleElement.innerText = 'تقرير البائع الأكثر ربحاً';
                break;
            case 'most_profitable_product':
                reportTitleElement.innerText = 'تقرير المنتج الأكثر ربحاً';
                break;
            case 'total_payments':
                reportTitleElement.innerText = 'تقرير عدد المدفوعات الكلي';
                break;
            case 'sold_products':
                reportTitleElement.innerText = 'تقرير المنتجات المباعة';
                break;
        }
        reportDetailsSection.style.display = 'block';
        document.getElementById('report-results').innerHTML = '<p class="text-muted">الرجاء تحديد نطاق تاريخ لعرض التقرير.</p>';
    });
}

function generateReport() {
    const startDate = document.getElementById('reportStartDate').value;
    const endDate = document.getElementById('reportEndDate').value;
    const reportResultsDiv = document.getElementById('report-results');

    if (startDate && endDate) {
        let reportDataHtml = `
            <div class="report-content">
                <div class="report-header mb-4">
                    <h4 class="text-center mb-3">${document.getElementById('report-title').innerText}</h4>
                    <p class="text-center text-muted">التقرير من <strong>${startDate}</strong> إلى <strong>${endDate}</strong></p>
                </div>`;

        // Show loading indicator
        reportResultsDiv.innerHTML = '<div class="text-center"><i class="fas fa-spinner fa-spin fa-2x"></i></div>';

        // Make API call based on report type
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
                url = `/Admin/Report/GetSoldProductsReport?startDate=${startDate}&endDate=${endDate}`;
                break;
        }

        $.ajax({
            url: url,
            type: 'GET',
            success: function(data) {
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
                                    <h5 class="mb-0">عدد المدفوعات الكلي للفترة: <strong>${data.totalPayments} دفعة</strong></h5>
                                </div>
                            </div>`;
                        break;
                    case 'sold_products':
                        reportDataHtml += `
                            <div class="report-body">
                                <div class="table-responsive">
                                    <table class="table table-bordered table-striped mt-3">
                                        <thead class="table-dark">
                                            <tr>
                                                <th>رقم المنتج</th>
                                                <th>اسم المنتج</th>
                                                <th>السعر</th>
                                                <th>الكمية المباعة</th>
                                                <th>تاريخ البيع</th>
                                            </tr>
                                        </thead>
                                        <tbody>`;
                        data.forEach(p => {
                            reportDataHtml += `
                                <tr>
                                    <td>${p.productId}</td>
                                    <td>${p.productName}</td>
                                    <td>${p.price} جنيه</td>
                                    <td>${p.quantity}</td>
                                    <td>${new Date(p.date).toLocaleDateString()}</td>
                                </tr>`;
                        });
                        reportDataHtml += `
                                        </tbody>
                                    </table>
                                </div>
                            </div>`;
                        break;
                }

                reportDataHtml += `</div>`;

                // Add report actions
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
            error: function(error) {
                reportResultsDiv.innerHTML = `
                    <div class="alert alert-danger">
                        <i class="fas fa-exclamation-circle"></i>
                        حدث خطأ أثناء تحميل التقرير. الرجاء المحاولة مرة أخرى.
                    </div>`;
            }
        });
    } else {
        alert('الرجاء تحديد تاريخي البدء والانتهاء.');
    }
}

function downloadReportAsPDF() {
    const reportContent = document.querySelector('.report-content');
    const reportTitle = document.getElementById('report-title').innerText;

    const opt = {
        margin: 1,
        filename: `${reportTitle}-${new Date().toISOString().split('T')[0]}.pdf`,
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };

    html2pdf().set(opt).from(reportContent).save();
}

function printReport() {
    window.print();
} 