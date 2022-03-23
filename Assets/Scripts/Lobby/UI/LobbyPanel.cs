namespace Lobby
{
	using UnityEngine;
	using UnityEngine.UI;

	public sealed class LobbyPanel : MonoBehaviour
	{
		public LobbyManager Lobby;
		public InputField NameInput;
		public Button CreateButton;
		public Button SearchButton;
		public Button LocalPlayButton;
		public RectTransform Root;
		public GameObject Prefab;

		void Start()
		{
			CreateButton.onClick.AddListener(Create);
			SearchButton.onClick.AddListener(Search);
			LocalPlayButton.onClick.AddListener(Lobby.LocalPlay);
			Search();
		}

		async void Create()
		{
			await Lobby.CreateRoom(NameInput.text);
		}

		async void Search()
		{

            foreach (Transform child in Root)
                Destroy(child.gameObject);
            foreach (var session in await Lobby.SearchRooms())
                Instantiate(Prefab, Root).GetComponent<GameSessionPanel>().Init(Lobby, session);

        }
    }
}