document.addEventListener("DOMContentLoaded", () => {
    const buttons = document.querySelectorAll("button");
    buttons.forEach((button, index) => {
        button.setAttribute("data-id", `button-${index + 1}`);
    });
});