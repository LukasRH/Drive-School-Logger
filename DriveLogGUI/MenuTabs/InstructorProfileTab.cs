﻿using System;
using System.Drawing;
using System.Windows.Forms;
using DriveLogCode.Objects;
using DriveLogGUI.Windows;

namespace DriveLogGUI.MenuTabs
{
    public partial class InstructorProfileTab : ProfileTab
    {
        private User _user;
        private bool _search;
        internal event SubPageNotification SubPageCreated;
        public InstructorProfileTab(User user, bool search = false)
        {
            InitializeComponent();
            _user = user;
            _search = search;
            UpdateLayout();
            UpdateInfo();

            foreach (Control c in upcommingTestsBackPanel.Controls)
            {
                c.MouseClick += progressBarPanel_Click;

                foreach (Control childControl in c.Controls)
                    childControl.MouseClick += progressBarPanel_Click;
            }
        }

        private void UpdateLayout()
        {
            if(_search)
            {
                backButton.Enabled = true;
                backButton.Visible = true;
                logoutButton.Enabled = false;
                logoutButton.Visible = false;
            }
        }

        /// <summary>
        /// Updates the user information with data from session class
        /// </summary>
        private void UpdateInfo()
        {
            profileHeaderLabel.Text = "Profile: " + _user.Username;
            nameOutputLabel.Text = _user.Fullname;
            phoneOutputLabel.Text = _user.Phone;
            cprOutputLabel.Text = _user.Cpr;
            emailOutputLabel.Text = _user.Email;
            addressOutputLabel.Text = _user.Address;
            cityOutputLabel.Text = $"{_user.City}, {_user.Zip}";

            if (!string.IsNullOrEmpty(_user.PicturePath) || _user.PicturePath != "")
            {
                ProfilePicture.Load(_user.PicturePath);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            EditUserInfoForm editForm = new EditUserInfoForm(_user);

            editForm.ShowDialog(this);
            UpdateInfo();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Find("userSearchTab", false)[0].Show();
            this.Dispose();
        }

        private void progressBarPanel_Click(object sender, EventArgs e)
        {
            if (!_search)
            {
                SubPageCreated?.Invoke(this);
            }
            else
            {
                this.Hide();
                DriveLogTab studentFoundDriveLogTab = new DriveLogTab(_user, true);
                studentFoundDriveLogTab.Location = this.Location;
                studentFoundDriveLogTab.Parent = this;
                this.Parent.Controls.Add(studentFoundDriveLogTab);
                studentFoundDriveLogTab.Show();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
