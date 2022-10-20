using SolarBlaze.Extensions;
using System.Text.Json;

namespace SolarBlaze.Services.MqttSqliteApi
{
    public class MqttSqliteApiClient
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public MqttSqliteApiClient(string baseUrl, HttpClient client)
        {
            _client = client;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<Response> FetchEventLog(Topic topics, DateTime start, DateTime end, uint limit)
        {
            if (start >= end)
                throw new ArgumentOutOfRangeException(nameof(start), "start must be < end");

            var names = TopicNames(topics);

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"{_baseUrl}/GetEventLog?start={start.ToUnixTimeSeconds()}&end={end.ToUnixTimeSeconds()}&topics={string.Join("%2C", names)}&limit={limit}");

            var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return Response.Error(response.StatusCode, response.ReasonPhrase);

            await using var responseStream = await response.Content.ReadAsStreamAsync();

            var container = await JsonSerializer.DeserializeAsync<TopicsContainer>(responseStream);
            return Response.Ok(response.StatusCode, container!);
        }

        public async Task<bool> HealthCheck()
        {
            try
            {
                var request = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"{_baseUrl}/healthz");

                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return false;

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);

                return await reader.ReadToEndAsync() == "Healthy";
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        #region helpers
        private static IEnumerable<string> TopicNames(Topic topic)
        {
            var flag = 1;
            for (var i = 0; i < 32; i++)
            {
                if (((int)topic & flag) != 0)
                {
                    var str = (Topic)flag switch
                    {
                        Topic.EnergyChargeDay => "lxp/BA12102001/inputs/all/e_chg_day",
                        Topic.EnergyDischargeDay => "lxp/BA12102001/inputs/all/e_dischg_day",
                        Topic.EnergyPVDay => "lxp/BA12102001/inputs/all/e_pv_day",
                        Topic.EnergyPVDay1 => "lxp/BA12102001/inputs/all/e_pv_day_1",
                        Topic.EnergyToGridDay => "lxp/BA12102001/inputs/all/e_to_grid_day",
                        Topic.EnergyToUserDay => "lxp/BA12102001/inputs/all/e_to_user_day",
                        Topic.PowerCharge => "lxp/BA12102001/inputs/all/p_charge",
                        Topic.PowerDischarge => "lxp/BA12102001/inputs/all/p_discharge",
                        Topic.PowerPV => "lxp/BA12102001/inputs/all/p_pv",
                        Topic.PowerPV1 => "lxp/BA12102001/inputs/all/p_pv_1",
                        Topic.PowerToGrid => "lxp/BA12102001/inputs/all/p_to_grid",
                        Topic.PowerToUser => "lxp/BA12102001/inputs/all/p_to_user",
                        Topic.StateOfCharge => "lxp/BA12102001/inputs/all/soc",
                        Topic.StateOfHealth => "lxp/BA12102001/inputs/all/soh",
                        Topic.Status => "lxp/BA12102001/inputs/all/status",
                        Topic.VoltageBattery => "lxp/BA12102001/inputs/all/v_bat",
                        Topic.VoltagePV => "lxp/BA12102001/inputs/all/v_pv",
                        Topic.VoltagePV1 => "lxp/BA12102001/inputs/all/v_pv_1",
                        _ => null,
                    };

                    if (str != null)
                        yield return str;
                }

                flag *= 2;
            }
        }
        #endregion
    }

    [Flags]
    public enum Topic
    {
        None = 0,
        All = ~0,

        EnergyChargeDay = 1,
        EnergyDischargeDay = 2,
        EnergyPVDay = 4,
        EnergyPVDay1 = 8,
        EnergyToGridDay = 16,
        EnergyToUserDay = 32,
        PowerCharge = 64,
        PowerDischarge = 128,
        PowerPV = 256,
        PowerPV1 = 512,
        PowerToGrid = 1024,
        PowerToUser = 2048,
        StateOfCharge = 4096,
        StateOfHealth = 8192,
        Status = 16384,
        VoltageBattery = 32768,
        VoltagePV = 65536,
        VoltagePV1 = 131072
    }
}
