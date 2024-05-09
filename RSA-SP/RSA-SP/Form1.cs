using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace RSA_SP
{
    public partial class Form1 : Form
    {
        private BigInteger modulus;

        public Form1()
        {
            InitializeComponent();
            btnGenerateKeys.Click += BtnGenerateKeys_Click;
            btnEncrypt.Click += BtnEncrypt_Click;
            btnSend.Click += BtnSend_Click;
        }

        private void BtnGenerateKeys_Click(object sender, EventArgs e)
        {
            int bits = 512;
            (BigInteger publicKey, BigInteger privateKey, BigInteger mod) = RSA.GenerateKeys(bits);
            txtPublicKey.Text = publicKey.ToString();
            txtPrivateKey.Text = privateKey.ToString();
            modulus = mod;
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            if (BigInteger.TryParse(txtPublicKey.Text, out BigInteger publicKey))
            {
                var message = txtMessage.Text;
                var encryptedMessage = RSA.Encrypt(message, publicKey, modulus);
                txtEncryptedMessage.Text = encryptedMessage;
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPublicKey.Text) && !string.IsNullOrEmpty(txtMessage.Text) && !string.IsNullOrEmpty(txtEncryptedMessage.Text))
            {
                var formattedMessage = $"Public Key: {txtPublicKey.Text} | Modulus: {modulus} | Message: {txtMessage.Text} | Encrypted: {txtEncryptedMessage.Text}";
                Client.SendMessage(formattedMessage);
                MessageBox.Show("Issiusta i RSA-SP2!");
            }
            else
            {
                MessageBox.Show("Uzpildykite viska.");
            }
        }
    }
}
