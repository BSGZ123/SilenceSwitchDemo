using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SilenceSwitchDemo
{
    public class AudioDevice
    {
        /*
        public static NativeMethods.IMMDevice GetDefaultAudioDevice()
        {
            NativeMethods.IMMDeviceEnumerator enumerator = null;
            try
            {
                enumerator = new NativeMethods.IMMDeviceEnumerator();
                NativeMethods.IMMDevice defaultDevice;
                var hresult = enumerator.GetDefaultAudioEndpoint(NativeMethods.EDataFlow.eRender, NativeMethods.ERole.eConsole, out defaultDevice);

                if (hresult == 0 && defaultDevice != null)
                {
                    return defaultDevice;
                }
                else
                {
                    Marshal.ThrowExceptionForHR(hresult);
                    return null;
                }
            }
            finally
            {
                if (enumerator != null)
                {
                    Marshal.ReleaseComObject(enumerator);
                }
            }
        }*/

        public static NativeMethods.IAudioSessionManager2 GetAudioSessionManager(NativeMethods.IMMDevice device)
        {
            IntPtr sessionManagerPtr;
            var guid_IAudioSessionManager2 = typeof(NativeMethods.IAudioSessionManager2).GUID;
            var hresult = device.Activate(guid_IAudioSessionManager2, NativeMethods.CLSCTX.CLSCTX_ALL, IntPtr.Zero, out sessionManagerPtr);

            if (hresult == 0 && sessionManagerPtr != IntPtr.Zero)
            {
                return (NativeMethods.IAudioSessionManager2)Marshal.GetObjectForIUnknown(sessionManagerPtr);
            }
            else
            {
                Marshal.ThrowExceptionForHR(hresult);
                return null;
            }
        }
    }
}
