﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIVClientGUI
{
    public partial class AddServerForm : Form
    {
        public AddServerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress o;
            if (textBox2.TextLength > 0 && textBox3.TextLength > 0 && char.IsNumber(textBox3.Text, 0) && System.Net.IPAddress.TryParse(textBox2.Text, out o))
            {
                System.IO.File.AppendAllLines("servers.list", new string[1]{
                    textBox2.Text + ":" + textBox3.Text
                });
                this.Close();
            }
        }

        private void AddServerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
