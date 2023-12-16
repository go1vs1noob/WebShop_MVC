$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#companyTable').DataTable({
        "ajax": {
            url: '/admin/company/getall'
        },
        "columns": [
            { data: 'id', "width": "20%" },
            { data: 'name', "width": "20%" },
            { data: 'city', "width": "20%" },
            { data: 'phoneNumber', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group">
                        <a class="btn btn-outline-primary" href="company/edit/${data}">Edit</a>
                        <a class="btn btn-outline-secondary" href="company/delete/${data}">Delete</a>
                    </div>`;
                },
                "width": "20%"
            }
        ]
    });
}