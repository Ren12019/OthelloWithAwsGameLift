namespace Gameplay
{
	using UnityEngine;

	[RequireComponent(typeof(ColorConfig))]
	[RequireComponent(typeof(GridManager))]
	[RequireComponent(typeof(InputConfig))]
	[RequireComponent(typeof(ScoreConfig))]
	[RequireComponent(typeof(ShapeConfig))]
	public sealed class GameManager : MonoBehaviour
	{
		public Viewer Viewer;
		public float FallInterval = 0.75f;

		NetworkManager m_Network;
		ColorConfig m_Color;
		GridManager m_Grid;
		InputConfig m_Input;
		ScoreConfig m_Score;
		ShapeConfig m_Shape;

		ShapeController m_CurrentShape;
		int m_ShapeCounter;
		float m_LastFallTime;
		bool m_GridDirty;
		bool m_ViewerDirty;
		byte[] m_ViewerBuffer;
		int m_LineScore;
		int m_TotalScore;

		void Awake()
		{
			m_Network = GetComponent<NetworkManager>();
			m_Color = GetComponent<ColorConfig>();
			m_Grid = GetComponent<GridManager>();
			m_Input = GetComponent<InputConfig>();
			m_Score = GetComponent<ScoreConfig>();
			m_Shape = GetComponent<ShapeConfig>();
			m_ViewerBuffer = new byte[GridManager.Height * GridManager.Width];
		}

		void Start()
		{
			m_Grid.Init();
			m_Shape.Init();
			m_ShapeCounter = 0;
		}

		void Update()
		{
			if (m_CurrentShape == null && !SpawnShape())
			{
				// TODO game over
				m_Grid.PlaceShape(m_CurrentShape);
				enabled = false;
			}
			else
			{
				UpdateInput();
				if (Time.time - m_LastFallTime >= FallInterval) SoftDrop();
				UpdateGrid();
			}

			UpdateViewer();
		}

		bool SpawnShape()
		{
			m_CurrentShape = m_Shape.SpawnShape(GridManager.SpawnPos);
			m_CurrentShape.Init(++m_ShapeCounter, m_Grid);
			m_LastFallTime = Time.time;
			SetViewerDirty();
			return m_Grid.TryPlaceShape(m_CurrentShape);
		}

		void UpdateInput()
		{
			if (Input.GetKeyDown(m_Input.Left)) Move(Vector2Int.left);
			if (Input.GetKeyDown(m_Input.Right)) Move(Vector2Int.right);
			if (Input.GetKeyDown(m_Input.RotateCW) || Input.GetKeyDown(m_Input.RotateCWAlt)) Rotate(true);
			if (Input.GetKeyDown(m_Input.RotateCCW) || Input.GetKeyDown(m_Input.RotateCCWAlt)) Rotate(false);
			if (Input.GetKeyDown(m_Input.HardDrop)) HardDrop();
			else if (Input.GetKeyDown(m_Input.SoftDrop)) SoftDrop();
		}

		bool Move(Vector2Int offset)
		{
			bool ok = m_CurrentShape.Move(offset);
			if (ok) SetViewerDirty();
			return ok;
		}

		bool Rotate(bool clockwise)
		{
			bool ok = m_CurrentShape.Rotate(clockwise);
			if (ok) SetViewerDirty();
			return ok;
		}

		bool SoftDrop()
		{
			bool ok = Move(Vector2Int.down);
			if (!ok)
			{
				Destroy(m_CurrentShape.gameObject);
				SetGridDirty();
			}

			m_LastFallTime = Time.time;
			return ok;
		}

		bool HardDrop()
		{
			bool ok = false;
			while (SoftDrop()) ok = true;
			return ok;
		}

		void SetGridDirty() => m_GridDirty = true;

		void UpdateGrid()
		{
			if (!m_GridDirty) return;
			int count = m_Grid.RemoveFullRows();
			if (count > 0)
			{
				m_LineScore += count;
				m_TotalScore += m_Score.Get(count);
				Viewer.SetScore(m_TotalScore, m_LineScore);
			}

			m_GridDirty = false;
		}

		void SetViewerDirty() => m_ViewerDirty = true;

		void UpdateViewer()
		{
			if (!m_ViewerDirty) return;
			m_Grid.Capture(m_ViewerBuffer);
			Viewer.SetBlocks(m_ViewerBuffer, m_Color.ShapeColors);
			if (m_Network) m_Network.SendUpdateBlocks(m_ViewerBuffer);
			m_ViewerDirty = false;
		}

		public void InsertObstacleRows(int count)
		{
			m_Grid.InsertObstacleRows(0, count, m_ShapeCounter);
			SetViewerDirty();
		}
	}
}