using SilenceSwitchDemo;
using System;
using System.Runtime.InteropServices;

public class AudioController
{
    public static void MuteApplication(string processName)
    {
        NativeMethods.IMMDevice device = null;
        NativeMethods.IAudioSessionManager2 sessionManager = null;
        NativeMethods.IAudioSessionControl2 sessionControl = null;
        try
        {
            device = AudioDevice.GetDefaultAudioDevice();
            if (device == null) return;

            sessionManager = AudioDevice.GetAudioSessionManager(device);
            if (sessionManager == null) return;

            sessionControl = AudioSession.FindAudioSessionByProcessName(sessionManager, processName);
            if (sessionControl == null)
            {
                Console.WriteLine($"未找到名为 {processName} 的应用.");
                return;
            }

            AudioSession.MuteSession(sessionControl);
            Console.WriteLine($"已禁用 {processName} 的音频.");
        }
        finally
        {
            if (sessionControl != null) Marshal.ReleaseComObject(sessionControl);
            if (sessionManager != null) Marshal.ReleaseComObject(sessionManager);
            if (device != null) Marshal.ReleaseComObject(device);
        }
    }

    public static void UnmuteApplication(string processName)
    {
        NativeMethods.IMMDevice device = null;
        NativeMethods.IAudioSessionManager2 sessionManager = null;
        NativeMethods.IAudioSessionControl2 sessionControl = null;

        try
        {
            device = AudioDevice.GetDefaultAudioDevice();
            if (device == null) return;

            sessionManager = AudioDevice.GetAudioSessionManager(device);
            if (sessionManager == null) return;

            sessionControl = AudioSession.FindAudioSessionByProcessName(sessionManager, processName);

            if (sessionControl == null)
            {
                Console.WriteLine($"未找到名为 {processName} 的应用.");
                return;
            }

            AudioSession.UnmuteSession(sessionControl);
            Console.WriteLine($"已启用 {processName} 的音频.");
        }
        finally
        {
            if (sessionControl != null) Marshal.ReleaseComObject(sessionControl);
            if (sessionManager != null) Marshal.ReleaseComObject(sessionManager);
            if (device != null) Marshal.ReleaseComObject(device);
        }
    }
}
