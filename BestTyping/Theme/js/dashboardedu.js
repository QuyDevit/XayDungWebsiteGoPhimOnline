$(document).ready(function () {
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

    var arradd = [];
    var idRoom = 0;
    var userList = [];
    var $modal = $("#myModal");
    var $modalContent = $modal.find('.modal-content');

    var modaladduser = $("#modaladduser");
    var modaladduserContent = modaladduser.find('.modal-content');

    function resetinput() {
        $("#classname").val("");
        $("#classdescription").val("");
    }

    //$("#cancel").click(function () {
    //    setTimeout(function () {
    //        $("body").addClass("modal-open");
    //    },100)
    //    modaladduser.show();
    //    modaladduserContent.css('animation', 'zoomIn 0.6s');
    //    $("#add").css("opacity", 0.5).removeClass("open");
    //});

    // //Danh sách người dùng ban đầu
    //userList = [
    //    { name: "Qúy Nguyễn", email: "pingvocuc333@gmail.com" },
    //    { name: "Tuấn Nguyễn", email: "pingvocuc222@gmail.com" },
    //    { name: "Sang Nguyễn", email: "pingvocuc111@gmail.com" },
    //    { name: "Kiệt Nguyễn", email: "pingvocuc000@gmail.com" },
    //    { name: "Bình Nguyễn", email: "pingvocuc555@gmail.com" }
    //];

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



    $("#create-group").click(function () {
        $("body").addClass("modal-open");
        $("#acpect").css("opacity", 0.5).removeClass("open");
        $modal.show();
        $modalContent.css('animation', 'zoomIn 0.6s');

        let classname = $("#classname");
    

        classname.on("input", function () {
            const classNameValue = $(this).val().trim();
            // Kiểm tra nếu giá trị trường "#classname" không rỗng
            if (classNameValue !== "") {
                // Mở nút "#acpect" và thêm class "open"
                $("#acpect").prop("disabled", false).css("opacity",1).addClass("open");
            } else {
                // Khóa nút "#acpect" và loại bỏ class "open"
                $("#acpect").prop("disabled", true).css("opacity", 0.5).removeClass("open");
            }
        })

        $("#acpect").click(function () {
            let name = classname.val();
            let classdescription = $("#classdescription").val();
            let classisprivate = $("#classisprivate").val();

            if (name == "" || classisprivate == "") {
                ToastError("Tên không được để trống")
            }
            var isprivate = true;
            if (classisprivate == "1") {
                isprivate = false;
            }
            var currentTimestamp = new Date().getTime();
    
            $.ajax({
                type: 'post',
                url: "/DashBoardEdu/CreateClassRoom",
                data: {
                    name: name,
                    description: classdescription,
                    isprivate: isprivate,
                    createdate: currentTimestamp
                },
                success: function (response) {
                    if (response.code === 200) {
                        idRoom = response.roomID;
                        userList = response.list;
                        $("#cancel").trigger("click");
                        setTimeout(function () {
                            $("body").addClass("modal-open");
                        }, 100)
                        modaladduser.show();
                        modaladduserContent.css('animation', 'zoomIn 0.6s');
                        $("#add").click(function () {
                            $.ajax({
                                type: 'post',
                                url: '/DashBoardEdu/AddUserClassRoom',
                                data: {
                                    data: getEmailUserAdd(),
                                    roomid:idRoom
                                },
                                success: function (response) {
                                    if (response.code === 200) {
                                        userList = [];
                                        arradd = [];
                                        $("#list-after").empty();
                                        $("#skip").trigger("click");
                                    } else {
                                        console.log(response.msg);
                                    }
                                }, error: function (err) {

                                }
                            })
                        })

                    } else {
                        ToastError(response.msg);
                    }
                }, error: function (err) {
                    ToastError(err);
                }

            })
        })
    });


    function checkadduser() {
        var count = $("#list-after").find(".list-after_item").length;
        if (count != 0) {
            $("#add").prop("disabled", false).css("opacity", 1).addClass("open");
        } else {
            $("#add").prop("disabled", false).css("opacity",0.5).removeClass("open");
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

    $(".close, #cancel").click(function () {
        $("body").removeClass("modal-open");
        resetinput();
        $modalContent.css('animation', 'zoomOut 0.6s');
        setTimeout(function () {
            $modal.hide();
        }, 500);
    });

    $("#skip").click(function () {
        $("body").removeClass("modal-open");
        modaladduserContent.css('animation', 'zoomOut 0.6s');
        setTimeout(function () {
            modaladduser.hide();
        }, 500); 
    });

    $(window).click(function (event) {
        if ($(event.target).is($modal)) {
            $("body").removeClass("modal-open");
            resetinput();
            $modalContent.css('animation', 'zoomOut 0.6s');
            setTimeout(function () {
                $modal.hide();
            }, 500); 
        }
    });
});