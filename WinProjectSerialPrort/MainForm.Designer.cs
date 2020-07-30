﻿namespace WinProjectSerialPrort
{
    partial class MainForm
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
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblResponse = new System.Windows.Forms.Label();
            this.lblState = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.myProgressBar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(411, 319);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 288);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(475, 20);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.Click += new System.EventHandler(this.txtMessage_Click);
            // 
            // lblResponse
            // 
            this.lblResponse.AutoSize = true;
            this.lblResponse.Location = new System.Drawing.Point(12, 327);
            this.lblResponse.Name = "lblResponse";
            this.lblResponse.Size = new System.Drawing.Size(40, 13);
            this.lblResponse.TabIndex = 2;
            this.lblResponse.Text = "Estado";
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblState.Location = new System.Drawing.Point(396, 9);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(38, 13);
            this.lblState.TabIndex = 3;
            this.lblState.Text = "OfLine";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(317, 35);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(76, 22);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPortName
            // 
            this.txtPortName.FormattingEnabled = true;
            this.txtPortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM9",
            "COM9"});
            this.txtPortName.Location = new System.Drawing.Point(399, 35);
            this.txtPortName.Name = "txtPortName";
            this.txtPortName.Size = new System.Drawing.Size(86, 21);
            this.txtPortName.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Serial Communication";
            // 
            // txtChat
            // 
            this.txtChat.Location = new System.Drawing.Point(12, 72);
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(474, 216);
            this.txtChat.TabIndex = 7;
            this.txtChat.Text = "";
            // 
            // myProgressBar
            // 
            this.myProgressBar.Location = new System.Drawing.Point(12, 62);
            this.myProgressBar.Name = "myProgressBar";
            this.myProgressBar.Size = new System.Drawing.Size(474, 10);
            this.myProgressBar.Step = 1;
            this.myProgressBar.TabIndex = 8;
            this.myProgressBar.Value = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 353);
            this.Controls.Add(this.myProgressBar);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPortName);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.lblResponse);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
            this.Name = "MainForm";
            this.Text = "WinChat";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

       
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblResponse;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox txtPortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtChat;
        private System.Windows.Forms.ProgressBar myProgressBar;
    }
}

