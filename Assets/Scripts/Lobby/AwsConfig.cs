namespace Lobby
{
	using UnityEngine;

	public sealed class AwsConfig : MonoBehaviour
	{
		public AwsRegionType Region;
		public string AccessKeyId;
		public string SecretAccessKey;
		public string GameLiftAliasId;

		public bool IsValid() =>
			IsValid(AccessKeyId, nameof(AccessKeyId)) &
			IsValid(SecretAccessKey, nameof(SecretAccessKey)) &
			IsValid(GameLiftAliasId, nameof(GameLiftAliasId));

		static bool IsValid(string value, string name)
		{
			if (!string.IsNullOrWhiteSpace(value)) return true;
			Debug.LogError($"{nameof(AwsConfig)}.{name} not set");
			return false;
		}
	}
}