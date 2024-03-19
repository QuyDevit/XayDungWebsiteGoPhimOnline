
$(document).ready(function () {


    $(".nav__tab-competitions button").click(function () {
        $(".nav__tab-competitions button").removeClass("show")
        $(this).addClass("show");
        var tabToShow = $(this).data("tab");
        $(".container-competitions").hide();
        $("#" + tabToShow).show();
    });
    function ToastSuccess(title, content, joincode) {
        // Success Toast
        Swal.fire({
            icon: 'success',
            title: title,
            html: `<strong>${content}</strong>`,
            showCloseButton: true,
            showConfirmButton: true,
            confirmButtonText: 'OK',
            didClose: function () {
                // Sự kiện này sẽ được gọi khi Toast đóng
                if (joincode) {
                    location.href = '../Home/Competitions/' + joincode;
                }
            }
        });
    }
    function ToastError(content) {
        // Error Toast
        Swal.fire({
            icon: 'error',
            title: "Thông báo",
            html: `<strong>${content}</strong>`,
            showCloseButton: true,
            showConfirmButton: true,
            confirmButtonText: 'OK',
        });
    }

    $("#btncreatecompetition").click(function () {
        const isprivate = $("#checkprivate").prop('checked');
        const languageElements = $(".check-language__game-input");
        let getLanguage = ''; 

        languageElements.each(function () {
            if ($(this).prop('checked')) {
                getLanguage = $(this).val(); 
            }
        });

        if (getLanguage == '') {
            ToastError("Vui lòng chọn ngôn ngữ để tiếp tục!")
            return;
        }
        var currentTimestamp = new Date().getTime();
        $.ajax({
            type: 'post',
            url: '/CompetitionsTyping/HandleCreateCompetition',
            data: {
                isPrivate: isprivate,
                language: getLanguage,
                createdate: currentTimestamp,
                exerciseid : 4
            },
            success: function (response) {
                if (response.code == 200) {
                    var joincode = response.data;
                    ToastSuccess("Thành công!", response.msg, joincode);
                } else if (response.code == 400) {
                    ToastError(response.msg);
                } else {
                    ToastError(response.msg);
                }
            }, error: function (err) {

            }
        })
    });
})