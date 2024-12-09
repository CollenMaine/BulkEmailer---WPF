using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Firebase.Auth;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MimeKit;
using MimeKit.Text;
using System.Windows.Forms;

namespace BulkEmailer___WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "*****************************",
            BasePath = "*******************************"
        };
        private Firebase.Auth.FirebaseConfig authConfig;
        private FirebaseAuthProvider authProvider;
        private IFirebaseClient client;
        private List<User> list;
        private string attachment;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            list = new List<User>();
            client = new FirebaseClient(config);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int testString;

        public int TestString
        {
            get { return testString; }
            set 
            { 
                testString = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private async void btnLoadEmails_Click(object sender, RoutedEventArgs e)
        {
            FirebaseResponse response = await client.GetAsync("Users/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            if (data != null)
            {
                foreach (dynamic item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<User>(((JProperty)item).Value.ToString()));
                }
                CleanList();
                System.Windows.Forms.MessageBox.Show("Data retreived successfully.", "Data retreival", MessageBoxButtons.OK);
            }
            else
                System.Windows.Forms.MessageBox.Show("Retreival unsuccessful.", "Data retreival", MessageBoxButtons.OK);
        }

        private void CleanList()
        {
            List<User> users = new List<User>();
            foreach (var v in list)
            {
                if (!string.IsNullOrEmpty(v.email))
                {
                    if (!users.Contains(v))
                        users.Add(v);
                }
            }
            list.Clear();
            foreach (var s in users)
                list.Add(s);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAttach_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Title = "Select a file";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                attachment = dialog.FileName;
            }
        }

        private async void btnSendEmails_Click(object sender, RoutedEventArgs e)
        {
            foreach (var user in list)
            {
                await SendEmail(user.email, user.name, txtbxTitle.Text, txtbxBody.Text, chkbxIsHTML.IsChecked.Value);
            }
            System.Windows.Forms.MessageBox.Show("Emails sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task SendEmail(string _toEmail, string _toName, string _subject, string _body, bool isHTML, string filePath = "")
        {
            filePath = attachment;
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Cramdown", "dev@cramdown.co.za"));
            email.To.Add(new MailboxAddress(_toName, _toEmail));
            email.Subject = _subject;

            TextPart textPart = new TextPart(isHTML ? TextFormat.Html : TextFormat.Text)
            {
                Text = _body
            };

            if (String.IsNullOrEmpty(filePath))
                email.Body = textPart;

            else
            {
                var multiPart = new Multipart("mixed");
                multiPart.Add(textPart);
                email.Body = multiPart;

                var attachment = new MimePart("image", System.IO.Path.GetExtension(filePath))
                {
                    Content = new MimeContent(File.OpenRead(filePath), ContentEncoding.Default),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = System.IO.Path.GetFileName(filePath)
                };
                multiPart.Add(attachment);
            }

            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                smtp.Connect("***********", 25, false);
                smtp.Authenticate("*********", "*******");
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        }

        private void btnViewEmails_Click(object sender, RoutedEventArgs e)
        {
            if (list != null)
            {
                string s = "";
                foreach (User user in list)
                {
                    s += user.GetDetails() + "\n";
                }
                System.Windows.Forms.MessageBox.Show(s, "User emails", MessageBoxButtons.OK);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("There are no emails loaded.");
            }
        }

        private void btnClearEmails_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            System.Windows.Forms.MessageBox.Show("Emails cleared");
        }
    }
}
