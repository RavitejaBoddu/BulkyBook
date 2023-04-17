﻿var dataTable;

document.addEventListener("DOMContentLoaded", () => {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "12%" },
            { "data": "isbn", "width": "12%" },
            { "data": "price", "width": "10%" },
            { "data": "author", "width": "12%" },
            { "data": "category.name", "width": "12%" },
            { "data": "coverType.name", "width": "12%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="btn-group" role="group">
                            <a class="btn btn-primary mx-2" href="/Admin/Product/Upsert?id=${data}"><i class="bi bi-pencil-square"></i>Edit</a>
                            <a class="btn btn-danger mx-2" onClick="Delete('/Admin/Product/Delete/${data}')"><i class="bi bi-trash"></i>Delete</a>
                    </div>
                    `
                },
                "width": "12%",
            },
        ]
    });
};

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}