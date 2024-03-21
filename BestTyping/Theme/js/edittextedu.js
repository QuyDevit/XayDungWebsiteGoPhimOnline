$(document).ready(function () {
    function ToastSuccess(title, content, shouldReload) {
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
                if (shouldReload) {
                    location.href = '/DashBoardEdu/MyTextTestEdu';
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

    $("#btn-createtext").click(function () {
        let title = $("#titletext").val();
        let language = $("#languageSelect").val();
        let content = $("#contenttext").val().trim().replaceAll(/\n+/g, " ");
        let isprivate = $("#checkprivate").is(":checked");

        if (title === "" || content == "") {
            ToastError("Tiêu đề và nội dung không được rỗng")
            return;
        }

        $.ajax({
            type: "post",
            url: "/DashBoardEdu/EditTextEduApi",
            data: {
                data: data,
                title: title,
                language: language,
                content: content,
                isprivate: isprivate
            },
            success: function (response) {
                if (response.code === 200) {
                    ToastSuccess("Thành công", response.msg, true);
                } else {
                    ToastError(response.msg)
                }
            }
        })
    })
})