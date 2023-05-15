$(document).ready(function () {
    $("#lnkLogout").on("click", function () {
        $("#Logout").submit();
    });
});
function CallActions(Url, Types, data, success, error, async) {

    $.ajax({

        url: Url,
        type: Types,
       
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        datatype: "json",
        async: async,
        success: success,
        error: error,
    });
}
