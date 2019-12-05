using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;
namespace WindowsFormsApp1
{
   
    public partial class Form1 : Form
    {
        private SimpleOpenGlControl AnT;

        //y' =  y - x^2 - исходное уравнение
        //k = y - x^2
        //y = x^2 + k, k = -4, .... 4 
        private double y(double x, double k)
        {
            return Math.Pow(x, 2) + k;
        }
        //result расчитывает координаты точек интегральной кривой для заданных начальных условий (x0,y0)
        private List<double> result(double x0, double y0)
        {
            List<double> coordinates = new List<double>();
            double k;
            double y, y1 = y0;
            if (x0 < 4)
            {
                for (double i = x0; i <= 4; i += 0.25)
                {
                    k = y1 - Math.Pow(i, 2);//k = y1 - x^2 - найдем угловой коэффициент 
                    y = i * k;//найдем y для наденного k
                    coordinates.Add(y);//добавляем в список, хранящий координаты 
                    y1 = y;//старому значению y1 присвоим новое значение у
                }
            }
            else//в случае если x0 больше 4
            {
                //обратный ход 
                for (double i = x0; i >= -4; i -= 0.25)
                {
                    k = y1 - Math.Pow(i, 2);
                    y = (i - k) * k;
                    coordinates.Add(y);
                    y1 = y;
                }
            }
            return coordinates;
        }

        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
            print();       
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            Glut.glutInit();//инициализация,библиотека
            Glut.glutInitDisplayMode(Glut.GLUT_RGBA | Glut.GLUT_DOUBLE);
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);//цвет экрана
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);//матрица проекции на экран
            Gl.glLoadIdentity();
            Glu.gluOrtho2D(0.0, AnT.Width, 0.0, AnT.Height);
            
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            
        }


        public void print()// в методе print происходит отрисовка системы координат и решения уравнения
        {
            Gl.glColor3f(0.0f, 1.0f, 0.0f);
            
            //отрисовываем систему координат
            Gl.glBegin(Gl.GL_LINES);
            
                Gl.glVertex2d(2.0, 0);
                Gl.glVertex2d(-2.0, 0);
                Gl.glVertex2d(0, 2.0);
                Gl.glVertex2d(0, -2.0);

            Gl.glEnd();
            
            //y = x^2 + k, k = -4, ....4 для каждого k рисуем график
            Gl.glColor3f(1.0f, 0.0f, 1.0f);
            for (double k = -4; k <= 4; k += 0.5)
            {
                Gl.glBegin(Gl.GL_LINE_STRIP);
                for (double j = -4; j <= 4; j += 0.01)
                {
                    Gl.glVertex2d(j, y(j, k));// метод y(j,k) соответствует функции y = x^2 + k
                }
                Gl.glEnd();
            }
            //for (double k = -4; k <= 4; k += 0.5)
            //{
            //    Gl.glBegin(Gl.GL_LINES);
            //    for (double j = -4; j <= 4; j += 0.3)
            //    {
            //        Gl.glVertex2d(j - 0.1, k * (j - 0.1) + y(j, k));
            //        Gl.glVertex2d(j + 0.1, k * (j + 0.1) + y(j, k));

            //    }
            //    Gl.glEnd();
            //}

            // u, w  - задаем начальное условие
            double u = -1;
            double w = 1;
            List<double> res = result(u, w);//получаем список координат точек интегральной кривой 
                                            //для начального условия (u,w)
            Gl.glColor3f(1.0f, 0.0f, 0.0f);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            foreach(double r in res)
            {
                Gl.glVertex2d(u,r);//для каждго r из res рисуем точку 
                u += 0.25;//прибавляем шаг
            }
            Gl.glEnd();
            
            AnT.Invalidate();       
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void simpleOpenGlControl1_Load_1(object sender, EventArgs e)
        {

        }
   
        private void InitializeComponent()
        {
            this.AnT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.SuspendLayout();
            // 
            // AnT
            // 
            this.AnT.AccumBits = ((byte)(0));
            this.AnT.AutoCheckErrors = false;
            this.AnT.AutoFinish = false;
            this.AnT.AutoMakeCurrent = true;
            this.AnT.AutoSwapBuffers = true;
            this.AnT.BackColor = System.Drawing.Color.Black;
            this.AnT.ColorBits = ((byte)(32));
            this.AnT.DepthBits = ((byte)(16));
            this.AnT.Location = new System.Drawing.Point(12, 12);
            this.AnT.Name = "AnT";
            this.AnT.Size = new System.Drawing.Size(497, 431);
            this.AnT.StencilBits = ((byte)(0));
            this.AnT.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(560, 455);
            this.Controls.Add(this.AnT);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }
    }
}
