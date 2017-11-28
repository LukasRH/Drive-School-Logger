﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DriveLogCode;
using DriveLogCode.DataAccess;
using DriveLogCode.DesignSchemes;
using DriveLogCode.Objects;
using DriveLogGUI.CustomEventArgs;

namespace DriveLogGUI
{
    public class Appointment : AppointmentStructure
    {
        public Label LabelAppointment;
        public Panel BookedPanelOnLabel;
        public List<Lesson> bookedLessons;
        public DateTime ToTime => StartTime.AddMinutes(AvailableTime * 45);
        public string TimeFormat => GetTime();
        public string DateFormat => GetDate();
        public bool bookedByUser;

        public Appointment(AppointmentStructure appointmentStructure) : base(appointmentStructure)
        {
            bookedLessons = DatabaseParser.GetAllLessonsFromAppointmentID(Id);
            GenerateLabel();

            if (Session.LoggedInUser.Sysmin)
            {
                InstructorLabelData();
            } else 
            {
                UserLabelData();
            }

            SubscribeToEvent();
        }

        private void InstructorLabelData()
        {
            LabelAppointment.Text = InstructorName;
        }


        private void GenerateLabel()
        {
            LabelAppointment = new Label();
            LabelAppointment.Text = "no?";
            LabelAppointment.BackColor = GetColorForLabel(this.LessonType);
            LabelAppointment.TextAlign = ContentAlignment.MiddleCenter;
            LabelAppointment.Font = new Font(new FontFamily("Calibri Light"), 9f, FontStyle.Regular, LabelAppointment.Font.Unit);

            LabelAppointment.ForeColor = ColorScheme.CalendarRed;
            BookedPanelOnLabel = new Panel();
            BookedPanelOnLabel.Height = LabelAppointment.Height / 4;
            BookedPanelOnLabel.Width = LabelAppointment.Width;
            BookedPanelOnLabel.Location = new Point(0, 0);
            BookedPanelOnLabel.BackColor = Color.FromArgb(255, Color.Red);
            BookedPanelOnLabel.Click += (s, e) => label_Clicked(new ApppointmentEventArgs(this));
            BookedPanelOnLabel.Hide();

            LabelAppointment.ForeColor = Color.Black;
            LabelAppointment.Controls.Add(BookedPanelOnLabel);
        }

        private Color GetColorForLabel(string appointmentLessonType)
        {
            if (appointmentLessonType == LessonTypes.Theoretical) {
                return ColorScheme.CalendarBlue;
            }
            if (appointmentLessonType == LessonTypes.Practical) {
                return ColorScheme.CalendarGreen;
            }
            if (appointmentLessonType == LessonTypes.Manoeuvre) {
                return ColorScheme.CalendarYellow;
            }
            if (appointmentLessonType == LessonTypes.Slippery) {
                return ColorScheme.CalendarYellow;
            }
            if (appointmentLessonType == LessonTypes.Other) {
                return ColorScheme.CalendarYellow;
            }

            return ColorScheme.CalendarRed;

        }

        public void AppointmentBooked()
        {
            BookedPanelOnLabel.Show();

        }
        public void UserLabelData()
        {
            LabelAppointment.Text = LessonType;
        }

        public event EventHandler<ApppointmentEventArgs> ClickOnAppointmentTriggered;

        public void SubscribeToEvent()
        {
            LabelAppointment.Click += (s, e) => label_Clicked(new ApppointmentEventArgs(this));
            LabelAppointment.MouseEnter += (s, e) => LabelAppointment.Cursor = Cursors.Hand;
        }

        private void label_Clicked(ApppointmentEventArgs e)
        {
            ClickOnAppointmentTriggered?.Invoke(this, e);
        }
        private string GetTime()
        {
            return $"Time: {FromTimeToTime()}";
        }

        public string FromTimeToTime()
        {
            return
                $"{AddZeroToDates(StartTime.Hour)}:{AddZeroToDates(StartTime.Minute)} - {AddZeroToDates(ToTime.Hour)}:{AddZeroToDates(ToTime.Minute)}";
        }

        private string GetDate()
        {
            return $"Date: {DateShortFormat()}";
        }

        public string DateShortFormat()
        {
            return $"{ToTime.ToShortDateString().Replace('/', '-')}";
        }
        private string AddZeroToDates(int checkString)
        {
            string fixedString;

            if (checkString < 10) {
                fixedString = $"0{checkString}";
                return fixedString;
            }
            return checkString.ToString();
        }
    }
}
