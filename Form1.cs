using CmlLib.Core;
using System;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.Installer.FabricMC;
using System.Windows.Forms;

namespace Minecraft_Launcher
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }
        
        
        
        private async void Form1_Load(object sender, EventArgs e)
        {
            var path = new MinecraftPath();
            var launcher = new CMLauncher(path);
            var versions = await launcher.GetAllVersionsAsync();
            var fabricVersionLoader = new FabricVersionLoader();
            var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();
            metroComboBox1.Items.Clear();
            foreach (var v in versions)
            {
                metroComboBox1.Items.Add(v.Name);
            }
            foreach (var v in fabricVersions)
            {
                metroComboBox1.Items.Add(v.Name);
            }
           
        }

     

        private async void metroButton2_Click(object sender, EventArgs ev)
        {
            
            Console.WriteLine("다운로드 시작");
            System.Net.ServicePointManager.DefaultConnectionLimit = 256;


            var path = new MinecraftPath();
            var launcher = new CMLauncher(path);

            
            launcher.FileChanged += (e) =>
            {
                
                
            };

            var loginHandler = JELoginHandlerBuilder.BuildDefault();
            Console.WriteLine("login...");

            try
            {
                var session = await loginHandler.Authenticate();
                Console.WriteLine("로그인 성공");
                var fabricVersionLoader = new FabricVersionLoader();

                var fabricVersions = await fabricVersionLoader.GetVersionMetadatasAsync();

                var versionName = metroComboBox1.SelectedItem.ToString();

                var fabric = fabricVersions.GetVersionMetadata(versionName);
                await fabric.SaveAsync(path);

                var process = await launcher.CreateProcessAsync(versionName, new MLaunchOption
                {
                    MaximumRamMb = 4096,
                    Session = session
                });

                process.Start();
                Console.WriteLine("다운로드 완료");

            }
            catch (Exception e)
            {
                MessageBox.Show("마인크래프트 실행 실패\n", "마인크래프트", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }



       
    }
}
