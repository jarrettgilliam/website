'use strict'

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
    getResource('api/resource/email', r => {
        document.location.href = r;
    });
}

function getResume() {
    const w = window.open();
    getResource('api/resource/resume',
            r => { w.location = r; },
            () => { w.close(); });
}

document.getElementById("btn-resume").addEventListener("click", getResume);
document.getElementById("btn-email").addEventListener("click", getEmail);
