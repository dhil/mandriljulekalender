// Calendar, Door, Quotes, and Page logic
const $stateName = "calendarState";
let $calendar = null;

// Calendar data model
const Calendar = (function () {
  /*
   * The calendar state is encoded as an array of natural numbers
   * (layout) and a bit vector (states). The layout array maps grid
   * positions to actual door numbers, whilst the states bit vector
   * encodes whether the nth grid cell has been opened.
   */
  const make = function (resetDate, layout, states) {
    if (layout == null) {
      layout = [...Array(25).keys()].map((i) => i + 1);
      for (let i = layout.length - 1; i > 0; i--) {
        let j = Math.floor(Math.random() * (i + 1));
        [layout[i], layout[j]] = [layout[j], layout[i]];
      }
    }
    if (states == null) states = 0;
    if (resetDate == null)
      resetDate = new Date(new Date().getFullYear() + 1 + "-01-31");
    return { resetDate: resetDate, layout: layout, states: states };
  };
  const isOpen = function (calendar, doorN) {
    return (calendar.states & (1 << doorN)) != 0;
  };
  const allClosed = function (calendar) {
    return calendar.states === 0;
  };
  const open = function (calendar, doorN) {
    if (calendar.locked) return;
    calendar.states |= 1 << doorN;
    return;
  };
  const doorAt = function (calendar, position) {
    return calendar.layout[position];
  };

  // Serialisation and deserialisation
  const serialise = function (calendar) {
    // The layout array is encoded as a string, where each single
    // digit number is prefixed with a zero.
    let layoutEncoded = calendar.layout
      .map((door) => (door < 10 ? "0" + door : door + ""))
      .join("");
    let stateEncoded = [
      layoutEncoded,
      calendar.states,
      calendar.resetDate,
    ].join(",");
    return stateEncoded;
  };
  const deserialise = function (serialised) {
    // For backwards compatibility with the previous serialisation
    // format (prior to commit
    // 1425c4029bf393cd09cc77c3cc5e0742435e027a) we need to check
    // whether `resetDate` is non-null in which case we
    // instantiate a date object, otherwise we defer instantiation
    // to the `make` function.
    let [doorLayout, doorStates, resetDate] = serialised.split(",");
    if (resetDate != null) resetDate = new Date(resetDate);
    doorLayout = doorLayout.match(/.{1,2}/g);
    return make(
      resetDate,
      doorLayout.map((door) => door | 0),
      doorStates | 0
    );
  };
  const persist = function (key, calendar) {
    const expirationDate = new Date($calendar.resetDate);
    Cookie.set(key, serialise(calendar), expirationDate);
    return;
  };
  return {
    make: make,
    isOpen: isOpen,
    allClosed: allClosed,
    open: open,
    doorAt: doorAt,
    serialise: serialise,
    deserialise: deserialise,
    persist: persist,
  };
})();

