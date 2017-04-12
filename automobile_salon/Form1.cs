using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Animations;
using MaterialSkin.Controls;
using MaterialSkin;

namespace automobile_salon
{
    public partial class main_menu : MaterialForm
    {
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        System.Windows.Forms.Timer timer;

        public main_menu()
        {
            InitializeComponent();
            initMain_menu();
             var skinManager = MaterialSkinManager.Instance;
          // skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            skinManager.ColorScheme = new ColorScheme(Primary.LightGreen700,Primary.LightGreen900,Primary.LightGreen100,Accent.Amber700,TextShade.BLACK);
           


        }

        public void initMain_menu()
        {
            StartPosition = FormStartPosition.CenterScreen;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Текущая дата и время:";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            strip_menu.Items.Add(infoLabel);
            strip_menu.Items.Add(dateLabel);
            strip_menu.Items.Add(timeLabel);
            timer = new System.Windows.Forms.Timer() { Interval = 1000 };
            timer.Tick += timerDatetime_Tick;
            timer.Start();


        }

        void timerDatetime_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

   
        private void conect_Click(object sender, EventArgs e)
        {
            Form sForm = new wolkswagen();
            sForm.Show();
            this.Hide();
        }
    }

}
