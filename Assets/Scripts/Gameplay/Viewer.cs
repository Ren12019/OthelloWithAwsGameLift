namespace Gameplay
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public sealed class Viewer : MonoBehaviour
	{
		public Text ScoreText;
		public Text LineText;
		public List<Image> Cells;

		void Reset()
		{
			GetComponentsInChildren(true, Cells = new List<Image>());
			Cells.RemoveAll(img => img.gameObject == gameObject);
		}

		public void SetBlocks(IList<byte> blocks, IList<Color> colors)
		{
			int cellCount = Cells.Count;
			int blockCount = Mathf.Min(blocks.Count, cellCount);
			int colorCount = colors.Count;
			var defaultColor = colorCount > 0 ? colors[0] : default;
			for (int i = 0; i < blockCount; i++)
			{
				int colorIndex = blocks[i];
				Cells[i].color = colorIndex < colorCount ? colors[colorIndex] : defaultColor;
			}

			for (int i = blockCount; i < cellCount; i++)
				Cells[i].color = defaultColor;
		}

		public void SetScore(int score, int line)
		{
			if (ScoreText) ScoreText.text = score.ToString();
			if (LineText) LineText.text = line.ToString();
		}
	}
}