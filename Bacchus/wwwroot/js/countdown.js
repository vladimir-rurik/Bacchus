function countdown(endDate) {
	let days, hours, minutes, seconds;

	endDate = new Date(endDate).getTime();

	if (isNaN(endDate)) {
		return;
	}

	setInterval(calculate, 1000);

	function calculate() {

		let startDate = new Date();

		startDate = new Date(startDate.getUTCFullYear(),
			startDate.getUTCMonth(),
			startDate.getUTCDate(),
			startDate.getUTCHours(),
			startDate.getUTCMinutes(),
			startDate.getUTCSeconds());

		let timeRemaining = parseInt((endDate - startDate.getTime()) / 1000);

		if (timeRemaining >= 0) {
			days = parseInt(timeRemaining / 86400);
			timeRemaining = (timeRemaining % 86400);

			hours = parseInt(timeRemaining / 3600);
			timeRemaining = (timeRemaining % 3600);

			minutes = parseInt(timeRemaining / 60);
			timeRemaining = (timeRemaining % 60);

			seconds = parseInt(timeRemaining);

			document.querySelectorAll(".days").forEach(function (daysElement, index) {
				daysElement.innerHTML = parseInt(days, 10);
			});
			document.querySelectorAll(".hours").forEach(function (hoursElement, index) {
				hoursElement.innerHTML = ("0" + hours).slice(-2);
			});
			document.querySelectorAll(".minutes").forEach(function (minutesElement, index) {
				minutesElement.innerHTML = ("0" + minutes).slice(-2);
			});
			document.querySelectorAll(".seconds").forEach(function (secondsElement, index) {
				secondsElement.innerHTML = ("0" + seconds).slice(-2);
			});
		} else {
			return;
		}
	}
}
