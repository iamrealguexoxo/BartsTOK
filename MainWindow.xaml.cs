using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BartsTOK
{
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        private const uint KEYEVENTF_KEYUP = 2;

        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        private DispatcherTimer mainTimer;
        private bool isRunning = false;
        private List<string> textLines = new List<string>();
        private int currentLineIndex = 0;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            mainTimer = new DispatcherTimer();
            mainTimer.Tick += MainTimer_Tick;

            this.KeyDown += (s, e) =>
            {
                if (e.Key == Key.F1) BtnStart_Click(null, null);
                if (e.Key == Key.F2) BtnStop_Click(null, null);
            };
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                MessageBox.Show("Bitte geben Sie Text ein!");
                return;
            }

            if (!double.TryParse(txtLinePause.Text, out double linePause) || linePause < 0)
            {
                MessageBox.Show("Ungültige Pause!");
                return;
            }

            textLines = new List<string>(text.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            currentLineIndex = 0;
            isRunning = true;

            mainTimer.Interval = TimeSpan.FromSeconds(linePause);
            mainTimer.Start();

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            lblStatus.Content = "Status: Läuft ▶";
            AddLog("✓ Barts TOK gestartet");
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            isRunning = false;
            mainTimer.Stop();

            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            lblStatus.Content = "Status: Gestoppt ⏹";
            AddLog("✗ Barts TOK gestoppt");
        }

        private void BtnTray_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.WindowState = WindowState.Minimized;
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (!isRunning) return;

            string lineToSend = "";

            if (rbRandom.IsChecked == true)
            {
                lineToSend = textLines[random.Next(textLines.Count)];
            }
            else if (rbSequential.IsChecked == true || rbSequentialStop.IsChecked == true)
            {
                lineToSend = textLines[currentLineIndex];
                currentLineIndex++;

                if (currentLineIndex >= textLines.Count)
                {
                    if (rbSequentialStop.IsChecked == true)
                    {
                        BtnStop_Click(null, null);
                        return;
                    }
                    currentLineIndex = 0;
                }
            }

            SendTextWithPause(lineToSend);

            if (cbAutoNewline.IsChecked == true)
            {
                SendKey(0x0D);
            }

            lblStatus.Content = "Status: Läuft ▶ - " + DateTime.Now.ToString("HH:mm:ss");
        }

        private void SendTextWithPause(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (!double.TryParse(txtCharPause.Text, out double charPause))
                charPause = 0.01;

            foreach (char c in text)
            {
                SendChar(c);
                if (charPause > 0)
                    Thread.Sleep((int)(charPause * 1000));
            }

            AddLog("→ Gesendet: '" + text + "'");
        }

        private void SendChar(char c)
        {
            short vk = VkKeyScan(c);
            byte key = (byte)(vk & 0xFF);
            byte shift = (byte)((vk >> 8) & 0xFF);

            if ((shift & 1) != 0) keybd_event(0x10, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            if ((shift & 1) != 0) keybd_event(0x10, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        private void SendKey(byte key)
        {
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
            Thread.Sleep(50);
        }

        private void AddLog(string message)
        {
            logBox.Items.Insert(0, "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + message);
            if (logBox.Items.Count > 50)
                logBox.Items.RemoveAt(logBox.Items.Count - 1);
        }
    }
}