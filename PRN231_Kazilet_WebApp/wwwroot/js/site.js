var connection;

async function getQuestion() {
    try {
        console.log(connection);
    } catch (err) {
        console.error(err.toString());
    }
}

function getCookie(key) {
    const name = key + "=";
    const decodedCookie = document.cookie;
    const cookiesArray = decodedCookie.split(';');

    for (let i = 0; i < cookiesArray.length; i++) {
        let cookie = cookiesArray[i].trim();
        if (cookie.indexOf(name) === 0) {
            return cookie.substring(name.length, cookie.length);
        }
    }

    return null;
}

function connect(token) {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7024/signalrServer?token=" + token, {
            accessTokenFactory: () => token,
            skipNegotiation: true,  // skipNegotiation as we specify WebSockets
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();

    connection.start().then(function () {
        connection.on("UserJoined", function (username) {
            try {
                var playerDiv = document.getElementById("playerDiv");
                if (playerDiv) {
                    var myUsername = document.getElementById("username").innerHTML;
                    var totalPlayers = Number(document.getElementById("no-players").innerHTML);

                    let parentDiv = $("<div></div>")
                        .addClass("flex mr-4 items-center flex-row p-3  text-center animate-pulse")
                    let player = $(`
    <div class="relative mr-2 w-10 h-10 overflow-hidden bg-gray-100 rounded-full dark:bg-gray-600">
        <svg class="absolute w-12 h-12 text-gray-400 -left-1" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
            <path fill-rule="evenodd" d="M10 9a3 3 0 100-6 3 3 0 000 6zm-7 9a7 7 0 1114 0H3z" clip-rule="evenodd"></path>
        </svg>
    </div>
    <div class="text-lg">
        ${myUsername == username ? `${username} (You)` : username}
    </div>`);

                    parentDiv.append(player);
                    playerDiv.append(parentDiv.get(0));
                    document.getElementById("no-players").innerHTML = totalPlayers + 1;
                }
            } catch (error) {
                console.log(error);
            }
        });


        connection.on("UserLeft", function (username) {

        });

        connection.on("GetQuestion", function (question) {

        });
        console.log(connection);


    }).catch(function (err) {
        return console.error(err.toString());
    });
}