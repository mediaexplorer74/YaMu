using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using Yandex.Music.Api;
using Yandex.Music.Api.Models;
using Yandex.Music.Api.Models.Track;
using Yandex.Music.Client;
using Windows.Storage;

namespace TermStyle
{
    public class AudioPlayer
    {
        private string _title;
        private TimeSpan _time;
        private int TotalPosition = 0;
        private YandexMusicApi _api;
        private List<YTrack> _tracks;
        private int _activeTrack;
        private TimeSpan _currentTime;
        
        //public AudioPlayer(YandexMusicClient api, List<YandexMusicClient> tracks)
        //{
        //    _api = api;
        //    _tracks = tracks;
        //    _activeTrack = 0;
        //    _currentTime = new TimeSpan();
        //}

        public AudioPlayer(YandexMusicApi api, List<YTrack> tracks)
        {
            _api = api;
            _tracks = tracks;
            _activeTrack = 0;
            _currentTime = new TimeSpan();
        }

        public async void Play()
        {
            var track = _tracks[_activeTrack];
            
            Debug.WriteLine("Extracting track...");

            //_api.ExtractTrackToFile(track, "music");

            StorageFolder folder = await KnownFolders.GetFolderForUserAsync(null, KnownFolderId.MusicLibrary);

            //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            //Task.Factory.StartNew(() =>
            //{
            //StorageFolder folder = KnownFolders.MusicLibrary;
            using 
                (AudioFileReader audioFile = new AudioFileReader(
                    /*$"music/{track.GetTrack("1").Title}.mp3"*/
                    folder.Path + "\\" + "some.mp3")
                )
                {
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                        //SetTrack(track.Title, audioFile.TotalTime);

                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {

                            SetCurrentPosition(audioFile.CurrentTime);
                            Show();
                            Thread.Sleep(1000);
                        }

                        Play();
                        Debug.WriteLine("Play...");
                    }
                }
            //});
            
            
            _activeTrack++;
        }

        public void SetTrack(string title, TimeSpan time)
        {
            _title = title;
            _time = time;

            if (_title.Length >= 12)
                _title = $"{_title.Substring(0, 12)}...";
        }

        public void SetCurrentPosition(TimeSpan time)
        {
            _currentTime = time;
            
            var current = time.TotalSeconds;
            var total = 58 / _time.TotalSeconds;
            
            //TotalPosition = (int)((_time.TotalSeconds / 100) * time.TotalSeconds);
            TotalPosition = (int)(total * current);
        }
        
        public void Show()
        {
            Debug.WriteLine("***");
            Debug.WriteLine($":.\t{_title}\t{new string(' ', 30)}\t\t{_currentTime.Minutes}:{_currentTime.Seconds} | {_time.Minutes}:{_time.Seconds}");
            Debug.WriteLine($":.\t|{new string('-', TotalPosition)}>{new string('.', 58 - TotalPosition)}|");
        }
    }
}