
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
                    location.href = '/Home/MyTextPractice';
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
        const title = $("#titletext").val();
        const language = $("#languageSelect").val();
        const content = $("#contenttext").val();
        const isprivate = $("#checkprivate").is(":checked");

        if (title === "" || content == "") {
            ToastError("Tiêu đề và nội dung không được rỗng")
            return;
        }

        const wordCount = content.trim().split(/\s+/).length;
        var currentTimestamp = new Date().getTime();
        $.ajax({
            type: "post",
            url: "/TextPractice/CreateTextPractice",
            data: {
                title: title,
                language: language,
                content: content,
                isprivate: isprivate,
                createat: currentTimestamp,
                textlength: wordCount
            },
            success: function (response) {
                if (response.code === 200) {
                    ToastSuccess("Thành công", response.msg, true);
                } else {
                    ToastError(response.msg)
                }
            }, error: function (err) {

            }
        })
    })
})