using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPL.Application.Data;
using VPL.Threading.Modeler;

namespace MiqoCraft
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            MainUserControlV2 mainControl = new MainUserControlV2();
            mainControl.Dock = DockStyle.Fill;
            this.Controls.Add(mainControl);
        }
    }
}
