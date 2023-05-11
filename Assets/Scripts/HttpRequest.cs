
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

public class HttpRequest {
    private readonly string baseURL;

    public HttpRequest()
    {
        baseURL = "http://localhost:8080";
        //baseURL = "https://school-384409.oa.r.appspot.com";
    }
    public Tuple<long, string> SendDataToServer(string posterName, int roomId, int userId, string fileName, byte[] fileData, string rsc) {
        using (var client = new HttpClient())
        using (var content = new MultipartFormDataContent())
        {
            // Add the posterName, roomId, and userId parameters as form data
            content.Add(new StringContent(posterName), "posterName");
            content.Add(new StringContent(roomId.ToString()), "roomId");
            content.Add(new StringContent(userId.ToString()), "userId");

            // Add the file as form data
            var fileContent = new ByteArrayContent(fileData);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            content.Add(fileContent, "file", fileName);

            // Send the request
            var response = client.PostAsync(baseURL + rsc , content).Result;

            // Read the response body
            var responseBody = response.Content.ReadAsStringAsync().Result;

            // Return the response status code and body as a Tuple
            return Tuple.Create((long)response.StatusCode, responseBody);
        }
    }

    public Tuple<long, string> SendDataToServer(List<KeyValuePair<string, object>> queryParameters, string body, string rsc, string method)
    {
        // Create the URL string with query parameters if there are any
        string url = baseURL + rsc;
        if (queryParameters != null && queryParameters.Count > 0)
        {
            url += "?";
            foreach (KeyValuePair<string, object> kvp in queryParameters)
            {
                url += string.Format("{0}={1}&", kvp.Key, kvp.Value);
            }
            url = url.Remove(url.Length - 1); // Remove the last '&' character
        }

        // Create the HttpWebRequest object with the specified method
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = method;
        request.ContentType = "application/json";
        byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
        int bodyLength = bodyBytes.Length;
        request.ContentLength = bodyLength;
        
        // Add the body content to the request if there is any
        if (!string.IsNullOrEmpty(body))
        {
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(body);
            }
        }

        // Send the request and get the response
        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)request.GetResponse();
        }
        catch (WebException ex)
        {
            if (ex.Response == null)
            {
                return new Tuple<long, string>(0, ex.Message);
            }
            else
            {
                response = (HttpWebResponse)ex.Response;
            }
        }

        // Read the response content
        string responseContent = null;
        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        {
            responseContent = reader.ReadToEnd();
        }

        // Return the response status code and content as a Tuple
        return new Tuple<long, string>((long)response.StatusCode, responseContent);
    }
    
    
}






   

