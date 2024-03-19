

$(document).ready(function() {
    function hideDropdownMenu(dropdownMenu) {
        dropdownMenu.removeClass("show");
    }

    function isOutsideClick(target, element) {
        return !element.is(target) && element.has(target).length === 0;
    }

    const chooseMenuAccount = $(".nav-account");
    const dropdownMenuAccount = $(".dropdown-menu-account");
    const chooseLanguage = $(".nav-lang_dropdown");
    const dropdownMenu = $(".navbar .dropdown-menu");

    chooseMenuAccount.click(function (e) {
        if (dropdownMenuAccount.hasClass("show")) {
            hideDropdownMenu(dropdownMenuAccount);
        } else {
            hideDropdownMenu(dropdownMenu); 
            dropdownMenuAccount.addClass("show");
        }
        e.stopPropagation();
    });

    chooseMenuAccount.find("img, span, i").click(function (e) {
        chooseMenuAccount.trigger("click"); 
        e.stopPropagation();
    });

    chooseLanguage.click(function (e) {
        if (dropdownMenu.hasClass("show")) {
            hideDropdownMenu(dropdownMenu);
        } else {
            hideDropdownMenu(dropdownMenuAccount); 
            dropdownMenu.addClass("show");
        }
        e.stopPropagation();
    });

    chooseLanguage.find(".nav-lang__select-text, .icon_down-menu").click(function (e) {
        chooseLanguage.trigger("click"); 
        e.stopPropagation();
    });

    $(document).click(function (e) {
        if (isOutsideClick(e.target, chooseMenuAccount)) {
            hideDropdownMenu(dropdownMenuAccount);
        }

        if (isOutsideClick(e.target, chooseLanguage)) {
            hideDropdownMenu(dropdownMenu);
        }
    });

    $("#logout").click(function () {
        $.ajax({
            type: "POST",
            url: "/Account/Logout",
            dataType: "json", 
            contentType: "application/json; charset=utf-8",  
            success: function (response) {
                if (response.code == 200) {
                    location.reload();
                } else {
                    alert("lỗi");
                }
            },
            error: function (xhr, status, error) {
                console.error("AJAX Error:", status, error);
            }
        });
    });


    $("#login").click(function () {
        $("body").addClass("modal-open"); // Thêm lớp modal-open vào thẻ body
        $(".modal_overlay, #login-modal").fadeIn();
    });

    $(".close_modal, .modal_overlay").click(function () {
        $("body").removeClass("modal-open"); // Loại bỏ lớp modal-open khỏi thẻ body
        $(".modal_overlay, #login-modal").fadeOut();
        setTimeout(function () {
            $(".left").addClass("active");
            $(".right").removeClass("active");
            $(".container-right").hide();
            $(".container-left").show();
        },500)   
    });

    $("#btnforget").click(function () {
        $("#tabmain").hide();
        $(".container-left").hide();
        $("#forgetpass").show();
        $(".container-forgetpass").show()
    })

    $("#backtabmain").click(function () {
        $("#forgetpass").hide();
        $(".container-forgetpass").hide()
        $("#tabmain").show();
        $(".container-left").show();
    })
    //ĐẾM NGƯỢC TIME MỞ NÚT GỬI MÃ KHI GỬI MÃ THÀNH CÔNG
    function disableSendCodeButton() {
        $("#sendcode").prop("disabled", true);

        var count = 60;
        var countdown = setInterval(function () {
            $("#sendcode").text("(" + count + "s)");
            count--;

            if (count < 0) {
                clearInterval(countdown);
                $("#sendcode").text("Gửi mã");
                $("#sendcode").prop("disabled", false);
            }
        }, 1000);
    }
    //Kiểm tra mail hoặc tài khoản có tồn tại không rồi gửi mã
    $("#sendcode").click(function () {
        var usernameforget = $("#usernameforget").val();
        var recipientEmail = $("#emailforget").val();

        if (usernameforget === "" || recipientEmail === "") {
            ToastError("Vui lòng nhập Tài khoản và Email")
            return;
        }
        $.ajax({
            url: '/Account/CheckMail', 
            type: 'POST',
            data: {
                recipientEmail: recipientEmail,
                username: usernameforget
            },
            success: function (response) {
                if (response.code === 200) {
                    disableSendCodeButton();
                    $(".loading").show();
                    $.ajax({
                        url: '/Account/SendVerificationCode',
                        type: 'POST',
                        data: {
                            recipientEmail: recipientEmail,
                        },
                        success: function (response) {
                            if (response.code === 200) {
                                $(".loading").hide();
                                ToastSuccess(response.msg, "Vui lòng kiểm tra Email của bạn", false);
                            } else {
                                $(".loading").hide();
                                ToastError(response.msg);
                            }
                        },
                        error: function () {
                            ToastError("Đã xảy ra lỗi. Vui lòng thử lại.");
                        }
                    });
                } else {
                    ToastError(response.msg);
                }
            },
            error: function () {
                ToastError("Đã xảy ra lỗi. Vui lòng thử lại.");
            }
        });   
    })
    //Xử lý Reset Password
    $("#resetpass").click(function () {
        let usernameforget = $("#usernameforget").val();
        let passwordforget = $("#passwordforget").val();
        let repasswordforget = $("#repasswordforget").val();
        let emailforget = $("#emailforget").val();
        let codeforget = $("#codeforget").val();

        if (usernameforget === "" || passwordforget === "" || repasswordforget === "" || emailforget === "" || codeforget === "") {
            ToastError("Vui lòng nhập đầy đủ thông tin")
            return;
        }

        // Kiểm tra định dạng email
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regex đơn giản cho email
        if (!emailPattern.test(emailforget)) {
            ToastError("Email không hợp lệ");
            return;
        }

        // Kiểm tra mật khẩu đủ mạnh
        // Mật khẩu cần ít nhất 6 ký tự, gồm chữ và số
        var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
        if (!passwordPattern.test(passwordforget)) {
            ToastError("Mật khẩu phải dài ít nhất 6 ký tự và bao gồm cả chữ và số");
            return;
        }

        // Kiểm tra mật khẩu và mật khẩu nhập lại có khớp nhau
        if (passwordforget !== repasswordforget) {
            ToastError("Mật khẩu không trùng khớp");
            return;
        }

        $.ajax({
            type: "POST",
            url: '/Account/ResetPassWord',
            data: {
                username: usernameforget,
                password: passwordforget,
                email: emailforget,
                sendcode: codeforget
            },
            success: function (response) {
                if (response.code === 200) {
                    clearInput(1);
                    $("#backtabmain").trigger('click');
                    ToastSuccess("Thành công", response.msg, false)
                } else {
                    ToastError(response.msg)
                }
            },
            error: function (err) {
                commonError(err);
            }
        });
        
    })

    $(".left, .right").click(function () {
        $(this).addClass("active");
        $(".left, .right").not(this).removeClass("active");

        if ($(this).hasClass("left")) {
            $(".container-right").hide();
            $(".container-left").show();
        } else {
            $(".container-left").hide();
            $(".container-right").show();
        }
    });
    // Hàm reset reCAPTCHA
    function resetRecaptcha() {
        grecaptcha.reset();
    }
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
                    location.reload();
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

    function clearInput(check) {
        if (check == 0) {
            $("#fullname").val("");
            $("#username").val("");
            $("#password").val("");
            $("#repassword").val("");
            $("#email").val("");
        } else {
            $("#usernameforget").val("");
            $("#passwordforget").val("");
            $("#repasswordforget").val("");
            $("#emailforget").val("");
            $("#codeforget").val("");
        }
        
    }

    //Xử Lý Đăng ký tài khoản
    $("#register").click(function () {
       
        var fullname = $("#fullname").val();
        var username = $("#username").val();
        var password = $("#password").val();
        var repassword = $("#repassword").val();
        var email = $("#email").val();


        if (fullname === "" || username === "" || password === "" || repassword === "" || email === "") {
            ToastError("Vui lòng nhập đầy đủ thông tin")
            resetRecaptcha();
            return;
        }

        // Kiểm tra định dạng email
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Regex đơn giản cho email
        if (!emailPattern.test(email)) {
            ToastError("Email không hợp lệ");
            resetRecaptcha();
            return;
        }

        // Kiểm tra mật khẩu đủ mạnh
        // Mật khẩu cần ít nhất 6 ký tự, gồm chữ và số
        var passwordPattern = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/;
        if (!passwordPattern.test(password)) {
            ToastError("Mật khẩu phải dài ít nhất 6 ký tự và bao gồm cả chữ và số");
            resetRecaptcha();
            return;
        }

        // Kiểm tra mật khẩu và mật khẩu nhập lại có khớp nhau
        if (password !== repassword) {
            ToastError("Mật khẩu không trùng khớp");
            resetRecaptcha();
            return;
        }

        var captcharesponse = grecaptcha.getResponse().trim();
        if (!captcharesponse) {
            ToastError("Vui lòng hoàn thành hình ảnh xác thực.");
            resetRecaptcha();
            return;
        }
        // Kiểm tra CAPTCHA trước khi gửi yêu cầu đăng ký
        $.ajax({
            type: "POST",
            url: '/Account/CheckCaptcha',
            data: { captcharesponse: captcharesponse },
            success: function (response) {
                if (response.result === "PASS") {
                    $(".loading").show();
                    // Nếu CAPTCHA hợp lệ, thực hiện đăng ký
                    $.ajax({
                        type: "POST",
                        url: '/Account/RegisterAccount',
                        data: {
                            fullname: fullname,
                            username: username,
                            password: password,
                            email: email
                        },
                        success: function (response) {
                            if (response.code === 200) {
                                $(".loading").hide();
                                $(".modal_overlay").trigger('click');
                                clearInput(0);
                                resetRecaptcha();
                                ToastSuccess("Đăng ký thành công", response.msg, false)
                            } else if (response.code === 400) {
                                $(".loading").hide();
                                ToastError(response.msg)
                                resetRecaptcha();
                            } else {
                                $(".loading").hide();
                                ToastError(response.msg)
                                resetRecaptcha();
                            }
                        },
                        error: function (err) {
                            commonError(err);
                            resetRecaptcha();
                        }
                    });
                } else {
                    ToastError("Xác thực CAPTCHA thất bại")
                    resetRecaptcha();
                }
            },
            error: function (err) {
                commonError(err);
                resetRecaptcha();
            }
        });
    });

    // Bắt sự kiện keypress cho #passsignin và #usernamesignin
    $("#passsignin, #usernamesignin").keypress(function (e) {
        if (e.which === 13) { 
            performLogin();
        }
    });

    // Bắt sự kiện click cho nút #signin
    $("#signin").click(function () {
        performLogin();
    });

    function performLogin() {
        let usernamesignin = $("#usernamesignin").val();
        let passsignin = $("#passsignin").val();

        if (usernamesignin === "" || passsignin === "") {
            ToastError("Vui lòng nhập đầy đủ thông tin!")
            return;
        }
        $.ajax({
            type: "POST",
            url: "/Account/CheckLogin",
            data: {
                usernamesignin: usernamesignin,
                passsignin: passsignin
            },
            success: function (reponse) {
                if (reponse.code == 200) {
                    ToastSuccess("Thành công", reponse.msg, true);
                }
                else {
                    ToastError(reponse.msg);
                }
            },
            error: function (err) {

            }
        })      
    }
 
});