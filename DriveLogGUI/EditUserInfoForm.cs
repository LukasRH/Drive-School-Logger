﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DriveLogCode;

namespace DriveLogGUI
{
    public partial class EditUserInfoForm : Form
    {
        public Image ProfilePicture { get; set; } = null;
        private User user = Session.LoggedInUser;
        
        public EditUserInfoForm()
        {
            InitializeComponent();
            UpdateInfo();
        }

        private Point _lastClick;
        
        private readonly Color _correctColor = Color.FromArgb(109, 144, 150);
        private readonly Color _wrongColor = Color.FromArgb(229, 187, 191);
        private readonly Color _neutralColor = Color.FromArgb(200, 212, 225);
        private readonly Color _whitetextColor = Color.FromArgb(251, 251, 251);
        private readonly Color _standardLabelColor = Color.FromArgb(127, 132, 144);
        private readonly Color _slurredLableColor = Color.FromArgb(224, 224, 224);
        private readonly Color _slurredTextBoxColor = Color.FromArgb(240, 240, 240);
        private readonly Color _standardTextBoxColor = Color.FromArgb(200, 212, 225);

        private bool usernameOk = true;
        private bool passwordOk = true;
        private bool verifyPasswordOk = true;
        private bool firstnameOk = true;
        private bool lastnameOk = true;
        private bool phoneOk = true;
        private bool emailOk = true;
        private bool addressOk = true;
        private bool zipOk = true;
        private bool cityOk = true;

        private IUploadHandler uploader;


        private void UpdateInfo()
        {
            usernameBox.Text = user.Username;
            firstnameBox.Text = user.Firstname;
            lastnameBox.Text = user.Lastname;
            phoneBox.Text = user.Phone;
            emailBox.Text = user.Email;
            addressBox.Text = user.Address;
            zipBox.Text = user.Zip;
            cityBox.Text = user.City;

            if (!string.IsNullOrEmpty(user.PicturePath) || user.PicturePath != "")
            {
                pictureBox.Load(user.PicturePath);
            }

            if (user.Sysmin)
            {
                instructorCheckBox.Visible = true;
                instructorCheckBox.Checked = true;
            }

            uploader = new UploadHandler();
        }

        private void verifyPasswordBox_TextChanged(object sender, EventArgs e)
        {
            if (verifyPasswordBox.Text != verifyPasswordBox.defaultText)
                verifyPasswordBox.PasswordChar = '*';
            else
                verifyPasswordBox.PasswordChar = '\0';

            if (verifyPasswordBox.Text == user.Password)
            {
                ChangeEnableStatus(true);
                ChangeBackColorTextBox(verifyPasswordBox, true);
                ChangeLabelAndBoxColor(_standardLabelColor, _standardTextBoxColor);
                ChangeVerifySectionColor(_slurredLableColor, _slurredTextBoxColor);
                this.ActiveControl = null;
            }
            else
            {
                ChangeEnableStatus(false);
                ChangeBackColorTextBox(verifyPasswordBox, false);
                accountDetailsLabel.ForeColor = _slurredLableColor;
                ChangeLabelAndBoxColor(_slurredLableColor, _slurredTextBoxColor);
                ChangeVerifySectionColor(_standardLabelColor, _standardTextBoxColor);
            }
        }

        private void ChangeEnableStatus(bool enabled)
        {
            usernameBox.Enabled = enabled;
            editPasswordBox.Enabled = enabled;
            verifyEditPasswordBox.Enabled = enabled;
            firstnameBox.Enabled = enabled;
            lastnameBox.Enabled = enabled;
            phoneBox.Enabled = enabled;
            emailBox.Enabled = enabled;
            addressBox.Enabled = enabled;
            zipBox.Enabled = enabled;
            cityBox.Enabled = enabled;

            editPictureButton.Enabled = enabled;
            saveChangesButton.Enabled = enabled;
        }

        private void ChangeLabelAndBoxColor(Color lableColor, Color textBoxColor)
        {
            accountDetailsLabel.ForeColor = lableColor;
            personalInformationLabel.ForeColor = lableColor;
            addressLabel.ForeColor = lableColor;

            usernameBox.BackColor = textBoxColor;
            editPasswordBox.BackColor = textBoxColor;
            verifyEditPasswordBox.BackColor = textBoxColor;
            firstnameBox.BackColor = textBoxColor;
            lastnameBox.BackColor = textBoxColor;
            phoneBox.BackColor = textBoxColor;
            emailBox.BackColor = textBoxColor;
            addressBox.BackColor = textBoxColor;
            zipBox.BackColor = textBoxColor;
            cityBox.BackColor = textBoxColor;
        }

