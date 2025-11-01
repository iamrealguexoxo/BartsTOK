using System;
using System.Linq;
using System.Windows;

namespace BartsTOK
{
    public partial class ScheduleEditorWindow : Window
    {
        public ScheduleEntry Entry { get; set; } = new ScheduleEntry();

        public ScheduleEditorWindow()
        {
            InitializeComponent();
            cbAction.SelectedIndex = 0;
        }

        public void LoadEntry()
        {
            txtName.Text = Entry.Name ?? "";
            txtTime.Text = Entry.Time ?? "00:00";
            if (Entry.Action == "Stop") cbAction.SelectedIndex = 1; else cbAction.SelectedIndex = 0;

            if (string.Equals(Entry.Days, "Everyday", StringComparison.OrdinalIgnoreCase))
            {
                cbEveryday.IsChecked = true;
                cbMon.IsChecked = cbTue.IsChecked = cbWed.IsChecked = cbThu.IsChecked = cbFri.IsChecked = cbSat.IsChecked = cbSun.IsChecked = false;
            }
            else
            {
                cbEveryday.IsChecked = false;
                var parts = (Entry.Days ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();
                cbMon.IsChecked = parts.Contains("Mon");
                cbTue.IsChecked = parts.Contains("Tue");
                cbWed.IsChecked = parts.Contains("Wed");
                cbThu.IsChecked = parts.Contains("Thu");
                cbFri.IsChecked = parts.Contains("Fri");
                cbSat.IsChecked = parts.Contains("Sat");
                cbSun.IsChecked = parts.Contains("Sun");
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Entry.Name = txtName.Text ?? "";
            Entry.Time = txtTime.Text ?? "00:00";
            Entry.Action = (cbAction.SelectedItem is System.Windows.Controls.ComboBoxItem ci) ? ci.Content.ToString() ?? "Start" : "Start";
            Entry.Enabled = true;

            if (cbEveryday.IsChecked == true)
                Entry.Days = "Everyday";
            else
            {
                var days = new System.Collections.Generic.List<string>();
                if (cbMon.IsChecked == true) days.Add("Mon");
                if (cbTue.IsChecked == true) days.Add("Tue");
                if (cbWed.IsChecked == true) days.Add("Wed");
                if (cbThu.IsChecked == true) days.Add("Thu");
                if (cbFri.IsChecked == true) days.Add("Fri");
                if (cbSat.IsChecked == true) days.Add("Sat");
                if (cbSun.IsChecked == true) days.Add("Sun");
                Entry.Days = string.Join(",", days);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
