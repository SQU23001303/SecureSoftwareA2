function showResult(data) {
    document.getElementById("result").textContent =
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
    showResult(text);

    if (res.ok) {
        clearFields(["regUsername", "regPassword"]);
    }
}

async function login() {
    const res = await fetch('/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            username: document.getElementById("loginUsername").value,
            password: document.getElementById("loginPassword").value
        })
    });

    const text = await res.text();
    showResult(text);

    if (res.ok) {
        clearFields(["loginUsername", "loginPassword"]);
    }
}

async function createBooking() {
    const res = await fetch('/api/Booking/create', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            customerName: document.getElementById("customerName").value,
            serviceType: document.getElementById("serviceType").value,
            bookingDate: document.getElementById("bookingDate").value
        })
    });

    const text = await res.text();
    showResult(text);

    if (res.ok) {
        clearFields(["customerName", "serviceType", "bookingDate"]);
    }
}

async function searchBooking() {
    const name = document.getElementById("searchName").value;

    const res = await fetch(`/api/Booking/search?customerName=${name}`);
    showResult(await res.json());
}

function clearFields(ids) {
    ids.forEach(id => {
        const el = document.getElementById(id);
        if (el) el.value = "";
    });
}