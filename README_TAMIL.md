# TAMIL MAGAN TV – BROADCAST PRO (Starter)

இது முதல் கட்ட WPF UI + Playlist starter project.

## தேவைகள்
- Windows 10/11
- Visual Studio 2022 அல்லது அதற்குப் பிந்தைய பதிப்பு
- .NET 10 SDK
- WPF workload

## இயக்குவது
1. ZIP-ஐ Extract செய்யவும்.
2. `TamilMaganBroadcastPro.csproj`-ஐ Visual Studio-வில் திறக்கவும்.
3. NuGet restore முடிந்ததும் Build > Build Solution.
4. F5 அழுத்தி இயக்கவும்.
5. `ADD MEDIA` மூலம் உங்கள் வீடியோ/ஆடியோ கோப்புகளை Playlist-ல் சேர்க்கவும்.

## இந்த கட்டத்தில் உள்ளவை
- கருப்பு + ஊதா நிற Broadcast UI அடித்தளம்
- Multi-preview placeholder பகுதிகள்
- Playlist-ல் பல media files சேர்த்தல்
- Remove, Move Up, Move Down
- Clock மற்றும் status display

## இன்னும் இணைக்கப்படாதவை
- உண்மையான multi-channel video playback
- RTMP / UDP output
- Audio mixer
- CG / logo / ticker overlay
- Scheduling, recording, live input

அடுத்த கட்டத்தில் LibVLCSharp WPF playback engine இணைக்கப்படும். RTMP/UDP output-க்கு FFmpeg-based pipeline வடிவமைக்கப்படும். இந்த starter-ல் அந்த streaming வசதிகள் செயல்படுவதாகக் கூறப்படவில்லை.

## Architecture குறிப்பு
- UI: WPF
- Language: C#
- Playlist model: ObservableCollection
- Playback: Phase 2-ல் LibVLCSharp
- Streaming: Phase 3-ல் FFmpeg process pipeline
- Settings / schedule persistence: பின்னர் SQLite
