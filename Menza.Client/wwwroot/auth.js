import {initializeApp} from "https://www.gstatic.com/firebasejs/9.17.2/firebase-app.js";
import {getAuth, signInWithPopup, GoogleAuthProvider, onAuthStateChanged}
    from "https://www.gstatic.com/firebasejs/9.17.2/firebase-auth.js";

const firebaseConfig = {
    apiKey: "AIzaSyANG9n7O7y1V3CxT91xus6V8mANt8A8JRs",
    authDomain: "menza-ejg.firebaseapp.com",
    projectId: "menza-ejg",
    storageBucket: "menza-ejg.appspot.com",
    messagingSenderId: "103920663096",
    appId: "1:103920663096:web:fe044b10d8b2c681a7c4cd"
};

initializeApp(firebaseConfig);
const auth = getAuth();

export const signOut = auth.signOut;

onAuthStateChanged(auth, async (user) => {
    if (user) {
        if (!user.email.endsWith("@eotvos-tata.org")) {
            await signOut();
            alert("Az eotvos-tata.org végű email címedet használd!");
            return;
        }
        console.log(user.email);
    } else {
        console.log("Signed out");
    }
});

export async function signIn() {
    const provider = new GoogleAuthProvider();
    await signInWithPopup(auth, provider)
}

export function onAccessTokenChanged(callback) {
    onAuthStateChanged(auth, async (user) => {
        callback(await user?.getIdToken(true));
    });
}