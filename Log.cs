/********************************/
/*    © Maxim Trusakov, 2022    */
/********************************/

using System;
using System.Threading;

namespace TLog
{
    public class Log : IDisposable
    {
        private string _obj_name = "";
        public string ObjectName { get { return _obj_name; } set { _obj_name = value; } }

        public Log(string objectName)
        {
            _obj_name = objectName.Split(',')[0];
        }

        public void Info(string mes)
        {
            string mesout = $"{PrintDateTime()} TId={Thread.CurrentThread.ManagedThreadId} [Info] {_obj_name} : {mes}";
            Writer.GetInstance.WriteToBuffer(mesout);
            Console.WriteLine(mesout);
        }

        public void Warn(string mes)
        {
            string mesout = $"{PrintDateTime()} TId={Thread.CurrentThread.ManagedThreadId} [Warning] {_obj_name} : {mes}";
            Writer.GetInstance.WriteToBuffer(mesout);
            Console.WriteLine(mesout);
        }

        public void Debug(string mes)
        {
            if (!Writer.GetInstance.Debug) return;
            string mesout = $"{PrintDateTime()} TId={Thread.CurrentThread.ManagedThreadId} [Debug] {_obj_name} : {mes}";
            Writer.GetInstance.Write(mesout);
            Console.WriteLine(mesout);
        }

        public void Error(string mes)
        {
            string mesout = $"{PrintDateTime()} TId={Thread.CurrentThread.ManagedThreadId} [ERROR] {_obj_name} : {mes}";
            Writer.GetInstance.Write(mesout);
            Console.WriteLine(mesout);
        }

        private string PrintDateTime()
        {
            return DateTime.Now.ToString("dd/MM/yy HH:mm:ss");
        }

        public void SetDebug(bool flg)
        {
            Writer.GetInstance.Debug = flg;
        }

        public void Dispose()
        {
            string mesout = $"{PrintDateTime()} Log Finish";
            Writer.GetInstance.Write(mesout);
        }
    }
}
