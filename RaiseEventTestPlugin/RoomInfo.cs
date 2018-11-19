using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPlugin
{
    //store all room related info in this struct
    public struct RoomInfo
    {
        //The room number
        public int I_RoomNumber;
        //The number of player in the room
        public int I_NumberOfPlayers;
        //The number of clients that pressed the ready button in the waiting room ( at the Unity client side)
        public int I_NoOfClientReady;
        //The number of cloud anchor successfully resolved
        public int I_NoOfSuccessfulResolved;
    }
}
