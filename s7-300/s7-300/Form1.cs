using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using S7.Net;

namespace s7_300
{
    public partial class Form1 : Form
    {
        //创建一个s7-300PLC对象
        Plc s7300=null;
        //状态区-连接状态
        bool status = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //连接PLC
        private void btnLink_Click(object sender, EventArgs e)
        {
            try
            {
                s7300 = new Plc(CpuType.S7300, tbPlcAddress.Text, short.Parse(tbRack.Text), short.Parse(tbSlot.Text));
                s7300.Open();
                status = true;
                pingState.BackColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //断开PLC
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (s7300 != null)
                {
                    s7300.Close();
                    status = false;
                    pingState.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //读取PLC中的数据
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (status)
            {
                try
                {
                    object result = s7300.Read(tbDataAddress.Text);
                    tbReadData.Text = string.Format("{0}", result.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("PLC未连接","警告");
            }
        }

        //往PLC写入数据
        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (status)
            {
                try
                {
                    string writeData = tbWriteData.Text;
                    //根据选定的数据类型变换类型
                    //Bit
                    if (cbDataType.SelectedIndex == 0)
                    {
                        s7300.Write(tbDataAddress.Text, bool.Parse(writeData));
                    }
                    //Byte
                    else if (cbDataType.SelectedIndex == 1)
                    {
                        s7300.Write(tbDataAddress.Text, byte.Parse(writeData));
                    }
                    //Word,16位无符号整数用ushort
                    else if (cbDataType.SelectedIndex == 2)
                    {
                        s7300.Write(tbDataAddress.Text, ushort.Parse(writeData));
                    }
                    //DWord,32位无符号整数用uint
                    else if (cbDataType.SelectedIndex == 3)
                    {
                        s7300.Write(tbDataAddress.Text, uint.Parse(writeData));
                    }
                    //Int,16位整数用short
                    else if (cbDataType.SelectedIndex == 4)
                    {
                        s7300.Write(tbDataAddress.Text,short.Parse(writeData).ConvertToUshort());
                    }
                    //DInt,32位整数用int
                    else if(cbDataType.SelectedIndex==5)
                    {
                        s7300.Write(tbDataAddress.Text,int.Parse(writeData));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("PLC未连接", "警告");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //测试按钮
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (status)
            {
                try
                {
                    //Q2.0=1
                    //byte[] result= { 0,1 };
                    //s7300.WriteBytes(DataType.Output,1,1,result);
                    byte[] result = s7300.ReadBytes(DataType.DataBlock, 1, 2, 2);
                    StringBuilder logMessage = new StringBuilder("读取到的结果是:");
                    foreach(byte data in result)
                    {
                        logMessage = logMessage.AppendFormat(data.ToString()+"  ");
                    }
                    MessageBox.Show(logMessage.ToString());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
