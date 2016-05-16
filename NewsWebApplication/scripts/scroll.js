var page = 0;
var callBack = false;

function lastPostFunc() {
    if (page > -1 && !callBack) {
        callBack = true;
        page++;
        $('#divLoader').html('<img src="Content/Loader.gif" height="100px" width="166px">');
        $.get("/Home/News/" + page, function (data) {
            if (data != '') {
                $('#newsList').append(data);
            }
            else {
                page--;
            }
            callBack = false;
            $('#divLoader').empty();
        })
    }
}

$(window).scroll(function () {
    if ($(window).scrollTop() == $(document).height() - $(window).height()) {
        lastPostFunc();
    }
})