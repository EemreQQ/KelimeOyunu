using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KelimeOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int start = 0;
            string s = null;
            char[,] Matrix = new char[10, 10];
            Random r = new Random();
            List<Button> buttonsList = new List<Button>();
            List<string> istenenKelimeler = new List<string>() { "emre", "tuba" };
            Dictionary<char, int> adetHarf = new Dictionary<char, int>();
            char c =' ';
            int count = 1;

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    c = (char)r.Next('A', 'Z' + 1); // Random bir harf tut
                    if (c == 'W' || c == 'Q' || c == 'X') { j--; continue; }
                    Matrix[i, j] = c;
                    
                    if (adetHarf.ContainsKey(c)) // O harften var mı? Evet,adedini arttır.
                        adetHarf[c]++; 
                    else adetHarf.Add(c, 1); // Hayır yok,ekle.
                    
                    Button b = new Button(); // Yeni Buton oluştur
                    b.Text = c.ToString(); //
                    b.Size = new Size(35, 35);
                    b.Location = new Point((j + 1) * 35, (i + 1) * 35);
                    b.MouseDown += Button_MouseDown;
                    this.Controls.Add(b);
                }
            }
            
            foreach (var item in adetHarf)
            {
                TreeNode t = new TreeNode(item.Key.ToString());
                t.Nodes.Add(new TreeNode(item.Value.ToString()));
                treeView1.Nodes.Add(t);
            }
            
            void Button_MouseDown(object sender, MouseEventArgs e)
            {
                Button button = sender as Button;
                if (button.BackColor == Color.Yellow)
                {
                    button.BackColor = SystemColors.Control;
                    buttonsList.Remove(button);
                }
                else 
                { 
                    buttonsList.Add(button);
                    button.BackColor = Color.Yellow;
                }
                DogruKelime();
            }   
            
            void DogruKelime()
            {
                if (buttonsList.Count > 1) start = buttonsList.Count - 1; 
                // Listenin uzunluğu 1'den büyük mü? Evet..
                // En son gelen harfi başlangıc değeri yap.  // E - M - R - E
                for (int i = start; i < buttonsList.Count; i++)
                {
                    s += buttonsList[i].Text.ToString().ToLower(); // en son gelen değeri s'ye ekle
                    if (istenenKelimeler.Contains(s)) // Eklenilen harfle birlikte mantıklı bir kelime oluştu mu?
                    {
                        MessageBox.Show("Bir kelime buldunuz");
                        buttonsList.Clear();
                        s = null;
                        start = 0;
                    }
                }
             
            }
        }
    }
}
