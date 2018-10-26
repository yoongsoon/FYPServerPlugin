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
        //When all the normal clients are ready to start the game , the plugin will send a message to the master client
        // to inform him that he can start the game
        S2C_ReadyToStart = 8,
        //normal client send message to plugin to inform that the normal client is Not ready to start the game
        C2S_UnReady,

        //master client relay information to all clients(including himself) through the plugin to leave the waiting room
        C2S_LeaveWaitingRoom,
        S2C_LeaveWaitingRoom,

        // each client request from the plugin to spawn ALL the players
        C2S_Request_To_SpawnPlayers,
        S2C_Request_To_SpawnPlayers,
        //each client request to respawn its own player and inform other clients
        C2S_RequestToRespawnPlayer,
        S2C_RequestToRespawnPlayer,

        //Non master clients that have their anchor successfuly resolved , will inform the plugin
        C2S_Anchor_Resolved_Success,
        //once the plugin received all the non master clients message of successful anchor resolved,
        // the plugin will send message to all the clients (including master client) to start the game
        S2C_Anchor_Resolved_Success,
    }
}
