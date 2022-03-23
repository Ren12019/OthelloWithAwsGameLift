namespace Gameplay
{
	using UnityEngine;

	public sealed class ShapeController : MonoBehaviour
	{
		public ShapeType Type;
		public Transform Pivot;
		public SpriteRenderer[] Blocks;

		void Reset()
		{
			Pivot = transform.Find(nameof(Pivot));
			Blocks = GetComponentsInChildren<SpriteRenderer>(true);
		}

		public int ShapeId { get; private set; }
		GridManager m_Grid;

		public void Init(int shapeId, GridManager grid)
		{
			ShapeId = shapeId;
			m_Grid = grid;
		}

		public bool Move(Vector2Int offset)
		{
			var delta = new Vector3(offset.x, offset.y);
			transform.position += delta;
			bool ok = m_Grid.TryPlaceShape(this);
			if (!ok) transform.position -= delta;
			return ok;
		}

		public bool Rotate(bool clockwise)
		{
			float angle = clockwise ? -90f : 90f;
			transform.RotateAround(Pivot.position, Vector3.forward, angle);
			bool ok = m_Grid.TryPlaceShape(this);
			if (!ok) transform.RotateAround(Pivot.position, Vector3.forward, -angle);
			return ok;
		}
	}

	public enum ShapeType
	{
		None,
		I,
		J,
		L,
		O,
		S,
		T,
		Z,
		Other,
	}
}