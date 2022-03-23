namespace Gameplay
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public sealed class GridManager : MonoBehaviour
	{
		public const int Height = 20;
		public const int Width = 10;
		public static readonly Vector2Int SpawnPos = new Vector2Int((Width - 1) / 2, Height - 1);

		public static Vector2Int RoundPos(Vector2 pos) =>
			new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

		List<Block[]> m_Blocks;

		public void Init()
		{
			m_Blocks = new List<Block[]>(Height);
			for (int i = 0; i < Height; i++) m_Blocks.Add(new Block[Width]);
		}

		public static bool InsideGrid(Vector2Int pos, bool tolerateHeight = false) =>
			InsideGrid(pos.y, pos.x, tolerateHeight);

		public static bool InsideGrid(int row, int column, bool tolerateHeight = false) =>
			row >= 0 && (tolerateHeight || row < Height) && 0 <= column && column < Width;

		public bool TryGetBlock(Vector2Int pos, out Block value) => TryGetBlock(pos.y, pos.x, out value);

		public bool TryGetBlock(int row, int column, out Block value)
		{
			bool ok = InsideGrid(row, column);
			value = ok ? GetBlock(row, column) : default;
			return ok;
		}

		public Block GetBlock(Vector2Int pos) => GetBlock(pos.y, pos.x);

		public Block GetBlock(int row, int column) => m_Blocks[row][column];

		public bool TrySetBlock(Vector2Int pos, Block value) => TrySetBlock(pos.y, pos.x, value);

		public bool TrySetBlock(int row, int column, Block value)
		{
			bool ok = InsideGrid(row, column);
			if (ok) SetBlock(row, column, value);
			return ok;
		}

		public void SetBlock(Vector2Int pos, Block value) => SetBlock(pos.y, pos.x, value);

		public void SetBlock(int row, int column, Block value) => m_Blocks[row][column] = value;

		public bool TryPlaceShape(ShapeController shape)
		{
			bool ok = CanPlaceShape(shape);
			if (ok) PlaceShape(shape);
			return ok;
		}

		public bool CanPlaceShape(ShapeController shape)
		{
			for (int i = 0, count = shape.Blocks.Length; i < count; i++)
			{
				var pos = RoundPos(shape.Blocks[i].transform.position);
				if (!InsideGrid(pos, true)) return false;
				if (pos.y < Height && !GetBlock(pos).CanPlace(shape)) return false;
			}

			return true;
		}

		public void PlaceShape(ShapeController shape)
		{
			for (int i = 0; i < Height; i++)
			for (int j = 0; j < Width; j++)
				if (GetBlock(i, j).ShapeId == shape.ShapeId)
					SetBlock(i, j, default);

			for (int i = 0, count = shape.Blocks.Length; i < count; i++)
				TrySetBlock(RoundPos(shape.Blocks[i].transform.position), shape);
		}

		public void Capture(byte[] blocks)
		{
			int index = 0;
			for (int i = 0; i < Height; i++)
			for (int j = 0; j < Width; j++, index++)
			{
				if (index >= blocks.Length) return;
				blocks[index] = (byte) GetBlock(i, j).ShapeType;
			}

			if (index < blocks.Length) Array.Clear(blocks, index, blocks.Length - index);
		}

		public int RemoveFullRows()
		{
			int count = 0;
			for (int i = 0; i < Height; i++)
				if (IsRowFull(i))
				{
					RemoveRow(i--);
					++count;
				}

			return count;
		}

		public bool IsRowFull(int i)
		{
			for (int j = 0; j < Width; j++)
				if (GetBlock(i, j).CanPlace())
					return false;
			return true;
		}

		public void RemoveRow(int i)
		{
			var row = m_Blocks[i];
			Array.Clear(row, 0, Width);
			m_Blocks.RemoveAt(i);
			m_Blocks.Add(row);
		}

		public void InsertObstacleRows(int startRow, int count, int seed)
		{
			for (int i = 0; i < count; i++, seed++)
			{
				var row = InsertRow(startRow);
				for (int j = 0; j < Width; j++)
				{
					var type = (j & 1) == (seed & 1) ? ShapeType.Other : ShapeType.None;
					row[j] = new Block(type, 0);
				}
			}
		}

		public Block[] InsertRow(int i)
		{
			int last = m_Blocks.Count - 1;
			var row = m_Blocks[last];
			m_Blocks.RemoveAt(last);
			m_Blocks.Insert(i, row);
			return row;
		}
	}
}