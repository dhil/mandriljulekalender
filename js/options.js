// Menu and options logic
const Options = (function () {
  const menu = document.getElementById("menu");
  const menuButton = document.getElementById("menuButton");
  let menuOpen = false;
  let showMenu = function () {
    menu.style.width = "auto";
    menuOpen = true;
  };
  let hideMenu = function (event) {
    if (event.target === menuButton) return;
    menu.style.width = "0";
    menuOpen = false;
  };
  const toggleMenu = function () {
    if (menuOpen) {
      hideMenu();
    } else {
      showMenu();
    }
  };
  const toggleSnow = function () {
    let toggleSnowOption = document.getElementById("toggleSnowOption");
    if (Snow.isActive()) {
      Snow.stop();
      toggleSnowOption.innerHTML = "Vis Sne";
    } else {
      Snow.start();
      toggleSnowOption.innerHTML = "Skjul Sne";
    }
    hideMenu();
  };
  return {
    toggleMenu: toggleMenu,
    toggleSnow: toggleSnow,
    hideMenu: hideMenu,
    showMenu: showMenu,
  };
})();
window.Options = Options;
