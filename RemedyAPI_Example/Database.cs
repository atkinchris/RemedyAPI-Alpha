using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace RemedyAPI_Example {
    class Database {

        private string DatabaseLocation;
        private SQLiteConnection m_dbConnection;

        public Database( string path) {
            DatabaseLocation = path + "remedy.db";
            m_dbConnection = new SQLiteConnection( string.Format( "Data Source={0};", DatabaseLocation ) );
            if ( !File.Exists( DatabaseLocation ) ) {
                SQLiteConnection.CreateFile( DatabaseLocation );
                m_dbConnection.Open();
                var sql = "CREATE TABLE Stack (Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, Outstanding INTEGER, Submitted INTEGER, Resolved INTEGER)";
                var command = new SQLiteCommand( sql, m_dbConnection );
                command.ExecuteNonQuery();
            }
        }

        public void Add( int outstanding, int submitted, int resolved ) {
            using ( var connection = new SQLiteConnection( @"Data Source=" + DatabaseLocation ) ) {
                var insertSQL = new SQLiteCommand( "INSERT INTO Stack (Outstanding, Submitted, Resolved) VALUES (@o,@s,@r)", connection );
                insertSQL.Parameters.AddWithValue( "@o", outstanding );
                insertSQL.Parameters.AddWithValue( "@s", submitted );
                insertSQL.Parameters.AddWithValue( "@r", resolved );
                connection.Open();
                try {
                    insertSQL.ExecuteNonQuery();
                } catch ( Exception ex ) {
                    throw new Exception( ex.Message );
                }
            }
        }

        public Dictionary<DateTime, List<int>> GetData( int length ) {
            var data = new Dictionary<DateTime, List<int>>();
            using ( var connect = new SQLiteConnection( @"Data Source=" + DatabaseLocation ) ) {
                connect.Open();
                using ( SQLiteCommand fmd = connect.CreateCommand() ) {
                    fmd.CommandText = string.Format( "SELECT * FROM Stack ORDER BY Timestamp", length );
                    fmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = fmd.ExecuteReader();

                    int i = 0;
                    while ( r.Read() && i < length ) {
                        var timestamp = (DateTime)r[ 0 ];
                        var outstanding = int.Parse(r[ 1 ].ToString());
                        var submitted = int.Parse( r[ 2 ].ToString() );
                        var resolved = int.Parse( r[ 3 ].ToString() );

                        var values = new List<int> {
                                outstanding,
                                submitted,
                                resolved
                            };

                        data.Add( timestamp, values );
                        i++;
                    }
                }
            }
            return data;
        }

    }

    static class DateTimeExtensions {
        static public long ToJavascriptTime( this DateTime dt ) {
            return (long)dt.Subtract( new DateTime( 1970, 1, 1 ) ).TotalMilliseconds;
        }

        static public string ToSQLiteTime( this DateTime dt ) {
            string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format( dateTimeFormat, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond );
        }
    }
}
