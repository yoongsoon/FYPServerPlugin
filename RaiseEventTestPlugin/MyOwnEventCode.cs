using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin
{
    public enum MyOwnEventCode : Byte
    {
        //client request room ID from plugin
        C2S_RequestRoomID = 3,
        // plugin send room ID to the target client
        S2C_SendRoomID = 4,

        //Master client( cloud anchor host) send message to other clients informing that the cloud anchor is succesfully hosted
        C2S_InformSuccessHost = 5,
        //plugin inform other clients that the cloud anchor is successfully hosted by the master client
        S2C_InformSuccessHost = 6,

        //normal client send message to plugin to inform that the normal client is ready to start the game
        C2S_Ready = 7,
        //plugin send message to other clients (excluding the sending client) to inform that the sender client is ready
        S2C_Ready,
        //When all the normal clients are ready to start the game , the plugin will send a message to the master client
        // to inform him that he can start the game
        S2C_ReadyToStart,
        //normal client send message to plugin to inform that the normal client is Not ready to start the game
        C2S_UnReady,
        //plugin send message to other clients (excluding the sending client) to inform that the sender client is NOT ready
        S2C_UnReady,
        //plugin send message to master client to inform him that he can't start the game if  the number of client ready to start the game
        // is lesser than the total number of players (excluding the master client)
        S2C_Not_ReadyToStart,

        //master client relay information to all clients(including himself) through the plugin to leave the waiting room
        C2S_LeaveWaitingRoom,
        S2C_LeaveWaitingRoom,

        // each client request from the plugin to spawn ALL the players
        C2S_Request_To_SpawnPlayers,
        S2C_Request_To_SpawnPlayers,

        //Non master clients that have their anchor successfuly resolved , will inform the plugin
        C2S_Anchor_Resolved_Success,
        //once the plugin received all the non master clients message of successful anchor resolved,
        // the plugin will send message to all the clients (including master client) to start the game
        S2C_Anchor_Resolved_Success,

        //Master client request to start the loading for new scene
        C2S_Start_LoadingScreen,
        //plugin send message to all clients to inform them to start loading
        S2C_Start_LoadingScreen,

        //master client send message to plugin to inform the failure to host the anchor
        C2S_InformFailHost,
        //plugin send message to other clients to inform them that the master client fails to host the nachor
        S2C_InformFailHost,

        //master client send message to plugin to inform he is attempting to host the cloud ancor
        C2S_InfoAttemptToHost,
        //plugin send message to other clients to inform them that the master client is attempting to host the anchor
        S2C_InfoAttemptToHost,

        //client that left the waiting room send a message that contains its actorID to the plugin 
        C2S_LeftRoom,
        //plugin relay the information to other clients
        S2C_LeftRoom,

        //client send message to all the other clients via the plugin to inform everyone that he has joined the waiting room
        C2S_JoinedWaitingRoom,
        S2C_JoinedWaitingRoom,

        //info the plugin that the master client has switched in waiting room, therefore reset the ready count in the plugin
        C2S_MasterClientSwitchedInWaitngRoom,

        //the client that reach the exit tile will inform the plugin
        C2S_OneOftheClientWon,
        //the plugin will relay the information to all other clients
        S2C_OneOftheClientWon,

        //the client lose all 3 health will inform the plugin
        C2S_OneOftheClientLose,
        //the plugin will inform rest of the clients of the the client that lose all 3 health
        S2C_OneOftheClientLose,

        //The maptile that is going to be destroyed will send its tile index to the plugin
        C2S_DestroyMapTile,
        //the plugin will then relay the tile index of the tile that is going to be destroyed  to all clients excluding the sender client
        S2C_DestroyMapTile,

        //The map tile that is being stepped on  will sends its tile index  to the plugin
        C2S_StepOnTile,
        //the plugin will then relay the tile index of the tile being stepped on to all clients excluding the sender client
        S2C_StepOnTile,
        //The map tile that is being Interacted with will sends its tile index to the plugin
        C2S_InteractOnTile,
        //the plugin will then relay the tile index of the tile being Interacted with to all clients excluding the sender client
        S2C_InteractOnTile,

        //The map tile that is being entered will sends its tile index to the plugin
        C2S_EnterOnTile,
        //the plugin will then relay the tile index of the tile being entered to all clients excluding the sender client
        S2C_EnterOnTile,

        //In AR the the player spawn at offsetted position , therefore to ensure that all clients spawn on the same position,
        //  the master client  will send its position of all the playe game objects to other clients to use
        C2S_UpdatePosFromMasterClient,
        S2C_UpdatePosFromMasterClient,

    }
}
