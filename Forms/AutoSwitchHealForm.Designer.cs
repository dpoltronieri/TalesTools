using _4RTools.Utils;
using System.Windows.Forms;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _4RTools.Forms
{
    partial class AutoSwitchHealForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoSwitchHealForm));
            this.txtlessHpPercent = new System.Windows.Forms.NumericUpDown();
            this.labelSP = new System.Windows.Forms.Label();
            this.labelHP = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtlessHpKey = new System.Windows.Forms.TextBox();
            this.txtlessSpKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtlessSpPercent = new System.Windows.Forms.NumericUpDown();
            this.txtmoreSpKey = new System.Windows.Forms.TextBox();
            this.txtmoreHpKey = new System.Windows.Forms.TextBox();
            this.txtmoreSpPercent = new System.Windows.Forms.NumericUpDown();
            this.txtmoreHpPercent = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtitemKey = new System.Windows.Forms.TextBox();
            this.txtskillKey = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.picBoxSP = new System.Windows.Forms.PictureBox();
            this.picBoxHP = new System.Windows.Forms.PictureBox();
            this.txtspPercent = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtnextItemKey = new System.Windows.Forms.TextBox();
            this.txtqtdSkill = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.txtequipDelay = new System.Windows.Forms.NumericUpDown();
            this.txtswitchDelay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.txtlessHpPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtlessSpPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmoreSpPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmoreHpPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtspPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtqtdSkill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtequipDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtswitchDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // txtlessHpPercent
            // 
            this.txtlessHpPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtlessHpPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlessHpPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtlessHpPercent.ForeColor = System.Drawing.Color.White;
            this.txtlessHpPercent.Location = new System.Drawing.Point(51, 12);
            this.txtlessHpPercent.Name = "txtlessHpPercent";
            this.txtlessHpPercent.Size = new System.Drawing.Size(44, 23);
            this.txtlessHpPercent.TabIndex = 39;
            // 
            // labelSP
            // 
            this.labelSP.AutoSize = true;
            this.labelSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelSP.Location = new System.Drawing.Point(97, 39);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(20, 17);
            this.labelSP.TabIndex = 38;
            this.labelSP.Text = "%";
            // 
            // labelHP
            // 
            this.labelHP.AutoSize = true;
            this.labelHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelHP.Location = new System.Drawing.Point(97, 15);
            this.labelHP.Name = "labelHP";
            this.labelHP.Size = new System.Drawing.Size(20, 17);
            this.labelHP.TabIndex = 37;
            this.labelHP.Text = "%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label2.Location = new System.Drawing.Point(211, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Delay";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label1.Location = new System.Drawing.Point(293, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "sec";
            // 
            // txtlessHpKey
            // 
            this.txtlessHpKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtlessHpKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlessHpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtlessHpKey.ForeColor = System.Drawing.Color.White;
            this.txtlessHpKey.Location = new System.Drawing.Point(115, 12);
            this.txtlessHpKey.Name = "txtlessHpKey";
            this.txtlessHpKey.Size = new System.Drawing.Size(50, 23);
            this.txtlessHpKey.TabIndex = 43;
            // 
            // txtlessSpKey
            // 
            this.txtlessSpKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtlessSpKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlessSpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtlessSpKey.ForeColor = System.Drawing.Color.White;
            this.txtlessSpKey.Location = new System.Drawing.Point(115, 37);
            this.txtlessSpKey.Name = "txtlessSpKey";
            this.txtlessSpKey.Size = new System.Drawing.Size(50, 23);
            this.txtlessSpKey.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "HP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "SP";
            // 
            // txtlessSpPercent
            // 
            this.txtlessSpPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtlessSpPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtlessSpPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtlessSpPercent.ForeColor = System.Drawing.Color.White;
            this.txtlessSpPercent.Location = new System.Drawing.Point(51, 37);
            this.txtlessSpPercent.Name = "txtlessSpPercent";
            this.txtlessSpPercent.Size = new System.Drawing.Size(44, 23);
            this.txtlessSpPercent.TabIndex = 40;
            // 
            // txtmoreSpKey
            // 
            this.txtmoreSpKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtmoreSpKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmoreSpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtmoreSpKey.ForeColor = System.Drawing.Color.White;
            this.txtmoreSpKey.Location = new System.Drawing.Point(245, 36);
            this.txtmoreSpKey.Name = "txtmoreSpKey";
            this.txtmoreSpKey.Size = new System.Drawing.Size(50, 23);
            this.txtmoreSpKey.TabIndex = 56;
            // 
            // txtmoreHpKey
            // 
            this.txtmoreHpKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtmoreHpKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmoreHpKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtmoreHpKey.ForeColor = System.Drawing.Color.White;
            this.txtmoreHpKey.Location = new System.Drawing.Point(245, 11);
            this.txtmoreHpKey.Name = "txtmoreHpKey";
            this.txtmoreHpKey.Size = new System.Drawing.Size(50, 23);
            this.txtmoreHpKey.TabIndex = 55;
            // 
            // txtmoreSpPercent
            // 
            this.txtmoreSpPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtmoreSpPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmoreSpPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtmoreSpPercent.ForeColor = System.Drawing.Color.White;
            this.txtmoreSpPercent.Location = new System.Drawing.Point(181, 36);
            this.txtmoreSpPercent.Name = "txtmoreSpPercent";
            this.txtmoreSpPercent.Size = new System.Drawing.Size(44, 23);
            this.txtmoreSpPercent.TabIndex = 54;
            // 
            // txtmoreHpPercent
            // 
            this.txtmoreHpPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtmoreHpPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtmoreHpPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtmoreHpPercent.ForeColor = System.Drawing.Color.White;
            this.txtmoreHpPercent.Location = new System.Drawing.Point(181, 11);
            this.txtmoreHpPercent.Name = "txtmoreHpPercent";
            this.txtmoreHpPercent.Size = new System.Drawing.Size(44, 23);
            this.txtmoreHpPercent.TabIndex = 53;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(227, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 17);
            this.label5.TabIndex = 52;
            this.label5.Text = "%";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label6.Location = new System.Drawing.Point(227, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 17);
            this.label6.TabIndex = 51;
            this.label6.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label7.Location = new System.Drawing.Point(46, -2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 57;
            this.label7.Text = "Abaixo de";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label8.Location = new System.Drawing.Point(178, -2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 58;
            this.label8.Text = "Acima de";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label9.Location = new System.Drawing.Point(120, -2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 59;
            this.label9.Text = "Equipa";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label10.Location = new System.Drawing.Point(250, -2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 60;
            this.label10.Text = "Equipa";
            // 
            // txtitemKey
            // 
            this.txtitemKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtitemKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtitemKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtitemKey.ForeColor = System.Drawing.Color.White;
            this.txtitemKey.Location = new System.Drawing.Point(49, 101);
            this.txtitemKey.Name = "txtitemKey";
            this.txtitemKey.Size = new System.Drawing.Size(45, 23);
            this.txtitemKey.TabIndex = 314;
            // 
            // txtskillKey
            // 
            this.txtskillKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtskillKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtskillKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtskillKey.ForeColor = System.Drawing.Color.White;
            this.txtskillKey.Location = new System.Drawing.Point(138, 101);
            this.txtskillKey.Name = "txtskillKey";
            this.txtskillKey.Size = new System.Drawing.Size(45, 23);
            this.txtskillKey.TabIndex = 316;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 100);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 315;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Tag = "319";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(107, 106);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(19, 11);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 317;
            this.pictureBox4.TabStop = false;
            // 
            // picBoxSP
            // 
            this.picBoxSP.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSP.Image = global::_4RTools.Resources._4RTools.ETCResource.SP;
            this.picBoxSP.Location = new System.Drawing.Point(6, 36);
            this.picBoxSP.Name = "picBoxSP";
            this.picBoxSP.Size = new System.Drawing.Size(25, 25);
            this.picBoxSP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxSP.TabIndex = 35;
            this.picBoxSP.TabStop = false;
            // 
            // picBoxHP
            // 
            this.picBoxHP.BackColor = System.Drawing.Color.Transparent;
            this.picBoxHP.Image = global::_4RTools.Resources._4RTools.ETCResource.HP;
            this.picBoxHP.Location = new System.Drawing.Point(6, 11);
            this.picBoxHP.Name = "picBoxHP";
            this.picBoxHP.Size = new System.Drawing.Size(25, 25);
            this.picBoxHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBoxHP.TabIndex = 34;
            this.picBoxHP.TabStop = false;
            // 
            // txtspPercent
            // 
            this.txtspPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtspPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtspPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtspPercent.ForeColor = System.Drawing.Color.White;
            this.txtspPercent.Location = new System.Drawing.Point(49, 125);
            this.txtspPercent.Name = "txtspPercent";
            this.txtspPercent.Size = new System.Drawing.Size(45, 23);
            this.txtspPercent.TabIndex = 318;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label11.Location = new System.Drawing.Point(46, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 319;
            this.label11.Text = "Masterball";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label12.Location = new System.Drawing.Point(145, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 13);
            this.label12.TabIndex = 320;
            this.label12.Text = "Skill";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label13.Location = new System.Drawing.Point(241, 87);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 323;
            this.label13.Text = "Proximo Pet";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(204, 105);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(19, 11);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 322;
            this.pictureBox2.TabStop = false;
            // 
            // txtnextItemKey
            // 
            this.txtnextItemKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtnextItemKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtnextItemKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtnextItemKey.ForeColor = System.Drawing.Color.White;
            this.txtnextItemKey.Location = new System.Drawing.Point(249, 100);
            this.txtnextItemKey.Name = "txtnextItemKey";
            this.txtnextItemKey.Size = new System.Drawing.Size(45, 23);
            this.txtnextItemKey.TabIndex = 321;
            // 
            // txtqtdSkill
            // 
            this.txtqtdSkill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtqtdSkill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtqtdSkill.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtqtdSkill.ForeColor = System.Drawing.Color.White;
            this.txtqtdSkill.Location = new System.Drawing.Point(138, 125);
            this.txtqtdSkill.Name = "txtqtdSkill";
            this.txtqtdSkill.Size = new System.Drawing.Size(45, 23);
            this.txtqtdSkill.TabIndex = 324;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label14.Location = new System.Drawing.Point(92, 128);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(20, 17);
            this.label14.TabIndex = 325;
            this.label14.Text = "%";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label15.Location = new System.Drawing.Point(182, 130);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 13);
            this.label15.TabIndex = 326;
            this.label15.Text = "vezes";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label16.Location = new System.Drawing.Point(295, 130);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(20, 13);
            this.label16.TabIndex = 329;
            this.label16.Text = "ms";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label17.Location = new System.Drawing.Point(216, 130);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 13);
            this.label17.TabIndex = 328;
            this.label17.Text = "Delay";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 85);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 1);
            this.panel1.TabIndex = 330;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(30, 131);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(21, 13);
            this.label18.TabIndex = 332;
            this.label18.Text = "SP";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Image = global::_4RTools.Resources._4RTools.ETCResource.SP;
            this.pictureBox3.Location = new System.Drawing.Point(7, 125);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(25, 25);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox3.TabIndex = 331;
            this.pictureBox3.TabStop = false;
            // 
            // txtequipDelay
            // 
            this.txtequipDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtequipDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtequipDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtequipDelay.ForeColor = System.Drawing.Color.White;
            this.txtequipDelay.Location = new System.Drawing.Point(245, 61);
            this.txtequipDelay.Name = "txtequipDelay";
            this.txtequipDelay.Size = new System.Drawing.Size(50, 23);
            this.txtequipDelay.TabIndex = 333;
            this.txtequipDelay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtswitchDelay
            // 
            this.txtswitchDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
            this.txtswitchDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtswitchDelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtswitchDelay.ForeColor = System.Drawing.Color.White;
            this.txtswitchDelay.Location = new System.Drawing.Point(249, 125);
            this.txtswitchDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.txtswitchDelay.Name = "txtswitchDelay";
            this.txtswitchDelay.Size = new System.Drawing.Size(45, 23);
            this.txtswitchDelay.TabIndex = 334;
            this.txtswitchDelay.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // AutoSwitchHealForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(45)))), ((int)(((byte)(49)))));
            this.ClientSize = new System.Drawing.Size(313, 152);
            this.Controls.Add(this.txtswitchDelay);
            this.Controls.Add(this.txtequipDelay);
            this.Controls.Add(this.txtspPercent);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtqtdSkill);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtnextItemKey);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtitemKey);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.txtskillKey);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtmoreSpKey);
            this.Controls.Add(this.txtmoreHpKey);
            this.Controls.Add(this.txtmoreSpPercent);
            this.Controls.Add(this.txtmoreHpPercent);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtlessSpKey);
            this.Controls.Add(this.txtlessHpKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtlessSpPercent);
            this.Controls.Add(this.txtlessHpPercent);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelHP);
            this.Controls.Add(this.picBoxSP);
            this.Controls.Add(this.picBoxHP);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(155)))), ((int)(((byte)(164)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoSwitchHealForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "AutopotForm";
            ((System.ComponentModel.ISupportInitialize)(this.txtlessHpPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtlessSpPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmoreSpPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmoreHpPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxHP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtspPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtqtdSkill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtequipDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtswitchDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown txtlessHpPercent;
        private System.Windows.Forms.Label labelSP;
        private System.Windows.Forms.Label labelHP;
        private System.Windows.Forms.PictureBox picBoxSP;
        private System.Windows.Forms.PictureBox picBoxHP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtlessHpKey;
        private System.Windows.Forms.TextBox txtlessSpKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtlessSpPercent;
        private System.Windows.Forms.TextBox txtmoreSpKey;
        private System.Windows.Forms.TextBox txtmoreHpKey;
        private System.Windows.Forms.NumericUpDown txtmoreSpPercent;
        private System.Windows.Forms.NumericUpDown txtmoreHpPercent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtitemKey;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.TextBox txtskillKey;
        private System.Windows.Forms.NumericUpDown txtspPercent;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtnextItemKey;
        private System.Windows.Forms.NumericUpDown txtqtdSkill;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.PictureBox pictureBox3;
        private NumericUpDown txtequipDelay;
        private NumericUpDown txtswitchDelay;
    }
}