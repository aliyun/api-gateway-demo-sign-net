using aliyun_api_gateway_sdk.Constant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace aliyun_api_gateway_sdk.Util
{
    public class HttpUtil
    {
        public static HttpWebResponse HttpGet(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, List<String> signHeaderPrefixList) 
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.GET, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.GET, timeout, headers);

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;     
        }

        public static HttpWebResponse HttpDelete(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.DELETE, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.DELETE, timeout, headers);

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }

        public static HttpWebResponse HttpHead(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.HEAD, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.HEAD, timeout, headers);

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }


        public static HttpWebResponse HttpPost(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, Dictionary<String, String> formParam, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.POST, url, formParam, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.POST, timeout, headers);

            if (!(formParam == null || formParam.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in formParam.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, formParam[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, formParam[key]);
                    }
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }

        public static HttpWebResponse HttpPost(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, byte[] data, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.POST, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.POST, timeout, headers);

            if (!(data == null || data.Length == 0))
            {
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }

        public static HttpWebResponse HttpPost(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, String body, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.POST, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.POST, timeout, headers);

            if (!String.IsNullOrEmpty(body))
            {
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(body.ToString());
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }

        public static HttpWebResponse HttpPut(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, byte[] data, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.PUT, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.PUT, timeout, headers);

            if (!(data == null || data.Length == 0))
            {
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }

        public static HttpWebResponse HttpPut(String url, String appKey, String appSecret, int timeout, Dictionary<String, String> headers, String body, List<String> signHeaderPrefixList)
        {
            headers = InitialBasicHeader(headers, appKey, appSecret, HttpMethod.PUT, url, null, signHeaderPrefixList);
            HttpWebRequest httpRequest = InitHttpRequest(url, HttpMethod.PUT, timeout, headers);

            if (!String.IsNullOrEmpty(body))
            {
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(body.ToString());
                    stream.Write(data, 0, data.Length);
                }
            }

            HttpWebResponse httpResponse = GetResponse(httpRequest);
            return httpResponse;
        }


        private static HttpWebResponse GetResponse(HttpWebRequest httpRequest)
        {
            HttpWebResponse httpResponse = null;
            try
            {
                WebResponse response = httpRequest.GetResponse();
                httpResponse = (HttpWebResponse)response;

            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            return httpResponse;
        }

        private static HttpWebRequest InitHttpRequest(String url,String method, int timeout, Dictionary<String, String> headers)
        {
            HttpWebRequest httpRequest = null;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.ServicePoint.Expect100Continue = false;
            httpRequest.Method = method;
            httpRequest.KeepAlive = true;
            httpRequest.Timeout = timeout;

            if (headers.ContainsKey("Accept"))
            {
                httpRequest.Accept = DictionaryUtil.Pop(headers, "Accept");
            }
            if (headers.ContainsKey("Date"))
            {
                httpRequest.Date = Convert.ToDateTime(DictionaryUtil.Pop(headers, "Date"));
            }
            if (headers.ContainsKey(HttpHeader.HTTP_HEADER_CONTENT_TYPE))
            {
                httpRequest.ContentType = DictionaryUtil.Pop(headers, HttpHeader.HTTP_HEADER_CONTENT_TYPE);
            }

            foreach (var header in headers)
            {
                httpRequest.Headers.Add(header.Key, header.Value);
            }
            return httpRequest;
        }


        private static Dictionary<String, String> InitialBasicHeader(Dictionary<String, String> headers, String appKey, String appSecret, String method, String requestAddress, Dictionary<String, String> formParam, List<String> signHeaderPrefixList)
        {
            if (headers == null) {
                headers = new Dictionary<String, String>();
            }

            Uri uri = new Uri(requestAddress);
            StringBuilder stringBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(uri.LocalPath) && !"/".Equals(uri.LocalPath))
            {
                stringBuilder.Append(uri.LocalPath);
            }

            if (!String.IsNullOrEmpty(uri.Query))
            {
                //stringBuilder.Append("?");
                stringBuilder.Append(uri.Query);
            }

            //headers.Add(HttpHeader.HTTP_HEADER_USER_AGENT, Constants.USER_AGENT);
            headers.Add(SystemHeader.X_CA_TIMESTAMP, DateUtil.ConvertDateTimeInt(DateTime.Now).ToString());
            headers.Add(SystemHeader.X_CA_NONCE, Guid.NewGuid().ToString());
            headers.Add(SystemHeader.X_CA_KEY, appKey);
            headers.Add(SystemHeader.X_CA_SIGNATURE, SignUtil.Sign(method, stringBuilder.ToString(),appSecret, headers, formParam, signHeaderPrefixList));

            return headers;
        }


        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
