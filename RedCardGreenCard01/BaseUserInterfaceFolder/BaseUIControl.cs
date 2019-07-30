using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RedCardGreenCard01.BaseUserInterfaceFolder
{
    public class BaseUIControl
    {
        protected bool debugMode = false;
        protected string classId = "BaseUIControl";
        protected string fnId = "";
        protected Font font;
        public Point position;
        public Size size;
        public int renderOrder = 0;
        public Color backgroundColor;
        public Color borderColor;
        public int borderWidth = 1;
        public bool visible = false;
        public bool updatable = false;

        CustomRect rect = new CustomRect();

        public BaseUIControl()
        {
            debugMode = Helpers.Debug;
            position = new Point();
            size = new Size(10, 10);
            borderWidth = 4;
            borderColor = Color.Black;
            backgroundColor = Color.White;
            visible = true;
            updatable = true;
        }
        public void Load(Point position, Size size, int borderWidth = 4)
        {
            this.position = position;
            this.size = size;
            this.borderWidth = borderWidth;
            rect.Load(position, size);
        }
        public void Update()
        {
            rect.Update();
        }
        public void Render(Graphics g)
        {
            Point strPos = new Point(20, 20);
            SolidBrush backgroundBrush = new SolidBrush(backgroundColor);
            Pen borderPen = new Pen(borderColor, borderWidth);

            rect.Render(g);

            backgroundBrush.Dispose();
            borderPen.Dispose();
            CleanupAfterRender(ref font);
        }
        

        virtual protected Font ReadyFontDefault()
        {
            if (debugMode) fnId = $"{classId}.ReadyFontDefault()";
            return new Font("Arial", 12, FontStyle.Regular);
        }
        virtual protected void CleanupAfterRender(ref Font font)
        {
            if (font != null)
            {
                font.Dispose();
                font = null;
            }
        }
        protected void DrawString(Graphics g, string text, Point position)
        {
            if (font == null) font = ReadyFontDefault();
            g.DrawString(text, font, Brushes.Black, position);
        }
    } // end of class BaseUIControl

    
    public abstract class BaseDrawableObject
    {
        protected bool debugMode = false;
        protected string classId = "BaseDrawableObject";
        protected string fnId = "";
        protected Font font;
        protected PointF origin;
        public bool visible = true;
        protected void DrawString(Graphics g, string text, Point position)
        {
            if (font == null) font = ReadyFontDefault();
            g.DrawString(text, font, Brushes.Black, position);
        }
        protected void DrawStringInBox(Graphics g, Font f, string text, ref Point position, ref SizeF boxSize, int rightpadding = 2)
        {
            SizeF temp = g.MeasureString(text, f);
            boxSize.Height += temp.Height;
            if (temp.Width > boxSize.Width) boxSize.Width = temp.Width + rightpadding;
            DrawString(g, text, position);
            position.Y += (int)(temp.Height + 0.49f);
        }
        protected Font ReadyFontDefault()
        {
            if (debugMode) fnId = $"{classId}.ReadyFontDefault()";
            return new Font("Arial", 12, FontStyle.Regular);
        }
        protected void CleanupAfterRender(ref Font font)
        {
            if (font != null)
            {
                font.Dispose();
                font = null;
            }
        }
        abstract public void Update();
        abstract public void Render(Graphics g);
    }
    public class Line : BaseDrawableObject
    {
        public PointF start;
        public PointF end;
        public Color color;
        public float width = 1;
        public float distance = 0;
        public float rotation = 0;

        float xdif = 0;
        float ydif = 0;
        float startingRot = 0;

        public Line()
        {
            classId = "Line";
            origin = new PointF();
            debugMode = Helpers.Debug;
            color = Color.Black;
            start = new Point();
            end = new Point();
        }

        public void Load(Point start, Point end)
        {
            origin = start;
            this.start = start;
            this.end = end;
            startingRot = CalculateRotation(start, end);
            rotation = startingRot;
            distance = Distance();
        }
        public float Distance()
        {
            float dx = (end.X - start.X);
            float dy = (end.Y - start.Y);
            float dxdy = (dx * dx) + (dy * dy);
            distance = (float)Math.Sqrt(dxdy);
            return distance;
        }
        public float Distance(PointF origin, PointF end)
        {
            float dx = (end.X - origin.X);
            float dy = (end.Y - origin.Y);
            float dxdy = (dx * dx) + (dy * dy);
            distance = (float)Math.Sqrt(dxdy);
            return distance;
        }
        protected float CalculateRotation(PointF start, PointF end)
        {
            float rot = 0;
            ydif = end.Y - start.Y;
            xdif = end.X - start.X;
            rot = (float)Math.Atan2(ydif, xdif);
            return rot;
        }
        override public void Update()
        {
            rotation += 0.01f;
            end.X = (float)(start.X + Math.Cos(rotation) * distance);
            end.Y = (float)(start.Y + Math.Sin(rotation) * distance);
            if (rotation > (2 * Math.PI)) rotation -= (float)(2 * Math.PI);
            if (rotation < 0) rotation += (float)(2 * Math.PI);
        }
        override public void Render(Graphics g)
        {
            Pen pen = new Pen(color);
            g.DrawLine(pen, start, end);
            pen.Dispose();
            CleanupAfterRender(ref font);
        }
        public void DrawStats(Graphics g, ref Point strPos, string objName)
        {
            font = ReadyFontDefault();
            Point topleft = new Point(strPos.X, strPos.Y);
            SizeF boxSize = new SizeF();

            DrawStringInBox(g, font, objName, ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"start: {start}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"end: {end}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"distance: {distance}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"startingRot: {startingRot}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"rotation: {rotation}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"ydif: {ydif}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"xdif: {xdif}", ref strPos, ref boxSize);

            g.DrawRectangle(Pens.Black, topleft.X, topleft.Y, boxSize.Width, boxSize.Height);
        }

        //protected void DrawStringInBox(Graphics g, Font f, string text, ref Point position, ref SizeF boxSize, int rightpadding = 2)
        //{
        //    SizeF temp = g.MeasureString(text, f);
        //    boxSize.Height += temp.Height;
        //    if (temp.Width > boxSize.Width) boxSize.Width = temp.Width + rightpadding;
        //    DrawString(g, text, position);
        //    position.Y += (int)(temp.Height + 0.49f);
        //}
        //virtual protected Font ReadyFontDefault()
        //{
        //    if (debugMode) fnId = $"{classId}.ReadyFontDefault()";
        //    return new Font("Arial", 12, FontStyle.Regular);
        //}
        //virtual protected void CleanupAfterRender(ref Font font)
        //{
        //    if (font != null)
        //    {
        //        font.Dispose();
        //        font = null;
        //    }
        //}
        //protected void DrawString(Graphics g, string text, Point position)
        //{
        //    if (font == null) font = ReadyFontDefault();
        //    g.DrawString(text, font, Brushes.Black, position);
        //}

    } // end of class Line
    public class LineV2 : BaseDrawableObject
    {
        public PointF start;
        public PointF end;
        public Color color;
        public float width = 1;
        public float distance = 0;
        public float rotation = 0;

        float segment1Distance = 0;
        float segment2Distance = 0;
        float segment1Rotation = 0;
        float segment2Rotation = 0;

        float xdif = 0;
        float ydif = 0;
        float startingRot = 0;

        public LineV2()
        {
            classId = "LineV2";
            origin = new PointF();
            debugMode = Helpers.Debug;
            color = Color.Black;
            start = new Point();
            end = new Point();
        }

        public void Load(Point start, Point end)
        {
            this.origin = start;
            this.start = start;
            this.end = end;
            segment1Rotation = CalculateRotation(origin, start);
            segment2Rotation = CalculateRotation(origin, end);
            segment1Distance = Distance(origin, start);
            segment2Distance = Distance(origin, end);
        }
        public void Load(Point origin, Point start, Point end)
        {
            this.origin = origin;
            this.start = start;
            this.end = end;
            segment1Rotation = CalculateRotation(origin, start);
            segment2Rotation = CalculateRotation(origin, end);
            segment1Distance = Distance(origin, start);
            segment2Distance = Distance(origin, end);
        }
        public float Distance()
        {
            float dx = (end.X - start.X);
            float dy = (end.Y - start.Y);
            float dxdy = (dx * dx) + (dy * dy);
            distance = (float)Math.Sqrt(dxdy);
            return distance;
        }
        public float Distance(PointF origin, PointF end)
        {
            float dx = (end.X - origin.X);
            float dy = (end.Y - origin.Y);
            float dxdy = (dx * dx) + (dy * dy);
            distance = (float)Math.Sqrt(dxdy);
            return distance;
        }
        protected float CalculateRotation(PointF start, PointF end)
        {
            float rot = 0;
            ydif = end.Y - start.Y;
            xdif = end.X - start.X;
            rot = (float)Math.Atan2(ydif, xdif);
            return rot;
        }
        override public void Update()
        {
            //rotation = 0.01f;
            segment1Rotation += rotation;
            segment2Rotation += rotation;
            start.X = (float)(origin.X + Math.Cos(segment1Rotation) * segment1Distance);
            start.Y = (float)(origin.Y + Math.Sin(segment1Rotation) * segment1Distance);
            end.X = (float)(origin.X + Math.Cos(segment2Rotation) * segment2Distance);
            end.Y = (float)(origin.Y + Math.Sin(segment2Rotation) * segment2Distance);

            if (segment1Rotation > (2 * Math.PI)) segment1Rotation -= (float)(2 * Math.PI);
            if (segment1Rotation < 0) segment1Rotation += (float)(2 * Math.PI);

            if (segment2Rotation > (2 * Math.PI)) segment2Rotation -= (float)(2 * Math.PI);
            if (segment2Rotation < 0) segment2Rotation += (float)(2 * Math.PI);
        }
        override public void Render(Graphics g)
        {
            Pen pen = new Pen(color);
            g.DrawLine(pen, start, end);
            pen.Dispose();
            CleanupAfterRender(ref font);
        }
        public void Render(Graphics g, Color c)
        {
            Pen pen = new Pen(c);
            g.DrawLine(pen, start, end);
            pen.Dispose();
            CleanupAfterRender(ref font);
        }
        public void Render(Graphics g, Pen p)
        {
            g.DrawLine(p, start, end);
            CleanupAfterRender(ref font);
        }
        public void DrawLinesFromOrigin(Graphics g, Color c)
        {
            Pen p = new Pen(c);
            g.DrawLine(p, origin, start);
            g.DrawLine(p, origin, end);
            p.Dispose();
        }
        public void DrawStats(Graphics g, ref Point strPos, string objName)
        {
            font = ReadyFontDefault();
            Point topleft = new Point(strPos.X, strPos.Y);
            SizeF boxSize = new SizeF();

            DrawStringInBox(g, font, objName, ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"origin: {origin}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"start: {start}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"end: {end}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"segment1Distance: {segment1Distance}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"segment2Distance: {segment2Distance}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"startingRot: {startingRot}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"segment1Rotation: {segment1Rotation}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"segment1Rotation: {segment2Rotation}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"ydif: {ydif}", ref strPos, ref boxSize);
            DrawStringInBox(g, font, $"xdif: {xdif}", ref strPos, ref boxSize);

            g.DrawRectangle(Pens.Black, topleft.X, topleft.Y, boxSize.Width, boxSize.Height);
        }
    } // end of class LineV2

    public class CustomRect : BaseDrawableObject
    {
        Point topLeft = new Point();
        Point bottomLeft = new Point();
        Point topRight = new Point();
        Point bottomRight = new Point();
        
        LineV2 top = new LineV2();
        LineV2 right = new LineV2();
        LineV2 bottom = new LineV2();
        LineV2 left = new LineV2();

        public Color borderColor = Color.Black;
        public Color defaultBorderColor = Color.Black;
        public Color backgroundColor = Color.White;
        public Color defaultBackgroundColor = Color.White;
        //public Color mouseOverColor = Color.AntiqueWhite;

        Size size;
        Rectangle r;

        public CustomRect()
        {
            classId = "CustomRect";
            if (debugMode) fnId = $"{classId}.CustomRect()";
            origin = new PointF();
            debugMode = Helpers.Debug;
        }
        public void Load(Point topLeft, Size size)
        {
            this.size = size;
            r = new Rectangle(topLeft, size);
            origin = topLeft;
            origin.X += size.Width / 2;
            origin.Y += size.Height / 2;
            SetCorners(topLeft, size);
            top.Load(new Point((int)origin.X, (int)origin.Y), topLeft, topRight);
            right.Load(new Point((int)origin.X, (int)origin.Y), topRight, bottomRight);
            bottom.Load(new Point((int)origin.X, (int)origin.Y), bottomRight, bottomLeft);
            left.Load(new Point((int)origin.X, (int)origin.Y), topLeft, bottomLeft);
            backgroundColor = defaultBackgroundColor;
            borderColor = defaultBorderColor;
        }
        protected void SetCorners(Point topLeft, Size size)
        {
            this.topLeft = topLeft;
            bottomLeft = new Point(topLeft.X, topLeft.Y + size.Height);
            topRight = new Point(topLeft.X + size.Width, topLeft.Y);
            bottomRight = new Point(topLeft.X + size.Width, topLeft.Y + size.Height);
            //top.rotation = 0.01f;
            //right.rotation = 0.01f;
            //bottom.rotation = 0.01f;
            //left.rotation = 0.01f;
        }
        public bool InBoundingBox(Point pos)
        {
            if (pos.X > topLeft.X && pos.X < topRight.X && pos.Y > topLeft.Y && pos.Y < bottomLeft.Y)
                return true;
            return false;
        }
        override public void Update()
        {
            top.Update();
            right.Update();
            bottom.Update();
            left.Update();
        }
        override public void Render(Graphics g)
        {
            SolidBrush bgBrush = new SolidBrush(backgroundColor);
            g.FillRectangle(bgBrush, r);
            top.Render(g, borderColor);
            right.Render(g, borderColor);
            bottom.Render(g, borderColor);
            left.Render(g, borderColor);
            bgBrush.Dispose();
        }

    } // end of class CustomRect : BaseDrawableObject

}



