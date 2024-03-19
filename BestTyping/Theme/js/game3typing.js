
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

    $("#toggle-sidebar").click(function () {
        $("#sidebar").addClass("hide");
        $("#sidebar2").show();
    });
    $("#toggle-sidebar2").click(function () {
        $("#sidebar").removeClass("hide");
        $("#sidebar2").hide();
    });

    function startCountdown(callback) {
        let countdown = 3;
        ctx.font = '48px Arial';
        ctx.fillStyle = '#fff';
        ctx.textAlign = 'center';
        ctx.fillText('Ready', canvas.width / 2, canvas.height / 2);

        const countdownInterval = setInterval(() => {
            if (countdown > 0) {
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                ctx.fillText(countdown, canvas.width / 2, canvas.height / 2);
            }

            countdown--;
            if (countdown < 0) {
                clearInterval(countdownInterval);
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                callback(); // Bắt đầu trò chơi sau khi đếm ngược
            }
        }, 1000);
    }

    function setUpStartGameEvent() {
        $(document).off('keydown').on('keydown', function (e) {
            if (e.keyCode == 13 && !isPlaying) {
                startGame();
            }
        });
    }

    function startGame() {
        isPlaying = true;
        $(".selection input:not(:checked)").closest('.selection').css("opacity", 0.5);
        $(".selection input:not(:checked)").prop("disabled", true);
        $(document).off('keydown').on('keydown', keyDownHandler); // Gỡ bỏ sự kiện keydown cũ trước khi thêm mới
        startCountdown(gameLoop); // Bắt đầu đếm ngược
        $("#note").text("In Process Game");
    }

    var typingCorrectSound = new Howl({
        src: ['../Theme/sound/public_sounds_correct.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });
    var typingWrongSound = new Howl({
        src: ['../Theme/sound/public_sounds_beep.wav'],
        volume: 1, // Âm lượng (tùy chọn)
    });

    function handleLevelClick(levelValue, value) {
        $("#level1, #level2, #level3").prop("checked", false);
        $("#" + levelValue).prop("checked", true);;
        $("#note").text("Type \"Enter\" to play");
        letter.speed = value;
    }

    $("#level1").click(function () {
        handleLevelClick("level1", 2);
        level = 1;
        setUpStartGameEvent();
    });

    $("#level2").click(function () {
        handleLevelClick("level2", 4);
        level = 2;
        setUpStartGameEvent();
    });

    $("#level3").click(function () {
        handleLevelClick("level3", 6);
        level = 3;
        setUpStartGameEvent()
    });
    let animationFrameId;

    $(".btn-reset-level").click(function () {
        // Đặt lại trò chơi
        cancelAnimationFrame(animationFrameId);
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        resetGame();
        $(".selection input").prop("disabled", false).prop("checked", false).closest('.selection').css("opacity", 1);
        $("#note").text("\"Choose the level that suits you best\"");
        $(document).off('keydown').on('keydown', keyDownHandler); // Gỡ bỏ sự kiện keydown cũ trước khi thêm mới
        isPlaying = false;       
    });

    const canvas = $("#canvas")[0];
    const ctx = canvas.getContext("2d");

    const letter = {
        font: '35px Arial',
        color: '#fff',
        width: 15,
        height: 20,
        speed: 1,
        probability: 0.02,
    };

    const explosion = {
        particles: [],
        particleCount: 20,
        particleSize: 3,
        explosionSpeed: 5
    };
    let isPlaying = false;
    let letters = [];
    let score = 0;
    let level = 1;
    let lives = 5;
    let highscore = getHighscore(); // Lấy điểm cao nhất từ localStorage
    
    let explosionPositions = [];


    function keyDownHandler(e) {
        if (e.keyCode >= 65 && e.keyCode <= 90) {
            const typedLetter = String.fromCharCode(e.keyCode).toUpperCase();

            const matchingLetters = letters.filter(fallingLetter => typedLetter === fallingLetter.char);

            if (matchingLetters.length > 0) {
                matchingLetters.forEach(fallingLetter => {
                    const indexToRemove = letters.indexOf(fallingLetter);
                    letters.splice(indexToRemove, 1);
                    typingCorrectSound.play();
                    explosionPositions.push({ x: fallingLetter.x + letter.width / 2, y: fallingLetter.y + letter.height / 2 });
                });

                score += matchingLetters.length;

                if (score > highscore) {
                    highscore = score;
                    setHighscore(highscore);
                }

                if (explosionPositions.length > 0) {
                    triggerExplosion();
                }
            } else {
                typingWrongSound.play(); // Chơi âm thanh khi gõ sai
            }
        }
    }


    function createLetter() {
        if (Math.random() < letter.probability) {
            const x = Math.random() * (canvas.width - letter.width);
            const speed = letter.speed;
            const char = String.fromCharCode(65 + Math.floor(Math.random() * 26));
            letters.push({ x, y: -letter.height, char, speed });
        }
    }

    function removeLetters() {
        for (let i = letters.length - 1; i >= 0; i--) {
            const fallingLetter = letters[i];

            fallingLetter.y += fallingLetter.speed;

            if (fallingLetter.y > canvas.height) {
                letters.splice(i, 1);
                lives--;

                if (lives === 0) {
                    var currentTimestamp = new Date().getTime();
                    $.ajax({
                        type: 'post',
                        url: '/TypingGame/HandlerSaveResultGame',
                        data: {
                            score: score,
                            timestamp: currentTimestamp,
                            level: level,
                            exerciseid: 8,
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
                    alert("Score your: " + score + ". You want restart game!")
                    resetGame();
                }
            }
        }
    }

    function draw() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        ctx.font = letter.font;
        ctx.fillStyle = letter.color;
        for (const fallingLetter of letters) {
            ctx.fillText(fallingLetter.char, fallingLetter.x + 4, fallingLetter.y + letter.height - 4);
        }

        for (const particle of explosion.particles) {
            ctx.beginPath();

            const randomColor = `rgba(${Math.random() * 255},${Math.random() * 255},${Math.random() * 255}, 0.7)`;

            ctx.fillStyle = randomColor;
            ctx.arc(particle.x, particle.y, explosion.particleSize, 0, 2 * Math.PI);
            ctx.fill();
            ctx.closePath();
        }

        ctx.font = '32px Arial';
        ctx.fillStyle = '#fff';
        ctx.fillText('Score: ' + score, 10, 30);
        ctx.fillText('Highscore: ' + highscore, 10, 70);
        ctx.fillText('❤: ' + lives, canvas.width - 100, 30);
    }

    function resetGame() {
        letters = [];
        score = 0;
        lives = 5;
    }

    function setCanvasDimensions() {
        canvas.width = $('.container-game').width();
        canvas.height = $('.container-game').height();
    }

    function gameLoop() {
        setCanvasDimensions();
        createLetter();
        removeLetters();
        updateExplosion();
        draw();

        animationFrameId = requestAnimationFrame(gameLoop);
    }

    function triggerExplosion() {
        explosion.particles = [];
        explosionPositions.forEach(position => {
            for (let i = 0; i < explosion.particleCount; i++) {
                explosion.particles.push({
                    x: position.x,
                    y: position.y,
                    speedX: (Math.random() - 0.5) * explosion.explosionSpeed,
                    speedY: (Math.random() - 0.5) * explosion.explosionSpeed
                });
            }
        });

        explosionPositions = [];
    }

    function updateExplosion() {
        for (let i = explosion.particles.length - 1; i >= 0; i--) {
            const particle = explosion.particles[i];
            particle.x += particle.speedX;
            particle.y += particle.speedY;
            if (particle.x < 0 || particle.x > canvas.width || particle.y < 0 || particle.y > canvas.height) {
                explosion.particles.splice(i, 1);
            }
        }
    }

    function getHighscore() {
        const storedHighscore = localStorage.getItem("high-score-game3");
        return storedHighscore ? JSON.parse(storedHighscore) : 0;
    }

    function setHighscore(newHighscore) {
        localStorage.setItem("high-score-game3", JSON.stringify(newHighscore));
    }

    setCanvasDimensions();
});
