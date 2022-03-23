namespace Gameplay
{
	using UnityEngine;
	using UnityEngine.SceneManagement;
	using UnityEngine.UI;

	public sealed class GamePanel : MonoBehaviour
	{
		public GameManager Game;
		public NetworkManager Network;
		public Text ModeText;
		public Button ReadyButton;
		public Button LobbyButton;

		void Start()
		{
			ReadyButton.onClick.AddListener(Ready);
			LobbyButton.onClick.AddListener(Lobby);
			ModeText.text = NetworkManager.PlayerSession == null ? "Single" : "Multi";
		}

		void Ready()
		{
			if (NetworkManager.PlayerSession == null) Game.enabled = true;
			else Network.SendReady();
			ReadyButton.interactable = false;
		}

		void Lobby()
		{
			SceneManager.LoadScene("Lobby");
		}
	}
}