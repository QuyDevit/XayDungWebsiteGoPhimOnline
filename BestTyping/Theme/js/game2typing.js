$("#toggle-sidebar").click(function () {
    $("#sidebar").addClass("hide");
    $("#sidebar2").show();
});
$("#toggle-sidebar2").click(function () {
    $("#sidebar").removeClass("hide");
    $("#sidebar2").hide();
});
var KeyContainerColors = (function Colors() {
    var colors = ["#A7BF4E",
        "#EDF25C",
        "#A67B56",
        "#F25A38",
        "#8CC3F2",
        "#F2B90F",
        "#F2620F",
        "#BF2633",
        "#D9304F",
        "#F24976",
        "#8BC3D9",
        "#D9AB73",
        "#DEC26E",
        "#423547",
        "#F2D541",
        "#F25270"];

    return {
        getCount: function () {
            return colors.length;
        },
        getColor: function (idx) {
            return colors[idx];
        },
        getRandomColor: function () {
            var idx = parseInt(Math.random() * colors.length);
            return colors[idx];
        }
    }
})();

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




(function ($) {
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

    var delayInSeconds = 0.3;
    var delayInMillis = delayInSeconds * 1000;

    function handleLevelClick(levelValue,value) {
        $("#level1, #level2, #level3").prop("checked", false);
        $("#" + levelValue).prop("checked", true);;
        delayInSeconds = value;
        delayInMillis = value * 1000;
    }

    $("#level1").click(function () {
        handleLevelClick("level1", 0.3);
        level = 1;
    });

    $("#level2").click(function () {
        handleLevelClick("level2", 0.2);
        level = 2;
    });

    $("#level3").click(function () {
        handleLevelClick("level3", 0.1);
        level = 3;
    });

    var GAME_STATE = {
        NEW: 0,
        PLAYING: 1,
        OVER: 2,
        RESETING: 3,
        HELP: 4
    };

    var KEYS = {
        ENTER: 13
    };

    var K_S = {
        PRESS_ENTER: "PRESS ENTER",
        X: ""
    };

    var LTIMES = 5;
    var DELAY = 640 / LTIMES;
    var level = 1;
    var MIN = 2;
    var score = 0;
    var gameContent = $("#game-content");
    var scoreText = $("#score-text");
    var bestScoreText = $("#best-score-text");
    var boxContainers = $("#key-containers .key-container");

    var gameState = GAME_STATE.NEW;
    var gameStateStack = [];
    var queue = [];

    var l = 0;

    var boxes = [];

    function resetBox(box) {
        container = boxContainers.eq(box.idx);
        container.attr('style', '');

        var containerText = container.find(".container-text").eq(0);
        containerText.text('');

        box.used = false;
    }

    function resetGame() {
        boxes.length = 0;
        queue.length = 0;
        updateGameScore(0);

        var count = boxContainers.length;

        for (var i = 0; i < count; i++) {

            boxes[i] = {
                used: false,
                idx: i
            }

            resetBox(boxes[i]);
        }
    }

    function getFreeBoxes() {
        var freeBoxes = [];
        var boxesCount = boxes.length;
        for (var i = 0; i < boxesCount; i++) {
            var box = boxes[i];
            if (!box.used) {
                freeBoxes.push(box);
            };
        }

        return freeBoxes;
    }


    function gameLoop(t) {
        var startTime = Date.now();

        switch (gameState) {
            case (GAME_STATE.NEW): {

                // valueHolder.textContent = K_S.X;
                break;
            }
            case (GAME_STATE.PLAYING): {

                if (l == 0) {
                    // valueHolder.textContent = '';

                    var box = getRandomFreeBox();

                    if (box) {
                        box.used = true;

                        var randNum = parseInt(Math.random() * 36);
                        var valueToPush,
                            textContent;

                        if (randNum >= 10) {
                            randNum -= 10;

                            valueToPush = randNum + 97;
                            textContent = String.fromCharCode(valueToPush);
                        }
                        else {
                            valueToPush = randNum + 48;
                            textContent = randNum;
                        }

                        var container = getContainerFromBox(box);
                        container.css('border-color', KeyContainerColors.getRandomColor());

                        var containerText = container.find(".container-text").eq(0);

                        containerText.text(textContent);

                        queue.push({
                            key: valueToPush,
                            box: box
                        });
                    }

                    if (queue.length == boxContainers.length) {
                        updatePopupOverMessage("Overflown!");
                        $(".selection input:not(:checked)").closest('.selection').css("opacity", 1);
                        $(".selection input:not(:checked)").prop("disabled", false);
                        gameStateStack.push(GAME_STATE.OVER);
                        updateGameState(GAME_STATE.RESETING);
                    }

                }

                l = (l + 1) % 5;

                break;
            }
            case (GAME_STATE.RESETING): {
                updateGameLastScore(score);

                l = 0;
                // valueHolder.textContent = K_S.X;
                resetGame();
                updateGameState(gameStateStack.pop());

                break;
            }
            case (GAME_STATE.OVER): {

                break;
            }

        }

        var now = Date.now();
        var elapsed = now - startTime;
        var remainingDelay = delayInMillis - elapsed;

        var nextIterationDelay = Math.max(0, remainingDelay);

        setTimeout(gameLoop, nextIterationDelay);
    }

    function updateGameState(newState) {
        gameState = newState;
        gameContent.attr('class', "state-" + gameState);
    }

    function updateGameScore(newScore) {
        score = newScore;
        scoreText.text(score);

        if (typeof (Storage) != "undefined") {
            var bestScore = localStorage.getItem('best-score');
            if (typeof (bestScore) == "string") {
                bestScore = parseInt(bestScore);
                if (bestScore < score) {
                    bestScore = score;

                    localStorage.setItem('best-score', score);
                };
            }
            else {
                bestScore = score;
                localStorage.setItem('best-score', score);
            }
            bestScoreText.text(bestScore);
        }
        else {
            bestScoreText.text(newScore);
        }
    }

    function updateGameLastScore(newLastScore) {
        var popupScoreText = $("#popup-score-text");
        popupScoreText.text(newLastScore);
    }

    function updatePopupOverMessage(msg) {
        var popupOverMsg = $("#popup-over-msg");
        popupOverMsg.text(msg);
    }

    function getRandomIndex(count) {
        return parseInt(Math.random() * count);
    }

    function getRandomFreeBox() {
        var freeBoxes = getFreeBoxes();
        if (freeBoxes.length == 0) return;

        return freeBoxes[getRandomIndex(freeBoxes.length)];
    }

    function getContainerFromBox(box) {
        var containers = $("#key-containers .key-container");
        var container = containers.eq(box.idx);

        return container;
    }

    function checkKeyCode(keyCode) {
        if (queue.length > MIN && queue[0].key == keyCode) {
            resetBox(queue[0].box);
            queue.splice(0, 1);
            typingCorrectSound.play();
            updateGameScore(score + 1);

        }
        else {
            if (queue.length <= MIN) {
                typingWrongSound.play();
                updatePopupOverMessage("You can hit a key only if there are atleast " + (MIN + 1) + " key(s) on the screen!");
                $(".selection input:not(:checked)").closest('.selection').css("opacity", 1);
                $(".selection input:not(:checked)").prop("disabled", false);               
            }
            else {
                updatePopupOverMessage("Wrong key!");
                typingWrongSound.play();
                var currentTimestamp = new Date().getTime();
                $.ajax({
                    type: 'post',
                    url: '/TypingGame/HandlerSaveResultGame',
                    data: {
                        score: score,
                        timestamp: currentTimestamp,
                        level: level,
                        exerciseid: 7,
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
                $(".selection input:not(:checked)").closest('.selection').css("opacity", 1);
                $(".selection input:not(:checked)").prop("disabled", false);
            }
            gameStateStack.push(GAME_STATE.OVER);
            updateGameState(GAME_STATE.RESETING);
        }
    }

    $(document).on('keypress', function (e) {
        var keyCode = e.keyCode;
        if (e.key) {
            if (e.key == 'Enter') {
                keyCode = KEYS.ENTER;
            }
            else {
                keyCode = e.key.charCodeAt(0)
            }
        }

        if (gameState == GAME_STATE.PLAYING) {
            checkKeyCode(keyCode);
        }
        else {

            if (keyCode == KEYS.ENTER) {
                gameStateStack.push(GAME_STATE.PLAYING);
                updateGameState(GAME_STATE.RESETING);
            }
        }
    });

    (function addListeners() {
        var startBtn = $("#start-button");
        startBtn.on('click', function startBtnHandler() {
            gameStateStack.push(GAME_STATE.PLAYING);
            $(".selection input:not(:checked)").closest('.selection').css("opacity", 0.5);
            $(".selection input:not(:checked)").prop("disabled", true);
            updateGameState(GAME_STATE.RESETING);
        });

        var restartBtn = $("#restart-button");
        restartBtn.on('click', function restartBtnHandler() {
            gameStateStack.push(GAME_STATE.PLAYING);
            $(".selection input:not(:checked)").closest('.selection').css("opacity", 0.5);
            $(".selection input:not(:checked)").prop("disabled", true);
            updateGameState(GAME_STATE.RESETING);
        });

        $(".how-to-play-button").on('click', function howToPlayHandler() {
            gameStateStack.push(gameState);
            updateGameState(GAME_STATE.HELP);
        });

        $(".pop-state-btn").on('click', function popStateHandler() {
            updateGameState(gameStateStack.pop());
        });

        gameContent.on('click', function (e) {
            if (gameState == GAME_STATE.PLAYING) {
                if ($(e.target).hasClass('container-text')) {
                    checkKeyCode(e.target.textContent.charCodeAt(0))
                }
            }
        })
    })();

    resetGame();
    gameLoop(0);
})(jQuery);
