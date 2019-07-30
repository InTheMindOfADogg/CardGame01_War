using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RedCardGreenCard01.InputControllersFolder
{
    public class MouseController
    {
        protected string classId = "MouseController";
        protected string fnId = "";

        bool leftButtonDown = false;            //lbd = leftButtonDown
        bool rightButtonDown = false;           //rbd = rightButtonDown
        bool middleButtonDown = false;          //mbd = middleButtonDown
        bool lastLeftButtonDown = false;        //llbd = lastLeftButtonDown
        bool lastRightButtonDown = false;       //lrbd = lastRightButtonDown
        bool lastMiddleButtonDown = false;      //lmbd = lastMiddleButtonDown

        int wheelDelta = 0;
        int totalWheelDelta = 0;
        public bool scrollUp = false;
        public bool scrollDown = false;

        public bool leftClick = false;
        public bool rightClick = false;
        public bool middleClick = false;

        public bool leftHeld = false;
        public bool rightHeld = false;
        public bool middleHeld = false;

        public bool leftRelease = false;
        public bool rightRelease = false;

        public Point position = new Point();
        public Point lastPosition = new Point();
        public bool moving = false;
        
        List<GraphicsPath> pathList = new List<GraphicsPath>();
        GraphicsPath currentPath;

        public MouseController()
        {
            currentPath = new GraphicsPath();
        }
        public void PositionReader(MouseEventArgs e)
        {
            position = e.Location;
        }
        public void ButtonDownReader(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                leftButtonDown = true;
            if (e.Button == MouseButtons.Right)
                rightButtonDown = true;
            if (e.Button == MouseButtons.Middle)
                middleButtonDown = true;
        }
        public void ButtonUpReader(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                leftButtonDown = false;
            if (e.Button == MouseButtons.Right)
                rightButtonDown = false;
            if (e.Button == MouseButtons.Middle)
                middleButtonDown = false;
        }
        public void WheelScrollReader(MouseEventArgs e)
        {
            wheelDelta = e.Delta;
        }
        public void SetCurrentState()
        {
            leftClick = (!lastLeftButtonDown && leftButtonDown) ? true : false;
            rightClick = (!lastRightButtonDown && rightButtonDown) ? true : false;
            middleClick = (!lastMiddleButtonDown && middleButtonDown) ? true : false;

            leftHeld = (lastLeftButtonDown && leftButtonDown) ? true : false;
            rightHeld = (lastRightButtonDown && rightButtonDown) ? true : false;
            middleHeld = (lastMiddleButtonDown && middleButtonDown) ? true : false;

            leftRelease = (!leftHeld && lastLeftButtonDown) ? true : false;
            rightRelease = (!rightHeld && lastRightButtonDown) ? true : false;

            moving = (lastPosition != position) ? true : false;
            scrollUp = (wheelDelta > 0) ? true : false;
            scrollDown = (wheelDelta < 0) ? true : false;
            if (wheelDelta != 0)
                totalWheelDelta += wheelDelta;
            wheelDelta = 0;
        }
        public void SetLastState()
        {
            lastPosition = position;
            lastLeftButtonDown = leftButtonDown;
            lastRightButtonDown = rightButtonDown;
            lastMiddleButtonDown = middleButtonDown;
        }
        public void WriteStats(Graphics g, Font f)
        {
            //Font f = new Font("Arial", 12, FontStyle.Regular);
            Point strPos = new Point();
            int deltaY = 20;
            g.DrawString($"mouse position: {position.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"last mouse position: {lastPosition.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"mouse moving: {moving.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"left button down: {leftButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"right button down: {rightButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"middle button down: {middleButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"last left button down: {lastLeftButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"last right button down: {lastRightButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"last middle button down: {lastMiddleButtonDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"left button released: {leftRelease.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"scroll up: {scrollUp.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"last scroll up: {scrollDown.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
            g.DrawString($"Total wheel scroll: {totalWheelDelta.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
        }
        public void DrawPositionString(Graphics g, Font f)
        {
            fnId = $"{classId}.AddPlayCardButton()";
            if (f == null)
            {
                Helpers.PrintError(fnId, $"Font is null");
                return;
            }
            Point strPos = new Point();
            int deltaY = 20;
            g.DrawString($"mouse position: {position.ToString()}", f, Brushes.Black, strPos);
            strPos.Y += deltaY;
        }

        public void StartDrawPath()
        {
            currentPath.AddLine(position.X, position.Y, position.X, position.Y);
        }
        public void EndDrawPath()
        {
            if (Helpers.Debug) fnId = $"{classId}.EndDrawPath()";
            pathList.Add(currentPath);
            Helpers.PrintDebug(fnId, $"pathList.Count: {pathList.Count}");
            currentPath = new GraphicsPath();
        }
        public void ClearPath()
        {
            if (Helpers.Debug) fnId = $"{classId}.ClearPath()";
            currentPath.ClearMarkers();
            pathList.Clear();
            Helpers.PrintDebug(fnId, $"pathList.Count: {pathList.Count}");
        }
        public void DrawPaths(Graphics g)
        {
            g.DrawPath(Pens.Red, currentPath);
            for (int i = 0; i < pathList.Count; i++)
            {
                g.DrawPath(Pens.Red, pathList[i]);
            }
        }

    } // end of class MouseController
}
