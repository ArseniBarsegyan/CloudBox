$(function () {
    $.ajaxSetup({ cache: false });
    //click on login button at Home/Index will show modal box
    $("#btnlogin").click(function (e) {
        e.preventDefault();
        $.get(this.href,
            function (data) {
                $("#dialogContent").html(data);
                $("#modDialog").modal("show");
            });
    });

    $("#loginLink").click(function (e) {
        e.preventDefault();
        $.get(this.href,
            function (data) {
                $("#dialogContent").html(data);
                $("#modDialog").modal("show");
            });
    });
})