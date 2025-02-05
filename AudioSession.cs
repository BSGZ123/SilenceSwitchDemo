using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using SilenceSwitchDemo;

public class AudioSession
{
    public static NativeMethods.IAudioSessionControl2 FindAudioSessionByProcessName(NativeMethods.IAudioSessionManager2 sessionManager, string processName)
    {
        NativeMethods.IAudioSessionEnumerator sessionEnumerator = null;
        try
        {
            NativeMethods.IAudioSessionControl2 foundSession = null;

            var hresult = sessionManager.GetSessionEnumerator(out sessionEnumerator);
            if (hresult == 0 && sessionEnumerator != null)
            {
                int sessionCount;
                hresult = sessionEnumerator.GetCount(out sessionCount);
                if (hresult == 0)
                {
                    for (int i = 0; i < sessionCount; i++)
                    {
                        NativeMethods.IAudioSessionControl2 sessionControl = null;
                        try
                        {
                            hresult = sessionEnumerator.GetSession(i, out sessionControl);
                            if (hresult == 0 && sessionControl != null)
                            {
                                uint processId;
                                hresult = sessionControl.GetProcessId(out processId);
                                if (hresult == 0 && processId != 0)
                                {
                                    IntPtr hProcess = NativeMethods.OpenProcess(0x0400 | 0x1000, false, (int)processId);
                                    if (hProcess != IntPtr.Zero)
                                    {
                                        StringBuilder processPathBuilder = new StringBuilder(2048); // Use StringBuilder to get path
                                        NativeMethods.GetModuleFileNameEx(hProcess, IntPtr.Zero, processPathBuilder, processPathBuilder.Capacity);
                                        NativeMethods.CloseHandle(hProcess);
                                        string path = processPathBuilder.ToString();
                                        if (!string.IsNullOrEmpty(path))
                                        {
                                            string sessionProcessName = System.IO.Path.GetFileNameWithoutExtension(path);
                                            if (sessionProcessName.Equals(processName, StringComparison.OrdinalIgnoreCase))
                                            {
                                                foundSession = sessionControl; //设置foundSession
                                                sessionControl = null; // 避免在 finally 块中释放它
                                                return foundSession;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            if (sessionControl != null)
                            {
                                Marshal.ReleaseComObject(sessionControl);
                            }
                        }
                    }
                }
            }

            return null;
        }
        finally
        {
            if (sessionEnumerator != null)
            {
                Marshal.ReleaseComObject(sessionEnumerator);
            }
        }
    }

    public static void MuteSession(NativeMethods.IAudioSessionControl2 sessionControl)
    {
        NativeMethods.ISimpleAudioVolume simpleAudioVolume = null;
        try
        {
            IntPtr simpleAudioVolumePtr;
            var guid_ISimpleAudioVolume = typeof(NativeMethods.ISimpleAudioVolume).GUID;
            var hresult = Marshal.QueryInterface(Marshal.GetIUnknownForObject(sessionControl), ref guid_ISimpleAudioVolume, out simpleAudioVolumePtr);
            if (hresult == 0 && simpleAudioVolumePtr != IntPtr.Zero)
            {
                simpleAudioVolume = (NativeMethods.ISimpleAudioVolume)Marshal.GetObjectForIUnknown(simpleAudioVolumePtr);
                bool isMuted;
                simpleAudioVolume.GetMute(out isMuted);
                if (!isMuted)
                {
                    simpleAudioVolume.SetMute(true, Guid.Empty);
                }
            }
        }
        finally
        {
            if (simpleAudioVolume != null)
            {
                Marshal.ReleaseComObject(simpleAudioVolume);
            }
        }
    }

    public static void UnmuteSession(NativeMethods.IAudioSessionControl2 sessionControl)
    {
        NativeMethods.ISimpleAudioVolume simpleAudioVolume = null;
        try
        {
            IntPtr simpleAudioVolumePtr;
            var guid_ISimpleAudioVolume = typeof(NativeMethods.ISimpleAudioVolume).GUID;
            var hresult = Marshal.QueryInterface(Marshal.GetIUnknownForObject(sessionControl), ref guid_ISimpleAudioVolume, out simpleAudioVolumePtr);
            if (hresult == 0 && simpleAudioVolumePtr != IntPtr.Zero)
            {
                simpleAudioVolume = (NativeMethods.ISimpleAudioVolume)Marshal.GetObjectForIUnknown(simpleAudioVolumePtr);
                bool isMuted;
                simpleAudioVolume.GetMute(out isMuted);
                if (isMuted)
                {
                    simpleAudioVolume.SetMute(false, Guid.Empty);
                }
            }
        }
        finally
        {
            if (simpleAudioVolume != null)
            {
                Marshal.ReleaseComObject(simpleAudioVolume);
            }
        }
    }
}
