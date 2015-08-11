using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BatRunner
{
    public class ExcuteCmd
    {
        public delegate void StartExcute();
        public event StartExcute OnStartEvent;
        public delegate void MessageExcute(string retStr);
        public event MessageExcute OnMessageEvent;
        public delegate void EndExcute();
        public event EndExcute OnEndEvent;
        public delegate void OutTimeExcute();
        public event OutTimeExcute OnOutTimeEvent;

        private void CallStartEvent()
        {
            if (OnStartEvent != null)
            {
                OnStartEvent();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void CallMessageEvent(string retStr)
        {
            if (OnMessageEvent != null) OnMessageEvent(retStr);
        }

        private void CallEndEvent()
        {
            if (OnEndEvent != null)
            {
                System.Threading.Thread.Sleep(100);
                OnEndEvent();
            }
        }

        private void CallOutTimeEvent()
        {
            if (OnOutTimeEvent != null) OnOutTimeEvent();
        }

        private struct ExcuteParam
        {
            public string title;
            public string command;
            public int seconds;
        }

        private ExcuteParam excuteParam;

        private void Execute()
        {
            CallStartEvent();
            string title = excuteParam.title;
            string command = excuteParam.command;
            int seconds = excuteParam.seconds;

            CallMessageEvent("");
            CallMessageEvent("======================================================");
            CallMessageEvent(DateTime.Now.ToString());
            CallMessageEvent("BeginCommand：" + title);
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = command;//设定需要执行的命令  
                startInfo.Arguments = "/C ";//+ command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                //                            process.OutputDataReceived += process_OutputDataReceived;
                process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

                try
                {
                    if (process.Start())//开始进程  
                    {
                        process.BeginOutputReadLine();
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束  
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        //CallMessageEvent(process.StandardOutput.ReadLine());//读取进程的输出  
                    }
                }
                catch (Exception ex)
                {
                    CallMessageEvent(ex.Message);
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
                CallMessageEvent(DateTime.Now.ToString());
                CallMessageEvent("EndCommand：" + title);
                CallEndEvent();
            }
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            CallMessageEvent(e.Data);
        }

        public void Execute(string title, string command)
        {
            Execute(title, command, 100000);
        }
        public void Execute(string title, string command, int seconds)
        {
            excuteParam.title = title;
            excuteParam.command = command;
            excuteParam.seconds = seconds;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Execute));
            thread.Start();
        }
    }
}
