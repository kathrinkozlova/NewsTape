﻿$('#submit').on('click', function (e) {
    e.preventDefault();
    var files = document.getElementById('upload').files;
    if (files.length > 0) {
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }

            $.ajax({
                type: "POST",
                url: '@Url.Action("Create", "News")',
                contentType: false,
                processData: false,
                data: data,
                success: function (result) {
                    alert(result);
                },
                error: function (xhr, status, p3) {
                    alert(xhr.responseText);
                }
            });
        } else {
            alert("Браузер не поддерживает загрузку файлов HTML5!");
        }
    }
});