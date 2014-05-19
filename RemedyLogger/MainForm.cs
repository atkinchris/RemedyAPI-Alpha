using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Principal;
using System.Timers;
using System.Windows.Forms;
using RemedyAPI;
using Timer = System.Timers.Timer;

namespace RemedyLogger {
    public partial class MainForm : Form {

        private Queries _queries;
        private Server _remedy;
        private readonly BackgroundWorker _bw = new BackgroundWorker();
        private readonly Timer _timer = new Timer();
        private string _path;
        private Database _db;

        public MainForm() {
            InitializeComponent();

            notifyIcon1.Icon = IconGenerator.GetIcon( @"R" );
            notifyIcon1.Text = "R! " + statusLabel.Text;

            _bw.DoWork += bw_DoWork;
            _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            startButton.Click += Start;
            _timer.Interval = 60 * 1000;
            _timer.Elapsed += RefreshData;

            var windowsIdentity = WindowsIdentity.GetCurrent();
            if ( windowsIdentity != null ) {
                var identity = windowsIdentity.Name;
                usernameInput.Text = identity.Substring( identity.IndexOf( @"\", StringComparison.Ordinal ) + 1 );
            }
            outputPathText.Text = @"Z:\stats\";
        }

        private void Start( object sender, EventArgs e ) {

            var username = usernameInput.Text;
            var password = passwordInput.Text;
            _path = outputPathText.Text;

            if ( string.IsNullOrEmpty( username ) ) {
                MessageBox.Show( "Invalid username", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( string.IsNullOrEmpty( password ) ) {
                MessageBox.Show( "Invalid password", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( !Directory.Exists( _path ) ) {
                MessageBox.Show( "Invalid directory", "Oops", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            _db = new Database( _path );

            _remedy = new Server( username, password ) {
                CacheTime = 45
            };
            _queries = new Queries();

            var groups = new[] {
                "Collaboration Services",
                "CS Deploy & Operate",
                "CS Deploy and Operate",
                "End User Devices"
            };

            string today = DateTime.Today.ToShortDateString();
            _queries.Add( "Submitted", new Query( string.Format( "(\'{0}\' > \"{1}\")", "Submit Date", today ), groups ) { Status = StatusTypes.All } );
            _queries.Add( "Resolved", new Query( string.Format( "(\'{0}\' > \"{1}\")", "Last Resolved Date", today ), groups ) { Status = StatusTypes.Closed } );
            var outstandingQuery = new Query( string.Format( "(\'{0}\' < \"{1}\")", "Status", "Resolved" ), groups );
            outstandingQuery.Users.Add( "Ian Taylor", true );
            _queries.Add( "Outstanding", outstandingQuery );
            ToggleRefresh( true );
            RefreshData( null, null );
        }

        private void Stop( object sender, EventArgs e ) {
            ToggleRefresh( false );
            _remedy = null;
        }

        private void ToggleRefresh( bool start ) {
            usernameInput.Enabled = !start;
            passwordInput.Enabled = !start;
            outputPathText.Enabled = !start;
            startButton.Text = start ? "Stop" : "Start";
            if ( start ) {
                startButton.Click -= Start;
                startButton.Click += Stop;
            } else {
                startButton.Click -= Stop;
                startButton.Click += Start;
            }
            _timer.Enabled = start;
        }

        private void RefreshData( object source, ElapsedEventArgs e ) {
            statusLabel.Text = "Refreshing data...";
            if ( _bw.IsBusy != true ) {
                _bw.RunWorkerAsync();
            }
        }

        private void bw_DoWork( object sender, DoWorkEventArgs e ) {
            _remedy.ExecuteQuery( _queries );
        }

        private void bw_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e ) {
            if ( e.Error == null ) {
                UpdateResults();
            } else {
                statusLabel.Text = "Error: " + e.Error.Message;
            }
        }

        private void UpdateResults() {
            statusLabel.Text = "Ready.";
            var results = _queries.GetResultsCount();

            _db.Add( results["Outstanding"], results["Submitted"], results["Resolved"] );
            var dbResults = _db.GetData( 60 );

            var flotData = new List<FlotSeries> {
                new FlotSeries("Outstanding", dbResults, 1),
                new FlotSeries("Submitted", dbResults, 2),
                new FlotSeries("Resolved", dbResults, 3)
            };
            Json.WritetoFile( flotData, _path + "results.json" );
        }

        private void notifyIcon1_DoubleClick( object sender, EventArgs e ) {
            Show();
            if ( WindowState == FormWindowState.Minimized ) {
                WindowState = FormWindowState.Normal;
            }
            Activate();
        }

        protected override void OnResize( EventArgs e ) {
            if ( WindowState == FormWindowState.Minimized )
                Hide();
            base.OnResize( e );
        }
    }
}
