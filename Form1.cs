using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using XML.serializaton.Factories;
using XML.serializaton.Decorations;

namespace XML.serializaton
{
    public partial class Form1 : Form
    {
        Factory[] factory = new Factory[6];
        List<DecorationClass> DecorationList = new List<DecorationClass>();
        Type[] extraTypes = new Type[6];
        List<string> FieldList = new List<string>();
        List<TextBox> TextBoxList = new List<TextBox>();
        int i;
        bool flagEdit = false, flagDelete = false;

        public Form1()
        {
            InitializeComponent();
            InitializeFabrika();
            InitializaTypes();  
           
            TextBoxList.Add(textBox1);
            TextBoxList.Add(textBox2);
            TextBoxList.Add(textBox3);
            TextBoxList.Add(textBox4);
            TextBoxList.Add(textBox5);
        }
        void InitializeFabrika()
        {
            factory[0] = new EarringsFactory();
            factory[1] = new RingFactory();
            factory[2] = new ChainFactory();
            factory[3] = new CoulombFactory();
            factory[4] = new WatchesFactory();
            factory[5] = new PinFactory();
        }
        void InitializaTypes()
        {
            extraTypes[0] = typeof(Earrings);
            extraTypes[1] = typeof(Ring);
            extraTypes[2] = typeof(Chain);
            extraTypes[3] = typeof(Coulomb);
            extraTypes[4] = typeof(Watches);
            extraTypes[5] = typeof(Pin);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if ((DecorationList.Count > 0) /*&& (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)*/)
                {
                    //listBox1.Items.Clear();
                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<DecorationClass>), extraTypes);
                    //System.IO.StreamReader reader = new System.IO.StreamReader(openFileDialog1.FileName);

                    using (FileStream file = new FileStream("decoration.xml", FileMode.Create))
                    {
                        mySerializer.Serialize(file, DecorationList);
                        file.Close();
                    }
                    textBoxInfo.Text = "Serialization completed successfully.\r\n";
                }
                else
                    textBoxInfo.Text = "The list doesn't contain objects\r\n";
            }
            catch
            {
                textBoxInfo.Text = "Check the object(s) in list\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                FileStream fs = new FileStream("decoration.xml", FileMode.Open);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<DecorationClass>), extraTypes);

                List<DecorationClass> Decoration = (List<DecorationClass>)xmlSerializer.Deserialize(fs);

                foreach (DecorationClass element in Decoration)
                {
                    listBox1.Items.Add("Object = " + element.Object + "   Name = " +   element.Name + "   Weight = " + element.Weight + "   Material = " + element.Material);
                    DecorationList.Add(element);
                }
                fs.Close();

                textBoxInfo.Text = "Deserialization completed successfully\r\n";
            }
            
            catch
            {
                textBoxInfo.Text = "Check the file decoration.xml\r\n";
            }  
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutLabels();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FieldList.Clear();
                if(CheckInput())
                {
                    FieldList.Add(comboBox1.GetItemText(comboBox1.SelectedItem));
                    for (i = 1; i <= TextBoxList.Count; i++)
                       if (TextBoxList[i - 1].Text != "")
                            FieldList.Add(TextBoxList[i - 1].Text);
                    DecorationClass decoration = factory[comboBox1.SelectedIndex].FactoryMethod();
                    decoration.SetValues(FieldList);
                    DecorationList.Add(decoration);
                    listBox1.Items.Add("Object = " + decoration.Object + "   Name = " + decoration.Name + "   Weight = " + +decoration.Weight + "   Material = " + decoration.Material);
                    textBoxInfo.Text = "The object added.\r\n";
                }

            }
            catch 
            {
                textBoxInfo.Text = "The object isn't added.\r\n";
            }
        }
        bool CheckInput()
        {
            if ((textBox1.Text != "") && (textBox2.Text != "") && (textBox3.Text != "") && (textBox4.Text != "") && (textBox5.Text != ""))
                return true;
            else
            {
                textBoxInfo.Text = "Uncorrected input.";
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex != -1)
                {
                    DecorationList.Remove(DecorationList[listBox1.SelectedIndex]);
                    flagDelete = true;
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    flagDelete = false;
                    textBoxInfo.Text = "Object deleted.\r\n";
                }
                else
                    textBoxInfo.Text = "Choose the object.\r\n";
            }
            catch
            {
                textBoxInfo.Text = "Choose the object.\r\n";
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FieldList.Clear();
                if ((!flagEdit) && (!flagDelete))
                {
                    comboBox1.Text = DecorationList[listBox1.SelectedIndex].Object;
                    DecorationList[listBox1.SelectedIndex].GetValues(FieldList);
                    for (i = 0; i < FieldList.Count-1; i++)
                        TextBoxList[i].Text = FieldList[i+1];
               }
            }
            catch
            {
                textBoxInfo.Text = "You didn't choose the object.\r\n";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                if (CheckInput())
                {
                    FieldList.Clear();
                    FieldList.Add(comboBox1.GetItemText(comboBox1.SelectedItem));
                    for (i = 1; i <= TextBoxList.Count; i++)
                        if (TextBoxList[i - 1].Text != "")
                            FieldList.Add(TextBoxList[i - 1].Text);
                    DecorationList[listBox1.SelectedIndex].Object = comboBox1.GetItemText(comboBox1.SelectedItem);
                    DecorationList[listBox1.SelectedIndex].SetValues(FieldList);
                    flagEdit = true;
                    listBox1.Items[listBox1.SelectedIndex] = "Object =" + DecorationList[listBox1.SelectedIndex].Object + "   Name ="
                        + DecorationList[listBox1.SelectedIndex].Name + "   Weight =" + DecorationList[listBox1.SelectedIndex].Weight
                        + "   Material =" + DecorationList[listBox1.SelectedIndex].Material;
                    flagEdit = false;
                    textBoxInfo.Text = "The object edited.\r\n";
                }
                else
                {
                    textBoxInfo.Text = "Check the input values\r\n";
                }
            }
            else
            {
                textBoxInfo.Text = "Add the object to list.\r\n";
            }
        }
    }
}
