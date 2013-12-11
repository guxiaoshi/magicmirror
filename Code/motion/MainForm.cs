

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using VideoSource;
using Tiger.Video.VFW;

namespace motion
{
  
    public class MainForm : System.Windows.Forms.Form
    {

        private const int statLength = 15;
        private int statIndex = 0, statReady = 0;
        private int[] statCount = new int[statLength];

        private IMotionDetector detector = new MotionDetector3Optimized( );
        private int detectorType = 4;
        private int intervalsToSave = 0;

        private AVIWriter writer = null;
        private bool saveOnMotion = false;

        private System.Windows.Forms.MenuItem fileItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitFileItem;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Timers.Timer timer;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel fpsPanel;
        private System.Windows.Forms.Panel panel;
        private motion.CameraWindow cameraWindow;
        private System.Windows.Forms.MenuItem motionItem;
        private System.Windows.Forms.MenuItem noneMotionItem;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem detector1MotionItem;
        private System.Windows.Forms.MenuItem detector2MotionItem;
        private System.Windows.Forms.MenuItem detector3MotionItem;
        private System.Windows.Forms.MenuItem detector3OptimizedMotionItem;
        private System.Windows.Forms.MenuItem openLocalFileItem;
        private System.Windows.Forms.MenuItem detector4MotionItem;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem motionAlarmItem;
        private IContainer components;

