import { initializeApp } from "https://www.gstatic.com/firebasejs/9.23.0/firebase-app.js";
import { getMessaging, getToken, onMessage } from "https://www.gstatic.com/firebasejs/9.23.0/firebase-messaging.js";

const firebaseConfig = {
    apiKey: "AIzaSyBmZHzLlJJtySB1GzE2Z1L1R5WeMyOhmOs",
    authDomain: "fieldmanager-5d702.firebaseapp.com",
    projectId: "fieldmanager-5d702",
    storageBucket: "fieldmanager-5d702.appspot.com",
    messagingSenderId: "731880885372",
    appId: "1:731880885372:web:032b4a4f1664a055598df9",
    measurementId: "G-C14JZQB50Q"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

const messaging = getMessaging(app);

getToken(messaging, { vapidKey: "BMVpqA7SQY38qISpilLcMHa4C14kkrb9yZna_j8jFB293WkSbaddOeSf8vj8ORyAE6bD0u3PcrosVmXe4xwRUck" }).then((currentToken) => {
    if (currentToken) {
        console.log('token is ' + currentToken);
       // setTokenSentToServer(true);
        saveToken(currentToken);

    } else {
        // Show permission request UI
        console.log('No registration token available. Request permission to generate one.');
       // setTokenSentToServer(false);
    }
}).catch((err) => {
    console.log('An error occurred while retrieving token. ', err);
   // setTokenSentToServer(false);
});

function setTokenSentToServer(sent) {
    window.localStorage.setItem('sentToServer', sent ? 1 : 0);
}
function isTokenSentToServer() {
    return window.localStorage.getItem('sentToServer') === '1';
}

/*function requestPermission() {
    console.log('Requesting permission...');
    if (isTokenSentToServer()) {
        console.log("already granted");
    } else {

        try {
            Notification.requestPermission().then(function (permission) { console.log('permiss', permission) });
            getRegtoken();
        } catch (e) {
            console.log(e);
        }


    }
}*/

onMessage(messaging, (payload) => {

    console.log('Message received. ', payload);

    if (!("Notification" in window)) {

        // Check if the browser supports notifications
        alert("This browser does not support desktop notification");
    } else if (Notification.permission === "granted") {

        // Check whether notification permissions have already been granted;
        // if so, create a notification
        showNotification(payload.notification.title, payload.notification.body);
        // …
    } else if (Notification.permission !== "denied") {

        // We need to ask the user for permission
        Notification.requestPermission().then((permission) => {

            // If the user accepts, let's create a notification
            if (permission === "granted") {
                console.log('Final hit');
                //const notification = new Notification(payload.notification.title, notificationOptions);
                // …
            }
        });
    }
});

function saveToken(currentToken) {
    $.ajax({
        type: "POST",
        url: "/Account/SavePushToken",
        data: { "token": currentToken },
        success: function (response) {
            console.log(response);
        },
        failure: function (response) {
            console.log(response.responseText);
        },
        error: function (response) {
            console.log(response.responseText);
        }
    });
}

function showNotification(title, message) {
    console.log('Title :' + title + "message :" + message);

    const notificationOptions = {
        body: message
    };
    new Notification(title, notificationOptions);
}
