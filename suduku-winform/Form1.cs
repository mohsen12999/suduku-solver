using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace suduku_winform
{
    public partial class Form1 : Form
    {
        private TextBox[][] _textBoxs;
        private GroupBox[][] _groupBoxs;

        private List<int>[][] allPossibleList;
        private List<Tuple<int,int>> findList;
        public Form1()
        {
            InitializeComponent();
            MakeBtn();
        }


        private void clear_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    _textBoxs[i][j].Text = "";
                    _textBoxs[i][j].Enabled = true;
                }

            richTextBox1.Text = "";
            allPossibleList = new List<int>[9][];
            for (int i = 0; i < 9; i++)
            {
                allPossibleList[i]=new List<int>[9];
                for (int j = 0; j < 9; j++)
                {
                    allPossibleList[i][j] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
            }
            findList = new List<Tuple<int, int>>();
        }

        private void MakeBtn()
        {

            _textBoxs = new TextBox[9][];
            _groupBoxs = new GroupBox[3][];
            allPossibleList = new List<int>[9][];
            findList = new List<Tuple<int, int>>();

            this.SuspendLayout();

            for (int i = 0; i < 9; i++)
            {
                _textBoxs[i] = new TextBox[9];
                allPossibleList[i] = new List<int>[9];

                int gi = i / 3;
                if (i % 3 == 0)
                {
                    _groupBoxs[gi] = new GroupBox[3];
                }
                for (int j = 0; j < 9; j++)
                {
                    int gj = j / 3;
                    if (i % 3 == 0 && j % 3 == 0)
                    {
                        _groupBoxs[gi][gj] = new GroupBox
                        {
                            Location = new System.Drawing.Point(gi * 160 + 20, gj * 160 + 20),
                            Name = "groupBox" + (i * 3 + j).ToString(),
                            Size = new System.Drawing.Size(145, 145),
                            TabStop = false
                        };
                        this.Controls.Add(_groupBoxs[gi][gj]);
                    }

                    _textBoxs[i][j] = new TextBox
                    {
                        Text = "",
                        Size = new Size(30, 30),
                        TextAlign = HorizontalAlignment.Center,
                        Font = new Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular,
                            System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        Location = new Point((i % 3) * 40 + 15, (j % 3) * 40 + 20)
                    };

                    allPossibleList[i][j] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                    _groupBoxs[gi][gj].Controls.Add(_textBoxs[i][j]);

                    //this.Controls.Add(_textBoxs[i][j]);
                }

            }
        }


        private bool checkNumber = false;
        private void start_button_Click(object sender, EventArgs e)
        {
            //check cells
            if(!checkNumber)
                CheckNumberAndLockCell();
            //lock cells
            foreach (var findTuple in findList)
            {
                ApplyNumber(findTuple);
                //if allPossibleList[i][j].count()==1
            }

            bool isFind = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //richTextBox1.Text += allPossibleList[i][j].Count + "-";
                    if (allPossibleList[i][j].Count == 1)
                    {
                        richTextBox1.Text += "find one! i:"+i+",j:"+j+" -total:"+ findList.Count+"\n";
                        _textBoxs[i][j].Text = allPossibleList[i][j].First().ToString();
                        allPossibleList[i][j].Clear();//=new List<int>();
                        _textBoxs[i][j].Enabled = false;
                        findList.Add(new Tuple<int, int>(i, j));
                        isFind = true;
                    }
                }

                //richTextBox1.Text += "\n";
            }

            if (!isFind)
            {
                richTextBox1.Text += "can not find any thing \n";

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        richTextBox1.Text += allPossibleList[i][j].Count + "-";
                    }
                    richTextBox1.Text += "\n";
                }

                richTextBox1.Text += "\n" + findList.Count + "\n";
                foreach (var find in findList)
                {
                    richTextBox1.Text += "(" + find.Item1 + "," + find.Item2 + ") -";
                }
            }

            if (findList.Count == 81)
            {
                richTextBox1.Text += "------------------ \n Find All :)";
            }
        }

        private void CheckNumberAndLockCell()
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    var txt = _textBoxs[i][j].Text;
                    if (txt != "")
                        if (int.TryParse(txt, out int num))
                            if (num > 0 && num <= 9)
                            {
                                _textBoxs[i][j].Enabled = false;
                                allPossibleList[i][j].Clear(); //= new List<int>();
                                findList.Add(new Tuple<int, int>(i,j));
                                continue;
                            }

                    _textBoxs[i][j].Enabled = true;
                    _textBoxs[i][j].Text = "";
                }
        }

        private void ApplyNumber(Tuple<int,int> tuple)
        {
            int num = int.Parse(_textBoxs[tuple.Item1][tuple.Item2].Text);

            //row
            for (int i = 0; i < 9; i++)
            {
                if(i==tuple.Item1)
                    continue;
                allPossibleList[i][tuple.Item2].Remove(num);
            }
            //column
            for (int j = 0; j < 9; j++)
            {
                if(j==tuple.Item2)
                    continue;
                allPossibleList[tuple.Item1][j].Remove(num);
            }
            //check square
            int gi = tuple.Item1 / 3;
            int gj = tuple.Item2 / 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    allPossibleList[gi*3+i][gj*3+j].Remove(num);
                }
            }

        }

        //check row

        //check column

        //check square
    }
}
