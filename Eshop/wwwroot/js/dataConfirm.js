window.addEventListener("load", () => {
    const elements = docuent.querySelectorAll("a[data-confirm], button[data-confirm], input[data-confirm");
    for (const element of elements) {
        element.addEventListener("click", e => {
            const confirmed = confirm(element.getAttribute("data-confirm"));
            if (!confirmed)
                e.preventDefault();
        });
    }
});