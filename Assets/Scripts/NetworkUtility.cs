using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;

public static class NetworkUtility
{
	public static int SearchAvailableUdpPort(int from = 1024, int to = ushort.MaxValue)
	{
		from = Mathf.Clamp(from, 1, ushort.MaxValue);
		to = Mathf.Clamp(to, 1, ushort.MaxValue);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		var set = LsofUdpPorts(from, to);
#else
        var set = GetActiveUdpPorts();
#endif
		for (int port = from; port <= to; port++)
			if (!set.Contains(port))
				return port;
		return -1;
	}

	static HashSet<int> GetActiveUdpPorts()
	{
		return new HashSet<int>(IPGlobalProperties.GetIPGlobalProperties()
			.GetActiveUdpListeners().Select(listener => listener.Port));
	}

	static HashSet<int> LsofUdpPorts(int from, int to)
	{
		var set = new HashSet<int>();
		string command = string.Join(" | ",
			$"lsof -nP -iUDP:{from.ToString()}-{to.ToString()}",
			"sed -E 's/->[0-9.:]+$//g'",
			@"grep -Eo '\d+$'");
		var process = Process.Start(new ProcessStartInfo
		{
			FileName = "/bin/bash",
			Arguments = $"-c \"{command}\"",
			RedirectStandardOutput = true,
			UseShellExecute = false,
		});
		if (process != null)
		{
			process.WaitForExit();
			var stream = process.StandardOutput;
			while (!stream.EndOfStream)
				if (int.TryParse(stream.ReadLine(), out int port))
					set.Add(port);
		}

		return set;
	}
}