using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TrackWebsite
{
    internal sealed class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Process pr = RI();
            if (pr != null)
            {
                pid = pr.Id;
                CallBackPtr callBackPtr = new CallBackPtr(EnumReport.Report);
                EnumReport.EnumWindows(callBackPtr, 0);
                if (win.Count != 0)
                {
                    DisplayWindow(win[0]);
                }
                else if (UnvWin.Count != 0)
                {
                    //MessageBox.Show("Пожалуйста, подождите...", "Енот");
                    foreach (IntPtr item in UnvWin)
                    {
                        if (GetWindowText(item).Contains(Process.GetCurrentProcess().ProcessName))
                        {
                            DisplayWindow(item);
                        }
                    }
                }
            }
            else
                Application.Run(new MainForm(args));
        }

        static void DisplayWindow(IntPtr handle)
        {
            ShowWindow(handle, 9);
            SetForegroundWindow(handle);
        }

        public static List<IntPtr> win = new List<IntPtr>();
        public static List<IntPtr> UnvWin = new List<IntPtr>();
        public static int pid;

        public delegate bool CallBackPtr(IntPtr hwnd, int lParam);
        public class EnumReport
        {
            [DllImport("user32.dll")]
            public static extern int EnumWindows(CallBackPtr callPtr, int lPar);

            public static bool Report(IntPtr hwnd, int lParam)
            {
                if (GetProgressByHandle(hwnd).Id == pid)
                {
                    if (IsWindowVisible(hwnd))
                    {
                        win.Add(hwnd);
                    }
                    else
                    {
                        UnvWin.Add(hwnd);
                    }
                }
                return true;
            }
        }
        /// <summary>
        /// Определяет видимость окна. True, если окно видно пользователю.
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);[DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
        /// <summary>
        /// Получает заголовок окна по дескриптору
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
        /// <summary>
        /// Находит процесс приложения по его дескриптору
        /// </summary>
        /// <param name="Handle">дескриптор</param>
        /// <returns></returns>
        public static Process GetProgressByHandle(IntPtr Handle)
        {
            int pid;
            GetWindowThreadProcessId(Handle, out pid);
            Process result = System.Diagnostics.Process.GetProcessById(pid);
            return result;
        }

        public static Process RI()
        {
            Process current = Process.GetCurrentProcess();
            Process[] pr = Process.GetProcessesByName(current.ProcessName);
            foreach (Process i in pr)
            {
                if (i.Id != current.Id && Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                {
                    return i;
                }
            }
            return null;
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public static List<IntPtr> GetChildWindows(IntPtr parent, bool VisibleOnly)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// Callback method to be used when enumerating windows.
        /// </summary>
        /// <param name="handle">Handle of the next window</param>
        /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
        /// <returns>True to continue the enumeration, false to bail</returns>
        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
    }
}
