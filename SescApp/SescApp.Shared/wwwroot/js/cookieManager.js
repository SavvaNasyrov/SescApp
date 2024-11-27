const setCookie = function setCookie(name, value, days, secure = false, sameSite = 'Strict') {
    if (value === undefined) {
        console.warn(`Cookie "${name}" не установлен, так как значение не задано.`);
        return;
    }

    let expires = "";
    if (days) {
        const date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }

    document.cookie = name + "=" + encodeURIComponent(value) + expires + "; path=/" +
        "; SameSite=" + sameSite +
        (secure ? "; Secure" : "");
}

const getCookie = function getCookie(cookieName) {
    let cookies = document.cookie.split(";").map(cookie => {
        let [key, value] = cookie.trim().split("=");
        return [key, value];
    });

    let target = cookies.find(([key]) => key === cookieName);

    return target ? target[1] : null;
}