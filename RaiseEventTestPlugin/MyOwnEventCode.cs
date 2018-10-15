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

    }
}
