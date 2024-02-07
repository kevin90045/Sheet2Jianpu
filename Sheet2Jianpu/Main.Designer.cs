namespace Sheet2Jianpu
{
    partial class Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_parts = new System.Windows.Forms.ListView();
            this.button_chromatic = new System.Windows.Forms.Button();
            this.button_chord = new System.Windows.Forms.Button();
            this.button_bass = new System.Windows.Forms.Button();
            this.button_convert = new System.Windows.Forms.Button();
            this.button_remove = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.pictureBox_about = new System.Windows.Forms.PictureBox();
            this.button_options = new System.Windows.Forms.Button();
            this.button_alto_horn = new System.Windows.Forms.Button();
            this.button_bariton = new System.Windows.Forms.Button();
            this.button_blues = new System.Windows.Forms.Button();
            this.button_tremolo = new System.Windows.Forms.Button();
            this.button_double_bass = new System.Windows.Forms.Button();
            this.button_soprano_horn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_about)).BeginInit();
            this.SuspendLayout();
            // 
            // listView_parts
            // 
            this.listView_parts.FullRowSelect = true;
            this.listView_parts.GridLines = true;
            this.listView_parts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_parts.HideSelection = false;
            this.listView_parts.Location = new System.Drawing.Point(12, 46);
            this.listView_parts.MultiSelect = false;
            this.listView_parts.Name = "listView_parts";
            this.listView_parts.Size = new System.Drawing.Size(169, 226);
            this.listView_parts.TabIndex = 1;
            this.listView_parts.UseCompatibleStateImageBehavior = false;
            this.listView_parts.View = System.Windows.Forms.View.Details;
            // 
            // button_chromatic
            // 
            this.button_chromatic.Location = new System.Drawing.Point(187, 46);
            this.button_chromatic.Name = "button_chromatic";
            this.button_chromatic.Size = new System.Drawing.Size(75, 23);
            this.button_chromatic.TabIndex = 2;
            this.button_chromatic.Text = "半音階口琴";
            this.button_chromatic.UseVisualStyleBackColor = true;
            this.button_chromatic.Click += new System.EventHandler(this.button_chromatic_Click);
            // 
            // button_chord
            // 
            this.button_chord.Location = new System.Drawing.Point(187, 220);
            this.button_chord.Name = "button_chord";
            this.button_chord.Size = new System.Drawing.Size(75, 23);
            this.button_chord.TabIndex = 4;
            this.button_chord.Text = "和弦口琴";
            this.button_chord.UseVisualStyleBackColor = true;
            this.button_chord.Click += new System.EventHandler(this.button_chord_Click);
            // 
            // button_bass
            // 
            this.button_bass.Location = new System.Drawing.Point(187, 249);
            this.button_bass.Name = "button_bass";
            this.button_bass.Size = new System.Drawing.Size(75, 23);
            this.button_bass.TabIndex = 8;
            this.button_bass.Text = "低音口琴";
            this.button_bass.UseVisualStyleBackColor = true;
            this.button_bass.Click += new System.EventHandler(this.button_bass_Click);
            // 
            // button_convert
            // 
            this.button_convert.Location = new System.Drawing.Point(12, 12);
            this.button_convert.Name = "button_convert";
            this.button_convert.Size = new System.Drawing.Size(75, 23);
            this.button_convert.TabIndex = 0;
            this.button_convert.Text = "開啟並轉換";
            this.button_convert.UseVisualStyleBackColor = true;
            this.button_convert.Click += new System.EventHandler(this.button_convert_Click);
            // 
            // button_remove
            // 
            this.button_remove.BackgroundImage = global::Sheet2Jianpu.Properties.Resources.remove;
            this.button_remove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_remove.Location = new System.Drawing.Point(12, 278);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(26, 23);
            this.button_remove.TabIndex = 13;
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // button_down
            // 
            this.button_down.BackgroundImage = global::Sheet2Jianpu.Properties.Resources.down;
            this.button_down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_down.Location = new System.Drawing.Point(76, 278);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(26, 23);
            this.button_down.TabIndex = 11;
            this.button_down.UseVisualStyleBackColor = true;
            this.button_down.Click += new System.EventHandler(this.button_down_Click);
            // 
            // button_up
            // 
            this.button_up.BackgroundImage = global::Sheet2Jianpu.Properties.Resources.up;
            this.button_up.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_up.Location = new System.Drawing.Point(44, 278);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(26, 23);
            this.button_up.TabIndex = 10;
            this.button_up.UseVisualStyleBackColor = true;
            this.button_up.Click += new System.EventHandler(this.button_up_Click);
            // 
            // pictureBox_about
            // 
            this.pictureBox_about.Image = global::Sheet2Jianpu.Properties.Resources.about;
            this.pictureBox_about.Location = new System.Drawing.Point(240, 12);
            this.pictureBox_about.Name = "pictureBox_about";
            this.pictureBox_about.Size = new System.Drawing.Size(22, 22);
            this.pictureBox_about.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_about.TabIndex = 15;
            this.pictureBox_about.TabStop = false;
            this.pictureBox_about.Click += new System.EventHandler(this.pictureBox_about_Click);
            // 
            // button_options
            // 
            this.button_options.Location = new System.Drawing.Point(97, 11);
            this.button_options.Name = "button_options";
            this.button_options.Size = new System.Drawing.Size(75, 23);
            this.button_options.TabIndex = 16;
            this.button_options.Text = "設定";
            this.button_options.UseVisualStyleBackColor = true;
            this.button_options.Click += new System.EventHandler(this.button_options_Click);
            // 
            // button_alto_horn
            // 
            this.button_alto_horn.Location = new System.Drawing.Point(187, 191);
            this.button_alto_horn.Name = "button_alto_horn";
            this.button_alto_horn.Size = new System.Drawing.Size(75, 23);
            this.button_alto_horn.TabIndex = 7;
            this.button_alto_horn.Text = "中音銅角";
            this.button_alto_horn.UseVisualStyleBackColor = true;
            this.button_alto_horn.Click += new System.EventHandler(this.button_alto_horn_Click);
            // 
            // button_bariton
            // 
            this.button_bariton.Location = new System.Drawing.Point(187, 133);
            this.button_bariton.Name = "button_bariton";
            this.button_bariton.Size = new System.Drawing.Size(75, 23);
            this.button_bariton.TabIndex = 6;
            this.button_bariton.Text = "中音口琴";
            this.button_bariton.UseVisualStyleBackColor = true;
            this.button_bariton.Click += new System.EventHandler(this.button_bariton_Click);
            // 
            // button_blues
            // 
            this.button_blues.Location = new System.Drawing.Point(187, 104);
            this.button_blues.Name = "button_blues";
            this.button_blues.Size = new System.Drawing.Size(75, 23);
            this.button_blues.TabIndex = 9;
            this.button_blues.Text = "十孔口琴";
            this.button_blues.UseVisualStyleBackColor = true;
            this.button_blues.Click += new System.EventHandler(this.button_blues_Click);
            // 
            // button_tremolo
            // 
            this.button_tremolo.Location = new System.Drawing.Point(187, 75);
            this.button_tremolo.Name = "button_tremolo";
            this.button_tremolo.Size = new System.Drawing.Size(75, 23);
            this.button_tremolo.TabIndex = 5;
            this.button_tremolo.Text = "複音口琴";
            this.button_tremolo.UseVisualStyleBackColor = true;
            this.button_tremolo.Click += new System.EventHandler(this.button_tremolo_Click);
            // 
            // button_double_bass
            // 
            this.button_double_bass.Location = new System.Drawing.Point(187, 278);
            this.button_double_bass.Name = "button_double_bass";
            this.button_double_bass.Size = new System.Drawing.Size(75, 23);
            this.button_double_bass.TabIndex = 3;
            this.button_double_bass.Text = "倍低音口琴";
            this.button_double_bass.UseVisualStyleBackColor = true;
            this.button_double_bass.Click += new System.EventHandler(this.button_double_bass_Click);
            // 
            // button_soprano_horn
            // 
            this.button_soprano_horn.Location = new System.Drawing.Point(187, 162);
            this.button_soprano_horn.Name = "button_soprano_horn";
            this.button_soprano_horn.Size = new System.Drawing.Size(75, 23);
            this.button_soprano_horn.TabIndex = 17;
            this.button_soprano_horn.Text = "高音銅角";
            this.button_soprano_horn.UseVisualStyleBackColor = true;
            this.button_soprano_horn.Click += new System.EventHandler(this.button_soprano_horn_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 312);
            this.Controls.Add(this.button_soprano_horn);
            this.Controls.Add(this.button_options);
            this.Controls.Add(this.pictureBox_about);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.button_blues);
            this.Controls.Add(this.button_bass);
            this.Controls.Add(this.button_alto_horn);
            this.Controls.Add(this.button_bariton);
            this.Controls.Add(this.button_tremolo);
            this.Controls.Add(this.button_chord);
            this.Controls.Add(this.button_double_bass);
            this.Controls.Add(this.button_chromatic);
            this.Controls.Add(this.listView_parts);
            this.Controls.Add(this.button_convert);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "Sheet2Jianpu";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_about)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listView_parts;
        private System.Windows.Forms.Button button_chromatic;
        private System.Windows.Forms.Button button_chord;
        private System.Windows.Forms.Button button_bass;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_down;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Button button_convert;
        private System.Windows.Forms.PictureBox pictureBox_about;
        private System.Windows.Forms.Button button_options;
        private System.Windows.Forms.Button button_alto_horn;
        private System.Windows.Forms.Button button_bariton;
        private System.Windows.Forms.Button button_blues;
        private System.Windows.Forms.Button button_tremolo;
        private System.Windows.Forms.Button button_double_bass;
        private System.Windows.Forms.Button button_soprano_horn;
    }
}

