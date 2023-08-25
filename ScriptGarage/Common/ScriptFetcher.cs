using Newtonsoft.Json.Linq;
using ScriptGarage.Model;

namespace ScriptGarage.Common
{
    public class ScriptFetcher
    {
        private string _repoUrl = "https://raw.githubusercontent.com/HoRockx2/script-garage/main/script-list.json";
        private HttpClient _http;

        private string _downloadFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScriptGarage");

        public List<Script> Scripts { get; set; } = new List<Script>();

        public event EventHandler ScriptsFetched;

        public ScriptFetcher()
        {
            _http = new HttpClient();
        }

        public void StartPooling()
        {
        }

        public async Task Pooling()
        {
            //var json = await _http.GetStringAsync(_repoUrl);

            var json = "{\"script-list\": [\"hello-echo\"]}";

            //trim new line from json
            json = json.Replace("\n", "");

            var jObj = JObject.Parse(json);

            var scriptFolder = "https://raw.githubusercontent.com/HoRockx2/script-garage/main/scripts/";
            var readmeFileName = "readme.json";

            foreach (var script in jObj["script-list"].ToArray())
            {
                // combine scriptFolder, script and print it
                var scriptUrl = $"{scriptFolder}/{script}/{readmeFileName}";

                //get scriptUrl file and json parse
                var scriptJson = await _http.GetStringAsync(scriptUrl);
                var scriptJObj = JObject.Parse(scriptJson);

                // combine scriptFolder, script and scriptJObj["cmd"] and ".ps1"
                var scriptCmdUrl = $"{scriptFolder}/{script}/{scriptJObj["cmd"]}.ps1";

                // download scriptCmdUrl file and save it to scriptJObj["cmd"] + ".ps1"
                var scriptCmd = await _http.GetStringAsync(scriptCmdUrl);

                string path = Path.Combine(_downloadFolder, script.ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // skpi if file is exist // i think i need to add version check logic
                if (File.Exists(path))
                {
                    continue;
                }

                // save file to _downloadFolder scriptCmd content. and file name is scriptJObj["cmd"] + ".ps1"
                var scriptCmdFileName = Path.Combine(path, $"{scriptJObj["cmd"]}.ps1");
                await File.WriteAllTextAsync(scriptCmdFileName, scriptCmd);

                var scriptReadmeFileName = Path.Combine(path, readmeFileName);
                await File.WriteAllTextAsync(scriptReadmeFileName, scriptJson);

                // add scriptJson to Scripts
                Scripts.Add(scriptJObj.ToObject<Script>());
            }
        }
    }
}