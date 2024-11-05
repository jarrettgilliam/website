function throwIfNull<T>(value: T | null): T {
    if (value === null) {
        throw new Error(`Value of type ${typeof value} is null`);
    }

    return value;
}

function grecaptchaReady(): Promise<void> {
    return new Promise(resolve => grecaptcha.ready(() => resolve()));
}

async function getResource(endpoint: string) : Promise<Response> {
    await grecaptchaReady();
    const token = await grecaptcha.execute('6LdQ4uIgAAAAAAXQbFEfBlACvn8lRh6txqQhcy_6', { action: 'submit' });
    const response = await fetch(`${endpoint}?token=${token}`);

    if (!response.ok) {
        setTimeout(() => alert("HTTP-Error: " + response.status), 1);
    }

    return response;
}

async function getEmail() {
    const r = await getResource('api/resource/email');
    if (r.ok) {
        document.location.href = await r.text();
    }
}

async function getResume() {
    const w = throwIfNull(window.open());
    const r = await getResource('api/resource/resume');

    if (r.ok) {
        w.location = await r.text();
    } else {
        w.close();
    }
}

throwIfNull(document.getElementById("btn-resume")).addEventListener("click", getResume);
throwIfNull(document.getElementById("btn-email")).addEventListener("click", getEmail);