        private void ChangeVerifySectionColor(Color lableColor, Color textBoxColor)
        {
            verifyPasswordLabel.ForeColor = lableColor;
            verifyPasswordInfo.ForeColor = lableColor;
            verifyPasswordBox.BackColor = textBoxColor;
            verifyPasswordBox.ForeColor = Color.DarkGray;
        }

        private void verifyPasswordBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void verifyPasswordBox_Leave(object sender, EventArgs e)
        {
            if (verifyPasswordBox.Text == String.Empty)
                verifyPasswordBox.Text = verifyPasswordBox.defaultText;
        }

        private void usernameBox_Leave(object sender, EventArgs e)
        {
            if (usernameBox.Text == user.Username)
                usernameOk = true;
            else
                usernameOk = RegisterVerification.UsernameVerifacation(usernameBox.Text);
            
            DefaultTextBox_Leave(usernameBox, usernameOk);
        }

        private void editPasswordBox_Leave(object sender, EventArgs e)
        {
            DefaultTextBox_Leave(editPasswordBox, passwordOk);
        }

        private void verifyEditPasswordBox_Leave(object sender, EventArgs e)
        {
            if (verifyEditPasswordBox.Text == editPasswordBox.Text)
                verifyPasswordOk = true;

            DefaultTextBox_Leave(verifyEditPasswordBox, verifyPasswordOk);
        }

        private void editPasswordBox_TextChanged(object sender, EventArgs e)
        {
            passwordOk = RegisterVerification.PasswordVertification(editPasswordBox.Text);

            if (editPasswordBox.Text != editPasswordBox.defaultText)
                editPasswordBox.PasswordChar = '*';
            else
                editPasswordBox.PasswordChar = '\0';

            if (passwordOk)
            {
                ChangeBackColorTextBox(editPasswordBox, true);
            }
            else
            {
                ChangeBackColorTextBox(editPasswordBox, false);
            }

            if (verifyEditPasswordBox.Text == editPasswordBox.Text)
                ChangeBackColorTextBox(verifyEditPasswordBox, true);
            else
            {
                ChangeBackColorTextBox(verifyEditPasswordBox, false);
                verifyPasswordOk = false;
            }

            if (editPasswordBox.Text == usernameBox.Text)
            {
                passwordStatusLabel.Text = "Password can not be the same as your username";
                passwordStatusLabel.ForeColor = _wrongColor;
            }
            else
            {
                int strength = RegisterVerification.PasswordStrength(editPasswordBox.Text);

                if (strength == 0)
                    passwordStatusLabel.Text = "";
                else if (strength < 12)
                    ChangeLabelTextAndColor(passwordStatusLabel, "Weak", _wrongColor);
                else if (strength < 22)
                    ChangeLabelTextAndColor(passwordStatusLabel, "Medium", Color.FromArgb(229, 200, 3));
                else
                    ChangeLabelTextAndColor(passwordStatusLabel, "Strong", _correctColor);
            }
        }

        private void verifyEditPasswordBox_TextChanged(object sender, EventArgs e)
        {
            if (verifyEditPasswordBox.Text != verifyEditPasswordBox.defaultText)
                verifyEditPasswordBox.PasswordChar = '*';
            else
                verifyEditPasswordBox.PasswordChar = '\0';

            if (verifyEditPasswordBox.Text == editPasswordBox.Text)
                ChangeBackColorTextBox(verifyEditPasswordBox, true);
            else
            {
                ChangeBackColorTextBox(verifyEditPasswordBox, false);
                verifyPasswordOk = false;
            }
        }

        private void firstnameBox_Leave(object sender, EventArgs e)
        {
            firstnameOk = RegisterVerification.InputOnlyLettersVerification(firstnameBox.Text);
            DefaultTextBox_Leave(firstnameBox, firstnameOk);
        }

        private void lastnameBox_Leave(object sender, EventArgs e)
        {
            lastnameOk = RegisterVerification.InputOnlyLettersVerification(lastnameBox.Text);
            DefaultTextBox_Leave(lastnameBox, lastnameOk);
        }

        private void phoneBox_Leave(object sender, EventArgs e)
        {
            if (phoneBox.Text == user.Phone)
                phoneOk = true;
            else
                phoneOk = RegisterVerification.PhoneVerifacation(phoneBox.Text);

            DefaultTextBox_Leave(phoneBox, phoneOk);
        }

        private void emailBox_Leave(object sender, EventArgs e)
        {
            if (emailBox.Text == user.Email)
                emailOk = true;
            else
                emailOk = RegisterVerification.EmailVerification(emailBox.Text);

            DefaultTextBox_Leave(emailBox, emailOk);
        }

