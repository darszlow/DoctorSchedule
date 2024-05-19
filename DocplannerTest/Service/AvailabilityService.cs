using System.Net.Http.Headers;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using DocplannerTest.DTO;
using System.Text;

namespace DocplannerTest.Service
{
    public class AvailabilityService : IAvailabilityService
    {
        public string token = "Basic dGVjaHVzZXI6c2VjcmV0cGFzc1dvcmQ=";

        public AvailabilityService()
        {

        }

        public async Task<WeeklyAvailabilityDTO> GetWeekly(string dateStr)
        {
            //TO DO move to appsettings
            string url = "https://draliatest.azurewebsites.net/api/availability/GetWeeklyAvailability/"+ dateStr;
            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        if(String.IsNullOrEmpty(responseBody))
                        {
                            throw new Exception("Null response body from Slot service");
                        }

                        WeeklyAvailabilityDTO person = JsonConvert.DeserializeObject<WeeklyAvailabilityDTO>(responseBody);
                        return person;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("WeeklyAvailabilityDTO deserialize error: " + ex.Message+" Response body: "+ responseBody);
                    }   
                }
                catch (HttpRequestException e)
                {
                    throw new Exception("Error while retrieving information about slots. StatusCode"+e.StatusCode+" Message: "+e.Message);
                }
            }
        }

        public async Task TakeSlot(TakeSlotDTO slotData)
        {
            string url = "https://draliatest.azurewebsites.net/api/availability/TakeSlot";

            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(slotData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                request.Headers.Add("Authorization", token);

                try
                {
                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception("Error invoking TakeSlot. Error code: " + response.StatusCode+" "+response.ToString());
                    }
                }
                catch (HttpRequestException e)
                {
                    throw new Exception("Error while TakeSlot. StatusCode" + e.StatusCode + " Message: " + e.Message);
                }
            }
        }
    }
}
