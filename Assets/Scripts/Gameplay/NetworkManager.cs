namespace Gameplay
{
    using Amazon.GameLift.Model;
    using Aws.GameLift.Realtime;
    using Aws.GameLift.Realtime.Event;
    using System;
    using UnityEngine;
    using System.Threading;

    [RequireComponent(typeof(ColorConfig))]
    public sealed class NetworkManager : MonoBehaviour
    {
        public static PlayerSession PlayerSession;

        public Viewer Viewer;
        public ushort DefaultUdpPort = 7777;

        GameManager m_Game;
        ColorConfig m_Color;
        Client m_Client;
        bool m_Connected;
        SynchronizationContext context;

        void Awake()
        {
            context = SynchronizationContext.Current;
            m_Game = GetComponent<GameManager>();
            m_Color = GetComponent<ColorConfig>();
            m_Client = new Client();
            m_Client.ConnectionOpen += OnOpen;
            m_Client.ConnectionClose += OnClose;
            m_Client.ConnectionError += OnError;
            m_Client.DataReceived += OnDataThread;
        }

        void OnEnable()
        {
            if (PlayerSession == null)
            {
                enabled = false;
                Debug.Log("NetworkManager.PlayerSession not set");
                return;
            }

            int udpPort = NetworkUtility.SearchAvailableUdpPort(DefaultUdpPort, DefaultUdpPort + 100);
            if (udpPort <= 0)
            {
                Debug.LogError("No available UDP port");
                return;
            }

            m_Client.Connect(PlayerSession.IpAddress, PlayerSession.Port, udpPort,
                new ConnectionToken(PlayerSession.PlayerSessionId, null));
        }

        void OnDisable()
        {
            if (m_Client.Connected) m_Client.Disconnect();
        }

        void OnOpen(object sender, EventArgs e)
        {
            Debug.Log("OnOpen");
            m_Connected = true;
        }

        void OnClose(object sender, EventArgs e)
        {
            Debug.Log("OnClose");
            m_Connected = false;
        }

        void OnError(object sender, ErrorEventArgs e)
        {
            Debug.LogError("OnError");
            Debug.LogException(e.Exception);
        }

        void OnDataThread(object sender, DataReceivedEventArgs e)
        {
            context.Post(_ =>
            {
                OnData(sender, e);
            }, null);
        }

        void OnData(object sender, DataReceivedEventArgs e)
        {
            switch (e.OpCode)
            {
                case OpCode.ServerStartGame:
                    if (m_Game) m_Game.enabled = true;
                    break;
                case OpCode.ClientUpdateBlocks:
                    if (e.Sender != m_Client.Session.ConnectedPeerId)
                        Viewer.SetBlocks(e.Data, m_Color.ShapeColors);
                    break;
            }
        }

        public void SendReady()
        {
            if (!m_Connected) return;
            m_Client.SendEvent(OpCode.ClientReadyToStart);
        }

        public void SendUpdateBlocks(byte[] data)
        {
            if (!m_Connected) return;
            var message = m_Client.NewMessage(OpCode.ClientUpdateBlocks).WithTargetGroup(Constants.GROUP_ID_ALL_PLAYERS).WithPayload(data);
            m_Client.SendMessage(message);
        }
    }

    public static class OpCode
    {
        // 1xx Server to Client
        public const int ServerStartGame = 101;

        // 2xx Client to Server
        public const int ClientReadyToStart = 201;

        // 3xx Client to Client
        public const int ClientUpdateBlocks = 301;
    }
}