        private void addressBox_Leave(object sender, EventArgs e)
        {
            addressOk = RegisterVerification.AdressVerification(addressBox.Text);
            DefaultTextBox_Leave(addressBox, addressOk);
        }

        private void zipBox_Leave(object sender, EventArgs e)
        {
            zipOk = RegisterVerification.ZipVerifacation(zipBox.Text);
            DefaultTextBox_Leave(zipBox, zipOk);
        }

        private void cityBox_Leave(object sender, EventArgs e)
        {
            cityOk = RegisterVerification.CityVerification(cityBox.Text);

            DefaultTextBox_Leave(cityBox, cityOk);
        }

        private void DefaultTextBox_Leave(TextboxBorderColor textBox, bool verificationOk)
        {
            if (verificationOk)
                ChangeBackColorTextBox(textBox, true);
            else if (textBox.Text == "")
            {
                textBox.Text = textBox.defaultText;
                ChangeBackColorTextBox(textBox, false);
            }
            else
                ChangeBackColorTextBox(textBox, false);
        }

        private void DefaultTextBox_Enter(TextboxBorderColor textBox)
        {
            if (textBox.Text == textBox.DefaultText)
                textBox.Text = "";
        }

        private void ChangeBackColorTextBox(TextboxBorderColor textBox, bool verify)
        {
            if (textBox.Text == textBox.defaultText)
            {
                textBox.BackColor = _neutralColor;
                textBox.ForeColor = Color.DarkGray;
            }
            else if (verify)
            {
                textBox.BackColor = _correctColor;
                textBox.ForeColor = _whitetextColor;
            }
            else
            {
                textBox.BackColor = _wrongColor;
                textBox.ForeColor = Color.Black;
            }
        }

        private void usernameBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void editPasswordBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void verifyEditPasswordBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void firstnameBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void lastnameBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void phoneBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void emailBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void addressBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void zipBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void cityBox_MouseClick(object sender, MouseEventArgs e)
        {
            DefaultTextBox_Enter((TextboxBorderColor)sender);
        }

        private void ChangeLabelTextAndColor(Label label, string text, Color color)
        {
            label.Text = text;
            label.ForeColor = color;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void topPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _lastClick = e.Location;
        }

        private void topPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - _lastClick.X;
                this.Top += e.Y - _lastClick.Y;
            }
        }

        private void editPictureButton_Click(object sender, EventArgs e)
        {
            UploadProfilePicForm uploadPictureForm = new UploadProfilePicForm(this);
            uploadPictureForm.ShowDialog();
            pictureBox.Image = ProfilePicture;
        }

        private void saveChangesButton_Click(object sender, EventArgs e)
        {
            if (!(usernameOk && passwordOk && verifyPasswordOk && firstnameOk && lastnameOk && phoneOk && emailOk && addressOk && zipOk && cityOk))
            {
                CustomMsgBox.Show("Please fix the red boxes before saving", "Failed", CustomMsgBoxIcon.Warrning);
                return;
            }

            bool updateSuccess;
            string picturePath;

            if (uploader.SaveProfilePicture(ProfilePicture, Properties.Settings.Default["PictureUpload"].ToString()) != null)
                picturePath = uploader.SaveProfilePicture(ProfilePicture, Properties.Settings.Default["PictureUpload"].ToString());
            else
                picturePath = user.PicturePath;

            if (editPasswordBox.Text != editPasswordBox.defaultText)
            {
                updateSuccess = MySql.UpdateUser(user.Cpr, firstnameBox.Text, lastnameBox.Text, phoneBox.Text, emailBox.Text, addressBox.Text,
                zipBox.Text, cityBox.Text, usernameBox.Text, editPasswordBox.Text, picturePath, instructorCheckBox.Checked ? "true" : "false");
            }
            else
            {
                updateSuccess = MySql.UpdateUser(user.Cpr, firstnameBox.Text, lastnameBox.Text, phoneBox.Text, emailBox.Text, addressBox.Text,
                zipBox.Text, cityBox.Text, usernameBox.Text, user.Password, picturePath, instructorCheckBox.Checked ? "true" : "false");
            }

            if(updateSuccess)
            {
                CustomMsgBox.Show("You have succesfully updated your profile", "Success", CustomMsgBoxIcon.Complete);
                DataTable user = MySql.GetUserByName(usernameBox.Text);
                Session.LoadUserFromDataTable(user);
                this.Dispose();
            }
            else
            {
                CustomMsgBox.Show("No connection could be made to the database, please try again later", "No Connection", CustomMsgBoxIcon.Error);
            }
        }
    }
}