const Door = (function () {
  const descriptions = [
    "Casper får besøg af Asger Debono, som lægger lækkerier ud på foderbrættet.",
    "Casper får besøg af Jørgen Ingemann, der får besøg af Julemanden som den første hvert år.",
    "Casper får endnu engang besøg af Asger Debono, der ved hvad der rører sig inden for halm.",
    "Casper får besøg af Chacha Jublenn, der ved alt om post.",
    "Casper får besøg af Hr. Vellemand, der aldrig har fået lige det han ønskede sig.",
    "Casper får besøg af Cosy Joe, en af de 8000 vise mænd.",
    "Casper får besøg af Doby Slinger, som aldrig bliver fortalt noget.",
    "Casper får besøg af Thor Vitter Rynkemås, der fortæller hvordan man tilbereder en juleand.",
    "Et herligt gensyn med Thor Vitter Rynkemås, der denne gang viser, hvordan man fjerner nougatpletter.",
    "Casper får besøg af Stofa Donnington der har røven fuld af penge.",
    "Casper får besøg af Julian Tisange, som mener at de danske juletraditioner bør ændres.",
    "Stofa Donnington kommer igen på besøg, denne gang fortæller han om juletraditioner.",
    "Timothy Birthebæk kigger forbi, og fortæller om det at være overvægtig, spise for to, og synge i en trio med én anden.",
    "Julian kommer endnu gang på besøg for at snakke mere om ændring af juletraditioner.",
    "Casper får besøg af Miffer Hansepajuk, der fortæller om sin markedsføring af en ny adventskrans.",
    "Endnu et gensyn med Julian til en snak om juletraditioner.",
    "I dag er der historie oplæsning.",
    "Casper får besøg af Herbert Røffel, der fortæller om livet som kravlenisse.",
    "Casper fortæller, hvordan man kan forberede sig til jul.",
    "Casper fortæller om alle de dejlige ting, som man kan lave med vat.",
    "Casper fortæller glöggens historie.",
    "Casper fortæller om de nye øl, der kommer til jul næste år.",
    "Casper fortæller, hvordan man sender julekort.",
    "Casper får besøg af englen Fergie.",
    "Casper forsøger at oprette forbindelse til rumbukken Fyffy.",
  ];
  const videoIds = [
    "whh2aljch8w",
    "fX3vzQKRdps",
    "Cc3aivx_uNU",
    "oG8oFVE7Gpo",
    "WKO3FP-SLzY",
    "P5hCu2JTQcs",
    "XlYg3LWzzIo",
    "xNC5Le-ZPjo",
    "XUfNMgBBQzk",
    "ZWjsFCGFZVk",
    "JPxhyGNhOsY",
    "LWoOeqBRo1g",
    "3QA55AJjMJ0",
    "FWSucptTJTc",
    "j89fnmYings",
    "Cg-GvBaCSj0",
    "XcvuLTcxZ8g",
    "Q0AmcPbHqEA",
    "nqfmM-aK8vY",
    "i5TTySKyWxg",
    "jz59tiZTe1E",
    "We5Si8jn3eI",
    "0uARiZkTGSA",
    "8kMTRDXxBVM",
    "OH_c3VR3O6M",
  ];
  const OPEN = 0,
    ALREADY_OPEN = 1,
    TOO_EARLY = 2;
  const open = function (gridPosition) {
    // Doors are coded as checkboxes, where we interpret "checked"
    // as denoting the door as "open", otherwise it is
    // "closed".
    // Some rules:
    // 1) A door that has been opened cannot be closed again.
    // 2) A door can only be opened if its number/label is less
    // than or equal to the current day in December...
    // Moreover, if it is January and at least one door is open,
    // then you may open all other doors.
    let doorNumber = Calendar.doorAt($calendar, gridPosition);
    if (Calendar.isOpen($calendar, gridPosition)) {
      return {
        tag: ALREADY_OPEN,
        episode: doorNumber,
        episodeDescription: descriptions[doorNumber - 1],
        videoId: videoIds[doorNumber - 1],
      };
    } else {
      let currentDate = new Date().getTime();
      let doorDate = new Date(new Date().getFullYear(), 11, doorNumber);
      if (
        $development ||
        doorDate <= currentDate ||
        (!Calendar.allClosed($calendar) && new Date().getMonth() === 0)
      ) {
        Calendar.open($calendar, gridPosition);
        return { tag: OPEN };
      } else {
        return { tag: TOO_EARLY };
      }
    }
    return;
  };
  const make = function (gridPosition, doorNumber, isOpen) {
    let front = document.createElement("div");
    front.classList.add("front");
    front.appendChild(document.createTextNode(doorNumber.toString()));
    let rear = document.createElement("div");
    rear.classList.add("back");
    let door = document.createElement("div");
    door.classList.add("door");
    door.appendChild(front);
    door.appendChild(rear);
    let input = document.createElement("input");
    input.setAttribute("type", "checkbox");
    input.checked = isOpen;
    input.onclick = function (_event) {
      return Page.openDoor(input, gridPosition);
    };
    let label = document.createElement("label");
    label.appendChild(input);
    label.appendChild(door);
    let doorN = document.createElement("div");
    let doorId = gridPosition + 1;
    doorN.setAttribute("id", "door-" + doorId);
    doorN.classList.add("door-" + doorNumber);
    doorN.appendChild(label);
    return doorN;
  };
  return {
    open: open,
    make: make,
    Response: { OPEN: OPEN, ALREADY_OPEN: ALREADY_OPEN, TOO_EARLY: TOO_EARLY },
  };
})();

