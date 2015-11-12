using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;

namespace asgn5v1
{
    /// <summary>
    /// Summary description for Transformer.
    /// </summary>
    public class Transformer : System.Windows.Forms.Form
    {
        private volatile bool abort;
        private volatile bool threadRunning;
        public delegate void constantRotation(bool x, bool y, bool z);
        Thread worker;

        private System.ComponentModel.IContainer components;
        //private bool GetNewData();

        // basic data for Transformer

        int numpts = 0;
        int numlines = 0;
        bool gooddata = false;
        double[,] vertices;
        double[,] scrnpts;
        double[,] ctrans = new double[4, 4];  //your main transformation matrix
        private System.Windows.Forms.ImageList tbimages;
        private System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.ToolBarButton transleftbtn;
        private System.Windows.Forms.ToolBarButton transrightbtn;
        private System.Windows.Forms.ToolBarButton transupbtn;
        private System.Windows.Forms.ToolBarButton transdownbtn;
        private System.Windows.Forms.ToolBarButton toolBarButton1;
        private System.Windows.Forms.ToolBarButton scaleupbtn;
        private System.Windows.Forms.ToolBarButton scaledownbtn;
        private System.Windows.Forms.ToolBarButton toolBarButton2;
        private System.Windows.Forms.ToolBarButton rotxby1btn;
        private System.Windows.Forms.ToolBarButton rotyby1btn;
        private System.Windows.Forms.ToolBarButton rotzby1btn;
        private System.Windows.Forms.ToolBarButton toolBarButton3;
        private System.Windows.Forms.ToolBarButton rotxbtn;
        private System.Windows.Forms.ToolBarButton rotybtn;
        private System.Windows.Forms.ToolBarButton rotzbtn;
        private System.Windows.Forms.ToolBarButton toolBarButton4;
        private System.Windows.Forms.ToolBarButton shearrightbtn;
        private System.Windows.Forms.ToolBarButton shearleftbtn;
        private System.Windows.Forms.ToolBarButton toolBarButton5;
        private System.Windows.Forms.ToolBarButton resetbtn;
        private System.Windows.Forms.ToolBarButton exitbtn;
        int[,] lines;

        public Transformer()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            Text = "COMP 4560:  Assignment 5 (200830) (Your Name Here)";
            ResizeRedraw = true;
            BackColor = Color.Black;
            MenuItem miNewDat = new MenuItem("New &Data...",
                new EventHandler(MenuNewDataOnClick));
            MenuItem miExit = new MenuItem("E&xit",
                new EventHandler(MenuFileExitOnClick));
            MenuItem miDash = new MenuItem("-");
            MenuItem miFile = new MenuItem("&File",
                new MenuItem[] { miNewDat, miDash, miExit });
            MenuItem miAbout = new MenuItem("&About",
                new EventHandler(MenuAboutOnClick));
            Menu = new MainMenu(new MenuItem[] { miFile, miAbout });

