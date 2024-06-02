using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unsplasharp;

namespace FlickrDesktopApp
{
    /// <summary>   
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<ImageModel> Images { get; set; }
        private string AccessKey = "T7oOFF-VuXqwWVRD06TULiWXUHpDqthhdNgAJLDNDMM";
        private int photosPerPage = 12;
        private int page = 1;


        public MainWindow()
        {
            //Components from xaml are initalized
            InitializeComponent();
            Images = new ObservableCollection<ImageModel>();
        }

        private async void photoSearch_Click(object sender, RoutedEventArgs e)
        {
            //LoadInitialImages method is invoke when photoSearch_Click event is subscribed 
            await LoadInitialImages();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Get the source of the image
            var imageSource = ((Image)sender).Source;

            // Create and show the new window
            var imageWindow = new ImageWindow(imageSource);
            imageWindow.Show();
        }

        private async void loadMorePhotos_Click(object sender, RoutedEventArgs e)
        {
            // Load more images when the end of the scroll is reached
            await LoadMoreImages();
        }

        private async Task LoadInitialImages()
        {
            //Page is reset to 1 when new query is passed in the input
            page=1;

            //Previous result is cleared when a new input query is added
            Images.Clear();
            await LoadImages();
        }

        private async Task LoadImages()
        {
            var photosToSearch = inputQuery.Text;
            var client = new UnsplasharpClient(AccessKey);

            //var photoUrl = await client.SearchPhotos(photosToSearch); // Also works

            string url = string.Format("{0}?query={1}&page={2}&per_page={3}&client_id={4}", "https://api.unsplash.com/search/photos", photosToSearch, page, photosPerPage, AccessKey);
            var photoUrl = await client.FetchSearchPhotosList(url);

            for (int i = 0; i < photosPerPage; i++)
            {
                var imagePath = photoUrl[i].Urls.Regular;
                Images.Add(new ImageModel { ImageUrl = imagePath });
            }

            photoListView.ItemsSource = Images;
        }

        private async Task LoadMoreImages()
        {
            page++;
            await LoadImages();
        }
    }

    public class ImageModel
    {
        public string ImageUrl { get; set; }
    }
}
