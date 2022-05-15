/********************************/
/*    © Maxim Trusakov, 2022    */
/********************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace TLog
{
    public class Writer
    {
        private static readonly Object _lock = new Object();
        private static Writer instance = null;
        private string path = "";
        private string logfile = "";
        public bool Debug = true;
        private bool isInit = false;

        private List<string> _buf = new List<string>();

        // Time Interval Property
        const double _interval = 10 * 1000; // 10 seconds
        private Timer checkForTime = new Timer(_interval);


        private Writer()
        {

        }

        public static Writer GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                            instance = new Writer();
                        if (!instance.isInit)
                            instance.Init();
                    }
                }
                return instance;
            }
        }

        private void Init()
        {
            try
            {
                path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logs");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                logfile = Path.Combine(path, DateTime.Now.ToString("dd-MM-yy HHmmss") + " log.txt");
                File.WriteAllText(logfile, "Log Create " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\r\n", System.Text.Encoding.UTF8);
                // Init & Start Timer
                checkForTime.Elapsed += new ElapsedEventHandler(GetInstance.SaveLogToDisk);
                checkForTime.Enabled = true;
            }
            catch (Exception ex) { }
            finally
            {
                isInit = true;
            }
        }

        public void WriteToBuffer(string mes)
        {
            lock (_lock)
            {
                _buf.Add(mes);
            }
        }

        public void Write(string mes)
        {
            lock (_lock)
            {
                _buf.Add(mes);
                WriteToDisk();
            }
        }

        private void WriteToDisk()
        {
            if (!isInit) return;
            if (_buf.Count > 0)
                try
                {
                    File.AppendAllLines(logfile, _buf.ToArray(), System.Text.Encoding.UTF8);
                    _buf.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
        }

        public void SaveLogToDisk(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                WriteToDisk();
            }
        }

        public string[] GetLog()
        {
            lock (_lock)
            {
                if (File.Exists(logfile))
                    return File.ReadAllLines(logfile);
                else
                    return null;
            }
        }
    }
}
