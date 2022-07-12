'use strict'

function getResource(baseUri, callback) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6LdQ4uIgAAAAAAXQbFEfBlACvn8lRh6txqQhcy_6', {action: 'submit'}).then(async function (token) {
            let response = await fetch(`${baseUri}?token=${token}`);

            if (response.ok) {
                let resourceLink = await response.text();
                callback(resourceLink);
            } else {
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
    getResource('api/Resource/GetResume', r => {
        window.open(r);
    });
}