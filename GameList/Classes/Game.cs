using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using GameList.Classes;
using Windows.UI;

namespace GameList.Classes
{
    [DataContract(Name = "Gamez", Namespace = "TestNamespace")]
    public class Game
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public string Platform { get; set; }
        [DataMember]
        public Uri Uri { get; set; }
        [DataMember]
        public bool Owned { get; set; }




        public SolidColorBrush BackgroundColor { get; set; }
        public BitmapImage Image { get; set; }

    }

    
    public class GameManager 
    {

        public static void GetAllGames(ObservableCollection<Game> games)
        {
            var allGames = GetGames();
            games.Clear();
            allGames.ForEach(p => games.Add(p));
            foreach (Game game in games)
            {
                game.Image = new BitmapImage(game.Uri);
            }
        }

        public static List<Game> GetGames()
        {

            var gameList = new List<Game>
            {
                #region Original Collection
               
                new Game() { Number = 1, Name = "Dishonored 2", Year = 2016, Genre = "Action-adventure", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Dishonored-2.jpg"), Image = new BitmapImage() },
                new Game() { Number = 2, Name = "Watch Dogs 2", Year = 2016, Genre = "Action-adventure", Platform = "Playstation 4",
                    Uri = new Uri("ms-appx:///Assets/Watch-Dogs-2.jpg") },
                new Game() { Number = 3, Name = "Civilization VI", Year = 2016, Genre = "Strategy", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Civlization-6.jpg") },
                new Game() { Number = 4, Name = "Steep", Year = 2016, Genre = "Sports", Platform = "Xbox One",
                    Uri = new Uri("ms-appx:///Assets/Steep.jpg") },
                new Game() { Number = 5, Name = "Planet Coaster", Year = 2016, Genre = "Simulation", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Planet-Coaster.jpg") },
                new Game() { Number = 6, Name = "Mafia 3", Year = 2016, Genre = "Action-adventure", Platform = "Playstation 4",
                    Uri = new Uri("ms-appx:///Assets/Mafia-3.png") },
                new Game() { Number = 7, Name = "Rainbow Six Siege", Year = 2015, Genre = "FPS", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Rainbow-Six-Siege.jpg") },
                new Game() { Number = 8, Name = "Astroneer", Year = 2016, Genre = "Sandbox", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Astroneer.png") },
                new Game() { Number = 9, Name = "Rimworld", Year = 2013, Genre = "Simulation", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Rimworld.jpg") },
                new Game() { Number = 10, Name = "Warhammer Vermintide", Year = 2016, Genre = "FPS", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Warhammer-Vermintide.jpg") },
                new Game() { Number = 11, Name = "Just Cause 3", Year = 2015, Genre = "FPS", Platform = "Xbox One",
                    Uri = new Uri("ms-appx:///Assets/Just-Cause-3.jpg") },
                new Game() { Number = 12, Name = "Assassin's Creed Unity", Year = 2014, Genre = "Action-adventure", Platform = "Playstation 4",
                    Uri = new Uri("ms-appx:///Assets/Assassin's-Creed-Unity.jpg") },
                new Game() { Number = 13, Name = "Batman Arkham Origins", Year = 2015, Genre = "Action-adventure", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Batman-Arkham-Origins.jpg") },
                new Game() { Number = 14, Name = "Ghost Recon Wildlands", Year = 2017, Genre = "Action-adventure", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Ghost-Recon-Wildlands.jpg") },
                new Game() { Number = 15, Name = "Resident Evil 7", Year = 2017, Genre = "Horror", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Resident-Evil-7.jpg") },
                new Game() { Number = 16, Name = "Conan Exiles", Year = 2017, Genre = "Survival", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/Conan-Exiles.jpg") },
                new Game() { Number = 17, Name = "For Honor", Year = 2017, Genre = "Action", Platform = "PC",
                    Uri = new Uri("ms-appx:///Assets/For-Honor.jpg") }
                    
                #endregion
                
               
            };
            foreach (Game game in gameList)
            {
                if (game.Owned == true)
                {
                    SolidColorBrush colorBrush = new SolidColorBrush();
                    game.BackgroundColor = new SolidColorBrush(colorBrush.Color = Colors.Green);
                }
                else
                {
                    SolidColorBrush colorBrush = new SolidColorBrush();
                    game.BackgroundColor = new SolidColorBrush(colorBrush.Color = Colors.Red);
                }
            }
            return gameList;
        }
    }
}
