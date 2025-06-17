
function deleteCategory(id) {
    if (confirm("⚠ هل أنت متأكد من حذف هذه الفئة؟\n\nلا يمكن التراجع عن هذا الإجراء!")) {

        let url = `/Admin/CategoryManagement/Delete/${id}`;
        toggleSpinner(true, 'جاري الحذف...');
        $.ajax({
            url: url,
            type: 'GET', // Consider using 'DELETE' here if appropriate
            dataType: 'json', // Expect JSON response
            success: function (result) {
                if (result.success) {

                    loadAdminContent("Categories", 1);
                    toastr.success("تم حذف الفئة بنجاح");
                    toggleSpinner(false);
                } else {
                    toastr.success("لم يتم الحذف لقد حدث خطأ");
                }
            },
            error: function (error) {
                toastr.success('حدث خطأ في الاتصال بالخادم');
            }
        });
    }
}
