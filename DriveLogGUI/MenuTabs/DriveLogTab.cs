﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DriveLogCode;
using DriveLogCode.DataAccess;
using DriveLogCode.Objects;

namespace DriveLogGUI.MenuTabs
{
    public partial class DriveLogTab : UserControl
    {
        public virtual event EventHandler LogOutButtonClick;

        private User _user;
        private List<Panel> driveLogPanelList = new List<Panel>();
        private List<LessonTemplate> templateslist = new List<LessonTemplate>();
        private List<Lesson> lessonslist = new List<Lesson>();
        private Color standartTextColor = Color.FromArgb(127, 132, 144);
        private Color standartTitleColor = Color.FromArgb(67, 72, 84);
        private bool _search;

        public DriveLogTab(User user, bool search = false)
        {
            InitializeComponent();
            driveLogHeaderLabel.Text = "Drive Log: " + user.Fullname;
            _user = user;
            _search = search;
            templateslist = DatabaseParser.GetTemplatesList();
            lessonslist = DatabaseParser.GetScheduledAndCompletedLessonsByUserIdList(user.Id);

            for (int i = 0; i < templateslist.Count; i++)
            {
                GenerateDriveLogPanel(i, templateslist[i]);
            }

            foreach (Panel panel in driveLogPanelList)
            {
                backPanel.Controls.Add(panel);
            }

            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (_search)
            {
                backButton.Enabled = true;
                backButton.Visible = true;
                logoutButton.Enabled = false;
                logoutButton.Visible = false;
            }
        }

