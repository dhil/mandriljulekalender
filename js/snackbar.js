// Snackbar/notification logic
const Snackbar = (function () {
  let snackbarTimeout = null;
  let updateProgressTimeout = null;
  let snackbar = document.getElementById("snackbar");
  let snackbarProgress = document.getElementById("snackbar-progress");
  const fadeIn = function (target) {
    target.style.opacity = 0.85;
    return;
  };
  const fadeOut = function (target) {
    target.style.opacity = 0;
    return;
  };
  const startProgress = function () {
    snackbarProgress.style.transition = "";
    snackbarProgress.style.width = "100%";
    snackbarProgress.style.opacity = 1;
    return setTimeout(function () {
      snackbarProgress.style.transition = "8s linear";
      snackbarProgress.style.width = "0%";
    }, 10);
  };
  const notify = function (quote, seconds = 3) {
    if (snackbarTimeout !== null) {
      clearTimeout(snackbarTimeout);
    }
    if (updateProgressTimeout !== null) {
      clearTimeout(updateProgressTimeout);
    }
    fadeIn(snackbar);
    updateProgressTimeout = startProgress();
    let snackbarText = document.getElementById("snackbar-text");
    snackbarText.innerHTML = quote.text;
    let snackbarOrigin = document.getElementById("snackbar-origin");
    snackbarOrigin.innerHTML = "- " + quote.character;

    // After `seconds` hide the snackbar
    // Store timeout to cancel it if user is a child and spams the button
    snackbarTimeout = setTimeout(function () {
      fadeOut(snackbar);
    }, seconds * 1000);
    return;
  };
  return { notify: notify };
})();
window.Snackbar = Snackbar;