        public MainForm( )
        {
          
            InitializeComponent( );

         
        }


        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( components != null )
                {
                    components.Dispose( );
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileItem = new System.Windows.Forms.MenuItem();
            this.openLocalFileItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.exitFileItem = new System.Windows.Forms.MenuItem();
            this.motionItem = new System.Windows.Forms.MenuItem();
            this.noneMotionItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.detector1MotionItem = new System.Windows.Forms.MenuItem();
            this.detector2MotionItem = new System.Windows.Forms.MenuItem();
            this.detector3MotionItem = new System.Windows.Forms.MenuItem();
            this.detector3OptimizedMotionItem = new System.Windows.Forms.MenuItem();
            this.detector4MotionItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.motionAlarmItem = new System.Windows.Forms.MenuItem();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.timer = new System.Timers.Timer();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.fpsPanel = new System.Windows.Forms.StatusBarPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.cameraWindow = new motion.CameraWindow();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileItem,
            this.motionItem});
            // 
            // fileItem
            // 
            this.fileItem.Index = 0;
            this.fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openLocalFileItem,
            this.menuItem1,
            this.exitFileItem});
            this.fileItem.Text = "设备";
            // 
            // openLocalFileItem
            // 
            this.openLocalFileItem.Index = 0;
            this.openLocalFileItem.Text = "本地探头";
            this.openLocalFileItem.Click += new System.EventHandler(this.openLocalFileItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // exitFileItem
            // 
            this.exitFileItem.Index = 2;
            this.exitFileItem.Text = "退出";
            this.exitFileItem.Click += new System.EventHandler(this.exitFileItem_Click);
            // 
            // motionItem
            // 
            this.motionItem.Index = 1;
            this.motionItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.noneMotionItem,
            this.menuItem2,
            this.detector1MotionItem,
            this.detector2MotionItem,
            this.detector3MotionItem,
            this.detector3OptimizedMotionItem,
            this.detector4MotionItem,
            this.menuItem3,
            this.motionAlarmItem});
            this.motionItem.Text = "动作";
            this.motionItem.Popup += new System.EventHandler(this.motionItem_Popup);
            // 
            // noneMotionItem
            // 
            this.noneMotionItem.Index = 0;
            this.noneMotionItem.Text = "无";
            this.noneMotionItem.Click += new System.EventHandler(this.noneMotionItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.Text = "-";
            // 
            // detector1MotionItem
            // 
            this.detector1MotionItem.Index = 2;
            this.detector1MotionItem.Text = "D1";
            this.detector1MotionItem.Click += new System.EventHandler(this.detector1MotionItem_Click);
            // 
            // detector2MotionItem
            // 
            this.detector2MotionItem.Index = 3;
            this.detector2MotionItem.Text = "D2";
            this.detector2MotionItem.Click += new System.EventHandler(this.detector2MotionItem_Click);
            // 
            // detector3MotionItem
            // 
            this.detector3MotionItem.Index = 4;
            this.detector3MotionItem.Text = "D3";
            this.detector3MotionItem.Click += new System.EventHandler(this.detector3MotionItem_Click);
            // 
            // detector3OptimizedMotionItem
            // 
            this.detector3OptimizedMotionItem.Index = 5;
            this.detector3OptimizedMotionItem.Text = "D3 - 优化";
            this.detector3OptimizedMotionItem.Click += new System.EventHandler(this.detector3OptimizedMotionItem_Click);
            // 
            // detector4MotionItem
            // 
            this.detector4MotionItem.Index = 6;
            this.detector4MotionItem.Text = "D4";
            this.detector4MotionItem.Click += new System.EventHandler(this.detector4MotionItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 7;
            this.menuItem3.Text = "-";
            // 
            // motionAlarmItem
            // 
            this.motionAlarmItem.Checked = true;
            this.motionAlarmItem.Index = 8;
            this.motionAlarmItem.Text = "提示";
            this.motionAlarmItem.Click += new System.EventHandler(this.motionAlarmItem_Click);

     
            // 
            // timer
            // 
            this.timer.Interval = 1000D;
            this.timer.SynchronizingObject = this;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 195);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.fpsPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(408, 24);
            this.statusBar.TabIndex = 1;
            // 
            // fpsPanel
            // 
            this.fpsPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.fpsPanel.Name = "fpsPanel";
            this.fpsPanel.Width = 391;
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Controls.Add(this.cameraWindow);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(408, 195);
            this.panel.TabIndex = 2;
            // 
            // cameraWindow
            // 
            this.cameraWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.cameraWindow.Camera = null;
            this.cameraWindow.Location = new System.Drawing.Point(49, 41);
            this.cameraWindow.Name = "cameraWindow";
            this.cameraWindow.Size = new System.Drawing.Size(387, 261);
            this.cameraWindow.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(408, 219);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.statusBar);
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "测试";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).EndInit();
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( )
        {
            Application.Run( new MainForm( ) );
        }

   
        private void MainForm_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            CloseFile( );
        }

     
        private void exitFileItem_Click( object sender, System.EventArgs e )
        {
            this.Close( );
        }

       

     


    
     
        private void openLocalFileItem_Click( object sender, System.EventArgs e )
        {
            CaptureDeviceForm form = new CaptureDeviceForm( );

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                
                CaptureDevice localSource = new CaptureDevice( );
                localSource.VideoSource = form.Device;

               
                OpenVideoSource( localSource );
            }
        }

       
        private void OpenVideoSource( IVideoSource source )
        {
          
            this.Cursor = Cursors.WaitCursor;

           
            CloseFile( );

      
            if ( detector != null )
            {
                detector.MotionLevelCalculation = motionAlarmItem.Checked;
            }

            
            Camera camera = new Camera( source, detector );
            
            camera.Start( );

            
            cameraWindow.Camera = camera;

          
            statIndex = statReady = 0;

           
            camera.NewFrame += new EventHandler( camera_NewFrame );
            camera.Alarm += new EventHandler( camera_Alarm );

           
            timer.Start( );

            this.Cursor = Cursors.Default;
        }

        
        private void CloseFile( )
        {
            Camera camera = cameraWindow.Camera;

            if ( camera != null )
            {
              
                cameraWindow.Camera = null;

                camera.SignalToStop( );
          
                camera.WaitForStop( );

                camera = null;

                if ( detector != null )
                    detector.Reset( );
            }

            if ( writer != null )
            {
                writer.Dispose( );
                writer = null;
            }
            intervalsToSave = 0;
        }

      
        private void timer_Elapsed( object sender, System.Timers.ElapsedEventArgs e )
        {
            Camera camera = cameraWindow.Camera;

            if ( camera != null )
            {
              
                statCount[statIndex] = camera.FramesReceived;

               
                if ( ++statIndex >= statLength )
                    statIndex = 0;
                if ( statReady < statLength )
                    statReady++;

                float fps = 0;

            
                for ( int i = 0; i < statReady; i++ )
                {
                    fps += statCount[i];
                }
                fps /= statReady;

                statCount[statIndex] = 0;

                fpsPanel.Text = fps.ToString( "F2" ) + " fps";
            }

        
            if ( intervalsToSave > 0 )
            {
                if ( ( --intervalsToSave == 0 ) && ( writer != null ) )
                {
                    writer.Dispose( );
                    writer = null;
                }
            }
        }

      
        private void noneMotionItem_Click( object sender, System.EventArgs e )
        {
            detector = null;
            detectorType = 0;
            SetMotionDetector( );
        }

        // Select detector 1
        private void detector1MotionItem_Click( object sender, System.EventArgs e )
        {
            detector = new MotionDetector1( );
            detectorType = 1;
            SetMotionDetector( );
        }

        // Select detector 2
        private void detector2MotionItem_Click( object sender, System.EventArgs e )
        {
            detector = new MotionDetector2( );
            detectorType = 2;
            SetMotionDetector( );
        }

        // Select detector 3
        private void detector3MotionItem_Click( object sender, System.EventArgs e )
        {
            detector = new MotionDetector3( );
            detectorType = 3;
            SetMotionDetector( );
        }

        // Select detector 3 - 优化
        private void detector3OptimizedMotionItem_Click( object sender, System.EventArgs e )
        {
            detector = new MotionDetector3Optimized( );
            detectorType = 4;
            SetMotionDetector( );
        }

        // Select detector 4
        private void detector4MotionItem_Click( object sender, System.EventArgs e )
        {
            detector = new MotionDetector4( );
            detectorType = 5;
            SetMotionDetector( );
        }

       
        private void SetMotionDetector( )
        {
            Camera camera = cameraWindow.Camera;

          
            if ( detector != null )
            {
                detector.MotionLevelCalculation = motionAlarmItem.Checked;
            }

           
            if ( camera != null )
            {
                camera.Lock( );
                camera.MotionDetector = detector;

                  statIndex = statReady = 0;
                camera.Unlock( );
            }
        }

      
        private void motionItem_Popup( object sender, System.EventArgs e )
        {
            MenuItem[] items = new MenuItem[]
			{
				noneMotionItem, detector1MotionItem,
				detector2MotionItem, detector3MotionItem, detector3OptimizedMotionItem,
				detector4MotionItem
			};

            for ( int i = 0; i < items.Length; i++ )
            {
                items[i].Checked = ( i == detectorType );
            }
        }

      
        private void camera_Alarm( object sender, System.EventArgs e )
        {
            
            intervalsToSave = (int) ( 5 * ( 1000 / timer.Interval ) );
        }

      
        private void camera_NewFrame( object sender, System.EventArgs e )
        {
            if ( ( intervalsToSave != 0 ) && ( saveOnMotion == true ) )
            {
              
                if ( writer == null )
                {
                   
                    DateTime date = DateTime.Now;
                    String fileName = String.Format( "{0}-{1}-{2} {3}-{4}-{5}.avi",
                        date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second );

                    try
                    {
                       
                        writer = new AVIWriter( "wmv3" );
                        
                        writer.Open( fileName, cameraWindow.Camera.Width, cameraWindow.Camera.Height );
                    }
                    catch ( ApplicationException ex )
                    {
                        if ( writer != null )
                        {
                            writer.Dispose( );
                            writer = null;
                        }
                    }
                }

                
                Camera camera = cameraWindow.Camera;

                camera.Lock( );
                writer.AddFrame( camera.LastFrame );
                camera.Unlock( );
            }
        }


      
        private void motionAlarmItem_Click( object sender, System.EventArgs e )
        {
            motionAlarmItem.Checked = !motionAlarmItem.Checked;

            if ( detector != null )
            {
                detector.MotionLevelCalculation = motionAlarmItem.Checked;
            }
        }

        private void D1_Click(object sender, EventArgs e)
        {

        }

   
    }
}
