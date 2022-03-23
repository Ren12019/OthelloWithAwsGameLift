namespace Gameplay
{
	using System.Collections.Generic;
	using UnityEngine;

	public sealed class ShapeConfig : MonoBehaviour
	{
		public List<GameObject> ShapePrefabs;

		public void Init()
		{
			ShapePrefabs.RemoveAll(go => go == null);
			if (ShapePrefabs.Count == 0)
				Debug.LogError("ShapeConfig not set");
		}

		public ShapeController SpawnShape(Vector2Int pos)
		{
			int i = Random.Range(0, ShapePrefabs.Count);
			var go = Instantiate(ShapePrefabs[i], (Vector2) pos, Quaternion.identity);
			return go.GetComponent<ShapeController>();
		}
	}
}