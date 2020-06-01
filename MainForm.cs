using System.Windows.Forms;

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
