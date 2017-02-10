using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Modem
{
	public class InstallMainClass
	{
		public const int REG_NONE = 0;

		public const int REG_SZ = 1;

		public const int REG_EXPAND_SZ = 2;

		public const int REG_BINARY = 3;

		public const int REG_DWORD = 4;

		public const int REG_DWORD_LITTLE_ENDIAN = 4;

		public const int REG_DWORD_BIG_ENDIAN = 5;

		public const int REG_LINK = 6;

		public const int REG_MULTI_SZ = 7;

		public const int REG_RESOURCE_LIST = 8;

		public const int REG_FULL_RESOURCE_DESCRIPTOR = 9;

		public const int REG_RESOURCE_REQUIREMENTS_LIST = 10;

		public const int REG_QWORD = 11;

		public const int REG_QWORD_LITTLE_ENDIAN = 11;

		public const uint APPLICATION_ERROR_MASK = 0x20000000;

		public const uint ERROR_SEVERITY_SUCCESS = 0;

		public const uint ERROR_SEVERITY_INFORMATIONAL = 0x40000000;

		public const uint ERROR_SEVERITY_WARNING = 0x80000000;

		public const uint ERROR_SEVERITY_ERROR = 0xc0000000;

		public const uint ERROR_NO_ASSOCIATED_CLASS = 0xe0000200;

		public const uint ERROR_CLASS_MISMATCH = 0xe0000201;

		public const uint ERROR_DUPLICATE_FOUND = 0xe0000202;

		public const uint ERROR_NO_DRIVER_SELECTED = 0xe0000203;

		public const uint ERROR_KEY_DOES_NOT_EXIST = 0xe0000204;

		public const uint ERROR_INVALID_DEVINST_NAME = 0xe0000205;

		public const uint ERROR_INVALID_CLASS = 0xe0000206;

		public const uint ERROR_DEVINST_ALREADY_EXISTS = 0xe0000207;

		public const uint ERROR_DEVINFO_NOT_REGISTERED = 0xe0000208;

		public const uint ERROR_INVALID_REG_PROPERTY = 0xe0000209;

		public const uint ERROR_NO_INF = 0xe000020a;

		public const uint ERROR_NO_SUCH_DEVINST = 0xe000020b;

		public const uint ERROR_CANT_LOAD_CLASS_ICON = 0xe000020c;

		public const uint ERROR_INVALID_CLASS_INSTALLER = 0xe000020d;

		public const uint ERROR_DI_DO_DEFAULT = 0xe000020e;

		public const uint ERROR_DI_NOFILECOPY = 0xe000020f;

		public const uint ERROR_INVALID_HWPROFILE = 0xe0000210;

		public const uint ERROR_NO_DEVICE_SELECTED = 0xe0000211;

		public const uint ERROR_DEVINFO_LIST_LOCKED = 0xe0000212;

		public const uint ERROR_DEVINFO_DATA_LOCKED = 0xe0000213;

		public const uint ERROR_DI_BAD_PATH = 0xe0000214;

		public const uint ERROR_NO_CLASSINSTALL_PARAMS = 0xe0000215;

		public const uint ERROR_FILEQUEUE_LOCKED = 0xe0000216;

		public const uint ERROR_BAD_SERVICE_INSTALLSECT = 0xe0000217;

		public const uint ERROR_NO_CLASS_DRIVER_LIST = 0xe0000218;

		public const uint ERROR_NO_ASSOCIATED_SERVICE = 0xe0000219;

		public const uint ERROR_NO_DEFAULT_DEVICE_INTERFACE = 0xe000021a;

		public const uint ERROR_DEVICE_INTERFACE_ACTIVE = 0xe000021b;

		public const uint ERROR_DEVICE_INTERFACE_REMOVED = 0xe000021c;

		public const uint ERROR_BAD_INTERFACE_INSTALLSECT = 0xe000021d;

		public const uint ERROR_NO_SUCH_INTERFACE_CLASS = 0xe000021e;

		public const uint ERROR_INVALID_REFERENCE_STRING = 0xe000021f;

		public const uint ERROR_INVALID_MACHINENAME = 0xe0000220;

		public const uint ERROR_REMOTE_COMM_FAILURE = 0xe0000221;

		public const uint ERROR_MACHINE_UNAVAILABLE = 0xe0000222;

		public const uint ERROR_NO_CONFIGMGR_SERVICES = 0xe0000223;

		public const uint ERROR_INVALID_PROPPAGE_PROVIDER = 0xe0000224;

		public const uint ERROR_NO_SUCH_DEVICE_INTERFACE = 0xe0000225;

		public const uint ERROR_DI_POSTPROCESSING_REQUIRED = 0xe0000226;

		public const uint ERROR_INVALID_COINSTALLER = 0xe0000227;

		public const uint ERROR_NO_COMPAT_DRIVERS = 0xe0000228;

		public const uint ERROR_NO_DEVICE_ICON = 0xe0000229;

		public const uint ERROR_INVALID_INF_LOGCONFIG = 0xe000022a;

		public const uint ERROR_DI_DONT_INSTALL = 0xe000022b;

		public const uint ERROR_INVALID_FILTER_DRIVER = 0xe000022c;

		public const uint ERROR_NON_WINDOWS_NT_DRIVER = 0xe000022d;

		public const uint ERROR_NON_WINDOWS_DRIVER = 0xe000022e;

		public const uint ERROR_NO_CATALOG_FOR_OEM_INF = 0xe000022f;

		public const uint ERROR_DEVINSTALL_QUEUE_NONNATIVE = 0xe0000230;

		public const uint ERROR_NOT_DISABLEABLE = 0xe0000231;

		public const uint ERROR_CANT_REMOVE_DEVINST = 0xe0000232;

		public const uint ERROR_INVALID_TARGET = 0xe0000233;

		public const uint ERROR_DRIVER_NONNATIVE = 0xe0000234;

		public const uint ERROR_IN_WOW64 = 0xe0000235;

		public const uint ERROR_SET_SYSTEM_RESTORE_POINT = 0xe0000236;

		public const uint ERROR_INCORRECTLY_COPIED_INF = 0xe0000237;

		public const uint ERROR_SCE_DISABLED = 0xe0000238;

		public const int KEY_QUERY_VALUE = 1;

		public const int KEY_SET_VALUE = 2;

		public const int KEY_CREATE_SUB_KEY = 4;

		public const int KEY_ENUMERATE_SUB_KEYS = 8;

		public const int KEY_NOTIFY = 16;

		public const int KEY_CREATE_LINK = 32;

		public const int KEY_WOW64_32KEY = 0x200;

		public const int KEY_WOW64_64KEY = 0x100;

		public const int KEY_WOW64_RES = 0x300;

		public const uint KEY_ALL_ACCESS = 0xf003f;

		public const uint DELETE = 0x10000;

		public const uint READ_CONTROL = 0x20000;

		public const uint WRITE_DAC = 0x40000;

		public const uint WRITE_OWNER = 0x80000;

		public const uint SYNCHRONIZE = 0x100000;

		public const uint STANDARD_RIGHTS_REQUIRED = 0xf0000;

		public const uint STANDARD_RIGHTS_READ = 0x20000;

		public const uint STANDARD_RIGHTS_WRITE = 0x20000;

		public const uint STANDARD_RIGHTS_EXECUTE = 0x20000;

		public const uint STANDARD_RIGHTS_ALL = 0x1f0000;

		public const uint SPECIFIC_RIGHTS_ALL = 0xffff;

		public const int DIREG_DEV = 1;

		public const int DIREG_DRV = 2;

		public const int DIREG_BOTH = 4;

		public const int DICS_FLAG_GLOBAL = 1;

		public const int DICS_FLAG_CONFIGSPECIFIC = 2;

		public const int DICS_FLAG_CONFIGGENERAL = 4;

		public const int FACILITY_WINDOWS_CE = 24;

		public const int FACILITY_WINDOWS = 8;

		public const int FACILITY_URT = 19;

		public const int FACILITY_UMI = 22;

		public const int FACILITY_SXS = 23;

		public const int FACILITY_STORAGE = 3;

		public const int FACILITY_STATE_MANAGEMENT = 34;

		public const int FACILITY_SSPI = 9;

		public const int FACILITY_SCARD = 16;

		public const int FACILITY_SETUPAPI = 15;

		public const int FACILITY_SECURITY = 9;

		public const int FACILITY_RPC = 1;

		public const int FACILITY_WIN32 = 7;

		public const int FACILITY_CONTROL = 10;

		public const int FACILITY_NULL = 0;

		public const int FACILITY_METADIRECTORY = 35;

		public const int FACILITY_MSMQ = 14;

		public const int FACILITY_MEDIASERVER = 13;

		public const int FACILITY_INTERNET = 12;

		public const int FACILITY_ITF = 4;

		public const int FACILITY_HTTP = 25;

		public const int FACILITY_DPLAY = 21;

		public const int FACILITY_DISPATCH = 2;

		public const int FACILITY_CONFIGURATION = 33;

		public const int FACILITY_COMPLUS = 17;

		public const int FACILITY_CERT = 11;

		public const int FACILITY_BACKGROUNDCOPY = 32;

		public const int FACILITY_ACS = 20;

		public const int FACILITY_AAF = 18;

		public const int ERROR_INVALID_DATA = 13;

		public const uint ERROR_SUCCESS = 0;

		public const int ERROR_INSUFFICIENT_BUFFER = 122;

		public const int NO_ERROR = 0;

		public const uint INVALID_HANDLE_VALUE = 0xffffffff;

		public const int DIGCF_DEFAULT = 1;

		public const int DIGCF_PRESENT = 2;

		public const int DIGCF_ALLCLASSES = 4;

		public const int DIGCF_PROFILE = 8;

		public const int DIGCF_DEVICEINTERFACE = 16;

		public const int INSTALLFLAG_FORCE = 1;

		public const int INSTALLFLAG_READONLY = 2;

		public const int INSTALLFLAG_NONINTERACTIVE = 4;

		public const int INSTALLFLAG_BITS = 7;

		public const int DIF_SELECTDEVICE = 1;

		public const int DIF_INSTALLDEVICE = 2;

		public const int DIF_ASSIGNRESOURCES = 3;

		public const int DIF_PROPERTIES = 4;

		public const int DIF_REMOVE = 5;

		public const int DIF_FIRSTTIMESETUP = 6;

		public const int DIF_FOUNDDEVICE = 7;

		public const int DIF_SELECTCLASSDRIVERS = 8;

		public const int DIF_VALIDATECLASSDRIVERS = 9;

		public const int DIF_INSTALLCLASSDRIVERS = 10;

		public const int DIF_CALCDISKSPACE = 11;

		public const int DIF_DESTROYPRIVATEDATA = 12;

		public const int DIF_VALIDATEDRIVER = 13;

		public const int DIF_MOVEDEVICE = 14;

		public const int DIF_DETECT = 15;

		public const int DIF_INSTALLWIZARD = 16;

		public const int DIF_DESTROYWIZARDDATA = 17;

		public const int DIF_PROPERTYCHANGE = 18;

		public const int DIF_ENABLECLASS = 19;

		public const int DIF_DETECTVERIFY = 20;

		public const int DIF_INSTALLDEVICEFILES = 21;

		public const int DIF_UNREMOVE = 22;

		public const int DIF_SELECTBESTCOMPATDRV = 23;

		public const int DIF_ALLOW_INSTALL = 24;

		public const int DIF_REGISTERDEVICE = 25;

		public const int DIF_NEWDEVICEWIZARD_PRESELECT = 26;

		public const int DIF_NEWDEVICEWIZARD_SELECT = 27;

		public const int DIF_NEWDEVICEWIZARD_PREANALYZE = 28;

		public const int DIF_NEWDEVICEWIZARD_POSTANALYZE = 29;

		public const int DIF_NEWDEVICEWIZARD_FINISHINSTALL = 30;

		public const int DIF_UNUSED1 = 31;

		public const int DIF_INSTALLINTERFACES = 32;

		public const int DIF_DETECTCANCEL = 33;

		public const int DIF_REGISTER_COINSTALLERS = 34;

		public const int DIF_ADDPROPERTYPAGE_ADVANCED = 35;

		public const int DIF_ADDPROPERTYPAGE_BASIC = 36;

		public const int DIF_RESERVED1 = 37;

		public const int DIF_TROUBLESHOOTER = 38;

		public const int DIF_POWERMESSAGEWAKE = 39;

		public const int DIF_ADDREMOTEPROPERTYPAGE_ADVANCED = 40;

		public const int DIF_UPDATEDRIVER_UI = 41;

		public const int DIF_RESERVED2 = 48;

		public const int SPDRP_DEVICEDESC = 0;

		public const int SPDRP_HARDWAREID = 1;

		public const int SPDRP_COMPATIBLEIDS = 2;

		public const int SPDRP_UNUSED0 = 3;

		public const int SPDRP_SERVICE = 4;

		public const int SPDRP_UNUSED1 = 5;

		public const int SPDRP_UNUSED2 = 6;

		public const int SPDRP_CLASS = 7;

		public const int SPDRP_CLASSGUID = 8;

		public const int SPDRP_DRIVER = 9;

		public const int SPDRP_CONFIGFLAGS = 10;

		public const int SPDRP_MFG = 11;

		public const int SPDRP_FRIENDLYNAME = 12;

		public const int SPDRP_LOCATION_INFORMATION = 13;

		public const int SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 14;

		public const int SPDRP_CAPABILITIES = 15;

		public const int SPDRP_UI_NUMBER = 16;

		public const int SPDRP_UPPERFILTERS = 17;

		public const int SPDRP_LOWERFILTERS = 18;

		public const int SPDRP_BUSTYPEGUID = 19;

		public const int SPDRP_LEGACYBUSTYPE = 20;

		public const int SPDRP_BUSNUMBER = 21;

		public const int SPDRP_ENUMERATOR_NAME = 22;

		public const int SPDRP_SECURITY = 23;

		public const int SPDRP_SECURITY_SDS = 24;

		public const int SPDRP_DEVTYPE = 25;

		public const int SPDRP_EXCLUSIVE = 26;

		public const int SPDRP_CHARACTERISTICS = 27;

		public const int SPDRP_ADDRESS = 28;

		public const int SPDRP_UI_NUMBER_DESC_FORMAT = 29;

		public const int SPDRP_DEVICE_POWER_DATA = 30;

		public const int SPDRP_REMOVAL_POLICY = 31;

		public const int SPDRP_REMOVAL_POLICY_HW_DEFAULT = 32;

		public const int SPDRP_REMOVAL_POLICY_OVERRIDE = 33;

		public const int SPDRP_INSTALL_STATE = 34;

		public const int SPDRP_LOCATION_PATHS = 35;

		public const int SPDRP_MAXIMUM_PROPERTY = 36;

		public const int DICD_GENERATE_ID = 1;

		public static BackgroundWorker bw;

		private FileStream fs;

		private StreamWriter sw;

		public InstallMainClass()
		{
			this.fs = new FileStream("LogSetup.txt", FileMode.Create, FileAccess.Write);
			this.sw = new StreamWriter(this.fs);
		}

		private bool FindExistingDevice(string HardwareID)
		{
			this.print(">>正在准备枚举设备...........", 4);
			InstallMainClass.SP_DEVINFO_DATA sPDEVINFODATum = new InstallMainClass.SP_DEVINFO_DATA();
			InstallMainClass._GUID __GUID = new InstallMainClass._GUID();
			IntPtr intPtr = InstallMainClass.SetupDiGetClassDevs(ref __GUID, 0, (IntPtr)0, 6);
			if ((int)intPtr == -1)
			{
				this.print("错误:无法得到设备的DeviceInfoSet", 3);
			}
			bool flag = false;
			sPDEVINFODATum.cbSize = 28;
			for (int i = 0; InstallMainClass.SetupDiEnumDeviceInfo(intPtr, i, ref sPDEVINFODATum); i++)
			{
				int num = 0;
				byte[] numArray = null;
				int num1 = 0;
				while (!InstallMainClass.SetupDiGetDeviceRegistryProperty(intPtr, ref sPDEVINFODATum, 1, ref num, numArray, num1, ref num1) && InstallMainClass.GetLastError() != 13)
				{
					if (InstallMainClass.GetLastError() != 122)
					{
						this.print("错误:无法得到设备的RegistryProperty,正在销毁DeviceInfo.......", 3);
						InstallMainClass.SetupDiDestroyDeviceInfoList(intPtr);
						this.print("销毁DeviceInfo成功,安装中止", 3);
					}
					else
					{
						numArray = new byte[num1];
					}
				}
				if (InstallMainClass.GetLastError() != 13)
				{
					string str = Encoding.Default.GetString(numArray, 0, num1 - 2);
					this.print(str);
					if (str != HardwareID)
					{
						flag = false;
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (InstallMainClass.GetLastError() == 0)
			{
				return flag;
			}
			else
			{
				return flag;
			}
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		public static extern uint GetLastError();

		public bool install(string InfFileName, string HardwareID, int RebootRequired, string Port, string AT, string MaximumPortSpeed)
		{
			if (File.Exists(InfFileName))
			{
				if (!this.FindExistingDevice(HardwareID))
				{
					this.print(">>枚举设备完毕!", 4);
					this.print(string.Format(">>未找到硬件ID:{0},正准备注册该ID.......", HardwareID));
					if (!this.InstallRootEnumeratedDriver(InfFileName, HardwareID, RebootRequired, Port, AT, MaximumPortSpeed))
					{
						return false;
					}
				}
				else
				{
					this.print(">>枚举设备完毕!", 4);
					this.print(string.Format(">>找到硬件ID:{0},设备已安装,正准备更新设备.......", HardwareID));
					if (InstallMainClass.UpdateDriverForPlugAndPlayDevices((IntPtr)0, HardwareID, InfFileName, 1, ref RebootRequired))
					{
						this.print(">>更新设备成功!", 2);
					}
					else
					{
						this.print(">>更新设备失败!", 3);
						return false;
					}
				}
				return true;
			}
			else
			{
				InstallMainClass.bw.ReportProgress(3, "发生错误：安装信息文件丢失！！！");
				return false;
			}
		}

		private bool InstallRootEnumeratedDriver(string InfFile, string HardwareID, int RebootRequired, string Port, string AT, string MaximumPortSpeed)
		{
			InstallMainClass.SP_DEVINFO_DATA sPDEVINFODATum = new InstallMainClass.SP_DEVINFO_DATA();
			InstallMainClass._GUID __GUID = new InstallMainClass._GUID();
			byte[] numArray = new byte[32];
			int num = 0;
			string str = "";
			if (InstallMainClass.SetupDiGetINFClass(InfFile, ref __GUID, numArray, (int)numArray.Length, ref num))
			{
				for (int i = 0; i < num - 1; i++)
				{
					char chr = (char)numArray[i];
					str = string.Concat(str, chr.ToString());
				}
				this.print(">>正在创建设备信息列表");
				IntPtr intPtr = InstallMainClass.SetupDiCreateDeviceInfoList(ref __GUID, (IntPtr)0);
				sPDEVINFODATum.cbSize = 28;
				if (InstallMainClass.SetupDiCreateDeviceInfo(intPtr, numArray, ref __GUID, "null", (IntPtr)0, 1, ref sPDEVINFODATum))
				{
					this.print(">>创建设备信息列表成功!", 2);
					byte[] bytes = Encoding.ASCII.GetBytes(HardwareID);
					this.print(">>SetDeviceRegistryProperty.........");
					if (InstallMainClass.SetupDiSetDeviceRegistryProperty(intPtr, ref sPDEVINFODATum, 1, bytes, HardwareID.Length + 2))
					{
						this.print(">>SetDeviceRegistryProperty成功!");
						if (this.RegisterModem(intPtr, ref sPDEVINFODATum, Port, AT, MaximumPortSpeed))
						{
							this.print(">>正在安装驱动........");
							if (InstallMainClass.UpdateDriverForPlugAndPlayDevices((IntPtr)0, HardwareID, InfFile, 1, ref RebootRequired))
							{
								return true;
							}
							else
							{
								this.print(">>安装失败,正在移除设备", 3);
								if (InstallMainClass.SetupDiCallClassInstaller(5, intPtr, ref sPDEVINFODATum))
								{
									this.print(">>设备移除成功");
									return false;
								}
								else
								{
									return false;
								}
							}
						}
						else
						{
							if (InstallMainClass.SetupDiCallClassInstaller(5, intPtr, ref sPDEVINFODATum))
							{
								return false;
							}
							else
							{
								return false;
							}
						}
					}
					else
					{
						InstallMainClass.SetupDiDestroyDeviceInfoList(intPtr);
						this.print("失败！,已销毁设备信息列表", 3);
						return false;
					}
				}
				else
				{
					this.print(">>创建设备信息列表失败!", 3);
					InstallMainClass.SetupDiDestroyDeviceInfoList(intPtr);
					return false;
				}
			}
			else
			{
				this.print(">>读取inf文件出错!!!", 3);
				return false;
			}
		}

		private void print(string log, int c)
		{
			InstallMainClass.bw.ReportProgress(c, string.Concat(log, "\r\n"));
			DateTime now = DateTime.Now;
			this.sw.Write(string.Concat(now.ToString(), "->> ", log, "\r\n"));
			this.sw.Flush();
		}

		private void print(string log)
		{
			InstallMainClass.bw.ReportProgress(1, string.Concat(log, "\r\n"));
			DateTime now = DateTime.Now;
			this.sw.Write(string.Concat(now.ToString(), "->> ", log, "\r\n"));
			this.sw.Flush();
		}

		[DllImport("advapi32.dll", CharSet=CharSet.None)]
		public static extern uint RegCloseKey(IntPtr hKey);

		private bool RegisterModem(IntPtr hdi, ref InstallMainClass.SP_DEVINFO_DATA pdevData, string Port, string AT, string MaximumPortSpeed)
		{
			this.print(">>正在注册Modem.........!");
			string str = "AttachedTo";
			int num = 0;
			InstallMainClass.SP_DRVINFO_DATA sPDRVINFODATum = new InstallMainClass.SP_DRVINFO_DATA();
			uint num1 = 0;
			InstallMainClass.SP_DEVINFO_DATA sPDEVINFODATum = new InstallMainClass.SP_DEVINFO_DATA();
			this.print(">>注册设备信息.........!");
			if (InstallMainClass.SetupDiRegisterDeviceInfo(hdi, ref pdevData, 0, ref num1, ref num, ref sPDEVINFODATum))
			{
				this.print(">>注册设备信息成功!");
				IntPtr intPtr = InstallMainClass.SetupDiOpenDevRegKey(hdi, ref pdevData, 1, 0, 2, 0xf003f);
				if ((int)intPtr != -1 || InstallMainClass.GetLastError() != 0xe0000204)
				{
					return false;
				}
				else
				{
					intPtr = InstallMainClass.SetupDiCreateDevRegKey(hdi, ref pdevData, 1, 0, 2, (IntPtr)0, null);
					if (-1 != (int)intPtr)
					{
						this.print(string.Concat(">>正在绑定端口到", Port));
						byte[] bytes = Encoding.ASCII.GetBytes(Port);
						if (InstallMainClass.RegSetValueEx(intPtr, str, 0, 1, bytes, (uint)bytes.Length) == 0)
						{
							this.print(">>绑定端口成功");
							this.print(">>正在初始化AT指今....");
							if (AT != "")
							{
								byte[] numArray = Encoding.ASCII.GetBytes(AT);
								if (InstallMainClass.RegSetValueEx(intPtr, "UserInit", 0, 1, numArray, (uint)numArray.Length) != 0)
								{
									this.print(">>初始化AT指今失败", 3);
									return false;
								}
							}
							this.print(">>初始化AT指今成功");
							this.print(">>正在设置最大端口速率....");
							byte[] bytes1 = Encoding.ASCII.GetBytes(MaximumPortSpeed);
							if (InstallMainClass.RegSetValueEx(intPtr, "MaximumPortSpeed", 0, 4, bytes1, (uint)bytes1.Length) == 0)
							{
								this.print(">>设置最大端口速率成功");
								InstallMainClass.RegCloseKey(intPtr);
								this.print(">>注册表相关健已关闭");
								num1 = 0;
								num = 0;
								sPDEVINFODATum = new InstallMainClass.SP_DEVINFO_DATA();
								if (InstallMainClass.SetupDiRegisterDeviceInfo(hdi, ref pdevData, 0, ref num1, ref num, ref sPDEVINFODATum))
								{
									this.print(">>注册Modem成功");
									InstallMainClass.SetupDiGetSelectedDriver(hdi, ref pdevData, ref sPDRVINFODATum);
									return true;
								}
								else
								{
									this.print(">>注册Modem失败", 3);
									return false;
								}
							}
							else
							{
								this.print(">>设置最大端口速率失败", 3);
								return false;
							}
						}
						else
						{
							this.print(">>绑定端口失败", 3);
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			else
			{
				this.print(">>注册设备信息失败!");
				return false;
			}
		}

		[DllImport("advapi32.dll", CharSet=CharSet.None)]
		public static extern uint RegSetValueEx(IntPtr hKey, string lpValueName, uint Reserved, uint dwType, byte[] lpData, uint cbData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiCallClassInstaller(int InstallFunction, IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiCreateDeviceInfo(IntPtr DeviceInfoSet, byte[] DeviceName, ref InstallMainClass._GUID ClassGuid, string DeviceDescription, IntPtr hwndParent, int CreationFlags, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiCreateDeviceInfoList(ref InstallMainClass._GUID ClassGuid, IntPtr HWND);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiCreateDevRegKey(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, uint Scope, uint HwProfile, uint KeyType, IntPtr InfHandle, string InfSectionName);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, int MemberIndex, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiGetClassDevs(ref InstallMainClass._GUID ClassGuid, int Enumerator, IntPtr hwndParent, int Flags);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiGetClassDevsA(ref InstallMainClass._GUID ClassGuid, uint Enumerator, IntPtr hwndParent, uint Flags);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiGetClassDevsA(ref InstallMainClass._GUID ClassGuid, uint samDesired, uint Flags, ref string hwndParent, IntPtr Reserved);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiGetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, int Property, ref int PropertyRegDataType, byte[] PropertyBuffer, int PropertyBufferSize, ref int RequiredSize);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiGetINFClass(string inf_file_name, ref InstallMainClass._GUID _guid, byte[] classname, int size, ref int RequiredSize);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiGetSelectedDriver(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, ref InstallMainClass.SP_DRVINFO_DATA DriverInfoData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiOpenClassRegKeyExA(ref Guid ClassGuid, uint samDesired, int Flags, IntPtr MachineName, uint Reserved);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern IntPtr SetupDiOpenDevRegKey(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, uint Scope, uint HwProfile, uint KeyType, uint samDesired);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiRegisterDeviceInfo(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, int Flags, ref uint CompareProc, ref int CompareContext, ref InstallMainClass.SP_DEVINFO_DATA DupDeviceInfoData);

		[DllImport("setupapi.dll", CharSet=CharSet.None)]
		public static extern bool SetupDiSetDeviceRegistryProperty(IntPtr DeviceInfoSet, ref InstallMainClass.SP_DEVINFO_DATA DeviceInfoData, int Property, byte[] PropertyBuffer, int PropertyBufferSize);

		[DllImport("newdev.dll", CharSet=CharSet.None)]
		public static extern bool UpdateDriverForPlugAndPlayDevices(IntPtr hwndParent, string HardwareId, string FullInfPath, int InstallFlags, ref int bRebootRequired);

		public struct _GUID
		{
			public uint Data1;

			public ushort Data2;

			public ushort Data3;

			public byte[] Data4;
		}

		public struct FILETIME
		{
			private uint dwLowDateTime;

			private uint dwHighDateTime;
		}

		public struct SP_DEVINFO_DATA
		{
			public uint cbSize;

			public InstallMainClass._GUID ClassGuid;

			public uint DevInst;

			public uint Reserved;
		}

		public struct SP_DRVINFO_DATA
		{
			private uint cbSize;

			private uint DriverType;

			private uint Reserved;

			private char[] Description;

			private char[] MfgName;

			private char[] ProviderName;

			private InstallMainClass.FILETIME DriverDate;

			private ulong DriverVersion;
		}
	}
}