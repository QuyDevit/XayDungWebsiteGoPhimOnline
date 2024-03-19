
document.addEventListener("DOMContentLoaded", function() {
    var inputField = document.getElementById("inputfield");
    if (inputField) {
        inputField.focus();
    }
});

$(document).ready(function () {
    $('#table-rank').DataTable();
    $('#table-tests').DataTable();
    $('#table-global').DataTable();
});

$(document).ready(function () {
    // Lấy tất cả các nút bằng class 'button__rank-background'
    const buttons = $(".button__rank-background");

    // Lấy tất cả các tab__pane-wrapper
    const panes = $(".tab__pane-wrapper");

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


$(document).ready(function () {
    var isModalOpen = false;
     //Xử lý trải nghiệm người dùng
    $(document).on('keydown', function (e) {
        switch (e.which) {
            case 27:
                if ($("#fillter").hasClass("full-screen")) {
                    $("#fillter").removeClass("full-screen")
                    $("#container__fillter").removeClass("fillter-div");
                } else {
                    $("#yes").addClass("active");
                    $("#no").removeClass("active");
                    $("#overlay").show();
                    $("#modal-question").show();
                    $("body").css("overflow", "hidden");
                    isModalOpen = true;
                }            
                break;
            case 39:
                if (isModalOpen) {
                    $("#yes").addClass("active");
                    $("#no").removeClass("active");
                }
                break;
            case 37:
                if (isModalOpen) {
                    $("#no").addClass("active");
                    $("#yes").removeClass("active");
                }
                break;
            case 13:
                if (isModalOpen) {
                    if ($("#yes").hasClass("active")) {
                        resetGame();
                    }
                    $("#overlay, #modal-question").hide();
                    $("body").css("overflow", "");
                    isModalOpen = false;
                }
                break;
            case 116: 
                e.preventDefault();
                if (isModalOpen) {
                    $("#overlay").hide();
                    $("#modal-question").hide();
                    $("body").css("overflow", "");
                    resetGame();
                } else {
                    resetGame();
                }
                break;
        }
    });
    $("#yes").click(function () {
        resetGame();
        $("#overlay").hide();
        $("#modal-question").hide();
        isModalOpen = false;
        $("body").css("overflow", "");
    })
    $("#no").click(function () {
        $("#overlay").hide();
        $("#modal-question").hide();
        isModalOpen = false;
        $("body").css("overflow", "");
    })

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
   

    //Xử lý typing test
    var totalCorrectCharacters = 0;
    var totalWrongCharacters = 0;
    var isFullScreen = false;
    var totalCorrectWords = 0;
    var totalWrongWords = 0;
    var totalCharacters = 0;
    var totalWord = 0;
    var accuracyPercentage = 0;
    var checkShowNext = 0;
    var currentWord = 1;
    var startTime = 60;
    var timerInterval;
    var typingStarted = false;
    var flagLanguage = "Vietnamese";
    var wpm = 0;
    var wordArray = [];
    var exerCiseTextId = 0;

    $(".nav__select-language").on("change", function () {
        // Lấy giá trị ngôn ngữ đã chọn
        flagLanguage = $(this).val();

        // Gọi hàm resetGame() với giá trị ngôn ngữ đã chọn
        resetGame();
    });

    $("#checkbox").change(function () {
        if ($(this).is(":checked")) {
            isFullScreen = true;
        } else {
            isFullScreen = false;
        }
    });

    //Xử lý lấy danh sách từ
    function getExerciseText() {
        $(document).ready(function () {
            $.ajax({
                type: 'post',
                url: '/TypingTest/GetExerciseText',
                data: {
                    exerciseid: 1,
                    language: flagLanguage
                },
                success: function (response) {
                    if (response.code == 200) {
                        wordArray = response.data;
                        exerCiseTextId = response.exercisetextid;
                        renderWords(wordArray);
                        //console.log(wordArray)
                       
                    } else {
                        console.log(response.msg);
                    }
                },
                error: function (err) {
                    console.log("Error:", err);
                }
            });
        });
    }

    function countVisibleChildren(container, children) {
        var visibleCount = 0;
        var containerRect = container[0].getBoundingClientRect();

        children.each(function () {
            var childRect = this.getBoundingClientRect();

            if (
                childRect.top >= containerRect.top &&
                childRect.bottom <= containerRect.bottom
            ) {
                visibleCount++;
            }
        });

        return visibleCount;
    }

    function toggleFullScreen(enableFullScreen) {
        if (enableFullScreen) {
            $("#fillter").addClass("full-screen");
            $("#container__fillter").addClass("fillter-div");
        } else {
            $("#fillter").removeClass("full-screen");
            $("#container__fillter").removeClass("fillter-div");
        }
    }
    function updateInfoPlayer() {
        var spans = document.querySelectorAll('.word');

        for (let i = 0; i < spans.length; i++) {
            // Đếm số từ và ký tự đúng
            if (spans[i].classList.contains('correct')) {
                totalCorrectWords++;
                totalCorrectCharacters += spans[i].innerText.length;
            }
            // Đếm số từ và ký tự sai
            if (spans[i].classList.contains('wrong')) {
                totalWrongWords++;
                totalWrongCharacters += spans[i].innerText.length;
            }
        }
        totalCharacters = totalCorrectCharacters + totalWrongCharacters;
        if (totalCorrectWords - 10 > totalWrongWords) {
            wpm = Math.round((totalCharacters / 5 / (60 - startTime)) * 60);
            wpm = wpm < 0 || !wpm || wpm === Infinity ? 0 : wpm;
            $("#keystroke").html(`( <span style="color: green;">${totalCorrectCharacters}</span> | <span style="color: red;">${totalWrongCharacters}</span> )   ${totalCharacters}`);
        } else {
            wpm = Math.round(totalCorrectCharacters / 5);
            $("#keystroke").html(`( <span style="color: green;">${totalCorrectCharacters}</span> | <span style="color: red;">${totalWrongWords}</span> )   ${totalCorrectCharacters + totalWrongWords}`);
        }
        $("#wpm").text(wpm + " WPM");

        totalWord = totalCorrectWords + totalWrongWords;
        accuracyPercentage = totalWord === 0 ? 0 : (totalCorrectWords / totalWord) * 100;
        $("#accuracy").text(accuracyPercentage.toFixed(2) + "%");

        $("#correctword").html(`<span style="color: green;">${totalCorrectWords}</span>`);

        $("#wrongword").html(`<span style="color: red;">${totalWrongWords}</span>`);
    }

    function startTimer() {
        timerInterval = setInterval(function () {
            startTime--;
            updateTimer();
            if (startTime <= 0) {
                clearInterval(timerInterval);
                disableInput();                
                updateInfoPlayer();
                toggleFullScreen(false);
                setTimeout(function () {
                    $("#quote").addClass("slide-up").hide()
                    $("#result-test").show();
                    $("#result-test").addClass("slide-down");
                },500)
                $("#inputfield").val('');
                var currentTimestamp = new Date().getTime();
                $.ajax({
                    type: 'post',
                    url: '/TypingTest/HandleSaveResult',
                    data: {
                        sumword: totalWord,
                        accuracy: parseFloat(accuracyPercentage.toFixed(2)),
                        exercise: exerCiseTextId,
                        wpm : wpm,
                        mistakes: totalWrongWords,
                        correctwords: totalCorrectWords,
                        timestamp: currentTimestamp,
                        totalcharacters: totalCharacters
                    },
                    success: function (response) {
                        if (response.code == 200) {
                        } else if (response.code == 400) {
                            ToastError(response.msg);
                        } else {
                           //console.log(response.msg);
                        }
                    },
                    error: function (err) {
                        console.log("Error:", err);
                    }
                });
            }
        }, 1000);
    }

    function updateTimer() {
        var minutes = Math.floor(startTime / 60);
        var seconds = startTime % 60;
        var formattedTime = minutes + ":" + (seconds < 10 ? "0" + seconds : seconds);
        $("#timer").text(formattedTime);
    }

    function disableInput() {
        $("#inputfield").prop("disabled", true);     
        $(".highlight").removeClass("highlight");
        $(".checkhighlight").removeClass("checkhighlight");
    }

    function resetGame() {
        $("#fillter").hide();
        $(".psoload").show(); 
        setTimeout(function () {
            $("#fillter").show();
            $("#quote").show();
            $("#container__fillter").show();
            $("#container__fillter").addClass("slide-down");
            $(".psoload").hide();
            $("#inputfield").focus();
        }, 500);
        clearInterval(timerInterval);
        $("#inputfield").val("");
        $("#inputfield").prop("disabled", false);

        $(".correct").removeClass("correct");
        $(".wrong").removeClass("wrong");
        $(".disabled").removeClass("disabled");
        wpm = 0;
        currentWord = 1;
        totalCorrectCharacters = 0;
        totalWrongWords = 0;
        totalCorrectWords = 0;
        totalWrongCharacters = 0;
        totalCharacters = 0;
        accuracyPercentage = 0;
        totalWord = 0;
        startTime = 60;
        typingStarted = false;
        updateTimer();
        getExerciseText();
       
        highlightNextWord();
    }

    function highlightNextWord() {
        $(".highlight").removeClass("highlight");
        $("#word" + currentWord).addClass("highlight");
    }

    $("#inputfield").on("input", function (event) {
        if (!typingStarted) {
            typingStarted = true;
            setTimeout(function () {
                var container = $("#quote");
                var childElements = container.find(".word");
                checkShowNext = countVisibleChildren(container, childElements);
            }, 5000)
           
            startTimer();
        }
        if (isFullScreen) {
            toggleFullScreen(true);
        }
        var charEntered = $(this).val().slice(-1);

        if (/\s/g.test(charEntered)) {
            if ($("#inputfield").val().length == 1) {
                $("#inputfield").val("");
            } else {
                checkWord();
                if (currentWord  == checkShowNext) {
                    for (i = 1; i < checkShowNext; i++) {
                        $("#word" + i).hide();
                    }
                    var container = $("#quote");
                    var childElements = container.find(".word");
                    checkShowNext = countVisibleChildren(container, childElements) + currentWord - 1;
                }
            }
        } else {
            checkCurrentWord();
        }
    });
    function checkCurrentWord() {
        var inputText = $("#inputfield").val();
        var targetWord = $("#word" + currentWord).text();

        if (inputText == targetWord.substring(0, inputText.length)) {
            $("#word" + currentWord).removeClass("correct checkhighlight").addClass("highlight");
        } else {
            $("#word" + currentWord).removeClass("correct highlight").addClass("checkhighlight");
        }
    }

    function checkWord() {

        var inputText = $("#inputfield").val().trim();
        var targetWord = $("#word" + currentWord).text();

        if (inputText === targetWord) {
            $("#word" + currentWord).addClass("correct");         
        } else {            
            $("#word" + currentWord).removeClass("checkhighlight").addClass("wrong");
        }

        currentWord++;

        if (currentWord <= 200) {
            highlightNextWord();
        } else {
            disableInput();
        }
        $("#inputfield").val("");
    }

    function renderWords(wordsArray) {
        var container = $("#quote");
        container.empty();

        for (var i = 0; i < wordsArray.length; i++) {
            if (i == 0) {
                container.append("<span class='word highlight' id='word" + (i + 1) + "'>" + wordsArray[i] + "</span>\n");
            }
            else {
                container.append("<span class='word' id='word" + (i + 1) + "'>" + wordsArray[i] + "</span>\n");
            }
        }
    }

    $(".btn-reset").on("click", function () {
        resetGame();          
    });

    resetGame();
});




