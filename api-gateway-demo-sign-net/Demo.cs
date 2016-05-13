using aliyun_api_gateway_sdk.Constant;
using aliyun_api_gateway_sdk.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aliyun_api_gateway_sdk
{
    class Demo
    {
        private const String appKey = "appKey";
        private const String appSecret = "appSecret";
        private const String host = "http://test.alicloudapi.com";

        static void Main(string[] args)
        {
            doGet();

            doPostForm();
            doPostStream();
            doPostString();

            doPutStream();
            doPutString();


            doDelete();

            doHead();
            Console.Read();

        }

        private static void doGet() {
            String url = "/testquery";

            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            using (HttpWebResponse response = HttpUtil.HttpGet(host + url, appKey, appSecret, 30000, headers, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostForm() {
            String url = "/postform";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_FORM);
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            Dictionary<String, String> formParam = new Dictionary<string, string>();
            formParam.Add("name", "testly");
            using (HttpWebResponse response = HttpUtil.HttpPost(host + url, appKey, appSecret, 30000, headers, formParam, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }


        private static void doPostStream()
        {
            String url = "/poststream";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            byte[] bytesBody = Encoding.UTF8.GetBytes("post bytes body content".ToCharArray());
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(bytesBody));

            using (HttpWebResponse response = HttpUtil.HttpPost(host + url, appKey, appSecret, 30000, headers, bytesBody, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPostString()
        {
            String url = "/poststring";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            String body = "post string body content";
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(body)));

            using (HttpWebResponse response = HttpUtil.HttpPost(host + url, appKey, appSecret, 30000, headers, body, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }


        private static void doPutStream()
        {
            String url = "/putstream";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            byte[] bytesBody = Encoding.UTF8.GetBytes("put bytes body content".ToCharArray());
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(bytesBody));

            using (HttpWebResponse response = HttpUtil.HttpPut(host + url, appKey, appSecret, 30000, headers, bytesBody, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doPutString()
        {
            String url = "/putstring";
            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_TYPE, ContentType.CONTENT_TYPE_STREAM);
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);

            String body = "put string body content";
            headers.Add(HttpHeader.HTTP_HEADER_CONTENT_MD5, MessageDigestUtil.Base64AndMD5(Encoding.UTF8.GetBytes(body)));

            using (HttpWebResponse response = HttpUtil.HttpPut(host + url, appKey, appSecret, 30000, headers, body, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doDelete()
        {
            String url = "/testdelete";

            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            headers.Add("TestDelete", "asfdasewr");
            using (HttpWebResponse response = HttpUtil.HttpDelete(host + url, appKey, appSecret, 30000, headers, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }

        private static void doHead()
        {
            String url = "/testdelete";

            Dictionary<String, String> headers = new Dictionary<string, string>();
            headers.Add(HttpHeader.HTTP_HEADER_ACCEPT, ContentType.CONTENT_TYPE_JSON);
            headers.Add("testhead", "asfdasewr");
            using (HttpWebResponse response = HttpUtil.HttpHead(host + url, appKey, appSecret, 30000, headers, null))
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.Method);
                Console.WriteLine(response.Headers);
                Stream st = response.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                Console.WriteLine(reader.ReadToEnd());
                Console.WriteLine(Constants.LF);

            }
        }
   
    }
}
