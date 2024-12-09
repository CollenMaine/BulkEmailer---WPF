using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BulkEmailer___WPF.View.UserControls
{
    public partial class CustomTextbox : UserControl, INotifyPropertyChanged
    {
        //public static readonly DependencyProperty TextProperty =
        //DependencyProperty.Register("Text", typeof(string), typeof(CustomTextbox),
        //                            new PropertyMetadata(string.Empty));

        //public string Text
        //{
        //    get => (string)GetValue(TextProperty);
        //    set => SetValue(TextProperty, value);
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private string placeholder;       
        public string Placeholder
        {
            get { return placeholder; }
            set 
            { 
                placeholder = value;
                OnPropertyChanged();
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomTextbox()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtbxContent.Clear();
            txtbxContent.Focus();
        }

        private void txtbxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtbxContent.Text))
                txtPlaceholder.Visibility = Visibility.Visible;
            else
                txtPlaceholder.Visibility = Visibility.Hidden;
        }

        private void btnClear_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClear.Background = Brushes.Transparent;
        }
    }
}
