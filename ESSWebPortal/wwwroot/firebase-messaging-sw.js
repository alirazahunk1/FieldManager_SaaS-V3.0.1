importScripts("https://www.gstatic.com/firebasejs/9.23.0/firebase-app-compat.js");
importScripts("https://www.gstatic.com/firebasejs/9.23.0/firebase-messaging-compat.js");

const firebaseConfig = {
    apiKey: "AIzaSyBmZHzLlJJtySB1GzE2Z1L1R5WeMyOhmOs",
    authDomain: "fieldmanager-5d702.firebaseapp.com",
    projectId: "fieldmanager-5d702",
    storageBucket: "fieldmanager-5d702.appspot.com",
    messagingSenderId: "731880885372",
    appId: "1:731880885372:web:032b4a4f1664a055598df9",
    measurementId: "G-C14JZQB50Q"
};

firebase.initializeApp(firebaseConfig);

const messaging = firebase.messaging();

messaging.onBackgroundMessage((payload) => {
    console.log(
        'Received background message ',
        payload
    );
    // Customize notification here
    const notificationTitle = payload.notification.title;
    const notificationOptions = {
        body: payload.notification.body,
        icon: '/firebase-logo.png'
    };

    self.registration.showNotification(notificationTitle, notificationOptions);
});