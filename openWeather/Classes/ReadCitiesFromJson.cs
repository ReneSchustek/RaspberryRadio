using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OpenWeather.Classes.WebSocket;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenWeather.Classes
{
    public class ReadCitiesFromJson
    {
        #region Models
        private readonly IHubContext<SendOpenWeatherConf> _hubContext;

        #endregion

        #region Constructor
        public ReadCitiesFromJson(IHubContext<SendOpenWeatherConf> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        /// <summary>
        /// Städte aus json-File auslesen -> zur Verfügung gestellt von openWeather
        /// </summary>
        /// <returns>Boolean</returns>
        /// <remarks>Nur beim ersten Laden und nur, wenn Tabelle leer ist</remarks>
        public async Task ReadAsync()
        {

            string fileContent = string.Empty;
            string wsMessage = string.Empty;

            int maxValue = 0;
            string percent = string.Empty;
            int i = 0;

            try
            {
                string fullPath = Path.Combine(Folders.Get("files"), "city.list.json");
                if (File.Exists(fullPath))
                {
                    //Info senden
                    wsMessage = "0,0,0%,Daten werden eingelesen.";
                    await _hubContext.Clients.All.SendAsync("OnOpenWeatherRefresh", wsMessage);

                    //Datei auslesen
                    using (StreamReader reader = new StreamReader(fullPath, Encoding.UTF8)) { fileContent = await reader.ReadToEndAsync(); }
                    XmlDocument xml = JsonConvert.DeserializeXmlNode("{\"root\":" + fileContent + "}", "root");
                    XmlNode root = xml.FirstChild;

                    maxValue = root.ChildNodes.Count;

                    //Websocket-Nachricht senden
                    wsMessage = maxValue + ",0,0%,Es werden " + maxValue + " Städte eingelesen.";
                    await _hubContext.Clients.All.SendAsync("OnOpenWeatherRefresh", wsMessage);

                    if (root.HasChildNodes)
                    {
                        for (i = 0; i < root.ChildNodes.Count; i++)
                        {
                            OpenWeatherCityModel city = new OpenWeatherCityModel();

                            if (root.ChildNodes[i].HasChildNodes)
                            {
                                for (int y = 0; y < root.ChildNodes[i].ChildNodes.Count; y++)
                                {

                                    if (root.ChildNodes[i].ChildNodes[y].Name.ToLower() == "id") { city.CityId = Convert.ToInt32(root.ChildNodes[i].ChildNodes[y].InnerText); }
                                    if (root.ChildNodes[i].ChildNodes[y].Name.ToLower() == "name") { city.Name = root.ChildNodes[i].ChildNodes[y].InnerText; }
                                    if (root.ChildNodes[i].ChildNodes[y].Name.ToLower() == "country") { city.Country = root.ChildNodes[i].ChildNodes[y].InnerText; }

                                    if (root.ChildNodes[i].ChildNodes[y].HasChildNodes)
                                    {
                                        for (int z = 0; z < root.ChildNodes[i].ChildNodes[y].ChildNodes.Count; z++)
                                        {
                                            if (root.ChildNodes[i].ChildNodes[y].ChildNodes[z].Name.ToLower() == "lon") { city.Lon = Decimal.Parse(root.ChildNodes[i].ChildNodes[y].ChildNodes[z].InnerText, CultureInfo.InvariantCulture); }
                                            if (root.ChildNodes[i].ChildNodes[y].ChildNodes[z].Name.ToLower() == "lat") { city.Lat = Decimal.Parse(root.ChildNodes[i].ChildNodes[y].ChildNodes[z].InnerText, CultureInfo.InvariantCulture); }
                                        }
                                    }
                                }
                            }

                            OpenWeatherCityService cityService = new OpenWeatherCityService();
                            int id = await cityService.CreateAsync(city);

                            percent = CalculatePercent(Convert.ToDecimal(maxValue), Convert.ToDecimal(i)).ToString().Replace(",", ".");
                            wsMessage = maxValue + "," + i + "," + percent + "%," + city.Name + " (" + city.Country + ") (Id: " + id + ")";
                            await _hubContext.Clients.All.SendAsync("OnOpenWeatherRefresh", wsMessage);

                            string message = city.Name + " - " + city.Country + " (Id: " + id.ToString() + ") wurde gespeichert.";
                        }
                    }

                    wsMessage = maxValue + ",0,100%,,Das Einlesen wurde fertiggestellt.";
                    await _hubContext.Clients.All.SendAsync("OnOpenWeatherRefresh", wsMessage);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(ex.ToString(), "error");

                wsMessage = maxValue + "," + i + "," + percent + "%,Es ist ein Fehler aufgetreten. Der Vorgang wurde abgebrochen.";
                await _hubContext.Clients.All.SendAsync("OnOpenWeatherRefresh", wsMessage);
            }
        }

        /// <summary>
        /// Berechnet den Prozentsatz
        /// </summary>
        /// <param name="g">Grundwert</param>
        /// <param name="w">Anteil</param>
        /// <returns>Prozentsatz</returns>
        private decimal CalculatePercent(decimal g, decimal w)
        {
            decimal result = w / g;
            result = result * 100;
            result = Math.Round(result, 2);
            return result;
        }
    }
}
