namespace Gameplay
{
	using System.Linq;
	using UnityEngine;

	public sealed class ScoreConfig : MonoBehaviour
	{
		[SerializeField] int[] m_Scores;

		void Reset() => m_Scores = new int[] {10, 30, 50, 70};

		public int Get(int rowCount) => m_Scores.ElementAtOrDefault(rowCount - 1);
	}
}