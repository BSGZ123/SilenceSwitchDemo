﻿using CoreAudio;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

/*
 * 经过三天的查阅资料，甚至借助了AI，发现以我的积累想原生实现第三方Windows音频管理是不切实际的。
 * 尤其是繁多的P/Invoke 定义，需要大量时间查阅书籍资料以及调试，遂选择放弃。
 * 所以选中了CoreAudio库，它已经帮助我们定义实现了各类P/Invoke，非托管导入函数及常见音频管理办法。
 * CoreAudio库没有使用文档，但给出了使用示例。
 */

namespace SilenceSwitchDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //枚举音频设备 所有音频端点及默认音频端点 
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator(Guid.NewGuid());
            //获取默认的多媒体音频渲染设备 (播放设备)  肯定播放设备  设备角色为多媒体
            MMDevice device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            //现在就是遍历当前音频设备上所有的音频会话
            foreach (var session in device.AudioSessionManager2.Sessions)
            {
                //检测当前音频会话是否处于活跃状态
                if (session.State == AudioSessionState.AudioSessionStateActive)
                {
                    Console.CursorVisible = false;
                    PrintSessionInfo(session);//打印当前活动音频会话参数

                    //获取当前当前音频会话的音频计量信息
                    AudioMeterInformation mi = session.AudioMeterInformation;
                    //获取当前音频会话的简单音量控制接口，用于控制静音和音量。
                    SimpleAudioVolume volume = session.SimpleAudioVolume;

                    (int cw, int ch) = (Console.WindowWidth, Console.WindowHeight);
                    int start = Console.CursorTop;

                    //不断更新 VU 表和处理用户输入
                    while (true)
                    {
                        if (cw != Console.WindowWidth || ch != Console.WindowHeight)
                        {
                            Console.Clear();
                            PrintSessionInfo(session);

                            start = Console.CursorTop;
                            Console.SetCursorPosition(0, start);
                            cw = Console.WindowWidth;
                            ch = Console.WindowHeight;
                        }

                        int w = Console.WindowWidth - 1;
                        int len = (int)(mi.MasterPeakValue * w);

                        Console.SetCursorPosition(0, start);
                        Console.ForegroundColor = ConsoleColor.Green;
                        //显示音量
                        for (int j = 0; j < len; j++) Console.Write("█");
                        for (int j = 0; j < w - len + 1; j++) Console.Write(" ");

                        Console.SetCursorPosition(0, start + 1);
                        WriteLine("Mute   ", $"{volume.Mute,6}");
                        WriteLine("Master ", $"{volume.MasterVolume * 100,6:N2}");

                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo key = Console.ReadKey(true);
                            switch (key.Key)
                            {
                                //切换静音状态
                                case ConsoleKey.M:
                                    volume.Mute = !volume.Mute;
                                    break;
                                case ConsoleKey.Escape:
                                case ConsoleKey.Q:
                                    ResetConsole();
                                    return;
                                case ConsoleKey.DownArrow:
                                    float curvol = volume.MasterVolume - 0.1f;
                                    if (curvol < 0) curvol = 0;
                                    volume.MasterVolume = curvol;
                                    break;
                                case ConsoleKey.UpArrow:
                                    float curvold = volume.MasterVolume + 0.1f;
                                    if (curvold > 1) curvold = 1;
                                    volume.MasterVolume = curvold;
                                    break;
                            }

                        }
                    }

                }
            }
        }

        private static void PrintSessionInfo(AudioSessionControl2 session)
        {
            WriteLine("DisplayName", session.DisplayName);
            WriteLine("State", session.State.ToString());
            WriteLine("IconPath", session.IconPath);
            WriteLine("SessionIdentifier", session.SessionIdentifier);
            WriteLine("SessionInstanceIdentifier", session.SessionInstanceIdentifier);
            WriteLine("ProcessID", session.ProcessID.ToString());
            WriteLine("IsSystemIsSystemSoundsSession", session.IsSystemSoundsSession.ToString());

            //获取音频会话对应的进程，并打印进程名称和主窗口标题
            Process p = Process.GetProcessById((int)session.ProcessID);
            WriteLine("ProcessName", p.ProcessName);
            WriteLine("MainWindowTitle", p.MainWindowTitle);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n---[Hotkeys]---");
            WriteLine("M", "Toggle Mute");
            WriteLine("↑", "Lower volume");
            WriteLine("↓", "Raise volume");
            WriteLine("Q", "Quit\n");
        }

        static void WriteLine(string key, string value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{key}: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"{value}");
        }

        //重置控制台的颜色和光标可见性，以便程序退出后恢复控制台的默认状态。
        static void ResetConsole()
        {
            Console.CursorVisible = true;
            Console.ResetColor();
        }

    }

}
