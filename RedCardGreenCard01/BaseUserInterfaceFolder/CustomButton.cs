using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RedCardGreenCard01.BaseUserInterfaceFolder
{
    public class CustomButton : BaseDrawableObject
    {
        CustomRect rect;// = new Rect();
        string text = "Button";
        bool textSet = false;
        Point position;
        Size size;
        Point textPos;
        SizeF textSize;
        bool textSizeAndPosSet = false;
        int textPadding = 2;
        public bool mouseOver = false;
        public bool clicked = false;
        public Color mouseOverColor = Color.AntiqueWhite;
        public Color clickedColor = Color.Green;
        public CustomButton()
        {
            classId = "CustomButton";
            rect = new CustomRect();
            position = new Point();
            size = new Size();
            textPos = new Point();
            textSize = new SizeF();
        }

        public void Load(Point topLeft, Size size)
        {
            position = topLeft;
            this.size = size;
            rect.Load(topLeft, size);
            textSet = false;
        }
        public void Load(Point topLeft, string buttonText)
        {

            text = buttonText;
            position = topLeft;
            textSet = true;
            rect.Load(position, new Size((int)(textSize.Width + 0.49), (int)(textSize.Height + 0.49)));

        }

        public bool InBoundingBox(Point pos)
        {
            return rect.InBoundingBox(pos);
        }
        public bool CheckIfMouseOver(Point pos)
        {
            mouseOver = InBoundingBox(pos) ? true : false;
            rect.backgroundColor = mouseOver ? mouseOverColor : rect.defaultBackgroundColor;
            return mouseOver;
        }
        public void Clicked()
        {
            if (debugMode) fnId = $"{classId}.Clicked()";
            clicked = mouseOver ? true : false;
            if (clicked) Helpers.PrintDebug(fnId, $"Clicked");
        }

        override public void Update()
        {
            rect.Update();
        }
        override public void Render(Graphics g)
        {
            if (!textSizeAndPosSet && textSet)
            {
                SetTextPositionAndSize(g, position, text);
                rect.Load(position, new Size((int)(textSize.Width + 0.49), (int)(textSize.Height + 0.49)));
                textSizeAndPosSet = true;
            }
            rect.backgroundColor = clicked ? clickedColor : ((mouseOver) ? mouseOverColor : rect.defaultBackgroundColor);
            rect.Render(g);
            DrawString(g, text, textPos);

            clicked = false;
            CleanupAfterRender(ref font);
        }
        void SetTextPositionAndSize(Graphics g, Point topLeft, string text)
        {
            font = ReadyFontDefault();
            textSize = g.MeasureString(text, font);
            textSize.Width += textPadding;
            textSize.Height += textPadding;
            textPos.X = topLeft.X + textPadding / 2;
            textPos.Y = topLeft.Y + textPadding / 2;
            //font.Dispose();
        }

    } // end of class CustomButton
}
