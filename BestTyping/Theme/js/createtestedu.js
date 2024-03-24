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
                location.href = '/DashBoardEdu/MyTestEdu';
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
toastr.options = {
    "closeButton": true, // Hiển thị nút đóng
    "debug": false,
    "newestOnTop": false,
    "progressBar": true, // Hiển thị thanh tiến trình
    "positionClass": "toast-top-right", // Vị trí
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "2000", // Thời gian tự đóng (ms)
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

var timestart = 0;
var timeend = 0;
flatpickr("#datepickerstart", {
    enableTime: true,
    dateFormat: "Y-m-d H:i",
    time_24hr: true,
    onChange: function (selectedDates) {
        // Lấy timestamp dạng số mili giây
        timestart = selectedDates[0].getTime();

        console.log(timestart);
    }
});
flatpickr("#datepickerend", {
    enableTime: true,
    dateFormat: "Y-m-d H:i",
    time_24hr: true,
    onChange: function (selectedDates) {
        // Lấy timestamp dạng số mili giây
        timeend = selectedDates[0].getTime();

        console.log(timeend);
    }
});


var $modal = $("#modalchooseclass");
var $modalContent = $modal.find('.modal-content');

$("#choose-class").click(function () {
    var currentTimestamp = new Date().getTime();
    console.log(currentTimestamp);
    $modal.show();
    $modalContent.css('animation', 'zoomIn 0.6s');
})

$(".close, #cancel").click(function () {
    $("body").removeClass("modal-open");
    $modalContent.css('animation', 'zoomOut 0.6s');
    setTimeout(function () {
        $modal.hide();
    }, 500);
});

var checkAll = $('#check-all');
var checkboxes = $('input[type="checkbox"]', '#table-class tbody');

// Check all child checkboxes when "check-all" is clicked
checkAll.click(function () {
    var isChecked = $(this).is(':checked');
    checkboxes.prop('checked', isChecked);
});

// Update "check-all" state based on child checkboxes
checkboxes.change(function () {
    var allChecked = checkboxes.length === checkboxes.filter(':checked').length;
    checkAll.prop('checked', allChecked);
});

var arrclass = [];
$("#confirm").click(function () {
    arrclass = [];
    checkboxes.filter(':checked').each(function () {
        arrclass.push($(this).val());
    })
    console.log(arrclass);
    $(".close").trigger("click");
})

//Tạo bài kiểm tra

$("#create-test").click(function () {
    let text = $("#text-test").val();
    let checkrandom = $("#checkrandom").is("checked");
    let titletest = $("#title-test").val();
    let timeduration = $("#time-duration").val();
    let maxattempts = $("#max-attempts").val();
    let passtest = $("#pass-test").val();


    if (text == "" || titletest == "" || timeduration == "" || maxattempts == "" || passtest == "" || arrclass.length == 0 || timestart == 0 || timeend == 0) {
        toastr.warning("Vui lòng nhập đầy đủ thông tin");
        return;
    }
    if (maxattempts <= 0) {
        toastr.warning("Số lần làm bài không được <= 0");
        return;
    }

    var currentTimestamp = new Date().getTime();
    $.ajax({
        type: 'post',
        url: '/DashBoardEdu/CreateTestEdu',
        data: {
            textid: text,
            random: checkrandom,
            titletest: titletest,
            arrlist: arrclass,
            timestart: timestart,
            timeend: timeend,
            examduration: timeduration,
            pass: passtest,
            maxattempts: maxattempts,
            createdate: currentTimestamp
        },
        success: function (response) {
            if (response.code == 200) {
                ToastSuccess("Thành công", response.msg, false);
            } else {
                ToastError(response.msg);
            }
        },
        error: function (err) {

        }

    })
})