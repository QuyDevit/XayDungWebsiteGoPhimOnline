$(document).ready(function () {
    $("#btn-owner").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#owner").toggle();

    });
    $("#btn-member").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#member").toggle();

    });

    $("#btnopen-code").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#setting-code").toggle();

    });
    $("#btnopen-pass").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#setting-pass").toggle();

    });
    $("#btnopen-img").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#setting-img").toggle();

    });
    $("#btnopen-description").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#setting-description").toggle();

    });
    $("#btnopen-status").click(function () {
        $(this).find(".fa").toggleClass("fa-chevron-right fa-chevron-down");
        $("#setting-status").toggle();

    });

    // copy pass room
    $("#copy-pass").click(function () {
        var copyText = $("#pass-room").val();
        navigator.clipboard.writeText(copyText).then(function () {
            console.log("Sao chép thành công!");
        }, function (err) {
            console.error("Sao chép thất bại!", err);
        });
    });
    // copy code room
    $("#copy-coderoom").click(function () {
        var copyText = $("#code-room").text();
        navigator.clipboard.writeText(copyText).then(function () {
            console.log("Sao chép thành công!");
        }, function (err) {
            console.error("Sao chép thất bại!", err);
        });
    });



    $(document).ready(function () {
        // Lấy tất cả các nút bằng class 'button__rank-background'
        const buttons = $(".button__tab-background");

        // Lấy tất cả các tab__pane-wrapper
        const panes = $(".tab__wrapper-content");

        // Xử lý sự kiện click cho mỗi nút
        buttons.click(function () {
            // Loại bỏ lớp 'show' từ tất cả các nút
            buttons.removeClass("show");

            // Thêm lớp 'show' cho nút được click
            $(this).addClass("show");

            // Ẩn tất cả các tab__pane-wrapper
            panes.hide();

            // Lấy ID của tab tương ứng với nút được click
            const tabId = $(this).data("tab");

            // Hiển thị tab__pane-wrapper có ID tương ứng
            $("#" + tabId).show();
        });
    });

});