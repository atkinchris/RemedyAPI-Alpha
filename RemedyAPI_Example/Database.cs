using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace RemedyAPI_Example {
    class Database {

        private readonly string _databaseLocation;

        public Database( string path ) {
            _databaseLocation = path + "remedy.db";
            var mDbConnection = new SQLiteConnection( string.Format( "Data Source={0};", _databaseLocation ) );
            if ( File.Exists( _databaseLocation ) ) {
                return;
            }
            SQLiteConnection.CreateFile( _databaseLocation );
            mDbConnection.Open();
            const string sql = "CREATE TABLE Stack (Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP, Outstanding INTEGER, Submitted INTEGER, Resolved INTEGER)";
            var command = new SQLiteCommand( sql, mDbConnection );
            command.ExecuteNonQuery();
        }

        public void Add( int outstanding, int submitted, int resolved ) {
            using ( var connection = new SQLiteConnection( @"Data Source=" + _databaseLocation ) ) {
                var insertSql = new SQLiteCommand( "INSERT INTO Stack (Outstanding, Submitted, Resolved) VALUES (@o,@s,@r)", connection );
                insertSql.Parameters.AddWithValue( "@o", outstanding );
                insertSql.Parameters.AddWithValue( "@s", submitted );
                insertSql.Parameters.AddWithValue( "@r", resolved );
                connection.Open();
                try {
                    insertSql.ExecuteNonQuery();
                } catch ( Exception ex ) {
                    throw new Exception( ex.Message );
                }
            }
        }

        public Dictionary<DateTime, List<int>> GetData( int length ) {
            var data = new Dictionary<DateTime, List<int>>();
            using ( var connect = new SQLiteConnection( @"Data Source=" + _databaseLocation ) ) {
                connect.Open();
                using ( var fmd = connect.CreateCommand() ) {
                    fmd.CommandText = "SELECT * FROM Stack ORDER BY Timestamp";
                    fmd.CommandType = CommandType.Text;
                    var r = fmd.ExecuteReader();

                    var i = 0;
                    while ( r.Read() && i < length ) {
                        var timestamp = (DateTime)r[0];
                        var outstanding = int.Parse( r[1].ToString() );
                        var submitted = int.Parse( r[2].ToString() );
                        var resolved = int.Parse( r[3].ToString() );

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

        static public string ToSqLiteTime( this DateTime dt ) {
            const string dateTimeFormat = "{0}-{1}-{2} {3}:{4}:{5}.{6}";
            return string.Format( dateTimeFormat, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond );
        }
    }
}
