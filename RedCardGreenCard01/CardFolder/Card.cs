using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RedCardGreenCard01
{
    public class Card
    {
        string classId = "Card";
        public string suit;
        public int suitValue;
        public string name;
        public int value;
        public bool delt;
        public bool inplay;
        public bool faceDown;
        public Bitmap imageFront;
        public Bitmap imageBack;
        Size size;
        Size sizeBack;
        public Point position;

        
        public bool InPlay() { return inplay; }
        public Size Size() { return size; }


        public Card()
        {
            suit = "No Suit";
            suitValue = -1;
            name = "Empty";
            value = -1;
            delt = false;
            inplay = false;
            faceDown = true;
            size = new Size();
            sizeBack = new Size();
            position = new Point();
        }
        public void Load(string suit, int suitValue, string name, int value)
        {
            this.suit = suit;
            this.suitValue = suitValue;
            this.name = name;
            this.value = value;
        }
        public void SetImageFront(Bitmap image)
        {
            string functionId = $"{classId}.SetImageFront(Bitmap)";
            this.imageFront = image;
            size = image.Size;
        }
        public void SetImageBack(Bitmap image)
        {
            string functionId = $"{classId}.SetImageBack(Bitmap)";
            this.imageBack = image;
            sizeBack = image.Size;
        }

        public void RenderCard(Graphics g)
        {
            string functionId = $"{classId}.DrawCard(Graphics)";
            if (faceDown && imageBack != null)
            {
                DrawImage(g, imageBack, position);
                return;
            }
            if (!faceDown && imageFront != null)
            {
                DrawImage(g, imageFront, position);
                return;
            }
            if (imageFront == null)
                Helpers.PrintError(functionId, $"unable to draw card ({name}) - imageFront is null");
            if (imageBack == null)
                Helpers.PrintError(functionId, $"unable to draw card ({name}) - imageBack is null");
        }
        public void DrawCard(Graphics g, Point location)
        {
            string functionId = $"{classId}.DrawCard(Graphics)";
            if (faceDown &&  imageBack != null)
            {
                DrawImage(g, imageBack, location);
                return;
            }
            if (!faceDown && imageFront != null)
            {
                DrawImage(g, imageBack, location);
                return;
            }
            if (imageFront == null)
                Helpers.PrintError(functionId, $"unable to draw card ({name}) - imageFront is null");
            if (imageBack == null)
                Helpers.PrintError(functionId, $"unable to draw card ({name}) - imageBack is null");
        }
        void DrawImage(Graphics g, Bitmap bitmap, Point location)
        {
            string functionId = $"{classId}.DrawImage(Graphics, Bitmap, Point)";
            if (bitmap != null)
            {
                g.DrawImage(bitmap, location);
                return;
            }
            if (bitmap == null)
                Helpers.PrintError(functionId, $"Bitmap is null");
            if (location == null)
                Helpers.PrintError(functionId, $"Location is null");
        }
        public bool Clicked(Point clickPos)
        {
            Rectangle r = BoundingBox();
            if(clickPos.X > (r.X) && clickPos.X < (r.X + r.Width) && clickPos.Y > r.Y && clickPos.Y < (r.Y + r.Height))
            {
                return true;
            }
            return false;

        }
        Rectangle BoundingBox()
        {
            Rectangle r = new Rectangle();
            r.X = position.X;
            r.Y = position.Y;
            r.Width = size.Width;
            r.Height = size.Height;
            return r;
        }


        public void PrintFullName()
        {
            Console.Write($"{name} of {suit}");
        }
    }
}
