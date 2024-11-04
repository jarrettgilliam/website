function getResource(
    baseUri: string,
    onSuccess: (r: string) => void,
    onError?: () => void)
    : void {
    grecaptcha.ready(function () {
        grecaptcha.execute('6LdQ4uIgAAAAAAXQbFEfBlACvn8lRh6txqQhcy_6', {action: 'submit'}).then(async function (token: string) {
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
    getResource('api/resource/email', (r: string) => {
        document.location.href = r;
    });
}

function getResume() {
    const w = window.open();
    if (!w) return;

    getResource('api/resource/resume',
        (r: string) => { w.location = r; },
            () => { w.close(); });
}

document.getElementById("btn-resume")?.addEventListener("click", getResume);
document.getElementById("btn-email")?.addEventListener("click", getEmail);
