$(document).ready(function () {
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

    $(".operations-delete").click(function (e) {
        e.preventDefault();
        var dataid = $(this).data("id");

        Swal.fire({
            title: 'Bạn có chắc không?',
            text: "Bạn sẽ không thể hồi phục tài nguyên này!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Hủy",
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "post",
                    url: "/DashBoardEdu/DeleteTextEdu",
                    data: {
                        data: dataid
                    },
                    success: function (response) {
                        if (response.code == 200) {
                            toastr.success(response.msg);
                            setTimeout(function () {
                                location.reload();
                            }, 2000);
                        } else {
                            toastr.error(response.msg);
                        }
                    }, error: function (err) {
                        toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
                    }
                })
            }
        });
    })
})