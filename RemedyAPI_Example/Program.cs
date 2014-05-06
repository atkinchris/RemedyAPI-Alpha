using System.Windows.Forms;

namespace RemedyQuery {
    class Program {
        static void Main( string[] args ) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainForm() );
        }
    }
}
