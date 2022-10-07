/*
Curated by closely following BatteryAcid's youtube tutorial on setting up AWS
with Unity: https://www.youtube.com/watch?v=X45VYma6738&t=0s
*/
const AWS = require('aws-sdk');
const ddb = new AWS.DynamoDB.DocumentClient();
require('./disconnect-patch.js');
const TABLE_NAME = "game-session-1";
let disconnectWebSocket = undefined;

function init(event) {
    console.log(event)
    const apigwManagementApi = new AWS.ApiGatewayManagementApi({ apiVersion: '2018-11-29', endpoint: event.requestContext.domainName + '/' + event.requestContext.stage });
    disconnectWebSocket = async(connectionId) => {
        await apigwManagementApi.deleteConnection({ ConnectionId: connectionId }).promise();
    }
}

function getGameSession(playerId) {
    return ddb.scan({
        TableName: TABLE_NAME,
        FilterExpression: "#p1 = :playerId or #p2 = :playerId",
        ExpressionAttributeNames: {
            "#p1": "player1",
            "#p2": "player2"
        },
        ExpressionAttributeValues: {
            ":playerId": playerId
        }
    }).promise();
}

async function closeGame(uuid) {
    ddb.update({
        TableName: TABLE_NAME,
        Key: {
            "uuid": uuid
        },
        UpdateExpression: "set gameStatus = :status",
        ExpressionAttributeValues: {
            ":status": "closed"
        }
    }).promise();
}

exports.handler = (event, context, callback) => {
    console.log("Disconnect event received: %j", event);
    init(event);
    const connectionIdForCurrentRequest = event.requestContext.connectionId;
    console.log("Request from player: " + connectionIdForCurrentRequest);

    getGameSession(connectionIdForCurrentRequest).then((data) => {
        console.log("getGameSession: " + data.Items[0].uuid);

        if (data.Items[0].player1 == connectionIdForCurrentRequest) {
            closeGame(data.Items[0].uuid);
            //Player 1 has disconnected, so disconnect player 2 as well
            if (data.Items[0].player2 !== 'empty') {
                console.log("Disconnecting player 2: " + data.Items[0].player2);
                
                disconnectWebSocket(data.Items[0].player2).then(() => {}, (err) => {
                    console.log("Error: player 2 may have already disconnected.");
                    console.log(err);
                });
            }
            else {
                console.log("Player2 missing");
            }
        }
        else {
            //Player 2 has disconnected, so disconnect player 1 as well
            console.log("Disconnecting player 1: " + data.Items[0].player1);
            closeGame(data.Items[0].uuid);
            disconnectWebSocket(data.Items[0].player1).then(() => {}, (err) => {
                console.log("Error: player 1 may have already disconnected.");
                console.log(err);
            });;
        }
    });

    return callback(null, { statusCode: 200});
}