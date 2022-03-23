namespace Lobby
{
    using Amazon.GameLift;
    using Amazon.GameLift.Model;
    using Gameplay;
    using System;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [RequireComponent(typeof(AwsConfig))]
    public sealed class LobbyManager : MonoBehaviour
    {
        AwsConfig m_Config;

        void Awake()
        {
            m_Config = GetComponent<AwsConfig>();
        }

        AmazonGameLiftClient m_GameLift;

        void OnEnable()
        {
            if (!m_Config.IsValid())
            {
                enabled = false;
                return;
            }

            m_GameLift = new AmazonGameLiftClient(m_Config.AccessKeyId,
                m_Config.SecretAccessKey, m_Config.Region.ToEndpoint());
        }

        void OnDisable()
        {
            m_GameLift?.Dispose();
        }

        public async Task<PlayerSession> CreateRoom(string roomName = null)
        {
            Debug.Log("CreateRoom");
            if (string.IsNullOrEmpty(roomName)) roomName = Guid.NewGuid().ToString();
            var request = new CreateGameSessionRequest
            {
                AliasId = m_Config.GameLiftAliasId,
                MaximumPlayerSessionCount = 2,
                Name = roomName
            };
            var response = await m_GameLift.CreateGameSessionAsync(request);
            return await JoinRoom(response.GameSession.GameSessionId);
        }

        public async Task<PlayerSession> JoinRoom(string gameSessionId)
        {
            Debug.Log("JoinRoom");
            var response = await m_GameLift.CreatePlayerSessionAsync(new CreatePlayerSessionRequest
            {
                GameSessionId = gameSessionId,
                PlayerId = SystemInfo.deviceUniqueIdentifier,
            });
            var playerSession = response.PlayerSession;
            NetworkManager.PlayerSession = playerSession;
            SceneManager.LoadScene("Gameplay");
            return playerSession;
        }

        public async Task<List<GameSession>> SearchRooms()
        {
            Debug.Log("SearchRooms");
            var response = await m_GameLift.SearchGameSessionsAsync(new SearchGameSessionsRequest
            {
                AliasId = m_Config.GameLiftAliasId,
            });
            return response.GameSessions;
        }

        public void LocalPlay()
        {
            NetworkManager.PlayerSession = null;
            SceneManager.LoadScene("Gameplay");
        }
    }
}
