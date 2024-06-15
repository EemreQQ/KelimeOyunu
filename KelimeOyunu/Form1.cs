using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace KelimeOyunu
{
    public partial class Form1 : Form
    {
        string s = null;
        char[,] Matrix = new char[12, 12];

        Random r = new Random();
        List<Button> buttonsList = new List<Button>();
        SortedDictionary<char, int> Harfadet = new SortedDictionary<char, int>();
        char c = ' ';
        string rndmKelime = "";
        int rndadet = 0;
        bool IsTrue = false;
        int buttonSayisi = 0;

        // Kelime listesi
        List<string> istenenKelimeler = new List<string>();
        // İstenen kelimelerden rastgele tutulan isim/kelime random harflerimle oluşturuluyor mu?
        Dictionary<char, int> RndHarfadet = new Dictionary<char, int>();

        public Form1()
        {
            InitializeComponent();

            StreamReader streamReader = new StreamReader("C:\\Users\\emre_\\source\\repos\\KelimeOyunu\\KelimeOyunu\\isimler.txt");
            while (!streamReader.EndOfStream)
            {
                string str = streamReader.ReadLine();
                if (!istenenKelimeler.Contains(str)) istenenKelimeler.Add(str);
            }

            string harfler = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";

            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    // Random bir harf tut
                    c = harfler[r.Next(0, harfler.Length)];
                    Matrix[i, j] = c;


                    if (Harfadet.ContainsKey(c))
                    {
                        if (c.ToString() == "Ğ" && Harfadet['Ğ'] >= 2) // 2 den fazla yumuşak g'ye gerek yok
                        {
                            j--; continue;
                        }
                        Harfadet[c]++;// O harften var mı? Evet,adedini arttır.
                    }
                    else Harfadet.Add(c, 1); // Hayır yok,ekle.

                    Button b = new Button(); // Yeni Buton oluştur
                    b.Text = c.ToString(); //
                    b.Size = new Size(32, 32);
                    b.Location = new Point((j + 1) * 32, (i) * 32);
                    b.BackColor = Color.Tan;
                    b.MouseDown += Button_MouseDown;
                    b.Font = new Font("Arial", 14 );
                    this.Controls.Add(b);
                }
            }


            for (int i = 0; listBox1.Items.Count < 8; i++)
            {
                rndmKelime = istenenKelimeler[r.Next(0, istenenKelimeler.Count)];
                // Rastgele kelime seçildi.
                for (int j = 0; j < rndmKelime.Length; j++)
                {
                    if (!RndHarfadet.ContainsKey(rndmKelime[j]))
                        RndHarfadet.Add(rndmKelime[j], 1);
                    else RndHarfadet[rndmKelime[j]]++;
                }
                // Rastgele seçilen kelimenin harfleri sayıldı

                // Bu kadar harf var mı diye iki Dictionarydeki değerlere bakıyoruz..
                foreach (var item in RndHarfadet)
                {
                    rndadet = item.Value;
                    IsTrue = true;
                    if (!Harfadet.ContainsKey(item.Key) || Harfadet[item.Key] <= rndadet)
                    {
                        IsTrue = false;
                        RndHarfadet.Clear();
                        break;
                    }
                }
                foreach (var ch in RndHarfadet)
                {
                    Harfadet[ch.Key] -= ch.Value;
                }

                if (IsTrue && !listBox1.Items.Contains(rndmKelime)) { listBox1.Items.Add(rndmKelime.ToString()); }
                RndHarfadet.Clear();
            }

            void Button_MouseDown(object sender, MouseEventArgs e)
            {
                Button button = sender as Button;
                if (button.BackColor == Color.Yellow)
                {
                    button.BackColor = SystemColors.Control;
                    s = s.Remove(buttonsList.Count - 1);
                    buttonsList.Remove(button);
                }
                else
                {
                    buttonsList.Add(button);
                    s += buttonsList[buttonsList.Count - 1].Text.ToString();
                    button.BackColor = Color.Yellow;
                }
                DogruKelime();

            }

            bool DogruKelime()
            {
                if (listBox1.Items.Contains(s)) // Eklenilen harfle birlikte mantıklı bir kelime oluştu mu?                   
                {
                    MessageBox.Show("Bir kelime buldunuz");
                    listBox1.Items.RemoveAt(listBox1.Items.IndexOf(s));
                    if (listBox1.Items.Count == 0) MessageBox.Show("Tebrikler,daha fazla bulunacak kelime yok.");
                    s = null;

                    for (int j = 0; j < buttonsList.Count; j++)
                    {
                        buttonsList[j].MouseDown -= Button_MouseDown;
                        buttonsList[j].BackColor = Color.Gray;
                    }
                    buttonsList.Clear();
                    return true;
                }
                return false;
            }
        }
    }
}
