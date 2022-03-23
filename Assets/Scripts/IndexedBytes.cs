public readonly struct IndexedBytes
{
	public readonly byte[] Data;
	public readonly int Index;

	public IndexedBytes(byte[] data, int index = 0) =>
		(Data, Index) = (data, index);

	public IndexedBytes MoveIndex(int offset) =>
		new IndexedBytes(Data, Index + offset);

	public IndexedBytes WriteInt32(int value)
	{
		Data[Index] = (byte) (value >> 24);
		Data[Index + 1] = (byte) (value >> 16);
		Data[Index + 2] = (byte) (value >> 8);
		Data[Index + 3] = (byte) value;
		return MoveIndex(sizeof(int));
	}

	public IndexedBytes ReadInt32(out int value)
	{
		value = Data[Index] << 24 | Data[Index + 1] << 16 | Data[Index + 2] << 8 | Data[Index + 3];
		return MoveIndex(sizeof(int));
	}
}

public static class IndexedBytesExtensions
{
	public static IndexedBytes AsIndexedBytes(this byte[] data, int index = 0) =>
		new IndexedBytes(data, index);
}