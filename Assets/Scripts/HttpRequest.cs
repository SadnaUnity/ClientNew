
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

public class HttpRequest {
    private readonly string baseURL;

    public HttpRequest()
    {
        baseURL = "http://localhost:8080";
    }
    /*public Tuple<long, string> SendDataToServer(List<KeyValuePair<string, object>> queryParameters, string body, string rsc, string method)
    {
        string msg;
        string url;

        url = baseURL + rsc;

        url = AddQueryParams(queryParameters, url);

        // Create UnityWebRequest object
        UnityWebRequest request = new UnityWebRequest(url, method);

        // Set request headers
        request.SetRequestHeader("Content-Type", "application/json");

        // Convert JSON data to byte array
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(body);

        // Set request body
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler.contentType = "application/json";
        
        // Send request
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            // You can do any additional processing here
        }

        // Handle response
        if (request.result != UnityWebRequest.Result.Success)
        {
            msg = "Error: " + request.error;
        }
        else
        {
            msg = request.downloadHandler.text;
        }
        
        //kill them because the make memory
        request.disposeUploadHandlerOnDispose = true;
        request.disposeDownloadHandlerOnDispose = true;
        
        return new Tuple<long, string>(request.responseCode, msg);
    }
    private string AddQueryParams(List<KeyValuePair<string, object>> queryParameters, string url)
    {
        if (queryParameters != null)
        {
            StringBuilder stringBuilder = new StringBuilder(url);
            if (!url.Contains("?")) {
                stringBuilder.Append("?");
            } else if (!url.EndsWith("&")) {
                stringBuilder.Append("&");
            }

            foreach (var pair in queryParameters) {
                if (pair.Value is int) {
                    stringBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(pair.Value.ToString()));
                } else if (pair.Value is string) {
                    stringBuilder.AppendFormat("{0}={1}&", Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(pair.Value.ToString()));
                } else {
                    //Debug.Log("Invalid value type: {0}", pair.Value.GetType().Name);
                }
            }

            if (stringBuilder.Length > 0)
            {
                stringBuilder.Length--;
            }

            return stringBuilder.ToString();
        }
        else
        {
            return url;
        }
        
    }
*/
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






   