const Quotes = (function () {
  let quotes = null;
  const load = async function () {
    try {
      const response = await fetch("static/assets/quotes.json");
      if (!response.ok) {
        throw new Error("Failed to fetch JSON");
      }
      quotes = await response.json();
    } catch (error) {
      console.error("Error fetching/parsing JSON:", error);
    }
  };
  let last = -1;
  const getRandom = function () {
    const i = Math.floor(Math.random() * quotes.length);
    last = i == last ? (i + 1) % quotes.length : i;
    return quotes[last];
  };
  return { load: load, getRandom: getRandom };
})();

// Page view & controller
const Page = (function () {
  const render = function () {
    let doors = document.createDocumentFragment();
    for (let i = 0; i < 25; i++) {
      doors.appendChild(
        Door.make(
          i,
          Calendar.doorAt($calendar, i),
          Calendar.isOpen($calendar, i)
        )
      );
    }
    let calendarNode = document.getElementById("calendar");
    calendarNode.innerHTML = "";
    calendarNode.appendChild(doors);
    return;
  };
  const initialise = function () {
    if (Cookie.has($stateName)) {
      $calendar = Calendar.deserialise(Cookie.get($stateName));
    } else {
      $calendar = Calendar.make(null, null, null);
      Calendar.persist($stateName, $calendar);
    }
    Quotes.load();
    Achievements.init($calendar.resetDate);
    window.addEventListener("click", function (event) {
      Options.hideMenu(event);
    });
    return render();
  };
  const reset = function () {
    Cookie.forget($stateName);
    Achievements.reset();
    return location.reload();
  };
  let locked = false;
  const freeze = function () {
    locked = true;
    return;
  };
  const unfreeze = function () {
    locked = false;
    return;
  };
  const checkIsModal = function (target) {
    if (target.id === "videoplayer") {
      return true;
    }
    if (target.localName === "body") {
      return false;
    }
    return checkIsModal(target.parentNode);
  };
  function showVideoPlayer(episode, description, videoId) {
    // Freeze and blur background
    freeze();
    document.body.classList.add("dialogIsOpen");
    // Get the modal
    let modal = document.getElementById("videoplayer");
    // Get the <span> element that closes the modal
    let closeButton = document.getElementsByClassName("close")[0];
    // Add episode description along with an embedded YouTube video frame to the modal body
    let modalBody = document.getElementsByClassName("modal-body")[0];
    modalBody.innerHTML =
      "<p>" +
      description +
      '</p><iframe class="embedded-player" src="https://www.youtube.com/embed/' +
      videoId +
      '" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>';
    
    document.getElementById("video-title").innerHTML = "Afsnit " + episode;
    // When the user clicks on <span> (x), close the modal
    let closeModal = function () {
      modal.style.display = "none";
      modalBody.innerHTML = "";
      window.onclick = null;
      document.body.className = document.body.className.replace(
        "dialogIsOpen",
        ""
      );
      Achievements.trackVideoClosed(videoId, true)
      unfreeze();
    };
    closeButton.onclick = closeModal;
    window.onclick = function (_event) {
      if (_event.target.type === "checkbox") {
        return;
      }
      if (!checkIsModal(_event.target)) {
        // if we are clicking outside the modal close the modal
        _event.preventDefault();
        closeModal();
      }
    };
    modal.style.display = "block";
    Achievements.trackVideoShown(videoId)
  }
  const openDoor = function (cell, position) {
    cell.checked = Calendar.isOpen($calendar, position); // inhibit the event
    if (locked) return;
    let response = Door.open(position);
    switch (response.tag) {
      case Door.Response.OPEN:
        cell.checked = true;
        Calendar.persist($stateName, $calendar);
        break;
      case Door.Response.ALREADY_OPEN:
        // Show video modal
        showVideoPlayer(
          response.episode,
          response.episodeDescription,
          response.videoId
        );
        break;
      case Door.Response.TOO_EARLY:
        // Display notification
        const quote = Quotes.getRandom();
        Snackbar.notify("Denne låge må ikke åbnes endnu!", quote.text, quote.character, 8);
        break;
      default:
        throw "Unrecognised door response";
    }

    Achievements.trackDoorOpen({
      doorNumber: Calendar.doorAt($calendar, position),
      now: new Date(),
      alreadyOpen: cell.checked,
      wasOpenAction: response.tag === Door.Response.OPEN,
    });

    return;
  };
  return { initialise: initialise, reset: reset, openDoor: openDoor };
})();
window.Page = Page;