#region moving to own class 09/03/2018
//public class CustomButton : BaseDrawableObject
//{
//    CustomRect rect;// = new Rect();
//    string text = "Button";
//    bool textSet = false;
//    Bitmap textImage = null;
//    Point position;
//    Size size;
//    Point textPos;
//    SizeF textSize;
//    bool textSizeAndPosSet = false;
//    int textPadding = 2;
//    public bool mouseOver = false;
//    public bool clicked = false;
//    public Color mouseOverColor = Color.AntiqueWhite;
//    public Color clickedColor = Color.Green;
//    public CustomButton()
//    {
//        classId = "CustomButton";
//        rect = new CustomRect();
//        position = new Point();
//        size = new Size();
//        textPos = new Point();
//        textSize = new SizeF();
//    }

//    public void Load(Point topLeft, Size size)
//    {
//        position = topLeft;
//        this.size = size;
//        rect.Load(topLeft, size);
//        textSet = false;
//    }
//    public void Load(Point topLeft, string buttonText)
//    {

//        text = buttonText;
//        position = topLeft;
//        textSet = true;
//        rect.Load(position, new Size((int)(textSize.Width + 0.49), (int)(textSize.Height + 0.49)));

//    }

//    public bool InBoundingBox(Point pos)
//    {
//        return rect.InBoundingBox(pos);
//    }
//    public bool CheckIfMouseOver(Point pos)
//    {
//        mouseOver = InBoundingBox(pos) ? true : false;
//        rect.backgroundColor = mouseOver ? mouseOverColor : rect.defaultBackgroundColor;
//        return mouseOver;
//    }
//    public void Clicked()
//    {
//        if (debugMode) fnId = $"{classId}.Clicked()";
//        clicked = mouseOver ? true : false;
//        if (clicked) Helpers.PrintDebug(fnId, $"Clicked");
//    }

//    override public void Update()
//    {
//        rect.Update();
//    }
//    override public void Render(Graphics g)
//    {
//        if(!textSizeAndPosSet && textSet)
//        {
//            SetTextPositionAndSize(g, position, text);
//            rect.Load(position, new Size((int)(textSize.Width + 0.49), (int)(textSize.Height + 0.49)));
//            textSizeAndPosSet = true;
//        }
//        rect.backgroundColor = clicked ? clickedColor : ((mouseOver) ? mouseOverColor : rect.defaultBackgroundColor);
//        rect.Render(g);
//        DrawString(g, text, textPos);

//        clicked = false;
//        CleanupAfterRender(ref font);
//    }
//    void SetTextPositionAndSize(Graphics g, Point topLeft, string text)
//    {
//        font = ReadyFontDefault();
//        textSize = g.MeasureString(text, font);
//        textSize.Width += textPadding;
//        textSize.Height += textPadding;
//        textPos.X = topLeft.X + textPadding / 2;
//        textPos.Y = topLeft.Y + textPadding / 2;
//        //font.Dispose();
//    }

//} // end of class CustomButton

#endregion moving to own class 09/03/2018
