import { initializeApp } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-app.js";
import { getStorage, ref as sRef, uploadBytesResumable, getDownloadURL } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-storage.js";
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
    //Lưu ảnh nhóm
    $("#btnsave-img").click(function () {
        const file = $("#room-avatar")[0].files[0];
        if (file) {
            const storageRef = sRef(storage, "Group/" + file.name);
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
                            url: "/DashBoardEdu/SaveAvatarGroup",
                            data: {
                                data: idRoom,
                                url: downloadURL
                            },
                            success: function (response) {
                                if (response.code == 200) {
                                    toastr.success(response.msg);
                                } else  {
                                    toastr.error(response.msg);
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

    $("#btn-openfile").click(function () {
        $("#room-avatar").click();
    });

    // Bắt sự kiện khi chọn file ảnh
    $("#room-avatar").change(function () {
        const file = this.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $("#img-room").attr("src", e.target.result);
            }
            reader.readAsDataURL(file);
        }
    });

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

    //Lưu mô tả nhóm
    $("#create-description").click(function () {
        let description = $("#description-room").val();
        $.ajax({
            type: "post",
            url: "/DashBoardEdu/SaveDescriptionGroup",
            data: {
                data: idRoom,
                description: description
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                }else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })
    //Thay đổi status
    $("#change-status").click(function () {
        $("#status-input").click();
        var span = $("#status-room");
        var isprivate = $("#status-input").is(":checked");
        $.ajax({
            type: "post",
            url: "/DashBoardEdu/ChangeStatusRoom",
            data: {
                data: idRoom,
                status: isprivate
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    if (isprivate == true) {
                        span.removeClass("public");
                        span.text("Riêng tư");
                        span.addClass("private");
                    } else {
                        span.removeClass("private");
                        span.text("Công khai");
                        span.addClass("public");
                    }
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })

    //Tạo mã code
    $("#create-code").click(function () {
        $(this).empty();
        var loadspiner = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`;
        $(this).append(loadspiner);
        setTimeout(function () {
            $.ajax({
                type: "post",
                url: "/DashBoardEdu/CreateCodeRoom",
                data: {
                    data: idRoom,
                },
                success: function (response) {
                    if (response.code == 200) {
                        toastr.success(response.msg);
                        $("#create-code").prop("disabled", true).hide();
                        $("#code-room").text(response.data);
                        $(".render-code").show();
                    } else {
                        toastr.warning(response.msg);
                    }
                }, error: function (err) {
                    toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
                }
            })
        },1000)
    })
    //repeat code room
    $("#repeat-code").click(function () {
        $.ajax({
            type: "post",
            url: "/DashBoardEdu/CreateCodeRoom",
            data: {
                data: idRoom,
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    $("#create-code").prop("disabled", true).hide();
                    $("#code-room").text(response.data);
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })
    //Remove code room
    $("#remove-code").click(function () {
        $.ajax({
            type: "post",
            url: "/DashBoardEdu/DeleteCodeRoom",
            data: {
                data: idRoom,
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    $("#create-code").text("Tạo mã").prop("disabled", false).show();
                    $("#code-room").text("");
                    $(".render-code").hide();
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })

    //Create pass room
    $("#create-pass").click(function () {
        var passroom = $("#pass-room").val();

        var passroomPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
        if (!passroomPattern.test(passroom)) {
            toastr.error("Mật khẩu cần ít nhất 6 ký tự và phải bao gồm cả chữ và số");
            return;
        }

        $.ajax({
            type: "post",
            url: "/DashBoardEdu/SavePassRoom",
            data: {
                data: idRoom,
                pass: passroom,
                check : 1
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    $("#delete-pass").show();
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })

    //Delete pass room
    $("#delete-pass").click(function () {
        var passroom = $("#pass-room").val();
        $.ajax({
            type: "post",
            url: "/DashBoardEdu/SavePassRoom",
            data: {
                data: idRoom,
                pass: passroom,
                check: 0
            },
            success: function (response) {
                if (response.code == 200) {
                    toastr.success(response.msg);
                    $("#delete-pass").hide();
                    $("#pass-room").val("");
                } else {
                    toastr.warning(response.msg);
                }
            }, error: function (err) {
                toastr.error("Có lỗi xảy ra khi gửi yêu cầu!");
            }
        })
    })

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
        $("#status-room").toggle();
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

    // full screen code
    $("#fullscreen-code").click(function () {
        $(".full-screen").show();
        $(".full-screen_wrapper span").text($("#code-room").text());
        $(".full-screen_wrapper i").click(function () {
            $(".full-screen").hide();
        })
    })


    //Hủy duyệt
    $(document).on("click", ".delete-join", function () {
        var data = $(this).data("delete");

        var row = $(this).closest(".request-row");
        row.remove();

         $.ajax({
            type: 'post',
             url: '/DashBoardStudent/DeleteRequestJoin',
            data: {
                data: data,
                roomid: idRoom
            },
            success: function (response) {
                if (response.code === 200) {
                    toastr.success(response.msg);
                } else {
                    console.log(response.msg);
                }
            }, error: function (err) {

            }
        })
    });
    //Thêm duyệt
    $(document).on("click", ".accept-join", function () {
        var data = $(this).data("add");

        var row = $(this).closest(".request-row");
        row.remove();

        $.ajax({
            type: 'post',
            url: '/DashBoardStudent/AcceptRequestJoin',
            data: {
                data: data,
                roomid: idRoom
            },
            success: function (response) {
                if (response.code === 200) {
                    toastr.success(response.msg);
                } else {
                    console.log(response.msg);
                }
            }, error: function (err) {

            }
        })
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
    var arradd = [];
    var userList = [];
    var modaladduser = $("#modaladduser");
    var modaladduserContent = modaladduser.find('.modal-content');

    $("#add-user").click(function () {
        $("#add").prop("disabled", true).css("opacity", 0.5).removeClass("open");
        userList = datalist;
        modaladduser.show();
        modaladduserContent.css('animation', 'zoomIn 0.6s');

        $("#add").click(function () {
            $.ajax({
                type: 'post',
                url: '/DashBoardEdu/AddUserClassRoom',
                data: {
                    data: getEmailUserAdd(),
                    roomid: idRoom
                },
                success: function (response) {
                    if (response.code === 200) {
                        $("#skip").trigger("click");
                        setTimeout(function () {
                            location.reload();
                        }, 700)
                    } else {
                        console.log(response.msg);
                    }
                }, error: function (err) {

                }
            })
        })
    })


    $("#skip").click(function () {
        $("body").removeClass("modal-open");
        modaladduserContent.css('animation', 'zoomOut 0.6s');
        userList = [];
        arradd = [];
        $("#list-user").empty();
        $("#list-after").empty();
        setTimeout(function () {
            modaladduser.hide();
        }, 500);     
    });

    // Lắng nghe sự kiện "input" trên ô input
    $("#emailfilter").on("input", function () {
        var filterValue = $(this).val().toLowerCase(); // Lấy giá trị nhập vào và chuyển thành chữ thường
        var filteredUsers = userList.filter(function (user) {
            return user.email.toLowerCase().includes(filterValue); // Lọc danh sách người dùng dựa trên email
        });
        displayFilteredUsers(filteredUsers); // Hiển thị danh sách người dùng đã lọc
    });

    // Hàm hiển thị danh sách người dùng đã lọc
    function displayFilteredUsers(filteredUsers) {
        var $listUser = $("#list-user");
        $listUser.empty(); // Xóa danh sách người dùng hiện tại

        // Duyệt qua danh sách người dùng đã lọc và thêm vào danh sách hiển thị
        filteredUsers.forEach(function (user) {
            var listItem = `<li class="list-user_item form-control" data-email="${user.email}" data-name="${user.name}">
                            <span>${user.name}</span>
                            <span>${user.email}</span>
                        </li>`;
            $listUser.append(listItem);
        });
    }

    function bindClickEventToUserItems() {
        // Sử dụng delegated events
        $("#list-user").on('click', '.list-user_item', function () {
            var name = $(this).data("name");
            var email = $(this).data("email");

            var listItemAfter = `<li class="list-after_item form-control" data-email="${email}" data-name="${name}">
                                <span>${name}</span>
                                <span>${email}</span>
                                <span class="remove">&times;</span>
                            </li>`;
            $("#list-after").append(listItemAfter);
            checkadduser();

            userList = userList.filter(function (user) {
                return user.email !== email;
            });

            $(this).remove();
            // Lưu ý: bindRemoveEventToListAfterItems không cần gọi ở đây vì nó đã được gọi một lần và áp dụng cho tất cả các phần tử future
        });
    }

    // Đảm bảo bạn chỉ gọi bindRemoveEventToListAfterItems() một lần, không cần gọi lại nhiều lần
    bindRemoveEventToListAfterItems();


    // Gắn sự kiện remove cho list-after_item
    function bindRemoveEventToListAfterItems() {
        $("#list-after").on('click', '.remove', function () {
            var $parentItem = $(this).closest('.list-after_item');
            var name = $parentItem.data("name");
            var email = $parentItem.data("email");

            // Thêm người dùng trở lại vào userList
            userList.push({ name: name, email: email });

            // Xóa phần tử từ UI
            $parentItem.remove();
            checkadduser();

            // (Tùy chọn) Nếu bạn muốn, sau khi thêm người dùng trở lại vào userList,
            // bạn cũng có thể cập nhật lại danh sách người dùng hiển thị dựa trên bộ lọc hiện tại
            // Điều này giúp đảm bảo rằng danh sách người dùng được hiển thị luôn đồng bộ với dữ liệu
            // Lưu ý: Bạn cần có một hàm để cập nhật danh sách dựa trên bộ lọc hiện tại
            var filterValue = $("#emailfilter").val().toLowerCase();
            var filteredUsers = userList.filter(function (user) {
                return user.email.toLowerCase().includes(filterValue);
            });
            displayFilteredUsers(filteredUsers); // Giả sử bạn đã có hàm này từ trước
        });
    }
    bindClickEventToUserItems(); // Gắn sự kiện click cho list-user_item

    function checkadduser() {
        var count = $("#list-after").find(".list-after_item").length;
        if (count != 0) {
            $("#add").prop("disabled", false).css("opacity", 1).addClass("open");
        } else {
            $("#add").prop("disabled", true).css("opacity", 0.5).removeClass("open");
        }
    }

    function getEmailUserAdd() {
        var arrafter = $(".list-after_item"); // Lấy tất cả các phần tử có class "list-after_item"

        arrafter.each(function () {
            // Trong context này, "this" đại diện cho mỗi phần tử "list-after_item" đang được xét
            var email = $(this).data("email"); // Lấy giá trị data-email của phần tử
            arradd.push(email); // Thêm email vào mảng arrnew
        });
        return arradd;
    }
});