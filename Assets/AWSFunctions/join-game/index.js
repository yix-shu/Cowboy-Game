/*
Curated by closely following BatteryAcid's youtube tutorial on setting up AWS
with Unity: https://www.youtube.com/watch?v=X45VYma6738&t=0s
*/
const AWS = require('aws-sdk');
const ddb = new AWS.DynamoDB.DocumentClient();
require('./join-patch.js');
let send = undefined;
const TABLE_NAME = "game-session-1";
const PLAYING_OP = "11";

function init(event) {
   console.log(event.requestContext);
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

//finds existing game session
function getAvailableGameSession() {
   return ddb.scan({
      TableName: TABLE_NAME,
      FilterExpression: "#p2 = :empty and #status <> :closed",
      ExpressionAttributeNames: {
         "#p2": "player2",
         "#status": "gameStatus"
      },
      ExpressionAttributeValues: {
         ":empty": "empty",
         ":closed": "closed"
      }
   }).promise();
}

//connects to our dynamodb database or table
function addConnectionId(connectionId) {
   return getAvailableGameSession().then((data) => {
      console.log("Game session data: %j", data);

      if (data && data.Count < 1) { //if there is no existing session
         console.log("No existing session found. Creating new session");

         return ddb.put({
            TableName: TABLE_NAME,
            Item: {
               uuid: Date.now() + '', //for temporary use, but will want to use a uuid generator in the future 
               player1: connectionId,
               player2: "empty"
            },
         }).promise();
      } else {
         //if there is existing game session, then add player2 to the game session
         console.log("Session exists, adding player2 to existing session");

         return ddb.update({
            TableName: TABLE_NAME,
            Key: {
               "uuid": data.Items[0].uuid 
            },
            UpdateExpression: "set player2 = :p2",
            ExpressionAttributeValues: {
               ":p2": connectionId
            }
         }).promise().then(() => {
            //sends playing opcode to player1 to indicate game has begun
            send(data.Items[0].player1, '{ "uuid": ' + data.Items[0].uuid + ', "opcode": ' + PLAYING_OP +
               ' }');
         });
      }
   });
}

exports.handler = (event, context, callback) => {
   const connectionId = event.requestContext.connectionId;
   console.log(event); //debugging so we know what event is received
   init(event);

   addConnectionId(connectionId).then(() => {
      callback(null, {
         statusCode: 200,
      });
   });
}