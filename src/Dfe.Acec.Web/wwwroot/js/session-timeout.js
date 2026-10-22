(function () {
    "use strict";

    var dialog = document.getElementById("session-timeout-dialog");
    if (!dialog || typeof dialog.showModal !== "function") {
        return;
    }

    var data = dialog.dataset;
    var timeoutMs = Number(data.timeoutMinutes) * 60000;
    var warningMs = Math.min(Number(data.warningMinutes) * 60000, timeoutMs);
    if (!(timeoutMs > 0) || !(warningMs > 0)) {
        return;
    }

    var message = document.getElementById("session-timeout-message");
    var continueButton = document.getElementById("session-timeout-continue");
    var deadline = Date.now() + timeoutMs;
    var shownMinutes = 0;
    var busy = false;

    function formatMinutes(minutes) {
        return minutes === 1
            ? data.oneMinute
            : data.manyMinutes.replace("{0}", String(minutes));
    }

    function render(remainingMs) {
        var minutes = Math.max(1, Math.ceil(remainingMs / 60000));
        if (minutes === shownMinutes) {
            return;
        }

        shownMinutes = minutes;
        message.textContent = data.messageTemplate.replace("{0}", formatMinutes(minutes));
    }

    function expire() {
        window.location.assign(data.expiredUrl);
    }

    function tick() {
        var remainingMs = deadline - Date.now();
        if (remainingMs <= 0) {
            expire();
            return;
        }

        if (remainingMs <= warningMs) {
            if (!dialog.open) {
                dialog.showModal();
            }

            render(remainingMs);
        }
    }

    async function keepAlive() {
        if (busy) {
            return;
        }

        busy = true;
        try {
            var response = await fetch(data.keepAliveUrl, {
                method: "POST",
                credentials: "same-origin",
                headers: { "Content-Type": "application/x-www-form-urlencoded" },
                body: new URLSearchParams({ __RequestVerificationToken: data.token })
            });

            if (response.ok) {
                deadline = Date.now() + timeoutMs;
                shownMinutes = 0;
                dialog.close();
            } else if (response.status === 410) {
                expire();
            }
        } catch (error) {
            return;
        } finally {
            busy = false;
        }
    }

    continueButton.addEventListener("click", keepAlive);
    dialog.addEventListener("cancel", function (event) {
        event.preventDefault();
        keepAlive();
    });
    document.addEventListener("visibilitychange", tick);
    window.setInterval(tick, 1000);
})();
