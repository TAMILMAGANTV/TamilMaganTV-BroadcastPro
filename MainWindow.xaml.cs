using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using TamilMaganBroadcastPro.Models;

namespace TamilMaganBroadcastPro;

public partial class MainWindow : Window
{
    public ObservableCollection<PlaylistItem> Playlist { get; } = new();
    private readonly DispatcherTimer _clockTimer = new() { Interval = TimeSpan.FromSeconds(1) };

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _clockTimer.Tick += (_, _) => ClockText.Text = DateTime.Now.ToString("HH:mm:ss");
        _clockTimer.Start();
    }

    private void AddMedia_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Add media to playlist",
            Filter = "Media files|*.mp4;*.mkv;*.mov;*.avi;*.wmv;*.mp3;*.wav;*.m4v;*.ts|All files|*.*",
            Multiselect = true
        };

        if (dialog.ShowDialog() != true) return;

        foreach (var path in dialog.FileNames)
        {
            Playlist.Add(new PlaylistItem
            {
                FileName = Path.GetFileName(path),
                FullPath = path,
                FileType = Path.GetExtension(path).TrimStart('.').ToUpperInvariant(),
                Status = "Ready"
            });
        }
        StatusText.Text = $"{dialog.FileNames.Length} file(s) added. Total: {Playlist.Count}";
    }

    private void RemoveMedia_Click(object sender, RoutedEventArgs e)
    {
        if (PlaylistGrid.SelectedItem is not PlaylistItem item) return;
        Playlist.Remove(item);
        StatusText.Text = "Selected item removed from playlist.";
    }

    private void MoveUp_Click(object sender, RoutedEventArgs e) => MoveSelected(-1);
    private void MoveDown_Click(object sender, RoutedEventArgs e) => MoveSelected(1);

    private void MoveSelected(int offset)
    {
        if (PlaylistGrid.SelectedItem is not PlaylistItem item) return;
        int index = Playlist.IndexOf(item);
        int target = index + offset;
        if (target < 0 || target >= Playlist.Count) return;
        Playlist.Move(index, target);
        PlaylistGrid.SelectedItem = item;
        PlaylistGrid.ScrollIntoView(item);
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
        if (PlaylistGrid.SelectedItem is not PlaylistItem item)
        {
            StatusText.Text = "Select a playlist item first.";
            return;
        }
        StatusText.Text = $"Selected for playback: {item.FileName}. Playback engine is the next development milestone.";
    }

    private void Pause_Click(object sender, RoutedEventArgs e)
        => StatusText.Text = "Pause control will be connected to the playback engine in Phase 2.";

    private void Stop_Click(object sender, RoutedEventArgs e)
        => StatusText.Text = "Stop control will be connected to the playback engine in Phase 2.";

    private void PlaylistTab_Click(object sender, RoutedEventArgs e)
        => StatusText.Text = "Playlist workspace selected.";

    private void PlaylistGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (PlaylistGrid.SelectedItem is PlaylistItem item)
            StatusText.Text = $"Selected: {item.FileName}";
    }
}