        private void GenerateDriveLogPanel(int idx, LessonTemplate template)
        {
            int labelWidth = backPanel.Width - 30;
            int labelHeight = 14;
            bool lessonCompleted = false;
            List<Lesson> lessonquery = new List<Lesson>();

            Panel driveLogPanel = new Panel();
            driveLogPanel.BackColor = Color.White;
            driveLogPanel.BorderStyle = BorderStyle.FixedSingle;

            if (idx == 0)
                driveLogPanel.Location = new Point(6, 13);
            else
            {
                driveLogPanel.Location = new Point
                    (6, driveLogPanelList[idx - 1].Location.Y + driveLogPanelList[idx - 1].Height + 13);
            }

            foreach (Lesson lesson in lessonslist)
            {
                if(lesson.TemplateID == template.Id)
                    lessonquery.Add(lesson);
            }

            if (lessonquery.Count > 0 && lessonquery.Last().Progress == template.Time && lessonquery.Last().Completed)
                lessonCompleted = true;

            Label titleLabel = new Label();
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Location = new Point(5, 5);
            titleLabel.Font = new Font("Myanmar Text", 16F, FontStyle.Regular, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = standartTitleColor;
            titleLabel.Size = new Size(labelWidth, labelHeight + 10);
            titleLabel.Text = template.Title;
            driveLogPanel.Controls.Add(titleLabel);

            Label lessonDescriptionTitleLabel = new Label();
            lessonDescriptionTitleLabel.Location = new Point(1, titleLabel.Height + 10);
            lessonDescriptionTitleLabel.Size = new Size(labelWidth, labelHeight + 8);
            lessonDescriptionTitleLabel.Font = new Font("Myanmar Text", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lessonDescriptionTitleLabel.ForeColor = standartTitleColor;
            lessonDescriptionTitleLabel.Text = "Description";
            driveLogPanel.Controls.Add(lessonDescriptionTitleLabel);

            Label lessonDescriptionLabel = new Label();
            lessonDescriptionLabel.Location = new Point(5, lessonDescriptionTitleLabel.Location.Y + labelHeight + 10);
            lessonDescriptionLabel.MaximumSize = new Size(labelWidth, 0);
            lessonDescriptionLabel.AutoSize = true;
            lessonDescriptionLabel.ForeColor = standartTextColor;
            lessonDescriptionLabel.Text = template.Description;
            driveLogPanel.Controls.Add(lessonDescriptionLabel);

            Label instructorNameTitleLabel = new Label();
            instructorNameTitleLabel.Location = new Point(2, lessonDescriptionLabel.Location.Y + lessonDescriptionLabel.Height + 10);
            instructorNameTitleLabel.ForeColor = standartTitleColor;
            instructorNameTitleLabel.Size = new Size(labelWidth, labelHeight + 8);
            instructorNameTitleLabel.TextAlign = ContentAlignment.TopLeft;
            instructorNameTitleLabel.Font = new Font("Myanmar Text", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
            instructorNameTitleLabel.Text = "Instructor";
            driveLogPanel.Controls.Add(instructorNameTitleLabel);

            Label instructorNameLabel = new Label();
            instructorNameLabel.Location = new Point(5, instructorNameTitleLabel.Location.Y + labelHeight + 10);
            instructorNameLabel.ForeColor = standartTextColor;
            instructorNameLabel.Size = new Size(labelWidth, labelHeight);
            instructorNameLabel.TextAlign = ContentAlignment.TopLeft;
            if (lessonCompleted)
                instructorNameLabel.Text = lessonquery.Last().InstructorFullname;
            else
                instructorNameLabel.Text = "N/A";
            driveLogPanel.Controls.Add(instructorNameLabel);

            Label instructorSignLabel = new Label();
            instructorSignLabel.Location = new Point(backPanel.Width / 4 + 50, instructorNameLabel.Location.Y + 16);
            instructorSignLabel.Size = new Size(140, labelHeight * 4);
            instructorSignLabel.ForeColor = standartTextColor;
            instructorSignLabel.Text = "\n\n____________________\n     Instructor Signature";
            driveLogPanel.Controls.Add(instructorSignLabel);

            Label studentSignLabel = new Label();
            studentSignLabel.Location = new Point(backPanel.Width / 2 + 50, instructorNameLabel.Location.Y + 16);
            studentSignLabel.Size = new Size(140, labelHeight * 4);
            studentSignLabel.ForeColor = standartTextColor;
            studentSignLabel.Text = "\n\n____________________\n      Student Signature";
            driveLogPanel.Controls.Add(studentSignLabel);

            if (lessonCompleted)
            {
                Label dateCompletedLabel = new Label();
                dateCompletedLabel.Location = new Point(630, 12);
                dateCompletedLabel.Size = new Size(195, labelHeight);
                dateCompletedLabel.ForeColor = standartTextColor;
                dateCompletedLabel.Text = "Date Completed: " + lessonquery.Last().EndDate;
                driveLogPanel.Controls.Add(dateCompletedLabel);
                dateCompletedLabel.BringToFront();

                PictureBox checkMarkPictureBox = new PictureBox();
                checkMarkPictureBox.Location = new Point(700, instructorNameTitleLabel.Location.Y);
                checkMarkPictureBox.Size = new Size(90, 90);
                checkMarkPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                checkMarkPictureBox.Image = Properties.Resources.greentick;
                checkMarkPictureBox.BackColor = Color.Transparent;
                driveLogPanel.Controls.Add(checkMarkPictureBox);
                checkMarkPictureBox.BringToFront();

                PictureBox studentSignaturePictureBox = new PictureBox();
                studentSignaturePictureBox.Location = new Point(studentSignLabel.Location.X - 10, studentSignLabel.Location.Y - 15);
                studentSignaturePictureBox.Size = new Size(180, 45);
                studentSignaturePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                studentSignaturePictureBox.Load(_user.SignaturePath);
                studentSignaturePictureBox.BackColor = Color.Transparent;
                driveLogPanel.Controls.Add(studentSignaturePictureBox);
                studentSignaturePictureBox.BringToFront();

                PictureBox instructorSignaturePictureBox = new PictureBox();
                instructorSignaturePictureBox.Location = new Point(instructorSignLabel.Location.X - 10, instructorSignLabel.Location.Y - 15);
                instructorSignaturePictureBox.Size = new Size(180, 45);
                instructorSignaturePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                instructorSignaturePictureBox.Load(lessonquery.Last().InstructorSignaturePath);
                instructorSignaturePictureBox.BackColor = Color.Transparent;
                driveLogPanel.Controls.Add(instructorSignaturePictureBox);
                instructorSignaturePictureBox.BringToFront();
            }

            driveLogPanel.Size = new Size(backPanel.Width - 30,  studentSignLabel.Location.Y + studentSignLabel.Height + 5);
            driveLogPanelList.Add(driveLogPanel);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Find("StudentProfileTab", true).Last().Show();
            this.Dispose();
        }

        private void downloadBtn_Click(object sender, EventArgs e)
        {
            var pdfCreator = new PdfMaker();
            pdfCreator.MakeDriveLog(_user);
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            LogOutButtonClick?.Invoke(this, e);

        }
    }
}
