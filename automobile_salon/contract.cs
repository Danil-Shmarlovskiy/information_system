using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace automobile_salon
{
    public partial class contract : Form
    {
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        System.Windows.Forms.Timer timer;
        public contract()
        {
            InitializeComponent();
            initFormContract();
        }

        public void initFormContract()
        {
            StartPosition = FormStartPosition.CenterParent;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
            type_payment_box.Text = "Не указана";
        }

        void timerDatetime_Tick(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        /*очистка полей*/
        private void clear_Click(object sender, EventArgs e)
        {

        }

        private void create_contract_skoda()
        {
            
        }

        //добавление 
        private void add_contract_skoda_Click(object sender, EventArgs e)
        {
            if (type_payment_box.Text == "")
            {

            }
        }
    }



}
