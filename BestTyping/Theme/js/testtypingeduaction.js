$(document).ready(function () {
    var inputField = document.getElementById("inputfield");
    if (inputField) {
        inputField.focus();
    }
    //Xử lý typing test
    var totalCorrectCharacters = 0;
    var totalWrongCharacters = 0;
    var totalCorrectWords = 0;
    var totalWrongWords = 0;
    var totalCharacters = 0;
    var totalWord = 0;
    var accuracyPercentage = 0;
    var checkShowNext = 0;
    var currentWord = 1;
    var startTime = examDuration;
    var timerInterval;
    var typingStarted = false;
    var wpm = 0;

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
        if (totalCorrectWords - 5 > totalWrongWords) {
            wpm = Math.round((totalCharacters / 5 / (examDuration - startTime)) * 60);
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
    function endTest() {
        clearInterval(timerInterval);
        updateInfoPlayer();
        $("#fillter").remove();
       
        toggleFullScreen(false);
        setTimeout(function () {
            $("#result-test").show();
            $("#result-test").addClass("slide-down");
            confetti.start();
            setTimeout(() => {
                confetti.stop();
            }, 3000);
        }, 500);
        var currentTimestamp = new Date().getTime();
        $.ajax({
            type: 'post',
            url: '/DashBoardStudent/HandleSaveResultTypingEdu',
            data: {
                testid: datatest,
                roomid: data,
                accuracy: parseFloat(accuracyPercentage.toFixed(2)),
                wpm: wpm,
                mistakes: totalWrongWords,
                correctwords: totalCorrectWords,
                timestamp: currentTimestamp,
                keystrokes: totalCharacters,
                correctcharacters: totalCorrectCharacters,
                wrongcharacters: totalWrongCharacters
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

    function startTimer() {
        timerInterval = setInterval(function () {
            startTime--;
            updateTimer();
            if (startTime <= 0) {
                endTest();       
            }
        }, 1000);
    }

    function updateTimer() {
        var minutes = Math.floor(startTime / 60);
        var seconds = startTime % 60;
        var formattedTime = minutes + ":" + (seconds < 10 ? "0" + seconds : seconds);
        $("#timer").text(formattedTime);
    }

    function highlightNextWord() {
        $(".highlight").removeClass("highlight");
        $("#word" + currentWord).addClass("highlight");
    }
    var arrchar = [];

    $("#inputfield").on("input", function (event) {
        if (!typingStarted) {
            typingStarted = true;
            setTimeout(function () {
                var container = $("#quote");
                var childElements = container.find(".word");
                checkShowNext = countVisibleChildren(container, childElements);
            }, 5000)
        }

        var currentValue = $(this).val();
        // Xác định ký tự mới bằng cách so sánh độ dài của mảng arrchar với giá trị hiện tại
        if (arrchar.join("").length < currentValue.length) {
            arrchar.push(currentValue.slice(-1)); // Chỉ thêm ký tự mới nhất
        } else {
            // Xử lý xóa: Cập nhật lại mảng arrchar dựa trên giá trị hiện tại của input
            arrchar = currentValue.split("");
        }

        var charEntered = $(this).val().slice(-1);

        if (/\s/g.test(charEntered)) {
            if ($("#inputfield").val().length == 1) {
                $("#inputfield").val("");
            } else {
                checkWord();
                if (currentWord == checkShowNext) {
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

        if (inputText === targetWord && arrchar.length - 1 == targetWord.length) {
            $("#word" + currentWord).addClass("correct");
        } else {
            $("#word" + currentWord).removeClass("checkhighlight").addClass("wrong");
        }
        arrchar = [];
        currentWord++;

        if (currentWord <= text.length) {
            highlightNextWord();
        } else {
            endTest();
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
    updateTimer();
    renderWords(text);
    if (checkview == "True") {
        startTimer();
    }

})