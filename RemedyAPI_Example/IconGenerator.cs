using System.Drawing;
using System.Drawing.Text;

namespace RemedyAPI_Example {
    static class IconGenerator {
        public static Icon GetIcon( string text ) {
            var bitmap = new Bitmap( 32, 32 );

            var drawFont = new Font( "Calibri", 16, FontStyle.Bold );
            var drawBrush = new SolidBrush( Color.White );

            var graphics = Graphics.FromImage( bitmap );

            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString( text, drawFont, drawBrush, 1, 2 );
            var createdIcon = Icon.FromHandle( bitmap.GetHicon() );

            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();

            return createdIcon;
        }
    }
}
