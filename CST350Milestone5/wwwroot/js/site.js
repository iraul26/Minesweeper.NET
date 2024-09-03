$('#login-form').on('submit', null, function (event) {
	event.preventDefault();
	const username = $('#username-input').val();
	const password = $('#password-input').val();
	login(username, password);
});

$('#save-button').on('click', null, function () {
	$.ajax({
		url: '/Minesweeper/Save',
		dataType: 'text',
		success: function (data) {
			if (data == 'true') {
				var html = '<p>Save successful!</p>';
				$('#save-result-container').html(html);
				$('#save-result-container').show();
				$('#save-result-container').fadeOut(10000);
			}
		}
	});
});

function fill(column, row) {
	$.ajax({
		url: '/Minesweeper/Fill',
		data: {
			column: column,
			row: row
		},
		dataType: 'html',
		success: function (data) {
			$('#game-board').html(data);
		}
	});
}

function flag(event, column, row) {
	event.preventDefault();
	$.ajax({
		url: '/Minesweeper/Flag',
		data: {
			column: column,
			row: row
		},
		dataType: 'html',
		success: function (data) {
			$('#game-board').html(data);
		}
	});
}

function retrylogin() {
	$('#saves-list-container').fadeOut(500);
	$('#login-form-container').fadeIn(1500);
}

function login(username, password) {
	$.ajax({
		url: '/LoginAJAX',
		data: {
			username: username,
			password: password
		},
		dataType: 'html',
		success: function (data) {
			$('#login-form-container').fadeOut(500);
			$('#saves-list-container').html(data);
			$('#saves-list-container').fadeIn(1500);
		}
	});
}

function showsavedgames(data) {
	var html = '<div>';
	html += '<a href="/Minesweeper/Newgame"><h4>New Game</h4></a>';
	html += '<h4>Saved Games</h4>'
	html += '<table class="table table-dark">';
	html += '<thead>';
	html += '<tr>';
	html += '<th scope="col">ID</th>';
	html += '<th scope="col">Save Date</th>';
	html += '<th scope="col">Delete</th>';
	html += '</tr>'
	html += '</thead>';
	html += '<tbody>';
	$.each(data, function (key, value) {
		html += '<tr>';
		html += '<th scope="row"><a href="/Minesweeper/Load/' + value.id + '">' + value.id + '</a></th>';
		html += '<th><a href="/Minesweeper/Load/' + value.id + '">' + value.date + '</a></th>';
		html += '<th><button class="btn btn-danger" onclick="deletesave(0, ' + value.id + ')">Delete</button></th>';
		html += '</tr>'
	});
	html += '</tbody>';
	html += '</table>';
	$('#save-list').html(html);
}

function getsaves(playerid) {
	$.ajax({
		url: '/api/showSavedGames',
		dataType: 'json',
		success: function (data) {
			showsavedgames(data);
		}
	});
}

function deletesave(playerid, gameid) {
	$.ajax({
		url: '/api/deleteOneGame/' + gameid,
		dataType: 'json',
		success: function (data) {
			showsavedgames(data);
		}
	});
}