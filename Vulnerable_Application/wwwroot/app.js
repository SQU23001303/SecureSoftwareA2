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

    showResult(await res.text());
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

    showResult(await res.text());
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

    showResult(await res.text());
}

async function searchBooking() {
    const name = document.getElementById("searchName").value;

    const res = await fetch(`/api/Booking/search?customerName=${name}`);
    showResult(await res.json());
}