// Utility/helper functions
// ...to be filled with relevant code from mandril.js...

// Cookie handling utility
const Cookie = (function () {
  const get = function (name) {
    // Split into key-value pairs
    let entries = document.cookie.split(";");

    // Iterate the key-value pairs
    for (let i = 0; i < entries.length; i++) {
      let keyValuePair = entries[i].split("=");
      if (name == keyValuePair[0].trim()) {
        return decodeURIComponent(keyValuePair[1]);
      }
    }

    // Return null if not found
    return null;
  };
  const set = function (key, value, expirationDate) {
    document.cookie =
      key +
      "=" +
      encodeURIComponent(value) +
      ";expires=" +
      expirationDate +
      ";SameSite=lax;path=/";
    return;
  };
  const has = function (name) {
    return get(name) !== null;
  };
  const forget = function (key) {
    document.cookie =
      key + "=;expires=Thu, 01 Jan 1970 00:00:00 UTC;SameSite=lax;path=/";
    return;
  };
  return { get: get, set: set, has: has, forget: forget };
})();
