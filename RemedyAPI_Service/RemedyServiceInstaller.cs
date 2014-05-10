using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace RemedyAPI_Service {

    [RunInstaller( true )]
    public class RemedyServiceInstaller : Installer {

        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public RemedyServiceInstaller() {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "Remedy API Service";

            Installers.Add( serviceInstaller );
            Installers.Add( processInstaller );
        }
    }
}