namespace Gameplay
{
	using UnityEngine;

	public sealed class InputConfig : MonoBehaviour
	{
		public KeyCode Left = KeyCode.LeftArrow;
		public KeyCode Right = KeyCode.RightArrow;
		public KeyCode RotateCW = KeyCode.X;
		public KeyCode RotateCWAlt = KeyCode.UpArrow;
		public KeyCode RotateCCW = KeyCode.Z;
		public KeyCode RotateCCWAlt = KeyCode.RightControl;
		public KeyCode SoftDrop = KeyCode.DownArrow;
		public KeyCode HardDrop = KeyCode.Space;
	}
}