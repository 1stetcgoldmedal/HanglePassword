using System.Windows;

namespace HanglePassword
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // SimpleHangulPassword에 정의된 Password 속성을 가져옴
            MessageBox.Show($"입력된 비밀번호: {MyPasswordBox.Password}");
        }
    }
}