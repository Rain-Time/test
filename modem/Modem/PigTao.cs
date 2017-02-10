using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Modem
{
	public class PigTao
	{
		public PigTao()
		{
		}

		public int CreatConnect(string name, string number, string modem, string user, string password)
		{
			byte[] bytes = Encoding.Default.GetBytes(modem);
			byte[] numArray = Encoding.Default.GetBytes(name);
			byte[] bytes1 = Encoding.Default.GetBytes(number);
			byte[] numArray1 = Encoding.Default.GetBytes(user);
			byte[] bytes2 = Encoding.Default.GetBytes(password);
			return PigTao.RasCreateEntry(numArray, bytes1, bytes, numArray1, bytes2);
		}

		public string[] EnumConnect()
		{
			byte[] numArray = new byte[0x2800];
			PigTao.MRasEnumDev(numArray);
			string str = Encoding.Default.GetString(numArray);
			char[] chrArray = new char[1];
			chrArray[0] = '*';
			return str.Split(chrArray);
		}

		[DllImport("CreatDial.dll", CharSet=CharSet.Auto)]
		public static extern void MRasEnumDev(byte[] c);

		[DllImport("CreatDial.dll", CharSet=CharSet.None)]
		public static extern int RasCreateEntry(byte[] Name, byte[] number, byte[] Modem, byte[] User, byte[] PWD);
	}
}