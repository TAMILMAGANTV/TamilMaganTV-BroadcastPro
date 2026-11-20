
using LibVLCSharp.Shared;
using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace TamilMaganBroadcastPro
{
    public partial class MainWindow : Window
    {
        private readonly DataTable _playlist = new DataTable();

        private LibVLC? _libVlc;
        private MediaPlayer? _mediaPlayer;
        private Media? _currentMedia;

        public MainWindow()
        {
            InitializeComponent();

            _playlist.Columns.Add("NO", typeof(int));
            _playlist.Columns.Add("FILE NAME", typeof(string));
            _playlist.Columns.Add("DURATION", typeof(string));
            _playlist.Columns.Add("STATUS", typeof(string));
            _playlist.Columns.Add("FILE PATH", typeof(string));

            PlaylistGrid.ItemsSource = _playlist.DefaultView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Core.Initialize();

                _libVlc = new LibVLC();
                _mediaPlayer = new MediaPlayer(_libVlc);

                PreviewVideoView.MediaPlayer = _mediaPlayer;

                StatusText.Text = "VIDEO ENGINE READY";
            }
            catch (Exception ex)
            {
                StatusText.Text = "VIDEO ENGINE ERROR";

                MessageBox.Show(
                    "வீடியோ Engine தொடங்க முடியவில்லை.\n\n" + ex.Message,
                    "TAMIL MAGAN TV BROADCAST PRO",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select Video Files",
                Filter = "Video Files|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.ts;*.m4v|All Files|*.*",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    _playlist.Rows.Add(
                        _playlist.Rows.Count + 1,
                        Path.GetFileName(file),
                        "--:--",
                        "READY",
                        file
                    );
                }

                StatusText.Text =
                    dialog.FileNames.Length + " FILE(S) ADDED";
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (PlaylistGrid.SelectedItem is DataRowView selected)
            {
                selected.Row.Delete();

                RenumberPlaylist();

                StatusText.Text = "FILE REMOVED";
            }
            else
            {
                MessageBox.Show(
                    "முதலில் Playlist-ல் ஒரு வீடியோவைத் தேர்வு செய்யுங்கள்.");
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (_libVlc == null || _mediaPlayer == null)
            {
                MessageBox.Show(
                    "Video Engine இன்னும் தயாராகவில்லை. மீண்டும் முயற்சிக்கவும்.");
                return;
            }

            DataRowView? selected =
                PlaylistGrid.SelectedItem as DataRowView;

            if (selected == null && _playlist.DefaultView.Count > 0)
            {
                PlaylistGrid.SelectedIndex = 0;
                selected = PlaylistGrid.SelectedItem as DataRowView;
            }

            if (selected == null)
            {
                MessageBox.Show(
                    "முதலில் ADD FILE மூலம் வீடியோவைச் சேர்க்கவும்.");
                return;
            }

            string filePath =
                Convert.ToString(selected["FILE PATH"]) ?? "";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("வீடியோ கோப்பைக் கண்டுபிடிக்க முடியவில்லை.");
                return;
            }

            try
            {
                _mediaPlayer.Stop();

                _currentMedia?.Dispose();

                _currentMedia = new Media(
                    _libVlc,
                    new Uri(filePath)
                );

                _mediaPlayer.Play(_currentMedia);

                foreach (DataRow row in _playlist.Rows)
                {
                    row["STATUS"] = "READY";
                }

                selected["STATUS"] = "PLAYING";

                StatusText.Text =
                    "PLAYING: " + Path.GetFileName(filePath);
            }
            catch (Exception ex)
            {
                StatusText.Text = "PLAYBACK ERROR";

                MessageBox.Show(
                    "வீடியோவை இயக்க முடியவில்லை.\n\n" + ex.Message);
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
            }

            _currentMedia?.Dispose();
            _currentMedia = null;

            foreach (DataRow row in _playlist.Rows)
            {
                row["STATUS"] = "READY";
            }

            StatusText.Text = "PLAYBACK STOPPED";
        }

        private void RenumberPlaylist()
        {
            for (int i = 0; i < _playlist.Rows.Count; i++)
            {
                _playlist.Rows[i]["NO"] = i + 1;
            }
        }

        private void Window_Closing(
            object? sender,
            System.ComponentModel.CancelEventArgs e)
        {
            _mediaPlayer?.Stop();

            PreviewVideoView.MediaPlayer = null;

            _currentMedia?.Dispose();
            _mediaPlayer?.Dispose();
            _libVlc?.Dispose();
        }
    }
}
