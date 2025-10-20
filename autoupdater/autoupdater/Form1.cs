using System;
using System.Diagnostics; // for updater
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace autoupdater
{
    // main form
    public partial class Form1 : Form
    {
        //current version
        private string localVersion = "1.0";

        // takes version information from GitHub.
        private const string VersionUrl = "https://raw.githubusercontent.com/borashen059/autoupdater/refs/heads/main/version.txt";

        // download url
        private const string DownloadUrl = "please enter your download url...";

        public Form1()
        {
            InitializeComponent();
        }

        // load screen
        private async void Form1_Load(object sender, EventArgs e)
        {

            label1.Text = "Update Checking...";
            await CheckForUpdateAsync();
        }

        // async update control func
        private async Task CheckForUpdateAsync()
        {
            string remoteVersion = ""; //takes the version from server.

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // take version from GitHub
                    string responseText = await client.GetStringAsync(VersionUrl);
                    remoteVersion = responseText.Trim();
                }


                // version control
                if (remoteVersion != localVersion)
                {
                    // ask to user that have new version
                    DialogResult result = MessageBox.Show(
                        $"Have New Version.\n" +
                        $"Current version: {localVersion}\n" +
                        $"New Version: {remoteVersion}\n\n" +
                        $"Do You want to update?",
                        "Update Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        // if yes start update
                        label1.Text = "Update Starting...";
                        StartUpdateProcess(remoteVersion);
                    }
                    else
                    {
                        label1.Text = $"It's out of date, but you chose to continue. (Current version: {localVersion})";
                    }
                }
                else
                {
                    // program is already updated message.
                    label1.Text = "Program is already updated.";
                }
            }
            catch (Exception ex)
            {
                // error catch
                MessageBox.Show($"Update Checking Failed!: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label1.Text = "Update Hadn't done!";
            }
        }

        // starting to update process
        private void StartUpdateProcess(string newVersion)
        {
            try
            {
                // rotate to download page
                Process.Start(DownloadUrl);

                MessageBox.Show(
                    "You have been directed to the download page in your browser. Please close the program and install the new version.",
                    "Downloading...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // close app
                Application.Exit();



            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while starting the update: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}