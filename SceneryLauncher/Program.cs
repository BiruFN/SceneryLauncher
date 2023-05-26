using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SceneryLauncher
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "SceneryLauncher";

            Scenery.WriteLine("Welcome to Scenery");

            Scenery.WriteLine("Please choose a Fortnite location");
            CommonOpenFileDialog CommonOpenFileDialog = new CommonOpenFileDialog();
            CommonOpenFileDialog.Title = "Please choose a Fortnite location where FortniteGame and Engine exist";
            CommonOpenFileDialog.IsFolderPicker = true;
            if (CommonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
            {
                Environment.Exit(0);
            }
            var FileName = CommonOpenFileDialog.FileName;

            Scenery.WriteLine("Please login to Fortnite account");
            var CredentialsToken = Auth.GetCredentialsToken();
            var AccessToken = Auth.GetAccessToken(CredentialsToken);
            var ExchangeCode = Auth.GetExchangeCode(AccessToken);

            Scenery.WriteLine("Launching Fortnite...");

            //Injector Download
            new WebClient().DownloadFile("https://sceneryfn--birufn.repl.co/scenery/api/attachments/Injector.exe", Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "Injector.exe"));
            //Dll Download
            new WebClient().DownloadFile("https://sceneryfn--birufn.repl.co/scenery/api/attachments/v2/Scenery.dll", Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "Scenery.dll"));

            Process Launcher = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "FortniteLauncher.exe"),
                    Arguments = $"-AUTH_LOGIN=unused -AUTH_PASSWORD={ExchangeCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -EpicPortal",
                    UseShellExecute = false
                }
            };
            Launcher.Start();
            foreach (ProcessThread thread in Launcher.Threads)
            {
                Win32.SuspendThread(Win32.OpenThread(0x0002, false, thread.Id));
            }

            Process Shipping_EAC = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping_EAC.exe"),
                    Arguments = $"-AUTH_LOGIN=unused -AUTH_PASSWORD={ExchangeCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -EpicPortal -nobe -noeaceos -fromfl=eac -eac_dir EasyAntiCheat_Kamu -caldera=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiIiwiZ2VuZXJhdGVkIjoxNjg0NzQ2NjU4LCJjYWxkZXJhR3VpZCI6IjI4ODZkZWQwLWVlMGMtNDliZi1iNGZiLTIwZGYxZTVlN2ZhYyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiJkb1JlcXVlc3QgZXJyb3I6IGRvUmVxdWVzdCBmYWlsdXJlIGNvZGU6IDQwMCIsImZhbGxiYWNrIjp0cnVlfQ.fyTdBLpb1eU-lH3dxqSaaK5PxMqcjq14G2wzHHOuXi8",
                    UseShellExecute = false
                }
            };
            Shipping_EAC.Start();
            foreach (ProcessThread thread in Shipping_EAC.Threads)
            {
                Win32.SuspendThread(Win32.OpenThread(0x0002, false, thread.Id));
            }

            Process Shipping = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "FortniteClient-Win64-Shipping.exe"),
                    Arguments = $"-AUTH_LOGIN=unused -AUTH_PASSWORD={ExchangeCode} -AUTH_TYPE=exchangecode -epicapp=Fortnite -epicenv=Prod -EpicPortal -nobe -noeaceos -fromfl=eac -eac_dir EasyAntiCheat_Kamu -caldera=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiIiwiZ2VuZXJhdGVkIjoxNjg0NzQ2NjU4LCJjYWxkZXJhR3VpZCI6IjI4ODZkZWQwLWVlMGMtNDliZi1iNGZiLTIwZGYxZTVlN2ZhYyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiJkb1JlcXVlc3QgZXJyb3I6IGRvUmVxdWVzdCBmYWlsdXJlIGNvZGU6IDQwMCIsImZhbGxiYWNrIjp0cnVlfQ.fyTdBLpb1eU-lH3dxqSaaK5PxMqcjq14G2wzHHOuXi8",
                    UseShellExecute = false
                }
            };
            Shipping.Start();
            Shipping.WaitForInputIdle();

            Process Injector = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "Injector.exe"),
                    Arguments = $"\"{Shipping.Id}\" \"{Path.Combine(FileName, "FortniteGame", "Binaries", "Win64", "Scenery.dll")}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            Injector.Start();
            Injector.WaitForExit();

            Shipping.WaitForExit();
            Shipping_EAC.Kill();
            Launcher.Kill();
        }
    }
}
