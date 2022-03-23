namespace Lobby
{
	using Amazon;

	public enum AwsRegionType
	{
		USEast1,
		USEast2,
		USWest1,
		USWest2,
		EUNorth1,
		EUWest1,
		EUWest2,
		EUWest3,
		EUCentral1,
		APNortheast1,
		APNortheast2,
		APNortheast3,
		APSouth1,
		APSoutheast1,
		APSoutheast2,
		SAEast1,
		USGovCloudEast1,
		USGovCloudWest1,
		CNNorth1,
		CNNorthWest1,
		CACentral1,
	}

	public static class AwsRegionTypeExtensions
	{
		public static RegionEndpoint ToEndpoint(this AwsRegionType region)
		{
			switch (region)
			{
				case AwsRegionType.USEast1: return RegionEndpoint.USEast1;
				case AwsRegionType.USEast2: return RegionEndpoint.USEast2;
				case AwsRegionType.USWest1: return RegionEndpoint.USWest1;
				case AwsRegionType.USWest2: return RegionEndpoint.USWest2;
				case AwsRegionType.EUNorth1: return RegionEndpoint.EUNorth1;
				case AwsRegionType.EUWest1: return RegionEndpoint.EUWest1;
				case AwsRegionType.EUWest2: return RegionEndpoint.EUWest2;
				case AwsRegionType.EUWest3: return RegionEndpoint.EUWest3;
				case AwsRegionType.EUCentral1: return RegionEndpoint.EUCentral1;
				case AwsRegionType.APNortheast1: return RegionEndpoint.APNortheast1;
				case AwsRegionType.APNortheast2: return RegionEndpoint.APNortheast2;
				case AwsRegionType.APNortheast3: return RegionEndpoint.APNortheast3;
				case AwsRegionType.APSouth1: return RegionEndpoint.APSouth1;
				case AwsRegionType.APSoutheast1: return RegionEndpoint.APSoutheast1;
				case AwsRegionType.APSoutheast2: return RegionEndpoint.APSoutheast2;
				case AwsRegionType.SAEast1: return RegionEndpoint.SAEast1;
				case AwsRegionType.USGovCloudEast1: return RegionEndpoint.USGovCloudEast1;
				case AwsRegionType.USGovCloudWest1: return RegionEndpoint.USGovCloudWest1;
				case AwsRegionType.CNNorth1: return RegionEndpoint.CNNorth1;
				case AwsRegionType.CNNorthWest1: return RegionEndpoint.CNNorthWest1;
				case AwsRegionType.CACentral1: return RegionEndpoint.CACentral1;
				default: return null;
			}
		}
	}
}