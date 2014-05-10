using RemedyAPI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace RemedyAPI_Service {
    class RemedyService : ServiceBase {

        private Server server;
        private Queries queries;
        private Timer timer;
        private BackgroundWorker bw;

        static private string serviceName = "Remedy API Service";
        private string resultsPath;

        public RemedyService() {
            this.ServiceName = serviceName;
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.EventLog.Source = this.ServiceName;
        }

        static void Main() {
            System.ServiceProcess.ServiceBase.Run( new RemedyService() );

            if ( !EventLog.SourceExists( serviceName ) ) {
                EventLog.CreateEventSource( serviceName, "Application" );
            }
        }

        protected override void OnStart( string[] args ) {
            base.OnStart( args );
            var username = args[0];
            var password = args[1];
            resultsPath = args[2];

            bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler( bw_DoWork );
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler( bw_RunWorkerCompleted );

            server = new Server( username, password );
            queries = new Queries();
            timer = new Timer();
            timer.Interval = 60 * 1000;
            timer.Elapsed += new ElapsedEventHandler( RefreshData );

            var groups = new string[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };

            string today = DateTime.Today.ToShortDateString();
            queries.Add( "Submitted", new Query( string.Format( "(\'{0}\' > \"{1}\") AND (\'{2}\' < \"{3}\")", "Submit Date", today ), groups ) );
            queries.Add( "Resolved", new Query( string.Format( "(\'{0}\' > \"{1}\") AND (\'{2}\' < \"{3}\")", "Last Resolved Date", today ), groups ) );

            var outstandingQuery = new Query( string.Format( "(\'{0}\' < \"{1}\")", "Status", "Resolved" ), groups );
            outstandingQuery.users.Add( "Ian Taylor", true );
            queries.Add( "Outstanding", outstandingQuery );
            timer.Start();
            RefreshData( null, null );
            EventLog.WriteEntry( this.ServiceName, "Service Started.", EventLogEntryType.Information );
        }

        protected override void OnStop() {
            timer.Stop();
            timer = null;
            queries = null;
            server = null;
            EventLog.WriteEntry( this.ServiceName, "Service Stopped.", EventLogEntryType.Information );
        }

        private void RefreshData( object source, ElapsedEventArgs e ) {
            if ( bw.IsBusy != true ) {
                bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork( object sender, DoWorkEventArgs e ) {
            server.ExecuteQuery( queries );
        }

        private void bw_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            if ( e.Error == null ) {
                UpdateResults();
            }
            else {
                EventLog.WriteEntry( this.ServiceName, "Error: " + e.Error.Message.ToString(), EventLogEntryType.Error );
            }
        }

        private void UpdateResults() {
            var results = queries.GetResultsCount();

            if ( !File.Exists( resultsPath ) ) {
                var titles = new StringBuilder();
                titles.AppendFormat( "{0}", "Timecode" );
                foreach ( var title in results.Keys ) {
                    titles.AppendFormat( ",{0}", title );
                }
                titles.AppendLine();
                File.WriteAllText( resultsPath, titles.ToString() );
            }

            var output = new StringBuilder();
            output.Append( DateTime.Now.ToString() );
            foreach ( var result in results.Values ) {
                output.AppendFormat( ",{0}", result.ToString() );
            }
            using ( StreamWriter sw = File.AppendText( resultsPath ) ) {
                sw.WriteLine( output.ToString() );
            }
        }
    }
}
