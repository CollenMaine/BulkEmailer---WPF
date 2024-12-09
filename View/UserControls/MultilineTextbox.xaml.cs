using System.Windows.Controls;
using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BulkEmailer___WPF.View.UserControls
{
    public partial class MultilineTextbox : UserControl, INotifyPropertyChanged
    {
        public MultilineTextbox()
        {
            InitializeComponent();
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return text; }
            set
            {
                    text = value;
                    OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void txtbxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtbxContent.Text))
                txtPlaceholder.Visibility = Visibility.Visible;
            else
                txtPlaceholder.Visibility = Visibility.Hidden;
        }
    }
}
