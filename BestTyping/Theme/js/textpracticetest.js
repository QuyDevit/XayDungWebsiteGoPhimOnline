$(document).ready(function () {
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
                    location.href = '/Home/MyTextPractice';
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

    $("#btn-checkfavorite").click(function () {
        $.ajax({
            type: "post",
            url: "/TextPractice/LikeTextPracticeByUser",
            data: {
                codejoin: codejoin,  
            },
            success: function (response) {
                if (response.code === 200) {
                  
                } else {
                    ToastError(response.msg)
                }
            }
        })
    })
    function insertText(text) {
        const container = $("#text-container");
        const containerWidth = container.innerWidth(); // Chiều rộng bao gồm padding của container
        let lineCount = 1;
        let wordCount = 1;

        let currentLine = $("<div>").addClass("word-line line" + lineCount);
        container.append(currentLine);

        let currentLineWidth = 0; // Độ rộng hiện tại của dòng
        let isFirstWord = true;

        const sentences = text.split('\n'); // Tách văn bản thành các câu dựa vào dấu xuống dòng
        const totalSentences = sentences.length;

        sentences.forEach((sentence,index) => {
            const words = sentence.trim().split(" "); // Tách từng từ trong câu
            let hasWord = false; // Biến đánh dấu xem câu hiện tại có từ nào không

            words.forEach(wordText => {
                if (wordText.trim() != "") {
                    const word = $("<span>").addClass("word").attr("id", "word" + wordCount).text(wordText);
                    wordCount++;

                    if (isFirstWord) {
                        word.addClass("highlight");
                        isFirstWord = false;
                    }
                    currentLine.append(word);

                    // Tính toán chiều rộng của từ và kiểm tra xem có vượt quá chiều rộng tối đa không
                    const wordWidth = word.outerWidth(true); // Bao gồm cả margin
                    currentLineWidth += wordWidth;

                    hasWord = true;

                    if (currentLineWidth > containerWidth) {
                        // Nếu vượt quá, thêm dòng mới và reset lại các biến
                        currentLine = $("<div>").addClass("word-line line" + (++lineCount));
                        container.append(currentLine);
                        currentLine.append(word);
                        currentLineWidth = wordWidth;
                        isFirstWord = false;
                    }
                }
            });
            if (hasWord && index < totalSentences - 1) {
                // Thêm dòng mới sau khi xử lý mỗi câu (gặp dấu xuống dòng)
                currentLine = $("<div>").addClass("word-line line" + (++lineCount));
                container.append(currentLine);
                currentLineWidth = 0; // Reset chiều rộng của dòng mới
                isFirstWord = false;
            }
        });
    }

    insertText(text);

    var startTime = 0;
    var typingStarted = false;
    var currentWord = 1;
    var currenWordline = 1;
    var checkCountWordInWordLine = 0;
    var checkNextWordLine = 0;
    var sumWordLine = 0;
    var timeCompleted = 0;
    var arrWordResult = [];

    function highlightNextWord() {
        $(".highlight").removeClass("highlight");
        $("#word" + currentWord).addClass("highlight");
    }

    $("#inputfield").on("input", function (event) {
        if (!typingStarted) {
            typingStarted = true;
            startTimer();
            sumWordLine = $("#text-container").children(".word-line").length;
            checkCountWordInWordLine = $(".word-line.line" + currenWordline).children(".word").length;
            checkNextWordLine = checkCountWordInWordLine+1;
        }

        var charEntered = $(this).val().slice(-1);

        if (/\s/g.test(charEntered)) {
            if ($("#inputfield").val().length == 1) {
                $("#inputfield").val("");
            } else {
                checkWord();
                if (currentWord == checkNextWordLine && sumWordLine - currenWordline > 3) {
                    $(".word-line.line" + currenWordline).slideUp();
                    currenWordline++;
                    checkCountWordInWordLine = $(".word-line.line" + currenWordline).children(".word").length;
                    checkNextWordLine += checkCountWordInWordLine;
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
            arrWordResult.push(1);

            $("#word" + currentWord).addClass("correct");
        } else {
            $("#word" + currentWord).removeClass("checkhighlight").addClass("wrong");
            arrWordResult.push(0);
        }
        arrWordResult.push(inputText);
        currentWord++;

        if (currentWord <= textlength) {
            highlightNextWord();
        } else {
            timeCompleted = startTime;
            clearInterval(timerInterval);
            disableInput();

            let rating = 0;
            $("#completed").text(parseInt(completed) + 1);

            $('input[name="rate"]').on('click', function () {
                rating = $(this).val();
                $.ajax({
                    type: "post",
                    url: "/TextPractice/SaveResultTypingTextPractice",
                    data: {
                        codejoin: codejoin,
                        rating: rating
                    },
                    success: function (response) {
                        if (response.code === 200) {
                            $("#rating").html(`${response.avgratingnew} <i class="fa fa-star icon-custom" aria-hidden="true"></i>`);
                        } else {
                            ToastError(response.msg)
                        }
                    }
                })
            });
        }
        $("#inputfield").val("");
    }

    function disableInput() {
        $(".practice-typing__wapper").slideUp();
        $("#inputfield").prop("disabled", true);
        $(".highlight").removeClass("highlight");
        $(".checkhighlight").removeClass("checkhighlight");
        showResult();
        $(".practice__wrapper-result").slideToggle();
        if (checkuser == true) {
            $(".practice_wrapper-rating").slideToggle();
        }        
    }

    function showResult() {
        const reslut = $("#showresult");
        reslut.empty();
        for (let i = 0; i < arrWordResult.length; i+=2) {
            if (arrWordResult[i] == 1) {
                reslut.append("<span class='green'>" + arrWordResult[i + 1] + "</span>\n")
            } else {
                reslut.append("<span class='red'>" + arrWordResult[i + 1] + "</span>\n")
            }
            
        }
    }

    function startTimer() {
        timerInterval = setInterval(function () {
            startTime++;
            updateTimer();         
        }, 1000);
    }

    function updateTimer() {
        var minutes = Math.floor(startTime / 60);
        var seconds = startTime % 60;
        var formattedTime = "0" + minutes + ":" + (seconds < 10 ? "0" + seconds : seconds);
        $("#timer").text(formattedTime);
    }
})