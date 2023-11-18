using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private static System.Timers.Timer aTimer;
        private static DateTime usingDate = DateTime.Now;
        private static UsingModel usingModels = new UsingModel();
        private static string FormAppPath = "";
        private static string LogPath = "";
        private static bool fromAlreadyOpen = false;
        public Service1()
        {
            InitializeComponent();
            FormAppPath = ConfigurationManager.AppSettings["FormAppPath"];
            LogPath = ConfigurationManager.AppSettings["LogPath"];

            aTimer = new System.Timers.Timer();
            LogRead();


            aTimer.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Start();

        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            aTimer.Stop();
            CheckControl();
            aTimer.Start();
        }

        private void LogRead()
        {
            FirstLoad();

            if (usingModels == null || usingModels.UsingDate == default || usingModels.UsingDate.Date != DateTime.Now.Date)
            {
                NewDayLogWrite();
            }

            if (usingModels.UsingDate.Date == DateTime.Now.Date)
            {
                CheckControl();
            }
            else
            {
                NewDayLogWrite();
            }
        }

        private void FirstLoad()
        {
            string[] line = File.ReadAllLines(LogPath);
            string data = line[line.Length - 1];
            if (!string.IsNullOrWhiteSpace(data))
            {
                usingModels.UsingDate = Convert.ToDateTime(data.Split(',')[0]);
                usingModels.DayUsingMinute = Convert.ToInt32(data.Split(',')[1]);
                usingModels.DayTickCount = Convert.ToInt32(data.Split(',')[2]);

                if (usingModels.UsingDate == DateTime.Now.Date)
                {
                    usingModels.DayUsingMinute += usingModels.DayTickCount;
                    usingModels.DayTickCount = 0;
                    string appendText = string.Format("{0:dd.MM.yyyy}", usingModels.UsingDate) + $",{usingModels.DayUsingMinute},0";
                    line[line.Length - 1] = appendText;
                    line = line.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    File.WriteAllLines(LogPath, line);
                }
            }
        }

        private static void CheckControl()
        {
            int rightUsed = usingModels.DayUsingMinute + usingModels.DayTickCount;
            int minuteOfDailyUse = Convert.ToInt32(TimeSpan.FromHours(MinuteOfDailyUse()).TotalMinutes);
            if (rightUsed >= minuteOfDailyUse)
            {
                ProcessStart();
            }
            else
            {
                fromAlreadyOpen = false;
                ChangeLog();
            }
        }

        private void NewDayLogWrite()
        {
            usingModels = new UsingModel
            {
                UsingDate = DateTime.Now.Date,
                DayUsingMinute = 0,
                DayTickCount = 0,
            };
            string appendText = string.Format("{0:dd.MM.yyyy}", usingModels.UsingDate) + ",0,0";
            File.AppendAllText(LogPath, appendText);
        }

        private static void ChangeLog()
        {
            string[] line = File.ReadAllLines(LogPath);

            usingModels.DayTickCount = Convert.ToInt32(TimeSpan.FromMilliseconds(Environment.TickCount).TotalMinutes);
            string appendText = string.Format("{0:dd.MM.yyyy}", usingModels.UsingDate) + $",{usingModels.DayUsingMinute},{usingModels.DayTickCount}";
            line[line.Length - 1] = appendText;
            line = line.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            File.WriteAllLines(LogPath, line);
        }

        private static void ProcessStart()
        {
            if (!fromAlreadyOpen)
            {
                fromAlreadyOpen = true;
                Process.Start(FormAppPath);
            }
        }

        private static int MinuteOfDailyUse()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                return Convert.ToInt32(TimeSpan.FromHours(4).TotalMinutes);
            }
            else
            {
                return Convert.ToInt32(TimeSpan.FromHours(3).TotalMinutes);
            }
        }

        protected override void OnStart(string[] args)
        {
        }
        protected override void OnStop()
        {
        }
    }
}
// gün, başlangıç kullanılan süre, yükseltilen kullanılmış süre, 
// 01.01.2023,55,(sistemden bilgisayar kaç dakika kullanıldıysa o süre + başlangıç kullanım süresi)
