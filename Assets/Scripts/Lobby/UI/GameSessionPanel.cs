namespace Lobby
{
	using Amazon.GameLift.Model;
	using UnityEngine;
	using UnityEngine.UI;

	public sealed class GameSessionPanel : MonoBehaviour
	{
		public Text NameText;
		public Text DateText;
		public Text InfoText;
		public Button JoinButton;

		LobbyManager m_Lobby;
		GameSession m_Session;

		void Start()
		{
			JoinButton.onClick.AddListener(Join);
		}

		async void Join()
		{
			if (m_Session != null) await m_Lobby.JoinRoom(m_Session.GameSessionId);
		}

		public void Init(LobbyManager lobby, GameSession session)
		{
			m_Lobby = lobby;
			m_Session = session;
			NameText.text = session.Name;
			DateText.text = session.CreationTime.ToLocalTime().ToString();
			InfoText.text = $"{session.CurrentPlayerSessionCount} / {session.MaximumPlayerSessionCount}";
		}
	}
}