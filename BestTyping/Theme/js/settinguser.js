import { initializeApp } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-app.js";
import { getStorage, ref as sRef, uploadBytesResumable, getDownloadURL  } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-storage.js";
const firebaseConfig = {
    apiKey: "AIzaSyA4NUDSuzDBU44h2KIxBLu4Mxgk1Zu3se0",
    authDomain: "login-besttyping.firebaseapp.com",
    projectId: "login-besttyping",
    storageBucket: "login-besttyping.appspot.com",
    messagingSenderId: "958669352120",
    appId: "1:958669352120:web:6d148aa63f3a5317a1d84d"
};

const app = initializeApp(firebaseConfig);
const storage = getStorage();

$(document).ready(function () {
    $('#table-rank').DataTable();

    $("#btnsavefile").click(function () {
        const file = $("#userAvatarInput")[0].files[0];
        if (file) {
            const storageRef = sRef(storage, "Avatars/" + file.name);
            const uploadTask = uploadBytesResumable(storageRef, file);
            uploadTask.on('state_changed',
                (snapshot) => {                                      
                },
                (error) => {
                },
                () => {
                    getDownloadURL(uploadTask.snapshot.ref).then((downloadURL) => {
                        $.ajax({
                            type: "post",
                            url: "/Account/SaveAvatar",
                            data: {
                                url: downloadURL
                            },
                            success: function (response) {
                                if (response.code == 200) {
                                    toastr.success(response.msg);
                                    setTimeout(function () {
                                        location.reload();
                                    }, 2000);
                                } else if (response.code == 400) {
                                    toastr.error(response.msg);
                                } else {
                                    toastr.warning(response.msg);
                                }
                            }, error: function (err) {
                                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
                            }
                        })
                    });
                }
            );
        } else {
            toastr.error("Vui lòng chọn file ảnh!");
        }
    });

    // Bắt sự kiện khi chọn file ảnh
    $("#userAvatarInput").change(function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $("#avataractive").attr("src", e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });


    $(".nav__tab-setting button").click(function () {
        $(".nav__tab-setting button").removeClass("show")
        $(this).addClass("show");
        var tabToShow = $(this).data("tab");
        $(".container-settings").hide();
        $("#" + tabToShow).show();
    });


    $(".user-avatar i").click(function () {
        $("#userAvatarInput").click();
    });


    $("#onpenchangepass").click(function () {
        $("#changepass").slideToggle();
    })

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

    //LƯU THÔNG TIN
    $("#btnsaveinfo").click(function () {
        const name = $("#newname").val();
        const introduce = $("#introduce").val();

        $.ajax({
            type: "post",
            url: "/Account/ChangeInfo",
            data: {
                name: name,
                introduce: introduce
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    setTimeout(function () {
                        location.reload();
                    }, 2000); 
                } else if (response.code == 400) {
                    toastr.error(response.msg); 
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!"); 
            }
        })
    });

    //LƯU MẬT KHẨU MỚI
    $("#btnsavechangepass").click(function () {
        const oldpass = $("#oldpass").val();
        const newpass = $("#newpass").val();
        const renewpass = $("#renewpass").val();

        if (oldpass == "" || newpass == "" || renewpass == "") {
            toastr.error("Vui lòng nhập mật khẩu đầy đủ");
            return;
        }
        // Kiểm tra mật khẩu đủ mạnh
        // Mật khẩu cần ít nhất 6 ký tự, gồm chữ và số
        var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
        if (!passwordPattern.test(newpass)) {
            toastr.error("Mật khẩu cần ít nhất 6 ký tự và phải bao gồm cả chữ và số");
            return;
        }

        if (newpass != renewpass) {
            toastr.error("Mật khẩu không khớp");
            return;
        }


        $.ajax({
            type: "post",
            url: "/Account/ChangePassInfo",
            data: {
                oldpass: oldpass,
                newpass: newpass
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg); 
                }  else {
                    toastr.error(response.msg); 
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!"); 
            }
        })
    });

    var checkAll = $('#check-all');
    var checkboxes = $('input[type="checkbox"]', '#table-rank tbody');

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

    $("#btn-deleteresult").click(function () {
        var arrvalue = [];
        checkboxes.filter(':checked').each(function () {
            arrvalue.push($(this).val());
        })

        // Sử dụng SweetAlert2 để hiện thị hộp thoại xác nhận
        Swal.fire({
            title: 'Bạn có chắc không?',
            text: "Bạn sẽ không thể hồi phục tài nguyên này!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText:"Hủy",
            confirmButtonText: 'Đồng ý'
        }).then((result) => {
            if (result.isConfirmed) {      
                $.ajax({
                    type: "post",
                    url: "/Account/DeleteTypingResult",
                    data: {
                        data: arrvalue
                    },
                    success: function (response) {
                        if (response.code == 200) {
                            toastr.success(response.msg);
                            setTimeout(function () {
                                location.reload();
                            }, 2000);
                        }  else {
                            toastr.error(response.msg);
                        }
                    }, error: function (err) {
                        toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
                    }
                })
            }
        });
    })
});