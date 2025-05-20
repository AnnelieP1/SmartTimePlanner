document.addEventListener("DOMContentLoaded", function () {
    const calendarEl = document.getElementById("calendar");

    fetch("/api/scheduleapi")
        .then(response => response.json())
        .then(data => {
            const events = [];

            data.forEach(entry => {
                const conflicts = entry.conflicts?.flatMap(c => c.conflictBetween) || [];

                entry.schedule.forEach(item => {
                    const isConflict = conflicts.some(conflict =>
                        conflict.startTime === item.startTime &&
                        conflict.endTime === item.endTime &&
                        conflict.activity === item.activity
                    );

                    events.push({
                        title: `${entry.consultant}: ${item.activity}`,
                        start: item.startTime,
                        end: item.endTime,
                        classNames: isConflict ? ['conflict-event'] : [],
                        extendedProps: {
                            consultant: entry.consultant,
                            activity: item.activity,
                            time: `${item.startTime} - ${item.endTime}`
                        }
                    });
                });
            });

            const calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                events: events, // <-- viktigt!
                eventDidMount: function (info) {
                    tippy(info.el, {
                        content: `
                            <strong>${info.event.extendedProps.consultant}</strong><br>
                            ${info.event.extendedProps.activity}<br>
                            <small>${info.event.extendedProps.time}</small>
                        `,
                        allowHTML: true,
                        placement: 'top',
                        theme: 'smarttime'
                    });
                }
            });

            window.calendar = calendar; // gör tillgänglig för refetch
            calendar.render();
        })
        .catch(error => {
            console.error("Fel vid hämtning av schema:", error);
        });
});
