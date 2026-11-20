using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TamilMaganBroadcastPro.Models;

public sealed class PlaylistItem : INotifyPropertyChanged
{
    private string _status = "Ready";
    public string FileName { get; set; } = "";
    public string FullPath { get; set; } = "";
    public string FileType { get; set; } = "";
    public string Duration { get; set; } = "--:--:--";
    public string Status
    {
        get => _status;
        set { _status = value; OnPropertyChanged(); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
