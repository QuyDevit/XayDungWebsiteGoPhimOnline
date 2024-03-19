import { initializeApp } from "https://www.gstatic.com/firebasejs/10.8.0/firebase-app.js";
import { getAuth, signInWithPopup, GoogleAuthProvider, FacebookAuthProvider} from "https://www.gstatic.com/firebasejs/10.8.0/firebase-auth.js";
const firebaseConfig = {
    apiKey: "AIzaSyA4NUDSuzDBU44h2KIxBLu4Mxgk1Zu3se0",
    authDomain: "login-besttyping.firebaseapp.com",
    projectId: "login-besttyping",
    storageBucket: "login-besttyping.appspot.com",
    messagingSenderId: "958669352120",
    appId: "1:958669352120:web:6d148aa63f3a5317a1d84d"
};

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

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
auth.languageCode = 'en';
//Login with gg
const provider = new GoogleAuthProvider();
const googleLogin = $(".login-gg-btn");
googleLogin.click(function () {
    signInWithPopup(auth, provider)
        .then((result) => {           
            const user = result.user;
            $.ajax({
                type: "POST",
                url: '/Account/LoginWithSocial',
                data: {
                    fullname: user.displayName,
                    email: user.email,
                    avatar: user.photoURL
                },
                success: function (response) {
                    if (response.code == 200) {
                        $(".modal_overlay").trigger('click');
                        ToastSuccess("Thành công", response.msg, true)
                    } else if (response.code == 400) {
                        ToastSuccess("Thành công", response.msg, true)
                    } else {
                        TToastError(response.msg);
                    }
                },
                error: function (err) {
                    commonError(err);                  
                }
            });
        }).catch((error) => {
            const errorCode = error.code;
            const errorMessage = error.message;
        });
})

const providerfb = new FacebookAuthProvider();
const facebookLogin = $(".login-fb-btn");

facebookLogin.click(function () {
    signInWithPopup(auth, providerfb)
        .then((result) => {
            const user = result.user;
            // This gives you a Facebook Access Token. You can use it to access the Facebook API.
            const credential = FacebookAuthProvider.credentialFromResult(result);
            const token = credential.accessToken;
            const imageURL = `https://graph.facebook.com/me/picture?height=200&width=200&access_token=${token}`;

            $.ajax({
                type: "POST",
                url: '/Account/LoginWithSocial',
                data: {
                    fullname: user.displayName,
                    email: user.email,
                    avatar: imageURL
                },
                success: function (response) {
                    if (response.code == 200) {
                        $(".modal_overlay").trigger('click');
                        ToastSuccess("Thành công", response.msg, true)
                    } else if (response.code == 400) {
                        ToastSuccess("Thành công", response.msg, true)
                    } else {
                        TToastError(response.msg);
                    }
                },
                error: function (err) {
                    commonError(err);
                }
            });
        })
        .catch((error) => {
            const errorCode = error.code;
            const errorMessage = error.message;
        });
});