using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TamilMaganBroadcastPro
{
    public partial class MainWindow : Window
    {
        private readonly DataTable playlist = new();
        private DataGrid? playlistGrid;

        public MainWindow()
        {
            InitializeComponent();

            playlist.Columns.Add("NO");
            playlist.Columns.Add("FILE NAME");
            playlist.Columns.Add("DURATION");
            playlist.Columns.Add("STATUS");

            playlistGrid = FindVisualChild<DataGrid>(this);

            if (playlistGrid != null)
            {
                playlistGrid.AutoGenerateColumns = true;
                playlistGrid.ItemsSource = playlist.DefaultView;
            }

            ConnectButtons(this);
        }

        private void ConnectButtons(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button button)
                {
                    switch (button.Content?.ToString())
                    {
                        case "ADD FILE":
                            button.Click += AddFile_Click;
                            break;

                        case "REMOVE":
                            button.Click += Remove_Click;
                            break;

                        case "PLAY":
                            button.Click += Play_Click;
                            break;

                        case "STOP":
                            button.Click += Stop_Click;
                            break;
                    }
                }

                ConnectButtons(child);
            }
        }

        private void AddFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Video Files",
                Filter = "Video Files|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.ts|All Files|*.*",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    playlist.Rows.Add(
                        playlist.Rows.Count + 1,
                        Path.GetFileName(file),
                        "--:--",
                        "READY"
                    );
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (playlistGrid?.SelectedItem is DataRowView selected)
            {
                selected.Row.Delete();

                for (int i = 0; i < playlist.Rows.Count; i++)
                    playlist.Rows[i]["NO"] = i + 1;
            }
            else
            {
                MessageBox.Show("Please select a playlist item.");
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Playlist is ready. Video playback will be added in the next stage.",
                "TAMIL MAGAN TV BROADCAST PRO"
            );
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Playback stop control is ready for integration.");
        }

        private static T? FindVisualChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                    return result;

                var nested = FindVisualChild<T>(child);

                if (nested != null)
                    return nested;
            }

            return null;
        }
    }
}
