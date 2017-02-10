using Microsoft.Win32;
using System;

namespace Modem
{
	public class SetDefaultConnection
	{
		public SetDefaultConnection()
		{
		}

		public bool Set(string name)
		{
			bool flag;
			RegistryKey localMachine = Registry.LocalMachine;
			RegistryKey registryKey = null;
			try
			{
				registryKey = localMachine.OpenSubKey("SOFTWARE\\Microsoft\\RAS AutoDial\\Default", true);
				registryKey.SetValue("DefaultInternet", name);
				registryKey.Close();
			}
			catch
			{
				registryKey = null;
			}
			if (registryKey == null)
			{
				try
				{
					RegistryKey registryKey1 = localMachine.CreateSubKey("SOFTWARE\\Microsoft\\RAS AutoDial\\Default");
					registryKey1.SetValue("DefaultInternet", name);
					registryKey1.Close();
					return true;
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
			return true;
		}
	}
}