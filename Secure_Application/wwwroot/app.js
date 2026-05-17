let loggedInUser = null;

function showResult(elementId, data) {
    document.getElementById(elementId).textContent =
        typeof data === "string" ? data : JSON.stringify(data, null, 2);
}

async function register() {
    const res = await fetch('/api/Auth/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: document.getElementById("regUsername").value,
            password: document.getElementById("regPassword").value
        })
    });

    const text = await res.text();
    showResult("registerResult", text);

    if (res.ok) {
        clearFields(["regUsername", "regPassword"]);
    }
}

async function login() {
    const username = document.getElementById("loginUsername").value;

    const res = await fetch('/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: username,
            password: document.getElementById("loginPassword").value
        })
    });

    const text = await res.text();
    showResult("loginResult", text);

    if (res.ok) {
        loggedInUser = username;

        document.getElementById("loginStatus").textContent =
            "Status: Logged in as " + loggedInUser;

        document.getElementById("bookingArea").style.display = "block";
        document.getElementById("loginRequiredMessage").style.display = "none";

        clearFields(["loginUsername", "loginPassword"]);
        loadMyBookings();
    }
}

async function createBooking() {
    if (!loggedInUser) {
        showResult("bookingResult", "You must be logged in to create a booking.");
        return;
    }

    const res = await fetch('/api/Booking/create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: loggedInUser,
            customerName: document.getElementById("customerName").value,
            serviceType: document.getElementById("serviceType").value,
            bookingDate: document.getElementById("bookingDate").value
        })
    });

    const text = await res.text();
    showResult("bookingResult", text);

    if (res.ok) {
        clearFields(["customerName", "serviceType", "bookingDate"]);
        loadMyBookings();
    }
}

async function loadMyBookings() {
    if (!loggedInUser) {
        showResult("myBookingsResult", "Please log in first.");
        return;
    }

    const res = await fetch(`/api/Booking/my-bookings?username=${encodeURIComponent(loggedInUser)}`);
    const text = await res.text();

    try {
        showResult("myBookingsResult", JSON.parse(text));
    } catch {
        showResult("myBookingsResult", text);
    }
}

function clearFields(ids) {
    ids.forEach(id => {
        const el = document.getElementById(id);
        if (el) el.value = "";
    });
}