/*
Curated by closely following BatteryAcid's youtube tutorial on setting up AWS
with Unity: https://www.youtube.com/watch?v=X45VYma6738&t=0s
*/
const AWS = require('aws-sdk');
const ddb = new AWS.DynamoDB.DocumentClient();
require('./messaging-patch.js');
let send = undefined;
const TABLE_NAME = "game-session-1";
const REQUEST_START_OP = "1";

const SHOOT_OP = "5";
const RELOAD_OP = "7";
const HOLD_OP = "9";

//burden on the game to calculate and process outcomes
const YOU_DIED_OP = "44"; 
const YOU_WON_OP = "88";
const TIE_OP = "66";

const PLAYING_OP = "11";

function init(event) {
   const apigwManagementApi = new AWS.ApiGatewayManagementApi({
      apiVersion: '2018-11-29',
      endpoint: event.requestContext.domainName + '/' + event.requestContext.stage
   });
   //sends request with connectionid and data
   send = async (connectionId, data) => {
      await apigwManagementApi.postToConnection({
         ConnectionId: connectionId,
         Data: `${data}`
      }).promise();
   }
}

//connects to our dynamodb database or table
function getConnections() {
   return ddb.scan({
      TableName: TABLE_NAME,
   }).promise();
}

//finds existing game session
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

exports.handler = (event, context, callback) => {
   console.log("Event received: %j", event);
   console.log(event.requestContext); //debugging
   init(event);

   let message = JSON.parse(event.body);
   console.log("Message: ", message);

   let connectionIdForCurrentRequest = event.requestContext.connectionId;
   console.log("Current connectionid: " + connectionIdForCurrentRequest);

   if (message && message.opcode) {

      switch (message.opcode) {
         case REQUEST_START_OP:
            console.log("Opcode 1 sent");

            getGameSession(connectionIdForCurrentRequest).then((data) => {
               console.log("getGameSession: " + data.Items[0].uuid);

               // Checking for closed in case player quits
               // closed is to ensure player2 can't join an abandoned game session
               var opcodeStart = "0";
               if (data.Items[0].gameStatus != "closed" && data.Items[0].player2 != "empty") {
                  opcodeStart = PLAYING_OP;
               }

               send(connectionIdForCurrentRequest, '{ "uuid": ' + data.Items[0].uuid + ', "opcode": ' +
                  opcodeStart + ' }');
            });

            break;

         case SHOOT_OP:
            console.log("Opcode 5 sent");

            getGameSession(connectionIdForCurrentRequest).then((data) => {
               console.log("getGameSession: %j", data.Items[0]);

               var sendToConnectionId = connectionIdForCurrentRequest;
               if (data.Items[0].player1 == connectionIdForCurrentRequest) {
                  // sends request out to player2
                  sendToConnectionId = data.Items[0].player2;
               } else {
                  // sends request out to player1
                  sendToConnectionId = data.Items[0].player1;
               }

               console.log("sending throw message to: " + sendToConnectionId);
               send(sendToConnectionId, '{ "uuid": ' + data.Items[0].uuid + ', "opcode": ' +
                  SHOOT_OP + ', "message": "other player shot" }');
            });

            break;
            
         case RELOAD_OP:
            console.log("Opcode 7 sent");

            getGameSession(connectionIdForCurrentRequest).then((data) => {
               console.log("getGameSession: %j", data.Items[0]);

               var sendToConnectionId = connectionIdForCurrentRequest;
               if (data.Items[0].player1 == connectionIdForCurrentRequest) {
                  // sends request out to player2
                  sendToConnectionId = data.Items[0].player2;
               } else {
                  // sends request out to player1
                  sendToConnectionId = data.Items[0].player1;
               }

               console.log("sending reload message to: " + sendToConnectionId);
               send(sendToConnectionId, '{ "uuid": ' + data.Items[0].uuid + ', "opcode": ' +
                  RELOAD_OP + ', "message": "other player reloaded" }');
            });

            break;
            
         case HOLD_OP: //passing turn and dodging potential shooting
            console.log("Opcode 9 sent");

            getGameSession(connectionIdForCurrentRequest).then((data) => {
               console.log("getGameSession: %j", data.Items[0]);

               var sendToConnectionId = connectionIdForCurrentRequest;
               if (data.Items[0].player1 == connectionIdForCurrentRequest) {
                  // sends request out to player2
                  sendToConnectionId = data.Items[0].player2;
               } else {
                  // sends request out to player1
                  sendToConnectionId = data.Items[0].player1;
               }

               console.log("sending hold message to: " + sendToConnectionId);
               send(sendToConnectionId, '{ "uuid": ' + data.Items[0].uuid + ', "opcode": ' +
                  HOLD_OP + ', "message": "other player held/passed" }');
            });

         default:
            //No default case
      }
   }

   return callback(null, {
      statusCode: 200,
   });
};