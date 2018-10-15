using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Hive;
using Photon.Hive.Plugin;

namespace TestPlugin
{
    // There is a 1 to 1 relationship between room and instance of a plugin - so each room has its own instance of a plugin.
    public class RaiseEventTestPlugin : PluginBase
    {
        private RoomInfo m_InfoRoom;
        private CustomObject m_ObjectCustom = new CustomObject();

        public string ServerString
        {
            get;
            private set;
        }
        public int CallsCount
        {
            get;
            private set;
        }
        public RaiseEventTestPlugin()
        {
            this.UseStrictMode = true;
            this.ServerString = "ServerMessage";
            this.CallsCount = 0;

            m_ObjectCustom.Init();

        }
        public override string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        //Called by the first client who create the room
        public override void OnCreateGame(ICreateGameCallInfo info)
        {
            base.OnCreateGame(info);

            //Set a random integer as the room number
            Random rnd = new Random();
            m_InfoRoom.I_RoomNumber = rnd.Next(1, 9999);
        }


        public override void OnJoin(IJoinGameCallInfo info)
        {
            base.OnJoin(info);
            m_InfoRoom.I_NumberOfPlayers++;
        }
        public override void OnLeave(ILeaveGameCallInfo info)
        {
            base.OnLeave(info);
            m_InfoRoom.I_NumberOfPlayers--;
            //s
        }

        public override void OnRaiseEvent(IRaiseEventCallInfo info)
        {

            try
            {
                base.OnRaiseEvent(info);
            }
            catch (Exception e)
            {
                this.PluginHost.BroadcastErrorInfoEvent(e.ToString(), info);
                return;
            }

            switch ((MyOwnEventCode)info.Request.EvCode)
            {
                case MyOwnEventCode.C2S_RequestRoomID:
                    {
                        m_ObjectCustom.message = m_InfoRoom.I_RoomNumber.ToString();
                        byte[] roomNoInBytes = CustomObject.Serialize(m_ObjectCustom);

                        //Broadcast back to the requesting client
                        PluginHost.BroadcastEvent(recieverActors: new List<int> { info.ActorNr },
                                    senderActor: 0,
                                    data: new Dictionary<byte, object>() { {
                       (byte)245, roomNoInBytes }, { 254, 0 } },
                                    evCode: (byte)MyOwnEventCode.S2C_SendRoomID,
                                    cacheOp: 0);
                    }
                    break;

                case MyOwnEventCode.C2S_InformSuccessHost:
                    {

                       // send the enviro level index and level index stored in info.Request.Data to other clients                   
                        //Broadcast back to other clients except master client
                        PluginHost.BroadcastEvent(target : ReciverGroup.Others,
                                    senderActor: 0,
                                    targetGroup: 0,
                                    data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data }, { 254, 0 } },
                                    evCode: (byte)MyOwnEventCode.S2C_InformSuccessHost,
                                    cacheOp: 0);
                    }
                    break;

            }
        }
    }

}
