using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Yandex.Music.Api;
using Yandex.Music.Api.Models;
using Yandex.Music.Client;

using System.Xml.Serialization; // ?
using YaMu; // ?



using TermStyle;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YaMu
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public string UserFile;
        
        private YandexMusicClient _api;

        public MainPage()
        {
            this.InitializeComponent();
            
        }

        private void Button1Click(object sender, RoutedEventArgs e)
        {
            GetMyFavor();
        }


        //
        // Get Favorites (RnD)
        void GetMyFavor()
        {
            // clear infobox
            InfoBox.Items.Clear();

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            string musicDir = folderPath;// + "\\"+ "YaMu";

            YandexMusicClient api = new YandexMusicClient();

            // handle the login
            bool a = HandleLogin(api);


            // try to login
            bool b = TryLogin();

            Console.WriteLine("Extracting Metallica's popular tracks...");

            // Metallica
            Yandex.Music.Api.Models.Artist.YArtistBriefInfo TTT = api.GetArtist("3121");

            //List<YandexMusicClient> 
            List<Yandex.Music.Api.Models.Track.YTrack> tracks = null;

            try
            {
                // get 10 popular tracks
                tracks = TTT.PopularTracks;
            }
            catch (Exception ex1)
            {
                Debug.WriteLine("Exception: " + ex1.Message);
            }

            // check that all is ok...
            if (tracks != null)
            {
                Int32 index = 1;
                List<string[]> tableData = new List<string[]>();
                Int32 border = 20;

                tracks.ForEach(x =>
                {
                    string[] list = new string[4];

                    string title = x.Title;

                    if (title.Length >= border)
                        title = $"{title.Substring(0, border)}...";

                    string artist = x.Artists.First().Name;

                    if (artist.Length >= 5)
                        artist = $"{artist.Substring(0, 5)}...";

                    string duration = ((long)(x.DurationMs)).ToString();

                    if (duration.Length >= 5)
                        duration = $"{duration.Substring(0, 5)}...";

                    list[0] = index.ToString();
                    list[1] = title;
                    list[2] = artist;
                    list[3] = duration;

                    tableData.Add(list);

                    InfoBox.Items.Add(list[0] + " | " + list[1] + " | " + list[2] + " | " + list[3]);

                    index++;
                });

                //Console.Clear();
                Table table = new Table();
                table.SetHeader("#", "TITLE", "ARTIST", "DURATION");
                table.SetData(tableData);
                table.Show();


                //AudioPlayer audio = new AudioPlayer(api, tracks);

                //audio.Play();
            }
            else
            {
                Debug.WriteLine("No favotite tracks found.");
                InfoBox.Items.Add("No favotite tracks found.");
            }


        }//

   
        // RnD

        public bool HandleLogin(YandexMusicClient api)//(YandexApi api)
        {
            _api = api;
            return true;
        }

        public bool TryLogin()
        {

            UserFile = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) +
            "\\" + "user.xml";

            // check the status of login
            if (!IsLogin())
            {
                Debug.WriteLine("Your need to authorize into Yandex.Music");
                InfoBox.Items.Add("Your need to authorize into Yandex.Music");
                

                var login = Login.Text;
                Debug.Write("login: " + login);
                InfoBox.Items.Add("login: " + login);

                var pass = Password.Text;
                Debug.Write("password: " + pass);

                var user = new User
                {
                    Login = login,
                    Password = pass
                };

                
                Debug.WriteLine($"Authorize...");

                bool isAuth;

                isAuth = _api.Authorize(user.Login, user.Password);

                Debug.WriteLine($"Authorize: {isAuth}");

                /*
                if (isAuth)
                {
                    using (var stream = new FileStream(UserFile, FileMode.Create))
                    {
                        var serializer = new XmlSerializer(typeof(User));

                        serializer.Serialize(stream, user);
                    }
                }
                else
                {
                    //Debug.Clear();
                    Debug.WriteLine("Error pass or login");
                    Login();
                }
                */
            }
            else
            {
                var user = default(User);

                Debug.WriteLine($"Authorize...");

                /*
                using (var stream = new FileStream(UserFile, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(User));

                    user = (User) serializer.Deserialize(stream);
                }
                */

                var isAuth = _api.Authorize(user.Login, user.Password);

                Debug.WriteLine($"Authorize: {isAuth}");
                Debug.WriteLine($"Hello, {user.Login}");
                InfoBox.Items.Add($"Hello, {user.Login}");
            }

            return true;
        }

        private bool IsLogin() => File.Exists(UserFile);

    }//MainPage class end

}//YaMu namespace end
