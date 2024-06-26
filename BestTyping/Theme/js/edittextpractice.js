﻿$(document).ready(function () {
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
        let title = $("#titletext").val();
        let language = $("#languageSelect").val();
        let content = $("#contenttext").val();
        let isprivate = $("#checkprivate").is(":checked");

        if (title === "" || content == "") {
            ToastError("Tiêu đề và nội dung không được rỗng")
            return;
        }

        const wordCount = content.trim().split(/\s+/).length;
        $.ajax({
            type: "post",
            url: "/TextPractice/EditTextPracticeApi",
            data: {
                codejoin: codejoin,
                title: title,
                language: language,
                content: content,
                isprivate: isprivate,
                textlength: wordCount
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