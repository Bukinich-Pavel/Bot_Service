using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace Bot
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        ServiceInstaller service1Installer;
        ServiceInstaller service2Installer;
        ServiceProcessInstaller processInstaller;
        public Installer1()
        {
            InitializeComponent();
            service1Installer = new ServiceInstaller();
            service2Installer = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            service1Installer.StartType = ServiceStartMode.Automatic;
            service1Installer.ServiceName = "BotKsy";
            service2Installer.StartType = ServiceStartMode.Automatic;
            service2Installer.ServiceName = "BotSendKsy";
            Installers.Add(processInstaller);
            Installers.Add(service1Installer);
            Installers.Add(service2Installer);
        }
    }
}
