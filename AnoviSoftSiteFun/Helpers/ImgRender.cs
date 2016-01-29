using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnoviSoftSiteFun.Helpers
{
    class ImgRender
    {
        public static Image DrawText(String text, Font font, Color textColor, Color backColor)
        {
            var img = new Bitmap(1, 1);
            var drawing = Graphics.FromImage(img);
            var textSize = drawing.MeasureString(text, font);
            img.Dispose();
            drawing.Dispose();
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);
            drawing = Graphics.FromImage(img);
            drawing.Clear(backColor);
            Brush textBrush = new SolidBrush(textColor);
            drawing.DrawString(text, font, textBrush, 0, 0);
            drawing.Save();
            textBrush.Dispose();
            drawing.Dispose();
            return img;

        }
    }
}
