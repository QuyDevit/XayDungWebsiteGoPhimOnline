
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

        var inputField = $("#text_field");
        if (inputField) {
            inputField.focus();
        }

    $("#toggle-sidebar").click(function () {
        $("#sidebar").addClass("hide");
        $("#sidebar2").show();
    });
    $("#toggle-sidebar2").click(function () {
        $("#sidebar").removeClass("hide");
        $("#sidebar2").hide();
    });

    function handleTimeClick(timeValue, displayText, duration) {
        $("#time10, #time30, #time60").prop("checked", false);
        $("#" + timeValue).prop("checked", true);
        $("#timer").text(displayText);
        resetGame(duration);
    }

    $("#time10").click(function () {
        handleTimeClick("time10", "10s", 10);
        resetGame(10);
        level = 1;
    });

    $("#time30").click(function () {
        handleTimeClick("time30", "30s", 30);
        resetGame(30);
        level = 2;
    });

    $("#time60").click(function () {
        handleTimeClick("time60", "60s", 60);
        resetGame(60);
        level = 3;
    });

    var typingSound = new Howl({
        src: ['../Theme/sound/key-sound_Default.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });
    var typingCorrectSound = new Howl({
        src: ['../Theme/sound/public_sounds_correct.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });
    var typingWrongSound = new Howl({
        src: ['../Theme/sound/public_sounds_beep.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });

    var startTime = 10;
    var typingStarted = false;
    var timerInterval;
    var score = 0;
    var highestScore = 0;
    var level = 1;

    var storedHighestScore = localStorage.getItem("highestScore");
    if (storedHighestScore) {
        highestScore = parseInt(storedHighestScore, 10);
        $("#highscore").text(highestScore);
    }

    function resetGame(duration) {
        clearInterval(timerInterval);
        $("#text_field").val('');
        $("#wordrandom").text("Type \"start\" to play");
        $("#text_field").attr("placeholder", "\"start\"");
        $("#wordrandom").addClass("blink");
        localStorage.setItem("highestScore", highestScore);
        typingStarted = false;
        score = 0;
        if (duration == 10) {
            level = 1;
        } else if (duration == 30) {
            level = 2;
        } else {
            level = 3;
        }
        startTime = duration;
        updateTimer();
    }
    $("#text_field").on("input", function () {
        
        var charEntered = $(this).val().slice(-1);
     

        if (/\s/g.test(charEntered)) {
            var inputText = $("#text_field").val().trim();
            if (!typingStarted) {
                if (inputText === "start") {
                    typingCorrectSound.play();
                    startTimer();
                    $("#score").text(score);
                    score++;
                    $("#score").text(score);
                    typingStarted = true;
                    $("#wordrandom").removeClass("blink");
                    var wordRandom = getRandomWord();
                    $("#timeover").hide();
                    $("#wordrandom").text(wordRandom);
                    $("#text_field").attr("placeholder", "type \"" + wordRandom + "\"");
                    $("#text_field").val("")
                }
                else {
                    typingWrongSound.play();
                }
            }
            else {
                checkWord();
            }
        } else {
            typingSound.play();
        }
    });

    function checkWord() {
        var inputText = $("#text_field").val().trim();
        var targetWord = $("#wordrandom").text();

        if (inputText === targetWord) {
            typingCorrectSound.play();
            score++;
            $("#score").text(score);
            var wordRandom = getRandomWord();
            $("#wordrandom").text(wordRandom);
            $("#text_field").attr("placeholder", "type \"" + wordRandom + "\"");
            $("#text_field").val("");
        }
        else {
            typingWrongSound.play();
        }
    }

    function startTimer() {
        updateTimer();
        timerInterval = setInterval(function () {
            startTime--;
            updateTimer();
            if (startTime <= 0) {
                clearInterval(timerInterval);
                $("#text_field").val('');
                $("#timeover").show();
                var currentTimestamp = new Date().getTime();
                $.ajax({
                    type: 'post',
                    url: '/TypingGame/HandlerSaveResultGame',
                    data: {
                        score: score,
                        timestamp: currentTimestamp,
                        level: level,
                        exerciseid : 6,
                    },
                    success: function (response) {
                        if (response.code == 400) {
                            ToastError(response.msg);
                        } 
                    },
                    error: function (error) {
                        ToastError(error);
                    }
                })
                if (score > highestScore) {
                    highestScore = score;
                    $("#highscore").text(highestScore);
                    localStorage.setItem("highestScore", highestScore);
                } else {
                    $("#highscore").text(highestScore);
                }
                if ($("#time10").is(":checked")) {
                    resetGame(10);
                } else if ($("#time30").is(":checked")) {
                    resetGame(30);
                } else {
                    resetGame(60);
                }
                
            }
        }, 1000);
    }

    function updateTimer() {
        $("#timer").text(startTime + 's');
    }

    function getRandomWord() {
        var englishWords = arrtext;
        var randomIndex = Math.floor(Math.random() * englishWords.length);
        return englishWords[randomIndex];
    }

});