namespace webSocketServer
{
    partial class frmWebSocketServer
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

            mWebSocketServer.Abort();

        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panImg = new System.Windows.Forms.Panel();
            this.txtYres = new System.Windows.Forms.TextBox();
            this.txtXres = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // panImg
            // 
            this.panImg.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panImg.Location = new System.Drawing.Point(32, 52);
            this.panImg.Name = "panImg";
            this.panImg.Size = new System.Drawing.Size(386, 337);
            this.panImg.TabIndex = 0;
            this.panImg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panImg_MouseDown);
            this.panImg.MouseEnter += new System.EventHandler(this.panImg_MouseEnter);
            this.panImg.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panImg_MouseMove);
            this.panImg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panImg_MouseUp);
            this.panImg.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panImg_MouseWheel);
            // 
            // txtYres
            // 
            this.txtYres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYres.Location = new System.Drawing.Point(105, 12);
            this.txtYres.Name = "txtYres";
            this.txtYres.Size = new System.Drawing.Size(66, 22);
            this.txtYres.TabIndex = 11;
            this.txtYres.Text = "600";
            // 
            // txtXres
            // 
            this.txtXres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXres.Location = new System.Drawing.Point(30, 12);
            this.txtXres.Name = "txtXres";
            this.txtXres.Size = new System.Drawing.Size(69, 22);
            this.txtXres.TabIndex = 10;
            this.txtXres.Text = "800";
            // 
            // frmWebSocketServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 401);
            this.Controls.Add(this.txtYres);
            this.Controls.Add(this.txtXres);
            this.Controls.Add(this.panImg);
            this.Name = "frmWebSocketServer";
            this.Text = "WebSocket Server";
            this.Load += new System.EventHandler(this.frmWebSocketServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panImg;
        private System.Windows.Forms.TextBox txtYres;
        private System.Windows.Forms.TextBox txtXres;

    }
}

