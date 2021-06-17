   public static class RunCommand
    {
        public static string RunCmdAndGetOutput(string cmd)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            process.StartInfo = startInfo;
            process.Start();

            var res = process.StandardOutput.ReadToEnd();

            return res;
        }

        public static List<string> RunChainCmdsAndGetOutput(List<string> commands)
        {
            string cmd = "/C ";
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            //Chain commands together
            for (int i = 0; i < commands.Count - 1; i++)
            {
                cmd += commands[i] + " && ";
            }
            cmd += commands[commands.Count - 1];
            commands.Clear();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                commands.Add(process.StandardOutput.ReadLine());
            }

            return commands;
        }
    }