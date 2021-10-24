using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        int r = 10;
        Point movePoint;
        Graphics graph;
        List<Point> pointsP;
        bool polIsDraw = false;
        public Form1()
        {
            InitializeComponent();
            graph = pictureBox1.CreateGraphics();
            pointsP = new List<Point>();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                graph.DrawEllipse(Pens.Red, e.X - r / 2, e.Y - r / 2, r, r);
                pointsP.Add(new Point(e.X, e.Y));
                label2.Text = pointsP.Count.ToString();
                MyDraw();
                DrawBezier();
            }
            if (radioButton2.Checked)
            {
                movePoint = PointInList(pointsP, new Point(e.X, e.Y));
            }
            if (radioButton3.Checked)
            {
                Point deletePoint = PointInList(pointsP, new Point(e.X, e.Y));
                if (deletePoint != new Point(0, 0))
                {
                    pointsP.Remove(deletePoint);
                    MyDraw();
                    DrawBezier();
                }
                    
            }
            //if (polIsDraw)
            //{
            //    //pointsP = new List<Point>();
            //    pointsP.Add(new Point(e.X, e.Y));
            //    polIsDraw = false;
            //}
            //else
            //    pointsP.Add(new Point(e.X, e.Y));

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label4.Text = e.X + " " + e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (radioButton2.Checked && movePoint != new Point(0, 0))
            {
                int ind = pointsP.IndexOf(movePoint);
                pointsP[ind] = new Point(e.X, e.Y);
                MyDraw();
                DrawBezier();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (pointsP.Count < 4)
                label1.Text = "You need add " + (4 - pointsP.Count % 4) + " more point(s)";
            //else if (pointsP.Count > 4 && pointsP.Count % 3 != 1)
            //{
            //    label1.Text = "You need add " + (3 - ((pointsP.Count - 1) % 3)) + " more point(s)";
            //}
            else
            {
                label1.Text = "Well done :)";
                //Point[] pnts = new Point[pointsP.Count];
                MyDraw();
                DrawBezier();
            }
        }

        public void MyDraw()
        {
            graph.Clear(Color.White);
            for (int i = 0; i < pointsP.Count; i++)
            {
                if (i % 3 == 0)
                    graph.DrawEllipse(Pens.Red, pointsP[i].X - r / 2, pointsP[i].Y - r / 2, r, r);
                else
                    graph.DrawEllipse(Pens.Cyan, pointsP[i].X - r / 2, pointsP[i].Y - r / 2, r, r);
                
            }
               
            for (int i = 0; i < pointsP.Count - 1; i++)
            {
                //graph.DrawEllipse(Pens.Red, pointsP[i + 1].X - r / 2, pointsP[i + 1].Y - r / 2, r, r);
                graph.DrawLine(Pens.LightGray, pointsP[i], pointsP[i + 1]);
                //pnts[i] = pointsP[i];
            }
        }

        public void DrawBezier()
        {
            for (int i = 0; i <= pointsP.Count - 4; i += 3)
            {
                List<Point> pointsB = new List<Point>();
                float t = 0, h = 0.01F;
                float B0x = pointsP[i].X, B0y = pointsP[i].Y;
                float Bx = pointsP[i].X, By = pointsP[i].X;
                while (t < 1)
                {
                    Bx =    pointsP[i].X * (1 - t) * (1 - t) * (1 - t) + 
                        3 * pointsP[i + 1].X * (1 - t) * (1 - t) * t +
                        3 * pointsP[i + 2].X * (1 - t) * t * t +
                            pointsP[i + 3].X * t * t * t;
                    By = pointsP[i].Y * (1 - t) * (1 - t) * (1 - t) +
                        3 * pointsP[i + 1].Y * (1 - t) * (1 - t) * t +
                        3 * pointsP[i + 2].Y * (1 - t) * t * t +
                            pointsP[i + 3].Y * t * t * t;
                    pointsB.Add(new Point((int)Bx, (int)By));
                    graph.DrawLine(Pens.Blue, new Point((int)B0x, (int)B0y), new Point((int)Bx, (int)By));
                    B0x = Bx;
                    B0y = By;
                    t += h;
                }
                
                //Point[] pnts = new Point[pointsB.Count];
                //for (int j = 0; j < pointsB.Count; j++)
                //    pnts[j] = pointsB[j];
                //graph.DrawCurve(Pens.Blue, pnts);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            graph.Clear(Color.White);
            pointsP = new List<Point>();
        }

        public Point PointInList(List<Point> pointsP, Point point)
        {
            foreach (Point p in pointsP)
                if (p.X > point.X - r && p.X < point.X + r && p.Y > point.Y - r && p.Y < point.Y + r)
                    return p;
            return new Point(0,0);
        }
    }
}
