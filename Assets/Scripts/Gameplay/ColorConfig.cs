namespace Gameplay
{
	using UnityEngine;

	public sealed class ColorConfig : MonoBehaviour
	{
		public Color[] ShapeColors;

		void Reset()
		{
			ShapeColors = new[]
			{
				Color.clear,
				Color.red,
				new Color(1, 0.4f, 0),
				Color.yellow,
				Color.green,
				Color.cyan,
				Color.blue,
				new Color(0.6f, 0, 1),
				Color.gray,
			};
		}
	}
}