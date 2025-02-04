using System;
using System.Runtime.InteropServices;

namespace SilenceSwitchDemo
{
    public static class NativeMethods
    {
        // IMMDevice API
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMMDevice
        {
            //激活设备上的一个接口,并返回指向新接口的指针。
            [PreserveSig]
            int Activate(
                [In] Guid riid,
                [In] CLSCTX dwClsCtx,
                [In, Optional] nint pActivationParams,
                [Out] out nint ppInterface);

            //打开设备的属性存储（用于读取或设置设备属性）。
            [PreserveSig]
            int OpenPropertyStore([In] int stgmAccess, [Out] out nint ppProperties);

            [PreserveSig]
            int GetId([Out, MarshalAs(UnmanagedType.LPWStr)] out string ppstrId);

            [PreserveSig]
            int GetState([Out] out int pdwState);
        }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMMDeviceEnumerator
        {
            //枚举音频端点（例如，扬声器、麦克风）。
            [PreserveSig]
            int EnumAudioEndpoints(
                [In] EDataFlow dataFlow,
                [In] DeviceState dwStateMask,
                [Out] out nint ppDevices);

            //获取默认的音频端点。
            [PreserveSig]
            int GetDefaultAudioEndpoint(
                [In] EDataFlow dataFlow,
                [In] ERole role,
                [Out, MarshalAs(UnmanagedType.Interface)] out IMMDevice ppDevice);
        }
        [Guid("886D8EEB-8CF2-4446-8D02-CDBA1D4DCF3E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IPropertyStore
        {
            //获取属性的数量。
            [PreserveSig]
            int GetCount([Out] out int cProps);

            //通过索引获取属性的键。
            [PreserveSig]
            int GetAt([In] int iProp, [Out] out PropertyKey pkey);

            //根据属性键获取属性值。
            [PreserveSig]
            int GetValue([In] ref PropertyKey key, [Out] PropVariant pv);

            //根据属性键设置属性值。
            [PreserveSig]
            int SetValue([In] ref PropertyKey key, [In] PropVariant pv);

            //保存对属性存储的更改。
            [PreserveSig]
            int Commit();
        }

        [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioSessionManager2
        {
            //获取音频会话枚举器。
            [PreserveSig]
            int GetSessionEnumerator([Out, MarshalAs(UnmanagedType.Interface)] out IAudioSessionEnumerator SessionEnum);

            //注册音频会话的通知。
            [PreserveSig]
            int RegisterSessionNotification([In, MarshalAs(UnmanagedType.Interface)] IAudioSessionNotification SessionNotification);

            //取消音频会话通知。
            [PreserveSig]
            int UnregisterSessionNotification([In, MarshalAs(UnmanagedType.Interface)] IAudioSessionNotification SessionNotification);

            //注册音频衰减通知。
            [PreserveSig]
            int RegisterDuckNotification([MarshalAs(UnmanagedType.LPWStr)] string SessionId,
                                          [In, MarshalAs(UnmanagedType.Interface)] IAudioDuckNotification DuckNotification);

            //取消音频衰减通知。
            [PreserveSig]
            int UnregisterDuckNotification([In, MarshalAs(UnmanagedType.Interface)] IAudioDuckNotification DuckNotification);

            //获取指定会话 Id 的音频会话控制接口。
            [PreserveSig]
            int GetSession(
               [In, MarshalAs(UnmanagedType.LPWStr)] string SessionId,
               [Out, MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl2 SessionControl);

            //获取当前进程音频会话控制接口。
            [PreserveSig]
            int GetCurrentProcessSession(
              [Out, MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl2 SessionControl);
        }

        [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioSessionEnumerator
        {
            //获取音频会话的数量。
            [PreserveSig]
            int GetCount([Out] out int count);

            //获取指定索引处的音频会话接口。
            [PreserveSig]
            int GetSession([In] int index, [Out, MarshalAs(UnmanagedType.Interface)] out IAudioSessionControl2 session);
        }

        [Guid("bfb7ffda-7e81-4b07-bc1d-68a0a2a1286a"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioSessionControl2
        {
            //获取音频会话的状态（例如，活动、非活动）。
            [PreserveSig]
            int GetState([Out] out AudioSessionState retVal);

            //获取音频会话的显示名称。
            [PreserveSig]
            int GetDisplayName([Out, MarshalAs(UnmanagedType.LPWStr)] out string retVal);

            //设置音频会话的显示名称。
            [PreserveSig]
            int SetDisplayName([In, MarshalAs(UnmanagedType.LPWStr)] string value, [In] Guid eventContext);

            //获取音频会话的图标路径。
            [PreserveSig]
            int GetIconPath([Out, MarshalAs(UnmanagedType.LPWStr)] out string retVal);

            //设置音频会话的图标路径。
            [PreserveSig]
            int SetIconPath([In, MarshalAs(UnmanagedType.LPWStr)] string value, [In] Guid eventContext);

            //获取会话的分组参数。
            [PreserveSig]
            int GetGroupingParam([Out] out Guid retVal);

            //设置会话的分组参数。
            [PreserveSig]
            int SetGroupingParam([In] Guid value, [In] Guid eventContext);

            //注册音频会话通知
            [PreserveSig]
            int RegisterAudioSessionNotification([In, MarshalAs(UnmanagedType.Interface)] IAudioSessionNotification client);

            //取消音频会话通知
            [PreserveSig]
            int UnregisterAudioSessionNotification([In, MarshalAs(UnmanagedType.Interface)] IAudioSessionNotification client);

            //获取拥有此音频会话的进程 ID。
            [PreserveSig]
            int GetProcessId([Out] out uint retVal);

            //检查是否是系统声音会话。
            [PreserveSig]
            int IsSystemSoundsSession();

            //设置是否对会话启用音频衰减。
            [PreserveSig]
            int SetDuckingPreference([In] bool optOut);

            //获取是否对会话启用了音频衰减。
            [PreserveSig]
            int IsDuckingEnabled();

        }
        [Guid("24918ACC-64B3-37C1-8CA9-74A66E9957FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface ISimpleAudioVolume
        {
            //设置会话的音量
            [PreserveSig]
            int SetMasterVolume([In] float fLevel, [In] Guid EventContext);

            //获取会话的音量
            [PreserveSig]
            int GetMasterVolume([Out] out float pfLevel);

            //静音/取消静音会话。
            [PreserveSig]
            int SetMute([In] bool bMute, [In] Guid EventContext);

            //获取会话静音状态。
            [PreserveSig]
            int GetMute([Out, MarshalAs(UnmanagedType.Bool)] out bool pbMute);
        }

        [Guid("CB7E2C8B-200A-414E-B249-7784187D2D41"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioSessionNotification
        {
            //当新的音频会话创建时触发。
            [PreserveSig]
            int OnSessionCreated([In, MarshalAs(UnmanagedType.Interface)] IAudioSessionControl2 NewSession);

            //当音频会话断开连接时触发。
            [PreserveSig]
            int OnSessionDisconnected([In] AudioSessionDisconnectReason DisconnectReason);

            //当音频会话状态改变时触发。
            [PreserveSig]
            int OnStateChanged([In] AudioSessionState NewState);

            //当音频会话的显示名称改变时触发。
            [PreserveSig]
            int OnDisplayNameChanged([In, MarshalAs(UnmanagedType.LPWStr)] string NewDisplayName, [In] Guid EventContext);

            //当音频会话的图标路径改变时触发。
            [PreserveSig]
            int OnIconPathChanged([In, MarshalAs(UnmanagedType.LPWStr)] string NewIconPath, [In] Guid EventContext);

            //当音频会话的分组参数改变时触发。
            [PreserveSig]
            int OnGroupingParamChanged([In] Guid NewGroupingParam, [In] Guid EventContext);
        }


        [Guid("568B9108-4498-4D69-B919-68A70C0D9232"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IAudioDuckNotification
        {
            //当有音频需要衰减时触发。
            [PreserveSig]
            int OnDuck([MarshalAs(UnmanagedType.LPWStr)] string SessionId);

            //当音频衰减解除时触发。
            [PreserveSig]
            int OnUndoDuck([MarshalAs(UnmanagedType.LPWStr)] string SessionId);
        }

        //与IMMDeviceEnumerator接口的 GUID 相同，表示此类实现了这个接口，ComImport表示此类是一个 COM 对象且禁用默认的类接口。
        [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), ClassInterface(ClassInterfaceType.None)]
        public class MMDeviceEnumerator
        {
        }

        //COM 对象创建的核心函数
        [DllImport("ole32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.IUnknown)]//指定返回类型为 IUnknown，以便在 C# 中使用？
        public static extern object CoCreateInstance(
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,//要创建的 COM 对象的 CLSID。
            [In, MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
            [In] CLSCTX dwClsCtx,//COM 对象的运行上下文，例如，进程内或进程外。
            [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern nint GetModuleHandle(string lpModuleName);//要获取的模块名称

        [DllImport("psapi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]//进程句柄 模块句柄 用于存储文件路径的缓冲区  缓冲区大小
        public static extern bool GetModuleFileNameEx(nint hProcess, nint hModule, [Out] System.Text.StringBuilder lpFilename, [In] int nSize);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(nint hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern nint OpenProcess(int dwDesiredAccess,
                                                    [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
                                                    int dwProcessId);

        //COM 对象的运行上下文
        [Flags]
        public enum CLSCTX : int
        {
            CLSCTX_INPROC_SERVER = 0x1,
            CLSCTX_INPROC_HANDLER = 0x2,
            CLSCTX_LOCAL_SERVER = 0x4,
            CLSCTX_INPROC_SERVER16 = 0x8,
            CLSCTX_REMOTE_SERVER = 0x10,
            CLSCTX_INPROC_HANDLER16 = 0x20,
            CLSCTX_RESERVED1 = 0x40,
            CLSCTX_RESERVED2 = 0x80,
            CLSCTX_RESERVED3 = 0x100,
            CLSCTX_RESERVED4 = 0x200,
            CLSCTX_NO_CODE_DOWNLOAD = 0x400,
            CLSCTX_RESERVED5 = 0x800,
            CLSCTX_NO_CUSTOM_MARSHAL = 0x1000,
            CLSCTX_ENABLE_CODE_DOWNLOAD = 0x2000,
            CLSCTX_DEFAULT = 0x1,
            CLSCTX_ALL = CLSCTX_INPROC_SERVER | CLSCTX_INPROC_HANDLER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER,
            CLSCTX_SERVER = CLSCTX_INPROC_SERVER | CLSCTX_LOCAL_SERVER | CLSCTX_REMOTE_SERVER
        }

        //音频会话的状态
        public enum AudioSessionState
        {
            AudioSessionStateInactive = 0,
            AudioSessionStateActive = 1,
            AudioSessionStateExpired = 2
        }

        //音频设备的状态
        public enum DeviceState
        {
            DEVICE_STATE_ACTIVE = 0x00000001,
            DEVICE_STATE_DISABLED = 0x00000002,
            DEVICE_STATE_NOTPRESENT = 0x00000004,
            DEVICE_STATE_UNPLUGGED = 0x00000008,
            DEVICE_STATEMASK_ALL = 0x0000000F
        }

        //音频会话断开连接的原因
        public enum AudioSessionDisconnectReason
        {
            DisconnectReason_Disconnect = 0,
            DisconnectReason_SessionLogoff = 1,
            DisconnectReason_SessionDisconnected = 2,
            DisconnectReason_ProcessExiting = 3,
            DisconnectReason_SessionRundown = 4
        }

        //数据流方向，例如渲染或捕获。
        public enum EDataFlow
        {
            eRender,
            eCapture,
            eAll
        }

        //音频端点的角色，例如控制台，多媒体或通信。
        public enum ERole
        {
            eConsole,
            eMultimedia,
            eCommunications
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PropertyKey
        {
            public Guid fmtid;
            public int pid;
        }

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        public struct PropVariant
        {
            [FieldOffset(0)]
            public ushort vt;
            [FieldOffset(8)]
            public nint ptr; //union with long value

            public PropVariant(int value)
            {
                vt = (ushort)VarEnum.VT_I4;
                ptr = value;
            }
            public PropVariant(string value)
            {
                vt = (ushort)VarEnum.VT_LPWSTR;
                ptr = Marshal.StringToCoTaskMemAuto(value);
            }

            public object? GetValue()
            {
                switch ((VarEnum)vt)
                {
                    case VarEnum.VT_I4:
                        return ptr.ToInt32();
                    case VarEnum.VT_LPWSTR:
                        return Marshal.PtrToStringAuto(ptr);
                    default:
                        return null;
                }
            }

            public void Clear()
            {
                if (vt == (ushort)VarEnum.VT_LPWSTR && ptr != nint.Zero)
                    Marshal.FreeCoTaskMem(ptr);

                vt = (ushort)VarEnum.VT_EMPTY;
            }
        }

        public enum VarEnum
        {
            VT_EMPTY = 0,
            VT_NULL = 1,
            VT_I2 = 2,
            VT_I4 = 3,
            VT_R4 = 4,
            VT_R8 = 5,
            VT_CY = 6,
            VT_DATE = 7,
            VT_BSTR = 8,
            VT_DISPATCH = 9,
            VT_ERROR = 10,
            VT_BOOL = 11,
            VT_VARIANT = 12,
            VT_UNKNOWN = 13,
            VT_DECIMAL = 14,
            VT_I1 = 16,
            VT_UI1 = 17,
            VT_UI2 = 18,
            VT_UI4 = 19,
            VT_I8 = 20,
            VT_UI8 = 21,
            VT_INT = 22,
            VT_UINT = 23,
            VT_VOID = 24,
            VT_HRESULT = 25,
            VT_PTR = 26,
            VT_SAFEARRAY = 27,
            VT_CARRAY = 28,
            VT_USERDEFINED = 29,
            VT_LPSTR = 30,
            VT_LPWSTR = 31,
            VT_RECORD = 36,
            VT_INT_PTR = 37,
            VT_UINT_PTR = 38,
            VT_FILETIME = 64,
            VT_BLOB = 65,
            VT_STREAM = 66,
            VT_STORAGE = 67,
            VT_STREAMED_OBJECT = 68,
            VT_STORED_OBJECT = 69,
            VT_BLOB_OBJECT = 70,
            VT_CF = 71,
            VT_CLSID = 72,
            VT_VERSIONED_STREAM = 73,
            VT_BSTR_BLOB = 0xfff,
            VT_VECTOR = 0x1000,
            VT_ARRAY = 0x2000,
            VT_BYREF = 0x4000,
            VT_RESERVED = 0x8000,
            VT_ILLEGAL = 0xffff
        }

    }
}