using GameList.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Input;
using Windows.System;







// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameList
{
    public sealed partial class MainPage : Page
    {

        public ObservableCollection<Game> games;
        public ObservableCollection<Game> filteredGames;
        public List<BitmapImage> List;
        public Brush myBrush = null;
        public string uriPath;
        public Game chosenGame;
        public string storageFolder;
        public int yearInt;
       


        public MainPage()
        {
            this.InitializeComponent();



            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string path = System.IO.Path.Combine(folder.Path, @"XmlStorageFile.xml");
            storageFolder = path;
            


            if (File.Exists(path))
            {
                Deserialize(storageFolder);
            }
            else
            {
                games = new ObservableCollection<Game>();
                GameManager.GetAllGames(games);
                Serialize(storageFolder);
            }

            #region Allow this part to see where the app has created its storage file
            //PathTextBox.Visibility = Visibility.Visible;
            //string pathWithoutFile = storageFolder.Substring(0, storageFolder.Length - 18);
            //PathTextBox.Text = pathWithoutFile;
            #endregion

        }
        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var chosenGame = (Game)e.ClickedItem;
            this.chosenGame = chosenGame;

            if (this.chosenGame.Owned == true)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    IsEnabled = true,
                    Content = "What do you want to do",
                    PrimaryButtonText = "I don't own this game",
                    SecondaryButtonText = "Delete the game"
                };
                dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
                dialog.SecondaryButtonClick += Dialog_SecondaryButtonClick;
                await dialog.ShowAsync();
            }
            else
            {
                ContentDialog dialog2 = new ContentDialog()
                {
                    IsEnabled = true,
                    Content = "What do you want to do",
                    PrimaryButtonText = "I own this game",
                    SecondaryButtonText = "Delete the game"
                };
                dialog2.PrimaryButtonClick += Dialog2_PrimaryButtonClick;
                dialog2.SecondaryButtonClick += Dialog2_SecondaryButtonClick;
                await dialog2.ShowAsync();
            }
            
        }
        #region Content Dialogs
        //1
        private void Dialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            games.Remove(chosenGame);
            Serialize(storageFolder);
            MyFrame.Navigate(typeof(MainPage));
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            chosenGame.Owned = false;
            Serialize(storageFolder);
            MyFrame.Navigate(typeof(MainPage));
        }
        //2
        private void Dialog2_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            games.Remove(chosenGame);
            Serialize(storageFolder);
            MyFrame.Navigate(typeof(MainPage));
        }

        private void Dialog2_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            chosenGame.Owned = true;
            Serialize(storageFolder);
            MyFrame.Navigate(typeof(MainPage));
        }
        
        #endregion


        public void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (int.TryParse(YearTextBox.Text, out yearInt))
            {
                yearInt = Int32.Parse(YearTextBox.Text);
            }
            else
            {
                YearTextBox.Text = String.Empty;
            }

            
            if (NameTextBox.Text == String.Empty || YearTextBox.Text == String.Empty || uriPath == String.Empty || PlatformTextBox.Text == String.Empty)
            {
                #region Placeholder Text Changed
                if (NameTextBox.Text == String.Empty)
                {
                    NameTextBox.PlaceholderText = "Please enter a name";
                    NameTextBox.Background = new SolidColorBrush(Colors.Red);
                }
                if (GenreTextBox.Text == String.Empty)
                {
                    GenreTextBox.PlaceholderText = "Please enter a genre";
                    GenreTextBox.Background = new SolidColorBrush(Colors.Red);
                }
                if (PlatformTextBox.Text == String.Empty)
                {
                    PlatformTextBox.PlaceholderText = "Please enter a platform";
                    PlatformTextBox.Background = new SolidColorBrush(Colors.Red);
                }
                if (uriPath == null)
                {
                    DropPanel.Background = new SolidColorBrush(Colors.Red);
                }
                #endregion

                return;
            }
            else
            {
                string name = NameTextBox.Text;
                
                string genre = GenreTextBox.Text;
                string platform = PlatformTextBox.Text;

                Game addedGame = new Game()
                {
                    Number = games.Count + 1,
                    Name = name,
                    Year = yearInt,
                    Genre = genre,
                    Platform = platform,
                    Uri = new Uri(uriPath)
                };
                games.Add(addedGame);
                foreach (Game game in games)
                {
                    game.Image = new BitmapImage(game.Uri);
                }
                Serialize(storageFolder);
                Deserialize(storageFolder);

                NameTextBox.Text = String.Empty;
                YearTextBox.Text = String.Empty;
                GenreTextBox.Text = String.Empty;
                PlatformTextBox.Text = String.Empty;
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            }
        }

        public void Serialize(string storageFolder)
        {
            FileStream writer = new FileStream(storageFolder, FileMode.Create, FileAccess.ReadWrite);
            DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<Game>));
            serializer.WriteObject(writer, games);
            writer.Dispose();
        }

        public void Deserialize(string storageFolder)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<Game>));
            FileStream filestream = new FileStream(storageFolder, FileMode.Open, FileAccess.ReadWrite);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(filestream, new XmlDictionaryReaderQuotas());
            games = (ObservableCollection<Game>)serializer.ReadObject(reader);
            
            foreach (Game game in games)
            {
                game.Image = new BitmapImage(game.Uri);

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
            filestream.Dispose();
        }

        private async void DropPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();

                if (items.Any())
                {
                    var storageFile = (StorageFile)items[0];
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    

                    if (contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/gif" ||
                        contentType == "image/pjpeg" || contentType == "image/bmp")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, 
                            NameCollisionOption.ReplaceExisting);
                        uriPath = newFile.Path;
                        PreviewImage.Source = new BitmapImage(new Uri(uriPath));
                    }
                    else
                    {
                        
                        return;
                    }
                }

            }
        }

        private void DropPanel_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;

            //e.DragUIOverride.Caption = "Your image here";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            //e.DragUIOverride.IsGlyphVisible = true;
        }

        private void SplitViewButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
