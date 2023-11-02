$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        "ajax": {
            url: '/admin/product/getall'
        },
        "columns": [
            { data: 'id', "width": "20%" },
            { data: 'name', "width": "20%" },
            { data: 'price', "width": "20%" },
            { data: 'category.name', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="btn-group">
                        <a class="btn btn-outline-primary" href="product/edit/${data}">Edit</a>
                        <a class="btn btn-outline-secondary" href="product/delete/${data}">Delete</a>
                    </div>`;
                },
                "width": "20%"
            }
        ]
    });
}