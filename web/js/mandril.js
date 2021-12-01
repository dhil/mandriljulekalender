/*
 * Dynamic calendar rendering
 */

const Cookie = (function() {
    const get = function(name) {
        // Split into key-value pairs
        let entries = document.cookie.split(";");

        // Iterate the key-value pairs
        for(let i = 0; i < entries.length; i++) {
            let keyValuePair = entries[i].split("=");
            if (name == keyValuePair[0].trim()) {
                // Decode the cookie value and return
                return decodeURIComponent(keyValuePair[1]);
            }
        }
        // Return null if not found
        return null;
    };

    const set = function(key, value, expirationDate) {
        document.cookie = key + "=" + encodeURIComponent(value) + ";expires=" + expirationDate + ";SameSite=lax;path=/";
        return;
    };

    const has = function(name) {
        return get(name) !== null;
    };

    const forget = function(key) {
        document.cookie = key + "=;expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/";
        return;
    }

    return {'get': get, 'set': set, 'has': has, 'forget': forget};
})();

class Calendar {
    constructor(doorLayout, doorStates) {
        this.layout = doorLayout;
        this.states = doorStates;
    }

    // Logic
    open(doorN) { // zero-indexed
        this.states |= (1 << doorN);
        return this;
    }

    isOpen(doorN) {
        return (this.states & (1 << doorN)) != 0;
    }

    isClosed(doorN) { return !isOpen(doorN); }

    // Decoding and encoding
    static deserialise(s) {
        let [doorLayout, doorStates] = s.split(',');
        doorLayout = doorLayout.match(/.{1,2}/g);
        return new Calendar(doorLayout.map((door) => door | 0), doorStates | 0);
    }

    serialise() {
        let layoutEncoded = this.layout.map((door) => door < 10 ? "0" + door : door + "").join("");
        let stateEncoded = layoutEncoded + "," + this.states;
        return stateEncoded;
    }

    static makeNew() {
        let layout = [...Array(25).keys()].map((i) => i + 1);
        // Shuffle layout
        for (let i = layout.length - 1; i > 0; i--) {
            let j = Math.floor(Math.random() * (i + 1));
            [layout[i], layout[j]] = [layout[j], layout[i]];
        }
        return new Calendar(layout, 0);
    }
}

let $calendar = null;
const $stateName = "calendarState";
let $locked = false;

function persist() {
    const expirationDate = new Date((new Date().getFullYear() + 1) + "-01-31");
    Cookie.set($stateName, $calendar.serialise(), expirationDate);
    return;
}

const Door = (function() {
    const descriptions = [ "Casper får besøg af Asger Debono, som lægger lækkerier ud på foderbrættet.",
			   "Casper får besøg af Jørgen Ingemann, der får besøg af Julemanden som den første hvert år.",
			   "Endnu engang besøg af Asger Debono der ved hvad der rører sig inden for halm.",
			   "Casper får besøg af Chacha Jublenn der ved alt om post.",
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
			   "Casper forsøger at oprette forbindelse til rumbukken Fyffy." ];
    const videoIds = [ "whh2aljch8w",
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
		       "OH_c3VR3O6M" ];

    function toast() {
        let snackbar = document.getElementById("snackbar");
        snackbar.className = "show";
        // After 3 seconds, remove the show class from DIV
        setTimeout(function() {
            snackbar.className = snackbar.className.replace("show", "");
        }, 3000);
        return;
    }

    function displayVideoPlayer(episode, description, videoId) {
        //
        document.body.classList.add("dialogIsOpen");
        // Get the modal
        var modal = document.getElementById("videoplayer");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        //
        let body = document.getElementsByClassName("modal-body")[0];
        body.innerHTML = "<p>" + description + "</p><iframe class=\"embedded-player\" src=\"https://www.youtube.com/embed/" + videoId + "\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";
        document.getElementById("video-title").innerHTML = "Afsnit " + episode;

        // When the user clicks on <span> (x), close the modal
        span.onclick = function() {
            modal.style.display = "none";
            body.innerHTML = "";
            document.body.className = document.body.className.replace("dialogIsOpen","");
            $locked = false;
        }
        modal.style.display = "block";
    }

    const open = function(d, n) {
        d.checked = $calendar.isOpen(n);
        if ($locked) return;
        let doorNumber = $calendar.layout[n];
        // Doors are coded as checkboxes, where we interpret "checked"
        // as denoting the door as "open", otherwise it is
        // "closed".
        // Some rules:
        // 1) A door that has been opened cannot be closed again.
        // 2) A door can only be opened if its number/label is less
        // than or equal to the current day in December.
        if (d.checked) {
            // Open video modal
            $locked = true;
            displayVideoPlayer(doorNumber, descriptions[doorNumber - 1], videoIds[doorNumber - 1]);
        } else {
            let currentDate = new Date();
            let doorDate = new Date(currentDate.getFullYear() + "-12-" + doorNumber);
            if (doorDate <= currentDate) {
                d.checked = true;
                $calendar.open(n);
                persist();
            } else {
                d.checked = false;
                // Show notification
                toast();
            }
        }
        return;
    };

    const make = function(i, n, isOpen) {
        let front = document.createElement("div");
        front.classList.add("front");
        front.appendChild(document.createTextNode(n.toString()));
        let rear = document.createElement("div");
        rear.classList.add("back");

        let door = document.createElement("div");
        door.classList.add("door");
        door.appendChild(front); door.appendChild(rear);

        let input = document.createElement("input");
        input.setAttribute("type", "checkbox");
        input.checked = isOpen;
        input.onclick = function(_event) { return Door.open(input, i); };

        let label = document.createElement("label");
        label.appendChild(input); label.appendChild(door);

        let doorN = document.createElement("div");
        let j = i + 1;
        doorN.setAttribute("id", "door-" + j);
        doorN.classList.add("door-" + n);
        doorN.appendChild(label);
        return doorN;
    };

    return {'open': open, 'make': make};
})();


const Page = (function() {
    const render = function() {
        let doors = document.createDocumentFragment();
        for (let i = 0; i < 25; i++) {
            doors.appendChild(Door.make(i, $calendar.layout[i], $calendar.isOpen(i)));
        }
        let calendarNode = document.getElementById("calendar");
        calendarNode.innerHTML = "";
        calendarNode.appendChild(doors);
        return;
    };

    const initialise = function() {
        if (Cookie.has($stateName)) {
            $calendar = Calendar.deserialise(Cookie.get($stateName));
        } else {
            $calendar = Calendar.makeNew();
            persist();
        }
        return render();
    };

    const reset = function() {
        Cookie.forget($stateName);
        return location.reload();
    };
    return {'initialise': initialise, 'reset': reset};
})();
