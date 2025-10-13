"use strict";

/**
 * Achievements
 */
const Achievements = (function () {
  const COOKIE_KEY = "achievementsState";
  let resetDate = null;

  // --- Definitions ---
  const DEFS = [
    {
      id: "firstDoor",
      title: "Den første låge",
      desc: "Åbn din første låge.",
      icon: "🗝️",
    },
    {
      id: "tooEarly",
      title: "Ho ho hov pilfinger!",
      desc: "Forsøg at åbne en låge for tidligt.",
      icon: "⏰",
    },
    {
      id: "all25",
      title: "25 dage senere",
      desc: "Åbn alle 25 låger.",
      icon: "🎄",
    },
    {
      id: "januaryOpen",
      title: "Julen varer lige til påske",
      desc: "Åbn en låge i januar.",
      icon: "🌨️",
    },
    {
      id: "snowToggle5",
      title: "Sneblind",
      desc: "Slå sneen til/fra fem gange.",
      icon: "❄️",
    },

    {
      id: "watch5",
      title: "Stofa Donnington-certificeret",
      desc: "Se 5 forskellige afsnit.",
      icon: "📺",
    },
    {
      id: "repeatSameDoor",
      title: "Doby Slinger ved intet",
      desc: "Klik gentagne gange på samme låge, uden der sker noget nyt.",
      icon: "🔁",
    },
    {
      id: "fullyWatched",
      title: "Cosy Joe-sindstilstand",
      desc: "Se et afsnit færdigt uden at lukke.",
      icon: "📺",
    },
    {
      id: "lastDoor",
      title: "Fergie’s Velsignelse",
      desc: "Åbn den sidste låge (25).",
      icon: "👼",
    },
    {
      id: "misorder10",
      title: "Julian Tisange’s Reformpakke",
      desc: "Åbn låger i ‘forkert’ rækkefølge mindst 10 gange.",
      icon: "🔀",
    },

    // Meta
    {
      id: "share",
      title: "Miffer Hansepajuk Marketing Award",
      desc: "Del kalenderen.",
      icon: "📣",
    },
    {
      id: "multiDay3",
      title: "Asger Debono’s Halmpris",
      desc: "Brug kalenderen på tre forskellige dage.",
      icon: "📅",
    },
    {
      id: "thorCombo",
      title: "Thor Vitter Rynkemås Gourmet",
      desc: "Åbn låge 7 og 8 i træk (and + nougat).",
      icon: "🍗🍫",
    },
    {
      id: "afterMidnight",
      title: "Foderbrættets Vogter",
      desc: "Brug kalenderen efter midnat.",
      icon: "🌙",
    },

    // Meta-meta
    {
      id: "grandMaster",
      title: "Mandril-mester",
      desc: "Lås op for alle andre achievements.",
      icon: "🏆",
    },
  ];

  // Hurtig lookup
  const DEF_BY_ID = Object.fromEntries(DEFS.map((d) => [d.id, d]));

  // --- State & persistens ---
  let state = null;
  function defaultState() {
    return {
      unlocked: [], // array af unlocked achievement id'er
      counters: {
        snowToggles: 0,
        tooEarlyAttempts: 0,
        misorders: 0, // ‘forkert rækkefølge’
      },
      video: {
        seenEpisodes: {}, // { episodeNumber: { shown: n, fully: m } }
      },
      usage: {
        days: {}, // YYYY-MM-DD -> true
        lastDoorNumberOpened: null,
        lastDoorClicked: null,
      },
    };
  }

  function getState() {
    return state;
  }

  function load() {
    try {
      if (Cookie.has(COOKIE_KEY)) {
        const raw = Cookie.get(COOKIE_KEY);
        const parsed = JSON.parse(decodeURIComponent(raw));
        return parsed || defaultState();
      }
    } catch (_e) {
      /* ignore */
    }
    return defaultState();
  }

  function persist() {
    let expire = new Date(resetDate);
    Cookie.set(
      COOKIE_KEY,
      encodeURIComponent(JSON.stringify(state)),
      expire.toUTCString()
    );
  }

  // --- Utils ---
  function notifyUnlock(def) {
    if (!def) return;

    Snackbar.notify(
      "Achievement Opnået",
      `Du har låst op for: “${def.title}”`,
      "Mandrillen",
      8
    );
  }

  function addUnlock(id) {
    if (!DEF_BY_ID[id]) return false;
    if (state.unlocked.includes(id)) return false;
    state.unlocked.push(id);
    persist();
    notifyUnlock(DEF_BY_ID[id]);
    // Check om alt er åbnet -> grandMaster
    if (
      DEFS.filter((d) => d.id !== "grandMaster").every((d) =>
        state.unlocked.includes(d.id)
      )
    ) {
      if (!state.unlocked.includes("grandMaster")) {
        state.unlocked.push("grandMaster");
        persist();
        notifyUnlock(DEF_BY_ID["grandMaster"]);
      }
    }
    return true;
  }

  // --- API ---
  function init(date) {
    resetDate = date;
    state = load();
    trackVisit(); // registrér dagens besøg
  }

  function unlock(id) {
    addUnlock(id);
  }
  function has(id) {
    return state.unlocked.includes(id);
  }
  function getAll() {
    return DEFS.slice();
  }
  function getUnlocked() {
    return state.unlocked.slice();
  }

  // --- Tracking hooks ---
  function trackVisit(now = new Date()) {
    const yyyy = now.getFullYear();
    const mm = String(now.getMonth() + 1).padStart(2, "0");
    const dd = String(now.getDate()).padStart(2, "0");
    const key = `${yyyy}-${mm}-${dd}`;
    if (!state.usage.days[key]) {
      state.usage.days[key] = true;
      persist();
    }
    // multiDay3
    if (Object.keys(state.usage.days).length >= 3) addUnlock("multiDay3");
    // afterMidnight
    if (now.getHours() >= 0 && now.getHours() < 4) addUnlock("afterMidnight");
  }

  function trackTooEarlyAttempt() {
    state.counters.tooEarlyAttempts += 1;
    persist();
    addUnlock("tooEarly");
  }

  function trackSnowToggle() {
    state.counters.snowToggles += 1;
    persist();
    if (state.counters.snowToggles >= 5) addUnlock("snowToggle5");
  }

  function trackVideoShown(episode) {
    if (!state.video.seenEpisodes[episode]) {
      state.video.seenEpisodes[episode] = { shown: 0, fully: 0 };
    }
    state.video.seenEpisodes[episode].shown += 1;
    persist();
  }

  function trackVideoClosed(episode, fullyWatched) {
    if (!state.video.seenEpisodes[episode]) return;
    if (fullyWatched) {
      state.video.seenEpisodes[episode].fully += 1;
      persist();
      addUnlock("fullyWatched");
    }

    // watch5 (fem forskellige)
    const distinct = Object.keys(state.video.seenEpisodes).length;
    if (distinct >= 5) addUnlock("watch5");
  }

  function trackShareInvoked() {
    addUnlock("share");
  }

  // all25 – kræver at alle 25 er åbnet.
  function all25(doorNumber) {
    if (!state.openedPositions) state.openedPositions = {};
    state.openedPositions[doorNumber] = true;
    persist();
    if (Object.keys(state.openedPositions).length >= 25) addUnlock("all25");
  }

  function trackDoorOpen({
    doorNumber,
    now = new Date(),
    alreadyOpen = false,
    wasOpenAction = false,
  }) {
    if (wasOpenAction && state.unlocked.indexOf("firstDoor") === -1)
      addUnlock("firstDoor");

    // januaryOpen (hvis i januar)
    if (now.getMonth() === 0) addUnlock("januaryOpen");

    // lastDoor
    if (doorNumber === 25 && wasOpenAction) addUnlock("lastDoor");

    if (wasOpenAction) all25(doorNumber);

    // repeatSameDoor – klik på allerede åben låge (brug Page’s ALREADY_OPEN case)
    if (alreadyOpen) {
      // hvis samme låge klikkes flere gange i træk
      if (state.usage.lastDoorClicked === doorNumber) {
        addUnlock("repeatSameDoor");
      }
      state.usage.lastDoorClicked = doorNumber;
      persist();
    }

    // misorder10 – heuristik: hvis man åbner en låge med et nummer,
    // der er > 1 højere end sidst åbnede nummer, tæller vi som “forkert”
    if (
      state.usage.lastDoorNumberOpened != null &&
      doorNumber > state.usage.lastDoorNumberOpened + 1
    ) {
      state.counters.misorders += 1;
      persist();
      if (state.counters.misorders >= 10) addUnlock("misorder10");
    }
    state.usage.lastDoorNumberOpened = doorNumber;

    // thorCombo – 7 efterfulgt af 8
    if (doorNumber === 8 && state._prevDoorWas7) addUnlock("thorCombo");
    state._prevDoorWas7 = doorNumber === 7;
  }

  // --- Modal ---
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
    if (
      target.id === "achievements" ||
      target.id === "showAchievementsOption"
    ) {
      return true;
    }
    if (target.localName === "body") {
      return false;
    }
    return checkIsModal(target.parentNode);
  };
  function showAchievementsModal() {
    // Freeze and blur background
    freeze();
    document.body.classList.add("dialogIsOpen");
    // Get the modal
    let modal = document.getElementById("achievements");
    // Get the <span> element that closes the modal
    let closeButton = modal.getElementsByClassName("close")[0];
    // Add episode description along with an embedded YouTube video frame to the modal body
    let modalBody = modal.getElementsByClassName("modal-body")[0];

    renderAchievementsGrid();

    document.getElementById("modal-content-title").innerHTML = "Achievements";
    // When the user clicks on <span> (x), close the modal
    let closeModal = function () {
      modal.style.display = "none";
      modalBody.innerHTML = "";
      window.onclick = null;
      document.body.className = document.body.className.replace(
        "dialogIsOpen",
        ""
      );
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
  }

  function renderAchievementsGrid() {
    let root = document.getElementById("achievements-root");
    if (!root) return;
    let grid = root.querySelector("#achievements-grid");
    let bar = root.querySelector(".achievements-progress-bar > span");
    let label = root.querySelector(".achievements-progress-label");

    let all = Achievements.getAll();
    let unlockedSet = new Set(Achievements.getUnlocked());

    // Sortér: unlocked først, derefter alfabetisk på titel
    let sorted = all.slice().sort((a, b) => {
      let au = unlockedSet.has(a.id) ? 0 : 1;
      let bu = unlockedSet.has(b.id) ? 0 : 1;
      if (au !== bu) return au - bu;
      return a.title.localeCompare(b.title, "da");
    });

    // Progress
    let unlockedCount = unlockedSet.size;
    let total = all.length;
    let pct = total ? Math.round((unlockedCount / total) * 100) : 0;
    if (bar) bar.style.width = pct + "%";
    if (label) label.textContent = `${unlockedCount} / ${total}`;

    grid.innerHTML = "";
    for (const a of sorted) {
      const unlocked = unlockedSet.has(a.id);

      const card = document.createElement("div");
      card.className = "achievement-card" + (unlocked ? "" : " locked");
      card.title = a.desc;

      const badge = document.createElement("div");
      badge.className = "badge";
      badge.textContent = a.icon || "⭐";

      const meta = document.createElement("div");
      meta.className = "meta";

      const title = document.createElement("div");
      title.className = "title";
      title.textContent = a.title;

      const desc = document.createElement("div");
      desc.className = "desc";
      desc.textContent = a.desc;

      const lock = document.createElement("div");
      lock.className = "lock";
      lock.textContent = unlocked ? "✅" : "🔒";

      meta.appendChild(title);
      meta.appendChild(desc);
      card.appendChild(badge);
      card.appendChild(meta);
      card.appendChild(lock);
      grid.appendChild(card);
    }
  }

  return {
    init,
    unlock,
    has,
    getAll,
    getState,
    getUnlocked,
    showAchievementsModal,
    trackVisit,
    trackDoorOpen,
    trackTooEarlyAttempt,
    trackSnowToggle,
    trackVideoShown,
    trackVideoClosed,
    trackShareInvoked,
  };
})();
