
$(document).ready(function ()
{
    $("#toggle-sidebar").click(function () {
        $("#sidebar").addClass("hide");
        $("#sidebar2").show();
    });
    $("#toggle-sidebar2").click(function () {
        $("#sidebar").removeClass("hide");
        $("#sidebar2").hide();
    });

    var typingCorrectSound = new Howl({
        src: ['../Theme/sound/public_sounds_correct.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });
    var typingWrongSound = new Howl({
        src: ['../Theme/sound/public_sounds_beep.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });

    let wpm = 0;
    let level = 1;
    let startTime;
    let requestId; // Biến để lưu ID của requestAnimationFrame
    let deleteCount = 0; // Biến đếm số lượng phần tử đã bị xóa
    let isTimerStopped = false;
    let hasValidKeyPressed = false;
    let elementChild = 40;
    let correctKeyLeft = 0;
    let correctKeyRight = 0;
    let wrongKey = 0;
    let timeTaken = 0;
    let bestWpm = getHighWpm();

    if (bestWpm) {
        $("#highwpm").text(bestWpm);
    }

    function handleLevelClick(levelValue, value) {
        $("#level1, #level2, #level3").prop("checked", false);
        $("#" + levelValue).prop("checked", true);;
        elementChild = value;
    }

    $("#level1").click(function () {
        handleLevelClick("level1", 40);
        $("#left-container").empty();
        $("#right-container").empty();
        level = 1;
        resetGame(40);
    });

    $("#level2").click(function () {
        handleLevelClick("level2", 80);
        $("#left-container").empty();
        $("#right-container").empty();
        level = 2;
        resetGame(80);

    });

    $("#level3").click(function () {
        handleLevelClick("level3", 120);
        $("#left-container").empty();
        $("#right-container").empty();
        level = 3;
        resetGame(120);

    });

    function resetGame(newElementChild) {
        deleteCount = 0;
        isTimerStopped = false;
        elementChild = newElementChild || 40; // Nếu không có giá trị mới, sử dụng 40
        hasValidKeyPressed = false;
        correctKeyLeft = 0;
        correctKeyRight = 0;
        wrongKey = 0;
        timeTaken = 0;
        startTime = undefined;
        $(".time-game").text("0.00");
        const processLeftActive = $(".process-left-active");
        processLeftActive[0].style.setProperty('--before-width', 0 +"%");
        const processRightActive = $(".process-right-active");
        processRightActive[0].style.setProperty('--before-width', 0 + "%");
        renderLeftElements();
        renderRightElements();

    }

    $(".btn-reset-game").click(function () {
        $(".show-result").hide();
        $("#active-game").show();
        $("#level1").prop("checked", true);
        $("#left-container").empty();
        $("#right-container").empty();
        resetGame(40);
    })

    // Function to get one of the specified colors
    function getRandomColor() {
        const colors = ['#e58b88', '#70ae98', '#9dabdd', '#ecbe7a'];
        return colors[Math.floor(Math.random() * colors.length)];
    }

    // Function to create a random arrow direction
    function getRandomArrowDirection() {
        const directions = ['↑', '↓', '←', '→'];
        return directions[Math.floor(Math.random() * directions.length)];
    }

    // Function to create a random letter
    function getRandomLetter() {
        const letters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        return letters[Math.floor(Math.random() * letters.length)];
    }

    // Function to create a key word element
    function createKeyWordElement(isLeft) {
        const keyWordElement = document.createElement('div');
        keyWordElement.classList.add('key-word');

        if (isLeft) {
            const randomColor = getRandomColor();
            keyWordElement.style.backgroundColor = randomColor;
            keyWordElement.textContent = getRandomLetter();
        } else {
            // Set default colors for arrows
            keyWordElement.style.backgroundColor = '#e58b88'; // Default color for arrow

            // Set different colors based on arrow direction
            const arrowDirection = getRandomArrowDirection();
            switch (arrowDirection) {
                case '↑':
                    keyWordElement.style.backgroundColor = '#e58b88'; // Red
                    break;
                case '↓':
                    keyWordElement.style.backgroundColor = '#70ae98'; // Green
                    break;
                case '←':
                    keyWordElement.style.backgroundColor = '#9dabdd'; // Blue
                    break;
                case '→':
                    keyWordElement.style.backgroundColor = '#ecbe7a'; // Yellow
                    break;
                default:
                    break;
            }

            keyWordElement.textContent = arrowDirection;
        }

        return keyWordElement;
    }

    // Function to render elements on the left container
    function renderLeftElements() {
        const leftContainer = document.getElementById('left-container');

        for (let i = 0; i < elementChild/2; i++) {
            const keyWordElement = createKeyWordElement(true);
            leftContainer.appendChild(keyWordElement);
        }
    }

    // Function to render elements on the right container
    function renderRightElements() {
        const rightContainer = document.getElementById('right-container');

        for (let i = 0; i < elementChild/2; i++) {
            const keyWordElement = createKeyWordElement(false);
            rightContainer.appendChild(keyWordElement);
        }
    }

    renderLeftElements();
    renderRightElements();

    function startTimer() {
        if (!hasValidKeyPressed || startTime !== undefined) {
            return; // Don't start the timer if no valid key has been pressed
        }
        startTime = new Date();
        updateTimer(); // Gọi ngay sau khi bắt đầu để khởi động vòng lặp requestAnimationFrame
    }

    function updateTimer() {
        if (!isTimerStopped && startTime !== undefined ) {
            const currentTime = new Date();
            const elapsedTime = currentTime - startTime;

            const seconds = (elapsedTime / 1000).toFixed(2);
            timeTaken = seconds;
            document.querySelector('.time-game').textContent = seconds;
            requestId = requestAnimationFrame(updateTimer);
        }
    }

    function stopTimer() {
        cancelAnimationFrame(requestId); // Hủy vòng lặp khi cần dừng
        isTimerStopped = true;
    }

    function handleCorrectWord() {
        if (!startTime || isTimerStopped) {
            startTimer();
        } else {
            updateTimer();
        }
    }

    function handleKeyDown(event) {
        if (event.key !== "F5") {
            event.preventDefault();
            if (
                /^[a-zA-Z]$/.test(event.key) ||
                ["ArrowUp", "ArrowDown", "ArrowLeft", "ArrowRight"].includes(event.key)
            ) {
                hasValidKeyPressed = true; // Set the flag to indicate a valid key has been pressed
                handleCorrectWord(); // Start the timer if it hasn't already started
            }
            const leftContainer = document.getElementById('left-container');
            const rightContainer = document.getElementById('right-container');
            let leftElements = leftContainer.querySelectorAll('.key-word');
            let rightElements = rightContainer.querySelectorAll('.key-word');

            const leftFirstElement = leftElements[0];
            const rightFirstElement = rightElements[0];

            if (!leftFirstElement && !rightFirstElement) {
                return;
            }

            const keyPressed = event.key.toUpperCase();

            if (hasValidKeyPressed) {
                if (
                    (leftFirstElement && keyPressed === leftFirstElement.textContent) ||
                    (leftFirstElement && event.key === 'ArrowUp' && leftFirstElement.textContent === '↑') ||
                    (leftFirstElement && event.key === 'ArrowDown' && leftFirstElement.textContent === '↓') ||
                    (leftFirstElement && event.key === 'ArrowLeft' && leftFirstElement.textContent === '←') ||
                    (leftFirstElement && event.key === 'ArrowRight' && leftFirstElement.textContent === '→')
                ) {
                    leftContainer.removeChild(leftFirstElement);
                    // Update the NodeList after removing the element
                    leftElements = leftContainer.querySelectorAll('.key-word');
                    correctKeyLeft++
                    typingCorrectSound.play();
                    const processLeftActive = $(".process-left-active");
                    processLeftActive[0].style.setProperty('--before-width', (correctKeyLeft / (elementChild / 2)) * 100 + "%");
                    deleteCount++;
                } else if (
                    (rightFirstElement && keyPressed === rightFirstElement.textContent) ||
                    (rightFirstElement && event.key === 'ArrowUp' && rightFirstElement.textContent === '↑') ||
                    (rightFirstElement && event.key === 'ArrowDown' && rightFirstElement.textContent === '↓') ||
                    (rightFirstElement && event.key === 'ArrowLeft' && rightFirstElement.textContent === '←') ||
                    (rightFirstElement && event.key === 'ArrowRight' && rightFirstElement.textContent === '→')
                ) {
                    rightContainer.removeChild(rightFirstElement);
                    // Update the NodeList after removing the element
                    rightElements = rightContainer.querySelectorAll('.key-word');
                    correctKeyRight++;
                    typingCorrectSound.play();
                    const processRightActive = $(".process-right-active");
                    processRightActive[0].style.setProperty('--before-width', (correctKeyRight / (elementChild / 2)) * 100 + "%");
                    deleteCount++;
                }
                else {
                    typingWrongSound.play();
                    wrongKey++;
                }
            }          
        }
    }

    function getHighWpm() {
        const storedHighscore = localStorage.getItem("best-wpm-game4");
        return storedHighscore ? JSON.parse(storedHighscore) : 0;
    }

    function setHighWpm(newHighwpm) {
        localStorage.setItem("best-wpm-game4", JSON.stringify(newHighwpm));
    }

    function showResult() {
        $("#time-taken").text(timeTaken);
        const accuracyPercentage = (elementChild + wrongKey) ? parseFloat(((elementChild / (elementChild + wrongKey)) * 100).toFixed(2)) : 0;
        $("#accuracy").text(accuracyPercentage+"%");
        wpm = Math.round((elementChild - wrongKey) / (timeTaken / 60));
        wpm < 0 ? 0 : wpm;
        if (wpm > bestWpm) {
            bestWpm = wpm;
            $("#highwpm").text(bestWpm);
            setHighWpm(bestWpm);
        }
        $("#wpm").text(wpm);
        $("#wrongkey").text(wrongKey);
        var currentTimestamp = new Date().getTime();
        $.ajax({
            type: 'post',
            url: '/TypingGame/HandlerSaveResultGame',
            data: {
                score: wpm,
                timestamp: currentTimestamp,
                level: level,
                exerciseid: 9,
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
    }

    document.addEventListener('keydown', function (event) {
            handleCorrectWord();
        handleKeyDown(event);
        if (deleteCount >= elementChild) {
            stopTimer();
            $("#active-game").hide();
            $(".show-result").show();
            showResult();
           
        }
    });
})