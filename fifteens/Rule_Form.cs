using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace fifteens
{
    public partial class Rule_Form : Form
    {
        Form1 form1;
        public Rule_Form(Form1 form)
        {
            InitializeComponent();
            form1 = form;
        }

        private void Rule_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.timer1.Enabled = form1.isGameStart;
        }
    }
}
