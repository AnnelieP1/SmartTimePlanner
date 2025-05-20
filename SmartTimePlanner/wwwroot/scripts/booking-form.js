document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("schedule-form");
    const result = document.getElementById("form-result");

    form.addEventListener("submit", async function (e) {
        e.preventDefault();

        const data = {
            consultantName: form.consultantName.value,
            dayOfWeek: form.dayOfWeek.value,
            startTime: form.startTime.value,
            endTime: form.endTime.value,
            activity: form.activity.value
        };

        try {
            const response = await fetch("/api/schedulesubmit/save", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    consultantName: form.consultantName.value,
                    dayOfWeek: form.dayOfWeek.value,
                    startTime: form.startTime.value,
                    endTime: form.endTime.value,
                    activity: form.activity.value
                })
            });

            const resultData = await response.json();

            if (response.ok) {
                result.innerHTML = `<span style="color:lightgreen">✔ ${resultData.message}</span>`;

                if (window.calendar && typeof window.calendar.refetchEvents === 'function') {
                    window.calendar.refetchEvents();
                }
                    
                // Här kan du trigga kalender-refresh t.ex. calendar.refetchEvents()
            } else {
                result.innerHTML = `<span style="color:orange">⚠ ${resultData.message}</span>`;
            }
        } catch (err) {
            result.innerHTML = `<span style="color:red">❌ Error: ${err.message}</span>`;
        }
    });
});