            threadRunning = false;
            abort = false;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Transformer));
            this.tbimages = new System.Windows.Forms.ImageList(this.components);
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.transleftbtn = new System.Windows.Forms.ToolBarButton();
            this.transrightbtn = new System.Windows.Forms.ToolBarButton();
            this.transupbtn = new System.Windows.Forms.ToolBarButton();
            this.transdownbtn = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.scaleupbtn = new System.Windows.Forms.ToolBarButton();
            this.scaledownbtn = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.rotxby1btn = new System.Windows.Forms.ToolBarButton();
            this.rotyby1btn = new System.Windows.Forms.ToolBarButton();
            this.rotzby1btn = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.rotxbtn = new System.Windows.Forms.ToolBarButton();
            this.rotybtn = new System.Windows.Forms.ToolBarButton();
            this.rotzbtn = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
            this.shearrightbtn = new System.Windows.Forms.ToolBarButton();
            this.shearleftbtn = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton5 = new System.Windows.Forms.ToolBarButton();
            this.resetbtn = new System.Windows.Forms.ToolBarButton();
            this.exitbtn = new System.Windows.Forms.ToolBarButton();
            this.SuspendLayout();
            // 
            // tbimages
            // 
            this.tbimages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tbimages.ImageStream")));
            this.tbimages.TransparentColor = System.Drawing.Color.Transparent;
            this.tbimages.Images.SetKeyName(0, "");
            this.tbimages.Images.SetKeyName(1, "");
            this.tbimages.Images.SetKeyName(2, "");
            this.tbimages.Images.SetKeyName(3, "");
            this.tbimages.Images.SetKeyName(4, "");
            this.tbimages.Images.SetKeyName(5, "");
            this.tbimages.Images.SetKeyName(6, "");
            this.tbimages.Images.SetKeyName(7, "");
            this.tbimages.Images.SetKeyName(8, "");
            this.tbimages.Images.SetKeyName(9, "");
            this.tbimages.Images.SetKeyName(10, "");
            this.tbimages.Images.SetKeyName(11, "");
            this.tbimages.Images.SetKeyName(12, "");
            this.tbimages.Images.SetKeyName(13, "");
            this.tbimages.Images.SetKeyName(14, "");
            this.tbimages.Images.SetKeyName(15, "");
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.transleftbtn,
            this.transrightbtn,
            this.transupbtn,
            this.transdownbtn,
            this.toolBarButton1,
            this.scaleupbtn,
            this.scaledownbtn,
            this.toolBarButton2,
            this.rotxby1btn,
            this.rotyby1btn,
            this.rotzby1btn,
            this.toolBarButton3,
            this.rotxbtn,
            this.rotybtn,
            this.rotzbtn,
            this.toolBarButton4,
            this.shearrightbtn,
            this.shearleftbtn,
            this.toolBarButton5,
            this.resetbtn,
            this.exitbtn});
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.tbimages;
            this.toolBar1.Location = new System.Drawing.Point(484, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(24, 306);
            this.toolBar1.TabIndex = 0;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // transleftbtn
            // 
            this.transleftbtn.ImageIndex = 1;
            this.transleftbtn.Name = "transleftbtn";
            this.transleftbtn.ToolTipText = "translate left";
            // 
            // transrightbtn
            // 
            this.transrightbtn.ImageIndex = 0;
            this.transrightbtn.Name = "transrightbtn";
            this.transrightbtn.ToolTipText = "translate right";
            // 
            // transupbtn
            // 
            this.transupbtn.ImageIndex = 2;
            this.transupbtn.Name = "transupbtn";
            this.transupbtn.ToolTipText = "translate up";
            // 
            // transdownbtn
            // 
            this.transdownbtn.ImageIndex = 3;
            this.transdownbtn.Name = "transdownbtn";
            this.transdownbtn.ToolTipText = "translate down";
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // scaleupbtn
            // 
            this.scaleupbtn.ImageIndex = 4;
            this.scaleupbtn.Name = "scaleupbtn";
            this.scaleupbtn.ToolTipText = "scale up";
            // 
            // scaledownbtn
            // 
            this.scaledownbtn.ImageIndex = 5;
            this.scaledownbtn.Name = "scaledownbtn";
            this.scaledownbtn.ToolTipText = "scale down";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // rotxby1btn
            // 
            this.rotxby1btn.ImageIndex = 6;
            this.rotxby1btn.Name = "rotxby1btn";
            this.rotxby1btn.ToolTipText = "rotate about x by 1";
            // 
            // rotyby1btn
            // 
            this.rotyby1btn.ImageIndex = 7;
            this.rotyby1btn.Name = "rotyby1btn";
            this.rotyby1btn.ToolTipText = "rotate about y by 1";
            // 
            // rotzby1btn
            // 
            this.rotzby1btn.ImageIndex = 8;
            this.rotzby1btn.Name = "rotzby1btn";
            this.rotzby1btn.ToolTipText = "rotate about z by 1";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // rotxbtn
            // 
            this.rotxbtn.ImageIndex = 9;
            this.rotxbtn.Name = "rotxbtn";
            this.rotxbtn.ToolTipText = "rotate about x continuously";
            // 
            // rotybtn
            // 
            this.rotybtn.ImageIndex = 10;
            this.rotybtn.Name = "rotybtn";
            this.rotybtn.ToolTipText = "rotate about y continuously";
            // 
            // rotzbtn
            // 
            this.rotzbtn.ImageIndex = 11;
            this.rotzbtn.Name = "rotzbtn";
            this.rotzbtn.ToolTipText = "rotate about z continuously";
            // 
            // toolBarButton4
            // 
            this.toolBarButton4.Name = "toolBarButton4";
            this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // shearrightbtn
            // 
            this.shearrightbtn.ImageIndex = 12;
            this.shearrightbtn.Name = "shearrightbtn";
            this.shearrightbtn.ToolTipText = "shear right";
            // 
            // shearleftbtn
            // 
            this.shearleftbtn.ImageIndex = 13;
            this.shearleftbtn.Name = "shearleftbtn";
            this.shearleftbtn.ToolTipText = "shear left";
            // 
            // toolBarButton5
            // 
            this.toolBarButton5.Name = "toolBarButton5";
            this.toolBarButton5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // resetbtn
            // 
            this.resetbtn.ImageIndex = 14;
            this.resetbtn.Name = "resetbtn";
            this.resetbtn.ToolTipText = "restore the initial image";
            // 
            // exitbtn
            // 
            this.exitbtn.ImageIndex = 15;
            this.exitbtn.Name = "exitbtn";
            this.exitbtn.ToolTipText = "exit the program";
            // 
            // Transformer
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(508, 306);
            this.Controls.Add(this.toolBar1);
            this.Name = "Transformer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Transformer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Transformer());
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            Graphics grfx = pea.Graphics;
            Pen pen = new Pen(Color.White, 3);
            double temp;
            int k;

            if (gooddata)
            {
                //create the screen coordinates:
                // scrnpts = vertices*ctrans

                for (int i = 0; i < numpts; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        temp = 0.0d;
                        for (k = 0; k < 4; k++)
                            temp += vertices[i, k] * ctrans[k, j];
                        scrnpts[i, j] = temp;
                    }
                }


                //now draw the lines

                for (int i = 0; i < numlines; i++)
                {
                    grfx.DrawLine(pen, (int)scrnpts[lines[i, 0], 0], (int)scrnpts[lines[i, 0], 1],
                        (int)scrnpts[lines[i, 1], 0], (int)scrnpts[lines[i, 1], 1]);
                }


            } // end of gooddata block	
        } // end of OnPaint

        void MenuNewDataOnClick(object obj, EventArgs ea)
        {
            //MessageBox.Show("New Data item clicked.");
            gooddata = GetNewData();
            RestoreInitialImage();
        }

        void MenuFileExitOnClick(object obj, EventArgs ea)
        {
            Close();
        }

        void MenuAboutOnClick(object obj, EventArgs ea)
        {
            AboutDialogBox dlg = new AboutDialogBox();
            dlg.ShowDialog();
        }

        void RestoreInitialImage()
        {
            initializeVertices();
            Invalidate();
        } // end of RestoreInitialImage

        bool GetNewData()
        {
            string strinputfile, text;
            ArrayList coorddata = new ArrayList();
            ArrayList linesdata = new ArrayList();
            OpenFileDialog opendlg = new OpenFileDialog();
            opendlg.Title = "Choose File with Coordinates of Vertices";
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                strinputfile = opendlg.FileName;
                FileInfo coordfile = new FileInfo(strinputfile);
                StreamReader reader = coordfile.OpenText();
                do
                {
                    text = reader.ReadLine();
                    if (text != null) coorddata.Add(text);
                } while (text != null);
                reader.Close();
                DecodeCoords(coorddata);
            }
            else
            {
                MessageBox.Show("***Failed to Open Coordinates File***");
                return false;
            }

            opendlg.Title = "Choose File with Data Specifying Lines";
            if (opendlg.ShowDialog() == DialogResult.OK)
            {
                strinputfile = opendlg.FileName;
                FileInfo linesfile = new FileInfo(strinputfile);
                StreamReader reader = linesfile.OpenText();
                do
                {
                    text = reader.ReadLine();
                    if (text != null) linesdata.Add(text);
                } while (text != null);
                reader.Close();
                DecodeLines(linesdata);
            }
            else
            {
                MessageBox.Show("***Failed to Open Line Data File***");
                return false;
            }

            initializeVertices();
            return true;
        } // end of GetNewData

        void DecodeCoords(ArrayList coorddata)
        {
            //this may allocate slightly more rows that necessary
            vertices = new double[coorddata.Count, 4];
            numpts = 0;
            string[] text = null;
            for (int i = 0; i < coorddata.Count; i++)
            {
                text = coorddata[i].ToString().Split(' ', ',');
                vertices[numpts, 0] = double.Parse(text[0]);
                if (vertices[numpts, 0] < 0.0d) break;
                vertices[numpts, 1] = double.Parse(text[1]);
                vertices[numpts, 2] = double.Parse(text[2]);
                vertices[numpts, 3] = 1.0d;
                numpts++;
            }

        }// end of DecodeCoords

        void DecodeLines(ArrayList linesdata)
        {
            //this may allocate slightly more rows that necessary
            lines = new int[linesdata.Count, 2];
            numlines = 0;
            string[] text = null;
            for (int i = 0; i < linesdata.Count; i++)
            {
                text = linesdata[i].ToString().Split(' ', ',');
                lines[numlines, 0] = int.Parse(text[0]);
                if (lines[numlines, 0] < 0) break;
                lines[numlines, 1] = int.Parse(text[1]);
                numlines++;
            }
        } // end of DecodeLines

        void setIdentity(double[,] A, int nrow, int ncol)
        {
            for (int i = 0; i < nrow; i++)
            {
                for (int j = 0; j < ncol; j++) A[i, j] = 0.0d;
                A[i, i] = 1.0d;
            }
        }// end of setIdentity

        void initializeVertices()
        {
            scrnpts = new double[numpts, 4];
            setIdentity(ctrans, 4, 4);  //initialize transformation matrix to identity

            int width = (ClientRectangle.Width - toolBar1.ClientRectangle.Width) / 20;
            int height = ClientRectangle.Height / 20;

            // Get the center of the vertices
            double vertexWidthMin = double.MaxValue, vertexWidthMax = double.MinValue, vertexWidth;
            for (int i = 0; i < 4; i++)
            {
                if (vertices[i, 0] < vertexWidthMin)
                    vertexWidthMin = vertices[i, 0];
                if (vertices[i, 0] > vertexWidthMax)
                    vertexWidthMax = vertices[1, 0];
            }

            vertexWidth = (vertexWidthMax - vertexWidthMin) / 2 + vertexWidthMin;

            double vertexHeightMin = double.MaxValue, vertexHeightMax = double.MinValue, vertexHeight;
            for (int i = 0; i < 4; i++)
            {
                if (vertices[i, 1] < vertexHeightMin)
                    vertexHeightMin = vertices[i, 1];
                if (vertices[i, 1] > vertexHeightMax)
                    vertexHeightMax = vertices[1, 1];
            }

            vertexHeight = (vertexHeightMax - vertexHeightMin) / 2 + vertexHeightMin;

            // Translate to origin
            matrixTranslate(ctrans, -vertices[0, 0], -vertices[0, 1]);

            // Scale to half the window's height
            matrixScale(ctrans, height/2, height/2, height/2);

            // Reflect on the X-Axis
            matrixReflection(ctrans, false, true, false);

            // Translate to middle of the screen
            matrixTranslate(ctrans, ClientRectangle.Width/2, ClientRectangle.Height/2);
        }

        /// <summary>
        /// Perform a Translation Transformation on a Matrix
        /// The assumption is that we are using 4x4 matrices
        /// </summary>
        /// <param name="a">The target matrix</param>
        /// <param name="row">the number of rows</param>
        /// <param name="col">the number of cols</param>
        /// <param name="x">How much to translate along x</param>
        /// <param name="y">how much to translate along y</param>
        /// <param name="z">How much to translate along z</param>
        void matrixTranslate(double[,] a, double x, double y = 0, double z = 0)
        {
            double[,] translationMatrix = new double[4, 4];

            setIdentity(translationMatrix, 4, 4);

            translationMatrix[3, 0] = x;
            translationMatrix[3, 1] = y;
            translationMatrix[3, 2] = z;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double temp = 0.0d;
                    for (int k = 0; k < 4; k++)
                        temp += a[i, k] * translationMatrix[k, j];

                    a[i, j] = temp;
                }
            }
        }

        /// <summary>
        /// Performs a Scale Transformation on a Matrix
        /// The assumption is that we are using 4x4 matrices
        /// </summary>
        /// <param name="a">The target matrix</param>
        /// <param name="xFactor">How much to scale x by</param>
        /// <param name="yFactor">How much to scale y by</param>
        /// <param name="zFactor">How much to scale z by</param>
        void matrixScale(double[,] a, double xFactor, double yFactor = 1, double zFactor = 1)
        {
            double[,] scaleMatrix = new double[4, 4];

            setIdentity(scaleMatrix, 4, 4);

            scaleMatrix[0, 0] = xFactor;
            scaleMatrix[1, 1] = yFactor;
            scaleMatrix[2, 2] = zFactor;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double temp = 0.0d;
                    for (int k = 0; k < 4; k++)
                        temp += a[i, k] * scaleMatrix[k, j];

                    a[i, j] = temp;
                }
            }
        }

        /// <summary>
        /// Performs a Reflection Transformation on a Matrix
        /// Assumption is that we are using 4x4 matrices
        /// </summary>
        /// <param name="a">The target matrix</param>
        /// <param name="x">Are we reflecting X</param>
        /// <param name="y">Are we reflecting Y</param>
        /// <param name="z">Are we reflecting Z</param>
        void matrixReflection(double[,] a, bool x, bool y = false, bool z = false)
        {
            double[,] reflectionMatrix = new double[4, 4];

            setIdentity(reflectionMatrix, 4, 4);

            if (x)
                reflectionMatrix[0, 0] = -1;
            if (y)
                reflectionMatrix[1, 1] = -1;
            if (z)
                reflectionMatrix[2, 2] = -1;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double temp = 0.0d;
                    for (int k = 0; k < 4; k++)
                        temp += a[i, k] * reflectionMatrix[k, j];

                    a[i, j] = temp;
                }
            }
        }

        void matrixShear(double[,] a, bool right = false)
        {

            double[,] shearMatrix = new double[4, 4];

            setIdentity(shearMatrix, 4, 4);


            if (right)
                shearMatrix[1, 0] = -0.10d;
            else
                shearMatrix[1, 0] = 0.10d;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double temp = 0.0d;
                    for (int k = 0; k < 4; k++)
                        temp += a[i, k] * shearMatrix[k, j];

                    a[i, j] = temp;
                }
            }
        }

        void matrixRotation(double[,] a, double angle, bool x = true, bool y = false, bool z = false)
        {
            double[,] rotationMatrix = new double[4, 4];

            setIdentity(rotationMatrix, 4, 4);

            double sin = Math.Round(Math.Sin(angle),2);
            double cos = Math.Round(Math.Cos(angle),2);

            if (x)
            {
                rotationMatrix[1, 1] = cos;
                rotationMatrix[1, 2] = sin;
                rotationMatrix[2, 1] = -sin;
                rotationMatrix[2, 2] = cos;
            }
            else if (y)
            {
                rotationMatrix[0, 0] = cos;
                rotationMatrix[0, 2] = sin;
                rotationMatrix[2, 0] = -sin;
                rotationMatrix[2, 2] = cos;
            }
            else if (z)
            {
                rotationMatrix[0, 0] = cos;
                rotationMatrix[0, 1] = sin;
                rotationMatrix[1, 0] = -sin;
                rotationMatrix[1, 1] = cos;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double temp = 0.0d;
                    for (int k = 0; k < 4; k++)
                        temp += a[i, k] * rotationMatrix[k, j];

                    a[i, j] = temp;
                }
            }
        }


        private void Transformer_Load(object sender, System.EventArgs e)
        {

        }

        void rotatingThread(bool x = true, bool y = false, bool z = false)
        {
            while (!abort)
            {
                setRotation(x, y, z);
            }
        }

        void setRotation(bool x, bool y, bool z)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new constantRotation(setRotation), new object[] { x, y, z });
                    return;
                }

                if (abort)
                    return;

                matrixTranslate(ctrans, -scrnpts[0, 0], -scrnpts[0, 1], -scrnpts[0,2]);
                matrixRotation(ctrans, 0.05f, x, y, z);
                matrixTranslate(ctrans, scrnpts[0, 0], scrnpts[0, 1], scrnpts[0,2]);
                Refresh();
                Application.DoEvents();
            }
            catch (Exception e)
            {

            }
        }

        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (threadRunning)
            {
                abort = true;
            }

            if (worker != null)
                worker.Abort();

            // Wait for worker thread to terminate before executing further instructions
            Thread.Sleep(0);


            if (e.Button == transleftbtn)
            {
                matrixTranslate(ctrans, -75, 0, 0);
                Refresh();
            }
            if (e.Button == transrightbtn)
            {
                matrixTranslate(ctrans, 75, 0, 0);
                Refresh();
            }
            if (e.Button == transupbtn)
            {
                matrixTranslate(ctrans, 0, -35, 0);
                Refresh();
            }

            if (e.Button == transdownbtn)
            {
                matrixTranslate(ctrans, 0, 35, 0);
                Refresh();
            }
            if (e.Button == scaleupbtn)
            {
                matrixTranslate(ctrans, -scrnpts[0, 0], -scrnpts[0, 1]);
                matrixScale(ctrans, 1.1, 1.1, 1.1);
                matrixTranslate(ctrans, scrnpts[0, 0], scrnpts[0, 1]);
                Refresh();
            }
            if (e.Button == scaledownbtn)
            {
                matrixTranslate(ctrans, -scrnpts[0, 0], -scrnpts[0, 1]);
                matrixScale(ctrans, 0.9, 0.9, 0.9);
                matrixTranslate(ctrans, scrnpts[0, 0], scrnpts[0, 1]);
                Refresh();
            }
            if (e.Button == rotxby1btn)
            {
                matrixTranslate(ctrans, 0, -scrnpts[0, 1], -scrnpts[0,2]);
                matrixRotation(ctrans, 0.05);
                matrixTranslate(ctrans, 0, scrnpts[0, 1], scrnpts[0,2]);
                Refresh();
            }
            if (e.Button == rotyby1btn)
            {
                int height = ClientRectangle.Height / 20;
                height = height / 2;

                matrixTranslate(ctrans, -scrnpts[0, 0], 0, -scrnpts[0,2]);
                matrixRotation(ctrans, 0.05, false, true);
                matrixTranslate(ctrans, scrnpts[0, 0], 0, scrnpts[0,2]);
                Refresh();
            }
            if (e.Button == rotzby1btn)
            {
                matrixTranslate(ctrans, -scrnpts[0, 0], -scrnpts[0, 1]);
                matrixRotation(ctrans, 0.05f, false, false, true);
                matrixTranslate(ctrans, scrnpts[0, 0], scrnpts[0, 1]);
                Refresh();
            }

            if (e.Button == rotxbtn)
            {
                worker = new Thread(() => rotatingThread());

                abort = false;
                worker.Start();
                threadRunning = true;
            }
            if (e.Button == rotybtn)
            {
                worker = new Thread(() => rotatingThread(false, true));

                abort = false;
                worker.Start();
                threadRunning = true;
            }

            if (e.Button == rotzbtn)
            {
                worker = new Thread(() => rotatingThread(false, false, true));

                abort = false;
                worker.Start();
                threadRunning = false;
            }

            if (e.Button == shearleftbtn)
            {
                double baselineX = 0;
                double baselineY = double.MinValue;

                for(int i = 0; i < scrnpts.GetLength(0); i++)
                {
                    if (scrnpts[i, 1] > baselineY)
                    {
                        baselineX = scrnpts[i, 0];
                        baselineY = scrnpts[i, 1];
                    }
                }

                matrixTranslate(ctrans, -baselineX, -baselineY);
                matrixShear(ctrans);
                matrixTranslate(ctrans, baselineX, baselineY);

                Console.WriteLine("Baseline Y: " + baselineY);

                Refresh();
            }

            if (e.Button == shearrightbtn)
            {
                double baselineX = 0;
                double baselineY = double.MinValue;

                for (int i = 0; i < scrnpts.GetLength(0); i++)
                {
                    if (scrnpts[i, 1] > baselineY)
                    {
                        baselineX = scrnpts[i, 0];
                        baselineY = scrnpts[i, 1];
                    }
                }

                Console.WriteLine("Baseline Y: " + baselineY);

                matrixTranslate(ctrans, -baselineX, -baselineY);
                matrixShear(ctrans, true);
                matrixTranslate(ctrans, baselineX, baselineY);

                Refresh();
            }

            if (e.Button == resetbtn)
            {
                RestoreInitialImage();
            }

            if (e.Button == exitbtn)
            {
                Close();
            }

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            abort = true;
            base.OnFormClosing(e);
        }

        // End of Function	
    }
}
