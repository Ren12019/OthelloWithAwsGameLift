namespace Gameplay
{
	public readonly struct Block
	{
		public readonly ShapeType ShapeType;
		public readonly int ShapeId;

		public Block(ShapeType shapeType, int shapeId)
		{
			ShapeType = shapeType;
			ShapeId = shapeId;
		}

		public bool CanPlace() => ShapeType == ShapeType.None;
		public bool CanPlace(ShapeController shape) => CanPlace() || ShapeId == shape.ShapeId;

		public static implicit operator Block(ShapeController shape) =>
			shape != null ? new Block(shape.Type, shape.ShapeId) : default;
	}
}