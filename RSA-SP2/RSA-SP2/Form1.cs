using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace RSA_SP2
{
    public partial class Form1 : Form
    {
        private BigInteger modulus;

        public Form1()
        {
            InitializeComponent();
            Server.MessageReceived += Server_MessageReceived;
            Server.StartServer(8080);
            btnValidate.Click += BtnValidate_Click;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Server_MessageReceived(string message)
        {
            Invoke((MethodInvoker)delegate {
                var parts = message.Split('|');
                if (parts.Length == 4)
                {
                    txtPublicKey.Text = parts[0].Split(':')[1].Trim();
                    modulus = BigInteger.Parse(parts[1].Split(':')[1].Trim());
                    txtMessage.Text = parts[2].Split(':')[1].Trim();
                    txtSignature.Text = parts[3].Split(':')[1].Trim();
                }
            });
        }

        private void BtnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                string message = txtMessage.Text;
                string signature = txtSignature.Text;
                BigInteger publicKey = BigInteger.Parse(txtPublicKey.Text);

                bool isValid = RSA.ValidateSignature(message, signature, publicKey, modulus);
                MessageBox.Show(isValid ? "Signature is valid." : "Signature is invalid.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating signature: {ex.Message}");
            }
        }
    }
}
