<?php


require_once(dirname(__FILE__) . "/config.php");


$conn = new mysqli(DB_HOST, DB_USER, DB_PASS, DB_NAME);
if ($conn->connect_error) die("failed connection: " . $conn->connect_error);

$out = [];

if ($_SERVER['REQUEST_METHOD'] === 'GET') {

    $res = $conn->query("select id, alien_name as alienName, body_hue as bodyHue, eye_hue as eyeHue, score, death_position_x as deathPositionX, death_position_y as deathPositionY from ld57_leaderboard");

    $out["entries"] = $res->fetch_all(MYSQLI_ASSOC);

    header('Content-type: application/json');
    echo(json_encode($out));
    $conn->close();
    return http_response_code(200);
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $json = file_get_contents('php://input');
    $data = json_decode($json);

    $statement = $conn->prepare("insert into ld57_leaderboard (alien_name, body_hue, eye_hue, score, death_position_x, death_position_y) VALUES (?, ?, ?, ?, ?, ?)");
    $statement->bind_param("siiiii", $alienName, $bodyHue, $eyeHue, $score, $deathPositionX, $deathPositionY);

    $alienName = $data->alienName;
    $bodyHue = $data->bodyHue;
    $eyeHue = $data->eyeHue;
    $score = $data->score;
    $deathPositionX = $data->deathPositionX;
    $deathPositionY = $data->deathPositionY;

    $out["success"] = $statement->execute();
    $out["id"] = $conn->insert_id;

    header('Content-type: application/json');
    echo(json_encode($out));
    $conn->close();
    return http_response_code(200);

}

$conn->close();
return http_response_code(400);

