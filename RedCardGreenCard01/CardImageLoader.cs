using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace RedCardGreenCard01
{
    public class CardImageLoader
    {
        static string resourcePath = Environment.CurrentDirectory + @"\..\..\Resources\";
        static string sheetFile = @"decksheet.png";
        Bitmap cardSheet;

        Point sheetPos = new Point(0, 0);
        Size cardSize = new Size(81, 117);

        ///
        //void SetNextCardPositionOnSheet()
        //{
        //    //Point pos = new Point();
        //    if (sheetPos.X + cardSize.Width < cardSheet.Width)
        //    {
        //        sheetPos.X += cardSize.Width;
        //    }
        //    else
        //    {
        //        sheetPos.X = 0;
        //        if (sheetPos.Y + cardSize.Height < cardSheet.Height)
        //        {
        //            sheetPos.Y += cardSize.Height;
        //        }
        //        else
        //            sheetPos.Y = 0;
        //    }
        //    //return pos;
        //}
        ///


    } // end of class CardImageLoader
}
