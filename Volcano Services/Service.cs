using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Volcano_Services
{
    public partial class Service : ServiceBase
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool DeleteVolumeMountPoint(string lpszVolumeMountPoint);

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string appPath = getAPPPath();

            if (appPath == null)
            {
                this.Stop();
                return;
            }
            else
            {
                try
                {
                    Process process = new Process();

                    process.StartInfo.FileName = appPath;
                    process.StartInfo.Arguments = "/auto-start";
                    process.StartInfo.UseShellExecute = false;

                    process.Start();
                }
                catch
                {
                    Console.WriteLine("An error occurred while automatically starting the volcano background program");

                }
            }

            this.Stop();
        }

        protected override void OnStop()
        {
        }

        private string getAPPPath()
        {
            string driveLetter = Environment.GetEnvironmentVariable("SystemDrive");

            string drivePath = $@"{driveLetter}\";

            bool driveExist = DeleteVolumeMountPoint(drivePath);

            if (driveExist)
            {
                string appPath = $@"{drivePath}Program Files\Volcano\volcano_service.exe";

                if (File.Exists(appPath))
                {
                    return appPath;
                } else { return null; }
            }
            else
            {
                return null;
            }
        }
    }
}
