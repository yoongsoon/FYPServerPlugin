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
        private int I_PlayerGoCount;
        private int I_NumberOfUpdatedPos;

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

            m_InfoRoom.I_NumberOfPlayers++;
        }


        public override void OnJoin(IJoinGameCallInfo info)
        {
            base.OnJoin(info);
            m_InfoRoom.I_NumberOfPlayers++;
        }

        //On leave is also called when player is inside a room or got disconnected
        public override void OnLeave(ILeaveGameCallInfo info)
        {
            base.OnLeave(info);
            m_InfoRoom.I_NumberOfPlayers--;
            if (m_InfoRoom.I_NoOfClientReady >= 0)
                m_InfoRoom.I_NoOfClientReady--;

            int numberPlayerCounter = m_InfoRoom.I_NumberOfPlayers - 1;


            if ((m_InfoRoom.I_NoOfClientReady < numberPlayerCounter) || (m_InfoRoom.I_NoOfClientReady == 0 && numberPlayerCounter == 0))
            {
                PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                     senderActor: 0,
                     targetGroup: 0,
                     data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                     evCode: (byte)MyOwnEventCode.S2C_Not_ReadyToStart,
                     cacheOp: 0);
            }
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
   
                        //Broadcast back to the requesting client
                        PluginHost.BroadcastEvent(recieverActors: new List<int> { info.ActorNr },
                                    senderActor: 0,
                                    data: new Dictionary<byte, object>() { {
                       (byte)245,  m_InfoRoom.I_RoomNumber }, { 254, 0 } },
                                    evCode: (byte)MyOwnEventCode.S2C_SendRoomID,
                                    cacheOp: 0);
                    }
                    break;

                case MyOwnEventCode.C2S_InformSuccessHost:
                    {

                        // send the enviro level index and level index stored in info.Request.Data to other clients                   
                        //Broadcast back to other clients except master client
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                                    senderActor: 0,
                                    targetGroup: 0,
                                    data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data }, { 254, 0 } },
                                    evCode: (byte)MyOwnEventCode.S2C_InformSuccessHost,
                                    cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_Ready:
                    {
                        m_InfoRoom.I_NoOfClientReady++;

                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                             senderActor: 0,
                             targetGroup: 0,
                             data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data }, { 254, 0 } },
                             evCode: (byte)MyOwnEventCode.S2C_Ready,
                             cacheOp: 0);


                        //when total number of players excluding the master client  equals or greater to the number of normal client ready to start the game
                        //send a message to the master client to inform him that he can start the game.
                        if (m_InfoRoom.I_NumberOfPlayers - 1 >= m_InfoRoom.I_NoOfClientReady)
                        {
                            PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                                 senderActor: 0,
                                 targetGroup: 0,
                                 data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                 evCode: (byte)MyOwnEventCode.S2C_ReadyToStart,
                                 cacheOp: 0);
                        }

                    }
                    break;
                case MyOwnEventCode.C2S_UnReady:
                    {
                        m_InfoRoom.I_NoOfClientReady--;

                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                       (byte)245,  info.Request.Data }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_UnReady,
                        cacheOp: 0);

                        //Number of clients is lesser than the total number of players excluding the master client , indicate not all the players are ready to start the game
                        if (m_InfoRoom.I_NoOfClientReady < m_InfoRoom.I_NumberOfPlayers - 1)
                        {
                            PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                                 senderActor: 0,
                                 targetGroup: 0,
                                 data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                 evCode: (byte)MyOwnEventCode.S2C_Not_ReadyToStart,
                                 cacheOp: 0);
                        }
                    }
                    break;
                case MyOwnEventCode.C2S_LeaveWaitingRoom:
                    {
                        //to all clients
                        PluginHost.BroadcastEvent(target: ReciverGroup.All,
                            senderActor: 0,
                            targetGroup: 0,
                            data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                            evCode: (byte)MyOwnEventCode.S2C_LeaveWaitingRoom,
                            cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_Request_To_SpawnPlayers:
                    {
                        I_PlayerGoCount++;

                        if (I_PlayerGoCount >= m_InfoRoom.I_NumberOfPlayers)
                        {
                            //broadcast back too all clients
                            PluginHost.BroadcastEvent(target: ReciverGroup.All,
                                senderActor: 0,
                                targetGroup: 0,
                                      data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                       evCode: (byte)MyOwnEventCode.S2C_Request_To_SpawnPlayers,
                                       cacheOp: 0);
                        }
                    }
                    break;
                case MyOwnEventCode.C2S_Anchor_Resolved_Success:
                    {
                        m_InfoRoom.I_NoOfSuccessfulResolved++;

                        //When the number of successful anchors resolved is greater or equal to the total
                        //number of clients (excluding the master client)
                        if (m_InfoRoom.I_NoOfSuccessfulResolved >= m_InfoRoom.I_NumberOfPlayers - 1)
                        {

                            //broadcast back to all clients that they can start the game
                            PluginHost.BroadcastEvent(target: ReciverGroup.All,
                                senderActor: 0,
                                targetGroup: 0,
                                      data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                       evCode: (byte)MyOwnEventCode.S2C_Anchor_Resolved_Success,
                                       cacheOp: 0);
                        }
                    }
                    break;
                case MyOwnEventCode.C2S_Start_LoadingScreen:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.All,
                           senderActor: 0,
                           targetGroup: 0,
                                 data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                  evCode: (byte)MyOwnEventCode.S2C_Start_LoadingScreen,
                                  cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_InformFailHost:
                    {
                        //inform others about the failure to host
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                          senderActor: 0,
                          targetGroup: 0,
                                data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                                 evCode: (byte)MyOwnEventCode.S2C_InformFailHost,
                                 cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_InfoAttemptToHost:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                              data: new Dictionary<byte, object>() { {
                       (byte)245, null }, { 254, 0 } },
                               evCode: (byte)MyOwnEventCode.S2C_InfoAttemptToHost,
                               cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_LeftRoom:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                     senderActor: 0,
                     targetGroup: 0,
                           data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data  }, { 254, 0 } },
                            evCode: (byte)MyOwnEventCode.S2C_LeftRoom,
                            cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_JoinedWaitingRoom:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                     senderActor: 0,
                     targetGroup: 0,
                           data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data  }, { 254, 0 } },
                            evCode: (byte)MyOwnEventCode.S2C_JoinedWaitingRoom,
                            cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_MasterClientSwitchedInWaitngRoom:
                    {
                        m_InfoRoom.I_NoOfClientReady = 0;
                    }
                    break;
                case MyOwnEventCode.C2S_OneOftheClientWon:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.All,
                   senderActor: 0,
                   targetGroup: 0,
                         data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data   }, { 254, 0 } },
                          evCode: (byte)MyOwnEventCode.S2C_OneOftheClientWon,
                          cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_OneOftheClientLose:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.All,
                senderActor: 0,
                targetGroup: 0,
                      data: new Dictionary<byte, object>() { {
                       (byte)245, info.Request.Data  }, { 254, 0 } },
                       evCode: (byte)MyOwnEventCode.S2C_OneOftheClientLose,
                       cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_DestroyMapTile:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                        (byte)245, info.Request.Data  }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_DestroyMapTile,
                        cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_StepOnTile:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                        (byte)245, info.Request.Data  }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_StepOnTile,
                        cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_EnterOnTile:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                        (byte)245, info.Request.Data  }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_EnterOnTile,
                        cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_InteractOnTile:
                    {
                        PluginHost.BroadcastEvent(target: ReciverGroup.Others,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                        (byte)245, info.Request.Data  }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_InteractOnTile,
                        cacheOp: 0);
                    }
                    break;
                case MyOwnEventCode.C2S_UpdatePosFromMasterClient:
                    {
                        I_NumberOfUpdatedPos++;

                        if (I_NumberOfUpdatedPos >= m_InfoRoom.I_NumberOfPlayers -1)
                        {
                            PluginHost.BroadcastEvent(target: ReciverGroup.All,
                        senderActor: 0,
                        targetGroup: 0,
                        data: new Dictionary<byte, object>() { {
                        (byte)245, null  }, { 254, 0 } },
                        evCode: (byte)MyOwnEventCode.S2C_UpdatePosFromMasterClient,
                        cacheOp: 0);
                        }
                    }
                    break;
            }
        }
    }

}
