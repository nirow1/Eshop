window.addEventListener("load", () => {
    const messages = document.querySelectorAll('.flash-messages-container > *');
    const duration = 10000;

    for (const message of messages) {
        const alert = new bootstrap.Alert(message);
        setTimeout(() => alert.close(), duration);
    }
});