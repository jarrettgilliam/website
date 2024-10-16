'use strict'

document.addEventListener('DOMContentLoaded', function () {
    const s = document.createElement('script');
    s.src = "https://www.google.com/recaptcha/api.js?render=6LdQ4uIgAAAAAAXQbFEfBlACvn8lRh6txqQhcy_6";
    document.body.appendChild(s);
});

document.getElementById("btn-getResume").addEventListener("click", getResume);
document.getElementById("btn-getEmail").addEventListener("click", getEmail);

function getResource(baseUri, onSuccess, onError) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6LdQ4uIgAAAAAAXQbFEfBlACvn8lRh6txqQhcy_6', {action: 'submit'}).then(async function (token) {
            const response = await fetch(`${baseUri}?token=${token}`);

            if (response.ok) {
                const resourceLink = await response.text();
                onSuccess(resourceLink);
            } else {
                if (onError) {
                    onError();
                }
                alert("HTTP-Error: " + response.status);
            }
        });
    });
}

function getEmail() {
    getResource('api/Resource/GetEmail', r => {
        document.location.href = r;
    });
}

function getResume() {
    const w = window.open();
    getResource('api/Resource/GetResume',
            r => { w.location = r; },
            () => { w.close(); });
}