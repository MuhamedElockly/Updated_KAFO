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

        if (currentReportType === 'profit') {
            const profit = (Math.random() * 5000 + 1000).toFixed(2);
            reportDataHtml += `
                <div class="report-body">
                    <div class="alert alert-success">
                        <h5 class="mb-0">إجمالي الأرباح للفترة: <strong>${profit} ريال</strong></h5>
                    </div>
                </div>`;
        } else if (currentReportType === 'sales') {
            const sales = (Math.random() * 10000 + 2000).toFixed(2);
            reportDataHtml += `
                <div class="report-body">
                    <div class="alert alert-info">
                        <h5 class="mb-0">إجمالي المبيعات للفترة: <strong>${sales} ريال</strong></h5>
                    </div>
                </div>`;
        } else if (currentReportType === 'most_sold') {
            const products = [
                { name: "منتج أ", quantity: Math.floor(Math.random() * 100) + 10 },
                { name: "منتج ب", quantity: Math.floor(Math.random() * 100) + 10 },
                { name: "منتج ج", quantity: Math.floor(Math.random() * 100) + 10 }
            ];
            products.sort((a, b) => b.quantity - a.quantity);

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
            products.forEach(p => {
                reportDataHtml += `
                                <tr>
                                    <td>${p.name}</td>
                                    <td>${p.quantity}</td>
                                </tr>`;
            });
            reportDataHtml += `
                            </tbody>
                        </table>
                    </div>
                </div>`;
        } else if (currentReportType === 'most_profitable_seller') {
            const sellers = [
                { name: "البائع أ", profit: (Math.random() * 1000 + 500).toFixed(2) },
                { name: "البائع ب", profit: (Math.random() * 1000 + 500).toFixed(2) },
                { name: "البائع ج", profit: (Math.random() * 1000 + 500).toFixed(2) }
            ];
            sellers.sort((a, b) => b.profit - a.profit);

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
            sellers.forEach(s => {
                reportDataHtml += `
                                <tr>
                                    <td>${s.name}</td>
                                    <td>${s.profit} ريال</td>
                                </tr>`;
            });
            reportDataHtml += `
                            </tbody>
                        </table>
                    </div>
                </div>`;
        } else if (currentReportType === 'most_profitable_product') {
            const products = [
                { name: "منتج X", profit: (Math.random() * 500 + 100).toFixed(2) },
                { name: "منتج Y", profit: (Math.random() * 500 + 100).toFixed(2) },
                { name: "منتج Z", profit: (Math.random() * 500 + 100).toFixed(2) }
            ];
            products.sort((a, b) => b.profit - a.profit);

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
            products.forEach(p => {
                reportDataHtml += `
                                <tr>
                                    <td>${p.name}</td>
                                    <td>${p.profit} ريال</td>
                                </tr>`;
            });
            reportDataHtml += `
                            </tbody>
                        </table>
                    </div>
                </div>`;
        } else if (currentReportType === 'total_payments') {
            const paymentCount = Math.floor(Math.random() * 1000 + 100);
            reportDataHtml += `
                <div class="report-body">
                    <div class="alert alert-primary">
                        <h5 class="mb-0">عدد المدفوعات الكلي للفترة: <strong>${paymentCount} دفعة</strong></h5>
                    </div>
                </div>`;
        } else if (currentReportType === 'sold_products') {
            const soldProducts = [
                { id: 101, name: "لابتوب ديل", price: 2500, quantity: 1, date: "2024-05-20" },
                { id: 102, name: "هاتف سامسونج", price: 1800, quantity: 1, date: "2024-05-21" },
                { id: 103, name: "سماعات بلوتوث", price: 250, quantity: 1, date: "2024-05-21" },
                { id: 104, name: "شاحن سريع", price: 120, quantity: 1, date: "2024-05-22" }
            ];
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
            soldProducts.forEach(p => {
                reportDataHtml += `
                                <tr>
                                    <td>${p.id}</td>
                                    <td>${p.name}</td>
                                    <td>${p.price} ريال</td>
                                    <td>${p.quantity}</td>
                                    <td>${p.date}</td>
                                </tr>`;
            });
            reportDataHtml += `
                            </tbody>
                        </table>
                    </div>
                </div>`;
        }

        reportDataHtml += `</div>`;

        // Add report actions
        const reportActionsTemplate = document.getElementById('report-actions-template');
        if (reportActionsTemplate) {
            const reportActions = reportActionsTemplate.content.cloneNode(true);
            reportResultsDiv.innerHTML = reportDataHtml;
            reportResultsDiv.appendChild(reportActions);
        } else {
            // If template is not found, just add the report content
            reportResultsDiv.innerHTML = reportDataHtml;
            // Add buttons directly
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

        alert(`جارٍ إنشاء تقرير ${currentReportType} من ${startDate} إلى ${endDate}`);
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