using System.Drawing;

namespace RemedyAPI_Example {
    static class IconGenerator {
        public static Icon GetIcon( string text ) {
            Bitmap bitmap = new Bitmap( 32, 32 );

            System.Drawing.Font drawFont = new System.Drawing.Font( "Calibri", 16, FontStyle.Bold );
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush( System.Drawing.Color.White );

            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage( bitmap );

            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.DrawString( text, drawFont, drawBrush, 1, 2 );
            Icon createdIcon = Icon.FromHandle( bitmap.GetHicon() );

            drawFont.Dispose();
            drawBrush.Dispose();
            graphics.Dispose();
            bitmap.Dispose();

            return createdIcon;
        }
    }
}
